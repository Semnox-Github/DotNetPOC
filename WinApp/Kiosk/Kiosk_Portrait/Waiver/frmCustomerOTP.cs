/********************************************************************************************
 * Project Name - Portrait Kiosk
 * Description  - user interface -frmCustomerOTP
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      9-Oct-2019      Deeksha            Created.
 *2.120      18-May-2021      Dakshakh Raj       Handling text box fore color changes.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.1     22-Feb-2023      Vignesh Bhat       Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Windows.Forms;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using System.Drawing;
using Semnox.Parafait.Languages;
using System.Collections.Generic;

namespace Parafait_Kiosk
{
    public partial class frmCustomerOTP : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private CustomerDTO customerDTO;
        private string OTP;
        private string defaultMsg;
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;
        public frmCustomerOTP(CustomerDTO customerDTO)
        {
            log.LogMethodEntry(customerDTO);
            this.utilities = KioskStatic.Utilities; 
            this.customerDTO = customerDTO; 
            utilities.setLanguage();
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            SetTextBoxFontColors();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.logToFile("Loading customer OTP form");
            defaultMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2425);//Please enter the OTP
            InitializeKeyboard();
            SetCustomizedFontColors();
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmCustomerOTP_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetFocus();
            HideKeyboardObject();
            txtMessage.Text = defaultMsg;
            int timeOutcounterValue = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "VALIDITY_PERIOD_FOR_WAIVER_REGISTRATION_OTP", 5));
            timeOutcounterValue = timeOutcounterValue * 60;

            KioskStatic.logToFile("Time out counter value "+ timeOutcounterValue.ToString());
            log.Info("Time out counter value " + timeOutcounterValue.ToString());

            SetKioskTimerTickValue(timeOutcounterValue);
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0"); 
            if (!string.IsNullOrEmpty(customerDTO.Email))
            {
                OTP = GenerateOTPAndEmail();
                EmailOTP(OTP);
            }
            else
            {
                KioskStatic.logToFile("Email id is not provided, closing the form");
                log.Info("Email id is not provided");
                this.Close();
            } 
            log.LogMethodExit();
        }

        string GenerateOTPAndEmail()
        {
            log.LogMethodEntry(); 
            KioskStatic.logToFile("Generating OTP Code ");
            string otp = utilities.GenerateRandomNumber(6, Utilities.RandomNumberType.Numeric);
            log.LogMethodExit(otp);
            return otp;
        }
        private void InitializeKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    SetVirtualKeyboard();
                }
                else
                {
                    SetCustomKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing keyboard in Customer OTP screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                int loc = panel2.Top + (txtOTP.Top -18);
                virtualKeyboardController = new VirtualWindowsKeyboardController(loc);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, popupOnScreenKeyBoard);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in Customer OTP screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                int loc = panel2.Top + (txtOTP.Top - 18);
                customKeyboardController = new VirtualKeyboardController(loc);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry, null, lblOTP.Font.FontFamily.Name);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in Customer OTP screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void DisposeKeyboardObject()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.Dispose();
                }
                else
                {
                    customKeyboardController.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Disposing keyboard in Customer OTP screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        void EmailOTP(string otp)
        {
            log.LogMethodEntry(); 
            try
            {
                if (!string.IsNullOrEmpty(customerDTO.Email))
                {
                    KioskHelper.SendOTPEmail(customerDTO, otp, utilities);
                    ResetKioskTimer(); 
                }
                else
                {
                    log.Info("No email id, unable to send OTP");
                    KioskStatic.logToFile("No email id, unable to send OTP");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                using (frmOKMsg frmMsg = new frmOKMsg(ex.Message))
                {
                    frmMsg.ShowDialog();
                }
                KioskStatic.logToFile("Error: "+ txtMessage.Text);
            }
            
            log.LogMethodExit();
        }
        void ValidateOTP()
        {
            log.LogMethodEntry(); 
            KioskStatic.logToFile("Validate OTP");
            if (txtOTP.Text == OTP)
            {
                KioskStatic.logToFile("OTP validation is successful");
                log.Info("OTP validation is successful");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 2327);
                KioskStatic.logToFile(txtMessage.Text);
                log.Error(txtMessage.Text);
                using (frmOKMsg frmMsg = new frmOKMsg(txtMessage.Text))
                {
                    frmMsg.ShowDialog();
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
            if (tickSecondsRemaining <= 60)
            {
                //lblTimeRemaining.Font = TimeOutFont;
                lblTimeRemaining.Text = tickSecondsRemaining.ToString("#0");
            }
            else
            {
                //lblTimeRemaining.Font = savTimeOutFont;
                lblTimeRemaining.Text = (tickSecondsRemaining / 60).ToString() + ":" + (tickSecondsRemaining % 60).ToString().PadLeft(2, '0');
            }
            if (tickSecondsRemaining <= 0)
            {
                //displayMessageLine(MessageUtils.getMessage(457), WARNING);
                Application.DoEvents();
                this.Close();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void linkLblResendOTP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            { 
                KioskStatic.logToFile("Resend OTP link is clicked");
                //showhideKeypad('H');
                txtMessage.Text = defaultMsg;
                linkLblResendOTP.Enabled = false;
                using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2424)))//resend OTP?
                {
                    if (frmyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        KioskStatic.logToFile("Attempting to resend OTP");
                        log.Info("Attempting to resend OTP");
                        OTP = string.Empty;
                        OTP = GenerateOTPAndEmail();
                        EmailOTP(OTP); 
                    }
                    else
                    {
                        KioskStatic.logToFile("User clicked no for Resend OTP prompt");
                        log.Info("User clicked no for Resend OTP prompt");
                    }
                }
            }
            finally
            {
                linkLblResendOTP.Enabled = true;
            }
            log.LogMethodExit();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(); 
            txtMessage.Text = defaultMsg;
            //showhideKeypad('H');
            ValidateOTP();
            log.LogMethodExit();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(); 
            txtMessage.Text = defaultMsg;
            KioskStatic.logToFile("Cancel button is clicked");
            using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2326)))
            {
                if (frmyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    //this.Hide();
                    KioskStatic.logToFile("Cancel button is clicked, closing form");
                    log.Info("Cancel button is clicked, closing form");
                    this.Close();
                }
            }
            log.LogMethodExit();
        }
        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();

        } 
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        } 
        private void frmCustomerOTP_Closing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetKioskTimerTickValue();
            DisposeKeyboardObject();
            KioskStatic.logToFile("Closing customer OTP form");
            log.LogMethodExit();
        }

        private void SetTextBoxFontColors()
        {
            if (KioskStatic.CurrentTheme == null ||
                  (KioskStatic.CurrentTheme != null && KioskStatic.CurrentTheme.TextForeColor == Color.White))
            {
                txtOTP.ForeColor = Color.Black;
            }
            else
            {
                txtOTP.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCustomerOTP");
            try
            {
                this.lblOTP.ForeColor = KioskStatic.CurrentTheme.FrmCustOTPLblOTPTextForeColor;
                this.lblOTPmsg.ForeColor = KioskStatic.CurrentTheme.FrmCustOTPLblOTPmsgTextForeColor;
                this.lblTimeRemaining.ForeColor = KioskStatic.CurrentTheme.FrmCustOTPLblTimeRemainingTextForeColor;
                this.txtOTP.ForeColor = KioskStatic.CurrentTheme.FrmCustOTPTxtOTPTextForeColor;
                this.linkLblResendOTP.ForeColor = KioskStatic.CurrentTheme.FrmCustOTPLinkLblResendOTPTextForeColor;
                this.btnOkay.ForeColor = KioskStatic.CurrentTheme.FrmCustOTPBtnOkayTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmCustOTPBtnCancelTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmCustOTPTxtMessageTextForeColor;
                this.BackgroundImage = ThemeManager.GetBackgroundImageTwo(ThemeManager.CurrentThemeImages.CustomerOTPBackgroundImage);
                btnOkay.BackgroundImage =
                    btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                lblTimeRemaining.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCustomerOTP: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void HideKeyboardObject()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.HideKeyboard();
                }
                else
                {
                    customKeyboardController.HideKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in HideKeyboardObject() in Customer OTP screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetFocus()
        {
            log.LogMethodEntry();
            try
            {
                txtOTP.Focus(); 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetFocus(): " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
