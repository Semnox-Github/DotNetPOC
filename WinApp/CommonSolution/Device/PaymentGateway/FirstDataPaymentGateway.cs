using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
//using Semnox.Parafait.PaymentGateway;
//using Semnox.Parafait.TransactionPayments;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// FirstData Payment Gateway class
    /// </summary>
    public class FirstDataPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private FirstDataAdapter firstDataAdapter;

        private const string TRANSACTION_TYPE_AUTHORIZATION = "Authorization";
        private const string TRANSACTION_TYPE_PRE_AUTHORIZATION = "TATokenRequest";
        private const string TRANSACTION_TYPE_COMPLETION = "Completion";

        /// <summary>
        /// Constructor of FirstDataPaymentGatewayImplementation class.
        /// </summary>
        /// <param name="utilities">Parafait utilities.</param>
        /// <param name="isUnattended">Whether the payment process is supervised by an attendant.</param>
        /// <param name="showMessageDelegate">Delegate that is invoked to display Message</param>
        /// <param name="writeToLogDelegate"></param>
        public FirstDataPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate) : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);

            firstDataAdapter = new FirstDataAdapter(utilities, showMessageDelegate);
            FirstDataAdapter.IsUnattended = isUnattended;

            log.LogMethodExit(null);
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
            if (utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y"))
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.CCResponseId != -1)
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, TRANSACTION_TYPE_AUTHORIZATION));
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

        private CCTransactionsPGWDTO GetPreAuthorizationCCTransactionsPGWDTO(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            CCTransactionsPGWDTO preAuthorizationCCTransactionsPGWDTO = null;
            if (utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y"))
            {
                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRANSACTION_ID, transactionPaymentsDTO.TransactionId.ToString()));
                if (transactionPaymentsDTO.SplitId != -1)
                {
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SPLIT_ID, transactionPaymentsDTO.SplitId.ToString()));
                }
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, TRANSACTION_TYPE_PRE_AUTHORIZATION));
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                {
                    preAuthorizationCCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                }
            }

            log.LogMethodExit(preAuthorizationCCTransactionsPGWDTO);
            return preAuthorizationCCTransactionsPGWDTO;
        }

        private FirstDataTransactionRequest BuildFirstDataTransactionRequest(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, cCRequestPGWDTO, cCTransactionsPGWDTO);

            FirstDataTransactionRequest firstDataTransactionRequest = new FirstDataTransactionRequest();
            firstDataTransactionRequest.ReferenceNo = transactionPaymentsDTO.TransactionId.ToString().PadLeft(12, '0');
            firstDataTransactionRequest.TransactionAmount = transactionPaymentsDTO.Amount;
            firstDataTransactionRequest.TipAmount = transactionPaymentsDTO.TipAmount;
            firstDataTransactionRequest.STAN = cCRequestPGWDTO.RequestID.ToString().PadLeft(6, '0');
            firstDataTransactionRequest.LocalDateTime = ServerDateTime.Now.ToString("yyyyMMddHHmmss");
            if(cCTransactionsPGWDTO != null)
            {
                firstDataTransactionRequest.OrigSTAN = cCTransactionsPGWDTO.ProcessData;
                firstDataTransactionRequest.OrigRef = cCTransactionsPGWDTO.InvoiceNo;
                firstDataTransactionRequest.OrigResponseCode = cCTransactionsPGWDTO.DSIXReturnCode;
                firstDataTransactionRequest.OrigAuthID = cCTransactionsPGWDTO.AuthCode;
                firstDataTransactionRequest.OrigToken = cCTransactionsPGWDTO.TokenID;
                firstDataTransactionRequest.OrigCardName = cCTransactionsPGWDTO.UserTraceData;
                firstDataTransactionRequest.OrigGroupData = cCTransactionsPGWDTO.AcqRefData;

                firstDataTransactionRequest.OrigCardExpiryDate = cCTransactionsPGWDTO.ResponseOrigin;
                firstDataTransactionRequest.wasOrigSwiped = (string.IsNullOrEmpty(cCTransactionsPGWDTO.CaptureStatus)) ? false : cCTransactionsPGWDTO.CaptureStatus.Contains("1");
                firstDataTransactionRequest.OrigAccNo = cCTransactionsPGWDTO.AcctNo;
                firstDataTransactionRequest.OrigDatetime = cCTransactionsPGWDTO.TransactionDatetime.ToString("yyyyMMddHHmmss");
                firstDataTransactionRequest.OrigGMTDatetime = cCTransactionsPGWDTO.TransactionDatetime.ToUniversalTime().ToString("yyyyMMddHHmmss");
                firstDataTransactionRequest.OrigTransactionType = cCTransactionsPGWDTO.TranCode;
                firstDataTransactionRequest.OrigTransactionAmount = (string.IsNullOrEmpty(cCTransactionsPGWDTO.Authorize)) ? 0d : Convert.ToDouble(cCTransactionsPGWDTO.Authorize);
                firstDataTransactionRequest.isCredit = (string.IsNullOrEmpty(cCTransactionsPGWDTO.CardType)) ? false : cCTransactionsPGWDTO.CardType.Equals("Credit");
            }

            log.LogMethodExit(firstDataTransactionRequest);
            return firstDataTransactionRequest;
        }

        /// <summary>
        /// Makes payment.
        /// </summary>
        /// <param name="transactionPaymentsDTO">transactionPaymentsDTO</param>
        /// <returns></returns>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            StandaloneRefundNotAllowed(transactionPaymentsDTO);
            if (transactionPaymentsDTO != null)
            {
                //if(transactionPaymentsDTO.Amount>0)
                //{ 
                firstDataAdapter.PrintReceipt = printReceipt;
                firstDataAdapter.isCredit = IsCreditCard;
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_PURCHASE);
                CCTransactionsPGWDTO cCTransactionsPGWDTO = GetPreAuthorizationCCTransactionsPGWDTO(transactionPaymentsDTO);
                FirstDataTransactionRequest firstDataTransactionRequest = BuildFirstDataTransactionRequest(transactionPaymentsDTO, cCRequestPGWDTO, cCTransactionsPGWDTO);
                firstDataTransactionRequest.TransactionType = "Sale";
                bool success = firstDataAdapter.MakePayment(firstDataTransactionRequest);
                if (success == true)
                {
                    transactionPaymentsDTO.CreditCardAuthorization = firstDataAdapter.FirstDataResp.responseAuthId;
                    transactionPaymentsDTO.Reference = firstDataAdapter.FirstDataResp.ReferenceNo;
                    transactionPaymentsDTO.CCResponseId = string.IsNullOrEmpty(firstDataAdapter.FirstDataResp.ccResponseId) ? -1 : Convert.ToInt32(firstDataAdapter.FirstDataResp.ccResponseId);
                    if (firstDataTransactionRequest.isCredit)
                        transactionPaymentsDTO.CreditCardName = firstDataAdapter.FirstDataResp.CardName;
                    else
                        transactionPaymentsDTO.CreditCardName = firstDataAdapter.FirstDataResp.CardName + "_DEBIT";//Modification 01-oct-2015 Ends
                    transactionPaymentsDTO.Memo = firstDataAdapter.FirstDataResp.ReceiptText;
                    transactionPaymentsDTO.Amount = firstDataAdapter.FirstDataResp.TransAmount;
                    utilities.EventLog.logEvent(PaymentGateways.FirstData.ToString(), 'I', "FirstData", "APPROVED", CREDIT_CARD_PAYMENT, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    //}
                    //else
                    //{
                    //    log.Debug("Variable refund");
                    //    transactionPaymentsDTO.Amount = transactionPaymentsDTO.Amount * -1;
                    //    TransactionPaymentsDTO transactionPaymentDTO = RefundAmount(transactionPaymentsDTO);
                    //    transactionPaymentDTO.Amount = transactionPaymentDTO.Amount * -1;
                    //    return transactionPaymentDTO;
                    //}
                }
                else
                {
                    utilities.EventLog.logEvent(PaymentGateways.FirstData.ToString(), 'I', "DECLINED", firstDataAdapter.FirstDataResp.responseMessage, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                    log.LogMethodExit(null, "Throwing PaymentGatewayException - " + firstDataAdapter.FirstDataResp.responseMessage);
                    throw new PaymentGatewayException(firstDataAdapter.FirstDataResp.responseMessage);
                }
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        /// <summary>
        /// Reverts the payment.
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            firstDataAdapter.PrintReceipt = printReceipt;
            TransactionPaymentsDTO revertTransactionPaymentsDTO = null;
            if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount > 0)
            {
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_PURCHASE);
                FirstDataTransactionRequest firstDataTransactionRequest = BuildFirstDataTransactionRequest(transactionPaymentsDTO, cCRequestPGWDTO, null);
                // While reversing the transaction line the some values used to come like 10.3333333 and first
                //data payment gateway used to give error. this used to remove the extra decimal places.
                int temp = Convert.ToInt32((transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount) * 100);
                firstDataTransactionRequest.TransactionAmount = (double)temp / 100;
                bool success = firstDataAdapter.ReverseOrVoid(firstDataTransactionRequest, transactionPaymentsDTO.CCResponseId);
                if (success == true)
                {
                    revertTransactionPaymentsDTO = new TransactionPaymentsDTO();
                    revertTransactionPaymentsDTO.CreditCardAuthorization = firstDataAdapter.FirstDataResp.responseAuthId;
                    revertTransactionPaymentsDTO.Reference = firstDataAdapter.FirstDataResp.ReferenceNo;
                    revertTransactionPaymentsDTO.CCResponseId = string.IsNullOrEmpty(firstDataAdapter.FirstDataResp.ccResponseId) ? -1 : Convert.ToInt32(firstDataAdapter.FirstDataResp.ccResponseId);
                    if (firstDataTransactionRequest.isCredit)
                        revertTransactionPaymentsDTO.CreditCardName = firstDataAdapter.FirstDataResp.CardName;
                    else
                        revertTransactionPaymentsDTO.CreditCardName = firstDataAdapter.FirstDataResp.CardName + "_DEBIT";//Modification 01-oct-2015 Ends
                    revertTransactionPaymentsDTO.Memo = firstDataAdapter.FirstDataResp.ReceiptText;
                    double totalAmount = firstDataAdapter.FirstDataResp.TransAmount;
                    if (totalAmount >= transactionPaymentsDTO.Amount)
                    {
                        revertTransactionPaymentsDTO.Amount = -1 * transactionPaymentsDTO.Amount;
                        totalAmount = totalAmount - transactionPaymentsDTO.Amount;
                    }
                    else
                    {
                        revertTransactionPaymentsDTO.Amount = -1 * totalAmount;
                        totalAmount = 0;
                    }
                    if (totalAmount >= transactionPaymentsDTO.TipAmount)
                    {
                        revertTransactionPaymentsDTO.TipAmount = -1 * transactionPaymentsDTO.TipAmount;
                    }
                    else
                    {
                        revertTransactionPaymentsDTO.TipAmount = -1 * totalAmount;
                    }
                    utilities.EventLog.logEvent(PaymentGateways.FirstData.ToString(), 'I', "FirstData", "APPROVED", CREDIT_CARD_REFUND, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                }
                else
                {
                    utilities.EventLog.logEvent(PaymentGateways.FirstData.ToString(), 'I', "DECLINED", firstDataAdapter.FirstDataResp.responseMessage, CREDIT_CARD_REFUND, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                    log.LogMethodExit(null, "Throwing PaymentGatewayException - " + firstDataAdapter.FirstDataResp.responseMessage);
                    throw new PaymentGatewayException(firstDataAdapter.FirstDataResp.responseMessage);
                }

            }

            log.LogMethodExit(revertTransactionPaymentsDTO);
            return revertTransactionPaymentsDTO;
        }


        /// <summary>
        /// Performs settlement.
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="IsForcedSettlement"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO PerformSettlement(TransactionPaymentsDTO transactionPaymentsDTO, bool IsForcedSettlement = false)//2017-09-27
        {
            log.LogMethodEntry(transactionPaymentsDTO, IsForcedSettlement);

            TransactionPaymentsDTO returnTransactionPaymentsDTO = null;
            if (transactionPaymentsDTO != null)
            {
                if (!IsForcedSettlement)//2017-09-27
                {
                    Semnox.Parafait.Device.PaymentGateway.frmTransactionTypeUI frmTranType = new Semnox.Parafait.Device.PaymentGateway.frmTransactionTypeUI(utilities, (TRANSACTION_TYPE_COMPLETION), transactionPaymentsDTO.Amount, showMessageDelegate);
                    if (frmTranType.ShowDialog() != DialogResult.Cancel)
                    {
                        transactionPaymentsDTO.TipAmount = frmTranType.TipAmount;
                    }
                    else
                    {
                        log.LogMethodExit(returnTransactionPaymentsDTO);
                        //return returnTransactionPaymentsDTO;
                        throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                    }
                }//2017-09-27
                CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO cCTransactionsPGWDTO = cCTransactionsPGWBL.CCTransactionsPGWDTO;
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_PURCHASE);
                FirstDataTransactionRequest firstDataTransactionRequest = BuildFirstDataTransactionRequest(transactionPaymentsDTO, cCRequestPGWDTO, cCTransactionsPGWDTO);
                firstDataTransactionRequest.TransactionType = TRANSACTION_TYPE_COMPLETION;

                bool success = firstDataAdapter.MakePayment(firstDataTransactionRequest);
                if (success == true)
                {
                    transactionPaymentsDTO.CreditCardAuthorization = firstDataAdapter.FirstDataResp.responseAuthId;
                    transactionPaymentsDTO.Reference = firstDataAdapter.FirstDataResp.ReferenceNo;
                    transactionPaymentsDTO.CCResponseId = string.IsNullOrEmpty(firstDataAdapter.FirstDataResp.ccResponseId) ? -1 : Convert.ToInt32(firstDataAdapter.FirstDataResp.ccResponseId);
                    if (firstDataTransactionRequest.isCredit)
                        transactionPaymentsDTO.CreditCardName = firstDataAdapter.FirstDataResp.CardName;
                    else
                        transactionPaymentsDTO.CreditCardName = firstDataAdapter.FirstDataResp.CardName + "_DEBIT";
                    transactionPaymentsDTO.Memo = firstDataAdapter.FirstDataResp.ReceiptText;
                    transactionPaymentsDTO.Amount = firstDataAdapter.FirstDataResp.TransAmount - transactionPaymentsDTO.TipAmount;
                    utilities.EventLog.logEvent(PaymentGateways.FirstData.ToString(), 'I', "FirstData", "APPROVED", CREDIT_CARD_PAYMENT, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    returnTransactionPaymentsDTO = transactionPaymentsDTO;
                }
                else
                {
                    utilities.EventLog.logEvent(PaymentGateways.FirstData.ToString(), 'I', "DECLINED", firstDataAdapter.FirstDataResp.responseMessage, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                    log.LogMethodExit(null, "Throwing PaymentGatewayException - firstDataAdapter.FirstDataResp.responseMessage");
                    throw new PaymentGatewayException(firstDataAdapter.FirstDataResp.responseMessage);
                }
            }

            log.LogMethodExit(returnTransactionPaymentsDTO);
            return returnTransactionPaymentsDTO;
        }

        /// <summary>
        /// Returns list of CCTransactionsPGWDTO's  pending for settelement. 
        /// </summary>
        /// <returns></returns>
        public override List<CCTransactionsPGWDTO> GetAllUnsettledCreditCardTransactions()
        {
            log.LogMethodEntry();

            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = null;
            CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, TRANSACTION_TYPE_AUTHORIZATION));
            cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);

            log.LogMethodExit(cCTransactionsPGWDTOList);
            return cCTransactionsPGWDTOList;
        }

        /// <summary>
        /// Settle Transaction Payment
        /// </summary>
        public override TransactionPaymentsDTO SettleTransactionPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            TransactionPaymentsDTO returnTransactionPaymentsDTO = null;
            if (transactionPaymentsDTO != null)
            {
                CanAdjustTransactionPayment(transactionPaymentsDTO);
                CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO cCTransactionsPGWDTO = cCTransactionsPGWBL.CCTransactionsPGWDTO;
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_PURCHASE);
                FirstDataTransactionRequest firstDataTransactionRequest = BuildFirstDataTransactionRequest(transactionPaymentsDTO, cCRequestPGWDTO, cCTransactionsPGWDTO);
                firstDataTransactionRequest.TransactionType = TRANSACTION_TYPE_COMPLETION;

                bool success = firstDataAdapter.MakePayment(firstDataTransactionRequest);
                if (success == true)
                {
                    transactionPaymentsDTO.CreditCardAuthorization = firstDataAdapter.FirstDataResp.responseAuthId;
                    transactionPaymentsDTO.Reference = firstDataAdapter.FirstDataResp.ReferenceNo;
                    transactionPaymentsDTO.CCResponseId = string.IsNullOrEmpty(firstDataAdapter.FirstDataResp.ccResponseId) ? -1 : Convert.ToInt32(firstDataAdapter.FirstDataResp.ccResponseId);
                    if (firstDataTransactionRequest.isCredit)
                        transactionPaymentsDTO.CreditCardName = firstDataAdapter.FirstDataResp.CardName;
                    else
                        transactionPaymentsDTO.CreditCardName = firstDataAdapter.FirstDataResp.CardName + "_DEBIT";
                    transactionPaymentsDTO.Memo = firstDataAdapter.FirstDataResp.ReceiptText;
                    transactionPaymentsDTO.Amount = firstDataAdapter.FirstDataResp.TransAmount - transactionPaymentsDTO.TipAmount;
                    utilities.EventLog.logEvent(PaymentGateways.FirstData.ToString(), 'I', "FirstData", "APPROVED", CREDIT_CARD_PAYMENT, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    returnTransactionPaymentsDTO = transactionPaymentsDTO;
                }
                else
                {
                    utilities.EventLog.logEvent(PaymentGateways.FirstData.ToString(), 'I', "DECLINED", firstDataAdapter.FirstDataResp.responseMessage, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    log.LogMethodExit(null, "Throwing PaymentGatewayException - firstDataAdapter.FirstDataResp.responseMessage");
                    throw new PaymentGatewayException(firstDataAdapter.FirstDataResp.responseMessage);
                }
            }
            log.LogMethodExit(returnTransactionPaymentsDTO);
            return returnTransactionPaymentsDTO;
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
