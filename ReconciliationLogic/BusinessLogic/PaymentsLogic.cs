using ReconciliationLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReconciliationLogic.BusinessLogic
{
    public class PaymentsLogic : BaseClass
    {
       
        List<PaymentsNotMatched> paymentsNotMatched = new List<PaymentsNotMatched>();
        List<ItemPrice> PricesList = new List<ItemPrice>();
        List<Payment> PaymentsList = new List<Payment>();
        List<Purchases> PurchasesList = new List<Purchases>();
        public PaymentsLogic()
        {
            
        }
        /// <summary>
        /// Main Method
        /// </summary>
        public async Task GetPayments()
        {
            await Task.Run(async () =>
            {
                //Reads from Purchasess.dat
                Console.WriteLine("Reads from Purchasess.dat");
                await MyLogger.WriteLog("Reads from Purchasess.dat");
                PurchasesList = await Purchases.GetPurchases();
                Console.WriteLine("Ok");
                await MyLogger.WriteLog("Done");
                //Reads fom Pricess XML
                Console.WriteLine("Reads from Pices.xml");
                await MyLogger.WriteLog("Reads from Pices.xml");
                PricesList = await ItemPricesList.GetItemPricesList();
                Console.WriteLine("Ok");
                await MyLogger.WriteLog("Done");
                //Reads from Payments Json
                Console.WriteLine("Reads from Payments.json");
                await MyLogger.WriteLog("Reads from Payments.json");
                PaymentsList = await Payment.GetPaiments();
                Console.WriteLine("Ok");
                await MyLogger.WriteLog("Done");

                Console.WriteLine("Start the PaymentsNotMatched Loop");
                await MyLogger.WriteLog("Start the PaymentsNotMatched Loop");
                foreach (var p in PurchasesList)
                {
                    PaymentsNotMatched n = new PaymentsNotMatched();
                    n.Customer = p.Customer;
                    n.Month = p.Month;
                    n.Year = p.Year;
                    n.AmountPaid = await GetAmoutPayd(p);
                    foreach (var i in p.Item)
                    {
                        //Sum the price to total to pay. 
                       n.Amount += await GetPriceForProductAsync(i.Id);
                    }

                    // Get the Due value
                    n.AmountDue = n.AmountPaid - n.Amount;

                    //include only unmatched payments
                    if (n.AmountDue != 0)
                        paymentsNotMatched.Add(n);
                }
                Console.WriteLine("Done");
                await MyLogger.WriteLog("Done");
                // Free the memmory
                PaymentsList.Clear();
                PurchasesList.Clear();
                //Sort the list 

                List<PaymentsNotMatched> payments = paymentsNotMatched.OrderByDescending(x => x.AmountDue).ToList();
                // Free the memmory
                paymentsNotMatched.Clear();

                //Save a json File in Data Folder.
                Console.WriteLine("Save PaymentsNotMatched.json File in the Data Folder.");
                await FileUtillities.WriteJasonToFile(payments,
                    AppPath + @"\PaymentsNotMatched.json");
                Console.WriteLine("Done");
                await MyLogger.WriteLog("Done");
                //Save Jason File for the WebApp in the dta Folder for the Web Application
                Console.WriteLine("Save Jason File for the WebApp in the dta Folder for the Web Application.");
                await FileUtillities.WriteJasonToFile(payments,
                    JsonPath + @"\PaymentsNotMatched.json");
                Console.WriteLine("Done");
                await MyLogger.WriteLog("Done");
                //Save a csv File
                Console.WriteLine("Save PaymentsNotMatched.csv File in the Data Folder.");
                await FileUtillities.CreateCSVFromGenericList(payments,
                    AppPath + @"\PaymentsNotMatched.csv");
                Console.WriteLine("Done");
                await MyLogger.WriteLog("Done");

                //Display the data in  a Web Page in  WebApp 
            });
        }
        /// <summary>
        /// Gets the Amount payd from the PaymentsList
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task<decimal> GetAmoutPayd(Purchases p)
        {
            decimal ret = 0;
            await Task.Run(async () =>
            {   
                var paid = PaymentsList.Where(x => x.Customer == p.Customer && x.Month == p.Month && x.Year == p.Year).FirstOrDefault();
                if (paid != null)
                    ret = paid.Amount;
                else
                {
                    string message = @"Payment not found in Payments list | Customer: " +
                        p.Customer + " | Year: " + p.Year + " | Moth:" + p.Month;
                   await MyLogger.WriteLog(message);
                    Console.WriteLine(message);
                }
               
            });
             return ret;
        }


        /// <summary>
        /// Gets the price forom the List PricesList
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<decimal> GetPriceForProductAsync(int id)
        {
            var ret = PricesList.Find(a => a.Item == id);
            
            if (ret != null)
                return ret.Price;
            else
            {
                string message = "Price not found in Prices list for Item: " + id ;
                await MyLogger.WriteLog(message);
                return 0;
            }
        }


    }
}
