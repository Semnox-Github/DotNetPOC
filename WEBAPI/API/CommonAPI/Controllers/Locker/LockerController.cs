/********************************************************************************************
 * Project Name - Tools Controller
 * Description  - Created to fetch, update and insert locker management zones
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        02-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
 *2.90        26-May-2020   Mushahid Faizan           Modified :As per Rest API standard,Added SearchParams and Renamed controller from LockerZonesController to LockerController
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Lockers;
namespace Semnox.CommonAPI.Locker
{
    public class LockerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON locker zones
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Locker/Lockers")]
        public HttpResponseMessage Get(string isActive = null, int zoneId = -1, string zoneName = null, int parentZoneId = -1, string zoneCode = null, string lockerMode = null,
                                         bool loadInactiveChildRecord = false)
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

                LockerZonesList lockerZonesList = new LockerZonesList(executionContext);
                List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(lockerMode))
                {
                    searchParameters.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.LOCKER_MODE, lockerMode.ToString()));
                }
                if (parentZoneId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.PARENT_ZONE_ID, parentZoneId.ToString()));
                }
                if (!string.IsNullOrEmpty(zoneCode))
                {
                    searchParameters.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ZONE_CODE, zoneCode.ToString()));
                }
                if (zoneId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ZONE_ID, zoneId.ToString()));
                }
                if (!string.IsNullOrEmpty(zoneName))
                {
                    searchParameters.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ZONE_NAME, zoneName.ToString()));
                }

                var content = lockerZonesList.GetLockerZonesList(searchParameters, loadInactiveChildRecord);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON locker zones
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Locker/Lockers")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<LockerZonesDTO> lockerZonesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(lockerZonesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (lockerZonesDTOList != null && lockerZonesDTOList.Any())
                {
                    LockerZonesList lockerZonesList = new LockerZonesList(executionContext, lockerZonesDTOList);
                    lockerZonesList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = lockerZonesDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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