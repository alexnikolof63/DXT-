using ReconciliationLogic.BusinessLogic;

namespace ReconciliationLogic
{
    public class BaseClass
    {
        public static string AppPath { get; set; }
        public static string JsonPath { get; set; }

        public static Logger MyLogger = new Logger();
       
    }
}
