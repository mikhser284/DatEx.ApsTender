using System;
using System.Collections.Generic;
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
        public Guid NomenclatureUuid { get; set; }

        [JsonProperty("qty")]
        public Double Quantity { get; set; }

        [JsonProperty("tenderItemUuid")]
        public Guid TenderItemUuid { get; set; }

        [JsonIgnore]
        public List<TenderLotItemOffers> Offers { get; set; } = new List<TenderLotItemOffers>();

        public void RetreiveOffers(ApsClient apsClient)
        {
            Offers = apsClient.GetLotItemOffers(TenderItemUuid);
        }

        [JsonProperty("offersUrl")]
        public String OffersUrl { get; set; }

        [JsonProperty("offersUrlEx")]
        public String OffersUrlEx { get; set; }

        public override String ToString()
        {
            return $"      {NomenclatureId,15} ({TenderItemUuid}) | {Name,-50} | {Quantity,10} | {MeasureUnitName,-10} | {OffersUrlEx}";
        }
    }

}
