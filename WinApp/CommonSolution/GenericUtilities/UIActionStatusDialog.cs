/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - pop up message box with message for non background UI actions
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By            Remarks          
 *********************************************************************************************
 *2.130.8    19-May-2022            Guru S A        Created
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// pop up message box with message for UI Action Status
    /// </summary>
    public partial class UIActionStatusDialog : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool runTimer = true;
        private Timer statusDialogTimer;
        private int openUICount = 1;
        private int showCounter = 0;
        public delegate void RaiseFocus();
        public event RaiseFocus raiseFocusEvent;
        private ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue;
        private static string CLOSEFORM = "CLOSEFORM";
        public UIActionStatusDialog(string message, int openFormCount, bool showProgressbar = false, ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue = null)
        {
            log.LogMethodEntry(message, openFormCount, showProgressbar, statusProgressMsgQueue);
            InitializeComponent();
            this.TopMost = false;
            this.statusProgressMsgQueue = statusProgressMsgQueue;
            label.Text = message;
            lblProgressMessage.Visible = showProgressbar;
            pbProgress.Visible = showProgressbar;
            if (showProgressbar == false)
            {
                label.Location = new System.Drawing.Point(label.Location.X, (this.Height - label.Height) / 2);
            }
            openUICount = openFormCount;//Application.OpenForms.Count + 1;
            // this.Visible = false;
            this.WindowState = FormWindowState.Minimized;
            log.LogMethodExit();
        }

        private void WaitDialogLoad(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.WindowState = FormWindowState.Minimized;
            //this.Visible = false;
            SetTimer();
            this.WindowState = FormWindowState.Minimized;
            log.LogMethodExit();
        }
        private void SetTimer()
        {
            log.LogMethodEntry();
            if (runTimer)
            {
                statusDialogTimer = new Timer();
                statusDialogTimer.Interval = 25;
                statusDialogTimer.Tick += StatusDialogTimer_Tick;
                statusDialogTimer.Start();
                showCounter = 0;
            }
            log.LogMethodExit();
        }

        private void StatusDialogTimer_Tick(object sender, EventArgs e)
        {
            bool dontRestart = false;
            try
            {
                statusDialogTimer.Stop();
                //SetWindowStateAsNormal();
                HideOrBringFront();
                if (statusProgressMsgQueue != null)
                {
                    KeyValuePair<int, string> msgItem = new KeyValuePair<int, string>();
                    while (statusProgressMsgQueue.IsEmpty == false)
                    {
                        statusProgressMsgQueue.TryDequeue(out msgItem);
                        if (msgItem.Value == CLOSEFORM)
                        {
                            dontRestart = true;
                            this.Close();
                            log.LogMethodExit("CLose form");
                            break;
                        }
                        else
                        {
                            if (pbProgress.Visible)
                            {
                                lblProgressMessage.Text = msgItem.Value;
                                pbProgress.Value = msgItem.Key;
                                HideOrBringFront();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dontRestart == false)
                {
                    statusDialogTimer.Start();
                }
            }
        }

        private void HideOrBringFront()
        {
            try
            {
                int latestOpenCOunt = Application.OpenForms.Count;
                if (latestOpenCOunt > openUICount && this.WindowState == FormWindowState.Normal)
                {
                    showCounter = 0;
                    this.WindowState = FormWindowState.Minimized;
                    RaiseFocusEvent();
                    //this.Visible = false;
                }
                if (latestOpenCOunt <= openUICount && showCounter < 3)
                {
                    this.WindowState = FormWindowState.Normal;
                    showCounter++;
                    //this.Visible = true;
                    this.BringToFront();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SetWindowStateAsNormal()
        {
            try
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
            }
            catch (Exception ex)
            { 
            }
        }

        private void WaitDialogClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (statusDialogTimer != null)
                {
                    statusDialogTimer.Stop();
                    statusDialogTimer.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        public void RaiseFocusEvent()
        {
            log.LogMethodEntry();
            try
            {
                if (raiseFocusEvent != null)
                {
                    raiseFocusEvent();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }

}
