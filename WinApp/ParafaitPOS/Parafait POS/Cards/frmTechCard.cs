/********************************************************************************************
 * Project Name - Staff Card Management UI, frm TechCadrd
 * Description  - UI for creating or updating Technician Card
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Amaresh         Created 
 ********************************************************************************************
 *1.00        24-Apr-2017   Suneetha        Modified 
 ********************************************************************************************
 *2.60        23-Feb-2019   Mathew          Added support for peripheral devices attached to POS
 *2.70        1-Jul-2019    Lakshminarayana Modified to add support for ULC cards 
 *2.80.0      20-Aug-2019   Girish Kundar       Modified :  Added logger methods and Removed unused namespace's
 *2.70.3      01-Jan-2020   Mathew Ninan    Device Registration logic is modified. It was reinitializing reader device
 *                                          leading to continuous beep issue
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Parafait_POS
{
    public partial class frmTechCard : Form
    {
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        internal RemotingClient CardRoamingRemotingClient = null;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        bool selected = false;
        Transaction transaction;
        Utilities Utilities;
        USBDevice listener;
        DeviceClass readerDevice = null;
        object site_id = -1;
        private readonly TagNumberParser tagNumberParser;

        public frmTechCard(Utilities _utilities)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);//Added for generating logger root Loglevel on 08-Mar-2016
            //End: site_id to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry();
            InitializeComponent();
            Utilities = _utilities;
            Utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);

            if (Utilities.ParafaitEnv.IsCorporate)
            {
                site_id = Utilities.ParafaitEnv.SiteId;
                machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                site_id = -1;
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(Utilities.ParafaitEnv.LoginID);
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));

            GetTechCardsCombo();
            GetStaffCardProducts();
            ClearFields();
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    txtMessage.Text = message;
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                try
                {
                    readerDevice = sender as DeviceClass;
                    HandleCardRead(tagNumber.Value);
                }
                catch (Exception ex)
                {
                    txtMessage.Text = ex.Message;
                    log.Fatal("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                }
            }
            log.LogMethodExit();
        }

        public void GetTechCardsCombo()
        {
            log.LogMethodEntry();
            try
            {
                UserIdentificationTagListBL userCardsList = new UserIdentificationTagListBL();
                List<UserIdentificationTagsDTO> userCards = userCardsList.GetUsersCards();
                cmbTechCards.SelectedIndexChanged -= cmbTechCards_SelectedIndexChanged;
                
                cmbTechCards.DataSource = userCards;
                cmbTechCards.DisplayMember = "CardNumber";
                cmbTechCards.ValueMember = "CardId";
                cmbTechCards.SelectedIndex = -1;
                cmbTechCards.SelectedIndexChanged += cmbTechCards_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                log.Error("Ends-GetTechCardsCombo() Error: " + ex.Message);
            }
            log.LogMethodExit();
        }

        void ClearFields()
        {
            log.LogMethodEntry();
            txtCardNumber.Clear();
            txtMessage.Clear();
            txtNotes.Clear();
            txtTechnicianName.Clear();
            txtTechnicianLastName.Clear(); 
            txtTickets.Text = "0";
            txtCredits.Text = "0";
            lnkDeactivate.Enabled = false;
            txtBalanceGames.Text = "0";
            dtpValidTill.CustomFormat = " ";
            UpdateButtons();
            txtProduct.Text = "";
            txtProduct.Tag = null;
            txtNotes.Enabled = false;
            log.LogMethodExit();
        }

        void HandleCardRead(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            try
            {
                cmbTechCards.SelectedIndex = -1;
                ClearFields();
                txtMessage.Text = "";

                Card card = new Card(readerDevice, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                string message = "";
                
                if (!refreshCardFromHQ(ref card, ref message))
                {
                    txtMessage.Text = message;
                    log.Info("Ends-handleCardRead(" + CardNumber + ") as unable to refresh card from HQ");
                    log.LogMethodExit();
                    return;
                }

                if (card.CardStatus.Equals("ISSUED"))
                {
                    //enable deactive link only issued card added on 23-03-17
                    lnkDeactivate.Enabled = true;

                    //enable selecting staff card product only for staff card
                    if (card.technician_card == 'Y')
                        flpStaffCardProducts.Enabled = true;

                    if (card.technician_card != 'Y')
                    {
                        txtMessage.Text = "Not a Staff Card";
                        log.Info("Ends-HandleCardRead(" + CardNumber + ") as issued card is not a Staff Card");
                        log.LogMethodExit();
                        return;
                    }

                    cmbTechCards.SelectedValue = card.card_id;

                    //to select or deselect the product button
                    UpdateButtons();
                }
                else
                {
                    lnkDeactivate.Enabled = false;
                    lnkSelectStaff.Enabled = true;
                    txtCardNumber.Text = card.CardNumber;
                    txtNotes.Enabled = true;
                    if (string.IsNullOrEmpty(txtTechnicianName.Text) || txtTechnicianName.Tag == null)
                    {
                        flpStaffCardProducts.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-HandleCardRead(" + CardNumber + ") Error : " + ex.Message);
            }
            log.LogMethodExit();
        }

        void GetStaffCardProducts()
        {
            log.LogMethodEntry();
            try
            {
                Products productList = new Products();
                int displayGrpId = -1;

                if (!DBNull.Value.Equals(Utilities.getParafaitDefaults("STAFF_CARD_PRODUCTS_DISPLAY_GROUP")))
                    displayGrpId = Convert.ToInt32(Utilities.getParafaitDefaults("STAFF_CARD_PRODUCTS_DISPLAY_GROUP"));  

                ProductsFilterParams parameters = new ProductsFilterParams(displayGrpId, Utilities.ParafaitEnv.POSTypeId, Utilities.ParafaitEnv.POSMachineId);
                List<ProductsStruct> listStaffCardProducts = productList.GetStaffCardProducts(parameters);
                log.LogVariableState("listStaffCardProducts", listStaffCardProducts);
                if (listStaffCardProducts != null && listStaffCardProducts.Count > 0)
                {
                    for (int i = 0; i < listStaffCardProducts.Count; i++)
                    {
                        Button CardProductButton = new Button();
                        CardProductButton.Click += new EventHandler(btnSample_Click);
                        CardProductButton.Name = "ProductButton" + i.ToString();

                        if (listStaffCardProducts[i].ProductId != -1)
                        {
                            if (!string.IsNullOrEmpty(listStaffCardProducts[i].ProductName))
                                CardProductButton.Text = listStaffCardProducts[i].ProductName.ToString();

                            CardProductButton.Tag = listStaffCardProducts[i].ProductId;

                            CardProductButton.Font = btnSample.Font;
                            CardProductButton.ForeColor = btnSample.ForeColor;
                            CardProductButton.Size = btnSample.Size;
                            CardProductButton.FlatStyle = btnSample.FlatStyle;
                            CardProductButton.FlatAppearance.BorderColor = btnSample.FlatAppearance.BorderColor;
                            CardProductButton.FlatAppearance.BorderSize = btnSample.FlatAppearance.BorderSize;
                            CardProductButton.FlatAppearance.MouseDownBackColor = btnSample.FlatAppearance.MouseOverBackColor = Color.Transparent;
                            CardProductButton.BackgroundImageLayout = ImageLayout.Zoom;
                            CardProductButton.BackColor = Color.Transparent;
                            CardProductButton.BackgroundImage = btnSample.BackgroundImage;

                            CardProductButton.Enabled = true;
                            flpStaffCardProducts.Controls.Add(CardProductButton);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-GetStaffCardProducts() error" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnSample_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (!string.IsNullOrEmpty(txtTechnicianName.Text) && txtTechnicianName.Tag != null)
            {
                txtMessage.Text = "";
                object productId = (sender as Button).Tag;

                int pId = DBNull.Value.Equals(productId) ? -1 : Convert.ToInt32(productId);
   
                bool isLimitAllowed = true;
                string message = string.Empty;
                StaffCardManagementDataHandler dataHandle = new StaffCardManagementDataHandler(Utilities);
                isLimitAllowed = dataHandle.CheckStaffCardCreditLimit(txtCardNumber.Text, pId, ref message);
                txtMessage.Text = message;
                if (isLimitAllowed)
                {
                    if (!selected)
                    {
                        txtNotes.Enabled = true;
                        UpdateButtonColors(true, productId);
                        selected = true;
                    }
                    else
                    {
                        txtNotes.Enabled = false;
                        UpdateButtonColors(false, productId);
                        selected = false;
                    }
                }
            }
            else
            {
                txtMessage.Text = Utilities.MessageUtils.getMessage(1162);
            }
            log.LogMethodExit();
        }

        void UpdateButtonColors(bool select, object productId)
        {
            log.LogMethodEntry(select);
            if (select)
            {
                foreach (Control c in flpStaffCardProducts.Controls)
                {
                    if (c is Button && c.Tag != null && c.Tag.Equals(productId))
                    {
                        txtProduct.Text = c.Text;
                        txtProduct.Tag = c.Tag;
                        c.BackgroundImage = Properties.Resources.ManualProduct;
                        c.Enabled = true;
                    }
                    else if (c is Button && c.Tag != null)
                    {
                        c.Enabled = false;
                    }
                }
            }
            else
            {
                foreach (Control c in flpStaffCardProducts.Controls)
                {

                    if (c is Button && c.Tag != null)
                    {
                        c.BackgroundImage = Properties.Resources.ComboProduct;
                        c.Enabled = true;
                    }
                    txtProduct.Text = "";
                    txtProduct.Tag = null;
                }
            }
            log.LogMethodExit();
        }

        void GetCardDetails(Card card)
        {
            log.LogMethodEntry(card);
            txtCardNumber.Text = card.CardNumber;
            txtNotes.Text = card.notes;

            txtTickets.Text = card.ticket_count.ToString();

            txtCredits.Text = Convert.ToString(card.credits);
            lnkDeactivate.Enabled = true;

            UserIdentificationTagsDTO userIdTagDT = null;

            if (cmbTechCards.SelectedItem != null && cmbTechCards.SelectedIndex != -1)
                   userIdTagDT = cmbTechCards.SelectedItem as UserIdentificationTagsDTO;
            
            //add user dto to technician name text box
            UsersDTO userObj = null;
            UsersList userLst = new UsersList(machineUserContext);
            if (userIdTagDT != null)
            {
                if (!DBNull.Value.Equals(userIdTagDT.UserId))
                {
                    try
                    {
                        Users user = new Users(Utilities.ExecutionContext, Convert.ToInt32(userIdTagDT.UserId));
                        userObj = user.UserDTO;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        userObj = null;
                    }
                }
            }
            if (userObj != null)
            {
                flpStaffCardProducts.Enabled = true;
                txtTechnicianName.Tag = userObj;
                txtTechnicianName.Text = userObj.UserName;
                txtTechnicianLastName.Text = userObj.EmpLastName;
                lnkSelectStaff.Enabled = false;
            }

            //get card games details
            StaffCardManagementDataHandler staffCardHandler = new StaffCardManagementDataHandler(Utilities);
            DataTable dt = staffCardHandler.GetCardGames(card.card_id);

            if (dt != null && dt.Rows.Count > 0)
            {
                int balanceGames = 0;
                dtpValidTill.CustomFormat = Utilities.ParafaitEnv.DATE_FORMAT;
                dtpValidTill.Value = dt.Rows[0]["ExpiryDate"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["ExpiryDate"]) : ServerDateTime.Now.AddYears(1);
                balanceGames = Convert.ToInt32(dt.Rows[0]["BalanceGames"]);
                txtBalanceGames.Text = balanceGames.ToString();
            }
            else
            {
                txtBalanceGames.Text = card.tech_games.ToString();
                dtpValidTill.CustomFormat = " ";
            }
            log.LogMethodExit();
        }

        private void cmbTechCards_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cmbTechCards.SelectedIndex == -1)
            {
                log.Info("Ends-cmbTechCards_SelectedIndexChanged() as cmbTechCards.SelectedIndex == -1");
                return;
            }

            txtMessage.Text = "";

            ClearFields();
            Card card = new Card((int)cmbTechCards.SelectedValue, Utilities.ParafaitEnv.LoginID, Utilities);
            GetCardDetails(card);
            log.LogMethodExit();
        }

        private void lnkAdvanced_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            if (!save())
            {
                log.Info("Ends-lnkAdvanced_LinkClicked() as unable to save() ");
                log.LogMethodExit();
                return;
            }
            log.LogMethodExit();
        }

        bool LoadBonus(Card card)
        {
            log.LogMethodEntry(card);
            double credits = 0;
            try
            {
                credits = Convert.ToDouble(txtCredits.Text);
                credits = credits - card.credits;
            }
            catch
            {
                log.Fatal("Ends-LoadBonus(" + card + ") due to exception in credits ");
            }

            double limit = 0;
            double.TryParse(Utilities.getParafaitDefaults("LOAD_BONUS_LIMIT"), out limit);

            if (credits > limit)
            {
                txtMessage.Text = Utilities.MessageUtils.getMessage(43, limit);
                this.ActiveControl = txtTickets;
                log.Info("Ends-LoadBonus(" + card + ") as entered a value less than or equal to " + limit + " for Bonus");//Added for logger function on  08-Mar-2016        
                log.LogMethodExit(false);
                return false;
            }

            TaskProcs tp = new TaskProcs(Utilities);
            string message = "";
            TaskProcs.EntitlementType bonusType;
            if (Utilities.getParafaitDefaults("LOAD_CREDITS_INSTEAD_OF_CARD_BALANCE").Equals("Y"))
                bonusType = TaskProcs.EntitlementType.Credits;
            else
                bonusType = TaskProcs.EntitlementType.Bonus;

            if (credits != 0)
            {
                if (!tp.loadBonus(card,
                                    (double)credits,
                                    bonusType,
                                    false,
                                    -1,
                                    "Load bonus. Staff Card: " + card.CardNumber + (card.customerDTO == null ? "" : " / " + card.customerDTO.FirstName),
                                    ref message))
                {
                    txtMessage.Text = message;
                    log.Info("Ends-LoadBonus(" + card + ") as unable to load bonus due to " + message);//Added for logger function on 08-Mar-2016
                    log.LogMethodExit(false);
                    return false;
                }
                else
                {
                    log.Debug("Ends-LoadBonus(" + card + ")");
                    log.LogMethodExit(true);
                    return true;
                }
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
        }

        bool save()
        {
            log.LogMethodEntry();
            string message = "";
            int userTaggedId = -1;
            bool userExist = false;
            if (txtCardNumber.Text.Equals(""))
            {
                txtMessage.Text = Utilities.MessageUtils.getMessage(257);
                log.Info("Ends-save() as no cards are Tapped ");//Added for logger function on 08-Mar-2016
                return false;
            }

            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            transaction = new Transaction(POSStatic.POSPrintersDTOList, Utilities);
            try
            {
                Card card = new Card(txtCardNumber.Text, Utilities.ParafaitEnv.LoginID, Utilities);
                card.notes = txtNotes.Text;
                card.tech_games = 0;
                card.technician_card = 'Y';
                //Added on 7-oct-2016 for making first and last name mandatory
                if (txtTechnicianName.Text.Trim().Equals("") == true)
                {
                    txtMessage.Text = Utilities.MessageUtils.getMessage(1162);
                    log.Info("Ends-save() as first name mandatory ");//Added for logger function on 08-Mar-2016
                    return false;
                }

                #region Load Products into Card
                if (!string.IsNullOrEmpty(txtProduct.Text) && txtProduct.Tag != null)
                {
                    //create transaction and save
                    int retVal = transaction.createTransactionLine(card, Convert.ToInt32(txtProduct.Tag), 1, ref message);

                    if (retVal != 0)
                    {
                        txtMessage.Text = message;
                        return false;
                    }
                    for (int i = 0; i < transaction.TrxLines.Count; i++)
                    {
                        transaction.TrxLines[i].Price = 0;
                        transaction.TrxLines[i].LineAmount = 0;
                    }
                    transaction.CashAmount = 0;
                    transaction.Transaction_Amount = 0;
                    transaction.Net_Transaction_Amount = 0;
                    int retcode = transaction.SaveTransacation(ref message);
                    if (retcode != 0)
                    {
                        txtMessage.Text = message;
                        return false;
                    }
                }
                else//if product not selected, create just staff card with no credits
                {
                    if (card.CardStatus.Equals("NEW"))
                    {
                        card.technician_card = 'Y';
                        card.createCard(SQLTrx);
                    }
                }
                #endregion

                #region Tag user card               
                if (!string.IsNullOrEmpty(card.CardNumber))
                {
                    if (!string.IsNullOrEmpty(txtTechnicianName.Text) && txtTechnicianName.Tag != null)
                    {
                        if (card.CardStatus.Equals("ISSUED"))
                        {
                            UsersDTO userObj = txtTechnicianName.Tag as UsersDTO;
                            if (userObj != null)
                            {
                                userExist = CheckUserExist(userObj.UserId);
                            }
                        }

                        //create user if not exists
                        if (!userExist)
                        {
                            userTaggedId = AddTechniciantoCard(card, ref message);
                            if (userTaggedId < 0)
                            {
                                txtMessage.Text = message;
                                return false;
                            }
                        }
                    }
                }
                #endregion

                if (userTaggedId != -1 && userTaggedId != 0)
                {
                    SQLTrx.Commit();
                }

                //clear all selected fields
                ClearFields();

                //refresh card object
                card.getCardDetails(card.card_id);

                try
                {
                    //update cardId to UserIdentification tag
                    if (card != null && card.card_id != -1 && !string.IsNullOrEmpty(card.CardNumber) && card.valid_flag == 'Y')
                    {
                        UserIdentificationTagsDTO userIdentificationTagsDTO = GetUserIdentificationTagsDTO(card.CardNumber);
                        if(userIdentificationTagsDTO != null)
                        {
                            Users user = new Users(Utilities.ExecutionContext, userIdentificationTagsDTO.UserId, true, true);
                            UsersDTO usersDTO = user.UserDTO;
                            foreach (var item in usersDTO.UserIdentificationTagsDTOList)
                            {
                                if(item.CardNumber == card.CardNumber)
                                {
                                    item.CardId = card.card_id;
                                }
                            }
                            user = new Users(Utilities.ExecutionContext, usersDTO);
                            using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                            {
                                parafaitDBTransaction.BeginTransaction();
                                user.Save(parafaitDBTransaction.SQLTrx);
                                parafaitDBTransaction.EndTransaction();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Ends-save() due to exception " + ex.Message);
                }
                //refresh staff cards combo box
                GetTechCardsCombo();

                cmbTechCards.SelectedValue = card.card_id;

                txtMessage.Text = Utilities.MessageUtils.getMessage(122);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                SQLTrx.Rollback();
                txtMessage.Text = ex.Message;
                log.Fatal("Ends-save() due to exception " + ex.Message);
                return false;
            }
            finally
            {
                log.Debug("Ends-save()");
                cnn.Close();
            }
        }

        public int AddTechniciantoCard(Card card, ref string message)
        {
            log.LogMethodEntry();
            int userTagId = -1;
            try
            {
                if (!string.IsNullOrEmpty(txtTechnicianName.Text) && txtTechnicianName.Tag != null)
                {
                    UsersDTO userDTO = txtTechnicianName.Tag as UsersDTO;
                    LookupValuesList lookupValuesListBL = new LookupValuesList(machineUserContext);

                    if (userDTO != null)
                    {
                        UserIdentificationTagsDTO userIdTagDTO = GetUserIdentificationTagsDTO(card.CardNumber);
                        if (userIdTagDTO == null)
                        {
                            Users users = new Users(Utilities.ExecutionContext, userDTO.UserId, true, true);
                            UsersDTO usersDTO = users.UserDTO;
                            if(usersDTO.UserIdentificationTagsDTOList == null)
                            {
                                usersDTO.UserIdentificationTagsDTOList = new List<UserIdentificationTagsDTO>();
                            }
                            
                            userIdTagDTO = new UserIdentificationTagsDTO();
                            userIdTagDTO.CardNumber = card.CardNumber;
                            userIdTagDTO.UserId = userDTO.UserId;
                            userIdTagDTO.ActiveFlag = true;
                            userIdTagDTO.LastUpdatedBy = Utilities.ParafaitEnv.LoginID;
                            userIdTagDTO.AttendanceReaderTag = true;
                            userIdTagDTO.StartDate = lookupValuesListBL.GetServerDateTime();
                            usersDTO.UserIdentificationTagsDTOList.Add(userIdTagDTO);
                            using(ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                            {
                                parafaitDBTransaction.BeginTransaction();
                                users = new Users(Utilities.ExecutionContext, usersDTO);
                                users.Save(parafaitDBTransaction.SQLTrx);
                                parafaitDBTransaction.EndTransaction();
                            }
                            userTagId = users.UserDTO.UserIdentificationTagsDTOList.FirstOrDefault(x => x.CardNumber == card.CardNumber).Id;
                        }
                        else
                        {
                            message = Utilities.MessageUtils.getMessage(1163);
                        }
                    }
                }
                else
                {
                    message = Utilities.MessageUtils.getMessage(1162);
                }
                log.Debug("Ends-AddTechniciantoCard()");
            }
            catch (Exception ex)
            {
                log.Error("Ends-AddTechniciantoCard() Error while adding staff," + ex.Message);
            }
            log.LogMethodExit("userTagId");
            return userTagId;
        }

        public UserIdentificationTagsDTO GetUserIdentificationTagsDTO(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            UserIdentificationTagsDTO result = null;
            try
            {
                UserIdentificationTagListBL userIdentificationTagListBL = new UserIdentificationTagListBL();
                List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> userIdTagSearchParams = new List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>>();
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ACTIVE_FLAG, "1"));
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.CARD_NUMBER, cardNumber));
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.SITE_ID, Convert.ToString(site_id)));
                List<UserIdentificationTagsDTO> userIdentificationTagsDTOList = userIdentificationTagListBL.GetUserIdentificationTagsDTOList(userIdTagSearchParams);
                if (userIdentificationTagsDTOList != null && userIdentificationTagsDTOList.Count > 0)
                {
                    result = new UserIdentificationTagsDTO();
                    result = userIdentificationTagsDTOList[0];
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends - CheckCardExist Error: " + ex.Message);
                result = null;
            }
            log.LogMethodExit("userTagDTO");
            return result;
        }

        public bool CheckUserExist(int userId)
        {
            log.LogMethodEntry(userId);
            bool found = false;
            try
            {
                UserIdentificationTagsDTO userIdTagDTO = new UserIdentificationTagsDTO();
                UserIdentificationTagListBL userIdTagsList = new UserIdentificationTagListBL();
                List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> userIdTagSearchParams = new List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>>();

                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ACTIVE_FLAG, "1"));
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.USER_ID, Convert.ToString(userId)));
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.SITE_ID, Convert.ToString(site_id)));

                List<UserIdentificationTagsDTO> userIdTagList = userIdTagsList.GetUserIdentificationTagsDTOList(userIdTagSearchParams);

                if (userIdTagList != null && userIdTagList.Count > 0)
                {
                    found = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends - CheckUserExist Error: " + ex.Message);
                found = false;
            }
            log.LogMethodExit(found);
            return found;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            save();
            UpdateButtons();
            txtProduct.Text = "";
            txtProduct.Tag = null;
            log.LogMethodExit();
        }

        private void frmTechCard_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Common.Devices.UnregisterCardReaders();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void lnkDeactivate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                Card card = new Card(txtCardNumber.Text, Utilities.ParafaitEnv.LoginID, Utilities);
                DialogResult dialogResult = MessageBox.Show(Utilities.MessageUtils.getMessage(1159, card.card_id), "Warning", MessageBoxButtons.YesNo);
           
                if (DialogResult.Yes != dialogResult)
                    //ParafaitMessageBox(Utilities.MessageUtils.getMessage(1159, CardId), "Warning", MessageBoxButtons.YesNo))
                {
                    return;
                }
                else
                {
                    if (card.card_id > 0)
                    {
                        card.invalidateCard(null);
                        UserIdentificationTagsDTO userIdentificationTagsDTO = GetUserIdentificationTagsDTO(card.CardNumber);
                        if(userIdentificationTagsDTO != null)
                        {
                            Users user = new Users(Utilities.ExecutionContext, userIdentificationTagsDTO.UserId, true, true);
                            UsersDTO usersDTO = user.UserDTO;
                            foreach (var item in usersDTO.UserIdentificationTagsDTOList)
                            {
                                if(item.CardNumber == card.CardNumber)
                                {
                                    item.ActiveFlag = false;
                                    item.EndDate = ServerDateTime.Now;
                                }
                            }
                            user = new Users(Utilities.ExecutionContext, usersDTO);
                            using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                            {
                                parafaitDBTransaction.BeginTransaction();
                                user.Save(parafaitDBTransaction.SQLTrx);
                                parafaitDBTransaction.EndTransaction();
                            }
                        }

                        ClearFields();
                        flpStaffCardProducts.Enabled = false;
                        GetTechCardsCombo();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends - lnkDeactivate_LinkClicked Error: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void lnkSelectStaff_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                frmFindStaff frmStaff = new frmFindStaff(Utilities);
                DialogResult res = frmStaff.ShowDialog();
                if (res == DialogResult.OK)
                {
                    UsersDTO dto = frmStaff.UserObj;
                    if (dto != null)
                    {

                        bool isUserFound = CheckUserExist(dto.UserId);

                        if (isUserFound)
                        {
                            txtMessage.Text = Utilities.MessageUtils.getMessage(1160);
                            return;
                        }
                        else
                        {
                            txtTechnicianName.Text = dto.UserName.ToString();
                            txtTechnicianLastName.Text = dto.EmpLastName.ToString();
                            txtTechnicianName.Tag = dto;
                            txtMessage.Text = "";
                            flpStaffCardProducts.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-lnkSelectStaff_LinkClicked() Error: " + ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }

        public bool refreshCardFromHQ(ref Card CurrentCard, ref string message)
        {
            log.LogMethodEntry();
            try
            {
                CardUtils cardUtil = new CardUtils(Utilities);
                return cardUtil.getCardFromHQ(POSUtils.CardRoamingRemotingClient, ref CurrentCard, ref message);
            }
            catch (Exception ex)
            {
                log.Error("Ends -refreshCardFromHQ() "+ex.Message);
                if (ex.Message.ToLower().Contains("fault"))
                {
                    message = Utilities.MessageUtils.getMessage(216);
                    try
                    {
                        CardRoamingRemotingClient = new RemotingClient();
                    }
                    catch
                    {
                        message = Utilities.MessageUtils.getMessage(217);
                    }
                }
                else
                    message = "On-Demand Roaming: " + ex.Message;

                log.LogMethodExit(false);
                return false;
            }
        }

        void UpdateButtons()
        {
            log.LogMethodEntry();
            if (txtProduct.Tag != null)
            {
                if (!selected)
                {
                    txtNotes.Enabled = true;
                    UpdateButtonColors(true, txtProduct.Tag);
                    selected = true;
                }
                else
                {
                    txtNotes.Enabled = false;
                    UpdateButtonColors(false, txtProduct.Tag);
                    selected = false;
                }
            }
            log.LogMethodExit();
        }
        
    }
}
