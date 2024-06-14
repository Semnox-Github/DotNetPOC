/********************************************************************************************
 * Project Name - Promotions
 * Description  - Controller for the Promotions class.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.70.0      18-Jul-2019     Mushahid Faizan    Created 
 *2.80        31-Mar-2020     Mushahid Faizan    Modified as per the Rest API Phase 1 changes
 *                                               Renamed Controller from PromotionCalendarController to PromotionController
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;

namespace Semnox.CommonAPI.Promotion
{
    public class PromotionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Promotions Calendar Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Promotion/Promotions")]
        public HttpResponseMessage Get(string isActive = null, bool loadActiveChild = false, bool buildChildRecords = false,
                                       string promotionName = null, DateTime? fromDate = null, DateTime? toDate = null, int promotionId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, loadActiveChild, buildChildRecords, promotionName, fromDate, toDate, promotionId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                List<KeyValuePair<PromotionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PromotionDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PromotionDTO.SearchByParameters, string>(PromotionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<PromotionDTO.SearchByParameters, string>(PromotionDTO.SearchByParameters.ACTIVE_FLAG, isActive));
                    }
                }
                DateTime startDate = serverTimeObject.GetServerDateTime();
                DateTime endDate = serverTimeObject.GetServerDateTime().AddDays(1);

                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                else
                {
                    endDate = serverTimeObject.GetServerDateTime();
                }

                if (fromDate != null || toDate != null)
                {
                    searchParameters.Add(new KeyValuePair<PromotionDTO.SearchByParameters, string>(PromotionDTO.SearchByParameters.TIME_FROM, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<PromotionDTO.SearchByParameters, string>(PromotionDTO.SearchByParameters.TIME_TO, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (!string.IsNullOrEmpty(promotionName))
                {
                    searchParameters.Add(new KeyValuePair<PromotionDTO.SearchByParameters, string>(PromotionDTO.SearchByParameters.PROMOTION_NAME, promotionName.ToString()));
                }
                if (promotionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<PromotionDTO.SearchByParameters, string>(PromotionDTO.SearchByParameters.PROMOTION_ID, promotionId.ToString()));
                }
                PromotionListBL promotionListBL = new PromotionListBL(executionContext);
                var content = promotionListBL.GetPromotionDTOList(searchParameters, buildChildRecords, loadActiveChild, null);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }

            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on PromotionDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Promotion/Promotions")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<PromotionDTO> promotionDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(promotionDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (promotionDTOList != null && promotionDTOList.Any())
                {
                    PromotionListBL promotionListBL = new PromotionListBL(executionContext, promotionDTOList);
                    promotionListBL.Save();
                    log.LogMethodExit(promotionDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = promotionDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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
        /// Performs a Delete operation on PromotionDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Promotion/Promotions")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<PromotionDTO> promotionDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(promotionDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (promotionDTOList != null && promotionDTOList.Any())
                {
                    PromotionListBL promotionListBL = new PromotionListBL(executionContext, promotionDTOList);
                    promotionListBL.Delete();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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
