/********************************************************************************************
 * Project Name - POS
 * Description  - Container class to hold the product menu panels
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      10-Jun-2021      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.POS
{
    public class ProductMenuContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<ProductMenuDTO> productMenuDTOList;
        private readonly List<ProductMenuPanelDTO> productMenuPanelDTOList;
        private readonly Dictionary<int, ProductMenuDTO> productMenuIdProductMenuDTODictionary = new Dictionary<int, ProductMenuDTO>();
        private readonly Dictionary<int, ProductMenuPanelDTO> panelIdProductMenuPanelDTODictionary = new Dictionary<int, ProductMenuPanelDTO>();
        private readonly Dictionary<string, ProductMenuPanelDTO> panelGuidProductMenuPanelDTODictionary = new Dictionary<string, ProductMenuPanelDTO>();
        private readonly Dictionary<int, ProductMenuPanelContainerDTO> panelIdProductMenuPanelContainerDTODictionary = new Dictionary<int, ProductMenuPanelContainerDTO>();
        
        private readonly Dictionary<int, HashSet<int>> menuIdReferencedPanelIdHashSet = new Dictionary<int, HashSet<int>>();
        private readonly Dictionary<int, HashSet<int>> menuIdMainPanelIdHashSet = new Dictionary<int, HashSet<int>>();
        private readonly Cache<ProductMenuContainerCacheKey, ProductMenuContainerSnapshotDTOCollection> productMenuContainerSnapshotDTOCollectionCache = new Cache<ProductMenuContainerCacheKey, ProductMenuContainerSnapshotDTOCollection>();
        private readonly int siteId;
        private readonly DateTime? maxLastUpdateTime;
        public ProductMenuContainer(int siteId)
            : this(siteId,
                   GetProductMenuDTOList(siteId),
                   GetProductMenuPanelDTOList(siteId),
                   GetMaxLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }



        public ProductMenuContainer(int siteId,
                                    List<ProductMenuDTO> productMenuDTOList,
                                    List<ProductMenuPanelDTO> productMenuPanelDTOList,
                                    DateTime? maxLastUpdateTime)
        {
            log.LogMethodEntry(siteId, productMenuDTOList, productMenuPanelDTOList, maxLastUpdateTime);
            this.siteId = siteId;
            this.maxLastUpdateTime = maxLastUpdateTime;
            this.productMenuDTOList = productMenuDTOList;
            this.productMenuPanelDTOList = productMenuPanelDTOList;
            AssignProductMenuPanels();
            AssignProductMenus();
            log.LogMethodExit();
        }

        protected void AssignProductMenus()
        {
            log.LogMethodEntry();
            productMenuIdProductMenuDTODictionary.Clear();
            menuIdReferencedPanelIdHashSet.Clear();
            menuIdMainPanelIdHashSet.Clear();
            foreach (var productMenuDTO in productMenuDTOList)
            {
                if (productMenuIdProductMenuDTODictionary.ContainsKey(productMenuDTO.MenuId) ||
                    menuIdReferencedPanelIdHashSet.ContainsKey(productMenuDTO.MenuId) ||
                    menuIdMainPanelIdHashSet.ContainsKey(productMenuDTO.MenuId))
                {
                    continue;
                }
                productMenuIdProductMenuDTODictionary.Add(productMenuDTO.MenuId, productMenuDTO);
                menuIdReferencedPanelIdHashSet.Add(productMenuDTO.MenuId, new HashSet<int>());
                menuIdMainPanelIdHashSet.Add(productMenuDTO.MenuId, new HashSet<int>());
                if (productMenuDTO.ProductMenuPanelMappingDTOList == null ||
                    productMenuDTO.ProductMenuPanelMappingDTOList.Any() == false)
                {
                    continue;
                }
                foreach (var productMenuPanelMappingDTO in productMenuDTO.ProductMenuPanelMappingDTOList)
                {
                    AddReferencedPanels(productMenuDTO.MenuId, productMenuPanelMappingDTO.PanelId);
                    menuIdMainPanelIdHashSet[productMenuDTO.MenuId].Add(productMenuPanelMappingDTO.PanelId);
                }
            }
            log.LogMethodExit();
        }

        protected void AssignProductMenuPanels()
        {
            log.LogMethodEntry();
            panelIdProductMenuPanelDTODictionary.Clear();
            panelGuidProductMenuPanelDTODictionary.Clear();
            panelIdProductMenuPanelContainerDTODictionary.Clear();
            foreach (var productMenuPanelDTO in productMenuPanelDTOList)
            {
                if (panelIdProductMenuPanelDTODictionary.ContainsKey(productMenuPanelDTO.PanelId) ||
                    panelGuidProductMenuPanelDTODictionary.ContainsKey(productMenuPanelDTO.Guid))
                {
                    continue;
                }
                panelIdProductMenuPanelDTODictionary.Add(productMenuPanelDTO.PanelId, productMenuPanelDTO);
                panelGuidProductMenuPanelDTODictionary.Add(productMenuPanelDTO.Guid, productMenuPanelDTO);
            }

            //separate loop to handle circular panel references
            foreach (var productMenuPanelDTO in productMenuPanelDTOList)
            {
                if (panelIdProductMenuPanelContainerDTODictionary.ContainsKey(productMenuPanelDTO.PanelId))
                {
                    continue;
                }
                ProductMenuPanelContainerDTO productMenuPanelContainerDTO = GetProductMenuPanelContainerDTO(productMenuPanelDTO);
                panelIdProductMenuPanelContainerDTODictionary.Add(productMenuPanelDTO.PanelId, productMenuPanelContainerDTO);
            }
            log.LogMethodExit();
        }

        private void AddReferencedPanels(int menuId, int panelId)
        {
            log.LogMethodEntry(menuId, panelId);
            if(panelIdProductMenuPanelContainerDTODictionary.ContainsKey(panelId) == false)
            {
                log.LogMethodExit(null, "Unable to find a panel with panelId : " + panelId);
                return;
            }
            menuIdReferencedPanelIdHashSet[menuId].Add(panelId);
            ProductMenuPanelContainerDTO productMenuPanelContainerDTO = panelIdProductMenuPanelContainerDTODictionary[panelId];
            foreach (var childPanelId in productMenuPanelContainerDTO.ProductMenuPanelContentContainerDTOList.Where(x => x.ChildPanelId > -1).Select(x => x.ChildPanelId))
            {
                if(menuIdReferencedPanelIdHashSet[menuId].Contains(childPanelId))
                {
                    continue;
                }
                menuIdReferencedPanelIdHashSet[menuId].Add(childPanelId);
                AddReferencedPanels(menuId, childPanelId);
            }
            
        }

        private static DateTime? GetMaxLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                ProductMenuListBL productMenuListBL = new ProductMenuListBL();
                result = productMenuListBL.GetProductMenuModuleLastUpdateTime(siteId);
                ProductsList productsList = new ProductsList();
                DateTime? productModuleLastUpdateTime = productsList.GetProductsLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (productModuleLastUpdateTime.HasValue && result.Value < productModuleLastUpdateTime.Value))
                {
                    result = productModuleLastUpdateTime;
                }
                POSMachineList pOSMachineList = new POSMachineList();
                DateTime? posMachineModuleLastUpdateTime = pOSMachineList.GetPOSModuleLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (posMachineModuleLastUpdateTime.HasValue && result.Value < posMachineModuleLastUpdateTime.Value))
                {
                    result = productModuleLastUpdateTime;
                }
                UserRolesList userRolesList = new UserRolesList();
                DateTime? userRoleModuleLastUpdateTime = userRolesList.GetUserRoleModuleLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (userRoleModuleLastUpdateTime.HasValue && result.Value < userRoleModuleLastUpdateTime.Value))
                {
                    result = userRoleModuleLastUpdateTime;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the product menu max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<ProductMenuPanelDTO> GetProductMenuPanelDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ProductMenuPanelDTO> result = null;
            try
            {
                var productMenuPanelListBL = new ProductMenuPanelListBL();
                List<KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>> searchParameters = new List<KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>(ProductMenuPanelDTO.SearchByProductMenuPanelParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>(ProductMenuPanelDTO.SearchByProductMenuPanelParameters.IS_ACTIVE, "1"));
                result = productMenuPanelListBL.GetProductMenuPanelDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the product menu panels.", ex);
            }

            if (result == null)
            {
                result = new List<ProductMenuPanelDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<ProductMenuDTO> GetProductMenuDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ProductMenuDTO> result = null;
            try
            {
                var productMenuListBL = new ProductMenuListBL();
                List<KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>> searchParameters = new List<KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>(ProductMenuDTO.SearchByProductMenuParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>(ProductMenuDTO.SearchByProductMenuParameters.IS_ACTIVE, "1"));
                result = productMenuListBL.GetProductMenuDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the product menus.", ex);
            }

            if (result == null)
            {
                result = new List<ProductMenuDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }

        private ProductMenuPanelContainerDTO GetProductMenuPanelContainerDTO(ProductMenuPanelDTO productMenuPanelDTO)
        {
            log.LogMethodEntry(productMenuPanelDTO);
            ProductMenuPanelContainerDTO result = new ProductMenuPanelContainerDTO(productMenuPanelDTO.PanelId, false,
                 productMenuPanelDTO.DisplayOrder, productMenuPanelDTO.Name,
                 productMenuPanelDTO.CellMarginLeft, productMenuPanelDTO.CellMarginRight,
                 productMenuPanelDTO.CellMarginTop, productMenuPanelDTO.CellMarginBottom,
                 productMenuPanelDTO.RowCount, productMenuPanelDTO.ColumnCount,
                 productMenuPanelDTO.ImageURL, productMenuPanelDTO.Guid);
            if (productMenuPanelDTO.ProductMenuPanelContentDTOList == null ||
                productMenuPanelDTO.ProductMenuPanelContentDTOList.Any() == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            foreach (var productMenuPanelContentDTO in productMenuPanelDTO.ProductMenuPanelContentDTOList)
            {
                if (IsValidProductMenuPanelContentDTO(productMenuPanelContentDTO) == false)
                {
                    continue;
                }
                int productId = GetProductId(productMenuPanelContentDTO);
                int childPanelId = GetChildPanelId(productMenuPanelContentDTO);
                string name = GetName(productMenuPanelContentDTO);
                string imageFileName = GetImageFileName(productMenuPanelContentDTO);
                bool isDiscounted = GetIsDiscounted(productMenuPanelContentDTO);
                ProductMenuPanelContentContainerDTO productMenuPanelContentContainerDTO =
                    new ProductMenuPanelContentContainerDTO(productMenuPanelContentDTO.Id,
                                                             productMenuPanelContentDTO.PanelId,
                                                             productId,
                                                             childPanelId,
                                                             name,
                                                             (productMenuPanelContentDTO.RowIndex * productMenuPanelDTO.ColumnCount) + productMenuPanelContentDTO.ColumnIndex,
                                                             string.IsNullOrWhiteSpace(productMenuPanelContentDTO.ImageURL)? imageFileName : productMenuPanelContentDTO.ImageURL,
                                                             productMenuPanelContentDTO.BackColor,
                                                             productMenuPanelContentDTO.TextColor,
                                                             productMenuPanelContentDTO.Font,
                                                             productMenuPanelContentDTO.ColumnIndex,
                                                             productMenuPanelContentDTO.RowIndex,
                                                             productMenuPanelContentDTO.ButtonType,
                                                             isDiscounted);
                result.ProductMenuPanelContentContainerDTOList.Add(productMenuPanelContentContainerDTO);
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool IsValidProductMenuPanelContentDTO(ProductMenuPanelContentDTO productMenuPanelContentDTO)
        {
            log.LogMethodEntry(productMenuPanelContentDTO);
            bool result = false;
            if(ProductMenuObjectTypes.IsValidObjectType(productMenuPanelContentDTO.ObjectType) == false)
            {
                string errorMessage = "Invalid object type(" + productMenuPanelContentDTO.ObjectType + "). Object Type is not defined.";
                log.LogMethodExit(result,"Throwing Exception -" + errorMessage);
                throw new Exception(errorMessage);
            }
            if(productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT)
            {
                ProductsContainerDTO productsContainerDTO = GetProductsContainerDTOOrDefault(productMenuPanelContentDTO.ObjectGuid);
                result = productsContainerDTO != null && productsContainerDTO.DisplayInPOS == "Y" && productsContainerDTO.IsActive;
            }
            else if(productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT_MENU_PANEL)
            {
                result = panelGuidProductMenuPanelDTODictionary.ContainsKey(productMenuPanelContentDTO.ObjectGuid);
            }
            else
            {
                string errorMessage = "Object type(" + productMenuPanelContentDTO.ObjectType + ") is not supported.";
                log.LogMethodExit(result, "Throwing Exception -" + errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(result);
            return result;
        }

        protected virtual ProductsContainerDTO GetProductsContainerDTOOrDefault(int productId)
        {
            log.LogMethodEntry(productId);
            ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTOOrDefault(siteId, productId);
            log.LogMethodExit(productsContainerDTO);
            return productsContainerDTO;
        }

        protected virtual ProductsContainerDTO GetProductsContainerDTOOrDefault(string productguid)
        {
            log.LogMethodEntry(productguid);
            ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTOOrDefault(siteId, productguid);
            log.LogMethodExit(productsContainerDTO);
            return productsContainerDTO;
        }

        private int GetProductId(ProductMenuPanelContentDTO productMenuPanelContentDTO)
        {
            log.LogMethodEntry(productMenuPanelContentDTO);
            int result = -1;
            if(productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT)
            {
                ProductsContainerDTO productsContainerDTO = GetProductsContainerDTOOrDefault(productMenuPanelContentDTO.ObjectGuid);
                result = productsContainerDTO.ProductId;
            }
            log.LogMethodExit(result);
            return result;
        }

        private int GetChildPanelId(ProductMenuPanelContentDTO productMenuPanelContentDTO)
        {
            log.LogMethodEntry(productMenuPanelContentDTO);
            int result = -1;
            if (productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT_MENU_PANEL)
            {
                result = panelGuidProductMenuPanelDTODictionary[productMenuPanelContentDTO.ObjectGuid].PanelId;
            }
            log.LogMethodExit(result);
            return result;
        }

        private string GetName(ProductMenuPanelContentDTO productMenuPanelContentDTO)
        {
            log.LogMethodEntry(productMenuPanelContentDTO);
            string result = string.Empty;
            if (productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT)
            {
                ProductsContainerDTO productsContainerDTO = GetProductsContainerDTOOrDefault(productMenuPanelContentDTO.ObjectGuid);
                if(productsContainerDTO != null)
                {
                    result = productsContainerDTO.ProductName;
                }
            }
            else if (productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT_MENU_PANEL)
            {
                result = panelGuidProductMenuPanelDTODictionary[productMenuPanelContentDTO.ObjectGuid].Name;
            }
            else
            {
                string errorMessage = "Object type(" + productMenuPanelContentDTO.ObjectType + ") is not supported.";
                log.LogMethodExit(result, "Throwing Exception -" + errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(result);
            return result;
        }

        private string GetImageFileName(ProductMenuPanelContentDTO productMenuPanelContentDTO)
        {
            log.LogMethodEntry(productMenuPanelContentDTO);
            string result = string.Empty;
            if (productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT)
            {
                ProductsContainerDTO productsContainerDTO = GetProductsContainerDTOOrDefault(productMenuPanelContentDTO.ObjectGuid);
                if (productsContainerDTO != null)
                {
                    result = productsContainerDTO.ImageFileName;
                }
            }
            else if (productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT_MENU_PANEL)
            {
                result = panelGuidProductMenuPanelDTODictionary[productMenuPanelContentDTO.ObjectGuid].ImageURL;
            }
            else
            {
                string errorMessage = "Object type(" + productMenuPanelContentDTO.ObjectType + ") is not supported.";
                log.LogMethodExit(result, "Throwing Exception -" + errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool GetIsDiscounted(ProductMenuPanelContentDTO productMenuPanelContentDTO)
        {
            log.LogMethodEntry(productMenuPanelContentDTO);
            bool result = false;
            if (productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT)
            {
                //result = DiscountMasterList.;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<ProductsContainerDTO> GetProductsContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = ProductsContainerList.GetActiveProductsContainerDTOList(siteId);
            log.LogMethodExit(result);
            return result;
        }

        public ProductMenuContainer Refresh()
        {
            log.LogMethodEntry();
            var keys = productMenuContainerSnapshotDTOCollectionCache.Keys;
            foreach (var key in keys)
            {
                if (key.DateTimeRange.EndDateTime < ServerDateTime.Now)
                {
                    ProductMenuContainerSnapshotDTOCollection value;
                    if (productMenuContainerSnapshotDTOCollectionCache.TryRemove(key, out value))
                    {
                        log.Debug("Removing ProductCalendarContainer of key " + key);
                    }
                    else
                    {
                        log.Debug("Unable to remove ProductCalendarContainer of key " + key);
                    }
                }
            }

            DateTime? updateTime = GetMaxLastUpdateTime(siteId);
            if (maxLastUpdateTime.HasValue
                && maxLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in product menu since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ProductsContainerList.Rebuild(siteId);
            UserRoleContainerList.Rebuild(siteId);
            POSMachineContainerList.Rebuild(siteId);

            ProductMenuContainer result = new ProductMenuContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

        private List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(List<int> productMenuIdList)
        {
            log.LogMethodEntry(productMenuIdList);
            List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList = new List<ProductMenuPanelContainerDTO>();
            foreach (var menuId in productMenuIdList)
            {
                if (menuIdReferencedPanelIdHashSet.ContainsKey(menuId) == false ||
                    menuIdMainPanelIdHashSet.ContainsKey(menuId) == false ||
                    productMenuIdProductMenuDTODictionary.ContainsKey(menuId) == false)
                {
                    continue;
                }
                foreach (var panelId in menuIdReferencedPanelIdHashSet[menuId])
                {
                    if (panelIdProductMenuPanelContainerDTODictionary.ContainsKey(panelId) == false)
                    {
                        continue;
                    }
                    ProductMenuPanelContainerDTO productMenuPanelContainerDTO = new ProductMenuPanelContainerDTO(panelIdProductMenuPanelContainerDTODictionary[panelId]);
                    productMenuPanelContainerDTO.IsMainPanel = menuIdMainPanelIdHashSet[menuId].Contains(panelId);
                    productMenuPanelContainerDTOList.Add(productMenuPanelContainerDTO);
                }
            }
            log.LogMethodExit(productMenuPanelContainerDTOList);
            return productMenuPanelContainerDTOList;
        }

        public List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(List<int> productMenuIdList, List<int> excludedPanelIdList, int posTypeId, int languageId)
        {
            log.LogMethodEntry(productMenuIdList, excludedPanelIdList);
            List<ProductMenuPanelContainerDTO> result = GetProductMenuPanelContainerDTOList(productMenuIdList);
            result = RemovePOSTypeSpecificProducts(result, posTypeId);
            result = RemovePanels(result, excludedPanelIdList);
            result = RemoveNonReferencedPanels(result);
            result = RemoveEmptyPanels(result);
            result = TranslatePanels(result, languageId);
            log.LogMethodExit(result);
            return result;
        }

        private List<ProductMenuPanelContainerDTO> TranslatePanels(List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList, int languageId)
        {
            log.LogMethodEntry(productMenuPanelContainerDTOList, languageId);
            if (languageId <= -1)
            {
                log.LogMethodExit(productMenuPanelContainerDTOList, "languageId <= -1");
                return productMenuPanelContainerDTOList;
            }
            List<ProductMenuPanelContainerDTO> result = GetCopy(productMenuPanelContainerDTOList);
            foreach (var productMenuPanelContainerDTO in result)
            {
                productMenuPanelContainerDTO.Name = GetObjectTranslation(languageId, "PRODUCT_MENU_PANEL", "NAME", panelIdProductMenuPanelContainerDTODictionary[productMenuPanelContainerDTO.PanelId].Guid, productMenuPanelContainerDTO.Name);
                foreach (var productMenuPanelContentContainerDTO in productMenuPanelContainerDTO.ProductMenuPanelContentContainerDTOList)
                {
                    string name = productMenuPanelContentContainerDTO.Name;
                    if(productMenuPanelContentContainerDTO.ProductId > -1)
                    {
                        name = GetObjectTranslation(languageId, "PRODUCTS", "PRODUCT_NAME", GetProductsContainerDTOOrDefault(productMenuPanelContentContainerDTO.ProductId).Guid, name);
                    }
                    else if(productMenuPanelContentContainerDTO.ChildPanelId > -1)
                    {
                        name = GetObjectTranslation(languageId, "PRODUCT_MENU_PANEL", "NAME", panelIdProductMenuPanelContainerDTODictionary[productMenuPanelContentContainerDTO.ChildPanelId].Guid, name);
                    }
                    productMenuPanelContentContainerDTO.Name = name;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        protected virtual string GetObjectTranslation(int languageId, string tableObject, string element, string elementGuid, string defaultValue)
        {
            log.LogMethodEntry(languageId, tableObject, element, elementGuid, defaultValue);
            string result = ObjectTranslationContainerList.GetObjectTranslation(siteId, languageId, tableObject, element, elementGuid, defaultValue);
            log.LogMethodExit(result);
            return result;
        }

        private List<ProductMenuPanelContainerDTO> RemovePOSTypeSpecificProducts(List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList, int posTypeId)
        {
            log.LogMethodEntry(productMenuPanelContainerDTOList, posTypeId);
            if (posTypeId <= -1)
            {
                log.LogMethodExit(productMenuPanelContainerDTOList, "posTypeId <= -1");
                return productMenuPanelContainerDTOList;
            }
            List<ProductMenuPanelContainerDTO> result = GetCopy(productMenuPanelContainerDTOList);
            foreach (var productMenuPanelContainerDTO in result)
            {
                int noOfContentsRemoved = productMenuPanelContainerDTO.ProductMenuPanelContentContainerDTOList.RemoveAll(x => x.ProductId != -1 &&
                                                                                                                              GetProductsContainerDTOOrDefault(x.ProductId).POSTypeId != -1 &&
                                                                                                                              GetProductsContainerDTOOrDefault(x.ProductId).POSTypeId != posTypeId);
                if (noOfContentsRemoved > 0)
                {
                    log.Debug("Referenced Product content removed in panel :" + productMenuPanelContainerDTO.Name);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<ProductMenuPanelContainerDTO> RemovePanels(List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList, List<int> excludedPanelIdList)
        {
            log.LogMethodEntry(productMenuPanelContainerDTOList, excludedPanelIdList);
            if (excludedPanelIdList == null ||
                excludedPanelIdList.Any() == false)
            {
                log.LogMethodExit(productMenuPanelContainerDTOList, "excludedPanelIdList is empty");
                return productMenuPanelContainerDTOList;
            }
            List<ProductMenuPanelContainerDTO> result = GetCopy(productMenuPanelContainerDTOList);
            int noOfPanelsRemoved = result.RemoveAll(x => excludedPanelIdList.Contains(x.PanelId));
            log.LogVariableState("noOfPanelsRemoved", noOfPanelsRemoved);
            foreach (var productMenuPanelContainerDTO in result)
            {
                int noOfContentsRemoved = productMenuPanelContainerDTO.ProductMenuPanelContentContainerDTOList.RemoveAll(x => excludedPanelIdList.Contains(x.ChildPanelId));
                if (noOfContentsRemoved > 0)
                {
                    log.Debug("Referenced content removed in panel :" + productMenuPanelContainerDTO.Name);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<ProductMenuPanelContainerDTO> GetCopy(List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList)
        {
            log.LogMethodEntry(productMenuPanelContainerDTOList);
            List<ProductMenuPanelContainerDTO> result = new List<ProductMenuPanelContainerDTO>();
            if(productMenuPanelContainerDTOList != null && productMenuPanelContainerDTOList.Any())
            {
                foreach (var productMenuPanelContainerDTO in productMenuPanelContainerDTOList)
                {
                    result.Add(new ProductMenuPanelContainerDTO(productMenuPanelContainerDTO));
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<ProductMenuPanelContainerDTO> RemoveNonReferencedPanels(List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList)
        {
            log.LogMethodEntry(productMenuPanelContainerDTOList);
            List<ProductMenuPanelContainerDTO> result = GetCopy(productMenuPanelContainerDTOList);
            while(true)
            {
                List<int> nonReferencedPanelIdList = result.Where(x => x.IsMainPanel == false)
                                                           .Where(x => result.SelectMany(y => y.ProductMenuPanelContentContainerDTOList).Any(z => z.ChildPanelId == x.PanelId) == false)
                                                           .Select(x => x.PanelId)
                                                           .ToList();
                if(nonReferencedPanelIdList.Any() == false)
                {
                    break;
                }
                result = RemovePanels(result, nonReferencedPanelIdList);
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<ProductMenuPanelContainerDTO> RemoveEmptyPanels(List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList)
        {
            log.LogMethodEntry(productMenuPanelContainerDTOList);
            List<ProductMenuPanelContainerDTO> result = GetCopy(productMenuPanelContainerDTOList);
            while (true)
            {
                List<int> emptyPanels = result.Where(x => x.ProductMenuPanelContentContainerDTOList == null || x.ProductMenuPanelContentContainerDTOList.Any() == false)
                                                           .Select(x => x.PanelId)
                                                           .ToList();
                if (emptyPanels.Any() == false)
                {
                    break;
                }
                result = RemovePanels(result, emptyPanels);
            }
            log.LogMethodExit(result);
            return result;
        }

        public ProductMenuContainerSnapshotDTOCollection GetProductMenuContainerSnapshotDTOCollection(int posMachineId, int userRoleId, int languageId, string menuType, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(posMachineId, userRoleId, languageId, menuType, dateTimeRange);
            ProductMenuContainerCacheKey key = new ProductMenuContainerCacheKey(siteId, posMachineId, userRoleId, languageId, menuType, dateTimeRange);
            ProductMenuContainerSnapshotDTOCollection result = productMenuContainerSnapshotDTOCollectionCache.GetOrAdd(key, (k) => CreateProductMenuContainerSnapshotDTOCollection(posMachineId, userRoleId, languageId, menuType, dateTimeRange));
            log.LogMethodExit(result);
            return result;
        }

        public List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(int posMachineId, int userRoleId, int languageId, string menuType, DateTimeRange dateTimeRange, DateTime dateTime)
        {
            log.LogMethodEntry(posMachineId, userRoleId, languageId, menuType, dateTimeRange, dateTime);
            ProductMenuContainerSnapshotDTOCollection productMenuContainerSnapshotDTOCollection = GetProductMenuContainerSnapshotDTOCollection(posMachineId, userRoleId, languageId, menuType, dateTimeRange);
            List<ProductMenuPanelContainerDTO> result = new List<ProductMenuPanelContainerDTO>();
            if (productMenuContainerSnapshotDTOCollection == null ||
                productMenuContainerSnapshotDTOCollection.ProductMenuContainerSnapshotDTOList == null ||
                productMenuContainerSnapshotDTOCollection.ProductMenuContainerSnapshotDTOList.Any() == false)
            {
                log.LogMethodExit(result, "productMenuContainerSnapshotDTOCollection is empty");
                return result;
            }
            foreach (var productMenuContainerSnapshotDTO in productMenuContainerSnapshotDTOCollection.ProductMenuContainerSnapshotDTOList)
            {
                if (dateTime >= productMenuContainerSnapshotDTO.StartDateTime && dateTime < productMenuContainerSnapshotDTO.EndDateTime)
                {
                    result = productMenuContainerSnapshotDTO.ProductMenuPanelContainerDTOList;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private ProductMenuContainerSnapshotDTOCollection CreateProductMenuContainerSnapshotDTOCollection(int posMachineId, int userRoleId, int languageId, string menuType, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(posMachineId, userRoleId, languageId, menuType, dateTimeRange);
            List<ProductMenuContainerSnapshotDTO> productMenuContainerSnapshotDTOList = GetProductMenuContainerSnapshotDTOList(posMachineId, userRoleId, languageId, menuType, dateTimeRange);
            ProductMenuContainerSnapshotDTOCollection result = new ProductMenuContainerSnapshotDTOCollection(productMenuContainerSnapshotDTOList);
            log.LogMethodExit(result);
            return result;
        }

        private List<ProductMenuContainerSnapshotDTO> GetProductMenuContainerSnapshotDTOList(int posMachineId, int userRoleId, int languageId, string menuType, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(posMachineId, userRoleId, languageId, menuType, dateTimeRange);
            List<ProductMenuContainerSnapshotDTO> result = new List<ProductMenuContainerSnapshotDTO>();
            UserRoleContainerDTO userRoleContainerDTO = GetUserRoleContainerDTO(userRoleId);
            if (userRoleContainerDTO == null)
            {
                log.LogMethodExit(result, "userRoleContainerDTO empty");
                return result;
            }
            POSMachineContainerDTO pOSMachineContainerDTO = GetPOSMachineContainerDTO(posMachineId);
            if (pOSMachineContainerDTO == null ||
                pOSMachineContainerDTO.ProductMenuIdList == null ||
                pOSMachineContainerDTO.ProductMenuIdList.Any() == false)
            {
                log.LogMethodExit(result, "pOSMachineContainerDTO empty or pOSMachineContainerDTO.ProductMenuIdList is empty");
                return result;
            }
            List<int> productMenuIdList = pOSMachineContainerDTO.ProductMenuIdList.Where(x => productMenuIdProductMenuDTODictionary.ContainsKey(x) && productMenuIdProductMenuDTODictionary[x].Type == menuType).ToList();
            List<int> excludedPanelIdList = new List<int>();
            if (pOSMachineContainerDTO.ExcludedProductMenuPanelIdList != null)
            {
                excludedPanelIdList.AddRange(pOSMachineContainerDTO.ExcludedProductMenuPanelIdList);
            }
            if (userRoleContainerDTO.ExcludedProductMenuPanelIdList != null)
            {
                excludedPanelIdList.AddRange(userRoleContainerDTO.ExcludedProductMenuPanelIdList);
            }
            ContinuousDateTimeRanges continuousDateTimeRangesBasedOnProductMenu = SplitDateTimeRangeBasedOnProductMenu(dateTimeRange, productMenuIdList);
            foreach (var subDateTimeRange in continuousDateTimeRangesBasedOnProductMenu.DateTimeRanges)
            {
                List<int> activeProductMenuIdList = GetActiveProductMenuIdList(productMenuIdList, subDateTimeRange);
                List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList = GetProductMenuPanelContainerDTOList(activeProductMenuIdList, excludedPanelIdList, pOSMachineContainerDTO.POSTypeId, languageId);
                ContinuousDateTimeRanges continuousDateTimeRangesBasedOnProductCalendar = SplitDateTimeRangeBasedOnProductCalendar(productMenuPanelContainerDTOList, subDateTimeRange);
                foreach (var range in continuousDateTimeRangesBasedOnProductCalendar.DateTimeRanges)
                {
                    ProductMenuContainerSnapshotDTO productMenuContainerSnapshotDTO = GetProductMenuContainerSnapshotDTO(productMenuPanelContainerDTOList, range);
                    result.Add(productMenuContainerSnapshotDTO);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private ProductMenuContainerSnapshotDTO GetProductMenuContainerSnapshotDTO(List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(productMenuPanelContainerDTOList, dateTimeRange);
            ProductMenuContainerSnapshotDTO productMenuContainerSnapshotDTO = new ProductMenuContainerSnapshotDTO(dateTimeRange.StartDateTime, dateTimeRange.EndDateTime);
            List<ProductMenuPanelContainerDTO> filteredProductMenuPanelContainerDTOList = RemoveNonAvailableProducts(productMenuPanelContainerDTOList, dateTimeRange.StartDateTime);
            filteredProductMenuPanelContainerDTOList = RemoveEmptyPanels(filteredProductMenuPanelContainerDTOList);
            productMenuContainerSnapshotDTO.ProductMenuPanelContainerDTOList = filteredProductMenuPanelContainerDTOList;
            log.LogMethodExit(productMenuContainerSnapshotDTO);
            return productMenuContainerSnapshotDTO;
        }

        private List<ProductMenuPanelContainerDTO> RemoveNonAvailableProducts(List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList, DateTime startDateTime)
        {
            List<ProductMenuPanelContainerDTO> result = GetCopy(productMenuPanelContainerDTOList);
            foreach (var productMenuPanelContainerDTO in result)
            {
                int noOfContentsRemoved = productMenuPanelContainerDTO.ProductMenuPanelContentContainerDTOList.RemoveAll(x => x.ProductId != -1 && IsProductAvailableOn(x.ProductId, startDateTime) == false);
                if (noOfContentsRemoved > 0)
                {
                    log.Debug("Non Available product content removed in panel :" + productMenuPanelContainerDTO.Name);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        protected virtual bool IsProductAvailableOn(int productId, DateTime startDateTime)
        {
            log.LogMethodEntry(productId, startDateTime);
            bool result = ProductsContainerList.IsProductAvailable(siteId, productId, startDateTime);
            log.LogMethodExit(result);
            return result;
        }

        private ContinuousDateTimeRanges SplitDateTimeRangeBasedOnProductCalendar(List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList, DateTimeRange subDateTimeRange)
        {
            log.LogMethodEntry(productMenuPanelContainerDTOList, subDateTimeRange);
            ContinuousDateTimeRanges continuousDateTimeRanges = subDateTimeRange;
            foreach (var productMenuPanelContentContainerDTO in productMenuPanelContainerDTOList.SelectMany(x => x.ProductMenuPanelContentContainerDTOList))
            {
                if(productMenuPanelContentContainerDTO.ProductId <= -1)
                {
                    continue;
                }
                ProductCalendarContainerDTO productCalendarContainerDTO = GetProductCalendarContainerDTO(productMenuPanelContentContainerDTO.ProductId, subDateTimeRange);
                foreach (var productCalendarDetailContainerDTO in productCalendarContainerDTO.ProductCalendarDetailContainerDTOList)
                {
                    continuousDateTimeRanges = continuousDateTimeRanges.Split(productCalendarDetailContainerDTO.StartDateTime, TimeSpan.FromMinutes(5));
                    continuousDateTimeRanges = continuousDateTimeRanges.Split(productCalendarDetailContainerDTO.EndDateTime, TimeSpan.FromMinutes(5));
                }
            }
            log.LogMethodExit(continuousDateTimeRanges);
            return continuousDateTimeRanges;
        }

        protected virtual ProductCalendarContainerDTO GetProductCalendarContainerDTO(int productId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(productId, dateTimeRange);
            ProductCalendarContainerDTO result = ProductsContainerList.GetProductCalendarContainerDTO(siteId, productId, dateTimeRange);
            log.LogMethodExit(result);
            return result;
        }

        private ContinuousDateTimeRanges SplitDateTimeRangeBasedOnProductMenu(DateTimeRange dateTimeRange, List<int> productMenuIdList)
        {
            log.LogMethodEntry(dateTimeRange, productMenuIdList);
            ContinuousDateTimeRanges continuousDateTimeRanges = dateTimeRange;
            foreach (var productMenuId in productMenuIdList)
            {
                if (productMenuIdProductMenuDTODictionary.ContainsKey(productMenuId) == false)
                {
                    continue;
                }
                ProductMenuDTO productMenuDTO = productMenuIdProductMenuDTODictionary[productMenuId];
                if (productMenuDTO.StartDate.HasValue)
                {
                    continuousDateTimeRanges = continuousDateTimeRanges.Split(productMenuDTO.StartDate.Value, TimeSpan.FromMinutes(5));
                }
                if (productMenuDTO.EndDate.HasValue)
                {
                    continuousDateTimeRanges = continuousDateTimeRanges.Split(productMenuDTO.EndDate.Value, TimeSpan.FromMinutes(5));
                }
            }
            log.LogMethodExit(continuousDateTimeRanges);
            return continuousDateTimeRanges;
        }

        private List<int> GetActiveProductMenuIdList(List<int> productMenuIdList, DateTimeRange subDateTimeRange)
        {
            log.LogMethodEntry(productMenuIdList, subDateTimeRange);
            List<int> result = new List<int>();
            foreach (var productMenuId in productMenuIdList)
            {
                if (productMenuIdProductMenuDTODictionary.ContainsKey(productMenuId) == false)
                {
                    continue;
                }
                ProductMenuDTO productMenuDTO = productMenuIdProductMenuDTODictionary[productMenuId];
                if (productMenuDTO.StartDate.HasValue && subDateTimeRange.EndDateTime <= productMenuDTO.StartDate.Value)
                {
                    continue;
                }
                if (productMenuDTO.EndDate.HasValue && subDateTimeRange.StartDateTime >= productMenuDTO.EndDate.Value)
                {
                    continue;
                }
                result.Add(productMenuId);
            }
            log.LogMethodExit(result);
            return result;
        }

        protected virtual UserRoleContainerDTO GetUserRoleContainerDTO(int userRoleId)
        {
            log.LogMethodEntry(userRoleId);
            UserRoleContainerDTO result = UserRoleContainerList.GetUserRoleContainerDTOOrDefault(siteId, userRoleId);
            log.LogMethodExit(result);
            return result;
        }

        protected virtual POSMachineContainerDTO GetPOSMachineContainerDTO(int posMachineId)
        {
            log.LogMethodEntry(posMachineId);
            POSMachineContainerDTO result = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(siteId, posMachineId);
            log.LogMethodExit(result);
            return result;
        }

        
    }
}
