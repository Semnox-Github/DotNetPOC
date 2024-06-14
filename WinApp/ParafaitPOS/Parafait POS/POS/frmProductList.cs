/********************************************************************************************
 * Project Name - frmProductList
 * Description  - Product List form
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
 *2.70         26-Mar-2019     Guru S A       Booking phase 2 enhancement changes  
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.Transaction;

namespace Parafait_POS
{
    public partial class frmProductList : Form
    {
        object categoryId;
        int selectedQuantity;
        int QuantityRequired;
        int ComboProductId;
        int bookingId;
        int minQuantity; //Added on 10-Feb-2016 for checking Minimum Quantity set for a product
        bool editedBooking = false;
        public List<KeyValuePair<int, int>> SelectedProductList = new List<KeyValuePair<int, int>>();
        //Added to maintain the list of selected category products and send it to the calling form
        ReservationDTO.SelectedCategoryProducts selectedProducts;
        public List<ReservationDTO.SelectedCategoryProducts> lstSelectedProductList;
        public List<ReservationDTO.SelectedCategoryProducts> selectedCategoryListProducts;
        public List<ReservationDTO.SelectedCategoryProducts> selectedList;

        DataTable dt;

        public frmProductList(object CategoryName, object CategoryIdpar, int Quantity)
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            categoryId = CategoryIdpar;
            QuantityRequired = Quantity;
            lblText.Text = POSStatic.MessageUtils.getMessage("Select &1 &2 products", Quantity, CategoryName);

        }

        //Added constructor to get Combo Product Id from createProduct
        public frmProductList(object CategoryName, object CategoryIdpar, int Quantity, int minimumQuantity)
            : this(CategoryName, CategoryIdpar, Quantity)
        {
            minQuantity = minimumQuantity;
        }

        public frmProductList(object CategoryName, object CategoryIdpar, int Quantity, int comboId, int id, List<ReservationDTO.SelectedCategoryProducts> CategoryProuctFinalSelectedList, bool isEditedBooking)
            : this(CategoryName, CategoryIdpar, Quantity)
        {
            ComboProductId = comboId;
            bookingId = id;
            selectedCategoryListProducts =  CategoryProuctFinalSelectedList;
            editedBooking = isEditedBooking;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            lstSelectedProductList = new List<ReservationDTO.SelectedCategoryProducts>();
            selectedList = new List<ReservationDTO.SelectedCategoryProducts>();
            selectedList.Clear();
            if (QuantityRequired < selectedQuantity)
            {
                POSUtils.ParafaitMessageBox("Select at most " + QuantityRequired.ToString() + " products");
                return;
            }
            else if (minQuantity > 0 && (selectedQuantity < minQuantity))
            {
                POSUtils.ParafaitMessageBox("Select at least " + minQuantity.ToString() + " products");
                return;
            }
            else if (QuantityRequired > selectedQuantity)
            {
                if (POSUtils.ParafaitMessageBox("You have selected less than required quantity. Do you want to continue?", "Quantity Check", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }

            foreach (Control c in flpProducts.Controls)
            {

                selectedProducts = new ReservationDTO.SelectedCategoryProducts();
                if (c.GetType().ToString().EndsWith("NumericUpDown"))
                {
                    NumericUpDown nud = c as NumericUpDown;
                    if (nud.Value > 0)
                    {
                        SelectedProductList.Add(new KeyValuePair<int, int>((int)flpProducts.GetNextControl(nud, false).Tag, (int)nud.Value));
                        //Begin:Added to add productid, parent combo id and quantity to the list//
                        selectedProducts.parentComboProductId = ComboProductId;
                        selectedProducts.productId = (int)flpProducts.GetNextControl(nud, false).Tag;
                        selectedProducts.quantity = (int)nud.Value;

                        lstSelectedProductList.Add(selectedProducts);
                        //end: here
                    }
                    //Begin:Added to delete the prodcut within the list if quantity was reduced to "0" ie product was unselected//
                    if (selectedCategoryListProducts != null && selectedCategoryListProducts.Count >= 1)
                    {
                        for (int l = selectedCategoryListProducts.Count - 1; l >= 0; l--)
                        {
                            if (selectedCategoryListProducts[l].productId == (int)flpProducts.GetNextControl(nud, false).Tag && selectedCategoryListProducts[l].parentComboProductId == ComboProductId && (int)nud.Value <= 0)
                            {
                                selectedCategoryListProducts.RemoveAt(l);
                            }
                        }
                    }
                    //End
                }
            }
            selectedList.AddRange(lstSelectedProductList);
            //Begin: Delete the items from the list if the Original list had the product with same quantity
            if (selectedCategoryListProducts != null && selectedCategoryListProducts.Count >= 1)
            {
                for (int i = 0; i < selectedCategoryListProducts.Count; i++)
                {
                    for (int j = selectedList.Count - 1; j >= 0; j--)
                    {
                        if (selectedList[j].productId == selectedCategoryListProducts[i].productId && selectedList[j].parentComboProductId == selectedCategoryListProducts[i].parentComboProductId && selectedList[j].quantity == selectedCategoryListProducts[j].quantity)
                        {
                            selectedList.RemoveAt(j);
                        }
                    }
                }
            }
            //End//
            if (selectedCategoryListProducts != null)
            {
                selectedCategoryListProducts.AddRange(selectedList);//Return the List to the callinfg form
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void frmProductList_Load(object sender, EventArgs e)
        {
            dt = POSStatic.Utilities.executeDataTable(@"select product_name, product_id 
                                                                        from products 
                                                                        where active_flag = 'Y' 
                                                                        and categoryId in (select CategoryId from getCategoryList(@categoryId))
                                                                        order by sort_order, product_name", new SqlParameter("@categoryId", categoryId));
            //Begin: Added to get the category products  selected during the reservation
            DataTable selectedCategoryProducts = POSStatic.Utilities.executeDataTable(@"Select tt.product_id,tt.quantity from trx_lines tt,  
                                                                                        (select LineId,tl.TrxId from trx_lines tl, bookings b where tl.trxid  = b.TrxId
                                                                                        and b.BookingId = @bookingId
                                                                                        and tl.product_id = @ComboProductId)a
                                                                                        where a.LineId = tt.ParentLineId and tt.TrxId = a.TrxId 
                                                                                        and 
                                                                                        not exists(Select cp.Product_Id from ComboProduct cp where 
                                                                                        cp.ChildProductId = tt.product_id and cp.Product_Id = @ComboProductId)",
                                                                                        new SqlParameter("@bookingId", bookingId),
                                                                                        new SqlParameter("@ComboProductId", ComboProductId));


            //end
            flpProducts.Controls.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                Button btnProduct = new Button();
                btnProduct.Size = btnSample.Size;
                btnProduct.Text = dr["product_name"].ToString();
                btnProduct.Tag = dr["product_id"];
                btnProduct.Font = btnSample.Font;
                btnProduct.BackColor = btnSample.BackColor;
                btnProduct.FlatStyle = btnSample.FlatStyle;
                btnProduct.FlatAppearance.BorderColor = btnSample.FlatAppearance.BorderColor;
                btnProduct.FlatAppearance.MouseOverBackColor = btnSample.FlatAppearance.MouseOverBackColor;
                btnProduct.FlatAppearance.MouseDownBackColor = btnSample.FlatAppearance.MouseDownBackColor;
                flpProducts.Controls.Add(btnProduct);

                NumericUpDown nud = new NumericUpDown();
                nud.Font = nudSample.Font;
                nud.Size = nudSample.Size;
                nud.Margin = nudSample.Margin;
                nud.TextAlign = nudSample.TextAlign;
                nud.Minimum = 0;
                nud.Maximum = 500; //set maximum value as default maximum value is 100
                nud.ValueChanged += new EventHandler(nud_ValueChanged);
                flpProducts.Controls.Add(nud);

                btnProduct.Click += new EventHandler(btnProduct_Click);
                //Begin: Load the prodcuts selected before reserving//
                if (bookingId == -1 || editedBooking)
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (ReservationDTO.SelectedCategoryProducts products in selectedCategoryListProducts)
                        {
                            if (products.productId == Convert.ToInt32(dr["product_id"].ToString()))
                            {
                                nud.Value = products.quantity;
                            }
                        }
                    }
                }
                //end
                //Begin: Load the items after reservation, items queried based on the booking id//
                if (bookingId != -1)
                {
                    if (selectedCategoryProducts != null && selectedCategoryProducts.Rows.Count > 0)
                    {
                        foreach (DataRow drProducts in selectedCategoryProducts.Rows)
                        {
                            if (Convert.ToInt32(dr["product_id"].ToString()) == Convert.ToInt32(drProducts["product_id"].ToString()))
                            {
                                nud.Value = Convert.ToDecimal(drProducts["quantity"].ToString());
                            }
                        }
                    }
                }
                //End

            }
        }

        void nud_ValueChanged(object sender, EventArgs e)
        {
            selectedQuantity = 0;
            foreach (Control c in flpProducts.Controls)
            {
                if (c.GetType().ToString().EndsWith("NumericUpDown"))
                {
                    NumericUpDown nud = c as NumericUpDown;
                    selectedQuantity += (int)nud.Value;
                }
            }

            lblSelected.Text = selectedQuantity.ToString() + " " + POSStatic.MessageUtils.getMessage("selected");
        }

        void btnProduct_Click(object sender, EventArgs e)
        {
            Button btnProduct = sender as Button;
            NumericUpDown nud = flpProducts.GetNextControl(btnProduct, true) as NumericUpDown;
            if (nud.Value < nud.Maximum)
                nud.Value++;
        }
    }
}
