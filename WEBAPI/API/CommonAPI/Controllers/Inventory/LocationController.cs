/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert for Location entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.100.0     04-Oct-2020  Mushahid Faizan         Modified: as per API Standards, namespace changes, endPoint Changes, added searchParameters in get(),
 *                                                 Removed Delete().
 *2.110.0    23-Nov-2020   Mushahid Faizan         Web Inventory UI resdesign changes with REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Inventory.Location;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class LocationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Location.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Locations")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int locationId = -1,  string locationIdList = null, string locationName = null, int locationTypeId = -1, string isRemarksMandatory = null,
                                        string isStore = null, string barcode = null, string isMassUpdateAllowed = null, int customDataSetId = -1, int currentPage = 0, int pageSize = 10,
                                        bool includeWastageLocation = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, locationId, locationName, locationTypeId, isRemarksMandatory,
                                    isStore, barcode, isMassUpdateAllowed, customDataSetId, currentPage, pageSize, includeWastageLocation);

                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                if (locationId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.LOCATION_ID, locationId.ToString()));
                }
                if (locationTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.LOCATION_TYPE_ID, locationTypeId.ToString()));
                }
                if (customDataSetId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.CUSTOMDATASETID, customDataSetId.ToString()));
                }
                if (!string.IsNullOrEmpty(locationName))
                {
                    searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.LOCATION_NAME, locationName));
                }
                if (!string.IsNullOrEmpty(isStore))
                {
                    searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.ISSTORE, isStore));
                }
                if (!string.IsNullOrEmpty(isMassUpdateAllowed))
                {
                    searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.MASSUPDATEALLOWED, isMassUpdateAllowed));
                }
                if (!string.IsNullOrEmpty(isRemarksMandatory))
                {
                    searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.ISREMARKSMANDATORY, isRemarksMandatory));
                }
                if (!string.IsNullOrEmpty(barcode))
                {
                    searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.BARCODE, barcode));

                }
                if (!string.IsNullOrEmpty(locationIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> locationListId = new List<int>();

                    locationListId = locationIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String requisitionListString = String.Join(",", locationListId.ToArray());
                    searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.LOCATION_ID_LIST, requisitionListString));
                }

                LocationList locationBlList = new LocationList(executionContext);

                int totalNoOfPages = 0;
                int totalNoOfLocations = await Task<int>.Factory.StartNew(() => { return locationBlList.GetLocationCount(searchParameters, null); });
                log.LogVariableState("totalNoOfLocations", totalNoOfLocations);
                totalNoOfPages = (totalNoOfLocations / pageSize) + ((totalNoOfLocations % pageSize) > 0 ? 1 : 0);

                ILocationUseCases locationUseCases = InventoryUseCaseFactory.GetLocationUseCases(executionContext);
                List<LocationDTO> locationDTOList = await locationUseCases.GetLocations(searchParameters, null, includeWastageLocation, currentPage, pageSize);
                log.LogMethodExit(locationDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = locationDTOList, currentPageNo = currentPage, TotalCount = totalNoOfLocations });

            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Location
        /// </summary>
        /// <param name="locationDTOList">locationDTOList</param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/Inventory/Locations")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<LocationDTO> locationDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(locationDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (locationDTOList != null && locationDTOList.Any(a => a.LocationId > 0))
                {
                    log.LogMethodExit(locationDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ILocationUseCases locationUseCases = InventoryUseCaseFactory.GetLocationUseCases(executionContext);
                await locationUseCases.SaveLocations(locationDTOList);
                log.LogMethodExit(locationDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = locationDTOList });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Location
        /// </summary>
        /// <param name="locationDTOList">locationDTOList</param>
        /// <returns>HttpMessage</returns>
        [HttpPut]
        [Route("api/Inventory/Locations")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<LocationDTO> locationDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(locationDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                // Checks if the LocationId id is greater than to 0, If it is greater than to 0, then it updates 
                if (locationDTOList != null && locationDTOList.Any(a => a.LocationId < 0))
                {
                    log.LogMethodExit(locationDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ILocationUseCases locationUseCases = InventoryUseCaseFactory.GetLocationUseCases(executionContext);
                await locationUseCases.SaveLocations(locationDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = locationDTOList });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

    }
}
