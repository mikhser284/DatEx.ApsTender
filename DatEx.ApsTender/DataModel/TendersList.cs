using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DatEx.ApsTender.DataModel
{
    


    public class TendersList_RequestResult
    {
        [JsonProperty("success")]
        public Boolean IsSuccess { get; set; }

        [JsonProperty("data")]
        public List<TendersList_RequestData> Data { get; set; }

        [JsonProperty("next_page")]
        public TendersList_NextDataPage NextPage { get; set; }
    }

    public class TendersList_RequestData
    {
        [JsonProperty("tenderId")]
        public Int32 TenderId { get; set; }

        [JsonProperty("tenderOwnerPath")]
        public String TenderOwnerPath { get; set; }

        [JsonProperty("tenderUuid")]
        public Guid TenderUuid { get; set; }
    }

    public class TendersList_NextDataPage
    {
        [JsonProperty("process")]
        public Int32 Process { get; set; }

        [JsonProperty("limit")]
        public Int32 Limit { get; set; }

        [JsonProperty("offset")]
        public Int32 Offset { get; set; }

        [JsonProperty("path")]
        public String Path { get; set; }
    }
}
