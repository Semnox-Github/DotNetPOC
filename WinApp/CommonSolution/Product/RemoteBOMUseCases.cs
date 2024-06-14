/********************************************************************************************
 * Project Name -Product
 * Description  -BOMUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    09-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class RemoteBOMUseCases:RemoteUseCases, IBOMUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string BOM_URL = "api/Product/ProductBOM";

        public RemoteBOMUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<BOMDTO>> GetBOMs(List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>>
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
                List<BOMDTO> result = await Get<List<BOMDTO>>(BOM_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<BOMDTO.SearchByBOMParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case BOMDTO.SearchByBOMParameters.BOMID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("bOMId".ToString(), searchParameter.Value));
                        }
                        break;
                    case BOMDTO.SearchByBOMParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isactive".ToString(), searchParameter.Value));
                        }
                        break;
                    case BOMDTO.SearchByBOMParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                    case BOMDTO.SearchByBOMParameters.UOM_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("uOMId".ToString(), searchParameter.Value));
                        }
                        break;
                    case BOMDTO.SearchByBOMParameters.PRODUCT_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case BOMDTO.SearchByBOMParameters.CHILDPRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("childProductId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveBOMs(List<BOMDTO> bOMDTOList)
        {
            log.LogMethodEntry(bOMDTOList);
            try
            {
                string responseString = await Post<string>(BOM_URL, bOMDTOList);
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
