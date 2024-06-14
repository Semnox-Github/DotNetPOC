/********************************************************************************************
 * Project Name - Maintenance Group UI
 * Description  - User interface for maintenance group
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jan-2016   Raghuveera     Created 
 *2.70        12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2
 *2.70.3      02-Apr-2020   Girish Kundar  Modified: Do not allow duplicate  name
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Maintenance Group UI
    /// </summary>
    public partial class MaintenanceGroupUI : Form
    {
        Utilities utilities;
        BindingSource maintenanceTaskGroupListBS;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public MaintenanceGroupUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref maintenanceTaskGroupDataGridView);
            utilities.setLanguage(this);
            
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                btnPublishToSite.Visible = true;
            }
            else
            {
                btnPublishToSite.Visible = false;
            }//Ends: Modification on 14-Jul-2016 for adding publish to site feature
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            PopulateMaintenanceTaskGroupGrid();
            log.LogMethodExit();
        }
        private void maintenanceTaskGroupDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (maintenanceTaskGroupDataGridView.Rows.Count == maintenanceTaskGroupListBS.Count)
            {
                maintenanceTaskGroupListBS.RemoveAt(maintenanceTaskGroupListBS.Count - 1);
            }
            log.LogMethodExit();
        }
        private void maintenanceTaskGroupDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.maintenanceTaskGroupDataGridView.SelectedRows.Count <= 0 && this.maintenanceTaskGroupDataGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("No rows selected. Please select the rows you want to delete and press delete");
                log.LogMethodExit();
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.maintenanceTaskGroupDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.maintenanceTaskGroupDataGridView.SelectedCells)
                {
                    maintenanceTaskGroupDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow maintenanceTaskRow in this.maintenanceTaskGroupDataGridView.SelectedRows)
            {
                if (maintenanceTaskRow.Cells[0].Value == null)
                {
                    log.LogMethodExit("maintenanceTaskRow.Cells[0].Value == null");
                    return;
                }
                if (Convert.ToInt32(maintenanceTaskRow.Cells[0].Value.ToString()) <= 0)
                {
                    maintenanceTaskGroupDataGridView.Rows.RemoveAt(maintenanceTaskRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource maintenanceGroupDTOListDTOBS = (BindingSource)maintenanceTaskGroupDataGridView.DataSource;
                        var maintenanceTaskGroupDTOList = (SortableBindingList<JobTaskGroupDTO>)maintenanceGroupDTOListDTOBS.DataSource;
                        JobTaskGroupDTO maintenanceGroupDTO = maintenanceTaskGroupDTOList[maintenanceTaskRow.Index];
                        maintenanceGroupDTO.IsActive = false;
                        JobTaskGroupBL maintenanceGroup = new JobTaskGroupBL(machineUserContext, maintenanceGroupDTO);
                        maintenanceGroup.Save();
                    }
                }
            }
            if (rowsDeleted == true)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            }
            btnSearch.PerformClick();
            log.LogMethodExit();
        }
        private void maintenanceTaskGroupCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }
        private void maintenanceTaskGroupRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            chbShowActiveEntries.Checked = true;
            txtName.Text = "";
            PopulateMaintenanceTaskGroupGrid();
            log.LogMethodExit();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> maintenanceTaskGroupSearchParams = new List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.IS_ACTIVE, "1"));
            }
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.TASK_GROUP_NAME, txtName.Text));
            }
            maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            JobTaskGroupList maintenanceTaskGroupList = new JobTaskGroupList(machineUserContext);

            List<JobTaskGroupDTO> maintenanceTaskGroupDTOList = maintenanceTaskGroupList.GetAllJobTaskGroups(maintenanceTaskGroupSearchParams);
            BindingSource maintenanceTaskGroupDTOListBS = new BindingSource();
            if (maintenanceTaskGroupDTOList != null)
            {
                SortableBindingList<JobTaskGroupDTO> maintenanceTaskGroupDTOSortList = new SortableBindingList<JobTaskGroupDTO>(maintenanceTaskGroupDTOList);
                maintenanceTaskGroupDTOListBS.DataSource = maintenanceTaskGroupDTOSortList;
            }
            else
                maintenanceTaskGroupDTOListBS.DataSource = new SortableBindingList<JobTaskDTO>();
            maintenanceTaskGroupDataGridView.DataSource = maintenanceTaskGroupDTOListBS;
            log.LogMethodExit();
        }
        private void maintenanceTaskGroupSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            BindingSource maintenanceTaskGroupListBS = (BindingSource)maintenanceTaskGroupDataGridView.DataSource;
            var maintenanceTaskGroupListOnDisplay = (SortableBindingList<JobTaskGroupDTO>)maintenanceTaskGroupListBS.DataSource;
            if (maintenanceTaskGroupListOnDisplay.Count > 0)
            {
                List<JobTaskGroupDTO> tempList = new List<JobTaskGroupDTO>(maintenanceTaskGroupListOnDisplay);
                var isNull = tempList.Any(item => item.TaskGroupName == null);
                if (isNull)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2607, "task group"), "Validation Error");
                    return;
                }
                List<string> nameList = tempList.Select(x => x.TaskGroupName.Trim().ToLower()).ToList();
                if (nameList.Count != nameList.Distinct().Count())
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2608, "task group"), "Validation Error");
                    return;
                }
                foreach (JobTaskGroupDTO maintenanceTaskGroupDTO in maintenanceTaskGroupListOnDisplay)
                {
                    if(string.IsNullOrEmpty(maintenanceTaskGroupDTO.TaskGroupName.Trim()))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(999), "Validation Error");
                        log.LogMethodExit("string.IsNullOrEmpty(maintenanceTaskGroupDTO.TaskGroupName)");
                        return;
                    }
                    JobTaskGroupBL maintenanceTaskGroup = new JobTaskGroupBL(machineUserContext, maintenanceTaskGroupDTO);
                    maintenanceTaskGroup.Save();
                }
                //maintenanceTaskGroupDataGridView.DataSource = null;
                //maintenanceTaskGroupDataGridView.DataSource = maintenanceTaskGroupListBS;               
                btnSearch.PerformClick();  
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }
        private void maintenanceTaskGroupDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ", " + utilities.MessageUtils.getMessage("Column") + " " + maintenanceTaskGroupDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads maintenance groups to the grid
        /// </summary>
        private void PopulateMaintenanceTaskGroupGrid()
        {
            log.LogMethodEntry();
            JobTaskGroupList maintenanceTaskGroupList = new JobTaskGroupList(machineUserContext);
            List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> maintenanceTaskGroupSearchParams = new List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.IS_ACTIVE, "1"));
            }
            maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<JobTaskGroupDTO> maintenanceTaskGroupListOnDisplay = maintenanceTaskGroupList.GetAllJobTaskGroups(maintenanceTaskGroupSearchParams);
            maintenanceTaskGroupListBS = new BindingSource();
            if (maintenanceTaskGroupListOnDisplay != null)
            {
                SortableBindingList<JobTaskGroupDTO> maintenanceTaskGroupDTOSortList = new SortableBindingList<JobTaskGroupDTO>(maintenanceTaskGroupListOnDisplay);
                maintenanceTaskGroupListBS.DataSource = maintenanceTaskGroupDTOSortList;
            }
            else
                maintenanceTaskGroupListBS.DataSource = new SortableBindingList<JobTaskGroupDTO>();
            maintenanceTaskGroupListBS.AddingNew += maintenanceTaskGroupDataGridView_BindingSourceAddNew;
            maintenanceTaskGroupDataGridView.DataSource = maintenanceTaskGroupListBS;
            log.LogMethodExit();
        }

        private void MaintenanceGroupUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnPublishToSite.Size = new System.Drawing.Size(126, 23);
            log.LogMethodExit();
        }

        private void btnPublishToSite_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (maintenanceTaskGroupDataGridView.SelectedRows != null && maintenanceTaskGroupDataGridView.SelectedRows.Count > 0)
                {
                    int taskIdSelected = -1;
                    if (maintenanceTaskGroupDataGridView.SelectedRows[0].Cells[0].Value != null)
                    {
                        int.TryParse(maintenanceTaskGroupDataGridView.SelectedRows[0].Cells[0].Value.ToString(), out taskIdSelected);
                    }
                    if (taskIdSelected >= 0)
                    {
                        Publish.PublishUI publishUI = new Publish.PublishUI(utilities, taskIdSelected, "MaintenanceTaskGroup", maintenanceTaskGroupDataGridView.SelectedRows[0].Cells[1].Value.ToString());
                        publishUI.ShowDialog();
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Maintenance Group Publish");
                log.Error(ex);
            }
            log.LogMethodExit();
        } 
    }
}
