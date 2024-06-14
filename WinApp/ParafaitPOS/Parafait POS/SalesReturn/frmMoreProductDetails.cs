using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Parafait_POS.SalesReturn
{
    public partial class frmMoreProductDetails : Form
    {
        long pID;
        string pName;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities = POSStatic.Utilities;
        MessageUtils MessageUtils = POSStatic.MessageUtils;

        public frmMoreProductDetails(long productID, string productName)
        {
            log.Debug("Starts-frmMoreProductDetails()");
            InitializeComponent();
            Utilities.setLanguage(this);

            pID = productID;
            pName = productName;
            log.Debug("Ends-frmMoreProductDetails()");
        }

        private void frmProductDetails_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-frmProductDetails_Load()");
            loadProductDetails();
            lblHeading.Text = Utilities.MessageUtils.getMessage("Details for product") + ": " + pName;
            log.Debug("Ends-frmProductDetails_Load()");
        }

        private void btnPLClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnPLClose_Click()");
            this.Close();
            log.Debug("Ends-btnPLClose_Click()");
        }

        private void loadProductDetails()
        {
            log.Debug("Starts-loadProductDetails()");
            try
            {
                SqlCommand cmd = Utilities.getCommand();
                DataTable dt = new DataTable();

                cmd.CommandText = @"select Name, Value 
                                    from (
                                            select 1 sort, 'Inventory Product Code' Name, inventoryproductcode Value
                                            from products
                                            where product_id = @product_id
                                            union all
                                            select 2, 'Sale Price', format(price, '" + Utilities.getParafaitDefaults("AMOUNT_FORMAT") + @"')
                                            from products
                                            where product_id = @product_id
                                            union all
                                            select 3, 'Cost', format(cost, '" + Utilities.getParafaitDefaults("AMOUNT_FORMAT") + @"')
                                            from product p
                                            where p.manualproductId = (select product_id from products where product_id = @product_id)
											union all
                                            select 4, tax_name + ' %', format(t.tax_percentage, '"+ Utilities.getParafaitDefaults("AMOUNT_FORMAT") + @"')
                                            from products p, tax t
                                            where product_id = @product_id
	                                            and p.tax_id = t.tax_id
                                            union all
                                            select 5, 'Tax Inclusive?', TaxInclusivePrice
                                            from products 
                                            where product_id = @product_id
                                            union all
                                            select 6, 'SKU: ' + segmentname, valuechar
                                            from product p, segmentdataview c
                                            where p.segmentcategoryid = c.SegmentCategoryId
	                                            and p.manualproductId = (select product_id from products where product_id = @product_id)
                                            union all
                                            select 7, 'Inventory quantity: ' + name, format(Quantity, '" + Utilities.getParafaitDefaults("AMOUNT_FORMAT") + @"')
                                            from Location l, Inventory i, product p
                                            where l.LocationId = i.LocationId
	                                            and l.LocationId in (select distinct inventorylocationid
						                                             from posmachines
						                                             union 
						                                             select locationid
						                                             from location
						                                             where IsStore = 'Y')
                                                and p.productid = i.productid
	                                            and p.manualproductId = (select product_id from products where product_id = @product_id)
                                            union all
                                            select top 1 8, 'Barcode ', stuff((isnull((SELECT top 1 case count(*) when 0 then '' else '|'+ isnull(BarCode, '') end BarCode
														FROM ProductBarcode 
														WHERE productid = p.ProductId and isactive = 'Y'
														group by BarCode
														FOR XML PATH('')), '') 
														
														)
														, 1, 1 , '')
                                            from product p
                                            where p.manualproductId = (select product_id from products where product_id = @product_id)
                                           )v
                                           order by sort";
                cmd.Parameters.AddWithValue("@product_id", pID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dgvProductDetails.DataSource = dt;
                Utilities.setLanguage(dgvProductDetails);
                cmd.Dispose();
            }
            catch
            {

            }
            log.Debug("Ends-loadProductDetails()");
        }
    }
}
