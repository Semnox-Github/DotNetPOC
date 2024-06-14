/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmChooseProduct.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 *2.80       5-Sep-2019       Deeksha            Added logger methods.
 *2.80       14-Nov-2019      Girish Kundar      Modified: As part of ticket printer integration
 *2.110      21-Dec-2020      Jinto Thomas       Modified: As part of WristBand printer changes
 *2.150.1    22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Device;

namespace Parafait_Kiosk
{
    public partial class frmChooseProduct : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string Function;
        int productScrollIndex = 1;//Starts:Modification on 17-Dec-2015 for introducing new theme
                                   //Bitmap productImage, productPressedImage, descImage, descPressedImage, bestDealImage, bestDealPressedImage;//Ends:Modification on 17-Dec-2015 for introducing new theme
        string selectedEntitlementType;
        private POSPrinterDTO rfidPrinterDTO = null;
        public bool isClosed = false;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        public frmChooseProduct(KioskTransaction kioskTransaction, string pFunction, string entitlementType)
        {
            log.LogMethodEntry("kioskTransaction", pFunction, entitlementType);
            KioskStatic.logToFile("frmChooseProduct()");
            selectedEntitlementType = entitlementType;
            KioskStatic.Utilities.setLanguage();
            InitializeComponent();
            KioskStatic.formatMessageLine(txtMessage);
            KioskStatic.setDefaultFont(this);

            Function = pFunction;
            this.kioskTransaction = kioskTransaction;

            this.ShowInTaskbar = false;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            KioskStatic.Utilities.setLanguage(this);

            try
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
                //btnScrollUp.BackgroundImage = ThemeManager.CurrentThemeImages.ScrollUpButtonImage;
                //btnScrollDown.BackgroundImage = ThemeManager.CurrentThemeImages.ScrollDownButton;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnHome.Size = btnHome.BackgroundImage.Size;
                txtMessage.BackgroundImage = ThemeManager.CurrentThemeImages.BottomMessageLineImage;
                btnScrollLeft.BackgroundImage = ThemeManager.CurrentThemeImages.ScrollLeftButton;
                btnScrollLeft.Size = btnScrollLeft.BackgroundImage.Size;
                btnScrollRight.BackgroundImage = ThemeManager.CurrentThemeImages.ScrollRightButton;//Ends:Modification on 17-Dec-2015 for introducing new theme
                btnScrollRight.Size = btnScrollRight.BackgroundImage.Size;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmChooseProduct()" + ex.Message);
            }


