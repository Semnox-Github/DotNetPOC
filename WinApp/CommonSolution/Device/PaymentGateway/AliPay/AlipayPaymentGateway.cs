/********************************************************************************************
 * Project Name - AlipayPaymentGateway
 * Description  - AlipayPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80        18-Jul-2020      Jinto Thomas    Created           
 *2.100.0     01-Sep-2020      Guru S A        Payment link changes                               
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Device.PaymentGateway
{
  
    class AlipayPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string appId;
        string pid;
        string alipayPublicKey;
        string merchantPrivatekey;
        string merchantPublicKey;
        string serverUrl;
        string charSet;
        string signType;
        string version;
        IAlipayTradeService serviceClient;
        string status;
        string out_trade_no;

        private enum ResultStatus
        {
            SUCCESS,
            FAILED,
            UNKNOWN,
        }
        private enum TradeStatus
        {
            TRADE_SUCCESS,
            TRADE_CLOSED,
            WAIT_BUYER_PAY,
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public AlipayPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
           : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            SystemOptionsBL systemOptionsBl;
            try
            {
                systemOptionsBl = new SystemOptionsBL(utilities.ExecutionContext, "Payment Gateway Keys", "Gateway Public Key");
                if (systemOptionsBl.GetSystemOptionsDTO != null)
                {
                    string encryptedOptionValue = systemOptionsBl.GetSystemOptionsDTO.OptionValue;
                    alipayPublicKey = Encryption.Decrypt(encryptedOptionValue);
                }
                else
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Alipay Public key is not set"));
                }
                systemOptionsBl = new SystemOptionsBL(utilities.ExecutionContext, "Payment Gateway Keys", "Merchant Private Key");
                if (systemOptionsBl.GetSystemOptionsDTO != null)
                {
                    string encryptedOptionValue = systemOptionsBl.GetSystemOptionsDTO.OptionValue;
                    merchantPrivatekey = Encryption.Decrypt(encryptedOptionValue);
                }
                else
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Merchant private key is not set"));
                }
                systemOptionsBl = new SystemOptionsBL(utilities.ExecutionContext, "Payment Gateway Keys", "Merchant Public Key");
                if (systemOptionsBl.GetSystemOptionsDTO != null)
                {
                    string encryptedOptionValue = systemOptionsBl.GetSystemOptionsDTO.OptionValue;
                    merchantPublicKey = Encryption.Decrypt(encryptedOptionValue);
                }
                else
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Merchant public key is not set"));
                }

                appId = utilities.getParafaitDefaults("GATEWAY_APPID");
                pid = utilities.getParafaitDefaults("GATEWAY_PID");
                serverUrl = utilities.getParafaitDefaults("CREDIT_CARD_HOST_URL");
                if (string.IsNullOrEmpty(appId))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("AppId is not set"));
                }
                if (string.IsNullOrEmpty(pid))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Pid is not set"));
                }
                if (string.IsNullOrEmpty(serverUrl))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("serverUrl is not set"));
                }

                //alipayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnzMuFhrGG6zvebje1A9jXCgBEpJ6S4fS24GaEh8iZZ4nbQgiEBvsXh1nESMTKl0c1iKH7X/+PreLp1Z6lOBKCcQqYW/yIE9mrU4bvCLmQuon+jC5YctxphniSDiOI3aIqixMC7gxZVQFzZScZYdnHOMmnYLRRvDOCrhqY+gmxhD4xl1GuAwc5JtPOqbpBMFz+P9MLyNQqggC8wk5PAdxAN3Xd6KuIGivaLfX/jO1/jYQxyC6dCDrhvEDFEY0xpjoeZhQfreN89BlmWupinYbG0w/5/95YJdrcsiw3Ql4ViACl55/tZZraVkVXTuirXd9ky9K2TRdjmKzHvcOeWmD5wIDAQAB";
                //merchantPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhIHHRyKtkpr1SbcRdyRBOjhsNlsGAScG61DifC2qJg6vYdrIgYkQlgtHf0qB3liZDgkC0DS04WO3dnJl6b9wjwLZryFgpYj1Z7YABSQCm4AI11M3mQGRUPFHkN4oC/4Nl2NK9wEZxcAKPYuYeQ8zHofKODue4lcB9JyZjC1Rsz689/ZdPjXJ2qUjnWloSBzzcp33QhRGzasqvT2Rs7lJsYlho7LTw74eqmd+4ps2IK060PP4Go/70AHRhqJewLLL6Lr3ZNG8eTsBHHKeQ/Ozmyp2aMV3CG4LZRPblnsX9UZxtOtaxtGMlHvQWj+ifWQUEKNHwuf+PW06K2kxnvHtDQIDAQAB";
                //merchantPrivatekey = "MIIEowIBAAKCAQEAhIHHRyKtkpr1SbcRdyRBOjhsNlsGAScG61DifC2qJg6vYdrIgYkQlgtHf0qB3liZDgkC0DS04WO3dnJl6b9wjwLZryFgpYj1Z7YABSQCm4AI11M3mQGRUPFHkN4oC/4Nl2NK9wEZxcAKPYuYeQ8zHofKODue4lcB9JyZjC1Rsz689/ZdPjXJ2qUjnWloSBzzcp33QhRGzasqvT2Rs7lJsYlho7LTw74eqmd+4ps2IK060PP4Go/70AHRhqJewLLL6Lr3ZNG8eTsBHHKeQ/Ozmyp2aMV3CG4LZRPblnsX9UZxtOtaxtGMlHvQWj+ifWQUEKNHwuf+PW06K2kxnvHtDQIDAQABAoIBAAVaFIlr/iS4u4WBrmPog/XtB3nejUyInf/tIWiwk3m8CBtkscqBlbjptbaPdNVdMLlfZcyxBElCNMvE7RbW054DGHW5XGTzNi49LJ0Iik5rim/f/ZPhe1QQmrpgLq/lT/k4WnPFiJvzGLbJp5vkIEVwaJuC1PUBXKPbm1wNDIq3w4gGJTfsB+54agEKr7QwEZQRL76AFcuXFWU83qmn62miSVHd31U/0EjejNIIeZ+Xh6OZZD2JSDHPv6m9b+C6vHT911nJ3lSjN/6HGLelfgvHDQfA2q5fCe3XDcQTCb+aMnjoRl1Fy95l3G05Uux4F53RQk/5oa4PsWCTysKKQUUCgYEA8wf8YROegktn4UJL4f0PW/Ii2y5J3P1aDF3JZ7/cmGX/3GwCdhreeKMlPzwwary/mcSrAn3O2oRdOBZC+Lfi4+1u82qVbaSOuM3hwKpzqDxc0aRKOTVCIYaYsHWfj7FmrOHECqi67rElu7oMYffeEvLagghyovKevjppWaguNsMCgYEAi5PvzoLds3FrDi7ygwRgsOyhCh5yFeXlSOVHffIQnvZ1aJNawVv474+j9YLlEsIctMPYA4vHcUSvYqboxLNa3XdZ6L16e6ghuls5/lBEmHLpCDUp3Rb+n2lc73Ghz8Vg37N0wQaSTKFOpo7Q4WAwyqzukx/qgn2kRlYMYjtVL+8CgYBniDxk4qXBHfyIAuUuxc6YPePJecOvqCKHaCDX2O1R4woHzd+Sjsm9nMrOUIbTwcrh591uN0g64O6RaTJooHXA4bJFcl9sERFX+yU0Hakdv3FPQez4yA5/F8bOTZ2G6m0yMw9/9veDneXUPmLuDVkGu3yIrq6fajpkEaA1uKbPXwKBgH1GQRUD5gI5iNGJF4a6NiJ1r7BKVTEMTvdXOgxzZ7GVGRnML8eeSdaSAKHJYtqsOGGR6V59ZXtnH1cW4ZIyPBrMFXlMHxO/es4tNObpmjeN41PHi5RxIAVp5szOG2JFEEaXZfIdeM+oc9QQGLA/ymsOPW71VWDbbcbnUmQURsgnAoGBAKC8pSExTgxLjRRHjvuTuNwyxH3mncPYGQfgULlb01YlSuesaLdpI1sbxBWDm2G/AIMdKZ0zgRZsTX2tG0bQrCaU+gDXhAT5JDu4uPOPpNDd2q+rLvtQTd8ZhSQxQ4HPh0QhYnLpxcqEtwTH0oTxjYBYPsUVQilq/+cjWf4cRgt6";
                //appId = "2016091700533361";
                //pid = "2088102176057090";
                //serverUrl = "https://openapi.alipaydev.com/gateway.do";
                charSet = "utf-8";
                signType = "RSA2";
                version = "1.0";

                serviceClient = F2FBiz.CreateClientInstance(serverUrl, appId, merchantPrivatekey, version, signType, alipayPublicKey, charSet);
                log.LogMethodExit();
            }

            catch (Exception ex)
            {
                log.Error("Error occured while initialize gateway", ex);
                throw;
            }


            log.LogMethodExit(null);
        }

        /// <summary>
        /// Initialize method
        /// </summary>
        
        public override void Initialize()
        {
            log.LogMethodEntry();
            CheckLastTransactionStatus();
            log.LogMethodExit();
        }

        /// <summary>
        /// MakePayment method
        /// </summary>
        /// <param name="transactionPaymentsDTO">transactionPaymentsDTO</param>
        /// <returns> returns TransactionPaymentsDTO</returns>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO); 
            try
            {
                VerifyPaymentRequest(transactionPaymentsDTO);
                double required;
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                AlipayTradePayContentBuilder tradePayBuilder = BuildPayContent(ccRequestPGWDTO.RequestID, transactionPaymentsDTO);
                CCTransactionsPGWDTO ccTransactionsPGWDTO = null;
                CCTransactionsPGWBL ccTransactionsPGWBL = null;
                TradeResponse tradePaymentResponse = null;
                AlipayResponse alipayResponse = null;
                out_trade_no = tradePayBuilder.out_trade_no;
                required = Convert.ToDouble(tradePayBuilder.total_amount);
                string authCode = tradePayBuilder.auth_code;
                log.Debug("Payment In Process");
                AlipayF2FPayResult payResult = serviceClient.tradePay(tradePayBuilder);


                if (payResult.Status.Equals(ResultEnum.SUCCESS))
                {//success
                    alipayResponse = JsonConvert.DeserializeObject<AlipayResponse>(payResult.response.Body);

                    if (alipayResponse.Alipay_trade_pay_response != null)
                    {
                        tradePaymentResponse = alipayResponse.Alipay_trade_pay_response;
                    }
                    else
                    {
                        tradePaymentResponse = alipayResponse.Alipay_trade_query_response;
                    }
                    ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTO.RefNo = out_trade_no;
                    ccTransactionsPGWDTO.Authorize = tradePaymentResponse.Buyer_pay_amount;
                    ccTransactionsPGWDTO.AuthCode = tradePaymentResponse.Trade_no;
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                    ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    ccTransactionsPGWDTO.CardType = "Credit";
                    ccTransactionsPGWDTO.RecordNo = "A";
                    ccTransactionsPGWDTO.TextResponse = "Approved";
                    transactionPaymentsDTO.CreditCardNumber = authCode.Substring(authCode.Length - 4).PadLeft(authCode.Length, 'X');
                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;

                    ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();
                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                    if (Math.Abs(required - (Convert.ToDouble(tradePaymentResponse.Buyer_pay_amount))) > 0.1)
                    {
                        string message = "Error in transaction.. paid amount is less than required..Amount will be refunded ";
                        log.Error(message);
                        transactionPaymentsDTO.Amount = Convert.ToDouble(tradePaymentResponse.Buyer_pay_amount);
                        RefundAmount(transactionPaymentsDTO);
                        throw new Exception(message);
                    }

                    log.Debug("Payment Success");
                }

                else
                {
                    log.Debug("Failed");
                    
                    if (payResult.response.Code.Equals("40004")&& (string.IsNullOrEmpty(payResult.response.SubCode)))
                    {
                        throw new Exception("The remote name could not be resolved");
                    }
                    else
                    {
                        alipayResponse = JsonConvert.DeserializeObject<AlipayResponse>(payResult.response.Body);
                        if (alipayResponse.Alipay_trade_pay_response != null)
                        {
                            tradePaymentResponse = alipayResponse.Alipay_trade_pay_response;
                        }
                        else
                        {
                            tradePaymentResponse = alipayResponse.Alipay_trade_query_response;
                        }
                        if (string.IsNullOrEmpty(tradePaymentResponse.Sub_code))
                        {
                            string status = tradePaymentResponse.Trade_status;
                            if (status.Equals(TradeStatus.WAIT_BUYER_PAY.ToString()))
                            {
                                throw new Exception(tradePaymentResponse.Trade_status);
                            }
                            else
                            {
                                throw new Exception(tradePaymentResponse.Sub_code);
                            }
                        }
                        else
                        {
                            throw new Exception(tradePaymentResponse.Sub_code);
                        }

                    }
                }

                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while making payment", ex);
                log.LogMethodExit(null, "Throwing Payment Gateway Exception-" + ex.Message);
                throw new PaymentGatewayException(ex.Message);
            }
        }   

        public TradeResponse QueryPayment(string out_trade_no)
        {
            log.LogMethodEntry(out_trade_no);
            AlipayF2FQueryResult queryResult = serviceClient.tradeQuery(out_trade_no);
            AlipayResponse tradeQueryRresponse = JsonConvert.DeserializeObject<AlipayResponse>(queryResult.response.Body);
            log.LogMethodExit(tradeQueryRresponse);
            return tradeQueryRresponse.Alipay_trade_query_response;
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            try
            {
                if(transactionPaymentsDTO != null)
                {
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                    
                    double amount = Math.Round(transactionPaymentsDTO.Amount,2);
                    AlipayTradeRefundContentBuilder builder = new AlipayTradeRefundContentBuilder();
                    builder.out_trade_no = ccOrigTransactionsPGWDTO.RefNo;
                    builder.out_request_no = utilities.ParafaitEnv.SiteId + "-" + cCRequestPGWDTO.RequestID;
                    builder.refund_amount = amount.ToString();
                    log.Debug("Refund in Process");
                    AlipayF2FRefundResult refundResult = serviceClient.tradeRefund(builder);

                    AlipayResponse alipayResponse = JsonConvert.DeserializeObject<AlipayResponse>(refundResult.response.Body);
                                                           
                    if (refundResult.Status.Equals(ResultEnum.SUCCESS))
                    {//success
                        
                        if (Convert.ToDouble(alipayResponse.Alipay_trade_refund_response.Send_back_fee) > 0)
                        {
                            TradeResponse tradeQueryResponse = QueryPayment(builder.out_trade_no);
                            
                            string status = tradeQueryResponse.Trade_status;
                            if (status.Equals(TradeStatus.TRADE_CLOSED.ToString()) || status.Equals(TradeStatus.TRADE_SUCCESS.ToString()))
                            {
                                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                                ccTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                                ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                                ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                ccTransactionsPGWDTO.RecordNo = "A";
                                ccTransactionsPGWDTO.TextResponse = "Approved";
                                ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                log.Debug("Refund Success");
                            }
                            else
                            {
                                log.Debug("Refund Failed. Requested trade has been closed");
                                throw new Exception(tradeQueryResponse.Sub_code);
                            }
                        }
                        else
                        {
                            log.Debug("Error in refund, refunded amount is not valid");
                            throw new Exception("Error in refund, refunded amount is not valid");
                        }
                    }
                    else 
                    {
                        log.Debug("Refund Failed");
                        throw new Exception(alipayResponse.Alipay_trade_refund_response.Sub_code);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while making refund", ex);
                log.LogMethodExit(null, "Throwing Payment Gateway Exception-" + ex.Message);
                throw new PaymentGatewayException(ex.Message);
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public override void SendLastTransactionStatusCheckRequest(CCRequestPGWDTO cCRequestPGWDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);
            try
            {
                string amount = string.Empty;
                string authCode = string.Empty;
                string status = string.Empty;
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                
                log.Debug(utilities.MessageUtils.getMessage("Checking the transaction status" + ((cCRequestPGWDTO != null) ? " of TrxId:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")));
                
                
                CCTransactionsPGWDTO ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();
                if (cCRequestPGWDTO != null)
                {
                    log.Debug("cCRequestPGWDTO is not null");

                    out_trade_no = utilities.ParafaitEnv.SiteId + "_" + cCRequestPGWDTO.RequestID;
                    TradeResponse tradeQueryResponse = QueryPayment(out_trade_no);
                    
                    if (tradeQueryResponse.Code.ToString().Equals("10000"))
                    {
                        status = tradeQueryResponse.Trade_status;
                        amount = tradeQueryResponse.Buyer_pay_amount;
                        authCode = tradeQueryResponse.Trade_no;
                    }
                    else
                    {
                        log.Debug("There is no transaction exists");
                        log.Error("Last transaction status is not available.");
                        return;
                    }
                }
                
                if (status.Equals(TradeStatus.TRADE_CLOSED.ToString()) || status.Equals(TradeStatus.TRADE_SUCCESS.ToString()))
                {
                    log.Debug("There is transaction exist");
                    try
                    {
                        log.LogVariableState("query response", status);
                        ccTransactionsPGWDTOResponse.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTOResponse.RefNo = out_trade_no;
                        ccTransactionsPGWDTOResponse.Authorize = amount;
                        ccTransactionsPGWDTOResponse.AuthCode = authCode;
                        ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                        ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                        ccTransactionsPGWDTOResponse.CardType = "credit";
                        ccTransactionsPGWDTOResponse.RecordNo = "A";
                        ccTransactionsPGWDTOResponse.TextResponse = "Approved";
                        
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
                    catch (Exception ex)
                    {
                        log.Debug("Exception one");
                        log.Error("Last transaction check failed", ex);
                        throw;
                    }
                }
                else
                {
                    log.Debug("There is no transaction exists");
                    log.Error("Last transaction status is not available.");
                    return;
                }
            }
            catch
            {
                log.Debug("Exception two");
                throw;
            }

            log.LogMethodExit();
        }
        private AlipayTradePayContentBuilder BuildPayContent(int requestId, TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            AlipayTradePayContentBuilder builder = new AlipayTradePayContentBuilder();
                        
            builder.seller_id = pid;
            builder.out_trade_no = utilities.ParafaitEnv.SiteId + "_" + requestId;
            builder.scene = "bar_code";
            builder.subject = PaymentGatewayTransactionType.SALE.ToString();
            builder.auth_code = transactionPaymentsDTO.CreditCardNumber;           
            builder.total_amount = Convert.ToString(transactionPaymentsDTO.Amount);            
            builder.timeout_express = "3m";           
                        
            log.LogMethodExit(builder);
            return builder;
        }
    }
}
