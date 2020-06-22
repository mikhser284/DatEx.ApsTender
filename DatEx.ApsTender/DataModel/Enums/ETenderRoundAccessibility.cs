using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DatEx.ApsTender.DataModel
{
    /// <summary> Доступность тура тендера (Тип публикации тура тендера) </summary>
    public enum ETenderRoundAccessibility
    {
        /// <summary> Открытый тендер </summary>
        [EnumMember(Value = "Открытый")]
        Public,

        /// <summary> Закрытый тендер </summary>
        [EnumMember(Value = "Закрытый")]
        Private
    }

    public static class Ext_ETenderRoundAccessibility
    {
        private readonly static Dictionary<ETenderRoundAccessibility, String> Enum_String = new Dictionary<ETenderRoundAccessibility, String>
        {
            { ETenderRoundAccessibility.Public, "Публичный" },
            { ETenderRoundAccessibility.Private, "Приватный" },
        };

        public static String AsString(this ETenderRoundAccessibility enumValue) => Enum_String[enumValue];
    }
}
