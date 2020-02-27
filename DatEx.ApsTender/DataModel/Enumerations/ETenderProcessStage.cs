using System;
using System.Collections.Generic;
using System.Text;

namespace DatEx.ApsTender.DataModel
{
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
}
