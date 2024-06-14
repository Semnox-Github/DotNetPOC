/********************************************************************************************
 * Project Name - Deployment Plan UI
 * Description  - User interface for deployment plan
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016   Raghuveera          Created 
 ********************************************************************************************/


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

namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// Deployment Plan UI
    /// </summary>
    public partial class DeploymentPlanUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PatchApplicationTypeList patchApplicationTypeList;
        List<PatchApplicationTypeDTO> patchApplicationTypeListOnDisplay;
        BindingSource deploymentPlanListBS;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="_Utilities"> Parafait utilities object </param>
        public DeploymentPlanUI(Utilities _Utilities)
        {
            log.Debug("Starts-DeploymentPlanUI(Utilities) parameterized constructor.");
            var weekDays = new Dictionary<string, string>();
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref deploymentPlanDataGridView);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            machineUserContext.SetUserId(utilities.ParafaitEnv.Username);

            weekDays["A"] = utilities.MessageUtils.getMessage("Auto");
            weekDays["F"] = utilities.MessageUtils.getMessage("Forced");
            upgradeTypeDataGridViewTextBoxColumn.DataSource = new BindingSource(weekDays, null);
            upgradeTypeDataGridViewTextBoxColumn.DisplayMember = "Value";
            upgradeTypeDataGridViewTextBoxColumn.ValueMember = "Key";

            patchApplicationTypeList = new PatchApplicationTypeList();
            patchApplicationTypeListOnDisplay = patchApplicationTypeList.GetAllPatchApplicationTypes(null);
            if (patchApplicationTypeListOnDisplay == null)
            {
                patchApplicationTypeListOnDisplay = new List<PatchApplicationTypeDTO>();
            }
            patchApplicationTypeListOnDisplay.Insert(0, new PatchApplicationTypeDTO());
            BindingSource patchApplicationTypeBS = new BindingSource();
            patchApplicationTypeListOnDisplay[0].ApplicationType = utilities.MessageUtils.getMessage("<SELECT>");
            patchApplicationTypeBS.DataSource = patchApplicationTypeListOnDisplay;
            patchApplicationTypeIdDataGridViewTextBoxColumn.DataSource = patchApplicationTypeBS;
            patchApplicationTypeIdDataGridViewTextBoxColumn.ValueMember = "PatchApplicationTypeId";
            patchApplicationTypeIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
            patchApplicationTypeIdDataGridViewTextBoxColumn.DisplayMember = "ApplicationType";
            PopulateDeploymentPlanGrid();
            log.Debug("Ends-DeploymentPlanUI(Utilities) parameterized constructor.");
        }
        /// <summary>
        /// Loads Deployment Plans to the grid
        /// </summary>
        private void PopulateDeploymentPlanGrid()
        {
            log.Debug("Starts-PopulateDeploymentPlanGrid() method.");
            PatchApplicationDeploymentPlanList deploymentPlanList = new PatchApplicationDeploymentPlanList();

            List<PatchApplicationDeploymentPlanDTO> deploymentPlanListOnDisplay = deploymentPlanList.GetAllPatchApplicationDeploymentPlans(null);
            deploymentPlanListBS = new BindingSource();
            if (deploymentPlanListOnDisplay != null)
                deploymentPlanListBS.DataSource = deploymentPlanListOnDisplay;
            else
                deploymentPlanListBS.DataSource = new List<PatchApplicationDeploymentPlanDTO>();
            deploymentPlanListBS.AddingNew += deploymentPlanDataGridView_BindingSourceAddNew;
            deploymentPlanDataGridView.DataSource = deploymentPlanListBS;
            deploymentPlanDataGridView.DataError += new DataGridViewDataErrorEventHandler(deploymentPlanDataGridView_ComboDataError);
            log.Debug("Ends-PopulateDeploymentPlanGrid() method.");
        }
        private void deploymentPlanDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-deploymentPlanDataGridView_BindingSourceAddNew() Event.");
            if (deploymentPlanDataGridView.Rows.Count == deploymentPlanListBS.Count)
            {
                deploymentPlanListBS.RemoveAt(deploymentPlanListBS.Count - 1);
            }
            log.Debug("Ends-deploymentPlanDataGridView_BindingSourceAddNew() Event.");
        }
        private void deploymentPlanDataGridView_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-deploymentPlanDataGridView_ComboDataError() Event.");
            if (e.ColumnIndex == deploymentPlanDataGridView.Columns["patchApplicationTypeIdDataGridViewTextBoxColumn"].Index)
            {
                if (patchApplicationTypeListOnDisplay != null)
                    deploymentPlanDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = patchApplicationTypeListOnDisplay[0].PatchApplicationTypeId;
            }
            log.Debug("Ends-deploymentPlanDataGridView_ComboDataError() Event.");
        }

        private void deploymentPlanDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-deploymentPlanDataGridView_DataError() Event.");
            MessageBox.Show("Error in deployment plan grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + deploymentPlanDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.Debug("Ends-deploymentPlanDataGridView_DataError() Event.");
        }

        private void deploymentPlanSaveBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-deploymentPlanSaveBtn_Click() Event.");
            BindingSource deploymentPlanListBS = (BindingSource)deploymentPlanDataGridView.DataSource;
            var deploymentPlanListOnDisplay = (List<PatchApplicationDeploymentPlanDTO>)deploymentPlanListBS.DataSource;
            if (deploymentPlanListOnDisplay.Count > 0)
            {
                foreach (PatchApplicationDeploymentPlanDTO deploymentPlanDTO in deploymentPlanListOnDisplay)
                {
                    if (deploymentPlanDTO.IsChanged)
                    {
                        if (string.IsNullOrEmpty(deploymentPlanDTO.DeploymentPlanName))
                        {
                            MessageBox.Show("Please enter the plan name.");
                            return;
                        }
                        if (deploymentPlanDTO.DeploymentPlannedDate.Equals(DateTime.MinValue))
                        {
                            MessageBox.Show("Please enter the plan date.");
                            return;
                        }
                        if (string.IsNullOrEmpty(deploymentPlanDTO.DeploymentVersion))
                        {
                            MessageBox.Show("Please enter the deployment version.");
                            return;
                        }
                        if (deploymentPlanDTO.PatchApplicationTypeId == -1)
                        {
                            MessageBox.Show("Please select the application type.");
                            return;
                        }
                        if (string.IsNullOrEmpty(deploymentPlanDTO.DeploymentStatus))
                        {
                            MessageBox.Show("Please enter the deployment status.");
                            return;
                        }
                    }
                    PatchApplicationDeploymentPlan deploymentPlan = new PatchApplicationDeploymentPlan(deploymentPlanDTO);
                    deploymentPlan.Save();
                }
                PopulateDeploymentPlanGrid();
            }
            else
                MessageBox.Show("Nothing to save");
            log.Debug("Ends-deploymentPlanSaveBtn_Click() Event.");
        }

        private void deploymentPlanRefreshBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-deploymentPlanRefreshBtn_Click() Event.");
            PopulateDeploymentPlanGrid();
            log.Debug("Ends-deploymentPlanRefreshBtn_Click() Event.");
        }

        private void deploymentPlanDeleteBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-deploymentPlanDeleteBtn_Click() event.");
            if (this.deploymentPlanDataGridView.SelectedRows.Count <= 0 && this.deploymentPlanDataGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show("No rows selected. Please select the rows you want to delete and press delete..");
                log.Debug("Ends-deploymentPlanDeleteBtn_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message");
                return;
            }
            bool rowsDeleted = false;
            if (this.deploymentPlanDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.deploymentPlanDataGridView.SelectedCells)
                {
                    deploymentPlanDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }

            foreach (DataGridViewRow deploymentPlanRow in this.deploymentPlanDataGridView.SelectedRows)
            {
                if (Convert.ToInt32(deploymentPlanRow.Cells[0].Value.ToString()) <= 0)
                {
                    deploymentPlanDataGridView.Rows.RemoveAt(deploymentPlanRow.Index);
                    rowsDeleted = true;
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show("Rows deleted succesfully");
            else
                MessageBox.Show("Deleting the existing record is not allowed.Please make it inactive..");
            log.Debug("Ends-deploymentPlanDeleteBtn_Click() event.");
        }

        private void deploymentPlanCloseBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-deploymentPlanCloseBtn_Click() Event.");
            this.Close();
            log.Debug("Ends-deploymentPlanCloseBtn_Click() Event.");
        }
    }
}
