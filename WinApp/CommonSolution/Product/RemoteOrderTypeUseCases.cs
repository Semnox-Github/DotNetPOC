/********************************************************************************************
* Project Name - Product
* Description  - RemoteOrderTypeUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    19-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class RemoteOrderTypeUseCases : RemoteUseCases, IOrderTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ORDERTYPE_URL = "api/Transaction/OrderTypes";
        private const string ORDERTYPE_CONTAINER_URL = "api/Transaction/OrderTypeContainer";

        public RemoteOrderTypeUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<OrderTypeDTO>> GetOrderType(List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<OrderTypeDTO> result = await Get<List<OrderTypeDTO>>(ORDERTYPE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> orderTypeSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<OrderTypeDTO.SearchByParameters, string> searchParameter in orderTypeSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case OrderTypeDTO.SearchByParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case OrderTypeDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }


        public async Task<OrderTypeContainerDTOCollection> GetOrderTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            OrderTypeContainerDTOCollection result = await Get<OrderTypeContainerDTOCollection>(ORDERTYPE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
