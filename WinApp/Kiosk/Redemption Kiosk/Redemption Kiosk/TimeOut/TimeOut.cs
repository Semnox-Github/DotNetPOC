/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Timeout cs
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
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Redemption_Kiosk
{
    public static class TimeOut
    {
        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static frmRedemptionKioskTimeOut timeOutForm;
        public static bool AbortTimeOut(Form owner)
        {
            log.LogMethodEntry(owner);
            timeOutForm = new frmRedemptionKioskTimeOut();
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
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
