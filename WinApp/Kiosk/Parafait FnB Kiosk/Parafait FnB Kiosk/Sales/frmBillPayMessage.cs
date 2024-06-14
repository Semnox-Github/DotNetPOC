/********************************************************************************************
* Project Name - Parafait_Kiosk -frmBillPayMessage.cs
* Description  - frmBillPayMessage 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.logger;
using System.Threading;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Parafait_FnB_Kiosk
{
    public partial class frmBillPayMessage : BaseForm
    {
        BillAcceptor billAcceptor;
        KioskStatic.acceptance _ac;
        string billMessage = "";
        decimal AmountRequired = 0;
        bool cancelPressed;
        int operationStatus = -1;

        public frmBillPayMessage(string Message, KioskStatic.acceptance ac)
        {
            log.LogMethodEntry(Message, ac);
            InitializeComponent();
            InactivityTimerSwitch(false);
            lblDisplayText1.Text = Message;
            KioskStatic.ac = _ac = ac;
            cancelPressed = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            AmountRequired = UserTransaction.OrderDetails.TotalAmount;
            SetBillAcceptor();
            if (billAcceptor != null)
            {
                KioskStatic.ac = _ac = billAcceptor.acceptance;
                _ac.productPrice = AmountRequired;
            }
            if (_ac == null)
                _ac = KioskStatic.ac = new KioskStatic.acceptance();
            log.LogMethodExit();
        }

        public void frmBillPayMessage_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (billAcceptor != null)
            {
                int maxWait = 1000;
                while (billAcceptor.Working == false && maxWait > 0)
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                    maxWait -= 10;
                }
            }

            if (billAcceptor != null && billAcceptor.Working)
            {
                btnCancel.Enabled = true;
                btnCancel.Visible = true;
                cancelPressed = false;
                lblDisplayText1.Text = Common.utils.MessageUtils.getMessage(1397, (AmountRequired - _ac.totalValue).ToString(Common.utils.ParafaitEnv.AMOUNT_FORMAT));
            }
            else
            {
                Common.ShowMessage(Common.utils.MessageUtils.getMessage(1400));
                if (billAcceptor != null)
                {
                    billAcceptor.disableBillAcceptor();
                }
                Close();
            }
            log.LogMethodExit();
        }

        public delegate void frmBillPayMessageDelegate(KioskStatic.acceptance ac, int operationStatus);
        public frmBillPayMessageDelegate setCallBack;

        //public delegate void frmBillPayMessageAbortDelegate(KioskStatic.acceptance ac);
        //public frmBillPayMessageAbortDelegate setAbortCallBack;       

        private void frmBillPayMessage_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            if (billAcceptor != null)
            {
                Common.logToFile("Form close disabling billAcceptor");
                billAcceptor.disableBillAcceptor();
                if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
                {
                    ((NV9USB)billAcceptor).dataReceivedEvent = null;
                }
                else
                {
                    KioskStatic.baReceiveAction = null;
                }

            }
            log.LogMethodExit();
        }

        void refreshTotals()
        {
            Common.logEnter();
            if ((AmountRequired - _ac.totalValue) > 0)
            {
                lblDisplayText1.Text = Common.utils.MessageUtils.getMessage(1397, (AmountRequired - _ac.totalValue).ToString(Common.utils.ParafaitEnv.AMOUNT_FORMAT));
            }
            else
            {
                lblDisplayText1.Text = Common.utils.MessageUtils.getMessage(1396, (0).ToString(Common.utils.ParafaitEnv.AMOUNT_FORMAT));
            }
            Application.DoEvents();
            Common.logExit();
        }
        private Object thisLock = new Object();
        void checkMoneyStatus()
        {
            Common.logEnter();
            Common.logToFile("checkMoneyStatus() - enter");

            lock (thisLock)
            {
                try
                {
                    Common.logToFile("checkMoneyStatus()- before checking TotalValue greater than or equals to AmountRequired");
                    if (_ac.totalValue >= AmountRequired)
                    {
                        Common.logToFile("checkMoneyStatus() - ac.totalValue = " + _ac.totalValue.ToString("N2") + ", AmountRequired = " + AmountRequired.ToString("N2"));
                        if (cancelPressed)
                        {
                            Common.logToFile("checkMoneyStatus() - Cancel button Pressed");
                            return;
                        }
                        btnCancel.Enabled = false;
                        Application.DoEvents();
                        if (billAcceptor != null)
                        {
                            Common.logToFile("checkMoneyStatus() - disabling billAcceptor");
                            System.Threading.Thread.Sleep(300);
                            billAcceptor.disableBillAcceptor();
                        }
                        Application.DoEvents();
                        Common.logToFile("checkMoneyStatus() - Before calling setCallBack()");
                        operationStatus = 1;
                        setCallBack(_ac, operationStatus);

                        Common.logToFile("checkMoneyStatus() - After calling setCallBack()");
                        Close();
                        AmountRequired = 0;
                    }
                    else
                    {
                        Common.logToFile("checkMoneyStatus() - TotalValue greater than or equls to AmountRequired condition failed- ac.totalValue = " + _ac.totalValue.ToString("N2") + ", AmountRequired = " + AmountRequired.ToString("N2"));
                    }
                }
                catch (Exception ex)
                {
                    Common.logException(ex);
                    Common.ShowMessage(ex.Message);
                    Common.logToFile("checkMoneyStatus():" + ex.Message + "-" + ex.StackTrace);
                }
                finally
                {
                    Common.logToFile("checkMoneyStatus() - exit");
                    btnCancel.Enabled = true;
                }
                Common.logExit();
            }
        }

        void SetBillAcceptor()
        {
            Common.logEnter();
            int baport = Properties.Settings.Default.BillAcceptorPort;
            if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB) && baport > 0)
            {
                billAcceptor = KioskStatic.getBillAcceptor(baport.ToString());
                ((NV9USB)billAcceptor).dataReceivedEvent = billAcceptorDataReceived;
                billAcceptor.initialize();
            }
            else if (baport != 0)
            {
                billAcceptor = KioskStatic.getBillAcceptor(baport.ToString());
                KioskStatic.baReceiveAction = billAcceptorDataReceived;
                billAcceptor.requestStatus();
            }
            else
            {
                Common.logToFile("Please check bill Acceptor setup. Bill Acceptor port no " + baport.ToString());
            }
            Common.logExit();
        }

        void billAcceptorDataReceived()
        {
            log.LogMethodEntry();
            billMessage = "";
            KioskStatic.billAcceptorDatareceived = true;
            bool noteRecd = billAcceptor.ProcessReceivedData(KioskStatic.billAcceptorRec, ref billMessage);

            if (noteRecd)
            {
                Common.logToFile("Bill Acceptor note received: " + billMessage);
                //KioskStatic.updateKioskActivityLog(, -1, (CurrentCard == null ? "" : CurrentCard.CardNumber), "BILL-IN", "Bill Inserted", ac);
                KioskActivityLogDTO kioskActivityLogDTO = new KioskActivityLogDTO("N", ServerDateTime.Now, "BILL-IN", Convert.ToDouble(KioskStatic.config.Notes[billAcceptor.ReceivedNoteDenomination].Value),
                                                    "", KioskStatic.config.Notes[billAcceptor.ReceivedNoteDenomination].Name + " Bill Inserted", Common.utils.ParafaitEnv.POSMachineId,
                                                    Common.utils.ParafaitEnv.POSMachine, "", false,
                                                    _ac.TrxId, 0, KioskStatic.GlobalKioskTrxId++, -1);
                KioskActivityLogBL kioskActivityLogBL = new KioskActivityLogBL(kioskActivityLogDTO);
                kioskActivityLogBL.Save();

                billAcceptor.ReceivedNoteDenomination = 0;
                System.Threading.Thread.Sleep(300);
                billAcceptor.requestStatus();

                this.BeginInvoke((MethodInvoker)delegate
                {
                    refreshTotals();
                    checkMoneyStatus();
                });
            }
            if (billAcceptor.overpayReject)
            {
                billAcceptor.overpayReject = false;
                System.Threading.Thread.Sleep(300);
                billAcceptor.requestStatus();
                this.BeginInvoke((MethodInvoker)delegate
                {
                    lblDisplayText1.Text = lblDisplayText1.Text + Common.utils.MessageUtils.getMessage(1398);
                });
            }
            log.LogMethodExit();
        }

        int cancelPressCount = 0;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                cancelPressed = true;
                btnCancel.Enabled = false;
                bool loopBreak = false;
                int iters = 100;
                while (iters-- > 0)
                {
                    Thread.Sleep(20);
                    Application.DoEvents();
                    if ((billAcceptor != null && billAcceptor.ReceivedNoteDenomination > 0))
                    {
                        loopBreak = true;
                        break;
                    }
                }
                btnCancel.Enabled = true;
                Common.logToFile("Cancel pressed");

                if ((billAcceptor != null && billAcceptor.ReceivedNoteDenomination > 0) || loopBreak)
                {
                    cancelPressed = false;
                    if (cancelPressCount++ < 3)
                    {
                        log.LogMethodExit();
                        return;
                    }
                }
                if (billAcceptor != null && billAcceptor.StillProcessing())
                {
                    cancelPressed = false;
                    btnCancel.Enabled = true;
                    Common.logToFile("billAcceptor is stillProcessing. Wait");
                    return;
                }
                if (billAcceptor != null)
                    billAcceptor.disableBillAcceptor();

                Application.DoEvents();

                if (_ac.totalValue > 0)
                {
                    if (Common.ShowDialog(Common.utils.MessageUtils.getMessage(1402)) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //setAbortCallBack(_ac);
                        operationStatus = 0;
                        setCallBack(_ac, operationStatus);
                        this.Close();
                    }
                    else
                    {
                        cancelPressed = false;
                        if (billAcceptor != null)
                        {
                            if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
                            {
                                billAcceptor.disableBillAcceptor();
                                billAcceptor.initialize();
                            }
                            else
                            {
                                billAcceptor.requestStatus();
                            }
                        }
                    }
                }
                else
                {
                    if (billAcceptor != null)
                    {
                        billAcceptor.disableBillAcceptor();
                    }
                    this.Close();
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                Common.logToFile("btnCancel_Click(): " + ex.Message + ":" + ex.StackTrace);
                cancelPressed = false;
                btnCancel.Enabled = true;
                if (billAcceptor != null)
                    billAcceptor.initialize();
            }
        }
    }
}
