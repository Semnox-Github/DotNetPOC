/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - clsAlohaStatic.cs
 * 
 **************
 **Version Log
 ************** 
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.KioskCore.ExternalPOSInterface
{
    public static class clsAlohaStatic
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static int AlohaTerminalId;
        public static int AlohaEmpNumber;
        public static string AlohaEmpPassword;
        public static int AlohaJobCode;

        public static void Init()
        {
            log.LogMethodEntry();
            try
            {
                AlohaTerminalId = Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("ALOHA_TERM_ID"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting to INt for ALOHA_TERM_ID" + ex.Message);
            }
            try
            {
                AlohaJobCode = Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("ALOHA_JOB_CODE"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting to INt for ALOHA_JOB_CODE" + ex.Message);
            }
            try
            {
                AlohaEmpNumber = Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("ALOHA_USER_ID"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting to INt for ALOHA_USER_ID" + ex.Message);
            }
            try
            {
                AlohaEmpPassword = KioskStatic.Utilities.getParafaitDefaults("ALOHA_USER_PASSWORD");
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting to INt for ALOHA_USER_PASSWORD" + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
