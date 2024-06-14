/********************************************************************************************
 * Project Name - Facility Seat Layout Controller
 * Description  - Created to Get, Post and Deletes the FacilitySeatLayout entity.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.70        25-Feb-2019   Akshay Gulaganji          Created to Get, Post and Delete Methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Products
{
    /// <summary>
    /// API Controller for Facility Seat Layout
    /// </summary>
    public class FacilitySeatLayoutController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Gets the Facility Seat Layout Jason Object
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Products/FacilitySeatLayout/")]
        [Authorize]
        public HttpResponseMessage Get(string facilityId)
        {
            try
            {
                log.LogMethodEntry(facilityId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!string.IsNullOrEmpty(facilityId))
                {
                    searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.FACILITY_ID, facilityId));
                }

                FacilityList facilityList = new FacilityList(executionContext);
                var content = facilityList.GetFacilityDTOList(searchParameters,true,true);
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
        /// Posts the Jason Object of FacilityDTO List
        /// </summary>
        /// <param name="facilityDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Products/FacilitySeatLayout/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<FacilityDTO> facilityDTOList)
        {
            try
            {
                log.LogMethodEntry(facilityDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (facilityDTOList != null && facilityDTOList.Count > 0)
                {
                    FacilityList facilityListBL = new FacilityList(executionContext, facilityDTOList);
                    facilityListBL.SaveUpdateFacilityList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
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
