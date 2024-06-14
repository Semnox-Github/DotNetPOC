/********************************************************************************************
* Project Name - Parafait POS
* Description  - frmInputCards 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
*2.70.0      26-Mar-2019     Guru S A            Booking phase 2 enhancement changes 
*2.120.1     31-May-2021     Nitin Pai           Validate customer license for card mapping
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Logger = Semnox.Core.Utilities.Logger;

namespace Parafait_POS.Reservation
{
    public partial class frmInputCards : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Transaction _transaction; List<AttractionBooking> _atbList; List<string> _cardNumberList;
        bool allowPhysicalCardInput = true;

        private readonly TagNumberParser tagNumberParser;
        public class clsCard
        {
            internal string cardNumber;
            internal bool selected = false;
            internal List<string> products = new List<string>();
            internal clsCard(string inCardNumber)
            {
                cardNumber = inCardNumber;
            }
        }
        internal class clsProductAllocation
        {
            internal string productName;
            internal int Quantity;
            internal List<clsCard> cardNumbers = new List<clsCard>();
            internal object targetObject;
            internal clsProductAllocation(string inProductName, int inQty, object targetObj)
            {
                productName = inProductName;
                Quantity = inQty;
                targetObject = targetObj;
            }
        }

        internal class clsInputAllocation
        {
            internal List<clsCard> lstInputCards = new List<clsCard>();
            internal List<clsProductAllocation> productAllocation = new List<clsProductAllocation>();

            internal void allocate(clsProductAllocation targetProduct, bool allowPhysicalCardInput, string panelName)
            {
                List<clsCard> targetList = targetProduct.cardNumbers;
                Allocate(targetList, allowPhysicalCardInput, panelName, targetProduct.Quantity);
            }
            internal void Allocate(List<clsCard> targetList, bool allowPhysicalCardInput, string panelName, int qtyLimit = -1)
            {
                List<clsCard> fromList = null;
                bool fromInputList = false;
                if (!targetList.Equals(lstInputCards))
                {
                    foreach (clsCard card in lstInputCards)
                    {
                        if (card.selected)
                        {
                            fromInputList = true;
                            fromList = lstInputCards;
                            break;
                        }
                    }
                }

                if (fromList == null)
                {
                    foreach (clsProductAllocation product in productAllocation)
                    {
                        foreach (clsCard card in product.cardNumbers)
                        {
                            if (card.selected)
                            {
                                fromList = product.cardNumbers;
                                break;
                            }
                        }

                        if (fromList != null)
                            break;
                    }
                }

                if (fromList != null)
                {
                    List<clsCard> copyList = new List<clsCard>(fromList);
                    foreach (clsCard card in copyList)
                    {
                        if (qtyLimit != -1 && targetList.Count == qtyLimit)
                            break;

                        if (card.selected)
                        {
                            if (fromInputList == false || allowPhysicalCardInput == false)
                            {
                                fromList.Remove(card);
                            }
                            if (panelName == "flpTrxCards")
                            {
                                if (targetList.Exists(tc => tc.cardNumber == card.cardNumber) == false)
                                {
                                    targetList.Add(CloneCard(card));
                                }
                            }
                            else
                            {
                                targetList.Add(CloneCard(card));
                            }
                            card.selected = false;
                        }
                    }

                    foreach (clsProductAllocation product in productAllocation)
                    {
                        foreach (clsCard card in product.cardNumbers)
                        {
                            if (card.selected)
                            {
                                card.selected = false;
                            }
                        }
                    }
                }
            }

            private clsCard CloneCard(clsCard clsCard)
            {
                log.LogMethodEntry(clsCard);
                clsCard cloneCard = new clsCard(clsCard.cardNumber);
                cloneCard.selected = false;
                if (clsCard.products != null && clsCard.products.Any())
                {
                    cloneCard.products = new List<string>();
                    for (int i = 0; i < clsCard.products.Count; i++)
                    {
                        cloneCard.products.Add(clsCard.products[i]);
                    }
                }

                log.LogMethodExit(clsCard);
                return cloneCard;
            }
        }
        clsInputAllocation InputAllocation = new clsInputAllocation();

        public frmInputCards(string ProductName, int cardCount, Transaction transaction, List<string> cardNumberList)
        {
            Logger.setRootLogLevel(log);
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            InitializeComponent();

            _transaction = transaction;
            _cardNumberList = cardNumberList;

            getCardsInTrx(transaction);
            getAllottedCardsList(ProductName, cardCount);
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        public frmInputCards(string ProductName, List<clsCard> inScheduleCards, List<AttractionBooking> atbList)
        {
            Logger.setRootLogLevel(log);
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            InitializeComponent();

            _atbList = atbList;

            getCardsInScheduleList(inScheduleCards);
            getAllottedCardsList(ProductName, atbList);

            allowPhysicalCardInput = false;
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        public frmInputCards(string ProductName, Transaction transaction, List<AttractionBooking> atbList)
        {
            Logger.setRootLogLevel(log);
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            InitializeComponent();

            _transaction = transaction;
            _atbList = atbList;

            getCardsInTrx(transaction);
            getAllottedCardsList(ProductName, atbList);
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        void getCardsInTrx(Transaction trx)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (trx == null)
                return;

            foreach (Transaction.TransactionLine tl in trx.TrxLines)
            {
                if (tl.LineValid)
                {
                    if (string.IsNullOrEmpty(tl.CardNumber) == false)
                    {
                        if (InputAllocation.lstInputCards.Find(x => x.cardNumber == tl.CardNumber) == null)
                            InputAllocation.lstInputCards.Add(new clsCard(tl.CardNumber));
                    }
                }
            }

            foreach (clsCard inCard in InputAllocation.lstInputCards)
            {
                foreach (Transaction.TransactionLine tl in trx.TrxLines)
                {
                    if (tl.LineValid)
                    {
                        if (inCard.cardNumber.Equals(tl.CardNumber))
                        {
                            if (tl.AttractionDetails != null)
                                inCard.products.Add(tl.AttractionDetails);
                            else
                                inCard.products.Add(tl.ProductName);
                        }
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        void getCardsInScheduleList(List<clsCard> inScheduleCards)
        {
            log.LogMethodEntry();
            if (inScheduleCards == null)
                return;

            foreach (clsCard inCard in inScheduleCards)
            {
                if (InputAllocation.lstInputCards.Find(x => x.cardNumber == inCard.cardNumber) == null)
                    InputAllocation.lstInputCards.Add(new clsCard(inCard.cardNumber));
            }

            foreach (clsCard addedCard in InputAllocation.lstInputCards)
            {
                foreach (clsCard inCard in inScheduleCards)
                {
                    if (addedCard.cardNumber.Equals(inCard.cardNumber))
                    {
                        addedCard.products.Add(inCard.products[0]);
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        void getAllottedCardsList(string ProductName, List<AttractionBooking> atbList)
        {
            log.LogMethodEntry();
            foreach (AttractionBooking atb in atbList)
            {
                InputAllocation.productAllocation.Add(new clsProductAllocation(ProductName + Environment.NewLine +
                    atb.AttractionBookingDTO.AttractionScheduleName + " - " + atb.AttractionBookingDTO.ScheduleFromDate.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT),
                    atb.AttractionBookingDTO.BookedUnits,
                    atb));
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        void getAllottedCardsList(string ProductName, int cardCount)
        {
            InputAllocation.productAllocation.Add(new clsProductAllocation(ProductName, cardCount, null));
        }

        private void frmInputCards_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (allowPhysicalCardInput)
                Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));

            POSStatic.Utilities.setLanguage(this);
            refreshScreen();
            log.LogMethodExit();
        }

        void paintButton(clsCard inCard, Button btn)
        {
            if (inCard.selected)
            {
                btn.BackgroundImage = Properties.Resources.BlankBlack;
                btn.ForeColor = Color.White;
            }
            else
            {
                if (inCard.products.Count > 0)
                    btn.BackgroundImage = btnCardSample.BackgroundImage;
                else
                    btn.BackgroundImage = btnCardTappedSample.BackgroundImage;

                btn.ForeColor = btnCardSample.ForeColor;
            }
        }

        void refreshScreen()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            flpTrxCards.Controls.Clear();
            foreach (clsCard inCard in InputAllocation.lstInputCards)
            {
                Button btn = new Button();
                btn.FlatStyle = btnCardSample.FlatStyle;
                btn.FlatAppearance.BorderColor = btnCardSample.FlatAppearance.BorderColor;
                btn.FlatAppearance.BorderSize = btnCardSample.FlatAppearance.BorderSize;
                btn.FlatAppearance.MouseDownBackColor = btnCardSample.FlatAppearance.MouseDownBackColor;
                btn.FlatAppearance.MouseOverBackColor = btnCardSample.FlatAppearance.MouseOverBackColor;
                if (inCard.products.Count > 0)
                    btn.BackgroundImage = btnCardSample.BackgroundImage;
                else
                    btn.BackgroundImage = btnCardTappedSample.BackgroundImage;
                btn.TextAlign = btnCardSample.TextAlign;
                btn.Size = btnCardSample.Size;
                btn.Margin = btnCardSample.Margin;

                btn.Text = inCard.cardNumber + Environment.NewLine + string.Join(Environment.NewLine, inCard.products);
                btn.AutoEllipsis = true;
                btn.Tag = inCard;

                paintButton(inCard, btn);

                btn.Click += (object sender, EventArgs e) =>
                {
                    clsCard clickedCard = btn.Tag as clsCard;
                    clickedCard.selected = !clickedCard.selected;

                    paintButton(clickedCard, btn);
                };

                flpTrxCards.Controls.Add(btn);
            }

            flpProductsToAllocate.Controls.Clear();
            foreach (clsProductAllocation product in InputAllocation.productAllocation)
            {
                FlowLayoutPanel flp = new FlowLayoutPanel();
                flp.FlowDirection = flpAllocatedCardsSample.FlowDirection;
                flp.BorderStyle = flpAllocatedCardsSample.BorderStyle;
                flp.Width = flpAllocatedCardsSample.Width;
                flp.Height = flpProductsToAllocate.Height - 24;
                flp.WrapContents = flpAllocatedCardsSample.WrapContents;
                flp.BackColor = flpAllocatedCardsSample.BackColor;
                flp.AutoScroll = true;
                flp.Tag = product;
                flp.Name = product.productName;

                flpProductsToAllocate.Controls.Add(flp);

                Label lblProdName = new Label();
                lblProdName.Font = lblProductToAllotSample.Font;
                lblProdName.AutoSize = false;
                lblProdName.Size = lblProductToAllotSample.Size;
                lblProdName.BackColor = lblProductToAllotSample.BackColor;
                lblProdName.ForeColor = lblProductToAllotSample.ForeColor;
                lblProdName.Margin = lblProductToAllotSample.Margin;
                lblProdName.TextAlign = lblProductToAllotSample.TextAlign;
                lblProdName.Text = product.productName + Environment.NewLine + "#Tags: " + product.Quantity.ToString()
                                    + " / Remaining: " + (product.Quantity - product.cardNumbers.Count).ToString();
                flp.Controls.Add(lblProdName);

                foreach (clsCard inCard in product.cardNumbers)
                {
                    Button btn = new Button();
                    btn.FlatStyle = btnAllocatedCardSample.FlatStyle;
                    btn.FlatAppearance.BorderColor = btnAllocatedCardSample.FlatAppearance.BorderColor;
                    btn.FlatAppearance.BorderSize = btnAllocatedCardSample.FlatAppearance.BorderSize;
                    btn.FlatAppearance.MouseDownBackColor = btnAllocatedCardSample.FlatAppearance.MouseDownBackColor;
                    btn.FlatAppearance.MouseOverBackColor = btnAllocatedCardSample.FlatAppearance.MouseOverBackColor;
                    if (inCard.products.Count > 0)
                        btn.BackgroundImage = btnAllocatedCardSample.BackgroundImage;
                    else
                        btn.BackgroundImage = btnCardTappedSample.BackgroundImage;
                    btn.TextAlign = btnAllocatedCardSample.TextAlign;
                    btn.Size = btnAllocatedCardSample.Size;
                    btn.Margin = btnAllocatedCardSample.Margin;

                    btn.Text = inCard.cardNumber + Environment.NewLine + string.Join(Environment.NewLine, inCard.products);
                    btn.AutoEllipsis = true;
                    btn.Tag = inCard;

                    paintButton(inCard, btn);

                    btn.Click += (object sender, EventArgs e) =>
                    {
                        clsCard clickedCard = btn.Tag as clsCard;
                        clickedCard.selected = !clickedCard.selected;

                        paintButton(clickedCard, btn);
                    };

                    flp.Controls.Add(btn);
                }

                flp.Click += (object sender, EventArgs e) =>
                {
                    InputAllocation.allocate(flp.Tag as clsProductAllocation, allowPhysicalCardInput, flp.Name);
                    refreshScreen();
                };
            }

            bool allEntered = true;
            foreach (clsProductAllocation product in InputAllocation.productAllocation)
            {
                if (product.cardNumbers.Count < product.Quantity)
                {
                    allEntered = false;
                    break;
                }
            }
            if (allEntered)
                displayMessageLine(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2122));//"All Cards entered. Press OK."
            else
            {
                if (allowPhysicalCardInput)
                    displayMessageLine(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2123));//Please tap cards / tags or allocate from transaction
                else
                    displayMessageLine(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2124)); //"Please allocate available cards / tags"
            }
            POSUtils.SetLastActivityDateTime();
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
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                try
                {
                    CardSwiped(CardNumber);
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message);
                    log.Fatal("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void CardSwiped(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            POSUtils.SetLastActivityDateTime();
            bool alreadyAdded = false;
            if (InputAllocation.lstInputCards.Find(x => x.cardNumber == cardNumber) == null)
            {
                clsCard newCardObject = new clsCard(cardNumber);
                InputAllocation.lstInputCards.Add(newCardObject);
                MapIfSingleProduct(cardNumber);
                refreshScreen();
            }
            else
            {
                alreadyAdded = true;
            }
            //bool mappingIsPending = false;
            //foreach (clsProductAllocation product in InputAllocation.productAllocation)
            //{

            //    if (product.cardNumbers.Count != product.Quantity)
            //    {
            //        mappingIsPending = true;
            //        break;
            //        //foreach (clsProductAllocation chkProduct in InputAllocation.productAllocation)
            //        //{
            //        //    if (chkProduct.cardNumbers.Find(x => x.cardNumber == CardNumber) != null)
            //        //    {
            //        //        cardFound = true;
            //        //        break;
            //        //    }
            //        //}

            //        //if (!cardFound)
            //        //{
            //        //product.cardNumbers.Add(new clsCard(CardNumber));
            //        //if (InputAllocation.lstInputCards.Find(x => x.cardNumber == CardNumber) == null)
            //        //{
            //        //    InputAllocation.lstInputCards.Add(new clsCard(CardNumber));
            //        //}
            //        //else
            //        //{
            //        //    cardFound = true;
            //        //}
            //        //refreshScreen();
            //        //break;
            //        //}
            //        //else
            //        //{
            //        //MessageContainer.GetMessage(POSStatic.Utilities.ExecutionContext, "Edit Booking")
            //        //if (POSUtils.ParafaitMessageBox("Do you want to load the schedule to same card?", "Tap Card", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //        //{
            //        //    product.cardNumbers.Add(new clsCard(CardNumber));
            //        //    refreshScreen();
            //        //    break;
            //        //}
            //        //else

            //        //}
            //    }
            //}

            if (alreadyAdded)
            {
                displayMessageLine(POSStatic.MessageUtils.getMessage(59));
            }
            log.LogMethodExit();
        }

        private void MapIfSingleProduct(string newCardNumber)
        {
            log.LogMethodEntry(newCardNumber);
            POSUtils.SetLastActivityDateTime();
            if (InputAllocation != null && InputAllocation.productAllocation != null
               && InputAllocation.productAllocation.Count > 0 && InputAllocation.lstInputCards != null && InputAllocation.productAllocation.Count == 1
               && InputAllocation.productAllocation[0].cardNumbers.Count != InputAllocation.productAllocation[0].Quantity)
            {
                for (int i = 0; i < InputAllocation.lstInputCards.Count; i++)
                {
                    if (InputAllocation.lstInputCards[i].cardNumber == newCardNumber)
                    {
                        InputAllocation.lstInputCards[i].selected = true;
                    }
                    else
                    {
                        InputAllocation.lstInputCards[i].selected = false;
                    }
                }

                InputAllocation.allocate(InputAllocation.productAllocation[0], allowPhysicalCardInput, InputAllocation.productAllocation[0].productName);
                //InputAllocation.Allocate(InputAllocation.lstInputCards, allowPhysicalCardInput, InputAllocation.productAllocation[0].productName, InputAllocation.productAllocation[0].Quantity);
            }
            log.LogMethodExit();
        }

        private void displayMessageLine(string message, string type = "INFO")
        {
            textBoxMessageLine.Text = message;
            if (type == "ERROR")
                textBoxMessageLine.ForeColor = Color.Red;
            else
                textBoxMessageLine.ForeColor = Color.Black;
        }

        private void flpTrxCards_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                InputAllocation.Allocate(InputAllocation.lstInputCards, this.allowPhysicalCardInput, flpTrxCards.Name);
                refreshScreen();
                POSUtils.SetLastActivityDateTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                foreach (clsProductAllocation product in InputAllocation.productAllocation)
                {
                    if (product.Quantity != product.cardNumbers.Count)
                    {
                        textBoxMessageLine.Text = MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2125); // "Enter required number of cards for each product";
                        return;
                    }
                }

                if (_atbList != null)
                {
                    foreach (clsProductAllocation product in InputAllocation.productAllocation)
                    {
                        POSUtils.SetLastActivityDateTime();
                        AttractionBooking atb = product.targetObject as AttractionBooking;
                        atb.cardNumberList.Clear();
                        Products atrProducts = new Products(atb.AttractionBookingDTO.AttractionProductId);

                        foreach (clsCard card in product.cardNumbers)
                        {
                            POSUtils.SetLastActivityDateTime();
                            if (atrProducts.GetProductsDTO != null && !String.IsNullOrEmpty(atrProducts.GetProductsDTO.LicenseType) && _transaction != null)
                            {
                                AccountBL accountBL = new AccountBL(_transaction.Utilities.ExecutionContext, card.cardNumber);
                                if (accountBL.AccountDTO != null)
                                {
                                    String licenseCheck = _transaction.CheckLicenseForCustomerAndCard(accountBL.AccountDTO, atrProducts.GetProductsDTO.LicenseType,
                                        atb.AttractionBookingDTO.ScheduleFromDate, accountBL.AccountDTO.CustomerId);

                                    if (licenseCheck.Equals("N"))
                                    {
                                        String errorMessage = "Selected Card or Customer does not have the License required to puchase this product";
                                        displayMessageLine(card.cardNumber + " " + MessageContainerList.GetMessage(_transaction.Utilities.ExecutionContext, errorMessage), "ERROR");
                                        log.Info(errorMessage);
                                        return;
                                    }
                                }
                                else if (!String.IsNullOrEmpty(atrProducts.GetProductsDTO.LicenseType))
                                {
                                    String errorMessage = "Selected Card or Customer does not have the License required to puchase this product";
                                    displayMessageLine(card.cardNumber + " " + MessageContainerList.GetMessage(_transaction.Utilities.ExecutionContext, errorMessage), "ERROR");
                                    log.Info(errorMessage);
                                    return;
                                }
                            }
                            atb.cardNumberList.Add(card.cardNumber);
                        }
                    }
                }
                else if (_cardNumberList != null)
                {
                    _cardNumberList.Clear();
                    foreach (clsCard card in InputAllocation.productAllocation[0].cardNumbers)
                    {
                        _cardNumberList.Add(card.cardNumber);
                    }
                }

                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void frmInputCards_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (allowPhysicalCardInput)
                    Common.Devices.UnregisterCardReaders();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                foreach (clsCard card in InputAllocation.lstInputCards)
                {
                    card.selected = chkSelectAll.Checked;
                }

                refreshScreen();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }


    }
}
