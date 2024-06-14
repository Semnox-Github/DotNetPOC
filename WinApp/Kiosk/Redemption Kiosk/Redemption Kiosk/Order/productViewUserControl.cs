/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Product View user control
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
    public partial class productViewUserControl : UserControl
    {
        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        RedemptionGiftsDTO redemptionGift;
        RedemptionBL redemptionOrder;
        int productCount = 0;
        ProductBL product;
        public delegate void RefreshViewOrderDelegate();
        public RefreshViewOrderDelegate setRefreshCallBack;

        public delegate void ResetTimeOutDelegate();
        public ResetTimeOutDelegate ResetTimeOutCallback;

        double redemptionDiscount;
        public productViewUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        public productViewUserControl(RedemptionGiftsDTO redemptionGift , RedemptionBL redemptionOrder, double redemptionDiscount) : this()
        {
            log.LogMethodEntry();
            this.redemptionGift = redemptionGift;
            this.redemptionOrder = redemptionOrder;
            this.product = new ProductBL(Common.utils.ExecutionContext, redemptionGift.ProductId);
            this.redemptionDiscount = redemptionDiscount;
            log.LogMethodExit();
        }

        void RefreshViewUserControl()
        {
            log.LogMethodEntry();
            productCount = redemptionGift.ProductQuantity;
            lblProductCount.Text = redemptionGift.ProductQuantity.ToString();
            lblProductName.Text = redemptionGift.ProductName;
            lblProductTicket.Text = redemptionGift.Tickets.ToString();
            lblTicketCount.Text = redemptionGift.Tickets.ToString();
            string imageFolder = Common.utils.getParafaitDefaults("IMAGE_DIRECTORY");
            Bitmap productImage = Properties.Resources.default_product_image;
            if (redemptionGift.ImageFileName.ToString().Trim() != string.Empty)
            {
                try
                {
                    object o = Common.utils.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                           new SqlParameter("@FileName", imageFolder + "\\" + redemptionGift.ImageFileName.ToString()));
                    pbProductImage.BackgroundImage = Common.utils.ConvertToImage(o);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    pbProductImage.BackgroundImage = productImage;
                }
            }
            else
            {
                pbProductImage.BackgroundImage = productImage;
            }
        }

        void AddGiftQty()
        {
            log.LogMethodEntry();
            int avilableTickets = redemptionOrder.GetAvailbleTickets();
            if (avilableTickets <= 0 || (redemptionGift.Tickets > avilableTickets))
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
            UpdateGiftQuantity(); 
            log.LogMethodExit();
        }
         
        void ReduceGiftQty()
        {
            log.LogMethodEntry();
            if (productCount > 0)
            {
                productCount -= 1;
                UpdateGiftQuantity(); 
            } 
            log.LogMethodExit();

        }
        private void ProductViewUserControl_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshViewUserControl();
            Common.utils.setLanguage(this);
            log.LogMethodExit();
        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CallResetTimeOutCallback();
                frmRedemptionKioskGiftInfo frm = new frmRedemptionKioskGiftInfo(redemptionGift.ProductName, redemptionGift.ProductDescription, Convert.ToInt32(redemptionGift.Tickets), redemptionGift.ImageFileName, pbProductImage.BackgroundImage);
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

        private void BtnPlus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CallResetTimeOutCallback();
                AddGiftQty();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(); 
        }

        private void BtnMinus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CallResetTimeOutCallback();
                ReduceGiftQty();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CallResetTimeOutCallback();
                DeleteGift();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void UpdateGiftQuantity()
        {
            log.LogMethodEntry();            
            if (product.IsAvailableInInventory(Common.utils.ExecutionContext,productCount))
            {
                redemptionOrder.AddGift(product.getProductDTO, productCount, this.redemptionDiscount);
                redemptionGift = redemptionOrder.GetGiftEntry(product.getProductDTO.ProductId);
            }
            else
            {
                Common.ShowMessage(Common.utils.MessageUtils.getMessage(1612, product.getProductDTO.ProductName));
                //'Stock is empty for the product: &1. Cannot add this product
            }
            setRefreshCallBack();
            log.LogMethodExit();
        }

        void DeleteGift()
        {
            log.LogMethodEntry();
            redemptionOrder.RemoveGift(product.getProductDTO.ProductId);
            redemptionGift = null;
            setRefreshCallBack();
            log.LogMethodExit();
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
