/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmTimeOutCounter.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80      5-Sep-2019       Deeksha            Added logger methods.
********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmTimeOutCounter : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Timer timer;
        int timeOut = 21;

        public frmTimeOutCounter()
        {
            log.LogMethodEntry();
            InitializeComponent();
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.setDefaultFont(this);//Starts:Modification on 17-Dec-2015 for introducing new theme
            this.BackgroundImage = ThemeManager.CurrentThemeImages.CountdownTimer;//Ends:Modification on 17-Dec-2015 for introducing new theme
            timer = new Timer();
            log.LogMethodExit();
        }

        private void frmTimeOutCounter_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            timer.Interval = 500;
            timer.Tick += timer_Tick;
            timer.Start();

            lblTimeOut.Text = (timeOut / 2).ToString();
            log.LogMethodExit();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            timeOut--;

            if (timeOut == 1)
            {
                this.Hide();
                DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }

            lblTimeOut.Text = (timeOut / 2).ToString();
            log.LogMethodExit();
        }

        private void frmTimeOutCounter_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            timer.Stop();
            log.LogMethodExit();
        }

        private void frmTimeOutCounter_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.Abort;
            Close();
            log.LogMethodExit();
        }
    }
}
