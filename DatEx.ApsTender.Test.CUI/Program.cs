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

namespace DatEx.ApsTender.Test.CUI
{
    public enum TenderProcessState
    {
        Active,
        Finished
    }

    public class TenderState
    {
        public Int32 TenderNo { get; set; }
        public EStageOfTenderProcess ProcessState { get; set; }
    }

    class Program
    {
        static AppSettings AppConfig = AppSettings.Load();
        static ApsClient ApsClient = new ApsClient(AppConfig);

        static void Main(string[] args)
        {
            List<TenderState> tenders = new List<TenderState>();
            for(int i = 1; i <= 9; i++)
            {
                var requestResult = ApsClient.GetTendersOnStage((EStageOfTenderProcess)i);
                if(requestResult?.Data?.Count > 0) tenders.AddRange(requestResult.Data.Select(x => new TenderState { TenderNo = x.TenderId, ProcessState = (EStageOfTenderProcess)i }));
            }
            tenders = tenders.OrderBy(x => x.TenderNo).ToList();

            foreach(var tender in tenders) Console.WriteLine($"{tender.TenderNo,3} — {tender.ProcessState}");
        }
    }
}
