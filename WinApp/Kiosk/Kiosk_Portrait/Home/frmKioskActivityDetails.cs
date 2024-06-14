/********************************************************************************************
* Project Name - Parafait_Kiosk -frmKioskActivityDetails.cs
* Description  - frmKioskActivityDetails.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        06-Sep-2019      Deeksha            Added logger methods.
 *2.120        17-Apr-2021      Guru S A           Wristband printing flow enhancements
 *2.130.0      09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
 *2.150.0.0    21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
 *2.150.0.0    13-Oct-2022      Sathyavathi          Mask card number
 *2.150.1      22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.User;

// This form is created on 25-06-2015 to display the report of kiosk activity.
// this report is same as the option which is there in run report-> kiosk activity details report
namespace Parafait_Kiosk
{
    public partial class KioskActivityDetails : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = KioskStatic.Utilities;
        public List<int> listTrxId = new List<int>();
        int scrollIndex = 0;
        private bool IsMangerLogin = false;
        ExecutionContext executionContext = null;
        private const string KIOSK_REFUND_ACTIVITY_DESCRIPTION = "KIOSK Refund";

        public KioskActivityDetails(ExecutionContext machineExecutionContext, bool managerCardFlag = false)
        {
            log.LogMethodEntry(machineExecutionContext,managerCardFlag);
            executionContext = machineExecutionContext;
            InitializeComponent();
            this.IsMangerLogin = managerCardFlag;
            KioskStatic.setDefaultFont(this);

            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.KioskActivityDetailsBackgroundImage);//Starts:Modification on 17-Dec-2015 for introducing new theme
                panelKioskActivity.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                if (KioskStatic.CurrentTheme.TextForeColor != Color.White)
                    dgvKioskActivity.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                else
                    dgvKioskActivity.ForeColor = Color.Black;

                dgvKioskActivity.ColumnHeadersDefaultCellStyle.Font = new Font(lblGreeting.Font.FontFamily, 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //Ends:Modification on 17-Dec-2015 for introducing new theme
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            clearFields();
            fetchTrxDetails();
            if (string.IsNullOrEmpty(KioskStatic.showActivityDuration) || KioskStatic.showActivityDuration.Equals("0"))//checking for the value if it is zero then setting 15 as default value.
            {
                KioskStatic.showActivityDuration = "15";// By setting 15 this report will display the report of past 15 min activity
            }
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            lblGreeting.Text = KioskStatic.Utilities.MessageUtils.getMessage(lblGreeting.Text);
            txtMessage.Text = lblGreeting.Text;
            SetCustomizedFontColors();
            lblGreeting.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
            DisplaybtnPrev(true);
            DisplaybtnHome(false);
            Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void clearFields()
        {
            //Clearing the datagrid view
            log.LogMethodEntry();
            dgvKioskActivity.DataSource = null;
            log.LogMethodExit();
        }

        private void fetchTrxDetails()
        {
            log.LogMethodEntry();
            try
            {
                //Fetching the data and assigning to the datagridview.  

                SqlDataAdapter da;
                SqlCommand kioskActivityCmd = Utilities.getCommand();
                kioskActivityCmd.CommandText = "SELECT TrxId,TimeStamp as Date,Activity ," +
                                        "(select isnull((select top 1 isNull(CreditCardNumber, '')" +
                                        "from TrxPayments t left outer join PaymentModes pm on t.PaymentModeId = pm.PaymentModeId" +
                                        " where pm.isCreditCard = 'Y' and t.TrxId = kal.TrxId), '')) as 'Credit Card Number',CardNumber as 'Card Number',case notecoinflag when 'N' then 'Note' when 'C' then 'Coin' else '' end 'Note/ Coin?',Value as Amount,Message,KioskTrxId, kioskname as 'Kiosk Name'" +
                               " from dbo.KioskActivityLog kal" +
                               " where KioskName='" + Utilities.ParafaitEnv.POSMachine + "'  and TimeStamp >= @date order by TimeStamp desc";
                kioskActivityCmd.Parameters.AddWithValue("@date", ServerDateTime.Now.Hour < 6 ? ServerDateTime.Now.AddDays(-1).Date.AddHours(6) : ServerDateTime.Now.Date.AddHours(6));
                DataTable kioskActivityTbl = new DataTable();
                da = new SqlDataAdapter(kioskActivityCmd);
                da.Fill(kioskActivityTbl);
                //if (dgvKioskActivity.Rows != null && dgvKioskActivity.Rows.Count > 0)
                //{
                //    dgvKioskActivity.Rows.Clear();
                //}
                dgvKioskActivity.DataSource = kioskActivityTbl;
                Utilities.setLanguage(dgvKioskActivity);
                dgvKioskActivity.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
                dgvKioskActivity.Columns["Amount"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                dgvKioskActivity.Columns["Note/ Coin?"].Width = 0;
                dgvKioskActivity.Refresh();
                kioskActivityCmd.Connection.Close();
                if (dgvKioskActivity.SelectedCells.Count > 0)
                    dgvKioskActivity.SelectedCells[0].Selected = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
            // btnPrev.BackgroundImage = Properties.Resources.back_btn;
        }

        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
            // btnPrev.BackgroundImage = Properties.Resources.back_btn_pressed;
        }

        private void vScrollBarGp_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if (dgvKioskActivity.Rows.Count > 0)
                    dgvKioskActivity.FirstDisplayedScrollingRowIndex = e.NewValue;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void LeftButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void RightButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void KioskActivityDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;
            KioskTimerSwitch(true);
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void KioskActivityDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if ((Keys)e.KeyChar == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void dgvKioskActivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtMessage.Text = lblGreeting.Text;
                ResetKioskTimer();
                if (e.ColumnIndex < 0)
                {
                    log.LogMethodExit(null);
                    log.LogMethodExit();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frfmKioskActivityDetails");
            try
            {
                foreach (Control c in dgvKioskActivity.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityTextForeColor;
                }
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.KioskActivityHeaderTextForeColor;
                this.lblActivity.ForeColor = KioskStatic.CurrentTheme.KioskActivityDetailsHeaderTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.KioskActivityBtnPrevTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.KioskActivityTxtMessageTextForeColor;
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.bigHorizontalScrollKioskActivity.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollLeftEnabled, ThemeManager.CurrentThemeImages.ScrollLeftDisabled, ThemeManager.CurrentThemeImages.ScrollRightEnabled, ThemeManager.CurrentThemeImages.ScrollRightDisabled);
                this.bigVerticalScrollKioskActivity.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.dgvKioskActivity.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityHeaderTextForeColor;
                this.dgvKioskActivity.ForeColor = KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityInfoTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frfmKioskActivityDetails: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvKioskActivity_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvKioskActivity.Columns[e.ColumnIndex].Name == "Card Number")
                {
                    if (e.Value != null)
                    {
                        e.Value = KioskHelper.GetMaskedCardNumber(e.Value.ToString());
                        //e.FormattingApplied = true;
                    }
                }
                if (dgvKioskActivity.Columns[e.ColumnIndex].Name == "TrxId")
                {
                    if (e.Value != null && e.Value.ToString() == "-1")
                    {
                        e.Value = null;
                        //e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while dgvMemberDetails_CellFormatting in adult select screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void KioskActivityDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("KioskActivityDetails_FormClosed()");
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
