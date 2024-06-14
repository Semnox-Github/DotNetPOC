/********************************************************************************************
 * Project Name - Account Credit Plus List UI
 * Description  - Account Credit Plus List UI
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 *********************************************************************************************
 *2.80.0      17-Feb-2019       Deeksha             Modified to Make DigitalSignage module as
 *                                                  read only in Windows Management Studio.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Game;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Account Credit Plus UI class
    /// </summary>
    public partial class AccountCreditPlusListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int accountId;
        AccountDTO accountDTO;
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="accountId"></param>
        /// <param name="enableEditing"></param>
        public AccountCreditPlusListUI(Utilities utilities, int accountId, bool enableEditing = true)
        {
            log.LogMethodEntry(utilities, accountId);
            InitializeComponent();
            this.utilities = utilities;
            this.accountId = accountId;
            utilities.setupDataGridProperties(ref dgvAccountCreditPlusDTOList);
            utilities.setupDataGridProperties(ref dgvAccountCreditPlusConsumptionDTOList);
            utilities.setLanguage(this);

            if ((utilities.ParafaitEnv.LoginID != "semnox" && 
                ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ALLOW_MANUAL_CARD_UPDATE") == false) ||
                enableEditing == false)
            {
                dgvAccountCreditPlusDTOList.ReadOnly = dgvAccountCreditPlusConsumptionDTOList.ReadOnly = true;
                btnSave.Visible = false;
                btnDelete.Visible = false;
            }

            discountAmountDataGridViewTextBoxColumn.DefaultCellStyle =
            discountedPriceDataGridViewTextBoxColumn.DefaultCellStyle =
            discountPercentageDataGridViewTextBoxColumn.DefaultCellStyle =
            quantityLimitDataGridViewTextBoxColumn.DefaultCellStyle =
            consumptionBalanceDataGridViewTextBoxColumn.DefaultCellStyle =
            creditPlusBalanceDataGridViewTextBoxColumn.DefaultCellStyle =
            creditPlusDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();

            periodFromDataGridViewTextBoxColumn.DefaultCellStyle = 
            periodToDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            ThemeUtils.SetupVisuals(this);
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            log.LogMethodExit();
        }

        private async void CardCreditPlus_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAccountCreditPlusConsumptionDTOList.Enabled = false;
            dgvAccountCreditPlusDTOList.Enabled = false;
            lblStatus.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);
            OnDataLoadStart();
            await RefreshData();
            OnDataLoadComplete();
            if (managementStudioSwitch.EnableCardsModule)
            {
                dgvAccountCreditPlusConsumptionDTOList.Enabled = true;
                dgvAccountCreditPlusDTOList.Enabled = true;
            }
            log.LogMethodExit();
        }

        private async Task RefreshData()
        {
            try
            {
                creditPlusTypeDataGridViewComboBoxColumn.ValueMember = "Key";
                creditPlusTypeDataGridViewComboBoxColumn.DisplayMember = "Value";
                creditPlusTypeDataGridViewComboBoxColumn.DataSource = GetCreditPlusTypeList();

                orderTypeIdDataGridViewComboBoxColumn.ValueMember = "Id";
                orderTypeIdDataGridViewComboBoxColumn.DisplayMember = "Name";
                orderTypeIdDataGridViewComboBoxColumn.DataSource = await Task<List<OrderTypeDTO>>.Factory.StartNew(() => { return GetOrderTypeDTOList(); });

                gameIdDataGridViewComboBoxColumn.ValueMember = "GameId";
                gameIdDataGridViewComboBoxColumn.DisplayMember = "GameName";
                gameIdDataGridViewComboBoxColumn.DataSource = await Task<List<GameDTO>>.Factory.StartNew(() => { return GetGameDTOList(); });

                gameProfileIdDataGridViewComboBoxColumn.ValueMember = "GameProfileId";
                gameProfileIdDataGridViewComboBoxColumn.DisplayMember = "ProfileName";
                gameProfileIdDataGridViewComboBoxColumn.DataSource = await Task<List<GameProfileDTO>>.Factory.StartNew(() => { return GetGameProfileDTOList(); });

                productIdDataGridViewComboBoxColumn.ValueMember = "ProductId";
                productIdDataGridViewComboBoxColumn.DisplayMember = "ProductName";
                productIdDataGridViewComboBoxColumn.DataSource = await Task<List<ProductsDTO>>.Factory.StartNew(() => { return GetProductsDTOList(); });

                categoryIdDataGridViewComboBoxColumn.ValueMember = "CategoryId";
                categoryIdDataGridViewComboBoxColumn.DisplayMember = "Name";
                categoryIdDataGridViewComboBoxColumn.DataSource = await Task<List<CategoryDTO>>.Factory.StartNew(() => { return GetCategoryDTOList(); });

                pOSTypeIdDataGridViewComboBoxColumn.ValueMember = "POSTypeId";
                pOSTypeIdDataGridViewComboBoxColumn.DisplayMember = "POSTypeName";
                pOSTypeIdDataGridViewComboBoxColumn.DataSource = await Task<List<POSTypeDTO>>.Factory.StartNew(() => { return GetPOSTypeDTOList(); });

                await LoadAccountCreditPlusDTOList();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while setting order type data source", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private async Task LoadAccountCreditPlusDTOList()
        {
            log.LogMethodEntry();
            List<AccountCreditPlusDTO> accountCreditPlusDTOList = null;
            accountDTO = await Task<AccountDTO>.Factory.StartNew(()=> { return GetAccountDTO(accountId); });
            if (accountDTO != null)
            {
                if (accountDTO.AccountCreditPlusDTOList == null)
                {
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                }
                accountCreditPlusDTOList = accountDTO.AccountCreditPlusDTOList;
                foreach (var accountCreditPlusDTO in accountCreditPlusDTOList)
                {
                    if (accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList == null)
                    {
                        accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList = new List<AccountCreditPlusConsumptionDTO>();
                    }
                }
            }
            else
            {
                throw new Exception("Invalid account id: " + accountId.ToString());
            }
            accountCreditPlusDTOListBS.DataSource = accountCreditPlusDTOList;
            txtCardBalance.Text = String.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.AccountSummaryDTO.CreditPlusCardBalance);
            txtCredits.Text = String.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits);
            txtBonus.Text = String.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.AccountSummaryDTO.CreditPlusBonus);
            txtItemPurchase.Text = String.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.AccountSummaryDTO.CreditPlusItemPurchase);
            txtLoyalty.Text = String.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.AccountSummaryDTO.CreditPlusLoyaltyPoints);
            txtTickets.Text = String.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.AccountSummaryDTO.CreditPlusTickets);
            txtRefundable.Text = String.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.AccountSummaryDTO.CreditPlusRefundableBalance);
            log.LogMethodExit();
        }

        private AccountDTO GetAccountDTO(int accountId)
        {
            log.LogMethodEntry(accountId);
            AccountDTO accountDTO = null;
            AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountId);
            accountDTO = accountBL.AccountDTO;
            log.LogMethodExit(accountDTO);
            return accountDTO;
        }

        private void OnDataLoadStart()
        {
            log.LogMethodEntry();
            try
            {
                DisableControls();
                lblStatus.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while disabling controls", ex);
            }
            log.LogMethodExit();
        }

        private void OnDataLoadComplete()
        {
            log.LogMethodEntry();
            try
            {
                lblStatus.Text = "";
                EnableControls();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while enabling controls", ex);
            }
            log.LogMethodExit();
        }

        private void DisableControls()
        {
            log.LogMethodEntry();
            btnSave.Enabled = false;
            btnRefresh.Enabled = false;
            btnDelete.Enabled = false;
            btnClose.Enabled = false;
            RefreshControls();
            log.LogMethodExit();
        }

        private void EnableControls()
        {
            log.LogMethodEntry();
            if (utilities.ParafaitEnv.LoginID == "semnox" || ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ALLOW_MANUAL_CARD_UPDATE"))
            {
                if (managementStudioSwitch.EnableCardsModule)
                {
                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
            btnRefresh.Enabled = true;
            btnClose.Enabled = true;
            RefreshControls();
            log.LogMethodExit();
        }

        private void RefreshControls()
        {
            log.LogMethodEntry();
            btnSave.Refresh();
            btnRefresh.Refresh();
            btnDelete.Refresh();
            btnClose.Refresh();
            lblStatus.Refresh();
            //dgvAccountCreditPlusDTOList.Refresh();
            //dgvAccountCreditPlusConsumptionDTOList.Refresh();
            log.LogMethodExit();
        }

        #region DataSources

        private List<KeyValuePair<CreditPlusType, string>> GetCreditPlusTypeList()
        {
            log.LogMethodEntry();
            List<KeyValuePair<CreditPlusType, string>> creditPlusTypeList = new List<KeyValuePair<CreditPlusType, string>>();
            creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.CARD_BALANCE, MessageContainerList.GetMessage(utilities.ExecutionContext, "Card Balance")));
            creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.COUNTER_ITEM, MessageContainerList.GetMessage(utilities.ExecutionContext, "Counter Items Only")));
            creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.GAME_PLAY_BONUS, MessageContainerList.GetMessage(utilities.ExecutionContext, "Game Play Bonus")));
            creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.GAME_PLAY_CREDIT, MessageContainerList.GetMessage(utilities.ExecutionContext, "Game Play Credits")));
            creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.LOYALTY_POINT, MessageContainerList.GetMessage(utilities.ExecutionContext, "Loyalty Points")));
            creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.TICKET, MessageContainerList.GetMessage(utilities.ExecutionContext, "Tickets")));
            creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.TIME, MessageContainerList.GetMessage(utilities.ExecutionContext, "Time")));
            log.LogMethodExit(creditPlusTypeList);
            return creditPlusTypeList;
        }
        private List<OrderTypeDTO> GetOrderTypeDTOList()
        {
            log.LogMethodEntry();
            List<OrderTypeDTO> orderTypeDTOList = null;
            OrderTypeListBL orderTypeListBL = new OrderTypeListBL(utilities.ExecutionContext);
            List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<OrderTypeDTO.SearchByParameters, string>(OrderTypeDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            orderTypeDTOList = orderTypeListBL.GetOrderTypeDTOList(searchParameters);
            if (orderTypeDTOList == null)
            {
                orderTypeDTOList = new List<OrderTypeDTO>();
            }
            orderTypeDTOList.Insert(0, new OrderTypeDTO());
            orderTypeDTOList[0].Name = "<SELECT>";
            orderTypeDTOList[0].Id = -1;
            log.LogMethodExit(orderTypeDTOList);
            return orderTypeDTOList;
        }

        private List<GameDTO> GetGameDTOList()
        {
            log.LogMethodEntry();
            GameList gameList = new GameList();
            List<GameDTO> gameDTOList;
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            gameDTOList = gameList.GetGameList(searchParameters);
            if (gameDTOList == null)
            {
                gameDTOList = new List<GameDTO>();
            }

            gameDTOList.Insert(0, new GameDTO());
            gameDTOList[0].GameId = -1;
            gameDTOList[0].GameName = "-All-";
            log.LogMethodExit(gameDTOList);
            return gameDTOList;
        }

        private List<GameProfileDTO> GetGameProfileDTOList()
        {
            log.LogMethodEntry();
            List<GameProfileDTO> gameProfileDTOList = null;
            try
            {
                List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchGameProfileParameters = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
                searchGameProfileParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                GameProfileList gameProfileList = new GameProfileList(utilities.ExecutionContext, searchGameProfileParameters);
                gameProfileDTOList = gameProfileList.GetGameProfileList;
                if (gameProfileDTOList == null)
                {
                    gameProfileDTOList = new List<GameProfileDTO>();
                }

                gameProfileDTOList.Insert(0, new GameProfileDTO());
                gameProfileDTOList[0].GameProfileId = -1;
                gameProfileDTOList[0].ProfileName = "-All-";
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading games", ex);
            }
            log.LogMethodExit(gameProfileDTOList);
            return gameProfileDTOList;
        }

        private List<ProductsDTO> GetProductsDTOList()
        {
            log.LogMethodEntry();
            Products products = new Products();
            List<ProductsDTO> productsDTOList;
            ProductsFilterParams productsFilterParams = new ProductsFilterParams();
            //productsFilterParams.IsActive = "Y";
            productsFilterParams.SiteId = utilities.ExecutionContext.GetSiteId();
            productsDTOList = products.GetProductDTOList(productsFilterParams);
            if (productsDTOList == null)
            {
                productsDTOList = new List<ProductsDTO>();
            }
            productsDTOList.Insert(0, new ProductsDTO());
            productsDTOList[0].ProductId = -1;
            productsDTOList[0].ProductName = "-All-";
            log.LogMethodExit(productsDTOList);
            return productsDTOList;
        }

        private List<CategoryDTO> GetCategoryDTOList()
        {
            log.LogMethodEntry();
            CategoryList categoryList = new CategoryList(utilities.ExecutionContext);
            List<CategoryDTO> categoryDTOList;
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            categoryDTOList = categoryList.GetAllCategory(searchParameters);
            if (categoryDTOList == null)
            {
                categoryDTOList = new List<CategoryDTO>();
            }
            categoryDTOList.Insert(0, new CategoryDTO());
            categoryDTOList[0].CategoryId = -1;
            categoryDTOList[0].Name = "-All-";

            log.LogMethodExit(categoryDTOList);
            return categoryDTOList;
        }

        private List<POSTypeDTO> GetPOSTypeDTOList()
        {
            log.LogMethodEntry();
            List<POSTypeDTO> pOSTypeDTOList = null;
            POSTypeListBL pOSTypeListBL = new POSTypeListBL(utilities.ExecutionContext);
            List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            pOSTypeDTOList = pOSTypeListBL.GetPOSTypeDTOList(searchParameters);
            if (pOSTypeDTOList == null)
            {
                pOSTypeDTOList = new List<POSTypeDTO>();
            }
            pOSTypeDTOList.Insert(0, new POSTypeDTO());
            pOSTypeDTOList[0].POSTypeName = "<SELECT>";
            pOSTypeDTOList[0].POSTypeId = -1;
            log.LogMethodExit(pOSTypeDTOList);
            return pOSTypeDTOList;
        }
        #endregion

        private async void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (ValidateAccountCreditPlusDTOList())
            {
                OnDataLoadStart();
                if (Save())
                {
                    await LoadAccountCreditPlusDTOList();
                }
                OnDataLoadComplete();
            }
            log.LogMethodExit();
        }

        private bool Save()
        {
            log.LogMethodEntry();
            bool result = true;
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
                result = false;
                sqlTransaction.Rollback();
                log.LogVariableState("accountDTO", accountDTO);
                log.Error("Validation failed", ex);
                log.Error(ex.GetAllValidationErrorMessages());
                MessageBox.Show(ex.GetAllValidationErrorMessages());
            }
            catch (Exception ex)
            {
                result = false;
                sqlTransaction.Rollback();
                log.Error("Error while saving account credit plus", ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(718));
            }

            log.LogMethodExit(result);
            return result;
        }

        private bool ValidateAccountCreditPlusDTOList()
        {
            log.LogMethodEntry();
            bool valid = true;
            if (accountCreditPlusDTOListBS.DataSource != null && accountCreditPlusDTOListBS.DataSource is List<AccountCreditPlusDTO>)
            {
                List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountCreditPlusDTOListBS.DataSource as List<AccountCreditPlusDTO>;
                for (int i = 0; i < accountCreditPlusDTOList.Count; i++)
                {
                    if (accountCreditPlusDTOList[i].AccountCreditPlusConsumptionDTOList != null)
                    {
                        for (int j = 0; j < accountCreditPlusDTOList[i].AccountCreditPlusConsumptionDTOList.Count; j++)
                        {
                            if (accountCreditPlusDTOList[i].AccountCreditPlusConsumptionDTOList[j].IsChanged)
                            {
                                AccountCreditPlusConsumptionBL accountCreditPlusConsumptionBL = new AccountCreditPlusConsumptionBL(utilities.ExecutionContext, accountCreditPlusDTOList[i].AccountCreditPlusConsumptionDTOList[j]);
                                List<ValidationError> validationErrorList = accountCreditPlusConsumptionBL.Validate();
                                if (validationErrorList != null && validationErrorList.Count > 0)
                                {
                                    valid = false;
                                    log.LogVariableState("AccountCreditPlusConsumptionDTO", accountCreditPlusDTOList[i].AccountCreditPlusConsumptionDTOList[j]);
                                    log.LogMethodExit(valid, "AccountCreditPlusConsumptionDTO not valid");
                                    try
                                    {
                                        dgvAccountCreditPlusDTOList.Rows[i].Selected = true;
                                        dgvAccountCreditPlusConsumptionDTOList.Rows[j].Selected = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("Error occured while selecting the row with error", ex);
                                    }
                                    MessageBox.Show(GetValidationErrorMessage(validationErrorList));
                                    return valid;
                                }
                            }
                        }
                    }
                    if (accountCreditPlusDTOList[i].IsChanged)
                    {
                        AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(utilities.ExecutionContext, accountCreditPlusDTOList[i]);
                        List<ValidationError> validationErrorList = accountCreditPlusBL.Validate();
                        if (validationErrorList != null && validationErrorList.Count > 0)
                        {
                            valid = false;
                            dgvAccountCreditPlusDTOList.Rows[i].Selected = true;
                            MessageBox.Show(GetValidationErrorMessage(validationErrorList));
                            log.LogVariableState("AccountCreditPlusDTO", accountCreditPlusDTOList[i]);
                            log.LogMethodExit(valid, "AccountCreditPlusDTO not valid");
                            return valid;
                        }
                    }
                }
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private string GetValidationErrorMessage(List<ValidationError> validationErrorList)
        {
            log.LogMethodEntry(validationErrorList);
            string errorMessage;
            StringBuilder sb = new StringBuilder();
            foreach (var validationError in validationErrorList)
            {
                sb.Append(validationError.Message);
                sb.Append(Environment.NewLine);
            }
            errorMessage = sb.ToString();
            log.LogMethodExit(errorMessage);
            return errorMessage;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                OnDataLoadStart();
                await RefreshData();
                OnDataLoadComplete();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading account creditplus list.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                OnDataLoadStart();
                bool rowsDeleted = false;
                bool confirmDelete = true;
                bool refreshFromDB = false;
                dgvAccountCreditPlusConsumptionDTOList.EndEdit();
                dgvAccountCreditPlusDTOList.EndEdit();
                List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountCreditPlusDTOListBS.DataSource as List<AccountCreditPlusDTO>;
                if (dgvAccountCreditPlusDTOList.SelectedRows.Count <= 0 && dgvAccountCreditPlusDTOList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                List<AccountCreditPlusConsumptionDTO> accountCreditPlusConsumptionDTOList = accountCreditPlusConsumptionDTOListBS.DataSource as List<AccountCreditPlusConsumptionDTO>;
                if (accountCreditPlusConsumptionDTOList != null &&
                    accountCreditPlusConsumptionDTOList.Count > 0 &&
                    dgvAccountCreditPlusConsumptionDTOList.RowCount > 0)
                {

                    if (this.dgvAccountCreditPlusConsumptionDTOList.SelectedCells.Count > 0)
                    {
                        foreach (DataGridViewCell cell in dgvAccountCreditPlusConsumptionDTOList.SelectedCells)
                        {
                            if (dgvAccountCreditPlusConsumptionDTOList.Rows[cell.RowIndex].IsNewRow == false)
                            {
                                dgvAccountCreditPlusConsumptionDTOList.Rows[cell.RowIndex].Selected = true;
                            }
                        }
                    }
                    else
                    {
                        dgvAccountCreditPlusConsumptionDTOList.Rows[dgvAccountCreditPlusConsumptionDTOList.RowCount - 1].Selected = true;
                    }
                    foreach (DataGridViewRow row in dgvAccountCreditPlusConsumptionDTOList.SelectedRows)
                    {
                        if (row.IsNewRow == false)
                        {
                            if (accountCreditPlusConsumptionDTOList[row.Index].AccountCreditPlusConsumptionId == -1)
                            {
                                dgvAccountCreditPlusConsumptionDTOList.Rows.RemoveAt(row.Index);
                                rowsDeleted = true;
                            }
                            else
                            {
                                if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    refreshFromDB = true;
                                    AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO = accountCreditPlusConsumptionDTOList[row.Index];
                                    accountCreditPlusConsumptionDTO.IsActive = false;
                                }
                            }
                        }
                    }
                }
                if (rowsDeleted == false && refreshFromDB == false && accountCreditPlusDTOListBS.Current != null && accountCreditPlusDTOListBS.Current is AccountCreditPlusDTO)
                {
                    AccountCreditPlusDTO accountCreditPlusDTO = accountCreditPlusDTOListBS.Current as AccountCreditPlusDTO;
                    if (accountCreditPlusDTO.AccountCreditPlusId > 1)
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            refreshFromDB = true;
                            accountCreditPlusDTO.IsActive = false;
                        }
                    }
                    else
                    {
                        if (dgvAccountCreditPlusDTOList.CurrentRow.IsNewRow == false)
                        {
                            dgvAccountCreditPlusDTOList.Rows.RemoveAt(dgvAccountCreditPlusDTOList.CurrentRow.Index);
                            rowsDeleted = true;
                        }
                    }
                }

                if (confirmDelete && refreshFromDB)
                {
                    if (Save())
                    {
                        await LoadAccountCreditPlusDTOList();
                    }
                }
                if (rowsDeleted == true)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                }
                if (refreshFromDB == false && rowsDeleted == true)
                {
                    dgvAccountCreditPlusDTOList.Refresh();
                    dgvAccountCreditPlusConsumptionDTOList.Refresh();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while deleting the record", ex);
            }
            finally
            {
                OnDataLoadComplete();
            }

            log.LogMethodExit();
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void dgvAccountCreditPlusDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvAccountCreditPlusDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvAccountCreditPlusConsumptionDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvAccountCreditPlusConsumptionDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvAccountCreditPlusDTOList_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountCreditPlusDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            try
            {
                if (e.ColumnIndex == creditPlusBalanceDataGridViewTextBoxColumn.Index)
                {
                    if (accountCreditPlusDTOListBS.DataSource != null &&
                        accountCreditPlusDTOListBS.DataSource is List<AccountCreditPlusDTO>)
                    {
                        List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountCreditPlusDTOListBS.DataSource as List<AccountCreditPlusDTO>;
                        if (accountCreditPlusDTOList != null &&
                            accountCreditPlusDTOList.Count > 0 &&
                            accountCreditPlusDTOList.Count > e.RowIndex)
                        {
                            AccountCreditPlusDTO accountCreditPlusDTO = accountCreditPlusDTOList[e.RowIndex];
                            if (accountCreditPlusDTO.AccountCreditPlusId == -1)
                            {
                                if (dgvAccountCreditPlusDTOList.Rows[e.RowIndex].Cells[creditPlusBalanceDataGridViewTextBoxColumn.Index].Value != null)
                                {
                                    dgvAccountCreditPlusDTOList.Rows[e.RowIndex].Cells[creditPlusDataGridViewTextBoxColumn.Index].Value = dgvAccountCreditPlusDTOList.Rows[e.RowIndex].Cells[creditPlusBalanceDataGridViewTextBoxColumn.Index].Value;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while assigning credit plus loaded value", ex);
            }
            log.LogMethodExit();
        }

        private void accountCreditPlusDTOListBS_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetAccountCreditPlusConsumptionDataSource();
            log.LogMethodExit();
        }

        private void SetAccountCreditPlusConsumptionDataSource()
        {
            log.LogMethodEntry();
            if (accountCreditPlusDTOListBS.Current != null &&
                            accountCreditPlusDTOListBS.Current is AccountCreditPlusDTO)
            {
                AccountCreditPlusDTO accountCreditPlusDTO = accountCreditPlusDTOListBS.Current as AccountCreditPlusDTO;
                accountCreditPlusConsumptionDTOListBS.DataSource = accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList;
                if (accountCreditPlusDTO.AccountCreditPlusId > -1)
                {
                    dgvAccountCreditPlusConsumptionDTOList.AllowUserToAddRows = true;
                    dgvAccountCreditPlusConsumptionDTOList.Enabled = true;
                }
                else
                {
                    dgvAccountCreditPlusConsumptionDTOList.AllowUserToAddRows = false;
                    dgvAccountCreditPlusConsumptionDTOList.Enabled = false;
                }
            }
            else
            {
                dgvAccountCreditPlusConsumptionDTOList.AllowUserToAddRows = false;
                dgvAccountCreditPlusConsumptionDTOList.Enabled = false;
            }
            log.LogMethodExit();
        }

        private void accountCreditPlusDTOListBS_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            AccountCreditPlusDTO accountCreditPlusDTO = new AccountCreditPlusDTO();
            accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList = new List<AccountCreditPlusConsumptionDTO>();
            e.NewObject = accountCreditPlusDTO;
            log.LogMethodExit();
        }

        private void accountCreditPlusDTOListBS_DataSourceChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetAccountCreditPlusConsumptionDataSource();
            log.LogMethodExit();
        }

        private void dgvAccountCreditPlusConsumptionDTOList_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountCreditPlusConsumptionDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            //UpdateCreditPlusConsumptionDataGridView(e);
            log.LogMethodExit();
        }

        private bool IsIntCellEmpty(DataGridViewCell dataGridViewCell)
        {
            log.LogMethodEntry(dataGridViewCell);
            bool result = false;
            if(dataGridViewCell.Value == null || Convert.ToInt32(dataGridViewCell.Value) < 0)
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void dgvAccountCreditPlusConsumptionDTOList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountCreditPlusConsumptionDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            UpdateCreditPlusConsumptionDataGridView(e);
            log.LogMethodExit();
        }

        private void UpdateCreditPlusConsumptionDataGridView(DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(e);
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[gameIdDataGridViewComboBoxColumn.Index].ReadOnly = true;
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[gameProfileIdDataGridViewComboBoxColumn.Index].ReadOnly = true;
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[pOSTypeIdDataGridViewComboBoxColumn.Index].ReadOnly = true;
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[productIdDataGridViewComboBoxColumn.Index].ReadOnly = true;
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[quantityLimitDataGridViewTextBoxColumn.Index].ReadOnly = true;
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[categoryIdDataGridViewComboBoxColumn.Index].ReadOnly = true;
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountAmountDataGridViewTextBoxColumn.Index].ReadOnly = true;
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountedPriceDataGridViewTextBoxColumn.Index].ReadOnly = true;
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountPercentageDataGridViewTextBoxColumn.Index].ReadOnly = true;
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index].ReadOnly = true;
            dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[orderTypeIdDataGridViewComboBoxColumn.Index].ReadOnly = true;

            if (IsIntCellEmpty(dgvAccountCreditPlusConsumptionDTOList[pOSTypeIdDataGridViewComboBoxColumn.Index, e.RowIndex]) == false)
            {
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[pOSTypeIdDataGridViewComboBoxColumn.Index].ReadOnly = false;
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[quantityLimitDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountAmountDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountedPriceDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountPercentageDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index], null);
            }
            else if (IsIntCellEmpty(dgvAccountCreditPlusConsumptionDTOList[gameProfileIdDataGridViewComboBoxColumn.Index, e.RowIndex]) == false)
            {
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[gameProfileIdDataGridViewComboBoxColumn.Index].ReadOnly = false;
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[quantityLimitDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountAmountDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountedPriceDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountPercentageDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index], null);
            }
            else if (IsIntCellEmpty(dgvAccountCreditPlusConsumptionDTOList[gameIdDataGridViewComboBoxColumn.Index, e.RowIndex]) == false)
            {
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[gameIdDataGridViewComboBoxColumn.Index].ReadOnly = false;
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[quantityLimitDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountAmountDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountedPriceDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountPercentageDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index], null);
            }
            else if (IsIntCellEmpty(dgvAccountCreditPlusConsumptionDTOList[categoryIdDataGridViewComboBoxColumn.Index, e.RowIndex]) == false)
            {
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[categoryIdDataGridViewComboBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[quantityLimitDataGridViewTextBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountAmountDataGridViewTextBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountedPriceDataGridViewTextBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountPercentageDataGridViewTextBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[orderTypeIdDataGridViewComboBoxColumn.Index].ReadOnly = false;

                if (IsIntCellEmpty(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index]))
                    SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index], 1);
            }
            else if (IsIntCellEmpty(dgvAccountCreditPlusConsumptionDTOList[productIdDataGridViewComboBoxColumn.Index, e.RowIndex]) == false)
            {
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[productIdDataGridViewComboBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[quantityLimitDataGridViewTextBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountAmountDataGridViewTextBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountedPriceDataGridViewTextBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountPercentageDataGridViewTextBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index].ReadOnly = false;
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[orderTypeIdDataGridViewComboBoxColumn.Index].ReadOnly = false;

                if (IsIntCellEmpty(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index]))
                    SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index], 1);
            }
            else if (e.ColumnIndex != quantityLimitDataGridViewTextBoxColumn.Index
                && e.ColumnIndex != discountAmountDataGridViewTextBoxColumn.Index
                && e.ColumnIndex != discountedPriceDataGridViewTextBoxColumn.Index
                && e.ColumnIndex != consumptionBalanceDataGridViewTextBoxColumn.Index
                && e.ColumnIndex != discountPercentageDataGridViewTextBoxColumn.Index 
                && e.ColumnIndex != orderTypeIdDataGridViewComboBoxColumn.Index)
            {
                dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountAmountDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountedPriceDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[discountPercentageDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[quantityLimitDataGridViewTextBoxColumn.Index], null);
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList.Rows[e.RowIndex].Cells[consumptionBalanceDataGridViewTextBoxColumn.Index], null);
            }

            if (IsIntCellEmpty(dgvAccountCreditPlusConsumptionDTOList[productIdDataGridViewComboBoxColumn.Index, e.RowIndex]) &&
                IsIntCellEmpty(dgvAccountCreditPlusConsumptionDTOList[categoryIdDataGridViewComboBoxColumn.Index, e.RowIndex]) &&
                IsIntCellEmpty(dgvAccountCreditPlusConsumptionDTOList[orderTypeIdDataGridViewComboBoxColumn.Index, e.RowIndex]) == false)
            {
                SetDataGridViewCellValue(dgvAccountCreditPlusConsumptionDTOList[orderTypeIdDataGridViewComboBoxColumn.Index, e.RowIndex], -1);
            }
            log.LogMethodExit();
        }

        private void SetDataGridViewCellValue(DataGridViewCell cell, object value)
        {
            log.LogMethodEntry(cell, value);
            if (cell.ReadOnly == false)
            {
                cell.Value = value;
            }
            else
            {
                cell.ReadOnly = false;
                cell.Value = value;
                cell.ReadOnly = true;
            }
            log.LogMethodExit();
        }
    }
}
