/********************************************************************************************
 * Project Name - Reservation
 * Description  - BookingPackageDetails form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.0      26-Mar-2019   Guru S A                Created for Booking phase 2 enhancement changes 
 *2.80.0      09-Jun-2020   Jinto Thomas            Enable Active flag for Comboproduct data
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmBookingPackageDetails : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int bookingProductId = -1;
        private Utilities utilities = null;
        private int rowHeight = 30;
        private int yAxisPosition = 0;
        public frmBookingPackageDetails(int bookingProductId)
        {
            log.LogMethodEntry(bookingProductId);
            this.utilities = POSStatic.Utilities;
            InitializeComponent();
            this.bookingProductId = bookingProductId;
            utilities.setLanguage();
            this.DialogResult = DialogResult.OK;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void frmBookingPackageDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ShowPackageContent();
            utilities.setLanguage(this);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void ShowPackageContent()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (bookingProductId > -1)
            {
                Products productsBL = new Products(bookingProductId); 
                this.Text = this.Text + " : " + productsBL.GetProductsDTO.ProductName + "";
                BookingProduct bookingProductInfo = productsBL.GetBookingProductContent(true);
                if (bookingProductInfo != null)
                {
                    pnlProductDetails.BorderStyle = BorderStyle.Fixed3D;
                    pnlProductDetails.BackColor = Color.White;
                    if (bookingProductInfo.BookingProductPackagelist != null && bookingProductInfo.BookingProductPackagelist.Count > 0)
                    {
                        foreach (BookingPackageProduct packageItem in bookingProductInfo.BookingProductPackagelist)
                        { 
                            SetProductDetails(packageItem, 10, yAxisPosition); 
                        }
                    }
                    pnlProductDetails.Refresh(); 
                }
            }
            log.LogMethodExit();
        }

        private void SetProductDetails(BookingPackageProduct packageItem, int locationX, int locationY)
        {
            log.LogMethodEntry(packageItem, locationX, locationY);
            POSUtils.SetLastActivityDateTime();
            Label lblProductName = new Label();
            lblProductName.Name = "ProductName" + packageItem.ProductId.ToString();
            lblProductName.Text = packageItem.ProductName + (packageItem.ProductType == ProductTypeValues.COMBO? " ( " + packageItem.Quantity.ToString()+ " Guests)": "");
            lblProductName.Location = new Point(locationX+5, locationY);
            lblProductName.AutoSize = false;
            lblProductName.TextAlign = ContentAlignment.MiddleLeft;
            lblProductName.BackColor = Color.AliceBlue;
            lblProductName.Width = pnlProductDetails.Width - 45;
            //Label lblProductType = new Label();
            //lblProductType.Name = "ProductType" + packageItem.ProductId.ToString();
            //lblProductType.Text = packageItem.ProductType;
            //lblProductType.Location = new Point(lblProductName.Location.X + lblProductName.Size.Width+5, locationY * rowCount);
            //Label lblProductPrice = new Label();
            //lblProductPrice.Name = "ProductPrice" + packageItem.ProductId.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //lblProductPrice.Text = packageItem.Price.ToString();
            //lblProductPrice.Location = new Point(lblProductType.Location.X + lblProductType.Size.Width + 5, locationY * rowCount);
            //Label lblProductQty = new Label();
            //lblProductQty.Name = "ProductQty" + packageItem.ProductId.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
            //lblProductQty.Text = packageItem.Quantity.ToString();
            //lblProductQty.Location = new Point(lblProductPrice.Location.X + lblProductPrice.Size.Width + 5, locationY * rowCount);
            //Label lblPriceInclusive = new Label();
            //lblPriceInclusive.Name = "PriceInclusive" + packageItem.ProductId.ToString();
            //lblPriceInclusive.Text = packageItem.PriceInclusive.ToString();
            //lblPriceInclusive.Location = new Point(lblProductQty.Location.X + lblProductQty.Size.Width + 5, locationY * rowCount);
            pnlProductDetails.Controls.Add(lblProductName);
            yAxisPosition = yAxisPosition+ rowHeight; 
            if (packageItem.BookingPackageProductContents != null && packageItem.BookingPackageProductContents.Count > 0)
            { 
                foreach (BookingProductContent packageChildItem in packageItem.BookingPackageProductContents)
                {                       
                    SetProductContents(packageChildItem, lblProductName.Location.X+5, yAxisPosition); 
                }
                //pnlChildProduct.Refresh();
            }
            log.LogMethodExit();
        }

        private void SetProductContents( BookingProductContent packageChildItem, int locationX, int locationY)
        {
            log.LogMethodEntry(packageChildItem, locationX, locationY);
            POSUtils.SetLastActivityDateTime();
            Label lblProductName = new Label();
            lblProductName.Name = "ProductName" + packageChildItem.ProductId.ToString();
            lblProductName.Text = packageChildItem.ProductName;
            lblProductName.Location = new Point(locationX + 5, locationY);
            lblProductName.TextAlign = ContentAlignment.MiddleLeft;
            lblProductName.AutoSize = false; 
            pnlProductDetails.Controls.Add(lblProductName);
            yAxisPosition = yAxisPosition + rowHeight;
            lblProductName.BackColor = Color.White;
            lblProductName.Width = pnlProductDetails.Width - 50;

            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.Close();
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
