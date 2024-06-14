/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - Helper class to run a long running process in the background and show popup if it takes more the delay in seconds
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By            Remarks          
 *********************************************************************************************
 *2.120.0     19-May-2021            Lakshminarayana        Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Helper class to run a long running process in the background and show popup if it takes more the delay in seconds
    /// </summary>
    public class BackgroundProcessRunner
    {
        /// <summary>
        /// StartBackgroundProcessAfterXSeconds
        /// </summary>
        public static int LaunchWaitScreenAfterXSeconds = 10;
        public static int LaunchWaitScreenAfterXLongSeconds = 15;
        public static void SetLaunchWaitScreenAfterXSeconds()
        {
            try
            {
                int retValue = 10;
                try
                {
                    string retValueInStringForm = ConfigurationManager.AppSettings["LaunchWaitScreenAfterXSeconds"];
                    retValue = Convert.ToInt32(string.IsNullOrWhiteSpace(retValueInStringForm) == false ? retValueInStringForm : "10");
                }
                catch { retValue = 10; }
                LaunchWaitScreenAfterXSeconds = retValue;
                int retLongValue = 15;
                try
                {
                    string retLongValueInStringForm = ConfigurationManager.AppSettings["LaunchWaitScreenAfterXLongSeconds"];
                    retLongValue = Convert.ToInt32(string.IsNullOrWhiteSpace(retLongValueInStringForm) == false ? retLongValueInStringForm : "15");
                }
                catch { retLongValue = 15; }
                LaunchWaitScreenAfterXLongSeconds = retLongValue;
            }
            catch { }
        }
        /// <summary>
        /// run the long running process in the background
        /// </summary>
        /// <param name="longRunningActivity">long running activity</param>
        /// <param name="message">message to be shown in wait message pop up</param>
        /// <param name="delayInSeconds">wait time before showing the pop up</param>
        public static void Run(Action longRunningActivity, string message = "", int delayInSeconds = 5)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Processing.. Please Wait..";
            }
            Form form = new WaitDialog(message);
            using (NoSynchronizationContextScope.Enter())
            {
                Task task = Task.Factory.StartNew(longRunningActivity).ContinueWith((t) => { HideProgressForm(form); });
                if (Task.WaitAny(task, Task.Delay(delayInSeconds * 1000)) == 1)
                {
                    form.ShowDialog();
                }
            }
        }

        /// <summary>
        /// run the long running process in the background
        /// </summary>
        /// <param name="longRunningActivity">long running activity</param>
        /// <param name="message">message to be shown in wait message pop up</param>
        /// <param name="delayInSeconds">wait time before showing the pop up</param>
        public static void Run(Action<IProgress<ProgressReport>> longRunningActivity, string message = "", int delayInSeconds = 5)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Processing.. Please Wait..";
            }
            WaitDialog form = new WaitDialog(message, true);
            IProgress<ProgressReport> progress = new Progress<ProgressReport>(form.ReportProgress);
            using (NoSynchronizationContextScope.Enter())
            {
                Task task = Task.Factory.StartNew(() => { longRunningActivity(progress); }).ContinueWith((t) => { HideProgressForm(form); });
                if (Task.WaitAny(task, Task.Delay(delayInSeconds * 1000)) == 1)
                {
                    form.ShowDialog();
                }
            }
        }

        private static void HideProgressForm(Form form)
        {
            try
            {
                form.Invoke(new Action(() => { form.Close(); }));
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// run the long running process in the background
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="longRunningActivity">long running activity</param>
        /// <param name="message">message to be shown in wait message pop up</param>
        /// <param name="delayInSeconds">wait time before showing the pop up</param>
        /// <returns></returns>
        public static T Run<T>(Func<T> longRunningActivity, string message = "", int delayInSeconds = 5)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Processing.. Please Wait..";
            }
            WaitDialog form = new WaitDialog(message);
            int errorRecoveryCounter = 0;
            try
            {
                form.Cursor = Cursors.WaitCursor;
                Task<T> longRunningtask = Task<T>.Factory.StartNew(longRunningActivity);
                using (NoSynchronizationContextScope.Enter())
                {

                    Task task = longRunningtask.ContinueWith((t) => { HideProgressForm(form); });
                    if (Task.WaitAny(task, Task.Delay(delayInSeconds * 1000)) == 1)
                    {
                        form.ShowDialog();
                    }
                }
                T result = longRunningtask.Result;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception((ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
            finally
            {
                CloseForm(form, errorRecoveryCounter);
            }
        }

        /// <summary>
        /// run the long running process in the background
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="longRunningActivity">long running activity</param>
        /// <param name="message">message to be shown in wait message pop up</param>
        /// <param name="delayInSeconds">wait time before showing the pop up</param>
        /// <returns></returns>
        public static T Run<T>(Func<IProgress<ProgressReport>, T> longRunningActivity, string message = "", int delayInSeconds = 5)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Processing.. Please Wait..";
            }
            WaitDialog form = new WaitDialog(message, true);
            int errorRecoveryCounter = 0;
            try
            {
                form.Cursor = Cursors.WaitCursor;
                IProgress<ProgressReport> progress = new Progress<ProgressReport>(form.ReportProgress);
                Task<T> longRunningtask = Task<T>.Factory.StartNew(() =>
                {
                    return longRunningActivity(progress);  
                });
                using (NoSynchronizationContextScope.Enter())
                {

                    Task task = longRunningtask.ContinueWith((t) => { HideProgressForm(form); });
                    if (Task.WaitAny(task, Task.Delay(delayInSeconds * 1000)) == 1)
                    {
                        form.ShowDialog();
                    }
                }
                T result = longRunningtask.Result;
                return result;
            }
            catch (Exception ex)
            { 
                throw new Exception((ex.InnerException != null ? ex.InnerException.Message: ex.Message));
            }
            finally
            {
                CloseForm(form, errorRecoveryCounter);
            }
        }

        private static void CloseForm(WaitDialog form, int errorRecoveryCounter)
        {
            try
            {
                if (form != null)
                {
                    form.Close();
                    form.Dispose();
                }
            }
            catch
            {
                errorRecoveryCounter++;
                if (errorRecoveryCounter < 4)
                {
                    CloseForm(form, errorRecoveryCounter);
                }
            }
        }
        /// <summary>
        /// Run
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="longRunningActivity"></param>
        /// <returns></returns>
        public static T Run<T>(Func<T> longRunningActivity)
        { 
            try
            { 
                Task<T> longRunningtask = Task<T>.Factory.StartNew(longRunningActivity);
                using (NoSynchronizationContextScope.Enter())
                {

                    Task task = longRunningtask.ContinueWith((t) => { DoNothing(); });
                    Task.WaitAny(task);
                }
                T result = longRunningtask.Result;
                return result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += ": " + ex.InnerException.Message;
                }
                throw new Exception(msg);
            } 
        }


        private static void DoNothing()
        { 
        }
    } 
}
