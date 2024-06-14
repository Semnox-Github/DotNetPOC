/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the TaskTypes.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.120.00    02-Mar-2021   Roshan Devadiga       Created : POS UI redesign
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TaskTypeContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/TaskTypesContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);

                TaskTypesContainerDTOCollection taskTypesContainerDTOCollection = await
                          Task<TaskTypesContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return TaskTypesViewContainerList.GetTaskTypesContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(taskTypesContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = taskTypesContainerDTOCollection });
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
