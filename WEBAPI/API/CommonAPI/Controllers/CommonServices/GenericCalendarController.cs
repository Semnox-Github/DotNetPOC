/********************************************************************************************
 * Project Name - GenericCalendar Controller                                                                         
 * Description  - COntroller of the GenericCalendar class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.40        06-Oct-2018    Jagan Mohana Rao         Created new clas to fetch and update Generic Calendar.
 *2.90        08-Jun-2020    Mushahid Faizan          Modified: Log method,EndPoint Changes,Response Status code and Removed Token from body, Removed if-else conditon.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using Semnox.Parafait.Game;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.CommonServices
{
    public class GenericCalendarController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a get operation on Generic Calendar.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/CommonServices/GenericCalendars")]
        [Authorize]
        public HttpResponseMessage Get(string entityName, string entityId, string isActive)
        {
            log.LogMethodEntry(entityName, entityId, isActive);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                GenericCalendar genericCalendar = new GenericCalendar(executionContext);
                List<GenericCalendarDTO> genericCalendarDTOs = new List<GenericCalendarDTO>();
                genericCalendarDTOs = genericCalendar.GetGenericCalendar(entityName, Convert.ToInt32(entityId), isActive.ToString());

                return Request.CreateResponse(HttpStatusCode.OK, new { data = genericCalendarDTOs });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on Generic Calendar.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/CommonServices/GenericCalendars")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<GenericCalendarDTO> genericCalendarDTOList)
        {
            log.LogMethodEntry(genericCalendarDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (genericCalendarDTOList != null && genericCalendarDTOList.Any())
                {
                    GenericCalendarList genericCalendarList = new GenericCalendarList(genericCalendarDTOList, executionContext);
                    genericCalendarList.SaveUpdateGenericCalendarValues();
                    log.LogMethodExit(genericCalendarDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = genericCalendarDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
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

        // Faizan : Commented Delete method we can use the Post itself for same behaviour.

        ///// <summary>
        ///// Performs a Delete operation on Generic Calendar.
        ///// </summary>        
        ///// <returns>HttpResponseMessage</returns>
        ////DELETE: api/Subscriber/5
        //[HttpDelete]
        //[Route("api/CommonServices/GenericCalendars")]
        //[Authorize]
        //public HttpResponseMessage Delete(List<GenericCalendarDTO> genericCalendarDTOList)
        //{
        //    SecurityTokenDTO securityTokenDTO = null;
        //    try
        //    {
        //        log.LogMethodEntry(genericCalendarDTOList);

        //        SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        //        securityTokenBL.GenerateJWTToken();
        //        securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
        //        ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

        //        int content = -1;
        //        if (genericCalendarDTOList != null && genericCalendarDTOList.Any())
        //        {
        //            GenericCalendarList genericCalendar = new GenericCalendarList(genericCalendarDTOList, executionContext);
        //             content = genericCalendar.SaveUpdateGenericCalendarValues();
        //        }
        //        if (content != 0)
        //        {
        //            log.Debug("GenericCalendar-Delete() Method. If condition");
        //            return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
        //        }
        //        else
        //        {
        //            log.Debug("GenericCalendar-Delete() Method. Else condition");
        //            return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
        //        }
        //    }
        //    catch (ValidationException valEx)
        //    {
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
        //    }
        //    catch (Exception ex)
        //    {
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
        //    }
        //}
    }
}