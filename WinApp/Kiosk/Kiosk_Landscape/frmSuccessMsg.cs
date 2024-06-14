/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - frmSuccessMsg.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        4-Sep-2019       Deeksha             Added logger methods.
*2.150.1     22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;

namespace Parafait_Kiosk
{//Starts:Modification on 17-Dec-2015 for introducing new theme
    public partial class frmSuccessMsg : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //int ticks = 0;
        public frmSuccessMsg(string message, string Action, string Balance, string point, string pass,string Source)
        {
            log.LogMethodEntry(message, Action, Balance, point, pass, Source);
            InitializeComponent();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
            
            this.Size = this.BackgroundImage.Size;
            btnClose.BackgroundImage = ThemeManager.CurrentThemeImages.DoneButton;
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;            
            KioskStatic.setDefaultFont(this);
            if (!string.IsNullOrEmpty(message))
            {
                lblmsg.Text = message;
                lblmsg.Visible = true;
            }
            if (!string.IsNullOrEmpty(Action))
            {
                lblAction.Text = Action;
                lblAction.Visible = true;
            }
            if (!string.IsNullOrEmpty(Balance))
            {
                lblBalanceMsg.Text = Balance;
                lblBalanceMsg.Visible = true;
            }
            if (!string.IsNullOrEmpty(point))
            {
                lblPoint.Text = point;
                lblPoint.Visible = true;
            }
            if (!string.IsNullOrEmpty(pass))
            {               
                lblPasNo.Text = pass;
                lblPasNo.Visible = true;
            }
            if (!string.IsNullOrEmpty(Source))
            {
                 switch(Source)
                 {
                     case "NEW": pbSuccess.Image = ThemeManager.CurrentThemeImages.SucessAddImage;
                pbSuccess.Visible = true;
                         break;
                     case "Recharge": pbSuccess.Image = ThemeManager.CurrentThemeImages.SucessRechargeImage;
                pbSuccess.Visible = true;
                         break;
                     case "Redeem": pbSuccess.Image = ThemeManager.CurrentThemeImages.SucessRedeemImage; 
                pbSuccess.Visible = true;
                         break;
                     case "Register": pbSuccess.Image = ThemeManager.CurrentThemeImages.SucessRegister;
                pbSuccess.Visible = true;
                         break;
                 }
                
            }
            KioskTimerInterval(3000);
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void btnClose_MouseDown(object sender, MouseEventArgs e)
        {
            //btnClose.BackgroundImage = Properties.Resources.cancel_btn_pressed;
        }

        private void btnClose_MouseUp(object sender, MouseEventArgs e)
        {
            //btnClose.BackgroundImage = Properties.Resources.cancel_btn;
        }

        private void frmOKMsg_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timer1.Stop();
            //StopKioskTimer();
        }

        private void frmOKMsg_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(lblmsg.Text))
                lblmsg.Text = this.Text;
            log.LogMethodExit();
        }

    }
}//Ends:Modification on 17-Dec-2015 for introducing new theme
