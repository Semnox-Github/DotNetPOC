/********************************************************************************************
 * Project Name - POS
 * Description  - Proxy class implementation of product menu use cases
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class RemoteProductMenuUseCases : RemoteUseCases, IProductMenuUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCT_MENU_URL = "api/Product/Menus";
        private const string PRODUCT_MENU_CONTAINER_URL = "api/Product/MenuContainer";
        private const string PRODUCT_MENU_PANEL_URL = "api/Product/MenuPanels";
        private const string ADD_PRODUCT_MENU_PANEL_CONTENT_URL = "api/Product/MenuPanels/{panelId}/Contents";

        public RemoteProductMenuUseCases(ExecutionContext executionContext)
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ProductMenuDTO>> GetProductMenuDTOList(string isActive = null, int menuId = -1, string name = "", DateTime? endDateGreaterThanEqualTo = null, DateTime? startDateLessThanEqualTo = null, int siteId = -1, bool loadChildRecords = true, bool loadActiveChildRecords = true)
        {
            log.LogMethodEntry(isActive, menuId, name, endDateGreaterThanEqualTo, startDateLessThanEqualTo, siteId, loadChildRecords, loadActiveChildRecords);
            List<ProductMenuDTO> result = await Get<List<ProductMenuDTO>>(PRODUCT_MENU_URL, new WebApiGetRequestParameterCollection("isActive", 
                                                                                                                                    isActive,
                                                                                                                                    "menuId", 
                                                                                                                                    menuId,
                                                                                                                                    "name", 
                                                                                                                                    name,
                                                                                                                                    "endDateGreaterThanEqualTo", 
                                                                                                                                    endDateGreaterThanEqualTo,
                                                                                                                                    "startDateLessThanEqualTo", 
                                                                                                                                    startDateLessThanEqualTo,
                                                                                                                                    "siteId", 
                                                                                                                                    siteId,
                                                                                                                                    "loadChildRecords", 
                                                                                                                                    loadChildRecords,
                                                                                                                                    "loadActiveChildRecords", 
                                                                                                                                    loadActiveChildRecords));
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<ProductMenuPanelDTO>> GetProductMenuPanelDTOList(string isActive = null, int panelId = -1, string name = "", int siteId = -1, bool loadChildRecords = true, bool loadActiveChildRecords = true, string guid = "")
        {
            log.LogMethodEntry(isActive, panelId, name, siteId, loadChildRecords, loadActiveChildRecords);
            List <ProductMenuPanelDTO> result = await Get<List<ProductMenuPanelDTO>>(PRODUCT_MENU_PANEL_URL, new WebApiGetRequestParameterCollection("isActive", 
                                                                                                                                                     isActive,
                                                                                                                                                     "panelId", 
                                                                                                                                                     panelId,
                                                                                                                                                     "name", 
                                                                                                                                                     name,
                                                                                                                                                     "siteId", 
                                                                                                                                                     siteId,
                                                                                                                                                     "loadChildRecords", 
                                                                                                                                                     loadChildRecords,
                                                                                                                                                     "loadActiveChildRecords", 
                                                                                                                                                     loadActiveChildRecords,
                                                                                                                                                     "guid", 
                                                                                                                                                     guid));
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<ProductMenuDTO>> SaveProductMenuDTOList(List<ProductMenuDTO> productMenuDTOList)
        {
            log.LogMethodEntry(productMenuDTOList);
            List<ProductMenuDTO> result = await Post<List<ProductMenuDTO>>(PRODUCT_MENU_URL, productMenuDTOList);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<ProductMenuPanelDTO>> SaveProductMenuPanelDTOList(List<ProductMenuPanelDTO> productMenuPanelDTOList)
        {
            log.LogMethodEntry(productMenuPanelDTOList);
            List<ProductMenuPanelDTO> result = await Post<List<ProductMenuPanelDTO>>(PRODUCT_MENU_PANEL_URL, productMenuPanelDTOList);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<string> AddProductMenuPanelContentDTOList(int panelId, List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList)
        {
            log.LogMethodEntry(panelId, productMenuPanelContentDTOList);
            string url = ADD_PRODUCT_MENU_PANEL_CONTENT_URL.Replace(@"{panelId}", panelId.ToString());
            string result = await Post<string>(url, productMenuPanelContentDTOList);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<ProductMenuContainerSnapshotDTOCollection> GetProductMenuContainerSnapshotDTOCollection(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, startDateTime, endDateTime, hash, rebuildCache);
            ProductMenuContainerSnapshotDTOCollection result = await Get<ProductMenuContainerSnapshotDTOCollection>(PRODUCT_MENU_CONTAINER_URL, new WebApiGetRequestParameterCollection("siteId", 
                                                                                                                                                                                         siteId, 
                                                                                                                                                                                         "userRoleId", 
                                                                                                                                                                                         userRoleId,
                                                                                                                                                                                         "posMachineId", 
                                                                                                                                                                                         posMachineId,
                                                                                                                                                                                         "languageId", 
                                                                                                                                                                                         languageId,
                                                                                                                                                                                         "menuType", 
                                                                                                                                                                                         menuType,
                                                                                                                                                                                         "startDateTime", 
                                                                                                                                                                                         startDateTime,
                                                                                                                                                                                         "endDateTime", 
                                                                                                                                                                                         endDateTime,
                                                                                                                                                                                         "hash", 
                                                                                                                                                                                         hash,
                                                                                                                                                                                         "rebuildCache", 
                                                                                                                                                                                         rebuildCache));
            log.LogMethodExit(result);
            return result;
        }
    }
}
