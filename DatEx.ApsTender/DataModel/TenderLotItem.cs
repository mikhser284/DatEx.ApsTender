namespace DatEx.ApsTender.DataModel
{
    public class TenderLotItem
    {
        public string itemName { get; set; }
        public string unitName { get; set; }
        public int nmcId { get; set; }
        public string nmcUuid { get; set; }
        public int qty { get; set; }
        public string tenderItemUuid { get; set; }
        public string offersUrl { get; set; }
        public string offersUrlEx { get; set; }
    }

}
