/********************************************************************************************
 * Project Name - FacilityPOSAssignment API Controller
 * Description  - Created to Get, Post and Delete FacilityPOSAssignment for Products-> Set Up -> Facility --> POS Assignment  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60        07-May-2019   Akshay Gulaganji         Created
 *2.70.0      20-Jun-2019   Nagesh Badiger           Modified Delete method.
 *2.110.0     21-Nov-2020   Girish Kundar            EndPoint changes. 
 ***************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;

namespace Semnox.CommonAPI.Products
{
    public class FacilityPOSAssignmentController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the FacilityPOSAssignment Collection based on isActive and facilityId.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/FacilityPOSAssignments")]
        public HttpResponseMessage Get(string isActive, string facilityId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, facilityId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>(FacilityPOSAssignmentDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>(FacilityPOSAssignmentDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                }
                if (!string.IsNullOrEmpty(facilityId))
                {
                    searchParameters.Add(new KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>(FacilityPOSAssignmentDTO.SearchByParameters.FACILITY_ID, facilityId));
                }
                FacilityPOSAssignmentList facilityPOSAssignmentList = new FacilityPOSAssignmentList(executionContext);
                List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList = facilityPOSAssignmentList.GetFacilityPOSAssignmentDTOList(searchParameters);
                log.LogMethodExit(facilityPOSAssignmentDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = facilityPOSAssignmentDTOList  });
            }
            catch (ValidationException valEx)
            {
                string validationException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(validationException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = validationException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Performs a Post operation on FacilityPOSAssignment
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Product/FacilityPOSAssignments")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(facilityPOSAssignmentDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (facilityPOSAssignmentDTOList != null && facilityPOSAssignmentDTOList.Any())
                {
                    FacilityPOSAssignmentList facilityPOSAssignmentList = new FacilityPOSAssignmentList(executionContext, facilityPOSAssignmentDTOList);
                    facilityPOSAssignmentList.SaveFacilityPOSAssignmentDTOList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
            }
            catch (ValidationException valEx)
            {
                string validationException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(validationException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = validationException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Performs a Delete operation on FacilityPOSAssignment
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // Delete: api/Subscriber
        [HttpDelete]
        [Route("api/Product/FacilityPOSAssignments")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(facilityPOSAssignmentDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (facilityPOSAssignmentDTOList != null && facilityPOSAssignmentDTOList.Any())
                {
                    FacilityPOSAssignmentList facilityPOSAssignmentList = new FacilityPOSAssignmentList(executionContext, facilityPOSAssignmentDTOList);
                    facilityPOSAssignmentList.DeleteFacilityPOSAssignmentDTOList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
            }
            catch (ValidationException valEx)
            {
                string validationException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(validationException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = validationException });
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
