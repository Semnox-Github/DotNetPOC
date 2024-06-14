/********************************************************************************************
 * Project Name - TransactionCoreDatahandler
 * Description  - TransactionCoreDatahandler object of TransactionCoreDatahandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        15-june-2016   Rakshith          Created 
 *2.50.0      15-Jan-2019   Mathew Ninan      Deprecated StaticDataExchange class 
 *2.50.0      28-Jan-2019   Guru S A          Booking changes
 *2.70.0      1-Jul-2019    Lakshminarayana     Modified to add support for ULC cards 
 *2.70.0      18-Jul-2019   Mathew             Added logic to handle roaming data for card 
 *2.70        25-Mar-2019   Guru S A          Booking Phase 2 enhancements
 *2.70        18-Mar-2019   Akshay Gulaganji  Modified usedCouponDTO.IsActive (from string to bool)
 *2.70        06-Jul-2019   Akshay G          Merged from Development to Web branch
 *2.70.2      18-Dec-2019   Jinto Thomas      Added parameter execution context for userbl declaration with userid 
 *2.70.3      30-Mar-2020   Jeevan            Booking attende fixes , populate attendee list 
 *2.70.3      30-Mar-2020   Jeevan            Attraction Fixes for pass price , In case of transaction with only recharge product OTP made null
 *2.80.0      26-May-2020   Dakshakh          CardCount enhancement for product type Cardsale/GameTime/Attraction/Combo 
 *2.80.0      15-Jun-2020   Jeevan            Changes for payment refunds
 *2.120.7     10-May-2022   Nitin             Enhancement: Allow recharge of linked and child cards in website
 *2.120.7     10-May-2022   Nitin             Fix: Website is closing transactions with partial payment 
 *2.140.5     15-Feb-2023   Muaaz Musthafa    Added Payment date, Card credit type and number to GetPurchasedTransaction() to show on receipt
 *2.150.2     13-Mar-2023   Muaaz Musthafa    Fix: To handle time offset for Daylight saving time scenario 
 *2.140.5     13-Jun-2023   Ashish Sreejith   Modified SavePurchaseTransaction() to set price of B2B attraction price from price list
 *2.1550.0    17-Aug-2023   Muaaz Musthafa    Modified: SavePurchaseTransaction() to set primary card for debit card payment to calculate cardDiscount and ccp consumption
 *2.152.0     09-Jan-2024   Guruprasad Kodaja Modified: GetPosPrinterId() modified to check for valid receipt printer only.
  *********************************************************************************************/

//using POSCore;
//using Semnox.Core.HR.Users;
//using Semnox.Parafait.Products;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

