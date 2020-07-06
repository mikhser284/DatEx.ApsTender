namespace DatEx.ApsTender.DataModel
{
    using System;
    using System.Collections.Generic;
    using DatEx.ApsTender.Helpers;
    using Newtonsoft.Json;



    public class TenderLotItem
    {
        [JsonProperty("itemName")]
        public String Name { get; set; }

        [JsonProperty("unitName")]
        public String MeasureUnitName { get; set; }

        [JsonProperty("nmcId")]
        public Int32 NomenclatureId { get; set; }

        [JsonProperty("nmcUuid")]
        public Guid NomenclatureUuid { get; set; }

        [JsonProperty("qty")]
        public Double Quantity { get; set; }

        [JsonProperty("tenderItemUuid")]
        public Guid TenderItemUuid { get; set; }

        [JsonProperty("offersUrl")]
        public String OffersUrl { get; set; }

        [JsonProperty("offersUrlEx")]
        public String OffersUrlEx { get; set; }

        public List<TenderLotItemOffer> Offers { get; set; } = new List<TenderLotItemOffer>();

        public override String ToString()
        {
            return ToString(0);
        }

        public String ToString(Int32 indentLevel)
        {
            String indent = Ext_String.GetIndent(indentLevel);
            return $"{indent} {Name,-50} | {Quantity,10} {MeasureUnitName,-10} | {NomenclatureId,6} ({TenderItemUuid})";
        }
    }
}
