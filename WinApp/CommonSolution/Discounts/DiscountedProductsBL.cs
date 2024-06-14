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
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Discounts
{
    public class DiscountedProductsBL
    {
        private DiscountedProductsDTO discountedProductsDTO;
        private readonly Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;

        /// <summary>
        /// Default constructor of DiscountedProductsBL class
        /// </summary>
        private DiscountedProductsBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DiscountedProductsId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DiscountedProductsBL(ExecutionContext executionContext, int id, UnitOfWork unitOfWork)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, id, unitOfWork);
            DiscountedProductsDataHandler discountedProductsDataHandler = new DiscountedProductsDataHandler(executionContext, unitOfWork);
            discountedProductsDTO = discountedProductsDataHandler.GetDiscountedProducts(id);
            if (discountedProductsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "DiscountedProducts", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DiscountedProductsBL object using the DiscountedProductsDTO
        /// </summary>
        public DiscountedProductsBL(ExecutionContext executionContext, DiscountedProductsDTO discountedProductsDTO, UnitOfWork unitOfWork)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, discountedProductsDTO, unitOfWork);
            if (discountedProductsDTO.Id < 0)
            {
                ValidateCriteria(discountedProductsDTO.ProductId,
                                 discountedProductsDTO.CategoryId,
                                 discountedProductsDTO.ProductGroupId);
                ValidateQuantity(discountedProductsDTO.Quantity);
                ValidateDiscountedPrice(discountedProductsDTO.DiscountedPrice);
                ValidateDiscountAmount(discountedProductsDTO.DiscountAmount);
                ValidateDiscountPercentage(discountedProductsDTO.DiscountPercentage);
                ValidateDiscount(discountedProductsDTO.DiscountAmount,
                                 discountedProductsDTO.DiscountedPrice,
                                 discountedProductsDTO.DiscountPercentage);
            }
            this.discountedProductsDTO = discountedProductsDTO;
            log.LogMethodExit();
        }

        public void Update(DiscountedProductsDTO parameterDiscountedProductsDTO)
        {
            log.LogMethodEntry(parameterDiscountedProductsDTO);
            ValidateCriteria(parameterDiscountedProductsDTO.ProductId,
                             parameterDiscountedProductsDTO.CategoryId,
                             parameterDiscountedProductsDTO.ProductGroupId);
            ValidateDiscount(parameterDiscountedProductsDTO.DiscountAmount,
                             parameterDiscountedProductsDTO.DiscountedPrice,
                             parameterDiscountedProductsDTO.DiscountPercentage);
            ChangeProductId(parameterDiscountedProductsDTO.ProductId);
            ChangeCategoryId(parameterDiscountedProductsDTO.CategoryId);
            ChangeProductGroupId(parameterDiscountedProductsDTO.ProductGroupId);
            ChangeQuantity(parameterDiscountedProductsDTO.Quantity);
            ChangeIsActive(parameterDiscountedProductsDTO.IsActive);
            ChangeDiscounted(parameterDiscountedProductsDTO.Discounted);
            ChangeDiscountAmount(parameterDiscountedProductsDTO.DiscountAmount);
            ChangeDiscountedPrice(parameterDiscountedProductsDTO.DiscountedPrice);
            ChangeDiscountPercentage(parameterDiscountedProductsDTO.DiscountPercentage);
            log.LogMethodExit();
        }

        private void ChangeProductId(int productId)
        {
            log.LogMethodEntry(productId);
            if (discountedProductsDTO.ProductId == productId)
            {
                log.LogMethodExit(null, "No changes to DiscountedProducts productId");
                return;
            }
            discountedProductsDTO.ProductId = productId;
            log.LogMethodExit();
        }

        private void ChangeCategoryId(int categoryId)
        {
            log.LogMethodEntry(categoryId);
            if (discountedProductsDTO.CategoryId == categoryId)
            {
                log.LogMethodExit(null, "No changes to DiscountedProducts categoryId");
                return;
            }
            discountedProductsDTO.CategoryId = categoryId;
            log.LogMethodExit();
        }

        private void ChangeProductGroupId(int productGroupId)
        {
            log.LogMethodEntry(productGroupId);
            if (discountedProductsDTO.ProductGroupId == productGroupId)
            {
                log.LogMethodExit(null, "No changes to DiscountedProducts productGroupId");
                return;
            }
            discountedProductsDTO.ProductGroupId = productGroupId;
            log.LogMethodExit();
        }

        private void ChangeQuantity(int? quantity)
        {
            log.LogMethodEntry(quantity);
            if (discountedProductsDTO.Quantity == quantity)
            {
                log.LogMethodExit(null, "No changes to DiscountedProducts quantity");
                return;
            }
            ValidateQuantity(quantity);
            discountedProductsDTO.Quantity = quantity;
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (discountedProductsDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to DiscountedProducts isActive");
                return;
            }
            discountedProductsDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeDiscounted(string discounted)
        {
            log.LogMethodEntry(discounted);
            if (discountedProductsDTO.Discounted == discounted)
            {
                log.LogMethodExit(null, "No changes to DiscountedProducts discounted");
                return;
            }
            discountedProductsDTO.Discounted = discounted;
            log.LogMethodExit();
        }

        private void ChangeDiscountAmount(double? discountAmount)
        {
            log.LogMethodEntry(discountAmount);
            if (discountedProductsDTO.DiscountAmount == discountAmount)
            {
                log.LogMethodExit(null, "No changes to DiscountedProducts discountAmount");
                return;
            }
            ValidateDiscountAmount(discountAmount);
            discountedProductsDTO.DiscountAmount = discountAmount;
            log.LogMethodExit();
        }

        private void ChangeDiscountedPrice(decimal? discountedPrice)
        {
            log.LogMethodEntry(discountedPrice);
            if (discountedProductsDTO.DiscountedPrice == discountedPrice)
            {
                log.LogMethodExit(null, "No changes to DiscountedProducts discountedPrice");
                return;
            }
            ValidateDiscountedPrice(discountedPrice);
            discountedProductsDTO.DiscountedPrice = discountedPrice;
            log.LogMethodExit();
        }

        private void ChangeDiscountPercentage(double? discountPercentage)
        {
            log.LogMethodEntry(discountPercentage);
            if (discountedProductsDTO.DiscountPercentage == discountPercentage)
            {
                log.LogMethodExit(null, "No changes to DiscountedProducts discountPercentage");
                return;
            }
            ValidateDiscountPercentage(discountPercentage);
            discountedProductsDTO.DiscountPercentage = discountPercentage;
            log.LogMethodExit();
        }

        private void ValidateQuantity(int? quantity)
        {
            log.LogMethodEntry(quantity);
            if (quantity.HasValue && quantity <= -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Min Quantity"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Minimum quantity can't be negative.", "DiscountedProducts", "Quantity", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateDiscountAmount(double? discountAmount)
        {
            log.LogMethodEntry(discountAmount);
            if (discountAmount.HasValue && discountAmount <= -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Discount Amount"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Discount Amount can't be negative.", "DiscountedProducts", "DiscountAmount", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateDiscountedPrice(decimal? discountedPrice)
        {
            log.LogMethodEntry(discountedPrice);
            if (discountedPrice.HasValue && discountedPrice <= -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Discounted Price"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Discounted Price can't be negative.", "DiscountedProducts", "DiscountedPrice", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateDiscountPercentage(double? discountPercentage)
        {
            log.LogMethodEntry(discountPercentage);
            if (discountPercentage.HasValue && discountPercentage <= -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Discount Percentage"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Discount Percentage can't be negative.", "DiscountedProducts", "DiscountPercentage", errorMessage);
            }
            if (discountPercentage.HasValue && discountPercentage > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1876);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Percentage should not exceed 100%.", "DiscountedProducts", "DiscountPercentage", errorMessage);
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
                throw new ValidationException("Product is empty.", "DiscountedProducts", "ProductId", errorMessage);
            }
            if ((productId > -1 && categoryId > -1) || (productId > -1 && productGroupId > -1) ||
                (categoryId > -1 && productGroupId > -1))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4993);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Either product, category or product group can be set.", "DiscountedProducts", "ProductId", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateDiscount(double? discountAmount, decimal? discountedPrice, double? discountPercentage)
        {
            log.LogMethodEntry(discountAmount, discountedPrice, discountPercentage);
            if ((discountAmount.HasValue && (discountedPrice.HasValue || discountPercentage.HasValue)) ||
                (discountedPrice.HasValue &&(discountAmount.HasValue || discountPercentage.HasValue)) ||
                (discountPercentage.HasValue && (discountAmount.HasValue || discountedPrice.HasValue)))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4992);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Only Discount Amount, Discounted Price or Discount Percentage can be set.", "DiscountedProducts", "DiscountAmount", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the DiscountedProducts
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            DiscountedProductsDataHandler discountedProductsDataHandler = new DiscountedProductsDataHandler(executionContext, unitOfWork);
            discountedProductsDTO = SiteContainerList.ToSiteDateTime(executionContext, discountedProductsDTO);
            discountedProductsDataHandler.Save(discountedProductsDTO);
            discountedProductsDTO = SiteContainerList.FromSiteDateTime(executionContext, discountedProductsDTO);
            discountedProductsDTO.AcceptChanges();
            log.LogMethodExit();
        }

        #region Properties
        /// <summary>
        /// Gets the DTO
        /// </summary>
        internal DiscountedProductsDTO DiscountedProductsDTO
        {
            get
            {
                return discountedProductsDTO;
            }
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [Browsable(false)]
        public int Id
        {
            get
            {
                return discountedProductsDTO.Id;
            }
        }

        /// <summary>
        /// Get/Set method of the discountId field
        /// </summary>
        [Browsable(false)]
        public int DiscountId
        {
            get
            {
                return discountedProductsDTO.DiscountId;
            }

        }

        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        [DisplayName("Category")]
        public int CategoryId
        {
            get
            {
                return discountedProductsDTO.CategoryId;
            }

           
        }

        /// <summary>
        /// Get/Set method of the ProductGroupId field
        /// </summary>
        [DisplayName("Product Group")]
        public int ProductGroupId
        {
            get
            {
                return discountedProductsDTO.ProductGroupId;
            }

            
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product")]
        public int ProductId
        {
            get
            {
                return discountedProductsDTO.ProductId;
            }

            
        }

        /// <summary>
        /// Get/Set method of the Discounted field
        /// </summary>
        [DisplayName("Discounted")]
        public string Discounted
        {
            get
            {
                return discountedProductsDTO.Discounted;
            }

            
        }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public int? Quantity
        {
            get
            {
                return discountedProductsDTO.Quantity;
            }

            
        }

        /// <summary>
        /// Get/Set method of the DiscountPercentage field
        /// </summary>
        [DisplayName("Discount Percentage")]
        public double? DiscountPercentage
        {
            get
            {
                return discountedProductsDTO.DiscountPercentage;
            }

            
        }

        /// <summary>
        /// Get/Set method of the DiscountAmount field
        /// </summary>
        [DisplayName("Discount Amount")]
        public double? DiscountAmount
        {
            get
            {
                return discountedProductsDTO.DiscountAmount;
            }

            
        }

        /// <summary>
        /// Get/Set method of the DiscountedPrice field
        /// </summary>
        [DisplayName("Discounted Price")]
        public decimal? DiscountedPrice
        {
            get
            {
                return discountedProductsDTO.DiscountedPrice;
            }

            
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive
        {
            get
            {
                return discountedProductsDTO.IsActive;
            }

            
        }


        #endregion
    }

    /// <summary>
    /// Manages the list of DiscountedProducts
    /// </summary>
    public class DiscountedProductsListBL
    {
        private readonly Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;

        /// <summary>
        /// Parameterized constructor of DiscountedProductsListBL class
        /// </summary>
        public DiscountedProductsListBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates and saves the DiscountedProductsDTOList to the db
        /// </summary>
        public void Save(List<DiscountedProductsDTO> discountedProductsDTOList)
        {
            log.LogMethodEntry(discountedProductsDTOList);
            if (discountedProductsDTOList == null ||
                discountedProductsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            foreach (var discountedProductsDTO in discountedProductsDTOList)
            {
                DiscountedProductsBL discountedProductsBL = new DiscountedProductsBL(executionContext, discountedProductsDTO, unitOfWork);
                discountedProductsBL.Save();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DiscountedProductsDTO List for Discount Id List
        /// </summary>
        /// <param name="discountIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of DiscountedProductsDTO</returns>
        public List<DiscountedProductsDTO> GetDiscountedProductsDTOListOfDiscounts(List<int> discountIdList, bool activeRecords = true, bool onlyDiscountedChildRecord = false)
        {
            log.LogMethodEntry(discountIdList);
            DiscountedProductsDataHandler discountedProductsDataHandler = new DiscountedProductsDataHandler(executionContext, unitOfWork);
            List<DiscountedProductsDTO> discountedProductsDTOList = discountedProductsDataHandler.GetDiscountedProductsDTOListOfDiscounts(discountIdList, activeRecords, onlyDiscountedChildRecord);
            log.LogMethodExit(discountedProductsDTOList);
            return discountedProductsDTOList;
        }
    }
}