/********************************************************************************************
 * Project Name - Promotions
 * Description  - Controller for Campaigns class.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.80.0      26-Mar-2020     Mushahid Faizan    Created 
 *2.100.0     15-Sep-2020     Nitin Pai          Modified for Push Notification
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Customer.Campaign
{
    public class CampaignController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the JSON Object campaignDTOList List
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/Customer/Campaigns")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, bool loadActiveChild = false, string communicationMode = null, bool buildChildRecords = false, int campaignId = -1, bool buildCustomersInCampaign = false)
        {
            try
            {
                log.LogMethodEntry(isActive, loadActiveChild, buildChildRecords);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                int totalNoOfSMS = 0;
                int totalNoOfEmails = 0;
                int totalNoOfNotifications = 0;

                List<KeyValuePair<CampaignDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CampaignDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CampaignDTO.SearchByParameters, string>(CampaignDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (!string.IsNullOrEmpty(isActive.ToString()))
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<CampaignDTO.SearchByParameters, string>(CampaignDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(communicationMode))
                {
                    searchParameters.Add(new KeyValuePair<CampaignDTO.SearchByParameters, string>(CampaignDTO.SearchByParameters.COMMUNICATION_MODE, communicationMode.ToString()));
                }
                if (campaignId > -1)
                {
                    List<KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>>();
                    searchParameter.Add(new KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>(CampaignCustomerDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParameter.Add(new KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>(CampaignCustomerDTO.SearchByParameters.CAMPAIGN_ID, campaignId.ToString()));
                    CampaignCustomerListBL campaignCustomerListBL = new CampaignCustomerListBL(executionContext);

                    totalNoOfSMS = campaignCustomerListBL.GetPhoneCount(Convert.ToInt32(campaignId));
                    totalNoOfEmails = campaignCustomerListBL.GetEmailCount(Convert.ToInt32(campaignId));
                    totalNoOfNotifications = campaignCustomerListBL.GetNotificationCount(Convert.ToInt32(campaignId));
                }
                CampaignBL.CampaignListBL campaignListBL = new CampaignBL.CampaignListBL(executionContext);
                List<CampaignDTO> campaignDTOList = campaignListBL.GetCampaignDTOList(searchParameters, buildChildRecords, loadActiveChild, null);

                if (buildCustomersInCampaign)
                {
                    if (campaignDTOList != null && campaignDTOList.Any())
                    {
                        campaignDTOList.ForEach(campaignDTO =>
                        {
                            CampaignCustomerListBL campaignCustomerListBL = new CampaignCustomerListBL(executionContext);
                            campaignDTO.CampaignCustomerDTOList = campaignCustomerListBL.GetCustomersInCampaign(campaignDTO.CampaignId);
                        });
                    }
                }

                log.LogMethodExit(campaignDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = campaignDTOList, CustomerWithPhoneNos = totalNoOfSMS, CustomerWithEmail = totalNoOfEmails, CustomersWithApp = totalNoOfNotifications });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on campaignDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Customer/Campaigns")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<CampaignDTO> campaignDTOList)
        {
            try
            {
                log.LogMethodEntry(campaignDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (campaignDTOList != null && campaignDTOList.Any())
                {
                    CampaignBL.CampaignListBL campaignListBL = new CampaignBL.CampaignListBL(executionContext, campaignDTOList);
                    campaignListBL.Save();
                    log.LogMethodExit(campaignDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = campaignDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
