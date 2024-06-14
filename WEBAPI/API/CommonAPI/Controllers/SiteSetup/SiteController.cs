/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Site Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-Mar-2019   Jagan Mohana         Created 
 *2.60         08-May-2019   Nitin Pai            modified for Guest app
 ********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Site;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Newtonsoft.Json.Linq;
using Semnox.CommonAPI.Helpers;
using Newtonsoft.Json;

namespace Semnox.CommonAPI.SiteSetup
{
    public class SiteController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Site Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/Site")]
        public HttpResponseMessage Get(int siteId = -1, bool isActive = false, bool excludeMasterSite = false )
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();

                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                // removing as entries done from mgmt sutdio form will not populate the IS_ACTIVE flag
                //searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "Y"));
                if (siteId != -1)
                {
                    searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, siteId.ToString()));
                }

                SiteList siteList = new SiteList(executionContext);
                List<SiteDTO> content = siteList.GetAllSites(searchParameters);
                if (content != null && content.Count != 0 && isActive)
                {
                    // filter for InActive sites here
                    content = content.Where(x => (x.IsActive)).OrderBy(x => x.SiteId).ToList();

                    //if this is from external users/customers - do not show the master site
                    if (excludeMasterSite)
                    {
                        SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                        if (HQSite != null && HQSite.SiteId != -1)
                        {
                            content = content.Where(x => x.SiteId != HQSite.SiteId).ToList();
                        }
                    }
                }

                List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                if (executionContext.GetIsCorporate())
                {
                    searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, Convert.ToString(siteId)));
                }
                searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "WEBSERVICE_UPLOAD_URL"));

                ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
                var uploadSiteURL = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParams);

                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, UploadURL = uploadSiteURL, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on siteDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/SiteSetup/Site/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] JObject jObject)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(jObject);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (jObject != null)
                {
                    // if siteDTOs.SiteId is less than zero then insert or else update
                    //SiteList siteList = new SiteList(executionContext, siteDTOs);
                    //siteList.SaveUpdateSiteList();
                    List<SiteDTO> siteDTOList = new List<SiteDTO>();
                    List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = new List<ParafaitDefaultsDTO>();
                    var siteDTOString = jObject.SelectToken("SiteDTOList").ToString();
                    var uploadURL = jObject.SelectToken("UploadURL").ToString();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    if (!string.IsNullOrEmpty(siteDTOString))
                    {
                        siteDTOList = JsonConvert.DeserializeObject<List<SiteDTO>>(siteDTOString, settings);
                        if (siteDTOList[0].Logo.Length == 0)
                        {
                            siteDTOList[0].Logo = null;
                        }
                    }
                    // uploadURL holds the ParafaitDefaultsDTO properties.
                    // it will first convert the Jobject into string and then Deserilaize it into ParafaitDefaultsDTO.

                    if (!string.IsNullOrEmpty(uploadURL))
                    {
                        parafaitDefaultsDTOList = JsonConvert.DeserializeObject<List<ParafaitDefaultsDTO>>(uploadURL, settings);
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        parafaitDBTrx.BeginTransaction();
                        try
                        {
                            if (siteDTOList != null &&
                                            siteDTOList.Any())
                            {
                                Site site = new Site(executionContext, siteDTOList[0]);
                                site.Save(parafaitDBTrx.SQLTrx);
                            }
                            if (parafaitDefaultsDTOList != null &&
                                        parafaitDefaultsDTOList.Any())
                            {
                                ParafaitDefaultsBL parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, parafaitDefaultsDTOList[0]);
                                parafaitDefaultsBL.Save(parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            log.LogMethodExit(null, "Throwing Exception : " + valEx.Message);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Conflict, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
