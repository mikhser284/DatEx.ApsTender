using Newtonsoft.Json;
using System;

namespace DatEx.ApsTender.Test.CUI.DataModel
{
    public class ApsRequestResult<T>
    {
        /// <summary> Является ли запрос успешно выполненным </summary>
        [JsonProperty("success", Order = 1)]
        public Boolean IsSuccess { get; set; }
        
        /// <summary> Код ошибки </summary>
        [JsonProperty("errorCode", Order = 2)]
        public int ErrorCode { get; set; }

        /// <summary> Текст ошибки </summary>
        [JsonProperty("errorString")]
        public string ErrorString { get; set; }

        /// <summary> Данные, возвращенные сервисом </summary>
        [JsonProperty("data")]
        public T Data { get; set; }
    }

}
