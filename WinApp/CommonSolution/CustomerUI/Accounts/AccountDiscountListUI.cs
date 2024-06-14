/********************************************************************************************
 * Project Name - AccountDiscountListUI
 * Description  - AccountDiscountList UI
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 *********************************************************************************************
 *2.80.0     17-Feb-2019       Deeksha             Modified to Make DigitalSignage module as
 *                                                  read only in Windows Management Studio.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer.Accounts
{
    public partial class AccountDiscountListUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<DiscountsDTO> discountsDTOList;
        int accountId = -1;
        AccountDTO accountDTO;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Constructor of AccountDiscountListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="accountId">Account Id</param>
        public AccountDiscountListUI(Utilities utilities, int accountId)
        {
            log.LogMethodEntry(utilities, accountId);
            InitializeComponent();
            this.utilities = utilities;
            this.accountId = accountId;
            
            utilities.setupDataGridProperties(ref dgvAccountDiscountDTOList);
            if (utilities.ParafaitEnv.LoginID != "semnox" && utilities.getParafaitDefaults("ALLOW_MANUAL_CARD_UPDATE") == "N")
            {
                dgvAccountDiscountDTOList.ReadOnly = true;
                dgvAccountDiscountDTOList.AllowUserToAddRows = false;
            }
            utilities.setLanguage(this);
            expiryDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            lastUpdatedDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            ThemeUtils.SetupVisuals(this);
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void AccountDiscountListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            accountDiscountDTOListBS.DataSource = new SortableBindingList<AccountDiscountDTO>();
            LoadDiscountsDTOList();
            LoadAccountDiscountDTOList();
            log.LogMethodExit();
        }

        private void LoadAccountDiscountDTOList()
        {
            log.LogMethodEntry();
            AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountId, true, chbShowActiveEntries.Checked);
            this.accountDTO = accountBL.AccountDTO;
            SortableBindingList<AccountDiscountDTO> accountDiscountDTOSortableBindingList = null;
            if (accountDTO.AccountDiscountDTOList == null || accountDTO.AccountDiscountDTOList.Count == 0)
            {
                accountDiscountDTOSortableBindingList = new SortableBindingList<AccountDiscountDTO>();
            }
            else
            {
                if (cmbDiscount.SelectedValue != null && Convert.ToInt32(cmbDiscount.SelectedValue) != -1)
                {
                    accountDiscountDTOSortableBindingList = new SortableBindingList<AccountDiscountDTO>(accountDTO.AccountDiscountDTOList.FindAll((x) => x.DiscountId == Convert.ToInt32(cmbDiscount.SelectedValue)));
                }
                else
                {
                    accountDiscountDTOSortableBindingList = new SortableBindingList<AccountDiscountDTO>(accountDTO.AccountDiscountDTOList);
                }
            }
            accountDiscountDTOListBS.DataSource = accountDiscountDTOSortableBindingList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the discounts to the comboboxes
        /// </summary>
        private void LoadDiscountsDTOList()
        {
            log.LogMethodEntry();
            try
            {
                List<DiscountsDTO> transactionDiscountsDTOList = null;
                using(UnitOfWork unitOfWork = new UnitOfWork())
                {
                    DiscountsListBL discountsListBL = new DiscountsListBL(utilities.ExecutionContext, unitOfWork);
                    discountsDTOList = new List<DiscountsDTO>();
                    SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                    if (chbShowActiveEntries.Checked)
                    {
                        searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                    }
                    searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, DiscountsBL.DISCOUNT_TYPE_TRANSACTION));
                    transactionDiscountsDTOList = discountsListBL.GetDiscountsDTOList(searchParameters);
                    if (transactionDiscountsDTOList != null)
                    {
                        discountsDTOList.AddRange(transactionDiscountsDTOList);
                    }
                    searchParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                    if (chbShowActiveEntries.Checked)
                    {
                        searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                    }
                    searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, DiscountsBL.DISCOUNT_TYPE_GAMEPLAY));
                    List<DiscountsDTO> gamePlayDiscountsDTOList = discountsListBL.GetDiscountsDTOList(searchParameters);
                    if (gamePlayDiscountsDTOList != null)
                    {
                        discountsDTOList.AddRange(gamePlayDiscountsDTOList);
                    }
                }
                
                
                discountsDTOList.Insert(0, new DiscountsDTO());
                discountsDTOList[0].DiscountId = -1;
                discountsDTOList[0].DiscountName = "<SELECT>";

                BindingSource bs = new BindingSource();
                bs.DataSource = discountsDTOList;
                cmbDiscount.DisplayMember = "DiscountName";
                cmbDiscount.ValueMember = "DiscountId";
                cmbDiscount.DataSource = bs;

                bs = new BindingSource();
                bs.DataSource = discountsDTOList;
                discountIdDataGridViewComboBoxColumn.DataSource = bs;
                discountIdDataGridViewComboBoxColumn.ValueMember = "DiscountId";
                discountIdDataGridViewComboBoxColumn.DisplayMember = "DiscountName";
            }
            catch (Exception e)
            {
                log.Error("Error occured while loading discounts", e);
            }
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadAccountDiscountDTOList();
            log.LogMethodExit();
        }

        private void dgvAccountDiscountDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvAccountDiscountDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            chbShowActiveEntries.Checked = true;
            cmbDiscount.SelectedValue = -1;
            RefreshData();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAccountDiscountDTOList.EndEdit();
            SortableBindingList<AccountDiscountDTO> accountDiscountDTOSortableList = (SortableBindingList<AccountDiscountDTO>)accountDiscountDTOListBS.DataSource;
            AccountDiscountBL accountDiscountBL;
            bool error = false;
            if (accountDiscountDTOSortableList != null)
            {
                for (int i = 0; i < accountDiscountDTOSortableList.Count; i++)
                {
                    if (accountDiscountDTOSortableList[i].IsChanged)
                    {
                        accountDiscountBL = new AccountDiscountBL(utilities.ExecutionContext, accountDiscountDTOSortableList[i]);
                        List<ValidationError> validationErrorList = accountDiscountBL.Validate();
                        if (validationErrorList.Count > 0)
                        {
                            dgvAccountDiscountDTOList.Rows[i].Selected = true;
                            MessageBox.Show(validationErrorList[0].Message);
                            log.LogMethodExit("", "Validation error");
                            error = true;
                            break;
                        }
                    }
                }
                if(!error)
                {
                    if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Count > 0)
                    {
                        foreach (var accountDiscountDTO in accountDiscountDTOSortableList)
                        {
                            if (accountDTO.AccountDiscountDTOList.Contains(accountDiscountDTO) == false)
                            {
                                accountDTO.AccountDiscountDTOList.Add(accountDiscountDTO);
                            }
                        }
                    }
                    else
                    {
                        accountDTO.AccountDiscountDTOList = new List<AccountDiscountDTO>(accountDiscountDTOSortableList);
                    }
                    SqlConnection sqlConnection = utilities.getConnection();
                    SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                    try
                    {
                        AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountDTO);
                        accountBL.Save(sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch (ValidationException ex)
                    {
                        error = true;
                        sqlTransaction.Rollback();
                        log.LogVariableState("accountDTO", accountDTO);
                        log.Error("Validation failed", ex);
                        log.Error(ex.GetAllValidationErrorMessages());
                        MessageBox.Show(ex.GetAllValidationErrorMessages());
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        error = true;
                        log.Error("Error while saving accountDiscount.", ex);
                        MessageBox.Show(utilities.MessageUtils.getMessage(718));
                    }
                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            if (!error)
            {
                btnSearch.PerformClick();
            }
            else
            {
                dgvAccountDiscountDTOList.Update();
                dgvAccountDiscountDTOList.Refresh();
            }
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvAccountDiscountDTOList.SelectedRows.Count <= 0 && dgvAccountDiscountDTOList.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDB = false;
            if (this.dgvAccountDiscountDTOList.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvAccountDiscountDTOList.SelectedCells)
                {
                    if(dgvAccountDiscountDTOList.Rows[cell.RowIndex].IsNewRow == false)
                    {
                        dgvAccountDiscountDTOList.Rows[cell.RowIndex].Selected = true;
                    }
                }
            }
            foreach (DataGridViewRow row in dgvAccountDiscountDTOList.SelectedRows)
            {
                if(row.IsNewRow == false)
                {
                    if (Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                    {
                        dgvAccountDiscountDTOList.Rows.RemoveAt(row.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            refreshFromDB = true;
                            SortableBindingList<AccountDiscountDTO> accountDiscountDTOSortableList = (SortableBindingList<AccountDiscountDTO>)accountDiscountDTOListBS.DataSource;
                            AccountDiscountDTO accountDiscountDTO = accountDiscountDTOSortableList[row.Index];
                            accountDiscountDTO.IsActive = false;
                        }
                    }
                }
            }
            if(confirmDelete && refreshFromDB)
            {
                SqlConnection sqlConnection = utilities.getConnection();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountDTO);
                    accountBL.Save(sqlTransaction);
                    sqlTransaction.Commit();
                }
                catch (ValidationException ex)
                {
                    sqlTransaction.Rollback();
                    log.LogVariableState("accountDTO", accountDTO);
                    log.Error("Validation failed", ex);
                    log.Error(ex.GetAllValidationErrorMessages());
                    MessageBox.Show(ex.GetAllValidationErrorMessages());
                }
                catch (Exception ex)
                {
                    sqlTransaction.Rollback();
                    log.Error("Error while saving accountDiscount.", ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                }
            }
            if (rowsDeleted == true)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            }
            if (refreshFromDB == true)
            {
                btnSearch.PerformClick();
            }
            log.LogMethodExit();
        }

        private void accountDiscountDTOListBS_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            AccountDiscountDTO accountDiscountDTO = new AccountDiscountDTO();
            accountDiscountDTO.AccountId = Convert.ToInt32(accountId);
            e.NewObject = accountDiscountDTO;
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableCardsModule)
            {
                dgvAccountDiscountDTOList.AllowUserToAddRows = true;
                dgvAccountDiscountDTOList.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvAccountDiscountDTOList.AllowUserToAddRows = false;
                dgvAccountDiscountDTOList.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
