/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - TimeOut.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
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
            timeOutForm = new frmTimeOut();
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
