/********************************************************************************************
 * Project Name - Tools Controller
 * Description  - Created to fetch, update and insert locker access point
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        03-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
 *2.90        20-May-2020   Mushahid Faizan           Modified :As per Rest API standard, Added searchParams 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Lockers;
namespace Semnox.CommonAPI.Locker
{
    public class LockerAccessPointController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON locker access point
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Locker/LockerAccessPoints")]
        public HttpResponseMessage Get(string isActive = null, int accessPointId = -1, decimal ipAddress = -1, bool isAlive = false, int lockerIdFrom = -1, int lockerIdTo = -1, string lockerName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, accessPointId, ipAddress, isAlive, lockerIdFrom, lockerIdTo, lockerName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                LockerAccessPointList lockerAccessPointList = new LockerAccessPointList(executionContext);
                List<KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>> searchParameters = new List<KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>>();
                searchParameters.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (accessPointId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.ACCESS_POINT_ID, accessPointId.ToString()));
                }
                if (ipAddress > -1)
                {
                    searchParameters.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.IP_ADDRESS, ipAddress.ToString()));
                }
                if (isAlive)
                {
                    searchParameters.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.IS_ALIVE, "1"));
                }
                if (lockerIdFrom > -1)
                {
                    searchParameters.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.LOCKER_ID_FROM, lockerIdFrom.ToString()));
                }
                if (lockerIdTo > -1)
                {
                    searchParameters.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.LOCKER_ID_TO, lockerIdTo.ToString()));
                }
                if (!string.IsNullOrEmpty(lockerName))
                {
                    searchParameters.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.NAME, lockerName.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                List<LockerAccessPointDTO> content = lockerAccessPointList.GetAllLockerAccessPoint(searchParameters);
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
        /// Post the JSON locker acess point
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Locker/LockerAccessPoints")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<LockerAccessPointDTO> lockerAccessPointDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(lockerAccessPointDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (lockerAccessPointDTOList != null && lockerAccessPointDTOList.Any())
                {
                    LockerAccessPointList lockerAccessPointList = new LockerAccessPointList(executionContext, lockerAccessPointDTOList);
                    lockerAccessPointList.Save();
                    log.LogMethodExit(lockerAccessPointDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = lockerAccessPointDTOList });
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