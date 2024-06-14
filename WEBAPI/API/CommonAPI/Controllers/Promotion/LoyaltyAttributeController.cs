/********************************************************************************************
 * Project Name - Promotions
 * Description  - Controller for LoyaltyAttribute class.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.80.0      24-Mar-2019     Mushahid Faizan    Created 
 *2.80.0      24-Apr-2020     Girish Kundar      Modified : Added the  purchaseApplicable & consumptionApplicable parameter to GET request
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Achievements;
using Semnox.Parafait.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Promotion
{
    public class LoyaltyAttributeController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// Get the JSON Object of Loyalty Attribute List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Promotion/LoyaltyAttributes")]
        public HttpResponseMessage Get(string isActive = null, int loyaltyAttributeId = -1, bool? purchaseApplicable = null,
                                       bool? consumptionApplicable = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, loyaltyAttributeId, purchaseApplicable, consumptionApplicable);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>(LoyaltyAttributesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>(LoyaltyAttributesDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (loyaltyAttributeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>(LoyaltyAttributesDTO.SearchByParameters.LOYALTY_ATTRIBUTE_ID, loyaltyAttributeId.ToString()));
                }

                string purchaseApplicableinSearch = "N";
                if (purchaseApplicable != null && purchaseApplicable.Value)
                    purchaseApplicableinSearch = "Y";

                searchParameters.Add(new KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>(LoyaltyAttributesDTO.SearchByParameters.PURCHASE_APPLICABLE, purchaseApplicableinSearch));

                string consumptionApplicableinSearch = "N";
                if (consumptionApplicable != null && consumptionApplicable.Value)
                    consumptionApplicableinSearch = "Y";
                searchParameters.Add(new KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>(LoyaltyAttributesDTO.SearchByParameters.CONSUMPTION_APPLICABLE, consumptionApplicableinSearch));

                LoyaltyAttributeListBL loyaltyAttributeListBL = new LoyaltyAttributeListBL(executionContext);
                var loyaltyRuleList = loyaltyAttributeListBL.GetAllLoyaltyAttributesList(searchParameters);
                log.LogMethodExit(loyaltyRuleList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = loyaltyRuleList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    data = customException
                });
            }
        }

        /// <summary>
        /// Post the JSON Object Questions List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Promotion/LoyaltyAttributes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<LoyaltyAttributesDTO> loyaltyAttributesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(loyaltyAttributesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (loyaltyAttributesDTOList != null && loyaltyAttributesDTOList.Any())
                {
                    LoyaltyAttributeListBL loyaltyAttributeListBL = new LoyaltyAttributeListBL(executionContext, loyaltyAttributesDTOList);
                    loyaltyAttributeListBL.Save();
                    log.LogMethodExit(loyaltyAttributesDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = loyaltyAttributesDTOList });
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
