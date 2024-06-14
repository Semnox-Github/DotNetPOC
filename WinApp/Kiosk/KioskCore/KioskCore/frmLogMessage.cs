/********************************************************************************************
 * Project Name - KioskCore - frmLogMessage 
 * Description  - frmLogMessage
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       05-Sep-2018      Guru S A           Created 
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A          Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Windows.Forms;

namespace Semnox.Parafait.KioskCore
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
            txtLog.ForeColor = KioskStatic.CurrentTheme.FooterMsgErrorHighlightBackColor;
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
