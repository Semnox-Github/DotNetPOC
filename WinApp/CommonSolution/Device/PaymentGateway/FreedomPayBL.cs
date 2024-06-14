/********************************************************************************************
* Project Name - FreedomPayL
* Description  - Buisness logic
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
*********************************************************************************************
*2.70.2.0       20-Sep-2019      Archana            Created
*2.80.0       21-Jul-2020      Mathew             Card Type logic for Debit cards
********************************************************************************************/

using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class FreedomPayBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string freewayUrl = string.Empty;
        string storeId = string.Empty;
        string terminalId = string.Empty;
        FreedomPayDTO freedomPayDTO = null;
        Utilities utilities = null;        
        public string receiptText = string.Empty;

        public FreedomPayBL(Utilities utilities, string gatewayUrl)
        {
            log.LogMethodEntry();
            this.utilities = utilities;
            freewayUrl = gatewayUrl;
            freedomPayDTO = new FreedomPayDTO();
            log.LogMethodExit();
        }

        public FreedomPayBL(Utilities utilities, FreedomPayDTO freedomPayDTO, string gatewayUrl) : this(utilities, gatewayUrl)
        {
            log.LogMethodEntry(freedomPayDTO);
            this.freedomPayDTO = freedomPayDTO;            
            log.LogMethodExit();
        }

        public CCTransactionsPGWDTO ProcessTransaction(int requestTimeout = -1)
        {
            log.LogMethodEntry(requestTimeout);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            try
            {
                string response = SendCommand(CreateCommand(), requestTimeout);
                cCTransactionsPGWDTO = ParseResponse(response);
                if(cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode == "243" && freedomPayDTO.freedomPayRequestDTO.RequestType=="Void")
                {
                    log.Info("Void transaction declained. Invoking Refund...");
                    freedomPayDTO.freedomPayRequestDTO.RequestType = "Refund";
                    freedomPayDTO.freedomPayRequestDTO.MerchantReferenceCode = System.Guid.NewGuid().ToString();
                    response = SendCommand(CreateCommand());                    
                    cCTransactionsPGWDTO = ParseResponse(response);
                }                
            }
            catch(TimeoutException ex)
            {
                throw;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }

        private string CreateCommand()
        {
            log.LogMethodEntry();
            string requestString = string.Empty;
            try
            {
                XElement requestsXML = new XElement("POSRequest",
                                        new XElement("ChargeAmount", freedomPayDTO.freedomPayRequestDTO.ChargeAmount),
                                        new XElement("RequestGuid", freedomPayDTO.freedomPayRequestDTO.RequestGuid),
                                        new XElement("StoreId", freedomPayDTO.freedomPayRequestDTO.StoreId),
                                        new XElement("TerminalId", freedomPayDTO.freedomPayRequestDTO.TerminalId),
                                        new XElement("RequestType", freedomPayDTO.freedomPayRequestDTO.RequestType),
                                        new XElement("MerchantReferenceCode", freedomPayDTO.freedomPayRequestDTO.MerchantReferenceCode),
                                        new XElement("ClientEnvironment", freedomPayDTO.freedomPayRequestDTO.ClientEnvironment),
                                        new XElement("WorkstationId", freedomPayDTO.freedomPayRequestDTO.WorkStationId),
                                        new XElement("InvoiceNumber", freedomPayDTO.freedomPayRequestDTO.InvoiceNumber)
                                        );
		if(freedomPayDTO.freedomPayRequestDTO.RequestType == "Sale" || freedomPayDTO.freedomPayRequestDTO.RequestType == "Auth")
                {
                    requestsXML.Add(new XElement("AllowPartial", freedomPayDTO.freedomPayRequestDTO.AllowPartial));
                }                
		if (freedomPayDTO.freedomPayRequestDTO.RequestType == "Void")
                {
                    requestsXML.Add(new XElement("RequestId", freedomPayDTO.freedomPayRequestDTO.RequestId));
                }                
                requestString = requestsXML.ToString();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.Info("Freedompay request object :" + requestString);
            log.LogMethodExit(requestString);
            return requestString;
        }

        
        protected string SendCommand(string requestXml, int requestTimeOut = -1)
        {
            log.LogMethodEntry(requestXml, requestTimeOut);
            log.Info("requestTimeOut value =:" + requestTimeOut);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(freewayUrl);
                if (requestTimeOut > 0)
                {
                    request.Timeout = requestTimeOut * 1000;
                    log.Info("requestTimeOut value =:" + requestTimeOut);
                }
                request.ReadWriteTimeout = System.Threading.Timeout.Infinite;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    if (responseStr.Contains("<ChipData>"))
                    {
                        responseStr = MaskChipDataText(responseStr);
                    }
                    log.LogMethodExit(responseStr);
                    return responseStr;
                }
                log.LogMethodExit(null);
                return null;
            }
            catch (WebException e)
            {
                log.Error(e);
                if (e.Status == WebExceptionStatus.Timeout)
                {
                    throw new TimeoutException(e.Message,e);                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex);
                log.LogMethodExit();
                throw;
            }
           
        }
        string MaskChipDataText(string responseString)
        {
            log.LogMethodEntry();
            string responseWithMaskedChipData = string.Empty;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(responseString);
                XmlNode node = xmlDocument.SelectSingleNode("/POSResponse/ChipData");
                node.InnerText = "********";
                responseWithMaskedChipData = xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(responseWithMaskedChipData);
            return responseWithMaskedChipData;
        }
        private CCTransactionsPGWDTO ParseResponse(string response)
        {
            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            try
            {
                log.LogMethodEntry(response);
                //log.Info("Freedompay response object :" + response);
                XmlDocument responseXml = new XmlDocument();
                responseXml.LoadXml(response);
                POSResponse responseObject = new POSResponse();
                var reader = new StringReader(response);
                var serializer = new XmlSerializer(typeof(POSResponse));
                responseObject = (POSResponse)serializer.Deserialize(reader);
                if (responseObject != null)
                {
                    cCTransactionsPGWDTO.AcctNo = responseObject.MaskedCardNumber != null ? new String('X', 12) + responseObject.MaskedCardNumber.Substring(12) : string.Empty;
                    cCTransactionsPGWDTO.Authorize = responseObject.ApprovedAmount.ToString();
                    cCTransactionsPGWDTO.DSIXReturnCode = responseObject.ErrorCode.ToString();
                    cCTransactionsPGWDTO.RefNo = responseObject.RequestId != null ? responseObject.RequestId : 0.ToString();                    
                    cCTransactionsPGWDTO.TranCode = responseObject.Decision;
                    cCTransactionsPGWDTO.TextResponse = responseObject.Message;
                    cCTransactionsPGWDTO.RecordNo = responseObject.MerchantReferenceCode;
                    cCTransactionsPGWDTO.CardType = responseObject.IssuerName != null ? (responseObject.CardType != null  ? responseObject.IssuerName + ((responseObject.CardType).ToUpper() == "DEBIT" ? "_" + (responseObject.CardType).ToUpper() : string.Empty) : responseObject.IssuerName) : string.Empty;
                    cCTransactionsPGWDTO.CardType = cCTransactionsPGWDTO.CardType == "DEBIT_DEBIT" ? "VISA_DEBIT" : cCTransactionsPGWDTO.CardType;
                    cCTransactionsPGWDTO.CaptureStatus = responseObject.EntryMode;
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.ProcessData = responseObject.NameOnCard;
                    cCTransactionsPGWDTO.AuthCode = responseObject.ApprovalCode;
                    cCTransactionsPGWDTO.TokenID = responseObject.TransactionId;                    
                    cCTransactionsPGWDTO.ResponseOrigin = "PinVerified:" + responseObject.PinVerified + "|" + "SignatureVerified:" + responseObject.SignatureRequired;
                    cCTransactionsPGWDTO.AcqRefData = responseObject.ExpiryDate;
                    if (freedomPayDTO.freedomPayRequestDTO.RequestType == "Sale")
                    {
                        cCTransactionsPGWDTO.UserTraceData = "Sale";
                    }
                    else if(freedomPayDTO.freedomPayRequestDTO.RequestType == "Void")
                    {
                        cCTransactionsPGWDTO.UserTraceData = "Void";
                    }
                    else if (freedomPayDTO.freedomPayRequestDTO.RequestType == "Refund")
                    {
                        cCTransactionsPGWDTO.UserTraceData = "Refund";
                    }
                    cCTransactionsPGWDTO.InvoiceNo = freedomPayDTO.freedomPayRequestDTO.InvoiceNumber.ToString();                    
                    receiptText = responseObject.ReceiptText;
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }
    }
}
