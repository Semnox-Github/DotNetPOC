
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Semnox.Core.Utilities;
using Semnox.Parafait.logging;


using System.Windows.Forms;
//using Semnox.Parafait.Transaction;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class BoricaPaymentGateway : PaymentGateway
    {
        frmBoricaCore frmBoricaCore;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parametrized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public BoricaPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate) : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            this.showMessageDelegate = showMessageDelegate;
            log.LogMethodExit(null);
        }

        ///<summary>
        ///Makes Payment.
        ///</summary>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            this.frmBoricaCore = new frmBoricaCore(utilities, transactionPaymentsDTO.Amount);
            DialogResult dr;
            string Message = "";
            try
            {
                StandaloneRefundNotAllowed(transactionPaymentsDTO);
                if (this.frmBoricaCore != null)
                {
                    dr = this.frmBoricaCore.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        transactionPaymentsDTO.CreditCardAuthorization = this.frmBoricaCore.BoricaResp.AuthCode;
                        transactionPaymentsDTO.Reference = this.frmBoricaCore.BoricaResp.ReferenceNo;
                        transactionPaymentsDTO.CreditCardNumber = this.frmBoricaCore.BoricaResp.CardholderID.PadLeft(16, 'X');
                        transactionPaymentsDTO.Amount = this.frmBoricaCore.BoricaResp.TranAmount;
                        utilities.EventLog.logEvent(PaymentGateways.Borica.ToString(), 'I', "Borica", "APPROVED", CREDIT_CARD_PAYMENT, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    }
                    else
                    {
                        Message = this.frmBoricaCore.BoricaResp.ResponseMessage;
                        utilities.EventLog.logEvent(PaymentGateways.Borica.ToString(), 'I', "DECLINED", Message, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                        throw new PaymentGatewayException(Message);
                    }
                }
                else
                {
                    Message = this.frmBoricaCore.BoricaResp.ResponseMessage;
                    utilities.EventLog.logEvent(PaymentGateways.Borica.ToString(), 'E', "Error", Message, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    throw new PaymentGatewayException(Message);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured during processing payment", ex);
                utilities.EventLog.logEvent(PaymentGateways.Borica.ToString(), 'D', ex.Message, ex.Message, CREDIT_CARD_PAYMENT, 3);

                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + ex);
                throw new PaymentGatewayException(ex.Message);
            }

            if (!string.IsNullOrEmpty(Message))
            {
                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
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

            // Message = "Verify that refund is done through Bank terminal!...";
            //return true;
            if (showMessageDelegate != null)
            {
                showMessageDelegate("Verify that refund is done through Bank terminal!...", "Borica Payment Gateway", MessageBoxButtons.OK);
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
    }
}
