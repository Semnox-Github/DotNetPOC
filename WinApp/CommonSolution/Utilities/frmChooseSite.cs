using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait;
using Semnox.Core.Utilities;

namespace Semnox.Core.Utilities
{
    public partial class frmChooseSite : Form
    {
        public int SiteId = -1;
        public DBUtils Utilities;
        public frmChooseSite(DBUtils ParafaitUtilities)
        {
            InitializeComponent();
            Utilities = ParafaitUtilities;
        }

        private void frmChooseSite_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = Utilities.getCommand();
            //cmd.CommandText = "select site_id, site_name from site s where not exists (select 1 from company where master_site_id = s.site_id)";
            cmd.CommandText = "select site_id, site_name from site s order by site_name";
            DataTable DT = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(DT);

            cmbSites.DataSource = DT;
            cmbSites.DisplayMember = "site_name";
            cmbSites.ValueMember = "site_id";
            this.Activate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SiteId = Convert.ToInt32(cmbSites.SelectedValue);
            this.Close();
        }
    }
}
