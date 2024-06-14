/********************************************************************************************
 * Project Name - Screen Setup UI
 * Description  - User interface for screen setup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        22-Jan-2016   Raghuveera          Created 
 *2.70.2      14-Aug-2019   Deeksha             Added logger methods
 *2.80.0      17-Feb-2019   Deeksha             Modified to Make DigitalSignage module as
 *                                              read only in Windows Management Studio.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// ScreenSetup UI
    /// </summary>
    public partial class ScreenSetupUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private List<LookupValuesDTO> lookupValuesDTOList = null;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_Utilities"> Will hold environment</param>
        public ScreenSetupUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvScreenSetupGrid);

            if(utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the status to the comboboxes
        /// </summary>
        private void LoadAlignement()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SCREEN_ALIGNMENT"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if(lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDTOList[0].LookupValueId = -1;
                lookupValuesDTOList[0].LookupValue = "<SELECT>";
                alignmentDataGridViewTextBoxColumn.DataSource = lookupValuesDTOList;
                alignmentDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                alignmentDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
                log.LogMethodExit();
            }
            catch(Exception e)
            {
                log.Error("Ends-LoadStatus() Method with an Exception:", e);
            }
        }
        /// <summary>
        /// Loads the screen setup to grid
        /// </summary>
        private void PopulateScreenSetupGrid()
        {
            log.LogMethodEntry();
            ScreenSetupList screenSetupList = new ScreenSetupList(machineUserContext);
            List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> screenSetupSearchParams = new List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>>();
            if(chbShowActiveEntries.Checked)
            {
                screenSetupSearchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            if(!string.IsNullOrEmpty(txtName.Text))
            {
                screenSetupSearchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.NAME, txtName.Text));
            }
            screenSetupSearchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<ScreenSetupDTO> screenSetupListOnDisplay = screenSetupList.GetAllScreenSetup(screenSetupSearchParams);
            BindingSource screenSetupListBS = new BindingSource();
            if(screenSetupListOnDisplay != null)
            {
                SortableBindingList<ScreenSetupDTO> screenSetupDTOSortList = new SortableBindingList<ScreenSetupDTO>(screenSetupListOnDisplay);
                screenSetupListBS.DataSource = screenSetupDTOSortList;
            }
            else
                screenSetupListBS.DataSource = new SortableBindingList<ScreenSetupDTO>();
            dgvScreenSetupGrid.DataSource = screenSetupListBS;
            log.LogMethodExit();
        }
        private void dgvScreenSetupGrid_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if(e.ColumnIndex == dgvScreenSetupGrid.Columns["alignmentDataGridViewTextBoxColumn"].Index)
            {
                if(lookupValuesDTOList != null)
                    dgvScreenSetupGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = lookupValuesDTOList[0].LookupValueId;
            }

            log.LogMethodExit();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvScreenSetupGrid.EndEdit();
                BindingSource screenSetupListBS = (BindingSource)dgvScreenSetupGrid.DataSource;
                var screenSetupListOnDisplay = (SortableBindingList<ScreenSetupDTO>)screenSetupListBS.DataSource;
                if (screenSetupListOnDisplay.Count > 0)
                {
                    for (int i = 0; i < screenSetupListOnDisplay.Count; i++)
                    {
                        ScreenSetupDTO screenSetupDTO = screenSetupListOnDisplay[i] as ScreenSetupDTO;
                        if (string.IsNullOrEmpty(screenSetupDTO.Name))
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", nameDataGridViewTextBoxColumn.HeaderText));
                            return;
                        }
                        if (screenSetupDTO.Alignment == -1)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", alignmentDataGridViewTextBoxColumn.HeaderText));
                            return;
                        }
                        if (screenSetupDTO.ScrDivHorizontal <= 0)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", scrDivHorizontalDataGridViewTextBoxColumn.HeaderText));
                            return;
                        }
                        if (screenSetupDTO.ScrDivVertical <= 0)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", scrDivVerticalDataGridViewTextBoxColumn.HeaderText));
                            return;
                        }
                        ScreenSetup screenSetup = new ScreenSetup(machineUserContext, screenSetupDTO);
                        try
                        {
                            screenSetup.Save(null);
                        }
                        catch (ForeignKeyException)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                            dgvScreenSetupGrid.Rows[i].Selected = true;
                            break;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            dgvScreenSetupGrid.Rows[i].Selected = true;
                            break;
                        }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnRefersh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtName.Text = "";
                chbShowActiveEntries.Checked = true;
                PopulateScreenSetupGrid();
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
                if (this.dgvScreenSetupGrid.SelectedRows.Count <= 0 && this.dgvScreenSetupGrid.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-screenSetupDeleteBtn_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvScreenSetupGrid.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvScreenSetupGrid.SelectedCells)
                    {
                        dgvScreenSetupGrid.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow screenSetupRow in this.dgvScreenSetupGrid.SelectedRows)
                {
                    if (Convert.ToInt32(screenSetupRow.Cells[0].Value.ToString()) < 0)
                    {
                        dgvScreenSetupGrid.Rows.RemoveAt(screenSetupRow.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            BindingSource screenSetupDTOListDTOBS = (BindingSource)dgvScreenSetupGrid.DataSource;
                            var screenSetupDTOList = (SortableBindingList<ScreenSetupDTO>)screenSetupDTOListDTOBS.DataSource;
                            ScreenSetupDTO screenSetupDTO = screenSetupDTOList[screenSetupRow.Index];
                            screenSetupDTO.IsActive = false;
                            ScreenSetup screenSetup = new ScreenSetup(machineUserContext, screenSetupDTO);
                            try
                            {
                                screenSetup.Save(null);
                            }
                            catch (ForeignKeyException)
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                dgvScreenSetupGrid.Rows[screenSetupRow.Index].Selected = true;
                                break;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                PopulateScreenSetupGrid();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvScreenSetupGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                int screenId = Convert.ToInt32(dgvScreenSetupGrid.Rows[e.RowIndex].Cells["screenIdDataGridViewTextBoxColumn"].Value);
                if (screenId != -1)
                {
                    if (e.RowIndex > -1 && e.ColumnIndex == dgvScreenSetupGrid.Columns["ContentMap"].Index)
                    {
                        ScreenZoneContentMapUI screenZoneContentMapUI = new ScreenZoneContentMapUI(utilities, Convert.ToInt32(dgvScreenSetupGrid.Rows[e.RowIndex].Cells["screenIdDataGridViewTextBoxColumn"].Value));
                        screenZoneContentMapUI.ShowDialog();
                    }
                    else if (e.RowIndex > -1 && e.ColumnIndex == dgvScreenSetupGrid.Columns["AddUpdateZones"].Index)
                    {
                        ScreenZoneDefSetupUI screenZoneDefSetupUI = new ScreenZoneDefSetupUI(utilities, Convert.ToInt32(dgvScreenSetupGrid.Rows[e.RowIndex].Cells["screenIdDataGridViewTextBoxColumn"].Value));
                        screenZoneDefSetupUI.ShowDialog();
                    }
                }
                else
                {
                    dgvScreenSetupGrid.Rows[e.RowIndex].Selected = true;
                    MessageBox.Show(utilities.MessageUtils.getMessage(1134));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void ScreenSetupUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadAlignement();
            PopulateScreenSetupGrid();
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                dgvScreenSetupGrid.AllowUserToAddRows = true;
                dgvScreenSetupGrid.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvScreenSetupGrid.AllowUserToAddRows = false;
                dgvScreenSetupGrid.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
