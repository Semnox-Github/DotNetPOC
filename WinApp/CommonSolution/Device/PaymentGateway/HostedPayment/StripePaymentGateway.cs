/********************************************************************************************
 * Project Name - Stripe Payment Gateway                                                                     
 * Description  - Class to handle the payment of Stripe Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai            Created for Guest app
 ********************************************************************************************/
using System;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Stripe;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class StripePaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int StripeErrorMessage = 2203;
        private HostedGatewayDTO hostedGatewayDTO;

        public StripePaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            
            this.hostedGatewayDTO = new HostedGatewayDTO();
            InitConfigurations();
            log.LogMethodExit(null);
        }

        private void InitConfigurations()
        {
            String stripeSecretKey = String.IsNullOrEmpty(Encryption.GetParafaitKeys("STRIPE_SECRET_KEY")) ? "" : Encryption.GetParafaitKeys("STRIPE_SECRET_KEY").ToString();
            StripeConfiguration.SetApiKey(stripeSecretKey);
        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            //this.hostedGatewayDTO.RequestURL = this.post_url;

            //CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            //this.hostedGatewayDTO.GatewayRequestString = SubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "frmPayPost");

            //log.LogMethodExit(this.hostedGatewayDTO);
            hostedGatewayDTO.PublicKey = String.IsNullOrEmpty(Encryption.GetParafaitKeys("STRIPE_PUBLISHABLE_KEY")) ? "" : Encryption.GetParafaitKeys("STRIPE_PUBLISHABLE_KEY").ToString();
            hostedGatewayDTO.TransactionPaymentsDTO = transactionPaymentsDTO;

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)transactionPaymentsDTO.Amount,
                Currency = transactionPaymentsDTO.CurrencyCode,
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            hostedGatewayDTO.GatewayRequestString = JsonConvert.SerializeObject(new { client_secret = paymentIntent.ClientSecret });
            return this.hostedGatewayDTO;
        }

        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            try
            {
                if (transactionPaymentsDTO != null)
                {
                    
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    if (ccRequestPGWDTO == null)
                    {
                        log.LogMethodExit("failed to create ccrequest ");
                        throw new Exception("Payment Failed");
                    }
                    Charge stripeCharge = null;

                    var stripeChargeOptions = new ChargeCreateOptions
                    {
                        Amount = (long)transactionPaymentsDTO.Amount * 100,
                        Currency = transactionPaymentsDTO.CurrencyCode.ToLower(),
                        Source = transactionPaymentsDTO.Reference,
                        Description = transactionPaymentsDTO.TransactionId.ToString() + "-" + transactionPaymentsDTO.SiteId.ToString(),
                        Capture = true
                    };

                    try
                    {
                        var stripeService = new ChargeService();
                        stripeCharge = stripeService.Create(stripeChargeOptions);
                    }
                    catch (StripeException ex)
                    {
                        String exception = GetCaptureType(ex);
                        log.Error(transactionPaymentsDTO.ToString() + "-" + ex.ToString());
                        throw new Exception(utilities.MessageUtils.getMessage(StripeErrorMessage));
                    }

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    if (stripeCharge != null && stripeCharge.Paid)
                    {
                        ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                        ccTransactionsPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                        ccTransactionsPGWDTO.TransactionDatetime = stripeCharge.Created;
                        ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTO.RecordNo = stripeCharge.BalanceTransactionId;
                        ccTransactionsPGWDTO.RefNo = stripeCharge.Id;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWDTO.RefNo = stripeCharge.Id;
                        ccTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                        ccTransactionsPGWDTO.Authorize = (stripeCharge.Amount / 100).ToString();
                        ccTransactionsPGWDTO.CaptureStatus = stripeCharge.Status;
                        ccTransactionsPGWDTO.TranCode = "Sale";
                        ccTransactionsPGWDTO.AuthCode = stripeCharge.AuthorizationCode;
                        ccTransactionsPGWDTO.TextResponse = stripeCharge.Status;
                        if (stripeCharge.Status.ToLower() == "succeeded")
                        {
                            ccTransactionsPGWDTO.CaptureStatus = "succeeded";
                            ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUCCESS);
                        }
                        else if (stripeCharge.Status.ToLower() == "pending")
                        {
                            ccTransactionsPGWDTO.CaptureStatus = "pending";
                            ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUBMITTED);
                        }
                        else
                        {
                            ccTransactionsPGWDTO.CaptureStatus = "failed";
                            ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_DECLINED);
                            log.Error(transactionPaymentsDTO.ToString() + stripeCharge != null ? stripeCharge.ToString() : "");
                            throw new Exception(utilities.MessageUtils.getMessage(StripeErrorMessage));
                        }

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();

                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                        if (Math.Abs(transactionPaymentsDTO.Amount - (stripeCharge.Amount / 100)) > 0.10)
                        {
                            try
                            {
                                ccTransactionsPGWDTO.CaptureStatus = "failed";
                                ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_DECLINED);
                                RefundAmount(transactionPaymentsDTO);
                            }
                            catch (Exception ex)
                            {
                                log.Error(transactionPaymentsDTO.ToString() + "-" + ex.ToString());
                            }
                            finally
                            {
                                throw new Exception("Stripe charge failed: amount has been refunded");
                            }
                        }
                    }
                    else
                    {
                        log.Error(transactionPaymentsDTO.ToString() + stripeCharge != null ? stripeCharge.ToString() : ""); 
                        throw new Exception(utilities.MessageUtils.getMessage("Invalid response."));
                    }
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    CCTransactionsPGWBL ccOriginalTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOriginalTransactionsPGWBL.CCTransactionsPGWDTO;
   
                    var stripeRefundOptions = new RefundCreateOptions()
                    {
                        Charge = ccOrigTransactionsPGWDTO.RefNo,
                        Amount = (long)transactionPaymentsDTO.Amount * 100
                    };
                    var stripeRefundService = new RefundService();
                    Refund refund = null;
                    try
                    {
                        refund = stripeRefundService.Create(stripeRefundOptions);
                    }
                    catch (StripeException ex)
                    {
                        String exception = GetCaptureType(ex);
                        log.Error(transactionPaymentsDTO.ToString() + "-" + ex.ToString());
                        throw new Exception(utilities.MessageUtils.getMessage(StripeErrorMessage));
                    }

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                    ccTransactionsPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                    ccTransactionsPGWDTO.TransactionDatetime = refund.Created;
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.RecordNo = refund.BalanceTransactionId;
                    ccTransactionsPGWDTO.RefNo = refund.Id;
                    transactionPaymentsDTO.Reference = ccTransactionsPGWDTO.RefNo = refund.Id;
                    ccTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                    ccTransactionsPGWDTO.Authorize = (refund.Amount / 100).ToString();
                    ccTransactionsPGWDTO.CaptureStatus = refund.Status;
                    ccTransactionsPGWDTO.TranCode = "VoidSale";
                    ccTransactionsPGWDTO.TextResponse = refund.Status;

                    if (refund.Status.ToLower() == "succeeded")
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "succeeded";
                        ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUCCESS);
                    }
                    else if (refund.Status.ToLower() == "pending")
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "pending";
                        ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUBMITTED);
                    }
                    else
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "failed";
                        log.Error(transactionPaymentsDTO.ToString() + refund != null ? refund.ToString() : "");
                        throw new Exception(utilities.MessageUtils.getMessage(StripeErrorMessage));
                    }

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();

                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.Amount = refund.Amount / 100;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw;
                }
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        private string GetCaptureType(StripeException e)
        {
            log.LogMethodEntry(e);
            string captureStatus = "";
            switch (e.StripeError.Type)
            {
                case "card_error":
                    captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
                    log.Error(captureStatus);
                    break;
                case "api_connection_error":
                    captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
                    log.Error(captureStatus);
                    break;
                case "api_error":
                    captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
                    log.Error(captureStatus);
                    break;
                case "authentication_error":
                    captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
                    log.Error(captureStatus);
                    break;
                case "invalid_request_error":
                    captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
                    log.Error(captureStatus);
                    break;
                case "rate_limit_error":
                    captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
                    log.Error(captureStatus);
                    break;
                case "validation_error":
                    captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
                    log.Error(captureStatus);
                    break;
                default:
                    captureStatus = "Unknown error!";
                    log.Error(captureStatus);
                    break;
            }
            log.LogMethodExit(captureStatus);
            return captureStatus;
        }


    }
}
