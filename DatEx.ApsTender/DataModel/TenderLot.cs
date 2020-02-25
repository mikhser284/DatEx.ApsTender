namespace DatEx.ApsTender.DataModel
{
    public class TenderLot
    {
        public int lotNumber { get; set; }
        public string lotName { get; set; }
        public int stageNumber { get; set; }
        public int lotState { get; set; }
        public string lotResultNote { get; set; }
        public string lotStateName { get; set; }
        public string lotReport { get; set; }
        public TenderCriteria[] criteria { get; set; }
        public TenderLotItem[] items { get; set; }
    }

}
