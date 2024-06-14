/********************************************************************************************
* Project Name - Semnox.Parafait.Device.PaymentGateway - CyberSourcePaymentGateway
* Description  - CyberSource payment gateway class
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
*********************************************************************************************
*2.50.0       25-Jan-2019      Archana            Created
*2.150.1      22-Feb-2023      Guru S A           Kiosk Cart Enhancements
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class CyberSourcePaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDisplayStatusUI statusDisplayUi;
        bool isAuthEnabled = false;
        public CyberSourcePaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            this.isUnattended = isUnattended;
            isAuthEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ALLOW_CREDIT_CARD_AUTHORIZATION");
            log.LogMethodExit();
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            try
            {
                CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
                CyberSourceRequest cyberSourceRequest = new CyberSourceRequest();
                CCTransactionsPGWBL cCTransactionsPGWBL = null;

                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                try
                {
                    StandaloneRefundNotAllowed(transactionPaymentsDTO);
                    if (!isUnattended)
                    {
                        if (isAuthEnabled)
                        {
                            frmTransactionTypeUI frmTranType = new frmTransactionTypeUI(utilities, "Authorization", transactionPaymentsDTO.Amount, showMessageDelegate);
                            if (frmTranType.ShowDialog() != DialogResult.Cancel)
                            {
                                if (frmTranType.TransactionType == "Sale")
                                {
                                    cyberSourceRequest.EcrTransactionType = "1";
                                }
                                else if (frmTranType.TransactionType == "Authorization")
                                {
                                    cyberSourceRequest.EcrTransactionType = "5";
                                }
                            }
                            else
                            {
                                log.LogMethodExit(transactionPaymentsDTO);
                                throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                            }
                        }
                        else
                        {
                            cyberSourceRequest.EcrTransactionType = "1";
                        }
                    }

                    cyberSourceRequest.Amount = (transactionPaymentsDTO.Amount * 100).ToString("##0");
                    cyberSourceRequest.EcrTid = "0";
                    cyberSourceRequest.EcrTrxCompletionAmount = (transactionPaymentsDTO.Amount * 100).ToString("##0");
                    cyberSourceRequest.EcrOriginalReceiptNo = ccRequestPGWDTO.RequestID.ToString();
                    cyberSourceRequest.EcrReceiptNumber = ccRequestPGWDTO.RequestID.ToString();
                    cyberSourceRequest.ReferenceNumber = ccRequestPGWDTO.RequestID;
                    cyberSourceRequest.TransactionType = TransactionType.SALE;
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "Cyber Source Payment Gateway");
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    statusDisplayUi.DisplayText("Please insert your card and follow device instruction");
                    ICyberSourceHandler cyberSourceHandler = GetCyberSourceHandler(isUnattended);
                    cCTransactionsPGWDTO = cyberSourceHandler.ProcessTransaction(cyberSourceRequest);
                    if (isUnattended)
                    {
                        // Print(GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, false), false);
                        GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, false);
                    }
                    if (cCTransactionsPGWDTO != null)
                    {
                        cCTransactionsPGWDTO.RefNo = ccRequestPGWDTO.RequestID.ToString();
                        cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        cCTransactionsPGWBL.Save();
                        if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode.ToString() == "000")
                        {
                            transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                            transactionPaymentsDTO.CreditCardName = cCTransactionsPGWDTO.UserTraceData;
                            transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.InvoiceNo;
                            if (isUnattended)
                            {
                                statusDisplayUi.DisplayText(cCTransactionsPGWDTO.TextResponse);
                            }
                        }
                        else
                        {
                            log.LogMethodExit("Transaction failed");
                            if (statusDisplayUi != null)
                                statusDisplayUi.DisplayText((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                            throw new Exception((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                        }
                    }
                    else
                        throw new Exception("Payment transaction failed");
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing PaymentGatewayException - " + ex);
                    throw new PaymentGatewayException(ex.Message,ex);
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.DisplayText(ex.Message);
                }
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                log.Debug("Closing status window");
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }
        }

        private ICyberSourceHandler GetCyberSourceHandler(bool isUnattended)
        {
            if (isUnattended)
            {
                return new CyberSourceKioskHandler();
            }
            else
            {
                return new CyberSourcePOSHandler();
            }
        }


        /// <summary>
        /// Returns boolean based on whether payment requires a settlement to be done.
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override bool IsSettlementPending(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            bool returnValue = false;
            if (isAuthEnabled)
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.CCResponseId != -1)
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, "AUTH"));
                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                    if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                    {
                        returnValue = true;
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                CCTransactionsPGWDTO cCTransactionsPGWDTO = null;

                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
                CyberSourceRequest cyberSourceRequest = new CyberSourceRequest();
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                CCTransactionsPGWBL cCTransactionsPGWBL = null;

                cyberSourceRequest.TransactionType = TransactionType.VOID;
                cyberSourceRequest.OriginalTrxNumber = transactionPaymentsDTO.Reference;
                cyberSourceRequest.ReferenceNumber = cCRequestPGWDTO.RequestID;
                cyberSourceRequest.Amount = (transactionPaymentsDTO.Amount * 100).ToString("##0");
                cyberSourceRequest.EcrTid = "0";
                cyberSourceRequest.EcrTrxCompletionAmount = (transactionPaymentsDTO.Amount * 100).ToString("##0");
                cyberSourceRequest.EcrOriginalReceiptNo = transactionPaymentsDTO.Reference;
                cyberSourceRequest.EcrReceiptNumber = cCRequestPGWDTO.RequestID.ToString();
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "Cyber Source Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                ICyberSourceHandler cyberSourceHandler = GetCyberSourceHandler(isUnattended);
                if ((Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize)) == transactionPaymentsDTO.Amount)
                {
                    cCTransactionsPGWDTO = cyberSourceHandler.VoidTransaction(cyberSourceRequest);
                    if (isUnattended)
                    {
                        //Print(GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, false), false);
                        GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, false);
                    }
                }
                else if (!isUnattended)
                {
                    cyberSourceRequest.EcrTransactionType = "2";
                    cCTransactionsPGWDTO = cyberSourceHandler.ProcessTransaction(cyberSourceRequest);
                }
                else
                {
                    log.LogMethodExit("Partial refund is not allowed");
                    if (statusDisplayUi != null)
                    {
                        statusDisplayUi.DisplayText("Partial refund is not allowed.");
                    }
                    throw new Exception("Partial refund is not allowed");
                }

                cCTransactionsPGWDTO.RefNo = cCRequestPGWDTO.RequestID.ToString();
                cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                cCTransactionsPGWBL.Save();

                if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode.ToString() == "000")
                {
                    transactionPaymentsDTO.CreditCardNumber = ccOrigTransactionsPGWDTO.AcctNo;
                    transactionPaymentsDTO.CreditCardName = ccOrigTransactionsPGWDTO.UserTraceData;
                    transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.InvoiceNo;
                    statusDisplayUi.DisplayText(cCTransactionsPGWDTO.TextResponse);
                }
                else
                {
                    log.LogMethodExit("ccTransactionsPGWDTO is null");
                    statusDisplayUi.DisplayText((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                    throw new Exception((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                }

                log.LogMethodExit();
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            finally
            {
                log.Debug("Closing status window");
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }
        }


        /// <summary>
        /// Performs settlement.
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="IsForcedSettlement"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO PerformSettlement(TransactionPaymentsDTO transactionPaymentsDTO, bool IsForcedSettlement = false)
        {
            log.LogMethodEntry(transactionPaymentsDTO, IsForcedSettlement);
            TransactionPaymentsDTO returnTransactionPaymentsDTO = null;
            try
            {
                if (!isUnattended)
                {
                    CyberSourceRequest cyberSourceRequest = new CyberSourceRequest();
                    if (transactionPaymentsDTO != null)
                    {
                        if (!IsForcedSettlement)
                        {
                            Semnox.Parafait.Device.PaymentGateway.frmTransactionTypeUI frmTranType = new Semnox.Parafait.Device.PaymentGateway.frmTransactionTypeUI(utilities, "Completion", transactionPaymentsDTO.Amount, showMessageDelegate);
                            if (frmTranType.ShowDialog() != DialogResult.Cancel)
                            {
                                transactionPaymentsDTO.TipAmount = frmTranType.TipAmount;
                                cyberSourceRequest.EcrTransactionType = "6";
                            }
                            else
                            {
                                log.LogMethodExit(returnTransactionPaymentsDTO);
                                throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                            }
                        }
                        CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = cCTransactionsPGWBL.CCTransactionsPGWDTO;
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                        cyberSourceRequest.EcrTid = "0";
                        cyberSourceRequest.EcrTrxCompletionAmount = (transactionPaymentsDTO.TipAmount * 100).ToString("##0");
                        cyberSourceRequest.Amount = (transactionPaymentsDTO.TipAmount * 100).ToString("##0");
                        cyberSourceRequest.EcrOriginalReceiptNo = transactionPaymentsDTO.Reference;
                        cyberSourceRequest.EcrReceiptNumber = cCRequestPGWDTO.RequestID.ToString();
                        statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "Cyber Source Payment Gateway");
                        statusDisplayUi.EnableCancelButton(false);
                        Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                        thr.Start();
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                        ICyberSourceHandler cyberSourceHandler = GetCyberSourceHandler(isUnattended);
                        cCTransactionsPGWDTO = cyberSourceHandler.ProcessTransaction(cyberSourceRequest);
                        if (cCTransactionsPGWDTO != null)
                        {
                            cCTransactionsPGWDTO.RefNo = cCRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            cCTransactionsPGWBL.Save();
                            if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode.ToString() == "000")
                            {
                                transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                                transactionPaymentsDTO.CreditCardName = cCTransactionsPGWDTO.UserTraceData;
                                transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.InvoiceNo;
                                statusDisplayUi.DisplayText(cCTransactionsPGWDTO.TextResponse);
                            }
                            else
                            {
                                log.LogMethodExit("ccTransactionsPGWDTO is null");
                                statusDisplayUi.DisplayText((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                                throw new Exception((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                            }
                        }
                    }
                }
                log.LogMethodExit(returnTransactionPaymentsDTO);
                return returnTransactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            finally
            {
                log.Debug("Closing status window");
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }
        }

        protected override string GetReceiptText(TransactionPaymentsDTO trxPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO, bool IsMerchantCopy)
        {
            log.LogMethodEntry();
            try
            {
                string[] addressArray = utilities.ParafaitEnv.SiteAddress.Split(',');
                string receiptText = "";
                receiptText += CCGatewayUtils.AllignText(utilities.ParafaitEnv.SiteName, Alignment.Center);
                if (addressArray != null && addressArray.Length > 0)
                {
                    for (int i = 0; i < addressArray.Length; i++)
                    {
                        receiptText += Environment.NewLine + CCGatewayUtils.AllignText(addressArray[i] + ((i != addressArray.Length - 1) ? "," : "."), Alignment.Center);
                    }
                }
                receiptText += Environment.NewLine;
                string maskedMerchantId = (new String('X', 8) + ((ccTransactionsPGWDTO.AcqRefData.Length > 4) ? ccTransactionsPGWDTO.AcqRefData.Substring(ccTransactionsPGWDTO.AcqRefData.Length - 4)
                                                                                         : ccTransactionsPGWDTO.AcqRefData));
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Merchant ID") + ": ".PadLeft(6) + maskedMerchantId, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Terminal ID") + ": ".PadLeft(6) + ccTransactionsPGWDTO.RecordNo, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Transaction Date") + ": ".PadLeft(6) + ccTransactionsPGWDTO.TransactionDatetime.ToString("MMM dd yyyy HH:mm"), Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Transaction Type") + ": ".PadLeft(6) + ccTransactionsPGWDTO.TranCode, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Invoice Number") + ": ".PadLeft(6) + ccTransactionsPGWDTO.InvoiceNo, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Total") + " : " + Convert.ToDouble(ccTransactionsPGWDTO.Purchase).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);

                receiptText += Environment.NewLine;
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage((ccTransactionsPGWDTO.DSIXReturnCode == "000" ? "APPROVED" : "DECLINED")), Alignment.Center);


                receiptText += Environment.NewLine;
                
                //receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Cardholder Name") + ": ".PadLeft(6) + ccTransactionsPGWDTO.UserTraceData, Alignment.Left);
                string maskedPAN = ((string.IsNullOrEmpty(ccTransactionsPGWDTO.AcctNo) ? ccTransactionsPGWDTO.AcctNo
                                                                             : (new String('X', 12) + ((ccTransactionsPGWDTO.AcctNo.Length > 4)
                                                                                                     ? ccTransactionsPGWDTO.AcctNo.Substring(ccTransactionsPGWDTO.AcctNo.Length - 4)
                                                                                                     : ccTransactionsPGWDTO.AcctNo))));
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Card Number") + ": ".PadLeft(6) + maskedPAN, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Card Type") + ": ".PadLeft(6) + ccTransactionsPGWDTO.CardType, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("AID") + ": ".PadLeft(6) + ccTransactionsPGWDTO.TokenID, Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("TVR") + ": ".PadLeft(6) + ccTransactionsPGWDTO.ProcessData, Alignment.Left);
                receiptText += Environment.NewLine;
                if (IsMerchantCopy)
                {
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Cardholder Signature"), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText("_______________________", Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage(1180), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText("**" + utilities.MessageUtils.getMessage("Merchant Copy") + "**", Alignment.Center);

                }
                else
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("IMPORTANT— retain this copy for your records"), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText("**" + utilities.MessageUtils.getMessage("Cardholder Copy") + " **", Alignment.Center);
                }

                receiptText += Environment.NewLine;
                receiptText += CCGatewayUtils.AllignText(" " + utilities.MessageUtils.getMessage("Thank You"), Alignment.Center);
                log.LogMethodExit(receiptText);
                if (IsMerchantCopy)
                {
                    ccTransactionsPGWDTO.MerchantCopy = receiptText;
                }
                else
                {
                    ccTransactionsPGWDTO.CustomerCopy = receiptText;
                }
                return receiptText;
            }
            catch (Exception ex)
            {
                log.Fatal("GetReceiptText() failed to print receipt exception:" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Settle Transaction Payment
        /// </summary>
        public override TransactionPaymentsDTO SettleTransactionPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            CCTransactionsPGWDTO ccTransactionsPGWDTO = null;
            TransactionPaymentsDTO returnTransactionPaymentsDTO = null;
            try
            {
                if (!isUnattended)
                {
                    CyberSourceRequest cyberSourceRequest = new CyberSourceRequest();
                    if (transactionPaymentsDTO != null)
                    {
                        CanAdjustTransactionPayment(transactionPaymentsDTO);
                        CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = cCTransactionsPGWBL.CCTransactionsPGWDTO;
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                        cyberSourceRequest.EcrTid = "0";
                        cyberSourceRequest.EcrTrxCompletionAmount = (transactionPaymentsDTO.TipAmount * 100).ToString("##0");
                        cyberSourceRequest.Amount = (transactionPaymentsDTO.TipAmount * 100).ToString("##0");
                        cyberSourceRequest.EcrOriginalReceiptNo = transactionPaymentsDTO.Reference;
                        cyberSourceRequest.EcrReceiptNumber = cCRequestPGWDTO.RequestID.ToString();

                        ICyberSourceHandler cyberSourceHandler = GetCyberSourceHandler(isUnattended);
                        cCTransactionsPGWDTO = cyberSourceHandler.ProcessTransaction(cyberSourceRequest);
                        if (cCTransactionsPGWDTO != null)
                        {
                            cCTransactionsPGWDTO.RefNo = cCRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            cCTransactionsPGWBL.Save();
                            if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode.ToString() == "000")
                            {
                                transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                                transactionPaymentsDTO.CreditCardName = cCTransactionsPGWDTO.UserTraceData;
                                transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.InvoiceNo;
                                statusDisplayUi.DisplayText(cCTransactionsPGWDTO.TextResponse);
                            }
                            else
                            {
                                log.LogMethodExit("ccTransactionsPGWDTO is null");
                                statusDisplayUi.DisplayText((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                                throw new Exception((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                            }
                        }
                    }
                    else
                    {
                        log.LogMethodExit("Transaction payment info is missing");
                        throw new Exception("Transaction payment info is missing");
                    }
                }
                log.LogMethodExit(returnTransactionPaymentsDTO);
                return returnTransactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public override void CanAdjustTransactionPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            if (transactionPaymentsDTO != null)
            {
                string limit = utilities.getParafaitDefaults("MAX_TIP_AMOUNT_PERCENTAGE");
                long tipLimit = Convert.ToInt64(string.IsNullOrEmpty(limit) ? "200" : limit);
                if (tipLimit > 0 && ((transactionPaymentsDTO.Amount * tipLimit) / 100) < transactionPaymentsDTO.TipAmount)
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Tip limit exceeded"));
                }
            }
            else
            {
                throw new Exception("Transaction payment info is missing");
            }
            log.LogMethodExit();
        }

    }
}
