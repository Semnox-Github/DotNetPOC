/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - Attractions Quantity Screen
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.155.0.0   16-Jun-2023      Sathyavathi        Created for Attraction Sale in Kiosk
 *2.152.0.0   12-Dec-2023      Suraj Pai          Modified for Attraction Sale in TableTop Kiosk
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
using Semnox.Core.GenericUtilities;

namespace Parafait_Kiosk
{
    public partial class frmAttractionQty : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        private ProductsContainerDTO selectedProductsContainerDTO;
        private const int MIN_QUANTITY = 1;
        private const int MAX_QUANTITY = 999;
        private int selectedProductId;
        private int minQty;
        private int maxQty;
        private int qtySelected;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmAttractionQty(KioskTransaction kioskTransaction, int productId)
        {
            log.LogMethodEntry("kioskTransaction", productId);
            KioskStatic.logToFile("In frmAttractionQty()");
            this.kioskTransaction = kioskTransaction;
            this.selectedProductId = productId;
            try
            {
                this.selectedProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, productId);
                if (selectedProductsContainerDTO == null)
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
                KioskStatic.logToFile(ex.Message);
            }
            InitializeComponent();
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
            qtySelected = selectedProductsContainerDTO.MinimumQuantity;
            SetOnscreenMessages();
            if(selectedProductsContainerDTO.ProductType == ProductTypeValues.COMBO)
            {
                pnlComboDetails.Visible = true;
            }
            else
            {
                pnlComboDetails.Visible = lblComboDetails.Visible = false;
                pnlQuantity.Location = new Point(710, 235);
            }
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmAttractionQty_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                minQty = GetMinQuantity(selectedProductsContainerDTO);
                maxQty = GetMaxQuantity(selectedProductsContainerDTO);
                qtySelected = minQty;
                UpdateQtyInfoOnScreen(qtySelected);

