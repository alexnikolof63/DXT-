using System.Xml.Serialization;

namespace ReconciliationLogic.Models
{
    public class ItemPrice
    {
        [XmlElement("Item")]
        public int Item { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }

}
