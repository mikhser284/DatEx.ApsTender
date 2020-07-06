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

    public class ApsClient
    {
        private readonly HttpClient HttpClient;

        public ApsClient(AppSettings appSettings)
        {
            HttpClient = GetConfiguredRestClient(appSettings);// new Uri(baseAddress));
        }

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

        public RequestResult<TenderData> GetTenderData(Int32 tenderNo, Int32? tenderType = null)
        {
            List<String> parameters = new List<string>();
            parameters.AddHttpParameter("Id", tenderNo);
            parameters.AddHttpParameter("type", tenderType ?? 1);
            //
            return GetAsData<RequestResult<TenderData>>("tender/get".AsParametrizedHttpRequest(parameters));
        }

        public List<TenderLotItemOffer> GetLotItemOffers(TenderData tenderData)
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
                            lotItem.Offers.Clear();
                            lotItem.Offers.AddRange(offers);
                            answer.TenderLotNo = lot.LotNumber;
                            answer.SupplierId = item.SupplierId;
                            answer.IsFile = !String.IsNullOrWhiteSpace(answer.FileUrl);
                        }
                    }
                    allOffers.AddRange(offers);
                }
                catch(Exception ex)
                {
                    var requestRes = JsonConvert.DeserializeObject<RequestResult<List<TenderLotItemOffer>>>(result);
                    if(requestRes.IsSuccess == false) offers = new List<TenderLotItemOffer>();
                }
            }
            
            return allOffers;
        }

        //public async List<TenderLotItemOffers> GetTenderRoundOffers(TenderData tenderData)
        //{
        //    if(tenderData == null) throw new ArgumentNullException(nameof(tenderData));
        //    List<Task<HttpResponseMessage>> requests = new List<Task<HttpResponseMessage>>();
        //    foreach(var tenderLot in tenderData.TenderLots)
        //    {
        //        foreach(TenderLotItem item in tenderLot.LotItems)
        //        {

        //        }
        //    }
        //}


        public TendersList_RequestResult GetTendersOnStage(ETenderProcessStage? stageOfTenderProcess = null, Int32 itemsPerPage = 10_000)
        {
            List<String> parameters = new List<string>();
            parameters.AddHttpParameter("process", stageOfTenderProcess);
            parameters.AddHttpParameter("limit", itemsPerPage);
            //
            return GetAsData<TendersList_RequestResult>("tender/listProcess".AsParametrizedHttpRequest(parameters));
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
                result = JToken.Parse(result).ToString(Newtonsoft.Json.Formatting.Indented);
#endif
                RequestInfo_TenderStageInfo deserializedObj = JsonConvert.DeserializeObject<RequestInfo_TenderStageInfo>(result);
                if (deserializedObj?.RequestData == null) continue;
                tendersStageInfo.Add(deserializedObj.RequestData);
            }
            return tendersStageInfo;
        }

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


        public List<TenderData> GetTendersData(List<Int32> tendersNumbers)
        {
            List<TenderData> tendersData = new List<TenderData>();
            if (tendersNumbers == null || tendersNumbers.Count == 0) return tendersData;
            Task<HttpResponseMessage>[] requests = new Task<HttpResponseMessage>[tendersNumbers.Count];
            for (Int32 i = 0; i < tendersNumbers.Count; i++)
            {
                Int32 tenderNo = tendersNumbers[i];
                requests[i] = HttpClient.GetAsync($"tender/get?id={tenderNo}");
            }
            Task.WaitAll(requests);
            for (Int32 i = 0; i < tendersNumbers.Count; i++)
            {
                HttpResponseMessage response = requests[i].Result;
                response.EnsureSuccessStatusCode();
                String result = response.Content.ReadAsStringAsync().Result;
                result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
                result = JToken.Parse(result).ToString(Newtonsoft.Json.Formatting.Indented);
#endif
                RequestResult<TenderData> deserializedObj = JsonConvert.DeserializeObject<RequestResult<TenderData>>(result);
                if (deserializedObj == null || deserializedObj.IsSuccess != true) continue;
                tendersData.Add(deserializedObj.Data);
            }
            return tendersData;
        }



        public TenderStageInfo SkipApprovementOfSecurityService(TenderData tenderData)
        {
            return MoveTenderNext(tenderData, ETenderProcessStage.St6_OffersProcessingApprovement);
        }

        public TenderStageInfo SkipApprovementOfTenderComitet(TenderData tenderData)
        {       
            return MoveTenderNext(tenderData, ETenderProcessStage.St8_SolutionApprovement);
        }

        private TenderStageInfo MoveTenderNext(TenderData tenderData, ETenderProcessStage suitableProcessStage)
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


        public async Task<Byte[]> GetFile(Guid fileUuid) => await HttpClient.GetByteArrayAsync($"tender/getfile?uuid={fileUuid.ToString()}");

        public async void SaveFileAs(Guid fileUuid, String filePath)
        {
            Byte[] fileBody = await GetFile(fileUuid);
            File.WriteAllBytes(filePath, fileBody);
        }

        //public async Task<List<Byte[]>> GetCommertialOffers(TenderData tenderData)
        //{
        //    if(tenderData == null) throw new ArgumentNullException(nameof(tenderData));

        //    foreach(var lot in tenderData.TenderLots)
        //    {
        //        if(lot.LotItems == null || lot.LotItems.Count == 0) continue;

        //        foreach(var lotItem in lot.LotItems)
        //        {
        //            //lotItem.TenderItemUuid

        //            HttpClient.Get
        //        }
        //    }
        //}
    }


    public class OperationResult
    {
        [JsonProperty("success")]
        public Boolean IsSuccesful { get; set; }
    }
}
