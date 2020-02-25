using System.Runtime.Serialization;

namespace DatEx.ApsTender.DataModel
{
    /// <summary> Доступность тура тендера (Тип публикации тура тендера) </summary>
    public enum ETenderRoundAccessibility
    {
        /// <summary> Открытый тендер </summary>
        [EnumMember(Value = "Открытый")]
        OpenTender,

        /// <summary> Закрытый тендер </summary>
        [EnumMember(Value = "Закрытый")]
        ClosedTender
    }
}
