/******************************************************************************************************
 * Project Name - Device
 * Description  - Mashreq Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By                 Remarks          
 ******************************************************************************************************
 *2.140.3     11-Aug-2022    Prasad, Dakshakh Raj        Mashreq Payment gateway integration
 ********************************************************************************************************/
using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using sgEftInterface;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{

    internal class MashreqCommonRequestHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EftInterface _eft;
        public EFTData eftData;
        private Dictionary<string, string> IntermediateMessagesMap = new Dictionary<string, string>();
        private string isCommandTypeFound;
        private int port;
        private int mashreqTransactionTimeout;
        private int mashreqInitialResponseWaitPeriod;
        private ExecutionContext executionContext;

        public MashreqCommonRequestHandler(ExecutionContext executionContext, int port)
        {
            log.LogMethodEntry(port);
            this.port = port;
            this.executionContext = executionContext;
            log.LogMethodExit(port);
        }

        private void InitializeDefaults()
        {
            log.LogMethodEntry();
            string mashreqTimeout = string.Empty;
            if (int.TryParse(ConfigurationManager.AppSettings["MashreqTransactionTimeout"], out mashreqTransactionTimeout) == false)
            {
                mashreqTransactionTimeout = 120000;
            }
            if (int.TryParse(ConfigurationManager.AppSettings["MashreqInitialResponseWaitPeriod"], out mashreqInitialResponseWaitPeriod) == false)
            {
                mashreqInitialResponseWaitPeriod = 15000;
            }
            // LoadIntermediateMessagesMap();
            MakeConnection(port);
            if (_eft == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5168);
                log.Error(errorMessage);
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            if (_eft.IsBusy)
            {
                string errorMessage = "Terminal is busy! Please wait...";
                log.Error(errorMessage);
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit();
        }

        private void MakeConnection(int port)
        {
            log.LogMethodEntry(port);
            try
            {
                if (_eft == null)
                {
                    _eft = EftInterface.CreateEftInterface("COM" + port,
                          ConfigurationManager.AppSettings["InterMediateXml"]);
                    _eft.ControllerWait = mashreqInitialResponseWaitPeriod;// 15 seconds//setting timeout for the response message
                    _eft.TransactionTimeout = mashreqTransactionTimeout; // 3 minutes //setting a timeout for the transaction
                    _eft.EnableLog = ConfigurationManager.AppSettings["EnableLog"];

                    //subscribe to the event
                    _eft.OnUSBConnection += OnUSBConnection;
                    _eft.OnUSBDisconnection += OnUSBDisconnection;
                    _eft.OnTrace += TraceMessages;
                    _eft.OnTransactionStatus += IntermediateMessages;
                }

                //_eft.OnTransactionStatus += IntermediateMessages;
                log.LogVariableState("_eft", _eft);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Error establishing connection: " + ex.Message);
            }
            log.LogMethodExit(_eft);

        }

        public async Task<object> CreatePurchase(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            try
            {
                InitializeDefaults();
                int ret = _eft.Purchase(requestDTO.transactionAmount, requestDTO.mrefValue);
                log.LogVariableState("ret", ret);
                if (ret == 0)
                {

                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                object result = await getResponse();
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }


        public async Task<object> CreateRefund(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            try
            {
                InitializeDefaults();
                int ret = _eft.Refund(requestDTO.transactionAmount, requestDTO.mrefValue, requestDTO.authCode);
                if (ret == 0)
                {
                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                log.LogVariableState("ret", ret);
                object result = await getResponse();
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public async Task<object> CreatePreAuth(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            try
            {
                InitializeDefaults();
                int ret = _eft.PreAuth(requestDTO.transactionAmount, requestDTO.mrefValue);
                if (ret == 0)
                {
                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                object result = await getResponse();
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public async Task<object> CreatePreAuthCompletion(MashreqRequestDTO requestDTO)
        {
            try
            {
                InitializeDefaults();
                int ret = _eft.PreAuthCompletion(requestDTO.invoiceNumber, requestDTO.mrefValue);
                if (ret == 0)
                {
                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                object result = await getResponse();
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public async Task<object> CreatePreReceipt(MashreqRequestDTO requestDTO)
        {
            try
            {
                InitializeDefaults();
                int ret = _eft.PreReceipt(requestDTO.transactionAmount, requestDTO.mrefValue);
                if (ret == 0)
                {
                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                object result = await getResponse();
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public async Task<object> CreatePreReceiptCompletionWithTip(MashreqRequestDTO requestDTO)
        {
            try
            {
                InitializeDefaults();
                int ret = _eft.TipCompletion(requestDTO.invoiceNumber, requestDTO.mrefValue);
                if (ret == 0)
                {
                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                object result = await getResponse();
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public async Task<object> CreatePurchaseWithTip(MashreqRequestDTO requestDTO)
        {
            try
            {
                InitializeDefaults();
                int ret = _eft.PurchaseWithTip(requestDTO.transactionAmount, requestDTO.mrefValue);
                if (ret == 0)
                {
                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                object result = await getResponse();
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public async Task<object> CreateVoid(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            try
            {
                InitializeDefaults();
                int ret = _eft.VoidRefund(requestDTO.transactionAmount, requestDTO.invoiceNumber, requestDTO.mrefValue);
                if (ret == 0)
                {
                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                log.LogVariableState("ret", ret);
                object result = await getResponse();
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public async Task<object> CreateDuplicate(MashreqRequestDTO requestDTO)
        {
            try
            {
                InitializeDefaults();
                int ret = _eft.Duplicate(requestDTO.invoiceNumber, requestDTO.mrefValue);
                if (ret == 0)
                {
                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                object result = await getResponse();
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private void IntermediateMessages(object sender, IntermediateResponseDataEventArgs args)
        {
            log.LogMethodEntry(sender, args);
            string xmlString;
            var commandType = "";
            var errorCode = "";
            var intermediateResponse = "";

            try
            {
                xmlString = args.XmlData;
                eftData = deserializeXmlResponse(xmlString);
                log.LogVariableState("eftData in IntermediateMessage", eftData);
                commandType = eftData.CommandType;
                log.LogVariableState("commandType in IntermediateMessage", commandType);
                errorCode = eftData.ErrorCode;
                log.LogVariableState("errorCode in IntermediateMessage", errorCode);

                string[] commandTypeArray = { "100", "101", "104", "106", "109", "114", "119", "120", "121", "122", "123", "124" };
                isCommandTypeFound = Array.Find(commandTypeArray, element => element == commandType);
                log.LogVariableState("isCommandTypeFound in IntermediateMessage", isCommandTypeFound);

                if (string.IsNullOrEmpty(isCommandTypeFound) == false)
                {
                    log.LogMethodExit("string.IsNullOrEmpty(isCommandTypeFound) == false");
                    return;
                }
                if (commandType != null)
                {
                    intermediateResponse = getIntermediateResponseText(commandType);
                    log.LogVariableState("intermediateResponse", intermediateResponse);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                eftData = null;
                isCommandTypeFound = string.Empty;
                //_eft.OnTransactionStatus -= IntermediateMessages;
            }
            log.LogMethodExit();
        }

        //private object ProcessResponse()
        //{
        //    log.LogMethodEntry();
        //    var errorCode = "";
        //    var intermediateResponse = "";
        //    //var errorMessage = "";
        //    int ret;
        //    try
        //    {
        //        //MashreqDbConnection mashreqDbConnection = new MashreqDbConnection();
        //        if (eftData != null)
        //        {
        //            log.LogVariableState("eftData", eftData);
        //            errorCode = eftData.ErrorCode;
        //            log.LogVariableState("errorCode in ProcessData", errorCode);
        //            if (errorCode != null && errorCode != "E000")
        //            {
        //                // E000 is NO ERROR
        //                // We have received an error; process it
        //                //errorMessage = getErrorMessage(errorCode.ToString());
        //                //if (eftData.MREFValue != null)
        //                //{
        //                //    mashreqDbConnection.SaveResponseToDatabase(GetTimeStamp(), eftData.MREFValue.ToString(), eftData.TxnDescription.ToString(), eftData.ErrorCode.ToString(), eftData.ResponseCode.ToString(), eftData.Amount.ToString(), eftData.InvoiceNo.ToString(), eftData.AuthCode.ToString(), errorMessage);
        //                //}

        //                //Console.WriteLine(errorMessage);
        //                if (errorCode == "E067")
        //                {
        //                    log.Error("E067:Please delete the last transaction from the device.");
        //                    // TBC perform last trnx check
        //                    // at this moment device asks us to delete the trnx
        //                    throw new Exception("E067:Please delete the last transaction from the device.");
        //                    // in the response it returns error code E014=>User Cancellation
        //                    // we need to log that to the db
        //                    // reason for E067= terminal is not on base, or removed the card abruptly
        //                }
        //                else
        //                {
        //                    log.Error("Error captured: "+ errorCode);
        //                    throw new Exception("Error captured");
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw;
        //    }
        //    log.LogMethodExit();
        //    return null;
        //}

        public async Task<EFTData> getResponse()
        {
            log.LogMethodEntry();
            EFTData response = null;
            try
            {
                DateTime maxLimitTime = DateTime.Now.AddMilliseconds(mashreqTransactionTimeout);
                while (DateTime.Now < maxLimitTime)
                {
                    await Task.Delay(2000);
                    if (!string.IsNullOrEmpty(isCommandTypeFound))
                    {
                        response = eftData;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            finally
            {
                isCommandTypeFound = string.Empty;
                eftData = null;
            }
            log.LogMethodExit(response);
            return response;
        }


        //public bool GetCommandTypeValue()
        //{
        //    log.LogMethodEntry();
        //    for (int i = 0; i < 60; i++)
        //    {
        //        log.LogVariableState("isCommandTypeFound", isCommandTypeFound);
        //        if (string.IsNullOrWhiteSpace(isCommandTypeFound))
        //        {
        //            Thread.Sleep(1000);
        //            Task.Delay(1000);
        //            i++;
        //            if (i == 60)
        //            {
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            log.LogMethodExit(true);
        //            return true;
        //        }
        //    }
        //    log.LogMethodExit(false);
        //    return false;
        //}
        //while (isCommandTypeFound == null)
        //{
        //    Thread.Sleep(1000);
        //    Task.Delay(1000);
        //    tryCount++;
        //    if (tryCount == 60)
        //    {
        //        break;
        //    }
        //    GetCommandTypeValue();
        //}
        //do
        //{
        //    return true;
        //}
        //}

        //While(isCommandTypeFound == null)
        //{

        //    GetCommandTypeValue();
        //}
        //else
        //{
        //    return true;
        //}
        //return false;


        public async Task<object> GetTerminalInfo()
        {
            log.LogMethodEntry();
            object result;
            try
            {
                InitializeDefaults();
                int ret = _eft.GetTerminalInfo();
                if (ret == 0)
                {
                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                result = await getResponse();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(result);
            return result;
        }


        public async Task<object> CheckLastTransactionStatus(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            try
            {
                InitializeDefaults();
                int ret = _eft.LastTransactionStatus(requestDTO.mrefValue);
                log.LogVariableState("ret", ret);
                if (ret == 0)
                {
                    log.Error("Could not connect to the terminal. Please check connection.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5168));
                }
                object result = await getResponse();
                log.LogMethodEntry(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Error checking last transaction ", ex);
                throw;
            }
        }

        private void OnUSBConnection(object source, EventArgs e)
        {
            log.LogMethodEntry(source);
            try
            {
                // TBC
                log.Debug("Payment Device connected via USB");
                log.Debug(source.ToString());
                log.Debug(e.ToString());
                Console.WriteLine("Payment Device connected via USB");
                Console.WriteLine(source.ToString());
                Console.WriteLine(e.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //throw ;
            }

            log.LogMethodExit();
        }

        private void OnUSBDisconnection(object source, EventArgs e)
        {
            Console.WriteLine("USB Disconnection");
            log.LogMethodEntry(source);
            try
            {
                if (_eft != null)
                {
                    _eft.Close();
                }
                log.Error("Error: Payment Device disconnected from USB!");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                _eft = null;
            }
        }

        private void TraceMessages(object sender, TraceMessageEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                string traceMessage = string.Format("{0}::{1}", e.TraceDate, e.Trace);
                log.Debug("Trace Message: "+ traceMessage);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private string getIntermediateResponseText(string commandType)
        {
            log.LogMethodEntry(commandType);
            string responseText = "";
            try
            {
                if (IntermediateMessagesMap.ContainsKey(commandType))
                {
                    IntermediateMessagesMap.TryGetValue(commandType, out responseText);
                }
                else
                {
                    responseText = "Other";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(responseText);
            return responseText;
        }

        //private string getErrorMessage(string errorCode)
        //{
        //    log.LogMethodEntry(errorCode);
        //    string responseText = "";
        //    try
        //    {
        //        if (ErrorMessagesMap.ContainsKey(errorCode))
        //        {
        //            ErrorMessagesMap.TryGetValue(errorCode, out responseText);
        //        }
        //        else
        //        {
        //            responseText = "Other";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw;
        //    }
        //    log.LogMethodExit(responseText);
        //    return responseText;
        //}

        private EFTData deserializeXmlResponse(string xmlString)
        {
            log.LogMethodEntry(xmlString);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EFTData));
                using (var reader = new StringReader(xmlString))
                {
                    eftData = (EFTData)serializer.Deserialize(reader);

                    if (eftData == null)
                    {
                        log.Error("Error in deserialization of the response!" + xmlString);
                        throw new Exception("Error in deserialization of the response!");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(eftData);
            return eftData;
        }

        private string EncodeResponse(EFTData eFTData)
        {
            log.LogMethodEntry(eFTData);
            var encodedResponse = string.Empty;
            try
            {
                encodedResponse = JsonConvert.SerializeObject(eFTData);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Exception from EncodeResponse: " + ex.Message);
            }
            log.LogMethodExit(encodedResponse);
            return encodedResponse;
        }

        private void LoadIntermediateMessagesMap()
        {
            log.LogMethodEntry();
            try
            {
                IntermediateMessagesMap.Add("001", "Enter card Displayed");
                IntermediateMessagesMap.Add("002", "Card Inserted");
                IntermediateMessagesMap.Add("003", "FallBack happened");
                IntermediateMessagesMap.Add("004", "Display Swipe Card");
                IntermediateMessagesMap.Add("005", "Card Swiped");
                IntermediateMessagesMap.Add("006", "Enter PIN");
                IntermediateMessagesMap.Add("007", "PIN Entered");
                IntermediateMessagesMap.Add("008", "Online Processing");
                IntermediateMessagesMap.Add("009", "Response received from the host");
                IntermediateMessagesMap.Add("010", "Printing of Receipt");
                IntermediateMessagesMap.Add("011", "Remove Card displayed");
                IntermediateMessagesMap.Add("012", "Card Removed");
                IntermediateMessagesMap.Add("015", "Transaction Being Reversed");
                IntermediateMessagesMap.Add("014", "Transaction Failed");
                IntermediateMessagesMap.Add("013", "Transaction Successful");
                IntermediateMessagesMap.Add("016", "Waiting for Logo");
                IntermediateMessagesMap.Add("017", "Displaying EPP Menu");
                IntermediateMessagesMap.Add("018", "Sending EPP Inq");
                IntermediateMessagesMap.Add("019", "EPP Inq Response Received from Host");
                IntermediateMessagesMap.Add("020", "Print EPP Plan");
                IntermediateMessagesMap.Add("021", "Displaying Salaam Menu");
                IntermediateMessagesMap.Add("022", "Sending Salaam Inq");
                IntermediateMessagesMap.Add("023", "Salaam Inq Received from Host");
                IntermediateMessagesMap.Add("024", "Print Salam Inq");
                IntermediateMessagesMap.Add("025", "DCC Rate Look up");
                IntermediateMessagesMap.Add("026", "DCC Offer Screen");
                IntermediateMessagesMap.Add("027", "DCC OPT IN");
                IntermediateMessagesMap.Add("028", "DCC OPT OUT");
                IntermediateMessagesMap.Add("029", "DCC Selection Cancelled");
                IntermediateMessagesMap.Add("030", "DCC Selection Wait");
                IntermediateMessagesMap.Add("031", "Fallback Contactless to Contact");
                IntermediateMessagesMap.Add("032", "Out of Paper, replace paper roll");
                IntermediateMessagesMap.Add("033", "Enter Tip Amount");
                IntermediateMessagesMap.Add("034", "Display New Amount");
                IntermediateMessagesMap.Add("035", "Enter Original Amount");
                IntermediateMessagesMap.Add("036", "Send Auth Completion Verification");
                IntermediateMessagesMap.Add("037", "Display Auth verification screen");
                IntermediateMessagesMap.Add("038", "Auth Completion Amount entered");
                IntermediateMessagesMap.Add("039", "Enter Auth Code");
                IntermediateMessagesMap.Add("040", "Purchase Transaction to Auto EPP");
                IntermediateMessagesMap.Add("041", "Display Auto EPP (if the card or amount is eligible for EPP)");
                IntermediateMessagesMap.Add("042", "PIN ByPass Entered");
                IntermediateMessagesMap.Add("043", "Manual Entry");
                IntermediateMessagesMap.Add("044", "Contactless (Tap card)");
                IntermediateMessagesMap.Add("045", "Max Tip Exceeded");
                IntermediateMessagesMap.Add("046", "Terminal Removed from Base");
                IntermediateMessagesMap.Add("047", "Check Last Txn Status");
                IntermediateMessagesMap.Add("048", "Enter MREF");
                IntermediateMessagesMap.Add("049", "MREF Entered");
                IntermediateMessagesMap.Add("052", "GPRS Communication");
                IntermediateMessagesMap.Add("053", "Ethernet Communication");
                IntermediateMessagesMap.Add("055", "Serial Communication");
                IntermediateMessagesMap.Add("056", "Scan QR");
                IntermediateMessagesMap.Add("057", "EDW Customer app notification received");
                IntermediateMessagesMap.Add("058", "Wallet Txn history query/Get Tran status");
                IntermediateMessagesMap.Add("059", "Wallet Response timeout");
                IntermediateMessagesMap.Add("060", "Enter Reference number");
                IntermediateMessagesMap.Add("061", "Enter OTP");
                IntermediateMessagesMap.Add("062", "OTP Entered");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        //private string GetTimeStamp()
        //{
        //    DateTime timestamp;
        //    try
        //    {
        //        timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Exception in getting timestamp", ex);
        //    }

        //    return timestamp.ToString();

        //}

        //public void CloseConnection()
        //{
        //    try
        //    {
        //        if (_eft == null)
        //        {
        //            throw new Exception("No connection available to terminate!");
        //        }
        //        _eft.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error while closing the connection: "+ex);
        //    }
        //    finally
        //    {
        //        _eft = null;
        //    }
        //}
    }
}
