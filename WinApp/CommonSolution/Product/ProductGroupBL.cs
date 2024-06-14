/********************************************************************************************
 * Project Name - Product
 * Description  - Product Group Business object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 ********************************************************************************************* 
 *2.170.0     05-Jul-2023      Lakshminarayana     Created
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductGroupBL
    {
        #region Fields
        private ProductGroupDTO productGroupDTO;
        private readonly Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor of ProductGroupBL class
        /// </summary>
        private ProductGroupBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductGroupId parameter
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">To load the child DTO Records</param>
        public ProductGroupBL(ExecutionContext executionContext, UnitOfWork unitOfWork, 
                              int id, bool loadChildRecords = false, 
                              bool activeChildRecords = false)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, unitOfWork, id, loadChildRecords, activeChildRecords);
            LoadProductGroup(id, loadChildRecords, activeChildRecords);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductGroup id as the parameter
        /// Would fetch the ProductGroup object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        private void LoadProductGroup(int id, bool loadChildRecords = false, bool activeChildRecords = false)
        {
            log.LogMethodEntry(id, loadChildRecords, activeChildRecords);
            ProductGroupDataHandler productGroupDataHandler = new ProductGroupDataHandler(executionContext, unitOfWork);
            productGroupDTO = productGroupDataHandler.GetProductGroup(id);
            ThrowIfProductGroupIsNull(id);
            if (loadChildRecords)
            {
                ProductGroupBuilderBL productGroupBuilderBL = new ProductGroupBuilderBL(executionContext, unitOfWork);
                productGroupBuilderBL.Build(productGroupDTO, activeChildRecords);
            }
            log.LogMethodExit(productGroupDTO);
        }

        private void ThrowIfProductGroupIsNull(int menuId)
        {
            log.LogMethodEntry();
            if (productGroupDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductGroup", menuId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterProductGroupDTO">parameterProductGroupDTO</param>
        /// <param name="unitOfWork">unitOfWork</param>
        public ProductGroupBL(ExecutionContext executionContext, ProductGroupDTO parameterProductGroupDTO, UnitOfWork unitOfWork)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, parameterProductGroupDTO, unitOfWork);

            if (parameterProductGroupDTO.Id > -1)
            {
                LoadProductGroup(parameterProductGroupDTO.Id, true, true);//added sql
                ThrowIfProductGroupIsNull(parameterProductGroupDTO.Id);
                Update(parameterProductGroupDTO);
            }
            else
            {
                ValidateName(parameterProductGroupDTO.Name);
                ValidateIsActive(parameterProductGroupDTO.IsActive);
                productGroupDTO = new ProductGroupDTO(-1, parameterProductGroupDTO.Name, parameterProductGroupDTO.IsActive);
                if (parameterProductGroupDTO.ProductGroupMapDTOList != null && parameterProductGroupDTO.ProductGroupMapDTOList.Any())
                {
                    productGroupDTO.ProductGroupMapDTOList = new List<ProductGroupMapDTO>();
                    foreach (ProductGroupMapDTO parameterProductGroupMapDTO in parameterProductGroupDTO.ProductGroupMapDTOList)
                    {
                        if (parameterProductGroupMapDTO.Id > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductGroupMap", parameterProductGroupMapDTO.Id);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var productGroupMapDTO = new ProductGroupMapDTO(-1, -1, 
                                                                        parameterProductGroupMapDTO.ProductId, 
                                                                        parameterProductGroupMapDTO.SortOrder,
                                                                        parameterProductGroupMapDTO.IsActive);
                        ProductGroupMapBL productGroupMapBL = new ProductGroupMapBL(executionContext, productGroupMapDTO, unitOfWork);
                        productGroupDTO.ProductGroupMapDTOList.Add(productGroupMapBL.ProductGroupMapDTO);
                    }
                }
                ValidateProductGroupMapConstaints();
            }
            log.LogMethodExit();
        }
        #endregion

        #region Update Methods
        public void Update(ProductGroupDTO parameterProductGroupDTO)
        {
            log.LogMethodEntry(parameterProductGroupDTO);
            ChangeName(parameterProductGroupDTO.Name);
            ChangeIsActive(parameterProductGroupDTO.IsActive);
            Dictionary<int, ProductGroupMapDTO> productGroupMapDTODictionary = new Dictionary<int, ProductGroupMapDTO>();
            if (productGroupDTO.ProductGroupMapDTOList != null &&
                productGroupDTO.ProductGroupMapDTOList.Any())
            {
                foreach (var productGroupMapDTO in productGroupDTO.ProductGroupMapDTOList)
                {
                    productGroupMapDTODictionary.Add(productGroupMapDTO.Id, productGroupMapDTO);
                }
            }
            if (parameterProductGroupDTO.ProductGroupMapDTOList != null &&
                parameterProductGroupDTO.ProductGroupMapDTOList.Any())
            {
                foreach (var parameterProductGroupMapDTO in parameterProductGroupDTO.ProductGroupMapDTOList)
                {
                    if (productGroupMapDTODictionary.ContainsKey(parameterProductGroupMapDTO.Id))
                    {
                        ProductGroupMapBL productGroupMap = new ProductGroupMapBL(executionContext, productGroupMapDTODictionary[parameterProductGroupMapDTO.Id], unitOfWork);
                        productGroupMap.Update(parameterProductGroupMapDTO);
                    }
                    else if (parameterProductGroupMapDTO.Id > -1)
                    {
                        ProductGroupMapBL productGroupMap = new ProductGroupMapBL(executionContext, parameterProductGroupMapDTO.Id, unitOfWork);
                        productGroupDTO.ProductGroupMapDTOList.Add(productGroupMap.ProductGroupMapDTO);
                        productGroupMap.Update(parameterProductGroupMapDTO);
                    }
                    else
                    {
                        var productGroupMapDTO = new ProductGroupMapDTO(-1, -1,
                                                                        parameterProductGroupMapDTO.ProductId,
                                                                        parameterProductGroupMapDTO.SortOrder,
                                                                        parameterProductGroupMapDTO.IsActive);
                        ProductGroupMapBL productGroupMapBL = new ProductGroupMapBL(executionContext, productGroupMapDTO, unitOfWork);
                        productGroupDTO.ProductGroupMapDTOList.Add(productGroupMapBL.ProductGroupMapDTO);
                    }
                }
            }
            ValidateProductGroupMapConstaints();
            log.LogMethodExit();
        }

        private void ChangeName(string name)
        {
            log.LogMethodEntry(name);
            if (productGroupDTO.Name == name)
            {
                log.LogMethodExit(null, "No changes to ProductGroup name");
                return;
            }
            ValidateName(name);
            productGroupDTO.Name = name;
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (productGroupDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to ProductGroup isActive");
                return;
            }
            ValidateIsActive(isActive);
            productGroupDTO.IsActive = isActive;
            log.LogMethodExit();
        }
        #endregion

        #region Validation Methods
        private void ValidateProductGroupMapConstaints()
        {
            log.LogMethodEntry();
            if(productGroupDTO.IsActive == false || productGroupDTO.ProductGroupMapDTOList.Any() == false)
            {
                log.LogMethodExit("inactive group or empty child list");
                return;
            }
            var duplicateGroup = productGroupDTO.ProductGroupMapDTOList.Where(x => x.IsActive).GroupBy(x => x.ProductId).Where(x => x.Count() > 1);
            if(duplicateGroup.Any())
            {
                var duplicateProductGroupMapDTO = duplicateGroup.First().FirstOrDefault();
                Products product = new Products(duplicateProductGroupMapDTO.ProductId);
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4991, product.GetProductsDTO.ProductName, productGroupDTO.Name);
                throw new ValidationException("Duplicate product group map records.", "ProductGroup", "ProductId", errorMessage);
            }
            
            log.LogMethodExit();
        }

        private void ValidateIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if ((productGroupDTO != null && productGroupDTO.Id > -1) && isActive == false)
            {
                ProductGroupDataHandler productGroupDataHandler = new ProductGroupDataHandler(executionContext, unitOfWork);
                bool isRecordReferenced = productGroupDataHandler.IsProductGroupReferenced(productGroupDTO.Id);
                if (isRecordReferenced)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1869);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Unable to delete this record. Please check the reference record first.", "ProductGroup", "IsActive", errorMessage);
                }
            }
            log.LogMethodExit();
        }

        private void ValidateName(string name)
        {
            log.LogMethodEntry(name);
            if (string.IsNullOrWhiteSpace(name))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Name"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name is empty.", "ProductGroup", "Name", errorMessage);
            }
            if (name.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Name"), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name greater than 100 characters.", "ProductGroup", "Name", errorMessage);
            }
            log.LogMethodExit();
        }
        #endregion

        #region Save Method
        
        /// <summary>
        /// Saves theProductGroup
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save()
        {
            log.LogMethodEntry();
            ProductGroupDataHandler productGroupDataHandler = new ProductGroupDataHandler(executionContext, unitOfWork);
            if (productGroupDTO.IsChanged)
            {
                log.LogVariableState("productGroupDTO", productGroupDTO);
                productGroupDTO = SiteContainerList.ToSiteDateTime(executionContext, productGroupDTO);
                productGroupDTO = productGroupDataHandler.Save(productGroupDTO);
                productGroupDTO = SiteContainerList.FromSiteDateTime(executionContext, productGroupDTO);
                productGroupDTO.AcceptChanges();
            }
            // Will Save the Child ProductGroupDTO
            log.LogVariableState("productGroupDTO.ProductGroupMapDTOList Value :", productGroupDTO.ProductGroupMapDTOList);
            if (productGroupDTO.ProductGroupMapDTOList != null && productGroupDTO.ProductGroupMapDTOList.Any())
            {
                List<ProductGroupMapDTO> updatedProductGroupMapDTOList = new List<ProductGroupMapDTO>();
                foreach (ProductGroupMapDTO productGroupMapDTO in productGroupDTO.ProductGroupMapDTOList)
                {
                    if (productGroupMapDTO.ProductGroupId != productGroupDTO.Id)
                    {
                        productGroupMapDTO.ProductGroupId = productGroupDTO.Id;
                    }
                    if (productGroupMapDTO.IsChanged)
                    {
                        updatedProductGroupMapDTOList.Add(productGroupMapDTO);
                    }
                }
                log.LogVariableState("updatedProductGroupDTOList :", updatedProductGroupMapDTOList);
                ProductGroupMapListBL productGroupMapListBL = new ProductGroupMapListBL(executionContext, unitOfWork);
                productGroupMapListBL.Save(updatedProductGroupMapDTOList);
            }
            log.LogMethodExit();
        }
        #endregion 

        #region Properties

        /// <summary>
        /// Get method of the ProductUserGroupsId field
        /// </summary>
        public int Id { get { return productGroupDTO.Id; }  }

        /// <summary>
        /// Get method of the name field
        /// </summary>
        public string Name { get { return productGroupDTO.Name; } }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductGroupDTO ProductGroupDTO
        {
            get
            {
                ProductGroupDTO result = new ProductGroupDTO(productGroupDTO);
                return result;
            }
        }
        #endregion
    }

    /// <summary>
    /// Manages the list of ProductGroup
    /// </summary>
    public class ProductGroupListBL
    {
        private readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductGroupListBL()
        {
            log.LogMethodEntry();
            unitOfWork = new UnitOfWork();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of ProductGroupListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ProductGroupListBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method should be used to Save and Update the ProductGroup.
        /// </summary>
        public List<ProductGroupDTO> Save(List<ProductGroupDTO> productGroupDTOList)
        {
            log.LogMethodEntry();
            List<ProductGroupDTO> savedProductGroupDTOList = new List<ProductGroupDTO>();
            if (productGroupDTOList == null || productGroupDTOList.Any() == false)
            {
                log.LogMethodExit(savedProductGroupDTOList);
                return savedProductGroupDTOList;
            }
            foreach (ProductGroupDTO productGroupDTO in productGroupDTOList)
            {
                ProductGroupBL productGroupBL = new ProductGroupBL(executionContext, productGroupDTO, unitOfWork);
                productGroupBL.Save();
                savedProductGroupDTOList.Add(productGroupBL.ProductGroupDTO);
            }
            log.LogMethodExit(savedProductGroupDTOList);
            return savedProductGroupDTOList;
        }

        /// <summary>
        /// Returns the productGroup DTO list count matching the search criteria
        /// </summary>
        public int GetProductGroupDTOListCount(SearchParameterList<ProductGroupDTO.SearchByParameters> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            ProductGroupDataHandler productGroupDataHandler = new ProductGroupDataHandler(executionContext, unitOfWork);
            int result = productGroupDataHandler.GetProductGroupDTOListCount(searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the ProductGroup list
        /// </summary>
        public List<ProductGroupDTO> GetProductGroupDTOList(SearchParameterList<ProductGroupDTO.SearchByParameters> searchParameters, 
                                                            bool loadChildRecords = false, 
                                                            bool loadActiveChildRecords = false,
                                                            int pageNumber = 0,
                                                            int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            ProductGroupDataHandler productGroupDataHandler = new ProductGroupDataHandler(executionContext, unitOfWork);
            List<ProductGroupDTO> productGroupDTOsList = productGroupDataHandler.GetProductGroupDTOList(searchParameters, pageNumber, pageSize);
            if (productGroupDTOsList != null && productGroupDTOsList.Any() && loadChildRecords)
            {
                ProductGroupBuilderBL productGroupBuilderBL = new ProductGroupBuilderBL(executionContext, unitOfWork);
                productGroupBuilderBL.Build(productGroupDTOsList, loadActiveChildRecords);
            }
            log.LogMethodExit(productGroupDTOsList);
            return productGroupDTOsList;
        }

        /// <summary>
        /// Returns the ProductGroup list
        /// </summary>
        public List<ProductGroupDTO> GetProductGroupDTOList(List<string> discountGuidList,
                                                      bool loadChildRecords = true,
                                                      bool loadActiveChildRecords = true)
        {
            log.LogMethodEntry(discountGuidList, loadChildRecords, loadActiveChildRecords);
            ProductGroupDataHandler productgroupDataHandler = new ProductGroupDataHandler(executionContext, unitOfWork);
            List<ProductGroupDTO> productgroupDTOsList = productgroupDataHandler.GetProductGroupDTOList(discountGuidList);
            if (productgroupDTOsList != null && productgroupDTOsList.Any() && loadChildRecords)
            {
                ProductGroupBuilderBL productgroupBuilderBL = new ProductGroupBuilderBL(executionContext, unitOfWork);
                productgroupBuilderBL.Build(productgroupDTOsList, loadActiveChildRecords);
            }
            log.LogMethodExit(productgroupDTOsList);
            return productgroupDTOsList;
        }

        /// <summary>
        /// Returns the ProductGroup module last update time list
        /// </summary>
        public DateTime? GetProductGroupModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductGroupDataHandler productGroupDataHandler = new ProductGroupDataHandler(executionContext, unitOfWork);
            DateTime? result = productGroupDataHandler.GetProductGroupModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

    }

    internal class ProductGroupBuilderBL
    {
        private readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;

        internal ProductGroupBuilderBL(ExecutionContext executionContext, 
                                       UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        internal void Build(ProductGroupDTO productGroupDTO, bool activeChildRecords = true)
        {
            log.LogMethodEntry(productGroupDTO, activeChildRecords);
            Build(new List<ProductGroupDTO>(){ productGroupDTO }, activeChildRecords);
            log.LogMethodExit();
        }

        internal void Build(List<ProductGroupDTO> productGroupDTOList, bool activeChildRecords = true)
        {
            log.LogMethodEntry(productGroupDTOList, activeChildRecords);
            Dictionary<int, ProductGroupDTO> productGroupDTOIdMap = new Dictionary<int, ProductGroupDTO>();
            for (int i = 0; i < productGroupDTOList.Count; i++)
            {
                if (productGroupDTOIdMap.ContainsKey(productGroupDTOList[i].Id))
                {
                    continue;
                }
                productGroupDTOIdMap.Add(productGroupDTOList[i].Id, productGroupDTOList[i]);
            }

            ProductGroupMapListBL productGroupMapListBL = new ProductGroupMapListBL(executionContext, unitOfWork);
            List<ProductGroupMapDTO> productGroupMapDTOList = productGroupMapListBL.GetProductGroupMapDTOList(productGroupDTOIdMap.Keys.ToList(), activeChildRecords);
            for (int i = 0; i < productGroupMapDTOList.Count; i++)
            {
                ProductGroupDTO productGroupDTO = productGroupDTOIdMap[productGroupMapDTOList[i].ProductGroupId];
                productGroupDTO.ProductGroupMapDTOList.Add(productGroupMapDTOList[i]);
            }
            log.LogMethodExit();
        }
    }
}
