/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - Attrcation Summary Screen
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.155.0.0   16-Jun-2023      Sathyavathi        Created for Attraction Sale in Kiosk
 ********************************************************************************************/
using System;
using System.Data;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using System.Drawing;
using Semnox.Parafait.Customer.Accounts;

namespace Parafait_Kiosk
{
    public partial class frmAttractionSummary : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductsContainerDTO> comboChildProductsContainerDTOList;
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        private ProductsContainerDTO productsContainerDTO;
        private const int MAX_QUANTITY = 999;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmAttractionSummary(KioskTransaction kioskTransaction, KioskAttractionDTO kioskAttrcationDTO)
        {
            log.LogMethodEntry("kioskTransaction", kioskAttrcationDTO);
            KioskStatic.logToFile("In frmAttractionSummary()");
            this.kioskTransaction = kioskTransaction;
            this.kioskAttractionDTO = kioskAttrcationDTO;
            try
            {
                this.productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, kioskAttrcationDTO.ProductId);
                if (productsContainerDTO == null)
                {
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4810); //ERROR: Failed to get product details
                    KioskStatic.logToFile(msg);
                    log.Error(msg);
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error getting productsContainerDTO in attrcation summary" + ex.Message);
            }
            InitializeComponent();
            SetOnscreenMessages();
            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetCustomizedFontColors();
            DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            SetKioskTimerTickValue(30);
            SetCustomImages();
            lblProductName.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
            try
            {
                comboChildProductsContainerDTOList = new List<ProductsContainerDTO>(productsContainerDTO.ProductId);
                if (productsContainerDTO.ProductType == ProductTypeValues.COMBO)
                {
                    if (productsContainerDTO.ComboProductContainerDTOList != null && productsContainerDTO.ComboProductContainerDTOList.Any())
                    {
                        foreach (ComboProductContainerDTO comboProductContainerDTO in productsContainerDTO.ComboProductContainerDTOList)
                        {
                            ProductsContainerDTO childProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, comboProductContainerDTO.ChildProductId);
                            comboChildProductsContainerDTOList.Add(childProductsContainerDTO);
                        }
                    }
                }
                else
                {
                    comboChildProductsContainerDTOList.Add(productsContainerDTO);
                }

