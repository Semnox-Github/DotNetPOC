/********************************************************************************************
 * Project Name - DeliveryIntegration
 * Description  - OnlineOrderDeliveryIntegrationUseCaseFactory.cs
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      13-Jul-2022     Guru S A                Created : urban Pipers changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// OnlineOrderDeliveryIntegrationUseCaseFactory
    /// </summary>
    public class OnlineOrderDeliveryIntegrationUseCaseFactory
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// IOnlineOrderDeliveryIntegrationUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IOnlineOrderDeliveryIntegrationUseCases GetOnlineOrderDeliveryIntegrationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IOnlineOrderDeliveryIntegrationUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteOnlineOrderDeliveryIntegrationUseCases(executionContext);
            }
            else
            {
                result = new LocalOnlineOrderDeliveryIntegrationUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
