/********************************************************************************************
 * Project Name - frmAgeGate
 * Description  - user interface
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmAgeGate : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = KioskStatic.Utilities;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;

        public DateTime BirthDate = DateTime.MinValue;
        public CustomerDTO customerDTO = null;
        string _cardNumber;
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;

        public frmAgeGate(string CardNumber = "")
        {
            log.LogMethodEntry(CardNumber);
            InitializeComponent();

            _cardNumber = CardNumber;
            log.LogVariableState("CardNumber", _cardNumber);
            KioskStatic.setDefaultFont(this);//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnCancel.BackgroundImage = btnNext.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;//Ends:Modification on 17-Dec-2015 for introducing new theme
            panel1.BackgroundImage = ThemeManager.CurrentThemeImages.BirthDateEntryBox;
            lblSiteName.Text = KioskStatic.SiteHeading;

            //if (KioskStatic.CurrentTheme.TextForeColor != Color.White)//Starts:Modification on 17-Dec-2015 for introducing new theme
            //{
            //    txtDate1.ForeColor = txtDate2.ForeColor = txtDate3.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            //}
            //else
            //{
            //    txtDate1.ForeColor = txtDate2.ForeColor = txtDate3.ForeColor = Color.DarkOrchid;
            //}
            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.AgeGateBackgroundImage);
           
            //    lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            //    label1.ForeColor = label2.ForeColor = label4.ForeColor = lblDateFormat.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            //Ends:Modification on 17-Dec-2015 for introducing new theme
            label1.Text = Utilities.MessageUtils.getMessage(802);
            label2.Text = Utilities.MessageUtils.getMessage(803);
            //label3.Text = Utilities.MessageUtils.getMessage(804);
            SetCustomizedFontColors();
            DisplaybtnCancel(true);
            DisplaybtnHome(false);
            txtDate1.Clear();
            txtDate2.Clear();
            txtDate3.Clear();
            InitializeKeyboard();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
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
                KioskStatic.logToFile("Error Initializing keyboard in  Get Email Details screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(panel1.Top);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { }, popupOnScreenKeyBoard);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in  Age Gate screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                customKeyboardController = new VirtualKeyboardController(panel1.Top);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() {  }, showKeyboardOnTextboxEntry, null, label1.Font.FontFamily.Name);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in  Age Gate screen: " + ex.Message);
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
                KioskStatic.logToFile("Error Initializing keyboard in  Age Gate screen: " + ex.Message);
            }
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
            try
            {
                //inactiveTimer.Stop();
                //secondsRemaining = 30;
                ResetKioskTimer();
                StopKioskTimer();
                //if (keypad != null && keypad.IsDisposed == false)
                //    keypad.Hide();
                string dateFormatToBeUsed = "MM-dd-yyyy";
                if (!string.IsNullOrEmpty(txtDate1.Text.Trim())
                  && !string.IsNullOrEmpty(txtDate2.Text.Trim())
                  && !string.IsNullOrEmpty(txtDate3.Text.Trim()))
                {
                    try
                    {
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                        BirthDate = DateTime.ParseExact(txtDate1.Text + txtDate2.Text + txtDate3.Text, dateFormatToBeUsed.Replace("/", "").Replace("-", ""), provider);
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
                            log.Error(exp.Message);
                            using (frmOKMsg fom = new frmOKMsg(MessageUtils.getMessage(10)))
                            {
                                fom.ShowDialog();
                            }
                            this.ActiveControl = txtDate1;
                            //inactiveTimer.Start();
                            StartKioskTimer();
                            log.LogMethodExit();
                            return;
                        }
                        log.Error("Error occurred while executing btnYes_Click()" + ex.Message);
                    }

                    try
                    {
                        string[] dates = BirthDate.ToString(dateFormatToBeUsed).Split('/', '-');
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
                            //(new frmOKMsg(Utilities.MessageUtils.getMessage(805, ageLimit))).ShowDialog();
                            using (frmOKMsg fom = new frmOKMsg(Utilities.MessageUtils.getMessage(805, ageLimit)))
                            {
                                fom.ShowDialog();
                            }
                            //inactiveTimer.Start();
                            StartKioskTimer();
                            this.ActiveControl = txtDate1;
                            log.LogMethodExit();
                            return;
                        }
                    }
                    DisposeKeyboardObject();
                    //if (keypad != null && keypad.IsDisposed == false)
                    //    keypad.Close();

                    using (Customer fcustomer = new Customer(_cardNumber, BirthDate, true))
                    {
                        DialogResult dr = fcustomer.ShowDialog();
                        if (dr == DialogResult.Cancel)
                        {
                            string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Timeout");
                            throw new CustomerStatic.TimeoutOccurred(msg);
                        }
                        customerDTO = fcustomer.customerDTO;
                    }

                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    Close();
                }
                else
                {
                    //(new frmOKMsg(Utilities.MessageUtils.getMessage(10))).ShowDialog();
                    using (frmOKMsg fom = new frmOKMsg(Utilities.MessageUtils.getMessage(10)))
                    {
                        fom.ShowDialog();
                    }
                    //inactiveTimer.Start();
                    StartKioskTimer();
                    this.ActiveControl = txtDate1;
                }
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                KioskStatic.logToFile("Timeout occured");
                log.Error(ex);
                PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                this.DialogResult = DialogResult.Cancel;
                log.LogMethodExit();
                return;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in frmAgeGate screen in btnYes_Click(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void lnkTerms_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            StopKioskTimer();
            //if (keypad.IsDisposed == false)
            //    keypad.Hide();
            using (frmTermsAndConditions frt = new frmTermsAndConditions(KioskStatic.ApplicationContentModule.REGISTRATION))
            {
                if (frt.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    chkReadConfirm.Checked = true;
                }
            }
            //inactiveTimer.Start();
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void chkReadConfirm_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            btnNext.Enabled = chkReadConfirm.Checked;

            if (chkReadConfirm.Checked)
                pbCheckBox.Image = Properties.Resources.tick_box_checked;
            else
                pbCheckBox.Image = Properties.Resources.tick_box_unchecked;
            log.LogMethodExit();
        }

        //AlphaNumericKeyPad keypad;
        //Timer inactiveTimer = new Timer();
        //int secondsRemaining = 30;
        private void frmAgeGate_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.ActiveControl = txtDate1;
            log.LogMethodExit();
        }

        private void txtBirthDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox txt = sender as TextBox;
            //if (keypad == null || keypad.IsDisposed)
            //{
            //    keypad = new AlphaNumericKeyPad(this, txt, KioskStatic.CurrentTheme.KeypadSizePercentage);
            //    keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, this.Height - keypad.Height - 30);
            //}
            //else
            //    keypad.currentTextBox = txt;

            //keypad.Show();
            Application.DoEvents();
            txt.SelectAll();
            log.LogMethodExit();
        }

        private void frmAgeGate_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            // inactiveTimer.Stop();
            SetKioskTimerTickValue();
            ResetKioskTimer();
            //if (keypad != null && keypad.IsDisposed == false)
            //    keypad.Close();
            DisposeKeyboardObject();
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
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in age gate");
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
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in age gate: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void FormOnKeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(e);
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
        private void FormOnKeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
        private void FormOnKeyUp(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(e);
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
        private void FormOnMouseClick(Object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(); try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
