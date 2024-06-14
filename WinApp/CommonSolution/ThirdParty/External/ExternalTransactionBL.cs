/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the card details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.140.3    22-Jul-2022   Abhishek                 Created : External  REST API.
 *2.150.4    28-Sep-2023   Abhishek                 Modified : Generate transactionOTP in single site 
                                                               if GetLocalTransactionOTP is true. 
 *2.151.0    11-Oct-2023   Abhishek                 Modified : The debit card payment support for the transaction.                                                           
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Semnox.Parafait.ThirdParty.External
{
    /// <summary>
    /// Bussiness logic of the ExternalTransactionBL class
    /// </summary>
    public class ExternalTransactionBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private int id;
        private string accountNumber;
        private ExternalTransactionDTO externalTransactionDTO;

        /// <summary>
        /// Constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private ExternalTransactionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public ExternalTransactionBL(ExecutionContext executionContext, int id)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id);
            this.id = id;
            log.LogMethodExit();
        }

        public ExternalTransactionBL(ExecutionContext executionContext, string accountNumber)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountNumber);
            this.accountNumber = accountNumber;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ExternalTransactionBL object using the ExternalTransactionDTO 
        /// </summary>
        /// <param name="executionContex"></param>
        /// <param name="externalTransactionDTO"></param>
        public ExternalTransactionBL(ExecutionContext executionContext, ExternalTransactionDTO externalTransactionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, externalTransactionDTO);
            this.externalTransactionDTO = externalTransactionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetTransactionDetails
        /// </summary>
        /// <param name="transactionId">transactionId</param>
        /// <param name="utilities">utilities</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public ExternalTransactionDTO GetTransactionDetails(int transactionId, Utilities utilities, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionId, utilities, sqlTransaction);
            ExternalTransactionDTO externalTransactionsDTO = new ExternalTransactionDTO();
            List<TransactionDTO> transactionDTOList = GetAllTransactionDTOList(transactionId, utilities, sqlTransaction);
            if (transactionDTOList != null && transactionDTOList.Count > 0)
            {
                string status = transactionDTOList[0].Status == "CLOSED" ? TransactionTypes.Complete.ToString() : TransactionTypes.Create.ToString();
                externalTransactionsDTO = new ExternalTransactionDTO(status, transactionDTOList[0].TransactionDate,
                    transactionDTOList[0].PrimaryCard, transactionDTOList[0].TransactionNetAmount, transactionDTOList[0].ExternalSystemReference, transactionDTOList[0].TransactionId,
                    transactionDTOList[0].TransactionOTP);
                Operators operators = new Operators();
                if (externalTransactionDTO != null && externalTransactionDTO.Operators != null)
                {
                    operators = new Operators(externalTransactionDTO.Operators.LoginId, externalTransactionDTO.Operators.SiteId, externalTransactionDTO.Operators.PosMachine);
                    externalTransactionsDTO.Operators = operators;
                }
                else
                {
                    operators = new Operators(executionContext.GetUserId(), executionContext.GetSiteId(), transactionDTOList[0].PosMachine);
                    externalTransactionsDTO.Operators = operators;
                }
                List<Adjustments> adjustmentsDTOList = new List<Adjustments>();
                Adjustments adjustment = new Adjustments();
                List<Points> pointsDTOList = new List<Points>();
                Points points = new Points();
                if (externalTransactionDTO != null && externalTransactionDTO.Adjustments != null && externalTransactionDTO.Adjustments.Any() && transactionDTOList[0].TransactionLinesDTOList != null)
                {
                    foreach (Adjustments adjustments in externalTransactionDTO.Adjustments)
                    {
                        if (adjustments.Point != null && adjustments.Point.Any())
                        {
                            foreach (Points point in adjustments.Point)
                            {
                                points = new Points(point.Type, point.Value);
                                pointsDTOList.Add(points);
                            }
                        }
                        adjustment = new Adjustments(adjustments.Type, adjustments.ProductId, adjustments.ProductName, adjustments.ProductReference, adjustments.CardNumber,
                            adjustments.Quantity, adjustments.Amount, pointsDTOList);
                        adjustmentsDTOList.Add(adjustment);
                    }
                }
                else if(transactionDTOList[0].TransactionLinesDTOList != null && transactionDTOList[0].TransactionLinesDTOList.Any())
                {
                    foreach (TransactionLineDTO transactionLineDTO in transactionDTOList[0].TransactionLinesDTOList)
                    {
                        ProductsDTO productsDTO = GetProducts(transactionLineDTO.ProductId, "", "");
                        adjustment = new Adjustments("AddProduct", productsDTO.ProductId, productsDTO.ProductName, productsDTO.ExternalSystemReference, transactionLineDTO.CardNumber,
                            Convert.ToInt32(transactionLineDTO.Quantity), transactionLineDTO.Amount, pointsDTOList);
                        adjustmentsDTOList.Add(adjustment);
                    }
                }
                externalTransactionsDTO.Adjustments = adjustmentsDTOList;
                List<TaxRates> taxRatesDTOList = new List<TaxRates>();
                TaxRates taxRatesDTO = new TaxRates();
                if (externalTransactionDTO != null && externalTransactionDTO.TaxRates != null && externalTransactionDTO.TaxRates.Any())
                {
                    foreach (TaxRates taxRates in externalTransactionDTO.TaxRates)
                    {
                        taxRatesDTO = new TaxRates(taxRates.TaxName, taxRates.TotalTaxAmount);
                        taxRatesDTOList.Add(taxRatesDTO);
                    }
                }
                externalTransactionsDTO.TaxRates = taxRatesDTOList;
                List<Discounts> discountsDTOList = new List<Discounts>();
                Discounts discountsDTO = new Discounts();
                if (externalTransactionDTO != null && externalTransactionDTO.Discounts != null && externalTransactionDTO.Discounts.Any())
                {
                    foreach (Discounts discounts in externalTransactionDTO.Discounts)
                    {
                        discountsDTO = new Discounts(discounts.DiscountName, discounts.Amount, discounts.CouponNumber, discounts.Remarks);
                        discountsDTOList.Add(discountsDTO);
                    }
                }
                externalTransactionsDTO.Discounts = discountsDTOList;
                List<Payments> paymentsDTOList = new List<Payments>();
                Payments paymentsDTO = new Payments();
                if (externalTransactionDTO != null && externalTransactionDTO.Payments != null && externalTransactionDTO.Payments.Any() && transactionDTOList[0].TransactionLinesDTOList != null
                    && transactionDTOList[0].TrxPaymentsDTOList != null)
                {
                    foreach (Payments payments in externalTransactionDTO.Payments)
                    {
                        paymentsDTO = new Payments(payments.Type, payments.Amount, transactionDTOList[0].TrxPaymentsDTOList.Count > 0 ? transactionDTOList[0].TrxPaymentsDTOList[0].PaymentId : payments.PaymentId,
                            transactionDTOList[0].TrxPaymentsDTOList.Count > 0 ? transactionDTOList[0].TrxPaymentsDTOList[0].PaymentModeId : payments.PaymentModeId,
                            transactionDTOList[0].TrxPaymentsDTOList.Count > 0 ? transactionDTOList[0].TrxPaymentsDTOList[0].PaymentDate.ToString() : payments.PaymentDate, payments.Remarks);
                        CreditCard creditCard = new CreditCard(payments.CreditCard.CardNumber, payments.CreditCard.MaskedCardNumber, payments.CreditCard.ExpiryDate,
                            payments.CreditCard.CardName);
                        paymentsDTO.CreditCard = creditCard;
                        paymentsDTOList.Add(paymentsDTO);
                    }
                }
                else if (transactionDTOList[0].TrxPaymentsDTOList != null && transactionDTOList[0].TrxPaymentsDTOList.Any())
                {
                    foreach (TransactionPaymentsDTO transactionPaymentsDTO in transactionDTOList[0].TrxPaymentsDTOList)
                    {
                        string paymentType = "Cash";
                        if (transactionDTOList[0].TrxPaymentsDTOList.Count > 0)
                        {
                            PaymentMode paymentMode = new PaymentMode(executionContext, transactionPaymentsDTO.PaymentModeId);
                            paymentType = paymentMode.GetPaymentModeDTO.IsCash ? "Cash" : "CreditDebit";

                        }
                        paymentsDTO = new Payments(paymentType, Convert.ToDecimal(transactionPaymentsDTO.Amount), transactionDTOList[0].TrxPaymentsDTOList.Count > 0 ? transactionPaymentsDTO.PaymentId : -1,
                            transactionDTOList[0].TrxPaymentsDTOList.Count > 0 ? transactionPaymentsDTO.PaymentModeId : -1,
                            transactionDTOList[0].TrxPaymentsDTOList.Count > 0 ? transactionPaymentsDTO.PaymentDate.ToString() : string.Empty, string.Empty);
                        CreditCard creditCard = new CreditCard(transactionDTOList[0].TrxPaymentsDTOList.Count > 0 ? transactionPaymentsDTO.CreditCardNumber : string.Empty,
                            transactionDTOList[0].TrxPaymentsDTOList.Count > 0 ? transactionPaymentsDTO.CreditCardNumber : string.Empty, transactionDTOList[0].TrxPaymentsDTOList.Count > 0 ? transactionPaymentsDTO.CreditCardExpiry : string.Empty,
                            transactionDTOList[0].TrxPaymentsDTOList.Count > 0 ? transactionPaymentsDTO.CreditCardName : string.Empty);
                        paymentsDTO.CreditCard = creditCard;
                        paymentsDTOList.Add(paymentsDTO);
                    }
                }
                externalTransactionsDTO.Payments = paymentsDTOList;
            }
            log.LogVariableState("ExternalTransactionDTO", externalTransactionsDTO);
            log.LogMethodExit(externalTransactionsDTO);
            return externalTransactionsDTO;
        }

        /// <summary>
        /// Save Transaction
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public ExternalTransactionDTO Save(SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(sqltransaction);
            Utilities utilities = GetUtility();
            ExternalTransactionDTO externalTransactionsDTO = new ExternalTransactionDTO();
            //Validate();
            int transactionId = -1;

            using (ParafaitDBTransaction trx = new ParafaitDBTransaction())
            {
                trx.BeginTransaction();
                int refundProductId = -1;
                if (externalTransactionDTO != null)
                {
                    Transaction.Transaction transaction = new Parafait.Transaction.Transaction(utilities);
                    transaction.Remarks = "ExternalPOS Transactions" + ServerDateTime.Now;
                    TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
                    TagNumber tagNumber;
                    if (!string.IsNullOrEmpty(externalTransactionDTO.LoyaltyCardNumber))
                    {
                        if (tagNumberParser.TryParse(externalTransactionDTO.LoyaltyCardNumber, out tagNumber) == false)
                        {
                            string message = tagNumberParser.Validate(externalTransactionDTO.LoyaltyCardNumber);
                            log.LogMethodExit(null, "Throwing Exception- " + message);
                            throw new Exception(message);
                        }
                    }
                    refundProductId = GetRefundProductId();
                    if (externalTransactionDTO.Adjustments != null && externalTransactionDTO.Adjustments.Any())
                    {
                        var orderByAdjustmentTypeList = externalTransactionDTO.Adjustments.OrderBy(x => x.Type).ToList(); // Make sure transaction is saved at the end
                        foreach (Adjustments adjustment in orderByAdjustmentTypeList)
                        {
                            Transaction.Card cards = new Transaction.Card(utilities);
                            if (!string.IsNullOrEmpty(adjustment.CardNumber))
                            {
                                AccountBL accountBL = new AccountBL(executionContext, adjustment.CardNumber, false, false, trx.SQLTrx);
                                if (accountBL.AccountDTO == null && adjustment.Type == AdjustmentTypes.AddValue.ToString())
                                {
                                    log.Debug("Account does not exists .Creating new Account");
                                    DateTime currentTime = ServerDateTime.Now;
                                    AccountDTO accountDTO = new AccountDTO(-1, adjustment.CardNumber, "", currentTime, null, false, null, null, true,
                                                                null, null, null, null, null, null, -1, null, true, true, false, null, null, "N", null,
                                                                false, null, -1, null, null, -1, null, null, false, -1, "", "",
                                                                currentTime, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(),
                                                                currentTime);
                                    accountBL = new AccountBL(executionContext, accountDTO);
                                    accountBL.Save(trx.SQLTrx);
                                    cards = new Parafait.Transaction.Card(accountBL.AccountDTO.AccountId, executionContext.GetUserId(), utilities, trx.SQLTrx);
                                }
                                else
                                {
                                    cards = new Parafait.Transaction.Card(adjustment.CardNumber, executionContext.GetUserId(), utilities, trx.SQLTrx);
                                }
                            }
                            CreateTransactionLine(cards, adjustment, transaction, utilities, trx.SQLTrx, refundProductId);
                        }
                    }
                    string transactionMessage = string.Empty;
                    if (externalTransactionDTO.Payments != null && externalTransactionDTO.Payments.Any())
                    {
                        foreach (Payments payments in externalTransactionDTO.Payments)
                        {
                            if (payments.Type == AdjustmentTypes.Cash.ToString())
                            {
                                if (payments.PaymentModeId < 0)
                                {
                                    PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                                    if (paymentModeDTOList != null)
                                    {
                                        TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, Convert.ToDouble(payments.Amount),
                                                                                                        "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                                        executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                                        transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                                        transaction.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                                    }
                                }
                                else
                                {
                                    PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, payments.PaymentModeId.ToString()));
                                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                                    if (paymentModeDTOList != null && paymentModeDTOList.Any())
                                    {
                                        if (paymentModeDTOList.FirstOrDefault().IsCash == false)
                                        {
                                            string errorMessage = "Please enter valid Payment Type. ";
                                            log.Error("Throwing Exception- " + errorMessage);
                                            throw new ValidationException(errorMessage);
                                        }
                                        TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, payments.PaymentModeId, Convert.ToDouble(payments.Amount),
                                                                                                        "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                                        executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                                        transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                                        transaction.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                                    }
                                    else
                                    {
                                        string errorMessage = "Invalid Payment Mode. Please setup specific payment mode.";
                                        log.Error("Throwing Exception- " + errorMessage);
                                        throw new ValidationException(errorMessage);
                                    }
                                }
                            }
                            else if (payments.Type == AdjustmentTypes.CreditDebit.ToString())
                            {
                                if (payments.CreditCard == null)
                                {
                                    string errorMessage = "Please enter card details. ";
                                    log.Error("Throwing Exception- " + errorMessage);
                                    throw new ValidationException(errorMessage);
                                }
                                if (payments.PaymentModeId < 0)
                                {
                                    PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
                                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                                    if (paymentModeDTOList != null)
                                    {
                                        if (string.IsNullOrEmpty(payments.CreditCard.MaskedCardNumber) || string.IsNullOrEmpty(payments.CreditCard.ExpiryDate))
                                        {
                                            string errorMessage = "Please enter MaskedCardNumber and  ExpiryDate. ";
                                            log.Error("Throwing Exception- " + errorMessage);
                                            throw new ValidationException(errorMessage);
                                        }
                                        TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, Convert.ToDouble(payments.Amount),
                                                                                          payments.CreditCard.MaskedCardNumber, payments.CreditCard.CardName, "", payments.CreditCard.ExpiryDate,
                                                                                          string.Empty, -1, string.Empty, -1, 0, -1, string.Empty, string.Empty, false, -1, -1, string.Empty, ServerDateTime.Now,
                                                                                          executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, string.Empty, null);
                                        transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                                        transaction.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                                    }
                                }
                                else
                                {
                                    PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, payments.PaymentModeId.ToString()));
                                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                                    if (paymentModeDTOList != null && paymentModeDTOList.Any())
                                    {
                                        int cardId = -1;
                                        TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO();
                                        if (paymentModeDTOList.FirstOrDefault().IsDebitCard)
                                        {
                                            if (string.IsNullOrEmpty(payments.CreditCard.CardNumber))
                                            {
                                                string errorMessage = "Please enter the debit card number.";
                                                log.Error("Throwing Exception- " + errorMessage);
                                                throw new ValidationException(errorMessage);
                                            }
                                            AccountBL accountBL = new AccountBL(executionContext, payments.CreditCard.CardNumber, true, true);
                                            if (accountBL.AccountDTO == null)
                                            {
                                                string errorMessage = "Debit Card not found.";
                                                log.Error("Throwing Exception- " + errorMessage);
                                                throw new ValidationException(errorMessage);
                                            }
                                            else
                                            {
                                                cardId = accountBL.GetAccountId();
                                            }
                                            bool voucherApplied = transaction.ApplyVoucher(new Card(payments.CreditCard.CardNumber, "", utilities), ref transactionMessage);
                                            if (voucherApplied)
                                            {
                                                log.Info("voucherApplied for transaction:" + payments.CreditCard.CardNumber);
                                                transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, Convert.ToDouble(payments.Amount),
                                                                                                     payments.CreditCard.MaskedCardNumber, payments.CreditCard.CardName, "", payments.CreditCard.ExpiryDate,
                                                                                                     string.Empty, cardId, string.Empty, -1, 0, -1, string.Empty, string.Empty, false, -1, -1, string.Empty, ServerDateTime.Now,
                                                                                                     executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, string.Empty, null);
                                            }
                                            else
                                            {
                                                double credits = Convert.ToDouble(accountBL.AccountDTO.Credits);
                                                double transactionAmount = Convert.ToDouble(payments.Amount);
                                                CreditPlus creditPlus = new CreditPlus(utilities);
                                                double creditPlusAvailable = creditPlus.getCreditPlusForPOS(accountBL.AccountDTO.AccountId, utilities.ParafaitEnv.POSTypeId, transaction);
                                                double creditAmountAvailable = credits;
                                                double amount = 0;
                                                double paymentUsedCreditplus = 0;
                                                if (transactionAmount > creditPlusAvailable)
                                                {
                                                    paymentUsedCreditplus = creditPlusAvailable;
                                                    transactionAmount -= creditPlusAvailable;

                                                    if (transactionAmount > creditAmountAvailable)
                                                    {
                                                        amount = credits;
                                                        transactionAmount -= creditAmountAvailable;
                                                    }
                                                    else
                                                    {
                                                        double usedCredits = transactionAmount;
                                                        amount = usedCredits;
                                                        transactionAmount = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    if (transactionAmount >= 0)
                                                    {
                                                        paymentUsedCreditplus = transactionAmount;
                                                    }
                                                    else
                                                    {
                                                        amount = transactionAmount;
                                                    }
                                                    transactionAmount = 0;
                                                }
                                                if (transactionAmount != 0)
                                                {
                                                    string errorMessage = MessageContainerList.GetMessage(executionContext, 183);
                                                    log.Error("Throwing Exception- Insufficient Credits on Game Card(s)");
                                                    throw new ValidationException(errorMessage);
                                                }
                                                transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, amount,
                                                                                                     payments.CreditCard.MaskedCardNumber, payments.CreditCard.CardName, "", payments.CreditCard.ExpiryDate,
                                                                                                     string.Empty, cardId, string.Empty, -1, paymentUsedCreditplus, -1, string.Empty, string.Empty, false, -1, -1, string.Empty, ServerDateTime.Now,
                                                                                                     executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, string.Empty, null);
                                            }
                                        }
                                        else if (paymentModeDTOList.FirstOrDefault().IsCreditCard)
                                        {
                                            if (string.IsNullOrEmpty(payments.CreditCard.MaskedCardNumber) || string.IsNullOrEmpty(payments.CreditCard.ExpiryDate))
                                            {
                                                string errorMessage = "Please enter MaskedCardNumber and  ExpiryDate. ";
                                                log.Error("Throwing Exception- " + errorMessage);
                                                throw new ValidationException(errorMessage);
                                            }
                                            transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, Convert.ToDouble(payments.Amount),
                                                                                                      payments.CreditCard.MaskedCardNumber, payments.CreditCard.CardName, "", payments.CreditCard.ExpiryDate,
                                                                                                      string.Empty, -1, string.Empty, -1, 0, -1, string.Empty, string.Empty, false, -1, -1, string.Empty, ServerDateTime.Now,
                                                                                                      executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, string.Empty, null);
                                        }
                                        transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                                        transaction.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                                    }
                                    else
                                    {
                                        string errorMessage = "Invalid Payment Mode. Please setup specific payment mode.";
                                        log.Error("Throwing Exception- " + errorMessage);
                                        throw new ValidationException(errorMessage);
                                    }
                                }
                            }
                            else
                            {
                                log.Error("Please enter valid Payment Type ");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid Payment Type"));
                            }
                        }
                    }


                    //foreach (Discounts discounts in externalTransactionDTO.Discounts)
                    //{
                    //    DiscountsListBL discountsListBL = new DiscountsListBL(executionContext);
                    //    List<KeyValuePair<DiscountsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountsDTO.SearchByParameters, string>>();
                    //    searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    //    searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_NAME, discounts.DiscountName));
                    //    searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                    //    List<DiscountsDTO> discountsDTOList = discountsListBL.GetDiscountsDTOList(searchParameters, false, false, false, sqltransaction);
                    //    if (discountsDTOList != null)
                    //    {
                    //        if (discountsDTOList.Count > 1)
                    //        {
                    //            log.Debug("Duplicate discount exists. Please enter valid discount");
                    //            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Duplicate discount exists. Please enter valid discount"));
                    //        }
                    //        else
                    //        {
                    //            TransactionDiscountsDTO transactionDiscountsDTO = new TransactionDiscountsDTO();
                    //            if (discountsDTOList[0].VariableDiscounts == "Y")
                    //            {
                    //                transactionDiscountsDTO = new TransactionDiscountsDTO(-1, -1, -1, discountsDTOList[0].DiscountId, Convert.ToDecimal(discountsDTOList[0].DiscountPercentage),
                    //                discounts.Amount, discounts.Remarks, -1, DiscountApplicability.LINE);
                    //            }
                    //            else
                    //            {
                    //                transactionDiscountsDTO = new TransactionDiscountsDTO(-1, -1, -1, discountsDTOList[0].DiscountId, Convert.ToDecimal(discountsDTOList[0].DiscountPercentage),
                    //                Convert.ToDecimal(discountsDTOList[0].DiscountAmount), discounts.Remarks, -1, DiscountApplicability.LINE);

                    //            }
                    //            foreach (Transaction.Transaction.TransactionLine transactionLine in transaction.TransactionLineList)
                    //            {
                    //                transactionLine.TransactionLineDTO.TransactionDiscountsDTOList.Add(transactionDiscountsDTO);
                    //            }
                    //        }

                    //    }
                    //}
                    if (!string.IsNullOrEmpty(externalTransactionDTO.ExternalReference))
                    {
                        transaction.externalSystemReference = externalTransactionDTO.ExternalReference;
                    }

                    int trxId = -1;
                    if (externalTransactionDTO.Type == TransactionTypes.Complete.ToString())
                    {
                        trxId = transaction.SaveTransacation(trx.SQLTrx, ref transactionMessage);
                    }
                    else if (externalTransactionDTO.Type == TransactionTypes.Create.ToString())
                    {
                        trxId = transaction.SaveOrder(ref transactionMessage, trx.SQLTrx);
                    }
                    else
                    {
                        log.Debug("Please enter valid Transaction Type");
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid Transaction Type. "));
                    }
                    if (trxId != 0)
                    {
                        log.Debug("Failed to create transaction " + transactionMessage);
                        throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction. " + transactionMessage));
                    }
                    transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "External POS save transaction status : " + transaction.Trx_id, trx.SQLTrx);
                    log.Debug("SaveTransaction" + transactionMessage);
                    if (trxId <= 0)
                    {
                        LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "ENTITLEMENT_TO_PRODUCT_MAP", null);
                        if (lookupsContainerDTO != null && lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValue == "GetLocalTransactionOTP").FirstOrDefault().Description == "Y")
                        {
                            Parafait.Transaction.TransactionBL transactionBL = new Transaction.TransactionBL(executionContext, transaction.Trx_id, trx.SQLTrx);
                            if (transactionBL.TransactionDTO.TransactionId > -1 && string.IsNullOrEmpty(transactionBL.TransactionDTO.TransactionOTP))
                            {
                                string transactionOTP = string.Empty;
                                bool alreadyExists = false;
                                do
                                {
                                    transactionOTP = utilities.GenerateRandomNumber(8, Utilities.RandomNumberType.Numeric);
                                    alreadyExists = transaction.AlreadyUsedOTP(transactionOTP, trx.SQLTrx);
                                }
                                while (alreadyExists == true);
                                transactionBL.TransactionDTO.TransactionOTP = transactionOTP;
                            }
                            transactionBL.Save(trx.SQLTrx);
                        }
                        transactionId = transaction.TransactionDTO.TransactionId;
                    }
                }
                trx.EndTransaction();
                externalTransactionsDTO = GetTransactionDetails(transactionId, utilities, sqltransaction);
                log.LogMethodExit(externalTransactionsDTO);
                return externalTransactionsDTO;
            }
        }

        /// <summary>
        /// CreateTransactionLine
        /// </summary>
        /// <param name="cards">cards</param>
        /// <param name="adjustment">adjustment</param>
        /// <param name="transaction">transaction</param>
        /// <param name="utilities">utilities</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        internal void CreateTransactionLine(Transaction.Card cards, Adjustments adjustment,
                                          Parafait.Transaction.Transaction transaction,
                                          Utilities utilities, SqlTransaction sqlTransaction, int refundProductId = -1)
        {
            try
            {
                log.LogMethodEntry(cards, adjustment, transaction, utilities, sqlTransaction);
                int trxLineReturnValue = -1;
                int productId = -1;
                int transactionId = -1;
                CreditPlus creditPlus = new CreditPlus(utilities);
                string transactionLineMessage = string.Empty;
                if (string.IsNullOrEmpty(adjustment.Type))
                {
                    log.Error("Please enter valid Adjustment Type");
                    throw new ValidationException("Please enter valid Adjustment Type");
                }
                #region AddProduct
                if (adjustment.Type == AdjustmentTypes.AddProduct.ToString())
                {
                    if (adjustment.Quantity < 0)
                    {
                        log.Error("Please Enter product quantity");
                        throw new ValidationException("Please Enter product quantity");
                    }
                    ProductsDTO productsDTO = GetProducts(adjustment.ProductId, adjustment.ProductName, adjustment.ProductReference);
                    List<ProductsDTO> productsDTOList = GetProductsByProductType();
                    int count = productsDTOList.Where(x => x.ProductId == productsDTO.ProductId).Count();
                    string cardNumber = string.Empty;
                    if ((productsDTO.ProductType == "NEW" || productsDTO.ProductType == "CARDSALE" || productsDTO.ProductType == "GAMETIME" || productsDTO.ProductType == "CHECK-IN" || productsDTO.ProductType == "ATTRACTION")
                        && cards.CardNumber == null && executionContext.GetIsCorporate())
                    {
                        TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(utilities.ExecutionContext);
                        RandomTagNumber randomTagNumber = new RandomTagNumber(utilities.ExecutionContext, tagNumberLengthList);
                        cardNumber = productsDTO.AutoGenerateCardNumber == "Y" ? randomTagNumber.Value : "T" + randomTagNumber.Value.Substring(1);
                        cards = new Card(cardNumber, utilities.ParafaitEnv.LoginID, utilities);
                    }
                    if (count > 0 && string.IsNullOrEmpty(cards.CardNumber) && executionContext.GetIsCorporate() == false)
                    {
                        log.Error("Please enter the card number for card products");
                        throw new ValidationException("Please enter the card number for card products");
                    }
                    trxLineReturnValue = transaction.createTransactionLine(cards, productsDTO.ProductId, (adjustment.Amount == null || adjustment.Amount < 0) ?
                        Convert.ToDouble(-1) : Convert.ToDouble(adjustment.Amount), adjustment.Quantity, ref transactionLineMessage);
                    log.Debug(transactionLineMessage);
                    if (trxLineReturnValue != 0)
                    {
                        log.Debug("Failed to create transaction line:AdjustmentTypes.AddProduct");
                        throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line. " + transactionLineMessage));
                    }
                }
                #endregion
                #region AddValue
                else if (adjustment.Type == AdjustmentTypes.AddValue.ToString())
                {
                    if (string.IsNullOrEmpty(cards.CardNumber))
                    {
                        log.Error("Please enter the card number");
                        throw new ValidationException("Please enter the card number");
                    }
                    if (adjustment.Point != null && adjustment.Point.Any())
                    {
                        foreach (Points point in adjustment.Point)
                        {
                            if (point.Type == "T")
                            {
                                productId = GetVariableProductId("ExternalPOSTickets");
                                trxLineReturnValue = transaction.createTransactionLine(cards, productId, Convert.ToDouble(point.Value), 1, ref transactionLineMessage);
                                log.Debug(transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to create transaction line:AdjustmentTypes.AddValue. " + transactionLineMessage);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line. " + transactionLineMessage));
                                }
                            }
                            else if (point.Type == "L")
                            {
                                productId = GetVariableProductId("ExternalPOSLoyaltyPoint");
                                trxLineReturnValue = transaction.createTransactionLine(cards, productId, Convert.ToDouble(point.Value), 1, ref transactionLineMessage);
                                log.Debug(transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to create transaction line:AdjustmentTypes.AddValue. " + transactionLineMessage);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line. " + transactionLineMessage));
                                }
                            }
                            else if (point.Type == "P")
                            {
                                productId = GetVariableProductId("ExternalPOSItem");
                                trxLineReturnValue = transaction.createTransactionLine(cards, productId, Convert.ToDouble(point.Value), 1, ref transactionLineMessage);
                                log.Debug(transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to create transaction line:AdjustmentTypes.AddValue. " + transactionLineMessage);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line. " + transactionLineMessage));
                                }
                            }
                            else if (point.Type == "A")
                            {
                                productId = GetVariableProductId("ExternalPOSCardBalance");
                                trxLineReturnValue = transaction.createTransactionLine(cards, productId, Convert.ToDouble(point.Value), 1, ref transactionLineMessage);
                                log.Debug(transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to create transaction line:AdjustmentTypes.AddValue. " + transactionLineMessage);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line. " + transactionLineMessage));
                                }
                            }
                            else if (point.Type == "B")
                            {
                                productId = GetVariableProductId("ExternalPOSBonus");
                                trxLineReturnValue = transaction.createTransactionLine(cards, productId, Convert.ToDouble(point.Value), 1, ref transactionLineMessage);
                                log.Debug(transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to create transaction line:AdjustmentTypes.AddValue. " + transactionLineMessage);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line. " + transactionLineMessage));
                                }
                            }
                            else if (point.Type == "G")
                            {
                                productId = GetVariableProductId("ExternalPOSGamePlay");
                                trxLineReturnValue = transaction.createTransactionLine(cards, productId, Convert.ToDouble(point.Value), 1, ref transactionLineMessage);
                                log.Debug(transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to create transaction line:AdjustmentTypes.AddValue. " + transactionLineMessage);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line. " + transactionLineMessage));
                                }
                            }
                            else
                            {
                                log.Error("Please enter valid point type");
                                throw new ValidationException("Please enter valid point type");
                            }
                        }
                    }
                    else
                    {
                        log.Error("Please enter the point details");
                        throw new ValidationException("Please enter the point details");
                    }
                }
                #endregion
                else if (adjustment.Type == AdjustmentTypes.RemoveValue.ToString())
                {
                    if (string.IsNullOrEmpty(cards.CardNumber))
                    {
                        log.Error("Please enter the card number");
                        throw new ValidationException("Please enter the card number");
                    }
                    if (refundProductId < 0)
                    {
                        log.LogMethodExit("Refund product not found");
                        throw new ValidationException("Refund product not found");
                    }
                    if (adjustment.Point != null && adjustment.Point.Any())
                    {
                        AccountBL accountBL = new AccountBL(executionContext, cards.CardNumber, true, true, sqlTransaction);
                        if (transaction.Trx_id <= 0)
                        {
                            int saveOrderValue = transaction.SaveOrder(ref transactionLineMessage, sqlTransaction);
                            if (saveOrderValue != 0)
                            {
                                log.Debug("Failed to create Order");
                                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create Order"));
                            }
                            log.Debug(transactionLineMessage + ":" + transaction.Trx_id);
                            transactionId = transaction.Trx_id;
                        }
                        else
                        {
                            transactionId = transaction.Trx_id;
                        }
                        TaskProcs tp = new TaskProcs(utilities);
                        foreach (Points point in adjustment.Point)
                        {
                            if (point.Value > 0 && point.Type == "B")
                            {
                                //productId = GetVariableProductId("ExternalPOSBonus");
                                if (Convert.ToDecimal(accountBL.AccountDTO.TotalBonusBalance) - point.Value < 0)
                                {
                                    log.Error("Insufficient Game Play Bonus: Required: " + point.Value + " Available: " + accountBL.AccountDTO.TotalBonusBalance);
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, point, accountBL.AccountDTO.TotalBonusBalance));
                                }
                                double bonusToDeduct = Convert.ToDouble(point.Value);
                                if (cards.CreditPlusBonus > 0 && bonusToDeduct > 0)
                                {
                                    double balance = creditPlus.DeductGenericCreditPlus(cards, "B", bonusToDeduct, sqlTransaction);
                                    if (balance > 0)
                                    {
                                        bonusToDeduct = bonusToDeduct - balance;
                                    }
                                    transactionLineMessage = "Bonus"; // pass negative value to this method
                                    trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, bonusToDeduct, 1, ref transactionLineMessage);
                                    if (trxLineReturnValue != 0)
                                    {
                                        log.Debug("Failed to  create TransactionLine. " + transactionLineMessage);
                                        throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine. " + transactionLineMessage));
                                    }
                                    var transactionLine = transaction.TransactionLineList.LastOrDefault();
                                    {
                                        if (transactionLine.ProductID == refundProductId)
                                        {
                                            transactionLine.LineAmount = Convert.ToDouble(bonusToDeduct) * -1;
                                            transactionLine.Price = Convert.ToDouble(bonusToDeduct) * -1;
                                            transaction.updateAmounts(false);
                                        }
                                    }
                                    //tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "External POS deduct bonus", sqlTransaction, -1, -1, bonusToDeduct, -1, transactionId);
                                }
                            }
                            else if (point.Value > 0 && point.Type == "A")
                            {
                                if (Convert.ToDecimal(accountBL.AccountDTO.TotalCreditsBalance) - point.Value < 0)
                                {
                                    log.Error("Insufficient Credits: Required: " + point.Value + " Available: " + accountBL.AccountDTO.TotalCreditsBalance);
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, point.Value, accountBL.AccountDTO.TotalCreditsBalance));
                                }
                                //productId = GetVariableProductId("ExternalPOSCardBalance");
                                double creditsToDeduct = Convert.ToDouble(point.Value);
                                double balance = creditPlus.DeductGenericCreditPlus(cards, "A", creditsToDeduct, sqlTransaction);
                                if (balance > 0)
                                {
                                    creditsToDeduct = creditsToDeduct - balance;
                                }
                                transactionLineMessage = "CardBalance";
                                trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, creditsToDeduct, 1, ref transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to  create TransactionLine." + transactionLineMessage);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine. " + transactionLineMessage));
                                }
                                var transactionLine = transaction.TransactionLineList.LastOrDefault();
                                {
                                    if (transactionLine.ProductID == refundProductId)
                                    {
                                        transactionLine.Price = Convert.ToDouble(creditsToDeduct) * -1;
                                        transactionLine.LineAmount = Convert.ToDouble(creditsToDeduct) * -1;
                                        transaction.updateAmounts(false);
                                    }
                                }
                                //tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "qubica deduct credits", sqlTransaction, creditsToDeduct, -1, -1, -1, -1);
                            }
                            else if (point.Value > 0 && point.Type == "T")
                            {
                                if (Convert.ToDecimal(accountBL.AccountDTO.TotalTicketsBalance) - point.Value < 0)
                                {
                                    log.Error("Insufficient Tickets: Required: " + point.Value + " Available: " + accountBL.AccountDTO.TotalTicketsBalance);
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, point.Value, accountBL.AccountDTO.TotalTicketsBalance));
                                }
                                //productId = GetVariableProductId("ExternalPOSTickets");
                                double ticketsToDeduct = Convert.ToDouble(point.Value);
                                creditPlus.deductCreditPlusTicketsLoyaltyPoints(cards.CardNumber, ticketsToDeduct, 0, sqlTransaction);

                                transactionLineMessage = "Ticket";
                                trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, ticketsToDeduct, 1, ref transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to  create TransactionLine");
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine"));
                                }
                                var transactionLine = transaction.TransactionLineList.LastOrDefault();
                                {
                                    if (transactionLine.ProductID == refundProductId)
                                    {
                                        transactionLine.LineAmount = Convert.ToDouble(ticketsToDeduct) * -1;
                                        transactionLine.Price = Convert.ToDouble(ticketsToDeduct) * -1;
                                        transaction.updateAmounts(false);
                                    }
                                }
                                //tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "External Pos deduct tickets", sqlTransaction, -1, -1, -1, Convert.ToInt32(ticketsToDeduct), -1);
                            }
                            else if (point.Value > 0 && point.Type == "L")
                            {
                                if (Convert.ToDecimal(accountBL.AccountDTO.AccountSummaryDTO.CreditPlusLoyaltyPoints) - point.Value < 0)
                                {
                                    log.Error("Insufficient Loyalty Points: Required: " + point.Value + " Available: " + accountBL.AccountDTO.AccountSummaryDTO.CreditPlusLoyaltyPoints);
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, point.Value, accountBL.AccountDTO.AccountSummaryDTO.CreditPlusLoyaltyPoints));
                                }
                                //productId = GetVariableProductId("ExternalPOSLoyaltyPoint");
                                double loyaltyPointsToDeduct = Convert.ToDouble(point.Value);
                                double balance = creditPlus.DeductGenericCreditPlus(cards, "L", loyaltyPointsToDeduct, sqlTransaction);
                                if (balance > 0)
                                {
                                    loyaltyPointsToDeduct = loyaltyPointsToDeduct - balance;
                                }
                                transactionLineMessage = "LoyaltyPoints";
                                trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, loyaltyPointsToDeduct, 1, ref transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to  create TransactionLine. " + transactionLineMessage);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine. " + transactionLineMessage));
                                }
                                var transactionLine = transaction.TransactionLineList.LastOrDefault();
                                {
                                    if (transactionLine.ProductID == refundProductId)
                                    {
                                        transactionLine.LineAmount = Convert.ToDouble(loyaltyPointsToDeduct) * -1;
                                        transactionLine.Price = Convert.ToDouble(loyaltyPointsToDeduct) * -1;
                                        transaction.updateAmounts(false);
                                    }
                                }
                                //tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "External POS deduct loyalty points", sqlTransaction, -1, -1, -1, Convert.ToInt32(loyaltyPointsToDeduct), -1);
                            }
                            else if (point.Value > 0 && point.Type == "P")
                            {
                                if (Convert.ToDecimal(accountBL.AccountDTO.AccountSummaryDTO.CreditPlusItemPurchase) - point.Value < 0)
                                {
                                    log.Error("Insufficient Items: Required: " + point.Value + " Available: " + accountBL.AccountDTO.AccountSummaryDTO.CreditPlusItemPurchase);
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, point.Value, accountBL.AccountDTO.AccountSummaryDTO.CreditPlusItemPurchase));
                                }
                                //productId = GetVariableProductId("ExternalPOSItem");
                                double itemsToDeduct = Convert.ToDouble(point.Value);
                                double balance = creditPlus.DeductGenericCreditPlus(cards, "P", itemsToDeduct, sqlTransaction);
                                if (balance > 0)
                                {
                                    itemsToDeduct = itemsToDeduct - balance;
                                }
                                transactionLineMessage = "Counter Items";
                                trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, itemsToDeduct, 1, ref transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to  create TransactionLine." + transactionLineMessage);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine. " + transactionLineMessage));
                                }
                                var transactionLine = transaction.TransactionLineList.LastOrDefault();
                                {
                                    if (transactionLine.ProductID == refundProductId)
                                    {
                                        transactionLine.LineAmount = Convert.ToDouble(itemsToDeduct) * -1;
                                        transactionLine.Price = Convert.ToDouble(itemsToDeduct) * -1;
                                        transaction.updateAmounts(false);
                                    }
                                }
                                //tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "External pos deduct counter items", sqlTransaction, -1, -1, -1, Convert.ToInt32(itemsToDeduct), -1);
                            }
                            else if (point.Value > 0 && point.Type == "G")
                            {
                                if (Convert.ToDecimal(accountBL.AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits) - point.Value < 0)
                                {
                                    log.Error("Insufficient Game Play Credits: Required: " + point.Value + " Available: " + accountBL.AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits);
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, point.Value, accountBL.AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits));
                                }
                                //productId = GetVariableProductId("ExternalPOSGamePlay");
                                double gameCreditsToDeduct = Convert.ToDouble(point.Value);
                                double balance = creditPlus.DeductGenericCreditPlus(cards, "G", gameCreditsToDeduct, sqlTransaction);
                                if (balance > 0)
                                {
                                    gameCreditsToDeduct = gameCreditsToDeduct - balance;
                                }
                                transactionLineMessage = "Game Play Credits";
                                trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, gameCreditsToDeduct, 1, ref transactionLineMessage);
                                if (trxLineReturnValue != 0)
                                {
                                    log.Debug("Failed to  create TransactionLine." + transactionLineMessage);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine. " + transactionLineMessage));
                                }
                                var transactionLine = transaction.TransactionLineList.LastOrDefault();
                                {
                                    if (transactionLine.ProductID == refundProductId)
                                    {
                                        transactionLine.LineAmount = Convert.ToDouble(gameCreditsToDeduct) * -1;
                                        transactionLine.Price = Convert.ToDouble(gameCreditsToDeduct) * -1;
                                        transaction.updateAmounts(false);
                                    }
                                }
                                //tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "External pos deduct game play credits", sqlTransaction, -1, -1, -1, Convert.ToInt32(gameCreditsToDeduct), -1);
                            }
                            else
                            {
                                log.Error("Please enter valid point type");
                                throw new ValidationException("Please enter valid point type");
                            }
                        }
                    }
                    else
                    {
                        log.Error(".Please enter the point details");
                        throw new ValidationException("Please enter the point details");
                    }

                }

                else
                {
                    log.Error("Please enter the valid Adjustment Type");
                    throw new ValidationException("Please enter the valid Adjustment Type");
                }
            }

            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Products
        /// </summary>
        /// <param name="productName">productName</param>
        /// <param name="productReference">productReference</param>
        /// <returns>ProductsDTO</returns>
        private ProductsDTO GetProducts(int productId, string productName, string productReference)
        {
            log.LogMethodEntry(productId, productName, productReference);
            ProductsDTO productsDTO = new ProductsDTO();
            ProductsList productsList = new ProductsList(executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            if (productId > -1)
            {
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, productId.ToString()));
            }
            else if (!string.IsNullOrEmpty(productName))
            {
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_EXACT_NAME, productName.ToString()));
            }
            else if (!string.IsNullOrEmpty(productReference))
            {
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE, productReference.ToString()));
            }
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParams);
            if (productsDTOList != null && productsDTOList.Any())
            {
                if (productsDTOList.Count > 1)
                {
                    throw new ValidationException("Duplicate products exist, please use the correct product");
                }
                productsDTO = productsDTOList[0];
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 111));
            }
            log.LogMethodExit(productsDTO);
            return productsDTO;
        }

        /// <summary>
        /// Get Products using Product Type
        /// </summary>
        /// <returns>productsDTOList</returns>
        private List<ProductsDTO> GetProductsByProductType()
        {
            log.LogMethodEntry();
            ProductsDTO productsDTO = new ProductsDTO();
            ProductsList productsList = new ProductsList(executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST,
                             "'" + ProductTypeValues.NEW + "','" + ProductTypeValues.LOCKERRETURN + "','" + ProductTypeValues.GAMETIME + "','" + ProductTypeValues.LOCKER + "','" + ProductTypeValues.RECHARGE + "','" + ProductTypeValues.CARDSALE + "'"));

            List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParams);
            log.LogMethodExit(productsDTOList);
            return productsDTOList;
        }

        internal int GetRefundProductId()
        {
            log.LogMethodEntry();
            int refundProductId = -1;
            ProductsList productsList = new ProductsList(executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, ProductTypeValues.REFUND));
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParams);
            if (productsDTOList != null && productsDTOList.Any())
            {
                refundProductId = productsDTOList[0].ProductId;
            }
            log.LogMethodExit(refundProductId);
            return refundProductId;
        }

        /// <summary>
        /// Get Utility
        /// </summary>
        /// <returns></returns>
        internal Utilities GetUtility()
        {
            log.LogMethodEntry();
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
            utilities.ParafaitEnv.POSMachineId = executionContext.GetMachineId();
            POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(executionContext.GetSiteId(), executionContext.GetMachineId());
            if (pOSMachineContainerDTO != null)
            {
                utilities.ParafaitEnv.SetPOSMachine("", pOSMachineContainerDTO.POSName);
            }
            else
            {
                utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            }

            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            UserContainerDTO user = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", executionContext.GetSiteId());
            utilities.ParafaitEnv.User_Id = user.UserId;
            utilities.ParafaitEnv.RoleId = user.RoleId;
            utilities.ExecutionContext.SetUserId(user.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit();
            return utilities;
        }

        /// <summary>
        /// Get Variable ProductId
        /// </summary>
        /// <param name="adjustmentProductName">adjustmentProductName</param>
        /// <returns>productId</returns>
        internal int GetVariableProductId(string adjustmentProductName)
        {
            log.LogMethodEntry(adjustmentProductName);
            int variableProductId = -1;
            string productName = string.Empty;
            LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "ENTITLEMENT_TO_PRODUCT_MAP", null);
            switch (adjustmentProductName)
            {
                case "ExternalPOSLoyaltyPoint":
                    {

                        productName = lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValue == "L").FirstOrDefault().Description;
                        if (string.IsNullOrEmpty(productName))
                        {
                            productName = "EXTERNALPOSLOYALTYPOINTVARIABLE";
                        }
                    }
                    break;
                case "ExternalPOSCardBalance":
                    {
                        productName = lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValue == "A").FirstOrDefault().Description;
                        if (string.IsNullOrEmpty(productName))
                        {
                            productName = "EXTERNALPOSCARDBALANCEVARIABLE";
                        }
                    }
                    break;
                case "ExternalPOSTickets":
                    {
                        productName = lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValue == "T").FirstOrDefault().Description;
                        if (string.IsNullOrEmpty(productName))
                        {
                            productName = "EXTERNALPOSTICKETSVARIABLE";
                        }
                    }
                    break;
                case "ExternalPOSItem":
                    {
                        productName = lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValue == "P").FirstOrDefault().Description;
                        if (string.IsNullOrEmpty(productName))
                        {
                            productName = "EXTERNALPOSITEMVARIABLE";
                        }
                    }
                    break;
                case "ExternalPOSBonus":
                    {
                        productName = lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValue == "B").FirstOrDefault().Description;
                        if (string.IsNullOrEmpty(productName))
                        {
                            productName = "EXTERNALPOSBONUSVARIABLE";
                        }
                    }
                    break;
                case "ExternalPOSGamePlay":
                    {
                        productName = lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValue == "G").FirstOrDefault().Description;
                        if (string.IsNullOrEmpty(productName))
                        {
                            productName = "EXTERNALPOSGAMEPLAYVARIABLE";
                        }
                    }
                    break;
                case "ExternalPOSCardIssue":
                    {
                        productName = lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValue == "I").FirstOrDefault().Description;
                        if (string.IsNullOrEmpty(productName))
                        {
                            productName = "EXTERNALPOSCARDISSUEVARIABLE";
                        }
                    }
                    break;
                default:
                    productName = adjustmentProductName;
                    break;
            }
            ProductsList productsList = new ProductsList(executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_EXACT_NAME, productName));
            List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParameters);
            log.Debug("productsDTOList : " + productsDTOList);
            if (productsDTOList != null && productsDTOList.Any())
            {
                variableProductId = productsDTOList[0].ProductId;
            }
            else
            {
                log.Error("ExternalPOS products are not setup.");
                throw new ValidationException("ExternalPOS products are not setup.");
            }
            log.LogMethodExit(variableProductId);
            return variableProductId;
        }

        /// <summary>
        /// Get Variable ProductId
        /// </summary>
        /// <param name="transactionId">transactionId</param>
        /// <param name="utilities">utilities</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>transactionDTOList</returns>
        public List<TransactionDTO> GetAllTransactionDTOList(int transactionId, Utilities utilities, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionId, utilities, sqlTransaction);
            TransactionListBL transactionListBL = new TransactionListBL(executionContext);
            List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
            List<TransactionDTO> transactionDTOList = transactionListBL.GetTransactionDTOList(searchParameters, utilities, null, 0, 1, true, false, false);
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        public ExternalTransactionDTO AddProduct(int transactionId, ExternalAddProductDTO externalAddProductDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionId, externalAddProductDTO, sqlTransaction);
            Utilities utilities = GetUtility();
            string transactionMessage = string.Empty;
            if (transactionId < 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, 1673);
                string errorMessage = "Invalid inputs - TransactionId  is empty. " + message;
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (externalAddProductDTO.Type != AdjustmentTypes.AddProduct.ToString())
            {
                log.Error("Please enter valid Adjustment  type ");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid Adjustment  type"));
            }
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            Transaction.Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
            Transaction.Card cards = new Transaction.Card(utilities);
            if (!string.IsNullOrEmpty(externalAddProductDTO.CardNumber))
            {
                AccountBL accountBL = new AccountBL(executionContext, externalAddProductDTO.CardNumber, false, false, sqlTransaction);
                if (accountBL.AccountDTO == null)
                {
                    //log.Debug("Account does not exists .Creating new Account");
                    //DateTime currentTime = ServerDateTime.Now;
                    //AccountDTO accountDTO = new AccountDTO(-1, externalAddProductDTO.CardNumber, "", currentTime, null, false, null, null, true,
                    //                            null, null, null, null, null, null, -1, null, true, true, false, null, null, "N", null,
                    //                            false, null, -1, null, null, -1, null, null, false, -1, "", "",
                    //                            currentTime, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(),
                    //                            currentTime);
                    //accountBL = new AccountBL(executionContext, accountDTO);
                    //accountBL.Save(sqlTransaction);

                    cards = new Parafait.Transaction.Card(externalAddProductDTO.CardNumber, executionContext.GetUserId(), utilities, sqlTransaction);
                }
                else
                {
                    cards = new Parafait.Transaction.Card(accountBL.AccountDTO.AccountId, executionContext.GetUserId(), utilities, sqlTransaction);
                }
            }
            Adjustments adjustments = new Adjustments("AddProduct", externalAddProductDTO.ProductId, null, externalAddProductDTO.ProductReference, externalAddProductDTO.CardNumber,
                 externalAddProductDTO.Quantity, externalAddProductDTO.Amount, new List<Points>());
            CreateTransactionLine(cards, adjustments, transaction, utilities, sqlTransaction);
            int trxId = transaction.SaveOrder(ref transactionMessage, sqlTransaction);
            if (trxId != 0)
            {
                log.Debug("Failed to save transaction " + transactionMessage);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to save transaction" + transactionMessage));
            }
            transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "External POS save transaction status : " + transaction.Trx_id, sqlTransaction);
            ExternalTransactionDTO externalTransactionDTO = GetTransactionDetails(transactionId, utilities, sqlTransaction);
            log.Debug("SaveTransaction" + transactionMessage);
            log.LogMethodExit(externalTransactionDTO);
            return externalTransactionDTO;
        }

        public void AddValue(int cardId, ExternalAddCreditsDTO externalAddCreditsDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardId, externalAddCreditsDTO, sqlTransaction);
            Utilities utilities = GetUtility();
            string transactionMessage = string.Empty;

            if (cardId < 0)
            {
                string errorMessage = "Invalid inputs - Account Id  is empty";
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (externalAddCreditsDTO.Type != AdjustmentTypes.AddValue.ToString())
            {
                log.Error("Please enter valid Adjustment type ");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid Adjustment type"));
            }
            Transaction.Transaction transaction = new Parafait.Transaction.Transaction(utilities);
            Transaction.Card cards = new Transaction.Card(utilities);
            if (cardId > -1)
            {
                AccountBL accountBL = new AccountBL(executionContext, cardId, false, false, sqlTransaction);
                if (accountBL.AccountDTO == null)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2948);
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                cards = new Parafait.Transaction.Card(accountBL.AccountDTO.AccountId, executionContext.GetUserId(), utilities, sqlTransaction);
            }

            Adjustments adjustments = new Adjustments(externalAddCreditsDTO.Type, -1, null, null, cards.CardNumber,
              1, null, externalAddCreditsDTO.Point);
            CreateTransactionLine(cards, adjustments, transaction, utilities, sqlTransaction);
            double amount = transaction.Net_Transaction_Amount;
            PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
            searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
            if (paymentModeDTOList != null)
            {

                TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, amount,
                                                                                                    "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                                    executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);
            }
            int trxId = transaction.SaveTransacation(sqlTransaction, ref transactionMessage);
            if (trxId != 0)
            {
                log.Debug("Failed to save transaction " + transactionMessage);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to save transaction" + transactionMessage));
            }
            transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "External POS save transaction status : " + transaction.Trx_id, sqlTransaction);
            log.Debug("SaveTransaction" + transactionMessage);
            log.LogMethodExit();
        }

        public void RemoveValue(int cardId, ExternalRemoveCreditsDTO externalRemoveCreditsDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardId, externalRemoveCreditsDTO, sqlTransaction);
            int refundProductId = -1;
            Utilities utilities = GetUtility();
            string transactionMessage = string.Empty;
            //using (ParafaitDBTransaction trx = new ParafaitDBTransaction())
            //{
            //    trx.BeginTransaction();
            if (cardId < 0)
            {
                string errorMessage = "Invalid inputs - Account Id  is empty";
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (externalRemoveCreditsDTO.Type != AdjustmentTypes.RemoveValue.ToString())
            {
                log.Error("Please enter valid Adjustment type ");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid Adjustment type"));
            }
            refundProductId = GetRefundProductId();
            Transaction.Transaction transaction = new Parafait.Transaction.Transaction(utilities);
            Transaction.Card cards = new Transaction.Card(utilities);
            if (cardId > -1)
            {
                AccountBL accountBL = new AccountBL(executionContext, cardId, false, false, sqlTransaction);
                if (accountBL.AccountDTO == null)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2948);
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                cards = new Parafait.Transaction.Card(accountBL.AccountDTO.AccountId, executionContext.GetUserId(), utilities, sqlTransaction);
            }

            Adjustments adjustments = new Adjustments(externalRemoveCreditsDTO.Type, -1, null, null, cards.CardNumber,
                1, null, externalRemoveCreditsDTO.Point);
            CreateTransactionLine(cards, adjustments, transaction, utilities, sqlTransaction, refundProductId);
            double amount = transaction.Net_Transaction_Amount;
            PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
            searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
            if (paymentModeDTOList != null)
            {

                TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, amount,
                                                                                                    "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                                    executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);
            }

            int trxId = transaction.SaveTransacation(sqlTransaction, ref transactionMessage);
            if (trxId != 0)
            {
                log.Debug("Failed to save transaction " + transactionMessage);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to save transaction" + transactionMessage));
            }
            transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "External POS save transaction status : " + transaction.Trx_id, sqlTransaction);
            log.Debug("SaveTransaction" + transactionMessage);
            //    trx.EndTransaction();
            //}
            log.LogMethodExit();
        }

        public ExternalTransactionDTO AddPayment(int transactionId, ExternalAddPaymentDTO externalAddPaymentDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionId, externalAddPaymentDTO, sqlTransaction);
            Utilities utilities = GetUtility();
            string transactionMessage = string.Empty;
            if (transactionId < 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, 1673);
                string errorMessage = "Invalid inputs - TransactionId  is empty. " + message;
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            Transaction.Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
            if (transaction.TransactionLineList != null && transaction.TransactionLineList.Any())
            {
                if (externalAddPaymentDTO.Type == AdjustmentTypes.Cash.ToString())
                {
                    if (externalAddPaymentDTO.PaymentModeId < 0)
                    {
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                        if (paymentModeDTOList != null)
                        {
                            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, Convert.ToDouble(externalAddPaymentDTO.Amount),
                                                                                            "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                            executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                            transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                            transaction.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                        }
                    }
                    else
                    {
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, externalAddPaymentDTO.PaymentModeId.ToString()));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                        if (paymentModeDTOList != null && paymentModeDTOList.Any())
                        {
                            if (paymentModeDTOList.FirstOrDefault().IsCash == false)
                            {
                                string errorMessage = "Please enter valid Payment Type. ";
                                log.Error("Throwing Exception- " + errorMessage);
                                throw new ValidationException(errorMessage);
                            }

                            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, externalAddPaymentDTO.PaymentModeId, Convert.ToDouble(externalAddPaymentDTO.Amount),
                                                                                            "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                            executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                            transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                            transaction.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                        }
                        else
                        {
                            string errorMessage = "Invalid Payment Mode. Please setup specific payment mode.";
                            log.Error("Throwing Exception- " + errorMessage);
                            throw new ValidationException(errorMessage);
                        }
                    }
                }
                else if (externalAddPaymentDTO.Type == AdjustmentTypes.CreditDebit.ToString())
                {
                    if (externalAddPaymentDTO.CreditCard == null)
                    {
                        string errorMessage = "Please enter credit card details. ";
                        log.Error("Throwing Exception- " + errorMessage);
                        throw new ValidationException(errorMessage);
                    }


                    if (externalAddPaymentDTO.PaymentModeId < 0)
                    {
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                        if (paymentModeDTOList != null)
                        {
                            if (string.IsNullOrEmpty(externalAddPaymentDTO.CreditCard.MaskedCardNumber) || string.IsNullOrEmpty(externalAddPaymentDTO.CreditCard.ExpiryDate))
                            {
                                string errorMessage = "Please enter MaskedCardNumber and  ExpiryDate. ";
                                log.Error("Throwing Exception- " + errorMessage);
                                throw new ValidationException(errorMessage);
                            }
                            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, Convert.ToDouble(externalAddPaymentDTO.Amount),
                                                                                           externalAddPaymentDTO.CreditCard.MaskedCardNumber, externalAddPaymentDTO.CreditCard.CardName, "", externalAddPaymentDTO.CreditCard.ExpiryDate,
                                                                                           string.Empty, -1, string.Empty, -1, 0, -1, string.Empty, string.Empty, false, -1, -1, string.Empty, ServerDateTime.Now,
                                                                                           executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, string.Empty, null);

                            transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                            transaction.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                        }
                    }
                    else
                    {
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, externalAddPaymentDTO.PaymentModeId.ToString()));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                        if (paymentModeDTOList != null && paymentModeDTOList.Any())
                        {
                            int cardId = -1;
                            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO();
                            if (paymentModeDTOList.FirstOrDefault().IsDebitCard)
                            {
                                if (string.IsNullOrEmpty(externalAddPaymentDTO.CreditCard.CardNumber))
                                {
                                    string errorMessage = "Please enter the debit card number.";
                                    log.Error("Throwing Exception- " + errorMessage);
                                    throw new ValidationException(errorMessage);
                                }
                                AccountBL accountBL = new AccountBL(executionContext, externalAddPaymentDTO.CreditCard.CardNumber, true, true);
                                if (accountBL.AccountDTO == null)
                                {
                                    string errorMessage = "Debit Card not found.";
                                    log.Error("Throwing Exception- " + errorMessage);
                                    throw new ValidationException(errorMessage);
                                }
                                else
                                {
                                    cardId = accountBL.GetAccountId();
                                }
                                bool voucherApplied = transaction.ApplyVoucher(new Card(externalAddPaymentDTO.CreditCard.CardNumber, "", utilities), ref transactionMessage);
                                if (voucherApplied)
                                {
                                    log.Info("voucherApplied for transaction:"+ externalAddPaymentDTO.CreditCard.CardNumber);
                                    transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, Convert.ToDouble(externalAddPaymentDTO.Amount),
                                                                                         externalAddPaymentDTO.CreditCard.MaskedCardNumber, externalAddPaymentDTO.CreditCard.CardName, "", externalAddPaymentDTO.CreditCard.ExpiryDate,
                                                                                         string.Empty, cardId, string.Empty, -1, 0, -1, string.Empty, string.Empty, false, -1, -1, string.Empty, ServerDateTime.Now,
                                                                                         executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, string.Empty, null);
                                }
                                else
                                {
                                    double credits = Convert.ToDouble(accountBL.AccountDTO.Credits);
                                    double transactionAmount = Convert.ToDouble(externalAddPaymentDTO.Amount);
                                    CreditPlus creditPlus = new CreditPlus(utilities);
                                    double creditPlusAvailable = creditPlus.getCreditPlusForPOS(accountBL.AccountDTO.AccountId, utilities.ParafaitEnv.POSTypeId, transaction);
                                    double creditAmountAvailable = credits;
                                    double amount = 0;
                                    double paymentUsedCreditplus = 0;
                                    if (transactionAmount > creditPlusAvailable)
                                    {
                                        paymentUsedCreditplus = creditPlusAvailable;
                                        transactionAmount -= creditPlusAvailable;

                                        if (transactionAmount > creditAmountAvailable)
                                        {
                                            amount = credits;
                                            transactionAmount -= creditAmountAvailable;
                                        }
                                        else
                                        {
                                            double usedCredits = transactionAmount;
                                            amount = usedCredits;
                                            transactionAmount = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (transactionAmount >= 0)
                                        {
                                            paymentUsedCreditplus = transactionAmount;
                                        }
                                        else
                                        {
                                            amount = transactionAmount;
                                        }
                                        transactionAmount = 0;
                                    }
                                    if (transactionAmount != 0)
                                    {
                                        string errorMessage = MessageContainerList.GetMessage(executionContext,183);
                                        log.Error("Throwing Exception- Insufficient Credits on Game Card(s)");
                                        throw new ValidationException(errorMessage);
                                    }
                                    transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, amount,
                                                                                         externalAddPaymentDTO.CreditCard.MaskedCardNumber, externalAddPaymentDTO.CreditCard.CardName, "", externalAddPaymentDTO.CreditCard.ExpiryDate,
                                                                                         string.Empty, cardId, string.Empty, -1, paymentUsedCreditplus, -1, string.Empty, string.Empty, false, -1, -1, string.Empty, ServerDateTime.Now,
                                                                                         executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, string.Empty, null);
                                }
                            }
                            else if (paymentModeDTOList.FirstOrDefault().IsCreditCard)
                            {
                                if (string.IsNullOrEmpty(externalAddPaymentDTO.CreditCard.MaskedCardNumber) || string.IsNullOrEmpty(externalAddPaymentDTO.CreditCard.ExpiryDate))
                                {
                                    string errorMessage = "Please enter MaskedCardNumber and  ExpiryDate. ";
                                    log.Error("Throwing Exception- " + errorMessage);
                                    throw new ValidationException(errorMessage);
                                }
                                transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, Convert.ToDouble(externalAddPaymentDTO.Amount),
                                                                                          externalAddPaymentDTO.CreditCard.MaskedCardNumber, externalAddPaymentDTO.CreditCard.CardName, "", externalAddPaymentDTO.CreditCard.ExpiryDate,
                                                                                          string.Empty, -1, string.Empty, -1, 0, -1, string.Empty, string.Empty, false, -1, -1, string.Empty, ServerDateTime.Now,
                                                                                          executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, string.Empty, null);
                            }
                            transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                            transaction.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                        }
                        else
                        {
                            string errorMessage = "Invalid Payment Mode. Please setup specific payment mode.";
                            log.Error("Throwing Exception- " + errorMessage);
                            throw new ValidationException(errorMessage);
                        }
                    }
                }
                else
                {
                    log.Error("Please enter valid Payment Type ");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid Payment Type"));
                }
                int trxId = transaction.SaveOrder(ref transactionMessage, sqlTransaction);
                if (trxId != 0)
                {
                    log.Debug("Failed to save transaction " + transactionMessage);
                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to save transaction" + transactionMessage));
                }
                transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "External POS save transaction status : " + transaction.Trx_id, sqlTransaction);
                log.Debug("SaveTransaction" + transactionMessage);
            }
            else
            {
                throw new ValidationException("Please add entitlement to the transaction");
            }
            ExternalTransactionDTO externalTransactionDTO = GetTransactionDetails(transactionId, utilities, sqlTransaction);
            log.LogMethodExit(externalTransactionDTO);
            return externalTransactionDTO;
        }

        public void RemovePayment(int transactionId, ExternalRemovePaymentDTO externalRemovePaymentDTO, SqlTransaction sqlTransaction = null)
        {
            Utilities utilities = GetUtility();
            string transactionMessage = string.Empty;
            if (transactionId < 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, 1673);
                string errorMessage = "Invalid inputs - TransactionId  is empty. " + message;
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            Transaction.Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
            if (transaction.TransactionLineList != null && transaction.TransactionLineList.Any())
            {
                if (externalRemovePaymentDTO.Type == AdjustmentTypes.Cash.ToString())
                {
                    if (externalRemovePaymentDTO.PaymentModeId < 0)
                    {
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                        if (paymentModeDTOList != null)
                        {
                            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, transaction.Transaction_Amount * -1,
                                                                                            "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                            utilities.ParafaitEnv.LoginID, -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                            transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                            transaction.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                        }
                    }
                    else
                    {
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, externalRemovePaymentDTO.PaymentModeId.ToString()));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                        if (paymentModeDTOList != null)
                        {
                            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, -1, externalRemovePaymentDTO.PaymentModeId, transaction.Transaction_Amount * -1,
                                                                                            "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                            utilities.ParafaitEnv.LoginID, -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                            transactionPaymentsDTO.paymentModeDTO = paymentModeDTOList[0];
                            transaction.TransactionPaymentsDTOList.Add(transactionPaymentsDTO);
                        }
                    }
                }
                else if (externalRemovePaymentDTO.Type == AdjustmentTypes.CreditDebit.ToString())
                {
                    if (externalRemovePaymentDTO.PaymentModeId < 0)
                    {
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                        if (paymentModeDTOList != null)
                        {
                            TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, transaction.Transaction_Amount * -1,
                                                                                                         string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, -1, string.Empty, -1, -1, "", "", false, executionContext.GetSiteId(), -1, "", ServerDateTime.Now,
                                                                                                        executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, string.Empty, null, null, null, true);
                            trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                            transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                        }
                    }
                    else
                    {
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, externalRemovePaymentDTO.PaymentModeId.ToString()));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                        if (paymentModeDTOList != null)
                        {
                            TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, externalRemovePaymentDTO.PaymentModeId, transaction.Transaction_Amount * -1,
                                                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, -1, string.Empty, -1, -1, "", "", false, executionContext.GetSiteId(), -1, "", ServerDateTime.Now,
                                                                                                       executionContext.GetUserId(), -1, null, 0, -1, executionContext.POSMachineName, -1, string.Empty, null, null, null, true);
                            trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                            transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                        }
                    }
                }
                else
                {
                    log.Error("Please enter valid Payment Type ");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid Payment Type"));
                }
                int trxId = transaction.SaveOrder(ref transactionMessage, sqlTransaction);
                if (trxId != 0)
                {
                    log.Debug("Failed to save transaction " + transactionMessage);
                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to save transaction" + transactionMessage));
                }
                transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "External POS save transaction status : " + transaction.Trx_id, sqlTransaction);
                log.Debug("SaveTransaction" + transactionMessage);
            }
        }

        public void AddDiscount(int transactionId, ExternalAddDiscountDTO externalAddDiscountDTO, SqlTransaction sqlTransaction = null)
        {
            Utilities utilities = GetUtility();
            string transactionMessage = string.Empty;
            if (transactionId < 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, 1673);
                string errorMessage = "Invalid inputs - TransactionId  is empty. " + message;
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            Transaction.Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
            if (transaction.TransactionLineList != null && transaction.TransactionLineList.Any())
            {

                DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, new ExternallyManagedUnitOfWork(sqlTransaction));
                SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_NAME, externalAddDiscountDTO.DiscountName));
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                List<DiscountsDTO> discountsDTOList = discountsListBL.GetDiscountsDTOList(searchParameters, false, false, false);
                if (discountsDTOList != null)
                {
                    if (discountsDTOList.Count > 1)
                    {
                        log.Debug("Duplicate discount exists. Please enter valid discount");
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Duplicate discount exists. Please enter valid discount"));
                    }
                    else
                    {
                        TransactionDiscountsDTO transactionDiscountsDTO = new TransactionDiscountsDTO();
                        if (discountsDTOList[0].VariableDiscounts == "Y")
                        {
                            transactionDiscountsDTO = new TransactionDiscountsDTO(-1, -1, -1, discountsDTOList[0].DiscountId, Convert.ToDecimal(discountsDTOList[0].DiscountPercentage),
                            externalAddDiscountDTO.Amount, externalAddDiscountDTO.Remarks, -1, DiscountApplicability.LINE);
                        }
                        else
                        {
                            transactionDiscountsDTO = new TransactionDiscountsDTO(-1, -1, -1, discountsDTOList[0].DiscountId, Convert.ToDecimal(discountsDTOList[0].DiscountPercentage),
                            Convert.ToDecimal(discountsDTOList[0].DiscountAmount), externalAddDiscountDTO.Remarks, -1, DiscountApplicability.LINE);

                        }
                        foreach (Transaction.Transaction.TransactionLine transactionLine in transaction.TransactionLineList)
                        {
                            transactionLine.TransactionDiscountsDTOList.Add(transactionDiscountsDTO);
                        }
                    }
                }

                int trxId = transaction.SaveOrder(ref transactionMessage, sqlTransaction);
                if (trxId != 0)
                {
                    log.Debug("Failed to save transaction " + transactionMessage);
                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to save transaction" + transactionMessage));
                }
                transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "External POS save transaction status : " + transaction.Trx_id, sqlTransaction);
                log.Debug("SaveTransaction" + transactionMessage);
            }

        }

        public void RemoveDiscount(int transactionId, ExternalRemoveDiscountDTO externalRemoveDiscountDTO, SqlTransaction sqlTransaction = null)
        {
            Utilities utilities = GetUtility();
            string transactionMessage = string.Empty;
            if (transactionId < 0)
            {
                log.Error("TransactionId should be greater than zero");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Failed to save transaction" + transactionMessage));
            }
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            Transaction.Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
            if (transaction.TransactionLineList != null && transaction.TransactionLineList.Any())
            {

                DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, new ExternallyManagedUnitOfWork(sqlTransaction));
                SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_NAME, externalRemoveDiscountDTO.DiscountName));
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                List<DiscountsDTO> discountsDTOList = discountsListBL.GetDiscountsDTOList(searchParameters, false, false, false);
                if (discountsDTOList != null)
                {
                    if (discountsDTOList.Count > 1)
                    {
                        log.Debug("Duplicate discount exists. Please enter valid discount");
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Duplicate discount exists. Please enter valid discount"));
                    }
                    else
                    {
                        TransactionDiscountsDTO transactionDiscountsDTO = new TransactionDiscountsDTO();
                        transactionDiscountsDTO = new TransactionDiscountsDTO(-1, -1, -1, discountsDTOList[0].DiscountId, Convert.ToDecimal(discountsDTOList[0].DiscountPercentage),
                        Convert.ToDecimal(discountsDTOList[0].DiscountAmount), externalRemoveDiscountDTO.Remarks, -1, DiscountApplicability.LINE);
                        foreach (Transaction.Transaction.TransactionLine transactionLine in transaction.TransactionLineList)
                        {
                            transactionLine.TransactionDiscountsDTOList.Add(transactionDiscountsDTO);
                        }
                    }
                }

                int trxId = transaction.SaveOrder(ref transactionMessage, sqlTransaction);
                if (trxId != 0)
                {
                    log.Debug("Failed to save transaction " + transactionMessage);
                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to save transaction" + transactionMessage));
                }
                transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "External POS save transaction status : " + transaction.Trx_id, sqlTransaction);
                log.Debug("SaveTransaction" + transactionMessage);
            }
            else
            {
                throw new ValidationException("Please add entitlement to the transaction");
            }
        }

        public void CompleteTransaction(int transactionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionId, sqlTransaction);
            Utilities utilities = GetUtility();
            string transactionMessage = string.Empty;
            if (transactionId < 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, 1673);
                string errorMessage = "Invalid inputs - TransactionId  is empty. " + message;
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            Transaction.Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
            if (transaction.TransactionLineList == null || transaction.TransactionLineList.Count < 1)
            {
                string message = MessageContainerList.GetMessage(executionContext, 1673);
                string errorMessage = "Invalid inputs - Transaction  is empty. " + message;
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            int trxId = transaction.SaveTransacation(sqlTransaction, ref transactionMessage);
            if (trxId != 0)
            {
                log.Debug("Failed to save transaction " + transactionMessage);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to save transaction. " + transactionMessage));
            }
            transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "External POS save transaction status : " + transaction.Trx_id, sqlTransaction);
            log.Debug("SaveTransaction" + transactionMessage);
            log.LogMethodExit();
        }

        public void ReverseTransaction(int transactionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionId, sqlTransaction);
            Utilities utilities = GetUtility();
            if (transactionId < 0)
            {
                string messages = MessageContainerList.GetMessage(executionContext, 1673);
                string errorMessage = "Invalid inputs - TransactionId  is empty. " + messages;
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            string transactionMessage = string.Empty;
            string message = string.Empty;
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            if (!transactionUtils.reverseTransaction(transactionId, -1, true, utilities.ParafaitEnv.POSMachine == null ? Environment.MachineName : utilities.ParafaitEnv.POSMachine,
                 utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.User_Id, utilities.ParafaitEnv.LoginID, "Reverse LoadTickets: ", ref message))
            {
                log.Error(message);
                throw new Exception(message);
            }
            log.Debug("ReverseTransaction" + message);
            log.LogMethodExit();
        }

        public void Activate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Utilities utilities = GetUtility();

            if (string.IsNullOrEmpty(accountNumber))
            {
                string errorMessage = "Invalid inputs - Account Number is empty";
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            Transaction.Transaction transaction = new Parafait.Transaction.Transaction(utilities);
            Transaction.Card cards = new Transaction.Card(utilities);
            if (!string.IsNullOrEmpty(accountNumber))
            {
                AccountBL accountBL = new AccountBL(executionContext, accountNumber, false, false, sqlTransaction);
                if (accountBL.AccountDTO != null)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 363);
                    log.Error("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                else
                {
                    log.Debug("Account does not exists.Creating new Account");
                    cards = new Parafait.Transaction.Card(accountNumber, executionContext.GetUserId(), utilities, sqlTransaction);
                }
            }

            int trxLineReturnValue = -1;
            string transactionLineMessage = string.Empty;
            int productId = GetVariableProductId("ExternalPOSCardIssue");
            if(productId > -1)
            {
                trxLineReturnValue = transaction.createTransactionLine(cards, productId, -1, 1, ref transactionLineMessage);
            }
            else
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 111);
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.Debug(transactionLineMessage);
            if (trxLineReturnValue != 0)
            {
                log.Debug("Failed to create transaction line:Account Activate. " + transactionLineMessage);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line. " + transactionLineMessage));
            }

            PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
            searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
            if (paymentModeDTOList != null)
            {

                TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, transaction.Transaction_Amount,
                                                                                    "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                    utilities.ParafaitEnv.LoginID, -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);
            }
            string transactionMessage = string.Empty;
            int trxId = transaction.SaveTransacation(sqlTransaction, ref transactionMessage);
            if (trxId != 0)
            {
                log.Debug("Failed to save transaction " + transactionMessage);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to save transaction" + transactionMessage));
            }
            transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "External POS save transaction status : " + transaction.Trx_id, sqlTransaction);
            log.Debug("SaveTransaction" + transactionMessage);
            log.LogMethodExit();
        }

        public void SaveAccountIdentifier(string accountIdentifier, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountIdentifier, sqlTransaction);

            if (id == -1 || string.IsNullOrWhiteSpace(accountIdentifier))
            {
                string errorMessage = "Invalid Inputs - Account Id or Account Identifier is empty";
                log.LogMethodExit("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                AccountBL accountBL = new AccountBL(executionContext, id);
                if (accountBL.AccountDTO == null || accountBL.AccountDTO.AccountId == -1)
                {
                    string errorMessage = "Invalid AccountId " + id;
                    log.LogMethodExit("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                accountBL.AccountDTO.AccountIdentifier = accountIdentifier;
                accountBL.SaveManualChanges(true, parafaitDBTrx.SQLTrx);
            }
            log.LogMethodExit();
        }

        public string GenerateAccountNumber(bool tempCard, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(tempCard, sqlTransaction);
            string accountNumber = string.Empty;
            TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(executionContext);
            RandomTagNumber randomTagNumber = new RandomTagNumber(executionContext, tagNumberLengthList);
            if (tempCard)
            {
                accountNumber = "T" + randomTagNumber.Value.Substring(1);
            }
            else
            {
                accountNumber = randomTagNumber.Value;
            }
            log.LogMethodExit(accountNumber);
            return accountNumber;
        }
    }

    public class ExternalTransactionListBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ExternalTransactionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public ExternalTransactionDTO AddProducts(int transactionId, ExternalAddProductDTO externalAddProductDTO)
        {
            log.LogMethodEntry(transactionId, externalAddProductDTO);
            ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, transactionId);
            ExternalTransactionDTO externalTransactionDTO = externalTransactionBL.AddProduct(transactionId, externalAddProductDTO);
            log.LogMethodExit(externalTransactionDTO);
            return externalTransactionDTO;
        }

        public ExternalTransactionDTO AddPayment(int transactionId, ExternalAddPaymentDTO externalAddPaymentDTO)
        {
            log.LogMethodEntry(transactionId, externalAddPaymentDTO);
            ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, transactionId);
            ExternalTransactionDTO externalTransactionDTO = externalTransactionBL.AddPayment(transactionId, externalAddPaymentDTO);
            log.LogMethodExit(externalTransactionDTO);
            return externalTransactionDTO;
        }

        public void RemovePayment(int transactionId, ExternalRemovePaymentDTO externalAddPaymentDTO)
        {
            log.LogMethodEntry(executionContext);
            ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, transactionId);
            externalTransactionBL.RemovePayment(transactionId, externalAddPaymentDTO);
            log.LogMethodExit();
        }

        public void AddValue(int cardId, ExternalAddCreditsDTO externalAddCreditsDTO)
        {
            log.LogMethodEntry(executionContext);
            ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, cardId);
            externalTransactionBL.AddValue(cardId, externalAddCreditsDTO);
            log.LogMethodExit();
        }

        public void RemoveValue(int cardId, ExternalRemoveCreditsDTO externalRemoveCreditsDTO)
        {
            log.LogMethodEntry(executionContext);
            ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, cardId);
            externalTransactionBL.RemoveValue(cardId, externalRemoveCreditsDTO);
            log.LogMethodExit();
        }

        public void AddDiscount(int transactionId, ExternalAddDiscountDTO externalAddDiscountDTO)
        {
            log.LogMethodEntry(executionContext);
            ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, transactionId);
            externalTransactionBL.AddDiscount(transactionId, externalAddDiscountDTO);
            log.LogMethodExit();
        }

        public void RemoveDiscount(int transactionId, ExternalRemoveDiscountDTO externalRemoveDiscountDTO)
        {
            log.LogMethodEntry(executionContext);
            ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, transactionId);
            externalTransactionBL.RemoveDiscount(transactionId, externalRemoveDiscountDTO);
            log.LogMethodExit();
        }

        public void CompleteTransaction(int transactionId)
        {
            log.LogMethodEntry(executionContext);
            ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, transactionId);
            externalTransactionBL.CompleteTransaction(transactionId);
            log.LogMethodExit();
        }


        public void ReverseTransaction(int transactionId)
        {
            log.LogMethodEntry(executionContext);
            ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, transactionId);
            externalTransactionBL.ReverseTransaction(transactionId);
            log.LogMethodExit();
        }

        public void Activate(string accountNumber)
        {
            log.LogMethodEntry(executionContext);
            ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, accountNumber);
            externalTransactionBL.Activate();
            log.LogMethodExit();
        }
    }
}