/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalLocationUseCases class 
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
    public class LocalLocationUseCases : ILocationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalLocationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<LocationDTO>> GetLocations(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null,
                          bool includeWastageLocation = false, int currentPage = 0, int pageSize = 0)
        {
            return await Task<List<LocationDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                LocationList locationListBL = new LocationList(executionContext);
                List<LocationDTO> locationDTOList = locationListBL.GetAllLocations(searchParameters, sqlTransaction, includeWastageLocation, currentPage, pageSize);

                log.LogMethodExit(locationDTOList);
                return locationDTOList;
            });
        }

        public async Task<int> GetLocationCount(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                LocationList locationListBL = new LocationList(executionContext);
                int locationCount = locationListBL.GetLocationCount(searchParameters);
                log.LogMethodExit(locationCount);
                return locationCount;
            });
        }

        public async Task<string> SaveLocations(List<LocationDTO> locationDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(locationDTOList);
                if (locationDTOList == null)
                {
                    throw new ValidationException("locationDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (LocationDTO locationDTO in locationDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            LocationBL locationBL = new LocationBL(executionContext, locationDTO);
                            locationBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw ;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ;
                        }
                    }
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }


        public async Task<LocationContainerDTOCollection> GetLocationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<LocationContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    LocationContainerList.Rebuild(siteId);
                }
                List<LocationContainerDTO> currencyRuleContainerList = LocationContainerList.GetLocationContainerDTOList(siteId);
                LocationContainerDTOCollection result = new LocationContainerDTOCollection(currencyRuleContainerList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }


    }
}
