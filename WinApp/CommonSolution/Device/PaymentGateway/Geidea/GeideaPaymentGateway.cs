/******************************************************************************************************
 * Project Name - Device
 * Description  - Geidea Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.140.3     11-Aug-2022    Prasad & Dakshakh Raj   Geidea Payment gateway integration
 ********************************************************************************************************/
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
using Semnox.Parafait.Device.PaymentGateway;
using System.Xml;
using System.Drawing;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class GeideaPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string deviceId;
        bool isAuthEnabled;
        bool isManual;
        bool enableAutoAuthorization;
        string minPreAuth;
        string posId;
        int isPrintReceiptEnabled;
        //string errMsg = string.Empty;
        // params by Prasad
        int port, baudRate = 38400, parityBit, dataBit = 8, stopBit = 1;
        string[] SUCCESS_RESPONSE_CODES = { "000", "001", "003", "007", "087", "089", "400" };
        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();
        //private Dictionary<string, string> ErrorMessagesMap = new Dictionary<string, string>();
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


        public GeideaPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
    : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {

            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel   
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
            deviceId = utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_ID");
            string value = utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO");
            port = (string.IsNullOrEmpty(value)) ? -1 : Convert.ToInt32(value);
            isAuthEnabled = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
            enableAutoAuthorization = utilities.getParafaitDefaults("ENABLE_AUTO_CREDITCARD_AUTHORIZATION").Equals("Y");
            PrintReceipt = false;
            if (utilities.getParafaitDefaults("CC_PAYMENT_RECEIPT_PRINT").Equals("N"))//If CC_PAYMENT_RECEIPT_PRINT which comes from POS is set as false then terminal should print, If you need terminal to print the receipt then set CC_PAYMENT_RECEIPT_PRINT value as N
            {
                PrintReceipt = true;
            }
            isPrintReceiptEnabled = PrintReceipt == true ? 1 : 0;
            log.LogVariableState("deviceId", deviceId);
            if (string.IsNullOrEmpty(deviceId))
            {
                log.Error("Please enter CREDIT_CARD_TERMINAL_ID value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TERMINAL_ID value in configuration."));
            }
            if (port == -1)
            {
                log.Error("Please enter CREDIT_CARD_TERMINAL_PORT_NO value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TERMINAL_PORT_NO value in configuration."));
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
            statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Geidea  Payment Gateway");
            statusDisplayUi.EnableCancelButton(false);
            isManual = false;
            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
            TransactionType trxType = TransactionType.SALE;
            string paymentId = string.Empty;
            CCTransactionsPGWDTO cCOrgTransactionsPGWDTO = null;
            double amount = (transactionPaymentsDTO.Amount) * 100;
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
                                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Pre-Authorization is not supported"));
                                        log.Error("Pre-Authorization is not supported");
                                        throw new Exception("Pre-Authorization is not supported");
                                    }
                                }
                                else
                                {
                                    log.Error("Operation cancelled.");
                                    throw new Exception(utilities.MessageUtils.getMessage("Operation cancelled."));
                                }
                            }
                        }
                    }
                }
                thr.Start();
                if (transactionPaymentsDTO != null)
                {
                    string isTransactionSucceeded = string.Empty;

                    GeideaResponseDTO responseObject = null;
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, trxType.ToString());
                    log.LogVariableState("ccRequestPGWDTO", ccRequestPGWDTO);
                    if (transactionPaymentsDTO.Amount >= 0)
                    {
                        if (trxType == TransactionType.AUTHORIZATION)
                        {
                            log.Debug("Entered into Authorization selection with Trx type " + trxType.ToString());
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                            log.LogVariableState("1", "Entered into requestDTO creation");
                            var requestDTO = new GeideaRequestDTO(
                                    trnxAmount: Convert.ToInt64(amount),
                                    naqdAmount: null,
                                    refundAmount: null,
                                    isPrintReceiptEnabled: isPrintReceiptEnabled,
                                    trxnRrn: string.Empty,
                                    originalRefundDate: string.Empty,
                                    ecrRefNumber: ccRequestPGWDTO.RequestID.ToString(),
                                    cardNumber: null,
                                    trxnApprovalNumber: null,
                                    isUnattended: false,
                                    trnxType: 12, // Authorization
                                    terminalID: deviceId,
                                    cashierNo: null,
                                    repeatCommandRequestBuffer: null,
                                    port: port,
                                    baudRate: baudRate,
                                    parityBit: parityBit,
                                    dataBit: dataBit,
                                    stopBit: stopBit,
                                    paymentId: null
                                    );
                            log.LogVariableState("Authorization RequestDTO Value", requestDTO);

                            responseObject = MakeTransactionRequest(requestDTO, ccRequestPGWDTO);
                            log.LogVariableState("Authorization responseObject", responseObject);
                            if (responseObject == null)
                            {
                                log.Error($"responseObject was null");
                                throw new Exception($"Payment failed");
                            }
                            if (string.IsNullOrWhiteSpace(responseObject.ResponseCode))
                            {
                                log.Error($"responseObject.ResponseCode was null");
                                throw new Exception($"Payment failed");
                            }
                            log.Debug("Authorization response received");
                            double resamount = Convert.ToDouble(responseObject.PurAmount);
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.PanNo);
                            cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                            cCTransactionsPGWDTO.CardType = responseObject.SchemeId;
                            cCTransactionsPGWDTO.RefNo = responseObject.EcrRefNo;
                            cCTransactionsPGWDTO.RecordNo = responseObject.TrxRrn;
                            cCTransactionsPGWDTO.TextResponse = responseObject.ResponseText;
                            cCTransactionsPGWDTO.TranCode = trxType.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = resamount.ToString();
                            cCTransactionsPGWDTO.AcqRefData = GetMaskedMid(Convert.ToString(responseObject.MerchantId));
                            SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            log.Debug("Authorization response saved to cCTransactionsPGW");
                            log.Debug("Authorization response - Validating against success codes");
                            isTransactionSucceeded = Array.Find(SUCCESS_RESPONSE_CODES, element => element == Convert.ToString(responseObject.ResponseCode));
                            log.Debug($"Check if isTransactionSucceeded is null(Tx failed) or not null(Tx succeeded)={isTransactionSucceeded}");
                            if (string.IsNullOrWhiteSpace(isTransactionSucceeded))
                            {
                                // Tx failed
                                log.Error($"Payment failed: {Convert.ToString(responseObject.ResponseText)}");
                                throw new Exception($"Payment failed: {Convert.ToString(responseObject.ResponseText)}");
                            }
                            log.Debug("Authorization response - Validation successfull");

                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.Amount = resamount;
                            transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;

                        }
                        else if (trxType == TransactionType.SALE)
                        {
                            log.Debug("Entered into Sale with Tx type " + trxType.ToString());
                            if (cCOrgTransactionsPGWDTO != null)
                            {
                                // Geidea ADVICE => trnxType=13
                                paymentId = cCOrgTransactionsPGWDTO.RecordNo.ToString();
                                frmFinalizeTransaction frmFinalizeTransaction = new frmFinalizeTransaction(utilities, overallTransactionAmount, overallTipAmountEntered, Convert.ToDecimal(transactionPaymentsDTO.Amount), Convert.ToDecimal(transactionPaymentsDTO.TipAmount), transactionPaymentsDTO.CreditCardNumber, showMessageDelegate);
                                if (frmFinalizeTransaction.ShowDialog() != DialogResult.Cancel)
                                {
                                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                                    log.Debug("Starts Advice");
                                    var requestDTO = new GeideaRequestDTO(
                                        trnxAmount: Convert.ToInt64(amount),
                                        naqdAmount: null,
                                        refundAmount: null,
                                        isPrintReceiptEnabled: isPrintReceiptEnabled,
                                        trxnRrn: string.Empty,
                                        originalRefundDate: string.Empty,
                                        ecrRefNumber: ccRequestPGWDTO.RequestID.ToString(),
                                        cardNumber: null,
                                        trxnApprovalNumber: cCOrgTransactionsPGWDTO.AuthCode,
                                        isUnattended: false,
                                        trnxType: 13, // Advice
                                        terminalID: deviceId,
                                        cashierNo: null,
                                        repeatCommandRequestBuffer: null,
                                        port: port,
                                        baudRate: baudRate,
                                        parityBit: parityBit,
                                        dataBit: dataBit,
                                        stopBit: stopBit,
                                        paymentId: paymentId
                                    );
                                    log.LogVariableState("Capture requestDTO", requestDTO);

                                    responseObject = MakeTransactionRequest(requestDTO, ccRequestPGWDTO);

                                    log.LogVariableState("Capture responseObject", responseObject);
                                    if (responseObject == null)
                                    {
                                        log.Error($"responseObject was null");
                                        throw new Exception($"Payment failed");
                                    }
                                    if (string.IsNullOrWhiteSpace(responseObject.ResponseCode))
                                    {
                                        log.Error($"responseObject.ResponseCode was null");
                                        throw new Exception($"Payment failed");
                                    }
                                    log.Debug("Capture/Advice response received");
                                    double resAmount = Convert.ToDouble(responseObject.PurAmount);
                                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                    cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                    cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.PanNo);
                                    cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                                    cCTransactionsPGWDTO.CardType = responseObject.SchemeId;
                                    cCTransactionsPGWDTO.RefNo = responseObject.EcrRefNo;
                                    cCTransactionsPGWDTO.RecordNo = responseObject.TrxRrn;
                                    cCTransactionsPGWDTO.TextResponse = responseObject.ResponseText;
                                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                    cCTransactionsPGWDTO.Authorize = resAmount.ToString();
                                    cCTransactionsPGWDTO.AcqRefData = GetMaskedMid(Convert.ToString(responseObject.MerchantId));
                                    SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                    ccTransactionsPGWBL.Save();
                                    log.Debug("Capture/Advice response saved to cCTransactionsPGW");
                                    log.Debug("Capture/Advice - Validating against success codes");
                                    isTransactionSucceeded = Array.Find(SUCCESS_RESPONSE_CODES, element => element == Convert.ToString(responseObject.ResponseCode));
                                    log.Debug($"Check if isTransactionSucceeded is null(Tx failed) or not null(Tx succeeded)={isTransactionSucceeded}");
                                    if (string.IsNullOrWhiteSpace(isTransactionSucceeded))
                                    {
                                        // Tx failed
                                        log.Error($"Payment failed: {Convert.ToString(responseObject.ResponseText)}");
                                        throw new Exception($"Payment failed: {Convert.ToString(responseObject.ResponseText)}");
                                    }
                                    log.Debug("Capture/Advice response - Validation successfull");



                                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                    transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                    transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                    transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                                }
                                else
                                {
                                    log.LogMethodExit(transactionPaymentsDTO);
                                    log.Error("CANCELLED");
                                    throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                                }
                            }
                            else
                            {
                                log.Info($"Entered into Sale with Tx Type = Sale = {trxType.ToString()}");
                                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                                // Geidea PURCHASE => trnxType=0
                                log.Debug("Starts Sale");
                                var requestDTO = new GeideaRequestDTO(
                                    trnxAmount: Convert.ToInt64(amount),
                                    naqdAmount: null,
                                    refundAmount: null,
                                    isPrintReceiptEnabled: isPrintReceiptEnabled,
                                    trxnRrn: null,
                                    originalRefundDate: string.Empty,
                                    ecrRefNumber: ccRequestPGWDTO.RequestID.ToString(),
                                    cardNumber: null,
                                    trxnApprovalNumber: null,
                                    isUnattended: false,
                                    trnxType: 0,
                                    terminalID: deviceId,
                                    cashierNo: null,
                                    repeatCommandRequestBuffer: null,
                                    port: port,
                                    baudRate: baudRate,
                                    parityBit: parityBit,
                                    dataBit: dataBit,
                                    stopBit: stopBit,
                                    paymentId: null);
                                log.LogVariableState("Sale requestDTO", requestDTO);
                                responseObject = MakeTransactionRequest(requestDTO, ccRequestPGWDTO);
                                log.LogVariableState("Sale responseObject", responseObject);
                                if (responseObject != null)
                                {
                                    if (string.IsNullOrWhiteSpace(responseObject.ResponseCode))
                                    {
                                        log.Error($"responseObject.ResponseCode was null");
                                        throw new Exception($"Payment failed");
                                    }
                                    log.Debug("Sale response received");
                                    double resamount = Convert.ToDouble(responseObject.PurAmount);
                                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                    cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                    cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.PanNo);
                                    cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                                    cCTransactionsPGWDTO.CardType = responseObject.SchemeId;
                                    cCTransactionsPGWDTO.RefNo = responseObject.EcrRefNo;
                                    cCTransactionsPGWDTO.RecordNo = responseObject.TrxRrn;
                                    cCTransactionsPGWDTO.TextResponse = responseObject.ResponseText;
                                    cCTransactionsPGWDTO.TranCode = trxType.ToString();
                                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                    cCTransactionsPGWDTO.Authorize = resamount.ToString();
                                    cCTransactionsPGWDTO.AcqRefData = GetMaskedMid(Convert.ToString(responseObject.MerchantId));
                                    SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                    ccTransactionsPGWBL.Save();
                                    log.Debug("Sale response saved to cCTransactionsPGW");
                                    log.Debug("Sale response - Validating against success codes");
                                    isTransactionSucceeded = Array.Find(SUCCESS_RESPONSE_CODES, element => element == Convert.ToString(responseObject.ResponseCode));
                                    log.Debug($"Check if isTransactionSucceeded is null(Tx failed) or not null(Tx succeeded)={isTransactionSucceeded}");
                                    if (string.IsNullOrWhiteSpace(isTransactionSucceeded))
                                    {
                                        // Tx failed
                                        log.Error($"Payment failed: {Convert.ToString(responseObject.ResponseText)}");
                                        throw new Exception($"Payment failed: {Convert.ToString(responseObject.ResponseText)}");
                                    }
                                    log.Debug("Sale response - Validation successfull");
                                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                    transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                    transactionPaymentsDTO.Amount = resamount;
                                    transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                                    log.LogVariableState("Sale cCTransactionsPGWDTO", cCTransactionsPGWDTO);
                                    log.LogVariableState("Sale transactionPaymentsDTO", transactionPaymentsDTO);
                                }
                                else
                                {
                                    log.Fatal("Response was null");
                                    throw new Exception("Error in processing Payment");
                                }
                            }
                        }
                    }
                    else
                    {
                        log.Debug("Entered into Independent Refund");
                        amount = amount * -1;
                        var requestDTO = new GeideaRequestDTO(
                                trnxAmount: null,
                                naqdAmount: null,
                                refundAmount: Convert.ToInt64(amount),
                                isPrintReceiptEnabled: isPrintReceiptEnabled,
                                trxnRrn: string.Empty,
                                originalRefundDate: string.Empty,
                                ecrRefNumber: ccRequestPGWDTO.RequestID.ToString(),
                                cardNumber: null,
                                trxnApprovalNumber: null,
                                isUnattended: false,
                                trnxType: 2, // depends on void or refund; for void=3, for refund=2
                                terminalID: deviceId,
                                cashierNo: null,
                                repeatCommandRequestBuffer: null,
                                port: port,
                                baudRate: baudRate,
                                parityBit: parityBit,
                                dataBit: dataBit,
                                stopBit: stopBit,
                                paymentId: null
                                );
                        log.LogVariableState("Item Refund requestDTO", requestDTO);
                        responseObject = MakeTransactionRequest(requestDTO, ccRequestPGWDTO);
                        log.LogVariableState("Item Refund responseObject", responseObject);
                        if (responseObject != null)
                        {
                            if (string.IsNullOrWhiteSpace(responseObject.ResponseCode))
                            {
                                log.Error($"responseObject.ResponseCode was null");
                                throw new Exception($"Payment failed");
                            }
                            log.Debug("Independent Refund response received");
                            double refundAmount = Convert.ToDouble(responseObject.PurAmount);
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.PanNo);
                            cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                            cCTransactionsPGWDTO.CardType = responseObject.SchemeId;
                            cCTransactionsPGWDTO.RefNo = responseObject.EcrRefNo;
                            cCTransactionsPGWDTO.RecordNo = responseObject.TrxRrn;
                            cCTransactionsPGWDTO.TextResponse = responseObject.ResponseText;
                            cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                            cCTransactionsPGWDTO.AcqRefData = GetMaskedMid(Convert.ToString(responseObject.MerchantId));
                            SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            log.Debug("Independent Refund response saved to cCTransactionsPGW");
                            log.LogVariableState("Independent Refund cCTransactionsPGWDTO", cCTransactionsPGWDTO);
                            log.Debug("Independent Refund response - Validating against success codes");
                            isTransactionSucceeded = Array.Find(SUCCESS_RESPONSE_CODES, element => element == Convert.ToString(responseObject.ResponseCode));
                            log.Debug($"Check if isTransactionSucceeded is null(Tx failed) or not null(Tx succeeded)={isTransactionSucceeded}");
                            if (string.IsNullOrWhiteSpace(isTransactionSucceeded))
                            {
                                // Tx failed
                                log.Error($"Item Refund failed: {Convert.ToString(responseObject.ResponseText)}");
                                throw new Exception($"Item Refund failed: {Convert.ToString(responseObject.ResponseText)}");
                            }

                            log.Debug("Independent Refund response - Validation successfull");


                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize) * -1;
                            transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            log.LogVariableState("Item Refund transactionPaymentsDTO", transactionPaymentsDTO);

                        }
                        else
                        {
                            log.Error("Item Refund Response waas empty");
                            throw new Exception("Item Refund failed");
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
                throw new Exception(ex.Message, ex);

            }
            finally
            {
                log.Debug("Reached Finally");
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                    statusDisplayUi = null;
                }
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;

        }
        private GeideaResponseDTO MakeLastTransactionCheckRequest()
        {
            log.LogMethodEntry();
            GeideaCommandHandler geideaCommandHandler = new GeideaCommandHandler(statusDisplayUi, utilities.ExecutionContext);
            GeideaResponseDTO geideaResponse = null;
            try
            {
                geideaResponse = geideaCommandHandler.PerformLastTrxCheck(isPrintReceiptEnabled, deviceId, port, baudRate, parityBit, dataBit, stopBit);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(geideaResponse);
            return geideaResponse;


        }
        private GeideaResponseDTO MakeTransactionRequest(GeideaRequestDTO geideaRequestDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            log.LogMethodEntry(geideaRequestDTO, cCRequestPGWDTO);
            log.Debug($"Make Transaction Request for CCRequestId={cCRequestPGWDTO.RequestID}");
            GeideaCommandHandler geideaCommandHandler = new GeideaCommandHandler(statusDisplayUi, utilities.ExecutionContext);
            GeideaResponseDTO geideaResponse = null;

            try
            {
                //Validate request string
                string bufferString = geideaCommandHandler.GetRequestString(geideaRequestDTO);
                log.Debug("bufferString: " + bufferString);
                // Do the DeviceStatus check
                GeideaResponseDTO deviceStatus = geideaCommandHandler.CheckDeviceStatus(geideaRequestDTO);
                // Perform the transaction
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                geideaResponse = geideaCommandHandler.MakeTransactionRequest(geideaRequestDTO, bufferString);
                log.LogVariableState("Geidea Response :", geideaResponse);
            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(geideaResponse);
            return geideaResponse;
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            GeideaResponseDTO responseObject;
            try
            {
                string isTransactionSucceeded = string.Empty;
                PrintReceipt = true;
                if (transactionPaymentsDTO != null)
                {
                    CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    log.LogVariableState("Refund ccOrigTransactionsPGWBL", ccOrigTransactionsPGWBL);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;
                    log.LogVariableState("Refund ccOrigTransactionsPGWDTO", ccOrigTransactionsPGWDTO);
                    if (transactionPaymentsDTO.Amount < 0)
                    {
                        log.Error("Variable Refund Not Supported");
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Variable Refund Not Supported"));
                    }


                    //To Void a payment
                    //Get the Request time
                    CCRequestPGWDTO saleccRequestDTO = null;
                    try
                    {
                        CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, Convert.ToInt32(ccOrigTransactionsPGWDTO.InvoiceNo));
                        saleccRequestDTO = cCRequestPGWBL.CCRequestPGWDTO;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    double amount = transactionPaymentsDTO.Amount;
                    if (!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize))
                    {
                        decimal authorizeAmount = Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize);
                        decimal voidAmount = Convert.ToDecimal(amount);
                        if (authorizeAmount == voidAmount &&
                            saleccRequestDTO != null && saleccRequestDTO.RequestDatetime > DateTime.Now.AddMinutes(-1))
                        {
                            transactionPaymentsDTO = VoidPayment(transactionPaymentsDTO);
                            log.LogMethodExit(transactionPaymentsDTO);
                            return transactionPaymentsDTO;
                        }
                    }


                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, (-transactionPaymentsDTO.Amount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Geidea Payment Gateway");
                    statusDisplayUi.EnableCancelButton(false);

                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();

                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.REFUND.ToString());
                    log.LogVariableState("Refund cCRequestPGWDTO", cCRequestPGWDTO);

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


                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    {
                        // request time has crossed 1 min
                        if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) != Convert.ToDecimal(amount)))//This is the valid condition if we check refundable first. Because once it enters to this section it will do full reversal
                        {
                            log.Debug("Partial Refund Started");
                            var requestDTO = new GeideaRequestDTO(
                                trnxAmount: Convert.ToInt64(amount * 100),
                                naqdAmount: null,
                                refundAmount: Convert.ToInt64(amount * 100),
                                isPrintReceiptEnabled: isPrintReceiptEnabled,
                                trxnRrn: ccOrigTransactionsPGWDTO.RecordNo.ToString(),
                                originalRefundDate: originalPaymentDate.ToString("ddMMyyyy"),
                                ecrRefNumber: cCRequestPGWDTO.RequestID.ToString(),
                                cardNumber: null,
                                trxnApprovalNumber: null,
                                isUnattended: false,
                                trnxType: 2, // depends on void or refund; for void=3, for refund=2
                                terminalID: deviceId,
                                cashierNo: null,
                                repeatCommandRequestBuffer: null,
                                port: port,
                                baudRate: baudRate,
                                parityBit: parityBit,
                                dataBit: dataBit,
                                stopBit: stopBit,
                                paymentId: ccOrigTransactionsPGWDTO.RecordNo.ToString()
                                );
                            log.LogVariableState("Partial Refund requestDTO", requestDTO);

                            responseObject = MakeTransactionRequest(requestDTO, cCRequestPGWDTO);
                            log.LogVariableState("Partial Refund responseObject", responseObject);
                            if (responseObject != null)
                            {
                                if (string.IsNullOrWhiteSpace(responseObject.ResponseCode))
                                {
                                    log.Error($"responseObject.ResponseCode was null");
                                    throw new Exception($"Refund failed");
                                }
                                log.Debug("Partial Refund response received");
                                refundAmount = Convert.ToDouble(responseObject.PurAmount);
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.PanNo);
                                cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                                cCTransactionsPGWDTO.CardType = responseObject.SchemeId;
                                cCTransactionsPGWDTO.RefNo = responseObject.EcrRefNo;
                                cCTransactionsPGWDTO.RecordNo = responseObject.TrxRrn;
                                cCTransactionsPGWDTO.TextResponse = responseObject.ResponseText;
                                cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                cCTransactionsPGWDTO.AcqRefData = GetMaskedMid(Convert.ToString(responseObject.MerchantId));
                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                log.Debug("Partial Refund response saved to cCTransactionsPGW");
                                log.LogVariableState("Partial Refund cCTransactionsPGWDTO", cCTransactionsPGWDTO);
                                log.Debug("Partial Refund response - Validating against success codes");
                                isTransactionSucceeded = Array.Find(SUCCESS_RESPONSE_CODES, element => element == Convert.ToString(responseObject.ResponseCode));
                                log.Debug($"Check if isTransactionSucceeded is null(Tx failed) or not null(Tx succeeded)={isTransactionSucceeded}");
                                if (string.IsNullOrWhiteSpace(isTransactionSucceeded))
                                {
                                    // Tx failed
                                    log.Error($"Partial Refund failed: {Convert.ToString(responseObject.ResponseText)}");
                                    throw new Exception($"Refund failed: {Convert.ToString(responseObject.ResponseText)}");
                                }
                                log.Debug("Partial Refund response - Validation successfull");

                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                                log.LogVariableState("Partial Refund transactionPaymentsDTO", transactionPaymentsDTO);
                            }
                            else
                            {
                                log.Error($"responseObject was null");
                                throw new Exception($"Refund failed");
                            }
                        }
                        else if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) == Convert.ToDecimal(amount)))
                        {
                            log.Debug("Full Refund Started");
                            // next day full refund = void
                            var requestDTO = new GeideaRequestDTO(
                                trnxAmount: Convert.ToInt64(amount * 100),
                                naqdAmount: null,
                                refundAmount: Convert.ToInt64(amount * 100),
                                isPrintReceiptEnabled: isPrintReceiptEnabled,
                                trxnRrn: ccOrigTransactionsPGWDTO.RecordNo.ToString(),
                                originalRefundDate: originalPaymentDate.ToString("ddMMyyyy"),
                                ecrRefNumber: cCRequestPGWDTO.RequestID.ToString(),
                                cardNumber: null,
                                trxnApprovalNumber: null,
                                isUnattended: false,
                                trnxType: 2, // depends on void or refund; for void=3, for refund=2
                                terminalID: deviceId,
                                cashierNo: null,
                                repeatCommandRequestBuffer: null,
                                port: port,
                                baudRate: baudRate,
                                parityBit: parityBit,
                                dataBit: dataBit,
                                stopBit: stopBit,
                                paymentId: ccOrigTransactionsPGWDTO.RecordNo.ToString() // TBC with Prasanna if this is the paymentId
                                );
                            log.LogVariableState("Full Refund requestDTO", requestDTO);
                            responseObject = MakeTransactionRequest(requestDTO, cCRequestPGWDTO);
                            log.LogVariableState("Full Refund responseObject", responseObject);
                            if (responseObject != null)
                            {
                                if (string.IsNullOrWhiteSpace(responseObject.ResponseCode))
                                {
                                    log.Error($"responseObject.ResponseCode was null");
                                    throw new Exception($"Refund failed");
                                }
                                log.Debug("Full Refund response received");

                                refundAmount = Convert.ToDouble(responseObject.PurAmount);
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.PanNo);
                                cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                                cCTransactionsPGWDTO.CardType = responseObject.SchemeId;
                                cCTransactionsPGWDTO.RefNo = responseObject.EcrRefNo;
                                cCTransactionsPGWDTO.RecordNo = responseObject.TrxRrn;
                                cCTransactionsPGWDTO.TextResponse = responseObject.ResponseText;
                                cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                cCTransactionsPGWDTO.AcqRefData = GetMaskedMid(Convert.ToString(responseObject.MerchantId));
                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                log.Debug("Full Refund response saved to cCTransactionsPGW");
                                log.LogVariableState("Full Refund cCTransactionsPGWDTO", cCTransactionsPGWDTO);
                                log.Debug("Full Refund response - Validating against success codes");
                                isTransactionSucceeded = Array.Find(SUCCESS_RESPONSE_CODES, element => element == Convert.ToString(responseObject.ResponseCode));
                                log.Debug($"Check if isTransactionSucceeded is null(Tx failed) or not null(Tx succeeded)={isTransactionSucceeded}");
                                if (string.IsNullOrWhiteSpace(isTransactionSucceeded))
                                {
                                    // Tx failed
                                    log.Error($"Full Refund failed: {Convert.ToString(responseObject.ResponseText)}");
                                    throw new Exception($"Refund failed: {Convert.ToString(responseObject.ResponseText)}");
                                }
                                log.Debug("Full Refund response - Validation successfull");



                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                                log.LogVariableState("Full Refund transactionPaymentsDTO", transactionPaymentsDTO);
                            }
                            else
                            {
                                log.Error($"responseObject was null");
                                throw new Exception($"Refund failed");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.DisplayText("Error occured while Refunding the Amount");
                }
                log.Error("Error occured while Refunding the Amount", ex);
                log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                log.LogMethodExit(null, "throwing Exception");
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 5166, ex.Message));
                //Refund failed. Please perform manual refund. Reason: &1
            }
            finally
            {
                log.Debug("Reached Finally");
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                    statusDisplayUi = null;
                }
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

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

        // void is possible only within 60 seconds from the trnx
        public TransactionPaymentsDTO VoidPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            // Geidea Reversal TrnxType=3
            // void is possible only within 60 seconds of original trnx
            try
            {
                string isTransactionSucceeded = string.Empty;
                if (transactionPaymentsDTO != null)
                {
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, (-transactionPaymentsDTO.Amount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Geidea Payment Gateway");
                    statusDisplayUi.EnableCancelButton(false);

                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();

                    GeideaResponseDTO responseObject;
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.VOID.ToString());
                    log.LogVariableState("VoidPayment() cCRequestPGWDTO", cCRequestPGWDTO);

                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    log.Debug("Starts Void");
                    var requestDTO = new GeideaRequestDTO(
                                trnxAmount: null,
                                naqdAmount: null,
                                refundAmount: null,
                                isPrintReceiptEnabled: isPrintReceiptEnabled,
                                trxnRrn: null,
                                originalRefundDate: null,
                                ecrRefNumber: cCRequestPGWDTO.RequestID.ToString(),
                                cardNumber: null,
                                trxnApprovalNumber: null,
                                isUnattended: false,
                                trnxType: 3, // void
                                terminalID: deviceId,
                                cashierNo: null,
                                repeatCommandRequestBuffer: null,
                                port: port,
                                baudRate: baudRate,
                                parityBit: parityBit,
                                dataBit: dataBit,
                                stopBit: stopBit,
                               paymentId: transactionPaymentsDTO.PaymentId.ToString()
                                );
                    log.LogVariableState("VoidPayment() requestDTO", requestDTO);
                    responseObject = MakeTransactionRequest(requestDTO, cCRequestPGWDTO);
                    log.LogVariableState("VoidPayment() responseObject", responseObject);

                    if (responseObject != null)
                    {
                        if (string.IsNullOrWhiteSpace(responseObject.ResponseCode))
                        {
                            log.Error($"responseObject.ResponseCode was null");
                            throw new Exception($"Reversal failed");
                        }
                        log.Debug("Received Void Response");
                        double refundAmount = Convert.ToDouble(responseObject.PurAmount);
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                        cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.PanNo);
                        cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                        cCTransactionsPGWDTO.CardType = responseObject.SchemeId;
                        cCTransactionsPGWDTO.RefNo = responseObject.EcrRefNo;
                        cCTransactionsPGWDTO.RecordNo = responseObject.TrxRrn;
                        cCTransactionsPGWDTO.TextResponse = responseObject.ResponseText;
                        cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.VOID.ToString();
                        cCTransactionsPGWDTO.AcqRefData = GetMaskedMid(Convert.ToString(responseObject.MerchantId));
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        log.Debug("Void Response saved to CCTransactionPgw");
                        log.LogVariableState("VoidPayment() cCTransactionsPGWDTO", cCTransactionsPGWDTO);
                        log.Debug("Validating Void Response against the success response codes");
                        isTransactionSucceeded = Array.Find(SUCCESS_RESPONSE_CODES, element => element == Convert.ToString(responseObject.ResponseCode));
                        log.Debug($"Check if isTransactionSucceeded is null(Tx failed) or not null(Tx succeeded)={isTransactionSucceeded}");
                        if (string.IsNullOrWhiteSpace(isTransactionSucceeded))
                        {
                            // Tx failed
                            log.Error($"Reversal failed: {Convert.ToString(responseObject.ResponseText)}");
                            throw new Exception($"Reversal failed: {Convert.ToString(responseObject.ResponseText)}");
                        }

                        log.Debug("Void Response - Validation Successfull");
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                        transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                        transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                        log.LogVariableState("VoidPayment() transactionPaymentsDTO", transactionPaymentsDTO);
                    }
                    else
                    {
                        log.Error($"responseObject was null");
                        throw new Exception($"Reversal failed");
                    }
                }
                else
                {
                    log.Error("Exception in processing Payment:transactionPaymentsDTO was null");
                    throw new Exception("Exception in processing Payment");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Exception in processing Payment " + ex.Message);
            }
            finally
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                    statusDisplayUi = null;
                }
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public override TransactionPaymentsDTO PerformSettlement(TransactionPaymentsDTO transactionPaymentsDTO, bool IsForcedSettlement = false)
        {
            log.LogMethodEntry(transactionPaymentsDTO, IsForcedSettlement);
            GeideaResponseDTO responseObject;
            try
            {
                string isTransactionSucceeded = string.Empty;
                if (transactionPaymentsDTO != null)
                {
                    double baseAmount = transactionPaymentsDTO.Amount * 100;

                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Geidea Payment Gateway");

                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;

                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    log.LogVariableState("ccRequestPGWDTO", ccRequestPGWDTO);


                    if (!IsForcedSettlement)
                    {
                        log.Debug("Starts Capture");
                        // Geidea ADVICE(Capture) TrnxType=13 
                        var requestDTO = new GeideaRequestDTO(
                            trnxAmount: Convert.ToInt64(baseAmount),
                            naqdAmount: null,
                            refundAmount: null,
                            isPrintReceiptEnabled: isPrintReceiptEnabled, // printing is enabled
                            trxnRrn: null,
                            originalRefundDate: null,
                            ecrRefNumber: ccRequestPGWDTO.RequestID.ToString(),
                            cardNumber: null,
                            trxnApprovalNumber: ccOrigTransactionsPGWDTO.RecordNo,
                            isUnattended: false,
                            trnxType: 13, // advice/capture
                            terminalID: deviceId,
                            cashierNo: null,
                            repeatCommandRequestBuffer: null,
                            port: port,
                            baudRate: baudRate,
                            parityBit: parityBit,
                            dataBit: dataBit,
                            stopBit: stopBit,
                            paymentId: transactionPaymentsDTO.PaymentId.ToString()
                            );
                        log.Debug("Initiate Capture Request");
                        log.LogVariableState("PerformSettlement Capture requestDTO", requestDTO);
                        responseObject = MakeTransactionRequest(requestDTO, ccRequestPGWDTO);
                        log.LogVariableState("PerformSettlement Capture responseObjectCapture", responseObject);
                        if (responseObject != null)
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));


                            if (string.IsNullOrWhiteSpace(responseObject.ResponseCode))
                            {
                                log.Error($"responseObject.ResponseCode was null");
                                throw new Exception($"Settlement failed");
                            }
                            log.Debug("Received Capture Response");

                            double resamount = Convert.ToDouble(responseObject.PurAmount);
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.PanNo);
                            cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                            cCTransactionsPGWDTO.CardType = responseObject.SchemeId;
                            cCTransactionsPGWDTO.RefNo = responseObject.EcrRefNo;
                            cCTransactionsPGWDTO.RecordNo = responseObject.TrxRrn;
                            cCTransactionsPGWDTO.TextResponse = responseObject.ResponseText;
                            cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.CAPTURE.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = resamount.ToString();
                            cCTransactionsPGWDTO.AcqRefData = GetMaskedMid(Convert.ToString(responseObject.MerchantId));
                            SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                            ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            log.Debug("Capture Response saved to CCTransactionPGW");
                            log.LogVariableState("PerformSettlement Capture ccTransactionsPGWBL", ccTransactionsPGWBL);
                            log.Debug("Validating Capture Response against the success response codes");
                            isTransactionSucceeded = Array.Find(SUCCESS_RESPONSE_CODES, element => element == Convert.ToString(responseObject.ResponseCode));
                            log.Debug($"Check if isTransactionSucceeded is null(Tx failed) or not null(Tx succeeded)={isTransactionSucceeded}");
                            if (string.IsNullOrWhiteSpace(isTransactionSucceeded))
                            {
                                // Tx failed
                                log.Error($"PerformSettlement failed: {Convert.ToString(responseObject.ResponseText)}");
                                throw new Exception($"Settlement failed: {Convert.ToString(responseObject.ResponseText)}");
                            }
                            log.Debug("Capture Response - Validation successfull");

                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.Amount = resamount;
                            transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            log.LogVariableState("PerformSettlement Capture transactionPaymentsDTO", transactionPaymentsDTO);

                        }
                        else
                        {
                            log.LogMethodExit(transactionPaymentsDTO);
                            log.Error("Capture response was null");
                            throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                        }
                    }
                }
                else
                {
                    statusDisplayUi.DisplayText("Invalid payment data.");
                    log.Error("Capture transactionPaymentsDTO was null");
                    throw new Exception(utilities.MessageUtils.getMessage("Invalid payment data."));
                }
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
                {
                    statusDisplayUi.CloseStatusWindow();
                    statusDisplayUi = null;
                }
            }
            log.LogMethodExit(transactionPaymentsDTO);
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
        public override void SendLastTransactionStatusCheckRequest(CCRequestPGWDTO cCRequestPGWDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);
            GeideaResponseDTO responseObject;
            try
            {
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();

                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage("Checking the transaction status" + ((cCRequestPGWDTO != null) ? " of TrxId:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "Geidea  Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                CCTransactionsPGWDTO ccTransactionsPGWDTOResponse = null;


                if (cCTransactionsPGWDTO != null)
                {
                    log.Debug("cCTransactionsPGWDTO is not null");
                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOcapturedList = null;
                    if (!string.IsNullOrEmpty(cCTransactionsPGWDTO.TextResponse) && !cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.REFUND.ToString()) && !cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.VOID.ToString()))
                    {
                        thr.Start();
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
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

                        // Geidea REPEAT(Get Last Transacation) TrnxType=21
                        // ================ IMPORTANT ====================================
                        // trnxType=21 is not provided by Geidea, rather it is decided by us for our convinience
                        //statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                        // responseObject = GetLastTransactionCheck(cCRequestPGWDTO);
                        log.Debug("Starts Last Transaction check");
                        //var requestDTO = new GeideaRequestDTO(
                        //    trnxAmount: null,
                        //    naqdAmount: null,
                        //    refundAmount: null,
                        //    isPrintReceiptEnabled: isPrintReceiptEnabled, // printing is disabled
                        //    trxnRrn: null,
                        //    originalRefundDate: null,
                        //    ecrRefNumber: null,
                        //    cardNumber: null,
                        //    trxnApprovalNumber: null,
                        //    isUnattended: false,
                        //    trnxType: 21, // Repeat
                        //    terminalID: deviceId,
                        //    cashierNo: null,
                        //    repeatCommandRequestBuffer: "0B",
                        //    port: port,
                        //    baudRate: baudRate,
                        //    parityBit: parityBit,
                        //    dataBit: dataBit,
                        //    stopBit: stopBit,
                        //    paymentId: null
                        //    );
                        //log.LogVariableState("Last Trx Check requestDTO", requestDTO);
                        responseObject = MakeLastTransactionCheckRequest();
                        log.LogVariableState("Last Trx Check responsePaymentObject", responseObject);

                        if (responseObject == null)
                        {
                            log.Error($"responseObject was null");
                            return;
                            //throw new Exception($"Last Trx Check failed");
                        }
                        if (string.IsNullOrWhiteSpace(responseObject.ResponseCode))
                        {
                            log.Error($"responseObject.ResponseCode was null. Last Transaction Check failed");
                            return;
                            //throw new Exception($"Last Trx Check failed");
                        }
                        log.Debug("Received Last trx Check Response");
                        string requestId = cCRequestPGWDTO.RequestID.ToString();
                        log.LogVariableState("requestId", requestId);
                        string ecrReqNo = responseObject.EcrRefNo.Substring(responseObject.EcrRefNo.Length - requestId.Length);
                        log.LogVariableState("ecrReqNo", ecrReqNo);
                        if (ecrReqNo != requestId)
                        {
                            log.Error($"responseObject.EcrRefNo != cCRequestPGWDTO.RequestID. Last Transaction Check failed.");
                            return;
                        }
                        double resamount = Convert.ToDouble(responseObject.PurAmount);
                        ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();
                        ccTransactionsPGWDTOResponse.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTOResponse.AcctNo = GetMaskedCardNumber(responseObject.PanNo);
                        ccTransactionsPGWDTOResponse.AuthCode = responseObject.AuthCode;
                        ccTransactionsPGWDTOResponse.CardType = responseObject.SchemeId;
                        ccTransactionsPGWDTOResponse.RefNo = responseObject.EcrRefNo;
                        ccTransactionsPGWDTOResponse.RecordNo = responseObject.TrxRrn;
                        ccTransactionsPGWDTOResponse.TextResponse = responseObject.ResponseText;
                        ccTransactionsPGWDTOResponse.TranCode = cCRequestPGWDTO.TransactionType;
                        ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                        ccTransactionsPGWDTOResponse.Authorize = resamount.ToString();
                        log.LogVariableState("Last Trx Check ccTransactionsPGWDTOResponse", ccTransactionsPGWDTOResponse);

                        string isTransactionSucceeded = Array.Find(SUCCESS_RESPONSE_CODES, element => element == Convert.ToString(responseObject.ResponseCode));
                        log.Debug($"Check if isTransactionSucceeded is null(Tx failed) or not null(Tx succeeded)={isTransactionSucceeded}");
                        if (string.IsNullOrWhiteSpace(isTransactionSucceeded))
                        {
                            // Tx failed
                            log.Error($"Last Trx Check failed: {Convert.ToString(responseObject.ResponseText)}");
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTOResponse);
                            ccTransactionsPGWBL.Save();
                            log.Debug("Last Trx Check saved to cCTransactionsPGW");
                            return;
                            //throw new Exception($"Last Trx Check failed: {Convert.ToString(responseObject.ResponseText)}");
                        }
                        log.Debug("Last trx check response - Validation successfull");
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
                    //statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                    log.Debug("cCRequestPGWDTO is not null");
                    thr.Start();
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Last Transaction Status Check is Processing..."));
                    //responseObject = GetLastTransactionCheck(cCRequestPGWDTO);
                    log.Debug("Starts Last Transaction Check");
                    //var requestDTO = new GeideaRequestDTO(
                    //       trnxAmount: null,
                    //       naqdAmount: null,
                    //       refundAmount: null,
                    //       isPrintReceiptEnabled: isPrintReceiptEnabled, // printing is disabled
                    //       trxnRrn: null,
                    //       originalRefundDate: null,
                    //       ecrRefNumber: null,
                    //       cardNumber: null,
                    //       trxnApprovalNumber: null,
                    //       isUnattended: false,
                    //       trnxType: 21, // Repeat
                    //       terminalID: deviceId,
                    //       cashierNo: null,
                    //       repeatCommandRequestBuffer: "0B",
                    //       port: port,
                    //       baudRate: baudRate,
                    //       parityBit: parityBit,
                    //       dataBit: dataBit,
                    //       stopBit: stopBit,
                    //       paymentId: null
                    //       );
                    //log.LogVariableState("Last Trx Check requestDTO", requestDTO);
                    responseObject = MakeLastTransactionCheckRequest();
                    log.LogVariableState("Last Trx Check responsePaymentObject", responseObject);

                    if (responseObject == null)
                    {
                        log.Error($"responseObject was null");
                        return;
                        //throw new Exception($"Last Trx Check failed");
                    }
                    if (string.IsNullOrWhiteSpace(responseObject.ResponseCode))
                    {
                        log.Error($"responseObject. ResponseCode was null");
                        return;
                        //throw new Exception($"Last Trx Check failed");
                    }
                    log.Debug("Last trx check response received");
                    string requestId = cCRequestPGWDTO.RequestID.ToString();
                    log.Debug("requestId: " + requestId);
                    string ecrReqNo = responseObject.EcrRefNo.Substring(responseObject.EcrRefNo.Length - requestId.Length);
                    log.Debug("ecrReqNo :" + ecrReqNo);
                    if (ecrReqNo != requestId)
                    {
                        log.Error($"responseObject.EcrRefNo != cCRequestPGWDTO.RequestID. Last Transaction Check failed.");
                        return;
                    }

                    double resamount = Convert.ToDouble(responseObject.PurAmount);
                    ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTOResponse.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTOResponse.AcctNo = GetMaskedCardNumber(responseObject.PanNo);
                    ccTransactionsPGWDTOResponse.AuthCode = responseObject.AuthCode;
                    ccTransactionsPGWDTOResponse.CardType = responseObject.SchemeId;
                    ccTransactionsPGWDTOResponse.RefNo = responseObject.EcrRefNo;
                    ccTransactionsPGWDTOResponse.RecordNo = responseObject.TrxRrn;
                    ccTransactionsPGWDTOResponse.TextResponse = responseObject.ResponseText;
                    ccTransactionsPGWDTOResponse.TextResponse = responseObject.ResponseText;
                    ccTransactionsPGWDTOResponse.TranCode = cCRequestPGWDTO.TransactionType;


                    ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                    ccTransactionsPGWDTOResponse.Authorize = resamount.ToString();
                    log.LogVariableState("Last Trx Check ccTransactionsPGWDTOResponse", ccTransactionsPGWDTOResponse);
                    log.Debug("Last trx check response - Validating against success codes");
                    string isTransactionSucceeded = Array.Find(SUCCESS_RESPONSE_CODES, element => element == Convert.ToString(responseObject.ResponseCode));
                    log.Debug($"Check if isTransactionSucceeded is null(Tx failed) or not null(Tx succeeded)={isTransactionSucceeded}");
                    if (string.IsNullOrWhiteSpace(isTransactionSucceeded))
                    {
                        // Trx failed
                        log.Error($"Last Trx Check failed: {Convert.ToString(responseObject.ResponseText)}");
                        log.Debug("Saving ccTransactionsPGWDTOResponse.");
                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTOResponse);
                        ccTransactionsPGWBL.Save();
                        log.Debug("Last Trx Check saved to cCTransactionsPGW");
                        return;
                    }
                    log.Debug("Last trx check response - Validation successfull");
                }

                // }
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

                        //ccTransactionsPGWDTOResponse.TranCode = "SALE";
                        if (cCTransactionsPGWDTO == null)
                        {
                            log.Debug("Saving ccTransactionsPGWDTOResponse.");
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTOResponse);
                            ccTransactionsPGWBL.Save();
                        }
                        log.LogVariableState("ccTransactionsPGWDTOResponse", ccTransactionsPGWDTOResponse);
                        if (!string.IsNullOrEmpty(ccTransactionsPGWDTOResponse.TextResponse) && (cCRequestPGWDTO.TransactionType == TransactionType.SALE.ToString()
                            || cCRequestPGWDTO.TransactionType == TransactionType.AUTHORIZATION.ToString()))
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
                                statusDisplayUi = null;
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
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                    statusDisplayUi = null;
                }
            }
            log.LogMethodExit();
        }

        private string GetMaskedCardNumber(string cardNumber)
        {
            log.LogMethodEntry();
            string card = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    return card;
                }
                // TBC assumption => card has 16 digits
                card = string.Format("************{0}", cardNumber.Substring(cardNumber.Length - 4, 4));

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(card);
            return card;
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
                    string maskedMerchantId = GetMaskedMid(ccTransactionsPGWDTO.AcqRefData);
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Merchant ID") + "     : ".PadLeft(12) + maskedMerchantId, Alignment.Left);
                }
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Date") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TransactionDatetime.ToString("MMM dd yyyy HH:mm"), Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Type") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TranCode, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Invoice Number") + "  : ".PadLeft(6) + ccTransactionsPGWDTO.InvoiceNo, Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.AuthCode))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Authorization") + "   : ".PadLeft(10) + ccTransactionsPGWDTO.AuthCode, Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.CardType))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Card Type") + "       : ".PadLeft(15) + ccTransactionsPGWDTO.CardType, Alignment.Left);
                if (!string.IsNullOrEmpty(trxPaymentsDTO.NameOnCreditCard))
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
                if (ccTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Amount") + " : " + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    //receiptText += Environment.NewLine;
                }
                else
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                           : " + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    receiptText += Environment.NewLine;
                }
                if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse)) && ccTransactionsPGWDTO.TranCode.ToUpper().Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                          : " + "_____________", Alignment.Left);
                }
                receiptText += Environment.NewLine;
                if (IsMerchantCopy)
                {
                    if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse)))
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
        private string GetMaskedMid(string mid)
        {
            log.LogMethodEntry();
            string maskedMid = string.Empty;
            if (!string.IsNullOrWhiteSpace(mid))
            {
                maskedMid = mid.Length > 4 ? new string('X', mid.Length - 4) + mid.Substring(mid.Length - 4) : mid;
            }
            log.LogMethodExit(maskedMid);
            return maskedMid;
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

