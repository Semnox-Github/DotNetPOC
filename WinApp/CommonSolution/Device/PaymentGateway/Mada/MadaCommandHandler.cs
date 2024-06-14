/********************************************************************************************
 * Project Name - Mada Command Handler
 * Description  - Data handler of the MadaPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *2.100.0     26-Sep-2020   Dakshakh        Created  
 ********************************************************************************************/

using System;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Runtime.InteropServices;
using Semnox.Parafait.Device.PaymentGateway.Mada;
using System.Collections.Generic;
using System.Xml;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class MadaCommandHandler
    {
        protected Utilities utilities;
        protected string merchantId;
        protected TransactionPaymentsDTO transactionPaymentsDTO;
        private Dictionary<string, string> creditCardSchemeIdMap = new Dictionary<string, string>();
        [DllImport(@"madaapi_v1_5.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr api_RequestCOMTrxnXML(int port, int Rate, int x, int y, int z, byte[] inOutBuff, byte[] intval,  int trnxType, byte[] ECR_Flag, byte[] panNo, byte[] purAmount, byte[] stanNo, byte[] dataTime, byte[] expDate, byte[] trxRrn, byte[] authCode, byte[] rspCode, byte[] terminalId, byte[] schemeId, byte[] merchantId, byte[] addtlAmount, byte[] ecrrefno, byte[] version, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder outResp, byte[] outRespLen);

        [DllImport(@"madaapi_v1_5.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr api_CheckStatus(int bPort, int dwBaudRate, int bParity, int bDataBits, int bStopBits, byte[] inReqBuff, byte[] inReqLen);

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MadaCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO);
            this.utilities = utilities;
            this.transactionPaymentsDTO = transactionPaymentsDTO;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Perform Transaction
        /// </summary>
        /// <param name="madaRequest"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public MadaResponse PerformTransaction(MadaRequest madaRequest, ref string Message)
        {
            log.LogMethodEntry(madaRequest, Message);
            try
            {
                LoadCreditCardSchemeIdMap();
                string result = CreateRequest(madaRequest);
                int retryCount = 0;
                if (!string.IsNullOrEmpty(result))
                {
                    byte[] intval = new byte[1];
                    byte[] ECR_Flag = new byte[1];
                    byte[] inOutBuff = System.Text.Encoding.ASCII.GetBytes(result);
                    byte[] intv = new byte[1];
                    byte[] panNo = new byte[23];
                    byte[] purAmount = new byte[13];
                    byte[] stanNo = new byte[7];
                    byte[] dataTime = new byte[15];
                    byte[] expDate = new byte[5];
                    byte[] trxRrn = new byte[13];
                    byte[] authCode = new byte[7];
                    byte[] rspCode = new byte[4];
                    byte[] terminalId = new byte[17];
                    byte[] schemeId = new byte[3];
                    byte[] merchantId = new byte[16];
                    byte[] addtlAmount = new byte[13];
                    byte[] ecrrefno = new byte[17];
                    byte[] version = new byte[10];
                    byte[] outRespLen = new byte[1];
                    intval[0] = (byte)inOutBuff.Length;
                    ECR_Flag[0] = (byte)0;
                    StringBuilder outResp = new StringBuilder(15000);
                    MadaResponse madaResponse = null;

                    switch (madaRequest.TrxType)
                    {
                        case PaymentGatewayTransactionType.SALE:
                            var saleResult = api_RequestCOMTrxnXML(Convert.ToInt32(madaRequest.BPort), 38400, 0, 8, 0, inOutBuff, intval, 0, ECR_Flag,  panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                            madaResponse = ConvertResponse(panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                            if (madaResponse != null && madaResponse.ResponseCode != null)
                            {
                                CreateResponse(madaResponse, Convert.ToString(PaymentGatewayTransactionType.SALE));
                            }
                            return madaResponse;

                        case PaymentGatewayTransactionType.REFUND:
                            while (madaResponse == null)
                            {
                                retryCount++;
                                if(retryCount >= 2)
                                {
                                    System.Threading.Thread.Sleep(2000);
                                }
                                var refundResult = api_RequestCOMTrxnXML(Convert.ToInt32(madaRequest.BPort), 38400, 0, 8, 0, inOutBuff, intval, 2, ECR_Flag, panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                                madaResponse = ConvertResponse(panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                                if (madaResponse != null && madaResponse.ResponseCode != null)
                                {
                                    CreateResponse(madaResponse, Convert.ToString(PaymentGatewayTransactionType.REFUND));
                                }
                                if (retryCount >= 2)
                                {
                                    break;
                                }
                            }
                            return madaResponse;
                        case PaymentGatewayTransactionType.VOID:
                            var voidResult = api_RequestCOMTrxnXML(Convert.ToInt32(madaRequest.BPort), 38400, 0, 8, 0, inOutBuff, intval, 3, ECR_Flag, panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                            madaResponse = ConvertResponse(panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                            if (madaResponse != null && madaResponse.ResponseCode != null)
                            {
                                CreateResponse(madaResponse, Convert.ToString(PaymentGatewayTransactionType.VOID));
                            }
                            return madaResponse;
                        case PaymentGatewayTransactionType.PARING:
                            while (madaResponse == null || !madaResponse.ResponseCode.Equals("0"))
                            {
                                retryCount++;
                                var outp = api_CheckStatus(Convert.ToInt32(madaRequest.BPort), 38400, 0, 8, 0, inOutBuff, intval);
                                madaResponse = new MadaResponse(null, 0, 0, null, null, null, null, null, outp.ToString(), null, null, null, null, null, null, null, null, null, null);
                                madaResponse.ResponseCode = outp.ToString();
                                if (madaResponse.ResponseCode != null)
                                {
                                    madaResponse.ResponseText = GetResponseText(madaResponse.ResponseCode);
                                }
                                if (retryCount >= 3)
                                {
                                    break;
                                }
                            }
                            return madaResponse;
                        default:
                            return madaResponse;
                    }
                }
                else
                {
                    log.LogMethodExit(null);
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while communicating with MADA", ex);
                log.LogMethodExit(null);
                return null;
            }
        }

        private void LoadCreditCardSchemeIdMap()
        {
            log.LogMethodEntry();
            try
            {
                creditCardSchemeIdMap.Add("P1", "mada / SPAN");
                creditCardSchemeIdMap.Add("MC", "Master Card");
                creditCardSchemeIdMap.Add("DM", "Maestro Card");
                creditCardSchemeIdMap.Add("VC", "Visa Card");
                creditCardSchemeIdMap.Add("AX", "American Express Card");
                creditCardSchemeIdMap.Add("UP", "Union Pay");
                creditCardSchemeIdMap.Add("GN", "GCCNET Card");
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Loading CreditCard SchemeId", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Create Request
        /// </summary>
        /// <param name="madaRequest"></param>
        /// <returns></returns>
        public string CreateRequest(MadaRequest madaRequest)
        {
            string retString = "";
            try
            {
                if (madaRequest != null)
                {
                    switch (madaRequest.TrxType)
                    {
                        case PaymentGatewayTransactionType.SALE:
                            retString = madaRequest.PurAmount.ToString() + ";" + madaRequest.PrintReciept.ToString() + ";" + madaRequest.EcrRefNumber + "!";
                            return retString;
                        case PaymentGatewayTransactionType.REFUND:
                            string originalRefundDate = madaRequest.OriginalRefundDate.ToString("ddMMyyyy");
                            retString = madaRequest.RefundAmount.ToString() + ";" + madaRequest.TrxRrn + ";" + madaRequest.PrintReciept.ToString() + ";" + originalRefundDate + ";" + madaRequest.EcrRefNumber + "!";
                            return retString;
                        case PaymentGatewayTransactionType.VOID:
                            retString = madaRequest.PrintReciept.ToString() + ";" + madaRequest.EcrRefNumber + "!";
                            return retString;
                        case PaymentGatewayTransactionType.PARING:
                            retString = "14!";
                            return retString;
                        default:
                            return retString;
                    }
                }
                else
                {
                    log.LogMethodExit(retString);
                    return retString;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while creating response", ex);
                log.LogMethodExit(retString);
                return retString;
            }
        }

        private MadaResponse ConvertResponse(byte[] panNo, byte[] purAmount, byte[] stanNo, byte[] dataTime, byte[] expDate, byte[] trxRrn, byte[] authCode,
                                             byte[] rspCode, byte[] terminalId, byte[] schemeId, byte[] merchantId, byte[] addtlAmount, byte[] ecrrefno, byte[] version,
                                             StringBuilder outResp, byte[] outRespLen)
        {
            log.LogMethodEntry(panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
            MadaResponse transactionResponse = null;
            try
            {
                string creditCardName = "";
                if (creditCardSchemeIdMap.ContainsKey(ByteTostring(schemeId)))
                {
                    creditCardSchemeIdMap.TryGetValue(ByteTostring(schemeId), out creditCardName);
                }
                transactionResponse = new MadaResponse(ByteTostring(panNo), (Convert.ToDecimal(ByteTostring(purAmount)) / 100), Convert.ToDecimal(ByteTostring(addtlAmount)), ByteTostring(stanNo),
                                                                        ByteTostring(dataTime), ByteTostring(expDate), ByteTostring(trxRrn), ByteTostring(authCode), ByteTostring(rspCode), ByteTostring(terminalId),
                                                                        creditCardName, ByteTostring(merchantId), ByteTostring(ecrrefno), ByteTostring(version), Convert.ToString(outResp), ByteTostring(outRespLen), null, null, null);
                log.Debug("Ends-ConvertResponse() method");
            }
            catch (Exception ex)
            {
                log.Error("Error occured while converting response", ex);
            }
            log.LogMethodExit(transactionResponse);
            return transactionResponse;
        }

        private string ByteTostring(byte[] byteData)
        {
            log.LogMethodEntry(byteData);
            string stringData = "";
            try
            {
                stringData = Encoding.Default.GetString(byteData);
                if (!string.IsNullOrEmpty(stringData))
                {
                    stringData = stringData.Substring(0, (stringData.Length - 1));
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while converting Byte to String", ex);
            }
            log.LogMethodExit(stringData);
            return stringData;
        }

        private void CreateResponse(MadaResponse madaResponse, string trxType)
        {
            log.LogMethodEntry(madaResponse, trxType);
            try
            {
                madaResponse.PanNo = madaResponse.PanNo != null ? new string('X', 12) + madaResponse.PanNo.Substring(12,4) : string.Empty;
                string formatString = "yyyyMMddHHmmss";
                string captureStatus = getCaptureStatus(madaResponse);
                madaResponse.CCTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, null, madaResponse.StanNo, null, madaResponse.ResponseCode, -1, GetResponseText(madaResponse.ResponseCode), madaResponse.PanNo,
                                                                             madaResponse.SchemeId, trxType, madaResponse.TrxRrn, utilities.ParafaitEnv.POSMachine.ToString(), Convert.ToString(madaResponse.PurAmount),
                                                                             madaResponse.TrxDateTime != null ? DateTime.ParseExact(madaResponse.TrxDateTime, formatString, null) : DateTime.MinValue, madaResponse.AuthCode, madaResponse.TerminalId, null, madaResponse.VersionNumber, captureStatus, null, null, null, null,null);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while creating response", ex);
            }
            log.LogMethodExit();
        }

        private string getCaptureStatus(MadaResponse madaResponse)
        {
            log.LogMethodEntry(madaResponse);
            string captureStatus = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                if (madaResponse.OutReciept != null)
                {
                    xmlDoc.LoadXml(madaResponse.OutReciept); // suppose that myXmlString contains "<Names>...</Names>"
                    XmlNodeList xnList = xmlDoc.SelectNodes("/TransactionResponse");
                    foreach (XmlNode xn in xnList)
                    {
                        captureStatus = xn["EntryMode"] != null ? xn["EntryMode"].InnerText : " ";
                    }
                }
                log.LogMethodExit(captureStatus);
                return captureStatus;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while getting entry mode", ex);
                log.LogMethodExit(captureStatus);
                return captureStatus;
            }
        }

        private string GetResponseText(string response)
        {
            log.LogMethodEntry(response);
            string responseText = "";
            try
            {
                switch (response)
                {
                    case "0":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Success");
                        break;
                    case "-1":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Library Failed");
                        break;
                    case "-2":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "No Response Received");
                        break;
                    case "-3":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Not able to open port");
                        break;
                    case "-4":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Acknowledgement Failed");
                        break;
                    case "1":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Terminal TMS not Loaded ");
                        break;
                    case "2":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Blocked Card");
                        break;
                    case "3":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "No Active Application Found");
                        break;
                    case "4":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card Read Error");
                        break;
                    case "5":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Insert Card Only");
                        break;
                    case "6":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Maximum Amount Limit Exceeded");
                        break;
                    case "7":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "PIN Quit");
                        break;
                    case "8":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "User Cancelled or Timeout");
                        break;
                    case "9":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card Scheme Not Supported");
                        break;
                    case "10":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Terminal Busy");
                        break;
                    case "11":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Paper Out");
                        break;
                    case "12":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "No Reconciliation Record found");
                        break;
                    case "13":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Transaction Cancelled");
                        break;
                    case "14":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "De-SAF Processing");
                        break;
                    case "000":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Approved");
                        break;
                    case "001":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Honor with identification & Approved");
                        break;
                    case "003":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Approved(VIP)");
                        break;
                    case "007":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Approved , update ICC(To be used when a response includes an issuer script. This code is used for SPAN only.)");
                        break;
                    case "087":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Offline Approved(Chip only)  Approve");
                        break;
                    case "089":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Unable to go On-line.Off - line approved(Chip only)  Approve");
                        break;
                    case "100":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Do not honor Decline  101  Expired card  Decline");
                        break;
                    case "102":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Suspected fraud(To be used when ARQC validation fails) Decline");
                        break;
                    case "103":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card acceptor contact acquirer  Decline  104  Restricted card  Decline");
                        break;
                    case "105":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card acceptor call acquirer’s security department Decline");
                        break;
                    case "106":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Allowable PIN tries exceeded, Decline");
                        break;
                    case "107":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Refer to card issuer  Decline");
                        break;
                    case "108":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Refer to card issuer’s special conditions Decline");
                        break;
                    case "109":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Invalid merchant  Decline");
                        break;
                    case "110":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Invalid amount  Decline");
                        break;
                    case "111":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Invalid card number Decline");
                        break;
                    case "112":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "PIN data required Decline");
                        break;
                    case "114":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "No account of type requested Decline");
                        break;
                    case "115":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Requested function not supported  Decline");
                        break;
                    case "116":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Not sufficient funds Decline");
                        break;
                    case "117":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Incorrect PIN  Decline");
                        break;
                    case "118":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "No card record Decline");
                        break;
                    case "119":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Transaction not permitted to cardholder Decline");
                        break;
                    case "120":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Transaction not permitted to terminal Decline");
                        break;
                    case "121":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Exceeds withdrawal amount limit  Decline");
                        break;
                    case "122":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Security violation  Decline");
                        break;
                    case "123":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Exceeds withdrawal frequency limit  Decline");
                        break;
                    case "125":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card not effective Decline");
                        break;
                    case "126":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Invalid PIN block Decline");
                        break;
                    case "127":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "PIN length error Decline");
                        break;
                    case "128":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "PIN key synch error  Decline");
                        break;
                    case "182":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Invalid date(Visa 80)  Decline");
                        break;
                    case "183":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Cryptographic error found in PIN or CVV(Visa 81)  Decline");
                        break;
                    case "184":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Incorrect CVV(Visa 82)  Decline ");
                        break;
                    case "185":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Unable to verify PIN(Visa 83)  Decline");
                        break;
                    case "188":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Offline declined  Decline");
                        break;
                    case "190":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Unable to go online – Offline declined  Decline");
                        break;
                    case "200":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Do not honor Decline");
                        break;
                    case "201":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Expired card  Decline");
                        break;
                    case "202":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Suspected fraud(To be used when ARQC validation fails) Decline");
                        break;
                    case "203":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card acceptor contact acquirer  Decline");
                        break;
                    case "204":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Restricted card, Decline");
                        break;
                    case "205":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card acceptor call acquirer’s security department, Decline");
                        break;
                    case "206":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Allowable PIN tries exceeded  Decline");
                        break;
                    case "207":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Special conditions  Decline ");
                        break;
                    case "208":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Lost card, Decline");
                        break;
                    case "209":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Stolen card  Decline");
                        break;
                    case "210":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Suspected counterfeit card Decline");
                        break;
                    case "400":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Accepted");
                        break;
                    case "902":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Invalid transaction, Decline");
                        break;
                    case "903":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Re - enter transaction Decline");
                        break;
                    case "904":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Format error  Decline");
                        break;
                    case "906":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Cutover in process Decline");
                        break;
                    case "907":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card issuer or switch inoperative  Decline");
                        break;
                    case "908":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Transaction destination cannot be found for routing Decline");
                        break;
                    case "909":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "System malfunction  Decline");
                        break;
                    case "910":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card issuer signed off  Decline");
                        break;
                    case "911":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card issuer timed out  Decline");
                        break;
                    case "912":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Card issuer unavailable  Decline");
                        break;
                    case "913":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Duplicate transmission  Decline");
                        break;
                    case "914":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Not able to trace back to original transaction  Decline");
                        break;
                    case "915":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Reconciliation cutover or checkpoint error  Decline");
                        break;
                    case "916":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "MAC incorrect(permissible in 1644)  Decline");
                        break;
                    case "917":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "MAC key sync  Decline ");
                        break;
                    case "918":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "No communication keys available for use Decline  ");
                        break;
                    case "919":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Encryption key sync error  Decline");
                        break;
                    case "920":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Security software / hardware error – try again Decline");
                        break;
                    case "921":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Security software/ hardware error – no action  Decline");
                        break;
                    case "922":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Message number out of sequence  Decline ");
                        break;
                    case "940":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Unknown terminal  Decline");
                        break;
                    case "942":
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Invalid Reconciliation Date");
                        break;
                    default:
                        responseText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Invalid Response code &1", response);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while creating response", ex);
            }
            log.LogMethodExit(responseText);
            return responseText;
        }
    }
}
