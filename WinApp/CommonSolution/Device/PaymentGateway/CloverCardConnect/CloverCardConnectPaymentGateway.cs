using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.Net.Security;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Semnox.Parafait.Languages;
using System.Data.SqlClient;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CloverCardConnectPaymentGateway : PaymentGateway
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
        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();
        private bool isPrintReceiptEnabled;



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


        public CloverCardConnectPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
    : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {

            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel   
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
            merchantId = utilities.getParafaitDefaults("CREDIT_CARD_STORE_ID");
            gatewayUrl = utilities.getParafaitDefaults("CREDIT_CARD_HOST_URL");

            deviceId = utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_ID");
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

            PrintReceipt = false;
            if (utilities.getParafaitDefaults("CC_PAYMENT_RECEIPT_PRINT").Equals("N"))//If CC_PAYMENT_RECEIPT_PRINT which comes from POS is set as false then terminal should print, If you need terminal to print the receipt then set CC_PAYMENT_RECEIPT_PRINT value as N
            {
                PrintReceipt = true;
            }
            isPrintReceiptEnabled = PrintReceipt;


            posId = utilities.ExecutionContext.POSMachineName;
            log.LogVariableState("merchantId", merchantId);
            log.LogVariableState("isUnattended", isUnattended);
            log.LogVariableState("gatewayUrl", gatewayUrl);
            log.LogVariableState("deviceId", deviceId);
            log.LogVariableState("authorization", isAuthEnabled);
            log.LogVariableState("enableAutoAuthorization", enableAutoAuthorization);
            log.LogVariableState("posId", posId);



            //if (string.IsNullOrEmpty(deviceUrl))
            //{
            //    throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_DEVICE_URL value in configuration."));
            //}
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
            //if (string.IsNullOrEmpty(username))
            //{
            //    throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_HOST_USERNAME value in configuration."));
            //}
            if (string.IsNullOrEmpty(merchantId))
            {
                throw new Exception(utilities.MessageUtils.getMessage("Configuration CREDIT_CARD_MERCHANT_ID is not set."));
            }
            //if (string.IsNullOrEmpty(idempotency))
            //{
            //    throw new Exception(utilities.MessageUtils.getMessage("Configuration CREDIT_CARD_IDEMOPOTENCYKEY is not set."));
            //}

            try
            {
                CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);
                string result = cloverGatewayCommandHandler.DisplayWelcomeScreen(deviceId, posId);
                if (result == "Exception")
                {
                    throw new Exception("Exception while connecting to Device");
                }
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

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            VerifyPaymentRequest(transactionPaymentsDTO);
            //Form activeForm = GetActiveForm();
            statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Clover CardConnect Payment Gateway");
            //{
            //statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
            statusDisplayUi.EnableCancelButton(false);
            isManual = false;
            //Form form = statusDisplayUi as Form;
            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
            TransactionType trxType = TransactionType.SALE;
            string paymentId = string.Empty;
            CCTransactionsPGWDTO cCOrgTransactionsPGWDTO = null;
            double amount = (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount) * 100;
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
                                    }
                                    else if (frmTranType.TransactionType.Equals("TATokenRequest"))
                                    {
                                        trxType = TransactionType.TATokenRequest;
                                        transactionPaymentsDTO.Amount = Convert.ToDouble(minPreAuth);
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

                if (transactionPaymentsDTO != null)
                {
                    CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);
                    string result;
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, trxType.ToString());

                    if (trxType == TransactionType.AUTHORIZATION)
                    {
                        if (cCOrgTransactionsPGWDTO != null)
                        {
                            paymentId = cCOrgTransactionsPGWDTO.RecordNo.ToString();
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                            //form.Show(activeForm);
                            //SetNativeEnabled(activeForm, false);

                            transactionPaymentsDTO.TipAmount = 0;
                            //double tipAmount = transactionPaymentsDTO.TipAmount * 100;
                            result = cloverGatewayCommandHandler.CreateAuth(amount, deviceId, posId, paymentId);
                            if (result == "Exception")
                            {
                                throw new Exception("Exception in authorizing final Payment ");
                            }
                            dynamic responseObject = JsonConvert.DeserializeObject(result);
                            //string resultPayment = cloverGatewayCommandHandler.GetPaymentByExternalPaymentId(cCOrgTransactionsPGWDTO.InvoiceNo, deviceId, posId);
                            string paymentIdofResponse = Convert.ToString(responseObject.paymentId);
                            string resultPayment = cloverGatewayCommandHandler.GetPaymentByPaymentId(Convert.ToString(paymentIdofResponse), deviceId, posId);
                            responseObject = JsonConvert.DeserializeObject(resultPayment);
                            log.LogVariableState("responseObject", responseObject);
                            double resAmount = Convert.ToDouble(responseObject.payment.amount) * 0.01;
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responseObject.payment.cardTransaction.last4);
                            cCTransactionsPGWDTO.AuthCode = responseObject.payment.cardTransaction.authCode;
                            cCTransactionsPGWDTO.CardType = responseObject.payment.cardTransaction.extra.authorizingNetworkName;
                            cCTransactionsPGWDTO.CaptureStatus = responseObject.payment.cardTransaction.entryType;
                            cCTransactionsPGWDTO.RefNo = responseObject.payment.externalPaymentId;
                            cCTransactionsPGWDTO.RecordNo = responseObject.payment.id;
                            cCTransactionsPGWDTO.AcqRefData = "AID:" + responseObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responseObject.payment.cardTransaction.referenceId;
                            cCTransactionsPGWDTO.TokenID = responseObject.payment.cardTransaction.token;
                            //cCTransactionsPGWDTO.TextResponse = responseObject.payment.cardTransaction.state;
                            cCTransactionsPGWDTO.TextResponse = "Approval";
                            cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = (resAmount).ToString();
                            //cCTransactionsPGWDTO.TipAmount = (tipAmount * 0.01).ToString();
                            if (isPrintReceiptEnabled)
                            {
                                cloverGatewayCommandHandler.CreatePrintPaymentReceipt(cCTransactionsPGWDTO.RecordNo, deviceId, posId);
                            }

                            SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                            transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                            transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                        }
                        else
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                            //form.Show(activeForm);
                            //SetNativeEnabled(activeForm, false);


                            result = cloverGatewayCommandHandler.CreateDirectAuth(amount, deviceId, posId, ccRequestPGWDTO.RequestID.ToString());
                            if (result == "Exception")
                            {
                                throw new Exception("Exception in processing Authorizing ");
                            }
                            else
                            {
                                dynamic responseObject = JsonConvert.DeserializeObject(result);
                                log.LogVariableState("responseObject", responseObject);
                                double resamount = Convert.ToDouble(responseObject.payment.amount) * 0.01;
                                //PaymentDTO paymentDTO = JsonConvert.DeserializeObject<PaymentDTO>(result);
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responseObject.payment.cardTransaction.last4);
                                cCTransactionsPGWDTO.AuthCode = responseObject.payment.cardTransaction.authCode;
                                cCTransactionsPGWDTO.CardType = responseObject.payment.cardTransaction.extra.authorizingNetworkName;
                                cCTransactionsPGWDTO.CaptureStatus = responseObject.payment.cardTransaction.entryType;
                                cCTransactionsPGWDTO.RefNo = responseObject.payment.externalPaymentId;
                                cCTransactionsPGWDTO.RecordNo = responseObject.payment.id;
                                cCTransactionsPGWDTO.AcqRefData = "AID:" + responseObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responseObject.payment.cardTransaction.referenceId;
                                cCTransactionsPGWDTO.TokenID = responseObject.payment.cardTransaction.token;
                                //cCTransactionsPGWDTO.TextResponse = responseObject.payment.cardTransaction.state;
                                cCTransactionsPGWDTO.TextResponse = "Approval";
                                cCTransactionsPGWDTO.TranCode = trxType.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = resamount.ToString();

                                if (isPrintReceiptEnabled)
                                {
                                    cloverGatewayCommandHandler.CreatePrintPaymentReceipt(cCTransactionsPGWDTO.RecordNo, deviceId, posId);
                                }
                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = resamount;
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            }

                        }
                    }
                    else if (trxType == TransactionType.SALE)
                    {
                        if (cCOrgTransactionsPGWDTO != null)
                        {
                            paymentId = cCOrgTransactionsPGWDTO.RecordNo.ToString();
                            frmFinalizeTransaction frmFinalizeTransaction = new frmFinalizeTransaction(utilities, overallTransactionAmount, overallTipAmountEntered, Convert.ToDecimal(transactionPaymentsDTO.Amount), Convert.ToDecimal(transactionPaymentsDTO.TipAmount), cCOrgTransactionsPGWDTO.AcctNo, showMessageDelegate);
                            if (frmFinalizeTransaction.ShowDialog() != DialogResult.Cancel)
                            {
                                //form.Show(activeForm);
                                //SetNativeEnabled(activeForm, false);
                                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(frmFinalizeTransaction.TipAmount);
                                double tipAmount = transactionPaymentsDTO.TipAmount * 100;
                                result = cloverGatewayCommandHandler.CreateCapture(paymentId, amount, deviceId, posId, transactionPaymentsDTO.TipAmount * 100);
                                if (result == "Exception")
                                {
                                    throw new Exception("Exception in authorizing final Payment ");
                                }
                                dynamic responseObject = JsonConvert.DeserializeObject(result);
                                //string resultPayment = cloverGatewayCommandHandler.GetPaymentByExternalPaymentId(cCOrgTransactionsPGWDTO.InvoiceNo, deviceId, posId);
                                string paymentIdofResponse = Convert.ToString(responseObject.paymentId);
                                string resultPayment = cloverGatewayCommandHandler.GetPaymentByPaymentId(Convert.ToString(paymentIdofResponse), deviceId, posId);
                                responseObject = JsonConvert.DeserializeObject(resultPayment);
                                log.LogVariableState("responseObject", responseObject);
                                double resAmount = Convert.ToDouble(responseObject.payment.amount) * 0.01;
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responseObject.payment.cardTransaction.last4);
                                cCTransactionsPGWDTO.AuthCode = responseObject.payment.cardTransaction.authCode;
                                cCTransactionsPGWDTO.CardType = responseObject.payment.cardTransaction.extra.authorizingNetworkName;
                                cCTransactionsPGWDTO.CaptureStatus = responseObject.payment.cardTransaction.entryType;
                                cCTransactionsPGWDTO.RefNo = responseObject.payment.externalPaymentId;
                                cCTransactionsPGWDTO.RecordNo = responseObject.payment.id;
                                cCTransactionsPGWDTO.AcqRefData = "AID:" + responseObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responseObject.payment.cardTransaction.referenceId;
                                cCTransactionsPGWDTO.TokenID = responseObject.payment.cardTransaction.token;
                                //cCTransactionsPGWDTO.TextResponse = responseObject.payment.cardTransaction.state;
                                cCTransactionsPGWDTO.TextResponse = "Approval";
                                cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = (resAmount).ToString();
                                cCTransactionsPGWDTO.TipAmount = (tipAmount * 0.01).ToString();

                                if (isPrintReceiptEnabled)
                                {
                                    cloverGatewayCommandHandler.CreatePrintPaymentReceipt(cCTransactionsPGWDTO.RecordNo, deviceId, posId);
                                }

                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            }
                            else
                            {
                                log.LogMethodExit(transactionPaymentsDTO);
                                throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                            }
                        }
                        else
                        {
                            //form.Show(activeForm);
                            //SetNativeEnabled(activeForm, false);
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                            result = cloverGatewayCommandHandler.CreateDirectSale(amount, deviceId, posId, ccRequestPGWDTO.RequestID.ToString());
                            //result = cloverGatewayCommandHandler.CreateReadTip(amount, deviceId, posId);
                            //result = cloverGatewayCommandHandler.CreatePaymentWithEmailedReceipt(amount, deviceId, posId, ccRequestPGWDTO.RequestID.ToString(), testEmailAddress);
                            if (result == "Exception")
                            {
                                throw new Exception("Exception in processing Payment ");
                            }
                            else
                            {
                                dynamic responseObject = JsonConvert.DeserializeObject(result);
                                log.LogVariableState("responseObject", responseObject);
                                double resamount = Convert.ToDouble(responseObject.payment.amount) * 0.01;
                                //PaymentDTO paymentDTO = JsonConvert.DeserializeObject<PaymentDTO>(result);
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responseObject.payment.cardTransaction.last4);
                                cCTransactionsPGWDTO.AuthCode = responseObject.payment.cardTransaction.authCode;
                                cCTransactionsPGWDTO.CardType = responseObject.payment.cardTransaction.extra.authorizingNetworkName;
                                cCTransactionsPGWDTO.CaptureStatus = responseObject.payment.cardTransaction.entryType;
                                cCTransactionsPGWDTO.RefNo = responseObject.payment.externalPaymentId;
                                cCTransactionsPGWDTO.RecordNo = responseObject.payment.id;
                                cCTransactionsPGWDTO.AcqRefData = "AID:" + responseObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responseObject.payment.cardTransaction.referenceId;
                                cCTransactionsPGWDTO.TokenID = responseObject.payment.cardTransaction.token;
                                cCTransactionsPGWDTO.TextResponse = responseObject.payment.cardTransaction.state;
                                cCTransactionsPGWDTO.TranCode = trxType.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = resamount.ToString();

                                if (isPrintReceiptEnabled)
                                {
                                    cloverGatewayCommandHandler.CreatePrintPaymentReceipt(cCTransactionsPGWDTO.RecordNo, deviceId, posId);
                                }


                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = resamount;
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;

                            }
                        }

                    }
                    else if (trxType == TransactionType.TATokenRequest)
                    {
                        //form.Show(activeForm);
                        //SetNativeEnabled(activeForm, false);
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                        amount = transactionPaymentsDTO.Amount * 100;
                        if (amount < 100 || Convert.ToDouble(minPreAuth) < 1.00)
                        {
                            throw new Exception("Pre-Auth amount value should be minimum 1");
                        }
                        result = cloverGatewayCommandHandler.CreatePreAuth(amount, deviceId, posId, ccRequestPGWDTO.RequestID.ToString());
                        if (result == "Exception")
                        {
                            throw new Exception("Exception in processing Payment ");
                        }
                        else
                        {
                            dynamic responseObject = JsonConvert.DeserializeObject(result);
                            log.LogVariableState("responseObject", responseObject);
                            double resamount = Convert.ToDouble(responseObject.payment.amount) * 0.01;
                            //PaymentDTO paymentDTO = JsonConvert.DeserializeObject<PaymentDTO>(result);
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responseObject.payment.cardTransaction.last4);
                            cCTransactionsPGWDTO.AuthCode = responseObject.payment.cardTransaction.authCode;
                            cCTransactionsPGWDTO.CardType = responseObject.payment.cardTransaction.extra.authorizingNetworkName;
                            cCTransactionsPGWDTO.CaptureStatus = responseObject.payment.cardTransaction.entryType;
                            cCTransactionsPGWDTO.RefNo = responseObject.payment.externalPaymentId;
                            cCTransactionsPGWDTO.RecordNo = responseObject.payment.id;
                            cCTransactionsPGWDTO.AcqRefData = "AID:" + responseObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responseObject.payment.cardTransaction.referenceId; ;
                            cCTransactionsPGWDTO.TokenID = responseObject.payment.cardTransaction.token;
                            //cCTransactionsPGWDTO.TextResponse = responseObject.payment.cardTransaction.state;
                            cCTransactionsPGWDTO.TextResponse = "Approval";
                            cCTransactionsPGWDTO.TranCode = trxType.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = resamount.ToString();


                            SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.Amount = 0;
                            transactionPaymentsDTO.TipAmount = 0;
                            transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
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
                throw;
            }
            finally
            {
                //SetNativeEnabled(activeForm, true);
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
                CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);
                string result = cloverGatewayCommandHandler.DisplayWelcomeScreen(deviceId, posId);
                if (result == "Exception")
                {
                    log.Error("Pin Pad Reset Error");
                }
            }
            //}
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;

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

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            //Form activeForm = GetActiveForm();
            try
            {
                if (transactionPaymentsDTO != null)
                {
                    if (transactionPaymentsDTO.Amount < 0)
                    {
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Variable Refund Not Supported"));
                    }
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(4202, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Clover Card Connect Payment Gateway");
                    //{
                    //statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    try
                    {
                        //Form form = statusDisplayUi as Form;
                        //form.Show(activeForm);
                        //SetNativeEnabled(activeForm, false);
                        thr.Start();
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));

                        CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);
                        string result;
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.REFUND.ToString());
                        CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                        CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;
                        string resultPayment = cloverGatewayCommandHandler.GetPaymentByPaymentId(ccOrigTransactionsPGWDTO.RecordNo.ToString(), deviceId, posId);
                        log.LogVariableState("resultPayment", resultPayment);
                        dynamic responsePayment = JsonConvert.DeserializeObject(resultPayment);
                        double amount = (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount);
                        double refundAmount;

                        DateTime originalPaymentDate = new DateTime();
                        TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                        List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, transactionPaymentsDTO.TransactionId.ToString()));
                        List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPaymentsDTOList(searchParameters);
                        if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Any())
                        {
                            originalPaymentDate = transactionPaymentsDTOList[0].PaymentDate;
                        }
                        else
                        {
                            originalPaymentDate = ccOrigTransactionsPGWDTO.TransactionDatetime;
                        }
                        DateTime bussStartTime = utilities.getServerTime().Date.AddHours(Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")));
                        DateTime bussEndTime = bussStartTime.AddDays(1);
                        if (utilities.getServerTime() < bussStartTime)
                        {
                            bussStartTime = bussStartTime.AddDays(-1);
                            bussEndTime = bussStartTime.AddDays(1);
                        }

                        // check business date
                        if ((originalPaymentDate >= bussStartTime) && (originalPaymentDate <= bussEndTime))
                        {
                            if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) != Convert.ToDecimal(amount)))//This is the valid condition if we check refundable first. Because once it enters to this section it will do full reversal
                            {
                                log.Error("Partial Void is not possible. Please wait for the batch to settle.");//Batch is not settled
                                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Partial Void is not possible. Please wait for the batch to settle."));
                                throw new Exception(utilities.MessageUtils.getMessage("Partial Void is not possible. Please wait for the batch to settle."));
                            }
                            else
                            {
                                result = cloverGatewayCommandHandler.CreateVoid(ccOrigTransactionsPGWDTO.RecordNo.ToString(), deviceId, posId);
                                if (result == "Exception")
                                {
                                    throw new Exception("Exception in processing Payment ");
                                }
                                else
                                {
                                    dynamic responseObject = JsonConvert.DeserializeObject(result);
                                    log.LogVariableState("responseObject", responseObject);
                                    refundAmount = Convert.ToDouble(responseObject.payment.amount) * 0.01;
                                    //CloverAuthResponseDTO connectionResponse = JsonConvert.DeserializeObject<CloverAuthResponseDTO>(jsonString);
                                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                    cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                    cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responseObject.payment.cardTransaction.last4);
                                    cCTransactionsPGWDTO.AuthCode = responseObject.payment.cardTransaction.authCode;
                                    cCTransactionsPGWDTO.CardType = responseObject.payment.cardTransaction.extra.authorizingNetworkName;
                                    cCTransactionsPGWDTO.CaptureStatus = responseObject.payment.cardTransaction.entryType;
                                    cCTransactionsPGWDTO.RefNo = responseObject.payment.externalPaymentId;
                                    cCTransactionsPGWDTO.RecordNo = responseObject.payment.id;
                                    cCTransactionsPGWDTO.AcqRefData = "AID:" + responseObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responseObject.payment.cardTransaction.referenceId; ;
                                    cCTransactionsPGWDTO.TokenID = responseObject.payment.cardTransaction.token;
                                    cCTransactionsPGWDTO.TextResponse = responseObject.payment.cardTransaction.state;
                                    cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                    cCTransactionsPGWDTO.Authorize = refundAmount.ToString();

                                    SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                                    if (isPrintReceiptEnabled)
                                    {
                                        cloverGatewayCommandHandler.CreatePrintVoidReceipt(cCTransactionsPGWDTO.RecordNo, deviceId, posId);
                                    }

                                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                    ccTransactionsPGWBL.Save();
                                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                    //transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                    //transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                    //transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                    //transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                                }
                            }
                        }
                        else
                        {
                            // next business day
                            if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) != Convert.ToDecimal(amount)))//This is the valid condition if we check refundable first. Because once it enters to this section it will do full reversal
                            {
                                // next day partial refund
                                result = cloverGatewayCommandHandler.CreatePartialRefund(ccOrigTransactionsPGWDTO.RecordNo.ToString(), Convert.ToDouble(amount * 100), deviceId, posId);
                                if (result == "Exception")
                                {
                                    throw new Exception("Exception in processing Payment ");
                                }
                                else
                                {
                                    dynamic responseObject = JsonConvert.DeserializeObject(result);
                                    log.LogVariableState("responseObject", responseObject);
                                    refundAmount = Convert.ToDouble(responseObject.payment.amount) * 0.01;
                                    //CloverAuthResponseDTO connectionResponse = JsonConvert.DeserializeObject<CloverAuthResponseDTO>(jsonString);
                                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                    cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                    cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responseObject.payment.cardTransaction.last4);
                                    cCTransactionsPGWDTO.AuthCode = responseObject.payment.cardTransaction.authCode;
                                    cCTransactionsPGWDTO.CardType = responseObject.payment.cardTransaction.extra.authorizingNetworkName;
                                    cCTransactionsPGWDTO.CaptureStatus = responseObject.payment.cardTransaction.entryType;
                                    cCTransactionsPGWDTO.RefNo = responseObject.payment.externalPaymentId;
                                    cCTransactionsPGWDTO.RecordNo = responseObject.payment.id;
                                    cCTransactionsPGWDTO.AcqRefData = responseObject.payment.cardTransaction.referenceId;
                                    cCTransactionsPGWDTO.TokenID = responseObject.payment.cardTransaction.token;
                                    cCTransactionsPGWDTO.TextResponse = responseObject.payment.cardTransaction.state;
                                    cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                    cCTransactionsPGWDTO.Authorize = refundAmount.ToString();

                                    SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                                    if (isPrintReceiptEnabled)
                                    {
                                        cloverGatewayCommandHandler.CreatePrintVoidReceipt(cCTransactionsPGWDTO.RecordNo, deviceId, posId);
                                    }


                                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                    ccTransactionsPGWBL.Save();
                                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                    //transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                    //transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                    //transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                    //transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                                }
                            }
                            else if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) == Convert.ToDecimal(amount)))
                            {
                                // next day full refund = void
                                result = cloverGatewayCommandHandler.CreateVoid(ccOrigTransactionsPGWDTO.RecordNo.ToString(), deviceId, posId);
                                if (result == "Exception")
                                {
                                    throw new Exception("Exception in processing Payment ");
                                }
                                else
                                {
                                    dynamic responseObject = JsonConvert.DeserializeObject(result);
                                    log.LogVariableState("responseObject", responseObject);
                                    refundAmount = Convert.ToDouble(responseObject.payment.amount) * 0.01;
                                    //CloverAuthResponseDTO connectionResponse = JsonConvert.DeserializeObject<CloverAuthResponseDTO>(jsonString);
                                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                    cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                    cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responseObject.payment.cardTransaction.last4);
                                    cCTransactionsPGWDTO.AuthCode = responseObject.payment.cardTransaction.authCode;
                                    cCTransactionsPGWDTO.CardType = responseObject.payment.cardTransaction.extra.authorizingNetworkName;
                                    cCTransactionsPGWDTO.CaptureStatus = responseObject.payment.cardTransaction.entryType;
                                    cCTransactionsPGWDTO.RefNo = responseObject.payment.externalPaymentId;
                                    cCTransactionsPGWDTO.RecordNo = responseObject.payment.id;
                                    cCTransactionsPGWDTO.AcqRefData = "AID:" + responseObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responseObject.payment.cardTransaction.referenceId;
                                    cCTransactionsPGWDTO.TokenID = responseObject.payment.cardTransaction.token;
                                    cCTransactionsPGWDTO.TextResponse = responseObject.payment.cardTransaction.state;
                                    cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                    cCTransactionsPGWDTO.Authorize = refundAmount.ToString();

                                if (isPrintReceiptEnabled)
                                {
                                    cloverGatewayCommandHandler.CreatePrintVoidReceipt(cCTransactionsPGWDTO.RecordNo, deviceId, posId);
                                }

                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                    ccTransactionsPGWBL.Save();
                                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                    //transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                    //transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                    //transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                    //transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //statusDisplayUi.DisplayText("Error occured while Refunding the Amount");
                        log.Error("Error occured while Refunding the Amount", ex);
                        log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                        log.LogMethodExit(null, "throwing Exception");
                        throw;
                    }
                    finally
                    {
                        try
                        {
                            if (statusDisplayUi != null)
                            {
                                Application.DoEvents();
                                statusDisplayUi.CloseStatusWindow();
                            }
                        }
                        catch { }
                    }
                }
                //}
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Refunding the Amount", ex);
                log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                log.LogMethodExit(null, "throwing Exception");
                throw new Exception("Refund failed exception :" + ex.Message);
            }
            finally
            {
                CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);
                string result = cloverGatewayCommandHandler.DisplayWelcomeScreen(deviceId, posId);
                if (result == "Exception")
                {
                    log.Error("Pind Pad Reset Error");
                }
            }
        }

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

        public override TransactionPaymentsDTO PayTip(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            transactionPaymentsDTO = PerformSettlement(transactionPaymentsDTO, true);
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public TransactionPaymentsDTO VoidPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            try
            {
                if (transactionPaymentsDTO != null)
                {
                    CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);
                    string result;
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.VOID.ToString());
                    result = cloverGatewayCommandHandler.CreateVoid(transactionPaymentsDTO.PaymentId.ToString(), deviceId, posId);
                    if (result == "Exception")
                    {
                        throw new Exception("Exception in processing Payment ");
                    }
                    else
                    {
                        dynamic responseObject = JsonConvert.DeserializeObject(result);
                        //CloverAuthResponseDTO connectionResponse = JsonConvert.DeserializeObject<CloverAuthResponseDTO>(jsonString);
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responseObject.payment.cardTransaction.last4);
                        cCTransactionsPGWDTO.AuthCode = responseObject.payment.cardTransaction.authCode;
                        cCTransactionsPGWDTO.CardType = responseObject.payment.cardTransaction.extra.authorizingNetworkName;
                        cCTransactionsPGWDTO.CaptureStatus = responseObject.payment.cardTransaction.entryType;
                        cCTransactionsPGWDTO.RefNo = responseObject.payment.externalPaymentId;
                        cCTransactionsPGWDTO.RecordNo = responseObject.payment.id;
                        cCTransactionsPGWDTO.AcqRefData = "AID:" + responseObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responseObject.payment.cardTransaction.referenceId; ;
                        cCTransactionsPGWDTO.TokenID = responseObject.payment.cardTransaction.token;
                        cCTransactionsPGWDTO.TextResponse = responseObject.payment.cardTransaction.state;
                        cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                        SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                        transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                        transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                        return transactionPaymentsDTO;
                    }
                }
                else
                {
                    throw new Exception("Exception in processing Payment ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in processing Payment ");
            }
            finally
            {
                CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);
                string result = cloverGatewayCommandHandler.DisplayWelcomeScreen(deviceId, posId);
                if (result == "Exception")
                {
                    throw new Exception("Exception in processing Void Payment");
                }
            }
        }

        public override TransactionPaymentsDTO PerformSettlement(TransactionPaymentsDTO transactionPaymentsDTO, bool IsForcedSettlement = false)
        {
            log.LogMethodEntry(transactionPaymentsDTO, IsForcedSettlement);
            //Form activeForm = GetActiveForm();
            try
            {
                bool threadStarted = false;
                if (transactionPaymentsDTO != null)
                {
                    double baseAmount = transactionPaymentsDTO.Amount * 100;
                    double tipAmount = transactionPaymentsDTO.TipAmount;

                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext,isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Clover CardConnect Payment Gateway");
                    //{
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    try
                    {

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                        CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;

                        CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                        CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);


                        if (!IsForcedSettlement)
                        {
                            frmFinalizeTransaction frmFinalizeTransaction = new frmFinalizeTransaction(utilities, overallTransactionAmount, overallTipAmountEntered, Convert.ToDecimal(transactionPaymentsDTO.Amount), Convert.ToDecimal(transactionPaymentsDTO.TipAmount), transactionPaymentsDTO.CreditCardNumber, showMessageDelegate);
                            if (frmFinalizeTransaction.ShowDialog() != DialogResult.Cancel)
                            {
                                tipAmount = Convert.ToDouble(frmFinalizeTransaction.TipAmount) * 100;


                                string resultObjectCapture = cloverGatewayCommandHandler.CreateCapture(ccOrigTransactionsPGWDTO.RecordNo.ToString(), baseAmount, deviceId, posId, tipAmount);
                                if (resultObjectCapture.Equals("Exception"))
                                {
                                    throw new Exception("Error occured while performing settlement");
                                }
                                else
                                {
                                    //Form form = statusDisplayUi as Form;
                                    //form.Show(activeForm);
                                    //SetNativeEnabled(activeForm, false);  
                                    thr.Start();
                                    threadStarted = true;
                                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                                    dynamic responseObjectCapture = JsonConvert.DeserializeObject(resultObjectCapture);
                                    log.LogVariableState("responseObjectCapture", responseObjectCapture);
                                    string resultPayment = cloverGatewayCommandHandler.GetPaymentByPaymentId(ccOrigTransactionsPGWDTO.RecordNo.ToString(), deviceId, posId);
                                    dynamic responsePaymentObject = JsonConvert.DeserializeObject(resultPayment);
                                    log.LogVariableState("responsePaymentObject", responsePaymentObject);

                                    if ((string)responsePaymentObject.payment.result == "AUTH_COMPLETED")
                                    {
                                        double resamount = Convert.ToDouble(responsePaymentObject.payment.amount) * 0.01;
                                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                        cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                        cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responsePaymentObject.payment.cardTransaction.last4);
                                        cCTransactionsPGWDTO.AuthCode = responsePaymentObject.payment.cardTransaction.authCode;
                                        cCTransactionsPGWDTO.CardType = responsePaymentObject.payment.cardTransaction.extra.authorizingNetworkName;
                                        cCTransactionsPGWDTO.CaptureStatus = responsePaymentObject.payment.cardTransaction.entryType;
                                        cCTransactionsPGWDTO.RefNo = responsePaymentObject.payment.externalPaymentId;
                                        cCTransactionsPGWDTO.RecordNo = responsePaymentObject.payment.id;
                                        cCTransactionsPGWDTO.AcqRefData = "AID:" + responsePaymentObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responsePaymentObject.payment.cardTransaction.referenceId; ;
                                        cCTransactionsPGWDTO.TokenID = responsePaymentObject.payment.cardTransaction.token;
                                        //cCTransactionsPGWDTO.TextResponse = responsePaymentObject.payment.cardTransaction.state;
                                        cCTransactionsPGWDTO.TextResponse = "Approval";
                                        cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                        cCTransactionsPGWDTO.Authorize = resamount.ToString();
                                        double tip = tipAmount * 0.01;
                                        cCTransactionsPGWDTO.TipAmount = tip.ToString();

                                        ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                        ccTransactionsPGWBL.Save();
                                        if (isPrintReceiptEnabled)
                                        {
                                            cloverGatewayCommandHandler.CreatePrintPaymentReceipt(cCTransactionsPGWDTO.RecordNo, deviceId, posId);
                                        }
                                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                        transactionPaymentsDTO.Amount = resamount;
                                        transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;

                                        //if (tipAmount > 0)
                                        //{
                                        //    string resultObjectTipAdjustment = cloverGatewayCommandHandler.CreateTipAdjustment(ccOrigTransactionsPGWDTO.RecordNo.ToString(), tipAmount, deviceId, posId);
                                        //    if (resultObjectTipAdjustment.Equals("Exception"))
                                        //    {
                                        //        throw new Exception("Error occured while performing tip adjust after capture");
                                        //    }

                                        //    //ccTransactionsPGWBL.Save();

                                        //}
                                    }
                                }
                            }
                            else
                            {
                                log.LogMethodExit(transactionPaymentsDTO);
                                throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                            }
                        }
                        else
                        {
                            thr.Start();
                            threadStarted = true;
                            if (tipAmount > 0)
                            {
                                //Form form = statusDisplayUi as Form;
                                //form.Show(activeForm);
                                //SetNativeEnabled(activeForm, false);
                                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                                if (ccOrigTransactionsPGWDTO.TranCode.ToString() == TransactionType.AUTHORIZATION.ToString())
                                {
                                    string resultObjectCapture = cloverGatewayCommandHandler.CreateCapture(ccOrigTransactionsPGWDTO.RecordNo.ToString(), baseAmount, deviceId, posId, tipAmount);
                                    log.LogVariableState("resultObjectCapture", resultObjectCapture);
                                    if (resultObjectCapture.Equals("Exception"))
                                    {
                                        throw new Exception("Error occured while performing settlement");
                                    }
                                }
                                else
                                {
                                    string resultObjectTipAdjustment = cloverGatewayCommandHandler.CreateTipAdjustment(ccOrigTransactionsPGWDTO.RecordNo.ToString(), tipAmount * 100, deviceId, posId);
                                    log.LogVariableState("resultObjectTipAdjustment", resultObjectTipAdjustment);
                                    if (resultObjectTipAdjustment.Equals("Exception"))
                                    {
                                        throw new Exception("Error occured while performing tip adjust after capture");
                                    }
                                }

                                string resultPayment = cloverGatewayCommandHandler.GetPaymentByPaymentId(ccOrigTransactionsPGWDTO.RecordNo.ToString(), deviceId, posId);
                                dynamic responsePaymentObject = JsonConvert.DeserializeObject(resultPayment);
                                log.LogVariableState("responsePaymentObject", responsePaymentObject);

                                double tip = tipAmount;
                                double resamount = Convert.ToDouble(responsePaymentObject.payment.amount) * 0.01;

                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = String.Concat("XXXXXXXXXXXX", responsePaymentObject.payment.cardTransaction.last4);
                            cCTransactionsPGWDTO.AuthCode = responsePaymentObject.payment.cardTransaction.authCode;
                            cCTransactionsPGWDTO.CardType = responsePaymentObject.payment.cardTransaction.extra.authorizingNetworkName;
                            cCTransactionsPGWDTO.CaptureStatus = responsePaymentObject.payment.cardTransaction.entryType;
                            cCTransactionsPGWDTO.RefNo = responsePaymentObject.payment.externalPaymentId;
                            cCTransactionsPGWDTO.RecordNo = ccOrigTransactionsPGWDTO.RecordNo;
                            cCTransactionsPGWDTO.AcqRefData = "AID:" + responsePaymentObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responsePaymentObject.payment.cardTransaction.referenceId;
                            cCTransactionsPGWDTO.TokenID = responsePaymentObject.payment.cardTransaction.token;
                            //cCTransactionsPGWDTO.TextResponse = responsePaymentObject.payment.cardTransaction.state;
                            cCTransactionsPGWDTO.TextResponse = "Approval";
                            if (ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()))
                            {
                                cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.TIPADJUST.ToString();
                            }
                            else
                            {
                                cCTransactionsPGWDTO.TranCode = TransactionType.CAPTURE.ToString();
                            }                            
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = resamount.ToString();
                            cCTransactionsPGWDTO.TipAmount = tip.ToString();

                                ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (statusDisplayUi != null)
                            statusDisplayUi.DisplayText("Error occured while performing settlement");
                        log.Error("Error occured while performing settlement", ex);
                        log.LogMethodExit(null, "Throwing Exception " + ex);
                        throw;
                    }
                    finally
                    {
                        if (statusDisplayUi != null && threadStarted)
                        {
                            statusDisplayUi.CloseStatusWindow();
                        }
                    }
                    //}
                }
                else
                {
                    //statusDisplayUi.DisplayText("Invalid payment data.");
                    throw new Exception(utilities.MessageUtils.getMessage("Invalid payment data."));
                }
                return transactionPaymentsDTO;
            }
            finally
            {
                CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);
                string result = cloverGatewayCommandHandler.DisplayWelcomeScreen(deviceId, posId);
                if (result == "Exception")
                {
                    throw new Exception("Exception in processing Payment");
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
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage("Checking the transaction status" + ((cCRequestPGWDTO != null) ? " of TrxId:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "Clover CardConnect Payment Gateway");
                //{
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                try
                {
                    thr.Start();
                    //Form form = statusDisplayUi as Form;
                    //form.Show(activeForm);
                    //SetNativeEnabled(activeForm, false);
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);
                    CCTransactionsPGWDTO ccTransactionsPGWDTOResponse = null;


                    if (cCTransactionsPGWDTO != null)
                    {
                        log.Debug("cCTransactionsPGWDTO is not null");
                        List<CCTransactionsPGWDTO> cCTransactionsPGWDTOcapturedList = null;
                        if (!string.IsNullOrEmpty(cCTransactionsPGWDTO.RecordNo) && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.REFUND.ToString()) && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.VOID.ToString()))
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
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                        //DisplayInDevice(displayCommand, utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                        string resultPayment = cloverGatewayCommandHandler.GetPaymentByExternalPaymentId(cCTransactionsPGWDTO.InvoiceNo.ToString(), deviceId, posId);
                        if (resultPayment != "Exception")
                        {
                            dynamic responsePaymentObject = JsonConvert.DeserializeObject(resultPayment);
                            log.LogVariableState("responsePaymentObject", responsePaymentObject);

                                if (responsePaymentObject != null && responsePaymentObject.payment != null && !string.IsNullOrEmpty((string)responsePaymentObject.payment.result) && (string)responsePaymentObject.payment.result == "SUCCESS")
                                {
                                    double resamount = Convert.ToDouble(responsePaymentObject.payment.amount) * 0.01;
                                    double tipAmount = Convert.ToDouble(responsePaymentObject.payment.tipAmount) * 0.01;
                                    ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();

                                ccTransactionsPGWDTOResponse.AcctNo = String.Concat("XXXXXXXXXXXX", responsePaymentObject.payment.cardTransaction.last4);
                                ccTransactionsPGWDTOResponse.AuthCode = responsePaymentObject.payment.cardTransaction.authCode;
                                ccTransactionsPGWDTOResponse.CardType = responsePaymentObject.payment.cardTransaction.extra.authorizingNetworkName;
                                ccTransactionsPGWDTOResponse.CaptureStatus = responsePaymentObject.payment.cardTransaction.entryType;
                                ccTransactionsPGWDTOResponse.RefNo = responsePaymentObject.payment.externalPaymentId;
                                ccTransactionsPGWDTOResponse.RecordNo = responsePaymentObject.payment.id;
                                ccTransactionsPGWDTOResponse.AcqRefData = "AID:" + responsePaymentObject.payment.cardTransaction.extra.applicationIdentifier + "|Reference Id:" + responsePaymentObject.payment.cardTransaction.referenceId;
                                ccTransactionsPGWDTOResponse.TokenID = responsePaymentObject.payment.cardTransaction.token;
                                //ccTransactionsPGWDTOResponse.TextResponse = responsePaymentObject.payment.cardTransaction.state;
                                cCTransactionsPGWDTO.TextResponse = "Approval";
                                ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                                ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                                ccTransactionsPGWDTOResponse.Authorize = resamount.ToString();
                                double tip = tipAmount * 0.01;
                                ccTransactionsPGWDTOResponse.TipAmount = tip.ToString();
                            }
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
                        string resultPayment = cloverGatewayCommandHandler.GetPaymentByExternalPaymentId(cCRequestPGWDTO.RequestID.ToString(), deviceId, posId);
                    if (resultPayment != "Exception")
                    {
                        dynamic responsePaymentObject = JsonConvert.DeserializeObject(resultPayment);
                        log.LogVariableState("responsePaymentObject", responsePaymentObject);
                        if (responsePaymentObject!= null && responsePaymentObject.payment != null && !string.IsNullOrEmpty((string)responsePaymentObject.payment.result) && (string)responsePaymentObject.payment.result == "SUCCESS")
                        {
                            double resamount = Convert.ToDouble(responsePaymentObject.payment.amount) * 0.01;
                            double tipAmount = Convert.ToDouble(responsePaymentObject.payment.tipAmount) * 0.01;


                                ccTransactionsPGWDTOResponse.AcctNo = String.Concat("XXXXXXXXXXXX", responsePaymentObject.payment.cardTransaction.last4);
                                ccTransactionsPGWDTOResponse.AuthCode = responsePaymentObject.payment.cardTransaction.authCode;
                                ccTransactionsPGWDTOResponse.CardType = responsePaymentObject.payment.cardTransaction.extra.authorizingNetworkName;
                                ccTransactionsPGWDTOResponse.CaptureStatus = responsePaymentObject.payment.cardTransaction.entryType;
                                ccTransactionsPGWDTOResponse.RefNo = responsePaymentObject.payment.externalPaymentId;
                                ccTransactionsPGWDTOResponse.RecordNo = responsePaymentObject.payment.id;
                                ccTransactionsPGWDTOResponse.AcqRefData = responsePaymentObject.payment.cardTransaction.referenceId;
                                ccTransactionsPGWDTOResponse.TokenID = responsePaymentObject.payment.cardTransaction.token;
                                // ccTransactionsPGWDTOResponse.TextResponse = responsePaymentObject.payment.cardTransaction.state;
                                ccTransactionsPGWDTOResponse.TextResponse = "Approval";
                                ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                                ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                                ccTransactionsPGWDTOResponse.Authorize = resamount.ToString();
                                double tip = tipAmount * 0.01;
                                ccTransactionsPGWDTOResponse.TipAmount = tip.ToString();
                            }
                            else
                            {
                                ccTransactionsPGWDTOResponse.TextResponse = "Txn not found";
                                ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                                ccTransactionsPGWDTOResponse.RecordNo = "C";
                            }
                        }
                        else
                        {
                            ccTransactionsPGWDTOResponse.TextResponse = "Txn not found";
                            ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                            ccTransactionsPGWDTOResponse.RecordNo = "C";
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
                                //showMessageDelegate(utilities.MessageUtils.getMessage("Last transaction status check is failed. :" + ((cCRequestPGWDTO != null) ? " TransactionID:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "Last Transaction Status Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            log.Error("Last transaction check failed", ex);
                            throw;
                        }
                    }
                }
                finally
                {
                    try
                    {
                        if (statusDisplayUi != null)
                            statusDisplayUi.CloseStatusWindow();
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception three.one without throw in finally");
                        log.Error(ex);
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
                    CloverCardConnectCommandHandler cloverGatewayCommandHandler = new CloverCardConnectCommandHandler(gatewayUrl, authorization);
                    string result = cloverGatewayCommandHandler.DisplayWelcomeScreen(deviceId, posId);
                    if (result == "Exception")
                    {
                        log.Error("Error in DisplayWelcomeScreen");
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("Exception three without throw in finally");
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
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
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.CardType))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Card Type") + "       : ".PadLeft(15) + ccTransactionsPGWDTO.CardType, Alignment.Left);
                //receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Cardholder Name") + ": ".PadLeft(3) + trxPaymentsDTO.NameOnCreditCard, Alignment.Left);
                //string maskedPAN = ((string.IsNullOrEmpty(ccTransactionsPGWDTO.AcctNo) ? trxPaymentsDTO.CreditCardNumber
                //                                                             : (new String('X', 12) + ((trxPaymentsDTO.CreditCardNumber.Length > 4)
                //                                                                                     ? trxPaymentsDTO.CreditCardNumber.Substring(trxPaymentsDTO.CreditCardNumber.Length - 4)
                //                                                                                     : trxPaymentsDTO.CreditCardNumber))));
                if (!string.IsNullOrWhiteSpace(ccTransactionsPGWDTO.AcctNo))
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("PAN") + ": ".PadLeft(24) + ccTransactionsPGWDTO.AcctNo, Alignment.Left);
                }
                if (!string.IsNullOrWhiteSpace(ccTransactionsPGWDTO.CaptureStatus))
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Entry Mode") + ": ".PadLeft(13) + ccTransactionsPGWDTO.CaptureStatus, Alignment.Left);
                }

                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.AcqRefData))
                {
                    //receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Reference Id") + ": ".PadLeft(15) + ccTransactionsPGWDTO.AcqRefData, Alignment.Left);
                    string[] data = null;
                    string[] emvData = ccTransactionsPGWDTO.AcqRefData.Split('|');
                    for (int i = 0; i < emvData.Length; i++)
                    {
                        data = emvData[i].Split(':');
                        if (data[0].Equals("AID") && !string.IsNullOrEmpty(data[1]))
                        {
                            receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("AID") + ": ".PadLeft(25) + data[1], Alignment.Left);
                        }
                        else if (data[0].Equals("Reference Id") && !string.IsNullOrEmpty(data[1]))
                        {
                            receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Reference Id") + ": ".PadLeft(11) + data[1], Alignment.Left);
                        }
                        //else if (data[0].Equals("IAD") && !string.IsNullOrEmpty(data[1]))
                        //{
                        //    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("IAD") + ": ".PadLeft(25) + data[1], Alignment.Left);
                        //}
                    }
                }
                //receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("POS User") + ": ".PadLeft(15) + utilities.ExecutionContext.UserId, Alignment.Left);
                //receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage((ccTransactionsPGWDTO.RecordNo.Equals("A")) ? "APPROVED" : (ccTransactionsPGWDTO.RecordNo.Equals("B")) ? "RETRY" : "DECLINED") + "-" + ccTransactionsPGWDTO.DSIXReturnCode, Alignment.Center);
                //if (ccTransactionsPGWDTO.RecordNo.Equals("C"))
                //{
                //    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage(ccTransactionsPGWDTO.TextResponse), Alignment.Center);
                //}
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage(ccTransactionsPGWDTO.TextResponse), Alignment.Center);
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
                //if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.RecordNo) && ccTransactionsPGWDTO.RecordNo.Equals("A")) && ccTransactionsPGWDTO.TranCode.ToUpper().Equals(TransactionType.AUTHORIZATION.ToString()))
                //{
                //    receiptText += Environment.NewLine;
                //    if (!string.IsNullOrWhiteSpace(trxPaymentsDTO.ExternalSourceReference) && trxPaymentsDTO.ExternalSourceReference == "G")
                //    {
                //        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Gratuity has already been included."), Alignment.Center);
                //        receiptText += Environment.NewLine;
                //        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Additional Tip") + ": ".PadLeft(28 - utilities.MessageUtils.getMessage("Additional Tip").Length) + "_____________", Alignment.Left);
                //    }
                //    else
                //    {
                //        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Tip") + ": ".PadLeft(34 - utilities.MessageUtils.getMessage("Tip").Length) + "_____________", Alignment.Left);
                //    }
                //    receiptText += Environment.NewLine;
                //    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                          : " + "_____________", Alignment.Left);
                //}
                receiptText += Environment.NewLine;
                if (IsMerchantCopy)
                {
                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse))
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
                if ((!ccTransactionsPGWDTO.TranCode.Equals("CAPTURE") || (ccTransactionsPGWDTO.TranCode.Equals("CAPTURE") && IsMerchantCopy)))
                {
                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse))
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

    }
}
