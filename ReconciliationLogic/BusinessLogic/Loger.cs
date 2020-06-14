using System;
using System.IO;
using System.Threading.Tasks;

namespace ReconciliationLogic.BusinessLogic
{
    public class Logger : BaseClass
    {
        public async Task WriteLog(string message)
        {
            string LogPath = Path.Combine(AppPath, "ExecutionLog.log");
            try
            {
                await Task.Run(() =>
                {
                    using (StreamWriter sw = File.AppendText(LogPath))
                    {
                         sw.WriteLine(DateTime.Now + " | " + message);
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }

}
