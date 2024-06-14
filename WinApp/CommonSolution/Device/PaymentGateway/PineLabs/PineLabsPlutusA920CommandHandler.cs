/******************************************************************************************************
 * Project Name - Device
 * Description  - PineLabs Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.140.5     11-Jan-2023    Prasad & Fiona   PineLabs Payment gateway integration
 ********************************************************************************************************/
using System;
using System.Linq;
using System.Text;
using PlutusExchangeLib;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class PineLabsPlutusA920CommandHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        const string COMMA = ",";
        string emptyString = string.Empty;

        private PineLabsPlutusA920ResponseDTO getFormattedResponse(string responseString)
        {
            log.LogMethodEntry(responseString);
            PineLabsPlutusA920ResponseDTO responseDTO = new PineLabsPlutusA920ResponseDTO();
            try
            {
                if (string.IsNullOrWhiteSpace(responseString))
                {
                    log.Error("getFormattedResponse():Response String was empty");
                    throw new Exception("Response was empty");
                }

                log.Debug($"getFormattedResponse():responseString={responseString}");

                // check if response is valid csv string
                if (responseString.Contains(','))
                {

                    // check if all 37 keys received
                    // determined using number of commas available in the response string
                    int commas = responseString.Count(c => c == Convert.ToChar(COMMA));
                    if (commas == 36)
                    {
                        log.Info("Response was a valid CSV");
                        string[] data = responseString.Split(',');
                        log.Debug($"CSV Response was: {JsonConvert.SerializeObject(data)}");
                        var metadata = data[26].Split('|');
                        log.Debug($"Metadata from CSV Response was: {metadata}");

                        responseDTO.InternalResponseCode = "1";
                        responseDTO.InternalResponseMessage = "Success";
                        responseDTO.InvoiceNo = RemoveExcessQuotes(data[0]);
                        responseDTO.AuthCode = RemoveExcessQuotes(data[1]);
                        responseDTO.TextResponse = RemoveExcessQuotes(data[2]);
                        responseDTO.AcctNo = RemoveExcessQuotes(data[3]);
                        responseDTO.CardType = RemoveExcessQuotes(data[6]);
                        responseDTO.RecordNo = RemoveExcessQuotes(data[7]); //invoice no
                        responseDTO.DSIXReturnCode = RemoveExcessQuotes(data[11]); // remark
                        responseDTO.RefNo = RemoveExcessQuotes(data[33]);
                        responseDTO.Authorize = RemoveExcessQuotes(data[34]);
                        responseDTO.CaptureStatus = RemoveExcessQuotes(data[36]);
                        responseDTO.MerchantId = RemoveExcessQuotes(data[13]);
                        responseDTO.TranCode = RemoveExcessQuotes(data[35]) == "4001" ? "SALE" : "REFUND";
                        responseDTO.BankCode = RemoveExcessQuotes(data[21]);
                        responseDTO.RRN = !string.IsNullOrWhiteSpace(data[14]) ? RemoveExcessQuotes(data[14]) : string.Empty;
                    }
                    else
                    {
                        log.Error("Invalid Response from Terminal - Less Number of Fields");
                        throw new Exception("Transaction Failed: Invalid Response from Terminal");
                    }
                }
                else
                {
                    log.Error("Invalid Response from Terminal - Invalid CSV String");
                    throw new Exception("Transaction Failed: Invalid Response from Terminal");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(responseDTO);
            return responseDTO;
        }

        private PineLabsPlutusA920ResponseDTO getFormattedUpiResponse(string responseString)
        {
            log.LogMethodEntry(responseString);
            PineLabsPlutusA920ResponseDTO responseDTO = new PineLabsPlutusA920ResponseDTO();
            try
            {
                if (string.IsNullOrWhiteSpace(responseString))
                {
                    log.Error("getFormattedUpiResponse():Response String was empty");
                    throw new Exception("Response was empty");
                }

                log.Debug($"getFormattedUpiResponse():responseString={responseString}");

                // check if response is valid csv string
                if (responseString.Contains(','))
                {
                    // check if all 37 keys received
                    // 37 keys has 36 commas
                    // determined using number of commas available in the response string
                    int commas = responseString.Count(c => c == Convert.ToChar(COMMA));
                    if (commas != 36)
                    {
                        // error occured
                        log.Error("Response was invalid csv string");
                        if (commas == 11)
                        {
                            log.Debug("Error string received");
                            string[] errorData = responseString.Split(',');
                            string errorMessage = RemoveExcessQuotes(errorData[11]);
                            log.Error($"Error in UPI transaction: {errorMessage}");
                            throw new Exception($"Error in UPI transaction: {errorMessage}");
                        }
                        else
                        {
                            log.Error("Error in UPI transaction");
                            throw new Exception("Error in UPI transaction");
                        }
                    }
                    string[] data = responseString.Split(',');

                    log.Debug($"CSV Response was: {JsonConvert.SerializeObject(data)}");
                    if (data == null || data.Length == 0)
                    {
                        log.Error("UPI Response was null");
                        throw new Exception("UPI Transaction Failed");
                    }

                    if (RemoveExcessQuotes(data[11]) == "Failed To Communicate With Device")
                    {
                        log.Error("UPI Response was : Failed To Communicate With Device");
                        throw new Exception("UPI Transaction Failed because of communication error");
                    }
                    var metadata = data[26].Split('|');
                    log.Debug($"Metadata from CSV Response was: {metadata}");

                    responseDTO.InternalResponseCode = "1";
                    responseDTO.InternalResponseMessage = "Success";
                    responseDTO.InvoiceNo = RemoveExcessQuotes(data[0]); // ccRequestId
                    responseDTO.AuthCode = RemoveExcessQuotes(data[1]); // Approval Code
                    responseDTO.TextResponse = RemoveExcessQuotes(data[2]); //Host Response
                    responseDTO.AcctNo = RemoveExcessQuotes(data[3]); // Card Number
                    responseDTO.CardType = RemoveExcessQuotes(data[6]); // Mode e.g. UPI
                    responseDTO.RecordNo = RemoveExcessQuotes(data[7]); //invoice no
                    responseDTO.DSIXReturnCode = RemoveExcessQuotes(data[11]); // remark
                    responseDTO.RefNo = RemoveExcessQuotes(data[33]); // Pine Labs Payment Id
                    responseDTO.Authorize = string.IsNullOrWhiteSpace(RemoveExcessQuotes(data[28])) ? RemoveExcessQuotes(data[34]) : RemoveExcessQuotes(data[28]); // Amount
                    //responseDTO.CaptureStatus = RemoveExcessQuotes(data[36]);
                    responseDTO.MerchantId = RemoveExcessQuotes(data[13]);
                    //responseDTO.TranCode = RemoveExcessQuotes(data[35]) == "5120" ? "UPI SALE" : "BHARAT QR SALE";
                    responseDTO.UpiQrProgramType = RemoveExcessQuotes(data[21]);
                    responseDTO.TxnAcquirerName = RemoveExcessQuotes(data[12]);
                    responseDTO.RRN = !string.IsNullOrWhiteSpace(data[14]) ? RemoveExcessQuotes(data[14]) : string.Empty;
                    responseDTO.BatchNo = RemoveExcessQuotes(data[8]);
                }
                else
                {
                    log.Error("Invalid Response from Terminal - Invalid CSV String");
                    throw new Exception("Transaction Failed: Invalid Response from Terminal");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(responseDTO);
            return responseDTO;
        }

        private static string RemoveExcessQuotes(string inputString)
        {
            log.LogMethodEntry(inputString);
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(inputString))
                {
                    result = inputString.Replace("\"", string.Empty).Trim();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(result);
            return result;
        }

        // CREDIT CARD PAYMENT METHODS
        public PineLabsPlutusA920ResponseDTO Sale(string ccRequestId, double amount)
        {
            log.LogMethodEntry(ccRequestId, amount);
            PineLabsPlutusA920ResponseDTO response = null;
            try
            {
                if (string.IsNullOrWhiteSpace(ccRequestId) || amount <= 0)
                {
                    log.Error($"Sale: Invalid Input parameters supplied.");
                    throw new Exception("Invalid Input parameters supplied");
                }

                log.Debug($"Sale: ccRequestId={ccRequestId}, amount={amount}");

                ExchangeObj exchangeObj = new ExchangeObj();
                //string directSaleRequestString = ccRequestId + "," + Convert.ToString(amount);
                string directSaleRequestString = $"{ccRequestId}{COMMA}{Convert.ToString(amount)}";
                log.Debug($"Sale: Request String={directSaleRequestString}");


                exchangeObj.PL_TriggerTransaction(4001, ref directSaleRequestString);

                // check if null or empty response
                if (String.IsNullOrWhiteSpace(directSaleRequestString))
                {
                    log.Error("Ivalid Response from Terminal - Null/Empty String");
                    throw new Exception("Ivalid Response from Terminal - Null/Empty String");
                }

                log.Debug($"Sale: CSV Response String = {directSaleRequestString}");

                response = getFormattedResponse(directSaleRequestString);
                log.Debug($"Sale: Formatted Response = {JsonConvert.SerializeObject(response)}");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw;
            }
            log.LogMethodExit(response);
            return response;
        }

        public PineLabsPlutusA920ResponseDTO Refund(string ccRequestId, double amount, string bankCode)
        {
            log.LogMethodEntry(ccRequestId, amount, bankCode);
            PineLabsPlutusA920ResponseDTO response = null;
            try
            {
                if (string.IsNullOrWhiteSpace(ccRequestId) || string.IsNullOrWhiteSpace(bankCode) || amount <= 0)
                {
                    log.Error("Invalid Input parameters supplied");
                    throw new Exception("Invalid Input parameters supplied");
                }

                log.Debug($"Refund: Params Received were ccRequestId={ccRequestId}, amount={amount}, bankCode={bankCode}");

                ExchangeObj exchangeObj = new ExchangeObj();
                //string refundRequestString = ccRequestId + "," + Convert.ToString(amount) + ",,,," + invoiceNumber + ",,,,,";
                string refundRequestString = ccRequestId + "," + Convert.ToString(amount) + "," + bankCode + ",,,,,,,,";
                log.Debug($"Refund: refundRequestString={refundRequestString}");

                exchangeObj.PL_TriggerTransaction(4002, ref refundRequestString);
                if (String.IsNullOrWhiteSpace(refundRequestString))
                {
                    log.Error("Refund: Ivalid Response from Terminal - Null/Empty String");
                    throw new Exception("Ivalid Response from Terminal - Null/Empty String");
                }
                else
                {
                    log.Debug($"Refund: CSV Response String={refundRequestString}");
                    response = getFormattedResponse(refundRequestString);
                    log.Debug($"Refund: Formatted Response = {JsonConvert.SerializeObject(response)}");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw;
            }
            log.LogMethodExit(response);
            return response;
        }

        public string Void(string ccRequestId, double amount, string invoiceNumber, string bankCode)
        {
            log.LogMethodEntry(ccRequestId, amount, invoiceNumber);
            //PineLabsPlutusA920ResponseDTO response = null;
            string voidString;
            try
            {
                if (string.IsNullOrWhiteSpace(ccRequestId) || string.IsNullOrWhiteSpace(invoiceNumber) || string.IsNullOrWhiteSpace(bankCode) || amount <= 0)
                {
                    log.Error("Void: Invalid Input parameters supplied");
                    throw new Exception("Invalid Input parameters supplied");
                }

                log.Debug($"Void: Request Params Received were ccRequestId={ccRequestId}, invoiceNumber={invoiceNumber}, amount={amount}, bankCode={bankCode}");
                ExchangeObj exchangeObj = new ExchangeObj();
                int counter = 0;

                do
                {
                    voidString = $"{ccRequestId},{Convert.ToString(amount)},{bankCode},,,{invoiceNumber},,,,,";
                    log.Debug($"Void: voidRequestString={voidString}");
                    exchangeObj.PL_TriggerTransaction(4006, ref voidString);
                    counter++;
                } while (voidString.Trim() == "Failed To Communicate With Device" && counter < 3);

                if (String.IsNullOrWhiteSpace(voidString))
                {
                    log.Error("Refund: Ivalid Response from Terminal - Null/Empty String");
                    throw new Exception("Ivalid Response from Terminal - Null/Empty String");
                }
                log.Debug($"Void: Response={voidString}");

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(voidString);
            return voidString;
        }

        public GetPaymentResponseDto GetPaymentStatus(PineLabsGetPaymentRequestDto requestDto, PineLabsPlutusA920Configurations configurations)
        {
            log.LogMethodEntry(requestDto, configurations);
            GetPaymentResponseDto responseDto = null;
            try
            {
                if (requestDto == null)
                {
                    log.Error("GetPaymentStatus: RequestDto was empty");
                    throw new Exception("No Request Information is provided to fetch the transaction");
                }

                if (string.IsNullOrWhiteSpace(requestDto.TxnId)
                    || string.IsNullOrWhiteSpace(requestDto.TxnAmtPaise)
                    || string.IsNullOrWhiteSpace(requestDto.MerchantID)
                    || string.IsNullOrWhiteSpace(requestDto.TxnTypeIdentifierString)
                    || string.IsNullOrWhiteSpace(requestDto.SecurityToken)
                    )
                {
                    log.Error("GetPaymentStatus: Required input params not provided to fetch the transaction");
                    throw new Exception("Required input params not provided to fetch the transaction");
                }

                if (configurations == null)
                {
                    log.Error("GetPaymentStatus: configurations was empty");
                    throw new Exception("Configurations was not set to fetch the transaction");
                }

                string API_URL = configurations.API_URL;

                string responseFromServer;

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(API_URL);
                myHttpWebRequest.Method = "POST";


                string json = JsonConvert.SerializeObject(requestDto, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });

                byte[] data = Encoding.UTF8.GetBytes(json);

                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream responseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                responseFromServer = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                responseStream.Close();
                myHttpWebResponse.Close();

                if (string.IsNullOrWhiteSpace(responseFromServer))
                {
                    log.Error("GetPaymentStatus: responseFromServer was empty");
                    throw new Exception("We did not receive any response for the recently fetched transaction");
                }

                responseDto = JsonConvert.DeserializeObject<GetPaymentResponseDto>(responseFromServer);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(responseDto);
            return responseDto;
        }

        // UPI PAYMENT METHODS

        public PineLabsPlutusA920ResponseDTO UpiSale(string ccRequestId, double amount, int upiType)
        {
            log.LogMethodEntry(ccRequestId, amount, upiType);
            string upiSaleRequestString = string.Empty; ;
            // upiType 2=UPI, 1=BharatQR
            // 5120=UPI, 5123=BharatQR
            // 
            //General SALE Request:  5120, TX12345678,100,,,,,,,
            //UPI SALE Request:      5120,TX12345678,100,2,,,,,,
            //BharatQR SALE Request: 5123,TX12345678,100,1,,,,,,

            PineLabsPlutusA920ResponseDTO response = null;
            try
            {
                if (string.IsNullOrWhiteSpace(ccRequestId) || amount <= 0 || upiType <= 0)
                {
                    log.Error($"UpiSale: Invalid Input parameters supplied.");
                    throw new Exception("Invalid Input parameters supplied");
                }

                log.Debug($"UpiSale: Request params: ccRequestId={ccRequestId}, amount={amount}, upiType={upiType}");
                int upiTransactionCode = upiType == 2 ? 5120 : 5123;

                ExchangeObj exchangeObj = new ExchangeObj();
                int counter = 0;
                string result = string.Empty;

                do
                {
                    upiSaleRequestString = $"{ccRequestId}{COMMA}{Convert.ToString(amount)}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}";
                    log.Debug($"UpiSale: Request String={upiSaleRequestString}, upiTransactionCode={upiTransactionCode}");
                    exchangeObj.PL_TriggerTransaction(upiTransactionCode, ref upiSaleRequestString);
                    counter++;
                    string[] splitString = upiSaleRequestString.Split(',');
                    result = splitString[splitString.Length - 1].Trim('"');
                } while (result == "Failed To Communicate With Device" && counter < 3);
                

                // check if null or empty response
                if (String.IsNullOrWhiteSpace(upiSaleRequestString))
                {
                    log.Error("Invalid Response from Terminal - Null/Empty String");
                    throw new Exception("Invalid Response from Terminal - Null/Empty String");
                }

                log.Debug($"UpiSale: CSV Response String = {upiSaleRequestString}");

                response = getFormattedUpiResponse(upiSaleRequestString);
                log.Debug($"UpiSale: Formatted Response = {JsonConvert.SerializeObject(response)}");

                if (response.TextResponse != "TRANSACTION INITIATED CHECK GET STATUS" && response.TextResponse != "APPROVED")
                {
                    log.Error($"Error occured in UPI Sale Request: {response.TextResponse} | {response.DSIXReturnCode}");
                    throw new Exception($"Error in processing Payment: {response.TextResponse} | {response.DSIXReturnCode}");
                }

                log.Debug("UPI Sale Request raised successfully");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(response);
            return response;
        }

        public PineLabsPlutusA920ResponseDTO UpiVoid(string ccRequestId, double amount, string batchId, string invoiceNo)
        {
            log.LogMethodEntry(ccRequestId, amount, batchId, invoiceNo);
            //Standard Request: 5121,1245,100,,,,,,,,,9023,119,,,,,,,,,,,
            PineLabsPlutusA920ResponseDTO voidResponseDto = null;
            string voidString;
            try
            {
                if (string.IsNullOrWhiteSpace(ccRequestId) || amount <= 0 || string.IsNullOrWhiteSpace(batchId) || string.IsNullOrWhiteSpace(invoiceNo))
                {
                    log.Error("UpiVoid: Invalid Input parameters supplied");
                    throw new Exception("Invalid Input parameters supplied");
                }

                log.Debug($"UpiVoid: Request Params Received were ccRequestId={ccRequestId}, invoiceNo={invoiceNo}, amount={amount}, batchId={batchId}");
                ExchangeObj exchangeObj = new ExchangeObj();

                voidString = $"{ccRequestId}{COMMA}{Convert.ToString(amount)}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{batchId}{COMMA}{invoiceNo}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}";
                log.Debug($"UpiVoid: voidRequestString={voidString}");
                exchangeObj.PL_TriggerTransaction(5121, ref voidString);
                if (String.IsNullOrWhiteSpace(voidString))
                {
                    log.Error("UpiVoid: Invalid Response from Terminal - Null/Empty String");
                    throw new Exception("Invalid Response from Terminal - Null/Empty String");
                }
                log.Debug($"UpiVoid: Response={voidString}");
                voidResponseDto = getFormattedUpiResponse(voidString);
                log.Debug($"UpiVoid: voidResponseDto={voidResponseDto}");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(voidResponseDto);
            return voidResponseDto;
        }

        public PineLabsPlutusA920ResponseDTO UpiGetStatus(string ccRequestId, double amount, string batchId, string invoiceNo)
        {
            log.LogMethodEntry(ccRequestId, amount, batchId, invoiceNo);
            // 5122,1245,100,,,,,,,,,9023,119,,,,,,,,,,,

            PineLabsPlutusA920ResponseDTO response = null;
            try
            {
                if (string.IsNullOrWhiteSpace(ccRequestId) || amount <= 0 || string.IsNullOrWhiteSpace(batchId) || string.IsNullOrWhiteSpace(invoiceNo))
                {
                    log.Error($"UpiSale: Invalid Input parameters supplied.");
                    throw new Exception("Invalid Input parameters supplied");
                }

                log.Debug($"UpiSale: ccRequestId={ccRequestId}, amount={amount}, batchId={batchId}, roc={invoiceNo}");
                ExchangeObj exchangeObj = new ExchangeObj();
                string upiGetStatusRequestString = $"{ccRequestId}{COMMA}{Convert.ToString(amount)}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{batchId}{COMMA}{invoiceNo}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}{COMMA}";
                log.Debug($"upiGetStatusRequestString: Request String={upiGetStatusRequestString}");


                exchangeObj.PL_TriggerTransaction(5122, ref upiGetStatusRequestString);

                // check if null or empty response
                if (String.IsNullOrWhiteSpace(upiGetStatusRequestString))
                {
                    log.Error("Invalid Response from Terminal - Null/Empty String");
                    throw new Exception("Invalid Response from Terminal - Null/Empty String");
                }

                log.Debug($"upiGetStatusRequestString: CSV Response String = {upiGetStatusRequestString}");

                response = getFormattedUpiResponse(upiGetStatusRequestString);
                log.Debug($"upiGetStatusRequestString: Formatted Response = {JsonConvert.SerializeObject(response)}");

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(response);
            return response;
        }

        #region Other Transaction Types
        //public object PreAuth(string ccRequestId, double amount)
        //{
        //    ExchangeObj exchangeObj = new ExchangeObj();
        //    string preAuthRequestString = ccRequestId + "," + Convert.ToString(amount);
        //    exchangeObj.PL_TriggerTransaction(4007, ref preAuthRequestString);
        //    return null;
        //}
        //public object SaleComplete(string ccRequestId, double amount, string invoiceNumber)
        //{
        //    ExchangeObj exchangeObj = new ExchangeObj();
        //    string saleCompleteRequestString = ccRequestId + "," + Convert.ToString(amount) + ",,,," + invoiceNumber + ",,,,,";
        //    exchangeObj.PL_TriggerTransaction(4008, ref saleCompleteRequestString);
        //    return null;
        //}
        //public object Adjust(string ccRequestId, double amount, string invoiceNumber)
        //{
        //    ExchangeObj exchangeObj = new ExchangeObj();
        //    string adjustRequestString = ccRequestId + "," + Convert.ToString(amount) + ",,,," + invoiceNumber + ",,,,,";
        //    exchangeObj.PL_TriggerTransaction(4005, ref adjustRequestString);
        //    return null;
        //}
        //public object TipAdjust(string ccRequestId, double amount, string invoiceNumber)
        //{
        //    ExchangeObj exchangeObj = new ExchangeObj();
        //    string tipAdjustRequestString = ccRequestId + "," + Convert.ToString(amount) + ",,,," + invoiceNumber + ",,,,,";
        //    exchangeObj.PL_TriggerTransaction(4015, ref tipAdjustRequestString);
        //    return null;
        //}
        #endregion
    }
}
