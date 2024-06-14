using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Represents a locker tile
    /// </summary>
    public partial class LockerControl : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// default constructor
        /// </summary>
        public LockerControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            chkActive.Checked = false;
            chkDisable.Checked = false;
            log.LogMethodExit();
        }


        private void txtIdentifier_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (char.IsControl(e.KeyChar) == false && char.IsNumber(e.KeyChar) == false)
                e.Handled = true;
            log.LogMethodExit();
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (chkActive.Checked)
            {

                if (chkDisable.Checked)
                    BackColor = Color.OrangeRed;
                else
                    BackColor = System.Drawing.Color.ForestGreen;
                this.txtIdentifier.Enabled = txtLockerName.Enabled = chkDisable.Enabled = true;
            }
            else
            {
                BackColor = Color.Gray;
                this.txtIdentifier.Enabled = txtLockerName.Enabled = chkDisable.Enabled = false;
            }
            log.LogMethodExit();
        }

        private void chkDisable_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (chkActive.Checked == false)
            {
                BackColor = Color.Gray;
                return;
            }

            if (chkDisable.Checked)
            {
                BackColor = Color.OrangeRed;
            }
            else
            {
                BackColor = System.Drawing.Color.ForestGreen;
            }
            log.LogMethodExit();
        }
    }
}
