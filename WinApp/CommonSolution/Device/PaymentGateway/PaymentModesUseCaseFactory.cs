/********************************************************************************************
 * Project Name - Device
 * Description  - PaymentModesUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      18-Aug-2021      Fiona                    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class PaymentModesUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IPaymentModesUseCases GetPaymentModesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPaymentModesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemotePaymentModesUseCases(executionContext);
            }
            else
            {
                result = new LocalPaymentModesUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
