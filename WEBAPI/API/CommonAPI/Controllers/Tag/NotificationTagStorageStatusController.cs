/********************************************************************************************
 * Project Name - Tags
 * Description  - Update the storage status
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.110.0    23-Apr-2021   Girish Kundar             Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Tags;

namespace Semnox.CommonAPI.Controllers.Tags
{
    public class NotificationTagStorageStatusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON NotificationTagsDTOList
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPut]
        [Route("api/Tag/NotificationTags/{tagId}/StorageStatus")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromUri] int tagId =-1 , bool isInStorage = true)
        {
            log.LogMethodEntry(tagId, isInStorage);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (tagId > -1)
                {
                    INotificationTagUseCases notificationTagUseCases = TagUseCaseFactory.GetNotificationTagUseCases(executionContext);
                    string result = await notificationTagUseCases.StorageInOutStatusChange(tagId,isInStorage);
                    log.LogMethodExit(result);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
