///********************************************************************************************
// * Project Name - Products
// * Description  - Created to fetch, update and insert SetupSegmentDefinitionMapping in the product entity.   
// *  
// **************
// **Version Log
// ************** 
// *Version     Date          Modified By              Remarks          
// *********************************************************************************************
// *2.50        10-Jan-2019   Muhammed Mehraj          Created to get, insert, update and Delete Methods.
// *2.60        21-Mar-2019   Nagesh Badiger           Added ExecutionContext and added Custom Generic Exception 
// ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Products
{
    public class SegmentDefinitionMappingController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;

        /// <summary>
        /// Get the JSON Setup SegmentDefinition Mapping
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Products/SegmentDefinitionMapping/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive, string applicability)
        {
            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>> searchBySegmentDefinitionSourceMapDTOSearchParams = new List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>>();
                if (!string.IsNullOrEmpty(applicability))
                {
                    searchBySegmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_APPLICABILITY, applicability));
                }
                searchBySegmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));
                if (isActive == "1")
                {
                    searchBySegmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.IS_ACTIVE, isActive));
                }
                bool loadChild = true;
                SegmentDefinitionSourceMapList segmentDefinition = new SegmentDefinitionSourceMapList(executionContext);
                var content = segmentDefinition.GetAllSegmentDefinitionSourceMaps(searchBySegmentDefinitionSourceMapDTOSearchParams,loadChild);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Post the JSON Object Setup Segment Definition Map
        /// </summary>
        /// <param name="segmentDefinitionSourceMapDTO">segmentDefinitionList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Products/SegmentDefinitionMapping/")]
        [Authorize]
        public HttpResponseMessage Post(List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTOList)
        {
            try
            {
                log.LogMethodEntry(segmentDefinitionSourceMapDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (segmentDefinitionSourceMapDTOList != null || segmentDefinitionSourceMapDTOList.Count != 0)
                {
                    SegmentDefinitionSourceMapList segmentDefinition = new SegmentDefinitionSourceMapList(executionContext, segmentDefinitionSourceMapDTOList);
                   // segmentDefinition.SaveUpdateSegmentDefinationMappingList();
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
        /// Delete the JSON Object Setup Segment Definition Map
        /// </summary>
        /// <param name="segmentDefinitionSourceMapDTO">segmentDefinitionList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Products/SegmentDefinitionMapping/")]
        [Authorize]
        public HttpResponseMessage Delete(List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTOList)
        {            
            try
            {
                log.LogMethodEntry(segmentDefinitionSourceMapDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (segmentDefinitionSourceMapDTOList != null || segmentDefinitionSourceMapDTOList.Count != 0)
                {
                    SegmentDefinitionSourceMapList segmentDefinition = new SegmentDefinitionSourceMapList(executionContext, segmentDefinitionSourceMapDTOList);
                   // segmentDefinition.Save();
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
