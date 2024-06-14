/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the MIFARE key controller.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 2.200.0       17-Nov-2020   Lakshminarayana    Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Configuration
{
    public class SystemOptionContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/SystemOptionContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                SystemOptionContainerDTOCollection systemOptionContainerDTOCollection = await
                           Task<SystemOptionContainerDTOCollection>.Factory.StartNew(() => {
                                    if(rebuildCache)
                                    {
                                        SystemOptionViewContainerList.Rebuild(siteId);
                                    }
                                    return SystemOptionViewContainerList.GetSystemOptionContainerDTOCollection(siteId, hash);
                                  });
                log.LogMethodExit(systemOptionContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = systemOptionContainerDTOCollection });
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