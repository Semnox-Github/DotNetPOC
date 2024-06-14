/********************************************************************************************
 * Project Name - Product Controller
 * Description  - Created to fetch, update and insert Segments.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        24-Jan-2019   Muhammed Mehraj          Created 
*2.110.0       21-Nov-2019   Girish Kundar       Modified :  REST API changes for Inventory UI redesign
*2.120.0       23-Mar-2021   Mushahid Faizan       Added FromBody tag in Post method
*2.140.0      11-Jan-2022    Abhishek            Modified:Post method to return the response
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Controllers.Products
{
    public class ProductSegmentsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Segemtvalues and create custom controls for UI
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/Product/ProductSegments")]
        [Authorize]
        public HttpResponseMessage Get(string applicability, string segmentCategoryId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(applicability, segmentCategoryId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                var segmentDefinitionDTOSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                segmentDefinitionDTOSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "Y"));
                segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, applicability));
                SegmentCategorizationValueWrapperBL productSegment = new SegmentCategorizationValueWrapperBL(executionContext);
                var content = productSegment.GetSegmentValues(segmentDefinitionDTOSearchParams, applicability, segmentCategoryId);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content  });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
        /// Post segmentvalue collection with productid and applicability
        /// segmentvalue collection will be further populated into SegementCategorizationDTO
        /// </summary>
        /// <param name="segmentCategorizationsList"></param>
        /// <param name="productId"></param>
        /// <param name="applicablity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Product/ProductSegments")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<SegmentCategorizationValueWrapperDTO> segmentCategorizationsList,[FromUri] string productId, [FromUri] string applicablity)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(segmentCategorizationsList, productId, applicablity);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                List<SegmentCategorizationValueDTO> segmentCategorizationValueDTOList = new List<SegmentCategorizationValueDTO>();
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (segmentCategorizationsList != null && segmentCategorizationsList.Any())
                {
                    SegmentCategorizationValueWrapperBL categorizationValueCustomAttrBL = new SegmentCategorizationValueWrapperBL(executionContext);
                    segmentCategorizationValueDTOList = categorizationValueCustomAttrBL.SaveUpdateSegmentValueList(segmentCategorizationsList, Convert.ToInt32(productId), applicablity);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }

            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
