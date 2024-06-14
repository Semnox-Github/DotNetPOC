/********************************************************************************************
 * Project Name - Product Price
 * Description  - Factory class to instantiate product price use cases
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      18-Aug-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// ProductPriceUseCaseFactory
    /// </summary>
    public class ProductPriceUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// GetProductUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IProductPriceUseCases GetProductPriceUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductPriceUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductPriceUseCases(executionContext);
            }
            else
            {
                result = new LocalProductPriceUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
