/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Messages UI
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskDisplayMessage : frmRedemptionKioskBaseForm
    {
        public frmRedemptionKioskDisplayMessage(string message, bool responseRequired = false, string CloseButtonText = "")
        {
            log.LogMethodEntry(message, responseRequired);
            InitializeComponent();
            Common.utils.setLanguage(this);
            if (responseRequired)
            {
                btnClose.Visible = false;
                btnYes.Visible = btnNo.Visible = true;
                SetKioskTimerSecondsValue(5);
            }
            else
            {
                if (CloseButtonText != string.Empty)
                {
                    btnClose.Text = CloseButtonText;
                }
                btnClose.Visible = true;
                btnYes.Visible = btnNo.Visible = false;
                SetKioskTimerSecondsValue(3);
            }
            lblDisplayText1.Text = message;
            this.StartPosition = FormStartPosition.CenterScreen;
            log.LogMethodExit();
        }
        internal override void InactivityTimer_Tick(object sender, EventArgs e)
        {
            //log.LogMethodEntry(sender, e);
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            if (tickSecondsRemaining <= 60)
            {
                tickSecondsRemaining = tickSecondsRemaining - 1;
            }

            SetKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining == 0)
            {
                //if (redemptionOrder != null)
                //{
                //    redemptionOrder = null;
                //    redemptionOrder = new RedemptionBL(Common.utils);
                //}
                log.Debug("Close Message Form");
                this.Close();
                //Common.GoHome();
            }
           // log.LogMethodExit();
        }

    }
}


