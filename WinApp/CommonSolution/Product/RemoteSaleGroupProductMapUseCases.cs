/********************************************************************************************
 * Project Name -Product
 * Description  -SaleGroupProductMapUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    19-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    class RemoteSaleGroupProductMapUseCases:RemoteUseCases,ISaleGroupProductMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SALESGROUPPRODUCTMAP_URL = "api/Product/SaleGroupProductMaps";
        public RemoteSaleGroupProductMapUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<SaleGroupProductMapDTO>> GetSaleGroupProductMaps(List<KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>>
                   searchParameters)
        {
            log.LogMethodEntry(searchParameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<SaleGroupProductMapDTO> result = await Get<List<SaleGroupProductMapDTO>>(SALESGROUPPRODUCTMAP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.TYPE_MAP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("typeMapId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SALE_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("saleGroupId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SQUENCE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("sequenceId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveSaleGroupProductMaps(List<SaleGroupProductMapDTO> saleGroupProductMapList)
        {
            log.LogMethodEntry(saleGroupProductMapList);
            try
            {
                string responseString = await Post<string>(SALESGROUPPRODUCTMAP_URL, saleGroupProductMapList);
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
