
namespace DatEx.ApsTender.DataModel
{
    using DatEx.ApsTender.Helpers;
    using Newtonsoft.Json;
    using System;

    /// <summary> Ответ контрагента по критерию </summary>
    [JsonObject(Title = "criteriaValue")]
    public class TenderCriteriaAnswer
    {
        [JsonIgnore]
        public Int32 TenderLotNo { get; set; }

        [JsonIgnore]
        public Guid? TenderLotItemUuid { get; set; }

        [JsonIgnore]
        public Int32? SupplierId { get; set; }

        /// <summary> Id (в APS) </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary> Название </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary> Ответ </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary> Идентификатор файла </summary>
        [JsonProperty("fileUrl")]
        public String FileUrl { get; set; }

        public Boolean IsFile { get; set; }

        public override String ToString() => ToString(0);

        public String ToString(Int32 indentLevel)
        {
            String indent = Ext_String.GetIndent(indentLevel);
            String file = String.IsNullOrEmpty(FileUrl) ? "" : $" (File url: {FileUrl})";
            return $"{indent} - {Name} = {Value}{file}";
        }
    }
}
