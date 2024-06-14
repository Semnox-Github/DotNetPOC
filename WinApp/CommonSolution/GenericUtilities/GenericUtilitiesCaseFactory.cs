/********************************************************************************************
 * Project Name - User
 * Description  - Factory class to instantiate the user use cases 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         13-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// Factory class to instantiate the user use cases
    /// </summary>
    public class GenericUtilitiesUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //public static IObjectTranslationsUseCases GetObjectTranslations(ExecutionContext executionContext)
        //{
        //    log.LogMethodEntry(executionContext);
        //    IObjectTranslationsUseCases result;
        //    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
        //    {
        //        result = new RemoteObjectTranslationsUseCases(executionContext);
        //    }
        //    else
        //    {
        //        result = new LocalObjectTranslationsUseCases(executionContext);
        //    }

        //    log.LogMethodExit(result);
        //    return result;
        //}
        public static IEntityOverrideDatesUseCases GetEntityOverrideDates(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IEntityOverrideDatesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteEntityOverrideDatesUseCases(executionContext);
            }
            else
            {
                result = new LocalEntityOverrideDatesUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static ICountryUseCases GetCountries(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICountryUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCountryUseCases(executionContext);
            }
            else
            {
                result = new LocalCountryUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static IScheduleCalendarUseCases GetScheduleCalendars(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IScheduleCalendarUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteScheduleCalendarUseCases(executionContext);
            }
            else
            {
                result = new LocalScheduleCalendarUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static IDefaultDataTypeUseCases GetDefaultDataTypes(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IDefaultDataTypeUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteDefaultDataTypeUseCases(executionContext);
            }
            else
            {
                result = new LocalDefaultDataTypeUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static IApplicationContentUseCases GetApplicationContents(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IApplicationContentUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteApplicationContentUseCases(executionContext);
            }
            else
            {
                result = new LocalApplicationContentUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static ICustomAttributesUseCases GetCustomAttributes(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomAttributesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomAttributesUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomAttributesUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetKioskSetupUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IKioskSetupUseCases GetKioskSetupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IKioskSetupUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteKioskSetupUseCases(executionContext);
            }
            else
            {
                result = new LocalKioskSetupUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
