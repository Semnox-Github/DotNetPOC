/********************************************************************************************
 * Project Name - Maintenance Task UI
 * Description  - User interface for maintenance task
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jan-2016   Raghuveera     Created 
 *********************************************************************************************
 *1.00        22-Feb-2016   Suneetha.S     Modified 
 *2.70        12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2 
 *2.70.2      12-Aug-2019   Deeksha        Modified logger methods.
 *2.70.3      02-Apr-2020   Girish Kundar  Modified: Do not allow duplicate  name
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Maintenance Task UI
    /// </summary>
    public partial class MaintenanceTaskUI : Form
    {
        JobTaskGroupList jobTaskGroupList;
        List<JobTaskGroupDTO> jobTaskGroupDTOListOnDisplay;
        Utilities utilities;
        BindingSource maintenanceTaskListBS;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool loadSearch = true;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public MaintenanceTaskUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            RegisterKeyDownHandlers(this);
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref maintenanceTaskDataGridView);
            utilities.setLanguage(this);
            
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                btnPublishToSite.Visible = true;
            }
            else
            {
                btnPublishToSite.Visible = false;
            } 
            jobTaskGroupList = new JobTaskGroupList(machineUserContext);
            List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> maintenanceTaskGroupSearchParams = new List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>>();
            maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            jobTaskGroupDTOListOnDisplay = jobTaskGroupList.GetAllJobTaskGroups(maintenanceTaskGroupSearchParams);
            if (jobTaskGroupDTOListOnDisplay != null)
            {
                jobTaskGroupDTOListOnDisplay.Insert(0, new JobTaskGroupDTO());
                BindingSource bindingSource = new BindingSource();
                BindingSource searchBinding = new BindingSource();
                bindingSource.DataSource = jobTaskGroupDTOListOnDisplay;
                searchBinding.DataSource = jobTaskGroupDTOListOnDisplay;
                cmbGroupName.DataSource = bindingSource;
                cmbGroupName.ValueMember = "JobTaskGroupId";
                cmbGroupName.DisplayMember = "TaskGroupName";
                jobTaskGroupDTOListOnDisplay[0].TaskGroupName = utilities.MessageUtils.getMessage("None");
                maintTaskGroupIdDataGridViewTextBoxColumn.DataSource = jobTaskGroupDTOListOnDisplay;
                maintTaskGroupIdDataGridViewTextBoxColumn.ValueMember = "JobTaskGroupId";
                maintTaskGroupIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
                maintTaskGroupIdDataGridViewTextBoxColumn.DisplayMember = "TaskGroupName";
                dgvGroupSearch.DataSource = searchBinding;
                for (int i = 0; i < dgvGroupSearch.Columns.Count; i++)
                {
                    if (!dgvGroupSearch.Columns[i].Name.Equals("TaskGroupName"))
                    {
                        dgvGroupSearch.Columns[i].Visible = false;
                    }
                    else
                    {
                        dgvGroupSearch.Columns[i].Width = dgvGroupSearch.Width;
                    }
                }
            }
            PopulateMaintenanceTaskGrid();
            log.LogMethodExit();
        }
        private void maintenanceTaskDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (maintenanceTaskDataGridView.Rows.Count == maintenanceTaskListBS.Count)
            {
                maintenanceTaskListBS.RemoveAt(maintenanceTaskListBS.Count - 1);
            }
            log.LogMethodExit();
        }
        private void maintenanceTaskDataGridView_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == maintenanceTaskDataGridView.Columns["maintTaskGroupIdDataGridViewTextBoxColumn"].Index)
            {
                if (jobTaskGroupDTOListOnDisplay != null)
                    maintenanceTaskDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = jobTaskGroupDTOListOnDisplay[0].JobTaskGroupId;
            }
            log.LogMethodExit();
        }
        private void maintenanceTaskGroupCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }
        private void maintenanceTaskGroupDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.maintenanceTaskDataGridView.SelectedRows.Count <= 0 && this.maintenanceTaskDataGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.LogMethodExit();
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.maintenanceTaskDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.maintenanceTaskDataGridView.SelectedCells)
                {
                    maintenanceTaskDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow maintenanceTaskRow in this.maintenanceTaskDataGridView.SelectedRows)
            {
                if (maintenanceTaskRow.Cells[0].Value == null)
                {
                    log.LogMethodExit();
                    return;
                }
                if (Convert.ToInt32(maintenanceTaskRow.Cells[0].Value.ToString()) <= 0)
                {
                    maintenanceTaskDataGridView.Rows.RemoveAt(maintenanceTaskRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource maintenanceTaskDTOListDTOBS = (BindingSource)maintenanceTaskDataGridView.DataSource;
                        var maintenanceTaskDTOList = (SortableBindingList<JobTaskDTO>)maintenanceTaskDTOListDTOBS.DataSource;
                        JobTaskDTO maintenanceTaskDTO = maintenanceTaskDTOList[maintenanceTaskRow.Index];
                        maintenanceTaskDTO.IsActive = false;
                        JobTaskBL maintenanceTask = new JobTaskBL(machineUserContext, maintenanceTaskDTO);
                        maintenanceTask.Save();
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            btnSearch.PerformClick();
            log.LogMethodExit();
        }
        private void maintenanceTaskSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                BindingSource maintenanceTaskListBS = (BindingSource)maintenanceTaskDataGridView.DataSource;
                var maintenanceTaskListOnDisplay = (SortableBindingList<JobTaskDTO>)maintenanceTaskListBS.DataSource;
                if (maintenanceTaskListOnDisplay.Count > 0)
                {
                    List<JobTaskDTO> tempList = new List<JobTaskDTO>(maintenanceTaskListOnDisplay);
                    if (tempList != null && tempList.Count > 0)
                    {
                        var query = tempList.GroupBy(x => new { x.TaskName, x.JobTaskGroupId })
                       .Where(g => g.Count() > 1)
                       .Select(y => y.Key)
                       .ToList();
                        if (query.Count > 0)
                        {
                            log.Debug("Duplicate entries detail : " + query[0]);
                            MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2608, " task / task group"), "Validation Error");
                            return;
                        }
                    }
                    foreach (JobTaskDTO maintenanceTaskDTO in maintenanceTaskListOnDisplay)
                    {
                        if (maintenanceTaskDTO.IsChanged)
                        {
                            if (string.IsNullOrEmpty(maintenanceTaskDTO.TaskName.Trim()))
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(975), "Validation Error");
                                log.LogMethodExit();
                                return;
                            }
                            if (maintenanceTaskDTO.JobTaskGroupId == -1)
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(976));
                                log.LogMethodExit();
                                return;
                            }
                            if (maintenanceTaskDTO.ValidateTag)
                            {
                                if (string.IsNullOrEmpty(maintenanceTaskDTO.CardNumber))
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(977));
                                    log.LogMethodExit();
                                    return;
                                }
                                else
                                {
                                    maintenanceTaskDTO.CardNumber = maintenanceTaskDTO.CardNumber.ToUpper();
                                }
                            }
                            else
                            {
                                maintenanceTaskDTO.CardNumber = "";
                            }
                            JobTaskBL maintenanceTask = new JobTaskBL(machineUserContext, maintenanceTaskDTO);
                            maintenanceTask.Save();
                        }
                    }
                    //maintenanceTaskDataGridView.DataSource = null;
                    //maintenanceTaskDataGridView.DataSource = maintenanceTaskListBS;
                    btnSearch.PerformClick();
                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void maintenanceTaskDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",  " + utilities.MessageUtils.getMessage("Column") + " " + maintenanceTaskDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> maintenanceTaskSearchParams = new List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.IS_ACTIVE, "1"));
            }
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.TASK_NAME, txtName.Text));
            }
            if (!string.IsNullOrEmpty(cmbGroupName.Text) && !cmbGroupName.SelectedValue.ToString().Equals("-1"))
            {
                maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_GROUP_ID, (string.IsNullOrEmpty(cmbGroupName.Text)) ? "" : cmbGroupName.SelectedValue.ToString().Equals("-1") ? "" : cmbGroupName.SelectedValue.ToString()));
            }
            maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            JobTaskList maintenanceTaskList = new JobTaskList(machineUserContext);
            List<JobTaskDTO> maintenanceTaskDTOList = maintenanceTaskList.GetAllJobTasks(maintenanceTaskSearchParams);
            BindingSource maintenanceTaskDTOListBS = new BindingSource();
            if (maintenanceTaskDTOList != null)
            {
                SortableBindingList<JobTaskDTO> maintenanceTaskDTOSortList = new SortableBindingList<JobTaskDTO>(maintenanceTaskDTOList);
                maintenanceTaskDTOListBS.DataSource = maintenanceTaskDTOSortList;
            }
            else
                maintenanceTaskDTOListBS.DataSource = new SortableBindingList<JobTaskDTO>();
            maintenanceTaskDataGridView.DataSource = maintenanceTaskDTOListBS;
            log.LogMethodExit();
        }
        private void maintenanceTaskGroupRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cmbGroupName.SelectedValue = -1;
            txtName.Text = "";
            chbShowActiveEntries.Checked = true;
            PopulateMaintenanceTaskGrid();
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads Task to the grid
        /// </summary>
        private void PopulateMaintenanceTaskGrid()
        {
            log.LogMethodEntry();
            JobTaskList maintenanceTaskList = new JobTaskList(machineUserContext);
            List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> maintenanceTaskSearchParams = new List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.IS_ACTIVE, "1"));
            }
            maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<JobTaskDTO> maintenanceTaskListOnDisplay = maintenanceTaskList.GetAllJobTasks(maintenanceTaskSearchParams);
            maintenanceTaskListBS = new BindingSource();
            if (maintenanceTaskListOnDisplay != null)
            {
                SortableBindingList<JobTaskDTO> maintenanceTaskDTOSortList=new SortableBindingList<JobTaskDTO>(maintenanceTaskListOnDisplay);
                maintenanceTaskListBS.DataSource = maintenanceTaskDTOSortList;
            }
            else
                maintenanceTaskListBS.DataSource = new SortableBindingList<JobTaskDTO>();
            maintenanceTaskListBS.AddingNew += maintenanceTaskDataGridView_BindingSourceAddNew;
            maintenanceTaskDataGridView.DataSource = maintenanceTaskListBS;            
            maintenanceTaskDataGridView.DataError += new DataGridViewDataErrorEventHandler(maintenanceTaskDataGridView_ComboDataError);
            log.LogMethodExit();
        }      

        private void cmbGroupName_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cmbGroupName.Text.Equals("Semnox.Parafait.Maintenance.JobTaskGroupDTO"))
            {
                txtGroupSearch.Text = "";
            }
            else
            {
                txtGroupSearch.Text = cmbGroupName.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }

        private void txtGroupSearch_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (loadSearch)
            {
                if (txtGroupSearch.Text.Length > 0)
                {
                    if (jobTaskGroupDTOListOnDisplay != null)
                    {
                        List<JobTaskGroupDTO> maintenanceTaskGroupList = jobTaskGroupDTOListOnDisplay.Where(x => (bool)((string.IsNullOrEmpty(x.TaskGroupName) ? "" : x.TaskGroupName.ToLower()).Contains(txtGroupSearch.Text.ToLower()))).ToList<JobTaskGroupDTO>();
                        if (maintenanceTaskGroupList.Count > 0)
                        {
                            dgvGroupSearch.Visible = true;
                            dgvGroupSearch.DataSource = maintenanceTaskGroupList;
                        }
                        else
                        {
                            dgvGroupSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    txtGroupSearch.Text = "";
                    dgvGroupSearch.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }

        private void dgvGroupSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtGroupSearch.Text = dgvGroupSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbGroupName.Text = txtGroupSearch.Text;
                dgvGroupSearch.Visible = false;
            }
            catch { }
            log.LogMethodExit();
        }

        private void txtGroupSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Down)
            {
                if (dgvGroupSearch.Rows.Count > 0)
                {
                    dgvGroupSearch.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvGroupSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtGroupSearch.Text = dgvGroupSearch.SelectedCells[0].Value.ToString();
                    cmbGroupName.Text = txtGroupSearch.Text;
                    dgvGroupSearch.Visible = false;
                    txtGroupSearch.Focus();
                }
                catch { }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtGroupSearch.Focus();
            }
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
            log.LogMethodEntry();
            dgvGroupSearch.Visible  = false;
            loadSearch = false;
            log.LogMethodExit();
        }

        private void GenericAssetUI_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            dgvGroupSearch.Visible = false;
            loadSearch = false;
            log.LogMethodExit();
        }

        private void btnPublishToSite_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (maintenanceTaskDataGridView.SelectedRows != null && maintenanceTaskDataGridView.SelectedRows.Count > 0)
                {
                    int assetIdSelected = -1;
                    if (maintenanceTaskDataGridView.SelectedRows[0].Cells[0].Value != null)
                    {
                        int.TryParse(maintenanceTaskDataGridView.SelectedRows[0].Cells[0].Value.ToString(), out assetIdSelected);
                    }
                    if (assetIdSelected >= 0)
                    {
                        Publish.PublishUI publishUI = new Publish.PublishUI(utilities, assetIdSelected, "MaintenanceTask", maintenanceTaskDataGridView.SelectedRows[0].Cells[1].Value.ToString());
                        publishUI.ShowDialog();
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Maintenance Task Publish");
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void MaintenanceTaskUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnPublishToSite.Size = new Size(127, 23);
            log.LogMethodExit();
        }
    }
}
