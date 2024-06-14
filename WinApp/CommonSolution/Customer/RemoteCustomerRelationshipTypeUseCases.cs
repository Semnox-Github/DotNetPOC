/********************************************************************************************
* Project Name - Customer
* Description  - RemoteCustomerRelationshipTypeUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    31-Aug-2021      Mushahid Faizan        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    public class RemoteCustomerRelationshipTypeUseCases : RemoteUseCases, ICustomerRelationshipTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ORDERTYPE_URL = "api/Customer/CustomerRelationshipTypes";
        private const string ORDERTYPE_CONTAINER_URL = "api/Customer/CustomerRelationshipTypeContainer";

        public RemoteCustomerRelationshipTypeUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<CustomerRelationshipTypeDTO>> GetCustomerRelationshipType(List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<CustomerRelationshipTypeDTO> result = await Get<List<CustomerRelationshipTypeDTO>>(ORDERTYPE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> orderTypeSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string> searchParameter in orderTypeSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }


        public async Task<CustomerRelationshipTypeContainerDTOCollection> GetCustomerRelationshipTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            CustomerRelationshipTypeContainerDTOCollection result = await Get<CustomerRelationshipTypeContainerDTOCollection>(ORDERTYPE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
