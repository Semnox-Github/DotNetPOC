/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteLocationUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory.Location
{
    public class RemoteLocationUseCases : RemoteUseCases, ILocationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string LOCATION_URL = "api/Inventory/Locations";
        private const string LOCATION_CONTAINER_URL = "/api/Inventory/LocationContainer";
        private const string LOCATION_COUNT_URL = "api/Inventory/LocationCounts";

        public RemoteLocationUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<LocationDTO>> GetLocations(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>
                          parameters, SqlTransaction sqlTransaction = null,
                          bool includeWastageLocation = false, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("includeWastageLocation".ToString(), includeWastageLocation.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<LocationDTO> result = await Get<List<LocationDTO>>(LOCATION_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetLocationCount(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>
                          parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int result = await Get<int>(LOCATION_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<LocationDTO.SearchByLocationParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case LocationDTO.SearchByLocationParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case LocationDTO.SearchByLocationParameters.LOCATION_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("locationName".ToString(), searchParameter.Value));
                        }
                        break;
                    case LocationDTO.SearchByLocationParameters.LOCATION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("locationId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LocationDTO.SearchByLocationParameters.ISSTORE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isStore".ToString(), searchParameter.Value));
                        }
                        break;
                    case LocationDTO.SearchByLocationParameters.MASSUPDATEALLOWED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isMassUpdateAllowed".ToString(), searchParameter.Value));
                        }
                        break;
                    case LocationDTO.SearchByLocationParameters.ISREMARKSMANDATORY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isRemarksMandatory".ToString(), searchParameter.Value));
                        }
                        break;
                    case LocationDTO.SearchByLocationParameters.BARCODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("barcode".ToString(), searchParameter.Value));
                        }
                        break;
                    case LocationDTO.SearchByLocationParameters.CUSTOMDATASETID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customDataSetId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LocationDTO.SearchByLocationParameters.LOCATION_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("locationTypeId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveLocations(List<LocationDTO> locationDTOList)
        {
            log.LogMethodEntry(locationDTOList);
            try
            {
                string responseString = await Post<string>(LOCATION_URL, locationDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<LocationContainerDTOCollection> GetLocationContainerDTOCollection(int siteId,string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            LocationContainerDTOCollection result = await Get<LocationContainerDTOCollection>(LOCATION_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

    }
}
