/********************************************************************************************
 * Project Name - User
 * Description  - Class of CueTextBox
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's. 
 **********************************************************************************************/
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Text box showing prompt text
    /// </summary>
    public class CueTextBox : TextBox
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Show the prompt text to the user
        /// </summary>
        [Localizable(true)]
        public string Cue
        {
            get { return mCue; }
            set { mCue = value; updateCue(); }
        }

        private void updateCue()
        {
            log.LogMethodEntry();
            if (this.IsHandleCreated && mCue != null)
            {
                SendMessage(this.Handle, 0x1501, (IntPtr)1, mCue);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// On Handle Created
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            log.LogMethodEntry(e);
            base.OnHandleCreated(e);
            updateCue();
            log.LogMethodExit();
        }
        private string mCue;

        // PInvoke
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, string lp);
    }
}
