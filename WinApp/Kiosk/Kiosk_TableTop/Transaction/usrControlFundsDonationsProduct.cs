/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - user control for Fund and Donation object in frmDonationAndFundRaiser form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using System.Data.SqlClient;
using Semnox.Core.GenericUtilities;

namespace Parafait_Kiosk
{
    public partial class usrControlFundsDonationsProduct : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool isSelected;
        private ExecutionContext executionContext;
        private bool isFundraiserEvent;
        private ProductsDTO fundDonationProductsDTO;
        private int parentWidth;
        internal delegate void SelctedProductDelegate(int productId);
        internal SelctedProductDelegate selctedProductDelegate;

        public bool IsSelected { get { return isSelected; } set { isSelected = value; } }
        public int GetProductId { get { return (fundDonationProductsDTO != null ? fundDonationProductsDTO.ProductId : -1); } }
        public usrControlFundsDonationsProduct(ExecutionContext executionContext, ProductsDTO productsDTO, bool isFund, int parentWidth)
        {
            log.LogMethodEntry(executionContext, productsDTO, isFund, parentWidth);
            InitializeComponent();
            this.isFundraiserEvent = isFund;
            this.fundDonationProductsDTO = productsDTO;
            this.executionContext = executionContext;
            this.parentWidth = parentWidth;
            SetDisplayElements(fundDonationProductsDTO);
            log.LogMethodExit();
        }
        public void usrControl_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (this.IsSelected)
                {
                    this.isSelected = false;
                    pbxSelectd.Visible = false;
                }
                else
                {
                    this.isSelected = true;
                    pbxSelectd.Visible = true;
                }
                FireDelegateMethod();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in usrControl_Click() of usrControlFundsDonationsProduct : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetBackgroundImage(ProductsDTO productDTO)
        {
            log.LogMethodEntry(productDTO);
            string imageFolder = KioskStatic.Utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            if (productDTO.ImageFileName.ToString().Trim() != string.Empty)
            {
                try
                {
                    object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                            new SqlParameter("@FileName", imageFolder + "\\" + productDTO.ImageFileName.ToString()));

                    this.usrControlPanel.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                    btnSampleProduct.Text = "";
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + productDTO.ImageFileName.ToString());
                    this.usrControlPanel.BackgroundImage = ThemeManager.CurrentThemeImages.ChooseProductButton;
                }
            }
            else
            {
                this.usrControlPanel.BackgroundImage = ThemeManager.CurrentThemeImages.ChooseProductButton;

            }
            log.LogMethodExit();
        }
        private void SetDisplayElements(ProductsDTO productDTO)
        {
            log.LogMethodEntry(productDTO);
            KioskStatic.logToFile("SetDisplayElements() of usrControlFundsDonationsProduct: " + productDTO);

            btnSampleProduct.Name = "ProductButton";
            btnSampleProduct.Tag = productDTO.ProductId;
            btnSampleProduct.Font = new Font(KioskStatic.CurrentTheme.DefaultFont.Name, btnSampleProduct.Font.Size, btnSampleProduct.Font.Style);
            btnSampleProduct.ForeColor = KioskStatic.CurrentTheme.FundsDonationsBtnProductTextForeColor;
            SetBackgroundImage(productDTO);
            string productName = KioskHelper.GetProductName(productDTO.ProductId);
            if (isFundraiserEvent)
            {
                btnSampleProduct.Text = productName.ToString();
            }
            else
            {
                btnSampleProduct.Text = productName.ToString() + " - " +
                                            KioskStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL +
                                            ((int)productDTO.Price).ToString();
            }
            int panelX = ((parentWidth / 2) - (this.usrControlPanel.Width / 2));
            this.usrControlPanel.Location = new Point(panelX, this.usrControlPanel.Location.Y);
            this.Width = parentWidth;
        }

        private void FireDelegateMethod()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("FireDelegateMethod()");
            try
            {
                if (selctedProductDelegate != null)
                {
                    selctedProductDelegate(fundDonationProductsDTO.ProductId);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while Firing usrControlFundsDonations product Delegate Method: "+ ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
