/********************************************************************************************
 * Project Name - Products Controller
 * Description  - Created to fetch, update and insert segment definition in the product module.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.60        22-Jan-2019   Akshay Gulaganji    Created to get, insert, update and Delete Methods.
 *2.60        17-Mar-2019   Manoj Durgam        Added ExecutionContext to the constructor
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    public class SegmentDefinitionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;

        /// <summary>
        /// Get the JSON Segment Definations.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/SegmentDefinition/")]
        public HttpResponseMessage Get(string isActive, string applicableEntity)
        {
            try
            {
                log.LogMethodEntry(isActive, applicableEntity);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParameters = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (!string.IsNullOrEmpty(applicableEntity))
                {
                    searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, applicableEntity));
                }
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, isActive));
                }
                SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(executionContext);
                var content = segmentDefinitionList.GetAllSegmentDefinitions(searchParameters);
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
        /// Post the JSON Object Segment Definition
        /// </summary>
        /// <param name="segmentDefinitionList">segmentDefinitionList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Products/SegmentDefinition/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<SegmentDefinitionDTO> segmentDefinitionList)
        {
            try
            {
                log.LogMethodEntry(segmentDefinitionList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (segmentDefinitionList != null || segmentDefinitionList.Count != 0)
                {
                    SegmentDefinitionList segmentDefinition = new SegmentDefinitionList(executionContext, segmentDefinitionList);
                    //segmentDefinition.SaveUpdatesegmentDefinitionList();
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
        /// Delete the Segment Definitions
        /// </summary> 
        /// <param name="segmentDefinitionList">segmentDefinitionList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Products/SegmentDefinition/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<SegmentDefinitionDTO> segmentDefinitionList)
        {
            try
            {
                log.LogMethodEntry(segmentDefinitionList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (segmentDefinitionList != null || segmentDefinitionList.Count != 0)
                {
                    SegmentDefinitionList segmentDefinition = new SegmentDefinitionList(executionContext, segmentDefinitionList);
                  //  segmentDefinition.SaveUpdatesegmentDefinitionList();
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
