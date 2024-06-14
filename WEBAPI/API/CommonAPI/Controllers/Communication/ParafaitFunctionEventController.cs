/********************************************************************************************
* Project Name - CommnonAPI - Communication Module 
* Description  - API for the ParafaitFunctionEvents Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.110.0     10-Feb-2021     Guru S A            For subscription changes
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
using Semnox.Parafait.Communication;

namespace Semnox.CommonAPI.Communication
{
    public class ParafaitFunctionEventsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Communication/ParafaitFunctionEvents")]
        public async Task<HttpResponseMessage> Get(int parafaitFunctionEventId = -1, string parafaitFunctionEventName = null, int parafaitFunctonId = -1,  string isActive = null)
        {
            log.LogMethodEntry(parafaitFunctionEventId, parafaitFunctionEventName, parafaitFunctonId,isActive);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IParafaitFunctionEventUseCases parafaitFunctionEventUseCases = CommunicationUseCaseFactory.GetParafaitFunctionEventUseCases(executionContext);

                List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>> parafaitFunctonSearchParameters = new List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>>();
                parafaitFunctonSearchParameters.Add(new KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>(ParafaitFunctionEventDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (parafaitFunctonId > -1)
                {
                    parafaitFunctonSearchParameters.Add(new KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>(ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_ID, parafaitFunctonId.ToString()));
                }
                if (parafaitFunctionEventId > -1)
                {
                    parafaitFunctonSearchParameters.Add(new KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>(ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID, parafaitFunctionEventId.ToString()));
                }
                if (!string.IsNullOrEmpty(parafaitFunctionEventName))
                {
                    parafaitFunctonSearchParameters.Add(new KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>(ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_NAME, parafaitFunctionEventName.ToString()));
                }
                
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        parafaitFunctonSearchParameters.Add(new KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>(ParafaitFunctionEventDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                //ParafaitFunctionEventListBL parafaitFunctionsListBL = new ParafaitFunctionEventListBL(executionContext);
                List<ParafaitFunctionEventDTO> parafaitFunctionsDTOList = await parafaitFunctionEventUseCases.GetParafaitFunctionEvent(parafaitFunctonSearchParameters);
                log.LogMethodExit(parafaitFunctionsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = parafaitFunctionsDTOList
                });
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

        ///// <summary>
        ///// Performs a Post operation on ParafaitFunctionEventDTO details
        ///// </summary>        
        ///// <returns>HttpResponseMessage</returns>
        //[HttpPost]
        //[Route("api/Communication/ParafaitFunctionEvents")]
        //[Authorize]
        //public async Task<HttpResponseMessage> Post([FromBody] List<ParafaitFunctionEventDTO> parafaitFunctionEventDTOList)
        //{
        //    try
        //    {
        //        ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
        //        IParafaitFunctionEventUseCases parafaitFunctionsUseCases = CommunicationUseCaseFactory.GetParafaitFunctionEventUseCases(executionContext);
        //        //ParafaitFunctionEventsListBL parafaitFunctionsListBL = new ParafaitFunctionEventsListBL(executionContext, parafaitFunctionsDTOList);
        //        var content = await parafaitFunctionsUseCases.SaveParafaitFunctionEvent(parafaitFunctionEventDTOList);
        //        log.LogMethodExit(content);
        //        return Request.CreateResponse(HttpStatusCode.OK, new
        //        {
        //            data = content
        //        });
        //    }
        //    catch (Exception ex)
        //    {

        //        log.Error(ex);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new
        //        {
        //            data = ExceptionSerializer.Serialize(ex)
        //        });
        //    }
        //}
    }
}