using Semnox.Parafait.Booking;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Waiver;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using Semnox.Parafait.DBSynch;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Customer;
using Semnox.Parafait.POS;
using Semnox.Parafait.Device.PaymentGateway;
using Newtonsoft.Json;
using Semnox.Parafait.Printer;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// class TransactionCoreDatahandler
    /// </summary>
    public class TransactionCoreDatahandler : IDisposable
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        Utilities parafaitUtility;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        string connstring;
        TransactionStatus transactionStatus;

        TimeZoneUtil timeZoneUtil;

        private readonly TagNumberLengthList tagNumberLengthList;

        /// <summary>
        /// Default constructor of  TransactionCoreDatahandler class
        /// </summary>
        public TransactionCoreDatahandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            connstring = dataAccessHandler.ConnectionString;
            parafaitUtility = new Utilities(connstring);
            parafaitUtility.ParafaitEnv.IsCorporate = checkIsCorporate();
            if (parafaitUtility.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(parafaitUtility.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }

            timeZoneUtil = new TimeZoneUtil();
            tagNumberLengthList = new TagNumberLengthList(machineUserContext);
            log.LogMethodExit();
        }



        /// <summary>
        /// Saves transaction 
        /// </summary>
        /// <param name="orderedProducts">orderedProducts</param>
        /// <param name="transcationParamStructDTO">transcationParamStructDTO</param>
        /// <returns> returns list of TransactionKeyValueStruct</returns>
        public List<TransactionKeyValueStruct> SaveTransaction(TransactionParams transactionParams, List<LinkedPurchaseProductsStruct> inOrderedProducts)
        {

            List<TransactionCoreLineLinker> transactionLineList = new List<TransactionCoreLineLinker>();
            List<TransactionKeyValueStruct> transactionDetails = new List<TransactionKeyValueStruct>();

            List<LinkedPurchaseProductsStruct> orderedProducts = new List<LinkedPurchaseProductsStruct>();
            List<LinkedPurchaseProductsStruct> ComboProductsList = new List<LinkedPurchaseProductsStruct>();
            string message = "";

            System.Text.StringBuilder sbLog = new System.Text.StringBuilder();

            try
            {

                // Validate Login and assign user Id /Role Id
                UsersDTO userDTO = ValidateLogin(transactionParams.LoginId, transactionParams.SiteId);

                transactionParams.UserId = userDTO.UserId;
                transactionParams.RoleId = userDTO.RoleId;

                // Validate Card and assign Card Number
                ValidateCard(transactionParams);

                parafaitUtility.ParafaitEnv.SiteId = transactionParams.SiteId;
                parafaitUtility.ParafaitEnv.LoginID = transactionParams.LoginId;
                parafaitUtility.ParafaitEnv.User_Id = transactionParams.UserId;
                parafaitUtility.ParafaitEnv.RoleId = transactionParams.RoleId;

                if (parafaitUtility.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(parafaitUtility.ParafaitEnv.SiteId);
                    machineUserContext.SetIsCorporate(parafaitUtility.ParafaitEnv.IsCorporate);
                }

                int offSetDuration = 0;
                double businessDayStartTime = 6;
                double.TryParse(parafaitUtility.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);

                if (transactionParams.ApplySystemVisitDate == false)
                {
                    transactionParams.VisitDate = transactionParams.VisitDate.AddHours(businessDayStartTime + 1);
                    offSetDuration = timeZoneUtil.GetOffSetDuration(transactionParams.SiteId, transactionParams.VisitDate.Date);
                    transactionParams.VisitDate = transactionParams.VisitDate.AddSeconds(offSetDuration);
                }

                // Moved timezone hours apply to Update Transcation method to Handle Card Expiry issue Apr-20-2017

                machineUserContext.SetUserId(parafaitUtility.ParafaitEnv.LoginID);
                parafaitUtility.ParafaitEnv.SetPOSMachine("", transactionParams.PosIdentifier);
                if (parafaitUtility.ParafaitEnv.POSMachineId == -1)
                {
                    throw new Exception("POS is not Registered for online Transaction");
                }

                parafaitUtility.ParafaitEnv.Initialize();

                if (transactionParams.ForceIsCorporate)
                {
                    parafaitUtility.ParafaitEnv.IsCorporate = true;
                    log.Info("ParafaitEnv Forced to Corporate");
                }

                string getDepositProductQuery = @"select product_id productId
                                                                    from products x, product_type y
                                                                    where x.product_type_id = y.product_type_id
                                                                     and y.product_type = 'CARDDEPOSIT'
                                                                     and (x.site_id IS NULL OR x.site_Id = @siteId)";

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@siteId", parafaitUtility.ParafaitEnv.SiteId.ToString()));
                DataTable dtDepositProduct = dataAccessHandler.executeSelectQuery(getDepositProductQuery, sqlParams.ToArray());

                int depositProductId;
                try
                {
                    depositProductId = Convert.ToInt32(dtDepositProduct.Rows[0]["productId"]);
                }
                catch (Exception ex)
                {
                    throw new Exception("Deposit product setup is incorrect. Please set it up properly. " + ex.Message);
                }

                parafaitUtility.ParafaitEnv.CardDepositProductId = depositProductId;

                CustomerDTO customerDTO = new CustomerDTO();

                Transaction currTransaction;

                //if (transactionParams.TrxId > 0)
                //            {
                //                // BEGIN - Modifed to Update a  existing transaction OPEN Transaction 

                //                TransactionUtils trxUtils = new TransactionUtils(parafaitUtility);
                //                currTransaction = new Transaction(parafaitUtility);
                //                currTransaction = trxUtils.CreateTransactionFromDB(transactionParams.TrxId, parafaitUtility);

                //	if (currTransaction.Status != Transaction.TrxStatus.OPEN)
                //	{
                //		throw new Exception("Only OPEN Transaction allowed to be updated ");
                //	}
                //	else
                //	{
                //		//Remove Existing Lines from the OrderedProducts
                //		CheckAndRemoveFromOrderedProducts(transactionParams, ref inOrderedProducts);
                //	}

                //	// ENDS - Modifed to Update a  existing transaction OPEN Transaction
                //}
                //            else
                //            {
                currTransaction = new Transaction(parafaitUtility);
                //            }


                if (transactionParams.CustomerId >= 0)
                {
                    // BEGIN Added for Site DB Web compatability changes
                    ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
                    executionContext.SetSiteId(-1);
                    // EOF Added for Site DB Web compatability changes

                    CustomerBL customerBL = new CustomerBL(executionContext, transactionParams.CustomerId);
                    customerDTO = customerBL.CustomerDTO;

                    if (customerDTO == null || customerDTO.Id == -1)
                        throw new Exception("Customer does not exist.");
                    currTransaction.customerDTO = customerDTO;

                    executionContext.SetSiteId(transactionParams.SiteId);
                    if (parafaitUtility.ParafaitEnv.IsCorporate)
                        currTransaction.Utilities.ExecutionContext.SetSiteId(transactionParams.SiteId);
                }

                // Starts - Adde to update EntitlementReferenceDate and Transcation date as Visit date
                if (transactionParams.VisitDate != DateTime.MinValue)
                {
                    currTransaction.TrxDate = transactionParams.VisitDate;
                    currTransaction.EntitlementReferenceDate = transactionParams.VisitDate;
                }

                currTransaction.ApplyBookingDatePromotionPrice = true;

                if (transactionParams.WaiverSigningMode == "ONLINE")
                    currTransaction.WaiverSignedMode = WaiverSigningMode.ONLINE;

                bool isAlohaEnv = false;
                bool createChildLines = false; //Added boolean flag to avoid manual product creation in COMBO products.

                // Collection of product ids and cards, if cardcount of a product is set as 1, then it should be loaded to same card
                Dictionary<int, string> cardsForProducts = new Dictionary<int, string>();
                List<Transaction.ComboCardProduct> allCardProductList = new List<Transaction.ComboCardProduct>();
                // Start: Handle Combo Products 
                foreach (LinkedPurchaseProductsStruct linkdPSProductsStruct in inOrderedProducts)
                {
                    //linkdPSProductsStruct.LinkLineId = (linkdPSProductsStruct.LinkLineId == 0 ? -1 : linkdPSProductsStruct.LinkLineId);
                    message = linkdPSProductsStruct.Remarks;

                    if (!string.IsNullOrEmpty(linkdPSProductsStruct.ProductType))
                    {
                        if (linkdPSProductsStruct.ProductType == "COMBO")
                        {
                            Transaction.TransactionLine parentTrxLine = new Transaction.TransactionLine();
                            List<Transaction.ComboCardProduct> cardProductList = new List<Transaction.ComboCardProduct>();
                            List<KeyValuePair<int, int>> CategoryProductList = new List<KeyValuePair<int, int>>();
                            IEnumerable<LinkedPurchaseProductsStruct> childProducts = inOrderedProducts.Where(c => c.LinkLineId.Equals(linkdPSProductsStruct.PurchaseLineId)).ToList();

                            // list list of attraction products from the list
                            IEnumerable<LinkedPurchaseProductsStruct> AttractionProductsList = inOrderedProducts.Where(c => c.LinkLineId.Equals(linkdPSProductsStruct.PurchaseLineId) &&
                                                                                                                            c.ComboProductType != null && c.ComboProductType.Equals(ProductTypeValues.ATTRACTION)).ToList();
                            IEnumerable<LinkedPurchaseProductsStruct> categoryProducts = inOrderedProducts.Where(c => c.LinkLineId.Equals(linkdPSProductsStruct.PurchaseLineId) &&
                                                                                                                            c.ProductType.ToUpper().Equals("COMBOCATEGORY")).ToList();
                            if (categoryProducts.Any())
                            {
                                sbLog.Append("Combo category");

                                if (childProducts.Count() <= 0)
                                    throw new Exception("Error! Cannot add combo products");

                                foreach (LinkedPurchaseProductsStruct linkedProduct in childProducts)
                                {
                                    if (linkdPSProductsStruct.PurchaseLineId == linkedProduct.LinkLineId && (linkedProduct.ComboProductType == null || !linkedProduct.ComboProductType.Equals(ProductTypeValues.ATTRACTION)))
                                    {
                                        CategoryProductList.Add(new KeyValuePair<int, int>(linkedProduct.ProductId, linkedProduct.ProductQuantity));
                                    }
                                }
                                int flag = currTransaction.CreateComboProduct(linkdPSProductsStruct.ProductId, -1, linkdPSProductsStruct.ProductQuantity, ref message, parentTrxLine, cardProductList, CategoryProductList, false);
                                if (flag != 0)
                                    throw new Exception("Error! Cannot add combo category products - " + message);
                            }
                            else if (childProducts.Any())
                            {
                                sbLog.Append("Combo card");
                                childProducts = inOrderedProducts.Where(c => c.LinkLineId.Equals(linkdPSProductsStruct.PurchaseLineId) &&
                                                                                                                    (c.ComboProductType == null || !c.ComboProductType.Equals(ProductTypeValues.ATTRACTION))).ToList();

                                bool hasGroupedComboProds = childProducts.Where(x => x.IsGroupedCombo == true).Count() > 0 ? true : false;
                                int groupedComboIndex = 0;
                                //RandomTagNumber randomTNumber = new RandomTagNumber(parafaitUtility.ExecutionContext, tagNumberLengthList);
                                foreach (LinkedPurchaseProductsStruct linkedProduct in childProducts)
                                {
                                    RandomTagNumber randomTNumber = new RandomTagNumber(parafaitUtility.ExecutionContext, tagNumberLengthList);
                                    int totalQuantity = linkdPSProductsStruct.ProductQuantity * linkedProduct.ProductQuantity;
                                    if (linkdPSProductsStruct.PurchaseLineId == linkedProduct.LinkLineId)
                                    {
                                        Transaction.ComboCardProduct comboCardProduct = new Transaction.ComboCardProduct();
                                        comboCardProduct.ChildProductId = linkedProduct.ProductId;
                                        comboCardProduct.Quantity = linkedProduct.ProductQuantity;
                                        comboCardProduct.ComboProductId = linkdPSProductsStruct.ProductId;
                                        comboCardProduct.ChildProductType = linkedProduct.ProductType;

                                        List<string> cardList = new List<string>();
                                        Products products = new Products(linkedProduct.ProductId);
                                        Products lPSProductsStruct = new Products(linkdPSProductsStruct.ProductId);
                                        for (int i = 0; i < totalQuantity; i++)
                                        {
                                            if (hasGroupedComboProds && linkedProduct.IsGroupedCombo == false)
                                            {
                                                // For Grouped Combo Assign from Main product 
                                                cardList.Add(cardProductList[groupedComboIndex].CardNumbers[0].ToString());
                                                linkedProduct.CardNumber = cardProductList[groupedComboIndex].CardNumbers[0].ToString();
                                                groupedComboIndex++;
                                            }
                                            else if (lPSProductsStruct.GetProductsDTO.LoadToSingleCard.Equals(true))
                                            {
                                                if (allCardProductList.Exists(p => p.ComboProductId == linkdPSProductsStruct.ProductId))
                                                {
                                                    Transaction.ComboCardProduct matchingComboCardProduct = allCardProductList.FirstOrDefault(p => p.ComboProductId == linkdPSProductsStruct.ProductId);
                                                    cardList.Add(matchingComboCardProduct.CardNumbers[0]);
                                                }
                                                if (cardList.Count <= 0)
                                                {
                                                    linkedProduct.CardNumber = linkedProduct.AutoGenerateCardNumber == "Y" ? randomTNumber.Value : "T" + randomTNumber.Value.Substring(1);
                                                    cardList.Add(linkedProduct.CardNumber);
                                                }
                                            }
                                            else if (products.GetProductsDTO.LoadToSingleCard.Equals(true))
                                            {
                                                Transaction.ComboCardProduct childConsolidateProductList = allCardProductList.FirstOrDefault(p => p.ChildProductId == products.GetProductsDTO.ProductId);
                                                if (childConsolidateProductList != null)
                                                {
                                                    cardList.Add(childConsolidateProductList.CardNumbers[0]);
                                                }
                                                else
                                                {
                                                    linkedProduct.CardNumber = linkedProduct.AutoGenerateCardNumber == "Y" ? randomTNumber.Value : "T" + randomTNumber.Value.Substring(1);
                                                    cardList.Add(linkedProduct.CardNumber);
                                                }
                                            }
                                            else
                                            {
                                                RandomTagNumber randomTagNumber = new RandomTagNumber(parafaitUtility.ExecutionContext, tagNumberLengthList);
                                                linkedProduct.CardNumber = linkedProduct.AutoGenerateCardNumber == "Y" ? randomTagNumber.Value : "T" + randomTagNumber.Value.Substring(1);
                                                cardList.Add(linkedProduct.CardNumber);
                                            }
                                        }
                                        comboCardProduct.CardNumbers = cardList;
                                        cardProductList.Add(comboCardProduct);
                                        allCardProductList.Add(comboCardProduct);
                                    }
                                }
                                int flag = currTransaction.CreateComboProduct(linkdPSProductsStruct.ProductId, -1, linkdPSProductsStruct.ProductQuantity, ref message, parentTrxLine, cardProductList, CategoryProductList, false);
                                if (flag != 0)
                                {
                                    throw new Exception("Error! Cannot add combo card products - " + message);
                                }

                                if (childProducts.Where(x => x.IsGroupedCombo == true).Count() > 0)
                                {
                                    foreach (LinkedPurchaseProductsStruct linkedProduct in childProducts)
                                    {
                                        if (linkedProduct.IsGroupedCombo && !string.IsNullOrEmpty(linkedProduct.CardNumber))
                                        {
                                            if (currTransaction.TrxLines.Where(x => x.ProductID == linkedProduct.ProductId && x.CardNumber == linkedProduct.CardNumber).Count() == 1)
                                            {
                                                if (childProducts.Where(x => x.CardNumber == linkedProduct.CardNumber && x.ProductId != linkedProduct.ProductId).Count() == 1)
                                                {
                                                    string groupedProdName = childProducts.Where(x => x.CardNumber == linkedProduct.CardNumber && x.ProductId != linkedProduct.ProductId).FirstOrDefault().ProductName;
                                                    currTransaction.TrxLines.Where(x => x.ProductID == linkedProduct.ProductId && x.CardNumber == linkedProduct.CardNumber).FirstOrDefault().Remarks = groupedProdName;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int flag = currTransaction.CreateComboProduct(linkdPSProductsStruct.ProductId, -1, linkdPSProductsStruct.ProductQuantity, ref message, parentTrxLine, cardProductList, CategoryProductList);
                                if (flag != 0)
                                {
                                    throw new Exception("Error! Cannot add combo products - " + message);
                                }
                            }
                            // if the category combo or regular combo contains attraction products, then add these products to list
                            if (AttractionProductsList.Any())
                            {
                                RandomTagNumber rTagNumber = new RandomTagNumber(parafaitUtility.ExecutionContext, tagNumberLengthList);
                                string randomTagNumberForParentLTSC = rTagNumber.Value;
                                foreach (LinkedPurchaseProductsStruct currProduct in AttractionProductsList)
                                {
                                    Card currCard = null;
                                    if ((currProduct.CardNumber != null) && (currProduct.CardNumber.Length > 0))
                                    {
                                        currCard = new Card(currProduct.CardNumber, transactionParams.LoginId, parafaitUtility);
                                        if (currCard.customer_id < 0 && customerDTO.Id > 0)
                                        {
                                            currCard.customerDTO = customerDTO;
                                        }
                                    }

                                    if (currProduct.AttractionBookingList != null && currProduct.AttractionBookingList.Any())
                                    {
                                        // Added for Attraction booking 
                                        List<Card> cardList = new List<Card>();

                                        if (currCard != null)
                                        {
                                            cardList.Add(currCard);
                                        }

                                        Products products = new Products(currProduct.ProductId);
                                        if (cardList != null && currProduct.ProductQuantity != cardList.Count)
                                        {
                                            // build product dto here
                                            // check oif load to single is true
                                            // get the card number from transaction
                                            // if card is null and load to single is true, generate card number here
                                            // use it below
                                            Products parentProduct = new Products(linkdPSProductsStruct.ProductId);
                                            bool loadToSingle = products.GetProductsDTO.LoadToSingleCard || parentProduct.GetProductsDTO.LoadToSingleCard;
                                            string attCardNumber = "";
                                            if (loadToSingle)
                                            {
                                                attCardNumber = currTransaction.GetConsolidateCardFromTransaction(products.GetProductsDTO, linkdPSProductsStruct.ProductId, parentProduct.GetProductsDTO.LoadToSingleCard);
                                                if (String.IsNullOrEmpty(attCardNumber))
                                                {
                                                    RandomTagNumber randomTagNumber = new RandomTagNumber(parafaitUtility.ExecutionContext, tagNumberLengthList);
                                                    attCardNumber = products.GetProductsDTO.AutoGenerateCardNumber == "Y" ? randomTagNumber.Value : "T" + randomTagNumber.Value.Substring(1);
                                                }
                                            }
                                            while (currProduct.ProductQuantity != cardList.Count)
                                            {
                                                if (loadToSingle)
                                                {
                                                    currCard = new Card(attCardNumber, transactionParams.LoginId, parafaitUtility);

                                                }
                                                else
                                                {
                                                    RandomTagNumber randomTagNumber = new RandomTagNumber(parafaitUtility.ExecutionContext, tagNumberLengthList);
                                                    string cardNum = currProduct.AutoGenerateCardNumber == "Y" ? randomTagNumber.Value : "T" + randomTagNumber.Value.Substring(1);
                                                    currCard = new Card(cardNum, transactionParams.LoginId, parafaitUtility);
                                                    if (products.GetProductsDTO.LoadToSingleCard)
                                                    {
                                                        if (!cardsForProducts.ContainsKey(currProduct.ProductId))
                                                        {
                                                            cardsForProducts.Add(currProduct.ProductId, cardNum);
                                                        }
                                                    }
                                                }
                                                if (currCard != null && currCard.customer_id < 0 && customerDTO.Id > 0)
                                                {
                                                    currCard.customerDTO = customerDTO;
                                                }

                                                cardList.Add(currCard);
                                            }
                                        }

                                        int flag = CreateAttractionLine(currProduct, transactionParams, currTransaction, cardList, ref message, offSetDuration);
                                        if (flag != 0)
                                        {
                                            throw new Exception("Error! Cannot add attraction products - " + message);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            sbLog.Append("Combo link temp card " + linkdPSProductsStruct.LinkLineId.ToString());
                            if (linkdPSProductsStruct.LinkLineId == -1)
                            {
                                // Added to handle recharge products
                                if (linkdPSProductsStruct.ProductType.ToUpper() == "RECHARGE" || linkdPSProductsStruct.ProductType.ToUpper() == "VARIABLECARD")
                                {
                                    if (linkdPSProductsStruct.CardNumber == null || linkdPSProductsStruct.CardNumber == "")
                                    {
                                        throw new Exception("Error! Card Number Not valid for Recharge ");
                                    }
                                }
                                else
                                {
                                    Products products = new Products(linkdPSProductsStruct.ProductId);
                                    if (products.GetProductsDTO.ProductType == ProductTypeValues.ATTRACTION && products.GetProductsDTO.LoadToSingleCard && cardsForProducts.ContainsKey(linkdPSProductsStruct.ProductId))
                                    {
                                        if (string.IsNullOrEmpty(linkdPSProductsStruct.CardNumber))
                                        {
                                            linkdPSProductsStruct.CardNumber = cardsForProducts[linkdPSProductsStruct.ProductId];
                                        }
                                    }
                                    else if ((products.GetProductsDTO.ProductType == ProductTypeValues.CARDSALE
                                          || products.GetProductsDTO.ProductType == ProductTypeValues.GAMETIME
                                          ) && products.GetProductsDTO.LoadToSingleCard
                                          && cardsForProducts.ContainsKey(linkdPSProductsStruct.ProductId))
                                    {
                                        if (string.IsNullOrEmpty(linkdPSProductsStruct.CardNumber))
                                        {
                                            linkdPSProductsStruct.CardNumber = cardsForProducts[linkdPSProductsStruct.ProductId];
                                        }
                                    }
                                    else
                                    {
                                        RandomTagNumber randomTagNumber = new RandomTagNumber(parafaitUtility.ExecutionContext, tagNumberLengthList);
                                        if (string.IsNullOrEmpty(linkdPSProductsStruct.CardNumber))
                                        {
                                            linkdPSProductsStruct.CardNumber = linkdPSProductsStruct.AutoGenerateCardNumber == "Y" ? randomTagNumber.Value : "T" + randomTagNumber.Value.Substring(1);
                                            if (!cardsForProducts.ContainsKey(linkdPSProductsStruct.ProductId) && products.GetProductsDTO.LoadToSingleCard)//Added LTSC check5-13-2020
                                            {
                                                cardsForProducts.Add(linkdPSProductsStruct.ProductId, linkdPSProductsStruct.CardNumber);
                                            }
                                        }
                                    }
                                }

                                orderedProducts.Add(linkdPSProductsStruct);
                                sbLog.Append("Combo link temp card  gen");
                            }
                        }
                    }
                    else
                    {
                        sbLog.Append("Combo link temp card normal");
                        orderedProducts.Add(linkdPSProductsStruct);
                    }

                }

                //List<SqlParameter> insertCustomerParameters = new List<SqlParameter>();
                //insertCustomerParameters.Add(new SqlParameter("@log", sbLog.ToString()));
                //int idOfRowInserted = dataAccessHandler.executeInsertQuery("insert into dblog (logdesc) values (@log) SELECT CAST(scope_identity() AS int)", insertCustomerParameters.ToArray());

                //END

                foreach (LinkedPurchaseProductsStruct currProduct in orderedProducts)
                {
                    Card currCard = null;
                    if ((currProduct.CardNumber != null) && (currProduct.CardNumber.Length > 0))
                    {
                        currCard = new Card(currProduct.CardNumber, transactionParams.LoginId, parafaitUtility);
                        if (currCard.customer_id < 0 && customerDTO.Id > 0)
                        {
                            currCard.customerDTO = customerDTO;
                        }
                    }

                    // if the product is a recharge product, set the primary card of the transaction as the primary card of the customer
                    // this is required in scenarios where a customer is recharging a linked or child's card
                    if (currTransaction.PrimaryCard == null && currCard.CardStatus != "NEW" &&
                        currCard.customer_id != transactionParams.CustomerId && transactionParams.CustomerId >= 0)
                    {
                        //Get the primary card for the customer here and assign it as primary card of the transaction here.
                        AccountListBL accountListBL = new AccountListBL(parafaitUtility.ExecutionContext);
                        List<KeyValuePair<AccountDTO.SearchByParameters, string>> customerSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                        customerSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));
                        customerSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.REFUND_FLAG, "N"));
                        customerSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.TECHNICIAN_CARD, "N"));
                        customerSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, transactionParams.CustomerId.ToString()));
                        List<AccountDTO> customerCards = accountListBL.GetAccountDTOList(customerSearchParameters, false, true, null);

                        if (customerCards != null && customerCards.Any())
                        {
                            AccountDTO primaryCard = customerCards.FirstOrDefault(x => x.PrimaryAccount == true);
                            if (primaryCard != null && primaryCard.AccountId > -1 && (primaryCard.ExpiryDate == null || primaryCard.ExpiryDate > ServerDateTime.Now))
                            {
                                currTransaction.PrimaryCard = new Card(primaryCard.AccountId, transactionParams.LoginId, parafaitUtility);
                            }
                            else
                            {
                                customerCards = customerCards.OrderBy(x => x.IssueDate).ToList();
                                foreach (AccountDTO tempCard in customerCards)
                                {
                                    if (currTransaction.PrimaryCard == null && (tempCard.ExpiryDate == null || tempCard.ExpiryDate > ServerDateTime.Now))
                                        currTransaction.PrimaryCard = new Card(customerCards[0].AccountId, transactionParams.LoginId, parafaitUtility);
                                }
                            }
                        }
                    }

                    //Set the primary card of the transaction as the primary card of the customer to check whether ccp consumption present or not
                    if (currTransaction.PrimaryCard == null && !string.IsNullOrWhiteSpace(transactionParams.PaymentCardNumber))
                    {
                        Card primaryCard = new Card(transactionParams.PaymentCardNumber, transactionParams.LoginId, parafaitUtility);
                        currTransaction.PrimaryCard = primaryCard;
                    }

                    if (currProduct.LinkLineId == -1)
                    {
                        double amount = -1;
                        if (currProduct.Amount > 0)
                        {
                            if ((currCard != null) && (currCard.CardStatus == "NEW"))
                            {
                                //string getDepositProductQuery = @"select product_id productId
                                //                                    from products x, product_type y
                                //                                    where x.product_type_id = y.product_type_id
                                //                                     and y.product_type = 'CARDDEPOSIT'
                                //                                     and x.site_Id = @siteId";

                                //List<SqlParameter> sqlParams = new List<SqlParameter>();
                                //sqlParams.Add(new SqlParameter("@siteId", parafaitUtility.ParafaitEnv.SiteId.ToString()));

                                //DataTable dtDepositProduct = dataAccessHandler.executeSelectQuery(getDepositProductQuery, sqlParams);

                                //int depositProductId;
                                //try
                                //{
                                //    depositProductId = Convert.ToInt32(dtDepositProduct.Rows[0]["productId"]);
                                //}
                                //catch (Exception ex)
                                //{
                                //    throw new Exception("Deposit product setup is incorrect. Please set it up properly. " + ex.Message);
                                //}
                                bool isDepositProductPresent = false;
                                for (int j = 0; j < orderedProducts.Count && isDepositProductPresent == false; j++)
                                {
                                    if ((depositProductId == orderedProducts[j].ProductId) && (orderedProducts[j].CardNumber.Equals(currProduct.CardNumber, StringComparison.OrdinalIgnoreCase)))
                                        isDepositProductPresent = true;
                                }
                                if (isDepositProductPresent == false)
                                {
                                    Transaction.TransactionLine depositOutTransactionLine = new Transaction.TransactionLine(); //creating object of type TransactionLine. Output for ParentLineId
                                    int depositLineSaveStatus = currTransaction.createTransactionLine(currCard, depositProductId, 0, 1, ref message, depositOutTransactionLine, createChildLines);
                                    depositOutTransactionLine.Remarks = "";
                                }
                            }
                        }

                        if ((isAlohaEnv == false) || (currProduct.Amount == 0))
                        {
                            if (currProduct.Amount > 0)
                                amount = currProduct.Amount;
                            else
                                amount = -1;

                            int lineSaveStatus = -1;
                            if (currProduct.AttractionBookingList != null && currProduct.AttractionBookingList.Any())
                            {
                                Products currentProduct = new Products(currProduct.ProductId);

                                // Added for Attraction booking 
                                List<Card> cardList = new List<Card>();
                                if (currCard != null)
                                {
                                    cardList.Add(currCard);
                                }

                                Products products = new Products(currProduct.ProductId);
                                if (cardList != null && currProduct.ProductQuantity != cardList.Count)
                                {
                                    while (currProduct.ProductQuantity != cardList.Count)
                                    {
                                        if (products.GetProductsDTO.LoadToSingleCard && cardsForProducts.ContainsKey(currProduct.ProductId))
                                        {
                                            currCard = new Card(cardsForProducts[currProduct.ProductId], transactionParams.LoginId, parafaitUtility);
                                        }
                                        else
                                        {
                                            RandomTagNumber randomTagNumber = new RandomTagNumber(parafaitUtility.ExecutionContext, tagNumberLengthList);
                                            string cardNum = currProduct.AutoGenerateCardNumber == "Y" ? randomTagNumber.Value : "T" + randomTagNumber.Value.Substring(1);
                                            currCard = new Card(cardNum, transactionParams.LoginId, parafaitUtility);
                                            if (products.GetProductsDTO.LoadToSingleCard)
                                            {
                                                if (!cardsForProducts.ContainsKey(currProduct.ProductId))
                                                {
                                                    cardsForProducts.Add(currProduct.ProductId, cardNum);
                                                }
                                            }
                                        }
                                        if (currCard.customer_id < 0 && customerDTO.Id > 0)
                                        {
                                            currCard.customerDTO = customerDTO;
                                        }
                                        cardList.Add(currCard);
                                    }
                                }

                                lineSaveStatus = CreateAttractionLine(currProduct, transactionParams, currTransaction, cardList, ref message, offSetDuration);
                            }
                            else
                            {
                                // The amount would be greater than 0 only in case of variable purchase. Otherwise, the amount 
                                // should not be passed. The amount at the product level would be used. 
                                Transaction.TransactionLine outTransactionLine = new Transaction.TransactionLine(); //creating object of type TransactionLine. Output for ParentLineId
                                lineSaveStatus = currTransaction.createTransactionLine(currCard, currProduct.ProductId, amount, currProduct.ProductQuantity, ref message, outTransactionLine, createChildLines);
                                //  outTransactionLine.Remarks = currProduct.Guid;
                                outTransactionLine.Remarks = currProduct.Remarks;
                                transactionLineList.Add(new TransactionCoreLineLinker(currProduct.PurchaseLineId, outTransactionLine));

                            }

                            if (lineSaveStatus != 0) //changed to check for non-zero value
                                throw new Exception(message);

                        }
                        else
                        {
                            bool firstLineComplete = false;
                            DataTable dt = parafaitUtility.executeDataTable(@"select product_id, p.price, v.CustomDataNumber, p.product_name
                                                                                    from products p, product_type pt, CustomDataView v 
                                                                                   where pt.product_type_id = p.product_type_id 
                                                                                     and pt.product_type = 'RECHARGE'
                                                                                     and price <= @amount 
                                                                                     and price > 0
                                                                                     and ISNULL(p.active_flag,'Y') = 'Y'
                                                                                     and p.CustomDataSetId = v.CustomDataSetId
                                                                                     and v.Name = 'External System Identifier'
                                                                                     and v.CustomDataNumber is not null
                                                                                   order by p.price desc",
                                                        new System.Data.SqlClient.SqlParameter("@amount", currProduct.Amount));
                            if (dt.Rows.Count > 0)
                            {
                                double finalAmount = -1;
                                try
                                {
                                    finalAmount = Convert.ToDouble(currProduct.Amount);
                                    int rowNumber = 0;
                                    while ((finalAmount > 0) && (rowNumber < dt.Rows.Count))
                                    {
                                        int prodId = Convert.ToInt32(dt.Rows[rowNumber]["product_id"]);
                                        int externalRef = Convert.ToInt32(dt.Rows[rowNumber]["CustomDataNumber"]);
                                        double price = Convert.ToDouble(dt.Rows[rowNumber]["price"]);
                                        if (price <= finalAmount)
                                        {
                                            finalAmount -= price;
                                            Transaction.TransactionLine outTransactionLine = new Transaction.TransactionLine(); //creating object of type TransactionLine. Output for ParentLineId
                                            int lineSaveStatus = currTransaction.createTransactionLine(currCard, prodId, -1, 1, ref message, outTransactionLine, createChildLines);
                                            if (firstLineComplete == false)
                                            {
                                                outTransactionLine.Remarks = currProduct.Guid;
                                                firstLineComplete = true;
                                            }
                                            else
                                                outTransactionLine.Remarks = "";
                                            if (lineSaveStatus != 0) //changed to check for non-zero value
                                                throw new Exception(message);
                                            transactionLineList.Add(new TransactionCoreLineLinker(currProduct.PurchaseLineId, outTransactionLine));

                                        }
                                        else
                                            rowNumber++;
                                    }
                                    if (finalAmount != 0)
                                    {
                                        throw new Exception("Variable recharge amount count not be exactly matched to the products setup in Parafait. The transaction cannot be setup in Aloha. Variable amount: " + currProduct.Amount);
                                    }
                                }
                                catch (FormatException)
                                {
                                    throw new Exception("Variable recharge amount is not a number. Please check..");
                                }
                            }
                            else
                            {
                                throw new Exception("External System Reference for Parafait products not found. Variable amount cannot be setup for Aloha transaction.. Variable amount: " + currProduct.Amount);
                            }
                        }
                    }

                    for (int i = 0; i < orderedProducts.Count; i++)
                    {
                        if (orderedProducts[i].LinkLineId == currProduct.PurchaseLineId)
                        {
                            TransactionCoreLineLinker parentLine = transactionLineList.Find(trxLine => trxLine.TransactionLineIdx == currProduct.PurchaseLineId);
                            Transaction.TransactionLine childTransactionLine = new Transaction.TransactionLine(); //creating object of type TransactionLine. Output for ParentLineId

                            int lineSaveStatus = currTransaction.createTransactionLine(currCard, orderedProducts[i].ProductId, -1, orderedProducts[i].ProductQuantity, parentLine.TransactionLine, ref message, childTransactionLine, createChildLines);

                            childTransactionLine.Remarks = orderedProducts[i].Remarks;
                            if (lineSaveStatus != 0) //changed to check for non-zero value
                                throw new Exception(message);
                            transactionLineList.Add(new TransactionCoreLineLinker(orderedProducts[i].PurchaseLineId, childTransactionLine));
                        }
                    }
                }


                // Apply Discount Coupons
                if (!String.IsNullOrEmpty(transactionParams.DiscountCouponCode))
                {

                    List<DiscountCouponsUsedDTO> lstDiscountCouponsUsed = new List<DiscountCouponsUsedDTO>();
                    if (transactionParams.ShouldCommit == false && transactionParams.TrxId > 0 && !String.IsNullOrEmpty(transactionParams.DiscountCouponCode))
                    {
                        // Booking Online Edit Feature 
                        DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL(machineUserContext);
                        List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> discountCouponsUsedSearchParams = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
                        discountCouponsUsedSearchParams.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.TRANSACTION_ID, transactionParams.TrxId.ToString()));
                        discountCouponsUsedSearchParams.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.IS_ACTIVE, "Y"));
                        lstDiscountCouponsUsed = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(discountCouponsUsedSearchParams);

                        if (lstDiscountCouponsUsed != null)
                        {
                            foreach (DiscountCouponsUsedDTO usedCouponDTO in lstDiscountCouponsUsed)
                            {
                                usedCouponDTO.IsActive = false;
                                new DiscountCouponsUsedBL(machineUserContext, usedCouponDTO).Save();
                            }
                        }
                    }


                    try
                    {
                        string[] discoupons = transactionParams.DiscountCouponCode.ToString().Split('|');
                        foreach (string coupon in discoupons)
                        {
                            if (!String.IsNullOrEmpty(coupon))
                            {
                                DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(machineUserContext, coupon, null);
                                if (discountCouponsBL.CouponStatus == CouponStatus.ACTIVE)
                                {
                                    currTransaction.ApplyCoupon(coupon);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        throw ex;
                    }
                    finally
                    {
                        if (lstDiscountCouponsUsed != null)
                        {
                            foreach (DiscountCouponsUsedDTO usedCouponDTO in lstDiscountCouponsUsed)
                            {
                                usedCouponDTO.IsActive = true;
                                new DiscountCouponsUsedBL(machineUserContext, usedCouponDTO).Save();
                            }
                        }
                    }
                }


                //currTransaction.UpdateWaiverSignedHistoryStatus(transactionParams.WaiverSignedDTOList);
                if (transactionParams.MembershipId >= 0 && transactionParams.MembershipRewardsId >= 0)
                    currTransaction.UpdateMembershipInfoOnLines(inOrderedProducts, transactionParams.MembershipId, transactionParams.MembershipRewardsId, transactionParams.ExpireWithMembership, transactionParams.ForMembershipOnly);

                // currTransaction.UpdateWaiverSignedHistoryStatus(transactionParams.WaiverSignedDTOList);
                double paymentRoundOffAmount = currTransaction.TransactionPaymentsDTOList.Where(x => x.paymentModeDTO != null
                                                                            && x.paymentModeDTO.IsRoundOff).Sum(x => x.Amount);          
                if (transactionParams.ShouldCommit == false)
                {
                    currTransaction.CreateRoundOffPayment();
                    transactionDetails.Add(new TransactionKeyValueStruct("TrxAmount", currTransaction.Pre_TaxAmount.ToString(parafaitUtility.ParafaitEnv.AMOUNT_FORMAT)));
                    transactionDetails.Add(new TransactionKeyValueStruct("TrxTax", currTransaction.Tax_Amount.ToString(parafaitUtility.ParafaitEnv.AMOUNT_FORMAT)));
                    transactionDetails.Add(new TransactionKeyValueStruct("Discount_Amount", currTransaction.Discount_Amount.ToString(parafaitUtility.ParafaitEnv.AMOUNT_FORMAT)));
                    transactionDetails.Add(new TransactionKeyValueStruct("TrxTotalAmount", currTransaction.Transaction_Amount.ToString(parafaitUtility.ParafaitEnv.AMOUNT_FORMAT)));
                    transactionDetails.Add(new TransactionKeyValueStruct("Net_Transaction_Amount", currTransaction.Net_Transaction_Amount.ToString(parafaitUtility.ParafaitEnv.AMOUNT_FORMAT)));
                    transactionDetails.Add(new TransactionKeyValueStruct("TrxRoundOffAmount", paymentRoundOffAmount.ToString(parafaitUtility.ParafaitEnv.AMOUNT_FORMAT)));
                    transactionDetails.Add(new TransactionKeyValueStruct("TrxFinalAmount", (currTransaction.Transaction_Amount - paymentRoundOffAmount).ToString(parafaitUtility.ParafaitEnv.AMOUNT_FORMAT)));

                    int countOfCCPConsumptionApplied = 0; //Count of ccp consumption through the trxLine
                    List<AttractionBookingDTO> bookedATBList = new List<AttractionBookingDTO>();
                    for (int i = 0; i < currTransaction.TrxLines.Count; i++)
                    {
                        if (currTransaction.TrxLines[i].LineValid == true)
                        {
                            transactionDetails.Add(new TransactionKeyValueStruct("TrxLineProductId_" + i, currTransaction.TrxLines[i].ProductID.ToString()));
                            transactionDetails.Add(new TransactionKeyValueStruct("TrxLineGuid_" + i, string.IsNullOrEmpty(currTransaction.TrxLines[i].Remarks) ? "" : currTransaction.TrxLines[i].Remarks.ToString()));
                            transactionDetails.Add(new TransactionKeyValueStruct("TrxLineProductName_" + i, currTransaction.TrxLines[i].ProductName.ToString()));
                            transactionDetails.Add(new TransactionKeyValueStruct("TrxLineQuantity_" + i, currTransaction.TrxLines[i].quantity.ToString()));
                            transactionDetails.Add(new TransactionKeyValueStruct("TrxLinePrice_" + i, currTransaction.TrxLines[i].Price.ToString(parafaitUtility.ParafaitEnv.AMOUNT_FORMAT)));
                            transactionDetails.Add(new TransactionKeyValueStruct("TrxLineTax_" + i, currTransaction.TrxLines[i].tax_amount.ToString(parafaitUtility.ParafaitEnv.AMOUNT_FORMAT)));
                            transactionDetails.Add(new TransactionKeyValueStruct("TrxLineAmount_" + i, currTransaction.TrxLines[i].LineAmount.ToString(parafaitUtility.ParafaitEnv.AMOUNT_FORMAT)));
                            transactionDetails.Add(new TransactionKeyValueStruct("TrxLineCardNumber_" + i, (currTransaction.TrxLines[i].CardNumber == null) ? (string.Empty) : (currTransaction.TrxLines[i].CardNumber.ToString())));
                            if (currTransaction.TrxLines[i].ParentLine == null)
                                transactionDetails.Add(new TransactionKeyValueStruct("TrxParentLineId_" + i, "-1"));
                            else
                            {
                                bool parentLineFound = false;
                                for (int j = 0; (j < currTransaction.TrxLines.Count && (parentLineFound == false)); j++)
                                {
                                    if (currTransaction.TrxLines[j] == currTransaction.TrxLines[i].ParentLine)
                                    {
                                        parentLineFound = true;
                                        transactionDetails.Add(new TransactionKeyValueStruct("TrxParentLineId_" + i, j.ToString()));
                                    }
                                }
                            }

                            if (currTransaction.TrxLines[i].LineAtb != null)
                            {
                                currTransaction.TrxLines[i].LineAtb.AttractionBookingDTO.AttractionProductId = currTransaction.TrxLines[i].ProductID;
                                currTransaction.TrxLines[i].LineAtb.AttractionBookingDTO.ScheduleFromDate = currTransaction.TrxLines[i].LineAtb.AttractionBookingDTO.ScheduleFromDate.AddSeconds(-1 * offSetDuration);
                                currTransaction.TrxLines[i].LineAtb.AttractionBookingDTO.ScheduleToDate = currTransaction.TrxLines[i].LineAtb.AttractionBookingDTO.ScheduleToDate.AddSeconds(-1 * offSetDuration);
                                //if (currTransaction.TrxLines[i].Price > 0)
                                //{
                                //    currTransaction.TrxLines[i].LineAtb.AttractionBookingDTO.Price = currTransaction.TrxLines[i].LineAtb.AttractionBookingDTO.Price;
                                //}
                                if (currTransaction.TrxLines[i].Price > 0)
                                {
                                    currTransaction.TrxLines[i].LineAtb.AttractionBookingDTO.Price = currTransaction.TrxLines[i].Price;
                                }
                                bookedATBList.Add(currTransaction.TrxLines[i].LineAtb.AttractionBookingDTO);
                            }

                            if (!string.IsNullOrWhiteSpace(transactionParams.PaymentCardNumber) && currTransaction.TrxLines[i].CreditPlusConsumptionId > 0)
                            {
                                countOfCCPConsumptionApplied += 1;
                            }

                        }
                    }

                    transactionDetails.Add(new TransactionKeyValueStruct("CountofCCPConsumption", countOfCCPConsumptionApplied > 0 ? Convert.ToString(countOfCCPConsumptionApplied) : "0"));

                    transactionDetails.Add(new TransactionKeyValueStruct("DiscountSummary", GetDiscountCouponsSummary(currTransaction)));

                    //Gets the list Of WaiverSummary 
                    //transactionDetails.Add(new TransactionKeyValueStruct("WaiverSummary", GetWaiverSummary(currTransaction.WaiversSignedHistoryDTOList)));

                    transactionDetails.Add(new TransactionKeyValueStruct("AttractionBookingList", JsonConvert.SerializeObject(bookedATBList)));
                }
                else
                {

                    if (transactionParams.CreditCardInvoiceNumber != -1)
                        currTransaction.PaymentReference = transactionParams.TrxPaymentReference + ' ' + transactionParams.CreditCardInvoiceNumber.ToString(); //25-May-2015:: Added Mercury Invoice number passed to Mercury Gateway
                    else
                        currTransaction.PaymentReference = transactionParams.TrxPaymentReference;

                    if (transactionParams.CloseTransaction)
                    {
                        PaymentGatewayFactory.GetInstance().Initialize(parafaitUtility, true, null);

                        if (transactionParams.CreditCardNumber != null && transactionParams.CreditCardNumber.CompareTo("") != 0)
                        {
                            PaymentModeDTO paymentModeDTO = new PaymentMode(machineUserContext, transactionParams.PaymentModeId).GetPaymentModeDTO;
                            TransactionPaymentsDTO trxCCPaymentDTO = new TransactionPaymentsDTO(-1, -1, transactionParams.PaymentModeId, Convert.ToDouble((currTransaction.Net_Transaction_Amount - paymentRoundOffAmount)),
                                                                                                  "**********" + transactionParams.CreditCardNumber.Substring(transactionParams.CreditCardNumber.Length - 4),
                                                                                                  transactionParams.CreditCardName, transactionParams.CreditCardType, transactionParams.CreditCardExpDate,
                                                                                                  transactionParams.CreditCardPaymentReference, -1, "", -1, -1, transactionParams.PaymentReference, "", false, transactionParams.SiteId,
                                                                                                  -1, "", parafaitUtility.getServerTime(),
                                                                                                  parafaitUtility.ParafaitEnv.LoginID, -1,
                                                                                                  null, 0, -1, parafaitUtility.ParafaitEnv.POSMachine, -1, "", null);
                            trxCCPaymentDTO.paymentModeDTO = paymentModeDTO;
                            trxCCPaymentDTO.GatewayPaymentProcessed = true;
                            trxCCPaymentDTO.paymentModeDTO.GatewayLookUp = PaymentGateways.None;
                            trxCCPaymentDTO.paymentModeDTO.AcceptChanges();
                            currTransaction.TransactionPaymentsDTOList.Add(trxCCPaymentDTO);
                            //staticDataExchange ccPaymentProcessingSDA = currTransaction.StaticDataExchange;
                            //ccPaymentProcessingSDA.ClearPaymentData();
                            //ccPaymentProcessingSDA.PaymentModeDetails.Clear();
                            //staticDataExchange.PaymentModeDetail paymentModeDetail = new staticDataExchange.PaymentModeDetail();
                            //paymentModeDetail.CreditCardAuthorization = transactionParams.CreditCardPaymentReference;
                            //paymentModeDetail.CreditCardExpiry = transactionParams.CreditCardExpDate;
                            //paymentModeDetail.NameOnCard = transactionParams.CreditCardName;
                            //paymentModeDetail.CreditCardName = transactionParams.CreditCardType;
                            //paymentModeDetail.CreditCardNumber = "**********" + transactionParams.CreditCardNumber.Substring(transactionParams.CreditCardNumber.Length - 4);
                            //paymentModeDetail.Amount = Convert.ToDouble((currTransaction.Net_Transaction_Amount - currTransaction.StaticDataExchange.PaymentRoundOffAmount));
                            //paymentModeDetail.isCreditCard = true;
                            //paymentModeDetail.Gateway = PaymentGateways.None;
                            //paymentModeDetail.PaymentModeId = transactionParams.PaymentModeId;

                            //paymentModeDetail.Reference = transactionParams.PaymentReference; //25-May-2015:: Adding Reference received from Mercury to TrxPayments.Reference field Modified on 18/11/166 Rakshith
                            //ccPaymentProcessingSDA.PaymentModeDetails.Add(paymentModeDetail);

                            //ccPaymentProcessingSDA.PaymentCreditCardAmount = Convert.ToDouble((currTransaction.Net_Transaction_Amount - currTransaction.StaticDataExchange.PaymentRoundOffAmount));
                            //ccPaymentProcessingSDA.PaymentCreditCardSurchargeAmount = 0;
                            //ccPaymentProcessingSDA.PaymentModeId = paymentModeDetail.PaymentModeId;
                        }
                        else if (!string.IsNullOrEmpty(transactionParams.PaymentCardNumber)) //Debit card payment Ver 1.01
                        {
                            Card payCard = null;
                            payCard = new Card(transactionParams.PaymentCardNumber, transactionParams.LoginId, parafaitUtility);
                            PaymentModeList paymentModeListBL = new PaymentModeList(machineUserContext);
                            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                            if (paymentModeDTOList != null)
                            {
                                double transactionAmount = Convert.ToDouble((currTransaction.Net_Transaction_Amount - paymentRoundOffAmount));
                                TransactionPaymentsDTO debitTrxPaymentDTO = new TransactionPaymentsDTO();
                                debitTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                                debitTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                                debitTrxPaymentDTO.CardId = payCard.card_id;
                                debitTrxPaymentDTO.CardEntitlementType = "C";
                                debitTrxPaymentDTO.PaymentCardNumber = payCard.CardNumber;
                                //staticDataExchange gameCardPaymentProcessingSDA = currTransaction.StaticDataExchange;
                                //gameCardPaymentProcessingSDA.ClearPaymentData();
                                //gameCardPaymentProcessingSDA.PaymentModeDetails.Clear();
                                //staticDataExchange.PaymentModeDetail paymentModeDetail = new staticDataExchange.PaymentModeDetail();
                                ////paymentModeDetail.isDebitCard = true;
                                //paymentModeDetail.Amount = Convert.ToDouble((currTransaction.Net_Transaction_Amount - currTransaction.StaticDataExchange.PaymentRoundOffAmount));
                                //gameCardPaymentProcessingSDA.PaymentModeDetails.Add(paymentModeDetail);
                                //gameCardPaymentProcessingSDA.GameCardId = payCard.card_id;
                                //gameCardPaymentProcessingSDA.PaymentCardNumber = payCard.CardNumber;
                                //gameCardPaymentProcessingSDA.PaymentGameCardAmount = Convert.ToDouble((currTransaction.Net_Transaction_Amount - currTransaction.StaticDataExchange.PaymentRoundOffAmount));

                                CreditPlus creditPlus = new CreditPlus(parafaitUtility);
                                double creditPlusAmount = 0;
                                //get credit plus balance using method from Utils
                                creditPlusAmount = creditPlus.getCreditPlusForPOS(payCard.card_id, parafaitUtility.ParafaitEnv.POSTypeId, currTransaction);
                                double credits = payCard.credits;
                                if ((credits + creditPlusAmount) < transactionAmount) //Trx Amount is more than game card balance
                                    throw new Exception("Insufficient Credits on Game Card: " + transactionParams.PaymentCardNumber);
                                if (creditPlusAmount >= transactionAmount) //Credit Plus is more than trx amount. So credit plus is deducted
                                {
                                    debitTrxPaymentDTO.PaymentUsedCreditPlus = transactionAmount;
                                    debitTrxPaymentDTO.Amount = 0;
                                }
                                else //use credit plus and balance to be taken from credits
                                {
                                    debitTrxPaymentDTO.PaymentUsedCreditPlus = creditPlusAmount;
                                    debitTrxPaymentDTO.Amount = (transactionAmount - creditPlusAmount);
                                }
                                currTransaction.TransactionPaymentsDTOList.Add(debitTrxPaymentDTO);
                            }
                        } // end Ver 1.01
                        else
                        {
                            PaymentModeList paymentModeListBL = new PaymentModeList(machineUserContext);
                            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                            if (paymentModeDTOList != null)
                            {
                                currTransaction.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                                TransactionPaymentsDTO cashTrxPaymentDTO = new TransactionPaymentsDTO();
                                cashTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                                cashTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                                cashTrxPaymentDTO.Amount = Convert.ToDouble((currTransaction.Net_Transaction_Amount - paymentRoundOffAmount));
                                cashTrxPaymentDTO.IsTaxable = null;
                                currTransaction.TransactionPaymentsDTOList.Add(cashTrxPaymentDTO);
                            }
                            //staticDataExchange cashPaymentProcessingSDA = currTransaction.StaticDataExchange;
                            //cashPaymentProcessingSDA.ClearPaymentData();
                            //cashPaymentProcessingSDA.PaymentModeDetails.Clear();
                            //staticDataExchange.PaymentModeDetail paymentModeDetail = new staticDataExchange.PaymentModeDetail();
                            //paymentModeDetail.isCash = true;
                            //paymentModeDetail.Amount = Convert.ToDouble((currTransaction.Net_Transaction_Amount - currTransaction.StaticDataExchange.PaymentRoundOffAmount));
                            //cashPaymentProcessingSDA.PaymentModeDetails.Add(paymentModeDetail);
                            //cashPaymentProcessingSDA.PaymentCashAmount = Convert.ToDouble((currTransaction.Net_Transaction_Amount - currTransaction.StaticDataExchange.PaymentRoundOffAmount));
                        }
                    }


                    foreach (LinkedPurchaseProductsStruct currProduct in orderedProducts)
                    {
                        if (currProduct.ProductType == "ATTRACTION" && currProduct.AttractionBookingList != null && currProduct.AttractionBookingList.Count > 0)
                        {
                            foreach (AttractionBookingDTO atbDTO in currProduct.AttractionBookingList)
                            {
                                if (atbDTO.BookingId != -1)
                                {
                                    AttractionBooking attractionBooking = new AttractionBooking(parafaitUtility.ExecutionContext, atbDTO);
                                    //attractionBooking.BookingId = atbDTO.BookingId;
                                    attractionBooking.Expire();
                                }
                            }
                        }
                    }


                    int retCode;
                    if (transactionParams.CloseTransaction == false)
                    {
                        currTransaction.Remarks = transactionParams.OrderRemarks + " offsetapplied : " + offSetDuration.ToString();
                        retCode = currTransaction.SaveOrder(ref message); // Create Open Transaction in case of credit cards
                    }
                    else
                    {
                        retCode = currTransaction.SaveTransacation(ref message);
                    }

                    if (retCode != 0)
                        throw new Exception(message);
                    else
                    {
                        long trxId = currTransaction.Trx_id;
                        transactionDetails.Add(new TransactionKeyValueStruct("TransactionId", trxId.ToString()));

                        transactionDetails.Add(new TransactionKeyValueStruct("DiscountSummary", GetDiscountCouponsSummary(currTransaction)));
                        //transactionDetails.Add(new TransactionKeyValueStruct("WaiverSummary", GetWaiverSummary(currTransaction.WaiversSignedHistoryDTOList)));

                        log.Debug("SaveTransacation Complete" + trxId.ToString());

                        try
                        {
                            List<AttractionBookingDTO> bookedATBList = new List<AttractionBookingDTO>();
                            foreach (Transaction.TransactionLine currTrxLine in currTransaction.TrxLines)
                            {
                                if (currTrxLine.card != null)
                                {
                                    AccountDTO updatedAccountDTO = new AccountBL(currTransaction.Utilities.ExecutionContext, currTrxLine.card.card_id, true, true).AccountDTO;
                                    DBSynchLogService dBSynchLogService = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "Cards", updatedAccountDTO.Guid, updatedAccountDTO.SiteId);
                                    dBSynchLogService.CreateRoamingData();
                                    if (updatedAccountDTO.AccountCreditPlusDTOList != null)
                                    {
                                        foreach (AccountCreditPlusDTO updatedAccountCreditPlus in updatedAccountDTO.AccountCreditPlusDTOList)
                                        {
                                            DBSynchLogService dBSynchLogServiceCP = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardCreditPlus", updatedAccountCreditPlus.Guid, updatedAccountCreditPlus.SiteId);
                                            dBSynchLogServiceCP.CreateRoamingData();
                                            if (updatedAccountCreditPlus.AccountCreditPlusConsumptionDTOList != null)
                                            {
                                                foreach (AccountCreditPlusConsumptionDTO updatedAccountCPConsumpDTO in updatedAccountCreditPlus.AccountCreditPlusConsumptionDTOList)
                                                {
                                                    DBSynchLogService dBSynchLogServiceCPConsp = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardCreditPlusConsumption", updatedAccountCPConsumpDTO.Guid, updatedAccountCPConsumpDTO.SiteId);
                                                    dBSynchLogServiceCPConsp.CreateRoamingData();
                                                }
                                            }
                                            if (updatedAccountCreditPlus.AccountCreditPlusPurchaseCriteriaDTOList != null)
                                            {
                                                foreach (AccountCreditPlusPurchaseCriteriaDTO updatedAccountCPPCDTO in updatedAccountCreditPlus.AccountCreditPlusPurchaseCriteriaDTOList)
                                                {
                                                    DBSynchLogService dBSynchLogServiceCPPC = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardCreditPlusPurchaseCriteria", updatedAccountCPPCDTO.Guid, updatedAccountCPPCDTO.SiteId);
                                                    dBSynchLogServiceCPPC.CreateRoamingData();
                                                }
                                            }
                                        }
                                    }
                                    if (updatedAccountDTO.AccountGameDTOList != null)
                                    {
                                        foreach (AccountGameDTO updatedAccountGame in updatedAccountDTO.AccountGameDTOList)
                                        {
                                            DBSynchLogService dBSynchLogServiceGame = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardGames", updatedAccountGame.Guid, updatedAccountGame.SiteId);
                                            dBSynchLogServiceGame.CreateRoamingData();
                                            if (updatedAccountGame.AccountGameExtendedDTOList != null)
                                            {
                                                foreach (AccountGameExtendedDTO updatedAccountGameExtDTO in updatedAccountGame.AccountGameExtendedDTOList)
                                                {
                                                    DBSynchLogService dBSynchLogServiceGameExt = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardGameExtended", updatedAccountGameExtDTO.Guid, updatedAccountGameExtDTO.SiteId);
                                                    dBSynchLogServiceGameExt.CreateRoamingData();
                                                }
                                            }
                                        }
                                    }
                                    if (updatedAccountDTO.AccountDiscountDTOList != null)
                                    {
                                        foreach (AccountDiscountDTO updatedAccountDiscount in updatedAccountDTO.AccountDiscountDTOList)
                                        {
                                            DBSynchLogService dBSynchLogServiceDiscount = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardDiscounts", updatedAccountDiscount.Guid, updatedAccountDiscount.SiteId);
                                            dBSynchLogServiceDiscount.CreateRoamingData();
                                        }
                                    }
                                    //check if Transaction is happening in site, which is enabled for on-demand roaming
                                    if (currTransaction.Utilities.getParafaitDefaults("ENABLE_ON_DEMAND_ROAMING").Equals("Y")
                                        && currTransaction.Utilities.getParafaitDefaults("AUTOMATIC_ON_DEMAND_ROAMING").Equals("Y")
                                        && currTransaction.Utilities.getParafaitDefaults("ALLOW_ROAMING_CARDS").Equals("Y")
                                        )
                                    {
                                        //account site id does not match with transaction site id. This can happen in virtual store scenario
                                        if (updatedAccountDTO.SiteId != currTransaction.Utilities.ExecutionContext.GetSiteId())
                                        {
                                            DBSynchLogDTO dbSynchLogDTO = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "Cards", currTransaction.Utilities.getServerTime(), currTransaction.Utilities.ExecutionContext.GetSiteId());
                                            DBSynchLogBL dbSynchLogBL = new DBSynchLogBL(currTransaction.Utilities.ExecutionContext, dbSynchLogDTO);
                                            dbSynchLogBL.Save();
                                        }
                                    }
                                }

                                if (currTrxLine.LineAtb != null)
                                {
                                    currTrxLine.LineAtb.AttractionBookingDTO.AttractionProductId = currTrxLine.ProductID;
                                    currTrxLine.LineAtb.AttractionBookingDTO.ScheduleFromDate = currTrxLine.LineAtb.AttractionBookingDTO.ScheduleFromDate.AddSeconds(-1 * offSetDuration);
                                    currTrxLine.LineAtb.AttractionBookingDTO.ScheduleToDate = currTrxLine.LineAtb.AttractionBookingDTO.ScheduleToDate.AddSeconds(-1 * offSetDuration);
                                    bookedATBList.Add(currTrxLine.LineAtb.AttractionBookingDTO);
                                }
                            }
                            transactionDetails.Add(new TransactionKeyValueStruct("AttractionBookingList", JsonConvert.SerializeObject(bookedATBList)));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Create Roaming data failed for Transaction: " + trxId.ToString(), ex);
                        }
                        //Apply Offset to TrxDate
                        //if (trxId > 0 && transactionParams.ApplyOffset && offSetDuration != 0)
                        //{
                        //    log.Debug("Apply Offset to Temp Cards in CardGames and Cards for ExpiryDate " + trxId.ToString());

                        //    string cardExpiryUpdateQuery = @"Update trx_header 
                        //                                        set TrxDate=dateadd(SECOND, @offSetDuration, TrxDate),
                        //                                            LastUpdateTime =  getdate()
                        //                                        where trxid = @TrxId ;
                        //                                     Update CardGames 
                        //                                        set FromDate = dateadd(SECOND, @offSetDuration, FromDate),
                        //                                            ExpiryDate = dateadd(SECOND, @offSetDuration, ExpiryDate),
                        //                                            last_update_date = getdate()
                        //                                     where card_id in (select card_id from trx_lines where trxid = @TrxId and card_number like 'T%' )
                        //                                     and TrxId = @TrxId ;
                        //                                    Update Cards 
                        //                                  set ExpiryDate = dateadd(SECOND, @offSetDuration ,ExpiryDate),
                        //                                   last_update_time =  getdate()
                        //                                     where card_id in (select card_id from trx_lines where trxid = @TrxId and card_number like 'T%' )";

                        //    List<SqlParameter> updateTxrParameters = new List<SqlParameter>();
                        //    updateTxrParameters.Add(new SqlParameter("@offSetDuration", offSetDuration));
                        //    updateTxrParameters.Add(new SqlParameter("@TrxId", trxId));
                        //    dataAccessHandler.executeUpdateQuery(cardExpiryUpdateQuery, updateTxrParameters.ToArray());
                        //}
                    }
                }
            }

            catch (Exception ex)
            {
                log.Log("Error - TransactionDatahandler() Method", ex);
                throw;
            }
            finally
            {
                closeConnection();
            }
            return transactionDetails;
        }



        /// <summary>
        ///  GetDiscountCouponsSummary from currTransaction
        /// </summary>
        /// <param name="currTransaction"></param>
        /// <returns></returns>
        private string GetDiscountCouponsSummary(Transaction inCurrTransaction)
        {
            StringBuilder stbJsonData = new StringBuilder();
            foreach (DiscountsSummaryDTO dst in inCurrTransaction.DiscountsSummaryDTOList.ToList())
            {
                string couponCode = "Automatic";

                if (inCurrTransaction.DiscountApplicationHistoryDTOList != null)
                {
                    foreach (DiscountApplicationHistoryDTO dsth in inCurrTransaction.DiscountApplicationHistoryDTOList.ToList())
                    {
                        if (dsth.DiscountId == dst.DiscountId)
                        {
                            couponCode = dsth.CouponNumber;
                            break;
                        }
                    }
                }

                stbJsonData.Append("{");
                stbJsonData.Append("\"DiscountId\":" + dst.DiscountId.ToString() + ",");
                stbJsonData.Append("\"DiscountName\":\"" + dst.DiscountName.ToString() + "\",");
                stbJsonData.Append("\"CouponCode\":\"" + couponCode + "\",");
                stbJsonData.Append("\"DiscountPercentage\":\"" + dst.DiscountPercentage.ToString("n2") + "\",");
                stbJsonData.Append("\"DiscountAmount\":\"" + dst.DiscountAmount.ToString("c2") + "\"");
                stbJsonData.Append("}");
            }

            log.Debug(stbJsonData.ToString());
            return stbJsonData.ToString() == "" ? "" : "[" + stbJsonData.ToString() + "]";

        }

        /// <summary>
        ///  GetWaiverSummary from currTransaction
        /// </summary>
        /// <param name="currTransaction"></param>
        /// <returns></returns>
        private string GetWaiverSummary(List<WaiverSignatureDTO> inWaiversSignedDTOSummaryList)
        {
            if (inWaiversSignedDTOSummaryList == null)
                return "";

            return new JavaScriptSerializer().Serialize(inWaiversSignedDTOSummaryList);

        }



        /// <summary>
        /// Method to Check the Ordered Products in Trx_lines and remove from order list 
        /// </summary>
        /// <param name="trxParams"></param>
        /// <param name="inOrderedProducts"></param>
        public void CheckAndRemoveFromOrderedProducts(TransactionParams trxParams, ref List<LinkedPurchaseProductsStruct> inOrderedProducts)
        {
            log.Debug("Starts- CheckAndRemoveFromOrderedProducts Method .");
            string selectTrxQuery = @"select * from trx_lines where TrxId=@trxId and CancelledBy is null ";
            SqlParameter[] selectTrxParameters = new SqlParameter[1];
            selectTrxParameters[0] = new SqlParameter("@trxId", trxParams.TrxId);
            DataTable dtTrxLines = dataAccessHandler.executeSelectQuery(selectTrxQuery, selectTrxParameters);

            if (dtTrxLines.Rows.Count > 0)
            {
                foreach (DataRow drLine in dtTrxLines.Rows)
                {
                    LinkedPurchaseProductsStruct linkedPurchaseProductsStruct = inOrderedProducts.Where(x => x.TrxLineId.ToString() == drLine["LineId"].ToString()).FirstOrDefault();
                    if (linkedPurchaseProductsStruct != null && linkedPurchaseProductsStruct.ProductId > 0)
                    {
                        inOrderedProducts.Remove(linkedPurchaseProductsStruct);
                    }
                }
            }
        }






        /// <summary>
        /// Updates transaction 
        /// </summary>
        /// <param name="transcationParamStructDTO">transcationParamStructDTO</param>
        /// <returns> returns TransactionStatus</returns>
        public TransactionStatus UpdateTransaction(TransactionParams trxParams)
        {
            log.Debug("Starts- UpdateTransaction(trxParams) Method .");
            transactionStatus = new TransactionStatus();

            try
            {
                if (trxParams.TrxId <= 0 || trxParams.LoginId == "")
                {
                    transactionStatus.Message = "Invalid Transaction/Login Id";
                    log.Debug("ERROR - UpdateTransaction(trxParams) Method ." + transactionStatus.Message.ToString());

                    return transactionStatus;
                }

                int offSetDuration = timeZoneUtil.GetOffSetDuration(trxParams.SiteId, trxParams.VisitDate.Date);

                log.Debug("UpdateTransaction(trxParams) Method ." + trxParams.OrderRemarks + "offsetapplied Sec:" + offSetDuration.ToString());

                string TrxUpdateQuery = @"Update trx_header 
                                                set TrxDate = dateadd(SECOND ,@offSetDuration,TrxDate),
                                                    Remarks = case when @Remarks = '' then Remarks else @Remarks end ,
                                                    LastUpdateTime =  getdate()
                                                where TrxId = @TrxId ";

                SqlParameter[] updateTxrParameters = new SqlParameter[3];

                updateTxrParameters[0] = new SqlParameter("@offSetDuration", trxParams.ApplyOffset ? offSetDuration : 0);
                updateTxrParameters[1] = new SqlParameter("@Remarks", trxParams.OrderRemarks + "offsetapplied Sec:" + offSetDuration.ToString() + "|");
                updateTxrParameters[2] = new SqlParameter("@TrxId", trxParams.TrxId);

                dataAccessHandler.executeUpdateQuery(TrxUpdateQuery, updateTxrParameters);
                transactionStatus.IsExecuted = true;

            }
            catch (Exception ex)
            {
                transactionStatus.Message = ex.Message;
                log.Log("ERROR - UpdateTransaction(trxParams) Method .", ex);
            }
            finally
            {
                closeConnection();
            }

            log.Debug("Ends- UpdateTransaction(trxParams) Method .");

            return transactionStatus;

        }


        /// <summary>
        /// Confirm Purchase Transaction
        /// </summary>
        /// <param name="transactionParams">transactionParams</param>
        /// <returns> returns TransactionStatus</returns>
        public TransactionStatus ConfirmPurchaseTransaction(TransactionParams trxParams)
        {
            transactionStatus = new TransactionStatus();
            string message = "";

            try
            {
                if (trxParams.TrxId <= 0 || trxParams.LoginId == "")
                {
                    transactionStatus.Message = "Invalid Transaction/Login Id";
                    return transactionStatus;
                }

                int TrxUserId = -1;
                trxParams.SiteId = GetTransactionSiteId(trxParams.TrxId, ref TrxUserId);
                UsersDTO userDTO = ValidateLogin(trxParams.LoginId, trxParams.SiteId);

                if (userDTO.UserId != TrxUserId)
                {
                    userDTO = new Users(machineUserContext, TrxUserId).UserDTO;
                }

                trxParams.LoginId = userDTO.LoginId;
                trxParams.UserId = userDTO.UserId;
                trxParams.RoleId = userDTO.RoleId;

                parafaitUtility.ParafaitEnv.SiteId = trxParams.SiteId;
                parafaitUtility.ParafaitEnv.LoginID = trxParams.LoginId;
                parafaitUtility.ParafaitEnv.RoleId = trxParams.RoleId;
                parafaitUtility.ParafaitEnv.User_Id = trxParams.UserId;
                parafaitUtility.ParafaitEnv.SetPOSMachine("", trxParams.PosIdentifier);
                parafaitUtility.ParafaitEnv.Initialize();
                parafaitUtility.ExecutionContext.SetSiteId(parafaitUtility.ParafaitEnv.SiteId);
                parafaitUtility.ExecutionContext.SetUserId(parafaitUtility.ParafaitEnv.LoginID);

                TransactionUtils trxUtils = new TransactionUtils(parafaitUtility);
                //staticDataExchange staticDataExchange = new staticDataExchange(parafaitUtility);
                //staticDataExchange.InitializeVariables();

                if (parafaitUtility.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(parafaitUtility.ParafaitEnv.SiteId);
                    machineUserContext.SetIsCorporate(parafaitUtility.ParafaitEnv.IsCorporate);
                }

                Transaction currTransaction = new Transaction(parafaitUtility);
                currTransaction = trxUtils.CreateTransactionFromDB(trxParams.TrxId, parafaitUtility);
                if (currTransaction.Status.ToString() == Transaction.TrxStatus.CLOSED.ToString())
                {
                    transactionStatus.Message = currTransaction.Status.ToString();
                }
                else if (currTransaction.Status != Transaction.TrxStatus.OPEN && currTransaction.Status != Transaction.TrxStatus.INITIATED
                              && currTransaction.Status != Transaction.TrxStatus.ORDERED && currTransaction.Status != Transaction.TrxStatus.PREPARED)
                {
                    transactionStatus.Message = "Transaction Confirmation Failed " + trxParams.TrxId.ToString() + " Status - " + currTransaction.Status;
                }
                else
                {

                    if (Convert.ToDouble(currTransaction.Net_Transaction_Amount) > 0)
                    {

                        if (trxParams.PaymentModeId == -1 && !String.IsNullOrEmpty(currTransaction.Remarks))
                        {
                            string[] resvalues = currTransaction.Remarks.ToString().Split('|');
                            foreach (string word in resvalues)
                            {
                                if (word.Contains("PaymentModeId") == true)
                                {
                                    trxParams.PaymentModeId = Convert.ToInt32(word.Split(':')[1]);
                                }
                            }

                        }

                        //PaymentGatewayFactory.GetInstance().Initialize(parafaitUtility, true, null);

                        //staticDataExchange ccPaymentProcessingSDA = currTransaction.StaticDataExchange;
                        //ccPaymentProcessingSDA.ClearPaymentData();
                        //ccPaymentProcessingSDA.PaymentModeDetails.Clear();

                        double amount = 0;
                        if (!string.IsNullOrWhiteSpace(trxParams.AuthorizedPaymentAmount) && !trxParams.AuthorizedPaymentAmount.Equals("0"))
                        {
                            log.Debug("Authorized payment amount " + trxParams.AuthorizedPaymentAmount);
                            Double.TryParse(trxParams.AuthorizedPaymentAmount, out amount);
                        }
                        else
                        {
                            log.Debug("Authorized payment amount not found, get it from CCT");
                            CCTransactionsPGWBL ccTBL = new CCTransactionsPGWBL(trxParams.CCResponseId);
                            if (ccTBL != null)
                            {
                                log.Debug("Got CC Transactions");
                                log.Debug("Latest CCT DTO is " + ccTBL.CCTransactionsPGWDTO.ResponseID + ":Amount:" + ccTBL.CCTransactionsPGWDTO.Authorize);
                                Double.TryParse(ccTBL.CCTransactionsPGWDTO.Authorize, out amount);
                            }
                            else
                            {
                                log.Debug("No CC Transactions found for " + trxParams.CCResponseId);
                            }
                        }

                        PaymentModeDTO paymentModeDTO = new PaymentMode(machineUserContext, trxParams.PaymentModeId).GetPaymentModeDTO;
                        TransactionPaymentsDTO trxCCPaymentDTO = new TransactionPaymentsDTO(-1, -1, trxParams.PaymentModeId, amount,
                                                                                              trxParams.CreditCardNumber,
                                                                                              trxParams.CreditCardName, trxParams.CreditCardType, trxParams.CreditCardExpDate,
                                                                                              trxParams.CreditCardPaymentReference, -1, "", -1, -1, trxParams.PaymentReference, "", false, trxParams.SiteId,
                                                                                              trxParams.CCResponseId, "", parafaitUtility.getServerTime(),
                                                                                              parafaitUtility.ParafaitEnv.LoginID, -1,
                                                                                              null, 0, -1, parafaitUtility.ParafaitEnv.POSMachine, -1,
                                                                                              parafaitUtility.ParafaitEnv.CURRENCY_CODE, null);
                        trxCCPaymentDTO.paymentModeDTO = paymentModeDTO;
                        if (trxParams.PaymentProcessingCompleted)
                        {
                            trxCCPaymentDTO.GatewayPaymentProcessed = true;
                            trxCCPaymentDTO.paymentModeDTO.GatewayLookUp = PaymentGateways.None;
                            trxCCPaymentDTO.paymentModeDTO.AcceptChanges();
                        }
                        else
                        {
                            PaymentGatewayFactory.GetInstance().Initialize(parafaitUtility, true, null);
                            trxCCPaymentDTO.GatewayPaymentProcessed = false;
                            if (trxCCPaymentDTO.paymentModeDTO != null && trxCCPaymentDTO.paymentModeDTO.IsCreditCard)
                            {
                                string gateway = (new PaymentMode(machineUserContext, trxCCPaymentDTO.paymentModeDTO)).Gateway;
                                if (!string.IsNullOrEmpty(gateway) && Enum.IsDefined(typeof(PaymentGateways), gateway))
                                    trxCCPaymentDTO.paymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), gateway);
                            }
                        }
                        currTransaction.TransactionPaymentsDTOList.Add(trxCCPaymentDTO);
                        //staticDataExchange.PaymentModeDetail paymentModeDetail = new staticDataExchange.PaymentModeDetail();
                        //paymentModeDetail.CreditCardAuthorization = trxParams.CreditCardPaymentReference;
                        //paymentModeDetail.CreditCardExpiry = trxParams.CreditCardExpDate;
                        //paymentModeDetail.NameOnCard = trxParams.CreditCardName;
                        //paymentModeDetail.CreditCardName = trxParams.CreditCardType;
                        //paymentModeDetail.CreditCardNumber = "**********" + trxParams.CreditCardNumber.Substring(trxParams.CreditCardNumber.Length - 4);
                        ////paymentModeDetail.Amount = Convert.ToDouble((currTransaction.Net_Transaction_Amount - currTransaction.StaticDataExchange.PaymentRoundOffAmount));
                        //paymentModeDetail.Amount = Convert.ToDouble(currTransaction.Net_Transaction_Amount);

                        //paymentModeDetail.isCreditCard = true;
                        //paymentModeDetail.Gateway = PaymentGateways.None;
                        //paymentModeDetail.PaymentModeId = trxParams.PaymentModeId;

                        //paymentModeDetail.Reference = trxParams.PaymentReference; //25-May-2015:: Adding Reference received from Mercury to TrxPayments.Reference field Modified on 18/11/166 Rakshith
                        //ccPaymentProcessingSDA.PaymentModeDetails.Add(paymentModeDetail);

                        //ccPaymentProcessingSDA.PaymentCreditCardAmount = Convert.ToDouble((currTransaction.Net_Transaction_Amount));
                        //ccPaymentProcessingSDA.PaymentCreditCardSurchargeAmount = 0;
                        //ccPaymentProcessingSDA.PaymentModeId = paymentModeDetail.PaymentModeId;
                    }
                    int retCode = currTransaction.SaveTransacation(ref message);
                    if (retCode != 0)
                        throw new Exception(message);
                    else
                    {
                        try
                        {
                            bool roamingFlagsOn = false;
                            if (currTransaction.Utilities.getParafaitDefaults("ENABLE_ON_DEMAND_ROAMING").Equals("Y")
                                        && currTransaction.Utilities.getParafaitDefaults("AUTOMATIC_ON_DEMAND_ROAMING").Equals("Y")
                                        && currTransaction.Utilities.getParafaitDefaults("ALLOW_ROAMING_CARDS").Equals("Y")
                                        )
                            {
                                roamingFlagsOn = true;
                            }

                            foreach (Transaction.TransactionLine currTrxLine in currTransaction.TrxLines)
                            {
                                if (currTrxLine.card != null)
                                {
                                    AccountDTO updatedAccountDTO = new AccountBL(currTransaction.Utilities.ExecutionContext, currTrxLine.card.card_id, true, true).AccountDTO;
                                    DBSynchLogService dBSynchLogService = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "Cards", updatedAccountDTO.Guid, updatedAccountDTO.SiteId);
                                    dBSynchLogService.CreateRoamingData();
                                    if (updatedAccountDTO.AccountCreditPlusDTOList != null)
                                    {
                                        foreach (AccountCreditPlusDTO updatedAccountCreditPlus in updatedAccountDTO.AccountCreditPlusDTOList)
                                        {
                                            DBSynchLogService dBSynchLogServiceCP = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardCreditPlus", updatedAccountCreditPlus.Guid, updatedAccountCreditPlus.SiteId);
                                            dBSynchLogServiceCP.CreateRoamingData();
                                            if (roamingFlagsOn && updatedAccountDTO.SiteId != currTransaction.Utilities.ExecutionContext.GetSiteId())
                                            {
                                                DBSynchLogService dBSynchLogServiceCPVs = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardCreditPlus", updatedAccountCreditPlus.Guid, currTransaction.Utilities.ExecutionContext.GetSiteId());
                                                dBSynchLogServiceCPVs.CreateRoamingData();
                                            }

                                            if (updatedAccountCreditPlus.AccountCreditPlusConsumptionDTOList != null)
                                            {
                                                foreach (AccountCreditPlusConsumptionDTO updatedAccountCPConsumpDTO in updatedAccountCreditPlus.AccountCreditPlusConsumptionDTOList)
                                                {
                                                    DBSynchLogService dBSynchLogServiceCPConsp = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardCreditPlusConsumption", updatedAccountCPConsumpDTO.Guid, updatedAccountCPConsumpDTO.SiteId);
                                                    dBSynchLogServiceCPConsp.CreateRoamingData();
                                                    if (roamingFlagsOn && updatedAccountDTO.SiteId != currTransaction.Utilities.ExecutionContext.GetSiteId())
                                                    {
                                                        DBSynchLogService dBSynchLogServiceCPConspVs = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardCreditPlusConsumption", updatedAccountCPConsumpDTO.Guid, currTransaction.Utilities.ExecutionContext.GetSiteId());
                                                        dBSynchLogServiceCPConspVs.CreateRoamingData();
                                                    }
                                                }
                                            }
                                            if (updatedAccountCreditPlus.AccountCreditPlusPurchaseCriteriaDTOList != null)
                                            {
                                                foreach (AccountCreditPlusPurchaseCriteriaDTO updatedAccountCPPCDTO in updatedAccountCreditPlus.AccountCreditPlusPurchaseCriteriaDTOList)
                                                {
                                                    DBSynchLogService dBSynchLogServiceCPPC = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardCreditPlusPurchaseCriteria", updatedAccountCPPCDTO.Guid, updatedAccountCPPCDTO.SiteId);
                                                    dBSynchLogServiceCPPC.CreateRoamingData();
                                                    if (roamingFlagsOn && updatedAccountDTO.SiteId != currTransaction.Utilities.ExecutionContext.GetSiteId())
                                                    {
                                                        DBSynchLogService dBSynchLogServiceCPPCVs = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardCreditPlusPurchaseCriteria", updatedAccountCPPCDTO.Guid, currTransaction.Utilities.ExecutionContext.GetSiteId());
                                                        dBSynchLogServiceCPPCVs.CreateRoamingData();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (updatedAccountDTO.AccountGameDTOList != null)
                                    {
                                        foreach (AccountGameDTO updatedAccountGame in updatedAccountDTO.AccountGameDTOList)
                                        {
                                            DBSynchLogService dBSynchLogServiceGame = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardGames", updatedAccountGame.Guid, updatedAccountGame.SiteId);
                                            dBSynchLogServiceGame.CreateRoamingData();
                                            if (roamingFlagsOn && updatedAccountDTO.SiteId != currTransaction.Utilities.ExecutionContext.GetSiteId())
                                            {
                                                DBSynchLogService dBSynchLogServiceGameVs = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardGames", updatedAccountGame.Guid, currTransaction.Utilities.ExecutionContext.GetSiteId());
                                                dBSynchLogServiceGameVs.CreateRoamingData();
                                            }

                                            if (updatedAccountGame.AccountGameExtendedDTOList != null)
                                            {
                                                foreach (AccountGameExtendedDTO updatedAccountGameExtDTO in updatedAccountGame.AccountGameExtendedDTOList)
                                                {
                                                    DBSynchLogService dBSynchLogServiceGameExt = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardGameExtended", updatedAccountGameExtDTO.Guid, updatedAccountGameExtDTO.SiteId);
                                                    dBSynchLogServiceGameExt.CreateRoamingData();

                                                    if (roamingFlagsOn && updatedAccountDTO.SiteId != currTransaction.Utilities.ExecutionContext.GetSiteId())
                                                    {
                                                        DBSynchLogService dBSynchLogServiceGameExtVs = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardGameExtended", updatedAccountGameExtDTO.Guid, currTransaction.Utilities.ExecutionContext.GetSiteId());
                                                        dBSynchLogServiceGameExtVs.CreateRoamingData();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (updatedAccountDTO.AccountDiscountDTOList != null)
                                    {
                                        foreach (AccountDiscountDTO updatedAccountDiscount in updatedAccountDTO.AccountDiscountDTOList)
                                        {
                                            DBSynchLogService dBSynchLogServiceDiscount = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardDiscounts", updatedAccountDiscount.Guid, updatedAccountDiscount.SiteId);
                                            dBSynchLogServiceDiscount.CreateRoamingData();

                                            if (roamingFlagsOn && updatedAccountDTO.SiteId != currTransaction.Utilities.ExecutionContext.GetSiteId())
                                            {
                                                DBSynchLogService dBSynchLogServiceDiscountVs = new DBSynchLogService(currTransaction.Utilities.ExecutionContext, "CardDiscounts", updatedAccountDiscount.Guid, currTransaction.Utilities.ExecutionContext.GetSiteId());
                                                dBSynchLogServiceDiscountVs.CreateRoamingData();
                                            }
                                        }
                                    }

                                    if (roamingFlagsOn)
                                    {
                                        //account site id does not match with transaction site id. This can happen in virtual store scenario
                                        if (updatedAccountDTO.SiteId != currTransaction.Utilities.ExecutionContext.GetSiteId())
                                        {
                                            DBSynchLogDTO dbSynchLogDTO = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "Cards", currTransaction.Utilities.getServerTime(), currTransaction.Utilities.ExecutionContext.GetSiteId());
                                            DBSynchLogBL dbSynchLogBL = new DBSynchLogBL(currTransaction.Utilities.ExecutionContext, dbSynchLogDTO);
                                            dbSynchLogBL.Save();
                                        }
                                    }
                                }
                            }
                            try
                            {
                                SignWaiverEmail signWaiverEmail = new SignWaiverEmail(currTransaction.Utilities.ExecutionContext, currTransaction, currTransaction.Utilities);
                                signWaiverEmail.SendWaiverSigningLink(null);
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error in SendWaiverSigningLink", ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Create Roaming data failed for Transaction: " + currTransaction.Trx_id.ToString(), ex);
                        }
                        transactionStatus.Message = "Transaction Successfully Confirmed -" + currTransaction.Trx_id.ToString();
                        transactionStatus.IsExecuted = true;
                    }
                }

            }
            catch (Exception ex)
            {
                transactionStatus.Message = ex.Message;
            }
            finally
            {
                closeConnection();
            }

            return transactionStatus;
        }



        /// <summary>
        /// Cancel Purchase Transaction
        /// </summary>
        /// <param name="transactionParams">transactionParams</param>
        /// <returns> returns list of TransactionKeyValueStruct</returns>
        public TransactionStatus CancelPurchaseTransaction(TransactionParams trxParams)
        {
            transactionStatus = new TransactionStatus();
            string message = "";

            try
            {
                if (trxParams.TrxId <= 0 || trxParams.LoginId == "")
                {
                    transactionStatus.Message = "Invalid Transaction/Login Id";
                    return transactionStatus;
                }

                int TrxUserId = -1;
                trxParams.SiteId = GetTransactionSiteId(trxParams.TrxId, ref TrxUserId);
                UsersDTO userDTO = ValidateLogin(trxParams.LoginId, trxParams.SiteId);

                if (userDTO.UserId != TrxUserId)
                {
                    userDTO = new Users(machineUserContext, TrxUserId).UserDTO;
                }

                trxParams.LoginId = userDTO.LoginId;
                trxParams.UserId = userDTO.UserId;
                trxParams.RoleId = userDTO.RoleId;

                parafaitUtility.ParafaitEnv.SiteId = trxParams.SiteId;
                parafaitUtility.ParafaitEnv.LoginID = trxParams.LoginId;
                parafaitUtility.ParafaitEnv.User_Id = trxParams.UserId;
                parafaitUtility.ParafaitEnv.RoleId = trxParams.RoleId;
                parafaitUtility.ParafaitEnv.Initialize();

                if (parafaitUtility.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(parafaitUtility.ParafaitEnv.SiteId);
                    machineUserContext.SetIsCorporate(parafaitUtility.ParafaitEnv.IsCorporate);
                }

                TransactionUtils trxUtils = new TransactionUtils(parafaitUtility);

                Transaction currTransaction = new Transaction(parafaitUtility);
                currTransaction = trxUtils.CreateTransactionFromDB(trxParams.TrxId, parafaitUtility);

                //if (currTransaction.Status != Transaction.TrxStatus.OPEN)
                //{
                //    transactionStatus.Message = "Only OPEN Transaction allowed to be cancelled " + trxParams.TrxId.ToString() + " Status - " + currTransaction.Status;
                //    transactionStatus.Status = currTransaction.Status.ToString();
                //}
                //else
                //{
                //    trxUtils.reverseTransaction(trxParams.TrxId, -1, true, trxParams.PosIdentifier, trxParams.LoginId, trxParams.UserId, "Admin", trxParams.OrderRemarks, ref message);
                //    transactionStatus.IsExecuted = true;
                //    transactionStatus.Message = message;
                //}
                if (currTransaction.Status == Transaction.TrxStatus.OPEN || currTransaction.Status == Transaction.TrxStatus.BOOKING)
                {
                    trxUtils.reverseTransaction(trxParams.TrxId, -1, true, trxParams.PosIdentifier, trxParams.LoginId, trxParams.UserId, "Admin", trxParams.OrderRemarks, ref message);
                    transactionStatus.IsExecuted = true;
                    transactionStatus.Message = message;
                }
                else
                {
                    transactionStatus.Message = "Only OPEN or BOOKING Transaction allowed to be cancelled " + trxParams.TrxId.ToString() + " Status - " + currTransaction.Status;
                    transactionStatus.Status = currTransaction.Status.ToString();
                }

            }
            catch (Exception ex)
            {
                transactionStatus.Message = ex.Message;
            }
            finally
            {
                closeConnection();
            }

            return transactionStatus;
        }



        private bool checkIsCorporate()
        {
            string getSiteQuery = @"select count(*) sitecount from site";
            DataTable dtSites = dataAccessHandler.executeSelectQuery(getSiteQuery, null);

            if (Convert.ToInt32(dtSites.Rows[0]["sitecount"]) > 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// ValidateCard details based on the logged in agent user
        /// </summary>
        /// <param name="trxParams">TransactionParams</param>
        private void ValidateCard(TransactionParams trxParams)
        {
            if (trxParams.AutoPayDebitCard) // Get the Card# associcated with the login
            {
                DataTable dtCards = GetCardDetails(trxParams.LoginId);
                if (dtCards == null)
                    throw new Exception("Card Not Present!");
                if (dtCards.Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dtCards.Rows[0]["ExpiryDate"]).Date < ServerDateTime.Now.Date)
                    {
                        throw new Exception("Card is expired");
                    }

                    if (dtCards.Rows[0]["card_id"].ToString() != "" || dtCards.Rows[0]["card_id"] != null)
                    {
                        trxParams.PaymentCardNumber = dtCards.Rows[0]["card_number"].ToString();
                    }
                }
                else
                {
                    throw new Exception("Card Number do not exist");
                }
            }

        }

        /// <summary>
        /// GetCard details based on the logged in agent user
        /// </summary>
        /// <param name="loginId">string</param>
        public DataTable GetCardDetails(string loginId)
        {
            try
            {

                string selectCardQuery = @"select 
                                               card_id,card_number,  isnull(ExpiryDate ,GETDATE()) ExpiryDate, credits, site_id
                                               from   
                                               cards  c  
                                               where valid_flag='Y'  
                                               and refund_flag = 'N'
                                               and c.card_number not like 'T%'
                                               and customer_id in(select c.customer_id 
                                                                    from customers c,Partners p, users u ,Agents a 
                                                                    where u.user_id=a.User_Id and a.PartnerId=p.PartnerId  
                                                                    and p.Customer_Id=c.customer_id 
                                                                    and u.loginid=@loginId)";

                SqlParameter[] selectCardParameters = new SqlParameter[1];
                selectCardParameters[0] = new SqlParameter("@loginId", loginId);
                return dataAccessHandler.executeSelectQuery(selectCardQuery, selectCardParameters);
            }
            finally
            {

                closeConnection();
            }
        }



        /// <summary>
        /// Get Transaction SiteId 
        /// </summary>
        /// <param name="TrxId">int</param>
        private int GetTransactionSiteId(int TrxId, ref int TrxUserId)
        {
            try
            {
                string selectTrxQuery = @"select site_id,user_id from trx_header where TrxId=@TrxId ";
                SqlParameter[] selectTrxParameters = new SqlParameter[1];
                selectTrxParameters[0] = new SqlParameter("@TrxId", TrxId);

                DataTable dtTrx = dataAccessHandler.executeSelectQuery(selectTrxQuery, selectTrxParameters);
                if (dtTrx.Rows.Count == 1)
                {
                    TrxUserId = (dtTrx.Rows[0]["user_id"] == DBNull.Value ? -1 : Convert.ToInt32(dtTrx.Rows[0]["user_id"]));
                    return (dtTrx.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dtTrx.Rows[0]["site_id"]));
                }
                else
                {
                    throw new Exception("Transaction ID is Invalid! ");
                }

            }
            catch
            {
                throw;
            }

        }



        /// <summary>
        /// SaveAttractionBooking - Method used to save or remove attraction booking 
        /// </summary>
        /// <param name="AttractionBookingDTOList"></param>
        /// <returns></returns>
        public List<AttractionBookingDTO> SaveAttractionBooking(int siteId, List<AttractionBookingDTO> AttractionBookingDTOList)
        {
            log.LogMethodEntry(AttractionBookingDTOList);
            parafaitUtility.ParafaitEnv.SiteId = siteId;
            parafaitUtility.ParafaitEnv.Initialize();
            double businessDayStartTime = 6;
            double.TryParse(parafaitUtility.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);

            foreach (AttractionBookingDTO atbDTO in AttractionBookingDTOList)
            {
                DateTime scheduleDate = atbDTO.ScheduleFromDate;
                int offSetDuration = 0;
                scheduleDate = scheduleDate.Date.AddHours(businessDayStartTime);
                offSetDuration = timeZoneUtil.GetOffSetDuration(siteId, scheduleDate);
                atbDTO.ScheduleFromDate = atbDTO.ScheduleFromDate.AddSeconds(offSetDuration);
                atbDTO.ScheduleToDate = atbDTO.ScheduleToDate.AddSeconds(offSetDuration);

                AttractionBooking attractionBooking = new AttractionBooking(parafaitUtility.ExecutionContext, atbDTO);
                if (atbDTO.BookingId != -1)
                {
                    attractionBooking.Expire();
                }
                else
                {
                    //attractionBooking.AvailableUnits = (int)atbDTO.AvailableUnits;
                    //attractionBooking.BookedUnits = (int)atbDTO.BookedUnits;
                    //attractionBooking.AttractionPlayId = atbDTO.AttractionPlayId;
                    //attractionBooking.AttractionScheduleId = atbDTO.AttractionScheduleId;
                    //attractionBooking.ScheduleTime = atbDTO.ScheduleTime;
                    //attractionBooking.ScheduleToTime = 1;
                    //attractionBooking.Price = -1;
                    //attractionBooking.site_id = atbDTO.SiteId;

                    log.Info("Save Attraction Booking Save " + atbDTO.AvailableUnits);

                    //GG if (atbDTO != null && atbDTO.FacilitySeatsDTOList.Count > 0)
                    //{
                    //	foreach (FacilitySeatsDTO facilitySeatsDTO in atbDTO.FacilitySeatsDTOList)
                    //	{
                    //		attractionBooking.SelectedSeats.Add(facilitySeatsDTO.SeatId);
                    //		attractionBooking.SelectedSeatNames.Add(facilitySeatsDTO.SeatName);
                    //	}
                    //GG }

                    //ifGG (attractionBooking.Save(-1, null, ref message))
                    attractionBooking.Save(-1);
                    atbDTO.BookingId = attractionBooking.AttractionBookingDTO.BookingId;
                    //reverse the offset duration
                    atbDTO.ScheduleFromDate = atbDTO.ScheduleFromDate.AddSeconds(-1 * offSetDuration);
                    atbDTO.ScheduleToDate = atbDTO.ScheduleToDate.AddSeconds(-1 * offSetDuration);
                    //GG else
                    //{
                    //	throw new Exception(message);
                    //}
                }
            }

            log.LogMethodExit(null);
            return AttractionBookingDTOList;
        }



        private int CreateAttractionLine(LinkedPurchaseProductsStruct currProduct, TransactionParams transactionParams, Transaction currTransaction, List<Card> cardList, ref string message, int offsetDuration)
        {

            int lineSaveStatus = -1;
            try
            {
                //GGCheck
                foreach (AttractionBookingDTO attractionBookingsDTO in currProduct.AttractionBookingList)
                {
                    DataTable dtAttractionSchedule = new DataTable();
                    int attractionScheduleId = attractionBookingsDTO.AttractionScheduleId;
                    DateTime scheduleDate = currProduct.ScheduleDate;

                    string getAttractionScheduleIdQuery = @"select ats.AttractionScheduleId, 
                    						                        ap.AttractionPlayId,
                    						                        ap.PlayName , ats.ScheduleToTime
                                                        	    from AttractionSchedules ats,
                                                        	        AttractionPlays ap
                                                        	    where ap.AttractionPlayId = ats.AttractionPlayId 
                                                        	    and ats.AttractionScheduleId = @attractionScheduleId
                                                        	    and (ats.site_id is null or ats.site_id = @siteId)";

                    SqlParameter[] selectAttractionParameters = new SqlParameter[2];
                    selectAttractionParameters[0] = new SqlParameter("@siteId", transactionParams.SiteId);
                    selectAttractionParameters[1] = new SqlParameter("@attractionScheduleId", attractionScheduleId);

                    dtAttractionSchedule = dataAccessHandler.executeSelectQuery(getAttractionScheduleIdQuery, selectAttractionParameters);
                    if (dtAttractionSchedule.Rows.Count != 1)
                    {
                        message = "Attraction schedule has issues. Please check with admin. Attraction schedule id is " + attractionScheduleId;
                        throw new Exception(message);
                    }

                    AttractionBookingDTO attractionBookingDTO = new AttractionBookingDTO();
                    attractionBookingDTO.BookingId = attractionBookingsDTO.BookingId;
                    attractionBookingDTO.AttractionScheduleId = attractionScheduleId;
                    attractionBookingDTO.AttractionPlayId = attractionBookingsDTO.AttractionPlayId;
                    attractionBookingDTO.AttractionPlayName = attractionBookingsDTO.AttractionPlayName;
                    attractionBookingDTO.AvailableUnits = attractionBookingsDTO.AvailableUnits;
                    attractionBookingDTO.BookedUnits = attractionBookingsDTO.BookedUnits;
                    attractionBookingDTO.ScheduleFromDate = attractionBookingsDTO.ScheduleFromDate.AddSeconds(offsetDuration);
                    attractionBookingDTO.ScheduleToDate = attractionBookingsDTO.ScheduleToDate.AddSeconds(offsetDuration);
                    attractionBookingDTO.ScheduleFromTime = attractionBookingsDTO.ScheduleFromTime;
                    attractionBookingDTO.ScheduleToTime = attractionBookingsDTO.ScheduleToTime;
                    attractionBookingDTO.ExpiryDate = DateTime.MinValue;
                    attractionBookingDTO.FacilityMapId = attractionBookingsDTO.FacilityMapId;
                    attractionBookingDTO.Source = AttractionBookingDTO.SourceEnum.WALK_IN;

                    if (attractionBookingsDTO.Price != -1)
                    {
                        attractionBookingDTO.Price = attractionBookingsDTO.Price;
                    }

                    AttractionBooking attractionBooking = new AttractionBooking(machineUserContext, attractionBookingDTO);
                    //attractionBooking.AttractionBookingDTO.SelectedSeats = null;
                    //attractionBooking.AttractionBookingDTO.SelectedSeatNames = null;

                    if (attractionBookingsDTO != null && attractionBookingsDTO.AttractionBookingSeatsDTOList.Count > 0)
                    {
                        //foreach (FacilitySeatsDTO facilitySeatsDTO in attractionBookingsDTO.FacilitySeatsDTOList)
                        //{
                        //    attractionBooking.SelectedSeats.Add(facilitySeatsDTO.SeatId);
                        //    attractionBooking.SelectedSeatNames.Add(facilitySeatsDTO.SeatName);
                        //}
                        attractionBooking.AttractionBookingDTO.AttractionBookingSeatsDTOList = attractionBookingsDTO.AttractionBookingSeatsDTOList;
                    }

                    //lineSaveStatus = currTransaction.createTransactionLine(currCard, currProduct.ProductId, attractionBooking, currProduct.Price, currProduct.ProductQuantity, ref message);
                    lineSaveStatus = currTransaction.CreateAttractionProduct(currProduct.ProductId, attractionBookingDTO.Price != -1 ? attractionBookingDTO.Price : currProduct.Price, currProduct.ProductQuantity, currProduct.LinkLineId, attractionBooking, cardList, ref message);

                }
            }
            catch
            {
                return -1;
            }
            return lineSaveStatus;
        }


        /// <summary>
        /// GetPurchaseTranscations
        /// </summary>
        /// <param name="purListFilterDTO">PurchasesListFilterDTO</param>
        /// <returns> returns list of Transaction</returns>
        public List<TransactionDetails> GetPurchaseTransactions(PurchasesListParams purchasesListParams, bool showByCreationDate = false)
        {

            try
            {
                log.Debug("Starts- GetPurchaseTransactions(purchasesListParams) Method .");
                List<TransactionCoreDetailedPurchaseStruct> detailedPurchasesStructList = new List<TransactionCoreDetailedPurchaseStruct>();
                if ((purchasesListParams.UserId <= -1 && purchasesListParams.CustomerId <= -1) && string.IsNullOrEmpty(purchasesListParams.LoginId))
                {
                    throw new Exception("Invalid Login/Customer Id Parameters.");
                }

                string selectUserQuery = @"select h.*,
                                                getdate() ScheduleTime,
                                                tp.CreditCardAuthorization,tp.reference,tp.AdvancePaid,tp.CreditCardNumber,
                                                tp.CreditCardName, tp.PaymentDate,
                                                isnull(pm.PaymentMode,'') PaymentMode,
                                                b.BookingId,b.FromDate,b.ToDate,b.ReservationCode,
                                                b.Status BookingStatus,b.customerId bcustomerId,
                                                b.bookingName,b.age,b.gender,b.remarks bookingRemarks,
                                                b.Quantity Participants ,b.ExtraGuests,
                                                b.BookingProductId,h.site_id,
                                                (Select top 1 trs.SchedulesId
                                                   from TrxReservationSchedule trs 
                                                  where trs.TrxId = b.TrxId
                                               order by trs.Cancelled, trs.TrxId, trs.LineId )  AttractionScheduleId ,
											   (Select top 1 trs.FacilityMapId
                                                   from TrxReservationSchedule trs 
                                                  where trs.TrxId = b.TrxId 
                                               order by trs.Cancelled, trs.TrxId, trs.LineId ) as FacilityMapId,
											   (Select top 1 fm.FacilityMapName
                                                   from TrxReservationSchedule trs,
                                                        facilityMap fm 
                                                  where trs.TrxId = b.TrxId
                                                    and trs.FacilityMapId = fm.FacilityMapId
                                               order by trs.Cancelled, trs.TrxId, trs.LineId ) as FacilityDesc,
												h.Status, h.TransactionOTP, 
                                                case when (isnull(b.BookingId,0)>0 and b.FromDate<getdate() or b.Status='CANCELLED') 
                                                    then 'N' else 'Y' end as editStatus,
                                                h.user_id, h.CreationDate,h.Remarks
                                                from trx_header h
						                left join bookings b on (h.TrxId = b.TrxId and b.status not in ('SYSTEMABANDONED')) 
                                        --left outer join AttractionSchedules ats on (b.AttractionScheduleId=ats.AttractionScheduleId) 
                                        --left outer join CheckInFacility fac on (fac.FacilityId=ats.FacilityId) 
                                        left outer join (select top 1 trxid,PaymentModeId,
                                                                isnull(CreditCardAuthorization,'') CreditCardAuthorization,
                                                                isnull(reference,'') reference,
                                                                isnull(amount,0) AdvancePaid,
                                                                isnull(CreditCardNumber,'') CreditCardNumber,
                                                                isnull(CreditCardName,'') CreditCardName,
                                                                isnull(PaymentDate,'') PaymentDate
                                                            from trxpayments 
                                                            where trxid = @trxId and isnull(CreditCardNumber,'') <>'' ) tp on h.TrxId = tp.trxid 
                                        left outer join PaymentModes pm on pm.PaymentModeId=tp.PaymentModeId
                                        WHERE (isnull(h.customerId, -1) = case when @customerId = -1 then isnull(h.customerId, -1) else @CustomerId end) "
                                               + (purchasesListParams.TranscationId == -1 ? " " : " and (h.TrxId = @trxId ) ") + @" 
                                              and (case when @ShowAllTransaction = 1 then h.TrxId else isnull(h.Original_System_Reference, -1) end > 0 )
                                              and (case when @ShowAllTransaction = 1 then h.TrxId else 
                                                         case when h.status not in ('CANCELLED','SYSTEMABANDONED','OPEN', 'INITIATED','ORDERED','PREPARED') or (b.Status ='CANCELLED' and b.ReservationCode is not null) 
                                                            then 1 else -1 end
                                                        end > 0 )
                                              and (h.user_id = case when @userId = -1 then h.user_id else @userId end)
                                              and (isnull(b.ReservationCode, '')  = case when @ReservationCode = '' then isnull(b.ReservationCode, '')  else @ReservationCode end)
                                              and (case when @TransactionType = 'BOOKING' then isnull(b.BookingId , -1) else h.TrxId end > -1 )" + (showByCreationDate ? "and h.CreationDate between @fromDate and @toDate " : "and h.TrxDate between @fromDate and @toDate + 1 ") + @"order by trxid desc ";

                SqlParameter[] selectPurchaseParameters = new SqlParameter[8];
                selectPurchaseParameters[0] = new SqlParameter("@CustomerId", purchasesListParams.CustomerId);
                selectPurchaseParameters[1] = new SqlParameter("@userId", purchasesListParams.UserId);
                selectPurchaseParameters[2] = new SqlParameter("@fromDate", purchasesListParams.FromDate);
                selectPurchaseParameters[3] = new SqlParameter("@toDate", purchasesListParams.ToDate);
                selectPurchaseParameters[4] = new SqlParameter("@trxId", purchasesListParams.TranscationId);
                selectPurchaseParameters[5] = new SqlParameter("@ReservationCode", purchasesListParams.ReservationCode);
                selectPurchaseParameters[6] = new SqlParameter("@TransactionType", purchasesListParams.TransactionType);
                selectPurchaseParameters[7] = new SqlParameter("@ShowAllTransaction", (purchasesListParams.ShowAllTransaction == true ? 1 : 0));

                log.Debug(selectUserQuery);
                DataTable dtPurchase = dataAccessHandler.executeSelectQuery(selectUserQuery, selectPurchaseParameters);

                TransactionDetails transactionDetails = null;
                List<TransactionDetails> purchasesList = new List<TransactionDetails>();
                if (dtPurchase.Rows.Count > 0)
                {
                    foreach (DataRow purchaseRow in dtPurchase.Rows)
                    {
                        int siteId = purchaseRow["site_id"].ToString() == "" ? -1 : Convert.ToInt32(purchaseRow["site_id"]);
                        int offSetDuration = timeZoneUtil.GetOffSetDuration(siteId, Convert.ToDateTime(purchaseRow["trxdate"])) * (-1);

                        transactionDetails = new TransactionDetails(
                                                purchaseRow["trxid"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["trxid"]),
                                                Convert.ToDateTime(purchaseRow["trxdate"]).AddSeconds(offSetDuration),
                                                purchaseRow["TrxNetAmount"] == DBNull.Value ? -1 : Convert.ToDouble(purchaseRow["TrxNetAmount"]),
                                                purchaseRow["TaxAmount"] == DBNull.Value ? -1 : Convert.ToDouble(purchaseRow["TaxAmount"]));

                        //currTransaction.LineId = purchaseRow["lineid"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["lineid"]);  //added 
                        transactionDetails.PaymentMode = purchaseRow["PaymentMode"].ToString();
                        transactionDetails.CustomerId = purchaseRow["customerId"].ToString() == "" ? -1 : Convert.ToInt32(purchaseRow["customerId"]);
                        transactionDetails.Original_System_Reference = purchaseRow["Original_System_Reference"] == DBNull.Value ? "" : purchaseRow["Original_System_Reference"].ToString();
                        transactionDetails.TransactionReference = purchaseRow["reference"] == DBNull.Value ? "" : purchaseRow["reference"].ToString();
                        transactionDetails.Remarks = purchaseRow["Remarks"] == DBNull.Value ? "" : purchaseRow["Remarks"].ToString();

                        transactionDetails.BookingId = purchaseRow["BookingId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["BookingId"]);
                        transactionDetails.ReservationCode = purchaseRow["ReservationCode"] == DBNull.Value ? "" : purchaseRow["ReservationCode"].ToString();
                        transactionDetails.LastFourDigitOfCC = purchaseRow["CreditCardNumber"] == DBNull.Value ? "" : purchaseRow["CreditCardNumber"].ToString();
                        transactionDetails.CreditCardType = purchaseRow["CreditCardName"] == DBNull.Value ? "" : purchaseRow["CreditCardName"].ToString();
                        transactionDetails.TrxPaymentDate = purchaseRow["PaymentDate"] == DBNull.Value ? ServerDateTime.Now.AddSeconds(offSetDuration) : Convert.ToDateTime(purchaseRow["PaymentDate"]).AddSeconds(offSetDuration);

                        DateTime resFromDate = DateTime.MinValue;
                        DateTime resToDate = DateTime.MinValue;

                        transactionDetails.ReservationDateTime = "";

                        if (purchaseRow["FromDate"] != DBNull.Value)
                        {
                            int duration = (int)(Convert.ToDateTime(purchaseRow["ToDate"]) - Convert.ToDateTime(purchaseRow["FromDate"])).TotalMinutes;
                            int offSeDurationResFromDate = timeZoneUtil.GetOffSetDuration(siteId, Convert.ToDateTime(purchaseRow["FromDate"])) * (-1);
                            resFromDate = Convert.ToDateTime(purchaseRow["FromDate"]).AddSeconds(offSeDurationResFromDate);
                            resToDate = resFromDate.AddMinutes(duration);
                            transactionDetails.ReservationDateTime = resFromDate.ToShortDateString() + "   " + resFromDate.ToShortTimeString() + " - " + resToDate.ToShortTimeString();
                        }

                        transactionDetails.TransactionStatus = purchaseRow["Status"] == DBNull.Value ? "" : purchaseRow["Status"].ToString();
                        transactionDetails.TransactionOTP = purchaseRow["TransactionOTP"] == DBNull.Value ? string.Empty : purchaseRow["TransactionOTP"].ToString();
                        transactionDetails.Status = purchaseRow["BookingStatus"] == DBNull.Value ? "" : purchaseRow["BookingStatus"].ToString();
                        transactionDetails.CustomerId = purchaseRow["bcustomerId"] == DBNull.Value ? transactionDetails.CustomerId : Convert.ToInt32(purchaseRow["bcustomerId"]);
                        transactionDetails.TransactionGrandTotal = transactionDetails.TransactionTotal;
                        transactionDetails.TransactionRoundOffAmount = Math.Round(transactionDetails.TransactionGrandTotal, parafaitUtility.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                        transactionDetails.SiteId = purchaseRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["site_id"]);
                        transactionDetails.EditStatus = purchaseRow["EditStatus"].ToString();
                        transactionDetails.AdvancePaid = purchaseRow["AdvancePaid"] == DBNull.Value ? 0.0 : Convert.ToDouble(purchaseRow["AdvancePaid"]);

                        if (purchaseRow["CreationDate"] != DBNull.Value)
                        {
                            offSetDuration = timeZoneUtil.GetOffSetDuration(siteId, Convert.ToDateTime(purchaseRow["CreationDate"])) * (-1);
                            transactionDetails.CreationDate = Convert.ToDateTime(purchaseRow["CreationDate"]).AddSeconds(offSetDuration);
                        }

                        purchasesList.Add(transactionDetails);

                        if (purchasesListParams.ShowTransactionLines)
                        {
                            //string selectNewCardQuery = @"select l.card_id
                            //                                  from trx_header h, trx_lines l
                            //                                 where h.TrxId=@trxId and h.trxid = l.trxid 
                            //                                   and exists (select 'x' 
                            //                                                 from cards c 
                            //                                                where c.card_id = l.card_id 
                            //                                                and DATEDIFF(ss, c.issue_date, h.CreationDate) <= 10) ";

                            string selectNonRechargeQuery = @"select l.LineId, l.TrxId from trx_header h, trx_lines l, products p
                                                            where h.TrxId=@trxId and h.trxid = l.trxid 
                                                                and l.product_id = p.product_id
                                                                and not exists(select 'x' from product_type where p.product_type_id =  product_type_id  and product_type ='RECHARGE') ";

                            SqlParameter[] selectNonRechargeCheckParameters = new SqlParameter[1];
                            selectNonRechargeCheckParameters[0] = new SqlParameter("@trxId", purchasesListParams.TranscationId);

                            DataTable dtNewcards = dataAccessHandler.executeSelectQuery(selectNonRechargeQuery, selectNonRechargeCheckParameters);
                            if (dtNewcards.Rows.Count <= 0)
                            {
                                transactionDetails.TransactionOTP = "";
                            }
                            if (transactionDetails.BookingId != -1)
                            {
                                ReservationBL reservationBL = new ReservationBL(machineUserContext, parafaitUtility, transactionDetails.BookingId);

                                //bookingDetails.BookingName = purchaseRow["BookingId"] == DBNull.Value ? "" : purchaseRow["bookingName"].ToString();
                                // bookingDetails.Age = purchaseRow["age"] == DBNull.Value ? 0 : Convert.ToInt32(purchaseRow["age"]);
                                // bookingDetails.BookingName = purchaseRow["BookingName"] == DBNull.Value ? "" : purchaseRow["BookingName"].ToString();
                                // bookingDetails.Quantity = purchaseRow["Participants"] == DBNull.Value ? 0 : Convert.ToInt32(purchaseRow["Participants"]);
                                //bookingDetails.ExtraGuests = purchaseRow["ExtraGuests"] == DBNull.Value ? 0 : Convert.ToInt32(purchaseRow["ExtraGuests"]);
                                //bookingDetails.Remarks = purchaseRow["bookingRemarks"] == DBNull.Value ? "" : purchaseRow["bookingRemarks"].ToString();
                                //bookingDetails.bookingProductId = purchaseRow["BookingProductId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["BookingProductId"]);
                                //bookingDetails.attractionScheduleId = purchaseRow["AttractionScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["AttractionScheduleId"]);
                                //bookingDetails.facilityId = purchaseRow["FacilityId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["FacilityId"]);
                                //bookingDetails.FacilityName = purchaseRow["FacilityDesc"] == DBNull.Value ? "" : purchaseRow["FacilityDesc"].ToString();

                                //if (resFromDate != DateTime.MinValue)
                                //{
                                //    bookingDetails.reservationFromDateTime = resFromDate;
                                //    bookingDetails.ExpiryTime = resToDate;
                                //}

                                //if (bookingDetails.Remarks.Contains(","))
                                //{
                                //    bookingDetails.Channel = bookingDetails.Remarks.Split(',')[1];
                                //    bookingDetails.Remarks = bookingDetails.Remarks.Split(',')[0];
                                //}

                                //bookingDetails.Channel = (bookingDetails.Channel == null ? "" : bookingDetails.Channel);

                                reservationBL.BookingTransaction.LoadAttendeeDetails();
                                transactionDetails.BookingAttendeeList = reservationBL.BookingTransaction.BookingAttendeeList;

                                //bookingDetails.Remarks = (bookingDetails.Remarks == null ? "" : bookingDetails.Remarks);
                                transactionDetails.ReservationDTO = reservationBL.GetReservationDTO;
                                int offSetDurationRes = timeZoneUtil.GetOffSetDuration(siteId, reservationBL.GetReservationDTO.FromDate) * (-1);
                                transactionDetails.ReservationDTO.FromDate = transactionDetails.ReservationDTO.FromDate.AddSeconds(offSetDurationRes);
                                transactionDetails.ReservationDTO.ToDate = transactionDetails.ReservationDTO.ToDate.AddSeconds(offSetDurationRes);
                            }

                            transactionDetails.TransactionUserDTO = GetTransactionUser(Convert.ToInt32(purchaseRow["user_id"]));
                            GetTransactionLines(ref transactionDetails, offSetDuration);

                            // Get Discount Summary 
                            if (purchasesListParams.TranscationId > 0)
                            {
                                parafaitUtility.ParafaitEnv.SiteId = transactionDetails.SiteId;
                                Transaction currPosTransaction = GetTransactionByTrxId(purchasesListParams.TranscationId, "External POS");
                                if (currPosTransaction.DiscountsSummaryDTOList.Count() > 0)
                                    transactionDetails.DiscountSummary = GetDiscountCouponsSummary(currPosTransaction);

                                //if (currPosTransaction.WaiversSignedHistoryDTOList != null &&  currPosTransaction.WaiversSignedHistoryDTOList.Count() > 0)
                                //    transactionDetails.WaiverSignedSummary = GetWaiverSummary(currPosTransaction.WaiversSignedHistoryDTOList);

                                log.Debug("Discount summary loaded" + transactionDetails.DiscountSummary);


                                if ((transactionDetails.TransactionStatus.ToUpper() == Transaction.TrxStatus.RESERVED.ToString()
                                      || transactionDetails.TransactionStatus.ToUpper() == Transaction.TrxStatus.CLOSED.ToString())
                                    && currPosTransaction.WaiverSignatureRequired())
                                {
                                    try
                                    {
                                        parafaitUtility.ParafaitEnv.LoginID = "External POS";
                                        machineUserContext.SetSiteId(parafaitUtility.ParafaitEnv.SiteId);
                                        SignWaiverEmail signWaiverEmail = new SignWaiverEmail(machineUserContext, currPosTransaction, parafaitUtility);
                                        StringBuilder stbJsonData = new StringBuilder();
                                        stbJsonData.Append("[{");
                                        stbJsonData.Append("\"SigningStatus\":\"" + (currPosTransaction.IsWaiverSignaturePending() == true ? "Y" : "N") + "\",");
                                        stbJsonData.Append("\"SigningLink\":\"" + signWaiverEmail.GenerateWaiverSigningLink(null) + "\"");
                                        stbJsonData.Append("}]");
                                        transactionDetails.WaiverSignedSummary = stbJsonData.ToString();
                                        log.LogVariableState("WaiverSignedSummary", stbJsonData.ToString());

                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("Waiver Signature :" + currPosTransaction.Trx_id.ToString(), ex);
                                    }
                                }

                                transactionDetails.TaxSummary = "";
                                currPosTransaction.TransactionInfo.createTransactionInfo(purchasesListParams.TranscationId, -1);
                                if (currPosTransaction.TransactionInfo.TrxTax != null && currPosTransaction.TransactionInfo.TrxTax.Count > 0)
                                {
                                    transactionDetails.TaxSummary = new JavaScriptSerializer().Serialize(currPosTransaction.TransactionInfo.TrxTax);
                                    log.Debug("Tax summary loaded" + transactionDetails.TaxSummary);
                                }
                            }

                        }
                    }
                }

                log.Debug("Ends- GetPurchaseTransactions(purchasesListParams) Method .");
                return purchasesList;
            }
            catch (Exception ex)
            {
                log.Log("Error - GetPurchaseTransactions() Method", ex);
                throw;
            }
            finally
            {
                closeConnection();
            }

        }

        /// <summary>
        /// GetTransactionUser
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>TransactionUser</returns>
        private TransactionUser GetTransactionUser(int userId)
        {

            TransactionUser transactionUser = new TransactionUser();

            string selectTrUserQuery = @"select u.user_id,
                                                username, loginid, isnull(MobileNo,'') MobileNo,
                                                isnull(u.email,'') email,
                                                case when a.AgentId is null then 'N' else 'Y' end as IsAgent
                                            from users u left outer join agents a on u.user_id = a.User_Id 
                                            where u.User_Id = @userId ";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@userId", userId);
            DataTable dtUser = dataAccessHandler.executeSelectQuery(selectTrUserQuery, selectUserParameters);

            if (dtUser.Rows.Count > 0)
            {
                foreach (DataRow userRow in dtUser.Rows)
                {
                    transactionUser = new TransactionUser(Convert.ToInt32(userRow["user_id"]),
                                                                        userRow["username"].ToString(),
                                                                        userRow["loginid"].ToString(),
                                                                        userRow["MobileNo"].ToString(),
                                                                        userRow["IsAgent"].ToString(),
                                                                        userRow["email"].ToString());

                }
            }

            return transactionUser;

        }


        /// <summary>
        /// GetTransactionLines
        /// </summary>
        /// <param name="currTran"></param>
        private void GetTransactionLines(ref TransactionDetails currTran, int offsetDuration)
        {
            double totalAmount = 0;
            double taxAmount = 0;
            double discount = 0;
            double tranasctionAmount;

            if (currTran == null)
                return;


            currTran.TransactionTotal = 0;

            string selectPurchaseLines = @"select tv.*,isnull(l.card_number,'') card_number, l.ParentLineId, 
                                        das.ScheduleDateTime as ScheduleTime ,
                                        das.AttractionScheduleId,
                                        isnull(abs.ScheduleToTime,0) as ScheduleToTime, 
                                        das.FacilityMapId,
                                        das.ScheduleToDateTime as ScheduleToDate
                                         from TransactionView tv 
                                                    inner join trx_lines l on l.TrxId = tv.trxid and l.lineid=tv.lineid
													left join attractionbookings ab on ab.TrxId= @trxId and  ab.LineId=tv.lineid
													left join AttractionSchedules abs on abs.AttractionScheduleId= ab.AttractionScheduleId 
                                                    left join DayAttractionSchedule das on ab.DayAttractionScheduleId= das.DayAttractionScheduleId
                                        where ( tv.trxid = @trxId)
                                         order by l.lineid ";

            SqlParameter[] selectPurchaseParameters = new SqlParameter[1];
            selectPurchaseParameters[0] = new SqlParameter("@trxId", currTran.TransactionId);
            DataTable dtPurchaseLines = dataAccessHandler.executeSelectQuery(selectPurchaseLines, selectPurchaseParameters);

            List<TransactionDetails> purchasesList = new List<TransactionDetails>();
            if (dtPurchaseLines.Rows.Count > 0)
            {
                //foreach (int iDataRow purchaseRow in dtPurchaseLines.Rows)

                int TrxLineCount = dtPurchaseLines.Rows.Count;

                for (int i = 0; i < TrxLineCount; i++)
                {
                    DataRow purchaseRow = dtPurchaseLines.Rows[i];

                    Double TotPrice = Convert.ToDouble(purchaseRow["Trx_Price"]);

                    if (!string.IsNullOrEmpty(purchaseRow["card_number"].ToString()) && purchaseRow["product_type"].ToString() != "CARDDEPOSIT")
                    {
                        foreach (DataRow purchaseRow1 in dtPurchaseLines.Rows)
                        {
                            if (purchaseRow1["card_number"].ToString() == purchaseRow["card_number"].ToString() && purchaseRow1["product_type"].ToString() == "CARDDEPOSIT")
                            {
                                TotPrice += Convert.ToDouble(purchaseRow1["Trx_Price"]);
                            }
                        }
                    }

                    LinkedPurchasedProducts currProduct = new LinkedPurchasedProducts(
                                purchaseRow["product_id"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["product_id"]),
                                purchaseRow["product_name"] == DBNull.Value ? "" : purchaseRow["product_name"].ToString(),
                                purchaseRow["lineid"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["lineid"]),
                                purchaseRow["quantity"] == DBNull.Value ? 0 : Convert.ToInt32(purchaseRow["quantity"]),
                                purchaseRow["Trx_Price"] == DBNull.Value ? 0 : TotPrice,
                                purchaseRow["Tax"] == DBNull.Value ? 0 : Convert.ToDouble(purchaseRow["Tax"]),
                                purchaseRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["card_id"]),
                                purchaseRow["card_number"].ToString(),
                                purchaseRow["lineid"] == DBNull.Value ? "-1" : purchaseRow["lineid"].ToString(),
                                purchaseRow["ParentLineId"] == DBNull.Value ? "-1" : purchaseRow["ParentLineId"].ToString(),
                                purchaseRow["Line_Remarks"] == DBNull.Value ? "" : purchaseRow["Line_Remarks"].ToString()
                            );

                    currProduct.AttractionScheduleId = purchaseRow["AttractionScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["AttractionScheduleId"]);
                    currProduct.ScheduleDate = purchaseRow["ScheduleTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseRow["ScheduleTime"]);
                    currProduct.ScheduleToDate = purchaseRow["ScheduleToDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseRow["ScheduleToDate"]);

                    log.Debug("ScheduleDate before offset calculation- ScheduleFromDate: " + currProduct.ScheduleDate + " ScheduleToDate: " + currProduct.ScheduleToDate);

                    if (currProduct.AttractionScheduleId != -1)
                    {
                        AttractionBookingDTO atbDTO = new AttractionBookingDTO();
                        atbDTO.AttractionScheduleId = currProduct.AttractionScheduleId;
                        atbDTO.AttractionProductId = currProduct.ProductId;
                        atbDTO.FacilityMapId = purchaseRow["FacilityMapId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["FacilityMapId"]);

                        //13-March-2023 - Fix to handle time offset for Daylight saving time scenario
                        int siteId = purchaseRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseRow["site_id"]);
                        int offSetDurationSchFromDate = timeZoneUtil.GetOffSetDuration(siteId, currProduct.ScheduleDate) * (-1);
                        int offSetDurationSchToDate = timeZoneUtil.GetOffSetDuration(siteId, currProduct.ScheduleToDate) * (-1);

                        atbDTO.ScheduleFromDate = currProduct.ScheduleDate.AddSeconds(offSetDurationSchFromDate);
                        atbDTO.ScheduleToDate = currProduct.ScheduleToDate.AddSeconds(offSetDurationSchToDate);

                        log.Debug("ScheduleDate after offset calculation- ScheduleFromDate: " + atbDTO.ScheduleFromDate + " ScheduleToDate: " + atbDTO.ScheduleToDate);

                        if (Convert.ToDouble(purchaseRow["ScheduleToTime"]) >= 0.00 && Convert.ToDouble(purchaseRow["ScheduleToTime"]) <= 6.00)
                        {
                            atbDTO.ExpiryDate = currProduct.ScheduleDate.Date.AddDays(1).AddHours(Convert.ToDouble(purchaseRow["ScheduleToTime"]));
                        }
                        else
                        {
                            atbDTO.ExpiryDate = currProduct.ScheduleDate.Date.AddHours(Convert.ToDouble(purchaseRow["ScheduleToTime"]));
                        }
                        currProduct.AttractionBookingList.Add(atbDTO);
                    }

                    currProduct.ProductType = purchaseRow["product_type"] == DBNull.Value ? "" : purchaseRow["product_type"].ToString();
                    totalAmount = totalAmount + currProduct.TotalAmount;
                    taxAmount = taxAmount + currProduct.TaxAmount;


                    if (purchaseRow["product_type"].ToString() != "DISCOUNT")
                    {
                        if (double.TryParse(purchaseRow["Net_Amount"].ToString(), out tranasctionAmount))
                            currTran.TransactionTotal += tranasctionAmount;
                    }

                    if (currProduct.ProductType != "CARDDEPOSIT" && currProduct.ProductType != "LOYALTY")
                        currTran.AddProduct(currProduct);

                    if (purchaseRow["product_type"].ToString() == "DISCOUNT")
                    {
                        discount += purchaseRow["amount"] == DBNull.Value ? 0 : Convert.ToDouble(purchaseRow["amount"]);
                    }
                }

                currTran.DiscountAmount = Math.Abs(discount);

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
                parafaitUtility.ParafaitEnv.Initialize();
                parafaitUtility.ParafaitEnv.SiteId = siteId;
                Messaging messaging = new Messaging(parafaitUtility);
                return messaging.sendSMSSynchronous(number, message);
            }
            catch (Exception ex)
            {
                log.Log("Error - SendMessage() Method", ex);
                throw;
            }
            finally
            {
                closeConnection();
            }
        }


        private void closeConnection()
        {
            if (parafaitUtility.sqlConnection != null && parafaitUtility.sqlConnection.State == ConnectionState.Open)
            {
                parafaitUtility.sqlConnection.Close();
            }
            parafaitUtility.Dispose();
        }


        private UsersDTO ValidateLogin(string LoginId, int SiteId = -1)
        {

            if (LoginId.ToString() == null && LoginId.ToString() == "")
            {
                throw new Exception("Login Id is Invalid!");
            }

            UsersDTO userDTO;
            if (parafaitUtility.ParafaitEnv.IsCorporate)
            {
                userDTO = new Users(parafaitUtility.ExecutionContext, LoginId, SiteId).UserDTO;
            }
            else
            {
                userDTO = new Users(parafaitUtility.ExecutionContext, LoginId).UserDTO;
            }

            if (userDTO == null || userDTO.UserId <= 0)
            {
                throw new Exception("Login Id does not exist!");
            }

            return userDTO;


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
            log.Debug("Starts- GetTransactionReceipt(int trxId, string macAddress, int width, int height) Method .");
            try
            {
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.IP_ADDRESS, macAddress));
                POSMachineList posMachineList = new POSMachineList(machineUserContext);
                List<POSMachineDTO> posMachines = posMachineList.GetAllPOSMachines(searchParameters);
                POSMachines posMachine = new POSMachines(machineUserContext, posMachines[0].POSMachineId);
                List<POSPrinterDTO> posPrintersDTOList = posMachine.PopulatePrinterDetails();
                Transaction currTransaction = GetTransactionByTrxId(trxId, macAddress);

                currTransaction.TransactionInfo.createTransactionInfo(currTransaction.Trx_id, -1);

                int printerId = GetPosPrinterId(macAddress);
                POSPrinterDTO posPrinterDTO = posPrintersDTOList.Find(p => p.PrinterId == printerId);
                PrintTransaction printTransaction = new PrintTransaction(posPrintersDTOList);
                log.Debug("Ends- GetTransactionReceipt(int trxId, string macAddress, int width, int height) Method .");
                return printTransaction.printPosReceipt(currTransaction, posPrinterDTO, -1, width, height, secondaryPrint);

            }
            catch (Exception ex)
            {
                log.Log("Error- GetTransactionReceipt(int trxId, string macAddress, int width, int height) Method by throwing exception .", ex);
                throw;
            }
            finally
            {
                closeConnection();
            }

        }

        /// <summary>
        /// GetPosPrinterId
        /// </summary>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        private int GetPosPrinterId(String macAddress)
        {
            int printerId = -1;
            try
            {
                string selectPosPrinterIdQuery = @"select isnull(PP1.PrinterId,'-1') as PrinterId 
                                                            from POSMachines PM 
                                                            left join (
                                                                       select pp.POSMachineId,PP.PrinterId
           																	   from  POSPrinters PP, Lookupview lv
																			  where  pp.PrinterTypeId = lv.LookupValueId 
                                                                                and lv.LookupName = 'PRINTER_TYPE'
                                                                                and lv.LookupValue = 'ReceiptPrinter'
                                                                       ) PP1 on PM.POSMachineId = PP1.POSMachineId
                                                    where PM.IPAddress = @ipAddress";
                SqlParameter[] selectParameters = new SqlParameter[1];
                selectParameters[0] = new SqlParameter("@ipAddress", macAddress);
                DataTable dtPrinter = dataAccessHandler.executeSelectQuery(selectPosPrinterIdQuery, selectParameters);
                if (dtPrinter.Rows.Count > 0)
                {
                    printerId = Convert.ToInt32(dtPrinter.Rows[0]["PrinterId"].ToString());
                }
            }
            catch (Exception ex)
            {
                log.Log("Error- GetPosPrinterId(string macAddress) throwing exception .", ex);
                throw;
            }
            return printerId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="trxParams"></param>
        /// <returns></returns>
        public TransactionStatus CancelPurchaseTransactionLine(TransactionParams trxParams)
        {
            transactionStatus = new TransactionStatus();

            try
            {
                if (trxParams.TrxId <= 0 || trxParams.LoginId == "")
                {
                    transactionStatus.Message = "Invalid Transaction/Login Id";
                    return transactionStatus;
                }
                else if (trxParams.TrxLineId <= 0)
                {
                    transactionStatus.Message = "Invalid Transaction Line Id";
                    return transactionStatus;
                }

                int TrxUserId = -1;
                trxParams.SiteId = GetTransactionSiteId(trxParams.TrxId, ref TrxUserId);
                UsersDTO userDTO = ValidateLogin(trxParams.LoginId, trxParams.SiteId);

                if (userDTO.UserId != TrxUserId)
                {
                    userDTO = new Users(machineUserContext, TrxUserId).UserDTO;
                }

                trxParams.LoginId = userDTO.LoginId;
                trxParams.UserId = userDTO.UserId;
                trxParams.RoleId = userDTO.RoleId;

                parafaitUtility.ParafaitEnv.SiteId = trxParams.SiteId;
                parafaitUtility.ParafaitEnv.LoginID = trxParams.LoginId;
                parafaitUtility.ParafaitEnv.User_Id = trxParams.UserId;
                parafaitUtility.ParafaitEnv.RoleId = trxParams.RoleId;
                parafaitUtility.ParafaitEnv.Initialize();

                TransactionUtils trxUtils = new TransactionUtils(parafaitUtility);

                Transaction currTransaction = new Transaction(parafaitUtility);
                currTransaction = trxUtils.CreateTransactionFromDB(trxParams.TrxId, parafaitUtility);

                if (currTransaction.Status != Transaction.TrxStatus.OPEN && currTransaction.Status != Transaction.TrxStatus.INITIATED
                    && currTransaction.Status != Transaction.TrxStatus.ORDERED && currTransaction.Status != Transaction.TrxStatus.PREPARED)
                {
                    transactionStatus.Message = "Only OPEN Transaction allowed to be cancelled " + trxParams.TrxId.ToString() + " Status - " + currTransaction.Status;
                }
                else
                {
                    for (int i = 0; i < currTransaction.TrxLines.Count; i++)
                    {
                        if (currTransaction.TrxLines[i].DBLineId == trxParams.TrxLineId)
                        {
                            currTransaction.CancelTransactionLine(i);
                        }
                    }

                    string message = "";
                    int retCode = currTransaction.SaveOrder(ref message);

                    if (retCode != 0)
                        throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                transactionStatus.Message = ex.Message;
            }
            finally
            {
                closeConnection();
            }

            return transactionStatus;
        }

        /// <summary>
        /// get transaction by trx id
        /// </summary>
        /// <param name="trxId"> transaction id</param>
        /// <param name="macAddress">pos mac id</param>
        /// <returns>returns transaction object</returns>
        public Transaction GetTransactionByTrxId(int trxId, string macAddress)
        {
            log.Debug("Starts- GetTransactionByTrxId(int trxId, string macAddress) method .");
            parafaitUtility.ParafaitEnv.Initialize();
            parafaitUtility.ParafaitEnv.SetPOSMachine(macAddress, macAddress);

            TransactionUtils trxUtils = new TransactionUtils(parafaitUtility);
            Transaction currTransaction = new Transaction(parafaitUtility);
            currTransaction = trxUtils.CreateTransactionFromDB(trxId, parafaitUtility);
            log.Debug("Ends- GetTransactionByTrxId(int trxId, string macAddress) method .");
            return currTransaction;
        }

        public string GetTransactionStatusByTrxId(int trxId)
        {
            log.Debug("Starts- GetTransactionByTrxId(int trxId, string macAddress) method .");

            string status = string.Empty;

            string selectPosPrinterIdQuery = @"select Status 
                                                            from trx_header 
                                                    where TrxId = @trxId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@trxId", trxId);
            DataTable dtPrinter = dataAccessHandler.executeSelectQuery(selectPosPrinterIdQuery, selectParameters);
            if (dtPrinter.Rows.Count > 0)
            {
                status = (dtPrinter.Rows[0]["Status"].ToString());
            }
            return status;
        }

        public void Dispose()
        {
            ((IDisposable)parafaitUtility).Dispose();
        }

        /// <summary>
        /// GetTransactionsByDateTime
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Transactions list</returns>
        public List<TransactionDetails> GetTransactionsByDateTime(DateTime lastRunTime, DateTime currentRunTime, int siteId)
        {
            log.LogMethodEntry(lastRunTime, currentRunTime, siteId);
            List<TransactionDetails> transactionDetailsList = new List<TransactionDetails>();

            string selectTrUserQuery = @"select trxId, trxDate, trxNetAmount, TaxAmount
                                          from trx_header
                                         where trx_header.LastUpdateTime >= @LastRunTime 
                                           and trx_header.LastUpdateTime < @CurrentRunTime
                                           and status = 'CLOSED'
                                           and (site_id = @SiteId or @SiteId = -1) 
                                           and NOT EXISTS (SELECT 1 from LoyaltyBatchProcess lbp where lbp.transactionId = trx_header.TrxId ) ";
            SqlParameter[] selectParameters = new SqlParameter[3];
            selectParameters[0] = new SqlParameter("@SiteId", siteId);
            selectParameters[1] = new SqlParameter("@LastRunTime", lastRunTime);
            selectParameters[2] = new SqlParameter("@CurrentRunTime", currentRunTime);
            DataTable dtTrx = dataAccessHandler.executeSelectQuery(selectTrUserQuery, selectParameters);

            if (dtTrx.Rows.Count > 0)
            {
                foreach (DataRow trxRow in dtTrx.Rows)
                {
                    TransactionDetails transactionDetails = new TransactionDetails(
                                                trxRow["trxid"] == DBNull.Value ? -1 : Convert.ToInt32(trxRow["trxid"]),
                                                Convert.ToDateTime(trxRow["trxdate"]),
                                                trxRow["TrxNetAmount"] == DBNull.Value ? -1 : Convert.ToDouble(trxRow["TrxNetAmount"]),
                                                trxRow["TaxAmount"] == DBNull.Value ? -1 : Convert.ToDouble(trxRow["TaxAmount"]));
                    transactionDetailsList.Add(transactionDetails);
                }
            }

            return transactionDetailsList;

        }

    }
}

