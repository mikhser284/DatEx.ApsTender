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

namespace DatEx.ApsTender.Test.CUI
{
    class Program
    {
        static AppSettings AppConfig = AppSettings.Load();
        static ApsClient ApsClient = new ApsClient(AppConfig);

        static void Main(string[] args)
        {
            var tenderData = ApsClient.GetTenderData(419);

            
            Console.WriteLine(tenderData.Data.TenderNumber);
        }
    }
}
