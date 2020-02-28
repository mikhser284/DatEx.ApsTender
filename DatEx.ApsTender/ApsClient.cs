using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DatEx.ApsTender.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DatEx.ApsTender
{
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
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            return httpClient;
        }

        public String GetAsJson(String query)
        {
            HttpResponseMessage response = HttpClient.GetAsync(query).Result;
            response.EnsureSuccessStatusCode();
            String result = response.Content.ReadAsStringAsync().Result;
            result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
            result = JToken.Parse(result).ToString(Newtonsoft.Json.Formatting.Indented);
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
            result = JToken.Parse(result).ToString(Newtonsoft.Json.Formatting.Indented);
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
            for(Int32 i = 0; i <= 9 ; i++)
            {
                Int32 processStage = i;
                requests[i] = HttpClient.GetAsync($"tender/listProcess?process={processStage}&limit={itemsPerPage}");
            }
            Task.WaitAll(requests);
            for(Int32 i = 0; i <= 9; i++)
            {
                HttpResponseMessage response = requests[i].Result;
                response.EnsureSuccessStatusCode();
                String result = response.Content.ReadAsStringAsync().Result;
                result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
                result = JToken.Parse(result).ToString(Newtonsoft.Json.Formatting.Indented);
#endif
                var deserializedObj = JsonConvert.DeserializeObject<TendersList_RequestResult>(result);
                if(deserializedObj?.Data == null) continue;
                tendersAndStates.AddRange(deserializedObj.Data.Select(x => new TenderState(x.TenderId, i)));
            }
            tendersAndStates = tendersAndStates.OrderBy(x => x.TenderNo).ToList();
            return tendersAndStates;
        }

        public List<TenderStageInfo> GetTendersStageInfo(List<Int32> tendersNumers)
        {
            List<TenderStageInfo> tendersStageInfo = new List<TenderStageInfo>();
            if(tendersNumers == null || tendersNumers.Count == 0) return tendersStageInfo;
            Task<HttpResponseMessage>[] requests = new Task<HttpResponseMessage>[tendersNumers.Count];
            for(Int32 i = 0; i < tendersNumers.Count; i++)
            {
                Int32 tenderNo = tendersNumers[i];
                requests[i] = HttpClient.GetAsync($"tender/getStatus?id={tenderNo}");
            }
            Task.WaitAll(requests);
            for(Int32 i = 0; i < tendersNumers.Count; i++)
            {
                HttpResponseMessage response = requests[i].Result;
                response.EnsureSuccessStatusCode();
                String result = response.Content.ReadAsStringAsync().Result;
                result = Regex.Replace(result, @"(?<![\\])\\(?![bfnrt""\\])", @"\\");
#if DEBUG
                result = JToken.Parse(result).ToString(Newtonsoft.Json.Formatting.Indented);
#endif
                RequestInfo_TenderStageInfo deserializedObj = JsonConvert.DeserializeObject<RequestInfo_TenderStageInfo>(result);
                if(deserializedObj?.RequestData == null) continue;
                tendersStageInfo.Add(deserializedObj.RequestData);
            }
            return tendersStageInfo;
        }

        public RequestInfo_TenderStageInfo GetTenderStageInfo(Int32 tenderNo)
        {
            return GetAsData<RequestInfo_TenderStageInfo>($"tender/getStatus?id={tenderNo}");
        }
    }

    public static class Ext_HttpUtils
    {
        public static List<String> AddHttpParameter(this List<String> parametersList, String paramName, ETenderProcessStage? paramValue)
        {
            if(paramValue != null) parametersList.Add($"{paramName}={(Int32)paramValue}");
            return parametersList;
        }

        public static List<String> AddHttpParameter(this List<String> parametersList, String paramName, DateTime? paramValue)
        {
            if(paramValue != null) parametersList.Add($"{paramName}={paramValue:yyyy-MM-dd}T{paramValue:HH:mm:ss}Z");
            return parametersList;
        }

        public static List<String> AddHttpParameter(this List<String> parametersList, String paramName, String paramValue)
        {
            if(!String.IsNullOrEmpty(paramValue)) parametersList.Add($"{paramName}={paramValue}");
            return parametersList;
        }

        public static List<String> AddHttpParameter(this List<String> parametersList, String paramName, Int32? paramValue)
        {
            if(paramValue != null) parametersList.Add($"{paramName}={paramValue}");
            return parametersList;
        }

        public static List<String> AddHttpParameter(this List<String> parametersList, String paramName, Boolean? paramValue)
        {
            if(paramValue != null) parametersList.Add($"{paramName}={(paramValue == false ? 0 : 1)}");
            return parametersList;
        }

        public static String AsParametrizedHttpRequest(this String request, List<String> parametersList)
        {
            return request + ((parametersList != null && parametersList.Count > 0) ? $"?{String.Join('&', parametersList)}" : "");
        }
    }
}
