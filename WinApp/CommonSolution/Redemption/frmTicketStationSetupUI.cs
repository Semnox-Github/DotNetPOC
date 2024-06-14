/********************************************************************************************
 * Project Name - frmTicketStationSetupUI
 * Description  - frmTicketStation Setup UI class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.2.0      16-Sept -2019    Girish Kundar       Created. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Reflection;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// This form is used to setup the physical ticket station  machines.
    /// </summary>
    public partial class frmTicketStationSetupUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SortableBindingList<TicketStationDTO> ticketStationDTOList;
        private const string POS_TICKET_STATION = "POS";
        private const string PHYSICAL_TICKET_STATION = "STATION";

        /// <summary>
        /// parameterized Constructor 
        /// </summary>
        /// <param name="utilities">utilities</param>
        public frmTicketStationSetupUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            SetGridCellStyle();
            LoadStationTypeList();
            LoadCheckBitAlgorithm();
            LoadTicketStation();
            log.LogMethodExit();
        }

        /// <summary>
        /// Sets the cell style for the grid view columns
        /// </summary>
        private void SetGridCellStyle()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvTicketStation);
            ThemeUtils.SetupVisuals(this);
            utilities.setLanguage(this);
            idDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            ticketStationIdDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewTextCellStyle();
            voucherLengthDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            ticketLengthDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            lastUpdatedByDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewTextCellStyle();
            lastUpdatedDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the Station types to the  stationTypeDataGridViewComboBoxColumn
        /// </summary>
        private void LoadStationTypeList()
        {
            log.LogMethodEntry();
            stationTypeDataGridViewComboBoxColumn.DataSource = GetTicketStationTypeDataSource();
            stationTypeDataGridViewComboBoxColumn.ValueMember = "Key";
            stationTypeDataGridViewComboBoxColumn.DisplayMember = "Value";
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the check bit algorithms to checkBitAlgorithmDataGridViewComboBox
        /// </summary>
        private void LoadCheckBitAlgorithm()
        {
            log.LogMethodEntry();
            checkBitAlgorithmDataGridViewComboBox.DataSource = GetCheckBitAlgorithmDataSource();
            checkBitAlgorithmDataGridViewComboBox.ValueMember = "Key";
            checkBitAlgorithmDataGridViewComboBox.DisplayMember = "Value";
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads all the Ticket station details to the Grid view. 
        /// </summary>
        private void LoadTicketStation()
        {
            log.LogMethodEntry();
            TicketStationListBL ticketStationList = new TicketStationListBL(utilities.ExecutionContext);
            List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> searchParameters = new List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>>();
            searchParameters.Add(new KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>(TicketStationDTO.SearchByTicketStationParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            List<TicketStationDTO> ticketStationDTOListForDisplay = ticketStationList.GetTicketStationDTOList(searchParameters);
            if (ticketStationDTOListForDisplay != null && ticketStationDTOListForDisplay.Any() == false)
            {
                ticketStationDTOListForDisplay.Add(new TicketStationDTO());
            }
            ticketStationDTOList = new SortableBindingList<TicketStationDTO>(ticketStationDTOListForDisplay);
            ticketStationListBS.DataSource = ticketStationDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Gets the Data Source for the TICKETSTATIONTYPE combo box.
        /// </summary>
        /// <returns>List<KeyValuePair<string, string>></returns>
        private List<KeyValuePair<TicketStationDTO.TICKETSTATIONTYPE, string>> GetTicketStationTypeDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<TicketStationDTO.TICKETSTATIONTYPE, string>> ticketStationTypeDataSource = new List<KeyValuePair<TicketStationDTO.TICKETSTATIONTYPE, string>>();
            ticketStationTypeDataSource.Add(new KeyValuePair<TicketStationDTO.TICKETSTATIONTYPE, string>(TicketStationDTO.TICKETSTATIONTYPE.POS_TICKET_STATION, POS_TICKET_STATION));
            ticketStationTypeDataSource.Add(new KeyValuePair<TicketStationDTO.TICKETSTATIONTYPE, string>(TicketStationDTO.TICKETSTATIONTYPE.PHYSICAL_TICKET_STATION, PHYSICAL_TICKET_STATION));
            log.LogMethodExit(ticketStationTypeDataSource);
            return ticketStationTypeDataSource;
        }

        /// <summary>
        ///  Gets the Data Source for the CheckBitAlgorithm combo box.
        /// </summary>
        /// <returns>List<KeyValuePair<string, string>></returns>
        private List<KeyValuePair<string, string>> GetCheckBitAlgorithmDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> checkBitAlgorithmDataSource = new List<KeyValuePair<string, string>>();
            checkBitAlgorithmDataSource.Add(new KeyValuePair<string, string>(TicketStationAlgorithm.DEFAULT, "None"));
            checkBitAlgorithmDataSource.Add(new KeyValuePair<string, string>(TicketStationAlgorithm.MODULO_TEN_WEIGHT_THREE, "Modulo Ten Weight Three"));
            log.LogMethodExit(checkBitAlgorithmDataSource);
            return checkBitAlgorithmDataSource;
        }

        /// <summary>
        /// Shows the validation errors
        /// </summary>
        /// <param name="validationErrorList"></param>
        public void ShowValidationError(List<ValidationError> validationErrorList)
        {
            log.LogMethodEntry(validationErrorList);
            foreach (var validationError in validationErrorList)
            {
                log.Error(validationError.Message);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, validationError.Message), MessageContainerList.GetMessage(utilities.ExecutionContext, "Validation Error"));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Ticket station details to the table.
        /// Checks for the duplicate station Id .
        /// Validates the ticket station setup information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                log.LogMethodEntry(sender, e);
                bool recordUpdated = false;
                dgvTicketStation.EndEdit();
                ticketStationDTOList = (SortableBindingList<TicketStationDTO>)ticketStationListBS.DataSource;

                if (ticketStationDTOList != null && ticketStationDTOList.Count > 0)
                {
                    foreach (TicketStationDTO ticketStationDTO in ticketStationDTOList)
                    {
                        if (ticketStationDTO.IsChanged)
                        {
                            if (!Validate(ticketStationDTO))
                            {
                                dgvTicketStation.RefreshEdit();
                                return;
                            }
                            else
                            {
                                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                                {
                                    try
                                    {
                                        parafaitDBTrx.BeginTransaction();
                                        TicketStationBL ticketStationBL = new TicketStationBL(utilities.ExecutionContext, ticketStationDTO);
                                        ticketStationBL.Save(parafaitDBTrx.SQLTrx);
                                        recordUpdated = true;
                                        parafaitDBTrx.EndTransaction();
                                    }
                                    catch (ValidationException ex)
                                    {
                                        ShowValidationError(ex.ValidationErrorList);
                                        parafaitDBTrx.RollBack();
                                        recordUpdated = false;
                                        log.Error("ValidationException :", ex);
                                    }
                                    catch (Exception ex)
                                    {
                                        parafaitDBTrx.RollBack();
                                        recordUpdated = false;
                                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Error") + " " + ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Error"));
                                        return;
                                    }
                                }
                            }
                        }

                    }
                    if (recordUpdated)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 122));
                        LoadTicketStation();
                    }
                    
                }
                else
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 371));
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Error") + " " + ex.Message, "Save Error");
                log.Error("Error in save. Exception:" + ex.ToString());
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the  ticketStationDTO for empty or null, numeric , alphanumeric , length validations
        /// </summary>
        /// <param name="ticketStationDTO">ticketStationDTO</param>
        /// <returns> true /false</returns>
        private bool Validate(TicketStationDTO ticketStationDTO)
        {
            log.LogMethodEntry(ticketStationDTO);
            if (string.IsNullOrEmpty(ticketStationDTO.TicketStationId) &&
                ticketStationDTO.VoucherLength.ToString() == null &&
                ticketStationDTO.TicketLength.ToString() == null)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 371), MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Data"));
                return false;
            }
            try
            {
                TicketStationBL ticketStationBL = new TicketStationBL(utilities.ExecutionContext, ticketStationDTO);
                List<ValidationError> validationErrorList = ticketStationBL.Validate();
                if (validationErrorList.Count > 0)
                {
                    throw new ValidationException("Validation Failed", validationErrorList);
                }
            }
            catch (ValidationException ex)
            {
                ShowValidationError(ex.ValidationErrorList);
                log.Error("ValidationException :", ex);
                return false;
            }

            if (string.IsNullOrEmpty(ticketStationDTO.TicketStationId))
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, MessageContainerList.GetMessage(utilities.ExecutionContext, "Ticket Station Id")), MessageContainerList.GetMessage(utilities.ExecutionContext, "Validation Error"));
                return false;
            }
            if (string.IsNullOrEmpty(ticketStationDTO.TicketStationId) == false && 
                ticketStationDTO.IsChanged && ticketStationDTO.TicketStationId.Length > 2)
            {
                if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, "Station Id length more than 2 characters!") + ". " + MessageContainerList.GetMessage(utilities.ExecutionContext, 1695), MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Data"), MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
            }
            if (ticketStationDTO.IsChanged && (ticketStationDTO.TicketLength < 3 || 
                ticketStationDTO.TicketLength > 6))
            {
                if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, "Ticket length should be between 3 and 6!") + ". " + MessageContainerList.GetMessage(utilities.ExecutionContext, 1695), MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Data"), MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
            }
            if (ticketStationDTO.IsChanged && (ticketStationDTO.VoucherLength < 12 || 
                ticketStationDTO.VoucherLength > 16))
            {
                if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, "Voucher length should be between 12 and 16!") + ". " + MessageContainerList.GetMessage(utilities.ExecutionContext, 1695), MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Data"), MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
            }          
            return true;
        }

        /// <summary>
        /// Finds whether any DTO is changes if yes returns true else returns false
        /// This method is called when user clicks refresh or Close buttons.
        /// </summary>
        /// <returns> true /false</returns>
        private bool HasChanges()
        {
            log.LogMethodEntry();
            dgvTicketStation.EndEdit();
            bool dataChanged = false;
            ticketStationDTOList = (SortableBindingList<TicketStationDTO>)ticketStationListBS.DataSource;
            if (ticketStationDTOList != null && ticketStationDTOList.Count > 0)
            {
                dataChanged = ticketStationDTOList.ToList().Exists(ts => ts.IsChanged == true);
            }
            log.LogMethodExit(dataChanged);
            return dataChanged;
        }

        /// <summary>
        /// This method closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            if (HasChanges())
            {
                if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 127), MessageContainerList.GetMessage(utilities.ExecutionContext, "Unsaved Data"), MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    log.LogMethodExit();
                    return;
                }
            }
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// This method refreshes the Grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (HasChanges())
            {
                if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 714), MessageContainerList.GetMessage(utilities.ExecutionContext, "Unsaved Data"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    log.LogMethodExit();
                    return;
                }
            }
            LoadTicketStation();
            log.LogMethodExit();
        }


        /// <summary>
        /// This method is used to Inactivate the Ticket station records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (dgvTicketStation.SelectedRows.Count <= 0 && this.dgvTicketStation.SelectedCells.Count <= 0)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 959));
                log.Debug("Ends-btnDelete_Click event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                return;
            }
            bool confirmDelete = false;
            if (this.dgvTicketStation.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.dgvTicketStation.SelectedCells)
                {
                    dgvTicketStation.Rows[cell.RowIndex].Selected = true;
                }
            }
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrx.BeginTransaction();
                    foreach (DataGridViewRow dgvSelectedRow in this.dgvTicketStation.SelectedRows)
                    {
                        if (dgvSelectedRow.Cells[0].Value == null)
                        {
                            return;
                        }
                        if (Convert.ToInt32(dgvSelectedRow.Cells[0].Value.ToString()) <= 0)
                        {
                            dgvTicketStation.Rows.RemoveAt(dgvSelectedRow.Index);
                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 957));
                            return;
                        }
                        else
                        {
                            if (confirmDelete || (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 958), "Confirm InActvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                BindingSource ticketStationDTOListBS = (BindingSource)dgvTicketStation.DataSource;
                                var ticketStationDTOList = (SortableBindingList<TicketStationDTO>)ticketStationDTOListBS.DataSource;
                                TicketStationDTO ticketStationDTO = ticketStationDTOList[dgvSelectedRow.Index];
                                ticketStationDTO.IsActive = false;
                                TicketStationBL ticketStationBL = new TicketStationBL(utilities.ExecutionContext, ticketStationDTO);
                                ticketStationBL.Save(parafaitDBTrx.SQLTrx);
                            }
                        }
                    }

                    parafaitDBTrx.EndTransaction();
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    MessageBox.Show(utilities.MessageUtils.getMessage("Save Error") + " " + ex.Message, utilities.MessageUtils.getMessage("Save Error"));
                    return;
                }
            }
            LoadTicketStation();
            log.LogMethodExit();
        }

        /// <summary>
        /// This method sets the initial values to the grid columns. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTicketStation_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            e.Row.Cells["isActiveDataGridViewCheckBoxColumn"].Value = true;
            e.Row.Cells["ticketLengthDataGridViewTextBoxColumn"].Value = "0";
            e.Row.Cells["voucherLengthDataGridViewTextBoxColumn"].Value = "0";
            e.Row.Cells["checkDigitDataGridViewCheckBoxColumn"].Value = true;
            e.Row.Cells["checkBitAlgorithmDataGridViewComboBox"].Value = TicketStationAlgorithm.DEFAULT;
            log.LogMethodExit();
        }


        /// <summary>
        /// This method handles the data error in the grid view 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTicketStation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if (e.Exception != null)
            {
                MessageBox.Show((MessageContainerList.GetMessage(utilities.ExecutionContext, utilities.MessageUtils.getMessage(585, Name, e.RowIndex + 1, "-" + dgvTicketStation.Columns[e.ColumnIndex].DataPropertyName))), MessageContainerList.GetMessage(utilities.ExecutionContext, 10830));
            }
            e.Cancel = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method handles the Adding New ticket station record .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ticketStationListBS_AddingNew(object sender, System.ComponentModel.AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            TicketStationDTO ticketStationDTO = new TicketStationDTO();
            ticketStationDTO.LastUpdatedDate = DateTime.Now;
            ticketStationDTO.TicketStationType = TicketStationDTO.TICKETSTATIONTYPE.PHYSICAL_TICKET_STATION;
            e.NewObject = ticketStationDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// This method used to make the First row is readonly and Ticket station Type column readonly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTicketStation_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvTicketStation.Rows.Count > 1)
            {
                foreach (DataGridViewRow row in dgvTicketStation.Rows)
                {
                    if (row.Cells["stationTypeDataGridViewComboBoxColumn"].Value != null && row.Cells["stationTypeDataGridViewComboBoxColumn"].Value.ToString() == TicketStationDTO.TICKETSTATIONTYPE.POS_TICKET_STATION.ToString())
                    {
                        row.ReadOnly = true;
                    }
                }
                dgvTicketStation.Columns["stationTypeDataGridViewComboBoxColumn"].ReadOnly = true;

            }
            log.LogMethodExit();
        }
    }
}
