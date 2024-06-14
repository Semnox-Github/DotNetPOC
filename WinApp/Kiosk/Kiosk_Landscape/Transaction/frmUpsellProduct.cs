/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmUpsellProduct.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        5-Sep-2019       Deeksha            Added logger methods.
*2.150.1     22-Feb-2023       Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;

namespace Parafait_Kiosk
{
    public partial class frmUpsellProduct : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;

        DataRow selectedProduct, offerProduct;
        string selecteEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        string _Function;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        public frmUpsellProduct(KioskTransaction kioskTransaction, DataRow SelectedProduct, DataRow OfferProduct, string Function, string entitlmentType)
        {
            log.LogMethodEntry(SelectedProduct, OfferProduct, Function, entitlmentType);
            selectedProduct = SelectedProduct;
            _Function = Function;
            offerProduct = OfferProduct;
            selecteEntitlementType = entitlmentType;
            this.kioskTransaction = kioskTransaction;


            Utilities.setLanguage();
            InitializeComponent();
            //exitTimer.Enabled = false;



            Utilities.setLanguage(this);
            KioskStatic.setDefaultFont(this);

            displayMessageLine(offerProduct["OfferMessage"].ToString(), MESSAGE);
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);
            //lblTimeOut.Text = "20";
            lblTimeOut.Text = GetKioskTimerTickValue().ToString();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            this.ShowInTaskbar = false;
            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnHome.Size = btnHome.BackgroundImage.Size;
            btnNo.BackgroundImage = btnYes.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;
            panelOffer.BackgroundImage = panelSelected.BackgroundImage = ThemeManager.CurrentThemeImages.SpecialOfferButton;
            lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            //for (int i = 0; i < this.Controls.Count; i++)
            //{
            //    this.Controls[i].Visible = false;
            //}//12-06-2015:Ends//Ends:Modification on 17-Dec-2015 for introducing new theme
            