                if (selectedProductsContainerDTO.ProductType == ProductTypeValues.COMBO)
                {
                    LoadComboDetails(); 
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in frmAttractionQty_Load(): " + ex.Message);
            }
            finally
            {
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private void LoadComboDetails()
        {
            log.LogMethodEntry();
            StopKioskTimer();
            this.flpComboProducts.Controls.Clear();
            try
            {
                if (selectedProductsContainerDTO.ComboProductContainerDTOList != null && selectedProductsContainerDTO.ComboProductContainerDTOList.Any())
                {
                    int i = 1;
                    foreach (ComboProductContainerDTO comboItem in selectedProductsContainerDTO.ComboProductContainerDTOList)
                    {
                        //Create user ctrl element
                        string productName = KioskHelper.GetProductName(comboItem.ChildProductId);
                        string childProductInfo = comboItem.Quantity + " x " + productName;
                        UsrCtrlComboDetails usrCtrlComboDetails = new UsrCtrlComboDetails(KioskStatic.Utilities.ExecutionContext, childProductInfo);
                        this.flpComboProducts.Controls.Add(usrCtrlComboDetails);
                        usrCtrlComboDetails.ComboChildIndex = i.ToString();
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in initializeProductTab() of frmComboDetails: " + ex.Message);
            }
            finally
            {
                ResetKioskTimer();
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                DisableButtons();
                KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
                base.btnHome_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.Error("Error in btnCancel_Click", ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DisableButtons();
            ResetKioskTimer();
            StopKioskTimer();
            try
            {
                KioskAttractionDTO kioskAttractionDTO = new KioskAttractionDTO(selectedProductId, qtySelected);
                using (frmProcessingAttractions frm = new frmProcessingAttractions(kioskTransaction, kioskAttractionDTO, 0))
                {
                    DialogResult dr = frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                    kioskAttractionDTO = frm.GetKioskAttractionDTO;
                    if (dr == System.Windows.Forms.DialogResult.No) //back button pressed
                    {
                        kioskTransaction.ClearTemporarySlots(kioskAttractionDTO);
                    }
                    else
                    {
                        this.DialogResult = dr;
                        this.Close();
                    }
                } 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("btnProceed_Click() in frmAttractionQty : " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                StartKioskTimer();
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                ResetKioskTimer();
                this.DialogResult = DialogResult.No;
                Close();
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void SetOnscreenMessages()
        {
            log.LogMethodEntry();
            try
            {
                UpdateQtyInfoOnScreen(qtySelected);
                txtQty.Text = qtySelected.ToString();
                lblBuyText.Text = (selectedProductsContainerDTO.ProductType == ProductTypeValues.COMBO) ? 
                    MessageContainerList.GetMessage(executionContext, "Buying the package")//Buying the package
                    : MessageContainerList.GetMessage(executionContext, "Booking tickets for"); //Booking tickets for
                string productName = KioskHelper.GetProductName(selectedProductsContainerDTO.ProductId);
                lblProductName.Text = productName;
                lblProductDescription.Text = selectedProductsContainerDTO.Description;
                lblHowManyMsg.Text = MessageContainerList.GetMessage(executionContext, "How many?"); //How many?
                lblEnterQtyMsg.Text = txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Enter the quantity"); //Enter the quantity

                lblComboDetails.Text = MessageContainerList.GetMessage(executionContext, 5157, productName); //'1 quantity of &1 includes:' 
                btnProceed.Text = MessageContainerList.GetMessage(executionContext, "Start Booking");
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Unexpected Error in SetOnScreenMessages" + ex.Message);
                log.Error("Error in SetOnScreenMessages", ex);
            }
            log.LogMethodExit();
        }

        private int GetMinQuantity(ProductsContainerDTO prodsContainerDTO)
        {
            log.LogMethodEntry(prodsContainerDTO);
            ResetKioskTimer();
            int minQuantity = MIN_QUANTITY;
            try
            {
                if (prodsContainerDTO.MinimumQuantity > 1)
                {
                    minQuantity = prodsContainerDTO.MinimumQuantity;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error getting minimum quantity in frmAttractionQty screen: " + ex);
            }
            log.LogMethodExit(minQuantity);
            return minQuantity;
        }

        private int GetMaxQuantity(ProductsContainerDTO prodsContainerDTO)
        {
            log.LogMethodEntry(prodsContainerDTO);
            ResetKioskTimer();
            int maxQuantity = MAX_QUANTITY;
            try
            {
                if (prodsContainerDTO.MaximumQuantity != null && prodsContainerDTO.MaximumQuantity > 0)
                {
                    maxQuantity = Convert.ToInt32(prodsContainerDTO.MaximumQuantity);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error getting maximum quantity in frmAttractionQty screen: " + ex);
            }

            log.LogMethodExit(maxQuantity);
            return maxQuantity;
        }

        private void pcbDecreaseQty_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                ResetKioskTimer();
                if (txtQty.Enabled)
                {
                    Focus();
                    qtySelected--;

                    if (qtySelected < minQty)
                    {
                        qtySelected = minQty;
                        frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4345, minQty)); //For this product, you must select a minimum of &1 quantity
                    }
                    UpdateQtyInfoOnScreen(qtySelected);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in pcbDecreaseQty_Click() of frmAttractionQty" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void pcbIncreaseQty_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                ResetKioskTimer();
                if (txtQty.Enabled)
                {
                    Focus();
                    qtySelected++;

                    //maxQuantity may come as 0 when not defined.
                    if ((maxQty > 0) && (qtySelected > maxQty))
                    {
                        qtySelected = Convert.ToInt32(maxQty);
                        frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4344, maxQty));//For this product, you can choose no more than &1 quantities
                    }
                    UpdateQtyInfoOnScreen(qtySelected);
                }
                    
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in pcbIncreaseQty_Click() of frmAttractionQty" + ex.Message);
            }

            log.LogMethodExit();
        }

        private void txtQty_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            if (selectedProductsContainerDTO.QuantityPrompt.Equals("Y"))
            {
                txtQty.Text = qtySelected.ToString();
                double newQty = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm("Enter Quantity", qtySelected.ToString(), KioskStatic.Utilities);

                if (newQty >= minQty && newQty <= maxQty)
                {
                    qtySelected = Convert.ToInt32(newQty);
                    UpdateQtyInfoOnScreen(qtySelected);
                }
                else if (newQty != -1)
                {
                    frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(executionContext, 4136, minQty, maxQty).ToString()); //'Quantity entered must be between &1 and &2'
                }
            }
            log.LogMethodExit();
        }

        private void UpdateQtyInfoOnScreen(int value)
        {
            log.LogMethodEntry();
            txtQty.Text = value.ToString();
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }

            if (tickSecondsRemaining <= 0)
            {
                Application.DoEvents();
                base.CloseForms();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void frmAttractionQty_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmAttractionQty_FormClosed()", ex);
            }

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting customized background images for frmAttractionQty Screen");
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.AttractionQtyBackgroundImage); //background image
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.pnlComboDetails.BackgroundImage = ThemeManager.CurrentThemeImages.PanelComboDetails;
                btnPrev.BackgroundImage = btnCancel.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
                this.pcbDecreaseQty.BackgroundImage = selectedProductsContainerDTO.QuantityPrompt.Equals("Y") ?
                ThemeManager.CurrentThemeImages.DecreaseQtyButton : ThemeManager.CurrentThemeImages.DecreaseQtyDisabledButton;
                this.pcbIncreaseQty.BackgroundImage = selectedProductsContainerDTO.QuantityPrompt.Equals("Y") ?
                    ThemeManager.CurrentThemeImages.IncreaseQtyButton : ThemeManager.CurrentThemeImages.IncreaseQtyDisabledButton;
                this.pnlQtyTxtBox.BackgroundImage = ThemeManager.CurrentThemeImages.QuantityTextBox;
                this.pbQuantity.BackgroundImage = ThemeManager.CurrentThemeImages.QuantityImage;
                if (selectedProductsContainerDTO.QuantityPrompt.Equals("N"))
                    txtQty.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized background images for frmAttractionQty", ex);
                KioskStatic.logToFile("Error while setting customized background images for frmAttractionQty: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.AttractionQtyHomeButtonTextForeColor;//home button
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.AttractionQtyBackButtonTextForeColor;//Back button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.AttractionQtyCancelButtonTextForeColor;//Back button
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.AttractionQtyProceedButtonTextForeColor;//Proceed button
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.AttractionQtyFooterTextForeColor; //footer message 
                this.txtQty.ForeColor = KioskStatic.CurrentTheme.AttractionQtyTxtQtyTextForeColor;
                this.lblHowManyMsg.ForeColor = KioskStatic.CurrentTheme.AttractionQtyLblHowManyMsgTextForeColor;
                this.lblEnterQtyMsg.ForeColor = KioskStatic.CurrentTheme.AttractionQtyLblEnterQtyMsgTextForeColor;
                this.lblBuyText.ForeColor = KioskStatic.CurrentTheme.AttractionQtyLblBuyTextTextForeColor;
                this.lblProductName.ForeColor = KioskStatic.CurrentTheme.AttractionQtyLblProductNameTextForeColor;
                this.lblComboDetails.ForeColor = KioskStatic.CurrentTheme.AttractionQtyLblComboDetailsTextForeColor;
                this.lblProductDescription.ForeColor = KioskStatic.CurrentTheme.AttractionQtyLblProductDescriptionTextForeColor;
                this.bigVerticalScrollView.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors for frmAttractionQty", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements of frmAttractionQty: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            this.btnProceed.Enabled = false;
            this.btnPrev.Enabled = false;
            this.btnCancel.Enabled = false;
            this.txtQty.Enabled = false;
            this.pcbDecreaseQty.Enabled = false;
            this.pcbIncreaseQty.Enabled = false;
            log.LogMethodExit();
        }

        private void EnableButtons()
        {
            log.LogMethodEntry();
            this.btnProceed.Enabled = true;
            this.btnPrev.Enabled = true;
            this.btnCancel.Enabled = true;
            if(selectedProductsContainerDTO.QuantityPrompt.Equals("Y"))
            {
                this.txtQty.Enabled = true;
                this.pcbDecreaseQty.Enabled = true;
                this.pcbIncreaseQty.Enabled = true;
            }
            log.LogMethodExit();
        }
    }
}
