/********************************************************************************************
 * Project Name - Product Details
 * Description  - Form to show product details
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        02-FEB-2019      Nitin Pai      Created 
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Parafait_POS
{
    public partial class ProductDetailsUserControl : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;


        public ProductDetailsUserControl(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        public void SearchAndLoadProductDetails(int productId)
        {
            log.LogMethodEntry();

            try
            {
                Products products = new Products(productId);

                lblProductName.Text = products.GetProductsDTO.ProductName;
                
                lblTax.Text = products.GetProductsDTO.TaxName.ToString();
                lblFPrice.Text = products.GetProductsDTO.Price.ToString(utilities.getAmountFormat());
                lblFaceValue.Text = products.GetProductsDTO.FaceValue == -1 ? "" : products.GetProductsDTO.FaceValue.ToString(utilities.getAmountFormat());

                if (products.GetProductsDTO.TaxInclusivePrice.ToString() == "Y")
                {
                    cboxTaxInclusive.Checked = true;
                }
                else
                {
                    cboxTaxInclusive.Checked = false;
                }

                lblProductType.Text = products.GetProductsDTO.ProductType == null ? "" : products.GetProductsDTO.ProductType;
                txtDescription.Text = products.GetProductsDTO.Description;
                webBrowserDescription.DocumentText = products.GetProductsDTO.WebDescription;

                
                if (products.GetProductsDTO.ProductType == "MANUAL" || products.GetProductsDTO.ProductType == "COMBO")
                {
                    lblFaceValueName.Visible = lblFaceValue.Visible = lblBonusName.Visible = lblBonus.Visible = lblCourtesy.Visible = lblCourtesyName.Visible = 
                        lblCredits.Visible = lblCreditName.Visible = lblTime.Visible = lblTimeName.Visible = false;

                    int offset = lblFaceValueName.Location.Y - lblFPriceName.Location.Y;

                    lblDescName.Location = new Point(lblDescName.Location.X, lblFPriceName.Location.Y + offset);
                    txtDescription.Location = new Point(txtDescription.Location.X, lblDescName.Location.Y + 30);
                    txtDescription.Height = 365;

                    lblWebDescName.Location = new Point(lblWebDescName.Location.X, lblFPriceName.Location.Y + offset);
                    webBrowserDescription.Location = new Point(webBrowserDescription.Location.X, lblWebDescName.Location.Y + 30);
                    webBrowserDescription.Height = 365;

                }
                else
                {
                    lblFaceValueName.Visible = lblFaceValue.Visible = lblBonusName.Visible = lblBonus.Visible = lblCourtesy.Visible = lblCourtesyName.Visible =
                        lblCredits.Visible = lblCreditName.Visible = lblTime.Visible = lblTimeName.Visible = true;


                    lblCredits.Text = products.GetProductsDTO.Credits == -1 ? "" : products.GetProductsDTO.Credits.ToString(utilities.getNumberFormat());
                    lblCourtesy.Text = products.GetProductsDTO.Courtesy == -1 ? "" : products.GetProductsDTO.Courtesy.ToString(utilities.getNumberFormat());
                    lblBonus.Text = products.GetProductsDTO.Bonus == -1 ? "" : products.GetProductsDTO.Bonus.ToString(utilities.getNumberFormat());
                    lblTime.Text = products.GetProductsDTO.Time == -1 ? "" : products.GetProductsDTO.Time.ToString(utilities.getNumberFormat());


                    int offset = lblFaceValueName.Location.Y - lblFPriceName.Location.Y;

                    lblDescName.Location = new Point(lblDescName.Location.X, lblFaceValueName.Location.Y + offset);
                    txtDescription.Location = new Point(txtDescription.Location.X, lblDescName.Location.Y + 30);
                    txtDescription.Height = 340;

                    lblWebDescName.Location = new Point(lblWebDescName.Location.X, lblFaceValueName.Location.Y + offset);
                    webBrowserDescription.Location = new Point(webBrowserDescription.Location.X, lblWebDescName.Location.Y + 30);
                    webBrowserDescription.Height = 340;
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured in SearchAndLoadProductDetails", ex);
                // handle the error, do not propagate to POS
            }

            log.LogMethodEntry();
        }
    }
}
