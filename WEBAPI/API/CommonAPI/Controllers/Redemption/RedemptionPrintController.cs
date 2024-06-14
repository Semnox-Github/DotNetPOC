/********************************************************************************************
 * Project Name -  RedemptionPrint  Controller
 * Description  - Gets the Receipt 
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    20-Nov-2020   Girish Kundar            Created.
 ***************************************************************************************************/
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
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.POS.Redemption
{
    public class RedemptionPrintController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON String
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/Redemption/RedemptionOrder/{orderId}/Print")]
        public async Task<HttpResponseMessage> Get([FromUri] int orderId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>> redemptionCardSeacrhParams = new List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>>();
                redemptionCardSeacrhParams.Add(new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                var printObject =  await redemptionUseCases.GetRedemptionOrderPrint(orderId);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = printObject });

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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

    }
}
