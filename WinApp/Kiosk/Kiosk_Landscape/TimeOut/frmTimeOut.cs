/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmTimeOut.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80      5-Sep-2019       Deeksha            Added logger methods.
********************************************************************************************/
using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmTimeOut : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        frmTimeOutCounter ft;
        public frmTimeOut()
        {
            log.LogMethodEntry();
            InitializeComponent();
            ft = new frmTimeOutCounter();
            log.LogMethodExit();
        }

        private void frmTimeOut_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ft.FormClosed += (s, ev) =>
                {
                    DialogResult = ft.DialogResult;
                    Close();
                };
            ft.TopMost = true;
            ft.Show();
            //MessageBox.Show(this.Owner.Name);
            log.LogMethodExit();
        }

        private void frmTimeOut_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            CloseForm();
            log.LogMethodExit();
        }

        public void CloseForm()
        {
            log.LogMethodEntry();
            ft.DialogResult = System.Windows.Forms.DialogResult.Abort;
            ft.Close();
            log.LogMethodExit();
        }
    }
}
