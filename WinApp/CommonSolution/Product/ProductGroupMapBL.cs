/********************************************************************************************
 * Project Name - Product
 * Description  - Product Group Map Business object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 ********************************************************************************************* 
 *2.170.0     05-Jul-2023      Lakshminarayana     Created
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
    public class ProductGroupMapBL
    {
        private ProductGroupMapDTO productGroupMapDTO;
        private readonly Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;

        /// <summary>
        /// Default constructor of ProductGroupMapBL class
        /// </summary>
        private ProductGroupMapBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductGroupMapId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProductGroupMapBL(ExecutionContext executionContext, int id, UnitOfWork unitOfWork)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, id, unitOfWork);
            ProductGroupMapDataHandler productGroupMapDataHandler = new ProductGroupMapDataHandler(executionContext, unitOfWork);
            productGroupMapDTO = productGroupMapDataHandler.GetProductGroupMap(id);
            if (productGroupMapDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductGroupMap", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ProductGroupMapBL object using the ProductGroupMapDTO
        /// </summary>
        public ProductGroupMapBL(ExecutionContext executionContext, ProductGroupMapDTO productGroupMapDTO, UnitOfWork unitOfWork)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, productGroupMapDTO, unitOfWork);
            if (productGroupMapDTO.Id < 0)
            {
                ValidateProductId(productGroupMapDTO.ProductId);
                ValidateSortOrder(productGroupMapDTO.SortOrder);
            }
            this.productGroupMapDTO = productGroupMapDTO;
            log.LogMethodExit();
        }

        public void Update(ProductGroupMapDTO parameterProductGroupMapDTO)
        {
            log.LogMethodEntry(parameterProductGroupMapDTO);
            ChangeProductId(parameterProductGroupMapDTO.ProductId);
            ChangeSortOrder(parameterProductGroupMapDTO.SortOrder);
            ChangeIsActive(parameterProductGroupMapDTO.IsActive);
            log.LogMethodExit();
        }

        private void ChangeProductId(int productId)
        {
            log.LogMethodEntry(productId);
            if (productGroupMapDTO.ProductId == productId)
            {
                log.LogMethodExit(null, "No changes to ProductGroupMap productId");
                return;
            }
            ValidateProductId(productId);
            productGroupMapDTO.ProductId = productId;
            log.LogMethodExit();
        }

        private void ChangeSortOrder(int sortOrder)
        {
            log.LogMethodEntry(sortOrder);
            if (productGroupMapDTO.SortOrder == sortOrder)
            {
                log.LogMethodExit(null, "No changes to ProductGroupMap sortOrder");
                return;
            }
            ValidateSortOrder(sortOrder);
            productGroupMapDTO.SortOrder = sortOrder;
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (productGroupMapDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to ProductGroupMap isActive");
                return;
            }
            productGroupMapDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ValidateProductId(int productId)
        {
            log.LogMethodEntry(productId);
            if(productId <= -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Product"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Product is empty.", "ProductGroupMap", "ProductId", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateSortOrder(int sortOrder)
        {
            log.LogMethodEntry(sortOrder);
            if (sortOrder <= -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Sort Order"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Sort Order should be positive integer.", "ProductGroupMap", "SortOrder", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ProductGroupMap
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            if (productGroupMapDTO.IsChanged)
            {
                ProductGroupMapDataHandler productGroupMapDataHandler = new ProductGroupMapDataHandler(executionContext, unitOfWork);
                productGroupMapDTO = SiteContainerList.ToSiteDateTime(executionContext, productGroupMapDTO);
                productGroupMapDataHandler.Save(productGroupMapDTO);
                productGroupMapDTO = SiteContainerList.FromSiteDateTime(executionContext, productGroupMapDTO);
                productGroupMapDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        internal ProductGroupMapDTO ProductGroupMapDTO
        {
            get
            {
                return productGroupMapDTO;
            }
        }

        /// <summary>
        /// Get method of the Id field
        /// </summary>
        public int Id { get { return productGroupMapDTO.Id; }}

        /// <summary>
        /// Get method of the productGroupId field
        /// </summary>
        public int ProductGroupId { get { return productGroupMapDTO.ProductGroupId; }  }

        /// <summary>
        /// Get method of the productId field
        /// </summary>     
        public int ProductId { get { return productGroupMapDTO.ProductId; } }

        /// <summary>
        /// Get method of the sortOrder field
        /// </summary>
        public int SortOrder { get { return productGroupMapDTO.SortOrder; }  }

        /// Get method of the isActive field
        /// </summary>      
        public bool IsActive { get { return productGroupMapDTO.IsActive; }  }

    }

    /// <summary>
    /// Manages the list of ProductGroupMap
    /// </summary>
    public class ProductGroupMapListBL
    {
        private readonly Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;

        /// <summary>
        /// Parameterized constructor of ProductGroupMapListBL class
        /// </summary>
        public ProductGroupMapListBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates and saves the ProductGroupMapDTOList to the db
        /// </summary>
        public void Save(List<ProductGroupMapDTO> productGroupMapDTOList)
        {
            log.LogMethodEntry(productGroupMapDTOList);
            if (productGroupMapDTOList == null ||
                productGroupMapDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            foreach (var productGroupMapDTO in productGroupMapDTOList)
            {
                ProductGroupMapBL productGroupMapBL = new ProductGroupMapBL(executionContext, productGroupMapDTO, unitOfWork);
                productGroupMapBL.Save();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProductGroupMapDTO List for ProductGroupIdList
        /// </summary>
        public List<ProductGroupMapDTO> GetProductGroupMapDTOList(List<int> productGroupIdList, bool activeRecords = true)
        {
            log.LogMethodEntry(productGroupIdList, activeRecords);
            ProductGroupMapDataHandler productGroupMapDataHandler = new ProductGroupMapDataHandler(executionContext, unitOfWork);
            List<ProductGroupMapDTO> productGroupMapDTOList = productGroupMapDataHandler.GetProductGroupMapDTOListOfProductGroups(productGroupIdList, activeRecords);
            log.LogMethodExit(productGroupMapDTOList);
            return productGroupMapDTOList;
        }
    }
}