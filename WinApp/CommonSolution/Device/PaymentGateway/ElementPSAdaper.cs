using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RBA_SDK;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.ElementExpress;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// ElementPSAdaper class
    /// </summary>
    public class ElementPSAdaper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// ElementExpressResponse
        /// </summary>
        public Response ElementExpressResponse;
        /// <summary>
        /// object CCResponseId
        /// </summary>
        public object CCResponseId;
        /// <summary>
        /// bool IsDebitCard
        /// </summary>
        public bool IsDebitCard = false;
        /// <summary>
        /// bool PrintReceipt
        /// </summary>
        public bool PrintReceipt = true;

        IngenicoIUP250 IUP250PinPad = null;
        Utilities _utilities;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inUtilities"></param>
        public ElementPSAdaper(Utilities inUtilities)
        {
            log.LogMethodEntry(inUtilities);
            _utilities = inUtilities;
            log.LogMethodExit(null);
        }

        void pinPadMessageHandler(RBA_SDK.MESSAGE_ID msgId, PinPadResponseAttributes PinpadResponse)
        {
            log.LogMethodEntry(msgId, PinpadResponse);
            log.LogMethodExit(null);

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ElementPSAdaper()
        {
            log.LogMethodEntry();
            if (IUP250PinPad != null)
            {
                IUP250PinPad.MessageOffline();
                IUP250PinPad.setReceiveHandler = null;
                IUP250PinPad.Close();
                IUP250PinPad.Shutdown();
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Makes Payment
        /// </summary>
        /// <param name="TrxId"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public bool MakePayment(int TrxId, double Amount)
        {
            log.LogMethodEntry(TrxId, Amount);
            if (IUP250PinPad == null)
            {
                IUP250PinPad = new IngenicoIUP250();
                IUP250PinPad.setReceiveHandler = pinPadMessageHandler;

                int tries = 20;
                bool success = false;
                int _portNo;
                try
                {
                    _portNo = Convert.ToInt32(_utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO"));
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while reading CREDIT_CARD_TERMINAL_PORT_NO ", ex);
                    IUP250PinPad = null;
                    log.LogMethodExit(null, "Throwing Application exception-Invalid CREDIT_CARD_TERMINAL_PORT_NO ");
                    throw new ApplicationException("Invalid CREDIT_CARD_TERMINAL_PORT_NO");
                }

                if (System.IO.Ports.SerialPort.GetPortNames().Contains("COM" + _portNo.ToString()))
                {
                    while (tries-- > 0)
                    {
                        try
                        {
                            IUP250PinPad.Connect(_portNo);
                            success = true;
                            System.Threading.Thread.Sleep(500); // give a small delay after connect
                            break;
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while connecting to pin pad", ex);
                            System.Threading.Thread.Sleep(500);
                        }
                    }
                }

                if (!success)
                {
                    IUP250PinPad = null;
                    log.LogMethodExit(null, "Throwing application exception-Unable to connect to PinPad");
                    throw new ApplicationException("Unable to connect to PinPad");
                }
            }

            ElementTransactionRequest trxRequest = new ElementTransactionRequest();
            trxRequest.TransactionType = "PAYMENT";
            trxRequest.TransactionAmount = Amount;
            trxRequest.TransactionId = TrxId;
            trxRequest.DebitCardSale = IsDebitCard;
            trxRequest.PrintReceipt = PrintReceipt;

            ElementPSCore ui = new ElementPSCore(IUP250PinPad, trxRequest, _utilities);
            DialogResult dr = ui.ShowDialog();

            ElementExpressResponse = ui.ElementExpressResponse;
            CCResponseId = ui.CCResponseId;
            bool returnvalue = (dr == DialogResult.OK);
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        /// <summary>
        /// Reverts Payment
        /// </summary>
        /// <param name="OrigCCResponseId"></param>
        /// <returns></returns>
        public bool VoidSale(object OrigCCResponseId)
        {
            log.LogMethodEntry(OrigCCResponseId);
            if (OrigCCResponseId == null || OrigCCResponseId == DBNull.Value)
            {
                log.LogMethodExit(true);
                return true;
            }


            ElementTransactionRequest trxRequest = new ElementTransactionRequest();
            trxRequest.TransactionType = "VOID";
            getDetailsForReturnVoidSale(OrigCCResponseId, trxRequest);

            ElementPSCore ui = new ElementPSCore(IUP250PinPad, trxRequest, _utilities);
            DialogResult dr = ui.ShowDialog();

            ElementExpressResponse = ui.ElementExpressResponse;
            CCResponseId = ui.CCResponseId;
            bool returnvalue = (dr == DialogResult.OK);
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        /// <summary>
        /// Return Sale Method
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="OrigCCResponseId"></param>
        /// <returns></returns>
        public bool ReturnSale(double Amount, object OrigCCResponseId)
        {
            log.LogMethodEntry(Amount, OrigCCResponseId);
            if (OrigCCResponseId == null || OrigCCResponseId == DBNull.Value)
            {
                log.LogMethodExit(true);
                return true;
            }
            ElementTransactionRequest trxRequest = new ElementTransactionRequest();
            trxRequest.TransactionType = "RETURN";
            trxRequest.TransactionAmount = Amount;
            getDetailsForReturnVoidSale(OrigCCResponseId, trxRequest);

            ElementPSCore ui = new ElementPSCore(IUP250PinPad, trxRequest, _utilities);
            DialogResult dr = ui.ShowDialog();

            ElementExpressResponse = ui.ElementExpressResponse;
            CCResponseId = ui.CCResponseId;
            bool returnvalue = (dr == DialogResult.OK);
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        /// <summary>
        /// Partial Reversal
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="OrigCCResponseId"></param>
        /// <returns></returns>
        public bool PartialReversal(double Amount, object OrigCCResponseId)
        {
            log.LogMethodEntry(Amount, OrigCCResponseId);
            if (OrigCCResponseId == null || OrigCCResponseId == DBNull.Value)
            {
                log.LogMethodExit(true);
                return true;
            }


            ElementTransactionRequest trxRequest = new ElementTransactionRequest();
            trxRequest.TransactionType = "PARTIAL REVERSAL";
            trxRequest.TransactionAmount = Amount;
            getDetailsForReturnVoidSale(OrigCCResponseId, trxRequest);

            ElementPSCore ui = new ElementPSCore(IUP250PinPad, trxRequest, _utilities);
            DialogResult dr = ui.ShowDialog();

            ElementExpressResponse = ui.ElementExpressResponse;
            CCResponseId = ui.CCResponseId;
            bool returnvalue = (dr == DialogResult.OK);
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        /// <summary>
        /// Full Reversal
        /// </summary>
        /// <param name="OrigCCResponseId"></param>
        /// <returns></returns>
        public bool FullReversal(object OrigCCResponseId)
        {
            log.LogMethodEntry(OrigCCResponseId);
            if (OrigCCResponseId == null || OrigCCResponseId == DBNull.Value)
            {
                log.LogMethodExit(true);
                return true;
            }


            ElementTransactionRequest trxRequest = new ElementTransactionRequest();
            trxRequest.TransactionType = "FULL REVERSAL";
            getDetailsForReturnVoidSale(OrigCCResponseId, trxRequest);

            ElementPSCore ui = new ElementPSCore(IUP250PinPad, trxRequest, _utilities);
            DialogResult dr = ui.ShowDialog();

            ElementExpressResponse = ui.ElementExpressResponse;
            CCResponseId = ui.CCResponseId;
            bool returnvalue = (dr == DialogResult.OK);
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        /// <summary>
        /// Health Check
        /// </summary>
        /// <returns></returns>
        public bool HealthCheck()
        {
            log.LogMethodEntry();
            ElementTransactionRequest trxRequest = new ElementTransactionRequest();
            trxRequest.TransactionType = "HEALTH CHECK";

            ElementPSCore ui = new ElementPSCore(IUP250PinPad, trxRequest, _utilities);
            DialogResult dr = ui.ShowDialog();

            ElementExpressResponse = ui.ElementExpressResponse;
            bool returnvalue = (dr == DialogResult.OK);
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        /// <summary>
        /// Reversal or Void Payment
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="OrigCCResponseId"></param>
        /// <returns></returns>
        public bool ReverseOrVoid(double Amount, object OrigCCResponseId)
        {
            log.LogMethodEntry(Amount, OrigCCResponseId);
            if (OrigCCResponseId == null || OrigCCResponseId == DBNull.Value)
            {
                log.LogMethodExit(true);
                return true;
            }


            ElementTransactionRequest trxRequest = new ElementTransactionRequest();
            getDetailsForReturnVoidSale(OrigCCResponseId, trxRequest);
            if (Amount == trxRequest.OrigTransactionAmount)
                trxRequest.TransactionType = "FULL REVERSAL";
            else
                trxRequest.TransactionType = "PARTIAL REVERSAL";

            trxRequest.TransactionAmount = Amount;

            ElementPSCore ui = new ElementPSCore(IUP250PinPad, trxRequest, _utilities);
            DialogResult dr = ui.ShowDialog();
            bool returnTrx = false;
            if (ui.ElementExpressResponse.ExpressResponseCode != "0")
            {
                if (Amount == trxRequest.OrigTransactionAmount)
                {
                    trxRequest.TransactionType = "VOID";
                    ui = new ElementPSCore(IUP250PinPad, trxRequest, _utilities);
                    dr = ui.ShowDialog();
                    if (ui.ElementExpressResponse.ExpressResponseCode != "0")
                        returnTrx = true;
                }
                else
                    returnTrx = true;

                if (returnTrx)
                {
                    trxRequest.TransactionType = "RETURN";
                    ui = new ElementPSCore(IUP250PinPad, trxRequest, _utilities);
                    dr = ui.ShowDialog();
                }
            }

            ElementExpressResponse = ui.ElementExpressResponse;
            CCResponseId = ui.CCResponseId;
            bool returnvalue = (dr == DialogResult.OK);
            log.LogMethodExit(returnvalue);
            return (returnvalue);

        }

        void getDetailsForReturnVoidSale(object CCResponseId, ElementTransactionRequest etr)
        {
            log.LogMethodEntry(CCResponseId, etr);
            DataTable dt = _utilities.executeDataTable("exec SPGetVoidSaleReturnInvoiceDetails @ResponseId = @pResponseId",
                                                        new SqlParameter("@pResponseId", CCResponseId));
            log.LogVariableState("@pResponseId", CCResponseId);
            if (dt.Rows.Count == 0)
            {
                log.LogMethodExit(null, "Throwing Application exception-Unable to retrieve original transaction");
                throw new ApplicationException("Unable to retrieve original transaction");
            }

            if (etr.TransactionType != "VOID" && dt.Rows[0]["TranCode"].ToString().Equals("Return"))
            {
                log.LogMethodExit(null, "Throwing Application exception-" + "Cannot perform " + etr.TransactionType + " on a Sale Return");
                throw new ApplicationException("Cannot perform " + etr.TransactionType + " on a Sale Return");
            }


            etr.OrigElementTransactionId = dt.Rows[0]["RefNo"];
            etr.TransactionId = Convert.ToInt32(dt.Rows[0]["InvoiceNo"]);
            try
            {
                etr.OrigTransactionAmount = Convert.ToDouble(dt.Rows[0]["Authorize"]);
            }
            catch (Exception ex)
            {
                log.Error("Error occured in original Transaction Amount", ex);
            }
            etr.DebitCardSale = dt.Rows[0]["CardType"].ToString().Equals("DEBIT");
        }
    }
}
