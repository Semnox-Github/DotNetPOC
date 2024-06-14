/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - frmChooseProduct.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.80        14-Nov-2019      Girish Kundar      Modified: As part of ticket printer integration
 *2.80.1      02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.110       21-Dec-2020      Jinto Thomas       Modified: As part of WristBand printer changes
 *2.120       17-Apr-2021      Guru S A           Wristband printing flow enhancements
 *2.130.0     30-Jun-2021      Dakshak            Theme changes to support customized Font ForeColor
 *2.130.11    13-Oct-2022      Vignesh Bhat       Ability to display background images for display group 
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.logger;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using System.Collections.Generic;
using Semnox.Parafait.Device;

namespace Parafait_Kiosk
{
    public partial class frmChooseProduct : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string Function;
        public string selectedEntitlementType = "Credits";
        private POSPrinterDTO rfidPrinterDTO = null;
        private string imageFolder;

        public frmChooseProduct(string pFunction, string entitlementType, string displayGroup = "ALL", string backgroundImageFileName = null)
        {
            log.LogMethodEntry(pFunction, entitlementType, displayGroup);
            KioskStatic.logToFile("frmChooseProduct()");
            imageFolder = KioskStatic.Utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            InitializeComponent();
            try
            {
                SetFormBackgroundImage(backgroundImageFileName);
                KioskStatic.setDefaultFont(this);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmChooseProduct()" + ex.Message);
            }
            selectedEntitlementType = entitlementType;
            KioskStatic.Utilities.setLanguage();
            KioskStatic.setDefaultFont(this);

            Function = pFunction;
            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);
            txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(218);
            this.ShowInTaskbar = false;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            KioskStatic.Utilities.setLanguage(this);


            initializeProductTab(displayGroup);
            SetCustomizedFontColors();

