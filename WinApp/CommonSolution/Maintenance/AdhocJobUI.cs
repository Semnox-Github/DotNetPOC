/********************************************************************************************
 * Project Name - Adhoc Job UI
 * Description  - User interface for adhoc jobs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************************************
 *1.00        22-Jan-2016   Raghuveera          Created 
 *1.00        22-Feb-2016   Suneetha.S          Modified 
 *2.70        10-Jul-2019   Dakshakh raj        Modified : Modified format for date search in search by schedule date 
 *2.70        12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Adhoc job UI
    /// </summary>
    public partial class AdhocJobUI : Form
    {
        AssetList assetList;
        List<GenericAssetDTO> genericAssetListOnDisplay;
        JobTaskList jobTaskList;
        List<JobTaskDTO> jobTaskDTOListOnDisplay;
        List<UsersDTO> usersDTOList;
        BindingSource maintenanceJobBS;
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool loadSearch = true;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public AdhocJobUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            RegisterKeyDownHandlers(this);
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
            setupGrid();
            LoadAssignedTo();
            LoadAsset();
            jobTaskList = new JobTaskList(machineUserContext);
            jobTaskDTOListOnDisplay = jobTaskList.GetAllJobTasks(null);
            if (jobTaskDTOListOnDisplay != null)
            {
                jobTaskDTOListOnDisplay.Insert(0, new JobTaskDTO());
                jobTaskId.DataSource = jobTaskDTOListOnDisplay;
                jobTaskId.ValueMember = "JobTaskId";
                jobTaskId.ValueType = typeof(Int32);
                jobTaskId.DisplayMember = "TaskName";
            }
            PopulateAdhocGrid();
            log.LogMethodExit();
        }
        private void adhocRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateAdhocGrid();
            log.LogMethodExit();
        }
        private void adhocCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }
        private void adhocDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + adhocDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }
        private void adhocDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (adhocDataGridView.Rows.Count == maintenanceJobBS.Count)
            {
                maintenanceJobBS.RemoveAt(maintenanceJobBS.Count - 1);
            }
            log.LogMethodExit();
        }
        private void adhocDataGridView_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == adhocDataGridView.Columns["jobTaskId"].Index)
            {
                if (jobTaskDTOListOnDisplay != null)
                    adhocDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = jobTaskDTOListOnDisplay[0].JobTaskId;
            }
            else if (e.ColumnIndex == adhocDataGridView.Columns["assetIdDataGridViewTextBoxColumn"].Index)
            {
                if (genericAssetListOnDisplay != null)
                    adhocDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = genericAssetListOnDisplay[0].AssetId;
            }
            log.LogMethodExit();
        }
        private void adhocSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (validate())
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.DESCRIPTION, "open"));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                    if (lookupValuesDTOList == null || lookupValuesDTOList.Count == 0)
                        throw new Exception("\"open\" status not found in lookups 'MAINT_JOB_STATUS'.");

                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupJobTypeSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupJobTypeSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"));
                    lookupJobTypeSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Job"));
                    lookupJobTypeSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    List<LookupValuesDTO> lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupJobTypeSearchParams);

                    BindingSource addhocListBS = (BindingSource)adhocDataGridView.DataSource;
                    var adhocListOnDisplay = (SortableBindingList<UserJobItemsDTO>)addhocListBS.DataSource;
                    if (adhocListOnDisplay.Count > 0)
                    {
                        foreach (UserJobItemsDTO maintenanceJobDTO in adhocListOnDisplay)
                        {
                            maintenanceJobDTO.ChklstScheduleTime = utilities.getServerTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            if (string.IsNullOrEmpty(maintenanceJobDTO.TaskName))
                            {
                                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2208));//"Task name is not entered."
                                return;
                            }
                            if (maintenanceJobDTO.ValidateTag)
                            {
                                if (string.IsNullOrEmpty(maintenanceJobDTO.TaskCardNumber))
                                {
                                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2209)); //Please enter card number
                                    log.LogMethodExit(MessageContainerList.GetMessage(machineUserContext, 2209));
                                    return;
                                }
                                else
                                {
                                    maintenanceJobDTO.TaskCardNumber = maintenanceJobDTO.TaskCardNumber.ToUpper();
                                }
                            }
                            else
                            {
                                maintenanceJobDTO.TaskCardNumber = "";
                            }

                            if (lookupOpenValuesDTOList != null)
                            {
                                maintenanceJobDTO.MaintJobType = lookupOpenValuesDTOList[0].LookupValueId;
                            }
                            else
                            {
                                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2210));//Lookup MAINT_JOB_TYPE is not found
                                log.LogMethodExit(MessageContainerList.GetMessage(machineUserContext, 2210));
                                return;
                            }
                            maintenanceJobDTO.MaintJobName = maintenanceJobDTO.TaskName;
                            maintenanceJobDTO.AssetName = cmbAssetName.Text;
                            maintenanceJobDTO.AssetId = Convert.ToInt32(cmbAssetName.SelectedValue);
                            maintenanceJobDTO.Status = lookupValuesDTOList[0].LookupValueId;
                            maintenanceJobDTO.DurationToComplete = Convert.ToInt32(string.IsNullOrEmpty(txtCompleted.Text) ? "-1" : txtCompleted.Text);
                            maintenanceJobDTO.AssignedTo = cmbAssignedTo.Text;
                            maintenanceJobDTO.AssignedUserId = Convert.ToInt32(cmbAssignedTo.SelectedValue);
                            maintenanceJobDTO.LastUpdatedDate = "";
                            maintenanceJobDTO.LastUpdatedBy = null;

                            UserJobItemsBL adhoc = new UserJobItemsBL(machineUserContext, maintenanceJobDTO);
                            adhoc.Save();

                        }
                        adhocDataGridView.DataSource = null;
                        adhocDataGridView.DataSource = addhocListBS;
                        //MessageBox.Show("Save successful");
                        PopulateAdhocGrid();
                    }
                    else
                        MessageBox.Show(utilities.MessageUtils.getMessage(371));
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, "Error saving data") + e1.Message, MessageContainerList.GetMessage(machineUserContext, "Adhoc Job Save Error"));
                log.Error(e1);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// setup grid
        /// </summary>
        private void setupGrid()
        {
            log.LogMethodEntry();
            for (int i = 0; i < adhocDataGridView.Columns.Count; i++)
            {
                adhocDataGridView.Columns[i].Visible = false;
            }

            //adhocDataGridView.Columns["assetIdDataGridViewTextBoxColumn"].Visible =
            adhocDataGridView.Columns["remarksMandatoryDataGridViewTextBoxColumn"].Visible =
            adhocDataGridView.Columns["validateTagDataGridViewTextBoxColumn"].Visible =
            adhocDataGridView.Columns["taskNameDataGridViewTextBoxColumn"].Visible =
            adhocDataGridView.Columns["taskCardNumberDataGridViewTextBoxColumn"].Visible = true;
            //adhocDataGridView.Columns["assetIdDataGridViewTextBoxColumn"].DisplayIndex = 0;
            adhocDataGridView.Columns["taskNameDataGridViewTextBoxColumn"].DisplayIndex = 1;
            adhocDataGridView.Columns["validateTagDataGridViewTextBoxColumn"].DisplayIndex = 2;
            adhocDataGridView.Columns["taskCardNumberDataGridViewTextBoxColumn"].DisplayIndex = 3;
            adhocDataGridView.Columns["remarksMandatoryDataGridViewTextBoxColumn"].DisplayIndex = 4;
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads data source to assigned to combobox
        /// </summary>
        private void LoadAssignedTo()
        {
            log.LogMethodEntry();
            try
            { 
                UsersList usersList = new UsersList(machineUserContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> usersSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                usersDTOList = usersList.GetAllUsers(usersSearchParams);
                if (usersDTOList != null)
                {
                    usersDTOList.Insert(0, new UsersDTO());
                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = usersDTOList;
                    cmbAssignedTo.DataSource = usersDTOList;
                    cmbAssignedTo.DisplayMember = "UserName";
                    cmbAssignedTo.ValueMember = "UserId";

                    dgvAssignedTo.DataSource = bindingSource;
                    for (int i = 0; i < dgvAssignedTo.Columns.Count; i++)
                    {
                        if (!dgvAssignedTo.Columns[i].Name.Equals("UserName"))
                        {
                            dgvAssignedTo.Columns[i].Visible = false;
                        }
                        else
                        {
                            dgvAssignedTo.Columns[i].Width = dgvAssignedTo.Width;
                        }
                    }
                } 
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Function loads the records to the grid.
        /// </summary>
        private void PopulateAdhocGrid()
        {
            log.LogMethodEntry();
            try
            { 
                txtCompleted.ResetText();
                txtName.ResetText();
                cmbAssignedTo.ResetText();
                maintenanceJobBS = new BindingSource();
                maintenanceJobBS.DataSource = new SortableBindingList<UserJobItemsDTO>();
                adhocDataGridView.DataSource = maintenanceJobBS;
                maintenanceJobBS.AddingNew += adhocDataGridView_BindingSourceAddNew;
                adhocDataGridView.DataSource = maintenanceJobBS;
                utilities.setupDataGridProperties(ref adhocDataGridView);
                adhocDataGridView.DataError += new DataGridViewDataErrorEventHandler(adhocDataGridView_ComboDataError); 
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validates the input and returns the status
        /// </summary>
        /// <returns>Boolean value</returns>
        private bool validate()
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 961));//Please enter asset name.
                log.Debug(MessageContainerList.GetMessage(machineUserContext, 961));
                txtName.Focus();
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                cmbAssetName.Text = txtName.Text;
                if (!cmbAssetName.Text.Equals(txtName.Text))
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2211));//Please select existing asset name
                    log.Debug(MessageContainerList.GetMessage(machineUserContext, 2211));
                    txtName.Focus();
                    log.LogMethodExit(false);
                    return false;
                }
                if ((int)cmbAssetName.SelectedValue == -1)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2212));// "Please select asset name.");//
                    log.Debug(MessageContainerList.GetMessage(machineUserContext, 2212));
                    txtName.Focus();
                    log.LogMethodExit(false);
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(txtCompleted.Text))
            {
                try
                {
                    int x = int.Parse(txtCompleted.Text);
                }
                catch
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2213));// "Please enter numeric value for duration in days.");
                    log.Debug("Duration in days entered is not numeric");
                    txtCompleted.Focus();
                    log.LogMethodExit(false);
                    return false;
                }
            }
            if (cmbAssignedTo.SelectedValue.ToString().Equals("-1"))
            {
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2214));//Please select the user to whom you are going to assign this task
                log.Debug("Assigned to is not selected");
                cmbAssignedTo.Focus();
                log.LogMethodExit(false);
                return false;
            }
            if (adhocDataGridView.Rows.Count == 1)
            {
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2215));// "Please enter task.");
                log.Debug("Task is not entered.");
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit();
            return true;
        }

        private void adhocDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.adhocDataGridView.CurrentCell != null)
            {
                if (this.adhocDataGridView.SelectedRows.Count <= 0 && this.adhocDataGridView.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("No rows selected. Please select the rows you want to delete and press delete");
                    log.LogMethodExit();
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.adhocDataGridView.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.adhocDataGridView.SelectedCells)
                    {
                        adhocDataGridView.Rows[cell.RowIndex].Selected = true;
                    }
                }

                foreach (DataGridViewRow maintenanceJobRow in this.adhocDataGridView.SelectedRows)
                {
                    if (Convert.ToInt32(maintenanceJobRow.Cells[0].Value.ToString()) <= 0)
                    {
                        adhocDataGridView.Rows.RemoveAt(maintenanceJobRow.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            BindingSource maintenanceJobDTOListDTOBS = (BindingSource)adhocDataGridView.DataSource;
                            var maintenanceJobDTOList = (SortableBindingList<UserJobItemsDTO>)maintenanceJobDTOListDTOBS.DataSource;
                            UserJobItemsDTO maintenanceJobDTO = maintenanceJobDTOList[maintenanceJobRow.Index];
                            maintenanceJobDTO.IsActive = false;
                            UserJobItemsBL maintenanceJob = new UserJobItemsBL(machineUserContext, maintenanceJobDTO);
                            maintenanceJob.Save();
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads asset to the Comboboxes
        /// </summary>
        private void LoadAsset()
        {
            log.LogMethodEntry();
            assetList = new AssetList(machineUserContext);
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> searchByAssetParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
            searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            genericAssetListOnDisplay = assetList.GetAllAssets(searchByAssetParameters);
            if (genericAssetListOnDisplay != null)
            {
                BindingSource binding = new BindingSource();
                BindingSource searchBinding = new BindingSource();
                binding.DataSource = genericAssetListOnDisplay;
                searchBinding.DataSource = genericAssetListOnDisplay;
                genericAssetListOnDisplay.Insert(0, new GenericAssetDTO());
                assetIdDataGridViewTextBoxColumn.DataSource = genericAssetListOnDisplay;
                assetIdDataGridViewTextBoxColumn.ValueMember = "AssetId";
                assetIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
                assetIdDataGridViewTextBoxColumn.DisplayMember = "Name";
                cmbAssetName.DataSource = binding;
                cmbAssetName.ValueMember = "AssetId";
                cmbAssetName.DisplayMember = "Name";
                dgvAssetNameSearch.DataSource = searchBinding;
                for (int i = 0; i < dgvAssetNameSearch.Columns.Count; i++)
                {
                    if (!dgvAssetNameSearch.Columns[i].Name.Equals("Name"))
                    {
                        dgvAssetNameSearch.Columns[i].Visible = false;
                    }
                    else
                    {
                        dgvAssetNameSearch.Columns[i].Width = dgvAssetNameSearch.Width;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvAssetNameSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtName.Text = dgvAssetNameSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbAssetName.Text = txtName.Text;
                dgvAssetNameSearch.Visible = false;
            }
            catch (Exception ex){ log.Error(ex); }
            log.LogMethodExit();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (loadSearch)
            {
                if (txtName.Text.Length > 0)
                {
                    if (genericAssetListOnDisplay != null)
                    {
                        List<GenericAssetDTO> assetDTOList = genericAssetListOnDisplay.Where(x => (bool)((string.IsNullOrEmpty(x.Name) ? "" : x.Name.ToLower()).Contains(txtName.Text.ToLower()))).ToList<GenericAssetDTO>();
                        if (assetDTOList.Count > 0)
                        {
                            dgvAssetNameSearch.Visible = true;
                            dgvAssetNameSearch.DataSource = assetDTOList;
                        }
                        else
                        {
                            dgvAssetNameSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    cmbAssetName.Text = "";
                    dgvAssetNameSearch.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }

        private void cmbAssetName_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (cmbAssetName.Text.Equals("Semnox.Parafait.Maintenance.GenericAssetDTO"))
            {
                txtName.Text = "";
            }
            else
            {
                txtName.Text = cmbAssetName.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }

        private void txtAssignedTo_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (loadSearch)
            {
                if (txtAssignedTo.Text.Length > 0)
                {
                    if (usersDTOList != null)
                    {
                        List<UsersDTO> usersList = usersDTOList.Where(x => (bool)((string.IsNullOrEmpty(x.UserName) ? "" : x.UserName.ToLower()).Contains(txtAssignedTo.Text.ToLower()))).ToList<UsersDTO>();
                        if (usersList.Count > 0)
                        {
                            dgvAssignedTo.Visible = true;
                            dgvAssignedTo.DataSource = usersList;
                        }
                        else
                        {
                            dgvAssignedTo.Visible = false;
                        }
                    }
                }
                else
                {
                    cmbAssignedTo.Text = "";
                    dgvAssignedTo.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }

        private void dgvAssignedTo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtAssignedTo.Text = dgvAssignedTo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbAssignedTo.Text = txtAssignedTo.Text;
                dgvAssignedTo.Visible = false;
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void cmbAssignedTo_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (cmbAssignedTo.Text.Equals("Semnox.Parafait.User.UsersDTO"))
            {
                txtAssignedTo.Text = "";
            }
            else
            {
                txtAssignedTo.Text = cmbAssignedTo.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }

        private void cmbAssetName_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAssignedTo.Visible = dgvAssetNameSearch.Visible = false;
            log.LogMethodExit();
        }

        private void cmbAssignedTo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAssignedTo.Visible = dgvAssetNameSearch.Visible = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// To fix the issue hiding the List on click of any controls in the form
        /// </summary>
        /// <param name="control"></param>
        private void RegisterKeyDownHandlers(Control control)
        {
            log.LogMethodEntry(control);
            foreach (Control ctl in control.Controls)
            {
                ctl.Click += MyKeyPressEventHandler;
                RegisterKeyDownHandlers(ctl);
            }
            log.LogMethodExit();
        }

        public void MyKeyPressEventHandler(Object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAssetNameSearch.Visible = dgvAssignedTo.Visible = false;
            loadSearch = false;
            log.LogMethodExit();
        }

        private void GenericAssetUI_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAssetNameSearch.Visible = dgvAssignedTo.Visible = false;
            loadSearch = false;
            log.LogMethodExit();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Down)
            {
                if (dgvAssetNameSearch.Rows.Count > 0)
                {
                    dgvAssetNameSearch.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvAssetNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtName.Text = dgvAssetNameSearch.SelectedCells[0].Value.ToString();
                    cmbAssetName.Text = txtName.Text;
                    dgvAssetNameSearch.Visible = false;
                    txtName.Focus();
                }
                catch (Exception ex) { log.Error(ex); }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtName.Focus();
            }
            log.LogMethodExit();
        }

        private void txtAssignedTo_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Down)
            {
                if (dgvAssignedTo.Rows.Count > 0)
                {
                    dgvAssignedTo.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvAssignedTo_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtAssignedTo.Text = dgvAssignedTo.SelectedCells[0].Value.ToString();
                    cmbAssignedTo.Text = txtAssignedTo.Text;
                    dgvAssignedTo.Visible = false;
                    txtAssignedTo.Focus();
                }
                catch (Exception ex) { log.Error(ex); }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtAssignedTo.Focus();
            }
            log.LogMethodExit();
        } 

    }
}
