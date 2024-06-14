/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Localization
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.40        28-Sept-2018   Jagan Mohana Rao        Created 
 *2.80        08-Apr-2020    Nitin Pai               Cobra changes for Waiver, Customer and Online Sales
 *2.80        20-Apr-2020    Mushahid Faizan         Modified as part of Enhancement to get all literals at one time.
 *2.110       20-Jan-2021    Nitin Pai               return localication in form of JSON file
 *2.120       16-Mar-2021    Girish Kundar           Modified : added logic to create localization folder inside the parafait 
 *2.120       05-May-2021    Mushahid Faizan         Modified : Typo in the Error message "Unable to find Localization folder"
 *2.130.7     06-Apr-2023    Abhishek                Modified : Get to fetch data using containers.
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Script.Serialization;
using Semnox.CommonAPI.Helpers;
using Semnox.CommonAPI.Localization;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Cards;
using Semnox.Parafait.Communication;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.CommonServices
{
    public class LocalizationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        [Route("api/Localization/Headers/")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage Get(string moduleName, string entityName = "ALL")
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(moduleName, entityName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                string content = string.Empty;
                switch (moduleName.ToUpper().ToString())
                {
                    case "APPLICATION":
                        AllFormates applicationServicesBL = new AllFormates(executionContext, entityName);
                        content = applicationServicesBL.GetLocalizedLabelsAndHeaders();
                        Dictionary<string, string> formateTypes = applicationServicesBL.GetFormateTypes();
                        log.LogMethodExit(content);
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = content, formateTypesList = formateTypes, token = securityTokenDTO.Token });
                    case "GAMES":
                        GamesLocalizationBL gameslocalizationBL = new GamesLocalizationBL(executionContext, entityName);
                        content = gameslocalizationBL.GetLocalizedLabelsAndHeaders();
                        break;
                    case "DIGITALSIGNAGE":
                        DigitalSignageLocalizationBL digitalSignagelocalizationBL = new DigitalSignageLocalizationBL(executionContext, entityName);
                        content = digitalSignagelocalizationBL.GetLocalizedLabelsAndHeaders();
                        break;
                    case "PRODUCTS":
                        ProductsLocalizationBL productslocalizationBL = new ProductsLocalizationBL(executionContext, entityName);
                        content = productslocalizationBL.GetLocalizedLabelsAndHeaders();
                        break;
                    case "CARDS":
                        AccountsLocalizationBL accountsLocalizationBL = new AccountsLocalizationBL(executionContext, entityName);
                        content = accountsLocalizationBL.GetLocalizedLabelsAndHeaders();
                        break;
                    case "SITESETUP":
                        SiteSetupLocalizationBL siteSetupLocalizationBL = new SiteSetupLocalizationBL(executionContext, entityName);
                        content = siteSetupLocalizationBL.GetLocalizedLabelsAndHeaders();
                        break;
                    case "CUSTOMERREGISTRATION":
                        CustomerRegistrationLocalizationBL customerRegistrationLocalizationBL = new CustomerRegistrationLocalizationBL(executionContext, entityName);
                        content = customerRegistrationLocalizationBL.GetLocalizedLabelsAndHeaders();
                        break;
                    case "WAIVERS":
                        WaiverLocalizationBL waiverLocalizationBL = new WaiverLocalizationBL(executionContext, entityName);
                        content = waiverLocalizationBL.GetLocalizedLabelsAndHeaders();
                        break;
                }
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Localization/Headers")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, int messageNo = -1, bool literalsOnly = false, bool messagesOnly = false, int languageId = -1,
            String outputFormat = null)
        {
            try
            {
                log.LogMethodEntry(siteId, messageNo, literalsOnly, messagesOnly);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters = new List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>>();
                searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.SITE_ID, siteId == -1 ? executionContext.GetSiteId().ToString() : siteId.ToString()));
                List<MessagesDTO> content = new List<MessagesDTO>();
                // resetting the site in context so that correct container is fetched
                if (executionContext.GetIsCorporate() && executionContext.GetSiteId() != siteId && siteId != -1)
                    executionContext.SetSiteId(siteId);

                if (languageId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.LANGUAGE_ID, languageId.ToString()));
                }
                else //Check if default language is set , If Yes then build the translation for that language 
                {
                    int defaultLanguageId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_LANGUAGE"));
                    if (defaultLanguageId > -1)
                    {
                        searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.LANGUAGE_ID, defaultLanguageId.ToString()));
                        languageId = defaultLanguageId;
                    }
                }

                if (String.IsNullOrEmpty(outputFormat))
                {
                    if (literalsOnly)
                        searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.LITERALS_ONLY, ""));

                    if (messagesOnly)
                        searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.MESSAGES_ONLY, ""));

                    if (messageNo > -1)
                        searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.MESSAGE_NO, messageNo.ToString()));
                    
                    MessageListBL messageListBL = new MessageListBL(executionContext);
                    content = messageListBL.GetAllMessagesList(searchParameters, true, false);
                }
                else
                {
                    MessageContainerDTOCollection messageContainerDTOCollection = MessageViewContainerList.GetMessageContainerDTOCollection(executionContext.GetSiteId(), languageId, null);
                    Dictionary<string, string> returnCollection = new Dictionary<string, string>();
                    if (messageContainerDTOCollection != null)
                    {
                        foreach (MessageContainerDTO messageDTO in messageContainerDTOCollection.MessageContainerDTOList)
                        {
                            String translatedMessage = messageDTO.Message;
                            if (languageId > -1)
                            {
                                if (messageDTO.TranslatedMessage != null && messageDTO.TranslatedMessage.Any())
                                {
                                    translatedMessage = messageDTO.TranslatedMessage;
                                }
                                else
                                {
                                    translatedMessage = messageDTO.Message;
                                }
                            }
                            if (!returnCollection.ContainsKey(messageDTO.Message))
                                returnCollection.Add(messageDTO.Message, translatedMessage);
                        }
                    }
                   
                    String jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(returnCollection);
                    try
                    {
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jsonContent);

                        if (outputFormat.Equals("JSON", StringComparison.InvariantCultureIgnoreCase))
                        {
                            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            return response;
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid Output Format" });
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug(ex.Message);
                        throw;
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    data = ex.Message
                });
            }
        }
    }
}