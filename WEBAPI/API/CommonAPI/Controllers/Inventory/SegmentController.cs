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
 *2.100.0     15-Oct-2020   Mushahid Faizan        Modified : Renamed Controller from SegmentDefinitionController to SegmentController, Changed end points,
 *                                                  Added search parameters in get, Removed Delete() and removed token from response body.
 *2.110.0    23-Nov-2020   Mushahid Faizan         Web Inventory UI resdesign changes with REST API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class SegmentController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Segment Definations.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Segments")]
        public async Task<HttpResponseMessage> Get(string isActive = null, string applicableEntity = null, int segmentDefinitionId = -1, string segmentName = null, string sequenceOrder = null,
                                                    string isMandatory = null, bool loadActiveChild = false, bool buildChildRecords = false, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParameters = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!string.IsNullOrEmpty(applicableEntity))
                {
                    searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, applicableEntity));
                }
                if (!string.IsNullOrEmpty(segmentName))
                {
                    searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEGMENT_NAME, segmentName));
                }
                if (!string.IsNullOrEmpty(sequenceOrder))
                {
                    searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEQUENCE_ORDER, sequenceOrder));
                }
                if (!string.IsNullOrEmpty(isMandatory))
                {
                    searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_MANDATORY, isMandatory));
                }
                if (segmentDefinitionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEGMENT_DEFINITION_ID, segmentDefinitionId.ToString()));
                }

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, isActive));
                    }
                }
                SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(executionContext);

                int totalNoOfPages = 0;
                int totalCount = await Task<int>.Factory.StartNew(() => { return segmentDefinitionList.GetSegmentDefinitionCount(searchParameters, null); });
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

                ISegmentDefinitionUseCases segmentDefinitionUseCases = InventoryUseCaseFactory.GetSegmentDefinitionUseCases(executionContext);
                List<SegmentDefinitionDTO> segmentDefinitionDTOList = await segmentDefinitionUseCases.GetSegmentDefinitions(searchParameters, buildChildRecords, loadActiveChild, currentPage, pageSize);
                log.LogMethodExit(segmentDefinitionDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = segmentDefinitionDTOList, currentPageNo = currentPage, TotalCount = totalCount });

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
        /// Post the JSON Object Segment Definition
        /// </summary>
        /// <param name="segmentDefinitionDTOList">segmentDefinitionList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Inventory/Segments")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<SegmentDefinitionDTO> segmentDefinitionDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(segmentDefinitionDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (segmentDefinitionDTOList == null || segmentDefinitionDTOList.Any(a => a.SegmentDefinitionId > 0))
                {
                    log.LogMethodExit(segmentDefinitionDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ISegmentDefinitionUseCases segmentDefinitionUseCases = InventoryUseCaseFactory.GetSegmentDefinitionUseCases(executionContext);
                await segmentDefinitionUseCases.SaveSegmentDefinitions(segmentDefinitionDTOList);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = segmentDefinitionDTOList });
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
        /// Post the JSON Object Segment Definition
        /// </summary>
        /// <param name="segmentDefinitionDTOList">segmentDefinitionList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Route("api/Inventory/Segments")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<SegmentDefinitionDTO> segmentDefinitionDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(segmentDefinitionDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (segmentDefinitionDTOList == null || segmentDefinitionDTOList.Any(a => a.SegmentDefinitionId < 0))
                {
                    log.LogMethodExit(segmentDefinitionDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                ISegmentDefinitionUseCases segmentDefinitionUseCases = InventoryUseCaseFactory.GetSegmentDefinitionUseCases(executionContext);
                await segmentDefinitionUseCases.SaveSegmentDefinitions(segmentDefinitionDTOList);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = segmentDefinitionDTOList });
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
