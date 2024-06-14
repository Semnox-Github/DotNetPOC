/********************************************************************************************
 * Project Name - Parafait - frmEntityExcusion
 * Description  - Entity Override 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 ********************************************************************************************* 
* 2.80        24-Jun-2020      Deeksha            Modified to Make Product module 
*                                                 read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// class of frmEntityOverride
    /// </summary>
    public partial class frmEntityOverride : Form
    {
        Utilities Utilities;
        string entityReference;
        string entityName;
        string entityNameOnDisplay;

        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entityReference"></param>
        /// <param name="entityNameOnDisplay"></param>
        /// <param name="_utilities"></param>
        public frmEntityOverride(string entityName, string entityNameOnDisplay, string entityReference, Utilities _utilities)
        {
            log.LogMethodEntry(entityName, entityNameOnDisplay, entityReference, _utilities);
            InitializeComponent();

            this.entityName = entityName;
            this.entityReference = entityReference;
            this.entityNameOnDisplay = entityNameOnDisplay;
            Utilities = _utilities;
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        /// <summary>
        /// frmEntityOverride_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmEntityOverride_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Utilities.setupDataGridProperties(ref dgvEntityExclusion);

            if (Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }

            machineUserContext.SetUserId(Utilities.ParafaitEnv.Username);
            //overrideDateDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateCellStyle();
            //overrideDateDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            lastUpdatedDateDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();

            LoadDay();
            PopulatedgvEntityExclusion();
            lblExclude.Text = Utilities.MessageUtils.getMessage("Include / Exclude Days from") + " " + entityNameOnDisplay;
            overrideDateDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateCellStyle();
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads day to the grid.
        /// </summary>
        private void LoadDay()
        {
            log.LogMethodEntry();
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Value", typeof(string));
            dt.Rows.Add(-1, " ");
            dt.Rows.Add(1, Utilities.MessageUtils.getMessage("Sunday"));
            dt.Rows.Add(2, Utilities.MessageUtils.getMessage("Monday"));
            dt.Rows.Add(3, Utilities.MessageUtils.getMessage("Tuesday"));
            dt.Rows.Add(4, Utilities.MessageUtils.getMessage("Wednesday"));
            dt.Rows.Add(5, Utilities.MessageUtils.getMessage("Thursday"));
            dt.Rows.Add(6, Utilities.MessageUtils.getMessage("Friday"));
            dt.Rows.Add(7, Utilities.MessageUtils.getMessage("Saturday"));

            dayDataGridViewTextBoxColumn.DataSource = dt;
            dayDataGridViewTextBoxColumn.DisplayMember = "Value";
            dayDataGridViewTextBoxColumn.ValueMember = "Id";
            log.LogMethodExit();
        }

        /// <summary>
        /// PopulatedgvEntityExclusion
        /// </summary>
        void PopulatedgvEntityExclusion()
        {
            log.LogMethodEntry();

            try
            {
                EntityOverrideList entityOverrideList = new EntityOverrideList(machineUserContext);
                List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> SearchParameters = new List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>>();
                SearchParameters.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_GUID, entityReference));
                List<EntityOverrideDatesDTO> entityOverrideListOnDisplay = entityOverrideList.GetAllEntityOverrideList(SearchParameters);

                BindingSource entityOverrideListBS = new BindingSource();

                if (entityOverrideListOnDisplay != null)
                {
                    foreach (EntityOverrideDatesDTO d in entityOverrideListOnDisplay)
                    {
                        if (!string.IsNullOrEmpty(d.OverrideDate))
                        {
                            d.OverrideDate = Convert.ToDateTime(d.OverrideDate).ToShortDateString();
                        }
                    }
                    entityOverrideListBS.DataSource = entityOverrideListOnDisplay;
                }
                else
                {
                    entityOverrideListBS.DataSource = new List<EntityOverrideDatesDTO>();
                }
                dgvEntityExclusion.DataSource = entityOverrideListBS;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnSave_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                BindingSource entityOverrideListBS = (BindingSource)dgvEntityExclusion.DataSource;
                var entityOverrideListOnDisplay = (List<EntityOverrideDatesDTO>)entityOverrideListBS.DataSource;

                EntityOverrideDate entityOverrideDate;

                if (entityOverrideListOnDisplay != null && entityOverrideListOnDisplay.Count > 0)
                {
                    foreach (EntityOverrideDatesDTO d in entityOverrideListOnDisplay)
                    {
                        if (string.IsNullOrEmpty(d.OverrideDate) && d.Day == -1)
                        {
                            MessageBox.Show("Please enter the value");
                            break;
                        }

                        if (d.IsChanged)
                        {
                            d.EntityName = entityName;
                            d.EntityGuid = entityReference;
                            entityOverrideDate = new EntityOverrideDate(machineUserContext, d);
                            entityOverrideDate.Save(-1);
                        }
                    }
                    PopulatedgvEntityExclusion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnRefresh_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulatedgvEntityExclusion();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnDelete_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvEntityExclusion.CurrentRow == null || dgvEntityExclusion.CurrentRow.Cells[0].Value == DBNull.Value || dgvEntityExclusion.CurrentRow.Cells[0].Value.ToString() == "-1")
                    return;
                else
                {
                    DialogResult result = MessageBox.Show("Do you want to delete ?", "Delete Entity Override", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int id = Convert.ToInt32(dgvEntityExclusion.CurrentRow.Cells[0].Value);

                        EntityOverrideDate entityOverrideDate = new EntityOverrideDate(machineUserContext);
                        int deleteStatus = entityOverrideDate.Delete(id);
                        PopulatedgvEntityExclusion();
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                MessageBox.Show(expn.Message.ToString());
            }
        }

        /// <summary>
        /// btnClose_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvEntityExclusion_DataError
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvEntityExclusion_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                MessageBox.Show("Error in EntityOverride grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + dgvEntityExclusion.Columns[e.ColumnIndex].DataPropertyName +
                   ": " + e.Exception.Message);
                e.Cancel = true;
            }
            catch { }
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvEntityExclusion_CellValidating
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvEntityExclusion_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvEntityExclusion.Columns[e.ColumnIndex].Name == "overrideDateDataGridViewTextBoxColumn")
            {
                try
                {
                    if (!string.IsNullOrEmpty(e.FormattedValue.ToString()))
                    {
                        dgvEntityExclusion.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Convert.ToDateTime(e.FormattedValue).ToShortDateString();
                    }
                    e.Cancel = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    e.Cancel = true;
                }
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                dgvEntityExclusion.AllowUserToAddRows = true;
                dgvEntityExclusion.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvEntityExclusion.AllowUserToAddRows = false;
                dgvEntityExclusion.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
