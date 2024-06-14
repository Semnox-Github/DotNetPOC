/********************************************************************************************
 * Project Name - Digital Signage
 * Description  - TickerListUI
 * 
 **************
 **Version Log
 **************
 *Version       Date             Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha          Added logger methods.
 *2.70.3        11-Feb-2020      Deeksha          Invariant culture-Font Issue Fix
 *2.80.0        17-Feb-2019      Deeksha          Modified to Make DigitalSignage module as
 *                                                read only in Windows Management Studio.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Publish;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Parafait.DigitalSignage
{
    public partial class TickerListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// Constructor of TickerListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public TickerListUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvTickerDTOList);
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

        private void TickerListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvTickerDTOList.CellValueChanged += dgvTickerDTOList_CellValueChanged;
            tickerTextDataGridViewTextBoxColumn.MaxInputLength = 100;
            nameDataGridViewTextBoxColumn.MaxInputLength = 100;
            LoadScrollDirectionType();
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadTickerDTOList();
            UpdateTickerTextStyle();
            log.LogMethodExit();
        }

        private void LoadTickerDTOList()
        {
            log.LogMethodEntry();
            TickerListBL tickerListBL = new TickerListBL(machineUserContext);
            List<KeyValuePair<TickerDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TickerDTO.SearchByParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                searchParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            searchParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.NAME, txtName.Text));
            searchParameters.Add(new KeyValuePair<TickerDTO.SearchByParameters, string>(TickerDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<TickerDTO> tickerDTOList = tickerListBL.GetTickerDTOList(searchParameters);
            SortableBindingList<TickerDTO> tickerDTOSortableList;
            if(tickerDTOList != null)
            {
                tickerDTOSortableList = new SortableBindingList<TickerDTO>(tickerDTOList);
            }
            else
            {
                tickerDTOSortableList = new SortableBindingList<TickerDTO>();
            }
            tickerDTOListBS.DataSource = tickerDTOSortableList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the scroll directions to the comboboxes
        /// </summary>
        private void LoadScrollDirectionType()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INDENTATION"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> scrollDirectionLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if(scrollDirectionLookUpValueList == null)
                {
                    scrollDirectionLookUpValueList = new List<LookupValuesDTO>();
                }
                scrollDirectionLookUpValueList.Insert(0, new LookupValuesDTO());
                scrollDirectionLookUpValueList[0].LookupValueId = -1;
                scrollDirectionLookUpValueList[0].Description = "<SELECT>";
                scrollDirectionDataGridViewComboBoxColumn.DataSource = scrollDirectionLookUpValueList;
                scrollDirectionDataGridViewComboBoxColumn.ValueMember = "LookupValueId";
                scrollDirectionDataGridViewComboBoxColumn.DisplayMember = "Description";

                log.LogMethodExit();
            }
            catch(Exception e)
            {
                log.Error("Ends-LoadScrollDirectionType() Method with an Exception:", e);
            }
        }

        private void UpdateTickerTextStyle(int rowIndex, TickerDTO tickerDTO = null)
        {
            log.LogMethodEntry(rowIndex, tickerDTO);
            if(tickerDTO == null)
            {
                tickerDTO = ((SortableBindingList<TickerDTO>)tickerDTOListBS.DataSource)[rowIndex];
            }
            Color color = Color.Black;
            try
            {
                color = System.Drawing.ColorTranslator.FromHtml(tickerDTO.TextColor);
            }
            catch(Exception)
            {
            }
            dgvTickerDTOList.Rows[rowIndex].Cells[tickerTextDataGridViewTextBoxColumn.Index].Style.ForeColor = color;
            color = Color.White;
            try
            {
                color = System.Drawing.ColorTranslator.FromHtml(tickerDTO.BackColor);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            dgvTickerDTOList.Rows[rowIndex].Cells[tickerTextDataGridViewTextBoxColumn.Index].Style.BackColor = color;
            Font font;
            try
            {
                font = CustomFontConverter.ConvertStringToFont(utilities.ExecutionContext, tickerDTO.Font);

            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                font = dgvTickerDTOList.DefaultCellStyle.Font;
            }
            dgvTickerDTOList.Rows[rowIndex].Cells[tickerTextDataGridViewTextBoxColumn.Index].Style.Font = font;
            log.LogMethodExit();
        }

        private void UpdateTickerTextStyle()
        {
            log.LogMethodEntry();
            try
            {
                DataGridViewRowCollection rows = dgvTickerDTOList.Rows;
                SortableBindingList<TickerDTO> tickerDTOSortableList = (SortableBindingList<TickerDTO>)tickerDTOListBS.DataSource;
                if(tickerDTOSortableList != null && rows != null)
                {
                    for(int i = 0; i < rows.Count; i++)
                    {
                        UpdateTickerTextStyle(i, tickerDTOSortableList[i]);
                    }
                }
                log.LogMethodExit();
            }
            catch(Exception e)
            {
                log.Error("Ends-UpdateTickerTextStyle() Method with an Exception:", e);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            RefreshData();
            log.LogMethodExit();
        }

        private void dgvTickerDTOList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                string backColor = string.Empty;
                string textColor = string.Empty;
                string fontname = string.Empty;
                if (e.ColumnIndex == textColorDataGridViewTextBoxColumn.Index || e.ColumnIndex == backColorDataGridViewTextBoxColumn.Index)
                {
                    ColorDialog cd = new ColorDialog();
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
                        textColor = converter.ConvertToString(cd.Color);
                        dgvTickerDTOList.CurrentCell.Value = textColor;
                        dgvTickerDTOList.CurrentCell = dgvTickerDTOList[e.ColumnIndex + 1, e.RowIndex];
                    }
                }

                if (e.ColumnIndex == fontDataGridViewTextBoxColumn.Index)
                {
                    FontDialog fd = new FontDialog();
                    Font font = dgvTickerDTOList.DefaultCellStyle.Font;
                    try
                    {
                        font = CustomFontConverter.ConvertStringToFont(utilities.ExecutionContext, dgvTickerDTOList.CurrentCell.Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                    fd.Font = font;
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                        fontname = converter.ConvertToString(fd.Font);
                        dgvTickerDTOList.CurrentCell.Value = fontname;
                        dgvTickerDTOList.CurrentCell = dgvTickerDTOList[e.ColumnIndex + 1, e.RowIndex];
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvTickerDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvTickerDTOList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(); ;
            this.Close();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvTickerDTOList.EndEdit();
                SortableBindingList<TickerDTO> tickerDTOSortableList = (SortableBindingList<TickerDTO>)tickerDTOListBS.DataSource;
                string message;
                TickerBL tickerBL;
                bool error = false;
                if (tickerDTOSortableList != null)
                {
                    for (int i = 0; i < tickerDTOSortableList.Count; i++)
                    {
                        if (tickerDTOSortableList[i].IsChanged)
                        {
                            message = ValidateTickerDTO(tickerDTOSortableList[i]);
                            if (string.IsNullOrEmpty(message))
                            {
                                try
                                {
                                    tickerBL = new TickerBL(machineUserContext, tickerDTOSortableList[i]);
                                    tickerBL.Save();
                                }
                                catch (ForeignKeyException ex)
                                {
                                    log.Error(ex.Message);
                                    dgvTickerDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                    break;
                                }
                                catch (Exception)
                                {
                                    error = true;
                                    log.Error("Error while saving ticker.");
                                    dgvTickerDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                    break;
                                }
                            }
                            else
                            {
                                error = true;
                                dgvTickerDTOList.Rows[i].Selected = true;
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
                    dgvTickerDTOList.Update();
                    dgvTickerDTOList.Refresh();
                }
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
                if (dgvTickerDTOList.SelectedRows.Count <= 0 && dgvTickerDTOList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                bool refreshFromDB = false;
                if (this.dgvTickerDTOList.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvTickerDTOList.SelectedCells)
                    {
                        dgvTickerDTOList.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow row in dgvTickerDTOList.SelectedRows)
                {
                    if (Convert.ToInt32(row.Cells[0].Value.ToString()) < 0)
                    {
                        dgvTickerDTOList.Rows.RemoveAt(row.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            refreshFromDB = true;
                            SortableBindingList<TickerDTO> tickerDTOSortableList = (SortableBindingList<TickerDTO>)tickerDTOListBS.DataSource;
                            TickerDTO tickerDTO = tickerDTOSortableList[row.Index];
                            tickerDTO.IsActive = false;
                            TickerBL tickerBL = new TickerBL(machineUserContext, tickerDTO);
                            try
                            {
                                tickerBL.Save();
                            }
                            catch (ForeignKeyException ex)
                            {
                                log.Error(ex.Message);
                                dgvTickerDTOList.Rows[row.Index].Selected = true;
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

        private string ValidateTickerDTO(TickerDTO tickerDTO)
        {
            log.LogMethodEntry(tickerDTO);
            string message = string.Empty;
            if(string.IsNullOrEmpty(tickerDTO.Name) || string.IsNullOrWhiteSpace(tickerDTO.Name))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", nameDataGridViewTextBoxColumn.HeaderText);
            }
            if(string.IsNullOrEmpty(tickerDTO.TickerText) || string.IsNullOrWhiteSpace(tickerDTO.TickerText))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", tickerTextDataGridViewTextBoxColumn.HeaderText);
            }
            if(!string.IsNullOrEmpty(tickerDTO.TextColor) && !string.IsNullOrWhiteSpace(tickerDTO.TextColor))
            {
                if(!ValidateColor(tickerDTO.TextColor))
                {
                    message = utilities.MessageUtils.getMessage(1144);
                    message = message.Replace("&1", textColorDataGridViewTextBoxColumn.HeaderText);
                }
            }
            if(!string.IsNullOrEmpty(tickerDTO.BackColor) && !string.IsNullOrWhiteSpace(tickerDTO.BackColor))
            {
                if(!ValidateColor(tickerDTO.BackColor))
                {
                    message = utilities.MessageUtils.getMessage(1144);
                    message = message.Replace("&1", backColorDataGridViewTextBoxColumn.HeaderText);
                }
            }
            if(!string.IsNullOrEmpty(tickerDTO.Font) && !string.IsNullOrWhiteSpace(tickerDTO.Font))
            {
                try
                {
                    TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));
                    Font font = CustomFontConverter.ConvertStringToFont(utilities.ExecutionContext, tickerDTO.Font);
                    string fontText = fontConverter.ConvertToInvariantString(font);
                    if(fontText != tickerDTO.Font)
                    {
                        message = utilities.MessageUtils.getMessage(1144);
                        message = message.Replace("&1", fontDataGridViewTextBoxColumn.HeaderText);
                    }
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    message = utilities.MessageUtils.getMessage(1144);
                    message = message.Replace("&1", fontDataGridViewTextBoxColumn.HeaderText);
                }
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
            catch(Exception ex)
            {
                log.Error(ex);
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private void dgvTickerDTOList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if(textColorDataGridViewTextBoxColumn != null &&
                backColorDataGridViewTextBoxColumn != null &&
                fontDataGridViewTextBoxColumn != null)
            {
                if(e.ColumnIndex == textColorDataGridViewTextBoxColumn.Index ||
                e.ColumnIndex == backColorDataGridViewTextBoxColumn.Index ||
                e.ColumnIndex == fontDataGridViewTextBoxColumn.Index)
                {
                    UpdateTickerTextStyle(e.RowIndex);
                }
            }
            log.LogMethodExit();
        }

        private void dgvTickerDTOList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry();
            UpdateTickerTextStyle();
            log.LogMethodExit();
        }

        private void lnkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvTickerDTOList.CurrentRow.Cells["tickerIdDataGridViewTextBoxColumn"].Value != null)
                {
                    int id = Convert.ToInt32(dgvTickerDTOList.CurrentRow.Cells["tickerIdDataGridViewTextBoxColumn"].Value);

                    if (id <= 0)
                        return;
                    PublishUI publishUI = new PublishUI(utilities, id, "Ticker", dgvTickerDTOList.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString());
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
                dgvTickerDTOList.AllowUserToAddRows = true;
                dgvTickerDTOList.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvTickerDTOList.AllowUserToAddRows = false;
                dgvTickerDTOList.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
