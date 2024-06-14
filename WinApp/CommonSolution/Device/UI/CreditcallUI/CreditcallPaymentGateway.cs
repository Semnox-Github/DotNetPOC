//using Semnox.Parafait.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Semnox.Parafait.TransactionPayments;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway.CreditcallUI
{
    class CreditcallPaymentGateway : PaymentGateway
    {
        CreditcallGateway creditcallGateway;
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
            creditcallGateway = new CreditcallGateway();
            CreditcallGateway.unAttendedCC = isUnattended;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Makes payment.
        /// </summary>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            bool ret = creditcallGateway.doTransaction(utilities, (transactionPaymentsDTO.Amount * 100).ToString(), transactionPaymentsDTO.TransactionId, "sale", "BR_" + transactionPaymentsDTO.TransactionId.ToString());
            if (ret)
            {
                transactionPaymentsDTO.CreditCardAuthorization = CreditcallUI.CreditcallGateway.authCode;
                transactionPaymentsDTO.Reference = CreditcallUI.CreditcallGateway.referenceNo;
                transactionPaymentsDTO.Memo = CreditcallUI.CreditcallGateway.receiptText;
                utilities.EventLog.logEvent(PaymentGateways.CreditCall.ToString(), 'I', CreditcallUI.CreditcallGateway.trxStatus, "", CREDIT_CARD_PAYMENT, 1, "CreditcallReference, Authorization", transactionPaymentsDTO.Reference + ", " + transactionPaymentsDTO.CreditCardAuthorization, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
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
            transactionPaymentsDTO.Reference = transactionPaymentsDTO.TransactionId.ToString();
            bool ret = false;
            if (transactionPaymentsDTO.Reference == null)
            {
                //-the Utilities object was added as a parameter in the dotransaction() function on -02062015
                ret = creditcallGateway.doTransaction(utilities, (transactionPaymentsDTO.Amount * 100).ToString(), transactionPaymentsDTO.TransactionId, "refund", "BR_" + transactionPaymentsDTO.TransactionId.ToString());
            }
            else
            {
                //-the Utilities object was added as a parameter in the dotransaction() function on -02062015
                ret = creditcallGateway.doTransaction(utilities, (transactionPaymentsDTO.Amount * 100).ToString(), transactionPaymentsDTO.TransactionId, "linkedRefund", "BR_" + transactionPaymentsDTO.TransactionId.ToString(), transactionPaymentsDTO.Reference);
            }

            isUnattended = false;
            CreditcallGateway.unAttendedCC = isUnattended;
            if (ret)
            {

                transactionPaymentsDTO.CreditCardAuthorization = CreditcallUI.CreditcallGateway.authCode;
                transactionPaymentsDTO.Reference = CreditcallUI.CreditcallGateway.referenceNo;
                transactionPaymentsDTO.Memo = CreditcallUI.CreditcallGateway.receiptText;
                utilities.EventLog.logEvent(PaymentGateways.CreditCall.ToString(), 'I', CreditcallUI.CreditcallGateway.trxStatus, "", CREDIT_CARD_PAYMENT, 1, "CreditcallReference, Authorization", transactionPaymentsDTO.Reference + ", " + transactionPaymentsDTO.CreditCardAuthorization, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

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
