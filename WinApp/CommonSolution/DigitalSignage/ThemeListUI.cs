/********************************************************************************************
 * Project Name - Digial Signage - ThemeListUI
 * Description  - ThemeListUI form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        04-May-2019      Guru S A       Make game module as readonly in Windows Management Studio 
 *2.70.2      14-Aug-2019      Dakshakh       Added logger methods
 *2.80.0      17-Feb-2019      Deeksha        Modified to Make DigitalSignage module as
 *                                            read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Theme UI
    /// </summary>
    public partial class ThemeListUI : Form
    {
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private string themeTypeList;
        private bool readerTheme;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Constructor of ThemeListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public ThemeListUI(Utilities utilities, bool readerTheme = false)
        {
            log.LogMethodEntry(utilities, readerTheme);
            InitializeComponent();
            this.readerTheme = readerTheme;
            if (readerTheme)
            {
                themeTypeList = "Audio,Display,Visualization";
            }
            else
            {
                themeTypeList = "Panel";
                themeNumberDataGridViewTextBoxColumn.Visible = false;
            }
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvScreenTransitionsDTOList);
            utilities.setupDataGridProperties(ref dgvThemeDTOList);
            lnkHQPublish.Visible = false;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                if (utilities.ParafaitEnv.IsMasterSite)
                {
                    lnkHQPublish.Visible = true;
                }
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            log.LogMethodExit();
        }

        private void themeListBS_CurrentItemChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SortableBindingList<ScreenTransitionsDTO> screenTransitionsDTOSortableList = null;
            if(themeDTOListBS.Current != null && themeDTOListBS.Current is ThemeDTO)
            {
                ThemeDTO themeDTO = themeDTOListBS.Current as ThemeDTO;
                if(themeDTO.ScreenTransitionsDTOList != null)
                {
                    screenTransitionsDTOSortableList = themeDTO.ScreenTransitionsDTOList;
                }
            }
            if(screenTransitionsDTOSortableList == null)
            {
                dgvScreenTransitionsDTOList.AllowUserToAddRows = false;
                screenTransitionsDTOListBS.DataSource = new SortableBindingList<ScreenTransitionsDTO>();
            }
            else
            {
                dgvScreenTransitionsDTOList.AllowUserToAddRows = true;
                screenTransitionsDTOListBS.DataSource = screenTransitionsDTOSortableList;
            }
            log.LogMethodExit();
        }

        private void ThemeListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cbThemeType.DropDownStyle = ComboBoxStyle.DropDownList;
            descriptionDataGridViewTextBoxColumn.MaxInputLength = 100;
            nameDataGridViewTextBoxColumn.MaxInputLength = 50;   
            LoadThemeType();
            LoadScreenSetupDTOList();
            LoadEventDTOList();
            RefreshData();
            UpdateUIElements();
            log.LogMethodExit();
        }


        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableGameModule)
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                dgvThemeDTOList.AllowUserToAddRows = true;
                dgvThemeDTOList.ReadOnly = false;
                dgvScreenTransitionsDTOList.AllowUserToAddRows = true;
                dgvScreenTransitionsDTOList.ReadOnly = false;
            }
            else
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                dgvThemeDTOList.AllowUserToAddRows = false;
                dgvThemeDTOList.ReadOnly = true;
                dgvScreenTransitionsDTOList.AllowUserToAddRows = false;
                dgvScreenTransitionsDTOList.ReadOnly = true;
            }
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                dgvThemeDTOList.AllowUserToAddRows = true;
                dgvThemeDTOList.ReadOnly = false;
                dgvScreenTransitionsDTOList.AllowUserToAddRows = true;
                dgvScreenTransitionsDTOList.ReadOnly = false;
            }
            else
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                dgvThemeDTOList.AllowUserToAddRows = false;
                dgvThemeDTOList.ReadOnly = true;
                dgvScreenTransitionsDTOList.AllowUserToAddRows = false;
                dgvScreenTransitionsDTOList.ReadOnly = true;
            }
            log.LogMethodExit();
        }

        private void LoadThemeType()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "THEME_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if(lookUpValueList == null)
                {
                    lookUpValueList = new List<LookupValuesDTO>();
                }
                List<LookupValuesDTO> themeTypeLookUpValueList = new List<LookupValuesDTO>();
                foreach (var lookupValuesDTO in lookUpValueList)
                {
                    if(themeTypeList.Contains(lookupValuesDTO.LookupValue))
                    {
                        themeTypeLookUpValueList.Add(lookupValuesDTO);
                    }
                }
                themeTypeLookUpValueList.Insert(0, new LookupValuesDTO());
                themeTypeLookUpValueList[0].LookupValueId = -1;
                themeTypeLookUpValueList[0].LookupValue = "<SELECT>";

                BindingSource bs = new BindingSource();
                bs.DataSource = themeTypeLookUpValueList;
                typeIdDataGridViewComboBoxColumn.DataSource = bs;
                typeIdDataGridViewComboBoxColumn.ValueMember = "LookupValueId";
                typeIdDataGridViewComboBoxColumn.DisplayMember = "LookupValue";

                bs = new BindingSource();
                bs.DataSource = themeTypeLookUpValueList;
                cbThemeType.DataSource = bs;
                cbThemeType.ValueMember = "LookupValueId";
                cbThemeType.DisplayMember = "LookupValue";
                log.LogMethodExit();
            }
            catch(Exception e)
            {
                log.Error("Ends-LoadAlignmentType() Method with an Exception:", e);
            }
            
        }

        private void LoadScreenSetupDTOList()
        {
            log.LogMethodEntry();
            try
            {
                ScreenSetupList screenSetupList = new ScreenSetupList(machineUserContext);
                List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.IS_ACTIVE, "1"));
                List<ScreenSetupDTO> screenSetupDTOList = screenSetupList.GetAllScreenSetup(searchParams);
                if(screenSetupDTOList == null)
                {
                    screenSetupDTOList = new List<ScreenSetupDTO>();
                }
                ScreenSetupDTO screenSetupDTO = new ScreenSetupDTO();
                screenSetupDTO.Name = "<SELECT>";
                screenSetupDTOList.Insert(0, screenSetupDTO);
                SortableBindingList<ScreenSetupDTO> screenSetupDTOSortableBindingList = new SortableBindingList<ScreenSetupDTO>(screenSetupDTOList);

                BindingSource bs = new BindingSource();
                bs.DataSource = screenSetupDTOSortableBindingList;

                initialScreenIdDataGridViewComboBoxColumn.DataSource = bs;
                initialScreenIdDataGridViewComboBoxColumn.ValueMember = "ScreenId";
                initialScreenIdDataGridViewComboBoxColumn.DisplayMember = "Name";

                bs = new BindingSource();
                bs.DataSource = screenSetupDTOSortableBindingList;

                dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn.DataSource = bs;
                dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn.ValueMember = "ScreenId";
                dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn.DisplayMember = "Name";

                bs = new BindingSource();
                bs.DataSource = screenSetupDTOSortableBindingList;

                dgvScreenTransitionsDTOListToScreenIdComboBoxColumn.DataSource = bs;
                dgvScreenTransitionsDTOListToScreenIdComboBoxColumn.ValueMember = "ScreenId";
                dgvScreenTransitionsDTOListToScreenIdComboBoxColumn.DisplayMember = "Name";
                log.LogMethodExit();
            }
            catch(Exception e)
            {
                log.Error("Ends-LoadScreenSetupDTOList() Method with an Exception:", e);
            }
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            dgvScreenTransitionsDTOList.AllowUserToAddRows = false;
            LoadThemeDTOList();
            log.LogMethodExit();
        }

        private void LoadThemeDTOList()
        {
            log.LogMethodEntry();
            ThemeListBL themeListBL = new ThemeListBL(machineUserContext);
            List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.TYPE_LIST, themeTypeList));
            searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.NAME, txtName.Text));
            try
            {
                if(Convert.ToInt32(cbThemeType.SelectedValue) >= 0)
                {
                    searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.TYPE_ID, Convert.ToString(cbThemeType.SelectedValue)));
                }
            }
            catch(Exception)
            {
            }
            searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<ThemeDTO> themeDTOList = themeListBL.GetThemeDTOList(searchParameters, true, true);
            SortableBindingList<ThemeDTO> themeDTOSortableList;
            if(themeDTOList != null)
            {
                themeDTOSortableList = new SortableBindingList<ThemeDTO>(themeDTOList);
            }
            else
            {
                themeDTOSortableList = new SortableBindingList<ThemeDTO>();
            }
            themeDTOListBS.DataSource = themeDTOSortableList;
            log.LogMethodExit();
        }

        private void LoadEventDTOList()
        {
            log.LogMethodEntry();
            EventListBL eventListBL = new EventListBL(machineUserContext);
            List<KeyValuePair<EventDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EventDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.IS_ACTIVE,  "1"));
            searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<EventDTO> eventDTOList = eventListBL.GetEventDTOList(searchParameters);
            if(eventDTOList == null)
            {
                eventDTOList = new List<EventDTO>();
            }
            eventDTOList.Insert(0, new EventDTO());
            eventDTOList[0].Name = "<SELECT>";
            dgvScreenTransitionsDTOListEventIdComboBoxColumn.DataSource = eventDTOList;
            dgvScreenTransitionsDTOListEventIdComboBoxColumn.ValueMember = "Id";
            dgvScreenTransitionsDTOListEventIdComboBoxColumn.DisplayMember = "Name";
            log.LogMethodExit();
        }

        private void dgvThemeDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            bool handled = false;
            if(e.ColumnIndex == initialScreenIdDataGridViewComboBoxColumn.Index)
            {
                SortableBindingList<ThemeDTO> themeDTOSortableList = null;
                if(themeDTOListBS.DataSource is SortableBindingList<ThemeDTO>)
                {
                    themeDTOSortableList = (SortableBindingList<ThemeDTO>)themeDTOListBS.DataSource;
                }
                if(themeDTOSortableList != null && themeDTOSortableList.Count > 0)
                {
                    DataGridViewComboBoxCell cell = dgvScreenTransitionsDTOList[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
                    if(cell != null)
                    {
                        cell.Value = -1;
                        handled = true;
                    }
                }
            }
            if(handled == false)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvThemeDTOList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            }
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvScreenTransitionsDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            bool handled = false;
            if(e.ColumnIndex == dgvScreenTransitionsDTOListToScreenIdComboBoxColumn.Index ||
                e.ColumnIndex == dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn.Index ||
                e.ColumnIndex == dgvScreenTransitionsDTOListEventIdComboBoxColumn.Index)
            {
                SortableBindingList<ScreenTransitionsDTO> screenTransitionsDTOSortableList = null;
                if(screenTransitionsDTOListBS.DataSource is SortableBindingList<ScreenTransitionsDTO>)
                {
                    screenTransitionsDTOSortableList = (SortableBindingList<ScreenTransitionsDTO>)screenTransitionsDTOListBS.DataSource;
                }
                if(screenTransitionsDTOSortableList != null && screenTransitionsDTOSortableList.Count > 0)
                {
                    DataGridViewComboBoxCell cell = dgvScreenTransitionsDTOList[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
                    if(cell != null)
                    {
                        cell.Value = -1;
                        handled = true;
                    }
                }
            }
            if(handled == false)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvScreenTransitionsDTOList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            }
            e.Cancel = true;
            log.Error(e);
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            chbShowActiveEntries.Checked = true;
            txtName.ResetText();
            cbThemeType.SelectedIndex = 0;
            LoadScreenSetupDTOList();
            LoadEventDTOList();
            RefreshData();
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
            RefreshData();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SortableBindingList<ThemeDTO> themeDTOSortableList = (SortableBindingList<ThemeDTO>)themeDTOListBS.DataSource;
            string message = string.Empty;
            ThemeBL themeBL;
            bool error = false;
            dgvScreenTransitionsDTOList.EndEdit();
            dgvThemeDTOList.EndEdit();
            if(themeDTOSortableList != null)
            {
                for(int i = 0; i < themeDTOSortableList.Count; i++)
                {
                    if(themeDTOSortableList[i].IsChanged)
                    {
                        message = ValidateThemeDTO(themeDTOSortableList[i]);
                        if(!string.IsNullOrEmpty(message))
                        {
                            dgvThemeDTOList.Rows[i].Selected = true;
                            MessageBox.Show(message);
                            return;
                        }
                    }
                    for(int j = 0; j < themeDTOSortableList[i].ScreenTransitionsDTOList.Count; j++)
                    {
                        if(themeDTOSortableList[i].ScreenTransitionsDTOList[j].IsChanged)
                        {
                            message = ValidateScreenTransitionsDTO(themeDTOSortableList[i].ScreenTransitionsDTOList[j]);
                            if(!string.IsNullOrEmpty(message))
                            {
                                dgvScreenTransitionsDTOList.Rows[j].Selected = true;
                                break;
                            }
                        }
                    }
                    if(string.IsNullOrEmpty(message))
                    {
                        try
                        {
                            themeBL = new ThemeBL(machineUserContext,themeDTOSortableList[i]);
                            themeBL.Save();
                        }
                        catch(ForeignKeyException ex)
                        {
                            log.Error(ex.Message);
                            dgvThemeDTOList.Rows[i].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                            break;
                        }
                        catch(Exception)
                        {
                            error = true;
                            log.Error("Error while saving Theme.");
                            dgvThemeDTOList.Rows[i].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(718));
                            break;
                        }
                    }
                    else
                    {
                        error = true;
                        dgvThemeDTOList.Rows[i].Selected = true;
                        MessageBox.Show(message);
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            if(!error)
            {
                btnSearch.PerformClick();
            }
            else
            {
                dgvThemeDTOList.Update();
                dgvThemeDTOList.Refresh();
                dgvScreenTransitionsDTOList.Update();
                dgvScreenTransitionsDTOList.Refresh();
            }
            log.LogMethodExit();
        }

        private string ValidateThemeDTO(ThemeDTO themeDTO)
        {
            log.LogMethodEntry(themeDTO);
            string message = string.Empty;
            if(string.IsNullOrEmpty(themeDTO.Name) || string.IsNullOrWhiteSpace(themeDTO.Name))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", nameDataGridViewTextBoxColumn.HeaderText);
            }
            if(themeDTO.TypeId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", typeIdDataGridViewComboBoxColumn.HeaderText);
            }
            if(readerTheme == false && themeDTO.InitialScreenId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", initialScreenIdDataGridViewComboBoxColumn.HeaderText);
            }
            log.LogMethodExit();
            return message;
        }

        private string ValidateScreenTransitionsDTO(ScreenTransitionsDTO screenTransitionsDTO)
        {
            log.LogMethodEntry(screenTransitionsDTO);
            string message = string.Empty;
            if(screenTransitionsDTO.FromScreenId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn.HeaderText);
            }
            if(screenTransitionsDTO.EventId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvScreenTransitionsDTOListEventIdComboBoxColumn.HeaderText);
            }
            if(screenTransitionsDTO.ToScreenId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvScreenTransitionsDTOListToScreenIdComboBoxColumn.HeaderText);
            }
            log.LogMethodExit(message);
            return message;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            if (dgvThemeDTOList.SelectedRows.Count <= 0 && dgvThemeDTOList.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.LogMethodExit();
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDb = false;
            if(dgvScreenTransitionsDTOList.SelectedCells.Count > 0)
            {
                foreach(DataGridViewCell cell in dgvScreenTransitionsDTOList.SelectedCells)
                {
                    dgvScreenTransitionsDTOList.Rows[cell.RowIndex].Selected = true;
                }
            }
            if(dgvScreenTransitionsDTOList.SelectedRows.Count > 0)
            {
                foreach(DataGridViewRow row in dgvScreenTransitionsDTOList.SelectedRows)
                {
                    if(row.Cells[dgvScreenTransitionsDTOListIsActiveCheckBoxColumn.Index].Value != null &&
                        string.Equals(row.Cells[dgvScreenTransitionsDTOListIsActiveCheckBoxColumn.Index].Value.ToString(), "Y"))
                    {
                        if(row.Cells[dgvScreenTransitionsDTOListIdTextBoxColumn.Index].Value != null &&
                            Convert.ToInt32(row.Cells[dgvScreenTransitionsDTOListIdTextBoxColumn.Index].Value.ToString()) < 0)
                        {
                            dgvScreenTransitionsDTOList.Rows.RemoveAt(row.Index);
                            rowsDeleted = true;
                        }
                        else
                        {
                            if(confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                refreshFromDb = true;
                                SortableBindingList<ScreenTransitionsDTO> screenTransitionsDTOSortableList = (SortableBindingList<ScreenTransitionsDTO>)screenTransitionsDTOListBS.DataSource;
                                ScreenTransitionsDTO screenTransitionsDTO = screenTransitionsDTOSortableList[row.Index];
                                screenTransitionsDTO.IsActive = false;
                                ScreenTransitionsBL screenTransitionsBL = new ScreenTransitionsBL(machineUserContext, screenTransitionsDTO);
                                screenTransitionsBL.Save();
                            }
                        }
                    }
                }
                if(rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
            }
            if(rowsDeleted == false && confirmDelete == false)
            {
                if(themeDTOListBS != null && themeDTOListBS.Current != null && themeDTOListBS.Current is ThemeDTO)
                {
                    ThemeDTO themeDTO = themeDTOListBS.Current as ThemeDTO;
                    if(themeDTO != null)
                    {
                        if(themeDTO.ScreenTransitionsDTOList != null && themeDTO.ScreenTransitionsDTOList.Count > 0)
                        {
                            ScreenTransitionsDTO screenTransitionsDTO = null;
                            for(int i = themeDTO.ScreenTransitionsDTOList.Count - 1; i >= 0; i--)
                            {
                                if(string.Equals(themeDTO.ScreenTransitionsDTOList[i].IsActive, "Y"))
                                {
                                    screenTransitionsDTO = themeDTO.ScreenTransitionsDTOList[i];
                                    break;
                                }
                            }
                            if(screenTransitionsDTO != null)
                            {
                                if(screenTransitionsDTO.Id < 0)
                                {
                                    themeDTO.ScreenTransitionsDTOList.Remove(screenTransitionsDTO);
                                    rowsDeleted = true;
                                }
                                else
                                {
                                    if(confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                    {
                                        confirmDelete = true;
                                        refreshFromDb = true;
                                        screenTransitionsDTO.IsActive = false;
                                        ScreenTransitionsBL screenTransitionsBL = new ScreenTransitionsBL(machineUserContext, screenTransitionsDTO);
                                        screenTransitionsBL.Save();
                                    }
                                }
                            }
                        }
                        if(rowsDeleted == false && confirmDelete == false)
                        {
                            if(themeDTO.Id < 0)
                            {
                                SortableBindingList<ThemeDTO> themeDTOSortableList = (SortableBindingList<ThemeDTO>)themeDTOListBS.DataSource;
                                themeDTOSortableList.Remove(themeDTO);
                                rowsDeleted = true;
                            }
                            else
                            {
                                if(confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    refreshFromDb = true;
                                    themeDTO.IsActive = false;
                                    ThemeBL themeBL = new ThemeBL(machineUserContext,themeDTO);
                                    try
                                    {
                                        themeBL.Save();
                                    }
                                    catch(ForeignKeyException ex)
                                    {
                                        log.Error(ex.Message);
                                        MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                    }
                                }
                            }
                            if(rowsDeleted == true)
                                MessageBox.Show(utilities.MessageUtils.getMessage(957));
                        }

                    }
                }
            }
            if(refreshFromDb)
            {
                btnSearch.PerformClick();
            }
            log.LogMethodExit();
        }

        private void lnkHQPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            PublishUI publishUI;
            if (dgvThemeDTOList.SelectedCells != null && dgvThemeDTOList.SelectedCells.Count > 0)
            {
                dgvThemeDTOList.Rows[dgvThemeDTOList.SelectedCells[0].RowIndex].Selected = true;
            }
            if (dgvThemeDTOList.SelectedRows != null && dgvThemeDTOList.SelectedRows.Count > 0)
            {
                publishUI = new PublishUI(utilities, Convert.ToInt32(dgvThemeDTOList.SelectedRows[0].Cells["idDataGridViewTextBoxColumn"].Value), "Theme", dgvThemeDTOList.SelectedRows[0].Cells["nameDataGridViewTextBoxColumn"].Value.ToString());
                publishUI.ShowDialog();
            }
            log.LogMethodExit();
        }
    }
}
