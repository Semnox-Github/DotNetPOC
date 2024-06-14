/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Users Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         11-Mar-2019   Jagan Mohana         Created 
 *2.60         08-May-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
                                                  Added isActive SearchParameter in HttpGet Method.
*2.80         05-Apr-2020   Girish Kundar         Modified: API path change and removed token from the response body  
*2.110.0      12-Nov-2019   Lakshminarayana       Modified: Changed as part of POS UI Redesign  
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.HR
{
    public class UserController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Users Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/Users")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int userId = -1, int userRoleId = -1, string userName = null, string userStatus = null,
                                     int empNumber = -1, int departmentId = -1, string cardNumber = null, bool loadUserTags = false, bool activeChildRecords = true)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IUserUseCases userUseCases = UserUseCaseFactory.GetUserUseCases(executionContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, Convert.ToString(executionContext.SiteId)));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, isActive));
                    }
                }
                if (userId > -1)
                {
                    searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, userId.ToString()));
                }
                if (userRoleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_ID, userRoleId.ToString()));
                }
                if (empNumber > -1)
                {
                    searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.EMP_NUMBER, empNumber.ToString()));
                }
                if (departmentId > -1)
                {
                    searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.DEPARTMENT_ID, departmentId.ToString()));
                }
                if (string.IsNullOrEmpty(userName) == false)
                {
                    searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_NAME, userName));
                }
                if (string.IsNullOrEmpty(userStatus) == false)
                {
                    searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_STATUS, userStatus));
                }
                if (string.IsNullOrEmpty(cardNumber) == false)
                {
                    searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.CARD_NUMBER, cardNumber));
                }
                List<UsersDTO> userDTOList = await userUseCases.GetUserDTOList(searchParameters, loadUserTags, activeChildRecords);
                log.LogMethodExit(userDTOList);
                UserContainerDTO userContainerDTO = UserViewContainerList.GetUserContainerDTO(executionContext.SiteId, executionContext.UserId);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = userDTOList,
                    currentUserRoleId = userContainerDTO.RoleId
                });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Performs a Post operation on usersDTOs details
        /// </summary>         
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/HR/Users")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<UsersDTO> usersDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IUserUseCases userUseCases = UserUseCaseFactory.GetUserUseCases(executionContext);
                List<UsersDTO> userDTOList = await userUseCases.SaveUsersDTOList(usersDTOList);
                log.LogMethodExit(userDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = userDTOList,
                });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
