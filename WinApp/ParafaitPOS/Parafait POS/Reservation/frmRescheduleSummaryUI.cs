/* Description  - Reschedule Reservation Summary form
*
**************
** Version Log
 **************
 * Version     Date          Modified By         Remarks
*********************************************************************************************
*2.80.0        05-Jun-2020   Guru S A            Created for reservation enhancements 
*********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmRescheduleSummaryUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ReservationBL reservationBL;
        private ExecutionContext executionContext = null;
        private Utilities utilities;
        private List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> validationList = null;
        private List<TransactionReservationScheduleDTO> tempTransactionReservationScheduleDTOList = null;
        private const int panelBtnElementHeight = 35;
        private const int panelCbxElementHeight = 30;
        private const int panelCbxElementWidth = 27;
        private Font panelElementFont = new Font("Tohama", 12F, FontStyle.Regular, GraphicsUnit.Point);//, ((byte)(0)));
        private Color panelElementBackColor = SystemColors.Control;
        private Transaction.TransactionLine bookingProductTrxLine = null;
        private Transaction.TransactionLine bookingProductScheduleTrxLine = null;
        private const string COLLAPSEPANEL = "C";
        private const string EXPANDPANEL = "E";
        private int errorLineCounter = 0;
        private DateTime oldScheduleFromDateTime;
        private DateTime oldScheduleToDateTime;
        private DateTime newScheduleFromDateTime;
        private DateTime newScheduleToDateTime;
        //private const int NOISSUE = 0;
        //private const int OVERRIDABLEISSUE = 0;
        //private const int NEEDTOCANCELISSUE = 0;
        private Products bookingProductBL = null;
        internal class TrxLineDataForValidation
        {
            int bookingProductId = -1;//Booking product
            int comboProductId = -1; //Package/Additional package lines
            int trxLineId = -1; //Schedule lines
            bool? additionalProduct = null;
            //int issueCode = 0; //0 - no issue, 1 - overrideable issue, 2-need to cancel
            bool optToCancel = false;
            bool optToOverride = false;
            public int BookingProductId { get { return bookingProductId; } set { bookingProductId = value; } }
            public int ComboProductId { get { return comboProductId; } set { comboProductId = value; } }
            public int TrxLineId { get { return trxLineId; } set { trxLineId = value; } }
            //public int IssueCode { get { return issueCode; } set { issueCode = value; } }
            public bool? AdditionalProduct { get { return additionalProduct; } set { additionalProduct = value; } }
            public bool OptToCancel { get { return optToCancel; } set { optToCancel = value; } }
            public bool OptToOverride { get { return optToOverride; } set { optToOverride = value; } }
        }

        private List<TrxLineDataForValidation> trxLineDataForValidationList = new List<TrxLineDataForValidation>();

        public frmRescheduleSummaryUI(Utilities utilities, ExecutionContext executionContext, ReservationBL reservationBL, List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> validationList)
        {
            log.LogMethodEntry(executionContext, reservationBL, validationList);
            POSUtils.SetLastActivityDateTime();
            this.utilities = utilities;
            this.executionContext = executionContext;
            this.reservationBL = reservationBL;
            this.validationList = validationList;
            this.tempTransactionReservationScheduleDTOList = new List<TransactionReservationScheduleDTO>();
            Transaction.TransactionLine bookingScheduleTrxLine = this.reservationBL.BookingTransaction.GetBookingProductParentScheduleLine();
            this.tempTransactionReservationScheduleDTOList.Add(bookingScheduleTrxLine.TransactionReservationScheduleDTOList.Find(trs => trs.Cancelled == false && trs.TrxId == -1));
            panelElementFont = new Font(utilities.ParafaitEnv.DEFAULT_GRID_FONT, 15F, FontStyle.Regular);//, GraphicsUnit.Point, ((byte)(0)));
            //utilities.ParafaitEnv.DEFAULT_GRID_FONT, 15, FontStyle.Regular
            InitializeComponent();
            this.utilities.setLanguage();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void frmRescheduleSummaryUI_Load(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                LoadHeaderSection();
                SetBookingProductDataForValidation();
                bookingProductBL = reservationBL.GetBookingProduct();
                LoadPackageProductPanel();
                LoadAdditionalProductPanel();
                LoaddAddtionalTimeSlotPanel();
                SetPanelSizeAndSetExpandCollapseBtnDisplay();
                if (this.btnExpandCollapse1.Visible)
                {
                    CollapseExpandPanels(this.btnExpandCollapse1);
                }
                this.utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void LoadHeaderSection()
        {
            log.LogMethodEntry();
            if (reservationBL != null && reservationBL.GetReservationDTO != null && reservationBL.BookingTransaction != null)
            {
                lblBookingProductName.Text = reservationBL.GetReservationDTO.BookingProductName;
                GetBookingProductAndScheduleLines();
                if (bookingProductScheduleTrxLine != null && bookingProductScheduleTrxLine.TransactionReservationScheduleDTOList != null)
                {
                    TransactionReservationScheduleDTO oldTRSDTO = bookingProductScheduleTrxLine.TransactionReservationScheduleDTOList.Find(trs => trs.Cancelled == false && trs.TrxId > -1);
                    oldScheduleFromDateTime = oldTRSDTO.ScheduleFromDate;
                    oldScheduleToDateTime = oldTRSDTO.ScheduleToDate;
                    lblOldScheduleValue.Text = oldTRSDTO.ScheduleFromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)
                                        + " " + MessageContainerList.GetMessage(executionContext, "to")
                                        + " " + oldTRSDTO.ScheduleToDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    lblGuestQtyValue.Text = oldTRSDTO.GuestQuantity.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                    lblOldFacilityMapValue.Text = oldTRSDTO.FacilityMapName;
                }
                else
                {
                    lblOldScheduleValue.Text = lblGuestQtyValue.Text = lblOldFacilityMapValue.Text = string.Empty;
                }

                if (bookingProductScheduleTrxLine != null && bookingProductScheduleTrxLine.TransactionReservationScheduleDTOList != null)
                {
                    TransactionReservationScheduleDTO rescheduledTRSDTO = bookingProductScheduleTrxLine.TransactionReservationScheduleDTOList.Find(trs => trs.Cancelled == false && trs.TrxId == -1);
                    newScheduleFromDateTime = rescheduledTRSDTO.ScheduleFromDate;
                    newScheduleToDateTime = rescheduledTRSDTO.ScheduleToDate;
                    lblNewScheduleValue.Text = rescheduledTRSDTO.ScheduleFromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)
                                        + " " + MessageContainerList.GetMessage(executionContext, "to")
                                        + " " + rescheduledTRSDTO.ScheduleToDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);

                    lblNewFacilityMapValue.Text = rescheduledTRSDTO.FacilityMapName;
                }
                else
                {
                    lblNewScheduleValue.Text = lblNewScheduleValue.Text = lblNewFacilityMapValue.Text = string.Empty;
                }

            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void GetBookingProductAndScheduleLines()
        {
            log.LogMethodEntry();
            bookingProductTrxLine = this.reservationBL.GetBookingProductTransactionLine();
            bookingProductScheduleTrxLine = null;
            List<Transaction.TransactionLine> transactionScheduleLines = this.reservationBL.GetScheduleTransactionLines();
            if (transactionScheduleLines != null && transactionScheduleLines.Any())
            {
                bookingProductScheduleTrxLine = transactionScheduleLines.Find(tl => tl.LineValid && tl.CancelledLine == false && tl == bookingProductTrxLine.ParentLine);
            }
            log.LogMethodExit();
        }

        private void LoadPackageProductPanel()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                errorLineCounter = 0;
                if (reservationBL.ReservationTransactionIsNotNull())
                {
                    if (bookingProductBL != null)
                    {
                        List<ComboProductDTO> packageProductDTOList = bookingProductBL.GetComboPackageProductSetup(false);
                        List<Transaction.TransactionLine> packageProductTrxLines = reservationBL.GetPurchasedPackageProducts();
                        if (packageProductDTOList != null && packageProductDTOList.Any())
                        {
                            int rowIndex = 0;
                            for (int i = 0; i < packageProductDTOList.Count; i++)
                            {
                                List<Transaction.TransactionLine> productTrxLines = null;
                                if (packageProductTrxLines != null && packageProductTrxLines.Any())
                                {
                                    List<Transaction.TransactionLine> parentProductTrxLines = packageProductTrxLines.Where(tl => tl.LineValid == true
                                                                                                                                  && tl.ComboproductId == packageProductDTOList[i].ComboProductId
                                                                                                                                  && tl.ProductID == packageProductDTOList[i].ChildProductId
                                                                                                                                  ).ToList();
                                    if (parentProductTrxLines != null && parentProductTrxLines.Any())
                                    {

                                        productTrxLines = packageProductTrxLines.Where(tl => tl.LineValid == true
                                                                                           && ((tl.ComboproductId == packageProductDTOList[i].ComboProductId
                                                                                                 && tl.ProductID == packageProductDTOList[i].ChildProductId) ||
                                                                                                  parentProductTrxLines.Exists(tlParent => tlParent == tl.ParentLine) == true)
                                                                                            ).ToList();
                                        Panel pnlPackageProduct = CreateProductPanel(packageProductDTOList[i], productTrxLines, rowIndex, false);
                                        rowIndex++;
                                        this.pnlPackageProduct.SuspendLayout();
                                        this.pnlPackageProduct.Controls.Add(pnlPackageProduct);
                                        this.pnlPackageProduct.ResumeLayout(true);
                                    }
                                }
                            }
                        }
                    }
                }
                SetPackageProdErrorInfo(errorLineCounter);
                errorLineCounter = 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void SetPackageProdErrorInfo(int errLineCount)
        {
            log.LogMethodEntry(errLineCount);
            if (errLineCount > 0)
            {
                this.lblErrorInfo1.Show();
                this.pbxErrorInfo1.Show();
                this.lblErrorInfo1.Text = " (" + (errLineCount == 0 ? "0" : errLineCount.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)) + ") ";
            }
            else
            {
                this.lblErrorInfo1.Text = "0";
                this.lblErrorInfo1.Hide();
                this.pbxErrorInfo1.Hide();
            }
            log.LogMethodExit();
        }

        private void LoadAdditionalProductPanel()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                errorLineCounter = 0;
                if (reservationBL.ReservationTransactionIsNotNull())
                {
                    // int guestQty = reservationBL.GetGuestQuantity();
                    if (bookingProductBL != null)
                    {
                        List<ComboProductDTO> additionalProductDTOList = bookingProductBL.GetComboAdditionalProductSetup(false);
                        //List<ComboProductDTO> additionalProductDTOList = bookingProduct.GetComboAdditionalProductSetup(false);
                        List<Transaction.TransactionLine> additionalProductTrxLines = reservationBL.GetPurchasedAdditionalProducts();
                        if (additionalProductDTOList != null && additionalProductDTOList.Count > 0)
                        {
                            // ClearPnlAdditionalProducts();
                            //this.usrCtrlAdditionProductDetails1.Visible = false;
                            // int locationX = this.usrCtrlAdditionProductDetails1.Location.X;
                            //int locationY = this.usrCtrlAdditionProductDetails1.Location.Y;
                            //int counter = 0;
                            int rowIndex = 0;
                            for (int i = 0; i < additionalProductDTOList.Count; i++)
                            {
                                List<Transaction.TransactionLine> productTrxLines = null;
                                if (additionalProductTrxLines != null && additionalProductTrxLines.Any())
                                {
                                    List<Transaction.TransactionLine> parentProductTrxLines = additionalProductTrxLines.Where(tl => tl.LineValid == true
                                                                                                                                  && tl.ComboproductId == additionalProductDTOList[i].ComboProductId
                                                                                                                                  && tl.ProductID == additionalProductDTOList[i].ChildProductId
                                                                                                                                  ).ToList();
                                    if (parentProductTrxLines != null && parentProductTrxLines.Any())
                                    {
                                        productTrxLines = additionalProductTrxLines.Where(tl => tl.LineValid == true
                                                                                           && ((tl.ComboproductId == additionalProductDTOList[i].ComboProductId
                                                                                                 && tl.ProductID == additionalProductDTOList[i].ChildProductId) ||
                                                                                                  parentProductTrxLines.Exists(tlParent => tlParent == tl.ParentLine) == true)
                                                                                            ).ToList();
                                        Panel pnlAdditionalProduct = CreateProductPanel(additionalProductDTOList[i], productTrxLines, rowIndex, true);
                                        rowIndex++;
                                        this.pnlAdditionalPackageProduct.Controls.Add(pnlAdditionalProduct);
                                    }
                                }
                                //usrCtrlProductDetails usrCtrlProductDetailsEntry = new usrCtrlProductDetails(additionalProductDTOList[i], productTrxLines, 0, reservationBL.GetReservationDTO.Status);
                                //usrCtrlProductDetailsEntry.CancelProductLine += new usrCtrlProductDetails.CancelProductLineDelegate(CancelProductLine);
                                //usrCtrlProductDetailsEntry.RefreshTransactionLines += new usrCtrlProductDetails.RefreshTransactionLinesDelegate(RefreshProductTransactionLines);
                                //usrCtrlProductDetailsEntry.AddProductToTransaction += new usrCtrlProductDetails.AddProductToTransactionDelegate(AddProductToBookingTransaction);
                                //usrCtrlProductDetailsEntry.GetTrxProfile += new usrCtrlProductDetails.GetTrxProfileDelegate(GetTrxProfile);
                                //usrCtrlProductDetailsEntry.SetDiscounts += new usrCtrlProductDetails.SetDiscountsDelegate(SetDiscounts);
                                //usrCtrlProductDetailsEntry.GetTrxLineIndex += new usrCtrlProductDetails.GetTrxLineIndexDelegate(GetTrxLineIndex);
                                //usrCtrlProductDetailsEntry.EditModifiers += new usrCtrlProductDetails.EditModifiersDelegate(EditModifiers);
                                //usrCtrlProductDetailsEntry.RescheduleAttraction += new usrCtrlProductDetails.RescheduleAttractionDelegate(RescheduleAttraction);
                                //usrCtrlProductDetailsEntry.RescheduleAttractionGroup += new usrCtrlProductDetails.RescheduleAttractionGroupDelegate(RescheduleAttractionGroup);

                                //usrCtrlProductDetailsEntry.Name = "usrCtrlAdditionProductDetails0" + counter.ToString();
                                //pnlAdditionalProducts.Controls.Add(usrCtrlProductDetailsEntry);
                                //usrCtrlProductDetailsEntry.Location = new Point(locationX, locationY);
                                //usrCtrlProductDetailsEntry.Width = 785;
                                //locationY = locationY + 44;

                            }
                        }
                        //else
                        //{
                        //    ClearPnlAdditionalProducts();
                        //    this.usrCtrlAdditionProductDetails1.Enabled = false;
                        // }
                    }
                }
                SetAdditionalProdErroInfo(errorLineCounter);
                errorLineCounter = 0;
                //UpdateUserControlUIElements();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void SetAdditionalProdErroInfo(int errLineCount)
        {
            log.LogMethodEntry(errLineCount);
            if (errLineCount > 0)
            {
                this.lblErrorInfo2.Show();
                this.pbxErrorInfo2.Show();
                this.lblErrorInfo2.Text = " (" + (errLineCount == 0 ? "0" : errLineCount.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)) + ") ";
            }
            else
            {
                this.lblErrorInfo2.Text = "0";
                this.lblErrorInfo2.Hide();
                this.pbxErrorInfo2.Hide();
            }
            log.LogMethodExit();
        }

        private void SetBookingProductDataForValidation()
        {
            log.LogMethodEntry();
            KeyValuePair<Transaction.TransactionLine, List<ValidationError>> bookingProductLineValidation = validationList.Find(kPair => kPair.Key == bookingProductTrxLine);
            if (bookingProductLineValidation.Value != null && bookingProductLineValidation.Value.Any())
            {
                TrxLineDataForValidation bookingProductEntry = new TrxLineDataForValidation();
                bookingProductEntry.BookingProductId = bookingProductTrxLine.ProductID;
                this.lblBookingProductName.ForeColor = Color.Red;
                trxLineDataForValidationList.Add(bookingProductEntry);
            }
            log.LogMethodExit();
        }

        private void SetProductsDataForValidation(int comboProductId, bool additionalProduct)//, int issueCodeValue)
        {
            log.LogMethodEntry(comboProductId, additionalProduct);//, issueCodeValue);
            TrxLineDataForValidation pkgAdditionalProductEntry = new TrxLineDataForValidation();
            pkgAdditionalProductEntry.ComboProductId = comboProductId;
            pkgAdditionalProductEntry.AdditionalProduct = additionalProduct;
            //pkgAdditionalProductEntry.IssueCode = issueCodeValue;
            trxLineDataForValidationList.Add(pkgAdditionalProductEntry);
            log.LogMethodExit();
        }


        private void SetAdditionalReservationSlotDataForValidation(int lineIndex)
        {
            log.LogMethodEntry(lineIndex);
            TrxLineDataForValidation pkgAdditionalProductEntry = new TrxLineDataForValidation();
            pkgAdditionalProductEntry.TrxLineId = (this.reservationBL.BookingTransaction.TrxLines[lineIndex].DBLineId == 0 ? lineIndex + 1 : this.reservationBL.BookingTransaction.TrxLines[lineIndex].DBLineId);
            //pkgAdditionalProductEntry.IssueCode = NEEDTOCANCELISSUE;
            trxLineDataForValidationList.Add(pkgAdditionalProductEntry);
            log.LogMethodExit();
        }

        private Panel CreateProductPanel(ComboProductDTO comboProductDTO, List<Transaction.TransactionLine> productTrxLines, int rowIndex, bool AdditionalProduct)
        {
            log.LogMethodEntry(comboProductDTO, productTrxLines, rowIndex, AdditionalProduct);
            Color panelBackgroundColor = GetRowColor(rowIndex);
            Color panelElementBorderColor = Color.LightSteelBlue;
            Panel productPanel = new Panel()
            {
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            bool cancelCheckBoxIsEnabled = false;
            bool overrideCheckBoxIsEnabled = false;
            productPanel.SuspendLayout();
            Transaction.TransactionLine mainProductLine = productTrxLines.Find(tl => tl.LineValid && tl.ComboproductId == comboProductDTO.ComboProductId);

            Button btnProductName = new Button()
            {
                Name = "btnProductName",
                Text = mainProductLine.ProductName,
                Size = new Size(341, panelBtnElementHeight),
                FlatStyle = FlatStyle.Flat,
                AutoEllipsis = true,
                BackColor = panelBackgroundColor,
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false,
                Font = panelElementFont,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            btnProductName.FlatAppearance.MouseDownBackColor = panelBackgroundColor;
            btnProductName.FlatAppearance.MouseOverBackColor = panelBackgroundColor;
            btnProductName.FlatAppearance.BorderColor = panelElementBorderColor;
            List<Transaction.TransactionLine> mainProductLineList = productTrxLines.Where(tl => tl.LineValid && tl.ComboproductId == comboProductDTO.ComboProductId).ToList();
            Button btnQty = new Button()
            {
                Name = "btnQty",
                Text = mainProductLineList.Count.ToString(utilities.ParafaitEnv.NUMBER_FORMAT),
                Size = new Size(81, panelBtnElementHeight),
                FlatStyle = FlatStyle.Flat,
                BackColor = panelBackgroundColor,
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false,
                Font = panelElementFont,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            btnQty.FlatAppearance.MouseDownBackColor = panelBackgroundColor;
            btnQty.FlatAppearance.MouseOverBackColor = panelBackgroundColor;
            btnQty.FlatAppearance.BorderColor = panelElementBorderColor;
            List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> validationLineList = GetTrxLineValidationDetails(productTrxLines);
            string statusMessage = BuildStatusMessage(validationLineList);
            Button btnScheduleStatus = new Button()
            {
                Name = "btnScheduleStatus",
                Text = statusMessage,
                Size = new Size(310, panelBtnElementHeight),
                FlatStyle = FlatStyle.Flat,
                AutoEllipsis = true,
                // BackColor = panelElementBackColor,
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false,
                Font = panelElementFont,
                Tag = statusMessage,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            btnScheduleStatus.FlatAppearance.MouseDownBackColor = panelBackgroundColor;
            btnScheduleStatus.FlatAppearance.MouseOverBackColor = panelBackgroundColor;
            btnScheduleStatus.FlatAppearance.BorderColor = panelElementBorderColor;
            btnScheduleStatus.MouseEnter += new EventHandler(btnScheduleStatus_MouseEnter);
            btnScheduleStatus.MouseLeave += new EventHandler(btnScheduleStatus_MouseLeave);
            btnScheduleStatus.Click += new EventHandler(btnScheduleStatus_Click);

            cancelCheckBoxIsEnabled = EnableCancelCheckBox(validationLineList);
            CustomCheckBox cbxCancelProduct = new CustomCheckBox()
            {
                Name = "cbxCancelProduct",
                Enabled = cancelCheckBoxIsEnabled,
                Appearance = Appearance.Button,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                CheckAlign = System.Drawing.ContentAlignment.MiddleRight,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                ImageAlign = System.Drawing.ContentAlignment.MiddleRight,
                ImageIndex = 1,
                Size = new System.Drawing.Size(panelCbxElementWidth, panelCbxElementHeight),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage,
                UseVisualStyleBackColor = true,
                Tag = comboProductDTO.ComboProductId,
                Margin = new Padding(0),
                Padding = new Padding(0),
                Location = new Point(24, 1)
            };
            //cbxCancelled.FlatAppearance.BorderSize = 0;
            cbxCancelProduct.FlatAppearance.BorderColor = panelElementBorderColor;
            cbxCancelProduct.FlatAppearance.MouseDownBackColor = panelBackgroundColor;
            cbxCancelProduct.FlatAppearance.MouseOverBackColor = panelBackgroundColor;
            cbxCancelProduct.CheckedChanged += new EventHandler(ToggleProdCancelOption);

            overrideCheckBoxIsEnabled = EnableOverRideCheckBox(validationLineList);
            CustomCheckBox cbxOverRide = new CustomCheckBox()
            {
                Name = "cbxOverRide",
                Enabled = overrideCheckBoxIsEnabled,
                Appearance = Appearance.Button,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                CheckAlign = System.Drawing.ContentAlignment.MiddleRight,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                ImageAlign = System.Drawing.ContentAlignment.MiddleRight,
                ImageIndex = 1,
                Size = new System.Drawing.Size(panelCbxElementWidth, panelCbxElementHeight),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage,
                UseVisualStyleBackColor = true,
                Tag = comboProductDTO.ComboProductId,
                Margin = new Padding(0),
                Padding = new Padding(0),
                Location = new Point(24, 1)
            };
            //cbxOverRide.FlatAppearance.BorderSize = 0;
            cbxOverRide.FlatAppearance.BorderColor = panelElementBorderColor;
            cbxOverRide.FlatAppearance.MouseDownBackColor = panelBackgroundColor;
            cbxOverRide.FlatAppearance.MouseOverBackColor = panelBackgroundColor;
            cbxOverRide.CheckedChanged += new EventHandler(ToggleProdOverrideOption);

            Panel pnlCancelCbXBkground = new Panel()
            {
                BorderStyle = BorderStyle.None,
                BackColor = panelElementBorderColor,
                Size = new Size(76, panelBtnElementHeight),
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            Panel pnlCancelCbXBkgroundChild = new Panel()
            {
                BorderStyle = BorderStyle.None,
                BackColor = panelBackgroundColor,
                Size = new Size(74, panelBtnElementHeight - 2),
                Margin = new Padding(0),
                Padding = new Padding(0),
                Location = new Point(1, 1)
            };

            Panel pnlOverrideCbxBkground = new Panel()
            {
                BorderStyle = BorderStyle.None,
                BackColor = panelElementBorderColor,
                Size = new Size(76, panelBtnElementHeight),
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            Panel pnlOverrideCbxBkgroundChild = new Panel()
            {
                BorderStyle = BorderStyle.None,
                BackColor = panelBackgroundColor,
                Size = new Size(74, panelBtnElementHeight - 2),
                Margin = new Padding(0),
                Padding = new Padding(0),
                Location = new Point(1, 1)
            };

            pnlCancelCbXBkground.Controls.Add(pnlCancelCbXBkgroundChild);
            pnlCancelCbXBkgroundChild.Controls.Add(cbxCancelProduct);
            pnlOverrideCbxBkground.Controls.Add(pnlOverrideCbxBkgroundChild);
            pnlOverrideCbxBkgroundChild.Controls.Add(cbxOverRide);
            productPanel.Controls.Add(btnProductName);
            productPanel.Controls.Add(btnQty);
            productPanel.Controls.Add(btnScheduleStatus);
            productPanel.Controls.Add(pnlCancelCbXBkground);
            productPanel.Controls.Add(pnlOverrideCbxBkground);

            //btnProductName.Location = new Point(0, 2);
            btnProductName.Location = new Point(0, 0);

            btnQty.Location = new Point(btnProductName.Location.X + btnProductName.Width - 1, 0);
            btnScheduleStatus.Location = new Point(0 + btnQty.Location.X + btnQty.Width - 1, 0);
            //pnlCancelCbXBkground.Location = new Point(23 + btnScheduleStatus.Location.X + btnScheduleStatus.Width, 0);
            //pnlOverrideCbxBkground.Location = new Point(30 + pnlCancelCbXBkground.Location.X + pnlCancelCbXBkground.Width, 0);
            pnlCancelCbXBkground.Location = new Point(0 + btnScheduleStatus.Location.X + btnScheduleStatus.Width - 1, 0);
            pnlOverrideCbxBkground.Location = new Point(0 + pnlCancelCbXBkground.Location.X + pnlCancelCbXBkground.Width - 1, 0);
            productPanel.BackColor = panelBackgroundColor;
            productPanel.Size = new Size(btnProductName.Width + btnQty.Width + btnScheduleStatus.Width + pnlCancelCbXBkground.Width + pnlOverrideCbxBkground.Width - 2, btnProductName.Height + 0);
            productPanel.ResumeLayout(true);
            if (cancelCheckBoxIsEnabled || overrideCheckBoxIsEnabled)
            {
                errorLineCounter++;
                productPanel.ForeColor = Color.Red;
                //int issueCodeValue = overrideCheckBoxIsEnabled == true ? OVERRIDABLEISSUE : NEEDTOCANCELISSUE;
                SetProductsDataForValidation(comboProductDTO.ComboProductId, AdditionalProduct);
            }
            log.LogMethodExit(productPanel);
            return productPanel;
        }

        private List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> GetTrxLineValidationDetails(List<Transaction.TransactionLine> productTrxLines)
        {
            log.LogMethodEntry();
            List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> lineValidationList = new List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>>();
            if (this.validationList != null && this.validationList.Any())
            {
                for (int i = 0; i < productTrxLines.Count; i++)
                {

                    List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> keyValuePairs = this.validationList.Where(kpair => kpair.Key == productTrxLines[i]).ToList();
                    if (keyValuePairs != null && keyValuePairs.Any())
                    {
                        lineValidationList.AddRange(keyValuePairs);
                    }
                }
            }
            log.LogMethodExit(lineValidationList);
            return lineValidationList;
        }
        private string BuildStatusMessage(List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> validationLineList)
        {
            log.LogMethodEntry();
            string msg = string.Empty;
            bool hasErrorMsg = false;
            if (validationLineList != null && validationLineList.Any())
            {
                msg = string.Empty;
                for (int i = 0; i < validationLineList.Count; i++)
                {
                    if (validationLineList[i].Value != null && validationLineList[i].Value.Any())
                    {
                        for (int j = 0; j < validationLineList[i].Value.Count; j++)
                        {
                            if (msg.Contains(validationLineList[i].Value[j].Message) == false)
                            {
                                msg = msg + validationLineList[i].Value[j].Message + Environment.NewLine;
                            }
                            hasErrorMsg = true;
                        }
                    }
                }
            }
            if (hasErrorMsg == false)
            {
                msg = MessageContainerList.GetMessage(executionContext, "Ok");
            }
            log.LogMethodExit(msg);
            return msg;
        }


        private bool EnableCancelCheckBox(List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> validationLineList)
        {
            log.LogMethodEntry();
            bool allowCancel = false;
            if (validationLineList != null && validationLineList.Any()
                && validationLineList.Exists(kpair => kpair.Value != null && kpair.Value.Any()))
            {
                allowCancel = true;
            }
            log.LogMethodExit(allowCancel);
            return allowCancel;
        }


        private bool EnableOverRideCheckBox(List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> validationLineList)
        {
            log.LogMethodEntry();
            bool allowOverride = false;
            bool allowInactiveItemsForReschedule = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_INACTIVE_ITEMS_IN_RESERVATION_RESCHEDULE", false);
            if (allowInactiveItemsForReschedule && validationLineList != null && validationLineList.Any()
                && validationLineList.Exists(kpair => kpair.Value != null && kpair.Value.Any()
                                                    && kpair.Value.Exists(errInfo => errInfo.FieldName == "Inactive" || errInfo.FieldName == "ExpiryDate" || errInfo.FieldName == "StartDate")
                                                    && kpair.Value.Exists(errInfo => errInfo.FieldName != "Inactive" && errInfo.FieldName != "ExpiryDate" && errInfo.FieldName != "StartDate") == false))
            {
                allowOverride = true;
            }
            log.LogMethodExit(allowOverride);
            return allowOverride;
        }


        private void LoaddAddtionalTimeSlotPanel()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<Transaction.TransactionLine> transactionScheduleLines = this.reservationBL.GetScheduleTransactionLines();
            errorLineCounter = 0;
            if (transactionScheduleLines != null && transactionScheduleLines.Count > 1)
            {
                int rowIndex = 0;
                if (bookingProductScheduleTrxLine == null)
                {
                    bookingProductScheduleTrxLine = transactionScheduleLines.Find(tl => tl.LineValid && tl.CancelledLine == false && tl == bookingProductTrxLine.ParentLine);
                }
                for (int i = 0; i < transactionScheduleLines.Count; i++)
                {
                    if (transactionScheduleLines[i] != bookingProductScheduleTrxLine)
                    {
                        Panel pnlAdditionalTimeSlot = CreateAdditionalTimeSlotPanel(transactionScheduleLines[i], rowIndex);
                        rowIndex++;
                        this.pnlAdditionalSlots.Controls.Add(pnlAdditionalTimeSlot);
                    }
                }
            }
            SetAdditionalReservationSlotErrorInfo(errorLineCounter);
            errorLineCounter = 0;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void SetAdditionalReservationSlotErrorInfo(int errLineCount)
        {
            log.LogMethodEntry(errLineCount);
            if (errLineCount > 0)
            {
                this.lblErrorInfo3.Show();
                this.pbxErrorInfo3.Show();
                this.lblErrorInfo3.Text = " (" + (errLineCount == 0 ? "0" : errLineCount.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)) + ") ";
            }
            else
            {
                this.lblErrorInfo3.Text = "0";
                this.lblErrorInfo3.Hide();
                this.pbxErrorInfo3.Hide();
            }
            log.LogMethodExit();
        }

        private void pnlDetailsDisplayLostFocus(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            if (pnlDetailsDisplay != null)
            {
                pnlDetailsDisplay.Visible = false;
            }
            log.LogMethodExit();
        }
        private void btnScheduleStatus_MouseEnter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Hand;
            log.LogMethodExit();
        }

        private void btnScheduleStatus_MouseLeave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnScheduleStatus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                pnlDetailsDisplay.Visible = false;
                pnlDetailsDisplayChild.AutoSize = true;
                pnlDetailsDisplayChild.SuspendLayout();
                pnlDetailsDisplay.SuspendLayout();
                Button sendObj = (Button)sender;
                this.lblDetailsDisplay.Text = string.Empty;
                if (sendObj != null && sendObj.Tag != null)
                {
                    this.lblDetailsDisplay.Text = sendObj.Tag.ToString();
                }
                pnlDetailsDisplay.Location = sendObj.PointToScreen(Point.Empty);
                //System.Windows.SystemParameters.PrimaryScreenHeight
                if (this.Height - pnlDetailsDisplay.Location.Y - 50 - pnlDetailsDisplay.Height < 20)
                {
                    pnlDetailsDisplay.Location = new Point(pnlDetailsDisplay.Location.X - 100, pnlDetailsDisplay.Location.Y - pnlDetailsDisplay.Height);
                }
                else
                {
                    pnlDetailsDisplay.Location = new Point(pnlDetailsDisplay.Location.X - 100, pnlDetailsDisplay.Location.Y - 50);
                }
                pnlDetailsDisplayChild.ResumeLayout(true);
                this.pnlDetailsDisplayChild.AutoSize = false;
                pnlDetailsDisplayChild.AutoScroll = true;
                this.pnlDetailsDisplayChild.Size = new Size(pnlDetailsDisplay.Width - 6, pnlDetailsDisplay.Height - btnCloseDisplayPanel.Height - 11);
                //lblDetailsHeader.Size = new Size(pnlDetailsDisplay.Width - 10, btnCloseDisplayPanel.Height+3);
                //this.btnCloseDisplayPanel.Location = new Point(pnlDetailsDisplay.Width - btnCloseDisplayPanel.Width - 2, 3);
                this.btnCloseDisplayPanel.BringToFront();
                pnlDetailsDisplayChild.ResumeLayout(true);
                pnlDetailsDisplay.ResumeLayout(true);
                pnlDetailsDisplay.Show();
                pnlDetailsDisplay.Focus();
                ActiveControl = this.btnCloseDisplayPanel;
                pnlDetailsDisplay.BringToFront();
            }catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();

        }


        private Panel CreateAdditionalTimeSlotPanel(Transaction.TransactionLine reservationScheduleTrxLine, int rowIndex)
        {
            log.LogMethodEntry(reservationScheduleTrxLine, rowIndex);
            int lineIndex = -1;
            Color panelElementBorderColor = Color.LightSteelBlue;
            Panel pnlAdditionalTimeSlot = new Panel()
            {
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            pnlAdditionalTimeSlot.SuspendLayout();
            Color panelBackgroundColor = GetRowColor(rowIndex);
            TransactionReservationScheduleDTO newScheduleDTO = null;
            bool ableToGetNewSlot = false;
            try
            {
                lineIndex = this.reservationBL.BookingTransaction.TrxLines.IndexOf(reservationScheduleTrxLine);
                // newScheduleDTO = this.reservationBL.BookingTransaction.GetRescheduledAdditionalTimeSlotDTO(reservationScheduleTrxLine, this.transactionReservationScheduleDTOList[0], this.bookingProductTrxLine);
                newScheduleDTO = reservationScheduleTrxLine.GetRescheduledScheduleTimeSlotDTO(executionContext, lineIndex, oldScheduleFromDateTime, oldScheduleToDateTime,
                                                                                              newScheduleFromDateTime, newScheduleToDateTime);
                ableToGetNewSlot = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                newScheduleDTO = new TransactionReservationScheduleDTO();
                TransactionReservationScheduleDTO oldTRSDTO = reservationScheduleTrxLine.TransactionReservationScheduleDTOList.Find(trs => trs.Cancelled == false && trs.TrxId > -1);
                TimeSpan fromTimeSpan = oldTRSDTO.ScheduleFromDate - oldScheduleFromDateTime;
                TimeSpan toTimeSpan = oldTRSDTO.ScheduleToDate - oldScheduleToDateTime;
                DateTime newSlotFromDateTime = newScheduleFromDateTime + fromTimeSpan;
                DateTime newSlotToDateTime = newScheduleToDateTime + toTimeSpan;
                newScheduleDTO.ScheduleFromDate = newSlotFromDateTime;
                newScheduleDTO.ScheduleToDate = newSlotToDateTime;
            }
            string additionalSlotName = newScheduleDTO.ScheduleFromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)
                                        + " " + MessageContainerList.GetMessage(executionContext, "to")
                                        + " " + newScheduleDTO.ScheduleToDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);

            Button btnAdditionalTimeSlotName = new Button()
            {
                Name = "btnAdditionalTimeSlotName",
                Text = additionalSlotName,
                Size = new Size(480, panelBtnElementHeight),
                FlatStyle = FlatStyle.Flat,
                AutoEllipsis = true,
                BackColor = panelBackgroundColor,
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false,
                Font = panelElementFont,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            btnAdditionalTimeSlotName.FlatAppearance.MouseDownBackColor = panelBackgroundColor;
            btnAdditionalTimeSlotName.FlatAppearance.MouseOverBackColor = panelBackgroundColor;
            btnAdditionalTimeSlotName.FlatAppearance.BorderColor = panelElementBorderColor;

            List<Transaction.TransactionLine> transactionLineList = new List<Transaction.TransactionLine>();
            transactionLineList.Add(reservationScheduleTrxLine);
            List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> validationLineList = GetTrxLineValidationDetails(transactionLineList);
            string statusMessage = BuildStatusMessage(validationLineList);
            Button btnScheduleStatus = new Button()
            {
                Name = "btnScheduleStatus",
                Text = statusMessage,
                Size = new Size(250, panelBtnElementHeight),
                FlatStyle = FlatStyle.Flat,
                AutoEllipsis = true,
                BackColor = panelBackgroundColor,
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false,
                Font = panelElementFont,
                Tag = statusMessage,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            btnScheduleStatus.FlatAppearance.MouseDownBackColor = panelBackgroundColor;
            btnScheduleStatus.FlatAppearance.MouseOverBackColor = panelBackgroundColor;
            btnScheduleStatus.FlatAppearance.BorderColor = panelElementBorderColor;
            btnScheduleStatus.MouseEnter += new EventHandler(btnScheduleStatus_MouseEnter);
            btnScheduleStatus.MouseLeave += new EventHandler(btnScheduleStatus_MouseLeave);
            btnScheduleStatus.Click += new EventHandler(btnScheduleStatus_Click); 
            bool cancelCheckBoxIsEnabled = false;
            cancelCheckBoxIsEnabled = EnableCancelCheckBox(validationLineList);
            CustomCheckBox cbxCancelSlot = new CustomCheckBox()
            {
                Name = "cbxCancelSlot",
                Enabled = cancelCheckBoxIsEnabled,
                Appearance = Appearance.Button,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                CheckAlign = System.Drawing.ContentAlignment.MiddleRight,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                ImageAlign = System.Drawing.ContentAlignment.MiddleRight,
                ImageIndex = 1,
                Size = new System.Drawing.Size(panelCbxElementWidth, panelCbxElementHeight),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage,
                UseVisualStyleBackColor = true,
                Tag = (reservationScheduleTrxLine.DBLineId == 0 ? lineIndex + 1 : reservationScheduleTrxLine.DBLineId),
                Margin = new Padding(0),
                Padding = new Padding(0),
                Location = new Point(24, 1)
            };
            //cbxCancelled.FlatAppearance.BorderSize = 0;
            cbxCancelSlot.FlatAppearance.BorderColor = panelElementBorderColor;
            cbxCancelSlot.FlatAppearance.MouseDownBackColor = panelBackgroundColor;
            cbxCancelSlot.FlatAppearance.MouseOverBackColor = panelBackgroundColor;
            cbxCancelSlot.CheckedChanged += new EventHandler(ToggleAdditionalSlotCancelOption);

            //Label lblEmpty = new Label()
            //{
            //    Name = "lblEmpty",
            //    Enabled = false,
            //    FlatStyle = System.Windows.Forms.FlatStyle.Flat,
            //    Size = new System.Drawing.Size(panelCbxElementWidth, panelCbxElementHeight),
            //    TextAlign = System.Drawing.ContentAlignment.MiddleRight,
            //};

            Panel pnlCancelCbXBkground = new Panel()
            {
                BorderStyle = BorderStyle.None,
                BackColor = panelElementBorderColor,
                Size = new Size(152, panelBtnElementHeight),
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            //Panel pnlOverrideCbxBkground = new Panel()
            //{
            //    BorderStyle = BorderStyle.None,
            //    BackColor = panelElementBorderColor,
            //    Size = new Size(74, panelBtnElementHeight),
            //    Margin = new Padding(0),
            //    Padding = new Padding(0)
            //};
            Panel pnlCancelCbXBkgroundChild = new Panel()
            {
                BorderStyle = BorderStyle.None,
                BackColor = panelBackgroundColor,
                Size = new Size(150, panelBtnElementHeight - 2),
                Margin = new Padding(0),
                Padding = new Padding(0),
                Location = new Point(1, 1)
            };

            //Panel pnlOverrideCbxBkgroundChild = new Panel()
            //{
            //    BorderStyle = BorderStyle.None,
            //    BackColor = panelBackgroundColor,
            //    Size = new Size(73, panelBtnElementHeight - 2),
            //    Margin = new Padding(0),
            //    Padding = new Padding(0),
            //    Location = new Point(1, 1)
            //};

            pnlCancelCbXBkground.Controls.Add(pnlCancelCbXBkgroundChild);
            pnlCancelCbXBkgroundChild.Controls.Add(cbxCancelSlot);
            //pnlOverrideCbxBkground.Controls.Add(pnlOverrideCbxBkgroundChild);
            //pnlOverrideCbxBkgroundChild.Controls.Add(cbxOverRide);

            pnlAdditionalTimeSlot.Controls.Add(btnAdditionalTimeSlotName);
            pnlAdditionalTimeSlot.Controls.Add(btnScheduleStatus);
            pnlAdditionalTimeSlot.Controls.Add(pnlCancelCbXBkground);
            //pnlAdditionalTimeSlot.Controls.Add(lblEmpty);

            btnAdditionalTimeSlotName.Location = new Point(0, 0);
            btnScheduleStatus.Location = new Point(0 + btnAdditionalTimeSlotName.Location.X + btnAdditionalTimeSlotName.Width - 1, 0);
            //cbxCancelSlot.Location = new Point(23 + btnScheduleStatus.Location.X + btnScheduleStatus.Width, 0);
            pnlCancelCbXBkground.Location = new Point(0 + btnScheduleStatus.Location.X + btnScheduleStatus.Width - 1, 0);
            //lblEmpty.Location = new Point(30 + cbxCancelSlot.Location.X + cbxCancelSlot.Width, 0);
            pnlAdditionalTimeSlot.BackColor = panelBackgroundColor;
            pnlAdditionalTimeSlot.Size = new Size(btnAdditionalTimeSlotName.Width + btnScheduleStatus.Width + pnlCancelCbXBkground.Width - 1, btnAdditionalTimeSlotName.Height + 0);
            pnlAdditionalTimeSlot.ResumeLayout(true);
            if (cancelCheckBoxIsEnabled)
            {
                errorLineCounter++;
                pnlAdditionalTimeSlot.ForeColor = Color.Red;
                SetAdditionalReservationSlotDataForValidation(lineIndex);
            }
            else
            {
                if (ableToGetNewSlot)
                {
                    try
                    {
                        newScheduleDTO.TrxId = -1; //for temp block
                        TransactionReservationScheduleBL transactionReservationScheduleBL = new TransactionReservationScheduleBL(executionContext, newScheduleDTO);
                        transactionReservationScheduleBL.Save();
                        this.tempTransactionReservationScheduleDTOList.Add(transactionReservationScheduleBL.TransactionReservationScheduleDTO);
                        this.reservationBL.BookingTransaction.TrxLines[lineIndex].SetTransactionReservationScheduleDTO(executionContext, transactionReservationScheduleBL.TransactionReservationScheduleDTO);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
                        string existingMsg = btnScheduleStatus.Tag.ToString();
                        btnScheduleStatus.Tag = existingMsg + MessageContainerList.GetMessage(executionContext, 1824, ex.Message) + Environment.NewLine;
                        errorLineCounter++;
                        SetAdditionalReservationSlotDataForValidation(lineIndex);
                        cbxCancelSlot.Enabled = true;
                        pnlAdditionalTimeSlot.ForeColor = Color.Red;
                    }
                }
            }
            log.LogMethodExit(pnlAdditionalTimeSlot);
            return pnlAdditionalTimeSlot;
        }

        private static Color GetRowColor(int rowIndex)
        {
            log.LogMethodEntry();
            Color panelBackgroundColor;
            if (rowIndex % 2 == 0)
            {
                panelBackgroundColor = Color.White;
            }
            else
            {
                panelBackgroundColor = Color.Azure;
            }
            log.LogMethodExit(panelBackgroundColor);
            return panelBackgroundColor;
        }

        private void btnExpandCollapse_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            Button senderBtn = (Button)sender;
            if (senderBtn != null)
            {
                CollapseExpandPanels(senderBtn);
            }
            log.LogMethodExit();
        }

        private void CollapseExpandPanels(Button senderBtn)
        {
            log.LogMethodEntry();
            bool isExpandPanel = false;
            if (senderBtn.Tag == null || string.IsNullOrWhiteSpace(senderBtn.Tag.ToString()) || senderBtn.Tag.ToString() == COLLAPSEPANEL)
            {
                isExpandPanel = true;
            }
            if (senderBtn.Name == "btnExpandCollapse1")
            {
                if (isExpandPanel)
                {
                    this.pnlGrpPackageProducts.Show();
                    this.pnlGrpAdditionalProducts.Hide();
                    this.pnlGrpAdditionalSlots.Hide();
                    this.btnExpandCollapse1.Tag = EXPANDPANEL;
                    this.btnExpandCollapse1.Image = Properties.Resources.CollapseArrow;
                    this.btnExpandCollapse2.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse2.Image = Properties.Resources.ExpandArrow;
                    this.btnExpandCollapse3.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse3.Image = Properties.Resources.ExpandArrow;
                }
                else
                {
                    this.pnlGrpPackageProducts.Hide();
                    this.btnExpandCollapse1.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse1.Image = Properties.Resources.ExpandArrow;
                }
            }
            else if (senderBtn.Name == "btnExpandCollapse2")
            {
                if (isExpandPanel)
                {
                    this.pnlGrpPackageProducts.Hide();
                    this.pnlGrpAdditionalProducts.Show();
                    this.pnlGrpAdditionalSlots.Hide();
                    this.btnExpandCollapse1.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse1.Image = Properties.Resources.ExpandArrow;
                    this.btnExpandCollapse2.Tag = EXPANDPANEL;
                    this.btnExpandCollapse2.Image = Properties.Resources.CollapseArrow;
                    this.btnExpandCollapse3.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse3.Image = Properties.Resources.ExpandArrow;
                }
                else
                {
                    this.pnlGrpAdditionalProducts.Hide();
                    this.btnExpandCollapse2.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse2.Image = Properties.Resources.ExpandArrow;
                }
            }
            else //btnExpandCollapse3
            {
                if (isExpandPanel)
                {
                    this.pnlGrpPackageProducts.Hide();
                    this.pnlGrpAdditionalProducts.Hide();
                    this.pnlGrpAdditionalSlots.Show();
                    this.btnExpandCollapse1.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse1.Image = Properties.Resources.ExpandArrow;
                    this.btnExpandCollapse2.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse2.Image = Properties.Resources.ExpandArrow;
                    this.btnExpandCollapse3.Tag = isExpandPanel;
                    this.btnExpandCollapse3.Image = Properties.Resources.CollapseArrow;
                }
                else
                {
                    this.pnlGrpAdditionalSlots.Hide();
                    this.btnExpandCollapse3.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse3.Image = Properties.Resources.ExpandArrow;
                }
            }
            log.LogMethodExit();
        }

        private void SetPanelSizeAndSetExpandCollapseBtnDisplay()
        {
            log.LogMethodEntry();
            List<Panel> productPanelList = pnlPackageProduct.Controls.OfType<Panel>().ToList();
            List<Panel> AdditionalProductPanelList = pnlAdditionalPackageProduct.Controls.OfType<Panel>().ToList();
            List<Panel> slotPanelList = pnlAdditionalSlots.Controls.OfType<Panel>().ToList();
            SetSize(productPanelList, pnlPackageProduct, pnlGrpPackageProducts, vScrollPackgeProducts);
            SetSize(AdditionalProductPanelList, pnlAdditionalPackageProduct, pnlGrpAdditionalProducts, vScrollAdditionalProducts);
            SetSize(slotPanelList, pnlAdditionalSlots, pnlGrpAdditionalSlots, vScrollAdditionalSlots);
            ShowHideExpandCollapseBtn(productPanelList, AdditionalProductPanelList, slotPanelList);
            log.LogMethodExit();
        }


        private void SetSize(List<Panel> panelList, Panel childPanel, Panel parentPanel, VerticalScrollBarView childPanelScrollBar)
        {
            log.LogMethodEntry();
            childPanel.SuspendLayout();
            parentPanel.SuspendLayout();
            int childRecordCount = 0;
            if (panelList != null)
            {
                childRecordCount = panelList.Count;
            }
            if (childRecordCount <= 3)
            {
                childPanel.Height = childRecordCount * 33 + 10;
                childPanelScrollBar.Height = childPanel.Height;
                parentPanel.Height = childPanel.Height + 26;
                childPanelScrollBar.Visible = false;
            }
            else if (childRecordCount < 9)
            {
                childPanel.Height = childRecordCount * 33 + 10;
                childPanelScrollBar.Height = childPanel.Height;
                parentPanel.Height = childPanel.Height + 26;
                childPanelScrollBar.Visible = true;
            }
            else
            {
                childPanel.Height = 212;
                childPanelScrollBar.Height = childPanel.Height;
                parentPanel.Height = childPanel.Height + 26;
                childPanelScrollBar.Visible = true;
            }
            childPanel.ResumeLayout(true);
            parentPanel.ResumeLayout(true);
            childPanelScrollBar.UpdateButtonStatus();
            log.LogMethodExit();
        }
        private void ShowHideExpandCollapseBtn(List<Panel> productPanelList, List<Panel> additionalProductPanelList, List<Panel> slotPanelList)
        {
            log.LogMethodEntry();
            int totalRecordCount = (productPanelList == null ? 0 : productPanelList.Count)
                                   + (additionalProductPanelList == null ? 0 : additionalProductPanelList.Count) + (slotPanelList == null ? 0 : slotPanelList.Count);
            if (totalRecordCount < 8)
            {
                this.btnExpandCollapse1.Hide();
                this.btnExpandCollapse2.Hide();
                this.btnExpandCollapse3.Hide();
            }
            else
            {
                this.btnExpandCollapse1.Show();
                this.btnExpandCollapse2.Show();
                this.btnExpandCollapse3.Show();
            }
            log.LogMethodExit();
        }


        private void btnBlack_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.pressed2;
            log.LogMethodExit();
        }

        private void btnBlack_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }


        private void btnCloseDisplayPanel_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.R_Remove_Btn_Normal;
            log.LogMethodExit();
        }

        private void btnCloseDisplayPanel_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.R_Remove_Btn_Normal;
            log.LogMethodExit();
        }

        private void ToggleProdCancelOption(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                CheckBox selectedCheckBox = (CheckBox)sender;
                if (selectedCheckBox.Tag != null)
                {
                    int comboProductId = Convert.ToInt32(selectedCheckBox.Tag);
                    int rowIndex = trxLineDataForValidationList.FindIndex(tlv => tlv.ComboProductId == comboProductId);
                    if (selectedCheckBox.Checked)
                    {
                        if (trxLineDataForValidationList[rowIndex].OptToOverride == false)
                        {
                            trxLineDataForValidationList[rowIndex].OptToCancel = true;
                            ClearErrorColor(selectedCheckBox);
                        }
                        else
                        {
                            selectedCheckBox.Checked = false;
                            SetErrorColor(selectedCheckBox);
                        }
                    }
                    else
                    {
                        trxLineDataForValidationList[rowIndex].OptToCancel = false;
                        SetErrorColor(selectedCheckBox);
                    }
                    bool? additionalProduct = trxLineDataForValidationList[rowIndex].AdditionalProduct;
                    int errLineCount = trxLineDataForValidationList.Count(tlD => tlD.AdditionalProduct == additionalProduct && tlD.OptToCancel == false && tlD.OptToOverride == false);
                    SetErrorInfo(errLineCount, additionalProduct);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void ToggleProdOverrideOption(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                CheckBox selectedCheckBox = (CheckBox)sender;
                if (selectedCheckBox.Tag != null)
                {
                    int comboProductId = Convert.ToInt32(selectedCheckBox.Tag);
                    int rowIndex = trxLineDataForValidationList.FindIndex(tlv => tlv.ComboProductId == comboProductId);
                    if (selectedCheckBox.Checked)
                    {
                        if (trxLineDataForValidationList[rowIndex].OptToCancel == false)
                        {
                            trxLineDataForValidationList[rowIndex].OptToOverride = true;
                            ClearErrorColor(selectedCheckBox);
                        }
                        else
                        {
                            selectedCheckBox.Checked = false;
                            SetErrorColor(selectedCheckBox);
                        }
                    }
                    else
                    {
                        trxLineDataForValidationList[rowIndex].OptToOverride = false;
                        SetErrorColor(selectedCheckBox);
                    }
                    bool? additionalProduct = trxLineDataForValidationList[rowIndex].AdditionalProduct;
                    int errLineCount = trxLineDataForValidationList.Count(tlD => tlD.AdditionalProduct == additionalProduct && tlD.OptToCancel == false && tlD.OptToOverride == false);
                    SetErrorInfo(errLineCount, additionalProduct);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void ToggleAdditionalSlotCancelOption(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                CheckBox selectedCheckBox = (CheckBox)sender;
                if (selectedCheckBox.Tag != null)
                {
                    int lineId = Convert.ToInt32(selectedCheckBox.Tag);
                    int rowIndex = trxLineDataForValidationList.FindIndex(tlv => tlv.TrxLineId == lineId);
                    if (selectedCheckBox.Checked)
                    {
                        trxLineDataForValidationList[rowIndex].OptToCancel = true;
                        ClearErrorColor(selectedCheckBox);
                    }
                    else
                    {
                        trxLineDataForValidationList[rowIndex].OptToCancel = false;
                        SetErrorColor(selectedCheckBox);
                    }
                    int errLineCount = trxLineDataForValidationList.Count(tlD => tlD.AdditionalProduct == (bool?)null && tlD.OptToCancel == false && tlD.OptToOverride == false);
                    SetErrorInfo(errLineCount, null);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }


        private static void SetErrorColor(CheckBox selectedCheckBox)
        {
            log.LogMethodEntry();
            Panel productPanel = (Panel)selectedCheckBox.Parent;
            productPanel.ForeColor = Color.Red;
            log.LogMethodExit();
        }

        private static void ClearErrorColor(CheckBox selectedCheckBox)
        {
            log.LogMethodEntry();
            Panel productPanel = (Panel)selectedCheckBox.Parent;
            productPanel.ForeColor = Color.Black;
            log.LogMethodExit();
        }
        private void SetErrorInfo(int errLineCount, bool? additionalProduct)
        {
            log.LogMethodEntry(errLineCount, additionalProduct);
            if (additionalProduct == null)
            {
                SetAdditionalReservationSlotErrorInfo(errLineCount);
            }
            else if ((bool)additionalProduct)
            {
                SetAdditionalProdErroInfo(errLineCount);
            }
            else
            {
                SetPackageProdErrorInfo(errLineCount);
            }
            log.LogMethodExit();
        }


        private void btnReschedule_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                CanProceedWithReschedule();
                this.Cursor = Cursors.WaitCursor;
                bool allowOverride = OverrideLines();
                this.Cursor = Cursors.WaitCursor;
                dBTransaction = new ParafaitDBTransaction();
                dBTransaction.BeginTransaction();
                if (CancelReservationProdcutLines(dBTransaction.SQLTrx))
                {
                    POSUtils.SetLastActivityDateTime();
                    this.Cursor = Cursors.WaitCursor;
                    StillHasPackageProducts();
                    this.Cursor = Cursors.WaitCursor;
                    this.reservationBL.RescheduleReservation(allowOverride, dBTransaction.SQLTrx);
                    this.Cursor = Cursors.WaitCursor;
                    //'Booking reschedule is saved successfully. Do you want to confirm the booking?'
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2713),
                                                    MessageContainerList.GetMessage(executionContext, "Save"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        ConfirmBooking(dBTransaction.SQLTrx);
                    }
                    dBTransaction.EndTransaction();
                    dBTransaction.Dispose();
                    this.Cursor = Cursors.WaitCursor;
                    string validationMsg = this.reservationBL.VerifyDiscountChangesOnReservation();
                    if (string.IsNullOrWhiteSpace(validationMsg) == false && validationMsg.Length > 0)
                    {
                        POSUtils.ParafaitMessageBox(validationMsg, MessageContainerList.GetMessage(executionContext, "Validation"));
                    }
                    this.Cursor = Cursors.WaitCursor;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    POSUtils.SetLastActivityDateTime();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.SetLastActivityDateTime();
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                    this.reservationBL = new ReservationBL(executionContext, utilities, reservationBL.GetReservationDTO.BookingId);
                    GetBookingProductAndScheduleLines();
                    SetTempReservationScheduleDTO();
                }
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
            }

            log.LogMethodExit();
        }

        private void SetTempReservationScheduleDTO()
        {
            log.LogMethodEntry();
            if (this.tempTransactionReservationScheduleDTOList != null && this.tempTransactionReservationScheduleDTOList.Any())
            {
                for (int i = 0; i < this.tempTransactionReservationScheduleDTOList.Count; i++)
                {
                    int lineId = this.tempTransactionReservationScheduleDTOList[i].LineId;
                    Transaction.TransactionLine transactionLine = this.reservationBL.BookingTransaction.TrxLines.Find(tl => tl.DBLineId == lineId 
                                                                                                                     && tl.TransactionReservationScheduleDTOList != null
                                                                                                                     && tl.TransactionReservationScheduleDTOList.Any());
                    if (transactionLine != null)
                    {
                        int index = this.reservationBL.BookingTransaction.TrxLines.IndexOf(transactionLine);
                        this.reservationBL.BookingTransaction.TrxLines[index].SetTransactionReservationScheduleDTO(executionContext, tempTransactionReservationScheduleDTOList[i]);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void StillHasPackageProducts()
        {
            log.LogMethodEntry();
            List<ComboProductDTO> packageProductDTOList = bookingProductBL.GetComboPackageProductSetup(false);
            List<Transaction.TransactionLine> packageProductTrxLines = new List<Transaction.TransactionLine>();
            if (packageProductDTOList != null)
            {
                if (reservationBL.BookingTransaction.TrxLines.Exists(tl => tl.LineValid
                                                                      && tl.CancelledLine == false
                                                                      && tl.ComboproductId > -1
                                                                      && packageProductDTOList.Exists(pDTO => pDTO.ComboProductId == tl.ComboproductId)) == false)
                {
                    string errMsg = MessageContainerList.GetMessage(executionContext, 2659) + ". " + MessageContainerList.GetMessage(executionContext, 2158);
                    //Sorry unable to proceed. At least one package product is mandatory for the booking
                    throw new ValidationException(errMsg);
                }
            }
            log.LogMethodExit();
        }



        private void CanProceedWithReschedule()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            CheckBookingProductStatus();
            if (trxLineDataForValidationList != null && trxLineDataForValidationList.Any()
                && trxLineDataForValidationList.Exists(tlv => tlv.OptToCancel == false && tlv.OptToOverride == false))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2709));
                //'Please take action (Cancel/Override) on impacted entries to proceed'
            }
            List<Transaction.TransactionLine> transactionLineList = reservationBL.GetPurchasedPackageProducts();
            if (transactionLineList != null)
            {
                if (transactionLineList.Where(tl => tl.LineValid && tl.ComboproductId > -1).Select(tl => tl.ComboproductId).Distinct().Count()
                      == trxLineDataForValidationList.Count(tlv => tlv.AdditionalProduct == false && tlv.OptToCancel == true))
                {
                    string errMsg = MessageContainerList.GetMessage(executionContext, 2659) + ". " + MessageContainerList.GetMessage(executionContext, 2158);
                    //Sorry unable to proceed. At least one package product is mandatory for the booking
                    throw new ValidationException(errMsg);
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void CheckBookingProductStatus()
        {
            log.LogMethodEntry();
            bool bookingProductIsNotActive = false;
            bookingProductIsNotActive = (trxLineDataForValidationList != null && trxLineDataForValidationList.Any()
                && trxLineDataForValidationList.Exists(tlv => tlv.BookingProductId > -1 && tlv.OptToCancel == false && tlv.OptToOverride == false));

            if (bookingProductIsNotActive)
            {
                bool allowInactiveProducts = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_INACTIVE_ITEMS_IN_RESERVATION_RESCHEDULE");
                if (allowInactiveProducts)
                {
                    //Booking product &1 is inactive. Do you want to override and proceed with reschedule?
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2720, lblBookingProductName.Text), MessageContainerList.GetMessage(executionContext, "Override"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        TrxLineDataForValidation trxLineDataForValidation = trxLineDataForValidationList.Find(tlv => tlv.BookingProductId > -1 && tlv.OptToCancel == false && tlv.OptToOverride == false);
                        if (trxLineDataForValidation != null)
                        {
                            int index = trxLineDataForValidationList.IndexOf(trxLineDataForValidation);
                            if (index > -1)
                            {
                                trxLineDataForValidationList[index].OptToOverride = true;
                                lblBookingProductName.ForeColor = lblGuestQtyValue.ForeColor;
                            }
                        }
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2721));
                    //Sorry, cannot proceed. Inactive/unavailable products are not allowed in reservation reschedule
                }
            }
            log.LogMethodExit();
        }

        private bool CancelReservationProdcutLines(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            bool cancellationSuccessful = false;
            if (trxLineDataForValidationList != null && trxLineDataForValidationList.Any()
                && trxLineDataForValidationList.Exists(tlv => tlv.OptToCancel == true))
            {
                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2710), MessageContainerList.GetMessage(executionContext, "Cancel"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < trxLineDataForValidationList.Count; i++)
                    {
                        if (trxLineDataForValidationList[i].OptToCancel)
                        {
                            List<Transaction.TransactionLine> linesForCancellation = new List<Transaction.TransactionLine>();
                            if (trxLineDataForValidationList[i].ComboProductId > -1)
                            {
                                List<Transaction.TransactionLine> cancelLinesSetOne = reservationBL.BookingTransaction.TrxLines.Where(tl => tl.LineValid && tl.ComboproductId == trxLineDataForValidationList[i].ComboProductId).ToList();
                                if (cancelLinesSetOne != null && cancelLinesSetOne.Any())
                                {
                                    linesForCancellation.AddRange(cancelLinesSetOne);
                                }
                            }
                            if (trxLineDataForValidationList[i].TrxLineId > -1)
                            {
                                List<Transaction.TransactionLine> cancelLinesSetTwo = reservationBL.BookingTransaction.TrxLines.Where(tl => tl.LineValid
                                                                                                                                         && (tl.DBLineId > 0 ? tl.DBLineId == trxLineDataForValidationList[i].TrxLineId
                                                                                                                                                            : reservationBL.BookingTransaction.TrxLines.IndexOf(tl)
                                                                                                                                                               == trxLineDataForValidationList[i].TrxLineId - 1)).ToList();
                                if (cancelLinesSetTwo != null && cancelLinesSetTwo.Any())
                                {
                                    linesForCancellation.AddRange(cancelLinesSetTwo);
                                }
                            }

                            if (linesForCancellation != null && linesForCancellation.Any())
                            {
                                for (int j = 0; j < linesForCancellation.Count; j++)
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    CancelProductLine(linesForCancellation[j], sqlTrx);
                                }
                            }
                        }
                    }
                    cancellationSuccessful = true;
                }
            }
            else
            {
                cancellationSuccessful = true;
            }
            log.LogMethodExit(cancellationSuccessful);
            return cancellationSuccessful;
        }

        private void CancelProductLine(Transaction.TransactionLine trxLineRecord, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(trxLineRecord, sqlTrx);
            this.Cursor = Cursors.WaitCursor;
            if (trxLineRecord != null && reservationBL != null && reservationBL.ReservationTransactionIsNotNull())
            {
                POSUtils.SetLastActivityDateTime();
                int lineIndex = reservationBL.BookingTransaction.TrxLines.IndexOf(trxLineRecord);
                reservationBL.RemoveProduct(trxLineRecord.ProductID, lineIndex, sqlTrx, false);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();

        }


        private bool OverrideLines()
        {
            log.LogMethodEntry();
            bool allowOverride = false;
            if (trxLineDataForValidationList != null && trxLineDataForValidationList.Any()
                && trxLineDataForValidationList.Exists(tlv => tlv.OptToOverride == true))
            {
                bool allowInactiveProducts = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_INACTIVE_ITEMS_IN_RESERVATION_RESCHEDULE");
                if (allowInactiveProducts)
                {
                    POSUtils.SetLastActivityDateTime();
                    //Do you want to proceed with products that are no longer available?
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2711), MessageContainerList.GetMessage(executionContext, "Override"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        bool mgrApprovalToAllowInactiveProducts = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "MANAGER_APPROVAL_TO_ALLOW_INACTIVE_ITEMS");
                        if (mgrApprovalToAllowInactiveProducts)
                        {
                            int mgrId = -1;
                            if (Authenticate.Manager(ref mgrId) == false)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required"));
                            }
                            POSUtils.SetLastActivityDateTime();
                            allowOverride = true;
                        }
                        else
                        {
                            allowOverride = true; 
                        }
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2712));
                    //Do you want to proceed with products that are no longer available?
                }
            }
            log.LogMethodExit(allowOverride);
            return allowOverride;
        }

        private void ConfirmBooking(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                reservationBL.BookReservation(sqlTrx);
                reservationBL.ConfirmReservation(sqlTrx);
                log.Info("Reservation is CONFIRMED");
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void Scroll_ButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
    }
}
