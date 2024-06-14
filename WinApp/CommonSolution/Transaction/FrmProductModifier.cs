/********************************************************************************************
 * Class Name -  Transaction                                                                         
 * Description - frmProductModifier. 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 ********************************************************************************************************************
 *2.70.2        12-Aug-2019          Deeksha        Modified logger methods.
 *2.130.3       01-Feb-2022          Prashanth      Modified: modified LoadProductDetails() 
 *                                                  for UI enhancement, added changeColor()
 *2.130.11      17-Nov-2022          Mathew N       Show image for Modifier products. 
 *                                                  Filter out inactive modifiers under modifier set
 *******************************************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Transaction
{
    public partial class FrmProductModifier : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Utilities _utilities;
        string _productName;
        internal bool ModifierExists = false;
        internal bool AutoShowInPOS = false;
        int minQuantity;
        int maxQuantity;
        string modifierSetName;
        bool ShowFormAuto = true;
        int count;
        int productId;
        ModifierSetBL modifierSetBL;

        public class TransactionModifier
        {
            public PurchasedProducts purchasedProducts;
            public List<PurchasedModifierSet> ModifierSetDTO = new List<PurchasedModifierSet>();
        }

        PurchasedProducts purchasedProducts = new PurchasedProducts();
        public TransactionModifier transactionModifier = new TransactionModifier();
        PurchasedProducts selectedPurchasedProduct = new PurchasedProducts();
        PurchasedModifierSet selectedModifierSetDTO = new PurchasedModifierSet();

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productName"></param>
        /// <param name="inUtilities"></param>
        /// <param name="purchasedProducts"></param>
        /// <param name="ShowFormAuto"></param>
        public FrmProductModifier(int productId, string productName, Utilities inUtilities = null, PurchasedProducts purchasedProducts = null, bool ShowFormAuto = true)
        {
            log.LogMethodEntry(productId, productName, inUtilities);

            _utilities = inUtilities;
            _productName = productName;
            this.productId = productId;
            this.purchasedProducts = purchasedProducts;
            this.ShowFormAuto = ShowFormAuto;
            InitializeComponent();
            TreeNode node = new TreeNode(productName);
            node.Tag = this.productId;
            tvProductList.Nodes.Add(node);

            transactionModifier.purchasedProducts = purchasedProducts;

            this.Load += FrmProductModifiers_Load;

            flpModifierSets.Controls.Clear();
            LoadModifierSet(this.productId, false);

            log.LogMethodExit();
        }

        /// <summary>
        /// Product Modifier Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmProductModifiers_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (purchasedProducts != null)
            {
                transactionModifier.purchasedProducts = purchasedProducts;
                transactionModifier.ModifierSetDTO = purchasedProducts.PurchasedModifierSetDTOList;
            }
            else
            {
                transactionModifier.purchasedProducts = BuildPurchasedProduct(productId);
            }

            LoadModifierSet(this.productId, true);

            if(flpModifierSets.Controls.Count > 0)
            {
                BtnModifierSet_Click(flpModifierSets.Controls[0], null);

                LoadSelectedProducts(flpModifierSets.Controls[0]);
                CreateProductTreeView();
            }
            
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Loads the Modifier Sets for the product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="LoadToList"></param>
        private void LoadModifierSet(int productId, bool LoadToList)
        {
            log.LogMethodEntry(productId, LoadToList);
            ModifierExists = false;

            ProductModifiersList productModifiersList = new ProductModifiersList(_utilities.ExecutionContext);
            List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.PRODUCT_ID, productId.ToString()));
            List<ProductModifiersDTO> productModifiersDtoList = productModifiersList.GetAllProductModifiersList(searchParameters);

            if (productModifiersDtoList != null)
            {
                foreach (ProductModifiersDTO productModifiersDTO in productModifiersDtoList)
                {
                    modifierSetBL = new ModifierSetBL(productModifiersDTO.ModifierSetId, _utilities.ExecutionContext,false, false);

                    if (modifierSetBL.GetModifierSetDTO != null)
                    {
                        if (productModifiersDTO.AutoShowinPOS == "Y" || ShowFormAuto == false)
                        {
                            modifierSetBL.PurchasedModifierSet(modifierSetBL.GetModifierSetDTO);
                            PurchasedModifierSet purchasedModifierSet = modifierSetBL.GetPurchasedModifierSet;
                            
                            Button btnModifierSet = new Button();
                            btnModifierSet.Size = sampleModifierSet.Size;
                            btnModifierSet.BackColor = sampleModifierSet.BackColor;
                            btnModifierSet.Font = sampleModifierSet.Font;
                            btnModifierSet.BackgroundImage = sampleModifierSet.BackgroundImage;
                            btnModifierSet.BackgroundImageLayout = sampleModifierSet.BackgroundImageLayout;
                            btnModifierSet.FlatStyle = sampleModifierSet.FlatStyle;
                            btnModifierSet.FlatAppearance.BorderColor = sampleModifierSet.FlatAppearance.BorderColor;
                            btnModifierSet.FlatAppearance.BorderSize = sampleModifierSet.FlatAppearance.BorderSize;
                            btnModifierSet.FlatAppearance.CheckedBackColor = sampleModifierSet.FlatAppearance.CheckedBackColor;
                            btnModifierSet.FlatAppearance.MouseDownBackColor = sampleModifierSet.FlatAppearance.MouseDownBackColor;
                            btnModifierSet.FlatAppearance.MouseOverBackColor = sampleModifierSet.FlatAppearance.MouseOverBackColor;

                            btnModifierSet.Text = modifierSetBL.GetModifierSetDTO.SetName.ToString();
                            btnModifierSet.Tag = purchasedModifierSet;
                            btnModifierSet.Click += BtnModifierSet_Click;

                            if (LoadToList)
                            {
                                flpModifierSets.Controls.Add(btnModifierSet);
                            }
                            AutoShowInPOS = true;
                        }
                    }
                }
                ModifierExists = true;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the products for each of the modifier Set
        /// </summary>
        /// <param name="modifierSetDTO"></param>
        private void LoadProductDetails(PurchasedModifierSet modifierSetDTO , double noncardButtonSize)
        {
            log.LogMethodEntry(modifierSetDTO, noncardButtonSize);
            if (modifierSetDTO != null)
            {
                ModifierSetDetailsList modifierSetDetailsList = new ModifierSetDetailsList(_utilities.ExecutionContext);
                List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>> searchParametersModifierSetDetails = new List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>>();
                searchParametersModifierSetDetails.Add(new KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>(ModifierSetDetailsDTO.SearchByParameters.MODIFIER_SET_ID, modifierSetDTO.ModifierSetId.ToString()));
                searchParametersModifierSetDetails.Add(new KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>(ModifierSetDetailsDTO.SearchByParameters.ISACTIVE, "1"));
                List<ModifierSetDetailsDTO> modifierSetDetailsDTOList = modifierSetDetailsList.GetModifierSetDetailList(searchParametersModifierSetDetails);

                if (modifierSetDetailsDTOList != null)
                {
                    if (modifierSetDetailsDTOList.Count > 0)
                    {
                        foreach (ModifierSetDetailsDTO modifierSetDetail in modifierSetDetailsDTOList)
                        {
                            Products products = new Products(modifierSetDetail.ModifierProductId);
                            if (products.GetProductsDTO != null && products.GetProductsDTO.ActiveFlag)
                            {
                                if (modifierSetDetail.Price >= 0)
                                {
                                    products.GetProductsDTO.Price = Convert.ToDecimal(modifierSetDetail.Price);
                                }
                                Panel panel = new Panel();

                                Color panelBackColor;
                                if (string.IsNullOrEmpty(products.GetProductsDTO.ButtonColor))
                                {
                                    panelBackColor = panelModifierProduct.BackColor;
                                }
                                else
                                {
                                    String backColor = products.GetProductsDTO.ButtonColor;
                                    string[] RGB = backColor.Split(',');
                                    panelBackColor = new Color();
                                    try
                                    {
                                        panelBackColor = Color.FromArgb(Convert.ToInt32(RGB[0]), Convert.ToInt32(RGB[1]), Convert.ToInt32(RGB[2]));
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message);
                                        panelBackColor = panelModifierProduct.BackColor;
                                    }
                                }

                                Size s = new Size((int)(panelModifierProduct.Width * noncardButtonSize), (int)(panelModifierProduct.Height * noncardButtonSize));
                                panel.Size = s;
                                panel.TabIndex = panelModifierProduct.TabIndex;
                                panel.Tag = products.GetProductsDTO;
                                if (panelBackColor.Equals(panelModifierProduct.BackColor))
                                {
                                    panel.BackgroundImage = panelModifierProduct.BackgroundImage;
                                }
                                else
                                {
                                    panel.BackgroundImage = ChangeColor((Bitmap)panelModifierProduct.BackgroundImage, panelBackColor);
                                }
                                if (!string.IsNullOrWhiteSpace(products.GetProductsDTO.ImageFileName))
                                {
                                    try
                                    {
                                        object o = _utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                                           new SqlParameter("@FileName", _utilities.getParafaitDefaults("IMAGE_DIRECTORY") + "\\" + products.GetProductsDTO.ImageFileName));
                                        if (o != null && o != DBNull.Value)
                                            panel.BackgroundImage = _utilities.ConvertToImage(o);
                                    }
                                    // catch { }                        
                                    catch (Exception ex)
                                    {
                                        log.Fatal("Unable to fetch the image for modifier " + ex.Message);//Added for logger function on 29-Feb-2016
                                    }
                                }
                                panel.BackgroundImageLayout = panelModifierProduct.BackgroundImageLayout;
                                panel.Click += PanelModifierProduct_Click;

                                Color labelForeColor;
                                if (string.IsNullOrWhiteSpace(products.GetProductsDTO.TextColor))
                                {
                                    labelForeColor = lblModifierProduct.ForeColor;
                                }
                                else
                                {
                                    String foreColor = products.GetProductsDTO.TextColor;
                                    string[] RGBTextColor = foreColor.Split(',');
                                    labelForeColor = new Color();
                                    try
                                    {
                                        labelForeColor = Color.FromArgb(Convert.ToInt32(RGBTextColor[0]), Convert.ToInt32(RGBTextColor[1]), Convert.ToInt32(RGBTextColor[2]));
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message);
                                        labelForeColor = lblModifierProduct.ForeColor;
                                    }
                                }

                                Label label = new Label();
                                label.Text = products.GetProductsDTO.ProductName;
                                label.Size = new System.Drawing.Size(panel.Size.Width, (panel.Size.Height - btnModifierProductCancel.Size.Height - 3));
                                Font labelFont;
                                if (string.IsNullOrEmpty(products.GetProductsDTO.Font))
                                {
                                    labelFont = lblModifierProduct.Font;
                                }
                                else
                                {
                                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                                    try
                                    {
                                        labelFont = (Font)converter.ConvertFromString(products.GetProductsDTO.Font);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message);
                                        labelFont = lblModifierProduct.Font;
                                    }
                                }
                                label.Font = labelFont;
                                label.BackColor = lblModifierProduct.BackColor;
                                label.FlatStyle = lblModifierProduct.FlatStyle;
                                label.TextAlign = lblModifierProduct.TextAlign;                                
                                label.Tag = panel;
                                label.ForeColor = labelForeColor;
                                label.Click += LblModifierProduct_Click;

                                Button btnProductCancel = new Button();
                                btnProductCancel.Size = btnModifierProductCancel.Size;
                                btnProductCancel.BackgroundImage = btnModifierProductCancel.BackgroundImage;
                                btnProductCancel.BackColor = btnModifierProductCancel.BackColor;
                                btnProductCancel.BackgroundImageLayout = btnModifierProductCancel.BackgroundImageLayout;
                                btnProductCancel.FlatStyle = btnModifierProductCancel.FlatStyle;
                                btnProductCancel.FlatAppearance.MouseDownBackColor = btnModifierProductCancel.FlatAppearance.MouseDownBackColor;
                                btnProductCancel.FlatAppearance.MouseOverBackColor = btnModifierProductCancel.FlatAppearance.MouseOverBackColor;
                                btnProductCancel.Anchor = btnModifierProductCancel.Anchor;
                                btnProductCancel.FlatAppearance.BorderSize = btnModifierProductCancel.FlatAppearance.BorderSize;
                                btnProductCancel.Location = new Point(panel.Width - btnProductCancel.Width - 3, 0);
                                btnProductCancel.Visible = false;
                                btnProductCancel.Tag = panel;
                                btnProductCancel.Click += BtnProductCancel_Click;
                                label.Location = new Point((panel.Size.Width - label.Size.Width) / 2, btnProductCancel.Location.Y + btnProductCancel.Size.Height);
                                label.AutoEllipsis = true;
                                panel.Controls.Add(label);
                                panel.Controls.Add(btnProductCancel);
                                panel.Controls.SetChildIndex(label, 1);
                                panel.Controls.SetChildIndex(btnProductCancel, 0);

                                //Modified 02/2019 for BearCat - 86-68
                                //Commenting out the code to show remaining quantity on product screen. Can be added back if needed.
                                //ProductsAvailabilityBL availableProd = new ProductsAvailabilityBL(_utilities.ExecutionContext, products.GetProductsDTO.ProductId);
                                //if (availableProd.GetProductsAvailabilityDTO().IsAvailable == "N" &&  availableProd.GetProductsAvailabilityDTO().UnavailableTill > _utilities.getServerTime())
                                //{
                                //    Label avlLabel = new Label();
                                //    Size sAvl = new Size((int)((panelModifierProduct.Width * noncardButtonSize) / 4), (int)((panelModifierProduct.Height * noncardButtonSize) / 4));
                                //    avlLabel.Size = sAvl;
                                //    avlLabel.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.yellow_30x30;
                                //    avlLabel.Text = availableProd.GetProductsAvailabilityDTO().AvailableQty.ToString();
                                //    avlLabel.Font = new Font(lblModifierProduct.Font, FontStyle.Bold);
                                //    avlLabel.ForeColor = Color.Red;
                                //    avlLabel.FlatStyle = lblModifierProduct.FlatStyle;
                                //    avlLabel.TextAlign = ContentAlignment.MiddleCenter;
                                //    avlLabel.Location = new Point((panel.Size.Width - avlLabel.Size.Width - 3), (panel.Size.Height - avlLabel.Size.Height - 3));
                                //    avlLabel.Tag = panel;
                                //    avlLabel.Click += LblModifierProduct_Click;
                                //    panel.Controls.Add(avlLabel);
                                //}


                                flpModifierList.Controls.Add(panel);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private Bitmap ChangeColor(Bitmap scrBitmap, Color newColor)
        {
            log.LogMethodEntry(newColor);
            Color actulaColor;
            //make an empty bitmap the same size as scrBitmap
            Bitmap newBitmap = new Bitmap(scrBitmap.Width, scrBitmap.Height);
            for (int i = 0; i < scrBitmap.Width; i++)
            {
                for (int j = 0; j < scrBitmap.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actulaColor = scrBitmap.GetPixel(i, j);
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actulaColor.A > 150)
                        newBitmap.SetPixel(i, j, newColor);
                    else
                        newBitmap.SetPixel(i, j, actulaColor);
                }
            }
            log.LogMethodExit(newBitmap);
            return newBitmap;
        }

        /// <summary>
        /// Modifier Set is clicked, 
        /// invokes to load the corresponding products
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnModifierSet_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            foreach (Control c in flpModifierSets.Controls)
            {
                c.BackgroundImage = sampleModifierSet.BackgroundImage;
            }

            double noncardButtonSize = 1;

            try
            {
                noncardButtonSize = Convert.ToDouble(_utilities.getParafaitDefaults("MODIFIER_BUTTON_SIZE")) / 100.0;
            }
            catch (Exception ex)
            {
                log.Error("Cannot get defaults of NON-CARD_PRODUCT_BUTTON_SIZE", ex);
                log.LogVariableState("noncardButtonSize", noncardButtonSize);
            }

            selectedModifierSetDTO = (sender as Button).Tag as PurchasedModifierSet;

            PurchasedModifierSet purchasedModifierSet = transactionModifier.ModifierSetDTO.Find(x => x.ModifierSetId == selectedModifierSetDTO.ModifierSetId);
            if (transactionModifier.ModifierSetDTO == null)
                transactionModifier.ModifierSetDTO.Add(selectedModifierSetDTO);
            else if (purchasedModifierSet == null)
                transactionModifier.ModifierSetDTO.Add(selectedModifierSetDTO);

            this.Text = "Modifier for " + _productName + ": " + (sender as Button).Text;
            lblStatus.Text = "Modifier for " + _productName + ": " + (sender as Button).Text;
            (sender as Button).BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.ManualProduct;

            //Begin: Added for ParentModifier Product Details on 11-Apr-2016//
            AddParentModifiers(selectedModifierSetDTO);
            //end: Added for ParentModifier Product Details on 11-Apr-2016//

            flpModifierList.Controls.Clear();

            LoadProductDetails(selectedModifierSetDTO, noncardButtonSize);
            LoadSelectedProducts(sender);

            modifierSetBL = new ModifierSetBL(Convert.ToInt32(selectedModifierSetDTO.ModifierSetId), _utilities.ExecutionContext, true, true);

            //Added on 24-may-2016 Disable The Parent Buttons
            EnableDisableParentButtons(true);

            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the products, if any product is selected
        /// </summary>
        /// <param name="sender"></param>
        private void LoadSelectedProducts(object sender)
        {
            log.LogMethodEntry(sender);
            foreach (PurchasedModifierSet tr in transactionModifier.ModifierSetDTO)
            {
                if (tr.ModifierSetId == ((sender as Button).Tag as ModifierSetDTO).ModifierSetId)
                {
                    if (tr.PurchasedProductsList != null)
                    {
                        foreach (PurchasedProducts pr in tr.PurchasedProductsList)
                        {
                            if (pr.IsSelected == true)
                            {
                                foreach (Control c in flpModifierList.Controls)
                                {
                                    ProductsDTO temp = c.Tag as ProductsDTO;
                                    if (pr.ProductId == temp.ProductId)
                                    {
                                        c.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.ManualProduct;
                                        foreach (Control ct in c.Controls)
                                        {
                                            if (ct is Button)
                                                ct.Visible = true;
                                        }
                                        if (!string.IsNullOrWhiteSpace(temp.ImageFileName))
                                        {
                                            try
                                            {
                                                object o = _utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                                                   new SqlParameter("@FileName", _utilities.getParafaitDefaults("IMAGE_DIRECTORY") + "\\" + temp.ImageFileName));
                                                if (o != null && o != DBNull.Value)
                                                    c.BackgroundImage = _utilities.ConvertToImage(o);
                                            }
                                            // catch { }                        
                                            catch (Exception ex)
                                            {
                                                log.Fatal("Unable to fetch the image for modifier " + ex.Message);//Added for logger function on 29-Feb-2016
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Adds the parent modifier details
        /// </summary>
        /// <param name="modifierSetDTO"></param>
        //Begin: Added for ParentModifier Product Details on 11-Apr-2016//
        private void AddParentModifiers(PurchasedModifierSet modifierSetDTO)
        {
            log.LogMethodEntry(modifierSetDTO);

            flpParentModifierList.Controls.Clear();
            ModifierSetBL modifierSetBL = new ModifierSetBL(Convert.ToInt32(modifierSetDTO.ModifierSetId), _utilities.ExecutionContext, true, true);
            if (modifierSetBL.GetModifierSetDTO != null)
            {
                if (modifierSetBL.GetModifierSetDTO.ParentModifierSetId != -1)
                {
                    modifierSetBL = new ModifierSetBL(modifierSetBL.GetModifierSetDTO.ParentModifierSetId, _utilities.ExecutionContext, true, true);
                    if (modifierSetBL.GetModifierSetDTO != null && modifierSetBL.GetModifierSetDTO.ModifierSetDetailsDTO != null)
                    {
                        foreach (ModifierSetDetailsDTO modifierSetDetailsDTO in modifierSetBL.GetModifierSetDTO.ModifierSetDetailsDTO)
                        {
                            Products products = new Products(modifierSetDetailsDTO.ModifierProductId);

                            Button btnParentModifier = new Button();
                            btnParentModifier.Size = sampleParentModifier.Size;
                            btnParentModifier.BackColor = sampleParentModifier.BackColor;
                            btnParentModifier.BackgroundImage = sampleParentModifier.BackgroundImage;
                            btnParentModifier.BackgroundImageLayout = sampleParentModifier.BackgroundImageLayout;
                            btnParentModifier.FlatStyle = sampleParentModifier.FlatStyle;
                            btnParentModifier.Font = sampleParentModifier.Font;
                            btnParentModifier.FlatAppearance.BorderColor = sampleParentModifier.FlatAppearance.BorderColor;
                            btnParentModifier.FlatAppearance.BorderSize = sampleParentModifier.FlatAppearance.BorderSize;
                            btnParentModifier.FlatAppearance.CheckedBackColor = sampleParentModifier.FlatAppearance.CheckedBackColor;
                            btnParentModifier.FlatAppearance.MouseDownBackColor = sampleParentModifier.FlatAppearance.MouseDownBackColor;
                            btnParentModifier.FlatAppearance.MouseOverBackColor = sampleParentModifier.FlatAppearance.MouseOverBackColor;
                            btnParentModifier.Text = products.GetProductsDTO.ProductName;
                            btnParentModifier.Tag = products.GetProductsDTO;
                            btnParentModifier.Click += BtnParentModifier_Click;
                            flpParentModifierList.Controls.Add(btnParentModifier);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Invokes when parent modifier click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnParentModifier_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (transactionModifier.purchasedProducts != null &&
                transactionModifier.ModifierSetDTO != null)
            {
                foreach (PurchasedModifierSet purchasedModifierSet in transactionModifier.ModifierSetDTO)
                {
                    if (purchasedModifierSet.PurchasedProductsList != null)
                    {
                        int index = purchasedModifierSet.PurchasedProductsList.FindIndex(x => x.ProductId == selectedPurchasedProduct.ProductId && x.IsSelected == true);
                        if (index != -1 && purchasedModifierSet.PurchasedProductsList[index] != null)
                        {
                            purchasedModifierSet.PurchasedProductsList[index].ParentModifierProduct = new PurchasedProducts();
                            purchasedModifierSet.PurchasedProductsList[index].ParentModifierProduct = BuildPurchasedProduct((((sender as Button).Tag as ProductsDTO)).ProductId);
                            EnableDisableParentButtons(true);
                            break;
                        }
                    }
                }
            }
            CreateProductTreeView();

            log.LogMethodExit();
        }

        //Added on 26-apr-2016, Enable or Disable  the parent buttons
        private void EnableDisableParentButtons(bool deselect)
        {
            log.LogMethodEntry(deselect);

            if (deselect)
                foreach (Control c in flpParentModifierList.Controls)
                {
                    c.Enabled = false;
                }
            else
                foreach (Control c in flpParentModifierList.Controls)
                {
                    c.Enabled = true;
                }

            log.LogMethodExit();
        }
        //End: Added for ParentModifier Product Details on 11-Apr-2016//

        /// <summary>
        /// Invoked when the product is deselected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnProductCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Panel panel = new Panel();
            if (sender is Label)
            {
                panel = ((sender as Label).Tag) as Panel;
            }
            else
                panel = ((sender as Button).Tag) as Panel;
            foreach (Control c in panel.Controls)
            {
                if (c is Button)
                    c.Visible = false;
            }
            //panel.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.ComboProduct;
            ProductsDTO productsDTO = panel.Tag as ProductsDTO;

            PurchasedModifierSet modifierSetDTO = transactionModifier.ModifierSetDTO.Find(x => x.ModifierSetId == selectedModifierSetDTO.ModifierSetId);
            if (modifierSetDTO.PurchasedProductsList != null)
            {
                foreach (PurchasedProducts product in modifierSetDTO.PurchasedProductsList)
                {
                    if (product.ProductId == productsDTO.ProductId)
                    {
                        CancelProduct(product);
                        product.IsSelected = false;
                        product.ParentModifierProduct = null;
                    }
                }
            }

            CreateProductTreeView();
            log.LogMethodExit();
        }

        private void CancelProduct(PurchasedProducts purchasedProduct)
        {
            log.LogMethodEntry(purchasedProduct);
            if(purchasedProduct.PurchasedModifierSetDTOList != null &&
                purchasedProduct.PurchasedModifierSetDTOList.Count > 0)
            {
                foreach (PurchasedModifierSet purchasedModifierSet in purchasedProduct.PurchasedModifierSetDTOList)
                {
                    if (purchasedModifierSet.PurchasedProductsList != null
                        && purchasedModifierSet.PurchasedProductsList.Count > 0)
                    {
                        foreach (PurchasedProducts product in purchasedModifierSet.PurchasedProductsList)
                        {
                            CancelProduct(product);
                        }
                    }
                }
            }
            else
            {
                purchasedProduct.IsSelected = false;
                purchasedProduct.ParentModifierProduct = null;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Adds the product when the product is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelModifierProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Panel panel = new Panel();
            if (sender is Label)
            {
                panel = ((sender as Label).Tag) as Panel;
            }
            else
                panel = sender as Panel;

            ProductsDTO productsDTO = panel.Tag as ProductsDTO;

            //Modified 02/2019 for BearCat - 86-68
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(_utilities.ExecutionContext, "ALLOW_PRODUCTS_TOBE_MARKED_UNAVAILABLE"))
            {
                lblStatus.Text = "";
                ProductsAvailabilityListBL unavailableProductsList = new ProductsAvailabilityListBL(_utilities.ExecutionContext);
                ProductsAvailabilityDTO unavailableProduct = unavailableProductsList.SearchUnavailableProductByProductId(productsDTO.ProductId);
                if (!unavailableProduct.IsAvailable && (unavailableProduct.AvailableQty - 1 < 0) && unavailableProduct.UnavailableTill > _utilities.getServerTime())
                {
                    lblStatus.Text = _utilities.MessageUtils.getMessage(2049, Decimal.ToInt32(unavailableProduct.AvailableQty), unavailableProduct.ProductName);
                    log.LogMethodExit();
                    return;
                }
            }

            PurchasedModifierSet modifierSet = transactionModifier.ModifierSetDTO.Where(x => x.ModifierSetId == Convert.ToInt32(selectedModifierSetDTO.ModifierSetId)).FirstOrDefault();
            if (modifierSet.PurchasedProductsList == null)
                modifierSet.PurchasedProductsList = new List<PurchasedProducts>();
            PurchasedProducts product = modifierSet.PurchasedProductsList.Find(x => x.ProductId == productsDTO.ProductId);

            PurchasedProducts purchasedProduct = null;

            if(product != null && product.IsSelected == true && product.PurchasedModifierSetDTOList.Count == 0)
            {
                EnableDisableParentButtons(true);
                log.LogMethodExit();
                return;
            }
            if (product == null)
            {
                purchasedProduct = BuildPurchasedProduct(productsDTO.ProductId);
                purchasedProduct.IsSelected = true;
                purchasedProduct.Price = Convert.ToDouble(productsDTO.Price);
                modifierSet.PurchasedProductsList.Add(purchasedProduct);
            }
            else
            {
                product.IsSelected = true;
                purchasedProduct = product;
            }
            
            foreach (Control c in panel.Controls)
            {
                if (c is Button)
                    c.Visible = true;
            }
            //panel.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.ManualProduct;
            
            LoadProductModifiers(purchasedProduct);

            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the purchased product details
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private PurchasedProducts BuildPurchasedProduct(int productId)
        {
            log.LogMethodEntry(productId);
            Products products = new Products(productId);
            PurchasedProducts purchasedProducts = products.GetPurchasedProducts();
            log.LogMethodExit(purchasedProducts);
            return purchasedProducts;
        }

        private void LblModifierProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PanelModifierProduct_Click(sender, e);
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks if the selected product has modifierset
        /// </summary>
        /// <param name="purchasedProducts"></param>
        private void LoadProductModifiers(PurchasedProducts purchasedProducts)
        {
            log.LogMethodEntry(purchasedProducts);
            EnableDisableParentButtons(false);
            LoadModifierSet(purchasedProducts.ProductId, false);
            selectedPurchasedProduct = purchasedProducts;

            if (ModifierExists)
            {
                using (FrmProductModifier f = new FrmProductModifier(purchasedProducts.ProductId, purchasedProducts.ProductName, _utilities, purchasedProducts))
                {
                    f.ShowDialog();
                }
            }

            CreateProductTreeView();
            if (ModifierExists)
                EnableDisableParentButtons(true);

            log.LogMethodExit();
        }

        /// <summary>
        /// Checks the product quantity for each modifierset
        /// </summary>
        /// <param name="modifierSetId"></param>
        /// <returns></returns>
        private bool CheckQuantity(int modifierSetId)
        {
            log.LogMethodEntry(modifierSetId);
            count = 0;

            PurchasedModifierSet selectedModifierSetDTO = transactionModifier.ModifierSetDTO.Where(x => x.ModifierSetId == Convert.ToInt32(modifierSetId)).FirstOrDefault();
            if (selectedModifierSetDTO != null)
            {
                minQuantity = selectedModifierSetDTO.MinQuantity;
                maxQuantity = selectedModifierSetDTO.MaxQuantity;
                modifierSetName = selectedModifierSetDTO.SetName;
                if (minQuantity > 0 || maxQuantity > 0)
                {
                    if (minQuantity > 0 && selectedModifierSetDTO.PurchasedProductsList == null)
                    {

                        System.Windows.Forms.MessageBox.Show(_utilities.MessageUtils.getMessage(1712, minQuantity, modifierSetName));

                        log.LogMethodExit(false);
                        return false;
                    }
                    //Added to check when the list is not null//
                    else if (selectedModifierSetDTO.PurchasedProductsList != null && selectedModifierSetDTO.PurchasedProductsList.Count > 0)
                    {
                        foreach (PurchasedProducts d in selectedModifierSetDTO.PurchasedProductsList)
                        {
                            if(d.IsSelected == true)
                                count++;
                        }
                        if (minQuantity > 0 && count < minQuantity)
                        {
                            System.Windows.Forms.MessageBox.Show(_utilities.MessageUtils.getMessage(1713, minQuantity, modifierSetName));

                            log.LogMethodExit(false);
                            return false;
                        }

                        else if (maxQuantity > 0 && count > maxQuantity)
                        {
                            System.Windows.Forms.MessageBox.Show(_utilities.MessageUtils.getMessage(1714, maxQuantity, modifierSetName));

                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Tree view of the selected products
        /// </summary>
        private void CreateProductTreeView()
        {
            log.LogMethodEntry();
            tvProductList.Nodes.Clear();

            if (transactionModifier.purchasedProducts != null &&
                transactionModifier.ModifierSetDTO != null)
            {
                TreeNode node = new TreeNode(transactionModifier.purchasedProducts.ProductName);
                node.Tag = transactionModifier.purchasedProducts.ProductId;
                tvProductList.Nodes.Add(node);

                foreach (PurchasedModifierSet purchasedModifierSet in transactionModifier.ModifierSetDTO)
                {
                    GetNodes(node, purchasedModifierSet);
                }
            }
            if (tvProductList.Nodes.Count > 0)
            {
                tvProductList.Nodes[0].NodeFont = new System.Drawing.Font(tvProductList.Font, FontStyle.Bold);
                tvProductList.Nodes[0].ExpandAll();
                tvProductList.Nodes[0].Text = tvProductList.Nodes[0].Text; // reassign to set proper width for text
            }
            log.LogMethodExit();
        }

        private TreeNode[] GetChildren(List<PurchasedProducts> productsDTOList)
        {
            log.LogMethodEntry(productsDTOList);
            TreeNode[] tnCollection = null;
            if (productsDTOList != null)
            {
                tnCollection = new TreeNode[productsDTOList.Count];
                for (int j = 0; j < productsDTOList.Count; j++)
                {
                    if (productsDTOList[j].IsSelected == true)
                    {
                        if (productsDTOList[j].ParentModifierProduct != null)
                            tnCollection[j] = new TreeNode(productsDTOList[j].ParentModifierProduct.ProductName + " - " + productsDTOList[j].ProductName);
                        else
                            tnCollection[j] = new TreeNode(productsDTOList[j].ProductName);
                        tnCollection[j].Tag = productsDTOList[j].PurchasedModifierSetDTOList;
                    }
                }
            }
            log.LogMethodExit(tnCollection);
            return tnCollection;
        }

        private TreeNode GetNodes(TreeNode rootNode, PurchasedModifierSet modifierSetDTO = null)
        {
            log.LogMethodEntry(rootNode, modifierSetDTO);
            TreeNode[] tn = GetChildren(modifierSetDTO == null ? null : modifierSetDTO.PurchasedProductsList);
            if (tn == null)
                return null;
            else
            {
                foreach (TreeNode tnode in tn)
                {
                    if (tnode != null)
                    {
                        TreeNode node = null;
                        foreach (PurchasedModifierSet purchasedModifierSet in tnode.Tag as List<PurchasedModifierSet>)
                        {
                            node = GetNodes(tnode, purchasedModifierSet);
                        }
                        if (node == null)
                            rootNode.Nodes.Add(tnode);
                        else
                            rootNode.Nodes.Add(node);
                    }

                }
                log.LogMethodExit(rootNode);
                return (rootNode);
            }
        }

        //Begin:Added the below code event to check if the modifier selected is within Min and Max Quantity on 28-Jan-2016//
        private void BtnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            foreach (PurchasedModifierSet modifiersDTO in transactionModifier.ModifierSetDTO)
            {
                if (modifiersDTO.PurchasedProductsList != null && modifiersDTO.PurchasedProductsList.Count > 0)
                {
                    foreach (PurchasedProducts productsDTO in modifiersDTO.PurchasedProductsList)
                    {
                        if (productsDTO.ParentModifierProduct != null)
                        {
                            Products products = new Products();
                            productsDTO.ParentModifierProduct.Price = products.UpdateParentModifierPrice(productsDTO, modifiersDTO);
                        }
                    }
                }
            }

            foreach (Control c in flpModifierSets.Controls)
            {
                if (CheckQuantity(Convert.ToInt32(((c.Tag) as ModifierSetDTO).ModifierSetId)))
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    c.Focus();
                    DialogResult = System.Windows.Forms.DialogResult.None;
                    break;
                }
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            foreach (PurchasedModifierSet tr in transactionModifier.ModifierSetDTO)
            {
                if(tr.PurchasedProductsList != null && tr.PurchasedProductsList.Count > 0)
                {
                    foreach(PurchasedProducts purchasedProduct in tr.PurchasedProductsList)
                    {
                        if (purchasedProduct.IsSelected && purchasedProduct.TrxLineId <= -1)
                            purchasedProduct.IsSelected = false;
                    }
                }
            }
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            log.LogMethodExit();
        }
        //End Modification - 28-Jan-2016//
    }
}
