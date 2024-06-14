﻿//using Semnox.Core.SortableBindingList;
/********************************************************************************************
 * Project Name - Patch Application Type UI
 * Description  - User interface for patch application type
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
using Semnox.Parafait.logger;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// Patch Application Type UI
    /// </summary>
    public partial class AutoPatchApplicationTypeUI : Form
    {
        Utilities utilities;
        BindingSource applicationTypeListBS;
       Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext applicationTypeContext = ExecutionContext.GetExecutionContext();
        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="_Utilities">Utilities object as a parameter </param>
        public AutoPatchApplicationTypeUI(Utilities _Utilities)
        {
            log.Debug("Starts-PatchApplicationTypeUI(Utilities) parameterized constructor.");
            var weekDays = new Dictionary<string, string>();
            InitializeComponent();
            utilities = _Utilities;
            utilities.setLanguage(this);
            utilities.setupDataGridProperties(ref applicationTypeDataGridView);
            if(utilities.ParafaitEnv.LoginID.Equals("semnox")||utilities.ParafaitEnv.Role == "System Administrator")
            {
                isActiveDataGridViewTextBoxColumn.ReadOnly = false;
            }
            
            if (utilities.ParafaitEnv.IsCorporate)
            {
                applicationTypeContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                applicationTypeContext.SetSiteId(-1);
            }
            //applicationTypeContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            applicationTypeContext.SetUserId(utilities.ParafaitEnv.LoginID);
            applicationTypeDataGridViewTextBoxColumn.ReadOnly = true;
            PopulateApplicationTypeGrid();
            log.Debug("Ends-PatchApplicationTypeUI(Utilities) parameterized constructor.");
        }
        /// <summary>
        /// Loads application type to the grid
        /// </summary>
        private void PopulateApplicationTypeGrid()
        {
            log.Debug("Starts-PopulateDeploymentPlanGrid() method.");
            AutoPatchApplTypeList applicationTypeList = new AutoPatchApplTypeList();
            List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>> autoPatchApplTypeDTOSearchParams = new List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>>();
            autoPatchApplTypeDTOSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.SITE_ID, applicationTypeContext.GetSiteId().ToString()));
            List<AutoPatchApplTypeDTO> applicationTypeListOnDisplay = applicationTypeList.GetAllAutoPatchApplTypes(autoPatchApplTypeDTOSearchParams);
            applicationTypeListBS = new BindingSource();
            if (applicationTypeListOnDisplay != null)
                applicationTypeListBS.DataSource = new SortableBindingList<AutoPatchApplTypeDTO>(applicationTypeListOnDisplay);
            else
                applicationTypeListBS.DataSource = new SortableBindingList<AutoPatchApplTypeDTO>();
            applicationTypeListBS.AddingNew += applicationTypeDataGridView_BindingSourceAddNew;
            applicationTypeDataGridView.DataSource = applicationTypeListBS;
            log.Debug("Ends-PopulateDeploymentPlanGrid() method.");
        }
        private void applicationTypeDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-applicationTypeDataGridView_BindingSourceAddNew() Event.");
            if (applicationTypeDataGridView.Rows.Count == applicationTypeListBS.Count)
            {
                applicationTypeListBS.RemoveAt(applicationTypeListBS.Count - 1);
            }
            log.Debug("Ends-applicationTypeDataGridView_BindingSourceAddNew() Event.");
        }        

        private void applicationTypeDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-applicationTypeDataGridView_DataError() Event.");
            MessageBox.Show("Error in grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + applicationTypeDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.Debug("Ends-applicationTypeDataGridView_DataError() Event.");
        }
        private void applicationTypeSaveBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-applicationTypeSaveBtn_Click() Event.");
            BindingSource applicationTypeListBS = (BindingSource)applicationTypeDataGridView.DataSource;
            var applicationTypeListOnDisplay = (SortableBindingList<AutoPatchApplTypeDTO>)applicationTypeListBS.DataSource;
            if (applicationTypeListOnDisplay.Count > 0)
            {
                foreach (AutoPatchApplTypeDTO applicationTypeDTO in applicationTypeListOnDisplay)
                {
                    if (applicationTypeDTO.IsChanged)
                    {
                        if (string.IsNullOrEmpty(applicationTypeDTO.ApplicationType))
                        {
                            MessageBox.Show("Please enter the application type.");
                            return;
                        }                       
                    }
                    AutoPatchApplicationType applicationType = new AutoPatchApplicationType(applicationTypeDTO);
                    applicationType.Save();
                }
                PopulateApplicationTypeGrid();
            }
            else
                MessageBox.Show("Nothing to save");
            log.Debug("Ends-applicationTypeSaveBtn_Click() Event.");
        }

        private void applicationTypeRefreshBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-applicationTypeRefreshBtn_Click() Event.");
            PopulateApplicationTypeGrid();
            log.Debug("Ends-applicationTypeRefreshBtn_Click() Event.");
        }

        private void applicationTypeDeleteBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-applicationTypeDeleteBtn_Click() event.");
            if (this.applicationTypeDataGridView.SelectedRows.Count <= 0 && this.applicationTypeDataGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-applicationTypeDeleteBtn_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.applicationTypeDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.applicationTypeDataGridView.SelectedCells)
                {
                    applicationTypeDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow applicationTypeRow in this.applicationTypeDataGridView.SelectedRows)
            {
                if (applicationTypeRow.Cells[0].Value == null)
                {
                    return;
                }
                if (Convert.ToInt32(applicationTypeRow.Cells[0].Value.ToString()) <= 0)
                {
                    applicationTypeDataGridView.Rows.RemoveAt(applicationTypeRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource autoPatchApplTypeDTOListDTOBS = (BindingSource)applicationTypeDataGridView.DataSource;
                        var autoPatchApplTypeDTOList = (SortableBindingList<AutoPatchApplTypeDTO>)autoPatchApplTypeDTOListDTOBS.DataSource;
                        AutoPatchApplTypeDTO autoPatchApplTypeDTO = autoPatchApplTypeDTOList[applicationTypeRow.Index];
                        autoPatchApplTypeDTO.IsActive = "N";
                        AutoPatchApplicationType autoPatchApplType = new AutoPatchApplicationType(autoPatchApplTypeDTO);
                        autoPatchApplType.Save();
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            PopulateApplicationTypeGrid();
            log.Debug("Ends-applicationTypeDeleteBtn_Click() event.");
        }

        private void applicationTypeCloseBtn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-applicationTypeCloseBtn_Click() Event.");
            this.Close();
            log.Debug("Ends-applicationTypeCloseBtn_Click() Event.");
        }
    }
}
