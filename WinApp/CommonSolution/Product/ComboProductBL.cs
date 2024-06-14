/* Project Name - Semnox.Parafait.Product.ComboProduct 
* Description  - Business call object of the ComboProduct
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
*2.60        15-Feb-2019    Nagesh Badiger       Added SaveUpdateComboProductList() and constructor in ComboProductList class
*2.70        02-Jul-2019    Indrajeet Kumar      Created DeleteComboProductList() & DeleteComboProduct() method for Hard Deletion
*2.70.2      15-July-2019   Jagan Mohana         Validation method has been implmeneted in save method
*2.150.0     15-Mar-2022    Girish Kundar        Modified :  Added Maximum Quanity column and validation
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
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
    /// ComboProductBL
    /// </summary>
    public class ComboProductBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ComboProductDTO comboProductDTO;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor of ComboProductBL class
        /// </summary>
        public ComboProductBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            comboProductDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the comboProduct id as the parameter
        /// Would fetch the comboProduct object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public ComboProductBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ComboProductDataHandler comboProductDataHandler = new ComboProductDataHandler(sqlTransaction);
            comboProductDTO = comboProductDataHandler.GetComboProductDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ComboProductBL object using the ComboProductDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="comboProductDTO">ComboProductDTO object</param>
        public ComboProductBL(ExecutionContext executionContext, ComboProductDTO comboProductDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, comboProductDTO);
            this.comboProductDTO = comboProductDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ComboProduct
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ComboProductDataHandler comboProductDataHandler = new ComboProductDataHandler(sqlTransaction);

            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }

            if (comboProductDTO.ComboProductId < 0)
            {
                int id = comboProductDataHandler.InsertComboProduct(comboProductDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                comboProductDTO.ComboProductId = id;
                comboProductDTO.AcceptChanges();
            }
            else
            {
                if (comboProductDTO.IsChanged)
                {
                    comboProductDataHandler.UpdateComboProduct(comboProductDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    comboProductDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the ComboProduct details based on comboProductId
        /// </summary>
        /// <param name="comboProductId"></param>        
        public void DeleteComboProduct(int comboProductId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(comboProductId);
            try
            {
                ComboProductDataHandler comboProductDataHandler = new ComboProductDataHandler(sqlTransaction);
                comboProductDataHandler.DeleteComboProduct(comboProductId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the customer ComboProduct DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string productType = string.Empty;
            List<int> childProductIdList = new List<int>();
            childProductIdList.Add(comboProductDTO.ProductId);
            List<int> childCategoryIdList = new List<int>();

            ComboProductList comboProductList = new ComboProductList(executionContext);
            List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>();
            if (comboProductDTO.ProductId != 0)
            {
                searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRODUCT_ID, comboProductDTO.ProductId.ToString()));
            }

            List<ComboProductDTO> comboProductDTOList = comboProductList.GetComboProductDTOList(searchParameters, sqlTransaction);
            if (comboProductDTOList != null && comboProductDTOList.Count != 0)
            {
                foreach (ComboProductDTO comboItemDTO in comboProductDTOList)
                {
                    childProductIdList.Add(comboItemDTO.ChildProductId);
                    if (comboItemDTO.CategoryId != -1)
                    {
                        childCategoryIdList.Add(comboItemDTO.CategoryId);
                    }
                    if (comboItemDTO.ComboProductId == -1)
                    {
                        childProductIdList.Add(comboItemDTO.ChildProductId);
                    }
                    if (string.IsNullOrEmpty(productType))
                    {
                        string productTypeId = string.Empty;
                        ProductsFilterParams productsFilterParams = new ProductsFilterParams();
                        productsFilterParams.SiteId = executionContext.GetSiteId();
                        productsFilterParams.ProductId = comboItemDTO.ProductId;
                        ProductsList productList = new ProductsList(executionContext, productType, productsFilterParams);
                        List<ProductsDTO> productsDTOList = productList.GetProductList();
                        if (productsDTOList != null && productsDTOList.Count != 0)
                        {
                            productTypeId = productsDTOList.Select(m => m.ProductTypeId).SingleOrDefault().ToString();
                        }
                        if (!string.IsNullOrEmpty(productTypeId))
                        {
                            List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchProductTypeParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>();
                            searchProductTypeParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                            searchProductTypeParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.PRODUCT_TYPE_ID, productTypeId));
                            ProductTypeListBL productTypeListBL = new ProductTypeListBL(executionContext);
                            List<ProductTypeDTO> productTypeDTOList = productTypeListBL.GetProductTypeDTOList(searchProductTypeParameters, sqlTransaction);
                            if (productTypeDTOList != null && productTypeDTOList.Count != 0)
                            {
                                string comboProductType = productTypeDTOList.Select(m => m.ProductType).SingleOrDefault();
                                if (comboProductType.ToUpper().ToString() == "COMBO")
                                {
                                    productType = "COMBO";
                                }
                                else if (comboProductType.ToUpper().ToString() == "BOOKINGS")
                                {
                                    productType = "BOOKINGS";
                                }
                                else
                                {
                                    productType = comboItemDTO.ChildProductType;
                                }
                            }
                        }
                    }
                }
                childProductIdList.Add(comboProductDTO.ChildProductId);
            }

            OrderTypeListBL orderTypeListBL = new OrderTypeListBL(executionContext);
            HashSet<int> orderTypeIdSet = new HashSet<int>();
            if (childProductIdList != null && childProductIdList.Count > 0)
            {
                HashSet<int> productOrderTypeIdSet = orderTypeListBL.GetOrderTypeIdSetOfProducts(childProductIdList, sqlTransaction);
                if (productOrderTypeIdSet != null)
                {
                    orderTypeIdSet.UnionWith(productOrderTypeIdSet);
                }
            }

            if (childCategoryIdList != null && childCategoryIdList.Count > 0)
            {
                HashSet<int> categoryOrderTypeIdSet = orderTypeListBL.GetOrderTypeIdSetOfCategories(childCategoryIdList, null, sqlTransaction);
                if (categoryOrderTypeIdSet != null)
                {
                    orderTypeIdSet.UnionWith(categoryOrderTypeIdSet);
                }
            }
            List<OrderTypeGroupBL> orderTypeGroupBLList = new List<OrderTypeGroupBL>();
            bool orderTypeGroupFound = false;
            if (orderTypeIdSet.SetEquals(new HashSet<int>() { -1 }))
            {
                orderTypeGroupFound = true;
            }
            else
            {
                if (orderTypeGroupBLList == null || orderTypeGroupBLList.Count == 0)
                {
                    OrderTypeGroupListBL orderTypeGroupListBL = new OrderTypeGroupListBL(executionContext);
                    List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>();
                    searchParam.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParam.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG, 1.ToString()));
                    List<OrderTypeGroupDTO> orderTypeGroupDTOList = orderTypeGroupListBL.GetOrderTypeGroupDTOList(searchParam, sqlTransaction);
                    orderTypeGroupBLList = new List<OrderTypeGroupBL>();

                    if (orderTypeGroupDTOList != null)
                    {
                        foreach (var orderTypeGroupDTO in orderTypeGroupDTOList)
                        {
                            orderTypeGroupBLList.Add(new OrderTypeGroupBL(executionContext, orderTypeGroupDTO));
                        }
                    }
                }
                foreach (var orderTypeGroupBL in orderTypeGroupBLList)
                {
                    if (orderTypeGroupBL.Match(orderTypeIdSet))
                    {
                        orderTypeGroupFound = true;
                        break;
                    }
                }
            }
            if (orderTypeGroupFound == false)
            {
                if (!string.IsNullOrEmpty(productType) && productType == "COMBO")
                {
                    validationErrorList.Add(new ValidationError("Combo Product", "COMBO", MessageContainerList.GetMessage(executionContext, 1404)));
                }
                else
                {
                    validationErrorList.Add(new ValidationError("Combo Product", "BOOKINGS", MessageContainerList.GetMessage(executionContext, 1405)));

                }
            }
            if (comboProductDTO.MaximumQuantity.HasValue && comboProductDTO.Quantity.HasValue && comboProductDTO.Quantity > 0)
            {
                if (comboProductDTO.MaximumQuantity < comboProductDTO.Quantity)
                {
                    ValidationError quantityValidationError = new ValidationError("Combo Product", "MaximumQuantity", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Maximum Quantity")));
                    validationErrorList.Add(quantityValidationError);
                }
                if (comboProductDTO.MaximumQuantity < 0)
                {
                    ValidationError quantityValidationError = new ValidationError("Combo Product", "MaximumQuantity", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Maximum Quantity")));
                    validationErrorList.Add(quantityValidationError);
                }

            }
            if (comboProductDTO.Quantity.HasValue && comboProductDTO.Quantity < 0)
            {
                ValidationError quantityValidationError = new ValidationError("Combo Product", "MinimumQuantity", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Minimum Quantity")));
                validationErrorList.Add(quantityValidationError);
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        //private void Validation
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ComboProductDTO ComboProductDTO
        {
            get
            {
                return comboProductDTO;
            }
        }

    }


    /// <summary>
    /// Manages the list of ComboProducts
    /// </summary>
    public class ComboProductList
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ComboProductDTO> comboProductDTOsList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ComboProductList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.comboProductDTOsList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="comboProductDTOList"></param>
        /// <param name="executionContext"></param>
        public ComboProductList(ExecutionContext executionContext, List<ComboProductDTO> comboProductDTOList)
        {
            log.LogMethodEntry(executionContext, comboProductDTOList);
            this.executionContext = executionContext;
            this.comboProductDTOsList = comboProductDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetComboProductDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<ComboProductDTO></returns>
        public List<ComboProductDTO> GetComboProductDTOList(List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ComboProductDataHandler comboProductDataHandler = new ComboProductDataHandler(sqlTransaction);
            List<ComboProductDTO> returnValue = comboProductDataHandler.GetComboProductDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// Save or update combo product details
        /// </summary>
        public List<ComboProductDTO> SaveUpdateComboProductList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ComboProductDTO> comboProductDTOLists = new List<ComboProductDTO>();
            if (comboProductDTOsList != null)
            {
                foreach (ComboProductDTO comboProductDTO in comboProductDTOsList)
                {
                    try
                    {
                        ComboProductBL comboProductBL = new ComboProductBL(executionContext, comboProductDTO);
                        comboProductBL.Save(sqlTransaction);
                        comboProductDTOLists.Add(comboProductBL.ComboProductDTO);

                    }
                    catch (ValidationException valEx)
                    {
                        log.Error(valEx);
                        log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit();
            }
            return comboProductDTOLists;
        }
        /// <summary>
        /// Hard Deletions for ComboProduct
        /// </summary>
        public void DeleteComboProductList()
        {
            log.LogMethodEntry();
            if (comboProductDTOsList != null && comboProductDTOsList.Count > 0)
            {
                foreach (ComboProductDTO comboProductDTO in comboProductDTOsList)
                {
                    if (comboProductDTO.IsChanged && comboProductDTO.IsActive == false)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                ComboProductBL comboProductBL = new ComboProductBL(executionContext);
                                comboProductBL.DeleteComboProduct(comboProductDTO.ComboProductId, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ComboProductDTO List for product Id List
        /// </summary>
        /// <param name="productIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ComboProductDTO> GetComboProductDTOListForProducts(List<int> productIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productIdList, activeRecords, sqlTransaction);
            ComboProductDataHandler comboProductDataHandler = new ComboProductDataHandler(sqlTransaction);
            List<ComboProductDTO> comboProductDTOList = comboProductDataHandler.GetComboProductDTOList(productIdList, activeRecords);
            log.LogMethodExit(comboProductDTOList);
            return comboProductDTOList;
        }
    }
}
