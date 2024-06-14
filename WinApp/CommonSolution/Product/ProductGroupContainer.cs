/********************************************************************************************
 * Project Name - Product
 * Description  - ProductGroupContainer class to get the data from the container API
 *  
 **************
 **Version Log
 **************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.170.0      07-Jul-2023      Lakshminarayana            Created 
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Parafait.Product
{
    public class ProductGroupContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, ProductGroupContainerDTO> productGroupIdProductGroupContainerDTODictionary = new Dictionary<int, ProductGroupContainerDTO>();
        private readonly Dictionary<int, HashSet<int>> productGroupIdProductIdHashSetDictionary = new Dictionary<int, HashSet<int>>();
        private readonly Dictionary<string, ProductGroupContainerDTO> productGroupGuidProductGroupContainerDTODictionary = new Dictionary<string, ProductGroupContainerDTO>(StringComparer.InvariantCultureIgnoreCase);
        private readonly ProductGroupContainerDTOCollection productGroupContainerDTOCollection;
        private readonly DateTime? productGroupModuleLastUpdateTime;
        private readonly int siteId;
        public ProductGroupContainer(int siteId) : this(siteId, 
                                                        GetProductGroupDTOList(siteId), 
                                                        GetProductGroupModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public ProductGroupContainer(int siteId, 
                                     List<ProductGroupDTO> productGroupDTOList, 
                                     DateTime? productGroupModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId, productGroupDTOList, productGroupModuleLastUpdateTime);
            this.siteId = siteId;
            this.productGroupModuleLastUpdateTime = productGroupModuleLastUpdateTime;
            List<ProductGroupContainerDTO> productGroupContainerDTOList = new List<ProductGroupContainerDTO>();
            foreach (ProductGroupDTO productGroupDTO in productGroupDTOList)
            {
                ProductGroupContainerDTO productGroupContainerDTO = new ProductGroupContainerDTO(productGroupDTO.Id, productGroupDTO.Name, productGroupDTO.Guid);
                List<ProductGroupMapContainerDTO> productGroupMapContainerDTOList = new List<ProductGroupMapContainerDTO>();
                productGroupIdProductIdHashSetDictionary.Add(productGroupDTO.Id, new HashSet<int>());
                foreach (var productGroupMapDTO in productGroupDTO.ProductGroupMapDTOList)
                {
                    ProductGroupMapContainerDTO productGroupMapContainerDTO = new ProductGroupMapContainerDTO(productGroupMapDTO.Id,
                                                                                                              productGroupMapDTO.ProductGroupId,
                                                                                                              productGroupMapDTO.ProductId,
                                                                                                              productGroupMapDTO.SortOrder);
                    productGroupMapContainerDTOList.Add(productGroupMapContainerDTO);
                    productGroupIdProductIdHashSetDictionary[productGroupDTO.Id].Add(productGroupMapDTO.ProductId);
                }
                productGroupContainerDTO.ProductGroupMapContainerDTOList = productGroupMapContainerDTOList;
                productGroupContainerDTOList.Add(productGroupContainerDTO);
                productGroupIdProductGroupContainerDTODictionary.Add(productGroupDTO.Id, productGroupContainerDTO);
                productGroupGuidProductGroupContainerDTODictionary.Add(productGroupDTO.Guid, productGroupContainerDTO);
                
            }
            productGroupContainerDTOCollection = new ProductGroupContainerDTOCollection(productGroupContainerDTOList);
            log.Info("Number of items loaded by ProductGroupContainer for site "+siteId+":" + productGroupDTOList.Count);
            log.LogMethodExit();
        }

        private static List<ProductGroupDTO> GetProductGroupDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ProductGroupDTO> productGroupDTOList = null;
            try
            {
                ProductGroupListBL productGroupListBL = new ProductGroupListBL();
                SearchParameterList<ProductGroupDTO.SearchByParameters> searchParameters = new SearchParameterList<ProductGroupDTO.SearchByParameters>();
                searchParameters.Add(ProductGroupDTO.SearchByParameters.IS_ACTIVE, "1");
                searchParameters.Add(ProductGroupDTO.SearchByParameters.SITE_ID, siteId.ToString());
                productGroupDTOList = productGroupListBL.GetProductGroupDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the product group.", ex);
            }

            if (productGroupDTOList == null)
            {
                productGroupDTOList = new List<ProductGroupDTO>();
            }
            log.LogMethodExit(productGroupDTOList);
            return productGroupDTOList;
        }

        public List<ProductGroupContainerDTO> GetProductGroupContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(productGroupContainerDTOCollection.ProductGroupContainerDTOList);
            return productGroupContainerDTOCollection.ProductGroupContainerDTOList;
        }

        private static DateTime? GetProductGroupModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                ProductGroupListBL productGroupListBL = new ProductGroupListBL();
                result = productGroupListBL.GetProductGroupModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the product group max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public ProductGroupContainerDTO GetProductGroupContainerDTO(int productGroupId)
        {
            log.LogMethodEntry(productGroupId);
            if (productGroupIdProductGroupContainerDTODictionary.ContainsKey(productGroupId) == false)
            {
                string errorMessage = "product group with id :" + productGroupId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ProductGroupContainerDTO result = productGroupIdProductGroupContainerDTODictionary[productGroupId]; ;
            log.LogMethodExit(result);
            return result;
        }

        public HashSet<int> GetRefferedProductIdHashSet(int productGroupId)
        {
            log.LogMethodEntry(productGroupId);
            HashSet<int> result = new HashSet<int>();
            if (productGroupIdProductIdHashSetDictionary.ContainsKey(productGroupId))
            {
                result.UnionWith(productGroupIdProductIdHashSetDictionary[productGroupId]);
            }
            log.LogMethodExit(result);
            return result;
        }

        public ProductGroupContainerDTO GetProductGroupContainerDTOOrDefault(int productGroupId)
        {
            log.LogMethodEntry(productGroupId);
            ProductGroupContainerDTO result = null;
            if (productGroupIdProductGroupContainerDTODictionary.ContainsKey(productGroupId) == false)
            {
                log.LogMethodExit(result, "product group with id :" + productGroupId + " doesn't exists.");
                return result;
            }
            result = productGroupIdProductGroupContainerDTODictionary[productGroupId]; ;
            log.LogMethodExit(result);
            return result;
        }

        public ProductGroupContainerDTO GetProductGroupContainerDTO(string productGroupGuid)
        {
            log.LogMethodEntry(productGroupGuid);
            if (productGroupGuidProductGroupContainerDTODictionary.ContainsKey(productGroupGuid) == false)
            {
                string errorMessage = "product group with guid :" + productGroupGuid + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ProductGroupContainerDTO result = productGroupGuidProductGroupContainerDTODictionary[productGroupGuid]; ;
            log.LogMethodExit(result);
            return result;
        }

        public ProductGroupContainerDTO GetProductGroupContainerDTOOrDefault(string productGroupGuid)
        {
            log.LogMethodEntry(productGroupGuid);
            ProductGroupContainerDTO result = null;
            if (productGroupGuidProductGroupContainerDTODictionary.ContainsKey(productGroupGuid) == false)
            {
                log.LogMethodExit(result, "product group with guid :" + productGroupGuid + " doesn't exists.");
                return result;
            }
            result = productGroupGuidProductGroupContainerDTODictionary[productGroupGuid]; ;
            log.LogMethodExit(result);
            return result;
        }

        public ProductGroupContainerDTOCollection GetProductGroupContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(productGroupContainerDTOCollection);
            return productGroupContainerDTOCollection;
        }

        public ProductGroupContainer Refresh()
        {
            log.LogMethodEntry();
            ProductGroupListBL productGroupListBL = new ProductGroupListBL();
            DateTime? updateTime = GetProductGroupModuleLastUpdateTime(siteId);
            if (productGroupModuleLastUpdateTime.HasValue
                && productGroupModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in product group since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ProductGroupContainer result = new ProductGroupContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

    }
}
