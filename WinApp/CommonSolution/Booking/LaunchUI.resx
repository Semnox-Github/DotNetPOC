using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Booking
{
    /// <summary>
    /// Launch UI
    /// </summary>
    public partial class LaunchUI : Form
    {
        ParafaitUtils.Utilities utilities;
        string connstring = "";
        /// <summary>
        /// 
        /// </summary>
        public LaunchUI()
        {
            InitializeComponent();
            try
            {
                connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
                utilities = new ParafaitUtils.Utilities(connstring);
                utilities.ParafaitEnv.User_Id = 3;
                utilities.ParafaitEnv.LoginID = "semnox";
                utilities.ParafaitEnv.SiteId = -1;
                //utilities.ParafaitEnv.SiteId = 1008;
                //utilities.ParafaitEnv.IsCorporate = true;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ReservationDiscountsListUI reservationDiscountsListUI = new ReservationDiscountsListUI(utilities, 10);
                if (reservationDiscountsListUI.ShowDialog() == DialogResult.OK)
                {
                    reservationDiscountsListUI.Close();
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }
    }
}
