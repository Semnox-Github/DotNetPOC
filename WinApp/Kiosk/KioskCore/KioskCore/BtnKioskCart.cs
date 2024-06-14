/********************************************************************************************
* Project Name - Semnox.Parafait.KioskCore
* Description  - KioskTransaction 
* 
**************
**Version Log
**************
*Version         Date             Modified By           Remarks          
*********************************************************************************************
*2.150.1.0       05-Feb-2023      Guru S A              Added for Cart feature in Kiosk 
********************************************************************************************/
using Semnox.Parafait.Languages;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Parafait.KioskCore
{
    public partial class BtnKioskCart : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private Color cartForeColor = Color.White;
        private Color qtyTextColor = Color.White;
        private int counter = 20;
        public event EventHandler BtnCartClick;
        private int cartQty = 0;

        public BtnKioskCart(int itemQty, string btnText, Image cartIcon = null)
        {
            log.LogMethodEntry(itemQty, btnText);
            InitializeComponent();
            if (cartIcon != null)
            {
                this.BackgroundImage = cartIcon;
            }
            cartQty = itemQty;
            string displayQty = FormatQuantity(itemQty);
            this.lblItemQty.Text = displayQty;

            RefreshCartInfo(itemQty, btnText);
            log.LogMethodExit();
        }

        private string FormatQuantity(int itemQty)
        {
            log.LogMethodEntry(itemQty);
            string displayQty;
            string appender = "+";
            int nintyNine = 99;
            lblItemQty.TextAlign = ContentAlignment.MiddleCenter;
            if (itemQty > 99)
            {
                displayQty = nintyNine.ToString(System.Globalization.CultureInfo.InvariantCulture) + appender; 
            }
            else
            {
                displayQty = itemQty.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            log.LogMethodExit(displayQty);
            return displayQty;
        }

        public void RefreshCartInfo(int itemQty, string btnText)
        {
            log.LogMethodEntry(itemQty, btnText);
            this.lblCartText.Text = btnText;
            int oldValue = cartQty; 
            string displayQty = FormatQuantity(itemQty);
            this.lblItemQty.Text = displayQty;
            cartQty = itemQty;
            log.LogMethodExit(); 
        }
        public void SetCartImage(Image cartIcon)
        {
            log.LogMethodEntry();
            if (cartIcon != null)
            {
                this.BackgroundImage = cartIcon;
                //this.btnCart.BackgroundImageLayout = ImageLayout.Stretch;
            } 
            log.LogMethodExit();
        }
        public void SetFont(Font font)
        {
            log.LogMethodEntry();
            if (font != null)
            {
                this.Font = font;
                this.lblCartText.Font = font;
                this.lblItemQty.Font = font;
            }
            log.LogMethodExit();
        }

        public void SetForeColor(Color foreColorCode, Color qtyColorCode)
        {
            log.LogMethodEntry();
            if (foreColorCode != null)
            {
                //this.cartForeColor = foreColorCode;
                this.ForeColor = foreColorCode;
                this.lblCartText.ForeColor = foreColorCode;
                this.lblItemQty.ForeColor = qtyColorCode;
            }
            log.LogMethodExit();
        }
        public virtual void btnCart_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (BtnCartClick != null)
            {
                BtnCartClick(this, EventArgs.Empty);
            }
            log.LogMethodExit();
        } 
    }
}
