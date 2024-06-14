/********************************************************************************************
 * Project Name - Token Card Inventory UI
 * Description  - frmTokenCardInventory
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80.0        17-Feb-2019   Deeksha        Modified to Make DigitalSignage module as
 *                                           read only in Windows Management Studio.
 *2.90.0        20-Aug-2020   Guru S A       Fix for call from POS to enable buttons even if card module is turned off in Mgmt studio
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    public partial class frmTokenCardInventory : Form
    {
        DateTime lastSundayDate;
        String File_name = "";
        Utilities Utilities;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool showOnlytokenInventory = false;
        int tagTypeToken = -1;
        int tagTypeCard = -1;

        int activityTypeOnHand = -1;
        int activityTypePurchase = -1;
        int activityTypeTransfer = -1;
        int activityTypeOther = -1;

        int posMachineType = -1;
        int kioskMachineType = -1;
        int otherMachineType = -1;
        private ManagementStudioSwitch managementStudioSwitch; 

        public frmTokenCardInventory(Utilities _utilities, bool showOnlytokenInventory)
        {
            log.LogMethodEntry(showOnlytokenInventory);
            InitializeComponent();
            Utilities = _utilities;
            this.showOnlytokenInventory = showOnlytokenInventory;
            if (Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }

            Utilities.setupDataGridProperties(ref dgvStock);
            machineUserContext.SetUserId(Utilities.ParafaitEnv.Username);

            if (showOnlytokenInventory)
            {
                tc_cards.TabPages.RemoveByKey("tp_reports");
                tc_cards.TabPages.RemoveByKey("tp_delete");
                tc_cards.TabPages.RemoveByKey("tp_add");
            } 
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void frm_cardMaintenance_Load(object sender, EventArgs e)
        {
            lblDate.Text = lb_del_date.Text = System.DateTime.Now.ToString(Utilities.getDateFormat());
            lblUser.Text = lb_del_user.Text = Utilities.ParafaitEnv.Username;
            dtp_fromDate.CustomFormat = dtp_toDate.CustomFormat = Utilities.getDateFormat();
            rboTillDate.Checked = true;

            actiondateDataGridViewTextBoxColumn.DefaultCellStyle =
            lastUpdatedDateDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            populateCardStock();

            UpdateLookUpValues();

            string businessDayStartTime = Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME");
            int businessHour = 6;

            if (!string.IsNullOrEmpty(businessDayStartTime))
            {
                try
                {
                    businessHour = Convert.ToInt32(businessDayStartTime);
                }
                catch
                {
                    businessHour = 6;
                }
            }
            //check today is monday and buisness hour not crossed the get last 2 sunday date
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && DateTime.Now.Hour < businessHour)
            {
                lastSundayDate = DateTime.Today.Date.AddDays(-8).AddHours(businessHour);
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)//If Today is sunday consider last sunday
            {
                lastSundayDate = DateTime.Today.Date.AddDays(-7).AddHours(businessHour);
            }
            else
            {
                DateTime input = DateTime.Today.Date;
                int days = DayOfWeek.Sunday - input.DayOfWeek;
                lastSundayDate = input.AddDays(days).AddHours(businessHour);
            }

            if (tc_cards.SelectedTab.Text == "tp_reports")
            {
                GenerateReport();
            }
            else if (tc_cards.SelectedTab.Name == "tp_CardInventory")
            {
                UpdateCardInventoryTab();
            }

            Utilities.setLanguage(this);
        }

        void UpdateLookUpValues()
        {
            tagTypeToken = GetLookupValuesDTO("TAG_TYPE", "TOKEN").LookupValueId;
            tagTypeCard = GetLookupValuesDTO("TAG_TYPE", "CARD").LookupValueId;

            activityTypeOnHand = GetLookupValuesDTO("ACTIVITY_TYPE", "ON_HAND").LookupValueId;
            activityTypePurchase = GetLookupValuesDTO("ACTIVITY_TYPE", "PURCHASE").LookupValueId;
            activityTypeTransfer = GetLookupValuesDTO("ACTIVITY_TYPE", "TRANSFER").LookupValueId;
            activityTypeOther = GetLookupValuesDTO("ACTIVITY_TYPE", "OTHER").LookupValueId;

            posMachineType = GetLookupValuesDTO("MACHINE_TYPE", "POS").LookupValueId;
            kioskMachineType = GetLookupValuesDTO("MACHINE_TYPE", "KIOSK").LookupValueId;
            otherMachineType = GetLookupValuesDTO("MACHINE_TYPE", "OTHER").LookupValueId;
        }

        private void populateCardStock()
        {
            int card_issued = GetCardsIssued();
            int card_stock = GetCardStock();
            int cards_balance = 0;

            lb_inventory.Text = card_stock.ToString("N0");
            lb_issued_td.Text = card_issued.ToString("N0");
            cards_balance = card_stock - card_issued;
            lb_cards.Text = cards_balance.ToString("N0");
        }

        private int GetCardsIssued()
        {
            TokenCardInventory tokenInventory = new TokenCardInventory(machineUserContext);
            return tokenInventory.GetCardsIssued(Utilities.ParafaitEnv.SiteId);
        }

        private int GetCardStock()
        {
            TokenCardInventory tokenInventory = new TokenCardInventory(machineUserContext);
            return tokenInventory.GetCardStock(Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1);
        }

        private void GenerateReport()
        {
            TokenCardInventoryList tokenInventoryList = new TokenCardInventoryList(machineUserContext);
            List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParams = new List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>>();
            searchParams.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.SITE_ID, Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId.ToString() : "-1"));
            List<TokenCardInventoryDTO> lstTokenInventoryDTO;

            if (rboTillDate.Checked)
            {
                lstTokenInventoryDTO = tokenInventoryList.GetReportTokenInventoryList(searchParams);
            }
            else
            {
                searchParams.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.FROM_DATE, dtp_fromDate.Value.ToString("MM-dd-yyyy")));
                searchParams.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TO_DATE, dtp_toDate.Value.ToString("MM-dd-yyyy")));
                lstTokenInventoryDTO = tokenInventoryList.GetReportTokenInventoryList(searchParams);
                File_name = "Card Inventory Report - From " + dtp_fromDate.Text.ToString() + " to " + dtp_toDate.Text.ToString() + " as of " + System.DateTime.Now.ToString("dd-MMM-yyyy");
            }

            BindingSource tokenInventoryListBS = new BindingSource();
            tokenInventoryListBS.DataSource = lstTokenInventoryDTO;
            dgvStock.DataSource = tokenInventoryListBS;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCardSave_Click(object sender, EventArgs e)
        {
            try
            {
                int card_no = 0;
                try
                {
                    card_no = Convert.ToInt32(txtNoOfCards.Text);
                }
                catch
                {
                    MessageBox.Show(Utilities.MessageUtils.getMessage(587), Utilities.MessageUtils.getMessage("Add Cards"));
                    return;
                }

                TokenCardInventoryDTO tokenCardInventoryDTO = new TokenCardInventoryDTO();
                tokenCardInventoryDTO.FromSerialNumber = txtFromSerial.Text;
                tokenCardInventoryDTO.ToserialNumber = txtToSerial.Text;
                tokenCardInventoryDTO.Number = card_no;
                tokenCardInventoryDTO.Action = "Add";

                TokenCardInventory tokenInventory = new TokenCardInventory(machineUserContext, tokenCardInventoryDTO);
                tokenInventory.Save();

                populateCardStock();
                txtToSerial.Text = "";
                txtFromSerial.Text = "";
                txtNoOfCards.Text = "";
                MessageBox.Show(Utilities.MessageUtils.getMessage(549), Utilities.MessageUtils.getMessage("Add Cards"));
            }
            catch
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(588), Utilities.MessageUtils.getMessage("Add Cards"));
            }
        }

        private void BtnCardClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelSave_Click(object sender, EventArgs e)
        {
            try
            {
                int card_no = 0;
                try
                {
                    card_no = -1 * Convert.ToInt32(txtDelNumber.Text);
                }
                catch
                {
                    MessageBox.Show(Utilities.MessageUtils.getMessage(587), Utilities.MessageUtils.getMessage("Delete Cards"));
                    return;
                }

                TokenCardInventoryDTO tokenCardInventoryDTO = new TokenCardInventoryDTO();
                tokenCardInventoryDTO.Number = card_no;
                tokenCardInventoryDTO.Action = "Reduce";

                TokenCardInventory tokenInventory = new TokenCardInventory(machineUserContext, tokenCardInventoryDTO);
                tokenInventory.Save();

                populateCardStock();

                txtDelNumber.Text = "";
                MessageBox.Show(Utilities.MessageUtils.getMessage(550), Utilities.MessageUtils.getMessage("Reduce Cards"));
            }
            catch
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(588), Utilities.MessageUtils.getMessage("Reduce Cards"));
            }
        }

        private void txtNoOfCards_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void BtnDisk_Click(object sender, EventArgs e)
        {
            if (dgvStock.Rows.Count < 1)
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(371), Utilities.MessageUtils.getMessage("Save to Disk"));
            }
            else
            {
                Utilities.ExportToExcel(dgvStock, File_name, "Card Stock");
            }
        }

        private void tc_cards_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            ShowHideControls(true);
            if (tc_cards.SelectedTab.Name == "tp_reports")
            {
                GenerateReport();
            }
            else if (tc_cards.SelectedTab.Name == "tp_CardInventory")
            {
                UpdateCardInventoryTab();
            }
        }

        void ShowHideControls(bool flag)
        {
            lblCardInventory.Visible = flag;
            lb_inventory.Visible = flag;
            lblCardIssued.Visible = flag;
            lb_issued_td.Visible = flag;
            lblCardBalance.Visible = flag;
            lb_cards.Visible = flag;
        }

        void UpdateCardInventoryTab()
        {
            ShowHideControls(false);

            lblSiteName.Text = Utilities.ParafaitEnv.SiteName;
            lblTechName.Text = Utilities.ParafaitEnv.Username;

            lblWeekendDate.Text = Utilities.MessageUtils.getMessage("Sunday") + "(" + lastSundayDate.ToString(Utilities.getDateFormat()) + ")";

            PopulateTokenCardInventory();
        }

        void ClearFields()
        {
            txtTokenKiosk.Tag = null;
            txtTokenKiosk.Text = "";

            txtTokenPOS.Tag = null;
            txtTokenPOS.Text = "";

            txtTokenHand.Tag = null;
            txtTokenHand.Text = "";

            txtCardsOnHand.Tag = null;
            txtCardsOnHand.Text = "";

            txtCardPurchased.Tag = null;
            txtCardPurchased.Text = "";

            txtTransferredToken.Tag = null;
            txtTransferredToken.Text = "";
        }

        private void btnTokenCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        LookupValuesDTO GetLookupValuesDTO(string lookupName, string lookupValue)
        {
            LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> SearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            SearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(SearchParameters);

            if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
            {
                LookupValuesDTO posLookUpDTO = lookupValuesDTOList.Find(x => x.LookupValue == lookupValue);

                if (posLookUpDTO != null)
                {
                    return posLookUpDTO;
                }
            }
            return new LookupValuesDTO();
        }

        bool ValidateLookupValues()
        {
            if (posMachineType == -1 || kioskMachineType == -1 || otherMachineType == -1 //Check MachineType
                || activityTypeOnHand == -1 || activityTypeTransfer == -1 || activityTypePurchase == -1 || activityTypeOther == -1 // Check activityType 
                || tagTypeCard == -1 || tagTypeToken == -1) //Check TagType
            {
                return false;
            }

            return true;
        }

        void PopulateTokenCardInventory()
        {
            lblMessage.Text = "";
            if (ValidateLookupValues())
            {
                TokenCardInventoryList tokenInventoryList = new TokenCardInventoryList(machineUserContext);
                List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParams = new List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>>();
                searchParams.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.SITE_ID, Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId.ToString() : "-1"));
                searchParams.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.DATE, lastSundayDate.ToString("MM-dd-yyyy hh")));
                List<TokenCardInventoryDTO> lstTokenInventoryDTO = tokenInventoryList.GetAllTokenCardInventory(searchParams);

                TokenCardInventoryDTO tokenCardInventoryDTO;
                if (lstTokenInventoryDTO != null && lstTokenInventoryDTO.Count > 0)
                {
                    tokenCardInventoryDTO = lstTokenInventoryDTO.Find(x => x.MachineType == posMachineType);
                    if (tokenCardInventoryDTO != null)
                    {
                        txtTokenPOS.Text = tokenCardInventoryDTO.Number.ToString();
                        txtTokenPOS.Tag = tokenCardInventoryDTO;
                    }

                    tokenCardInventoryDTO = lstTokenInventoryDTO.Find(x => x.MachineType == kioskMachineType);
                    if (tokenCardInventoryDTO != null)
                    {
                        txtTokenKiosk.Text = tokenCardInventoryDTO.Number.ToString();
                        txtTokenKiosk.Tag = tokenCardInventoryDTO;
                    }

                    tokenCardInventoryDTO = lstTokenInventoryDTO.Find(x => x.TagType == tagTypeToken && x.MachineType == otherMachineType && x.ActivityType == activityTypeOnHand);
                    if (tokenCardInventoryDTO != null)
                    {
                        txtTokenHand.Text = tokenCardInventoryDTO.Number.ToString();
                        txtTokenHand.Tag = tokenCardInventoryDTO;
                    }

                    tokenCardInventoryDTO = lstTokenInventoryDTO.Find(x => x.TagType == tagTypeToken && x.MachineType == otherMachineType && x.ActivityType == activityTypeTransfer);
                    if (tokenCardInventoryDTO != null)
                    {
                        txtTransferredToken.Text = tokenCardInventoryDTO.Number.ToString();
                        txtTransferredToken.Tag = tokenCardInventoryDTO;
                    }

                    tokenCardInventoryDTO = lstTokenInventoryDTO.Find(x => x.TagType == tagTypeCard && x.MachineType == otherMachineType && x.ActivityType == activityTypeOnHand);
                    if (tokenCardInventoryDTO != null)
                    {
                        txtCardsOnHand.Text = tokenCardInventoryDTO.Number.ToString();
                        txtCardsOnHand.Tag = tokenCardInventoryDTO;
                    }

                    tokenCardInventoryDTO = lstTokenInventoryDTO.Find(x => x.TagType == tagTypeCard && x.MachineType == otherMachineType && x.ActivityType == activityTypePurchase);
                    if (tokenCardInventoryDTO != null)
                    {
                        txtCardPurchased.Text = tokenCardInventoryDTO.Number.ToString();
                        txtCardPurchased.Tag = tokenCardInventoryDTO;
                    }
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(1198), Utilities.MessageUtils.getMessage("Validation"));
            }
        }

        private void btnTokenSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = "";
                if (ValidateLookupValues())
                {
                    TokenCardInventory tokenInventoryBL;
                    TokenCardInventoryDTO tokenInventoryDTO;

                    #region Save Token Collected KIOSK
                    if (!string.IsNullOrEmpty(txtTokenKiosk.Text.Trim()))
                    {
                        if (txtTokenKiosk.Tag != null)
                        {
                            tokenInventoryDTO = txtTokenKiosk.Tag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(txtTokenKiosk.Text);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(txtTokenKiosk.Text);
                            tokenInventoryDTO.TagType = tagTypeToken;
                            tokenInventoryDTO.MachineType = kioskMachineType;
                            tokenInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenInventoryDTO.Action = "ADD";
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                        }

                        tokenInventoryBL = new TokenCardInventory(machineUserContext, tokenInventoryDTO);
                        tokenInventoryBL.Save();
                    }
                    #endregion

                    #region Save Token Collected POS
                    if (!string.IsNullOrEmpty(txtTokenPOS.Text.Trim()))
                    {
                        if (txtTokenPOS.Tag != null)
                        {
                            tokenInventoryDTO = txtTokenPOS.Tag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(txtTokenPOS.Text);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(txtTokenPOS.Text);
                            tokenInventoryDTO.TagType = tagTypeToken;
                            tokenInventoryDTO.MachineType = posMachineType;
                            tokenInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenInventoryDTO.Action = "ADD";
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                        }

                        tokenInventoryBL = new TokenCardInventory(machineUserContext, tokenInventoryDTO);
                        tokenInventoryBL.Save();
                    }
                    #endregion

                    #region Save remaining tokens
                    if (!string.IsNullOrEmpty(txtTokenHand.Text.Trim()))
                    {
                        if (txtTokenHand.Tag != null)
                        {
                            tokenInventoryDTO = txtTokenHand.Tag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(txtTokenHand.Text);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(txtTokenHand.Text);
                            tokenInventoryDTO.TagType = tagTypeToken;
                            tokenInventoryDTO.MachineType = otherMachineType;
                            tokenInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenInventoryDTO.Action = "ADD";
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                        }

                        tokenInventoryBL = new TokenCardInventory(machineUserContext, tokenInventoryDTO);
                        tokenInventoryBL.Save();
                    }
                    #endregion

                    #region Save Transferred tokens
                    if (!string.IsNullOrEmpty(txtTransferredToken.Text.Trim()))
                    {
                        if (txtTransferredToken.Tag != null)
                        {
                            tokenInventoryDTO = txtTransferredToken.Tag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(txtTransferredToken.Text); ;
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(txtTransferredToken.Text); ;
                            tokenInventoryDTO.TagType = tagTypeToken;
                            tokenInventoryDTO.MachineType = otherMachineType;
                            tokenInventoryDTO.ActivityType = activityTypeTransfer;
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                        }

                        if (tokenInventoryDTO.Number > 0)
                        {
                            tokenInventoryDTO.Action = "ADD";
                        }
                        else
                        {
                            tokenInventoryDTO.Action = "REMOVE";
                        }

                        tokenInventoryBL = new TokenCardInventory(machineUserContext, tokenInventoryDTO);
                        tokenInventoryBL.Save();
                    }
                    #endregion

                    #region Save Cards on Hand
                    if (!string.IsNullOrEmpty(txtCardsOnHand.Text.Trim()))
                    {
                        if (txtCardsOnHand.Tag != null)
                        {
                            tokenInventoryDTO = txtCardsOnHand.Tag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(txtCardsOnHand.Text);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(txtCardsOnHand.Text);
                            tokenInventoryDTO.TagType = tagTypeCard;
                            tokenInventoryDTO.MachineType = otherMachineType;
                            tokenInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                            tokenInventoryDTO.Action = "ADD";
                        }

                        tokenInventoryBL = new TokenCardInventory(machineUserContext, tokenInventoryDTO);
                        tokenInventoryBL.Save();
                    }
                    #endregion

                    #region Save Cards Purchased
                    if (!string.IsNullOrEmpty(txtCardPurchased.Text.Trim()))
                    {
                        if (txtCardPurchased.Tag != null)
                        {
                            tokenInventoryDTO = txtCardPurchased.Tag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(txtCardPurchased.Text);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(txtCardPurchased.Text);
                            tokenInventoryDTO.TagType = tagTypeCard;
                            tokenInventoryDTO.MachineType = otherMachineType;
                            tokenInventoryDTO.ActivityType = activityTypePurchase;
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                            tokenInventoryDTO.Action = "ADD";
                        }

                        tokenInventoryBL = new TokenCardInventory(machineUserContext, tokenInventoryDTO);
                        tokenInventoryBL.Save();
                    }
                    #endregion

                    PopulateTokenCardInventory();
                    lblMessage.Text = Utilities.MessageUtils.getMessage(1197, "Inventory details");
                }
                else
                {
                    MessageBox.Show(Utilities.MessageUtils.getMessage(1198), Utilities.MessageUtils.getMessage("Validation"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtTransferredToken_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTransferredToken.Text.Trim()))
                {
                    Convert.ToInt32(txtTransferredToken.Text);
                }
                e.Cancel = false;
            }
            catch
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(648), Utilities.MessageUtils.getMessage("Validation"));
                e.Cancel = true;
            }
        }

        private void txtTransferredToken_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTokenHand_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTokenHand.Text.Trim()))
                {
                    Convert.ToInt32(txtTokenHand.Text);
                }
                e.Cancel = false;
            }
            catch
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(648), Utilities.MessageUtils.getMessage("Validation"));
                e.Cancel = true;
            }
        }

        private void txtTokenPOS_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTokenPOS.Text.Trim()))
                {
                    Convert.ToInt32(txtTokenPOS.Text);
                }
                e.Cancel = false;
            }
            catch
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(648), Utilities.MessageUtils.getMessage("Validation"));
                e.Cancel = true;
            }
        }

        private void txtTokenKiosk_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTokenKiosk.Text.Trim()))
                {
                    Convert.ToInt32(txtTokenKiosk.Text);
                }
                e.Cancel = false;
            }
            catch
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(648), Utilities.MessageUtils.getMessage("Validation"));
                e.Cancel = true;
            }
        }

        private void txtCardsOnHand_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCardsOnHand.Text.Trim()))
                {
                    Convert.ToInt32(txtCardsOnHand.Text);
                }
                e.Cancel = false;
            }
            catch
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(648), Utilities.MessageUtils.getMessage("Validation"));
                e.Cancel = true;
            }
        }

        private void txtCardPurchased_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCardPurchased.Text.Trim()))
                {
                    Convert.ToInt32(txtCardPurchased.Text);
                }
                e.Cancel = false;
            }
            catch
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(648), Utilities.MessageUtils.getMessage("Validation"));
                e.Cancel = true;
            }
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableCardsModule || showOnlytokenInventory == true)
            {
                btnCardSave.Enabled = true;
                btnDelSave.Enabled = true;
                btnTokenSave.Enabled = true;
            }
            else
            {
                btnCardSave.Enabled = false;
                btnDelSave.Enabled = false;
                btnTokenSave.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
