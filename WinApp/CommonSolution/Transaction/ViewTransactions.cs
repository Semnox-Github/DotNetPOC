/********************************************************************************************
 * Project Name - View Transactions
 * Description  - Show transaction details and other Transaction functions
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad Created 
 *2.00        26-Sep-2018      Mathew Ninan   Modified OpenCashDrawer call in reverse Transaction
 *                                            to use PrinterBL
 *2.70.0      17-Jul-2019      Divya A        Filter for My Transactions Tab and Bookings Tab, show default also
*2.80.0       17-Jun-2020      Girish Kundar  Modified : Refresh header query moved to Transaction Datahandler as part of transaction look up UI in POS
*2.80.0       24-Jul-2020      Girish Kundar  Modified : Issue fix - remove the flag check for Management studio call for view transactions
*2.100.0      26-Sept-2020      Girish Kundar     Modified : CashDrawer modification for Bowapegas 
*2.110.0      26-Sept-2020      Girish Kundar     Modified : CenterEgde changes - Removed ThirdParty references 
 *2.130.4     22-Feb-2022       Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer.Cashdrawers;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.User;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    public partial class ViewTransactions : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int GlobalTrxId = -1;
        bool showAmountFieldsTransaction = true;
        public Panel viewTransactionsPanel;
        public Button RefreshButton;
        public Button ExportButton;
        public Button ExitButton;
        public GroupBox CriteriaGroupBox;
        public DataGridView dgvHeader;
        public DataGridView dgvDetail;
        public SplitContainer splitContainer;
        public delegate string ReversalAuthentication();
        public ReversalAuthentication invokeAuthentication;
        public AuthenticateManagerDelegate authenticateManager;
        public delegate void TrxPrint(int TrxId);
        public TrxPrint invokeTrxPrint;
        public delegate void TrxReversalSuccess(int TrxId);
        public TrxReversalSuccess invokeTrxReversalSuccess;
        public delegate void TrxReopen(int TrxId);
        public TrxReopen invokeTrxReopen;
        public delegate void WaiverSignedUI(int trxId, int lineId, Utilities Utilities);
        public WaiverSignedUI waiverSignedUI;
        private IUpdateDetails updateDetails;
        Utilities Utilities;
        //Semnox.Parafait.Device.COMPort CashDrawerSerialPort;


        public ViewTransactions(int pTrxId, DateTime pFromDate, DateTime pToDate, int pUserId, string pPOS, Utilities ParafaitUtilities, IUpdateDetails updateDetails)
        {
            log.LogMethodEntry(pTrxId, pFromDate, pToDate, pUserId, pPOS, ParafaitUtilities, updateDetails);

            InitializeComponent();
            ParafaitUtilities.setLanguage(this);

            Utilities = ParafaitUtilities;
            this.updateDetails = updateDetails;
            viewTransactionsPanel = panelViewTransactions;
            RefreshButton = btnRefresh;
            ExportButton = btnExcel;
            ExitButton = btnCancel;
            CriteriaGroupBox = grpCriteria;
            dgvHeader = dgvTrxHeader;
            dgvDetail = dgvTrxDetails;
            splitContainer = splitContainerTrx;

            showAmountFieldsTransaction = (Utilities.getParafaitDefaults("SHOW_AMOUNT_FIELDS_MYTRANSACTIONS") == "Y");

            if (pTrxId != -1)
                txtTrxId.Text = pTrxId.ToString();

            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select u.user_id, username " +
                                "from users u, (select user_id, us.role_id, DataAccessLevel " +
                                                "from users us, user_roles ur " +
                                                  "where ur.role_id = us.role_id " +
                                                  "and us.user_id = @user_id) v " +
                                "where DataAccessLevel = 'S' " +
                                    "or (DataAccessLevel = 'U' and u.user_id = v.user_id) " +
                                    "or (DataAccessLevel = 'R' and exists (select 1 from user_roles urls , ManagementFormAccess mfa" +
                                                                        @" where urls.guid = mfa.FunctionGUID
                                                                        and mfa.role_id = v.role_id
                                                                        and u.role_id = urls.role_id
                                                                        and mfa.access_allowed = 'Y'
                                                                        and isnull(mfa.IsActive,1) = 1
                                                                        and mfa.FunctionGroup = 'Data Access'
                                                                        and mfa.main_menu = 'User Roles')) " +
                                                            " union all select -1, ' - All - ' order by 2";

            cmd.Parameters.AddWithValue("@user_id", Utilities.ParafaitEnv.User_Id);

            log.LogVariableState("@user_id", Utilities.ParafaitEnv.User_Id);


            SqlDataAdapter dasites = new SqlDataAdapter(cmd);
            DataTable dtUsers = new DataTable();
            dasites.Fill(dtUsers);
            cmbUser.DataSource = dtUsers;
            cmbUser.ValueMember = "user_id";
            cmbUser.DisplayMember = "username";
            cmbUser.SelectedIndex = 0;

            if (pPOS != null) // called form POS
            {
                cmd.CommandText = "select @pos POSName ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@pos", pPOS);
                filterPanel.Visible = true;
                log.LogVariableState("@pos", pPOS);
            }
            else
            {
                if (Utilities.ParafaitEnv.Role != "" && Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT") == "Y")
                {
                    cmd.CommandText = "select ' - All - ' POSName union all " +
                                        "select POSname " +
                                        "from POSMachines pm, ManagementFormAccess ma " +
                                       "where pm.guid = ma.FunctionGUID " +
                                        "and ma.main_menu = 'POS Machine' " +
                                        "and ma.access_allowed = 'Y' " +
                                        "and ma.role_id = @role " +
                                        "and isnull(ma.IsActive,1) = 1 " +
                                        "order by 1 ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@role", Utilities.ParafaitEnv.RoleId);
                    log.LogVariableState("@role", Utilities.ParafaitEnv.RoleId);
                }
                else
                {
                    cmd.CommandText = "select ' - All - ' POSName union " +
                                        "select distinct pos_machine from trx_header " +
                                        "where trxdate >= @fromDate and trxdate < @toDate " +
                                        "union select posName from POSMachines";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@fromDate", dtpFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@toDate", dtpTo.Value.Date.AddDays(1));

                    log.LogVariableState("@fromDate", dtpFrom.Value.Date);
                    log.LogVariableState("@toDate", dtpTo.Value.Date.AddDays(1));
                }
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtPOS = new DataTable();
            da.Fill(dtPOS);
            cmbPOS.DataSource = dtPOS;
            cmbPOS.SelectedIndex = 0;
            cmbPOS.ValueMember = "POSName";
            cmbPOS.DisplayMember = "POSName";

            if (pFromDate != DateTime.MinValue)
                dtpFrom.Value = pFromDate;
            if (pToDate != DateTime.MinValue)
                dtpTo.Value = pToDate;
            if (pUserId != -1)
                cmbUser.SelectedValue = pUserId;
            if (pPOS != null)
                cmbPOS.SelectedValue = pPOS;

            dgvTrxHeader.SelectionMode = dgvTrxDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTrxHeader.ReadOnly = dgvTrxDetails.ReadOnly = true;
            dgvTrxDetails.AllowUserToAddRows = dgvTrxHeader.AllowUserToAddRows =
                    dgvTrxDetails.AllowUserToDeleteRows = dgvTrxHeader.AllowUserToDeleteRows = false;

            Utilities.setupDataGridProperties(ref dgvTrxHeader);
            Utilities.setupDataGridProperties(ref dgvTrxDetails);
            dgvTrxHeader.AllowUserToResizeRows = false;
            dgvTrxDetails.AllowUserToResizeRows = false;

            splitContainerTrx.BackColor = Color.LightSteelBlue;
            splitContainerTrx.Panel1.BackColor = splitContainerTrx.Panel2.BackColor = Color.White;

            log.LogMethodExit(null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                (panelViewTransactions.Parent as Form).Close();
            }
            catch (Exception ex)
            {
                log.Error("Error while closing Transaction Panel View", ex);
            }

            log.LogMethodExit(null);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (rbHeaderDetail.Checked)
            {
                refreshHeader();
            }
            else
            {
                refreshDetailOnly();
            }

            log.LogMethodExit(null);
        }

        public void refreshHeader(int trxId = -1, string TrxReference = "", bool pastTrx = false, bool futureTrx = false, bool futureAllTrx = false,
                               bool todayTrx = false, List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = null)
        {
            log.LogMethodEntry(trxId, TrxReference, pastTrx, futureTrx, futureAllTrx, todayTrx);
            splitContainerTrx.Panel1Collapsed = false;
            bool enableOrderShareAcrossUsers = false;
            bool enableOrderShareAcrossPOS = false;
            DateTime fromDate = ServerDateTime.Now;
            DateTime toDate = ServerDateTime.Now;
            enableOrderShareAcrossUsers = (Utilities.getParafaitDefaults("ENABLE_ORDER_SHARE_ACROSS_USERS") == "Y");
            enableOrderShareAcrossPOS = (Utilities.getParafaitDefaults("ENABLE_ORDER_SHARE_ACROSS_POS") == "Y");
            DateTime Now = Utilities.getServerTime();
            int StartHour = String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")) ? 6 :
                Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"));

            if (pastTrx)
            {
                rdPast.Focus();
                DateTime from = Now;
                if (Now.Hour < StartHour)
                    from = from.AddDays(-1);
                fromDate = from.AddDays(-3).Date.AddHours(StartHour);
                toDate = from.Date.AddHours(StartHour);
            }
            else if (futureTrx)
            {
                rdFuture3.Focus();
                fromDate = Now;
                toDate = Now.AddDays(4).Date.AddHours(StartHour);
            }
            else if (futureAllTrx)
            {
                rdFutureAll.Focus();
                fromDate = Now;
                toDate = DateTime.MaxValue.Date;
            }
            else if (todayTrx)
            {
                rdToday.Focus();
                fromDate = (Now.Hour < StartHour ? Now.AddDays(-1).Date.AddHours(StartHour) : Now.Date.AddHours(StartHour));
                toDate = (Now.Hour >= StartHour ? Now.AddDays(1).Date.AddHours(StartHour) : Now.Date.AddHours(StartHour));
            }
            else
            {
                rdToday.Focus();
                fromDate = dtpFrom.Value.Date.AddHours(StartHour);
                toDate = dtpTo.Value.Date.AddDays(1).AddHours(StartHour);
            }
            log.LogVariableState("@fromDate", dtpFrom.Value.Date.AddHours(StartHour));
            log.LogVariableState("@toDate", dtpTo.Value.Date.AddDays(1).AddHours(StartHour));
            log.LogVariableState("@loginUserId", Utilities.ParafaitEnv.User_Id);
            log.LogVariableState("@user_id", cmbUser.SelectedValue == null ? -1 : cmbUser.SelectedValue);
            log.LogVariableState("@POS", cmbPOS.SelectedValue == null ? " - All - " : cmbPOS.SelectedValue);
            log.LogVariableState("@status", cmbTrxStatus.SelectedIndex <= 0 ? DBNull.Value : cmbTrxStatus.SelectedItem);
            log.LogVariableState("@enableOrderShareAcrossPOS", (cmbPOS.SelectedValue.Equals(" - All - ") ? enableOrderShareAcrossPOS : false));
            log.LogVariableState("@showAmountFields", showAmountFieldsTransaction);
            log.LogVariableState("@enableOrderShareAcrossUsers", (cmbUser.SelectedValue.Equals(-1) ? enableOrderShareAcrossUsers : false));

            if (trxId != -1)
            {
                GlobalTrxId = trxId;
            }
            else if (txtTrxId.Text.Trim() != "")
            {
                try
                {
                    GlobalTrxId = Convert.ToInt32(txtTrxId.Text);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured when converrting txtTrxId to Integer", ex);
                    GlobalTrxId = -1;
                }
            }

            if (searchParameters == null)
            {
                // This search parameter is build as default 
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> trxSearchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                if (GlobalTrxId > -1)
                {
                    trxSearchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, GlobalTrxId.ToString()));
                }
                else
                {
                    if (string.IsNullOrEmpty(TrxReference) == false)
                    {
                        trxSearchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.REMARKS, TrxReference.ToString()));
                    }
                    string posName = cmbPOS.SelectedValue == null ? string.Empty : cmbPOS.SelectedValue.ToString();

                    if (string.IsNullOrEmpty(posName) == false && posName.Trim() != " - All -".Trim())
                    {
                        trxSearchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_NAME, posName.ToString()));
                    }
                    int selectedUserId = Convert.ToInt32(cmbUser.SelectedValue);
                    trxSearchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.USER_ID, selectedUserId.ToString()));
                    string status = cmbTrxStatus.SelectedIndex <= 0 ? null : cmbTrxStatus.SelectedItem.ToString();
                    if (string.IsNullOrEmpty(status) == false)
                    {
                        trxSearchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, status));
                    }
                    trxSearchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    trxSearchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, toDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                searchParameters = trxSearchParameters;
            }


            TransactionListBL transactionListBL = new TransactionListBL(Utilities.ExecutionContext);
            DataTable trxHeaderDTOList = transactionListBL.GetRefreshHeaderRecords(searchParameters, Utilities.ParafaitEnv.TRXNO_USER_COLUMN_HEADING,
                                                                                                showAmountFieldsTransaction, Utilities.ParafaitEnv.User_Id);
            log.LogVariableState("@TrxId", GlobalTrxId);
            log.LogVariableState("@reference", TrxReference);
            dgvTrxHeader.DataSource = trxHeaderDTOList;

            dgvTrxHeader.Columns[6].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvTrxHeader.Columns[7].DefaultCellStyle =
            dgvTrxHeader.Columns[8].DefaultCellStyle =
            //dgvTrxHeader.Columns[9].DefaultCellStyle =
            dgvTrxHeader.Columns[10].DefaultCellStyle =
            //dgvTrxHeader.Columns[11].DefaultCellStyle = 
            dgvTrxHeader.Columns[16].DefaultCellStyle =
            dgvTrxHeader.Columns[17].DefaultCellStyle =
            dgvTrxHeader.Columns[18].DefaultCellStyle =
            dgvTrxHeader.Columns[19].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvTrxHeader.Refresh();
            Utilities.setupDataGridProperties(ref dgvTrxHeader);
            dgvTrxHeader.BackgroundColor = Color.White;
            dgvTrxHeader.Columns["ID"].DefaultCellStyle.ForeColor = Color.Blue;
            dgvTrxHeader.Columns["Net_amount"].DefaultCellStyle.ForeColor = Color.Blue;
            dgvTrxHeader.Columns["avg_disc_%"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvTrxHeader.Columns["avg_disc_%"].DefaultCellStyle.ForeColor = Color.Blue;
            dgvTrxHeader.Columns["avg_disc_%"].DefaultCellStyle.Font = new Font(dgvTrxHeader.DefaultCellStyle.Font, FontStyle.Underline);
            dgvTrxHeader.Columns["Net_amount"].DefaultCellStyle.Font = new Font(dgvTrxHeader.DefaultCellStyle.Font, FontStyle.Underline);
            dgvTrxHeader.Columns["Net_amount"].DefaultCellStyle.Format = Utilities.ParafaitEnv.AMOUNT_FORMAT;
            dgvTrxHeader.Columns["Net_amount"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgvTrxHeader.Columns["Date"].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
            dgvTrxHeader.Columns["ID"].DefaultCellStyle.Font = new Font(dgvTrxHeader.DefaultCellStyle.Font, FontStyle.Underline);
            dgvTrxHeader.Columns["ID"].DefaultCellStyle.ForeColor = Color.Blue;
            dgvTrxHeader.Refresh();


            foreach (DataGridViewRow dr in dgvHeader.Rows)
            {
                if (dr.Cells["Ref"].Value.ToString().Contains("Reversal"))
                {
                    dr.DefaultCellStyle.BackColor = Color.Tomato;
                    dr.DefaultCellStyle.SelectionBackColor = Color.DarkOrange;
                }
            }

            refreshLines();
            GlobalTrxId = -1;

            Utilities.setLanguage(dgvTrxHeader);

            log.LogMethodExit(null);
        }

        private void refreshLines()
        {
            log.LogMethodEntry();

            if (dgvTrxHeader.CurrentRow == null)
            {
                dgvTrxDetails.DataSource = null;

                log.LogMethodExit(null);
                return;
            }

            SqlCommand cmd = Utilities.getCommand();
            dgvTrxDetails.Columns[0].Visible = dgvTrxHeader.Columns[0].Visible; // don't make this visible if called from reports
            cmd.CommandText = @"select  
                          p.[product_name] as Product 
                          ,l.[quantity]
                         ,case when @showAmountFields = 1 then l.[price] else 0 end as Price
                         ,case when @showAmountFields = 1 then l.[amount] else 0 end as amount
                         ,(select top 1 p.FirstName + ' '+ isnull(p.LastName,'')
                                              from customers c,
                                                      Profile p,
                                                      CustomerSignedWaiver csw,
                                                         WaiversSigned ws
                                                where ws.TrxId = l.trxId
                                                   and ws.LineId = l.LineId
                                                   and ws.IsActive = 1
                                                   and ws.CustomerSignedWaiverId = csw.CustomerSignedWaiverId
                                                   and csw.SignedFor = c.customer_id
                                                   and p.id = c.profileId) as Waiver_Customer
                         ,l.[card_number]
                          ,l.[credits] 
                          ,l.[courtesy] 
                          ,l.[bonus] 
                          ,l.[time] 
                          ,l.[tickets] 
                          ,t.[tax_name] " +
                          ",l.[tax_percentage] as \"Tax %\" " +
                         @",l.[loyalty_points] 
                         ,case when @showAmountFields = 1 then l.[UserPrice] else 0 end UserPrice
                         ,l.[LineId] Line
                         ,isnull(WaiverSignedCount,0) as Waivers_Signed
                         ,l.Remarks  
                     FROM trx_lines l left outer join products p on p.product_id = l.product_id
                         left outer join tax t on t.tax_id = l.tax_id
                         left outer join (Select w.LineId,  Count(DISTINCT  customerSignedWaiverId) as WaiverSignedCount from waiversSigned w
                                           where trxid = @trxid and isnull(isActive, 0) = 1 
                                           and isnull(customerSignedWaiverId, -1) != -1 group by w.LineId)  wsc on l.Lineid  = wsc.LineId
                     where trxid = @trxid
                            order by l.lineId";
            cmd.Parameters.AddWithValue("@trxid", dgvTrxHeader.CurrentRow.Cells["ID"].Value);
            cmd.Parameters.AddWithValue("@showAmountFields", showAmountFieldsTransaction);

            log.LogVariableState("@showAmountFields", showAmountFieldsTransaction);
            log.LogVariableState("@trxid", dgvTrxHeader.CurrentRow.Cells["ID"].Value);

            SqlDataAdapter dal = new SqlDataAdapter(cmd);
            DataTable dtl = new DataTable();
            dal.Fill(dtl);
            dgvTrxDetails.DataSource = dtl;
            Utilities.setupDataGridProperties(ref dgvTrxDetails);
            dgvTrxDetails.BackgroundColor = Color.White;
            dgvTrxDetails.Columns["Line"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dgvTrxDetails.Columns[2].DefaultCellStyle =
            dgvTrxDetails.Columns[3].DefaultCellStyle =
            dgvTrxDetails.Columns[4].DefaultCellStyle =
            dgvTrxDetails.Columns[5].DefaultCellStyle =
            dgvTrxDetails.Columns[6].DefaultCellStyle =
            dgvTrxDetails.Columns[7].DefaultCellStyle =
            dgvTrxDetails.Columns[8].DefaultCellStyle =
            dgvTrxDetails.Columns[9].DefaultCellStyle =
            dgvTrxDetails.Columns[12].DefaultCellStyle =
            dgvTrxDetails.Columns["loyalty_points"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvTrxDetails.Columns[10].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dgvTrxDetails.Columns["Tickets"].HeaderText = Utilities.getParafaitDefaults("REDEMPTION_TICKET_NAME_VARIANT");

            dgvTrxDetails.Columns["Waivers_Signed"].DefaultCellStyle.ForeColor = Color.Blue;
            dgvTrxDetails.Columns["Waivers_Signed"].DefaultCellStyle.Font = new Font(dgvTrxHeader.DefaultCellStyle.Font, FontStyle.Underline);
            dgvTrxDetails.Columns["Waivers_Signed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            Utilities.setLanguage(dgvTrxDetails);
            dgvTrxDetails.Refresh();

            log.LogMethodExit(null);
        }

        private void refreshDetailOnly()
        {
            log.LogMethodEntry();

            bool enableOrderShareAcrossUsers = false;
            bool enableOrderShareAcrossPOS = false;
            bool showAmountFieldsTransaction = true;
            enableOrderShareAcrossUsers = (Utilities.getParafaitDefaults("ENABLE_ORDER_SHARE_ACROSS_USERS") == "Y");
            enableOrderShareAcrossPOS = (Utilities.getParafaitDefaults("ENABLE_ORDER_SHARE_ACROSS_POS") == "Y");
            showAmountFieldsTransaction = (Utilities.getParafaitDefaults("SHOW_AMOUNT_FIELDS_MYTRANSACTIONS") == "Y");
            splitContainerTrx.Panel1Collapsed = true;
            SqlCommand cmd = Utilities.getCommand();
            dgvTrxDetails.Columns[0].Visible = false;
            cmd.CommandText = "exec dbo.SetContextInfo @loginUserId ;select h.trxid ID " +
                          ",h.trx_no as \"" + Utilities.ParafaitEnv.TRXNO_USER_COLUMN_HEADING + "\" " +
                          @",h.trxdate as Date 
                          ,case when @showAmountFields = 1 then [TrxAmount] else 0 end as Amount 
                          ,case when @showAmountFields = 1 then [TrxNetAmount] else 0 end as Net_amount 
                          ,case payment_mode when 1 then 'Cash' when 2 then 'Credit Card' when 3 then 'Debit Card' when 4 then 'Other' else 'Multiple' end as pay_mode 
                          ,[pos_machine] as POS 
                          ,u.username as Cashier 
                          ,p.[product_name] as Product " +
                          ",case when @showAmountFields = 1 then l.[price] else 0 end as \"Price/Disc%\" " +
                          @",case when @showAmountFields = 1 then l.[amount] else 0 end Line_amount 
                          ,(select top 1 p.FirstName + ' '+ isnull(p.LastName,'')
                                              from customers c,
                                                      Profile p,
                                                      CustomerSignedWaiver csw,
                                                         WaiversSigned ws
                                                where ws.TrxId = l.trxId
                                                   and ws.LineId = l.LineId
                                                   and ws.IsActive = 1
                                                   and ws.CustomerSignedWaiverId = csw.CustomerSignedWaiverId
                                                   and csw.SignedFor = c.customer_id
                                                   and p.id = c.profileId) as Waiver_Customer
                          ,l.[card_number] 
                          ,l.[credits] 
                          ,l.[courtesy] 
                          ,l.[bonus] 
                          ,l.[time] 
                          ,l.[tickets] 
                          ,t.[tax_name] " +
                          ",l.[tax_percentage] as \"Tax %\" " +
                          @",l.[quantity] 
                          ,l.[loyalty_points] 
                          ,l.[LineId] Line 
                          ,h.[Status] Status 
                      FROM trx_header h, users u, trx_lines l left outer join products p on p.product_id = l.product_id 
                          left outer join tax t on t.tax_id = l.tax_id,
                                (select top 1 *
                           from (select user_id, r.role_id, r.DataAccessLevel, r.DataAccessRuleId
                             from users u, user_roles r
                             where user_id = dbo.GetContextInfo()
                             and r.role_id = u.role_id
                             union all select -1, -1, 'S',-1) viw order by 1 desc) ur 
                            where l.trxid = h.trxid 
                            --and (isnull(h.status, 'CLOSED') = @status or @status is null) 
                            and u.user_id = h.user_id 
                            --and trxdate >= @fromDate and trxdate < @toDate 
                            --and (h.user_id = @user_id or @user_id = -1) 
                            --and (h.pos_machine = @POS or @POS = ' - All - ') 
                            --and (h.trxid = @TrxId or @TrxId = -1) 
                            and (h.trxid = @TrxId
                                    or (@TrxId = -1
                                        and trxdate >= @fromDate and trxdate < @toDate
                                        and (isnull(h.status, 'CLOSED') = @status or @status is null)
                                        --    and (h.user_id = @user_id or @user_id = -1)
                                        and (@enableOrderShareAcrossUsers = 1 or h.user_id = @user_id or @user_id = -1)
                                        and ((@enableOrderShareAcrossPOS = 1 or (h.pos_machine = @POS))
							                or @POS = ' - All - ')
                                       )
                                   )
                            and (h.user_id in (select user_id 
	                                 from  DataAccessView
					                 where
						               ( (Entity = 'Transaction' and ur.DataAccessRuleId is not null)
						                  OR 
						                  ur.DataAccessRuleId is null
						                )
						             ))--New rule introduced
                            order by h.trxdate desc, lineId";

            cmd.Parameters.AddWithValue("@fromDate", dtpFrom.Value.Date);
            cmd.Parameters.AddWithValue("@toDate", dtpTo.Value.Date.AddDays(1));
            cmd.Parameters.AddWithValue("@user_id", cmbUser.SelectedValue == null ? -1 : cmbUser.SelectedValue);
            cmd.Parameters.AddWithValue("@POS", cmbPOS.SelectedValue == null ? " - All - " : cmbPOS.SelectedValue);
            cmd.Parameters.AddWithValue("@status", cmbTrxStatus.SelectedIndex <= 0 ? DBNull.Value : cmbTrxStatus.SelectedItem);
            cmd.Parameters.AddWithValue("@enableOrderShareAcrossUsers", (cmbUser.SelectedValue.Equals(-1) ? enableOrderShareAcrossUsers : false));
            cmd.Parameters.AddWithValue("@enableOrderShareAcrossPOS", (cmbPOS.SelectedValue.Equals(" - All - ") ? enableOrderShareAcrossPOS : false));
            cmd.Parameters.AddWithValue("@showAmountFields", showAmountFieldsTransaction);
            cmd.Parameters.AddWithValue("@loginUserId", Utilities.ParafaitEnv.User_Id);

            log.LogVariableState("@fromDate", dtpFrom.Value.Date);
            log.LogVariableState("@toDate", dtpTo.Value.Date.AddDays(1));
            log.LogVariableState("@user_id", cmbUser.SelectedValue == null ? -1 : cmbUser.SelectedValue);
            log.LogVariableState("@POS", cmbPOS.SelectedValue == null ? " - All - " : cmbPOS.SelectedValue);
            log.LogVariableState("@status", cmbTrxStatus.SelectedIndex <= 0 ? DBNull.Value : cmbTrxStatus.SelectedItem);
            log.LogVariableState("@enableOrderShareAcrossPOS", (cmbPOS.SelectedValue.Equals(" - All - ") ? enableOrderShareAcrossPOS : false));
            log.LogVariableState("@showAmountFields", showAmountFieldsTransaction);
            log.LogVariableState("@enableOrderShareAcrossUsers", (cmbUser.SelectedValue.Equals(-1) ? enableOrderShareAcrossUsers : false));

            if (txtTrxId.Text.Trim() != "")
            {
                try
                {
                    GlobalTrxId = Convert.ToInt32(txtTrxId.Text);
                }
                catch (Exception ex)
                {
                    log.Error("Error while converting txtTrxId to Integer", ex);
                    GlobalTrxId = -1;
                }
            }
            cmd.Parameters.AddWithValue("@TrxId", GlobalTrxId);

            log.LogVariableState("@TrxId", GlobalTrxId);

            SqlDataAdapter dal = new SqlDataAdapter(cmd);
            DataTable dtl = new DataTable();
            dal.Fill(dtl);
            dgvTrxDetails.DataSource = null;
            dgvTrxDetails.DataSource = dtl;
            Utilities.setupDataGridProperties(ref dgvTrxDetails);
            dgvTrxDetails.BackgroundColor = Color.White;

            dgvTrxDetails.Columns[3].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvTrxDetails.Columns[4].DefaultCellStyle =
            dgvTrxDetails.Columns[5].DefaultCellStyle =
            dgvTrxDetails.Columns[10].DefaultCellStyle =
            dgvTrxDetails.Columns[11].DefaultCellStyle =
            dgvTrxDetails.Columns[14].DefaultCellStyle =
            dgvTrxDetails.Columns[15].DefaultCellStyle =
            dgvTrxDetails.Columns[16].DefaultCellStyle =
            dgvTrxDetails.Columns[17].DefaultCellStyle =
            dgvTrxDetails.Columns[20].DefaultCellStyle =
            dgvTrxDetails.Columns["loyalty_points"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvTrxDetails.Columns[21].DefaultCellStyle =
            dgvTrxDetails.Columns[18].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dgvTrxDetails.Columns["ID"].DefaultCellStyle.ForeColor = Color.Blue;

            Utilities.setLanguage(dgvTrxDetails);
            dgvTrxDetails.Refresh();
            log.LogMethodExit(null);
        }

        private void ViewTransactions_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            log.LogMethodExit(null);
        }

        private void dgvTrxHeader_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            refreshLines();

            log.LogMethodExit(null);
        }

        private void txtTrxId_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.KeyChar == 13)
                btnRefresh.PerformClick();
            else if (!(char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;

            log.LogMethodExit(null);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (rbHeaderDetail.Checked)
            {
                Utilities.ExportToExcel(dgvTrxHeader, Utilities.MessageUtils.getMessage("Transaction Header") + " " + dtpFrom.Value.ToString("dd-MMM-yyyy") + " - " + dtpTo.Value.ToString("dd-MMM-yyyy"), "Transactions", Utilities.ParafaitEnv.SiteName, dtpFrom.Value, dtpTo.Value);
                try
                {
                    Utilities.ExportToExcel(dgvTrxDetails, Utilities.MessageUtils.getMessage("Transaction Details") + " - " + Utilities.MessageUtils.getMessage("TrxID") + "-" + dgvTrxHeader.CurrentRow.Cells["ID"].Value.ToString() + " " + dtpFrom.Value.ToString("dd-MMM-yyyy") + " - " + dtpTo.Value.ToString("dd-MMM-yyyy"), "Transaction Details - TrxID-" + dgvTrxHeader.CurrentRow.Cells["ID"].Value.ToString(), Utilities.ParafaitEnv.SiteName, dtpFrom.Value, dtpTo.Value);
                }
                catch (Exception ex)
                {
                    log.Error("Error whie Exporting to Excel", ex);
                }
            }
            else
            {
                try
                {
                    Utilities.ExportToExcel(dgvTrxDetails, Utilities.MessageUtils.getMessage("Transaction Details") + " - " + dtpFrom.Value.ToString("dd-MMM-yyyy") + " - " + dtpTo.Value.ToString("dd-MMM-yyyy"), "Transaction Details", Utilities.ParafaitEnv.SiteName, dtpFrom.Value, dtpTo.Value);
                }
                catch (Exception ex)
                {
                    log.Error("Error whie Exporting to Excel", ex);
                }
            }
            log.LogMethodExit(null);
        }

        private void reverseTransaction(int trxId, int trxLine, string POSMachine, string loginId, int userId)
        {
            log.LogMethodEntry(trxId, trxLine, POSMachine, loginId, userId);

            string approver = Utilities.ParafaitEnv.LoginID;
            if (invokeAuthentication != null) // if authentication is required, set this to external function
            {
                approver = invokeAuthentication.Invoke();
                if (approver == "false")
                {
                    log.LogMethodExit(null);
                    return;
                }
            }
            bool CardReversal = true;

            ReversalRemarks rm = new ReversalRemarks(Utilities);
            while (1 == 1)
            {
                if (rm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(rm.Remarks))
                    {
                        MessageBox.Show(Utilities.MessageUtils.getMessage(341), "Reversal Remarks", MessageBoxButtons.OK);
                        continue;
                    }
                    else
                        break;
                }
                else
                {
                    log.LogMethodExit(null);
                    return;
                }
            }

            try
            {
                string message = "";
                //Modified code block for merkle integration, update coupon status API call
                Transaction reversingTrx = new Transaction(Utilities);

                //Begin Modified on 24-02-2017 - code block for merkle integration, update coupon status API call
                List<string> couponsList = new List<string>();
                TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                if (Utilities.getParafaitDefaults("ENABLE_MERKLE_INTEGRATION").Equals("Y"))
                {
                    couponsList = reversingTrx.GetCouponsList(trxId);
                }
                //End Modified on 24-02-2017  code block for merkle integration, update coupon status API call
                string remark = rm.Remarks + ((string.IsNullOrEmpty(rm.reason)) ? "" : "|" + rm.reason + "|");

                if (!TransactionUtils.reverseTransaction(trxId, trxLine, CardReversal, POSMachine, loginId, userId, approver, remark, ref message)) //19-Oct-2015 Modification done for fiscal printer
                    MessageBox.Show(message, "Reversal Error");
                else
                {
                    MessageBox.Show(message, "Reversal Successful");
                    POSMachines posMachine = new POSMachines(Utilities.ExecutionContext, Utilities.ParafaitEnv.POSMachineId);
                    List<POSPrinterDTO> posPrintersDTOList = posMachine.PopulatePrinterDetails();
                    //Added on 8-Aug-2016 for opening Cash Drawer
                    PrintTransaction pt = new PrintTransaction(posPrintersDTOList);
                    //commented existing code because declaring begining of else block 
                    string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CASHDRAWER_INTERFACE_MODE");
                    log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);
                    bool cashdrawerMandatory = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "CASHDRAWER_ASSIGNMENT_MANDATORY_FOR_TRX");
                    log.Debug("cashdrawerMandatory :" + cashdrawerMandatory);
                    POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(Utilities.ExecutionContext.SiteId,
                                                                       POSMachine, "", -1);

                    if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.NONE)
                       || cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE))
                    {
                        if (pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null &&
                                                    pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any())
                        {
                            var posCashdrawerDTO = pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Where(x => x.IsActive == true).FirstOrDefault();
                            if (posCashdrawerDTO != null && posCashdrawerDTO.CashdrawerId > -1)
                            {
                                CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, posCashdrawerDTO.CashdrawerId);
                                cashdrawerBL.OpenCashDrawer();
                            }
                        }
                    }
                    else if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.MULTIPLE))
                      {
                        if (pOSMachineContainerDTO.POSCashdrawerContainerDTOList == null ||
                                                    pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any() == false)
                        {
                            log.Error("cashdrawer is not mapped to the POS"); 
                            MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4072));
                            return;
                        }

                        List<ShiftDTO> openShifts = posMachine.GetAllOpenShifts();
                        if (openShifts != null && openShifts.Any())
                        {
                            var shiftDTO = openShifts.Where(x => x.ShiftLoginId == loginId && x.POSMachine == POSMachine).FirstOrDefault();
                            if (shiftDTO != null && shiftDTO.CashdrawerId > -1)
                            {
                                CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, shiftDTO.CashdrawerId);
                                cashdrawerBL.OpenCashDrawer();
                            }
                        }
                    }

                    //if (Utilities.ParafaitEnv.OPEN_CASH_DRAWER == "Y" && TransactionUtils.OpenCashDrawer)
                    //{
                    //    if (Utilities.ParafaitEnv.CASH_DRAWER_INTERFACE == "Serial Port")
                    //    {
                    //        if (CashDrawerSerialPort != null && CashDrawerSerialPort.comPort.IsOpen)
                    //        {
                    //            CashDrawerSerialPort.comPort.Write(Utilities.ParafaitEnv.CASH_DRAWER_SERIALPORT_STRING, 0, Utilities.ParafaitEnv.CASH_DRAWER_SERIALPORT_STRING.Length);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext);
                    //        printerBL.OpenCashDrawer();
                    //    }
                    //}


                    //Begin Modified on 24-02-2017 -update Coupons used details to merkle by calling API
                    if (Utilities.getParafaitDefaults("ENABLE_MERKLE_INTEGRATION").Equals("Y"))
                    {
                        if (reversingTrx != null)
                        {
                            if (couponsList != null)
                            {
                                try
                                {
                                    MessageBox.Show("Please wait..Response awaited from Merkle...", "Merkle Integration Enabled");
                                    bool status = UpdateCouponStatus("reissued", couponsList);
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Merkle Integration Failed", ex);
                                    MessageBox.Show("Merkle Integration Failed: " + ex.Message, "Merkle Integration Enabled");
                                    Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while Updating Coupon status - Used ,Customer WeChatb Token: " + reversingTrx.customerDTO.WeChatAccessToken + "Errro details: " + ex.ToString(), "", "MerkleAPIIntegration", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                                }
                            }
                        }
                    }
                    //End Modified on 24-02-2017 -update Coupons used details to merkle by calling API
                    if (invokeTrxReversalSuccess != null)
                        invokeTrxReversalSuccess.Invoke(trxId);
                }
            }
            catch (Exception ex)
            {
                log.Error("Reversal Error ", ex);
                MessageBox.Show(ex.Message, "Reversal Error");
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Updates the coupon status to merkel service
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <param name="couponsToUpdate"></param>
        /// <returns></returns>
        public bool UpdateCouponStatus(string couponStatus, List<string> couponsToUpdate)
        {
            log.LogMethodEntry(couponStatus, couponsToUpdate);

            bool retStatus = false;
            if (couponsToUpdate != null)
            {
                for (int d = 0; d < couponsToUpdate.Count; d++)
                {
                    if (couponsToUpdate[d] != null)
                    {
                        retStatus = updateDetails.Update(couponsToUpdate[d].ToString(), couponStatus);

                        if (!retStatus)
                            Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Merkle Failed Updating Coupon status -" + couponStatus + " Coupon Number: " + couponsToUpdate[d].ToString(), "", "MerkleAPIIntegration", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                    }
                }
            }
            log.LogMethodExit(retStatus);
            return retStatus;
        }

        private void dgvTrxDetails_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.ColumnIndex < 0)
            {
                log.LogMethodExit(null);
                return;
            }
            if (dgvTrxDetails.Columns[e.ColumnIndex].DataPropertyName == "ID")
            {
                dgvTrxDetails.Cursor = Cursors.Hand;
                try
                {
                    dgvTrxDetails[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvTrxDetails.DefaultCellStyle.Font, FontStyle.Underline);
                }
                catch (Exception ex)
                {
                    log.Error("Unable to assign required Font! ", ex);
                }
            }
            if (dgvTrxDetails.Columns[e.ColumnIndex].DataPropertyName == "WaiversSigned" || dgvTrxDetails.Columns[e.ColumnIndex].DataPropertyName == "waiverSigned")
            {
                dgvTrxDetails.Cursor = Cursors.Hand;
                //try
                //{
                //    dgvTrxDetails[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvTrxDetails.DefaultCellStyle.Font, FontStyle.Underline);
                //}
                //catch (Exception ex)
                //{
                //    log.Error("Unable to assign Default Cell Style!", ex);
                //}
            }

            log.LogMethodExit(null);
        }

        private void dgvTrxDetails_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.ColumnIndex < 0)
            {
                log.LogMethodExit(null);
                return;
            }
            if (dgvTrxDetails.Columns[e.ColumnIndex].DataPropertyName == "ID")
            {
                dgvTrxDetails.Cursor = Cursors.Default;
                try
                {
                    dgvTrxDetails[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvTrxDetails.DefaultCellStyle.Font, FontStyle.Regular);
                }
                catch (Exception ex)
                {
                    log.Error("Unable to assign required Font! ", ex);
                }
            }

            log.LogMethodExit(null);
        }

        private void dgvTrxDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.ColumnIndex < 0)
                {
                    log.LogMethodExit(null);
                    return;
                }

                if (dgvTrxDetails.Columns[e.ColumnIndex].DataPropertyName == "ID")
                {
                    try
                    {
                        GlobalTrxId = Convert.ToInt32(dgvTrxDetails["ID", e.RowIndex].Value);
                        refreshHeader(pastTrx: rdPast.Checked, futureTrx: rdFuture3.Checked, futureAllTrx: rdFutureAll.Checked, todayTrx: rdToday.Checked);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Unable to Convert Data Grid View Transactions to Integer", ex);
                    }
                }
                else if (dgvTrxDetails.Columns[e.ColumnIndex].Name == "btnCancelLine")
                {
                    if (Utilities.getParafaitDefaults("ENABLE_BIR_REGULATION_PROCESS").Equals("Y"))
                    {
                        log.Debug("Line reversal is removed as End of day is enabled");
                        return;
                    }

                    if (dgvTrxHeader.CurrentRow == null || dgvTrxHeader.CurrentRow.Cells["ID"].Value == null)
                    {
                        log.LogMethodExit(null);
                        return;
                    }

                    if (dgvTrxHeader.CurrentRow != null && dgvTrxHeader.CurrentRow.Cells["Status"].Value.ToString() != "CLOSED")
                    {
                        log.LogMethodExit(null);
                        return;
                    }

                    if (MessageBox.Show(Utilities.MessageUtils.getMessage(488), "Transaction Line Reversal", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        log.LogMethodExit(null);
                        return;
                    }

                    if (dgvTrxHeader.CurrentRow != null)
                    {
                        if (dgvTrxDetails.CurrentRow != null)
                        {
                            int TrxId = Convert.ToInt32(dgvTrxHeader.CurrentRow.Cells["ID"].Value);
                            int lineId = Convert.ToInt32(dgvTrxDetails.CurrentRow.Cells["Line"].Value);
                            reverseTransaction(TrxId, lineId, Utilities.ParafaitEnv.POSMachine, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.User_Id);

                            refreshHeader(pastTrx: rdPast.Checked, futureTrx: rdFuture3.Checked, futureAllTrx: rdFutureAll.Checked, todayTrx: rdToday.Checked);
                        }
                    }
                }
                else if (dgvTrxDetails.Columns[e.ColumnIndex].DataPropertyName == "Waivers_Signed")
                {
                    if (dgvTrxHeader.CurrentRow != null && dgvTrxHeader.CurrentRow.Cells["ID"].Value != null)
                    {
                        if (dgvTrxDetails.CurrentRow != null)
                        {
                            int TrxId = Convert.ToInt32(dgvTrxHeader.CurrentRow.Cells["ID"].Value);
                            int lineId = Convert.ToInt32(dgvTrxDetails.CurrentRow.Cells["Line"].Value);
                            int waiverCount = Convert.ToInt32(dgvTrxDetails.CurrentRow.Cells["Waivers_Signed"].Value);
                            if (waiverCount > 0)
                                waiverSignedUI(TrxId, lineId, Utilities);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(null);
        }



        private void dgvTrxHeader_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                log.LogMethodEntry(null);
                return;
            }
            if (dgvTrxHeader.Columns[e.ColumnIndex].DataPropertyName == "Net_amount" || dgvTrxHeader.Columns[e.ColumnIndex].DataPropertyName == "avg_disc_%"
                || dgvTrxHeader.Columns[e.ColumnIndex].DataPropertyName == "ID")
            {
                dgvTrxHeader.Cursor = Cursors.Hand;
                //try
                //{
                //    dgvTrxHeader[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvTrxHeader.DefaultCellStyle.Font, FontStyle.Underline);
                //}
                //catch(Exception ex)
                //{
                //    log.Error("Unable to assign Default Cell Style!", ex);
                //}
            }
            log.LogMethodExit(null);
        }

        private void dgvTrxHeader_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.ColumnIndex < 0)
            {
                log.LogMethodExit(null);
                return;
            }
            if (dgvTrxHeader.Columns[e.ColumnIndex].DataPropertyName == "Net_amount" || dgvTrxHeader.Columns[e.ColumnIndex].DataPropertyName == "avg_disc_%"
                || dgvTrxHeader.Columns[e.ColumnIndex].DataPropertyName == "ID")
            {
                dgvTrxHeader.Cursor = Cursors.Default;
                //try
                //{
                //    dgvTrxHeader[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvTrxHeader.DefaultCellStyle.Font, FontStyle.Underline);
                //}
                //catch(Exception ex)
                //{
                //    log.Error("Unable to assign Default Cell Style", ex);
                //}
            }

            log.LogMethodExit(null);
        }

        private void dgvTrxHeader_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit(null);
                return;
            }

            if (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2)
            {
                if (dgvTrxHeader.CurrentRow == null || dgvTrxHeader.CurrentRow.Cells["ID"].Value == null)
                {
                    log.LogMethodExit(null);
                    return;
                }
            }

            if (e.ColumnIndex == 0)
            {
                if (MessageBox.Show(Utilities.MessageUtils.getMessage(489), "Transaction Reversal", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    log.LogMethodExit(null);
                    return;
                }

                if (dgvTrxHeader.CurrentRow != null)
                {
                    int TrxId = Convert.ToInt32(dgvTrxHeader.CurrentRow.Cells["ID"].Value);
                    reverseTransaction(TrxId, -1, Utilities.ParafaitEnv.POSMachine, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.User_Id);
                    refreshHeader(pastTrx: rdPast.Checked, futureTrx: rdFuture3.Checked, futureAllTrx: rdFutureAll.Checked, todayTrx: rdToday.Checked);
                }
            }
            else if (e.ColumnIndex == 1)
            {
                if (invokeTrxPrint == null)
                {
                    log.LogMethodExit(null);
                    return;
                }

                if (MessageBox.Show(Utilities.MessageUtils.getMessage(490), "Transaction Print", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    log.LogMethodExit(null);
                    return;
                }

                if (dgvTrxHeader.CurrentRow != null)
                {
                    int TrxId = Convert.ToInt32(dgvTrxHeader.CurrentRow.Cells["ID"].Value);
                    invokeTrxPrint(TrxId);
                }
            }
            else if (e.ColumnIndex == 2)
            {
                if (invokeTrxReopen == null)
                {
                    log.LogMethodExit(null);
                    return;
                }
                if (dgvTrxHeader.CurrentRow != null && (dgvTrxHeader.CurrentRow.Cells["Status"].Value.ToString() != Transaction.TrxStatus.CLOSED.ToString()
                     || (dgvTrxHeader.CurrentRow.Cells["Status"].Value.ToString() == Transaction.TrxStatus.CLOSED.ToString()
                            && (Convert.ToDateTime(dgvTrxHeader.CurrentRow.Cells["Date"].Value)
                                    < ((ServerDateTime.Now.Hour < 6) ? ServerDateTime.Now.AddDays(-1).Date.AddHours(6)
                                                         : ServerDateTime.Now.Date.AddHours(6)))
                            || (Convert.ToDateTime(dgvTrxHeader.CurrentRow.Cells["Date"].Value) > ((ServerDateTime.Now.Hour < 6) ? ServerDateTime.Now.Date.AddHours(6)
                                                         : ServerDateTime.Now.AddDays(1).Date.AddHours(6))
                               )
                         ))
                    )
                {
                    MessageBox.Show(Utilities.MessageUtils.getMessage(1417), "Transaction Re-open", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    if (dgvTrxHeader.CurrentRow != null)
                    {
                        int TrxId = Convert.ToInt32(dgvTrxHeader.CurrentRow.Cells["ID"].Value);
                        object orginalTrxID = Utilities.executeScalar(("select isnull(OriginalTrxId,0) as OriginalTrxId from trx_header where TrxId=@trxId"),
                                           new SqlParameter("@trxId", TrxId));
                        if (Convert.ToInt32(orginalTrxID) > 0)
                        {
                            MessageBox.Show(Utilities.MessageUtils.getMessage(1417), "Transaction Re-open", MessageBoxButtons.OK);
                            log.LogMethodExit(null);
                            return;
                        }
                        else
                        {
                            int id = Convert.ToInt32(Utilities.executeScalar(("select 1 from trx_header where OriginalTrxId = @trxId"), new SqlParameter("@trxId", TrxId)));
                            if (id > 0)
                            {
                                MessageBox.Show(Utilities.MessageUtils.getMessage(1417), "Transaction Re-open", MessageBoxButtons.OK);
                                log.LogMethodExit(null);
                                return;
                            }
                            if (MessageBox.Show(Utilities.MessageUtils.getMessage(1420), "Transaction Re-open", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                log.LogMethodExit(null);
                                return;
                            }
                            invokeTrxReopen(TrxId);
                        }
                    }
                }
            }
            else if (dgvTrxHeader.Columns[e.ColumnIndex].DataPropertyName == "Net_amount" && showAmountFieldsTransaction)
            {
                try
                {
                    int trxId = Convert.ToInt32(dgvTrxHeader["ID", e.RowIndex].Value);
                    TrxPayments tp = new TrxPayments(trxId, Utilities, authenticateManager);
                    tp.ShowDialog();
                    refreshHeader(pastTrx: rdPast.Checked, futureTrx: rdFuture3.Checked, futureAllTrx: rdFutureAll.Checked, todayTrx: rdToday.Checked); //Added on 30-dec-2016 for refreshing grid after closing payment form
                }
                catch (Exception ex)
                {
                    log.Error("Unable to Convert Data View Grid Transactions to Integer", ex);
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (dgvTrxHeader.Columns[e.ColumnIndex].DataPropertyName == "avg_disc_%")
            {
                try
                {
                    int trxId = Convert.ToInt32(dgvTrxHeader["ID", e.RowIndex].Value);
                    TrxDiscounts tp = new TrxDiscounts(trxId, Utilities);
                    tp.ShowDialog();
                }
                catch (Exception ex)
                {
                    log.LogMethodExit(ex);
                    MessageBox.Show(ex.Message);
                }
            }
            else if (dgvTrxHeader.Columns[e.ColumnIndex].DataPropertyName == "ID")
            {
                try
                {
                    int trxId = Convert.ToInt32(dgvTrxHeader["ID", e.RowIndex].Value);
                    using (TrxUserLogsUI trxUserLogs = new TrxUserLogsUI(Utilities, trxId))
                    {
                        trxUserLogs.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    log.LogMethodExit(ex);
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit(null);
        }

        private void rdToday_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor.Current = Cursors.WaitCursor;
            if (rdToday.Checked)
            {
                refreshHeader(pastTrx: rdPast.Checked, futureTrx: rdFuture3.Checked, futureAllTrx: rdFutureAll.Checked, todayTrx: rdToday.Checked);
            }

            Cursor.Current = Cursors.Default;
            log.LogMethodExit();
        }

        private void rdPast_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor.Current = Cursors.WaitCursor;
            if (rdPast.Checked)
                refreshHeader(pastTrx: rdPast.Checked, futureTrx: rdFuture3.Checked, futureAllTrx: rdFutureAll.Checked, todayTrx: rdToday.Checked);
            Cursor.Current = Cursors.Default;
            log.LogMethodExit();
        }

        private void rdFuture3_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor.Current = Cursors.WaitCursor;
            if (rdFuture3.Checked)
                refreshHeader(pastTrx: rdPast.Checked, futureTrx: rdFuture3.Checked, futureAllTrx: rdFutureAll.Checked, todayTrx: rdToday.Checked);
            Cursor.Current = Cursors.Default;
            log.LogMethodExit();
        }

        private void rdFutureAll_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor.Current = Cursors.WaitCursor;
            if (rdFutureAll.Checked)
                refreshHeader(pastTrx: rdPast.Checked, futureTrx: rdFuture3.Checked, futureAllTrx: rdFutureAll.Checked, todayTrx: rdToday.Checked);
            Cursor.Current = Cursors.Default;
            log.LogMethodExit();
        }

    }
}
