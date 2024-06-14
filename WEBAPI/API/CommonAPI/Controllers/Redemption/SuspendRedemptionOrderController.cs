/********************************************************************************************
* Project Name - CommnonAPI - POS Redemption Module 
* Description  - API for the SuspendRedemptionOrderController.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.110.0     31-Dec-2020     Girish Kundar      Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.POS.Redemption
{
    public class SuspendRedemptionOrderController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Redemption/RedemptionOrders/Suspend")]
        public async Task<HttpResponseMessage> Post(List<RedemptionDTO> redemptionDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(redemptionDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                RedemptionDTO content = await redemptionUseCases.SuspendOrders(redemptionDTOList);
                log.LogMethodExit(redemptionDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
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
