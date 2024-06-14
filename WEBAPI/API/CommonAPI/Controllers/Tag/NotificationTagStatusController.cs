/**************************************************************************************************
 * Project Name - Games 
 * Description  - Controller for Notification Tag Status
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.150.2     28-Nov-2022       Abhishek                  Created - Game Server Cloud Movement.
 **************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Tags;

namespace Semnox.CommonAPI.Controllers.Tags
{
    public class NotificationTagStatusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON NotificationTagStatusDTOList
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Tag/NotificationTag/{notificationTagId}/StatusLog")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int notificationTagId, [FromBody]List<NotificationTagStatusDTO> notificationTagStatusDTOList)
        {
            log.LogMethodEntry(notificationTagId, notificationTagStatusDTOList);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                if (notificationTagStatusDTOList != null && notificationTagStatusDTOList.Any() && notificationTagId > -1)
                {
                    INotificationTagStatusUseCases notificationTagStatusUseCases = TagUseCaseFactory.GetNotificationTagStatusUseCases(executionContext);
                    List<NotificationTagStatusDTO> content = await notificationTagStatusUseCases.SaveNotificationTagStatus(notificationTagId, notificationTagStatusDTOList);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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
