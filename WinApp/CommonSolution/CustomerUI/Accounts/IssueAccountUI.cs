/********************************************************************************************
 * Project Name - CustomerUI
 * Description  - issue new account/ update existing account UI
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 *********************************************************************************************
 *2.70        10-Jul-2019       Lakshminarayana     Modified for adding ultralight c card support.
 *2.80        17-Feb-2019       Deeksha             Modified to Make DigitalSignage module as
 *                                                  read only in Windows Management Studio.
 *2.80        21-Jun-2020       Girish Kundar       Modified: Part of AccountBL cleanup. 
 *                                                            Moved the methods which works on multiple entities to new BL      
 *2.90         03-July-2020     Girish Kundar   Modified : Change as part of CardCodeDTOList replaced with AccountDTOList in CustomerDTO                                                          
 ********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Device;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Create and Update Account UI class
    /// </summary>
    public partial class IssueAccountUI : Form
    {
        private AccountDTO accountDTO;
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool savePreviousAccountStateInAccountAudit = true; // used to save card data in audit table before the first update is saved to database
        private readonly TagNumberParser tagNumberParser;
        private COMPort serialPort;
        private int portNumber;
        private DeviceClass readerDevice;
        private bool dataChanged = false;
        private bool closeReadersUponClose = false;
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="accountId"></param>
        /// <param name="readerDevice"></param>
        /// <param name="portNumber"></param>
        public IssueAccountUI(Utilities utilities, int accountId, DeviceClass readerDevice, int portNumber)
        {
            log.LogMethodEntry(utilities, accountId, readerDevice, portNumber);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(utilities.ExecutionContext);
            this.readerDevice = readerDevice;
            this.portNumber = portNumber;
            savePreviousAccountStateInAccountAudit = true;
            if (accountId >= 0)
            {
                AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountId);
                accountDTO = accountBL.AccountDTO;
            }
            else
            {
                accountDTO = new AccountDTO();
                accountDTO.IssueDate = utilities.getServerTime();
            }

            if (utilities.ExecutionContext.GetUserId().ToLower() != "semnox")
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ALLOW_MANUAL_CARD_UPDATE") == false)
                {
                    grpEntitlements.Enabled = false;
                    chkVIPCustomer.Enabled = false;
                    cmbCardType.Enabled = false;
                    btnAddToCard.Enabled = false;
                    grpGameTime.Enabled = false;
                    chkVIPCustomer.Enabled = false;
                    checkBoxValidFlag.Enabled = false;
                    grpMisc.Enabled = false;
                    grpRefund.Enabled = false;
                    grpTech.Enabled = false;
                    grpTickets.Enabled = false;
                    cbxPrimaryAccount.Enabled = false;
                    grpTech.Enabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ALLOW_TECH_CARD_UPDATE");
                }
            }

            if (readerDevice != null && accountDTO.AccountId < 0)
            {
                readerDevice.Register(new EventHandler(CardScanCompleteEventHandle));
                closeReadersUponClose = true;
            }
            ThemeUtils.SetupVisuals(this);
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    MessageBox.Show(message);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                string CardNumber = tagNumber.Value;
                cardSwiped(CardNumber, sender as DeviceClass);
            }
            log.LogMethodExit();
        }

        private void cardSwiped(string CardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(CardNumber, readerDevice);
            textBoxCardNumber.Text = CardNumber;
            log.LogMethodExit();
        }

        private void ListenSerialPort()
        {
            log.LogMethodEntry();
            if (portNumber == 0)
                return;

            serialPort = new COMPort(portNumber);
            serialPort.setReceiveAction = dataReceived;
            try
            {
                serialPort.Close();
            }
            catch { }
            serialPort.Open();
            log.LogMethodExit();
        }

        private bool checkCardExisting()
        {
            log.LogMethodEntry();
            AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
            AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria(AccountDTO.SearchByParameters.TAG_NUMBER, Operator.EQUAL_TO, textBoxCardNumber.Text);
            accountSearchCriteria.And(AccountDTO.SearchByParameters.VALID_FLAG, Operator.EQUAL_TO, "Y");
            List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria);
            bool result = true;
            if (accountDTOList != null && accountDTOList.Count > 0)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 682), MessageContainerList.GetMessage(utilities.ExecutionContext, "Manual Issue Error"));
                textBoxCardNumber.Text = "";
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void dataReceived()
        {
            log.LogMethodEntry();
            string receivedData = serialPort.ReceivedData;

            this.Invoke((MethodInvoker)delegate
            {
                if (String.IsNullOrEmpty(textBoxCardId.Text))
                {
                    textBoxCardNumber.Text = receivedData;
                    checkCardExisting();
                }
            });
            log.LogMethodExit();
        }

        private void IssueAccount_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string message = "";
            KeyManagement km = new KeyManagement(utilities.DBUtilities, utilities.ParafaitEnv);
            if (!km.validateLicense(ref message))
            {
                MessageBox.Show(message, MessageContainerList.GetMessage(utilities.ExecutionContext, "License Check"));
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 739), MessageContainerList.GetMessage(utilities.ExecutionContext, "License Check"));
                this.Close();
            }

            //Common.setupVisuals(this);

            if (string.IsNullOrEmpty(accountDTO.TagNumber)) // card reader required only for new card issue
            {
                cbxPrimaryAccount.Enabled = false;
                try
                {
                    ListenSerialPort();
                }
                catch { }
            }

            cmbCardType.Enabled = false;

            MembershipsList membershipsListBL = new MembershipsList(utilities.ExecutionContext);
            List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            List<MembershipDTO> membershipList = membershipsListBL.GetAllMembership(searchParam, utilities.ExecutionContext.GetSiteId());
            if (membershipList == null)
            {
                membershipList = new List<MembershipDTO>();
            }
            membershipList.Insert(0, new MembershipDTO());
            membershipList[0].MembershipName = "<SELECT>";
            cmbCardType.DataSource = membershipList;
            cmbCardType.DisplayMember = "MembershipName";
            cmbCardType.ValueMember = "MembershipID";

            cmbTechCardType.DataSource = GetTechnicianCardDataSource();
            cmbTechCardType.ValueMember = "Key";
            cmbTechCardType.DisplayMember = "Value";

            if (!string.IsNullOrEmpty(accountDTO.TagNumber)) // Update Card
            {
                DisplayAccountDetails();
            }
            else // insert card
            {
                checkBoxValidFlag.Checked = true;
                textBoxFaceValue.Text = String.Format("{0:" + utilities.ParafaitEnv.CURRENCY_SYMBOL + " " + utilities.getAmountFormat() + "}", 0);
                checkBoxValidFlag.Enabled = false;
                textBoxIssueDate.Text = Convert.ToDateTime(DateTime.Now).ToString(utilities.getDateTimeFormat());
                textBoxCardNumber.Focus();
                chkVIPCustomer.Checked = false;
                chkTicketAllowed.Checked = true;
                if (utilities.ParafaitEnv.REAL_TICKET_MODE == 'Y')
                    chkRealTicketMode.Checked = true;
                else
                    chkRealTicketMode.Checked = false;
                checkBoxRefundFlag.Checked = false;
                cmbTechCardType.SelectedValue = "N";
                chkTimerResetCard.Checked = false;
            }
            log.LogMethodExit();
        }

        private List<KeyValuePair<string, string>> GetTechnicianCardDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> techCardDataSource = new List<KeyValuePair<string, string>>();
            techCardDataSource.Add(new KeyValuePair<string, string>("N", MessageContainerList.GetMessage(utilities.ExecutionContext, "None")));
            techCardDataSource.Add(new KeyValuePair<string, string>("Y", MessageContainerList.GetMessage(utilities.ExecutionContext, "Staff Card")));
            techCardDataSource.Add(new KeyValuePair<string, string>("D", MessageContainerList.GetMessage(utilities.ExecutionContext, "Enable / Disable Gameplay")));
            log.LogMethodExit(techCardDataSource);
            return techCardDataSource;
        }

        private void DisplayAccountDetails()
        {
            textBoxCardId.Text = accountDTO.AccountId.ToString();
            textBoxCardNumber.Text = accountDTO.TagNumber;
            textBoxIssueDate.Text = (accountDTO.IssueDate.HasValue) ? accountDTO.IssueDate.Value.ToString(utilities.getDateTimeFormat()) : "";
            textBoxFaceValue.Text = String.Format("{0:" + utilities.ParafaitEnv.CURRENCY_SYMBOL + " " + utilities.getAmountFormat() + "}", accountDTO.FaceValue);
            textBoxNotes.Text = accountDTO.Notes;
            txtCredits.Text = GetAmountFormattedDecimalValue(accountDTO.Credits);
            txtCourtesy.Text = GetAmountFormattedDecimalValue(accountDTO.Courtesy);
            txtBonus.Text = string.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.Bonus);
            textBoxRefundAmount.Text = string.Format("{0:" + utilities.ParafaitEnv.CURRENCY_SYMBOL + " " + utilities.getAmountFormat() + "}", accountDTO.RefundAmount);
            textBoxTicketCount.Text = string.Format("{0:" + utilities.getNumberFormat() + "}", accountDTO.TicketCount);
            txtCustomerName.Text = accountDTO.CustomerName.ToString();
            if (accountDTO.LastUpdateDate != DateTime.MinValue)
            {
                txtLastUpdateTime.Text = accountDTO.LastUpdateDate.ToString(utilities.getDateTimeFormat());
            }
            txtLasUpdatedBy.Text = accountDTO.LastUpdatedBy;
            txtCreditsPlayed.Text = string.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.CreditsPlayed);
            txtLoyaltyPoints.Text = string.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.LoyaltyPoints);
            txtTime.Text = string.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.Time);
            textBoxTechGames.Text = GetNumberFormattedIntegerValue(accountDTO.TechGames);

            if (accountDTO.StartTime.HasValue)
                txtStartTime.Text = Convert.ToDateTime(accountDTO.StartTime.Value).ToString(utilities.getDateTimeFormat());

            if (accountDTO.LastPlayedTime.HasValue)
                txtLastPlayedTime.Text = Convert.ToDateTime(accountDTO.LastPlayedTime.Value).ToString(utilities.getDateTimeFormat());

            if (accountDTO.RefundDate.HasValue)
                textBoxRefundDate.Text = Convert.ToDateTime(accountDTO.RefundDate.Value).ToString(utilities.getDateTimeFormat());

            if (accountDTO.ExpiryDate.HasValue)
                txtExpiryDate.Text = Convert.ToDateTime(accountDTO.ExpiryDate.Value).ToString(utilities.getDateTimeFormat());

            checkBoxRefundFlag.Checked = accountDTO.RefundFlag;
            checkBoxValidFlag.Checked = accountDTO.ValidFlag;
            chkTicketAllowed.Checked = accountDTO.TicketAllowed;
            chkRealTicketMode.Checked = accountDTO.RealTicketMode;
            chkVIPCustomer.Checked = accountDTO.VipCustomer;
            cmbTechCardType.SelectedValue = accountDTO.TechnicianCard;
            chkTimerResetCard.Checked = accountDTO.TimerResetCard;
            txtAccountIdentifier.Text = accountDTO.AccountIdentifier;
            cbxPrimaryAccount.Checked = accountDTO.PrimaryAccount;

            if (accountDTO.PrimaryAccount)
            {
                cbxPrimaryAccount.Enabled = false;
            }
            else
            {
                if (accountDTO.AccountId > -1)
                {
                    cbxPrimaryAccount.Enabled = true;
                }
                else
                {
                    cbxPrimaryAccount.Enabled = false;
                }
            }

            if (accountDTO.CustomerId >= 0)
            {
                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, accountDTO.CustomerId);
                cmbCardType.SelectedValue = customerBL.CustomerDTO.MembershipId;
            }
            //cmbCardType.SelectedValue = cardRow["MembershipId"];

            if (accountDTO.CustomerId != -1)
                btnCustomer.ForeColor = Color.OrangeRed;


            if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Count > 0)
            {
                btnDiscounts.ForeColor = Color.OrangeRed;
            }

            if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Count > 0)
            {
                btnGames.ForeColor = Color.OrangeRed;
            }

            if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Count > 0)
            {
                btnCreditPlus.ForeColor = Color.OrangeRed;
            }

            txtCPCredits.Text = String.Format("{0:" + utilities.getAmountFormat() + "}", Convert.ToDecimal(accountDTO.AccountSummaryDTO.CreditPlusCardBalance) + Convert.ToDecimal(accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits) + Convert.ToDecimal(accountDTO.AccountSummaryDTO.CreditPlusItemPurchase));
            txtCPBonus.Text = String.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.AccountSummaryDTO.CreditPlusBonus);
            txtCPLoyalty.Text = String.Format("{0:" + utilities.getAmountFormat() + "}", accountDTO.AccountSummaryDTO.CreditPlusLoyaltyPoints);
            txtCPTickets.Text = String.Format("{0:" + utilities.getNumberFormat() + "}", accountDTO.AccountSummaryDTO.CreditPlusTickets);

            lblTransactionType.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Update Card");
            this.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Update Card");
            textBoxCardNumber.ReadOnly = true;
            txtCredits.Focus();
        }

        private string GetAmountFormattedDecimalValue(decimal? value)
        {
            return string.Format("{0:" + utilities.getAmountFormat() + "}", value);
        }

        private string GetNumberFormattedIntegerValue(int? value)
        {
            return string.Format("{0:" + utilities.getNumberFormat() + "}", value);
        }

        private void UpdateAccountDTO()
        {
            log.LogMethodEntry();
            textBoxCardNumber.Text = textBoxCardNumber.Text.Trim();
            if (accountDTO.AccountId <= -1)
            {
                accountDTO.TagNumber = textBoxCardNumber.Text;
                accountDTO.FaceValue = 0;
                accountDTO.RefundFlag = false;
                accountDTO.ValidFlag = true;
                accountDTO.Credits = GetDecimalValue(txtCredits.Text);
                accountDTO.Courtesy = GetDecimalValue(txtCourtesy.Text);
                accountDTO.Bonus = GetDecimalValue(txtBonus.Text);
                accountDTO.Time = GetDecimalValue(txtTime.Text);
                accountDTO.TechGames = GetIntegerValue(textBoxTechGames.Text);
                accountDTO.TicketCount = GetIntegerValue(textBoxTicketCount.Text);
                accountDTO.Notes = textBoxNotes.Text;
                accountDTO.TicketAllowed = chkTicketAllowed.Checked;
                accountDTO.RealTicketMode = chkRealTicketMode.Checked;
                accountDTO.VipCustomer = chkVIPCustomer.Checked;
                accountDTO.TechnicianCard = cmbTechCardType.SelectedValue.ToString();
                accountDTO.TimerResetCard = chkTimerResetCard.Checked;
            }
            else
            {
                if (AreEqual(txtCredits.Text, GetAmountFormattedDecimalValue(accountDTO.Credits)) == false)
                {
                    accountDTO.Credits = GetDecimalValue(txtCredits.Text);
                }
                if (AreEqual(txtCourtesy.Text, GetAmountFormattedDecimalValue(accountDTO.Courtesy)) == false)
                {
                    accountDTO.Courtesy = GetDecimalValue(txtCourtesy.Text);
                }
                if (AreEqual(txtBonus.Text, GetAmountFormattedDecimalValue(accountDTO.Bonus)) == false)
                {
                    accountDTO.Bonus = GetDecimalValue(txtBonus.Text);
                }
                if (AreEqual(textBoxTechGames.Text, GetNumberFormattedIntegerValue(accountDTO.TechGames)) == false)
                {
                    accountDTO.TechGames = GetIntegerValue(textBoxTechGames.Text);
                }
                if (string.IsNullOrWhiteSpace(txtStartTime.Text))
                {
                    if (accountDTO.StartTime.HasValue)
                    {
                        accountDTO.StartTime = null;
                    }
                    if (accountDTO.LastPlayedTime.HasValue)
                    {
                        accountDTO.LastPlayedTime = null;
                    }
                }
                if (accountDTO.TicketAllowed != chkTicketAllowed.Checked)
                {
                    accountDTO.TicketAllowed = chkTicketAllowed.Checked;
                }
                if (accountDTO.Notes != textBoxNotes.Text)
                {
                    accountDTO.Notes = textBoxNotes.Text;
                }
                if (accountDTO.RealTicketMode != chkRealTicketMode.Checked)
                {
                    accountDTO.RealTicketMode = chkRealTicketMode.Checked;
                }
                if (accountDTO.VipCustomer != chkVIPCustomer.Checked)
                {
                    accountDTO.VipCustomer = chkVIPCustomer.Checked;
                }
                if (accountDTO.ValidFlag != checkBoxValidFlag.Checked)
                {
                    accountDTO.ValidFlag = checkBoxValidFlag.Checked;
                }
                if (accountDTO.TechnicianCard != cmbTechCardType.SelectedValue.ToString())
                {
                    accountDTO.TechnicianCard = cmbTechCardType.SelectedValue.ToString();
                }
                if (accountDTO.TimerResetCard != chkTimerResetCard.Checked)
                {
                    accountDTO.TimerResetCard = chkTimerResetCard.Checked;
                }
                if (accountDTO.PrimaryAccount != cbxPrimaryAccount.Checked)
                {
                    accountDTO.PrimaryAccount = cbxPrimaryAccount.Checked;
                }
            }
            log.LogMethodExit();
        }


        private bool AreEqual(decimal? value1, decimal? value2)
        {
            log.LogMethodEntry(value1, value2);
            bool result = true;
            if (value1 != value2)
            {
                result = false;
                if ((value1.HasValue == false && value2.HasValue && value2.Value == 0) ||
                    (value2.HasValue == false && value1.HasValue && value1.Value == 0))
                {
                    result = true;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool AreEqual(string value1, string value2)
        {
            if (value1 == null && value2 != null)
            {
                return false;
            }
            return value1.Equals(value2);
        }

        private bool AreEqual(int? value1, int? value2)
        {
            log.LogMethodEntry(value1, value2);
            bool result = true;
            if (value1 != value2)
            {
                result = false;
                if ((value1.HasValue == false && value2.HasValue && value2.Value == 0) ||
                    (value2.HasValue == false && value1.HasValue && value1.Value == 0))
                {
                    result = true;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateAccountDTO();
            List<AccountDTO> accountDTOList = new List<AccountDTO>();
            if (accountDTO.IsChanged == false)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 371));
                log.LogMethodExit(null, "nothing to save");
                return;
            }
            if (accountDTO.AccountId >= 0 && accountDTO.CustomerId >= 0 && accountDTO.PrimaryAccount)
            {
                int currentCardId = Convert.ToInt32(textBoxCardId.Text);
                AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
                AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria(AccountDTO.SearchByParameters.CUSTOMER_ID, Operator.EQUAL_TO, accountDTO.CustomerId);
                accountSearchCriteria.And(AccountDTO.SearchByParameters.VALID_FLAG, Operator.EQUAL_TO, "Y");
                accountSearchCriteria.And(AccountDTO.SearchByParameters.ACCOUNT_ID, Operator.NOT_EQUAL_TO, accountDTO.AccountId);
                accountSearchCriteria.And(AccountDTO.SearchByParameters.PRIMARY_ACCOUNT, Operator.EQUAL_TO, "Y");
                accountDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria);
                if (accountDTOList != null && accountDTOList.Count > 0)
                {
                    if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1598, accountDTOList[0].TagNumber), MessageContainerList.GetMessage(utilities.ExecutionContext, "Primary Card Update"), MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        cbxPrimaryAccount.Checked = false;
                        log.LogMethodExit(null, "User canceled the primary card update");
                        return;
                    }
                    else
                    {
                        foreach (AccountDTO customerAccountDTO in accountDTOList)
                        {
                            customerAccountDTO.PrimaryAccount = false;
                        }
                      
                    }
                }
            }

            accountDTOList.Add(accountDTO); // add the current DTO
            SqlConnection connection = utilities.getConnection();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                foreach (AccountDTO customerAccountDTO in accountDTOList)
                {
                    savePreviousAccountStateInAccountAudit = true;
                    AccountBL accountBL = new AccountBL(utilities.ExecutionContext, customerAccountDTO);
                    accountBL.SaveManualChanges(savePreviousAccountStateInAccountAudit, transaction);
                }
                transaction.Commit();
                savePreviousAccountStateInAccountAudit = false;
                dataChanged = true;
            }
            catch (ValidationException ex)
            {
                log.LogVariableState("accountDTO", accountDTO);
                log.Error("Validation failed", ex);
                log.Error(ex.GetAllValidationErrorMessages());
                MessageBox.Show(ex.GetAllValidationErrorMessages());
                transaction.Rollback();
                log.LogMethodExit("", "Validation Exception Occurred while saving account");
                return;
            }
            catch (Exception ex)
            {
                log.LogVariableState("accountDTO", accountDTO);
                log.Error("Error occurred while saving the account", ex);
                MessageBox.Show(ex.Message);
                transaction.Rollback();
                log.LogMethodExit("", "Exception Occurred while saving account");
                return;
            }
            if (dataChanged)
            {
                btnRefresh.PerformClick();

            }
            log.LogMethodExit();
        }

        private decimal? GetDecimalValue(string decimalValueString)
        {
            log.LogMethodEntry(decimalValueString);
            decimal? result = null;
            decimal d;
            if (decimal.TryParse(decimalValueString, out d))
            {
                result = d;
            }
            log.LogMethodExit(result);
            return result;
        }

        private int? GetIntegerValue(string integerValueString)
        {
            log.LogMethodEntry(integerValueString);
            int i;
            int? result = null;
            if (int.TryParse(integerValueString, out i))
            {
                result = i;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void txtCredits_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!validateAndFormat((TextBox)sender))
            {
                e.Cancel = true;
            }
            log.LogMethodExit();
        }

        private bool validateAndFormat(TextBox txt)
        {
            log.LogMethodEntry(txt);
            bool result = true;
            txt.Text = txt.Text.Trim();
            if (!string.IsNullOrEmpty(txt.Text))
            {
                if (!utilities.isNumberPositive(txt.Text))
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 647));
                    result = false;
                }
                else
                {
                    txt.Text = Convert.ToDecimal(txt.Text).ToString(utilities.getAmountFormat());
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private void txtCourtesy_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!validateAndFormat((TextBox)sender))
                e.Cancel = true;
            log.LogMethodExit();
        }

        private void txtBonus_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!validateAndFormat((TextBox)sender))
                e.Cancel = true;
            log.LogMethodExit();
        }

        private void textBoxTicketCount_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!validateAndFormat((TextBox)sender))
                e.Cancel = true;
            log.LogMethodExit();
        }

        private void IssueCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (closeReadersUponClose)
            {
                try
                {
                    if (serialPort != null) // card reader required only for new card issue
                    {
                        serialPort.Close();
                        serialPort.comPort.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving serial port", ex);
                }

                if (readerDevice != null)
                    readerDevice.UnRegister();
            }

            if (dataChanged)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
            log.LogMethodExit();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (accountDTO.AccountId < 0)
            {
                log.LogMethodExit(null, "Account should be saved before linking to a customer");
                return;
            }
            UpdateAccountDTO();
            if (accountDTO.IsChanged)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1134));
                log.LogMethodExit(null, "Unsaved changes exist. Please save");
                return;
            }
            CustomerDTO customerDTO = null;
            if (accountDTO.CustomerId != -1)
            {
                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, accountDTO.CustomerId);
                customerDTO = customerBL.CustomerDTO;
            }
            else
            {
                customerDTO = new CustomerDTO();
            }
            CustomerDetailForm customerDetailForm = new CustomerDetailForm(utilities, customerDTO, MessageBox.Show, false);
            if (customerDetailForm.ShowDialog() == DialogResult.OK)
            {
                customerDTO = customerDetailForm.CustomerDTO;
                SqlConnection sqlConnection = utilities.getConnection();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    if (customerDTO.CustomerType == CustomerType.UNREGISTERED)
                    {
                        customerDTO.CustomerType = CustomerType.REGISTERED;
                    }
                    AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountDTO.AccountId, false, false, sqlTransaction);
                    CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDTO);
                    customerBL.Save(sqlTransaction);
                    if (accountBL.AccountDTO.CustomerId != customerDetailForm.CustomerDTO.Id)
                    {
                        accountBL.AccountDTO.CustomerId = customerDetailForm.CustomerDTO.Id;
                        accountBL.Save(sqlTransaction);
                    }
                    sqlTransaction.Commit();
                    dataChanged = true;
                }
                catch (ValidationException ex)
                {
                    log.LogVariableState("accountDTO", accountDTO);
                    log.LogVariableState("customerDTO", customerDTO);
                    log.Error("Validation failed", ex);
                    log.Error(ex.GetAllValidationErrorMessages());
                    sqlTransaction.Rollback();
                    MessageBox.Show(ex.GetAllValidationErrorMessages());
                }
                catch (Exception ex)
                {
                    log.LogVariableState("accountDTO", accountDTO);
                    log.Error("Error occurred while saving account", ex);
                    sqlTransaction.Rollback();
                    MessageBox.Show(ex.Message);
                }
            }
            if (dataChanged)
            {
                btnRefresh.PerformClick();
            }
            log.LogMethodExit();
        }

        private void IssueAccount_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (serialPort != null)
            {
                System.Threading.Thread.Sleep(100);
                serialPort.Open();
            }
            log.LogMethodExit();
        }

        private void IssueAccount_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (serialPort != null)
                serialPort.Close();
            log.LogMethodExit();
        }

        private void textBoxCardNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (char.IsLetterOrDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.KeyChar = char.ToUpper(e.KeyChar);
            else
                e.Handled = true;
            log.LogMethodExit();
        }

        private void btnClearTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtStartTime.Text = "";
            txtLastPlayedTime.Text = "";
            log.LogMethodExit();
        }

        private void textBoxCardNumber_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (string.IsNullOrEmpty(textBoxCardId.Text))
                checkCardExisting();
            log.LogMethodExit();
        }

        private void btnGames_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (accountDTO.AccountId >= 0)
            {
                UpdateAccountDTO();
                if (accountDTO.IsChanged)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1134));
                    log.LogMethodExit(null, "Unsaved changes exist. Please save");
                    return;
                }
                try
                {
                    AccountGameListUI accountGameListUI = new AccountGameListUI(utilities, accountDTO.AccountId);
                    accountGameListUI.ShowDialog();
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while displaying account games", ex);
                }
                btnRefresh.PerformClick();
            }
            log.LogMethodExit();
        }

        private void btnDiscounts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (accountDTO.AccountId >= 0)
            {
                UpdateAccountDTO();
                if (accountDTO.IsChanged)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1134));
                    log.LogMethodExit(null, "Unsaved changes exist. Please save");
                    return;
                }
                try
                {
                    using (AccountDiscountListUI accountDiscountListUI = new AccountDiscountListUI(utilities, accountDTO.AccountId))
                    {
                        accountDiscountListUI.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while displaying account discounts", ex);
                }
                btnRefresh.PerformClick();
            }
            log.LogMethodExit();
        }

        private string Secret = "";

        private void IssueAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if ((int)e.KeyChar == 3) // secret key to enable site key update
            {
                Secret = "C";
            }
            else
                if ((int)e.KeyChar == 18 && Secret == "C") // secret key to enable credit field update
            {
                Secret = "CR";
            }
            else
                if ((int)e.KeyChar == 5 && Secret == "CR")
            {
                Secret = "CRE";
            }
            else
                if ((int)e.KeyChar == 4 && Secret == "CRE")
            {
                grpEntitlements.Enabled = true;
                chkVIPCustomer.Enabled = true;
                Secret = "";
            }
            else
            {
                Secret = "";
            }
            log.LogMethodExit();
        }

        private void chkTimerResetCard_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (chkTimerResetCard.Checked)
            {
                cmbTechCardType.SelectedValue = "Y";
                cmbTechCardType.Enabled = false;
            }
            else
                cmbTechCardType.Enabled = true;
            log.LogMethodExit();
        }

        private void btnCreditPlus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (accountDTO.AccountId >= 0)
            {
                UpdateAccountDTO();
                if (accountDTO.IsChanged)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1134));
                    log.LogMethodExit(null, "Unsaved changes exist. Please save");
                    return;
                }
                try
                {
                    using (AccountCreditPlusListUI accountGameListUI = new AccountCreditPlusListUI(utilities, accountDTO.AccountId))
                    {
                        accountGameListUI.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while displaying account credit plus", ex);
                }
                btnRefresh.PerformClick();
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (accountDTO.AccountId >= 0)
            {
                AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountDTO.AccountId);
                accountDTO = accountBL.AccountDTO;
                DisplayAccountDetails();
            }
            log.LogMethodExit();
        }

        private void lnkAuditTrail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (accountDTO.AccountId >= 0)
            {
                try
                {
                    using (AccountAuditListUI accountAuditListUI = new AccountAuditListUI(utilities, accountDTO))
                    {
                        accountAuditListUI.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while displaying account audit trail", ex);
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void lnkActivities_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (accountDTO.AccountId >= 0)
            {
                try
                {
                    using (AccountActivityListUI accountActivityListUI = new AccountActivityListUI(utilities, accountDTO.AccountId, accountDTO.TagNumber))
                    {
                        accountActivityListUI.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while showing account activity view", ex);
                }
            }
            log.LogMethodExit();
        }

        private void btnAddToCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (accountDTO.AccountId >= 0)
            {
                UpdateAccountDTO();
                if (accountDTO.IsChanged)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1134));
                    log.LogMethodExit(null, "Unsaved changes exist. Please save");
                    return;
                }

                using (AddToAccountUI addToAccountUI = new AddToAccountUI(utilities, accountDTO.TechnicianCard.Equals("Y")))
                {
                    if (addToAccountUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (addToAccountUI.Credits.HasValue)
                        {
                            if (accountDTO.Credits.HasValue)
                            {
                                accountDTO.Credits = accountDTO.Credits.Value + addToAccountUI.Credits.Value;
                            }
                            else
                            {
                                accountDTO.Credits = addToAccountUI.Credits;
                            }
                        }
                        if (addToAccountUI.Courtesy.HasValue)
                        {
                            if (accountDTO.Courtesy.HasValue)
                            {
                                accountDTO.Courtesy = accountDTO.Courtesy.Value + addToAccountUI.Courtesy.Value;
                            }
                            else
                            {
                                accountDTO.Courtesy = addToAccountUI.Courtesy;
                            }
                        }
                        if (addToAccountUI.Bonus.HasValue)
                        {
                            if (accountDTO.Bonus.HasValue)
                            {
                                accountDTO.Bonus = accountDTO.Bonus.Value + addToAccountUI.Bonus.Value;
                            }
                            else
                            {
                                accountDTO.Bonus = addToAccountUI.Bonus;
                            }
                        }
                        if (addToAccountUI.Time.HasValue)
                        {
                            if (accountDTO.Time.HasValue)
                            {
                                accountDTO.Time = accountDTO.Time.Value + addToAccountUI.Time.Value;
                            }
                            else
                            {
                                accountDTO.Time = addToAccountUI.Time;
                            }
                        }
                        if (addToAccountUI.TechGames.HasValue)
                        {
                            if (accountDTO.TechGames.HasValue)
                            {
                                accountDTO.TechGames = accountDTO.TechGames.Value + addToAccountUI.TechGames.Value;
                            }
                            else
                            {
                                accountDTO.TechGames = addToAccountUI.TechGames;
                            }
                        }
                        if (addToAccountUI.TicketCount.HasValue)
                        {
                            if (accountDTO.TicketCount.HasValue)
                            {
                                accountDTO.TicketCount = accountDTO.TicketCount.Value + addToAccountUI.TicketCount.Value;
                            }
                            else
                            {
                                accountDTO.TicketCount = addToAccountUI.TicketCount;
                            }
                        }
                        if (addToAccountUI.AccountGameDTOList != null &&
                            addToAccountUI.AccountGameDTOList.Count > 0)
                        {
                            if (accountDTO.AccountGameDTOList == null)
                            {
                                accountDTO.AccountGameDTOList = new List<AccountGameDTO>();
                            }
                            accountDTO.AccountGameDTOList.AddRange(addToAccountUI.AccountGameDTOList);
                        }
                        if (accountDTO.IsChangedRecursive)
                        {
                            SqlConnection connection = utilities.getConnection();
                            SqlTransaction transaction = connection.BeginTransaction();
                            try
                            {
                                AccountBL accountBL = new AccountBL(utilities.ExecutionContext, accountDTO);
                                accountBL.SaveManualChanges(savePreviousAccountStateInAccountAudit, transaction);
                                transaction.Commit();
                                savePreviousAccountStateInAccountAudit = false;
                                dataChanged = true;
                            }
                            catch (ValidationException ex)
                            {
                                log.LogVariableState("accountDTO", accountDTO);
                                log.Error("Validation failed", ex);
                                log.Error(ex.GetAllValidationErrorMessages());
                                MessageBox.Show(ex.GetAllValidationErrorMessages());
                                transaction.Rollback();
                                log.LogMethodExit("", "Validation Exception Occurred while saving account");
                                return;
                            }
                            catch (Exception ex)
                            {
                                log.LogVariableState("accountDTO", accountDTO);
                                log.Error("Error occurred while saving the account", ex);
                                MessageBox.Show(ex.Message);
                                transaction.Rollback();
                                log.LogMethodExit("", "Exception Occurred while saving account");
                                return;
                            }
                        }
                    }
                }
                btnRefresh.PerformClick();
                if (dataChanged)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 548));
                }
            }
            log.LogMethodExit();
        }

        private void lnkBulkUpload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                using (TagSerialMappingListUI tagSerialMappingListUI = new TagSerialMappingListUI(utilities))
                {
                    tagSerialMappingListUI.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while tag serial mapping", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void IssueAccountUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while disposing the form", ex);
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableCardsModule)
            {
                btnSave.Enabled = true;
                btnAddToCard.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                btnAddToCard.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}