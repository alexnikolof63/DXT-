using ReconciliationLogic.BusinessLogic;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                //To work properly change the AppConfig file AppSettings 
                //put the right path.
                //Base Path to Data Folder
                ReconciliationLogic.BaseClass.AppPath = ConfigurationManager.AppSettings.Get("AppPath");
                //Base Path to dta Folder of the WebApp
                ReconciliationLogic.BaseClass.JsonPath = ConfigurationManager.AppSettings.Get("JsonPath");

                //Start the Main Program
                Console.WriteLine("-- Starting Application --");
                PaymentsLogic PB = new PaymentsLogic();
                await PB.GetPayments();
                Console.WriteLine("-- The Application finished the execution. --");
                Console.WriteLine("-- A log file is created into he Data Folder. --");
                Console.WriteLine("-- Please press any buton to exit --");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " | " + ex.InnerException.Message);
                Console.WriteLine("-- Pres any buton to exit --");
                Console.ReadLine();
                await PaymentsLogic.MyLogger.WriteLog(ex.ToString());

                throw ex;
            }

        }
    }
}
