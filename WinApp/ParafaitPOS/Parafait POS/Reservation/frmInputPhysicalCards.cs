/********************************************************************************************
* Project Name - Parafait POS
* Description  - frmInputCards 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana    Modified to add support for ULC cards 
 *2.80        11-Oct-2019      Guru S A           Waiver phase 2 enhancement
 *2.80        26-Nov-2019      Lakshminarayana    Virtual store enhancement
 *2.130.0     31-Aug-2021      Guru S A           Enable Serial number based card load
 *2.140.0     12-Dec-2021      Guru S A           Booking execute process performance fixes
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Logger = Semnox.Core.Utilities.Logger;
using Semnox.Parafait.Product;
using Semnox.Parafait.Tags;
using System.Data.SqlClient;
using Semnox.Parafait.Customer;
using System.Text.RegularExpressions;
using Semnox.Parafait.Customer.Accounts;

namespace Parafait_POS.Reservation
{
    public partial class frmInputPhysicalCards : Form
    {
        int swipedCount = 0;
        private Dictionary<string, string> cardList = new Dictionary<string, string>();
        public Dictionary<string, string> CompleteCardList { get { return cardList; } set { cardList = value; } }
        public Dictionary<string, string> MappedCardList { get { return GetMappedCardList(); } }

        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        bool splitEntitlement = false;
        private readonly TagNumberParser tagNumberParser;
        private bool reservationTransaction = false;
        private bool waiverSignPending = false;
        private string waiverPendingMsg = string.Empty;
        private const int MAXLOADMULTIPLECARDS = 5000;
        private Dictionary<int, ProductsDTO> productDictionary = new Dictionary<int, ProductsDTO>();
        public string WaiverPendingMsg { get { return waiverPendingMsg; } }
        private List<ProductsDTO> productListForDropDown = new List<ProductsDTO>();
        private bool userAction = false;
        private int addRowCount = 0;
        private int selectedProductId = -1;
        private string selectedProductName = string.Empty;
        private VirtualKeyboardController virtualKeyboard;
        private List<AccountDTO> tempAccountList = null;
        class cardproduct
        {
            public int serialNo;
            public string productName;
            public int quantity;
            public List<string> cards = new List<string>();
            public int productId = -1;
            public List<string> cardKeyList = new List<string>();
        }
        List<cardproduct> lstCardProducts = new List<cardproduct>();

        public frmInputPhysicalCards(int Quantity, int productId, string ProductName, bool splitEntitlement = false)
        {
            log.LogMethodEntry(Quantity, productId, ProductName, splitEntitlement);
            userAction = false;
            InitializeComponent();
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            cardproduct cp = new cardproduct();
            cp.quantity = Quantity;
            cp.serialNo = 1;
            cp.productId = productId;
            cp.productName = ProductName;
            lstCardProducts.Add(cp);
            this.splitEntitlement = splitEntitlement;

            while (Quantity-- > 0)
            {
                cardList.Add(Quantity.ToString(), "");
                cp.cardKeyList.Add(Quantity.ToString());
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        public frmInputPhysicalCards(int Quantity, List<Transaction.ComboCardProduct> cardProductList)
        {
            log.LogMethodEntry(Quantity, cardProductList);
            userAction = false;
            InitializeComponent();
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            int index = 0;
            int serialNo = 1;
            foreach (Transaction.ComboCardProduct cpDetail in cardProductList)
            {
                int qty = cpDetail.Quantity * Quantity;

                cardproduct cp = new cardproduct();
                cp.quantity = qty;
                cp.serialNo = serialNo++;
                cp.productName = cpDetail.ChildProductName;
                cp.productId = cpDetail.ChildProductId;
                lstCardProducts.Add(cp);

                while (qty-- > 0)
                {
                    cp = CreateCardNoForAutoGenCardProduct(cpDetail.ChildProductId, cp);
                    cardList.Add(index.ToString(), "");
                    cp.cardKeyList.Add(index.ToString());
                    index++;
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(Quantity + "," + cardProductList);
        }

        public frmInputPhysicalCards(Transaction Trx)
        {
            log.LogMethodEntry(Trx);
            userAction = false;
            InitializeComponent();
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            int index = 1;
            object o = null;
            reservationTransaction = Trx.IsReservationTransaction(null);
            waiverSignPending = Trx.IsWaiverSignaturePending();
            waiverPendingMsg = string.Empty;
            List<Transaction.TransactionLine> waiverTrxLineWithCardList = new List<Transaction.TransactionLine>();
            if (Trx.TrxLines != null)
            {
                waiverTrxLineWithCardList = Trx.TrxLines.Where(tl => tl.LineValid
                                                             && string.IsNullOrEmpty(tl.CardNumber) == false
                                                             && tl.WaiverSignedDTOList != null
                                                             && tl.WaiverSignedDTOList.Any()
                                                             && tl.WaiverSignedDTOList.Exists(ws => ws.CustomerSignedWaiverId == -1 && ws.IsActive == true)).ToList();

            }

            for (int lineIndex = 0; lineIndex < Trx.TrxLines.Count; lineIndex++)
            {
                POSUtils.SetLastActivityDateTime();
                //Added tl.ProductName != "Card Deposit" to ignore the lines created for card deposit on 06-Oct-2015//
                // changed to tl.ProductTypeCode != "CARDDEPOSIT" 14-Mar-2016
                Transaction.TransactionLine tl = Trx.TrxLines[lineIndex];
                if (tl.CardNumber != null && tl.LineValid && tl.CardNumber.StartsWith("T") && tl.ProductTypeCode != "CARDDEPOSIT")
                {
                    if (tl.LineValid && reservationTransaction && (Trx.IsWaiverSignaturePending(lineIndex)
                                                                    || (waiverTrxLineWithCardList != null
                                                                         && waiverTrxLineWithCardList.Exists(tlin => tlin.CardNumber == tl.CardNumber))))
                    {
                        if (string.IsNullOrEmpty(waiverPendingMsg) || waiverPendingMsg.Contains(tl.CardNumber) == false)
                        {
                            waiverPendingMsg = waiverPendingMsg + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2353, tl.CardNumber) + Environment.NewLine;
                            //Waiver signing is pending for transaction line with card number &1. 
                        }

                    }
                    else
                    {
                        if (!cardList.ContainsKey(tl.CardNumber))
                        {
                            cardList.Add(tl.CardNumber, "");
                            cardproduct cp = new cardproduct();
                            cp.cardKeyList.Add(tl.CardNumber);
                            cp.quantity = (int)tl.quantity;
                            cp.serialNo = index++;
                            cp.productName = (tl.LineAtb != null ? tl.ProductName + "-" + tl.AttractionDetails : tl.ProductName);
                            cp.productId = tl.ProductID;
                            if (!string.IsNullOrEmpty(tl.Remarks))
                            {
                                cp.productName += " - " + tl.Remarks;
                            }
                            if (tl.CardNumber.StartsWith("T"))
                            {
                                cp = CreateCardNoForAutoGenCardProduct(tl.ProductID, cp);
                            }

                            lstCardProducts.Add(cp);
                        }
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        public frmInputPhysicalCards(List<TransactionLineDTO> selectedTransactionLineDTOList)
        {
            log.LogMethodEntry(selectedTransactionLineDTOList);
            userAction = false;
            InitializeComponent();
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            int index = 1;
            object o = null;
            foreach (TransactionLineDTO tl in selectedTransactionLineDTOList)
            {
                POSUtils.SetLastActivityDateTime();
                //Added tl.ProductName != "Card Deposit" to ignore the lines created for card deposit on 06-Oct-2015//
                // changed to tl.ProductTypeCode != "CARDDEPOSIT" 14-Mar-2016

                if (tl.CardNumber != null && !cardList.ContainsKey(tl.CardNumber) && tl.CardNumber.StartsWith("T") && tl.ProductTypeCode != "CARDDEPOSIT")
                {
                    cardList.Add(tl.CardNumber, "");
                    cardproduct cp = new cardproduct
                    {
                        quantity = (int)(tl.Quantity.HasValue ? tl.Quantity.Value : 0),
                        serialNo = index++,
                        productName = tl.ProductName,
                        productId = tl.ProductId
                    };
                    if (!string.IsNullOrEmpty(tl.Remarks))
                    {
                        cp.productName += " - " + tl.Remarks;
                    }
                    cp.cardKeyList.Add(tl.CardNumber);
                    lstCardProducts.Add(cp);
                }
            }
            log.LogMethodExit();
        }
        private void DisplayInGrid()
        {
            log.LogMethodEntry();
            dgvMultipleCards.Rows.Clear();
            addRowCount = 0;
            int totalCardCount = 0;
            for (int i = 0; i < lstCardProducts.Count; i++)
            {
                POSUtils.SetLastActivityDateTime();
                cardproduct cp = lstCardProducts[i];
                if (selectedProductId == -1
                    || (selectedProductId > -1 && selectedProductId == cp.productId && selectedProductName == cp.productName))
                {
                    dgvMultipleCards.Rows.Add(cp.serialNo, cp.productName, cp.quantity, string.Join(Environment.NewLine, cp.cards.ToArray()));
                    dgvMultipleCards.Rows[addRowCount].Cells["SerialNumber"].Tag = cp;
                    dgvMultipleCards.Rows[addRowCount].Cells["Card_Number"].Tag = "UNSELECTED";
                    if (cp.cards.Count > 2)
                    {
                        dgvMultipleCards.Rows[addRowCount].Height = cp.cards.Count * 14;
                    }
                    addRowCount++;
                    totalCardCount = totalCardCount + cp.quantity;
                }

            }
            string productCountDisplayMsg = string.Empty;
            if (selectedProductId > -1)
            {
                int swippedAgainstProduct = GetSwippedCardCount(selectedProductId, selectedProductName);
                productCountDisplayMsg = " [ " + swippedAgainstProduct.ToString() + " / " + totalCardCount.ToString() + " ]";
            }
            lblCardCount.Text = swipedCount.ToString() + " / " + cardList.Count.ToString() + productCountDisplayMsg;
            if (cardList != null && cardList.Count != swipedCount)
            {
                displayMessageLine(POSStatic.MessageUtils.getMessage(4054));
                log.Info(POSStatic.MessageUtils.getMessage(4054));
                //Please Tap Card/Enter Serial Number range
            }
            if (cardList.Count == swipedCount)
            {
                displayMessageLine(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2122));
                //"All Cards entered. Press OK."
                log.Info(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2122));
            }
            log.LogMethodExit();
        }

        private void frmInputPhysicalCards_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                userAction = false;
                LoadCardProductList();
                Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));

                dgvMultipleCards.BackgroundColor = this.BackColor;
                dgvMultipleCards.AllowUserToAddRows = false;
                dgvMultipleCards.AllowUserToDeleteRows = false;
                dgvMultipleCards.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgvMultipleCards.DefaultCellStyle.SelectionBackColor = dgvMultipleCards.DefaultCellStyle.BackColor;
                dgvMultipleCards.DefaultCellStyle.SelectionForeColor = dgvMultipleCards.DefaultCellStyle.ForeColor;
                DisplayInGrid();
                if (splitEntitlement)
                {
                    lblEntitlement.Text = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "Points will be divided onto each card");
                }
                if (string.IsNullOrEmpty(waiverPendingMsg) == false)
                {
                    waiverPendingMsg = waiverPendingMsg + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2380);
                    //Skipping these transaction line cards to proceed with the rest. Please complete waiver signinng formalities if you want to include them
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, waiverPendingMsg));
                    log.Error(waiverPendingMsg);
                }
                POSStatic.Utilities.setLanguage(this);
                InitializeKeyboard();
                POSStatic.Utilities.setLanguage(this);
            }
            finally
            {
                userAction = true;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }


        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                POSUtils.SetLastActivityDateTime();
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
                        log.LogMethodExit(ex.Message);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, POSStatic.Utilities.ParafaitEnv.SiteId);
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message);
                        log.LogMethodExit(ex.Message);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    displayMessageLine(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }
                try
                {
                    CardSwiped(tagNumber.Value);
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message);
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void CardSwiped(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            POSUtils.SetLastActivityDateTime();
            displayMessageLine("");
            if (cardList.Count == swipedCount)
            {
                displayMessageLine(POSStatic.MessageUtils.getMessage(32, cardList.Count));
                log.Info("Ends-CardSwiped(" + CardNumber + ") as can load at most " + cardList.Count + " Cards");
                return;
            }
            if (selectedProductId > -1)
            {
                int swippedCardsForProduct = GetSwippedCardCount(selectedProductId, selectedProductName);
                int totalCardsForProduct = GetTotalCardCount(selectedProductId, selectedProductName);
                if (swippedCardsForProduct == totalCardsForProduct)
                {
                    displayMessageLine(POSStatic.MessageUtils.getMessage(32, totalCardsForProduct));
                    log.Info("Ends-CardSwiped(" + CardNumber + ") as can load at most " + totalCardsForProduct + " Cards for the product");
                    return;
                }
            }
            POSUtils.SetLastActivityDateTime();
            Card swipedCard = new Card(CardNumber, POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);
            if (swipedCard.technician_card.Equals('Y'))
            {
                displayMessageLine(POSStatic.MessageUtils.getMessage(197, CardNumber));
                log.Info(POSStatic.MessageUtils.getMessage(197, CardNumber));
                return;
            }
            else if (swipedCard.CardStatus != "NEW")
            {
                displayMessageLine("Issued Card");
                log.Info("CardSwiped(" + CardNumber + ") Swipped Card is a Issued Card");
            }

            bool cardFound = false;
            List<cardproduct> cardIsAlreadyMappedTo = new List<cardproduct>();
            foreach (cardproduct cp in lstCardProducts)
            {
                if (cp.cards.Contains(swipedCard.CardNumber))
                {
                    cardIsAlreadyMappedTo.Add(cp);
                    cardFound = true;
                    break;//GGG
                }
            }

            if (cardFound)
            {
                displayMessageLine(POSStatic.MessageUtils.getMessage(59));
                log.Info("CardSwiped(" + CardNumber + ") as Card already added");
                //return;
            }
            cardproduct selectProduct = null;
            foreach (cardproduct cp in lstCardProducts)
            {
                POSUtils.SetLastActivityDateTime();
                if (selectedProductId == -1
                    || (selectedProductId == cp.productId && selectedProductName == cp.productName))
                {
                    if (cp.cards.Count < cp.quantity
                        && (cardIsAlreadyMappedTo.Any() == false
                          || cardIsAlreadyMappedTo.Exists(ciam => ciam.productId == cp.productId
                                                            && ciam.serialNo == cp.serialNo) == false))
                    {
                        if (cp.productId > -1)
                        {
                            ProductsDTO productDTO = new Products(POSStatic.Utilities.ExecutionContext, cp.productId, false, false).GetProductsDTO;
                            if (productDTO != null
                                && productDTO.IssueNotificationDevice != null
                                && (bool)productDTO.IssueNotificationDevice)
                            {
                                List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> tagsSearchParameters = new List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>>();
                                tagsSearchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.SITE_ID, (POSStatic.Utilities.ParafaitEnv.IsCorporate ? POSStatic.Utilities.ParafaitEnv.SiteId : -1).ToString()));
                                tagsSearchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.TAGNOTIFICATIONSTATUS, TagNotificationStatusConverter.ToString(TagNotificationStatus.IN_USE)));
                                tagsSearchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.ISINSTORAGE, 0.ToString()));
                                tagsSearchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.TAGNUMBER, swipedCard.CardNumber));
                                NotificationTagsListBL notificationTagsListBL = new NotificationTagsListBL(POSStatic.Utilities.ExecutionContext);
                                List<NotificationTagsDTO> notificationTagsDTOList = notificationTagsListBL.GetAllNotificationTagsList(tagsSearchParameters);
                                if (notificationTagsDTOList.Count == 0)
                                {
                                    displayMessageLine(POSStatic.MessageUtils.getMessage(2959));
                                    log.Info(POSStatic.MessageUtils.getMessage(2959));
                                    return;
                                }
                            }
                        }
                        cp.cards.Add(swipedCard.CardNumber);
                        selectProduct = cp;
                        swipedCount++;
                        break;
                    }
                }
            }
            DisplayInGrid();
            int rowIndex = GetRowIdFromGrid(selectProduct);
            SetAsCurrentGridCell(rowIndex);
            log.LogMethodExit(CardNumber);
        }

        private KeyValuePair<int, string> GetSelectedProductInfo()
        {
            log.LogMethodEntry();
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>(-1, string.Empty);

            try
            {
                if (cmbCardProducts.SelectedItem != null && cmbCardProducts.SelectedIndex > -1)
                {
                    keyValuePair = new KeyValuePair<int, string>((cmbCardProducts.SelectedItem as ProductsDTO).ProductId,
                                                                (cmbCardProducts.SelectedItem as ProductsDTO).ProductName);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(keyValuePair);
            return keyValuePair;
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
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (cardList.Count != swipedCount && reservationTransaction == false)
                {
                    displayMessageLine("Enter " + cardList.Count.ToString() + " Cards");
                    log.LogMethodExit("CardList.Count != swipedCount");
                    return;
                }

                Dictionary<string, string> cards = new Dictionary<string, string>();
                POSUtils.SetLastActivityDateTime();
                foreach (cardproduct cp in lstCardProducts)
                {
                    if (cp.cards.Any())
                    {
                        for (int j = 0; j < cp.cards.Count; j++)
                        {
                            cards.Add(cp.cardKeyList[j], cp.cards[j]);
                        }
                    }
                }

                int cardCount = cards.Count;
                if (cardCount == 0)
                {
                    displayMessageLine(POSStatic.MessageUtils.getMessage(4054));
                    //    log.Info(POSStatic.MessageUtils.getMessage(4054));
                    //    //Please Tap Card/Enter Serial Number range
                    log.LogMethodExit();
                    return;
                }
                List<string> keys = new List<string>(cardList.Keys);
                int mappedCount = 0;
                POSUtils.SetLastActivityDateTime();
                for (int i = 0; i < keys.Count; i++)
                {
                    if (cards.ContainsKey(keys[i]) && string.IsNullOrWhiteSpace(cards[keys[i]]) == false)
                    {
                        cardList[keys[i]] = cards[keys[i]];
                        mappedCount++;
                    }
                    if (mappedCount >= cardCount)
                    {
                        break;
                    }
                }
                List<KeyValuePair<string, List<string>>> sameCardProdList = new List<KeyValuePair<string, List<string>>>();
                bool sameCardOnMulitpleLines = SameCardIsOnMulitpleLines(out sameCardProdList);
                if (sameCardOnMulitpleLines)
                {
                    if (ShowSameCardOnMultipeLineMessage(sameCardProdList) == DialogResult.No)
                    {
                        log.LogMethodExit();
                        return;
                    }
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void frmInputPhysicalCards_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            Common.Devices.UnregisterCardReaders();
            log.LogMethodExit();
        }

        private cardproduct CreateCardNoForAutoGenCardProduct(int productId, cardproduct cardProduct)
        {
            log.LogMethodEntry(productId, cardProduct);
            string autoGenCard = "";
            POSUtils.SetLastActivityDateTime();
            ProductsDTO productsDTO = GetProductDTO(productId);
            if (productsDTO != null && productsDTO.AutoGenerateCardNumber != null && productsDTO.AutoGenerateCardNumber.Equals("Y"))
            {
                RandomTagNumber randomTagNumber = new RandomTagNumber(POSStatic.Utilities.ExecutionContext);
                autoGenCard = randomTagNumber.Value;
                swipedCount++;
            }
            if (!String.IsNullOrEmpty(autoGenCard))
            {
                cardProduct.cards.Add(autoGenCard);
            }
            log.LogMethodExit(cardProduct);
            return cardProduct;
        }

        private cardproduct CreateCardNoForAutoGenCardProduct(cardproduct cardProduct)
        {
            log.LogMethodEntry(cardProduct);
            POSUtils.SetLastActivityDateTime();
            RandomTagNumber randomTagNumber = new RandomTagNumber(POSStatic.Utilities.ExecutionContext);
            string autoGenCard = randomTagNumber.Value;
            swipedCount++;
            cardProduct.cards.Add(autoGenCard);
            log.LogMethodExit(cardProduct);
            return cardProduct;
        }
        private void btnLoadCardSerialMapping_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                displayMessageLine("");
                if (cardList.Count == swipedCount)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2122));
                    //"All Cards entered. Press OK" 
                }
                int totalCardsForProduct = 0;
                int swippedCardsForProduct = 0;
                if (selectedProductId > -1)
                {
                    totalCardsForProduct = GetTotalCardCount(selectedProductId, selectedProductName);
                    swippedCardsForProduct = GetSwippedCardCount(selectedProductId, selectedProductName);
                    if (swippedCardsForProduct == totalCardsForProduct)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 4141, selectedProductName));
                        // "All cards entered for &1"
                    }
                }
                if (string.IsNullOrEmpty(txtFromCardSerialNumber.Text.Trim()))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 4042));
                    //"From Serial Number and To Serial Number cannot be blank"); 
                }
                displayMessageLine(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 1008));
                // "Processing... Please wait."); 
                string toSerialNumber;
                if (string.IsNullOrEmpty(txtToCardSerialNumber.Text.Trim()))
                {
                    toSerialNumber = GetToSerialNumber(txtFromCardSerialNumber.Text);
                    txtToCardSerialNumber.Text = toSerialNumber;
                }
                else
                {
                    toSerialNumber = txtToCardSerialNumber.Text.Trim();
                }
                POSUtils.SetLastActivityDateTime();
                TagSerialMappingListBL tagSerialMappingListBL = new TagSerialMappingListBL(POSStatic.Utilities.ExecutionContext);
                List<KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>(TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER_FROM, txtFromCardSerialNumber.Text.Trim()));
                searchParams.Add(new KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>(TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER_TO, toSerialNumber));
                searchParams.Add(new KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>(TagSerialMappingDTO.SearchByParameters.ALREADY_ISSUED, "N"));
                List<TagSerialMappingDTO> tagSerialMappingDTOList = tagSerialMappingListBL.GetTagSerialMappingDTOList(searchParams);

                POSUtils.SetLastActivityDateTime();
                if (tagSerialMappingDTOList == null || tagSerialMappingDTOList.Any() == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 4044));
                    //"No serial-mapped cards found for the given series");
                }
                int totalCount = tagSerialMappingDTOList.Count;
                if (MAXLOADMULTIPLECARDS < totalCount)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 4043, totalCount, MAXLOADMULTIPLECARDS));
                    //"Entered series has " + totalCount.ToString() + " mappings. Maximum allowed is:" + MAXLOADMULTIPLECARDS.ToString()); 
                }
                List<cardproduct> tempList = lstCardProducts;
                if (selectedProductId > -1)
                {
                    tempList = lstCardProducts.Where(cp => cp.productId == selectedProductId
                                                          && cp.productName == selectedProductName && cp.cards.Count < cp.quantity).ToList();
                    if (tempList == null)
                    {
                        tempList = new List<cardproduct>();
                    }
                }

                int serialCardCount = 0;
                string serialCardNumber = string.Empty;
                bool hasIssueNotificationDevice = false;
                hasIssueNotificationDevice = HasIssueNotificationDevice(tempList);

                List<NotificationTagsDTO> inUseNotificationTagsDTOList = new List<NotificationTagsDTO>();
                if (hasIssueNotificationDevice)
                {
                    inUseNotificationTagsDTOList = GetNotificationTagsDTO(tagSerialMappingDTOList);
                }
                StringBuilder failureMsgs = new StringBuilder();
                //DateTime startTime = DateTime.Now;
                tempAccountList = GetCardInfoForExecuteProcess(tagSerialMappingDTOList);
                for (int i = 0; i < tempList.Count; i++)
                {
                    POSUtils.SetLastActivityDateTime();
                    this.Cursor = Cursors.WaitCursor;

                    if (tagSerialMappingDTOList.Any() == false)
                    {
                        break;
                    }
                    cardproduct cp = tempList[i];
                    if (selectedProductId == -1
                        || (selectedProductId == cp.productId && selectedProductName == cp.productName))
                    {
                        while (cp.cards.Count < cp.quantity)
                        {
                            serialCardNumber = GetNextCardNumber(tagSerialMappingDTOList);
                            if (string.IsNullOrWhiteSpace(serialCardNumber))
                            {
                                break;
                            }
                            try
                            {
                                //CanUserSerialCardNumber(serialCardNumber);
                                CanUserSerialCardNumber1(serialCardNumber);
                                if (TagNumber.IsValid(POSStatic.Utilities.ExecutionContext, serialCardNumber))
                                {
                                    RadianValidation(serialCardNumber, inUseNotificationTagsDTOList, cp);
                                    cp.cards.Add(serialCardNumber);
                                    swipedCount++;
                                    serialCardCount++;
                                    swippedCardsForProduct++;
                                    tagSerialMappingDTOList = RemoveCardNumberFromList(tagSerialMappingDTOList, serialCardNumber);
                                    // break;
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 1697, serialCardNumber));
                                    //Invalid card number &1   
                                }
                            }
                            catch (Exception ex)
                            {
                                failureMsgs.Append(ex.Message);
                                //failureMsgs.Append(Environment.NewLine);
                                log.Error(ex);
                                tagSerialMappingDTOList = RemoveCardNumberFromList(tagSerialMappingDTOList, serialCardNumber);
                            }
                        }
                        if (cardList.Count == swipedCount)
                        {
                            break;
                        }
                        if (selectedProductId > -1 && swippedCardsForProduct == totalCardsForProduct)
                        {
                            break;
                        }
                    }
                }

                //DateTime endTime = DateTime.Now;
                //MessageBox.Show((endTime - startTime).Seconds.ToString());
                DisplayInGrid();
                if (failureMsgs != null && failureMsgs.Length > 1)
                {
                    POSUtils.ParafaitMessageBox(failureMsgs.ToString());
                }
                displayMessageLine(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 36,
                                   serialCardCount.ToString() + " " + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "cards")));
                //"&1 loaded successfully");
                if (cardList.Count == swipedCount)
                {
                    displayMessageLine(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2122));
                    log.Info(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2122));
                    //"All Cards entered. Press OK."
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private List<AccountDTO> GetCardInfoForExecuteProcess(List<TagSerialMappingDTO> tagSerialMappingDTOList)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<AccountDTO> accountDTOList = new List<AccountDTO>();
            List<string> cardNoList = tagSerialMappingDTOList.Select(tsm => tsm.TagNumber).ToList();
            AccountListBL accountListBL = new AccountListBL(POSStatic.Utilities.ExecutionContext);
            accountDTOList = accountListBL.CardNumberInfoForExecuteProcess(cardNoList);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(accountDTOList);
            return accountDTOList;
        }

        private string GetNextCardNumber(List<TagSerialMappingDTO> tagSerialMappingDTOList)
        {
            log.LogMethodEntry();
            string cardNumber = string.Empty;
            if (tagSerialMappingDTOList != null && tagSerialMappingDTOList.Any())
            {
                cardNumber = tagSerialMappingDTOList[0].TagNumber;
            }
            log.LogMethodExit(cardNumber);
            return cardNumber;
        }
        private void CanUserSerialCardNumber(string serialCardNumber)
        {
            log.LogMethodEntry(serialCardNumber);
            POSUtils.SetLastActivityDateTime();
            Card swipedCard = new Card(serialCardNumber, POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);
            //AccountBL accountsBL = new AccountBL(POSStatic.Utilities.ExecutionContext, serialCardNumber, false);
            //AccountListBL accountListBL = new AccountListBL(POSStatic.Utilities.ExecutionContext);
            //AccountDTO accountDTO = tempAccountList.Find(act => act.TagNumber == serialCardNumber);
            //if (accountDTO.TechnicianCard.Equals("Y"))
            if (swipedCard.technician_card.Equals('Y'))
            {
                throw new ValidationException(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 197, serialCardNumber));
            }
            else if (swipedCard.CardStatus != "NEW")
            //else if (accountDTO.AccountId > -1)
            {
                displayMessageLine("Issued Card");
                log.Info(serialCardNumber + " is an Issued Card");
            }
            bool cardFound = false;
            for (int i = 0; i < lstCardProducts.Count; i++)
            {
                if (lstCardProducts[i].cards.Contains(swipedCard.CardNumber))
                //if (lstCardProducts[i].cards.Contains(accountDTO.TagNumber))
                {
                    cardFound = true;
                    break;
                }
            }
            if (cardFound)
            {
                throw new ValidationException(serialCardNumber + " - " + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 59));
            }
            log.LogMethodExit();
        }

        private void CanUserSerialCardNumber1(string serialCardNumber)
        {
            log.LogMethodEntry(serialCardNumber);
            POSUtils.SetLastActivityDateTime();
            //Card swipedCard = new Card(serialCardNumber, POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);
            //AccountBL accountsBL = new AccountBL(POSStatic.Utilities.ExecutionContext, serialCardNumber, false);
            //AccountListBL accountListBL = new AccountListBL(POSStatic.Utilities.ExecutionContext);
            AccountDTO accountDTO = tempAccountList.Find(act => act.TagNumber == serialCardNumber);
            if (accountDTO.TechnicianCard.Equals("Y"))
            //if (swipedCard.technician_card.Equals('Y'))
            {
                throw new ValidationException(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 197, serialCardNumber));
            }
            //else if (swipedCard.CardStatus != "NEW")
            else if (accountDTO.AccountId > -1)
            {
                displayMessageLine("Issued Card");
                log.Info(serialCardNumber + " is an Issued Card");
            }
            bool cardFound = false;
            for (int i = 0; i < lstCardProducts.Count; i++)
            {
                //if (lstCardProducts[i].cards.Contains(swipedCard.CardNumber))
                if (lstCardProducts[i].cards.Contains(accountDTO.TagNumber))
                {
                    cardFound = true;
                    break;
                }
            }
            if (cardFound)
            {
                throw new ValidationException(serialCardNumber + " - " + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 59));
            }
            log.LogMethodExit();
        }
        private void RadianValidation(string serialCardNumber, List<NotificationTagsDTO> inUseNotificationTagsDTOList, cardproduct cp)
        {
            log.LogMethodEntry(serialCardNumber);
            if (cp.productId > -1)
            {
                ProductsDTO productDTO = GetProductDTO(cp.productId);
                if (productDTO != null
                    && productDTO.IssueNotificationDevice != null
                    && (bool)productDTO.IssueNotificationDevice)
                {
                    if ((inUseNotificationTagsDTOList == null || inUseNotificationTagsDTOList.Any() == false)
                        || (inUseNotificationTagsDTOList != null && inUseNotificationTagsDTOList.Any()
                            && inUseNotificationTagsDTOList.Exists(ntd => ntd.TagNumber == serialCardNumber) == false))
                    {
                        throw new ValidationException(serialCardNumber + " - " + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2959));
                        //Radian Tag cannot be issued. Radian Tag is not in Inventory or has an incorrect status. 
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private List<TagSerialMappingDTO> RemoveCardNumberFromList(List<TagSerialMappingDTO> tagSerialMappingDTOList, string serialCardNumber)
        {
            log.LogMethodEntry(serialCardNumber);
            if (tagSerialMappingDTOList != null && tagSerialMappingDTOList.Any())
            {
                int rowIndex = tagSerialMappingDTOList.FindIndex(tsm => tsm.TagNumber == serialCardNumber);
                if (rowIndex > -1)
                {
                    tagSerialMappingDTOList.RemoveAt(rowIndex);
                }
            }
            log.LogMethodExit();
            return tagSerialMappingDTOList;
        }
        private string GetToSerialNumber(string fromSerialNumber)
        {
            log.LogMethodEntry(fromSerialNumber);
            string toSerialNumber = fromSerialNumber;
            string tempString = Regex.Match(fromSerialNumber, @"-?\d+").Value;
            if (string.IsNullOrWhiteSpace(tempString) == false)
            {
                int numberValue = 0;
                if (int.TryParse(tempString, out numberValue))
                {
                    int toSerialNumberValue = GetToSerialNumberValue();
                    numberValue = numberValue + toSerialNumberValue;
                    string stringValue = numberValue.ToString();
                    char[] charList = fromSerialNumber.ToArray();
                    if (stringValue.Length < fromSerialNumber.Length)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < charList.Length; i++)
                        {
                            stringBuilder.Append(charList[i]);
                            string finalString = stringBuilder.ToString() + stringValue;
                            if (finalString.Length == fromSerialNumber.Length)
                            {
                                toSerialNumber = finalString;
                                break;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(toSerialNumber);
            return toSerialNumber;
        }

        private int GetToSerialNumberValue()
        {
            log.LogMethodEntry();
            int toNumber = cardList.Count - swipedCount;
            if (selectedProductId > -1)
            {
                toNumber = GetTotalCardCount(selectedProductId, selectedProductName) - GetSwippedCardCount(selectedProductId, selectedProductName);
            }
            log.LogMethodExit(toNumber);
            return toNumber;
        }

        private bool HasIssueNotificationDevice(List<cardproduct> cardproductList)
        {
            log.LogMethodEntry();
            bool hasIssueNotificationDevice = false;
            for (int i = 0; i < cardproductList.Count; i++)
            {
                cardproduct cp = cardproductList[i];
                if (cp.cards.Count < cp.quantity)
                {
                    if (cp.productId > -1)
                    {
                        ProductsDTO productDTO = GetProductDTO(cp.productId);
                        if (productDTO != null
                            && productDTO.IssueNotificationDevice != null
                            && (bool)productDTO.IssueNotificationDevice)
                        {
                            hasIssueNotificationDevice = true;
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit();
            return hasIssueNotificationDevice;
        }
        private ProductsDTO GetProductDTO(int productId)
        {
            log.LogMethodEntry(productId);
            ProductsDTO productsDTO = null;
            if (productDictionary.ContainsKey(productId))
            {
                productsDTO = productDictionary[productId];
            }
            else
            {
                productsDTO = new Products(POSStatic.Utilities.ExecutionContext, productId, false, false).GetProductsDTO;
                productDictionary.Add(productId, productsDTO);
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
            return productsDTO;
        }
        private List<NotificationTagsDTO> GetNotificationTagsDTO(List<TagSerialMappingDTO> tagSerialMappingDTOList)
        {
            log.LogMethodEntry();
            List<NotificationTagsDTO> inUseNotificationTagsDTOList = new List<NotificationTagsDTO>();
            if (tagSerialMappingDTOList != null && tagSerialMappingDTOList.Any())
            {
                POSUtils.SetLastActivityDateTime();
                List<string> cardNumberList = tagSerialMappingDTOList.Select(tsm => tsm.TagNumber).ToList();
                List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> tagsSearchParameters = new List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>>();
                tagsSearchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.SITE_ID, (POSStatic.Utilities.ParafaitEnv.IsCorporate ? POSStatic.Utilities.ParafaitEnv.SiteId : -1).ToString()));
                tagsSearchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.TAGNOTIFICATIONSTATUS, TagNotificationStatusConverter.ToString(TagNotificationStatus.IN_USE)));
                tagsSearchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.ISINSTORAGE, 0.ToString()));
                NotificationTagsListBL notificationTagsListBL = new NotificationTagsListBL(POSStatic.Utilities.ExecutionContext);
                inUseNotificationTagsDTOList = notificationTagsListBL.GetAllNotificationTagsList(cardNumberList, tagsSearchParameters);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
            return inUseNotificationTagsDTOList;
        }
        private void LoadCardProductList()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            productListForDropDown = new List<ProductsDTO>();
            ProductsDTO productsDTO = new ProductsDTO();
            productsDTO.ProductId = -1;
            productsDTO.ProductName = "-All-";
            productListForDropDown.Add(productsDTO);
            if (lstCardProducts != null && lstCardProducts.Any())
            {
                for (int i = 0; i < lstCardProducts.Count; i++)
                {
                    if (productListForDropDown.Exists(p => p.ProductName == lstCardProducts[i].productName
                                                         && p.ProductId == lstCardProducts[i].productId) == false)
                    {
                        productsDTO = new ProductsDTO();
                        productsDTO.ProductId = lstCardProducts[i].productId;
                        productsDTO.ProductName = lstCardProducts[i].productName;
                        productListForDropDown.Add(productsDTO);
                    }
                }
            }
            cmbCardProducts.DataSource = productListForDropDown;
            cmbCardProducts.DisplayMember = "ProductName";
            cmbCardProducts.ValueMember = "ProductId";
            log.LogMethodExit();
        }
        private void dgvMultipleCards_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dgvMultipleCards.Columns[e.ColumnIndex].Name == "Card_Number"
                        && (dgvMultipleCards.Rows[e.RowIndex].Cells["Card_Number"].Value != null
                            && dgvMultipleCards.Rows[e.RowIndex].Cells["Card_Number"].Value.ToString().Length > 1))
                    {
                        if (dgvMultipleCards.Rows[e.RowIndex].Cells["Card_Number"].Tag != null
                            && dgvMultipleCards.Rows[e.RowIndex].Cells["Card_Number"].Tag.ToString() == "UNSELECTED")
                        {
                            dgvMultipleCards.Rows[e.RowIndex].Cells["Card_Number"].Tag = "SELECT";
                            dgvMultipleCards.Rows[e.RowIndex].Cells["Card_Number"].Style.BackColor = Color.PaleTurquoise;
                        }
                        else
                        {
                            dgvMultipleCards.Rows[e.RowIndex].Cells["Card_Number"].Tag = "UNSELECTED";
                            dgvMultipleCards.Rows[e.RowIndex].Cells["Card_Number"].Style.BackColor = dgvMultipleCards.Rows[e.RowIndex].Cells["SerialNumber"].Style.BackColor;
                        }
                        dgvMultipleCards.EndEdit();
                        dgvMultipleCards.CurrentCell = dgvMultipleCards.Rows[e.RowIndex].Cells["SerialNumber"];
                        dgvMultipleCards.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void btnRemoveCardFromLine_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                List<cardproduct> selectedCardproductList = GetSelectedCardProductLines();
                if (selectedCardproductList.Any() == false)
                {
                    string msg = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 4142);
                    //Do you want to remove all mapped cards?
                    string title = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "Remove Cards");
                    if (POSUtils.ParafaitMessageBox(msg, title, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ClearCardsFromLines(null);
                        DisplayInGrid();
                    }
                    else
                    {
                        string msg1 = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2460);
                        //Please select a record to proceed
                        throw new ValidationException(msg1);
                    }
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 4143);
                    // "Do you want to remove selected cards?"
                    string title = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "Remove Cards");
                    if (POSUtils.ParafaitMessageBox(msg, title, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ClearCardsFromLines(selectedCardproductList);
                        DisplayInGrid();
                        int rowIndex = GetRowIdFromGrid(selectedCardproductList[0]);
                        SetAsCurrentGridCell(rowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message);
            }
            log.LogMethodExit();
        }
        private List<cardproduct> GetSelectedCardProductLines()
        {
            log.LogMethodEntry();
            List<cardproduct> selectedCardproductList = new List<cardproduct>();
            for (int i = 0; i < dgvMultipleCards.Rows.Count; i++)
            {
                if (dgvMultipleCards.Rows[i].Cells["Card_Number"].Tag != null
                    && dgvMultipleCards.Rows[i].Cells["Card_Number"].Tag.ToString() == "SELECT")
                {
                    selectedCardproductList.Add(dgvMultipleCards.Rows[i].Cells["SerialNumber"].Tag as cardproduct);
                }
            }
            log.LogMethodExit();
            return selectedCardproductList;
        }
        private void ClearCardsFromLines(List<cardproduct> selectedCardproductList)
        {
            log.LogMethodEntry();
            if (selectedCardproductList != null)
            {
                if (selectedCardproductList.Any())
                {
                    for (int i = 0; i < lstCardProducts.Count; i++)
                    {
                        if (selectedCardproductList.Exists(cp => cp.serialNo == lstCardProducts[i].serialNo &&
                                                                 cp.productId == lstCardProducts[i].productId &&
                                                                 cp.productName == lstCardProducts[i].productName))
                        {
                            swipedCount = swipedCount - lstCardProducts[i].cards.Count;
                            lstCardProducts[i].cards.Clear();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < lstCardProducts.Count; i++)
                {
                    if (selectedProductId == -1
                         || (selectedProductId > -1 && lstCardProducts[i].productId == selectedProductId && lstCardProducts[i].productName == selectedProductName))
                    {
                        swipedCount = swipedCount - lstCardProducts[i].cards.Count;
                        lstCardProducts[i].cards.Clear();
                    }
                }
            }
            log.LogMethodExit();
        }
        private void cmbCardProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (cmbCardProducts.SelectedIndex > -1 && userAction)
                {
                    KeyValuePair<int, string> keyValuePair = GetSelectedProductInfo();
                    selectedProductId = keyValuePair.Key;
                    selectedProductName = keyValuePair.Value;
                    DisplayInGrid();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private int GetSwippedCardCount(int productId, string productName)
        {
            log.LogMethodEntry(productId, productName);
            int swipeCount = 0;
            for (int i = 0; i < lstCardProducts.Count; i++)
            {
                if (productId == lstCardProducts[i].productId && productName == lstCardProducts[i].productName)
                {
                    swipeCount = swipeCount + lstCardProducts[i].cards.Count;
                }
            }
            log.LogMethodExit(swipeCount);
            return swipeCount;
        }
        private int GetTotalCardCount(int productId, string productName)
        {
            log.LogMethodEntry(productId, productName);
            int totolCardCount = 0;
            for (int i = 0; i < lstCardProducts.Count; i++)
            {
                if (productId == lstCardProducts[i].productId && productName == lstCardProducts[i].productName)
                {
                    totolCardCount = totolCardCount + lstCardProducts[i].quantity;
                }
            }
            log.LogMethodExit(totolCardCount);
            return totolCardCount;
        }
        private void btnSwap_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                displayMessageLine("");
                List<cardproduct> selectedCardproductList = GetSelectedCardProductLines();
                if (selectedCardproductList.Any() == false || selectedCardproductList.Count != 2)
                {
                    string msg = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 4144);
                    //"Sorry cannot proceeed. Swap requires two card entries."
                    throw new ValidationException(msg);
                }
                DoSwap(selectedCardproductList);
                DisplayInGrid();
                int rowIndex = GetRowIdFromGrid(selectedCardproductList[0]);
                SetAsCurrentGridCell(rowIndex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message);
            }
            log.LogMethodExit();
        }
        private void DoSwap(List<cardproduct> selectedCardproductList)
        {
            log.LogMethodEntry();
            if (selectedCardproductList[0].cards.Count != selectedCardproductList[1].cards.Count)
            {
                string msg = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 4145);
                //"Both entries should have same number of cards"
                throw new ValidationException(msg);

            }
            List<string> cardFromFirstEntry = selectedCardproductList[0].cards;
            List<string> cardFromSecondEntry = selectedCardproductList[1].cards;
            selectedCardproductList[0].cards = new List<string>(cardFromSecondEntry);
            selectedCardproductList[1].cards = new List<string>(cardFromFirstEntry);
            log.LogMethodExit();
        }
        private Dictionary<string, string> GetMappedCardList()
        {
            log.LogMethodEntry();
            Dictionary<string, string> mappedCardList = new Dictionary<string, string>();
            if (cardList.Any())
            {
                mappedCardList = cardList.Where(cp => string.IsNullOrWhiteSpace(cp.Value) == false).ToDictionary(t => t.Key, t => t.Value);
            }
            log.LogMethodExit(mappedCardList);
            return mappedCardList;
        }
        private void InitializeKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                virtualKeyboard = new VirtualKeyboardController();
                virtualKeyboard.Initialize(this, new List<Control>() { btnShowKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(POSStatic.Utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private int GetRowIdFromGrid(cardproduct cp)
        {
            log.LogMethodEntry(cp);
            int rowIndex = -1;
            if (cp != null)
            {
                for (int i = 0; i < dgvMultipleCards.Rows.Count; i++)
                {
                    if (dgvMultipleCards.Rows[i].Cells["SerialNumber"].Tag != null &&
                        (dgvMultipleCards.Rows[i].Cells["SerialNumber"].Tag as cardproduct).serialNo == cp.serialNo)
                    {
                        rowIndex = i;
                        break;
                    }
                }
            }
            log.LogMethodExit(rowIndex);
            return rowIndex;
        }
        private void SetAsCurrentGridCell(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            if (rowIndex > -1)
            {
                dgvMultipleCards.Focus();
                if (rowIndex < dgvMultipleCards.Rows.Count - 1)
                {
                    rowIndex++;
                }
                dgvMultipleCards.CurrentCell = dgvMultipleCards.Rows[rowIndex].Cells["SerialNumber"];
                dgvMultipleCards.CurrentCell.Selected = true;
            }
            log.LogMethodExit();
        }
        private bool SameCardIsOnMulitpleLines(out List<KeyValuePair<string, List<string>>> sameCardProdList)
        {
            log.LogMethodEntry();
            bool sameCardOnMulitpleLines = false;
            sameCardProdList = new List<KeyValuePair<string, List<string>>>();
            if (lstCardProducts != null)
            {
                for (int i = 0; i < lstCardProducts.Count; i++)
                {
                    cardproduct cp = lstCardProducts[i];
                    if (cp.cards != null && cp.cards.Any())
                    {
                        for (int j = 0; j < cp.cards.Count; j++)
                        {
                            string cardNo = cp.cards[j];
                            if (sameCardProdList.Exists(kvp => kvp.Key == cardNo) == false)
                            {
                                List<cardproduct> sameCardLines = lstCardProducts.Where(lcp => lcp.serialNo != cp.serialNo
                                                                                           && lcp.cards != null && lcp.cards.Exists(cc => cc == cardNo)).ToList();
                                if (sameCardLines != null && sameCardLines.Any())
                                {
                                    List<string> productInfoList = new List<string>();
                                    string productInfoInitial = "[ " + cp.serialNo.ToString("N0") + " ] - " + cp.productName;
                                    productInfoList.Add(productInfoInitial);
                                    for (int k = 0; k < sameCardLines.Count; k++)
                                    {
                                        string productInfo = "[ " + sameCardLines[k].serialNo.ToString("N0") + " ] - " + sameCardLines[k].productName;
                                        productInfoList.Add(productInfo);
                                    }
                                    KeyValuePair<string, List<string>> valuePair = new KeyValuePair<string, List<string>>(cardNo, productInfoList);
                                    sameCardProdList.Add(valuePair);
                                }
                            }
                        }
                    }
                }
            }
            if (sameCardProdList.Any())
            {
                sameCardOnMulitpleLines = true;
            }
            log.LogMethodExit(sameCardOnMulitpleLines);
            return sameCardOnMulitpleLines;
        }
        private DialogResult ShowSameCardOnMultipeLineMessage(List<KeyValuePair<string, List<string>>> sameCardProdList)
        {
            DialogResult dialogResult = DialogResult.No;
            POSUtils.SetLastActivityDateTime();
            using (frmSameCardOnMultipleLines f = new frmSameCardOnMultipleLines(sameCardProdList))
            {
                if (f.ShowDialog() == DialogResult.Yes)
                {
                    dialogResult = DialogResult.Yes;
                }
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit(dialogResult);
            return dialogResult;
        }
    }
}
