/********************************************************************************************
 * Project Name - Adyen Payment Gateway
 * Description  - Data handler of the AdyenPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        20-SEP-2019   Raghuveera      Created  
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class AdyenPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool enableAutoAuthorization;
        IDisplayStatusUI statusDisplayUi;
        private bool isCustomerAllowedToDecideEntryMode;
        string deviceUrl;
        string merchantId;
        string userName;
        string password;
        string deviceId;
        string tokenId;
        string mcc;
        string environmentType;
        bool isSignatureRequired;
        private bool isAuthorizationAllowed;
        public AdyenPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
           : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
            if (showMessageDelegate == null)
            {
                showMessageDelegate = MessageBox.Show;
            }
            deviceUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_DEVICE_URL");
            merchantId = utilities.getParafaitDefaults("CREDIT_CARD_STORE_ID");
            userName = utilities.getParafaitDefaults("CREDIT_CARD_HOST_USERNAME");
            password = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_HOST_PASSWORD");//utilities.getParafaitDefaults("CREDIT_CARD_HOST_PASSWORD");
            deviceUrl = utilities.getParafaitDefaults("CREDIT_CARD_DEVICE_URL");
            deviceId = utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_ID");
            tokenId = utilities.getParafaitDefaults("CREDIT_CARD_TOKEN_ID");
            mcc = utilities.getParafaitDefaults("CREDIT_CARD_MCC");
            isSignatureRequired = !utilities.getParafaitDefaults("ENABLE_SIGNATURE_VERIFICATION").Equals("N");
            log.LogVariableState("merchantId", merchantId);
            log.LogVariableState("isUnattended", isUnattended);
            log.LogVariableState("username", userName);
            log.LogVariableState("mcc", mcc);
            log.LogVariableState("password", password);
            log.LogVariableState("TokenId", tokenId);
            log.LogVariableState("deviceUrl", deviceUrl);
            log.LogVariableState("deviceId", deviceId);
            log.LogVariableState("isAuthorizationAllowed", isAuthorizationAllowed);
            log.LogVariableState("enableAutoAuthorization", enableAutoAuthorization);
            log.LogVariableState("isCustomerAllowedToDecideEntryMode", isCustomerAllowedToDecideEntryMode);
            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);            
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "PAYMENT_GATEWAY"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Adyen"));
            List<LookupValuesDTO> lookupValuesDTOs = lookupValuesList.GetAllLookupValues(searchParameters);
            if(lookupValuesDTOs != null && lookupValuesDTOs.Count>0)
            {
               string[] env = lookupValuesDTOs[0].Description.Split('|');
                if (env.Length > 1)
                {
                    environmentType = env[1];
                }
                else
                {
                    environmentType = "T";
                }
            }
            if (string.IsNullOrEmpty(deviceUrl))
            {
                log.Info("Please enter CREDIT_CARD_DEVICE_URL value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_DEVICE_URL value in configuration."));
            }
            if (!deviceUrl.StartsWith("https://") || !deviceUrl.ToLower().Contains("nexo"))
            {
                log.Info("Please enter valid url in CREDIT_CARD_DEVICE_URL configuration.");

                throw new Exception(utilities.MessageUtils.getMessage("Please enter valid url in CREDIT_CARD_DEVICE_URL configuration."));
            }
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TERMINAL_ID value in configuration."));
            }
            //if (string.IsNullOrEmpty(tokenId))
            //{
            //    throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TOKEN_ID value in configuration for authorization."));
            //}            
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_HOST_USERNAME value in configuration."));
            }
            if (string.IsNullOrEmpty(mcc))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_MCC value in configuration."));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_HOST_PASSWORD value in configuration."));
            }
            if (string.IsNullOrEmpty(merchantId))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Configuration CREDIT_CARD_MERCHANT_ID is not set."));
            }

            isAuthorizationAllowed = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
            enableAutoAuthorization = utilities.getParafaitDefaults("ENABLE_AUTO_CREDITCARD_AUTHORIZATION").Equals("Y");
            isCustomerAllowedToDecideEntryMode = utilities.getParafaitDefaults("ALLOW_CUSTOMER_TO_DECIDE_ENTRY_MODE").Equals("Y");            
            log.LogMethodExit(null);
        }
        public override void Initialize()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            PrintReceipt = true;
            statusDisplayUi = DisplayUIFactory.GetStatusUI(isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Adyen Payment Gateway");
            statusDisplayUi.EnableCancelButton(false);
            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
            AdyenRequest adyenRequest = new AdyenRequest();
            if(isUnattended && string.IsNullOrEmpty(transactionPaymentsDTO.MCC))
            {
                log.Debug("Setting POSLevel MCC:" + mcc);
                adyenRequest.MerchantCategoryCode = mcc;
            }
            else if(!isUnattended && string.IsNullOrEmpty(transactionPaymentsDTO.MCC))
            {
                log.Debug("Setting POSLevel MCC:"+ mcc);
                adyenRequest.MerchantCategoryCode = mcc;
            }
            else
            {
                log.Debug("Setting dynamic MCC:" + transactionPaymentsDTO.MCC);
                adyenRequest.MerchantCategoryCode = transactionPaymentsDTO.MCC;
            }
            adyenRequest.TransactionType = PaymentGatewayTransactionType.SALE;
            adyenRequest.MerchantAccountName = merchantId;
            AdyenCommandHandler adyenCommandHandler = new AdyenCommandHandler(userName, password, deviceUrl,environmentType, utilities);
            CCTransactionsPGWDTO cCOrgTransactionsPGWDTO = null;
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            try
            {
                if (!isUnattended)
                {
                    if (isAuthorizationAllowed && enableAutoAuthorization)
                    {
                        log.Debug("Creditcard auto authorization is enabled");
                        adyenRequest.TransactionType = PaymentGatewayTransactionType.AUTHORIZATION;
                    }
                    else
                    {
                        cCOrgTransactionsPGWDTO = GetPreAuthorizationCCTransactionsPGWDTO(transactionPaymentsDTO);
                        if (isAuthorizationAllowed)
                        {
                            frmTransactionTypeUI frmTranType = new frmTransactionTypeUI(utilities, "Authorization", transactionPaymentsDTO.Amount, showMessageDelegate);//(cCOrgTransactionsPGWDTO == null) ? "TATokenRequest" :
                            if (frmTranType.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                if (frmTranType.TransactionType.Equals("Authorization") || frmTranType.TransactionType.Equals("Sale"))
                                {
                                    if (frmTranType.TransactionType.Equals("Authorization"))
                                    {
                                        adyenRequest.TransactionType = PaymentGatewayTransactionType.AUTHORIZATION;
                                    }
                                    else
                                    {
                                        adyenRequest.TransactionType = PaymentGatewayTransactionType.SALE;
                                    }
                                    if (cCOrgTransactionsPGWDTO != null)
                                    {
                                        adyenRequest.TokenId = cCOrgTransactionsPGWDTO.TokenID;
                                    }
                                }
                                else if (frmTranType.TransactionType.Equals("TATokenRequest"))
                                {
                                    if (CustomerId == -1)
                                    {
                                        throw new Exception("The transaction is not linked with any card.");
                                    }
                                    adyenRequest.CustomerId = CustomerId.ToString();                                    
                                    adyenRequest.TransactionType = PaymentGatewayTransactionType.TATokenRequest;                                    
                                }
                            }
                            else
                            {
                                throw new Exception(utilities.MessageUtils.getMessage("Operation cancelled."));
                            }
                        }
                    }
                }                
                if (isCustomerAllowedToDecideEntryMode)
                {
                    frmEntryMode entryMode = new frmEntryMode();
                    utilities.setLanguage(entryMode);
                    entryMode.ShowDialog();
                    adyenRequest.IsManualEntry = entryMode.IsManual;
                }
                thr.Start();
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1473));
                adyenRequest.Amount = Convert.ToDecimal(transactionPaymentsDTO.Amount);
                adyenRequest.POIID = (isUnattended ? "UX300-" : "P400Plus-") + deviceId;
                adyenRequest.SaleId = transactionPaymentsDTO.TransactionId.ToString();
                adyenRequest.ServiceId = DateTime.Now.ToString("ddHHmmss");
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                adyenRequest.TransactionId = ccRequestPGWDTO.RequestID.ToString();
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                cCTransactionsPGWDTO = adyenCommandHandler.ProcessTransaction(adyenRequest);
                CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                cCTransactionsPGWBL.Save();
                if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode.Equals("Success"))
                {
                    if(Convert.ToDecimal(cCTransactionsPGWDTO.TipAmount)>0)
                    {
                        transactionPaymentsDTO.Amount= Convert.ToDouble(Convert.ToDecimal(cCTransactionsPGWDTO.Authorize) - Convert.ToDecimal(cCTransactionsPGWDTO.TipAmount));
                        transactionPaymentsDTO.TipAmount = Convert.ToDouble(cCTransactionsPGWDTO.TipAmount);
                    }
                    else
                    {
                        transactionPaymentsDTO.Amount = Convert.ToDouble(cCTransactionsPGWDTO.Authorize);
                    }
                    
                    transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.CreditCardAuthorization = cCTransactionsPGWDTO.AuthCode;
                    transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                    transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.RecordNo;
                    transactionPaymentsDTO.Memo = adyenCommandHandler.CustomerReceipt;
                    transactionPaymentsDTO.Memo += adyenCommandHandler.MerchantReceipt;
                    Print(adyenCommandHandler.CustomerReceipt,false);
                    Print(adyenCommandHandler.MerchantReceipt, true);
                }
                else if (cCTransactionsPGWDTO != null && !cCTransactionsPGWDTO.DSIXReturnCode.Equals("Success"))
                {
                    if (!string.IsNullOrEmpty(adyenCommandHandler.CustomerReceipt))
                    {
                        Print(adyenCommandHandler.CustomerReceipt, false);
                    }
                    throw new Exception(string.IsNullOrEmpty(cCTransactionsPGWDTO.TextResponse)?utilities.MessageUtils.getMessage("Transaction failed"):cCTransactionsPGWDTO.TextResponse);
                }
                else
                {
                    throw new Exception("No response!.");
                }
            }
            catch (Exception ex)
            {
                statusDisplayUi.DisplayText("Error occured during payment.");
                log.Error(ex);
                throw ex;                
            }
            finally
            {
                statusDisplayUi.CloseStatusWindow();
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            PrintReceipt = true;
            AdyenRequest adyenRequest = new AdyenRequest();
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            try
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount > 0)
                {
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Clover Payment Gateway");
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Transaction Reversal... ") + utilities.MessageUtils.getMessage(1008));
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                    if (ccOrigTransactionsPGWDTO != null && (Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) != (Convert.ToDecimal(transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount))))

                    {
                        adyenRequest.Amount = Convert.ToDecimal(transactionPaymentsDTO.Amount);
                    }
                    else
                    {
                        adyenRequest.Amount = Convert.ToDecimal(transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount);
                    }
                    adyenRequest.TransactionType = PaymentGatewayTransactionType.REFUND;
                    adyenRequest.POIID = (isUnattended ? "UX300-" : "P400Plus-") + deviceId;
                    adyenRequest.SaleId = transactionPaymentsDTO.TransactionId.ToString();
                    adyenRequest.ServiceId = DateTime.Now.ToString("ddHHmmss");
                    adyenRequest.OriginalResponseId = ccOrigTransactionsPGWDTO.RecordNo;
                    adyenRequest.OriginalTrxnDateTime = ccOrigTransactionsPGWDTO.TransactionDatetime;
                    adyenRequest.TransactionId= cCRequestPGWDTO.RequestID.ToString();
                    if (Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) > adyenRequest.Amount)
                    {
                        adyenRequest.OriginalTransactionId = ccOrigTransactionsPGWDTO.InvoiceNo;//For partial refund
                    }
                    adyenRequest.MerchantAccountName = merchantId;
                    AdyenCommandHandler adyenCommandHandler = new AdyenCommandHandler(userName, password, deviceUrl, environmentType, utilities);
                    cCTransactionsPGWDTO = adyenCommandHandler.ProcessTransaction(adyenRequest);
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                    CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                    cCTransactionsPGWBL.Save();
                    if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode.Equals("Success"))
                    {
                        transactionPaymentsDTO.Amount = Convert.ToDouble(cCTransactionsPGWDTO.Authorize)*0.01;
                        transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardAuthorization = cCTransactionsPGWDTO.AuthCode;
                        transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                        transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.RecordNo;
                        transactionPaymentsDTO.Memo = adyenCommandHandler.CustomerReceipt;
                        transactionPaymentsDTO.Memo += adyenCommandHandler.MerchantReceipt;
                        Print(adyenCommandHandler.CustomerReceipt, false);
                        Print(adyenCommandHandler.MerchantReceipt, true);
                    }
                    else if (cCTransactionsPGWDTO != null && !cCTransactionsPGWDTO.DSIXReturnCode.Equals("Success"))
                    {
                        if (!string.IsNullOrEmpty(adyenCommandHandler.CustomerReceipt))
                        {
                            Print(adyenCommandHandler.CustomerReceipt, false);
                        }
                        throw new Exception(string.IsNullOrEmpty(cCTransactionsPGWDTO.TextResponse) ? utilities.MessageUtils.getMessage("Transaction failed") : cCTransactionsPGWDTO.TextResponse);
                    }
                    else
                    {
                        throw new Exception("No response!.");
                    }
                    log.LogMethodExit(transactionPaymentsDTO);
                }
            }
            catch (Exception ex)
            {
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                log.Error("Error occured while Refunding the Amount", ex);
                log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                log.LogMethodExit(null, "throwing Exception");
                throw ex;
            }
            finally
            {
                statusDisplayUi.CloseStatusWindow();
                CleanUp();
            }
            return transactionPaymentsDTO;
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
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.TATokenRequest.ToString()));
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                {
                    preAuthorizationCCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                }
            }

            log.LogMethodExit(preAuthorizationCCTransactionsPGWDTO);
            return preAuthorizationCCTransactionsPGWDTO;
        }
    }
}