            initializeDisplayGroups();//Ends:Modification on 17-Dec-2015 for introducing new theme
            displayScrollButtons();
            log.LogMethodExit();
        }

        private void displayScrollButtons()
        {

            log.LogMethodEntry();
            productScrollIndex = 1;

            flpCardProducts.HorizontalScroll.Value = productScrollIndex;
            flpCardProducts.Refresh();
            btnScrollLeft.Visible = false;//Starts:Modification on 17-Dec-2015 for introducing new theme662d2f24


            if (flpCardProducts.Controls.Count != 0)
            {
                if (flpCardProducts.Controls.Count <= ((flpCardProducts.Width * 2) / flpCardProducts.Controls[0].Width))//NewTheme102015:Starts
                {
                    btnScrollRight.Visible = false;
                }
                else
                {
                    btnScrollRight.Visible = true;
                }
            }
            log.LogMethodExit();
        }



        private void frmChooseProduct_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;
            lblGreeting1.Text = KioskStatic.ProductScreenGreeting;

            lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;

            if (Function == "I")
            {
                lblGreeting2.Text = KioskStatic.Utilities.MessageUtils.getMessage(444);
                //if (KioskStatic.Utilities.ParafaitEnv.CardFaceValue > 0)//Starts:Modification on 17-Dec-2015 for introducing new theme
                //    lblGreetingDeposit.Text = KioskStatic.Utilities.MessageUtils.getMessage(502, KioskStatic.Utilities.ParafaitEnv.CardFaceValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                //else
                //    lblGreetingDeposit.Visible = false;//Ends:Modification on 17-Dec-2015 for introducing new theme

                Audio.PlayAudio(Audio.SelectNewCardProduct);
            }
            else
            {
                lblGreeting2.Text = KioskStatic.Utilities.MessageUtils.getMessage(1256);
                lblGreetingDeposit.Visible = false;
                Audio.PlayAudio(Audio.ChooseOption);
            }

            //txtMessage.ForeColor = lblGreeting2.ForeColor = lblGreeting1.ForeColor = lblGreetingDeposit.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;//Starts:Modification on 17-Dec-2015 for introducing new theme
            ThemeManager.InitializeFlowLayoutHorizontalScroll(flpCardProducts, 3);
            txtMessage.Text = lblGreeting2.Text;//"";//Starts:Modification on 17-Dec-2015 for introducing new theme

            this.FormClosing += frmChooseProduct_FormClosing;
            //inactivityTimer.Start();
            log.LogMethodExit();
        }

        void frmChooseProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            //inactivityTimer.Stop();
            log.LogMethodEntry();
            Audio.Stop();
            KioskStatic.logToFile("exit frmChooseProduct()");
            log.LogMethodExit();
        }
        public void initializeDisplayGroups()//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            flpCardProducts.Tag = null;
            flpCardProducts.Controls.Clear();


            DataTable ProductTbl = new DataTable();
            ProductTbl = KioskStatic.GetProductDisplayGroups(Function, selectedEntitlementType);

            if (ProductTbl.Rows.Count == 1)
            {
                if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                {
                    if (KioskHelper.isTimeEnabledStore() == true)
                    {
                        selectedEntitlementType = KioskTransaction.TIME_ENTITLEMENT;
                    }
                    else
                    {
                        frmEntitlement frmEntitle = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1345));
                        if (frmEntitle.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        {
                            frmEntitle.Dispose();
                            isClosed = true;
                            log.LogMethodExit();
                            return;
                        }
                        selectedEntitlementType = frmEntitle.selectedEntitlement;
                        frmEntitle.Dispose();
                    }
                }
                initializeProductTab("ALL", selectedEntitlementType);
                log.LogMethodExit();
                return;
            }
            for (int i = 0; i < ProductTbl.Rows.Count; i++)
            {
                Button displayGroupButton = new Button();
                displayGroupButton.Click += displayGroupButton_Click;
                //displayGroupButton.MouseDown += displayGroupButton_MouseDown;
                //displayGroupButton.MouseUp += flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right;
                displayGroupButton.Name = "displayGroupButton" + i.ToString();
                displayGroupButton.Text = ProductTbl.Rows[i]["display_group"].ToString();

                displayGroupButton.Tag = ProductTbl.Rows[i]["display_group"];
                displayGroupButton.Font = btnSampleName.Font;
                displayGroupButton.ForeColor = btnSampleName.ForeColor;
                displayGroupButton.Size = btnSampleName.Size; // new System.Drawing.Size(btnSampleName.Width, btnHeight);
                displayGroupButton.FlatStyle = btnSampleName.FlatStyle;
                displayGroupButton.FlatAppearance.BorderColor = btnSampleName.FlatAppearance.BorderColor;
                displayGroupButton.FlatAppearance.BorderSize = btnSampleName.FlatAppearance.BorderSize;
                displayGroupButton.BackColor = btnSampleName.BackColor;
                //displayGroupButton.BackgroundImage = btnSampleName.BackgroundImage;
                displayGroupButton.BackgroundImage = ThemeManager.CurrentThemeImages.PlainProductButton;
                displayGroupButton.BackgroundImageLayout = btnSampleName.BackgroundImageLayout;
                displayGroupButton.Margin = btnSampleName.Margin;
                displayGroupButton.TextAlign = btnSampleName.TextAlign;

                displayGroupButton.FlatAppearance.MouseOverBackColor = btnSampleName.FlatAppearance.MouseOverBackColor;
                displayGroupButton.FlatAppearance.MouseDownBackColor = btnSampleName.FlatAppearance.MouseDownBackColor;
                displayGroupButton.FlatAppearance.CheckedBackColor = btnSampleName.FlatAppearance.CheckedBackColor;

                flpCardProducts.Controls.Add(displayGroupButton);
            }
            flpCardProducts.Refresh();
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        void displayGroupButton_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            (sender as Button).BackgroundImage = Properties.Resources.save_cancel_btn;
            log.LogMethodExit();
        }

        void displayGroupButton_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            (sender as Button).BackgroundImage = Properties.Resources.save_cancel_btn_pressed;
            log.LogMethodExit();
        }

        void displayGroupButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            KioskStatic.logToFile("displayGroupButton clicked");
            if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
            {
                if (KioskHelper.isTimeEnabledStore() == true)
                {
                    selectedEntitlementType = KioskTransaction.TIME_ENTITLEMENT;
                }
                else
                {
                    frmEntitlement frmEntitle = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1345));
                    if (frmEntitle.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        frmEntitle.Dispose();
                        log.LogMethodExit();
                        return;
                    }
                    selectedEntitlementType = frmEntitle.selectedEntitlement;
                    frmEntitle.Dispose();
                }
            }
            initializeProductTab((sender as Button).Tag.ToString(), selectedEntitlementType);
            displayScrollButtons();
            flpCardProducts.Tag = (sender as Button).Tag;
            lblGreeting2.Text = KioskStatic.Utilities.MessageUtils.getMessage(218);
            txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(218);
            Audio.PlayAudio(Audio.SelectTopUpProduct);
            ResetKioskTimer();
            StartKioskTimer();
            log.LogMethodExit();
        }



        public void initializeProductTab(string displayGroup, string entitlementType)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry(displayGroup, entitlementType);
            Image productImage = ThemeManager.CurrentThemeImages.PlainProductButton;

            DataTable ProductTbl = null;

            ProductTbl = KioskHelper.getProducts(displayGroup, Function, entitlementType);
            flpCardProducts.Controls.Clear();

            string imageFolder = KioskStatic.Utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            for (int i = 0; i < ProductTbl.Rows.Count; i++)
            {
                Button ProductButton = new Button();
                ProductButton.Click += new EventHandler(ProductButton_Click);
                ProductButton.Name = "ProductButton" + i.ToString();
                ProductButton.Tag = ProductTbl.Rows[i]["product_id"];
                ProductButton.Font = btnSampleName.Font;
                ProductButton.ForeColor = btnSampleName.ForeColor;
                ProductButton.Size = btnSampleName.Size;
                ProductButton.FlatStyle = btnSampleName.FlatStyle;
                ProductButton.FlatAppearance.BorderColor = btnSampleName.FlatAppearance.BorderColor;
                ProductButton.FlatAppearance.BorderSize = btnSampleName.FlatAppearance.BorderSize;
                ProductButton.BackColor = btnSampleName.BackColor;

                ProductButton.TextAlign = btnSampleName.TextAlign;

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
                        ProductButton.Size = ProductButton.BackgroundImage.Size;
                    }
                    catch (Exception ex)
                    {
                        KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + ProductTbl.Rows[i]["ImageFileName"].ToString());
                        ProductButton.BackgroundImage = productImage;
                        ProductButton.Text = ProductTbl.Rows[i]["product_name"].ToString();
                    }
                }
                else
                {
                    ProductButton.BackgroundImage = productImage;
                    ProductButton.BackgroundImageLayout = ImageLayout.Stretch;
                    ProductButton.Text = ProductTbl.Rows[i]["product_name"].ToString();// +Environment.NewLine + ProductTbl.Rows[i]["description"].ToString();
                }
                //if (ProductButton.BackgroundImage != null)
                //{
                //    ProductButton.Size = ProductButton.BackgroundImage.Size;
                //}
                ProductButton.Margin = btnSampleName.Margin;
                Panel prodPanel = new Panel();
                prodPanel.Width = btnSampleName.Width;

                prodPanel.Height = btnSampleName.Height;
                prodPanel.Margin = btnSampleName.Margin;//Modification on 17-Dec-2015 for introducing new theme
                ProductButton.Location = new Point((prodPanel.Width - ProductButton.Width) / 2, 0);
                prodPanel.Controls.Add(ProductButton);
                flpCardProducts.Controls.Add(prodPanel);
                // }//Modification on 17-Dec-2015 for introducing new theme
            }
            ThemeManager.InitializeFlowLayoutHorizontalScroll(flpCardProducts, 2);
            flpCardProducts.Refresh();
            KioskStatic.logToFile("exit initializeProductTab()");
            log.LogMethodExit();
        }




        void ProductButton_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = (sender as Button);
            if (b.Name.StartsWith("desc"))
            {
                Control c = flpCardProducts.GetNextControl(b, false);
                if (c != null)
                    //  c.BackgroundImage = productImage;
                    if (b.BackgroundImage != null && b.BackgroundImage.Tag != null)
                    {
                        // b.BackgroundImage = bestDealImage;
                        b.BackgroundImage.Tag = "bestDeal";
                    }
                //else
                //  b.BackgroundImage = descImage;
            }
            else
            {
                Control c = flpCardProducts.GetNextControl(b, true);
                if (c != null)
                {
                    if (c.BackgroundImage != null && c.BackgroundImage.Tag != null)
                    {
                        //c.BackgroundImage = bestDealImage;
                        c.BackgroundImage.Tag = "bestDeal";
                    }
                    // else
                    //  c.BackgroundImage = descImage;
                }

                // b.BackgroundImage = productImage;
            }
            log.LogMethodExit();
        }

        void ProductButton_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = (sender as Button);
            if (b.Name.StartsWith("desc"))
            {
                Control c = flpCardProducts.GetNextControl(b, false);
                if (c != null)
                    // c.BackgroundImage = productPressedImage;
                    if (b.BackgroundImage != null && b.BackgroundImage.Tag != null)
                    {
                        //b.BackgroundImage = bestDealPressedImage;
                        b.BackgroundImage.Tag = "bestDeal";
                    }
                // else
                // b.BackgroundImage = descPressedImage;
            }
            else
            {
                Control c = flpCardProducts.GetNextControl(b, true);
                if (c != null)
                {
                    if (c.BackgroundImage != null && c.BackgroundImage.Tag != null)
                    {
                        //c.BackgroundImage = bestDealPressedImage;
                        c.BackgroundImage.Tag = "bestDeal";
                    }
                    //else
                    // c.BackgroundImage = descPressedImage;
                }

                //b.BackgroundImage = productPressedImage;
            }
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme


        bool setProductButtonColor(Button button, string color)
        {
            log.LogMethodEntry(button, color);
            try
            {
                Color BGcolor, ForeColor;
                if (color.Contains(",")) // RGB
                {
                    int p = color.IndexOf(',');
                    int r = Convert.ToInt32(color.Substring(0, p));
                    color = color.Substring(p + 1).Trim();

                    p = color.IndexOf(',');
                    int g = Convert.ToInt32(color.Substring(0, p));
                    color = color.Substring(p + 1).Trim();

                    int b = Convert.ToInt32(color);

                    BGcolor = Color.FromArgb(r, g, b);
                    ForeColor = Color.FromArgb(255 - r, 255 - g, 255 - b);
                }
                else
                {
                    BGcolor = Color.FromName(color);
                    if (BGcolor.ToArgb() == 0)
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                    ForeColor = Color.FromArgb(255 - BGcolor.R, 255 - BGcolor.G, 255 - BGcolor.B);
                }

                //button.BackColor = BGcolor;
                button.BackgroundImage = (Image)ChangeColor((Bitmap)button.BackgroundImage, BGcolor);
                button.ForeColor = ForeColor;
                log.LogMethodExit(true);
                return true;
            }
            catch
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        public Bitmap ChangeColor(Bitmap scrBitmap, Color newColor)
        {
            log.LogMethodEntry(scrBitmap, newColor);
            Color actulaColor;
            //make an empty bitmap the same size as scrBitmap
            Bitmap newBitmap = new Bitmap(scrBitmap.Width, scrBitmap.Height);
            for (int i = 0; i < scrBitmap.Width; i++)
            {
                for (int j = 0; j < scrBitmap.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actulaColor = scrBitmap.GetPixel(i, j);
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actulaColor.A > 150)
                        newBitmap.SetPixel(i, j, newColor);
                    else
                        newBitmap.SetPixel(i, j, actulaColor);
                }
            }
            log.LogMethodExit(newBitmap);
            return newBitmap;
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
                        txtMessage.Text = dt.Rows[0]["product_name"].ToString();
                    else
                        txtMessage.Text = dt.Rows[0]["product_name"].ToString() + " - " + dt.Rows[0]["Description"].ToString();

                    KioskStatic.logToFile(txtMessage.Text);

                    if (dt.Rows[0]["product_type"].ToString().Equals("VARIABLECARD"))
                    {
                        //double varAmount = NumberPadForm.ShowNumberPadForm(KioskStatic.Utilities.MessageUtils.getMessage(480), '-', KioskStatic.Utilities);
                        double varAmount = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm(KioskStatic.Utilities.MessageUtils.getMessage(480), "-", KioskStatic.Utilities);
                        try
                        {
                            varAmount = kioskTransaction.ValidateVariableAmount(varAmount);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            KioskStatic.logToFile(ex.Message);
                            log.LogMethodExit(ex.Message);
                            txtMessage.Text = ex.Message;
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
                                using (frmUpsellProduct fup = new frmUpsellProduct(kioskTransaction, dt.Rows[0], dtUpsell.Rows[0], Function, selectedEntitlementType))
                                {
                                    DialogResult dr = fup.ShowDialog();
                                    kioskTransaction = fup.GetKioskTransaction;
                                    if (dr != System.Windows.Forms.DialogResult.Cancel)
                                        Close();
                                }
                                log.LogMethodExit();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                    }

                    if (Function == KioskTransaction.GETNEWCARDTYPE)
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
                            using (frmCardCount frm = new frmCardCount(kioskTransaction, dt.Rows[0], selectedEntitlementType, CardCount))
                            {
                                DialogResult dr = frm.ShowDialog();
                                kioskTransaction = frm.GetKioskTransaction;
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
                                    if (customerDTO != null)
                                    {
                                        kioskTransaction.SetTransactionCustomer(customerDTO);
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
                                kioskTransaction = KioskHelper.AddNewCardProduct(kioskTransaction, dt.Rows[0], 1, selectedEntitlementType);
                                kioskTransaction.SelectedProductType = KioskTransaction.GETNEWCARDTYPE;
                                AlertUserIfWaiverSigningIsRequired(product_id, kioskTransaction);
                                using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                                {
                                    if (frpm.ShowDialog() == System.Windows.Forms.DialogResult.No) // back button pressed
                                    {
                                        kioskTransaction = frpm.GetKioskTransaction;
                                        log.LogMethodExit();
                                        return;
                                    }
                                    kioskTransaction = frpm.GetKioskTransaction;
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
                        using (frmTapCard ftc = new frmTapCard(true))
                        {
                            ftc.ShowDialog();
                            card = ftc.Card;
                            ftc.Dispose();
                        }

                        //card = new POSCore.Card("ABCDEFGH", "", KioskStatic.Utilities);
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
                            if (dt.Rows[0]["RegisteredCustomerOnly"].ToString() == "Y" && (card.customerDTO == null || (KioskStatic.Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M") && card.customerDTO != null && !card.customerDTO.PolicyTermsAccepted)))
                            {
                                customerDTO = CustomerStatic.ShowCustomerScreen(card.CardNumber);
                                if (customerDTO == null)
                                {
                                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                    this.Close();
                                    log.LogMethodExit();
                                    return;
                                }
                                else
                                {
                                    card.customerDTO = customerDTO;
                                }
                            }
                            kioskTransaction = KioskHelper.AddRechargeCardProduct(kioskTransaction, card, dt.Rows[0], 1, selectedEntitlementType);
                            kioskTransaction.SelectedProductType = KioskTransaction.GETRECHAREGETYPE;
                            AlertUserIfWaiverSigningIsRequired(product_id, kioskTransaction);
                            using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                            {
                                if (frpm.ShowDialog() == System.Windows.Forms.DialogResult.No) // back button pressed
                                {
                                    kioskTransaction = frpm.GetKioskTransaction;
                                    log.LogMethodExit();
                                    return;
                                }
                                kioskTransaction = frpm.GetKioskTransaction;
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
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile(ex.Message);
                    frmOKMsg.ShowUserMessage(ex.Message);
                }
                finally
                {
                    EnableProductButtons();
                }
            }
            else
            {
                txtMessage.Text = "Invalid Product";
                KioskStatic.logToFile("Invalid Product");
                //ticks = 0;
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (flpCardProducts.Tag == null)//Starts:Modification on 17-Dec-2015 for introducing new theme
                Close();
            else
                initializeDisplayGroups();//Ends:Modification on 17-Dec-2015 for introducing new theme
            log.LogMethodExit();
        }


        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
            // btnPrev.BackgroundImage = Properties.Resources.back_btn_pressed;//Starts:Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.back_btn;//Starts:Modification on 17-Dec-2015 for introducing new theme
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
            //inactivityTimer.Stop();
            log.LogMethodEntry();
            StopKioskTimer();
            log.LogMethodExit();
        }

        private void btnScrollLeft_Click(object sender, EventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            ThemeManager.FlowLayoutScrollLeft(flpCardProducts, ref productScrollIndex);
            if (flpCardProducts.Controls.Count > (flpCardProducts.Width / flpCardProducts.Controls[0].Width))//5 is the visible row count  without scrolling
            {
                if (productScrollIndex == 1)
                {
                    btnScrollLeft.Visible = false;
                }
                else
                {
                    btnScrollLeft.Visible = true;
                }
                int DownBound = (((flpCardProducts.Controls[0].Width + 1) * (flpCardProducts.Controls.Count + 1 - (flpCardProducts.Width / flpCardProducts.Controls[0].Width))) + 1);
                //int DownBound = (((flpCardProducts.Controls[0].Width + flpCardProducts.Controls[0].Margin.Left +) * (flpCardProducts.Controls.Count + 1 - (flpCardProducts.Width / flpCardProducts.Controls[0].Width))) + 1);
                if (DownBound <= productScrollIndex)
                {
                    btnScrollRight.Visible = false;
                }
                else
                {
                    btnScrollRight.Visible = true;
                }
            }
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void btnScrollRight_Click(object sender, EventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            ThemeManager.FlowLayoutScrollRight(flpCardProducts, ref productScrollIndex);
            if (flpCardProducts.Controls.Count > (flpCardProducts.Width / flpCardProducts.Controls[0].Width))//5 is the visible row count  without scrolling
            {
                if (productScrollIndex == 1)
                {
                    btnScrollLeft.Visible = false;
                }
                else
                {
                    btnScrollLeft.Visible = true;
                }
                int DownBound = (((flpCardProducts.Controls[0].Width + 1) * ((flpCardProducts.Controls.Count / 2) + 1 - (flpCardProducts.Width / flpCardProducts.Controls[0].Width))) + 1);
                if (DownBound <= productScrollIndex)
                {
                    btnScrollRight.Visible = false;
                }
                else
                {
                    btnScrollRight.Visible = true;
                }
            }
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
        public static void AlertUserIfWaiverSigningIsRequired(int productId, KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry(productId);
            try
            {
                kioskTransaction.IsWaiverSigningRequiredForTheProduct(productId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Product: " + productId + " : " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
