/* Project Name - Semnox.Parafait.Product.ProductsAllowedInFacilityMapBL 
* Description  - BL class for ProductsAllowedInFacilityMap
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70        13-Mar-2019    Guru S A             Created for Booking phase 2 enhancement changes 
*2.70        05-Jul-2019    Akshay G             Added IsExistInAllowedProducts(), CheckForDuplicateDefaultRentalProduct() and Delete() methods
*2.80.3      26-Feb-2020    Girish Kundar        Modified : 3 Tier Changes for API
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// BL class for ProductsAllowedInFacilityMap
    /// </summary>
    public class ProductsAllowedInFacilityMapBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductsAllowedInFacilityMapDTO productsAllowedInFacilityMapDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of ProductsAllowedInFacilityMapBL class
        /// </summary>
        public ProductsAllowedInFacilityMapBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            productsAllowedInFacilityMapDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the productsAllowedInFacilityMap id as the parameter
        /// Would fetch the productsAllowedInFacilityMap object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public ProductsAllowedInFacilityMapBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ProductsAllowedInFacilityMapDatahandler productsAllowedInFacilityDatahandler = new ProductsAllowedInFacilityMapDatahandler(sqlTransaction);
            productsAllowedInFacilityMapDTO = productsAllowedInFacilityDatahandler.GetProductsAllowedInFacilityMapDTO(id);
            if (productsAllowedInFacilityMapDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductsAllowedInFacilityMap", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ProductsAllowedInFacilityMapBL object using the ProductsAllowedInFacilityMapDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="productsAllowedInFacilityMapDTO">ProductsAllowedInFacilityMapDTO object</param>
        public ProductsAllowedInFacilityMapBL(ExecutionContext executionContext, ProductsAllowedInFacilityMapDTO productsAllowedInFacilityMapDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productsAllowedInFacilityMapDTO);
            this.productsAllowedInFacilityMapDTO = productsAllowedInFacilityMapDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether the allowed products is exist or not
        /// </summary>
        /// <param name="allowedProductId"></param>
        /// <param name="facilityMapId"></param>
        /// <param name="productId"></param>
        private void IsExistInAllowedProducts(int allowedProductId, int facilityMapId, int productId)
        {
            log.LogMethodEntry(allowedProductId, facilityMapId, productId);
            ProductsAllowedInFacilityMapListBL productsAllowedInFacilityListBL = new ProductsAllowedInFacilityMapListBL(executionContext);
            List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>
            {
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ID, productId.ToString())
            };
            List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityDTOList = productsAllowedInFacilityListBL.GetProductsAllowedInFacilityMapDTOList(searchParameters);

            if (allowedProductId != -1 && productsAllowedInFacilityDTOList != null && productsAllowedInFacilityDTOList.Count > 0)
            {
                productsAllowedInFacilityDTOList = productsAllowedInFacilityDTOList.Where(prod => prod.ProductsAllowedInFacilityMapId != allowedProductId).ToList();
            }
            if (productsAllowedInFacilityDTOList != null && productsAllowedInFacilityDTOList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2130));//Product is already mapped to the facility. Edit the record instead of adding it as new record
            }
            log.LogMethodExit();
        }
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (productsAllowedInFacilityMapDTO.FacilityMapId == -1)
            {
                validationErrorList.Add(new ValidationError("FacilityMap", "FacilityMapId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Virtual Facility Id"))));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Checks for duplicate rental product
        /// </summary>
        /// <param name="allowedProductId"></param>
        /// <param name="facilityMapId"></param>
        private void CheckForDuplicateDefaultRentalProduct(int allowedProductId, int facilityMapId)
        {
            log.LogMethodEntry(allowedProductId, facilityMapId);
            ProductsAllowedInFacilityMapListBL productsAllowedInFacilityListBL = new ProductsAllowedInFacilityMapListBL(executionContext);
            List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>
            {
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.DEFAULT_RENTAL_PRODUCT, "1"),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE, "1")
            };
            List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityDTOList = productsAllowedInFacilityListBL.GetProductsAllowedInFacilityMapDTOList(searchParameters);

            if (allowedProductId != -1 && productsAllowedInFacilityDTOList != null && productsAllowedInFacilityDTOList.Count > 0)
            {
                productsAllowedInFacilityDTOList = productsAllowedInFacilityDTOList.Where(prod => prod.ProductsAllowedInFacilityMapId != allowedProductId).ToList();
            }
            if (productsAllowedInFacilityDTOList != null && productsAllowedInFacilityDTOList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2129));// "There can be only one default rental product. Uncheck the existing option and save the changes before picking new default rental product"
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ProductsAllowedInFacilityMap
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (productsAllowedInFacilityMapDTO.IsActive)
                {
                    IsExistInAllowedProducts(productsAllowedInFacilityMapDTO.ProductsAllowedInFacilityMapId, productsAllowedInFacilityMapDTO.FacilityMapId, productsAllowedInFacilityMapDTO.ProductsId);
                    if (productsAllowedInFacilityMapDTO.DefaultRentalProduct)
                    {
                        CheckForDuplicateDefaultRentalProduct(productsAllowedInFacilityMapDTO.ProductsAllowedInFacilityMapId, productsAllowedInFacilityMapDTO.FacilityMapId);
                    }
                }
            }
            catch (ValidationException ex)
            {
                throw ex;
            }
            ProductsAllowedInFacilityMapDatahandler productsAllowedInFacilityDatahandler = new ProductsAllowedInFacilityMapDatahandler(sqlTransaction);
            if (productsAllowedInFacilityMapDTO.ProductsAllowedInFacilityMapId < 0)
            {
                productsAllowedInFacilityMapDTO = productsAllowedInFacilityDatahandler.InsertProductsAllowedInFacilityMap(productsAllowedInFacilityMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(productsAllowedInFacilityMapDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("ProductsAllowedInFacility", productsAllowedInFacilityMapDTO.Guid, sqlTransaction);
                }
                productsAllowedInFacilityMapDTO.AcceptChanges();
            }
            else
            {
                if (productsAllowedInFacilityMapDTO.IsChanged)
                {
                    productsAllowedInFacilityMapDTO = productsAllowedInFacilityDatahandler.UpdateProductsAllowedInFacilityMap(productsAllowedInFacilityMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(productsAllowedInFacilityMapDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("ProductsAllowedInFacility", productsAllowedInFacilityMapDTO.Guid, sqlTransaction);
                    }
                    productsAllowedInFacilityMapDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductsAllowedInFacilityMapDTO ProductsAllowedInFacilityMapDTO
        {
            get
            {
                return productsAllowedInFacilityMapDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ProductsAllowedInFacilityMapListBL
    /// </summary>
    public class ProductsAllowedInFacilityMapListBL
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTOList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ProductsAllowedInFacilityMapListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public ProductsAllowedInFacilityMapListBL(ExecutionContext executionContext, List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTOList)
        {
            log.LogMethodEntry(executionContext, productsAllowedInFacilityMapDTOList);
            this.executionContext = executionContext;
            this.productsAllowedInFacilityMapDTOList = productsAllowedInFacilityMapDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetProductsAllowedInFacilityMapDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<ProductsAllowedInFacilityMapDTO></returns>
        public List<ProductsAllowedInFacilityMapDTO> GetProductsAllowedInFacilityMapDTOList(List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters, bool loadProductsDTO = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadProductsDTO, sqlTransaction);
            ProductsAllowedInFacilityMapDatahandler productsAllowedInFacilityDatahandler = new ProductsAllowedInFacilityMapDatahandler(sqlTransaction);
            List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTOList = productsAllowedInFacilityDatahandler.GetProductsAllowedInFacilityMapDTOList(searchParameters);
            if (loadProductsDTO && productsAllowedInFacilityMapDTOList != null && productsAllowedInFacilityMapDTOList.Count > 0)
            {
                string productListId = " ";
                int count = 0;
                foreach (ProductsAllowedInFacilityMapDTO productsAllowedInFacilityMapDTO in productsAllowedInFacilityMapDTOList)
                {
                    if (count < productsAllowedInFacilityMapDTOList.Count - 1)
                    {
                        productListId = productListId + productsAllowedInFacilityMapDTO.ProductsId.ToString() + ", ";
                    }
                    else
                    {
                        productListId = productListId + productsAllowedInFacilityMapDTO.ProductsId.ToString();
                    }
                    count++;
                }
                ProductsList productsList = new ProductsList(executionContext);
                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchPara = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                searchPara.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID_LIST, productListId));
                searchPara.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchPara, true);
                if (productsDTOList != null)
                {
                    foreach (ProductsAllowedInFacilityMapDTO item in productsAllowedInFacilityMapDTOList)
                    {
                        item.ProductsDTO = productsDTOList.Find(p => p.ProductId == item.ProductsId);
                    }
                }
            }
            log.LogMethodExit(productsAllowedInFacilityMapDTOList);
            return productsAllowedInFacilityMapDTOList;
        }

        /// <summary>
        /// Saves the ProductsAllowedInFacilityMapDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ValidationException validationException = null;
            if (productsAllowedInFacilityMapDTOList != null && productsAllowedInFacilityMapDTOList.Count != 0)
            {
                foreach (ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO in productsAllowedInFacilityMapDTOList)
                {
                    if (productsAllowedInFacilityDTO.ProductsId == -1)
                    {
                        continue;
                    }
                    if (productsAllowedInFacilityDTO.IsChanged)
                    {
                        try
                        {
                            ProductsAllowedInFacilityMapBL productsAllowedInFacilityBL = new ProductsAllowedInFacilityMapBL(executionContext, productsAllowedInFacilityDTO);
                            productsAllowedInFacilityBL.Save(sqlTransaction);
                        }
                        catch (ValidationException ex)
                        {
                            validationException = ex;
                            continue;
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred while saving ProductsAllowedInFacilityDTO.", ex);
                            log.LogVariableState("ProductsAllowedInFacilityDTO", productsAllowedInFacilityDTO);
                            throw;
                        }
                    }
                }
                if (validationException != null)
                {
                    log.LogMethodEntry(validationException);
                    throw validationException;
                }
            }
            log.LogMethodExit();
        }
    }
}

