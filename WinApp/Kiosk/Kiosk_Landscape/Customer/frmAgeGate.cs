/********************************************************************************************
 * Project Name - frmAgeGate
 * Description  - user interface
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00                                        Created 
 *2.4.0       25-Nov-2018      Raghuveera     terms and condition is passing to the customer form
 *2.80        4-Sep-2019       Deeksha        Added logger methods.
 *2.150.1    22-Feb-2023       Guru S A       Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmAgeGate : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            //lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnNo.BackgroundImage = btnNext.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
            if (KioskStatic.CurrentTheme.TextForeColor != Color.White)
            {
                txtDate1.ForeColor = txtDate2.ForeColor = txtDate3.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            else
            {
                txtDate1.ForeColor = txtDate2.ForeColor = txtDate3.ForeColor = Color.DarkOrchid;
            }

            //label1.ForeColor = label2.ForeColor = label3.ForeColor = label4.ForeColor = lblDateFormat.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;//Ends:Modification on 17-Dec-2015 for introducing new theme

            label1.Text = Utilities.MessageUtils.getMessage(802);
            label2.Text = Utilities.MessageUtils.getMessage(803);
            label3.Text = Utilities.MessageUtils.getMessage(804);

            lblDateFormat.Text = "(" + ParafaitEnv.DATE_FORMAT.ToUpper() + ")";

            string dateFormat = ParafaitEnv.DATE_FORMAT.ToLower();

            //dateFormat = dateFormat.Replace("d", "0").Replace("MMM", ">LLL").Replace("MM", "00").Replace("y", "0");

            int daywidth = 2;
            int monthwidth = dateFormat.Count(x => x == 'm');
            int yearwidth = dateFormat.Count(x => x == 'y');
            int unitwidth = 32;

            switch (dateFormat[0])
            {
                case 'd': txtDate1.Width = daywidth * unitwidth; txtDate1.MaxLength = daywidth; break;
                case 'm': txtDate1.Width = monthwidth * unitwidth; txtDate1.MaxLength = monthwidth; break;
                case 'y': txtDate1.Width = yearwidth * unitwidth; txtDate1.MaxLength = yearwidth; break;
            }

            int i = 0;
            while (i < dateFormat.Length & (dateFormat[i] == dateFormat[0] | dateFormat[i] == '/' | dateFormat[i] == '-'))
            {
                i++;
            }

            if (i == dateFormat.Length)
                txtDate2.Visible = txtDate3.Visible = false;
            else
            {
                switch (dateFormat[i])
                {
                    case 'd': txtDate2.Width = daywidth * unitwidth; txtDate2.MaxLength = daywidth; break;
                    case 'm': txtDate2.Width = monthwidth * unitwidth; txtDate2.MaxLength = monthwidth; break;
                    case 'y': txtDate2.Width = yearwidth * unitwidth; txtDate2.MaxLength = yearwidth; break;
                }
            }

            int j = i;
            while (j < dateFormat.Length & (dateFormat[j] == dateFormat[i] | dateFormat[j] == '/' | dateFormat[j] == '-'))
            {
                j++;
            }
            if (j == dateFormat.Length)
                txtDate3.Visible = false;
            else
            {
                switch (dateFormat[j])
                {
                    case 'd': txtDate3.Width = daywidth * unitwidth; txtDate3.MaxLength = daywidth; break;
                    case 'm': txtDate3.Width = monthwidth * unitwidth; txtDate3.MaxLength = monthwidth; break;
                    case 'y': txtDate3.Width = yearwidth * unitwidth; txtDate3.MaxLength = yearwidth; break;
                }
            }

            txtDate2.Left = txtDate1.Left + txtDate1.Width + 3;
            txtDate3.Left = txtDate2.Left + txtDate2.Width + 3;

            txtDate1.Clear();
            txtDate2.Clear();
            txtDate3.Clear();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, Math.Max(0, Screen.PrimaryScreen.WorkingArea.Height - this.Height - 290));
            log.LogMethodExit();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BirthDate = DateTime.MinValue;
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            //inactiveTimer.Stop();
            log.LogMethodEntry();
            StopKioskTimer();
            if (!string.IsNullOrEmpty(txtDate1.Text.Trim())
              &&  !string.IsNullOrEmpty(txtDate2.Text.Trim())
              &&  !string.IsNullOrEmpty(txtDate3.Text.Trim()))
            {
                try
                {
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    BirthDate = DateTime.ParseExact(txtDate1.Text + txtDate2.Text + txtDate3.Text, ParafaitEnv.DATE_FORMAT.Replace("/", "").Replace("-", ""), provider);
                }
                catch(Exception ex)
                {
                    try
                    {
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
                        BirthDate = Convert.ToDateTime(txtDate1.Text + "-" + txtDate2.Text + "-" + txtDate3.Text, provider);
                    }
                    catch(Exception exp)
                    {
                        log.Error(exp.Message);
                        (new frmOKMsg(MessageUtils.getMessage(10))).ShowDialog();
                        //inactiveTimer.Start();
                        StartKioskTimer();
                        log.LogMethodExit();
                        return;
                    }
                    log.Error("Error occurred while executing btnYes_Click()" + ex.Message);
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
                    log.Error("Error occurred while executing btnYes_Click()" + ex.Message);
                }

                string ageLimit = Utilities.getParafaitDefaults("REGISTRATION_AGE_LIMIT").Trim();
                if (!string.IsNullOrEmpty(ageLimit))
                {
                    if (DateTime.Now < BirthDate.AddYears(Convert.ToInt32(ageLimit)))
                    {
                        (new frmOKMsg(Utilities.MessageUtils.getMessage(805, ageLimit))).ShowDialog();
                        this.ActiveControl = txtDate1;
                        //inactiveTimer.Start();
                        StartKioskTimer();
                        log.LogMethodExit();
                        return;
                    }
                }

                if (keypad != null && keypad.IsDisposed == false)
                    keypad.Close();

                using (Customer fcustomer = new Customer(_cardNumber, BirthDate, true))
                {
                    fcustomer.ShowDialog();
                    customerDTO = fcustomer.customerDTO;
                }
                this.Close();//Modification on 17-Dec-2015 for introducing new theme
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
            //secondsRemaining = 30;
            //inactiveTimer.Stop();
            log.LogMethodEntry();
            ResetKioskTimer();
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
            //secondsRemaining = 30;
            log.LogMethodEntry();
            ResetKioskTimer();
            btnNext.Enabled = chkReadConfirm.Checked;

            if (chkReadConfirm.Checked)
                pbCheckBox.Image = Properties.Resources.tick_box;
            else
                pbCheckBox.Image = Properties.Resources.tick_box_blank;
            log.LogMethodExit();
        }

        AlphaNumericKeyPad keypad;
        //Timer inactiveTimer = new Timer();
        //int secondsRemaining = 30;
        private void frmAgeGate_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.ActiveControl = txtDate1;

            //inactiveTimer.Interval = 30000;
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
                keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height - 20);
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
    }
}
