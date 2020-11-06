
namespace DatEx.ApsTender.DataModel
{
    using System;
    using System.Collections.Generic;
    using DatEx.ApsTender.DataModel.Enums;
    using DatEx.ApsTender.Helpers;
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
        public ELotSolution LotState { get; set; }

        [JsonProperty("lotStateName")]
        public String LotStateName { get; set; }

        [JsonProperty("lotResultNote")]
        public String LotResultNote { get; set; }

        [JsonProperty("lotReport")]
        public String LotReport { get; set; }

        [JsonProperty("criteria")]
        public List<TenderCriteria> LotCriteria { get; set; }

        [JsonProperty("items")]
        public List<TenderLotItem> LotItems { get; set; }

        public override String ToString()
        {
            return ToString(0);
        }

        public String ToString(Int32 indentLevel)
        {
            String indent = Ext_String.GetIndent(indentLevel);
            String str = $"{indent}{LotName} — {LotStateName} (Лот № {LotNumber}, Стадия № {StageNumber})";
            str += $"\n{indent}Примечания: {LotResultNote}";
            str += $"\n{indent}Отчет по лоту: {LotReport}";
            return str;
        }
    }

}
