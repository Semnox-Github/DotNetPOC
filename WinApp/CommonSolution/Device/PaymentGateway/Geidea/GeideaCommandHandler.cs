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
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Threading;
using System.Configuration;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class GeideaCommandHandler
    {
        //// define the device params
        //private const int port = 10;
        //private const int baudRate = 38400;
        //private const int parityBit = 0;
        //private const int dataBit = 8;
        //private const int stopBit = 0;
        private IDisplayStatusUI statusDisplayUi;
        Semnox.Core.Utilities.ExecutionContext ExecutionContext;

        public GeideaCommandHandler(IDisplayStatusUI statusDisplayUi, Semnox.Core.Utilities.ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.statusDisplayUi = statusDisplayUi;
            this.ExecutionContext = executionContext;
            BuildTrxTypeMap();
            LoadCreditCardSchemeIdMap();
            LoadResponseTestMapv1_9();
            log.LogMethodExit();
        }

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //GeideaResponseDTO madaResponse = null;
        //private string log_string = string.Empty;
        //private string bufferString = string.Empty;

        private Dictionary<string, string> creditCardSchemeIdMap = new Dictionary<string, string>();
        private Dictionary<string, string> ResponseTextMap = new Dictionary<string, string>();
        private Dictionary<int, string> TrxTypeMap = new Dictionary<int, string>();


        // importing the DLL; check notes doc for the procedure to import the DLL
        [DllImport(@"madaapi_v1_9.dll", CallingConvention = CallingConvention.StdCall)] // CallingConvention.StdCall=> The callee (the method being called) cleans the stack.
        public static extern IntPtr api_RequestCOMTrxn(int port, int Rate, int x, int y, int z, byte[] inOutBuff, byte[] intval, int trnxType, byte[] panNo, byte[] purAmount, byte[] stanNo, byte[] dataTime, byte[] expDate, byte[] trxRrn, byte[] authCode, byte[] rspCode, byte[] terminalId, byte[] schemeId, byte[] merchantId, byte[] addtlAmount, byte[] ecrrefno, byte[] version, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder outResp, byte[] outRespLen);

        [DllImport(@"madaapi_v1_9.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr api_CheckStatus(int port, int Rate, int x, int y, int z, byte[] inOutBuff, byte[] intval);

        [DllImport(@"madaapi_v1_9.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr api_GetCOMTerminalID(int port, int Rate, int x, int y, int z, byte[] inOutBuff, byte[] intval, byte[] terminalId);

        [DllImport(@"madaapi_v1_9.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr api_GetECRDetails(int port, int Rate, int x, int y, int z, byte[] inOutBuff, byte[] intval, byte[] ecrData, byte[] ecrPrinter, byte[] ecrSpeed);

        [DllImport(@"madaapi_v1_9.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr api_RequestCOMRepeat(int port, int Rate, int x, int y, int z, byte[] inOutBuff, byte[] intval, byte[] panNo, byte[] purAmount, byte[] stanNo, byte[] dataTime, byte[] expDate, byte[] trxRrn, byte[] authCode, byte[] rspCode, byte[] terminalId, byte[] schemeId, byte[] merchantId, byte[] addtlAmount, byte[] ecrrefno, byte[] version, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder outResp, byte[] outRespLen);

        //public static extern IntPtr api_CommTest(BYTE bPort, DWORD dwBaudRate, BYTE bParity, BYTE bDataBits,
        //BYTE bStopBits, unsigned char* inReqBuff, int* inReqLen);

        public GeideaResponseDTO PerformTransaction(GeideaRequestDTO requestDTO, string bufferString)
        {
            log.LogMethodEntry(requestDTO, bufferString);
            GeideaResponseDTO madaResponse = null;
            string outrespone = null;
            //LoadResponseTextMap();
            byte[] intval = new byte[1];
            byte[] panNo = new byte[23];
            byte[] purAmount = new byte[13];
            byte[] stanNo = new byte[7];
            byte[] dataTime = new byte[13];
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
            StringBuilder outResp = new StringBuilder(30000);
            IntPtr Ret;
            bool performLastTrxCheck = false;
            string trxErrorMsq = string.Empty;
            string lastTrxCheckErrorMsq = string.Empty;

            try
            {
                byte[] inOutBuff = Encoding.ASCII.GetBytes(bufferString);
                intval[0] = (byte)inOutBuff.Length;

                // calling the method
                if (requestDTO.trnxType != 21)
                {
                    //trnx other than REPEAT
                    log.Debug("Intiating Request");
                    Thread.Sleep(1000);
                    Ret = api_RequestCOMTrxn(requestDTO.port, requestDTO.baudRate, requestDTO.parityBit, requestDTO.dataBit, requestDTO.stopBit, inOutBuff, intval, requestDTO.trnxType, panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                    log.Debug("Received API return value: " + Ret.ToString());
                }
                else
                {
                    // REPEAT trnx
                    log.Debug("Intiating Request for Last trx check");
                    Thread.Sleep(1000);
                    Ret = api_RequestCOMRepeat(requestDTO.port, requestDTO.baudRate, requestDTO.parityBit, requestDTO.dataBit, requestDTO.stopBit, inOutBuff, intval, panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                    log.Debug("Received API return value: " + Ret.ToString());
                }
                if (Ret != null)
                {
                    if (Ret.ToString().Equals("0"))
                    {
                        // getting the response in desired format
                        log.Debug("Getting the response in desired format");
                        madaResponse = ConvertResponse(panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                        log.Debug("Response object received");
                        // getting the response text from the response code -- Dak - 000 failuers verify
                        if (madaResponse != null && !string.IsNullOrWhiteSpace(madaResponse.ResponseCode))
                        {
                            // madaResponse.ResponseText = GetResponseText(madaResponse.ResponseCode.ToString());
                            //  Check for success code
                            log.Debug("Response Code: " + madaResponse.ResponseCode);
                            madaResponse.ResponseText = GetResponseText(madaResponse.ResponseCode.ToString());
                            //If TrxType is not equal to last trx check(Repeat)
                            if (requestDTO.trnxType != 21)
                            {
                                log.Debug("Checking Response is of Current Request");
                                string requestId = requestDTO.ecrRefNumber;
                                log.Debug("Requested ecrNo: " + requestId);
                                string ecrReqNo = string.Empty;
                                ecrReqNo = string.IsNullOrWhiteSpace(madaResponse.EcrRefNo) ? "" : madaResponse.EcrRefNo.Substring(madaResponse.EcrRefNo.Length - requestId.Length);
                                log.Debug("Response ecrNo: " + ecrReqNo);
                                if (ecrReqNo != requestId)
                                {
                                    string errormessage = "Something went Wrong. Please try Again.";
                                    log.Error("response ecrReqNo != requestId. Incorrect Response received");
                                    throw new Exception(errormessage);
                                }
                                log.Debug("Validation successfull");
                            }
                        }
                        else
                        {
                            log.Error("Response not received");
                            throw new Exception("Response not received");
                        }
                        log.LogVariableState("Response", madaResponse);
                    }
                    else if (Ret.ToString().Equals("-2") || Ret.ToString().Equals("16"))
                    {
                        trxErrorMsq = GetResponseText(Ret.ToString());
                        performLastTrxCheck = true;
                    }
                    else
                    {
                        string result = GetResponseText(Ret.ToString());
                        log.Error($"Error: {result}");
                        throw new Exception(result);
                    }
                }
                else
                {
                    performLastTrxCheck = true;
                }
            }
            catch (Exception ex)
            {
                trxErrorMsq = ex.Message;
                log.Error($"Error: {trxErrorMsq}");
                log.Error(ex);
                //log_string = "Exception occured in performTrnx: " + ex.Message;
                //GeideaDbConnection.SaveLog(module_name: "purchase method", log_string, GetTimeStamp());
                //Console.WriteLine("Exception occured in performTrnx: " + ex.Message);
                throw ex;
            }
            if (performLastTrxCheck)
            {
                try
                {
                    log.Debug("Last trx check Initiated");
                    GeideaResponseDTO lastTrxcheckgeideaResponseDTO = PerformLastTrxCheck(requestDTO.isPrintReceiptEnabled, requestDTO.terminalID, requestDTO.port, requestDTO.baudRate, requestDTO.parityBit, requestDTO.dataBit, requestDTO.stopBit);
                    if (lastTrxcheckgeideaResponseDTO == null)
                    {
                        log.Error("Trx not found");
                        throw new Exception("Trx not found");
                        //throw new Exception($"Last Trx Check failed");
                    }
                    log.Debug("Received Last trx Check Response");
                    string requestId = requestDTO.ecrRefNumber;
                    log.Debug("requestId : " + requestId);
                    string ecrReqNo = lastTrxcheckgeideaResponseDTO.EcrRefNo.Substring(lastTrxcheckgeideaResponseDTO.EcrRefNo.Length - requestId.Length);
                    log.Debug("ecrReqNo: " + ecrReqNo);

                    if (ecrReqNo != requestId)
                    {
                        log.Error("Trx not found");
                        throw new Exception("Trx not found");
                    }
                    madaResponse = lastTrxcheckgeideaResponseDTO;
                }
                catch (Exception ex)
                {
                    lastTrxCheckErrorMsq = ex.Message;
                    log.Error(ex);
                    string errorMessage = string.IsNullOrWhiteSpace(trxErrorMsq) ? "" :
                        $"Error in {GetTrxType(requestDTO.trnxType)} trx: {trxErrorMsq}";
                    errorMessage += string.IsNullOrWhiteSpace(errorMessage) ? "" : " | " + $"Error in Last trx check: {ex.Message}";
                    throw new Exception(errorMessage);
                }

            }

            log.LogMethodExit(madaResponse);
            return madaResponse;
        }

        public GeideaResponseDTO PerformLastTrxCheck(int isPrintReceiptEnabled, string deviceId, int port, int baudRate, int parityBit, int dataBit, int stopBit)
        {

            log.LogMethodEntry(isPrintReceiptEnabled, deviceId, port, baudRate, parityBit, dataBit, stopBit);
            GeideaResponseDTO lastTrxCheckResponse = null;
            try
            {
                byte[] intval = new byte[1];
                byte[] panNo = new byte[23];
                byte[] purAmount = new byte[13];
                byte[] stanNo = new byte[7];
                byte[] dataTime = new byte[13];
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
                StringBuilder outResp = new StringBuilder(30000);
                IntPtr Ret;

                string bufferString = "0B";
                GeideaRequestDTO requestDTO = new GeideaRequestDTO(
                              trnxAmount: null,
                              naqdAmount: null,
                              refundAmount: null,
                              isPrintReceiptEnabled: isPrintReceiptEnabled, // printing is disabled
                              trxnRrn: null,
                              originalRefundDate: null,
                              ecrRefNumber: null,
                              cardNumber: null,
                              trxnApprovalNumber: null,
                              isUnattended: false,
                              trnxType: 21, // Repeat
                              terminalID: deviceId,
                              cashierNo: null,
                              repeatCommandRequestBuffer: bufferString,
                              port: port,
                              baudRate: baudRate,
                              parityBit: parityBit,
                              dataBit: dataBit,
                              stopBit: stopBit,
                              paymentId: null
                              );

                CheckDeviceStatus(requestDTO);

                byte[] inOutBuff = Encoding.ASCII.GetBytes(bufferString);
                intval[0] = (byte)inOutBuff.Length;
                log.Debug("Intiating Request for Last trx check");
                statusDisplayUi.DisplayText(MessageContainerList.GetMessage(ExecutionContext, "Processing..Please wait..."));
                Thread.Sleep(1000);
                Ret = api_RequestCOMRepeat(requestDTO.port, requestDTO.baudRate, requestDTO.parityBit, requestDTO.dataBit, requestDTO.stopBit, inOutBuff, intval, panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                log.Debug("Received API return value: " + Ret.ToString());

                if (Ret != null)
                {
                    if (Ret.ToString().Equals("0"))
                    {
                        // getting the response in desired format
                        log.Debug("Getting the response in desired format");
                        lastTrxCheckResponse = ConvertResponse(panNo, purAmount, stanNo, dataTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
                        log.Debug("Response object received");
                        // getting the response text from the response code -- Dak - 000 failuers verify
                        if (lastTrxCheckResponse != null && !string.IsNullOrWhiteSpace(lastTrxCheckResponse.ResponseCode))
                        {
                            // madaResponse.ResponseText = GetResponseText(madaResponse.ResponseCode.ToString());
                            //  Check for success code
                            log.Debug("Response Code: " + lastTrxCheckResponse.ResponseCode);
                            lastTrxCheckResponse.ResponseText = GetResponseText(lastTrxCheckResponse.ResponseCode.ToString());
                            //If TrxType is not equal to last trx check(Repeat)
                        }
                        else
                        {
                            log.Error("Response not received");
                            throw new Exception("Response not received");
                        }
                        log.LogVariableState("Response", lastTrxCheckResponse);
                    }
                    else
                    {
                        string result = GetResponseText(Ret.ToString());
                        log.Error($"Error: {result}");
                        throw new Exception(result);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }


            log.LogMethodExit(lastTrxCheckResponse);
            return lastTrxCheckResponse;
        }
        public string GetRequestString(GeideaRequestDTO requestDTO)
        {
            log.LogMethodEntry();
            string bufferString = GetBufferString(requestDTO);
            if (string.IsNullOrWhiteSpace(bufferString))
            {
                log.Error($"Error: BufferString was empty");
                // BufferString is empty
                //Console.WriteLine("Error: BufferString is empty");
                throw new Exception("Error: BufferString was empty");
            }
            log.LogMethodExit(bufferString);
            return bufferString;
        }
        public GeideaResponseDTO MakeTransactionRequest(GeideaRequestDTO requestDTO, string bufferString)
        {
            log.LogMethodEntry(requestDTO, bufferString);
            GeideaResponseDTO result = null;

            try
            {
                log.LogVariableState("bufferString", bufferString);
                if (!string.IsNullOrWhiteSpace(bufferString))
                {
                    statusDisplayUi.DisplayText(MessageContainerList.GetMessage(ExecutionContext, "Processing..Please wait..."));
                    result = PerformTransaction(requestDTO, bufferString);
                }
                else
                {
                    log.Error($"Error: BufferString was empty");
                    // BufferString is empty
                    //Console.WriteLine("Error: BufferString is empty");
                    throw new Exception("Error: BufferString was empty");
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error: {ex.ToString()}");
                log.Error(ex);
                //Console.WriteLine("Exception in MakeTransactionRequest: " + ex.Message);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        private string GetBufferString(GeideaRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            string TextString = string.Empty;
            try
            {
                if (requestDTO != null)
                {
                    switch (requestDTO.trnxType)
                    {
                        case 0:
                            //trnxType = Purchase
                            TextString = string.Format("{0};{1};{2}!", requestDTO.trnxAmount, requestDTO.isPrintReceiptEnabled, requestDTO.ecrRefNumber);
                            break;
                        case 1:
                            //trnxType = Purchase with Cashback
                            TextString = string.Format("{0};{1};{2};{3}!", requestDTO.trnxAmount, requestDTO.naqdAmount, requestDTO.isPrintReceiptEnabled, requestDTO.ecrRefNumber);
                            break;
                        case 2:
                            //trnxType = Refund
                            // Refund textString has two signatures; with cardNo and without cardNo

                            //without cardNo
                            TextString = string.Format("{0};{1};{2};{3};{4}!", requestDTO.refundAmount, requestDTO.trxnRrn, requestDTO.originalRefundDate, requestDTO.isPrintReceiptEnabled, requestDTO.ecrRefNumber);

                            //with cardNo
                            //TextString = string.Format("{0};{1};{2};{3};{4};{5}!", requestDTO.refundAmount, requestDTO.trxnRrn, requestDTO.originalRefundDate, requestDTO.cardNumber, requestDTO.isPrintReceiptEnabled, requestDTO.ecrRefNumber);
                            break;
                        case 3:
                            //trnxType = Reversal
                            TextString = string.Format("{0};{1}!", requestDTO.isPrintReceiptEnabled, requestDTO.ecrRefNumber);
                            break;
                        case 12:
                            //trnxType = Authorization
                            TextString = string.Format("{0};{1};{2}!", requestDTO.trnxAmount, requestDTO.isPrintReceiptEnabled, requestDTO.ecrRefNumber);
                            break;
                        case 13:
                            //trnxType = Advice
                            TextString = string.Format("{0};{1};{2};{3}!", requestDTO.trnxAmount, requestDTO.trxnApprovalNumber, requestDTO.isPrintReceiptEnabled, requestDTO.ecrRefNumber);
                            break;
                        case 21:
                            //trnxType = Repeat
                            TextString = string.Format("{0}!", requestDTO.repeatCommandRequestBuffer);
                            break;
                        default:
                            //trnxType = Repeat
                            TextString = string.Empty;
                            break;
                    }
                }
                else
                {
                    log.Error("RequestDTO was null");
                    //Console.WriteLine("RequestDTO is null");
                    throw new Exception("RequestDTO was null");
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception in generating buffer string: ");
                log.Error(ex);
                //Console.WriteLine("Exception in generating buffer string: " + ex.Message);
                throw new Exception("Exception in generating buffer string: " + ex.Message);
            }
            log.LogMethodExit(TextString);
            return TextString;
        }

        public GeideaResponseDTO CheckDeviceStatus(GeideaRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            log.Debug("Checking device status");
            GeideaResponseDTO madaResponse = null;
            //string response = string.Empty;
            try
            {

                log.Debug("Finished Loading Response Text Map");
                byte[] inOutBuff = Encoding.ASCII.GetBytes("04!");
                byte[] intval = new byte[1];
                intval[0] = (byte)inOutBuff.Length;
                log.Debug("inOutBuff value :" + inOutBuff);
                log.Debug("inOutBuff value :" + intval);
                log.Debug("intval[0] value :" + intval[0]);
                log.Debug("Enters into api_CheckStatus call ");
                int result = 3000;
                string waitPeriod = ConfigurationManager.AppSettings["GeideaDeviceStatusWaitPeriod"];
                if (int.TryParse(waitPeriod, out result) == false)
                {
                    result = 3000;
                }
                log.Debug("waitPeriod :" + waitPeriod);
                string deviceStatus = string.Empty;
                int counter = 0;
                string removeCardMessage = MessageContainerList.GetMessage(ExecutionContext, 5162);//Please remove the Card if it has not already been removed.s

                do
                {
                    counter++;
                    string message = MessageContainerList.GetMessage(ExecutionContext, 5163, GetTrxType(requestDTO.trnxType) + ": " + counter + Environment.NewLine + (counter > 3 ? removeCardMessage : string.Empty));
                    statusDisplayUi.DisplayText(message); //Checking Device Status... Attempting &1
                    Thread.Sleep(result);
                    log.Debug("Checking Device Status Attempt: " + counter);
                    var outp = api_CheckStatus(requestDTO.port, 38400, requestDTO.parityBit, 8, 0, inOutBuff, intval);
                    deviceStatus = outp.ToString().Trim();
                    log.Debug("Received Device Status Response: " + deviceStatus);
                } while ((deviceStatus == "11" || deviceStatus == "-4") && counter < 10);


                madaResponse = new GeideaResponseDTO(null, 0, 0, null, null, null, null, null, deviceStatus.ToString(), null, null, null, null, null, null, null, null);
                madaResponse.ResponseCode = deviceStatus.ToString();
                log.Debug("Final Device Status Response: " + deviceStatus);
                if (madaResponse != null && string.IsNullOrWhiteSpace(madaResponse.ResponseCode) == false)
                {
                    madaResponse.ResponseText = GetResponseText(madaResponse.ResponseCode);
                    log.Debug("Response Text :" + madaResponse.ResponseText);
                    log.LogVariableState("madaResponse.ResponseText value", madaResponse.ResponseText);
                }
                else
                {
                    log.Error("Device Status Response was null");
                    log.LogVariableState("Response was null", 1);
                    throw new Exception("Response was null");
                }
                if (madaResponse.ResponseCode != "0")
                {
                    log.Error($"Error: {madaResponse.ResponseText}");
                    throw new Exception(madaResponse.ResponseText);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occured in CheckDeviceStatus:" + ex);
                log.Error(ex);
                //Console.WriteLine("Exception occured in CheckDeviceStatus: " + ex.Message);
                throw ex;
            }
            log.Debug("Device status check completed");
            log.LogMethodExit(madaResponse);
            return madaResponse;
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
                //Console.WriteLine("Error occured while Loading CreditCard SchemeId", ex.Message);
                throw new Exception("Error occured while Loading CreditCard SchemeId", ex);

            }
            log.LogMethodExit(creditCardSchemeIdMap);
        }


        private GeideaResponseDTO ConvertResponse(byte[] panNo, byte[] purAmount, byte[] stanNo, byte[] dateTime, byte[] expDate, byte[] trxRrn, byte[] authCode, byte[] rspCode, byte[] terminalId, byte[] schemeId, byte[] merchantId, byte[] addtlAmount, byte[] ecrrefno, byte[] version, StringBuilder outResp, byte[] outRespLen)
        {
            log.LogMethodEntry(panNo, purAmount, stanNo, dateTime, expDate, trxRrn, authCode, rspCode, terminalId, schemeId, merchantId, addtlAmount, ecrrefno, version, outResp, outRespLen);
            GeideaResponseDTO transactionResponse = null;
            try
            {
                // get Credit Card Type from schemeId
                string creditCardName = "";
                if (creditCardSchemeIdMap.ContainsKey(ByteTostring(schemeId)))
                {
                    creditCardSchemeIdMap.TryGetValue(ByteTostring(schemeId), out creditCardName);
                }
                else
                {
                    creditCardName = "Other";
                }
                log.LogVariableState("creditCardName", creditCardName);
                string pan = Encoding.Default.GetString(trxRrn);
                // formulate TrnxResponse from the Constructor
                string purchaseAmount = ConvertByteToString(purAmount);
                string addAmount = ConvertByteToString(addtlAmount);
                log.LogVariableState("pan", pan);
                log.LogVariableState("purchaseAmount", purchaseAmount);
                log.LogVariableState("addAmount", addAmount);
                transactionResponse = new GeideaResponseDTO(ConvertByteToString(panNo), (Convert.ToDecimal(string.IsNullOrWhiteSpace(purchaseAmount) ? "0" : purchaseAmount) / 100), Convert.ToDecimal(Convert.ToDecimal(string.IsNullOrWhiteSpace(addAmount) ? "0" : addAmount)), ConvertByteToString(stanNo), ConvertByteToString(dateTime), ConvertByteToString(expDate), ConvertByteToString(trxRrn), ConvertByteToString(authCode), ConvertByteToString(rspCode), ConvertByteToString(terminalId), creditCardName, ConvertByteToString(merchantId), ConvertByteToString(ecrrefno), ConvertByteToString(version), Convert.ToString(outResp), ConvertByteToString(outRespLen), null);

            }
            catch (Exception ex)
            {
                log.Error("Error occured while converting response: ", ex);
                //Console.WriteLine("Error occured while converting response: ", ex.Message);
                throw ex;
            }
            log.LogMethodExit(transactionResponse);
            return transactionResponse;
        }

        private string ConvertByteToString(byte[] byteObject)
        {
            log.LogMethodEntry(byteObject);
            string retValue = ByteTostring(byteObject);
            retValue = retValue.Replace("\0", string.Empty);
            log.LogMethodExit(retValue);
            return retValue;
        }

        private string ByteTostring(byte[] byteData)
        {
            log.LogMethodEntry(byteData);

            string stringData = "";
            try
            {
                stringData = Encoding.UTF8.GetString(byteData);
                if (!string.IsNullOrEmpty(stringData))
                {
                    stringData = stringData.Substring(0, (stringData.Length - 1));
                }
                else
                {
                    throw new Exception("Byte to string conversion failed");
                }
                log.LogVariableState("stringData", stringData);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while converting Byte to String", ex);
                //Console.WriteLine("Error occured while converting Byte to String", ex.Message);
                throw new Exception("Error occured while converting Byte to String", ex);
            }
            log.LogMethodExit(stringData);
            return stringData;
        }

        private void LoadResponseTestMapv1_9()
        {
            log.Debug("Loading Response Map");
            log.LogMethodEntry();
            try
            {
                ResponseTextMap.Add("0", "Success");
                ResponseTextMap.Add("-1", "Library Failed");
                ResponseTextMap.Add("-2", "No Response Received");
                ResponseTextMap.Add("-3", "Not able to open port");
                ResponseTextMap.Add("-4", "Acknowledgement Failed");
                ResponseTextMap.Add("-5", "Wrong Buffer");
                ResponseTextMap.Add("1", "Terminal TMS not Loaded");
                ResponseTextMap.Add("2", "Blocked Card");
                ResponseTextMap.Add("3", "No Active Application Found");
                ResponseTextMap.Add("4", "Card Read Error");
                ResponseTextMap.Add("5", "Insert Card Only");
                ResponseTextMap.Add("6", "Maximum Amount Limit Exceeded");
                ResponseTextMap.Add("7", "PIN Quit");
                ResponseTextMap.Add("8", "User Cancelled or Timeout");
                ResponseTextMap.Add("9", "Data Error");
                ResponseTextMap.Add("10", "Card Scheme Not Supported");
                ResponseTextMap.Add("11", "Terminal Busy");
                ResponseTextMap.Add("12", "Paper Out");
                ResponseTextMap.Add("13", "No Reconciliation Record found");
                ResponseTextMap.Add("14", "De-SAF Processing");
                ResponseTextMap.Add("15", "Transaction Not Allowed");
                ResponseTextMap.Add("16", "Transaction Timeout");
                ResponseTextMap.Add("17", "LAN Cable not Connected");
                ResponseTextMap.Add("18", "SIM Not present in device");
                ResponseTextMap.Add("19", "GPRS Down in Device");
                ResponseTextMap.Add("20", "LAN Connection UP");
                ResponseTextMap.Add("21", "GPRS UP in device");
                ResponseTextMap.Add("22", "Reconciliation Failed");
                ResponseTextMap.Add("23", "Idle Screen");
                ResponseTextMap.Add("24", "Magstripe Not allowed");
                ResponseTextMap.Add("25", "DE-SAF Failed");
                ResponseTextMap.Add("26", "No Transaction to Reverse");
                ResponseTextMap.Add("27", "Reversal Transaction Not Allowed");
                ResponseTextMap.Add("28", "Refund PAN not Matching");
                ResponseTextMap.Add("29", "WIFI Down");
                ResponseTextMap.Add("30", "Invalid Password");
                ResponseTextMap.Add("31", "Card Removed");
                ResponseTextMap.Add("32", "Network is Down");

                // Additional data
                ResponseTextMap.Add("000", "Approved");
                ResponseTextMap.Add("001", "Honor with identification");
                ResponseTextMap.Add("003", "Approved (VIP)");
                ResponseTextMap.Add("007", "Approved, update ICC (To be used when a response includes an issuer script. This code is used for SPAN only.)");
                ResponseTextMap.Add("087", "Offline Approved (Chip only)");
                ResponseTextMap.Add("089", "Unable to go On-line. Off-line approved (Chip only)");
                ResponseTextMap.Add("100", "Do not honor");
                ResponseTextMap.Add("101", "Expired card");
                ResponseTextMap.Add("102", "Suspected fraud (To be used when ARQC validation fails)");
                ResponseTextMap.Add("103", "Card acceptor contact acquirer");
                ResponseTextMap.Add("104", "Restricted card");
                ResponseTextMap.Add("105", "Card acceptor call acquirer’s security department");
                ResponseTextMap.Add("106", "Allowable PIN tries exceeded");
                ResponseTextMap.Add("107", "Refer to card issuer");
                ResponseTextMap.Add("108", "Refer to card issuer’s special conditions");
                ResponseTextMap.Add("109", "Invalid merchant");
                ResponseTextMap.Add("110", "Invalid amount");
                ResponseTextMap.Add("111", "Invalid card number");
                ResponseTextMap.Add("112", "PIN data required");
                ResponseTextMap.Add("114", "No account of type requested");
                ResponseTextMap.Add("115", "Requested function not supported");
                ResponseTextMap.Add("116", "Not sufficient funds");
                ResponseTextMap.Add("117", "Incorrect PIN");
                ResponseTextMap.Add("118", "No card record");
                ResponseTextMap.Add("119", "Transaction not permitted to cardholder");
                ResponseTextMap.Add("120", "Transaction not permitted to terminal");
                ResponseTextMap.Add("121", "Exceeds withdrawal amount limit");
                ResponseTextMap.Add("122", "Security violation");
                ResponseTextMap.Add("123", "Exceeds withdrawal frequency limit");
                ResponseTextMap.Add("125", "Card not effective");
                ResponseTextMap.Add("126", "Invalid PIN block");
                ResponseTextMap.Add("127", "PIN length error");
                ResponseTextMap.Add("128", "PIN key sync error");
                ResponseTextMap.Add("129", "Suspected counterfeit card");
                ResponseTextMap.Add("182", "Invalid date (Visa 80)");
                ResponseTextMap.Add("183", "Cryptographic error found in PIN or CVV (Visa 81)");
                ResponseTextMap.Add("184", "Incorrect CVV (Visa 82)");
                ResponseTextMap.Add("185", "Unable to verify PIN (Visa 83)");
                ResponseTextMap.Add("187", "Original transaction for refund, preauthorization capture, preauthorization void or preauthorization extension not found based on original transaction data.");
                ResponseTextMap.Add("188", "Offline declined");
                ResponseTextMap.Add("190", "Unable to go online – Offline declined");
                ResponseTextMap.Add("199", "The refund, pre-authorization capture, preauthorization void or preauthorization extension transaction period exceeds the maximum time limit allowed by the mada business rules.");
                ResponseTextMap.Add("200", "Do not honor");
                ResponseTextMap.Add("201", "Expired card");
                ResponseTextMap.Add("202", "Suspected fraud (To be used when ARQC validation fails)");
                ResponseTextMap.Add("203", "Card acceptor contact acquirer");
                ResponseTextMap.Add("204", "Restricted card");
                ResponseTextMap.Add("205", "Card acceptor call acquirer’s security department");
                ResponseTextMap.Add("206", "Allowable PIN tries exceeded");
                ResponseTextMap.Add("207", "Special conditions");
                ResponseTextMap.Add("208", "Lost card");
                ResponseTextMap.Add("209", "Stolen card");
                ResponseTextMap.Add("210", "Suspected counterfeit card");
                ResponseTextMap.Add("400", "Accepted");
                ResponseTextMap.Add("902", "Invalid transaction");
                ResponseTextMap.Add("903", "Re-enter transaction");
                ResponseTextMap.Add("904", "Format error");
                ResponseTextMap.Add("906", "Cutover in process");
                ResponseTextMap.Add("907", "Card issuer or switch inoperative");
                ResponseTextMap.Add("908", "Transaction destination cannot be found for routing");
                ResponseTextMap.Add("909", "System malfunction");
                ResponseTextMap.Add("910", "Card issuer signed off");
                ResponseTextMap.Add("911", "Card issuer timed out");
                ResponseTextMap.Add("912", "Card issuer unavailable");
                ResponseTextMap.Add("913", "Duplicate transmission");
                ResponseTextMap.Add("914", "Not able to trace back to original transaction");
                ResponseTextMap.Add("915", "Reconciliation cutover or checkpoint error");
                ResponseTextMap.Add("916", "MAC incorrect (permissible in 1644)");
                ResponseTextMap.Add("917", "MAC key sync");
                ResponseTextMap.Add("918", "No communication keys available for use");
                ResponseTextMap.Add("919", "Encryption key sync error");
                ResponseTextMap.Add("920", "Security software/hardware error – try again");
                ResponseTextMap.Add("921", "Security software/hardware error – no action");
            }
            catch (Exception ex)
            {
                log.Debug("Failed to Load Response Map");
                log.Error(ex);
            }
            log.LogMethodExit();
            log.Debug("Completed Loading");

        }

        //private void LoadResponseTextMap()
        //{
        //    log.LogMethodEntry();
        //    try
        //    {
        //        ResponseTextMap.Add("0", "Success");
        //        ResponseTextMap.Add("-1", "Library Failed");
        //        ResponseTextMap.Add("-2", "No Response Received");
        //        ResponseTextMap.Add("-3", "Not able to open port");
        //        ResponseTextMap.Add("-4", "Acknowledgement Failed");
        //        ResponseTextMap.Add("1", "Terminal TMS not Loaded");
        //        ResponseTextMap.Add("2", "Blocked Card");
        //        ResponseTextMap.Add("3", "No Active Application Found");
        //        ResponseTextMap.Add("4", "Card Read Error");
        //        ResponseTextMap.Add("5", "Insert Card Only");
        //        ResponseTextMap.Add("6", "Maximum Amount Limit Exceeded");
        //        ResponseTextMap.Add("7", "PIN Quit");
        //        ResponseTextMap.Add("8", "User Cancelled or Timeout");
        //        ResponseTextMap.Add("9", "Data Error");
        //        ResponseTextMap.Add("10", "Card Scheme Not Supported");
        //        ResponseTextMap.Add("11", "Terminal Busy");
        //        ResponseTextMap.Add("12", "Paper Out");
        //        ResponseTextMap.Add("13", "No Reconciliation Record found");
        //        ResponseTextMap.Add("14", "Transaction Cancelled");
        //        ResponseTextMap.Add("15", "De-SAF Processing");
        //        ResponseTextMap.Add("16", "Transaction Not Allowed");
        //        ResponseTextMap.Add("17", "Reconciliation Failed");
        //        ResponseTextMap.Add("000", "Approved");
        //        ResponseTextMap.Add("001", "Honor with identification");
        //        ResponseTextMap.Add("003", "Approved (VIP)");
        //        ResponseTextMap.Add("007", "Approved, update ICC (To be used when a response includes an issuer script. This code is used for SPAN only.)");
        //        ResponseTextMap.Add("087", "Offline Approved (Chip only)");
        //        ResponseTextMap.Add("089", "Unable to go On-line. Off-line approved (Chip only)");
        //        ResponseTextMap.Add("100", "Do not honor");
        //        ResponseTextMap.Add("101", "Expired card");
        //        ResponseTextMap.Add("102", "Suspected fraud (To be used when ARQC validation fails)");
        //        ResponseTextMap.Add("103", "Card acceptor contact acquirer");
        //        ResponseTextMap.Add("104", "Restricted card ");
        //        ResponseTextMap.Add("105", "Card acceptor call acquirer’s security department ");
        //        ResponseTextMap.Add("106", "Allowable PIN tries exceeded ");
        //        ResponseTextMap.Add("107", "Refer to card issuer ");
        //        ResponseTextMap.Add("108", "Refer to card issuer’s special conditions ");
        //        ResponseTextMap.Add("109", "Invalid merchant ");
        //        ResponseTextMap.Add("110", "Invalid amount ");
        //        ResponseTextMap.Add("111", "Invalid card number ");
        //        ResponseTextMap.Add("112", "PIN data required ");
        //        ResponseTextMap.Add("114", "No account of type requested ");
        //        ResponseTextMap.Add("115", "Requested function not supported ");
        //        ResponseTextMap.Add("116", "Not sufficient funds ");
        //        ResponseTextMap.Add("117", "Incorrect PIN ");
        //        ResponseTextMap.Add("118", "No card record ");
        //        ResponseTextMap.Add("119", "Transaction not permitted to cardholder ");
        //        ResponseTextMap.Add("120", "Transaction not permitted to terminal ");
        //        ResponseTextMap.Add("121", "Exceeds withdrawal amount limit ");
        //        ResponseTextMap.Add("122", "Security violation ");
        //        ResponseTextMap.Add("123", "Exceeds withdrawal frequency limit ");
        //        ResponseTextMap.Add("125", "Card not effective ");
        //        ResponseTextMap.Add("126", "Invalid PIN block ");
        //        ResponseTextMap.Add("127", "PIN length error ");
        //        ResponseTextMap.Add("128", "PIN key synch error ");
        //        ResponseTextMap.Add("129", "Suspected counterfeit card ");
        //        ResponseTextMap.Add("182", "Invalid date (Visa 80) ");
        //        ResponseTextMap.Add("183", "Cryptographic error found in PIN or CVV (Visa 81) ");
        //        ResponseTextMap.Add("184", "Incorrect CVV (Visa 82) ");
        //        ResponseTextMap.Add("185", "Unable to verify PIN (Visa 83) ");
        //        ResponseTextMap.Add("188", "Offline declined");
        //        ResponseTextMap.Add("190", "Unable to go online – Offline declined ");
        //        ResponseTextMap.Add("200", "Do not honor ");
        //        ResponseTextMap.Add("201", "Expired card ");
        //        ResponseTextMap.Add("202", "Suspected fraud (To be used when ARQC validation fails) ");
        //        ResponseTextMap.Add("203", "Card acceptor contact acquirer ");
        //        ResponseTextMap.Add("204", "Restricted card ");
        //        ResponseTextMap.Add("205", "Card acceptor call acquirer’s security department ");
        //        ResponseTextMap.Add("206", "Allowable PIN tries exceeded ");
        //        ResponseTextMap.Add("207", "Special conditions ");
        //        ResponseTextMap.Add("208", "Lost card ");
        //        ResponseTextMap.Add("209", "Stolen card ");
        //        ResponseTextMap.Add("210", "Suspected counterfeit card ");
        //        ResponseTextMap.Add("400", "Accepted Accepted");
        //        ResponseTextMap.Add("902", "Invalid transaction ");
        //        ResponseTextMap.Add("903", "Re-enter transaction ");
        //        ResponseTextMap.Add("904", "Format error ");
        //        ResponseTextMap.Add("906", "Cutover in process ");
        //        ResponseTextMap.Add("907", "Card issuer or switch inoperative ");
        //        ResponseTextMap.Add("908", "Transaction destination cannot be found for routing ");
        //        ResponseTextMap.Add("909", "System malfunction ");
        //        ResponseTextMap.Add("910", "Card issuer signed off ");
        //        ResponseTextMap.Add("911", "Card issuer timed out ");
        //        ResponseTextMap.Add("912", "Card issuer unavailable ");
        //        ResponseTextMap.Add("913", "Duplicate transmission ");
        //        ResponseTextMap.Add("914", "Not able to trace back to original transaction ");
        //        ResponseTextMap.Add("915", "Reconciliation cutover or checkpoint error ");
        //        ResponseTextMap.Add("916", "MAC incorrect (permissible in 1644) ");
        //        ResponseTextMap.Add("917", "MAC key sync ");
        //        ResponseTextMap.Add("918", "No communication keys available for use ");
        //        ResponseTextMap.Add("919", "Encryption key sync error ");
        //        ResponseTextMap.Add("920", "Security software/hardware error – try again ");
        //        ResponseTextMap.Add("921", "Security software/hardware error – no action ");
        //        ResponseTextMap.Add("922", "Message number out of sequence ");
        //        ResponseTextMap.Add("923", "Request in progress ");
        //        ResponseTextMap.Add("940", "Unknown terminal ");
        //        ResponseTextMap.Add("942", "Invalid Reconciliation Date  ");
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Failed to load ResponseMap");
        //        throw new Exception("Failed to load ResponseMap", ex);
        //    }
        //    log.LogMethodExit(ResponseTextMap);
        //}

        private string GetResponseText(string responseCode)
        {
            log.LogMethodEntry(responseCode);
            string responseText = "";
            try
            {
                if (ResponseTextMap.ContainsKey(responseCode))
                {
                    ResponseTextMap.TryGetValue(responseCode, out responseText);
                }
                else
                {
                    responseText = "Payment failed, Please try again";
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception from getResponseText: " + ex.Message);
                log.Error("Exception from getResponseText: ", ex);
                throw new Exception("Exception from getResponseText: " + ex);
            }
            log.LogMethodExit(responseText);
            return responseText;
        }

        public string GetComTerminalId(GeideaRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            GeideaResponseDTO madaResponse = null;
            try
            {

                byte[] terminalId = new byte[17];
                byte[] inOutBuff = System.Text.Encoding.ASCII.GetBytes("07!");
                byte[] intval = new byte[1];
                intval[0] = (byte)inOutBuff.Length;
                var outp = api_GetCOMTerminalID(requestDTO.port, requestDTO.baudRate, requestDTO.parityBit, requestDTO.dataBit, requestDTO.stopBit, inOutBuff, intval, terminalId);
                madaResponse = new GeideaResponseDTO(null, 0, 0, null, null, null, null, null, outp.ToString(), ByteTostring(terminalId), null, null, null, null, null, null, null);
                if (madaResponse == null)
                {
                    throw new Exception("Response was null when fetching Terminal Id");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Exception in fetching terminalId: ", ex);
            }
            log.LogMethodExit(madaResponse.TerminalId);
            return madaResponse.TerminalId;
        }

        public void GetEcrDetails(GeideaRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            try
            {
                GeideaResponseDTO madaResponse = null;
                byte[] inOutBuff = Encoding.ASCII.GetBytes("06!");
                byte[] intval = new byte[1];
                byte[] ecrData = new byte[7];
                byte[] ecrPrinter = new byte[7];
                byte[] ecrSpeed = new byte[6];
                intval[0] = (byte)inOutBuff.Length;
                var outp = api_GetECRDetails(requestDTO.port, requestDTO.baudRate, requestDTO.parityBit, requestDTO.dataBit, requestDTO.stopBit, inOutBuff, intval, ecrData, ecrPrinter, ecrSpeed);
                // We get 3 params as a response: ecrData, ecrPrinter, ecrSpeed
                madaResponse = new GeideaResponseDTO(null, 0, 0, null, null, null, null, null, outp.ToString(), null, null, null, null, null, null, null, null);
                madaResponse.ResponseCode = outp.ToString();
                if (madaResponse != null && madaResponse.ResponseCode != null)
                {
                    madaResponse.ResponseText = GetResponseText(madaResponse.ResponseCode);
                }
                else
                {
                    log.Error("Response was null when getting ECR Details");
                    throw new Exception("Response was null when getting ECR Details");
                }

                Console.WriteLine(ByteTostring(ecrData));
                Console.WriteLine(ByteTostring(ecrPrinter));
                Console.WriteLine(ByteTostring(ecrSpeed));
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Error occured when getting ECR Details", ex);
            }
            log.LogMethodExit();
        }

        //public void MakeTest()
        //{
        //    byte[] inOutBuff = System.Text.Encoding.ASCII.GetBytes("100;1;1!");
        //    byte[] intval = new byte[1];
        //    intval[0] = (byte)inOutBuff.Length;
        //    //var res = api_CheckStatus(6, 38400, 0, 8, 1, inOutBuff, intval);
        //    //var res = api_GetCOMTerminalID(6, 38400, 0, 8, 0, inOutBuff, intval);
        //    var res = api_RequestCOMTrxn(port, baudRate, parityBit, dataBit, stopBit, inOutBuff, intval, 0);
        //    Console.WriteLine(res.ToString());
        //    Console.ReadLine();
        //}

        public static DateTime GetTimeStamp()
        {
            DateTime timestamp;
            try
            {
                timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in getting timestamp: " + ex.Message);
                throw;
            }
            return timestamp;
        }
        private void BuildTrxTypeMap()
        {
            log.LogMethodEntry();
            TrxTypeMap = new Dictionary<int, string>();
            TrxTypeMap.Add(12, "Authorization");
            TrxTypeMap.Add(13, "Advice");
            TrxTypeMap.Add(0, "Sale");
            TrxTypeMap.Add(2, "Refund");
            TrxTypeMap.Add(3, "Void");
            TrxTypeMap.Add(21, "Repeat");
            log.LogMethodExit();
        }
        private string GetTrxType(int trxType)
        {
            log.LogMethodEntry(trxType);
            string result = "";
            try
            {
                if (TrxTypeMap.ContainsKey(trxType))
                {
                    TrxTypeMap.TryGetValue(trxType, out result);
                }
                else
                {
                    throw new Exception("Invalid Transaction Type");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception from getResponseText: " + ex.Message);
                log.Error("Exception from GetTrxType: ", ex);
                throw new Exception("Exception from GetTrxType: " + ex);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
