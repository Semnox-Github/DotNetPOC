/********************************************************************************************
* Project Name - CommnonAPI - Communication Module 
* Description  - API for the ParafaitFunctions Controller.
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
    public class ParafaitFunctionsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Communication/ParafaitFunctions")]
        public async Task<HttpResponseMessage> Get(int parafaitFunctionId = -1, string parafaitFunctionName = null, string isActive = null, bool loadChildren = false, bool loadActiveChild = false)
        {
            log.LogMethodEntry(parafaitFunctionId, parafaitFunctionName, isActive, loadChildren, loadActiveChild);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IParafaitFunctionsUseCases parafaitFunctionsUseCases = CommunicationUseCaseFactory.GetParafaitFunctionsUseCases(executionContext);

                List<KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>> parafaitFunctonSearchParameters = new List<KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>>();
                parafaitFunctonSearchParameters.Add(new KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>(ParafaitFunctionsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (parafaitFunctionId > -1)
                {
                    parafaitFunctonSearchParameters.Add(new KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>(ParafaitFunctionsDTO.SearchByParameters.PARAFAIT_FUNCTION_ID, parafaitFunctionId.ToString()));
                }
                if (!string.IsNullOrEmpty(parafaitFunctionName))
                {
                    parafaitFunctonSearchParameters.Add(new KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>(ParafaitFunctionsDTO.SearchByParameters.PARAFAIT_FUNCTION_NAME, parafaitFunctionName.ToString()));
                } 
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        parafaitFunctonSearchParameters.Add(new KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>(ParafaitFunctionsDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }  
                ParafaitFunctionsListBL parafaitFunctionsListBL = new ParafaitFunctionsListBL(executionContext);
                List<ParafaitFunctionsDTO> parafaitFunctionsDTOList = await parafaitFunctionsUseCases.GetParafaitFunctions(parafaitFunctonSearchParameters, loadChildren, loadActiveChild);
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
        ///// Performs a Post operation on ParafaitFunctionsDTO details
        ///// </summary>        
        ///// <returns>HttpResponseMessage</returns>
        //[HttpPost]
        //[Route("api/Communication/ParafaitFunctions")]
        //[Authorize]
        //public async Task<HttpResponseMessage> Post([FromBody] List<ParafaitFunctionsDTO> parafaitFunctionsDTOList)
        //{
        //    try
        //    {
        //        ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
        //        IParafaitFunctionsUseCases parafaitFunctionsUseCases = CommunicationUseCaseFactory.GetParafaitFunctionsUseCases(executionContext);
        //        //ParafaitFunctionsListBL parafaitFunctionsListBL = new ParafaitFunctionsListBL(executionContext, parafaitFunctionsDTOList);
        //        var content = await parafaitFunctionsUseCases.SaveParafaitFunctions(parafaitFunctionsDTOList);
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