/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - user control for Playground Quantity Screen
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
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class usrCtrlCheckinCheckoutProductsQty : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductsContainerDTO comboChildProdsContainerDTO;
        internal delegate void SelctedQuantity(usrCtrlCheckinCheckoutProductsQty usrCtrlComboChildProductsQty);
        internal SelctedQuantity selctedQuantity;
        private int qtySelected;
        private int minQuantity;
        private int maxQuantity;
        private string errMsg = "";
        private bool showKeypad = false;

        internal int QtySelected { get { return qtySelected; } set { qtySelected = value; txtQty.Text = value.ToString(); } }
        internal ProductsContainerDTO ComboChildProductContainerDTO { get { return comboChildProdsContainerDTO; } set {} }
        internal int MinQuantity { get { return minQuantity; } }
        internal int MaxQuantity { get { return maxQuantity; } }
        internal string ErrMsg { get { return errMsg; } }
        internal bool ShowKeypad { get { return showKeypad; } }

        public usrCtrlCheckinCheckoutProductsQty(int productId, int minQty, int maxQty)
        {
            log.LogMethodEntry(productId, minQty, maxQty);
            InitializeComponent();
            this.comboChildProdsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, productId);
            this.minQuantity = minQty;
            this.maxQuantity = maxQty;
            if (comboChildProdsContainerDTO == null)
            {
                string msg = "Failed to get product details";
                KioskStatic.logToFile("Error: " + msg);
                log.Error(msg);
                return;
            }

            if((minQuantity > 0 && maxQuantity > 0) && (minQuantity == maxQuantity))
            {
                pcbDecreaseQty.Enabled = false;
                pcbIncreaseQty.Enabled = false;
                txtQty.Enabled = false;
                log.LogVariableState("ProductId: ", productId);
                log.LogVariableState("Minimum Quantity", minQuantity);
                log.LogVariableState("Maximum Quantity", maxQuantity);
                KioskStatic.logToFile("Disabled to edit the quantity as Minimum Quantity and Maximum Quantity are set equal");
            }
            try
            {
                SetDisplayElements(comboChildProdsContainerDTO);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error setting display elements in usrCtrlComboChildProductsQty" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetBackgroundImage(ProductsContainerDTO productsContainerDTO)
        {
            log.LogMethodEntry(productsContainerDTO);

            string imageFolder = KioskStatic.Utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            if (productsContainerDTO.ImageFileName.ToString().Trim() != string.Empty)
            {
                try
                {
                    object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                            new SqlParameter("@FileName", imageFolder + "\\" + productsContainerDTO.ImageFileName.ToString()));

                    this.usrControlPanel.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                    btnSampleProduct.Text = "";
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + productsContainerDTO.ImageFileName.ToString());
                    this.usrControlPanel.BackgroundImage = ThemeManager.CurrentThemeImages.ChooseProductButton;
                }
            }
            else
            {
                this.usrControlPanel.BackgroundImage = ThemeManager.CurrentThemeImages.ChooseProductButton;
            }
            this.pcbDecreaseQty.BackgroundImage = productsContainerDTO.QuantityPrompt.Equals("Y") ? 
                ThemeManager.CurrentThemeImages.DecreaseQtyButton : ThemeManager.CurrentThemeImages.DecreaseQtyDisabledButton;
            this.pcbIncreaseQty.BackgroundImage = productsContainerDTO.QuantityPrompt.Equals("Y") ? 
                ThemeManager.CurrentThemeImages.IncreaseQtyButton : ThemeManager.CurrentThemeImages.IncreaseQtyDisabledButton;

            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                btnSampleProduct.ForeColor = KioskStatic.CurrentTheme.ComboChildProductsQtyProductButtonTextForeColor; // product text
                txtQty.ForeColor = KioskStatic.CurrentTheme.ComboChildProductsQtyQuantityTextForeColor; // product text
                lblAgeCriteriaHeader.ForeColor = KioskStatic.CurrentTheme.PackageDetailsLblHeaderTextForeColor;
                lblAgeCriteria.ForeColor = KioskStatic.CurrentTheme.PackageDetailsLblTextForeColor; //age criteria label
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("ERROR: Error while setting customized font colors for the UI elements of usrCtrlComboChildProductsQty: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetDisplayElements(ProductsContainerDTO productsContainerDTO)
        {
            log.LogMethodEntry(productsContainerDTO);
            try
            {
                string productName = KioskHelper.GetProductName(productsContainerDTO.ProductId);
                btnSampleProduct.Name = productName;
                btnSampleProduct.Tag = productsContainerDTO.ProductId;
                btnSampleProduct.Font = new Font(KioskStatic.CurrentTheme.DefaultFont.Name, btnSampleProduct.Font.Size, btnSampleProduct.Font.Style);
                txtQty.Font = new Font(KioskStatic.CurrentTheme.DefaultFont.Name, txtQty.Font.Size, btnSampleProduct.Font.Style);
                lblAgeCriteriaHeader.Font =
                    lblAgeCriteria.Font = new Font(KioskStatic.CurrentTheme.DefaultFont.Name, lblAgeCriteria.Font.Size, btnSampleProduct.Font.Style);

                btnSampleProduct.Text = productName;
                txtQty.Text = minQuantity.ToString();
                qtySelected = minQuantity;

                panelAgeCriteria.Visible = (productsContainerDTO.CustomerProfilingGroupId != -1) ? true : false;
                lblAgeCriteriaHeader.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4812) + ":";//Age Criteria

                if (productsContainerDTO.AgeLowerLimit > KioskStatic.AGE_LOWER_LIMIT && productsContainerDTO.AgeUpperLimit < KioskStatic.AGE_UPPER_LIMIT)
                {
                    lblAgeCriteria.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4777, productsContainerDTO.AgeLowerLimit, productsContainerDTO.AgeUpperLimit);//1 to 13 yrs
                }
                else if (productsContainerDTO.AgeLowerLimit > KioskStatic.AGE_LOWER_LIMIT && productsContainerDTO.AgeUpperLimit == KioskStatic.AGE_UPPER_LIMIT)
                {
                    //lblAgeCriteria.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4779, productsContainerDTO.AgeLowerLimit);//above 1 yrs

                    lblAgeCriteria.Text = (productsContainerDTO.AgeLowerLimit == 1) ?
                        MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5174, productsContainerDTO.AgeLowerLimit) //Above 1 yr
                        : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4779, productsContainerDTO.AgeLowerLimit); //Above 2 yrs
                }
                else if (productsContainerDTO.AgeLowerLimit == KioskStatic.AGE_LOWER_LIMIT && productsContainerDTO.AgeUpperLimit < KioskStatic.AGE_UPPER_LIMIT)
                {
                    lblAgeCriteria.Text = (productsContainerDTO.AgeLowerLimit > 1) ?
                        MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4778, productsContainerDTO.AgeUpperLimit) //below 18 yrs
                        : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5172, productsContainerDTO.AgeUpperLimit); //below 1 yr
                }

                if (productsContainerDTO.QuantityPrompt.Equals("N"))
                    txtQty.Enabled = false;

                SetBackgroundImage(productsContainerDTO);
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Error setting display elements in usrCtrlComboChildProductsQty" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void FireDelegateMethod()
        {
            log.LogMethodEntry();
            try
            {
                if (selctedQuantity != null)
                {
                    selctedQuantity(this);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("ERROR: Error while Firing usrControlFundsDonations product Delegate Method: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void pcbDecreaseQty_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (!txtQty.Enabled)
                return;

                errMsg = "";
            showKeypad = false;
            try
            {
                Focus();
                qtySelected--;
                if (qtySelected < 0)
                    qtySelected = 0;

                if (qtySelected < minQuantity)
                {
                    qtySelected = minQuantity;
                    errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4345, minQuantity); //For this product, you must select a minimum of &1 quantity
                }

                txtQty.Text = qtySelected.ToString();
                FireDelegateMethod();

            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in pcbDecreaseQty_Click() of usrCtrlComboChildProductsQty" + ex.Message);
            }

            log.LogMethodExit();
        }

        private void pcbIncreaseQty_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (!txtQty.Enabled)
                return;

            errMsg = "";
            showKeypad = false;
            try
            {
                Focus();
                qtySelected++;

                //maxQuantity may come as 0 when not defined.
                if ((maxQuantity > 0) && (qtySelected > maxQuantity))
                {
                    qtySelected = maxQuantity;
                    errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4344, maxQuantity); //For this product, you can choose no more than &1 quantities
                }
                txtQty.Text = qtySelected.ToString();
                FireDelegateMethod();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in pcbIncreaseQty_Click() of usrCtrlComboChildProductsQty" + ex.Message);
            }

            log.LogMethodExit();
        }

        private void txtQty_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (comboChildProdsContainerDTO.QuantityPrompt.Equals("N"))
                return;

            errMsg = "";
            txtQty.Text = qtySelected.ToString();
            showKeypad = true;
            FireDelegateMethod();
            log.LogMethodExit();
        }
    }
}
