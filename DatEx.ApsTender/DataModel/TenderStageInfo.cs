using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DatEx.ApsTender.DataModel
{
    public class RequestInfo_TenderStageInfo
    {
        [JsonProperty("success")]
        public Boolean RequestIsSuccess { get; set; }

        [JsonProperty("data")]
        public TenderStageInfo RequestData { get; set; }
    }

    public class TenderStageInfo
    {
        [JsonProperty("tenderNumber")]
        public Int32 TenderNo { get; set; }

        [JsonProperty("tenderUuid")]
        public String TenderUuid { get; set; }

        [JsonProperty("apprPatternId")]
        public Int32 ApprovementModelId { get; set; }

        [JsonProperty("apprPatternName")]
        public String ApprovementModelName { get; set; }

        [JsonProperty("stageNumber")]
        public Int32 TenderRoundNo { get; set; }

        [JsonProperty("process")]
        public ETenderProcessStage TenderProcessStage { get; set; }

        [JsonProperty("tenderOwnerPath")]
        public String TenderOwnerPath { get; set; }

        [JsonProperty("members")]
        public List<TenderStageMember> TenderProcessStageMembers { get; set; }

        public override String ToString()
        {
            return $"Tender #{TenderNo,-4} (round #{TenderRoundNo}, {TenderProcessStage,-32}) — Ids of members: {String.Join(",", TenderProcessStageMembers)}";
        }
    }

    public class TenderStageMember
    {
        [JsonProperty("userId")]
        public Int32 UserId { get; set; }

        [JsonProperty("firstName")]
        public String FirstName { get; set; }

        [JsonProperty("middleName")]
        public String MiddleName { get; set; }
        
        [JsonProperty("lastName")]
        public String LastName { get; set; }

        public override String ToString() => $"{LastName} {FirstName} {MiddleName} (Id = {UserId})";
    }

}
