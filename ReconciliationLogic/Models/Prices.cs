using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReconciliationLogic.Models
{
    public class ItemPricesList : BaseClass
    {
        [XmlArray("ItemPricesList"), XmlArrayItem("ItemPrice")]
        public ItemPrice[] ItemPrice { get; set; }

        /// <summary>
        /// Converting an XML file to an objects list
        /// </summary>
        /// <returns></returns>
        public static async Task<List<ItemPrice>> GetItemPricesList()
        {          
            try
            {
                List<ItemPrice> PriceList = new List<ItemPrice>();
                await Task.Run(() =>
                {
                    string fileName = @"\Prices.xml";
                    string filename = AppPath + fileName;
                    XmlSerializer deserializer = new XmlSerializer(typeof(ItemPricesList), new XmlRootAttribute("ItemPricesRoot"));
                    using (StreamReader sReader = new StreamReader(filename))
                    {
                        ItemPricesList Prices = (ItemPricesList)deserializer.Deserialize(sReader);
                        PriceList = Prices.ItemPrice.ToList();
                    }
                        
                });
                return PriceList;
            }
            catch (Exception ex)
            {
                string ErrMessage = ex.Message;
                if (ex.InnerException.Message != null)
                    ErrMessage += " Inner Exeption: " + ex.InnerException;
                await MyLogger.WriteLog(ErrMessage);
                Console.WriteLine(ErrMessage);
                throw ex;                
            }          
        }
    }

}