            log.LogMethodExit();
        }

        private void frmChooseProduct_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;
            //lblGreeting1.Text = KioskStatic.ProductScreenGreeting;


            //txtMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            //txtMessage.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(218);

            if (Function == "I")
            {
                //lblGreeting1.Text = KioskStatic.Utilities.MessageUtils.getMessage(444);
                Audio.PlayAudio(Audio.SelectNewCardProduct);
            }
            else
            {
                //lblGreeting1.Text = KioskStatic.Utilities.MessageUtils.getMessage(445);
                Audio.PlayAudio(Audio.SelectTopUpProduct);
            }
            //lblGreeting1.ForeColor = lblGreeting1.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            if (selectedEntitlementType == "Time")
                // lblGreeting1.Text = KioskStatic.Utilities.MessageUtils.getMessage(12430);
                // else
                lblGreeting1.Text = KioskStatic.Utilities.MessageUtils.getMessage(1348);
            this.FormClosing += frmChooseProduct_FormClosing;
            //inactivityTimer.Start();
            displaybtnCancel(true);
            log.LogMethodExit();
        } 


        void frmChooseProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            //inactivityTimer.Stop();
            Audio.Stop();
            KioskStatic.logToFile("exit frmChooseProduct()");
            log.LogMethodExit();
        }

        public void initializeProductTab(string displayGroup)
        {
            log.LogMethodEntry(displayGroup);
            KioskStatic.logToFile("initializeProductTab(): " + displayGroup);
            //Bitmap productImage = Properties.Resources.plain_product_button;

            DataTable ProductTbl = null;
            try
            {
                ProductTbl = KioskHelper.getProducts(displayGroup, Function, selectedEntitlementType);
                if (selectedEntitlementType == "Time")
                {
                    int prodCount = ProductTbl.AsEnumerable().Count(p => p.Field<string>("product_type") == "GAMETIME");
                    if (prodCount <= 0)
                    {
                        KioskActivityLogDTO kioskActivityLogDTO = new KioskActivityLogDTO();
                        kioskActivityLogDTO.KioskTrxId = Convert.ToInt32(-1);
                        kioskActivityLogDTO.Activity = Function == "I" ? "Buy New Card Process" : "Recharge Card Process";
                        kioskActivityLogDTO.TrxId = Convert.ToInt32(-1);
                        kioskActivityLogDTO.Value = 0;
                        kioskActivityLogDTO.Message = "No product present under" + " " + displayGroup.ToString() + " " + "display group";
                        kioskActivityLogDTO.KioskId = KioskStatic.Utilities.ParafaitEnv.POSMachineId;
                        kioskActivityLogDTO.KioskName = KioskStatic.Utilities.ParafaitEnv.POSMachine;
                        kioskActivityLogDTO.TimeStamp = ServerDateTime.Now;
                        KioskActivityLogBL kioskActivityLogBL = new KioskActivityLogBL(kioskActivityLogDTO);
                        kioskActivityLogBL.Save();
                        frmOKMsg f = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(1394));
                        f.ShowDialog();
                        selectedEntitlementType = null;
                        Close();
                        log.LogMethodExit();
                        return;

                    }
                }

            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                (new frmOKMsg("Please Retry..." + ex.Message)).ShowDialog();
                KioskStatic.logToFile("exit initializeProductTab()");
                log.LogMethodExit();
                return;
            }

            flpCardProducts.Controls.Clear();

            string imageFolder = KioskStatic.Utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            for (int i = 0; i < ProductTbl.Rows.Count; i++)
            {
                if (ProductTbl.Rows[i]["product_type"].ToString().StartsWith("VARIABLE"))
                {
                    Button ProductButton = btnVariable;
                    ProductButton.Click -= ProductButton_Click;
                    ProductButton.Click += ProductButton_Click;
                    ProductButton.Tag = ProductTbl.Rows[i]["product_id"];
                    btnVariable.Visible = true;
                }
                else
                {
                    Button ProductButton = new Button();
                    ProductButton.Click += new EventHandler(ProductButton_Click);
                    ProductButton.Name = "ProductButton" + i.ToString();
                    ProductButton.Tag = ProductTbl.Rows[i]["product_id"];
                    ProductButton.Font = new Font(KioskStatic.CurrentTheme.DefaultFont.Name, btnSampleName.Font.Size, btnSampleName.Font.Style);
                    ProductButton.ForeColor = KioskStatic.CurrentTheme.ThemeForeColor;
                    ProductButton.Size = btnSampleName.Size;
                    ProductButton.FlatStyle = btnSampleName.FlatStyle;
                    ProductButton.FlatAppearance.BorderColor = btnSampleName.FlatAppearance.BorderColor;
                    ProductButton.FlatAppearance.BorderSize = btnSampleName.FlatAppearance.BorderSize;
                    ProductButton.BackColor = btnSampleName.BackColor;
                    ProductButton.BackgroundImageLayout = btnSampleName.BackgroundImageLayout;

                    ProductButton.Margin = btnSampleName.Margin;

                    ProductButton.FlatAppearance.MouseOverBackColor = btnSampleName.FlatAppearance.MouseOverBackColor;
                    ProductButton.FlatAppearance.MouseDownBackColor = btnSampleName.FlatAppearance.MouseDownBackColor;
                    ProductButton.FlatAppearance.CheckedBackColor = btnSampleName.FlatAppearance.CheckedBackColor;

                    if (ProductTbl.Rows[i]["ImageFileName"].ToString().Trim() != string.Empty)
                    {
                        try
                        {
                            object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                    new SqlParameter("@FileName", imageFolder + "\\" + ProductTbl.Rows[i]["ImageFileName"].ToString()));

                            ProductButton.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                            ProductButton.Text = "";
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + ProductTbl.Rows[i]["ImageFileName"].ToString());
                            ProductButton.BackgroundImage = KioskStatic.CurrentTheme.ChooseProductButton;
                            ProductButton.Text = ProductTbl.Rows[i]["product_name"].ToString();
                        }
                    }
                    else
                    {
                        ProductButton.BackgroundImage = KioskStatic.CurrentTheme.ChooseProductButton;
                        ProductButton.Text = ProductTbl.Rows[i]["product_name"].ToString();
                    }

                    ProductButton.Size = ProductButton.BackgroundImage.Size;

                    Panel prodPanel = new Panel();
                    prodPanel.Width = flpCardProducts.Width;
                    prodPanel.Height = ProductButton.Height;
                    ProductButton.Location = new Point((prodPanel.Width - ProductButton.Width) / 2, 0);
                    prodPanel.Controls.Add(ProductButton);
                    flpCardProducts.Controls.Add(prodPanel);
                }
            }

            if (flpCardProducts.Height > panelProducts.Height)
            {
                vScrollBarProducts.Visible = true;
                vScrollBarProducts.Maximum = (int)((flpCardProducts.Height - panelProducts.Height) * 1.3);
                vScrollBarProducts.SmallChange = Math.Max(1, vScrollBarProducts.Maximum / 10);
                vScrollBarProducts.LargeChange = Math.Max(1, vScrollBarProducts.Maximum / 4);
            }
            else
            {
                vScrollBarProducts.Visible = false;
            }

            flpCardProducts.Refresh();
            KioskStatic.logToFile("exit initializeProductTab()");
            log.LogMethodExit();
        }

        private void ProductButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("ProductButton_Click()");
            txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(218);

            //inactivityTimer.Stop();
            StopKioskTimer();
            Button b = (Button)sender;
            int product_id = Convert.ToInt32(b.Tag);

            flpCardProducts.Tag = null;

            DataTable dt = KioskStatic.getProductDetails(product_id);
            if (dt.Rows.Count > 0)
            {
                try
                {
                    DisableProductButtons();
                    if (dt.Rows[0]["Description"].ToString().Trim() == "")
                        KioskStatic.logToFile(dt.Rows[0]["product_name"].ToString());
                    else
                        KioskStatic.logToFile(dt.Rows[0]["product_name"].ToString() + " - " + dt.Rows[0]["Description"].ToString());

                    if (dt.Rows[0]["product_type"].ToString().Equals("VARIABLECARD"))
                    {
                        double varAmount = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm(KioskStatic.Utilities.MessageUtils.getMessage(480), "0.00", KioskStatic.Utilities);
                        if (varAmount <= 0)
                        {
                            KioskStatic.logToFile("Var amount <= 0; exit ProductButton_Click()");
                            log.LogMethodExit();
                            return;
                        }
                        if (KioskStatic.ALLOW_DECIMALS_IN_VARIABLE_RECHARGE)
                        {
                            KioskStatic.logToFile("ALLOW_DECIMALS_IN_VARIABLE_PURCHASE Set to Y");
                            varAmount = Math.Round(varAmount, 2);
                        }
                        else
                        {
                            if (varAmount != Math.Round(varAmount, 0))
                            {
                                using (frmOKMsg fok = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(932)))
                                { fok.ShowDialog(); }
                                KioskStatic.logToFile("Var amount decimal; exit ProductButton_Click()");
                                log.LogMethodExit();
                                return;
                            }
                        }
                        if (varAmount > KioskStatic.MAX_VARIABLE_RECHARGE_AMOUNT)
                        {
                            using (frmOKMsg fok = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(930, KioskStatic.MAX_VARIABLE_RECHARGE_AMOUNT.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL))))
                            {
                                fok.ShowDialog();
                            }
                            KioskStatic.logToFile("Var amount > max var amount (" + KioskStatic.MAX_VARIABLE_RECHARGE_AMOUNT.ToString() + "); exit ProductButton_Click()");
                            log.LogMethodExit();
                            return;
                        }

                        dt.Rows[0]["price"] = varAmount;
                    }
                    else
                    {
                        try
                        {
                            DataTable dtUpsell = KioskHelper.getUpsellProducts(dt.Rows[0]["product_id"]);
                            if (dtUpsell.Rows.Count > 0)
                            {
                                frmUpsellProduct fup = new frmUpsellProduct(dt.Rows[0], dtUpsell.Rows[0], Function, selectedEntitlementType);
                                if (fup.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                                    Close();
                                log.LogMethodExit();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred while executing ProductButton_Click()" + ex.Message);
                        }
                    }

                    if (Function == "I")
                    {
                        int CardCount = Convert.ToInt32(dt.Rows[0]["CardCount"]);

                        //rfidPrinterDTO = KioskStatic.GetRFIDPrinter(KioskStatic.Utilities.ExecutionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId);
                        //bool wristBandPrint = KioskStatic.IsWristBandPrintTag(Convert.ToInt32(dt.Rows[0]["product_id"]), rfidPrinterDTO);

                        //if (wristBandPrint)
                        //{
                        //    try
                        //    {
                        //        KioskStatic.logToFile("Calling Validate RFID Printer");
                        //        KioskStatic.ValidateRFIDPrinter(KioskStatic.Utilities.ExecutionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId, product_id);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        using (frmOKMsg fok = new frmOKMsg(ex.Message))
                        //        {
                        //            fok.ShowDialog();
                        //        }
                        //        KioskStatic.logToFile(ex.Message);
                        //        log.Error(ex);
                        //        return;
                        //    }
                        //}
                        if (CardCount <= 1)
                            CardCount = 0;

                        if (dt.Rows[0]["QuantityPrompt"].ToString().Equals("Y"))
                        {
                            frmCardCount frm = new frmCardCount(dt.Rows[0], selectedEntitlementType, CardCount);
                            DialogResult dr = frm.ShowDialog();
                            if (dr == System.Windows.Forms.DialogResult.No) // back button
                            {
                                frm.Dispose();
                                log.LogMethodExit();
                                return;
                            }
                            else if (dr == System.Windows.Forms.DialogResult.Cancel)
                            {
                                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                this.Close();
                            }
                            frm.Dispose();
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
                            rfidPrinterDTO = KioskStatic.GetRFIDPrinter(KioskStatic.Utilities.ExecutionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId);
                            bool wristBandPrintTag = KioskStatic.IsWristBandPrintTag(Convert.ToInt32(dt.Rows[0]["product_id"]), rfidPrinterDTO);
                            // dt.Rows[0]["AutoGenerateCardNumber"].ToString() == "N" || dt.Rows[0]["AutoGenerateCardNumber"] == DBNull.Value ? false : true;
                            if (KioskStatic.config.dispport == -1 && wristBandPrintTag == false)
                            {
                                log.Info("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                                frmOKMsg f = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(2384));
                                f.ShowDialog();
                                KioskStatic.logToFile("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                            }
                            else
                            {
                                frmPaymentMode frpm = new frmPaymentMode(Function, dt.Rows[0], null, customerDTO, selectedEntitlementType, CardCount);
                                if (frpm.ShowDialog() == System.Windows.Forms.DialogResult.No) // back button pressed
                                {
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                        }

                        DialogResult = System.Windows.Forms.DialogResult.Cancel;
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

                        //card = new POSCore.Card("F0CEF634", "", KioskStatic.Utilities);
                        if (card != null)
                        {
                            KioskStatic.logToFile("Card: " + card.CardNumber);

                            //Modified on 14-Apr-2017, to restrict load points to Tech card in KIOSK
                            if (card.technician_card.Equals('Y'))
                            {
                                txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(197, card.CardNumber);
                                log.LogMethodExit();
                                return;
                            }
                            //end

                            CustomerDTO customerDTO = null;
                            if (dt.Rows[0]["RegisteredCustomerOnly"].ToString() == "Y"
                                && (card.customerDTO == null
                                    || (KioskStatic.Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M")
                                         && card.customerDTO != null
                                         && !card.customerDTO.PolicyTermsAccepted
                                        )
                                    )
                                )
                            {
                                customerDTO = CustomerStatic.ShowCustomerScreen(card.CardNumber);
                                if (customerDTO == null)
                                {
                                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                    this.Close();
                                    return;
                                }
                            }

                            frmPaymentMode frpm = new frmPaymentMode("R", dt.Rows[0], card, customerDTO, selectedEntitlementType);
                            if (frpm.ShowDialog() == System.Windows.Forms.DialogResult.No) // back button pressed
                            {
                                log.LogMethodExit();
                                return;
                            }

                            DialogResult = System.Windows.Forms.DialogResult.Cancel;
                            Close();
                            Dispose();
                        }
                        else
                        {
                            KioskStatic.logToFile("Card not tapped");
                        }
                    }
                }
                finally
                {
                    EnableProductButtons();
                }
            }
            else
            {
                KioskStatic.logToFile("Invalid Product");
                //ticks = 0;
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Back pressed");
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }
        
        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.back_btn_pressed;
        }

        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.back_btn;
        }

        private void frmChooseProduct_Activated(object sender, EventArgs e)//Playpas1:starts
        {
            //ticks = 0;
            //inactivityTimer.Start();
            log.LogMethodEntry();
            ResetKioskTimer();
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void frmChooseProduct_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            // inactivityTimer.Stop();
            StopKioskTimer();
            log.LogMethodEntry();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Cancel pressed");
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodEntry();
        }

        private void vScrollBarProducts_Scroll(object sender, ScrollEventArgs e)
        {
            //ticks = 0;
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
            ResetKioskTimer();
            if (flpCardProducts.Top + flpCardProducts.Height > panelProducts.Height)
            {
                flpCardProducts.Top = flpCardProducts.Top - value;
            }
            log.LogMethodExit();
        }

        void scrollUp(int value = 10)
        {
            log.LogMethodEntry(value);
            ResetKioskTimer();
            if (flpCardProducts.Top < 0)
                flpCardProducts.Top = Math.Min(0, flpCardProducts.Top + value);
            log.LogMethodExit();
        }
        private void DisableProductButtons()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Disable Product Buttons");
                List<Panel> buttonPanelList = flpCardProducts.Controls.OfType<Panel>().ToList();
                if (buttonPanelList != null && buttonPanelList.Any())
                {
                    for (int i = 0; i < buttonPanelList.Count; i++)
                    {
                        List<Button> buttonList = buttonPanelList[i].Controls.OfType<Button>().ToList();
                        if (buttonList != null && buttonList.Any())
                        {
                            for (int j = 0; j < buttonList.Count; j++)
                            {
                                buttonList[j].Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void EnableProductButtons()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Enable Product Buttons");
                List<Panel> buttonPanelList = flpCardProducts.Controls.OfType<Panel>().ToList();
                if (buttonPanelList != null && buttonPanelList.Any())
                {
                    for (int i = 0; i < buttonPanelList.Count; i++)
                    {
                        List<Button> buttonList = buttonPanelList[i].Controls.OfType<Button>().ToList();
                        if (buttonList != null && buttonList.Any())
                        {
                            for (int j = 0; j < buttonList.Count; j++)
                            {
                                buttonList[j].Enabled = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void RestartRFIDPrinter()
        {
            log.LogMethodEntry();
            try
            {
                log.Info("Calling Restart RFID Printer");
                KioskStatic.logToFile("Calling Restart RFID Printer");
                DeviceContainer.RestartRFIDPrinter(KioskStatic.Utilities.ExecutionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId);
                log.Info("RFID Printer Restarted Successfully");
                KioskStatic.logToFile("RFID Printer Restarted Successfully");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error restarting RFID Printer: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                foreach (Control c in flpCardProducts.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("panel"))
                    {
                        foreach (Control btn in c.Controls)
                        {
                            string btnType = btn.GetType().ToString().ToLower();
                            if (btnType.Contains("button"))
                            {
                                btn.ForeColor = KioskStatic.CurrentTheme.ChooseProductsBtnTextForeColor;//Products buttons 
                            }
                        }
                    }
                }
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.ChooseProductsGreetingsTextForeColor;//How many points or minutes per card label
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.ChooseProductsBackBtnTextForeColor;//Back button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.ChooseProductsCancelBtnTextForeColor;//Cancel button
                this.btnVariable.ForeColor = KioskStatic.CurrentTheme.ChooseProductsVariableBtnTextForeColor;//Variable button
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.ChooseProductsFooterTextForeColor;//Footer text message
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetFormBackgroundImage(string backgroundImageFileName)
        {
            log.LogMethodEntry();
            if (string.IsNullOrWhiteSpace(backgroundImageFileName) == false)
            {
                try
                {
                    object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                 new SqlParameter("@FileName", imageFolder + "\\" + backgroundImageFileName));

                    this.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + backgroundImageFileName);
                    this.BackgroundImage = KioskStatic.CurrentTheme.ProductBackgroundImage; ;
                }
            }
            else
            {
                this.BackgroundImage = KioskStatic.CurrentTheme.ProductBackgroundImage; ;
            }
            log.LogMethodExit();
        }
    }
}
