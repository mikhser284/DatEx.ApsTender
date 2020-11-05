using System;
using System.Collections.Generic;
using System.IO;
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
            Int32 tenderNumber = 483;


            EUserChoice userChiice = EUserChoice.Exit;

            do
            {
                userChiice = ShowMenu();

                switch(userChiice)
                {
                    case EUserChoice.Exit:
                        {
                            break;
                        }
                    case EUserChoice.GetTenderState:
                        {
                            GetTenderState(tenderNumber);
                            break;
                        }
                    case EUserChoice.GetTenderData:
                        {
                            GetTenderData(tenderNumber);
                            break;
                        }
                    case EUserChoice.GetTenderDataAndTenderRoundOffers:
                        {
                            GetTenderDataAndTenderRoundOffers(tenderNumber);
                            break;
                        }
                    case EUserChoice.GetTenderDataAndTenderRoundOffersAndContractorFiles:
                        {
                            GetTenderDataAndTenderRoundOffersAndContractorFiles(tenderNumber);
                            break;
                        }
                    case EUserChoice.GetTenderDocumentation:
                        {
                            GetTenderDocumentation(tenderNumber);
                            break;
                        }
                    case EUserChoice.SkipTaskOfSecurityService:
                        {
                            SkipTaskOfSecurityService(tenderNumber);
                            break;
                        }
                    case EUserChoice.SkipTaskOfTenderComitet:
                        {
                            SkipTaskOfTenderComitet(tenderNumber);
                            break;
                        }
                    case EUserChoice.ApplySolytionOfTenderComitet:
                        {
                            ApplySolytionOfTenderComitet(tenderNumber);
                            break;
                        }
                    default:
                        {
                            throw new InvalidOperationException("Недопустимая операция");
                        }
                }

            } while (userChiice != EUserChoice.Exit);
        }


        public static EUserChoice ShowMenu()
        {
            var values = Enum.GetValues(typeof(EUserChoice));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Выберите действие:");
            foreach(var value in values)
            {
                EUserChoice val = (EUserChoice)value;
                Console.WriteLine($" {(Int32)val:00}. {val.GetFriendlyName()}");
            }
            Console.WriteLine();

            Boolean valueExist = false;
            Int32 input;
            do
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Ожидание ввода: ");
                while(!Int32.TryParse(Console.ReadLine(), out input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nВы ввели не корректное значение");
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                foreach (var value in values)
                {
                    EUserChoice val = (EUserChoice)value;
                    if(input == (Int32)val)
                    {
                        valueExist = true;
                        break;
                    }
                }

                if(!valueExist)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nВведите значение из списка");
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

            } while (!valueExist);
            Console.ResetColor();
            Console.WriteLine();
            
            return (EUserChoice)input;
        }


        public static void GetTenderState(Int32 tenderNumber)
        {
            TenderStageInfo tenderStateBeforeApprovement = ApsClient.GetTenderStageInfo(tenderNumber);
            Console.WriteLine($"\n{tenderStateBeforeApprovement}\n");
        }
        

        public static void GetTenderData(Int32 tenderNumber)
        {
            RequestResult<TenderData> reqResult = ApsClient.GetTenderData(tenderNumber);
            if (!reqResult.IsSuccess)
            {
                Console.WriteLine("Не удалось получить информацию о состоянии тендера");
                return;
            }
            TenderData tenderData = reqResult.Data;

            tenderData.Show();
        }

        public static void GetTenderDataAndTenderRoundOffers(Int32 tenderNumber)
        {
            RequestResult<TenderData> reqResult = ApsClient.GetTenderData(tenderNumber);
            if (!reqResult.IsSuccess)
            {
                Console.WriteLine("Не удалось получить информацию о состоянии тендера");
                return;
            }
            TenderData tenderData = reqResult.Data;
            
            List<TenderLotItemOffer> offers = ApsClient.GetTenderRoundOffers(tenderData);

            tenderData.Show();
        }


        public static void GetTenderDataAndTenderRoundOffersAndContractorFiles(Int32 tenderNumber)
        {
            RequestResult<TenderData> reqResult = ApsClient.GetTenderData(tenderNumber);
            if (!reqResult.IsSuccess)
            {
                Console.WriteLine("Не удалось получить информацию о состоянии тендера");
                return;
            }
            TenderData tenderData = reqResult.Data;
            ApsClient.GetTenderRoundOffers(tenderData);
            tenderData.Show();
            
            String workDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\_Data\Tender #{tenderNumber}"));
            if (!Directory.Exists(workDir)) Directory.CreateDirectory(workDir);

            ApsClient.GetFilesFromCommertialOffers(tenderData, workDir);
            Console.WriteLine($"\n\nФайлы контрагентов сохранены в папку \"{workDir}\"\n");
        }


        public static void GetTenderDocumentation(Int32 tenderNumber)
        {
            Console.WriteLine("<Не реализовано>");
        }


        public static void SkipTaskOfSecurityService(Int32 tenderNumber)
        {
            RequestResult<TenderData> reqResult = ApsClient.GetTenderData(tenderNumber);
            if (!reqResult.IsSuccess)
            {
                Console.WriteLine("Не удалось получить информацию о состоянии тендера");
                return;
            }
            TenderData tenderData = reqResult.Data;

            TenderStageInfo tenderStateBeforeApprovement = ApsClient.GetTenderStageInfo(tenderNumber);
            TenderStageInfo tenderStateAfterApprovement = ApsClient.SkipTask(tenderData, SkippableTask.St6_ConclusionOfSecurityService);

            Console.WriteLine($"\nСостояние процесса тендера было изменено: {tenderStateBeforeApprovement != tenderStateAfterApprovement}");
            Console.WriteLine($"\nСостояние тендера после пропуска задачи:\n{tenderStateAfterApprovement}");
        }


        public static void SkipTaskOfTenderComitet(Int32 tenderNumber)
        {
            RequestResult<TenderData> reqResult = ApsClient.GetTenderData(tenderNumber);
            if (!reqResult.IsSuccess)
            {
                Console.WriteLine("Не удалось получить информацию о состоянии тендера");
                return;
            }
            TenderData tenderData = reqResult.Data;

            TenderStageInfo tenderStateBeforeApprovement = ApsClient.GetTenderStageInfo(tenderNumber);
            TenderStageInfo tenderStateAfterApprovement = ApsClient.SkipTask(tenderData, SkippableTask.St8_ApprovementOfTenderCommittee);
            
            Console.WriteLine($"\nСостояние процесса тендера было изменено: {tenderStateBeforeApprovement != tenderStateAfterApprovement}");
            Console.WriteLine($"\nСостояние тендера после пропуска задачи:\n{tenderStateAfterApprovement}");
        }


        public static void ApplySolytionOfTenderComitet(Int32 tenderNumber)
        {
            Console.WriteLine("<Не реализовано>");
        }
    }
}
