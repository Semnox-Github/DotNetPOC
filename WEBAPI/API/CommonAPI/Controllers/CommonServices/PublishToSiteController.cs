/********************************************************************************************
 * Project Name - Publish To site Controller                                                                         
 * Description  - Controller of the Game class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.50        12-Dec-2018    Jagan Mohana          Created 
 *2.90        12-Jun-2020    Girish Kundar         Modified :  Enhanced to support bulk publishing feature 
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Publish;
using Semnox.Parafait.Site;

namespace Semnox.CommonAPI.CommonServices
{
    public class PublishToSiteController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();

        /// <summary>
        /// Get the JSON Object Game Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/CommonServices/PublishToSites")]
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
                OrganizationList organizationList = new Semnox.Parafait.Site.OrganizationList();
                List<OrganizationDTO> rootOrgnizationDTOList = new List<OrganizationDTO>();
                rootOrgnizationDTOList = organizationList.GetRootOrganizationList();
                List<PublishToSiteDTO> publishToSiteList = new List<PublishToSiteDTO>();
                if (rootOrgnizationDTOList != null && rootOrgnizationDTOList.Any())
                {
                    foreach (var rootOrg in rootOrgnizationDTOList)
                    {
                        PublishToSite publishToSite = new PublishToSite(executionContext);
                        PublishToSiteDTO publishToSiteDTO = new PublishToSiteDTO();
                        publishToSiteDTO.Id = Convert.ToString(rootOrg.OrgId);
                        publishToSiteDTO.Name = rootOrg.OrgName;
                        publishToSiteDTO = publishToSite.Recursive(publishToSiteDTO);
                        publishToSiteList.Add(publishToSiteDTO);
                    }
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = publishToSiteList });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Get the JSON Object Game Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/CommonServices/PublishToSites")]
        public HttpResponseMessage Post(List<PublishToSiteDTO> publishToSiteDTOList)
        {
            log.LogMethodEntry(publishToSiteDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                if (publishToSiteDTOList != null && publishToSiteDTOList.Any())
                {
                    PublishToSite publishToSiteBL = new PublishToSite(executionContext, publishToSiteDTOList);
                    publishToSiteBL.Publishing();
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}
