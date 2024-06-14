/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch card details.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    29-Jul-2022   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalCustomerUIMetaDataController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the JSON Object CustomerUIMetadata
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/External/CustomerUIMetadata")]
        [Authorize]
        public HttpResponseMessage Get()
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log.Debug("Customer ui meta data site " + executionContext.GetSiteId());
                CustomerUIMetadataBL customerUIMetadataBL = new CustomerUIMetadataBL(executionContext);
                var content = customerUIMetadataBL.GetCustomerUIMetadataDTOList(executionContext.GetSiteId());
                if (content.Count > 0)
                {
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit("Customer UI Metadata not found");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "No data found" });
                }
            }
            catch (ValidationException valex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(valex) });
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