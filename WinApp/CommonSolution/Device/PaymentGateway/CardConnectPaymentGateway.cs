/********************************************************************************************
 * Project Name - CardConnectPaymentGateway
 * Description  - CardConnectPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value 
 *2.60.0      13-Mar-2019      Raghuveera     Added new parameter to the status ui call
 *2.100.0     20-Aug-2020      Mathew Ninan   Fixed issue with tip amount in reversal scenario 
 *2.100.0     01-Sep-2020      Guru S A       Payment link changes
 *2.100.0     02-Dec-2020      Dakshakh Raj   Entry Modes Cancel button issue fix changes
 *2.120.1     09-Jun-2021      Mathew Ninan   Address validation for only Manual key-in cards 
 *2.130.4     22-Feb-2022      Mathew Ninan   Modified DateTime to ServerDateTime 
 *2.150.1     22-Feb-2023      Guru S A       Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
//using Semnox.Parafait.PaymentGateway;
//using Semnox.Parafait.TransactionPayments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data.SqlClient;
using Semnox.Parafait.Languages; 

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CardConnectPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();
        private string merchantId;
        string gatewayUrl;
        string username;
        string password;
        string deviceUrl;
        string deviceId;
        string authorization;
        string sessionKey;
        bool isAuthEnabled;
        bool isDeviceBeepSoundRequired;
        bool isAddressValidationRequired;
        bool isCustomerAllowedToDecideEntryMode;
        bool isManual;
        bool isDebitProcess = false;
        bool enableDebitPINTrx = false;
        bool isSignatureRequired;
        bool enableAutoAuthorization;
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
            CAPTURE
        }

        public CardConnectPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel   
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
            merchantId = utilities.getParafaitDefaults("CREDIT_CARD_STORE_ID");
            gatewayUrl = utilities.getParafaitDefaults("CREDIT_CARD_HOST_URL");
            username = utilities.getParafaitDefaults("CREDIT_CARD_HOST_USERNAME");
            password = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_HOST_PASSWORD");//utilities.getParafaitDefaults("CREDIT_CARD_HOST_PASSWORD");
            deviceUrl = utilities.getParafaitDefaults("CREDIT_CARD_DEVICE_URL");
            deviceId = utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_ID");
            authorization = utilities.getParafaitDefaults("CREDIT_CARD_TOKEN_ID");
            isAuthEnabled = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
            isDeviceBeepSoundRequired = utilities.getParafaitDefaults("ENABLE_CREDIT_CARD_DEVICE_BEEP_SOUND").Equals("Y");
            isAddressValidationRequired = utilities.getParafaitDefaults("ENABLE_ADDRESS_VALIDATION").Equals("Y");
            isCustomerAllowedToDecideEntryMode = utilities.getParafaitDefaults("ALLOW_CUSTOMER_TO_DECIDE_ENTRY_MODE").Equals("Y");
            isSignatureRequired = !utilities.getParafaitDefaults("ENABLE_SIGNATURE_VERIFICATION").Equals("N");
            enableAutoAuthorization = utilities.getParafaitDefaults("ENABLE_AUTO_CREDITCARD_AUTHORIZATION").Equals("Y");
            enableDebitPINTrx = utilities.getParafaitDefaults("ENABLE_DEBIT_PIN_PAYMENT").Equals("Y");
            isSignatureRequired = (isSignatureRequired) ? !isUnattended : false;
            log.LogVariableState("merchantId", merchantId);
            log.LogVariableState("isUnattended", isUnattended);
            log.LogVariableState("gatewayUrl", gatewayUrl);
            log.LogVariableState("username", username);
            log.LogVariableState("password", password);
            log.LogVariableState("deviceUrl", deviceUrl);
            log.LogVariableState("deviceId", deviceId);
            log.LogVariableState("authorization", isAuthEnabled);
            log.LogVariableState("enableAutoAuthorization", enableAutoAuthorization);
            if (string.IsNullOrEmpty(deviceUrl))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_DEVICE_URL value in configuration."));
            }
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TERMINAL_ID value in configuration."));
            }
            if (string.IsNullOrEmpty(authorization))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TOKEN_ID value in configuration for authorization."));
            }
            if (string.IsNullOrEmpty(gatewayUrl))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_HOST_URL value in configuration."));
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_HOST_USERNAME value in configuration."));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_HOST_PASSWORD value in configuration."));
            }
            if (string.IsNullOrEmpty(merchantId))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Configuration CREDIT_CARD_MERCHANT_ID is not set."));
            }
            try
            {
                CardConnectCommandHandler connectCommand = new ConnectCommandHandler(utilities, null, "", merchantId, deviceUrl, deviceId, authorization);
                connectCommand.CreateCommand(null);
                HttpWebResponse response = connectCommand.Sendcommand();
                sessionKey = connectCommand.GetResponse(response).ToString();
                CardConnectCommandHandler displayCommand = new DisplayCommandHandler(utilities, null, sessionKey, merchantId, deviceUrl, deviceId, authorization);
                DisplayInDevice(displayCommand, "Welcome to " + utilities.ParafaitEnv.POSMachine);

            }
            catch (Exception ex)
            {
                if (writeToLogDelegate != null)
                {
                    writeToLogDelegate(-1, "Load", -1, 0, "Device connection failed on load.", utilities.ParafaitEnv.POSMachineId, utilities.ParafaitEnv.POSMachine);
                }
                log.Fatal("Device connection failed on load. Exception:" + ex.ToString());
                //showMessageDelegate("Device connection failed on load.", "Card connect payment gateway", MessageBoxButtons.OK);
            }
            log.LogMethodExit(null);
        }
        public override void Initialize()
        {
            log.LogMethodEntry();
            CheckLastTransactionStatus();
            log.LogMethodExit();
        }
        public override void SendLastTransactionStatusCheckRequest(CCRequestPGWDTO cCRequestPGWDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            CardConnectCommandHandler displayCommand = null; 
            try
            {
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage("Checking the transaction status" + ((cCRequestPGWDTO != null) ? " of TrxId:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "Card Connect Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                InquireStatusCommandHandler inquireStatusCommand;
                CCTransactionsPGWDTO ccTransactionsPGWDTOResponse = null;
                InquireResponse inquireResponse;
                log.Debug("Genrating the status inquire");
                inquireStatusCommand = new InquireStatusCommandHandler(utilities, gatewayUrl, username, password, merchantId);
                log.Debug("Genrated the status inquire");
                displayCommand = new DisplayCommandHandler(utilities, null, sessionKey, merchantId, deviceUrl, deviceId, authorization);
                log.Debug("Genrated the display command");
                if (cCTransactionsPGWDTO != null)
                {
                    log.Debug("cCTransactionsPGWDTO is not null");
                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOcapturedList = null;
                    if (!string.IsNullOrEmpty(cCTransactionsPGWDTO.RecordNo) && cCTransactionsPGWDTO.RecordNo.Equals("A") && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.REFUND.ToString()) && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.VOID.ToString()))
                    {
                        if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
                        {
                            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTO.ResponseID.ToString()));
                            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                            if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                            {
                                if (cCTransactionsPGWDTOList[0].TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()))
                                {
                                    ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                                    cCTransactionsPGWDTOcapturedList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                                    if (cCTransactionsPGWDTOcapturedList != null && cCTransactionsPGWDTOcapturedList.Count > 0)
                                    {
                                        log.Debug("The authorized transaction is captured.");
                                        return;
                                    }
                                }
                                else if (cCTransactionsPGWDTOList[0].TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                                {
                                    ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                                    cCTransactionsPGWDTOcapturedList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                                    if (cCTransactionsPGWDTOcapturedList != null && cCTransactionsPGWDTOcapturedList.Count > 0)
                                    {
                                        log.Debug("The authorized transaction is adjusted for tip.");
                                        return;
                                    }
                                }
                                transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                                transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                                List<TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);
                                if (transactionPaymentsDTOs == null || transactionPaymentsDTOs.Count == 0)
                                {
                                    cCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                                }
                                else
                                {
                                    log.Debug("The capture/tip adjusted transaction exists for the authorization request with requestId:" + cCTransactionsPGWDTO.InvoiceNo + " and its upto date");
                                    return;
                                }
                            }
                        }
                        else if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()))
                        {
                            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTO.ResponseID.ToString()));
                            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);

                            if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                            {
                                if (cCTransactionsPGWDTOList[0].TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                                {
                                    ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                                    cCTransactionsPGWDTOcapturedList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                                    if (cCTransactionsPGWDTOcapturedList != null && cCTransactionsPGWDTOcapturedList.Count > 0)
                                    {
                                        log.Debug("The captured transaction is adjusted for tip.");
                                        return;
                                    }
                                }
                                transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                                transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                                List<TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);
                                if (transactionPaymentsDTOs == null || transactionPaymentsDTOs.Count == 0)
                                {
                                    cCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                                }
                                else
                                {
                                    log.Debug("The tip adjusted transaction exists for the capture request with requestId:" + cCTransactionsPGWDTO.InvoiceNo + " and its upto date");
                                    return;
                                }
                            }

                        }
                        else if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                        {
                            log.Debug("credit card transaction is tip adjustment.");
                            log.LogMethodExit(true);
                            return;
                        }
                        DisplayInDevice(displayCommand, utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                        inquireStatusCommand.CreateCommand(cCTransactionsPGWDTO);
                        HttpWebResponse response = inquireStatusCommand.Sendcommand();
                        inquireResponse = inquireStatusCommand.GetResponse(response) as InquireResponse;
                        ccTransactionsPGWDTOResponse = inquireStatusCommand.ParseResponse(inquireResponse);
                    }
                    else
                    {
                        log.Debug("credit card transaction done from this POS is not approved.");
                        log.LogMethodExit(true);
                        return;
                    }
                }
                else if (cCRequestPGWDTO != null)
                {
                    log.Debug("cCRequestPGWDTO is not null");
                    DisplayInDevice(displayCommand, utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                    inquireStatusCommand.CreateCommand(cCRequestPGWDTO);
                    HttpWebResponse response = inquireStatusCommand.Sendcommand();
                    inquireResponse = inquireStatusCommand.GetResponse(response) as InquireResponse;
                    ccTransactionsPGWDTOResponse = inquireStatusCommand.ParseResponse(inquireResponse);
                }
                if (ccTransactionsPGWDTOResponse == null)
                {
                    log.Debug("ccTransactionsPGWDTOResponse is null");
                    log.Error("Last transaction status is not available." + ((cCRequestPGWDTO == null) ? "" : " RequestId:" + cCRequestPGWDTO.RequestID + ", Amount:" + cCRequestPGWDTO.POSAmount));//ccrequestId etc
                    return;
                }
                else
                {
                    log.Debug("ccTransactionsPGWDTOResponse is not null");
                    try
                    {
                        log.LogVariableState("ccTransactionsPGWDTOResponse", ccTransactionsPGWDTOResponse);
                        ccTransactionsPGWDTOResponse.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTOResponse.CaptureStatus = (isManual) ? "KEYED" : (string.IsNullOrEmpty(ccTransactionsPGWDTOResponse.CaptureStatus)) ? "SWIPED" : ccTransactionsPGWDTOResponse.CaptureStatus;
                        ccTransactionsPGWDTOResponse.TranCode = "SALE";
                        if (cCTransactionsPGWDTO == null)
                        {
                            log.Debug("Saving ccTransactionsPGWDTOResponse.");
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTOResponse);
                            ccTransactionsPGWBL.Save();
                        }
                        log.LogVariableState("ccTransactionsPGWDTOResponse", ccTransactionsPGWDTOResponse);
                        if (!string.IsNullOrEmpty(ccTransactionsPGWDTOResponse.RefNo) && ccTransactionsPGWDTOResponse.RecordNo.Equals("A"))
                        {
                            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO();
                            try
                            {
                                transactionPaymentsDTO.TransactionId = Convert.ToInt32(cCRequestPGWDTO.InvoiceNo);
                            }
                            catch
                            {
                                log.Debug("Transaction id conversion is failed");
                            }
                            transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWDTOResponse.Authorize);
                            transactionPaymentsDTO.CCResponseId = (cCTransactionsPGWDTO == null) ? ccTransactionsPGWDTOResponse.ResponseID : cCTransactionsPGWDTO.ResponseID;
                            log.LogVariableState("transactionPaymentsDTO", transactionPaymentsDTO);
                            log.Debug("Calling RefundAmount()");
                            if (statusDisplayUi != null)
                            {
                                statusDisplayUi.CloseStatusWindow();
                            }
                            transactionPaymentsDTO = RefundAmount(transactionPaymentsDTO);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception one");
                        if (!isUnattended && showMessageDelegate != null)
                        {
                            showMessageDelegate(utilities.MessageUtils.getMessage("Last transaction status check is failed. :" + ((cCRequestPGWDTO != null) ? " TransactionID:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "Last Transaction Status Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        log.Error("Last transaction check failed", ex);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception two");
                log.Error(ex);
                throw;
            }
            finally
            {
                log.Debug("Reached finally.");
                try
                {
                    log.LogVariableState("displayCommand", displayCommand);
                    log.LogVariableState("statusDisplayUi", statusDisplayUi);
                    if (displayCommand != null)
                        DisplayInDevice(displayCommand, "Welcome to " + utilities.ParafaitEnv.POSMachine);
                    statusDisplayUi.CloseStatusWindow();
                      CardConnectCommandHandler DisconnectCommand = new DisconnectCommandHandler(utilities, null, sessionKey, merchantId, deviceUrl, deviceId, authorization);
                    log.LogVariableState("DisconnectCommand", DisconnectCommand);
                    DisconnectCommand.CreateCommand(null);
                    DisconnectCommand.Sendcommand();
                    log.Debug("Disconnected the device."); 
                }
                catch (Exception ex)
                {
                    log.Debug("Exception three without throw in finally");
                    log.Error(ex); 
                }
            }
            log.LogMethodExit();
        }
        
        private void StatusDisplayUi_CancelClicked(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (statusDisplayUi != null)
            {
                statusDisplayUi.DisplayText("Cancelling...");
            }
            CancellProcess();
            log.LogMethodExit(null);
        }
        private void CancellProcess()
        {
            log.LogMethodEntry();
            CardConnectCommandHandler CancelCommand = new CancelCommandHandler(utilities, null, sessionKey, merchantId, deviceUrl, deviceId, authorization);
            CancelCommand.CreateCommand(null);
            CancelCommand.Sendcommand();
            log.LogMethodExit(null);
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO); 
            VerifyPaymentRequest(transactionPaymentsDTO);
            PrintReceipt = true;
            statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Card Connect Payment Gateway");
            statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
            statusDisplayUi.EnableCancelButton(false);
            isManual = false;
            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
            TransactionType trxType = TransactionType.SALE;
            ReadCardResponse readCardResp = null;
            CCTransactionsPGWDTO cCOrgTransactionsPGWDTO = null;
            CardConnectCommandHandler displayCommand = null;
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
                                frmTransactionTypeUI frmTranType = new frmTransactionTypeUI(utilities, (cCOrgTransactionsPGWDTO == null) ? "TATokenRequest" : "Authorization", transactionPaymentsDTO.Amount, showMessageDelegate);
                                if (frmTranType.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    if (frmTranType.TransactionType.Equals("Authorization") || frmTranType.TransactionType.Equals("Sale"))
                                    {
                                        if (frmTranType.TransactionType.Equals("Authorization"))
                                        {
                                            trxType = TransactionType.AUTHORIZATION;
                                        }
                                        else
                                        {
                                            trxType = TransactionType.SALE;
                                        }
                                        if (cCOrgTransactionsPGWDTO != null)
                                        {
                                            string[] cardData = cCOrgTransactionsPGWDTO.AcqRefData.Split('|');
                                            string[] data;
                                            readCardResp = new ReadCardResponse();
                                            readCardResp.TokenId = cCOrgTransactionsPGWDTO.TokenID;
                                            isManual = cCOrgTransactionsPGWDTO.CaptureStatus.Equals("KEYED");
                                            for (int i = 0; i < cardData.Length; i++)
                                            {
                                                data = cardData[i].Split(':');
                                                if (data[0].Equals("Name"))
                                                {
                                                    readCardResp.Name = data[1];
                                                }
                                                if (data[0].Equals("EXP"))
                                                {
                                                    readCardResp.ExpDate = data[1];
                                                }
                                                if (data[0].Equals("ZIP"))
                                                {
                                                    readCardResp.ZipCode = data[1];
                                                }
                                            }
                                        }
                                    }
                                    else if (frmTranType.TransactionType.Equals("TATokenRequest"))
                                    {
                                        trxType = TransactionType.TATokenRequest;
                                        transactionPaymentsDTO.Amount = 0;
                                        transactionPaymentsDTO.TipAmount = 0;
                                    }
                                }
                                else
                                {
                                    throw new Exception(utilities.MessageUtils.getMessage("Operation cancelled."));
                                }
                            }
                        }
                    }
                }
                thr.Start();
                ReadCardRequest readCardRequest = new ReadCardRequest();
                readCardRequest.IsBeepSoundRequired = isDeviceBeepSoundRequired;
                readCardRequest.IsZipValidationRequired = false;//2.120.1
                readCardRequest.IsSignatureRequired = isSignatureRequired;
                if (trxType.Equals(TransactionType.SALE))
                {
                    readCardRequest.IsCaptureRequired = true;
                }
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                CardConnectCommandHandler connectCommand = new ConnectCommandHandler(utilities, transactionPaymentsDTO, "", merchantId, deviceUrl, deviceId, authorization);
                CardConnectCommandHandler readInput;
                CardConnectCommandHandler readCardCommand;
                AuthorizationResponse authorizationResponse = null;
                CCTransactionsPGWDTO ccTransactionsPGWDTO = null;
                CardConnectCommandHandler performSaleCommand;
                CardConnectCommandHandler binCommand = new BinCommandHandler(utilities, transactionPaymentsDTO, gatewayUrl, username, password, merchantId);
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1473));
                //Get Session key for the payment
                connectCommand.CreateCommand(null);
                HttpWebResponse response = connectCommand.Sendcommand();
                sessionKey = connectCommand.GetResponse(response).ToString();
                displayCommand = new DisplayCommandHandler(utilities, transactionPaymentsDTO, sessionKey, merchantId, deviceUrl, deviceId, authorization);
                DisplayInDevice(displayCommand, "");
                statusDisplayUi.EnableCancelButton(true);
                //Ask for Debit PIN or Credit based payment
                if (enableDebitPINTrx)
                {
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(4874));
                    CardConnectCommandHandler processDebitCommand = new ReadConfirmationCommandHandler(utilities, transactionPaymentsDTO, sessionKey, merchantId, deviceUrl, deviceId, authorization);
                    processDebitCommand.CreateCommand(utilities.MessageUtils.getMessage(4875));
                    response = processDebitCommand.Sendcommand();
                    isDebitProcess = Convert.ToBoolean(processDebitCommand.GetResponse(response));
                }
                if (isCustomerAllowedToDecideEntryMode)
                {
                    if (readCardResp == null)
                    {
                        if (isUnattended)
                        {
                            frmEntryMode entryMode = new frmEntryMode();
                            utilities.setLanguage(entryMode);
                            if (entryMode.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                if (entryMode.IsManual)
                                {
                                    isManual = entryMode.IsManual;
                                }
                            }
                            else
                            {
                                throw new Exception(utilities.MessageUtils.getMessage("Operation cancelled."));
                            }
                        }
                        else
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1474));
                            CardConnectCommandHandler readConfirmCommand = new ReadConfirmationCommandHandler(utilities, transactionPaymentsDTO, sessionKey, merchantId, deviceUrl, deviceId, authorization);
                            readConfirmCommand.CreateCommand(utilities.MessageUtils.getMessage(1475));
                            response = readConfirmCommand.Sendcommand();
                            isManual = Convert.ToBoolean(readConfirmCommand.GetResponse(response));
                        }
                    }
                }
                if (readCardResp == null)
                {
                    statusDisplayUi.EnableCancelButton(true);
                    statusDisplayUi.DisplayText((isManual) ? utilities.MessageUtils.getMessage(1469) : utilities.MessageUtils.getMessage(1470));
                    if (isManual)
                    {
                        readCardRequest.IsZipValidationRequired = isAddressValidationRequired;//2.120.1
                        readCardCommand = new ReadManualCommandHandler(utilities, transactionPaymentsDTO, sessionKey, merchantId, deviceUrl, deviceId, authorization);
                    }
                    else
                    {
                        readCardCommand = new ReadCardCommandHandler(utilities, transactionPaymentsDTO, sessionKey, merchantId, deviceUrl, deviceId, authorization);
                    }
                    if (isDebitProcess)
                    {
                        readCardRequest.aid = "debit";
                        readCardRequest.includePIN = true;
                    }
                    readCardCommand.CreateCommand(readCardRequest);
                    response = readCardCommand.Sendcommand();
                    readCardResp = readCardCommand.GetResponse(response) as ReadCardResponse;
                }
                if (readCardResp != null && !string.IsNullOrEmpty(readCardResp.TokenId))
                {
                    if (isAddressValidationRequired && isManual) //2.120.1
                    {
                        if (string.IsNullOrEmpty(readCardResp.ZipCode))
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1472));
                            readInput = new ReadInputCommandHandler(utilities, transactionPaymentsDTO, sessionKey, merchantId, deviceUrl, deviceId, authorization);
                            ReadInputRequest readInputRequest = new ReadInputRequest();
                            readInputRequest.DisplayMessage = utilities.MessageUtils.getMessage("Zip Code:");
                            readInputRequest.Format = InputType.NUMERIC;
                            readInputRequest.MinLength = 5;
                            readInputRequest.MaxLength = 10;
                            readInput.CreateCommand(readInputRequest);
                            response = readInput.Sendcommand();
                            readCardResp.ZipCode = readInput.GetResponse(response).ToString();
                            if (string.IsNullOrEmpty(readCardResp.ZipCode))
                            {
                                throw new Exception(utilities.MessageUtils.getMessage("In valid zip code."));
                            }
                        }
                    }
                    if (isDebitProcess)
                    {
                        readCardResp.aid = "debit";
                        readCardResp.includePIN = true;
                    }
                    performSaleCommand = new AuthorizationCommandHandler(utilities, transactionPaymentsDTO, gatewayUrl, username, password, merchantId, trxType.Equals(TransactionType.SALE));
                    DisplayInDevice(displayCommand, utilities.MessageUtils.getMessage(1008));
                    statusDisplayUi.EnableCancelButton(false);
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    readCardResp.orderId = ccRequestPGWDTO.RequestID.ToString();
                    performSaleCommand.CreateCommand(readCardResp);
                    response = performSaleCommand.Sendcommand();
                    authorizationResponse = performSaleCommand.GetResponse(response) as AuthorizationResponse;
                    ccTransactionsPGWDTO = performSaleCommand.ParseResponse(authorizationResponse);

                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.CaptureStatus = (isManual) ? "KEYED" : (string.IsNullOrEmpty(ccTransactionsPGWDTO.CaptureStatus)) ? "SWIPED" : ccTransactionsPGWDTO.CaptureStatus;
                    ccTransactionsPGWDTO.TranCode = trxType.ToString();
                    try
                    {
                        binCommand.CreateCommand(readCardResp.TokenId.ToString());
                        response = binCommand.Sendcommand();
                        BinData binData = binCommand.GetResponse(response) as BinData;
                        ccTransactionsPGWDTO.UserTraceData = CCGatewayUtils.GetCCCardTypeByStartingDigit(readCardResp.TokenId.Substring(1));
                        //ccTransactionsPGWDTO.UserTraceData = (binData.Product.Equals("V")) ? "Visa" : (binData.Product.Equals("M")) ? "Mastercard" : (binData.Product.Equals("D")) ? "Discover" : "Unknown";
                        //if (!ccTransactionsPGWDTO.UserTraceData.Equals("Unknown"))
                        //    transactionPaymentsDTO.CreditCardName = ccTransactionsPGWDTO.UserTraceData + (binData.CardUseString.Contains("Debit") ? "_DEBIT" : "");
                        ccTransactionsPGWDTO.CardType = (binData.CardUseString.Contains("Debit") ? "Debit" : "Credit");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Bin command returns an error:" + ex.ToString());
                    }
                    transactionPaymentsDTO.NameOnCreditCard = readCardResp.Name;
                    transactionPaymentsDTO.CreditCardNumber = readCardResp.TokenId.Substring(1, 2) + readCardResp.TokenId.Substring(readCardResp.TokenId.Length - 4).PadLeft(readCardResp.TokenId.Length - 2, 'X');
                    //ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                    log.LogVariableState("authorizationResponse", authorizationResponse);
                }
                else
                {
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Read card failed."));
                    DisplayInDevice(displayCommand, utilities.MessageUtils.getMessage("Read card failed."));
                    throw new Exception(utilities.MessageUtils.getMessage("Read card error."));
                }
                if (ccTransactionsPGWDTO != null)
                {
                    if (readCardResp != null)
                    {
                        ccTransactionsPGWDTO.AcqRefData += "|Name:" + readCardResp.Name + "|EXP:" + readCardResp.ExpDate + "|ZIP:" + readCardResp.ZipCode;
                    }
                    if (cCOrgTransactionsPGWDTO != null)
                    {
                        ccTransactionsPGWDTO.UserTraceData = cCOrgTransactionsPGWDTO.UserTraceData;
                    }
                    if ((string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData) || (!string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData) && ccTransactionsPGWDTO.UserTraceData.Equals("Unknown"))) && ccTransactionsPGWDTO.AcctNo.Substring(0, 1).Equals("3"))
                    {
                        ccTransactionsPGWDTO.UserTraceData = "AMEX";
                    }
                    if (ccTransactionsPGWDTO != null && !trxType.Equals(TransactionType.TATokenRequest))
                    {
                        SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                    }
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();
                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.RefNo) && ccTransactionsPGWDTO.RecordNo.Equals("A"))
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Approved") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CustomerCardProfileId = ccTransactionsPGWDTO.CustomerCardProfileId;
                        transactionPaymentsDTO.CreditCardExpiry = readCardResp.ExpDate;
                        DisplayInDevice(displayCommand, utilities.MessageUtils.getMessage("Approved") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));

                    }
                    else if (ccTransactionsPGWDTO.RecordNo.Equals("B"))
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Please Retry.") + "-" + ccTransactionsPGWDTO.TextResponse);
                        DisplayInDevice(displayCommand, utilities.MessageUtils.getMessage("Please Retry") + "-" + ccTransactionsPGWDTO.TextResponse);
                        throw new Exception(utilities.MessageUtils.getMessage("Please Retry.") + "-" + ccTransactionsPGWDTO.TextResponse);
                    }
                    else if (ccTransactionsPGWDTO.RecordNo.Equals("C"))
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Declined") + "-" + ccTransactionsPGWDTO.TextResponse);
                        DisplayInDevice(displayCommand, utilities.MessageUtils.getMessage("Declined.") + "-" + ccTransactionsPGWDTO.TextResponse);
                        //if (ccTransactionsPGWDTO != null)
                        //{
                        //    SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                        //}
                        throw new Exception(utilities.MessageUtils.getMessage("Declined.") + "-" + ccTransactionsPGWDTO.TextResponse);
                    }
                    else
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Payment Not complete"));
                        DisplayInDevice(displayCommand, utilities.MessageUtils.getMessage("Payment Not complete"));
                        throw new Exception(utilities.MessageUtils.getMessage("Payment Not complete"));
                    }
                }
                else
                {
                    throw new Exception("ccTransactionsPGWDTO not set.");
                }

                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;

            }
            catch (Exception ex)
            {

                statusDisplayUi.DisplayText((ex.ToString().Contains("500")) ? utilities.MessageUtils.getMessage(1476) : ex.Message);
                CancellProcess();
                log.Error("Error occured while making payments", ex);
                log.Fatal("Ends-MakePayment()  Exception:" + ex.ToString());
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw ((ex.ToString().Contains("500")) ? new Exception(utilities.MessageUtils.getMessage(1476)) : ex);
            }
            finally
            {
                try
                {
                    if (displayCommand != null)
                        DisplayInDevice(displayCommand, "Welcome to " + utilities.ParafaitEnv.POSMachine);
                    log.Debug("Ends-MakePayment() disconnecting...");
                    CardConnectCommandHandler DisconnectCommand = new DisconnectCommandHandler(utilities, null, sessionKey, merchantId, deviceUrl, deviceId, authorization);
                    DisconnectCommand.CreateCommand(null);
                    DisconnectCommand.Sendcommand();
                }
                catch (Exception ex)
                {
                    log.Fatal("Ends-MakePayment()  Disconnection failed with Exception:" + ex.ToString());
                }
                statusDisplayUi.CloseStatusWindow();
                sessionKey = "";
                //statusUi.Dispose();
            }
        }
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO); 
            try
            {
                PrintReceipt = true;
                CCTransactionsPGWDTO ccTransactionsPGWDTO = null;
                InquireStatusCommandHandler inquireStatusCommand;
                InquireResponse inquireResponse;
                if (transactionPaymentsDTO != null)//&& transactionPaymentsDTO.Amount > 0)
                {
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(4202, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Card Connect Payment Gateway");
                    statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                    double amount = (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount);
                    inquireStatusCommand = new InquireStatusCommandHandler(utilities, gatewayUrl, username, password, merchantId);
                    inquireStatusCommand.CreateCommand(ccOrigTransactionsPGWDTO);
                    HttpWebResponse inquireresponse = inquireStatusCommand.Sendcommand();
                    inquireResponse = inquireStatusCommand.GetResponse(inquireresponse) as InquireResponse;
                    //ccTransactionsPGWDTOResponse = inquireStatusCommand.ParseResponse(inquireResponse);


                    //if (ccOrigTransactionsPGWDTO.TransactionDatetime.AddMinutes(20).CompareTo(DateTime.Now) >= 0 &&((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize) == amount) || ccOrigTransactionsPGWDTO.TranCode.Equals(TransactionType.AUTHORIZATION)))//void transaction of authorized or full reverse

                    if (inquireResponse != null && inquireResponse.Refundable.Equals("Y") && !isUnattended)//Refund
                    {
                        if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) > Convert.ToDecimal(amount)))
                        {
                            transactionPaymentsDTO.TipAmount = 0.0;
                        }
                        CardConnectCommandHandler refundCommand = new RefundCommandHandler(utilities, transactionPaymentsDTO, gatewayUrl, username, password, merchantId);
                        refundCommand.CreateCommand(ccOrigTransactionsPGWDTO);
                        HttpWebResponse response = refundCommand.Sendcommand();
                        RefundResponse refundResp = refundCommand.GetResponse(response) as RefundResponse;
                        ccTransactionsPGWDTO = refundCommand.ParseResponse(refundResp);
                        ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                        ccTransactionsPGWDTO.UserTraceData = ccOrigTransactionsPGWDTO.UserTraceData;
                        ccTransactionsPGWDTO.AcctNo = ccOrigTransactionsPGWDTO.AcctNo;
                        if (string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData) && !string.IsNullOrEmpty(ccTransactionsPGWDTO.AcctNo) && ccTransactionsPGWDTO.AcctNo.Substring(0, 1).Equals("3"))
                        {
                            ccTransactionsPGWDTO.UserTraceData = "AMEX";
                        }
                        ccTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                        ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                        ccTransactionsPGWDTO.CaptureStatus = ccOrigTransactionsPGWDTO.CaptureStatus;
                        if (ccTransactionsPGWDTO != null)
                        {
                            SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                        }
                        ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                        //transactionPaymentsDTO.TipAmount = 0;//This is not required. Tip amount should go from cardConnect
                        log.LogVariableState("refundResp", refundResp);
                        if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.RefNo) && ccTransactionsPGWDTO.RecordNo.Equals("A"))
                        {
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.CreditCardNumber = ccOrigTransactionsPGWDTO.TokenID.Substring(1, 2) + ccOrigTransactionsPGWDTO.TokenID.Substring(ccOrigTransactionsPGWDTO.TokenID.Length - 4).PadLeft(ccOrigTransactionsPGWDTO.TokenID.Length - 2, 'X');
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Approved") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                            //if (ccTransactionsPGWDTO != null)
                            //{
                            //    SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                            //}
                        }
                        else if (ccTransactionsPGWDTO.RecordNo.Equals("B"))
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Please Retry") + "-" + ccTransactionsPGWDTO.TextResponse);
                            throw new Exception(utilities.MessageUtils.getMessage("Please Retry.") + "-" + ccTransactionsPGWDTO.TextResponse);
                        }
                        else if (ccTransactionsPGWDTO.RecordNo.Equals("C"))
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Declined") + "-" + ccTransactionsPGWDTO.TextResponse);
                            //if (ccTransactionsPGWDTO != null)
                            //{
                            //    SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                            //}
                            throw new Exception(utilities.MessageUtils.getMessage("Declined.") + "-" + ccTransactionsPGWDTO.TextResponse);
                        }
                        else
                        {
                            log.Error("Payment Not complete");
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Payment Not complete"));
                            throw new Exception(utilities.MessageUtils.getMessage("Payment Not complete"));
                        }
                    }
                    else if (inquireResponse != null && inquireResponse.Voidable.Equals("Y"))
                    {
                        log.Debug("ccOrigTransactionsPGWDTO.Authorize:" + ccOrigTransactionsPGWDTO.Authorize + ", amount:" + amount);
                        //if (inquireResponse.Setlstat.Equals("Queued for Capture") && ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) != Convert.ToDecimal(amount))))//This is the valid condition is we check voidable first.
                        //{
                        //    log.Error("Batch is not settled. So currently partial refund is not possible. Please wait for the batch to settle.");//Batch is not settled
                        //    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Batch is not settled. So currently partial refund is not possible. Please wait for the batch to settle."));
                        //    throw new Exception(utilities.MessageUtils.getMessage("Batch is not settled. So currently partial refund is not possible. Please wait for the batch to settle."));
                        //}
                        if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) != Convert.ToDecimal(amount)))//This is the valid condition if we check refundable first. Because once it enters to this section it will do full reversal
                        {
                            log.Error("Partial Void is not possible. Please wait for the batch to settle.");//Batch is not settled
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Partial Void is not possible. Please wait for the batch to settle."));
                            throw new Exception(utilities.MessageUtils.getMessage("Partial Void is not possible. Please wait for the batch to settle."));
                        }
                        CardConnectCommandHandler voidCommand = new VoidCommandHandler(utilities, transactionPaymentsDTO, gatewayUrl, username, password, merchantId);
                        voidCommand.CreateCommand(ccOrigTransactionsPGWDTO);
                        HttpWebResponse response = voidCommand.Sendcommand();
                        VoidResponse voidResp = voidCommand.GetResponse(response) as VoidResponse;
                        ccTransactionsPGWDTO = voidCommand.ParseResponse(voidResp);
                        ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                        ccTransactionsPGWDTO.UserTraceData = ccOrigTransactionsPGWDTO.UserTraceData;
                        ccTransactionsPGWDTO.AcctNo = ccOrigTransactionsPGWDTO.AcctNo;
                        if (string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData) && !string.IsNullOrEmpty(ccTransactionsPGWDTO.AcctNo) && ccTransactionsPGWDTO.AcctNo.Substring(0, 1).Equals("3"))
                        {
                            ccTransactionsPGWDTO.UserTraceData = "AMEX";
                        }
                        ccTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                        ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                        ccTransactionsPGWDTO.CaptureStatus = ccOrigTransactionsPGWDTO.CaptureStatus;
                        if (ccTransactionsPGWDTO != null)
                        {
                            SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                        }
                        ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        log.LogVariableState("voidResp", voidResp);
                        if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.RefNo) && ccTransactionsPGWDTO.RecordNo.Equals("A"))
                        {
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.CreditCardNumber = ccOrigTransactionsPGWDTO.TokenID.Substring(1, 2) + ccOrigTransactionsPGWDTO.TokenID.Substring(ccOrigTransactionsPGWDTO.TokenID.Length - 4).PadLeft(ccOrigTransactionsPGWDTO.TokenID.Length - 2, 'X');
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Approved") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                            //if (ccTransactionsPGWDTO != null)
                            //{
                            //    SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                            //}
                        }
                        else if (ccTransactionsPGWDTO.RecordNo.Equals("B"))
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Please Retry") + "-" + ccTransactionsPGWDTO.TextResponse);
                            throw new Exception(utilities.MessageUtils.getMessage("Please Retry.") + "-" + ccTransactionsPGWDTO.TextResponse);
                        }
                        else if (ccTransactionsPGWDTO.RecordNo.Equals("C"))
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Declined") + "-" + ccTransactionsPGWDTO.TextResponse);
                            //if (ccTransactionsPGWDTO != null)
                            //{
                            //    SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                            //}
                            throw new Exception(utilities.MessageUtils.getMessage("Declined.") + "-" + ccTransactionsPGWDTO.TextResponse);
                        }
                        else
                        {
                            log.Error("Payment Not complete");
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Payment Not complete"));
                            throw new Exception(utilities.MessageUtils.getMessage("Payment Not complete"));
                        }
                    }
                    else
                    {
                        log.Error("Refund of this transaction is not possible.Please contact gateway provider for more information.");
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Refund of this transaction is not possible.Please contact gateway provider for more information."));
                        throw new Exception(utilities.MessageUtils.getMessage("Refund of this transaction is not possible. Please contact gateway provider for more information."));
                    }
                }

                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                statusDisplayUi.DisplayText("Error occured while Refunding the Amount");
                log.Error("Error occured while Refunding the Amount", ex);
                log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                log.LogMethodExit(null, "throwing Exception");
                throw ex;
            }
            finally
            {
                statusDisplayUi.CloseStatusWindow(); 
            }
        }
        public void Print(string printText)
        {
            log.LogMethodEntry(printText);
            try
            {
                using (System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument())
                {
                    pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 300, 700);

                    pd.PrintPage += (sender, e) =>
                    {
                        Font f = new Font("Arial", 9);
                        e.Graphics.DrawString(printText, f, new SolidBrush(Color.Black), new RectangleF(0, 0, pd.DefaultPageSettings.PrintableArea.Width, pd.DefaultPageSettings.PrintableArea.Height));
                    };
                    pd.Print();
                }
            }
            catch (Exception ex)
            {
                utilities.EventLog.logEvent("PaymentGateway", 'I', "Receipt print failed.", printText, this.GetType().Name, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                log.Error("Error in printing cc receipt" + printText, ex);
            }
            log.LogMethodExit(null);
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
        public override bool IsTipAllowed(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            bool isAllowed = false;
            log.LogMethodEntry();
            if (isAuthEnabled)
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.CCResponseId != -1)
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.AUTHORIZATION.ToString()));
                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                    if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                    {
                        isAllowed = true;
                    }
                    if (!isAllowed)
                    {
                        searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));
                        searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.CAPTURE.ToString()));
                        cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                        if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                        {
                            isAllowed = true;
                        }
                    }
                    if (!isAllowed)
                    {
                        searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));
                        searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.TIPADJUST.ToString()));
                        cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                        if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                        {
                            isAllowed = true;
                        }
                    }
                }
            }
            log.LogMethodExit(isAllowed);
            return isAllowed;
        }
        private void SendPrintReceiptRequest(TransactionPaymentsDTO transactionPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO)
        {
            if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
            {
                transactionPaymentsDTO.Memo = GetReceiptText(transactionPaymentsDTO, ccTransactionsPGWDTO, false);
            }
            if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y" && !isUnattended)
            {
                transactionPaymentsDTO.Memo += GetReceiptText(transactionPaymentsDTO, ccTransactionsPGWDTO, true);
            }
        }
        private string GetReceiptText(TransactionPaymentsDTO trxPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO, bool IsMerchantCopy)
        {
            log.LogMethodEntry(trxPaymentsDTO, ccTransactionsPGWDTO, IsMerchantCopy);
            try
            {
                string[] addressArray = utilities.ParafaitEnv.SiteAddress.Split(',');
                string receiptText = "";
                receiptText += AllignText(utilities.ParafaitEnv.SiteName, Alignment.Center);
                if (addressArray != null && addressArray.Length > 0)
                {
                    for (int i = 0; i < addressArray.Length; i++)
                    {
                        receiptText += Environment.NewLine + AllignText(addressArray[i] + ((i != addressArray.Length - 1) ? "," : ""), Alignment.Center);
                    }
                }
                receiptText += Environment.NewLine;
                string maskedMerchantId = (new String('X', 8) + ((merchantId.Length > 4) ? merchantId.Substring(merchantId.Length - 4)
                                                                                         : merchantId));
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Merchant ID") + "     : ".PadLeft(12) + maskedMerchantId, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Date") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TransactionDatetime.ToString("MMM dd yyyy HH:mm"), Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Type") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TranCode, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Trx Id") + "  : ".PadLeft(22) + "@invoiceNo", Alignment.Left);
                object userName = utilities.executeScalar(@"select username + case when EmpLastName is null OR EmpLastName = '' then '' else ' ' + substring(EmpLastName, 1, 1) end 
                                                              from users u
                                                             where user_id = @userId", new SqlParameter("@userId", utilities.ExecutionContext.UserPKId));
                if (userName != null && userName != DBNull.Value)
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Cashier Name") + "  : ".PadLeft(8) + userName.ToString(), Alignment.Left);
                }
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.AuthCode))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Authorization") + "   : ".PadLeft(10) + ccTransactionsPGWDTO.AuthCode, Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Card Type") + "       : ".PadLeft(15) + ccTransactionsPGWDTO.UserTraceData, Alignment.Left);
                //receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Cardholder Name") + ": ".PadLeft(3) + trxPaymentsDTO.NameOnCreditCard, Alignment.Left);
                string maskedPAN = ((string.IsNullOrEmpty(trxPaymentsDTO.CreditCardNumber) ? trxPaymentsDTO.CreditCardNumber
                                                                             : (new String('X', 12) + ((trxPaymentsDTO.CreditCardNumber.Length > 4) 
                                                                                                     ? trxPaymentsDTO.CreditCardNumber.Substring(trxPaymentsDTO.CreditCardNumber.Length - 4)
                                                                                                     : trxPaymentsDTO.CreditCardNumber))));
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("PAN") + ": ".PadLeft(24) + maskedPAN, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Entry Mode") + ": ".PadLeft(13) + ccTransactionsPGWDTO.CaptureStatus, Alignment.Left);

                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.AcqRefData))
                {
                    string[] data = null;
                    string[] emvData = ccTransactionsPGWDTO.AcqRefData.Split('|');
                    for (int i = 0; i < emvData.Length; i++)
                    {
                        data = emvData[i].Split(':');
                        if (data[0].Equals("AID") && !string.IsNullOrEmpty(data[1]))
                        {
                            receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("AID") + ": ".PadLeft(25) + data[1], Alignment.Left);
                        }
                        else if (data[0].Equals("TVR") && !string.IsNullOrEmpty(data[1]))
                        {
                            receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("TVR") + ": ".PadLeft(25) + data[1], Alignment.Left);
                        }
                        else if (data[0].Equals("IAD") && !string.IsNullOrEmpty(data[1]))
                        {
                            receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("IAD") + ": ".PadLeft(25) + data[1], Alignment.Left);
                        }
                    }
                }
                //receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("POS User") + ": ".PadLeft(15) + utilities.ExecutionContext.UserId, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage((ccTransactionsPGWDTO.RecordNo.Equals("A")) ? "APPROVED" : (ccTransactionsPGWDTO.RecordNo.Equals("B")) ? "RETRY" : "DECLINED") + "-" + ccTransactionsPGWDTO.DSIXReturnCode, Alignment.Center);
                if (ccTransactionsPGWDTO.RecordNo.Equals("C"))
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage(ccTransactionsPGWDTO.TextResponse), Alignment.Center);
                }
                receiptText += Environment.NewLine;
                if (ccTransactionsPGWDTO.TranCode.Equals(TransactionType.CAPTURE.ToString()) || ccTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Amount") + "  : " + Convert.ToDouble(trxPaymentsDTO.Amount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    if (!string.IsNullOrWhiteSpace(trxPaymentsDTO.ExternalSourceReference) && trxPaymentsDTO.ExternalSourceReference == "G")
                    {
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Gratuity has already been included."), Alignment.Center);
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Additional Tip") + ": ".PadLeft(28 - utilities.MessageUtils.getMessage("Additional Tip").Length) + "_____________", Alignment.Left);
                    }
                    else
                    {
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Tip") + ": ".PadLeft(35 - utilities.MessageUtils.getMessage("Tip").Length) + Convert.ToDouble(trxPaymentsDTO.TipAmount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    }
                }
                if (ccTransactionsPGWDTO.TranCode.Equals(TransactionType.AUTHORIZATION.ToString()))
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Amount") + " : " + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    //receiptText += Environment.NewLine;
                }
                else
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                           : " + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    receiptText += Environment.NewLine;
                }
                if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.RecordNo) && ccTransactionsPGWDTO.RecordNo.Equals("A")) && ccTransactionsPGWDTO.TranCode.ToUpper().Equals(TransactionType.AUTHORIZATION.ToString()))
                {
                    receiptText += Environment.NewLine;
                    if (!string.IsNullOrWhiteSpace(trxPaymentsDTO.ExternalSourceReference) && trxPaymentsDTO.ExternalSourceReference == "G")
                    {
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Gratuity has already been included."), Alignment.Center);
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Additional Tip") + ": ".PadLeft(28 - utilities.MessageUtils.getMessage("Additional Tip").Length) + "_____________", Alignment.Left);
                    }
                    else
                    {
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Tip") + ": ".PadLeft(34 - utilities.MessageUtils.getMessage("Tip").Length) + "_____________", Alignment.Left);
                    }
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                          : " + "_____________", Alignment.Left);
                }
                receiptText += Environment.NewLine;
                if (IsMerchantCopy)
                {
                    if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.RecordNo) && ccTransactionsPGWDTO.RecordNo.Equals("A")))
                    {
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText("_______________________", Alignment.Center);
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Signature"), Alignment.Center);
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage(1180), Alignment.Center);
                        //}
                        if (ccTransactionsPGWDTO.TranCode.Equals(TransactionType.AUTHORIZATION.ToString()))
                        {
                            LookupValuesDTO lookupValuesDTO = GetLookupValues("ADDITIONAL_PRINT_FIELDS", "@SuggestiveTipText");
                            if (!string.IsNullOrEmpty(lookupValuesDTO.Description))
                            {
                                receiptText += Environment.NewLine;
                                receiptText += Environment.NewLine + AllignText(lookupValuesDTO.Description, Alignment.Center);
                            }
                            lookupValuesDTO = GetLookupValues("ADDITIONAL_PRINT_FIELDS", "@SuggestiveTipValues");
                            if (!string.IsNullOrEmpty(lookupValuesDTO.Description))
                            {
                                string[] tipPercentage = lookupValuesDTO.Description.Split('|');
                                string line = "";
                                foreach (string s in tipPercentage)
                                {
                                    if (!string.IsNullOrEmpty(s))
                                    {
                                        line = s + utilities.MessageUtils.getMessage("% is") + " " + (((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(overallTransactionAmount)) * (int.Parse(s) / 100.0)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                        receiptText += Environment.NewLine + AllignText(line, Alignment.Center);
                                    }
                                }
                                receiptText += Environment.NewLine;
                            }
                        }
                    }
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText("**" + utilities.MessageUtils.getMessage("Merchant Copy") + "**", Alignment.Center);
                }
                else
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("IMPORTANT— retain this copy for your records"), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText("**" + utilities.MessageUtils.getMessage("Cardholder Copy") + " **", Alignment.Center);
                }

                receiptText += Environment.NewLine;
                receiptText += AllignText(" " + utilities.MessageUtils.getMessage("Thank You"), Alignment.Center);
                if ((!ccTransactionsPGWDTO.TranCode.Equals("CAPTURE") || (ccTransactionsPGWDTO.TranCode.Equals("CAPTURE") && IsMerchantCopy && PrintReceipt)))
                {
                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.RefNo) && ccTransactionsPGWDTO.RecordNo.Equals("A"))
                    {
                        if (IsMerchantCopy)
                        {
                            ccTransactionsPGWDTO.MerchantCopy = receiptText;
                        }
                        else
                        {
                            ccTransactionsPGWDTO.CustomerCopy = receiptText;
                        }

                    }
                    else
                    {
                        receiptText = receiptText.Replace("@invoiceNo", "");
                        Print(receiptText);
                    }
                }
                log.LogMethodExit(receiptText);
                return receiptText;
            }
            catch (Exception ex)
            {
                log.Fatal("GetReceiptText() failed to print receipt exception:" + ex.ToString());
                return null;
            }
        }
        public static string AllignText(string text, Alignment align)
        {
            log.LogMethodEntry(text, align);

            int pageWidth = 40;
            string res;
            if (align.Equals(Alignment.Right))
            {
                string returnValueNew = text.PadLeft(pageWidth, ' ');
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            else if (align.Equals(Alignment.Center))
            {
                int len = (pageWidth - text.Length);
                int len2 = len / 2;
                len2 = len2 + text.Length;
                res = text.PadLeft(len2);
                if (res.Length > pageWidth && res.Length > text.Length)
                {
                    res = res.Substring(res.Length - pageWidth);
                }

                log.LogMethodExit(res);
                return res;
            }
            else
            {
                //res= text.PadLeft(5 + text.Length);  
                log.LogMethodExit(text);
                return text;
            }
        }
        private void DisplayInDevice(CardConnectCommandHandler displayCommand, string message)
        {
            log.LogMethodEntry(displayCommand, message);
            if (displayCommand != null)
            {
                displayCommand.CreateCommand(message);
                displayCommand.Sendcommand();
            }
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Returns boolean based on whether payment requires a settlement to be done.
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override bool IsSettlementPending(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            bool returnValue = false;
            if (isAuthEnabled)
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.CCResponseId != -1)
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, TransactionType.AUTHORIZATION.ToString()));
                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                    if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                    {
                        returnValue = true;
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        public override TransactionPaymentsDTO PerformSettlement(TransactionPaymentsDTO transactionPaymentsDTO, bool IsForcedSettlement = false)
        {
            log.LogMethodEntry(transactionPaymentsDTO, IsForcedSettlement);
            try
            {
                CCTransactionsPGWDTO ccTransactionsPGWDTO = null;
                InquireStatusCommandHandler inquireStatusCommand;
                InquireResponse inquireResponse;
                if (transactionPaymentsDTO != null)
                {
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                    inquireStatusCommand = new InquireStatusCommandHandler(utilities, gatewayUrl, username, password, merchantId);
                    inquireStatusCommand.CreateCommand(ccOrigTransactionsPGWDTO);
                    HttpWebResponse inquireresponse = inquireStatusCommand.Sendcommand();
                    inquireResponse = inquireStatusCommand.GetResponse(inquireresponse) as InquireResponse;
                    //ccTransactionsPGWDTOResponse = inquireStatusCommand.ParseResponse(inquireResponse);
                    if (!inquireResponse.Setlstat.Equals("Authorized") && !inquireResponse.Setlstat.Equals("Queued for Capture"))
                    {
                        log.LogMethodExit("No further adjustment is possible on this transaction.");
                        throw new Exception(utilities.MessageUtils.getMessage("No further adjustment is possible on this transaction."));
                    }
                    if (!IsForcedSettlement)//2017-09-27
                    {
                        //frmTransactionTypeUI frmTranType = new frmTransactionTypeUI(utilities, "Completion", transactionPaymentsDTO.Amount, showMessageDelegate);
                        //if (frmTranType.ShowDialog() != DialogResult.Cancel)
                        frmFinalizeTransaction frmFinalizeTransaction = new frmFinalizeTransaction(utilities, overallTransactionAmount, overallTipAmountEntered, Convert.ToDecimal(transactionPaymentsDTO.Amount), Convert.ToDecimal(transactionPaymentsDTO.TipAmount), transactionPaymentsDTO.CreditCardNumber, showMessageDelegate);
                        if (frmFinalizeTransaction.ShowDialog() != DialogResult.Cancel
                            && (transactionPaymentsDTO.TipAmount != Convert.ToDouble(frmFinalizeTransaction.TipAmount)
                                || inquireResponse.Setlstat.Equals("Authorized")
                                || (inquireResponse.Setlstat.Equals("Queued for Capture") && string.IsNullOrWhiteSpace(transactionPaymentsDTO.CustomerCardProfileId) == false)
                               ))
                        {
                            transactionPaymentsDTO.TipAmount = Convert.ToDouble(frmFinalizeTransaction.TipAmount);
                            //approverId= frmFinalizeTransaction.ManagerId;
                        }
                        else
                        {
                            log.LogMethodExit(transactionPaymentsDTO);
                            //return transactionPaymentsDTO;
                            throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                        }
                    }//2017-09-27
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Card Connect Payment Gateway");
                    statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    CardConnectCommandHandler captureCommand = new CaptureCommandHandler(utilities, transactionPaymentsDTO, gatewayUrl, username, password, merchantId);
                    captureCommand.CreateCommand(ccOrigTransactionsPGWDTO);
                    HttpWebResponse response = captureCommand.Sendcommand();
                    CaptureData captureResp = captureCommand.GetResponse(response) as CaptureData;
                    ccTransactionsPGWDTO = captureCommand.ParseResponse(captureResp);
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    if (ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()))
                    {
                        ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.TIPADJUST.ToString();
                    }
                    else
                    {
                        ccTransactionsPGWDTO.TranCode = TransactionType.CAPTURE.ToString();
                    }
                    ccTransactionsPGWDTO.UserTraceData = ccOrigTransactionsPGWDTO.UserTraceData;
                    ccTransactionsPGWDTO.AcctNo = ccOrigTransactionsPGWDTO.AcctNo;
                    if (string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData) && !string.IsNullOrEmpty(ccTransactionsPGWDTO.AcctNo) && ccTransactionsPGWDTO.AcctNo.Substring(0, 1).Equals("3"))
                    {
                        ccTransactionsPGWDTO.UserTraceData = "AMEX";
                    }
                    ccTransactionsPGWDTO.AuthCode = ccOrigTransactionsPGWDTO.AuthCode;
                    ccTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                    ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                    ccTransactionsPGWDTO.CaptureStatus = ccOrigTransactionsPGWDTO.CaptureStatus;
                    ccTransactionsPGWDTO.TipAmount = transactionPaymentsDTO.TipAmount.ToString();
                    ccTransactionsPGWDTO.AcqRefData = ccOrigTransactionsPGWDTO.AcqRefData;
                    if (ccTransactionsPGWDTO != null)
                    {
                        SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                    }
                    ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();
                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                    log.LogVariableState("captureResp", captureResp);
                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.RefNo) && ccTransactionsPGWDTO.RecordNo.Equals("A"))
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Approved") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                        //if (ccTransactionsPGWDTO != null)
                        //{
                        //    SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                        //}
                    }
                    else if (ccTransactionsPGWDTO.RecordNo.Equals("B"))
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Please Retry") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                        throw new Exception(utilities.MessageUtils.getMessage("Please Retry.") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                    }
                    else if (ccTransactionsPGWDTO.RecordNo.Equals("C"))
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Declined") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                        //if (ccTransactionsPGWDTO != null)
                        //{
                        //    SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                        //}
                        throw new Exception(utilities.MessageUtils.getMessage("Declined.") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                    }
                    else
                    {
                        statusDisplayUi.DisplayText("Payment Not complete");
                        throw new Exception(utilities.MessageUtils.getMessage("Payment Not complete"));
                    }
                }
                else
                {
                    statusDisplayUi.DisplayText("Invalid payment data.");
                    throw new Exception(utilities.MessageUtils.getMessage("Invalid payment data."));
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                if (statusDisplayUi != null)
                    statusDisplayUi.DisplayText("Error occured while performing settlement");
                log.Error("Error occured while performing settlement", ex);
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw (ex);
            }
            finally
            {
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }
        }
        public override TransactionPaymentsDTO PayTip(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            transactionPaymentsDTO = PerformSettlement(transactionPaymentsDTO, true);
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
        /// <summary>
        /// Returns list of CCTransactionsPGWDTO's  pending for settelement. 
        /// </summary>
        /// <returns></returns>
        public override List<CCTransactionsPGWDTO> GetAllUnsettledCreditCardTransactions()
        {
            log.LogMethodEntry();

            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = null;
            CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, TransactionType.AUTHORIZATION.ToString()));
            cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);

            log.LogMethodExit(cCTransactionsPGWDTOList);
            return cCTransactionsPGWDTOList;
        }
        /// <summary>
        /// Make Payment For Recurring Billing only
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO MakePaymentForRecurringBilling(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                PrintReceipt = false;
                CCTransactionsPGWDTO ccTransactionsPGWDTO = null;
                if (transactionPaymentsDTO != null)
                {
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    ReadCardResponse readCardResp = new ReadCardResponse();
                    readCardResp.Name = transactionPaymentsDTO.NameOnCreditCard;
                    readCardResp.ExpDate = transactionPaymentsDTO.CreditCardExpiry;
                    isManual = false;
                    CardConnectCommandHandler authorizCommand = new AuthorizationCommandHandler(utilities, transactionPaymentsDTO, gatewayUrl, username, password, merchantId, false);
                    authorizCommand.CreateCommand(readCardResp);
                    HttpWebResponse response = authorizCommand.Sendcommand();

                    AuthorizationResponse authorizationResponse = authorizCommand.GetResponse(response) as AuthorizationResponse;
                    ccTransactionsPGWDTO = authorizCommand.ParseResponse(authorizationResponse);
                    ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.TranCode = TransactionType.SALE.ToString();
                    readCardResp.TokenId = ccTransactionsPGWDTO.TokenID;

                    try
                    {
                        CardConnectCommandHandler binCommand = new BinCommandHandler(utilities, transactionPaymentsDTO, gatewayUrl, username, password, merchantId);
                        binCommand.CreateCommand(readCardResp.TokenId.ToString());
                        response = binCommand.Sendcommand();
                        BinData binData = binCommand.GetResponse(response) as BinData;
                        ccTransactionsPGWDTO.UserTraceData = CCGatewayUtils.GetCCCardTypeByStartingDigit(readCardResp.TokenId.Substring(1));
                        ccTransactionsPGWDTO.CardType = (binData.CardUseString.Contains("Debit") ? "Debit" : "Credit");
                    }
                    catch (Exception ex)
                    {
                        log.Error(authorizationResponse);
                        log.Error("Bin command returns an error:", ex);
                    }
                    transactionPaymentsDTO.NameOnCreditCard = readCardResp.Name;
                    transactionPaymentsDTO.CreditCardNumber = readCardResp.TokenId.Substring(1, 2) + readCardResp.TokenId.Substring(readCardResp.TokenId.Length - 4).PadLeft(readCardResp.TokenId.Length - 2, 'X');
                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                    log.LogVariableState("authorizationResponse", authorizationResponse);
                    //ccTransactionsPGWDTO.CaptureStatus = (isManual) ? "KEYED" : (string.IsNullOrEmpty(ccTransactionsPGWDTO.CaptureStatus)) ? "SWIPED" : ccTransactionsPGWDTO.CaptureStatus;
                    if (readCardResp != null)
                    {
                        ccTransactionsPGWDTO.AcqRefData += "|Name:" + readCardResp.Name + "|EXP:" + readCardResp.ExpDate + "|ZIP:" + readCardResp.ZipCode;
                    }
                    if ((string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData) || (!string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData) && ccTransactionsPGWDTO.UserTraceData.Equals("Unknown"))) && ccTransactionsPGWDTO.AcctNo.Substring(0, 1).Equals("3"))
                    {
                        ccTransactionsPGWDTO.UserTraceData = "AMEX";
                    }
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();

                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.RefNo) && ccTransactionsPGWDTO.RecordNo.Equals("A"))
                    {
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CustomerCardProfileId = ccTransactionsPGWDTO.CustomerCardProfileId;
                    }
                    else if (ccTransactionsPGWDTO.RecordNo.Equals("B"))
                    {
                        throw new Exception(utilities.MessageUtils.getMessage("Please Retry.") + "-" + ccTransactionsPGWDTO.TextResponse);
                    }
                    else if (ccTransactionsPGWDTO.RecordNo.Equals("C"))
                    {
                        throw new Exception(utilities.MessageUtils.getMessage("Declined.") + "-" + ccTransactionsPGWDTO.TextResponse);
                    }
                    else
                    {
                        throw new Exception(utilities.MessageUtils.getMessage("Payment Not complete"));
                    }
                }

                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while charging the customer card", ex);
                log.LogMethodExit(null, "throwing Exception");
                throw ex;
            }
        }

        /// <summary>
        /// GetCreditCardExpiryMonth
        /// </summary>
        /// <param name="cardExpiryData"></param>
        public override int GetCreditCardExpiryMonth(string cardExpiryData)
        {
            log.LogMethodEntry(cardExpiryData);
            int monthValue;
            if (string.IsNullOrWhiteSpace(cardExpiryData) || cardExpiryData.Length < 3
                || int.TryParse(cardExpiryData.Substring(0, 2), out monthValue) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 597));//Invalid date format in Expiry Date
            }
            log.LogMethodExit(monthValue);
            return monthValue;
        }
        /// <summary>
        /// GetCreditCardExpiryYear
        /// </summary>
        /// <param name="cardExpiryData"></param>
        public override int GetCreditCardExpiryYear(string cardExpiryData)
        {
            log.LogMethodEntry(cardExpiryData);
            int yearValue;
            string yearData = ServerDateTime.Now.Year.ToString().Substring(0,2);
            log.Info("yearData: " + yearData);
            if (string.IsNullOrWhiteSpace(cardExpiryData) || cardExpiryData.Length < 4
              || int.TryParse(yearData + cardExpiryData.Substring(2, 2), out yearValue) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 597));//Invalid date format in Expiry Date
            }
            log.LogMethodExit(yearValue);
            return yearValue;
        }

        /// <summary>
        /// Settle Transaction Payment
        /// </summary>
        public override TransactionPaymentsDTO SettleTransactionPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            CCTransactionsPGWDTO ccTransactionsPGWDTO = null;
            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CanAdjustTransactionPayment(transactionPaymentsDTO);
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);

                    CardConnectCommandHandler captureCommand = new CaptureCommandHandler(utilities, transactionPaymentsDTO, gatewayUrl, username, password, merchantId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = cCTransactionsPGWBL.CCTransactionsPGWDTO;
                    captureCommand.CreateCommand(ccOrigTransactionsPGWDTO);
                    HttpWebResponse response = captureCommand.Sendcommand();
                    CaptureData captureResp = captureCommand.GetResponse(response) as CaptureData;
                    ccTransactionsPGWDTO = captureCommand.ParseResponse(captureResp);
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    if (ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()))
                    {
                        ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.TIPADJUST.ToString();
                    }
                    else
                    {
                        ccTransactionsPGWDTO.TranCode = TransactionType.CAPTURE.ToString();
                    }
                    ccTransactionsPGWDTO.UserTraceData = ccOrigTransactionsPGWDTO.UserTraceData;
                    ccTransactionsPGWDTO.AcctNo = ccOrigTransactionsPGWDTO.AcctNo;
                    if (string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData) && !string.IsNullOrEmpty(ccTransactionsPGWDTO.AcctNo) && ccTransactionsPGWDTO.AcctNo.Substring(0, 1).Equals("3"))
                    {
                        ccTransactionsPGWDTO.UserTraceData = "AMEX";
                    }
                    ccTransactionsPGWDTO.AuthCode = ccOrigTransactionsPGWDTO.AuthCode;
                    ccTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                    ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                    ccTransactionsPGWDTO.CaptureStatus = ccOrigTransactionsPGWDTO.CaptureStatus;
                    ccTransactionsPGWDTO.TipAmount = transactionPaymentsDTO.TipAmount.ToString();
                    ccTransactionsPGWDTO.AcqRefData = ccOrigTransactionsPGWDTO.AcqRefData;
                    if (ccTransactionsPGWDTO != null)
                    {
                        SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                    }
                    cCTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    cCTransactionsPGWBL.Save();
                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.RefNo) && ccTransactionsPGWDTO.RecordNo.Equals("A"))
                    {
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                    }
                    else if (ccTransactionsPGWDTO.RecordNo.Equals("B"))
                    {
                        throw new Exception(utilities.MessageUtils.getMessage("Please Retry.") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                    }
                    else if (ccTransactionsPGWDTO.RecordNo.Equals("C"))
                    {
                        throw new Exception(utilities.MessageUtils.getMessage("Declined.") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                    }
                    else
                    {
                        throw new Exception(utilities.MessageUtils.getMessage("Invalid payment data."));
                    }
                    log.LogMethodExit(transactionPaymentsDTO);
                    return transactionPaymentsDTO;
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while performing settlement", ex);
                    log.LogMethodExit(null, "Throwing Exception " + ex);
                    throw;
                }
            }
            else
            {
                log.LogMethodExit("Transaction payment info is missing");
                throw new Exception("Transaction payment info is missing");
            }
        }

        /// <summary>
        /// Can Adjust Transaction Payment
        /// </summary>
        public override void CanAdjustTransactionPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            InquireResponse inquireResponse;
            InquireStatusCommandHandler inquireStatusHandler;
            if (transactionPaymentsDTO != null)
            {
                CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                inquireStatusHandler = new InquireStatusCommandHandler(null, gatewayUrl, username, password, merchantId);
                inquireStatusHandler.CreateCommand(cCTransactionsPGWBL.CCTransactionsPGWDTO);
                HttpWebResponse inquireresponse = inquireStatusHandler.Sendcommand();
                inquireResponse = inquireStatusHandler.GetResponse(inquireresponse) as InquireResponse;
                if (!inquireResponse.Setlstat.Equals("Authorized") && !inquireResponse.Setlstat.Equals("Queued for Capture"))
                {
                    log.LogMethodExit("No further adjustment is possible on this transaction.");
                    throw new Exception(utilities.MessageUtils.getMessage("No further adjustment is possible on this transaction."));
                }
            }
            else
            {
                throw new Exception("Transaction payment info is missing");
            }
            log.LogMethodExit();
        }
    }
}
