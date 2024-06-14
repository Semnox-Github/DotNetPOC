/********************************************************************************************
 * Project Name - DeliveryIntegration
 * Description  - DeliveryChannelUseCaseFactory.cs
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      10-Mar-2020     Girish Kundar                Created : urban Pipers changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.DeliveryIntegration
{
    public class DeliveryChannelUseCaseFactory
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IDeliveryChannelUseCases GetDeliveryChannelUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IDeliveryChannelUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteDeliveryChannelUseCases(executionContext);
            }
            else
            {
                result = new LocalDeliveryChannelUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
