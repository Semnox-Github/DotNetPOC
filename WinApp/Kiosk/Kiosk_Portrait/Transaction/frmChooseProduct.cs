/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmChooseProduct.cs
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        09-Sep-2019      Deeksha            Added logger methods.
* 2.80        14-Nov-2019      Girish Kundar      Modified: As part of ticket printer integration
* 2.110       21-Dec-2020      Jinto Thomas       Modified: As part of WristBand printer changes
* 2.120       17-Apr-2021      Guru S A           Wristband printing flow enhancements
* 2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
* 2.140.0     18-Oct-2021      Sathyavathi        Check-In Check-Out feature in Kiosk
* 2.150.0.0   21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
* 2.130.11    13-Oct-2022      Vignesh Bhat       Ability to display background images for display group 
* 2.150.0.0   23-Sep-2022      Sathyavathi        Check-In feature Phase-2
* 2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements 
* 2.150.7     10-Nov-2023      Sathyavathi        Customer Lookup Enhancement
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Device;
using Semnox.Parafait.POS;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;

namespace Parafait_Kiosk
{
    public partial class frmChooseProduct : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string Function;
        private int productScrollIndex = 1;
        private string selectedEntitlementType;
        private POSPrinterDTO rfidPrinterDTO = null;
        public bool isClosed = false;
        private string parentCardNumber;
        private bool isProductUILoaded = false;
        private const string KIOSKSETUP = "KIOSK_SETUP";
        private string imageFolder;
        private DataTable displayGroupDataTbl;
        private DataTable ProductTbl;
        private Dictionary<string, Image> displayGroupBackgroundImages = new Dictionary<string, Image>();
        private ExecutionContext executionContext;
        private bool showCartInKiosk = false;
        private string NUMBERFORMAT;
        private System.Windows.Forms.Timer refreshTimer;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmChooseProduct(KioskTransaction kioskTransaction, string pFunction, string entitlementType, string displayGroup = "ALL", string cardNum = "")
        {
            log.LogMethodEntry("kioskTransaction", pFunction, entitlementType, displayGroup);
            KioskStatic.logToFile("frmChooseProduct()");
            selectedEntitlementType = entitlementType;
            this.executionContext = KioskStatic.Utilities.ExecutionContext;
            imageFolder = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY");
            NUMBERFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            parentCardNumber = cardNum;
            this.kioskTransaction = kioskTransaction;
            log.LogVariableState("kioskTransaction", kioskTransaction);
            KioskStatic.Utilities.setLanguage();
            InitializeComponent();
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Tick += new System.EventHandler(this.RefreshTimerTick);
            refreshTimer.Interval = 10;
            refreshTimer.Enabled = true;
            refreshTimer.Stop();
            displayGroupDataTbl = null;
            showCartInKiosk = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_CART_IN_KIOSK", false);
            KioskStatic.setDefaultFont(this);
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnCart.SetCartImage(ThemeManager.CurrentThemeImages.KioskCartIcon);
            btnPrev.BackgroundImage = btnCancel.BackgroundImage = btnVariable.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            if (ThemeManager.CurrentThemeImages.VariableProductButtonImage != null)
            {
                btnVariable.BackgroundImage = ThemeManager.CurrentThemeImages.VariableProductButtonImage;
            }
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.ChooseProductBackgroundImage);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmChooseProduct" + ex.Message);
            }
            Function = pFunction;
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 218);
            //this.ShowInTaskbar = false;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            // initializeProductTab("ALL");
            InitializeDisplayGroups();
            //DisplayScrollButtons();
            SetCustomizedFontColors();
            DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            lblGreeting1.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
            DisplaybtnCart(showCartInKiosk);
            KioskStatic.Utilities.setLanguage(this);
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


        private void frmChooseProduct_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            SetGreetingsMessage();
            ThemeManager.InitializeFlowLayoutVerticalScroll(flpCardProducts, 6);
            this.FormClosing += frmChooseProduct_FormClosing;
            log.LogMethodExit();
        }

        private void SetGreetingsMessage()
        {
            log.LogMethodEntry();
            if (isProductUILoaded)
            {
                switch (Function)
                {
                    case "I":
                        lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, 444);//Choose a New Card
                        txtMessage.Text = lblGreeting1.Text;
                        if (string.IsNullOrWhiteSpace(KioskStatic.CurrentTheme.DisplayProductsGreetingOption) == false
                            && KioskStatic.CurrentTheme.DisplayProductsGreetingOption == KioskStatic.OPT2)
                        {
                            if (selectedEntitlementType == KioskTransaction.TIME_ENTITLEMENT)
                            {
                                lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, 1348);
                            }
                            else if (selectedEntitlementType == KioskTransaction.CREDITS_ENTITLEMENT)
                            {
                                lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, "How many Points would you like?");
                            }
                        }
                        Audio.PlayAudio(Audio.SelectNewCardProduct);
                        break;
                    case "C":
                        lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, 4147);
                        //Choose an Entry Package
                        txtMessage.Text = lblGreeting1.Text;
                        break;
                    case "R":
                        lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, 2438);//Please choose an option
                        txtMessage.Text = lblGreeting1.Text;
                        if (string.IsNullOrWhiteSpace(KioskStatic.CurrentTheme.DisplayProductsGreetingOption) == false
                            && KioskStatic.CurrentTheme.DisplayProductsGreetingOption == KioskStatic.OPT2)
                        {
                            if (selectedEntitlementType == KioskTransaction.TIME_ENTITLEMENT)
                            {
                                lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, 1348);
                            }
                            else if (selectedEntitlementType == KioskTransaction.CREDITS_ENTITLEMENT)
                            {
                                lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, "How many Points would you like?");
                            }
                        }
                        Audio.PlayAudio(Audio.SelectTopUpProduct);
                        break;
                    default:
                        lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, 2438);//Please choose an option
                        txtMessage.Text = lblGreeting1.Text;
                        Audio.PlayAudio(Audio.ChooseOption);
                        break;
                }
            }
            else
            {
                lblGreeting1.Text = KioskStatic.Utilities.MessageUtils.getMessage("Select Display Group");
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2438);//Please choose an option
            }
            log.LogMethodExit();
        }

        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                refreshTimer.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                refreshTimer.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
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
        private void InitializeDisplayGroups()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("initializeDisplayGroups():Called ");
            try
            {
                btnProceed.Visible = false;
                btnVariable.Visible = false;
                isProductUILoaded = false;
                flpCardProducts.SuspendLayout();
                flpCardProducts.Tag = null;
                flpCardProducts.Controls.Clear();
                this.bigVerticalScrollCardProducts.UpdateButtonStatus();
                string backgroundImageFileName;
                if (displayGroupDataTbl == null)
                {
                    displayGroupDataTbl = new DataTable();
                    List<int> prePaymentModeDisplayGroupIds = (kioskTransaction.PreSelectedPaymentModeDTO == null) ? null : KioskHelper.GetPaymentModesIdsWithDisplayGroup(kioskTransaction.PreSelectedPaymentModeDTO);
                    displayGroupDataTbl = KioskStatic.GetProductDisplayGroups(Function, selectedEntitlementType, prePaymentModeDisplayGroupIds);
                }
                if (displayGroupDataTbl.Rows.Count == 0)
                {//occurs when Payment Mode Driven Sales is enabled in Kiosk and included display group does not have products in this category 
                    frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4999));//Sorry, no products were found in this category :(
                }
                else if (displayGroupDataTbl.Rows.Count == 1)
                {
                    if (Function == KioskTransaction.GETNEWCARDTYPE || Function == KioskTransaction.GETRECHAREGETYPE)
                    {
                        if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                        {
                            if (KioskHelper.isTimeEnabledStore() == true)
                            {
                                selectedEntitlementType = KioskTransaction.TIME_ENTITLEMENT;
                            }
                            else
                            {   //What would you like to add?
                                using (frmEntitlement frmEntitle = new frmEntitlement(MessageContainerList.GetMessage(executionContext, 1345)))
                                {
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
                        }
                    }
                    backgroundImageFileName = displayGroupDataTbl.Rows[0]["BackgroundImageFileName"].ToString().Trim();
                    SetScreenBackground(backgroundImageFileName);
                    string displyGroup = displayGroupDataTbl.Rows[0]["display_group"].ToString();
                    isProductUILoaded = true;
                    InitializeProductTab(displyGroup, selectedEntitlementType);
                    log.LogMethodExit();
                    return;
                }
                for (int i = 0; i < displayGroupDataTbl.Rows.Count; i++)
                {
                    Button displayGroupButton;
                    displayGroupButton = BuildDisplayGroupButton(displayGroupDataTbl.Rows[i], i, displayGroupDataTbl);
                    Panel displayPanel = new Panel();
                    displayPanel.Width = flpCardProducts.Width - 30;
                    displayPanel.Height = displayGroupButton.Height;
                    displayPanel.Margin = btnSampleName.Margin;
                    displayGroupButton.Location = new Point((displayPanel.Width - displayGroupButton.Width) / 2, 0);
                    displayPanel.Controls.Add(displayGroupButton);
                    flpCardProducts.Controls.Add(displayPanel);
                    SetButtonBackgroundImage(displayGroupButton, displayGroupDataTbl.Rows[i]);
                    KioskStatic.Utilities.setLanguage(displayGroupButton);
                }
            }
            finally
            {
                flpCardProducts.ResumeLayout(true);
                this.bigVerticalScrollCardProducts.Refresh();
                SetGreetingsMessage();
            }
            //flpCardProducts.Refresh();  // Commented to eliminate the flickering issue. Added Suspend and Resume instead.
            KioskStatic.logToFile(" exit initializeDisplayGroups() ");
            log.LogMethodExit();
        }

        private Button BuildDisplayGroupButton(DataRow dataRow, int rowIndex, DataTable productTbl)
        {
            log.LogMethodEntry(dataRow);
            string backgroundImageFileName;
            Button displayGroupButton = new Button();
            displayGroupButton.Click += DisplayGroupButton_Click;
            //displayGroupButton.MouseDown += displayGroupButton_MouseDown;
            //displayGroupButton.MouseUp += flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right;
            displayGroupButton.Name = "displayGroupButton" + rowIndex.ToString();
            displayGroupButton.Text = dataRow["display_group"].ToString();
            displayGroupButton.Tag = dataRow["display_group"];
            displayGroupButton.Font = btnSampleName.Font;
            displayGroupButton.ForeColor = KioskStatic.CurrentTheme.ChooseProductsBtnTextForeColor;
            displayGroupButton.Size = btnSampleName.Size; // new System.Drawing.Size(btnSampleName.Width, btnHeight);
            displayGroupButton.FlatStyle = btnSampleName.FlatStyle;
            displayGroupButton.FlatAppearance.BorderColor = btnSampleName.FlatAppearance.BorderColor;
            displayGroupButton.FlatAppearance.BorderSize = btnSampleName.FlatAppearance.BorderSize;
            displayGroupButton.BackColor = btnSampleName.BackColor;
            displayGroupButton.BackgroundImage = ThemeManager.CurrentThemeImages.PlainProductButton;
            displayGroupButton.BackgroundImageLayout = btnSampleName.BackgroundImageLayout;
            displayGroupButton.Margin = btnSampleName.Margin;
            //displayGroupButton.TextAlign = btnSampleName.TextAlign;
            displayGroupButton.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.DisplayGroupButtonTextAlignment);
            displayGroupButton.Size = displayGroupButton.BackgroundImage.Size;
            backgroundImageFileName = GetBackgroundImageFileName(productTbl, displayGroupButton.Tag.ToString());
            Image backgroundImage = GetBackgroundImage(backgroundImageFileName);
            if (displayGroupBackgroundImages.ContainsKey(displayGroupButton.Tag.ToString()) == false)
            {
                displayGroupBackgroundImages.Add(displayGroupButton.Tag.ToString(), backgroundImage);
            }
            displayGroupButton.FlatAppearance.MouseOverBackColor = btnSampleName.FlatAppearance.MouseOverBackColor;
            displayGroupButton.FlatAppearance.MouseDownBackColor = btnSampleName.FlatAppearance.MouseDownBackColor;
            displayGroupButton.FlatAppearance.CheckedBackColor = btnSampleName.FlatAppearance.CheckedBackColor;
            log.LogMethodExit();
            return displayGroupButton;
        }

        private void SetScreenBackground(string backgroundImageFileName)
        {
            log.LogMethodEntry(backgroundImageFileName);
            if (string.IsNullOrWhiteSpace(backgroundImageFileName) == false)
            {
                Image backgroundImage = GetBackgroundImage(backgroundImageFileName);
                this.BackgroundImage = backgroundImage;
            }
            else
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.ChooseProductBackgroundImage);
            }
            log.LogMethodExit();
        }

        public override void btnCart_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Cart button clicked");
            try
            {
                try
                {
                    base.LaunchCartForm(NUMBERFORMAT);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    frmOKMsg.ShowUserMessage(ex.Message);
                }
                SetGreetingsMessage();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetButtonBackgroundImage(Button displayGroupButton, DataRow dataRow)
        {
            log.LogMethodEntry("displayGroupButton", dataRow);
            string imageFileName = dataRow["ImageFileName"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(imageFileName) == false)
            {
                try
                {
                    object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                            new SqlParameter("@FileName", imageFolder + "\\" + imageFileName));

                    displayGroupButton.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                    displayGroupButton.Text = "";
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + imageFileName);
                    displayGroupButton.BackgroundImage = ThemeManager.CurrentThemeImages.PlainProductButton;
                    displayGroupButton.Text = dataRow["display_group"].ToString();
                }
            }
            else
            {
                displayGroupButton.BackgroundImage = ThemeManager.CurrentThemeImages.PlainProductButton;
                displayGroupButton.Text = dataRow["display_group"].ToString();
            }
            log.LogMethodExit();
        }
        private void InitializeProductTab(string displayGroup, string entitlementType)
        {
            log.LogMethodEntry(displayGroup, entitlementType);
            KioskStatic.logToFile("initializeProductTab(): " + displayGroup);
            try
            {
                if (kioskTransaction.ShowCartInKiosk == true)
                {
                    btnProceed.Visible = true;
                    if (kioskTransaction != null && kioskTransaction.HasActiveNonFundRaiserOrDonationOrChargeProducts())
                    {
                        this.btnProceed.Enabled = true;
                    }
                    else
                    {
                        this.btnProceed.Enabled = false;
                    }
                }
                else
                {
                    btnProceed.Visible = false;
                }

                //this.SuspendLayout();
                flpCardProducts.SuspendLayout();
                DataTable ProductTbl = null;
                try
                {
                    ProductTbl = KioskHelper.getProducts(displayGroup, Function, entitlementType);
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile(ex.Message);
                    frmOKMsg.ShowUserMessage("Please Retry..." + ex.Message);
                    KioskStatic.logToFile("exit initializeProductTab()");
                    log.LogMethodExit();
                    return;
                }
                flpCardProducts.Controls.Clear();
                for (int i = 0; i < ProductTbl.Rows.Count; i++)
                {
                    if (ProductTbl.Rows[i]["product_type"].ToString().StartsWith("VARIABLE") &&
                        kioskTransaction.ShowCartInKiosk == false &&
                        string.IsNullOrWhiteSpace(KioskStatic.CurrentTheme.DisplayProductsGreetingOption) == false &&
                        KioskStatic.CurrentTheme.DisplayProductsGreetingOption == KioskStatic.OPT2)
                    {
                        BuildVariableProductButton(ProductTbl.Rows[i]["product_id"]);
                    }
                    else
                    {
                        Button ProductButton = BuildProductButton(ProductTbl.Rows[i], i);
                        Panel prodPanel = new Panel();
                        prodPanel.Width = flpCardProducts.Width - 30;
                        prodPanel.Height = ProductButton.Height;
                        prodPanel.Margin = btnSampleName.Margin;
                        ProductButton.Location = new Point((prodPanel.Width - ProductButton.Width) / 2, 0);
                        prodPanel.Controls.Add(ProductButton);
                        flpCardProducts.Controls.Add(prodPanel);
                    }
                }
                //ThemeManager.InitializeFlowLayoutVerticalScroll(flpCardProducts, 6);//6 buttons fits in the flow layout 
            }
            finally
            {
                flpCardProducts.ResumeLayout(true);
                this.bigVerticalScrollCardProducts.AutoHide = true;
                this.bigVerticalScrollCardProducts.Refresh();
                this.bigVerticalScrollCardProducts.UpdateButtonStatus();
                //this.ResumeLayout(true);
            }
            KioskStatic.logToFile("exit initializeProductTab()");
            log.LogMethodExit();
        }

        private Button BuildProductButton(DataRow dataRow, int rowIndex)
        {
            log.LogMethodEntry();
            Image productImage = ThemeManager.CurrentThemeImages.PlainProductButton;
            Button productButton = new Button();
            productButton.Click += new EventHandler(ProductButton_Click);
            productButton.Name = "ProductButton" + rowIndex.ToString();
            productButton.Tag = dataRow["product_id"];
            productButton.Font = btnSampleName.Font;
            productButton.ForeColor = KioskStatic.CurrentTheme.ChooseProductsBtnTextForeColor;
            productButton.Size = btnSampleName.Size;
            productButton.FlatStyle = btnSampleName.FlatStyle;
            productButton.FlatAppearance.BorderColor = btnSampleName.FlatAppearance.BorderColor;
            productButton.FlatAppearance.BorderSize = btnSampleName.FlatAppearance.BorderSize;
            productButton.BackColor = btnSampleName.BackColor;
            productButton.BackgroundImageLayout = btnSampleName.BackgroundImageLayout;
            //productButton.TextAlign = btnSampleName.TextAlign;
            productButton.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.DisplayProductButtonTextAlignment);
            productButton.FlatAppearance.MouseOverBackColor = btnSampleName.FlatAppearance.MouseOverBackColor;
            productButton.FlatAppearance.MouseDownBackColor = btnSampleName.FlatAppearance.MouseDownBackColor;
            productButton.FlatAppearance.CheckedBackColor = btnSampleName.FlatAppearance.CheckedBackColor;

            if (dataRow["ImageFileName"].ToString().Trim() != string.Empty)
            {
                try
                {
                    object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                            new SqlParameter("@FileName", imageFolder + "\\" + dataRow["ImageFileName"].ToString()));

                    productButton.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                    productButton.Text = "";
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + dataRow["ImageFileName"].ToString());
                    productButton.BackgroundImage = productImage;
                    string productName = KioskHelper.GetProductName((int)dataRow["product_id"]);
                    productButton.Text = productName;
                }
            }
            else
            {
                productButton.BackgroundImage = productImage;
                string productName = KioskHelper.GetProductName((int)dataRow["product_id"]);
                productButton.Text = productName;
            }
            productButton.Size = productButton.BackgroundImage.Size;
            productButton.Margin = btnSampleName.Margin;
            log.LogMethodExit();
            return productButton;
        }

        void DisplayGroupButton_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnObj = sender as Button;
                if (btnObj != null)
                {
                    btnObj.Enabled = false;
                }
                log.LogMethodEntry();
                DisableProductButtons();
                StopKioskTimer();
                isProductUILoaded = true;
                KioskStatic.logToFile("displayGroupButton clicked");
                Button displayGroupButton = new Button();
                displayGroupButton = (Button)sender;

                if (Function == KioskTransaction.GETNEWCARDTYPE || Function == KioskTransaction.GETRECHAREGETYPE)
                {
                    if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                    {
                        if (KioskHelper.isTimeEnabledStore() == true)
                        {
                            selectedEntitlementType = KioskTransaction.TIME_ENTITLEMENT;
                        }
                        else
                        {
                            using (frmEntitlement frmEntitle = new frmEntitlement(MessageContainerList.GetMessage(executionContext, 1345)))
                            {
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
                    }
                }
                if (displayGroupBackgroundImages.ContainsKey(displayGroupButton.Tag.ToString()))
                {
                    this.BackgroundImage = displayGroupBackgroundImages[displayGroupButton.Tag.ToString()];
                }
                else
                {
                    this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.ChooseProductBackgroundImage);
                }
                InitializeProductTab((sender as Button).Tag.ToString(), selectedEntitlementType);
                //DisplayScrollButtons();
                flpCardProducts.Tag = (sender as Button).Tag;
                //lblGreeting1.Text = MessageContainerList.GetMessage(executionContext, 218);
                SetGreetingsMessage();
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 218);
                if (Function == KioskTransaction.GETRECHAREGETYPE)
                {
                    Audio.PlayAudio(Audio.SelectTopUpProduct);
                }
                else if (Function == KioskTransaction.GETNEWCARDTYPE)
                {
                    Audio.PlayAudio(Audio.SelectNewCardProduct);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
            }
            finally
            {
                ResetKioskTimer();
                StartKioskTimer();
                EnableProductButtons();
            }
            log.LogMethodExit();
        }

        private void ProductButton_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnObj = sender as Button;
                if (btnObj != null)
                {
                    btnObj.Enabled = false;
                }
                DisableProductButtons();
                log.LogMethodEntry();
                KioskStatic.logToFile("ProductButton_Click()");
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
                        if (dt.Rows[0]["Description"].ToString().Trim() == "")
                            KioskStatic.logToFile(dt.Rows[0]["product_name"].ToString());
                        else
                            KioskStatic.logToFile(dt.Rows[0]["product_name"].ToString() + " - " + dt.Rows[0]["Description"].ToString());

                        if (dt.Rows[0]["product_type"].ToString().Equals("VARIABLECARD"))
                        {
                            double varAmount = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(executionContext, 480), '-', KioskStatic.Utilities);

                            varAmount = kioskTransaction.ValidateVariableAmount(varAmount);

                            dt.Rows[0]["price"] = varAmount;
                        }
                        else
                        {
                            try
                            {
                                DataTable dtUpsell = KioskHelper.getUpsellProducts(dt.Rows[0]["product_id"]);
                                if (dtUpsell.Rows.Count > 0)
                                {
                                    List<Semnox.Parafait.Transaction.Transaction.TransactionLine> activeLines = kioskTransaction.GetActiveTransactionLines;
                                    int beforProdCount = (activeLines != null ? activeLines.Count : 0);
                                    using (frmUpsellProduct fup = new frmUpsellProduct(kioskTransaction, dt.Rows[0], dtUpsell.Rows[0], Function, selectedEntitlementType))
                                    {
                                        DialogResult dr = fup.ShowDialog();
                                        kioskTransaction = fup.GetKioskTransaction;
                                        PrintAddedToCartMsg(beforProdCount, kioskTransaction, txtMessage);
                                        if (dr != System.Windows.Forms.DialogResult.Cancel)
                                        {
                                            if (kioskTransaction.ShowCartInKiosk == false)
                                            {
                                                Close();
                                            }
                                        }
                                        log.LogMethodExit();
                                        return;
                                    }
                                }
                            }
                            catch (CustomerStatic.TimeoutOccurred ex)
                            {
                                KioskStatic.logToFile("Timeout occured");
                                log.Error(ex);
                                PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                                this.DialogResult = DialogResult.Cancel;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error occurred while executing ProductButton_Click()" + ex.Message);
                            }
                        }

                        if (Function == KioskTransaction.GETNEWCARDTYPE || Function == KioskTransaction.GETFNBTYPE)
                        {
                            bool isRegisteredCustomerOnly = (dt.Rows[0]["RegisteredCustomerOnly"].ToString() == "Y") ? true : false;
                            bool showMsgRecommendCustomerToRegister = true;
                            bool isLinked = CustomerStatic.LinkCustomerToTheTransaction(kioskTransaction, executionContext, isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister);
                            if (this.DialogResult == DialogResult.Cancel)
                            {
                                log.LogMethodExit();
                                return;
                            }
                            CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(), isRegisteredCustomerOnly, isLinked);
                            if (isRegisteredCustomerOnly == true && kioskTransaction.HasCustomerRecord() == false)
                            {
                                log.LogMethodExit();
                                return;
                            }
                        }
                        if (Function == KioskTransaction.GETNEWCARDTYPE)
                        {
                            int CardCount = Convert.ToInt32(dt.Rows[0]["CardCount"]);
                            if (CardCount <= 1)
                                CardCount = 0;

                            if (dt.Rows[0]["QuantityPrompt"].ToString().Equals("Y"))
                            {
                                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> activeLines = kioskTransaction.GetActiveTransactionLines;
                                int beforProdCount = (activeLines != null ? activeLines.Count : 0);
                                using (frmCardCount frm = new frmCardCount(kioskTransaction, dt.Rows[0], selectedEntitlementType, CardCount))
                                {
                                    DialogResult dr = frm.ShowDialog();
                                    kioskTransaction = frm.GetKioskTransaction;
                                    PrintAddedToCartMsg(beforProdCount, kioskTransaction, txtMessage);
                                    if (dr == System.Windows.Forms.DialogResult.No) // back button
                                    {
                                        frm.Dispose();
                                        log.LogMethodExit();
                                        return;
                                    }
                                    else if (dr == System.Windows.Forms.DialogResult.Cancel && kioskTransaction.ShowCartInKiosk == false)
                                    {
                                        DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                        this.Close();
                                    }
                                    frm.Dispose();
                                }
                            }
                            else
                            {
                                rfidPrinterDTO = KioskStatic.GetRFIDPrinter(executionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId);
                                bool wristBandPrintTag = KioskStatic.IsWristBandPrintTag(Convert.ToInt32(dt.Rows[0]["product_id"]), rfidPrinterDTO);
                                int dispensorPort = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "CARD_DISPENSER_PORT", -1);
                                if (dispensorPort == -1 && wristBandPrintTag == false)
                                {
                                    log.Info("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                                    frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(executionContext, 2384));
                                    KioskStatic.logToFile("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                                }
                                else
                                {
                                    kioskTransaction = KioskHelper.AddNewCardProduct(kioskTransaction, dt.Rows[0], 1, selectedEntitlementType);
                                    AlertUser(product_id, kioskTransaction, ProceedActionImpl);
                                    if (kioskTransaction.ShowCartInKiosk == false)
                                    {
                                        kioskTransaction.SelectedProductType = KioskTransaction.GETNEWCARDTYPE;
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
                                    else
                                    {
                                        string msg = MessageContainerList.GetMessage(executionContext, 4842);
                                        KioskStatic.logToFile("frmChooseProducts: " + msg);
                                        txtMessage.Text = msg;
                                        //frmOKMsg.ShowUserMessage(msg);
                                    }
                                }
                            }
                            if (kioskTransaction.ShowCartInKiosk == false)
                            {
                                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                Close();
                                Dispose();
                            }
                        }
                        else if (Function.Equals(KioskTransaction.GETCHECKINCHECKOUTTYPE))
                        {
                            ProductsContainerDTO selectedProductContainerDTO = null;
                            int checkInproductId = Convert.ToInt32(dt.Rows[0]["product_id"]);
                            selectedProductContainerDTO = kioskTransaction.IsValidCheckinCheckoutProduct(checkInproductId);

                            if (kioskTransaction.ShowCartInKiosk == false)
                            {
                                kioskTransaction.SelectedProductType = KioskTransaction.GETCHECKINCHECKOUTTYPE;
                            }
                            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> activeLines = kioskTransaction.GetActiveTransactionLines;
                            int beforProdCount = (activeLines != null ? activeLines.Count : 0);
                            using (frmCheckInCheckOutQtyScreen frm = new frmCheckInCheckOutQtyScreen(kioskTransaction, selectedProductContainerDTO.ProductId, parentCardNumber))
                            {
                                DialogResult dre = frm.ShowDialog();
                                kioskTransaction = frm.GetKioskTransaction;
                                PrintAddedToCartMsg(beforProdCount, kioskTransaction, txtMessage);
                                if (dre != DialogResult.No) // back button
                                {
                                    if (kioskTransaction.ShowCartInKiosk == false)
                                    {
                                        DialogResult = DialogResult.Cancel;
                                        Close();
                                    }
                                }
                            }
                        }
                        else if (Function.Equals(KioskTransaction.GETFNBTYPE))
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
                                    txtMessage.Text = msg;
                                    return;
                                }
                                if (varQuantity < 1)
                                {
                                    string msg = MessageContainerList.GetMessage(executionContext, 2360);
                                    //Please enter valid quantity
                                    KioskStatic.logToFile(msg);
                                    log.LogMethodExit(msg);
                                    txtMessage.Text = msg;
                                    return;
                                }
                                KioskStatic.logToFile("Selected quantity is :" + quantity.ToString());
                            }
                            double prodPrice = -1;
                            kioskTransaction.AddManualProduct(product_id, prodPrice, quantity);
                            AlertUser(product_id, kioskTransaction, ProceedActionImpl);
                            if (kioskTransaction.ShowCartInKiosk == false)
                            {
                                kioskTransaction.SelectedProductType = KioskTransaction.GETFNBTYPE;
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
                            else
                            {
                                string msg = MessageContainerList.GetMessage(executionContext, 4842);
                                KioskStatic.logToFile("frmChooseProducts: " + msg);
                                txtMessage.Text = msg;
                                //frmOKMsg.ShowUserMessage(msg);
                                log.LogMethodExit();
                                return;
                            }
                            DialogResult = System.Windows.Forms.DialogResult.Cancel;
                            Close();
                            Dispose();
                        }
                        else if (Function.Equals(KioskTransaction.GETATTRACTIONSTYPE))
                        {
                            ProductsContainerDTO selectedProductContainerDTO = null;
                            int productId = Convert.ToInt32(dt.Rows[0]["product_id"]);
                            selectedProductContainerDTO = kioskTransaction.IsValidAttractionProduct(productId);

                            if (kioskTransaction.ShowCartInKiosk == false)
                            {
                                kioskTransaction.SelectedProductType = KioskTransaction.GETATTRACTIONSTYPE;
                            }
                            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> activeLines = kioskTransaction.GetActiveTransactionLines;
                            int beforProdCount = (activeLines != null ? activeLines.Count : 0);

                            using (frmAttractionQty frm = new frmAttractionQty(kioskTransaction, selectedProductContainerDTO.ProductId))
                            {
                                DialogResult dre = frm.ShowDialog();
                                //this.DialogResult = dre;
                                kioskTransaction = frm.GetKioskTransaction;
                                PrintAddedToCartMsg(beforProdCount, kioskTransaction, txtMessage);
                                if (dre != DialogResult.No) // back button
                                {
                                    if (kioskTransaction.ShowCartInKiosk == false)
                                    {
                                        DialogResult = DialogResult.Cancel;
                                        Close();
                                    }
                                }
                            }
                        }
                        else
                        {
                            bool isRegisteredCustomerOnly = (dt.Rows[0]["RegisteredCustomerOnly"].ToString() == "Y") ? true : false;
                            Card card = null;
                            using (frmTapCard ftc = new frmTapCard(kioskTransaction, isRegisteredCustomerOnly, true))
                            {
                                DialogResult dr =  ftc.ShowDialog();
                                if (dr == DialogResult.Cancel)
                                {

                                    string msg = MessageContainerList.GetMessage(executionContext, "Timeout");
                                    throw new CustomerStatic.TimeoutOccurred(msg);
                                }
                                kioskTransaction = ftc.GetKioskTransaction;
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
                                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 197, card.CardNumber);
                                    log.LogMethodExit();
                                    return;
                                }
                                if (isRegisteredCustomerOnly == true && kioskTransaction.HasCustomerRecord() == false)
                                {
                                    log.LogMethodExit();
                                    return;
                                }
                                kioskTransaction = KioskHelper.AddRechargeCardProduct(kioskTransaction, card, dt.Rows[0], 1, selectedEntitlementType);
                                AlertUser(product_id, kioskTransaction, ProceedActionImpl);
                                if (kioskTransaction.ShowCartInKiosk == false)
                                {
                                    kioskTransaction.SelectedProductType = KioskTransaction.GETRECHAREGETYPE;
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
                                else
                                {
                                    string msg = MessageContainerList.GetMessage(executionContext, 4842);
                                    KioskStatic.logToFile("frmChooseProducts: " + msg);
                                    txtMessage.Text = msg;
                                    //frmOKMsg.ShowUserMessage(msg);
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
                    catch (CustomerStatic.TimeoutOccurred ex)
                    {
                        KioskStatic.logToFile("Timeout occured");
                        log.Error(ex);
                        PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                        this.DialogResult = DialogResult.Cancel;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        KioskStatic.logToFile(ex.Message);
                        frmOKMsg.ShowUserMessage(ex.Message);
                        txtMessage.Text = ex.Message;
                    }
                    finally
                    {
                        this.RefreshCartIconText(NUMBERFORMAT);
                        //EnableProductButtons();
                    }
                }
                else
                {
                    KioskStatic.logToFile("Invalid Product");
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
                ResetKioskTimer();
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        public static void PrintAddedToCartMsg(int beforProdCount, KioskTransaction kioskTransaction, Button txtMessageBox)
        {
            log.LogMethodEntry(beforProdCount);
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> activeLines = kioskTransaction.GetActiveTransactionLines;
            int afterProdCount = (activeLines != null ? activeLines.Count : 0);
            if (afterProdCount > beforProdCount && kioskTransaction.ShowCartInKiosk)
            {
                string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842);
                txtMessageBox.Text = msg;
            }
            log.LogMethodExit();
        }
        public delegate void ProceedAction(KioskTransaction kioskTransaction);
        public static ProceedAction KioskProceedAction;
        public static void AlertUser(int productId, KioskTransaction kioskTransaction, ProceedAction KioskProceedAction)
        {
            log.LogMethodEntry(productId, "kioskTransaction", "KioskProceedAction");
            try
            { 
                if (kioskTransaction.ShowCartInKiosk)
                {
                    //string addProdMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842);
                    //frmOKMsg.ShowShortUserMessage(addProdMsg);
                    using (frmAddToCartAlert f = new frmAddToCartAlert(kioskTransaction, KioskProceedAction))
                    {
                        f.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Back pressed");
            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.ChooseProductBackgroundImage);
            if (isProductUILoaded == true && displayGroupDataTbl != null && displayGroupDataTbl.Rows.Count > 1)
            {
                InitializeDisplayGroups();
                //DisplayScrollButtons();
            }
            else
            {
                if (kioskTransaction.ShowCartInKiosk == true 
                    && Application.OpenForms.Count >=2
                    && Application.OpenForms[Application.OpenForms.Count-2].Name == "frmHome")
                {
                    base.btnHome_Click(sender, e);
                }
                else
                {
                    DialogResult = System.Windows.Forms.DialogResult.No;
                    Close();
                }
            }
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

        public override void Form_Activated(object sender, EventArgs e)//Playpas1:starts
        {
            //ticks = 0;
            //inactivityTimer.Start();
            log.LogMethodEntry();
            ResetKioskTimer();
            StartKioskTimer();
            RefreshCartIconText(NUMBERFORMAT);
            if (kioskTransaction != null && kioskTransaction.HasActiveNonFundRaiserOrDonationOrChargeProducts())
            {
                this.btnProceed.Enabled = true;
            }
            else
            {
                this.btnProceed.Enabled = false;
            }
            log.LogMethodExit();

        }
        public override void Form_Deactivate(object sender, EventArgs e)
        {
            //inactivityTimer.Stop();
            StopKioskTimer();
        }

        private void DisableProductButtons()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Disable Product Buttons");
                flpCardProducts.SuspendLayout();
                List<Panel> buttonPanelList = flpCardProducts.Controls.OfType<Panel>().ToList();
                if (buttonPanelList != null && buttonPanelList.Any())
                {
                    for (int i = buttonPanelList.Count - 1; i >= 0; i--)
                    {
                        List<Button> buttonList = buttonPanelList[i].Controls.OfType<Button>().ToList();
                        if (buttonList != null && buttonList.Any())
                        {
                            for (int j = buttonList.Count - 1; j >= 0; j--)
                            {
                                buttonList[j].Enabled = false;
                            }
                        }
                    }
                }
                this.btnHome.Enabled = false;
                this.btnPrev.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnCart.Enabled = false;
                this.btnProceed.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                flpCardProducts.ResumeLayout(true);
            }
            log.LogMethodExit();
        }
        private void EnableProductButtons()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Enable Product Buttons");
                flpCardProducts.SuspendLayout();
                List<Panel> buttonPanelList = flpCardProducts.Controls.OfType<Panel>().ToList();
                if (buttonPanelList != null && buttonPanelList.Any())
                {
                    for (int i = buttonPanelList.Count - 1; i >= 0; i--)
                    {
                        List<Button> buttonList = buttonPanelList[i].Controls.OfType<Button>().ToList();
                        if (buttonList != null && buttonList.Any())
                        {
                            for (int j = buttonList.Count - 1; j >= 0; j--)
                            {
                                buttonList[j].Enabled = true;
                            }
                        }
                    }
                }
                this.btnHome.Enabled = true;
                this.btnPrev.Enabled = true;
                this.btnCancel.Enabled = true;
                this.btnCart.Enabled = true;
                this.btnVariable.Enabled = true;
                if (kioskTransaction != null && kioskTransaction.HasActiveNonFundRaiserOrDonationOrChargeProducts())
                {
                    this.btnProceed.Enabled = true;
                }
                else
                {
                    this.btnProceed.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                flpCardProducts.ResumeLayout(true);
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
                DeviceContainer.RestartRFIDPrinter(executionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId);
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
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmChoooseProduct");
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
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.ChooseProductsCancelBtnTextForeColor;//Checkout button
                this.btnVariable.ForeColor = KioskStatic.CurrentTheme.ChooseProductVariableBtnTextForeColor;//OtherAmount button
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.ChooseProductsFooterTextForeColor;//Footer text message
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.ChooseProductsBtnHomeTextForeColor;//Footer text message
                this.btnCart.SetFont(this.btnHome.Font);
                this.btnCart.SetForeColor(this.btnHome.ForeColor, KioskStatic.CurrentTheme.KioskCartQuantityTextForeColor);
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.bigVerticalScrollCardProducts.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmChoooseProduct: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private string GetBackgroundImageFileName(DataTable ProductTbl, string displayGroup)
        {
            log.LogMethodEntry();
            string backgroundImageFileName = String.Empty;
            for (int i = 0; i < ProductTbl.Rows.Count; i++)
            {
                if (ProductTbl.Rows[i]["display_group"].ToString().Trim() == displayGroup)
                {
                    backgroundImageFileName = ProductTbl.Rows[i]["BackgroundImageFileName"].ToString().Trim();
                    break;
                }
                else
                {
                    continue;
                }
            }
            log.LogMethodExit(backgroundImageFileName);
            return backgroundImageFileName;
        }
        private Image GetBackgroundImage(string backgroundImageFileName)
        {
            log.LogMethodEntry();
            Image backgroundImage;
            if (string.IsNullOrWhiteSpace(backgroundImageFileName) == false)
            {
                try
                {
                    object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                 new SqlParameter("@FileName", imageFolder + "\\" + backgroundImageFileName));

                    backgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + backgroundImageFileName);
                    backgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.ChooseProductBackgroundImage);
                }
            }
            else
            {
                backgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.ChooseProductBackgroundImage);
            }
            log.LogMethodExit(backgroundImage);
            return backgroundImage;
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            ProceedActionImpl(kioskTransaction);
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void ProceedActionImpl(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry("kioskTransaction");
            try
            {
                kioskTransaction.TransactionHasItems();
                int itemCount = kioskTransaction.GetCartItemCount;
                if (itemCount > 0)
                {
                    using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                    {
                        DialogResult dr = frpm.ShowDialog();
                        kioskTransaction = frpm.GetKioskTransaction;
                        if (dr != System.Windows.Forms.DialogResult.No) // back button pressed
                        {
                            DialogResult = dr;
                            this.Close();
                        }
                    }
                }
                else
                {
                    //Cart is Empty
                    string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4843);
                    KioskStatic.logToFile("Skipping cart proceed action: " + errMsg);
                    Semnox.Core.Utilities.ValidationException validationException = new Semnox.Core.Utilities.ValidationException(errMsg);
                    log.Error(validationException);
                    throw validationException;
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

        private void RefreshTimerTick(System.Object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                refreshTimer.Stop();
                //this.flpCustomerMain.SuspendLayout();
                //this.SuspendLayout();
                //this.flpCustomerMain.ResumeLayout(true);
                //this.ResumeLayout(true);
                this.Refresh();
                //Application.DoEvents();
            }
            catch { }
            log.LogMethodExit();
        }

        private void BuildVariableProductButton(object productId)
        {
            log.LogMethodEntry(productId);
            try
            {
                if (btnVariable.Visible == false)
                {
                    Button ProductButton = btnVariable;
                    ProductButton.Click -= ProductButton_Click;
                    ProductButton.Click += ProductButton_Click;
                    ProductButton.Tag = productId;
                    btnVariable.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while building Variable Product Button", ex);
                KioskStatic.logToFile("Error while building Variable Product Button in frmChoooseProduct: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
