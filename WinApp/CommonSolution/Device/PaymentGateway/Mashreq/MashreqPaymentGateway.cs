/********************************************************************************************
 * Project Name - Payment Gateway
 * Description  - Mashreq payment gateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By                    Remarks          
 *********************************************************************************************
 *2.140.3     11-Aug-2022       Prasad & Dakshakh              Created 
 *2.150.1     22-Feb-2023       Guru S A                       Kiosk Cart Enhancements
 *******************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class MashreqPaymentGateway : PaymentGateway
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
        int port;
        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();
        string errorMessageFromDevice = string.Empty;
        string errMsg = string.Empty;
        private static MashreqCommandHandler mashreqCommandHandler = null;
        private Dictionary<string, string> ErrorMessagesMap = new Dictionary<string, string>();
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
            LastTransactionCheck,//7
            DUPLICATE,
            GETTERMINALINFO,//8,
            INDEPENDENT_REFUND
        }

        public MashreqPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
    : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {

            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel   
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
            minPreAuth = utilities.getParafaitDefaults("CREDIT_CARD_MIN_PREAUTH");
            isAuthEnabled = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
            enableAutoAuthorization = utilities.getParafaitDefaults("ENABLE_AUTO_CREDITCARD_AUTHORIZATION").Equals("Y");
            string value = utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO");
            port = (string.IsNullOrEmpty(value)) ? -1 : Convert.ToInt32(value);
            log.LogVariableState("Port", port);
            mashreqCommandHandler = new MashreqCommandHandler(utilities.ExecutionContext, port);
            log.LogVariableState("authorization", isAuthEnabled);
            log.LogVariableState("enableAutoAuthorization", enableAutoAuthorization);
            //if (string.IsNullOrEmpty(authorization))
            //{
            //    throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TOKEN_ID value in configuration for authorization."));
            //}
            log.LogMethodExit(null);
        }

        public override void Initialize()
        {
            log.LogMethodEntry();
            LoadErrorMessagesMap();
            CheckLastTransactionStatus();
            log.LogMethodExit();
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            //VerifyPaymentRequest(transactionPaymentsDTO);
            PrintReceipt = true;
            statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Mashreq Payment Gateway");
            statusDisplayUi.EnableCancelButton(false);
            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
            isManual = false;
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
                                frmTransactionTypeUI frmTranType = new frmTransactionTypeUI(utilities, (cCOrgTransactionsPGWDTO == null) ? "TATokenRequest" : "Sale", transactionPaymentsDTO.Amount, showMessageDelegate);
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

                if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount >= 0)
                {
                    dynamic mashreqResponse = null;
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, trxType.ToString());

                    if (trxType == TransactionType.AUTHORIZATION)
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));

                        var requestDTO = new MashreqRequestDTO()
                        {
                            transactionAmount = amount.ToString(),
                            mrefValue = ccRequestPGWDTO.RequestID.ToString(),
                            trnxType = (int)TransactionType.AUTHORIZATION,
                            deviceId = deviceId,
                            posId = posId
                        };
                        log.LogVariableState("masreqRequest", requestDTO);
                        mashreqResponse = CreateTransactionRequest(requestDTO);
                        log.LogVariableState("masreqResponse", mashreqResponse);
                        if (!isSuccess(mashreqResponse))
                        {
                            errMsg = GetErrorMessage(mashreqResponse);
                            errMsg = string.IsNullOrWhiteSpace(errMsg) ? "Authorization: Error in Getting response from the device" : errMsg;
                            log.Error(errMsg);
                            throw new Exception(errMsg);
                        }

                        log.LogVariableState("responseObject", mashreqResponse);
                        double resamount = Convert.ToDouble(mashreqResponse.Amount) * 0.01;
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                        cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(mashreqResponse.MaskCardNumber);
                        cCTransactionsPGWDTO.AuthCode = mashreqResponse.AuthCode;
                        cCTransactionsPGWDTO.CardType = mashreqResponse.CardSchemeName;
                        cCTransactionsPGWDTO.CaptureStatus = mashreqResponse.EntryMode;
                        cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                        cCTransactionsPGWDTO.RecordNo = mashreqResponse.InvoiceNo;
                        cCTransactionsPGWDTO.TextResponse = mashreqResponse.ResponseCode;
                        cCTransactionsPGWDTO.TranCode = trxType.ToString();
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.Authorize = resamount.ToString();
                        cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(mashreqResponse.MID) + "|TID:" + mashreqResponse.TID;

                        transactionPaymentsDTO.NameOnCreditCard = mashreqResponse.CardHolderName;

                        SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                        transactionPaymentsDTO.Amount = resamount;
                        transactionPaymentsDTO.CreditCardAuthorization = mashreqResponse.AuthCode;
                        transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;



                    }
                    else if (trxType == TransactionType.SALE)
                    {
                        double resAmount = 0;
                        if (cCOrgTransactionsPGWDTO != null)
                        {
                            paymentId = cCOrgTransactionsPGWDTO.RecordNo.ToString();
                            frmFinalizeTransaction frmFinalizeTransaction = new frmFinalizeTransaction(utilities, overallTransactionAmount, overallTipAmountEntered, Convert.ToDecimal(transactionPaymentsDTO.Amount), Convert.ToDecimal(transactionPaymentsDTO.TipAmount), transactionPaymentsDTO.CreditCardNumber, showMessageDelegate);
                            if (frmFinalizeTransaction.ShowDialog() != DialogResult.Cancel)
                            {
                                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(frmFinalizeTransaction.TipAmount);
                                double tipAmount = transactionPaymentsDTO.TipAmount * 100;


                                var requestDTO = new MashreqRequestDTO()
                                {
                                    trnxType = (int)TransactionType.CAPTURE,
                                    invoiceNumber = cCOrgTransactionsPGWDTO.RecordNo,
                                    mrefValue = ccRequestPGWDTO.RequestID.ToString(),
                                    deviceId = deviceId,
                                    posId = posId,
                                    paymentId = paymentId,
                                };
                                log.LogVariableState("mashreq Request", requestDTO);


                                mashreqResponse = CreateTransactionRequest(requestDTO);

                                log.LogVariableState("mashreq Response", mashreqResponse);

                                if (!isSuccess(mashreqResponse))
                                {
                                    errMsg = GetErrorMessage(mashreqResponse);
                                    errMsg = String.IsNullOrWhiteSpace(errMsg) ? "Capture: Error in Getting response from the device" : errMsg;
                                    log.Error(errMsg);
                                    throw new Exception(errMsg);
                                }

                                log.LogVariableState("responseObject", mashreqResponse);
                                resAmount = Convert.ToDouble(mashreqResponse.Amount) * 0.01;
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(mashreqResponse.MaskCardNumber);
                                cCTransactionsPGWDTO.AuthCode = mashreqResponse.AuthCode;
                                cCTransactionsPGWDTO.CardType = mashreqResponse.CardSchemeName;
                                cCTransactionsPGWDTO.CaptureStatus = mashreqResponse.EntryMode;
                                cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                                cCTransactionsPGWDTO.RecordNo = mashreqResponse.InvoiceNo;
                                cCTransactionsPGWDTO.TextResponse = mashreqResponse.ResponseCode;
                                cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = resAmount.ToString();
                                cCTransactionsPGWDTO.TipAmount = "0";
                                cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(mashreqResponse.MID) + "|TID:" + mashreqResponse.TID;

                                transactionPaymentsDTO.NameOnCreditCard = mashreqResponse.CardHolderName;


                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.CreditCardAuthorization = mashreqResponse.AuthCode;
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
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(4741));
                            //Please do not remove the card until the device instructs to do so

                            var requestDTO = new MashreqRequestDTO()
                            {
                                transactionAmount = amount.ToString(),
                                mrefValue = ccRequestPGWDTO.RequestID.ToString(),
                                trnxType = (int)TransactionType.SALE,
                                deviceId = deviceId,
                                posId = posId,
                                paymentId = paymentId,
                            };
                            log.LogVariableState("mashreqRequest Sale:", requestDTO);

                            mashreqResponse = CreateTransactionRequest(requestDTO);
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                            log.LogVariableState("mashreqResponse sale:", mashreqResponse);
                            if (!isSuccess(mashreqResponse))
                            {
                                // something went wrong
                                // perform Last Trx check
                                dynamic LastTrxCheckResult = CreateTransactionRequest(new MashreqRequestDTO
                                {
                                    trnxType = (int)TransactionType.LastTransactionCheck,
                                    //mrefValue = string.IsNullOrWhiteSpace(mashreqResponse.MREFValue) ? ccRequestPGWDTO.RequestID.ToString() : mashreqResponse.MREFValue
                                    mrefValue = ccRequestPGWDTO.RequestID.ToString()
                                });
                                log.LogVariableState("LastTrxCheckResult sale:", LastTrxCheckResult);

                                if (isSuccess(LastTrxCheckResult))
                                {
                                    // trx succeeded
                                    log.LogVariableState("responseObject from LastTrxCheck sale:", LastTrxCheckResult);
                                    resAmount = Convert.ToDouble(LastTrxCheckResult.Amount) * 0.01;
                                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                    cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                    cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(LastTrxCheckResult.MaskCardNumber);
                                    cCTransactionsPGWDTO.AuthCode = LastTrxCheckResult.AuthCode;
                                    cCTransactionsPGWDTO.CardType = LastTrxCheckResult.CardSchemeName;
                                    cCTransactionsPGWDTO.CaptureStatus = LastTrxCheckResult.EntryMode;
                                    cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(LastTrxCheckResult.MREFValue) ? ccRequestPGWDTO.RequestID.ToString() : LastTrxCheckResult.MREFValue;
                                    cCTransactionsPGWDTO.RecordNo = LastTrxCheckResult.InvoiceNo;
                                    cCTransactionsPGWDTO.TextResponse = LastTrxCheckResult.ResponseCode;
                                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                    cCTransactionsPGWDTO.Authorize = resAmount.ToString();
                                    cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(LastTrxCheckResult.MID) + "|TID:" + LastTrxCheckResult.TID;
                                    
                                    transactionPaymentsDTO.NameOnCreditCard = LastTrxCheckResult.CardHolderName;
                                    SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                    ccTransactionsPGWBL.Save();
                                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                    transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                    transactionPaymentsDTO.CreditCardAuthorization = LastTrxCheckResult.AuthCode;
                                    transactionPaymentsDTO.Amount = resAmount;
                                    transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                    transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                                }
                                else
                                {
                                    HandleMashreqErrors(mashreqResponse, LastTrxCheckResult, TransactionType.SALE.ToString());
                                    throw new Exception(errMsg);
                                }

                            }
                            else
                            {
                                // payment succeeded; update the db
                                log.LogVariableState("responseObject sale:", mashreqResponse);
                                resAmount = Convert.ToDouble(mashreqResponse.Amount) * 0.01;
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(mashreqResponse.MaskCardNumber);
                                cCTransactionsPGWDTO.AuthCode = mashreqResponse.AuthCode;
                                cCTransactionsPGWDTO.CardType = mashreqResponse.CardSchemeName;
                                cCTransactionsPGWDTO.CaptureStatus = mashreqResponse.EntryMode;
                                //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                                cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(mashreqResponse.MREFValue) ? ccRequestPGWDTO.RequestID.ToString() : mashreqResponse.MREFValue;
                                cCTransactionsPGWDTO.RecordNo = mashreqResponse.InvoiceNo;
                                cCTransactionsPGWDTO.TextResponse = mashreqResponse.ResponseCode;
                                cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = resAmount.ToString();
                                cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(mashreqResponse.MID) + "|TID:" + mashreqResponse.TID;


                                transactionPaymentsDTO.NameOnCreditCard = mashreqResponse.CardHolderName;
                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.CreditCardAuthorization = mashreqResponse.AuthCode;
                                transactionPaymentsDTO.Amount = resAmount;
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            }
                        }
                    }
                }
                else
                {
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    dynamic mashreqResponse = null;
                    trxType = TransactionType.REFUND;
                    //CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    //CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;
                    //double amount = (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount);
                    double refundAmount;
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, trxType.ToString());
                    refundAmount = - amount;
                    var requestDTO = new MashreqRequestDTO()
                    {
                        trnxType = (int)TransactionType.INDEPENDENT_REFUND,
                        deviceId = deviceId,
                        posId = posId,
                        transactionAmount = refundAmount.ToString(),
                        mrefValue = ccRequestPGWDTO.RequestID.ToString(),
                        //mrefValue = ccOrigTransactionsPGWDTO.RefNo.ToString(),
                        authCode = string.Empty

                    };
                    log.LogVariableState("requestDTO", requestDTO);

                    mashreqResponse = CreateTransactionRequest(requestDTO);

                    log.LogVariableState("mashreqResponse", mashreqResponse);
                    if (!isSuccess(mashreqResponse))
                    {
                        // something went wrong
                        // perform Last Trx check
                        dynamic LastTrxCheckResult = CreateTransactionRequest(new MashreqRequestDTO
                        {
                            trnxType = (int)TransactionType.LastTransactionCheck,
                            mrefValue = ccRequestPGWDTO.RequestID.ToString()
                        });

                        log.LogVariableState("LastTrxCheckResult in Refund", LastTrxCheckResult);


                        if (isSuccess(LastTrxCheckResult))
                        {
                            // trx succeeded
                            log.LogVariableState("responseObject from LastTrxCheck in Refund", LastTrxCheckResult);
                            refundAmount = Convert.ToDouble(LastTrxCheckResult.Amount) * 0.01;
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(LastTrxCheckResult.MaskCardNumber);
                            cCTransactionsPGWDTO.AuthCode = LastTrxCheckResult.AuthCode;
                            cCTransactionsPGWDTO.CardType = LastTrxCheckResult.CardSchemeName;
                            cCTransactionsPGWDTO.CaptureStatus = LastTrxCheckResult.EntryMode;
                            cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(LastTrxCheckResult.MREFValue) ? ccRequestPGWDTO.RequestID.ToString() : LastTrxCheckResult.MREFValue;
                            //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                            cCTransactionsPGWDTO.RecordNo = LastTrxCheckResult.InvoiceNo;
                            cCTransactionsPGWDTO.TextResponse = LastTrxCheckResult.ResponseCode;
                            cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                            cCTransactionsPGWDTO.AcqRefData = "|MID:" + LastTrxCheckResult.MID + "|TID:" + LastTrxCheckResult.TID;

                            transactionPaymentsDTO.NameOnCreditCard = LastTrxCheckResult.CardHolderName;

                            SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.CreditCardAuthorization = LastTrxCheckResult.AuthCode;
                            transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize) * -1;
                            transactionPaymentsDTO.TipAmount = ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount == null ? 0 : Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount) * -1;
                            transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;

                        }
                        else
                        {
                            // trx failed
                            HandleMashreqErrors(mashreqResponse, LastTrxCheckResult, TransactionType.REFUND.ToString());
                            throw new Exception(errMsg);
                        }

                    }
                    else
                    {
                        // Refund Succeeded
                        log.LogVariableState("responseObject", mashreqResponse);
                        refundAmount = Convert.ToDouble(mashreqResponse.Amount) * 0.01;
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                        cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(mashreqResponse.MaskCardNumber);
                        cCTransactionsPGWDTO.AuthCode = mashreqResponse.AuthCode;
                        cCTransactionsPGWDTO.CardType = mashreqResponse.CardSchemeName;
                        cCTransactionsPGWDTO.CaptureStatus = mashreqResponse.EntryMode;
                        cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(mashreqResponse.MREFValue) ? ccRequestPGWDTO.RequestID.ToString() : mashreqResponse.MREFValue;
                        //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                        cCTransactionsPGWDTO.RecordNo = mashreqResponse.InvoiceNo;
                        cCTransactionsPGWDTO.TextResponse = mashreqResponse.ResponseCode;
                        cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.Authorize = refundAmount.ToString();

                        transactionPaymentsDTO.NameOnCreditCard = mashreqResponse.CardHolderName;
                        SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                        transactionPaymentsDTO.CreditCardAuthorization = mashreqResponse.AuthCode;
                        transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize)* - 1;
                        transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount)* - 1;
                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;

                    }
                    //log.Fatal("Exception Inorrect object passed");
                    //throw new Exception("Exception in processing Payment ");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                log.Error(ex);
                throw new Exception(message, ex);
            }
            finally
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        private void HandleMashreqErrors(dynamic mashreqResponse, dynamic LastTrxCheckResult, string trxType)
        {
            log.LogMethodEntry(mashreqResponse, LastTrxCheckResult);
            try
            {
                string mashreqErrorMessage = string.Empty;
                errMsg = string.Empty;
                // trx failed
                if (mashreqResponse != null)
                {
                    if (!string.IsNullOrWhiteSpace(mashreqResponse.ResponseCode) && !mashreqResponse.ResponseCode.Equals("APPROVED"))
                    {
                        mashreqErrorMessage = mashreqResponse.ResponseCode;
                    }

                    if (!string.IsNullOrWhiteSpace(mashreqResponse.HostActionCodeMsg) && !mashreqResponse.HostActionCodeMsg.Equals("APPROVAL"))
                    {
                        mashreqErrorMessage += "," + mashreqResponse.HostActionCodeMsg;
                    }

                    if (!string.IsNullOrWhiteSpace(mashreqResponse.ErrorCode) && !mashreqResponse.ErrorCode.Equals("E000"))
                    {
                        mashreqErrorMessage += string.IsNullOrWhiteSpace(mashreqErrorMessage) ? GetMashreqErrorMessage(mashreqResponse.ErrorCode.Trim()) :
                               "," + GetMashreqErrorMessage(mashreqResponse.ErrorCode.Trim());
                    }
                }
                if (LastTrxCheckResult != null)
                {
                    string LastrxCheckErrorMessage = GetErrorMessage(LastTrxCheckResult);
                    errMsg = "Error in Last trx check: " + LastrxCheckErrorMessage;
                }

                errMsg += !string.IsNullOrWhiteSpace(mashreqErrorMessage) ? $" |Error in {trxType} trx:" + mashreqErrorMessage : string.Empty;
                log.Error($"Error in Last trx check {trxType}: {errMsg}");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                errMsg = ex.Message;
            }
            log.LogMethodExit();
        }
        private void GetTerminalInfo()
        {
            log.LogMethodEntry();
            dynamic mashreqResponse = CreateTransactionRequest(new MashreqRequestDTO()
            {
                trnxType = (int)TransactionType.GETTERMINALINFO
            });

            log.LogVariableState("mashreqResponse", mashreqResponse);
            if (mashreqResponse == null || !mashreqResponse.ErrorCode.Equals("E000"))
            {
                errMsg = GetErrorMessage(mashreqResponse);
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.LogMethodExit();
        }
        private void PerformLastTransactionCheck()
        {
            log.LogMethodEntry();
            dynamic mashreqResponse = CreateTransactionRequest(new MashreqRequestDTO()
            {
                trnxType = (int)TransactionType.LastTransactionCheck
            });

            log.LogVariableState("mashreqResponse", mashreqResponse);
            if (mashreqResponse == null || !mashreqResponse.ErrorCode.Equals("E000"))
            {
                errMsg = GetErrorMessage(mashreqResponse);
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.LogMethodExit();
        }

        private string GetErrorMessage(dynamic mashreqResponse)
        {
            log.LogMethodEntry(mashreqResponse);
            string errMsgFromDevice;
            if (mashreqResponse.GetType().GetProperty("Exception") != null)
            {
                errMsgFromDevice = mashreqResponse.Exception.InnerException.Message.ToString();
            }
            else if (mashreqResponse != null && !string.IsNullOrWhiteSpace(mashreqResponse.ErrorCode))
            {
                errMsgFromDevice = GetMashreqErrorMessage(mashreqResponse.ErrorCode.ToString());
            }
            else
            {
                errMsgFromDevice = "Payment failed, Please try again";
            }
        
            log.LogMethodExit(errMsgFromDevice);
            return errMsgFromDevice;
        }
        
        private string GetMashreqErrorMessage(string errorCode)
        {
            log.LogMethodEntry(errorCode);
            string responseText = "";
            try
            {
                if (ErrorMessagesMap.ContainsKey(errorCode))
                {
                    ErrorMessagesMap.TryGetValue(errorCode, out responseText);
                }
                else
                {
                    responseText = "Payment Declined, Please try again";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(responseText);
            return responseText;
        }

        private void LoadErrorMessagesMap()
        {
            log.LogMethodEntry();
            try
            {
                ErrorMessagesMap.Add("E000", "No Error");
                ErrorMessagesMap.Add("E001", "Terminal Not Initialized");
                ErrorMessagesMap.Add("E002", "Batch Full");
                ErrorMessagesMap.Add("E003", "Card Not removed from Card Reader");
                ErrorMessagesMap.Add("E004", "Incorrect Amount Received");
                ErrorMessagesMap.Add("E005", "Invalid Message Type Received");
                ErrorMessagesMap.Add("E007", "XML Format Error");
                ErrorMessagesMap.Add("E008", "Expired Card");
                ErrorMessagesMap.Add("E009", "Card Not Supported");
                ErrorMessagesMap.Add("E010", "Transaction being completed against wrong transaction type");
                ErrorMessagesMap.Add("E011", "Low Battery");
                ErrorMessagesMap.Add("E012", "Data Base Error Exception");
                ErrorMessagesMap.Add("E013", "Invalid Track");
                ErrorMessagesMap.Add("E014", "Customer Cancellation");
                ErrorMessagesMap.Add("E015", "Card Reader Time Out");
                ErrorMessagesMap.Add("E016", "Pin TimeOut");
                ErrorMessagesMap.Add("E017", "Invalid Expiry");
                ErrorMessagesMap.Add("E018", "Card not supported (Bin Not found)");
                ErrorMessagesMap.Add("E019", "AID Not Supported");
                ErrorMessagesMap.Add("E020", "Wrong Password (Need to check)");
                ErrorMessagesMap.Add("E021", "Chip Read Error");
                ErrorMessagesMap.Add("E022", "Service Code Check Error");
                ErrorMessagesMap.Add("E023", "Connection Error");
                ErrorMessagesMap.Add("E024", "Send Exception");
                ErrorMessagesMap.Add("E025", "Receive Exception");
                ErrorMessagesMap.Add("E026", "Invalid Receipt Number");
                ErrorMessagesMap.Add("E027", "Transaction already voided");
                ErrorMessagesMap.Add("E028", "Transaction not found");
                ErrorMessagesMap.Add("E029", "Wrong N Digits");
                ErrorMessagesMap.Add("E030", "Maximum Amount Digits Exceeded");
                ErrorMessagesMap.Add("E031", "ATM Only Exception");
                ErrorMessagesMap.Add("E032", "Amount Not Matching");
                ErrorMessagesMap.Add("E033", "Disk Exception (Remote Program Download)");
                ErrorMessagesMap.Add("E034", "Decompression Exception");
                ErrorMessagesMap.Add("E035", "reversal Incomplete");
                ErrorMessagesMap.Add("E036", "Card Removed during Transaction");
                ErrorMessagesMap.Add("E037", "Response parse Error");
                ErrorMessagesMap.Add("E038", "Reversal Send Exception");
                ErrorMessagesMap.Add("E039", "Reversal Receive Exception");
                ErrorMessagesMap.Add("E040", "Crypto Error");
                ErrorMessagesMap.Add("E041", "Batch Empty");
                ErrorMessagesMap.Add("E042", "Swipe Card Only");
                ErrorMessagesMap.Add("E043", "Run Time Exception");
                ErrorMessagesMap.Add("E044", "Host/EMV Response Code Error");
                ErrorMessagesMap.Add("E045", "Date Time Sync Error");
                ErrorMessagesMap.Add("E046", "ECR DB Error");
                ErrorMessagesMap.Add("E053", "Transaction not permitted at the terminal permissions/ Access denied");
                ErrorMessagesMap.Add("E055", "ICC Exception error");
                ErrorMessagesMap.Add("E057", "Permission Exception");
                ErrorMessagesMap.Add("E058", "Exceed Max Amount Exception");
                ErrorMessagesMap.Add("E059", "Operation Cancellation");
                ErrorMessagesMap.Add("E063", "Miss Match Data");
                ErrorMessagesMap.Add("E064", "Sim Configuration Error/ Sim Slot Exception");
                ErrorMessagesMap.Add("E065", "Gprs Restart Exception");
                ErrorMessagesMap.Add("E067", "Call last transaction status");
                ErrorMessagesMap.Add("E069", "Mref value is Mismatch");
                ErrorMessagesMap.Add("E070", "Mref not Valid");
                ErrorMessagesMap.Add("E071", "EMV Processing Error");
                ErrorMessagesMap.Add("E073", "Connection Interface Exhausted");
                ErrorMessagesMap.Add("E074", "EDW: Limit of transaction count per Terminal ID in 24 hours exceeded");
                ErrorMessagesMap.Add("E075", "EDW: Limit of transaction count per Terminal ID in 1 hour exceeded");
                ErrorMessagesMap.Add("E076", "EDW: Max transaction amount exceeded");
                ErrorMessagesMap.Add("E077", "E-11 : Invalid URN Number");
                ErrorMessagesMap.Add("E078", "E-11: Invalid OTP");
                ErrorMessagesMap.Add("E079", "App is getting crash");
                ErrorMessagesMap.Add("E080", "General Error");
                ErrorMessagesMap.Add("E99", "DLL Timeout");
                ErrorMessagesMap.Add("E180", "No device detected");
                ErrorMessagesMap.Add("E181", "USB Re-connecting");
                ErrorMessagesMap.Add("E182", "USB Disconnected");
                ErrorMessagesMap.Add("E06", "No response from EFT device [error generated from DLL]");
                ErrorMessagesMap.Add("E060", "Invalid Card Number");
                ErrorMessagesMap.Add("E083", "Detect card Failed");
                ErrorMessagesMap.Add("E125", "Application Crash or terminated");
                ErrorMessagesMap.Add("E126", "General Error");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                PrintReceipt = true;
                if (transactionPaymentsDTO != null)
                {
                    if (transactionPaymentsDTO.Amount < 0)
                    {
                        log.Error("Variable Refund Not Supported");
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Variable Refund Not Supported"));
                    }
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Mashreq Payment Gateway");
                    statusDisplayUi.EnableCancelButton(false);
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();

                    dynamic mashreqResponse;
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.REFUND.ToString());
                    CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;
                    double amount = (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount);
                    double refundAmount;

                    DateTime originalPaymentDate = ccOrigTransactionsPGWDTO.TransactionDatetime;
                    TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                    List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, transactionPaymentsDTO.TransactionId.ToString()));
                    List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPaymentsDTOList(searchParameters);
                    if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Any())
                    {
                        originalPaymentDate = transactionPaymentsDTOList[0].PaymentDate;
                    }
                    log.LogVariableState("originalPaymentDate", originalPaymentDate);

                    #region DaysCalculation_for_refund_void
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
                            var requestDTO = new MashreqRequestDTO()
                            {

                                trnxType = (int)TransactionType.VOID,
                                transactionAmount = (amount * 100).ToString(),
                                invoiceNumber = ccOrigTransactionsPGWDTO.RecordNo,
                                //mrefValue = ccOrigTransactionsPGWDTO.RefNo.ToString(),
                                mrefValue = cCRequestPGWDTO.RequestID.ToString(),
                                deviceId = deviceId,
                                posId = posId,
                            };
                            log.LogVariableState("VOID requestDTO", requestDTO);

                            mashreqResponse = CreateTransactionRequest(requestDTO);

                            log.LogVariableState(" VOID mashreqResponse", mashreqResponse);
                            if (!isSuccess(mashreqResponse))
                            {
                                // something went wrong
                                // perform Last Trx check
                                dynamic LastTrxCheckResult = CreateTransactionRequest(new MashreqRequestDTO
                                {
                                    trnxType = (int)TransactionType.LastTransactionCheck,
                                    mrefValue = cCRequestPGWDTO.RequestID.ToString()
                                });

                                log.LogVariableState("VOID LastTrxCheckResult", LastTrxCheckResult);


                                if (isSuccess(LastTrxCheckResult))
                                {
                                    // trx succeeded
                                    log.LogVariableState("responseObject from VOID LastTrxCheck", LastTrxCheckResult);
                                    refundAmount = Convert.ToDouble(LastTrxCheckResult.Amount) * 0.01;
                                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                    cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                    cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(LastTrxCheckResult.MaskCardNumber);
                                    cCTransactionsPGWDTO.AuthCode = LastTrxCheckResult.AuthCode;
                                    cCTransactionsPGWDTO.CardType = LastTrxCheckResult.CardSchemeName;
                                    cCTransactionsPGWDTO.CaptureStatus = LastTrxCheckResult.EntryMode;
                                    cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(LastTrxCheckResult.MREFValue) ? cCRequestPGWDTO.RequestID.ToString() : LastTrxCheckResult.MREFValue;
                                    //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                                    cCTransactionsPGWDTO.RecordNo = LastTrxCheckResult.InvoiceNo;
                                    cCTransactionsPGWDTO.TextResponse = LastTrxCheckResult.ResponseCode;
                                    cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                    cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                    cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(LastTrxCheckResult.MID) + "|TID:" + LastTrxCheckResult.TID;

                                    transactionPaymentsDTO.NameOnCreditCard = LastTrxCheckResult.CardHolderName;
                                    SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                    ccTransactionsPGWBL.Save();
                                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                    transactionPaymentsDTO.CreditCardAuthorization = LastTrxCheckResult.AuthCode;
                                    transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                    transactionPaymentsDTO.TipAmount = ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount == null ? 0 : Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                    transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                                }
                                else
                                {
                                    // trx failed
                                    //errMsg = GetErrorMessage(LastTrxCheckResult);
                                    HandleMashreqErrors(mashreqResponse, LastTrxCheckResult, TransactionType.VOID.ToString());
                                    log.Error($"Error in Last trx check Void: {errMsg}");
                                    throw new Exception(errMsg);
                                }

                            }
                            else
                            {
                                // VOID Succeeded
                                log.LogVariableState("responseObject VOID", mashreqResponse);
                                refundAmount = Convert.ToDouble(mashreqResponse.Amount) * 0.01;
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(mashreqResponse.MaskCardNumber);
                                cCTransactionsPGWDTO.AuthCode = mashreqResponse.AuthCode;
                                cCTransactionsPGWDTO.CardType = mashreqResponse.CardSchemeName;
                                cCTransactionsPGWDTO.CaptureStatus = mashreqResponse.EntryMode;
                                cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(mashreqResponse.MREFValue) ? cCRequestPGWDTO.RequestID.ToString() : mashreqResponse.MREFValue;
                                //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                                cCTransactionsPGWDTO.RecordNo = mashreqResponse.InvoiceNo;
                                cCTransactionsPGWDTO.TextResponse = mashreqResponse.ResponseCode;
                                cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(mashreqResponse.MID) + "|TID:" + mashreqResponse.TID;


                                transactionPaymentsDTO.NameOnCreditCard = mashreqResponse.CardHolderName;
                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.CreditCardAuthorization = mashreqResponse.AuthCode;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.TipAmount = ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount == null ? 0 : Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;

                            }
                        }
                    }
                    else
                    {
                        // next business day
                        if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) != Convert.ToDecimal(amount)))//This is the valid condition if we check refundable first. Because once it enters to this section it will do full reversal
                        {
                            var requestDTO = new MashreqRequestDTO()
                            {
                                trnxType = (int)TransactionType.REFUND,
                                deviceId = deviceId,
                                posId = posId,
                                transactionAmount = (amount * 100).ToString(),
                                mrefValue = cCRequestPGWDTO.RequestID.ToString(),
                                //mrefValue = ccOrigTransactionsPGWDTO.RefNo.ToString(),
                                authCode = ccOrigTransactionsPGWDTO.AuthCode

                            };
                            log.LogVariableState("requestDTO", requestDTO);

                            mashreqResponse = CreateTransactionRequest(requestDTO);

                            log.LogVariableState("mashreqResponse", mashreqResponse);
                            if (!isSuccess(mashreqResponse))
                            {
                                // something went wrong
                                // perform Last Trx check
                                dynamic LastTrxCheckResult = CreateTransactionRequest(new MashreqRequestDTO
                                {
                                    trnxType = (int)TransactionType.LastTransactionCheck,
                                    mrefValue = cCRequestPGWDTO.RequestID.ToString()
                                });

                                log.LogVariableState("LastTrxCheckResult in Refund", LastTrxCheckResult);


                                if (isSuccess(LastTrxCheckResult))
                                {
                                    // trx succeeded
                                    log.LogVariableState("responseObject from LastTrxCheck in Refund", LastTrxCheckResult);
                                    refundAmount = Convert.ToDouble(LastTrxCheckResult.Amount) * 0.01;
                                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                    cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                    cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(LastTrxCheckResult.MaskCardNumber);
                                    cCTransactionsPGWDTO.AuthCode = LastTrxCheckResult.AuthCode;
                                    cCTransactionsPGWDTO.CardType = LastTrxCheckResult.CardSchemeName;
                                    cCTransactionsPGWDTO.CaptureStatus = LastTrxCheckResult.EntryMode;
                                    cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(LastTrxCheckResult.MREFValue) ? cCRequestPGWDTO.RequestID.ToString() : LastTrxCheckResult.MREFValue;
                                    //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                                    cCTransactionsPGWDTO.RecordNo = LastTrxCheckResult.InvoiceNo;
                                    cCTransactionsPGWDTO.TextResponse = LastTrxCheckResult.ResponseCode;
                                    cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                    cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                    cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(LastTrxCheckResult.MID) + "|TID:" + LastTrxCheckResult.TID;

                                    transactionPaymentsDTO.NameOnCreditCard = LastTrxCheckResult.CardHolderName;

                                    SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                    ccTransactionsPGWBL.Save();
                                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                    transactionPaymentsDTO.CreditCardAuthorization = LastTrxCheckResult.AuthCode;
                                    transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                    transactionPaymentsDTO.TipAmount = ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount == null ? 0 : Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                    transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;

                                }
                                else
                                {
                                    // trx failed
                                    HandleMashreqErrors(mashreqResponse, LastTrxCheckResult, TransactionType.REFUND.ToString());
                                    log.Error(errMsg);
                                    throw new Exception(errMsg);
                                }

                            }
                            else
                            {
                                // Refund Succeeded
                                log.LogVariableState("responseObject", mashreqResponse);
                                refundAmount = Convert.ToDouble(mashreqResponse.Amount) * 0.01;
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(mashreqResponse.MaskCardNumber);
                                cCTransactionsPGWDTO.AuthCode = mashreqResponse.AuthCode;
                                cCTransactionsPGWDTO.CardType = mashreqResponse.CardSchemeName;
                                cCTransactionsPGWDTO.CaptureStatus = mashreqResponse.EntryMode;
                                cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(mashreqResponse.MREFValue) ? cCRequestPGWDTO.RequestID.ToString() : mashreqResponse.MREFValue;
                                //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                                cCTransactionsPGWDTO.RecordNo = mashreqResponse.InvoiceNo;
                                cCTransactionsPGWDTO.TextResponse = mashreqResponse.ResponseCode;
                                cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                
                                transactionPaymentsDTO.NameOnCreditCard = mashreqResponse.CardHolderName;
                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.CreditCardAuthorization = mashreqResponse.AuthCode;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;

                            }
                            #region old code
                            //if (mashreqResponse != null)
                            //{
                            //    log.LogVariableState("responseObject", mashreqResponse);
                            //    refundAmount = Convert.ToDouble(mashreqResponse.amount) * 0.01;
                            //    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            //    cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                            //    cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(mashreqResponse.MaskCardNumber);
                            //    cCTransactionsPGWDTO.AuthCode = mashreqResponse.AuthCode;
                            //    cCTransactionsPGWDTO.CardType = mashreqResponse.CardSchemeName;
                            //    cCTransactionsPGWDTO.CaptureStatus = mashreqResponse.EntryMode;
                            //    cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(mashreqResponse.MREFValue) ? cCRequestPGWDTO.RequestID.ToString() : mashreqResponse.MREFValue;
                            //    //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                            //    cCTransactionsPGWDTO.RecordNo = mashreqResponse.InvoiceNo;
                            //    cCTransactionsPGWDTO.TextResponse = mashreqResponse.ResponseCode;
                            //    cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                            //    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            //    cCTransactionsPGWDTO.Authorize = refundAmount.ToString();

                            //    SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                            //    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            //    ccTransactionsPGWBL.Save();
                            //    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            //    transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                            //    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            //    transactionPaymentsDTO.CreditCardAuthorization = mashreqResponse.AuthCode;
                            //    transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                            //    transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                            //    transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            //}
                            #endregion
                        }
                        else if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) == Convert.ToDecimal(amount)))
                        {
                            var requestDTO = new MashreqRequestDTO()
                            {
                                transactionAmount = (amount * 100).ToString(),
                                authCode = ccOrigTransactionsPGWDTO.AuthCode,
                                trnxType = (int)TransactionType.REFUND,
                                mrefValue = cCRequestPGWDTO.RequestID.ToString(),
                                deviceId = deviceId,
                                posId = posId,
                            };
                            log.LogVariableState("requestDTO", requestDTO);

                            mashreqResponse = CreateTransactionRequest(requestDTO);

                            log.LogVariableState("mashreqResponse", mashreqResponse);
                            if (!isSuccess(mashreqResponse))
                            {
                                dynamic LastTrxCheckResult = CreateTransactionRequest(new MashreqRequestDTO
                                {
                                    trnxType = (int)TransactionType.LastTransactionCheck,
                                    mrefValue = cCRequestPGWDTO.RequestID.ToString()
                                });

                                log.LogVariableState("LastTrxCheckResult in Refund", LastTrxCheckResult);


                                if (isSuccess(LastTrxCheckResult))
                                {
                                    // trx succeeded
                                    log.LogVariableState("responseObject from LastTrxCheck in Refund", LastTrxCheckResult);
                                    refundAmount = Convert.ToDouble(LastTrxCheckResult.Amount) * 0.01;
                                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                    cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                    cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(LastTrxCheckResult.MaskCardNumber);
                                    cCTransactionsPGWDTO.AuthCode = LastTrxCheckResult.AuthCode;
                                    cCTransactionsPGWDTO.CardType = LastTrxCheckResult.CardSchemeName;
                                    cCTransactionsPGWDTO.CaptureStatus = LastTrxCheckResult.EntryMode;
                                    cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(LastTrxCheckResult.MREFValue) ? cCRequestPGWDTO.RequestID.ToString() : LastTrxCheckResult.MREFValue;
                                    //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                                    cCTransactionsPGWDTO.RecordNo = LastTrxCheckResult.InvoiceNo;
                                    cCTransactionsPGWDTO.TextResponse = LastTrxCheckResult.ResponseCode;
                                    cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                    cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                    cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(LastTrxCheckResult.MID) + "|TID:" + LastTrxCheckResult.TID;
                                    transactionPaymentsDTO.NameOnCreditCard = LastTrxCheckResult.CardHolderName;
                                    SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);


                                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                    ccTransactionsPGWBL.Save();
                                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                    transactionPaymentsDTO.CreditCardAuthorization = LastTrxCheckResult.AuthCode;
                                    transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                    transactionPaymentsDTO.TipAmount = ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount == null ? 0 : Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                    transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;

                                }
                                else
                                {
                                    // trx failed
                                    HandleMashreqErrors(mashreqResponse, LastTrxCheckResult, TransactionType.REFUND.ToString());
                                    log.Error(errMsg);
                                    throw new Exception(errMsg);
                                }

                            }
                            else
                            {
                                //refund succeeded
                                log.LogVariableState("responseObject", mashreqResponse);
                                refundAmount = Convert.ToDouble(mashreqResponse.Amount) * 0.01;
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(mashreqResponse.MaskCardNumber);
                                cCTransactionsPGWDTO.AuthCode = mashreqResponse.AuthCode;
                                cCTransactionsPGWDTO.CardType = mashreqResponse.CardSchemeName;
                                cCTransactionsPGWDTO.CaptureStatus = mashreqResponse.EntryMode;
                                cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(mashreqResponse.MREFValue) ? cCRequestPGWDTO.RequestID.ToString() : mashreqResponse.MREFValue;
                                //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                                cCTransactionsPGWDTO.RecordNo = mashreqResponse.InvoiceNo;

                                cCTransactionsPGWDTO.TextResponse = mashreqResponse.ResponseCode;
                                cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(mashreqResponse.MID) + "|TID:" + mashreqResponse.TID;

                                transactionPaymentsDTO.NameOnCreditCard = mashreqResponse.CardHolderName;
                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.CreditCardAuthorization = mashreqResponse.AuthCode;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;

                            }
                        }
                    }

                    #endregion
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                statusDisplayUi.DisplayText("Error occured while Refunding the Amount");
                log.Error("Error occured while Refunding the Amount", ex);
                log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                log.LogMethodExit(null, "throwing Exception");
                throw new Exception("Refund failed exception :" + ex.Message);
            }
            finally
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
            }
        }

        private bool isSuccess(dynamic response)
        {
            // new method introduced
            try
            {
                log.LogMethodEntry(response);
                if (response == null)
                {
                    log.Error("Response was null");
                    log.LogMethodExit(false);
                    return false;
                }

                if (response.ErrorCode != null && response.ErrorCode.Equals("E000") &&
                    response.ResponseCode != null && response.ResponseCode.Equals("APPROVED") &&
                    response.HostActionCode != null && response.HostActionCode.Equals("00") &&
                    response.HostActionCodeMsg != null && response.HostActionCodeMsg.Equals("APPROVAL"))
                {
                    log.LogMethodExit(true);
                    return true;
                }
                log.Error("Error Occured processing transaction. Please check the response object for details");
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(false);
                return false;
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
                CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;
                if (transactionPaymentsDTO != null)
                {
                    dynamic mashreqResponse;
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.VOID.ToString());

                    var requestDTO = new MashreqRequestDTO()
                    {
                        trnxType = (int)TransactionType.VOID,
                        deviceId = deviceId,
                        posId = posId,
                    };
                    log.LogVariableState("requestDTO", requestDTO);

                    mashreqResponse = CreateTransactionRequest(requestDTO);

                    log.LogVariableState("mashreqResponse", mashreqResponse);
                    if (!isSuccess(mashreqResponse))
                    {
                        // something went wrong
                        // perform Last Trx check
                        dynamic LastTrxCheckResult = CreateTransactionRequest(new MashreqRequestDTO
                        {
                            trnxType = (int)TransactionType.LastTransactionCheck,
                            mrefValue = cCRequestPGWDTO.RequestID.ToString()
                        });

                        log.LogVariableState("VOID LastTrxCheckResult", LastTrxCheckResult);


                        if (isSuccess(LastTrxCheckResult))
                        {
                            // trx succeeded
                            log.LogVariableState("responseObject from VOID LastTrxCheck", LastTrxCheckResult);
                            double refundAmount = Convert.ToDouble(LastTrxCheckResult.Amount) * 0.01;
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(LastTrxCheckResult.MaskCardNumber);
                            cCTransactionsPGWDTO.AuthCode = LastTrxCheckResult.AuthCode;
                            cCTransactionsPGWDTO.CardType = LastTrxCheckResult.CardSchemeName;
                            cCTransactionsPGWDTO.CaptureStatus = LastTrxCheckResult.EntryMode;
                            cCTransactionsPGWDTO.RefNo = string.IsNullOrWhiteSpace(LastTrxCheckResult.MREFValue) ? cCRequestPGWDTO.RequestID.ToString() : LastTrxCheckResult.MREFValue;
                            //cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                            cCTransactionsPGWDTO.RecordNo = LastTrxCheckResult.InvoiceNo;
                            cCTransactionsPGWDTO.TextResponse = LastTrxCheckResult.ResponseCode;
                            cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                            cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(LastTrxCheckResult.MID) + "|TID:" + LastTrxCheckResult.TID;

                            transactionPaymentsDTO.NameOnCreditCard = LastTrxCheckResult.CardHolderName;
                            SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.CreditCardAuthorization = LastTrxCheckResult.AuthCode;
                            transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                            transactionPaymentsDTO.TipAmount = ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount == null ? 0 : Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                            transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                            log.LogMethodExit(transactionPaymentsDTO);
                            return transactionPaymentsDTO;
                        }
                        else
                        {
                            // trx failed
                            errMsg = GetErrorMessage(LastTrxCheckResult);
                            log.Error($"Error in Last trx check Void: {errMsg}");
                            throw new Exception(errMsg);
                        }
                    }
                    else
                    {
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(mashreqResponse.MaskCardNumber);
                        cCTransactionsPGWDTO.AuthCode = mashreqResponse.AuthCode;
                        cCTransactionsPGWDTO.CardType = mashreqResponse.CardSchemeName;
                        cCTransactionsPGWDTO.CaptureStatus = mashreqResponse.EntryMode;
                        cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                        cCTransactionsPGWDTO.RecordNo = mashreqResponse.InvoiceNo;
                        cCTransactionsPGWDTO.TextResponse = mashreqResponse.ResponseCode;
                        cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();

                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        transactionPaymentsDTO.NameOnCreditCard = mashreqResponse.CardHolderName;
                        SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                        transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                        transactionPaymentsDTO.TipAmount = ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount == null ? 0 : Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                        transactionPaymentsDTO.CreditCardAuthorization = mashreqResponse.AuthCode;

                        log.LogMethodExit(transactionPaymentsDTO);
                        return transactionPaymentsDTO;
                    } 
                }
                else
                {
                    log.Error("Exception in processing Payment ");
                    throw new Exception("Exception in processing Payment ");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Exception in processing Payment ");
            }
        }

        public override TransactionPaymentsDTO PerformSettlement(TransactionPaymentsDTO transactionPaymentsDTO, bool IsForcedSettlement = false)
        {
            dynamic mashreqResponse;
            log.LogMethodEntry(transactionPaymentsDTO, IsForcedSettlement);
            try
            {
                if (transactionPaymentsDTO != null)
                {
                    double baseAmount = transactionPaymentsDTO.Amount * 100;
                    //double tipAmount = transactionPaymentsDTO.TipAmount;

                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Mashreq Payment Gateway");

                    statusDisplayUi.EnableCancelButton(false);

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                    log.LogVariableState("ccOrigTransactionsPGWDTO", ccOrigTransactionsPGWDTO);
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    // TBC
                    var requestDTO = new MashreqRequestDTO()
                    {
                        trnxType = (int)TransactionType.CAPTURE,
                        mrefValue = ccRequestPGWDTO.RequestID.ToString(),
                        invoiceNumber = ccOrigTransactionsPGWDTO.RecordNo.ToString(),
                    };
                    log.LogVariableState("requestDTO", requestDTO);
                    //mashreqResponse = CreateTransactionRequest(new MashreqRequestDTO()
                    //{
                    //    trnxType = (int)TransactionType.GETTERMINALINFO
                    //});

                    //log.LogVariableState("mashreqResponse", mashreqResponse);
                    //if (mashreqResponse == null
                    //    || !mashreqResponse.Status.ToString().Equals("RanToCompletion")
                    //    || mashreqResponse.Result == null
                    //    || (mashreqResponse.ErrorCode != null && !mashreqResponse.ErrorCode.Equals("E000")))
                    //{
                    //    errMsg = GetErrorMessage(mashreqResponse);
                    //    throw new Exception(errMsg);
                    //}
                    mashreqResponse = CreateTransactionRequest(requestDTO);

                    log.LogVariableState("mashreqResponse", mashreqResponse);
                    if (mashreqResponse == null || !mashreqResponse.ErrorCode.Equals("E000"))
                    {
                        if (mashreqResponse != null)
                        {
                            //if (mashreqResponse.ErrorCode == "E067")
                            //{
                            //    dynamic LastTxCheckResponse = CreateTransactionRequest(new MashreqRequestDTO()
                            //    {
                            //        trnxType = (int)TransactionType.LastTransactionCheck,
                            //        mrefValue = mashreqResponse.MREFValue.ToString()
                            //    });
                            //}
                            errMsg = GetErrorMessage(mashreqResponse);
                        }
                        else
                        {
                            errMsg = "Capture: Error in Getting response from the device";
                        }
                        log.Error(errMsg);
                        throw new Exception(errMsg);
                    }
                    if (mashreqResponse == null)
                    {
                        throw new Exception("Error occured while performing settlement");
                    }
                    else
                    {
                        Form form = statusDisplayUi as Form;
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                        log.LogVariableState("responseObjectCapture", mashreqResponse);
                        log.LogVariableState("responsePaymentObject", mashreqResponse);
                        log.LogVariableState("responsePaymentObject Result", mashreqResponse);
                        if (mashreqResponse.ResponseCode == "APPROVED")
                        {
                            double resamount = Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize);
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = GetMaskedResponseField(mashreqResponse.MaskCardNumber);
                            cCTransactionsPGWDTO.AuthCode = mashreqResponse.AuthCode;
                            cCTransactionsPGWDTO.CardType = mashreqResponse.CardSchemeName;
                            cCTransactionsPGWDTO.CaptureStatus = mashreqResponse.EntryMode;
                            cCTransactionsPGWDTO.RefNo = mashreqResponse.MREFValue;
                            cCTransactionsPGWDTO.RecordNo = mashreqResponse.InvoiceNo;
                            cCTransactionsPGWDTO.TextResponse = mashreqResponse.ResponseCode;
                            cCTransactionsPGWDTO.TranCode = TransactionType.CAPTURE.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = resamount.ToString();
                            double tip = 0;
                            if (mashreqResponse.Amount != null)
                            {
                                double amount = 0;
                                log.LogVariableState("mashreqResponse.Amount", mashreqResponse.Amount);
                                double.TryParse(mashreqResponse.Amount, out amount);
                                tip = amount * 0.01 - resamount;
                                log.LogVariableState("Tip", tip);
                            }
                            cCTransactionsPGWDTO.TipAmount = tip.ToString("0.####");
                            cCTransactionsPGWDTO.AcqRefData = "|MID:" + GetMaskedResponseField(mashreqResponse.MID) + "|TID:" + mashreqResponse.TID;


                            ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.CreditCardAuthorization = mashreqResponse.AuthCode;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.Amount = resamount;
                            transactionPaymentsDTO.TipAmount = tip;
                            transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            transactionPaymentsDTO.NameOnCreditCard = mashreqResponse.CardHolderName;
                        }
                    }
                }
                else
                {
                    statusDisplayUi.DisplayText("Invalid payment data.");
                    throw new Exception(utilities.MessageUtils.getMessage("Invalid payment data."));
                }
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
            log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);
            try
            {
                //Form activeForm = GetActiveForm();
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage("Checking the transaction status" + ((cCRequestPGWDTO != null) ? " of TrxId:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "Mashreq Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                //thr.Start();
                ////Form form = statusDisplayUi as Form;
                ////form.Show(activeForm);
                ////SetNativeEnabled(activeForm, false);
                //statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));

                CCTransactionsPGWDTO ccTransactionsPGWDTOResponse = null;


                if (cCTransactionsPGWDTO != null)
                {
                    log.Debug("cCTransactionsPGWDTO is not null");
                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOcapturedList = null;
                    if (!string.IsNullOrEmpty(cCTransactionsPGWDTO.RecordNo)  && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.REFUND.ToString()) && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.VOID.ToString()))
                    {
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
                        if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.SALE.ToString()))
                        {
                            transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, cCTransactionsPGWDTO.ResponseID.ToString()));
                            transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                            List<TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);
                            if (transactionPaymentsDTOs != null && transactionPaymentsDTOs.Any())
                            {
                                log.Debug("The capture/tip adjusted transaction exists for the authorization request with requestId:" + cCTransactionsPGWDTO.InvoiceNo + " and its upto date");
                                return;
                            }
                            thr.Start();
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                            dynamic LastTrxCheckResult = CreateTransactionRequest(new MashreqRequestDTO
                            {
                                trnxType = (int)TransactionType.LastTransactionCheck,
                                mrefValue = cCTransactionsPGWDTO.InvoiceNo.ToString()
                            });

                            log.LogVariableState("VOID LastTrxCheckResult", LastTrxCheckResult);


                            if (isSuccess(LastTrxCheckResult))
                            {
                                log.LogVariableState("LastTrxCheckResult response", LastTrxCheckResult);

                                //if (!string.IsNullOrEmpty((string)LastTrxCheckResult.payment.result) && (string)LastTrxCheckResult.payment.result == "SUCCESS")
                                {
                                    double resAmount = Convert.ToDouble(LastTrxCheckResult.Amount) * 0.01;
                                    ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();

                                    ccTransactionsPGWDTOResponse.AcctNo = GetMaskedResponseField(LastTrxCheckResult.MaskCardNumber);
                                    ccTransactionsPGWDTOResponse.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                    ccTransactionsPGWDTOResponse.AuthCode = LastTrxCheckResult.AuthCode;
                                    ccTransactionsPGWDTOResponse.CardType = LastTrxCheckResult.CardSchemeName;
                                    ccTransactionsPGWDTOResponse.CaptureStatus = LastTrxCheckResult.EntryMode;
                                    ccTransactionsPGWDTOResponse.RefNo = string.IsNullOrWhiteSpace(LastTrxCheckResult.MREFValue) ? cCRequestPGWDTO.RequestID.ToString() : LastTrxCheckResult.MREFValue; ;
                                    ccTransactionsPGWDTOResponse.RecordNo = LastTrxCheckResult.InvoiceNo;
                                    ccTransactionsPGWDTOResponse.AcqRefData = "|MID:" + GetMaskedResponseField(LastTrxCheckResult.MID) + "|TID:" + LastTrxCheckResult.TID;
                                    //ccTransactionsPGWDTOResponse.TokenID = LastTrxCheckResult.payment.cardTransaction.token;
                                    //ccTransactionsPGWDTOResponse.TextResponse = responsePaymentObject.payment.cardTransaction.state;
                                    ccTransactionsPGWDTOResponse.TextResponse = LastTrxCheckResult.ResponseCode;
                                    ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                                    ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                                    ccTransactionsPGWDTOResponse.Authorize = resAmount.ToString();
                                    //double tip = tipAmount * 0.01;
                                    //ccTransactionsPGWDTOResponse.TipAmount = tip.ToString();
                                }
                            }
                        }
                        //DisplayInDevice(displayCommand, utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                        //var mashreqRequestDTO = new MashreqRequestDTO()
                        //{
                        //    trnxType = (int)TransactionType.LastTransactionCheck,
                        //    mrefValue = cCTransactionsPGWDTO.InvoiceNo.ToString(),

                        //};
                        //dynamic mashreqResponse;
                        //mashreqResponse = CreateTransactionRequest(mashreqRequestDTO);
                        //if (mashreqResponse == null
                        //            || !mashreqResponse.Status.ToString().Equals("RanToCompletion")
                        //            || mashreqResponse.Result == null
                        //            || (mashreqResponse.ErrorCode != null && !mashreqResponse.ErrorCode.Equals("E000")))
                        //{
                        //    errMsg = GetErrorMessage(mashreqResponse);
                        //    throw new Exception(errMsg);
                        //}
                        //if (mashreqResponse == null)
                        //{
                        //    throw new Exception("Error occured while checking last Transaction");
                        //}


                    }
                    else
                    {
                        log.Debug("credit card transaction done from this POS is not approved.");
                        log.LogMethodExit(true);
                        return;
                    }
                }
                else if (cCRequestPGWDTO != null && cCRequestPGWDTO.TransactionType == TransactionType.SALE.ToString())
                {
                    thr.Start();
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                    log.Debug("cCRequestPGWDTO is not null");

                    dynamic LastTrxCheckResult = CreateTransactionRequest(new MashreqRequestDTO
                    {
                        trnxType = (int)TransactionType.LastTransactionCheck,
                        mrefValue = cCRequestPGWDTO.RequestID.ToString()
                    });

                    log.LogVariableState("Sale LastTrxCheckResult", LastTrxCheckResult);

                    if (isSuccess(LastTrxCheckResult))
                    {
                        double resAmount = Convert.ToDouble(LastTrxCheckResult.Amount) * 0.01;

                        ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();
                        ccTransactionsPGWDTOResponse.AcctNo = GetMaskedResponseField(LastTrxCheckResult.MaskCardNumber);
                        ccTransactionsPGWDTOResponse.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTOResponse.AuthCode = LastTrxCheckResult.AuthCode;
                        ccTransactionsPGWDTOResponse.CardType = LastTrxCheckResult.CardSchemeName;
                        ccTransactionsPGWDTOResponse.CaptureStatus = LastTrxCheckResult.EntryMode;
                        ccTransactionsPGWDTOResponse.RefNo = string.IsNullOrWhiteSpace(LastTrxCheckResult.MREFValue) ? cCRequestPGWDTO.RequestID.ToString() : LastTrxCheckResult.MREFValue; ;
                        ccTransactionsPGWDTOResponse.RecordNo = LastTrxCheckResult.InvoiceNo;
                        ccTransactionsPGWDTOResponse.AcqRefData = "|MID:" + GetMaskedResponseField(LastTrxCheckResult.MID) + "|TID:" + LastTrxCheckResult.TID;
                        //ccTransactionsPGWDTOResponse.TokenID = LastTrxCheckResult.payment.cardTransaction.token;
                        //ccTransactionsPGWDTOResponse.TextResponse = responsePaymentObject.payment.cardTransaction.state;
                        ccTransactionsPGWDTOResponse.TextResponse = LastTrxCheckResult.ResponseCode;
                        ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                        ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                        ccTransactionsPGWDTOResponse.Authorize = resAmount.ToString();
                        //double tip = tipAmount * 0.01;
                        //ccTransactionsPGWDTOResponse.TipAmount = tip.ToString();
                    }
                    else
                    {

                        string errorMessage = GetErrorMessage(LastTrxCheckResult);
                        //ccTransactionsPGWDTOResponse.TextResponse = errorMessage;
                        //ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                        log.Error("Last Transaction Check Failed");
                        log.Error(errorMessage);
                        return;
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
                        ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                        if (cCTransactionsPGWDTO == null)
                        {
                            log.Debug("Saving ccTransactionsPGWDTOResponse.");
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTOResponse);
                            ccTransactionsPGWBL.Save();
                        }
                        log.LogVariableState("ccTransactionsPGWDTOResponse", ccTransactionsPGWDTOResponse);
                        //if (!string.IsNullOrEmpty(ccTransactionsPGWDTOResponse.RefNo) && !ccTransactionsPGWDTOResponse.RecordNo.Equals("C"))
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
                    //MashreqCommandHandler mashreqCommandHandler = new MashreqCommandHandler(gatewayUrl, authorization);
                    //string result = mashreqCommandHandler.DisplayWelcomeScreen(deviceId, posId);
                    //if (result == "Exception")
                    //{
                    //    throw new Exception("Exception in last transaction check");
                    //}
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

        //public async Task<object> CreateTransactionRequest(MashreqRequestDTO requestDTO)
        //{
        //    try
        //    {
        //        var task = await MakeTransactionRequest(requestDTO);
        //        return task;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public object CreateTransactionRequest(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            try
            {
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<object> task = MakeTransactionRequest(requestDTO);
                    task.Wait();
                    return task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            //var task = Task.Run(async () => await MakeTransactionRequest(requestDTO));
            //return task.Result;
        }

        private async Task<object> MakeTransactionRequest(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            dynamic res = null;
            try
            {
                Task<object> longRunningTask;
                switch (requestDTO.trnxType)
                {
                    case (int)TransactionType.SALE:
                        longRunningTask = mashreqCommandHandler.MakePurchase(requestDTO);
                        res = await longRunningTask;
                        break;
                    case (int)TransactionType.REFUND:
                        longRunningTask = mashreqCommandHandler.MakeRefund(requestDTO);
                        res = await longRunningTask;
                        break;
                    case (int)TransactionType.INDEPENDENT_REFUND:
                        longRunningTask = mashreqCommandHandler.MakeIndependentRefund(requestDTO);
                        res = await longRunningTask;
                        break;
                    case (int)TransactionType.AUTHORIZATION:
                        longRunningTask = mashreqCommandHandler.MakePreReceipt(requestDTO);
                        res = await longRunningTask;
                        break;
                    case (int)TransactionType.CAPTURE:
                        longRunningTask = mashreqCommandHandler.MakePreReceiptCompletionWithTip(requestDTO);
                        res = await longRunningTask;
                        break;
                    case (int)TransactionType.VOID:
                        longRunningTask = mashreqCommandHandler.MakeVoid(requestDTO);
                        res = await longRunningTask;
                        break;
                    case (int)TransactionType.LastTransactionCheck:
                        longRunningTask = mashreqCommandHandler.GetLastTransactionStatus(requestDTO);
                        res = await longRunningTask;
                        break;
                    case (int)TransactionType.DUPLICATE:
                        longRunningTask = mashreqCommandHandler.MakeDuplicate(requestDTO);
                        res = await longRunningTask;
                        break;
                    case (int)TransactionType.GETTERMINALINFO:
                        longRunningTask = mashreqCommandHandler.GetTerminalInfo();
                        res = await longRunningTask;
                        break;
                    default:
                        log.Error("Transaction type was not selected");
                        throw new Exception("Transaction type was not selected");
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(res);
            return res;
        }


         private string GetMaskedResponseField(string responseField)
         {
            log.LogMethodEntry();
            responseField = responseField.Trim();//triming the Merchant id
            string maskedResponseField = string.Empty;
            if (!string.IsNullOrWhiteSpace(responseField))
            {
                maskedResponseField = responseField.Length > 4 ? new string('X', responseField.Length - 4) + responseField.Substring(responseField.Length - 4) : responseField;
            }
            log.LogMethodExit(maskedResponseField);
            return maskedResponseField;
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
                    string[] emvData = ccTransactionsPGWDTO.AcqRefData.Split('|');
                    string[] data = null;
                    for (int i = 0; i < emvData.Length; i++)
                    {
                        data = emvData[i].Split(':');

                        if (data[0].Equals("MID") && !string.IsNullOrWhiteSpace(data[1]))
                        {
                            string datavalue = data[1].Trim();
                            string mid = GetMaskedResponseField(datavalue);
                            receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("MID") + ": ".PadLeft(25) + mid, Alignment.Left);
                        }
                        else if (data[0].Equals("TID") && !string.IsNullOrEmpty(data[1]))
                        {
                            receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("TID") + ": ".PadLeft(25) + data[1], Alignment.Left);
                        }
                    }
                    //    string maskedMerchantId = (new String('X', 8) + ((ccTransactionsPGWDTO.AcqRefData.Length > 4) ? ccTransactionsPGWDTO.AcqRefData.Substring(ccTransactionsPGWDTO.AcqRefData.Length - 4)
                    //                                                                         : ccTransactionsPGWDTO.AcqRefData));
                    //receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Merchant ID") + "     : ".PadLeft(12) + maskedMerchantId, Alignment.Left);
                }
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Date") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TransactionDatetime.ToString("MMM dd yyyy HH:mm"), Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Type") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TranCode, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Invoice Number") + "  : ".PadLeft(6) + ccTransactionsPGWDTO.InvoiceNo, Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.AuthCode))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Authorization") + "   : ".PadLeft(10) + ccTransactionsPGWDTO.AuthCode, Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.CardType))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Card Type") + "       : ".PadLeft(15) + ccTransactionsPGWDTO.CardType, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Cardholder Name") + ": ".PadLeft(3) + trxPaymentsDTO.NameOnCreditCard, Alignment.Left);
                //string maskedPAN = ((string.IsNullOrEmpty(ccTransactionsPGWDTO.AcctNo) ? ccTransactionsPGWDTO.AcctNo
                //                                                             : (new String('X', 12) + ((ccTransactionsPGWDTO.AcctNo.Length > 4)
                //                                                                                     ? ccTransactionsPGWDTO.AcctNo.Substring(ccTransactionsPGWDTO.AcctNo.Length - 4)
                //                                                                                     : ccTransactionsPGWDTO.AcctNo))));
                string maskedPAN = ccTransactionsPGWDTO.AcctNo;                                                                      // )));
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
    }
}
