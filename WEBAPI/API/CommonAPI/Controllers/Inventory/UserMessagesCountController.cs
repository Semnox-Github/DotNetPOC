/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch user message count.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.150.0    04-Nov-2022   Abhishek                 Created - Web Inventory UI resdesign
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class UserMessagesCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON
        /// </summary>
        /// <returns>ResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/UserMessageCount")]
        public async Task<HttpResponseMessage> Get(int roleId, int userId = -1, string moduleType = null, string loginId = null, string status = null,
                                                   int currentPage = 0, int pageSize = 10)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(roleId, userId, moduleType, loginId, status);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IUserMessagesUseCases userMessagesUseCases = InventoryUseCaseFactory.GetUserMessagesUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalNoOfUserMessages = await userMessagesUseCases.GetUserMessagesCount(roleId, userId, moduleType, loginId, executionContext.GetSiteId(), status);
                log.LogVariableState("totalNoOfUserMessages", totalNoOfUserMessages);
                totalNoOfPages = (totalNoOfUserMessages / pageSize) + ((totalNoOfUserMessages % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalNoOfUserMessages);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfUserMessages, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });
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
