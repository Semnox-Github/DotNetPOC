using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Account Audit UI Class
    /// </summary>
    public partial class AccountAuditListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AccountDTO accountDTO;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public AccountAuditListUI(Utilities utilities, AccountDTO accountDTO)
        {
            log.LogMethodEntry(utilities, accountDTO);
            InitializeComponent();
            this.utilities = utilities;
            this.accountDTO = accountDTO;
            ShowInTaskbar = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - 60;
            Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 40;

            utilities.setupDataGridProperties(ref dgvAccountAuditDTOList);
            utilities.setLanguage(this);
            this.Text = this.Text + " " + accountDTO.TagNumber;

            ticketCountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            siteIdDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            techGamesDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            uploadSiteIdDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            downloadBatchIdDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();

            issueDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            refundDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            startTimeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            lastPlayedTimeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            uploadTimeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            expiryDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            refreshFromHQTimeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();

            creditsDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            courtesyDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            bonusDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            timeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            creditsPlayedDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            loyaltyPointsDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();

            ThemeUtils.SetupVisuals(this);

            log.LogMethodExit();
        }

        private async void AccountAuditListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                await RefreshData();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading account audit list.", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private async Task RefreshData()
        {
            log.LogMethodEntry();
            await LoadAccountAuditDTOList();
            log.LogMethodExit();
        }

        private async Task LoadAccountAuditDTOList()
        {
            log.LogMethodEntry();
            IList<AccountAuditDTO> accountAuditDTOList = null;
            accountAuditDTOList = await Task<List<AccountAuditDTO>>.Factory.StartNew(() => { return GetAccountAuditDTOList(accountDTO.AccountId); });
            if (accountAuditDTOList == null)
            {
                accountAuditDTOList = new SortableBindingList<AccountAuditDTO>();
            }
            else
            {
                accountAuditDTOList = new SortableBindingList<AccountAuditDTO>(accountAuditDTOList);
            }
            accountAuditDTOListBS.DataSource = accountAuditDTOList;
            try
            {
                for (int i = 1; i < dgvAccountAuditDTOList.Columns.Count; i++)
                {
                    for (int j = 1; j < dgvAccountAuditDTOList.Rows.Count; j++)
                    {
                        if(dgvAccountAuditDTOList[i, j].Value != null || dgvAccountAuditDTOList[i, j - 1].Value != null)
                        {
                            if ((dgvAccountAuditDTOList[i, j].Value == null && dgvAccountAuditDTOList[i, j - 1].Value != null) ||
                            (dgvAccountAuditDTOList[i, j].Value != null && dgvAccountAuditDTOList[i, j - 1].Value == null) ||
                            (!dgvAccountAuditDTOList[i, j].Value.Equals(dgvAccountAuditDTOList[i, j - 1].Value)))
                                dgvAccountAuditDTOList[i, j].Style.BackColor = Color.OrangeRed;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating value change", ex);
            }
            log.LogMethodExit();
        }

        private List<AccountAuditDTO> GetAccountAuditDTOList(int accountId)
        {
            log.LogMethodEntry(accountId);
            List<AccountAuditDTO> accountAuditDTOList = null;
            try
            {
                AccountAuditListBL accountAuditListBL = new AccountAuditListBL(utilities.ExecutionContext);
                List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>> seachAccountAuditParameters = new List<KeyValuePair<AccountAuditDTO.SearchByParameters, string>>();
                seachAccountAuditParameters.Add(new KeyValuePair<AccountAuditDTO.SearchByParameters, string>(AccountAuditDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));
                accountAuditDTOList = accountAuditListBL.GetAccountAuditDTOList(seachAccountAuditParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading the accounts", ex);
            }
            log.LogMethodExit(accountAuditDTOList);
            return accountAuditDTOList;
        }
    }
}
