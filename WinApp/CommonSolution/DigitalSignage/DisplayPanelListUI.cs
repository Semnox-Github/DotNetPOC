/********************************************************************************************
 * Project Name - DisplayPanelList UI
 * Description  - User interface for DisplayPanelList
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By           Remarks          
 *********************************************************************************************
 *2.70.2        14-Aug-2019   Dakshakh              Added logger methods
 *2.70.2        08-Jan-2019   Lakshminarayana       Modified for locker layout changes
 *2.80.0        17-Feb-2019   Deeksha               Modified to Make DigitalSignage module as
 *                                                  read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Publish;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// DisplayPanelListUI Class.
    /// </summary>
    public partial class DisplayPanelListUI : Form
    {
        private Utilities utilities;
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private readonly DeviceClass readerDevice;
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// Constructor of DisplayPanelListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="readerDevice">Card reader</param>
        public DisplayPanelListUI(Utilities utilities, DeviceClass readerDevice)
        {
            log.LogMethodEntry(utilities, readerDevice);
            InitializeComponent();
            this.utilities = utilities;
            this.readerDevice = readerDevice;
            utilities.setupDataGridProperties(ref dgvDisplayPanelDTOList);
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
            lnkPublish.Visible = false;
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublish.Visible = true;
            }
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void DisplayPanelListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            pCNameDataGridViewTextBoxColumn.MaxInputLength = 100;
            panelNameDataGridViewTextBoxColumn.MaxInputLength = 100;
            displayGroupDataGridViewTextBoxColumn.MaxInputLength = 50;
            locationDataGridViewTextBoxColumn.MaxInputLength = 50;
            mACAddressDataGridViewTextBoxColumn.MaxInputLength = 50;
            descriptionDataGridViewTextBoxColumn.MaxInputLength = 100;
            localFolderDataGridViewTextBoxColumn.MaxInputLength = 50;
            LoadResolutionType();
            LoadTimeType();
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadDisplayPanelDTOList();
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the scroll directions to the combo boxes
        /// </summary>
        private void LoadResolutionType()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SCREEN RESOLUTION HORIZONTAL"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> resolutionLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (resolutionLookUpValueList == null)
                {
                    resolutionLookUpValueList = new List<LookupValuesDTO>();
                }
                resolutionLookUpValueList.Insert(0, new LookupValuesDTO());
                resolutionLookUpValueList[0].LookupValueId = -1;
                resolutionLookUpValueList[0].LookupValue = "<SELECT>";
                resolutionXDataGridViewComboBoxColumn.DataSource = resolutionLookUpValueList;
                resolutionXDataGridViewComboBoxColumn.ValueMember = "LookupValueId";
                resolutionXDataGridViewComboBoxColumn.DisplayMember = "LookupValue";

                lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SCREEN RESOLUTION VERTICAL"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                resolutionLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (resolutionLookUpValueList == null)
                {
                    resolutionLookUpValueList = new List<LookupValuesDTO>();
                }
                resolutionLookUpValueList.Insert(0, new LookupValuesDTO());
                resolutionLookUpValueList[0].LookupValueId = -1;
                resolutionLookUpValueList[0].LookupValue = "<SELECT>";
                resolutionYDataGridViewComboBoxColumn.DataSource = resolutionLookUpValueList;
                resolutionYDataGridViewComboBoxColumn.ValueMember = "LookupValueId";
                resolutionYDataGridViewComboBoxColumn.DisplayMember = "LookupValue";
                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadScrollDirectionType() Method with an Exception:", e);
            }
        }

        private void LoadTimeType()
        {
            log.LogMethodEntry();
            List<KeyValuePair<decimal, string>> timeList = new List<KeyValuePair<decimal, string>>();
            TimeSpan ts;
            for (int i = 0; i <= 95; i++)
            {
                ts = new TimeSpan(0, i * 15, 0);
                timeList.Add(new KeyValuePair<decimal, string>(Convert.ToDecimal(ts.Hours + ts.Minutes * 0.01), string.Format("{0:0}:{1:00} {2}", (ts.Hours % 12) == 0 ? (ts.Hours == 12 ? 12 : 0) : ts.Hours % 12, ts.Minutes, ts.Hours >= 12 ? "PM" : "AM")));
            }
            BindingSource bs = new BindingSource();
            bs.DataSource = timeList;
            startTimeDataGridViewComboBoxColumn.DataSource = bs;
            startTimeDataGridViewComboBoxColumn.ValueMember = "Key";
            startTimeDataGridViewComboBoxColumn.DisplayMember = "Value";
            bs = new BindingSource();
            bs.DataSource = timeList;
            endTimeDataGridViewComboBoxColumn.DataSource = bs;
            endTimeDataGridViewComboBoxColumn.ValueMember = "Key";
            endTimeDataGridViewComboBoxColumn.DisplayMember = "Value";
            log.LogMethodExit ();
        }

        private void LoadDisplayPanelDTOList()
        {
            log.LogMethodEntry();
            DisplayPanelListBL displayPanelListBL = new DisplayPanelListBL(machineUserContext);
            List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.DISPLAY_GROUP, txtDisplayGroup.Text));
            searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<DisplayPanelDTO> displayPanelDTOList = displayPanelListBL.GetDisplayPanelDTOList(searchParameters);
            SortableBindingList<DisplayPanelDTO> displayPanelDTOSortableList;
            if (displayPanelDTOList != null)
            {
                displayPanelDTOSortableList = new SortableBindingList<DisplayPanelDTO>(displayPanelDTOList);
            }
            else
            {
                displayPanelDTOSortableList = new SortableBindingList<DisplayPanelDTO>();
            }
            displayPanelDTOListBS.DataSource = displayPanelDTOSortableList;
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                chbShowActiveEntries.Checked = true;
                txtDisplayGroup.ResetText();
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

        private void dgvDisplayPanelDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvDisplayPanelDTOList.Columns[e.ColumnIndex].HeaderText +
                   ": " + e.Exception.Message);
                e.Cancel = true;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvDisplayPanelDTOList.EndEdit();
                SortableBindingList<DisplayPanelDTO> displayPanelDTOSortableList = (SortableBindingList<DisplayPanelDTO>)displayPanelDTOListBS.DataSource;
                string message;
                DisplayPanelBL displayPanelBL;
                bool error = false;
                if (displayPanelDTOSortableList != null)
                {
                    for (int i = 0; i < displayPanelDTOSortableList.Count; i++)
                    {
                        if (displayPanelDTOSortableList[i].IsChanged)
                        {
                            message = ValidateDisplayPanelDTO(displayPanelDTOSortableList[i]);
                            if (string.IsNullOrEmpty(message))
                            {
                                try
                                {
                                    displayPanelBL = new DisplayPanelBL(machineUserContext, displayPanelDTOSortableList[i]);
                                    displayPanelBL.Save();
                                }
                                catch (ForeignKeyException ex)
                                {
                                    log.Error(ex);
                                    dgvDisplayPanelDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                    break;
                                }
                                catch (Exception)
                                {
                                    error = true;
                                    log.Error("Error while saving displayPanel.");
                                    dgvDisplayPanelDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                    break;
                                }
                            }
                            else
                            {
                                error = true;
                                dgvDisplayPanelDTOList.Rows[i].Selected = true;
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
                    dgvDisplayPanelDTOList.Update();
                    dgvDisplayPanelDTOList.Refresh();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private string ValidateDisplayPanelDTO(DisplayPanelDTO displayPanelDTO, bool macAddressRequired = false)
        {
            log.LogMethodEntry(displayPanelDTO, macAddressRequired);
            string message = string.Empty;
            if (string.IsNullOrEmpty(displayPanelDTO.PanelName) || string.IsNullOrWhiteSpace(displayPanelDTO.PanelName))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", panelNameDataGridViewTextBoxColumn.HeaderText);
            }
            if (string.IsNullOrEmpty(displayPanelDTO.PCName) || string.IsNullOrWhiteSpace(displayPanelDTO.PCName))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", pCNameDataGridViewTextBoxColumn.HeaderText);
            }
            if (string.IsNullOrEmpty(displayPanelDTO.PCName) || string.IsNullOrWhiteSpace(displayPanelDTO.PCName))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", pCNameDataGridViewTextBoxColumn.HeaderText);
            }
            //if (displayPanelDTO.StartTime >= displayPanelDTO.EndTime)
            //{
            //    message = utilities.MessageUtils.getMessage(571);
            //}
            if (macAddressRequired)
            {
                if (string.IsNullOrEmpty(displayPanelDTO.MACAddress))
                {
                    message = utilities.MessageUtils.getMessage(1144);
                    message = message.Replace("&1", mACAddressDataGridViewTextBoxColumn.HeaderText);
                }
                else
                {
                    Regex regex = new Regex("^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$");
                    if (!regex.IsMatch(displayPanelDTO.MACAddress))
                    {
                        message = utilities.MessageUtils.getMessage(1144);
                        message = message.Replace("&1", mACAddressDataGridViewTextBoxColumn.HeaderText);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(displayPanelDTO.MACAddress))
                {
                    Regex regex = new Regex("^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$");
                    if (!regex.IsMatch(displayPanelDTO.MACAddress))
                    {
                        message = utilities.MessageUtils.getMessage(1144);
                        message = message.Replace("&1", mACAddressDataGridViewTextBoxColumn.HeaderText);
                    }
                }
            }
            if (displayPanelDTO.ShutdownSec != null && displayPanelDTO.ShutdownSec <= 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", shutdownSecDataGridViewTextBoxColumn.HeaderText);
            }
            if (displayPanelDTO.ResolutionX < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", resolutionXDataGridViewComboBoxColumn.HeaderText);
            }
            if (displayPanelDTO.ResolutionY < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", resolutionYDataGridViewComboBoxColumn.HeaderText);
            }
            if (string.IsNullOrWhiteSpace(displayPanelDTO.LocalFolder))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", localFolderDataGridViewTextBoxColumn.HeaderText);
            }
            log.LogMethodExit(message);
            return message;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvDisplayPanelDTOList.SelectedRows.Count <= 0 && dgvDisplayPanelDTOList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.LogMethodExit();
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                bool refreshFromDB = false;
                if (this.dgvDisplayPanelDTOList.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvDisplayPanelDTOList.SelectedCells)
                    {
                        dgvDisplayPanelDTOList.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow row in dgvDisplayPanelDTOList.SelectedRows)
                {
                    if (row.Cells[panelIdDataGridViewTextBoxColumn.Index].Value == null)
                    {
                        continue;
                    }
                    if (Convert.ToInt32(row.Cells[panelIdDataGridViewTextBoxColumn.Index].Value.ToString()) < 0)
                    {
                        dgvDisplayPanelDTOList.Rows.RemoveAt(row.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            refreshFromDB = true;
                            SortableBindingList<DisplayPanelDTO> displayPanelDTOSortableList = (SortableBindingList<DisplayPanelDTO>)displayPanelDTOListBS.DataSource;
                            DisplayPanelDTO displayPanelDTO = displayPanelDTOSortableList[row.Index];
                            displayPanelDTO.IsActive = false;
                            DisplayPanelBL displayPanelBL = new DisplayPanelBL(machineUserContext, displayPanelDTO);
                            try
                            {
                                displayPanelBL.Save();
                            }
                            catch (ForeignKeyException ex)
                            {
                                log.Error(ex.Message);
                                dgvDisplayPanelDTOList.Rows[row.Index].Selected = true;
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

        private void dgvDisplayPanelDTOList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            /*string localFolder = string.Empty;
            if(e.ColumnIndex == localFolderDataGridViewTextBoxColumn.Index)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if(dgvDisplayPanelDTOList.CurrentCell.Value != null && !string.IsNullOrEmpty(dgvDisplayPanelDTOList.CurrentCell.Value.ToString()))
                {
                    fbd.SelectedPath = dgvDisplayPanelDTOList.CurrentCell.Value.ToString();
                }
                if(fbd.ShowDialog() == DialogResult.OK)
                {
                    localFolder = fbd.SelectedPath;
                    dgvDisplayPanelDTOList.CurrentCell.Value = localFolder;
                    dgvDisplayPanelDTOList.EndEdit();
                }
            }*/
            log.LogMethodExit();
        }

        private void btnRestartPC_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvDisplayPanelDTOList.EndEdit();
            if (dgvDisplayPanelDTOList.SelectedRows.Count <= 0 && dgvDisplayPanelDTOList.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1169));
                log.LogMethodExit();
                return;
            }
            if (this.dgvDisplayPanelDTOList.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvDisplayPanelDTOList.SelectedCells)
                {
                    dgvDisplayPanelDTOList.Rows[cell.RowIndex].Selected = true;
                }
            }
            SortableBindingList<DisplayPanelDTO> displayPanelDTOSortableList = displayPanelDTOListBS.DataSource as SortableBindingList<DisplayPanelDTO>;
            if (displayPanelDTOSortableList != null && displayPanelDTOSortableList.Count > 0)
            {
                bool updatedDisplayPanelDTO = false;
                foreach (DataGridViewRow row in dgvDisplayPanelDTOList.SelectedRows)
                {
                    DisplayPanelDTO displayPanelDTO = displayPanelDTOSortableList[row.Index];
                    string message = ValidateDisplayPanelDTO(displayPanelDTO);
                    if (!string.IsNullOrEmpty(message))
                    {
                        MessageBox.Show(message);
                        dgvDisplayPanelDTOList.ClearSelection();
                        row.Selected = true;
                        return;
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1172).Replace("&1", displayPanelDTO.PCName));
                    }
                    updatedDisplayPanelDTO = true;
                    displayPanelDTO.RestartFlag = "Y";
                }
                if (updatedDisplayPanelDTO == true)
                {
                    btnSave.PerformClick();
                }
            }
            log.LogMethodExit();
        }

        private void btnStartPC_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvDisplayPanelDTOList.EndEdit();
            if (dgvDisplayPanelDTOList.SelectedRows.Count <= 0 && dgvDisplayPanelDTOList.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1135));
                log.LogMethodExit("Ends-btnStartPC_Click() event by \"No Devices selected. Please select the devices you want to start and press Start PC..\" message ");
                return;
            }
            if (this.dgvDisplayPanelDTOList.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvDisplayPanelDTOList.SelectedCells)
                {
                    dgvDisplayPanelDTOList.Rows[cell.RowIndex].Selected = true;
                }
            }
            SortableBindingList<DisplayPanelDTO> displayPanelDTOSortableList = displayPanelDTOListBS.DataSource as SortableBindingList<DisplayPanelDTO>;
            if (displayPanelDTOSortableList != null && displayPanelDTOSortableList.Count > 0)
            {
                foreach (DataGridViewRow row in dgvDisplayPanelDTOList.SelectedRows)
                {
                    DisplayPanelDTO displayPanelDTO = displayPanelDTOSortableList[row.Index];
                    string message = ValidateDisplayPanelDTO(displayPanelDTO, true);
                    if (!string.IsNullOrEmpty(message))
                    {
                        MessageBox.Show(message);
                        dgvDisplayPanelDTOList.ClearSelection();
                        row.Selected = true;
                        return;
                    }
                    DisplayPanelBL displayPanelBL = new DisplayPanelBL(machineUserContext, displayPanelDTO);
                    try
                    {
                        if (!displayPanelBL.StartPC())
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1136).Replace("&1", displayPanelDTO.PCName));
                        }
                        else
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1137).Replace("&1", displayPanelDTO.PCName));
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1136).Replace("&1", displayPanelDTO.PCName));
                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnShutDown_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvDisplayPanelDTOList.EndEdit();
                if (dgvDisplayPanelDTOList.SelectedRows.Count <= 0 && dgvDisplayPanelDTOList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1138));
                    log.LogMethodExit("Ends-btnStartPC_Click() event by \"No Devices selected. Please select the devices you want to shutdown and press Shutdown PC..\" message ");
                    return;
                }
                if (this.dgvDisplayPanelDTOList.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvDisplayPanelDTOList.SelectedCells)
                    {
                        dgvDisplayPanelDTOList.Rows[cell.RowIndex].Selected = true;
                    }
                }
                SortableBindingList<DisplayPanelDTO> displayPanelDTOSortableList = displayPanelDTOListBS.DataSource as SortableBindingList<DisplayPanelDTO>;
                if (displayPanelDTOSortableList != null && displayPanelDTOSortableList.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvDisplayPanelDTOList.SelectedRows)
                    {
                        DisplayPanelDTO displayPanelDTO = displayPanelDTOSortableList[row.Index];
                        string message = ValidateDisplayPanelDTO(displayPanelDTO);
                        if (!string.IsNullOrEmpty(message))
                        {
                            MessageBox.Show(message);
                            dgvDisplayPanelDTOList.ClearSelection();
                            row.Selected = true;
                            return;
                        }

                        DialogResult dg = MessageBox.Show(utilities.MessageUtils.getMessage(1139) + displayPanelDTO.PCName + " ?", " Shutdown ?", MessageBoxButtons.YesNo);
                        if (dg == DialogResult.No)
                        {
                            continue;
                        }
                        DisplayPanelBL displayPanelBL = new DisplayPanelBL(machineUserContext, displayPanelDTO);
                        if (displayPanelBL.ShutdownPC())
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1141).Replace("&1", displayPanelDTO.PCName));
                        }
                        else
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1140).Replace("&1", displayPanelDTO.PCName));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvDisplayPanelDTOList.EndEdit();
                if (displayPanelDTOListBS.Current != null && displayPanelDTOListBS.Current is DisplayPanelDTO)
                {
                    DisplayPanelDTO displayPanelDTO = displayPanelDTOListBS.Current as DisplayPanelDTO;
                    string message = ValidateDisplayPanelDTO(displayPanelDTO);
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        PreviewUI previewUI = new PreviewUI(utilities, displayPanelDTO, readerDevice);
                        previewUI.ShowDialog();
                        previewUI.Close();
                        previewUI.Dispose();
                    }
                    else
                    {
                        MessageBox.Show(message);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void lnkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvDisplayPanelDTOList.CurrentRow.Cells["panelIdDataGridViewTextBoxColumn"].Value != null)
                {
                    int id = Convert.ToInt32(dgvDisplayPanelDTOList.CurrentRow.Cells["panelIdDataGridViewTextBoxColumn"].Value);

                    if (id <= 0)
                        return;
                    PublishUI publishUI = new PublishUI(utilities, id, "DisplayPanel", dgvDisplayPanelDTOList.CurrentRow.Cells["panelNameDataGridViewTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }
        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                dgvDisplayPanelDTOList.AllowUserToAddRows = true;
                dgvDisplayPanelDTOList.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvDisplayPanelDTOList.AllowUserToAddRows = false;
                dgvDisplayPanelDTOList.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
