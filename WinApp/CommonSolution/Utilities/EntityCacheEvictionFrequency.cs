/********************************************************************************************
 * Project Name - Utilities
 * Description  - EntityCacheEvictionFrequency class to get the entity cache eviction frequency value
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         08-Dec-2020       Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Configuration;

namespace Semnox.Core.Utilities
{
    public static class EntityCacheEvictionFrequency
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static double refreshFrequency;
        private static double entityCacheEvictionFrequencyInMinutes;
        static EntityCacheEvictionFrequency()
        {
            log.LogMethodEntry();
            string dataRefreshFrequencyConfig = ConfigurationManager.AppSettings["EntityCacheEvictionFrequencyInMinutes"];
            if(double.TryParse(dataRefreshFrequencyConfig, out entityCacheEvictionFrequencyInMinutes) == false)
            {
                entityCacheEvictionFrequencyInMinutes = 30;
            }
            if(entityCacheEvictionFrequencyInMinutes < 1)
            {
                entityCacheEvictionFrequencyInMinutes = 30;
            }
            log.LogVariableState("entityCacheEvictionFrequencyInMinutes", entityCacheEvictionFrequencyInMinutes);
            refreshFrequency = entityCacheEvictionFrequencyInMinutes * 60 * 1000;
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
            log.LogMethodExit(entityCacheEvictionFrequencyInMinutes);
            return entityCacheEvictionFrequencyInMinutes;
        }
    }
}
