//using Semnox.Parafait.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Semnox.Core.Utilities;
//using Semnox.Parafait.TransactionPayments;
using System.Net;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class ElementExpressPaymentGateway : PaymentGateway
    {
        ElementPSAdaper elementPS;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public ElementExpressPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate) : base(utilities, isUnattended, showMessageDelegate,writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            elementPS = new ElementPSAdaper(utilities);
            log.LogMethodExit(null);
        }

        ///<summary>
        ///Makes Payment.
        ///</summary>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            string Message = "";
            StandaloneRefundNotAllowed(transactionPaymentsDTO);
            elementPS.PrintReceipt = printReceipt;
            elementPS.IsDebitCard = !IsCreditCard;
            CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
            bool ret = elementPS.MakePayment(ccRequestPGWDTO.RequestID, transactionPaymentsDTO.Amount);

            if (ret)
            {
                Message = "APPROVED";
                transactionPaymentsDTO.CreditCardAuthorization = elementPS.ElementExpressResponse.Transaction.ApprovalNumber;
                transactionPaymentsDTO.Reference = elementPS.ElementExpressResponse.Transaction.TransactionID;
                transactionPaymentsDTO.CCResponseId = Convert.ToInt32(elementPS.CCResponseId);
                transactionPaymentsDTO.CreditCardNumber = elementPS.ElementExpressResponse.Card.CardNumber;
                transactionPaymentsDTO.CreditCardExpiry = elementPS.ElementExpressResponse.Card.ExpirationMonth + elementPS.ElementExpressResponse.Card.ExpirationYear;
                transactionPaymentsDTO.NameOnCreditCard = "";
                transactionPaymentsDTO.CreditCardName = elementPS.ElementExpressResponse.Card.CardLogo + (elementPS.IsDebitCard ? "_DEBIT" : "");
                utilities.EventLog.logEvent(PaymentGateways.ElementExpress.ToString(), 'I', "ELEMENTEXPRESS", "APPROVED", CREDIT_CARD_PAYMENT, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
            }
            else
            {
                log.Error("Unable to make Payment" + elementPS.ElementExpressResponse.ExpressResponseMessage);
                Message = elementPS.ElementExpressResponse.ExpressResponseMessage;
                utilities.EventLog.logEvent(PaymentGateways.ElementExpress.ToString(), 'I', "DECLINED", Message, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                throw new PaymentGatewayException(Message);
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        ///<summary>
        ///Reverts the Payment.
        ///</summary>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            string Message = "";
            try
            {
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
                bool ret = elementPS.ReverseOrVoid(transactionPaymentsDTO.Amount, transactionPaymentsDTO.CCResponseId);

                if (ret)
                {
                    transactionPaymentsDTO.CCResponseId = Convert.ToInt32(elementPS.CCResponseId);
                    transactionPaymentsDTO.CreditCardAuthorization = elementPS.ElementExpressResponse.Transaction.ApprovalNumber;
                    transactionPaymentsDTO.Reference = elementPS.ElementExpressResponse.Transaction.TransactionID;
                    transactionPaymentsDTO.CreditCardNumber = "";
                    if (string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardName))
                        transactionPaymentsDTO.CreditCardName = elementPS.ElementExpressResponse.Card.CardLogo;
                }
                else
                {
                    Message = elementPS.ElementExpressResponse.ExpressResponseMessage;
                    utilities.EventLog.logEvent(PaymentGateways.ElementExpress.ToString(), 'I', elementPS.ElementExpressResponse.ExpressResponseMessage, Message, CREDIT_CARD_REFUND, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    throw new PaymentGatewayException(Message);
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to refund payment." + ex.Message);
                utilities.EventLog.logEvent(PaymentGateways.ElementExpress.ToString(), 'D', ex.Message, ex.Message, CREDIT_CARD_REFUND, 3);
                System.Windows.Forms.MessageBox.Show(ex.Message);
                throw new PaymentGatewayException(ex.Message);
            }

            if (!string.IsNullOrEmpty(Message))
            {
                log.Error(Message);
                log.LogMethodExit(null, "Throwing payment gateway exception-" + Message);
                throw new PaymentGatewayException(Message);
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
    }
}
