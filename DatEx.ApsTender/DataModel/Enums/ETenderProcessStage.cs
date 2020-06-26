
namespace DatEx.ApsTender.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.Text;



    /// <summary> Стадия тендерного процесса </summary>
    public enum ETenderProcessStage
    {
        /// <summary> Ст.1 • Паспорт тендера </summary>
        St1_PassportSetting = 1,
        
        /// <summary> Ст.2 • Подготовка документации </summary>
        St2_DocumentationSetting = 2,
        
        /// <summary> Ст.3 • Корректировка критериев </summary>
        St3_CriteriaSetting = 3,
        
        /// <summary> Ст.4 • Прием предложений </summary>
        St4_OffersAccepting = 4,
        
        /// <summary> Ст.5 • Обработка данных </summary>
        St5_OffersProcessing = 5,
        
        /// <summary> Ст.6 • Согласование обработки </summary>
        St6_OffersProcessingApprovement = 6,
        
        /// <summary> Ст.7 • Выбор решения </summary>
        St7_SolutionSelection = 7,
        
        /// <summary> Ст.8 • Утверждение решения </summary>
        St8_SolutionApprovement = 8,
        
        /// <summary> Ст.9 • Завершено </summary>
        St9_TenderClosed = 9,
    }

    public static class Ext_ETenderProcessStage
    {
        private readonly static Dictionary<ETenderProcessStage, String> Enum_String = new Dictionary<ETenderProcessStage, String>
        {
            { ETenderProcessStage.St1_PassportSetting, "Ст. #1 - Паспорт тендера" },
            { ETenderProcessStage.St2_DocumentationSetting, "Ст. #2 - Подготовка документации" },
            { ETenderProcessStage.St3_CriteriaSetting, "Ст. #3 - Корректировка критериев" },
            { ETenderProcessStage.St4_OffersAccepting, "Ст. #4 - Прием предложений" },
            { ETenderProcessStage.St5_OffersProcessing, "Ст. #5 - Обработка данных" },
            { ETenderProcessStage.St6_OffersProcessingApprovement, "Ст. #6 - Согласование обработки" },
            { ETenderProcessStage.St7_SolutionSelection, "Ст. #7 - Выбор решения" },
            { ETenderProcessStage.St8_SolutionApprovement, "Ст. #8 - Утверждение решения" },
            { ETenderProcessStage.St9_TenderClosed, "Ст. #9 - Завершено" },
        };

        public static String AsString(this ETenderProcessStage enumValue) => Enum_String[enumValue];
    }
}
