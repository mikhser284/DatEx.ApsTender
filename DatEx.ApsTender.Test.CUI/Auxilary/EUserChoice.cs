namespace DatEx.ApsTender.Test.CUI
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum EUserChoice
    {
        /// <summary> Выход </summary>        
        [FriendlyName("Выход")]
        Exit = 0,

        /// <summary> Получить состояние тендера </summary>
        [FriendlyName("Получить состояние тендера")]
        GetTenderState = 1,

        /// <summary> Получить данные тендера тендера </summary>
        [FriendlyName("Получить данные тендера")]
        GetTenderData = 2,

        /// <summary> Получить данные тендера, а так же и коммерческие предложения контрагентов по текущему туру </summary>
        [FriendlyName("Получить данные тендера, а так же и коммерческие предложения контрагентов по текущему туру")]
        GetTenderDataAndTenderRoundOffers = 3,


        /// <summary> Получить данные тендера, а так же и коммерческие предложения контрагентов И ИХ ФАЙЛЫ по текущему туру </summary>
        [FriendlyName("Получить данные тендера, а так же и коммерческие предложения контрагентов И ИХ ФАЙЛЫ по текущему туру")]
        GetTenderDataAndTenderRoundOffersAndContractorFiles = 4,

        /// <summary> Получить тендерную документацию </summary>
        [FriendlyName("Получить тендерную документацию")]
        GetTenderDocumentation,

        /// <summary> Пропусть задачу специалиста СБ </summary>
        [FriendlyName("Пропусть задачу специалиста СБ")]
        SkipTaskOfSecurityService,

        /// <summary> Пропустить задачу тендерного коммитета </summary>
        [FriendlyName("Пропустить задачу тендерного коммитета")]
        SkipTaskOfTenderComitet,

        /// <summary> Применить решение тендерного комитета </summary>
        [FriendlyName("Применить решение тендерного комитета")]
        ApplySolytionOfTenderComitet,
    }
}
