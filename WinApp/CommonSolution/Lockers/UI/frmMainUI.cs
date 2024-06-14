using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    public partial class frmMainUI : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;
        string connstring = "";
   
        public frmMainUI()
        {
            log.LogMethodEntry();
            InitializeComponent();
            try
            {
                connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
                utilities = new  Utilities(connstring);
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
            log.LogMethodExit();
        }

        /// <summary>
        /// button1_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void button1_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                //frmLockerManagementUI lockerSetupUI = new frmLockerManagementUI(utilities);
                //lockerSetupUI.Show();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnZonePicker_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnZonePicker_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                frmZonePicker frmzonePicker = new frmZonePicker(utilities,"FREE");
                frmzonePicker.Show();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }
    }
}
