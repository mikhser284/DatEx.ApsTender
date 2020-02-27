using System.Runtime.Serialization;

namespace DatEx.ApsTender.DataModel
{
    /// <summary> Тип тура тендера </summary>
    public enum ETenderRoundType
    {
        /// <summary> Тендер (RFx) </summary>
        [EnumMember(Value = "Тендер (RFx)")]
        TenderRfx,

        /// <summary> Аукцион/Редукцион </summary>
        [EnumMember(Value = "Аукцион/Редукцион")]
        AuctionOrReduction,

        /// <summary> Регистрация закупки </summary>
        [EnumMember(Value = "Регистрация закупки")]
        PurchaseRegistration
    }
}
