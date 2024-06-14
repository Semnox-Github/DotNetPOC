/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalLocationTypeUseCases
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0    14-Oct-2020   Mushahid Faizan Created 
 ********************************************************************************************/
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory.Location;

namespace Semnox.Parafait.Inventory
{
    public class LocalLocationTypeUseCases : ILocationTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalLocationTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<LocationTypeDTO>> GetLocationTypes(List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> searchParameters)
        {
            return await Task<List<LocationTypeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                LocationTypeList locationTypeListBL = new LocationTypeList(executionContext);
                List<LocationTypeDTO> locationTypeDTOList = locationTypeListBL.GetAllLocationType(searchParameters);
                log.LogMethodExit(locationTypeDTOList);
                return locationTypeDTOList;
            });
        }
        public async Task<string> SaveLocationTypes(List<LocationTypeDTO> locationTypeDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(locationTypeDTOList);
                if (locationTypeDTOList == null)
                {
                    throw new ValidationException("locationTypeDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (LocationTypeDTO locationTypeDTO in locationTypeDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            LocationType locationType = new LocationType(executionContext, locationTypeDTO);
                            locationType.Save(parafaitDBTrx.SQLTrx);
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

        public async Task<LocationTypeContainerDTOCollection> GetLocationTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<LocationTypeContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    LocationTypeContainerList.Rebuild(siteId);
                }
                List<LocationTypeContainerDTO> locationTypeContainerList = LocationTypeContainerList.GetLocationTypeContainerDTOList(siteId);
                LocationTypeContainerDTOCollection result = new LocationTypeContainerDTOCollection(locationTypeContainerList);
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
