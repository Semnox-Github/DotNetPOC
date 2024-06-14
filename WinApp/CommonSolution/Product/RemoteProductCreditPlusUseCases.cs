/********************************************************************************************
 * Project Name - Product
 * Description  - RemoteProductCreditPlusUseCases Class
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.0      01-April-2021     B Mahesh Pai              Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class RemoteProductCreditPlusUseCases: RemoteUseCases, IProductCreditPlusUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTCREDITPLUS_URL = "api/HR/ProductCreditPluss";
        

        public RemoteProductCreditPlusUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<ProductCreditPlusDTO>> GetProductCreditPlus(List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>>
                        parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ProductCreditPlusDTO> result = await Get<List<ProductCreditPlusDTO>>(PRODUCTCREDITPLUS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductCreditPlusDTO.SearchByParameters.PRODUCTCREDITPLUS_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ProductCreditPlusId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductCreditPlusDTO.SearchByParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ProductId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductCreditPlusDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductCreditPlusDTO.SearchByParameters.PRODUCT_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ProductIdList".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveProductCreditPlus(List<ProductCreditPlusDTO> productCreditPlusDTOList)
        {
            log.LogMethodEntry(productCreditPlusDTOList);
            try
            {
                string responseString = await Post<string>(PRODUCTCREDITPLUS_URL, productCreditPlusDTOList);
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
