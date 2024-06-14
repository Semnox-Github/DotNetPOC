/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the Parafait Configuration Values.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.100         11-May-2020   Girish Kundar        Created : POS UI redesign
 ********************************************************************************************/


using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
namespace Semnox.CommonAPI.Configuration
{
    public class ParafaitDefaultContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/ParafaitDefaultContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, int userPkId = -1, int machineId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IParafaitDefaultUseCases service = ParafaitDefaultUseCaseFactory.GetParafaitDefaultUseCases(executionContext);
                ParafaitDefaultContainerDTOCollection defaultViewDTOCollection = await service.GetParafaitDefaultContainerDTOCollection(siteId, userPkId, machineId, hash, rebuildCache);
                log.LogMethodExit(defaultViewDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = defaultViewDTOCollection });
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