            lblTimeOut.Top = lblSiteName.Bottom + 20;//playpass:Starts
            log.LogMethodExit();
        }

        private void frmNewcard_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Upsell Exiting...");

            Audio.Stop();
            log.LogMethodExit();
        }

        void initializeForm()
        {
            log.LogMethodEntry();
            try
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Modification on 17-Dec-2015 for introducing new theme
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing initializeForm()" + ex.Message);
            }

            btnSelectedProd.Text = Convert.ToDouble(selectedProduct["Price"]).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            string desc = selectedProduct["description"].ToString();
            if (desc.Trim() == "")
                btnSelectedProdDesc.Text = selectedProduct["product_name"].ToString();
            else
                btnSelectedProdDesc.Text = desc.Split('\r', '\n')[0];

            btnUpsellProduct.Text = Convert.ToDouble(offerProduct["Price"]).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            desc = offerProduct["description"].ToString();
            if (desc.Trim() == "")
                btnUpsellProductDesc.Text = offerProduct["product_name"].ToString();
            else
                btnUpsellProductDesc.Text = desc;
            log.LogMethodExit();
        }

        private void frmRedeemTokens_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            initializeForm();

            lblSiteName.Text = KioskStatic.SiteHeading;
            lblGreeting1.Text = MessageUtils.getMessage(417, selectedProduct["product_name"].ToString());
            KioskStatic.formatMessageLine(txtMessage, 23, ThemeManager.CurrentThemeImages.BottomMessageLineImage);//Starts:Modification on 17-Dec-2015 for introducing new theme
            //lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            //txtMessage.ForeColor  = KioskStatic.CurrentTheme.ScreenHeadingForeColor;//lblGreeting1.ForeColor
            //Ends:Modification on 17-Dec-2015 for introducing new theme
            Application.DoEvents();

            KioskStatic.logToFile("Enter Upsell screen");

            lblOffer.Top = (this.Height - lblOffer.Height) / 2;
            log.LogMethodExit();
        }

        private void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            Application.DoEvents();
            //switch (msgType)//Starts:Modification on 17-Dec-2015 for introducing new theme
            //{
            //    case "WARNING": txtMessage.BackColor = Color.Yellow; txtMessage.ForeColor = Color.Black; break;
            //    case "ERROR": txtMessage.BackColor = Color.Red; txtMessage.ForeColor = Color.Black; break;
            //    case "MESSAGE": txtMessage.BackColor = Color.Transparent; txtMessage.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor; break;
            //    default: txtMessage.ForeColor = Color.Black; break;
            //}//Ends:Modification on 17-Dec-2015 for introducing new theme

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
                callTransaction(false);
            }
            log.LogMethodExit();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Audio.Stop();
            KioskStatic.logToFile("upsell = No pressed");
            Application.DoEvents();
            callTransaction(false);
            log.LogMethodExit();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Audio.Stop();
            KioskStatic.logToFile("upsell = Yes pressed");
            Application.DoEvents();
            callTransaction(true);
            log.LogMethodExit();
        }

        private void frmUpsellProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
           // exitTimer.Stop();
        }

        private void btnNo_MouseDown(object sender, MouseEventArgs e)
        {
            // btnNo.BackgroundImage = Properties.Resources.ProductDescription;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnNo_MouseUp(object sender, MouseEventArgs e)
        {
            //btnNo.BackgroundImage = Properties.Resources.ProductDescriptionPressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnYes_MouseDown(object sender, MouseEventArgs e)
        {
            //btnYes.BackgroundImage = Properties.Resources.ProductDescriptionPressed;//Starts:Modification on 17-Dec-2015 for introducing new theme
            //btnNo.BackgroundImage = Properties.Resources.ProductDescription;//Ends:Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnYes_MouseUp(object sender, MouseEventArgs e)
        {
            //btnYes.BackgroundImage = Properties.Resources.ProductDescription;//Starts:Modification on 17-Dec-2015 for introducing new theme
            //btnNo.BackgroundImage = Properties.Resources.ProductDescriptionPressed;//Ends:Modification on 17-Dec-2015 for introducing new theme
        }

        void callTransaction(bool upSell)
        {
            log.LogMethodEntry(upSell);
            ResetKioskTimer();
            StopKioskTimer();

            //exitTimer.Stop();
            DataTable dt;
            if (upSell)
                dt = KioskStatic.getProductDetails(Convert.ToInt32(offerProduct["product_id"]));
            else
                dt = KioskStatic.getProductDetails(Convert.ToInt32(selectedProduct["product_id"]));

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
                    CustomerDTO customerDTO = null;
                    if (kioskTransaction.HasCustomerRecord() == false)
                    {
                        if (KioskStatic.RegisterBeforePurchase)
                        {
                            if (dt.Rows[0]["RegisteredCustomerOnly"].ToString() == "Y")
                            {
                                customerDTO = CustomerStatic.ShowCustomerScreen();
                                if (customerDTO == null)
                                {
                                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                    this.Close();
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                            else
                            {
                                Audio.PlayAudio(Audio.RegisterCardPrompt);
                                if (new frmYesNo(KioskStatic.Utilities.MessageUtils.getMessage(758), KioskStatic.Utilities.MessageUtils.getMessage(759)).ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                                {
                                    customerDTO = CustomerStatic.ShowCustomerScreen();
                                }
                            }
                        }
                    }
                    if (customerDTO != null)
                    {
                        kioskTransaction.SetTransactionCustomer(customerDTO);
                    }
                    int productId = Convert.ToInt32(dt.Rows[0]["product_id"]);
                    kioskTransaction = KioskHelper.AddNewCardProduct(kioskTransaction, dt.Rows[0], 1, selecteEntitlementType);
                    kioskTransaction.SelectedProductType = KioskTransaction.GETNEWCARDTYPE;
                    frmChooseProduct.AlertUserIfWaiverSigningIsRequired(productId, kioskTransaction);
                    using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                    {
                        if (frpm.ShowDialog() == System.Windows.Forms.DialogResult.No) // back button pressed
                        {
                            kioskTransaction = frpm.GetKioskTransaction;
                            //exitTimer.Start();
                            StartKioskTimer();
                            log.LogMethodExit();
                            return;
                        }
                        kioskTransaction = frpm.GetKioskTransaction;
                    }
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
                Dispose();
            }
            else
            {
                Card card = null;
                using (frmTapCard ftc = new frmTapCard(true))
                {
                    ftc.ShowDialog();
                    card = ftc.Card;
                    ftc.Dispose();
                }

                if (card != null)
                {
                    KioskStatic.logToFile("Card: " + card.CardNumber);
                    if (card.technician_card.Equals('Y'))
                    {
                        displayMessageLine(KioskStatic.Utilities.MessageUtils.getMessage(197, card.CardNumber), MESSAGE);
                        KioskStatic.logToFile("Technician Card not allowed: " + card.CardNumber);
                        //exitTimer.Start();
                        StartKioskTimer();
                        log.LogMethodExit();
                        return;
                    }
                    int productId = Convert.ToInt32(dt.Rows[0]["product_id"]);
                    kioskTransaction = KioskHelper.AddRechargeCardProduct(kioskTransaction, card, dt.Rows[0], 1, selecteEntitlementType);
                    kioskTransaction.SelectedProductType = KioskTransaction.GETRECHAREGETYPE;
                    frmChooseProduct.AlertUserIfWaiverSigningIsRequired(productId, kioskTransaction);
                    using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                    {
                        if (frpm.ShowDialog() == System.Windows.Forms.DialogResult.No) // back button pressed
                        {
                            kioskTransaction = frpm.GetKioskTransaction;
                            //exitTimer.Start();
                            StartKioskTimer();
                            log.LogMethodExit();
                            return;
                        }
                        kioskTransaction = frpm.GetKioskTransaction;
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

    }
}
