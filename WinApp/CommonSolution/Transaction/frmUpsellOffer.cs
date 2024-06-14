/********************************************************************************************
 * Class Name -  Transaction                                                                         
 * Description - frmUpsellOffer 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Parafait.Transaction
{
    public partial class frmUpsellOffer : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Utilities Utilities;
        public bool IsUpsellOfferExist;
        public bool IsSuggestiveSellOfferExist;
        public int UpsellProductId =-1;
        public List<int> suggestiveSellProductIds;

        public frmUpsellOffer(Utilities _utilities, DataRow productRow)
        {
            log.LogMethodEntry(_utilities, productRow);
            InitializeComponent();
            Utilities = _utilities;
            PopulateOfferProducts(productRow, false);
            log.LogMethodExit();
        }

        internal class OffsellData
        {
            public int ProductId;
            public string productName;
            public string OfferMessage;
            public double Price;
            public string Description;
            internal bool IsSelected = false;
        }

        void PopulateOfferProducts(DataRow productRow, bool checkForSuggestiveSellOnly)
        {
            log.LogMethodEntry(productRow, checkForSuggestiveSellOnly);
           try
           {
                IsUpsellOfferExist = false;
                IsSuggestiveSellOfferExist = false;
                suggestiveSellProductIds = new List<int>();
                flpUpsellOffersList.Controls.Clear();

                double noncardButtonSize = 1;
                try
                {
                    noncardButtonSize = Convert.ToDouble(Utilities.getParafaitDefaults("NON-CARD_PRODUCT_BUTTON_SIZE")) / 100.0;
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while converting to Double()" + ex.Message);
                }

                Size btnSize = new Size((int)(btnSample.Width * noncardButtonSize), (int)(btnSample.Height * noncardButtonSize));
                double productPrice = 0;
                try
                {                    
                    if (productRow["TaxInclusivePrice"].ToString() == "Y")
                    {
                        productPrice = Convert.ToDouble(productRow["price"]);
                    }
                    else
                    {
                        productPrice = Convert.ToDouble(productRow["price"]) * (1 + Convert.ToDouble(productRow["tax_percentage"]) / 100.0);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while converting to Double()" + ex.Message);
                }
                lblProductname.Text = productRow["product_name"].ToString() + " with " + (productPrice).ToString(Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);

                UpsellOffer upsellOffer = new UpsellOffer(Utilities.ExecutionContext);
                List<UpsellOfferProductsDTO> UpsellOfferProductsList = null;
                if(!checkForSuggestiveSellOnly)
                    UpsellOfferProductsList = upsellOffer.GetUpsellOfferProductsDTOList(Convert.ToInt32(productRow["product_id"]));
               // throw new Exception("Test");
                if (UpsellOfferProductsList != null && UpsellOfferProductsList.Count > 0 && checkForSuggestiveSellOnly == false)
                {
                  IsUpsellOfferExist = true;
                  IsSuggestiveSellOfferExist = false;
                  foreach (UpsellOfferProductsDTO d in UpsellOfferProductsList)
                  {
                    Button btnOfferProduct = new Button();
                    // btnOfferProduct.Size = btnSample.Size;
                    btnOfferProduct.Size = btnSize;
                    btnOfferProduct.Font = btnSample.Font;

                    btnOfferProduct.BackColor = Color.Transparent;
                    btnOfferProduct.BackgroundImage = btnSample.BackgroundImage;
                    btnOfferProduct.BackgroundImageLayout = ImageLayout.Zoom;
                    btnOfferProduct.FlatStyle = btnSample.FlatStyle;
                    btnOfferProduct.ForeColor = btnSample.ForeColor;
                    btnOfferProduct.FlatAppearance.BorderColor = btnSample.FlatAppearance.BorderColor;
                    btnOfferProduct.FlatAppearance.BorderSize = btnSample.FlatAppearance.BorderSize;
                    btnOfferProduct.FlatAppearance.CheckedBackColor = btnSample.FlatAppearance.CheckedBackColor;
                    btnOfferProduct.FlatAppearance.MouseDownBackColor = btnSample.FlatAppearance.MouseDownBackColor;
                    btnOfferProduct.FlatAppearance.MouseOverBackColor = btnSample.FlatAppearance.MouseOverBackColor;
                    btnOfferProduct.Text = d.ProductName + Environment.NewLine  + d.Price.ToString(Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);

                    OffsellData lclUpsellData = new OffsellData();
                    lclUpsellData.ProductId = d.ProductId;
                    lclUpsellData.productName = d.ProductName;
                    lclUpsellData.Price = d.Price;
                    lclUpsellData.OfferMessage = d.OfferMessage;
                    lclUpsellData.Description = d.Description;
                    lclUpsellData.IsSelected = false;
                    btnOfferProduct.Tag = lclUpsellData;

                    btnOfferProduct.Click += btnOfferProduct_Click;
                    flpUpsellOffersList.Controls.Add(btnOfferProduct);
                    }
                }
                else
                {
                    IsUpsellOfferExist = false;
                    UpsellOffersList upsellOffersList = new UpsellOffersList(Utilities.ExecutionContext);
                    List<UpsellOfferProductsDTO> suggestiveSellOfferProductsList = upsellOffersList.GetSuggestiveSellOfferProductsDTOList(Convert.ToInt32(productRow["product_id"]));
                    if(suggestiveSellOfferProductsList != null && suggestiveSellOfferProductsList.Count > 0)
                    {
                        IsSuggestiveSellOfferExist = true;
                        this.txtOfferMessage.Text = "Suggestive Sell Offer : "+ suggestiveSellOfferProductsList[0].OfferMessage;
                        foreach (UpsellOfferProductsDTO d in suggestiveSellOfferProductsList)
                        {
                            Button btnOfferProduct = new Button();
                            //btnOfferProduct.Size = btnSample.Size;
                            btnOfferProduct.Size = btnSize;
                            btnOfferProduct.Font = btnSample.Font;

                            btnOfferProduct.BackColor = Color.Transparent;
                            btnOfferProduct.BackgroundImage = btnSample.BackgroundImage;
                            btnOfferProduct.BackgroundImageLayout = ImageLayout.Zoom;
                            btnOfferProduct.FlatStyle = btnSample.FlatStyle;
                            btnOfferProduct.ForeColor = btnSample.ForeColor;
                            btnOfferProduct.FlatAppearance.BorderColor = btnSample.FlatAppearance.BorderColor;
                            btnOfferProduct.FlatAppearance.BorderSize = btnSample.FlatAppearance.BorderSize;
                            btnOfferProduct.FlatAppearance.CheckedBackColor = btnSample.FlatAppearance.CheckedBackColor;
                            btnOfferProduct.FlatAppearance.MouseDownBackColor = btnSample.FlatAppearance.MouseDownBackColor;
                            btnOfferProduct.FlatAppearance.MouseOverBackColor = btnSample.FlatAppearance.MouseOverBackColor;
                            // btnOfferProduct.Text = d.ProductName + Environment.NewLine + Utilities.ParafaitEnv.CURRENCY_SYMBOL + " " + d.Price.ToString(Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                            btnOfferProduct.Text = d.ProductName + Environment.NewLine + d.Price.ToString(Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);

                            OffsellData lclUpsellData = new OffsellData();
                            lclUpsellData.ProductId = d.ProductId;
                            lclUpsellData.productName = d.ProductName;
                            lclUpsellData.Price = d.Price;
                            lclUpsellData.OfferMessage = d.OfferMessage;
                            lclUpsellData.Description = d.Description;
                            lclUpsellData.IsSelected = false;
                            btnOfferProduct.Tag = lclUpsellData;

                            btnOfferProduct.Click += btnSuggestiveSellProduct_Click;
                            flpUpsellOffersList.Controls.Add(btnOfferProduct);
                        }                       
                    }
                    else
                    {
                        IsSuggestiveSellOfferExist = false;
                    }
                }

              if(IsSuggestiveSellOfferExist)
                {
                    this.Text = "Suggestive Sell Options"; 
                    
                }
                btnYesPlease.Enabled = false;

            }
            catch(Exception ex)
            {
                log.Error("Error occured while executing PopulateOfferProducts()" + ex.Message);
                IsUpsellOfferExist = false;
                IsSuggestiveSellOfferExist = false;
                UpsellProductId= -1;
                suggestiveSellProductIds = new List<int>();
            }
        }

        void btnOfferProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            OffsellData offerProductData = (sender as Button).Tag as OffsellData;

            if (offerProductData != null)
            {
                bool found = false;
                foreach (Control c in flpUpsellOffersList.Controls)
                {
                    OffsellData obj = c.Tag as OffsellData;
                    if (obj != null && obj.Equals(offerProductData))
                    {
                        if (obj.IsSelected)
                        {
                            //De-select button
                            c.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.ComboProduct;
                            txtOfferMessage.Text = "Special Offer :";
                            btnYesPlease.Enabled = false;
                            c.Enabled = true;
                            obj.IsSelected = false;
                            found = true;
                            break;
                        }
                        else
                        {
                            //Select the button
                            c.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.ManualProduct;
                            txtOfferMessage.Text = "Special Offer : " + obj.OfferMessage;
                            btnYesPlease.Enabled = true;
                            c.Enabled = true;
                            obj.IsSelected = true;
                        }
                    }
                    else
                    {
                        c.Enabled = false;
                    }
                }

                if (found)
                {
                    EnableDisableParentButtons(true);
                }
            }
            log.LogMethodExit();
        }

        void btnSuggestiveSellProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            OffsellData offerProductData = (sender as Button).Tag as OffsellData;
            int selectProductCount = 0;
            if (offerProductData != null)
            {
                 foreach (Control c in flpUpsellOffersList.Controls)
                {
                    OffsellData obj = c.Tag as OffsellData;
                    if (obj != null)
                    {
                        if (obj.Equals(offerProductData))
                        {
                            if (offerProductData.IsSelected)
                            {
                                //De-select button
                                c.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.ComboProduct;
                                obj.IsSelected = false;
                            }
                            else
                            {
                                //Select the button
                                c.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.ManualProduct;
                                btnYesPlease.Enabled = true;
                                obj.IsSelected = true;
                            }
                        }
                        if (obj.IsSelected)
                        {
                            selectProductCount += 1;
                        }
                    }
                }
                if (selectProductCount == 0)
                    btnYesPlease.Enabled = false;
            }
            log.LogMethodExit();
           
        }

        //Added for enable or disable Product button on 13-Jan-2017//
        void EnableDisableParentButtons(bool enable)
        {
            log.LogMethodEntry(enable);
            if (enable)
                foreach (Control c in flpUpsellOffersList.Controls)
                {
                    c.Enabled = true;
                }
            else
                foreach (Control c in flpUpsellOffersList.Controls)
                {
                    c.Enabled = false;
                }
            log.LogMethodExit();
        }

        private void btnNoThanks_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
           // UpsellProductId= -1;
            this.Close();
            log.LogMethodExit();
        }

        private void btnYesPlease_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (IsSuggestiveSellOfferExist)
            {
                foreach (Control c in flpUpsellOffersList.Controls)
                {
                    OffsellData offsellData = c.Tag as OffsellData;
                    if (offsellData != null && offsellData.IsSelected)
                    {
                        suggestiveSellProductIds.Add(offsellData.ProductId);
                    }
                }
                this.Close();
            }
            if (IsUpsellOfferExist)
            {
                UpsellProductId = -1;
                foreach (Control c in flpUpsellOffersList.Controls)
                {
                    OffsellData offsellData = c.Tag as OffsellData;
                    if (offsellData != null && offsellData.IsSelected)
                    {
                        UpsellProductId = offsellData.ProductId;
                        break;
                    }
                }
                Transaction newTrx = new Transaction(Utilities);
                DataRow product = newTrx.getProductDetails(UpsellProductId);
                newTrx = null;
                PopulateOfferProducts(product, true);
                if (IsSuggestiveSellOfferExist)
                {
                    //MessageBox.Show("Please take moment to see the recommneded products for " + product["product_name"].ToString());       
                    //textBoxMessageLine.Invoke(new Action(() => textBoxMessageLine.BackColor = Color.Yellow));
                    //textBoxMessageLine.Invoke(new Action(() => textBoxMessageLine.ForeColor = Color.Black));
                    //textBoxMessageLine.Invoke(new Action(() => textBoxMessageLine.Text = Utilities.MessageUtils.getMessage(1427)));
                    textBoxMessageLine.BackColor = Color.Yellow;
                    textBoxMessageLine.ForeColor = Color.Black;
                    textBoxMessageLine.Text = Utilities.MessageUtils.getMessage(1427);

                }
                else
                {
                    IsUpsellOfferExist = true;
                    this.Close();
                }
                   
            }
            log.LogMethodExit();
            
           
        }

        private void txtOfferMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            e.Handled = true;
            log.LogMethodExit();
        }
        //End: Added for enable or disable Product button on 13-Jan-2017//
    }
}
