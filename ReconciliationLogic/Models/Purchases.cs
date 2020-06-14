using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReconciliationLogic.Models
{
    public class Purchases : BaseClass
    {
        public Purchases()
        {
            this.Item = new List<Item>();
        }
        public int Customer { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public virtual ICollection<Item> Item { get; set; }
        public static async Task<List<Purchases>> GetPurchases()
        {
            List<Purchases> PurchasesList = new List<Purchases>();
            try
            {
                await Task.Run(() =>
                {

                    StreamReader objInput = new StreamReader(AppPath + "\\Purchases.dat", System.Text.Encoding.Default);
                    string contents = objInput.ReadToEnd().Trim();
                    string[] ArrPurchases = System.Text.RegularExpressions.Regex.Split(contents, "\\s+", RegexOptions.None);
                    Purchases p = new Purchases();
                    int level = 0;
                    foreach (string s in ArrPurchases)
                    {
                        if (!String.IsNullOrWhiteSpace(s))
                        {
                            //Console.WriteLine(s);
                            if (s.StartsWith("CUST"))
                            {
                                if (level == 1)
                                {
                                    PurchasesList.Add(p);
                                    p = new Purchases();
                                    level = 0;
                                }
                                else
                                {
                                    level = 1;
                                }
                                var i = s.Substring(4);
                                p.Customer = Convert.ToInt32(i);
                            }

                            if (s.StartsWith("DATE"))
                            {
                                var y = s.Substring(8, 4);
                                p.Year = Convert.ToInt32(y);
                                var m = s.Substring(6, 2);
                                p.Month = Convert.ToInt32(m);
                            }

                            if (s.StartsWith("ITEM"))
                            {
                                string id = s.Substring(4);
                                Item item = new Item
                                {
                                    Id = Convert.ToInt32(id)
                                };
                                p.Item.Add(item);
                            }

                        }

                    }
                });
                return PurchasesList;
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
