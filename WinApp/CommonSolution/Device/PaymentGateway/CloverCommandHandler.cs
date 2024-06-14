/********************************************************************************************
 * Project Name - Clover Command Handler
 * Description  - Data handler of the CloverPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        12-AUG-2019   Raghuveera      Created  
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using Semnox.Core.Utilities;
using System;
using System.Threading;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class CloverCommandHandler : ICloverConnectorListener
    {

        ICloverConnector cloverConnector;
        ShowMessageDelegate showMessageDelegate;
        public delegate void DisplayText(string text);
        public DisplayText displayText;        
        public delegate void ParingResponse(bool status, CloverResponse cloverResponse);
        ParingResponse ParingResponseCall;
        public delegate void TransactionResponse(CloverResponse cloverResponse);
        TransactionResponse SendTransactionResponse;
        public delegate void DeviceError(CloverResponse cloverResponse);
        public DeviceError deviceError;
        WebSocketCloverDeviceConfiguration WebSocketConfig; // set the 3 delegates in the ctor
        bool isManualEntryEnabled;
        bool isAutoAcceptSignature;
        bool isUnattended;
        bool isPrintCustomerCopy;
        private string paringToken;
        private string url;
        private string applicationName;
        private string posName;
        Utilities utils; 
        public Utilities utilities { set { utils = value; } }
        public bool IsManualEntryEnabled { set { isManualEntryEnabled = value; } }
        public bool IsAutoAcceptSignature { set { isAutoAcceptSignature = value; } }
        public bool IsPrintCustomerCopy { set { isPrintCustomerCopy = value; } }
        bool isDeviceConnected;
        public bool IsDeviceConnected { get { return isDeviceConnected; }  set { isDeviceConnected = value; } }

        ManualResetEvent eventCapture = new ManualResetEvent(false);
        public int CardEntryMethod
        {
            get
            {
                int value = 0;

                if (isManualEntryEnabled)
                {
                    value |= CloverConnector.CARD_ENTRY_METHOD_MANUAL;
                }
                value |= CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE;
                value |= CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT;
                value |= CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS;
                return value;
            }
        }

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CloverCommandHandler(string url, string applicationName, string posName, string paringToken, ShowMessageDelegate showMessageDelegate)
        {
            log.LogMethodEntry(url, applicationName, posName);
            //url="wss://192.168.1.14:12345/remote_pay"

            this.showMessageDelegate = showMessageDelegate;
            this.paringToken = paringToken;
            this.applicationName = applicationName;
            this.posName = posName;
            this.url = url;

            WebSocketConfig = new WebSocketCloverDeviceConfiguration(url, applicationName + " 2.70.2", true, 1, applicationName, posName, paringToken, null, null, null);
            WebSocketConfig.OnPairingCode = OnPairingCode;
            WebSocketConfig.OnPairingSuccess = OnPairingSuccess;
            WebSocketConfig.OnPairingState = OnPairingState;
            
            log.LogMethodExit();
        }
        ~CloverCommandHandler()
        {
            log.LogMethodEntry();
            if (cloverConnector != null)
            {
                cloverConnector.RemoveCloverConnectorListener(this);
                OnDeviceDisconnected();
                cloverConnector.Dispose();
            }
            log.LogMethodExit();
        }

        public void ProcessDevicePairingRequest(ParingResponse paringResponse)
        {
            log.LogMethodEntry();
            if (paringResponse != null)
            {
                ParingResponseCall = paringResponse;
            }
            cloverConnector = CloverConnectorFactory.createICloverConnector(WebSocketConfig);
            cloverConnector.AddCloverConnectorListener(this);
            cloverConnector.InitializeConnection();
            log.LogMethodExit();
        }
        public void ProcessRequest(CloverRequest cloverRequest, TransactionResponse transactionResponse)
        {
            log.LogMethodEntry(cloverRequest, transactionResponse);
            eventCapture = new ManualResetEvent(false);
            isUnattended = cloverRequest.IsUnattended;
            cloverConnector.RetrieveDeviceStatus(new RetrieveDeviceStatusRequest(true));
            eventCapture.WaitOne(1500);
            if (!isDeviceConnected && !isUnattended)
            {                
                WebSocketConfig = new WebSocketCloverDeviceConfiguration(url, applicationName + " 2.70.2", true, 1, applicationName, posName, paringToken, null, null, null);
                WebSocketConfig.OnPairingCode = OnPairingCode;
                WebSocketConfig.OnPairingSuccess = OnPairingSuccess;
                WebSocketConfig.OnPairingState = OnPairingState;
                ProcessDevicePairingRequest(null);
            }
            eventCapture = new ManualResetEvent(false);
            isUnattended = cloverRequest.IsUnattended;
            cloverConnector.RetrieveDeviceStatus(new RetrieveDeviceStatusRequest(true));
            eventCapture.WaitOne(1500);
            if (!isDeviceConnected)
            {
                log.LogMethodExit("Device not connected. Please try again.");
                throw new Exception(utils.MessageUtils.getMessage(2277));
            }
            this.SendTransactionResponse = transactionResponse;
            switch (cloverRequest.TrxType)
            {
                case PaymentGatewayTransactionType.SALE:
                    SaleRequest saleRequest = new SaleRequest();
                    saleRequest.Amount = Convert.ToInt64(cloverRequest.Amount * 100);
                    saleRequest.ExternalId = cloverRequest.ReferenceId;
                    saleRequest.CardEntryMethods = CardEntryMethod;
                    saleRequest.DisableRestartTransactionOnFail = true;
                    saleRequest.DisablePrinting = (isUnattended)? true:!isPrintCustomerCopy;
                    saleRequest.DisableReceiptSelection = (isUnattended) ? true : !isPrintCustomerCopy;//!isPrintCustomerCopy;
                    saleRequest.AutoAcceptSignature = isAutoAcceptSignature;
                    //saleRequest.TipMode = TipMode.NO_TIP;
                    if (!string.IsNullOrEmpty(cloverRequest.TokenId))
                    {
                        saleRequest.VaultedCard = new com.clover.sdk.v3.payments.VaultedCard();
                        saleRequest.VaultedCard.cardholderName = cloverRequest.CardHolderName;
                        saleRequest.VaultedCard.first6 = (!string.IsNullOrEmpty(cloverRequest.AccountNo) ? cloverRequest.AccountNo.Substring(0, 6) : null);
                        saleRequest.VaultedCard.last4 = (!string.IsNullOrEmpty(cloverRequest.AccountNo) ? cloverRequest.AccountNo.Substring(cloverRequest.AccountNo.Length - 5, 0) : null);
                        saleRequest.VaultedCard.expirationDate = cloverRequest.CardExpiryDate;
                        saleRequest.VaultedCard.token = cloverRequest.TokenId;
                    }
                    log.LogVariableState("saleRequest", saleRequest);
                    cloverConnector.Sale(saleRequest);
                    break;
                case PaymentGatewayTransactionType.REFUND:
                    ManualRefundRequest manualRefundRequest = new ManualRefundRequest();
                    manualRefundRequest.DisablePrinting = (isUnattended) ? true : !isPrintCustomerCopy;//!isPrintCustomerCopy;
                    manualRefundRequest.DisableReceiptSelection = (isUnattended) ? true : !isPrintCustomerCopy; //!isPrintCustomerCopy;
                    manualRefundRequest.DisablePrinting = false;
                    manualRefundRequest.Amount = Convert.ToInt64(cloverRequest.Amount * 100);
                    manualRefundRequest.CardEntryMethods = CardEntryMethod;
                    manualRefundRequest.ExternalId = cloverRequest.ReferenceId;
                    manualRefundRequest.Type = (cloverRequest.OrgTrxType.Equals(PaymentGatewayTransactionType.AUTHORIZATION) || cloverRequest.OrgTrxType.Equals(PaymentGatewayTransactionType.CAPTURE)) ? PayIntent.TransactionType.AUTH : PayIntent.TransactionType.PAYMENT;
                    if (!string.IsNullOrEmpty(cloverRequest.TokenId))
                    {
                        //manualRefundRequest.CardNotPresent = true;
                        manualRefundRequest.VaultedCard = new com.clover.sdk.v3.payments.VaultedCard();
                        manualRefundRequest.VaultedCard.cardholderName = cloverRequest.CardHolderName;
                        manualRefundRequest.VaultedCard.first6 = (!string.IsNullOrEmpty(cloverRequest.AccountNo) ? cloverRequest.AccountNo.Substring(0, 6) : null);
                        manualRefundRequest.VaultedCard.last4 = (!string.IsNullOrEmpty(cloverRequest.AccountNo) ? cloverRequest.AccountNo.Substring(cloverRequest.AccountNo.Length - 5, 0) : null);
                        manualRefundRequest.VaultedCard.expirationDate = cloverRequest.CardExpiryDate;
                        manualRefundRequest.VaultedCard.token = cloverRequest.TokenId;
                    }
                    log.LogVariableState("manualRefundRequest", manualRefundRequest);
                    cloverConnector.ManualRefund(manualRefundRequest);
                    //RefundPaymentRequest refundPaymentRequest = new RefundPaymentRequest();
                    //refundPaymentRequest.Amount = Convert.ToInt64(cloverRequest.Amount * 100);
                    //refundPaymentRequest.DisablePrinting = false;
                    //refundPaymentRequest.PaymentId = cloverRequest.ReferenceId;
                    //refundPaymentRequest.FullRefund = cloverRequest.IsFullRefund;
                    //refundPaymentRequest.OrderId = cloverRequest.OrderId;
                    //refundPaymentRequest.DisablePrinting = (isUnattended) ? true : !isPrintCustomerCopy;//!isPrintCustomerCopy;
                    //refundPaymentRequest.DisableReceiptSelection = (isUnattended) ? true : !isPrintCustomerCopy; //!isPrintCustomerCopy;
                    //cloverConnector.RefundPayment(refundPaymentRequest);
                    break;
                case PaymentGatewayTransactionType.VOID:
                    VoidPaymentRequest voidPaymentRequest = new VoidPaymentRequest();
                    voidPaymentRequest.PaymentId = cloverRequest.ReferenceId;
                    voidPaymentRequest.VoidReason = "USER_CANCEL";
                    log.LogVariableState("voidPaymentRequest", voidPaymentRequest);
                    cloverConnector.VoidPayment(voidPaymentRequest);
                    break;
                case PaymentGatewayTransactionType.CAPTURE://there is no capture feature in clover. But the ui calls Tip adjustment
                    break;
                case PaymentGatewayTransactionType.AUTHORIZATION:
                    AuthRequest authRequest = new AuthRequest();
                    authRequest.Amount = Convert.ToInt64(cloverRequest.Amount * 100);
                    authRequest.ExternalId = cloverRequest.ReferenceId;
                    authRequest.CardEntryMethods = CardEntryMethod;
                    authRequest.DisableRestartTransactionOnFail = true;
                    authRequest.DisablePrinting = (isUnattended) ? true : !isPrintCustomerCopy;//!isPrintCustomerCopy;
                    authRequest.DisableReceiptSelection = (isUnattended) ? true : !isPrintCustomerCopy;//!isPrintCustomerCopy;
                    authRequest.AutoAcceptSignature = isAutoAcceptSignature;
                    if (!string.IsNullOrEmpty(cloverRequest.TokenId))
                    {
                        authRequest.VaultedCard = new com.clover.sdk.v3.payments.VaultedCard();
                        authRequest.VaultedCard.cardholderName = cloverRequest.CardHolderName;
                        authRequest.VaultedCard.first6 = (!string.IsNullOrEmpty(cloverRequest.AccountNo) ? cloverRequest.AccountNo.Substring(0, 6) : null);
                        authRequest.VaultedCard.last4 = (!string.IsNullOrEmpty(cloverRequest.AccountNo) ? cloverRequest.AccountNo.Substring(cloverRequest.AccountNo.Length - 5, 0) : null);
                        authRequest.VaultedCard.expirationDate = cloverRequest.CardExpiryDate;
                        authRequest.VaultedCard.token = cloverRequest.TokenId;
                    }
                    log.LogVariableState("authRequest", authRequest);
                    cloverConnector.Auth(authRequest);
                    break;
                case PaymentGatewayTransactionType.TATokenRequest:
                    cloverConnector.VaultCard(CardEntryMethod);
                    break;
                case PaymentGatewayTransactionType.TIPADJUST:
                    TipAdjustAuthRequest taRequest = new TipAdjustAuthRequest();
                    taRequest.PaymentID = cloverRequest.ReferenceId;
                    taRequest.OrderID = cloverRequest.OrderId;
                    taRequest.TipAmount = Convert.ToInt64(cloverRequest.TipAmount * 100);
                    log.LogVariableState("TipAdjustAuthRequest", taRequest);
                    cloverConnector.TipAdjustAuth(taRequest);
                    break;
                case PaymentGatewayTransactionType.STATUSCHECK:
                    RetrievePaymentRequest retrievePaymentRequest = new RetrievePaymentRequest(cloverRequest.ReferenceId);
                    log.LogVariableState("retrievePaymentRequest", retrievePaymentRequest);
                    cloverConnector.RetrievePayment(retrievePaymentRequest);
                    break;
                case PaymentGatewayTransactionType.USERCANCEL:
                    CancelRequest();
                    break;
            }
            log.LogMethodExit();

        }

        private void CancelRequest()
        {
            log.LogMethodEntry();
            cloverConnector.InvokeInputOption(new InputOption() { keyPress = KeyPress.ESC, description = "Cancel" });
            //cloverConnector.ShowWelcomeScreen();
            log.LogMethodExit();
        }
        public void OnPairingCode(string pairingCode)
        {
            log.LogMethodEntry(pairingCode);
            displayText(utils.MessageUtils.getMessage(2278, pairingCode )+ ".");
            log.LogMethodExit();
        }
        public void OnPairingSuccess(string pairingAuthToken)
        {
            log.LogMethodEntry(pairingAuthToken);
            paringToken = pairingAuthToken;
            CloverResponse cloverResponse = new CloverResponse();
            cloverResponse.CCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cloverResponse.CCTransactionsPGWDTO.TokenID = pairingAuthToken;
            cloverResponse.CCTransactionsPGWDTO.TranCode = "PARING";
            cloverResponse.CCTransactionsPGWDTO.TextResponse = "Paring success.";
            cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode = "0";
            cloverResponse.CCTransactionsPGWDTO.TransactionDatetime = ServerDateTime.Now;
            displayText(utils.MessageUtils.getMessage("Pairing done!."));
            ParingResponseCall(true, cloverResponse);
            log.LogMethodExit();
        }

        public void OnPairingState(string state, string message)
        {
            log.LogMethodEntry(state, message);
            if (state == "AUTHENTICATING")
            {
                displayText(utils.MessageUtils.getMessage("Pairing Security Pin ") + utils.MessageUtils.getMessage(message));
            }
            log.LogMethodExit();
        }
        public void OnAuthResponse(AuthResponse authResponse)
        {
            log.LogMethodEntry(authResponse);
            CloverResponse cloverResponse = new CloverResponse();
            cloverResponse.CCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cloverResponse.CCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.AUTHORIZATION.ToString();
            cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode = authResponse.Result.ToString();
            cloverResponse.CCTransactionsPGWDTO.TextResponse = authResponse.Message;
            cloverResponse.ResponseMessage = authResponse.Message;

            if (authResponse.Payment != null)
            {
                cloverResponse.CCTransactionsPGWDTO.Authorize = authResponse.Payment.amount.ToString();
                if (authResponse.Payment.order != null)
                {
                    cloverResponse.CCTransactionsPGWDTO.RefNo = authResponse.Payment.order.id;
                }
                cloverResponse.CCTransactionsPGWDTO.InvoiceNo = authResponse.Payment.externalPaymentId;
                if (authResponse.Payment.cardTransaction != null)
                {
                    cloverResponse.CCTransactionsPGWDTO.AcctNo = authResponse.Payment.cardTransaction.first6 + authResponse.Payment.cardTransaction.last4.PadLeft(10, 'X');
                    cloverResponse.CCTransactionsPGWDTO.AuthCode = authResponse.Payment.cardTransaction.authCode;
                    cloverResponse.CCTransactionsPGWDTO.TokenID = authResponse.Payment.cardTransaction.token;
                    cloverResponse.CCTransactionsPGWDTO.CaptureStatus = authResponse.Payment.cardTransaction.entryType.ToString();
                    cloverResponse.CCTransactionsPGWDTO.AcqRefData = authResponse.Payment.cardTransaction.cardholderName;
                    cloverResponse.CCTransactionsPGWDTO.UserTraceData = authResponse.Payment.cardTransaction.cardType.ToString();
                    cloverResponse.CCTransactionsPGWDTO.ProcessData = authResponse.Payment.id + "|" + authResponse.Payment.cardTransaction.referenceId + "|" + authResponse.Payment.cardTransaction.paymentRef+"|"+ authResponse.Payment.externalReferenceId;
                    cloverResponse.CCTransactionsPGWDTO.TipAmount = authResponse.Payment.tipAmount.ToString();
                    if (authResponse.Payment.cardTransaction.vaultedCard != null)
                    {
                        cloverResponse.CCTransactionsPGWDTO.RecordNo = authResponse.Payment.cardTransaction.vaultedCard.expirationDate;
                    }
                    else
                    {
                        cloverResponse.CCTransactionsPGWDTO.RecordNo = "";
                    }
                }
            }
            SendTransactionResponse(cloverResponse);
            log.LogMethodExit();
        }
        public void OnCapturePreAuthResponse(CapturePreAuthResponse capturePreAuthResponse)
        {
            log.LogMethodEntry(capturePreAuthResponse);
            log.LogMethodExit();
        }
        public void OnCloseoutResponse(CloseoutResponse closeoutResponse)
        {
            log.LogMethodEntry(closeoutResponse);
            log.LogMethodExit();
        }
        public void OnConfirmPaymentRequest(ConfirmPaymentRequest confirmPaymentRequest)
        {
            log.LogMethodEntry(confirmPaymentRequest);
            string title = "";
            string message = "";
            displayText(utils.MessageUtils.getMessage("Confirm the payment."));
            if (confirmPaymentRequest.Challenges != null && confirmPaymentRequest.Challenges.Count > 0)
            {
                for (int i = 0; i < confirmPaymentRequest.Challenges.Count; i++)
                {
                    message = confirmPaymentRequest.Challenges[i].message;

                    // Customize title
                    switch (confirmPaymentRequest.Challenges[i].type)
                    {
                        case ChallengeType.DUPLICATE_CHALLENGE:
                            title = "Confirm Payment - Duplicate Payment";
                            break;
                        case ChallengeType.OFFLINE_CHALLENGE:
                            title = "Confirm Payment - Offline Payment";
                            break;
                        default:
                            title = "Confirm Payment";
                            break;
                    }
                    if (isUnattended)
                    {
                        log.Info(message + " is accepted since unattended.");
                        cloverConnector.AcceptPayment(confirmPaymentRequest.Payment);
                    }
                    else
                    {
                        if (showMessageDelegate(utils.MessageUtils.getMessage(message), title, System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Yes)
                        {
                            log.Info(message + " is accepted.");
                            cloverConnector.AcceptPayment(confirmPaymentRequest.Payment);
                        }
                        else
                        {
                            log.Info(message + " is rejected.");
                            cloverConnector.RejectPayment(confirmPaymentRequest.Payment, confirmPaymentRequest.Challenges[i]);
                        }
                    }
                }

            }
            log.LogMethodExit();
        }
        public virtual void OnCustomActivityResponse(CustomActivityResponse customActivityResponse)
        {
            log.LogMethodEntry(customActivityResponse);
            log.LogMethodExit();
        }
        public void OnCustomerProvidedData(CustomerProvidedDataEvent customerProvidedDataEvent)
        {
            log.LogMethodEntry(customerProvidedDataEvent);
            log.LogMethodExit();
        }
        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            log.LogMethodEntry(deviceEvent);
            log.Debug("Ends:" + deviceEvent.Code + "-" + deviceEvent.EventState.ToString() + "-" + deviceEvent.Message);
            log.LogMethodExit();
        }
        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            log.LogMethodEntry(deviceEvent);
            if (!string.IsNullOrEmpty(deviceEvent.Message))
            {
                displayText(utils.MessageUtils.getMessage(deviceEvent.Message));
            }
            log.Debug("Starts:" + deviceEvent.Code + "-" + deviceEvent.EventState.ToString() + "-" + deviceEvent.Message);
            log.LogMethodExit();
        }
        public void OnDeviceConnected()
        {
            log.LogMethodEntry();
            isDeviceConnected = true;
            log.Debug("Device connected.");
            displayText(utils.MessageUtils.getMessage("Device connected. Check the device screen for Passcode entry."));
            log.LogMethodExit();
        }
        public void OnDeviceDisconnected()
        {
            log.LogMethodEntry();
            try
            {
                isDeviceConnected = false;
                displayText(utils.MessageUtils.getMessage("Disconnected"));
                CloverResponse cloverResponse = new CloverResponse();
                cloverResponse.ResponseMessage = utils.MessageUtils.getMessage("Device disconnected");
                deviceError(cloverResponse);
                log.Debug("Device disconnected.");
            }
            catch
            {
                log.Error("Error occured during device disconnection");
            }
            log.LogMethodExit();
        }
        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            log.LogMethodEntry(deviceErrorEvent);
            displayText(utils.MessageUtils.getMessage(deviceErrorEvent.Message));//+ "-" + deviceErrorEvent.Code
            log.Error(deviceErrorEvent.Message + "-" + deviceErrorEvent.Code);
            CloverResponse cloverResponse = new CloverResponse();
            cloverResponse.ResponseMessage = deviceErrorEvent.Message;
            deviceError(cloverResponse);
            log.LogMethodExit();
        }
        public void OnDeviceReady(MerchantInfo merchantInfo)
        {
            log.LogMethodEntry(merchantInfo);
            isDeviceConnected = true;
            log.Debug("Device ready");
            log.LogMethodExit();
        }
        public void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response)
        {
            log.LogMethodEntry(response);
            log.LogMethodExit();
        }
        public void OnInvalidStateTransitionResponse(InvalidStateTransitionNotification invalidStateTransitionNotification)
        {
            log.LogMethodEntry(invalidStateTransitionNotification);
            log.LogMethodExit();
        }
        public void OnManualRefundResponse(ManualRefundResponse manualRefundResponse)
        {
            log.LogMethodEntry(manualRefundResponse);
            log.LogVariableState("ManualRefundResponse", manualRefundResponse);
            CloverResponse cloverResponse = new CloverResponse();
            cloverResponse.CCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cloverResponse.CCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
            cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode = manualRefundResponse.Result.ToString();
            cloverResponse.CCTransactionsPGWDTO.TextResponse = manualRefundResponse.Message;
            cloverResponse.ResponseMessage = manualRefundResponse.Message;

            if (manualRefundResponse.Credit != null)
            {
                if (manualRefundResponse.Credit.orderRef != null)
                {
                    cloverResponse.CCTransactionsPGWDTO.RefNo = manualRefundResponse.Credit.orderRef.id;
                }
                cloverResponse.CCTransactionsPGWDTO.Authorize = manualRefundResponse.Credit.amount.ToString();
                //cloverResponse.CCTransactionsPGWDTO.InvoiceNo = manualRefundResponse.Credit.externalPaymentId;
                if (manualRefundResponse.Credit.cardTransaction != null)
                {
                    cloverResponse.CCTransactionsPGWDTO.AcctNo = manualRefundResponse.Credit.cardTransaction.first6 + manualRefundResponse.Credit.cardTransaction.last4.PadLeft(10, 'X');
                    cloverResponse.CCTransactionsPGWDTO.AuthCode = manualRefundResponse.Credit.cardTransaction.authCode;
                    cloverResponse.CCTransactionsPGWDTO.TokenID = manualRefundResponse.Credit.cardTransaction.token;
                    cloverResponse.CCTransactionsPGWDTO.CaptureStatus = manualRefundResponse.Credit.cardTransaction.entryType.ToString();
                    cloverResponse.CCTransactionsPGWDTO.AcqRefData = manualRefundResponse.Credit.cardTransaction.cardholderName;
                    cloverResponse.CCTransactionsPGWDTO.UserTraceData = manualRefundResponse.Credit.cardTransaction.cardType.ToString();
                    cloverResponse.CCTransactionsPGWDTO.ProcessData = manualRefundResponse.Credit.id + "|" + manualRefundResponse.Credit.cardTransaction.referenceId + "|" + manualRefundResponse.Credit.cardTransaction.paymentRef;
                    
                    if (manualRefundResponse.Credit.cardTransaction.vaultedCard != null)
                    {
                        cloverResponse.CCTransactionsPGWDTO.RecordNo = manualRefundResponse.Credit.cardTransaction.vaultedCard.expirationDate;
                    }
                    else
                    {
                        cloverResponse.CCTransactionsPGWDTO.RecordNo = "";
                    }
                }
            }
            SendTransactionResponse(cloverResponse);
            log.LogMethodExit();
        }
        public void OnMessageFromActivity(MessageFromActivity messageFromActivity)
        {
            log.LogMethodEntry(messageFromActivity);
            log.LogMethodExit();
        }
        public void OnPreAuthResponse(PreAuthResponse preAuthResponse)
        {
            log.LogMethodEntry(preAuthResponse);
            log.LogMethodExit();
        }
        public void OnPrintJobStatusRequest(PrintJobStatusRequest printJobStatusRequest)
        {
            log.LogMethodEntry(printJobStatusRequest);
            log.LogMethodExit();
        }
        public void OnPrintJobStatusResponse(PrintJobStatusResponse printJobStatusResponse)
        {
            log.LogMethodEntry(printJobStatusResponse);
            log.LogMethodExit();
        }
        public void OnPrintManualRefundDeclineReceipt(PrintManualRefundDeclineReceiptMessage printManualRefundDeclineReceiptMessage)
        {
            log.LogMethodEntry(printManualRefundDeclineReceiptMessage);
            log.LogMethodExit();
        }
        public void OnPrintManualRefundReceipt(PrintManualRefundReceiptMessage printManualRefundReceiptMessage)
        {
            log.LogMethodEntry(printManualRefundReceiptMessage);
            log.LogMethodExit();
        }
        public void OnPrintPaymentDeclineReceipt(PrintPaymentDeclineReceiptMessage printPaymentDeclineReceiptMessage)
        {
            log.LogMethodEntry(printPaymentDeclineReceiptMessage);
            log.LogMethodExit();
        }
        public void OnPrintPaymentMerchantCopyReceipt(PrintPaymentMerchantCopyReceiptMessage printPaymentMerchantCopyReceiptMessage)
        {
            log.LogMethodEntry(printPaymentMerchantCopyReceiptMessage);
            log.LogMethodExit();
        }
        public void OnPrintPaymentReceipt(PrintPaymentReceiptMessage printPaymentReceiptMessage)
        {
            log.LogMethodEntry(printPaymentReceiptMessage);
            log.LogMethodExit();
        }
        public void OnPrintRefundPaymentReceipt(PrintRefundPaymentReceiptMessage printRefundPaymentReceiptMessage)
        {
            log.LogMethodEntry(printRefundPaymentReceiptMessage);
            log.LogMethodExit();
        }
        public void OnReadCardDataResponse(ReadCardDataResponse response)
        {
            log.LogMethodEntry(response);
            log.LogMethodExit();
        }
        public void OnRefundPaymentResponse(RefundPaymentResponse refundPaymentResponse)
        {
            log.LogMethodEntry(refundPaymentResponse);
            log.LogVariableState("RefundPaymentResponse", refundPaymentResponse);
            CloverResponse cloverResponse = new CloverResponse();
            cloverResponse.CCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cloverResponse.CCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.VOID.ToString();
            cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode = refundPaymentResponse.Result.ToString();
            cloverResponse.CCTransactionsPGWDTO.TextResponse = refundPaymentResponse.Message;
            cloverResponse.ResponseMessage = refundPaymentResponse.Message;

            if (refundPaymentResponse.Refund != null)
            {
                cloverResponse.CCTransactionsPGWDTO.Authorize = refundPaymentResponse.Refund.amount.ToString();
                cloverResponse.CCTransactionsPGWDTO.RefNo = refundPaymentResponse.Refund.id;
                cloverResponse.CCTransactionsPGWDTO.InvoiceNo = refundPaymentResponse.PaymentId;                
            }
            SendTransactionResponse(cloverResponse);
            log.LogMethodExit();
        }
        public virtual void OnResetDeviceResponse(ResetDeviceResponse resetDeviceResponse)
        {
            log.LogMethodEntry(resetDeviceResponse);
            log.LogMethodExit();
        }
        public virtual void OnRetrieveDeviceStatusResponse(RetrieveDeviceStatusResponse retrieveDeviceStatusResponse)
        {
            log.LogMethodEntry(retrieveDeviceStatusResponse);
            if(retrieveDeviceStatusResponse.State.ToString().Equals("IDLE"))
            {
                isDeviceConnected = true;
            }
            eventCapture.Set();
            log.LogMethodExit();
        }
        public void OnRetrievePaymentResponse(RetrievePaymentResponse retrievePaymentResponse)
        {
            log.LogMethodEntry(retrievePaymentResponse);
            CloverResponse cloverResponse = new CloverResponse();
            cloverResponse.CCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cloverResponse.CCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();

            cloverResponse.CCTransactionsPGWDTO.TextResponse = retrievePaymentResponse.Result.ToString() + "-" + (string.IsNullOrEmpty(retrievePaymentResponse.Message)? retrievePaymentResponse.Reason : retrievePaymentResponse.Message);
            cloverResponse.ResponseMessage = (string.IsNullOrEmpty(retrievePaymentResponse.Message) ? retrievePaymentResponse.Reason : retrievePaymentResponse.Message);

            if (retrievePaymentResponse.Payment != null)
            {
                cloverResponse.CCTransactionsPGWDTO.Authorize = retrievePaymentResponse.Payment.amount.ToString();
                cloverResponse.CCTransactionsPGWDTO.RefNo = retrievePaymentResponse.Payment.externalReferenceId;
                cloverResponse.CCTransactionsPGWDTO.InvoiceNo = retrievePaymentResponse.Payment.externalPaymentId;
                if (retrievePaymentResponse.Payment.cardTransaction != null)
                {
                    cloverResponse.CCTransactionsPGWDTO.AcctNo = retrievePaymentResponse.Payment.cardTransaction.first6 + retrievePaymentResponse.Payment.cardTransaction.last4.PadLeft(10, 'X');
                    cloverResponse.CCTransactionsPGWDTO.AuthCode = retrievePaymentResponse.Payment.cardTransaction.authCode;
                    cloverResponse.CCTransactionsPGWDTO.TokenID = retrievePaymentResponse.Payment.cardTransaction.token;
                    cloverResponse.CCTransactionsPGWDTO.CaptureStatus = retrievePaymentResponse.Payment.cardTransaction.entryType.ToString();
                    cloverResponse.CCTransactionsPGWDTO.AcqRefData = retrievePaymentResponse.Payment.cardTransaction.cardholderName;
                    cloverResponse.CCTransactionsPGWDTO.UserTraceData = retrievePaymentResponse.Payment.cardTransaction.cardType.ToString();
                    cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode = retrievePaymentResponse.Payment.cardTransaction.state.ToString();
                    cloverResponse.CCTransactionsPGWDTO.ProcessData = retrievePaymentResponse.Payment.id + "|"+retrievePaymentResponse.Payment.cardTransaction.referenceId + "|" + retrievePaymentResponse.Payment.cardTransaction.paymentRef;
                    cloverResponse.CCTransactionsPGWDTO.TipAmount = (retrievePaymentResponse.Payment.tipAmount==null)?"0":retrievePaymentResponse.Payment.tipAmount.ToString();
                    if (retrievePaymentResponse.Payment.cardTransaction.vaultedCard != null)
                    {
                        cloverResponse.CCTransactionsPGWDTO.RecordNo = retrievePaymentResponse.Payment.cardTransaction.vaultedCard.expirationDate;
                    }
                    else
                    {
                        cloverResponse.CCTransactionsPGWDTO.RecordNo = "";
                    }
                }
            }
            else
            {
                cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode = retrievePaymentResponse.QueryStatus.ToString();
                cloverResponse.CCTransactionsPGWDTO.InvoiceNo = retrievePaymentResponse.ExternalPaymentId.ToString();
                cloverResponse.CCTransactionsPGWDTO.RecordNo = "";
            }
            SendTransactionResponse(cloverResponse);
            log.LogMethodExit();
        }
        public void OnRetrievePendingPaymentsResponse(RetrievePendingPaymentsResponse retrievePendingPaymentsResponse)
        {
            log.LogMethodEntry(retrievePendingPaymentsResponse);
            log.LogMethodExit();
        }
        public void OnRetrievePrintersResponse(RetrievePrintersResponse retrievePrintersResponse)
        {
            log.LogMethodEntry(retrievePrintersResponse);
            log.LogMethodExit();
        }
        public void OnSaleResponse(SaleResponse saleResponse)
        {
            log.LogMethodEntry(saleResponse);
            log.LogVariableState("SaleResponse", saleResponse);
            CloverResponse cloverResponse = new CloverResponse();
            cloverResponse.CCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cloverResponse.CCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
            cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode = saleResponse.Result.ToString();
            cloverResponse.CCTransactionsPGWDTO.TextResponse = saleResponse.Message;
            cloverResponse.ResponseMessage = saleResponse.Message;
            
            if (saleResponse.Payment != null)
            {
                if (saleResponse.Payment.order != null)
                {
                    cloverResponse.CCTransactionsPGWDTO.RefNo = saleResponse.Payment.order.id;
                }
                cloverResponse.CCTransactionsPGWDTO.Authorize = saleResponse.Payment.amount.ToString();
                cloverResponse.CCTransactionsPGWDTO.InvoiceNo = saleResponse.Payment.externalPaymentId;
                if (saleResponse.Payment.cardTransaction != null)
                {
                    cloverResponse.CCTransactionsPGWDTO.AcctNo = saleResponse.Payment.cardTransaction.first6 + saleResponse.Payment.cardTransaction.last4.PadLeft(10, 'X');
                    cloverResponse.CCTransactionsPGWDTO.AuthCode = saleResponse.Payment.cardTransaction.authCode;
                    cloverResponse.CCTransactionsPGWDTO.TokenID = saleResponse.Payment.cardTransaction.token;
                    cloverResponse.CCTransactionsPGWDTO.CaptureStatus = saleResponse.Payment.cardTransaction.entryType.ToString();
                    cloverResponse.CCTransactionsPGWDTO.AcqRefData = saleResponse.Payment.cardTransaction.cardholderName;
                    cloverResponse.CCTransactionsPGWDTO.UserTraceData = saleResponse.Payment.cardTransaction.cardType.ToString();
                    cloverResponse.CCTransactionsPGWDTO.ProcessData = saleResponse.Payment.id +"|"+saleResponse.Payment.cardTransaction.referenceId + "|" + saleResponse.Payment.cardTransaction.paymentRef + "|" + saleResponse.Payment.externalReferenceId;
                    cloverResponse.CCTransactionsPGWDTO.TipAmount = saleResponse.Payment.tipAmount.ToString();

                    if (saleResponse.Payment.cardTransaction.vaultedCard != null)
                    {
                        cloverResponse.CCTransactionsPGWDTO.RecordNo = saleResponse.Payment.cardTransaction.vaultedCard.expirationDate;
                    }
                    else
                    {
                        cloverResponse.CCTransactionsPGWDTO.RecordNo = "";
                    }
                }
            }
            GenrateSalesReceipt(cloverResponse, saleResponse);
            SendTransactionResponse(cloverResponse);
            log.LogMethodExit();
        }
        private void GenrateSalesReceipt(CloverResponse cloverResponse, SaleResponse saleResponse)
        {
            log.LogMethodEntry(cloverResponse, saleResponse);
            try
            {
                if (saleResponse != null && !saleResponse.Result.Equals(com.clover.remotepay.sdk.ResponseCode.CANCEL))
                {
                    cloverResponse.CustomerReceiptText = "";
                    cloverResponse.CustomerReceiptText += CCGatewayUtils.AllignText(utils.ParafaitEnv.SiteName, Alignment.Center) + Environment.NewLine;
                    cloverResponse.CustomerReceiptText += Common.GetFormatedAddress(utils.ParafaitEnv.SiteAddress) + Environment.NewLine;
                    cloverResponse.CustomerReceiptText += Environment.NewLine;
                    cloverResponse.CustomerReceiptText += utils.getServerTime().ToString("dd-MMM-yyyy hh:mm:ss t") + Environment.NewLine;

                    if (saleResponse.Payment != null)
                    {
                        if (saleResponse.Payment.cardTransaction != null)
                        {
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Transaction") + " " + saleResponse.Payment.cardTransaction.transactionNo + Environment.NewLine;
                        }
                        cloverResponse.CustomerReceiptText += "1 " + utils.MessageUtils.getMessage("Manual Transaction") + "    " + (saleResponse.Payment.amount * 0.01).ToString(utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                        cloverResponse.CustomerReceiptText += Environment.NewLine;
                        cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Total") + "    : " + (saleResponse.Payment.amount * 0.01).ToString(utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                        if (saleResponse.Payment.tender != null)
                        {
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage(saleResponse.Payment.tender.label) +" "+ utils.MessageUtils.getMessage("Sale") + " " + (saleResponse.Payment.amount * 0.01).ToString(utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                        }
                        cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Station") + "   : " + utils.ParafaitEnv.POSMachine + Environment.NewLine;
                        cloverResponse.CustomerReceiptText += utils.getServerTime().ToString("dd-MMM-yyyy hh:mm:ss t") + Environment.NewLine;
                        if (saleResponse.Payment.cardTransaction != null)
                        {
                            cloverResponse.CustomerReceiptText += (saleResponse.Payment.amount * 0.01).ToString(utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + "|" + utils.MessageUtils.getMessage("Method") + ":" + saleResponse.Payment.cardTransaction.entryType + Environment.NewLine;
                            if (saleResponse.Payment.tender != null)
                            {
                                cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage(saleResponse.Payment.tender.label) + " " + saleResponse.Payment.cardTransaction.last4.PadLeft(16, 'X') + Environment.NewLine;
                            }
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage(saleResponse.Payment.cardTransaction.cardholderName) + Environment.NewLine;
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Ref #") +":"+ saleResponse.Payment.cardTransaction.referenceId + Environment.NewLine;
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Auth #") +":" +saleResponse.Payment.cardTransaction.authCode + Environment.NewLine;
                            if (saleResponse.Payment.cardTransaction.extra != null && saleResponse.Payment.cardTransaction.extra.ContainsKey("authorizingNetworkName"))
                            {
                                cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("AthNtwkNm") + "  :" + saleResponse.Payment.cardTransaction.extra["authorizingNetworkName"] + Environment.NewLine;
                            }
                        }
                        cloverResponse.CustomerReceiptText += Environment.NewLine;
                        if (saleResponse.Payment.order != null)
                        {
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Order") + " " + saleResponse.Payment.order.id + Environment.NewLine;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Receipt generation error", ex);
            }
            log.LogMethodExit(cloverResponse);
        }
        public void OnTipAdded(TipAddedMessage tipAddedMessage)
        {
            log.LogMethodEntry(tipAddedMessage);
            log.Debug("Tip amount " + tipAddedMessage.tipAmount + " added.");
            displayText(utils.MessageUtils.getMessage("Tip amount ") + tipAddedMessage.tipAmount + utils.MessageUtils.getMessage(" is added."));
            log.LogMethodExit();
        }
        public void OnTipAdjustAuthResponse(TipAdjustAuthResponse tipAdjustAuthResponse)
        {
            log.LogMethodEntry(tipAdjustAuthResponse);
            CloverResponse cloverResponse = new CloverResponse();
            cloverResponse.CCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cloverResponse.CCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.TIPADJUST.ToString();
            cloverResponse.CCTransactionsPGWDTO.TextResponse = tipAdjustAuthResponse.Message;
            cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode = tipAdjustAuthResponse.Result.ToString();
            cloverResponse.ResponseMessage = tipAdjustAuthResponse.Message;
            cloverResponse.CCTransactionsPGWDTO.TipAmount = tipAdjustAuthResponse.TipAmount.ToString();
            cloverResponse.CCTransactionsPGWDTO.RecordNo = tipAdjustAuthResponse.Reason;
            cloverResponse.CCTransactionsPGWDTO.ProcessData = tipAdjustAuthResponse.PaymentId+"|";
            SendTransactionResponse(cloverResponse);
            log.LogMethodExit();
        }
        public void OnVaultCardResponse(VaultCardResponse vaultCardResponse)
        {
            log.LogMethodEntry(vaultCardResponse);
            CloverResponse cloverResponse = new CloverResponse();
            cloverResponse.CCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cloverResponse.CCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.TATokenRequest.ToString();
            cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode = vaultCardResponse.Result.ToString();
            cloverResponse.CCTransactionsPGWDTO.TextResponse = vaultCardResponse.Message;
            cloverResponse.ResponseMessage = vaultCardResponse.Result.ToString();

            if (vaultCardResponse.Card != null)
            {
                cloverResponse.CCTransactionsPGWDTO.Authorize = "0";
                cloverResponse.CCTransactionsPGWDTO.AcctNo = vaultCardResponse.Card.first6 + vaultCardResponse.Card.last4.PadLeft(10, 'X');
                cloverResponse.CCTransactionsPGWDTO.TokenID = vaultCardResponse.Card.token;
                cloverResponse.CCTransactionsPGWDTO.AcqRefData = vaultCardResponse.Card.cardholderName;
                cloverResponse.CCTransactionsPGWDTO.TipAmount = "0";
                cloverResponse.CCTransactionsPGWDTO.RecordNo = vaultCardResponse.Card.expirationDate;
            }
            SendTransactionResponse(cloverResponse);
            log.LogMethodExit();
        }
        public void OnVerifySignatureRequest(VerifySignatureRequest verifySignatureRequest)
        {
            log.LogMethodEntry(verifySignatureRequest);
            if (isUnattended || isAutoAcceptSignature)
            {
                verifySignatureRequest.Accept();
            }
            else
            {
                frmSignatureCaptureUI sigForm = new frmSignatureCaptureUI();
                sigForm.VerifySignatureRequest = verifySignatureRequest;
                sigForm.ShowDialog();
            }
            log.LogMethodExit();
        }
        public void OnVoidPaymentRefundResponse(VoidPaymentRefundResponse voidPaymentRefundResponse)
        {
            log.LogMethodEntry(voidPaymentRefundResponse);
            log.LogMethodExit();
        }
        public void OnVoidPaymentResponse(VoidPaymentResponse voidPaymentResponse)
        {
            log.LogMethodEntry(voidPaymentResponse);
            log.LogVariableState("VoidPaymentResponse", voidPaymentResponse);
            CloverResponse cloverResponse = new CloverResponse();
            cloverResponse.CCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cloverResponse.CCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.VOID.ToString();
            cloverResponse.CCTransactionsPGWDTO.DSIXReturnCode = voidPaymentResponse.Result.ToString();
            cloverResponse.CCTransactionsPGWDTO.TextResponse = voidPaymentResponse.Message;
            cloverResponse.ResponseMessage = voidPaymentResponse.Message;

            if (voidPaymentResponse.Payment != null)
            {
                cloverResponse.CCTransactionsPGWDTO.Authorize = voidPaymentResponse.Payment.amount.ToString();
                if (voidPaymentResponse.Payment.order != null)
                {
                    cloverResponse.CCTransactionsPGWDTO.RefNo = voidPaymentResponse.Payment.order.id;
                }
                cloverResponse.CCTransactionsPGWDTO.InvoiceNo = voidPaymentResponse.Payment.externalPaymentId;
                if (voidPaymentResponse.Payment.cardTransaction != null)
                {
                    cloverResponse.CCTransactionsPGWDTO.AcctNo = voidPaymentResponse.Payment.cardTransaction.first6 + voidPaymentResponse.Payment.cardTransaction.last4.PadLeft(10, 'X');
                    cloverResponse.CCTransactionsPGWDTO.AuthCode = voidPaymentResponse.Payment.cardTransaction.authCode;
                    cloverResponse.CCTransactionsPGWDTO.TokenID = voidPaymentResponse.Payment.cardTransaction.token;
                    cloverResponse.CCTransactionsPGWDTO.CaptureStatus = voidPaymentResponse.Payment.cardTransaction.entryType.ToString();
                    cloverResponse.CCTransactionsPGWDTO.AcqRefData = voidPaymentResponse.Payment.cardTransaction.cardholderName;
                    cloverResponse.CCTransactionsPGWDTO.UserTraceData = voidPaymentResponse.Payment.cardTransaction.cardType.ToString();
                    cloverResponse.CCTransactionsPGWDTO.ProcessData = voidPaymentResponse.Payment.id + "|" + voidPaymentResponse.Payment.cardTransaction.referenceId;

                    if(voidPaymentResponse.Payment.cardTransaction.paymentRef != null)
                    {
                        cloverResponse.CCTransactionsPGWDTO.ProcessData += "|" + voidPaymentResponse.Payment.cardTransaction.paymentRef.id + "|" + voidPaymentResponse.Payment.externalReferenceId;
                    }
                    
                    cloverResponse.CCTransactionsPGWDTO.TipAmount = voidPaymentResponse.Payment.tipAmount.ToString();
                    if (voidPaymentResponse.Payment.cardTransaction.vaultedCard != null)
                    {
                        cloverResponse.CCTransactionsPGWDTO.RecordNo = voidPaymentResponse.Payment.cardTransaction.vaultedCard.expirationDate;
                    }
                    else
                    {
                        cloverResponse.CCTransactionsPGWDTO.RecordNo = "";
                    }
                }
            }
            GenrateVoidReceipt(cloverResponse, voidPaymentResponse);
            SendTransactionResponse(cloverResponse);
            log.LogMethodExit();
        }
        private void GenrateVoidReceipt(CloverResponse cloverResponse, VoidPaymentResponse voidPaymentResponse)
        {
            log.LogMethodEntry(cloverResponse, voidPaymentResponse);
            try
            {
                if (voidPaymentResponse != null)// && !voidPaymentResponse.Result.Equals(com.clover.remotepay.sdk.ResponseCode.CANCEL)
                {
                    cloverResponse.CustomerReceiptText = "";
                    cloverResponse.CustomerReceiptText += CCGatewayUtils.AllignText(utils.ParafaitEnv.SiteName, Alignment.Center) + Environment.NewLine;
                    cloverResponse.CustomerReceiptText += Common.GetFormatedAddress(utils.ParafaitEnv.SiteAddress) + Environment.NewLine;
                    cloverResponse.CustomerReceiptText += Environment.NewLine;
                    cloverResponse.CustomerReceiptText += utils.getServerTime().ToString("dd-MMM-yyyy hh:mm:ss t") + Environment.NewLine;

                    if (voidPaymentResponse.Payment != null)
                    {
                        if (voidPaymentResponse.Payment.cardTransaction != null)
                        {
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Transaction") + " " + voidPaymentResponse.Payment.cardTransaction.transactionNo + Environment.NewLine;
                        }
                        cloverResponse.CustomerReceiptText += Environment.NewLine;
                        cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Total") + "    :" + (voidPaymentResponse.Payment.amount * 0.01).ToString(utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                        if (voidPaymentResponse.Payment.tender != null)
                        {
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage(voidPaymentResponse.Payment.tender.label)+" "+ utils.MessageUtils.getMessage("Refund")+" " + (voidPaymentResponse.Payment.amount * 0.01).ToString(utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                        }
                        cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Station") + "   : " + utils.ParafaitEnv.POSMachine + Environment.NewLine;
                        cloverResponse.CustomerReceiptText += utils.getServerTime().ToString("dd-MMM-yyyy hh:mm:ss t") + Environment.NewLine;
                        cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Refund Id") + "  : " + voidPaymentResponse.PaymentId + Environment.NewLine;
                        if (voidPaymentResponse.Payment.cardTransaction != null)
                        {
                            cloverResponse.CustomerReceiptText += (voidPaymentResponse.Payment.amount * 0.01).ToString(utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + "|" + utils.MessageUtils.getMessage("Method") + ":" + voidPaymentResponse.Payment.cardTransaction.entryType + Environment.NewLine;
                            if (voidPaymentResponse.Payment.tender != null)
                            {
                                cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage(voidPaymentResponse.Payment.tender.label) +" "+ voidPaymentResponse.Payment.cardTransaction.last4.PadLeft(16, 'X') + Environment.NewLine;
                            }
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage(voidPaymentResponse.Payment.cardTransaction.cardholderName) + Environment.NewLine;
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Ref #") + ":" + voidPaymentResponse.Payment.cardTransaction.referenceId + Environment.NewLine;
                            cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("Auth #") + ":" + voidPaymentResponse.Payment.cardTransaction.authCode + Environment.NewLine;
                            if (voidPaymentResponse.Payment.cardTransaction.extra != null && voidPaymentResponse.Payment.cardTransaction.extra.ContainsKey("authorizingNetworkName"))
                            {
                                cloverResponse.CustomerReceiptText += utils.MessageUtils.getMessage("AthNtwkNm") + "  :" + voidPaymentResponse.Payment.cardTransaction.extra["authorizingNetworkName"] + Environment.NewLine;
                            }
                        }
                        cloverResponse.CustomerReceiptText += Environment.NewLine;
                       
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Receipt generation error", ex);
            }
            log.LogMethodExit(cloverResponse);
        }

    }
    internal class CloverRequest
    {
        private string referenceId;
        private string orderId;
        private decimal amount;
        private decimal tipAmount;
        private PaymentGatewayTransactionType trxType;
        private PaymentGatewayTransactionType orgtrxType;
        private string tokenId;
        private string accountNo;
        private string cardHolderName;
        private string cardExpiryDate;
        private bool isFullRefund;
        private bool isUnattended;

        public bool IsUnattended { get { return isUnattended; } set { isUnattended = value; } }
        public bool IsFullRefund { get { return isFullRefund; } set { isFullRefund = value; } }
        public string CardHolderName { get { return cardHolderName; } set { cardHolderName = value; } }
        public string CardExpiryDate { get { return cardExpiryDate; } set { cardExpiryDate = value; } }
        public string AccountNo { get { return accountNo; } set { accountNo = value; } }
        public string TokenId { get { return tokenId; } set { tokenId = value; } }
        public string ReferenceId { get { return referenceId; } set { referenceId = value; } }
        public string OrderId { get { return orderId; } set { orderId = value; } }
        public decimal Amount { get { return amount; } set { amount = value; } }
        public decimal TipAmount { get { return tipAmount; } set { tipAmount = value; } }
        public PaymentGatewayTransactionType TrxType { get { return trxType; } set { trxType = value; } }
        public PaymentGatewayTransactionType OrgTrxType { get { return orgtrxType; } set { orgtrxType = value; } }
    }
    internal class CloverResponse
    {
        private TransactionPaymentsDTO trxnPaymentsDTO;
        private CCTransactionsPGWDTO CCTraxnsPGWDTO;
        private string cReceiptText;
        private string mReceiptText;
        private string responseMessage;

        public TransactionPaymentsDTO transactionPaymentsDTO { get { return trxnPaymentsDTO; }set { trxnPaymentsDTO = value; } }
        public CCTransactionsPGWDTO CCTransactionsPGWDTO { get { return CCTraxnsPGWDTO; } set { CCTraxnsPGWDTO = value; } }
        public string CustomerReceiptText { get { return cReceiptText; } set { cReceiptText = value; } }
        public string MerchantReceiptText { get { return mReceiptText; } set { mReceiptText = value; } }
        public string ResponseMessage { get { return responseMessage; } set { responseMessage = value; } }
    }
}
