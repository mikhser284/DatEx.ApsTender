﻿using DatEx.ApsTender.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace DatEx.ApsTender.DataModel
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
        public String CompanyName { get; set; }

        /// <summary> Id номенклатурной категории </summary>
        [JsonProperty("industryId", Order = 9)]
        public Int32 NomenclatureCategoryId { get; set; }

        /// <summary> Номенклатурная категория </summary>
        [JsonProperty("industryName", Order = 10)]
        public String NomenclatureCategoryName { get; set; }

        /// <summary> Бюджет тендера (UAH) </summary>
        [JsonProperty("budget", Order = 11)]
        public Double BudgetUAH { get; set; }

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
        public ETenderCurrencyType Currency { get; set; }

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
        public ETenderProcessStage TenderProcessStage { get; set; }

        /// <summary> Ссылка на таблицу ценовых предложений </summary>
        [JsonProperty("offersEvaluationReportLink", Order = 21)]
        public String OffersEvaluationReportLink { get; set; }

        /// <summary> Паспорт тендера </summary>
        [JsonProperty("generalTerms", Order = 23)]
        public String TenderPassport { get; set; }
        
        /// <summary> Режим тендера </summary>
        [JsonProperty("mode", Order = 24)]
        public Int32 Mode { get; set; }

        /// <summary> Тип тендера </summary>
        [JsonProperty("kind", Order = 25)]
        public Int32 Kind { get; set; }

        [JsonProperty("TenderOwnerPath", Order = 26)]
        public String TenderOwnerPath { get; set; }

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

        /// <summary> Лоты тендера </summary>
        [JsonProperty("lots", Order = 27)]
        public List<TenderLot> TenderLots { get; set; }

        public override String ToString() => ToString(0);


        public String ToString(Int32 indentLevel)
        {
            String indent = Ext_String.GetIndent(indentLevel);
            return $"{indent}ТЕНДЕР #{TenderNumber} от {TenderCreatedAt:yyyy.MM.dd HH:mm} ({AuthorName}) — { TenderName} [{NomenclatureCategoryName}];"
                + $"\n{indent}Тур #{TenderRoundNumber} ({TenderRoundAccessibility.AsString()}, {TenderRoundType.AsString()}),"
                + $"\n{indent}{TenderProcessStage.AsString()};"
                + $"\n{indent}----------------------------------------"
                + $"\n{indent}Компания: {CompanyName};"
                + $"\n{indent}Валюта: {Currency} ({CurrencyRate: 0.00});";
        }

        public void Show()
        {
            Int32 indentLevel1 = 0;
            Int32 indentLevel2 = indentLevel1 + 1;
            Int32 indentLevel3 = indentLevel2 + 1;
            String indent1 = Ext_String.GetIndent(indentLevel1);
            String indent2 = Ext_String.GetIndent(indentLevel2);
            String indent3 = Ext_String.GetIndent(indentLevel3);

            Console.WriteLine(ToString(indentLevel1));

            Console.WriteLine($"{indent1}■ Лоты тендера:");
            foreach(var lot in TenderLots)
            {
                Console.WriteLine(lot.ToString(indentLevel2));
                Console.WriteLine($"{indent2}■ Позиции лота ({lot.LotItems?.Count} шт.):");
                foreach(var lotItem in lot.LotItems)
                {
                    Console.WriteLine(lotItem.ToString(indentLevel3));
                }
            }            
        }

        public static TenderData Retrieve(ApsClient apsClient, Int32 tenderNumber)
        {
            var requestResult = apsClient.GetTenderData(tenderNumber);
            if(requestResult.IsSuccess == false) throw new InvalidOperationException($"Error: {requestResult.ErrorCode} — {requestResult.ErrorString}");
            return requestResult.Data;
        }

        public static TenderData RetrieveFullData(ApsClient apsClient, Int32 tenderNumber)
        {
            var requestResult = apsClient.GetTenderData(tenderNumber);
            if(requestResult.IsSuccess == false) throw new InvalidOperationException($"Error: {requestResult.ErrorCode} — {requestResult.ErrorString}");
            TenderData tenderData = requestResult.Data;
            List<TenderLotItemOffer> offers = apsClient.GetLotItemOffers(tenderData);
            
            return tenderData;
        }        
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
