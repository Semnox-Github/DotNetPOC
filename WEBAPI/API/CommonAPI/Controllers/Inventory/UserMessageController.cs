/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Purchase Orders .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    09-Dec-2020  Mushahid Faizan         Created.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class UserMessageController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON
        /// </summary>
        /// <returns>ResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/UserMessages")]
        public async Task<HttpResponseMessage> Get(int roleId, int userId = -1, string moduleType = null, string loginId = null, string status = null,
                                                   bool buildPendingApprovalUserMessage = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(roleId, userId, moduleType, loginId, status);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IUserMessagesUseCases userMessagesUseCases = InventoryUseCaseFactory.GetUserMessagesUseCases(executionContext);
                List<UserMessagesDTO> userMessagesDTOList = await userMessagesUseCases.GetUserMessages(roleId, userId, moduleType, loginId, executionContext.GetSiteId(), status, buildPendingApprovalUserMessage);
                log.LogMethodExit(userMessagesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userMessagesDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

       

        /// <summary>
        /// Update the JSON Object UserMessageDTO
        /// </summary>
        /// <param name="userMessagesDTOList">purchaseOrderDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/UserMessages")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<UserMessagesDTO> userMessagesDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(userMessagesDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (userMessagesDTOList == null || userMessagesDTOList.Any(a => a.MessageId < 0))
                {
                    log.LogMethodExit(userMessagesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IUserMessagesUseCases userMessagesUseCases = InventoryUseCaseFactory.GetUserMessagesUseCases(executionContext);
                string response = await userMessagesUseCases.SaveUserMessages(userMessagesDTOList);

                log.LogMethodExit(userMessagesDTOList, response);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userMessagesDTOList, status = response});
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
