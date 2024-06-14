/********************************************************************************************
 * Project Name - frmLegacyCardToParafait
 * Description  - frmLegacyCardToParafait class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00         28-Mar-2019      Raghuveera          Moved this new form from legacy application to parafait POS
 * 2.70        1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards 
 * 2.80        10-Oct-2019      Girish Kundar       Modified: Keyboard Location issue fix
 * 2.100        1-Sep-2020       Dakshakh raj        Legacy gift card enhancement
 *2.130.4     22-Feb-2022       Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class frmLegacyCardToParafait : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool transferred = false;
        bool allowMultipleLegacyCards = false;
        bool existingParafaitCard = false;
        int existingParafaitCardId = 0;
        string transfer_to_card = "";
        bool isUnisSystem = false;
        bool isClubSpeed = false;
        List<LookupValuesDTO> lookupValuesDTOList;
        private readonly TagNumberParser tagNumberParser;
        LegacyCardDTO legacyCardDTO = null;
        int managerId = -1;
        bool LegacyCardDetails = false;
        Utilities Utilities = POSStatic.Utilities;
        List<ValidationError> ValidationErrorList = new List<ValidationError>();
        string errorMessage = "";

        public frmLegacyCardToParafait()
        {
            log.LogMethodEntry();
            InitializeComponent();
            LoadLegacyConfiguration();
            isUnisSystem = lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("IS_UNIS_SYSTEM")).ToList<LookupValuesDTO>()[0].Description.Equals("Y");
            isClubSpeed = lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("IS_CLUB_SPEED_ENVIRONMENT")).ToList<LookupValuesDTO>()[0].Description.Equals("Y");
            if (isUnisSystem && isClubSpeed)
            {
                log.Debug("IS_UNIS_SYSTEM & IS_CLUB_SPEED_ENVIRONMENT Both are Enabled");
            }
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            SetEditBtnVisibility(false);
            log.LogMethodExit();
        }

        private void SetEditBtnVisibility(bool isEnabled = false)
        {
            log.LogMethodEntry(isEnabled);
            btnEditCredits.Visible = btnEditBonus.Visible = btnEditTime.Visible = btnEditTickets.Visible = btnEditLoyaltyPoints.Visible = btnEditGames.Visible = isEnabled;
            log.LogMethodExit();
        }

        private void LoadLegacyConfiguration()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(POSStatic.Utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "LEGACY_CARD_TRANSFER_CONFIGURATIONS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, POSStatic.Utilities.ExecutionContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            }
            catch (Exception e)
            {
                log.Error(e); 
            }
            log.LogMethodExit();
        }

        private void frmLegacyCashToParafait_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            errorMessage = "";
            try
            {
                chkIncludePackages.Checked = Convert.ToBoolean(lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("LoadPackages")).ToList<LookupValuesDTO>()[0].Description); //Properties.Settings.Default.LoadPackages;
            }
            catch
            {
                chkIncludePackages.Checked = false;
            }

            btnClear.PerformClick();
            txtLegacyCardNumber.Focus();

            ParafaitCon = POSStatic.Utilities.getConnection();

            RegisterMifareDevice();
            if (isUnisSystem)
            {
                btnTransfer.Enabled = false;
                btnInitiateTransfer.Enabled = true;
                txtLegacyCardNumber.ReadOnly = true;
                EnableScreenForManualEntry();
            }
            else
            {
                txtLegacyCardNumber.ReadOnly = false;
                btnTransfer.Enabled = true;
                btnInitiateTransfer.Enabled = false;
            }
            log.LogMethodExit();
        }

        //DeviceClass readerDevice = null;
        Card legacyCard = null;
        void RegisterMifareDevice()
        {
            log.LogMethodEntry();
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
            if (Common.Devices.PrimaryBarcodeScanner != null)
            {
                Common.Devices.PrimaryBarcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
            }
            Common.Devices.RegisterLegacyCardReadEvent(new EventHandler(LegacyCardScanEventHandle));
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string message = "";
            //AddLegacyCardKey();
            txtMessage.Text = "";
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                try
                {
                    tagNumber = tagNumberParser.Parse(checkScannedEvent.Message);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while parsing the scanned tag number", ex);
                    POSUtils.ParafaitMessageBox(ex.Message);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                string CardNumber = tagNumber.Value;
                try
                {
                    Card card = new Card(Common.Devices.CardReaders[Common.Devices.CardReaders.Count - 1], CardNumber, POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);
                    if (card != null)
                    {
                        if (POSStatic.CardUtilities.refreshRequiredFromHQ(card))
                        {
                            if (!POSUtils.refreshCardFromHQ(ref card, ref message))
                            {
                                POSUtils.ParafaitMessageBox(message, "Roaming cards");
                                log.Info(message);
                                return;
                            }
                        }
                    }
                    txtParafaitCardNumber.Text = CardNumber;
                    CheckCardExisting();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                string scannedBarcode = POSStatic.Utilities.ProcessScannedBarCode(checkScannedEvent.Message, POSStatic.Utilities.ParafaitEnv.LEFT_TRIM_BARCODE, POSStatic.Utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);
                try
                {
                    //Thread error fix by threading 15-May-2016
                    this.Invoke((MethodInvoker)delegate
                    {
                        HandleBarcodeRead(scannedBarcode);
                    });
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void HandleBarcodeRead(string scannedBarcode)
        {
            log.LogMethodEntry(scannedBarcode);
            try
            {
                if (!string.IsNullOrEmpty(scannedBarcode))
                {
                    txtLegacyCardNumber.Text = scannedBarcode;
                    GetLegacyCardDetailsAsync();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Check Card Existing
        /// </summary>
        /// <returns></returns>
        bool CheckCardExisting()
        {
            log.LogMethodEntry();
            AccountListBL accountListBL = new AccountListBL(POSStatic.Utilities.ExecutionContext);
            AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria(AccountDTO.SearchByParameters.TAG_NUMBER, Operator.EQUAL_TO, txtParafaitCardNumber.Text.ToString());
            accountSearchCriteria.And(AccountDTO.SearchByParameters.VALID_FLAG, Operator.EQUAL_TO, "Y");
            List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria);
            existingParafaitCard = false;
            allowMultipleLegacyCards = false;
            allowMultipleLegacyCards = Convert.ToBoolean(lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("MapMultipleCards")).ToList<LookupValuesDTO>()[0].Description);//Properties.Settings.Default.MapMultipleCards;
            txtMessage.Text = "";
            if (accountDTOList != null && accountDTOList.Count == 1 && allowMultipleLegacyCards)
            {
                existingParafaitCard = true;
                existingParafaitCardId = Convert.ToInt32(accountDTOList[0].AccountId);
                log.Debug("Value of existingParafaitCard is true");
            }
            if (accountDTOList != null && accountDTOList.Count > 0 && allowMultipleLegacyCards == false)
            {
                log.Debug("Card already issued and configuration value is false for Allow Multiple Legacy");
                txtParafaitCardNumber.Text = "";
                txtMessage.Text = "Parafait Card is already issued. Use a new card";
                txtParafaitCardNumber.Focus();
                log.LogMethodExit(false);
                return false;
            }
            if (isUnisSystem && !existingParafaitCard)
            {
                btnTransfer.Enabled = true;
            }
            log.LogMethodExit(true);
            return true;
        }

        private async void btnGetDetails_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                log.LogMethodEntry(sender, e);
                txtMessage.Text = "Please wait while we process your request";
                if (isUnisSystem)
                {
                    txtFaceValue.Enabled = true;
                }
                else
                {
                    txtFaceValue.Enabled = false;
                }
                if (string.IsNullOrEmpty(txtLegacyCardNumber.Text))
                {
                    txtMessage.Text = "Please enter legacy card number";
                    txtTranslatedCardNumber.Text = "";
                    return;
                }
                await GetLegacyCardDetailsAsync();
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    txtMessage.Text = errorMessage;
                    errorMessage = "";
                    txtTranslatedCardNumber.Text = "";
                    return;
                }
                if (ValidationErrorList != null && ValidationErrorList.Count > 0)
                {
                    txtMessage.Text = ValidationErrorList[0].Message;
                    txtTranslatedCardNumber.Text = "";
                    return;
                }
                if (!LegacyCardDetails)
                {
                    if (isUnisSystem)
                    {
                        txtMessage.Text = "Legacy card does not exist. Please enter valid details and continue.";
                        txtTranslatedCardNumber.Text = "";
                    }
                    else
                    {
                        txtMessage.Text = "Invalid legacy Card. Please Retry.";
                        txtTranslatedCardNumber.Text = "";
                    }
                    txtLegacyCardNumber.Focus();
                }
                else if (transferred)
                {
                    txtMessage.Text = "Valid legacy Card. Transferred to Parafait card " + txtParafaitCardNumber.Text;
                    txtLegacyCardNumber.Focus();
                    SetEditBtnVisibility(false);

                }
                else
                {
                    txtParafaitCardNumber.Text = "";
                    if (isUnisSystem)
                    {
                        txtMessage.Text = "Valid legacy Card. Click on initiate transfer.";
                    }
                    else
                    {
                        txtMessage.Text = "Valid legacy Card. Tap the parafait card.";
                    }
                    txtParafaitCardNumber.Focus();
                    //SetEditBtnVisibility(true);
                }
                log.LogMethodExit();
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnGetDetails_ClickAsync() Method with an Exception:", ex);
            }
        }
        void EnableScreenForManualEntry(bool isEnabled = true)
        {
            log.LogMethodEntry();
            txtCredits.ReadOnly =
            txtCourtesy.ReadOnly =
            txtBonus.ReadOnly =
            txtCreditsPlayed.ReadOnly =
            // txtLastPlayedTime.ReadOnly =
            txtLegacyCardNumber.ReadOnly =
            txtLoyalty.ReadOnly =
            //txtMessage.ReadOnly =
            txtNotes.ReadOnly =
            txtTickets.ReadOnly =
            txtTime.ReadOnly =
            txtTransferDate.ReadOnly =
            txtFaceValue.ReadOnly = !isEnabled;
            //txtTranslatedCardNumber.ReadOnly 
            txtLegacyCardNumber.ReadOnly = false;
            dtpLastPlayed.Enabled = isEnabled;
            Color color = Color.White;
            if (!isEnabled)
            {
                color = SystemColors.Control;
            }
            txtLegacyCardNumber.BackColor = Color.White;
            txtCredits.BackColor =
            txtCourtesy.BackColor =
            txtBonus.BackColor =
            txtCreditsPlayed.BackColor =
            txtLastPlayedTime.BackColor =
            txtLoyalty.BackColor =
            //txtMessage.BackColor =
            txtNotes.BackColor =
            txtTickets.BackColor =
            txtTime.BackColor =
            txtTransferDate.BackColor =
            txtFaceValue.BackColor = color;

            //txtTranslatedCardNumber.BackColor =
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Legacy Card Details Async
        /// </summary>
        /// <returns></returns>
        async Task<bool> GetLegacyCardDetailsAsync()
        {
            log.LogMethodEntry();
            try
            {
                TranslateCardnumber();
                string cardNumber = txtTranslatedCardNumber.Text;
                if (string.IsNullOrEmpty(cardNumber))
                {
                    errorMessage = "Please enter valid legacy card number";
                    return false;
                }
                if (!string.IsNullOrEmpty(cardNumber))
                {
                    List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>> SearchByParameters = new List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>>();
                    SearchByParameters.Add(new KeyValuePair<LegacyCardDTO.SearchByParameters, string>(LegacyCardDTO.SearchByParameters.CARD_NUMBER_OR_PRINTED_CARD_NUMBER, cardNumber));
                    List<LegacyCardDTO> LegacyCardDTOList = new List<LegacyCardDTO>();
                    LegacyCardListBL legacyCardListBL = new LegacyCardListBL(POSStatic.Utilities.ExecutionContext, LegacyCardDTOList, Utilities);
                    LegacyCardDTOList = legacyCardListBL.GetLegacyCardDTOList(SearchByParameters);
                    if (LegacyCardDTOList != null && LegacyCardDTOList.Count > 0)
                    {
                        legacyCardDTO = LegacyCardDTOList[0];
                    }
                    else
                    {
                        legacyCardDTO = null;
                    }
                    if (legacyCardDTO == null)
                    {
                        legacyCardDTO = new LegacyCardDTO();
                        legacyCardDTO.CardId = -1;
                        legacyCardDTO.CardNumber = cardNumber;
                        legacyCardDTO.IssueDate = ServerDateTime.Now;
                    }
                }
                LegacyCardBL legacyCardBL = new LegacyCardBL(POSStatic.Utilities.ExecutionContext, legacyCardDTO, Utilities);
                LegacyCardDTO externalLegacyCardDTO = await Task<LegacyCardDTO>.Factory.StartNew(() =>
                {
                    try
                    {
                        errorMessage = "";
                        ValidationErrorList = null;
                        return legacyCardBL.GetLegacyCardDetails();
                    }
                    catch (ValidationException ex)
                    {
                        ValidationErrorList = ex.ValidationErrorList;
                        errorMessage = ex.Message;
                        log.Error(ex.Message);
                        log.LogMethodExit(null);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        if (string.IsNullOrEmpty(ex.Message))
                        {
                            errorMessage = "Error occured while getting card data, Please retry";
                        }
                        else
                        {
                            errorMessage = ex.Message;
                        }
                        log.Error(ex.Message);
                        log.LogMethodExit(null, "Throwing an Exception - " + ex.Message);
                        return null;
                    }
                });
                if (externalLegacyCardDTO == null)// && (legacyCardDTOlist.Count == 0 || legacyCardDTOlist.Count > 1))
                {
                    txtCredits.Text = txtCourtesy.Text = txtBonus.Text = txtTime.Text = txtTransferDate.Text = txtLastPlayedTime.Text = txtCreditsPlayed.Text = txtLoyalty.Text = txtNotes.Text = txtTickets.Text = txtFaceValue.Text = "";
                    chkTransferred.Checked = false;
                    if (isUnisSystem)
                    {
                        btnInitiateTransfer.Enabled = true;
                    }
                    log.LogMethodExit(false, "legacyCardDTOlist.Count == 0 || legacyCardDTOlist.Count > 1");
                    LegacyCardDetails = false;
                    return false;
                }

                //FaceValue
                if (externalLegacyCardDTO.RevisedFaceValue <= 0)
                {
                    txtFaceValue.Text = (externalLegacyCardDTO.FaceValue).ToString();
                }
                else
                {
                    txtFaceValue.Text = (externalLegacyCardDTO.RevisedFaceValue).ToString();
                }

                if (externalLegacyCardDTO.RevisedCredits <= 0)
                {
                    txtCredits.Text = (externalLegacyCardDTO.Credits).ToString();
                }
                else
                {
                    txtCredits.Text = (externalLegacyCardDTO.RevisedCredits).ToString();
                }
                //Courtesy
                if (externalLegacyCardDTO.RevisedCourtesy <= 0)
                {
                    txtCourtesy.Text = (externalLegacyCardDTO.Courtesy).ToString();
                }
                else
                {
                    txtCourtesy.Text = (externalLegacyCardDTO.RevisedCourtesy).ToString();
                }

                if (externalLegacyCardDTO.RevisedBonus <= 0)
                {
                    txtBonus.Text = (externalLegacyCardDTO.Bonus).ToString();
                }
                else
                {
                    txtBonus.Text = (externalLegacyCardDTO.RevisedBonus).ToString();
                }
                //CreditsPlayed
                if (externalLegacyCardDTO.RevisedCreditsPlayed <= 0)
                {
                    txtCreditsPlayed.Text = (externalLegacyCardDTO.CreditsPlayed).ToString();
                }
                else
                {
                    txtCreditsPlayed.Text = (externalLegacyCardDTO.RevisedCreditsPlayed).ToString();
                }
                txtTime.Text = externalLegacyCardDTO.Time.ToString();
                DisplayEntitlements(externalLegacyCardDTO);
                SetLegacyDetailsVisibility(externalLegacyCardDTO);

                txtLastPlayedTime.Text = externalLegacyCardDTO.LastPlayedTime == null ? "" : Convert.ToDateTime(externalLegacyCardDTO.LastPlayedTime).ToString("dd-MMM-yyyy hh:mm tt");
                //txtTickets.Text = externalLegacyCardDTO.TicketCount.ToString();
                txtNotes.Text = externalLegacyCardDTO.Notes == null ? "" : externalLegacyCardDTO.Notes.ToString();
                if (externalLegacyCardDTO.Transferred.ToString() == "Y")
                {
                    if (externalLegacyCardDTO.TransferToCard > -1)
                    {
                        AccountBL accountBL = new AccountBL(POSStatic.Utilities.ExecutionContext, externalLegacyCardDTO.TransferToCard, false, false);
                        txtTransferDate.Text = Convert.ToDateTime(externalLegacyCardDTO.TransferDate).ToString("dd-MMM-yyyy hh:mm tt");
                        if (accountBL.AccountDTO != null && !string.IsNullOrWhiteSpace(accountBL.AccountDTO.TagNumber))
                        {
                            txtParafaitCardNumber.Text = accountBL.AccountDTO.TagNumber;
                        }
                        transfer_to_card = externalLegacyCardDTO.TransferToCard.ToString();
                    }
                    chkTransferred.Checked = true;
                    transferred = true;
                }
                else
                {
                    chkTransferred.Checked = false;
                    transferred = false;
                    transfer_to_card = "";
                    txtTransferDate.Text = "";
                }
                if (isUnisSystem)
                {
                    if (chkTransferred.Checked)
                    {
                        btnInitiateTransfer.Enabled = false;
                        btnTransfer.Enabled = false;
                        EnableScreenForManualEntry(false);
                    }
                    else
                    {
                        btnInitiateTransfer.Enabled = true;
                        btnTransfer.Enabled = false;
                        EnableScreenForManualEntry(true);
                    }
                }
                else
                {
                    if (!chkTransferred.Checked)
                    {
                        btnTransfer.Enabled = true;
                    }
                    else
                    {
                        btnTransfer.Enabled = false;
                    }
                }
                log.LogMethodExit(true);
                LegacyCardDetails = true;
                return true;
            }
            catch (ValidationException ex)
            {
                log.Error("Error occured while getting card data, Please retry", ex);
                StringBuilder errorMessageBuilder = new StringBuilder("");
                foreach (var validationError in ex.ValidationErrorList)
                {
                    errorMessageBuilder.Append(validationError.Message);
                    errorMessageBuilder.Append(Environment.NewLine);
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                log.LogMethodExit(false);
                return false;
            }
        }


        /// <summary>
        /// Set Legacy Details Visibility
        /// </summary>
        private void SetLegacyDetailsVisibility(LegacyCardDTO externalLegacyCardDTO)
        {
            log.LogMethodEntry();
            if (externalLegacyCardDTO != null)
            {
                if (externalLegacyCardDTO.LegacyCardCreditPlusDTOList != null && externalLegacyCardDTO.LegacyCardCreditPlusDTOList.Any())
                {
                    List<LegacyCardCreditPlusDTO> legacyCardCreditPlusList = externalLegacyCardDTO.LegacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(CreditPlusType.CARD_BALANCE) || l.CreditPlusType.Equals(CreditPlusType.GAME_PLAY_CREDIT) || l.CreditPlusType.Equals(CreditPlusType.COUNTER_ITEM));
                    if (legacyCardCreditPlusList.Count > 0)
                    {
                        btnEditCredits.Visible = true;
                        txtCredits.ReadOnly = true;
                    }
                    else
                    {
                        btnEditCredits.Visible = false;
                        txtCredits.ReadOnly = false;
                    }
                }
                else
                {
                    txtCredits.ReadOnly = false;
                }
                if (externalLegacyCardDTO.LegacyCardCreditPlusDTOList != null && externalLegacyCardDTO.LegacyCardCreditPlusDTOList.Any())
                {
                    List<LegacyCardCreditPlusDTO> legacyCardCreditPlusList = externalLegacyCardDTO.LegacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(CreditPlusType.GAME_PLAY_BONUS));
                    if (legacyCardCreditPlusList.Count > 0)
                    {
                        btnEditBonus.Visible = true;
                        txtBonus.ReadOnly = true;
                    }
                    else
                    {
                        btnEditBonus.Visible = false;
                        txtBonus.ReadOnly = false;
                    }
                }
                else
                {
                    txtBonus.ReadOnly = false;
                }
                if (externalLegacyCardDTO.LegacyCardCreditPlusDTOList != null && externalLegacyCardDTO.LegacyCardCreditPlusDTOList.Any())
                {
                    List<LegacyCardCreditPlusDTO> legacyCardCreditPlusList = externalLegacyCardDTO.LegacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(CreditPlusType.TIME));
                    if (legacyCardCreditPlusList.Count > 0)
                    {
                        btnEditTime.Visible = true;
                        txtTime.ReadOnly = true;
                    }
                    else
                    {
                        btnEditTime.Visible = false;
                        txtTime.ReadOnly = false;
                    }
                }
                else
                {
                    txtTime.ReadOnly = false;
                }
                if (externalLegacyCardDTO.LegacyCardCreditPlusDTOList != null && externalLegacyCardDTO.LegacyCardCreditPlusDTOList.Any())
                {
                    List<LegacyCardCreditPlusDTO> legacyCardCreditPlusList = externalLegacyCardDTO.LegacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(CreditPlusType.TICKET));
                    if (legacyCardCreditPlusList.Count > 0)
                    {
                        btnEditTickets.Visible = true;
                        txtTickets.ReadOnly = true;
                    }
                    else
                    {
                        btnEditTickets.Visible = false;
                        txtTickets.ReadOnly = false;
                    }
                }
                else
                {
                    txtTickets.ReadOnly = false;
                }
                if (externalLegacyCardDTO.LegacyCardCreditPlusDTOList != null && externalLegacyCardDTO.LegacyCardCreditPlusDTOList.Any())
                {
                    List<LegacyCardCreditPlusDTO> legacyCardCreditPlusList = externalLegacyCardDTO.LegacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(CreditPlusType.LOYALTY_POINT));
                    if (legacyCardCreditPlusList.Count > 0)
                    {
                        btnEditLoyaltyPoints.Visible = true;
                        txtLoyalty.ReadOnly = true;
                    }
                    else
                    {
                        btnEditLoyaltyPoints.Visible = false;
                        txtLoyalty.ReadOnly = false;
                    }
                }
                else
                {
                    txtLoyalty.ReadOnly = false;
                }
                if (externalLegacyCardDTO.LegacyCardGamesDTOsList != null && externalLegacyCardDTO.LegacyCardGamesDTOsList.Any())
                {
                    List<LegacyCardGamesDTO> legacyCardGamesDTOList = externalLegacyCardDTO.LegacyCardGamesDTOsList;
                    if (legacyCardGamesDTOList.Count > 0)
                    {
                        btnEditGames.Visible = true;
                        txtGames.ReadOnly = true;
                    }
                    else
                    {
                        btnEditGames.Visible = false;
                        txtGames.ReadOnly = false;
                    }
                }
                else
                {
                    txtGames.ReadOnly = false;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Display Entitlements
        /// </summary>
        /// <param name="externalLegacyCardDTO"></param>
        private void DisplayEntitlements(LegacyCardDTO externalLegacyCardDTO)
        {
            log.LogMethodEntry(externalLegacyCardDTO);
            txtCredits.Text = externalLegacyCardDTO.LegacyCardSummaryDTO.TotalCredits > 0 ? externalLegacyCardDTO.LegacyCardSummaryDTO.TotalCredits.ToString() : externalLegacyCardDTO.RevisedCredits > 0 ? (externalLegacyCardDTO.RevisedCredits).ToString():(externalLegacyCardDTO.Credits).ToString();
            txtBonus.Text = externalLegacyCardDTO.LegacyCardSummaryDTO.TotalBonus > 0 ? externalLegacyCardDTO.LegacyCardSummaryDTO.TotalBonus.ToString() : externalLegacyCardDTO.RevisedBonus > 0 ? (externalLegacyCardDTO.RevisedBonus).ToString() : (externalLegacyCardDTO.Bonus).ToString();
            txtTime.Text = externalLegacyCardDTO.LegacyCardSummaryDTO.TotalTime > 0 ? externalLegacyCardDTO.LegacyCardSummaryDTO.TotalTime.ToString() : externalLegacyCardDTO.Time.ToString();
            txtTickets.Text = externalLegacyCardDTO.LegacyCardSummaryDTO.TotalTickets > 0 ? externalLegacyCardDTO.LegacyCardSummaryDTO.TotalTickets.ToString() : externalLegacyCardDTO.TicketCount.ToString();
            txtLoyalty.Text = externalLegacyCardDTO.LegacyCardSummaryDTO.TotalLoyaltyPoints.ToString();
            txtGames.Text = externalLegacyCardDTO.LegacyCardSummaryDTO.TotalGames.ToString();
            log.LogMethodExit();
        }

        bool checkTransferStatus()
        {
            log.LogMethodEntry();
            string cardNumber = txtTranslatedCardNumber.Text;
            List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>> SearchByParameters = new List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>>();
            SearchByParameters.Add(new KeyValuePair<LegacyCardDTO.SearchByParameters, string>(LegacyCardDTO.SearchByParameters.CARD_NUMBER_OR_PRINTED_CARD_NUMBER, cardNumber));
            List<LegacyCardDTO> LegacyCardDTOList = new List<LegacyCardDTO>();
            LegacyCardListBL legacyCardListBL = new LegacyCardListBL(POSStatic.Utilities.ExecutionContext, LegacyCardDTOList, Utilities);
            LegacyCardDTOList = legacyCardListBL.GetLegacyCardDTOList(SearchByParameters);
            Card card = null;
            if (LegacyCardDTOList != null && LegacyCardDTOList.Any())
            {
                LegacyCardDTO legacyCardDTO = LegacyCardDTOList[0];
                if (legacyCardDTO.TransferToCard <= 0)
                {
                    log.LogMethodExit(true, "card == null");
                    return true;
                }
                card = new Card(legacyCardDTO.TransferToCard, POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);
            }
            if (card == null)
            {
                log.LogMethodExit(true, "card == null");
                return true;
            }
            else
            {
                txtMessage.Text = "This card is already transferred to " + card.CardNumber;
                log.LogMethodExit(false, "This card is already transferred to " + card.CardNumber);
                SetEditBtnVisibility(false);
                return false;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            Close();
        }
        private async void btnTransfer_ClickAsync(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            btnTransfer.Enabled = false;
            txtMessage.Text = "Please wait while we process your request";
            if (txtLegacyCardNumber.Text == "")
            {
                txtMessage.Text = "Swipe or enter Legacy Card";
                txtLegacyCardNumber.Focus();
                log.LogMethodExit("Swipe or enter Legacy Card");
                return;
            }
            if (txtTranslatedCardNumber.Text == "")
            {
                txtMessage.Text = "Get the card details before transferring - Click on Get Details";
                txtTranslatedCardNumber.Focus();
                log.LogMethodExit("Get the card details before transferring - Click on Get Details");
                return;
            }
            if (txtParafaitCardNumber.Text.Trim() == "")
            {
                txtMessage.Text = "Tap Parafait Card";
                txtLegacyCardNumber.Focus();
                log.LogMethodExit("Tap Parafait Card");
                return;
            }
            if (!isClubSpeed && !isUnisSystem)
            {
                await GetLegacyCardDetailsAsync();
            }
            if (!isUnisSystem && !isClubSpeed && !LegacyCardDetails)
            {
                txtMessage.Text = "Invalid legacy Card. Retry...";
                txtLegacyCardNumber.Focus();
                log.LogMethodExit("Invalid legacy Card. Retry...");
                return;
            }

            if (!checkTransferStatus())
            {
                txtLegacyCardNumber.Focus();
                log.LogMethodExit("!checkTransferStatus()");
                return;
            }

            if (transferred)
            {
                string message = string.Empty;
                if (String.IsNullOrEmpty(transfer_to_card))
                {
                    message = "Card is already transferred to ";
                }
                else
                {
                    message = "Card is already transferred";
                }
                txtMessage.Text = message + transfer_to_card + ".";
                txtLegacyCardNumber.Focus();
                log.LogMethodExit(message + transfer_to_card + ".");
                return;
            }

            SqlCommand cmd = new SqlCommand();
            if (ParafaitCon.State == ConnectionState.Closed)
                ParafaitCon.Open();
            cmd.Connection = ParafaitCon;

            int parafait_card_id = 0;

            SqlConnection TrxCnn = null;
            TrxCnn = POSStatic.Utilities.createConnection();

            SqlTransaction sqlTransaction = TrxCnn.BeginTransaction();
            try
            {
                await TransferCardData(parafait_card_id, sqlTransaction, TrxCnn);
            }
            catch (Exception ex)
            {
                if (sqlTransaction != null)
                    sqlTransaction.Rollback();
                TrxCnn.Close();
                string errMsg = string.Empty;
                if (ex.Message != null)
                {
                    errMsg = ex.Message;
                }
                txtMessage.Text = "Error occured while transferring card data, Please retry" + errMsg;
                log.LogMethodExit(null, "Throwing an Exception - " + ex.Message);
                log.Error(ex);
                return;
            }
            //TrxCnn = POSStatic.Utilities.createConnection();
            //SqlTransaction sqlTrx = TrxCnn.BeginTransaction();
            try
            {
                int cardTime;
                if ((legacyCardDTO.Time <= 0))
                {
                    cardTime = 0;
                }
                else
                {
                    cardTime = Convert.ToInt32(legacyCardDTO.Time);
                }
                if (chkIncludePackages.Checked)
                {

                    if (LoadSacoaPackages(cmd, txtTranslatedCardNumber.Text, parafait_card_id, legacyCardDTO, cardTime, sqlTransaction))
                    {
                        legacyCardDTO.TransferredCardgames = 'Y';
                        LegacyCardBL legacyCardBL = new LegacyCardBL(Utilities.ExecutionContext, legacyCardDTO, Utilities);
                        legacyCardBL.Save(sqlTransaction);
                    }
                }
                else if (cardTime > 0)
                {
                    AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, legacyCardDTO.ParafaitCardNumber, true, true, sqlTransaction);
                    if (accountBL.AccountDTO != null)
                    {
                        AccountCreditPlusDTO accountCreditPlusDTO = new AccountCreditPlusDTO(-1, (legacyCardDTO.RevisedTime <= 0 ? legacyCardDTO.Time : legacyCardDTO.RevisedTime), CreditPlusType.TIME, true,
                           "Legacy card transfer", accountBL.AccountDTO.AccountId, Convert.ToInt32(legacyCardDTO.TrxId), -1, (legacyCardDTO.RevisedTime <= 0 ? legacyCardDTO.Time : legacyCardDTO.RevisedTime), ServerDateTime.Now, null, null, null, null, true, true,
                           true, true, true, true, true, null, -1, false, null, true, false, false, -1, -1, true, true, -1, AccountDTO.AccountValidityStatus.Valid, -1);
                        accountBL.AccountDTO.AccountCreditPlusDTOList.Add(accountCreditPlusDTO);
                        accountBL.Save(sqlTransaction);
                    }

                }
                sqlTransaction.Commit();
                TrxCnn.Close();
                txtLegacyCardNumber.Focus();
            }
            catch (Exception ex)
            {
                if (sqlTransaction != null)
                    sqlTransaction.Rollback();
                txtMessage.Text = ex.Message;
                log.Error(ex);
            }
            finally
            {
                TrxCnn.Close();
            }
            log.LogMethodExit();
        }

        private async Task<bool> TransferCardData(int parafait_card_id, SqlTransaction sqlTransaction, SqlConnection TrxCnn)
        {
            if (legacyCardDTO != null)
            {
                bool transferFlag = false;
                legacyCardDTO.RefundFlag = 'N';
                legacyCardDTO.ValidFlag = 'N';
                legacyCardDTO.VipCustomer = 'N';
                legacyCardDTO.TechnicianCard = 'N';
                legacyCardDTO.TechGames = -1;
                legacyCardDTO.TimerResetCard = 'N';
                legacyCardDTO.Status = 'S';
                legacyCardDTO.Transferred = 'Y';
                legacyCardDTO.TransferDate = ServerDateTime.Now;
                legacyCardDTO.ParafaitCardNumber = txtParafaitCardNumber.Text;
                legacyCardDTO.Notes = txtNotes.Text;
                if (isClubSpeed)
                {
                    legacyCardDTO.Credits = Convert.ToInt32(txtCredits.Text);
                    legacyCardDTO.RefundFlag = 'N';
                    legacyCardDTO.CustomerId = Convert.ToInt32(txtTranslatedCardNumber.Text);
                }
                if (legacyCardDTO.TempCardId > -1)
                {
                    parafait_card_id = legacyCardDTO.TempCardId;
                    Card sourceCard = new Card(parafait_card_id, POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);
                    Card targetCard = new Card(txtParafaitCardNumber.Text, POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);
                    TaskProcs tempTP = new TaskProcs(POSStatic.Utilities);
                    string message = "";
                    tempTP.transferCard(sourceCard, targetCard, "Transfer Membership as part of Legacy Transfer", ref message, sqlTransaction);
                    legacyCardDTO.TransferToCard = parafait_card_id;
                    LegacyCardBL legacyCardBL = new LegacyCardBL(Utilities.ExecutionContext, legacyCardDTO, Utilities);
                    legacyCardBL.SaveTempLegacyCards(sqlTransaction);

                    transferFlag = true;
                    txtMessage.Text = "Card transferred successfully";
                    SetEditBtnVisibility(false);
                }
                else if (isClubSpeed)
                {
                    LegacyCardBL legacyCardBL = new LegacyCardBL(Utilities.ExecutionContext, legacyCardDTO, Utilities);
                    await Task<bool>.Factory.StartNew(() =>
                    {
                        try
                        {
                            legacyCardBL.Save(sqlTransaction);
                            transferFlag = true;
                            txtMessage.Text = "Card transferred successfully";
                            SetEditBtnVisibility(false);
                            return true;
                        }
                        catch (ValidationException ex)
                        {
                            errorMessage = ex.Message;
                            return false;
                        }
                        catch (Exception ex)
                        {
                            if (string.IsNullOrEmpty(ex.Message))
                            {
                                errorMessage = "Error occured while getting card data, Please retry";
                            }
                            else
                            {
                                errorMessage = ex.Message;
                            }
                            log.Error(ex.Message);
                            log.LogMethodExit(ex.Message);
                            return false;
                        }
                    });
                }
                else if (allowMultipleLegacyCards && existingParafaitCard && !isClubSpeed)
                {
                    log.Debug("Parafait card already issued. Update to be performed for card id: " + existingParafaitCardId.ToString());
                    legacyCardDTO.TransferToCard = existingParafaitCardId;
                    LegacyCardBL legacyCardBL = new LegacyCardBL(Utilities.ExecutionContext, legacyCardDTO, Utilities);
                    legacyCardBL.Save(sqlTransaction);
                    transferFlag = true;
                    txtMessage.Text = "Card transferred successfully";
                    SetEditBtnVisibility(false);
                    parafait_card_id = existingParafaitCardId;
                }
                else if (existingParafaitCard == false && !isClubSpeed)
                {
                    LegacyCardBL legacyCardBL = new LegacyCardBL(Utilities.ExecutionContext, legacyCardDTO, Utilities);
                    legacyCardBL.Save(sqlTransaction);
                    txtMessage.Text = "Card transferred successfully";
                    SetEditBtnVisibility(false);
                    return true;
                }
                else
                {
                    chkTransferred.Checked = false;
                    if (sqlTransaction != null)
                        sqlTransaction.Rollback();
                    txtMessage.Text = "Card cannot be transferred. Retry.";
                    log.LogMethodExit("Card cannot be transferred. Retry.");
                    return false;
                }
                if (transferFlag)
                {
                    chkTransferred.Checked = true;
                    DateTime transfer_date = POSStatic.Utilities.getServerTime();//ServerDateTime.Now was changed to GetServerTime
                    txtTransferDate.Text = transfer_date.ToString("dd-MMM-yyyy hh:mm tt");
                    SetEditBtnVisibility(false);
                    txtMessage.Text = "Card transferred successfully";
                    //if (sqlTransaction != null)
                    //    sqlTransaction.Commit();
                    //TrxCnn.Close();
                    return true;
                }
                else
                {
                    txtMessage.Text = "Card cannot be transferred. Retry.";
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        bool LoadSacoaPackages(SqlCommand cmd, string legacyCardNumber, int parafaitCardId, LegacyCardDTO legacyCardDTO, int cardTime = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cmd, legacyCardNumber, parafaitCardId, cardTime);
            try
            {
                LegacyCardBL legacyCardBL = new LegacyCardBL(POSStatic.Utilities.ExecutionContext, legacyCardDTO, Utilities);
                DataTable dtGetSacoaPackages = legacyCardBL.GetSacoaPackages(legacyCardNumber, cardTime);
                if (dtGetSacoaPackages != null)
                {
                    if (dtGetSacoaPackages.Rows.Count == 0)
                    {
                        log.LogMethodExit(true, "dt.Rows.Count == 0");
                        return true;
                    }

                    Transaction trx = new Transaction(POSStatic.Utilities);
                    string message = "";

                    foreach (DataRow dr in dtGetSacoaPackages.Rows)
                    {
                        if (dr["ParafaitProductId"].Equals(DBNull.Value) == false)
                        {
                            message = "";
                            if (0 != trx.createTransactionLine(new Card(parafaitCardId, "", POSStatic.Utilities), (int)dr["ParafaitProductId"], 0, 1, ref message))
                            {
                                txtMessage.Text = message;
                                log.LogMethodExit(false, "createTransactionLine() !=0");
                                return false;
                            }
                        }
                    }
                    if (trx.TrxLines.Count > 0)
                    {
                        foreach (Transaction.TransactionLine tl in trx.TrxLines)
                            tl.LineAmount = tl.Price = tl.tax_amount = 0;

                        trx.Net_Transaction_Amount = trx.Transaction_Amount = 0;
                        if (trx.SaveTransacation(cmd.Transaction, ref message) == 0)
                        {
                            txtMessage.Text = message;
                            foreach (DataRow dr in dtGetSacoaPackages.Rows)
                            {
                                Transaction transaction = new Transaction(POSStatic.Utilities);
                                TransactionListBL transactionListBL = new TransactionListBL(POSStatic.Utilities.ExecutionContext);
                                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchByTransactionParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                                searchByTransactionParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, trx.Trx_id.ToString()));
                                searchByTransactionParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.HAS_PRODUCT_ID_LIST, dr["ParafaitProductId"].ToString()));
                                List<TransactionDTO> transactionDTOList = transactionListBL.GetTransactionDTOList(searchByTransactionParameters, POSStatic.Utilities, null, 0, 10000, true);
                                if (transactionDTOList != null && transactionDTOList.Count > 0
                                    && transactionDTOList[0].TransactionLinesDTOList != null
                                    && transactionDTOList[0].TransactionLinesDTOList.Count > 0)
                                {
                                    int TrxLineId = transactionDTOList[0].TransactionLinesDTOList[0].LineId;
                                    if (Convert.ToInt32(dr["Duration"]) == 0) // card game package. update quantities to transfer qty
                                    {
                                        legacyCardBL.UpdateLegacyCardGames(trx.Trx_id, parafaitCardId, dr["ParafaitProductId"], dr["Quantity"], TrxLineId, sqlTransaction);
                                    }
                                    else if (Convert.ToInt32(dr["LegacyProductId"]) == -1) // card time to be loaded as a separate product
                                    {
                                        legacyCardBL.UpdateCardCreditPlus(trx.Trx_id, parafaitCardId, dr["ParafaitProductId"], cardTime, TrxLineId, sqlTransaction);
                                    }
                                }
                            }
                        }
                    }
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                log.LogMethodExit(false);
                return false;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtTranslatedCardNumber.Text = txtLegacyCardNumber.Text = txtCredits.Text = txtCourtesy.Text = txtTime.Text = txtLastPlayedTime.Text = txtTransferDate.Text = txtBonus.Text = txtCustomer.Text = txtMessage.Text =
                txtTickets.Text = txtCreditsPlayed.Text = txtLoyalty.Text = txtNotes.Text = txtParafaitCardNumber.Text = txtFaceValue.Text = txtGames.Text = "";
            chkTransferred.Checked = false;
            SetEditBtnVisibility(false);
            txtLegacyCardNumber.Focus();
            log.LogMethodExit();
        }

        SqlConnection ParafaitCon;

        void CreateXrefTable()
        {
            log.LogMethodEntry();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = ParafaitCon;
            cmd.CommandText = "if not exists (select 1 from INFORMATION_SCHEMA.TABLES where table_name = 'mCashXref') " +
                                "create table mCashXref (MCASHCardNumber varchar(20), ParafaitCardNumber varchar(10), TimeStamp datetime); " +
                                "if exists (select 1 from INFORMATION_SCHEMA.TABLES where table_name = 'mCashXref') " +
                                "create index mCashXref_mCashCard on mCashXref(MCASHCardNumber)";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void lnkXRef_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            frmXRef f = new frmXRef(POSStatic.Utilities);
            f.ShowDialog();
            txtLegacyCardNumber.Focus();
            log.LogMethodExit();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if ((sender as TextBox).Enabled)
                CurrentActiveTextBox = sender as TextBox;
            if (keypad != null && !keypad.IsDisposed)
                keypad.currentTextBox = CurrentActiveTextBox;
            log.LogMethodExit();
        }

        AlphaNumericKeyPad keypad;
        public TextBox CurrentActiveTextBox;
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad == null || keypad.IsDisposed)
            {
                if (CurrentActiveTextBox == null)
                    CurrentActiveTextBox = new TextBox();
                keypad = new AlphaNumericKeyPad(this, CurrentActiveTextBox);
                keypad.currentTextBox = CurrentActiveTextBox;
                //keypad.Location = new Point((Screen.PrimaryScreen.Bounds.Width - keypad.Width) / 2, txtLegacyCardNumber.PointToScreen(txtLegacyCardNumber.Location).Y);
                keypad.Show();
            }
            else if (keypad.Visible)
            {
                keypad.Hide();
                if (CurrentActiveTextBox != null)
                    CurrentActiveTextBox.Focus();
            }
            else
            {
                keypad.Show();
            }
            log.LogMethodExit();
        }

        bool cardCleared = false;
        private void btnInitiateTransfer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            cardCleared = false;

            string message = "Please confirm that all the required details are filled.\n The unis card will be cleared on continue.\n Do you want to continue?.";
            if (POSUtils.ParafaitMessageBox(message, "Initiate Transfer", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                log.LogMethodExit(null, "User Didn't confirm.");
                return;
            }
            if (!string.IsNullOrEmpty(txtLegacyCardNumber.Text))
            {
                TranslateCardnumber();
            }
            if (ValidateInput() == false)
            {
                log.LogMethodExit(null, "Validation Failed.");
                return;
            }
            if (ClearLegacyCard() == false)
            {
                log.LogMethodExit(null, "Clear Legacy Card Failed.");
                return;
            }
            if (UpdateLegacyCards() == false)
            {
                log.LogMethodExit(null, "Update Legacy Cards Failed.");
                return;
            }

            btnTransfer.Enabled = true;
            btnInitiateTransfer.Enabled = false;
            txtMessage.Text = "Please tap the parafait card.";
            log.LogMethodExit();
        }

        private UlcKey GetLegacyUlcKey()
        {
            log.LogMethodEntry();
            string legacyUlcKeyString = Encryption.Decrypt(Encryption.GetParafaitKeys("LegacyULCAuthenticationKey"));
            UlcKey legacyUlcKey = new UlcKey(legacyUlcKeyString);
            log.LogMethodExit(legacyUlcKey);
            return legacyUlcKey;
        }

        private UlcKey GetNonStandardLegacyUlcKey()
        {
            log.LogMethodEntry();
            string nonStandardLegacyUlcKeyString = Encryption.Decrypt(Encryption.GetParafaitKeys("NonStandardUlcLegacyKey"));
            UlcKey nonStandardLegacyUlcKey = new UlcKey(nonStandardLegacyUlcKeyString);
            log.LogMethodExit(nonStandardLegacyUlcKey);
            return nonStandardLegacyUlcKey;
        }
        private bool ClearLegacyCard()
        {
            log.LogMethodEntry();
            int[] writeBlocks = new int[] { 1, 2, 4, 5, 8, 12, 13, 14, 16, 17 };
            byte[] customerAuthKey = new byte[6];


            string legacyKey = Encryption.Decrypt(Encryption.GetParafaitKeys("LegacyAuthenticationKey"));
            byte[] authKey = new byte[6];
            string[] sa = legacyKey.Substring(0, 17).Split('-');
            int i = 0;
            foreach (string s in sa)
            {
                authKey[i++] = Convert.ToByte(s, 16);
            }

            string legacyKey5 = Encryption.Decrypt(Encryption.GetParafaitKeys("LegacyAuthenticationKey5"));
            byte[] authKey5 = new byte[6];
            string[] sa5 = legacyKey5.Substring(0, 17).Split('-');
            i = 0;
            foreach (string s in sa5)
            {
                authKey5[i++] = Convert.ToByte(s, 16);
            }

            byte[] ffBuffer = new byte[16];
            i = 0;
            for (i = 0; i < 16; i++)
            {
                ffBuffer[i] = 0xff;
            }
            byte[] dataBuffer = new byte[16];
            string message = "";
            string legacyNewKey = Encryption.Decrypt(Encryption.GetParafaitKeys("NonStandardLegacyKey"));
            byte[] newKey = new byte[6];
            string[] newLegacy = legacyNewKey.Substring(0, 17).Split('-');
            i = 0;
            foreach (string s in newLegacy)
            {
                newKey[i++] = Convert.ToByte(s, 16);
            }
            try
            {
                if (legacyCard.ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    UlcKey legacyUlcKey = GetLegacyUlcKey();
                    UlcKey nonStandardUlcKey = GetNonStandardLegacyUlcKey();
                    if (!legacyCard.ReaderDevice.change_authentication_key((4 * i) + 3, legacyUlcKey.Value, nonStandardUlcKey.Value, ref message))
                    {
                        txtMessage.Text = message;
                        log.LogMethodExit(false, message);
                        return false;
                    }
                    foreach (int blockNum in writeBlocks)
                    {
                        if (!legacyCard.ReaderDevice.write_data(blockNum, 1, nonStandardUlcKey.Value, ffBuffer, ref message))
                        {
                            txtMessage.Text = message;
                            log.LogMethodExit(false, message);
                            return false;
                        }
                    }
                }
                else
                {
                    for (i = 0; i <= 3; i++)
                    {
                        if (!legacyCard.ReaderDevice.change_authentication_key((4 * i) + 3, authKey, newKey, ref message))
                        {
                            txtMessage.Text = message;
                            log.LogMethodExit(false, message);
                            return false;
                        }
                    }
                    if (!legacyCard.ReaderDevice.change_authentication_key((4 * i) + 3, authKey5, newKey, ref message))
                    {
                        txtMessage.Text = message;
                        log.LogMethodExit(false, message);
                        return false;
                    }
                    foreach (int blockNum in writeBlocks)
                    {
                        if (!legacyCard.ReaderDevice.write_data(blockNum, 1, newKey, ffBuffer, ref message))
                        {
                            txtMessage.Text = message;
                            log.LogMethodExit(false, message);
                            return false;
                        }
                    }
                }

                cardCleared = true;
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                txtMessage.Text = "Error in clearing the legacy card";
                log.Error("Error on clearing the unis card.", ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        private bool ValidateInput()
        {
            log.LogMethodEntry();
            decimal values = 0;
            if (string.IsNullOrEmpty(txtTranslatedCardNumber.Text))
            {
                txtMessage.Text = "Please tap the card and Get Details.";
                log.LogMethodExit(false, "Please tap the card and Get Details.");
                return false;
            }
            if (string.IsNullOrEmpty(txtCredits.Text))
            {
                txtMessage.Text = "Please enter the credits.";
                txtCredits.Focus();
                log.LogMethodExit(false, "Please enter the credits.");
                return false;
            }
            else
            {
                try
                {
                    values = decimal.Parse(txtCredits.Text);
                }
                catch
                {
                    txtMessage.Text = "Please enter the valid credits";
                    txtCredits.Focus();
                    log.LogMethodExit(false, "Please enter the valid credits.");
                    return false;
                }
            }
            string limitValue = "0";
            if (lookupValuesDTOList != null && lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("MANUAL_TOPUP_CREDIT_LIMIT")).ToList<LookupValuesDTO>().Count > 0)
            {
                limitValue = string.IsNullOrEmpty(lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("MANUAL_TOPUP_CREDIT_LIMIT")).ToList<LookupValuesDTO>()[0].Description) ? "0" : lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("MANUAL_TOPUP_CREDIT_LIMIT")).ToList<LookupValuesDTO>()[0].Description;
            }
            double limit = Convert.ToDouble(limitValue);
            double credits = Convert.ToDouble((string.IsNullOrEmpty(txtCredits.Text) ? "0" : txtCredits.Text));
            if (limit > 0 && limit < credits)
            {
                if (Authenticate.Manager(ref managerId) == false)
                {
                    log.LogMethodExit(null, "Manager Approval Failed.");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(txtBonus.Text))
            {
                txtBonus.Text = "0";
            }
            else
            {
                try
                {
                    values = decimal.Parse(txtBonus.Text);
                }
                catch
                {
                    txtMessage.Text = "Please enter the valid bonus.";
                    txtBonus.Focus();
                    log.LogMethodExit(false, "Please enter the valid bonus.");
                    return false;
                }
            }

            if (string.IsNullOrEmpty(txtCourtesy.Text))
            {
                txtCourtesy.Text = "0";
            }
            else
            {
                try
                {
                    values = decimal.Parse(txtCourtesy.Text);
                }
                catch
                {
                    txtMessage.Text = "Please enter the valid courtesy";
                    txtCourtesy.Focus();
                    log.LogMethodExit(false, "Please enter the valid courtesy");
                    return false;
                }
            }

            if (string.IsNullOrEmpty(txtCreditsPlayed.Text))
            {
                txtCreditsPlayed.Text = "0";
            }
            else
            {
                try
                {
                    values = decimal.Parse(txtCreditsPlayed.Text);
                }
                catch
                {
                    txtMessage.Text = "Please enter the valid credits played";
                    txtCreditsPlayed.Focus();
                    log.LogMethodExit(false, "Please enter the valid credits played");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(txtFaceValue.Text))
            {
                txtFaceValue.Text = "0";
            }
            else
            {
                try
                {
                    values = decimal.Parse(txtFaceValue.Text);
                }
                catch
                {
                    txtMessage.Text = "Please enter the valid face value.";
                    txtFaceValue.Focus();
                    log.LogMethodExit(false, "Please enter the valid face value.");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(txtLoyalty.Text))
            {
                txtLoyalty.Text = "0";
            }
            else
            {
                try
                {
                    values = decimal.Parse(txtLoyalty.Text);
                }
                catch
                {
                    txtMessage.Text = "Please enter the valid loyalty points.";
                    txtLoyalty.Focus();
                    log.LogMethodExit(false, "Please enter the valid loyalty points.");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(txtTickets.Text))
            {
                txtTickets.Text = "0";
            }
            else
            {
                try
                {
                    values = decimal.Parse(txtTickets.Text);
                }
                catch
                {
                    txtMessage.Text = "Please enter the valid tickets.";
                    txtTickets.Focus();
                    log.LogMethodExit(false, "Please enter the valid tickets.");
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(txtLastPlayedTime.Text))
            {
                DateTime dateTimevalue;
                try
                {
                    dateTimevalue = DateTime.ParseExact(txtLastPlayedTime.Text, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture);

                }
                catch
                {
                    try
                    {
                        dateTimevalue = DateTime.ParseExact(txtLastPlayedTime.Text, POSStatic.Utilities.ParafaitEnv.DATETIME_FORMAT, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        txtMessage.Text = "Please enter the valid played date.";
                        txtLastPlayedTime.Focus();
                        log.LogMethodExit(false, "Please enter the valid played date.");
                        return false;
                    }
                }
                if (dateTimevalue.CompareTo(POSStatic.Utilities.getServerTime()) > 0)
                {
                    txtMessage.Text = "Last played date should be less than or equal to today's date.";
                    txtLastPlayedTime.Focus();
                    log.LogMethodExit(false, "Last played date should be less than or equal to today's date.");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(txtTime.Text))
            {
                txtTime.Text = "0";
            }
            else
            {
                try
                {
                    values = decimal.Parse(txtTime.Text);
                }
                catch
                {
                    txtMessage.Text = "Please enter the valid time.";
                    txtTime.Focus();
                    log.LogMethodExit(false, "Please enter the valid time.");
                    return false;
                }
            }
            log.LogMethodExit(true);
            return true;
        }
        private bool UpdateLegacyCards()
        {
            log.LogMethodEntry();
            string cardNumber = txtTranslatedCardNumber.Text;
            List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>> SearchByParameters = new List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>>();
            SearchByParameters.Add(new KeyValuePair<LegacyCardDTO.SearchByParameters, string>(LegacyCardDTO.SearchByParameters.CARD_NUMBER_OR_PRINTED_CARD_NUMBER, cardNumber));
            List<LegacyCardDTO> LegacyCardDTOList = new List<LegacyCardDTO>();
            LegacyCardListBL legacyCardListBL = new LegacyCardListBL(POSStatic.Utilities.ExecutionContext, LegacyCardDTOList, Utilities);
            ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction();
            dBTransaction.BeginTransaction();
            LegacyCardDTOList = legacyCardListBL.GetLegacyCardDTOList(SearchByParameters, dBTransaction.SQLTrx);
            LegacyCardDTO legacyCardDTO = new LegacyCardDTO();
            try
            {
                if (LegacyCardDTOList != null && LegacyCardDTOList.Any())
                {
                    legacyCardDTO = LegacyCardDTOList[0];
                    legacyCardDTO.RevisedFaceValue = Convert.ToDecimal(txtFaceValue.Text);
                    legacyCardDTO.RevisedTicketCount = Convert.ToInt32(txtTickets.Text);
                    legacyCardDTO.RevisedCredits = Convert.ToDecimal(txtCredits.Text);
                    legacyCardDTO.RevisedCourtesy = Convert.ToDecimal(txtCourtesy.Text);
                    legacyCardDTO.RevisedBonus = Convert.ToDecimal(txtBonus.Text);
                    legacyCardDTO.RevisedTime = Convert.ToDecimal(txtTime.Text);
                    legacyCardDTO.RevisedCreditsPlayed = Convert.ToDecimal(txtCreditsPlayed.Text);
                    legacyCardDTO.RefundFlag = 'N';
                    legacyCardDTO.ValidFlag = 'N';
                    legacyCardDTO.VipCustomer = 'N';
                    legacyCardDTO.TechnicianCard = 'N';
                    legacyCardDTO.TechGames = -1;
                    legacyCardDTO.TimerResetCard = 'N';
                    legacyCardDTO.Status = 'S';
                    legacyCardDTO.Transferred = 'Y';
                    legacyCardDTO.TransferDate = ServerDateTime.Now;
                    if (string.IsNullOrEmpty(txtLastPlayedTime.Text))
                    {
                        legacyCardDTO.RevisedLastPlayedTime = DateTime.MinValue;
                    }
                    else
                    {
                        legacyCardDTO.RevisedLastPlayedTime = Convert.ToDateTime(txtLastPlayedTime.Text);
                    }
                    legacyCardDTO.RevisedLoyaltyPoints = Convert.ToDecimal(txtLoyalty.Text);
                    legacyCardDTO.RevisedBy = POSStatic.Utilities.ExecutionContext.GetUserId();
                    if (managerId != -1)
                        legacyCardDTO.ApprovedBy = managerId.ToString();
                    else
                        legacyCardDTO.ApprovedBy = "";

                    if (cardCleared)
                    {
                        legacyCardDTO.CardClearedDate = POSStatic.Utilities.getServerTime();
                        legacyCardDTO.CardClearedBy = POSStatic.Utilities.ExecutionContext.GetUserId();
                    }
                }
                else
                {
                    legacyCardDTO.CardId = -1;
                    legacyCardDTO.CardNumber = txtTranslatedCardNumber.Text;
                    legacyCardDTO.IssueDate = POSStatic.Utilities.getServerTime();
                    legacyCardDTO.FaceValue = Convert.ToDecimal(txtFaceValue.Text);
                    legacyCardDTO.RefundFlag = 'N';
                    legacyCardDTO.TicketCount = Convert.ToInt32(txtTickets.Text);
                    legacyCardDTO.LastUpdateTime = POSStatic.Utilities.getServerTime();
                    legacyCardDTO.Credits = Convert.ToDecimal(txtCredits.Text);
                    legacyCardDTO.Courtesy = Convert.ToDecimal(txtCourtesy.Text);
                    legacyCardDTO.Bonus = Convert.ToDecimal(txtBonus.Text);
                    legacyCardDTO.Time = Convert.ToDecimal(txtTime.Text);
                    legacyCardDTO.CreditsPlayed = Convert.ToDecimal(txtCreditsPlayed.Text);
                    legacyCardDTO.RefundFlag = 'N';
                    legacyCardDTO.ValidFlag = 'N';
                    //legacyCardDTO.TicketAllowed = 'Y';
                    //legacyCardDTO.RealTicketMode = 'Y';
                    legacyCardDTO.VipCustomer = 'N';
                    legacyCardDTO.TechnicianCard = 'N';
                    legacyCardDTO.TechGames = -1;
                    legacyCardDTO.TimerResetCard = 'N';
                    legacyCardDTO.Status = 'S';
                    legacyCardDTO.Transferred = 'Y';
                    legacyCardDTO.TransferDate = ServerDateTime.Now;
                    if (POSStatic.Utilities.ExecutionContext.GetSiteId() == -1)
                    {
                        legacyCardDTO.SiteId = -1;
                    }
                    else
                    {
                        legacyCardDTO.SiteId = POSStatic.Utilities.ExecutionContext.GetSiteId();
                    }
                    if (string.IsNullOrEmpty(txtLastPlayedTime.Text))
                    {
                        legacyCardDTO.LastPlayedTime = DateTime.MinValue;
                    }
                    else
                    {
                        legacyCardDTO.LastPlayedTime = Convert.ToDateTime(txtLastPlayedTime.Text);
                    }
                    legacyCardDTO.TimerResetCard = 'N';
                    legacyCardDTO.LoyaltyPoints = Convert.ToDecimal(txtLoyalty.Text);
                    legacyCardDTO.LastUpdatedBy = POSStatic.Utilities.ExecutionContext.GetUserId();
                    if (cardCleared)
                    {
                        legacyCardDTO.CardClearedDate = POSStatic.Utilities.getServerTime();
                        legacyCardDTO.CardClearedBy = POSStatic.Utilities.ExecutionContext.GetUserId();
                    }
                    legacyCardDTO.Transferred = 'N';
                    if (managerId != -1)
                    {
                        legacyCardDTO.ApprovedBy = managerId.ToString();
                    }
                    else
                    {
                        legacyCardDTO.ApprovedBy = "";
                    }
                    dBTransaction.EndTransaction();
                    this.legacyCardDTO = legacyCardDTO;
                    return true;
                }

                LegacyCardBL legacyCardBL = new LegacyCardBL(POSStatic.Utilities.ExecutionContext, legacyCardDTO, Utilities);
                legacyCardBL.Save(dBTransaction.SQLTrx);
                dBTransaction.SQLTrx.Commit();
                this.legacyCardDTO = legacyCardDTO;
                managerId = -1;
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                dBTransaction.RollBack();
                log.Error(ex);
                txtMessage.Text = ex.Message;
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                ParafaitCon.Close();
            }
        }

        private void LegacyCardScanEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Text = "";
            legacyCard = new Card(sender as DeviceClass, POSStatic.Utilities);
            //AddLegacyCardKey();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                string CardNumber = checkScannedEvent.Message;
                try
                {
                    LegacyCardSwiped(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    log.Fatal("Error" + ex.Message);
                }
            }
            log.LogMethodExit();
        }
        private void LegacyCardSwiped(string CardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(CardNumber, readerDevice);
            if (legacyCard == null)
                legacyCard = new Card(readerDevice, POSStatic.Utilities);
            string cardNumber = "";
            byte[] customerAuthKey = new byte[6];
            int customerKey = POSStatic.Utilities.MifareCustomerKey;

            string legacyKey = Encryption.Decrypt(Encryption.GetParafaitKeys("LegacyAuthenticationKey"));
            byte[] authKey = new byte[6];
            string[] sa = legacyKey.Substring(0, 17).Split('-');
            int i = 0;
            foreach (string s in sa)
            {
                authKey[i++] = Convert.ToByte(s, 16);
            }
            byte[] dataBuffer = new byte[16];
            string message = "";
            try
            {
                byte[] key = authKey;
                if (legacyCard.ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    key = GetLegacyUlcKey().Value;
                }

                if (legacyCard.ReaderDevice.read_data(1, 1, key, ref dataBuffer, ref message))
                {
                    cardNumber = BitConverter.ToString(dataBuffer).Substring(0, 14).Replace("-", "");
                    txtLegacyCardNumber.Text = cardNumber;
                }
                else
                {
                    txtLegacyCardNumber.Text = txtTranslatedCardNumber.Text;
                    txtMessage.Text = message;
                    log.Info(message);
                }
            }
            catch (Exception ex)
            {
                txtMessage.Text = "Error on reading the unis card.";
                log.Error("Error on reading the unis card.", ex);
                return;
            }
            log.LogMethodExit();
        }

        private void dtpLastPlayed_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtLastPlayedTime.Text = dtpLastPlayed.Value.ToString("dd-MMM-yyyy hh:mm tt");
            log.LogMethodExit();
        }
        private void TranslateCardnumber()
        {
            log.LogMethodEntry();
            string prefix = lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("PrefixCharacter")).ToList<LookupValuesDTO>()[0].Description;
            string suffix = lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("SuffixCharacter")).ToList<LookupValuesDTO>()[0].Description;

            string cardNumber = txtLegacyCardNumber.Text;

            if (prefix != "")
            {
                int pos = cardNumber.IndexOf(prefix);
                if (pos >= 0)
                    cardNumber = cardNumber.Substring(pos + prefix.Length);
            }
            int ignoreLeadingDigits = Convert.ToInt32(lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("IgnoreLeadingDigits")).ToList<LookupValuesDTO>()[0].Description);
            if (ignoreLeadingDigits > 0 && cardNumber.Length > ignoreLeadingDigits)
                cardNumber = cardNumber.Substring(ignoreLeadingDigits);

            if (ignoreLeadingDigits > 0 && cardNumber.Length > ignoreLeadingDigits)
                cardNumber = cardNumber.Substring(0, cardNumber.Length - ignoreLeadingDigits);
            else if (suffix != "")
                cardNumber = cardNumber.TrimEnd(suffix[0]);

            txtTranslatedCardNumber.Text = cardNumber;
            log.LogMethodExit();
        }
        private bool AddLegacyCardKey()
        {
            log.LogMethodEntry();
            int[] writeBlocks = new int[] { 1, 2, 4, 5, 8, 12, 13, 14, 16, 17 };
            byte[] customerAuthKey = new byte[6];
            int customerKey = POSStatic.Utilities.MifareCustomerKey;

            string legacyKey = Encryption.Decrypt(Encryption.GetParafaitKeys("LegacyAuthenticationKey"));
            byte[] authKey = new byte[6];
            string[] sa = legacyKey.Substring(0, 17).Split('-');
            int i = 0;
            foreach (string s in sa)
            {
                authKey[i++] = Convert.ToByte(s, 16);
            }

            string legacyKey5 = Encryption.Decrypt(Encryption.GetParafaitKeys("LegacyAuthenticationKey5"));
            byte[] authKey5 = new byte[6];
            string[] sa5 = legacyKey5.Substring(0, 17).Split('-');
            i = 0;
            foreach (string s in sa5)
            {
                authKey5[i++] = Convert.ToByte(s, 16);
            }

            //byte[] siteIdBuffer = new byte[16] { 0x17, 0x00, 0x33, 0x39, 0x84, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            byte[] siteIdBuffer = new byte[16] { 0x16, 0x01, 0x01, 0x44, 0x24, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            //byte[] siteIdBuffer = new byte[16] { 0x18, 0x00, 0x60, 0x61, 0x48, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            //byte[] siteIdBuffer = new byte[16] { 0x18, 0x01, 0x41, 0x12, 0x19, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            //siteIdBuffer = BitConverter.GetBytes(txtLegacyCardNumber.Text + "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF");
            i = 0;

            byte[] dataBuffer = new byte[16];
            string message = "";
            string legacyNewKey = Encryption.Decrypt(Encryption.GetParafaitKeys("NonStandardLegacyKey"));
            byte[] newKey = new byte[6];
            string[] newLegacy = legacyNewKey.Substring(0, 17).Split('-');
            i = 0;
            foreach (string s in newLegacy)
            {
                newKey[i++] = Convert.ToByte(s, 16);
            }
            try
            {
                foreach (int blockNum in writeBlocks)
                {
                    if (!legacyCard.ReaderDevice.write_data(blockNum, 1, newKey, siteIdBuffer, ref message))
                    {
                        txtMessage.Text = message;
                        log.LogMethodExit(false, message);
                        return false;
                    }
                }
                for (i = 0; i <= 3; i++)
                {
                    if (!legacyCard.ReaderDevice.change_authentication_key((4 * i) + 3, newKey, authKey, ref message))
                    {
                        txtMessage.Text = message;
                        log.LogMethodExit(false, message);
                        return false;
                    }
                }
                if (!legacyCard.ReaderDevice.change_authentication_key((4 * i) + 3, newKey, authKey5, ref message))
                {
                    txtMessage.Text = message;
                    log.LogMethodExit(false, message);
                    return false;
                }

                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                txtMessage.Text = "Error in clearing the legacy card";
                log.Error("Error on clearing the unis card.", ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        private void frmLegacyCashToParafait_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Common.Devices.UnregisterCardReaders();
            Common.Devices.UnRegisterUnAuthenticated();
            if (Common.Devices.PrimaryBarcodeScanner != null)
                Common.Devices.PrimaryBarcodeScanner.UnRegister();
            log.LogMethodExit();
        }
        private void btnEditCredits_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadFrmLegacyEntitlements(false, "CREDITS");
            log.LogMethodExit();

        }
        private void btnEditBonus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadFrmLegacyEntitlements(false, "BONUS");
            log.LogMethodExit();
        }

        private void btnEditTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadFrmLegacyEntitlements(false, "TIME");
            log.LogMethodExit();
        }

        private void btnEditTickets_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadFrmLegacyEntitlements(false, "TICKETS");
            log.LogMethodExit();
        }

        private void btnEditLoyaltyPoints_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadFrmLegacyEntitlements(false, "LOYALTYPOINTS");
            log.LogMethodExit();
        }

        private void LoadFrmLegacyEntitlements(bool readOnly, string entitlement)
        {
            log.LogMethodEntry(readOnly, entitlement);
            frmLegacyEntitlements frmLegacyEntitlements = new frmLegacyEntitlements(POSStatic.Utilities.ExecutionContext, readOnly, legacyCardDTO, entitlement);
            frmLegacyEntitlements.ShowDialog();
            legacyCardDTO = (LegacyCardDTO)frmLegacyEntitlements.Tag;
            List<LegacyCardDTO> legacyCardDTOList = new List<LegacyCardDTO>();
            legacyCardDTOList.Add(legacyCardDTO);
            LegacyCardBuilderBL legacyCardBuilderBL = new LegacyCardBuilderBL(POSStatic.Utilities.ExecutionContext, legacyCardDTOList);
            legacyCardDTOList = legacyCardBuilderBL.LoadLegacyCardSummary(legacyCardDTOList);
            this.legacyCardDTO = legacyCardDTOList[0];
            DisplayEntitlements(legacyCardDTO);
            log.LogMethodExit(legacyCardDTO);
        }

        private void btnEditGames_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadFrmLegacyEntitlements(false, "GAMES");
            log.LogMethodExit();
        }
    }
}
