/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - frmUpsellProduct 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        4-Sep-2019       Deeksha             Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.130.0    30-Jun-2021      Dakshak             Theme changes to support customized Font ForeColor
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

namespace Parafait_Kiosk
{
    public partial class frmUpsellProduct : BaseForm
    {
        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;

        DataRow selectedProduct, offerProduct;
        string selecteEntitlementType = "Credits";
        string _Function;
        public frmUpsellProduct(DataRow SelectedProduct, DataRow OfferProduct, string Function, string entitlmentType)
        {
            log.LogMethodEntry(SelectedProduct, OfferProduct, Function, entitlmentType);
            selectedProduct = SelectedProduct;
            _Function = Function;
            offerProduct = OfferProduct;
            selecteEntitlementType = entitlmentType;

            InitializeComponent();
            //exitTimer.Enabled = false;

            try
            {
                if (System.IO.File.Exists(Application.StartupPath + @"\Media\Images\UpsellOfferImage.png"))
                {
                    pbOfferLogo.Image = Image.FromFile(Application.StartupPath + @"\Media\Images\UpsellOfferImage.png");
                    pbOfferLogo.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmUpsellProduct()" + ex.Message);
            }

            Utilities.setLanguage(this);
            // KioskStatic.setDefaultFont(this);

            displayMessageLine(offerProduct["OfferMessage"].ToString(), MESSAGE);
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);
            //lblTimeOut.Text = "20";
            lblTimeOut.Text = GetKioskTimerTickValue().ToString();


            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            this.ShowInTaskbar = false;

            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].Visible = false;
            }//12-06-2015:Ends
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
                this.BackgroundImage = KioskStatic.CurrentTheme.UpsellBackgroundImage;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing initializeForm()" + ex.Message);
            }

            lblGreeting1.Text = MessageUtils.getMessage(417, Convert.ToDouble(selectedProduct["Price"]).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + " (" + selectedProduct["product_name"].ToString() + ")");

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

        private void frmRedeemTokens_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            initializeForm();

            lblSiteName.Text = KioskStatic.SiteHeading;

            Application.DoEvents();

            KioskStatic.logToFile("Enter Upsell screen");

            for (int i = 0; i < this.Controls.Count; i++)//12-06-2015:Starts
            {
                this.Controls[i].Visible = true;
            }//12-06-2015:Ends

            panelUpsell.Left = (this.Width - panelUpsell.Width) / 2;

            //exitTimer.Interval = 1000;
            //exitTimer.Start();
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        private void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            Application.DoEvents();
            //switch (msgType)
            //{
            //    case "WARNING": txtMessage.BackColor = Color.Yellow; txtMessage.ForeColor = Color.Black; break;
            //    case "ERROR": txtMessage.BackColor = Color.Red; txtMessage.ForeColor = Color.Black; break;
            //    case "MESSAGE": txtMessage.BackColor = Color.Transparent; txtMessage.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor; break;
            //    default: txtMessage.ForeColor = Color.Black; break;
            //}

            txtMessage.Text = message;
            log.LogMethodExit();
        }

        //double totSecs = 30;
        //private void exitTimer_Tick(object sender, EventArgs e)
        //{
        //    totSecs--;
        //    if (totSecs > 0)
        //    {
        //        lblTimeOut.Text = totSecs.ToString("#0");
        //    }

        //    if (totSecs == 10)
        //    {
        //        exitTimer.Stop();
        //        if (TimeOut.AbortTimeOut(this))
        //        {
        //            exitTimer.Start();
        //            totSecs = 30;
        //        }
        //        else
        //            totSecs = 0;
        //    }

        //    if (totSecs <= 0)
        //    {
        //        exitTimer.Stop();
        //        KioskStatic.logToFile("Exit Timer ticked. Calling transaction with upsell = false");
        //        callTransaction(false);
        //    }
        //}
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
            //exitTimer.Stop();
        }

        private void btnNo_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void btnNo_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void btnYes_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void btnYes_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        //Begin Timer Cleanup
        //protected override CreateParams CreateParams
        //{
        //    //this method is used to avoid flickering.
        //    get
        //    {
        //        CreateParams CP = base.CreateParams;
        //        CP.ExStyle = CP.ExStyle | 0x02000000;
        //        return CP;
        //    }
        //}
        //End Timer Cleanup

        void callTransaction(bool upSell)
        {
            //totSecs = 20;
            //exitTimer.Stop();
            log.LogMethodEntry(upSell);
            ResetKioskTimer();
            StopKioskTimer();
            DataTable dt;
            if (upSell)
                dt = KioskStatic.getProductDetails(Convert.ToInt32(offerProduct["product_id"]));
            else
                dt = KioskStatic.getProductDetails(Convert.ToInt32(selectedProduct["product_id"]));

            //if (KioskStatic.Utilities.getParafaitDefaults("DISABLE_PURCHASE_ON_CARD_LOW_LEVEL").Equals("Y"))
            //{
            //    frmEntitlement frm = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(10943));
            //    frm.ShowDialog();
            //    //string selectedEntitlement = frm.selectedEntitlement;
            //    frm.Dispose();
            //}

            if (_Function == "I" )
            {
                //int productId = Convert.ToInt32(dt.Rows[0]["product_id"]);
                //POSPrinterDTO rfidPrinterDTO = KioskStatic.GetRFIDPrinter(KioskStatic.Utilities.ExecutionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId);
                //bool wristBandPrint = KioskStatic.IsWristBandPrintTag(productId, rfidPrinterDTO); 
                //if (wristBandPrint)
                //{
                //    try
                //    {
                //        KioskStatic.logToFile("Calling Validate RFID Printer");
                //        KioskStatic.ValidateRFIDPrinter(KioskStatic.Utilities.ExecutionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId, productId);
                //    }
                //    catch (Exception ex)
                //    {
                //        using (frmOKMsg fok = new frmOKMsg(ex.Message))
                //        {
                //            fok.ShowDialog();
                //        }
                //        KioskStatic.logToFile(ex.Message);
                //        log.Error(ex);
                //        this.Close();
                //        log.LogMethodExit();
                //        return;
                //    }
                //}

                int CardCount = Convert.ToInt32(dt.Rows[0]["CardCount"]);
                                
                if (CardCount <= 1)
                    CardCount = 0;

                if (dt.Rows[0]["QuantityPrompt"].ToString().Equals("Y") )
                {
                    frmCardCount frm = new frmCardCount(dt.Rows[0], selecteEntitlementType, CardCount);
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.No)           
                    {
                        //exitTimer.Start();
                        StartKioskTimer();
                        log.LogMethodExit();
                        return;
                    }
                }
                else
                {
                    CustomerDTO customerDTO = null;
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

                    frmPaymentMode frpm = new frmPaymentMode(_Function, dt.Rows[0], null, customerDTO, selecteEntitlementType, CardCount);
                    if (frpm.ShowDialog() == System.Windows.Forms.DialogResult.No) // back button pressed
                    {
                        //exitTimer.Start();
                        StartKioskTimer();
                        log.LogMethodExit();
                        return;
                    }
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
                Dispose();
            }
            else
            {
                Card card = null;
                frmTapCard ftc = new frmTapCard(true);
                ftc.ShowDialog();
                card = ftc.Card;
                ftc.Dispose();

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

                    frmPaymentMode frpm = new frmPaymentMode("R", dt.Rows[0], card, null, selecteEntitlementType);
                    if (frpm.ShowDialog() == System.Windows.Forms.DialogResult.No) // back button pressed
                    {
                        //exitTimer.Start();
                        StartKioskTimer();
                        log.LogMethodExit();
                        return;
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
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
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
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;

            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
