/********************************************************************************************
 * Project Name - Adyen Command Handler
 * Description  - Data handler of the AdyenPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        20-SEP-2019   Raghuveera      Created  
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Adyen;
using Adyen.Model.Nexo.Message;
using Adyen.Model.Nexo;
using System.Security.Cryptography.X509Certificates;
using Adyen.Security;
using Adyen.Model.Modification;
using Semnox.Core.Utilities;
using System.Web.Script.Serialization;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class AdyenCommandHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EncryptionCredentialDetails encryptionCredentialDetails;
        object client;
        Utilities utilities;
        string currencyCode;
        private string customerReceipt;
        private string merchantReceipt;
        public string CustomerReceipt { get { return customerReceipt; } }
        public string MerchantReceipt { get { return merchantReceipt; } }
        public AdyenCommandHandler(string keyIdentifier, string password, string deviceUrl,string envType, Utilities utilities)
        {
            log.LogMethodEntry();
            this.utilities = utilities;
            currencyCode = utilities.getParafaitDefaults("CURRENCY_CODE");
            encryptionCredentialDetails = new EncryptionCredentialDetails
            {
                AdyenCryptoVersion = 1,
                KeyIdentifier = keyIdentifier,
                Password = password

                
            };
            //Semnox,!7%fQbZDg~bmuVk*{xva8B?>5e   User name password no use

            var Config = new Config
            {
                Environment= ((envType.Equals("L"))? Adyen.Model.Enum.Environment.Live:Adyen.Model.Enum.Environment.Test),
                Endpoint = deviceUrl,
                HttpRequestTimeout=240000,
            };
            client = new Client(Config);
            log.LogMethodExit();

        }
        public CCTransactionsPGWDTO ProcessTransaction(AdyenRequest adyenRequest)
        {
            log.LogMethodEntry(adyenRequest);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            switch (adyenRequest.TransactionType)
            {
                case PaymentGatewayTransactionType.SALE:
                    cCTransactionsPGWDTO = Performsale(adyenRequest);
                    break;
                case PaymentGatewayTransactionType.REFUND:
                    cCTransactionsPGWDTO = PerformRefund(adyenRequest);
                    if (!string.IsNullOrEmpty(customerReceipt))
                    {
                        //int amountStartIndex = customerReceipt.LastIndexOf("TOTAL          : $") + ("TOTAL          : $").Length + 1;
                        int amountEndIndex = customerReceipt.IndexOf("           --");
                        if (amountEndIndex > 0)
                        {
                            customerReceipt = customerReceipt.Substring(0, amountEndIndex) + "REFUND REQUEST : $ " + adyenRequest.Amount.ToString("0.00") + Environment.NewLine+ customerReceipt.Substring(amountEndIndex);
                            customerReceipt += Environment.NewLine;
                            customerReceipt += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("The refund will be visible on your statement in the next couple of weeks."), Alignment.Left);
                        }
                    }
                    if (!string.IsNullOrEmpty(merchantReceipt))
                    {
                        //int amountStartIndex = merchantReceipt.LastIndexOf("TOTAL          : $") + ("TOTAL          : $").Length + 1;
                        int amountEndIndex = merchantReceipt.IndexOf("           --");
                        if (amountEndIndex > 0)
                        {
                            merchantReceipt = merchantReceipt.Substring(0, amountEndIndex) + "REFUND REQUEST : $ " + adyenRequest.Amount.ToString("0.00") + Environment.NewLine+ merchantReceipt.Substring(amountEndIndex);
                            merchantReceipt += Environment.NewLine;
                            merchantReceipt += CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("The refund will be visible on your statement in the next couple of weeks."), Alignment.Left);
                        }
                    }
                    break;
                case PaymentGatewayTransactionType.AUTHORIZATION:
                    cCTransactionsPGWDTO = Performsale(adyenRequest);
                    break;
                case PaymentGatewayTransactionType.TATokenRequest:
                    cCTransactionsPGWDTO = PerformTokenRequest(adyenRequest);
                    break;
            }
            
            if (cCTransactionsPGWDTO != null && !string.IsNullOrEmpty(cCTransactionsPGWDTO.TextResponse))
            {
                cCTransactionsPGWDTO.TextResponse = cCTransactionsPGWDTO.TextResponse.Replace("%20", " ").Replace("%2f", "/").Replace("%3a", ":").Replace("%2a", "X").Replace("%24", "$").Replace("%21", "!");
            }            
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }

        private CCTransactionsPGWDTO Performsale(AdyenRequest adyenRequest)
        {
            log.LogMethodEntry(adyenRequest);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            SaleData saleData = new SaleData();
            if (adyenRequest.TransactionType.Equals(PaymentGatewayTransactionType.AUTHORIZATION))
            {
                //if (string.IsNullOrEmpty(adyenRequest.TokenId))
                //{
                    saleData = new SaleData()
                    {
                        SaleToAcquirerData = "tenderOption=AskGratuity&merchantAccount="+ adyenRequest.MerchantAccountName + "&store="+ adyenRequest.MerchantCategoryCode,
                        SaleTransactionID = new TransactionIdentification()
                        {
                            TransactionID = adyenRequest.TransactionId,
                            TimeStamp = utilities.getServerTime()
                        },
                    };
                //}
                //else
                //{
                    //This will never come
                    //saleData = new SaleData()
                    //{
                    //    TokenRequestedType = TokenRequestedType.Customer,
                    //    SaleToAcquirerData = "tenderOption=AskGratuity",//&shopperReference=" + adyenRequest.CustomerId + "&recurringContract=RECURRING
                    //    SaleTransactionID = new TransactionIdentification()
                    //    {
                    //        TransactionID = adyenRequest.TransactionId,
                    //        TimeStamp = utilities.getServerTime(),
                    //    },
                    //};
               // }
            }
            else if (adyenRequest.TransactionType.Equals(PaymentGatewayTransactionType.TATokenRequest))
            {
                if (string.IsNullOrEmpty(adyenRequest.CustomerId))
                {
                    log.Error("Please register a customer before sending Pre-Authorization request.");
                    throw new Exception("Please register a customer before sending Pre-Authorization request.");
                }
                else
                {
                    saleData = new SaleData()
                    {
                        TokenRequestedType = TokenRequestedType.Customer,
                        SaleToAcquirerData = "tenderOption=AskGratuity&shopperReference=" + adyenRequest.CustomerId + "&recurringContract=RECURRING&merchantAccount="+ adyenRequest.MerchantAccountName + "&store="+ adyenRequest.MerchantCategoryCode,
                        SaleTransactionID = new TransactionIdentification()
                        {
                            TransactionID = adyenRequest.TransactionId,
                            TimeStamp = utilities.getServerTime()
                        },
                    };
                }
            }
            else
            {
                saleData = new SaleData()
                {
                    SaleToAcquirerData = "merchantAccount="+ adyenRequest.MerchantAccountName + "&store="+ adyenRequest.MerchantCategoryCode,
                    SaleTransactionID = new TransactionIdentification()
                    {
                        TransactionID = adyenRequest.TransactionId,
                        TimeStamp = utilities.getServerTime()
                    },
                };
            }
            TransactionConditions transactionConditions = null;
            if (adyenRequest.IsManualEntry)
            {
                transactionConditions = new TransactionConditions();               
                transactionConditions.ForceEntryMode = new ForceEntryModeType[] { ForceEntryModeType.Keyed};
            }
            var saleToPoiRequest = new SaleToPOIRequest();
            saleToPoiRequest.MessageHeader = new MessageHeader
            {
                MessageType = MessageType.Request,
                MessageClass = MessageClassType.Service,
                MessageCategory = MessageCategoryType.Payment,
                SaleID = adyenRequest.SaleId,
                POIID = adyenRequest.POIID,
                ServiceID = adyenRequest.ServiceId                
            };
            saleToPoiRequest.MessagePayload = new Adyen.Model.Nexo.PaymentRequest()
            {
                SaleData = saleData,
                PaymentTransaction = new PaymentTransaction()
                {
                    AmountsReq = new AmountsReq()
                    {
                        Currency = currencyCode,
                        RequestedAmount = adyenRequest.Amount
                    },
                    TransactionConditions = transactionConditions
                }                
            };
            string s = saleToPoiRequest.MessagePayload.ToString();
            var posPayment = new Adyen.Service.PosPaymentLocalApi((Client)client);
            
            log.LogVariableState("saleToPoiRequest", saleToPoiRequest);
            log.LogVariableState("saleData", saleData);
            var saleToPoiResponse = posPayment.TerminalApiLocal(saleToPoiRequest, encryptionCredentialDetails, RemoteServerCertificateValidationCallback);
            log.LogVariableState("saleToPoiResponse", saleToPoiResponse);            
            cCTransactionsPGWDTO = ParseSalesResponse(saleToPoiResponse, adyenRequest.TransactionType.ToString());
            GetReceiptText(saleToPoiResponse);
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }

        private CCTransactionsPGWDTO PerformRefund(AdyenRequest adyenRequest)
        {
            log.LogMethodEntry(adyenRequest);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            SaleData saleData = new SaleData();
            if (!string.IsNullOrEmpty(adyenRequest.OriginalTransactionId))
            {
                saleData.SaleToAcquirerData = "currency =" + currencyCode;
                saleData.SaleTransactionID = new TransactionIdentification()
                {
                    TransactionID = adyenRequest.OriginalTransactionId,
                    TimeStamp = adyenRequest.OriginalTrxnDateTime
                };
            }
            var saleToPoiRequest = new SaleToPOIRequest();
            saleToPoiRequest.MessageHeader = new MessageHeader
            {
                MessageType = MessageType.Request,
                MessageClass = MessageClassType.Service,
                MessageCategory = MessageCategoryType.Reversal,
                SaleID = adyenRequest.SaleId,
                POIID = adyenRequest.POIID,
                ServiceID = adyenRequest.ServiceId
            };
            var reversalRequest = new ReversalRequest()
            {
                OriginalPOITransaction = new OriginalPOITransaction()
                {
                    POITransactionID = new TransactionIdentification()
                    {
                        TransactionID = adyenRequest.OriginalResponseId,
                        TimeStamp = adyenRequest.OriginalTrxnDateTime,
                    },

                },
                ReversalReason = ReversalReasonType.MerchantCancel,
                ReversedAmount = adyenRequest.Amount,
                SaleData = (string.IsNullOrEmpty(adyenRequest.OriginalTransactionId)) ? null : saleData
            };
            saleToPoiRequest.MessagePayload = reversalRequest;
            var posPayment = new Adyen.Service.PosPaymentLocalApi((Client)client);
            var json = new JavaScriptSerializer().Serialize(saleToPoiRequest);
            log.LogVariableState("JsonRequest", json.ToString());
            log.LogVariableState("saleToPoiRequest", saleToPoiRequest);
            try
            {
                var saleToPoiResponse = posPayment.TerminalApiLocal(saleToPoiRequest, encryptionCredentialDetails, RemoteServerCertificateValidationCallback);
                log.LogVariableState("saleToPoiResponse", saleToPoiResponse);
                cCTransactionsPGWDTO = ParseReversalResponse(saleToPoiResponse, adyenRequest.TransactionType.ToString());
                try
                {
                    GetReversalReceiptText(saleToPoiResponse);
                }
                catch(Exception ex)
                {
                    log.Error("Receipt generation error", ex);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error in transaction processing. " + ex.Message);
            }            
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }       

        private CCTransactionsPGWDTO ParseRefundResponse(ModificationResult refundResponse, string trxType)
        {
            log.LogMethodEntry(refundResponse);

            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            Dictionary<string, string> aditionalResponse = new Dictionary<string, string>();
            if (refundResponse != null)
            {
                cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                if (refundResponse.PspReference != null)
                {
                    cCTransactionsPGWDTO.RefNo = refundResponse.PspReference;
                }
                    
                    cCTransactionsPGWDTO.TranCode = trxType;

                if (refundResponse.Status != null)
                {
                    cCTransactionsPGWDTO.DSIXReturnCode = refundResponse.Status;
                }
                cCTransactionsPGWDTO.TextResponse = refundResponse.Message;
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();                
            }
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }

        private CCTransactionsPGWDTO PerformTokenRequest(AdyenRequest adyenRequest)
        {
            log.LogMethodEntry(adyenRequest);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            var saleToPoiRequest = new SaleToPOIRequest()
            {
                MessageHeader = new MessageHeader
                {
                    MessageType = MessageType.Request,
                    MessageClass = MessageClassType.Service,
                    MessageCategory = MessageCategoryType.Payment,
                    SaleID = adyenRequest.SaleId,
                    POIID = adyenRequest.POIID,
                    ServiceID = adyenRequest.ServiceId
                },
                MessagePayload = new Adyen.Model.Nexo.PaymentRequest()
                {

                    SaleData = new SaleData()
                    {
                        SaleToAcquirerData = "shopperReference=" + adyenRequest.CustomerId + "&recurringContract=RECURRING",
                        SaleTransactionID = new TransactionIdentification()
                        {
                            TransactionID = adyenRequest.TransactionId,
                            TimeStamp = utilities.getServerTime()
                        }                       
                    },
                    PaymentTransaction = new PaymentTransaction()
                    {
                        AmountsReq = new AmountsReq()
                        {
                            Currency = currencyCode,
                            RequestedAmount = adyenRequest.Amount
                        }
                    },
                },
             };
        var posPayment = new Adyen.Service.PosPaymentLocalApi((Client)client);
        var saleToPoiResponse = posPayment.TerminalApiLocal(saleToPoiRequest, encryptionCredentialDetails, RemoteServerCertificateValidationCallback);
        cCTransactionsPGWDTO = ParseSalesResponse(saleToPoiResponse, adyenRequest.TransactionType.ToString());
        GetReceiptText(saleToPoiResponse);
        log.LogMethodExit(cCTransactionsPGWDTO);
        return cCTransactionsPGWDTO;
        }
        private CCTransactionsPGWDTO ParseReversalResponse(SaleToPOIResponse saleToPoiResponse, string trxType)
        {
            log.LogMethodEntry(saleToPoiResponse);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            Dictionary<string, string> aditionalResponse = new Dictionary<string, string>();
            if (saleToPoiResponse != null)
            {
                if (saleToPoiResponse.MessagePayload != null)
                {
                    cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.TranCode = trxType;
                    if (saleToPoiResponse.MessageHeader != null)
                    {
                        cCTransactionsPGWDTO.ProcessData = saleToPoiResponse.MessageHeader.SaleID;
                        cCTransactionsPGWDTO.AcqRefData = "ServiceId:" + saleToPoiResponse.MessageHeader.ServiceID + "|" + "POIID:" + saleToPoiResponse.MessageHeader.POIID;
                    }
                    ReversalResponse reversalResponse = (ReversalResponse)saleToPoiResponse.MessagePayload;
                    //paymentResponse.Response
                    if (reversalResponse != null)
                    {
                        cCTransactionsPGWDTO.DSIXReturnCode = reversalResponse.Response.Result.ToString();
                        if (reversalResponse.POIData != null && reversalResponse.POIData.POITransactionID.TransactionID != null)
                        {
                            cCTransactionsPGWDTO.TokenID = reversalResponse.POIData.POITransactionID.TransactionID;
                            cCTransactionsPGWDTO.TransactionDatetime = reversalResponse.POIData.POITransactionID.TimeStamp;
                        }                        

                        
                        if (reversalResponse.Response != null && reversalResponse.Response.AdditionalResponse != null)
                        {
                            string[] addResp = reversalResponse.Response.AdditionalResponse.Split('&');
                            string[] resp;
                            foreach (string s in addResp)
                            {
                                resp = s.Split('=');
                                aditionalResponse.Add(resp[0], resp[1]);
                            }
                            if (aditionalResponse.Keys.Contains("posAuthAmountValue"))
                            {
                                cCTransactionsPGWDTO.Authorize = aditionalResponse["posAuthAmountValue"];
                            }
                            if (aditionalResponse.Keys.Contains("pspReference"))
                            {
                                cCTransactionsPGWDTO.RefNo = aditionalResponse["pspReference"];
                                cCTransactionsPGWDTO.RecordNo= aditionalResponse["pspReference"];
                                if(cCTransactionsPGWDTO.RecordNo.Length==16)
                                {
                                    cCTransactionsPGWDTO.RecordNo = ("." + cCTransactionsPGWDTO.RecordNo).PadLeft(33, '0');
                                }
                            }
                            if (aditionalResponse.Keys.Contains("posOriginalAmountValue"))
                            {
                                cCTransactionsPGWDTO.Purchase = aditionalResponse["posOriginalAmountValue"];
                            }
                            if (aditionalResponse.Keys.Contains("store"))
                            {
                                cCTransactionsPGWDTO.AcqRefData += "|" + "MCC:" + aditionalResponse["store"];
                            }
                            log.LogVariableState("aditionalResponse", aditionalResponse);
                        }
                    }
                }
            }
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }

        private CCTransactionsPGWDTO ParseSalesResponse(SaleToPOIResponse saleToPoiResponse,string trxType)
        {
            log.LogMethodEntry(saleToPoiResponse);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            Dictionary<string, string> aditionalResponse = new Dictionary<string, string>();
            if (saleToPoiResponse != null)
            {
                if (saleToPoiResponse.MessagePayload != null)
                {
                    cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.TranCode = trxType;
                    if (saleToPoiResponse.MessageHeader != null)
                    {
                        cCTransactionsPGWDTO.ProcessData = saleToPoiResponse.MessageHeader.SaleID;
                        cCTransactionsPGWDTO.AcqRefData = "ServiceId:" + saleToPoiResponse.MessageHeader.ServiceID + "|" + "POIID:" + saleToPoiResponse.MessageHeader.POIID;
                    }
                    PaymentResponse paymentResponse = (PaymentResponse)saleToPoiResponse.MessagePayload;
                    //paymentResponse.Response
                    if (paymentResponse != null)
                    {
                        cCTransactionsPGWDTO.DSIXReturnCode = paymentResponse.Response.Result.ToString();
                        if (paymentResponse.SaleData != null && paymentResponse.SaleData.SaleTransactionID != null)
                        {
                            cCTransactionsPGWDTO.InvoiceNo = paymentResponse.SaleData.SaleTransactionID.TransactionID;
                            cCTransactionsPGWDTO.TransactionDatetime = paymentResponse.SaleData.SaleTransactionID.TimeStamp;
                        }
                        if (paymentResponse.PaymentResult != null && paymentResponse.PaymentResult.PaymentAcquirerData != null)
                        {
                            cCTransactionsPGWDTO.AuthCode = paymentResponse.PaymentResult.PaymentAcquirerData.ApprovalCode;
                            if (paymentResponse.PaymentResult.PaymentAcquirerData.AcquirerTransactionID != null)
                            {
                                cCTransactionsPGWDTO.RefNo = paymentResponse.PaymentResult.PaymentAcquirerData.AcquirerTransactionID.TransactionID;
                            }
                        }
                        if (paymentResponse.PaymentResult != null && paymentResponse.PaymentResult.PaymentInstrumentData != null && paymentResponse.PaymentResult.PaymentInstrumentData.CardData != null)
                        {
                            cCTransactionsPGWDTO.AcctNo = paymentResponse.PaymentResult.PaymentInstrumentData.CardData.MaskedPAN;
                            if (paymentResponse.PaymentResult.PaymentInstrumentData.CardData.EntryMode != null && paymentResponse.PaymentResult.PaymentInstrumentData.CardData.EntryMode.Length > 0)
                            {
                                cCTransactionsPGWDTO.CaptureStatus = paymentResponse.PaymentResult.PaymentInstrumentData.CardData.EntryMode[0].ToString();
                            }
                            cCTransactionsPGWDTO.UserTraceData = paymentResponse.PaymentResult.PaymentInstrumentData.CardData.PaymentBrand;
                            if (paymentResponse.PaymentResult.PaymentInstrumentData.CardData.PaymentToken != null)
                            {
                                cCTransactionsPGWDTO.TokenID = paymentResponse.PaymentResult.PaymentInstrumentData.CardData.PaymentToken.TokenValue;
                            }
                        }
                        if (paymentResponse.PaymentResult != null && paymentResponse.PaymentResult.AmountsResp != null)
                        {
                            if (paymentResponse.PaymentResult.AmountsResp.AuthorizedAmount.HasValue)
                            {
                                cCTransactionsPGWDTO.Authorize = paymentResponse.PaymentResult.AmountsResp.AuthorizedAmount.Value.ToString();
                            }
                            if (paymentResponse.PaymentResult.AmountsResp.TipAmount.HasValue)
                            {
                                cCTransactionsPGWDTO.TipAmount = paymentResponse.PaymentResult.AmountsResp.TipAmount.Value.ToString();
                            }
                        }

                        if (paymentResponse.POIData != null && paymentResponse.POIData.POITransactionID != null)
                        {
                            cCTransactionsPGWDTO.RecordNo = paymentResponse.POIData.POITransactionID.TransactionID;
                        }
                        if(paymentResponse.Response!=null && paymentResponse.Response.AdditionalResponse!=null)
                        {
                            string[] addResp = paymentResponse.Response.AdditionalResponse.Split('&');
                            string[] resp;
                            foreach (string s in addResp)
                            {
                                resp = s.Split('=');
                                aditionalResponse.Add(resp[0], resp[1]);
                            }
                            if (aditionalResponse.Keys.Contains("cardScheme"))
                            {
                                cCTransactionsPGWDTO.CardType = aditionalResponse["cardScheme"];
                            }
                            if (aditionalResponse.Keys.Contains("store"))
                            {
                                cCTransactionsPGWDTO.AcqRefData += "|" + "MCC:" + aditionalResponse["store"];
                            }
                            if (aditionalResponse.Keys.Contains("refusalReason"))
                            {
                                cCTransactionsPGWDTO.TextResponse += aditionalResponse["refusalReason"];
                            }
                            if (aditionalResponse.Keys.Contains("refusalReasonRaw"))
                            {
                                cCTransactionsPGWDTO.TextResponse += aditionalResponse["refusalReasonRaw"];
                            }
                            
                            log.LogVariableState("aditionalResponse", aditionalResponse);
                        }
                    }
                }
            }
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }
        class ReceiptLine
        {
            public string Key;
            public string Name;
            public string Value;
        }
        private void GetReceiptText(SaleToPOIResponse saleToPOIResponse)
        {
            log.LogMethodEntry(saleToPOIResponse);
            if (saleToPOIResponse != null)
            {
                PaymentResponse paymentResponse = (PaymentResponse)saleToPOIResponse.MessagePayload;
                if(paymentResponse.PaymentReceipt != null && paymentResponse.PaymentReceipt.Length>0)
                {
                    GenerateReceipt(paymentResponse.PaymentReceipt);
                }
            }
                log.LogMethodExit();
        }


        private void GetReversalReceiptText(SaleToPOIResponse saleToPOIResponse)
        {
            log.LogMethodEntry(saleToPOIResponse);
            if (saleToPOIResponse != null)
            {
                ReversalResponse reversalResponse = (ReversalResponse)saleToPOIResponse.MessagePayload;
                if (reversalResponse.PaymentReceipt != null && reversalResponse.PaymentReceipt.Length > 0)
                {
                    GenerateReceipt(reversalResponse.PaymentReceipt);
                }
            }
            log.LogMethodExit();
        }

        private void GenerateReceipt(PaymentReceipt[] paymentReceipts)
        {
            List<ReceiptLine> receiptLines = new List<ReceiptLine>();
            ReceiptLine receiptLine;
            string[] keyValueName;
            List<ReceiptLine> customerReceiptLines = new List<ReceiptLine>();
            List<ReceiptLine> merchantReceiptLines = new List<ReceiptLine>();
            foreach (PaymentReceipt paymentReceipt in paymentReceipts)
            {
                foreach (OutputText ot in paymentReceipt.OutputContent.OutputText)
                {
                    if (!string.IsNullOrEmpty(ot.Text))
                    {
                        keyValueName = ot.Text.Split('&');
                        if (keyValueName != null && keyValueName.Length > 0)
                        {
                            receiptLine = new ReceiptLine();
                            foreach (string s in keyValueName)
                            {
                                if (!string.IsNullOrEmpty(s))
                                {
                                    switch (s.Substring(0, s.IndexOf('=')))
                                    {
                                        case "name":
                                            receiptLine.Name = s.Substring(s.IndexOf('=') + 1).Replace("%20", " ").Replace("%21", "!").Replace("%2a", "--");
                                            break;
                                        case "key":
                                            receiptLine.Key = s.Substring(s.IndexOf('=') + 1);
                                            break;
                                        case "value":
                                            receiptLine.Value = s.Substring(s.IndexOf('=') + 1).Replace("%20", " ").Replace("%2f", "/").Replace("%3a", ":").Replace("%2a", "X").Replace("%24", "$").Replace("%21", "!");
                                            break;
                                    }
                                }
                            }
                            if (paymentReceipt.DocumentQualifier.Equals(DocumentQualifierType.CustomerReceipt))
                            {
                                customerReceiptLines.Add(receiptLine);
                            }
                            else if (paymentReceipt.DocumentQualifier.Equals(DocumentQualifierType.CashierReceipt))
                            {
                                merchantReceiptLines.Add(receiptLine);
                            }                           
                        }
                    }
                }              
                
            }
            customerReceipt = "";
            foreach (ReceiptLine receiptline in customerReceiptLines)
            {                
                if (string.IsNullOrEmpty(receiptline.Value) && !string.IsNullOrEmpty(receiptline.Name))
                {
                    customerReceipt += CCGatewayUtils.AllignText(receiptline.Name, Alignment.Center) + Environment.NewLine;
                }
                else if (!string.IsNullOrEmpty(receiptline.Value) && !string.IsNullOrEmpty(receiptline.Name))
                {
                    customerReceipt += CCGatewayUtils.AllignText(receiptline.Name.PadRight(15) + ": " + receiptline.Value, Alignment.Left) + Environment.NewLine;
                }

            }
            log.LogVariableState("customerReceipt", customerReceipt);
            merchantReceipt = "";
            foreach (ReceiptLine receiptline in merchantReceiptLines)
            {
                if (string.IsNullOrEmpty(receiptline.Value) && !string.IsNullOrEmpty(receiptline.Name))
                {
                    merchantReceipt += CCGatewayUtils.AllignText(receiptline.Name, Alignment.Center) + Environment.NewLine;
                }
                else if (!string.IsNullOrEmpty(receiptline.Value) && !string.IsNullOrEmpty(receiptline.Name))
                {
                    merchantReceipt += CCGatewayUtils.AllignText(receiptline.Name.PadRight(15) + ": " + receiptline.Value, Alignment.Left) + Environment.NewLine;
                }
            }
            log.LogVariableState("merchantReceipt", merchantReceipt);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to check for ssL errors. In case of CertificateNameMismatch, throw error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private static bool RemoteServerCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            log.LogMethodEntry(sender, certificate, chain, sslPolicyErrors);
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                log.LogMethodExit(true);
                return true;
            }

            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
        }
    }
    internal class AdyenRequest
    {
        private string saleId;
        private string serviceId;
        private string transactionId;
        private decimal amount;
        private string originalTransactionId;
        private string originalResponseId;
        private string tokenId;
        private string poiId;
        private string merchantAccountName;
        private string merchantCategoryCode;
        private string customerId;
        private PaymentGatewayTransactionType transactionType;
        private DateTime originalTrxnDatetime;
        private bool isManualEntry;

        /// <summary>
        /// Unique id for cash register 
        /// </summary>
        public string SaleId { get { return saleId; } set { saleId = value; } }
        /// <summary>
        /// Unique Id for 48 hours
        /// </summary>
        public string ServiceId { get { return serviceId; } set { serviceId = value; } }
        /// <summary>
        /// Unique Id for the transaction
        /// </summary>
        public string TransactionId { get { return transactionId; } set { transactionId = value; } }
        /// <summary>
        /// Unique Id for the transaction
        /// </summary>
        public string POIID { get { return poiId; } set { poiId = value; } }
        /// <summary>
        /// Merchant Account Name
        /// </summary>
        public string MerchantAccountName { get { return merchantAccountName; } set { merchantAccountName = value; } }
        /// <summary>
        /// Merchant Category Code(MCC)
        /// </summary>
        public string MerchantCategoryCode { get { return merchantCategoryCode; } set { merchantCategoryCode = value; } }
        /// <summary>
        /// poiId is the terminal id eg:P400Plus-275173685
        /// </summary>
        public decimal Amount { get { return amount; } set { amount = value; } }
        /// <summary>
        /// Original transaction reference
        /// </summary>
        public string OriginalResponseId { get { return originalResponseId; } set { originalResponseId = value; } }

        /// <summary>
        /// Original transaction id
        /// </summary>
        public string OriginalTransactionId { get { return originalTransactionId; } set { originalTransactionId = value; } }
        /// <summary>
        /// Token of the card retrived from preauthorization
        /// </summary>
        public string TokenId { get { return tokenId; } set { tokenId = value; } }
        /// <summary>
        /// Customer id
        /// </summary>
        public string CustomerId { get { return customerId; } set { customerId = value; } }
        /// <summary>
        /// Customer id
        /// </summary>
        public bool IsManualEntry { get { return isManualEntry; } set { isManualEntry = value; } }
        /// <summary>
        /// Transaction type
        /// </summary>
        public PaymentGatewayTransactionType TransactionType { get { return transactionType; } set { transactionType = value; } }

        /// <summary>
        /// Original Trxn Datetime
        /// </summary>
        public DateTime OriginalTrxnDateTime { get { return originalTrxnDatetime; } set { originalTrxnDatetime = value; } }
    }
}
