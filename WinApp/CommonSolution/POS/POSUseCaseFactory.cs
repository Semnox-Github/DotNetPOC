/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - class of DateTimeRange
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      06-May-2021      Lakshminarayana           Modified: Static menu enhancement 
 2.140.0      14-Sep-2021      Deeksha                   Modified: Moved Shift use cases under User project 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.POS
{
    public class POSUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IPOSMachineUseCases GetPOSMachineUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPOSMachineUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemotePOSMachineUseCases(executionContext);
            }
            else
            {
                result = new LocalPOSMachineUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IPOSTypeUseCases GetPOSTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPOSTypeUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemotePOSTypeUseCases(executionContext);
            }
            else
            {
                result = new LocalPOSTypeUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetPOSPrinterOverrideRulesUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IPOSPrinterOverrideRulesUseCases GetPOSPrinterOverrideRulesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPOSPrinterOverrideRulesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemotePOSPrinterOverrideRulesUseCases(executionContext);
            }
            else
            {
                result = new LocalPOSPrinterOverrideRulesUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetPOSPrinterOverrideOptionsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IPOSPrinterOverrideOptionsUseCases GetPOSPrinterOverrideOptionsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPOSPrinterOverrideOptionsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemotePOSPrinterOverrideOptionsUseCases(executionContext);
            }
            else
            {
                result = new LocalPOSPrinterOverrideOptionsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// GetOverrideItemsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IOverrideItemUseCases GetOverrideItemsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IOverrideItemUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteOverrideItemUseCases(executionContext);
            }
            else
            {
                result = new LocalOverrideItemUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetProductMenuUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IProductMenuUseCases GetProductMenuUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductMenuUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductMenuUseCases(executionContext);
            }
            else
            {
                result = new LocalProductMenuUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetFacilityPOSAssignmentsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IFacilityPOSAssignmentUseCases GetFacilityPOSAssignmentUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IFacilityPOSAssignmentUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteFacilityPOSAssignmentUseCases(executionContext);
            }
            else
            {
                result = new LocalFacilityPOSAssignmentUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
