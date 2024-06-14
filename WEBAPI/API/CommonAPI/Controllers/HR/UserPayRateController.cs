/********************************************************************************************
 * Project Name - PayTypeController
 * Description  - Controller to setup User Pay Rate
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.130       05-Jul-2021      Nitin Pai  Added: Attendance and Pay Rate enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Controllers
{
    public class UserPayRateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="scanIdentifierType"></param>
        /// <param name="requiredCount"></param>
        /// <param name="requestingSite"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/HR/UserPayRates")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<UserPayRateDTO> userPayRateDTOList)
        {
            log.LogMethodEntry(userPayRateDTOList);

            ExecutionContext executionContext = null;
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();

            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<UserPayRateDTO> savedUserPayDTOList = null;

                if (userPayRateDTOList != null && userPayRateDTOList.Any())
                {
                    UserPayRateListBL userPayRateListBL = new UserPayRateListBL(executionContext);
                    savedUserPayDTOList = await Task <List<UserPayRateDTO>>.Factory.StartNew(() => {
                        return userPayRateListBL.SaveUserPayRateDTOList(userPayRateDTOList);
                    });

                    log.LogMethodExit(savedUserPayDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = savedUserPayDTOList });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Bad data" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (ValidationException ex)
            {
                log.Error(ex.Message);

                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {

                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        /// <summary>
        /// Get the JSON Object of ShiftDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/UserPayRates")]
        public async Task<HttpResponseMessage> Get(int? userPayRateId = null, int? userId = null, int? userRoleId = null, DateTime? effectiveDate = null, string isActive = null)
        {
            log.LogMethodEntry(userId, userRoleId, effectiveDate, isActive);

            ExecutionContext executionContext = null;
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            try
            {

                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<UserPayRateDTO> userPayDTOList = null;
                UserPayRateListBL userPayRateListBL = new UserPayRateListBL(executionContext);

                List<KeyValuePair<UserPayRateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<UserPayRateDTO.SearchByParameters, string>>();

                if (userPayRateId != null)
                    searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_PAY_RATE_ID, userPayRateId.ToString()));

                if (userId != null)
                {
                    searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_ID, userId.ToString()));
                    if(userRoleId == null)
                        searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_ROLE_ID_IS_NULL, "1"));
                }

                if (userRoleId != null)
                {
                    searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_ROLE_ID, userRoleId.ToString()));
                    if (userId == null)
                        searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_ID_IS_NULL, "1"));
                }

                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.IS_ACTIVE, isActive));
                }

                if (effectiveDate != null)
                    searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.EFFECTIVE_DATE, Convert.ToDateTime(effectiveDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

                userPayDTOList = await Task<List<UserPayRateDTO>>.Factory.StartNew(() => {
                    return userPayRateListBL.GetUserPayRateDTOList(searchParameters);
                });

                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userPayDTOList });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
