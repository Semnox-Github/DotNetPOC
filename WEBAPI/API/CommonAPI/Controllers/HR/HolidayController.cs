/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the Holiday.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.70.0      19-Oct-2019     Indrajeet Kumar     Created
*2.90        20-May-2020     Vikas Dwivedi       Modified as per the Standard CheckList
*2.120.0     01-Apr-2021     Prajwal S           Modified.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Semnox.Parafait.User;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.HR
{
    public class HolidayController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Holiday List
        /// This method will fetch all the Holiday record
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/Holidays")]
        public async Task<HttpResponseMessage> Get(int holidayId = -1, string name = null, DateTime? date = null, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(holidayId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<HolidayDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<HolidayDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<HolidayDTO.SearchByParameters, string>(HolidayDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (holidayId > -1)
                {
                    searchParameters.Add(new KeyValuePair<HolidayDTO.SearchByParameters, string>(HolidayDTO.SearchByParameters.HOLIDAY_ID, holidayId.ToString()));
                }
                if (!string.IsNullOrEmpty(name))
                {
                    searchParameters.Add(new KeyValuePair<HolidayDTO.SearchByParameters, string>(HolidayDTO.SearchByParameters.NAME, name.ToString()));
                }
                if (date != null)
                {
                    date = Convert.ToDateTime(date.ToString());
                    if (date == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<HolidayDTO.SearchByParameters, string>(HolidayDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                IHolidayUseCases holidayUseCases = UserUseCaseFactory.GetHolidayUseCases(executionContext);
                List<HolidayDTO> holidayDTOList = await holidayUseCases.GetHoliday(searchParameters);
                log.LogMethodExit(holidayDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = holidayDTOList,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation on Holiday
        /// </summary>
        /// <param name="holidayDTOList"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/Holidays")]
        public async Task<HttpResponseMessage> Post([FromBody] List<HolidayDTO> holidayDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(holidayDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (holidayDTOList == null || holidayDTOList.Any(a => a.HolidayId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IHolidayUseCases holidayUseCases = UserUseCaseFactory.GetHolidayUseCases(executionContext);
                List<HolidayDTO> holidayDTOLists = await holidayUseCases.SaveHoliday(holidayDTOList);
                log.LogMethodExit(holidayDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = holidayDTOLists,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the HolidayList collection
        /// <param name="holidayDTOList">HolidayList</param>
        [HttpPut]
        [Route("api/HR/Holidays")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<HolidayDTO> holidayDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(holidayDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (holidayDTOList == null || holidayDTOList.Any(a => a.HolidayId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IHolidayUseCases holidayUseCases = UserUseCaseFactory.GetHolidayUseCases(executionContext);
                await holidayUseCases.SaveHoliday(holidayDTOList);
                log.LogMethodExit(holidayDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Delete operation on Holiday Details
        /// </summary>
        /// <param name="holidayDTOList"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Authorize]
        [Route("api/HR/Holidays")]
        public HttpResponseMessage Delete([FromBody] List<HolidayDTO> holidayDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(holidayDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (holidayDTOList != null && holidayDTOList.Any())
                {
                    {
                        IHolidayUseCases holidayUseCases = UserUseCaseFactory.GetHolidayUseCases(executionContext);
                        holidayUseCases.DeleteHoliday(holidayDTOList);
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                    }
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });

                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
