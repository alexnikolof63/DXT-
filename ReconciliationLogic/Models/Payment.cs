using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ReconciliationLogic
{

    public class Payment: BaseClass
    {
        [JsonProperty(PropertyName = "Customer")]
        public int Customer { get; set; }

        [JsonProperty(PropertyName = "Year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "Month")]
        public int Month { get; set; }

        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount { get; set; }

        internal static async  Task<List<Payment>> GetPaiments()
        {
            string json = "";
            await Task.Run(() =>
            {
                json = File.ReadAllText(Path.Combine(AppPath, "Payments.json"));
            });
            return new JavaScriptSerializer().Deserialize<List<Payment>>(json);
        }
    }
}