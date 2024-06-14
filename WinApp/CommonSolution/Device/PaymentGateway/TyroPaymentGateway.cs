//using Semnox.Parafait.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Semnox.Core.Utilities;
//using Semnox.Parafait.TransactionPayments;
using System.Threading;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class TyroPaymentGateway : PaymentGateway
    {
        TyroEFTPOS tyroEFTPOS;

        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public TyroPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate) : base(utilities, isUnattended, showMessageDelegate,writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);

            tyroEFTPOS = new TyroEFTPOS(utilities);

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Makes Payment
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            StandaloneRefundNotAllowed(transactionPaymentsDTO);
            string Message = "";
            ThreadStart thr = delegate
            {
                tyroEFTPOS.performSale(transactionPaymentsDTO.Amount);
            };

            new Thread(thr).Start();

            bool timeOut = !tyroEFTPOS.mre.WaitOne(180000);

            if (timeOut)
                Message = utilities.MessageUtils.getMessage(345);

            if (tyroEFTPOS.Status == 0)
            {
                if (tyroEFTPOS.Result == "APPROVED")
                {
                    transactionPaymentsDTO.CreditCardAuthorization = tyroEFTPOS.AuthorizationCode;
                    transactionPaymentsDTO.Reference = tyroEFTPOS.Reference;
                    transactionPaymentsDTO.Memo = tyroEFTPOS.ReceiptText;
                    if (string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardName))
                        transactionPaymentsDTO.CreditCardName = tyroEFTPOS.CardType;
                    utilities.EventLog.logEvent(PaymentGateways.TYRO.ToString(), 'I', tyroEFTPOS.Result, tyroEFTPOS.ReceiptText, CREDIT_CARD_PAYMENT, 1, "EFTPOSReference, Authorization, ID", transactionPaymentsDTO.Reference + ", " + transactionPaymentsDTO.CreditCardAuthorization + ", " + tyroEFTPOS.ID, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    tyroEFTPOS.PrintReceipt(tyroEFTPOS.ReceiptText, tyroEFTPOS.signatureRequired, false);
                }
                else if (tyroEFTPOS.Result == "CANCELLED" || tyroEFTPOS.Result == "REVERSED")
                {
                    Message = utilities.MessageUtils.getMessage(492);
                    utilities.EventLog.logEvent(PaymentGateways.TYRO.ToString(), 'I', tyroEFTPOS.Result, tyroEFTPOS.Message, CREDIT_CARD_PAYMENT, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                }
                else
                {
                    Message = utilities.MessageUtils.getMessage(348, tyroEFTPOS.Result + ":" + tyroEFTPOS.Message);
                    utilities.EventLog.logEvent(PaymentGateways.TYRO.ToString(), 'I', tyroEFTPOS.Result, tyroEFTPOS.Message, CREDIT_CARD_PAYMENT, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                }
            }
            else
            {
                log.Error("Unable to Make Payment" + Message);
                Message = tyroEFTPOS.Message;
                utilities.EventLog.logEvent(PaymentGateways.TYRO.ToString(), 'I', tyroEFTPOS.Result, Message, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                throw new PaymentGatewayException(Message);
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        /// <summary>
        /// Reverts Payment
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            string Message = "";
            ThreadStart thr = delegate
            {
                tyroEFTPOS.performRefund(transactionPaymentsDTO.Amount);
            };

            new Thread(thr).Start();

            bool timeOut = !tyroEFTPOS.mre.WaitOne(180000);

            if (timeOut)
                Message = utilities.MessageUtils.getMessage(346);

            if (tyroEFTPOS.Status == 0)
            {
                if (tyroEFTPOS.Result == "APPROVED")
                {
                    transactionPaymentsDTO.CreditCardAuthorization = tyroEFTPOS.AuthorizationCode;
                    transactionPaymentsDTO.Reference = tyroEFTPOS.Reference;
                    transactionPaymentsDTO.Memo = tyroEFTPOS.ReceiptText;
                    if (string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardName))
                        transactionPaymentsDTO.CreditCardName = tyroEFTPOS.CardType;
                    utilities.EventLog.logEvent(PaymentGateways.TYRO.ToString(), 'I', tyroEFTPOS.Result, tyroEFTPOS.ReceiptText, CREDIT_CARD_REFUND, 1, "EFTPOSReference, Authorization, ID", transactionPaymentsDTO.Reference + ", " + transactionPaymentsDTO.CreditCardAuthorization + ", " + tyroEFTPOS.ID, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    tyroEFTPOS.PrintReceipt(tyroEFTPOS.ReceiptText, tyroEFTPOS.signatureRequired, false);
                }
                else if (tyroEFTPOS.Result == "CANCELLED" || tyroEFTPOS.Result == "REVERSED")
                {
                    Message = utilities.MessageUtils.getMessage(347);
                    utilities.EventLog.logEvent(PaymentGateways.TYRO.ToString(), 'I', tyroEFTPOS.Result, tyroEFTPOS.Message, CREDIT_CARD_REFUND, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                }
                else
                {
                    Message = utilities.MessageUtils.getMessage(348, tyroEFTPOS.Result + ":" + tyroEFTPOS.Message);
                    utilities.EventLog.logEvent(PaymentGateways.TYRO.ToString(), 'I', tyroEFTPOS.Result, tyroEFTPOS.Message, CREDIT_CARD_REFUND, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                }
            }
            else
            {
                log.Error("Unable to refund Amount" + Message);
                Message = tyroEFTPOS.Message;
                utilities.EventLog.logEvent(PaymentGateways.TYRO.ToString(), 'I', tyroEFTPOS.Result, Message, CREDIT_CARD_REFUND, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
            }
            if (!string.IsNullOrEmpty(Message))
            {
                log.Error(Message);
                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                throw new PaymentGatewayException(Message);
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
    }
}
