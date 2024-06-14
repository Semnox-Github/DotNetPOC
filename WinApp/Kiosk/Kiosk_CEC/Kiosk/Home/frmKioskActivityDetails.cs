/********************************************************************************************
 * Project Name - frmKioskActivityDeails
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 * 2.80         20-Sep-2019      Archana            Modified: Option to refund for Admin
 *2.80.1        02-Feb-2021      Deeksha            Theme changes to support customized Images/Font
 *2.100.0       05-Aug-2020      Guru S A           Kiosk activity log changes
 *2.120         17-Apr-2021      Guru S A           Wristband printing flow enhancements
 *2.130.0       30-Jun-2021      Dakshak            Theme changes to support customized Font ForeColor
 *2.150.0.0     13-Oct-2022      Sathyavathi        Mask card number
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
        Utilities Utilities = KioskStatic.Utilities;
        public List<int> listTrxId = new List<int>();
        int scrollIndex = 0;
        int maxScrollIndex = 0;
        int minScrollIndex = 0;
        private bool IsMangerLogin = false;
        ExecutionContext executionContext = null;
        private const string KIOSK_REFUND_ACTIVITY_DESCRIPTION = "KIOSK Refund";
        private Color dgvTextForeColor;
        public KioskActivityDetails(ExecutionContext machineExecutionContext, bool managerCardFlag = false)
        {
            log.LogMethodEntry(machineExecutionContext,managerCardFlag);
            executionContext = machineExecutionContext;
            Utilities.setLanguage();
            InitializeComponent();
            Utilities.setLanguage(this);
            this.IsMangerLogin = managerCardFlag;
            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);

            try
            {
                this.BackgroundImage = KioskStatic.CurrentTheme.DefaultBackgroundImage;// adding background image of  the current theem
                
            }
            catch { }
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            clearFields();
            //if (string.IsNullOrEmpty(KioskStatic.showActivityDuration) || KioskStatic.showActivityDuration.Equals("0"))//checking for the value if it is zero then setting 15 as default value.
            //{
            //    KioskStatic.showActivityDuration = "15";// By setting 15 this report will display the report of past 15 min activity
            //}

            lblGreeting.Text = KioskStatic.Utilities.MessageUtils.getMessage(lblGreeting.Text);
            txtMessage.Text = lblGreeting.Text;
            ShowOrHideRefundButton();
            SetCustomizedFontColors();

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
                kioskActivityCmd.CommandText = "SELECT kioskname as 'Kiosk Name',TimeStamp as Date,Activity ," +
                                                        "(select isnull((select top 1 isNull(CreditCardNumber, '')" +
                                                        "from TrxPayments t left outer join PaymentModes pm on t.PaymentModeId = pm.PaymentModeId" +
                                                        " where pm.isCreditCard = 'Y' and t.TrxId = kal.TrxId), '')) as 'Credit Card Number',CardNumber as 'Card Number',case notecoinflag when 'N' then 'Note' when 'C' then 'Coin' else '' end 'Note/ Coin?',Value as Amount,Message,KioskTrxId,TrxId" +
                                               " from dbo.KioskActivityLog kal" +
                                               " where KioskName='" + Utilities.ParafaitEnv.POSMachine + "'  and TimeStamp >= @date order by TimeStamp desc";
                kioskActivityCmd.Parameters.AddWithValue("@date", ServerDateTime.Now.Hour < 6 ? ServerDateTime.Now.AddDays(-1).Date.AddHours(6) : ServerDateTime.Now.Date.AddHours(6));
                DataTable kioskActivityTbl = new DataTable();
                da = new SqlDataAdapter(kioskActivityCmd);
                da.Fill(kioskActivityTbl);
                if (dgvKioskActivity.Rows != null && dgvKioskActivity.Rows.Count>0)
                {
                    dgvKioskActivity.Rows.Clear();
                }
                dgvKioskActivity.DataSource = kioskActivityTbl;
                Utilities.setLanguage(dgvKioskActivity);
                dgvKioskActivity.Columns["btnPrintTrx"].ReadOnly = false;
                dgvKioskActivity.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
                dgvKioskActivity.Columns["Amount"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                dgvKioskActivity.Columns["Note/ Coin?"].Width = 0;
                dgvKioskActivity.Refresh();
                kioskActivityCmd.Connection.Close();
                if (dgvKioskActivity.SelectedCells.Count > 0)
                    dgvKioskActivity.SelectedCells[0].Selected = false;
                //vScrollBarGp.Maximum = dgvKioskActivity.Rows.Count + 1;
                hScroll.Maximum = dgvKioskActivity.Columns.Count + 5;
                if (dgvKioskActivity.Rows.Count <= 10)
                {
                    buttonNext.Visible = false;
                }
                buttonPrev.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        //private void btnPrev_Click(object sender, EventArgs e)
        //{
        //    Close();
        //}
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
            if (dgvKioskActivity.Rows.Count > 0)
                dgvKioskActivity.FirstDisplayedScrollingRowIndex = e.NewValue;
            log.LogMethodExit();
        }

        private void KioskActivityDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;


            this.Invalidate();
            this.Refresh();
            KioskTimerSwitch(false);
            displaybtnCancel(false);
            RefreshScreen();
            log.LogMethodExit();
        }

        private void ShowOrHideRefundButton()
        {
            log.LogMethodEntry();
            if (IsMangerLogin == true && Utilities.getParafaitDefaults("ENABLE_REFUND_OPTION_FOR_ADMIN").Equals("Y"))
            {
                btnRefund.Visible = true;
                btnRefund.Enabled = false;
            }
            else
            {
                btnPrev.Location = new Point(btnPrev.Location.X + 130, btnPrev.Location.Y);
                btnPrint.Location = new Point(btnPrint.Location.X + 160, btnPrint.Location.Y);
                btnRefund.Visible = false;
            }
            log.LogMethodExit();
        }

        private void KioskActivityDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if ((Keys)e.KeyChar == Keys.Escape)
                this.Close();
            log.LogMethodExit();
        }

        private void hScroll_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvKioskActivity.Columns.Count > 0)
            {
                int hScrollIndex = e.NewValue;
                if ((e.NewValue - e.OldValue) >= 0 && e.NewValue >= 5 && hScroll.Maximum + 2 > e.NewValue)
                {
                    hScrollIndex = e.NewValue + 2;
                }
                dgvKioskActivity.FirstDisplayedScrollingColumnIndex = hScrollIndex;
            }
            log.LogMethodExit();
        }

        private void dgvKioskActivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            txtMessage.Text = lblGreeting.Text;
            if (e.ColumnIndex < 0)
            {
                log.LogMethodExit(null);
                log.LogMethodExit();
                return;
            }
            if (dgvKioskActivity.Columns[e.ColumnIndex].Name == "btnPrintTrx")
            {
                if (dgvKioskActivity.CurrentRow == null || dgvKioskActivity.CurrentRow.Cells["TrxId"].Value == DBNull.Value)
                {
                    log.LogMethodExit();
                    return;
                }
                else if (listTrxId.Contains(Convert.ToInt32(dgvKioskActivity.CurrentRow.Cells["TrxId"].Value)))
                {
                    listTrxId.Remove(Convert.ToInt32(dgvKioskActivity.CurrentRow.Cells["TrxId"].Value));
                    dgvKioskActivity.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    //dgvKioskActivity.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
                    dgvKioskActivity.Rows[e.RowIndex].DefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityTextForeColor;
                    (dgvKioskActivity.CurrentRow.Cells["btnPrintTrx"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                }
                else
                {
                    listTrxId.Add(Convert.ToInt32(dgvKioskActivity.CurrentRow.Cells["TrxId"].Value));
                    dgvKioskActivity.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Highlight;
                    dgvKioskActivity.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
                    (dgvKioskActivity.CurrentRow.Cells["btnPrintTrx"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                }
                if (listTrxId.Count == 1 || dgvKioskActivity.SelectedRows.Count == 1)
                {
                    if (dgvKioskActivity.CurrentRow.Cells["Activity"].Value != null 
                        && dgvKioskActivity.CurrentRow.Cells["Activity"].Value.ToString() != KioskStatic.ACTIVITY_TYPE_REFUND
                        && IsPaymentModeIsCreditCard(dgvKioskActivity.CurrentRow.Cells["TrxId"].Value.ToString()))
                    {
                        btnRefund.Enabled = true;
                    }
                }
                else
                {
                    btnRefund.Enabled = false;
                }
            }
        }
        private bool IsPaymentModeIsCreditCard(string trxId)
        {
            log.LogMethodEntry(trxId);
            bool isCreditCardPayment = false;
            TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
            List<TransactionPaymentsDTO> transactionPaymentsDTOList;
            List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, trxId.ToString()));
            transactionPaymentsDTOList = transactionPaymentsListBL.GetNonReversedTransactionPaymentsDTOList(searchParams);
            if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count >0)
            {
                if(transactionPaymentsDTOList[0].paymentModeDTO != null && transactionPaymentsDTOList[0].paymentModeDTO.IsCreditCard && transactionPaymentsDTOList[0].paymentModeDTO.GatewayLookUp != PaymentGateways.None)
                {
                    isCreditCardPayment = true;
                }

            }
            log.LogMethodExit(isCreditCardPayment);
            return isCreditCardPayment;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            scrollIndex += 10;
            buttonPrev.Visible = true;
            dgvKioskActivity.FirstDisplayedScrollingRowIndex = scrollIndex;
            if (scrollIndex + 10 >= dgvKioskActivity.RowCount)
                buttonNext.Visible = false;
            log.LogMethodExit();
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            scrollIndex -= 10;
            if (scrollIndex >= 10 || (scrollIndex == 0 && dgvKioskActivity.RowCount > 10))
                buttonNext.Visible = true;
            dgvKioskActivity.FirstDisplayedScrollingRowIndex = scrollIndex;
            if (scrollIndex <= 0)
                buttonPrev.Visible = false;
            log.LogMethodExit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtMessage.Text = lblGreeting.Text;
            if (listTrxId.Count == 0)
            {
                frmOKMsg fok = new frmOKMsg("No Transactions to Print");
                fok.ShowDialog();
                return;
            }
            KioskStatic.logToFile("Calling Print Method through admin options");
            string message = "";
            PrintMultipleTransactions printMultipleTransactions = new PrintMultipleTransactions(KioskStatic.Utilities);
            try
            {
                if (!printMultipleTransactions.Print(listTrxId, false, ref message))
                {
                    frmOKMsg fok = new frmOKMsg(message);
                    fok.ShowDialog();
                    KioskStatic.logToFile("Print message: " + message);
                    return;

                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                frmOKMsg fok = new frmOKMsg(ex.Message);
                fok.ShowDialog();
                return;
            }
            KioskStatic.logToFile("Completed Print through Admin options");
            log.LogMethodExit();
        }

        private void btnRefund_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtMessage.Text = lblGreeting.Text;
                btnRefund.Enabled = btnPrev.Enabled = btnPrint.Enabled = false;
                string message = string.Empty;
                int userId = -1;
                UsersList userList = new UsersList(executionContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>
                {
                    new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_NAME, "External POS")
                };
                List<UsersDTO> usersDTOList = userList.GetAllUsers(searchParameters);
                if (usersDTOList != null && usersDTOList.Count > 0)
                {
                    userId = usersDTOList[0].UserId;
                }
                TransactionUtils trxUtils = new TransactionUtils(Utilities);
                int trxIid = Convert.ToInt32(dgvKioskActivity.CurrentRow.Cells["TrxId"].Value);
                bool trxSuccessful = trxUtils.reverseTransaction(trxIid, -1, true, executionContext.GetMachineId().ToString(), Utilities.ParafaitEnv.LoginID, userId, "", KIOSK_REFUND_ACTIVITY_DESCRIPTION, ref message, true);
                if (trxSuccessful == false)
                {
                    throw new ValidationException(message);
                }
                else
                {
                    TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> trxSearchParam = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    trxSearchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.ORIGINAL_TRX_ID, trxIid.ToString()));
                    trxSearchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<TransactionDTO> transactionDTOList = transactionListBL.GetTransactionDTOList(trxSearchParam, Utilities);
                    KioskStatic.acceptance ac = new KioskStatic.acceptance();
                    if (transactionDTOList != null && transactionDTOList.Any())
                    {
                        transactionDTOList = transactionDTOList.OrderByDescending(trx => trx.TransactionDate).ToList();
                        ac.TrxId = transactionDTOList[0].TransactionId;
                    }
                    ac.totalValue = Convert.ToDecimal(dgvKioskActivity.CurrentRow.Cells["Amount"].Value);
                    KioskStatic.updateKioskActivityLog(-1, -1, "", KioskStatic.ACTIVITY_TYPE_REFUND, KIOSK_REFUND_ACTIVITY_DESCRIPTION, ac);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
                frmOKMsg fok = new frmOKMsg(ex.Message);
                fok.ShowDialog();
            }
            finally
            { 
                btnPrev.Enabled = btnPrint.Enabled = true;
            }
            listTrxId = new List<int>();
            clearFields();
            RefreshScreen();
            log.LogMethodExit();
        }

        void RefreshScreen()
        {
            log.LogMethodEntry();
            fetchTrxDetails();
            foreach (DataGridViewRow row in dgvKioskActivity.Rows)
            {   
                if (row.Cells["TrxId"].Value == DBNull.Value)
                {
                    (row.Cells["btnPrintTrx"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Grey_Check_Box_Empty;
                }
                else
                {
                    (row.Cells["btnPrintTrx"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                }
                //row.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
                row.DefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityTextForeColor; 
                row.DefaultCellStyle.BackColor = Color.White;
            }
            if (btnRefund.Visible == true)
            {
                btnRefund.Enabled = false;
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                foreach (Control c in dgvKioskActivity.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityTextForeColor;
                }
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.KioskActivityHeaderTextForeColor;
                this.lblActivity.ForeColor = KioskStatic.CurrentTheme.KioskActivityDetailsHeaderTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.KioskActivityBtnPrevTextForeColor;
                this.btnPrint.ForeColor = KioskStatic.CurrentTheme.KioskActivityBtnPrintTextForeColor;
                this.btnRefund.ForeColor = KioskStatic.CurrentTheme.KioskActivityBtnRefundTextForeColor;
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                txtMessage.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
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
                        e.FormattingApplied = true;
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
    }
}
