using System;
using System.Collections.Generic;
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

    public static class Ext_ETenderRoundType
    {
        private readonly static Dictionary<ETenderRoundType, String> Enum_String = new Dictionary<ETenderRoundType, string>
        {
            { ETenderRoundType.TenderRfx, "Сбор предложений" },
            { ETenderRoundType.AuctionOrReduction, "Редукцион" },
            { ETenderRoundType.PurchaseRegistration, "Регистрация закупки" }
        };

        public static String AsString(this ETenderRoundType enumValue) => Enum_String[enumValue];
    }
}
