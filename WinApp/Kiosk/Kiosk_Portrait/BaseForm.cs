/********************************************************************************************
* Project Name - Parafait_Kiosk - BaseForm
* Description  - BaseForm 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for Online Transaction in Kiosk changes 
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Parafait_Kiosk
{
    public partial class BaseForm : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected KioskTransaction kioskTransaction;
        protected KioskAttractionDTO kioskAttractionDTO;
        protected static List<string> attractionForms = new List<string>() { "frmAttractionSummary", "frmAttractionTapCard", "frmCardSaleOption", "frmSelectSlot", "frmProcessingAttractions", "frmAttractionQty" };
        public BaseForm()
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.ShowInTaskbar = false;
            kioskTimer.Interval = 1000;
            kioskTimer.Enabled = true;
            kioskTimer.Tick += KioskTimer_Tick;
            ResetKioskTimer();

            this.FormClosed += delegate
            {
                kioskTimer.Stop();
            };
            log.LogMethodExit();
        }

        private int kioskSecondsRemaining;
        private int kioskTimerTick = 30;

        public void SetKioskTimerTickValue(int tickCount = 30)
        {
            log.LogMethodEntry(tickCount);
            kioskTimerTick = tickCount;
            log.LogMethodExit();
        }
        public int GetKioskTimerTickValue()
        {
            log.LogMethodEntry();
            log.LogMethodExit(kioskTimerTick);
            return kioskTimerTick;
        }
        public int GetKioskTimerSecondsValue()
        {
            log.LogMethodEntry();
            log.LogMethodExit(kioskSecondsRemaining);
            return kioskSecondsRemaining;
        }

        public void setKioskTimerSecondsValue(int secRemaining)
        {
            log.LogMethodEntry(secRemaining);
            kioskSecondsRemaining = secRemaining;
            log.LogMethodExit(kioskSecondsRemaining);
        }

        public void ResetKioskTimer()
        {
            log.LogMethodEntry();
            try
            {
                kioskSecondsRemaining = kioskTimerTick;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(kioskSecondsRemaining);
        }
        public void StopKioskTimer()
        {
            log.LogMethodEntry();
            kioskTimer.Stop();
            log.LogMethodExit();
        }
        public void StartKioskTimer()
        {
            log.LogMethodEntry();
            kioskTimer.Start();
            log.LogMethodExit();
        }
        public bool GetKioskTimer()
        {
            log.LogMethodEntry();
            log.LogMethodExit(kioskTimer.Enabled);
            return kioskTimer.Enabled;
        }
        public void KioskTimerSwitch(bool boolSignal)
        {
            log.LogMethodEntry(boolSignal);
            kioskTimer.Enabled = boolSignal;
            log.LogMethodExit(kioskTimer.Enabled);
        }
        public void KioskTimerInterval(int timeDuration)
        {
            log.LogMethodEntry(timeDuration);
            kioskTimer.Interval = timeDuration;
            log.LogMethodExit(kioskTimer.Interval);
        }
        public virtual void KioskTimer_Tick(object sender, EventArgs e)
        {
            //log.LogMethodEntry(sender, e);
            if (this == ActiveForm)
            {
                if (kioskSecondsRemaining <= 10)
                {
                    if (TimeOut.AbortTimeOut(this))
                    {
                        ResetKioskTimer();
                    }
                    else
                    {
                        PerformAbortAction(kioskTransaction, kioskAttractionDTO);
                        CloseForms();
                        this.DialogResult = DialogResult.Cancel;
                    }
                }
                kioskSecondsRemaining--;
            }
            else
            {
                ResetKioskTimer();
            }
            // log.LogMethodExit();
        }

        public static void PerformTimeoutAbortAction(KioskTransaction kioskTransaction, KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry("kioskTransaction", kioskAttractionDTO);
            PerformAbortAction(kioskTransaction, kioskAttractionDTO);
            int lowerLimit = 0;
            for (int i = Application.OpenForms.Count - 1; i > lowerLimit; i--)
            {
                Application.OpenForms[i].Close();
            }
            log.LogMethodExit();
        }

        public virtual void Form_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            log.LogMethodExit();
        }

        public virtual void Form_Activated(object sender, EventArgs e)//Playpas1:starts
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            StartKioskTimer();
            log.LogMethodExit();
        }

        protected override CreateParams CreateParams
        {
            //this method is used to avoid the table layout flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        internal bool BillCollectorIsInWIPMode(BillAcceptor billAcceptor)
        {
            log.LogMethodEntry(billAcceptor);
            bool isInWIPMode = false;
            if (billAcceptor != null && billAcceptor.StillProcessing())
            {
                KioskStatic.logToFile("isInWIPMode: Still processing the bill");
                log.LogVariableState("Still processing the bill", billAcceptor);
                isInWIPMode = true;
            }
            KioskStatic.logToFile("isInWIPMode : " + isInWIPMode.ToString());
            log.LogMethodExit(isInWIPMode);
            return isInWIPMode;
        }
        public static void DirectCashAbortAction(KioskTransaction kioskTransaction, KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry("kioskTransaction", kioskAttractionDTO);
            if (KioskStatic.Utilities != null && KioskStatic.Utilities.ExecutionContext != null && kioskTransaction != null)
            {
                string trxStatus = kioskTransaction.GetTransactionStatus;
                decimal trxAmount = kioskTransaction.GetTotalPaymentsReceived();

                if ((trxStatus != Semnox.Parafait.Transaction.Transaction.TrxStatus.CLOSED.ToString()
                    || trxStatus != Semnox.Parafait.Transaction.Transaction.TrxStatus.CANCELLED.ToString()
                    || trxStatus != Semnox.Parafait.Transaction.Transaction.TrxStatus.SYSTEMABANDONED.ToString()
                    ) && (kioskTransaction.HasActiveTransactionLines() || trxAmount > 0)
                    && kioskTransaction.IsOnlineTransaction == false)
                {
                    string currencySymbol = ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL", "Rs N2");
                    string messageOne = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4197, trxAmount.ToString(currencySymbol));
                    // "You have inserted &1. Do you want to proceed with Abort?"
                    string messageTwo = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5009);
                    // "Do you want to abort current transaction?"
                    string finalMsg = (trxAmount > 0 ? messageOne : messageTwo);
                    using (frmYesNo f = new frmYesNo(finalMsg))
                    {
                        if (f.ShowDialog() == DialogResult.Yes)
                        {
                            PerformAbortAction(kioskTransaction, kioskAttractionDTO);
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4198));
                            //"Sorry cannot go back without aborting current transaction."
                        }
                    }
                }
                else
                {
                    CheckForBlockedSchedules(kioskAttractionDTO);
                    PerformAbortAction(kioskTransaction, kioskAttractionDTO);//clear the transaction
                }
            }
            log.LogMethodExit();
        }

        private static void CheckForBlockedSchedules(KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry(kioskAttractionDTO);
            if (kioskAttractionDTO != null
                                    && ((kioskAttractionDTO.AttractionBookingDTO != null && kioskAttractionDTO.AttractionBookingDTO.BookingId > -1)
                                      || kioskAttractionDTO.ChildAttractionBookingDTOList != null &&
                                          kioskAttractionDTO.ChildAttractionBookingDTOList.Exists(c => c.ChildAttractionBookingDTO != null &&
                                                                                                       c.ChildAttractionBookingDTO.BookingId > -1)))
            {
                // "Do you want to abort current transaction?"
                string messageTwo = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5009);
                using (frmYesNo f = new frmYesNo(messageTwo))
                {
                    if (f.ShowDialog() != DialogResult.Yes)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4198));
                        //"Sorry cannot go back without aborting current transaction."
                    }
                }
            }
            log.LogMethodExit();
        }

        private static void PerformAbortAction(KioskTransaction kioskTransaction, KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry("kioskTransaction", kioskAttractionDTO);
            try
            {
                if (kioskTransaction != null)
                {
                    try
                    {
                        try
                        {
                            kioskTransaction.ClearTemporarySlots(kioskAttractionDTO);
                        }
                        catch (Exception ex)
                        {

                            log.Error(ex);
                            KioskStatic.logToFile("Error while Clear TemporarySlots: " + ex.Message);
                            frmOKMsg.ShowUserMessage(ex.Message);
                        }
                        kioskTransaction.ClearTransaction(frmOKMsg.ShowUserMessage);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        KioskStatic.logToFile("Error while clearing transction: " + ex.Message);
                        frmOKMsg.ShowUserMessage(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in PerformAbortAction: " + ex.Message);
            }
            log.LogMethodExit();
        }
        protected virtual void CloseForms()
        {
            log.LogMethodEntry();
            int lowerLimit = 0;
            for (int i = Application.OpenForms.Count - 1; i > lowerLimit; i--)
            {
                Application.OpenForms[i].Close();
            }
            log.LogMethodExit();
        }

    }
}
