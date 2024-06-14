/********************************************************************************************
 * Project Name - GenerciOTP
 * Description  - GenerciOTPUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.150.0    19-Aug-2022       Yashdoahar C H          Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Core.GenericUtilities
{
    public class GenericOTPUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IGenericOTPUseCases GetGenericOTPUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IGenericOTPUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteGenericOTPUseCases(executionContext);
            }
            else
            {
                result = new LocalGenericOTPUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

    }
}
