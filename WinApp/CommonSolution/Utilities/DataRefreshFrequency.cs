/********************************************************************************************
 * Project Name - Utilities
 * Description  - DataRefreshFrequency class to get the refresh frequency value
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public static class DataRefreshFrequency
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static double refreshFrequency;
        private static double dataRefreshFrequencyInMinutes;
        static DataRefreshFrequency()
        {
            log.LogMethodEntry();
            string dataRefreshFrequencyConfig = ConfigurationManager.AppSettings["DataRefreshFrequencyInMinutes"];
            if(double.TryParse(dataRefreshFrequencyConfig, out dataRefreshFrequencyInMinutes) == false)
            {
                dataRefreshFrequencyInMinutes = 5;
            }
            if(dataRefreshFrequencyInMinutes < 5)
            {
                dataRefreshFrequencyInMinutes = 5;
            }
            log.LogVariableState("dataRefreshFrequencyInMinutes", dataRefreshFrequencyInMinutes);
            refreshFrequency = dataRefreshFrequencyInMinutes * 60 * 1000;
            log.LogVariableState("refreshFrequency", refreshFrequency);
            log.LogMethodExit();
        }
        public static double GetValue()
        {
            log.LogMethodEntry();
            log.LogMethodExit(refreshFrequency);
            return refreshFrequency;
        }

        public static double GetValueInMinutes()
        {
            log.LogMethodEntry();
            log.LogMethodExit(dataRefreshFrequencyInMinutes);
            return dataRefreshFrequencyInMinutes;
        }
    }
}
