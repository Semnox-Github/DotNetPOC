/********************************************************************************************
 * Project Name -  Location Controller
 * Description  - Created to fetch the  LocationType entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0     28-Oct-2020  Mushahid Faizan         Created.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class LocationTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Location Type.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/LocationTypes")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int locationTypeId = -1, string locationType = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> searchParameters = new List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>>();
                searchParameters.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                if (locationTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE_ID, locationTypeId.ToString()));
                }
                if (!string.IsNullOrEmpty(locationType))
                {
                    searchParameters.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE, locationType));
                }

                ILocationTypeUseCases locationTypeUseCases = InventoryUseCaseFactory.GetLocationTypeUseCases(executionContext);
                List<LocationTypeDTO> locationTypeDTOList = await locationTypeUseCases.GetLocationTypes(searchParameters);

                log.LogMethodExit(locationTypeDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = locationTypeDTOList });

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
