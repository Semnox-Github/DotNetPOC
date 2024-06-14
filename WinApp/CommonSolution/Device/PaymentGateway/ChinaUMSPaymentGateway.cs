//using Semnox.Parafait.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Semnox.Core.Utilities;
//using Semnox.Parafait.TransactionPayments;


namespace Semnox.Parafait.Device.PaymentGateway
{
    class ChinaUMSPaymentGateway : PaymentGateway
    {
        ChinaUMS chinaUMS = null;
        string Message = "";

        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public ChinaUMSPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate) : base(utilities, isUnattended, showMessageDelegate,writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);

            chinaUMS = new ChinaUMS(utilities);

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Makes payment.
        /// </summary>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            StandaloneRefundNotAllowed(transactionPaymentsDTO);
            CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_PURCHASE);
            ChinaUMSTransactionResponse transactionResponse = new ChinaUMSTransactionResponse();
            ChinaUMSTransactionRequest transactionRequest = new ChinaUMSTransactionRequest();
            transactionRequest.TerminalID = (utilities.ParafaitEnv.POSMachineId == -1) ? utilities.ParafaitEnv.POSMachine : utilities.ParafaitEnv.POSMachineId.ToString();
            transactionRequest.CashierNo = utilities.ParafaitEnv.User_Id.ToString().PadLeft(8);
            transactionRequest.CustomInformation = "";
            transactionRequest.OldTransactionDate = new DateTime();
            transactionRequest.OriginalReferenceNo = "";
            transactionRequest.OriginalSalesDraft = "";
            long.TryParse((transactionPaymentsDTO.Amount * 100).ToString(), out transactionRequest.TransactionAmount);
            try
            {
                if (transactionRequest.TransactionAmount != 0)
                {
                    chinaUMS.PerformSale(transactionRequest, ref transactionResponse, ref Message);
                    if (transactionResponse != null)
                    {
                        if (transactionResponse.ResponseCode.Equals("00") && transactionResponse.ResponseCode != null)
                        {
                            if (!(Math.Round(transactionPaymentsDTO.Amount, utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero) != Math.Round((Convert.ToDouble((transactionResponse.TransactionAmount)) / 100), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero)))
                            {
                                transactionPaymentsDTO.CreditCardNumber = transactionResponse.CardNo;
                                transactionPaymentsDTO.CreditCardAuthorization = transactionResponse.AuthorizationNo;
                                transactionPaymentsDTO.Reference = transactionResponse.ReferenceNo;
                                transactionPaymentsDTO.CCResponseId = transactionResponse.ccResponseId;
                                transactionPaymentsDTO.CreditCardName = transactionResponse.CardType;
                                if (!string.IsNullOrEmpty(transactionResponse.ReceiptText))
                                    transactionPaymentsDTO.Memo = transactionResponse.ReceiptText;
                            }
                        }
                        else
                        {
                            throw new Exception(utilities.MessageUtils.getMessage(Message));
                        }
                    }
                    else
                    {
                        throw new Exception(utilities.MessageUtils.getMessage(Message));
                    }
                }
                else
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Nothing to pay"));
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to make payment for the transaction");
                log.LogMethodExit("Throwing PaymentGatewayException - "+Message);
                throw new PaymentGatewayException(ex.Message);
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        /// <summary>
        /// Reverts the payment.
        /// </summary>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
            ChinaUMSTransactionResponse transactionResponse = new ChinaUMSTransactionResponse();
            ChinaUMSTransactionRequest transactionRequest = new ChinaUMSTransactionRequest();
            transactionRequest.TerminalID = (utilities.ParafaitEnv.POSMachineId == -1) ? utilities.ParafaitEnv.POSMachine : utilities.ParafaitEnv.POSMachineId.ToString();
            transactionRequest.CashierNo = utilities.ParafaitEnv.User_Id.ToString().PadLeft(8);
            transactionRequest.CustomInformation = "";// will be fetched inside chinaUMS dll
            transactionRequest.OldTransactionDate = new DateTime();
            transactionRequest.OriginalReferenceNo = "";// will be fetched inside chinaUMS dll
            transactionRequest.OriginalSalesDraft = "";// will be fetched inside chinaUMS dll
            transactionRequest.ccResponseId = (transactionPaymentsDTO.CCResponseId == 0) ? -1 : (int)transactionPaymentsDTO.CCResponseId;
            long.TryParse((transactionPaymentsDTO.Amount * 100).ToString(), out transactionRequest.TransactionAmount);
            try
            {
                if (transactionRequest.TransactionAmount != 0)
                {
                    chinaUMS.PerformRefund(transactionRequest, ref transactionResponse, ref Message);
                    if (transactionResponse != null)
                    {
                        if (transactionResponse.ResponseCode.Equals("00") && transactionResponse.ResponseCode != null)
                        {
                            if (!(Math.Round(transactionPaymentsDTO.Amount, utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero) != Math.Round((Convert.ToDouble((transactionResponse.TransactionAmount)) / 100), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero)))
                            {
                                transactionPaymentsDTO.CreditCardNumber = transactionResponse.CardNo;
                                transactionPaymentsDTO.CreditCardAuthorization = transactionResponse.AuthorizationNo;
                                transactionPaymentsDTO.Reference = transactionResponse.ReferenceNo;
                                transactionPaymentsDTO.CCResponseId = transactionResponse.ccResponseId;
                                transactionPaymentsDTO.CreditCardName = transactionResponse.CardType;
                                transactionPaymentsDTO.Amount = (transactionResponse.TransactionAmount) / 100.00;
                                if (!string.IsNullOrEmpty(transactionResponse.ReceiptText))
                                    transactionPaymentsDTO.Memo = transactionResponse.ReceiptText;
                            }
                        }
                        else
                        {
                            throw new Exception(utilities.MessageUtils.getMessage(Message));
                        }
                    }
                    else
                    {
                        throw new Exception(utilities.MessageUtils.getMessage(Message));
                    }
                }
                else
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Nothing to pay"));
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to Refund the payment");
                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                throw new PaymentGatewayException(ex.Message);
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
    }
}
