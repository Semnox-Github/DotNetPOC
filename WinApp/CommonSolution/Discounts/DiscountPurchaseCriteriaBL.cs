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

namespace Semnox.Parafait.Discounts
{
    public class DiscountPurchaseCriteriaBL
    {
        private DiscountPurchaseCriteriaDTO discountPurchaseCriteriaDTO;
        private readonly Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;

        /// <summary>
        /// Default constructor of DiscountPurchaseCriteriaBL class
        /// </summary>
        private DiscountPurchaseCriteriaBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DiscountPurchaseCriteriaId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DiscountPurchaseCriteriaBL(ExecutionContext executionContext, int id, UnitOfWork unitOfWork)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, id, unitOfWork);
            DiscountPurchaseCriteriaDataHandler discountPurchaseCriteriaDataHandler = new DiscountPurchaseCriteriaDataHandler(executionContext, unitOfWork);
            discountPurchaseCriteriaDTO = discountPurchaseCriteriaDataHandler.GetDiscountPurchaseCriteria(id);
            if (discountPurchaseCriteriaDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "DiscountPurchaseCriteria", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DiscountPurchaseCriteriaBL object using the DiscountPurchaseCriteriaDTO
        /// </summary>
        public DiscountPurchaseCriteriaBL(ExecutionContext executionContext, DiscountPurchaseCriteriaDTO discountPurchaseCriteriaDTO, UnitOfWork unitOfWork)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, discountPurchaseCriteriaDTO, unitOfWork);
            if (discountPurchaseCriteriaDTO.CriteriaId < 0)
            {
                ValidateCriteria(discountPurchaseCriteriaDTO.ProductId, 
                                 discountPurchaseCriteriaDTO.CategoryId, 
                                 discountPurchaseCriteriaDTO.ProductGroupId);
                ValidateMinQuantity(discountPurchaseCriteriaDTO.MinQuantity);
            }
            this.discountPurchaseCriteriaDTO = discountPurchaseCriteriaDTO;
            log.LogMethodExit();
        }

        public void Update(DiscountPurchaseCriteriaDTO parameterDiscountPurchaseCriteriaDTO)
        {
            log.LogMethodEntry(parameterDiscountPurchaseCriteriaDTO);
            ChangeProductId(parameterDiscountPurchaseCriteriaDTO.ProductId);
            ChangeCategoryId(parameterDiscountPurchaseCriteriaDTO.CategoryId);
            ChangeProductGroupId(parameterDiscountPurchaseCriteriaDTO.ProductGroupId);
            ChangeMinQuantity(parameterDiscountPurchaseCriteriaDTO.MinQuantity);
            ChangeIsActive(parameterDiscountPurchaseCriteriaDTO.IsActive);
            ValidateCriteria(discountPurchaseCriteriaDTO.ProductId,
                             discountPurchaseCriteriaDTO.CategoryId,
                             discountPurchaseCriteriaDTO.ProductGroupId);
            log.LogMethodExit();
        }

        private void ChangeProductId(int productId)
        {
            log.LogMethodEntry(productId);
            if (discountPurchaseCriteriaDTO.ProductId == productId)
            {
                log.LogMethodExit(null, "No changes to DiscountPurchaseCriteria productId");
                return;
            }
            discountPurchaseCriteriaDTO.ProductId = productId;
            log.LogMethodExit();
        }

        private void ChangeCategoryId(int categoryId)
        {
            log.LogMethodEntry(categoryId);
            if (discountPurchaseCriteriaDTO.CategoryId == categoryId)
            {
                log.LogMethodExit(null, "No changes to DiscountPurchaseCriteria categoryId");
                return;
            }
            discountPurchaseCriteriaDTO.CategoryId = categoryId;
            log.LogMethodExit();
        }

        private void ChangeProductGroupId(int productGroupId)
        {
            log.LogMethodEntry(productGroupId);
            if (discountPurchaseCriteriaDTO.ProductGroupId == productGroupId)
            {
                log.LogMethodExit(null, "No changes to DiscountPurchaseCriteria productGroupId");
                return;
            }
            discountPurchaseCriteriaDTO.ProductGroupId = productGroupId;
            log.LogMethodExit();
        }

        private void ChangeMinQuantity(int? minQuantity)
        {
            log.LogMethodEntry(minQuantity);
            if (discountPurchaseCriteriaDTO.MinQuantity == minQuantity)
            {
                log.LogMethodExit(null, "No changes to DiscountPurchaseCriteria minQuantity");
                return;
            }
            ValidateMinQuantity(minQuantity);
            discountPurchaseCriteriaDTO.MinQuantity = minQuantity;
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (discountPurchaseCriteriaDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to DiscountPurchaseCriteria isActive");
                return;
            }
            discountPurchaseCriteriaDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ValidateMinQuantity(int? minQuantity)
        {
            log.LogMethodEntry(minQuantity);
            if (minQuantity.HasValue && minQuantity <= -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Min Quantity"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Minimum quantity can't be negative.", "DiscountPurchaseCriteria", "MinQuantity", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateCriteria(int productId, int categoryId, int productGroupId)
        {
            log.LogMethodEntry(productId, categoryId, productGroupId);
            if (productId <= -1 && categoryId <= -1 && productGroupId <= -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Product"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Product is empty.", "DiscountPurchaseCriteria", "ProductId", errorMessage);
            }
            if ((productId > -1 && categoryId > -1) || (productId > -1 && productGroupId > -1) ||
                (categoryId > -1 && productGroupId > -1))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4993);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Either product, category or product group can be set.", "DiscountPurchaseCriteria", "ProductId", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the DiscountPurchaseCriteria
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            DiscountPurchaseCriteriaDataHandler discountPurchaseCriteriaDataHandler = new DiscountPurchaseCriteriaDataHandler(executionContext, unitOfWork);
            discountPurchaseCriteriaDTO = SiteContainerList.ToSiteDateTime(executionContext, discountPurchaseCriteriaDTO);
            discountPurchaseCriteriaDataHandler.Save(discountPurchaseCriteriaDTO);
            discountPurchaseCriteriaDTO = SiteContainerList.FromSiteDateTime(executionContext, discountPurchaseCriteriaDTO);
            discountPurchaseCriteriaDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        internal DiscountPurchaseCriteriaDTO DiscountPurchaseCriteriaDTO
        {
            get
            {
                return discountPurchaseCriteriaDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of DiscountPurchaseCriteria
    /// </summary>
    public class DiscountPurchaseCriteriaListBL
    {
        private readonly Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;

        /// <summary>
        /// Parameterized constructor of DiscountPurchaseCriteriaListBL class
        /// </summary>
        public DiscountPurchaseCriteriaListBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates and saves the DiscountPurchaseCriteriaDTOList to the db
        /// </summary>
        public void Save(List<DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTOList)
        {
            log.LogMethodEntry(discountPurchaseCriteriaDTOList);
            if (discountPurchaseCriteriaDTOList == null ||
                discountPurchaseCriteriaDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            foreach (var discountPurchaseCriteriaDTO in discountPurchaseCriteriaDTOList)
            {
                DiscountPurchaseCriteriaBL discountPurchaseCriteriaBL = new DiscountPurchaseCriteriaBL(executionContext, discountPurchaseCriteriaDTO, unitOfWork);
                discountPurchaseCriteriaBL.Save();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DiscountPurchaseCriteriaDTO List for Discount Id List
        /// </summary>
        /// <param name="discountIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of DiscountPurchaseCriteriaDTO</returns>
        public List<DiscountPurchaseCriteriaDTO> GetDiscountPurchaseCriteriaDTOListOfDiscounts(List<int> discountIdList, bool activeRecords = true)
        {
            log.LogMethodEntry(discountIdList);
            DiscountPurchaseCriteriaDataHandler discountPurchaseCriteriaDataHandler = new DiscountPurchaseCriteriaDataHandler(executionContext, unitOfWork);
            List<DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTOList = discountPurchaseCriteriaDataHandler.GetDiscountPurchaseCriteriaDTOListOfDiscounts(discountIdList, activeRecords);
            log.LogMethodExit(discountPurchaseCriteriaDTOList);
            return discountPurchaseCriteriaDTOList;
        }
    }
}