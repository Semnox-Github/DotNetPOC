/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmLogMessages.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80         4-Sep-2019       Deeksha        Added logger methods.
********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
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
            txtLog.ForeColor = KioskStatic.CurrentTheme.FieldLabelForeColor;
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
