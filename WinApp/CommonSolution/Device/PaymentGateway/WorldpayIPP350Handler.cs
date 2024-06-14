using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class WorldpayIPP350Handler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IPEndPoint remoteEP;
        IPEndPoint remoteEPReceipt;
        IPAddress ipAddress;
        public delegate void PrintReceipt(string receipt, bool isMerchantCopy);
        public PrintReceipt PrintCCReceipt;
        private bool isUnattended;
        Response response = new Response();
        WorldpayResponse worldpayResponse;
        Socket socketReceipt;
        Thread receiptThread;
        Socket socketTransaction;
        private int receiptTimeout;
        bool isReceiptThreadRunning;
        class Response
        {
            public byte[] transactionResponse = new byte[1024];
            public byte[] transactionReceipt = new byte[1024];
        }
        int bytesRec;
        public WorldpayIPP350Handler(string deviceIPAddress, int portnumber, int receiptPortNumber, bool isUnattended, int receiptTimeout = -1)
        {
            log.LogMethodEntry(deviceIPAddress, portnumber, receiptPortNumber, isUnattended);
            IPHostEntry host = Dns.GetHostEntry(deviceIPAddress);
            ipAddress = host.AddressList[0];
            remoteEP = new IPEndPoint(ipAddress, portnumber);
            remoteEPReceipt = new IPEndPoint(ipAddress, receiptPortNumber);
            this.isUnattended = isUnattended;
            this.receiptTimeout = receiptTimeout;
            log.LogMethodExit();
        }
        /// <summary>
        /// Process Transaction
        /// </summary>
        /// <param name="worldpayRequest"></param>
        /// <returns></returns>
        public WorldpayResponse ProcessTransaction(WorldpayRequest worldpayRequest)
        {
            log.LogMethodEntry(worldpayRequest);
            string command = "";
            string responseData = "";
            byte[] byteRequestCommand;
            response = new Response();
            worldpayResponse = new WorldpayResponse();
            log.LogVariableState("TransactionType", worldpayRequest.TransactionType.ToString());
            switch (worldpayRequest.TransactionType.ToString())
            {
              
                case "TATokenRequest": throw new Exception("This feature is not supported.");
                case "SALE":
                    command = "1=" + worldpayRequest.ReferenceNumber//reference no
                        + "\r\n2=0"//transaction type 0=sale
                        + "\r\n3=" + worldpayRequest.Amount
                       + ((worldpayRequest.EntryMode.Equals("KEYED")) ?
                          "\r\n18=1" : "")
                        + "\r\n99=0";
                    break;
                case "AUTHORIZATION":
                    command = "1=" + worldpayRequest.ReferenceNumber//reference no
                        + "\r\n2=1"//transaction type 1=Authorization
                        + "\r\n3=" + worldpayRequest.Amount
                        + ((worldpayRequest.EntryMode.Equals("KEYED")) ?
                          "\r\n18=1" : "")//1=Keyed
                        + "\r\n99=0";
                    break;
                case "CAPTURE":
                    command = "1=" + worldpayRequest.ReferenceNumber//reference no
                        + "\r\n2=2"//transaction type 2=Pre Sales Completion
                        + "\r\n3=" + worldpayRequest.Amount
                        + "\r\n5=" + worldpayRequest.MaskedCardNumber
                        + "\r\n6=" + worldpayRequest.ExpiryDate
                        + "\r\n13=" + worldpayRequest.OrgTransactionReference
                        + "\r\n99=0";
                    break;
                case "REFUND":
                    command = "1=" + worldpayRequest.ReferenceNumber//reference no
                        + ((string.IsNullOrEmpty(worldpayRequest.TokenId)) ? "\r\n2=20" : "\r\n2=13")//transaction type 20=refund, 13 refund with token
                        + "\r\n3=" + worldpayRequest.Amount
                        + ((worldpayRequest.EntryMode.Equals("KEYED") && string.IsNullOrEmpty(worldpayRequest.TokenId)) ?
                          "\r\n18=1" : "")//1=Keyed
                          + ((!string.IsNullOrEmpty(worldpayRequest.TokenId)) ? "\r\n19=" + worldpayRequest.TokenId : "")
                           + ((!string.IsNullOrEmpty(worldpayRequest.TokenId)) ? "\r\n13=" + worldpayRequest.OrgTransactionReference : "")
                        + "\r\n99=0";
                    break;
                case "VOID"://Only authorized transactions can be canceled 
                    command = "1=" + worldpayRequest.ReferenceNumber//reference no of original transaction
                        + "\r\n2=3"//transaction type 3=Cancel transaction(Void)
                        + "\r\n5=" + worldpayRequest.MaskedCardNumber
                        + "\r\n6=" + worldpayRequest.ExpiryDate
                        + "\r\n13=" + worldpayRequest.OrgTransactionReference
                        + "\r\n99=0";
                    break;
            }
            if (!string.IsNullOrEmpty(command))
            {
                log.LogVariableState("command", command);
                byteRequestCommand = CreateByteCommand(command);
                isReceiptThreadRunning = false;
                SendCommand(byteRequestCommand);
                if (response != null)
                {
                    responseData = ConvertResponse(response.transactionResponse);
                    log.LogVariableState("Response Data", responseData);
                    worldpayResponse.ccTransactionsPGWDTO = ParseResponse(responseData);
                    worldpayResponse.ccTransactionsPGWDTO.TranCode = worldpayRequest.TransactionType.ToString();
                    worldpayResponse.ccTransactionsPGWDTO.CustomerCopy = worldpayResponse.CardHolderReceipt;
                    worldpayResponse.ccTransactionsPGWDTO.MerchantCopy = worldpayResponse.MerchantReceipt;
                    if (worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("9"))
                    {
                        isReceiptThreadRunning = false;
                        log.Debug("Transaction returned cancelled status so exiting the process transaction with Receipt socket running");
                        // Release the socket.
                        if (socketReceipt != null && socketReceipt.Connected)
                        {
                            log.Debug("Closing receipt socket due to cancelled status");
                            socketReceipt.Shutdown(SocketShutdown.Both);
                            socketReceipt.Close();
                        }
                    }
                }
            }
            else
            {
                log.LogMethodExit(null, "Invalid command.");
                throw new Exception("Invalid command.");
            }
            while (isReceiptThreadRunning)
            {
                log.Debug("Waiting for receipt generation process to end...");
                Thread.Sleep(50);
            }
            log.LogMethodExit();
            return worldpayResponse;
        }
        private void GetCCTransactionReceipt()
        {
            log.LogMethodEntry();
            isReceiptThreadRunning = true;
            int mIndex, cIndex;
            string receipt = "";
            int receiptReadExitCounter = 5;
            string responseData;
            bool isMerchantCopyPrinted = isUnattended;
            bool isCustomerCopyPrinted = false;
            socketReceipt = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socketReceipt.Connect(remoteEPReceipt);
                socketReceipt.ReceiveTimeout = receiptTimeout;
                while (!isMerchantCopyPrinted || !isCustomerCopyPrinted)
                {
                    bytesRec = socketReceipt.Receive(response.transactionReceipt);
                    log.LogVariableState("transactionReceipt", response.transactionReceipt);
                    responseData = ConvertResponse(response.transactionReceipt);
                    log.LogVariableState("Reponse Data", responseData);
                    if (!string.IsNullOrEmpty(responseData))
                    {
                        mIndex = responseData.IndexOf("MERCHANT:");
                        cIndex = responseData.IndexOf("CUSTOMER:");
                        //the response data will be having both the receipt. Normally merchant receipt will come first and customer receipt will come next. In case the sequence changes then the following if else condition is required.
                        //The customer receipt will start with key word "CUSTOMER:" and merchant receipt will start with key word "MERCHANT:". Bellow we are finding the index of both the receipt and removing the keywords.
                        if (mIndex > -1 && cIndex > -1)
                        {
                            if (mIndex < cIndex)
                            {
                                receipt = responseData.Substring(mIndex, cIndex);
                                receipt = receipt.Replace("MERCHANT:", "");
                                worldpayResponse.MerchantReceipt = receipt;
                                receipt = responseData.Substring(cIndex);
                                receipt = receipt.Replace("CUSTOMER:", "");
                                worldpayResponse.CardHolderReceipt = receipt;
                            }
                            else if (mIndex > cIndex)
                            {
                                receipt = responseData.Substring(cIndex, mIndex);
                                receipt = receipt.Replace("CUSTOMER:", "");
                                worldpayResponse.CardHolderReceipt = receipt;
                                receipt = responseData.Substring(mIndex);
                                receipt = receipt.Replace("MERCHANT:", "");
                                worldpayResponse.MerchantReceipt = receipt;
                            }
                            isMerchantCopyPrinted = isCustomerCopyPrinted = true;
                            PrintCCReceipt(worldpayResponse.CardHolderReceipt, false);
                            PrintCCReceipt(worldpayResponse.MerchantReceipt, true);
                        }
                        else if (mIndex > -1 && cIndex == -1)
                        {
                            receipt = responseData.Substring(mIndex);
                            receipt = receipt.Replace("MERCHANT:", "");
                            worldpayResponse.MerchantReceipt = receipt;
                            PrintCCReceipt(worldpayResponse.MerchantReceipt, true);
                            isMerchantCopyPrinted = true;
                        }
                        else if (mIndex == -1 && cIndex > -1)
                        {
                            receipt = responseData.Substring(cIndex);
                            receipt = receipt.Replace("CUSTOMER:", "");
                            worldpayResponse.CardHolderReceipt = receipt;
                            isCustomerCopyPrinted = true;
                            PrintCCReceipt(worldpayResponse.CardHolderReceipt, false);
                        }

                    }
                    receiptReadExitCounter--;
                    if (receiptReadExitCounter <= 0)
                    {
                        isCustomerCopyPrinted = true;
                        isMerchantCopyPrinted = true;
                        log.Info("Receipt print process is not successful. Breaking...");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Exception occurred while generating the receipt:" + ex.ToString());
            }
            finally
            {
                // Release the socket.
                if (socketReceipt != null && socketReceipt.Connected)
                {
                    log.Debug("Closing receipt socket");
                    socketReceipt.Shutdown(SocketShutdown.Both);
                    socketReceipt.Close();
                }
                isReceiptThreadRunning = false;
            }
        }
        private byte[] CreateByteCommand(string command)
        {
            log.LogMethodEntry(command);
            byte[] request;
            request = Encoding.ASCII.GetBytes(command);
            log.LogMethodExit(request);
            return request;
        }
        private void SendCommand(byte[] request)
        {
            log.LogMethodEntry(request);
            socketTransaction = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socketTransaction.Connect(remoteEP);
                // Send the data through the socket. 
                log.LogVariableState("request", request);
                if (receiptThread == null)
                {
                    receiptThread = new Thread(GetCCTransactionReceipt);
                    receiptThread.Start();
                    log.LogVariableState("Receipt Thread started", receiptThread);
                }
                socketTransaction.SendTimeout = 10000;//10 second time out for sending command
                socketTransaction.ReceiveTimeout = receiptTimeout;//FIRST_DATA_CLIENT_TIMEOUT second time out for receiving response
                int bytesSent = socketTransaction.Send(request);
                // Receive the response from the remote device.
                bytesRec = socketTransaction.Receive(response.transactionResponse);
                log.LogVariableState("response", response.transactionResponse);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Exception:" + ex.Message, ex);
                if (socketReceipt != null && socketReceipt.Connected)
                {
                    log.Debug("Closing receipt socket");
                    socketReceipt.Shutdown(SocketShutdown.Both);
                    socketReceipt.Close();
                }
                isReceiptThreadRunning = false;
                throw ex;
            }
            finally
            {
                // Release the socket.  
                if (socketTransaction != null && socketTransaction.Connected)
                {
                    log.Debug("Closing transaction socket");
                    socketTransaction.Shutdown(SocketShutdown.Both);
                    socketTransaction.Close();
                }                
            }
        }
        public string ConvertResponse(byte[] response)
        {
            log.LogMethodEntry();
            string responseString;
            if (bytesRec > 0)
            {
                responseString = Encoding.ASCII.GetString(response, 0, bytesRec);
                log.LogVariableState("responseString", responseString);
            }
            else
            {
                throw (new Exception("Invalid response."));
            }
            log.LogMethodExit();
            return responseString;
        }
        public CCTransactionsPGWDTO ParseResponse(string response)
        {
            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
            log.LogMethodEntry();
            string[] splitChar = { "\n" };
            string[] responseArray = response.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            string[] responseValueField;
            foreach (string reponseField in responseArray)
            {
                responseValueField = reponseField.Split('=');
                log.Debug("ResponseValueField Length : "+ responseValueField != null ? (responseValueField[0] + ":" + responseValueField.Length.ToString()) : null);
                if (responseValueField != null && responseValueField.Length > 1)
                {
                    switch (responseValueField[0])
                {
                    case "1":
                        ccTransactionsPGWDTO.CaptureStatus = GetCaptureType(responseValueField[1]);
                        break;
                    case "2":
                        ccTransactionsPGWDTO.TranCode = responseValueField[1];
                        break;
                    case "3":
                        ccTransactionsPGWDTO.DSIXReturnCode = responseValueField[1];
                        if (!string.IsNullOrEmpty(responseValueField[1]))
                        {
                            ccTransactionsPGWDTO.TextResponse = ConvertResponseCode(Convert.ToInt32(responseValueField[1]));
                        }
                        break;
                    case "4":
                        ccTransactionsPGWDTO.AuthCode = responseValueField[1];
                        break;
                    case "5":
                        string maskedAccount = (new String('X', 12) + ((responseValueField[1].Length > 4)
                                           ? responseValueField[1].Substring(responseValueField[1].Length - 4)
                                           : responseValueField[1]));
                        ccTransactionsPGWDTO.AcctNo = maskedAccount; // responseValueField[1];
                        break;
                    case "6":
                        ccTransactionsPGWDTO.CardType = responseValueField[1];
                        break;
                    case "7": break;//EMV Card Data Element Application Effective Date which holds date from which the application may be used. The format is DDMMYYYY.
                    case "8":
                        if (ccTransactionsPGWDTO.TransactionDatetime == null)
                        {
                            ccTransactionsPGWDTO.TransactionDatetime = new DateTime();
                        }
                        if (!string.IsNullOrEmpty(responseValueField[1]))
                        {
                            DateTime dt = DateTime.ParseExact(responseValueField[1], "ddMMyyyy", CultureInfo.InvariantCulture);
                            ccTransactionsPGWDTO.TransactionDatetime = new DateTime(dt.Year, dt.Month, dt.Day, ccTransactionsPGWDTO.TransactionDatetime.Hour, ccTransactionsPGWDTO.TransactionDatetime.Minute, ccTransactionsPGWDTO.TransactionDatetime.Second);
                        }
                        break;
                    case "9":
                        if (ccTransactionsPGWDTO.TransactionDatetime == null)
                        {
                            ccTransactionsPGWDTO.TransactionDatetime = new DateTime();
                        }
                        ccTransactionsPGWDTO.TransactionDatetime = new DateTime(ccTransactionsPGWDTO.TransactionDatetime.Year,
                                                                                ccTransactionsPGWDTO.TransactionDatetime.Month,
                                                                                ccTransactionsPGWDTO.TransactionDatetime.Day,
                                                                                Convert.ToInt32(responseValueField[1].Substring(0, 2)),
                                                                                Convert.ToInt32(responseValueField[1].Substring(2, 2)),
                                                                                Convert.ToInt32(responseValueField[1].Substring(4, 2)));
                        break;
                    case "10": break;//Cardholder Name
                    case "11": break;//whole cardholder name
                    case "12": break;//Merchant Identifier.
                    case "13": ccTransactionsPGWDTO.RecordNo = responseValueField[1]; break;//Terminal Identifier.
                    case "14": break;//Card Verification Method
                    case "15": break;//Start Date, present only in case of a swiped UK Maestro/Solo card transaction.
                    case "16": break;//Total Number of Sale Counts
                    case "17": break;//Total Number of Refund Counts
                    case "18": break;//Total Sale Amount.
                    case "19": break;//Total Refund Amount.

                    case "21": break;//EFT Sequence number
                    case "22": break;//Merchant Address
                    case "23": break;//Merchant Name
                    case "25": break;//Batch number
                    case "26": break;//Referral telephone number 1
                    case "27": break;//Referral telephone number 2
                    case "28"://PGTR, Payment gateway transaction reference.
                        ccTransactionsPGWDTO.RefNo = responseValueField[1];
                        break;
                    case "29": break;//EMV Card Data Element Application Identifier (AID),
                    case "30": break;//PAN Sequence number or Issue Number. PAN Sequence number in case of an ICC transaction and Issue number in case of a Swiped UK Maestro/Solo card transaction
                    case "31": break;//Transaction Status Information (TSI), present only in case of an ICC transaction. Used for debug purpose only.
                    case "32": break;//Terminal Verification Results (TVR), present only in case of an ICC transaction. Used for debug purpose only.
                    case "33": break;//Retention reminder eg:PLEASE KEEP THIS RECEIPT FOR YOUR RECORDS
                    case "34": break;//Customer Declaration eg:PLEASE DEBIT MY ACCOUNT
                    case "35": break;//Additional Response Data, the CVV response
                    case "36"://Receipt Number
                        ccTransactionsPGWDTO.AcqRefData = responseValueField[1];
                        break;
                    case "37"://Card Expiry Date-IPC returns the encrypted expiry date in case of IPP350, iWL250 and Miura PEDs. Any valid MMYY date can be sent to IPC for cancel and pre sales completion transaction type.
                        ccTransactionsPGWDTO.ResponseOrigin = responseValueField[1];
                        break;
                    case "38"://Total Amount. Amount is returned in configured major/minor currency denomination.Total amount includes Sale Amount, Cash Back Amount(if any), Gratuity Amount(if any) and Pennies Donation Amount( if any)
                        ccTransactionsPGWDTO.Authorize = responseValueField[1];
                        break;
                    case "39": break;//Cash Back Amount.
                    case "40": //Gratuity Amount
                        ccTransactionsPGWDTO.TipAmount = responseValueField[1];
                        break;
                    case "41": break;//This field indicates Card type, If card is fuel card it returns 1 else 0.
                    case "59": break;//A field of 40 zeros(0) is returned to maintain the backward compatibility (earlier SHA1 hash value for the PAN number was returned in response
                    case "60":
                        ccTransactionsPGWDTO.ProcessData = responseValueField[1];//Card Issuer Code, this is the 3 digit WPH card issuer code. Should be used to identify the type of the card.
                        ccTransactionsPGWDTO.UserTraceData = GetCardType(responseValueField[1]);
                        break;
                    case "61":
                        ccTransactionsPGWDTO.TokenID = responseValueField[1];
                        break;
                    case "80": break;
                    case "98": break;
                    case "99": break;
                }
                }
            }
            if (ccTransactionsPGWDTO.TransactionDatetime.Equals(DateTime.MinValue))
            {
                ccTransactionsPGWDTO.TransactionDatetime = ServerDateTime.Now;
                ccTransactionsPGWDTO.InvoiceNo = "Cancelled";
            }
            log.LogMethodExit();
            return ccTransactionsPGWDTO;
        }
        private string GetCaptureType(string code)
        {
            log.LogMethodEntry();
            string captureStatus = "SWIPED";
            switch (code)
            {
                case "5":
                    captureStatus = "INSERED";
                    break;
                case "2":
                    captureStatus = "SWIPED";
                    break;
                case "1":
                    captureStatus = "KEYED";
                    break;
                case "81":
                    captureStatus = "Card holder not present";
                    break;
                case "0":
                    captureStatus = "X & Z report and transaction type Cancel";
                    break;
                case "21":
                    captureStatus = "Cash transaction";
                    break;
                case "7":
                    captureStatus = "Contactless chip transaction";
                    break;
                case "91":
                    captureStatus = "Contactless Stripe transaction";
                    break;
                case "92":
                    captureStatus = "Contactless On-Device transaction";
                    break;
            }
            log.LogMethodExit();
            return captureStatus;
        }

        private string GetCardType(string code)
        {
            log.LogMethodEntry();
            string cardType = "Unknown";
            switch (code)
            {
                case "000":
                    cardType = "NOT ISSUED";
                    break;
                case "001":
                    cardType = "DELTA";
                    break;
                case "002":
                    cardType = "UKELECTRON";
                    break;
                case "003":
                    cardType = "VISA";
                    break;
                case "004":
                    cardType = "VISA";
                    break;
                case "005":
                    cardType = "MASTER";
                    break;
                case "006":
                    cardType = "UK MAESTRO";
                    break;
                case "007":
                    cardType = "SOLO";
                    break;
                case "008":
                    cardType = "JCB";
                    break;
                case "009":
                    cardType = "MASTER";
                    break;
                case "010":
                    cardType = "VISA ATM";
                    break;
                case "011":
                    cardType = "ARVALPHH";
                    break;
                case "012":
                    cardType = "AMEX";
                    break;
                case "013":
                    cardType = "DINERS";
                    break;
                case "014":
                    cardType = "LASER";
                    break;
                case "015":
                    cardType = "DUET";
                    break;
                case "016":
                    cardType = "MASTER";
                    break;
                case "017":
                    cardType = "PLCC";
                    break;
                case "018":
                    cardType = "CLUBCARD";
                    break;
                case "019":
                    cardType = "DANKORT";
                    break;
                case "020":
                    cardType = "DISCOVER";
                    break;
                case "021":
                    cardType = "USDEBIT";
                    break;
                case "022":
                    cardType = "MASTER";
                    break;
                case "023":
                    cardType = "MASTER";
                    break;
                case "024":
                    cardType = "BANAMER";
                    break;
                case "080":
                    cardType = "FLEXCASH";
                    break;
                case "081":
                    cardType = "FLEXECASH";
                    break;
                case "090":
                    cardType = "YESPAY";
                    break;
            }
            log.LogMethodExit();
            return cardType;
        }
        private string ConvertResponseCode(int responseCode)
        {
            string response = "";
            log.LogMethodEntry(responseCode);
            switch (responseCode)
            {
                case 1:
                    response = "Approved Online";
                    break;
                case 2:
                    response = "Approved Offline";
                    break;
                case 3:
                    response = "Approved Manual(Referral)";
                    break;
                case 4:
                    response = "Declined Online";
                    break;
                case 5:
                    response = "Declined Offline";
                    break;
                case 9:
                    response = "Cancelled";
                    break;
                case 10:
                    response = "Non Fiscal Transaction performed";
                    break;
                case 16:
                    response = "Capture Card, declined online";
                    break;
                case 19:
                    response = "Transaction Aborted";
                    break;
                case 20:
                    response = "Pre sales completed";
                    break;
                case 21:
                    response = "Pre sales rejected";
                    break;
                case 22:
                    response = "Card number not matched";
                    break;
                case 23:
                    response = "Expiry date not matched";
                    break;
                case 24:
                    response = "Invalid transaction state";
                    break;
                case 25:
                    response = "Transaction not valid for requested operation";
                    break;
                case 26:
                    response = "Invalid PGTR";
                    break;
                case 27:
                    response = "Invalid Merchant";
                    break;
                case 28:
                    response = "Invalid Terminal";
                    break;
                case 29:
                    response = "Merchant status is not valid";
                    break;
                case 30:
                    response = "Invalid card number";
                    break;
                case 31:
                    response = "Expired Card";
                    break;
                case 32:
                    response = "Pre valid card";
                    break;
                case 33:
                    response = "Invalid issue number";
                    break;
                case 34:
                    response = "Invalid card expiry date";
                    break;
                case 35:
                    response = "Invalid start date";
                    break;
                case 36:
                    response = "Card not accepted";
                    break;
                case 37:
                    response = "Transaction not allowed";
                    break;
                case 38:
                    response = "Cash back not allowed";
                    break;
                case 42:
                    response = "Status Busy";
                    break;
                case 43:
                    response = "Status Not Busy";
                    break;
                case 44:
                    response = "Pinpad is not connected";
                    break;
                case 45:
                    response = "Pinpad is connected";
                    break;
                case 50:
                    response = "AVS details required";
                    break;
                default:
                    response = "Reason unknown";
                    break;
            }
            log.LogMethodExit();
            return response;
        }
    }
    internal class WorldpayResponse
    {
        private CCTransactionsPGWDTO transactionsPGWDTO;
        private string cardHolderReceipt;
        private string merchantReceipt;

        public CCTransactionsPGWDTO ccTransactionsPGWDTO { get { return transactionsPGWDTO; } set { transactionsPGWDTO = value; } }
        public string CardHolderReceipt { get { return cardHolderReceipt; } set { cardHolderReceipt = value; } }
        public string MerchantReceipt { get { return merchantReceipt; } set { merchantReceipt = value; } }
    }
    internal class WorldpayRequest
    {
        private string referenceNumber;
        private double amount;
        private PaymentGatewayTransactionType transactionType;
        private string maskedCardNumber;
        private string expiryDate;
        private string entryMode;
        private string orgTransactionReference;
        private string tokenId;

        public string ReferenceNumber { get { return referenceNumber; } set { referenceNumber = value; } }
        public double Amount { get { return amount; } set { amount = value; } }
        public PaymentGatewayTransactionType TransactionType { get { return transactionType; } set { transactionType = value; } }
        public string MaskedCardNumber { get { return maskedCardNumber; } set { maskedCardNumber = value; } }
        public string ExpiryDate { get { return expiryDate; } set { expiryDate = value; } }
        public string EntryMode { get { return entryMode; } set { entryMode = value; } }
        public string OrgTransactionReference { get { return orgTransactionReference; } set { orgTransactionReference = value; } }
        public string TokenId { get { return tokenId; } set { tokenId = value; } }
    }
}
