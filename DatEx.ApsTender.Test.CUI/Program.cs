using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using DatEx.ApsTender.DataModel;
using DatEx.ApsTender.Helpers;
using static DatEx.ApsTender.ApsClient;

namespace DatEx.ApsTender.Test.CUI
{
    class Program
    {
        static AppSettings AppConfig = AppSettings.Load();
        static ApsClient ApsClient = new ApsClient(AppConfig);

        static void Main(string[] args)
        {
            RequestResult<TenderData> requestResult = ApsClient.GetTenderData(468);
            TenderData tenderData = requestResult?.Data;
            if(!requestResult.IsSuccess)
            {
                Console.WriteLine($"Не удалось получить информацию о состоянии тенедера:\n{requestResult.ErrorString}");
                return;
            }
            TenderStageInfo tenderStageInfoBefore = ApsClient.GetTenderStageInfo(tenderData.TenderNumber);
            Console.WriteLine($"Before approvement:\n{tenderStageInfoBefore}\n\n");

            TenderStageInfo tenderStageInfoAfter = ApsClient.SkipApprovementOfSecurityService(tenderData);
            Console.WriteLine($"\n\nAfter approvement:\n{tenderStageInfoAfter}\n\n");

            Console.WriteLine($"Stage was changed: {tenderStageInfoBefore != tenderStageInfoAfter}");

            //F01_GetTendersInfo();
        }

        public static void F01_GetTendersInfo()
        {
            List<TenderState> tendersAndStates = ApsClient.GetTendersAndTheirStates();
            List<TenderStageInfo> tendersStageInfo = new List<TenderStageInfo>();

            //foreach(var page in tendersAndStates.Where(x => x.TenderNo > 468 && x.ProcessState != ETenderProcessStage.St9_TenderClosed).Paginate(10))
            foreach(var page in tendersAndStates.Where(x => x.TenderNo == 468).Paginate(10))
            {
                List<TenderStageInfo> tendersStageInfoPage = ApsClient.GetTendersStageInfo(page.Select(x => x.TenderNo).ToList());
                foreach(var tenderStageInfo in tendersStageInfoPage) Console.WriteLine(tenderStageInfo.ToString(0));
                tendersStageInfo.AddRange(tendersStageInfoPage);
            }

            return;

            Dictionary<Int32, TenderStageInfo> tenderStageInfoDict = tendersStageInfo.ToDictionary(k => k.TenderNo);

            foreach(var idsPage in tenderStageInfoDict.Keys.Paginate(10))
            {
                List<TenderData> tendersData = ApsClient.GetTendersData(idsPage.ToList()).ToList();
                foreach(TenderData tenderData in tendersData)
                {
                    Console.WriteLine();
                    Int32 membersCount = tenderStageInfoDict[tenderData.TenderNumber].TenderProcessStageMembers.Count;

                    if(tenderData == null) continue;
                    Console.WriteLine(tenderData.ToString(0));
                    Console.WriteLine($"Участники стадии тендера ({membersCount} шт.):\n" + String.Join("\n",
                        tenderStageInfoDict[tenderData.TenderNumber].TenderProcessStageMembers.Select(x => $" - {x}")));
                    
                    Console.WriteLine($"   Лоты тендера:\n");
                    foreach(var tenderLot in tenderData.TenderLots)
                    {
                        Console.WriteLine($"{tenderLot.ToString()}");

                        Console.WriteLine($"   Критерии лота:\n");
                        foreach(TenderCriteria item in tenderLot.LotCriteria)
                        {
                            Console.WriteLine($"      - {item.ToString()}");
                        }

                        Console.WriteLine($"   Позиции лота:\n");
                        foreach(TenderLotItem item in tenderLot.LotItems.OrderBy(x => x.Name).ThenBy(x => x.NomenclatureId).ThenByDescending(x => x.Quantity))
                        {
                            Console.WriteLine($"{item.ToString()}");
                            item.RetreiveOffers(ApsClient);
                            foreach(TenderLotItemOffers offer in item.Offers)
                            {
                                Console.WriteLine(offer.ToString(1));
                                foreach(TenderCriteriaAnswer criteriaAnswer in offer.TenderCriteriaAnswers)
                                {
                                    Console.WriteLine(criteriaAnswer.ToString(2));
                                }
                            }
                        }
                    }                    
                }
            }
        }
    }
}
