/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - Waiver Mapping
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.151.2     29-Dec-2023      Sathyavathi        Created for Waiver Mapping Enhancement
 ********************************************************************************************/
using System;
using System.Data;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using System.Drawing;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Waiver;
using Semnox.Parafait.Customer.Waivers;
using System.Globalization;
using static Parafait_Kiosk.frmCustomerDetailsForWaiver;
using System.Threading.Tasks;
using Semnox.Parafait.Product;

namespace Parafait_Kiosk
{
    public partial class frmMapAttendees : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        private int productNumBeingMapped = 1; //this is index of the products list after grouping and getting distinct products
        private int productIdBeingMapped; //this is index of the products list after grouping and getting distinct products
        private Semnox.Parafait.Transaction.Transaction.TransactionLine lineBeingMappedToCustomer;
        private const string HTMLWAIVERSIGNFORM = "HTMLWAIVERSIGNFORM";
        private UsrCtrlMapAttendeesToQuantity selectedUsrCtrl;
        private List<WaiverCustomerAndSignatureDTO> selectedWaiverAndCustomerSignatureList;
        private List<WaiverCustomerAndSignatureDTO> deviceSignatureList;
        private List<WaiverCustomerAndSignatureDTO> manualSignatureList;
        //private List<WaiverCustomerAndSignatureDTO> htmlSignatureList;
        //private List<CreateCustomerSignedWaiverSetDTO> htmlSignedWaiverSetDTOList = new List<CreateCustomerSignedWaiverSetDTO>();
        private string SIGNATURECHANNEL = WaiverSignatureDTO.WaiverSignatureChannel.KIOSK.ToString();
        private CustomerDTO signatoryCustomerDTO;
        private CustomerDTO signForcustomerDTO;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> firstLineOfEachWaiverProduct;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmMapAttendees(KioskTransaction kioskTransaction, int productNumber = 1)
        {
            log.LogMethodEntry("kioskTransaction");
            KioskStatic.logToFile("In frmMapAttendees()");
            this.kioskTransaction = kioskTransaction;
            this.productNumBeingMapped = (productNumber > -1) ? productNumber : 1;
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetKioskTimerTickValue(30);
            DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.Utilities.setLanguage(this);
            firstLineOfEachWaiverProduct = GetDistinctProductsFromTransactionObject();
            SetDisplayElements();
            log.LogMethodExit();
        }

        private void frmMapAttendees_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                //product number buttons that require waiver signing
                if (firstLineOfEachWaiverProduct.Count > 1)
                {
                    for (int i = productNumBeingMapped - 1; i < firstLineOfEachWaiverProduct.Count; i++)
                    {
                        UsrCtrlMapAttendeesToProduct usrCtrl = new UsrCtrlMapAttendeesToProduct(i + 1, firstLineOfEachWaiverProduct[i].ProductID);
                        if (i == 0)
                        {
                            usrCtrl.SelectProduct();
                            usrCtrl.SetLocation = new Point(0, 0);
                        }
                        else
                        {
                            //to bring the usrctrl to the centre. Otherwise flp by default places it at the top 
                            //which results in misallignment when there is size difference in the usrctrl.
                            usrCtrl.SetLocation = new Point(0, 3);
                        }
                        flpProductIndex.Controls.Add(usrCtrl);
                    }
                }
                else
                {
                    flpProductIndex.Visible = false;
                    lblMsg.TextAlign = ContentAlignment.MiddleCenter;
                    lblProductName.TextAlign = ContentAlignment.MiddleCenter;
                }

                Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine = firstLineOfEachWaiverProduct.FirstOrDefault();
                int qty = kioskTransaction.GetCountOfParticipantsRequireWiaverSigning(trxLine.ProductID);
                if (qty < 1)
                {
                    string errMsg = "Something went wrong. Quantity can't be less than 1";
                    throw new ValidationException(errMsg);
                }
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> linesWithFirstProduct = kioskTransaction.GetWaiverPendingLinesForTheProduct(trxLine.ProductID);
                productIdBeingMapped = linesWithFirstProduct[0].ProductID;
                for (int i = 0; i < qty; i++)
                {
                    //5465 - 'Assign participant for quantity &1'
                    string msg = MessageContainerList.GetMessage(executionContext, 5465, (i + 1).ToString());
                    UsrCtrlMapAttendeesToQuantity usrctrlQty = new UsrCtrlMapAttendeesToQuantity(KioskStatic.Utilities.ExecutionContext, (i + 1), msg);
                    usrctrlQty.Tag = linesWithFirstProduct[i];
                    usrctrlQty.selectedLine += new UsrCtrlMapAttendeesToQuantity.SelectedDelegate(SelctedLine);
                    flpUsrCtrls.Controls.Add(usrctrlQty);
                }

                UsrCtrlMapAttendeesToQuantity usrCtrlDefaultSelected = (UsrCtrlMapAttendeesToQuantity)flpUsrCtrls.Controls[0];
                SetSelectedUsrCtrl(usrCtrlDefaultSelected);
                selectedUsrCtrl.SetBtnTextForeColor();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in frmSelectSlot()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetSelectedUsrCtrl(UsrCtrlMapAttendeesToQuantity usrCtrlDefaultSelected)
        {
            log.LogMethodEntry("usrCtrlDefaultSelected");
            usrCtrlDefaultSelected.IsSelected = true;
            selectedUsrCtrl = usrCtrlDefaultSelected;
            lineBeingMappedToCustomer = (Semnox.Parafait.Transaction.Transaction.TransactionLine)usrCtrlDefaultSelected.Tag;
            log.LogMethodExit();
        }

        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> GetDistinctProductsFromTransactionObject()
        {
            log.LogMethodEntry();

            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> linesRequiringWaiverSigning = kioskTransaction.WaiverRequiredLines();
            if (linesRequiringWaiverSigning != null && linesRequiringWaiverSigning.Any())
            {
                firstLineOfEachWaiverProduct = linesRequiringWaiverSigning.GroupBy(t => new { t.ProductID }).Select(t => t.FirstOrDefault()).ToList();
            }
            log.LogMethodExit(firstLineOfEachWaiverProduct);
            return firstLineOfEachWaiverProduct;
        }

        public void SelctedLine(int selectedTrxLineIndex)
        {
            log.LogMethodEntry(selectedTrxLineIndex);
            try
            {
                ResetKioskTimer();
                SelctUsrCtrl(selectedTrxLineIndex);
                AssignParticipant();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (flpUsrCtrls != null && flpUsrCtrls.Controls != null && flpUsrCtrls.Controls.Count > 0)
                {
                    UsrCtrlMapAttendeesToQuantity usrCtrlDefaultSelected = (UsrCtrlMapAttendeesToQuantity)flpUsrCtrls.Controls[0];
                    SetSelectedUsrCtrl(usrCtrlDefaultSelected);
                }
            }
            log.LogMethodExit();
        }

