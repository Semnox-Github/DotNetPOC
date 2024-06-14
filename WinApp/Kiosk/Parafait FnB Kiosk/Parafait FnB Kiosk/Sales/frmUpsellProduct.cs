/********************************************************************************************
* Project Name - Parafait_Kiosk -frmUpsellProduct.cs
* Description  - frmUpsellProduct 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class frmUpsellProduct : BaseForm
    {
        public frmUpsellProduct(string Greeting1, string ProductName, string OfferMessage)
        {
            log.LogMethodEntry(Greeting1, ProductName, OfferMessage);
            InitializeComponent();
            lblGreeting1.Text = Greeting1;
            lblGreeting2.Text = "(" + ProductName + ")";

            lblOfferMessage1.Text = lblOfferMessage2.Text = lblOfferMessage3.Text = "";

            string[] lines = OfferMessage.Split('\n');
            lblOfferMessage1.Text = lines[0];
            if (lines.Length > 1)
                lblOfferMessage2.Text = lines[1];
            if (lines.Length > 2)
                lblOfferMessage3.Text = lines[2];
            log.LogMethodExit();
        }

        public frmUpsellProduct(string Greeting1, string ProductName, string OfferMessage, Image DisplayImage)
            : this(Greeting1, ProductName, OfferMessage)
        {
            log.LogMethodEntry(Greeting1, ProductName, OfferMessage, DisplayImage);
            if (DisplayImage != null)
            {
                panelOfferImage.BackgroundImage = DisplayImage;
                panelOfferImage.BackgroundImageLayout = ImageLayout.Center;
                if (string.IsNullOrEmpty(Greeting1) && string.IsNullOrEmpty(ProductName) && string.IsNullOrEmpty(OfferMessage))
                {
                    panelOfferImage.Size = DisplayImage.Size;
                    panelOfferImage.Left = (this.Width - panelOfferImage.Width)/2;
                    lblGreeting1.Visible =
                       lblGreeting2.Visible =
                       lblOfferMessage1.Visible =
                       lblOfferMessage2.Visible =
                       lblOfferMessage3.Visible = false;
                }
            }
            log.LogMethodExit();
        }

        private void frmUpsellProduct_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                object screenId = Common.utils.executeScalar(@"select screenId 
                                                        from AppScreens 
                                                        where CodeObjectName = 'frmUpsell'");
                if (screenId != null)
                {
                    _screenModel = new ScreenModel(Convert.ToInt32(screenId));
                    RenderPanelContent(_screenModel, panelOfferImage, 1);
                }
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }
    }
}
