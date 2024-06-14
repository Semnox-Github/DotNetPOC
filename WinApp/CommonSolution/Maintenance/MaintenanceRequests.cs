/********************************************************************************************
 * Project Name - Maintenance Requests UI
 * Description  - User interface for maintenance requests
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        03-Feb-2016   Raghuveera     Created 
 *2.70        12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Globalization;

namespace Semnox.Parafait.Maintenance
{
    public partial class MaintenanceRequests : Form
    {
        int AssetId;
        int jobTypeId;
        AssetList assetList;
        List<GenericAssetDTO> genericAssetListOnDisplay;
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BindingSource userJobItemsBS;
        List<LookupValuesDTO> jobTypeDTOList;
        List<LookupValuesDTO> statusDTOList;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public MaintenanceRequests(Utilities _Utilities, int pAssetId = -1)
        {
            log.LogMethodEntry(_Utilities, pAssetId);
            AssetId = pAssetId;
            InitializeComponent();
            utilities = _Utilities;

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
            jobTypeId = GetJobTypeID();
            log.LogMethodExit();
        }
        /// <summary>
        /// To get the Job type id
        /// </summary>
        private int GetJobTypeID()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupOpenValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"));
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Service Request"));
                List<LookupValuesDTO> lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupOpenValuesSearchParams);
                if (lookupOpenValuesDTOList == null || lookupOpenValuesDTOList.Count == 0)
                {
                    log.LogMethodExit("lookupOpenValuesDTOList == null || lookupOpenValuesDTOList.Count == 0");
                    return -1;
                }
                log.LogMethodExit(lookupOpenValuesDTOList[0].LookupValueId);
                return lookupOpenValuesDTOList[0].LookupValueId;
            }
            catch (Exception e)
            {
                log.Error(e);
                log.LogMethodExit(-1);
                return -1;
            }
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> userJobItemsSearchParams = new List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>>();
            if (jobTypeId != -1)
            {
                userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_TYPE, (jobTypeId == -1) ? "" : jobTypeId.ToString()));
            }
            if (String.IsNullOrEmpty(txtRequestTitle.Text) == false)
            {
                userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_NAME, txtRequestTitle.Text));
            }
            if (chbShowActiveEntries.Checked)
            {
                userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE, "1"));
            }
            //maintenanceJobSearchParams.Add(new KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>(MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSIGNED_TO, utilities.ParafaitEnv.User_Id.ToString()));
            if (cmbAsset.SelectedValue != null && !cmbAsset.SelectedValue.ToString().Equals("-1"))
                userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.ASSET_ID, (cmbAsset.SelectedValue == null || cmbAsset.SelectedValue.ToString().Equals("-1")) ? "" : cmbAsset.SelectedValue.ToString()));
            if (cmbRequestType.SelectedValue != null && !cmbRequestType.SelectedValue.ToString().Equals("-1"))
                userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TYPE_ID, (cmbRequestType.SelectedValue.ToString().Equals("-1")) ? "" : cmbRequestType.SelectedValue.ToString()));
            if (cmbStatus.SelectedValue != null && !cmbStatus.SelectedValue.ToString().Equals("-1"))
                userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.STATUS, ((cmbStatus.SelectedValue == null || cmbStatus.SelectedValue.ToString().Equals("-1"))) ? "" : cmbStatus.SelectedValue.ToString()));
            if (cmbPriority.SelectedValue != null && !cmbPriority.SelectedValue.ToString().Equals("-1"))
                userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.PRIORITY, ((cmbPriority.SelectedValue == null || cmbPriority.SelectedValue.ToString().Equals("-1"))) ? "" : cmbPriority.SelectedValue.ToString()));
            try
            {
                if (!string.IsNullOrEmpty(txtScheduleFrom.Text))
                {
                    userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_FROM_DATE, DateTime.Parse(txtScheduleFrom.Text).ToString("yyyy-MM-dd")));
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(utilities.MessageUtils.getMessage(970, new object[1] { utilities.MessageUtils.getMessage("Schedule from") }));
                log.Error(ex);
                log.LogMethodExit();
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtScheduleTo.Text))
                {
                    if (DateTime.Parse(txtScheduleFrom.Text).CompareTo(DateTime.Parse(txtScheduleTo.Text)) <= 0)
                    {
                        userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_TO_DATE, DateTime.Parse(txtScheduleTo.Text).ToString("yyyy-MM-dd")));
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(971));
                        log.LogMethodExit(utilities.MessageUtils.getMessage(971));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string msg = utilities.MessageUtils.getMessage(970, new object[1] { utilities.MessageUtils.getMessage("Schedule To") });
                MessageBox.Show(msg);
                log.LogMethodExit(msg);
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtRequestFrom.Text))
                {
                    userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_FROM_DATE, DateTime.Parse(txtRequestFrom.Text).ToString("yyyy-MM-dd")));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string msg = utilities.MessageUtils.getMessage(970, new object[1] { utilities.MessageUtils.getMessage("Request from") });
                MessageBox.Show(msg);
                log.LogMethodExit(msg);
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtRequestTo.Text))
                {
                    if (DateTime.Parse(txtRequestFrom.Text).CompareTo(DateTime.Parse(txtRequestTo.Text)) <= 0)
                    {
                        userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TO_DATE, DateTime.Parse(txtRequestTo.Text).ToString("yyyy-MM-dd")));
                    }
                    else
                    {
                        string msg = utilities.MessageUtils.getMessage(971);
                        MessageBox.Show(msg);
                        log.LogMethodExit(msg);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string msg = utilities.MessageUtils.getMessage(970, new object[1] { utilities.MessageUtils.getMessage("Request to") });
                MessageBox.Show(msg);
                log.LogMethodExit(msg);
                return;
            }
            userJobItemsSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            UserJobItemsListBL userJobItemsListBL = new UserJobItemsListBL(machineUserContext);
            userJobItemsBS = new BindingSource();
            List<UserJobItemsDTO> userJobItemsDTOList = userJobItemsListBL.GetAllUserJobItemDTOList(userJobItemsSearchParams, utilities.ParafaitEnv.User_Id);

            if (userJobItemsDTOList != null)
            {
                for (int i = 0; i < userJobItemsDTOList.Count; i++)
                {
                    try
                    {
                        userJobItemsDTOList[i].RequestDate = string.IsNullOrEmpty(userJobItemsDTOList[i].RequestDate) ? "" : DateTime.Parse(userJobItemsDTOList[i].RequestDate).ToString(utilities.getDateTimeFormat());
                    }
                    catch { }
                    try
                    {
                        userJobItemsDTOList[i].ChecklistCloseDate = string.IsNullOrEmpty(userJobItemsDTOList[i].ChecklistCloseDate) ? "" : DateTime.Parse(userJobItemsDTOList[i].ChecklistCloseDate).ToString(utilities.getDateTimeFormat());
                    }
                    catch { }
                }
                SortableBindingList<UserJobItemsDTO> userJobItemsDTOSortList = new SortableBindingList<UserJobItemsDTO>(userJobItemsDTOList);
                userJobItemsBS.DataSource = userJobItemsDTOList;
            }
            else
                userJobItemsBS.DataSource = new List<UserJobItemsDTO>();

            dgvMaintenanceRequests.DataSource = userJobItemsBS;

            dgvMaintenanceRequests.DataError += new DataGridViewDataErrorEventHandler(dgvMaintenanceRequests_ComboDataError);
            utilities.setupDataGridProperties(ref dgvMaintenanceRequests);
            requestDateDataGridViewTextBoxColumn.DefaultCellStyle = checklistCloseDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            dgvMaintenanceRequests.BorderStyle = BorderStyle.FixedSingle;
            utilities.setLanguage(dgvMaintenanceRequests);
        }

        void dgvMaintenanceRequests_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == dgvMaintenanceRequests.Columns["statusDataGridViewTextBoxColumn"].Index)
            {
                if (statusDTOList != null)
                    dgvMaintenanceRequests.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = statusDTOList[0].LookupValueId;
            }
            else if (e.ColumnIndex == dgvMaintenanceRequests.Columns["maintJobTypeDataGridViewTextBoxColumn"].Index)
            {
                if (jobTypeDTOList != null)
                    dgvMaintenanceRequests.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = jobTypeDTOList[0].LookupValueId;
            }
            log.LogMethodExit();
        }
        private void MaintenanceRequests_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnDelete.Enabled = utilities.ParafaitEnv.Manager_Flag.Equals("Y");
            btnRefresh.Top = btnEdit.Top;
            btnExport.Size = new Size(97, 23);
            GetAssets();
            GetRequestTypes();
            GetRequestStatus();
            GetRequestPriority();
            btnFind.Top = dtpScheduleFrom.Bottom - btnFind.Height + 1;
            btnFind.Left = cmbPriority.Right - btnFind.Width;
            btnRefresh.Top = btnNewRequest.Top;
            btnFind.PerformClick();
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads asset to the Comboboxes
        /// </summary>
        private void GetAssets()
        {
            log.LogMethodEntry();
            assetList = new AssetList(machineUserContext);
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
            assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, "Y"));
            assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            genericAssetListOnDisplay = assetList.GetAllAssets(assetSearchParams);
            if (genericAssetListOnDisplay != null)
            {
                BindingSource binding = new BindingSource();
                binding.DataSource = genericAssetListOnDisplay;
                genericAssetListOnDisplay.Insert(0, new GenericAssetDTO());

                cmbAsset.DataSource = binding;
                cmbAsset.ValueMember = "AssetId";
                cmbAsset.DisplayMember = "Name";
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the status to the comboboxes
        /// </summary>       
        private void GetRequestTypes()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_REQUEST_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                jobTypeDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (jobTypeDTOList != null)
                {
                    BindingSource bindingSource = new BindingSource();
                    jobTypeDTOList.Insert(0, new LookupValuesDTO());
                    //lookupValuesDTOList[0].Description = utilities.MessageUtils.getMessage("None");
                    bindingSource.DataSource = jobTypeDTOList;
                    cmbRequestType.DataSource = bindingSource;
                    cmbRequestType.ValueMember = "LookupValueId";
                    cmbRequestType.DisplayMember = "LookupValue";
                    requestTypeDataGridViewTextBoxColumn.DataSource = jobTypeDTOList;
                    requestTypeDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                    requestTypeDataGridViewTextBoxColumn.DisplayMember = "LookupValue";

                }
                else
                {
                    MessageBox.Show("'MAINT_REQUEST_TYPE' is not found in Lookups");
                } 
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the status
        /// </summary>
        private void GetRequestStatus()
        {
            log.LogMethodEntry();
            try
            {
                cmbStatus.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbStatus.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                statusDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (statusDTOList != null)
                {
                    BindingSource bindingSource = new BindingSource();
                    statusDTOList.Insert(0, new LookupValuesDTO());
                    bindingSource.DataSource = statusDTOList;
                    cmbStatus.DataSource = bindingSource;
                    cmbStatus.ValueMember = "LookupValueId";
                    cmbStatus.DisplayMember = "LookupValue";
                    statusDataGridViewTextBoxColumn.DataSource = statusDTOList;
                    statusDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                    statusDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupOpenValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Open"));
                    List<LookupValuesDTO> lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupOpenValuesSearchParams);
                    cmbStatus.SelectedValue = lookupOpenValuesDTOList[0].LookupValueId;

                }
                else
                {
                    MessageBox.Show("'MAINT_JOB_STATUS' is not found in Lookups");
                } 
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the priority combobox
        /// </summary>
        private void GetRequestPriority()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_PRIORITY"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList != null)
                {
                    BindingSource bindingSource = new BindingSource();
                    lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                    bindingSource.DataSource = lookupValuesDTOList;
                    cmbPriority.DataSource = bindingSource;
                    cmbPriority.ValueMember = "LookupValueId";
                    cmbPriority.DisplayMember = "LookupValue";
                }
                else
                {
                    MessageBox.Show("'MAINT_JOB_PRIORITY' is not found in Lookups");
                } 
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }

        private void SettheSelectedRecord(int id)
        {
            log.LogMethodEntry(id);
            for (int i = 0; i < dgvMaintenanceRequests.Rows.Count - 1; i++)
            {
                if (dgvMaintenanceRequests.Rows[i].Cells["maintChklstdetIdDataGridViewTextBoxColumn"].Value.ToString() == id.ToString())
                {
                    dgvMaintenanceRequests.CurrentCell = dgvMaintenanceRequests.Rows[i].Cells["maintChklstdetIdDataGridViewTextBoxColumn"];
                }
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void btnNewRequest_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateMaintenanceRequest umr = new UpdateMaintenanceRequest(utilities);
            umr.ShowDialog();
            btnFind.PerformClick();
            log.LogMethodExit();
        }

        private void dgvMaintenanceRequests_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    log.LogMethodExit("e.ColumnIndex < 0 || e.RowIndex < 0)");
                    return;
                }
                int requestId = (int)dgvMaintenanceRequests["maintChklstdetIdDataGridViewTextBoxColumn", e.RowIndex].Value;
                UpdateMaintenanceRequest umr = new UpdateMaintenanceRequest(utilities, requestId);
                umr.ShowDialog();
                btnFind.PerformClick();
                SettheSelectedRecord(requestId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
                log.LogMethodExit("Exception ex");
                return;
            }
            log.LogMethodExit();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                log.LogVariableState("dgvMaintenanceRequests.CurrentRow",dgvMaintenanceRequests.CurrentRow);
                if (dgvMaintenanceRequests.CurrentRow != null)
                {
                    int requestId = (int)dgvMaintenanceRequests.CurrentRow.Cells["maintChklstdetIdDataGridViewTextBoxColumn"].Value;
                    UpdateMaintenanceRequest umr = new UpdateMaintenanceRequest(utilities, requestId);
                    umr.ShowDialog();
                    btnFind.PerformClick();
                    SettheSelectedRecord(requestId);
                } 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
                log.LogMethodExit("Exception ex");
                return;
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            cmbAsset.SelectedValue = -1;
            cmbPriority.SelectedValue = -1;
            cmbRequestType.SelectedValue = -1;
            cmbStatus.Text = "open";
            txtRequestFrom.Text = "";
            txtRequestTitle.Text = "";
            txtRequestTo.Text = "";
            txtScheduleFrom.Text = "";
            txtScheduleTo.Text = "";
            btnFind.PerformClick();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvMaintenanceRequests.CurrentRow == null)
            {
                log.LogMethodExit("dgvMaintenanceRequests.CurrentRow == null");
                return;
            }
            if (dgvMaintenanceRequests.SelectedRows[0].Cells["maintChklstdetIdDataGridViewTextBoxColumn"] != null 
                 &&
                (dgvMaintenanceRequests.SelectedRows[0].Cells["maintChklstdetIdDataGridViewTextBoxColumn"].Value == null
                 || dgvMaintenanceRequests.SelectedRows[0].Cells["maintChklstdetIdDataGridViewTextBoxColumn"].Value.ToString() == "-1")
                )
            {
                log.LogMethodExit("dgvMaintenanceRequests.SelectedRows[0].Cells['maintChklstdetIdDataGridViewTextBoxColumn'].Value == null");
                return;
            }
            if (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Delete", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                log.LogMethodExit("Confirm Delete - No");
                return;
            }
            try
            {
                UserJobItemsDTO userJobItemsDTO = (UserJobItemsDTO)dgvMaintenanceRequests.CurrentRow.DataBoundItem;
                userJobItemsDTO.IsActive = false;
                UserJobItemsBL userJobItemsBL = new UserJobItemsBL(machineUserContext, userJobItemsDTO);
                userJobItemsBL.Save();
                btnFind.PerformClick(); 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
                log.LogMethodExit("Exception ex");
                return;
            }
            log.LogMethodExit();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string reportName = "User Job Item Requests";
            utilities.ExportToExcel(dgvMaintenanceRequests, reportName + " " + System.DateTime.Now.ToString("dd-MMM-yyyy"), reportName, utilities.ParafaitEnv.SiteName, dtpRequestFrom.Value, dtpRequestTo.Value);
            log.LogMethodExit();
        }

        private void dtpScheduleFrom_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtScheduleFrom.Text = dtpScheduleFrom.Value.ToString(utilities.getDateFormat());
            log.LogMethodExit();
        }

        private void dtpScheduleTo_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtScheduleTo.Text = dtpScheduleTo.Value.ToString(utilities.getDateFormat());
            log.LogMethodExit();
        }

        private void dtpRequestFrom_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtRequestFrom.Text = dtpRequestFrom.Value.ToString(utilities.getDateFormat());
            log.LogMethodExit();
        }

        private void dtpRequestTo_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtRequestTo.Text = dtpRequestTo.Value.ToString(utilities.getDateFormat());
            log.LogMethodExit();
        }
    }
}
