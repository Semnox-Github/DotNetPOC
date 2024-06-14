/********************************************************************************************
 * Project Name - Product
 * Description  - SalesOfferGroupUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    11-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class RemoteSalesOfferGroupUseCases:RemoteUseCases,ISalesOfferGroupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SALESOFFERGROUP_URL = "api/Product/SalesOfferGroups";

        public RemoteSalesOfferGroupUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<SalesOfferGroupDTO>> GetSalesOfferGroups(List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>>
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
                List<SalesOfferGroupDTO> result = await Get<List<SalesOfferGroupDTO>>(SALESOFFERGROUP_URL,searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.SALE_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("saleGroupId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("name".ToString(), searchParameter.Value));
                        }
                        break;
                    case SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveSalesOfferGroups(List<SalesOfferGroupDTO> salesOfferGroupsList)
        {
            log.LogMethodEntry(salesOfferGroupsList);
            try
            {
                string responseString = await Post<string>(SALESOFFERGROUP_URL, salesOfferGroupsList);
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
