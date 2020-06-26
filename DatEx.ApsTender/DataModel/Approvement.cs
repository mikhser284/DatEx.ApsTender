using System;
using DatEx.ApsTender.DataModel.Enums;
using Newtonsoft.Json;

namespace DatEx.ApsTender.DataModel
{
    [JsonObject(Title = "Rootobject")]
    internal class Approvement
    {
        [JsonProperty("data")]
        public ApprovementResult Result { get; set; }

        public static Approvement New(Int32 tenderNumber, ETenderProcessStage procStage, Int32 usrId, EApprovementSolution solution, String remarks)
        {
            return new Approvement
            {
                Result = new ApprovementResult
                {
                    TenderNumber = tenderNumber,
                    ProcessStage = procStage,
                    UserId = usrId,
                    Solution = solution,
                    Remarks = remarks
                }
            };
        }
    }


    [JsonObject(Title = "Data")]
    internal class ApprovementResult
    {
        [JsonProperty("tndId")]
        public Int32 TenderNumber { get; set; }

        [JsonProperty("process")]
        public ETenderProcessStage ProcessStage { get; set; }

        [JsonProperty("userId")]
        public Int32 UserId { get; set; }

        [JsonProperty("code")]
        public EApprovementSolution Solution { get; set; }

        [JsonProperty("note")]
        public String Remarks { get; set; }
    }
}
