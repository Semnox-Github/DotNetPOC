/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Choose Gift UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 *2.6.0       11-Apr-2019      Archana              Include/Exclude for redeemable products changes
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Transaction;

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskChooseGiftScreen : frmRedemptionKioskBaseForm
    {
        int productScrollIndex = 1;
        internal int? totalTickets = 0;
        internal bool isCategory = false;
        internal string range = "";
        internal string category = "Select All";
        internal DeviceClass giftScreenCardReaderDevice = null;
        internal DeviceClass giftScreenBarcodeScannerDevice = null;
        int pageNo;
        int pageSize = 9;
        int productCount;
        List<ProductDTO> productDTOList;
        Card redemptionPrimaryCard;
        double redemptionDiscount;
        bool callSearch;
        AlphaNumericKeyPad keypad;
        private readonly TagNumberParser tagNumberParser;
        private Image defaultProductImage = Properties.Resources.default_product_image;

        public frmRedemptionKioskChooseGiftScreen()
        {
            log.LogMethodEntry();
            InitializeComponent();
            btnLeft.Visible = btnRight.Visible = false;
            pageNo = 1;
            productCount = 0;
            redemptionPrimaryCard = null;
            SetPrimaryCardDetails();
            tagNumberParser = new TagNumberParser(Common.utils.ExecutionContext);
            log.LogMethodExit();
        }

        public override void UpdateHeader()
        {
            log.LogMethodEntry();
            if (redemptionOrder != null && redemptionOrder.RedemptionDTO != null)
            {
                totalTickets = redemptionOrder.GetTotalTickets();
                lblTicketsLoaded.Text = totalTickets.ToString();
                lblRedeemedTickets.Text = redemptionOrder.GetRedeemedTickets().ToString();
                lblAvailableTickets.Text = redemptionOrder.GetAvailbleTickets().ToString();
            } 
            log.LogMethodExit();
        }

        void ClearSearch()
        {
            log.LogMethodEntry();
            ResetTimeOut();
            txtSearch.Clear();
            ShowhideKeypad('H');
            range = "";
            category = "Select All";
            // LoadSearchProducts();
            if (isCategory)
            {
                btnCatgAll.PerformClick();
            }
            else
            {
                btnAll.PerformClick();
            }
            log.LogMethodExit();
        }

        private void FrmChooseGift_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.KeyPreview = true;
            RegisterDevices();
            try
            {

                ResetTimeOut();
                base.RenderPanelContent(_screenModel, panelHeader1, 1);
                base.RenderPanelContent(_screenModel, flpMenu, 5);
                base.RenderPanelContent(_screenModel, flpMenuCategory, 3);
                base.RenderPanelContent(_screenModel, panelBottom, 4);
                base.RenderPanelContent(_screenModel, panelDefaultProduct, 6);
                if (panelDefaultProduct.BackgroundImage != null
                    && panelDefaultProduct.BackgroundImage.Height > 30
                    && panelDefaultProduct.BackgroundImage.Width > 30)
                {
                    defaultProductImage = panelDefaultProduct.BackgroundImage;
                    //defaultProductImage = pbxDefaultProduct.BackgroundImage;
                }
                flpMenuCategory.Hide();
                LoadSearchProducts();
                callSearch = false;
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            Common.utils.setLanguage(this);
            log.LogMethodExit();
        }

        private void DisplayScrollButtons()
        {
            log.LogMethodEntry();
            productScrollIndex = 1;

            flpProducts.VerticalScroll.Value = productScrollIndex;
            flpProducts.Refresh();
            btnLeft.Visible = btnRight.Visible = false;
            //if (flpProducts.Controls.Count != 0)
            if (productCount != 0)
            {
                //if (flpProducts.Controls.Count <= 9) 
                if (productCount <= 9)
                {
                    btnRight.Visible = false;
                }
                else
                {
                    btnRight.Visible = true;
                }
            }
            log.LogMethodExit();
        }


        void LoadSearchProducts()
        {
            log.LogMethodEntry();
            ResetTimeOut();
            log.Info("Start Time LoadSearchMethod: " + DateTime.Now.ToString());
            UpdateHeader();
            ShowhideKeypad('H');
            if (redemptionOrder == null)
            {
                log.LogMethodExit("redemptionOrder == null");
                return;
            }

            int numberOfTickets = redemptionOrder.GetAvailbleTickets();

            if (numberOfTickets <= 0)
            {
                log.Debug(Common.utils.MessageUtils.getMessage(1633));
                log.LogMethodExit();
                return;
            }
            productCount = 0;
            pageNo = 1;
            flpProducts.Controls.Clear();
            Semnox.Parafait.Product.ProductList product = new Semnox.Parafait.Product.ProductList(Common.utils.ExecutionContext);
            productDTOList = new List<ProductDTO>();
            AdvancedSearch advenceSearch = null;
            ScreenModel.ElementParameter parameter = new ScreenModel.ElementParameter();
            int ticketsParam = numberOfTickets;
            if (isCategory == true && range != "All")
            {
                advenceSearch = new AdvancedSearch(Common.utils, null, null)
                {
                    searchCriteria = range
                };
            }
            else if (range != "" && range != "All" && Convert.ToInt32(range) <= numberOfTickets)
            {
                ticketsParam = Convert.ToInt32(range);
            }

            productDTOList = product.GetSearchCriteriaAllProductsWithInventory(Common.utils.ExecutionContext, advenceSearch, txtSearch.Text, ticketsParam, Common.utils.ParafaitEnv.POSMachineId, Common.utils.ParafaitEnv.LoginID);

            if (productDTOList != null && productDTOList.Count > 0)
            {
                productCount = productDTOList.Count;
                PopulateFlpProducts();
                RedemptionKioskHelper.InitializeFlowLayoutHorizontalScroll(flpProducts, 3, productCount);
                DisplayScrollButtons();
                flpProducts.Refresh();
            }
            else
            {

                if (txtSearch.Text != string.Empty)
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(4108, txtSearch.Text));
                    //"Search by [ &1 ] returned no matching gift records"
                }
                else
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1629, numberOfTickets.ToString()));
                    //No gifts available for &1 tickets
                }
                DisplayScrollButtons();
                flpProducts.Refresh();
                log.LogMethodExit();
                return;
            }

            log.Info("Number of gifts = " + productCount.ToString());
            log.Info("End Time LoadSearchMethod: " + DateTime.Now.ToString());
            log.LogMethodExit();
        }

        void PopulateFlpProducts()
        {
            log.LogMethodEntry();
            flpProducts.Controls.Clear();
            List<ProductDTO> pageProductsDTOLIst = productDTOList.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            ProductUserControl userControlItem;
            foreach (ProductDTO item in pageProductsDTOLIst)
            {
                userControlItem = new ProductUserControl(item, redemptionOrder, redemptionDiscount, defaultProductImage)
                {
                    setRefreshCallBack = LoadSearchProducts,
                    ResetTimeOutCallback = ResetTimeOut,
                    Size = new Size(flpProducts.Width / 3, flpProducts.Height / 3)
                };
                flpProducts.Controls.Add(userControlItem);
            }
            log.LogMethodExit();
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {

        }
        private void BtnLeft_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                if (pageNo > 1)
                {
                    pageNo -= 1;
                }
                else
                {
                    pageNo = 1;
                }
                this.Cursor = Cursors.WaitCursor;
                PopulateFlpProducts();
                productScrollIndex = RedemptionKioskHelper.FlowLayoutScrollLeft(flpProducts, productScrollIndex);
                ShowHideScrollButtons();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                pageNo += 1;
                this.Cursor = Cursors.WaitCursor;
                PopulateFlpProducts();
                productScrollIndex = RedemptionKioskHelper.FlowLayoutScrollRight(flpProducts, productScrollIndex);
                ShowHideScrollButtons();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        void ShowHideScrollButtons()
        {
            log.LogMethodEntry();
            //if (flpProducts.Controls.Count > (flpProducts.Height / (flpProducts.Controls[0].Height + flpProducts.Controls[0].Margin.Top + flpProducts.Controls[0].Margin.Bottom)))
            if (productCount > (flpProducts.Height / (flpProducts.Controls[0].Height + flpProducts.Controls[0].Margin.Top + flpProducts.Controls[0].Margin.Bottom)))
            {
                if (productScrollIndex == 1)
                {
                    btnLeft.Visible = false;
                }
                else
                {
                    btnLeft.Visible = true;
                }
                if (((flpProducts.VerticalScroll.Maximum - flpProducts.VerticalScroll.SmallChange) <= productScrollIndex)
                    || ((double)productCount / pageNo <= 9)
                    )
                {
                    btnRight.Visible = false;
                }
                else
                {
                    btnRight.Visible = true;
                }
            }
            log.LogMethodExit();
        }
        private void BtnStartOver_Click(object sender, EventArgs e)
        {

        }


        internal override bool ValidateAction(ScreenModel.UIPanelElement element)
        {
            log.LogMethodEntry(element);
            ResetTimeOut();
            if (element.ActionScreenId > 0)
            {
                ScreenModel screen = new ScreenModel(element.ActionScreenId);
                if (screen.CodeObjectName == "frmTicketsField")
                {
                    isCategory = false;
                    flpMenu.Show();
                    flpMenuCategory.Hide();
                    ScreenModel.UIPanel uiPanel = screen.GetPanelByIndex(1);
                    ScreenModel.UIPanelElement uiPanelElement = uiPanel.getElementByIndex(2);
                    btnCategoryField.Image = uiPanelElement.Attribute.DisplayImage;
                    uiPanel = _screenModel.GetPanelByIndex(1);
                    uiPanelElement = uiPanel.getElementByIndex(3);
                    btnTicketsField.Image = uiPanelElement.Attribute.DisplayImage;
                    btnAll.PerformClick();
                    log.LogMethodExit(false);
                    return false;
                }
                if (screen.CodeObjectName == "frmCategoryField")
                {
                    isCategory = true;
                    flpMenu.Hide();
                    flpMenuCategory.Show();
                    ScreenModel.UIPanel uiPanel = screen.GetPanelByIndex(1);
                    ScreenModel.UIPanelElement uiPanelElement = uiPanel.getElementByIndex(3);
                    btnTicketsField.Image = uiPanelElement.Attribute.DisplayImage;
                    uiPanelElement = uiPanel.getElementByIndex(2);
                    btnCategoryField.Image = uiPanelElement.Attribute.DisplayImage;
                    btnCatgAll.PerformClick();
                    log.LogMethodExit(false);
                    return false;
                }
                if (screen.CodeObjectName == "frmSearchProduct")
                {
                    ShowhideKeypad('H');
                    LoadSearchProducts();
                    log.LogMethodExit(false);
                    return false;
                }
                if (screen.CodeObjectName == "frmClearSearch")
                {
                    ShowhideKeypad('H');
                    ClearSearch();
                    log.LogMethodExit(false);
                    return false;
                }

                if (screen.CodeObjectName == "frmStartOver")
                {
                    ShowhideKeypad('H');
                    StartOver();
                    log.LogMethodExit(false);
                    return false;
                }

                if (screen.CodeObjectName == "frmTicketToggle" && isCategory == false)
                {
                    UpdateTicketCategoryImage(screen, element, 5);
                    RenderButton(element);
                    log.LogMethodExit(false);
                    return false;
                }
                if (screen.CodeObjectName == "frmCategoryToggle" && isCategory == true)
                {
                    UpdateTicketCategoryImage(screen, element, 3);
                    RenderButton(element);
                    log.LogMethodExit(false);
                    return false;
                }
                if (screen.CodeObjectName == "frmRedemptionKioskConfirmOrderScreen")
                {
                    log.Debug("for frmConfirmOrder");
                    ShowhideKeypad('H');
                    return InvokeConfirmOrder();
                }
                if (screen.CodeObjectName == "frmRedemptionKioskViewOrderScreen")
                {
                    callSearch = true;
                    ShowhideKeypad('H');
                }
            }
            ResetTimeOut();
            log.LogMethodExit(true);
            return true;
        }
        bool InvokeConfirmOrder()
        {
            log.LogMethodEntry();
            bool retValue = false;
            try
            {
                ResetTimeOut();
                if (!redemptionOrder.RedemptionHasGifts())
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1631));
                    retValue = false;
                }
                else
                {
                    retValue = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        void UpdateTicketCategoryImage(ScreenModel screen, ScreenModel.UIPanelElement element, int panelIndex)
        {
            log.LogMethodEntry(screen, element);
            ScreenModel.UIPanel newPanel = screen.GetPanelByIndex(1);
            ScreenModel.UIPanel pageUIPanel = _screenModel.GetPanelByIndex(panelIndex);
            foreach (ScreenModel.UIPanelElement elementEntry in newPanel.Elements)
            {
                if (element.ElementName == elementEntry.ElementName)
                {
                    if (elementEntry.ElementIndex == 1)
                    {
                        SetTicketImage(elementEntry, pageUIPanel);
                    }
                    else
                    {
                        SetTicketImage(elementEntry, newPanel);
                    }
                }
                else
                {
                    if (elementEntry.ElementIndex == 1)
                    {
                        SetTicketImage(elementEntry, newPanel);
                    }
                    else
                    {
                        SetTicketImage(elementEntry, pageUIPanel);
                    }
                }
            }

            log.LogMethodExit();
        }

        void SetTicketImage(ScreenModel.UIPanelElement elementEntry, ScreenModel.UIPanel uiPanel)
        {
            log.LogMethodEntry();
            ResetTimeOut();
            ScreenModel.UIPanelElement uiPanelElement = new ScreenModel.UIPanelElement();
            switch (elementEntry.ElementName)
            {
                case "TicketMenuOne":
                    uiPanelElement = uiPanel.getElementByIndex(1);
                    btnAll.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "TicketMenuTwo":
                    uiPanelElement = uiPanel.getElementByIndex(2);
                    btnOne.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "TicketMenuThree":
                    uiPanelElement = uiPanel.getElementByIndex(3);
                    btnTwo.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "TicketMenuFour":
                    uiPanelElement = uiPanel.getElementByIndex(4);
                    btnThree.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "TicketMenuFive":
                    uiPanelElement = uiPanel.getElementByIndex(5);
                    btnFour.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "TicketMenuSix":
                    uiPanelElement = uiPanel.getElementByIndex(6);
                    btnFive.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "TicketMenuSeven":
                    uiPanelElement = uiPanel.getElementByIndex(7);
                    btnSix.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "TicketMenuEight":
                    uiPanelElement = uiPanel.getElementByIndex(8);
                    btnSeven.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "CategoryMenuOne":
                    uiPanelElement = uiPanel.getElementByIndex(1);
                    btnCatgAll.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "CategoryMenuTwo":
                    uiPanelElement = uiPanel.getElementByIndex(2);
                    btnCatgOne.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "CategoryMenuThree":
                    uiPanelElement = uiPanel.getElementByIndex(3);
                    btnCategTwo.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "CategoryMenuFour":
                    uiPanelElement = uiPanel.getElementByIndex(4);
                    btnCategThree.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "CategoryMenuFive":
                    uiPanelElement = uiPanel.getElementByIndex(5);
                    btnCategFour.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "CategoryMenuSix":
                    uiPanelElement = uiPanel.getElementByIndex(6);
                    btnCategFive.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "CategoryMenuSeven":
                    uiPanelElement = uiPanel.getElementByIndex(7);
                    btnCategSix.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
                case "CategoryMenuEight":
                    uiPanelElement = uiPanel.getElementByIndex(8);
                    btnCategSeven.Image = uiPanelElement.Attribute.DisplayImage;
                    break;
            }
            log.LogMethodExit();
        }

        void RenderButton(ScreenModel.UIPanelElement element)
        {
            log.LogMethodEntry(element);
            ResetTimeOut();
            range = element.Parameters[0].UserSelectedValue.ToString();
            LoadSearchProducts();
            log.LogMethodExit();
        }

        private void BtnAll_Click(object sender, EventArgs e)
        {

        }

        private void BtnOne_Click(object sender, EventArgs e)
        {

        }

        private void LblTicketsLoaded_Click(object sender, EventArgs e)
        {

        }

        private void FrmChooseGift_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                if (giftScreenBarcodeScannerDevice != null)
                {
                    giftScreenBarcodeScannerDevice.Register(GiftScreenBarCodeScanCompleteEventHandle);
                }

                if (giftScreenCardReaderDevice != null)
                {
                    giftScreenCardReaderDevice.Register(GiftScreenCardScanCompleteEventHandle);
                }
                if (callSearch) //coming back from viewOrder
                {
                    callSearch = false;
                    LoadSearchProducts();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        void RegisterDevices()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(KioskStatic.Utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "REDEMPTION_KIOSK_DEVICE"));

            List<LookupValuesDTO> redemptionDeviceValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (redemptionDeviceValueList != null && redemptionDeviceValueList[0].LookupValue == "DeviceToEnable")
            {
                if (redemptionDeviceValueList[0].Description == "CARD")
                {
                    giftScreenCardReaderDevice = RedemptionKioskHelper.RegisterCardReader(this, GiftScreenCardScanCompleteEventHandle);
                }
                else if (redemptionDeviceValueList[0].Description == "BARCODE")
                {
                    giftScreenBarcodeScannerDevice = RedemptionKioskHelper.RegisterBarCodeScanner(this, GiftScreenBarCodeScanCompleteEventHandle);
                }
                else if (redemptionDeviceValueList[0].Description == "BOTH")
                {
                    giftScreenCardReaderDevice = RedemptionKioskHelper.RegisterCardReader(this, GiftScreenCardScanCompleteEventHandle);
                    giftScreenBarcodeScannerDevice = RedemptionKioskHelper.RegisterBarCodeScanner(this, GiftScreenBarCodeScanCompleteEventHandle);
                }
                else
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1620, " REDEMPTION_KIOSK_DEVICE" + Common.utils.MessageUtils.getMessage(441)));
                }
            }
            log.LogMethodExit();
        }

        internal void GiftScreenCardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                if (Application.OpenForms[Application.OpenForms.Count - 1].Name == "frmRedemptionKioskChooseGiftScreen")
                {
                    Card card = null;
                    if (e is DeviceScannedEventArgs)
                    {
                        DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                        ResetTimeOut();
                        TagNumber tagNumber;
                        if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                        {
                            string message = tagNumberParser.Validate(checkScannedEvent.Message);
                            Common.ShowMessage(message);
                            log.LogMethodExit(null, "Invalid Tag Number. " + message);
                            return;
                        }

                        string lclCardNumber = tagNumber.Value;
                        lclCardNumber = RedemptionKioskHelper.ReverseTopupCardNumber(lclCardNumber);
                        try
                        {
                            card = RedemptionKioskHelper.HandleCardRead(lclCardNumber, sender as DeviceClass);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                    if (card != null)
                    {
                        try
                        {
                            string mes = redemptionOrder.AddCard(card.CardNumber);
                            if (redemptionPrimaryCard == null)
                                SetPrimaryCardDetails();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            Common.ShowMessage(ex.Message);
                        }
                        RefreshScreen();
                    }
                }
                else
                {
                    log.Error("Card tapped but focus is not in Gift screen");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        internal void GiftScreenBarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                if (Application.OpenForms[Application.OpenForms.Count - 1].Name == "frmRedemptionKioskChooseGiftScreen")
                {
                    if (e is DeviceScannedEventArgs)
                    {
                        DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                        string scannedBarcode = Common.utils.ProcessScannedBarCode(checkScannedEvent.Message, Common.utils.ParafaitEnv.LEFT_TRIM_BARCODE, Common.utils.ParafaitEnv.RIGHT_TRIM_BARCODE);
                        ResetTimeOut();
                        try
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                AddScanedTicketToOrder(RedemptionKioskHelper.ProcessBarcode(scannedBarcode, redemptionOrder));
                                RefreshScreen();
                            });
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }
                else
                {
                    log.Error("Barcode scanned but focus is not in Gift screen");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        void AddScanedTicketToOrder(TicketReceipt ticketReceipt)
        {
            log.LogMethodEntry(ticketReceipt);
            try
            {
                ResetTimeOut();
                redemptionOrder.AddTicket(ticketReceipt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message); 
            }
            log.LogMethodExit();
        }

        void RefreshScreen()
        {
            log.LogMethodEntry();
            LoadSearchProducts();
            UpdateHeader();
            log.LogMethodExit();
        }

        private void FrmChooseGift_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                if (giftScreenCardReaderDevice != null)
                {
                    giftScreenCardReaderDevice.UnRegister();
                    // giftScreenCardReaderDevice.Dispose();
                }

                if (giftScreenBarcodeScannerDevice != null)
                {
                    giftScreenBarcodeScannerDevice.UnRegister();
                    //giftScreenBarcodeScannerDevice.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }



        private void FrmChooseGift_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ShowhideKeypad('H');
                if (giftScreenCardReaderDevice != null)
                {
                    giftScreenCardReaderDevice.UnRegister();
                    giftScreenCardReaderDevice.Dispose();
                }

                if (giftScreenBarcodeScannerDevice != null)
                {
                    giftScreenBarcodeScannerDevice.UnRegister();
                    giftScreenBarcodeScannerDevice.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void SetPrimaryCardDetails()
        {
            log.LogMethodEntry();
            redemptionPrimaryCard = redemptionOrder.GetRedemptionPrimaryCard(null);
            redemptionDiscount = 1;
            if (redemptionPrimaryCard != null)
            {
                redemptionDiscount = redemptionPrimaryCard.GetRedemptionDiscountForMembership();
                if (redemptionDiscount != 1)
                {
                    redemptionDiscount = 1 - redemptionDiscount;
                }
            } 
            log.LogMethodExit();
        }

        void ShowhideKeypad(char mode = 'N') // T for toggle, S for show and H for hide, 'N' for nothing
        {
            log.LogMethodEntry();
            try
            {
                if (keypad == null || keypad.IsDisposed)
                {
                    keypad = new AlphaNumericKeyPad(this, txtSearch, 100);
                    keypad.Location = new Point((this.Width - keypad.Width) / 2, keypad.Height - 40);
                }

                if (mode == 'T')
                {
                    if (keypad.Visible)
                        keypad.Hide();
                    else
                    {
                        //keypad.Location = new Point((this.Width - keypad.Width) / 2, keypad.Height/2);
                        keypad.Show();
                    }
                }
                else if (mode == 'S')
                {
                    //keypad.Location = new Point((this.Width - keypad.Width) / 2, keypad.Height/2);
                    keypad.Show();
                }
                else if (mode == 'H')
                    keypad.Hide();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetTimeOut();
                ShowhideKeypad('S');
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
