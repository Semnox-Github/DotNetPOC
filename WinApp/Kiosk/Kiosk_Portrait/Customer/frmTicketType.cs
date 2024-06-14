/********************************************************************************************
 * Project Name - frmTicketType
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.130.0     30-Jun-2021      Dakshakh            Theme changes to support customized Font ForeColor
 *2.150.1     22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 *******************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    public partial class frmTicketType : BaseForm  
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool isCardRealticket = false;
        AccountDTO accountDTO;
        Utilities utilities; 
        public frmTicketType(Utilities utilities, AccountDTO accountDTO)
        {
            log.LogMethodEntry(utilities, accountDTO);
            InitializeComponent();
            StartKioskTimer();
            this.accountDTO = accountDTO;            
            this.utilities = utilities;
            BtnRealTicket.Text = BtnRealTicket.Text.TrimStart();
            btnEticket.Text = btnEticket.Text.TrimStart();
            BtnRealTicket.Text = "     "+BtnRealTicket.Text;
            btnEticket.Text = "     " + btnEticket.Text;
            if (accountDTO.RealTicketMode)
            {
                isCardRealticket = true;
                BtnRealTicket.BackgroundImage = Parafait_Kiosk.Properties.Resources.ImageChecked;
                btnEticket.BackgroundImage = Parafait_Kiosk.Properties.Resources.ImageUnchecked;
            }
            else
            {
                isCardRealticket = false;
                btnEticket.BackgroundImage = Parafait_Kiosk.Properties.Resources.ImageChecked;
                BtnRealTicket.BackgroundImage = Parafait_Kiosk.Properties.Resources.ImageUnchecked;
            }

            //this.BackgroundImage = KioskStatic.CurrentTheme.YesNoBox;
            this.panelTicketMode.BackgroundImageLayout = ImageLayout.Stretch;
            this.panelTicketMode.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.TicketTypeBox);
            //this.Size = this.BackgroundImage.Size;
            btnOk.BackgroundImage = btnCancel.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.YesNoButtons);
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void BtnRealTicket_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            //load checked real ticket image
            isCardRealticket = true;
            BtnRealTicket.BackgroundImage = Parafait_Kiosk.Properties.Resources.ImageChecked;
            btnEticket.BackgroundImage = Parafait_Kiosk.Properties.Resources.ImageUnchecked;
            log.LogMethodExit();
        }

        private void btnEticket_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            //load checked E ticket image
            isCardRealticket = false;
            btnEticket.BackgroundImage = Parafait_Kiosk.Properties.Resources.ImageChecked;
            BtnRealTicket.BackgroundImage = Parafait_Kiosk.Properties.Resources.ImageUnchecked;
            log.LogMethodExit();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (accountDTO != null)
            {
                try
                {
                    accountDTO.RealTicketMode = isCardRealticket;
                    AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountDTO);
                    accountBL.Save(null);                   
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message,ex);
                    using (frmOKMsg oKMsg = new frmOKMsg(ex.Message))
                    {
                        oKMsg.ShowDialog();
                        oKMsg.Dispose();
                    }
                }
                finally
                {
                    StopKioskTimer();
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
            log.LogMethodExit();
        }
        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }

            if (tickSecondsRemaining <= 0)
            {                
                Close();
            }
            log.LogMethodExit();
        }        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            this.DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblChangeTicketMode.ForeColor = KioskStatic.CurrentTheme.TicketTypeHeaderTextForeColor;//(Change Ticket Mode header)
                this.BtnRealTicket.ForeColor = KioskStatic.CurrentTheme.TicketTypeOption1TextForeColor;//Faq(Terms)
                this.btnEticket.ForeColor = KioskStatic.CurrentTheme.TicketTypeOption2TextForeColor;//Footer text message
                this.btnOk.ForeColor = KioskStatic.CurrentTheme.TicketTypeBtnOkTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.TicketTypeBtnCancelTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
