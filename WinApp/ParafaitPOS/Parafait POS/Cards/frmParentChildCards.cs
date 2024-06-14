/********************************************************************************************************************
* Project Name - Parafait POS
* Description  - frmParentChildCards 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************************************
 *2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 *2.80.0      20-Aug-2019     Girish Kundar       Modified :  Added logger methods and Removed unused namespace's
 *                                                Added credit plus credits and item purchase 
 *2.100.0     09-Oct-2020     Indrajeet Kumar     Added design for supporting day limit percentage.                                                
********************************************************************************************************************/

using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Customer.Accounts;
using Logger = Semnox.Core.Utilities.Logger;

namespace Parafait_POS
{
    public partial class frmParentChildCards : Form
    {
        object _parentCard;
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        private readonly TagNumberParser tagNumberParser;
        private List<ParentChildCardsDTO> parentChildCardsDTOList;
        int TotalDailyLimitPercentage = 100;
        public frmParentChildCards(object parentCard = null)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry();
            InitializeComponent();
            _parentCard = parentCard;
            POSStatic.Utilities.setLanguage(this);//added on 26-Jul-2017
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            log.LogMethodExit();
        }

        public frmParentChildCards(List<ParentChildCardsDTO> parentChildCardDTOList)
            : this()
        {
            log.LogMethodEntry(parentChildCardDTOList);
            this.parentChildCardsDTOList = new List<ParentChildCardsDTO>();
            this.parentChildCardsDTOList.AddRange(parentChildCardDTOList);
            if (this.parentChildCardsDTOList.Exists(x => x.ParentCardId != -1))
            {
                int parentCardId = this.parentChildCardsDTOList.Where(x => x.ParentCardId != -1).FirstOrDefault().ParentCardId;
                Card parentCard = new Card(POSStatic.Utilities.ReaderDevice, parentCardId, POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);
                _parentCard = parentCard.CardNumber;
            }
            if (this.parentChildCardsDTOList != null && this.parentChildCardsDTOList.Count > 0)
            {
                btnSplitEqual.Tag = true;
                splitDailyLimitPercentage();
            }
            log.LogMethodExit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void frmParentChildCards_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            Common.Devices.UnregisterCardReaders();
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
                string scannedTagNumber = checkScannedEvent.Message;
                DeviceClass encryptedTagDevice = sender as DeviceClass;
                if (tagNumberParser.IsTagDecryptApplicable(encryptedTagDevice, checkScannedEvent.Message.Length))
                {
                    string decryptedTagNumber = string.Empty;
                    try
                    {
                        decryptedTagNumber = tagNumberParser.GetDecryptedTagData(encryptedTagDevice, checkScannedEvent.Message);
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number result: ", ex);
                        displayMessageLine(ex.Message);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, POSStatic.Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    displayMessageLine(message);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }
                log.LogVariableState("TagNumber", tagNumber);
                string CardNumber = tagNumber.Value;
                try
                {
                    CardSwiped(CardNumber);
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message);
                    log.Fatal("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                }
            }
            log.LogMethodExit();
        }

        private void frmParentChildCards_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));

            displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(12483));
            log.Info("frmParentChildCards_Load() - Tap Parent Card");//Added for logger function on 08-Mar-2016
            this.ActiveControl = dgvParentCard;

            Credits.DefaultCellStyle = dcCreditPlus.DefaultCellStyle = dcCredits.DefaultCellStyle =
                CreditPlus.DefaultCellStyle = POSStatic.Utilities.gridViewAmountCellStyle();

            if (this.parentChildCardsDTOList != null && this.parentChildCardsDTOList.Count > 0)
            {
                populateDgvChildFromList();
            }

            if (_parentCard != null)
            {
                insertCard(Convert.ToString(_parentCard));
            }

            log.LogMethodExit();
        }

        private void displayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            log.LogMethodExit();
        }

        void CardSwiped(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            insertCard(CardNumber);
            log.LogMethodExit();
        }

        private void insertCard(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            displayMessageLine("");

            Card card = new Card(POSStatic.Utilities.ReaderDevice, CardNumber, "", POSStatic.Utilities);

            string message = "";
            if (!POSUtils.refreshCardFromHQ(ref card, ref message))
            {
                displayMessageLine(message);

                log.Info("Ends-insertCard(" + CardNumber + ") as unable to refresh card from HQ");//Added for logger function on 08-Mar-2016
                log.LogMethodExit();
                return;
            }

            if (card.technician_card.Equals('Y'))
            {
                displayMessageLine("Technician Card not allowed");
                log.Info("Ends-insertCard(" + CardNumber + ") as Technician Card not allowed");//Added for logger function on 08-Mar-2016
                log.LogMethodExit();
                return;
            }

            object parent = POSStatic.Utilities.executeScalar("select top 1 c.card_number + case when valid_flag = 'N' then ' (Inactive)' else '' end from ParentChildCards pcc, cards c where pcc.ChildCardId = @cardId and c.card_id = pcc.ParentCardId and pcc.activeFlag=1", new SqlParameter("@cardId", card.card_id));
            if (parent != null)
            {
                //displayMessageLine("Tapped card is already a child card of " + parent.ToString());
                displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(1513, parent.ToString()));
                log.Info("Ends-insertCard(" + CardNumber + ") as Tapped card is already a child card of " + parent.ToString());//Added for logger function on 08-Mar-2016
                log.LogMethodExit();
                return;
            }

            if (dgvParentCard.Rows.Count == 0) // parent card
            {
                if (card.CardStatus == "NEW")
                {
                    //displayMessageLine("Tap an issued card for Parent card");
                    displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(1514));
                    log.Info("Ends-insertCard(" + CardNumber + ") as Tapped an unissued card for Parent card ");//Added for logger function on 08-Mar-2016
                    log.LogMethodExit();
                    return;
                }                

                dgvParentCard.Rows.Add(card.CardNumber,
                                       card.customerDTO == null ? "" : card.customerDTO.FirstName + " " + card.customerDTO.LastName,
                                       card.credits,
                                       card.CreditPlusCardBalance + card.CreditPlusCredits + card.creditPlusItemPurchase,
                                       card.card_id);
                populateDgvChildFromList(card.card_id);
                //DataTable dt = POSStatic.Utilities.executeDataTable(@"select pc.id, c.card_id, c.card_number, credits,
                //                                                        cu.customer_name + ' ' + isnull(cu.last_name, '') customer,
                //                                                        ActiveFlag, 
                //                                                        convert(int, DailyLimitPercentage) DailyLimitPercentage, 
                //                                                        (c.CreditPlusCardBalance + c.CreditPlusCredits + c.creditPlusItemPurchase) CreditPlusCardBalance
                //                                                      from ParentChildCards pc, cardView c 
                //                                                            left outer join CustomerView(@PassPhrase) cu
                //                                                            on cu.customer_id = c.customer_id
                //                                                      where pc.ParentCardId = @parentCardId
                //                                                      and pc.ChildCardId = c.card_id
                //                                                      and c.valid_flag = 'Y'",
                //                                                    new SqlParameter("@parentCardId", card.card_id), new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetDecryptedParafaitDefault(POSStatic.Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")));
                //int? dailyLimitPercentage = 0;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    dgvChildCards.Rows.Add(dr["id"], dr["card_id"], dr["card_number"], dr["customer"], dr["credits"], dr["CreditPlusCardBalance"], Convert.ToBoolean(dr["ActiveFlag"]), (dr["DailyLimitPercentage"]));
                //    if (dr["DailyLimitPercentage"] != null && dr["DailyLimitPercentage"] != DBNull.Value)
                //    {
                //        dailyLimitPercentage += Convert.ToInt32(dr["DailyLimitPercentage"]);
                //    }
                //}
                //if (dailyLimitPercentage != null && dailyLimitPercentage < 100)
                //{
                //    UpdateParentChildCardInfo(card.card_id);                    
                //}
                displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(1515));
            }
            else 
            {
                if (CardNumber.Equals(dgvParentCard["ParentCardNumber", 0].Value.ToString()))
                {
                    //displayMessageLine("Parent Card");
                    displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(1516));
                    log.Info("Ends-insertCard(" + CardNumber + ") as Tapped Card is an Parent card ");//Added for logger function on 08-Mar-2016
                    log.LogMethodExit();
                    return;
                }

                ParentChildCardsListBL chkParentCardsListBL = new ParentChildCardsListBL(POSStatic.Utilities.ExecutionContext);
                List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>(ParentChildCardsDTO.SearchByParameters.PARENT_CARD_ID, card.card_id.ToString()));
                List<ParentChildCardsDTO> chkParentCardsListDTOList = chkParentCardsListBL.GetParentChildCardsDTOList(searchParameters);
                if (chkParentCardsListDTOList != null && chkParentCardsListDTOList.Count > 0)
                {
                    displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(1516));
                    log.Info("Ends-insertCard(" + CardNumber + ") as Tapped Card is an Parent card ");
                    log.LogMethodExit();
                    return;
                }

                foreach (DataGridViewRow dr in dgvChildCards.Rows)
                {
                    if (CardNumber.Equals(dr.Cells["ChildCardNumber"].Value.ToString()))
                    {
                        //displayMessageLine("Child Card already entered");
                        displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(1517));
                        log.Info("Ends-insertCard(" + CardNumber + ") as Child Card already entered ");//Added for logger function on 08-Mar-2016
                        log.LogMethodExit();
                        return;
                    }
                }

                if (card.CardStatus.Equals("NEW") && POSStatic.Utilities.getParafaitDefaults("ALLOW_NEW_CARDS_FOR_CHILDCARDS").Equals("N"))
                {
                    //displayMessageLine("Please tap an issued Child Card");
                    displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(1518));
                    log.Info("Ends-insertCard(" + CardNumber + ") as an unissued Child card is Tapped");//Added for logger function on 08-Mar-2016
                    log.LogMethodExit();
                    return;
                }
                //load new card to child cards
                dgvChildCards.Rows.Add(-1,
                                       card.card_id,
                                       card.CardNumber,
                                       card.customerDTO == null ? "" : card.customerDTO.FirstName + " " + card.customerDTO.LastName,
                                       card.credits,
                                       card.CreditPlusCardBalance, 
                                       1);
                if (dgvChildCards.Rows.Count > 1)
                {
                    splitDailyLimitPercentage();
                }
            }
            log.LogMethodExit();
        }

        private void populateDgvChildFromList(object objParentCardId = null)
        {
            if (objParentCardId != null)
            {
                DataTable dt = POSStatic.Utilities.executeDataTable(@"select pc.id, c.card_id, c.card_number, credits,
                                                                        cu.customer_name + ' ' + isnull(cu.last_name, '') customer,
                                                                        ActiveFlag, 
                                                                        convert(int, DailyLimitPercentage) DailyLimitPercentage, 
                                                                        (c.CreditPlusCardBalance + c.CreditPlusCredits + c.creditPlusItemPurchase) CreditPlusCardBalance
                                                                      from ParentChildCards pc, cardView c 
                                                                            left outer join CustomerView(@PassPhrase) cu
                                                                            on cu.customer_id = c.customer_id
                                                                      where pc.ParentCardId = @parentCardId
                                                                      and pc.ChildCardId = c.card_id
                                                                      and c.valid_flag = 'Y'",
                                                                        new SqlParameter("@parentCardId", Convert.ToInt32(objParentCardId)), new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")));
                int? dailyLimitPercentage = null;
                foreach (DataRow dr in dt.Rows)
                {
                    dgvChildCards.Rows.Add(dr["id"], dr["card_id"], dr["card_number"], dr["customer"], dr["credits"], dr["CreditPlusCardBalance"], Convert.ToBoolean(dr["ActiveFlag"]), (dr["DailyLimitPercentage"]));
                    if (dr["DailyLimitPercentage"] != null && dr["DailyLimitPercentage"] != DBNull.Value)
                    {
                        dailyLimitPercentage = (dailyLimitPercentage == null ? 0 : dailyLimitPercentage) + Convert.ToInt32(dr["DailyLimitPercentage"]);
                    }
                }
                //if (dailyLimitPercentage != null && dailyLimitPercentage < 100)
                //{
                //    UpdateParentChildCardInfo(Convert.ToInt32(objParentCardId));
                //}
            }
            else if (this.parentChildCardsDTOList != null && this.parentChildCardsDTOList.Count > 0)
            {
                foreach (ParentChildCardsDTO dgvLoadParentChildCardDTO in this.parentChildCardsDTOList)
                {
                    AccountDTO childAccountDTO = new AccountBL(POSStatic.Utilities.ExecutionContext, dgvLoadParentChildCardDTO.ChildCardId, true, true).AccountDTO;
                    //load new card to child cards
                    dgvChildCards.Rows.Add(-1,
                                           dgvLoadParentChildCardDTO.ChildCardId,
                                           childAccountDTO.TagNumber,
                                           String.IsNullOrEmpty(childAccountDTO.CustomerName) ? "" : childAccountDTO.CustomerName,
                                           childAccountDTO.Credits,
                                           childAccountDTO.AccountSummaryDTO.CreditPlusCardBalance,
                                           1);
                }
            }
        }

        /// <summary>
        /// Method to update daily percentage and update ParentChildCard entity
        /// </summary>
        private void UpdateParentChildCardInfo(int parentCardId)
        {
            ParentChildCardsListBL parentChildCardsListBL = new ParentChildCardsListBL(POSStatic.Utilities.ExecutionContext);
            List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>(ParentChildCardsDTO.SearchByParameters.PARENT_CARD_ID, parentCardId.ToString()));
            parentChildCardsDTOList = parentChildCardsListBL.GetParentChildCardsDTOList(searchParameters);

            btnSplitEqual.Tag = true;
            if (this.parentChildCardsDTOList != null && this.parentChildCardsDTOList.Count > 1)
            {
                splitDailyLimitPercentage();
                foreach (DataGridViewRow dr in dgvChildCards.Rows)
                {
                    ParentChildCardsDTO parentChildCardDTO = parentChildCardsDTOList.Find(x => x.Id == Convert.ToInt32(dr.Cells["ParentChildCardId"].Value));
                    if (parentChildCardDTO != null)
                    {
                        parentChildCardDTO.DailyLimitPercentage = (dr.Cells["DayLimit"].Value) == DBNull.Value ? (int?)null : Convert.ToInt32(dr.Cells["DayLimit"].Value);
                    }
                }
                foreach (ParentChildCardsDTO parentChildCardDTO in parentChildCardsDTOList)
                {
                    ParentChildCardsBL parentChildCardsBL = new ParentChildCardsBL(POSStatic.Utilities.ExecutionContext, parentChildCardDTO);
                    parentChildCardsBL.Save();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnSave.Enabled = false;
            if (dgvParentCard.Rows.Count == 0 || dgvChildCards.Rows.Count == 0)
            {
                //displayMessageLine("Nothing to save");
                displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(371));
                log.Info("Ends-btnSave_Click() as Nothing to save");//Added for logger function on 08-Mar-2016
                log.LogMethodExit();
                return;
            }

            try
            {
                TaskProcs tp = new TaskProcs(POSStatic.Utilities);
                List<ParentChildCardsDTO> saveParentChildCardsDTOList = new List<ParentChildCardsDTO>();
                foreach (DataGridViewRow dr in dgvChildCards.Rows)
                {
                    Card card = new Card(dr.Cells["ChildCardNumber"].Value.ToString(), "", POSStatic.Utilities);
                    //Added to create new child card if configuration set up allows
                    if (card.CardStatus.Equals("NEW") && POSStatic.Utilities.getParafaitDefaults("ALLOW_NEW_CARDS_FOR_CHILDCARDS").Equals("Y"))
                    {
                        card.createCard(null);
                    }
                    int? dailyLimitPercentage = (dr.Cells["DayLimit"].Value) == null ? (int?)null : Convert.ToInt32(dr.Cells["DayLimit"].Value);
                    ParentChildCardsDTO parentChildCardDTO = new ParentChildCardsDTO(Convert.ToInt32(dr.Cells["ParentChildCardId"].Value), Convert.ToInt32(dgvParentCard["ParentCardId", 0].Value), card.card_id, Convert.ToBoolean(dr.Cells["ActiveFlag"].Value), -1, dailyLimitPercentage);
                    saveParentChildCardsDTOList.Add(parentChildCardDTO);
                    //if (Convert.ToBoolean(dr.Cells["ActiveFlag"].Value))
                    //    // tp.LinkChildCard(Convert.ToInt32(dgvParentCard["ParentCardId", 0].Value), Convert.ToInt32(dr.Cells["ChildCardId"].Value));
                    //    tp.LinkChildCard(Convert.ToInt32(dgvParentCard["ParentCardId", 0].Value), card.card_id);//Link card id
                    //else
                    //    // tp.DeLinkChildCard(Convert.ToInt32(dgvParentCard["ParentCardId", 0].Value), Convert.ToInt32(dr.Cells["ChildCardId"].Value));
                    //    tp.DeLinkChildCard(Convert.ToInt32(dgvParentCard["ParentCardId", 0].Value), card.card_id);//Delink card id
                }
                if (saveParentChildCardsDTOList.Count > 0)
                {
                    foreach(ParentChildCardsDTO parentChildCardDTO in saveParentChildCardsDTOList)
                    {
                        if (parentChildCardDTO.ParentCardId == -1)
                        {
                            parentChildCardDTO.ParentCardId = Convert.ToInt32(dgvParentCard["ParentCardId", 0].Value);
                        }
                        ParentChildCardsBL parentChildCardsBL = new ParentChildCardsBL(POSStatic.Utilities.ExecutionContext, parentChildCardDTO);
                        parentChildCardsBL.Save();
                    }
                }
                //displayMessageLine("Save successsful");
                displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(122));
                //reload child grid
                if (dgvParentCard["ParentCardId", 0].Value != null)
                {
                    dgvChildCards.Rows.Clear();
                    populateDgvChildFromList(dgvParentCard["ParentCardId", 0].Value);
                }
                log.Info("btnSave_Click() - Save successsful");//Added for logger function on 08-Mar-2016
            }
            catch(Exception ex)
            {
                displayMessageLine(ex.Message);
                log.Fatal("Ends-btnSave_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            finally
            {
                btnSave.Enabled = true;
            }
            log.LogMethodExit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvParentCard.Rows.Clear();
            dgvChildCards.Rows.Clear();
            displayMessageLine("");
            log.LogMethodExit();
        }
        
        void splitDailyLimitPercentage()
        {
            log.LogMethodEntry();
            if (dgvChildCards.Rows.Count == 0)
                return;
            int childCardsCount = 0;
            foreach (DataGridViewRow dr in dgvChildCards.Rows)
            {
                if (Convert.ToBoolean(dr.Cells["ActiveFlag"].Value))
                {
                    childCardsCount++;
                }

            }
            int totalToSplit = Math.Max(100, TotalDailyLimitPercentage);
            int each = 0;
            int first = 0;
            int last = 0;
            if (Convert.ToBoolean(btnSplitEqual.Tag))
            {
                each = totalToSplit / childCardsCount;
                first = each + totalToSplit - (each * childCardsCount);
            }
            else if (Convert.ToBoolean(btnFirstCardHalf.Tag))
            {
                if (childCardsCount == 1)
                    first = totalToSplit;
                else
                {
                    first = totalToSplit / 2;
                    each = (totalToSplit - first) / (childCardsCount - 1);
                    last = each + first - each * (childCardsCount - 1);
                   // first += totalToSplit - first - each * (childCardsCount - 1);
                }
            }
            else if (Convert.ToBoolean(btnCustomFirstCard.Tag))
            {
                if (childCardsCount == 1)
                    first = Math.Min(Convert.ToInt32(lblCustomFirstCard.Text), totalToSplit);
                else
                {
                    first = Math.Min(Convert.ToInt32(lblCustomFirstCard.Text), totalToSplit);
                    int balance = Math.Max(0, totalToSplit - first);
                    each = balance / (childCardsCount - 1);
                    last = each + balance - each * (childCardsCount - 1);
                }
            }
            else
            {
                each = 0;
                last = 0;
                first = 0;
            }

            dgvChildCards["DayLimit", 0].Value = first;
            for (int i = 1; i < dgvChildCards.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvChildCards["ActiveFlag", i].Value))
                {
                    dgvChildCards["DayLimit", i].Value = each;
                }
            }
            if (last > 0)
                dgvChildCards["DayLimit", dgvChildCards.Rows.Count - 1].Value = last;

            foreach (DataGridViewRow dr in dgvChildCards.Rows)
            {
                int add = 0;
                try
                {
                    if (Convert.ToBoolean(dr.Cells["ActiveFlag"].Value))
                    {
                        add = Convert.ToInt32(dr.Cells["DayLimit"].Value);
                    }
                }
                catch { }
            }
            log.LogMethodExit(null);
        }

        private void btnSplitEqual_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            panelCustomFirstCard.Visible = false;

            btnSplitEqual.BackgroundImage = Properties.Resources.pressed1;
            btnFirstCardHalf.BackgroundImage = Properties.Resources.normal1;
            btnCustomFirstCard.BackgroundImage = Properties.Resources.normal1;

            btnSplitEqual.Tag = true;
            btnCustomFirstCard.Tag =
            btnFirstCardHalf.Tag = false;

            splitDailyLimitPercentage();
            log.LogMethodExit(null);
        }

        private void btnFirstCardHalf_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            panelCustomFirstCard.Visible = false;

            btnSplitEqual.BackgroundImage = Properties.Resources.normal1;
            btnCustomFirstCard.BackgroundImage = Properties.Resources.normal1;
            btnFirstCardHalf.BackgroundImage = Properties.Resources.pressed1;

            btnCustomFirstCard.Tag =
            btnSplitEqual.Tag = false;
            btnFirstCardHalf.Tag = true;

            splitDailyLimitPercentage();
            log.LogMethodExit();
        }

        private void btnCustomFirstCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            panelCustomFirstCard.Visible = true;
            panelCustomFirstCard.Tag = TotalDailyLimitPercentage;
            lblCustomFirstCard.Text = (TotalDailyLimitPercentage / 2).ToString();

            btnSplitEqual.BackgroundImage = Properties.Resources.normal1;
            btnFirstCardHalf.BackgroundImage = Properties.Resources.normal1;
            btnCustomFirstCard.BackgroundImage = Properties.Resources.pressed1;

            btnSplitEqual.Tag =
            btnFirstCardHalf.Tag = false;
            btnCustomFirstCard.Tag = true;

            splitDailyLimitPercentage();
            log.LogMethodExit();
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (lblCustomFirstCard.Text.Equals("0"))
            {
                log.Debug("Ends-btnMinus_Click() Event");
                return;
            }

            lblCustomFirstCard.Text = (Convert.ToInt32(lblCustomFirstCard.Text) - 5).ToString();
            splitDailyLimitPercentage();
            log.LogMethodExit(null);
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (lblCustomFirstCard.Text.Equals("100"))
            {
                log.Debug("Ends-btnPlus_Click() Event");
                return;
            }

            lblCustomFirstCard.Text = (Convert.ToInt32(lblCustomFirstCard.Text) + 5).ToString();
            splitDailyLimitPercentage();
            log.LogMethodExit(null);
        }

        private void dgvChildCards_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == dgvChildCards.Rows[e.RowIndex].Cells["ActiveFlag"].ColumnIndex)
            {
                if (!Convert.ToBoolean(dgvChildCards.Rows[e.RowIndex].Cells["ActiveFlag"].Value))
                {
                    dgvChildCards.Rows[e.RowIndex].Cells["DayLimit"].Value = null;
                }
                else
                {
                    btnSplitEqual.Tag = true;
                }
                splitDailyLimitPercentage();
            }
        }

        private void dgvChildCards_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex != -1 && e.ColumnIndex == dgvChildCards.Rows[e.RowIndex].Cells["ActiveFlag"].ColumnIndex)
            {
                dgvChildCards.EndEdit();
            }
            log.LogMethodExit();
        }

        private void btnClear_Perc_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnClear_Perc.Tag = true;
            btnSplitEqual.Tag = false;
            btnFirstCardHalf.Tag = false;
            btnCustomFirstCard.Tag = false;
            splitDailyLimitPercentage();
            log.LogMethodExit();
        }
    }
}
