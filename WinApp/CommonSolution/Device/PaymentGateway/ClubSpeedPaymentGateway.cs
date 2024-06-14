
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Threading;
namespace Semnox.Parafait.Device.PaymentGateway
{
    public class ClubSpeedPaymentGateway : PaymentGateway
    {
        #region members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool isAuthEnabled;
        bool enableAutoAuthorization;
        bool isManual;
        IDisplayStatusUI statusDisplayUi;
        private List<LookupValuesContainerDTO> lookupValuesContainerDTOList = null;
        public delegate void DisplayWindow();
        #endregion

        #region properties
        public override bool IsTipAdjustmentAllowed
        {
            get { return true; }
        }
        public enum Alignment
        {
            Left,
            Right,
            Center
        }
        enum TransactionType
        {
            TATokenRequest,
            SALE,
            REFUND,
            AUTHORIZATION,
            VOID,
            PREAUTH,
            CAPTURE
        }
        #endregion

        public ClubSpeedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel   
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
            isAuthEnabled = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
            enableAutoAuthorization = utilities.getParafaitDefaults("ENABLE_AUTO_CREDITCARD_AUTHORIZATION").Equals("Y");
            log.LogVariableState("isUnattended", isUnattended);
            log.LogVariableState("authorization", isAuthEnabled);
            log.LogVariableState("enableAutoAuthorization", enableAutoAuthorization);
            log.LogMethodExit(null);
        }
        #region methods
        public override void Initialize()
        {
            log.LogMethodEntry();
            lookupValuesContainerDTOList = GetlookupValuesOfClubSpeed();//get lookupvalues
            log.LogMethodExit();
        }
        private List<LookupValuesContainerDTO> GetlookupValuesOfClubSpeed()
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = new List<LookupValuesContainerDTO>();
            LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(utilities.ExecutionContext.SiteId, "CLUBSPEED_GIFT_CARD_CONFIGURATIONS");
            if (lookupsContainerDTO != null && lookupsContainerDTO.LookupValuesContainerDTOList != null)
            {
                lookupValuesContainerDTOList = lookupsContainerDTO.LookupValuesContainerDTOList;
            }
            log.LogMethodEntry(lookupValuesContainerDTOList);
            return lookupValuesContainerDTOList;
        }
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            VerifyPaymentRequest(transactionPaymentsDTO);
            CCRequestPGWDTO cCRequestPGWDTOException = null;
            PrintReceipt = true;
            statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "ClubSpeed  Payment Gateway");
            statusDisplayUi.EnableCancelButton(false);
            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
            isManual = false;
            TransactionType trxType = TransactionType.SALE;
            string paymentId = string.Empty;
            CCTransactionsPGWDTO cCOrgTransactionsPGWDTO = null;
            double amount = transactionPaymentsDTO.Amount;
            Customer customerDetails = null;
            ClubSpeedCommandHandler commandHandler = new ClubSpeedCommandHandler(utilities.ExecutionContext, lookupValuesContainerDTOList);
            try
            {
                if (transactionPaymentsDTO.Amount >= 0)
                {
                    if (!isUnattended)
                    {
                        if (isAuthEnabled && enableAutoAuthorization)
                        {
                            log.Debug("Creditcard auto authorization is enabled");
                            trxType = TransactionType.AUTHORIZATION;
                        }
                        else
                        {
                            cCOrgTransactionsPGWDTO = GetPreAuthorizationCCTransactionsPGWDTO(transactionPaymentsDTO);
                            if (isAuthEnabled)
                            {
                                trxType = TransactionType.SALE;//by default trxType is SALE
                            }
                        }
                    }
                }
                thr.Start();
                if (transactionPaymentsDTO != null)
                {
                    log.LogVariableState("Enters into CreateCCRequestPGW method", 1);
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, trxType.ToString());
                    cCRequestPGWDTOException = ccRequestPGWDTO;//added
                    log.LogVariableState("Exits from CreateCCRequestPGW method", ccRequestPGWDTO);
                    var isPrintReceiptEnabled = PrintReceipt == true ? 1 : 0;


                    log.Debug("Calling Get Customer Balance based on CreditCardnumber:" + transactionPaymentsDTO.CreditCardNumber);
                    List<Customer> customers = commandHandler.getCustomerBalance(transactionPaymentsDTO.CreditCardNumber.Trim(), utilities.ExecutionContext);//changed to accept the cardNumber
                    if (customers != null && customers.Any())
                    {

                        foreach (var item in customers)
                        {
                            customerDetails = item;
                        }

                        if (customerDetails.balance < amount)
                        {
                            // insufficient balance
                            // throw error
                            statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4715, customerDetails.balance, customerDetails.cardId));
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4715, amount, customerDetails.cardId, customerDetails.balance));// The amount requested to pay is &1 but the balance of the card &2 is only &3.
                        }
                        else
                        {
                            //message to show about the points
                            statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4719, customerDetails.balance, customerDetails.cardId));//Your card &2 has balance of &1. Processing Payment. Please wait...
                            Customer paymentResponse = commandHandler.MakePayment(transactionPaymentsDTO.CreditCardNumber, amount, utilities.ExecutionContext);
                            statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4721, customerDetails.balance, paymentResponse.balance, customerDetails.cardId));//Your Current Balance is &1 for card &3. New balance after deduction is &2.
                            Thread.Sleep(3000);
                            log.LogVariableState("responseObject", paymentResponse);
                            if (paymentResponse == null)
                            {
                                // Payment could not be successful
                                // throw error
                                statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4716)); //Sorry!Payment Request failed..
                                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4716));
                            }
                            else
                            {
                                // payment successful
                                // update DB
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(paymentResponse.cardId.ToString());

                                cCTransactionsPGWDTO.CardType = transactionPaymentsDTO.paymentModeDTO.PaymentMode; // CS GiftCard 
                                cCTransactionsPGWDTO.RefNo = paymentResponse.referenceNo;
                                cCTransactionsPGWDTO.RecordNo = paymentResponse.cardId.ToString();

                                cCTransactionsPGWDTO.TextResponse = "Successful";
                                cCTransactionsPGWDTO.TranCode = trxType.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = amount.ToString();

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = amount;
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                                transactionPaymentsDTO.CreditCardAuthorization = paymentResponse.balance.ToString();
                                transactionPaymentsDTO.NameOnCreditCard = paymentResponse.cardId.ToString();//stored in transactionPaymentsDTO.NameOnCreditCard used for refund purpose in getting the cardNumber
                                statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4721, customerDetails.balance, paymentResponse.balance, paymentResponse.cardId));//use NameOnCreditCard to get cardNumber
                                //Your Current Balance is &1 for card &3. New balance after deduction is &2.
                            }
                        }
                    }
                    else
                    {
                        // customer info not found
                        // throw ex
                        statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4722, transactionPaymentsDTO.CreditCardNumber));//Unable to find the gift card &1
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4722, transactionPaymentsDTO.CreditCardNumber));
                    }
                    log.LogMethodExit(transactionPaymentsDTO);
                }
                else
                {
                    log.Fatal("Exception Inorrect object passed");
                    statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4372));//Exception in processing Payment
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4372));
                }
            }
            catch (Exception ex)
            {   //to handle case if the amount is deducted but due to network or other issues it throws exception
                Customer customerAfterPayment = null;
                List<Customer> customerDTOException = null;

                try
                {
                    customerDTOException = commandHandler.getCustomerBalance(transactionPaymentsDTO.CreditCardNumber.Trim(), utilities.ExecutionContext);//changed to accept the cardNumber
                }
                catch (Exception excp)
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, excp.Message));
                }
                if (customerDTOException != null && customerDTOException.Any())
                {

                    foreach (var item in customerDTOException)
                    {
                        customerAfterPayment = item;
                    }
                }
                if (customerDetails != null && customerAfterPayment != null)
                {
                    if (customerDetails.balance != customerAfterPayment.balance)
                    {
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTOException.RequestID.ToString();
                        cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(customerAfterPayment.cardId.ToString());

                        cCTransactionsPGWDTO.CardType = transactionPaymentsDTO.paymentModeDTO.PaymentMode; // CS GiftCard 
                        cCTransactionsPGWDTO.RefNo = customerAfterPayment.referenceNo;
                        cCTransactionsPGWDTO.RecordNo = customerAfterPayment.cardId.ToString();

                        cCTransactionsPGWDTO.TextResponse = "Successful";
                        cCTransactionsPGWDTO.TranCode = trxType.ToString();
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.Authorize = amount.ToString();

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                        transactionPaymentsDTO.Amount = amount;
                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                        transactionPaymentsDTO.CreditCardAuthorization = customerAfterPayment.balance.ToString();
                        transactionPaymentsDTO.NameOnCreditCard = customerAfterPayment.cardId.ToString();
                        return transactionPaymentsDTO;
                    }
                }

                statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.Message));
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.Message));
            }
            finally
            {
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            ClubSpeedCommandHandler commandHandler = new ClubSpeedCommandHandler(utilities.ExecutionContext, lookupValuesContainerDTOList);
            CCRequestPGWDTO CCRequestPGWDTORefund = null;
            CCTransactionsPGWDTO cCTransactionsPGWDTORefund = null;
            Customer customerBeforeRefund = null;
            Customer paymentResponse;
            try
            {
                List<Customer> customers = commandHandler.getCustomerBalance(transactionPaymentsDTO.NameOnCreditCard.Trim(), utilities.ExecutionContext);//changed to accept the cardNumber
                foreach (var item in customers)
                {
                    customerBeforeRefund = item;
                }

                PrintReceipt = true;
                var isPrintReceiptEnabled = PrintReceipt == true ? 1 : 0;
                if (transactionPaymentsDTO != null)
                {
                    if (transactionPaymentsDTO.Amount < 0)
                    {
                        statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4360));//Variable Refund Not Supported
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4360));
                    }
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "ClubSpeed Gift Card Payment Gateway");
                    statusDisplayUi.EnableCancelButton(false);
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.REFUND.ToString());
                    CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;
                    CCRequestPGWDTORefund = cCRequestPGWDTO;
                    cCTransactionsPGWDTORefund = ccOrigTransactionsPGWDTO;
                    double amount = (transactionPaymentsDTO.Amount);

                    // remove partial
                    // make sure the refund amt is less than oreder amt

                    // Process the Refund



                    // check for refund amt is not more than the purchase amt
                    if (Convert.ToDecimal(amount) <= Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize))
                    {
                        paymentResponse = commandHandler.RefundAmount(transactionPaymentsDTO.NameOnCreditCard, amount, utilities.ExecutionContext);
                        log.LogVariableState("paymentResponse", paymentResponse);
                        if (paymentResponse == null )
                        {
                            // Refund could not be successful
                            // throw error
                            statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4372));//Exception in processing Payment
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4372));
                        }
                        else
                        {
                            // Refund successful
                            // update DB
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(paymentResponse.cardId.ToString());

                            cCTransactionsPGWDTO.CardType = transactionPaymentsDTO.paymentModeDTO.PaymentMode; // ClubSpeed GiftCard
                            cCTransactionsPGWDTO.RefNo = paymentResponse.cardId.ToString();
                            cCTransactionsPGWDTO.RecordNo = paymentResponse.cardId.ToString();

                            cCTransactionsPGWDTO.TextResponse = "Refund Successful";
                            cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = amount.ToString();
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.Amount = amount;
                            transactionPaymentsDTO.CreditCardAuthorization = paymentResponse.balance.ToString();
                            transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4725, paymentResponse.balance, transactionPaymentsDTO.NameOnCreditCard));//Refund succeeded for card &2, Your Balance is &1.
                            Thread.Sleep(3000);
                        }
                    }
                    else
                    {
                        statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 4723));//Refund can not be completed as the amount to refund is more than the purchase amount
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4723));
                    }
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                Customer customerAfterRefund = null;
                List<Customer> customers = null;
                try
                {
                    customers = commandHandler.getCustomerBalance(transactionPaymentsDTO.NameOnCreditCard.Trim(), utilities.ExecutionContext);//changed to accept the cardNumber

                }
                catch (Exception excp)
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, excp.Message));
                }
                if (customers != null && customers.Any())
                {
                    foreach (var item in customers)
                    {
                        customerAfterRefund = item;
                    }
                }
                if (customerBeforeRefund != null && customerAfterRefund != null)
                {
                    if (customerBeforeRefund.balance != customerAfterRefund.balance)
                    {
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = CCRequestPGWDTORefund.RequestID.ToString();
                        cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(customerAfterRefund.cardId.ToString());

                        cCTransactionsPGWDTO.CardType = transactionPaymentsDTO.paymentModeDTO.PaymentMode; // ClubSpeed GiftCard
                        cCTransactionsPGWDTO.RefNo = customerAfterRefund.cardId.ToString();
                        cCTransactionsPGWDTO.RecordNo = customerAfterRefund.cardId.ToString();

                        cCTransactionsPGWDTO.TextResponse = "Refund Successful";
                        cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                        transactionPaymentsDTO.Amount = transactionPaymentsDTO.Amount;
                        transactionPaymentsDTO.CreditCardAuthorization = customerAfterRefund.balance.ToString();
                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                        return transactionPaymentsDTO;
                    }
                }
                statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.Message));
                log.Error("Error occured while Refunding the Amount", ex);
                log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                log.LogMethodExit(null, "throwing Exception");
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.Message));
            }

            finally
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
            }
        }

        private CCTransactionsPGWDTO GetPreAuthorizationCCTransactionsPGWDTO(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            CCTransactionsPGWDTO preAuthorizationCCTransactionsPGWDTO = null;
            if (utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y"))
            {
                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRANSACTION_ID, transactionPaymentsDTO.TransactionId.ToString()));
                if (transactionPaymentsDTO.SplitId != -1)
                {
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SPLIT_ID, transactionPaymentsDTO.SplitId.ToString()));
                }
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, TransactionType.TATokenRequest.ToString()));
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                {
                    preAuthorizationCCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                }
            }

            log.LogMethodExit(preAuthorizationCCTransactionsPGWDTO);
            return preAuthorizationCCTransactionsPGWDTO;
        }
        private string GetMaskedCardNumber(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            try
            {
                // TBC assumption => card has 16 digits
                string last4 = 4 > cardNumber.Length ? cardNumber : cardNumber.Substring(Math.Max(0, cardNumber.Length - 4));

                string card = string.Format("************{0}", last4);
                log.LogMethodExit(card);
                return card;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.Message));
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.Message));
            }
        }

        #endregion
    }
}
