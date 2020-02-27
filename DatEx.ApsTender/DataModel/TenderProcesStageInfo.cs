﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DatEx.ApsTender.DataModel
{
    public class TenderProcesStageInfo
    {
        [JsonProperty("success")]
        public Boolean IsSuccess { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("tenderNumber")]
        public String TenderNo { get; set; }

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
    }

}
