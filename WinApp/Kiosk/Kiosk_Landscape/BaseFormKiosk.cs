/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - BaseFormKiosk. 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        4-Sep-2019       Deeksha             Added logger methods.
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Parafait.KioskCore;
using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class BaseFormKiosk : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BaseFormKiosk()
        {
            log.LogMethodEntry();
            InitializeComponent(); 
            this.ShowInTaskbar = false;
            log.LogMethodExit();
        }

        public virtual void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Home button clicked");
            try
            {
                DirectCashAbortAction(kioskTransaction);
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                for (int i = Application.OpenForms.Count - 1; i > 0; i--)
                {
                    Application.OpenForms[i].Visible = false;
                    Application.OpenForms[i].Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error on Home button click: " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }

      
        public void displaybtnHome(bool switchValue)
        {
            log.LogMethodEntry(switchValue);
            this.btnHome.Visible= switchValue;
            log.LogMethodExit();
        }

    }
}
