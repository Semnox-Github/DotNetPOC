/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu Business object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.130.0        27-May-2021      Prajwal S       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductMenuBL
    {
        private ProductMenuDTO productMenuDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private ProductMenuBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the ProductMenuId parameter
        /// </summary>
        /// <param name="productMenuId">ProductMenuId</param>
        /// <param name="loadChildRecords">To load the child DTO Records</param>
        public ProductMenuBL(ExecutionContext executionContext, int productMenuId, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(productMenuId, loadChildRecords, activeChildRecords);
            LoadProductMenu(productMenuId, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductMenu id as the parameter
        /// Would fetch the ProductMenu object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        private void LoadProductMenu(int id, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, executionContext, sqlTransaction);
            ProductMenuDataHandler productMenuDataHandler = new ProductMenuDataHandler(sqlTransaction);
            productMenuDTO = productMenuDataHandler.GetProductMenu(id);
            ThrowIfProductMenuIsNull(id);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            SetFromSiteTimeOffset();
            log.LogMethodExit(productMenuDTO);
        }

        private void ThrowIfProductMenuIsNull(int menuId)
        {
            log.LogMethodEntry();
            if (productMenuDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductMenu", menuId);
                log.LogMethodExit(null, "Throwing Exception - "); //+ //message);
                throw new EntityNotFoundException("invalid Id");//message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parametersproductMenuDTO">sproductMenuDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProductMenuBL(ExecutionContext executionContext, ProductMenuDTO parametersproductMenuDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parametersproductMenuDTO, sqlTransaction);

            if (parametersproductMenuDTO.MenuId > -1)
            {
                LoadProductMenu(parametersproductMenuDTO.MenuId, true, false, sqlTransaction);
                ThrowIfProductMenuIsNull(parametersproductMenuDTO.MenuId);
                Update(parametersproductMenuDTO, sqlTransaction);
            }
            else
            {
                ValidateName(parametersproductMenuDTO.Name);
                ValidateDescription(parametersproductMenuDTO.Description);
                ValidateStartDateAndEndDate(parametersproductMenuDTO.StartDate, parametersproductMenuDTO.EndDate);
                ValidateType(parametersproductMenuDTO.Type);
                ValidateIsActive(parametersproductMenuDTO.IsActive, sqlTransaction);
                productMenuDTO = new ProductMenuDTO(-1, parametersproductMenuDTO.Name, parametersproductMenuDTO.Description,parametersproductMenuDTO.Type, parametersproductMenuDTO.StartDate, parametersproductMenuDTO.EndDate, parametersproductMenuDTO.IsActive);
                if (parametersproductMenuDTO.ProductMenuPanelMappingDTOList != null && parametersproductMenuDTO.ProductMenuPanelMappingDTOList.Any())
                {
                    productMenuDTO.ProductMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
                    foreach (ProductMenuPanelMappingDTO parameterProductMenuPanelMappingDTO in parametersproductMenuDTO.ProductMenuPanelMappingDTOList)
                    {
                        if (parameterProductMenuPanelMappingDTO.Id > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductMenuPanelMapping", parameterProductMenuPanelMappingDTO.Id);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var productMenuPanelMappingDTO = new ProductMenuPanelMappingDTO(-1, -1, parameterProductMenuPanelMappingDTO.PanelId, parameterProductMenuPanelMappingDTO.IsActive);
                        ProductMenuPanelMappingBL productMenuPanelMappingBL = new ProductMenuPanelMappingBL(executionContext, productMenuPanelMappingDTO);
                        productMenuDTO.ProductMenuPanelMappingDTOList.Add(productMenuPanelMappingBL.ProductMenuPanelMappingDTO);
                    }
                }
            }
            log.LogMethodExit();
        }


        private void Update(ProductMenuDTO parameterproductMenuDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterproductMenuDTO, sqlTransaction);
            ChangeName(parameterproductMenuDTO.Name);
            ChangeDescription(parameterproductMenuDTO.Description);
            ChangeStartDateAndEndDate(parameterproductMenuDTO.StartDate, parameterproductMenuDTO.EndDate);
            ChangeIsActive(parameterproductMenuDTO.IsActive, sqlTransaction);
            ChangeType(parameterproductMenuDTO.Type);
            Dictionary<int, ProductMenuPanelMappingDTO> productMenuPanelMappingDTODictionary = new Dictionary<int, ProductMenuPanelMappingDTO>();
            if (productMenuDTO.ProductMenuPanelMappingDTOList != null &&
                productMenuDTO.ProductMenuPanelMappingDTOList.Any())
            {
                foreach (var productMenuPanelMappingDTO in productMenuDTO.ProductMenuPanelMappingDTOList)
                {
                    productMenuPanelMappingDTODictionary.Add(productMenuPanelMappingDTO.Id, productMenuPanelMappingDTO);
                }
            }
            if (parameterproductMenuDTO.ProductMenuPanelMappingDTOList != null &&
                parameterproductMenuDTO.ProductMenuPanelMappingDTOList.Any())
            {
                foreach (var parameterProductMenuPanelMappingDTO in parameterproductMenuDTO.ProductMenuPanelMappingDTOList)
                {
                    if (productMenuPanelMappingDTODictionary.ContainsKey(parameterProductMenuPanelMappingDTO.Id))
                    {
                        ProductMenuPanelMappingBL productMenuPanelMapping = new ProductMenuPanelMappingBL(executionContext, productMenuPanelMappingDTODictionary[parameterProductMenuPanelMappingDTO.Id]);
                        productMenuPanelMapping.Update(parameterProductMenuPanelMappingDTO, sqlTransaction);
                    }
                    else if (parameterProductMenuPanelMappingDTO.Id > -1)
                    {
                        ProductMenuPanelMappingBL productMenuPanelMapping = new ProductMenuPanelMappingBL(executionContext, parameterProductMenuPanelMappingDTO.Id, sqlTransaction);
                        if (productMenuDTO.ProductMenuPanelMappingDTOList == null)
                        {
                            productMenuDTO.ProductMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
                        }
                        productMenuDTO.ProductMenuPanelMappingDTOList.Add(productMenuPanelMapping.ProductMenuPanelMappingDTO);
                        productMenuPanelMapping.Update(parameterProductMenuPanelMappingDTO, sqlTransaction);
                    }
                    else
                    {
                        ProductMenuPanelMappingBL productMenuPanelMappingBL = new ProductMenuPanelMappingBL(executionContext, parameterProductMenuPanelMappingDTO);
                        if (productMenuDTO.ProductMenuPanelMappingDTOList == null)
                        {
                            productMenuDTO.ProductMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
                        }
                        productMenuDTO.ProductMenuPanelMappingDTOList.Add(productMenuPanelMappingBL.ProductMenuPanelMappingDTO);
                    }
                }
            }
        }

        /// <summary>
        /// Change isActive
        /// </summary>
        /// <param name="isActive"></param>
        public void ChangeIsActive(bool isActive, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(isActive);
            if (productMenuDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to productMenu IsActive");
                return;
            }
            ValidateIsActive(isActive, sqlTransaction);
            productMenuDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Change start date and end date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void ChangeStartDateAndEndDate(DateTime? startDate, DateTime? endDate)
        {
            log.LogMethodEntry(startDate);
            if (productMenuDTO.StartDate == startDate &&
                productMenuDTO.EndDate == endDate)
            {
                log.LogMethodExit(null, "No changes to productMenu startDate and endDate");
                return;
            }
            ValidateStartDateAndEndDate(startDate, endDate);
            productMenuDTO.StartDate = startDate;
            productMenuDTO.EndDate = endDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Change name
        /// </summary>
        /// <param name="name"></param>
        public void ChangeName(string name)
        {
            log.LogMethodEntry(name);
            if (productMenuDTO.Name == name)
            {
                log.LogMethodExit(null, "No changes to productMenu Name");
                return;
            }
            ValidateName(name);
            productMenuDTO.Name = name;
            log.LogMethodExit();
        }

        /// <summary>
        /// Change menu type
        /// </summary>
        /// <param name="type"></param>
        public void ChangeType(string type)
        {
            log.LogMethodEntry(type);
            if (productMenuDTO.Type == type)
            {
                log.LogMethodExit(null, "No changes to productMenu type");
                return;
            }
            ValidateType(type);
            productMenuDTO.Type = type;
            log.LogMethodExit();
        }

        /// <summary>
        /// Change Description
        /// </summary>
        /// <param name="description"></param>
        public void ChangeDescription(string description)
        {
            log.LogMethodEntry(description);
            if (productMenuDTO.Description == description)
            {
                log.LogMethodExit(null, "No changes to productMenu Description");
                return;
            }
            ValidateDescription(description);
            productMenuDTO.Description = description;
            log.LogMethodExit();
        }

        private void ValidateName(string name)
        {
            log.LogMethodEntry(name);
            if (string.IsNullOrWhiteSpace(name))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Name"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name is empty.", "ProductMenu", "Name", errorMessage);
            }
            if (name.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Name"), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name greater than 100 characters.", "ProductMenu", "Name", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateIsActive(bool isActive, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(isActive);
            if((productMenuDTO != null && productMenuDTO.MenuId > -1) && isActive == false)
            {
                ProductMenuDataHandler productMenuDataHandler = new ProductMenuDataHandler(sqlTransaction);
                bool isRecordReferenced = productMenuDataHandler.GetIsRecordReferenced(productMenuDTO.MenuId);
                if (isRecordReferenced)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1869);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Unable to delete this record. Please check the reference record first.", "ProductMenu", "IsActive", errorMessage);
                }
            }
            log.LogMethodExit();
        }

        private void ValidateType(string type)
        {
            log.LogMethodEntry(type);
            if (ProductMenuType.IsValid(type) == false)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Type")); ;
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Type is empty.", "ProductMenu", "Type", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateDescription(string description)
        {
            log.LogMethodEntry(description);
            if (description.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, description), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("description greater than 100 characters.", "ProductMenu", "description", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateStartDateAndEndDate(DateTime? startDate, DateTime? endDate)
        {
            log.LogMethodEntry(startDate, endDate);
            if (startDate != DateTime.MinValue && startDate != null && endDate != null &&
                endDate != DateTime.MinValue &&
                startDate > endDate)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "End Date"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "ProductMenu", "EndDate", errorMessage);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Builds the child records for ProductMenu object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)    //added build
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            ProductMenuPanelMappingListBL productMenuPanelMappingListBL = new ProductMenuPanelMappingListBL(executionContext);
            List<ProductMenuPanelMappingDTO> productMenuPanelMappingDTOList = productMenuPanelMappingListBL.GetProductMenuPanelMappingDTOList(new List<int>() { productMenuDTO.MenuId }, activeChildRecords, sqlTransaction);
            if (productMenuPanelMappingDTOList.Count != 0 && productMenuPanelMappingDTOList.Any())
            {
                productMenuDTO.ProductMenuPanelMappingDTOList = productMenuPanelMappingDTOList;
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the productMenu
        /// Checks if the User id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductMenuDataHandler productMenuDataHandler = new ProductMenuDataHandler(sqlTransaction);
            SetToSiteTimeOffset();
            if (productMenuDTO.MenuId < 0)
            {
                productMenuDTO = productMenuDataHandler.Insert(productMenuDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productMenuDTO.AcceptChanges();
            }
            else
            {
                if (productMenuDTO.IsChanged)
                {
                    productMenuDTO = productMenuDataHandler.Update(productMenuDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productMenuDTO.AcceptChanges();
                }
                log.LogMethodExit();
            }

            // Will Save the Child ProductMenuPanelMappingDTO
            log.Debug("productMenuDTO.ProductMenuPanelMappingDTO Value :" + productMenuDTO.ProductMenuPanelMappingDTOList);
            if (productMenuDTO.ProductMenuPanelMappingDTOList != null && productMenuDTO.ProductMenuPanelMappingDTOList.Any())
            {
                List<ProductMenuPanelMappingDTO> updatedProductMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
                foreach (ProductMenuPanelMappingDTO productMenuPanelMappingDTO in productMenuDTO.ProductMenuPanelMappingDTOList)
                {
                    if (productMenuPanelMappingDTO.MenuId != productMenuDTO.MenuId)
                    {
                        productMenuPanelMappingDTO.MenuId = productMenuDTO.MenuId;
                    }
                    log.Debug("ProductMenuPanelMappingDTO.IsChanged Value :" + productMenuPanelMappingDTO.IsChanged);
                    if (productMenuPanelMappingDTO.IsChanged)
                    {
                        updatedProductMenuPanelMappingDTOList.Add(productMenuPanelMappingDTO);
                    }
                }
                log.Debug("updatedProductMenuPanelMappingDTO Value :" + updatedProductMenuPanelMappingDTOList);
                if (updatedProductMenuPanelMappingDTOList.Any())
                {
                    ProductMenuPanelMappingListBL productMenuPanelMappingListBL = new ProductMenuPanelMappingListBL(executionContext);
                    productMenuPanelMappingListBL.Save(updatedProductMenuPanelMappingDTOList, sqlTransaction);
                }
            }
            SetFromSiteTimeOffset();
            log.LogMethodExit();
        }



        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductMenuDTO ProductMenuDTO
        {
            get
            {
                ProductMenuDTO result = new ProductMenuDTO(productMenuDTO);
                return result;
            }
        }

        private void SetFromSiteTimeOffset()
        {
            log.LogMethodEntry(productMenuDTO);
            if (SiteContainerList.IsCorporate())
            {
                if (productMenuDTO != null)
                {
                    if (productMenuDTO.StartDate != null)
                    {
                        productMenuDTO.StartDate = SiteContainerList.FromSiteDateTime(productMenuDTO.SiteId, (DateTime)productMenuDTO.StartDate);
                    }
                    if (productMenuDTO.EndDate != null)
                    {
                        productMenuDTO.EndDate = SiteContainerList.FromSiteDateTime(productMenuDTO.SiteId, (DateTime)productMenuDTO.EndDate);
                    }
                    productMenuDTO.AcceptChanges();
                }
            }
            log.LogMethodExit(productMenuDTO);
        }
        private void SetToSiteTimeOffset()
        {
            log.LogMethodEntry(productMenuDTO);
            if (SiteContainerList.IsCorporate())
            {
                if (productMenuDTO != null && (productMenuDTO.MenuId == -1 || productMenuDTO.IsChanged))
                {
                    int siteId = executionContext.GetSiteId();
                    log.Info(siteId);
                    if (productMenuDTO.StartDate != null)
                    {
                        productMenuDTO.StartDate = SiteContainerList.ToSiteDateTime(productMenuDTO.SiteId, (DateTime)productMenuDTO.StartDate);
                    }
                    if (productMenuDTO.EndDate != null)
                    {
                        productMenuDTO.EndDate = SiteContainerList.ToSiteDateTime(productMenuDTO.SiteId, (DateTime)productMenuDTO.EndDate);
                    }
                }
            }
            log.LogMethodExit(productMenuDTO);
        }

    }

    /// <summary>
    /// Manages the list of ProductMenu
    /// </summary>
    /// 

    public class ProductMenuListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductMenuListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public ProductMenuListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductMenu list
        /// </summary>
        public List<ProductMenuDTO> GetProductMenuDTOList(List<KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            ProductMenuDataHandler productMenuDataHandler = new ProductMenuDataHandler(sqlTransaction);
            List<ProductMenuDTO> productMenuDTOsList = productMenuDataHandler.GetProductMenuDTOList(searchParameters, sqlTransaction);
            if (productMenuDTOsList != null && productMenuDTOsList.Any() && loadChildRecords)
            {
                Build(productMenuDTOsList, loadActiveChildRecords, sqlTransaction);
            }
            productMenuDTOsList = SetFromSiteTimeOffset(productMenuDTOsList);
            log.LogMethodExit(productMenuDTOsList);
            return productMenuDTOsList;
        }


        private void Build(List<ProductMenuDTO> productMenuDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productMenuDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ProductMenuDTO> productMenuDTOIdMap = new Dictionary<int, ProductMenuDTO>();
            List<int> productMenuIdList = new List<int>();
            Dictionary<int, ProductMenuDTO> scheduleIdProductMenuDTODictionary = new Dictionary<int, ProductMenuDTO>();
            for (int i = 0; i < productMenuDTOList.Count; i++)
            {
                if (productMenuDTOIdMap.ContainsKey(productMenuDTOList[i].MenuId))
                {
                    continue;
                }
                productMenuDTOIdMap.Add(productMenuDTOList[i].MenuId, productMenuDTOList[i]);
                productMenuIdList.Add(productMenuDTOList[i].MenuId);
            }

            ProductMenuPanelMappingListBL productMenuPanelMappingListBL = new ProductMenuPanelMappingListBL(executionContext);
            List<ProductMenuPanelMappingDTO> productMenuPanelMappingDTOList = productMenuPanelMappingListBL.GetProductMenuPanelMappingDTOList(productMenuIdList, activeChildRecords, sqlTransaction);
            if (productMenuPanelMappingDTOList != null && productMenuPanelMappingDTOList.Any())
            {
                for (int i = 0; i < productMenuPanelMappingDTOList.Count; i++)
                {
                    if (productMenuDTOIdMap.ContainsKey(productMenuPanelMappingDTOList[i].MenuId) == false)
                    {
                        continue;
                    }
                    ProductMenuDTO productMenuDTO = productMenuDTOIdMap[productMenuPanelMappingDTOList[i].MenuId];
                    if (productMenuDTO.ProductMenuPanelMappingDTOList == null)
                    {
                        productMenuDTO.ProductMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
                    }
                    productMenuDTO.ProductMenuPanelMappingDTOList.Add(productMenuPanelMappingDTOList[i]);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This method should be used to Save and Update the ProductMenu.
        /// </summary>
        public List<ProductMenuDTO> Save(List<ProductMenuDTO> productMenuDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ProductMenuDTO> savedProductMenuDTOList = new List<ProductMenuDTO>();
            if (productMenuDTOList == null || productMenuDTOList.Any() == false)
            {
                log.LogMethodExit(savedProductMenuDTOList);
                return savedProductMenuDTOList;
            }
            foreach (ProductMenuDTO productMenuDTO in productMenuDTOList)
            {
                ProductMenuBL productMenuBL = new ProductMenuBL(executionContext, productMenuDTO, sqlTransaction);
                productMenuBL.Save(sqlTransaction);
                savedProductMenuDTOList.Add(productMenuBL.ProductMenuDTO);
            }
            log.LogMethodExit(savedProductMenuDTOList);
            return savedProductMenuDTOList;
        }

        /// <summary>
        /// Product Menu Module Last Update Date
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public DateTime? GetProductMenuModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductMenuDataHandler productMenuDataHandler = new ProductMenuDataHandler();
            DateTime? result = productMenuDataHandler.GetProductMenuModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        private List<ProductMenuDTO> SetFromSiteTimeOffset(List<ProductMenuDTO> productMenuDTOList)
        {
            log.LogMethodEntry(productMenuDTOList);
            if (SiteContainerList.IsCorporate())
            {
                if (productMenuDTOList != null && productMenuDTOList.Any())
                {
                    for (int i = 0; i < productMenuDTOList.Count; i++)
                    {
                        if (productMenuDTOList[i].StartDate != null)
                        {
                            productMenuDTOList[i].StartDate = SiteContainerList.FromSiteDateTime(productMenuDTOList[i].SiteId, (DateTime)productMenuDTOList[i].StartDate);
                        }
                        if (productMenuDTOList[i].EndDate != null)
                        {
                            productMenuDTOList[i].EndDate = SiteContainerList.FromSiteDateTime(productMenuDTOList[i].SiteId, (DateTime)productMenuDTOList[i].EndDate);
                        }
                        productMenuDTOList[i].AcceptChanges();
                    }
                }
            }
            log.LogMethodExit(productMenuDTOList);
            return productMenuDTOList;
        }

    }
}