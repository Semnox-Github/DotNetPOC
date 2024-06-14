/********************************************************************************************
 * Project Name - PriceList
 * Description  - Factory class to instantiate the priceList use cases 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    10-Mar-2021       Roshan Devadiga         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.PriceList
{
    /// <summary>
    /// Factory class to instantiate the priceList use cases
    /// </summary>
    public class PriceListUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        ///GetPriceListUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IPriceListUseCases GetPriceListUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPriceListUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemotePriceListUseCases(executionContext);
            }
            else
            {
                result = new LocalPriceListUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

    }
}
