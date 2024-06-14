/********************************************************************************************
 * Project Name - Products Controller
 * Description  - Created to fetch, update and insert segementdefinationsourcevalues in the product entity.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        21-Jan-2019    Muhammed Mehraj          Created to get, insert, update and Delete Methods.
 *2.60        17-Mar-2019    Manoj Durgam             Added ExecutionContext to the constructor
 *2.60        21-Mar-2019    Nagesh Badiger          Added ExecutionContext and added Custom Generic Exception 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Semnox.CommonAPI.Products
{
    public class SegmentDefinationSourceValueController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;

        /// <summary>
        /// Get SegmentDefinationvalue list
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Products/SegmentDefinationSourceValue/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive, int sourceId)
        {            
            try
            {
                log.LogMethodEntry(isActive, sourceId);                
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                SegmentDefinitionSourceValueList segmentDefinitionSourceValueList = new SegmentDefinitionSourceValueList(executionContext);
                List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList = new List<SegmentDefinitionSourceValueDTO>();
                List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> segmentDefinitionSourceValueDTOSearchParams = new List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>>();
                if (sourceId != 0)
                {
                    segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_ID, sourceId.ToString()));
                }
                segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));
                if (isActive == "1")
                {
                    segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.IS_ACTIVE, isActive));
                }
                segmentDefinitionSourceValueDTOList = segmentDefinitionSourceValueList.GetAllSegmentDefinitionSourceValues(segmentDefinitionSourceValueDTOSearchParams);
                log.LogMethodExit(segmentDefinitionSourceValueDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = segmentDefinitionSourceValueDTOList, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }

        }
        /// <summary>
        /// Post segmentDefinitionSourceValues Record
        /// </summary>
        /// <param name="segmentDefinitionSourceValuesList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Products/SegmentDefinationSourceValue/")]
        [Authorize]
        public HttpResponseMessage Post(List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValuesList)
        {
            try
            {
                log.LogMethodEntry(segmentDefinitionSourceValuesList);                
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (segmentDefinitionSourceValuesList != null || segmentDefinitionSourceValuesList.Count != 0)
                {
                    SegmentDefinitionSourceValueList segmentDefinitionSourceValue = new SegmentDefinitionSourceValueList(executionContext, segmentDefinitionSourceValuesList);
                 //   segmentDefinitionSourceValue.SaveUpdateSegmentDefinationValueList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Delete segmentDefinitionSourceValues Record
        /// </summary>
        /// <param name="segmentDefinitionSourceValues"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Products/SegmentDefinationSourceValue/")]
        [Authorize]
        public HttpResponseMessage Delete(List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValuesList)
        {
            try
            {
                log.LogMethodEntry(segmentDefinitionSourceValuesList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (segmentDefinitionSourceValuesList != null || segmentDefinitionSourceValuesList.Count != 0)
                {
                    SegmentDefinitionSourceValueList segmentDefinitionSourceValue = new SegmentDefinitionSourceValueList(executionContext, segmentDefinitionSourceValuesList);
                   // segmentDefinitionSourceValue.SaveUpdateSegmentDefinationValueList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
