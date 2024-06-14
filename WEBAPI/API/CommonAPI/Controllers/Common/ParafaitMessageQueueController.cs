/********************************************************************************************
* Project Name - CommnonAPI - Common
* Description  - API for the ParafaitMessageQueue Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.120.0     09-Mar-2021     Prajwal S           Created
*2.140.0     11-Feb-2022     Fiona Lishal        Modified to handle entityGuidList
********************************************************************************************/

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
using Semnox.Parafait.GenericUtilities;
namespace Semnox.CommonAPI.Common
{
    public class ParafaitMessageQueueController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Common/ParafaitMessageQueue")]
        public async Task<HttpResponseMessage> Get(int parafaitMessageQueueId = -1, string entityName = null, string isActive = null, List<string> entityGuidList = null)
        {                              
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(parafaitMessageQueueId, entityName, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> parafaitMessageQueueSearchParameters = new List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>>();
                parafaitMessageQueueSearchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (parafaitMessageQueueId > -1)
                {
                    parafaitMessageQueueSearchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.MESSAGE_QUEUE_ID, parafaitMessageQueueId.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(entityName))
                {
                    parafaitMessageQueueSearchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_NAME, entityName.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        parafaitMessageQueueSearchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                IParafaitMessageQueueUseCases parafaitMessageQueueUseCases = ParafaitMessageQueueUseCaseFactory.GetParafaitMessageQueueUseCases(executionContext);
                List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOLists = new List<ParafaitMessageQueueDTO>();
                if (entityGuidList != null && entityGuidList.Any())
                {
                    parafaitMessageQueueDTOLists = await parafaitMessageQueueUseCases.GetParafaitMessageQueueDTOList(entityGuidList, parafaitMessageQueueSearchParameters);
                }
                else
                {
                    parafaitMessageQueueDTOLists = await parafaitMessageQueueUseCases.GetParafaitMessageQueue(parafaitMessageQueueSearchParameters);
                }
               
                log.LogMethodExit(parafaitMessageQueueDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = parafaitMessageQueueDTOLists,
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
        /// Performs a Post operation 
        /// </summary>
        /// <param name="parafaitMessageQueueDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Common/ParafaitMessageQueue")]
        public async Task<HttpResponseMessage> Post([FromBody] List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(parafaitMessageQueueDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                IParafaitMessageQueueUseCases parafaitMessageQueueUseCases = ParafaitMessageQueueUseCaseFactory.GetParafaitMessageQueueUseCases(executionContext);
                List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOLists = await parafaitMessageQueueUseCases.SaveParafaitMessageQueue(parafaitMessageQueueDTOList);
                log.LogMethodExit(parafaitMessageQueueDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = parafaitMessageQueueDTOLists,
                });
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
