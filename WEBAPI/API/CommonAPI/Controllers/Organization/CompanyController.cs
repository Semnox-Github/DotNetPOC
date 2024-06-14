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
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;


namespace Semnox.CommonAPI.Organization
{
    public class CompanyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        /// <summary>
        /// Get the JSON Object Company and Org Structure and Organization List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Organization/Companies")]
        public HttpResponseMessage Get()
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
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
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content  });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message  });
            }
        }

        /// <summary>
        /// Performs a Post operation on companyDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Organization/Companies")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<CompanyDTO> companyDTOList)
        {
            
                log.LogMethodEntry(companyDTOList);
                SecurityTokenDTO securityTokenDTO = null;
                ExecutionContext executionContext = null;
                try
                {
                    SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                    securityTokenBL.GenerateJWTToken();
                    securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                    executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                    if (companyDTOList != null)
                {
                    // if companyDTOList.CompanyId is less than zero then insert or else update
                    CompanyList countryDTOList = new CompanyList(executionContext, companyDTOList);
                    countryDTOList.SaveUpdateCompanyList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }
    }
}