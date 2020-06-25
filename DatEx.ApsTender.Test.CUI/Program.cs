using System;
using System.Collections.Generic;
using System.Linq;
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
            F01_GetTendersInfo();
        }

        public static void F01_GetTendersInfo()
        {
            List<TenderState> tendersAndStates = ApsClient.GetTendersAndTheirStates();
            List<TenderStageInfo> tendersStageInfo = new List<TenderStageInfo>();

            //foreach(var page in tendersAndStates.Where(x => x.TenderNo > 468 && x.ProcessState != ETenderProcessStage.St9_TenderClosed).Paginate(10))
            foreach(var page in tendersAndStates.Where(x => x.TenderNo > 468).Paginate(10))
            {
                List<TenderStageInfo> tendersStageInfoPage = ApsClient.GetTendersStageInfo(page.Select(x => x.TenderNo).ToList());
                foreach(var tenderStageInfo in tendersStageInfoPage) Console.WriteLine(tenderStageInfo);
                tendersStageInfo.AddRange(tendersStageInfoPage);
            }
            Dictionary<Int32, TenderStageInfo> tenderStageInfoDict = tendersStageInfo.ToDictionary(k => k.TenderNo);

            foreach(var idsPage in tenderStageInfoDict.Keys.Paginate(10))
            {
                List<TenderData> tendersData = ApsClient.GetTendersData(idsPage.ToList()).ToList();
                foreach(TenderData tenderData in tendersData)
                {
                    Console.WriteLine();
                    Int32 membersCount = tenderStageInfoDict[tenderData.TenderNumber].TenderProcessStageMembers.Count;

                    if(tenderData == null) continue;
                    Console.WriteLine(tenderData);
                    Console.WriteLine($"   Участники стадии тендера ({membersCount} шт.):\n     " + String.Join("\n     ",
                        tenderStageInfoDict[tenderData.TenderNumber].TenderProcessStageMembers.Select(x => $" - {x}")));
                    
                    Console.WriteLine($"   Лоты тендера:\n");
                    foreach(var tenderLot in tenderData.TenderLots)
                    {
                        Console.WriteLine($"{tenderLot.ToString()}");

                        foreach(var item in tenderLot.LotItems.OrderBy(x => x.Name).ThenBy(x => x.NomenclatureId).ThenByDescending(x => x.Quantity))
                        {
                            Console.WriteLine($"{item.ToString()}");
                        }
                    }
                    
                }
            }
        }
    }
}