                InitializeProductTab(comboChildProductsContainerDTOList);
                KioskStatic.Utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in Attraction Summary Constructor" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetOnscreenMessages()
        {
            log.LogMethodEntry();
            try
            {
                this.lblBookingInfo.Text = MessageContainerList.GetMessage(executionContext, 420); //Showing booking information for
                string productName = KioskHelper.GetProductName(productsContainerDTO.ProductId);
                this.lblProductName.Text = MessageContainerList.GetMessage(executionContext, productName);
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 420) + " " + productName; //"Showing booking information for "
                lblQty.Text = "(" + MessageContainerList.GetMessage(executionContext, "Quantity") + ": " + kioskAttractionDTO.Quantity.ToString() + ")";
                btnProceed.Text = MessageContainerList.GetMessage(executionContext, "Confirm Booking");
                lblSiteName.Text = KioskStatic.SiteHeading;
            }
            catch (Exception ex)
            {
                log.Error("Error in SetOnScreenMessages", ex);
            }
            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
                base.btnHome_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.Error("Error in btnCancel_Click", ex);
            }
            log.LogMethodExit();
        }

        private void frmAttractionSummary_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            this.txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008); //Processing..Please wait...
            try
            {
                //Create transaction lines for attraction products
                btnPrev.Enabled = btnCancel.Enabled = btnProceed.Enabled = false;
                ProductsContainerDTO productThatRequiresCustomer = comboChildProductsContainerDTOList.Where(p => p.RegisteredCustomerOnly.Equals("Y")).FirstOrDefault();
                bool isRegisteredCustomerOnly = (productsContainerDTO.RegisteredCustomerOnly.Equals("Y") || productThatRequiresCustomer != null) ? true : false;
                bool cardSale = KioskHelper.AttractionIsOfTypeCardSale(executionContext);
                if (cardSale)
                {
                    using (frmCardSaleOption ftc = new frmCardSaleOption())
                    {
                        DialogResult drt = ftc.ShowDialog();
                        if (drt == DialogResult.Cancel)
                        {
                            log.LogMethodExit();
                            return;
                        }

                        if (ftc.SelectedOption == frmCardSaleOption.CardSaleOption.EXISTING)
                        {
                            bool loadToSigleCard = productsContainerDTO.LoadToSingleCard;
                            int quantity = kioskAttractionDTO.Quantity;
                            List<Card> cardList = new List<Card>();
                            bool isSkipLinkingCustomer = false;
                            int i = 1;
                            while (quantity > 0)
                            {
                                string msg;
                                bool enableNote = false;
                                msg = MessageContainerList.GetMessage(executionContext, 458); //Please Tap Your Card
                                if (kioskAttractionDTO.Quantity > 1 && loadToSigleCard == false)
                                {
                                    //override the message
                                    msg = MessageContainerList.GetMessage(executionContext, 4118, i, kioskAttractionDTO.Quantity); //"Tap your card for quantity 1/3"
                                    enableNote = true;
                                }

                                using (frmAttractionTapCard fac = new frmAttractionTapCard(kioskTransaction, msg, enableNote))
                                {
                                    fac.ShowDialog();
                                    kioskTransaction = fac.GetKioskTransaction;
                                    if (fac.Card == null)
                                    {
                                        cardList = null;
                                        log.LogMethodExit();
                                        return;
                                    }
                                    else
                                    {
                                        if (isSkipLinkingCustomer == false)
                                        {
                                            bool isCustomerMandatory = (kioskTransaction.HasCustomerRecord() == false && isRegisteredCustomerOnly == true) ? true : false;
                                            bool isLinked = false;
                                            try
                                            {
                                                isLinked = CustomerStatic.LinkCustomerToTheCard(kioskTransaction, isCustomerMandatory, fac.Card);
                                                if (isLinked == false)
                                                {
                                                    isSkipLinkingCustomer = true;
                                                }
                                                if (!CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(), isCustomerMandatory, isLinked))
                                                {
                                                    log.LogMethodExit();
                                                    return;
                                                }
                                            }
                                            catch (CustomerStatic.TimeoutOccurred ex)
                                            {
                                                KioskStatic.logToFile("Timeout occured");
                                                log.Error(ex);
                                                PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                                                this.DialogResult = DialogResult.Cancel;
                                                log.LogMethodExit();
                                                return;
                                            }
                                        }
                                        if (i == 1)
                                        {
                                            kioskTransaction.SetPrimaryCard(fac.Card);
                                        }
                                        if (loadToSigleCard)
                                        {
                                            while (quantity > 0)
                                            {
                                                cardList.Add(fac.Card);
                                                quantity--;
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            cardList.Add(fac.Card);
                                        }
                                    }
                                }
                                i++;
                                quantity--;
                            }
                            if (cardList != null && cardList.Any())
                            {
                                AssignCardsToProducts(cardList);
                            }
                        }
                        else
                        {
                            bool showMsgRecommendCustomerToRegister = true;
                            bool isLinked = false;
                            try
                            {
                                isLinked = CustomerStatic.LinkCustomerToTheTransaction(kioskTransaction, executionContext, isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister);
                            }
                            catch (CustomerStatic.TimeoutOccurred ex)
                            {
                                log.Error(ex);
                                PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                                this.DialogResult = DialogResult.Cancel;
                                log.LogMethodExit();
                                return;
                            }
                            if (!CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(), 
                                    isRegisteredCustomerOnly, isLinked))
                            {
                                log.LogMethodExit();
                                return;
                            }
                            kioskAttractionDTO = kioskTransaction.GenerateAttractionCards(kioskAttractionDTO);
                        }
                    }
                }
                else
                {
                    bool showMsgRecommendCustomerToRegister = true;
                    bool isLinked = false;
                    try
                    {
                        isLinked = CustomerStatic.LinkCustomerToTheTransaction(kioskTransaction, executionContext, isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister);
                    }
                    catch (CustomerStatic.TimeoutOccurred ex)
                    {
                        log.Error(ex);
                        PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                        this.DialogResult = DialogResult.Cancel;
                        log.LogMethodExit();
                        return;
                    }
                    if (!CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(),
                            isRegisteredCustomerOnly, isLinked))
                    {
                        log.LogMethodExit();
                        return;
                    }
                }
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> addedLines = kioskTransaction.AddAttractiontProduct(kioskAttractionDTO);

                if (kioskTransaction.ShowCartInKiosk == false)
                {
                    if (kioskTransaction != null)
                    {
                        using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                        {
                            DialogResult dr = frpm.ShowDialog();
                            kioskTransaction = frpm.GetKioskTransaction;
                            if (dr != System.Windows.Forms.DialogResult.No) // back button pressed
                            {
                                DialogResult = dr;
                                this.Close();
                                log.LogMethodExit();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    frmChooseProduct.AlertUser(kioskAttractionDTO.ProductId, kioskTransaction, ProceedActionImpl);
                    DialogResult = DialogResult.OK;
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842); //adding to cart
                    KioskStatic.logToFile("frmAttractionSummary: " + msg);
                    //CloseForms();
                    GoBackToProductSelectionScreen();
                }
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error occurred while executing btnProceed_Click() in attraction summary : ";
                log.Error(msg + ex);
                KioskStatic.logToFile(msg + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                btnPrev.Enabled = btnCancel.Enabled = btnProceed.Enabled = true;
                string productName = KioskHelper.GetProductName(productsContainerDTO.ProductId);
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 420) + " " + productName; //"Showing booking information for "
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void GoBackToProductSelectionScreen()
        {
            log.LogMethodEntry();
            int lowerLimit = 2; //frmChooseProduct 
            for (int i = Application.OpenForms.Count - 1; i > lowerLimit; i--)
            {
                if (Application.OpenForms[i].Name == "frmChooseProduct")
                {
                    break;
                }
                if (attractionForms.Exists(fName => fName == Application.OpenForms[i].Name) == true)
                {
                    Application.OpenForms[i].Visible = false;
                    Application.OpenForms[i].Close();
                }
            }
            log.LogMethodExit();
        }

        private void AssignCardsToProducts(List<Card> cardList)
        {
            log.LogMethodEntry(cardList);

            if (kioskAttractionDTO.ChildAttractionBookingDTOList != null && kioskAttractionDTO.ChildAttractionBookingDTOList.Any())
            {
                foreach (Card cardItem in cardList)
                {
                    foreach (KioskAttractionChildDTO childItem in kioskAttractionDTO.ChildAttractionBookingDTOList)
                    {
                        if (childItem.ChildProductType == ProductTypeValues.ATTRACTION)
                        {
                            int childqty = childItem.ChildProductQuantity;
                            if (childItem.CardList == null)
                            {
                                childItem.CardList = new List<Card>();
                            }
                            while (childqty > 0)
                            {
                                childItem.CardList.Add(cardItem);
                                childqty--;
                            }
                        }
                    }
                }
            }
            else
            {
                kioskAttractionDTO.CardList = cardList;
            }
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            this.DialogResult = DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void vScrollBarProducts_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                if (e.NewValue > e.OldValue)
                    scrollDown(e.NewValue - e.OldValue);
                else if (e.NewValue < e.OldValue)
                    scrollUp(e.OldValue - e.NewValue);
            }
            catch { }
            log.LogMethodExit();
        }

        private void scrollDown(int value = 10)
        {
            log.LogMethodEntry(value);
            try
            {
                ResetKioskTimer();
                if (flpComboProducts.Top + flpComboProducts.Height > 3)
                {
                    flpComboProducts.Top = flpComboProducts.Top - value;
                }
            }
            catch { }
            log.LogMethodExit();
        }

        private void scrollUp(int value = 10)
        {
            log.LogMethodEntry(value);
            try
            {
                ResetKioskTimer();
                if (flpComboProducts.Top < 0)
                {
                    flpComboProducts.Top = Math.Min(0, flpComboProducts.Top + value);
                }
            }
            catch { }
            log.LogMethodExit();
        }

        private void InitializeProductTab(List<ProductsContainerDTO> productsContainerList)
        {
            log.LogMethodEntry(productsContainerList);
            this.flpComboProducts.Controls.Clear();
            try
            {
                ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, kioskAttractionDTO.ProductId);
                foreach (KioskAttractionChildDTO childDTO in kioskAttractionDTO.ChildAttractionBookingDTOList)
                {
                    ResetKioskTimer();
                    UsrCtrlAttractionSummary usrCtrlAttractionSummary = new UsrCtrlAttractionSummary(KioskStatic.Utilities.ExecutionContext, kioskAttractionDTO, childDTO);
                    //Create user ctrl element
                    this.flpComboProducts.Controls.Add(usrCtrlAttractionSummary);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in initializeProductTab() of frmAttractionSummary: " + ex);
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.AttractionSummaryBackgroundImage); //background image
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnPrev.BackgroundImage = btnCancel.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
                this.bigVerticalScrollCardProducts.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized background images for frmAttractionSummary", ex);
                KioskStatic.logToFile("Error while setting customized background images for frmAttractionSummary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                flpComboProducts.SuspendLayout();
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.AttractionSummaryBtnHomeForeColor;
                this.lblBookingInfo.ForeColor = KioskStatic.CurrentTheme.AttractionSummaryLblBookingInfoForeColor;
                this.lblProductName.ForeColor = KioskStatic.CurrentTheme.AttractionSummaryLblProductNameForeColor;
                this.lblQty.ForeColor = KioskStatic.CurrentTheme.AttractionSummaryLblQtyForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.AttractionSummaryBtnPrevForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.AttractionSummaryBtnCancelForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.AttractionSummaryBtnProceedForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.AttractionSummaryTxtMessageForeColor;
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors for frmAttractionSummary", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements of frmAttractionSummary: " + ex.Message);
            }
            flpComboProducts.ResumeLayout(true);
            log.LogMethodExit();
        }

        private void frmAttractionSummary_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmAttractionSummary_FormClosed()", ex);
            }

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        protected override void CloseForms()
        {
            log.LogMethodEntry();
            int lowerLimit = 1;
            for (int i = Application.OpenForms.Count - 1; i > lowerLimit; i--)
            {
                if (attractionForms.Exists(fName => fName == Application.OpenForms[i].Name) == true)
                {
                    Application.OpenForms[i].Visible = false;
                }
            }
            base.CloseForms();
            log.LogMethodExit();
        }
        private void ProceedActionImpl(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry("kioskTransaction");
            try
            {
                using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                {
                    DialogResult dr = frpm.ShowDialog();
                    kioskTransaction = frpm.GetKioskTransaction;
                    if (dr != System.Windows.Forms.DialogResult.No) // back button pressed
                    {
                        DialogResult = dr;
                        this.Close();
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Show();
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                txtMessage.Text = ex.Message;
                this.Close();
            }
            log.LogMethodExit();
        }

    }
}
