/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the CustomAttributesContainerController.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.130.0    27-Jul-2021   Mushahid Faizan       Created 
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

namespace Semnox.CommonAPI.Controllers.Common
{
    public class CustomAttributesContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Common/CustomAttributesContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                CustomAttributesContainerDTOCollection customAttributesContainerDTOCollection = await
                           Task<CustomAttributesContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               return CustomAttributesViewContainerList.GetCustomAttributesContainerDTOCollection(siteId, hash, rebuildCache);
                           });

                log.LogMethodExit(customAttributesContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customAttributesContainerDTOCollection });
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
