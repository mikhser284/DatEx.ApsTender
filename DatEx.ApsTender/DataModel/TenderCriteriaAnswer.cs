
namespace DatEx.ApsTender.DataModel
{
    using Newtonsoft.Json;
    using System;

    /// <summary> Ответ контрагента по критерию </summary>
    [JsonObject(Title = "criteriaValue")]
    public class TenderCriteriaAnswer
    {
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

        public override String ToString() => ToString(0);

        public String ToString(Int32 indentLevel)
        {
            String indent = new String(' ', indentLevel * ApsClient.IndentWidth);
            String file = String.IsNullOrEmpty(FileUrl) ? "" : $" (File url: {FileUrl})";
            return $"{indent} - {Name} = {Value}{file}";
        }
    }
}
