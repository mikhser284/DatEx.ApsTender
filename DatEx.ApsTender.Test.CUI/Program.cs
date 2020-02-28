using System;
using System.Collections.Generic;
using System.Linq;
using DatEx.ApsTender.DataModel;
using DatEx.ApsTender.Helpers;
using static DatEx.ApsTender.ApsClient;

namespace DatEx.ApsTender.Test.CUI
{
    [System.Runtime.InteropServices.Guid("2EE98138-547F-4322-8B4E-F3F680ED7482")]
    class Program
    {
        static AppSettings AppConfig = AppSettings.Load();
        static ApsClient ApsClient = new ApsClient(AppConfig);

        static void Main(string[] args)
        {
            //var tenderData = ApsClient.GetTenderData(447);

            //return; // ———————————————————————————————————


            List<TenderState> tendersAndStates = ApsClient.GetTendersAndTheirStates();
            List<TenderStageInfo> tendersStageInfo = new List<TenderStageInfo>();

            foreach(var page in tendersAndStates.Where(x => x.ProcessState != ETenderProcessStage.St9_TenderClosed).Paginate(20))
            {
                List<TenderStageInfo> tendersStageInfoPage = ApsClient.GetTendersStageInfo(page.Select(x => x.TenderNo).ToList());
                foreach(var tenderStageInfo in tendersStageInfoPage) Console.WriteLine(tenderStageInfo);
                tendersStageInfo.AddRange(tendersStageInfoPage);
            }
            Dictionary<Int32, TenderStageInfo> tenderStageInfoDict = tendersStageInfo.ToDictionary(k => k.TenderNo);

            List<TenderData> tendersData = new List<TenderData>();
            foreach(var item in tendersStageInfo)
            {
                Console.WriteLine();
                var res = ApsClient.GetTenderData(item.TenderNo);
                
                if(res?.Data == null) continue;
                Console.WriteLine(res.Data);
                Console.WriteLine("   Stage members:\n     " + String.Join("\n     ", tenderStageInfoDict[item.TenderNo].TenderProcessStageMembers));
                tendersData.Add(res.Data);
            }

            Console.WriteLine("End");
        }
    }
}
