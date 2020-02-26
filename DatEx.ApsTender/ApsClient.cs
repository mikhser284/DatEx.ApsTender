using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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

        public TendersList_RequestResult GetTendersOnStage(EStageOfTenderProcess? stageOfTenderProcess = null, Int32 itemsPerPage = 10_000)
        {
            List<String> parameters = new List<string>();
            parameters.AddHttpParameter("process", stageOfTenderProcess);            
            parameters.AddHttpParameter("limit", itemsPerPage);
            //
            return GetAsData<TendersList_RequestResult>("tender/listProcess".AsParametrizedHttpRequest(parameters));
        }

        public ClosedTenders_RequestResult  GetTenders_ClosedTenders()
        {
            return GetAsData<ClosedTenders_RequestResult>("tender/listClosed?offset=2017-01-01&limit=1000");            
        }
    }

    public static class Ext_HttpUtils
    {
        public static List<String> AddHttpParameter(this List<String> parametersList, String paramName, EStageOfTenderProcess? paramValue)
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
