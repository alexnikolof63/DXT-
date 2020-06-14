using Newtonsoft.Json;

namespace ReconciliationLogic.Models
{
    class PaymentsNotMatched : Payment
    {
        [JsonProperty(PropertyName = "AmountDue")]
        public decimal AmountDue { get; set; }

        [JsonProperty(PropertyName = "AmountPayd")]
        public decimal AmountPaid { get; set; }

    }
}
