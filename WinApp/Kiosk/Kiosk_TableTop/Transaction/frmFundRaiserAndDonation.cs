/********************************************************************************************
 * Project Name - Parafait_Kiosk  
 * Description  - frmDonationAndFundRaiser.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmFundRaiserAndDonation : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool isFundraiserEvent = false;
        private double userDonatedPrice;
        private List<ProductsDTO> activeFundsAndDonationsProductsDTOList;
        private Semnox.Core.Utilities.KeyPads.Kiosk.frmNumberPad numPad = null;
        private Panel NumberPadVarPanel;
        private ProductsDTO userSelectedDonationProduct;
        public List<ProductsDTO> selectedProductsDTOList = new List<ProductsDTO>();
        ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;

        public frmFundRaiserAndDonation(List<ProductsDTO> fundsAndDonationsDTOList, bool isFundRaiser)
        {
            log.LogMethodEntry(fundsAndDonationsDTOList, isFundRaiser);
            KioskStatic.logToFile("frmFundRaiserAndDonation()");
            activeFundsAndDonationsProductsDTOList = fundsAndDonationsDTOList;
            isFundraiserEvent = isFundRaiser;
            InitializeComponent();
            SetCustomImages();
            KioskStatic.setDefaultFont(this);
            InitializeProductTab(activeFundsAndDonationsProductsDTOList);
            if (isFundRaiser == true)
            {
                this.lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, 4137); //Are you here for Fund Raiser event?
            }
            else
            {
                this.lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, 4138); //Would you like to Donate for a cause?
            }
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 4139); //"Thank you for your generosity"
            //this.ShowInTaskbar = false;
            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.FundRaiserBackgroundImage);
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnOK.BackgroundImage = btnNoThanks.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetCustomizedFontColors();
            SetKioskTimerTickValue(20);
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmFundRaiserAndDonation_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            btnNoThanks.Visible = true;
            log.LogMethodExit();
        }
        public void InitializeProductTab(List<ProductsDTO> productsDTOList)
        {
            log.LogMethodEntry(productsDTOList);
            KioskStatic.logToFile("initializeProductTab(): " + productsDTOList);
            tlpFundDonationProducts.Controls.Clear();
            try
            {
                if (this.pbFundDonationLogo.Image != null)
                {
                    tlpFundDonationProducts.Controls.Add(pbFundDonationLogo);
                }
                foreach (ProductsDTO productDTO in productsDTOList)
                {
                    ResetKioskTimer();
                    usrControlFundsDonationsProduct usrCntrolProduct = CreateUsrCtlElement(executionContext, productDTO, isFundraiserEvent);
                    usrCntrolProduct.Width = tlpFundDonationProducts.Width - 2;
                    tlpFundDonationProducts.Controls.Add(usrCntrolProduct);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in initializeProductTab() of frmFundRaiserAndDonation : " + ex.Message);
            }

            tlpFundDonationProducts.Refresh();
            KioskStatic.logToFile("exit initializeProductTab()");
            ResetKioskTimer();
            log.LogMethodExit();
        }
        public void SelctedProduct(int productId)
        {
            log.LogMethodEntry(productId);
            try
            {
                ResetKioskTimer();
                //foreach (Control panelControl in tlpFundDonationProducts.Controls)
                //{
                //    if (panelControl is usrControlFundsDonationsProduct)
                //    {
                //        usrControlFundsDonationsProduct usrControl = (usrControlFundsDonationsProduct)panelControl;
                //        if (usrControl.GetProductId != productId)
                //        {
                //            usrControl.IsSelected = false;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        public override void Form_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            StartKioskTimer();
            log.LogMethodExit();
        }
        public override void Form_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in frmDonationAndFundRaiser_Deactivate : " + ex.Message);
            }
            log.LogMethodExit();
        }
        public void btnNoThanks_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                KioskStatic.logToFile("btnNoThanks clicked");
                DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in btnCancel_Click : " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                KioskStatic.logToFile("btnOK Clicked");
                selectedProductsDTOList = GetUserSelectedProducts();
                string numberFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");

                if (selectedProductsDTOList != null && selectedProductsDTOList.Any())
                {
                    if (isFundraiserEvent)
                    {
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        KioskStatic.logToFile("exit btnOK_Click() from Funds UI");
                        Close();
                    }
                    else
                    {
                        if (selectedProductsDTOList.Exists(p => p.AllowPriceOverride.Equals("Y")))
                        {
                            for (int i = 0; i < selectedProductsDTOList.Count; i++)
                            {
                                if (selectedProductsDTOList[i].AllowPriceOverride.Equals("Y"))
                                {
                                    using (frmYesNo frmYN = new frmYesNo(MessageContainerList.GetMessage(executionContext, 4128,
                                                                    KioskStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL, selectedProductsDTOList[i].Price.ToString(numberFormat)))) //"Do you wish to donate more than &1&2"
                                    {
                                        if (frmYN.ShowDialog() != System.Windows.Forms.DialogResult.No)
                                        {
                                            btnOK.Enabled = false;
                                            btnNoThanks.Enabled = false;
                                            ShowKeyPad(selectedProductsDTOList[i]);
                                            log.LogMethodExit();
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        ResetKioskTimer();
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        KioskStatic.logToFile("exit btnOK_Click() from Donations UI");
                        Close();
                    }
                }
                else
                {
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2438);//"Please choose an option";

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error occured while excecuting btnOK_Click() :" + ex.Message);
            }
            log.LogMethodExit();
        }

        private List<ProductsDTO> GetUserSelectedProducts()
        {
            log.LogMethodEntry();

            selectedProductsDTOList = new List<ProductsDTO>();
            try
            {
                ResetKioskTimer();
                if (activeFundsAndDonationsProductsDTOList != null && activeFundsAndDonationsProductsDTOList.Any())
                {
                    if (activeFundsAndDonationsProductsDTOList.Count == 1)
                    {
                        selectedProductsDTOList.Add(activeFundsAndDonationsProductsDTOList[0]);
                    }
                    else
                    {
                        foreach (Control panelControl in tlpFundDonationProducts.Controls)
                        {
                            if (panelControl is usrControlFundsDonationsProduct)
                            {
                                usrControlFundsDonationsProduct usrControl = (usrControlFundsDonationsProduct)panelControl;
                                if (usrControl.IsSelected)
                                {
                                    ProductsDTO productsDTO = activeFundsAndDonationsProductsDTOList.Find(p => p.ProductId == usrControl.GetProductId);
                                    if (productsDTO != null)
                                    {
                                        selectedProductsDTOList.Add(productsDTO);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error occured while excecuting GetUserSelectedProducts() :" + ex.Message);
            }
            log.LogMethodExit(selectedProductsDTOList);
            return selectedProductsDTOList;
        }
        
        private void ShowKeyPad(ProductsDTO productsDTO)
        {
            log.LogMethodEntry();
            userSelectedDonationProduct = productsDTO;
            if (numPad == null)
            {
                numPad = new Semnox.Core.Utilities.KeyPads.Kiosk.frmNumberPad();
                numPad.Init(KioskStatic.KIOSK_CARD_VALUE_FORMAT, 0, Convert.ToInt32(productsDTO.Price));
                NumberPadVarPanel = numPad.NumPadPanel();
                NumberPadVarPanel.Controls["btnClose"].Visible = false;
                NumberPadVarPanel.Controls["btnCloseX"].Visible = false;
                NumberPadVarPanel.Location = new Point((this.Width - NumberPadVarPanel.Width) / 2, (this.Height - NumberPadVarPanel.Height) / 2);
                this.Controls.Add(NumberPadVarPanel);
                numPad.setReceiveAction = EventnumPadOKReceived;
                this.KeyPreview = true;
                this.KeyPress += new KeyPressEventHandler(FormNumPad_KeyPress);
            }

            numPad.NewEntry = true;
            NumberPadVarPanel.Visible = true;
            NumberPadVarPanel.BringToFront();
            log.LogMethodExit();
        }

        void FormNumPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyChar == (char)Keys.Escape)
                NumberPadVarPanel.Visible = false;
            else
            {
                numPad.GetKey(e.KeyChar);
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private void EventnumPadOKReceived()
        {
            log.LogMethodEntry();
            double n = numPad.ReturnNumber;

            HandleUserDonatedPrice(userSelectedDonationProduct, n);
            log.LogMethodExit();
        }

        void HandleUserDonatedPrice(ProductsDTO productsDTO, double newPrice)
        {
            log.LogMethodEntry();

            try
            {
                ResetKioskTimer();
                if (string.IsNullOrWhiteSpace(newPrice.ToString()) == false)
                {
                    userDonatedPrice = Convert.ToDouble(newPrice);
                }

                if (userDonatedPrice < (double)productsDTO.Price)
                {
                    //Sorry, override amount cannot be less than product price
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext,
                                                4129, KioskStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL
                                                    , productsDTO.Price.ToString());
                }
                else
                {
                    if (userDonatedPrice > (double)productsDTO.Price)
                    {
                        for (int i = 0; i < selectedProductsDTOList.Count; i++)
                        {
                            if (selectedProductsDTOList[i].ProductId == productsDTO.ProductId)
                            {
                                selectedProductsDTOList[i].Price = (decimal)userDonatedPrice;
                                break;
                            }
                        }
                    }
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    KioskStatic.logToFile("Donation price override is performed: " + userDonatedPrice.ToString());
                    log.Info("Donation price override is performed: " + userDonatedPrice.ToString());
                    Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("error while overriding donation amount: " + ex.Message);
            }
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

        void scrollDown(int value = 10)
        {
            log.LogMethodEntry(value);
            try
            {
                ResetKioskTimer();
                if (tlpFundDonationProducts.Top + tlpFundDonationProducts.Height > 3)
                {
                    tlpFundDonationProducts.Top = tlpFundDonationProducts.Top - value;
                }
            }
            catch { }
            log.LogMethodExit();
        }

        void scrollUp(int value = 10)
        {
            log.LogMethodEntry(value);
            try
            {
                ResetKioskTimer();
                if (tlpFundDonationProducts.Top < 0)
                {
                    tlpFundDonationProducts.Top = Math.Min(0, tlpFundDonationProducts.Top + value);
                }
            }
            catch { }
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
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        private void frmDonationAndFundRaiser_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        private usrControlFundsDonationsProduct CreateUsrCtlElement(ExecutionContext executionContext, ProductsDTO productsDTO, bool isFund)
        {
            log.LogMethodEntry(productsDTO, isFund);
            ResetKioskTimer();
            usrControlFundsDonationsProduct usrCntrolProduct = new usrControlFundsDonationsProduct(executionContext, productsDTO, isFund, tlpFundDonationProducts.Width);
            usrCntrolProduct.selctedProductDelegate += new usrControlFundsDonationsProduct.SelctedProductDelegate(SelctedProduct);
            log.LogMethodExit();
            return usrCntrolProduct;
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting customized background images");
            try
            {
                tlpFundDonationProducts.SuspendLayout();
                this.pbFundDonationLogo.Image = null;
                if (isFundraiserEvent)
                {
                    if (ThemeManager.CurrentThemeImages.FundRaiserPictureBoxLogo != null)
                    {
                        this.pbFundDonationLogo.Image = ThemeManager.CurrentThemeImages.FundRaiserPictureBoxLogo; //fundraiser logo
                        this.pbFundDonationLogo.SizeMode = PictureBoxSizeMode.Zoom;
                        this.pbFundDonationLogo.Height = (this.pbFundDonationLogo.Height > ThemeManager.CurrentThemeImages.FundRaiserPictureBoxLogo.Height
                                                          ? ThemeManager.CurrentThemeImages.FundRaiserPictureBoxLogo.Height : this.pbFundDonationLogo.Height);
                    }
                }
                else
                {
                    if (ThemeManager.CurrentThemeImages.DonationsPictureBoxLogo != null)
                    {
                        this.pbFundDonationLogo.Image = ThemeManager.CurrentThemeImages.DonationsPictureBoxLogo; //donation logo
                        this.pbFundDonationLogo.SizeMode = PictureBoxSizeMode.Zoom;
                        this.pbFundDonationLogo.Height = (this.pbFundDonationLogo.Height > ThemeManager.CurrentThemeImages.DonationsPictureBoxLogo.Height
                                                          ? ThemeManager.CurrentThemeImages.DonationsPictureBoxLogo.Height : this.pbFundDonationLogo.Height);
                    }
                }
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.FundRaiserBackgroundImage); //background image

            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized background images", ex);
                KioskStatic.logToFile("Error while setting customized background images: " + ex.Message);
            }
            tlpFundDonationProducts.ResumeLayout(true);
            log.LogMethodExit();
        }

        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in Fund Raiser and Donations");
            try
            {
                tlpFundDonationProducts.SuspendLayout();
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.FundsDonationsGreetingTextForeColor;//Funds/Donation Greeting text ForeColor
                this.btnOK.ForeColor = KioskStatic.CurrentTheme.FundsDonationsBtnTextForeColor;//OK button
                this.btnNoThanks.ForeColor = KioskStatic.CurrentTheme.FundsDonationsBtnTextForeColor;//Cancel button
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FundsDonationsFooterTextForeColor; //footer message 
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.PaymentModeBtnHomeTextForeColor;//Total to pay info label
                this.bigVerticalScrollFundDonationProducts.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in Fund Raiser and Donations: " + ex.Message);
            }
            tlpFundDonationProducts.ResumeLayout(true);
            log.LogMethodExit();
        }
    }
}
