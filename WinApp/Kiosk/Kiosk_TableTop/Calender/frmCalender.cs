/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - frmDateTimePicker: Allows to pick and save date and time
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
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
using System.Globalization;

namespace Parafait_Kiosk
{
    public partial class frmCalender : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime dateTime;
        private bool showDatePicker;
        private bool showTimePicker;
        private bool allowEditDay;
        private bool allowEditMonth;
        private bool allowEditYear;
        private bool allowEditTime;
        private int day;
        private int month;
        private int year;
        private int hour;
        private int minute;
        private string ampm;
        private usrCtrlCalender usrCtrlDay;
        private usrCtrlCalender usrCtrlMonth;
        private usrCtrlCalender usrCtrlYear;
        private usrCtrlCalender usrCtrlHour;
        private usrCtrlCalender usrCtrlMinute;
        private usrCtrlCalender usrCtrlAMPM;

        internal DateTime DateTime { get { return dateTime; } set { } }

        public frmCalender(DateTime dateTime, bool showDate, bool allowEditDay, bool allowEditMonth, bool allowEditYear, bool showTime, bool allowEditTime)
        {
            log.LogMethodEntry(dateTime, showDate, showTime, allowEditDay, allowEditMonth, allowEditYear, allowEditTime);
            InitializeComponent();

            this.Size = this.BackgroundImage.Size;
            KioskStatic.setDefaultFont(this);
            KioskStatic.Utilities.setLanguage(this);

            this.dateTime = dateTime;
            this.showDatePicker = showDate;
            this.allowEditDay = allowEditDay;
            this.allowEditMonth = allowEditMonth;
            this.allowEditYear = allowEditYear;
            this.showTimePicker = showTime;
            this.allowEditTime = allowEditTime;

            DisplaybtnCancel(false);
            DisplaybtnPrev(true);
            SetKioskTimerTickValue(30);

            try
            {
                SetLabelDefaultText();
                SetCalenderStartupValues();
                RearrangeCalFieldsAsPerDateMonthConfig();
                LoadCalenderDropDownElements();
                SetCustomImages();
                SetCustomizedFontColors();
                GreyoutDisabledCalElements();
                SetCalenderLocation();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in frmCalender: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void GreyoutDisabledCalElements()
        {
            log.LogMethodEntry();

            if (showDatePicker)
            {
                if (!allowEditDay)
                {
                    lblDay.Enabled = false;
                    lblDay.ForeColor = SystemColors.InactiveCaptionText;
                    lblDay.BackColor = SystemColors.InactiveCaption;
                }

                if (!allowEditMonth)
                {
                    lblMonth.Enabled = false;
                    lblMonth.ForeColor = SystemColors.InactiveCaptionText;
                    lblMonth.BackColor = SystemColors.InactiveCaption;
                }

                if (!allowEditYear)
                {
                    lblYear.Enabled = false;
                    lblYear.ForeColor = SystemColors.InactiveCaptionText;
                    lblYear.BackColor = SystemColors.InactiveCaption;
                }
            }

            if (showTimePicker)
            {
                if (!allowEditTime)
                {
                    lblHour.Enabled =
                        lblMinute.Enabled =
                        lblAMPM.Enabled =
                        lblDelimeter.Enabled = false;

                    lblHour.ForeColor =
                        lblMinute.ForeColor =
                        lblAMPM.ForeColor =
                        lblDelimeter.ForeColor = SystemColors.InactiveCaptionText;

                    lblHour.BackColor =
                        lblMinute.BackColor =
                        lblAMPM.BackColor =
                        lblDelimeter.BackColor = SystemColors.InactiveCaption;
                }
            }
            log.LogMethodExit();
        }

        private void SetLabelDefaultText()
        {
            log.LogMethodEntry();

            btnPrev.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Cancel");
            btnSave.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Save");
            lblDelimeter.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, ":");

            log.LogMethodExit();
        }

        private void SetCalenderStartupValues()
        {
            log.LogMethodEntry();

            day = dateTime.Day;
            month = dateTime.Month;
            year = dateTime.Year;
            hour = dateTime.Hour;
            minute = dateTime.Minute;
            ampm = dateTime.ToString("tt", CultureInfo.InvariantCulture);

            lblDay.Text = dateTime.Day.ToString();
            string dateTimeFormat = KioskStatic.DateMonthFormat;
            dateTimeFormat = Regex.Replace(dateTimeFormat, @"[^0-9a-zA-Z]+", "");
            dateTimeFormat = dateTimeFormat.Trim('y', 'd', 'Y', 'D');
            lblMonth.Text = dateTime.ToString(dateTimeFormat);
            lblYear.Text = (dateTime == DateTime.MinValue) ? "1901" : dateTime.Year.ToString();
            lblHour.Text = dateTime.ToString("hh");
            lblMinute.Text = dateTime.ToString("mm");
            lblAMPM.Text = dateTime.ToString("tt", CultureInfo.InvariantCulture);

            log.LogMethodExit();
        }

        private void LoadCalenderDropDownElements()
        {
            log.LogMethodEntry();

            try
            {
                if (showDatePicker)
                {
                    if (allowEditDay)
                        LoadDayOfCalender();

                    if (allowEditMonth)
                        LoadMonthOfCalender();

                    if (allowEditYear)
                        LoadYearOfCalender();
                }

                if (showTimePicker && allowEditTime)
                {
                    LoadHourOfCalenderDate();
                    LoadMinutesOfCalenderDate();
                    LoadAMPMOfCalenderDate();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception  in LoadCalenderDropDownElements() of Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCalenderLocation()
        {
            log.LogMethodEntry();

            if (showDatePicker == true && showTimePicker == true)
            {
                //flpDatePicker.Location = new Point(50, 54);

                flpUsrCtrlDay.Location =
                    flpUsrCtrlMonth.Location =
                    flpUsrCtrlYear.Location = new Point(53, 140);

                flpUsrCtrlHour.Location =
                    flpUsrCtrlMinute.Location = new Point(545, 140);
                flpUsrCtrlAMPM.Location = new Point(802, 140);
            }
            else if (showDatePicker == true && showTimePicker == false)
            {
                flpDatePicker.Location = new Point(270, 54);

                flpUsrCtrlDay.Location =
                    flpUsrCtrlMonth.Location =
                    flpUsrCtrlYear.Location = new Point(273, 140);

            }
            else if (showDatePicker == false && showTimePicker == true)
            {
                flpTimePicker.Location = new Point(288, 54);

                flpUsrCtrlHour.Location =
                    flpUsrCtrlMinute.Location = new Point(291, 140);
                flpUsrCtrlAMPM.Location = new Point(548, 140);
            }
            log.LogMethodExit();
        }

        private void frmCalender_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            SetDatePickerVisibility(showDatePicker, ((allowEditDay == true) || (allowEditMonth == true)) ? false : allowEditYear,
                    (allowEditDay == true) ? false : allowEditMonth, allowEditDay);

            SetTimePickerVisibility(showTimePicker, allowEditTime, false, false);

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
                    DateTime selectedDate = new DateTime(year, month, day, hour, minute, 0);
                    if (DateTime.Compare(selectedDate, KioskStatic.Utilities.getServerTime()) > 0)
                    {
                        string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4822); //Birthday can''t be a date in the future
                        throw new ArgumentException(errMsg);
                    }
                    dateTime = selectedDate;
                    lblDay.BackColor = lblMonth.BackColor = lblYear.BackColor = Color.White;
                }
                Close();
            }
            catch (Exception ex)
            {
                string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 15); //Invalid date value
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

                if (!allowEditDay)
                    return;

                SetDatePickerVisibility(true, false, false, true);
                usrCtrlDay.HighlightSelectedVaue(lblDay.Text);
                usrCtrlDay.UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error: ERROR in lblDay_Click() of Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void lblMonth_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();

                if (!allowEditMonth)
                    return;

                SetDatePickerVisibility(true, false, true, false);
                usrCtrlMonth.HighlightSelectedVaue(lblMonth.Text);
                usrCtrlMonth.UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in lblMonth_Click() of Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void lblYear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();

                if (!allowEditYear)
                    return;

                SetDatePickerVisibility(true, true, false, false);
                usrCtrlYear.HighlightSelectedVaue(lblYear.Text);
                usrCtrlYear.UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in lblYear_Click() of Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void lblHour_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                SetTimePickerVisibility(true, true, false, false);
                usrCtrlHour.HighlightSelectedVaue(lblHour.Text.TrimStart(new Char[] { '0' }));
                usrCtrlHour.UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in lblHour_Click() of Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void lblMinute_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                SetTimePickerVisibility(true, false, true, false);
                usrCtrlMinute.HighlightSelectedVaue(lblMinute.Text);
                usrCtrlMinute.UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in lblMinute_Click() of Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void lblAMPM_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                SetTimePickerVisibility(true, false, false, true);
                usrCtrlAMPM.HighlightSelectedVaue(lblAMPM.Text);
                usrCtrlAMPM.UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in lblMinute_Click() of Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadDayOfCalender()
        {
            log.LogMethodEntry();
            try
            {
                usrCtrlDay = new usrCtrlCalender(usrCtrlCalender.CalenderElement.DAY);
                usrCtrlDay.valueSelected += new usrCtrlCalender.ValueSelected(ProcessNewlyPickedCalenderItem);
                usrCtrlDay.HighlightSelectedVaue(lblDay.Text);
                flpUsrCtrlDay.Controls.Add(usrCtrlDay);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadDayOfDOBColumn in Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadMonthOfCalender()
        {
            log.LogMethodEntry();
            try
            {
                usrCtrlMonth = new usrCtrlCalender(usrCtrlCalender.CalenderElement.MONTH);
                usrCtrlMonth.valueSelected += new usrCtrlCalender.ValueSelected(ProcessNewlyPickedCalenderItem);
                usrCtrlMonth.HighlightSelectedVaue(lblMonth.Text);
                flpUsrCtrlMonth.Controls.Add(usrCtrlMonth);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadMonthOfCalender in Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadYearOfCalender()
        {
            log.LogMethodEntry();
            try
            {
                usrCtrlYear = new usrCtrlCalender(usrCtrlCalender.CalenderElement.YEAR);
                usrCtrlYear.valueSelected += new usrCtrlCalender.ValueSelected(ProcessNewlyPickedCalenderItem);
                usrCtrlYear.HighlightSelectedVaue(lblYear.Text);
                flpUsrCtrlYear.Controls.Add(usrCtrlYear);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadYearOfCalender in Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadHourOfCalenderDate()
        {
            log.LogMethodEntry();
            try
            {
                usrCtrlHour = new usrCtrlCalender(usrCtrlCalender.CalenderElement.HOUR);
                usrCtrlHour.valueSelected += new usrCtrlCalender.ValueSelected(ProcessNewlyPickedCalenderItem);
                usrCtrlHour.HighlightSelectedVaue(lblHour.Text);
                flpUsrCtrlHour.Controls.Add(usrCtrlHour);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadTimePickerHour in Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadMinutesOfCalenderDate()
        {
            log.LogMethodEntry();
            try
            {
                usrCtrlMinute = new usrCtrlCalender(usrCtrlCalender.CalenderElement.MINUTE);
                usrCtrlMinute.valueSelected += new usrCtrlCalender.ValueSelected(ProcessNewlyPickedCalenderItem);
                usrCtrlMinute.HighlightSelectedVaue(lblMinute.Text);
                flpUsrCtrlMinute.Controls.Add(usrCtrlMinute);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadTimePickerMinutes in Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadAMPMOfCalenderDate()
        {
            log.LogMethodEntry();
            try
            {
                usrCtrlAMPM = new usrCtrlCalender(usrCtrlCalender.CalenderElement.AM_PM);
                usrCtrlAMPM.valueSelected += new usrCtrlCalender.ValueSelected(ProcessNewlyPickedCalenderItem);
                usrCtrlAMPM.HighlightSelectedVaue(lblAMPM.Text);
                flpUsrCtrlAMPM.Controls.Add(usrCtrlAMPM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadTimePickerAMPM in Calender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void RearrangeCalFieldsAsPerDateMonthConfig()
        {
            try
            {
                ResetKioskTimer();

                if (!showDatePicker)
                    return;

                //Convert date format into array of characters and remove duplicate characters
                var uniqueCharArray = KioskStatic.DateMonthFormat.ToLower().ToCharArray().Distinct().ToArray();
                var resultString = new String(uniqueCharArray.Where(Char.IsLetter).ToArray());

                flpDatePicker.Controls.Remove(flpMonth);
                flpDatePicker.Controls.Remove(flpDay);
                flpDatePicker.Controls.Remove(flpYear);

                switch (resultString)
                {
                    case "mdy":
                        flpDatePicker.Controls.Add(flpMonth);
                        flpDatePicker.Controls.Add(flpDay);
                        flpDatePicker.Controls.Add(flpYear);
                        break;

                    case "ymd":
                        flpDatePicker.Controls.Add(flpYear);
                        flpDatePicker.Controls.Add(flpMonth);
                        flpDatePicker.Controls.Add(flpDay);
                        break;

                    case "dmy":
                    default:
                        flpDatePicker.Controls.Add(flpDay);
                        flpDatePicker.Controls.Add(flpMonth);
                        flpDatePicker.Controls.Add(flpYear);

                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in rearranging calender fields in calender screen: " + ex.Message);
            }
        }
        private void frmCalender_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void ProcessNewlyPickedCalenderItem(usrCtrlCalender.CalenderElement element, string value)
        {
            log.LogMethodEntry(value);
            ResetKioskTimer();
            try
            {
                flpDatePicker.Visible = true;
                switch (element)
                {
                    case usrCtrlCalender.CalenderElement.DAY:
                        day = string.IsNullOrWhiteSpace(value) ? 1 : Convert.ToInt32(value);
                        lblDay.Text = day.ToString();
                        break;

                    case usrCtrlCalender.CalenderElement.MONTH:
                        string monthName = value;
                        month = DateTime.ParseExact(monthName, "MMM", CultureInfo.CurrentCulture).Month;
                        lblMonth.Text = monthName;
                        break;

                    case usrCtrlCalender.CalenderElement.YEAR:
                        year = string.IsNullOrWhiteSpace(value) ? ServerDateTime.Now.Year : Convert.ToInt32(value);
                        lblYear.Text = year.ToString();
                        break;

                    case usrCtrlCalender.CalenderElement.HOUR:
                        hour = string.IsNullOrWhiteSpace(value) ? ServerDateTime.Now.Hour : Convert.ToInt32(value);
                        lblHour.Text = hour.ToString("00");
                        break;

                    case usrCtrlCalender.CalenderElement.MINUTE:
                        minute = string.IsNullOrWhiteSpace(value) ? ServerDateTime.Now.Minute : Convert.ToInt32(value);
                        lblMinute.Text = minute.ToString("00");
                        break;

                    case usrCtrlCalender.CalenderElement.AM_PM:
                        if (ampm.Equals(value) == false)
                        {
                            if (ampm.Equals("AM") && value.Equals("PM"))
                            {
                                hour = hour + 12;
                            }
                            else if (ampm.Equals("PM") && value.Equals("AM"))
                            {
                                hour = hour - 12;
                            }
                        }
                        ampm = value;
                        lblAMPM.Text = ampm.ToString();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in ValueSelected() of frmCalender: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetDatePickerVisibility(bool flpDate, bool year, bool month, bool day)
        {
            log.LogMethodEntry(flpDate, year, month, day);

            try
            {
                ResetKioskTimer();

                flpDatePicker.Visible = flpDate;
                flpUsrCtrlYear.Visible = (flpDate == true) ? year : false;
                flpUsrCtrlMonth.Visible = (flpDate == true) ? month : false;
                flpUsrCtrlDay.Visible = (flpDate == true) ? day : false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in SetDatePickerVisibility() of frmCalender: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetTimePickerVisibility(bool flpTime, bool hour, bool minute, bool ampm)
        {
            log.LogMethodEntry(flpTime, hour, minute, ampm);

            try
            {
                ResetKioskTimer();

                flpTimePicker.Visible = flpTime;
                flpUsrCtrlHour.Visible = (flpTime == true) ? hour : false;
                flpUsrCtrlMinute.Visible = (flpTime == true) ? minute : false;
                flpUsrCtrlAMPM.Visible = (flpTime == true) ? ampm : false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in SetTimePickerVisibility() of frmCalender: " + ex.Message);
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
                this.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.CalenderBackgroundImage);
                this.btnPrev.BackgroundImage = btnSave.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.CalenderBtnBackgroundImage);
            }
            catch (Exception ex)
            {
                string msg = "Error while Setting Customized background images for Calender screen: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                this.btnSave.ForeColor = KioskStatic.CurrentTheme.CalenderBtnSaveTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CalenderBtnCancelTextForeColor;
                lblDay.ForeColor =
                    lblMonth.ForeColor =
                    lblYear.ForeColor =
                    lblHour.ForeColor =
                    lblMinute.ForeColor =
                    lblAMPM.ForeColor =
                    lblDelimeter.ForeColor = KioskStatic.CurrentTheme.CalenderItemTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors for Calender screen", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements for Calender screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmCalender_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            log.LogMethodExit();
        }
    }
}
