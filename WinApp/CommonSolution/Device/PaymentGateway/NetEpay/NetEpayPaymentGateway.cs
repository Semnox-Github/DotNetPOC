using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Reflection;
using System.Net.Security;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Runtime.InteropServices;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class NetEpayPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();
        string merchantId;
        string hostIp;
        string hostIpPort;
        string pinPadIp;
        string pinPadIpPort;
        string comPort;
        string sourceDevice;
        string version;
        string currencyCode;
        bool isAuthEnabled;
        bool isCustomerAllowedToDecideEntryMode;
        bool isManual;
        bool isSignatureRequired;
        string validator;
        bool enableAutoAuthorization;
        DSIEMVXLib.DsiEMVX dsiEMVX;

        public enum RecieptPrintAlignment
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public NetEpayPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
           : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel   
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers

            try
            {
                merchantId = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_STORE_ID");
                validator = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_HOST_URL");
                if (string.IsNullOrEmpty(validator))
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2859));
                }
                string[] hostUrl = validator.Split(':');
                if (hostUrl.Length < 2)
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2860));
                }
                hostIp = hostUrl[0];
                hostIpPort = hostUrl[1];
                validator = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_DEVICE_URL");
                if (string.IsNullOrEmpty(validator))
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2861));
                }
                string[] pinPad = validator.Split(':');
                if (pinPad.Length < 2)
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2862));
                }
                pinPadIp = pinPad[0];
                pinPadIpPort = pinPad[1];
                comPort = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_TERMINAL_PORT_NO");
                sourceDevice = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_TERMINAL_ID");

                isAuthEnabled = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
                isCustomerAllowedToDecideEntryMode = utilities.getParafaitDefaults("ALLOW_CUSTOMER_TO_DECIDE_ENTRY_MODE").Equals("Y");
                isSignatureRequired = !utilities.getParafaitDefaults("ENABLE_SIGNATURE_VERIFICATION").Equals("N");
                enableAutoAuthorization = utilities.getParafaitDefaults("ENABLE_AUTO_CREDITCARD_AUTHORIZATION").Equals("Y");
                currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString();




                if (string.IsNullOrEmpty(merchantId))
                {
                    throw new Exception(utilities.MessageUtils.getMessage(2863));
                }

                if (string.IsNullOrEmpty(comPort))
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2864));
                }
                if (string.IsNullOrEmpty(sourceDevice))
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2865));
                }
                dsiEMVX = new DSIEMVXLib.DsiEMVX();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while initialize gateway", ex);
                throw;
            }

            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            log.LogMethodEntry();
            PadReset();
            CheckLastTransactionStatus();
            log.LogMethodExit();
        }

        public override void SendLastTransactionStatusCheckRequest(CCRequestPGWDTO cCRequestPGWDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);

            NetEpayRequestDTO netEpayRequestDTO = new NetEpayRequestDTO(hostIp, hostIpPort, merchantId, sourceDevice, comPort, pinPadIp, pinPadIpPort);
            NetEpayRequestBL netEpayRequestBL = new NetEpayRequestBL(utilities.ExecutionContext);
            CCTransactionsPGWDTO ccTransactionsPGWDTOResponse = null;
            //Form activeForm = GetActiveForm();
            statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage("Checking the transaction status" + ((cCRequestPGWDTO != null) ? " of TrxId:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "NetEpay Payment Gateway");
            //{
            statusDisplayUi.EnableCancelButton(false);
            //Form form = statusDisplayUi as Form;

            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
            thr.Start();
            //form.Show(activeForm);
            //SetNativeEnabled(activeForm, false);
            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            try
            {
                if (cCTransactionsPGWDTO != null)
                {
                    log.Debug("cCTransactionsPGWDTO is not null");

                    if (!string.IsNullOrEmpty(cCTransactionsPGWDTO.RecordNo) && cCTransactionsPGWDTO.RecordNo.Equals("A") && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.REFUND.ToString()) && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.VOID.ToString()))
                    {
                        if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
                        {
                            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTO.ResponseID.ToString()));
                            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                            CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();
                            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);

                            if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                            {
                                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);

                                if (cCTransactionsPGWDTOList[0].TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()))
                                {
                                    ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                                    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOcapturedList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
                                    if (cCTransactionsPGWDTOcapturedList != null && cCTransactionsPGWDTOcapturedList.Count > 0)
                                    {
                                        log.Debug("The authorized transaction is captured.");
                                        return;
                                    }
                                }
                                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                                transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
                                transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                                List<TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);
                                if (transactionPaymentsDTOs == null || transactionPaymentsDTOs.Count == 0)
                                {
                                    cCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                                }
                                else
                                {
                                    log.Debug("The capture transaction exists for the authorization request with requestId:" + cCTransactionsPGWDTO.InvoiceNo + " and its upto date");
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

                        netEpayRequestDTO.UserTrace = utilities.ExecutionContext.GetMachineId().ToString();
                        netEpayRequestDTO.OperatorID = utilities.ExecutionContext.GetUserId().ToString();
                        netEpayRequestDTO = netEpayRequestBL.CreateTransactionStatusEnquiry(netEpayRequestDTO, cCRequestPGWDTO);

                        string resultString = string.Empty;
                        TStream tStream = new TStream(null, netEpayRequestDTO);
                        tStream.Admin = netEpayRequestDTO;

                        XmlSerializer requestXml = new XmlSerializer(tStream.GetType());
                        MemoryStream requestStream = new MemoryStream();

                        requestXml.Serialize(requestStream, tStream);
                        requestStream.Position = 0;
                        using (StreamReader reader = new StreamReader(requestStream))
                        {
                            String requestString = reader.ReadToEnd();
                            log.LogVariableState("requestString", requestString);
                            resultString = dsiEMVX.ProcessTransaction(requestString);
                            log.LogVariableState("resultString", resultString);
                        }

                        RStream rStream = new RStream();
                        XmlSerializer serializer = new XmlSerializer(rStream.GetType());
                        using (StringReader sr = new StringReader(resultString))
                        {
                            rStream = (RStream)serializer.Deserialize(sr);
                        }
                        if (rStream.CmdResponse.CmdStatus.Equals("Success"))
                        {
                            ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO(-1, cCRequestPGWDTO.RequestID.ToString(), null, "A", null, -1, rStream.CmdResponse.TextResponse,
                                                                  null, rStream.BatchReportTransaction.CardType, PaymentGatewayTransactionType.SALE.ToString(), rStream.BatchReportTransaction.RefNo,
                                                                  null, rStream.BatchReportTransaction.Authorize, utilities.getServerTime(), rStream.BatchReportTransaction.AuthCode,
                                                                  rStream.TranResponse.ProcessData, null, null, null, null, null, null, null, null);
                        }
                        else
                        {
                            log.Error(rStream.CmdResponse.TextResponse);
                            throw new Exception(rStream.CmdResponse.TextResponse);
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
                    netEpayRequestDTO.UserTrace = utilities.ExecutionContext.GetMachineId().ToString();
                    netEpayRequestDTO.OperatorID = utilities.ExecutionContext.GetUserId().ToString();
                    netEpayRequestDTO = netEpayRequestBL.CreateTransactionStatusEnquiry(netEpayRequestDTO, cCRequestPGWDTO);

                    string resultString = string.Empty;
                    TStream tStream = new TStream(null, netEpayRequestDTO);
                    tStream.Admin = netEpayRequestDTO;

                    XmlSerializer requestXml = new XmlSerializer(tStream.GetType());
                    MemoryStream requestStream = new MemoryStream();

                    requestXml.Serialize(requestStream, tStream);
                    requestStream.Position = 0;
                    using (StreamReader reader = new StreamReader(requestStream))
                    {
                        String requestString = reader.ReadToEnd();
                        log.LogVariableState("requestString", requestString);
                        resultString = dsiEMVX.ProcessTransaction(requestString);
                        log.LogVariableState("resultString", resultString);
                    }

                    RStream rStream = new RStream();
                    XmlSerializer serializer = new XmlSerializer(rStream.GetType());
                    using (StringReader sr = new StringReader(resultString))
                    {
                        rStream = (RStream)serializer.Deserialize(sr);
                    }
                    if (rStream.CmdResponse.CmdStatus == "Success")
                    {
                        ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO(-1, cCRequestPGWDTO.RequestID.ToString(), null, "A", null, -1, rStream.CmdResponse.TextResponse,
                                                                  null, rStream.BatchReportTransaction.CardType, PaymentGatewayTransactionType.SALE.ToString(), rStream.BatchReportTransaction.RefNo,
                                                                  null, rStream.BatchReportTransaction.Authorize, utilities.getServerTime(), rStream.BatchReportTransaction.AuthCode,
                                                                  null, null, null, null, null, null, null, null, null);
                    }
                    else
                    {
                        log.Error(rStream.CmdResponse.TextResponse);
                        throw new Exception(rStream.CmdResponse.TextResponse);
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
                log.Error("Last transaction check failed", ex);
                throw;
            }
            finally
            {
                log.Debug("Reached finally.");
                try
                {
                    //SetNativeEnabled(activeForm, true);
                    log.LogVariableState("statusDisplayUi", statusDisplayUi);
                    if (statusDisplayUi != null)
                    {
                        statusDisplayUi.CloseStatusWindow();
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("Exception three without throw in finally");
                    log.Error(ex);
                }
            }
            //}
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
            dsiEMVX.CancelRequest();
            log.LogMethodExit(null);
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            //Form activeForm = GetActiveForm();
            statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "NetEpay Payment Gateway");
            //{
            statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
            statusDisplayUi.EnableCancelButton(false);
            isManual = false;
            //Form form = statusDisplayUi as Form;
            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
            TransactionType trxType = TransactionType.SALE;
            CCTransactionsPGWDTO cCOrgTransactionsPGWDTO = null;
            string test = utilities.ParafaitEnv.POSMachine;
            NetEpayRequestBL netEpayRequestBL = new NetEpayRequestBL(utilities.ExecutionContext);
            try
            {
                PadReset();
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
                                    if (frmTranType.TransactionType.Equals("Authorization"))
                                    {
                                        trxType = TransactionType.AUTHORIZATION;
                                    }
                                    else if (frmTranType.TransactionType.Equals("Sale"))
                                    {
                                        trxType = TransactionType.SALE;
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
                else
                {
                    log.Debug("Standalone refund triggered");
                    transactionPaymentsDTO.Amount = transactionPaymentsDTO.Amount * -1;
                    TransactionPaymentsDTO transactionPaymentDTO = RefundAmount(transactionPaymentsDTO);
                    transactionPaymentDTO.Amount = transactionPaymentDTO.Amount * -1;
                    return transactionPaymentDTO;
                }
                thr.Start();

                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                NetEpayRequestDTO netEpayRequestDTO = new NetEpayRequestDTO(hostIp, hostIpPort, merchantId, sourceDevice, comPort, pinPadIp, pinPadIpPort);
                netEpayRequestDTO.UserTrace = utilities.ExecutionContext.GetMachineId().ToString();
                netEpayRequestDTO.OperatorID = utilities.ExecutionContext.GetUserId().ToString();
                netEpayRequestDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                netEpayRequestDTO.RefNo = ccRequestPGWDTO.RequestID.ToString();
                if (!isCustomerAllowedToDecideEntryMode) //Avoid confirmation message on device
                    netEpayRequestDTO.OKAmount = "DisAllow";

                if (isCustomerAllowedToDecideEntryMode && cCOrgTransactionsPGWDTO == null)
                {
                    frmEntryMode entryMode = new frmEntryMode();
                    utilities.setLanguage(entryMode);
                    entryMode.ShowDialog();
                    isManual = entryMode.IsManual;
                }

                //form.Show(activeForm);
                //SetNativeEnabled(activeForm, false);
                string resultString = string.Empty;
                //string amount = transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                if (cCOrgTransactionsPGWDTO != null && trxType == TransactionType.AUTHORIZATION)
                {
                    netEpayRequestDTO = netEpayRequestBL.CreatePreAuthRequest(netEpayRequestDTO, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), true);
                    //netEpayRequestDTO.InvoiceNo = cCOrgTransactionsPGWDTO.InvoiceNo;
                    netEpayRequestDTO.RecordNo = cCOrgTransactionsPGWDTO.TokenID;
                }
                else if (cCOrgTransactionsPGWDTO != null && trxType == TransactionType.SALE)
                {
                    //code for create EMVSale with ZeroAuth
                    netEpayRequestDTO = netEpayRequestBL.CreatePaymentRequest(netEpayRequestDTO, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), true);
                    netEpayRequestDTO.RecordNo = cCOrgTransactionsPGWDTO.TokenID;
                    //netEpayRequestDTO.InvoiceNo = cCOrgTransactionsPGWDTO.InvoiceNo;
                    netEpayRequestDTO.RefNo = cCOrgTransactionsPGWDTO.RefNo;
                }
                else if (trxType == TransactionType.AUTHORIZATION)
                {
                    //code for PreAuth
                    netEpayRequestDTO = netEpayRequestBL.CreatePreAuthRequest(netEpayRequestDTO, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT));
                }
                else if (trxType == TransactionType.SALE)
                {
                    netEpayRequestDTO = netEpayRequestBL.CreatePaymentRequest(netEpayRequestDTO, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT));
                }
                else if (trxType == TransactionType.TATokenRequest)
                {
                    //code for zero auth
                    netEpayRequestDTO = netEpayRequestBL.CreateZeroAuthRequest(netEpayRequestDTO, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT));
                }

                NetEpayAccount account;
                if (isManual)
                {
                    account = new NetEpayAccount("Prompt");
                    netEpayRequestDTO.Account = account;
                }
                else if (cCOrgTransactionsPGWDTO == null)
                {
                    account = new NetEpayAccount("SecureDevice");
                    netEpayRequestDTO.Account = account;
                }

                TStream tStream = new TStream(netEpayRequestDTO, null);

                XmlSerializer requestXml = new XmlSerializer(tStream.GetType());
                MemoryStream requestStream = new MemoryStream();
                log.Debug("Payment Process start");
                requestXml.Serialize(requestStream, tStream);
                requestStream.Position = 0;
                statusDisplayUi.EnableCancelButton(true);
                using (StreamReader reader = new StreamReader(requestStream))
                {
                    String requestString = reader.ReadToEnd();
                    log.LogVariableState("requestString", requestString);
                    resultString = dsiEMVX.ProcessTransaction(requestString);
                    log.LogVariableState("resultString", resultString);
                }

                XElement printData = XElement.Parse(resultString).Element("PrintData");
                RStream rStream = new RStream();
                XmlSerializer serializer = new XmlSerializer(rStream.GetType());
                using (StringReader sr = new StringReader(resultString))
                {
                    rStream = (RStream)serializer.Deserialize(sr);
                }
                if (rStream.CmdResponse.CmdStatus.Equals("Approved"))
                {
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, rStream.TranResponse.InvoiceNo, rStream.TranResponse.RecordNo, "A", rStream.CmdResponse.DSIXReturnCode,
                                                                 -1, rStream.CmdResponse.TextResponse, rStream.TranResponse.AcctNo, rStream.TranResponse.CardType, trxType.ToString(),
                                                                 rStream.TranResponse.RefNo, rStream.TranResponse.Amount.Purchase, rStream.TranResponse.Amount.Authorize, utilities.getServerTime(),
                                                                 rStream.TranResponse.AuthCode, rStream.TranResponse.ProcessData != null ? rStream.TranResponse.ProcessData : null, rStream.CmdResponse.ResponseOrigin,
                                                                 null, rStream.TranResponse.EntryMethod, rStream.TranResponse.AcqRefData, rStream.TranResponse.Amount.Gratuity, null, null, null);

                    SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO, printData);

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();
                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                    transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                    //transactionPaymentsDTO.Amount = 0.50;
                    transactionPaymentsDTO.TipAmount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.TipAmount);
                    transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                }

                else
                {
                    log.Error(rStream.CmdResponse.TextResponse);
                    throw new Exception(rStream.CmdResponse.ResponseOrigin + ": " + rStream.CmdResponse.TextResponse);
                }

            }
            catch (Exception ex)
            {
                log.Error("Error occured while making payment", ex);
                log.LogMethodExit(null, "Throwing Payment Gateway Exception-" + ex.Message);
                throw new PaymentGatewayException(ex.Message);
            }
            finally
            {
                //SetNativeEnabled(activeForm, true);
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
                PadReset();
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
        //    if(activeForm == null)
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
        /// 
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
                PadReset();
                DSIEMVXLib.DsiEMVX dsiEMVX = new DSIEMVXLib.DsiEMVX();

                if (transactionPaymentsDTO != null)
                {
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(4202, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "NetEpay Payment Gateway");
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
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
                        CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                        CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;
                        NetEpayRequestBL netEpayRequestBL = new NetEpayRequestBL(utilities.ExecutionContext);
                        string resultString = string.Empty;
                        NetEpayRequestDTO netEpayRequestDTO = new NetEpayRequestDTO(hostIp, hostIpPort, merchantId, sourceDevice, comPort, pinPadIp, pinPadIpPort);
                        netEpayRequestDTO.UserTrace = utilities.ExecutionContext.GetMachineId().ToString();
                        netEpayRequestDTO.OperatorID = utilities.ExecutionContext.GetUserId().ToString();
                        double trxlineAmount = (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount);
                        if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) > Convert.ToDecimal(trxlineAmount)))
                        {
                            transactionPaymentsDTO.TipAmount = 0.0;
                        }
                        double amount = (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount);
                        netEpayRequestDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        netEpayRequestDTO.RefNo = cCRequestPGWDTO.RequestID.ToString();
                        if (transactionPaymentsDTO.CCResponseId == -1)
                        {
                            netEpayRequestDTO = netEpayRequestBL.CreateReturnRequest(netEpayRequestDTO, amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT));
                        }
                        else
                        {
                            if (isUnattended)
                            {
                                netEpayRequestDTO = netEpayRequestBL.CreateVoidRequest(netEpayRequestDTO, ccOrigTransactionsPGWDTO, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), transactionPaymentsDTO.TipAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT));
                            }
                            else
                            {
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
                                    if (Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize) == amount)
                                    {
                                        // create void transaction
                                        netEpayRequestDTO = netEpayRequestBL.CreateVoidRequest(netEpayRequestDTO, ccOrigTransactionsPGWDTO, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), transactionPaymentsDTO.TipAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT));
                                    }
                                    else
                                    {
                                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2868));
                                    }
                                }
                                else
                                {
                                    netEpayRequestDTO = netEpayRequestBL.CreateReturnRequest(netEpayRequestDTO, amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), false);
                                    netEpayRequestDTO.RecordNo = ccOrigTransactionsPGWDTO.TokenID;
                                }
                            }

                        }

                        TStream tStream = new TStream(netEpayRequestDTO, null);
                        //tStream.Admin = netEpayRequestDTO;

                        XmlSerializer requestXml = new XmlSerializer(tStream.GetType());
                        MemoryStream requestStream = new MemoryStream();

                        requestXml.Serialize(requestStream, tStream);
                        requestStream.Position = 0;
                        using (StreamReader reader = new StreamReader(requestStream))
                        {
                            String requestString = reader.ReadToEnd();
                            log.LogVariableState("requestString", requestString);
                            resultString = dsiEMVX.ProcessTransaction(requestString);
                            log.LogVariableState("resultString", resultString);
                        }

                        XElement printData = XElement.Parse(resultString).Element("PrintData");
                        RStream rStream = new RStream();
                        XmlSerializer serializer = new XmlSerializer(rStream.GetType());
                        using (StringReader sr = new StringReader(resultString))
                        {
                            rStream = (RStream)serializer.Deserialize(sr);
                        }
                        if (rStream.CmdResponse.CmdStatus.Equals("Approved"))
                        {

                            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, rStream.TranResponse.InvoiceNo, null, "A", null, -1, rStream.CmdResponse.TextResponse,
                                                                            rStream.TranResponse.AcctNo, rStream.TranResponse.CardType, PaymentGatewayTransactionType.REFUND.ToString(), null, null, rStream.TranResponse.Amount.Authorize,
                                                                            utilities.getServerTime(), null, null, null, null, null, null, null, null, null, null);
                            ccTransactionsPGWDTO.TipAmount = transactionPaymentsDTO.TipAmount.ToString();
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Approved") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));
                            if (ccOrigTransactionsPGWDTO != null)
                            {
                                ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                            }

                            CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                            SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO, printData);
                            cCTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = cCTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        }
                        else
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(rStream.CmdResponse.TextResponse));
                            log.Error(rStream.CmdResponse.TextResponse);
                            throw new Exception(rStream.CmdResponse.CmdStatus + ": " + rStream.CmdResponse.TextResponse);
                        }
                    }
                    catch (Exception ex)
                    {
                        statusDisplayUi.DisplayText("Error occured while Refunding the Amount");
                        log.Error("Error occured while Refunding the Amount", ex);
                        log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                        log.LogMethodExit(null, "throwing Exception");
                        throw;
                    }
                    finally
                    {
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
                log.Error("Error occured while Refunding the Amount", ex);
                log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                log.LogMethodExit(null, "throwing Exception");
                throw;
            }
            finally
            {
                PadReset();
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="IsForcedSettlement"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO PerformSettlement(TransactionPaymentsDTO transactionPaymentsDTO, bool IsForcedSettlement = false)
        {
            log.LogMethodEntry(transactionPaymentsDTO, IsForcedSettlement);
            //Form activeForm = GetActiveForm();
            try
            {
                PadReset();

                DSIEMVXLib.DsiEMVX dsiEMVX = new DSIEMVXLib.DsiEMVX();

                if (transactionPaymentsDTO != null)
                {


                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;

                    double origTips = 0;
                    double actualTips = transactionPaymentsDTO.TipAmount;
                    double actualAmount = transactionPaymentsDTO.Amount;
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

                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Card Connect Payment Gateway");
                    //{
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    try
                    {
                        thr.Start();
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                        NetEpayRequestBL netEpayRequestBL = new NetEpayRequestBL(utilities.ExecutionContext);
                        CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                        NetEpayRequestDTO netEpayRequestDTO = new NetEpayRequestDTO(hostIp, hostIpPort, merchantId, sourceDevice, comPort, pinPadIp, pinPadIpPort);
                        netEpayRequestDTO.UserTrace = utilities.ExecutionContext.GetMachineId().ToString();
                        netEpayRequestDTO.OperatorID = utilities.ExecutionContext.GetUserId().ToString();
                        netEpayRequestDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();

                        if ((originalPaymentDate >= bussStartTime) && (originalPaymentDate <= bussEndTime))
                        {
                            if (!IsForcedSettlement)
                            {
                                frmFinalizeTransaction frmFinalizeTransaction = new frmFinalizeTransaction(utilities, overallTransactionAmount, overallTipAmountEntered, Convert.ToDecimal(transactionPaymentsDTO.Amount), Convert.ToDecimal(transactionPaymentsDTO.TipAmount), transactionPaymentsDTO.CreditCardNumber, showMessageDelegate);
                                if (frmFinalizeTransaction.ShowDialog() != DialogResult.Cancel)
                                {
                                    transactionPaymentsDTO.TipAmount = Convert.ToDouble(frmFinalizeTransaction.TipAmount);
                                    netEpayRequestBL.CreatePaymentAdjustRequest(netEpayRequestDTO, ccOrigTransactionsPGWDTO, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), transactionPaymentsDTO.TipAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT));
                                }
                                else
                                {
                                    log.LogMethodExit(transactionPaymentsDTO);
                                    throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                                }
                            }
                            else
                            {
                                if (!ccOrigTransactionsPGWDTO.TranCode.Equals("AUTHORIZATION"))
                                {
                                    origTips = Convert.ToDouble(ccOrigTransactionsPGWDTO.TipAmount);
                                    transactionPaymentsDTO.TipAmount -= origTips;
                                    transactionPaymentsDTO.Amount = 0;
                                    netEpayRequestBL.CreateTipAdjustRequest(netEpayRequestDTO, ccOrigTransactionsPGWDTO, transactionPaymentsDTO.Amount, transactionPaymentsDTO.TipAmount);
                                    transactionPaymentsDTO.Amount = actualAmount;
                                    transactionPaymentsDTO.TipAmount = actualTips;
                                }
                                else
                                {
                                    netEpayRequestBL.CreatePaymentAdjustRequest(netEpayRequestDTO, ccOrigTransactionsPGWDTO, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), transactionPaymentsDTO.TipAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT));
                                }
                            }
                        }
                        else
                        {
                            log.Error("Error while settle the transaction,requested transaction is not on same day");
                            throw new Exception("Error while settle the transaction,requested transaction is not on same day");
                        }

                        //Form form = statusDisplayUi as Form;
                        //form.Show(activeForm);
                        //SetNativeEnabled(activeForm, false);
                        string resultString = string.Empty;
                        TStream tStream = new TStream(netEpayRequestDTO, null);

                        XmlSerializer requestXml = new XmlSerializer(tStream.GetType());
                        MemoryStream requestStream = new MemoryStream();

                        requestXml.Serialize(requestStream, tStream);
                        requestStream.Position = 0;
                        using (StreamReader reader = new StreamReader(requestStream))
                        {
                            String requestString = reader.ReadToEnd();
                            log.LogVariableState("requestString", requestString);
                            resultString = dsiEMVX.ProcessTransaction(requestString);
                            log.LogVariableState("resultString", resultString);
                        }

                        XElement printData = XElement.Parse(resultString).Element("PrintData");
                        RStream rStream = new RStream();
                        XmlSerializer serializer = new XmlSerializer(rStream.GetType());
                        using (StringReader sr = new StringReader(resultString))
                        {
                            rStream = (RStream)serializer.Deserialize(sr);
                        }

                        if (rStream.CmdResponse.CmdStatus.Equals("Approved"))
                        {
                            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, rStream.TranResponse.InvoiceNo, rStream.TranResponse.RecordNo, "A", rStream.CmdResponse.DSIXReturnCode,
                                                                          -1, ccOrigTransactionsPGWDTO.TextResponse, ccOrigTransactionsPGWDTO.AcctNo, ccOrigTransactionsPGWDTO.CardType,
                                                                          null, rStream.TranResponse.RefNo, null, rStream.TranResponse.Amount.Authorize, utilities.getServerTime(),
                                                                          ccOrigTransactionsPGWDTO.AuthCode, rStream.TranResponse.ProcessData != null ? rStream.TranResponse.ProcessData : null, ccOrigTransactionsPGWDTO.ResponseID.ToString(),
                                                                          null, ccOrigTransactionsPGWDTO.CaptureStatus, ccOrigTransactionsPGWDTO.AcqRefData, rStream.TranResponse.Amount.Gratuity, null, null, null);
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Approved") + ((ccTransactionsPGWDTO.TextResponse.Equals("Approval")) ? "" : "-" + ccTransactionsPGWDTO.TextResponse));

                            if (ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()) || ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                            {
                                ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.TIPADJUST.ToString();
                                ccTransactionsPGWDTO.TipAmount = transactionPaymentsDTO.TipAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT); ;
                                ccTransactionsPGWDTO.Authorize = (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                            }
                            else
                            {
                                ccTransactionsPGWDTO.TranCode = TransactionType.CAPTURE.ToString();
                            }
                            ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                        }
                        else
                        {
                            statusDisplayUi.DisplayText(rStream.CmdResponse.TextResponse);
                            log.Error(rStream.CmdResponse.TextResponse);
                            throw new Exception(rStream.CmdResponse.CmdStatus + ": " + rStream.CmdResponse.TextResponse);
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
                        if (statusDisplayUi != null)
                            statusDisplayUi.CloseStatusWindow();
                    }
                }
                //}
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while performing settlement", ex);
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw;
            }
            finally
            {
                PadReset();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO PayTip(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            transactionPaymentsDTO = PerformSettlement(transactionPaymentsDTO, true);
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="ccTransactionsPGWDTO"></param>
        private void SendPrintReceiptRequest(TransactionPaymentsDTO transactionPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO, XElement printData)
        {
            if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
            {
                transactionPaymentsDTO.Memo = GetReceiptText(transactionPaymentsDTO, ccTransactionsPGWDTO, false, printData);
            }
            if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y")
            {
                transactionPaymentsDTO.Memo += GetReceiptText(transactionPaymentsDTO, ccTransactionsPGWDTO, true, printData);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trxPaymentsDTO"></param>
        /// <param name="ccTransactionsPGWDTO"></param>
        /// <param name="IsMerchantCopy"></param>
        /// <returns></returns>
        private string GetReceiptText(TransactionPaymentsDTO trxPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO, bool IsMerchantCopy, XElement printData)
        {
            log.LogMethodEntry(trxPaymentsDTO, ccTransactionsPGWDTO, IsMerchantCopy);
            try
            {
                var receipt = new System.Text.StringBuilder();


                string[] addressArray = utilities.ParafaitEnv.SiteAddress.Split(',');
                string receiptText = "";
                receiptText += AllignText(utilities.ParafaitEnv.SiteName, RecieptPrintAlignment.Center);
                if (addressArray != null && addressArray.Length > 0)
                {
                    for (int i = 0; i < addressArray.Length; i++)
                    {
                        receiptText += Environment.NewLine + AllignText(addressArray[i] + ((i != addressArray.Length - 1) ? "," : ""), RecieptPrintAlignment.Center);
                    }
                }
                //receipt.AppendLine(receiptText);

                foreach (XElement element in printData.Descendants())
                {
                    if (element.Value.Contains("Cardholder Signature") && !IsMerchantCopy)
                    {
                        receiptText = receiptText.Remove(receiptText.LastIndexOf(Environment.NewLine));
                        continue;
                    }
                    if (element.Value.Contains("INVOICE") && !IsMerchantCopy)
                    {
                        element.Value = "INVOICE : @invoiceNo";
                    }
                    receiptText += Environment.NewLine;
                    receiptText += AllignText(element.Value.TrimStart('.'), RecieptPrintAlignment.Left);
                    //receipt.AppendLine(AllignText(element.Value.TrimStart('.'), RecieptPrintAlignment.Left));
                }

                if (IsMerchantCopy)
                {
                    if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.RecordNo) && ccTransactionsPGWDTO.RecordNo.Equals("A")))
                    {
                        //receipt.AppendLine(AllignText("_______________________", RecieptPrintAlignment.Center));
                        //receipt.AppendLine(AllignText(utilities.MessageUtils.getMessage("Signature"), RecieptPrintAlignment.Center));

                        if (ccTransactionsPGWDTO.TranCode.Equals(TransactionType.AUTHORIZATION.ToString()))
                        {
                            LookupValuesDTO lookupValuesDTO = GetLookupValues("ADDITIONAL_PRINT_FIELDS", "@SuggestiveTipText");
                            if (!string.IsNullOrEmpty(lookupValuesDTO.Description))
                            {
                                receiptText += Environment.NewLine;
                                receiptText += AllignText(lookupValuesDTO.Description, RecieptPrintAlignment.Center);
                                //receipt.AppendLine(AllignText(lookupValuesDTO.Description, RecieptPrintAlignment.Center));
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
                                        receiptText += Environment.NewLine;
                                        receiptText += AllignText(line, RecieptPrintAlignment.Center);
                                        //receipt.AppendLine(AllignText(line, RecieptPrintAlignment.Center));
                                    }
                                }
                            }
                        }
                        receiptText += Environment.NewLine;
                        receiptText += AllignText("**" + utilities.MessageUtils.getMessage("Merchant Copy") + "**", RecieptPrintAlignment.Center);
                        //receipt.AppendLine(AllignText("**" + utilities.MessageUtils.getMessage("Merchant Copy") + "**", RecieptPrintAlignment.Center));
                    }

                }
                else
                {
                    receiptText += Environment.NewLine;
                    receiptText += AllignText("**" + utilities.MessageUtils.getMessage("Cardholder Copy") + "**", RecieptPrintAlignment.Center);
                    //receipt.AppendLine(AllignText("**" + utilities.MessageUtils.getMessage("Cardholder Copy") + "**", RecieptPrintAlignment.Center));
                }

                receipt.AppendLine(AllignText(" " + utilities.MessageUtils.getMessage("Thank You"), RecieptPrintAlignment.Center));
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

                log.LogMethodExit(receipt);
                return receipt.ToString();
            }
            catch (Exception ex)
            {
                log.Fatal("GetReceiptText() failed to print receipt exception:" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="align"></param>
        /// <returns></returns>
        public static string AllignText(string text, RecieptPrintAlignment align)
        {
            log.LogMethodEntry(text, align);

            int pageWidth = 40;
            string res;
            if (align.Equals(RecieptPrintAlignment.Right))
            {
                string returnValueNew = text.PadLeft(pageWidth, ' ');
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            else if (align.Equals(RecieptPrintAlignment.Center))
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
        public void PadReset()
        {
            log.LogMethodEntry();
            try
            {
                NetEpayRequestDTO netEpayRequestDTO = new NetEpayRequestDTO(hostIp, hostIpPort, merchantId, sourceDevice, comPort, pinPadIp, pinPadIpPort);
                netEpayRequestDTO.TranCode = "EMVPadReset";
                //netEpayRequestDTO.DisplayTextHandle = "00101484";

                string resultString = string.Empty;
                TStream tStream = new TStream(netEpayRequestDTO, null);

                XmlSerializer requestXml = new XmlSerializer(tStream.GetType());
                MemoryStream requestStream = new MemoryStream();

                requestXml.Serialize(requestStream, tStream);
                requestStream.Position = 0;
                using (StreamReader reader = new StreamReader(requestStream))
                {
                    String requestString = reader.ReadToEnd();
                    log.LogVariableState("requestString", requestString);
                    resultString = dsiEMVX.ProcessTransaction(requestString);
                    log.LogVariableState("resultString", resultString);
                }
                RStream rStream = new RStream();
                XmlSerializer serializer = new XmlSerializer(rStream.GetType());
                using (StringReader sr = new StringReader(resultString))
                {
                    rStream = (RStream)serializer.Deserialize(sr);
                }
                if (!rStream.CmdResponse.CmdStatus.Equals("Approved"))
                {
                    throw new Exception(rStream.CmdResponse.TextResponse);
                }
            }
            catch (Exception ex)
            {
                log.Error("Pad Reset Error", ex);
            }

            log.LogMethodExit();
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
    }
}
