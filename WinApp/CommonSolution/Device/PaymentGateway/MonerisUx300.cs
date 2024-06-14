using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Semnox.Parafait.TransactionPayments;
using System.Threading;
using System.Text.RegularExpressions;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway.Menories;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class MonerisUx300
    {
        SerialPortHandler serialPortHandler;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string TerminalID = "";
        private string MerchantName = "";
        private string MerchantAddress = "";
        frmUx300Status frmUnattended = null;
        private bool printReceipt;
        private int englishLangId;
        private int frenchLangId;
        private int applicationLanguageId;
        Utilities utilities;

        public MonerisUx300(int comPort, Utilities _Utilities)
        {
            log.LogMethodEntry(comPort, _Utilities);

            utilities = _Utilities;
            serialPortHandler = SerialPortHandler.GetSerialPortHandler("Moneris.log");            
            serialPortHandler.SetPortDetail(comPort, 9600);
            serialPortHandler.OpenVerifonePort();

            log.LogMethodExit(null);
        }

        private void ShowStatusForm()
        {
            log.LogMethodEntry();
            // frmUnattended.TopMost = true;
            frmUnattended.ShowDialog();            
            log.LogMethodExit(null);
        }
        public void setLanguage(int englishLangId, int frenchLangId)
        {
            log.LogMethodEntry(englishLangId, frenchLangId);
            this.englishLangId = englishLangId;
            this.frenchLangId = frenchLangId;
            applicationLanguageId = utilities.ParafaitEnv.LanguageId;
            log.LogMethodExit(null);
        }
        public bool ProcessTransaction(TransactionType tranType, TransactionPaymentsDTO transactionPaymentsDTO, bool printReceipt)
        {
            log.LogMethodEntry(tranType, transactionPaymentsDTO, printReceipt);
            int dataLength = 0;
            List<byte> byteCommand;
            byte[] responseBytes = null;
            List<byte> nakResponse = new List<byte>();
            string amountMessage = "";
            if (transactionPaymentsDTO != null)
            {
                amountMessage = utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                serialPortHandler.WriteToDeviceLog("Starts " + tranType.ToString()+" of amount "+ transactionPaymentsDTO.Amount +".Transaction Id:"+ transactionPaymentsDTO.TransactionId);
            }
            else
            {
                serialPortHandler.WriteToDeviceLog("Starts " + tranType.ToString());
            }
            frmUnattended = new frmUx300Status(amountMessage);
            Thread disPlayMsg = new Thread(ShowStatusForm);
            disPlayMsg.Start();
            try
            {
                if (serialPortHandler.IsPortOpen())
                {
                    serialPortHandler.CloseVerifonePort();
                    Thread.Sleep(200);
                }
                serialPortHandler.OpenVerifonePort();
                //ClearDeviceScreen();
                this.printReceipt = printReceipt;
                switch (tranType)
                {
                    case TransactionType.INIT:
                        frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.InitInprogress);
                        byteCommand = BuildInitializationCommand();
                        serialPortHandler.WriteToDeviceLog("Sending Init() command");
                        if (serialPortHandler.SendCommand(byteCommand, 180000))
                        {
                            serialPortHandler.WriteToDeviceLog("Reading Init() response");
                            if (serialPortHandler.ReadResponse(ref responseBytes, ref dataLength))
                            {
                                if (dataLength < 10)
                                {
                                    nakResponse.Add(responseBytes[9]);
                                    nakResponse.Add(responseBytes[10]);
                                    frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.InitFailed) + " " + GetNotcompleteMessage(nakResponse) + " .";
                                    log.LogMethodExit(null, "Throwing Exception - " + utilities.MessageUtils.getMessage(Common.InitFailed) + " " + GetNotcompleteMessage(nakResponse) + " .");
                                    CancelEvent();
                                    throw new Exception(utilities.MessageUtils.getMessage(Common.InitFailed) + " " + GetNotcompleteMessage(nakResponse) + " .");
                                }
                                else
                                {

                                    TerminalID = GetConfig(0, 16);//Getting terminal id
                                    MerchantName = utilities.ParafaitEnv.SiteName;
                                    MerchantAddress = Common.GetFormatedAddress(utilities.ParafaitEnv.SiteAddress);
                                    
                                    //MerchantName = GetConfig(5, 40);//Getting MerchantName
                                    //MerchantAddress = GetConfig(6, 60);//Getting MerchantAddress
                                    frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.DeviceInitSuccess);
                                    log.LogMethodExit(true);
                                    return true;
                                }
                            }
                            else
                            {
                                frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.ErrorInReadResp);
                                log.LogMethodExit(null, "Throwing Exception - " + utilities.MessageUtils.getMessage(Common.ErrorInReadResp));
                                CancelEvent();
                                throw new Exception(utilities.MessageUtils.getMessage(Common.ErrorInReadResp));
                            }
                        }
                        else
                        {
                            frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.InitFailed) + ".";
                            Thread.Sleep(500);
                            CancelEvent();
                            log.LogMethodExit(null, "Throwing Exception - " + utilities.MessageUtils.getMessage(Common.InitFailed) + ".");
                            throw new Exception(utilities.MessageUtils.getMessage(Common.InitFailed) + ".");
                        }
                    case TransactionType.SALE:
                        frmUnattended.HeaderTextToDisplay = utilities.MessageUtils.getMessage(Common.InsertDebitCreditCard);
                        byteCommand = BuildPurchaseCommand(transactionPaymentsDTO.Amount);
                        serialPortHandler.WriteToDeviceLog("Sending Sale() Command");
                        if (serialPortHandler.SendCommand(byteCommand, 120000))
                        {
                            serialPortHandler.WriteToDeviceLog("Reading Sale() Response");
                            if (serialPortHandler.ReadResponse(ref responseBytes, ref dataLength))
                            {
                                if (dataLength < 10)
                                {
                                    nakResponse.Add(responseBytes[9]);
                                    nakResponse.Add(responseBytes[10]);
                                    frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.TranFailedFor) + " " + GetNotcompleteMessage(nakResponse) + " .";
                                    log.LogMethodExit(null, "Throwing Exception - " + utilities.MessageUtils.getMessage(Common.TranFailedFor) + " " + GetNotcompleteMessage(nakResponse) + " .");
                                    //CancelEvent();
                                    throw new Exception(utilities.MessageUtils.getMessage(Common.TranFailedFor) + " " + GetNotcompleteMessage(nakResponse) + " .");
                                }
                                else
                                {

                                    ReadPurchaseResponse(transactionPaymentsDTO, responseBytes);
                                    log.LogMethodExit(true);
                                    return true;
                                }
                            }
                            else
                            {
                                frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.ErrorInReadResp);
                                log.LogMethodExit(null, "Throwing Exception - " + utilities.MessageUtils.getMessage(Common.ErrorInReadResp));
                                //CancelEvent();
                                throw new Exception(utilities.MessageUtils.getMessage(Common.ErrorInReadResp));
                            }
                        }
                        else
                        {
                            frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.SaleCommandFailed) + ".";
                            Thread.Sleep(500);
                            //CancelEvent();
                            log.LogMethodExit(null, "Throwing Exception - " + utilities.MessageUtils.getMessage(Common.SaleCommandFailed) + ".");
                            throw new Exception(utilities.MessageUtils.getMessage(Common.SaleCommandFailed) + ".");
                        }
                    case TransactionType.REFUND:
                        frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.VoidInProgress);
                        byteCommand = BuildLastRefundCommand();
                        serialPortHandler.WriteToDeviceLog("Sending Refund() Command");
                        if (serialPortHandler.SendCommand(byteCommand, 120000))
                        {
                            serialPortHandler.WriteToDeviceLog("Reading Refund() Response");
                            if (serialPortHandler.ReadResponse(ref responseBytes, ref dataLength))
                            {
                                if (dataLength < 9)
                                {
                                    nakResponse.Add(responseBytes[9]);
                                    nakResponse.Add(responseBytes[10]);
                                    frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.TranFailedFor) + " " + GetNotcompleteMessage(nakResponse) + " .";
                                    log.LogMethodExit(null, "Throwing Exception - " + utilities.MessageUtils.getMessage(Common.TranFailedFor) + " " + GetNotcompleteMessage(nakResponse) + " .");
                                    //CancelEvent();
                                    throw new Exception(utilities.MessageUtils.getMessage(Common.TranFailedFor) + " " + GetNotcompleteMessage(nakResponse) + " .");
                                }
                                else
                                {
                                    ReadRefundResponse(transactionPaymentsDTO, responseBytes);
                                    log.LogMethodExit(true);
                                    return true;
                                }
                            }
                            else
                            {
                                frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.ErrorInReadResp);
                                //CancelEvent();
                                log.LogMethodExit(null, "ProcessTransaction() method " + utilities.MessageUtils.getMessage(Common.ErrorInReadResp));
                                throw new Exception(utilities.MessageUtils.getMessage(Common.ErrorInReadResp));
                            }
                        }
                        else
                        {
                            frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.RefundCommandFailed) + ".";
                            Thread.Sleep(500);
                            //CancelEvent();
                            log.LogMethodExit(null, "ProcessTransaction() method " + utilities.MessageUtils.getMessage(Common.RefundCommandFailed) + ".");
                            throw new Exception(utilities.MessageUtils.getMessage(Common.RefundCommandFailed) + ".");
                        }
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while processing the transaction " + ex.ToString());
                log.Fatal("ProcessTransaction() method " + ex.ToString());
                log.LogMethodExit(null, "Throwing Exception" + ex);
                CancelEvent();
                throw ex;
            }
            finally
            {
                Thread.Sleep(2000);
                ClearDeviceScreen();
                Thread.Sleep(200);
                if (serialPortHandler.IsPortOpen())
                {
                    serialPortHandler.CloseVerifonePort();
                }
                frmUnattended.IsExitTriggered = true;
                utilities.setLanguage(applicationLanguageId);
                serialPortHandler.WriteToDeviceLog("Ends " + tranType.ToString()+((transactionPaymentsDTO!=null)?". ResponseId:"+ transactionPaymentsDTO.CCResponseId:""));
            }
        }


        private string GetNotcompleteMessage(List<byte> nakBytes)
        {
            log.LogMethodEntry(nakBytes);

            string response;
            response = Convert.ToChar(nakBytes[0]).ToString() + Convert.ToChar(nakBytes[1]).ToString();
            switch (response)
            {
                case "01":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.UserCancelled);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "02":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.UserTimeout);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "03":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.ECRCancel);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "04":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.CardRemovedMsg);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "05":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.PinError);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "06":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.ComOpenError);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "07":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.ComRxTimeOut);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "08":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.ComTxTimeOut);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "10":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.SAFFull);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "11":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.SAFTVR);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "12":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.SAFLimited);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                default:
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.UnknownReason);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
            }
        }

        private string GetStatusMessage(string status)
        {
            log.LogMethodEntry(status);

            switch (status)
            {
                case "01":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.TransCompSuccess);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "02":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.TransDeclinedNotComp);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "03":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.TransNotCompleteRevesal);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "04":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.CardNotSupport);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "05":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.TransCanceledByUsr);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                case "06":
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.HostConnError);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                default:
                    {
                        string returnValueNew = utilities.MessageUtils.getMessage(Common.UnknownReason);
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
            }
        }

        private void ReadPurchaseResponse(TransactionPaymentsDTO transactionPaymentsDTO, byte[] responseArray)//, frmStatus fmStatus
        {
            log.LogMethodEntry(transactionPaymentsDTO, responseArray);

            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
            char[] charBytes = Encoding.ASCII.GetChars(responseArray);
            string line = new string(charBytes);
            line = line.Replace("?", "");
            line = line.Replace("\u001c", "~");
            string[] responseStr = line.Split('~');
            ccTransactionsPGWDTO.RecordNo = "None";
            for (int i = 1; i < responseStr.Length; i++)
            {
                switch (responseStr[i].Substring(0, 2))
                {
                    case "01": break;
                    case "02":
                        string dateTime = DateTime.Now.Year.ToString().Substring(0, 2) + responseStr[i].Substring(2, 2) + "-" + responseStr[i].Substring(4, 2) + "-" + responseStr[i].Substring(6, 2) + " " + responseStr[i].Substring(8, 2) + ":" + responseStr[i].Substring(10, 2) + ":" + responseStr[i].Substring(12, 2);
                        ccTransactionsPGWDTO.TransactionDatetime = Convert.ToDateTime(dateTime);
                        break;
                    case "03":
                        ccTransactionsPGWDTO.Authorize = responseStr[i].Substring(2);
                        break;
                    case "04": break;
                    case "05":
                        ccTransactionsPGWDTO.AcctNo = responseStr[i].Substring(2);
                        break;
                    case "06":
                        ccTransactionsPGWDTO.DSIXReturnCode = responseStr[i].Substring(2);
                        break;
                    case "07":
                        ccTransactionsPGWDTO.ResponseOrigin = responseStr[i].Substring(2);
                        break;
                    case "08":
                        ccTransactionsPGWDTO.AuthCode = responseStr[i].Substring(2);
                        break;
                    case "09":
                        ccTransactionsPGWDTO.RefNo = responseStr[i].Substring(2);
                        break;
                    case "10":
                        ccTransactionsPGWDTO.ProcessData = responseStr[i].Substring(2);
                        break;
                    case "11":
                        ccTransactionsPGWDTO.CaptureStatus = responseStr[i].Substring(2);
                        break;
                    case "12":
                        ccTransactionsPGWDTO.CardType = responseStr[i].Substring(2);
                        break;
                    case "13":
                        ccTransactionsPGWDTO.UserTraceData = responseStr[i].Substring(2);
                        break;
                    case "14":
                        ccTransactionsPGWDTO.RecordNo = responseStr[i].Substring(2);
                        break;
                    case "15": ccTransactionsPGWDTO.AcqRefData += "," + responseStr[i]; break;
                    case "16": ccTransactionsPGWDTO.AcqRefData += "," + responseStr[i]; break;
                    case "17": ccTransactionsPGWDTO.AcqRefData += "," + responseStr[i]; break;
                    case "18": ccTransactionsPGWDTO.AcqRefData += "," + responseStr[i]; break;
                    case "19": ccTransactionsPGWDTO.AcqRefData += "," + responseStr[i]; break;
                    case "20": ccTransactionsPGWDTO.AcqRefData += "," + responseStr[i]; break;
                    case "21": ccTransactionsPGWDTO.AcqRefData += "," + responseStr[i]; break;
                    case "22": ccTransactionsPGWDTO.AcqRefData += "," + responseStr[i]; break;
                    case "23": ccTransactionsPGWDTO.AcqRefData += "," + responseStr[i]; break;
                    case "24": break;
                    case "25": break;
                    case "26": break;
                    case "32": ccTransactionsPGWDTO.TextResponse = responseStr[i].Substring(2); break;
                    case "34": ccTransactionsPGWDTO.AcqRefData += "," + responseStr[i]; break;
                }
            }
            ccTransactionsPGWDTO.AcqRefData += ", Device=Ux";
            ccTransactionsPGWDTO.TranCode = "P";
            ccTransactionsPGWDTO.InvoiceNo = transactionPaymentsDTO.TransactionId.ToString();
            ccTransactionsPGWDTO.CustomerCopy = ReceiptText(ccTransactionsPGWDTO);
            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
            ccTransactionsPGWBL.Save();
            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
            transactionPaymentsDTO.Memo = ccTransactionsPGWDTO.CustomerCopy;
            //PrintReceipt(transactionPaymentsDTO, ccTransactionsPGWDTO);
            if (ccTransactionsPGWDTO.TextResponse.Equals("A"))
            {
                frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.Approved);
            }
            else if (ccTransactionsPGWDTO.TextResponse.Equals("a"))
            {
                frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.ApprovedChipMulFunc);//"Approved – Chip Malfunction";
            }
            else if (ccTransactionsPGWDTO.TextResponse.Equals("D"))
            {
                frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.NotApproved); //"Not Approved";
                log.LogMethodExit(null, "Throwing Exception " + Common.NotApproved);
                throw (new Exception(utilities.MessageUtils.getMessage(Common.NotApproved)));
            }
            else if (ccTransactionsPGWDTO.TextResponse.Equals("N"))
            {
                frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.NotComplete);
                log.LogMethodExit(null, "Throwing Exception " + Common.NotComplete);
                throw (new Exception(utilities.MessageUtils.getMessage(Common.NotComplete)));
            }
            else
            {
                frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.TransactionFailed);
                log.LogMethodExit(null, "Throwing Exception " + Common.TransactionFailed);
                throw (new Exception(utilities.MessageUtils.getMessage(Common.TransactionFailed)));
            }

            log.LogMethodExit(null);
        }

        private void ReadRefundResponse(TransactionPaymentsDTO transactionPaymentsDTO, byte[] responseArray)//, frmStatus fmStatus
        {
            log.LogMethodEntry(transactionPaymentsDTO, responseArray);

            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
            List<byte> responseByte = new List<byte>();
            int configLength = responseArray[1] * 256 + responseArray[2];
            responseByte.Add(responseArray[8]);
            responseByte.Add(responseArray[9]);
            responseByte.Add(responseArray[10]);
            char[] charBytes = Encoding.ASCII.GetChars(responseByte.ToArray());
            string line = new string(charBytes);

            if (line.Substring(0, 1).Equals("R"))
            {
                ccTransactionsPGWDTO.TranCode = line.Substring(0, 1);
                ccTransactionsPGWDTO.InvoiceNo = transactionPaymentsDTO.TransactionId.ToString();
                ccTransactionsPGWDTO.DSIXReturnCode = line.Substring(1);
                ccTransactionsPGWDTO.TextResponse = GetStatusMessage(line.Substring(1));
                ccTransactionsPGWDTO.AcqRefData = "Device=Ux";
                ccTransactionsPGWDTO.RecordNo = "None";
                ccTransactionsPGWDTO.TransactionDatetime = DateTime.Now;
                ccTransactionsPGWDTO.CaptureStatus = "";
                ccTransactionsPGWDTO.CustomerCopy= ReceiptText(ccTransactionsPGWDTO);
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                ccTransactionsPGWBL.Save();
                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                
                //PrintReceipt(transactionPaymentsDTO, ccTransactionsPGWDTO);
                if (ccTransactionsPGWDTO.DSIXReturnCode.Equals("01"))
                {
                    frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.VoidApproved);//"Approved";
                }
                else
                {
                    frmUnattended.MessageToDisplay = ccTransactionsPGWDTO.TextResponse;
                    log.LogMethodExit(null, "Throwing Exception - " + ccTransactionsPGWDTO.TextResponse);
                    throw (new Exception(ccTransactionsPGWDTO.TextResponse));
                }
            }
            else
            {
                frmUnattended.MessageToDisplay = utilities.MessageUtils.getMessage(Common.InvalidResponse);//"Invalid Response";
                log.LogMethodExit(null, "Throwing Exception " + Common.InvalidResponse);
                throw (new Exception(utilities.MessageUtils.getMessage(Common.InvalidResponse)));
            }
            log.LogMethodExit(null);
        }

        private void PrintReceipt(TransactionPaymentsDTO transactionPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, ccTransactionsPGWDTO);

            try
            {
                string customerCpy = "";
                string merchantCpy = "";
                customerCpy += Environment.NewLine;
                merchantCpy += Environment.NewLine;
                customerCpy += Environment.NewLine;
                merchantCpy += Environment.NewLine;
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.CaptureStatus)&&(ccTransactionsPGWDTO.CaptureStatus.Equals("T") || ccTransactionsPGWDTO.CaptureStatus.Equals("H")))
                {
                    customerCpy = merchantCpy = Common.AllignText(utilities.MessageUtils.getMessage(Common.NoSignatureRequired), Common.Alignment.Center);
                }
                if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                {
                    if (transactionPaymentsDTO != null && !string.IsNullOrEmpty(transactionPaymentsDTO.Memo))
                    {
                        customerCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CustomerCpyAgreement1), Common.Alignment.Center);
                        customerCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CustomerCpyAgreement2), Common.Alignment.Center);
                        customerCpy += Environment.NewLine;
                        customerCpy += Environment.NewLine;
                        customerCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CustomerCopy), Common.Alignment.Center);
                        if (printReceipt)
                            Common.Print(transactionPaymentsDTO.Memo + customerCpy);
                    }
                }
                //if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y" && !isUnattended)
                //{
                if (transactionPaymentsDTO != null && !string.IsNullOrEmpty(transactionPaymentsDTO.Memo))
                {
                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.CaptureStatus) && !ccTransactionsPGWDTO.CaptureStatus.Equals("T") && !ccTransactionsPGWDTO.CaptureStatus.Equals("H"))
                    {
                        merchantCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Signature), Common.Alignment.Left);
                        merchantCpy += Environment.NewLine + Common.AllignText("x_________________________", Common.Alignment.Left);
                    }
                    merchantCpy += Environment.NewLine;
                    merchantCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.MerchantCpyAgreement), Common.Alignment.Left);
                    merchantCpy += Environment.NewLine;
                    merchantCpy += Environment.NewLine;
                    merchantCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.MerchantCopy), Common.Alignment.Center);

                    // Common.Print(transactionPaymentsDTO.Memo + merchantCpy);
                }
                //}
                transactionPaymentsDTO.Memo = transactionPaymentsDTO.Memo+customerCpy + Environment.NewLine + Environment.NewLine + transactionPaymentsDTO.Memo+ merchantCpy;
            }
            catch(Exception ex)
            {
                log.Error("Error occured while printing the receipt", ex);
            }
        }
        private string ReceiptText(CCTransactionsPGWDTO ccTransactionsPGWDTO)
        {
            log.LogMethodEntry(ccTransactionsPGWDTO);
            string receiptText = "";
            string text = "";
            try
            {
                int languageId = utilities.ParafaitEnv.LanguageId;

                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.ProcessData))
                {
                    if (ccTransactionsPGWDTO.ProcessData.Equals("01"))
                        languageId = englishLangId;
                    else if (ccTransactionsPGWDTO.ProcessData.Equals("02"))
                        languageId = frenchLangId;
                }
                if (utilities.ParafaitEnv.LanguageId != languageId)
                    utilities.setLanguage(languageId);
                List<string> otherFields = (string.IsNullOrEmpty(ccTransactionsPGWDTO.AcqRefData)) ? null : ccTransactionsPGWDTO.AcqRefData.Split(',').ToList<string>();
                List<string> filteredList;
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.UserTraceData) && ccTransactionsPGWDTO.UserTraceData.ToLower().Contains("interac"))
                {
                    receiptText += Common.AllignText(utilities.MessageUtils.getMessage(Common.TransactionRecord), Common.Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine;
                }
                receiptText += Common.AllignText(MerchantName, Common.Alignment.Center);
                receiptText += Environment.NewLine + MerchantAddress;
                receiptText += Environment.NewLine;
                receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Type) + " : " + ((ccTransactionsPGWDTO.TranCode.Equals("P")) ? utilities.MessageUtils.getMessage(Common.Purchase) : utilities.MessageUtils.getMessage(Common.Refund)), Common.Alignment.Left);
                if (ccTransactionsPGWDTO.CardType != null)
                {
                    text = ccTransactionsPGWDTO.UserTraceData + "  ";
                    if (ccTransactionsPGWDTO.CardType.Equals("03"))
                        text += utilities.MessageUtils.getMessage(Common.Savings);
                    else if (ccTransactionsPGWDTO.CardType.Equals("05"))
                        text += utilities.MessageUtils.getMessage(Common.Chequing);


                    //receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ACCT) + "  :    " + utilities.MessageUtils.getMessage(Common.Chequing), Common.Alignment.Left);

                    receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ACCT) + "  : " + text, Common.Alignment.Left);
                }
                receiptText += Environment.NewLine;
                receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Amount) + "  : " + (Convert.ToDouble(ccTransactionsPGWDTO.Authorize) / 100).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Common.Alignment.Left);
                receiptText += Environment.NewLine;
                receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Card) + " #:" + ccTransactionsPGWDTO.AcctNo, Common.Alignment.Left);
                receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage("Date") + " :" + ccTransactionsPGWDTO.TransactionDatetime.ToString("yy/MM/dd") + "  " + utilities.MessageUtils.getMessage("Time") + "  :" + ccTransactionsPGWDTO.TransactionDatetime.ToString("HH:mm:ss"), Common.Alignment.Left);
                receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Reference) + " #:" + TerminalID + ccTransactionsPGWDTO.RefNo + "  " + ccTransactionsPGWDTO.CaptureStatus, Common.Alignment.Left);//+ ccTransactionsPGWDTO.TranCode
                receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Auth) + " #:" + ccTransactionsPGWDTO.AuthCode, Common.Alignment.Left);
                //if (ccTransactionsPGWDTO.CaptureStatus != null)// && ccTransactionsPGWDTO.CaptureStatus.Equals("C")
                //{
                filteredList = otherFields.Where(x => (bool)(!string.IsNullOrEmpty(x) && x.Substring(0, 2).Equals("18"))).ToList<string>();
                if (filteredList != null && filteredList.Count > 0)
                    receiptText += Environment.NewLine + Common.AllignText(filteredList[0].Substring(2), Common.Alignment.Left);
                else
                {
                    filteredList = otherFields.Where(x => (bool)(!string.IsNullOrEmpty(x) && x.Substring(0, 2).Equals("17"))).ToList<string>();
                    if (filteredList != null && filteredList.Count > 0)
                        receiptText += Environment.NewLine + Common.AllignText(filteredList[0].Substring(2), Common.Alignment.Left);
                    else
                    {
                        receiptText += Environment.NewLine + Common.AllignText(ccTransactionsPGWDTO.UserTraceData, Common.Alignment.Left);
                    }
                }

                filteredList = otherFields.Where(x => (bool)(!string.IsNullOrEmpty(x) && x.Substring(0, 2).Equals("16"))).ToList<string>();
                if (filteredList != null && filteredList.Count > 0)
                    receiptText += Environment.NewLine + Common.AllignText(filteredList[0].Substring(2), Common.Alignment.Left);

                filteredList = otherFields.Where(x => (bool)(!string.IsNullOrEmpty(x) && x.Substring(0, 2).Equals("22"))).ToList<string>();
                if (filteredList != null && filteredList.Count > 0)
                    receiptText += Environment.NewLine + Common.AllignText(filteredList[0].Substring(2), Common.Alignment.Left);

                filteredList = otherFields.Where(x => (bool)(!string.IsNullOrEmpty(x) && x.Substring(0, 2).Equals("23"))).ToList<string>();
                if (filteredList != null && filteredList.Count > 0)
                    receiptText += "  " + Common.AllignText(filteredList[0].Substring(2), Common.Alignment.Left);

                //}
                //else 
                //if (ccTransactionsPGWDTO.CaptureStatus != null && ccTransactionsPGWDTO.CaptureStatus.Equals("S"))
                //{
                //receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Auth) + " #:" + ccTransactionsPGWDTO.AuthCode, Common.Alignment.Left);
                //}
                //else 
                receiptText += Environment.NewLine;
                if (ccTransactionsPGWDTO.CaptureStatus != null && ccTransactionsPGWDTO.CaptureStatus.Equals("F"))
                {
                    receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ChipCardSwiped), Common.Alignment.Center);
                    receiptText += Environment.NewLine;
                }
                else if (ccTransactionsPGWDTO.CaptureStatus != null && ccTransactionsPGWDTO.CaptureStatus.Equals("G"))
                {
                    receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ChipCardKeyed), Common.Alignment.Center);
                    receiptText += Environment.NewLine;
                }
                if (ccTransactionsPGWDTO.ResponseOrigin != null && ccTransactionsPGWDTO.ResponseOrigin.Equals("991"))
                {
                    receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CardRemovedMsg) + "-991", Common.Alignment.Center);
                    receiptText += Environment.NewLine;
                }
                else if (ccTransactionsPGWDTO.ResponseOrigin != null && ccTransactionsPGWDTO.ResponseOrigin.Equals("990"))
                {
                    receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.DeclinedByCard) + "-990", Common.Alignment.Center);
                    receiptText += Environment.NewLine;
                }
                if (ccTransactionsPGWDTO.RecordNo != null && ccTransactionsPGWDTO.RecordNo.Equals("Y") && !string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse) && ccTransactionsPGWDTO.TextResponse.ToLower().Equals("a") && !string.IsNullOrEmpty(ccTransactionsPGWDTO.CaptureStatus) && ccTransactionsPGWDTO.CaptureStatus.ToLower().Equals("c"))
                {
                    receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.VerifiedByPin), Common.Alignment.Left);
                    receiptText += Environment.NewLine;
                }
                receiptText += Environment.NewLine + Common.AllignText(((ccTransactionsPGWDTO.ResponseOrigin != null && (ccTransactionsPGWDTO.ResponseOrigin.Equals("990") || ccTransactionsPGWDTO.ResponseOrigin.Equals("991"))) ? "" : (ccTransactionsPGWDTO.DSIXReturnCode + ((string.IsNullOrEmpty(ccTransactionsPGWDTO.DSIXReturnCode) && string.IsNullOrEmpty(ccTransactionsPGWDTO.ResponseOrigin)) ? "" : "/") + ccTransactionsPGWDTO.ResponseOrigin + " ")) + GetResponseMessage(Convert.ToInt32(ccTransactionsPGWDTO.DSIXReturnCode), ccTransactionsPGWDTO.TextResponse, ccTransactionsPGWDTO.ResponseOrigin), Common.Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.DSIXReturnCode) && (ccTransactionsPGWDTO.ResponseOrigin != null && !ccTransactionsPGWDTO.ResponseOrigin.Equals("990") && !ccTransactionsPGWDTO.ResponseOrigin.Equals("991")) && (ccTransactionsPGWDTO.DSIXReturnCode.Equals("01") || ccTransactionsPGWDTO.DSIXReturnCode.Equals("00")))
                {
                    receiptText += Common.AllignText(" " + utilities.MessageUtils.getMessage(Common.ThankYou), Common.Alignment.Left);
                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse) && ccTransactionsPGWDTO.TextResponse.Equals("a"))
                    {
                        receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ChipCardMalFunc), Common.Alignment.Center);
                    }
                }
                receiptText += Environment.NewLine;

                //receiptText += Environment.NewLine + Common.AllignText("*IMPORTANT - retain this copy for your records", Common.Alignment.Left);
                //receiptText += Environment.NewLine;
                //receiptText += Environment.NewLine + Common.AllignText("***CUSTOMER COPY***", Common.Alignment.Center);
            }
            catch(Exception ex)
            {
                log.Error("Error in getting receipt text:" + ex.ToString());
            }
            log.LogMethodExit(receiptText);
            return receiptText;
        }

        private string GetResponseMessage(int code, string response, string responseCode)
        {
            log.LogMethodEntry(code, response, responseCode);

            string responseMsg = "";
            switch(code)
            {
                case 05:
                case 51:
                case 54:
                case 55:
                case 57:
                case 58:
                case 61:
                case 62:
                case 65:
                case 75:
                case 82:
                case 92:
                    responseMsg = utilities.MessageUtils.getMessage(Common.TransactionNotApproved);break;
                default:
                    if (!string.IsNullOrEmpty(responseCode)&& (responseCode.Equals("990") || responseCode.Equals("991")))
                    {
                        responseMsg = utilities.MessageUtils.getMessage(Common.TransactionNotCompleted);
                    }
                    else
                    {
                        responseMsg = (response.Equals("A") ? utilities.MessageUtils.getMessage(Common.Approved) : (response.Equals("a") ? utilities.MessageUtils.getMessage(Common.Approved) : (response.Equals("D") ? utilities.MessageUtils.getMessage(Common.TransactionNotApproved) : response.Equals("N") ? utilities.MessageUtils.getMessage(Common.TransactionNotCompleted) : utilities.MessageUtils.getMessage(Common.Unknown))));
                    }
                    break;
            }
            log.LogMethodExit(responseMsg);
            return responseMsg;
        }
        private void ClearDeviceScreen()
        {
            log.LogMethodEntry();
            byte[] response = null;
            int len = 0;
            bool state;
            List<byte> byteCommand;
            byteCommand = BuildClearDisplayCommand();
            serialPortHandler.WriteToDeviceLog("Sending ClearScreen() Command");
            state = serialPortHandler.SendCommand(byteCommand);
            if (state)
            {
                serialPortHandler.WriteToDeviceLog("Reading ClearScreen() Response");
                serialPortHandler.ReadResponse(ref response, ref len);
                byteCommand = BuildDisplayWelcomeCommand();
                serialPortHandler.WriteToDeviceLog("Sending DisplayWelcome() Command");
                state = serialPortHandler.SendCommand(byteCommand);
                serialPortHandler.WriteToDeviceLog("Reading DisplayWelcome() Response");
                serialPortHandler.ReadResponse(ref response, ref len);
                serialPortHandler.ClearPortBuffer();
            }

            log.LogMethodEntry(null);
        }
        private void CancelEvent()
        {
            log.LogMethodEntry();

            try
            {
                bool state;
                byte[] response = null;
                int len = 0;
                List<byte> byteCommand;
                byteCommand = BuildCancelCommand();
                serialPortHandler.WriteToDeviceLog("Sending CancelEvent() Command");
                state = serialPortHandler.SendCommand(byteCommand);
                serialPortHandler.WriteToDeviceLog("Reading CancelEvent() Response");
                serialPortHandler.ReadResponse(ref response, ref len);
            }
            catch(Exception ex)
            {
                log.Error("Error occured while cancelling the evenet", ex);
            }

            log.LogMethodExit(null);
        }

        private string GetConfig(int configId, int maxLength)
        {
            log.LogMethodEntry(configId, maxLength);

            string data = "";
            int dataLength = 0;
            byte[] responseBytes = null;
            List<byte> byteCommand = null;
            List<byte> nakResponse = new List<byte>();
            byteCommand = BuildGetconfigCommand(configId, maxLength);
            serialPortHandler.WriteToDeviceLog("Sending GetConfig("+ configId + ") Command");
            if (serialPortHandler.SendCommand(byteCommand))
            {
                serialPortHandler.WriteToDeviceLog("Reading GetConfig(" + configId + ") Response");
                if (serialPortHandler.ReadResponse(ref responseBytes, ref dataLength))
                {
                    if (dataLength < 10)
                    {
                        nakResponse.Add(responseBytes[9]);
                        nakResponse.Add(responseBytes[10]);
                        log.LogMethodExit(null, "Throwing Exception - " + utilities.MessageUtils.getMessage(Common.GetConfigFailedFor) + GetNotcompleteMessage(nakResponse) + " .");
                        throw new Exception(utilities.MessageUtils.getMessage(Common.GetConfigFailedFor) + GetNotcompleteMessage(nakResponse) + " .");
                    }
                    else
                    {
                        data = ReadGonfigResponse(ref responseBytes);
                    }
                }
                else
                {
                    log.LogMethodEntry("Throwing Exception - " + Common.ErrorInReadResp);
                    throw new Exception(utilities.MessageUtils.getMessage(Common.ErrorInReadResp));
                }
            }
            else
            {
                log.LogMethodEntry("Throwing Exception - "  + Common.ReadConfigFailed);
                throw new Exception(utilities.MessageUtils.getMessage(Common.ReadConfigFailed));
            }
            log.LogMethodExit(data);
            return data;
        }

        private string ReadGonfigResponse(ref byte[] responseBytes)
        {
            log.LogMethodEntry(responseBytes);

            int configLength = responseBytes[8] * 256 + responseBytes[9];

            char[] charBytes;
            byte[] dataByte = new byte[configLength];

            for (int i = 10; i < 10 + configLength; i++)
            {
                dataByte[i - 10] = responseBytes[i];
            }
            charBytes = Encoding.ASCII.GetChars(dataByte);
            string line = new string(charBytes);
            line = line.Replace("?", "");
            line = line.Replace("\u001c", "~");

            log.LogMethodExit(line);
            return line;
        }
        private List<byte> BuildGetconfigCommand(int configId, int maxLength)
        {
            log.LogMethodEntry(configId, maxLength);

            List<byte> command = new List<byte>();
            command.Add(SerialPortHandler.STX);
            command.Add(0x00);
            command.Add(0x07);
            command.Add(0x02);
            command.Add(0x0D);
            command.Add(0x00);
            command.Add(0x04);
            command.Add(Convert.ToByte(configId));
            command.Add(0x00);
            command.Add(Convert.ToByte(maxLength));
            command.Add(SerialPortHandler.ETX);

            log.LogMethodExit(command);
            return command;
        }
        private List<byte> BuildClearDisplayCommand()
        {
            log.LogMethodEntry();

            List<byte> command = new List<byte>();
            command.Add(SerialPortHandler.STX);
            command.Add(0x00);
            command.Add(0x04);
            command.Add(0x02);
            command.Add(0x02);
            command.Add(0x00);
            command.Add(0x02);
            command.Add(SerialPortHandler.ETX);

            log.LogMethodExit(command);
            return command;
        }

        private List<byte> BuildDisplayWelcomeCommand()
        {
            log.LogMethodEntry();

            byte[] trxnMsgByte;
            List<byte> command = new List<byte>();
            int size;
            trxnMsgByte = Encoding.ASCII.GetBytes("<br><br><center>WELCOME</center>");//</br>
            size = 8 + trxnMsgByte.Length;
            command.Add(SerialPortHandler.STX);
            command.Add(0x00);
            command.Add(Convert.ToByte(size));
            command.Add(0x02);
            command.Add(0x02);
            command.Add(0x00);
            command.Add(0x00);
            command.Add(0x00);
            command.Add(0x00);
            command.Add(0x00);
            command.Add(Convert.ToByte(trxnMsgByte.Length));
            foreach (byte b in trxnMsgByte)
                command.Add(b);
            command.Add(SerialPortHandler.ETX);

            log.LogMethodExit(command);
            return command;
        }

        private List<byte> BuildInitializationCommand()
        {
            log.LogMethodEntry();

            List<byte> command = new List<byte>();
            command.Add(SerialPortHandler.STX);
            command.Add(0x00);
            command.Add(0x05);
            command.Add(0x02);
            command.Add(0x0D);
            command.Add(0x00);
            command.Add(0x00);
            command.Add(0x49);
            command.Add(SerialPortHandler.ETX);

            log.LogMethodExit(command);
            return command;
        }

        private List<byte> BuildLastRefundCommand()
        {
            log.LogMethodEntry();

            List<byte> command = new List<byte>();
            command.Add(SerialPortHandler.STX);
            command.Add(0x00);
            command.Add(0x05);
            command.Add(0x02);
            command.Add(0x0D);
            command.Add(0x00);
            command.Add(0x00);
            command.Add(0x52);
            command.Add(SerialPortHandler.ETX);

            log.LogMethodExit(command);
            return command;
        }

        private List<byte> BuildCancelCommand()
        {
            log.LogMethodEntry();

            List<byte> command = new List<byte>();
            command.Add(SerialPortHandler.STX);
            command.Add(0x00);
            command.Add(0x04);
            command.Add(0x02);
            command.Add(0x0D);
            command.Add(0x00);
            command.Add(0x01);
            command.Add(SerialPortHandler.ETX);

            log.LogMethodExit(command);
            return command;
        }

        private List<byte> BuildPurchaseCommand(double amount)
        {
            log.LogMethodEntry(amount);

            long tranAmount = Convert.ToInt64(amount.ToString("0.00").Replace(".", ""));
            byte[] trxnAmountByte;
            List<byte> command = new List<byte>();
            int size;
            trxnAmountByte = Encoding.ASCII.GetBytes(tranAmount.ToString().PadLeft(6, '0'));
            size = 5 + trxnAmountByte.Length;
            command.Add(SerialPortHandler.STX);
            command.Add(0x00);
            command.Add(Convert.ToByte(size));
            command.Add(0x02);
            command.Add(0x0D);
            command.Add(0x00);
            command.Add(0x00);
            command.Add(0x50);
            foreach (byte b in trxnAmountByte)
                command.Add(b);
            command.Add(SerialPortHandler.ETX);

            log.LogMethodExit(command);
            return command;
        }
    }
}
