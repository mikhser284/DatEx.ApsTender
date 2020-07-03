using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatEx.ApsTender.Helpers;
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

        [JsonProperty("stageNumber")]
        public Int32 TenderRoundNo { get; set; }

        [JsonProperty("process")]
        public ETenderProcessStage TenderProcessStage { get; set; }

        [JsonProperty("apprPatternId")]
        public Int32 ApprovementModelId { get; set; }

        [JsonProperty("apprPatternName")]
        public String ApprovementModelName { get; set; }

        [JsonProperty("tenderOwnerPath")]
        public String TenderOwnerPath { get; set; }

        [JsonProperty("members")]
        public List<TenderStageMember> TenderProcessStageMembers { get; set; }

        public override string ToString() => ToString(0);

        public String ToString(Int32 indentLevel)
        {
            String indent = Ext_String.GetIndent(indentLevel);
            Int32 indentLevel2 = indentLevel + 2;
            return $"{indent}Информация о текущем состоянии тендера"
                + $"\n{indent} - Номер тендера:                {TenderNo}"
                + $"\n{indent} - Номер текущего тура:          {TenderRoundNo}"
                + $"\n{indent} - Стадия тендерного процесса:   {TenderProcessStage.AsString()}"
                + $"\n{indent} - Id модели согласования:       {ApprovementModelId}"
                + $"\n{indent} - Название модели согласования: {ApprovementModelName}"
                + $"\n{indent} - Участники тендера ({TenderProcessStageMembers.Count} шт.):\n{String.Join(",", TenderProcessStageMembers.Select(x => $"{x.ToString(indentLevel2)}"))}"
                + $"\n{indent} - UUID тендера:                 {TenderUuid}"
                + $"\n{indent} - Tender owner path:            {TenderOwnerPath}"
                ;
        }

        //public static Boolean operator ==(TenderStageInfo a, TenderStageInfo b)
        //{
        //    if(a == null && b == null) return true;
        //    if((a == null && b != null) || (a != null && b == null)) return false;
        //    if(a.TenderNo == b.TenderNo
        //        && a.TenderUuid == b.TenderUuid
        //        && a.TenderRoundNo == b.TenderRoundNo
        //        && a.TenderProcessStage == b.TenderProcessStage
        //        && a.ApprovementModelId == b.ApprovementModelId
        //        && a.ApprovementModelName == b.ApprovementModelName
        //        && a.TenderProcessStageMembers.Count == b.TenderProcessStageMembers.Count)
        //    {
        //        if (a.TenderProcessStageMembers.Count == 0) return true;
        //        HashSet<Int32> usersIds = new HashSet<int>(a.TenderProcessStageMembers.Select(x => x.UserId));
        //        foreach (var member in b.TenderProcessStageMembers) if (!usersIds.Contains(member.UserId)) return false;

        //        return true;
        //    }
        //    return false;
        //}

        //public static Boolean operator !=(TenderStageInfo a, TenderStageInfo b)
        //{
        //    return !(a == b);
        //}
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

        public override string ToString() => ToString(0);

        public String ToString(Int32 indentLevel)
        {
            String indent = Ext_String.GetIndent(indentLevel);
            return $"{indent}{LastName} {FirstName} {MiddleName} (Id = {UserId})";
        }
    }

}
