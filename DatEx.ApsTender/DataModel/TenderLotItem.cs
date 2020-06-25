using System;
using Newtonsoft.Json;

namespace DatEx.ApsTender.DataModel
{
    public class TenderLotItem
    {
        [JsonProperty("itemName")]
        public String Name { get; set; }

        [JsonProperty("unitName")]
        public String MeasureUnitName { get; set; }

        [JsonProperty("nmcId")]
        public Int32 NomenclatureId { get; set; }

        [JsonProperty("nmcUuid")]
        public String NomenclatureUuid { get; set; }

        [JsonProperty("qty")]
        public Double Quantity { get; set; }

        [JsonProperty("tenderItemUuid")]
        public String TenderItemUuid { get; set; }

        [JsonProperty("offersUrl")]
        public String OffersUrl { get; set; }

        [JsonProperty("offersUrlEx")]
        public String OffersUrlEx { get; set; }

        public override String ToString()
        {
            return $"      {NomenclatureId,10} | {Name,-50} | {Quantity,15} | {MeasureUnitName,-10} | {OffersUrl}";
        }
    }

}
