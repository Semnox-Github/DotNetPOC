/********************************************************************************************
 * Project Name - POS
 * Description  - Created to fetch, update and insert in the POS Printer Override Options .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    30-Dec-2020   Dakshakh Raj             Created as part of Peru Invoice changes
 ***************************************************************************************************/
using System;
using System.Web;
using System.Net;
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
    public class POSPrinterOverrideOptionsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON POS Printer Override Rules.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/POS/POSPrinterOverrideOptions")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int pOSPrinterOverrideOptionId = -1, string optionName = null, string fiscalizationType = null, string doImmediatePost = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, pOSPrinterOverrideOptionId, optionName, fiscalizationType, doImmediatePost);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>(POSPrinterOverrideOptionsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>(POSPrinterOverrideOptionsDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(optionName))
                {
                    searchParameters.Add(new KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>(POSPrinterOverrideOptionsDTO.SearchByParameters.OPTION_NAME, optionName.ToString()));
                }
                if (pOSPrinterOverrideOptionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>(POSPrinterOverrideOptionsDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID, pOSPrinterOverrideOptionId.ToString()));
                }
                if (!string.IsNullOrEmpty(fiscalizationType))
                {
                    searchParameters.Add(new KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>(POSPrinterOverrideOptionsDTO.SearchByParameters.FISCALIZATION_TYPE, fiscalizationType.ToString()));
                }
                if (string.IsNullOrEmpty(doImmediatePost) == false)
                {
                    if (doImmediatePost.ToString() == "1" || doImmediatePost.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>(POSPrinterOverrideOptionsDTO.SearchByParameters.DO_IMMEDIATE_POST, "1"));
                    }
                    else if (doImmediatePost.ToString() == "0" || doImmediatePost.ToString() == "N")
                    {
                        searchParameters.Add(new KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>(POSPrinterOverrideOptionsDTO.SearchByParameters.DO_IMMEDIATE_POST, "0"));
                    }
                }
                IPOSPrinterOverrideOptionsUseCases pOSPrinterOverrideOptionsUseCases = POSUseCaseFactory.GetPOSPrinterOverrideOptionsUseCases(executionContext);
                List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList = await pOSPrinterOverrideOptionsUseCases.GetPOSPrinterOverrideOptions(searchParameters, null);

                log.LogMethodExit(pOSPrinterOverrideOptionsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = pOSPrinterOverrideOptionsDTOList });
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
        /// Post the JSON Object POS Printer Override Options
        /// </summary>
        /// <param name="">pOSPrinterOverrideOptionsDTOList</param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/POS/POSPrinterOverrideOptions")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(pOSPrinterOverrideOptionsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (pOSPrinterOverrideOptionsDTOList == null || pOSPrinterOverrideOptionsDTOList.Any(a => a.POSPrinterOverrideOptionId > 0))
                {
                    log.LogMethodExit(pOSPrinterOverrideOptionsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });

                }
                IPOSPrinterOverrideOptionsUseCases pOSPrinterOverrideOptionsUseCases = POSUseCaseFactory.GetPOSPrinterOverrideOptionsUseCases(executionContext);
                await pOSPrinterOverrideOptionsUseCases.SavePOSPrinterOverrideOptions(pOSPrinterOverrideOptionsDTOList);

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
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, new
            //    {
            //        data = ExceptionSerializer.Serialize(ex)
            //    });
            //}
        }

        /// <summary>
        /// Post the JSON Object POS Printer Override Options
        /// </summary>
        /// <param name="pOSPrinterOverrideRulesDTOList">pOSPrinterOverrideOptionsDTOList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Route("api/POS/POSPrinterOverrideOptions")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(pOSPrinterOverrideOptionsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (pOSPrinterOverrideOptionsDTOList == null || pOSPrinterOverrideOptionsDTOList.Any(a => a.POSPrinterOverrideOptionId < 0))
                {
                    log.LogMethodExit(pOSPrinterOverrideOptionsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IPOSPrinterOverrideOptionsUseCases pOSPrinterOverrideOptionsUseCases = POSUseCaseFactory.GetPOSPrinterOverrideOptionsUseCases(executionContext);
                await pOSPrinterOverrideOptionsUseCases.SavePOSPrinterOverrideOptions(pOSPrinterOverrideOptionsDTOList);

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