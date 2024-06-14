/********************************************************************************************
 * Project Name - CustomerUI
 * Description  - Account list UI
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        10-Sept-2019      Girish Kundar       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Customer.UI
{
    public partial class CustomerAssociatedCardsUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SortableBindingList<AccountDTO> accountDTOList;
        private CustomerDTO customerDTO;
        private bool enableEditing = true;
        private string cardNumber = string.Empty;
        public SortableBindingList<AccountDTO> AccountDTOList
        {
            get { return accountDTOList; }
            set { accountDTOList = value; LoadAccountDTOList(); }
        }

        public CustomerDTO CustomerDTO
        {
            get { return customerDTO; }
            set { customerDTO = value; LoadAccountDTOList(); }
        }
        public string CardNumber
        {
            get { return cardNumber; }
            set  {  cardNumber = value; }
        }

        public CustomerAssociatedCardsUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities, customerDTO);
            InitializeComponent();
            this.utilities = utilities;
            this.customerDTO = null;
            SetGridCellStyle();
            enableEditing = false;
            CheckAddCreditPlusInCardInfo();
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks for ADD_CREDITPLUS_IN_CARD_INFO" == "Y"
        /// </summary>
        private void CheckAddCreditPlusInCardInfo()
        {
            log.LogMethodEntry();
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADD_CREDITPLUS_IN_CARD_INFO") == "Y")
            {
                totalCreditPlusBalanceDataGridViewtextBoxColumn.Visible = true;
                creditsDataGridViewTextBoxColumn.Visible = false;
            }
            else
            {
                totalCreditPlusBalanceDataGridViewtextBoxColumn.Visible = false;
                creditsDataGridViewTextBoxColumn.Visible = true;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Sets the cell style for the grid view columns
        /// </summary>
        private void SetGridCellStyle()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvAccountDTOList);
            utilities.setLanguage(this);
            issueDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            loyaltyPointsDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            refundDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            faceValueDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            refundAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            ticketCountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            creditsDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            courtesyDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            bonusDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            creditsPlayedDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            timeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            technicianCardDataGridViewComboBoxColumn.DataSource = GetTechnicianCardDataSource();
            expiryDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            dgvAccountDTOList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            technicianCardDataGridViewComboBoxColumn.ValueMember = "Key";
            technicianCardDataGridViewComboBoxColumn.DisplayMember = "Value";
            TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(utilities.ExecutionContext);
            creditPlusDataGridViewButtonColumn.Text = "...";
            creditPlusDataGridViewButtonColumn.UseColumnTextForButtonValue = true;
            creditPlusDataGridViewButtonColumn.Width = 30;
            dgvAccountDTOList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            log.LogMethodExit();
        }
        /// <summary>
        ///  Gets the Data Source for the TechnicianCard combo box.
        /// </summary>
        /// <returns>List<KeyValuePair<string, string>></returns>
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

        /// <summary>
        /// This method Gets the AccountDTOList for  the Given Customer Id.
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>IList<AccountDTO></returns>
        private IList<AccountDTO> GetAccountDTOList(int customerId)
        {
            log.LogMethodEntry(customerId);
            AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
            List<KeyValuePair<AccountDTO.SearchByParameters, string>> accountSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
            accountSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
            List<AccountDTO> accountDTOListForDisplay = accountListBL.GetAccountDTOList(accountSearchParameters, true, true);
            if (accountDTOListForDisplay == null)
            {
                accountDTOListForDisplay = new List<AccountDTO>();
                log.LogMethodExit(null, "No Active Cards for View ");
            }
            else
            {
                accountDTOListForDisplay = accountDTOListForDisplay.OrderByDescending(x => x.PrimaryAccount).ToList(); // To display Primary card on top
                accountDTOList = new SortableBindingList<AccountDTO>(accountDTOListForDisplay);
            }

            log.LogMethodExit(accountDTOList);
            return accountDTOList;
        }

        /// <summary>
        /// This methods sets the Binding Source to dgvAccountList.
        /// </summary>
        private void LoadAccountDTOList()
        {
            log.LogMethodEntry();
            if (customerDTO == null && accountDTOList == null)
            {
                log.LogMethodExit(null, "CustomerDTO and accountDTOList is Null");
                return;
            }
            else if (customerDTO != null)
            {
                SortableBindingList<AccountDTO> accountDTOListForDisplay = (SortableBindingList<AccountDTO>)GetAccountDTOList(customerDTO.Id);
                accountDTOListBS.DataSource = accountDTOListForDisplay;
            }
            else
            {
                accountDTOListBS.DataSource = accountDTOList;
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.OK;
            log.LogMethodExit();
        }

        /// <summary>
        /// This code will find the Primary Card If any and set the background color to Aquamarine.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvAccountDTOList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAccountDTOList.ClearSelection();
            if (accountDTOListBS.Count > 0)
            {
                List<AccountDTO> accountDTOList = ((SortableBindingList<AccountDTO>)accountDTOListBS.DataSource).ToList();
                if (accountDTOList != null && accountDTOList.Any())
                {
                    int foundIndex = accountDTOList.FindIndex(x => x.PrimaryAccount == true);
                    if (foundIndex > -1)
                    {
                        dgvAccountDTOList.Rows[foundIndex].DefaultCellStyle.BackColor = Color.Aquamarine;
                    }
                }
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
            else if (e.ColumnIndex == tagNumberDataGridViewTextBoxColumn.Index)
            {
                cardNumber = dgvAccountDTOList.Rows[e.RowIndex].Cells["tagNumberDataGridViewTextBoxColumn"].Value.ToString();
                //this.DialogResult = System.Windows.Forms.DialogResult.OK;
                //Close();
            }
            else if (e.ColumnIndex == creditPlusDataGridViewButtonColumn.Index)
            {
                try
                {
                    if (accountDTOListBS.Current != null &&
                    accountDTOListBS.Current is AccountDTO)
                    {
                        AccountDTO accountDTO = accountDTOListBS.Current as AccountDTO;
                        if (accountDTO.AccountId != -1)
                        {
                            Type creditPlusListUI = Type.GetType("Semnox.Parafait.Customer.Accounts.AccountCreditPlusListUI, CustomerUI");
                            object accountCreditPlusListUI = null;
                            if (creditPlusListUI != null)
                            {
                                ConstructorInfo constructorN = creditPlusListUI.GetConstructor(new Type[] { typeof(Utilities), typeof(int), typeof(bool) });
                                accountCreditPlusListUI = constructorN.Invoke(new object[] { utilities, accountDTO.AccountId, enableEditing });
                                Form frmAccountCreditPlusList = (Form)(accountCreditPlusListUI);
                                frmAccountCreditPlusList.ShowDialog();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while showing account creditPlus view", ex);
                }

            }
            log.LogMethodExit();
        }
    }
}
