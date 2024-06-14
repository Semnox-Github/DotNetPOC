/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Company and Org Structure and Organization details list
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-Mar-2019   Jagan Mohana         Created 
               29-Mar-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Site;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.SiteSetup
{
    public class CompanyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object Company and Org Structure and Organization List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/Company/")]
        public HttpResponseMessage Get()
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                bool isHqSite = false;
                if(Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]))
                {
                    isHqSite = true;
                }
                CompanyList companyList = new CompanyList(executionContext);
                List<KeyValuePair<CompanyDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CompanyDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CompanyDTO.SearchByParameters, string>(CompanyDTO.SearchByParameters.MASTER_SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                var content = companyList.GetAllCompanies(searchParameters, isHqSite);
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
        /// Performs a Post operation on companyDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/Company/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<CompanyDTO> companyDTOList)
        {
            try
            {
                log.LogMethodEntry(companyDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (companyDTOList != null)
                {
                    // if companyDTOList.CompanyId is less than zero then insert or else update
                    CompanyList countryDTOList = new CompanyList(executionContext, companyDTOList);
                    countryDTOList.SaveUpdateCompanyList();
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