/********************************************************************************************
 * Project Name - EventListUI
 * Description  - UI for Event List
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        14-Aug-2019   Dakshakh       Added logger methods
 *2.80.0        17-Feb-2019   Deeksha        Modified to Make DigitalSignage module as
 *                                           read only in Windows Management Studio.
 *2.100.0       13-Sep-2020   Girish Kundar  Modified : Phase -3 changes REST API                                         read only in Windows Management Studio.
 *********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Publish;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Event UI
    /// </summary>
    public partial class EventListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private List<LookupValuesDTO> eventTypeLookUpValueList;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Constructor of EventListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public EventListUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvEventDTOList);
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
            lnkPublish.Visible = false;
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublish.Visible = true;
            }
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void EventListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cbEventType.DropDownStyle = ComboBoxStyle.DropDownList;
            dgvEventDTOListDescriptionTextBoxColumn.MaxInputLength = 100;
            dgvEventDTOListNameTextBoxColumn.MaxInputLength = 50;
            dgvEventDTOListParameterTextBoxColumn.MaxInputLength = 8000;
            LoadEventType();
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadEventDTOList();
            log.LogMethodExit();
        }

        private void LoadEventDTOList()
        {
            log.LogMethodEntry();
            try
            {
                EventListBL eventListBL = new EventListBL(machineUserContext);
                List<KeyValuePair<EventDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EventDTO.SearchByParameters, string>>();
                if (chbShowActiveEntries.Checked)
                {
                    searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.IS_ACTIVE, "1"));
                }
                searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.NAME, txtName.Text));
                try
                {
                    if(Convert.ToInt32(cbEventType.SelectedValue) >= 0)
                    {
                        searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.TYPE_ID, Convert.ToString(cbEventType.SelectedValue)));
                    }
                }
                catch(Exception)
                {
                }
                searchParameters.Add(new KeyValuePair<EventDTO.SearchByParameters, string>(EventDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<EventDTO> eventDTOList = eventListBL.GetEventDTOList(searchParameters);
                SortableBindingList<EventDTO> eventDTOSortableList;
                if(eventDTOList != null)
                {
                    eventDTOSortableList = new SortableBindingList<EventDTO>(eventDTOList);
                }
                else
                {
                    eventDTOSortableList = new SortableBindingList<EventDTO>();
                }
                eventDTOListBS.DataSource = eventDTOSortableList;
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message); 
                log.Error("Ends-LoadEventDTOList() Method with an Exception:", ex);
            }
        }

        private void LoadEventType()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "EVENT_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                eventTypeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if(eventTypeLookUpValueList == null)
                {
                    eventTypeLookUpValueList = new List<LookupValuesDTO>();
                }
                eventTypeLookUpValueList.Insert(0, new LookupValuesDTO());
                eventTypeLookUpValueList[0].LookupValueId = -1;
                eventTypeLookUpValueList[0].LookupValue = "<SELECT>";
                BindingSource bs = new BindingSource();
                bs.DataSource = eventTypeLookUpValueList;
                cbEventType.DataSource = bs;
                cbEventType.ValueMember = "LookupValueId";
                cbEventType.DisplayMember = "LookupValue";

                bs = new BindingSource();
                bs.DataSource = eventTypeLookUpValueList;
                dgvEventDTOListTypeIdComboBoxColumn.DataSource = bs;
                dgvEventDTOListTypeIdComboBoxColumn.ValueMember = "LookupValueId";
                dgvEventDTOListTypeIdComboBoxColumn.DisplayMember = "LookupValue";
                log.LogMethodExit();
            }
            catch(Exception e)
            {
                log.Error("Ends-LoadEventType() Method with an Exception:", e);
            }
        }

        private void dgvEventDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvEventDTOList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                chbShowActiveEntries.Checked = true;
                txtName.ResetText();
                cbEventType.SelectedIndex = 0;
                RefreshData();
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
                RefreshData();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvEventDTOList.EndEdit();
                SortableBindingList<EventDTO> eventDTOSortableList = (SortableBindingList<EventDTO>)eventDTOListBS.DataSource;
                string message;
                EventBL eventBL;
                bool error = false;
                if (eventDTOSortableList != null)
                {
                    for (int i = 0; i < eventDTOSortableList.Count; i++)
                    {
                        if (eventDTOSortableList[i].IsChanged)
                        {
                            message = ValidateEventDTO(eventDTOSortableList[i]);
                            if (string.IsNullOrEmpty(message))
                            {
                                try
                                {
                                    eventBL = new EventBL(machineUserContext, eventDTOSortableList[i]);
                                    eventBL.Save();
                                }
                                catch (ForeignKeyException ex)
                                {
                                    log.Error(ex.Message);
                                    dgvEventDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                    break;
                                }
                                catch (Exception)
                                {
                                    error = true;
                                    log.Error("Error while saving event.");
                                    dgvEventDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                    break;
                                }
                            }
                            else
                            {
                                error = true;
                                dgvEventDTOList.Rows[i].Selected = true;
                                MessageBox.Show(message);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                }
                if (!error)
                {
                    btnSearch.PerformClick();
                }
                else
                {
                    dgvEventDTOList.Update();
                    dgvEventDTOList.Refresh();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private string ValidateEventDTO(EventDTO eventDTO)
        {
            log.LogMethodEntry(eventDTO);
            string message = string.Empty;
            if(string.IsNullOrEmpty(eventDTO.Name) || string.IsNullOrWhiteSpace(eventDTO.Name))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvEventDTOListNameTextBoxColumn.HeaderText);
            }
            if(eventDTO.TypeId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvEventDTOListTypeIdComboBoxColumn.HeaderText);
            }
            else
            {
                if(string.Equals(getEventType(eventDTO.TypeId), "Query"))
                {
                    if(string.IsNullOrEmpty(eventDTO.Parameter) || string.IsNullOrWhiteSpace(eventDTO.Parameter))
                    {
                        message = utilities.MessageUtils.getMessage(1144);
                        message = message.Replace("&1", dgvEventDTOListParameterTextBoxColumn.HeaderText);
                    }
                    else
                    {
                        EventBL eventBL = new EventBL(machineUserContext, eventDTO);
                        if(!eventBL.CheckQuery(eventDTO.Parameter))
                        {
                            message = utilities.MessageUtils.getMessage(1144);
                            message = message.Replace("&1", dgvEventDTOListParameterTextBoxColumn.HeaderText);
                        }
                    }

                }
                else if(string.Equals(getEventType(eventDTO.TypeId), "Timer"))
                {
                    if(string.IsNullOrEmpty(eventDTO.Parameter) || string.IsNullOrWhiteSpace(eventDTO.Parameter))
                    {
                        message = utilities.MessageUtils.getMessage(1144);
                        message = message.Replace("&1", dgvEventDTOListParameterTextBoxColumn.HeaderText);
                    }
                    else
                    {
                        int i;
                        if(!int.TryParse(eventDTO.Parameter, out i) || Convert.ToInt32(eventDTO.Parameter) <= 0)
                        {
                            message = utilities.MessageUtils.getMessage(1133);
                            message = message.Replace("&1", dgvEventDTOListParameterTextBoxColumn.HeaderText);
                        }
                    }
                }
            }
            log.LogMethodExit(message);
            return message;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvEventDTOList.SelectedRows.Count <= 0 && dgvEventDTOList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.LogMethodExit();
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                bool refreshFromDB = false;
                if (this.dgvEventDTOList.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvEventDTOList.SelectedCells)
                    {
                        dgvEventDTOList.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow row in dgvEventDTOList.SelectedRows)
                {
                    if (Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                    {
                        dgvEventDTOList.Rows.RemoveAt(row.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            SortableBindingList<EventDTO> eventDTOSortableList = (SortableBindingList<EventDTO>)eventDTOListBS.DataSource;
                            EventDTO eventDTO = eventDTOSortableList[row.Index];
                            eventDTO.IsActive = false;
                            EventBL eventBL = new EventBL(machineUserContext, eventDTO);
                            confirmDelete = true;
                            refreshFromDB = true;
                            try
                            {
                                eventBL.Save();
                            }
                            catch (ForeignKeyException ex)
                            {
                                log.Error(ex.Message);
                                dgvEventDTOList.Rows[row.Index].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                continue;
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                }
                if (refreshFromDB == true)
                {
                    btnSearch.PerformClick();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvEventDTOList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvEventDTOList.EndEdit();
                if (e.ColumnIndex == dgvEventDTOListParameterTextBoxColumn.Index)
                {
                    if (eventDTOListBS.Current != null && string.Equals(getEventType((eventDTOListBS.Current as EventDTO).TypeId), "Query"))
                    {
                        SortableBindingList<EventDTO> eventDTOSortableList = null;
                        if (eventDTOListBS != null && eventDTOListBS.DataSource != null)
                        {
                            eventDTOSortableList = eventDTOListBS.DataSource as SortableBindingList<EventDTO>;
                            EventDTO eventDTO = null;
                            if (eventDTOSortableList != null)
                            {
                                eventDTO = eventDTOSortableList[e.RowIndex];
                                if (eventDTO != null)
                                {
                                    QueryPopupUI queryPopupUI = new QueryPopupUI(utilities, eventDTO.Id, eventDTO.Parameter, eventDTO.Name);
                                    if (queryPopupUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        dgvEventDTOList.CurrentCell.Value = queryPopupUI.QueryString;
                                    }
                                    queryPopupUI.Close();
                                    queryPopupUI.Dispose();
                                    dgvEventDTOList.Update();
                                    dgvEventDTOList.Refresh();

                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private string getEventType(int typeId)
        {
            log.LogMethodEntry(typeId);
            string type = string.Empty;
            if(typeId >= 0 && eventTypeLookUpValueList != null && eventTypeLookUpValueList.Count > 0)
            {
                foreach(LookupValuesDTO lookupValuesDTO in eventTypeLookUpValueList)
                {
                    if(lookupValuesDTO.LookupValueId == typeId)
                    {
                        type = lookupValuesDTO.LookupValue;
                        break;
                    }
                }
            }
            log.LogMethodExit();
            return type;
        }

        private void lnkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvEventDTOList.CurrentRow.Cells["dgvEventDTOListIdTextBoxColumn"].Value != null)
                {
                    int id = Convert.ToInt32(dgvEventDTOList.CurrentRow.Cells["dgvEventDTOListIdTextBoxColumn"].Value);

                    if (id <= 0)
                        return;
                    PublishUI publishUI = new PublishUI(utilities, id, "Event", dgvEventDTOList.CurrentRow.Cells["dgvEventDTOListNameTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                dgvEventDTOList.AllowUserToAddRows = true;
                dgvEventDTOList.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                lnkPublish.Enabled = true;
            }
            else
            {
                dgvEventDTOList.AllowUserToAddRows = false;
                dgvEventDTOList.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                lnkPublish.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
