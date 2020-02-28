using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DatEx.ApsTender.DataModel
{
    public class TenderLot
    {
        [JsonProperty("lotNumber")]
        public Int32 LotNumber { get; set; }

        [JsonProperty("lotName")]
        public String LotName { get; set; }

        [JsonProperty("stageNumber")]
        public Int32 StageNumber { get; set; }

        [JsonProperty("lotState")]
        public Int32 LotState { get; set; }

        [JsonProperty("lotResultNote")]
        public String LotResultNote { get; set; }

        [JsonProperty("lotStateName")]
        public String LotStateName { get; set; }

        [JsonProperty("lotReport")]
        public String LotReport { get; set; }

        [JsonProperty("criteria")]
        public List<TenderCriteria> Criteria { get; set; }

        [JsonProperty("items")]
        public List<TenderLotItem> LotItems { get; set; }
    }

}
