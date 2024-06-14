/********************************************************************************************
 * Project Name - AccountGameListUI
 * Description  - AccountGameList UI
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 *********************************************************************************************
 *2.80.0      17-Feb-2019       Deeksha             Modified to Make DigitalSignage module as
 *                                                  read only in Windows Management Studio.
 *2.90         03-July-2020     Girish Kundar   Modified : Change as part of CardCodeDTOList replaced with AccountDTOList in CustomerDTO                                                  
 ********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Game;
using Semnox.Parafait.GenericUtilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Account Game List UI
    /// </summary>
    public partial class AccountGameListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int accountId;
        AccountDTO accountDTO;
        private List<CustomAttributesDTO> customAttributesDTOList;
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="accountId"></param>
        public AccountGameListUI(Utilities utilities, int accountId)
        {
            log.LogMethodEntry(utilities, accountId);
            InitializeComponent();
            this.utilities = utilities;
            this.accountId = accountId;

            if (utilities.ParafaitEnv.LoginID != "semnox" && ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext,"ALLOW_MANUAL_CARD_UPDATE") == false)
            {
                dgvAccountGameExtendedDTOList.ReadOnly = dgvAccountGameDTOList.ReadOnly = true;
                btnDelete.Visible = false;
                btnSave.Visible = false;
            }

            utilities.setupDataGridProperties(ref dgvAccountGameDTOList);
            utilities.setupDataGridProperties(ref dgvAccountGameExtendedDTOList);
            balanceGamesDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            quantityDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            fromDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            lastPlayedTimeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            expiryDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            PlayLimitPerGame.DefaultCellStyle = utilities.gridViewNumericCellStyle();

            customDataDataGridViewButtonColumn.Text = "...";
            customDataDataGridViewButtonColumn.UseColumnTextForButtonValue = true;
            customDataDataGridViewButtonColumn.Width = 30;

            utilities.setLanguage(this);
            ThemeUtils.SetupVisuals(this);
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            log.LogMethodExit();
        }

        private async void AccountGameListUI_Load(object sender, EventArgs e)
        {

            log.LogMethodEntry(sender, e);
            try
            {
                lblStatus.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);
                dgvAccountGameDTOList.Enabled = false;
                dgvAccountGameExtendedDTOList.Enabled = false;
                OnDataLoadStart();
                await RefreshData();
                OnDataLoadComplete();
                if (managementStudioSwitch.EnableCardsModule)
                {
                    dgvAccountGameDTOList.Enabled = true;
                    dgvAccountGameExtendedDTOList.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading account list.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private async Task RefreshData()
        {
            log.LogMethodEntry();
            accountGameDTOListBS.DataSource = new List<AccountGameDTO>();
            accountGameExtendedDTOListBS.DataSource = new List<AccountGameExtendedDTO>();
            frequencyDataGridViewComboBoxColumn.DataSource = GetAccountGameFrequencyDataSource();
            frequencyDataGridViewComboBoxColumn.ValueMember = "Key";
            frequencyDataGridViewComboBoxColumn.DisplayMember = "Value";

            List<LookupValuesDTO> accountGameEntitlementTypeList = await Task<List<LookupValuesDTO>>.Factory.StartNew(() => { return GetGameEntitlementTypeList(); });
            BindingSource bs = new BindingSource();
            bs.DataSource = accountGameEntitlementTypeList;
            entitlementTypeDataGridViewComboBoxColumn.ValueMember = "LookupValue";
            entitlementTypeDataGridViewComboBoxColumn.DisplayMember = "Description";
            entitlementTypeDataGridViewComboBoxColumn.DataSource = bs;

            List<GameDTO> gameDTOList = await Task<List<GameDTO>>.Factory.StartNew(() => { return GetGameDTOList(); });
            bs = new BindingSource();
            bs.DataSource = gameDTOList;
            gameIdAccountGameDataGridComboBoxColumn.DisplayMember = "GameName";
            gameIdAccountGameDataGridComboBoxColumn.ValueMember = "GameId";
            gameIdAccountGameDataGridComboBoxColumn.DataSource = bs;

            bs = new BindingSource();
            bs.DataSource = gameDTOList;
            gameIdAccountGameExtendedDataGridViewComboBoxColumn.DisplayMember = "GameName";
            gameIdAccountGameExtendedDataGridViewComboBoxColumn.ValueMember = "GameId";
            gameIdAccountGameExtendedDataGridViewComboBoxColumn.DataSource = bs;

            List<GameProfileDTO> gameProfileDTOList = await Task<List<GameProfileDTO>>.Factory.StartNew(() => { return GetGameProfileDTOList(); });
            bs = new BindingSource();
            bs.DataSource = gameProfileDTOList;

            gameProfileIdAccountGameDataGridViewComboBoxColumn.DisplayMember = "ProfileName";
            gameProfileIdAccountGameDataGridViewComboBoxColumn.ValueMember = "GameProfileId";
            gameProfileIdAccountGameDataGridViewComboBoxColumn.DataSource = bs;

            bs = new BindingSource();
            bs.DataSource = gameProfileDTOList;

            gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.DisplayMember = "ProfileName";
            gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.ValueMember = "GameProfileId";
            gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.DataSource = bs;

            await LoadAccountGameDTOList();
            log.LogMethodExit();
        }

        private List<LookupValuesDTO> GetGameEntitlementTypeList()
        {
            log.LogMethodEntry();
            List<LookupValuesDTO> accountGameEntitlementTypeList = null;
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CARD_GAMES_ENTITLEMENT_TYPES"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                accountGameEntitlementTypeList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (accountGameEntitlementTypeList == null)
                {
                    accountGameEntitlementTypeList = new List<LookupValuesDTO>();
                }
                accountGameEntitlementTypeList.Insert(0, new LookupValuesDTO());
                accountGameEntitlementTypeList[0].LookupValue = "";
                accountGameEntitlementTypeList[0].Description = "Default";
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading games", ex);
            }
            log.LogMethodExit(accountGameEntitlementTypeList);
            return accountGameEntitlementTypeList;
        }

        private List<GameDTO> GetGameDTOList()
        {
            log.LogMethodEntry();
            List<GameDTO> gameDTOList = null;
            try
            {
                GameList gameList = new GameList();
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
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading games", ex);
            }
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
                GameProfileList gameProfileList = new GameProfileList(utilities.ExecutionContext);
                gameProfileDTOList = gameProfileList.GetGameProfileDTOList(searchGameProfileParameters, false);
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

        private async Task LoadAccountGameDTOList()
        {
            log.LogMethodEntry();
            List<AccountGameDTO> accountGameDTOList = null;
            accountDTO = await Task<AccountDTO>.Factory.StartNew(() => { return GetAccountDTO(accountId); });
            if (accountDTO != null)
            {
                if(accountDTO.AccountGameDTOList == null)
                {
                    accountDTO.AccountGameDTOList = new List<AccountGameDTO>();
                }
                accountGameDTOList = accountDTO.AccountGameDTOList;
                foreach (var accountGameDTO in accountGameDTOList)
                {
                    if (accountGameDTO.AccountGameExtendedDTOList == null)
                    {
                        accountGameDTO.AccountGameExtendedDTOList = new List<AccountGameExtendedDTO>();
                    }
                }
            }
            else
            {
                throw new Exception("Invalid account id: " + accountId.ToString());
            }
            accountGameDTOListBS.DataSource = accountGameDTOList;
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
            log.LogMethodExit();
        }

        private List<KeyValuePair<string, string>> GetAccountGameFrequencyDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> cardGameFrequencyDataSource = new List<KeyValuePair<string, string>>();
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("N", MessageContainerList.GetMessage(utilities.ExecutionContext, "None")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("D", MessageContainerList.GetMessage(utilities.ExecutionContext, "Daily")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("W", MessageContainerList.GetMessage(utilities.ExecutionContext, "Weekly")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("M", MessageContainerList.GetMessage(utilities.ExecutionContext, "Monthly")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("Y", MessageContainerList.GetMessage(utilities.ExecutionContext, "Yearly")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("B", MessageContainerList.GetMessage(utilities.ExecutionContext, "Birthday")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("A", MessageContainerList.GetMessage(utilities.ExecutionContext, "Anniversary")));
            log.LogMethodExit(cardGameFrequencyDataSource);
            return cardGameFrequencyDataSource;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            
            if (ValidateAccountGameDTOList())
            {
                OnDataLoadStart();
                if (Save())
                {
                    await LoadAccountGameDTOList();
                }
                OnDataLoadComplete();
            }
            log.LogMethodExit();
        }

        private bool ValidateAccountGameDTOList()
        {
            log.LogMethodEntry();
            bool valid = true;
            if(accountGameDTOListBS.DataSource != null && accountGameDTOListBS.DataSource is List<AccountGameDTO>)
            {
                List<AccountGameDTO> accountGameDTOList = accountGameDTOListBS.DataSource as List<AccountGameDTO>;
                for (int i = 0; i < accountGameDTOList.Count; i++)
                {
                    if(accountGameDTOList[i].AccountGameExtendedDTOList != null)
                    {
                        for (int j = 0; j < accountGameDTOList[i].AccountGameExtendedDTOList.Count; j++)
                        {
                            if(accountGameDTOList[i].AccountGameExtendedDTOList[j].IsChanged)
                            {
                                AccountGameExtendedBL accountGameExtendedBL = new AccountGameExtendedBL(utilities.ExecutionContext, accountGameDTOList[i].AccountGameExtendedDTOList[j]);
                                List<ValidationError> validationErrorList = accountGameExtendedBL.Validate();
                                if(validationErrorList != null && validationErrorList.Count > 0)
                                {
                                    valid = false;
                                    log.LogVariableState("AccountGameExtendedDTO", accountGameDTOList[i].AccountGameExtendedDTOList[j]);
                                    log.LogMethodExit(valid, "AccountGameExtendedDTO not valid");
                                    dgvAccountGameDTOList.Rows[i].Selected = true;
                                    dgvAccountGameExtendedDTOList.Rows[j].Selected = true;
                                    MessageBox.Show(GetValidationErrorMessage(validationErrorList));
                                    return valid;
                                }
                            }
                        }
                    }
                    if(accountGameDTOList[i].IsChanged)
                    {
                        AccountGameBL accountGameBL = new AccountGameBL(utilities.ExecutionContext, accountGameDTOList[i]);
                        List<ValidationError> validationErrorList = accountGameBL.Validate();
                        if (validationErrorList != null && validationErrorList.Count > 0)
                        {
                            valid = false;
                            dgvAccountGameDTOList.Rows[i].Selected = true;
                            MessageBox.Show(GetValidationErrorMessage(validationErrorList));
                            log.LogVariableState("AccountGameDTO", accountGameDTOList[i]);
                            log.LogMethodExit(valid, "AccountGameDTO not valid");
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
                log.Error("Error occured while loading account list.", ex);
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
                dgvAccountGameExtendedDTOList.EndEdit();
                dgvAccountGameDTOList.EndEdit();
                List<AccountGameDTO> accountGameDTOList = accountGameDTOListBS.DataSource as List<AccountGameDTO>;
                if (dgvAccountGameDTOList.SelectedRows.Count <= 0 && dgvAccountGameDTOList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                List<AccountGameExtendedDTO> accountGameExtendedDTOList = accountGameExtendedDTOListBS.DataSource as List<AccountGameExtendedDTO>;
                if (accountGameExtendedDTOList != null &&
                    accountGameExtendedDTOList.Count > 0 &&
                    dgvAccountGameExtendedDTOList.RowCount > 0)
                {

                    if (this.dgvAccountGameExtendedDTOList.SelectedCells.Count > 0)
                    {
                        foreach (DataGridViewCell cell in dgvAccountGameExtendedDTOList.SelectedCells)
                        {
                            if(dgvAccountGameExtendedDTOList.Rows[cell.RowIndex].IsNewRow == false)
                            {
                                dgvAccountGameExtendedDTOList.Rows[cell.RowIndex].Selected = true;
                            }
                        }
                    }
                    else
                    {
                        dgvAccountGameExtendedDTOList.Rows[dgvAccountGameExtendedDTOList.RowCount - 1].Selected = true;
                    }
                    foreach (DataGridViewRow row in dgvAccountGameExtendedDTOList.SelectedRows)
                    {
                        if(row.IsNewRow == false)
                        {
                            if (row.Cells["accountGameExtendedIdDataGridViewTextBoxColumn"].Value == null || Convert.ToInt32(row.Cells["accountGameExtendedIdDataGridViewTextBoxColumn"].Value.ToString()) <= 0)
                            {
                                dgvAccountGameExtendedDTOList.Rows.RemoveAt(row.Index);
                                rowsDeleted = true;
                            }
                            else
                            {
                                if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    refreshFromDB = true;
                                    AccountGameExtendedDTO accountGameExtendedDTO = accountGameExtendedDTOList[row.Index];
                                    accountGameExtendedDTO.IsActive = false;
                                }
                            }
                        }
                    }
                }
                if (rowsDeleted == false && refreshFromDB == false && accountGameDTOListBS.Current != null && accountGameDTOListBS.Current is AccountGameDTO)
                {
                    AccountGameDTO accountGameDTO = accountGameDTOListBS.Current as AccountGameDTO;
                    if (accountGameDTO.AccountGameId > 1)
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            refreshFromDB = true;
                            accountGameDTO.IsActive = false;
                        }
                    }
                    else
                    {
                        if (dgvAccountGameDTOList.CurrentRow.IsNewRow == false)
                        {
                            dgvAccountGameDTOList.Rows.RemoveAt(dgvAccountGameDTOList.CurrentRow.Index);
                            rowsDeleted = true;
                        }
                    }
                }

                if (confirmDelete && refreshFromDB)
                {
                    if (Save())
                    {
                        await LoadAccountGameDTOList();
                    }
                }
                if (rowsDeleted == true)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                }
                if (refreshFromDB == false && rowsDeleted == true)
                {
                    dgvAccountGameDTOList.Refresh();
                    dgvAccountGameExtendedDTOList.Refresh();
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
                log.Error("Error while saving accountDiscount.", ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(718));
            }
            
            log.LogMethodExit(result);
            return result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void dgvAccountGameDTOList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountGameDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            try
            {
                if (e.ColumnIndex == customDataDataGridViewButtonColumn.Index)
                {
                    if (customAttributesDTOList == null)
                    {
                        CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(utilities.ExecutionContext);
                        List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, Applicability.CARDGAMES.ToString()));
                        searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters);
                        if (customAttributesDTOList == null)
                        {
                            customAttributesDTOList = new List<CustomAttributesDTO>();
                        }
                    }
                    if (customAttributesDTOList.Count == 0)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 559, Applicability.CARDGAMES.ToString()));
                        return;
                    }
                    if(ValidateAccountGameDTOList())
                    {
                        if (e.RowIndex >= 0 &&
                        e.ColumnIndex == customDataDataGridViewButtonColumn.Index &&
                        accountGameDTOListBS.Current != null &&
                        accountGameDTOListBS.Current is AccountGameDTO)
                        {
                            AccountGameDTO accountGameDTO = accountGameDTOListBS.Current as AccountGameDTO;
                            CustomDataSetDTO customDataSetDTO = null;
                            if (accountGameDTO.CustomDataSetId > -1)
                            {
                                CustomDataSetBL customDataSetBL = new CustomDataSetBL(utilities.ExecutionContext, accountGameDTO.CustomDataSetId);
                                customDataSetDTO = customDataSetBL.CustomDataSetDTO;
                            }
                            else
                            {
                                customDataSetDTO = new CustomDataSetDTO();
                            }

                            CustomDataListUI customDataListUI = new CustomDataListUI(utilities, customDataSetDTO, Applicability.CARDGAMES);
                            customDataListUI.ShowDialog();
                            if (accountGameDTO.CustomDataSetId != customDataSetDTO.CustomDataSetId)
                            {
                                accountGameDTO.CustomDataSetId = customDataSetDTO.CustomDataSetId;
                                Save();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while displaying custom data", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvAccountGameExtendedDTOList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountGameExtendedDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            UpdateGameIdAndGameProfileIdColumnsOfAccountGameExtended(e);
            log.LogMethodExit();
        }

        private void UpdateGameIdAndGameProfileIdColumnsOfAccountGameExtended(DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountGameExtendedDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            try
            {
                if (e.ColumnIndex == gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.Index)
                {
                    if (dgvAccountGameExtendedDTOList.Rows[e.RowIndex].Cells[gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.Index].Value != null &&
                        Convert.ToInt32(dgvAccountGameExtendedDTOList.Rows[e.RowIndex].Cells[gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.Index].Value) > -1)
                    {
                        if (dgvAccountGameExtendedDTOList.Rows[e.RowIndex].Cells[gameIdAccountGameExtendedDataGridViewComboBoxColumn.Index].Value != DBNull.Value &&
                            Convert.ToInt32(dgvAccountGameExtendedDTOList.Rows[e.RowIndex].Cells[gameIdAccountGameExtendedDataGridViewComboBoxColumn.Index].Value) > -1)
                        {
                            dgvAccountGameExtendedDTOList.Rows[e.RowIndex].Cells[gameIdAccountGameExtendedDataGridViewComboBoxColumn.Index].Value = -1;
                        }
                        dgvAccountGameExtendedDTOList.Rows[e.RowIndex].Cells[gameIdAccountGameExtendedDataGridViewComboBoxColumn.Index].ReadOnly = true;
                    }
                    else
                    {
                        dgvAccountGameExtendedDTOList.Rows[e.RowIndex].Cells[gameIdAccountGameExtendedDataGridViewComboBoxColumn.Index].ReadOnly = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error  occured while making game combobox readonly", ex);
            }
            log.LogMethodExit();
        }

        private void UpdateGameIdAndGameProfileIdColumnsOfAccountGame(DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountGameDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            try
            {
                if (e.ColumnIndex == gameProfileIdAccountGameDataGridViewComboBoxColumn.Index)
                {
                    if (dgvAccountGameDTOList.Rows[e.RowIndex].Cells[gameProfileIdAccountGameDataGridViewComboBoxColumn.Index].Value != null &&
                        Convert.ToInt32(dgvAccountGameDTOList.Rows[e.RowIndex].Cells[gameProfileIdAccountGameDataGridViewComboBoxColumn.Index].Value) > -1)
                    {
                        if (dgvAccountGameDTOList.Rows[e.RowIndex].Cells[gameIdAccountGameDataGridComboBoxColumn.Index].Value != DBNull.Value &&
                            Convert.ToInt32(dgvAccountGameDTOList.Rows[e.RowIndex].Cells[gameIdAccountGameDataGridComboBoxColumn.Index].Value) > -1)
                        {
                            dgvAccountGameDTOList.Rows[e.RowIndex].Cells[gameIdAccountGameDataGridComboBoxColumn.Index].Value = -1;
                        }
                        dgvAccountGameDTOList.Rows[e.RowIndex].Cells[gameIdAccountGameDataGridComboBoxColumn.Index].ReadOnly = true;
                    }
                    else
                    {
                        dgvAccountGameDTOList.Rows[e.RowIndex].Cells[gameIdAccountGameDataGridComboBoxColumn.Index].ReadOnly = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error  occured while making game combobox readonly", ex);
            }
            log.LogMethodExit();
        }

        private void dgvAccountGameExtendedDTOList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountGameExtendedDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            UpdateGameIdAndGameProfileIdColumnsOfAccountGameExtended(e);
            log.LogMethodExit();
        }

        private void dgvAccountGameDTOList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountGameDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            UpdateGameIdAndGameProfileIdColumnsOfAccountGame(e);
            log.LogMethodExit();
        }

        private void dgvAccountGameDTOList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= dgvAccountGameDTOList.RowCount)
            {
                log.LogMethodExit(null, string.Format("Not a valid RowIndex:{0}, Column Index:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            if (e.ColumnIndex == quantityDataGridViewTextBoxColumn.Index)
            {
                dgvAccountGameDTOList.Rows[e.RowIndex].Cells[balanceGamesDataGridViewTextBoxColumn.Index].Value = dgvAccountGameDTOList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
            else
            {
                UpdateGameIdAndGameProfileIdColumnsOfAccountGame(e);
            }
            log.LogMethodExit();
        }

        private void dgvAccountGameDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvAccountGameDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvAccountGameExtendedDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvAccountGameExtendedDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void accountGameDTOListBS_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            AccountGameDTO accountGameDTO = new AccountGameDTO();
            accountGameDTO.AccountGameExtendedDTOList = new List<AccountGameExtendedDTO>();
            e.NewObject = accountGameDTO;
            log.LogMethodExit();
        }

        private void accountGameDTOListBS_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateAccountGameExtendedDataSource();
            log.LogMethodExit();
        }

        private void UpdateAccountGameExtendedDataSource()
        {
            log.LogMethodEntry();
            try
            {
                if (accountGameDTOListBS.Current != null && accountGameDTOListBS.Current is AccountGameDTO)
                {
                    AccountGameDTO accountGameDTO = accountGameDTOListBS.Current as AccountGameDTO;
                    accountGameExtendedDTOListBS.DataSource = accountGameDTO.AccountGameExtendedDTOList;
                    if (accountGameDTO.AccountGameId > -1)
                    {
                        dgvAccountGameExtendedDTOList.Enabled = true;
                        dgvAccountGameExtendedDTOList.AllowUserToAddRows = true;
                    }
                    else
                    {
                        dgvAccountGameExtendedDTOList.Enabled = false;
                        dgvAccountGameExtendedDTOList.AllowUserToAddRows = false;
                    }
                }
                else
                {
                    dgvAccountGameExtendedDTOList.Enabled = false;
                    dgvAccountGameExtendedDTOList.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while setting the accountGameExtendedDTOListBS datasource", ex);

            }
            log.LogMethodExit();
        }

        private void dgvAccountGameDTOList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvAccountGameDTOList.CurrentCell.ColumnIndex == gameProfileIdAccountGameDataGridViewComboBoxColumn.Index)
            {
                dgvAccountGameDTOList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            log.LogMethodExit();
        }

        private void dgvAccountGameExtendedDTOList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvAccountGameExtendedDTOList.CurrentCell.ColumnIndex == gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.Index)
            {
                dgvAccountGameExtendedDTOList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            log.LogMethodExit();
        }

        private void accountGameDTOListBS_DataSourceChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateAccountGameExtendedDataSource();
            log.LogMethodExit();
        }
    }
}
