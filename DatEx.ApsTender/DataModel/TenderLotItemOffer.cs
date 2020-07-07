
namespace DatEx.ApsTender.DataModel
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using DatEx.ApsTender.Helpers;



    [JsonObject(Title = "offer")]
    public class TenderLotItemOffer
    {
        /// <summary> Id контрагента </summary>
        [JsonProperty("supplierId")]
        public Int32 SupplierId { get; set; }

        /// <summary> Контрагент </summary>
        [JsonProperty("supplier")]
        public String SupplierName { get; set; }

        /// <summary> ЕДРПОУ контрагента </summary>
        [JsonProperty("edrpou")]
        public String Edrpou { get; set; }

        /// <summary> Является победителем </summary>
        [JsonProperty("isWinner")]
        public Boolean? IsWinner { get; set; }

        /// <summary> Ответы по критериям тендера для данной позиции </summary>
        [JsonProperty("criteriaValues")]
        public List<TenderCriteriaAnswer> TenderCriteriaAnswers { get; set; }

        public override String ToString() => ToString(0);

        public String ToString(Int32 indentLevel)
        {
            String indent = Ext_String.GetIndent(indentLevel);
            String isWinner = IsWinner.AsString("ПОБЕДИТЕЛЬ", "", "");
            return $"{indent}"
                + $"Контрагент: {SupplierName} {isWinner}\n{indent}"
                + $"ЕДРПОУ:     {Edrpou}\n{indent}"
                + $"Id:         {SupplierId}";            
        }
    }
}
