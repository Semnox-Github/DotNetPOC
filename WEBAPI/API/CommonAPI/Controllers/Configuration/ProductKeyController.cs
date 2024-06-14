/********************************************************************************************
 * Project Name - Site Setup
 * Description  - API for the Purge Data in site Setup module.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.60        04-Jun-2019   Mushahid Faizan     Created 
 *2.70.3      21-Apr-2020  Girish Kundar        Modified : Added POST parameter saveAuthKey 
 *2.90.0      14-Jun-2020   Girish Kundar       Modified : REST API phase 2 changes/standard  
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Site;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.Configuration
{
    public class ProductKeyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Product Key Details
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/ProductKeys")]
        public HttpResponseMessage Get(string activityType = null, int siteId = -1, string dateTime = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(activityType, siteId, dateTime);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                //executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                DateTime? authKeyDate = null;
                CultureInfo provider = CultureInfo.InvariantCulture;
                switch (activityType.ToUpper())
                {
                    case "PRODUCTKEY":
                        List<KeyValuePair<ProductKeyDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductKeyDTO.SearchByParameters, string>>();
                        if (siteId > 0)
                        {
                            executionContext.SetSiteId(siteId);
                            searchParameters.Add(new KeyValuePair<ProductKeyDTO.SearchByParameters, string>(ProductKeyDTO.SearchByParameters.SITE_ID, Convert.ToString(siteId)));
                        }
                        else
                        {
                            searchParameters.Add(new KeyValuePair<ProductKeyDTO.SearchByParameters, string>(ProductKeyDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                        }

                        ProductKeyBL productKeyBL = new ProductKeyBL(executionContext);
                        // This below code will return the expiry Date of the License Key
                        string expiryDate = productKeyBL.GetExpiryDate();
                        // This below code will return the NoOfLicensedPosMachines
                        int noOfLicensedMachine = productKeyBL.GetNoOfLicensedPOSMachines();
                        ProductKeyListBL productKeyListBL = new ProductKeyListBL(executionContext);
                        var content = productKeyListBL.GetProductKeyDTOList(searchParameters, true);
                        bool isProtected = false;
                        if (securityTokenDTO.LoginId.ToLower() != "semnox")
                        {
                            isProtected = true;
                        }

                        if (content != null && content.Count != 0)
                        {
                            if (content[0].AuthKey.Length > 0 && content[0].AuthKey != null)
                            {
                                string authKeys = Encoding.Default.GetString(content[0].AuthKey);
                                if (String.IsNullOrEmpty(authKeys) == false)
                                {
                                    string ke = Encryption.Decrypt(authKeys);
                                    if (ke.Length > 17)
                                    {

                                        authKeyDate = DateTime.ParseExact(ke.Substring(17), "dd-MMM-yyyy", provider);
                                    }
                                }
                            }
                        }
                        string maxCardsEncrypted = string.Empty;
                        Site site = new Site(executionContext.GetSitePKId());
                        if (site.getSitedTO != null)
                        {
                            maxCardsEncrypted = site.getSitedTO.MaxCards;
                        }
                        int maxCards = 0;
                        try
                        {
                            if (!string.IsNullOrEmpty(maxCardsEncrypted))
                            {
                                string siteKey = string.Empty;
                                string licKey = string.Empty;
                                DBUtils dBUtils = new DBUtils();
                                ParafaitEnv env = new ParafaitEnv(dBUtils);
                                env.SiteId = executionContext.GetSiteId();
                                env.IsCorporate = executionContext.GetIsCorporate();
                                env.LanguageId = executionContext.GetLanguageId();
                                env.LoginID = executionContext.GetUserId();
                                KeyManagement keyManagementObj = new KeyManagement(dBUtils, env);
                                keyManagementObj.ReadKeysFromDB(ref siteKey, ref licKey);
                                maxCards = Convert.ToInt32(Encryption.Decrypt(maxCardsEncrypted, siteKey.PadRight(8, '0').Substring(0, 8)));
                            }
                            else
                            {
                                maxCards = 0;
                            }
                        }
                        catch { }
                        log.LogMethodExit(content);
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = content, isProtectedReadonly = isProtected, ExpiryDate = expiryDate, NoOfLicensedMachine = noOfLicensedMachine, AuthKeyDate = authKeyDate, MaxCardsCount = maxCards });
                    case "GENERATEAUTHKEY":
                        DateTime dateTimeFormate = Convert.ToDateTime(dateTime);
                        string authKey = Encryption.Encrypt("FF-FF-FF-FF-FF-FF" + dateTimeFormate.ToString("dd-MMM-yyyy", provider));
                        byte[] bytes = Encoding.ASCII.GetBytes(authKey);
                        log.LogMethodExit(authKey);
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = bytes });
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.NotFound, new
                {
                    data = "",
                    token = securityTokenDTO.Token
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on Product Key Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Configuration/ProductKeys")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ProductKeyDTO> productKeyDTOList, bool authKeyUpdate = false, string activityType = null, string cardValue = null, string siteKeyValue = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(productKeyDTOList, authKeyUpdate, activityType);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                //executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                switch (activityType.ToUpper())
                {
                    case "PRODUCTKEY":
                        if (productKeyDTOList != null && productKeyDTOList.Count != 0)
                        {
                            ProductKeyListBL productKeyListBL = new ProductKeyListBL(executionContext, productKeyDTOList);
                            productKeyListBL.Save(authKeyUpdate);
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                        }
                        else
                        {
                            log.LogMethodExit();
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                        }

                    case "ADDCARDS":
                        if (!string.IsNullOrEmpty(cardValue))
                        {
                            int addCards = 0;
                            int maxCard = 0;
                            string addCardCode = Encryption.Decrypt(cardValue);
                            string[] addCardValue = addCardCode.Split('|');
                            ProductKeyServiceBL productKeyServiceBL = new ProductKeyServiceBL(executionContext);
                            if (addCardValue != null && addCardValue.Count() > 2)
                            {
                                if (addCardValue[0] == siteKeyValue)
                                {
                                    if (DateTime.Now.Date >= Convert.ToDateTime(addCardValue[2].ToString()).AddDays(-2) &&
                                        DateTime.Now.Date <= Convert.ToDateTime(addCardValue[2].ToString()))
                                    {
                                        addCards = Convert.ToInt32(double.Parse(addCardValue[1]));
                                        maxCard = productKeyServiceBL.SaveMaxCards(addCards, siteKeyValue, cardValue);
                                    }
                                    else
                                    {
                                        log.Error("Sorry, Add Cards Key is no longer valid");
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5074));
                                    }
                                }
                                log.LogMethodExit();
                                return Request.CreateResponse(HttpStatusCode.OK, new { data = maxCard });
                            }
                            else
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1895));
                            }
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1894));
                        }
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
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
    }
}
