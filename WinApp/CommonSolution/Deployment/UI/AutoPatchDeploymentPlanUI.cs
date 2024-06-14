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
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// Deployment Plan UI
    /// </summary>
    public partial class AutoPatchDeploymentPlanUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AutoPatchApplTypeList autoPatchApplTypeList;
        List<AutoPatchApplTypeDTO> autoPatchApplTypeListOnDisplay;
        BindingSource deploymentPlanListBS;
        BindingSource deploymentPlanApplicationListBS;
        ExecutionContext autoPatchApplType = ExecutionContext.GetExecutionContext();
        int depPlanId = -1;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="_Utilities"> Parafait utilities object </param>
        public AutoPatchDeploymentPlanUI(Utilities _Utilities)
        {
            log.Debug("Starts-DeploymentPlanUI(Utilities) parameterized constructor.");
            var upgradeType = new Dictionary<string, string>();           
            InitializeComponent();
            utilities = _Utilities;
            utilities.setLanguage(this);
            if (utilities.ParafaitEnv.LoginID.Equals("semnox")||utilities.ParafaitEnv.Role == "System Administrator")
            {
                isActiveDataGridViewTextBoxColumn.ReadOnly = false;
                isActiveDataGridViewTextBoxColumn1.ReadOnly = false;
            }           
            deploymentPlannedDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            utilities.setupDataGridProperties(ref deploymentPlanDataGridView);
            utilities.setupDataGridProperties(ref deploymentPlanApplicationDataGridView);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                autoPatchApplType.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                autoPatchApplType.SetSiteId(-1);
            } 
            autoPatchApplType.SetUserId(utilities.ParafaitEnv.LoginID);

            upgradeType["A"] = utilities.MessageUtils.getMessage("Auto");
            upgradeType["F"] = utilities.MessageUtils.getMessage("Forced");
            upgradeTypeDataGridViewTextBoxColumn.DataSource = new BindingSource(upgradeType, null);
            upgradeTypeDataGridViewTextBoxColumn.DisplayMember = "Value";
            upgradeTypeDataGridViewTextBoxColumn.ValueMember = "Key";

            autoPatchApplTypeList = new AutoPatchApplTypeList();
            List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>> autoPatchApplTypeDTOSearchParams = new List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>>();
            autoPatchApplTypeDTOSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.SITE_ID, autoPatchApplType.GetSiteId().ToString()));
            autoPatchApplTypeListOnDisplay = autoPatchApplTypeList.GetAllAutoPatchApplTypes(autoPatchApplTypeDTOSearchParams);
            if (autoPatchApplTypeListOnDisplay == null)
            {
                autoPatchApplTypeListOnDisplay = new List<AutoPatchApplTypeDTO>();
            }
            autoPatchApplTypeListOnDisplay.Insert(0, new AutoPatchApplTypeDTO());
            BindingSource patchApplicationTypeBS = new BindingSource();
            autoPatchApplTypeListOnDisplay[0].ApplicationType = utilities.MessageUtils.getMessage("<SELECT>");
            patchApplicationTypeBS.DataSource = autoPatchApplTypeListOnDisplay;
            patchApplicationTypeIdDataGridViewTextBoxColumn.DataSource = patchApplicationTypeBS;
            patchApplicationTypeIdDataGridViewTextBoxColumn.ValueMember = "PatchApplicationTypeId";
            patchApplicationTypeIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
            patchApplicationTypeIdDataGridViewTextBoxColumn.DisplayMember = "ApplicationType";
            PopulateDeploymentPlanGrid();
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                btnApplyPatchToSite.Visible = true;
            }
            if ((utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite) || utilities.getParafaitDefaults("WEB_UPLOAD_SERVER_ENABLED").Equals("N"))//Starts:Modification on 27-10-2016 for local and HQ dependent sites upgarding
            {                
                deploymentPlanSaveBtn.Visible = true;
                deploymentPlanRefreshBtn.Visible = true;
                deploymentPlanDeleteBtn.Visible = true;
                deploymentPlanCloseBtn.Visible = true;
                deploymentPlanDataGridView.ReadOnly = false;
                deploymentPlanApplicationDataGridView.ReadOnly = false;
            }
            else
            {
                btnApplyPatchToSite.Visible = false;
                deploymentPlanSaveBtn.Visible = false;
                deploymentPlanRefreshBtn.Visible = false;
                deploymentPlanDeleteBtn.Visible = false;
                deploymentPlanCloseBtn.Visible = false;
                deploymentPlanDataGridView.ReadOnly = true;
                deploymentPlanApplicationDataGridView.ReadOnly = true;
            }//Ends:Modification on 27-10-2016 for local and HQ dependent sites upgarding
            
            log.Debug("Ends-DeploymentPlanUI(Utilities) parameterized constructor.");
        }
        /// <summary>
        /// Loads Deployment Plans to the grid
        /// </summary>
        private void PopulateDeploymentPlanGrid()
        {
            log.Debug("Starts-PopulateDeploymentPlanGrid() method.");
            AutoPatchDepPlanList deploymentPlanList = new AutoPatchDepPlanList();
            List<KeyValuePair<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string>> depPlanSearchParams = new List<KeyValuePair<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string>>();
            depPlanSearchParams.Add(new KeyValuePair<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string>(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.SITE_ID, autoPatchApplType.GetSiteId().ToString()));
            List<AutoPatchDepPlanDTO> deploymentPlanListOnDisplay = deploymentPlanList.GetAllAutoPatchDepPlans(depPlanSearchParams);
            deploymentPlanListBS = new BindingSource();
            if (deploymentPlanListOnDisplay != null)
            {
                deploymentPlanListBS.DataSource = new SortableBindingList<AutoPatchDepPlanDTO>(deploymentPlanListOnDisplay);
                PopulateDeploymentPlanApplicationGrid(deploymentPlanListOnDisplay[0].PatchDeploymentPlanId);
            }
            else
            {
                deploymentPlanListBS.DataSource = new SortableBindingList<AutoPatchDepPlanDTO>();
                PopulateDeploymentPlanApplicationGrid(-1);
            }
            deploymentPlanListBS.AddingNew += deploymentPlanDataGridView_BindingSourceAddNew;
            deploymentPlanDataGridView.DataSource = deploymentPlanListBS;

            log.Debug("Ends-PopulateDeploymentPlanGrid() method.");
        }
        /// <summary>
        /// Loads Deployment Plans Application to the grid
        /// </summary>
        private void PopulateDeploymentPlanApplicationGrid(int planId)
        {
            log.Debug("Starts-PopulateDeploymentPlanGrid() method.");
            List<AutoPatchDepPlanApplicationDTO> deploymentPlanApplicationListOnDisplay=null;
            deploymentPlanApplicationDataGridView.DataSource = null;
            if (planId != -1)
            {
                AutoPatchDepPlanApplicationList deploymentPlanApplicationList = new AutoPatchDepPlanApplicationList();
                List<KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>> depPlanApplicationsSearchParams = new List<KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>>();
                depPlanApplicationsSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.SITE_ID, autoPatchApplType.GetSiteId().ToString()));
                depPlanApplicationsSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_ID, planId.ToString()));
                deploymentPlanApplicationListOnDisplay = deploymentPlanApplicationList.GetAllAutoPatchDepPlanApplications(depPlanApplicationsSearchParams);
            }            
            deploymentPlanApplicationListBS = new BindingSource();
            if (deploymentPlanApplicationListOnDisplay != null)
            {
                deploymentPlanApplicationListBS.DataSource = new SortableBindingList<AutoPatchDepPlanApplicationDTO>(deploymentPlanApplicationListOnDisplay);
            }
            else
                deploymentPlanApplicationListBS.DataSource = new SortableBindingList<AutoPatchDepPlanApplicationDTO>();
            deploymentPlanApplicationListBS.AddingNew += deploymentPlanApplicationDataGridView_BindingSourceAddNew;
            deploymentPlanApplicationDataGridView.DataSource = deploymentPlanApplicationListBS;
            deploymentPlanApplicationDataGridView.DataError += new DataGridViewDataErrorEventHandler(deploymentPlanApplicationDataGridView_ComboDataError);
            log.Debug("Ends-PopulateDeploymentPlanGrid() method.");
        }
        private void deploymentPlanApplicationDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-deploymentPlanDataGridView_BindingSourceAddNew() Event.");
            if (deploymentPlanApplicationDataGridView.Rows.Count == deploymentPlanApplicationListBS.Count)
            {
                deploymentPlanApplicationListBS.RemoveAt(deploymentPlanApplicationListBS.Count - 1);
            }
            log.Debug("Ends-deploymentPlanDataGridView_BindingSourceAddNew() Event.");
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
        private void deploymentPlanApplicationDataGridView_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-deploymentPlanApplicationDataGridView_ComboDataError() Event.");
            if (e.ColumnIndex == deploymentPlanApplicationDataGridView.Columns["patchApplicationTypeIdDataGridViewTextBoxColumn"].Index)
            {
                if (autoPatchApplTypeListOnDisplay != null)
                    deploymentPlanApplicationDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = autoPatchApplTypeListOnDisplay[0].PatchApplicationTypeId;
            }
            log.Debug("Ends-deploymentPlanApplicationDataGridView_ComboDataError() Event.");
        }

        private void deploymentPlanDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-deploymentPlanDataGridView_DataError() Event.");
            MessageBox.Show("Error in deployment plan grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + deploymentPlanDataGridView.Columns[e.ColumnIndex].DataPropertyName +
                ": " + ((e.Exception!=null)?e.Exception.Message : "Error"));
            e.Cancel = true;
            log.Debug("Ends-deploymentPlanDataGridView_DataError() Event.");
        }

        private void deploymentPlanSaveBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-deploymentPlanSaveBtn_Click() Event.");
            BindingSource deploymentPlanListBS = (BindingSource)deploymentPlanDataGridView.DataSource;
            var deploymentPlanListOnDisplay = (SortableBindingList<AutoPatchDepPlanDTO>)deploymentPlanListBS.DataSource;
            if (deploymentPlanListOnDisplay.Count > 0)
            {
                foreach (AutoPatchDepPlanDTO deploymentPlanDTO in deploymentPlanListOnDisplay)
                {
                    if (deploymentPlanDTO.IsChanged)
                    {
                        if (string.IsNullOrEmpty(deploymentPlanDTO.DeploymentPlanName))
                        {
                            //MessageBox.Show("Please enter the plan name.");
                            MessageBox.Show(utilities.MessageUtils.getMessage(1421));
                            return;
                        }
                        if (deploymentPlanDTO.DeploymentPlannedDate.Equals(DateTime.MinValue))
                        {
                            //MessageBox.Show("Please enter the plan date.");
                            MessageBox.Show(utilities.MessageUtils.getMessage(1422));
                            return;
                        }
                        if (string.IsNullOrEmpty(deploymentPlanDTO.PatchFileName))
                        {
                            //MessageBox.Show("Please enter the plan name.");
                            MessageBox.Show(utilities.MessageUtils.getMessage(1423));
                            return;
                        }
                        if (!utilities.ParafaitEnv.IsCorporate && utilities.getParafaitDefaults("WEB_UPLOAD_SERVER_ENABLED").Equals("N"))//Starts:Modification on 27-10-2016 for local and HQ dependent sites upgarding
                        {
                            deploymentPlanDTO.IsReady=true;
                        }//Ends:Modification on 27-10-2016 for local and HQ dependent sites upgarding
                    }
                    AutoPatchDeploymentPlan deploymentPlan = new AutoPatchDeploymentPlan(deploymentPlanDTO);
                    deploymentPlan.Save();
                    if (deploymentPlanDTO.IsChanged)
                    {
                        SaveDeploymentApplication(deploymentPlanDTO.PatchDeploymentPlanId);
                    }
                }
                SaveDeploymentApplication(-1);
                PopulateDeploymentPlanGrid();
            }
            else
                MessageBox.Show("Nothing to save");
            log.Debug("Ends-deploymentPlanSaveBtn_Click() Event.");
        }
        private void SaveDeploymentApplication(int planId)
        {
            log.Debug("Starts-SaveDeploymentApplication() method.");
            BindingSource deploymentPlanListBS = (BindingSource)deploymentPlanApplicationDataGridView.DataSource;
            var deploymentPlanApplcationListOnDisplay = (SortableBindingList<AutoPatchDepPlanApplicationDTO>)deploymentPlanApplicationListBS.DataSource;
            if (deploymentPlanApplcationListOnDisplay.Count > 0)
            {
                foreach (AutoPatchDepPlanApplicationDTO deploymentPlanApllicationDTO in deploymentPlanApplcationListOnDisplay)
                {
                    if (deploymentPlanApllicationDTO.IsChanged)
                    {
                        if (planId!=-1)
                        deploymentPlanApllicationDTO.PatchDeploymentPlanId = planId;
                        if (deploymentPlanApllicationDTO.PatchApplicationTypeId == -1)
                        {
                            MessageBox.Show("Please select the application type.");
                            return;
                        }
                        if (string.IsNullOrEmpty(deploymentPlanApllicationDTO.DeploymentVersion))
                        {
                            MessageBox.Show("Please enter the deployment version.");
                            return;
                        }
                        if (string.IsNullOrEmpty(deploymentPlanApllicationDTO.MinimumVersionRequired))
                        {
                            MessageBox.Show("Please enter the minimum required version.");
                            return;
                        }
                        if (deploymentPlanApllicationDTO.PatchApplicationTypeId == -1)
                        {
                            MessageBox.Show("Please select the application type.");
                            return;
                        }
                    }
                    AutoPatchDepPlanApplication deploymentPlanApplication = new AutoPatchDepPlanApplication(deploymentPlanApllicationDTO);
                    deploymentPlanApplication.Save();
                }
                PopulateDeploymentPlanApplicationGrid(-1);
            }
            log.Debug("Ends-SaveDeploymentApplication() method.");

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
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-deploymentPlanDeleteBtn_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.deploymentPlanDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.deploymentPlanDataGridView.SelectedCells)
                {
                    deploymentPlanDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }

            foreach (DataGridViewRow deploymentPlanRow in this.deploymentPlanDataGridView.SelectedRows)
            {
                if (deploymentPlanRow.Cells[0].Value == null)
                {
                    return;
                }
                if (Convert.ToInt32(deploymentPlanRow.Cells[0].Value.ToString()) <= 0)
                {
                    deploymentPlanDataGridView.Rows.RemoveAt(deploymentPlanRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource autoPatchDepPlanListDTOBS = (BindingSource)deploymentPlanDataGridView.DataSource;
                        var autoPatchDepPlanDTOList = (SortableBindingList<AutoPatchDepPlanDTO>)autoPatchDepPlanListDTOBS.DataSource;
                        AutoPatchDepPlanDTO autoPatchDepPlanDTO = autoPatchDepPlanDTOList[deploymentPlanRow.Index];
                        autoPatchDepPlanDTO.IsActive = "N";
                        AutoPatchDeploymentPlan autoPatchDeploymentPlan = new AutoPatchDeploymentPlan(autoPatchDepPlanDTO);
                        autoPatchDeploymentPlan.Save();
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            PopulateDeploymentPlanGrid();
            log.Debug("Ends-deploymentPlanDeleteBtn_Click() event.");
        }

        private void deploymentPlanCloseBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-deploymentPlanCloseBtn_Click() Event.");
            this.Close();
            log.Debug("Ends-deploymentPlanCloseBtn_Click() Event.");
        }

        private void btnApplyPatchToSite_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnApplyPatchToSite_Click() Event.");
            BindingSource deploymentPlanListBS = (BindingSource)deploymentPlanDataGridView.DataSource;
            SortableBindingList<AutoPatchDepPlanDTO> deploymentPlanDTOList = (SortableBindingList<AutoPatchDepPlanDTO>)deploymentPlanListBS.DataSource;
            AutoPatchDepPlanDTO autoPatchDepPlanDTO;
            
            if (deploymentPlanDTOList != null && deploymentPlanDTOList.Count > 0 && (deploymentPlanDataGridView.SelectedCells.Count > 0))
            {
                if (deploymentPlanDataGridView.SelectedCells[0].RowIndex != deploymentPlanDataGridView.Rows.Count - 1)
                {
                    autoPatchDepPlanDTO = deploymentPlanDTOList[deploymentPlanDataGridView.SelectedCells[0].RowIndex];
                }
                else
                    autoPatchDepPlanDTO = null;
            }
            else
            {
                autoPatchDepPlanDTO = null;
            }
            if (autoPatchDepPlanDTO != null)
            {
                log.Info("btnApplyPatchToSite_Click() Event Loading site map.");
                try
                {
                    log.Info("btnApplyPatchToSite_Click() event entered try block.");
                    DeploymentSiteMapUI deploymentSiteMapUI = new DeploymentSiteMapUI(utilities, autoPatchDepPlanDTO);
                    deploymentSiteMapUI.ShowDialog();
                }
                catch(Exception ex)
                {
                    log.Info("btnApplyPatchToSite_Click() event Exception:"+ex.ToString());                   
                }
                autoPatchDepPlanDTO.IsActive = "N";
                AutoPatchDeploymentPlan autoPatchDeploymentPlan = new AutoPatchDeploymentPlan(autoPatchDepPlanDTO);
                autoPatchDeploymentPlan.Save();
            }
            log.Debug("Ends-btnApplyPatchToSite_Click() Event.");            
        }

        private void deploymentPlanApplicationDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-deploymentPlanApplicationDataGridView_DataError() Event.");
            MessageBox.Show("Error in deployment application grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + deploymentPlanApplicationDataGridView.Columns[e.ColumnIndex].DataPropertyName +
                ": " + ((e.Exception != null) ? e.Exception.Message : "Error"));
            e.Cancel = true;
            log.Debug("Ends-deploymentPlanApplicationDataGridView_DataError() Event.");
        }

        private void deploymentPlanDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-deploymentPlanDataGridView_CellClick() Event.");
            if (e.RowIndex >= 0)
            {
                if (deploymentPlanDataGridView.Rows[e.RowIndex].Cells["patchDeploymentPlanIdDataGridViewTextBoxColumn"].Value == null)
                {
                    depPlanId = -1;
                }
                else
                {
                    depPlanId = (int)deploymentPlanDataGridView.Rows[e.RowIndex].Cells["patchDeploymentPlanIdDataGridViewTextBoxColumn"].Value;
                }
                PopulateDeploymentPlanApplicationGrid(depPlanId);
            }
            log.Debug("Ends-deploymentPlanDataGridView_CellClick() Event.");
        }

        private void deploymentPlanDataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-deploymentPlanDataGridView_RowLeave() Event.");
            
            BindingSource deploymentPlanListBS = (BindingSource)deploymentPlanApplicationDataGridView.DataSource;
            var deploymentPlanApplcationListOnDisplay = (SortableBindingList<AutoPatchDepPlanApplicationDTO>)deploymentPlanApplicationListBS.DataSource;
            if (deploymentPlanApplcationListOnDisplay.Count > 0)
            {
                foreach (AutoPatchDepPlanApplicationDTO deploymentPlanApllicationDTO in deploymentPlanApplcationListOnDisplay)
                {
                    if (deploymentPlanApllicationDTO.IsChanged)
                    {
                        if (MessageBox.Show("Do you want to save entered record?.", "Save Conformation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            deploymentPlanSaveBtn.PerformClick();
                        }
                        else
                        {
                            PopulateDeploymentPlanApplicationGrid(-1);
                        }
                        break;
                    }
                }
            }            
            log.Debug("Ends-deploymentPlanDataGridView_RowLeave() Event.");
        }

        private void deploymentPlanApplicationDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.Debug("Starts-deploymentPlanApplicationDataGridView_DefaultValuesNeeded() Event.");
            e.Row.Cells["patchDeploymentPlanIdApplDataGridViewTextBoxColumn"].Value = depPlanId;
            log.Debug("Ends-deploymentPlanApplicationDataGridView_DefaultValuesNeeded() Event.");
        }

        private void AutoPatchDeploymentPlanUI_Load(object sender, EventArgs e)
        {
            btnApplyPatchToSite.Size = new System.Drawing.Size(126, 23);
        }     
        private void deploymentPlanDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            deploymentPlanDataGridView_CellClick(sender, e);
        }        
       
    }
}
