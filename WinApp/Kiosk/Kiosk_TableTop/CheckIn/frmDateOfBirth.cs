/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - frmEditBirthYear Handles Playground Entry menu
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.0.0   19-Sep-2022      Sathyavathi        Created: Check-In feature Phase-2
*2.150.0.0   02-Dec-2022      Sathyavathi        Check-In feature Phase-2 Additional features
********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;
using System.Drawing;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Parafait_Kiosk
{
    public partial class frmDateOfBirth : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime dateOfBirth;
        private bool allowEditBirthDayAndMonth;
        private int day;
        private int month;
        private int year;

        internal DateTime DateOfBirth { get { return dateOfBirth; } set { } }

        public frmDateOfBirth(DateTime doB, bool allowEditDayAndMonth, string headerMsg)
        {
            log.LogMethodEntry(doB, allowEditDayAndMonth, headerMsg);
            InitializeComponent();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardBox;
            btnPrev.BackgroundImage = btnSave.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            this.Size = this.BackgroundImage.Size;
            KioskStatic.setDefaultFont(this);
            KioskStatic.Utilities.setLanguage(this);

            flpCalender.Visible = true;
            this.dateOfBirth = doB;
            this.allowEditBirthDayAndMonth = allowEditDayAndMonth;

            DisplaybtnCancel(false);
            DisplaybtnPrev(true);
            btnPrev.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Cancel");
            btnSave.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Save");

            if (dateOfBirth != DateTime.MinValue)
            {
                day = dateOfBirth.Day;
                month = dateOfBirth.Month;
                year = dateOfBirth.Year;


                lblDay.Text = dateOfBirth.Day.ToString();
                string dateOfBirthFormat = KioskStatic.DateMonthFormat;
                dateOfBirthFormat = Regex.Replace(dateOfBirthFormat, @"[^0-9a-zA-Z]+", "");
                dateOfBirthFormat = dateOfBirthFormat.Trim('y', 'd', 'Y', 'D');
                lblMonth.Text = dateOfBirth.ToString(dateOfBirthFormat);
                lblYear.Text = dateOfBirth.Year.ToString();
            }
            else
            {
                lblDay.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Select");
                lblMonth.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Select");
                lblYear.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Select");
            }
            SortCalenderFields();
            if (allowEditBirthDayAndMonth)
            {
                LoadDayOfCalenderColumn();
                LoadMonthOfCalenderColumn();
            }
            LoadYearOfCalenderColumn();
            panelCalenderContents.Visible = false;
            dgvDay.Visible = dgvMonth.Visible = dgvYear.Visible = false;

            lblDay.Enabled = lblMonth.Enabled = allowEditBirthDayAndMonth;
            lblHeader.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, headerMsg);
            lblMsg.Text = allowEditBirthDayAndMonth ? "" : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4348); //"You are allowed to change only the birth year";
            if (!allowEditBirthDayAndMonth)
            {
                lblDay.ForeColor = lblMonth.ForeColor = SystemColors.InactiveCaptionText;
                lblDay.BackColor = lblMonth.BackColor = SystemColors.InactiveCaption;
            }
            panelCalenderContents.Visible = false;
            SetKioskTimerTickValue(30);
            //ResetKioskTimer();
            SetCustomImages();
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        private void frmEditBirthYear_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            Application.DoEvents();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e) //OK button click
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                if (day > 0 && month > 0 && year > 0)
                {
                    dateOfBirth = new DateTime(year, month, day);
                    lblDay.BackColor = lblMonth.BackColor = lblYear.BackColor = Color.White;
                }
                Close();
            }
            catch (Exception ex)
            {
                string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4809); //ERROR: Invalid date of birth
                log.Error(errMsg, ex);
                KioskStatic.logToFile(errMsg + ex.Message);
                frmOKMsg.ShowUserMessage(errMsg);
                lblDay.BackColor = lblMonth.BackColor = lblYear.BackColor = Color.OrangeRed;
            }
            log.LogMethodExit();
        }

        public override void btnPrev_Click(object sender, EventArgs e) //Back button click
        {
            log.LogMethodEntry(sender, e);
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void lblDay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();

                if (!allowEditBirthDayAndMonth)
                    return;

                panelCalenderContents.SuspendLayout();
                ClearCalenderContents();
                panelCalenderContents.Controls.Add(dgvDay);
                dgvDay.Visible = true;
                dgvMonth.Visible = dgvYear.Visible = false;
                vScrollBar.DataGridView = dgvDay;
                panelCalenderContents.Refresh();
                panelCalenderContents.ResumeLayout();
                panelCalenderContents.Visible = true;
                Point lblDayLocationOnForm = lblDay.FindForm().PointToClient(lblDay.Parent.PointToScreen(lblDay.Location));
                panelCalenderContents.Location = new Point(lblDayLocationOnForm.X, lblDayLocationOnForm.Y);
                panelCalenderContents.BringToFront();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error: Error in lblDay_Click() of Date Of Birth form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void lblMonth_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                if (!allowEditBirthDayAndMonth)
                    return;

                panelCalenderContents.SuspendLayout();
                ClearCalenderContents();
                panelCalenderContents.Controls.Add(dgvMonth);
                dgvMonth.Visible = true;
                dgvDay.Visible = dgvYear.Visible = false;
                vScrollBar.DataGridView = dgvMonth;
                panelCalenderContents.Location = new Point(lblMonth.Top, lblMonth.Top);
                panelCalenderContents.Refresh();
                panelCalenderContents.ResumeLayout();
                panelCalenderContents.Visible = true;
                Point lblMonthLocationOnForm = lblMonth.FindForm().PointToClient(lblMonth.Parent.PointToScreen(lblMonth.Location));
                panelCalenderContents.Location = new Point(lblMonthLocationOnForm.X, lblMonthLocationOnForm.Y);
                panelCalenderContents.BringToFront();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error: Error in lblMonth_Click() of Date Of Birth form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void lblYear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                panelCalenderContents.SuspendLayout();
                ClearCalenderContents();
                panelCalenderContents.Controls.Add(dgvYear);
                dgvYear.Visible = true;
                dgvDay.Visible = dgvMonth.Visible = false;
                vScrollBar.DataGridView = dgvYear;
                panelCalenderContents.Location = new Point(lblYear.Top, lblYear.Top);
                panelCalenderContents.Refresh();
                panelCalenderContents.ResumeLayout();
                panelCalenderContents.Visible = true;
                Point lblYearLocationOnForm = lblYear.FindForm().PointToClient(lblYear.Parent.PointToScreen(lblYear.Location));
                panelCalenderContents.Location = new Point(lblYearLocationOnForm.X, lblYearLocationOnForm.Y);
                panelCalenderContents.BringToFront();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error: Error in lblYear_Click() of Date Of Birth form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvYear_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            try
            {
                year = Convert.ToInt32(dgvYear.CurrentRow.Cells["dgvTextBoxColumnYear"].Value);
                lblYear.Text = year.ToString();
                panelCalenderContents.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error: Error in dgvYear_CellClick() in Date Of Birth form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvMonth_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            try
            {
                string monthName = dgvMonth.CurrentRow.Cells["dgvTextBoxColumnMonth"].Value.ToString();
                month = DateTime.ParseExact(monthName, "MMM", System.Globalization.CultureInfo.CurrentCulture).Month;
                lblMonth.Text = monthName;
                panelCalenderContents.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error: Error in dgvMonth_CellClick() in Date Of Birth form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvDay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            try
            {
                day = Convert.ToInt32(dgvDay.CurrentRow.Cells["dgvTextBoxColumnDay"].Value);
                lblDay.Text = day.ToString();
                panelCalenderContents.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error: Error in dgvDay_CellClick in Date Of Birth form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadDayOfCalenderColumn()
        {
            log.LogMethodEntry();
            try
            {
                for (int i = 1; i <= 31; i++)
                {
                    int rowId = dgvDay.Rows.Add();
                    DataGridViewRow newRow = dgvDay.Rows[rowId];
                    dgvDay.Rows[rowId].Cells["dgvTextBoxColumnDay"].Value = i.ToString();
                }
                dgvDay.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error: Error in LoadDayOfDOBColumn in Date Of Birth form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadMonthOfCalenderColumn()
        {
            log.LogMethodEntry();
            try
            {
                string[] arrOfMonthNames = System.Globalization.DateTimeFormatInfo.InvariantInfo.AbbreviatedMonthGenitiveNames;
                List<string> monthNames = new List<string>(arrOfMonthNames);
                for (int i = 0; i < monthNames.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(monthNames[i]))
                    {
                        int rowId = dgvMonth.Rows.Add();
                        DataGridViewRow newRow = dgvMonth.Rows[rowId];
                        dgvMonth.Rows[rowId].Cells["dgvTextBoxColumnMonth"].Value = monthNames[i];
                    }
                }
                dgvMonth.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error: Error in LoadMonthOfDOBColumn in Date Of Birth form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadYearOfCalenderColumn()
        {
            log.LogMethodEntry();
            try
            {
                for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 120; i--)
                {
                    int rowId = dgvYear.Rows.Add();
                    DataGridViewRow newRow = dgvYear.Rows[rowId];
                    dgvYear.Rows[rowId].Cells["dgvTextBoxColumnYear"].Value = i.ToString();
                }
                dgvYear.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error: Error in LoadYearOfDOBColumn in Date Of Birth form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SortCalenderFields()
        {
            try
            {
                ResetKioskTimer();

                //Convert date format into array of characters and remove duplicate characters
                var uniqueCharArray = KioskStatic.DateMonthFormat.ToLower().ToCharArray().Distinct().ToArray();
                var resultString = new String(uniqueCharArray.Where(Char.IsLetter).ToArray());
                flpCalender.Controls.Clear();

                switch (resultString)
                {
                    case "mdy":
                        flpCalender.Controls.Add(lblMonth);
                        flpCalender.Controls.Add(lblScrollImageMonth);

                        flpCalender.Controls.Add(lblDay);
                        flpCalender.Controls.Add(lblScrollImageDay);

                        flpCalender.Controls.Add(lblYear);
                        flpCalender.Controls.Add(lblScrollImageYear);

                        break;

                    case "ymd":
                        flpCalender.Controls.Add(lblYear);
                        flpCalender.Controls.Add(lblScrollImageYear);

                        flpCalender.Controls.Add(lblMonth);
                        flpCalender.Controls.Add(lblScrollImageMonth);

                        flpCalender.Controls.Add(lblDay);
                        flpCalender.Controls.Add(lblScrollImageDay);
                        break;

                    case "dmy":
                    default:
                        flpCalender.Controls.Add(lblDay);
                        flpCalender.Controls.Add(lblScrollImageDay);

                        flpCalender.Controls.Add(lblMonth);
                        flpCalender.Controls.Add(lblScrollImageMonth);

                        flpCalender.Controls.Add(lblYear);
                        flpCalender.Controls.Add(lblScrollImageYear);
                        break;
                }
                lblDayHeader.Location = new Point(lblDay.Location.X, lblDayHeader.Location.Y);
                lblMonthHeader.Location = new Point(lblMonth.Location.X, lblMonthHeader.Location.Y);
                lblYearHeader.Location = new Point(lblYear.Location.X, lblYearHeader.Location.Y);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error: Error while binding DOB Month filed in Select Child() in frmEditbirthYear: " + ex.Message);
            }
        }

        private void ClearCalenderContents()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();

                foreach (Control c in panelCalenderContents.Controls)
                {
                    if (c.Name == dgvDay.Name
                        || c.Name == dgvMonth.Name
                        || c.Name == dgvYear.Name)
                    {
                        panelCalenderContents.Controls.Remove(c);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error: Error in lblDay_Click() of Date Of Birth form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void vScrollBar_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
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
                Application.DoEvents();
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardBox;
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.panelCalenderContents.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                this.btnPrev.BackgroundImage = btnSave.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;

                vScrollBar.DownButtonBackgroundImage = 
                    vScrollBar.DownButtonDisabledBackgroundImage =
                    lblScrollImageDay.Image = 
                    lblScrollImageMonth.Image = 
                    lblScrollImageYear.Image = ThemeManager.CurrentThemeImages.ScrollDownButton;

                vScrollBar.UpButtonBackgroundImage = 
                    vScrollBar.UpButtonDisabledBackgroundImage = ThemeManager.CurrentThemeImages.ScrollUpButtonImage;
            }
            catch (Exception ex)
            {
                string msg = "Error while Setting Customized background images for frmSelectAdult: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblMsg.ForeColor = KioskStatic.CurrentTheme.EditDateOfBirthScreenHeaderTextForeColor;
                this.btnSave.ForeColor = KioskStatic.CurrentTheme.EditDateOfBirthScreenBtnSaveTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.EditDateOfBirthScreenBtnCancelTextForeColor;
                lblDayHeader.ForeColor = 
                    lblMonthHeader.ForeColor = 
                    lblYearHeader.ForeColor = KioskStatic.CurrentTheme.EditDateOfBirthCalenderHeaderTextForeColor;
                dgvDay.RowTemplate.DefaultCellStyle.ForeColor =
                    dgvMonth.RowTemplate.DefaultCellStyle.ForeColor =
                    dgvYear.RowTemplate.DefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.EditDateOfBirthGridTextForeColor;
                lblHeader.ForeColor = KioskStatic.CurrentTheme.EditDateOfBirthGreetingTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmEdirBirthYear_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            log.LogMethodExit();
        }
    }
}
