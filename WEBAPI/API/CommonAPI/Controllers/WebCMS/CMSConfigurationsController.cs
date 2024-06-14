/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the CMS Configuration and Localization
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.80       11-Oct-2019   Mushahid Faizan    Created
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
using Semnox.Parafait.WebCMS;

namespace Semnox.CommonAPI.CommonServices
{
    public class CMSConfigurationsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      

        [HttpGet]
        [Route("api/WebCMS/CMSConfigurations")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, string moduleName = null, string entityName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, moduleName, entityName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), securityTokenDTO.LanguageId);
                // Get the CMSModules based on the passed moduleName.
                List<CMSModulesDTO> cmsModulesDTOList = new List<CMSModulesDTO>();
                int menuID = -1;

                string moduleNames = string.Empty;
                if (!String.IsNullOrEmpty(moduleName))
                {
                    CMSModulesBLList cmsModulesBLList = new CMSModulesBLList(executionContext);
                    List<KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string>> moduleSearchParams = new List<KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string>>();
                    moduleSearchParams.Add(new KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string>(CMSModulesDTO.SearchByRequestParameters.DESCRIPTION, moduleName));
                    cmsModulesDTOList = cmsModulesBLList.GetAllCMSModules(moduleSearchParams);
                    if (cmsModulesDTOList != null && cmsModulesDTOList.Count != 0)
                    {
                        moduleNames = cmsModulesDTOList.Select(m => m.Description).First(); // Select moduleNames from CMSModules to get the CMSMenuItems Details.
                    }
                }

                CMSMenuItemBLList cmsMenuItemBLList = new CMSMenuItemBLList(executionContext);
                List<KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>> menuItemsParameter = new List<KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>>();
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        menuItemsParameter.Add(new KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>(CMSMenuItemsDTO.SearchByRequestParameters.ACTIVE, isActive));
                    }
                }
                if (menuID > 0)
                {
                    menuItemsParameter.Add(new KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>(CMSMenuItemsDTO.SearchByRequestParameters.MENU_ID, menuID.ToString()));
                }
                List<CMSMenuItemsDTO> cmsMenuItemsDTOList = cmsMenuItemBLList.GetAllCMSMenuItems(menuItemsParameter);
                List<CMSThemesDTO> cmsThemesDTOList = new List<CMSThemesDTO>();
                List<KeyValuePair<CMSThemesDTO.SearchByParameters, string>> themesSearchParams = new List<KeyValuePair<CMSThemesDTO.SearchByParameters, string>>();
                themesSearchParams.Add(new KeyValuePair<CMSThemesDTO.SearchByParameters, string>(CMSThemesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                CMSThemesListBL cMSThemesListBL = new CMSThemesListBL(executionContext);
                cmsThemesDTOList = cMSThemesListBL.GetAllCMSThemes(themesSearchParams);

                List<CMSPagesDTO> cmsPagesDTOList = new List<CMSPagesDTO>();
                CMSPagesBLList cmsPagesBLList = new CMSPagesBLList(executionContext);
                List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>> pagesSearchParams = new List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>>();
                if (!string.IsNullOrEmpty(entityName))
                {
                    pagesSearchParams.Add(new KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>(CMSPagesDTO.SearchByRequestParameters.PAGE_NAME, entityName.ToString()));
                }
                cmsPagesDTOList = cmsPagesBLList.GetAllPages(pagesSearchParams);
                if (cmsThemesDTOList != null && cmsThemesDTOList.Count != 0)
                {
                    foreach (CMSThemesDTO cMSThemesDTO in cmsThemesDTOList)
                    {
                        if (cmsPagesDTOList != null && cmsPagesDTOList.Count != 0)
                        {
                            var pages = cmsPagesDTOList.FirstOrDefault(m => m.PageId == cMSThemesDTO.PageId);
                            if (pages != null) cMSThemesDTO.PageName = pages.PageName;
                        }
                        cMSThemesDTO.ModuleName = moduleNames;
                    }
                }

                List<CMSContentDTO> cmsContentDTOList = new List<CMSContentDTO>();
                CMSContentBLList cmsContentBLList = new CMSContentBLList(executionContext);
                List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>> contentParameter = new List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>>();
                if (!string.IsNullOrEmpty(entityName) && cmsPagesDTOList != null && cmsPagesDTOList.Count != 0)
                {
                    var pages = cmsPagesDTOList.Select(m => m.PageId).First();
                    contentParameter.Add(new KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>(CMSContentDTO.SearchByRequestParameters.PAGE_ID, pages.ToString()));
                }
                cmsContentDTOList = cmsContentBLList.GetAllCmsContent(contentParameter);

                if (cmsThemesDTOList != null && cmsThemesDTOList.Any() || cmsMenuItemsDTOList != null && cmsMenuItemsDTOList.Any()
                    || cmsContentDTOList != null && cmsContentDTOList.Any())
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { Themes = cmsThemesDTOList, Menus = cmsMenuItemsDTOList, Content = cmsContentDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
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
