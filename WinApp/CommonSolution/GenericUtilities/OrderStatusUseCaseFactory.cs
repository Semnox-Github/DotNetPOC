/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - OrderStatusUseCaseFactory.cs
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      10-Mar-2020     Girish Kundar                Created : urban Pipers changes
 ********************************************************************************************/
using System;
using System.Configuration;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Core.GenericUtilities
{
    public class OrderStatusUseCaseFactory
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IOrderStatusUseCases GetOrderStatusUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IOrderStatusUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteOrderStatusUseCases(executionContext);
            }
            else
            {
                result = new LocalOrderStatusUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
