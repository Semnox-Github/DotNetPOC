/********************************************************************************************
 * Project Name - TransactionCore
 * Description  - TransactionCore object of TransactionCore
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        15-june-2016   Rakshith          Created 
 *2.70        25-Mar-2019      Guru S A       Booking Phase 2 enhancements
 *2.100       08-Aug-2020    Mathew Ninan     Print time capture in Transaction 
 *2.130.1     01-Dec-2021    Nitin Pai        Receipt Email details are not calculated correctly
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Device.Printer;
using Semnox.Parafait.Printer;
using Semnox.Parafait.POS;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// class Transactions
    /// </summary>
    public class TransactionCore
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
       
        /// <summary>
        /// Saves transaction 
        /// </summary>
        /// <param name="orderedProducts">orderedProducts</param>
        /// <param name="transcationParamStructDTO">transcationParamStructDTO</param>
        /// <returns> returns list of TransactionKeyValueStruct</returns>
        public List<TransactionKeyValueStruct> SaveTransaction(TransactionParams transactionParams, List<LinkedPurchaseProductsStruct> orderedProducts)
        {
            TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler(); 
            return transactionCoreDatahandler.SaveTransaction(transactionParams, orderedProducts);
        }


        /// <summary>
        /// Save and GetTransaction details
        /// </summary>
        /// <param name="transactionParams"></param>
        /// <param name="orderedProducts"></param>
        /// <returns></returns>
        public List<TransactionDetails> SaveAndGetTransaction(TransactionParams transactionParams, List<LinkedPurchaseProductsStruct> orderedProducts)
        {
            List<TransactionDetails> TransactionDetailsList = new List<TransactionDetails>();
            List<TransactionKeyValueStruct> trxList = SaveTransaction(transactionParams, orderedProducts);
            foreach (TransactionKeyValueStruct transactionKeyValueStruct in trxList)
            {
                if (transactionKeyValueStruct.Key == "TransactionId")
                {
                    int trxId = -1;
                    int.TryParse(transactionKeyValueStruct.Value, out trxId);
                    if (trxId > -1)
                    {
                        PurchasesListParams purchasesListParams = new PurchasesListParams();
                        purchasesListParams.TranscationId = trxId;
                        purchasesListParams.ShowAllTransaction = true;
                        purchasesListParams.ShowTransactionLines = true;
                        purchasesListParams.LoginId = transactionParams.LoginId;
                        purchasesListParams.UserId = transactionParams.UserId;
                        transactionParams.TrxId = trxId;
                        TransactionDetailsList = GetPurchaseTransactions(purchasesListParams);
                    }
              
                }

            }
            return TransactionDetailsList;
        }


        /// <summary>
        /// Updates transaction 
        /// </summary>
        /// <param name="transcationParamStructDTO">transcationParamStructDTO</param>
        /// <returns> returns TransactionStatus</returns>
        public TransactionStatus UpdateTransaction(TransactionParams transactionParams)
        {
            TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler();
            return transactionCoreDatahandler.UpdateTransaction(transactionParams);
        }


        /// <summary>
        /// Confirm Purchase Transaction updating the payment details 
        /// </summary>
        /// <param name="transcationParamStructDTO">transcationParamStructDTO</param>
        /// <returns> returns list of TransactionKeyValueStruct</returns>
        public TransactionStatus ConfirmPurchaseTransaction(TransactionParams transactionParams)
        {
            TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler(); 
            return transactionCoreDatahandler.ConfirmPurchaseTransaction(transactionParams);

        }



        /// <summary>
        /// Cancel Purchase Transaction
        /// </summary>
        /// <param name="transcationParamStructDTO">transcationParamStructDTO</param>
        /// <returns> returns list of TransactionKeyValueStruct</returns>
        public TransactionStatus CancelPurchaseTransaction(TransactionParams transactionParams)
        {
            TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler();
            return transactionCoreDatahandler.CancelPurchaseTransaction(transactionParams);

        }

        /// <summary>
        /// Get Purchases
        /// </summary>
        /// <param name="purchasesListParams"></param>
        /// <returns> returns list of Transaction and Transcation Line details based on the parameters</returns>
        public List<TransactionDetails> GetPurchaseTransactions(PurchasesListParams purchasesListParams, bool showByCreationDate = false)
        {
            TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler();
            return transactionCoreDatahandler.GetPurchaseTransactions(purchasesListParams, showByCreationDate);
        }


        public List<TransactionDetails> GetTicketTransactionDetailsList(List<TransactionDetails> orginalTransactionDetailsList)
        {
            List<TransactionDetails> listTransactionDetails = new List<TransactionDetails>();
            orginalTransactionDetailsList[0].Products = orginalTransactionDetailsList[0].Products.Where(c => (c.ProductType == "ATTRACTION" || c.ProductType == "NEW" ||
            c.ProductType == "RECHARGE" || c.ProductType == "CARDSALE" || c.ProductType == "VARIABLECARD" || c.ProductType == "GAMETIME" || c.ProductType == "VOUCHER" || c.ProductType == "COMBO")).ToList();
            List<LinkedPurchasedProducts> parentLinkedPurchasedProductsList = orginalTransactionDetailsList[0].Products.Where(c => c.LinkedLineIdentifier.Equals("-1")).ToList();

            foreach (LinkedPurchasedProducts parentProduct in parentLinkedPurchasedProductsList)
            {
                List<LinkedPurchasedProducts> childLinkedPurchasedProductsList = orginalTransactionDetailsList[0].Products.Where(c => c.LinkedLineIdentifier == parentProduct.ProductLineIdentifier).ToList();
                foreach (LinkedPurchasedProducts childProduct in childLinkedPurchasedProductsList)
                {
                    if (parentProduct.ProductLineIdentifier == childProduct.LinkedLineIdentifier)
                    {
                        childProduct.ProductName = childProduct.ProductName + "/" + parentProduct.ProductName;
                    }
                }
            }
            orginalTransactionDetailsList[0].Products = orginalTransactionDetailsList[0].Products.Where(c => (c.ProductType != "COMBO")).ToList();

            return orginalTransactionDetailsList;
        }

        public List<TransactionDetails> GetComboTransactionDetailsList(List<TransactionDetails> orginalTransactionDetailsList)
        {
            int lineNo = 1;

            //List<TransactionDetails> orginalTransactionDetailsList = GetPurchaseTransactions(purchasesListParams);
            List<TransactionDetails> comboTransactionDetailsList = new List<TransactionDetails>();

            if (orginalTransactionDetailsList.Count() == 0 || orginalTransactionDetailsList[0].Products == null)
            {
                return orginalTransactionDetailsList;
            }

            List<LinkedPurchasedProducts> parentLinkedPurchasedProductsList = orginalTransactionDetailsList[0].Products.Where(c => c.LinkedLineIdentifier.Equals("-1") && c.ProductType != "DISCOUNT").ToList();


            List<LinkedPurchasedProducts> mergedLinkedPurchasedProductsList = new List<LinkedPurchasedProducts>();
            bool childproductFound = false;
            bool productFound = false;

            foreach (LinkedPurchasedProducts parentProduct in parentLinkedPurchasedProductsList)
            {
                List<LinkedPurchasedProducts> childLinkedPurchasedProductsList = orginalTransactionDetailsList[0].Products.Where(c => c.LinkedLineIdentifier == parentProduct.ProductLineIdentifier).ToList();

                int ParentComboCount = parentLinkedPurchasedProductsList.Where(c => c.ProductId == parentProduct.ProductId && c.ProductType == "COMBO" && c.LinkedLineIdentifier == "-1").Count();
                int PriceInclCount = parentLinkedPurchasedProductsList.Where(c => c.ProductId == parentProduct.ProductId && c.ProductType == "COMBO" && c.Amount == 0 && c.LinkedLineIdentifier == "-1").Count();

                if (ParentComboCount > 0)
                {
                    if (parentLinkedPurchasedProductsList.Where(c => c.ProductId == parentProduct.ProductId && c.ProductType == "COMBO" && c.LinkedLineIdentifier == "-1").Count() > 0)
                    {
                        parentProduct.TotalAmount = parentLinkedPurchasedProductsList.Where(c => c.ProductId == parentProduct.ProductId && c.ProductType == "COMBO" && c.LinkedLineIdentifier == "-1").Sum(c => c.Amount);
                    }

                    if (parentLinkedPurchasedProductsList.Where(c => c.ProductId == parentProduct.ProductId && c.ProductType == "COMBO" && c.LinkedLineIdentifier == "-1" && c.Price > 0).Count() > 0)
                    {
                        parentProduct.Price = parentLinkedPurchasedProductsList.Where(c => c.ProductId == parentProduct.ProductId && c.ProductType == "COMBO" && c.LinkedLineIdentifier == "-1" && c.Price > 0).First().Price;
                    }

                    parentProduct.Quantity = ParentComboCount;
                    String prdSuffix = string.Empty;
                    if (orginalTransactionDetailsList.Count > 1 && orginalTransactionDetailsList[0].BookingId > 0)
                    {
                        if (PriceInclCount > 0)
                        {
                            prdSuffix = " ( " + MessageContainerList.GetMessage(machineUserContext, " Price Incl.Quantity: ");
                            prdSuffix += PriceInclCount.ToString() + " )";
                        }
                    }
                    parentProduct.ProductName = parentProduct.ProductName + prdSuffix;
                }

                //Normal products   
                if (childLinkedPurchasedProductsList.Count == 0)
                {
                    foreach (LinkedPurchasedProducts newProduct in mergedLinkedPurchasedProductsList)
                    {
                        if ((parentProduct.ProductId == newProduct.ProductId && newProduct.LinkedLineIdentifier == "-1" && parentProduct.ProductType != "ATTRACTION") ||
                            (parentProduct.ProductId == newProduct.ProductId && parentProduct.AttractionScheduleId == newProduct.AttractionScheduleId && newProduct.LinkedLineIdentifier == "-1"))
                        {
                            productFound = true;
                            newProduct.Quantity = newProduct.Quantity + parentProduct.Quantity;
                            newProduct.Price = newProduct.Price;
                            newProduct.TotalAmount = newProduct.Price * newProduct.Quantity;
                            newProduct.ProductDescription = parentProduct.ProductName;
                        }
                    }
                }

                // Checking for parent products
                if (childLinkedPurchasedProductsList.Count > 0)
                {
                    foreach (LinkedPurchasedProducts newProduct in mergedLinkedPurchasedProductsList)
                    {
                        if (parentProduct.ProductId == newProduct.ProductId && newProduct.LinkedLineIdentifier == "-1")
                        {
                            productFound = true;
                        }
                    }
                }
                if (productFound)
                {
                    productFound = false;
                }
                else
                {
                    parentProduct.ProductLineIdentifier = lineNo.ToString();
                    parentProduct.LinkedLineIdentifier = "-1";
                    parentProduct.ProductDescription = "";
                    parentProduct.Price = Math.Round(parentProduct.Price, 2); 
                    //parentProduct.TotalAmount = Math.Round(parentProduct.TotalAmount, 2);

                    if (parentProduct.ProductType == "DISCOUNT")
                        parentProduct.ProductName = parentProduct.ProductName + " (" + parentProduct.ProductType + ")";

                    lineNo++;
                    mergedLinkedPurchasedProductsList.Add(parentProduct);
                }


                //For child Products
                foreach (LinkedPurchasedProducts childProduct in childLinkedPurchasedProductsList)
                {
                    foreach (LinkedPurchasedProducts newProduct in mergedLinkedPurchasedProductsList)
                    {
                        if (childProduct.ProductId == newProduct.ProductId)
                        {
                            LinkedPurchasedProducts parentSearch = mergedLinkedPurchasedProductsList.Where(x => x.ProductLineIdentifier == newProduct.LinkedLineIdentifier).FirstOrDefault();
                            if (parentSearch != null && parentProduct.ProductId == parentSearch.ProductId)
                            {
                                if (childProduct.ProductType == "ATTRACTION")
                                {
                                    if (childProduct.AttractionScheduleId != newProduct.AttractionScheduleId)
                                    {
                                        continue;
                                    }
                                }

                                childproductFound = true;
                                newProduct.Quantity = newProduct.Quantity + childProduct.Quantity;
                                newProduct.Price = newProduct.Price;
                                //newProduct.TotalAmount = newProduct.Price * newProduct.Quantity;
                                newProduct.ProductDescription = parentProduct.ProductName;
                            }
                        }
                    }
                    if (childproductFound)
                    {
                        childproductFound = false;
                    }
                    else
                    {
                        childProduct.ProductLineIdentifier = lineNo.ToString();
                        childProduct.LinkedLineIdentifier = parentProduct.ProductLineIdentifier;
                        lineNo++;

                        if (orginalTransactionDetailsList[0].BookingId <= 0)  // Added for Reservation to exclude
                            mergedLinkedPurchasedProductsList.Add(childProduct);
                    }
                }
            }

            comboTransactionDetailsList.Add(orginalTransactionDetailsList[0]);
            comboTransactionDetailsList[0].Products = mergedLinkedPurchasedProductsList;

            return comboTransactionDetailsList;
        }

        public string GetProductGroupCount(List<LinkedPurchasedProducts> prodlist)
        {

            //var teamTotalScores = from player in prodlist
            //                      where player.ProductType != "combo"
            //                      group player by player.ProductName into playerGroup
            //                      select new
            //                      {
            //                          GroupName = playerGroup.Key,
            //                          GroupCount = playerGroup.Sum(x => x.Quantity),
            //                      };


            //string str = string.Empty;
            //foreach (var item in teamTotalScores)
            //    str = str + item.GroupName + " : " +item.GroupCount + " | " ;

            //if (str.Length > 3 )
            //    str = str.Remove(str.Length - 3);

            //return str;

            string productGroup = "";
            string comboProductGroup = "";


            if (prodlist != null)
            {
                bool includeCombo = false;
                foreach (LinkedPurchasedProducts searchedlinkedPurchasedProduct in prodlist)
                {

                    //For child product
                    if (searchedlinkedPurchasedProduct.ProductType.ToLower() != "combo")
                    {
                        string seperator = string.IsNullOrEmpty(productGroup) ? "" : " / ";
                        int productCount = 0;
                        foreach (LinkedPurchasedProducts mainlinkedPurchasedProducts in prodlist)
                        {
                            if (mainlinkedPurchasedProducts.ProductName == searchedlinkedPurchasedProduct.ProductName)
                                productCount += mainlinkedPurchasedProducts.Quantity;
                        }
                        productGroup += seperator + searchedlinkedPurchasedProduct.ProductName + ":" + productCount;
                    }
                    //For Combo product Also checking for price >0 
                    else if (searchedlinkedPurchasedProduct.ProductType.ToLower() == "combo")
                    {
                        includeCombo = searchedlinkedPurchasedProduct.Price > 0 ? true : false;
                        if (includeCombo)
                        {
                            string seperator = string.IsNullOrEmpty(productGroup) ? "" : " / ";
                            int productCount = 0;
                            foreach (LinkedPurchasedProducts mainlinkedPurchasedProducts in prodlist)
                            {
                                if (mainlinkedPurchasedProducts.ProductName == searchedlinkedPurchasedProduct.ProductName)
                                    productCount += mainlinkedPurchasedProducts.Quantity;
                            }
                            comboProductGroup += seperator + searchedlinkedPurchasedProduct.ProductName + ":" + productCount;
                        } 
                    } 
                }

                if (includeCombo)
                {
                    productGroup = comboProductGroup;
                }

            }
            return productGroup;


        }

        /// <summary>
        /// Get Transaction list
        /// </summary>
        /// <param name="lastRundate"></param>
        /// <param name="siteId"></param>
        /// <returns> returns list of Transaction based on the parameter</returns>
        public List<TransactionDetails> GetTransactionsByDateTime(DateTime lastRundate,DateTime currentRunTime, int siteId)
        {
            log.LogMethodEntry(lastRundate, currentRunTime, siteId);
            TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler();
            List<TransactionDetails> transactionDetailsList = transactionCoreDatahandler.GetTransactionsByDateTime(lastRundate, currentRunTime, siteId);
            log.LogMethodExit(transactionDetailsList);
            return transactionDetailsList;
        }



        /// <summary>
        ///  GetCardBalance
        /// </summary>
        /// < param name="loginId"">loginId</param>
        /// <returns> returns double balance</returns>
        public double GetCardBalance(string loginId)
        {
            try
            {
                TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler();
                DataTable cardtable = transactionCoreDatahandler.GetCardDetails(loginId);
                if (cardtable.Rows.Count > 0)
                {
                    int cardId = Convert.ToInt32(cardtable.Rows[0]["card_id"]);
                    machineUserContext.SetSiteId(Convert.ToInt32(cardtable.Rows[0]["site_id"]));
                    AccountDTO accountDTO = new AccountBL(machineUserContext, cardId).AccountDTO;
                    double balance = Convert.ToDouble(accountDTO.AccountSummaryDTO.CreditPlusCardBalance) + Convert.ToDouble(cardtable.Rows[0]["credits"]);
                    return balance;
                }
                else
                    return -1;
            }
            catch
            {
                return -2;
            }

        }


        /// <summary>
        /// Send syncronous message method
        /// </summary>
        /// <param name="number">number</param>
        /// <param name="message">message</param>
        /// <returns> returns string</returns>
        public string SendMessage(string number, string message, int siteId)
        {
            try
            {
                TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler();
                return transactionCoreDatahandler.SendMessage(number, message, siteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Method to Get TransactionReceipt base64 string
        /// <param name="trxId">It takes TrxId as parameter to fetch details and transaction receipts.</param>
        /// <param name="macAddress">macId to identify the POS machine</param>
        /// <param name="width,height">height and width ofthe receipt image</param>
        /// <returns>Returns base64 string of trx receipt based on passed params </returns>
        /// </summary>
        public string GetTransactionReceipt(int trxId, string macAddress, int width, int height, bool secondaryPrint = false)
        {
            try
            {
                TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler();
                return transactionCoreDatahandler.GetTransactionReceipt(trxId, macAddress, width, height, secondaryPrint);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// get transaction by trx id
        /// </summary>
        /// <param name="trxId"> transaction id</param>
        /// <param name="macAddress">pos mac id</param>
        /// <returns>returns transaction object</returns>
        public Transaction GetTransactionByTrxId(int trxId, string macAddress)
        {
            try
            {
                TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler();
                return transactionCoreDatahandler.GetTransactionByTrxId(trxId, macAddress);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// get transaction status by trx id
        /// </summary>
        /// <param name="trxId"></param>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        public string GetTransactionStatusByTrxId(int trxId, string macAddress)
        {
            try
            {
                TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler();
                return transactionCoreDatahandler.GetTransactionStatusByTrxId(trxId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///  if tablet printer is configured then this will return the base64 string of the configured reciept template 
        ///  else it will print receipt in configured network printer
        ///  if both are not found then it will return an exception
        /// </summary>
        /// <param name="trxId">transaction id</param>
        /// <param name="macAddress">pos mac address</param>
        /// <param name="tabletPrinterMac">printer mac address</param>
        /// <param name="width">receipt paper width</param>
        /// <param name="height">receipt paper height</param>
        /// <returns>key value struct of the receipt template base64 string or status message of print</returns>
        public List<TransactionKeyValueStruct> PrintTrxReceipt(int trxId, string macAddress, string tabletPrinterMac, int width, int height)
        {
            try
            {
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.IP_ADDRESS, macAddress));
                POSMachineList posMachineList = new POSMachineList(machineUserContext);
                List<POSMachineDTO> posMachines = posMachineList.GetAllPOSMachines(searchParameters);
                POSMachines posMachine = new POSMachines(machineUserContext, posMachines[0].POSMachineId);
                List<POSPrinterDTO> posPrintersDTOList = posMachine.PopulatePrinterDetails();

                List<POSPrinterDTO> printers = new List<POSPrinterDTO>();
                POSPrinterListBL printerBLList = new POSPrinterListBL(machineUserContext);
                printers = printerBLList.getPrinterList(macAddress);
                List<TransactionKeyValueStruct> printDetails = new List<TransactionKeyValueStruct>();
                /*
                 * when no printers are configured and this method get  called it will get the printers list withdefault printer
                 * in case the webservice is invoked by tablet then webservice will try to execute the method by fetching default printer configured in system 
                 * and try to show the print popup which will lead to an exception.
                 * so the default printer will be removed from the list
                 * in case of the default printer the printer id is returned as -1 so the following condition will do the removing default printer.
                */
                if ((printers.Count > 0) && (printers[0].PrinterId == -1))
                    printers.RemoveAt(0);

                if (printers.Count > 0)
                {
                    POSPrinterDTO configuredPrinter = null;
                    if (!String.IsNullOrEmpty(tabletPrinterMac))
                        configuredPrinter = printers.FirstOrDefault(PrinterDTO => PrinterDTO.PrinterLocation.Equals(tabletPrinterMac));

                    if (configuredPrinter != null)
                    {
                        string receiptEncodedString = GetTransactionReceipt(trxId, macAddress, width, height);
                        printDetails.Add(new TransactionKeyValueStruct("RECEIPT_TEMPLATE", receiptEncodedString));

                        if(configuredPrinter.SecondaryPrinter != null && configuredPrinter.SecondaryPrinter.PrinterId != -1)
                        {
                            string receiptEncodedString1 = GetTransactionReceipt(trxId, macAddress, width, height, true);
                            printDetails.Add(new TransactionKeyValueStruct("RECEIPT_TEMPLATE_AC_COPY", receiptEncodedString1));
                        }
                    }
                    else
                    {
                        Transaction currTransaction = GetTransactionByTrxId(trxId, macAddress);
                        currTransaction.PrintStartTime = DateTime.Now;
                        PrintTransaction printTransaction = new PrintTransaction(posPrintersDTOList);
                        string message = null;
                        bool printKOTStatus = printTransaction.Print(currTransaction, ref message);
                        if (printKOTStatus == false)
                            throw new Exception(message);
                        currTransaction.UpdateTrxHeaderSavePrintTime(trxId, null, null, currTransaction.PrintStartTime, DateTime.Now);
                        printDetails.Add(new TransactionKeyValueStruct("RECEIPT_PRINTED", "Receipt printed successfully"));
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(tabletPrinterMac))
                        throw new Exception("No printers configured. Please configure");
                    else
                        throw new Exception("Receipt template for " + tabletPrinterMac + " not configured. Please configure");

                }
                return printDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



		/// <summary>
		/// SaveAttractionBooking - Method used to save or remove attraction booking 
		/// </summary>
		/// <param name="AttractionBookingDTOList"></param>
		/// <returns></returns>
		public List<AttractionBookingDTO> SaveAttractionBooking(int siteId, List<AttractionBookingDTO> AttractionBookingDTOList)
		{
			TransactionCoreDatahandler transactionCoreDatahandler = new TransactionCoreDatahandler();
			return transactionCoreDatahandler.SaveAttractionBooking(siteId, AttractionBookingDTOList);
		}


	}
}

