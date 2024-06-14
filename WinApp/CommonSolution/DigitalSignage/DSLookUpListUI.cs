/********************************************************************************************
 * Project Name - DSLookuplist UI
 * Description  - UI for DS Lookup list
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        14-Aug-2019   Dakshakh       Added logger methods
 *2.70.3        11-Feb-2020   Deeksha        Invariant culture-Font Issue Fix
 *2.80.0        17-Feb-2019   Deeksha        Modified to Make DigitalSignage module as
 *                                           read only in Windows Management Studio.
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Publish;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// DSLookUp UI Class.
    /// </summary>
    public partial class DSLookUpListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private bool showPopup = false;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Constructor of ThemeListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public DSLookUpListUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvDSignageLookupValuesDTOList);
            utilities.setupDataGridProperties(ref dgvDSLookupDTOList);
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

        private void dSLookUpListBS_CurrentItemChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SortableBindingList<DSignageLookupValuesDTO> dSignageLookupValuesDTOSortableList = null;
            if (dSLookupDTOListBS.Current != null && dSLookupDTOListBS.Current is DSLookupDTO)
            {
                DSLookupDTO dSLookupDTO = dSLookupDTOListBS.Current as DSLookupDTO;
                if (dSLookupDTO.DSignageLookupValuesDTOList != null)
                {
                    dSignageLookupValuesDTOSortableList = dSLookupDTO.DSignageLookupValuesDTOList;
                }
            }
            if (dSignageLookupValuesDTOSortableList == null)
            {
                dgvDSignageLookupValuesDTOList.Enabled = false;
                dSignageLookupValuesDTOListBS.DataSource = new SortableBindingList<DSignageLookupValuesDTO>();
            }
            else
            {
                dgvDSignageLookupValuesDTOList.Enabled = true;
                dSignageLookupValuesDTOListBS.DataSource = dSignageLookupValuesDTOSortableList;
                UpdateTextStyle();
            }
            log.LogMethodExit();
        }

        private void DSLookUpListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvDSLookupDTOListDSLookupNameTextBoxColumn.MaxInputLength = 50;
            dgvDSLookupDTOListDescriptionTextBoxColumn.MaxInputLength = 100;
            dgvDSignageLookupValuesDTOListValue1TextBoxColumn.MaxInputLength = 500;
            dgvDSignageLookupValuesDTOListValue2TextBoxColumn.MaxInputLength = 500;
            dgvDSignageLookupValuesDTOListValue3TextBoxColumn.MaxInputLength = 500;
            dgvDSignageLookupValuesDTOListValue4TextBoxColumn.MaxInputLength = 500;
            dgvDSignageLookupValuesDTOListValue5TextBoxColumn.MaxInputLength = 500;
            LoadAlignmentType();
            LoadDataType();
            LoadContentLayout();
            RefreshData();
            log.LogMethodExit();
        }

        

        private void LoadAlignmentType()
        {
            log.LogMethodExit();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INDENTATION"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> scrollDirectionLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (scrollDirectionLookUpValueList == null)
                {
                    scrollDirectionLookUpValueList = new List<LookupValuesDTO>();
                }
                scrollDirectionLookUpValueList.Insert(0, new LookupValuesDTO());
                scrollDirectionLookUpValueList[0].LookupValueId = -1;
                scrollDirectionLookUpValueList[0].Description = "<SELECT>";
                for (int i = 1; i <= 5; i++)
                {
                    try
                    {
                        BindingSource alignmentBS = new BindingSource();
                        alignmentBS.DataSource = scrollDirectionLookUpValueList;
                        (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "IndentationComboBoxColumn"] as DataGridViewComboBoxColumn).DataSource = alignmentBS;
                        (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "IndentationComboBoxColumn"] as DataGridViewComboBoxColumn).ValueMember = "LookupValueId";
                        (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "IndentationComboBoxColumn"] as DataGridViewComboBoxColumn).DisplayMember = "Description";
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadAlignmentType() Method with an Exception:", e);
            }
        }

        private void LoadDataType()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DATA_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> scrollDirectionLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (scrollDirectionLookUpValueList == null)
                {
                    scrollDirectionLookUpValueList = new List<LookupValuesDTO>();
                }
                scrollDirectionLookUpValueList.Insert(0, new LookupValuesDTO());
                scrollDirectionLookUpValueList[0].LookupValueId = -1;
                scrollDirectionLookUpValueList[0].Description = "<SELECT>";
                for (int i = 1; i <= 5; i++)
                {
                    try
                    {
                        BindingSource alignmentBS = new BindingSource();
                        alignmentBS.DataSource = scrollDirectionLookUpValueList;
                        (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "DataTypeComboBoxColumn"] as DataGridViewComboBoxColumn).DataSource = alignmentBS;
                        (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "DataTypeComboBoxColumn"] as DataGridViewComboBoxColumn).ValueMember = "LookupValueId";
                        (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "DataTypeComboBoxColumn"] as DataGridViewComboBoxColumn).DisplayMember = "Description";
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }

        private void LoadContentLayout()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CONTENT_LAYOUT"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> contentLayoutLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (contentLayoutLookUpValueList == null)
                {
                    contentLayoutLookUpValueList = new List<LookupValuesDTO>();
                }
                contentLayoutLookUpValueList.Insert(0, new LookupValuesDTO());
                contentLayoutLookUpValueList[0].LookupValueId = -1;
                contentLayoutLookUpValueList[0].Description = "<SELECT>";
                for (int i = 1; i <= 5; i++)
                {
                    try
                    {
                        BindingSource alignmentBS = new BindingSource();
                        alignmentBS.DataSource = contentLayoutLookUpValueList;
                        (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "ContentLayoutComboBoxColumn"] as DataGridViewComboBoxColumn).DataSource = alignmentBS;
                        (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "ContentLayoutComboBoxColumn"] as DataGridViewComboBoxColumn).ValueMember = "LookupValueId";
                        (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "ContentLayoutComboBoxColumn"] as DataGridViewComboBoxColumn).DisplayMember = "Description";
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }
        private void RefreshData()
        {
            log.LogMethodEntry();
            dgvDSignageLookupValuesDTOList.Enabled = false;
            LoadDSLookupDTOList();
            log.LogMethodExit();
        }

        private void LoadDSLookupDTOList()
        {
            log.LogMethodEntry();
            DSLookupListBL dSLookupListBL = new DSLookupListBL(machineUserContext);
            List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DSLookupDTO.SearchByParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                searchParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            searchParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.DSLOOKUP_NAME, txtName.Text));
            searchParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<DSLookupDTO> dSLookupDTOList = dSLookupListBL.GetDSLookupDTOList(searchParameters);
            SortableBindingList<DSLookupDTO> dSLookupDTOSortableList;
            if (dSLookupDTOList != null)
            {
                dSLookupDTOSortableList = new SortableBindingList<DSLookupDTO>(dSLookupDTOList);
            }
            else
            {
                dSLookupDTOSortableList = new SortableBindingList<DSLookupDTO>();
            }
            dSLookupDTOListBS.DataSource = dSLookupDTOSortableList;
            log.LogMethodExit();
        }

        private void dgvDSLookupDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvDSLookupDTOList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvDSignageLookupValuesDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvDSignageLookupValuesDTOList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.Error(e);
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                chbShowActiveEntries.Checked = true;
                txtName.ResetText();
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
                SortableBindingList<DSLookupDTO> dSLookupDTOSortableList = (SortableBindingList<DSLookupDTO>)dSLookupDTOListBS.DataSource;
                string message = string.Empty;
                DSLookupBL dSLookupBL;
                bool error = false;
                dgvDSignageLookupValuesDTOList.EndEdit();
                dgvDSLookupDTOList.EndEdit();
                if (dSLookupDTOSortableList != null)
                {
                    for (int i = 0; i < dSLookupDTOSortableList.Count; i++)
                    {
                        if (dSLookupDTOSortableList[i].IsChanged)
                        {
                            message = ValidateDSLookupDTO(dSLookupDTOSortableList[i]);
                        }
                        for (int j = 0; j < dSLookupDTOSortableList[i].DSignageLookupValuesDTOList.Count; j++)
                        {
                            if (dSLookupDTOSortableList[i].DSignageLookupValuesDTOList[j].IsChanged)
                            {
                                message = ValidateDSignageLookupValuesDTO(dSLookupDTOSortableList[i].DSignageLookupValuesDTOList[j]);
                                if (!string.IsNullOrEmpty(message))
                                {
                                    dgvDSignageLookupValuesDTOList.Rows[j].Selected = true;
                                    break;
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(message))
                        {
                            try
                            {
                                dSLookupBL = new DSLookupBL(machineUserContext, dSLookupDTOSortableList[i]);
                                dSLookupBL.Save();
                            }
                            catch (ForeignKeyException ex)
                            {
                                log.Error(ex.Message);
                                dgvDSLookupDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                break;
                            }
                            catch (Exception)
                            {
                                error = true;
                                log.Error("Error while saving DSLookup.");
                                dgvDSLookupDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                break;
                            }
                        }
                        else
                        {
                            error = true;
                            dgvDSLookupDTOList.Rows[i].Selected = true;
                            MessageBox.Show(message);
                            break;
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
                    dgvDSLookupDTOList.Update();
                    dgvDSLookupDTOList.Refresh();
                    dgvDSignageLookupValuesDTOList.Update();
                    dgvDSignageLookupValuesDTOList.Refresh();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private string ValidateDSLookupDTO(DSLookupDTO dSLookupDTO)
        {
            log.LogMethodEntry(dSLookupDTO);
            string message = string.Empty;
            if (string.IsNullOrEmpty(dSLookupDTO.DSLookupName) || string.IsNullOrWhiteSpace(dSLookupDTO.DSLookupName))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSLookupDTOListDSLookupNameTextBoxColumn.HeaderText);
            }
            log.LogMethodExit(message);
            return message;
        }

        private string ValidateDSignageLookupValuesDTO(DSignageLookupValuesDTO dSignageLookupValuesDTO)
        {
            log.LogMethodEntry(dSignageLookupValuesDTO);
            string message = string.Empty;
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val1BackColor) && !ValidateColor(dSignageLookupValuesDTO.Val1BackColor))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal1BackColorTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val1TextColor) && !ValidateColor(dSignageLookupValuesDTO.Val1TextColor))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal1TextColorTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val2BackColor) && !ValidateColor(dSignageLookupValuesDTO.Val2BackColor))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal2BackColorTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val2TextColor) && !ValidateColor(dSignageLookupValuesDTO.Val2TextColor))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal2TextColorTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val3BackColor) && !ValidateColor(dSignageLookupValuesDTO.Val3BackColor))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal3BackColorTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val3TextColor) && !ValidateColor(dSignageLookupValuesDTO.Val3TextColor))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal3TextColorTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val4BackColor) && !ValidateColor(dSignageLookupValuesDTO.Val4BackColor))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal4BackColorTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val4TextColor) && !ValidateColor(dSignageLookupValuesDTO.Val4TextColor))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal4TextColorTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val5BackColor) && !ValidateColor(dSignageLookupValuesDTO.Val5BackColor))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal5BackColorTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val5TextColor) && !ValidateColor(dSignageLookupValuesDTO.Val5TextColor))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal5TextColorTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val1Font) && !ValidateFont(dSignageLookupValuesDTO.Val1Font))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal1FontTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val2Font) && !ValidateFont(dSignageLookupValuesDTO.Val2Font))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal2FontTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val3Font) && !ValidateFont(dSignageLookupValuesDTO.Val3Font))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal3FontTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val4Font) && !ValidateFont(dSignageLookupValuesDTO.Val4Font))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal4FontTextBoxColumn.HeaderText);
            }
            if (!string.IsNullOrEmpty(dSignageLookupValuesDTO.Val5Font) && !ValidateFont(dSignageLookupValuesDTO.Val5Font))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", dgvDSignageLookupValuesDTOListVal5FontTextBoxColumn.HeaderText);
            }
            log.LogMethodExit(message);
            return message;
        }

        private bool ValidateColor(string stringColor)
        {
            log.LogMethodEntry(stringColor);
            bool valid = false;
            try
            {
                Color color = System.Drawing.ColorTranslator.FromHtml(stringColor);
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
                valid = string.Equals(converter.ConvertToString(color), stringColor);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private bool ValidateFont(string stringFont)
        {
            log.LogMethodEntry(stringFont);
            bool valid = false;
            try
            {
                TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));
                Font font = CustomFontConverter.ConvertStringToFont(utilities.ExecutionContext, stringFont);
                string fontText = fontConverter.ConvertToString(font);
                valid = string.Equals(fontText, stringFont);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvDSLookupDTOList.SelectedRows.Count <= 0 && dgvDSLookupDTOList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.LogMethodExit();
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                bool refreshFromDb = false;
                if (dgvDSignageLookupValuesDTOList.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvDSignageLookupValuesDTOList.SelectedCells)
                    {
                        dgvDSignageLookupValuesDTOList.Rows[cell.RowIndex].Selected = true;
                    }
                }
                if (dgvDSignageLookupValuesDTOList.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvDSignageLookupValuesDTOList.SelectedRows)
                    {
                        if (row.Cells[dgvDSignageLookupValuesDTOListIsActiveCheckBoxColumn.Index].Value != null &&
                            bool.Equals(row.Cells[dgvDSignageLookupValuesDTOListIsActiveCheckBoxColumn.Index].Value, true))
                        {
                            if (row.Cells[dgvDSignageLookupValuesDTOListDSLookupValueIDTextBoxColumn.Index].Value != null &&
                                Convert.ToInt32(row.Cells[dgvDSignageLookupValuesDTOListDSLookupValueIDTextBoxColumn.Index].Value.ToString()) < 0)
                            {
                                dgvDSignageLookupValuesDTOList.Rows.RemoveAt(row.Index);
                                rowsDeleted = true;
                            }
                            else
                            {
                                if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    refreshFromDb = true;
                                    SortableBindingList<DSignageLookupValuesDTO> dSignageLookupValuesDTOSortableList = (SortableBindingList<DSignageLookupValuesDTO>)dSignageLookupValuesDTOListBS.DataSource;
                                    DSignageLookupValuesDTO dSignageLookupValuesDTO = dSignageLookupValuesDTOSortableList[row.Index];
                                    dSignageLookupValuesDTO.IsActive = false;
                                    DSignageLookupValuesBL dSignageLookupValuesBL = new DSignageLookupValuesBL(machineUserContext, dSignageLookupValuesDTO);
                                    dSignageLookupValuesBL.Save();
                                }
                            }
                        }
                    }
                    if (rowsDeleted == true)
                        MessageBox.Show(utilities.MessageUtils.getMessage(957));
                }
                if (rowsDeleted == false && confirmDelete == false)
                {
                    if (dSLookupDTOListBS != null && dSLookupDTOListBS.Current != null && dSLookupDTOListBS.Current is DSLookupDTO)
                    {
                        DSLookupDTO dSLookupDTO = dSLookupDTOListBS.Current as DSLookupDTO;
                        if (dSLookupDTO != null)
                        {
                            if (dSLookupDTO.DSignageLookupValuesDTOList != null && dSLookupDTO.DSignageLookupValuesDTOList.Count > 0)
                            {
                                DSignageLookupValuesDTO dSignageLookupValuesDTO = null;
                                for (int i = dSLookupDTO.DSignageLookupValuesDTOList.Count - 1; i >= 0; i--)
                                {
                                    if (bool.Equals(dSLookupDTO.DSignageLookupValuesDTOList[i].IsActive, true))
                                    {
                                        dSignageLookupValuesDTO = dSLookupDTO.DSignageLookupValuesDTOList[i];
                                        break;
                                    }
                                }
                                if (dSignageLookupValuesDTO != null)
                                {
                                    if (dSignageLookupValuesDTO.DSLookupValueID < 0)
                                    {
                                        dSLookupDTO.DSignageLookupValuesDTOList.Remove(dSignageLookupValuesDTO);
                                        rowsDeleted = true;
                                    }
                                    else
                                    {
                                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                        {
                                            confirmDelete = true;
                                            refreshFromDb = true;
                                            dSignageLookupValuesDTO.IsActive = false;
                                            DSignageLookupValuesBL dSignageLookupValuesBL = new DSignageLookupValuesBL(machineUserContext, dSignageLookupValuesDTO);
                                            dSignageLookupValuesBL.Save();
                                        }
                                    }
                                }
                            }
                            if (rowsDeleted == false && confirmDelete == false)
                            {
                                if (dSLookupDTO.DSLookupID < 0)
                                {
                                    SortableBindingList<DSLookupDTO> dSLookupDTOSortableList = (SortableBindingList<DSLookupDTO>)dSLookupDTOListBS.DataSource;
                                    dSLookupDTOSortableList.Remove(dSLookupDTO);
                                    rowsDeleted = true;
                                }
                                else
                                {
                                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                    {
                                        confirmDelete = true;
                                        refreshFromDb = true;
                                        dSLookupDTO.IsActive = false;
                                        DSLookupBL dSLookupBL = new DSLookupBL(machineUserContext, dSLookupDTO);
                                        try
                                        {
                                            dSLookupBL.Save();
                                        }
                                        catch (ForeignKeyException ex)
                                        {
                                            log.Error(ex.Message);
                                            MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                        }
                                    }
                                }
                                if (rowsDeleted == true)
                                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                            }

                        }
                    }
                }
                if (refreshFromDb)
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

        private void dgvDSignageLookupValuesDTOList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                string backColor = string.Empty;
                string textColor = string.Empty;
                string fontname = string.Empty;
                if (IsColorColumn(e.ColumnIndex))
                {
                    ColorDialog cd = new ColorDialog();
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
                        textColor = converter.ConvertToString(cd.Color);
                        dgvDSignageLookupValuesDTOList.CurrentCell.Value = textColor;
                        dgvDSignageLookupValuesDTOList.CurrentCell = dgvDSignageLookupValuesDTOList[e.ColumnIndex + 1, e.RowIndex];
                    }
                }

                if (IsFontColumn(e.ColumnIndex))
                {
                    FontDialog fd = new FontDialog();
                    Font font = dgvDSignageLookupValuesDTOList.DefaultCellStyle.Font;
                    try
                    {
                        font = CustomFontConverter.ConvertStringToFont(utilities.ExecutionContext, dgvDSignageLookupValuesDTOList.CurrentCell.Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    fd.Font = font;
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                        fontname = converter.ConvertToString(fd.Font);
                        dgvDSignageLookupValuesDTOList.CurrentCell.Value = fontname;
                        dgvDSignageLookupValuesDTOList.CurrentCell = dgvDSignageLookupValuesDTOList[e.ColumnIndex + 2, e.RowIndex];
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private bool IsColorColumn(int index)
        {
            log.LogMethodEntry(index);
            bool returnValue = false;
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    if (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "BackColorTextBoxColumn"].Index == index)
                    {
                        returnValue = true;
                        break;
                    }
                    if (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "TextColorTextBoxColumn"].Index == index)
                    {
                        returnValue = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private bool IsFontColumn(int index)
        {
            log.LogMethodEntry(index);
            bool returnValue = false;
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    if (dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "FontTextBoxColumn"].Index == index)
                    {
                        returnValue = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private void UpdateTextStyle()
        {
            log.LogMethodEntry();
            if (dgvDSignageLookupValuesDTOList.Rows != null)
            {
                for (int i = 0; i < dgvDSignageLookupValuesDTOList.Rows.Count; i++)
                {
                    UpdateTextStyle(i);
                }
            }
            log.LogMethodExit();
        }

        private void UpdateTextStyle(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            string backColorString;
            string foreColorString;
            string fontString;
            Color backColor;
            Color foreColor;
            Font font;
            int columnIndex = 0;
            for (int i = 1; i <= 5; i++)
            {
                backColor = Color.White;
                foreColor = Color.Black;
                backColorString = String.Empty;
                foreColorString = String.Empty;
                fontString = String.Empty;
                try
                {
                    columnIndex = dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "BackColorTextBoxColumn"].Index;
                    if (dgvDSignageLookupValuesDTOList[columnIndex, rowIndex].Value != null)
                    {
                        backColorString = dgvDSignageLookupValuesDTOList[columnIndex, rowIndex].Value.ToString();
                    }
                    if (!string.IsNullOrEmpty(backColorString))
                    {
                        backColor = System.Drawing.ColorTranslator.FromHtml(backColorString);
                        dgvDSignageLookupValuesDTOList[dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListValue" + i + "TextBoxColumn"].Index, rowIndex].Style.BackColor = backColor;
                    }
                    columnIndex = dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "TextColorTextBoxColumn"].Index;
                    if (dgvDSignageLookupValuesDTOList[columnIndex, rowIndex].Value != null)
                    {
                        foreColorString = dgvDSignageLookupValuesDTOList[columnIndex, rowIndex].Value.ToString();
                    }
                    if (!string.IsNullOrEmpty(foreColorString))
                    {
                        foreColor = System.Drawing.ColorTranslator.FromHtml(foreColorString);
                        dgvDSignageLookupValuesDTOList[dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListValue" + i + "TextBoxColumn"].Index, rowIndex].Style.ForeColor = foreColor;
                    }

                    columnIndex = dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListVal" + i + "FontTextBoxColumn"].Index;
                    if (dgvDSignageLookupValuesDTOList[columnIndex, rowIndex].Value != null)
                    {
                        fontString = dgvDSignageLookupValuesDTOList[columnIndex, rowIndex].Value.ToString();
                    }
                    font = dgvDSignageLookupValuesDTOList.DefaultCellStyle.Font;
                    try
                    {
                        if (!string.IsNullOrEmpty(fontString))
                        {
                            font = CustomFontConverter.ConvertStringToFont(utilities.ExecutionContext, fontString);
                        }
                    }
                    catch (Exception)
                    {
                        font = dgvDSignageLookupValuesDTOList.DefaultCellStyle.Font;
                    }
                    dgvDSignageLookupValuesDTOList[dgvDSignageLookupValuesDTOList.Columns["dgvDSignageLookupValuesDTOListValue" + i + "TextBoxColumn"].Index, rowIndex].Style.Font = font;

                }
                catch (Exception)
                {
                }
            }
            log.LogMethodExit();
        }

        private void dgvDSignageLookupValuesDTOList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                UpdateTextStyle(e.RowIndex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void dgvDSLookupDTOList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                string dynamicFlag = string.Empty;
                if (e.ColumnIndex == dgvDSLookupDTOListQueryButtonColumn.Index || e.ColumnIndex == dgvDSLookupDTOListDynamicFlagCheckBoxColumn.Index)
                {
                    if (dgvDSLookupDTOList.CurrentRow.Cells["dgvDSLookupDTOListDSLookupNameTextBoxColumn"].Value == null ||
                        string.IsNullOrEmpty(dgvDSLookupDTOList.CurrentRow.Cells["dgvDSLookupDTOListDSLookupNameTextBoxColumn"].Value.ToString()))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1132));
                        dgvDSLookupDTOList.CancelEdit();
                        dgvDSLookupDTOList.RefreshEdit();
                        return;
                    }
                    if (e.ColumnIndex == dgvDSLookupDTOListQueryButtonColumn.Index)
                    {
                        if (dgvDSLookupDTOList[dgvDSLookupDTOListDynamicFlagCheckBoxColumn.Index, e.RowIndex].Value != null &&
                            string.Equals(dgvDSLookupDTOList[dgvDSLookupDTOListDynamicFlagCheckBoxColumn.Index, e.RowIndex].Value.ToString(), "Y"))
                        {
                            ShowQueryPopup(e);
                        }
                    }
                    else if (e.ColumnIndex == dgvDSLookupDTOListDynamicFlagCheckBoxColumn.Index)
                    {
                        dgvDSLookupDTOList.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        showPopup = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodEntry();
        }

        private void dgvDSLookupDTOList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (showPopup && e.ColumnIndex == dgvDSLookupDTOListDynamicFlagCheckBoxColumn.Index)
                {
                    showPopup = false;
                    dgvDSLookupDTOList[e.ColumnIndex + 1, e.RowIndex].Selected = true;
                    if (dgvDSLookupDTOList[e.ColumnIndex, e.RowIndex].Value != null &&
                        string.Equals(dgvDSLookupDTOList[e.ColumnIndex, e.RowIndex].Value.ToString(), "Y"))
                    {
                        ShowQueryPopup(e);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void ShowQueryPopup(DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            SortableBindingList<DSLookupDTO> dSLookupDTOSortableList = null;
            if (dSLookupDTOListBS != null && dSLookupDTOListBS.DataSource != null)
            {
                dSLookupDTOSortableList = dSLookupDTOListBS.DataSource as SortableBindingList<DSLookupDTO>;
                DSLookupDTO dSLookupDTO = null;
                if (dSLookupDTOSortableList != null)
                {
                    dSLookupDTO = dSLookupDTOSortableList[e.RowIndex];
                    if (dSLookupDTO != null)
                    {
                        QueryPopupUI queryPopupUI = new QueryPopupUI(utilities, dSLookupDTO.DSLookupID, dSLookupDTO.Query, dSLookupDTO.DSLookupName);
                        if (queryPopupUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            dSLookupDTO.Query = queryPopupUI.QueryString;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(dSLookupDTO.Query) || string.IsNullOrWhiteSpace(dSLookupDTO.Query))
                            {
                                dSLookupDTO.DynamicFlag = "N";
                            }
                        }
                        queryPopupUI.Close();
                        queryPopupUI.Dispose();
                        dgvDSLookupDTOList.Update();
                        dgvDSLookupDTOList.Refresh();
                    }
                }
            }
            log.LogMethodExit ();
        }

        private void dgvDSLookupDTOList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvDSLookupDTOListOffsetXTextBoxColumn.Index == e.ColumnIndex ||
                    dgvDSLookupDTOListOffsetYTextBoxColumn.Index == e.ColumnIndex ||
                    dgvDSLookupDTOListHDR12SpacingTextBoxColumn.Index == e.ColumnIndex ||
                    dgvDSLookupDTOListHDR23SpacingTextBoxColumn.Index == e.ColumnIndex ||
                    dgvDSLookupDTOListHDR34SpacingTextBoxColumn.Index == e.ColumnIndex ||
                    dgvDSLookupDTOListHDR45SpacingTextBoxColumn.Index == e.ColumnIndex ||
                    dgvDSLookupDTOListTextHeightTextBoxColumn.Index == e.ColumnIndex ||
                    dgvDSLookupDTOListTextWidthTextBoxColumn.Index == e.ColumnIndex)
                {
                    validateNonNegetiveInteger(e, dgvDSLookupDTOList.Columns[e.ColumnIndex].HeaderText);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void dgvDSignageLookupValuesDTOList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvDSignageLookupValuesDTOListBeforeSpacingRowsTextBoxColumn.Index == e.ColumnIndex)
                {
                    validateNonNegetiveInteger(e, dgvDSignageLookupValuesDTOList.Columns[e.ColumnIndex].HeaderText);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void validateNonNegetiveInteger(DataGridViewCellValidatingEventArgs e, string columnHeaderText)
        {
            log.LogMethodEntry(e, columnHeaderText);
            try
            {
                int i;
                if (!string.IsNullOrEmpty(Convert.ToString(e.FormattedValue)))
                {
                    if (!int.TryParse(Convert.ToString(e.FormattedValue), out i) || Convert.ToInt32(e.FormattedValue) < 0)
                    {
                        e.Cancel = true;
                        MessageBox.Show(utilities.MessageUtils.getMessage(1133).Replace("&1", columnHeaderText));
                    }
                    else
                    {
                        e.Cancel = false;
                    }
                }
                else
                {
                    e.Cancel = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void dgvDSignageLookupValuesDTOList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry ();
            try
            {
                UpdateTextStyle();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void lnkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvDSLookupDTOList.CurrentRow.Cells["dgvDSLookupDTOListDSLookupIDTextBoxColumn"].Value != null)
                {
                    int id = Convert.ToInt32(dgvDSLookupDTOList.CurrentRow.Cells["dgvDSLookupDTOListDSLookupIDTextBoxColumn"].Value);

                    if (id <= 0)
                        return;
                    PublishUI publishUI = new PublishUI(utilities, id, "DSLookup", dgvDSLookupDTOList.CurrentRow.Cells["dgvDSLookupDTOListDSLookupNameTextBoxColumn"].Value.ToString());
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
                dgvDSLookupDTOList.AllowUserToAddRows = true;
                dgvDSLookupDTOList.ReadOnly = false;
                dgvDSignageLookupValuesDTOList.AllowUserToAddRows = true;
                dgvDSignageLookupValuesDTOList.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvDSLookupDTOList.AllowUserToAddRows = false;
                dgvDSLookupDTOList.ReadOnly = true;
                dgvDSignageLookupValuesDTOList.AllowUserToAddRows = false;
                dgvDSignageLookupValuesDTOList.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
