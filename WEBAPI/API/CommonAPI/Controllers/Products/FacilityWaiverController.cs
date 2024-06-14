/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Facility Waiver
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80        02-Dec-2019   Jagan          Created 
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
    public class FacilityWaiverController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object Facility Waiver List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/FacilityWaiver/")]
        public HttpResponseMessage Get(string isActive, int facilityId = -1)
        {
            try
            {
                log.LogMethodEntry(isActive, facilityId);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                FacilityWaiverListBL facilityWaiverListBL = new FacilityWaiverListBL(executionContext);
                List<KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>>();

                searchParameters.Add(new KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>(FacilityWaiverDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>(FacilityWaiverDTO.SearchByParameters.IS_ACTIVE, isActive));
                }
                if (facilityId > 0)
                {
                    searchParameters.Add(new KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>(FacilityWaiverDTO.SearchByParameters.FACILITY_ID, facilityId.ToString()));
                }
                var content = facilityWaiverListBL.GetAllFacilityWaiverList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on FacilityWaiverDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Products/FacilityWaiver/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<FacilityWaiverDTO> facilityWaiverDTOList)
        {
            try
            {
                log.LogMethodEntry(facilityWaiverDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (facilityWaiverDTOList != null)
                {
                    // if FacilityWaiverDTOList.Id is less than zero then insert or else update
                    FacilityWaiverListBL facilityWaiverListBL = new FacilityWaiverListBL(executionContext, facilityWaiverDTOList);
                    facilityWaiverListBL.SaveFacilityWaiver();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
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

        /// <summary>
        /// Performs a Delete operation on FacilityWaiverDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Products/FacilityWaiver/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<FacilityWaiverDTO> facilityWaiverDTOList)
        {
            try
            {
                log.LogMethodEntry(facilityWaiverDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (facilityWaiverDTOList != null)
                {
                    // if FacilityWaiverDTOList.Id is less than zero then insert or else update
                    FacilityWaiverListBL facilityWaiverListBL = new FacilityWaiverListBL(executionContext, facilityWaiverDTOList);
                    facilityWaiverListBL.SaveFacilityWaiver();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
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