/********************************************************************************************
 * Project Name - POS
 * Description  - Controller for POSMachineType.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By                Remarks          
 *********************************************************************************************
 *2.60       23-Jan-2019    Nagesh Badiger          Created to get, insert, update and Delete Methods.
 **********************************************************************************************
 *2.60       20-Mar-2019    Akshay Gulaganji        Added CustomGenericException and ExecutionContext
 *2.80       26-Feb-2020    Vikas Dwivedi           Renamed POSManagementCountersController to POSMachineTypeController and Modified as per the RestAPI Phase 1 Changes.
 *********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;

namespace Semnox.CommonAPI.POS
{
    public class POSMachineTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        /// <summary>
        /// Get the JSON Object of POSTypeDTO.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/POS/POSMachineTypes")]
        public HttpResponseMessage Get(string isActive = null)
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
                List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));

                POSTypeListBL posTypeListBL = new POSTypeListBL(executionContext);
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                var content = posTypeListBL.GetPOSTypeDTOList(searchParameters);

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
        /// Post the JSON Object Of POSTypeDTO List
        /// </summary>
        /// <param name="posTypeDTOList">POSTypeListBL</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/POS/POSMachineTypes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<POSTypeDTO> posTypeDTOList, bool isLicensedPOSMachines = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(posTypeDTOList, isLicensedPOSMachines);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (posTypeDTOList != null && posTypeDTOList.Any())
                {
                    POSTypeListBL poSTypeBL = new POSTypeListBL(executionContext, posTypeDTOList);
                    poSTypeBL.Save(isLicensedPOSMachines);
                    log.LogMethodExit(posTypeDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (POSMachinesLicenseException posex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(posex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Accepted, new { data = customException });
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
        /// Delete the POSTypeDTO Record
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/POS/POSMachineTypes")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<POSTypeDTO> posTypeDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(posTypeDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (posTypeDTOList != null && posTypeDTOList.Any())
                {
                    POSTypeListBL poSTypeBL = new POSTypeListBL(executionContext, posTypeDTOList);
                    poSTypeBL.DeletePOSTypesList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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
