/********************************************************************************************
 * Class Name -  Transaction                                                                         
 * Description - TrxUserLogs UI 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Semnox.Parafait.Transaction
{
    public partial class TrxUserLogsUI : Form
    {
        Utilities Utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        int trxId;

        public TrxUserLogsUI(Utilities utilities, int trxId)
        {
            log.LogMethodEntry(utilities, trxId);
            InitializeComponent();
            Utilities = utilities;
            this.trxId = trxId;
            utilities.setupDataGridProperties(ref dgvViewTransaction);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void TrxUserLogsUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtMessage.Text = "";
            this.Text += " for transaction id" + ":" + trxId.ToString();
            activityDateDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            approvalTimeDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            RefreshData();
            log.LogMethodExit();
        }
        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadTrxUserLogsDTOList();
            log.LogMethodExit();
        }
        private void LoadTrxUserLogsDTOList()
        {
            log.LogMethodEntry();
            TrxUserLogsList trxUserLogs = new TrxUserLogsList(machineUserContext);
            List<KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>(TrxUserLogsDTO.SearchByParameters.TRX_ID, trxId.ToString()));
            List<TrxUserLogsDTO> trxUserLogsDTOList = trxUserLogs.GetAllTrxUserLogs(searchParameters);
            List<TrxUserLogsDTO> updatedTrxUserLogsDTOList = null;
            if (trxUserLogsDTOList != null)
            {
                updatedTrxUserLogsDTOList = trxUserLogs.GetUpdatedTrxUserLogs(trxUserLogsDTOList);
            }
            else
            {
                txtMessage.Text = Utilities.MessageUtils.getMessage("Transaction logs are not available");
                return;
            }
            SortableBindingList<TrxUserLogsDTO> trxUserLogsDTOSortableList;
            if (updatedTrxUserLogsDTOList != null)
            {
                trxUserLogsDTOSortableList = new SortableBindingList<TrxUserLogsDTO>(updatedTrxUserLogsDTOList);
            }
            else
            {
                trxUserLogsDTOSortableList = new SortableBindingList<TrxUserLogsDTO>();
                
            }
            trxUserLogsDTOBindingSource.DataSource = trxUserLogsDTOSortableList;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }
    }
}
