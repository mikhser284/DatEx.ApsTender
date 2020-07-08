namespace DatEx.ApsTender
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using DatEx.ApsTender.DataModel;
    using DatEx.ApsTender.DataModel.Enums;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using DatEx.ApsTender.Helpers;
    using System.IO;
    using System.Web;

    public partial class ApsClient
    {
        public ApsClient(AppSettings appSettings)
        {
            HttpClient = GetConfiguredRestClient(appSettings);
        }



        /// <summary> Получить данные тендера </summary>
        /// <param name="tenderNo"> № тендера </param>
        public RequestResult<TenderData> GetTenderData(Int32 tenderNo)
        {
            List<String> parameters = new List<string>();
            parameters.AddHttpParameter("Id", tenderNo);
            return GetAsData<RequestResult<TenderData>>("tender/get".AsParametrizedHttpRequest(parameters));
        }



        /// <summary> Получить данные тендеров </summary>
        /// <param name="tendersNumbers"> Список номеров тендеров </param>
        public List<TenderData> GetTendersData(List<Int32> tendersNumbers)
        {
            List<TenderData> tendersData = new List<TenderData>();
            if(tendersNumbers == null || tendersNumbers.Count == 0) return tendersData;
            Task<HttpResponseMessage>[] requests = new Task<HttpResponseMessage>[tendersNumbers.Count];
            for(Int32 i = 0; i < tendersNumbers.Count; i++)
            {
                Int32 tenderNo = tendersNumbers[i];
                requests[i] = HttpClient.GetAsync($"tender/get?id={tenderNo}");
            }
            Task.WaitAll(requests);
            for(Int32 i = 0; i < tendersNumbers.Count; i++)
            {
                HttpResponseMessage response = requests[i].Result;
                response.EnsureSuccessStatusCode();
                String result = response.Content.ReadAsStringAsync().Result;
                result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
                result = JToken.Parse(result).ToString(Newtonsoft.Json.Formatting.Indented);
#endif
                RequestResult<TenderData> deserializedObj = JsonConvert.DeserializeObject<RequestResult<TenderData>>(result);
                if(deserializedObj == null || deserializedObj.IsSuccess != true) continue;
                tendersData.Add(deserializedObj.Data);
            }
            return tendersData;
        }



        /// <summary> Получить коммерческие предложения по текущему туру тендера </summary>
        /// <param name="tenderData"> Данные тендера </param>
        public List<TenderLotItemOffer> GetTenderRoundOffers(TenderData tenderData)
        {
            List<Task> tasks = new List<Task>();
            List<TenderLotItemOffer> allOffers = new List<TenderLotItemOffer>();
            foreach(TenderLot lot in tenderData.TenderLots)
                foreach(TenderLotItem lotItem in lot.LotItems)
                {
                    Task<HttpResponseMessage> requestResult = HttpClient.GetAsync($"tender/getoffers?uuid={lotItem.TenderItemUuid.ToString().ToUpper()}");
                    TenderLot tenderLot = lot;
                    TenderLotItem tenderLotItem = lotItem;
                    tasks.Add(requestResult.ContinueWith(x => FinishInitialization(x, tenderLot, tenderLotItem, allOffers)));
                }
            Task.WaitAll(tasks.ToArray());

            static void FinishInitialization(Task<HttpResponseMessage> requestResult, TenderLot lot, TenderLotItem lotItem, List<TenderLotItemOffer> allOffers)
            {
                HttpResponseMessage response = requestResult.Result;
                response.EnsureSuccessStatusCode();
                String result = response.Content.ReadAsStringAsync().Result;
                result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
                result = JToken.Parse(result).ToString(Formatting.Indented);                
#endif
                List<TenderLotItemOffer> offers = null;
                try
                {
                    offers = JsonConvert.DeserializeObject<List<TenderLotItemOffer>>(result);
                    foreach(var item in offers)
                    {
                        foreach(var answer in item.TenderCriteriaAnswers)
                        {
                            answer.TenderLotItemUuid = lotItem.TenderItemUuid;
                            answer.TenderLotItemName = lotItem.Name;
                            lotItem.Offers.Clear();
                            lotItem.Offers.AddRange(offers);
                            answer.TenderLotNo = lot.LotNumber;
                            answer.SupplierId = item.SupplierId;
                            answer.SupplierName = item.SupplierName;
                            answer.SupplierEdrpou = item.Edrpou;
                            if(!String.IsNullOrEmpty(answer.FileUrl))
                            {
                                String queryValue = HttpUtility.ParseQueryString(new Uri(answer.FileUrl).Query).GetValues("uuid").FirstOrDefault();
                                answer.FileUuid = String.IsNullOrEmpty(queryValue) ? null : ((Guid?)new Guid(queryValue));
                            }
                        }
                    }
                    allOffers.AddRange(offers);
                }
                catch(JsonSerializationException ex)
                {
                    var requestRes = JsonConvert.DeserializeObject<RequestResult<List<TenderLotItemOffer>>>(result);
                    if(requestRes.IsSuccess == false) offers = new List<TenderLotItemOffer>();
                }
            }
            
            return allOffers;
        }



        /// <summary> Получить перечень тендеров которые находятся на указанной стадии </summary>
        /// <param name="stageOfTenderProcess"> Стадия тендерного процесса </param>
        /// <param name="itemsPerPage"> Количество элементов на одну страницу </param>
        public TendersList_RequestResult GetTendersOnStage(ETenderProcessStage? stageOfTenderProcess = null, Int32 itemsPerPage = 10_000)
        {
            List<String> parameters = new List<string>();
            parameters.AddHttpParameter("process", stageOfTenderProcess);
            parameters.AddHttpParameter("limit", itemsPerPage);
            //
            return GetAsData<TendersList_RequestResult>("tender/listProcess".AsParametrizedHttpRequest(parameters));
        }


        
        /// <summary> Получить тендера и их текущее состояние </summary>
        /// <param name="itemsPerPage"> Количество элементов на одну страницу </param>
        /// <returns></returns>
        public List<TenderState> GetTendersAndTheirStates(Int32 itemsPerPage = 10_000)
        {
            List<TenderState> tendersAndStates = new List<TenderState>();
            Task<HttpResponseMessage>[] requests = new Task<HttpResponseMessage>[10];
            for (Int32 i = 0; i <= 9; i++)
            {
                Int32 processStage = i;
                requests[i] = HttpClient.GetAsync($"tender/listProcess?process={processStage}&limit={itemsPerPage}");
            }
            Task.WaitAll(requests);
            for (Int32 i = 0; i <= 9; i++)
            {
                HttpResponseMessage response = requests[i].Result;
                response.EnsureSuccessStatusCode();
                String result = response.Content.ReadAsStringAsync().Result;
                result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
                result = JToken.Parse(result).ToString(Newtonsoft.Json.Formatting.Indented);
#endif
                var deserializedObj = JsonConvert.DeserializeObject<TendersList_RequestResult>(result);
                if (deserializedObj?.Data == null) continue;
                tendersAndStates.AddRange(deserializedObj.Data.Select(x => new TenderState(x.TenderId, i)));
            }
            tendersAndStates = tendersAndStates.OrderBy(x => x.TenderNo).ToList();
            return tendersAndStates;
        }



        /// <summary> Получить информацию о текущей стадии (состоянии) тендеров </summary>
        /// <param name="tendersNumbers"> Номера тендеров </param>
        public List<TenderStageInfo> GetTendersStageInfo(List<Int32> tendersNumbers)
        {
            List<TenderStageInfo> tendersStageInfo = new List<TenderStageInfo>();
            if (tendersNumbers == null || tendersNumbers.Count == 0) return tendersStageInfo;
            Task<HttpResponseMessage>[] requests = new Task<HttpResponseMessage>[tendersNumbers.Count];
            for (Int32 i = 0; i < tendersNumbers.Count; i++)
            {
                Int32 tenderNo = tendersNumbers[i];
                requests[i] = HttpClient.GetAsync($"tender/getStatus?id={tenderNo}");
            }
            Task.WaitAll(requests);
            for (Int32 i = 0; i < tendersNumbers.Count; i++)
            {
                HttpResponseMessage response = requests[i].Result;
                response.EnsureSuccessStatusCode();
                String result = response.Content.ReadAsStringAsync().Result;
                result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
                result = JToken.Parse(result).ToString(Formatting.Indented);
#endif
                RequestInfo_TenderStageInfo deserializedObj = JsonConvert.DeserializeObject<RequestInfo_TenderStageInfo>(result);
                if (deserializedObj?.RequestData == null) continue;
                tendersStageInfo.Add(deserializedObj.RequestData);
            }
            return tendersStageInfo;
        }



        /// <summary> Получить информацию о текущей стадии (состоянии) тендера </summary>
        /// <param name="tendersNumbers"> Номера тендеров </param>
        public TenderStageInfo GetTenderStageInfo(Int32 tenderNo)
        {
            HttpResponseMessage response = HttpClient.GetAsync($"tender/getStatus?id={tenderNo}").Result;
            response.EnsureSuccessStatusCode();
            String result = response.Content.ReadAsStringAsync().Result;
            result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
            result = JToken.Parse(result).ToString(Newtonsoft.Json.Formatting.Indented);
#endif
            RequestInfo_TenderStageInfo deserializedObj = JsonConvert.DeserializeObject<RequestInfo_TenderStageInfo>(result);
            return deserializedObj?.RequestData;
        }


        
        /// <summary> Пропустить задачу </summary>
        /// <param name="tenderData"> Данные тендера </param>
        /// <param name="task"> Пропускаемая задача </param>
        public TenderStageInfo SkipTask(TenderData tenderData, SkippableTask task)
        {
            switch(task)
            {
                case SkippableTask.St6_ConclusionOfSecurityService:
                    return SkipTask(tenderData, ETenderProcessStage.St6_OffersProcessingApprovement);
                case SkippableTask.St8_ApprovementOfTenderCommittee:
                    return SkipTask(tenderData, ETenderProcessStage.St8_SolutionApprovement);
                default: throw new NotImplementedException("Unknown enum value");
            }
        }


        


        public async Task<Byte[]> GetFile(Guid fileUuid) => await HttpClient.GetByteArrayAsync($"tender/getfile?uuid={fileUuid.ToString().ToUpper()}");

        public async void SaveFileAs(Guid fileUuid, String filePath)
        {
            Byte[] fileBody = await GetFile(fileUuid);
            File.WriteAllBytes(filePath, fileBody);
        }

        public async Task<Byte[]> GetCommertialOfferFile(TenderCriteriaAnswer tenderCriteriaAnswer)
        {
            return await HttpClient.GetByteArrayAsync($"file/get?uuid={tenderCriteriaAnswer.FileUuid.ToString().ToUpper()}");
        }

        public void GetFilesFromCommertialOffers(TenderData tenderData, String workDirectory)
        {
            List<TenderCriteriaAnswer> answersWithFile = tenderData.TenderLots
                .SelectMany(lot => lot.LotItems)
                .SelectMany(lotItem => lotItem.Offers)
                .SelectMany(offer => offer.TenderCriteriaAnswers)
                .Where(offer => offer.FileUuid != null)
                //.DistinctBy(offer => offer.FileUuid)
                .ToList();

            List<TenderCriteriaAnswer> groupAnswers = new List<TenderCriteriaAnswer>();
            List<TenderCriteriaAnswer> singleAnswers = new List<TenderCriteriaAnswer>();

            var res = answersWithFile.GroupBy(x => x.FileUuid);
            foreach(var group in res)
                foreach(var val in group)
                    if(group.Count() > 1)
                    {
                        groupAnswers.Add(val);
                        break;
                    }
                    else singleAnswers.Add(val);

            LoadFiles(groupAnswers, true);
            LoadFiles(singleAnswers, false);
            
            void LoadFiles(List<TenderCriteriaAnswer> answersWithFiles, Boolean isGroupAnswers)
            {
                foreach(var page in answersWithFiles.Paginate(10))
                {
                    List<Task> tasks = new List<Task>();
                    foreach(TenderCriteriaAnswer item in page)
                    {
                        TenderCriteriaAnswer answer = item;
                        tasks.Add(HttpClient.GetByteArrayAsync($"file/get?uuid={item.FileUuid.ToString().ToUpper()}").ContinueWith(x => SaveFile(isGroupAnswers, x, answer)));
                    }
                    Task.WaitAll(tasks.ToArray());
                }
            }

            void SaveFile(Boolean isGroupAnswer, Task<Byte[]> requestResult, TenderCriteriaAnswer item)
            {
                //String longPartOfFileName = $"{item.SupplierName} — {item.}";
                String ext = Path.GetExtension(item.Value);
                
                String filePath = Path.Combine(workDirectory, $"#{tenderData.TenderNumber:0000} тур {tenderData.TenderRoundNumber} лот {item.TenderLotNo}), {item.TenderLotItemName} — [{item.SupplierEdrpou}] {item.SupplierName}, {item.Name}{ext}");
                Byte[] fileBody = requestResult.Result;
                File.WriteAllBytes(filePath, fileBody);
            }
        }
    }

    public partial class ApsClient
    {
        private readonly HttpClient HttpClient;

        private HttpClient GetConfiguredRestClient(AppSettings appSettings)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(appSettings.HttpAddressOf.ApsRestService);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", appSettings.ApsConnectorAuthInfoInBase64String);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public String GetAsJson(String query)
        {
            HttpResponseMessage response = HttpClient.GetAsync(query).Result;
            response.EnsureSuccessStatusCode();
            String result = response.Content.ReadAsStringAsync().Result;
            result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
            result = JToken.Parse(result).ToString(Formatting.Indented);
#endif
            return result;
        }

        private T GetAsData<T>(String query)
        {
            HttpResponseMessage response = HttpClient.GetAsync(query).Result;
            response.EnsureSuccessStatusCode();
            String result = response.Content.ReadAsStringAsync().Result;
            result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
            result = JToken.Parse(result).ToString(Formatting.Indented);
#endif
            return JsonConvert.DeserializeObject<T>(result);
        }

        private TenderStageInfo SkipTask(TenderData tenderData, ETenderProcessStage suitableProcessStage)
        {
            if(tenderData is null) throw new ArgumentNullException(nameof(tenderData));
            TenderStageInfo tenderStageInfo = null;
            String rem = suitableProcessStage switch
            {
                ETenderProcessStage.St6_OffersProcessingApprovement => "[API] Пропуск задачи \"Заключение специалиста СБ\"",
                ETenderProcessStage.St8_SolutionApprovement         => "[API] Пропуск задачи \"Согласование тедерного комитета\" и перенос тендера на следующий тур.",
                _                                                   => throw new NotImplementedException()
            };

            Int32 tenderNumber = tenderData.TenderNumber;
            tenderStageInfo = GetTenderStageInfo(tenderNumber);
            while(tenderStageInfo.TenderProcessStage == suitableProcessStage)
            {
                if(tenderStageInfo is null) throw new NullReferenceException(nameof(tenderStageInfo));
                if(tenderStageInfo.ApprovementModelId != 10) throw new ArgumentException("Эта функция расчитана на использование совместно с моделью согласования \"ApsProxy\" (Id == 10), "
                   + $"но текущая модель согласования тендера \"{tenderStageInfo.ApprovementModelName}\" (Id == {tenderStageInfo.ApprovementModelId})");

                foreach(TenderStageMember stageMember in tenderStageInfo.TenderProcessStageMembers)
                {
#if DEBUG
                    Console.Write($"Тендер №{tenderStageInfo.TenderNo}, тур {tenderStageInfo.TenderRoundNo} ({tenderStageInfo.TenderProcessStage.AsString()}) — Пропуск задачи пользователя {stageMember}: ");
#endif
                    Approvement apprInfo = Approvement.New(tenderNumber, suitableProcessStage, stageMember.UserId, EApprovementSolution.Approved, rem);
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(apprInfo), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = HttpClient.PostAsync("tender/approve", stringContent).Result;
                    response.EnsureSuccessStatusCode();

                    String result = response.Content.ReadAsStringAsync().Result;
                    result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
                    OperationResult opResult = JsonConvert.DeserializeObject<OperationResult>(result);
#if DEBUG
                    Console.WriteLine(opResult.IsSuccesful ? "выполнено" : "НЕ УДАЛОСЬ ВЫПОЛНИТЬ");
#endif
                }
                tenderStageInfo = GetTenderStageInfo(tenderNumber);
            }
            return tenderStageInfo;
        }
    }

    public enum SkippableTask
    {
        /// <summary> Заключение специалиста СБ </summary>
        St6_ConclusionOfSecurityService,
        /// <summary> Согласование тендерного комитера </summary>
        St8_ApprovementOfTenderCommittee
    }

    public class TenderState
    {
        public Int32 TenderNo { get; set; }
        public ETenderProcessStage ProcessState { get; set; }

        public TenderState(Int32 tenderNo, Int32 processState)
        {
            TenderNo = tenderNo;
            ProcessState = (ETenderProcessStage)processState;
        }
    }

    public class OperationResult
    {
        [JsonProperty("success")]
        public Boolean IsSuccesful { get; set; }
    }
}
