/********************************************************************************************
 * Project Name - POS
 * Description  - Created to fetch, update and insert in the POS Printer Override Rules .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    30-Dec-2020   Dakshakh Raj             Created as part of Peru Invoice changes
 ***************************************************************************************************/
using System;
using System.Net;
using System.Web;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Semnox.Parafait.POS;
using Semnox.Core.Utilities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.POS
{
    public class POSPrinterOverrideRulesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON POS Printer Override Rules.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/POS/POSPrinterOverrideRules")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int pOSPrinterId = -1, int pOSPrinterOverrideRuleId = -1, int pOSPrinterOverrideOptionId = -1, string optionItemCode = null, int printerId = -1)
        {
            log.LogMethodEntry(isActive, pOSPrinterId, pOSPrinterOverrideRuleId, pOSPrinterOverrideOptionId, optionItemCode, printerId);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrWhiteSpace(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrWhiteSpace(optionItemCode))
                {
                    searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.OPTION_ITEM_CODE, optionItemCode.ToString()));
                }
                if (pOSPrinterId > -1)
                {
                    searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID, pOSPrinterId.ToString()));
                }
                if (pOSPrinterOverrideRuleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_RULE_ID, pOSPrinterOverrideRuleId.ToString()));
                }
                if (pOSPrinterOverrideOptionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID, pOSPrinterOverrideOptionId.ToString()));
                }
                if (printerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.PRINTER_ID, printerId.ToString()));
                }
                IPOSPrinterOverrideRulesUseCases pOSPrinterOverrideRulesUseCases = POSUseCaseFactory.GetPOSPrinterOverrideRulesUseCases(executionContext);
                List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList = await pOSPrinterOverrideRulesUseCases.GetPOSPrinterOverrideRules(searchParameters);

                log.LogMethodExit(pOSPrinterOverrideRulesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = pOSPrinterOverrideRulesDTOList });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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

        /// <summary>
        /// Post the JSON Object POS Printer Override Rules
        /// </summary>
        /// <param name="">pOSPrinterOverrideRulesDTOList</param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/POS/POSPrinterOverrideRules")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList)
        {
            log.LogMethodEntry(pOSPrinterOverrideRulesDTOList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (pOSPrinterOverrideRulesDTOList == null || pOSPrinterOverrideRulesDTOList.Any(a => a.POSPrinterOverrideRuleId > 0))
                {
                    log.LogMethodExit(pOSPrinterOverrideRulesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });

                }
                IPOSPrinterOverrideRulesUseCases pOSPrinterOverrideRulesUseCases = POSUseCaseFactory.GetPOSPrinterOverrideRulesUseCases(executionContext);
                var content = await pOSPrinterOverrideRulesUseCases.SavePOSPrinterOverrideRules(pOSPrinterOverrideRulesDTOList);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = content
                });

                //return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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

        /// <summary>
        /// Post the JSON Object POS Printer Override Rules
        /// </summary>
        /// <param name="pOSPrinterOverrideRulesDTOList">pOSPrinterOverrideRulesDTOList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Route("api/POS/POSPrinterOverrideRules")]
        [Authorize]
        public HttpResponseMessage Put([FromBody]List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList)
        {
            log.LogMethodEntry(pOSPrinterOverrideRulesDTOList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (pOSPrinterOverrideRulesDTOList == null || pOSPrinterOverrideRulesDTOList.Any(a => a.POSPrinterOverrideRuleId < 0))
                {
                    log.LogMethodExit(pOSPrinterOverrideRulesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IPOSPrinterOverrideRulesUseCases pOSPrinterOverrideRulesUseCases = POSUseCaseFactory.GetPOSPrinterOverrideRulesUseCases(executionContext);
                pOSPrinterOverrideRulesUseCases.SavePOSPrinterOverrideRules(pOSPrinterOverrideRulesDTOList);

                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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