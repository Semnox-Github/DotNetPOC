/********************************************************************************************
 * Project Name - Clover Payment Gateway
 * Description  - Data handler of the CloverPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        12-AUG-2019    Raghuveera      Created  
 *2.150.1     22-Feb-2023    Guru S A        Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CloverPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();
        CloverResponse cloverResponse;
        CloverCommandHandler commandHandler;
        /// <summary>
        /// ManualResetEvent mre
        /// </summary>
        public ManualResetEvent eventCapture;
        string deviceUrl;
        private bool isCCDevicePaired;
        private bool isAuthorizationAllowed;
        private bool isAutoAcceptSignature;
        private bool enableAutoAuthorization;
        private bool isCustomerAllowedToDecideEntryMode;
        private bool isPrintCustomerCopy;

        public override bool IsTipAdjustmentAllowed
        {
            get { return true; }
        }
        public CloverPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
            if (showMessageDelegate == null)
            {
                showMessageDelegate = MessageBox.Show;
            }
            deviceUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_DEVICE_URL");
            log.LogVariableState("deviceUrl", deviceUrl);
            if (string.IsNullOrEmpty(deviceUrl))
            {
                log.Info("Please enter CREDIT_CARD_DEVICE_URL value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_DEVICE_URL value in configuration."));
            }
            if (!deviceUrl.StartsWith("ws://") && !deviceUrl.StartsWith("wss://"))
            {
                log.Info("Please enter valid url in CREDIT_CARD_DEVICE_URL configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter valid url in CREDIT_CARD_DEVICE_URL configuration."));
            }
            isPrintCustomerCopy = utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT").Equals("Y");
            isAuthorizationAllowed = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
            enableAutoAuthorization = utilities.getParafaitDefaults("ENABLE_AUTO_CREDITCARD_AUTHORIZATION").Equals("Y");
            isCustomerAllowedToDecideEntryMode = utilities.getParafaitDefaults("ALLOW_CUSTOMER_TO_DECIDE_ENTRY_MODE").Equals("Y");
            isAutoAcceptSignature = utilities.getParafaitDefaults("AUTO_ACCEPT_SIGNATURE").Equals("Y");
            isCCDevicePaired = false;
            log.LogMethodExit(null);
        }
        public override void Initialize()
        {
            log.LogMethodEntry();
            try
            {
                if (commandHandler == null)
                {
                    CCTransactionsPGWDTO cTransactionsPGWDTO;
                    TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO();
                    transactionPaymentsDTO.Amount = 0;
                    transactionPaymentsDTO.TransactionId = utilities.ParafaitEnv.POSMachineId;
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, PaymentGatewayTransactionType.PARING.ToString());
                    cTransactionsPGWDTO = GetLastParingResponse();
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "", "Clover Payment Gateway");
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Connecting device..."));
                    thr.Start();
                    commandHandler = new CloverCommandHandler(deviceUrl, utilities.ParafaitEnv.POSMachine, ((isUnattended) ? "ParafaitKiosk" : "ParafaitPOS"), (cTransactionsPGWDTO != null) ? cTransactionsPGWDTO.TokenID : "", showMessageDelegate);
                    commandHandler.utilities = utilities;
                    commandHandler.deviceError = DeviceErrorResponse;
                    commandHandler.displayText = statusDisplayUi.DisplayText;
                    commandHandler.IsManualEntryEnabled = isCustomerAllowedToDecideEntryMode;
                    commandHandler.IsPrintCustomerCopy = isPrintCustomerCopy;
                    commandHandler.IsAutoAcceptSignature = isAutoAcceptSignature;
                    commandHandler.ProcessDevicePairingRequest(GetDevicePairingResponse);
                    eventCapture = new ManualResetEvent(false);
                    if (!eventCapture.WaitOne(new TimeSpan(0, 3, 0)) || !isCCDevicePaired)
                    {
                        log.Error("Device pairing is failed.");
                        throw new Exception(utilities.MessageUtils.getMessage(2271));
                    }
                }
            }
            catch (Exception ex)
            {
                eventCapture.Reset();
                if (statusDisplayUi != null)
                {
                    log.Error("Error in initializing", ex);
                    statusDisplayUi.DisplayText(ex.Message);
                    throw;
                }
            }
            finally
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
            }
            log.LogMethodExit();

        }
        private void StatusDisplayUi_CancelClicked(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            CloverRequest cloverRequest = new CloverRequest();
            cloverRequest.TrxType = PaymentGatewayTransactionType.USERCANCEL;
            commandHandler.ProcessRequest(cloverRequest, TransactionResponse);
            log.LogMethodExit();
        }
        private void GetDevicePairingResponse(bool status, CloverResponse cloverResponse)
        {
            log.LogMethodEntry();
            if (status)
            {
                isCCDevicePaired = true;
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2272));
                log.Info("Device pairing is successful.");
            }
            else
            {
                isCCDevicePaired = false;
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2271));
                log.Info("Device pairing is failed. Transaction can not be processed.");
            }
            try
            {
                if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                {
                    cloverResponse.CCTransactionsPGWDTO.RecordNo = Environment.MachineName;
                    cloverResponse.CCTransactionsPGWDTO.InvoiceNo = utilities.ParafaitEnv.POSMachineId.ToString();
                    SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                }
            }
            catch (Exception ex)
            {
                log.Error("Paring response is not saved.", ex);
            }
            if (eventCapture != null)
                eventCapture.Set();
            log.LogMethodExit();
        }
        private void TransactionResponse(CloverResponse cloverResponse)
        {
            log.LogMethodEntry(cloverResponse);
            this.cloverResponse = cloverResponse;
            if (eventCapture != null)
            {
                eventCapture.Set();
            }
            else
            {
                try
                {
                    if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                    {
                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.InvoiceNo))
                        {
                            cloverResponse.CCTransactionsPGWDTO.InvoiceNo = " ";
                        }
                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.RecordNo))
                        {
                            cloverResponse.CCTransactionsPGWDTO.RecordNo = " ";
                        }
                        SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                    }
                }
                catch (Exception ex) { log.Error("Late response:", ex); }

            }
            log.LogMethodExit();
        }
        private void DeviceErrorResponse(CloverResponse cloverResponse)
        {
            log.LogMethodEntry(cloverResponse);
            this.cloverResponse = cloverResponse;
            if (eventCapture != null)
            {
                eventCapture.Set();
            }
            else
            {
                try
                {
                    if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                    {
                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.InvoiceNo))
                        {
                            cloverResponse.CCTransactionsPGWDTO.InvoiceNo = " ";
                        }
                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.RecordNo))
                        {
                            cloverResponse.CCTransactionsPGWDTO.RecordNo = " ";
                        }
                        SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                    }
                }
                catch (Exception ex) { log.Error("Late response:", ex); }
            }
            log.LogMethodExit();
        }
        private CCTransactionsPGWDTO GetLastParingResponse()
        {
            log.LogMethodEntry();
            CCTransactionsPGWDTO cTransactionsPGWDTO = null;
            CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, utilities.ParafaitEnv.POSMachineId.ToString()));
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.PARING.ToString()));
            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOs = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(searchParameters);
            if (cCTransactionsPGWDTOs != null && cCTransactionsPGWDTOs.Count > 0)
            {
                cTransactionsPGWDTO = (cCTransactionsPGWDTOs.OrderByDescending(s => s.TransactionDatetime)).ToArray<CCTransactionsPGWDTO>()[0];
            }
            log.LogMethodExit(cTransactionsPGWDTO);
            return cTransactionsPGWDTO;
        }
        private void SaveTransactionResponse(CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(cCTransactionsPGWDTO);
            if (string.IsNullOrEmpty(cCTransactionsPGWDTO.RecordNo))
            {
                cCTransactionsPGWDTO.RecordNo = " ";
            }
            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
            CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
            cCTransactionsPGWBL.Save();
            log.LogMethodExit();
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
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            try
            {
                StandaloneRefundNotAllowed(transactionPaymentsDTO);
                CCTransactionsPGWDTO cCOrgTransactionsPGWDTO = null;
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Clover Payment Gateway");
                commandHandler.displayText = statusDisplayUi.DisplayText;
                statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                statusDisplayUi.EnableCancelButton(true);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));

                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                CloverRequest cloverRequest = new CloverRequest();
                cloverRequest.IsUnattended = isUnattended;
                cloverRequest.TrxType = PaymentGatewayTransactionType.SALE;
                if (!isUnattended)
                {
                    if (isAuthorizationAllowed && enableAutoAuthorization)
                    {
                        log.Debug("Creditcard auto authorization is enabled");
                        cloverRequest.TrxType = PaymentGatewayTransactionType.AUTHORIZATION;
                    }
                    else
                    {
                        cCOrgTransactionsPGWDTO = GetPreAuthorizationCCTransactionsPGWDTO(transactionPaymentsDTO);
                        if (isAuthorizationAllowed)
                        {
                            frmTransactionTypeUI frmTranType = new frmTransactionTypeUI(utilities, (cCOrgTransactionsPGWDTO == null) ? "TATokenRequest" : "Authorization", transactionPaymentsDTO.Amount, showMessageDelegate);
                            if (frmTranType.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                if (frmTranType.TransactionType.Equals("Authorization") || frmTranType.TransactionType.Equals("Sale"))
                                {
                                    if (frmTranType.TransactionType.Equals("Authorization"))
                                    {
                                        cloverRequest.TrxType = PaymentGatewayTransactionType.AUTHORIZATION;
                                    }
                                    else
                                    {
                                        cloverRequest.TrxType = PaymentGatewayTransactionType.SALE;
                                    }
                                    if (cCOrgTransactionsPGWDTO != null)
                                    {
                                        cloverRequest.TokenId = cCOrgTransactionsPGWDTO.TokenID;
                                        cloverRequest.AccountNo = cCOrgTransactionsPGWDTO.AcctNo;
                                        cloverRequest.CardExpiryDate = cCOrgTransactionsPGWDTO.RecordNo;
                                        cloverRequest.CardHolderName = cCOrgTransactionsPGWDTO.AcqRefData;
                                    }
                                }
                                else if (frmTranType.TransactionType.Equals("TATokenRequest"))
                                {
                                    cloverRequest.TrxType = PaymentGatewayTransactionType.TATokenRequest;
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
                thr.Start();
                cloverRequest.ReferenceId = (isUnattended ? "U" : "A") + ccRequestPGWDTO.RequestID.ToString().PadLeft(31, '0');
                cloverRequest.Amount = Convert.ToDecimal(transactionPaymentsDTO.Amount);
                eventCapture = new ManualResetEvent(false);
                commandHandler.ProcessRequest(cloverRequest, TransactionResponse);
                eventCapture.Reset();
                if (!eventCapture.WaitOne())
                {
                    if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                    {
                        try
                        {
                            SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error in saving the response", ex);
                        }
                    }
                    else
                    {
                        log.Debug("CCTransactionsPGWDTO is null");
                    }
                    if (cloverResponse != null && !string.IsNullOrEmpty(cloverResponse.ResponseMessage))
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                        log.Error(cloverResponse.ResponseMessage);
                        throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                    }
                    else
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                        log.Error("Error occured during the payment.");
                        throw new Exception(utilities.MessageUtils.getMessage(2273));
                    }
                }
                else
                {
                    if (cloverResponse == null)
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                        log.Error("Error occured during the payment.");
                        throw new Exception(utilities.MessageUtils.getMessage(2273));
                    }
                    else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO == null)
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("No response. ") + utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                        log.Error("No response. " + cloverResponse.ResponseMessage);
                        throw new Exception(utilities.MessageUtils.getMessage("No response.") + utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                    }
                    else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                    {
                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.InvoiceNo))
                        {
                            cloverResponse.CCTransactionsPGWDTO.InvoiceNo = cloverRequest.ReferenceId;
                        }
                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.RecordNo))
                        {
                            cloverResponse.CCTransactionsPGWDTO.RecordNo = " ";
                        }
                        if (cCOrgTransactionsPGWDTO != null)
                        {
                            if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TokenID))
                            {
                                cloverResponse.CCTransactionsPGWDTO.TokenID = cCOrgTransactionsPGWDTO.TokenID;
                            }
                            if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AcctNo) || (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AcctNo) && cloverResponse.CCTransactionsPGWDTO.AcctNo.Equals("XXXXXXXXXX")))
                            {
                                cloverResponse.CCTransactionsPGWDTO.AcctNo = cCOrgTransactionsPGWDTO.AcctNo;
                            }
                            if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.RecordNo))
                            {
                                cloverResponse.CCTransactionsPGWDTO.RecordNo = cCOrgTransactionsPGWDTO.RecordNo;
                            }
                            if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AcqRefData))
                            {
                                cloverResponse.CCTransactionsPGWDTO.AcqRefData = cCOrgTransactionsPGWDTO.AcqRefData;
                            }
                        }
                        cloverResponse.CCTransactionsPGWDTO.CustomerCopy = cloverResponse.CustomerReceiptText;
                        cloverResponse.CCTransactionsPGWDTO.MerchantCopy = cloverResponse.MerchantReceiptText;
                        SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                        if (cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode.Equals("SUCCESS"))
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage((string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                            transactionPaymentsDTO.CreditCardNumber = cloverResponse.CCTransactionsPGWDTO.AcctNo;
                            transactionPaymentsDTO.CreditCardName = cloverResponse.CCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.NameOnCreditCard = cloverResponse.CCTransactionsPGWDTO.AcqRefData;
                            transactionPaymentsDTO.CreditCardAuthorization = cloverResponse.CCTransactionsPGWDTO.AuthCode;
                            transactionPaymentsDTO.CCResponseId = cloverResponse.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.Reference = cloverResponse.CCTransactionsPGWDTO.InvoiceNo;
                            transactionPaymentsDTO.TipAmount = (Convert.ToDouble(string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TipAmount) ? "0" : cloverResponse.CCTransactionsPGWDTO.TipAmount) * 0.01);
                            transactionPaymentsDTO.Amount = Convert.ToDouble(cloverResponse.CCTransactionsPGWDTO.Authorize) * 0.01;
                            transactionPaymentsDTO.Memo = cloverResponse.CustomerReceiptText;
                            //if ((isUnattended && isPrintCustomerCopy) && !string.IsNullOrEmpty(cloverResponse.CustomerReceiptText))
                            //{
                            //    Print(cloverResponse.CustomerReceiptText);
                            //}
                        }
                        else
                        {
                            log.Error(cloverResponse.ResponseMessage);
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage((string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                            throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage((string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                log.Error(ex);
                throw;
            }
            finally
            {
                CleanUp();
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
        public override void CleanUp()
        {
            log.LogMethodEntry();
            cloverResponse = null;
            eventCapture = null;
            log.LogMethodExit();
        }
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            CloverRequest cloverTrxStatusRequest = new CloverRequest();
            CloverRequest cloverRequest = new CloverRequest();
            try
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount > 0)
                {
                    if (transactionPaymentsDTO.CCResponseId == -1)
                    {
                        log.LogMethodExit("Original transaction response not found!");
                        throw new Exception(utilities.MessageUtils.getMessage("Original transaction response not found!"));
                    }
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(4202, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Clover Payment Gateway");
                    commandHandler.displayText = statusDisplayUi.DisplayText;
                    statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                    cloverTrxStatusRequest.TrxType = PaymentGatewayTransactionType.STATUSCHECK;
                    cloverTrxStatusRequest.IsUnattended = isUnattended;
                    //if (!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.ProcessData))
                    //{
                    //    cloverTrxStatusRequest.ReferenceId = ccOrigTransactionsPGWDTO.ProcessData.Split('|')[0];
                    //}
                    //else
                    //{
                    cloverTrxStatusRequest.ReferenceId = ccOrigTransactionsPGWDTO.InvoiceNo;
                    //}
                    eventCapture = new ManualResetEvent(false);
                    commandHandler.ProcessRequest(cloverTrxStatusRequest, TransactionResponse);
                    eventCapture.Reset();
                    if (!eventCapture.WaitOne())
                    {
                        if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                        {
                            try
                            {
                                SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error in saving the response", ex);
                            }
                        }
                        else
                        {
                            log.Debug("CCTransactionsPGWDTO is null");
                        }
                        if (cloverResponse != null && !string.IsNullOrEmpty(cloverResponse.ResponseMessage))
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                            log.Error(cloverResponse.ResponseMessage);
                            throw new Exception(cloverResponse.ResponseMessage);
                        }
                        else
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                            log.Error("Error occured during the payment.");
                            throw new Exception(utilities.MessageUtils.getMessage(2273));
                        }
                    }
                    else
                    {
                        if (cloverResponse == null)
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                            log.Error("Error occured during the payment.");
                            throw new Exception(utilities.MessageUtils.getMessage(2273));
                        }
                        else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO == null)
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("No response. ") + utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                            log.Error("No response. " + cloverResponse.ResponseMessage);
                            throw new Exception(utilities.MessageUtils.getMessage("No response. ") + utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                        }
                        else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                        {
                            cloverRequest.ReferenceId = ccOrigTransactionsPGWDTO.ProcessData.Split('|')[0];
                            cloverRequest.OrderId = ccOrigTransactionsPGWDTO.RefNo;
                            if (cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode.Equals("NOT_FOUND"))
                            {
                                log.Info(cloverResponse.CCTransactionsPGWDTO.TextResponse);
                                try
                                {
                                    SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                                }
                                catch
                                {
                                    log.Error("save response failed");
                                }
                                throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.TextResponse));
                            }
                            else if ((cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode.Equals("PENDING") || ccOrigTransactionsPGWDTO.TransactionDatetime.CompareTo(utilities.getServerTime().AddHours(-12)) >= 0) && Convert.ToInt64(ccOrigTransactionsPGWDTO.Authorize) == Convert.ToInt64(transactionPaymentsDTO.Amount * 100))
                            {
                                cloverRequest.TrxType = PaymentGatewayTransactionType.VOID;
                            }
                            else
                            {
                                cloverRequest.TrxType = PaymentGatewayTransactionType.REFUND;
                                cloverRequest.ReferenceId = (isUnattended ? "U" : "A") + cCRequestPGWDTO.RequestID.ToString().PadLeft(31, '0');
                                if ((Convert.ToInt64(ccOrigTransactionsPGWDTO.Authorize) + Convert.ToInt64(string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.TipAmount) ? "0" : ccOrigTransactionsPGWDTO.TipAmount)) != Convert.ToInt64((transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount) * 100))
                                {
                                    transactionPaymentsDTO.TipAmount = 0;
                                    cloverRequest.Amount = Convert.ToDecimal(transactionPaymentsDTO.Amount);
                                }
                                else
                                {
                                    cloverRequest.Amount = Convert.ToDecimal(transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount);
                                }
                                //    cloverRequest.IsFullRefund = false;

                                if (ccOrigTransactionsPGWDTO != null)
                                {
                                    cloverRequest.TokenId = ccOrigTransactionsPGWDTO.TokenID;
                                    cloverRequest.AccountNo = ccOrigTransactionsPGWDTO.AcctNo;
                                    cloverRequest.CardExpiryDate = ccOrigTransactionsPGWDTO.RecordNo;
                                    cloverRequest.CardHolderName = ccOrigTransactionsPGWDTO.AcqRefData;
                                }
                                //}
                                //else
                                //{
                                //    cloverRequest.IsFullRefund = true;
                                //}
                            }

                            eventCapture = new ManualResetEvent(false);
                            cloverResponse = null;
                            cloverRequest.IsUnattended = isUnattended;
                            commandHandler.ProcessRequest(cloverRequest, TransactionResponse);
                            eventCapture.Reset();
                            if (!eventCapture.WaitOne())
                            {
                                if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                                {
                                    try
                                    {
                                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.InvoiceNo))
                                        {
                                            cloverResponse.CCTransactionsPGWDTO.InvoiceNo = cloverRequest.ReferenceId;
                                        }
                                        SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("Error in saving the response", ex);
                                    }
                                }
                                else
                                {
                                    log.Debug("CCTransactionsPGWDTO is null");
                                }
                                if (cloverResponse != null && !string.IsNullOrEmpty(cloverResponse.ResponseMessage))
                                {
                                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                                    log.Error(cloverResponse.ResponseMessage);
                                    throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                                }
                                else
                                {
                                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                                    log.Error("Error occured during the payment.");
                                    throw new Exception(utilities.MessageUtils.getMessage(2273));
                                }
                            }
                            else
                            {
                                if (cloverResponse == null)
                                {
                                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                                    log.Error("Error occured during the payment.");
                                    throw new Exception(utilities.MessageUtils.getMessage(2273));
                                }
                                else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO == null)
                                {
                                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("No response. ") + utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                                    log.Error("No response. " + cloverResponse.ResponseMessage);
                                    throw new Exception("No response." + cloverResponse.ResponseMessage);
                                }
                                else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                                {
                                    if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.InvoiceNo))
                                    {
                                        cloverResponse.CCTransactionsPGWDTO.InvoiceNo = cloverRequest.ReferenceId;
                                    }
                                    if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.RecordNo))
                                    {
                                        cloverResponse.CCTransactionsPGWDTO.RecordNo = " ";
                                    }
                                    if (cloverResponse.CCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.REFUND.ToString()))
                                    {
                                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AcctNo))
                                        {
                                            cloverResponse.CCTransactionsPGWDTO.AcctNo = ccOrigTransactionsPGWDTO.AcctNo;
                                        }
                                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AuthCode))
                                        {
                                            cloverResponse.CCTransactionsPGWDTO.AuthCode = ccOrigTransactionsPGWDTO.AuthCode;
                                        }
                                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TokenID))
                                        {
                                            cloverResponse.CCTransactionsPGWDTO.TokenID = ccOrigTransactionsPGWDTO.TokenID;
                                        }
                                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.CaptureStatus))
                                        {
                                            cloverResponse.CCTransactionsPGWDTO.CaptureStatus = ccOrigTransactionsPGWDTO.CaptureStatus;
                                        }
                                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AcqRefData))
                                        {
                                            cloverResponse.CCTransactionsPGWDTO.AcqRefData = ccOrigTransactionsPGWDTO.AcqRefData;
                                        }
                                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.UserTraceData))
                                        {
                                            cloverResponse.CCTransactionsPGWDTO.UserTraceData = ccOrigTransactionsPGWDTO.UserTraceData;
                                        }
                                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.ProcessData))
                                        {
                                            cloverResponse.CCTransactionsPGWDTO.ProcessData = ccOrigTransactionsPGWDTO.ProcessData;
                                        }
                                        if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.RecordNo))
                                        {
                                            cloverResponse.CCTransactionsPGWDTO.RecordNo = ccOrigTransactionsPGWDTO.RecordNo;
                                        }
                                        cloverResponse.CCTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                                        cloverResponse.CCTransactionsPGWDTO.InvoiceNo = cloverRequest.ReferenceId;

                                    }
                                    cloverResponse.CCTransactionsPGWDTO.MerchantCopy = cloverResponse.MerchantReceiptText;
                                    cloverResponse.CCTransactionsPGWDTO.CustomerCopy = cloverResponse.CustomerReceiptText;
                                    SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                                    if (cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode.Equals("SUCCESS"))
                                    {
                                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage((string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                                        transactionPaymentsDTO.CreditCardNumber = cloverResponse.CCTransactionsPGWDTO.AcctNo;
                                        transactionPaymentsDTO.CreditCardName = cloverResponse.CCTransactionsPGWDTO.CardType;
                                        transactionPaymentsDTO.NameOnCreditCard = cloverResponse.CCTransactionsPGWDTO.AcqRefData;
                                        transactionPaymentsDTO.CreditCardAuthorization = cloverResponse.CCTransactionsPGWDTO.AuthCode;
                                        transactionPaymentsDTO.CCResponseId = cloverResponse.CCTransactionsPGWDTO.ResponseID;
                                        transactionPaymentsDTO.Reference = cloverResponse.CCTransactionsPGWDTO.InvoiceNo;
                                        if ((Convert.ToDecimal(Convert.ToDouble(cloverResponse.CCTransactionsPGWDTO.Authorize) * 0.01)) > Convert.ToDecimal(transactionPaymentsDTO.Amount.ToString("0.00")))
                                        {
                                            transactionPaymentsDTO.TipAmount = ((Convert.ToDouble(cloverResponse.CCTransactionsPGWDTO.Authorize) * (0.01)) - transactionPaymentsDTO.Amount) * -1;
                                        }
                                        else
                                        {
                                            transactionPaymentsDTO.Amount = Convert.ToDouble(cloverResponse.CCTransactionsPGWDTO.Authorize) * (-0.01);
                                        }
                                        //transactionPaymentsDTO.Amount = Convert.ToDouble(cloverResponse.CCTransactionsPGWDTO.Authorize) * 0.01;
                                        transactionPaymentsDTO.Memo = cloverResponse.CustomerReceiptText;
                                        //if ((isUnattended && isPrintCustomerCopy) && !string.IsNullOrEmpty(cloverResponse.CustomerReceiptText))
                                        //{
                                        //    Print(cloverResponse.CustomerReceiptText);
                                        //}
                                    }
                                    else
                                    {
                                        log.Error(cloverResponse.ResponseMessage);
                                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage((string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                                        throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage((string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                                    }
                                }
                            }
                        }
                    }
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
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public override bool IsSettlementPending(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            bool returnValue = false;
            if (isAuthorizationAllowed)
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
                if (transactionPaymentsDTO != null)
                {
                    if (!IsForcedSettlement)//2017-09-27
                    {
                        //frmTransactionTypeUI frmTranType = new frmTransactionTypeUI(utilities, "Completion", transactionPaymentsDTO.Amount, showMessageDelegate);
                        frmFinalizeTransaction frmFinalizeTransaction = new frmFinalizeTransaction(utilities, overallTransactionAmount, overallTipAmountEntered, Convert.ToDecimal(transactionPaymentsDTO.Amount), Convert.ToDecimal(transactionPaymentsDTO.TipAmount), transactionPaymentsDTO.CreditCardNumber, showMessageDelegate);
                        if (frmFinalizeTransaction.ShowDialog() != DialogResult.Cancel)
                        {
                            transactionPaymentsDTO.TipAmount = Convert.ToDouble(frmFinalizeTransaction.TipAmount);
                        }
                        else
                        {
                            log.LogMethodExit("CANCELLED");
                            throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                        }
                    }
                    transactionPaymentsDTO = PayTip(transactionPaymentsDTO);
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while performing settlement", ex);
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw (ex);
            }
        }
        public override bool IsTipAllowed(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            bool isAllowed = false;
            log.LogMethodEntry();
            if (isAuthorizationAllowed)
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.CCResponseId != -1)
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.CAPTURE.ToString()));
                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                    if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                    {
                        isAllowed = true;
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

        public override TransactionPaymentsDTO PayTip(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                string limit = utilities.getParafaitDefaults("MAX_TIP_AMOUNT_PERCENTAGE");
                long tipLimit = Convert.ToInt64(string.IsNullOrEmpty(limit) ? "200" : limit);
                if (tipLimit > 0 && ((transactionPaymentsDTO.Amount * tipLimit) / 100) < transactionPaymentsDTO.TipAmount)
                {
                    if (showMessageDelegate == null)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1832, tipLimit), "Tip validation", MessageBoxButtons.OK);
                    }
                    else
                    {
                        showMessageDelegate(utilities.MessageUtils.getMessage(1832, tipLimit), "Tip validation", MessageBoxButtons.OK);
                    }
                    throw new Exception(utilities.MessageUtils.getMessage("Tip limit exceeded"));
                }
                CloverRequest cloverRequest = new CloverRequest();
                CloverRequest cloverTrxStatusRequest = new CloverRequest();
                if (transactionPaymentsDTO != null)
                {
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Clover Payment Gateway");
                    commandHandler.displayText = statusDisplayUi.DisplayText;
                    statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;

                    if (!ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()) && !ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()) && !ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                    {
                        log.Info("Tip adjustment is not allowed for the transaction type " + ccOrigTransactionsPGWDTO.TranCode);
                        throw new Exception(utilities.MessageUtils.getMessage(2275, ccOrigTransactionsPGWDTO.TranCode));
                    }
                    if (ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()) && transactionPaymentsDTO.TipAmount == 0)
                    {
                        ccOrigTransactionsPGWDTO.ResponseID = -1;
                        ccOrigTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                        ccOrigTransactionsPGWDTO.TextResponse = "Internal capture.";
                        SaveTransactionResponse(ccOrigTransactionsPGWDTO);
                        transactionPaymentsDTO.CCResponseId = ccOrigTransactionsPGWDTO.ResponseID;
                        log.LogMethodExit(transactionPaymentsDTO);
                        return transactionPaymentsDTO;
                    }
                    cloverTrxStatusRequest.IsUnattended = isUnattended;
                    cloverTrxStatusRequest.TrxType = PaymentGatewayTransactionType.STATUSCHECK;
                    //if (!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.ProcessData))
                    //{
                    //cloverTrxStatusRequest.ReferenceId = ccOrigTransactionsPGWDTO.ProcessData.Split('|')[0];
                    //}
                    //else
                    //{
                    cloverTrxStatusRequest.ReferenceId = ccOrigTransactionsPGWDTO.InvoiceNo;
                    //}
                    //eventCapture = new ManualResetEvent(false);
                    //commandHandler.ProcessRequest(cloverTrxStatusRequest, TransactionResponse);
                    //eventCapture.Reset();
                    //if (!eventCapture.WaitOne())
                    //{
                    //    if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                    //    {
                    //        try
                    //        {
                    //            SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            log.Error("Error in saving the response", ex);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        log.Debug("CCTransactionsPGWDTO is null");
                    //    }
                    //    if (cloverResponse != null && !string.IsNullOrEmpty(cloverResponse.ResponseMessage))
                    //    {
                    //        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                    //        log.Error(cloverResponse.ResponseMessage);
                    //        throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                    //    }
                    //    else
                    //    {
                    //        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                    //        log.Error("Error occured during the payment.");
                    //        throw new Exception(utilities.MessageUtils.getMessage(2273));
                    //    }
                    //}
                    //else
                    //{
                    //if (cloverResponse == null)
                    //{
                    //    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                    //    log.Error("Error occured during the payment.");
                    //    throw new Exception(utilities.MessageUtils.getMessage(2273));
                    //}
                    //else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO == null)
                    //{
                    //    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("No response. ") + utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                    //    log.Error("No response. " + cloverResponse.ResponseMessage);
                    //    throw new Exception(utilities.MessageUtils.getMessage("No response.") + utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                    //}
                    //else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                    //{
                    cloverRequest.ReferenceId = ccOrigTransactionsPGWDTO.ProcessData.Split('|')[0];
                    cloverRequest.OrderId = ccOrigTransactionsPGWDTO.RefNo;

                    DateTime originalPaymentDate = new DateTime();
                    TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                    List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, transactionPaymentsDTO.TransactionId.ToString()));
                    List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPaymentsDTOList(searchParameters);
                    if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Any())
                    {
                        originalPaymentDate = transactionPaymentsDTOList[0].PaymentDate;
                    }
                    DateTime bussStartTime = utilities.getServerTime().Date.AddHours(Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")));
                    DateTime bussEndTime = bussStartTime.AddDays(1);
                    if (utilities.getServerTime() < bussStartTime)
                    {
                        bussStartTime = bussStartTime.AddDays(-1);
                        bussEndTime = bussStartTime.AddDays(1);
                    }

                    if ((originalPaymentDate >= bussStartTime) && (originalPaymentDate <= bussEndTime))
                    {
                        cloverRequest.TrxType = PaymentGatewayTransactionType.TIPADJUST;
                        cloverRequest.TipAmount = Convert.ToDecimal(transactionPaymentsDTO.TipAmount);
                    }
                    else if (ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
                    {
                        ccOrigTransactionsPGWDTO.ResponseID = -1;
                        ccOrigTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                        ccOrigTransactionsPGWDTO.TextResponse = "Internal capture. Transaction is closed in gateway. Tip can not be added.";
                        SaveTransactionResponse(ccOrigTransactionsPGWDTO);
                        transactionPaymentsDTO.CCResponseId = ccOrigTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.TipAmount = 0;
                        log.LogMethodExit(transactionPaymentsDTO);
                        return transactionPaymentsDTO;
                    }

                    //if (cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode.Equals("PENDING"))
                    //    {
                    //        cloverRequest.TrxType = PaymentGatewayTransactionType.TIPADJUST;
                    //        cloverRequest.TipAmount = Convert.ToDecimal(transactionPaymentsDTO.TipAmount);
                    //    }
                    //    else if (cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode.Equals("CLOSED") && ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
                    //    {
                    //        ccOrigTransactionsPGWDTO.ResponseID = -1;
                    //        ccOrigTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                    //        ccOrigTransactionsPGWDTO.TextResponse = "Internal capture. Transaction is closed in gateway. Tip can not be added.";
                    //        SaveTransactionResponse(ccOrigTransactionsPGWDTO);
                    //        transactionPaymentsDTO.CCResponseId = ccOrigTransactionsPGWDTO.ResponseID;
                    //        transactionPaymentsDTO.TipAmount = 0;
                    //        log.LogMethodExit(transactionPaymentsDTO);
                    //        return transactionPaymentsDTO;
                    //    }
                    //else if (cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode.Equals("NOT_FOUND") && ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
                    //{
                    //    log.Info(cloverResponse.CCTransactionsPGWDTO.TextResponse);
                    //    try
                    //    {
                    //        SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                    //    }
                    //    catch
                    //    {
                    //        log.Error("save response failed");
                    //    }
                    //    throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.TextResponse));
                    //}
                    else
                    {
                        log.Info("Transaction already settled.Tip can not be adjusted.");
                        throw new Exception(utilities.MessageUtils.getMessage(2276));
                    }

                    eventCapture = new ManualResetEvent(false);
                    cloverResponse = null;
                    cloverRequest.IsUnattended = isUnattended;
                    commandHandler.ProcessRequest(cloverRequest, TransactionResponse);
                    eventCapture.Reset();
                    if (!eventCapture.WaitOne())
                    {
                        if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                        {
                            try
                            {
                                SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error in saving the response", ex);
                            }
                        }
                        else
                        {
                            log.Debug("CCTransactionsPGWDTO is null");
                        }
                        if (cloverResponse != null && !string.IsNullOrEmpty(cloverResponse.ResponseMessage))
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                            log.Error(cloverResponse.ResponseMessage);
                            throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                        }
                        else
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                            log.Error("Error occured during the payment.");
                            throw new Exception(utilities.MessageUtils.getMessage(2273));
                        }
                    }
                    else
                    {
                        if (cloverResponse == null)
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                            log.Error("Error occured during the payment.");
                            throw new Exception(utilities.MessageUtils.getMessage(2273));
                        }
                        else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO == null)
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("No response. ") + utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                            log.Error("No response. " + cloverResponse.ResponseMessage);
                            throw new Exception("No response." + cloverResponse.ResponseMessage);
                        }
                        else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                        {
                            if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.InvoiceNo))
                            {
                                cloverResponse.CCTransactionsPGWDTO.InvoiceNo = ccOrigTransactionsPGWDTO.InvoiceNo;
                            }
                            if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.RecordNo))
                            {
                                cloverResponse.CCTransactionsPGWDTO.RecordNo = " ";
                            }
                            if (cloverResponse.CCTransactionsPGWDTO.TranCode == PaymentGatewayTransactionType.AUTHORIZATION.ToString())
                            {
                                cloverResponse.CCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                            }
                            cloverResponse.CCTransactionsPGWDTO.RefNo = ccOrigTransactionsPGWDTO.RefNo;
                            if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.Authorize))
                            {
                                cloverResponse.CCTransactionsPGWDTO.Authorize = ccOrigTransactionsPGWDTO.Authorize;
                            }
                            SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                            if (cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode.Equals("SUCCESS"))
                            {
                                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage(((!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse)) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                                if (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AcctNo))
                                {
                                    transactionPaymentsDTO.CreditCardNumber = cloverResponse.CCTransactionsPGWDTO.AcctNo;
                                }
                                if (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.CardType))
                                {
                                    transactionPaymentsDTO.CreditCardName = cloverResponse.CCTransactionsPGWDTO.CardType;
                                }
                                if (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AcqRefData))
                                {
                                    transactionPaymentsDTO.NameOnCreditCard = cloverResponse.CCTransactionsPGWDTO.AcqRefData;
                                }
                                if (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AuthCode))
                                {
                                    transactionPaymentsDTO.CreditCardAuthorization = cloverResponse.CCTransactionsPGWDTO.AuthCode;
                                }
                                if (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.Authorize))
                                {
                                    transactionPaymentsDTO.Amount = Convert.ToDouble(cloverResponse.CCTransactionsPGWDTO.Authorize) * 0.01;
                                }
                                transactionPaymentsDTO.CCResponseId = cloverResponse.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.Reference = cloverResponse.CCTransactionsPGWDTO.InvoiceNo;
                            }
                            else
                            {
                                log.Error(cloverResponse.ResponseMessage);
                                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage((string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                                throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage((string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                            }
                        }
                    }
                    //}
                    //}
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                if (statusDisplayUi != null)
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2274));
                log.Error("Error occured while performing settlement", ex);
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw (ex);
            }
            finally
            {
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
                CleanUp();
            }
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
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.AUTHORIZATION.ToString()));
            cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
            log.LogMethodExit(cCTransactionsPGWDTOList);
            return cCTransactionsPGWDTOList;
        }

        /// <summary>
        /// Settle Transaction Payment
        /// </summary>
        public override TransactionPaymentsDTO SettleTransactionPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            CCTransactionsPGWDTO ccTransactionsPGWDTO = null;
            CloverRequest cloverRequest = new CloverRequest();
            CloverRequest cloverTrxStatusRequest = new CloverRequest();
            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CanAdjustTransactionPayment(transactionPaymentsDTO);
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = cCTransactionsPGWBL.CCTransactionsPGWDTO;
                    if (ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()) && transactionPaymentsDTO.TipAmount == 0)
                    {
                        ccOrigTransactionsPGWDTO.ResponseID = -1;
                        ccOrigTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                        ccOrigTransactionsPGWDTO.TextResponse = "Internal capture.";
                        SaveTransactionResponse(ccOrigTransactionsPGWDTO);
                        transactionPaymentsDTO.CCResponseId = ccOrigTransactionsPGWDTO.ResponseID;
                        log.LogMethodExit(transactionPaymentsDTO);
                        return transactionPaymentsDTO;
                    }
                    cloverTrxStatusRequest.IsUnattended = isUnattended;
                    cloverTrxStatusRequest.TrxType = PaymentGatewayTransactionType.STATUSCHECK;
                    cloverTrxStatusRequest.ReferenceId = ccOrigTransactionsPGWDTO.InvoiceNo;

                    cloverRequest.ReferenceId = ccOrigTransactionsPGWDTO.ProcessData.Split('|')[0];
                    cloverRequest.OrderId = ccOrigTransactionsPGWDTO.RefNo;

                    DateTime originalPaymentDate = new DateTime();
                    TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                    List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, transactionPaymentsDTO.TransactionId.ToString()));
                    List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPaymentsDTOList(searchParameters);
                    if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Any())
                    {
                        originalPaymentDate = transactionPaymentsDTOList[0].PaymentDate;
                    }
                    DateTime bussStartTime = utilities.getServerTime().Date.AddHours(Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")));
                    DateTime bussEndTime = bussStartTime.AddDays(1);
                    if (utilities.getServerTime() < bussStartTime)
                    {
                        bussStartTime = bussStartTime.AddDays(-1);
                        bussEndTime = bussStartTime.AddDays(1);
                    }

                    if ((originalPaymentDate >= bussStartTime) && (originalPaymentDate <= bussEndTime))
                    {
                        cloverRequest.TrxType = PaymentGatewayTransactionType.TIPADJUST;
                        cloverRequest.TipAmount = Convert.ToDecimal(transactionPaymentsDTO.TipAmount);
                    }
                    else if (ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
                    {
                        ccOrigTransactionsPGWDTO.ResponseID = -1;
                        ccOrigTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                        ccOrigTransactionsPGWDTO.TextResponse = "Internal capture. Transaction is closed in gateway. Tip can not be added.";
                        SaveTransactionResponse(ccOrigTransactionsPGWDTO);
                        transactionPaymentsDTO.CCResponseId = ccOrigTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.TipAmount = 0;
                        log.LogMethodExit(transactionPaymentsDTO);
                        return transactionPaymentsDTO;
                    }
                    else
                    {
                        log.Info("Transaction already settled.Tip can not be adjusted.");
                        throw new Exception(utilities.MessageUtils.getMessage(2276));
                    }

                    eventCapture = new ManualResetEvent(false);
                    cloverResponse = null;
                    cloverRequest.IsUnattended = isUnattended;
                    commandHandler.ProcessRequest(cloverRequest, TransactionResponse);
                    eventCapture.Reset();
                    if (!eventCapture.WaitOne())
                    {
                        if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                        {
                            try
                            {
                                SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error in saving the response", ex);
                            }
                        }
                        else
                        {
                            log.Debug("CCTransactionsPGWDTO is null");
                        }
                        if (cloverResponse != null && !string.IsNullOrEmpty(cloverResponse.ResponseMessage))
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                            log.Error(cloverResponse.ResponseMessage);
                            throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                        }
                        else
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                            log.Error("Error occured during the payment.");
                            throw new Exception(utilities.MessageUtils.getMessage(2273));
                        }
                    }
                    else
                    {
                        if (cloverResponse == null)
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(2273));
                            log.Error("Error occured during the payment.");
                            throw new Exception(utilities.MessageUtils.getMessage(2273));
                        }
                        else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO == null)
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("No response. ") + utilities.MessageUtils.getMessage(cloverResponse.ResponseMessage));
                            log.Error("No response. " + cloverResponse.ResponseMessage);
                            throw new Exception("No response." + cloverResponse.ResponseMessage);
                        }
                        else if (cloverResponse != null && cloverResponse.CCTransactionsPGWDTO != null)
                        {
                            if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.InvoiceNo))
                            {
                                cloverResponse.CCTransactionsPGWDTO.InvoiceNo = ccOrigTransactionsPGWDTO.InvoiceNo;
                            }
                            if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.RecordNo))
                            {
                                cloverResponse.CCTransactionsPGWDTO.RecordNo = " ";
                            }
                            if (cloverResponse.CCTransactionsPGWDTO.TranCode == PaymentGatewayTransactionType.AUTHORIZATION.ToString())
                            {
                                cloverResponse.CCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                            }
                            cloverResponse.CCTransactionsPGWDTO.RefNo = ccOrigTransactionsPGWDTO.RefNo;
                            if (string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.Authorize))
                            {
                                cloverResponse.CCTransactionsPGWDTO.Authorize = ccOrigTransactionsPGWDTO.Authorize;
                            }
                            SaveTransactionResponse(cloverResponse.CCTransactionsPGWDTO);
                            if (cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode.Equals("SUCCESS"))
                            {
                                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage(((!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse)) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                                if (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AcctNo))
                                {
                                    transactionPaymentsDTO.CreditCardNumber = cloverResponse.CCTransactionsPGWDTO.AcctNo;
                                }
                                if (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.CardType))
                                {
                                    transactionPaymentsDTO.CreditCardName = cloverResponse.CCTransactionsPGWDTO.CardType;
                                }
                                if (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AcqRefData))
                                {
                                    transactionPaymentsDTO.NameOnCreditCard = cloverResponse.CCTransactionsPGWDTO.AcqRefData;
                                }
                                if (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.AuthCode))
                                {
                                    transactionPaymentsDTO.CreditCardAuthorization = cloverResponse.CCTransactionsPGWDTO.AuthCode;
                                }
                                if (!string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.Authorize))
                                {
                                    transactionPaymentsDTO.Amount = Convert.ToDouble(cloverResponse.CCTransactionsPGWDTO.Authorize) * 0.01;
                                }
                                transactionPaymentsDTO.CCResponseId = cloverResponse.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.Reference = cloverResponse.CCTransactionsPGWDTO.InvoiceNo;
                            }
                            else
                            {
                                log.Error(cloverResponse.ResponseMessage);
                                throw new Exception(utilities.MessageUtils.getMessage(cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode) + utilities.MessageUtils.getMessage((string.IsNullOrEmpty(cloverResponse.CCTransactionsPGWDTO.TextResponse) ? "" : "-" + cloverResponse.CCTransactionsPGWDTO.TextResponse)));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while performing settlement", ex);
                    log.LogMethodExit(null, "Throwing Exception " + ex);
                    throw (ex);
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
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

            if (transactionPaymentsDTO != null)
            {
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                string limit = utilities.getParafaitDefaults("MAX_TIP_AMOUNT_PERCENTAGE");
                long tipLimit = Convert.ToInt64(string.IsNullOrEmpty(limit) ? "200" : limit);
                if (tipLimit > 0 && ((transactionPaymentsDTO.Amount * tipLimit) / 100) < transactionPaymentsDTO.TipAmount)
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Tip limit exceeded"));
                }
                if (!ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()) && !ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()) && !ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                {
                    log.Info("Tip adjustment is not allowed for the transaction type " + ccOrigTransactionsPGWDTO.TranCode);
                    throw new Exception(utilities.MessageUtils.getMessage(2275, ccOrigTransactionsPGWDTO.TranCode));
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
