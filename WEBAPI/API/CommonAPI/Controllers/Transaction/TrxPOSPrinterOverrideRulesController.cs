/********************************************************************************************
 * Project Name - Transaction
 * Description  - TrxPOSPrinterOverrideRulesController class
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
  2.110       05-Jan-2021      Dakshakh Raj              Created : Peru Invoice changes
 ********************************************************************************************/
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
using Semnox.Parafait.Transaction;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TrxPOSPrinterOverrideRulesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON POS Printer Override Rules.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/TrxPOSPrinterOverrideRules")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int pOSPrinterId = -1, int transactionId = -1, int trxPosPrinterOverrideRuleId = -1, int pOSPrinterOverrideRuleId = -1, int pOSPrinterOverrideOptionId = -1, string optionItemCode = null)
        {
            log.LogMethodEntry(isActive, pOSPrinterId, transactionId, trxPosPrinterOverrideRuleId, pOSPrinterOverrideRuleId, pOSPrinterOverrideOptionId, optionItemCode);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrWhiteSpace(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (string.IsNullOrWhiteSpace(optionItemCode) == false)
                {
                    searchParameters.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.OPTION_ITEM_CODE, optionItemCode.ToString()));
                }
                if (transactionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                }
                if (trxPosPrinterOverrideRuleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.TRX_POS_PRINTER_OVERRIDE_RULE_ID, trxPosPrinterOverrideRuleId.ToString()));
                }
                if (pOSPrinterId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID, pOSPrinterId.ToString()));
                }
                if (pOSPrinterOverrideRuleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINETR_OVERRIDE_RULE_ID, pOSPrinterOverrideRuleId.ToString()));
                }
                if (pOSPrinterOverrideOptionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINETR_OVERRIDE_OPTION_ID, pOSPrinterOverrideOptionId.ToString()));
                }
                ITrxPOSPrinterOverrideRulesUseCases trxPOSPrinterOverrideRulesUseCases = TransactionUseCaseFactory.GetTrxPOSPrinterOverrideRulesUseCases(executionContext);
                List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList = await trxPOSPrinterOverrideRulesUseCases.GetTrxPOSPrinterOverrideRules(searchParameters, null);

                log.LogMethodExit(trxPOSPrinterOverrideRulesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = trxPOSPrinterOverrideRulesDTOList });
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
        /// <param name="">trxPOSPrinterOverrideRulesDTOList</param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/Transaction/TrxPOSPrinterOverrideRules")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRulesDTOList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (trxPOSPrinterOverrideRulesDTOList == null || trxPOSPrinterOverrideRulesDTOList.Any(a => a.TrxPOSPrinterOverrideRuleId > 0))
                {
                    log.LogMethodExit(trxPOSPrinterOverrideRulesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });

                }
                ITrxPOSPrinterOverrideRulesUseCases trxPOSPrinterOverrideRulesUseCases = TransactionUseCaseFactory.GetTrxPOSPrinterOverrideRulesUseCases(executionContext);
                trxPOSPrinterOverrideRulesUseCases.SaveTrxPOSPrinterOverrideRules(trxPOSPrinterOverrideRulesDTOList);

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

        /// <summary>
        /// Post the JSON Object Trx POS Printer Override Rules
        /// </summary>
        /// <param name="trxPOSPrinterOverrideRulesDTOList">trxPOSPrinterOverrideRulesDTOList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Route("api/Transaction/TrxPOSPrinterOverrideRules")]
        [Authorize]
        public HttpResponseMessage Put([FromBody]List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRulesDTOList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (trxPOSPrinterOverrideRulesDTOList == null || trxPOSPrinterOverrideRulesDTOList.Any(a => a.TrxPOSPrinterOverrideRuleId < 0))
                {
                    log.LogMethodExit(trxPOSPrinterOverrideRulesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                ITrxPOSPrinterOverrideRulesUseCases trxPOSPrinterOverrideRulesUseCases = TransactionUseCaseFactory.GetTrxPOSPrinterOverrideRulesUseCases(executionContext);
                trxPOSPrinterOverrideRulesUseCases.SaveTrxPOSPrinterOverrideRules(trxPOSPrinterOverrideRulesDTOList);

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