
/********************************************************************************************
 * Project Name - Screen Zone Def Setup UI
 * Description  - User interface for screen zone def setup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Mar-2016   Raghuveera          Created 
 *2.70.2      14-Aug-2019   Dakshakh            Added logger methods
 *2.80.0      17-Feb-2019   Deeksha             Modified to Make DigitalSignage module as
 *                                              read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// User interface for Screen Zone Def Setup UI
    /// </summary>
    public partial class ScreenZoneDefSetupUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private int screenId = -1;
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_Utilities"> Will hold environment</param>
        public ScreenZoneDefSetupUI(Utilities _Utilities, int screenId)
        {
            log.LogMethodEntry(_Utilities, screenId);
            InitializeComponent();
            utilities = _Utilities;
            this.screenId = screenId;
            utilities.setupDataGridProperties(ref dgvZoneSetupGrid);

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
            PopulateScreenZoneDefSetupGrid();
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the screen zone def setup to grid
        /// </summary>
        private void PopulateScreenZoneDefSetupGrid()
        {
            log.LogMethodEntry();
            ScreenZoneDefSetupList screenZoneDefSetupList = new ScreenZoneDefSetupList(machineUserContext);
            List<KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>> screenZoneDefSetupSearchParams = new List<KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                screenZoneDefSetupSearchParams.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                screenZoneDefSetupSearchParams.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.NAME, txtName.Text));
            }
            screenZoneDefSetupSearchParams.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.SCREEN_ID, screenId.ToString()));
            screenZoneDefSetupSearchParams.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<ScreenZoneDefSetupDTO> screenZoneDefSetupListOnDisplay = screenZoneDefSetupList.GetAllScreenZoneDefSetup(screenZoneDefSetupSearchParams);
            BindingSource screenZoneDefSetupListBS = new BindingSource();
            if (screenZoneDefSetupListOnDisplay != null)
            {
                SortableBindingList<ScreenZoneDefSetupDTO> screenZoneDefSetupDTOSortList = new SortableBindingList<ScreenZoneDefSetupDTO>(screenZoneDefSetupListOnDisplay);
                screenZoneDefSetupListBS.DataSource = screenZoneDefSetupDTOSortList;
            }
            else
                screenZoneDefSetupListBS.DataSource = new SortableBindingList<ScreenZoneDefSetupDTO>();
            dgvZoneSetupGrid.DataSource = screenZoneDefSetupListBS;
            log.LogMethodExit();
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvZoneSetupGrid.EndEdit();
                BindingSource screenZoneDefSetupListBS = (BindingSource)dgvZoneSetupGrid.DataSource;
                var screenZoneDefSetupListOnDisplay = (SortableBindingList<ScreenZoneDefSetupDTO>)screenZoneDefSetupListBS.DataSource;
                if (screenZoneDefSetupListOnDisplay.Count > 0)
                {
                    foreach (ScreenZoneDefSetupDTO screenZoneDefSetupDTO in screenZoneDefSetupListOnDisplay)
                    {
                        if (string.IsNullOrEmpty(screenZoneDefSetupDTO.Name))
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", nameDataGridViewTextBoxColumn.HeaderText));
                            return;
                        }
                        if (screenZoneDefSetupDTO.ScreenId == -1)
                        {
                            screenZoneDefSetupDTO.ScreenId = screenId;
                        }
                        ScreenZoneDefSetup screenZoneDefSetup = new ScreenZoneDefSetup(machineUserContext, screenZoneDefSetupDTO);
                        screenZoneDefSetup.Save(null);
                    }
                    btnSearch.PerformClick();

                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnRefersh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtName.Text = "";
                chbShowActiveEntries.Checked = true;
                PopulateScreenZoneDefSetupGrid();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (this.dgvZoneSetupGrid.SelectedRows.Count <= 0 && this.dgvZoneSetupGrid.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.LogMethodExit();
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvZoneSetupGrid.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvZoneSetupGrid.SelectedCells)
                    {
                        dgvZoneSetupGrid.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow screenZoneDefSetupRow in this.dgvZoneSetupGrid.SelectedRows)
                {
                    if (Convert.ToInt32(screenZoneDefSetupRow.Cells[0].Value.ToString()) < 0)
                    {
                        dgvZoneSetupGrid.Rows.RemoveAt(screenZoneDefSetupRow.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            BindingSource screenZoneDefSetupDTOListDTOBS = (BindingSource)dgvZoneSetupGrid.DataSource;
                            var screenZoneDefSetupDTOList = (SortableBindingList<ScreenZoneDefSetupDTO>)screenZoneDefSetupDTOListDTOBS.DataSource;
                            ScreenZoneDefSetupDTO screenZoneDefSetupDTO = screenZoneDefSetupDTOList[screenZoneDefSetupRow.Index];
                            screenZoneDefSetupDTO.IsActive = false;
                            ScreenZoneDefSetup screenZoneDefSetup = new ScreenZoneDefSetup(machineUserContext, screenZoneDefSetupDTO);
                            screenZoneDefSetup.Save(null);
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                btnSearch.PerformClick();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                PopulateScreenZoneDefSetupGrid();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnViewZone_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvZoneSetupGrid.SelectedCells != null && dgvZoneSetupGrid.SelectedCells.Count > 0)
                {
                    dgvZoneSetupGrid.Rows[dgvZoneSetupGrid.SelectedCells[0].RowIndex].Selected = true;
                }

                if (dgvZoneSetupGrid.SelectedRows != null && dgvZoneSetupGrid.SelectedRows.Count > 0)
                {
                    if (dgvZoneSetupGrid.SelectedRows[0].Cells["zoneIdDataGridViewTextBoxColumn"].Value != null &&
                        Convert.ToInt32(dgvZoneSetupGrid.SelectedRows[0].Cells["zoneIdDataGridViewTextBoxColumn"].Value) > -1)
                    {
                        ViewZone viewZone = new ViewZone(Convert.ToInt32(dgvZoneSetupGrid.SelectedRows[0].Cells["screenIdDataGridViewTextBoxColumn"].Value), dgvZoneSetupGrid.SelectedRows[0].Cells["nameDataGridViewTextBoxColumn"].Value.ToString(), Convert.ToInt32(dgvZoneSetupGrid.SelectedRows[0].Cells["topLeftDataGridViewTextBoxColumn"].Value), Convert.ToInt32(dgvZoneSetupGrid.SelectedRows[0].Cells["bottomRightDataGridViewTextBoxColumn"].Value));
                        viewZone.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1134));
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                dgvZoneSetupGrid.AllowUserToAddRows = true;
                dgvZoneSetupGrid.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvZoneSetupGrid.AllowUserToAddRows = false;
                dgvZoneSetupGrid.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
