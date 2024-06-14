using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public partial class frmEntryMode : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool isManual;
        /// <summary>
        /// Is manual or not
        /// </summary>
        public bool IsManual { get { return isManual; } }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public frmEntryMode()
        {
            log.LogMethodEntry();
            InitializeComponent();
            isManual = false;
            this.TopMost = true;
            log.LogMethodExit(null);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit(null);
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(rbtnKeyed.Checked)
            {
                isManual = true;
            }            
            this.DialogResult = DialogResult.OK;
            Close();
            log.LogMethodExit(null);
        }
    }
}
