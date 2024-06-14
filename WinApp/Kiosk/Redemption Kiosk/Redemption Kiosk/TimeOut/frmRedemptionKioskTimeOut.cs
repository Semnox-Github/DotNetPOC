/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Time Out UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskTimeOut : Form
    {
        frmRedemptionKioskTimeOutCounter ft;
        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public frmRedemptionKioskTimeOut()
        {
            log.LogMethodEntry();
            InitializeComponent();
            ft = new frmRedemptionKioskTimeOutCounter();
            log.LogMethodExit();
        }

        private void FrmTimeOut_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ft.FormClosed += (s, ev) =>
                {
                    DialogResult = ft.DialogResult;
                    Close();
                };
            ft.TopMost = true;
            ft.Show();
            Common.utils.setLanguage(this);
            log.LogMethodExit(); 
        }

        private void FrmTimeOut_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender ,e);
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

        protected override CreateParams CreateParams
        {
            //this method is used to avoid the table layout flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }
    }
}
