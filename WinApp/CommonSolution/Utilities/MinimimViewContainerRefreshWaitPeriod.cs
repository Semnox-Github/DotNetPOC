/********************************************************************************************
 * Project Name - Utilities
 * Description  - MinimimViewContainerRefreshWaitPeriod class to get the minimum wait period before refreshing the view container
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Lakshminarayana             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System.Configuration;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// MinimimViewContainerRefreshWaitPeriod class to get the minimum wait period before refreshing the view container
    /// </summary>
    public static class MinimimViewContainerRefreshWaitPeriod
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static double valueInMinutes;
        static MinimimViewContainerRefreshWaitPeriod()
        {
            log.LogMethodEntry();
            
            string config = ConfigurationManager.AppSettings["MinimimViewContainerRefreshWaitPeriodInMinutes"];
            if(double.TryParse(config, out valueInMinutes) == false)
            {
                valueInMinutes = 1;
            }
            if(valueInMinutes < 1)
            {
                valueInMinutes = 1;
            }
            if(valueInMinutes > DataRefreshFrequency.GetValueInMinutes())
            {
                valueInMinutes = DataRefreshFrequency.GetValueInMinutes() / 2;
            }
            log.LogVariableState("MinimimViewContainerRefreshWaitPeriod", valueInMinutes);
            log.LogMethodExit();
        }
        public static double GetValueInMinutes()
        {
            log.LogMethodEntry();
            log.LogMethodExit(valueInMinutes);
            return valueInMinutes;
        }
    }
}
