using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Product
{
    public partial class AccountingCodeCombinationUI : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        Utilities utilities;
        BindingSource accountingCodeCombinationListBS;
        int objectId;

        public AccountingCodeCombinationUI()
        { }

        public AccountingCodeCombinationUI(Utilities _Utilities, int id)
        {
            log.Debug("Starts-AccountingCodeCombinationUI(_Utilities) constructor.");
            InitializeComponent();
            utilities = _Utilities;
            objectId = id;
            utilities.setupDataGridProperties(ref dgvAccountingCodeCombination);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            log.Debug("Ends-AccountingCodeCombinationUI(_Utilities) constructor.");
        }

        private void AccountingCodeCombinationUI_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-AccountingCodeCombinationUI_Load() event.");
            PopulateAccountingCodeCombination();
            log.Debug("Starts-AccountingCodeCombinationUI_Load() event.");
        }

        private void PopulateAccountingCodeCombination()
        {
            log.Debug("Starts-PopulateAccountingCodeCombination() method.");
            try
            {
                PopulateObjectType();
                PopulateTransactionType();
                PopulateTax();

                AccountingCodeCombinationList accountingCodeCombinationList = new AccountingCodeCombinationList(machineUserContext);
                List<KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>> accountingCodeCombinationSearchParams = new List<KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>>();
                accountingCodeCombinationSearchParams.Add(new KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.OBJECTID, objectId.ToString()));
                List<AccountingCodeCombinationDTO> accountingCodeCombinationListOnDisplay = accountingCodeCombinationList.GetAllAccountingCode(accountingCodeCombinationSearchParams);
                accountingCodeCombinationListBS = new BindingSource();

                if (accountingCodeCombinationListOnDisplay != null)
                    accountingCodeCombinationListBS.DataSource = new SortableBindingList<AccountingCodeCombinationDTO>(accountingCodeCombinationListOnDisplay);
                else
                {
                    accountingCodeCombinationListBS.DataSource = new SortableBindingList<AccountingCodeCombinationDTO>();
                }

                accountingCodeCombinationListBS.AddingNew += dgvAccountingCodeCombination_BindingSourceAddNew;
                dgvAccountingCodeCombination.DataSource = accountingCodeCombinationListBS;
                log.Debug("Ends-PopulateAccountingCodeCombination() method.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Debug("Ends-PopulateAccountingCodeCombination() method.");
            }
        }

        private void dgvAccountingCodeCombination_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-dgvAccountingCodeCombination_BindingSourceAddNew() Event.");
            try
            {
                if (dgvAccountingCodeCombination.Rows.Count == accountingCodeCombinationListBS.Count)
                {
                    accountingCodeCombinationListBS.RemoveAt(accountingCodeCombinationListBS.Count - 1);
                }
                log.Debug("Ends-dgvAccountingCodeCombination_BindingSourceAddNew() Event.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Debug("Ends-dgvAccountingCodeCombination_BindingSourceAddNew() Event.");
            }
        }

        private void PopulateObjectType()
        {
            log.Debug("Starts-PopulateObjectType() method.");
            try
            {
                var ObjectTypeDictionary = new Dictionary<string, string>();
                ObjectTypeDictionary["Revenue"] = utilities.MessageUtils.getMessage("Revenue");
                ObjectTypeDictionary["Cost"] = utilities.MessageUtils.getMessage("Cost");
                ObjectTypeDictionary["Inventory Receipt"] = utilities.MessageUtils.getMessage("Inventory Receipt");
                ObjectTypeDictionary["Inventory Adjustments"] = utilities.MessageUtils.getMessage("Inventory Adjustments");
                ObjectTypeDictionary["Sales Invoice"] = utilities.MessageUtils.getMessage("Sales Invoice");
                ObjectTypeDictionary["Discount"] = utilities.MessageUtils.getMessage("Discount");

                objectTypeDataGridViewTextBoxColumn.DataSource = new BindingSource(ObjectTypeDictionary, null);
                objectTypeDataGridViewTextBoxColumn.DisplayMember = "Value";
                objectTypeDataGridViewTextBoxColumn.ValueMember = "Key";
                log.Debug("Ends-PopulateObjectType() method.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Debug("Ends-PopulateObjectType() method.");
            }
        }

        private void PopulateTransactionType()
        {
            log.Debug("Starts-PopulateTransactionType() Event.");
            try
            {
                var TransactionTypeDictionary = new Dictionary<string, string>();
                TransactionTypeDictionary["Debit"] = utilities.MessageUtils.getMessage("Debit");
                TransactionTypeDictionary["Debit /Cash"] = utilities.MessageUtils.getMessage("Debit /Cash");
                TransactionTypeDictionary["Credit /Revenue"] = utilities.MessageUtils.getMessage("Credit /Revenue");
                TransactionTypeDictionary["Credit /VAT"] = utilities.MessageUtils.getMessage("Credit /VAT");

                transactionTypeDataGridViewTextBoxColumn.DataSource = new BindingSource(TransactionTypeDictionary, null);
                transactionTypeDataGridViewTextBoxColumn.DisplayMember = "Value";
                transactionTypeDataGridViewTextBoxColumn.ValueMember = "Key";
                log.Debug("Ends-PopulateTransactionType() Event.");
            }
            catch (Exception ex)
            {
                log.Debug("Ends-PopulateTransactionType() Event.");
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateTax()
        {
            log.Debug("Starts-PopulateTax() method.");
            try
            {
                TaxList taxList = new TaxList(machineUserContext);
                List<TaxDTO> taxDTOList = new List<TaxDTO>();
                List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> taxSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                taxDTOList = taxList.GetAllTaxes(taxSearchParams);
                if (taxDTOList == null)
                {
                    taxDTOList = new List<TaxDTO>();
                }
                taxDTOList.Insert(0, new TaxDTO());
                taxDTOList[0].TaxName = "<Select>";
                taxDTOList[0].TaxId = -1;
                BindingSource taxListBS = new BindingSource();
                taxListBS.DataSource = taxDTOList;

                taxDataGridViewTextBoxColumn.DataSource = taxListBS;
                taxDataGridViewTextBoxColumn.ValueMember = "TaxId";
                taxDataGridViewTextBoxColumn.DisplayMember = "TaxName";
                log.Debug("Ends-PopulateTax() method.");
            }
            catch (Exception ex)
            {
                log.Debug("Ends-PopulateTax() method.");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click() Event.");
            try
            {
                BindingSource accountingCodeCombinationListBS = (BindingSource)dgvAccountingCodeCombination.DataSource;
                var accountingCodeCombinationListOnDisplay = (SortableBindingList<AccountingCodeCombinationDTO>)accountingCodeCombinationListBS.DataSource;
                if (accountingCodeCombinationListOnDisplay.Count > 0)
                {
                    foreach (AccountingCodeCombinationDTO accountingCodeCombinationDTO in accountingCodeCombinationListOnDisplay)
                    {
                        if (accountingCodeCombinationDTO.IsChanged)
                        {
                            accountingCodeCombinationDTO.ObjectId = objectId;
                            if (string.IsNullOrEmpty(accountingCodeCombinationDTO.AccountingCode))
                            {
                                MessageBox.Show("Please enter value for the code.");
                                return;
                            }
                        }
                        AccountingCodeCombination accountingCodeCombination = new AccountingCodeCombination(machineUserContext, accountingCodeCombinationDTO);
                        accountingCodeCombination.Save();
                    }
                    PopulateAccountingCodeCombination();
                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                log.Debug("Ends-btnSave_Click() Event.");
            }
            catch (Exception ex)
            {
                log.Debug("Ends-btnSave_Click() Event.");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRefresh_Click() Event.");
            PopulateAccountingCodeCombination();
            log.Debug("Ends-btnRefresh_Click() Event.");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnDelete_Click() event.");
            try
            {
                if (this.dgvAccountingCodeCombination.SelectedRows.Count <= 0 && this.dgvAccountingCodeCombination.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvAccountingCodeCombination.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvAccountingCodeCombination.SelectedCells)
                    {
                        dgvAccountingCodeCombination.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow accountingCodeCombinationRow in this.dgvAccountingCodeCombination.SelectedRows)
                {
                    if (accountingCodeCombinationRow.Cells[0].Value != null)
                    {
                        if (Convert.ToInt32(accountingCodeCombinationRow.Cells[0].Value) < 0)
                        {
                            dgvAccountingCodeCombination.Rows.RemoveAt(accountingCodeCombinationRow.Index);
                            rowsDeleted = true;
                        }
                        else
                        {
                            if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                BindingSource accountingCodeCombinationDTOListDTOBS = (BindingSource)dgvAccountingCodeCombination.DataSource;
                                var accountingCodeCombinationDTOList = (SortableBindingList<AccountingCodeCombinationDTO>)accountingCodeCombinationDTOListDTOBS.DataSource;
                                AccountingCodeCombinationDTO accountingCodeCombinationDTO = accountingCodeCombinationDTOList[accountingCodeCombinationRow.Index];
                                accountingCodeCombinationDTO.IsActive = false;
                                AccountingCodeCombination accountingCodeCombination = new AccountingCodeCombination(machineUserContext, accountingCodeCombinationDTO);
                                accountingCodeCombination.Save();
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                PopulateAccountingCodeCombination();
                log.Debug("Ends-btnDelete_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnDelete_Click() event with exception: " + ex.ToString());
                MessageBox.Show("Delete failed!!!.\n Error: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click() event.");
            this.Dispose();
            log.Debug("Ends-btnClose_Click() event.");
        }
    }
}
