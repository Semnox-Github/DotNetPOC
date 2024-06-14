/********************************************************************************************
 * Project Name - CustomerUI
 * Description  - Account list ui
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *2.70        10-Jul-2019       Lakshminarayana     Modified for adding ultralight c card support.
 *2.80        17-Feb-2019       Deeksha             Modified to Make Cards module as
 *                                                  read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Account List UI class
    /// </summary>
    public partial class AccountListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int totalNoOfPages = 0;
        private int currentPage = 0;
        private int pageSize = 20;
        private AccountSearchCriteria accountAdvancedSearchCriteria;
        private DeviceClass readerDevice;
        private int portNumber;
        COMPort serialPort;
        bool enableEditing = true;
        private readonly TagNumberParser tagNumberParser;
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public AccountListUI(Utilities utilities, DeviceClass readerDevice, int portNumber, string tagNumber = "")
        {
            log.LogMethodEntry(utilities, readerDevice, portNumber, tagNumber);
            InitializeComponent();
            this.utilities = utilities;
            this.readerDevice = readerDevice;
            this.portNumber = portNumber;
            utilities.setupDataGridProperties(ref dgvAccountDTOList);
            utilities.setLanguage(this);

            issueDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            loyaltyPointsDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            refundDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            faceValueDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            refundAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            ticketCountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            creditsDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            courtesyDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            bonusDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            creditsPlayedDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            timeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            technicianCardDataGridViewComboBoxColumn.DataSource = GetTechnicianCardDataSource();
            tagNumberDataGridViewTextBoxColumn.DefaultCellStyle.ForeColor = Color.Blue;
            expiryDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            dgvAccountDTOList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            technicianCardDataGridViewComboBoxColumn.ValueMember = "Key";
            technicianCardDataGridViewComboBoxColumn.DisplayMember = "Value";
            tagNumberParser = new TagNumberParser(utilities.ExecutionContext);
            TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(utilities.ExecutionContext);
            txtCardNumber.MaxLength = tagNumberLengthList.MaximumValidTagNumberLength;
            try
            {
                ListenSerialPort();
            }
            catch { }
            if(string.IsNullOrWhiteSpace(tagNumber) == false)
            {
                txtCardNumber.Text = tagNumber;
                chkRoamingCards.Checked = true;
                chkValidRecords.Checked = true;
                cardSwiped(tagNumber);
                btnCustomer.Visible = false;
                btnNewCard.Visible = false;
                btnUpdateCard.Visible = false;
                groupBox1.Visible = false;
                customerNameDataGridViewTextBoxColumn.Visible = false;
                enableEditing = false;
            }

            creditPlusDataGridViewButtonColumn.Text = "...";
            creditPlusDataGridViewButtonColumn.UseColumnTextForButtonValue = true;
            creditPlusDataGridViewButtonColumn.Width = 30;

            if (readerDevice != null)
                readerDevice.Register(new EventHandler(CardScanCompleteEventHandle));
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            if (!(e is DeviceScannedEventArgs)) return;
            DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                
            TagNumber tagNumber;
            string scannedTagNumber = checkScannedEvent.Message;
            DeviceClass encryptedTagDevice = sender as DeviceClass;
            if (tagNumberParser.IsTagDecryptApplicable(encryptedTagDevice, checkScannedEvent.Message.Length))
            {
                string decryptedTagNumber = string.Empty;
                try
                {
                    decryptedTagNumber = tagNumberParser.GetDecryptedTagData(encryptedTagDevice, checkScannedEvent.Message);
                }
                catch (Exception ex)
                {
                    log.LogVariableState("Decrypted Tag Number result: ", ex);
                    MessageBox.Show(ex.Message);
                    return;
                }
                try
                {
                    scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, utilities.ParafaitEnv.SiteId);
                }
                catch (ValidationException ex)
                {
                    log.LogVariableState("Decrypted Tag Number validation: ", ex);
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (Exception ex)
                {
                    log.LogVariableState("Decrypted Tag Number validation: ", ex);
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
            {
                string message = tagNumberParser.Validate(scannedTagNumber);
                MessageBox.Show(message);
                log.LogMethodExit(null, "Invalid Tag Number.");
                return;
            }
            cardSwiped(tagNumber.Value);
        }

        private void cardSwiped(string CardNumber)
        {
            txtCardNumber.Text = CardNumber;
        }

        private void ListenSerialPort()
        {
            if (portNumber == 0)
                return;

            serialPort = new COMPort(portNumber);
            serialPort.setReceiveAction = dataReceived;
            serialPort.Open();
        }

        private void dataReceived()
        {
            string receivedData = serialPort.ReceivedData;

            this.Invoke((MethodInvoker)delegate
            {
                txtCardNumber.Text = receivedData;
            });
        }

        private List<KeyValuePair<string, string>> GetTechnicianCardDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> techCardDataSource = new List<KeyValuePair<string, string>>();
            techCardDataSource.Add(new KeyValuePair<string, string>("N", MessageContainerList.GetMessage(utilities.ExecutionContext, "None")));
            techCardDataSource.Add(new KeyValuePair<string, string>("Y", MessageContainerList.GetMessage(utilities.ExecutionContext, "Staff Card")));
            techCardDataSource.Add(new KeyValuePair<string, string>("D", MessageContainerList.GetMessage(utilities.ExecutionContext, "Enable / Disable Gameplay")));
            log.LogMethodExit(techCardDataSource);
            return techCardDataSource;
        }

        private async void AccountListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lblPage.Text = "";
            try
            {
                OnDataLoadStart();
                await RefreshData();
                OnDataLoadComplete();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading account list.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void OnDataLoadStart()
        {
            log.LogMethodEntry();
            DisableControls();
            lblStatus.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);
            log.LogMethodExit();
        }

        private void OnDataLoadComplete()
        {
            log.LogMethodEntry();
            lblStatus.Text = "";
            EnableControls();
            DisplayCurrentPageCount();
            UpdatePagingControls();
            log.LogMethodExit();
        }

        private void DisableControls()
        {
            log.LogMethodEntry();
            btnClose.Enabled = false;
            btnUpdateCard.Enabled = false;
            btnNewCard.Enabled = false;
            btnCardActivity.Enabled = false;
            btnCustomer.Enabled = false;
            btnExportToExcel.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            btnFirst.Enabled = false;
            btnLast.Enabled = false;
            btnSearch.Enabled = false;
            btnAdvancedSearch.Enabled = false;
            btnClear.Enabled = false;
            chkValidRecords.Enabled = false;
            chkVipCustomer.Enabled = false;
            chkRoamingCards.Enabled = false;
            chkTechCards.Enabled = false;
            txtCardNumber.Enabled = false;
            txtCustomerName.Enabled = false;
            dtpFromIssueDate.Enabled = false;
            dtpToIssueDate.Enabled = false;
            RefreshControls();
            log.LogMethodExit();
        }

        private void EnableControls()
        {
            log.LogMethodEntry();
            btnClose.Enabled = true;
            btnUpdateCard.Enabled = true;
            if (managementStudioSwitch.EnableCardsModule)
            {
                btnNewCard.Enabled = true;
            }
            btnCardActivity.Enabled = true;
            btnCustomer.Enabled = true;
            btnExportToExcel.Enabled = true;
            btnNext.Enabled = true;
            btnPrevious.Enabled = true;
            btnFirst.Enabled = true;
            btnLast.Enabled = true;
            btnSearch.Enabled = true;
            btnAdvancedSearch.Enabled = true;
            btnClear.Enabled = true;
            chkValidRecords.Enabled = true;
            chkVipCustomer.Enabled = true;
            chkRoamingCards.Enabled = true;
            chkTechCards.Enabled = true;
            txtCardNumber.Enabled = true;
            txtCustomerName.Enabled = true;
            dtpFromIssueDate.Enabled = true;
            dtpToIssueDate.Enabled = true;
            RefreshControls();
            log.LogMethodExit();
        }

        private void RefreshControls()
        {
            log.LogMethodEntry();
            btnClose.Refresh();
            btnUpdateCard.Refresh();
            btnNewCard.Refresh();
            btnCardActivity.Refresh();
            btnCustomer.Refresh();
            btnExportToExcel.Refresh();
            btnNext.Refresh();
            btnPrevious.Refresh();
            btnFirst.Refresh();
            btnLast.Refresh();
            btnSearch.Refresh();
            btnAdvancedSearch.Refresh();
            btnClear.Refresh();
            chkValidRecords.Refresh();
            chkVipCustomer.Refresh();
            chkRoamingCards.Refresh();
            chkTechCards.Refresh();
            txtCardNumber.Refresh();
            txtCustomerName.Refresh();
            dtpFromIssueDate.Refresh();
            dtpToIssueDate.Refresh();
            lblStatus.Refresh();
            log.LogMethodExit();
        }

        private void DisplayCurrentPageCount()
        {
            log.LogMethodEntry();
            if (totalNoOfPages > 0)
            {
                lblPage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Page") + " " + (currentPage + 1) + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "of") + " " + totalNoOfPages;
            }
            else
            {
                lblPage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Page") + " " + (currentPage) + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "of") + " " + totalNoOfPages;
            }
            log.LogMethodExit();
        }

        private void UpdatePagingControls()
        {
            log.LogMethodEntry();
            if (totalNoOfPages == 0)
            {
                btnFirst.Enabled = false;
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
            }
            else
            {
                if (currentPage == 0)
                {
                    btnPrevious.Enabled = false;
                    btnFirst.Enabled = false;
                }
                else
                {
                    btnPrevious.Enabled = true;
                    btnFirst.Enabled = true;
                }
                if (currentPage == (totalNoOfPages - 1))
                {
                    btnNext.Enabled = false;
                    btnLast.Enabled = false;
                }
                else
                {
                    btnNext.Enabled = true;
                    btnLast.Enabled = true;
                }
            }
            log.LogMethodExit();
        }

        private async Task RefreshData()
        {
            log.LogMethodEntry();
            currentPage = 0;
            AccountSearchCriteria accountSearchCriteria = GetAccountSearchCriteria();
            int totalNoOfAccounts = await Task<int>.Factory.StartNew(() => { return GetAccountCount(accountSearchCriteria); });
            log.LogVariableState("totalNoOfAccounts", totalNoOfAccounts);
            totalNoOfPages = (totalNoOfAccounts / pageSize) + ((totalNoOfAccounts % pageSize) > 0 ? 1 : 0);
            log.LogVariableState("totalNoOfPages", totalNoOfPages);
            await LoadAccountDTOList();
            log.LogMethodExit();
        }

        private AccountSearchCriteria GetAccountSearchCriteria()
        {
            log.LogMethodEntry();
            AccountSearchCriteria searchCriteria = null;
            if (this.accountAdvancedSearchCriteria != null)
            {
                searchCriteria = accountAdvancedSearchCriteria;
            }
            else
            {
                searchCriteria = new AccountSearchCriteria();
                if (chkValidRecords.Checked)
                {
                    searchCriteria.And(AccountDTO.SearchByParameters.VALID_FLAG, Operator.EQUAL_TO, "Y");
                }
                if (chkVipCustomer.Checked)
                {
                    searchCriteria.And(AccountDTO.SearchByParameters.VIP_CUSTOMER, Operator.EQUAL_TO, "Y");
                }
                if (chkRoamingCards.Checked == false)
                {
                    searchCriteria.And(new AccountSearchCriteria(AccountDTO.SearchByParameters.SITE_ID, Operator.IS_NULL).Or(AccountDTO.SearchByParameters.SITE_ID, Operator.EQUAL_TO, utilities.ExecutionContext.GetSiteId()));
                }
                if (chkTechCards.Checked)
                {
                    searchCriteria.And(AccountDTO.SearchByParameters.TECHNICIAN_CARD, Operator.NOT_EQUAL_TO, "N");
                }
                if (string.IsNullOrWhiteSpace(txtCardNumber.Text) == false)
                {
                    searchCriteria.And(AccountDTO.SearchByParameters.TAG_NUMBER, Operator.LIKE, txtCardNumber.Text);
                }
                if (string.IsNullOrWhiteSpace(txtCustomerName.Text) == false)
                {
                    searchCriteria.And(AccountDTO.SearchByParameters.CUSTOMER_NAME, Operator.LIKE, txtCustomerName.Text);
                }
                if (chkTechCards.Checked == false && string.IsNullOrWhiteSpace(txtCardNumber.Text) && string.IsNullOrWhiteSpace(txtCustomerName.Text.Trim()))
                {
                    searchCriteria.And(AccountDTO.SearchByParameters.ISSUE_DATE, Operator.GREATER_THAN_OR_EQUAL_TO, dtpFromIssueDate.Value.Date);
                    searchCriteria.And(AccountDTO.SearchByParameters.ISSUE_DATE, Operator.LESSER_THAN, dtpToIssueDate.Value.Date.AddDays(1));
                }
            }
            log.LogMethodExit(searchCriteria);
            return searchCriteria;
        }

        private int GetAccountCount(AccountSearchCriteria accountSearchCriteria)
        {
            log.LogMethodEntry(accountSearchCriteria);
            int accountCount = 0;
            try
            {
                AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
                accountCount = accountListBL.GetAccountCount(accountSearchCriteria);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while getting account count", ex);
            }
            log.LogMethodExit(accountCount);
            return accountCount;
        }

        private async Task LoadAccountDTOList()
        {
            log.LogMethodEntry();
            AccountSearchCriteria accountSearchCriteria = GetAccountSearchCriteria();
            IList<AccountDTO> accountDTOList = null;
            if (totalNoOfPages > 0)
            {
                accountSearchCriteria.OrderBy(AccountDTO.SearchByParameters.ISSUE_DATE, OrderByType.DESC);
                accountSearchCriteria.OrderBy(AccountDTO.SearchByParameters.ACCOUNT_ID, OrderByType.DESC);
                accountSearchCriteria.Paginate(currentPage, pageSize);
                accountDTOList = await Task<List<AccountDTO>>.Factory.StartNew(() => { return GetAccountDTOList(accountSearchCriteria); });
            }
            if (accountDTOList == null)
            {
                accountDTOList = new SortableBindingList<AccountDTO>();
            }
            else
            {
                accountDTOList = new SortableBindingList<AccountDTO>(accountDTOList);
            }
            accountDTOListBS.DataSource = accountDTOList;
            log.LogMethodExit();
        }

        private List<AccountDTO> GetAccountDTOList(AccountSearchCriteria accountSearchCriteria)
        {
            log.LogMethodEntry(accountSearchCriteria);
            List<AccountDTO> accountDTOList = null;
            try
            {
                AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
                accountDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria, false, false);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the accounts", ex);
            }
            log.LogMethodExit(accountDTOList);
            return accountDTOList;
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            accountAdvancedSearchCriteria = null;
            OnDataLoadStart();
            await RefreshData();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private void dgvAccountList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144).Replace("&1", dgvAccountDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private async void btnFirst_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                currentPage = 0;
                OnDataLoadStart();
                await LoadAccountDTOList();
                OnDataLoadComplete();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the first page.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private async void btnLast_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                currentPage = totalNoOfPages - 1;
                OnDataLoadStart();
                await LoadAccountDTOList();
                OnDataLoadComplete();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the last page.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private async void btnPrevious_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                currentPage--;
                if (currentPage < 0)
                {
                    currentPage = 0;
                }
                OnDataLoadStart();
                await LoadAccountDTOList();
                OnDataLoadComplete();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the previous page.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                currentPage++;
                if (currentPage >= totalNoOfPages)
                {
                    currentPage = totalNoOfPages - 1;
                }
                OnDataLoadStart();
                await LoadAccountDTOList();
                OnDataLoadComplete();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the next page.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private async void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (accountAdvancedSearchCriteria == null)
                {
                    accountAdvancedSearchCriteria = new AccountSearchCriteria();
                }
                using(AdvancedSearchUI advancedSearchUI = new AdvancedSearchUI(utilities, accountAdvancedSearchCriteria))
                {
                    if (advancedSearchUI.ShowDialog() == DialogResult.OK)
                    {
                        OnDataLoadStart();
                        await RefreshData();
                        OnDataLoadComplete();
                    }
                    else
                    {
                        accountAdvancedSearchCriteria = null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while performing advanced search.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private async void btnUpdateCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (accountDTOListBS.Current != null &&
                accountDTOListBS.Current is AccountDTO)
            {
                AccountDTO accountDTO = accountDTOListBS.Current as AccountDTO;
                if (accountDTO.AccountId != -1)
                {
                    using (IssueAccountUI issueAccountUI = new IssueAccountUI(utilities, accountDTO.AccountId, readerDevice, portNumber))
                    {
                        if (issueAccountUI.ShowDialog() == DialogResult.OK)
                        {
                            OnDataLoadStart();
                            await LoadAccountDTOList();
                            OnDataLoadComplete();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private async void btnNewCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (serialPort != null)
                serialPort.Close();
            using (IssueAccountUI issueAccountUI = new IssueAccountUI(utilities, -1, readerDevice, portNumber))
            {
                if (issueAccountUI.ShowDialog() == DialogResult.OK)
                {
                    OnDataLoadStart();
                    await LoadAccountDTOList();
                    OnDataLoadComplete();
                }
            }
            if (serialPort != null)
                serialPort.Open();
            log.LogMethodExit();
        }

        private void btnCardActivity_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ShowAccountActivityView();
            log.LogMethodExit();
        }

        private async void btnCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (accountDTOListBS.Current != null &&
                        accountDTOListBS.Current is AccountDTO)
                {
                    AccountDTO accountDTO = accountDTOListBS.Current as AccountDTO;
                    if (accountDTO.AccountId != -1)
                    {
                        CustomerDTO customerDTO = null;
                        if (accountDTO.CustomerId != -1)
                        {
                            CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, accountDTO.CustomerId);
                            customerDTO = customerBL.CustomerDTO;
                        }
                        else
                        {
                            customerDTO = new CustomerDTO();
                        }
                        CustomerDetailForm customerDetailForm = new CustomerDetailForm(utilities, customerDTO, MessageBox.Show, false);
                        if (customerDetailForm.ShowDialog() == DialogResult.OK)
                        {
                            if (customerDTO.Id != customerDetailForm.CustomerDTO.Id)
                            {
                                customerDTO = customerDetailForm.CustomerDTO;
                            }
                            SqlConnection sqlConnection = utilities.getConnection();
                            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                            try
                            {
                                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDTO);
                                customerBL.Save(sqlTransaction);
                                if (accountDTO.CustomerId != customerDTO.Id)
                                {
                                    accountDTO.CustomerId = customerDTO.Id;
                                    AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountDTO);
                                    accountBL.Save(sqlTransaction);
                                }
                                sqlTransaction.Commit();
                            }
                            catch (ValidationException ex)
                            {
                                log.LogVariableState("accountDTO", accountDTO);
                                log.Error("Validation failed", ex);
                                log.Error(ex.GetAllValidationErrorMessages());
                                sqlTransaction.Rollback();
                                MessageBox.Show(ex.GetAllValidationErrorMessages());
                            }
                            catch (Exception ex)
                            {
                                log.LogVariableState("accountDTO", accountDTO);
                                log.Error("Validation failed", ex);
                                sqlTransaction.Rollback();
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                sqlConnection.Close();
                            }
                            OnDataLoadStart();
                            await LoadAccountDTOList();
                            OnDataLoadComplete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating customer information.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
            int totalNoOfAccount = accountListBL.GetAccountCount(GetAccountSearchCriteria());
            if(totalNoOfAccount <=0)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1593));
                log.LogMethodExit(null, "no accounts are selected to export. change the filter condition");
                return;
            }
            try
            {
                using (AccountExportUI accountExportUI = new AccountExportUI(utilities, GetAccountSearchCriteria()))
                {
                    accountExportUI.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while exporting account data.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            chkRoamingCards.Checked = true;
            chkTechCards.Checked = false;
            chkValidRecords.Checked = true;
            chkVipCustomer.Checked = false;
            txtCardNumber.Text = "";
            txtCustomerName.Text = "";
            dtpFromIssueDate.Value = dtpToIssueDate.Value = DateTime.Now;
            accountAdvancedSearchCriteria = null;
            log.LogMethodExit();
        }

        private void txtCardNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                log.LogVariableState("e.KeyChar", e.KeyChar);
                if (char.IsLetterOrDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                    e.KeyChar = char.ToUpper(e.KeyChar);
                else
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while changing tag number to CAPS", ex);
            }
            log.LogMethodExit();
        }

        private void dgvAccountDTOList_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            try
            {
                if (e.ColumnIndex == tagNumberDataGridViewTextBoxColumn.Index)
                {

                    dgvAccountDTOList.Cursor = Cursors.Hand;
                    dgvAccountDTOList[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvAccountDTOList.DefaultCellStyle.Font, FontStyle.Underline);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while changing the font of tagnumber column", ex);
            }
            log.LogMethodExit();
        }

        private void dgvAccountDTOList_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            try
            {
                if (e.ColumnIndex == tagNumberDataGridViewTextBoxColumn.Index)
                {
                    dgvAccountDTOList.Cursor = Cursors.Default;
                    dgvAccountDTOList[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvAccountDTOList.DefaultCellStyle.Font, FontStyle.Regular);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while changing the font of tagnumber column", ex);
            }
            log.LogMethodExit();
        }

        private void dgvAccountDTOList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            if (e.ColumnIndex == tagNumberDataGridViewTextBoxColumn.Index)
            {
                ShowAccountActivityView();
            }
            else if(e.ColumnIndex == creditPlusDataGridViewButtonColumn.Index)
            {
                try
                {
                    if (accountDTOListBS.Current != null &&
                    accountDTOListBS.Current is AccountDTO)
                    {
                        AccountDTO accountDTO = accountDTOListBS.Current as AccountDTO;
                        if (accountDTO.AccountId != -1)
                        {
                            using (AccountCreditPlusListUI accountCreditPlusListUI = new AccountCreditPlusListUI(utilities, accountDTO.AccountId, enableEditing))
                            {
                                accountCreditPlusListUI.ShowDialog();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while showing account creditplus view", ex);
                }
                
            }
            log.LogMethodExit();
        }

        private void ShowAccountActivityView()
        {
            log.LogMethodEntry();
            try
            {
                if (accountDTOListBS.Current != null &&
                accountDTOListBS.Current is AccountDTO)
                {
                    AccountDTO accountDTO = accountDTOListBS.Current as AccountDTO;
                    if (accountDTO.AccountId != -1)
                    {
                        using (AccountActivityListUI accountActivityListUI = new AccountActivityListUI(utilities, accountDTO.AccountId, accountDTO.TagNumber))
                        {
                            accountActivityListUI.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while showing account activity view", ex);
            }
            log.LogMethodExit();
        }

        private void dgvAccountDTOList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            if (e.ColumnIndex == siteIdDataGridViewTextBoxColumn.Index)
            {
                if (e.Value != null)
                {
                    int value = Convert.ToInt32(e.Value);
                    if (value == -1)
                    {
                        e.Value = "";
                        e.FormattingApplied = true;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvAccountDTOList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            if(enableEditing)
            {
                btnUpdateCard_Click(null, null);
            }
            log.LogMethodExit();
        }
        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableCardsModule)
            {
                dgvAccountDTOList.AllowUserToAddRows = true;
                dgvAccountDTOList.ReadOnly = false;
                btnUpdateCard.Enabled = true;
                btnNewCard.Enabled = true;
            }
            else
            {
                dgvAccountDTOList.AllowUserToAddRows = false;
                dgvAccountDTOList.ReadOnly = true;
                btnUpdateCard.Enabled = false;
                btnNewCard.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}