
namespace DatEx.ApsTender.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using DatEx.ApsTender.DataModel.Enums;
    using Newtonsoft.Json;



    public class TenderLot
    {
        [JsonProperty("lotNumber")]
        public Int32 LotNumber { get; set; }

        [JsonProperty("lotName")]
        public String LotName { get; set; }

        [JsonProperty("stageNumber")]
        public Int32 StageNumber { get; set; }

        [JsonProperty("lotState")]
        public ELogStateName LotState { get; set; }

        [JsonProperty("lotResultNote")]
        public String LotResultNote { get; set; }

        [JsonProperty("lotStateName")]
        public String LotStateName { get; set; }

        [JsonProperty("lotReport")]
        public String LotReport { get; set; }

        [JsonProperty("criteria")]
        public List<TenderCriteria> LotCriteria { get; set; }

        [JsonProperty("items")]
        public List<TenderLotItem> LotItems { get; set; }

        public override String ToString()
        {
            String str = $"{LotName} — {LotStateName} (Лот № {LotNumber}, Стадия № {StageNumber})";
            str += $"\n   Примечания: {LotResultNote}";
            str += $"\n   Отчет по лоту: {LotReport}";
            return str;
        }
    }

}
