/********************************************************************************************
 * Project Name -Product
 * Description  -ProductAvailability UseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    06-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    class RemoteProductAvailabilityUseCases:RemoteUseCases,IProductAvailabilityUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTAVAILABILITY_URL = "api/Product/AvailableProducts";
        public RemoteProductAvailabilityUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<KeyValuePair<string, List<ProductsAvailabilityDTO>>>> GetProductAvailability(string loginId, bool searchUnavailableProduct = false)
        {
            log.LogMethodEntry(loginId, searchUnavailableProduct);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("searchUnavailableProduct".ToString(), searchUnavailableProduct.ToString()));
            try
            {
                List<KeyValuePair<string, List<ProductsAvailabilityDTO>>> responseString = await Get<List<KeyValuePair<string, List<ProductsAvailabilityDTO>>>>(PRODUCTAVAILABILITY_URL, searchParameterList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<List<ValidationError>> SaveAvailableProducts(List<ProductsAvailabilityDTO> productsAvailabilityDTOList, string loginId)
        {
            log.LogMethodEntry(productsAvailabilityDTOList);
            try
            {
                List<ValidationError> responseString = await Post<List<ValidationError>>(PRODUCTAVAILABILITY_URL,productsAvailabilityDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
