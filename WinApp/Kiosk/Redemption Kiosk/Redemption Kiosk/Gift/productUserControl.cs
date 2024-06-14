/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Product user control
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Parafait.Product;
using Semnox.Parafait.Redemption;

namespace Redemption_Kiosk
{
    public partial class ProductUserControl : UserControl
    {
        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ProductBL product; 
        RedemptionBL redemptionOrder;
        int productCount = 0;
        int initialProductCount = 0;
        double redemptionDiscount;
        private Image defaultProductImage;
        public ProductUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        public ProductUserControl(ProductDTO productDTO, RedemptionBL redemptionOrder, double redemptionDiscount, Image defaultProductImage) : this()
        {
            log.LogMethodEntry(productDTO, redemptionOrder, redemptionDiscount, "defaultProductImage");
            this.redemptionOrder = redemptionOrder;
            this.product = new ProductBL(Common.utils.ExecutionContext, productDTO);
            this.redemptionDiscount = redemptionDiscount;
            this.defaultProductImage = defaultProductImage;
            initialProductCount = 0;
            log.LogMethodExit();
        }
       

        void RefreshUserControl()
        {
            log.LogMethodEntry();
            lblProductCount.Text = productCount.ToString();
            log.LogMethodExit();
        }

        private void ProductUserControl_Load(object sender, EventArgs e)
        {
            //this.Size = new Size(323, 371);
            log.LogMethodEntry(sender, e);
            LoadProductDTO();
            RefreshUserControl();
            Common.utils.setLanguage(this);
            log.LogMethodExit();

        }

        void LoadProductDTO()
        {
            log.LogMethodEntry();
            if (product != null)
            {
               
                UpdateGiftDetails();
                if (redemptionOrder.GetAvailbleTickets() >= Convert.ToInt32((product.getProductDTO.PriceInTickets * this.redemptionDiscount)))
                {
                    btnAdd.Enabled = true;
                }
                else
                {
                    btnAdd.Enabled = false;
                }

                lblProductName.Text = product.getProductDTO.ProductName.ToString();
                lblTicketCount.Text = product.getProductDTO.PriceInTickets.ToString();
                string imageFolder = Common.utils.getParafaitDefaults("IMAGE_DIRECTORY"); 
                if (product.getProductDTO.ImageFileName.ToString().Trim() != string.Empty)
                {
                    try
                    {
                        object o = Common.utils.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                new SqlParameter("@FileName", imageFolder + "\\" + product.getProductDTO.ImageFileName.ToString()));

                        pbProductImage.BackgroundImage = Common.utils.ConvertToImage(o);
                    }
                    catch (Exception ex)
                    { 
                        log.Error(product.getProductDTO.ImageFileName.ToString(), ex);
                        pbProductImage.BackgroundImage = defaultProductImage;
                    }
                }
                else
                {
                    pbProductImage.BackgroundImage = defaultProductImage;
                }
            }
            log.LogMethodExit();
        }

        private void BtnMinus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CallResetTimeOutCallback();
                if (productCount > 0)
                {
                    productCount -= 1;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            RefreshUserControl();
            log.LogMethodExit();
        }
        private void BtnPlus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CallResetTimeOutCallback();
                int avilableTickets = redemptionOrder.GetAvailbleTickets();
                int newCount = (productCount + 1) - initialProductCount;
                if (avilableTickets <= 0 || ((newCount * (product.getProductDTO.PriceInTickets * this.redemptionDiscount)) > avilableTickets))
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1632, product.getProductDTO.ProductName));
                    log.LogMethodExit();
                    return;
                }

                if (product.IsAvailableInInventory(Common.utils.ExecutionContext,productCount + 1))
                {
                    productCount += 1;
                }
                else
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1612, product.getProductDTO.ProductName));
                    //'Stock is empty for the product: &1. Cannot add this product
                }

                RefreshUserControl();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        public delegate void RefreshSearchDelegate();
        public RefreshSearchDelegate setRefreshCallBack;

        public delegate void ResetTimeOutDelegate();
        public ResetTimeOutDelegate ResetTimeOutCallback;
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CallResetTimeOutCallback();
                if (btnAdd.Tag.ToString() == "Add" && redemptionOrder.GetAvailbleTickets() < 1)
                {
                    log.LogMethodExit();
                    return;
                }
                else if (btnAdd.Tag.ToString() == "Add" && productCount == 0)
                {
                    productCount = 1;
                }

                if (product.IsAvailableInInventory(Common.utils.ExecutionContext, productCount))
                {
                    redemptionOrder.AddGift(product.getProductDTO, productCount, this.redemptionDiscount);
                    UpdateGiftDetails();
                }
                else
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1612, product.getProductDTO.ProductName));
                    //'Stock is empty for the product: &1. Cannot add this product
                }
                setRefreshCallBack();
                CallResetTimeOutCallback();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void NormalProductDisplay()
        {
            log.LogMethodEntry();
            btnAdd.Tag = "Add";
            this.BackgroundImage = Redemption_Kiosk.Properties.Resources.Product_box_normal;
            btnAdd.Text = Common.utils.MessageUtils.getMessage("Add");
            panelAddProduct.BackgroundImage = Properties.Resources.Panel_AddProduct;
            btnPlus.BackgroundImage = Properties.Resources.Plus_Btn;
            btnMinus.BackgroundImage = Properties.Resources.Minus_Btn;
            log.LogMethodExit();
        }

        void SelectedProductDisplay()
        {
            log.LogMethodEntry();
            btnAdd.Tag = "Update";
            btnAdd.Text = Common.utils.MessageUtils.getMessage("Update");
            this.BackgroundImage = Redemption_Kiosk.Properties.Resources.Product_box_selected;
            panelAddProduct.BackgroundImage = Properties.Resources.Selected_Panel_Add;
            btnPlus.BackgroundImage = Properties.Resources.Selected_Plus_Btn;
            btnMinus.BackgroundImage = Properties.Resources.Selected_Minus_Btn;
            log.LogMethodExit();
        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CallResetTimeOutCallback();
                frmRedemptionKioskGiftInfo frm = new frmRedemptionKioskGiftInfo(product.getProductDTO, this.redemptionDiscount, pbProductImage.BackgroundImage);
                frm.ShowDialog();
                frm.Dispose();
                CallResetTimeOutCallback();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
         

        void UpdateGiftDetails()
        {
            var existingGift = redemptionOrder.GetGiftEntry(product.getProductDTO.ProductId);
            if (existingGift != null)
            {
                productCount = existingGift.ProductQuantity;
                initialProductCount = existingGift.ProductQuantity;
                RefreshUserControl();
                SelectedProductDisplay();
            }
            else
            {
                initialProductCount = 0;
                NormalProductDisplay();
            }
        }

        private void CallResetTimeOutCallback()
        {
            log.LogMethodEntry();
            if (ResetTimeOutCallback != null)
            {
                ResetTimeOutCallback();
            }
            log.LogMethodExit();
        }
    }
}
