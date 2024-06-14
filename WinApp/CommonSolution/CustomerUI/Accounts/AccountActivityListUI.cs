using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Account activity list UI class
    /// </summary>
    public partial class AccountActivityListUI : Form
    {
        private int accountId;
        private string tagNumber;
        private bool showDetailedGameMetricData;
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="accountId"></param>
        /// <param name="tagNumber"></param>
        public AccountActivityListUI(Utilities utilities, int accountId, string tagNumber)
        {
            log.LogMethodEntry(utilities, accountId, tagNumber);
            this.accountId = accountId;
            this.tagNumber = tagNumber;
            this.utilities = utilities;
            InitializeComponent();
            utilities.setLanguage(this);
            utilities.setupDataGridProperties(ref dgvGamePlayDTOList);
            utilities.setupDataGridProperties(ref dgvAccountActivityDTOList);
            this.Text = this.Text + " " + tagNumber;
            this.Height = Screen.PrimaryScreen.Bounds.Height - 30;
            showDetailedGameMetricData = false;
            lnkShowHideExtended.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Show Extended");
            cPCardBalanceDataGridViewTextBoxColumn.Visible = false;
            cPBonusDataGridViewTextBoxColumn.Visible = false;
            cPCreditsDataGridViewTextBoxColumn.Visible = false;
            cardGameDataGridViewTextBoxColumn.Visible = false;
            btnExportToExcelGamePlay.Width = btnExportToExcelPurchases.Width = 115;

            dateAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle = 
            playDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            amountAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle =
            courtesyAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle =
            bonusAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle =
            timeAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle =
            loyaltyPointsAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle =
            priceAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle =
            creditsAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle =
            bonusDataGridViewTextBoxColumn.DefaultCellStyle =
            creditsDataGridViewTextBoxColumn.DefaultCellStyle =
            courtesyDataGridViewTextBoxColumn.DefaultCellStyle =
            timeDataGridViewTextBoxColumn.DefaultCellStyle =
            cPCardBalanceDataGridViewTextBoxColumn.DefaultCellStyle =
            cPBonusDataGridViewTextBoxColumn.DefaultCellStyle =
            cPCreditsDataGridViewTextBoxColumn.DefaultCellStyle =
            cardGameDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();

            quantityAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle =
            tockensAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle =
            ticketsAccountActivityDataGridViewTextBoxColumn.DefaultCellStyle =
            eTicketsDataGridViewTextBoxColumn.DefaultCellStyle =
            manualTicketsDataGridViewTextBoxColumn.DefaultCellStyle =
            ticketCountDataGridViewTextBoxColumn.DefaultCellStyle =
            ticketEarterTicketsDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            ticketsAccountActivityDataGridViewTextBoxColumn.HeaderText = utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT;

            ThemeUtils.SetupVisuals(this);
            log.LogMethodExit();
        }

        private async void CardActivity_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAccountActivityDTOList.Enabled = false;
            dgvGamePlayDTOList.Enabled = false;
            lblStatus.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);
            OnDataLoadStart();
            await RefreshData();
            OnDataLoadComplete();
            dgvAccountActivityDTOList.Enabled = true;
            dgvGamePlayDTOList.Enabled = true;
            log.LogMethodExit();
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
            btnRefresh.Enabled = false;
            btnClose.Enabled = false;
            btnExportToExcelGamePlay.Enabled = false;
            btnExportToExcelPurchases.Enabled = false;
            lnkConsolidated.Enabled = false;
            lnkShowHideExtended.Enabled = false;
            RefreshControls();
            log.LogMethodExit();
        }

        private void EnableControls()
        {
            log.LogMethodEntry();
            btnRefresh.Enabled = true;
            btnClose.Enabled = true;
            btnExportToExcelGamePlay.Enabled = true;
            btnExportToExcelPurchases.Enabled = true;
            lnkConsolidated.Enabled = true;
            lnkShowHideExtended.Enabled = true;
            RefreshControls();
            log.LogMethodExit();
        }

        private void RefreshControls()
        {
            log.LogMethodEntry();
            btnRefresh.Refresh();
            btnClose.Refresh();
            btnExportToExcelGamePlay.Refresh();
            btnExportToExcelPurchases.Refresh();
            lnkConsolidated.Refresh();
            lnkShowHideExtended.Refresh();
            lblStatus.Refresh();
            log.LogMethodExit();
        }

        private async Task RefreshData()
        {
            log.LogMethodEntry();
            try
            {
                await LoadAccountActivityDTOList();
                await LoadGamePlayDTOList();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading data", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private async Task LoadAccountActivityDTOList()
        {
            log.LogMethodEntry();
            try
            {
                SortableBindingList<AccountActivityDTO> accountActivityDTOSortableBindingList = await Task<SortableBindingList<AccountActivityDTO>>.Factory.StartNew(() => { return GetAccountActivityDTOList(accountId); });
                accountActivityDTOListBS.DataSource = accountActivityDTOSortableBindingList;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading data", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private async Task LoadGamePlayDTOList()
        {
            log.LogMethodEntry();
            try
            {
                SortableBindingList<GamePlayDTO> gamePlayDTOSortableBindingList = await Task<SortableBindingList<GamePlayDTO>>.Factory.StartNew(() => { return GetGamePlayDTOList(accountId, showDetailedGameMetricData); });
                gamePlayDTOListBS.DataSource = gamePlayDTOSortableBindingList;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading data", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private SortableBindingList<AccountActivityDTO> GetAccountActivityDTOList(int accountId)
        {
            log.LogMethodEntry();
            SortableBindingList<AccountActivityDTO> accountActivityDTOSortableBindingList;
            List<AccountActivityDTO> accountActivityDTOList = null;
            AccountActivityViewListBL accountActivityViewListBL = new AccountActivityViewListBL(utilities.ExecutionContext);
            List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));
            accountActivityDTOList = accountActivityViewListBL.GetAccountActivityDTOList(searchByParameters);
            if(accountActivityDTOList == null)
            {
                accountActivityDTOSortableBindingList = new SortableBindingList<AccountActivityDTO>();
            }
            else
            {
                accountActivityDTOSortableBindingList = new SortableBindingList<AccountActivityDTO>(accountActivityDTOList);
            }
            log.LogMethodExit(accountActivityDTOSortableBindingList);
            return accountActivityDTOSortableBindingList;
        }

        private SortableBindingList<GamePlayDTO> GetGamePlayDTOList(int accountId, bool detailed)
        {
            log.LogMethodEntry();
            SortableBindingList<GamePlayDTO> gamePlayDTOSortableBindingList;
            List<GamePlayDTO> gamePlayDTOList = null;
            GamePlaySummaryListBL accountGameMetricViewListBL = new GamePlaySummaryListBL(utilities.ExecutionContext);
            List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountId.ToString()));
            gamePlayDTOList = accountGameMetricViewListBL.GetGamePlayDTOList(searchByParameters, true, detailed);
            if (gamePlayDTOList == null)
            {
                gamePlayDTOSortableBindingList = new SortableBindingList<GamePlayDTO>();
            }
            else
            {
                gamePlayDTOSortableBindingList = new SortableBindingList<GamePlayDTO>(gamePlayDTOList);
            }
            log.LogMethodExit(gamePlayDTOSortableBindingList);
            return gamePlayDTOSortableBindingList;
        }


        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            OnDataLoadStart();
            await RefreshData();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void btnExportToExcelPurchases_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SortableBindingList<AccountActivityDTO> accountActivityDTOList = accountActivityDTOListBS.DataSource as SortableBindingList<AccountActivityDTO>;
            if(accountActivityDTOList == null || accountActivityDTOList.Count <= 0)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1600));
                log.LogMethodExit(null, "nothing to export");
                return;
            }
            try
            {
                AccountActivityDTODefinition accountActivityDTODefinition = new AccountActivityDTODefinition(utilities.ExecutionContext, "", utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT);
                using (AccountActivityExportUI accountActivityExportUI = new AccountActivityExportUI(utilities, accountId, tagNumber, accountActivityDTOList, accountActivityDTODefinition))
                {
                    accountActivityExportUI.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while exporting account purchase data.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnExportToExcelGamePlay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SortableBindingList<GamePlayDTO> gamePlayDTOList = gamePlayDTOListBS.DataSource as SortableBindingList<GamePlayDTO>;
            if (gamePlayDTOList == null || gamePlayDTOList.Count <= 0)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1600));
                log.LogMethodExit(null, "nothing to export");
                return;
            }
            try
            {
                GamePlayDTODefinition gamePlayDTODefinition = new GamePlayDTODefinition(utilities.ExecutionContext, "", showDetailedGameMetricData);
                AccountGamePlayExportUI accountGamePlayExportUI = new AccountGamePlayExportUI(utilities, accountId, tagNumber, gamePlayDTOList, gamePlayDTODefinition);
                accountGamePlayExportUI.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while exporting account game play data.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private async void lnkShowHideExtended_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            showDetailedGameMetricData = !showDetailedGameMetricData;
            if (showDetailedGameMetricData)
            {

                lnkShowHideExtended.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Hide Extended");
                cPCardBalanceDataGridViewTextBoxColumn.Visible = true;
                cPBonusDataGridViewTextBoxColumn.Visible = true;
                cPCreditsDataGridViewTextBoxColumn.Visible = true;
                cardGameDataGridViewTextBoxColumn.Visible = true;
            }
            else
            {
                lnkShowHideExtended.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Show Extended");
                cPCardBalanceDataGridViewTextBoxColumn.Visible = false;
                cPBonusDataGridViewTextBoxColumn.Visible = false;
                cPCreditsDataGridViewTextBoxColumn.Visible = false;
                cardGameDataGridViewTextBoxColumn.Visible = false;
            }
            OnDataLoadStart();
            await LoadGamePlayDTOList();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private async void lnkConsolidated_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                OnDataLoadStart();
                AccountActivityHelper accountActivityHelper = await Task<AccountActivityHelper>.Factory.StartNew(() => { return GetInitializedAccountActivityHelper(accountId, tagNumber, utilities.ParafaitEnv.SiteId); });
                SortableBindingList<AccountActivityDTO> accountActivityDTOSortableBindingList = await Task<SortableBindingList<AccountActivityDTO>>.Factory.StartNew(() => { return GetConsolidatedAccountActivityDTOList(accountActivityHelper, accountId); });
                accountActivityDTOListBS.DataSource = accountActivityDTOSortableBindingList;
                SortableBindingList<GamePlayDTO> gamePlayDTOSortableBindingList = await Task<SortableBindingList<GamePlayDTO>>.Factory.StartNew(() => { return GetConsolidatedGamePlayDTOList(accountActivityHelper, accountId, showDetailedGameMetricData); });
                gamePlayDTOListBS.DataSource = gamePlayDTOSortableBindingList;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while geeting server account activity", ex);
                string message = "";
                if(ex.InnerException != null)
                {
                    if(ex.InnerException.InnerException != null)
                    {
                        message = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        message = ex.InnerException.Message;
                    }
                }
                else
                {
                    message = ex.Message;
                }
                MessageBox.Show(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Remoting Error"));
            }
            finally
            {
                OnDataLoadComplete();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private SortableBindingList<AccountActivityDTO> GetConsolidatedAccountActivityDTOList(AccountActivityHelper accountActivityHelper, int accountId)
        {
            log.LogMethodEntry(accountActivityHelper);
            SortableBindingList<AccountActivityDTO> accountActivityDTOSortableBindingList;
            List<AccountActivityDTO> accountActivityDTOList = null;
            AccountActivityViewListBL accountActivityViewListBL = new AccountActivityViewListBL(utilities.ExecutionContext, accountActivityHelper);
            List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));
            accountActivityDTOList = accountActivityViewListBL.GetConsolidatedAccountActivityDTOList(searchByParameters);
            if (accountActivityDTOList == null)
            {
                accountActivityDTOSortableBindingList = new SortableBindingList<AccountActivityDTO>();
            }
            else
            {
                accountActivityDTOSortableBindingList = new SortableBindingList<AccountActivityDTO>(accountActivityDTOList);
            }
            log.LogMethodExit(accountActivityDTOSortableBindingList);
            return accountActivityDTOSortableBindingList;
        }

        private SortableBindingList<GamePlayDTO> GetConsolidatedGamePlayDTOList(AccountActivityHelper accountActivityHelper, int accountId, bool detailed)
        {
            log.LogMethodEntry();
            SortableBindingList<GamePlayDTO> gamePlayDTOSortableBindingList;
            List<GamePlayDTO> gamePlayDTOList = null;
            GamePlaySummaryListBL accountGameMetricViewListBL = new GamePlaySummaryListBL(utilities.ExecutionContext, accountActivityHelper);
            List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountId.ToString()));
            gamePlayDTOList = accountGameMetricViewListBL.GetConsolidatedGamePlayDTOList(searchByParameters, true, detailed);
            if (gamePlayDTOList == null)
            {
                gamePlayDTOSortableBindingList = new SortableBindingList<GamePlayDTO>();
            }
            else
            {
                gamePlayDTOSortableBindingList = new SortableBindingList<GamePlayDTO>(gamePlayDTOList);
            }
            log.LogMethodExit(gamePlayDTOSortableBindingList);
            return gamePlayDTOSortableBindingList;
        }

        private AccountActivityHelper GetInitializedAccountActivityHelper(int accountId, string tagNumber, int siteId)
        {
            log.LogMethodEntry(accountId, tagNumber, siteId);
            AccountActivityHelper accountActivityHelper = new AccountActivityHelper(accountId, tagNumber, siteId);
            accountActivityHelper.Initialize();
            log.LogMethodExit(accountActivityHelper);
            return accountActivityHelper;
        }

        private void dgvGamePlayDTOList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (gamePlayDTOListBS.DataSource != null && gamePlayDTOListBS.DataSource is SortableBindingList<GamePlayDTO>)
                {
                    SortableBindingList<GamePlayDTO> gamePlayDTOSortableBindingList = gamePlayDTOListBS.DataSource as SortableBindingList<GamePlayDTO>;
                    if (gamePlayDTOSortableBindingList != null &&
                        gamePlayDTOSortableBindingList.Count > 0)
                    {
                        for (int i = 0; i < gamePlayDTOSortableBindingList.Count; i++)
                        {
                            if (gamePlayDTOSortableBindingList[i].TaskId > -1)
                            {
                                dgvGamePlayDTOList.Rows[i].DefaultCellStyle.BackColor = Color.Tomato;
                                dgvGamePlayDTOList.Rows[i].DefaultCellStyle.SelectionBackColor = Color.DarkOrange;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while changing back and selection color of a data row", ex);
            }
            log.LogMethodExit();
        }

        private void dgvGamePlayDTOList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == playDateDataGridViewTextBoxColumn.Index)
            {
                if (e.Value != null && e.Value is DateTime)
                {
                    if((DateTime)e.Value == DateTime.MinValue)
                    {
                        e.Value = "";
                        e.FormattingApplied = true;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void AccountActivityListUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while disposing the form", ex);
            }
            log.LogMethodExit();
        }
    }
}
