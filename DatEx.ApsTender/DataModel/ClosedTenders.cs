using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DatEx.ApsTender.DataModel
{
    public class ClosedTenders_RequestResult
    {
        [JsonProperty("success")]
        public Boolean IsSuccess { get; set; }

        [JsonProperty("data")]
        public List<ClosedTenders_RequestData> Data { get; set; }

        [JsonProperty("next_page")]
        public ClosedTenders_NextDataPage NextPage { get; set; }
    }

    public class ClosedTenders_NextDataPage
    {
        [JsonProperty("limit")]
        public Int32 Limit { get; set; }

        [JsonProperty("offset")]
        public DateTime Offset { get; set; }

        [JsonProperty("path")]
        public String Path { get; set; }
    }

    public class ClosedTenders_RequestData
    {
        [JsonProperty("tenderId")]
        public Int32 TenderId { get; set; }

        [JsonProperty("dateEnd")]
        public DateTime DateEnd { get; set; }

        [JsonProperty("tenderUuid")]
        public Guid TenderUuid { get; set; }
    }
}
