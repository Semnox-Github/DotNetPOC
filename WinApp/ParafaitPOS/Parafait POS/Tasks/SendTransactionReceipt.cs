/********************************************************************************************
 * Project Name - POS
 * Description  - UI for Send Transaction Receipt
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *             02-Feb-2023    Prashanth V      Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.Tasks
{
    public partial class SendTransactionReceipt : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private ExecutionContext executionContext;
        private bool autoKeyboard = false;
        private bool isDateTimeValueChanged;
        private TextBox currentTextBox;
        private int customerId = -1;
        private AlphaNumericKeyPad keypad;

        public SendTransactionReceipt(Utilities Utilities)
        {
            log.LogMethodEntry(Utilities);
            InitializeComponent();
            utilities = Utilities;
            executionContext = utilities.ExecutionContext;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            autoKeyboard = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD");
            log.Debug("autoKeyboard :" + autoKeyboard);
            log.LogMethodExit();
        }

        private void SetLabelTextForUI()
        {
            log.LogMethodEntry();
            lblFromDate.Text = MessageContainerList.GetMessage(executionContext, "From Date") + ":";
            lblToDate.Text = MessageContainerList.GetMessage(executionContext, "To Date") + ":";
            lblTrxOTP.Text = MessageContainerList.GetMessage(executionContext, "Trx OTP") + ":";
            lblPhone.Text = MessageContainerList.GetMessage(executionContext, "Phone") + ":";
            lblEmail.Text = MessageContainerList.GetMessage(executionContext, "Email") + ":";
            lblTrxNo.Text = MessageContainerList.GetMessage(executionContext, "Trx No") + ":";
            lblCustomerLookUp.Text = MessageContainerList.GetMessage(executionContext, "Customer Name") + ":";
            lblPOS.Text = MessageContainerList.GetMessage(executionContext, "POS") + ":";
            log.LogMethodExit();
        }
        private void LoadPOSMachineDropdown()
        {
            log.LogMethodEntry();
            List<POSMachineDTO> posMachineDTOList = new List<POSMachineDTO>();
            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParams = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            searchParams.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "Y"));
            POSMachineList posMachineList = new POSMachineList(executionContext);
            posMachineDTOList = posMachineList.GetAllPOSMachines(searchParams);
            if (posMachineDTOList == null)
            {
                posMachineDTOList = new List<POSMachineDTO>();
            }

            POSMachineDTO posMachineDTO = new POSMachineDTO();
            posMachineDTO.POSName = "- Select -";
            posMachineDTOList.Insert(0, posMachineDTO);
            cmbPOS.DataSource = posMachineDTOList;
            cmbPOS.DisplayMember = "POSName";
            cmbPOS.ValueMember = "POSMachineId";
            cmbPOS.SelectedIndex = 0;
            log.LogMethodExit();
        }
        private void SendTransactionReceipt_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetUIElementsFontAndFormat();
            SetDateAndTime();
            SetLabelTextForUI();
            LoadPOSMachineDropdown();
            currentTextBox = txtTrxNo;
            txtTrxNo.Focus();
            log.LogMethodExit();
        }
        private void SetUIElementsFontAndFormat()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvTransaction);
            SetDGVCellFont(dgvTransaction);
            dtpFromDate.CustomFormat = dtpToDate.CustomFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            dgvTransaction.RowTemplate.Resizable = DataGridViewTriState.True;
            dgvTransaction.RowTemplate.Height = 40;
            dgvTransaction.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            log.LogMethodExit();
        }
        private void SetDGVCellFont(DataGridView dgvInput)
        {
            log.LogMethodEntry(dgvInput);
            Font font;
            try
            {
                font = new Font(utilities.ParafaitEnv.DEFAULT_GRID_FONT, 9, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying new font", ex);
                font = new Font("Arial", 8, FontStyle.Regular);
            }
            dgvTransaction.ColumnHeadersDefaultCellStyle.Font = new Font(font.FontFamily, 9F, FontStyle.Bold);
            foreach (DataGridViewColumn c in dgvInput.Columns)
            {
                c.DefaultCellStyle.Font = new Font(font.FontFamily, 9F, FontStyle.Regular);
            }
            log.LogMethodExit();
        }
        private void SetDateAndTime()
        {
            log.LogMethodEntry();
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.CustomFormat = "ddd,dd MMM yyy";
            dtpFromDate.ShowUpDown = false;
            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.CustomFormat = "ddd,dd MMM yyy";
            dtpToDate.ShowUpDown = false;

            DateTime Now, From, To = utilities.getServerTime();
            Now = From = To;

            int startHour = Convert.ToInt32(utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"));
            From = From.Date.AddHours(startHour);
            To = To.AddDays(1).Date.AddHours(startHour);
            String startHourStr = startHour >= 10 ? startHour.ToString() : ("0" + startHour.ToString());

            dtpFromDate.Value = From;
            dtpToDate.Value = To;
            cmbFromHour.SelectedItem = startHourStr;
            cmbFromMin.SelectedItem = "00";
            cmbFromAM.SelectedItem = "AM";
            cmbToHour.SelectedItem = startHourStr;
            cmbToMin.SelectedItem = "00";
            cmbToAM.SelectedItem = "AM";
            isDateTimeValueChanged = false;
            log.LogMethodExit();
        }

        private bool ValidateDetails()
        {
            log.LogMethodEntry();
            lblErrorMessage.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(txtTrxNo.Text) && string.IsNullOrWhiteSpace(txtTrxOTP.Text)
                && string.IsNullOrWhiteSpace(txtPhone.Text) && string.IsNullOrWhiteSpace(txtEmail.Text) && string.IsNullOrWhiteSpace(txtCustomerLookUp.Text)
                && cmbPOS.SelectedIndex <= 0 && dtpFromDate.Value == null && dtpToDate.Value == null)
            {
                log.Error("Nothing to search");
                lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, 5044);
                return false;
            }

            if (!string.IsNullOrEmpty(dtpFromDate.Value.ToString()) && !string.IsNullOrEmpty(dtpToDate.Value.ToString()) && dtpFromDate.Value.Date > dtpToDate.Value.Date)
            {
                lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, 1183); // From date should  be less or equal to To date.
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private void SetCurrentTextBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            currentTextBox = sender as TextBox;
            if (autoKeyboard)
            {
                if (keypad != null && !keypad.IsDisposed && currentTextBox == keypad.currentTextBox)
                {
                    return;
                }
                else if (keypad != null && !keypad.IsDisposed && currentTextBox != null)
                {
                    keypad.currentTextBox = currentTextBox;
                }
                btnShowKeyPad_Click(sender, e);
            }
            else if (keypad != null && !keypad.IsDisposed && keypad.Visible && keypad.currentTextBox != currentTextBox)
            {
                keypad.currentTextBox = currentTextBox;
                keypad.Hide();
            }
            log.LogMethodExit();
        }

        private void IsDateTimeChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.Debug("Current Value : isDateTimeValueChanged : " + isDateTimeValueChanged);
            isDateTimeValueChanged = true;
            log.Debug("After value committed Value : isDateTimeValueChanged : " + isDateTimeValueChanged);
            log.Debug("Date value is changed :");
            log.LogMethodExit();
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) || !string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtCustomerLookUp.Enabled = false;
                btnCustomerLookup.Enabled = false;
            }
            else
            {
                txtCustomerLookUp.Enabled = true;
                btnCustomerLookup.Enabled = true;
            }
            customerId = -1;
            log.LogMethodExit();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) || !string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                txtCustomerLookUp.Enabled = false;
                btnCustomerLookup.Enabled = false;
            }
            else
            {
                txtCustomerLookUp.Enabled = true;
                btnCustomerLookup.Enabled = true;
            }
            customerId = -1;
            log.LogMethodExit();
        }

        private void SendTransactionPurchaseMessage(TransactionSummaryViewDTO transactionSummaryViewDTO, List<KeyValuePair<string, string>> contactList)
        {
            log.LogMethodEntry();
            try
            {
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionSummaryViewDTO.TransactionId, utilities);
                if (transaction != null)
                {
                    TransactionEventsBL transactionEventsBL = new TransactionEventsBL(utilities.ExecutionContext, utilities, ParafaitFunctionEvents.PURCHASE_EVENT, transaction, null, null, null);
                    transactionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE, contactList, null);
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 5046), utilities.MessageUtils.getMessage("Message"), MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                log.Error(ex);
                lblErrorMessage.Text = message;
                return;
            }
        }

        private void txtCustomer_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (string.IsNullOrEmpty(txtCustomerLookUp.Text) == false)
            {
                customerId = -1;
                CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
                CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
                customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.LIKE, txtCustomerLookUp.Text)
                                      .OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                      .Paginate(0, 20);
                List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    if (customerDTOList.Count == 1)
                    {
                        log.LogMethodExit("customerDTOList.Count == 1");
                        this.Cursor = Cursors.Default;
                        CustomerDTO customerDTO = customerDTOList[0];
                        customerId = customerDTO.Id;
                        txtPhone.Enabled = false;
                        txtEmail.Enabled = false;
                        return;
                    }
                    CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities, txtCustomerLookUp.Text);
                    customerLookupUI.MinimizeBox = false;
                    customerLookupUI.MaximizeBox = false;
                    if (customerLookupUI.ShowDialog() == DialogResult.OK)
                    {
                        CustomerDTO customerDTO = customerLookupUI.SelectedCustomerDTO;
                        txtCustomerLookUp.Text = customerDTO.FirstName;
                        customerId = customerDTO.Id;
                        txtPhone.Enabled = false;
                        txtEmail.Enabled = false;
                    }
                    else
                    {
                        txtPhone.Enabled = true;
                        txtEmail.Enabled = true;
                    }
                }
                else
                {
                    txtPhone.Enabled = false;
                    txtEmail.Enabled = false;
                }
            }
            else
            {
                txtPhone.Enabled = true;
                txtEmail.Enabled = true;
                customerId = -1;
            }
            log.LogMethodExit();
        }

        private void btnCustomerLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                customerId = -1;
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities,
                                                                         txtCustomerLookUp.Text,
                                                                         "",
                                                                         "",
                                                                         "",
                                                                         "",
                                                                         "");
                customerLookupUI.MinimizeBox = false;
                customerLookupUI.MaximizeBox = false;
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    CustomerDTO customerDTO = customerLookupUI.SelectedCustomerDTO;
                    txtCustomerLookUp.Text = customerDTO.FirstName;
                    customerId = customerDTO.Id;
                    txtPhone.Enabled = false;
                    txtEmail.Enabled = false;
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.GetAllValidationErrorMessages(), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Lookup"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblErrorMessage.Text = string.Empty;
            try
            {
                if (ValidateDetails())
                {
                    List<TransactionSummaryViewDTO> transactionSummaryViewDTOs = new List<TransactionSummaryViewDTO>();
                    ITransactionSummaryViewUseCases transactionSummaryViewUseCases = TransactionUseCaseFactory.GetTransactionSummaryViewUseCases(executionContext);
                    DateTime? fromDate = DateTime.MinValue;
                    DateTime? toDate = DateTime.MinValue;
                    string trxNo = string.Empty;
                    string trxOTP = string.Empty;
                    int selectedPOSMachineId = -1;
                    int selectedCustomerId = -1;
                    if (!string.IsNullOrEmpty(dtpFromDate.Value.ToString()) && !string.IsNullOrEmpty(dtpToDate.Value.ToString()) &&
                        !string.IsNullOrWhiteSpace(cmbFromHour.SelectedItem.ToString()) && !string.IsNullOrWhiteSpace(cmbToHour.SelectedItem.ToString()) &&
                        !string.IsNullOrWhiteSpace(cmbFromMin.SelectedItem.ToString()) && !string.IsNullOrWhiteSpace(cmbToMin.SelectedItem.ToString()) &&
                        !string.IsNullOrWhiteSpace(cmbFromAM.SelectedItem.ToString()) && !string.IsNullOrWhiteSpace(cmbToAM.SelectedItem.ToString()))
                    {
                        string tempFromDate = dtpFromDate.Value.Date.ToString("yyyy-MM-dd") + " " + (cmbFromAM.SelectedItem.Equals("AM") ? cmbFromHour.SelectedItem : (Convert.ToInt32(cmbFromHour.SelectedItem) + 12).ToString()) + ":" + cmbFromMin.SelectedItem + ":00";
                        log.LogVariableState("fromDate:", tempFromDate);
                        string tempToDate = dtpToDate.Value.Date.ToString("yyyy-MM-dd") + " " + (cmbToAM.SelectedItem.Equals("AM") ? cmbToHour.SelectedItem : (Convert.ToInt32(cmbToHour.SelectedItem) + 12).ToString()) + ":" + cmbToMin.SelectedItem + ":00";
                        log.LogVariableState("toDate :", tempToDate);
                        fromDate = DateTime.ParseExact(tempFromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        toDate = DateTime.ParseExact(tempToDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    trxNo = (string.IsNullOrWhiteSpace(txtTrxNo.Text) ? string.Empty : txtTrxNo.Text.ToString());
                    trxOTP = (string.IsNullOrWhiteSpace(txtTrxOTP.Text) ? string.Empty : txtTrxOTP.Text.ToString());
                    if (cmbPOS.SelectedIndex > 0)
                    {
                        selectedPOSMachineId = Convert.ToInt32(cmbPOS.SelectedValue);
                    }
                    if (!string.IsNullOrWhiteSpace(txtCustomerLookUp.Text))
                    {
                        if (customerId != -1)
                        {
                            selectedCustomerId = customerId;
                        }
                        else
                        {
                            lblErrorMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 2945);
                            dgvTransactionBindingSource.DataSource = new List<TransactionSummaryViewDTO>();
                            return;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(txtPhone.Text) || !string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        List<KeyValuePair<CustomerSearchByParameters, string>> searchParams = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                        if (!string.IsNullOrWhiteSpace(txtPhone.Text))
                        {
                            searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.PHONE_NUMBER_LIST, txtPhone.Text.ToString()));
                        }
                        if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                        {
                            searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.EMAIL_LIST, txtEmail.Text.ToString()));
                        }
                        CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
                        List<CustomerDTO> customersDTOs = customerListBL.GetCustomerDTOList(searchParams);
                        if (customersDTOs != null && customersDTOs.Any())
                        {
                            customerId = customersDTOs[0].Id;
                            txtCustomerLookUp.Text = customersDTOs[0].FirstName;
                            transactionSummaryViewDTOs = await transactionSummaryViewUseCases.GetTransactionSummaryViewDTOList(posMachineId: selectedPOSMachineId,
                                                                                    transactionOTP: trxOTP, customerId: customerId,
                                                                                    fromDate: fromDate.Equals(DateTime.MinValue) ? null : fromDate, toDate: toDate.Equals(DateTime.MinValue) ? null : toDate,
                                                                                    transactionNumber: trxNo);

                            List<TransactionSummaryViewDTO> result = await transactionSummaryViewUseCases.GetTransactionSummaryViewDTOList(posMachineId: selectedPOSMachineId,
                                                                                    transactionOTP: trxOTP,
                                                                                    fromDate: fromDate.Equals(DateTime.MinValue) ? null : fromDate, toDate: toDate.Equals(DateTime.MinValue) ? null : toDate,
                                                                                    transactionNumber: trxNo, phoneNumberList: txtPhone.Text, emailList: txtEmail.Text);
                            if (transactionSummaryViewDTOs != null && transactionSummaryViewDTOs.Any())
                            {
                                if (result != null && result.Any())
                                {
                                    foreach (TransactionSummaryViewDTO transactionSummaryViewDTO in result)
                                    {
                                        if (!transactionSummaryViewDTOs.Any(t => t.TransactionId == transactionSummaryViewDTO.TransactionId))
                                        {
                                            transactionSummaryViewDTOs.Add(transactionSummaryViewDTO);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (result != null && result.Any())
                                {
                                    transactionSummaryViewDTOs.AddRange(result);
                                }
                            }
                        }
                        else
                        {
                            transactionSummaryViewDTOs = await transactionSummaryViewUseCases.GetTransactionSummaryViewDTOList(posMachineId: selectedPOSMachineId,
                                                                                    transactionOTP: trxOTP,
                                                                                    fromDate: fromDate.Equals(DateTime.MinValue) ? null : fromDate, toDate: toDate.Equals(DateTime.MinValue) ? null : toDate,
                                                                                    transactionNumber: trxNo, phoneNumberList: txtPhone.Text, emailList: txtEmail.Text);
                        }
                    }
                    else
                    {
                        transactionSummaryViewDTOs = await transactionSummaryViewUseCases.GetTransactionSummaryViewDTOList(posMachineId: selectedPOSMachineId,
                                                                                    transactionOTP: trxOTP, customerId: customerId,
                                                                                    fromDate: fromDate.Equals(DateTime.MinValue) ? null : fromDate, toDate: toDate.Equals(DateTime.MinValue) ? null : toDate,
                                                                                    transactionNumber: trxNo);
                    }
                    if (transactionSummaryViewDTOs != null && transactionSummaryViewDTOs.Any())
                    {
                        dgvTransactionBindingSource.DataSource = transactionSummaryViewDTOs;
                    }
                    else
                    {
                        lblErrorMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 2945);
                        dgvTransactionBindingSource.DataSource = new List<TransactionSummaryViewDTO>();
                    }
                    dgvTransaction.Refresh();
                    dgvTransaction.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                lblErrorMessage.Text = ex.Message;
                dgvTransactionBindingSource.DataSource = new List<TransactionSummaryViewDTO>();
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, currentTextBox);
                keypad.Show();
            }
            else if (keypad.Visible)
            {
                keypad.Hide();
            }
            else
            {
                keypad.Show();
            }
            log.LogMethodExit();
        }

        private void dgvTransaction_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lblErrorMessage.Text = string.Empty;
            try
            {
                if (e.ColumnIndex < 0)
                {
                    log.LogMethodExit(null);
                    return;
                }
                if (dgvTransaction.Columns[e.ColumnIndex].Name == "Resend")
                {
                    if (dgvTransaction.CurrentRow != null)
                    {
                        TransactionSummaryViewDTO transactionSummaryViewDTO = dgvTransaction.CurrentRow.DataBoundItem as TransactionSummaryViewDTO;
                        if (transactionSummaryViewDTO != null)
                        {
                            if (transactionSummaryViewDTO.CustomerId > -1)
                            {
                                CustomerBL customer = new CustomerBL(executionContext, transactionSummaryViewDTO.CustomerId);
                                if (customer != null && customer.CustomerDTO != null)
                                {
                                    List<string> dataListSourcePhone = new List<string>();
                                    List<string> dataListSourceEmail = new List<string>();
                                    if (customer.CustomerDTO.ContactDTOList != null && customer.CustomerDTO.ContactDTOList.Any())
                                    {
                                        if (customer.CustomerDTO.ContactDTOList.Any(c => c.ContactType.Equals(ContactType.PHONE)))
                                        {
                                            dataListSourcePhone = customer.CustomerDTO.ContactDTOList.Where(c => c.ContactType.Equals(ContactType.PHONE)).Select(x => x.Attribute1).ToList();
                                        }

                                        if (customer.CustomerDTO.ContactDTOList.Any(c => c.ContactType.Equals(ContactType.EMAIL)))
                                        {
                                            dataListSourceEmail = customer.CustomerDTO.ContactDTOList.Where(c => c.ContactType.Equals(ContactType.EMAIL)).Select(x => x.Attribute1).ToList();
                                        }
                                    }
                                    dataListSourcePhone.Insert(0, MessageContainerList.GetMessage(utilities.ExecutionContext, "Select"));
                                    dataListSourceEmail.Insert(0, MessageContainerList.GetMessage(utilities.ExecutionContext, "Select"));
                                    GenericDataEntry gde = new GenericDataEntry(2);
                                    gde.BackColor = Color.White;
                                    gde.ControlBox = false;
                                    gde.Text = MessageContainerList.GetMessage(executionContext, 5045);

                                    gde.DataEntryObjects[0].label = utilities.MessageUtils.getMessage("Phone");
                                    gde.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.StringList;
                                    gde.DataEntryObjects[0].width = 200;
                                    gde.DataEntryObjects[0].listDataSource = GenericDataEntry.GenerateListDataSource(dataListSourcePhone);
                                    if (dataListSourcePhone != null && dataListSourcePhone.Count > 1)
                                    {
                                        gde.DataEntryObjects[0].data = dataListSourcePhone[0];
                                    }
                                    else
                                    {
                                        gde.DataEntryObjects[0].readOnly = true;
                                    }

                                    gde.DataEntryObjects[1].label = utilities.MessageUtils.getMessage("Email");
                                    gde.DataEntryObjects[1].dataType = GenericDataEntry.DataTypes.StringList;
                                    gde.DataEntryObjects[1].width = 200;
                                    gde.DataEntryObjects[1].listDataSource = GenericDataEntry.GenerateListDataSource(dataListSourceEmail);
                                    if (dataListSourceEmail != null && dataListSourceEmail.Count > 1)
                                    {
                                        gde.DataEntryObjects[1].data = dataListSourceEmail[0];
                                    }
                                    else
                                    {
                                        gde.DataEntryObjects[1].readOnly = true;
                                    }

                                    if (gde.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        List<KeyValuePair<string, string>> customerContactList = new List<KeyValuePair<string, string>>();
                                        if (!string.IsNullOrWhiteSpace(gde.DataEntryObjects[0].data) && !gde.DataEntryObjects[0].data.Equals(dataListSourcePhone[0]))
                                        {
                                            customerContactList.Add(new KeyValuePair<string, string>("PHONENUMBER", gde.DataEntryObjects[0].data));
                                        }
                                        if (!string.IsNullOrWhiteSpace(gde.DataEntryObjects[1].data) && !gde.DataEntryObjects[1].data.Equals(dataListSourceEmail[0]))
                                        {
                                            customerContactList.Add(new KeyValuePair<string, string>("EMAILID", gde.DataEntryObjects[1].data));
                                        }
                                        if (customerContactList.Any())
                                        {
                                            SendTransactionPurchaseMessage(transactionSummaryViewDTO, customerContactList);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                GenericDataEntry gde = new GenericDataEntry(2);
                                gde.BackColor = Color.White;
                                gde.ControlBox = false;
                                gde.Text = MessageContainerList.GetMessage(executionContext, 5045);
                                gde.DataEntryObjects[0].label = utilities.MessageUtils.getMessage("Phone");
                                gde.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.String;
                                gde.DataEntryObjects[0].width = 200;
                                gde.DataEntryObjects[0].data = string.IsNullOrWhiteSpace(transactionSummaryViewDTO.PhoneNumber) ? string.Empty : transactionSummaryViewDTO.PhoneNumber;
                                gde.DataEntryObjects[1].label = utilities.MessageUtils.getMessage("Email");
                                gde.DataEntryObjects[1].dataType = GenericDataEntry.DataTypes.String;
                                gde.DataEntryObjects[1].width = 200;
                                gde.DataEntryObjects[1].data = string.IsNullOrWhiteSpace(transactionSummaryViewDTO.EmailId) ? string.Empty : transactionSummaryViewDTO.EmailId;
                                if (gde.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    if (string.IsNullOrWhiteSpace(gde.DataEntryObjects[0].data) && string.IsNullOrWhiteSpace(gde.DataEntryObjects[1].data))
                                    {
                                        lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, 4858, "Contact Details");
                                    }
                                    else if( (!string.IsNullOrWhiteSpace(gde.DataEntryObjects[0].data) && !Regex.IsMatch(gde.DataEntryObjects[0].data, @"^(\+\d{1,3}[- ]?)?[0-9]+$"))  || 
                                        (!string.IsNullOrWhiteSpace(gde.DataEntryObjects[1].data) && !Regex.IsMatch(gde.DataEntryObjects[1].data, @"^((([\w]+\.[\w]+)+)|([\w]+))@(([\w]+\.)+)([A-Za-z]{1,3})$")))
                                    {
                                        lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, 4801);
                                    }
                                    else
                                    {
                                        List<KeyValuePair<string, string>> contactList = new List<KeyValuePair<string, string>>();
                                        if (!string.IsNullOrWhiteSpace(gde.DataEntryObjects[0].data))
                                        {
                                            contactList.Add(new KeyValuePair<string, string>("PHONENUMBER", gde.DataEntryObjects[0].data));
                                        }
                                        if (!string.IsNullOrWhiteSpace(gde.DataEntryObjects[1].data))
                                        {
                                            contactList.Add(new KeyValuePair<string, string>("EMAILID", gde.DataEntryObjects[1].data));
                                        }
                                        if (contactList.Any())
                                        {
                                            SendTransactionPurchaseMessage(transactionSummaryViewDTO, contactList);
                                        }
                                    }
                                    
                                }    
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                lblErrorMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
    }
}
