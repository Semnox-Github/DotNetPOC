/********************************************************************************************
* Project Name - Parafait_Kiosk -frmUpsellProduct.cs
* Description  - frmUpsellProduct 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
*2.130.0       09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.0.0     21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
*2.150.1       22-Feb-2023      Guru S A           Kiosk Cart Enhancements
*2.150.6       10-Nov-2023      Sathyavathi        Customer Lookup Enhancements
 ********************************************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.POS;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmUpsellProduct : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        private DataRow selectedProduct;
        private DataRow offerProduct;
        private string _Function;
        private string selecteEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        private ExecutionContext executionContext;

        public frmUpsellProduct(KioskTransaction kioskTransaction, DataRow SelectedProduct, DataRow OfferProduct, string Function, string entitlmentType)
        {
            log.LogMethodEntry(SelectedProduct, OfferProduct, Function, entitlmentType);
            selecteEntitlementType = entitlmentType;
            selectedProduct = SelectedProduct;
            _Function = Function;
            offerProduct = OfferProduct;
            this.kioskTransaction = kioskTransaction;
            this.executionContext = KioskStatic.Utilities.ExecutionContext;
            InitializeComponent();
            try
            {
                InitializeForm();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmUpsellProduct()" + ex.Message);
            }
            displayMessageLine(offerProduct["OfferMessage"].ToString());
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);
            // lblTimeOut.Text = "20";
            lblTimeOut.Text = GetKioskTimerTickValue().ToString();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            //this.ShowInTaskbar = false;
            KioskStatic.setDefaultFont(this);
            KioskStatic.Utilities.setLanguage(this);
            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].Visible = false;
            }
            log.LogMethodExit();
        }

        private void frmNewcard_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Upsell Exiting...");
            Audio.Stop();
            log.LogMethodExit();
        }

        void InitializeForm()
        {
            log.LogMethodEntry();
            SetTheme();
            string amountFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, 417, 
                Convert.ToDouble(selectedProduct["Price"]).ToString(amountFormat) + " (" + selectedProduct["product_name"].ToString() + ")");

            string desc = offerProduct["description"].ToString();
            if (desc.Trim() == "")
                desc = offerProduct["product_name"].ToString();

            lblDesc1.Text = lblDesc2.Text = lblDesc3.Text = "";

            string[] lines = desc.Split('\r', '\n');
            lblDesc1.Text = lines[0];
            if (lines.Length > 1)
                lblDesc2.Text = lines[1];
            if (lines.Length > 2)
                lblDesc3.Text = lines[2];
            log.LogMethodExit();
        }
        private void SetTheme()
        {
            log.LogMethodEntry();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.UpsellBackgroundImage);
                pbOfferLogo.Image = ThemeManager.CurrentThemeImages.SpecialOfferLogo;
                pbOfferLogo.SizeMode = PictureBoxSizeMode.CenterImage;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnNo.BackgroundImage = ThemeManager.CurrentThemeImages.NoThanksButton;
                lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
                btnYes.BackgroundImage = ThemeManager.CurrentThemeImages.SureWhyNotButton;
                if (System.IO.File.Exists(Application.StartupPath + @"\Media\Images\UpsellOfferImage.png"))
                {
                    pbOfferLogo.Image = Image.FromFile(Application.StartupPath + @"\Media\Images\UpsellOfferImage.png");
                    pbOfferLogo.SizeMode = PictureBoxSizeMode.CenterImage;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred in SetTheme()", ex);
            }
            log.LogMethodExit();
        }

        private void frmRedeemTokens_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;
            Application.DoEvents();
            KioskStatic.logToFile("Enter Up sell screen");
            for (int i = 0; i < this.Controls.Count; i++)//12-06-2015:Starts
            {
                if (!(this.Controls[i].Name.Equals("btnPrev")
                    || this.Controls[i].Name.Equals("btnCancel")
                    || this.Controls[i].Name.Equals("btnCart")
                    || this.Controls[i].Name.Equals("btnHome")))
                {
                    this.Controls[i].Visible = true;
                }
            }
            lblSiteName.Visible = false;
            panelUpsell.Left = (this.Width - panelUpsell.Width) / 2; 
            SetCustomizedFontColors();
            log.LogMethodExit();
        } 
        private void displayMessageLine(string message)
        {
            log.LogMethodEntry(message); 
            txtMessage.Text = message;
            log.LogMethodExit();
        } 
        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            setKioskTimerSecondsValue(tickSecondsRemaining - 1);
            if (tickSecondsRemaining > 0)
            {
                lblTimeOut.Text = tickSecondsRemaining.ToString("#0");
            }

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
                StopKioskTimer();
                KioskStatic.logToFile("Exit Timer ticked. Calling transaction with upsell = false");
                CallTransaction(false);
            }
            log.LogMethodExit();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Audio.Stop();
            KioskStatic.logToFile("upsell = No pressed");
            Application.DoEvents();
            CallTransaction(false);
            log.LogMethodExit();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Audio.Stop();
            KioskStatic.logToFile("upsell = Yes pressed");
            Application.DoEvents();
            CallTransaction(true);
            log.LogMethodExit();
        }

        private void frmUpsellProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            //exitTimer.Stop();
        }

        private void btnNo_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void btnNo_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void btnYes_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void btnYes_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void CallTransaction(bool upSell)
        {
            log.LogMethodEntry(upSell);
            //totSecs = 20;
            //exitTimer.Stop();
            ResetKioskTimer();
            StopKioskTimer(); 
            DataTable dt;
            if (upSell)
            {
                dt = KioskStatic.getProductDetails(Convert.ToInt32(offerProduct["product_id"]));
            }
            else
                dt = KioskStatic.getProductDetails(Convert.ToInt32(selectedProduct["product_id"]));

            bool isRegisteredCustomerOnly = (dt.Rows[0]["RegisteredCustomerOnly"].ToString() == "Y") ? true : false;
            if (_Function == KioskTransaction.GETNEWCARDTYPE || _Function == KioskTransaction.GETFNBTYPE)
            {
                bool showMsgRecommendCustomerToRegister = true;
                bool isLinked = CustomerStatic.LinkCustomerToTheTransaction(kioskTransaction, executionContext, isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister);
                if (!CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(), isRegisteredCustomerOnly, isLinked))
                {
                    log.LogMethodExit();
                    return;
                }
            }
            if (_Function == KioskTransaction.GETNEWCARDTYPE)
            { 
                int CardCount = Convert.ToInt32(dt.Rows[0]["CardCount"]);

                if (CardCount <= 1)
                    CardCount = 0;

                if (dt.Rows[0]["QuantityPrompt"].ToString().Equals("Y"))
                {                    
                    using (frmCardCount frm = new frmCardCount(kioskTransaction, dt.Rows[0], selecteEntitlementType, CardCount))
                    {
                        DialogResult dr = frm.ShowDialog();
                        if (dr == System.Windows.Forms.DialogResult.No)
                        {
                            kioskTransaction = frm.GetKioskTransaction;
                            //exitTimer.Start();
                            StartKioskTimer();
                            this.Close();
                            log.LogMethodExit();
                            return;
                        }
                        kioskTransaction = frm.GetKioskTransaction;
                    }
                }
                else
                {
                    int productId = Convert.ToInt32(dt.Rows[0]["product_id"]);
                    kioskTransaction = KioskHelper.AddNewCardProduct(kioskTransaction, dt.Rows[0], 1, selecteEntitlementType);
                    frmChooseProduct.AlertUser(productId, kioskTransaction, ProceedActionImpl);
                    if (kioskTransaction.ShowCartInKiosk == false)
                    {
                        kioskTransaction.SelectedProductType = KioskTransaction.GETNEWCARDTYPE;
                        ProceedActionImpl(kioskTransaction);
                    }
                    else
                    {
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842);
                        KioskStatic.logToFile("frmUpsellProducts: " + msg);
                        displayMessageLine(msg);
                        //frmOKMsg.ShowUserMessage(msg);
                    }
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
                Dispose();
            }
            else if (_Function.Equals(KioskTransaction.GETFNBTYPE))
            {
                int quantity = 1;
                if (dt.Rows[0]["QuantityPrompt"].ToString().Equals("Y"))
                {
                    //Enter Product Quantity
                    double varQuantity = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(executionContext, 479), '-', KioskStatic.Utilities);
                    if (Int32.TryParse(varQuantity.ToString(), out quantity) == false)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 2360);
                        //Please enter valid quantity
                        KioskStatic.logToFile(msg);
                        log.LogMethodExit(msg);
                        this.Close();
                        txtMessage.Text = msg;
                        return;
                    }

                    KioskStatic.logToFile("Selected quantity is :" + quantity.ToString());
                }
                double prodPrice = -1;
                int productId = Convert.ToInt32(dt.Rows[0]["product_id"]);
                kioskTransaction.AddManualProduct(productId, prodPrice, quantity);
                frmChooseProduct.AlertUser(productId, kioskTransaction, ProceedActionImpl);
                if (kioskTransaction.ShowCartInKiosk == false)
                {
                    kioskTransaction.SelectedProductType = KioskTransaction.GETFNBTYPE;
                    ProceedActionImpl(kioskTransaction);
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842);
                    KioskStatic.logToFile("frmUpsellProducts: " + msg);
                    displayMessageLine(msg);
                    //frmOKMsg.ShowUserMessage(msg);
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
                Dispose();
            }
            else
            {
                Card card = null;
                try
                {
                    using (frmTapCard ftc = new frmTapCard(kioskTransaction, isRegisteredCustomerOnly, true))
                    {
                        DialogResult dr = ftc.ShowDialog();
                        if (dr == DialogResult.Cancel)
                        {
                            string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Timeout");
                            throw new CustomerStatic.TimeoutOccurred(msg);
                        }
                        kioskTransaction = ftc.GetKioskTransaction;
                        card = ftc.Card;
                        ftc.Dispose();
                    }
                }
                catch (CustomerStatic.TimeoutOccurred ex)
                {
                    log.Error(ex);
                    PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                    this.DialogResult = DialogResult.Cancel;
                    log.LogMethodExit();
                    return;
                }
                if (card != null)
                {
                    KioskStatic.logToFile("Card: " + card.CardNumber); 
                    if (card.technician_card.Equals('Y'))
                    {
                        displayMessageLine(MessageContainerList.GetMessage(executionContext,197, card.CardNumber));
                        KioskStatic.logToFile("Technician Card not allowed: " + card.CardNumber);
                        frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(executionContext, 197, card.CardNumber));
                        //exitTimer.Start();
                        this.Close();
                        StartKioskTimer();
                        log.LogMethodExit();
                        return;
                    }

                    if (isRegisteredCustomerOnly == true && kioskTransaction.HasCustomerRecord() == false)
                    {
                        log.LogMethodExit();
                        return;
                    }

                    int productId = Convert.ToInt32(dt.Rows[0]["product_id"]);
                    kioskTransaction = KioskHelper.AddRechargeCardProduct(kioskTransaction, card, dt.Rows[0], 1, selecteEntitlementType);
                    frmChooseProduct.AlertUser(productId, kioskTransaction, ProceedActionImpl);
                    if (kioskTransaction.ShowCartInKiosk == false)
                    {
                        kioskTransaction.SelectedProductType = KioskTransaction.GETRECHAREGETYPE;
                        ProceedActionImpl(kioskTransaction);
                    }
                    else
                    {
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842);
                        KioskStatic.logToFile("frmUpsellProducts: " + msg);
                        displayMessageLine(msg);
                        //frmOKMsg.ShowUserMessage(msg);
                    }
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                    Dispose();
                }
                else
                {
                    KioskStatic.logToFile("Card not tapped");
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    Close();
                }
            }
            log.LogMethodExit();
        }

        private void ProceedActionImpl(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry("kioskTransaction");
            try
            {
                using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                {
                    if (frpm.ShowDialog() == System.Windows.Forms.DialogResult.No) // back button pressed
                    {
                        kioskTransaction = frpm.GetKioskTransaction;
                        //exitTimer.Start();
                        StartKioskTimer();
                        this.Close();
                        log.LogMethodExit();
                        return;
                    }
                    kioskTransaction = frpm.GetKioskTransaction;
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
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmUpsell");
            try
            {
                this.lblTimeOut.ForeColor = KioskStatic.CurrentTheme.UpsellProductScreenTimeOutTextForeColor;
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.UpsellProductScreenHeader1TextForeColor;
                this.lblOffer.ForeColor = KioskStatic.CurrentTheme.UpsellProductScreenHeader2TextForeColor;
                this.lblDesc1.ForeColor = KioskStatic.CurrentTheme.UpsellProductScreenDesc1TextForeColor;
                this.lblDesc2.ForeColor = KioskStatic.CurrentTheme.UpsellProductScreenDesc2TextForeColor;
                this.lblDesc3.ForeColor = KioskStatic.CurrentTheme.UpsellProductScreenDesc3TextForeColor;
                this.btnYes.ForeColor = KioskStatic.CurrentTheme.UpsellProductScreenBtnYesTextForeColor;
                this.btnNo.ForeColor = KioskStatic.CurrentTheme.UpsellProductScreenBtnNoTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.UpsellProductScreenFooterTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.UpsellProductScreenBtnHomeTextForeColor;
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;

            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmUpsell: " + ex.Message);
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
    }
}