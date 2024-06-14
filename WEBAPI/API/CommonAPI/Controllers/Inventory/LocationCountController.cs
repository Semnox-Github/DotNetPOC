/********************************************************************************************
 * Project Name - LocationCount Controller
 * Description  - Created LocationCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   11-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Inventory.Location;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class LocationCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Location.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/LocationCounts")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int locationId = -1, string locationName = null, int locationTypeId = -1, string isRemarksMandatory = null,
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

                LocationList locationBlList = new LocationList(executionContext);
                ILocationUseCases locationUseCases = InventoryUseCaseFactory.GetLocationUseCases(executionContext);

                int totalNoOfPages = 0;
                int totalNoOfLocations = await locationUseCases.GetLocationCount(searchParameters, null); 
                log.LogVariableState("totalNoOfLocations", totalNoOfLocations);
                totalNoOfPages = (totalNoOfLocations / pageSize) + ((totalNoOfLocations % pageSize) > 0 ? 1 : 0);

                log.LogMethodExit(totalNoOfLocations);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfLocations, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = ExceptionSerializer.Serialize(ex)
                });
            }
        }
    }
}