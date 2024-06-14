/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Timeout Counter UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskTimeOutCounter : Form
    {
        Timer timer;
        int timeOut = 21;
        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public frmRedemptionKioskTimeOutCounter()
        {
            log.LogMethodEntry();
            InitializeComponent();
            KioskStatic.Utilities.setLanguage(this);
            timer = new Timer();
            log.LogMethodExit();
        }

        private void FrmTimeOutCounter_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            timer.Start();            
            lblTimeOut.Text = (timeOut / 2).ToString();
            Common.utils.setLanguage(this);
            log.LogMethodExit();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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

        private void FrmTimeOutCounter_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            timer.Stop();
            log.LogMethodExit();
        }

        private void FrmTimeOutCounter_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = System.Windows.Forms.DialogResult.Abort;
            Close();
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
    }
}
