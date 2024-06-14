/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - frmLogMessage.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using Semnox.Parafait.KioskCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmLogMessage : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool IgnoreAndContinue = false;
        System.Threading.ManualResetEvent mrevent;
        public frmLogMessage(System.Threading.ManualResetEvent mre)
        {
            log.LogMethodEntry(mre);
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            txtLog.ForeColor = Color.Black;
            mrevent = mre;
            log.LogMethodExit();
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            IgnoreAndContinue = true;
            btnIgnore.Enabled = false;
            mrevent.Set();
            log.LogMethodExit();
        }

        public void AppendLog(string text)
        {
            log.LogMethodEntry(text);
            txtLog.AppendText(text + Environment.NewLine);
            KioskStatic.logToFile(text);
            log.LogMethodExit();
        }

        public void ClearLog()
        {
            log.LogMethodEntry();
            txtLog.Clear();
            btnIgnore.Enabled = true;
            log.LogMethodExit();
        }
    }
}
