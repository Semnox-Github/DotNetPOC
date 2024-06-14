/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteLocationTypeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory.Location;

namespace Semnox.Parafait.Inventory
{
    public class RemoteLocationTypeUseCases : RemoteUseCases, ILocationTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string LOCATION_TYPE_URL = "api/Inventory/LocationTypes";
        private const string LOCATION_TYPE_CONTAINER_URL = "api/Inventory/LocationTypeContainer";

        public RemoteLocationTypeUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<LocationTypeDTO>> GetLocationTypes(List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<LocationTypeDTO> result = await Get<List<LocationTypeDTO>>(LOCATION_TYPE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case LocationTypeDTO.SearchByLocationTypeParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("locationTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("locationType".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveLocationTypes(List<LocationTypeDTO> locationTypeDTOList)
        {
            log.LogMethodEntry(locationTypeDTOList);
            try
            {
                string responseString = await Post<string>(LOCATION_TYPE_URL, locationTypeDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<LocationTypeContainerDTOCollection> GetLocationTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            LocationTypeContainerDTOCollection result = await Get<LocationTypeContainerDTOCollection>(LOCATION_TYPE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
