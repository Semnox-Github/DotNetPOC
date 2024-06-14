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
    class CreditcallPaymentGateway : PaymentGateway
    {
        CreditcallUI.CreditcallGateway creditcallGateway;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public CreditcallPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate) : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            creditcallGateway = new CreditcallUI.CreditcallGateway();
            CreditcallUI.CreditcallGateway.unAttendedCC = isUnattended;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Makes payment.
        /// </summary>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            if (transactionPaymentsDTO.Amount < 0)
            {
                log.Debug("Standalone refund triggered");
                transactionPaymentsDTO.Amount = transactionPaymentsDTO.Amount * -1;
                TransactionPaymentsDTO transactionPaymentDTO = RefundAmount(transactionPaymentsDTO);
                transactionPaymentDTO.Amount = transactionPaymentDTO.Amount * -1;
                return transactionPaymentDTO;
            }
            CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
            bool ret = creditcallGateway.doTransaction(utilities, (transactionPaymentsDTO.Amount * 100).ToString(), ccRequestPGWDTO.RequestID, "sale", "BR_" + ccRequestPGWDTO.RequestID.ToString());
            if (ret)
            {
                transactionPaymentsDTO.CreditCardAuthorization = CreditcallUI.CreditcallGateway.authCode;
                transactionPaymentsDTO.Reference = CreditcallUI.CreditcallGateway.referenceNo;
                transactionPaymentsDTO.Memo = CreditcallUI.CreditcallGateway.receiptText;
                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                ccTransactionsPGWDTO.AuthCode = CreditcallUI.CreditcallGateway.authCode;
                ccTransactionsPGWDTO.Authorize = (transactionPaymentsDTO.Amount * 100).ToString();
                ccTransactionsPGWDTO.InvoiceNo = transactionPaymentsDTO.TransactionId.ToString();
                ccTransactionsPGWDTO.TransactionDatetime = DateTime.Now;
                ccTransactionsPGWDTO.TokenID = ccRequestPGWDTO.RequestID.ToString();
                ccTransactionsPGWDTO.RecordNo = "A";
                ccTransactionsPGWDTO.RefNo = CreditcallUI.CreditcallGateway.referenceNo;
                ccTransactionsPGWDTO.CustomerCopy = CreditcallUI.CreditcallGateway.customerCopy;
                ccTransactionsPGWDTO.MerchantCopy = CreditcallUI.CreditcallGateway.merchantCopy;
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                ccTransactionsPGWBL.Save();
                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                utilities.EventLog.logEvent(PaymentGateways.CreditCall.ToString(), 'I', ((CreditcallUI.CreditcallGateway.trxStatus == null) ? CreditcallUI.CreditcallGateway.returnedMessage : CreditcallUI.CreditcallGateway.trxStatus), "", CREDIT_CARD_PAYMENT, 1, "CreditcallReference, Authorization", transactionPaymentsDTO.Reference + ", " + transactionPaymentsDTO.CreditCardAuthorization, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
            }
            else
            {
                log.Error("Unable to Make Payment. " + CreditcallUI.CreditcallGateway.returnedMessage);
                utilities.EventLog.logEvent(PaymentGateways.CreditCall.ToString(), 'I', CreditcallUI.CreditcallGateway.trxStatus == null ? "Failed" : CreditcallUI.CreditcallGateway.trxStatus, CreditcallUI.CreditcallGateway.returnedMessage, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                log.LogMethodExit(null, "Throwing payment gateway exception-" + CreditcallUI.CreditcallGateway.returnedMessage);
                throw new PaymentGatewayException(CreditcallUI.CreditcallGateway.returnedMessage);
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
            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
            CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
            transactionPaymentsDTO.Reference = (ccTransactionsPGWBL.CCTransactionsPGWDTO == null) ? transactionPaymentsDTO.TransactionId.ToString() : ccTransactionsPGWBL.CCTransactionsPGWDTO.TokenID;//transactionPaymentsDTO.TransactionId.ToString();
            bool ret = false;
            if (transactionPaymentsDTO.Reference == null)
            {
                //-the Utilities object was added as a parameter in the dotransaction() function on -02062015
                ret = creditcallGateway.doTransaction(utilities, (transactionPaymentsDTO.Amount * 100).ToString(), ccRequestPGWDTO.RequestID, "refund", "BR_" + ccRequestPGWDTO.RequestID.ToString());
            }
            else
            {
                //-the Utilities object was added as a parameter in the dotransaction() function on -02062015
                ret = creditcallGateway.doTransaction(utilities, (transactionPaymentsDTO.Amount * 100).ToString(), ((ccTransactionsPGWBL.CCTransactionsPGWDTO == null) ? transactionPaymentsDTO.TransactionId : ccRequestPGWDTO.RequestID), "linkedRefund", "BR_" + ((ccTransactionsPGWBL.CCTransactionsPGWDTO == null) ? transactionPaymentsDTO.TransactionId : ccRequestPGWDTO.RequestID).ToString(), transactionPaymentsDTO.Reference);
            }

            isUnattended = false;
            CreditcallUI.CreditcallGateway.unAttendedCC = isUnattended;
            if (ret)
            {

                transactionPaymentsDTO.CreditCardAuthorization = CreditcallUI.CreditcallGateway.authCode;
                transactionPaymentsDTO.Reference = CreditcallUI.CreditcallGateway.referenceNo;
                transactionPaymentsDTO.Memo = CreditcallUI.CreditcallGateway.receiptText;
                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                ccTransactionsPGWDTO.AuthCode = string.IsNullOrEmpty(CreditcallUI.CreditcallGateway.authCode) ? "RefundOld" : CreditcallUI.CreditcallGateway.authCode;
                ccTransactionsPGWDTO.Authorize = (transactionPaymentsDTO.Amount * 100).ToString();
                ccTransactionsPGWDTO.InvoiceNo = transactionPaymentsDTO.TransactionId.ToString();
                ccTransactionsPGWDTO.TokenID = ccRequestPGWDTO.RequestID.ToString();
                ccTransactionsPGWDTO.RecordNo = "A";
                ccTransactionsPGWDTO.TransactionDatetime = DateTime.Now;
                ccTransactionsPGWDTO.RefNo = string.IsNullOrEmpty(CreditcallUI.CreditcallGateway.referenceNo) ? "RefundOld" : CreditcallUI.CreditcallGateway.referenceNo;
                ccTransactionsPGWDTO.CustomerCopy = CreditcallUI.CreditcallGateway.customerCopy;
                ccTransactionsPGWDTO.MerchantCopy = CreditcallUI.CreditcallGateway.merchantCopy;
                CCTransactionsPGWBL ccsavedTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                ccsavedTransactionsPGWBL.Save();
                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                utilities.EventLog.logEvent(PaymentGateways.CreditCall.ToString(), 'I', ((CreditcallUI.CreditcallGateway.trxStatus == null) ? "Approved" : CreditcallUI.CreditcallGateway.trxStatus), "", CREDIT_CARD_PAYMENT, 1, "CreditcallReference, Authorization", transactionPaymentsDTO.Reference + ", " + transactionPaymentsDTO.CreditCardAuthorization, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

            }
            else
            {
                log.Error("Unable to Refund Payment. " + CreditcallUI.CreditcallGateway.returnedMessage);
                utilities.EventLog.logEvent(PaymentGateways.CreditCall.ToString(), 'I', CreditcallUI.CreditcallGateway.trxStatus == null ? "Failed" : CreditcallUI.CreditcallGateway.trxStatus, CreditcallUI.CreditcallGateway.returnedMessage, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                log.LogMethodExit(null, "Throwing Payment gateway exception" + CreditcallUI.CreditcallGateway.returnedMessage);
                throw new PaymentGatewayException(CreditcallUI.CreditcallGateway.returnedMessage);
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
    }
}
