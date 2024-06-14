/********************************************************************************************
* Project Name - Parafait_Kiosk - TimeOut.cs
* Description  - TimeOut.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
********************************************************************************************/

using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public static class TimeOut
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static frmTimeOut timeOutForm;
        public static bool AbortTimeOut(Form owner)
        {
            log.LogMethodEntry(owner);
            using (timeOutForm = new frmTimeOut())
            {
                if (timeOutForm.ShowDialog(owner) == DialogResult.Abort)
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
        }

        public static void Abort()
        {
            log.LogMethodEntry();
            try
            {
                timeOutForm.CloseForm();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing Abort()" + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
