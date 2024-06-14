
/********************************************************************************************
 * Project Name - DisplayGroup
 * Description  - Bussiness logic of the Product DisplayGroupFormat class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        18-may-2016   Amaresh          Created 
 *2.3.0      25-Jun-2018    Guru S A         Modifications handle products exclusion at user role
 *2.60       5-Feb-2019     Nagesh Badiger   Class ProductDisplayGroupFormat and ProductDisplayGroupList ,GetAllProductDisplayGroup(),constructors and methods.
 **********************************************************************************************
 *2.60       18-Mar-2019    Akshay Gulaganji    Added defaultConstructor and constructor with executionContext and GetProductGroupInclusionList()
 *2.80       11-Mar-2020    Vikas Dwivedi    Modified as per the standards for Phase 1 changes.
 *2.140.0    06-Dec-2021    Fiona         modified : Issue fix in  ProductDisplayGroupList Build method.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Bussiness Logic Of Product DisplayGroupFormat 
    /// </summary>

    public class ProductDisplayGroupFormat
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductDisplayGroupFormatDTO productDisplayGroupFormat;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>      
        /// <param name="executionContext"></param>
        public ProductDisplayGroupFormat(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductDisplayGroupFormat id as the parameter
        /// Would fetch the ProductDisplayGroupFormat object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext is passsed as the parameter</param>
        /// <param name="productDisplayGroupFormatDTO">ProductDisplayGroupFormatDTO object is passed as the parameter</param>
        public ProductDisplayGroupFormat(ExecutionContext executionContext, ProductDisplayGroupFormatDTO productDisplayGroupFormatDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productDisplayGroupFormatDTO);
            this.productDisplayGroupFormat = productDisplayGroupFormatDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the displayGroup  as the parameter
        /// Would fetch the ProductDisplayGroupFormatDTO object from the database based on the displayGroup passed. 
        /// </summary>
        /// <param name="displayGroup">Display Group</param>
        public ProductDisplayGroupFormat(ExecutionContext executionContext, string displayGroup, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, displayGroup, sqlTransaction);
            ProductDisplayGroupFormatDataHandler productDisplayGroupFormatDataHandler = new ProductDisplayGroupFormatDataHandler(sqlTransaction);
            productDisplayGroupFormat = productDisplayGroupFormatDataHandler.GetProductDisplayGroupFormat(displayGroup);
            if (productDisplayGroupFormat == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductDisplayGroupFomrat", displayGroup);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (productDisplayGroupFormat != null && loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Build the child records for ProductDisplayGroupFormat object.
        /// </summary>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext);
            List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParameters = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID, productDisplayGroupFormat.DisplayGroup.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.IS_ACTIVE, "1"));
            }
            productDisplayGroupFormat.ProductDisplayGroupList = productsDisplayGroupList.GetAllProductsDisplayGroup(searchParameters, null);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the productDisplayGroupFormat based on the displayGroup
        /// </summary>
        /// <param name="displayGroup">ProductDisplayGroupFormatDTO object</param>
        public ProductDisplayGroupFormatDTO GetProductDisplayFormat(string displayGroup, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(displayGroup);
            ProductDisplayGroupFormatDataHandler productDisplayFormat = new ProductDisplayGroupFormatDataHandler(sqlTransaction);
            productDisplayGroupFormat = productDisplayFormat.GetProductDisplayGroupFormat(displayGroup);
            log.LogMethodExit(productDisplayGroupFormat);
            return productDisplayGroupFormat;
        }

        /// <summary>
        /// Saves the productDisplayFormat 
        /// Checks if the id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (productDisplayGroupFormat.IsChangedRecursive == false
                && productDisplayGroupFormat.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ProductDisplayGroupFormatDataHandler productDisplayFormatDataHandler = new ProductDisplayGroupFormatDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, 14773);
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (productDisplayGroupFormat.Id < 0)
            {
                log.LogVariableState("ProductDisplayGroupFormatDTO", productDisplayGroupFormat);
                int id = productDisplayFormatDataHandler.InsertProductDisplayGroupFormat(productDisplayGroupFormat, executionContext.GetUserId(), executionContext.GetSiteId());
                productDisplayGroupFormat.Id = id;
            }
            else if (productDisplayGroupFormat.IsChanged)
            {
                log.LogVariableState("ProductDisplayGroupFormatDTO", productDisplayGroupFormat);
                productDisplayFormatDataHandler.UpdateProductDisplayFormat(productDisplayGroupFormat, executionContext.GetUserId(), executionContext.GetSiteId());
                productDisplayGroupFormat.AcceptChanges();
            }
            SaveProductDisplayGroup(sqlTransaction);
            
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : ProductsDisplayGroupDTOList 
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        private void SaveProductDisplayGroup(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productDisplayGroupFormat.ProductDisplayGroupList != null &&
                productDisplayGroupFormat.ProductDisplayGroupList.Any())
            {
                List<ProductsDisplayGroupDTO> updatedProductsDisplayGroupList = new List<ProductsDisplayGroupDTO>();
                foreach (var productDisplayGroupDTO in productDisplayGroupFormat.ProductDisplayGroupList)
                {
                    if (productDisplayGroupDTO.Id != productDisplayGroupFormat.Id)
                    {
                        productDisplayGroupDTO.Id = productDisplayGroupFormat.Id;
                    }
                    if (productDisplayGroupDTO.IsChanged)
                    {
                        updatedProductsDisplayGroupList.Add(productDisplayGroupDTO);
                    }
                }
                if (updatedProductsDisplayGroupList.Any())
                {
                    ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext, updatedProductsDisplayGroupList);
                    productsDisplayGroupList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validates the POSProductExclusionsDTO, ProductDisplayGroupFormatDTO - child only if saving is needed. 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(productDisplayGroupFormat.DisplayGroup))
            {
                validationErrorList.Add(new ValidationError("ProductDisplayGroupFormat", "DisplayGroup", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "DisplayGroup"))));
            }

            if (!string.IsNullOrWhiteSpace(productDisplayGroupFormat.DisplayGroup) && productDisplayGroupFormat.DisplayGroup.Length > 50)
            {
                validationErrorList.Add(new ValidationError("ProductDisplayGroupFormat", "DisplayGroup", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Display Group"), 50)));
            }
            //Use Only if validation before saving of child record  is needed.
            if (productDisplayGroupFormat.ProductDisplayGroupList != null)
            {
                foreach (var productsDisplayGroupDTO in productDisplayGroupFormat.ProductDisplayGroupList)
                {
                    if (productsDisplayGroupDTO.IsChanged)
                    {
                        ProductsDisplayGroup productsDisplayGroup = new ProductsDisplayGroup(executionContext, productsDisplayGroupDTO);
                        validationErrorList.AddRange(productsDisplayGroup.Validate());
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Delete the ProductDisplayGroupFormatDTO based on Id
        /// </summary>
        /// <param name="displayGroupId">displayGroupId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public int Delete(int displayGroupId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(displayGroupId, sqlTransaction);
            try
            {
                ProductDisplayGroupFormatDataHandler productDisplayGroupFormatDataHandler = new ProductDisplayGroupFormatDataHandler(sqlTransaction);
                int deleteStatus = productDisplayGroupFormatDataHandler.Delete(displayGroupId);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        //Added to Validate duplicate display group name
        public bool IsExistDisplayGroupFormat(string displayGroup)
        {
            log.LogMethodEntry(displayGroup);
            ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);

            List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParam = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
            searchParam.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParam.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP, displayGroup.Trim().ToString()));
            List<ProductDisplayGroupFormatDTO> productDisplayGroupListOnDisplay = productDisplayGroupList.GetAllProductDisplayGroup(searchParam);

            if (productDisplayGroupListOnDisplay != null && productDisplayGroupListOnDisplay.Any())
            {
                log.LogMethodExit();
                return true;
            }
            log.LogMethodExit();
            return false;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductDisplayGroupFormatDTO GetProductDisplayFormatDetails
        {
            get { return productDisplayGroupFormat; }
        }
    }

    /// <summary>
    /// Manages the list of Product Display Group
    /// </summary>
    public class ProductDisplayGroupList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList = new List<ProductDisplayGroupFormatDTO>();
        /// <summary>
        /// default Constructor
        /// </summary>      
        /// <param name="executionContext"></param>
        public ProductDisplayGroupList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>      
        /// <param name="executionContext"></param>
        public ProductDisplayGroupList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="productDisplayGroupFormatDTO"></param>
        /// <param name="executionContext"></param>
        public ProductDisplayGroupList(ExecutionContext executionContext, List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(productDisplayGroupFormatDTO, executionContext);
            this.productDisplayGroupFormatDTOList = productDisplayGroupFormatDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Product Display GroupList
        /// </summary>
        public List<ProductDisplayGroupFormatDTO> GetAllProductDisplayGroup(List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ProductDisplayGroupFormatDataHandler productDisplayGroupFormatDataHandler = new ProductDisplayGroupFormatDataHandler(sqlTransaction);
            productDisplayGroupFormatDTOList = productDisplayGroupFormatDataHandler.GetProductDisplayGroupFormatList(searchParameters);
            if (productDisplayGroupFormatDTOList != null && productDisplayGroupFormatDTOList.Any() && loadChildRecords)
            {
                Build(productDisplayGroupFormatDTOList, loadActiveChildRecords, sqlTransaction);
            }
            log.LogMethodExit(productDisplayGroupFormatDTOList);
            return productDisplayGroupFormatDTOList;
        }

        /// <summary>
        /// Builds the List of ProductDislplayGroupFormat object based on the list of DisplayGroup id
        /// </summary>
        /// <param name="pOSProductDisplayGroupFormatDTOList">pOSProductDisplayGroupFormatDTOList</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(List<ProductDisplayGroupFormatDTO> pOSProductDisplayGroupFormatDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pOSProductDisplayGroupFormatDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ProductDisplayGroupFormatDTO> posProductDisplayGroupFormatIdPOSProductDisplayGroupFormatDictionary = new Dictionary<int, ProductDisplayGroupFormatDTO>();
            string pOSProductDisplayGroupFormatIdSet = string.Empty;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < pOSProductDisplayGroupFormatDTOList.Count; i++)
            {
                if (pOSProductDisplayGroupFormatDTOList[i].Id == -1 ||
                    posProductDisplayGroupFormatIdPOSProductDisplayGroupFormatDictionary.ContainsKey(pOSProductDisplayGroupFormatDTOList[i].Id))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(pOSProductDisplayGroupFormatDTOList[i].Id);
                posProductDisplayGroupFormatIdPOSProductDisplayGroupFormatDictionary.Add(pOSProductDisplayGroupFormatDTOList[i].Id, pOSProductDisplayGroupFormatDTOList[i]);
            }
            pOSProductDisplayGroupFormatIdSet = sb.ToString();

            // loads child records - ProductsDisplayGroups
            ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList();
            List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchByProductDisplayGroupParams = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
            searchByProductDisplayGroupParams.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID_LIST, pOSProductDisplayGroupFormatIdSet.ToString()));
            if (activeChildRecords)
            {
                searchByProductDisplayGroupParams.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.IS_ACTIVE, "1"));
            }
            List<ProductsDisplayGroupDTO> productDisplayGroupDTOList = productsDisplayGroupList.GetAllProductsDisplayGroup(searchByProductDisplayGroupParams, sqlTransaction);
            if (productDisplayGroupDTOList != null && productDisplayGroupDTOList.Any())
            {
                log.LogVariableState("productDisplayGroupDTOList", productDisplayGroupDTOList);
                foreach (var productDisplayGroupDTO in productDisplayGroupDTOList)
                {
                    if (posProductDisplayGroupFormatIdPOSProductDisplayGroupFormatDictionary.ContainsKey(productDisplayGroupDTO.DisplayGroupId))
                    {
                        if (posProductDisplayGroupFormatIdPOSProductDisplayGroupFormatDictionary[productDisplayGroupDTO.DisplayGroupId].ProductDisplayGroupList == null)
                        {
                            posProductDisplayGroupFormatIdPOSProductDisplayGroupFormatDictionary[productDisplayGroupDTO.DisplayGroupId].ProductDisplayGroupList = new List<ProductsDisplayGroupDTO>();
                        }
                        posProductDisplayGroupFormatIdPOSProductDisplayGroupFormatDictionary[productDisplayGroupDTO.DisplayGroupId].ProductDisplayGroupList.Add(productDisplayGroupDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        public List<ProductDisplayGroupFormatDTO> GetOnlyUsedProductDisplayGroup(List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ProductDisplayGroupFormatDataHandler productDisplayGroupFormatDataHandler = new ProductDisplayGroupFormatDataHandler(sqlTransaction);
            productDisplayGroupFormatDTOList = productDisplayGroupFormatDataHandler.GetOnlyUsedProductDisplayGroupFormatList(searchParameters);
            log.LogMethodExit(productDisplayGroupFormatDTOList);
            return productDisplayGroupFormatDTOList;
        }
        /// <summary>
        /// Returns the Product Display GroupList configured to poss
        /// </summary>
        public List<ProductDisplayGroupFormatDTO> GetConfiguredDisplayGroupList(string macAddress, string loginId, SqlTransaction sqlTransaction = null,
            bool loadChildRecords = false, bool loadActiveChildRecords = false)
        {
            log.LogMethodEntry(macAddress, loginId);
            ProductDisplayGroupFormatDataHandler productDisplayGroupFormatDataHandler = new ProductDisplayGroupFormatDataHandler(sqlTransaction);
            productDisplayGroupFormatDTOList = productDisplayGroupFormatDataHandler.GetConfiguredDisplayGroupList(macAddress, loginId);
            if (productDisplayGroupFormatDTOList != null && productDisplayGroupFormatDTOList.Any() && loadChildRecords)
            {
                Build(productDisplayGroupFormatDTOList, loadActiveChildRecords, sqlTransaction);
            }
            log.LogMethodExit(productDisplayGroupFormatDTOList);
            return productDisplayGroupFormatDTOList;
        }

        /// <summary>
        /// Returns the Product Display GroupList configured to poss
        /// </summary>
        public List<ProductDisplayGroupFormatDTO> GetConfiguredDisplayGroupListForLogin(string loginId, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loginId);
            ProductDisplayGroupFormatDataHandler productDisplayGroupFormatDataHandler = new ProductDisplayGroupFormatDataHandler(sqlTransaction);
            List<ProductDisplayGroupFormatDTO> productDisplayGroups = productDisplayGroupFormatDataHandler.GetConfiguredDisplayGroupListForLogin(loginId);
            if (productDisplayGroups != null && productDisplayGroups.Any() && loadChildRecords)
            {
                Build(productDisplayGroups, loadActiveChildRecords, sqlTransaction);
            }
            log.LogMethodExit(productDisplayGroups);
            return productDisplayGroups;
        }
        /// <summary>
        /// Gets the ProductGroupInclusion List
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductDisplayGroupFormatDTO> GetProductGroupInclusionList(List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ProductDisplayGroupFormatDataHandler productDisplayGroupFormatDataHandler = new ProductDisplayGroupFormatDataHandler(sqlTransaction);
            List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList = productDisplayGroupFormatDataHandler.GetProductGroupInclusionList(searchParameters);
            log.LogMethodExit(productDisplayGroupFormatDTOList);
            return productDisplayGroupFormatDTOList;
        }

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the ProductDisplayGroupFormat List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productDisplayGroupFormatDTOList == null ||
                productDisplayGroupFormatDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < productDisplayGroupFormatDTOList.Count; i++)
            {
                var productDisplayGroupFormatDTO = productDisplayGroupFormatDTOList[i];
                if (productDisplayGroupFormatDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ProductDisplayGroupFormat productDisplayGroupFormat = new ProductDisplayGroupFormat(executionContext, productDisplayGroupFormatDTO);
                    //Check inserting Duplicate display group format
                    bool isExistDisplayGroupFormate = productDisplayGroupFormat.IsExistDisplayGroupFormat(productDisplayGroupFormatDTO.DisplayGroup);
                    if (!(productDisplayGroupFormatDTO.Id == -1 && isExistDisplayGroupFormate))
                    {
                        productDisplayGroupFormat.Save(sqlTransaction);
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving ProductDisplayGroupFormatDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("ProductDisplayGroupFormatDTO", productDisplayGroupFormatDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

       
        /// <summary>
        /// Hard Deletions for Attraction Plays 
        /// </summary>
        public void DeleteProductDisplayGroupFormatList()
        {
            log.LogMethodEntry();
            if (productDisplayGroupFormatDTOList != null && productDisplayGroupFormatDTOList.Any())
            {
                foreach (ProductDisplayGroupFormatDTO productDisplayGroupFormatDTO in productDisplayGroupFormatDTOList)
                {
                    if (productDisplayGroupFormatDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                ProductDisplayGroupFormat productDisplayGroupFormat = new ProductDisplayGroupFormat(executionContext);
                                productDisplayGroupFormat.Delete(productDisplayGroupFormatDTO.Id, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (SqlException sqlEx)
                            {
                                log.Error(sqlEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing SQL Exception : " + sqlEx.Message);
                                if (sqlEx.Number == 547)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869)); //Unable to delete this record.Please check the reference record first.
                                }
                                else
                                {
                                    throw;
                                }
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

        public DateTime? GetProductDisplayGroupFormatModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductDisplayGroupFormatDataHandler productDisplayGroupFormatDataHandler = new ProductDisplayGroupFormatDataHandler(null);
            DateTime? result = productDisplayGroupFormatDataHandler.GetProductDisplayGroupFormatModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
