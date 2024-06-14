/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller for the Table Layout
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         14-May-2019  Mushahid Faizan         Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.SiteSetup
{
    public class TableLayoutController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object 
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/TableLayout/")]
        public HttpResponseMessage Get(string isActive,string facilityId)
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<FacilityTableDTO.SearchByParameters, string>(FacilityTableDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                searchParameters.Add(new KeyValuePair<FacilityTableDTO.SearchByParameters, string>(FacilityTableDTO.SearchByParameters.FACILITY_ID, facilityId));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<FacilityTableDTO.SearchByParameters, string>(FacilityTableDTO.SearchByParameters.ISACTIVE, isActive));
                }
                FacilityTablesList facilityTables = new FacilityTablesList(executionContext);
                var content = facilityTables.GetAllFacilityTableList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on TableLayout details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/TableLayout/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<FacilityTableDTO> facilityTableDTOList)
        {
            try
            {
                log.LogMethodEntry(facilityTableDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (facilityTableDTOList != null)
                {
                    // if facilityTableDTOList.TableId is less than zero then insert or else update
                    FacilityTablesList facilityTablesList = new FacilityTablesList(executionContext, facilityTableDTOList);
                    facilityTablesList.SaveUpdateFacilityTablesList();
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
        /// Performs a Delete operation on TableLayout details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/TableLayout/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<FacilityTableDTO> facilityTableDTOList)
        {
            try
            {
                log.LogMethodEntry(facilityTableDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (facilityTableDTOList != null)
                {
                    FacilityTablesList facilityTablesList = new FacilityTablesList(executionContext, facilityTableDTOList);
                    facilityTablesList.SaveUpdateFacilityTablesList();
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