        private void SelctUsrCtrl(int selectedTrxLineIndex)
        {
            log.LogMethodEntry(selectedTrxLineIndex);

            ResetKioskTimer();
            lineBeingMappedToCustomer = null;
            selectedUsrCtrl = null;
            foreach (UsrCtrlMapAttendeesToQuantity usrCtrl in flpUsrCtrls.Controls)
            {
                usrCtrl.IsSelected = false;
                if (usrCtrl.Line == selectedTrxLineIndex)
                {
                    SetSelectedUsrCtrl(usrCtrl);
                }
            }

            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
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

        private void AssignParticipant()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            StopKioskTimer();
            try
            {
                signatoryCustomerDTO = null;
                signForcustomerDTO = null;
                txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1448);//Loading... Please wait...
                DisableButtons();
                if (lineBeingMappedToCustomer == null)
                {
                    string errMessage = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2460);  //2460 'Please select a record to proceed'
                    throw new ValidationException(errMessage);
                }
                using (frmCustomersAndRelationsList frmCustomerRelations = new frmCustomersAndRelationsList(kioskTransaction, lineBeingMappedToCustomer))
                {
                    DialogResult dr = frmCustomerRelations.ShowDialog();
                    kioskTransaction = frmCustomerRelations.GetKioskTransaction;
                    if (dr == DialogResult.OK)
                    {
                        bool isAllowedToSell = IsAgeProfileOfProductAllowsToSellForThisCustomer(productIdBeingMapped, frmCustomerRelations.SignForcustomerDTO);
                        if (isAllowedToSell)
                        {
                            signatoryCustomerDTO = frmCustomerRelations.SignatoryCustomerDTO;
                            signForcustomerDTO = frmCustomerRelations.SignForcustomerDTO;

                            //Need BL call here as customer would have freshly signed the waiver. We need to build the child records again.
                            CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, signForcustomerDTO.Id, true, true, null);
                            if (lineBeingMappedToCustomer.WaiverSignedDTOList != null && lineBeingMappedToCustomer.WaiverSignedDTOList.Any())
                            {
                                foreach (WaiverSignatureDTO waiverSignedDTO in lineBeingMappedToCustomer.WaiverSignedDTOList)
                                {
                                    if (waiverSignedDTO != null)
                                    {
                                        DateTime trxDate = kioskTransaction.GetTrxDate();
                                        if (lineBeingMappedToCustomer.LineAtb != null)
                                        {
                                            trxDate = lineBeingMappedToCustomer.LineAtb.AttractionBookingDTO.ScheduleFromDate;
                                        }
                                        bool isWaiverSigned = customerBL.HasSigned(waiverSignedDTO.WaiverSetDetailId, trxDate);
                                        if (!isWaiverSigned)
                                        {
                                            SignWaiver(waiverSignedDTO);
                                        }
                                        int lineId = kioskTransaction.GetTrxLines().IndexOf(lineBeingMappedToCustomer);
                                        kioskTransaction.ShowAlertForCustomerSignedWaiverExpiryDate(signForcustomerDTO, lineId);
                                    }
                                }
                                bool isMappingSuccess = MapSelectedCustomerToProductLine();
                                if (isMappingSuccess)
                                {
                                    kioskTransaction.LinkCustomerToTheLineCard(lineBeingMappedToCustomer, signForcustomerDTO.Id);
                                    //Attach mapped customer name agaist the line/quantity
                                    if (signForcustomerDTO != null
                                        && signForcustomerDTO.CustomerSignedWaiverDTOList != null
                                        && signForcustomerDTO.CustomerSignedWaiverDTOList.Exists(csw => lineBeingMappedToCustomer.WaiverSignedDTOList != null
                                        && lineBeingMappedToCustomer.WaiverSignedDTOList.Exists(ws => ws.CustomerSignedWaiverId == csw.CustomerSignedWaiverId)))
                                    {
                                        string mappedCustomerName = signForcustomerDTO.FirstName;
                                        if (!string.IsNullOrWhiteSpace(signForcustomerDTO.LastName))
                                        {
                                            mappedCustomerName += " " + signForcustomerDTO.LastName;
                                        }
                                        selectedUsrCtrl.UpdateBtnParticipantQty(mappedCustomerName);
                                        selectedUsrCtrl.IsMapped = true;
                                        selectedUsrCtrl.IndicateUsrCtrlAsMappingCompleted(true);
                                        selectedUsrCtrl.SetBGImageToIndicateMappingCompleted(ThemeManager.CurrentThemeImages.PersonTickIcon);
                                    }
                                }
                                RefreshUI();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("btnAssign_Click() in frmMapAttendees : " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                txtMessage.Text = lblGreetingMsg.Text;
                if (!kioskTransaction.WaiverMappingIsPending())
                {
                    //5496 - 'Press Complete'
                    txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5496);
                }
                EnableButtons();
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (kioskTransaction.WaiverMappingIsPending())
            {
                //5463 - Assign participant for each quantity of the product
                frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5463));
            }
            else
            {
                txtMessage.Text = string.Empty;
                //5467 - Waivers have been signed by each participant. Proceeding..
                string alertMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5467);
                string buttonText = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "OK");
                frmOKMsg.ShowShortUserMessage(alertMsg, buttonText);
                DialogResult = DialogResult.OK;
                Close();
            }

            log.LogMethodExit();
        }

        private void RefreshUI()
        {
            log.LogMethodEntry();
            StopKioskTimer();
            try
            {
                pnlMapAttendees.SuspendLayout();

                bool isWaiverMappingCompleted = IsWaiverMappingCompletedForTheProduct();
                if (isWaiverMappingCompleted)
                {
                    //Refresh Screen Details Panel i.e Product Number and Product Name being displayed
                    if ((firstLineOfEachWaiverProduct.Count > 1) && (productNumBeingMapped <= flpProductIndex.Controls.Count))
                    {
                        UsrCtrlMapAttendeesToProduct btnPrevProduct = flpProductIndex.Controls[productNumBeingMapped - 1] as UsrCtrlMapAttendeesToProduct;
                        btnPrevProduct.UnselectProduct();
                        productNumBeingMapped++; //move to next product
                        if (productNumBeingMapped <= flpProductIndex.Controls.Count)
                        {
                            //Highlight the next product
                            UsrCtrlMapAttendeesToProduct btnNextProduct = flpProductIndex.Controls[productNumBeingMapped - 1] as UsrCtrlMapAttendeesToProduct;
                            int productId = (int)btnNextProduct.Controls[0].Tag;
                            productIdBeingMapped = productId;
                            btnNextProduct.SelectProduct();

                            RefreshGreetingMsg();
                            RefreshProductNameDisplayed(productId);

                            //Refresh quantity panel
                            flpUsrCtrls.SuspendLayout();
                            flpUsrCtrls.Controls.Clear();

                            CreateUsrCtrlForEachQtyOfTheProductHighlighedAndAddToFLP(productId);
                            SelectDefaultLineToMapCustomer();
                        }
                    }
                }
                else
                {
                    //Auto select the next item
                    AutoSelectTheNextLine();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("frmMapAttendees - RefreshUI : " + ex.Message);
                frmOKMsg.ShowUserMessage("Error while refreshing screen data: " + ex.Message); //sathya, add msg number
            }
            finally
            {
                ResetKioskTimer();
                StartKioskTimer();
                flpUsrCtrls.ResumeLayout();
                pnlMapAttendees.ResumeLayout();
            }
            log.LogMethodExit();
        }

        private void RefreshGreetingMsg()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            lblGreetingMsg.Text = MessageContainerList.GetMessage(executionContext, 5472, MessageContainerList.GetMessage(executionContext, GetText())); //5472 - Assign Participants for your &1 product
            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 5472, MessageContainerList.GetMessage(executionContext, GetText())); //5472 - Assign Participants for your &1 product
            log.LogMethodExit();
        }

        private void AutoSelectTheNextLine()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            int indexOfCurrentItem = flpUsrCtrls.Controls.IndexOf(selectedUsrCtrl);
            selectedUsrCtrl.IsSelected = false;

            int indexOfNextItem = indexOfCurrentItem + 1;
            if (indexOfNextItem > -1 && indexOfNextItem < flpUsrCtrls.Controls.Count)
            {
                selectedUsrCtrl = flpUsrCtrls.Controls[indexOfNextItem] as UsrCtrlMapAttendeesToQuantity;
                SelctUsrCtrl(selectedUsrCtrl.Line);
            }

            if (indexOfNextItem == flpUsrCtrls.Controls.Count)
            {
                foreach (UsrCtrlMapAttendeesToQuantity usrCtrl in flpUsrCtrls.Controls)
                {
                    if (!usrCtrl.IsMapped)
                    {
                        selectedUsrCtrl = usrCtrl;
                        SelctUsrCtrl(selectedUsrCtrl.Line);
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void SelectDefaultLineToMapCustomer()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            UsrCtrlMapAttendeesToQuantity usrCtrlDefaultSelected = (UsrCtrlMapAttendeesToQuantity)flpUsrCtrls.Controls[0];
            usrCtrlDefaultSelected.IsSelected = true;
            selectedUsrCtrl = usrCtrlDefaultSelected;
            lineBeingMappedToCustomer = (Semnox.Parafait.Transaction.Transaction.TransactionLine)usrCtrlDefaultSelected.Tag;
            log.LogMethodExit();
        }

        private void CreateUsrCtrlForEachQtyOfTheProductHighlighedAndAddToFLP(int productId)
        {
            log.LogMethodEntry(productId);
            ResetKioskTimer();
            int qty = kioskTransaction.GetActiveTransactionLines.Count(t => t.ProductID == productId);
            if (qty < 1)
            {
                string errMsg = "Something went wrong. Quantity can't be less than 1";
                throw new ValidationException(errMsg);
            }
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> linesWithSelectedProduct = kioskTransaction.GetActiveTransactionLines.Where(t => t.ProductID == productId).ToList();
            for (int i = 0; i < qty; i++)
            {
                //5465 - 'Assign participant for quantity &1'
                string msg = MessageContainerList.GetMessage(executionContext, 5465, (i + 1).ToString()); //Literal - Quantity
                UsrCtrlMapAttendeesToQuantity usrctrlQty = new UsrCtrlMapAttendeesToQuantity(KioskStatic.Utilities.ExecutionContext, (i + 1), msg);
                usrctrlQty.Tag = linesWithSelectedProduct[i];
                usrctrlQty.selectedLine += new UsrCtrlMapAttendeesToQuantity.SelectedDelegate(SelctedLine);
                flpUsrCtrls.Controls.Add(usrctrlQty);
            }
            log.LogMethodExit();
        }

        private void RefreshProductNameDisplayed(int productId)
        {
            log.LogMethodEntry(productId);
            ResetKioskTimer();
            string productName = string.Empty;
            Semnox.Parafait.Transaction.Transaction.TransactionLine line = kioskTransaction.GetActiveTransactionLines.FirstOrDefault(t => t.ProductID == productId);
            if (line != null)
            {
                productName = line.ProductName;
            }
            lblProductName.Text = productName;
            log.LogMethodExit();
        }

        private bool IsWaiverMappingCompletedForTheProduct()
        {
            log.LogMethodEntry();
            bool isWaiverMappingCompleted = false;
            if (productNumBeingMapped <= flpProductIndex.Controls.Count)
            {
                int productId = firstLineOfEachWaiverProduct[productNumBeingMapped - 1].ProductID; //product number started from 1
                bool isSigningRequired = kioskTransaction.IsWaiverSigningRequiredForTheProduct(productId);
                if (!isSigningRequired)
                {
                    isWaiverMappingCompleted = true;
                }
            }
            else
            {
                isWaiverMappingCompleted = true;
            }
            log.LogMethodExit(isWaiverMappingCompleted);
            return isWaiverMappingCompleted;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                DialogResult = DialogResult.No;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnBack_Click() of Select Slot Screen" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void bigVerticalScrollView_ButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error in bigVerticalScrollView_ButtonClick", ex);
            }
            log.LogMethodExit();
        }

        private void SetDisplayElements()
        {
            log.LogMethodEntry();
            try
            {
                SetOnScreenMessages();
                SetCustomImages();
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetDisplayElements of frmMapAttendees" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetOnScreenMessages()
        {
            log.LogMethodEntry();

            lblGreetingMsg.Text = MessageContainerList.GetMessage(executionContext, 5472,
                firstLineOfEachWaiverProduct.Count == 1 ? "" : MessageContainerList.GetMessage(executionContext, "1st")); //5472 - Assign Participants for your &1 product
            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 5472,
                firstLineOfEachWaiverProduct.Count == 1 ? "" : MessageContainerList.GetMessage(executionContext, "1st")); //5472 - Assign Participants for your &1 product
            lblAssignParticipants.Text = txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Assign Participant"); //Literal
            lblProductName.Text = firstLineOfEachWaiverProduct[0].ProductName;
            lblMsg.Text = MessageContainerList.GetMessage(executionContext, 5463);  //5463 -Assign participant for each quantity of the product
            lblQuantity.Text = MessageContainerList.GetMessage(executionContext, "Quantity"); //Literal
            btnComplete.Text = MessageContainerList.GetMessage(executionContext, "Complete"); //Literal

            log.LogMethodExit();
        }

        private string GetText()
        {
            log.LogMethodEntry();
            string text = string.Empty;
            switch (firstLineOfEachWaiverProduct.Count)
            {
                case 1: text = MessageContainerList.GetMessage(executionContext, ""); break;
                case 2: text = MessageContainerList.GetMessage(executionContext, "2nd"); break;
                case 3: text = MessageContainerList.GetMessage(executionContext, "3rd"); break;
                default: text = MessageContainerList.GetMessage(executionContext, "1st"); break;
            }
            log.LogMethodExit(text);
            return text;
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.SelectSlotBackgroundImage);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnPrev.BackgroundImage = btnCancel.BackgroundImage = btnComplete.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                this.bigVerticalScrollView.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                pnlMapAttendees.BackgroundImage = ThemeManager.CurrentThemeImages.PanelTimeSection;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized background images for frmSelectSlot", ex);
                KioskStatic.logToFile("Error while setting customized background images for frmSelectSlot: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                lblGreetingMsg.ForeColor = KioskStatic.CurrentTheme.MapAttendeesLblGreetingTextForeColor;
                lblProductName.ForeColor = KioskStatic.CurrentTheme.MapAttendeesLblProductNameTextForeColor;
                lblMsg.ForeColor = KioskStatic.CurrentTheme.MapAttendeesLblMsgTextForeColor;
                btnPrev.ForeColor = KioskStatic.CurrentTheme.MapAttendeestBackButtonTextForeColor;
                btnCancel.ForeColor = KioskStatic.CurrentTheme.MapAttendeesCancelButtonTextForeColor;
                btnComplete.ForeColor = KioskStatic.CurrentTheme.MapAttendeesAssignButtonTextForeColor;
                txtMessage.ForeColor = KioskStatic.CurrentTheme.MapAttendeesFooterTextForeColor;
                lblQuantity.ForeColor = KioskStatic.CurrentTheme.MapAttendeesLblQuantityTextForeColor;
                lblAssignParticipants.ForeColor = KioskStatic.CurrentTheme.MapAttendeesLblAssignParticipantsTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.SelectSlotHomeButtonTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors for frmSelectSlot", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements of frmSelectSlot: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void frmMapAttendees_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmSelectSlot_FormClosed()", ex);
            }
            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            pnlMapAttendees.SuspendLayout();
            this.btnComplete.Enabled = false;
            this.btnPrev.Enabled = false;
            this.btnCancel.Enabled = false;
            flpUsrCtrls.Enabled = false;
            log.LogMethodExit();
        }

        private void EnableButtons()
        {
            log.LogMethodEntry();

            pnlMapAttendees.ResumeLayout();
            this.btnComplete.Enabled = true;
            this.btnPrev.Enabled = true;
            this.btnCancel.Enabled = true;
            flpUsrCtrls.Enabled = true;
            log.LogMethodExit();
        }

        private void SignWaiver(WaiverSignatureDTO waiverSignedDTO)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ResetKioskTimer();
                if (signForcustomerDTO == null)
                {
                    //'Please select customer record(s) for signing waiver'
                    throw new ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2426));
                }
                else
                {
                    WaiverSetContainer waiverSetContainer = WaiverSetContainer.GetInstance;
                    List<WaiverSetDTO> waiverSetDTOListTemp = waiverSetContainer.GetWaiverSetDTOList(KioskStatic.Utilities.ExecutionContext.SiteId);
                    WaiverSetDTO waiverSetDTO = waiverSetDTOListTemp.FirstOrDefault(wl => wl.WaiverSetId == waiverSignedDTO.WaiverSetId);
                    if (waiverSetDTO != null)
                    {
                        CreateWaiverCustomerMapList(waiverSetDTO);
                        GetSignatureAndSave(waiverSetDTO);
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void CreateWaiverCustomerMapList(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            this.Cursor = Cursors.WaitCursor;
            ResetKioskTimer();
            CreateWaiverCustomerAndSignatureDTOList(waiverSetDTO);
            UpdateListBasedOnCustomerSignatureStatus(waiverSetDTO);
            log.LogMethodExit();
        }

        private void CreateWaiverCustomerAndSignatureDTOList(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            selectedWaiverAndCustomerSignatureList = new List<WaiverCustomerAndSignatureDTO>();
            foreach (WaiversDTO waiversDTO in waiverSetDTO.WaiverSetDetailDTOList)
            {
                WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO = new WaiverCustomerAndSignatureDTO();
                waiverCustomerAndSignatureDTO.WaiversDTO = waiversDTO;
                waiverCustomerAndSignatureDTO.SignatoryCustomerDTO = signatoryCustomerDTO;
                waiverCustomerAndSignatureDTO.SignForCustomerDTOList = new List<CustomerDTO>() { signForcustomerDTO };
                waiverCustomerAndSignatureDTO.CustIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                waiverCustomerAndSignatureDTO.CustomerContentDTOList = null;
                selectedWaiverAndCustomerSignatureList.Add(waiverCustomerAndSignatureDTO);
            }
            log.LogMethodExit();
        }

        private void UpdateListBasedOnCustomerSignatureStatus(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            if (signForcustomerDTO != null)
            {
                CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, signForcustomerDTO);
                List<WaiversDTO> pendingWaiversList = customerBL.GetSignaturePendingWaivers(waiverSetDTO.WaiverSetDetailDTOList, kioskTransaction.GetTrxDate());
                if (pendingWaiversList == null || pendingWaiversList.Count != waiverSetDTO.WaiverSetDetailDTOList.Count)
                {
                    List<WaiversDTO> signedWaivers = new List<WaiversDTO>();
                    if (pendingWaiversList == null || pendingWaiversList.Any() == false)
                    {
                        signedWaivers = new List<WaiversDTO>(waiverSetDTO.WaiverSetDetailDTOList);
                    }
                    else
                    {
                        for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
                        {
                            if (pendingWaiversList.Exists(waiver => waiver.WaiverSetDetailId == selectedWaiverAndCustomerSignatureList[i].WaiversDTO.WaiverSetDetailId) == false)
                            {
                                signedWaivers.Add(selectedWaiverAndCustomerSignatureList[i].WaiversDTO);
                            }
                        }
                    }
                    if (signedWaivers != null && signedWaivers.Any())
                    {
                        CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(KioskStatic.Utilities.ExecutionContext);
                        List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParameters = new List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_FOR, signForcustomerDTO.Id.ToString()));
                        searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IS_ACTIVE, "1"));
                        searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IGNORE_EXPIRED, kioskTransaction.GetTrxDate().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(searchParameters);
                        if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                        {
                            for (int i = 0; i < signedWaivers.Count; i++)
                            {
                                List<CustomerSignedWaiverDTO> signedWaiverCopyList = customerSignedWaiverDTOList.Where(cw => cw.WaiverSetDetailId == signedWaivers[i].WaiverSetDetailId).ToList();
                                if (signedWaiverCopyList != null && signedWaiverCopyList.Any())
                                {
                                    foreach (CustomerSignedWaiverDTO item in signedWaiverCopyList)
                                    {
                                        if (!CanDeactivate(item))
                                        {
                                            log.Info("remove entry from signedWaivers: " + signedWaivers[i].WaiverSetDetailId);
                                            this.Cursor = Cursors.WaitCursor;
                                            using (frmOKMsg frmOk = new frmOKMsg(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2351,
                                                MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "re-sign"), signedWaivers[i].Name)))
                                            {
                                                frmOk.ShowDialog();
                                            }
                                            this.Cursor = Cursors.WaitCursor;
                                            signedWaivers.RemoveAt(i);
                                            i = i - 1;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    this.Cursor = Cursors.WaitCursor;
                    using (frmYesNo frmYes = new frmYesNo(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2485, customerBL.CustomerDTO.FirstName + " " + (string.IsNullOrEmpty(customerBL.CustomerDTO.LastName) ? string.Empty : customerBL.CustomerDTO.LastName))))
                    {
                        this.Cursor = Cursors.WaitCursor;
                        //"&1 has signed one or more selected waivers. Do you want to re-sign them?"
                        if ((signedWaivers != null && signedWaivers.Any())
                            && frmYes.ShowDialog() == DialogResult.Yes)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "WAIVER_DEACTIVATION_NEEDS_MANAGER_APPROVAL", true))
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Manager Approval Required.")
                                                              + " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 441));
                            }
                        }
                        else
                        {
                            this.Cursor = Cursors.WaitCursor;
                            for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
                            {
                                if (pendingWaiversList != null &&
                                    pendingWaiversList.Exists(waiver => waiver.WaiverSetDetailId == selectedWaiverAndCustomerSignatureList[i].WaiversDTO.WaiverSetDetailId) == false)
                                {
                                    selectedWaiverAndCustomerSignatureList[i].SignForCustomerDTOList.Remove(signForcustomerDTO);
                                }
                            }
                        }
                    }
                }
            }
            this.Cursor = Cursors.WaitCursor;
            for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
            {
                if (selectedWaiverAndCustomerSignatureList[i].SignForCustomerDTOList == null || selectedWaiverAndCustomerSignatureList[i].SignForCustomerDTOList.Any() == false)
                {
                    selectedWaiverAndCustomerSignatureList.RemoveAt(i);
                    i = i - 1;
                }
            }
            for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
            {
                selectedWaiverAndCustomerSignatureList[i] = WaiverCustomerAndSignatureBL.CreateCustomerContentForWaiver(KioskStatic.Utilities.ExecutionContext, selectedWaiverAndCustomerSignatureList[i]);
            }
            log.LogMethodExit();
        }

        private bool CanDeactivate(CustomerSignedWaiverDTO customerSignedWaiverDTO)
        {
            log.LogMethodEntry(customerSignedWaiverDTO);
            bool canDeactivate = true;
            if (customerSignedWaiverDTO != null && customerSignedWaiverDTO.CustomerSignedWaiverId > -1)
            {
                TransactionListBL transactionListBL = new TransactionListBL(KioskStatic.Utilities.ExecutionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_SIGNED_WAIVER_ID, customerSignedWaiverDTO.CustomerSignedWaiverId.ToString()));
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS_NOT_IN, Semnox.Parafait.Transaction.Transaction.TrxStatus.CANCELLED.ToString() + "," + Semnox.Parafait.Transaction.Transaction.TrxStatus.SYSTEMABANDONED.ToString()));
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, kioskTransaction.GetTrxDate().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, KioskStatic.Utilities.ExecutionContext.SiteId.ToString()));

                List<TransactionDTO> transactionDTOList = transactionListBL.GetTransactionDTOList(searchParam, KioskStatic.Utilities);
                log.LogVariableState("transactionDTOList", transactionDTOList);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    canDeactivate = false;
                }
            }
            log.LogMethodExit(canDeactivate);
            return canDeactivate;
        }

        private void GetSignatureAndSave(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            SplitBySigningOption(waiverSetDTO);
            if (manualSignatureList != null && manualSignatureList.Any())
            {
                throw new ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2345, waiverSetDTO.Description, MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Sign")));
            }
            DeviceWaivers();
            //HTMLWaivers();
            SaveCustomerSignedWaivers();
            log.LogMethodExit();
        }

        private void SplitBySigningOption(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            deviceSignatureList = null;
            manualSignatureList = null;
            //htmlSignatureList = null;
            bool device = false;
            //bool html = false;
            bool manual = false;
            if (waiverSetDTO.WaiverSetSigningOptionDTOList != null)
            {
                foreach (WaiverSetSigningOptionsDTO item in waiverSetDTO.WaiverSetSigningOptionDTOList)
                {
                    if (item.OptionName == WaiverSetSigningOptionsDTO.WaiverSigningOptions.DEVICE.ToString()
                        || item.OptionName == WaiverSetSigningOptionsDTO.WaiverSigningOptions.ONLINE.ToString())
                    {
                        if (waiverSetDTO.WaiverSetDetailDTOList != null && waiverSetDTO.WaiverSetDetailDTOList.Any())
                        {
                            if (waiverSetDTO.WaiverSetDetailDTOList.Exists(w => w.IsActive && !string.IsNullOrWhiteSpace(w.WaiverFileName))
                               /* && waiverSetDTO.WaiverSetDetailDTOList.Exists(w => w.IsActive && !string.IsNullOrWhiteSpace(w.WaiverHTML))*/)
                            {
                                ValidationException ve = new ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext,
                                                                                      "Waiver set &1 cannot have both PDF and HTML waivers", waiverSetDTO.Name));
                            }

                            device = waiverSetDTO.WaiverSetDetailDTOList.Exists(w => w.IsActive && !string.IsNullOrWhiteSpace(w.WaiverFileName));
                            //&& waiverSetDTO.WaiverSetDetailDTOList.Exists(w => w.IsActive && string.IsNullOrWhiteSpace(w.WaiverHTML));

                            //html = waiverSetDTO.WaiverSetDetailDTOList.Exists(w => w.IsActive && string.IsNullOrWhiteSpace(w.WaiverFileName))
                            //  && waiverSetDTO.WaiverSetDetailDTOList.Exists(w => w.IsActive && !string.IsNullOrWhiteSpace(w.WaiverHTML));
                        }
                    }
                    if (item.OptionName == WaiverSetSigningOptionsDTO.WaiverSigningOptions.MANUAL.ToString())
                    {
                        manual = true;
                    }
                }
            }
            if (device && manual || device)
            {
                deviceSignatureList = new List<WaiverCustomerAndSignatureDTO>();
                deviceSignatureList = selectedWaiverAndCustomerSignatureList;
            }
            //else if (html && manual || html)
            //{
            //    htmlSignatureList = new List<WaiverCustomerAndSignatureDTO>();
            //    htmlSignatureList = selectedWaiverAndCustomerSignatureList;
            //}
            else if (manual)
            {
                manualSignatureList = new List<WaiverCustomerAndSignatureDTO>();
                manualSignatureList = selectedWaiverAndCustomerSignatureList;
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2308, waiverSetDTO.Name));
                //Please check the waiver signing option for &1
            }
            log.LogMethodExit();
        }

        private void DeviceWaivers()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            if (deviceSignatureList != null && deviceSignatureList.Any())
            {
                this.Cursor = Cursors.WaitCursor;
                using (frmSignWaiverFiles frmSignWaiverFiles = new frmSignWaiverFiles(deviceSignatureList))
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (frmSignWaiverFiles.ShowDialog() == DialogResult.OK)
                    {
                        deviceSignatureList = frmSignWaiverFiles.GetSignedDTOList;
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1007));
                    }
                    this.Cursor = Cursors.WaitCursor;
                }
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }

        //private void HTMLWaivers()
        //{
        //    log.LogMethodEntry();
        //    ResetKioskTimer();
        //    if (htmlSignatureList != null && htmlSignatureList.Any())
        //    {
        //        this.Cursor = Cursors.WaitCursor;
        //        Screen[] sc;
        //        sc = Screen.AllScreens;
        //        try
        //        {
        //            htmlSignedWaiverSetDTOList = new List<CreateCustomerSignedWaiverSetDTO>();
        //            int waiverSetId = -1;
        //            int totalCount = 0;
        //            int currentFile = 1;
        //            CreateCustomerWaiverSetDTO createCustomerWaiverSetDTO = null;
        //            foreach (WaiverCustomerAndSignatureDTO item in htmlSignatureList)
        //            {
        //                if (item.WaiversDTO.WaiverSetId != waiverSetId)
        //                {
        //                    ICustomerWaiverUseCases customerWaiverUseCases = CustomerWaiverUseCaseFactory.GetCustomerWaiverUseCases(KioskStatic.Utilities.ExecutionContext);
        //                    using (NoSynchronizationContextScope.Enter())
        //                    {
        //                        CreateCustomerWaiverSetDocumentDTO createCustomerWaiverSetDocumentDTO = new CreateCustomerWaiverSetDocumentDTO();
        //                        createCustomerWaiverSetDocumentDTO.Channel = WaiverSignatureDTO.WaiverSignatureChannel.KIOSK.ToString();
        //                        createCustomerWaiverSetDocumentDTO.SignatoryGuestCustomerDTO = null;
        //                        createCustomerWaiverSetDocumentDTO.SignForGuestCustomersDTOList = null;
        //                        createCustomerWaiverSetDocumentDTO.SignForCustomersIdList = new List<int>();
        //                        createCustomerWaiverSetDocumentDTO.SignForCustomersIdList.AddRange(item.SignForCustomerDTOList.Select(sfc => sfc.Id).ToList());
        //                        Task<CreateCustomerWaiverSetDTO> createCustomerWaiverSetDTOTask = customerWaiverUseCases.CreateCustomerWaiverForm(item.WaiversDTO.WaiverSetId, item.SignatoryCustomerDTO.Id, false, createCustomerWaiverSetDocumentDTO);
        //                        createCustomerWaiverSetDTOTask.Wait();
        //                        createCustomerWaiverSetDTO = createCustomerWaiverSetDTOTask.Result;
        //                        totalCount = (createCustomerWaiverSetDTO.CreateCustomerWaiverDTOList != null
        //                                              ? createCustomerWaiverSetDTO.CreateCustomerWaiverDTOList.Count
        //                                              : 0);
        //                    };
        //                    waiverSetId = item.WaiversDTO.WaiverSetId;
        //                    CreateCustomerSignedWaiverSetDTO htmlSignedWaiverSetDTO = null;
        //                    htmlSignedWaiverSetDTO = new CreateCustomerSignedWaiverSetDTO(waiverSetId, item.SignatoryCustomerDTO.Id,
        //                               WaiverSignatureDTO.WaiverSignatureChannel.POS.ToString(), -1, null);
        //                    htmlSignedWaiverSetDTOList.Add(htmlSignedWaiverSetDTO);
        //                }
        //                if (createCustomerWaiverSetDTO != null && createCustomerWaiverSetDTO.CreateCustomerWaiverDTOList != null && createCustomerWaiverSetDTO.CreateCustomerWaiverDTOList.Any())
        //                {
        //                    List<CreateCustomerWaiverDTO> createCustomerWaiverDTOList = createCustomerWaiverSetDTO.CreateCustomerWaiverDTOList.Where(cw => cw.WaiverSetDetailId == item.WaiversDTO.WaiverSetDetailId && cw.WaiverSetId == item.WaiversDTO.WaiverSetId).ToList();

        //                    if (createCustomerWaiverDTOList != null && createCustomerWaiverDTOList.Any())
        //                    {
        //                        foreach (CreateCustomerWaiverDTO createCustomerWaiverDTO in createCustomerWaiverDTOList)
        //                        {
        //                            if (createCustomerWaiverDTO.SigningForCustomerDTOList != null && createCustomerWaiverDTO.SigningForCustomerDTOList.Any())
        //                            {
        //                                createCustomerWaiverDTO.SignForCustomersIdList = createCustomerWaiverDTO.SigningForCustomerDTOList.Select(cu => cu.CustomerId).ToList();
        //                            }
        //                            string htmlData = GenericUtils.ConvertBase64ToString(createCustomerWaiverDTO.CustomerWaiverHTMLForm);
        //                            string msg1 = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Showing") + " ";
        //                            string msg2 = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4776, currentFile, totalCount);
        //                            string msg3 = " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "files");
        //                            currentFile++;

        //                            Size labelSize = new Size(sc[0].Bounds.Width - 3, 50);
        //                            Point labelLocation = new Point(0, 0);
        //                            Size browserSize = new Size(sc[0].Bounds.Width - 3, sc[0].Bounds.Height - 3 - 50);
        //                            Point browserLocation = new Point(2, 2 + 50);
        //                            CustomWebBrowser customWebBrowser = new CustomWebBrowser(KioskStatic.Utilities.ExecutionContext, string.Empty, htmlData,
        //                                string.Empty, browserSize, browserLocation, BrowserNavigationCompleted);
        //                            using (HtmlWaiverrForm waiverForm = new HtmlWaiverrForm())
        //                            {
        //                                waiverForm.SuspendLayout();
        //                                Label lblMessage = new Label();
        //                                lblMessage.Text = msg1 + msg2 + msg3;
        //                                lblMessage.BackColor = Color.Transparent;
        //                                lblMessage.Location = labelLocation;
        //                                lblMessage.TextAlign = ContentAlignment.MiddleCenter;
        //                                lblMessage.Size = labelSize;
        //                                lblMessage.Font = this.lblGreetingMsg.Font;
        //                                lblMessage.ForeColor = this.lblGreetingMsg.ForeColor;
        //                                waiverForm.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Waiver Agreement");
        //                                waiverForm.Tag = createCustomerWaiverDTO;
        //                                waiverForm.Name = HTMLWAIVERSIGNFORM;
        //                                waiverForm.FormBorderStyle = FormBorderStyle.None;
        //                                waiverForm.BackgroundImage = ThemeManager.CurrentThemeImages.DefaultBackgroundImageTwo;
        //                                waiverForm.Left = sc[0].Bounds.Width;
        //                                waiverForm.Top = sc[0].Bounds.Height;
        //                                waiverForm.StartPosition = FormStartPosition.Manual;
        //                                waiverForm.Location = sc[0].Bounds.Location;
        //                                Point p = new Point(sc[0].Bounds.Location.X, sc[0].Bounds.Location.Y);
        //                                waiverForm.Location = p;
        //                                waiverForm.Size = new Size(sc[0].Bounds.Width - 1, sc[0].Bounds.Height - 1);
        //                                waiverForm.WindowState = FormWindowState.Normal;
        //                                Control ctrlObj = customWebBrowser.GetWebViewControl();
        //                                waiverForm.Controls.Add(lblMessage);
        //                                waiverForm.Controls.Add(ctrlObj);
        //                                waiverForm.ResumeLayout(true);
        //                                DialogResult statusResult = waiverForm.ShowDialog();
        //                                if (statusResult == DialogResult.Cancel)
        //                                {
        //                                    throw new ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1007));
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            this.Cursor = Cursors.Default;
        //        }
        //    }
        //    ResetKioskTimer();
        //    log.LogMethodExit();
        //}

        //private void BrowserNavigationCompleted(Uri uri, Control parentCtrl)
        //{
        //    log.LogMethodEntry(uri, (parentCtrl != null ? parentCtrl.Name : ""));
        //    try
        //    {
        //        if (uri != null && uri.AbsoluteUri.Contains("status=success"))
        //        {
        //            //createCustomerWaiverDTO
        //            if (parentCtrl != null && parentCtrl is Form && ((Form)parentCtrl).Name == HTMLWAIVERSIGNFORM)
        //            {
        //                if (parentCtrl.Tag != null)
        //                {
        //                    CreateCustomerWaiverDTO custWaiverDTO = (CreateCustomerWaiverDTO)parentCtrl.Tag;
        //                    foreach (CreateCustomerSignedWaiverSetDTO item in htmlSignedWaiverSetDTOList)
        //                    {
        //                        if (item.WaiverSetId == custWaiverDTO.WaiverSetId)
        //                        {
        //                            string responseData = uri.AbsoluteUri;
        //                            int startPosition = responseData.IndexOf("data=");
        //                            string signaturesAndAcceptanceEncoded = responseData.Substring(startPosition + 5);
        //                            CreateCustomerSignedWaiverDTO waiverDTO = null;
        //                            waiverDTO = new CreateCustomerSignedWaiverDTO(custWaiverDTO.WaiverSetId,
        //                               custWaiverDTO.WaiverSetDetailId, custWaiverDTO.DocumentIdentifier, custWaiverDTO.SignForCustomersIdList,
        //                               null, signaturesAndAcceptanceEncoded);
        //                            if (item.CreateCustomerSignedWaiverDTOList == null)
        //                            {
        //                                item.CreateCustomerSignedWaiverDTOList = new List<CreateCustomerSignedWaiverDTO>();
        //                            }
        //                            item.CreateCustomerSignedWaiverDTOList.Add(waiverDTO);
        //                        }
        //                    }
        //                }
        //                ((Form)parentCtrl).DialogResult = DialogResult.OK;
        //                ((Form)parentCtrl).Close();
        //            }
        //        }
        //        else if (uri != null && uri.AbsoluteUri.Contains("status=cancel"))
        //        {
        //            if (parentCtrl != null && parentCtrl is Form)
        //            {
        //                htmlSignedWaiverSetDTOList = new List<CreateCustomerSignedWaiverSetDTO>();
        //                ((Form)parentCtrl).DialogResult = DialogResult.Cancel;
        //                ((Form)parentCtrl).Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //    }
        //    log.LogMethodExit();
        //}

        private void SaveCustomerSignedWaivers()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            StopKioskTimer();
            try
            {
                List<CustomerDTO> signForCustDTOList = new List<CustomerDTO>();
                List<WaiverCustomerAndSignatureDTO> receivedSignatureDetailsList = new List<WaiverCustomerAndSignatureDTO>();
                if (deviceSignatureList != null && deviceSignatureList.Any())
                {
                    receivedSignatureDetailsList.AddRange(deviceSignatureList);
                }
                if (receivedSignatureDetailsList != null && receivedSignatureDetailsList.Any())
                {
                    CustomerBL signatoryCustomerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, receivedSignatureDetailsList[0].SignatoryCustomerDTO);
                    ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    int custSignedWaiverHeaderId = -1;
                    try
                    {
                        custSignedWaiverHeaderId = signatoryCustomerBL.CreateCustomerSignedWaiverHeader(SIGNATURECHANNEL, dBTransaction.SQLTrx);
                        bool customerIsAnAdult = signatoryCustomerBL.IsAdult();
                        int guardianId = (customerIsAnAdult ? signatoryCustomerBL.CustomerDTO.Id : -1);
                        foreach (WaiverCustomerAndSignatureDTO custSignatureDetails in receivedSignatureDetailsList)
                        {
                            foreach (CustomerDTO signForCustomerDTO in custSignatureDetails.SignForCustomerDTOList)
                            {
                                CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, signForCustomerDTO);
                                int mgrId = -1;
                                customerBL.CreateCustomerSignedWaiver(custSignatureDetails.WaiversDTO, custSignedWaiverHeaderId, custSignatureDetails.CustomerContentDTOList, custSignatureDetails.CustIdNameSignatureImageList, mgrId, KioskStatic.Utilities, guardianId);
                                CustomerStatic.CheckForTermsAndConditions(customerBL.CustomerDTO);
                                customerBL.Save(dBTransaction.SQLTrx);
                                if (signForCustDTOList.Exists(cust => cust.Id == customerBL.CustomerDTO.Id) == false)
                                {
                                    signForCustDTOList.Add(customerBL.CustomerDTO);
                                }
                            }
                        }
                        dBTransaction.EndTransaction();
                        string waiverCode = signatoryCustomerBL.GetWaiverCode(custSignedWaiverHeaderId);
                        string successMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1197,
                                                      MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Waivers"))
                                                      + ". " + (string.IsNullOrEmpty(waiverCode) ? string.Empty : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2432, waiverCode));
                        using (frmOKMsg frmMsg = new frmOKMsg(successMsg))
                        {
                            frmMsg.ShowDialog();
                        }
                        this.Cursor = Cursors.WaitCursor;

                        txtMessage.Text = successMsg;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        dBTransaction.RollBack();
                        throw;
                    }
                    try
                    {
                        SendWaiverEmail(signatoryCustomerBL.CustomerDTO, custSignedWaiverHeaderId);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        KioskStatic.logToFile("Send Waiver Email: " + ex.Message);
                        txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1824, ex.Message);
                        using (frmOKMsg frmMsg = new frmOKMsg(ex.Message))
                        {
                            frmMsg.ShowDialog();
                        }
                        this.Cursor = Cursors.WaitCursor;
                    }
                }
                //if (htmlSignatureList != null && htmlSignatureList.Any())
                //{
                //    foreach (WaiverCustomerAndSignatureDTO item in htmlSignatureList)
                //    {
                //        if (item.SignForCustomerDTOList != null && item.SignForCustomerDTOList.Any())
                //        {
                //            foreach (CustomerDTO custItem in item.SignForCustomerDTOList)
                //            {
                //                signForCustDTOList.Add(custItem);
                //            }
                //        }
                //    }
                //    if (htmlSignedWaiverSetDTOList != null && htmlSignedWaiverSetDTOList.Any())
                //    {
                //        List<string> msgList = new List<string>();
                //        CustomerBL signatoryCustBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, htmlSignatureList[0].SignatoryCustomerDTO);
                //        foreach (CreateCustomerSignedWaiverSetDTO item in htmlSignedWaiverSetDTOList)
                //        {
                //            try
                //            {

                //                ICustomerWaiverUseCases customerWaiverUseCases = CustomerWaiverUseCaseFactory.GetCustomerWaiverUseCases(KioskStatic.Utilities.ExecutionContext);
                //                using (NoSynchronizationContextScope.Enter())
                //                {
                //                    Task<string> signatureTask = customerWaiverUseCases.SignCustomerWaiverForm(item.WaiverSetId, item.SignatoryCustomerId,
                //                        item);
                //                    signatureTask.Wait();
                //                    string msg = signatureTask.Result;
                //                    msgList.Add(msg);
                //                };
                //            }
                //            catch (Exception ex)
                //            {
                //                log.Error(ex);
                //                throw;
                //            }
                //        }
                //        try
                //        {
                //            foreach (string item in msgList)
                //            {
                //                string successMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1197,
                //                                              MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Waivers"))
                //                                              + ". " + (string.IsNullOrEmpty(item) ? string.Empty : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2432, item));
                //                using (frmOKMsg frmMsg = new frmOKMsg(successMsg))
                //                {
                //                    frmMsg.ShowDialog();
                //                }

                //                txtMessage.Text = successMsg;
                //            }
                //            this.Cursor = Cursors.WaitCursor;
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Error(ex);
                //            frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1824, ex.Message));
                //        }
                //    }
                //}
            }
            finally
            {
                this.Cursor = Cursors.Default;
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void SendWaiverEmail(CustomerDTO customerDTO, int custSignedWaiverHeaderId)
        {
            log.LogMethodEntry(customerDTO, custSignedWaiverHeaderId);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            if (customerDTO != null && customerDTO.Id > -1 /*&& customerDTO.Id != guestCustomerId*/)
            {
                CustomerSignedWaiverHeaderListBL customerSignedWaiverHeaderListBL = new CustomerSignedWaiverHeaderListBL(KioskStatic.Utilities.ExecutionContext);
                customerSignedWaiverHeaderListBL.SendWaiverEmail(customerDTO, custSignedWaiverHeaderId, KioskStatic.Utilities, null);
            }
            log.LogMethodExit();
        }

        private bool MapSelectedCustomerToProductLine()
        {
            log.LogMethodEntry();
            bool isMappingSuccess = true;

            Semnox.Parafait.Transaction.Transaction.TransactionLine trxLineinProgress = lineBeingMappedToCustomer;
            if (signForcustomerDTO != null && signForcustomerDTO.Id > -1)
            {
                string licenseRequiredType = trxLineinProgress.LicenseType;
                if (!String.IsNullOrEmpty(licenseRequiredType) && kioskTransaction.ValidateLicenseTypeInWaiver(trxLineinProgress, signForcustomerDTO) == false)
                {
                    log.Error("Customer or Card does not have a valid license.");
                    isMappingSuccess = false;
                    log.LogMethodExit(isMappingSuccess);
                    return isMappingSuccess;
                }

                int lineId = kioskTransaction.GetTrxLines().IndexOf(trxLineinProgress); //We need to get all lines including non active lines so that we get correct line index.
                if (signForcustomerDTO != null && signForcustomerDTO.Id > -1)
                {
                    kioskTransaction.MapCustomerWaiversToLine(lineId, signForcustomerDTO);
                }
            }
            if (trxLineinProgress != null && trxLineinProgress.WaiverSignedDTOList != null)
            {
                foreach (WaiverSignatureDTO ws in trxLineinProgress.WaiverSignedDTOList)
                {
                    if (ws.CustomerSignedWaiverId == -1)
                    {
                        isMappingSuccess = false;
                    }
                }
            }
            log.LogMethodExit(isMappingSuccess);
            return isMappingSuccess;
        }

        private string GetAgeText(int productId)
        {
            log.LogMethodEntry(productId);
            ResetKioskTimer();

            string ageText = string.Empty;
            ProductsContainerDTO selectedProduct = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, productId);

            if (selectedProduct.AgeLowerLimit > KioskStatic.AGE_LOWER_LIMIT && selectedProduct.AgeUpperLimit < KioskStatic.AGE_UPPER_LIMIT)
            {
                ageText = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4777, selectedProduct.AgeLowerLimit, selectedProduct.AgeUpperLimit);//1 to 13 yrs
            }
            else if (selectedProduct.AgeLowerLimit > KioskStatic.AGE_LOWER_LIMIT && selectedProduct.AgeUpperLimit == KioskStatic.AGE_UPPER_LIMIT)
            {
                ageText = (selectedProduct.AgeLowerLimit == 1) ?
                    MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5174, selectedProduct.AgeLowerLimit) //Above 1 yr
                    : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4779, selectedProduct.AgeLowerLimit); //Above 2 yrs
            }
            else if (selectedProduct.AgeLowerLimit == KioskStatic.AGE_LOWER_LIMIT && selectedProduct.AgeUpperLimit < KioskStatic.AGE_UPPER_LIMIT)
            {
                ageText = (selectedProduct.AgeLowerLimit > 1) ?
                    MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4778, selectedProduct.AgeUpperLimit) //below 18 yrs
                    : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5172, selectedProduct.AgeUpperLimit); //below 1 yr
            }
            log.LogMethodExit(ageText);
            return ageText;
        }

        private bool IsAgeProfileOfProductAllowsToSellForThisCustomer(int productId, CustomerDTO customer)
        {
            log.LogMethodEntry();
            bool isAllowed = false;
            string errMsg = string.Empty;
            ProductsContainerDTO selectedProduct = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, productId);
            bool isIgnoreBirthYearSet = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR", false);

            if (selectedProduct.CustomerProfilingGroupId > -1 && isIgnoreBirthYearSet)
            {
                errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4466)
                + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5713); //Customer profiling and IGNORE_CUSTOMER_BIRTH_YEAR are both set
                throw new ValidationException(errMsg);
            }
            else if (selectedProduct.CustomerProfilingGroupId == -1 || isIgnoreBirthYearSet)
            {
                isAllowed = true;
            }
            else if ((customer.DateOfBirth == null || customer.DateOfBirth == DateTime.MinValue) && selectedProduct.CustomerProfilingGroupId > -1)
            {
                //5531 - Cannot proceed without customer date of birth information. Please update it via Register menu option on the home screen or contact staff
                errMsg += MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5531);
                throw new ValidationException(errMsg);
            }
            else if (selectedProduct.CustomerProfilingGroupId > -1 && (customer.DateOfBirth != null))
            {
                decimal age = KioskHelper.GetAge(customer.DateOfBirth);
                if (age > -1)
                {
                    age = Math.Round(age, 1);
                    if (age >= Math.Round(selectedProduct.AgeLowerLimit, 1) && age <= Math.Round(selectedProduct.AgeUpperLimit, 1))
                    {
                        isAllowed = true;
                    }
                    else
                    {
                        string ageText = GetAgeText(productIdBeingMapped);
                        //5530 - This item is available only for customers aged &1
                        errMsg += MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5530, ageText);
                        throw new ValidationException(errMsg);
                    }
                }
            }
            log.LogMethodExit(isAllowed);
            return isAllowed;
        }
    }
}
