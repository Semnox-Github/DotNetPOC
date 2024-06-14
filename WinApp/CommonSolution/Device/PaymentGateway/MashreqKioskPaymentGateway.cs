/********************************************************************************************
 * Project Name - Mashreq Payment Gateway
 * Description  - MashreqKisok class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.140.0     03-Sep-2021      Jinto Thomas    Created              
 *2.150.1     22-Feb-2023      Guru S A        Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using Semnox.Core.Utilities;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class TransactionDetails
    {
        public string amount { get; set; }

        public string orderId { get; set; }

    }
    //class MashreqRefund
    //{
    //    public string username;
    //    public string orderId;

    //    public MashreqRefund()
    //    {

    //    }
    //}
    class MashreqOrder
    {
        public string username;
        public string orderId;
        public string txnId;
        public TransactionDetails transaction { get; set; }

        public MashreqOrder()
        {

        }
    }
    class MashreqKioskPaymentGateway: PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string userName;
        string password;
        string hostUrl;
        string requeryUrl;
        string voidUrl;
        string deviceUrl;
        string currencyCode;
        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();

        public enum RecieptPrintAlignment
        {
            Left,
            Right,
            Center
        }

        public MashreqKioskPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
          : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);

            userName = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_HOST_USERNAME");
            password = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_HOST_PASSWORD");
            hostUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_HOST_URL");
            requeryUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_REQUERY_URL");
            voidUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_VOID_URL");
            deviceUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_DEVICE_URL");
            currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");

            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception(utilities.MessageUtils.getMessage("userName is not set"));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception(utilities.MessageUtils.getMessage("password is not set"));
            }
            if (string.IsNullOrEmpty(hostUrl))
            {
                throw new Exception(utilities.MessageUtils.getMessage("hostUrl is not set"));
            }
            if (string.IsNullOrEmpty(requeryUrl))
            {
                throw new Exception(utilities.MessageUtils.getMessage("requeryUrl is not set"));
            }
            if (string.IsNullOrEmpty(voidUrl))
            {
                throw new Exception(utilities.MessageUtils.getMessage("voidUrl is not set"));
            }
            if (string.IsNullOrEmpty(deviceUrl))
            {
                throw new Exception(utilities.MessageUtils.getMessage("deviceUrl is not set"));
            }
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new Exception(utilities.MessageUtils.getMessage("serverUrl is not set"));
            }

            log.Debug("userName: " + userName);
            log.Debug("deviceUrl: " + deviceUrl);
            log.Debug("password: " + password);
            log.Debug("requeryUrl: " + requeryUrl);
            log.Debug("hostUrl: " + hostUrl);
            log.Debug("password: " + voidUrl);


        }

        public override void Initialize()
        {
            log.LogMethodEntry();
            Login();
            CheckLastTransactionStatus();
            log.LogMethodExit();
        }

        private async Task<string> ProcessEnquiry(string requestString)
        {
            log.LogMethodEntry();
            string response = await Connect(requeryUrl, requestString, statusDisplayUi);
            log.LogMethodExit();
            return response;
        }

        public override void SendLastTransactionStatusCheckRequest(CCRequestPGWDTO cCRequestPGWDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);

            try
            {
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();

                log.Debug(utilities.MessageUtils.getMessage("Checking the transaction status" + ((cCRequestPGWDTO != null) ? " of TrxId:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")));

                CCTransactionsPGWDTO ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();

                if (cCRequestPGWDTO != null)
                {
                    log.Debug("cCRequestPGWDTO is not null");

                    var orderRequest = new MashreqOrder();
                    orderRequest.username = userName;
                    orderRequest.orderId = cCRequestPGWDTO.RequestID.ToString();
                    string serializedOrderRequest = JsonConvert.SerializeObject(orderRequest);
                    string response;

                    using (NoSynchronizationContextScope.Enter())
                    {
                        Task<string> responseTask = ProcessEnquiry(serializedOrderRequest);
                        responseTask.Wait();
                        response = responseTask.Result;
                    }
                    dynamic responseData = JsonConvert.DeserializeObject(response);

                    if (responseData["status"] != null && responseData["status"] == "AUTHORIZED")
                    {
                        ccTransactionsPGWDTOResponse.RefNo = responseData["txnId"];
                        ccTransactionsPGWDTOResponse.Authorize = responseData["amount"];
                        ccTransactionsPGWDTOResponse.AuthCode = responseData["authCode"];
                        ccTransactionsPGWDTOResponse.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTOResponse.DSIXReturnCode = responseData["mid"];
                        ccTransactionsPGWDTOResponse.TokenID = responseData["tid"];
                        ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                        ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                        ccTransactionsPGWDTOResponse.CardType = responseData["paymentCardType"];
                        ccTransactionsPGWDTOResponse.RecordNo = "A";
                        ccTransactionsPGWDTOResponse.TextResponse = "APPROVED";
                        ccTransactionsPGWDTOResponse.CaptureStatus = responseData["cardTxnTypeDesc"];

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
                            transactionPaymentsDTO = RefundAmount(transactionPaymentsDTO);
                        }
                    }
                    else
                    {
                        log.Debug("There is no transaction exists");
                        log.Error("Last transaction status is not available.");
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Connect(string uri, string data, IDisplayStatusUI statusDisplayUi)
        {
            log.LogMethodEntry(uri, data, statusDisplayUi);
            Thread.Sleep(1000); //wait for a sec, so server starts and ready to accept connection..
            byte[] buffer = new byte[1024];
            byte[] buffer1 = new byte[4096];
            string resultData = string.Empty;
            bool responseStart = false;
            ClientWebSocket webSocket = null;
           
            
            try
            {

                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                while (webSocket.State == WebSocketState.Open)
                {
                    Array.Clear(buffer, 0, buffer.Length);

                    buffer = Encoding.UTF8.GetBytes(data);
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    await Task.Delay(1000);
                    while (true)
                    {
                        Array.Clear(buffer1, 0, buffer1.Length);
                        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer1), CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                            break;
                        }
                        else
                        {
                            string response = Encoding.UTF8.GetString(buffer1).TrimEnd('\0');
                            if (response.StartsWith("{"))
                            {
                                responseStart = true;
                                resultData += response;
                            }
                            else if (responseStart)
                            {
                                resultData += response;
                            }
                            else
                            {
                                statusDisplayUi.DisplayText(response.Remove(0,2));
                            }

                        }
                    }
                    break;
                }
                log.LogMethodExit(resultData);
                return resultData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();             
            }
           
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
            statusDisplayUi.CloseStatusWindow();
            //dsiEMVX.CancelRequest();
            log.LogMethodExit(null);
        }


        private void Login()
        {
            log.LogMethodEntry();
            try
            {
                string requestString = "{\"username\":\"" + userName + "\"}";
                string response;
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "Connecting Device", "Mashreq Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<string> responseTask = Connect(deviceUrl, requestString, statusDisplayUi);
                    responseTask.Wait();
                    response = responseTask.Result;
                }
                dynamic responseData = JsonConvert.DeserializeObject(response);
                log.Debug("login response: " + responseData);
                if (responseData["success"] != "true")
                {
                    throw new Exception(utilities.MessageUtils.getMessage("D200 device login failed."));
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("Error while connecting device: "+ex.Message);
                statusDisplayUi.DisplayText(ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }
            
            
        }

        private async Task<string> ProcessPayment(string requestString)
        {
            log.LogMethodEntry(requestString);
            string response = await Connect(hostUrl, requestString, statusDisplayUi);
            log.LogMethodExit(response);
            return response;
        }

        private void checkAndVoidTransaction(string orderId)
        {

        }
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
                        
            try
            {
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Mashreq Payment Gateway");
                statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                var orderRequest = new MashreqOrder();
                orderRequest.username = userName;
                TransactionDetails transaction = new TransactionDetails();
                transaction.amount = transactionPaymentsDTO.Amount.ToString();
                transaction.orderId = ccRequestPGWDTO.RequestID.ToString();
                orderRequest.transaction = transaction;
                string serializedOrderRequest = JsonConvert.SerializeObject(orderRequest);
                string response = string.Empty;
                
                thr.Start();
                using (NoSynchronizationContextScope.Enter())
                {
                    log.Debug("Enter NoSynchronizationContextScope");
                    Task<string> responseTask = ProcessPayment(serializedOrderRequest);
                    responseTask.Wait();
                    response = responseTask.Result;
                    log.Debug("Exit NoSynchronizationContextScope");
                }
                dynamic responseData = JsonConvert.DeserializeObject(response);
                log.Debug("responseData: " + responseData);
                if (responseData == null)
                {
                    if (statusDisplayUi != null)
                        statusDisplayUi.CloseStatusWindow();
                    if (showMessageDelegate != null)
                    {
                        showMessageDelegate("Device Communicataion Lost!!! Please contact staff", "Mashreq Payment Gateway", MessageBoxButtons.OK);
                    }
                    throw new Exception("Devive Communicataion Lost");
                }

                

                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                if (responseData["status"]!=null && responseData["status"]== "AUTHORIZED")
                {
                    string aid = string.Empty;
                    string tc = string.Empty;
                    string tvr = string.Empty;
                    string tsi = string.Empty;
                    //Dictionary<string, string> bankTransactionsCode = GetBankTransactionCodes(responseData["customerReceiptUrl"].ToString());
                    //if (bankTransactionsCode.Count > 0)
                    //{
                    //    aid = bankTransactionsCode["AID"] != null ? bankTransactionsCode["AID"] : "";
                    //    tc = bankTransactionsCode["TC"] != null ? bankTransactionsCode["TC"] : "";
                    //    tvr = bankTransactionsCode["TVR"] != null ? bankTransactionsCode["TVR"] : "";
                    //    tsi = bankTransactionsCode["TSI"] != null ? bankTransactionsCode["TSI"] : "";
                    //}                  

                    ccTransactionsPGWDTO.RefNo = responseData["txnId"];
                    ccTransactionsPGWDTO.Authorize = responseData["amount"];
                    ccTransactionsPGWDTO.AuthCode = responseData["authCode"];
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.DSIXReturnCode = responseData["mid"];
                    ccTransactionsPGWDTO.TokenID = responseData["tid"];
                    ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                    ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    ccTransactionsPGWDTO.CardType = responseData["paymentCardBrand"]+" "+ responseData["paymentCardType"];  
                    ccTransactionsPGWDTO.RecordNo = "A";
                    ccTransactionsPGWDTO.TextResponse = "APPROVED";
                    ccTransactionsPGWDTO.CaptureStatus = responseData["cardTxnTypeDesc"];
                    ccTransactionsPGWDTO.AcqRefData = responseData["rrNumber"];
                    ccTransactionsPGWDTO.UserTraceData = responseData["deviceSerial"];
                    //if (bankTransactionsCode.Count > 0)
                    //{
                    //    ccTransactionsPGWDTO.AcqRefData = "AID:" + aid + "|" + "TC:" + tc + "|" + "TVR:" + tvr + "|" + "TSI:" + tsi;
                    //}
                    transactionPaymentsDTO.CreditCardNumber = responseData["formattedPan"];
                    transactionPaymentsDTO.CreditCardName = responseData["paymentCardBrand"];
                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;

                    SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();
                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                }
                else
                {
                    string message = responseData["errorMessage"] != null ? responseData["errorMessage"] : responseData["msg"];
                    log.Error(message);
                    throw new Exception(message);
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                statusDisplayUi.DisplayText(ex.Message);
                log.Error("Error occured while making payment", ex);
                log.LogMethodExit(null, "Throwing Payment Gateway Exception-" + ex.Message);
                throw new PaymentGatewayException(ex.Message);
            }
            finally
            {
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }
            
            
        }

        private async Task<string> ProcessRefund(string requestString)
        {
            log.LogMethodEntry();
            string response = await Connect(voidUrl, requestString, statusDisplayUi);
            log.LogMethodExit();
            return response;
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(4202, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Mashreq Payment Gateway");
                statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                statusDisplayUi.EnableCancelButton(true);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                if (transactionPaymentsDTO != null)
                {
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;

                    var orderRequest = new MashreqOrder();
                    orderRequest.username = userName;
                    orderRequest.orderId = ccOrigTransactionsPGWDTO.InvoiceNo.ToString();
                    orderRequest.txnId = ccOrigTransactionsPGWDTO.RefNo;
                    string serializedOrderRequest = JsonConvert.SerializeObject(orderRequest);
                    string response;
                    thr.Start();                  
                    using (NoSynchronizationContextScope.Enter())
                    {
                        Task<string> responseTask = ProcessRefund(serializedOrderRequest);
                        responseTask.Wait();
                        response = responseTask.Result;
                    }
                    dynamic responseData = JsonConvert.DeserializeObject(response);

                    if (responseData["status"] != null && responseData["status"] == "VOIDED")
                    {
                        CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTO.RefNo = responseData["txnId"];
                        ccTransactionsPGWDTO.Authorize = responseData["amount"];
                        ccTransactionsPGWDTO.AuthCode = responseData["authCode"];
                        ccTransactionsPGWDTO.DSIXReturnCode = responseData["mid"];
                        ccTransactionsPGWDTO.TokenID = responseData["tid"];
                        ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                        ccTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                        ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                        ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        ccTransactionsPGWDTO.RecordNo = "A";
                        ccTransactionsPGWDTO.TextResponse = "APPROVED";
                        ccTransactionsPGWDTO.CaptureStatus = ccOrigTransactionsPGWDTO.CaptureStatus;
                        ccTransactionsPGWDTO.AcqRefData = ccOrigTransactionsPGWDTO.AcqRefData;
                        ccTransactionsPGWDTO.AcctNo = ccOrigTransactionsPGWDTO.AcctNo;
                        ccTransactionsPGWDTO.UserTraceData = ccOrigTransactionsPGWDTO.UserTraceData;

                        SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);

                        ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        log.Debug("Refund Success");
                    }
                    else
                    {
                        log.Debug("Refund Failed. Requested trade has been closed" + responseData["errorMessage"]);
                        throw new Exception(responseData["errorMessage"]);
                    }
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while making refund", ex);
                log.LogMethodExit(null, "Throwing Payment Gateway Exception-" + ex.Message);
                throw new PaymentGatewayException(ex.Message);
            }
            finally
            {
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }


            
        }

        private Dictionary<string, string> GetBankTransactionCodes(string recieptUrl)
        {
            log.LogMethodEntry();
            Dictionary<string, string> bankTransactionsCode = new Dictionary<string, string>();
            var line = "";
            string found = "";
            var webRequest = WebRequest.Create(recieptUrl);

            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {

                while ((line = reader.ReadLine()) != null)
                {
                    
                    line = Regex.Replace(line, "<.*?>|[^0-9a-zA-Z:]+", String.Empty);
                    if (line.Contains("AID:"))
                    {
                        found = line.Substring(line.IndexOf(":") + 1);
                        bankTransactionsCode.Add("AID", found);
                    }
                    else if (line.Contains("TC:"))
                    {
                        found = line.Substring(line.IndexOf(":") + 1);
                        bankTransactionsCode.Add("TC", found);
                    }
                    else if (line.Contains("TVR:"))
                    {
                        found = line.Substring(line.IndexOf(":") + 1);
                        bankTransactionsCode.Add("TVR", found);
                    }
                    else if (line.Contains("TSI:"))
                    {
                        found = line.Substring(line.IndexOf(":") + 1);
                        bankTransactionsCode.Add("TSI", found);
                    }

                }

            }

            log.LogMethodExit();
            return bankTransactionsCode;
        }

        private void SendPrintReceiptRequest(TransactionPaymentsDTO transactionPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO)
        {
            if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
            {
                transactionPaymentsDTO.Memo = GetReceiptText(transactionPaymentsDTO, ccTransactionsPGWDTO, false);
            }
            if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y")
            {
                transactionPaymentsDTO.Memo += GetReceiptText(transactionPaymentsDTO, ccTransactionsPGWDTO, true);
            }
        }

        private string GetReceiptText(TransactionPaymentsDTO trxPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO, bool IsMerchantCopy)
        {
            log.LogMethodEntry(trxPaymentsDTO, ccTransactionsPGWDTO, IsMerchantCopy);
            
            int whiteSpacelength = "ICC MERCHANT ID".Length;
            try
            {
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
                receiptText += Environment.NewLine;
                if (IsMerchantCopy)
                {
                    receiptText += Environment.NewLine + AllignText("***" + utilities.MessageUtils.getMessage("MERCHANT COPY") + "***", RecieptPrintAlignment.Center);
                    receiptText += Environment.NewLine + AllignText("[" + utilities.MessageUtils.getMessage("ORIGINAL") + "]", RecieptPrintAlignment.Center);
                }
                else
                {
                    receiptText += Environment.NewLine + AllignText("***" + utilities.MessageUtils.getMessage("CUSTOMER COPY") + "***", RecieptPrintAlignment.Center);
                    receiptText += Environment.NewLine + AllignText("[" + utilities.MessageUtils.getMessage("ORIGINAL") + "]", RecieptPrintAlignment.Center);
                }
                receiptText += Environment.NewLine;
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Date") + ": ".PadLeft(0) + ccTransactionsPGWDTO.TransactionDatetime.ToString("MMM dd yyyy HH:mm"), RecieptPrintAlignment.Left);
                if(ccTransactionsPGWDTO.TranCode== "SALE")
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("PURCHASE APPROVED"), RecieptPrintAlignment.Left);
                }
                else if(ccTransactionsPGWDTO.TranCode == "REFUND")
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("PURCHASE VOIDED"), RecieptPrintAlignment.Left);
                }

                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("CARD NO.") + ": ".PadLeft(10) + ccTransactionsPGWDTO.AcctNo, RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("AMOUNT ICC") + ": ".PadLeft(8) + currencyCode + " " + ccTransactionsPGWDTO.Authorize, RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("ORDER NO") + ": ".PadLeft(10) + "@invoiceNo", RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("TRANSACTION ID") + ": ".PadLeft(4) + ccTransactionsPGWDTO.RefNo, RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("ICC MERCHANT ID") + ": ".PadLeft(1) + ccTransactionsPGWDTO.DSIXReturnCode, RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("TERMINAL ID") + ": ".PadLeft(7) + ccTransactionsPGWDTO.TokenID, RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("DEVICE SERIAL") + ": ".PadLeft(5) + ccTransactionsPGWDTO.TokenID, RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("AUTH") + ": ".PadLeft(14) + ccTransactionsPGWDTO.AuthCode, RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("RRNUMBER") + ": ".PadLeft(10) + ccTransactionsPGWDTO.AcqRefData, RecieptPrintAlignment.Left);
                //if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.AcqRefData))
                //{
                //    string[] codes = ccTransactionsPGWDTO.AcqRefData.Split('|');

                //    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("AID") + ": ".PadLeft(28) + codes[0].Substring(codes[0].IndexOf(":") + 1), RecieptPrintAlignment.Left);
                //    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("TC") + ": ".PadLeft(29) + codes[1].Substring(codes[1].IndexOf(":") + 1), RecieptPrintAlignment.Left);
                //    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("TVR") + ": ".PadLeft(27) + codes[2].Substring(codes[2].IndexOf(":") + 1), RecieptPrintAlignment.Left);
                //    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("TSI") + ": ".PadLeft(29) + codes[3].Substring(codes[3].IndexOf(":") + 1), RecieptPrintAlignment.Left);
                //}
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Appln Lbl") + ": ".PadLeft(9) + ccTransactionsPGWDTO.CardType, RecieptPrintAlignment.Left);

                if (!IsMerchantCopy && ccTransactionsPGWDTO.TranCode != "REFUND")
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText("_______________________", RecieptPrintAlignment.Center);
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("I AGREE TO PAY AS PER THE CARD ISSUER AGREEMENT AND RECEIVE CHARGE SLIP BY ELECTRONIC MEANS."), RecieptPrintAlignment.Center);
                    receiptText += Environment.NewLine + AllignText("_______________________", RecieptPrintAlignment.Center);
                }
                

                receiptText += Environment.NewLine + AllignText("PLEASE KEEP THIS COPY FOR YOUR RECORD", RecieptPrintAlignment.Center);
                receiptText += Environment.NewLine;
                receiptText += AllignText(" " + utilities.MessageUtils.getMessage("Thank You"), RecieptPrintAlignment.Center);

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
                    Print(receiptText);
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

        public override void PrintCCReceipt(List<TransactionPaymentsDTO> transactionPaymentDTOList)
        {
            log.LogMethodEntry(transactionPaymentDTOList);
            CCTransactionsPGWBL cCTransactionsPGWBL = null;
            foreach (TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentDTOList)
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.CCResponseId != -1)
                {
                    cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    {
                        if (cCTransactionsPGWBL.CCTransactionsPGWDTO != null)
                        {
                            log.LogVariableState("CCTransactionsPGWDTO", cCTransactionsPGWBL.CCTransactionsPGWDTO);
                            try
                            {
                                if (!string.IsNullOrEmpty(cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy) && ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PRINT_MERCHANT_RECEIPT").Equals("Y") && !isUnattended)
                                {
                                    log.Debug("Printing merchant copy");
                                    cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy = cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy.Replace("@invoiceNo", transactionPaymentsDTO.TransactionId.ToString());
                                    if (Print(cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy))
                                    {
                                        log.Debug("Printing merchant copy is completed");
                                        cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy = null;
                                    }
                                }
                                if (!string.IsNullOrEmpty(cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy) && ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PRINT_CUSTOMER_RECEIPT").Equals("Y"))
                                {
                                    log.Debug("Printing customer copy");
                                    cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy = cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy.Replace("@invoiceNo", transactionPaymentsDTO.TransactionId.ToString());
                                    if (Print(cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy))
                                    {
                                        log.Debug("Printing customer copy is completed");
                                        cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy = null;
                                    }
                                }
                                log.LogVariableState("Saving CCTransactionsPGWDTO", cCTransactionsPGWBL.CCTransactionsPGWDTO);
                                cCTransactionsPGWBL.Save();
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error in printing.", ex);
                            }

                        }
                    }
                    log.LogMethodExit();
                }
            }
        }

        private bool Print(string printText)
        {
            log.LogMethodEntry(printText);
            bool status = false;
            try
            {
                using (System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument())
                {
                    pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize();//("Custom", 300, 700);
                    pd.PrintPage += (sender, e) =>
                    {
                        Font f = new Font("Courier New", 8);
                        e.Graphics.DrawString(printText, f, new SolidBrush(Color.Black), new RectangleF(0, 0, pd.DefaultPageSettings.PrintableArea.Width, pd.DefaultPageSettings.PrintableArea.Height));
                    };
                    pd.Print();
                }
                status = true;
            }
            catch (Exception ex)
            {
                utilities.EventLog.logEvent("PaymentGateway", 'I', "Receipt print failed.", printText, this.GetType().Name, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                log.Error("Print receipt failed." + Environment.NewLine + printText, ex);
                status = false;
            }
            log.LogMethodExit(status);
            return status;
        }


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
    }
}
