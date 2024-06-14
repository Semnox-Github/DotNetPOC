/******************************************************************************************************
 * Project Name - Device
 * Description  - PayTMDQR Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.140.5     11-Jan-2023     Prasad & Fiona   PayTMDQR Payment gateway integration
 *2.150.1     22-Feb-2023     Guru S A         Kiosk Cart Enhancements
 ********************************************************************************************************/
using Newtonsoft.Json;
using Paytm;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class PaytmDQRPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string merchantId;
        string gatewayUrl;
        string deviceId;
        string authorization;
        bool isAuthEnabled;
        bool isDeviceBeepSoundRequired;
        bool isAddressValidationRequired;
        bool isCustomerAllowedToDecideEntryMode;
        bool isManual;
        bool isSignatureRequired;
        bool enableAutoAuthorization;
        string minPreAuth;
        string posId;
        string merchantKey;
        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();
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
            CAPTURE,
        }

        // TBC Params by Prasad
        // Configurations needed for Paytm DQR

        // Device Configs
        private string BAUD_RATE = "115200";
        private string PARITY_BIT = "0";
        private string DATA_BIT = "8";
        private string STOP_BIT = "1";
        private string DEBUG_MODE = "1";
        private string terminalId = "S12_123";
        private string port;

        // Rest API configs
        //const string merchantKey = "gwOi298znxK#1m2L";
        //merchantId = "FunCit24839986803319";
        // Staging
        private string hostURL = "https://securegw-stage.paytm.in";

        //Production
        //const string HOST_URL = "https://securegw.paytm.in";

        Dictionary<string, string> configurations = new Dictionary<string, string>();
        QrDisplay objds = new QrDisplay();


        const int PAYMENT_PENDING = 402;
        private int maxWaitPeriodInMin = 3;
        private int autoCheckInMin = 1;

        private object locker = new object();
        private bool isCheckNow = false;
        private bool isCancelled = false;

        private string jsonResponseBody = string.Empty;
        private string targetChecksum = string.Empty;
        // End Params by Prasad


        // Properties
        public bool IsCheckNow
        {
            get
            {
                lock (locker)
                {
                    return isCheckNow;
                }
            }



            set
            {
                lock (locker)
                {
                    isCheckNow = value;
                }
            }
        }



        public bool IsCancelled
        {
            get
            {
                lock (locker)
                {
                    return isCancelled;
                }
            }



            set
            {
                lock (locker)
                {
                    isCancelled = value;
                }
            }
        }









        public PaytmDQRPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
    : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {

            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel   
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
            merchantId = utilities.getParafaitDefaults("CREDIT_CARD_STORE_ID");
            merchantKey = utilities.getParafaitDefaults("CREDIT_CARD_HOST_USERNAME");
            hostURL = utilities.getParafaitDefaults("CREDIT_CARD_HOST_URL");
            string maxWaitTimeInMin = utilities.getParafaitDefaults("MAXIMUM_WAIT_PERIOD_IN_MINIUTES");
            if (string.IsNullOrWhiteSpace(maxWaitTimeInMin) == false)
            {
                if (int.TryParse(maxWaitTimeInMin, out maxWaitPeriodInMin) == false)
                {
                    maxWaitPeriodInMin = 3;
                }
            }

            string autoCheckInMinutes = utilities.getParafaitDefaults("AUTO_CHECK_IN_MINIUTES");
            if (string.IsNullOrWhiteSpace(autoCheckInMinutes) == false)
            {
                if (int.TryParse(autoCheckInMinutes, out autoCheckInMin) == false)
                {
                    autoCheckInMin = 1;
                }
            }
            string value = utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO");
            port = "COM" + value;
            terminalId = utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_ID");



            authorization = utilities.getParafaitDefaults("CREDIT_CARD_TOKEN_ID");
            minPreAuth = utilities.getParafaitDefaults("CREDIT_CARD_MIN_PREAUTH");
            //minPreAuth = "0";
            isAuthEnabled = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
            isDeviceBeepSoundRequired = utilities.getParafaitDefaults("ENABLE_CREDIT_CARD_DEVICE_BEEP_SOUND").Equals("Y");
            isAddressValidationRequired = utilities.getParafaitDefaults("ENABLE_ADDRESS_VALIDATION").Equals("Y");
            isCustomerAllowedToDecideEntryMode = utilities.getParafaitDefaults("ALLOW_CUSTOMER_TO_DECIDE_ENTRY_MODE").Equals("Y");
            isSignatureRequired = !utilities.getParafaitDefaults("ENABLE_SIGNATURE_VERIFICATION").Equals("N");
            enableAutoAuthorization = utilities.getParafaitDefaults("ENABLE_AUTO_CREDITCARD_AUTHORIZATION").Equals("Y");
            isSignatureRequired = (isSignatureRequired) ? !isUnattended : false;

            posId = utilities.ExecutionContext.POSMachineName;
            log.LogVariableState("merchantId", merchantId);
            log.LogVariableState("isUnattended", isUnattended);
            log.LogVariableState("gatewayUrl", gatewayUrl);
            log.LogVariableState("deviceId", deviceId);
            log.LogVariableState("authorization", isAuthEnabled);
            log.LogVariableState("enableAutoAuthorization", enableAutoAuthorization);
            log.LogVariableState("posId", posId);



            if (string.IsNullOrEmpty(merchantId))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_STORE_ID value in configuration."));
            }
            if (string.IsNullOrEmpty(merchantKey))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_HOST_USERNAME value in configuration."));
            }
            if (string.IsNullOrEmpty(hostURL))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_HOST_URL value in configuration."));
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TERMINAL_PORT_NO value in configuration."));
            }
            if (string.IsNullOrEmpty(terminalId))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TERMINAL_ID value in configuration."));
            }
            //if (string.IsNullOrEmpty(username))
            //{
            //    throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_HOST_USERNAME value in configuration."));
            ////}
            //if (string.IsNullOrEmpty(merchantId))
            //{
            //    throw new Exception(utilities.MessageUtils.getMessage("Configuration CREDIT_CARD_MERCHANT_ID is not set."));
            //}


            try
            {
                LoadConfigurations();
                PayTMDQRCommandHandler commandHandler = new PayTMDQRCommandHandler(configurations);
                //TBC this method should be called only once

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
            //CheckLastTransactionStatus();
            PayTMDQRCommandHandler commandHandler = new PayTMDQRCommandHandler(configurations);
            bool result = commandHandler.DownloadHomeScreen();
            if (!result)
            {
                log.Error("Error connecting to Device");
                throw new Exception("Error connecting to Device");
            }
            log.LogMethodExit();
        }

        private Dictionary<string, string> LoadConfigurations()
        {
            log.LogMethodEntry();
            try
            {
                configurations.Add("MID", merchantId);
                configurations.Add("MERCHANT_KEY", merchantKey);
                configurations.Add("POS_ID", terminalId);
                configurations.Add("DEBUG_MODE", DEBUG_MODE);
                configurations.Add("STOP_BIT", STOP_BIT);
                configurations.Add("DATA_BIT", DATA_BIT);
                configurations.Add("PARITY_BIT", PARITY_BIT);
                configurations.Add("BAUD_RATE", BAUD_RATE);
                configurations.Add("HOST_URL", hostURL);
                configurations.Add("COMPORT", port);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(configurations);
            return configurations;
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            VerifyPaymentRequest(transactionPaymentsDTO);
            PrintReceipt = true;
            //Form activeForm = GetActiveForm();
            statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Paytm DQR Payment Gateway");
            //{
            statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
            statusDisplayUi.CheckNowClicked += StatusDisplayUi_CheckNowClicked;
            statusDisplayUi.EnableCancelButton(false);
            isManual = false;
            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
            //Form form = statusDisplayUi as Form; 
            TransactionType trxType = TransactionType.SALE;
            string paymentId = string.Empty;
            CCTransactionsPGWDTO cCOrgTransactionsPGWDTO = null;
            double amount = transactionPaymentsDTO.Amount;
            try
            {
                thr.Start();

                if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount >= 0)
                {
                    string result;
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, trxType.ToString());
                    log.LogVariableState("Sale ccRequestPGWDTO", ccRequestPGWDTO);
                    //if (trxType == TransactionType.AUTHORIZATION)
                    //{
                    //    log.Error("Transaction Type Authorization not supported");
                    //    throw new Exception("Invalid Payment type");
                    //}
                    if (trxType == TransactionType.SALE)
                    {
                        if (cCOrgTransactionsPGWDTO != null)
                        {
                            log.Error("Transaction Type Capture not supported");
                            throw new Exception("Invalid Payment type");
                        }
                        else
                        {
                            // direct sale
                            //form.Show(activeForm);
                            //SetNativeEnabled(activeForm, false);
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                            PayTMDQRCommandHandler commandHandler = new PayTMDQRCommandHandler(configurations);
                            bool isHomeScreenLoaded = commandHandler.ShowDeviceHomeScreen();
                            if (!isHomeScreenLoaded)
                            {
                                log.Error("Error connecting to the device. ShowDeviceHomeScreen() failed.");
                                throw new Exception("Error connecting to the device");
                            }

                            log.Info("Device home screen has been loaded");
                            // device is connected and ready to accept request
                            PayTMDQRRequestDTO requestDTO = new PayTMDQRRequestDTO
                            {
                                transactionAmount = Convert.ToDecimal(amount),
                                orderId = ccRequestPGWDTO.RequestID.ToString()
                            };
                            log.LogVariableState("Sale requestDTO", requestDTO);

                            PayTMDQRResponseDTO createQRResponse = commandHandler.CreateQrCode(requestDTO);
                            log.LogVariableState("saleResponse", createQRResponse);

                            if (createQRResponse == null)
                            {
                                log.Error("Sale response was null");
                                throw new Exception("Unable to Generate QR code");
                            }

                            jsonResponseBody = JsonConvert.SerializeObject(createQRResponse.body);
                            targetChecksum = Checksum.generateSignature(jsonResponseBody, merchantKey);
                            if (!VerifyCheckSum(jsonResponseBody, targetChecksum))
                            {
                                log.Error("Create_QR: Checksum mismatched");
                                throw new Exception("Unable to verify the response");
                            }


                            //var successReponseCode = PayTMDQRCommandHandler.SuccessResponseMessage.Select(x => x.Item1).ToList();
                            var failureReponseCode = PayTMDQRCommandHandler.FailureResponseMessage.Select(x => x.Code).ToList();
                            string failureMessage = string.Empty;
                            if (failureReponseCode.Contains(createQRResponse.body.resultInfo.resultCode))
                            {
                                // QR generation failed
                                foreach (var item in PayTMDQRCommandHandler.FailureResponseMessage)
                                {
                                    if (item.Code == createQRResponse.body.resultInfo.resultCode)
                                    {
                                        failureMessage = item.Message;
                                    }

                                }

                                log.Error("Qr generation failed: " + failureMessage);
                                throw new Exception("Qr generation failed: " + failureMessage);
                            }

                            // QR succeeded

                            bool hasQRDisplayed = objds.displayTxnQr(merchantId, port, 110200, Convert.ToInt32(PARITY_BIT), Convert.ToInt32(DATA_BIT), Convert.ToInt32(STOP_BIT), requestDTO.orderId, amount.ToString(), createQRResponse.body.qrData, "Rs", Convert.ToInt32(DEBUG_MODE), terminalId);

                            if (!hasQRDisplayed)
                            {
                                log.Error("Error displaying QR on the Paytm Display");
                                throw new Exception("Error: Unable to render QR on the Display");
                            }
                            statusDisplayUi.EnableCancelButton(true);
                            statusDisplayUi.EnableCheckNowButton(true);
                            // Status Polling Started
                            Task<PayTMDQRResponseDTO> task = GetPaymentStatus(requestDTO.orderId);
                            while (task.IsCompleted == false)
                            {
                                Thread.Sleep(1000);
                                Application.DoEvents();
                            }
                            statusDisplayUi.EnableCancelButton(false);
                            statusDisplayUi.EnableCheckNowButton(false);
                            if (task.Result == null)
                            {
                                log.Error("Payment Response was null");
                                throw new Exception("Payment Failed: No response received");
                            }

                            PayTMDQRResponseDTO paymentResponse = task.Result;
                            // verify the checksum
                            jsonResponseBody = JsonConvert.SerializeObject(paymentResponse.body);
                            targetChecksum = Checksum.generateSignature(jsonResponseBody, merchantKey);
                            if (!VerifyCheckSum(jsonResponseBody, targetChecksum))
                            {
                                log.Error("QR Payment Response: Checksum mismatched");
                                throw new Exception("Unable to verify the response");
                            }

                            Boolean result2 = objds.showSuccessScreen(merchantId, port, 110200, Convert.ToInt32(PARITY_BIT), Convert.ToInt32(DATA_BIT), Convert.ToInt32(STOP_BIT), requestDTO.orderId, paymentResponse.body.txnAmount.ToString(), "INR", Convert.ToInt32(DEBUG_MODE), terminalId);
                            if (!result2)
                            {
                                log.Debug("CheckTxStatus: Error rendering Payment Success Image on Display.");
                            }
                            // update the database
                            log.Debug("Sale Succeeded");
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.CardType = paymentResponse.body.paymentMode;
                            cCTransactionsPGWDTO.RefNo = paymentResponse.body.orderId;
                            cCTransactionsPGWDTO.RecordNo = paymentResponse.body.txnId;
                            cCTransactionsPGWDTO.AcqRefData = paymentResponse.body.bankTxnId;
                            cCTransactionsPGWDTO.TextResponse = paymentResponse.body.resultInfo.resultMsg;
                            cCTransactionsPGWDTO.TranCode = trxType.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = paymentResponse.body.txnAmount;

                            SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                        }
                    }
                }
                else
                {
                    log.Fatal("Exception Inorrect object passed");
                    throw new Exception("Exception in processing Payment ");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex.InnerException != null ? ex.InnerException : ex;
            }
            finally
            {
                //SetNativeEnabled(activeForm, true);
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
                Thread.Sleep(3000);
                PayTMDQRCommandHandler commandHandler = new PayTMDQRCommandHandler(configurations);
                bool isHomeScreenLoaded = commandHandler.ShowDeviceHomeScreen();
                if (!isHomeScreenLoaded)
                {
                    log.Error("Error connecting to the device. ShowDeviceHomeScreen() failed.");
                    //throw new Exception("Error connecting to the device");
                }

            }
            //}

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        private void StatusDisplayUi_CancelClicked(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            IsCancelled = true;
            statusDisplayUi.EnableCancelButton(false);
            statusDisplayUi.EnableCheckNowButton(false);
            log.LogMethodExit();
        }

        private void StatusDisplayUi_CheckNowClicked(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            IsCheckNow = true;
            statusDisplayUi.EnableCheckNowButton(false);
            log.LogMethodExit();
        }

        async Task<PayTMDQRResponseDTO> GetPaymentStatus(string orderId)
        {
            try
            {
                log.LogMethodEntry(orderId);
                int reqSent = 0;
                DateTime maxLimitTime = DateTime.Now.AddMinutes(maxWaitPeriodInMin);
                DateTime startAutoCheckTime = DateTime.Now.AddMinutes(autoCheckInMin);
                log.LogVariableState("MAXIMUM_WAIT_PERIOD_IN_MINIUTES", maxWaitPeriodInMin);
                log.LogVariableState("AUTO_CHECK_IN_MINIUTES", autoCheckInMin);
                string status;
                PayTMDQRCommandHandler commandHandler = new PayTMDQRCommandHandler(configurations);
                PayTMDQRResponseDTO response = null;

                while (DateTime.Now < maxLimitTime)
                {
                    if (IsCancelled)
                    {
                        log.Error("Cancel button was clicked");
                        response = commandHandler.CheckTxStatus(new PayTMDQRRequestDTO { orderId = orderId });
                        if (response != null && Convert.ToInt32(response.body.resultInfo.resultCode) != PAYMENT_PENDING)
                        {
                            log.LogMethodExit(response);
                            log.Debug($"Actual Response received on {reqSent} attempt");
                            response.body.noOfRequestsSent = reqSent;
                            return response;
                        }
                        log.Error("Payment cancelled");
                        throw new Exception("Payment cancelled");
                    }

                    if (DateTime.Now > startAutoCheckTime || IsCheckNow)
                    {
                        //status = "Checking : " + DateTime.Now.ToString();
                        //UpdateStatus(status);

                        response = commandHandler.CheckTxStatus(new PayTMDQRRequestDTO { orderId = orderId });
                        reqSent++;
                        //check for the auto check logic 
                        //if gets the valid result return
                        if (response != null && Convert.ToInt32(response.body.resultInfo.resultCode) != PAYMENT_PENDING)
                        {
                            log.LogMethodExit(response);
                            log.Debug($"Actual Response received on {reqSent} attempt");
                            response.body.noOfRequestsSent = reqSent;
                            return response;
                        }
                    }
                    //else
                    //{
                    //    status = "Waiting : " + DateTime.Now.ToString();
                    //    UpdateStatus(status);
                    //}
                    await Task.Delay(3000);
                }
                log.Debug($"Total no. of requests sent: {reqSent}");
                log.LogMethodExit(response);
                log.Error("Overall timeout occured");
                throw new Exception("Time Out! Could not get the Payment Response");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        //public static Form GetActiveForm()
        //{
        //    // Returns null for an MDI app
        //    Form activeForm = Form.ActiveForm;
        //    if (activeForm == null)
        //    {
        //        FormCollection openForms = Application.OpenForms;
        //        for (int i = 0; i < openForms.Count && activeForm == null; ++i)
        //        {
        //            Form openForm = openForms[i];
        //            if (openForm.IsMdiContainer)
        //            {
        //                activeForm = openForm.ActiveMdiChild;
        //            }
        //        }
        //    }
        //    if (activeForm == null)
        //    {
        //        activeForm = Application.OpenForms[Application.OpenForms.Count - 1];
        //    }
        //    return activeForm;
        //}

        //const int GWL_STYLE = -16; const int WS_DISABLED = 0x08000000;
        //[DllImport("user32.dll")]
        //static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        //[DllImport("user32.dll")]
        //static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        //void SetNativeEnabled(Form form, bool enabled) { SetWindowLong(form.Handle, GWL_STYLE, GetWindowLong(form.Handle, GWL_STYLE) & ~WS_DISABLED | (enabled ? 0 : WS_DISABLED)); }
        /// <summary>
        /// RefundAmount
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            //Form activeForm = GetActiveForm();
            try
            {
                PrintReceipt = true;
                if (transactionPaymentsDTO != null)
                {
                    if (transactionPaymentsDTO.Amount < 0)
                    {
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Variable Refund Not Supported"));
                    }
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Paytm DQR Payment Gateway");
                    //{
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    try
                    {
                        //Form form = statusDisplayUi as Form;
                        //form.Show(activeForm);
                        //SetNativeEnabled(activeForm, false);
                        thr.Start();
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));

                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.REFUND.ToString());
                        log.LogVariableState("Refund cCRequestPGWDTO", cCRequestPGWDTO);
                        CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                        CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;
                        log.LogVariableState("Refund ccOrigTransactionsPGWDTO", ccOrigTransactionsPGWDTO);
                        double amount = transactionPaymentsDTO.Amount;
                        log.Debug($"amount to be refunded: {amount}");
                        double refundAmount;

                        // starting paytm Refund
                        PayTMDQRCommandHandler commandHandler = new PayTMDQRCommandHandler(configurations);
                        bool isHomeScreenLoaded = commandHandler.ShowDeviceHomeScreen();
                        if (!isHomeScreenLoaded)
                        {
                            log.Error("Error connecting the Device");
                        }
                        PayTMDQRRequestDTO requestDTO = new PayTMDQRRequestDTO
                        {
                            orderId = ccOrigTransactionsPGWDTO.InvoiceNo.ToString(), // payment ccRequestId
                            paytmPaymentId = ccOrigTransactionsPGWDTO.RecordNo.ToString(), // Paytm Transaction Id for Payment 
                            refundAmount = Convert.ToDecimal(amount),
                            refId = cCRequestPGWDTO.RequestID.ToString() // Refund ccRequestId
                        };

                        log.LogVariableState("Refund requestDTO", requestDTO);

                        PayTMDQRResponseDTO refundResponse = commandHandler.RefundAmount(requestDTO);
                        log.LogVariableState("Refund Response", refundResponse);

                        if (refundResponse == null)
                        {
                            log.Error("Create_Refund Response was null");
                        }

                        log.Debug("Get_Refund Started");
                        // get Refund Status
                        PayTMDQRRequestDTO getRefundRequestDTO = new PayTMDQRRequestDTO
                        {
                            orderId = ccOrigTransactionsPGWDTO.InvoiceNo.ToString(),// payment ccRequestId
                            refId = cCRequestPGWDTO.RequestID.ToString()// Refund ccRequestId
                        };
                        log.Debug($"Refund getRefundRequestDTO: {getRefundRequestDTO}");

                        PayTMDQRResponseDTO getRefundResponse = commandHandler.CheckRefundStatus(requestDTO);
                        log.Debug($"Refund getRefundResponse: {getRefundResponse}");
                        if (getRefundResponse == null)
                        {
                            log.Error("Get_Refund response was null");
                            throw new Exception("Refund request failed");
                        }

                        // verify the checksum
                        jsonResponseBody = JsonConvert.SerializeObject(getRefundResponse.body);
                        targetChecksum = Checksum.generateSignature(jsonResponseBody, merchantKey);
                        if (!VerifyCheckSum(jsonResponseBody, targetChecksum))
                        {
                            log.Error("Refund Response: Checksum mismatched");
                            throw new Exception("Unable to verify the response");
                        }

                        // We got the refund response
                        List<string> failureReponseCode = PayTMDQRCommandHandler.FailureResponseMessage.Select(x => x.Code).ToList();
                        string failureMessage = string.Empty;
                        log.LogVariableState("failureReponseCode", failureReponseCode);
                        log.LogVariableState("getRefundResponse.body.resultInfo.resultCode", getRefundResponse.body.resultInfo.resultCode);
                        if (failureReponseCode.Contains(getRefundResponse.body.resultInfo.resultCode))
                        {
                            // retrieve the error message
                            foreach (var item in PayTMDQRCommandHandler.FailureResponseMessage)
                            {
                                if (item.Code == getRefundResponse.body.resultInfo.resultCode)
                                {
                                    failureMessage = item.Message;
                                }

                            }

                            log.Error("Refund failed: " + failureMessage);
                            throw new Exception("Refund failed: " + failureMessage);
                        }

                        // Refund Succeeded
                        // Update the database
                        log.Info("getRefundResponse:Refund Request Succeeded");

                        refundAmount = Convert.ToDouble(getRefundResponse.body.refundAmount);
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        RefundDetail refundDetails = null;
                        if (getRefundResponse.body.refundDetailInfoList.Count > 0)
                        {
                            refundDetails = getRefundResponse.body.refundDetailInfoList.First();
                            //TBC I have added the UPI id in card field
                            cCTransactionsPGWDTO.AcctNo = refundDetails.maskedVpa;
                            cCTransactionsPGWDTO.CardType = refundDetails.payMethod;
                            cCTransactionsPGWDTO.ProcessData = refundDetails.userMobileNo;
                        }

                        cCTransactionsPGWDTO.AuthCode = getRefundResponse.body.authRefId;
                        cCTransactionsPGWDTO.RefNo = getRefundResponse.body.refId;
                        cCTransactionsPGWDTO.RecordNo = getRefundResponse.body.refundId;
                        cCTransactionsPGWDTO.AcqRefData = getRefundResponse.body.bankTxnId;
                        //cCTransactionsPGWDTO.TokenID = responseObject.payment.cardTransaction.token;
                        cCTransactionsPGWDTO.TextResponse = getRefundResponse.body.resultInfo.resultMsg;
                        cCTransactionsPGWDTO.DSIXReturnCode = getRefundResponse.body.acceptRefundStatus;
                        cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                        log.LogVariableState("getRefundResponse cCTransactionsPGWDTO", cCTransactionsPGWDTO);

                        SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                        transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                        //TBC
                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                        log.LogVariableState("getRefundResponse transactionPaymentsDTO", transactionPaymentsDTO);
                    }
                    finally
                    {
                        //SetNativeEnabled(activeForm, true);
                        if (statusDisplayUi != null)
                        {
                            statusDisplayUi.CloseStatusWindow();
                        }
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                statusDisplayUi.DisplayText("Error occured while Refunding the Amount");
                log.Error("Error occured while Refunding the Amount", ex);
                log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                log.LogMethodExit(null, "throwing Exception");
                throw new Exception("Refund failed exception :" + ex.Message);
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
        public override void SendLastTransactionStatusCheckRequest(CCRequestPGWDTO cCRequestPGWDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            try
            {
                //Form activeForm = GetActiveForm();
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage("Checking the transaction status" + ((cCRequestPGWDTO != null) ? " of TrxId:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "PaytmDQ Payment Gateway");
                //{
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                //Form form = statusDisplayUi as Form;
                //form.Show(activeForm);
                //SetNativeEnabled(activeForm, false);
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));

                CCTransactionsPGWDTO ccTransactionsPGWDTOResponse = null;
                var successReponseCode = PayTMDQRCommandHandler.SuccessResponseMessage.Select(x => x.Code).ToList();

                if (cCTransactionsPGWDTO != null)
                {

                    log.Debug("cCTransactionsPGWDTO is not null");

                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOcapturedList = null;
                    // TBC What does this indicate? cCTransactionsPGWDTO.RecordNo.Equals("A")
                    if (!string.IsNullOrEmpty(cCTransactionsPGWDTO.RefNo) && cCTransactionsPGWDTO.RecordNo.Equals("A") && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.REFUND.ToString()) && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.VOID.ToString()))
                    {
                        #region AUTH | CAPTURE | TIP ADJUST
                        //if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
                        //{
                        //    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTO.ResponseID.ToString()));
                        //    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        //    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                        //    if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                        //    {
                        //        if (cCTransactionsPGWDTOList[0].TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()))
                        //        {
                        //            ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                        //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                        //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        //            cCTransactionsPGWDTOcapturedList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                        //            if (cCTransactionsPGWDTOcapturedList != null && cCTransactionsPGWDTOcapturedList.Count > 0)
                        //            {
                        //                log.Debug("The authorized transaction is captured.");
                        //                return;
                        //            }
                        //        }
                        //        else if (cCTransactionsPGWDTOList[0].TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                        //        {
                        //            ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                        //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                        //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        //            cCTransactionsPGWDTOcapturedList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                        //            if (cCTransactionsPGWDTOcapturedList != null && cCTransactionsPGWDTOcapturedList.Count > 0)
                        //            {
                        //                log.Debug("The authorized transaction is adjusted for tip.");
                        //                return;
                        //            }
                        //        }
                        //        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                        //        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        //        List<TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);
                        //        if (transactionPaymentsDTOs == null || transactionPaymentsDTOs.Count == 0)
                        //        {
                        //            cCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                        //        }
                        //        else
                        //        {
                        //            log.Debug("The capture/tip adjusted transaction exists for the authorization request with requestId:" + cCTransactionsPGWDTO.InvoiceNo + " and its upto date");
                        //            return;
                        //        }
                        //    }
                        //}
                        //else if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()))
                        //{
                        //    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTO.ResponseID.ToString()));
                        //    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        //    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);

                        //    if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                        //    {
                        //        if (cCTransactionsPGWDTOList[0].TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                        //        {
                        //            ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                        //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                        //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        //            cCTransactionsPGWDTOcapturedList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                        //            if (cCTransactionsPGWDTOcapturedList != null && cCTransactionsPGWDTOcapturedList.Count > 0)
                        //            {
                        //                log.Debug("The captured transaction is adjusted for tip.");
                        //                return;
                        //            }
                        //        }
                        //        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                        //        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        //        List<TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);
                        //        if (transactionPaymentsDTOs == null || transactionPaymentsDTOs.Count == 0)
                        //        {
                        //            cCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                        //        }
                        //        else
                        //        {
                        //            log.Debug("The tip adjusted transaction exists for the capture request with requestId:" + cCTransactionsPGWDTO.InvoiceNo + " and its upto date");
                        //            return;
                        //        }
                        //    }

                        //}
                        //else if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                        //{
                        //    log.Debug("credit card transaction is tip adjustment.");
                        //    log.LogMethodExit(true);
                        //    return;
                        //}
                        #endregion

                        PayTMDQRRequestDTO requestDTO = new PayTMDQRRequestDTO { orderId = cCTransactionsPGWDTO.RefNo.ToString() };
                        PayTMDQRCommandHandler commandHandler = new PayTMDQRCommandHandler(configurations);
                        PayTMDQRResponseDTO responseOb = commandHandler.CheckTxStatus(requestDTO);
                        log.LogVariableState("Get Payment responseOb", responseOb);

                        if (responseOb == null)
                        {
                            log.Error($"Last Transaction Check failed for Tx={cCTransactionsPGWDTO.RefNo.ToString()}. Response was empty");
                        }

                        // verify the checksum
                        jsonResponseBody = JsonConvert.SerializeObject(responseOb.body);
                        targetChecksum = Checksum.generateSignature(jsonResponseBody, merchantKey);
                        if (!VerifyCheckSum(jsonResponseBody, targetChecksum))
                        {
                            log.Error("Last Tx Check: Checksum mismatched");
                            throw new Exception("Unable to verify the response");
                        }

                        if (successReponseCode.Contains(responseOb.body.resultInfo.resultCode))
                        {
                            // Sale Succeeded
                            double resamount = Convert.ToDouble(responseOb.body.txnAmount);
                            ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();


                            ccTransactionsPGWDTOResponse.AuthCode = responseOb.body.authRefId;
                            ccTransactionsPGWDTOResponse.CardType = "UPI";
                            ccTransactionsPGWDTOResponse.RefNo = responseOb.body.orderId;
                            ccTransactionsPGWDTOResponse.RecordNo = responseOb.body.txnId;
                            ccTransactionsPGWDTOResponse.AcqRefData = responseOb.body.bankTxnId;
                            ccTransactionsPGWDTOResponse.TextResponse = responseOb.body.resultInfo.resultMsg;
                            ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                            ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                            ccTransactionsPGWDTOResponse.Authorize = resamount.ToString();
                        }

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
                    ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();
                    PayTMDQRRequestDTO requestDTO = new PayTMDQRRequestDTO { orderId = cCRequestPGWDTO.RequestID.ToString() };
                    PayTMDQRCommandHandler commandHandler = new PayTMDQRCommandHandler(configurations);
                    PayTMDQRResponseDTO responseOb = commandHandler.CheckTxStatus(requestDTO);

                    log.LogVariableState("Get_Payment responseOb", responseOb);
                    if (responseOb == null)
                    {
                        log.Error($"Last Transaction Check failed for Tx={cCRequestPGWDTO.RequestID.ToString()}. Response was empty");
                    }

                    // verify the checksum
                    jsonResponseBody = JsonConvert.SerializeObject(responseOb.body);
                    targetChecksum = Checksum.generateSignature(jsonResponseBody, merchantKey);
                    if (!VerifyCheckSum(jsonResponseBody, targetChecksum))
                    {
                        log.Error("Last Tx Check: Checksum mismatched");
                        throw new Exception("Unable to verify the response");
                    }

                    if (successReponseCode.Contains(responseOb.body.resultInfo.resultCode))
                    {
                        // Sale Succeeded
                        double resamount = Convert.ToDouble(responseOb.body.txnAmount);
                        ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();
                        ccTransactionsPGWDTOResponse.AuthCode = responseOb.body.authRefId;
                        ccTransactionsPGWDTOResponse.CardType = "UPI";
                        ccTransactionsPGWDTOResponse.RefNo = responseOb.body.orderId;
                        ccTransactionsPGWDTOResponse.RecordNo = responseOb.body.txnId;
                        ccTransactionsPGWDTOResponse.AcqRefData = responseOb.body.bankTxnId;
                        ccTransactionsPGWDTOResponse.TextResponse = responseOb.body.resultInfo.resultMsg;
                        ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                        ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                        ccTransactionsPGWDTOResponse.Authorize = resamount.ToString();
                    }
                    else
                    {
                        ccTransactionsPGWDTOResponse.TextResponse = "Txn not found";
                        ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                        //ccTransactionsPGWDTOResponse.RecordNo = "C";
                    }
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
                        ccTransactionsPGWDTOResponse.TranCode = "SALE";
                        if (cCTransactionsPGWDTO == null)
                        {
                            log.Debug("Saving ccTransactionsPGWDTOResponse.");
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTOResponse);
                            ccTransactionsPGWBL.Save();
                        }
                        log.LogVariableState("ccTransactionsPGWDTOResponse", ccTransactionsPGWDTOResponse);
                        if (!string.IsNullOrEmpty(ccTransactionsPGWDTOResponse.RefNo) && !ccTransactionsPGWDTOResponse.RecordNo.Equals("C"))
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
                            transactionPaymentsDTO = RefundAmount(transactionPaymentsDTO);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception one");
                        if (!isUnattended && showMessageDelegate != null)
                        {
                            //showMessageDelegate(utilities.MessageUtils.getMessage("Last transaction status check is failed. :" + ((cCRequestPGWDTO != null) ? " TransactionID:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "Last Transaction Status Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        log.Error("Last transaction check failed", ex);
                        throw;
                    }
                }
                //}
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
                    if (statusDisplayUi != null)
                        statusDisplayUi.CloseStatusWindow();
                }
                catch (Exception ex)
                {
                    log.Debug("Exception three without throw in finally");
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }
        private void SendPrintReceiptRequest(TransactionPaymentsDTO transactionPaymentsDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, cCTransactionsPGWDTO);
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PRINT_CUSTOMER_RECEIPT") == "Y")
            {
                transactionPaymentsDTO.Memo = GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, false);
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PRINT_MERCHANT_RECEIPT") == "Y" && !isUnattended)
            {
                transactionPaymentsDTO.Memo += GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, true);
            }
            log.LogMethodExit();
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
                if (ccTransactionsPGWDTO.AcqRefData != null && !string.IsNullOrWhiteSpace(ccTransactionsPGWDTO.AcqRefData))
                {
                    string maskedMerchantId = (new String('X', 8) + ((ccTransactionsPGWDTO.AcqRefData.Length > 4) ? ccTransactionsPGWDTO.AcqRefData.Substring(ccTransactionsPGWDTO.AcqRefData.Length - 4)
                                                                                             : ccTransactionsPGWDTO.AcqRefData));
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Merchant ID") + "     : ".PadLeft(12) + maskedMerchantId, Alignment.Left);
                }
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Date") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TransactionDatetime.ToString("MMM dd yyyy HH:mm"), Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Type") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TranCode, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Invoice Number") + "  : ".PadLeft(6) + ccTransactionsPGWDTO.InvoiceNo, Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.AuthCode))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Authorization") + "   : ".PadLeft(10) + ccTransactionsPGWDTO.AuthCode, Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.CardType))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Card Type") + "       : ".PadLeft(15) + ccTransactionsPGWDTO.CardType, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Cardholder Name") + ": ".PadLeft(3) + trxPaymentsDTO.NameOnCreditCard, Alignment.Left);
                string maskedPAN = ((string.IsNullOrEmpty(ccTransactionsPGWDTO.AcctNo) ? ccTransactionsPGWDTO.AcctNo
                                                                             : (new String('X', 12) + ((ccTransactionsPGWDTO.AcctNo.Length > 4)
                                                                                                     ? ccTransactionsPGWDTO.AcctNo.Substring(ccTransactionsPGWDTO.AcctNo.Length - 4)
                                                                                                     : ccTransactionsPGWDTO.AcctNo))));
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("PAN") + ": ".PadLeft(24) + maskedPAN, Alignment.Left);
                //receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Entry Mode") + ": ".PadLeft(13) + ccTransactionsPGWDTO.CaptureStatus, Alignment.Left);

                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage(ccTransactionsPGWDTO.TextResponse.ToUpper()), Alignment.Center);
                receiptText += Environment.NewLine;
                if (ccTransactionsPGWDTO.TranCode.Equals(TransactionType.CAPTURE.ToString()) || ccTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Amount") + "  : " + Convert.ToDouble(trxPaymentsDTO.Amount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                }
                if (ccTransactionsPGWDTO.TranCode.Equals(TransactionType.AUTHORIZATION.ToString()))
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Amount") + " : " + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                           : " + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    //receiptText += Environment.NewLine;
                }
                else
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                           : " + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    receiptText += Environment.NewLine;
                }
                if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse) && ccTransactionsPGWDTO.TextResponse.ToUpper().Equals("APPROVED")) && ccTransactionsPGWDTO.TranCode.ToUpper().Equals(TransactionType.AUTHORIZATION.ToString()))
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                          : " + "_____________", Alignment.Left);
                }
                receiptText += Environment.NewLine;
                if (IsMerchantCopy)
                {
                    if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse) && ccTransactionsPGWDTO.TextResponse.Equals("APPROVED")))
                    {
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText("_______________________", Alignment.Center);
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Signature"), Alignment.Center);
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage(1180), Alignment.Center);
                        //}
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
                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse) && ccTransactionsPGWDTO.TextResponse.Equals("APPROVED"))
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

            int pageWidth = 70;
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
                log.LogMethodExit(text);
                return text;
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
        private bool VerifyCheckSum(string jsonResponseBody, string targetChecksum)
        {
            log.LogMethodEntry();
            bool isVerifySignature = Checksum.verifySignature(jsonResponseBody, merchantKey, targetChecksum);
            log.LogMethodExit(isVerifySignature);
            return isVerifySignature;
        }


    }
}
