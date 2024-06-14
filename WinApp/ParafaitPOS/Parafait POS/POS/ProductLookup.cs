using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Parafait_POS
{
    public partial class ProductLookup : Form
    {
        public int SelectedProductId = -1;
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public ProductLookup(string pFilter)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-ProductLookup(" + pFilter + ")");//Modified for Adding logger feature on 08-Mar-2016
            POSStatic.Utilities.setLanguage();            
            InitializeComponent();

            txtSearchText.Text = pFilter;
            btnSearch_Click(null, null);
            if (dgvProductSearch.Rows.Count == 1)
            {
                SelectedProductId = Convert.ToInt32(dgvProductSearch["product_id", 0].Value);
            }

            log.Debug("Ends-ProductLookup(" + pFilter + ")");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void ProductLookup_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-ProductLookup_Load()");//Modified for Adding logger feature on 08-Mar-2016
            POSStatic.Utilities.setupDataGridProperties(ref dgvProductSearch);
            POSStatic.Utilities.setLanguage(this);
            log.Debug("Ends-ProductLookup_Load()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSearch_Click()");//Added for logger function on 08-Mar-2016
            if (txtSearchText.Text.Length < 3)
            {
                POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(5495));
                return;
            }
            SqlCommand cmd = POSStatic.Utilities.getCommand();
            //24-Mar-2016
            cmd.CommandText = @"select p.product_id, p.product_name, p.description, p.price, inventoryproductcode as inv_product_code, p.credits, p.courtesy, p.bonus, p.time, 
                                       p.taxinclusiveprice as tax_inclusive_price 
                               from products p inner join product_type pt on p.product_type_id = pt.product_type_id 
                                 Left join ProductsDisplayGroup pd on p.product_id = pd.ProductId 
                                 left join ProductDisplayGroupFormat pdgf on pd.DisplayGroupId = pdgf.Id 
                                    where pt.product_type != 'REFUND' 
                                    and p.active_flag = 'Y' 
                                    and (p.product_name like @filter 
                                        or p.description like @filter) 
                                    and not exists (select 1 
                                                        from POSProductExclusions e 
                                                        where e.POSMachineId = @PosMachineId 
                                                           and e.ProductDisplayGroupFormatId = pdgf.Id) 
                                    and not exists (select 1 
                                                      from UserRoleDisplayGroupExclusions urdge , 
                                                           users u
                                                     where urdge.ProductDisplayGroupId = pdgf.Id
                                                       and urdge.role_id = u.role_id
                                                       and u.loginId = @loginId )
                              union 
                              select p.product_id, p.product_name, p.description, p.price, inventoryproductcode as inv_product_code, p.credits, p.courtesy, p.bonus, p.time, 
                                 p.taxinclusiveprice as tax_inclusive_price  
                                  from products p inner join product rp on rp.manualproductId = p.product_id 
                                   left outer join (select * 
													                          from ( 
																                        select *, row_number() over(partition by productid order by productid) as num 
																                        from productbarcode 
                                                                                        where barcode like @barcode and isactive = 'Y')v 
                                                                              where num = 1) b on rp.productid = b.productid 
                                 Left join ProductsDisplayGroup pd  on p.product_id = pd.ProductId 
                                 left join ProductDisplayGroupFormat pdgf on pd.DisplayGroupId = pdgf.Id 
                                 where b.barcode like @barcode 
                                 and p.active_flag = 'Y' 
                                 and not exists (select 1 
                                                   from POSProductExclusions e 
                                                    where e.POSMachineId = @PosMachineId 
                                                    and e.ProductDisplayGroupFormatId = pdgf.Id) 
                                 and not exists (select 1 
                                                   from UserRoleDisplayGroupExclusions urdge , 
                                                        users u
                                                  where urdge.ProductDisplayGroupId = pdgf.Id
                                                    and urdge.role_id = u.role_id
                                                    and u.loginId = @loginId )
                                order by 2";
            //24-Mar-2016
            cmd.Parameters.AddWithValue("@filter", "%" + txtSearchText.Text + "%");
            cmd.Parameters.AddWithValue("@barcode", "%" + txtSearchText.Text + "%");
            cmd.Parameters.AddWithValue("@PosMachineId", POSStatic.Utilities.ParafaitEnv.POSMachineId);
            cmd.Parameters.AddWithValue("@loginId", POSStatic.Utilities.ParafaitEnv.LoginID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvProductSearch.DataSource = null;
            dgvProductSearch.DataSource = dt;
            dgvProductSearch.Columns["product_id"].Visible = false;
            POSStatic.Utilities.setupDataGridProperties(ref dgvProductSearch);
              
            //Added 7-oct-2016 for columns exchange
            dgvProductSearch.Columns["product_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductSearch.Columns["product_name"].Width = 240;

            dgvProductSearch.Columns["description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductSearch.Columns["description"].Width = 220;
            //end

            this.ActiveControl = txtSearchText;
            dgvProductSearch.Refresh();
            log.Debug("Ends-btnSearch_Click()");//Added for logger function on 08-Mar-2016
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClear_Click()");//Added for logger function on 08-Mar-2016
            txtSearchText.Text = "";
            this.ActiveControl = txtSearchText;
            log.Debug("Ends-btnClear_Click()");//Added for logger function on 08-Mar-2016
        }

        private void dgvProductSearch_DoubleClick(object sender, EventArgs e)
        {
            log.Debug("Starts-dgvProductSearch_DoubleClick()");//Added for logger function on 08-Mar-2016
            selectValue();
            log.Debug("Ends-dgvProductSearch_DoubleClick()");//Added for logger function on 08-Mar-2016
        }

        void selectValue()
        {
            log.Debug("Starts-selectValue()");//Added for logger function on 08-Mar-2016
            try
            {
                SelectedProductId = Convert.ToInt32(dgvProductSearch.SelectedRows[0].Cells["product_id"].Value);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }            
            catch
            {
                log.Fatal("Ends-selectValue() due to exception in dgvProductSearch.SelectedRows[0].Cells[product_id].Value ");//Added for logger function on 08-Mar-2016
            }
           
            log.Debug("Ends-selectValue()");//Added for logger function on 08-Mar-2016
        }

        private void dgvProductSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvProductSearch_CellContentClick()");//Added for logger function on 08-Mar-2016
            if (e.ColumnIndex >= 0 && dgvProductSearch.Columns[e.ColumnIndex].Name == "SelectRow")
                selectValue();

            log.Debug("Ends-dgvProductSearch_CellContentClick()");//Added for logger function on 08-Mar-2016
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSelect_Click()");//Added for logger function on 08-Mar-2016
            selectValue();
            log.Debug("Ends-btnSelect_Click()");//Added for logger function on 08-Mar-2016
        }

        

        private void txtSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            log.Debug("Starts-txtSearchText_KeyDown()");//Added for logger function on 08-Mar-2016
            if (e.KeyData== Keys.Down)
            {
                try
                {
                    dgvProductSearch.Rows[dgvProductSearch.SelectedRows[0].Index + 1].Selected = true;
                }               
                catch
                {
                    log.Fatal("Ends-txtSearchText_KeyDown() due to exception in Selected Row Index of dgvProductSearch ");//Added for logger function on 08-Mar-2016
                }        
            }
            else if (e.KeyData == Keys.Up)
            {
                try
                {
                    dgvProductSearch.Rows[dgvProductSearch.SelectedRows[0].Index - 1].Selected = true;
                }
                catch
                {
                    log.Fatal("Ends-txtSearchText_KeyDown() due to exception in Selected Row Index of dgvProductSearch ");//Added for logger function on 08-Mar-2016
                }
            }
            log.Debug("Ends-txtSearchText_KeyDown()");//Added for logger function on 08-Mar-2016
        }
        
    }
}
