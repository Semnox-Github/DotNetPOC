/********************************************************************************************
 * Project Name - Parafait POS
 * Description  - Petty Add Cash/Card To Shift
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
*2.70.0       17-Jul-2019     Divya A        Add card/cash to shift 
*2.70.0      27-Jul-2019      Divya A        Fixes 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.User;
using System.Collections.Generic;

namespace Parafait_POS
{
    public partial class frmAddCashCardToShift : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities Utilities;
        private string message = string.Empty;
        private MessageUtils MessageUtils = POSStatic.MessageUtils;
        private int userId;
        Security.User user;
        private TextBox currentTextBox = null;
        private AlphaNumericKeyPad keypad;
        private bool openDrawer = false;

        public string Message { get { return message; } set { message = value; } }
        public bool OpenDrawer { get { return openDrawer; } set { openDrawer = value; } }
        public frmAddCashCardToShift(Utilities utilities, int userId)
        {
            log.LogMethodEntry("Enter Add to shift constructor");
            InitializeComponent();
            this.Utilities = utilities;
            this.userId = userId;
            currentTextBox = txtCash;
            this.Utilities.setLanguage(this);
            log.LogMethodExit("Exit Constructor");
        }

        private void BtnOkay_Click(object sender, EventArgs e)
        {
            string action = string.Empty, reason = string.Empty, remarks = string.Empty;
            double shiftAmount = 0.0;
            int cardCount = 0;            
            log.LogMethodEntry("Enter click add cash-card in/out");
            if (!rbIN.Checked && !rbOUT.Checked)
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Select a valid option : IN/OUT"));
                return;
            }else if (rbIN.Checked)
            {
                action = "Paid In";
            }
            else if (rbOUT.Checked)
            {
                action = "Paid Out";
            }

            if (string.IsNullOrEmpty(txtCash.Text) && string.IsNullOrEmpty(txtCards.Text))
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Enter amount or number of cards"));
                return;
            }

            if((!String.IsNullOrEmpty(txtCash.Text) &&!double.TryParse(txtCash.Text, out shiftAmount)) || (shiftAmount < 0 || (shiftAmount > 0 && shiftAmount < 0.01)))
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Enter valid amount"));
                return;
            }
            
            if((!String.IsNullOrEmpty(txtCards.Text) && !int.TryParse(txtCards.Text, out cardCount)) || (cardCount < 0 || (cardCount > 0 && cardCount < 1)))
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Enter valid number of cards."));
                return;
            }

            if (ReasonComboBox.SelectedIndex < 0)
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Select a reason for cash-in cash-out."));
                return;
            }
            else
            {
                reason = ((LookupValuesDTO)(ReasonComboBox.SelectedItem)).Description;
            }

            if (txtRemarks.Text.Length >= 2000)
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Please enter Remarks under 2000 characters"));
                return;
            }
            else if (!string.IsNullOrEmpty(txtRemarks.Text))
            {
                remarks = txtRemarks.Text;
            }

            double maxCashLimit = String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "MAXIMUM_PETTY_CASH_LIMIT")) ? 5000 : Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "MAXIMUM_PETTY_CASH_LIMIT"));
            double maxCashLimitForRole = String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PETTY_CASH_LIMIT_FOR_MANAGER_APPROVAL")) ? 0.01 : Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PETTY_CASH_LIMIT_FOR_MANAGER_APPROVAL"));
            int maxCardLimit = String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "MAXIMUM_PETTY_CARD_LIMIT")) ? 1000 : Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "MAXIMUM_PETTY_CARD_LIMIT"));
            int maxCardLimitForRole = String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PETTY_CARD_LIMIT_FOR_MANAGER_APPROVAL")) ? 1 : Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PETTY_CARD_LIMIT_FOR_MANAGER_APPROVAL"));
            try
            {
                if (shiftAmount <= maxCashLimit && cardCount <= maxCardLimit)
                {
                    Users user = new Users(Utilities.ExecutionContext, userId);
                    if ((shiftAmount >= maxCashLimitForRole) || (cardCount >= maxCardLimitForRole))
                    {
                        if (!Authenticate.Manager(ref userId))
                        {
                            Message = MessageUtils.getMessage(268);
                            this.Close();
                            return;
                        }
                        user = new Users(Utilities.ExecutionContext,userId);
                        POSUtils.AddToShift(ref message, action, shiftAmount, cardCount, reason, remarks, user.UserDTO.LoginId, Utilities.getServerTime());
                    }
                    else
                    {
                        POSUtils.AddToShift(ref message, action, shiftAmount, cardCount, reason, remarks, user.UserDTO.LoginId, Utilities.getServerTime());
                    }
                    OpenDrawer = true;
                    this.Close();
                    log.LogMethodExit("Exit Add cash-card in/out." + message);
                }
                else
                {
                    OpenDrawer = false;
                    POSUtils.ParafaitMessageBox("You have exceeded the maximum limit");
                    return;
                }
            }
            catch(Exception ex)
            {
                OpenDrawer = false;
                this.Close();
                log.LogMethodExit("Exit Add cash-card in/out." + ex);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry("AddtoShift cancel clicked");
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }
        
        private void BtnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, keypad == null ? currentTextBox : keypad.currentTextBox);
                keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Bottom - keypad.Height + 40);
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

        private void TxtCash_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            currentTextBox = txtCash;
            if (keypad != null)
                keypad.currentTextBox = txtCash;
            log.LogMethodExit();
        }

        private void TxtCards_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            currentTextBox = txtCards;
            if (keypad != null)
                keypad.currentTextBox = txtCards;
            log.LogMethodExit();
        }

        private void TxtRemarks_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            currentTextBox = txtRemarks;
            if (keypad != null)
                keypad.currentTextBox = txtRemarks;
            log.LogMethodExit();
        }

        private void frmAddCashCardToShift_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "PETTY_CASH_IN_OUT_REASONS"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> pettyCashInOutLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            ReasonComboBox.DataSource = pettyCashInOutLookUpValueList;
            log.LogMethodExit();
        }
    }
}
