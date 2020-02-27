using DatEx.ApsTender.Test.CUI.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using DatEx.ApsTender;
using DatEx.ApsTender.DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Threading.Tasks;
using static DatEx.ApsTender.ApsClient;

namespace DatEx.ApsTender.Test.CUI
{
    class Program
    {
        static AppSettings AppConfig = AppSettings.Load();
        static ApsClient ApsClient = new ApsClient(AppConfig);

        static void Main(string[] args)
        {
            Stopwatch watchB = new Stopwatch();
            watchB.Start();
            List<TenderState> tendersAndStates = ApsClient.GetTendersAndTheirStates();
            watchB.Stop();
            Console.WriteLine($"{watchB.Elapsed}");            

            foreach(var tender in tendersAndStates.Where(x => x.ProcessState == ETenderProcessStage.St6_OffersProcessingApprovement)) Console.WriteLine($"{tender.TenderNo,3} — {tender.ProcessState}");

            var stageInfo = ApsClient.GetTenderStageInfo(372);
        }
    }
}
