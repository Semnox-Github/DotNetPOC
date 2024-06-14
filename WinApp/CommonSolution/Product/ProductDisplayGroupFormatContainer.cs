/********************************************************************************************
 * Project Name - Product
 * Description  - ProductDisplayGroupFormat class to get the data from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.0      25-Jun-2021      Abhishek                  Created : POS UI Redesign 
 2.130.11     13-Oct-2022      Vignesh Bhat              Modified to support BackgroundImageFileName property
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ProductDisplayGroupFormatContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, ProductDisplayGroupFormatContainerDTO> productDisplayGroupFormatContainerDTODictionary = new Dictionary<int, ProductDisplayGroupFormatContainerDTO>();
        private readonly ProductDisplayGroupFormatContainerDTOCollection productDisplayGroupFormatContainerDTOCollection;
        private readonly DateTime? productDisplayGroupFormatModuleLastUpdateTime;
        private readonly int siteId;
        private readonly List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatList;

        public ProductDisplayGroupFormatContainer(int siteId) : this(siteId, GetProductDisplayGroupFormatDTOList(siteId), GetProductDisplayGroupFormatModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public ProductDisplayGroupFormatContainer(int siteId, List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList, DateTime? productDisplayGroupFormatModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.productDisplayGroupFormatList = productDisplayGroupFormatDTOList;
            this.productDisplayGroupFormatModuleLastUpdateTime = productDisplayGroupFormatModuleLastUpdateTime;
            List<ProductDisplayGroupFormatContainerDTO> productDisplayGroupFormatContainerDTOList = new List<ProductDisplayGroupFormatContainerDTO>();
            foreach (ProductDisplayGroupFormatDTO productDisplayGroupFormatDTO in productDisplayGroupFormatDTOList)
            {
                ProductDisplayGroupFormatContainerDTO productDisplayGroupFormatContainerDTO = CreateProductDisplayGroupFormatContainerDTO(productDisplayGroupFormatDTO);
                productDisplayGroupFormatContainerDTOList.Add(productDisplayGroupFormatContainerDTO);
                productDisplayGroupFormatContainerDTODictionary.Add(productDisplayGroupFormatDTO.Id, productDisplayGroupFormatContainerDTO);

            }
            productDisplayGroupFormatContainerDTOCollection = new ProductDisplayGroupFormatContainerDTOCollection(productDisplayGroupFormatContainerDTOList);
            log.LogMethodExit();
        }

        private ProductDisplayGroupFormatContainerDTO CreateProductDisplayGroupFormatContainerDTO(ProductDisplayGroupFormatDTO productDisplayGroupFormatDTO)
        {
            log.LogMethodEntry(productDisplayGroupFormatDTO);
            ProductDisplayGroupFormatContainerDTO productDisplayGroupFormatContainerDTO = new ProductDisplayGroupFormatContainerDTO(productDisplayGroupFormatDTO.Id, productDisplayGroupFormatDTO.DisplayGroup, productDisplayGroupFormatDTO.SortOrder,
                                                                    productDisplayGroupFormatDTO.ImageFileName, productDisplayGroupFormatDTO.ButtonColor, productDisplayGroupFormatDTO.TextColor, productDisplayGroupFormatDTO.Font, productDisplayGroupFormatDTO.Description, productDisplayGroupFormatDTO.ExternalSourceReference, productDisplayGroupFormatDTO.BackgroundImageFileName);
            if (productDisplayGroupFormatDTO.ProductDisplayGroupList != null &&
               productDisplayGroupFormatDTO.ProductDisplayGroupList.Any())
            {
                foreach (var productDisplayGroup in productDisplayGroupFormatDTO.ProductDisplayGroupList)
                {
                    productDisplayGroupFormatContainerDTO.ProductsDisplayGroupContainerDTOList.Add(new ProductsDisplayGroupContainerDTO(productDisplayGroup.Id,
                                                                                                           productDisplayGroup.DisplayGroupId, productDisplayGroup.DisplayGroupName));
                }
                productDisplayGroupFormatContainerDTO.ProductIdList = productDisplayGroupFormatDTO.ProductDisplayGroupList.Where(x => x.DisplayGroupId == productDisplayGroupFormatDTO.Id).Select(x => x.ProductId).ToList();
            }
            log.LogMethodExit(productDisplayGroupFormatContainerDTO);
            return productDisplayGroupFormatContainerDTO;
        }

        private static List<ProductDisplayGroupFormatDTO> GetProductDisplayGroupFormatDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList = null;
            try
            {
                ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList();
                List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParameters = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, siteId.ToString()));
                productDisplayGroupFormatDTOList = productDisplayGroupList.GetAllProductDisplayGroup(searchParameters,true,true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the product display group.", ex);
            }

            if (productDisplayGroupFormatDTOList == null)
            {
                productDisplayGroupFormatDTOList = new List<ProductDisplayGroupFormatDTO>();
            }
            log.LogMethodExit(productDisplayGroupFormatDTOList);
            return productDisplayGroupFormatDTOList;
        }

        private static DateTime? GetProductDisplayGroupFormatModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList();
                result = productDisplayGroupList.GetProductDisplayGroupFormatModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the product display group max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public ProductDisplayGroupFormatContainerDTO GetProductDisplayGroupFormatContainerDTO(int id)
        {
            log.LogMethodEntry(id);
            if (productDisplayGroupFormatContainerDTODictionary.ContainsKey(id) == false)
            {
                string errorMessage = "product display group format with id :" + id + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ProductDisplayGroupFormatContainerDTO result = productDisplayGroupFormatContainerDTODictionary[id]; ;
            log.LogMethodExit(result);
            return result;
        }

        public ProductDisplayGroupFormatContainerDTOCollection GetProductDisplayGroupFormatContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(productDisplayGroupFormatContainerDTOCollection);
            return productDisplayGroupFormatContainerDTOCollection;
        }

        public ProductDisplayGroupFormatContainer Refresh()
        {
            log.LogMethodEntry();
            ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList();
            DateTime? updateTime = productDisplayGroupList.GetProductDisplayGroupFormatModuleLastUpdateTime(siteId);
            if (productDisplayGroupFormatModuleLastUpdateTime.HasValue
                && productDisplayGroupFormatModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in product display group format since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ProductDisplayGroupFormatContainer result = new ProductDisplayGroupFormatContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
