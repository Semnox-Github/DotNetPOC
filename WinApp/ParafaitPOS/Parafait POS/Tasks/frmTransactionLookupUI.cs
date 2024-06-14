/********************************************************************************************
 * Project Name - POS
 * Description  - UI for transaction look up UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
*2.80         23-Apr-2020   Girish Kundar    Created
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class frmTransactionLookupUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private int trxNo;
        private int trxId;
        private string status;
        private string reference;
        private string otp;
        private DateTime fromDate;
        private DateTime toDate;
        private List<KeyValuePair<TransactionDTO.SearchByParameters, string>> keyValuePairs;
        private AlphaNumericKeyPad keypad;
        private bool autoKeyboard = false;
        private TextBox currentTextBox;
        private int customerId;
        private bool enableOrderShareAcrossUsers;
        private bool enableOrderShareAcrossPOS;
        private bool isDateTimeValueChanged;
        public List<KeyValuePair<TransactionDTO.SearchByParameters, string>> KeyValuePairs { get { return keyValuePairs; } }

        public frmTransactionLookupUI(Utilities Utilities) 
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
            LoadStatus();
            SetLableTextForUI();
            utilities.setLanguage(this);
            autoKeyboard = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD");
            enableOrderShareAcrossUsers = (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_ORDER_SHARE_ACROSS_USERS") == "Y");
            enableOrderShareAcrossPOS = (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS") == "Y");
            LoadPOSUserDropdown();
            log.Debug("enableOrderShareAcrossUsers :" + enableOrderShareAcrossUsers);
            log.Debug("enableOrderShareAcrossPOS :" + enableOrderShareAcrossPOS);
            log.Debug("autoKeyboard :" + autoKeyboard);
            log.LogMethodExit();
        }

        private void SetDateAndTime()
        {
            log.LogMethodEntry();
            DateTime Now, From, To = utilities.getServerTime();
            Now = From = To;
            int startHour = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BUSINESS_DAY_START_TIME"));
            if (Now.Hour < startHour)
            {
                To = To.Date.AddHours(startHour);
            }
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.CustomFormat = "ddd,dd MMM yyy";
            dtpFromDate.ShowUpDown = false;
            dtpFromDate.Value = From;

            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.CustomFormat = "ddd,dd MMM yyy";
            dtpToDate.ShowUpDown = false;
            dtpToDate.Value = To;

            cmbToHour.SelectedItem = Now.Hour <= 12 ? (Now.Hour < 10 ? "0" : "") + Now.Hour.ToString() : (((Now.Hour - 12) < 10 ? "0" : "") + (Now.Hour - 12).ToString()).ToString();
            int min = Now.Minute % 60;
            cmbToMin.SelectedItem = min < 15 ? "00" : min < 30 ? "15" : min < 45 ? "30" : "45";
            cmbToAM.SelectedItem = Now.Hour < 12 ? "AM" : "PM";
            cmbFromHour.SelectedItem = startHour <= 12 ? (startHour < 10 ? "0" : "") + startHour.ToString() : (startHour - 12).ToString();
            cmbFromMin.SelectedItem = "00";
            cmbFromAM.SelectedItem = startHour < 12 ? "AM" : "PM";
            isDateTimeValueChanged = false;
            log.LogMethodExit(); 
        }

        private void ClearFields()
        {
            log.LogMethodEntry();
            txtOTP.Text = string.Empty;
            txtReference.Text = string.Empty;
            txtTrxId.Text = string.Empty;
            txtTrxNumber.Text = string.Empty;
            SetDateAndTime();
            txtCustomer.Text = string.Empty;
            cmbStatus.SelectedIndex = -1;
            cmbUser.SelectedIndex = 0;
            lblErrorMessage.Text = string.Empty;
            currentTextBox = txtTrxId;
            txtTrxId.Focus();
            log.LogMethodExit();
        }

        private void SetLableTextForUI()
        {
            log.LogMethodEntry();
            lblFromDate.Text = MessageContainerList.GetMessage(executionContext, "From Date:");
            lblToDate.Text = MessageContainerList.GetMessage(executionContext, "To Date:");
            lblOTP.Text = MessageContainerList.GetMessage(executionContext, "OTP:");
            lblReference.Text = MessageContainerList.GetMessage(executionContext, "Reference:");
            lblStatus.Text = MessageContainerList.GetMessage(executionContext, "Status:");
            lblTrxNumber.Text = MessageContainerList.GetMessage(executionContext, "Trx No:");
            lblTrxId.Text = MessageContainerList.GetMessage(executionContext, "Trx Id:");
            lblCustomer.Text = MessageContainerList.GetMessage(executionContext, "Customer Name:");
            lblUser.Text = MessageContainerList.GetMessage(executionContext, "Cashier:");
            log.LogMethodExit();
        }
        private void LoadStatus()
        {
            try
            {
                log.LogMethodEntry();
                cmbStatus.Items.Add(" ");
                foreach (var status in Enum.GetValues(typeof(Transaction.TrxStatus)))
                {
                    cmbStatus.Items.Add((object)status);
                }
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }
        private void btlClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ClearFields();
            log.LogMethodExit();
        }

        private bool IsDateTimeChanged()
        {
            log.LogMethodEntry();
            bool isChanged = false;
            log.LogMethodExit(isChanged);
            return isChanged;
        }

        private bool ValidateDetails()
        {
            log.LogMethodEntry();
            lblErrorMessage.Text = string.Empty;
            if(string.IsNullOrEmpty(txtTrxId.Text) && string.IsNullOrEmpty(txtCustomer.Text)
                && string.IsNullOrEmpty(txtOTP.Text) && string.IsNullOrEmpty(txtReference.Text) && string.IsNullOrEmpty(txtTrxNumber.Text)
                && cmbStatus.SelectedIndex < 0 && Convert.ToInt32(cmbUser.SelectedValue) < 0 && isDateTimeValueChanged == false)
            {
                log.Error("Nothing to search");
                lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, "Invalid Search ");
                return false;
            }
            if (!string.IsNullOrEmpty(txtTrxId.Text))
            {
                int nVal = 0;
                if (int.TryParse(txtTrxId.Text, out nVal) == false)
                {
                    log.Error("Invalid trx Id");
                    lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, "Invalid trx Id");
                    return false;
                }
                if (Convert.ToInt32(txtTrxId.Text) < 0)
                {
                    log.Error("Invalid trx Id");
                    lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, "Invalid trx Id");
                    return false;
                }
            }

            //if (!string.IsNullOrEmpty(txtTrxNumber.Text))
            //{
            //    int nVal = 0;
            //    if (int.TryParse(txtTrxNumber.Text, out nVal) == false)
            //    {
            //        log.Error("Invalid trx Number");
            //        lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, "Invalid trx no");
            //        return false;
            //    }
            //    if (Convert.ToInt32(txtTrxNumber.Text) < 0)
            //    {
            //        log.Error("Invalid trx number");
            //        lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, "Invalid trx no");
            //        return false;
            //    }
            //}
           
            if (string.IsNullOrEmpty(dtpFromDate.Value.ToString()) || string.IsNullOrEmpty(dtpToDate.Value.ToString()))
            {
                lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, 15); // invalid Date value.
                return false;
            }
            if (dtpFromDate.Value.Date > dtpToDate.Value.Date)
            {
                lblErrorMessage.Text = MessageContainerList.GetMessage(executionContext, 1183); // From date should  be less or equal to To date.
                return false;
            }
            // If required can restrict the date search
            //double totalDays = dtpToDate.Value.Subtract(dtpFromDate.Value).TotalDays;
            //if (totalDays > 180)
            //{
            //    log.Debug("Date range for search should not be more than 180 days");
            //    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, "Date range for search should not be more than 180 days"), "Validation Error");
            //    return false;
            //}
            //if (totalDays < 0)
            //{
            //    //To date cannot be less than from date.
            //    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1093), "Validation Error");
            //    return false;
            //}

            log.LogMethodExit(true);
            return true;
        }

        private void btnKeyBoard_Click(object sender, EventArgs e)
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
                btnKeyBoard_Click(sender, e);
            }
            else if (keypad != null && !keypad.IsDisposed && keypad.Visible && keypad.currentTextBox != currentTextBox)
            {
                keypad.currentTextBox = currentTextBox;
                keypad.Hide();
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Dispose();
            log.LogMethodExit();
        }

        private void btnCustomerLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                customerId = -1;
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities,
                                                                         txtCustomer.Text,
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
                    txtCustomer.Text = customerDTO.FirstName;
                    customerId = customerDTO.Id;
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ClearFields();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (ValidateDetails())
                {
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    trxId = (string.IsNullOrEmpty(txtTrxId.Text) ? -1 : Convert.ToInt32(txtTrxId.Text));
                    if (trxId != -1)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, trxId.ToString()));
                    }
                    //trxNo = (string.IsNullOrEmpty(txtTrxNumber.Text) ? -1 : Convert.ToInt32(txtTrxNumber.Text));
                    if (string.IsNullOrWhiteSpace(txtTrxNumber.Text) == false)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_NUMBER, txtTrxNumber.Text.ToString()));
                    }
                    reference = (string.IsNullOrEmpty(txtReference.Text) ? string.Empty : txtReference.Text.ToString());
                    if (string.IsNullOrEmpty(reference) == false)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.REMARKS, reference.ToString()));
                    }
                    otp = (string.IsNullOrEmpty(txtOTP.Text) ? string.Empty : txtOTP.Text.ToString());
                    if (string.IsNullOrEmpty(otp) == false)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_OTP, otp.ToString()));
                    }
                    if (cmbFromHour.SelectedItem.Equals("12"))
                        cmbFromHour.SelectedItem = "00";
                    if (cmbToHour.SelectedItem.Equals("12"))
                        cmbToHour.SelectedItem = "00";

                    string tempFromDate = dtpFromDate.Value.Date.ToString("yyyy-MM-dd") + " " + (cmbFromAM.SelectedItem.Equals("AM") ? cmbFromHour.SelectedItem : (Convert.ToInt32(cmbFromHour.SelectedItem) + 12).ToString()) + ":" + cmbFromMin.SelectedItem + ":00";
                    log.LogVariableState("fromDate:", fromDate);
                    string tempToDate = dtpToDate.Value.Date.ToString("yyyy-MM-dd") + " " + (cmbToAM.SelectedItem.Equals("AM") ? cmbToHour.SelectedItem : (Convert.ToInt32(cmbToHour.SelectedItem) + 12).ToString()) + ":" + cmbToMin.SelectedItem + ":00";
                    log.LogVariableState("toDate :", toDate);
                    fromDate = DateTime.ParseExact(tempFromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    toDate = DateTime.ParseExact(tempToDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    TimeSpan timeDifference = toDate.Subtract(fromDate);
                    log.Debug("Time difference :" + timeDifference);
                    log.Debug("Days (Difference) "+ timeDifference.TotalDays);
                    if (timeDifference.TotalDays > 1 || isDateTimeValueChanged)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, toDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }

                    status = cmbStatus.SelectedIndex < 0 ? String.Empty : cmbStatus.SelectedItem.ToString();
                    if (string.IsNullOrEmpty(status) == false)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, status));
                    }

                    if (string.IsNullOrEmpty(txtCustomer.Text) == false)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                    }

                    // If enableOrderShareAcrossPOS = true then allow the user to see transaction details of all POS machines else current machine details will be shown
                    if (enableOrderShareAcrossPOS == false)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_NAME, utilities.ParafaitEnv.POSMachine));
                    }

                    // If enableOrderShareAcrossUsers = true then allow the user to pick any users transaction details else only logged in users details will be shown
                    int selectedUserId =  Convert.ToInt32(cmbUser.SelectedValue);
                    if (enableOrderShareAcrossUsers)
                    {
                        if(selectedUserId != -1)
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.USER_ID, selectedUserId.ToString()));
                    }
                    else  // if enableOrderShareAcrossUsers = false and not selected any user then get it from environment
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.USER_ID, utilities.ParafaitEnv.User_Id.ToString()));
                    }
                    log.Debug("searchParameters" + searchParameters);
                    keyValuePairs = searchParameters;
                    log.Debug("keyValuePairs" + keyValuePairs);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.Focus();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void txtCustomer_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (string.IsNullOrEmpty(txtCustomer.Text) == false)
            {
                customerId = -1;
                CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
                CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
                customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.LIKE, txtCustomer.Text)
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
                        return;
                    }
                    CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities, txtCustomer.Text);
                    customerLookupUI.MinimizeBox = false;
                    customerLookupUI.MaximizeBox = false;
                    if (customerLookupUI.ShowDialog() == DialogResult.OK)
                    {
                        CustomerDTO customerDTO = customerLookupUI.SelectedCustomerDTO;
                        txtCustomer.Text = customerDTO.FirstName;
                        customerId = customerDTO.Id;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void frmTransactionLookupUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetDateAndTime();
            currentTextBox = txtTrxId;
            txtTrxId.Focus();
            log.LogMethodExit();
        }

        private void LoadPOSUserDropdown()
        {
            log.LogMethodEntry();
            List<UsersDTO> eventHostUsersDTOList = new List<UsersDTO>();  
            if (enableOrderShareAcrossUsers)
            {
                string excludedUserRoleList = GetExcludedUserRoleList();
                UsersList usersListBL = new UsersList(executionContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_NOT_IN, excludedUserRoleList));
                searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                eventHostUsersDTOList = usersListBL.GetAllUsers(searchParameters);
                if(eventHostUsersDTOList == null)
                {
                    eventHostUsersDTOList = new List<UsersDTO>();
                }
            }
            else
            {
                UsersDTO currentUsersDTO = new UsersDTO();
                currentUsersDTO.UserName = utilities.ParafaitEnv.Username;
                currentUsersDTO.UserId = utilities.ParafaitEnv.User_Id;
                eventHostUsersDTOList.Add(currentUsersDTO);
            }
            UsersDTO usersDTO = new UsersDTO();
            usersDTO.UserName = "- None -";
            eventHostUsersDTOList.Insert(0, usersDTO);
            cmbUser.DataSource = eventHostUsersDTOList;
            cmbUser.DisplayMember = "UserName";
            cmbUser.ValueMember = "UserId";
            cmbUser.SelectedIndex = 0;
            log.LogMethodExit();
        }
        private string GetExcludedUserRoleList()
        {
            log.LogMethodEntry();
            string excludedUserRoleList = "";
            LookupValuesList lookupValuesListBL = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "EXCLUDED_USER_ROLES_FOR_TRX_LOOKUP_UI"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesListBL.GetAllLookupValues(searchParameters);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
            {
                int count = 0;
                foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                {
                    if (count < lookupValuesDTOList.Count - 1)
                    {
                        excludedUserRoleList = excludedUserRoleList + lookupValuesDTO.Description + ",";
                    }
                    else
                    {
                        excludedUserRoleList = excludedUserRoleList + lookupValuesDTO.Description;
                    }
                    count++;
                }
            }
            log.LogMethodExit(excludedUserRoleList);
            return excludedUserRoleList;
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
    }
}
