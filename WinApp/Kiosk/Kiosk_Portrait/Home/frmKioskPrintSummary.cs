/********************************************************************************************
* Project Name - Parafait_Kiosk -frmKioskPrintSummary.cs
* Description  - Allows admin to generate the transaction summary report and print/email the report.
* 
**************
**Version Log
**************
*Version       Date             Modified By        Remarks          
*********************************************************************************************
 *2.150.3.0   26-Apr-2023      Sathyavathi        Created
 *2.155.0     10-Jun-2023      Sathyavathi        Attraction Sale in Kiosk - Calendar changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Communication;
using System.Globalization;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
using System.Security.Principal;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;
using Semnox.Parafait.CommonUI;

namespace Parafait_Kiosk
{
    public partial class frmKioskPrintSummary : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = KioskStatic.Utilities;
        private int businessStartHour = 6;
        private const int MAXIMUM_DATE_RANGE_ALLOWED = 45;
        private DateTime fromDateTime;
        private DateTime toDateTime;
        private string dateTimeFormat;
        private string formattedReceipt;
        private bool isDoIncrementalKioskPrint;
        private string staffEmailId;
        private bool inNewTab = true;
        private string staffCardNumber;

        public frmKioskPrintSummary(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            try
            {
                staffCardNumber = cardNumber;
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.KioskPrintSummaryBackgroundImage);
                btnPrev.BackgroundImage =
                    btnPrint.BackgroundImage =
                    btnEmail.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                pBFromCalender.BackgroundImage = pBToCalender.BackgroundImage = ThemeManager.CurrentThemeImages.CalenderIconImage;

                businessStartHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 6);
                KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
                lblGreeting.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
                isDoIncrementalKioskPrint = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "DO_INCREMENTAL_KIOSK_PRINT_SUMMARY", false);
                SetDefaultText();
                dateTimeFormat = ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "DATETIME_FORMAT", "dd - MMM - yyyy h: mm tt");
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void KioskPrintSummary_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                btnNewTab.Visible = btnRePrintTab.Visible = isDoIncrementalKioskPrint;
                pnlReprint.Visible = false;
                btnRePrintTab.ForeColor = SystemColors.InactiveCaption;
                SetDefaultDateAndTime();
                staffEmailId = GetUserEmailId();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error loading KioskPrintSummary_Load: " + ex.Message);
            }
            finally
            {
                KioskTimerSwitch(true);
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void FromDateTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                //for Do Incremental Kiosk Print do not enable editing of dates as it is intended to pick last printed date and print the summary.
                if (isDoIncrementalKioskPrint == true && inNewTab == true)
                    return;

                //default date and time is current day 6AM
                fromDateTime = KioskHelper.LaunchCalendar(defaultDateTimeToShow: fromDateTime, enableDaySelection: true, enableMonthSelection: true, 
                    enableYearSelection: true, disableTill: DateTime.MinValue, showTimePicker: true, popupAlerts: frmOKMsg.ShowUserMessage);
                lblFromDateTime.Text = fromDateTime.ToString(dateTimeFormat);

                if (isDoIncrementalKioskPrint)
                {
                    cmbReprintDateRange.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: error handling Calender event in frmKioskPrintSummary" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void ToDateTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                //for Do Incremental Kiosk Print do not enable editing of dates as it is intended to pick last printed date and print the summary.
                if (isDoIncrementalKioskPrint == true && inNewTab == true)
                    return;

                //default date and time is current day and current time
                toDateTime = KioskHelper.LaunchCalendar(defaultDateTimeToShow: toDateTime, enableDaySelection: true, enableMonthSelection: true, 
                    enableYearSelection: true, disableTill: DateTime.MinValue, showTimePicker: true, popupAlerts: frmOKMsg.ShowUserMessage);
                lblToDateTime.Text = toDateTime.ToString(dateTimeFormat);
                if (isDoIncrementalKioskPrint)
                {
                    cmbReprintDateRange.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Error handling Calender event in frmKioskPrintSummary" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                if (ValidateDateRange())
                {
                    if (PreviewSummary() == true)
                    {
                        PrintSummary(fromDateTime, toDateTime);
                        txtMessage.Text = string.Empty;
                    }
                }
                else
                {
                    this.pBPrintSummaryImage.BackgroundImage = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Error handling Print Event in frmKioskPrintSummary" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                if (ValidateDateRange())
                {
                    string screenMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5101);
                    string fieldName = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "EMAIL Address");
                    string emailID = staffEmailId;
                    using (frmTextBox frmText = new frmTextBox(utilities.ExecutionContext, screenMsg, staffEmailId, fieldName, true)) //Confirm Email address
                    {
                        DialogResult dr = frmText.ShowDialog();
                        if (dr == DialogResult.Yes)
                        {
                            emailID = frmText.TextBoxData;
                            KioskStatic.logToFile("Email Address provided for the Kiosk Print Summary: " + emailID);
                            if (ValidateEmailAddress(emailID) == false)// Validates domain name, size like .com ,.comm .ukcom etc
                            {
                                string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 572);
                                txtMessage.Text = errMsg;
                                ValidationException validationException = new ValidationException(errMsg);
                                throw validationException;
                            }
                            if (PreviewSummary() == true)
                            {
                                List<KeyValuePair<Semnox.Parafait.Transaction.Transaction, string>> trxImageList = new List<KeyValuePair<Semnox.Parafait.Transaction.Transaction, string>>();
                                List<string> textList = new List<string>();
                                textList = formattedReceipt.Split('\n').ToList();
                                string base64String = KioskHelper.ConvertTextToBase64String(textList, 655, "Arial", 15.75F);
                                KioskHelper.SendPrintSummaryEmail(KioskStatic.Utilities.ExecutionContext, emailID, base64String);

                                if (isDoIncrementalKioskPrint == true && inNewTab == false)
                                {
                                    KioskStatic.UpdateKioskActivityLog(KioskStatic.Utilities.ExecutionContext, "KIOSK_SUMMARY_REPRINT_E", fromDateTime.ToString() + "," + toDateTime.ToString());
                                }
                                else
                                {
                                    KioskStatic.UpdateKioskActivityLog(KioskStatic.Utilities.ExecutionContext, "KIOSK_SUMMARY_PRINT_E", fromDateTime.ToString() + "," + toDateTime.ToString());
                                }
                                string successMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 333); //Email sent successfully
                                txtMessage.Text = successMsg;
                                frmOKMsg.ShowUserMessage(successMsg);
                            }
                        }
                    }
                }
                else
                {
                    this.pBPrintSummaryImage.BackgroundImage = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR in btnEmail_Click() of frmKioskPrintSummary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnNewTab_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                inNewTab = true;
                this.pBPrintSummaryImage.Size = new System.Drawing.Size(1026, 810);
                pnlReprint.Visible = false;
                pBPrintSummaryImage.BackgroundImage = null;
                btnNewTab.ForeColor = KioskStatic.CurrentTheme.PrintSummaryNewTabTextForeColor;
                btnRePrintTab.ForeColor = SystemColors.InactiveCaption;
                lblTabMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5095); //Click on the Print Or E-mail button to generate the summary report.

                if (isDoIncrementalKioskPrint)
                {
                    btnRePrintTab.Enabled = true;
                }
                ResetFromAndToDateTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR in btnPrintSummary_Click() of frmKioskPrintSummary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void ResetFromAndToDateTime()
        {
            log.LogMethodEntry();
            if (inNewTab)
            {
                SetDefaultDateAndTime();
            }
            log.LogMethodExit();
        }

        private void btnRePrintTab_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                inNewTab = false;
                this.pBPrintSummaryImage.Size = new System.Drawing.Size(1026, 660);
                pnlReprint.Visible = lblTabMsg.Visible = true;
                pBPrintSummaryImage.BackgroundImage = null;
                btnNewTab.ForeColor = SystemColors.InactiveCaption;
                btnRePrintTab.ForeColor = KioskStatic.CurrentTheme.PrintSummaryRePrintTabTextForeColor;
                lblTabMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5098); //Edit From and To dates if you wish to generate report for specific period.

                LoadPickDropdownList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR in btnRePrintSummary_Click() of frmKioskPrintSummary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadPickDropdownList()
        {
            log.LogMethodEntry();
            List<string> printedDateRange = new List<string>();
            printedDateRange.Insert(0, string.Empty);
            cmbReprintDateRange.DataSource = printedDateRange;
            cmbReprintDateRange.SelectedItem = printedDateRange[0]; //show empty when there is no data in KioskActivityLog
            DataTable dt = KioskStatic.Utilities.executeDataTable(@"select top 45 Message 
                                                                        from KioskActivityLog 
                                                                        where Activity in ( 'KIOSK_SUMMARY_PRINT', 'KIOSK_SUMMARY_PRINT_E')
                                                                        and KioskName = @pos
                                                                        order by 1 desc",
                                                new SqlParameter("@pos", KioskStatic.Utilities.ExecutionContext.POSMachineName));//,
            if (dt != null && dt.Rows.Count > 0)
            {
                dt = dt.DefaultView.ToTable(true, new String[] { "Message" });
                foreach (DataRow row in dt.Rows)
                {
                    string[] subs = row["Message"].ToString().Split(',');
                    subs[0] = "[" + Convert.ToDateTime(subs[0]).ToString(dateTimeFormat) + "]";
                    subs[1] = "[" + Convert.ToDateTime(subs[1]).ToString(dateTimeFormat) + "]";
                    string msg = subs[0] + " - " + subs[1];

                    printedDateRange.Add(msg);
                }
                cmbReprintDateRange.DataSource = null;
                cmbReprintDateRange.DataSource = printedDateRange;
                cmbReprintDateRange.SelectedItem = printedDateRange[1]; //item starts from index 1. 0 has empty item.
            }
            log.LogMethodExit();
        }

        private void cmbReprintDateRange_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                cmbReprintDateRange.SelectedItem = cmbReprintDateRange.SelectedValue.ToString();

                //extract from and to date from the combo text. Ex: [24-Apr-2023 6:00:00 AM] - [24-Apr-2023 4:05:41 PM]
                var pattern = @"\[(.*?)\]";
                var query = cmbReprintDateRange.SelectedItem;
                var matches = Regex.Matches(query.ToString(), pattern);

                fromDateTime = Convert.ToDateTime(matches[0].ToString().Trim(new Char[] { '[', ']' }));
                lblFromDateTime.Text = fromDateTime.ToString(dateTimeFormat);
                toDateTime = Convert.ToDateTime(matches[1].ToString().Trim(new Char[] { '[', ']' }));
                lblToDateTime.Text = toDateTime.ToString(dateTimeFormat);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR in cmbReprintDateRange_SelectedValueChanged of frmKioskPrintSummary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private bool ValidateEmailAddress(string email)
        {
            log.LogMethodEntry(email);
            ResetKioskTimer();

            bool status = false;
            if (!string.IsNullOrEmpty(email.Trim()))
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(email.Trim(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$") == true)
                {
                    status = true;
                }
            }
            log.LogMethodExit(status);
            return status;
        }

        private void SetDefaultText()
        {
            log.LogMethodEntry();

            lblSiteName.Text = KioskStatic.SiteHeading;
            lblGreeting.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Kiosk Activity Summary");
            lblFromText.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "From :");
            lblToText.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "To :");
            lblPickText.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Pick :");
            txtMessage.Text = lblGreeting.Text;
            btnPrev.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Back");
            btnPrint.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Print");
            btnRePrintTab.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Reprint Report");
            btnNewTab.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "New Report");
            btnEmail.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Email");
            //lblMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5095); //Click on the Print Or E-mail button to generate the summary report.
            lblTabMsg.Text = (isDoIncrementalKioskPrint) ?
                MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5095) //Edit From and To dates if you wish to generate report for specific period.
                : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5095) + " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5096); //Click on the Print Or E-mail button to generate the summary report. Edit From and To dates if you wish to generate report for specific period.
            log.LogMethodExit();
        }

        private void SetDefaultDateAndTime()
        {
            log.LogMethodEntry();
            SetDefaultFromDate();
            SetDefaultToDate();
            log.LogMethodExit();
        }

        private void SetDefaultFromDate()
        {
            log.LogMethodEntry();

            try
            {
                if (isDoIncrementalKioskPrint)
                {
                    //Picks last printed To date and time. 
                    DataTable dt = KioskStatic.Utilities.executeDataTable(@"select top 45 TimeStamp, Activity, Message 
                                                                        from KioskActivityLog 
                                                                        where Activity in ( 'KIOSK_SUMMARY_PRINT', 'KIOSK_SUMMARY_PRINT_E')
                                                                        and KioskName = @pos
                                                                        order by 1 desc",
                                                                        new SqlParameter("@pos", KioskStatic.Utilities.ExecutionContext.POSMachineName));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string msg = dt.Rows[0]["Message"].ToString();
                        string date = msg.Substring(msg.IndexOf(',') + 1);
                        DateTime lastPrintedDateTime = Convert.ToDateTime(date);
                        //adding 1 minute to the last printed time.
                        fromDateTime = new DateTime(lastPrintedDateTime.Year, lastPrintedDateTime.Month, lastPrintedDateTime.Day, lastPrintedDateTime.Hour, (lastPrintedDateTime.AddMinutes(1)).Minute, 0);
                    }
                    else
                    {
                        fromDateTime = new DateTime(ServerDateTime.Now.Year, ServerDateTime.Now.Month, ServerDateTime.Now.Day, businessStartHour, 0, 0);
                    }
                }
                else
                {
                    fromDateTime = new DateTime(ServerDateTime.Now.Year, ServerDateTime.Now.Month, ServerDateTime.Now.Day, businessStartHour, 0, 0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                fromDateTime = new DateTime(ServerDateTime.Now.Year, ServerDateTime.Now.Month, ServerDateTime.Now.Day, businessStartHour, 0, 0);
                KioskStatic.logToFile("ERROR: frmKioskPrintSummary - Unhandled exception in SetDefaultFromDate()");
            }
            lblFromDateTime.Text = fromDateTime.ToString(dateTimeFormat);
            log.LogMethodExit();
        }

        private void SetDefaultToDate()
        {
            log.LogMethodEntry();
            toDateTime = ServerDateTime.Now;
            lblToDateTime.Text = toDateTime.ToString(dateTimeFormat);
            log.LogMethodExit();
        }

        private bool ValidateDateRange()
        {
            log.LogMethodEntry();
            bool status = false;
            try
            {
                if (fromDateTime > toDateTime)
                {
                    //To Date should be greater than From Date
                    string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 724);
                    ValidationException validationException = new ValidationException(errMsg);
                    throw validationException;
                }

                if (Math.Abs(fromDateTime.Subtract(toDateTime).TotalDays) > MAXIMUM_DATE_RANGE_ALLOWED == true)
                {
                    //Sorry, unable to generate the data past 45 days.
                    string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5090);
                    ValidationException validationException = new ValidationException(errMsg);
                    throw validationException;
                }
                status = true;
                txtMessage.Text = string.Empty;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("ERROR generating Kiosk Summary Print: " + ex.Message
                    + Environment.NewLine + "Date Range: " + fromDateTime + "to" + toDateTime);
            }
            log.LogMethodExit(status);
            return status;
        }

        private bool PreviewSummary()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            bool status = false;
            try
            {
                if (GetPrintData() == true)
                {
                    List<KeyValuePair<Semnox.Parafait.Transaction.Transaction, string>> trxImageList = new List<KeyValuePair<Semnox.Parafait.Transaction.Transaction, string>>();
                    List<string> textList = new List<string>();
                    textList = formattedReceipt.Split('\n').ToList();
                    string base64String = KioskHelper.ConvertTextToBase64String(textList, 655, "Arial", 15.75F);
                    trxImageList.Add(new KeyValuePair<Semnox.Parafait.Transaction.Transaction, string>(null, base64String));

                    if (trxImageList != null && trxImageList.Any())
                    {
                        Image image = GenericUtils.ConvertBase64StringToImage(trxImageList[0].Value);
                        this.pBPrintSummaryImage.BackgroundImageLayout = ImageLayout.Center;
                        this.pBPrintSummaryImage.BackgroundImage = image;
                    }
                    status = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in LoadImage. Error: " + ex.Message);
            }
            log.LogMethodExit(status);
            return status;
        }

        private bool GetPrintData()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            bool status = false;
            try
            {
                KioskStatic.logToFile("Print Summary pressed");

                DataTable dt = KioskStatic.Utilities.executeDataTable(@"select 
                                                                        isnull(sum(Amount * CashRatio), 0) Cash, 
                                                                        isnull(sum(Amount * CreditCardRatio), 0) CreditCard,
                                                                        isnull(sum(Amount * GameCardRatio), 0) GameCard, 
                                                                        --isnull(aborted.aborted, 0) Aborted,
                                                                        --isnull(sum(Amount), 0) + isnull(aborted.aborted, 0) Total,
                                                                        isnull(sum(Amount), 0) Total,
                                                                        count(distinct trxId) [TotalOps],
                                                                        count(distinct new_card_id) [NewCards]
                                                                      from TransactionView v
                                                                      where trxdate >= @fromDateTime and trxdate <= @toDateTime
                                                                      and pos_machine = @pos
            								  --group by aborted.aborted",
                                                                  new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine),
                                                                  new SqlParameter("@fromDateTime", fromDateTime),
                                                                  new SqlParameter("@toDateTime", toDateTime));

                DataTable dtAborted = KioskStatic.Utilities.executeDataTable(@"select Activity, sum(value) aborted
                                                                                     from KioskActivityLog 
                                                                                    where Activity in ( 'ABORT' ,'ABORT_CC', 'ABORT_GAMECARD') 
                                                                                    and TimeStamp >= @fromDateTime and TimeStamp <= @toDateTime
                                                                                      and KioskName = @pos
                                                                                    group by Activity",
                                                                     new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine),
                                                                     new SqlParameter("@fromDateTime", fromDateTime),
                                                                     new SqlParameter("@toDateTime", toDateTime));

                DataTable dtMoney = KioskStatic.Utilities.executeDataTable(@"select NoteCoinFlag, Value, sum(value) Total, count(value) Quantity 
                                                                             from kioskactivitylog
                                                                             where NoteCoinFlag is not null
                                                                             and TimeStamp >= @fromDateTime and TimeStamp <= @toDateTime
                                                                               and KioskName = @pos
                                                                               group by NoteCoinFlag, value
                                                                               order by NoteCoinFlag, value",
                                                                      new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine),
                                                                      new SqlParameter("@fromDateTime", fromDateTime),
                                                                      new SqlParameter("@toDateTime", toDateTime));

                DataTable dtRefund = KioskStatic.Utilities.executeDataTable(@" select ISNULL(sum(value),0) Refunded
                                                                                    from KioskActivityLog 
                                                                                    where Activity = 'REFUND'
                                                                                    and TimeStamp >= @fromDateTime and TimeStamp <= @toDateTime
                                                                                    and KioskName = @pos ",
                                                                    new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine),
                                                                    new SqlParameter("@fromDateTime", fromDateTime),
                                                                    new SqlParameter("@toDateTime", toDateTime));

                formattedReceipt = string.Empty;
                formattedReceipt = "*---KIOSK Activity Summary---*" + Environment.NewLine;
                formattedReceipt += KioskStatic.Utilities.ParafaitEnv.SiteName + Environment.NewLine + Environment.NewLine;
                formattedReceipt += "From Date: " + fromDateTime.ToString(KioskStatic.Utilities.ParafaitEnv.DATETIME_FORMAT) + Environment.NewLine;
                formattedReceipt += "To Date: " + toDateTime.ToString(KioskStatic.Utilities.ParafaitEnv.DATETIME_FORMAT) + Environment.NewLine;
                formattedReceipt += "Kiosk: " + KioskStatic.Utilities.ParafaitEnv.POSMachine + Environment.NewLine + Environment.NewLine;

                decimal refundedAmount = 0;
                if (dtRefund.Rows.Count > 0)
                {
                    refundedAmount = Convert.ToDecimal(dtRefund.Rows[0]["Refunded"]);
                }

                decimal abortedCash = 0;
                decimal abortedCreditCard = 0;
                decimal abortedGameCard = 0;
                if (dtAborted.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAborted.Rows.Count; i++)
                    {
                        if (dtAborted.Rows[i]["Activity"].ToString() == KioskTransaction.GETABORTCASH.ToString())
                        {
                            abortedCash = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                        else if (dtAborted.Rows[i]["Activity"].ToString() == KioskTransaction.GETABORTCREDITCARD.ToString())
                        {
                            abortedCreditCard = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                        else if (dtAborted.Rows[i]["Activity"].ToString() == KioskTransaction.GETABORTGAMECARD.ToString())
                        {
                            abortedGameCard = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                    }
                }
                decimal cashAmount = 0;
                decimal cardAmount = 0;
                decimal gameCardAmount = 0;
                decimal totalTrxAmount = 0;
                decimal totalOps = 0;
                decimal newCards = 0;
                if (dt.Rows.Count > 0)
                {
                    cashAmount = Convert.ToDecimal(dt.Rows[0]["Cash"]);
                    cardAmount = Convert.ToDecimal(dt.Rows[0]["CreditCard"]);
                    gameCardAmount = Convert.ToDecimal(dt.Rows[0]["GameCard"]);
                    totalTrxAmount = Convert.ToDecimal(dt.Rows[0]["Total"]);
                    totalOps = Convert.ToDecimal(dt.Rows[0]["TotalOps"]);
                    newCards = Convert.ToDecimal(dt.Rows[0]["NewCards"]);
                }
                formattedReceipt += "Cash: " + cashAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                formattedReceipt += "Credit Card: " + cardAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                if (gameCardAmount > 0)
                {
                    formattedReceipt += "Game Card: " + gameCardAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                }

                formattedReceipt += "Aborted Cash: " + abortedCash.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                formattedReceipt += "Aborted Credit: " + abortedCreditCard.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                if (abortedGameCard > 0)
                {
                    formattedReceipt += "Aborted Game Card: " + abortedGameCard.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                }

                formattedReceipt += "Refunded Credit: " + refundedAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;

                formattedReceipt += "Total Trx: " + totalTrxAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine + Environment.NewLine;
                formattedReceipt += "#Ops: " + totalOps.ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT) + Environment.NewLine;
                formattedReceipt += "#New Cards: " + newCards.ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT) + Environment.NewLine + Environment.NewLine;


                decimal insertedTotal = 0;
                foreach (DataRow dr in dtMoney.Rows)
                {
                    if (dr["NoteCoinFlag"].ToString().Equals("T"))
                    {
                        formattedReceipt += ("  " + "Token").PadRight(12) + " #" + dr["Quantity"].ToString() + Environment.NewLine;
                    }
                    else
                    {
                        insertedTotal += Convert.ToDecimal(dr["Total"]);
                        formattedReceipt += (KioskStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL + " " + Convert.ToDouble(dr["Value"]).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + " " + (dr["NoteCoinFlag"].ToString().Equals("N") ? "Bill" : "Coin")).PadRight(12) + " #" + dr["Quantity"].ToString() + ", " + Convert.ToDecimal(dr["Total"]).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                    }
                }
                formattedReceipt += "Total Inserted: " + insertedTotal.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;

                formattedReceipt += Environment.NewLine;
                formattedReceipt += "-".PadRight(10, '-').PadLeft(20, ' ');
                log.LogVariableState("formattedReceipt", formattedReceipt);

                status = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error printing Kiosk Summary: " + ex.Message);
                using (frmOKMsg frm = new frmOKMsg(ex.Message, true))
                {
                    frm.ShowDialog();
                }
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit(true);
            return status;
        }

        private void PrintSummary(DateTime fromDateTime, DateTime toDateTime)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                KioskStatic.logToFile("Print Summary pressed");
                PrintDocument printDocument = new PrintDocument();
                PrinterDTO printerDTO = null;
                if (KioskStatic.POSMachineDTO == null || KioskStatic.POSMachineDTO.PosPrinterDtoList == null || KioskStatic.POSMachineDTO.PosPrinterDtoList.Any() == false)
                {
                    POSMachines posMachine = new POSMachines(KioskStatic.Utilities.ExecutionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId);
                    KioskStatic.POSMachineDTO.PosPrinterDtoList = posMachine.PopulatePrinterDetails();
                }
                List<POSPrinterDTO> POSPrintersDTOList = new List<POSPrinterDTO>(KioskStatic.POSMachineDTO.PosPrinterDtoList);
                if (POSPrintersDTOList != null && POSPrintersDTOList.Any() &&
                    POSPrintersDTOList.Exists(pp => pp.PrinterDTO != null
                                      && pp.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter))
                {
                    printerDTO = POSPrintersDTOList.Find(pp => pp.PrinterDTO != null
                                      && pp.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter).PrinterDTO;
                }
                else
                {
                    printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1, -1, 0);
                }
                log.LogVariableState("printerDTO", printerDTO);
                printDocument.PrinterSettings.PrinterName = String.IsNullOrWhiteSpace(printerDTO.PrinterLocation) ? printerDTO.PrinterName : printerDTO.PrinterLocation;
                KioskStatic.logToFile("Summary Print assigned to Printer: " + printDocument.PrinterSettings.PrinterName);
                log.LogVariableState("Printer: ", printDocument.PrinterSettings.PrinterName);
                PrinterBL printerBL = new PrinterBL(KioskStatic.Utilities.ExecutionContext, printerDTO);
                PrinterBuildBL printerBuildBL = new PrinterBuildBL(KioskStatic.Utilities.ExecutionContext);

                if (printerBuildBL.SetUpPrinting(printDocument, false, "", printerDTO))
                {
                    printDocument.PrintPage += (s, args) =>
                    {
                        args.Graphics.DrawString(formattedReceipt, new System.Drawing.Font("Courier New", 9, FontStyle.Bold), System.Drawing.Brushes.Black, 12, 20);
                    };
                    using (WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero))
                    {
                        //code to send print document to the printer
                        printDocument.Print();
                    }
                }

                if (isDoIncrementalKioskPrint == true && inNewTab == false)
                {
                    KioskStatic.UpdateKioskActivityLog(KioskStatic.Utilities.ExecutionContext, "KIOSK_SUMMARY_REPRINT", fromDateTime.ToString() + "," + toDateTime.ToString());

                }
                else
                {
                    KioskStatic.UpdateKioskActivityLog(KioskStatic.Utilities.ExecutionContext, "KIOSK_SUMMARY_PRINT", fromDateTime.ToString() + "," + toDateTime.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error printing Kiosk Summary: " + ex.Message);
                using (frmOKMsg frm = new frmOKMsg(ex.Message, true))
                {
                    frm.ShowDialog();
                }
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        public string GetUserEmailId()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            string email = string.Empty;
            try
            {
                UserIdentificationTagsDTO userIdTagDTO = null;
                UserIdentificationTagListBL userIdTagsList = new UserIdentificationTagListBL();
                List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> userIdTagSearchParams = new List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>>();

                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ACTIVE_FLAG, "1"));
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.CARD_NUMBER, staffCardNumber));
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.SITE_ID, KioskStatic.Utilities.ExecutionContext.SiteId.ToString()));

                List<UserIdentificationTagsDTO> userIdTagList = userIdTagsList.GetUserIdentificationTagsDTOList(userIdTagSearchParams);
                if (userIdTagList != null && userIdTagList.Count > 0)
                {
                    userIdTagDTO = userIdTagList[0];
                    Users uBL = new Users(KioskStatic.Utilities.ExecutionContext, userIdTagDTO.UserId, true);
                    email = uBL.UserDTO.Email;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(email);
            return email;
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.PrintSummarySiteTextForeColor;
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.PrintSummaryLblGreetingTextForeColor;
                this.lblPickText.ForeColor = this.lblFromText.ForeColor = KioskStatic.CurrentTheme.PrintSummaryLblFromDateTextForeColor;
                this.lblFromDateTime.ForeColor = KioskStatic.CurrentTheme.PrintSummaryLblFromDateTimeForeColor;
                this.lblToText.ForeColor = KioskStatic.CurrentTheme.PrintSummaryLblToDateTextForeColor;
                this.lblToDateTime.ForeColor = KioskStatic.CurrentTheme.PrintSummaryLblToDateTimeForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.PrintSummaryBtnPrevTextForeColor;
                this.btnPrint.ForeColor = KioskStatic.CurrentTheme.PrintSummaryBtnPrintTextForeColor;
                this.btnEmail.ForeColor = KioskStatic.CurrentTheme.PrintSummaryBtnEmailTextForeColor;
                this.btnNewTab.ForeColor = KioskStatic.CurrentTheme.PrintSummaryNewTabTextForeColor;
                this.btnRePrintTab.ForeColor = KioskStatic.CurrentTheme.PrintSummaryRePrintTabTextForeColor;
                this.cmbReprintDateRange.ForeColor = KioskStatic.CurrentTheme.PrintSummaryComboTextForeColor;
                this.lblTabMsg.ForeColor =
                    lblOR.ForeColor = KioskStatic.CurrentTheme.PrintSummaryLblTabTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.HomeScreenFooterTextForeColor;//Footer text message
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmKioskTransactionView: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining <= 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                {
                    this.Close();
                }
            }
            log.LogMethodExit();
        }
    }
}

