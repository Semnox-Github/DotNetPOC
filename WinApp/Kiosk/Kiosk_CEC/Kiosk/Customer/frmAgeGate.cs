/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmAgeGate.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 * 2.80      3-Sep-2019       Deeksha              Added logger methods.
 *2.130.0    30-Jun-2021      Dakshakh             Theme changes to support customized Font ForeColor
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmAgeGate : BaseFormKiosk
    {
        Utilities Utilities = KioskStatic.Utilities;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;

        public DateTime BirthDate = DateTime.MinValue;
        public CustomerDTO customerDTO = null;
        string _cardNumber;


        public frmAgeGate(string CardNumber = "")
        {
            log.LogMethodEntry(CardNumber);
            InitializeComponent();

            _cardNumber = CardNumber;
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.setDefaultFont(this);

            lblSiteName.Text = KioskStatic.SiteHeading;

            this.BackgroundImage = KioskStatic.CurrentTheme.AgeGateBackgroundImage;

            SetCustomizedFontColors();
            label1.Text = Utilities.MessageUtils.getMessage(802);
            label2.Text = Utilities.MessageUtils.getMessage(803);
            //label3.Text = Utilities.MessageUtils.getMessage(804);

            txtDate1.Clear();
            txtDate2.Clear();
            txtDate3.Clear();
            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BirthDate = DateTime.MinValue;
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //inactiveTimer.Stop();
            //secondsRemaining = 30;
            ResetKioskTimer();
            StopKioskTimer();
            if (keypad != null && keypad.IsDisposed == false)
                keypad.Hide();

            if (!string.IsNullOrEmpty(txtDate1.Text.Trim())
              && !string.IsNullOrEmpty(txtDate2.Text.Trim())
              && !string.IsNullOrEmpty(txtDate3.Text.Trim()))
            {
                try
                {
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    BirthDate = DateTime.ParseExact(txtDate1.Text + txtDate2.Text + txtDate3.Text, ParafaitEnv.DATE_FORMAT.Replace("/", "").Replace("-", ""), provider);
                }
                catch (Exception ex)
                {
                    try
                    {
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
                        BirthDate = Convert.ToDateTime(txtDate1.Text + "-" + txtDate2.Text + "-" + txtDate3.Text, provider);
                    }
                    catch (Exception exp)
                    {
                        (new frmOKMsg(MessageUtils.getMessage(10))).ShowDialog();
                        this.ActiveControl = txtDate1;
                        //inactiveTimer.Start();
                        StartKioskTimer();
                        log.Error(exp.Message);
                        log.LogMethodExit();
                        return;
                    }
                    log.Error(ex.Message);
                }

                try
                {
                    string[] dates = BirthDate.ToString(Utilities.getDateFormat()).Split('/', '-');
                    txtDate1.Text = dates[0];
                    txtDate2.Text = dates[1];
                    txtDate3.Text = dates[2];
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                string ageLimit = Utilities.getParafaitDefaults("REGISTRATION_AGE_LIMIT").Trim();
                if (!string.IsNullOrEmpty(ageLimit))
                {
                    if (ServerDateTime.Now < BirthDate.AddYears(Convert.ToInt32(ageLimit)))
                    {
                        (new frmOKMsg(Utilities.MessageUtils.getMessage(805, ageLimit))).ShowDialog();
                        //inactiveTimer.Start();
                        StartKioskTimer();
                        this.ActiveControl = txtDate1;
                        log.LogMethodExit();
                        return;
                    }
                }

                if (keypad != null && keypad.IsDisposed == false)
                    keypad.Close();

                Customer fcustomer = new Customer(_cardNumber, BirthDate);
                fcustomer.ShowDialog();
                customerDTO = fcustomer.customerDTO;

                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                Close();
            }
            else
            {
                (new frmOKMsg(Utilities.MessageUtils.getMessage(10))).ShowDialog();
                //inactiveTimer.Start();
                StartKioskTimer();
                this.ActiveControl = txtDate1;
            }
            log.LogMethodExit();
        }

        private void lnkTerms_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            //secondsRemaining = 30;    
            ResetKioskTimer();
            //inactiveTimer.Stop();
            StopKioskTimer();
            if (keypad.IsDisposed == false)
                keypad.Hide();
            if (new frmRegisterTnC().ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                chkReadConfirm.Checked = true;
            }
            //inactiveTimer.Start();           
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void chkReadConfirm_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //secondsRemaining = 30;
            ResetKioskTimer();

            btnNext.Enabled = chkReadConfirm.Checked;

            if (chkReadConfirm.Checked)
                pbCheckBox.Image = Properties.Resources.tick_box_checked;
            else
                pbCheckBox.Image = Properties.Resources.tick_box_unchecked;
            log.LogMethodExit();
        }

        AlphaNumericKeyPad keypad;
        //Timer inactiveTimer = new Timer();
        //int secondsRemaining = 30;
        private void frmAgeGate_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.ActiveControl = txtDate1;
            displaybtnPrev(false);
            displaybtnCancel(true);
            //inactiveTimer.Interval = 1000;
            //inactiveTimer.Tick += inactiveTimer_Tick;
            //inactiveTimer.Start();
            log.LogMethodExit();
        }

        private void txtBirthDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox txt = sender as TextBox;
            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, txt, KioskStatic.CurrentTheme.KeypadSizePercentage);
                keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, this.Height - keypad.Height - 30);
            }
            else
                keypad.currentTextBox = txt;

            keypad.Show();
            Application.DoEvents();
            txt.SelectAll();
            log.LogMethodExit();
        }

        private void frmAgeGate_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            //inactiveTimer.Stop();
            SetKioskTimerTickValue();
            ResetKioskTimer();
            if (keypad != null && keypad.IsDisposed == false)
                keypad.Close();
            log.LogMethodExit();
        }

        private void frmAgeGate_KeyPress(object sender, KeyPressEventArgs e)
        {
            //secondsRemaining = 30;
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void pbCheckBox_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            chkReadConfirm.Checked = !chkReadConfirm.Checked;
            log.LogMethodExit();
        }

        private void txtDate_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox txt = sender as TextBox;
            if (txt.Text.Length == txt.MaxLength)
            {
                Control c = this.GetNextControl(txt, true);
                if (c != null)
                    c.Focus();
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.label1.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenHeader1TextForeColor;//(Header 1 - Parents want to earn…) -#AgeGateScreenHeader1ForeCOlor
                this.label2.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenHeader2TextForeColor;//(Header 2 - Fill out your info...) - #AgeGateScreenHeader2ForeCOlor
                this.label4.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenHeader3TextForeColor;//(Header 3 - YourBirthDay) -#AgeGateScreenHeader3ForeColor
                this.txtDate1.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenMonthTextBoxTextForeColor;//(Month text box) -#AgeGateScreenMonthTextBoxForeColor
                this.txtDate2.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenDateTextBoxTextForeColor;//(Date text box) -#AgeGateScreenDateTextBoxForeColor
                this.txtDate3.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenYearTextBoxTextForeColor;//(Year text box) -#AgeGateScreenYearTextBoxForeColor
                this.lblDateFormat.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenDateFormatTextForeColor;//(MM-DD-YYYY Date format) -#AgeGateScreenDateFormatForeColor
                this.chkReadConfirm.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenReadConfirmTextForeColor;//(I have Read and Agree to) -#AgeGateScreenReadConfirmForeColor
                this.lnkTerms.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenlnkTermsTextForeColor;//(Privacy Policy / Terms of Use) -#AgeGateScreenlnkTermsForeColor
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenBtnCancelTextForeColor;//#AgeGateScreenBtnCancelForeColor
                this.btnNext.ForeColor = KioskStatic.CurrentTheme.AgeGateScreenBtnNextTextForeColor;//#AgeGateScreenBtnNextForeColor
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
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
