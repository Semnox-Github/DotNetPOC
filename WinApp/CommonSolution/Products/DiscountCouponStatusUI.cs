using Semnox.Core;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.GenericUtilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Category;

namespace Semnox.Parafait.Products
{
    /// <summary>
    /// Used for Checking discount coupon status.
    /// </summary>
    public partial class DiscountCouponStatusUI : Form
    {
        Utilities utilities;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AlphaNumericKeyPad keypad;
        Control currentAlphanumericTextBox;
        DeviceClass primaryBarcodeScanner;
        int businessStartTime;

        /// <summary>
        /// Parameterized Constructor.
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="primaryBarcodeScanner"></param>
        public DiscountCouponStatusUI(Utilities utilities, DeviceClass primaryBarcodeScanner)
        {
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
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            utilities.setupDataGridProperties(ref dgvDiscountedProductsDTOList);
            utilities.setupDataGridProperties(ref dgvDiscountsPurchaseCriteriaDTOList);
            utilities.setupDataGridProperties(ref dgvDiscountsDTOList);
            utilities.setLanguage(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnOK_Click() Event");
            lblCouponStatus.Text = string.Empty;
            lblEffectiveDate.Text = string.Empty;
            lblExpiryDate.Text = string.Empty;
            discountnsDTOListBS.DataSource = new SortableBindingList<DiscountsDTO>();
            discountPurchaseCriteriaDTOListBS.DataSource = new SortableBindingList<DiscountPurchaseCriteriaDTO>();
            discountedProductsDTOListBS.DataSource = new SortableBindingList<DiscountedProductsDTO>();
            if (string.IsNullOrWhiteSpace(txtCouponNumber.Text) == false)
            {
                DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(txtCouponNumber.Text);
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
                        DiscountsBL discountsBL = new DiscountsBL(discountCouponsBL.DiscountCouponsDTO.DiscountId);
                        if (discountsBL.DiscountsDTO != null && discountsBL.DiscountsDTO.DiscountId != -1)
                        {
                            SortableBindingList<DiscountsDTO> discountDTOList = new SortableBindingList<DiscountsDTO>();
                            discountDTOList.Add(discountsBL.DiscountsDTO);
                            discountnsDTOListBS.DataSource = discountDTOList;
                            if (discountsBL.DiscountsDTO.DiscountPurchaseCriteriaDTOList != null &&
                                discountsBL.DiscountsDTO.DiscountPurchaseCriteriaDTOList.Count > 0)
                            {
                                SortableBindingList<DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTOList =
                                    new SortableBindingList<DiscountPurchaseCriteriaDTO>(discountsBL.DiscountsDTO.DiscountPurchaseCriteriaDTOList);
                                discountPurchaseCriteriaDTOListBS.DataSource = discountPurchaseCriteriaDTOList;
                            }
                            if (discountsBL.DiscountsDTO.DiscountedProductsDTOList != null &&
                                discountsBL.DiscountsDTO.DiscountedProductsDTOList.Count > 0)
                            {
                                SortableBindingList<DiscountedProductsDTO> discountedProductsDTOList =
                                    new SortableBindingList<DiscountedProductsDTO>(discountsBL.DiscountsDTO.DiscountedProductsDTOList.FindAll(x=>x.Discounted == "Y"));
                                discountedProductsDTOListBS.DataSource = discountedProductsDTOList;
                            }
                        }
                    }

                }
            }
            log.Debug("Ends-btnOK_Click() Event");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click() Event");
            this.DialogResult = DialogResult.Cancel;
            log.Debug("Ends-btnClose_Click() Event");
        }

        private void btnAlphaKeypad_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnAlphaKeypad_Click() Event");
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
            log.Debug("Ends-btnAlphaKeypad_Click() Event");
        }
        private void DiscountCouponStatusUI_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-DiscountCouponStatusUI_Load() Event");
            txtCouponNumber.Focus();
            LoadProductsDTOList();
            LoadCategoryDTOList();
            log.Debug("Ends-DiscountCouponStatusUI_Load() Event");
        }

        private void LoadProductsDTOList()
        {
            log.Debug("Starts-LoadProductsDTOList() method.");
            Semnox.Parafait.Product.Products products = new Semnox.Parafait.Product.Products();
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

            log.Debug("Ends-LoadProductsDTOList() Method");
        }

        private void LoadCategoryDTOList()
        {
            log.Debug("Starts-LoadCategoryDTOList() method.");
            CategoryList categoryList = new CategoryList(machineUserContext);
            List<CategoryDTO> categoryDTOList;
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            categoryDTOList = categoryList.GetAllCategory(searchParameters);
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

            log.Debug("Ends-LoadCategoryDTOList() Method");
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.Debug("Starts-BarCodeScanCompleteEventHandle()");//Added for logger function on 08-Mar-2016
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
            log.Debug("Ends-BarCodeScanCompleteEventHandle()");//Added for logger function on 08-Mar-2016
        }
    }
}
