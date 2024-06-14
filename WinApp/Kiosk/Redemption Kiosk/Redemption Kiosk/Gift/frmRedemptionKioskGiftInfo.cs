/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Gift Info UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 ********************************************************************************************/
using Semnox.Parafait.Product;
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

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskGiftInfo : frmRedemptionKioskBaseForm
    {
        ProductDTO productDTO;
        double redemptionDiscount; 
        public frmRedemptionKioskGiftInfo(ProductDTO productDTO, double redemptionDiscount, Image productImage)
        {
            log.LogMethodEntry(productDTO);
            this.productDTO = productDTO;
            this.redemptionDiscount = redemptionDiscount;
            InitializeComponent();
            pbProductImage.BackgroundImage = productImage;
            log.LogMethodExit();
        }

        public frmRedemptionKioskGiftInfo(string productName, string description, int tickets, string imageFileName, Image productImage)
        {
            log.LogMethodEntry(productName, description, tickets, imageFileName);
            if (productDTO == null)
            {
                productDTO = new ProductDTO();
            }
            productDTO.ProductName = productName;
            productDTO.PriceInTickets =  tickets;
            productDTO.Description = description;
            productDTO.ImageFileName = imageFileName;
            this.redemptionDiscount = 1;
            InitializeComponent();
            pbProductImage.BackgroundImage = productImage;
            log.LogMethodExit();
        }

        private void FrmGiftInfo_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //this.Size = this.BackgroundImage.Size;
            lblproductName.Text = productDTO.ProductName;
            lblproductTicket.Text = (productDTO.PriceInTickets * this.redemptionDiscount).ToString();
            txtDescription.Text = productDTO.Description;
            txtDescription.SelectAll();
            txtDescription.SelectionAlignment = HorizontalAlignment.Center;
            txtDescription.DeselectAll(); 
            Common.utils.setLanguage(this);
            log.LogMethodExit();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

    }
}
