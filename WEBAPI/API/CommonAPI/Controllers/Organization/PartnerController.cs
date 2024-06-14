/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert partners
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.60        06-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
*2.90        21-May-2020   Mushahid Faizan           Modified :As per Rest API standard, Added SearchParams and Renamed controller from PartnerSetupController to PartnerController
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Organization
{
    public class PartnerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Partner
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Organization/Partners")]
        public HttpResponseMessage Get(string isActive = null, int partnerId = -1, int customerId = -1, bool loadActiveChild = false, bool buildChildRecords = false)
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

                PartnersList partnersList = new PartnersList(executionContext);
                List<KeyValuePair<PartnersDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PartnersDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PartnersDTO.SearchByParameters, string>(PartnersDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (partnerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<PartnersDTO.SearchByParameters, string>(PartnersDTO.SearchByParameters.PARTNER_ID, partnerId.ToString()));
                }
                if (customerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<PartnersDTO.SearchByParameters, string>(PartnersDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<PartnersDTO.SearchByParameters, string>(PartnersDTO.SearchByParameters.ACTIVE, isActive.ToString()));
                    }
                }
                List<PartnersDTO> content = partnersList.GetAllPartnersDTOList(searchParameters, buildChildRecords, loadActiveChild);
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
        /// Post the JSON Partners
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Organization/Partners")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<PartnersDTO> partnersDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(partnersDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (partnersDTOList != null && partnersDTOList.Any())
                {
                    PartnersList partnersList = new PartnersList(executionContext, partnersDTOList);
                    partnersList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = partnersDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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