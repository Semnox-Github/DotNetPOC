/********************************************************************************************
*Project Name - frmOrderPrint                                                                          
*Description  - 
*************
**Version Log
*************
*Version     Date                   Modified By                Remarks          
*********************************************************************************************
* 2.70.2       20-Aug-2019            Archana                    Form is moved to Inventory project
*                                                              Auto PO changes             
********************************************************************************************/
using System;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class frmOrderPrint : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string documentStatus;
        public frmOrderPrint()
        {
            log.LogMethodEntry();
            InitializeComponent();
            rbDraft.Checked = true;
            log.LogMethodExit();
        }

        private void cb_print_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (rbDraft.Checked)
                documentStatus = "D";
            else if (rbFinal.Checked)
                 documentStatus = "F";
            this.DialogResult = DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }
    }
}
