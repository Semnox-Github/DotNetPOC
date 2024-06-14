/********************************************************************************************
 * Project Name - frmDeactivateCards
 * Description  - frmDeactivateCards ui
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified to get enrypted key and password values
 *2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 *2.70.3      20-Aug-2019    Girish Kundar       Modified :  Added logger methods and Removed unused namespace's
 *2.70.3      30-Jan-2020    Archana             Modified : Added lookup value to show/hide points/Time balance in the deactivation screen
 *2.80.0      25-Mar-2020    Mathew Ninan     Added Credit Plus balance to display grid. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Logger = Semnox.Core.Utilities.Logger;

namespace Parafait_POS.Cards
{
    public partial class frmDeactivateCards : Form
    {
        int SerialNo = 0;
        private readonly TagNumberParser tagNumberParser;

        private readonly UlcKeyStore ulcKeyStore;
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //End: Modified Added for logger function on 08-Mar-2016
        List<Card> MultipleCards;
        

        public frmDeactivateCards()
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry();
            InitializeComponent();

            POSStatic.Utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            ulcKeyStore = new UlcKeyStore();
            log.LogMethodExit();
        }

        private void frmInputPhysicalCards_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            (Application.OpenForms["POS"] as Parafait_POS.POS).lastTrxActivityTime = DateTime.Now;

            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
            ShowOrHideCardDetailsGridViewColumns();
            dgvMultipleCards.BackgroundColor = this.BackColor;
            dgvMultipleCards.AllowUserToAddRows = false;
            dgvMultipleCards.AllowUserToDeleteRows = false;
            dgvMultipleCards.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dcBonus.DefaultCellStyle = dcCourtesy.DefaultCellStyle = dcCredits.DefaultCellStyle = POSStatic.Utilities.gridViewAmountCellStyle();
            dcTickets.DefaultCellStyle = POSStatic.Utilities.gridViewNumericCellStyle();

            displayMessageLine(POSStatic.MessageUtils.getMessage(257));
            log.Info("frmInputPhysicalCards_Load() - Please Tap a Card");//Added for logger function on 08-Mar-2016
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
                    CardSwiped(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message);
                    log.Fatal("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                }
            }
            log.LogMethodExit();
        }

        private void CardSwiped(string CardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry( CardNumber);
            displayMessageLine("");

            byte[] customerAuthKey = new byte[6];
            int customerKey = POSStatic.Utilities.MifareCustomerKey;

            const int BLOCK_NUMBER = 4; //block number for changing auth key
            byte[] basicAuthKey = new byte[6]; //default key
            string authkey = Encryption.Decrypt(Encryption.GetParafaitKeys("MifareAuthorization"));//Encryption.Decrypt("0aNVShI2+C3Nw3yOnFGjbk+wLOV7Ia7z");
            string[] sa = authkey.Substring(0, 17).Split('-');
            int ikey = 0;
            foreach (string s in sa)
            {
                basicAuthKey[ikey++] = Convert.ToByte(s, 16);
            }

            string key = Encryption.GetParafaitKeys("NonMifareAuthorization");
            for (int i = 0; i < 5; i++)
                customerAuthKey[i] = Convert.ToByte(key[i]);
            customerAuthKey[5] = Convert.ToByte(customerKey);

            byte[] siteIdBuffer = new byte[16];
            siteIdBuffer[0] = siteIdBuffer[1] = siteIdBuffer[2] = siteIdBuffer[3] = 0xff;

            string message = "";

            try //Change authentication key to default key
            {
                if (readerDevice.CardType == CardType.MIFARE)
                {
                    readerDevice.change_authentication_key(BLOCK_NUMBER + 3, customerAuthKey, basicAuthKey, ref message);
                }
                else if(readerDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    readerDevice.change_authentication_key(BLOCK_NUMBER + 3,  ulcKeyStore.LatestCustomerUlcKey.Value, ulcKeyStore.DefaultUltralightCKeys[0].Value, ref message);
                }
                //readerDevice.write_data(6, 1, basicAuthKey, siteIdBuffer, ref message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message);
            }

            Card swipedCard = new Card(readerDevice, CardNumber, POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);

            if (!POSUtils.refreshCardFromHQ(ref swipedCard, ref message))
            {
                displayMessageLine(message);
                log.Info("Ends-CardSwiped(" + CardNumber + ") as unable to refresh card from HQ");//Added for logger function on 08-Mar-2016
                return;
            }

            if (swipedCard.CardStatus == "NEW")
            {
                displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(262));
                log.Info("Ends-CardSwiped(" + CardNumber + ") as Cannot Refund New Cards");//Added for logger function on 08-Mar-2016
                return;
            }
            else if (swipedCard.technician_card.Equals('Y'))
            {
                displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(197, CardNumber));
                log.Info("Ends-CardSwiped(" + CardNumber + ") as Technician Card (" + CardNumber + ") not allowed for Transaction");//Added for logger function on 08-Mar-2016
                return;
            }

            bool cardFound = false;
            foreach (DataGridViewRow dr in dgvMultipleCards.Rows)
            {
                if (dr.Cells["Card_Number"].Value != null && swipedCard.CardNumber == dr.Cells["Card_Number"].Value.ToString())
                {
                    cardFound = true;
                    break;
                }
            }

            if (cardFound)
            {
                displayMessageLine(POSStatic.MessageUtils.getMessage(59));
                log.Info("Ends-CardSwiped(" + CardNumber + ") as Card is already added");//Added for logger function on 08-Mar-2016
                return;
            }

            string customer = "";
            if (swipedCard.customerDTO != null)
                customer = swipedCard.customerDTO.FirstName + " " + swipedCard.customerDTO.LastName;
            // add it to dgv for display
            dgvMultipleCards.Rows.Add(new object[] { ++SerialNo, swipedCard.CardNumber, customer, swipedCard.credits + swipedCard.CreditPlusCardBalance + swipedCard.CreditPlusCredits + swipedCard.creditPlusItemPurchase, swipedCard.bonus + swipedCard.CreditPlusBonus, swipedCard.courtesy, swipedCard.ticket_count + swipedCard.CreditPlusTickets });
            dgvMultipleCards.Refresh();

            log.LogMethodExit();
        }

        private void displayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            textBoxMessageLine.Text = message;
            log.LogMethodExit();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (dgvMultipleCards.Rows.Count == 0)
            {
                displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(90));
                log.Info("Ends-buttonOK_Click() as no cards are tapped");//Added for logger function on 08-Mar-2016
                return;
            }

            TaskProcs tp = new TaskProcs(POSStatic.Utilities);

            foreach (DataGridViewRow dr in dgvMultipleCards.Rows)
            {
                string cardNumber = dr.Cells["Card_Number"].Value.ToString();

                Card card = new Card(cardNumber, POSStatic.ParafaitEnv.LoginID, POSStatic.Utilities);
                if (card.CardStatus.Equals("ISSUED"))
                {
                    string message = "";
                    if (!tp.RefundCard(card, 0, 0, 0, "Deactivate", ref message, true))
                    {
                        displayMessageLine(message);
                        log.Info("Ends-buttonOK_Click() unable to Deactivate error " + message);//Added for logger function on 08-Mar-2016
                        return;
                    }
                    else
                        card.invalidateCard(null);
                }
            }

            displayMessageLine(dgvMultipleCards.Rows.Count.ToString() + " cards deactivated");
            log.Info("buttonOK_Click() -Total cards deactivated " + dgvMultipleCards.Rows.Count.ToString());//Added for logger function on 08-Mar-2016
            dgvMultipleCards.Rows.Clear();
            log.LogMethodExit();
        }

        private void frmInputPhysicalCards_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            Common.Devices.UnregisterCardReaders();
            log.LogMethodExit();
        }

        private void dgvMultipleCards_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                log.Info("Ends-dgvMultipleCards_CellClick() as e.RowIndex < 0 || e.ColumnIndex < 0");//Added for logger function on 08-Mar-2016
                return;
            }

            if (dgvMultipleCards.Columns[e.ColumnIndex].Equals(dcRemove))
            {
                dgvMultipleCards.Rows.RemoveAt(e.RowIndex);
            }
            log.LogMethodExit();
        }

        private void buttonRefund_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvMultipleCards.Rows.Count == 0)
                {
                    displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(90));
                    log.Info("Ends-buttonRefund_Click() as no cards are tapped");
                    return;
                }

                MultipleCards = new List<Card>();
                foreach (DataGridViewRow dr in dgvMultipleCards.Rows)
                {
                    string cardNumber = dr.Cells["Card_Number"].Value.ToString();

                    Card newCard = new Card(cardNumber, POSStatic.ParafaitEnv.LoginID, POSStatic.Utilities);
                    MultipleCards.Add(newCard);
                }

                object Parameter = null;
                FormCardTasks refundFrm = new FormCardTasks(TaskProcs.REFUNDCARD, MultipleCards, POSStatic.Utilities, Parameter);
                //refundFrm.ShowDialog();
                if (refundFrm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                string message = "";

                if (refundFrm.ListTrxId.Count > 0)
                {
                    PrintMultipleTransactions printMultipleTransactions = new PrintMultipleTransactions(POSStatic.Utilities);
                    if (POSStatic.Utilities.ParafaitEnv.TRX_AUTO_PRINT_AFTER_SAVE == "Y")
                    {

                        if (!printMultipleTransactions.Print(refundFrm.ListTrxId, false, ref message))
                        {
                            // displayMessageLine(message, WARNING);
                            log.Warn("PrintSpecificTransaction(" + refundFrm.ListTrxId + ",rePrint) - Unable to Print Transaction error: " + message);//Added for logger function on 08-Mar-2016
                        }
                    }
                    else if (POSStatic.Utilities.ParafaitEnv.TRX_AUTO_PRINT_AFTER_SAVE == "A")
                    {
                        if (POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(205), "Refund Print", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (!printMultipleTransactions.Print(refundFrm.ListTrxId, false, ref message))
                            {
                                //displayMessageLine(message, WARNING);
                                log.Warn("PrintSpecificTransaction(" + refundFrm.ListTrxId + ",rePrint) - Unable to Print Transaction error: " + message);//Added for logger function on 08-Mar-2016
                            }
                        }
                    }

                }

                displayMessageLine(dgvMultipleCards.Rows.Count.ToString() + " cards Refunded");
                log.Info("buttonRefund_Click() -Total cards Refunded " + dgvMultipleCards.Rows.Count.ToString());//Added for logger function on 02-sep-2017
                dgvMultipleCards.Rows.Clear();
                log.LogMethodExit();
            }
            catch
            {
                displayMessageLine(POSStatic.Utilities.MessageUtils.getMessage(90));
                log.Info("Ends-buttonRefund_Click() Tap the cards to refund");
                return;

            }
        }

        private void ShowOrHideCardDetailsGridViewColumns()
        {
            log.LogMethodEntry();
            LookupsList lookupsListObject = new LookupsList(POSStatic.Utilities.ExecutionContext);
            List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>
            {
                new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_NAME, "DISPLAY_CARD_ENTITLEMENTS_IN_DEACTIVATION_UI"),
                new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, POSStatic.Utilities.ExecutionContext.GetSiteId().ToString())
            };
            List<LookupsDTO> lookupsDTOList = lookupsListObject.GetAllLookups(searchParameters, true);
            if(lookupsDTOList != null && lookupsDTOList.Count > 0)
            {
                if(lookupsDTOList[0].LookupValuesDTOList != null && lookupsDTOList[0].LookupValuesDTOList.Count >0
                    && lookupsDTOList[0].LookupValuesDTOList[0].LookupValue == "DisplayCardEntitlement" &&
                    lookupsDTOList[0].LookupValuesDTOList[0].Description != "Y")
                {
                    dcCredits.Visible = false;
                    dcTickets.Visible = false;
                    dcBonus.Visible = false;
                    dcCourtesy.Visible = false;
                }
            }
            log.LogMethodExit();
        }
    }
}
