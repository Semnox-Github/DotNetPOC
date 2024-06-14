/********************************************************************************************
 * Project Name - CommonLookupController Controller                                                                         
 * Description  - Controller for the Mater Data tables all modules
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.50        20-Nov-2018    Jagan Mohana Rao          Created 
 *2.80        08-Apr-2020    Nitin Pai                 Cobra changes for WMS
 *2.90        09-Jun-2020    Mushahid Faizan           Modified : Commando changes for WMS
 *2.100       09-Sep-2020    Girish Kundar             Modified : Attendance pay rate changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;
using Semnox.Parafait.Product;
using Semnox.CommonAPI.Lookups;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Achievements;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.CommonServices
{
    public class CommonLookupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        /// <summary>
        /// Get method to fetch all the master table values for all modules
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/CommonServices/CommonLookup/")]
        public HttpResponseMessage Get([FromUri] string moduleName = null, string entityName = null, string dependentDropdownName = "", string dependentDropdownSelectedId = "")
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(moduleName, entityName);

                 securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (moduleName != null && entityName != null)
                {
                    log.Debug("CommonLookupController-Get() Method.");
                    List<CommonLookupsDTO> lookups = new List<CommonLookupsDTO>();
                    string isActive = string.Empty;
                    switch (moduleName.ToUpper().ToString())
                    {
                        case "LANGUAGES":

                            Languages languagesList = new Languages(executionContext);
                            List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                            searchParam.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            List<LanguagesDTO> languagesDTOList = languagesList.GetAllLanguagesList(searchParam);

                            List<CommonLookupDTO> commonLookupDTOList = new List<CommonLookupDTO>();
                            CommonLookupsDTO commonLookupsDTO = new CommonLookupsDTO();
                            if (languagesDTOList != null && languagesDTOList.Any())
                            {
                                languagesDTOList = languagesDTOList.OrderBy(x => x.LanguageName).ToList();

                                foreach (LanguagesDTO languagesDTO in languagesDTOList)
                                {
                                    CommonLookupDTO lookupDataObject;
                                    Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                     { "CultureCode", Convert.ToString(languagesDTO.CultureCode) }
                                };
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(languagesDTO.LanguageId), languagesDTO.LanguageName.ToString(), values);
                                    commonLookupDTOList.Add(lookupDataObject);
                                }
                                commonLookupsDTO.Items = commonLookupDTOList;
                                lookups.Add(commonLookupsDTO);
                            }
                            log.LogMethodExit(commonLookupDTOList);
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = commonLookupDTOList });
                        case "GAMES":
                            GameLookupBL gameLookupBL = new Lookups.GameLookupBL(entityName, executionContext);
                            lookups = gameLookupBL.GetLookUpMasterDataList();
                            break;
                        case "DIGITALSIGNAGE":
                            isActive = "1";
                            DigitalSignageLookupBL lokkupList = new DigitalSignageLookupBL(entityName, executionContext, dependentDropdownName, dependentDropdownSelectedId, isActive);
                            lookups = lokkupList.GetLookUpMasterDataList();
                            break;
                        case "PRODUCTS":
                            isActive = "1";
                            ProductsLookupBL productLookupBL = new ProductsLookupBL(entityName, executionContext, dependentDropdownName, dependentDropdownSelectedId, isActive);
                            lookups = productLookupBL.GetLookUpMasterDataList();
                            break;
                        case "CARDS":
                            isActive = "1";
                            AccountLookupBL accountLookupBL = new AccountLookupBL(entityName, executionContext, dependentDropdownName, dependentDropdownSelectedId, isActive);
                            lookups = accountLookupBL.GetLookUpMasterDataList();
                            break;
                        case "SITESETUP":
                            isActive = "1";
                            SiteSetupLookupBL siteSetupLookupBL = new SiteSetupLookupBL(entityName, executionContext, dependentDropdownName, dependentDropdownSelectedId, isActive);
                            lookups = siteSetupLookupBL.GetLookUpMasterDataList(securityTokenDTO.RoleId);
                            break;
                        case "MAINTENANCE":
                            MaintenanceLookUpBL maintenanceLookUpBL = new MaintenanceLookUpBL(entityName, executionContext);
                            lookups = maintenanceLookUpBL.GetLookUpMasterDataList();
                            break;
                        case "WAIVERS":
                            WaiverLookupBL waiverLookupBL = new WaiverLookupBL(entityName, executionContext);
                            lookups = waiverLookupBL.GetLookUpMasterDataList();
                            break;
                        case "FEEDBACKSURVEY":
                            CustomerFeedbackLookupBL customerFeedbackLookupBL = new CustomerFeedbackLookupBL(entityName, executionContext);
                            lookups = customerFeedbackLookupBL.GetLookUpMasterDataList();
                            break;
                        case "PROMOTIONS":
                            PromotionsLookupBL promotionsLookupBL = new PromotionsLookupBL(entityName, executionContext);
                            lookups = promotionsLookupBL.GetLookUpMasterDataList();
                            break;
                        case "ACHIEVEMENTS":
                            AchievementsLookupBL achievementsLookupBL = new AchievementsLookupBL(entityName, executionContext);
                            lookups = achievementsLookupBL.GetLookUpMasterDataList();
                            break;
                        //case "REPORTS":
                        //    ReportsLookupBL reportsLookupBL = new ReportsLookupBL(entityName, executionContext);
                        //    lookups = reportsLookupBL.GetLookUpMasterDataList();
                        //    break;
                        //case "HR":
                        //    HRLookupBL hrLookupBL = new HRLookupBL(entityName, executionContext);
                        //    lookups = hrLookupBL.GetLookUpMasterDataList();
                        //    break;
                        case "TOOLS":
                            ToolsLookupBL toolsLookupBL = new ToolsLookupBL(entityName, executionContext);
                            lookups = toolsLookupBL.GetLookUpMasterDataList();
                            break;
                        case "MESSAGINGCHANNELTYPE":

                            /// It will give the Lookup Id based on LookupName - MESSAGING_CHANNEL
                            LookupsList lookupsList = new LookupsList(executionContext);
                            List<KeyValuePair<LookupsDTO.SearchByParameters, string>> lookupSearchParam = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
                            lookupSearchParam.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_NAME, "MESSAGING_CHANNEL"));
                            lookupSearchParam.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            List<LookupsDTO> lookupsDTOList = lookupsList.GetAllLookups(lookupSearchParam, false, false, null);
                            int lookupId = lookupsDTOList[0].LookupId;

                            List<CommonLookupDTO> commonLookupList = new List<CommonLookupDTO>();
                            CommonLookupsDTO commonLookupDTO = new CommonLookupsDTO();

                            if (lookupsDTOList != null && lookupsDTOList.Any())
                            {
                                foreach (LookupsDTO lookupsDTO in lookupsDTOList)
                                {
                                    CommonLookupDTO lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupsDTO.LookupId), lookupsDTO.LookupName);
                                    commonLookupList.Add(lookupDataObject);
                                }
                            }

                            /// It will fetch the LookValueId based on above LookupId
                            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                            lookupValuesSearchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_ID, lookupId.ToString()));
                            lookupValuesSearchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParam, null);

                            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                            {
                                foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                                {
                                    CommonLookupDTO lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                    commonLookupList.Add(lookupDataObject);
                                }
                            }
                            commonLookupDTO.Items = commonLookupList;
                            lookups.Add(commonLookupDTO);

                            log.LogMethodExit(lookups);
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = lookups });

                        case "PAYTYPE":
                            List<CommonLookupDTO> cmmnLookupList = new List<CommonLookupDTO>();
                            CommonLookupsDTO cmmnLookupDTO = new CommonLookupsDTO();
                            Dictionary<int, String> payTypeIdDictionary = new Dictionary<int, string>
                            {
                                { -1,"-- None --" },
                                { 1,"Hourly" },
                                { 2,"Weekly" },
                                { 3,"Monthly" }
                            };
                            if (payTypeIdDictionary.Count != 0)
                            {
                                foreach (var payType in payTypeIdDictionary)
                                {
                                    CommonLookupDTO lookupDataObject;
                                    lookupDataObject = new CommonLookupDTO(payType.Key.ToString(), payType.Value);
                                    cmmnLookupList.Add(lookupDataObject);
                                }
                            }
                            cmmnLookupDTO.Items = cmmnLookupList;
                            lookups.Add(cmmnLookupDTO);
                            log.LogMethodExit(lookups);
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = lookups });
                        default:
                            log.Debug("No matching module name found.");
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                    }
                    log.LogMethodExit(lookups);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = lookups });
                }
                else
                {
                    log.Debug("CommonLookupController-Get() Method.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message  });
            }
        }
    }
}
