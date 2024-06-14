/********************************************************************************************
 * Class Name -  Transaction                                                                         
 * Description - Invoice Sequence SetUp UI 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Used for storing the details of the Invoice Sequences
    /// </summary>
    public partial class InvoiceSequenceSetupUI : Form
    {
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<LookupValuesDTO> invoiceTypeLookUpValueList;
        SortableBindingList<InvoiceSequenceSetupDTO> invoiceSequenceSetupSortableList;

        /// <summary>
        /// Constructor of InvoiceSequenceSetupUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public InvoiceSequenceSetupUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvInvoiceSequenceSetupDTOList);
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

        private void InvoiceSequenceSetupUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (utilities.ParafaitEnv.Manager_Flag.Equals("Y") || utilities.ParafaitEnv.LoginID.Equals("semnox"))
            {
                btnDelete.Enabled = true;
                btnSave.Enabled = true;
                txtSysAuthorization.Enabled = true;
                dgvInvoiceSequenceSetupDTOList.ReadOnly = false;
            }
            else
            {
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                txtSysAuthorization.Enabled = false;
                dgvInvoiceSequenceSetupDTOList.ReadOnly = true;
            }
            RefreshData();
            log.LogMethodExit();
        }

        private void LoadSystemResolutionNumber()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SYSTEM_AUTHORIZATION"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            invoiceTypeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (invoiceTypeLookUpValueList != null)
            {
                for (int i = 0; i < invoiceTypeLookUpValueList.Count; i++)
                {
                    if (invoiceTypeLookUpValueList[i].LookupValue == "SYSTEM_AUTHORIZATION_NUMBER" && !string.IsNullOrEmpty(invoiceTypeLookUpValueList[i].Description))
                    {
                        txtSysAuthorization.Text = invoiceTypeLookUpValueList[i].Description;
                        txtSysAuthorization.Enabled = false;
                        seriesStartNumberDataGridViewTextBoxColumn.ReadOnly = true;
                        approvedDateDataGridViewTextBoxColumn.ReadOnly = true;
                    }
                    else
                    {
                        txtSysAuthorization.Enabled = true;
                    }
                }
            }
            log.LogMethodExit();
        }
        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadSystemResolutionNumber();
            LoadInvoiceType();
            LoadInvoiceSequenceSetuDTOList();
            log.LogMethodExit();
        }

        private void LoadInvoiceType()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INVOICE_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                invoiceTypeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (invoiceTypeLookUpValueList == null)
                {
                    invoiceTypeLookUpValueList = new List<LookupValuesDTO>();
                }
                invoiceTypeLookUpValueList.Insert(0, new LookupValuesDTO());
                invoiceTypeLookUpValueList[0].LookupValueId = -1;
                invoiceTypeLookUpValueList[0].LookupValue = "<SELECT>";
                BindingSource bs = new BindingSource();
                bs.DataSource = invoiceTypeLookUpValueList;
                ddlInvoiceType.DataSource = bs;
                ddlInvoiceType.ValueMember = "LookupValueId";
                ddlInvoiceType.DisplayMember = "LookupValue";

                bs = new BindingSource();
                bs.DataSource = invoiceTypeLookUpValueList;
                invoiceTypeIdDataGridViewTextBoxColumn.DataSource = bs;
                invoiceTypeIdDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                invoiceTypeIdDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        private void LoadInvoiceSequenceSetuDTOList()
        {
            log.LogMethodEntry();
            try
            {
                InvoiceSequenceSetupListBL invoiceSequenceSetupListBL = new InvoiceSequenceSetupListBL(machineUserContext);
                List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>>();
                if (chbShowActiveEntries.Checked)
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.ISACTIVE, "1"));
                try
                {
                    if (Convert.ToInt32(ddlInvoiceType.SelectedValue) >= 0)
                    {
                        searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.INVOICE_TYPE_ID, Convert.ToString(ddlInvoiceType.SelectedValue)));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while executing LoadInvoiceSequenceSetuDTOList()" + ex.Message);
                }
                searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOList = invoiceSequenceSetupListBL.GetAllInvoiceSequenceSetupList(searchParameters);

                if (invoiceSequenceSetupDTOList != null)
                {
                    invoiceSequenceSetupSortableList = new SortableBindingList<InvoiceSequenceSetupDTO>(invoiceSequenceSetupDTOList);
                }
                else
                {
                    invoiceSequenceSetupSortableList = new SortableBindingList<InvoiceSequenceSetupDTO>();
                }
                InvoiceSequenceSetupDTOListBS.DataSource = invoiceSequenceSetupSortableList;

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message); 
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadInvoiceSequenceSetuDTOList();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvInvoiceSequenceSetupDTOList.EndEdit();

            if (!string.IsNullOrWhiteSpace(txtSysAuthorization.Text)
                && txtSysAuthorization.Enabled == true)
            {
                DataTable datatable = utilities.executeDataTable(@"select lookupId from Lookups
                                                                                where lookupName = @lookupName",
                                                                  new System.Data.SqlClient.SqlParameter("@lookupName", "SYSTEM_AUTHORIZATION"));
                LookupValuesDTO lookupValuesDTO = new LookupValuesDTO();
                int lookupId;
                lookupId = (datatable.Rows[0]["lookupId"] == DBNull.Value ? -1 : Convert.ToInt32(datatable.Rows[0]["lookupId"].ToString()));
                Lookups lookups = new Lookups(machineUserContext, lookupId,true,true);
                lookupValuesDTO.LookupId = lookupId;
                lookupValuesDTO.LookupValue = "SYSTEM_AUTHORIZATION_NUMBER";
                lookupValuesDTO.Description = txtSysAuthorization.Text;
                if(lookups.LookupsDTO.LookupValuesDTOList == null)
                {
                    lookups.LookupsDTO.LookupValuesDTOList = new List<LookupValuesDTO>() { lookupValuesDTO };
                }
                else
                {
                    lookups.LookupsDTO.LookupValuesDTOList.Add(lookupValuesDTO);
                }
                lookups.Save();
                txtSysAuthorization.Enabled = false;
            }

            SortableBindingList<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOSortableList = (SortableBindingList<InvoiceSequenceSetupDTO>)InvoiceSequenceSetupDTOListBS.DataSource;
            string message;
            bool confirmSave = false;
            InvoiceSequenceSetupBL invoiceSequenceSetupBL;
            if (invoiceSequenceSetupDTOSortableList != null)
            {
                for (int i = 0; i < invoiceSequenceSetupDTOSortableList.Count; i++)
                {
                    if (invoiceSequenceSetupDTOSortableList[i].IsChanged)
                    {
                        message = ValidateInvoiceSequenceSetupDTO(invoiceSequenceSetupDTOSortableList[i]);
                        if (string.IsNullOrEmpty(message))
                        {
                            if (string.IsNullOrEmpty(txtSysAuthorization.Text) || confirmSave)
                            {
                                invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(machineUserContext, invoiceSequenceSetupDTOSortableList[i]);
                                try
                                {
                                    invoiceSequenceSetupBL.Save();
                                    confirmSave = true;
                                    if (invoiceSequenceSetupDTOSortableList[i].InvoiceSequenceSetupId > -1)
                                    {
                                        MessageBox.Show(utilities.MessageUtils.getMessage(1414));
                                    }
                                    RefreshData();
                                }
                                catch (ForeignKeyException ex)
                                {
                                    log.Error(ex.Message);
                                    dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                    break;
                                }
                                catch (InvalidResolutionNumberException ex)
                                {
                                    log.Error(ex.Message);
                                    dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1374));
                                    break;
                                }
                                catch (InvalidPrefixException ex)
                                {
                                    log.Error(ex.Message);
                                    dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1373));
                                    break;
                                }
                                catch (InvalidSeriesNumberException ex)
                                {
                                    log.Error(ex.Message);
                                    dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1375));
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Error while saving event.", ex);
                                    dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1824, ex.Message));
                                    break;
                                }
                            }
                            else if ((confirmSave || (MessageBox.Show(utilities.MessageUtils.getMessage(1328), "Confirm Save.", MessageBoxButtons.YesNo) == DialogResult.Yes)) &&
                                !string.IsNullOrEmpty(txtSysAuthorization.Text))
                            {
                                invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(machineUserContext, invoiceSequenceSetupDTOSortableList[i]);
                                try
                                {
                                    invoiceSequenceSetupDTOSortableList[i].ApprovedDate = ServerDateTime.Now.Date;
                                    invoiceSequenceSetupBL.Save();
                                    confirmSave = true;
                                    if (invoiceSequenceSetupDTOSortableList[i].InvoiceSequenceSetupId > -1)
                                    {
                                        MessageBox.Show(utilities.MessageUtils.getMessage(1414));
                                    }
                                    RefreshData();
                                }
                                catch (ForeignKeyException ex)
                                {
                                    log.Error(ex.Message);
                                    dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                    break;
                                }
                                catch (InvalidResolutionNumberException ex)
                                {
                                    log.Error(ex.Message);
                                    dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1374));
                                    break;
                                }
                                catch (InvalidPrefixException ex)
                                {
                                    log.Error(ex.Message);
                                    dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1373));
                                    break;
                                }
                                catch (InvalidSeriesNumberException ex)
                                {
                                    log.Error(ex.Message);
                                    dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1375));
                                    break;
                                }
                                catch (Exception)
                                {
                                    log.Error("Error while saving event.");
                                    dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            dgvInvoiceSequenceSetupDTOList.Rows[i].Selected = true;
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
            log.LogMethodExit();
        }

        private string ValidateInvoiceSequenceSetupDTO(InvoiceSequenceSetupDTO invoiceSequenceSetupDTO)
        {
            log.LogMethodEntry(invoiceSequenceSetupDTO);
            string message = string.Empty;
            if (((invoiceSequenceSetupDTO.ExpiryDate == null) || (invoiceSequenceSetupDTO.ExpiryDate < ServerDateTime.Now))
                && (!string.IsNullOrWhiteSpace(txtSysAuthorization.Text)))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", expiryDateDataGridViewTextBoxColumn.HeaderText);
            }
            else if ((invoiceSequenceSetupDTO.ExpiryDate == DateTime.MinValue) &&
                (string.IsNullOrWhiteSpace(txtSysAuthorization.Text)))
            {
                invoiceSequenceSetupDTO.ExpiryDate = DateTime.MaxValue;
            }

            if (invoiceSequenceSetupDTO.Prefix == null)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", prefixDataGridViewTextBoxColumn.HeaderText);
            }

            if (invoiceSequenceSetupDTO.InvoiceTypeId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", invoiceTypeIdDataGridViewTextBoxColumn.HeaderText);
            }

            if (invoiceSequenceSetupDTO.SeriesEndNumber == null && (!string.IsNullOrWhiteSpace(txtSysAuthorization.Text)))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", seriesEndNumberDataGridViewTextBoxColumn.HeaderText);
            }
            else if (invoiceSequenceSetupDTO.SeriesEndNumber == null && (string.IsNullOrWhiteSpace(txtSysAuthorization.Text)))
            {
                invoiceSequenceSetupDTO.SeriesEndNumber = Int32.MaxValue;
            }

            if (invoiceSequenceSetupDTO.ResolutionNumber == null && (!string.IsNullOrWhiteSpace(txtSysAuthorization.Text)))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", resolutionNumberDataGridViewTextBoxColumn.HeaderText);
            }
            else if (invoiceSequenceSetupDTO.ResolutionNumber == null && (string.IsNullOrWhiteSpace(txtSysAuthorization.Text)))
            {
                invoiceSequenceSetupDTO.ResolutionNumber = " ";
            }

            if ((invoiceSequenceSetupDTO.ResolutionDate == DateTime.MinValue && (!string.IsNullOrWhiteSpace(txtSysAuthorization.Text)))
                || (invoiceSequenceSetupDTO.ResolutionDate > ServerDateTime.Now.Date))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", resolutionDateDataGridViewTextBoxColumn.HeaderText);
            }
            else if (invoiceSequenceSetupDTO.ResolutionDate == DateTime.MinValue && (string.IsNullOrWhiteSpace(txtSysAuthorization.Text)))
            {
                invoiceSequenceSetupDTO.ResolutionDate = ServerDateTime.Now;
            }
            log.LogMethodExit(message);
            return message;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            chbShowActiveEntries.Checked = true;
            ddlInvoiceType.SelectedIndex = 0;
            RefreshData();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (dgvInvoiceSequenceSetupDTOList.SelectedRows.Count <= 0 && dgvInvoiceSequenceSetupDTOList.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.LogMethodExit();
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDB = false;
            if (this.dgvInvoiceSequenceSetupDTOList.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvInvoiceSequenceSetupDTOList.SelectedCells)
                {
                    dgvInvoiceSequenceSetupDTOList.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow row in dgvInvoiceSequenceSetupDTOList.SelectedRows)
            {
                if (Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                {
                    dgvInvoiceSequenceSetupDTOList.Rows.RemoveAt(row.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactivation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        SortableBindingList<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOSortableList = (SortableBindingList<InvoiceSequenceSetupDTO>)InvoiceSequenceSetupDTOListBS.DataSource;
                        InvoiceSequenceSetupDTO invoiceSequenceSetupDTO = invoiceSequenceSetupDTOSortableList[row.Index];
                        invoiceSequenceSetupDTO.IsActive = false;
                        InvoiceSequenceSetupBL invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(machineUserContext, invoiceSequenceSetupDTO);
                        confirmDelete = true;
                        refreshFromDB = true;
                        try
                        {
                            invoiceSequenceSetupBL.Save();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            dgvInvoiceSequenceSetupDTOList.Rows[row.Index].Selected = true;
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

        private void dgvInvoiceSequenceSetupDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvInvoiceSequenceSetupDTOList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvInvoiceSequenceSetupDTOList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if ((e.ColumnIndex == expiryDateDataGridViewTextBoxColumn.Index) && e.RowIndex >= 0)
            {

            }
            log.LogMethodExit();
        }

        private void InvoiceSequenceSetupDTOListBS_BindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry();
            if (invoiceSequenceSetupSortableList != null)
            {
                for (int i = 0; i < invoiceSequenceSetupSortableList.Count; i++)
                {
                    if (invoiceSequenceSetupSortableList[i].InvoiceSequenceSetupId != -1 &&
                        (!string.IsNullOrWhiteSpace(txtSysAuthorization.Text)))
                    {
                        dgvInvoiceSequenceSetupDTOList.Rows[i].ReadOnly = true;
                    }
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// The datagridview calender column class
    /// </summary>
    public class CalendarColumn : DataGridViewColumn
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// CalendarColumn
        /// </summary>
        public CalendarColumn() : base(new CalendarCell())
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// CellTemplate method
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                {
                    throw new InvalidCastException("Must be a CalendarCell");
                }
                base.CellTemplate = value;
            }
        }
    }


    /// <summary>
    /// The Calender Cell in the datagrid
    /// </summary>
    public class CalendarCell : DataGridViewTextBoxCell
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Use the short date format.
        /// </summary>
        public CalendarCell()
            : base()
        {
            log.LogMethodEntry();
            this.Style.Format = "d";
            log.LogMethodExit();
        }

        /// <summary>
        /// Sets the value for the calender cell
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="initialFormattedValue"></param>
        /// <param name="dataGridViewCellStyle"></param>
        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            log.LogMethodEntry(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            CalendarEditingControl ctl =
                DataGridView.EditingControl as CalendarEditingControl;
            // Use the default row value when Value property is null.
            if (this.Value == null)
            {
                ctl.Value = (DateTime)this.DefaultNewRowValue;
            }
            else
            {
                ctl.Value = ServerDateTime.Now;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Return the type of the editing control that CalendarCell uses.
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(CalendarEditingControl);
            }
        }

        /// <summary>
        /// Return the type of the value that CalendarCell contains.
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(DateTime);
            }
        }

        /// <summary>
        /// Use the current date and time as the default value.
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return null;
            }
        }
    }

    class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public CalendarEditingControl()
        {
            log.LogMethodEntry();
            this.Format = DateTimePickerFormat.Short;
            log.LogMethodExit();
        }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToShortDateString();
            }
            set
            {
                if (value is String)
                {
                    try
                    {
                        // This will throw an exception of the string is 
                        // null, empty, or not in the format of a date.
                        this.Value = DateTime.Parse((String)value);
                    }
                    catch (Exception ex)
                    {
                        // In the case of an exception, just use the 
                        // default value so we're not left with a null
                        // value.
                        this.Value = ServerDateTime.Now;
                        log.Error("Error occured while executing EditingControlFormattedValue() " + ex.Message);
                    }
                }
            }
        }

        // Implements the 
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            log.LogMethodEntry(context);
            log.LogMethodExit(EditingControlFormattedValue);
            return EditingControlFormattedValue;
        }

        // Implements the 
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            log.LogMethodEntry(dataGridViewCellStyle);
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
            log.LogMethodExit();
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
        // property.
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
        // method.
        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            log.LogMethodEntry("key", dataGridViewWantsInputKey);
            // Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    log.LogMethodExit(true);
                    return true;
                default:
                    log.LogMethodExit(!dataGridViewWantsInputKey);
                    return !dataGridViewWantsInputKey;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
        // method.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            log.LogMethodEntry(selectAll);
            // No preparation needs to be done.
            log.LogMethodExit();
        }

        // Implements the IDataGridViewEditingControl
        // .RepositionEditingControlOnValueChange property.
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlValueChanged property.
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingPanelCursor property.
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnValueChanged(EventArgs eventargs)
        {
            log.LogMethodEntry(eventargs);
            // Notify the DataGridView that the contents of the cell
            // have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
            log.LogMethodExit();
        }
    }
}
