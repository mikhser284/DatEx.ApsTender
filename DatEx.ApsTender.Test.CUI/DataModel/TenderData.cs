using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatEx.ApsTender.Test.CUI.DataModel
{

    public class TenderData
    {
        /// <summary> № тендера </summary>
        [JsonProperty("tenderNumber", Order = 1)]
        public Int32 TenderNumber { get; set; }

        /// <summary> Название тендера </summary>
        [JsonProperty("tenderName", Order = 2)]
        public String TenderName { get; set; }

        /// <summary> Guid тендера </summary>
        [JsonProperty("tenderUuid", Order = 3)]
        public Guid TenderGuid { get; set; }

        /// <summary> Id автора тендера </summary>
        [JsonProperty("tenderAuthorId", Order = 4)]
        public Int32 AuthorId { get; set; }

        /// <summary> ФИО автора тендера </summary>
        [JsonProperty("tenderAuthorName", Order = 5)]
        public String AuthorName { get; set; }

        /// <summary> Email автора тендера </summary>
        [JsonProperty("tenderAuthorEMail", Order = 6)]
        public String AuthorEMail { get; set; }

        /// <summary> ID компании по которой проводится тендер </summary>
        [JsonProperty("companyId", Order = 7)]
        public Int32 CompanyId { get; set; }

        /// <summary> Название компании по которой проводится тендер </summary>
        [JsonProperty("companyName", Order = 8)]
        public string CompanyName { get; set; }

        /// <summary> Id номенклатурной категории </summary>
        [JsonProperty("industryId", Order = 9)]
        public int NomenclatureCategoryId { get; set; }

        /// <summary> Номенклатурная категория </summary>
        [JsonProperty("industryName", Order = 10)]
        public string NomenclatureCategoryName { get; set; }

        /// <summary> Бюджет тендера (UAH) </summary>
        [JsonProperty("budget", Order = 11)]
        public int BudgetUAH { get; set; }

        /// <summary> Дата создания </summary>
        [JsonProperty("dateCreate", Order = 12)]
        public DateTime TenderCreatedAt { get; set; }

        /// <summary> Начало приема предложений </summary>
        [JsonProperty("dateStart", Order = 13)]
        public DateTime OffersAcceptingStartAt { get; set; }

        /// <summary> Окончание приема предложений </summary>
        [JsonProperty("dateEnd", Order = 14)]
        public DateTime OffersAcceptingEndAt { get; set; }

        /// <summary> Валюта тендера </summary>
        [JsonProperty("tenderCurrency", Order = 15)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ECurrencyType Currency { get; set; }

        /// <summary> Курс валюты к UAH </summary>
        [JsonProperty("tenderCurrencyRate", Order = 16)]
        public Double CurrencyRate { get; set; }

        /// <summary> Является ли тендер удачно завершенным </summary>
        [JsonProperty("successful", Order = 17)]
        public Boolean? TenderIsSuccessfulFinished { get; set; }

        /// <summary> Тип тура тендера </summary>
        [JsonProperty("stageMode", Order = 18)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ETenderRoundType TenderRoundType { get; set; }

        /// <summary> Тип публикации </summary>
        [JsonProperty("stageKind", Order = 19)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ETenderRoundAccessibility TenderRoundAccessibility { get; set; }

        /// <summary> Номер тура </summary>
        [JsonProperty("stageNumber", Order = 20)]
        public Int32 TenderRoundNumber { get; set; }

        /// <summary> Стадия процесса </summary>
        [JsonProperty("process", Order = 21)]
        public Int32 TenderProcessStage { get; set; }

        /// <summary> Ссылка на таблицу ценовых предложений </summary>
        [JsonProperty("offersEvaluationReportLink", Order = 21)]
        public string OffersEvaluationReportLink { get; set; }

        /// <summary> Паспорт тендера </summary>
        [JsonProperty("generalTerms", Order = 23)]
        public string TenderPassport { get; set; }
        
        /// <summary> Режим тендера </summary>
        [JsonProperty("mode", Order = 24)]
        public Int32 mode { get; set; }

        /// <summary> Тип тендера </summary>
        [JsonProperty("kind", Order = 25)]
        public Int32 kind { get; set; }
        public string tenderOwnerPath { get; set; }

        /// <summary> Лоты тендера </summary>
        [JsonProperty("lots", Order = 26)]
        public List<TenderLot> TenderLots { get; set; }





        /// <summary> Ссылка на протокол тендера </summary>
        [JsonProperty("protocolLink")]
        public String TenderProtocolLink { get; set; }

        /// <summary> Id субкомпании (в APS) </summary>
        [JsonProperty("subCompanyId")]
        public Int64? SubCompanyId { get; set; }

        /// <summary> Субкомпания </summary>
        [JsonProperty("subCompanyName")]
        public String SubCompanyName { get; set; }

        /// <summary> Id подразделения (в APS) </summary>
        [JsonProperty("depId")]
        public Int64? DepartmentId { get; set; }

        /// <summary> Подразделение </summary>
        [JsonProperty("depName")]
        public String DepartmentName { get; set; }

        /// <summary> Экономия (UAH) </summary>
        [JsonProperty("economy")]
        public Double EconomyUAH { get; set; }

        /// <summary> Экономия от 2-ой лучшей цены (UAH) </summary>
        [JsonProperty("economy2")]
        public Double Economy2UAH { get; set; }

        /// <summary> Стоимость решения (UAH) </summary>
        [JsonProperty("solutionCost")]
        public Double SolutionCostUAH { get; set; }

        /// <summary> Прикрепленные файлы </summary>
        [JsonProperty("ownerFiles")]
        public List<AttachedFile> OwnerFiles { get; set; }
    }


    /// <summary> Файл, прикрепленный автором тендера </summary>
    [JsonObject(Title = "ownerfile")]
    public class AttachedFile
    {
        /// <summary> Название файла </summary>
        [JsonProperty("fileName")]
        public string FileName { get; set; }

        /// <summary> Дата изменения </summary>
        [JsonProperty("dateChange")]
        public DateTime FileChangedAt { get; set; }

        /// <summary> Id пользователя внесшего изменения (в APS) </summary>
        [JsonProperty("userChangeId")]
        public Int64 ChangesAuthorId { get; set; }

        /// <summary> Guid файла </summary>
        [JsonProperty("fileUuid")]
        public Guid FileGuid { get; set; }

        /// <summary> Файл видим для контрагентов </summary>
        [JsonProperty("suppVisible")]
        public bool IsVisibleForSupplier { get; set; }
    }

}
