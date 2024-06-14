/********************************************************************************************
 * Project Name - MaintenanceChangeAssignUserUI
 * Description  -UI for Maintenance Change Assign User
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.70.0      12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2  
 *2.70        10-Jul-2019   Dakshakh raj   Modified : Modified format for date search in search by schedule date
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Maintenance
{
    public partial class MaintenanceChangeAssignUserUI : Form
    {
        //int AssetId;
        int jobTypeId;
        AssetList assetList;
        List<GenericAssetDTO> genericAssetListOnDisplay;
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BindingSource maintenanceJobBS;
        List<LookupValuesDTO> jobTypeDTOList;
        List<LookupValuesDTO> statusDTOList;
        List<UsersDTO> usersDTOList;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public MaintenanceChangeAssignUserUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
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
            LoadSiteCombo();
            LoadAssignedTo();
            foreach(DataGridViewColumn dgc in dgvMaintenanceRequests.Columns)
            {
                dgc.ReadOnly = true;
            }
            assignedToDataGridViewTextBoxColumn.Visible = false;
            assignedUserIdDataGridViewTextBoxColumn.ReadOnly = false;
            assignedUserIdDataGridViewTextBoxColumn.Visible = true;
            statusDataGridViewTextBoxColumn.DisplayIndex = statusDataGridViewTextBoxColumn.DisplayIndex - 2;
            assignedUserIdDataGridViewTextBoxColumn.DisplayIndex = statusDataGridViewTextBoxColumn.DisplayIndex + 1;

            
            utilities.setupDataGridProperties(ref dgvMaintenanceRequests);
            log.LogMethodExit();
        }

        private void MaintenanceChangeAssignUserUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnFind.Left = dtpScheduleTo.Right - btnFind.Width;
            log.LogMethodExit();
        }
        /// <summary>
        /// Fetching the sites from site table
        /// </summary>
        private void LoadSiteCombo()
        {
            log.LogMethodEntry();
            try
            {
                SiteList siteList = new SiteList(machineUserContext);
                List<SiteDTO> siteDTOList = siteList.GetAllSites(null);
                if (siteDTOList == null)
                {
                    siteDTOList = new List<SiteDTO>();
                }
                if (siteDTOList.Count > 0)
                {
                    siteDTOList.RemoveAt(0);
                }
                siteDTOList.Insert(0, new SiteDTO());
                siteDTOList[0].SiteName = "<SELECT>";
                
                cmbSite.DataSource = siteDTOList;
                cmbSite.DisplayMember = "SiteName";
                cmbSite.ValueMember = "SiteId";

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// To get the Job type id
        /// </summary>
        private int GetJobTypeID(int siteId)
        {
            log.LogMethodEntry(siteId);
            try
            { 
                LookupValuesList lookupValuesList=new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupOpenValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, siteId.ToString()));
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"));
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Service Request"));
                List<LookupValuesDTO> lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupOpenValuesSearchParams);
                if (lookupOpenValuesDTOList == null || lookupOpenValuesDTOList.Count == 0)
                {
                    log.Debug("lookupOpenValuesDTOList == null || lookupOpenValuesDTOList.Count == 0");
                    log.LogMethodExit(-1);
                    return -1;
                } 
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
            if (cmbSite.SelectedValue == null)
            {
                log.LogMethodExit("cmbSite.SelectedValue == null");
                return;
            }
            else if (Convert.ToInt32(cmbSite.SelectedValue) == -1)
            {
                maintenanceJobBS.DataSource = new List<UserJobItemsDTO>();
            }
            else
            {
                jobTypeId = GetJobTypeID(Convert.ToInt32(cmbSite.SelectedValue));
                List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> maintenanceJobSearchParams = new List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>>();
                if (jobTypeId != -1)
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_TYPE, (jobTypeId == -1) ? "" : jobTypeId.ToString()));
                maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_NAME, txtRequestTitle.Text));
                maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE, (chbShowActiveEntries.Checked) ? "Y" : "N"));
                //maintenanceJobSearchParams.Add(new KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>(MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSIGNED_TO, utilities.ParafaitEnv.User_Id.ToString()));
                if (cmbAsset.SelectedValue != null && !cmbAsset.SelectedValue.ToString().Equals("-1"))
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.ASSET_ID, (cmbAsset.SelectedValue == null || cmbAsset.SelectedValue.ToString().Equals("-1")) ? "" : cmbAsset.SelectedValue.ToString()));
                if (cmbRequestType.SelectedValue != null && !cmbRequestType.SelectedValue.ToString().Equals("-1"))
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TYPE_ID, (cmbRequestType.SelectedValue.ToString().Equals("-1")) ? "" : cmbRequestType.SelectedValue.ToString()));
                if (cmbStatus.SelectedValue != null && !cmbStatus.SelectedValue.ToString().Equals("-1"))
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.STATUS, ((cmbStatus.SelectedValue == null || cmbStatus.SelectedValue.ToString().Equals("-1"))) ? "" : cmbStatus.SelectedValue.ToString()));
                if (cmbPriority.SelectedValue != null && !cmbPriority.SelectedValue.ToString().Equals("-1"))
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.PRIORITY, ((cmbPriority.SelectedValue == null || cmbPriority.SelectedValue.ToString().Equals("-1"))) ? "" : cmbPriority.SelectedValue.ToString()));
                if (cmbAssignedTo.SelectedValue != null && !cmbAssignedTo.SelectedValue.ToString().Equals("-1"))
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.ASSIGNED_TO, ((cmbAssignedTo.SelectedValue == null || cmbAssignedTo.SelectedValue.ToString().Equals("-1"))) ? "" : cmbAssignedTo.SelectedValue.ToString()));
                try
                {
                    if (!string.IsNullOrEmpty(txtScheduleFrom.Text))
                    {
                        maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_FROM_DATE, DateTime.Parse(txtScheduleFrom.Text).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(970, new object[1] { utilities.MessageUtils.getMessage("Schedule from") }));
                    log.Debug("Schedule From Date is not in correct format");
                    log.LogMethodExit();
                    return;
                }
                try
                {
                    if (!string.IsNullOrEmpty(txtScheduleTo.Text))
                    {
                        if (DateTime.Parse(txtScheduleFrom.Text).CompareTo(DateTime.Parse(txtScheduleTo.Text)) <= 0)
                            maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_TO_DATE, DateTime.Parse(txtScheduleTo.Text).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        else
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(971));
                            log.Debug("Schedule To Date should be greater than from date");
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(970, new object[1] { utilities.MessageUtils.getMessage("Schedule To") }));
                    log.Debug("Schedule To date is not in correct format");
                    log.LogMethodExit();
                    return;
                }
                try
                {
                    if (!string.IsNullOrEmpty(txtRequestFrom.Text))
                    {
                        maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_FROM_DATE, DateTime.Parse(txtRequestFrom.Text).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(970, new object[1] { utilities.MessageUtils.getMessage("Request from") }));
                    log.Debug("Request From Date is not in correct format");
                    log.LogMethodExit();
                    return;
                }
                try
                {
                    if (!string.IsNullOrEmpty(txtRequestTo.Text))
                    {
                        if (DateTime.Parse(txtRequestFrom.Text).CompareTo(DateTime.Parse(txtRequestTo.Text)) <= 0)
                        {
                            maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TO_DATE, DateTime.Parse(txtRequestTo.Text).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(971));
                            log.Debug("To Date should be greater than from date");
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(970, new object[1] { utilities.MessageUtils.getMessage("Request to") }));
                    log.Debug("To date is not in correct format");
                    log.LogMethodExit();
                    return;
                }
                maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID, Convert.ToInt32(cmbSite.SelectedValue).ToString()));
                MaintenanceChangeAssignJobList maintenanceChangeAssignJobList = new MaintenanceChangeAssignJobList(machineUserContext);
                maintenanceJobBS = new BindingSource();
                List<UserJobItemsDTO> maintenanceJobDTOList = maintenanceChangeAssignJobList.GetAllMaintenanceWithHQPublishedJobList(maintenanceJobSearchParams);

                if (maintenanceJobDTOList != null)
                {
                    for (int i = 0; i < maintenanceJobDTOList.Count; i++)
                    {
                        try
                        {
                            maintenanceJobDTOList[i].RequestDate = string.IsNullOrEmpty(maintenanceJobDTOList[i].RequestDate) ? "" : DateTime.Parse(maintenanceJobDTOList[i].RequestDate).ToString(utilities.getDateTimeFormat());
                        }
                        catch (Exception ex){ log.Error(ex); }
                        try
                        {
                            maintenanceJobDTOList[i].ChecklistCloseDate = string.IsNullOrEmpty(maintenanceJobDTOList[i].ChecklistCloseDate) ? "" : DateTime.Parse(maintenanceJobDTOList[i].ChecklistCloseDate).ToString(utilities.getDateTimeFormat());
                        }
                        catch (Exception ex) { log.Error(ex); }
                    }
                    SortableBindingList<UserJobItemsDTO> maintenanceJobDTOSortList = new SortableBindingList<UserJobItemsDTO>(maintenanceJobDTOList);
                    maintenanceJobBS.DataSource = maintenanceJobDTOSortList;
                }
                else
                    maintenanceJobBS.DataSource = new SortableBindingList<UserJobItemsDTO>();
            }
            dgvMaintenanceRequests.DataError += new DataGridViewDataErrorEventHandler(dgvMaintenanceRequests_ComboDataError);
            dgvMaintenanceRequests.DataSource = maintenanceJobBS;
            DataGridViewCellStyle cellstyle=new DataGridViewCellStyle();
            cellstyle.Format=utilities.getDateFormat();
            requestDateDataGridViewTextBoxColumn.DefaultCellStyle =
          checklistCloseDateDataGridViewTextBoxColumn.DefaultCellStyle =cellstyle ;// = utilities.gridViewDateCellStyle();
            
            dgvMaintenanceRequests.BorderStyle = BorderStyle.FixedSingle;
            utilities.setLanguage(dgvMaintenanceRequests);
            log.LogMethodExit();

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

        private void dtpScheduleTo_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtScheduleTo.Text = dtpScheduleTo.Value.ToString(utilities.getDateFormat());
            log.LogMethodExit();
        }

        private void dtpScheduleFrom_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtScheduleFrom.Text = dtpScheduleFrom.Value.ToString(utilities.getDateFormat());
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

        private void cmbSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (cmbSite.SelectedValue != null && !cmbSite.SelectedValue.ToString().Equals("Semnox.Parafait.Site.SiteDTO"))
            {
                if (Convert.ToInt32(cmbSite.SelectedValue)>=0)
                {
                    dgvMaintenanceRequests.DataSource = new SortableBindingList<UserJobItemsDTO>();            
                    GetAssets(Convert.ToInt32(cmbSite.SelectedValue));
                    GetRequestTypes(Convert.ToInt32(cmbSite.SelectedValue));
                    GetRequestStatus(Convert.ToInt32(cmbSite.SelectedValue));
                    GetRequestPriority(Convert.ToInt32(cmbSite.SelectedValue));
                    LoadAssignedTo(Convert.ToInt32(cmbSite.SelectedValue));
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads data source to assigned to combobox
        /// </summary>
        private void LoadAssignedTo(int siteId = -1)
        {
            log.LogMethodEntry(siteId);
            try
            { 
                UsersList usersList = new UsersList(machineUserContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> usersSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, (siteId == -1) ? machineUserContext.GetSiteId().ToString() : siteId.ToString()));
                usersDTOList = usersList.GetAllUsers(usersSearchParams);
                if (usersDTOList == null)
                {
                    usersDTOList = new List<UsersDTO>();                    
                }
                usersDTOList.Insert(0, new UsersDTO());
                if (siteId == -1)
                {
                    assignedUserIdDataGridViewTextBoxColumn.DataSource = usersDTOList;
                    assignedUserIdDataGridViewTextBoxColumn.DisplayMember = "LoginId";
                    assignedUserIdDataGridViewTextBoxColumn.ValueMember = "UserId";
                }
                else
                {
                    cmbAssignedTo.DataSource = usersDTOList;
                    cmbAssignedTo.DisplayMember = "LoginId";
                    cmbAssignedTo.ValueMember = "UserId";
                } 
            }
            catch (Exception e)
            {
                log.Error("Ends-loadAssignedTo() Method with an Exception:", e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads asset to the Comboboxes
        /// </summary>
        private void GetAssets(int siteId)
        {
            log.LogMethodEntry(siteId);
            assetList = new AssetList(machineUserContext);
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
            assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, "Y"));
            assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, siteId.ToString()));
            genericAssetListOnDisplay = assetList.GetAllAssets(assetSearchParams);
            if (genericAssetListOnDisplay == null)
            {
                genericAssetListOnDisplay = new List<GenericAssetDTO>();
            }
            BindingSource binding = new BindingSource();
            binding.DataSource = genericAssetListOnDisplay;
            genericAssetListOnDisplay.Insert(0, new GenericAssetDTO());

            cmbAsset.DataSource = binding;
            cmbAsset.ValueMember = "AssetId";
            cmbAsset.DisplayMember = "Name";
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the status to the comboboxes
        /// </summary>       
        private void GetRequestTypes(int siteId)
        {
            log.LogMethodEntry(siteId);
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_REQUEST_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, siteId.ToString()));
                jobTypeDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (jobTypeDTOList == null)
                {
                    jobTypeDTOList = new List<LookupValuesDTO>();
                }
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
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the status
        /// </summary>
        private void GetRequestStatus(int siteId)
        {
            log.LogMethodEntry(siteId);
            try
            {
                cmbStatus.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbStatus.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, siteId.ToString()));
                statusDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (statusDTOList == null)
                {
                    statusDTOList = new List<LookupValuesDTO>();
                }
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
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the priority combobox
        /// </summary>
        private void GetRequestPriority(int siteId)
        {
            log.LogMethodEntry(siteId);
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_PRIORITY"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, siteId.ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                bindingSource.DataSource = lookupValuesDTOList;
                cmbPriority.DataSource = bindingSource;
                cmbPriority.ValueMember = "LookupValueId";
                cmbPriority.DisplayMember = "LookupValue"; 
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (Convert.ToInt32(cmbSite.SelectedValue) >= 0)
            {
                UsersList usersList;
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByUserParameters;
                List<UsersDTO> siteUsersDTOList;
                BindingSource maintenanceRequestListBS = (BindingSource)dgvMaintenanceRequests.DataSource;
                var maintenanceRequestListOnDisplay = (SortableBindingList<UserJobItemsDTO>)maintenanceRequestListBS.DataSource;
                if (maintenanceRequestListOnDisplay.Count > 0)
                {
                    foreach (UserJobItemsDTO maintenanceJobDTO in maintenanceRequestListOnDisplay)
                    {
                        if (maintenanceJobDTO.AssignedUserId == -1)
                        {
                            continue;
                        }
                        else
                        {
                            usersList = new UsersList(machineUserContext);
                            searchByUserParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                            searchByUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, Convert.ToInt32(cmbSite.SelectedValue).ToString()));
                            searchByUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.MASTER_ENTITY_ID, maintenanceJobDTO.AssignedUserId.ToString()));
                            siteUsersDTOList = usersList.GetAllUsers(searchByUserParameters);
                            if (siteUsersDTOList != null)
                            {
                                maintenanceJobDTO.AssignedUserId = siteUsersDTOList[0].UserId;
                                maintenanceJobDTO.AssignedTo = siteUsersDTOList[0].LoginId;
                            }
                            else
                            {
                                log.Error("publish user entity first.");
                                 MessageBox.Show("Publish user entity first.");
                                log.LogMethodExit();
                                 return;
                            }
                        }
                        MaintenanceChangeAssignUser maintenanceJob = new MaintenanceChangeAssignUser(machineUserContext,maintenanceJobDTO);
                        maintenanceJob.Save(Convert.ToInt32(cmbSite.SelectedValue));
                    }
                    //maintenanceTaskGroupDataGridView.DataSource = null;
                    //maintenanceTaskGroupDataGridView.DataSource = maintenanceTaskGroupListBS;               
                    btnFind.PerformClick();
                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            else
            {
                MessageBox.Show("Please select the site.");
            }
            log.LogMethodExit();
        }
    }
}
