
namespace DatEx.ApsTender.DataModel
{
    using System.Runtime.Serialization;



    /// <summary> Валюта тендера </summary>
    public enum ETenderCurrencyType
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
