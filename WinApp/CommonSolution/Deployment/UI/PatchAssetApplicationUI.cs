/********************************************************************************************
 * Project Name - Patch Asset Application UI
 * Description  - User interface for patch asset application
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016   Raghuveera          Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
//using Semnox.Parafait.MonitorAsset;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.logger;

 namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// Patch Asset Application UI
    /// </summary>
    public partial class PatchAssetApplicationUI : Form
    {
        Utilities utilities;
        BindingSource assetApplicationListBS;
        PatchApplicationTypeList patchApplicationTypeList;
        List<PatchApplicationTypeDTO> patchApplicationTypeListOnDisplay;
        MonitorAssetList monitorAssetList;
        List<MonitorAssetDTO> monitorAssetListOnDisplay;

       Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="_Utilities">Utilities object as a parameter </param>
        public PatchAssetApplicationUI(Utilities _Utilities)
        {
            log.Debug("Starts-PatchAssetApplicationUI(Utilities) parameterized constructor.");
            var weekDays = new Dictionary<string, string>();
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref assetApplicationDataGridView);
            ExecutionContext assetApplicationContext = ExecutionContext.GetExecutionContext();
            assetApplicationContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            assetApplicationContext.SetUserId(utilities.ParafaitEnv.Username);
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

            monitorAssetList = new MonitorAssetList(assetApplicationContext);
            monitorAssetListOnDisplay = monitorAssetList.GetAllMonitorAssets(null);
            if (monitorAssetListOnDisplay == null)
            {
                monitorAssetListOnDisplay = new List<MonitorAssetDTO>();
            }
            monitorAssetListOnDisplay.Insert(0, new MonitorAssetDTO());
            BindingSource monitorAssetBS = new BindingSource();
            monitorAssetListOnDisplay[0].Name = utilities.MessageUtils.getMessage("<SELECT>");
            monitorAssetBS.DataSource = monitorAssetListOnDisplay;
            assetIdDataGridViewTextBoxColumn.DataSource = monitorAssetBS;
            assetIdDataGridViewTextBoxColumn.ValueMember = "AssetId";
            assetIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
            assetIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            PopulateAssetApplicationGrid();
            log.Debug("Ends-PatchAssetApplicationUI(Utilities) parameterized constructor.");
        }
        /// <summary>
        /// Loads application type to the grid
        /// </summary>
        private void PopulateAssetApplicationGrid()
        {
            log.Debug("Starts-PopulateDeploymentPlanGrid() method.");
            PatchAssetApplicationList assetApplicationList = new PatchAssetApplicationList();

            List<PatchAssetApplicationDTO> assetApplicationListOnDisplay = assetApplicationList.GetAllPatchAssetApplications(null);
            assetApplicationListBS = new BindingSource();
            if (assetApplicationListOnDisplay != null)
                assetApplicationListBS.DataSource = assetApplicationListOnDisplay;
            else
                assetApplicationListBS.DataSource = new List<PatchAssetApplicationDTO>();
            assetApplicationListBS.AddingNew += assetApplicationDataGridView_BindingSourceAddNew;
            assetApplicationDataGridView.DataSource = assetApplicationListBS;
            assetApplicationDataGridView.DataError += new DataGridViewDataErrorEventHandler(assetApplicationDataGridView_ComboDataError);
            log.Debug("Ends-PopulateDeploymentPlanGrid() method.");
        }
        private void assetApplicationDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-assetApplicationDataGridView_BindingSourceAddNew() Event.");
            if (assetApplicationDataGridView.Rows.Count == assetApplicationListBS.Count)
            {
                assetApplicationListBS.RemoveAt(assetApplicationListBS.Count - 1);
            }
            log.Debug("Ends-assetApplicationDataGridView_BindingSourceAddNew() Event.");
        }
        private void assetApplicationDataGridView_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-assetApplicationDataGridView_ComboDataError() Event.");
            if (e.ColumnIndex == assetApplicationDataGridView.Columns["patchApplicationTypeIdDataGridViewTextBoxColumn"].Index)
            {
                if (patchApplicationTypeListOnDisplay != null)
                    assetApplicationDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = patchApplicationTypeListOnDisplay[0].PatchApplicationTypeId;
            }
            log.Debug("Ends-assetApplicationDataGridView_ComboDataError() Event.");
        }

        private void assetApplicationDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-assetApplicationDataGridView_DataError() Event.");
            MessageBox.Show("Error in grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + assetApplicationDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.Debug("Ends-assetApplicationDataGridView_DataError() Event.");
        }

        private void assetApplicationSaveBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-assetApplicationSaveBtn_Click() Event.");
            BindingSource assetApplicationListBS = (BindingSource)assetApplicationDataGridView.DataSource;
            var assetApplicationListOnDisplay = (List<PatchAssetApplicationDTO>)assetApplicationListBS.DataSource;
            if (assetApplicationListOnDisplay.Count > 0)
            {
                foreach (PatchAssetApplicationDTO assetApplicationDTO in assetApplicationListOnDisplay)
                {
                    if (assetApplicationDTO.IsChanged)
                    {
                        if (assetApplicationDTO.AssetId==-1)
                        {
                            MessageBox.Show("Please select the asset.");
                            return;
                        }
                        if (assetApplicationDTO.PatchApplicationTypeId==-1)
                        {
                            MessageBox.Show("Please select application type.");
                            return;
                        }
                        if (string.IsNullOrEmpty(assetApplicationDTO.PatchVersionNumber))
                        {
                            MessageBox.Show("Please enter the patch version.");
                            return;
                        }
                        if (string.IsNullOrEmpty(assetApplicationDTO.ApplicationPath))
                        {
                            MessageBox.Show("Please select the application path.");
                            return;
                        }                        
                    }
                    PatchAssetApplication assetApplication = new PatchAssetApplication(assetApplicationDTO);
                    assetApplication.Save();
                }
                PopulateAssetApplicationGrid();
            }
            else
                MessageBox.Show("Nothing to save");
            log.Debug("Ends-assetApplicationSaveBtn_Click() Event.");
        }

        private void assetApplicationRefreshBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-assetApplicationRefreshBtn_Click() Event.");
            PopulateAssetApplicationGrid();
            log.Debug("Ends-assetApplicationRefreshBtn_Click() Event.");
        }

        private void assetApplicationDeleteBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-assetApplicationDeleteBtn_Click() event.");
            if (this.assetApplicationDataGridView.SelectedRows.Count <= 0 && this.assetApplicationDataGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show("No rows selected. Please select the rows you want to delete and press delete..");
                log.Debug("Ends-assetApplicationDeleteBtn_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message");
                return;
            }
            bool rowsDeleted = false;
            if (this.assetApplicationDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.assetApplicationDataGridView.SelectedCells)
                {
                    assetApplicationDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }

            foreach (DataGridViewRow deploymentPlanRow in this.assetApplicationDataGridView.SelectedRows)
            {
                if (Convert.ToInt32(deploymentPlanRow.Cells[0].Value.ToString()) <= 0)
                {
                    assetApplicationDataGridView.Rows.RemoveAt(deploymentPlanRow.Index);
                    rowsDeleted = true;
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show("Rows deleted succesfully");
            else
                MessageBox.Show("Deleting the existing record is not allowed.Please make it inactive..");
            log.Debug("Ends-assetApplicationDeleteBtn_Click() event.");
        }

        private void assetApplicationCloseBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-assetApplicationCloseBtn_Click() Event.");
            this.Close();
            log.Debug("Ends-assetApplicationCloseBtn_Click() Event.");
        }
       
    }
}
