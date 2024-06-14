/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - frmOKMsg.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        4-Sep-2019       Deeksha             Added logger methods.
*2.150.1     22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Parafait.KioskCore;
using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmOKMsg : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //int ticks = 0;
        public frmOKMsg(string message, bool enableTimeOut = true)
        {
            log.LogMethodEntry(message, enableTimeOut);
            InitializeComponent();            
            KioskStatic.setDefaultFont(this);
            this.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardBox;//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnClose.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;//Ends:Modification on 17-Dec-2015 for introducing new theme
            this.Size = this.BackgroundImage.Size;
            KioskStatic.Utilities.setLanguage(this);

            lblmsg.Text = message;
            //if (enableTimeOut)
            //    timer1.Start();
            if (!enableTimeOut)
            {
                StopKioskTimer();
            }
            log.LogMethodExit();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            if (tickSecondsRemaining < 20)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            else
                setKioskTimerSecondsValue(tickSecondsRemaining - 1);
            log.LogMethodExit();
        }

        private void btnClose_MouseDown(object sender, MouseEventArgs e)
        {
            // btnClose.BackgroundImage = Properties.Resources.cancel_btn_pressed;//Starts:Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnClose_MouseUp(object sender, MouseEventArgs e)
        {
            //btnClose.BackgroundImage = Properties.Resources.cancel_btn;//Starts:Modification on 17-Dec-2015 for introducing new theme
        }

        private void frmOKMsg_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timer1.Stop();
            //if (!GetKioskTimer())
            //{	
            //    StartKioskTimer();
            //}
            log.LogMethodEntry();
            StopKioskTimer();
            log.LogMethodExit();
        }

        private void frmOKMsg_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(lblmsg.Text))
                lblmsg.Text = this.Text;
            log.LogMethodExit();
        }
        
        public static void ShowUserMessage(string message)
        {
            using (frmOKMsg frm = new frmOKMsg(message))
            {
                frm.ShowDialog();
            }
        }
        public static void ShowOkMessage(string message, bool enableTimeOut)
        {
            using (frmOKMsg frm = new frmOKMsg(message, enableTimeOut))
            {
                frm.ShowDialog();
            }
        }
    }
}
