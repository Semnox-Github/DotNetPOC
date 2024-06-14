/********************************************************************************************
 * Project Name - Product
 * Description  - ProductTypeContainer class to get the data from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.1      24-Jun-2021      Abhishek           Created : POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ProductTypeContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, ProductTypeContainerDTO> productTypeContainerDTODictionary = new Dictionary<int, ProductTypeContainerDTO>();
        private readonly ProductTypeContainerDTOCollection productTypeContainerDTOCollection;
        private readonly DateTime? productTypeModuleLastUpdateTime;
        private readonly int siteId;
        private readonly List<ProductTypeDTO> productTypeDTOList;

        public ProductTypeContainer(int siteId) : this(siteId, GetProductTypeDTOList(siteId), GetProductTypeModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public ProductTypeContainer(int siteId, List<ProductTypeDTO> productTypeDTOList, DateTime? productTypeModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.productTypeDTOList = productTypeDTOList;
            this.productTypeModuleLastUpdateTime = productTypeModuleLastUpdateTime;
            List<ProductTypeContainerDTO> productTypeContainerDTOList = new List<ProductTypeContainerDTO>();
            foreach (ProductTypeDTO productTypeDTO in productTypeDTOList)
            {
                ProductTypeContainerDTO productTypeContainerDTO = new ProductTypeContainerDTO(productTypeDTO.ProductTypeId, productTypeDTO.ProductType, productTypeDTO.Description,
                                                                    productTypeDTO.CardSale, productTypeDTO.ReportGroup, productTypeDTO.OrderTypeId);
                productTypeContainerDTOList.Add(productTypeContainerDTO);
                productTypeContainerDTODictionary.Add(productTypeDTO.ProductTypeId, productTypeContainerDTO);

            }
            productTypeContainerDTOCollection = new ProductTypeContainerDTOCollection(productTypeContainerDTOList);
            log.LogMethodExit();
        }

        private static List<ProductTypeDTO> GetProductTypeDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ProductTypeDTO> productTypeDTOList = null;
            try
            {
                ProductTypeListBL productTypeListBL = new ProductTypeListBL();
                List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                productTypeDTOList = productTypeListBL.GetProductTypeDTOList(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the product type.", ex);
            }

            if (productTypeDTOList == null)
            {
                productTypeDTOList = new List<ProductTypeDTO>();
            }
            log.LogMethodExit(productTypeDTOList);
            return productTypeDTOList;
        }

        private static DateTime? GetProductTypeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                ProductTypeListBL productTypeListBL = new ProductTypeListBL();
                result = productTypeListBL.GetProductTypeModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the product type max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public ProductTypeContainerDTO GetProductTypeContainerDTO(int productTypeId)
        {
            log.LogMethodEntry(productTypeId);
            if (productTypeContainerDTODictionary.ContainsKey(productTypeId) == false)
            {
                string errorMessage = "product type with Product Type Id :" + productTypeId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ProductTypeContainerDTO result = productTypeContainerDTODictionary[productTypeId]; ;
            log.LogMethodExit(result);
            return result;
        }

        public ProductTypeContainerDTOCollection GetProductTypeContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(productTypeContainerDTOCollection);
            return productTypeContainerDTOCollection;
        }

        public ProductTypeContainer Refresh()
        {
            log.LogMethodEntry();
            ProductTypeListBL productTypeListBL = new ProductTypeListBL();
            DateTime? updateTime = productTypeListBL.GetProductTypeModuleLastUpdateTime(siteId);
            if (productTypeModuleLastUpdateTime.HasValue
                && productTypeModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in product type since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ProductTypeContainer result = new ProductTypeContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
