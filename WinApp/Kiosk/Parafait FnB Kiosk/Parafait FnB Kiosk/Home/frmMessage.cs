/********************************************************************************************
* Project Name - Parafait_Kiosk -frmHome.cs
* Description  - frmHome 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class frmMessage : BaseForm
    {
        public frmMessage(string Message, bool ResponseRequired = false)
        {
            log.LogMethodEntry(Message, ResponseRequired);
            InitializeComponent();
            if (ResponseRequired)
            {
                btnClose.Visible = false;
                btnYes.Visible = btnNo.Visible = true;
            }
            else
            {
                btnClose.Visible = true;
                btnYes.Visible = btnNo.Visible = false;
            }

            lblDisplayText1.Text = Message;

            this.StartPosition = FormStartPosition.CenterScreen;
            log.LogMethodExit();
        }
    }
}
