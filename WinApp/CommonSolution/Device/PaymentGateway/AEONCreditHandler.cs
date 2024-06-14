/********************************************************************************************
 * Project Name - AEON Credit Handler
 * Description  - Data handler of the AEONCreditHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        20-Jul-2019   Raghuveera      Created  
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class AEONCreditHandler
    {
        SerialPortHandler serialPortHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CCTransactionsPGWDTO cCTransactionsPGWDTO;
        private AEONCreditResponse aeonCreditResponse;
        private string mReceiptText;
        private string cReceiptText;
        public string MReceiptText { get { return mReceiptText; } }
        public string CReceiptText { get{ return cReceiptText; } }
        private Utilities utilities;
        public Dictionary<int, string> cardTypes = new Dictionary<int, string>();
        Dictionary<int, string> tranTypes = new Dictionary<int, string>();

        public AEONCreditHandler(Utilities utilities)
        {
            log.LogMethodEntry();
            this.utilities = utilities;
            serialPortHandler = SerialPortHandler.GetSerialPortHandler("AEONCredit.log");

            int comPort = Convert.ToInt32(utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO"));
            if (comPort <= 0)
            {
                log.LogMethodExit(null, "Throwing Exception - AEONCredit CREDIT_CARD_TERMINAL_PORT_NO not set.");
                throw new Exception("AEONCredit CREDIT_CARD_TERMINAL_PORT_NO not set.");
            }
            serialPortHandler.SetPortDetail(comPort,9600,System.IO.Ports.Parity.None,8, System.IO.Ports.StopBits.One, System.IO.Ports.Handshake.None);
            serialPortHandler.OpenVerifonePort();
            cardTypes.Add(41, "Visa Credit Card");
            cardTypes.Add(42, "MasterCard Credit Card");
            cardTypes.Add(60, "eMoney Card");
            cardTypes.Add(61, "AEON Pay-V");
            cardTypes.Add(62, "AEON Pay-M");
            cardTypes.Add(63, "AEON Pay-E");

            tranTypes.Add(10, "Chip Card");
            tranTypes.Add(20, "Contactless");
            tranTypes.Add(30, "Swipe");
            tranTypes.Add(40, "Fallback");
            tranTypes.Add(50, "Manual");
            tranTypes.Add(60, "QR");
            log.LogMethodExit();
        }
        public CCTransactionsPGWDTO ProcessTransaction(TransactionPaymentsDTO trxPaymentDTO, CCTransactionsPGWDTO ccOrigTransDTO, TransactionType trxType)
        {
            log.LogMethodEntry(trxPaymentDTO, trxType);
            string command = "";
            aeonCreditResponse = new AEONCreditResponse();
            switch (trxType.ToString())
            {
                case "SALE":
                    command = CreateSaleCommand(trxPaymentDTO);                    
                    break;
                case "REFUND":
                    command = CreateRefundCommand(trxPaymentDTO);
                    break;
                case "VOID":
                    command = CreateVoidCommand(ccOrigTransDTO);
                    break;
            }
            if (!string.IsNullOrEmpty(command))
            {
                SendCommand(command);
                UpdateccTransactionsDTO();
                cCTransactionsPGWDTO.TranCode = trxType.ToString();
                if(ccOrigTransDTO!=null)
                {
                    cCTransactionsPGWDTO.ResponseOrigin = ccOrigTransDTO.ResponseID.ToString();                    
                }
                if (string.IsNullOrEmpty(cCTransactionsPGWDTO.Authorize))
                {
                    cCTransactionsPGWDTO.Authorize = trxPaymentDTO.Amount.ToString("0.00");
                    aeonCreditResponse.SalesTotal_H0 = trxPaymentDTO.Amount.ToString("0.00").Replace("-", "");
                }
                GenarateReceipt(trxPaymentDTO);
                trxPaymentDTO.Memo = cReceiptText +Environment.NewLine + Environment.NewLine + mReceiptText;
            }
            else
            {
                log.LogMethodExit("Invalid command");
                throw new Exception("Invalid command");
            }
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }
        private string CreateSaleCommand(TransactionPaymentsDTO trxPaymentDTO)
        {
            log.LogMethodEntry(trxPaymentDTO);
            string command = "";
            command = "6000000000";
            command = command + "1020000" + Convert.ToChar(SerialPortHandler.FS);
            command = command + "400" + Convert.ToChar(SerialPortHandler.DC2) + (trxPaymentDTO.Amount * 100).ToString().PadLeft(12, '0') + Convert.ToChar(SerialPortHandler.FS);
            log.LogMethodExit(command);
            return command;
        }
        private string CreateRefundCommand(TransactionPaymentsDTO trxPaymentDTO)
        {
            log.LogMethodEntry(trxPaymentDTO);
            string command = "";
            command = "6000000000";
            command = command + "1041000" + Convert.ToChar(SerialPortHandler.FS);
            command = command + "400"+ Convert.ToChar(SerialPortHandler.DC2) + (trxPaymentDTO.Amount * 100).ToString().PadLeft(12, '0') + Convert.ToChar(SerialPortHandler.FS);
            log.LogMethodExit(command);
            return command;
        }
        private string CreateVoidCommand(CCTransactionsPGWDTO ccOrigTransDTO)
        {
            log.LogMethodEntry(ccOrigTransDTO);
            string command = "";
            command = "6000000000";
            command = command + "1040000" + Convert.ToChar(SerialPortHandler.FS);
            command = command + "6506" + (ccOrigTransDTO.RefNo).ToString().PadLeft(6, '0') + Convert.ToChar(SerialPortHandler.FS);
            command = command + "670%" + ccOrigTransDTO.RecordNo.ToString().PadLeft(25, '0') + Convert.ToChar(SerialPortHandler.FS);
            log.LogMethodExit(command);
            return command;
        }
        private void UpdateResponseObject(string response)
        {
            log.LogMethodEntry(response);
            //Reads the response and update the response to the AEONResponse object
            int index = 0;
            string[] fieldDataArray;
            string caseData;            
            string convertedStringData;
            byte[] data;
            int len=0;
            if (string.IsNullOrEmpty(response))
            {
                log.LogMethodExit("Response is empty");
                throw new Exception("Response is empty");
            }
            int fsindex = response.IndexOf("1C");
            if (fsindex > 0)
            {
                data = serialPortHandler.FromHex(response.Substring(fsindex - 9, 8));//32-30-30-30-30-1C 
                aeonCreditResponse.ResponseCode = Encoding.ASCII.GetString(data).Substring(0, 2);
                aeonCreditResponse.MoreIndicator = Encoding.ASCII.GetString(data).Substring(2, 1);
            }
            string[] spliter = new string[1] { "1C" };
            fieldDataArray = response.Substring(fsindex + 2).Split(spliter,StringSplitOptions.None);
            while (fieldDataArray != null && fieldDataArray.Length > 0 && index < fieldDataArray.Length-1)
            {
                log.LogVariableState("fieldDataArray["+index+"]", fieldDataArray[index]);
                data = serialPortHandler.FromHex(fieldDataArray[index].Substring(1,5));// 30-32-00-40
                convertedStringData = Encoding.ASCII.GetString(data);
                log.LogVariableState("convertedcaseData", convertedStringData);
                caseData = convertedStringData.Substring(0, 2);
                log.LogVariableState("caseData", caseData);
                len = Convert.ToInt32(fieldDataArray[index].Substring(7, 5).Replace("-", ""));
                data = serialPortHandler.FromHex(fieldDataArray[index].Substring(13, (len*2)+ len-1));// 30-32-00-40
                convertedStringData = Encoding.ASCII.GetString(data);
                convertedStringData = convertedStringData.Trim(new[] { Convert.ToChar(0x00) });
                log.LogVariableState("Data", convertedStringData);
                //data = BitConverter.GetBytes();
                switch (caseData)
                {

                    //approvalCode
                    case "01":aeonCreditResponse.ApprovalCode_01 = convertedStringData; break;
                    //responseText 
                    case "02": aeonCreditResponse.ResponseText_02 = convertedStringData; break;
                    //transactionDate 
                    case "03": aeonCreditResponse.TransactionDate_03 = convertedStringData; break;
                    //transactionTime 
                    case "04": aeonCreditResponse.TransactionTime_04 = convertedStringData; break;
                    //terminalId 
                    case "16": aeonCreditResponse.TerminalId_16 = convertedStringData; break;
                    //panMask 
                    case "30": aeonCreditResponse.PanMask_30 = convertedStringData; break;
                    //cardExpireDate 
                    case "31": aeonCreditResponse.CardExpireDate_31 = convertedStringData; break;
                    //memberExpireDate 
                    case "32": aeonCreditResponse.MemberExpireDate_32 = convertedStringData; break;
                    //encryptedPan 
                    case "33": aeonCreditResponse.EncryptedPan_33 = convertedStringData; break;
                    //accountBalance 
                    case "34": aeonCreditResponse.AccountBalance_34 = convertedStringData; break;
                    //amount 
                    case "40": aeonCreditResponse.Amount_40 = convertedStringData; break;
                    //cashBackAmount 
                    case "42": aeonCreditResponse.CashBackAmount_42 = convertedStringData; break;
                    //batchNumber 
                    case "50": aeonCreditResponse.BatchNumber_50 = convertedStringData; break;
                    //uniqueInvoiceNo 
                    case "65": aeonCreditResponse.UniqueInvoiceNo_65 = convertedStringData; break;
                    //extendedInvoiceNo 
                    case "67": aeonCreditResponse.ExtendedInvoiceNo_67 = convertedStringData; break;
                    //customfielddata
                    case "99": aeonCreditResponse.CustomData_99 = convertedStringData; break;
                    //merchantNameAddress 
                    case "D0": aeonCreditResponse.MerchantNameAddress_D0 = convertedStringData; break;
                    //merchantNumber 
                    case "D1": aeonCreditResponse.MerchantNumber_D1 = convertedStringData; break;
                    //cardIssuerName 
                    case "D2": aeonCreditResponse.CardIssuerName_D2 = convertedStringData; break;
                    //retrievalReferenceNumber 
                    case "D3": aeonCreditResponse.RetrievalReferenceNumber_D3 = convertedStringData; break;
                    //cardIssuerId 
                    case "D4": aeonCreditResponse.CardIssuerId_D4 = convertedStringData; break;
                    //cardHolderName 
                    case "D5": aeonCreditResponse.CardHolderName_D5 = convertedStringData; break;
                    //cardIssuerDate 
                    case "D6": aeonCreditResponse.CardIssuerDate_D6 = convertedStringData; break;
                    //cardLabel 
                    case "D7": aeonCreditResponse.CardLabel_D7 = convertedStringData; break;
                    //hostName 
                    case "D8": aeonCreditResponse.HostName_D8 = convertedStringData; break;
                    //stan 
                    case "D9": aeonCreditResponse.Stan_D9 = convertedStringData; break;
                    //aid 
                    case "E0": aeonCreditResponse.Aid_E0 = convertedStringData; break;
                    //applicationProfile 
                    case "E1": aeonCreditResponse.ApplicationProfile_E1 = convertedStringData; break;
                    //cid 
                    case "E2": aeonCreditResponse.Cid_E2 = convertedStringData; break;
                    //applicationCryptGram 
                    case "E3": aeonCreditResponse.ApplicationCryptGram_E3 = convertedStringData; break;
                    //tsi 
                    case "E4": aeonCreditResponse.Tsi_E4 = convertedStringData; break;
                    //tvr 
                    case "E5": aeonCreditResponse.Tvr_E5 = convertedStringData; break;
                    //cardEntryMode 
                    case "E6": aeonCreditResponse.CardEntryMode_E6 = convertedStringData; break;
                    //cashierId 
                    case "E7": aeonCreditResponse.CashierId_E7 = convertedStringData; break;
                    //receiptFooterMcpy 
                    case "F0": aeonCreditResponse.ReceiptFooterMcpy_F0 = convertedStringData; break;
                    //receiptFooterCcpy 
                    case "F1": aeonCreditResponse.ReceiptFooterCcpy_F1 = convertedStringData; break;
                    //salesTotal 
                    case "H0": aeonCreditResponse.SalesTotal_H0 = convertedStringData; break;
                    //offlineSalesTotal 
                    case "H1": aeonCreditResponse.OfflineSalesTotal_H1 = convertedStringData; break;
                    //voidSalesTotal 
                    case "H2": aeonCreditResponse.VoidSalesTotal_H2 = convertedStringData; break;
                    //cashBackTotal 
                    case "H3": aeonCreditResponse.CashBackTotal_H3 = convertedStringData; break;
                    //voidCashBackTotal 
                    case "H4": aeonCreditResponse.VoidCashBackTotal_H4 = convertedStringData; break;
                    //refundTotal 
                    case "H5": aeonCreditResponse.RefundTotal_H5 = convertedStringData; break;
                    //voidRefundTotal 
                    case "H6": aeonCreditResponse.VoidRefundTotal_H6 = convertedStringData; break;
                    //settlementSequenceNo 
                    case "S1": aeonCreditResponse.SettlementSequenceNo_S1 = convertedStringData; break;
                    //walletProductId 
                    case "W1": aeonCreditResponse.WalletProductId_W1 = convertedStringData; break;
                }
                index++;
            }
            if (string.IsNullOrEmpty(aeonCreditResponse.ResponseText_02))
            {
                aeonCreditResponse.ResponseText_02 = GetResponse(aeonCreditResponse.ResponseCode);
            }            
            log.LogMethodExit(aeonCreditResponse);
        }
        private void UpdateccTransactionsDTO()
        {
            log.LogMethodEntry();
            if(aeonCreditResponse!=null)
            {
                cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                cCTransactionsPGWDTO.RefNo = aeonCreditResponse.UniqueInvoiceNo_65;
                cCTransactionsPGWDTO.RecordNo = aeonCreditResponse.ExtendedInvoiceNo_67;
                //cCTransactionsPGWDTO.Purchase = aeonCreditResponse.Amount_40;
                cCTransactionsPGWDTO.Authorize= aeonCreditResponse.SalesTotal_H0;
                cCTransactionsPGWDTO.TipAmount = aeonCreditResponse.CashBackTotal_H3;
                cCTransactionsPGWDTO.AcctNo = aeonCreditResponse.PanMask_30;
                cCTransactionsPGWDTO.Purchase = utilities.ParafaitEnv.POSMachine.ToString();
                cCTransactionsPGWDTO.CaptureStatus = aeonCreditResponse.CardEntryMode_E6;
                cCTransactionsPGWDTO.CardType = aeonCreditResponse.CardIssuerId_D4;
                cCTransactionsPGWDTO.DSIXReturnCode = aeonCreditResponse.ResponseCode;                
                cCTransactionsPGWDTO.TextResponse = aeonCreditResponse.ResponseText_02.Trim();
                cCTransactionsPGWDTO.TransactionDatetime = DateTime.ParseExact((string.IsNullOrEmpty(aeonCreditResponse.TransactionDate_03) ? ServerDateTime.Now.ToString("yyMMdd") : aeonCreditResponse.TransactionDate_03) + (string.IsNullOrEmpty(aeonCreditResponse.TransactionTime_04) ? ServerDateTime.Now.ToString("HHmmss") : aeonCreditResponse.TransactionTime_04), "yyMMddHHmmss", CultureInfo.InvariantCulture);
                cCTransactionsPGWDTO.UserTraceData = aeonCreditResponse.CardLabel_D7;
                cCTransactionsPGWDTO.AuthCode = aeonCreditResponse.ApprovalCode_01; 
                cCTransactionsPGWDTO.TokenID = aeonCreditResponse.Stan_D9;
                cCTransactionsPGWDTO.AcqRefData = "AID:" + aeonCreditResponse.Aid_E0 + "|Name:" + aeonCreditResponse.CardHolderName_D5 + "|Exp:" + aeonCreditResponse.CardExpireDate_31 + "|CID:"
                    + aeonCreditResponse.Cid_E2 + "|TSI:" + aeonCreditResponse.Tsi_E4 + "|TVR:" + aeonCreditResponse.Tvr_E5;
                cCTransactionsPGWDTO.ProcessData = aeonCreditResponse.RetrievalReferenceNumber_D3+"|"+aeonCreditResponse.TerminalId_16;
                if (string.IsNullOrEmpty(cCTransactionsPGWDTO.RecordNo))
                {
                    cCTransactionsPGWDTO.RecordNo = " ";
                }
            }
            log.LogMethodExit();
        }
        private void SendCommand(string command)
        {
            log.LogMethodEntry(command);
            List<byte> byteArray;
            string responseString = "";
            byte[] responsebyte = null;
            int respLength = 0;
            byteArray = serialPortHandler.ConvertStringToByte(command).ToList<byte>();
            int length = byteArray.Count;
            byteArray.Insert(0, Convert.ToByte(int.Parse(length.ToString(), System.Globalization.NumberStyles.HexNumber))); 
            byteArray.Insert(0, 0);
            byteArray.Insert(0, SerialPortHandler.STX);
            byteArray.Add(SerialPortHandler.ETX);
            byteArray.Add(serialPortHandler.ComputeLRC(byteArray.ToArray(),true));
            string commandstring = BitConverter.ToString(byteArray.ToArray());
            log.LogVariableState("Command",commandstring);
            serialPortHandler.WriteToDeviceLog("ECR>Device: Command=" + commandstring);
            if (serialPortHandler.SendCommand(byteArray,180000,true))
            {                 
                
                if (serialPortHandler.ReadResponse(ref responsebyte, ref respLength, true))
                {
                    if(responsebyte==null|| responsebyte.Length==0)
                    {
                        log.LogMethodExit(null,"Response is null");
                        throw new Exception("No Response.");
                    }
                    responseString = BitConverter.ToString(responsebyte.ToArray());
                    UpdateResponseObject(responseString);
                }
                else
                {
                    if (responsebyte == null || responsebyte.Length == 0)
                    {
                        log.LogMethodExit(null, "Response is null");
                        throw new Exception("No Response.");
                    }
                }
            }
            else
            {
                log.LogMethodExit(null, "Send command is failed.Device may not be connected or busy.");
                throw new Exception("Unable to send command to device.AEON device might be busy or not connected.");
            }
            if (!string.IsNullOrEmpty(responseString))
            {
                while (aeonCreditResponse.MoreIndicator.Equals("1"))
                {
                    responsebyte = null;
                    respLength = 0;
                    if (serialPortHandler.ReadResponse(ref responsebyte, ref respLength, true))
                    {
                        if (responsebyte == null || responsebyte.Length == 0)
                        {
                            log.LogMethodExit(null, "More Response is null");
                            throw new Exception("No Response.");
                        }
                        responseString = BitConverter.ToString(responsebyte.ToArray());
                        UpdateResponseObject(responseString);
                    }
                    else
                    {
                        if (responsebyte == null || responsebyte.Length == 0)
                        {
                            log.LogMethodExit(null, "More Response is null");
                            throw new Exception("No Response.");
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private string GetResponse(string code)
        {
            log.LogMethodEntry(code);
            string message = "";
            switch (code)
            {                
                case "01": message = "PLEASE CALL ISSUER"; break;
                case "02": message = "PLEASE CALL REFERRAL"; break;
                case "03": message = "INVLD MERCHANT"; break;
                case "04": message = "PLS.PICK UP CARD"; break;
                case "05": message = "DO NOT HONOUR"; break;
                case "06": message = "ERROR"; break;
                case "07": message = "Pickup Card, Spl Cond"; break;
                case "08": message = "VERIFY ID AND SIGN"; break;
                case "10": message = "Appvd for Partial Amt"; break;
                case "11": message = "Approved(VIP)"; break;
                case "12": message = "INVLD TRANSACTION"; break;
                case "13": message = "INVLD AMT"; break;
                case "14": message = "INVLD CARD NUM"; break;
                case "15": message = "No such Issuer"; break;
                case "16": message = "Approved, Update Tk 3"; break;
                case "17": message = "Customer Cancellation"; break;
                case "18": message = "Customer Dispute"; break;
                case "19": message = "RE - ENTER TRANSACTION"; break;
                case "20": message = "INVALID RESPONSE"; break;
                case "21": message = "NO TRANSACTIONS"; break;
                case "22": message = "Suspected Malfunction"; break;
                case "23": message = "Unaccepted Trans Fee"; break;
                case "24": message = "Declined, wrong P55"; break;
                case "25": message = "Declined, wrong crypto"; break;
                case "26": message = "Dup Rec, Old Rec Rplcd"; break;
                case "27": message = "FIELD EDIT ERROR"; break;
                case "28": message = "FILE LOCKED OUT"; break;
                case "29": message = "File Update Error"; break;
                case "30": message = "FORMAT ERROR"; break;
                case "31": message = "BANK NOT SUPPORTED"; break;
                case "32": message = "Completed Partially"; break;
                case "33": message = "EXPIRED CARD"; break;
                case "34": message = "SUSPECTED FRAUD"; break;
                case "35": message = "Contact Acquirer"; break;
                case "36": message = "Restricted Card"; break;
                case "37": message = "Call Acq.Security"; break;
                case "38": message = "PIN tries Exceeded"; break;
                case "39": message = "No Credit Account"; break;
                case "40": message = "FUNC.NOT SUPPORTED"; break;
                case "41": message = "LOST CARD"; break;
                case "42": message = "NO UNIVERSAL ACCOUNT"; break;
                case "43": message = "Please Call - CC"; break;
                case "44": message = "No Investment Account"; break;
                case "45": message = "ISO error #45"; break;
                case "46": message = "PLS INSERT CARD"; break;
                case "47": message = "ISO error #47"; break;
                case "48": message = "ISO error #48"; break;
                case "49": message = "ISO error #49"; break;
                case "50": message = "ONLINE PIN REQUESTED"; break;
                case "51": message = "INSUFFICIENT FUND"; break;
                case "52": message = "NO CHEQUE ACC"; break;
                case "53": message = "NO SAVINGS ACCOUNT"; break;
                case "54": message = "EXPIRED CARD"; break;
                case "55": message = "Incorrect PIN"; break;
                case "56": message = "No Card Record"; break;
                case "57": message = "Txn not Permtd - card"; break;
                case "58": message = "TRANS NOT PERMITTED"; break;
                case "59": message = "Suspected Fraud"; break;
                case "60": message = "CONTACT ACQUIRER"; break;
                case "61": message = "EXCEED LIMIT"; break;
                case "62": message = "Restricted Card"; break;
                case "63": message = "SECURITY VIOLATION"; break;
                case "64": message = "ORG AMOUNT INCORRECT"; break;
                case "65": message = "Freq.Limit Exceed"; break;
                case "66": message = "CALL ACQ'S SECURITY"; break;
                case "67": message = "HARD CAPTURE"; break;
                case "68": message = "Resp Recvd too Late"; break;
                case "69": message = "PLS TRY AGAIN LATER"; break;
                case "70": message = "PIN NOT MATCHED"; break;
                case "71": message = "PIN EXPIRED"; break;
                case "72": message = "ISO ERROR #72"; break;
                case "73": message = "ISO ERROR #73"; break;
                case "74": message = "No Comm With Host"; break;
                case "75": message = "PIN TRIES EXCEED"; break;
                case "76": message = "KEY SYNC ERROR"; break;
                case "77": message = "Resvd. for Nat.use"; break;
                case "78": message = "OLD ROC NOT FOUND"; break;
                case "79": message = "BATCH ALREADY OPEN"; break;
                case "80": message = "Resvd. for Nat.use"; break;
                case "81": message = "Private error #81"; break;
                case "82": message = "Private error #82"; break;
                case "83": message = "INVALID PIN"; break;
                case "84": message = "Private error #84"; break;
                case "85": message = "BATCH NOT FOUND"; break;
                case "86": message = "PRIVATE ERROR #86"; break;
                case "87": message = "Private error #87"; break;
                case "88": message = "HAVE CM CALL AMEX"; break;
                case "89": message = "RESVD.FOT NAT.USE"; break;
                case "90": message = "Cutoff in Process"; break;
                case "91": message = "NETWORK ERROR"; break;
                case "92": message = "Trans can't be Routed"; break;
                case "93": message = "TXN CAN NOT BE COMPLETED"; break;
                case "94": message = "DUPL TRANSMISSION"; break;
                case "95": message = "RECONCILE ERROR"; break;
                case "96": message = "INVALID MESSAGE"; break;
                case "97": message = "RESVD.FOR NAT.USE"; break;
                case "98": message = "RESVD.FOR NAT.USE"; break;
                case "99": message = "RESVD.FOR NAT.USE"; break;
                case "NA": message = "TRANSACTION NOT SUCCESS"; break;
                case "CP": message = "Card Present"; break;
                case "MS": message = "MUST SETTLE"; break;
                case "NF": message = "RECORD NOT FOUND"; break;
                case "CE": message = "TRANS COMM ERROR"; break;
                case "CC": message = "CANNOT CANCEL"; break;
                case "UA": message = "USER ABORT"; break;
                case "ST": message = "SCREEN TIMEOUT"; break;
                case "WE": message = "WALLET ERROR"; break;
            }
            log.LogMethodExit(message);
            return message;

        }
        private void GenarateReceipt(TransactionPaymentsDTO trxPaymentDTO)
        {
            log.LogMethodEntry();
            if (aeonCreditResponse != null)
            {
                int counter = 0;
                cReceiptText = mReceiptText = "";
                if(string.IsNullOrEmpty(aeonCreditResponse.MerchantNameAddress_D0))
                {
                    log.LogVariableState("aeonCreditResponse", aeonCreditResponse);
                    log.LogMethodExit("No data to print receipt");
                    return;
                }
                while (counter < (aeonCreditResponse.MerchantNameAddress_D0.Length / 40)+1)
                {
                    cReceiptText = mReceiptText += CCGatewayUtils.AllignText(aeonCreditResponse.MerchantNameAddress_D0.Substring(counter*40, (aeonCreditResponse.MerchantNameAddress_D0.Length- (counter * 40))<40? (aeonCreditResponse.MerchantNameAddress_D0.Length - (counter * 40)):40), Alignment.Center)+Environment.NewLine;
                    counter++;
                }
                cReceiptText = mReceiptText += Environment.NewLine + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("DATE/TIME") +"      :"+cCTransactionsPGWDTO.TransactionDatetime.ToString("dd/MM/yy HH:mm:ss"), Alignment.Left)+ Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("TID") + "           :" + aeonCreditResponse.TerminalId_16, Alignment.Left) +Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("MID") + "           :" + aeonCreditResponse.MerchantNumber_D1, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("BATCH NUM") + "     :" + aeonCreditResponse.BatchNumber_50, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("INV#") + "          :" + aeonCreditResponse.UniqueInvoiceNo_65, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("MACHINE") + "       :" + utilities.ParafaitEnv.POSMachine, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("TRXID") + "         :" + trxPaymentDTO.TransactionId, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage(cCTransactionsPGWDTO.TranCode) , Alignment.Center) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("CARD TYPE") + "     :" + cardTypes[Convert.ToInt32(aeonCreditResponse.CardIssuerId_D4)], Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("APP NAME") + "      :" + aeonCreditResponse.CardLabel_D7, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("CARD NUM") + "      :" + aeonCreditResponse.PanMask_30.Substring(0,6).PadRight(12,'X')+ aeonCreditResponse.PanMask_30.Substring(12, 4), Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("EXP") + "           :" + "XX/XX", Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("ENT") + "           :" + tranTypes[Convert.ToInt32(aeonCreditResponse.CardEntryMode_E6)], Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("APPR CODE") + "     :" + aeonCreditResponse.ApprovalCode_01, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("TRACE#") + "        :" + aeonCreditResponse.Stan_D9, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("RREF NUM") + "      :" + aeonCreditResponse.RetrievalReferenceNumber_D3, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("APP CRYPT") + "     :" + aeonCreditResponse.ApplicationCryptGram_E3, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("TVR VALUE") + "     :" + aeonCreditResponse.Tvr_E5, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("AID") + "           :" + aeonCreditResponse.Aid_E0, Alignment.Left) + Environment.NewLine;
                cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("AMT") + "           :" + Convert.ToDecimal(aeonCreditResponse.SalesTotal_H0).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Center) + Environment.NewLine;
                if (Convert.ToInt32(aeonCreditResponse.CardIssuerId_D4) == 60)
                {
                    cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("NEW BAL") + "       :" + aeonCreditResponse.AccountBalance_34, Alignment.Left) + Environment.NewLine;
                    cReceiptText = mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("MEM EXP") + "       :" + aeonCreditResponse.MemberExpireDate_32, Alignment.Left) + Environment.NewLine;
                }
                cReceiptText += CCGatewayUtils.AllignText(aeonCreditResponse.ReceiptFooterCcpy_F1, Alignment.Center) + Environment.NewLine;
                cReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("--CUSTOMER COPY--"), Alignment.Center) + Environment.NewLine;
                if (aeonCreditResponse.ReceiptFooterMcpy_F0.Contains("<newline>"))
                {
                    foreach (string str in aeonCreditResponse.ReceiptFooterMcpy_F0.Split(new string[] { "<newline>" }, StringSplitOptions.None))
                        mReceiptText += CCGatewayUtils.AllignText(str, Alignment.Center) + Environment.NewLine;
                }
                else
                {
                    mReceiptText += CCGatewayUtils.AllignText(aeonCreditResponse.ReceiptFooterMcpy_F0, Alignment.Center) + Environment.NewLine;
                }
                mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("--MERCHANT COPY--"), Alignment.Center) + Environment.NewLine;
                cReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Thank you"), Alignment.Center) + Environment.NewLine;
                mReceiptText += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Thank you"), Alignment.Center) + Environment.NewLine;

            }
            log.LogMethodExit();
        }
    }

    internal class AEONCreditResponse
    {
        string responseCode;
        string moreIndicator;
        string approvalCode_01;
        string responseText_02;
        string transactionDate_03;
        string transactionTime_04;
        string terminalId_16;
        string panMask_30;
        string cardExpireDate_31;
        string memberExpireDate_32;
        string encryptedPan_33;
        string accountBalance_34;
        string amount_40;
        string cashBackAmount_42;
        string batchNumber_50;
        string uniqueInvoiceNo_65;
        string extendedInvoiceNo_67;
        string customData_99;
        string merchantNameAddress_D0;
        string merchantNumber_D1;
        string cardIssuerName_D2;
        string retrievalReferenceNumber_D3;
        string cardIssuerId_D4;
        string cardHolderName_D5;
        string cardIssuerDate_D6;
        string cardLabel_D7;
        string hostName_D8;
        string stan_D9;
        string aid_E0;
        string applicationProfile_E1;
        string cid_E2;
        string applicationCryptGram_E3;
        string tsi_E4;
        string tvr_E5;
        string cardEntryMode_E6;
        string cashierId_E7;
        string receiptFooterMcpy_F0;
        string receiptFooterCcpy_F1;
        string salesTotal_H0;
        string offlineSalesTotal_H1;
        string voidSalesTotal_H2;
        string cashBackTotal_H3;
        string voidCashBackTotal_H4;
        string refundTotal_H5;
        string voidRefundTotal_H6;
        string settlementSequenceNo_S1;
        string walletProductId_W1;


        /// <summary>
        /// Response code 
        /// </summary>
        public string ResponseCode { get { return responseCode; } set { responseCode = value; } }
        /// <summary>
        /// More indicator  
        /// </summary>
        public string MoreIndicator { get { return moreIndicator; } set { moreIndicator = value; } }
        /// <summary>
        /// Approval code 
        /// </summary>
        public string ApprovalCode_01 { get { return approvalCode_01; } set { approvalCode_01 = value; } }
        /// <summary>
        /// Response text
        /// </summary>
        public string ResponseText_02 { get { return responseText_02; } set { responseText_02 = value; } }
        /// <summary>
        /// Transaction Date YYMMDD
        /// </summary>
        public string TransactionDate_03 { get { return transactionDate_03; } set { transactionDate_03 = value; } }
        /// <summary>
        /// Transaction Time HHMMSS
        /// </summary>
        public string TransactionTime_04 { get { return transactionTime_04; } set { transactionTime_04 = value; } }
        /// <summary>
        /// Terminal Identification Number
        /// </summary>
        public string TerminalId_16 { get { return terminalId_16; } set { terminalId_16 = value; } }
        /// <summary>
        /// PAN (Mask)
        /// </summary>
        public string PanMask_30 { get { return panMask_30; } set { panMask_30 = value; } }
        /// <summary>
        /// Expiration Date (YYMM)
        /// </summary>
        public string CardExpireDate_31 { get { return cardExpireDate_31; } set { cardExpireDate_31 = value; } }
        /// <summary>
        /// Member Expiry Date (YYMM)
        /// </summary>
        public string MemberExpireDate_32 { get { return memberExpireDate_32; } set { memberExpireDate_32 = value; } }
        /// <summary>
        /// Encrypted PAN
        /// </summary>
        public string EncryptedPan_33 { get { return encryptedPan_33; } set { encryptedPan_33 = value; } }
        /// <summary>
        /// Account Balance
        /// </summary>
        public string AccountBalance_34 { get { return accountBalance_34; } set { accountBalance_34 = value; } }
        /// <summary>
        /// Amount, Transaction
        /// </summary>
        public string Amount_40 { get { return amount_40; } set { amount_40 = value; } }
        /// <summary>
        /// Amount, Cash Back
        /// </summary>
        public string CashBackAmount_42 { get { return cashBackAmount_42; } set { cashBackAmount_42 = value; } }
        /// <summary>
        /// Batch Number
        /// </summary>
        public string BatchNumber_50 { get { return batchNumber_50; } set { batchNumber_50 = value; } }
        /// <summary>
        /// Invoice Number
        /// Used to identify a transaction.There can only be one
        /// transaction with a particular value in the batch in the Terminal.
        /// </summary>
        public string UniqueInvoiceNo_65 { get { return uniqueInvoiceNo_65; } set { uniqueInvoiceNo_65 = value; } }
        /// <summary>
        /// Extended Invoice Number
        /// Optional, same as the 3rd party invoice number(e.g.
        /// Wallet invoice number) contained in the custom data of
        /// the original sale transaction response
        /// </summary>
        public string ExtendedInvoiceNo_67 { get { return extendedInvoiceNo_67; } set { extendedInvoiceNo_67 = value; } }
        /// <summary>
        /// Custom data depending on the application (e.g. walletrelated
        /// such as Alipay / WeChat Pay / etc)
        /// </summary>
        public string CustomData_99 { get { return customData_99; } set { customData_99 = value; } }
        /// <summary>
        /// Merchant Name And Address
        /// Organized as 3 lines of 23 characters
        /// </summary>
        public string MerchantNameAddress_D0 { get { return merchantNameAddress_D0; } set { merchantNameAddress_D0 = value; } }
        /// <summary>
        /// Merchant Number
        /// </summary>
        public string MerchantNumber_D1 { get { return merchantNumber_D1; } set { merchantNumber_D1 = value; } }
        /// <summary>
        /// Card Issuer Name
        /// </summary>
        public string CardIssuerName_D2 { get { return cardIssuerName_D2; } set { cardIssuerName_D2 = value; } }
        /// <summary>
        /// Retrieval Reference Number
        /// </summary>
        public string RetrievalReferenceNumber_D3 { get { return retrievalReferenceNumber_D3; } set { retrievalReferenceNumber_D3 = value; } }
        /// <summary>
        /// Card Issuer ID
        ///41 – Visa Credit Card
        ///42 – MasterCard Credit Card
        ///60 – eMoney Card
        ///61 – AEON Pay-V(Visa Credit Card)
        ///62 – AEON Pay-M(Master Credit Card)
        ///63 – AEON Pay-E(E-money)
        /// </summary>
        public string CardIssuerId_D4 { get { return cardIssuerId_D4; } set { cardIssuerId_D4 = value; } }
        /// <summary>
        /// Cardholder Name
        /// </summary>
        public string CardHolderName_D5 { get { return cardHolderName_D5; } set { cardHolderName_D5 = value; } }
        /// <summary>
        /// Card Issuer Date(YYMMDD)
        /// </summary>
        public string CardIssuerDate_D6 { get { return cardIssuerDate_D6; } set { cardIssuerDate_D6 = value; } }
        /// <summary>
        /// Card Label Card preferred name, e.g.VISA / MasterCard
        /// </summary>
        public string CardLabel_D7 { get { return cardLabel_D7; } set { cardLabel_D7 = value; } }
        /// <summary>
        /// Host Name
        /// </summary>
        public string HostName_D8 { get { return hostName_D8; } set { hostName_D8 = value; } }
        /// <summary>
        /// STAN
        /// </summary>
        public string Stan_D9 { get { return stan_D9; } set { stan_D9 = value; } }
        /// <summary>
        /// AID (EMV)
        /// </summary>
        public string Aid_E0 { get { return aid_E0; } set { aid_E0 = value; } }
        /// <summary>
        /// Application Profile (EMV)
        /// </summary>
        public string ApplicationProfile_E1 { get { return applicationProfile_E1; } set { applicationProfile_E1 = value; } }
        /// <summary>
        /// CID (EMV)
        /// </summary>
        public string Cid_E2 { get { return cid_E2; } set { cid_E2 = value; } }
        /// <summary>
        /// Application Cryptogram (EMV)
        /// </summary>
        public string ApplicationCryptGram_E3 { get { return applicationCryptGram_E3; } set { applicationCryptGram_E3 = value; } }
        /// <summary>
        /// TSI (EMV)
        /// </summary>
        public string Tsi_E4 { get { return tsi_E4; } set { tsi_E4 = value; } }
        /// <summary>
        /// TVR (EMV)
        /// </summary>
        public string Tvr_E5 { get { return tvr_E5; } set { tvr_E5 = value; } }
        /// <summary>        
        /// Card Entry Mode
        /// ‘10’ – chip card transaction
        /// ‘20’ – contactless transaction
        /// ‘30’ – swipe transaction
        /// ‘40’ – fallback transaction
        /// ‘50’ – manual transaction
        /// ‘60’ – QR transaction
        /// </summary>
        public string CardEntryMode_E6 { get { return cardEntryMode_E6; } set { cardEntryMode_E6 = value; } }
        /// <summary>
        /// Cashier ID
        /// AEON BIG POS – Print on receipt for Sale, Balance
        /// Inquiry, Refund and Top up transaction
        /// </summary>
        public string CashierId_E7 { get { return cashierId_E7; } set { cashierId_E7 = value; } }
        /// <summary>
        /// Receipt Footer (merchant copy), e.g. PIN VERIFIED,
        /// NO PIN REQUIRED
        /// When it contains below placeholders:
        /// <newline> for new line
        /// <sign> for signature bar
        /// </summary>
        public string ReceiptFooterMcpy_F0 { get { return receiptFooterMcpy_F0; } set { receiptFooterMcpy_F0 = value; } }
        /// <summary>
        /// Receipt Footer (customer copy), e.g. PIN VERIFIED,
        /// NO PIN REQUIRED
        /// When it contains below placeholders:
        /// <newline> for new line
        /// <sign> for signature bar
        /// </summary>
        public string ReceiptFooterCcpy_F1 { get { return receiptFooterCcpy_F1; } set { receiptFooterCcpy_F1 = value; } }
        /// <summary>
        /// Sales Total
        /// </summary>
        public string SalesTotal_H0 { get { return salesTotal_H0; } set { salesTotal_H0 = value; } }
        /// <summary>
        /// Offline Sales Total
        /// </summary>
        public string OfflineSalesTotal_H1 { get { return offlineSalesTotal_H1; } set { offlineSalesTotal_H1 = value; } }
        /// <summary>
        /// Void Sales Total
        /// </summary>
        public string VoidSalesTotal_H2 { get { return voidSalesTotal_H2; } set { voidSalesTotal_H2 = value; } }
        /// <summary>
        /// Cash Back Total
        /// </summary>
        public string CashBackTotal_H3 { get { return cashBackTotal_H3; } set { cashBackTotal_H3 = value; } }
        /// <summary>
        /// Void Cash Back Total
        /// </summary>
        public string VoidCashBackTotal_H4 { get { return voidCashBackTotal_H4; } set { voidCashBackTotal_H4 = value; } }
        /// <summary>
        /// Refund Total
        /// </summary>
        public string RefundTotal_H5 { get { return refundTotal_H5; } set { refundTotal_H5 = value; } }
        /// <summary>
        /// Void Refund Total
        /// </summary>
        public string VoidRefundTotal_H6 { get { return voidRefundTotal_H6; } set { voidRefundTotal_H6 = value; } }
        /// <summary>
        /// Settlement Sequence No
        /// </summary>
        public string SettlementSequenceNo_S1 { get { return settlementSequenceNo_S1; } set { settlementSequenceNo_S1 = value; } }
        /// <summary>
        /// 01 – Alipay
        /// 02 – WeChat CN
        /// 03 – WeChat MY
        /// 04 – Razer
        /// </summary>
        public string WalletProductId_W1 { get { return walletProductId_W1; } set { walletProductId_W1 = value; } }
    }
}
