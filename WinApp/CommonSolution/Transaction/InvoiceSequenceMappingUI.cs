/********************************************************************************************
 * Class Name -  Transaction                                                                         
 * Description - Invoice Sequence mapping UI 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Used for Mapping Sequence with Invoice Sequence
    /// </summary>
    public partial class InvoiceSequenceMappingUI : Form
    {
        Utilities utilities;
        int sequenceId;
        private static readonly  Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        SortableBindingList<InvoiceSequenceMappingDTO> invoiceSequenceMappingSortableList;
        Dictionary<int, LookupValuesDTO> lookupValuesDictionary;
        Dictionary<int, InvoiceSequenceSetupDTO> invoiceSequenceSetupDTODictionary;
        int SelectedVal;

        /// <summary>
        /// Constructor of InvoiceSequenceMappingUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="sequenceId"></param>
        public InvoiceSequenceMappingUI(Utilities utilities, int sequenceId)
        {
            log.LogMethodEntry(utilities, sequenceId);
            InitializeComponent();
            this.utilities = utilities;
            this.sequenceId = sequenceId;
            utilities.setupDataGridProperties(ref dgvInvoiceSequenceMapping);
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
            log.LogMethodExit();
        }

        private void InvoiceSequenceMappingUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (utilities.ParafaitEnv.Manager_Flag.Equals("Y") || utilities.ParafaitEnv.LoginID.Equals("semnox"))
            {
                btnDelete.Enabled = true;
                btnSave.Enabled = true;
                dgvInvoiceSequenceMapping.ReadOnly = false;
            }
            else
            {
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                dgvInvoiceSequenceMapping.ReadOnly = true;
            }
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            lookupValuesDictionary = lookupValuesList.GetLookupValuesMap("INVOICE_TYPE");
            LoadInvoiceSequenceSetupList();
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadInvoiceSequenceMappingDTOList();
            log.LogMethodExit();
        }

        private void LoadInvoiceSequenceSetupList()
        {
            log.LogMethodEntry();
            InvoiceSequenceSetupListBL invoiceSequenceSetupListBL = new InvoiceSequenceSetupListBL(machineUserContext);
            List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.ISACTIVE, "1"));
            }
            searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOList = invoiceSequenceSetupListBL.GetAllInvoiceSequenceSetupList(searchParameters);
            invoiceSequenceSetupDTODictionary = new Dictionary<int, InvoiceSequenceSetupDTO>();
            if (invoiceSequenceSetupDTOList == null)
            {
                invoiceSequenceSetupDTOList = new List<InvoiceSequenceSetupDTO>();
            }
            else
            {
                foreach (var item in invoiceSequenceSetupDTOList)
                {
                    invoiceSequenceSetupDTODictionary.Add(item.InvoiceSequenceSetupId, item);
                }
            }

            List<KeyValuePair<int, string>> invoiceSequenceSetupList = new List<KeyValuePair<int, string>>();
            {
                invoiceSequenceSetupList.Insert(0, new KeyValuePair<int, string>(-1, "<SELECT>"));
                foreach (var item in invoiceSequenceSetupDTOList)
                {
                    invoiceSequenceSetupList.Add(new KeyValuePair<int, string>(item.InvoiceSequenceSetupId, item.Prefix + "(" + item.SeriesStartNumber + "-" + item.SeriesEndNumber + ")"));
                }
            }

            BindingSource bs = new BindingSource();
            bs.DataSource = invoiceSequenceSetupList;
            invoiceSequenceSetupIdDataGridViewTextBoxColumn.DataSource = bs;
            invoiceSequenceSetupIdDataGridViewTextBoxColumn.ValueMember = "Key";
            invoiceSequenceSetupIdDataGridViewTextBoxColumn.DisplayMember = "Value";
            log.LogMethodExit();
        }

        private void LoadInvoiceSequenceMappingDTOList()
        {
            log.LogMethodEntry();
            try
            {
                InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(machineUserContext);
                List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.ISACTIVE, (chbShowActiveEntries.Checked) ? "1" : "0"));
                searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.SEQUENCE_ID, sequenceId.ToString()));
                List<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOList = invoiceSequenceMappingListBL.GetAllInvoiceSequenceMappingList(searchParameters);

                if (invoiceSequenceMappingDTOList != null)
                {
                    invoiceSequenceMappingSortableList = new SortableBindingList<InvoiceSequenceMappingDTO>(invoiceSequenceMappingDTOList);
                }
                else
                {
                    invoiceSequenceMappingSortableList = new SortableBindingList<InvoiceSequenceMappingDTO>();
                }
                invoiceSequenceMappingDTOListBS.DataSource = invoiceSequenceMappingSortableList;

                log.LogMethodExit(); ;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvInvoiceSequenceMapping.EndEdit();
            SortableBindingList<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOSortableList = (SortableBindingList<InvoiceSequenceMappingDTO>)invoiceSequenceMappingDTOListBS.DataSource;
            string message;
            InvoiceSequenceMappingBL invoiceSequenceMappingBL;

            if (invoiceSequenceMappingDTOSortableList != null)
            {
                for (int i = 0; i < invoiceSequenceMappingDTOSortableList.Count; i++)
                {
                    if (invoiceSequenceMappingDTOSortableList[i].IsChanged)
                    {
                        message = ValidateInvoiceSequenceMappingDTO(invoiceSequenceMappingSortableList[i]);
                        if (string.IsNullOrEmpty(message))
                        {
                            try
                            {
                                if (utilities.getParafaitDefaults("USE_ORIGINAL_TRXNO_FOR_REFUND") == "Y" && dgvInvoiceSequenceMapping[InvoiceType.Index, i].Value != null &&
                                    dgvInvoiceSequenceMapping[InvoiceType.Index, i].Value.ToString() == "CREDIT")
                                {
                                    message = utilities.MessageUtils.getMessage(1329);
                                    MessageBox.Show(message);
                                    break;
                                }

                                invoiceSequenceMappingSortableList[i].SequenceId = sequenceId;
                                invoiceSequenceMappingBL = new InvoiceSequenceMappingBL(machineUserContext, invoiceSequenceMappingDTOSortableList[i]);
                                invoiceSequenceMappingBL.Save();
                                if (invoiceSequenceMappingDTOSortableList[i].Id > -1)
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1414));
                                }
                            }
                            catch (InvalidMappingException ex)
                            {
                                log.Error(ex.Message);
                                message = utilities.MessageUtils.getMessage(1330);
                                MessageBox.Show(message);
                                break;
                            }
                            catch (InvalidInvoiceSequenceException ex)
                            {
                                log.Error(ex.Message);
                                message = utilities.MessageUtils.getMessage(1331);
                                MessageBox.Show(message);
                                break;
                            }
                            catch (SeriesExpiredException ex)
                            {
                                log.Error(ex.Message);
                                message = utilities.MessageUtils.getMessage(1332);
                                MessageBox.Show(message);
                                break;
                            }
                            catch (Exception)
                            {
                                log.Error("Error while saving event.");
                                dgvInvoiceSequenceMapping.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                break;
                            }

                        }
                        else
                        {
                            dgvInvoiceSequenceMapping.Rows[i].Selected = true;
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
            RefreshData();
            log.LogMethodExit();
        }

        private string ValidateInvoiceSequenceMappingDTO(InvoiceSequenceMappingDTO invoiceSequenceMappingDTO)
        {
            log.LogMethodEntry(invoiceSequenceMappingDTO);
            string message = string.Empty;
            if (((invoiceSequenceMappingDTO.EffectiveDate == null))
                && invoiceSequenceMappingDTO.Id == -1)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", effectiveDateDataGridViewTextBoxColumn.HeaderText);
            }
            if (invoiceSequenceMappingDTO.InvoiceSequenceSetupId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", invoiceSequenceSetupIdDataGridViewTextBoxColumn.HeaderText);
            }
            log.LogMethodExit(message);
            return message;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            chbShowActiveEntries.Checked = true;
            RefreshData();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (dgvInvoiceSequenceMapping.SelectedRows.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDB = false;
            if (this.dgvInvoiceSequenceMapping.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvInvoiceSequenceMapping.SelectedCells)
                {
                    dgvInvoiceSequenceMapping.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow row in dgvInvoiceSequenceMapping.SelectedRows)
            {
                if (Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                {
                    dgvInvoiceSequenceMapping.Rows.RemoveAt(row.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        SortableBindingList<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOSortableList = (SortableBindingList<InvoiceSequenceMappingDTO>)invoiceSequenceMappingDTOListBS.DataSource;
                        InvoiceSequenceMappingDTO invoiceSequenceMappingDTO = invoiceSequenceMappingDTOSortableList[row.Index];
                        invoiceSequenceMappingDTO.IsActive = false;
                        InvoiceSequenceMappingBL invoiceSequenceMappingBL = new InvoiceSequenceMappingBL(machineUserContext, invoiceSequenceMappingDTO);
                        confirmDelete = true;
                        refreshFromDB = true;
                        try
                        {
                            invoiceSequenceMappingBL.Save();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            dgvInvoiceSequenceMapping.Rows[row.Index].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(1083));
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
                RefreshData();
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
            LoadInvoiceSequenceSetupList();
            LoadInvoiceSequenceMappingDTOList();
            log.LogMethodExit();
        }

        private void dgvInvoiceSequenceMapping_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == invoiceSequenceSetupIdDataGridViewTextBoxColumn.Index && e.RowIndex >= 0)
            {
                SelectedVal = Convert.ToInt32((dgvInvoiceSequenceMapping[e.ColumnIndex, e.RowIndex].Value));
                PopulateInvoiceSequenceDetails(e.RowIndex, SelectedVal);
            }
            log.LogMethodExit();
        }

        private void PopulateInvoiceSequenceDetails(int rowIndex, int SelectedVal)
        {
            log.LogMethodEntry(rowIndex, SelectedVal);
            if (invoiceSequenceSetupDTODictionary != null && invoiceSequenceSetupDTODictionary.ContainsKey(SelectedVal))
            {
                if (lookupValuesDictionary != null && lookupValuesDictionary.ContainsKey(invoiceSequenceSetupDTODictionary[SelectedVal].InvoiceTypeId))
                {
                    dgvInvoiceSequenceMapping[InvoiceType.Index, rowIndex].Value = lookupValuesDictionary[invoiceSequenceSetupDTODictionary[SelectedVal].InvoiceTypeId].LookupValue;
                }
                dgvInvoiceSequenceMapping[ExpiryDate.Index, rowIndex].Value = invoiceSequenceSetupDTODictionary[SelectedVal].ExpiryDate;
                dgvInvoiceSequenceMapping[SeriesSatrtNumber.Index, rowIndex].Value = invoiceSequenceSetupDTODictionary[SelectedVal].SeriesStartNumber;
                dgvInvoiceSequenceMapping[SeriesEndNumber.Index, rowIndex].Value = invoiceSequenceSetupDTODictionary[SelectedVal].SeriesEndNumber;
                dgvInvoiceSequenceMapping[CurrentValue.Index, rowIndex].Value = invoiceSequenceSetupDTODictionary[SelectedVal].CurrentValue;
            }
            log.LogMethodExit();
        }

        private void dgvInvoiceSequenceMapping_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry();
            if (invoiceSequenceMappingSortableList != null)
            {
                for (int i = 0; i < invoiceSequenceMappingSortableList.Count; i++)
                {
                    PopulateInvoiceSequenceDetails(i, invoiceSequenceMappingSortableList[i].InvoiceSequenceSetupId);
                }
            }
            log.LogMethodExit();
        }

        private void dgvInvoiceSequenceMapping_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvInvoiceSequenceMapping.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

    }
}
