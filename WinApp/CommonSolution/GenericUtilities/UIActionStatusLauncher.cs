/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - Launcher for pop up message box with message for non background UI actions
 * 
 * 
 **************
 **Version Log
 **************
 *Version       Date                   Modified By            Remarks          
 *********************************************************************************************
 *2.130.9.02    08-Sep-2022            Guru S A        Created
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Semnox.Core.GenericUtilities.UIActionStatusDialog;

namespace Semnox.Core.GenericUtilities
{
    public class UIActionStatusLauncher : IDisposable
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private System.Timers.Timer waitTimer;
        private RaiseFocus raiseFocusEvent;
        private ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue;
        private bool showProgress;
        private int waitSeconds;
        private int openFormCount;
        private string msg;
        private int countValue = 1;
        private ApplicationContext ap;
        /// <summary>
        /// UIActionStatusLauncherV2
        /// </summary> 
        /// <returns></returns>
        public UIActionStatusLauncher(string msg, RaiseFocus raiseFocusEvent, ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue,
            bool showProgress, int waitSeconds)
        {
            log.LogMethodEntry();
            this.countValue = 1;
            this.ap = new ApplicationContext();
            this.msg = msg;
            this.openFormCount = Application.OpenForms.Count + 1;
            this.raiseFocusEvent = raiseFocusEvent;
            this.statusProgressMsgQueue = statusProgressMsgQueue;
            this.showProgress = showProgress;
            this.waitSeconds = waitSeconds;
            waitTimer = new System.Timers.Timer();
            waitTimer.Interval = waitSeconds * 1000;
            waitTimer.Elapsed += new System.Timers.ElapsedEventHandler(WaitUITimerTick);
            waitTimer.Start();
            waitTimer.Enabled = true;
            log.LogMethodExit();
        }

        private void WaitUITimerTick(Object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                waitTimer.Stop();
                bool dontRestart = false;
                if (statusProgressMsgQueue != null)
                {
                    KeyValuePair<int, string> msgItem = new KeyValuePair<int, string>();
                    while (statusProgressMsgQueue.IsEmpty == false)
                    {
                        statusProgressMsgQueue.TryDequeue(out msgItem);
                        if (msgItem.Value == "CLOSEFORM")
                        {
                            dontRestart = true;
                            break;
                        }
                    }
                }
                if (dontRestart == false)
                {
                    try
                    {
                        Thread t = new Thread(ThreadedForm);
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void ThreadedForm()
        {
            try
            {
                using (UIActionStatusDialog f = new UIActionStatusDialog(msg, openFormCount, showProgress, statusProgressMsgQueue))
                {
                    f.raiseFocusEvent += new UIActionStatusDialog.RaiseFocus(raiseFocusEvent);
                    //f.Show();
                    ap.MainForm = f;
                    if (ap.MainForm != null)
                    {
                        ap.MainForm.WindowState = FormWindowState.Minimized;
                    }
                    Application.Run(ap);
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// DisposeAP
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="countValue"></param>
        public void Dispose()
        {
            log.LogMethodEntry();
            try
            {
                DisposeObjects();
            }
            catch
            {
                //log.Error(ex);
                Dispose();
            }
            log.LogMethodExit();
        }

        private void DisposeObjects()
        {
            countValue++;
            if (waitTimer != null && countValue < 4)
            {
                SendMessageToStatusMsgQueue(statusProgressMsgQueue, "CLOSEFORM", 100, 100);
                waitTimer.Stop();
                System.Threading.Thread.Sleep(100);
                waitTimer.Enabled = false;
                waitTimer.Dispose();
                if (ap != null)
                {
                    ap.ExitThread();
                    ap.Dispose();
                }
            }
        }

        /// <summary>
        /// SendMessageToStatusMsgQueue
        /// </summary> 
        public static void SendMessageToStatusMsgQueue(ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue,
            string processMsg, int totalCount, int currentCount)
        {
            try
            {
                //log.LogMethodEntry();
                if (statusProgressMsgQueue != null)
                {
                    //string processMsg = MessageContainerList.GetMessage(executionContext, "of");
                    statusProgressMsgQueue.Enqueue(new KeyValuePair<int, string>(((currentCount * 100) / totalCount), processMsg));
                }
            }
            catch (Exception ex)
            {
            }
            //log.LogMethodExit();
        }
    }
}
