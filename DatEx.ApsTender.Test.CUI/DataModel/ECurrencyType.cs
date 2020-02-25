using System.Runtime.Serialization;

namespace DatEx.ApsTender.Test.CUI.DataModel
{
    /// <summary> Валюта тендера </summary>
    public enum ECurrencyType
    {
        /// <summary> Евро </summary>
        [EnumMember(Value = "EUR")]
        EUR,

        /// <summary> Гривна </summary>
        [EnumMember(Value = "UAH")]
        UAH,

        /// <summary> Доллар </summary>
        [EnumMember(Value = "USD")]
        USD
    }
}
