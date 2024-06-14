/********************************************************************************************
 * Project Name - DiscountCouponStatusUI
 * Description  - UI for DiscountCoupon Status 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      3-Aug-2019    Girish Kundar  added LogMethodEntry() and LogMethodExit()
 *2.150.0     22-Apr-2021   Abhishek       Modified : POS UI Redesign
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.DiscountSetup
{
    /// <summary>
    /// Used for Checking discount coupon status.
    /// </summary>
    public partial class DiscountCouponStatusUI : Form
    {
        private readonly Utilities utilities;
        private  static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AlphaNumericKeyPad keypad;
        private Control currentAlphanumericTextBox;
        private DeviceClass primaryBarcodeScanner;
        private int businessStartTime;

        /// <summary>
        /// Parameterized Constructor.
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="primaryBarcodeScanner"></param>
        public DiscountCouponStatusUI(Utilities utilities, DeviceClass primaryBarcodeScanner)
        {
            log.LogMethodEntry(utilities , primaryBarcodeScanner);
            this.utilities = utilities;
            this.primaryBarcodeScanner = primaryBarcodeScanner;
            if(primaryBarcodeScanner != null)
            {
                primaryBarcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
            }
            InitializeComponent();
            if(int.TryParse(utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessStartTime) == false)
            {
                businessStartTime = 6;
            } 
            utilities.setupDataGridProperties(ref dgvDiscountedProductsDTOList);
            utilities.setupDataGridProperties(ref dgvDiscountsPurchaseCriteriaDTOList);
            utilities.setupDataGridProperties(ref dgvDiscountsDTOList);
            utilities.setLanguage(this);

            discountPercentageDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            discountAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            discountPercentageDataGridViewTextBoxColumn1.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            discountAmountDataGridViewTextBoxColumn1.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            discountedPriceDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();

            log.LogMethodExit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            lblCouponStatus.Text = string.Empty;
            lblEffectiveDate.Text = string.Empty;
            lblExpiryDate.Text = string.Empty;
            discountPurchaseCriteriaDTOListBS.DataSource = new SortableBindingList<DiscountPurchaseCriteriaContainerDTO>();
            discountedProductsDTOListBS.DataSource = new SortableBindingList<DiscountedProductsContainerDTO>();
            if (string.IsNullOrWhiteSpace(txtCouponNumber.Text) == false)
            {
                DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(utilities.ExecutionContext, txtCouponNumber.Text);
                if (discountCouponsBL.CouponStatus == CouponStatus.ACTIVE)
                {
                    lblCouponStatus.Text = utilities.MessageUtils.getMessage("Active coupon");
                }
                else if (discountCouponsBL.CouponStatus == CouponStatus.EXPIRED)
                {
                    lblCouponStatus.Text = utilities.MessageUtils.getMessage("Expired coupon");
                }
                else if (discountCouponsBL.CouponStatus == CouponStatus.INEFFECTIVE)
                {
                    lblCouponStatus.Text = utilities.MessageUtils.getMessage("Issued coupon not yet active");
                }
                else if (discountCouponsBL.CouponStatus == CouponStatus.INVALID)
                {
                    lblCouponStatus.Text = utilities.MessageUtils.getMessage("Invalid coupon");
                }
                else if (discountCouponsBL.CouponStatus == CouponStatus.IN_ACTIVE)
                {
                    lblCouponStatus.Text = utilities.MessageUtils.getMessage("Inactive coupon");
                }
                else if (discountCouponsBL.CouponStatus == CouponStatus.USED)
                {
                    lblCouponStatus.Text = utilities.MessageUtils.getMessage("Used coupon");
                }
                if (discountCouponsBL.DiscountCouponsDTO != null && discountCouponsBL.DiscountCouponsDTO.CouponSetId != -1)
                {
                    if (discountCouponsBL.DiscountCouponsDTO.StartDate != null)
                    {
                        lblEffectiveDate.Text = (discountCouponsBL.DiscountCouponsDTO.StartDate.Value.Date.AddHours(businessStartTime)).ToString("dddd, dd-MMM-yyyy h:mm tt");
                    }
                    if (discountCouponsBL.DiscountCouponsDTO.ExpiryDate != null)
                    {
                        lblExpiryDate.Text = (discountCouponsBL.DiscountCouponsDTO.ExpiryDate.Value.Date.AddDays(1).AddHours(businessStartTime)).ToString("dddd, dd-MMM-yyyy h:mm tt");
                    }
                    if (discountCouponsBL.DiscountCouponsDTO.DiscountId != -1)
                    {
                        DiscountContainerDTO discountContainerDTO = DiscountContainerList.GetDiscountContainerDTO(utilities.ExecutionContext, discountCouponsBL.DiscountCouponsDTO.DiscountId);
                        if (discountContainerDTO != null && discountContainerDTO.DiscountId != -1)
                        {
                            SortableBindingList<DiscountContainerDTO> discountContainerDTOList = new SortableBindingList<DiscountContainerDTO>
                            {
                                discountContainerDTO
                            };
                            discountnsDTOListBS.DataSource = discountContainerDTOList;
                            if (discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList != null &&
                                discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList.Count > 0)
                            {
                                SortableBindingList<DiscountPurchaseCriteriaContainerDTO> discountPurchaseCriteriaDTOList =
                                    new SortableBindingList<DiscountPurchaseCriteriaContainerDTO>(discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList);
                                discountPurchaseCriteriaDTOListBS.DataSource = discountPurchaseCriteriaDTOList;
                            }
                            if (discountContainerDTO.DiscountedProductsContainerDTOList != null &&
                                discountContainerDTO.DiscountedProductsContainerDTOList.Count > 0)
                            {
                                SortableBindingList<DiscountedProductsContainerDTO> discountedProductsDTOList =
                                    new SortableBindingList<DiscountedProductsContainerDTO>(discountContainerDTO.DiscountedProductsContainerDTOList.FindAll(x => x.Discounted == "Y"));
                                discountedProductsDTOListBS.DataSource = discountedProductsDTOList;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            log.LogMethodExit();
        }

        private void btnAlphaKeypad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (currentAlphanumericTextBox == null)
            {
                currentAlphanumericTextBox = txtCouponNumber;
            }
            Point p = currentAlphanumericTextBox.PointToScreen(Point.Empty);
            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, currentAlphanumericTextBox);
                if (p.Y + 60 + keypad.Height < Screen.PrimaryScreen.WorkingArea.Height)
                    keypad.Location = new Point(this.Location.X, p.Y + 60);
                else
                    keypad.Location = new Point(this.Location.X, this.PointToScreen(currentAlphanumericTextBox.Location).Y - keypad.Height);
                keypad.Show();
            }
            else if (keypad.Visible)
                keypad.Hide();
            else
            {
                keypad.Show();
            }
            log.LogMethodExit();
        }
        private void DiscountCouponStatusUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtCouponNumber.Focus();
            LoadProductsDTOList();
            LoadCategoryDTOList();
            log.LogMethodExit();
        }

        private void LoadProductsDTOList()
        {
            log.LogMethodEntry();
            Products products = new Products();
            List<ProductsDTO> productsDTOList;
            ProductsFilterParams productsFilterParams = new ProductsFilterParams();
            productsDTOList = products.GetProductDTOList(productsFilterParams);
            if (productsDTOList == null)
            {
                productsDTOList = new List<ProductsDTO>();
            }
            productsDTOList.Insert(0, new ProductsDTO());
            productsDTOList[0].ProductId = -1;
            productsDTOList[0].ProductName = "-All-";


            dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.DataSource = productsDTOList;
            dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.DisplayMember = "ProductName";
            dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.ValueMember = "ProductId";

            dgvDiscountedProductsDTOListProductIdComboBoxColumn.DataSource = productsDTOList;
            dgvDiscountedProductsDTOListProductIdComboBoxColumn.DisplayMember = "ProductName";
            dgvDiscountedProductsDTOListProductIdComboBoxColumn.ValueMember = "ProductId";

            log.LogMethodExit();
        }

        private void LoadCategoryDTOList()
        {
            log.LogMethodEntry();
            CategoryList categoryList = new CategoryList(utilities.ExecutionContext);
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            List<CategoryDTO> categoryDTOList = categoryList.GetAllCategory(searchParameters);
            if (categoryDTOList == null)
            {
                categoryDTOList = new List<CategoryDTO>();
            }
            categoryDTOList.Insert(0, new CategoryDTO());
            categoryDTOList[0].CategoryId = -1;
            categoryDTOList[0].Name = "-All-";
            dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.DataSource = categoryDTOList;
            dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.DisplayMember = "Name";
            dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.ValueMember = "CategoryId";

            dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.DataSource = categoryDTOList;
            dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.DisplayMember = "Name";
            dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.ValueMember = "CategoryId";

            log.LogMethodExit();
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);//Added for logger function on 08-Mar-2016
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                string scannedBarcode = utilities.ProcessScannedBarCode(checkScannedEvent.Message, utilities.ParafaitEnv.LEFT_TRIM_BARCODE, utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);

                this.Invoke((MethodInvoker)delegate
                {
                    txtCouponNumber.Text = scannedBarcode;
                    btnOK_Click(null, null);
                });
            }
            log.LogMethodExit();//Added for logger function on 08-Mar-2016
        }
    }
}
