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
using Semnox.Parafait.Product;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Discounts
{
    public class DiscountsBL
    {
        /// <summary>
        /// Discounts applicable for transactions
        /// </summary>
        public const string DISCOUNT_TYPE_TRANSACTION = "T";
        /// <summary>
        /// Discounts applicable for game plays
        /// </summary>
        public const string DISCOUNT_TYPE_GAMEPLAY = "G";
        /// <summary>
        /// Discounts applicable for loyalty
        /// </summary>
        public const string DISCOUNT_TYPE_LOYALTY = "L";

        private DiscountsDTO discountsDTO;
        private readonly Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;

        /// <summary>
        /// Default constructor of DiscountsBL class
        /// </summary>
        private DiscountsBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DiscountsId parameter
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">To load the child DTO Records</param>
        public DiscountsBL(ExecutionContext executionContext, UnitOfWork unitOfWork,
                              int id, bool loadChildRecords = true,
                              bool activeChildRecords = true)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, unitOfWork, id, loadChildRecords, activeChildRecords);
            LoadDiscounts(id, loadChildRecords, activeChildRecords);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Discounts id as the parameter
        /// Would fetch the Discounts object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        private void LoadDiscounts(int id, bool loadChildRecords = false, bool activeChildRecords = false)
        {
            log.LogMethodEntry(id, loadChildRecords, activeChildRecords);
            DiscountsDataHandler discountsDataHandler = new DiscountsDataHandler(executionContext, unitOfWork);
            discountsDTO = discountsDataHandler.GetDiscounts(id);
            ThrowIfDiscountsIsNull(id);
            if (loadChildRecords)
            {
                Build(activeChildRecords);
            }
            log.LogMethodExit(discountsDTO);
        }

        private void ThrowIfDiscountsIsNull(int menuId)
        {
            log.LogMethodEntry();
            if (discountsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Discount", menuId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterDiscountsDTO">parameterDiscountsDTO</param>
        /// <param name="unitOfWork">unitOfWork</param>
        public DiscountsBL(ExecutionContext executionContext, DiscountsDTO parameterDiscountsDTO, UnitOfWork unitOfWork)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, parameterDiscountsDTO, unitOfWork);

            if (parameterDiscountsDTO.DiscountId > -1)
            {
                LoadDiscounts(parameterDiscountsDTO.DiscountId, true, true);//added sql
                ThrowIfDiscountsIsNull(parameterDiscountsDTO.DiscountId);
                Update(parameterDiscountsDTO);
            }
            else
            {
                ValidateDiscountName(parameterDiscountsDTO.DiscountName);
                ValidateDiscountPercentage(parameterDiscountsDTO.DiscountPercentage);
                ValidateMinimumSaleAmount(parameterDiscountsDTO.MinimumSaleAmount);
                ValidateMinimumCredits(parameterDiscountsDTO.MinimumCredits);
                ValidateSortOrder(parameterDiscountsDTO.SortOrder);
                ValidateDiscountType(parameterDiscountsDTO.DiscountType);
                ValidateDiscountAmount(parameterDiscountsDTO.DiscountAmount);
                ValidateApplicationLimit(parameterDiscountsDTO.ApplicationLimit);
                ValidateIsActive(parameterDiscountsDTO.IsActive);
                discountsDTO = new DiscountsDTO(-1, parameterDiscountsDTO.DiscountName,
                                                parameterDiscountsDTO.DiscountPercentage, parameterDiscountsDTO.AutomaticApply,
                                                parameterDiscountsDTO.MinimumSaleAmount, parameterDiscountsDTO.MinimumCredits,
                                                parameterDiscountsDTO.DisplayInPOS, parameterDiscountsDTO.SortOrder,
                                                parameterDiscountsDTO.ManagerApprovalRequired, parameterDiscountsDTO.InternetKey,
                                                parameterDiscountsDTO.DiscountType, parameterDiscountsDTO.CouponMandatory,
                                                parameterDiscountsDTO.DiscountAmount, parameterDiscountsDTO.RemarksMandatory,
                                                parameterDiscountsDTO.VariableDiscounts, parameterDiscountsDTO.ScheduleId,
                                                parameterDiscountsDTO.TransactionProfileId, parameterDiscountsDTO.IsActive,
                                                parameterDiscountsDTO.DiscountCriteriaLines, parameterDiscountsDTO.AllowMultipleApplication,
                                                parameterDiscountsDTO.ApplicationLimit);
                if (parameterDiscountsDTO.DiscountedProductsDTOList != null && parameterDiscountsDTO.DiscountedProductsDTOList.Any())
                {
                    discountsDTO.DiscountedProductsDTOList = new List<DiscountedProductsDTO>();
                    foreach (DiscountedProductsDTO parameterDiscountedProductsDTO in parameterDiscountsDTO.DiscountedProductsDTOList)
                    {
                        if (parameterDiscountedProductsDTO.Id > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "DiscountedProducts", parameterDiscountedProductsDTO.Id);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var discountedProductsDTO = new DiscountedProductsDTO(-1, -1,
                                                                        parameterDiscountedProductsDTO.ProductId,
                                                                        parameterDiscountedProductsDTO.Discounted,
                                                                        parameterDiscountedProductsDTO.IsActive,
                                                                        parameterDiscountedProductsDTO.CategoryId,
                                                                        parameterDiscountedProductsDTO.ProductGroupId,
                                                                        parameterDiscountedProductsDTO.Quantity,
                                                                        parameterDiscountedProductsDTO.DiscountPercentage,
                                                                        parameterDiscountedProductsDTO.DiscountAmount,
                                                                        parameterDiscountedProductsDTO.DiscountedPrice);
                        DiscountedProductsBL discountedProductsBL = new DiscountedProductsBL(executionContext, discountedProductsDTO, unitOfWork);
                        discountsDTO.DiscountedProductsDTOList.Add(discountedProductsBL.DiscountedProductsDTO);
                    }
                }
                if (parameterDiscountsDTO.DiscountedGamesDTOList != null && parameterDiscountsDTO.DiscountedGamesDTOList.Any())
                {
                    discountsDTO.DiscountedGamesDTOList = new List<DiscountedGamesDTO>();
                    foreach (DiscountedGamesDTO parameterDiscountedGamesDTO in parameterDiscountsDTO.DiscountedGamesDTOList)
                    {
                        if (parameterDiscountedGamesDTO.Id > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "DiscountedGames", parameterDiscountedGamesDTO.Id);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var discountedGamesDTO = new DiscountedGamesDTO(-1, -1,
                                                                        parameterDiscountedGamesDTO.GameId,
                                                                        parameterDiscountedGamesDTO.Discounted,
                                                                        parameterDiscountedGamesDTO.IsActive);
                        DiscountedGamesBL discountedGamesBL = new DiscountedGamesBL(executionContext, discountedGamesDTO, unitOfWork);
                        discountsDTO.DiscountedGamesDTOList.Add(discountedGamesBL.DiscountedGamesDTO);
                    }
                }
                if (parameterDiscountsDTO.DiscountPurchaseCriteriaDTOList != null && parameterDiscountsDTO.DiscountPurchaseCriteriaDTOList.Any())
                {
                    discountsDTO.DiscountPurchaseCriteriaDTOList = new List<DiscountPurchaseCriteriaDTO>();
                    foreach (DiscountPurchaseCriteriaDTO parameterDiscountPurchaseCriteriaDTO in parameterDiscountsDTO.DiscountPurchaseCriteriaDTOList)
                    {
                        if (parameterDiscountPurchaseCriteriaDTO.CriteriaId > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "DiscountPurchaseCriteria", parameterDiscountPurchaseCriteriaDTO.CriteriaId);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var discountPurchaseCriteriaDTO = new DiscountPurchaseCriteriaDTO(-1, -1,
                                                                                          parameterDiscountPurchaseCriteriaDTO.ProductId,
                                                                                          parameterDiscountPurchaseCriteriaDTO.CategoryId,
                                                                                          parameterDiscountPurchaseCriteriaDTO.ProductGroupId,
                                                                                          parameterDiscountPurchaseCriteriaDTO.MinQuantity,
                                                                                          parameterDiscountPurchaseCriteriaDTO.IsActive);
                        DiscountPurchaseCriteriaBL discountPurchaseCriteriaBL = new DiscountPurchaseCriteriaBL(executionContext, discountPurchaseCriteriaDTO, unitOfWork);
                        discountsDTO.DiscountPurchaseCriteriaDTOList.Add(discountPurchaseCriteriaBL.DiscountPurchaseCriteriaDTO);
                    }
                }
                if (parameterDiscountsDTO.ScheduleCalendarDTO != null)
                {
                    if (parameterDiscountsDTO.ScheduleCalendarDTO.ScheduleId > -1)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 2196, "ScheduleCalendar", parameterDiscountsDTO.ScheduleCalendarDTO.ScheduleId);
                        log.LogMethodExit(null, "Throwing Exception - " + message);
                        throw new EntityNotFoundException(message);
                    }
                    var scheduleCalendarDTO = new ScheduleCalendarDTO(-1,
                                                                      parameterDiscountsDTO.ScheduleCalendarDTO.ScheduleName,
                                                                      parameterDiscountsDTO.ScheduleCalendarDTO.ScheduleTime,
                                                                      parameterDiscountsDTO.ScheduleCalendarDTO.ScheduleEndDate,
                                                                      parameterDiscountsDTO.ScheduleCalendarDTO.RecurFlag,
                                                                      parameterDiscountsDTO.ScheduleCalendarDTO.RecurFrequency,
                                                                      parameterDiscountsDTO.ScheduleCalendarDTO.RecurEndDate,
                                                                      parameterDiscountsDTO.ScheduleCalendarDTO.RecurType,
                                                                      parameterDiscountsDTO.ScheduleCalendarDTO.IsActive);
                    ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, scheduleCalendarDTO);
                    discountsDTO.ScheduleCalendarDTO = scheduleCalendarBL.ScheduleCalendarDTO;
                }
                ValidateDiscountsConstaints();
            }
            log.LogMethodExit();
        }



        public void Update(DiscountsDTO parameterDiscountsDTO)
        {
            log.LogMethodEntry(parameterDiscountsDTO);
            ChangeIsActive(parameterDiscountsDTO.IsActive);
            ChangeDiscountName(parameterDiscountsDTO.DiscountName);
            ChangeDiscountPercentage(parameterDiscountsDTO.DiscountPercentage);
            ChangeAutomaticApply(parameterDiscountsDTO.AutomaticApply);
            ChangeMinimumSaleAmount(parameterDiscountsDTO.MinimumSaleAmount);
            ChangeMinimumCredits(parameterDiscountsDTO.MinimumCredits);
            ChangeDisplayInPOS(parameterDiscountsDTO.DisplayInPOS);
            ChangeSortOrder(parameterDiscountsDTO.SortOrder);
            ChangeManagerApprovalRequired(parameterDiscountsDTO.ManagerApprovalRequired);
            ChangeInternetKey(parameterDiscountsDTO.InternetKey);
            ChangeDiscountType(parameterDiscountsDTO.DiscountType);
            ChangeCouponMandatory(parameterDiscountsDTO.CouponMandatory);
            ChangeDiscountAmount(parameterDiscountsDTO.DiscountAmount);
            ChangeRemarksMandatory(parameterDiscountsDTO.RemarksMandatory);
            ChangeVariableDiscounts(parameterDiscountsDTO.VariableDiscounts);
            ChangeScheduleId(parameterDiscountsDTO.ScheduleId);
            ChangeTransactionProfileId(parameterDiscountsDTO.TransactionProfileId);
            ChangeDiscountCriteriaLines(parameterDiscountsDTO.DiscountCriteriaLines);
            ChangeAllowMultipleApplication(parameterDiscountsDTO.AllowMultipleApplication);
            ChangeApplicationLimit(parameterDiscountsDTO.ApplicationLimit);
            UpdateDiscountedProducts(parameterDiscountsDTO.DiscountedProductsDTOList);
            UpdateDiscountPurchaseCriteria(parameterDiscountsDTO.DiscountPurchaseCriteriaDTOList);
            UpdateDiscountedGames(parameterDiscountsDTO.DiscountedGamesDTOList);
            UpdateScheduleCalendar(parameterDiscountsDTO.ScheduleCalendarDTO);
            ValidateDiscountsConstaints();
            log.LogMethodExit();
        }

        private void UpdateDiscountedProducts(List<DiscountedProductsDTO> discountedProductsDTOList)
        {
            log.LogMethodEntry(discountedProductsDTOList);
            if (discountedProductsDTOList == null ||
                discountedProductsDTOList.Any() == false)
            {
                log.LogMethodExit("discountedProductsDTOList is empty.");
                return;
            }
            Dictionary<int, DiscountedProductsDTO> discountedProductsDTODictionary = new Dictionary<int, DiscountedProductsDTO>();
            if (discountsDTO.DiscountedProductsDTOList != null &&
                discountsDTO.DiscountedProductsDTOList.Any())
            {
                foreach (var discountedProductsDTO in discountsDTO.DiscountedProductsDTOList)
                {
                    discountedProductsDTODictionary.Add(discountedProductsDTO.Id, discountedProductsDTO);
                }
            }
            foreach (var parameterDiscountedProductsDTO in discountedProductsDTOList)
            {
                if (discountedProductsDTODictionary.ContainsKey(parameterDiscountedProductsDTO.Id))
                {
                    DiscountedProductsBL discountedProducts = new DiscountedProductsBL(executionContext, discountedProductsDTODictionary[parameterDiscountedProductsDTO.Id], unitOfWork);
                    discountedProducts.Update(parameterDiscountedProductsDTO);
                }
                else if (parameterDiscountedProductsDTO.Id > -1)
                {
                    DiscountedProductsBL discountedProducts = new DiscountedProductsBL(executionContext, parameterDiscountedProductsDTO.Id, unitOfWork);
                    discountsDTO.DiscountedProductsDTOList.Add(discountedProducts.DiscountedProductsDTO);
                    discountedProducts.Update(parameterDiscountedProductsDTO);
                }
                else
                {
                    var discountedProductsDTO = new DiscountedProductsDTO(-1, -1,
                                                                        parameterDiscountedProductsDTO.ProductId,
                                                                        parameterDiscountedProductsDTO.Discounted,
                                                                        parameterDiscountedProductsDTO.IsActive,
                                                                        parameterDiscountedProductsDTO.CategoryId,
                                                                        parameterDiscountedProductsDTO.ProductGroupId,
                                                                        parameterDiscountedProductsDTO.Quantity,
                                                                        parameterDiscountedProductsDTO.DiscountPercentage,
                                                                        parameterDiscountedProductsDTO.DiscountAmount,
                                                                        parameterDiscountedProductsDTO.DiscountedPrice);
                    DiscountedProductsBL discountedProductsBL = new DiscountedProductsBL(executionContext, discountedProductsDTO, unitOfWork);
                    discountsDTO.DiscountedProductsDTOList.Add(discountedProductsBL.DiscountedProductsDTO);
                }
            }
            log.LogMethodExit();
        }

        private void UpdateDiscountPurchaseCriteria(List<DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTOList)
        {
            log.LogMethodEntry(discountPurchaseCriteriaDTOList);
            if (discountPurchaseCriteriaDTOList == null ||
                discountPurchaseCriteriaDTOList.Any() == false)
            {
                log.LogMethodExit("discountPurchaseCriteriaDTOList is empty.");
                return;
            }
            Dictionary<int, DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTODictionary = new Dictionary<int, DiscountPurchaseCriteriaDTO>();
            if (discountsDTO.DiscountPurchaseCriteriaDTOList != null &&
                discountsDTO.DiscountPurchaseCriteriaDTOList.Any())
            {
                foreach (var discountPurchaseCriteriaDTO in discountsDTO.DiscountPurchaseCriteriaDTOList)
                {
                    discountPurchaseCriteriaDTODictionary.Add(discountPurchaseCriteriaDTO.CriteriaId, discountPurchaseCriteriaDTO);
                }
            }
            foreach (var parameterDiscountPurchaseCriteriaDTO in discountPurchaseCriteriaDTOList)
            {
                if (discountPurchaseCriteriaDTODictionary.ContainsKey(parameterDiscountPurchaseCriteriaDTO.CriteriaId))
                {
                    DiscountPurchaseCriteriaBL discountPurchaseCriteria = new DiscountPurchaseCriteriaBL(executionContext, discountPurchaseCriteriaDTODictionary[parameterDiscountPurchaseCriteriaDTO.CriteriaId], unitOfWork);
                    discountPurchaseCriteria.Update(parameterDiscountPurchaseCriteriaDTO);
                }
                else if (parameterDiscountPurchaseCriteriaDTO.CriteriaId > -1)
                {
                    DiscountPurchaseCriteriaBL discountPurchaseCriteria = new DiscountPurchaseCriteriaBL(executionContext, parameterDiscountPurchaseCriteriaDTO.CriteriaId, unitOfWork);
                    discountsDTO.DiscountPurchaseCriteriaDTOList.Add(discountPurchaseCriteria.DiscountPurchaseCriteriaDTO);
                    discountPurchaseCriteria.Update(parameterDiscountPurchaseCriteriaDTO);
                }
                else
                {
                    var discountPurchaseCriteriaDTO = new DiscountPurchaseCriteriaDTO(-1, -1,
                                                                                          parameterDiscountPurchaseCriteriaDTO.ProductId,
                                                                                          parameterDiscountPurchaseCriteriaDTO.CategoryId,
                                                                                          parameterDiscountPurchaseCriteriaDTO.ProductGroupId,
                                                                                          parameterDiscountPurchaseCriteriaDTO.MinQuantity,
                                                                                          parameterDiscountPurchaseCriteriaDTO.IsActive);
                    DiscountPurchaseCriteriaBL discountPurchaseCriteriaBL = new DiscountPurchaseCriteriaBL(executionContext, discountPurchaseCriteriaDTO, unitOfWork);
                    discountsDTO.DiscountPurchaseCriteriaDTOList.Add(discountPurchaseCriteriaBL.DiscountPurchaseCriteriaDTO);
                }
            }
            log.LogMethodExit();
        }

        private void UpdateDiscountedGames(List<DiscountedGamesDTO> discountedGamesDTOList)
        {
            log.LogMethodEntry(discountedGamesDTOList);
            if (discountedGamesDTOList == null ||
                discountedGamesDTOList.Any() == false)
            {
                log.LogMethodExit("discountedGamesDTOList is empty.");
                return;
            }
            Dictionary<int, DiscountedGamesDTO> discountedGamesDTODictionary = new Dictionary<int, DiscountedGamesDTO>();
            if (discountsDTO.DiscountedGamesDTOList != null &&
                discountsDTO.DiscountedGamesDTOList.Any())
            {
                foreach (var discountedGamesDTO in discountsDTO.DiscountedGamesDTOList)
                {
                    discountedGamesDTODictionary.Add(discountedGamesDTO.Id, discountedGamesDTO);
                }
            }
            foreach (var parameterDiscountedGamesDTO in discountedGamesDTOList)
            {
                if (discountedGamesDTODictionary.ContainsKey(parameterDiscountedGamesDTO.Id))
                {
                    DiscountedGamesBL discountedGames = new DiscountedGamesBL(executionContext, discountedGamesDTODictionary[parameterDiscountedGamesDTO.Id], unitOfWork);
                    discountedGames.Update(parameterDiscountedGamesDTO);
                }
                else if (parameterDiscountedGamesDTO.Id > -1)
                {
                    DiscountedGamesBL discountedGames = new DiscountedGamesBL(executionContext, parameterDiscountedGamesDTO.Id, unitOfWork);
                    discountsDTO.DiscountedGamesDTOList.Add(discountedGames.DiscountedGamesDTO);
                    discountedGames.Update(parameterDiscountedGamesDTO);
                }
                else
                {
                    var discountedGamesDTO = new DiscountedGamesDTO(-1, -1,
                                                                        parameterDiscountedGamesDTO.GameId,
                                                                        parameterDiscountedGamesDTO.Discounted,
                                                                        parameterDiscountedGamesDTO.IsActive);
                    DiscountedGamesBL discountedGamesBL = new DiscountedGamesBL(executionContext, discountedGamesDTO, unitOfWork);
                    discountsDTO.DiscountedGamesDTOList.Add(discountedGamesBL.DiscountedGamesDTO);
                }
            }
            log.LogMethodExit();
        }

        private void UpdateScheduleCalendar(ScheduleCalendarDTO parameterScheduleCalendarDTO)
        {
            log.LogMethodEntry(parameterScheduleCalendarDTO);
            if(parameterScheduleCalendarDTO == null)
            {
                log.LogMethodExit("parameterScheduleCalendarDTO is empty");
                return;
            }
            if(discountsDTO.ScheduleCalendarDTO != null)
            {
                ScheduleCalendarBL discountedProducts = new ScheduleCalendarBL(executionContext, discountsDTO.ScheduleCalendarDTO);
                discountedProducts.Update(parameterScheduleCalendarDTO, unitOfWork.SQLTrx);
            }
            else if (parameterScheduleCalendarDTO.ScheduleId > -1)
            {
                ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, parameterScheduleCalendarDTO.ScheduleId, true, true, unitOfWork.SQLTrx);
                scheduleCalendarBL.Update(parameterScheduleCalendarDTO, unitOfWork.SQLTrx);
                discountsDTO.ScheduleCalendarDTO = scheduleCalendarBL.ScheduleCalendarDTO;
            }
            else
            {
                var scheduleCalendarDTO = new ScheduleCalendarDTO(-1,
                                                                  parameterScheduleCalendarDTO.ScheduleName,
                                                                  parameterScheduleCalendarDTO.ScheduleTime,
                                                                  parameterScheduleCalendarDTO.ScheduleEndDate,
                                                                  parameterScheduleCalendarDTO.RecurFlag,
                                                                  parameterScheduleCalendarDTO.RecurFrequency,
                                                                  parameterScheduleCalendarDTO.RecurEndDate,
                                                                  parameterScheduleCalendarDTO.RecurType,
                                                                  parameterScheduleCalendarDTO.IsActive);
                ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, scheduleCalendarDTO);
                discountsDTO.ScheduleCalendarDTO = scheduleCalendarBL.ScheduleCalendarDTO;
            }
            log.LogMethodExit();
        }

        public void ChangeDiscountName(string discountName)
        {
            log.LogMethodEntry(discountName);
            if (discountsDTO.DiscountName == discountName)
            {
                log.LogMethodExit(null, "No changes to discount name");
                return;
            }
            ValidateDiscountName(discountName);
            discountsDTO.DiscountName = discountName;
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (discountsDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to Discounts isActive");
                return;
            }
            ValidateIsActive(isActive);
            discountsDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeDiscountPercentage(double? discountPercentage)
        {
            log.LogMethodEntry(discountPercentage);
            if (discountsDTO.DiscountPercentage == discountPercentage)
            {
                log.LogMethodExit(null, "No changes to discount percentage");
                return;
            }
            ValidateDiscountPercentage(discountPercentage);
            discountsDTO.DiscountPercentage = discountPercentage;
            log.LogMethodExit();
        }

        public void ChangeMinimumSaleAmount(double? minimumSaleAmount)
        {
            log.LogMethodEntry(minimumSaleAmount);
            if (discountsDTO.MinimumSaleAmount == minimumSaleAmount)
            {
                log.LogMethodExit(null, "No changes to minimum sale amount");
                return;
            }
            ValidateMinimumSaleAmount(minimumSaleAmount);
            discountsDTO.MinimumSaleAmount = minimumSaleAmount;
            log.LogMethodExit();
        }

        public void ChangeMinimumCredits(double? minimumCredits)
        {
            log.LogMethodEntry(minimumCredits);
            if (discountsDTO.MinimumCredits == minimumCredits)
            {
                log.LogMethodExit(null, "No changes to minimum credits");
                return;
            }
            ValidateMinimumCredits(minimumCredits);
            discountsDTO.MinimumCredits = minimumCredits;
            log.LogMethodExit();
        }

        public void ChangeSortOrder(int sortOrder)
        {
            log.LogMethodEntry(sortOrder);
            if (discountsDTO.SortOrder == sortOrder)
            {
                log.LogMethodExit(null, "No changes to sort order");
                return;
            }
            ValidateSortOrder(sortOrder);
            discountsDTO.SortOrder = sortOrder;
            log.LogMethodExit();
        }

        public void ChangeDiscountType(string discountType)
        {
            log.LogMethodEntry(discountType);
            if (discountsDTO.DiscountType == discountType)
            {
                log.LogMethodExit(null, "No changes to discount type");
                return;
            }
            ValidateDiscountType(discountType);
            discountsDTO.DiscountType = discountType;
            log.LogMethodExit();
        }

        public void ChangeDiscountAmount(double? discountAmount)
        {
            log.LogMethodEntry(discountAmount);
            if (discountsDTO.DiscountAmount == discountAmount)
            {
                log.LogMethodExit(null, "No changes to discount amount");
                return;
            }
            ValidateDiscountAmount(discountAmount);
            discountsDTO.DiscountAmount = discountAmount;
            log.LogMethodExit();
        }

        public void ChangeApplicationLimit(int? applicationLimit)
        {
            log.LogMethodEntry(applicationLimit);
            if (discountsDTO.ApplicationLimit == applicationLimit)
            {
                log.LogMethodExit(null, "No changes to application limit");
                return;
            }
            ValidateApplicationLimit(applicationLimit);
            discountsDTO.ApplicationLimit = applicationLimit;
            log.LogMethodExit();
        }

        public void ChangeAutomaticApply(string automaticApply)
        {
            log.LogMethodEntry(automaticApply);
            if (discountsDTO.AutomaticApply == automaticApply)
            {
                log.LogMethodExit(null, "No changes to automatic apply");
                return;
            }
            discountsDTO.AutomaticApply = automaticApply;
            log.LogMethodExit();
        }

        public void ChangeDisplayInPOS(string displayInPOS)
        {
            log.LogMethodEntry(displayInPOS);
            if (discountsDTO.DisplayInPOS == displayInPOS)
            {
                log.LogMethodExit(null, "No changes to display in pos");
                return;
            }
            discountsDTO.DisplayInPOS = displayInPOS;
            log.LogMethodExit();
        }

        public void ChangeManagerApprovalRequired(string managerApprovalRequired)
        {
            log.LogMethodEntry(managerApprovalRequired);
            if (discountsDTO.ManagerApprovalRequired == managerApprovalRequired)
            {
                log.LogMethodExit(null, "No changes to manager approval required");
                return;
            }
            discountsDTO.ManagerApprovalRequired = managerApprovalRequired;
            log.LogMethodExit();
        }

        public void ChangeInternetKey(int? internetKey)
        {
            log.LogMethodEntry(internetKey);
            if (discountsDTO.InternetKey == internetKey)
            {
                log.LogMethodExit(null, "No changes to internet key");
                return;
            }
            discountsDTO.InternetKey = internetKey;
            log.LogMethodExit();
        }

        public void ChangeCouponMandatory(string couponMandatory)
        {
            log.LogMethodEntry(couponMandatory);
            if (discountsDTO.CouponMandatory == couponMandatory)
            {
                log.LogMethodExit(null, "No changes to coupon mandatory");
                return;
            }
            discountsDTO.CouponMandatory = couponMandatory;
            log.LogMethodExit();
        }

        public void ChangeRemarksMandatory(string remarksMandatory)
        {
            log.LogMethodEntry(remarksMandatory);
            if (discountsDTO.RemarksMandatory == remarksMandatory)
            {
                log.LogMethodExit(null, "No changes to remarks mandatory");
                return;
            }
            discountsDTO.RemarksMandatory = remarksMandatory;
            log.LogMethodExit();
        }

        public void ChangeVariableDiscounts(string variableDiscounts)
        {
            log.LogMethodEntry(variableDiscounts);
            if (discountsDTO.VariableDiscounts == variableDiscounts)
            {
                log.LogMethodExit(null, "No changes to variable discounts");
                return;
            }
            discountsDTO.VariableDiscounts = variableDiscounts;
            log.LogMethodExit();
        }

        public void ChangeAllowMultipleApplication(bool allowMultipleApplication)
        {
            log.LogMethodEntry(allowMultipleApplication);
            if (discountsDTO.AllowMultipleApplication == allowMultipleApplication)
            {
                log.LogMethodExit(null, "No changes to allow multiple application");
                return;
            }
            discountsDTO.AllowMultipleApplication = allowMultipleApplication;
            log.LogMethodExit();
        }

        public void ChangeDiscountCriteriaLines(bool discountCriteriaLines)
        {
            log.LogMethodEntry(discountCriteriaLines);
            if (discountsDTO.DiscountCriteriaLines == discountCriteriaLines)
            {
                log.LogMethodExit(null, "No changes to discount criteria lines");
                return;
            }
            discountsDTO.DiscountCriteriaLines = discountCriteriaLines;
            log.LogMethodExit();
        }

        public void ChangeScheduleId(int scheduleId)
        {
            log.LogMethodEntry(scheduleId);
            if (discountsDTO.ScheduleId == scheduleId)
            {
                log.LogMethodExit(null, "No changes to schedule id");
                return;
            }
            discountsDTO.ScheduleId = scheduleId;
            log.LogMethodExit();
        }

        public void ChangeTransactionProfileId(int transactionProfileId)
        {
            log.LogMethodEntry(transactionProfileId);
            if (discountsDTO.TransactionProfileId == transactionProfileId)
            {
                log.LogMethodExit(null, "No changes to transaction profile id");
                return;
            }
            discountsDTO.TransactionProfileId = transactionProfileId;
            log.LogMethodExit();
        }

        private void ValidateDiscountsConstaints()
        {
            log.LogMethodEntry();
            if (discountsDTO.AllowMultipleApplication &&
                discountsDTO.ApplicationLimit.HasValue &&
                discountsDTO.ApplicationLimit > 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Application Limit"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Please enter valid value for Application Limit.", "Discount", "ApplicationLimit", errorMessage);
            }
            if (discountsDTO.DiscountType != DiscountsBL.DISCOUNT_TYPE_TRANSACTION &&
                (discountsDTO.DiscountPercentage.HasValue == false || discountsDTO.DiscountPercentage.Value == 0))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Discount Percentage"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Please enter valid value for Discount Percentage.", "Discount", "DiscountPercentage", errorMessage);
            }

            if (discountsDTO.DiscountedProductsDTOList.Any())
            {
                var duplicateDiscountedProducts = discountsDTO.DiscountedProductsDTOList.Where(x => x.IsActive && x.ProductId > -1).GroupBy(x => x.ProductId).Where(x => x.Count() > 1);
                if (duplicateDiscountedProducts.Any())
                {
                    var duplicateDiscountedProductsDTO = duplicateDiscountedProducts.First().FirstOrDefault();
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4994, MessageContainerList.GetMessage(executionContext, "Discounted Product"), MessageContainerList.GetMessage(executionContext, "Product"), duplicateDiscountedProductsDTO.ProductId.ToString());
                    throw new ValidationException("Duplicate discounted products. Product with id:" + duplicateDiscountedProductsDTO.ProductId + " already assigned.", "DiscountedProducts", "ProductId", errorMessage);
                }

                duplicateDiscountedProducts = discountsDTO.DiscountedProductsDTOList.Where(x => x.IsActive && x.CategoryId > -1).GroupBy(x => x.CategoryId).Where(x => x.Count() > 1);
                if (duplicateDiscountedProducts.Any())
                {
                    var duplicateDiscountedProductsDTO = duplicateDiscountedProducts.First().FirstOrDefault();
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4994, MessageContainerList.GetMessage(executionContext, "Discounted Product"), MessageContainerList.GetMessage(executionContext, "Category"), duplicateDiscountedProductsDTO.CategoryId.ToString());
                    throw new ValidationException("Duplicate discounted products. Category with id:" + duplicateDiscountedProductsDTO.CategoryId + " already assigned.", "DiscountedProducts", "CategoryId", errorMessage);
                }

                duplicateDiscountedProducts = discountsDTO.DiscountedProductsDTOList.Where(x => x.IsActive && x.ProductGroupId > -1).GroupBy(x => x.ProductGroupId).Where(x => x.Count() > 1);
                if (duplicateDiscountedProducts.Any())
                {
                    var duplicateDiscountedProductsDTO = duplicateDiscountedProducts.First().FirstOrDefault();
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4994, MessageContainerList.GetMessage(executionContext, "Discounted Product"), MessageContainerList.GetMessage(executionContext, "Product Group"), duplicateDiscountedProductsDTO.ProductGroupId.ToString());
                    throw new ValidationException("Duplicate discounted products. Product group with id:" + duplicateDiscountedProductsDTO.ProductGroupId + " already assigned.", "DiscountedProducts", "ProductGroupId", errorMessage);
                }
            }

            if (discountsDTO.DiscountPurchaseCriteriaDTOList.Any())
            {
                var duplicateDiscountPurchaseCriteria = discountsDTO.DiscountPurchaseCriteriaDTOList.Where(x => x.IsActive && x.ProductId > -1).GroupBy(x => x.ProductId).Where(x => x.Count() > 1);
                if (duplicateDiscountPurchaseCriteria.Any())
                {
                    var duplicateDiscountPurchaseCriteriaDTO = duplicateDiscountPurchaseCriteria.First().FirstOrDefault();
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4994, MessageContainerList.GetMessage(executionContext, "Discount Purchase Criteria"), MessageContainerList.GetMessage(executionContext, "Product"), duplicateDiscountPurchaseCriteriaDTO.ProductId.ToString());
                    throw new ValidationException("Duplicate discount purchase criteria. Product with id:" + duplicateDiscountPurchaseCriteriaDTO.ProductId + " already assigned.", "DiscountPurchaseCriteria", "ProductId", errorMessage);
                }

                duplicateDiscountPurchaseCriteria = discountsDTO.DiscountPurchaseCriteriaDTOList.Where(x => x.IsActive && x.CategoryId > -1).GroupBy(x => x.CategoryId).Where(x => x.Count() > 1);
                if (duplicateDiscountPurchaseCriteria.Any())
                {
                    var duplicateDiscountPurchaseCriteriaDTO = duplicateDiscountPurchaseCriteria.First().FirstOrDefault();
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4994, MessageContainerList.GetMessage(executionContext, "Discount Purchase Criteria"), MessageContainerList.GetMessage(executionContext, "Category"), duplicateDiscountPurchaseCriteriaDTO.CategoryId.ToString());
                    throw new ValidationException("Duplicate discount purchase criteria. Category with id:" + duplicateDiscountPurchaseCriteriaDTO.CategoryId + " already assigned.", "DiscountPurchaseCriteria", "CategoryId", errorMessage);
                }

                duplicateDiscountPurchaseCriteria = discountsDTO.DiscountPurchaseCriteriaDTOList.Where(x => x.IsActive && x.ProductGroupId > -1).GroupBy(x => x.ProductGroupId).Where(x => x.Count() > 1);
                if (duplicateDiscountPurchaseCriteria.Any())
                {
                    var duplicateDiscountPurchaseCriteriaDTO = duplicateDiscountPurchaseCriteria.First().FirstOrDefault();
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4994, MessageContainerList.GetMessage(executionContext, "Discount Purchase Criteria"), MessageContainerList.GetMessage(executionContext, "Product Group"), duplicateDiscountPurchaseCriteriaDTO.ProductGroupId.ToString());
                    throw new ValidationException("Duplicate discount purchase criteria. Product group with id:" + duplicateDiscountPurchaseCriteriaDTO.ProductGroupId + " already assigned.", "DiscountPurchaseCriteria", "ProductGroupId", errorMessage);
                }
            }

            if (discountsDTO.DiscountedGamesDTOList.Any())
            {
                var duplicateDiscountedGames = discountsDTO.DiscountedGamesDTOList.Where(x => x.IsActive && x.GameId > -1).GroupBy(x => x.GameId).Where(x => x.Count() > 1);
                if (duplicateDiscountedGames.Any())
                {
                    var duplicateDiscountedGamesDTO = duplicateDiscountedGames.First().FirstOrDefault();
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4994, MessageContainerList.GetMessage(executionContext, "Discounted Game"), MessageContainerList.GetMessage(executionContext, "Game"), duplicateDiscountedGamesDTO.GameId.ToString());
                    throw new ValidationException("Duplicate discounted games. Game with id:" + duplicateDiscountedGamesDTO.GameId + " already assigned.", "DiscountedGames", "GameId", errorMessage);
                }
            }

            if (discountsDTO.DiscountedProductsDTOList != null && 
                discountsDTO.DiscountedProductsDTOList.Any(x => x.IsActive))
            {
                double discountedProductsAmount = 0;
                bool discountedPriceExist = false;
                double discountAmount = (discountsDTO.DiscountAmount == null ? 0 : (double)discountsDTO.DiscountAmount);
                double discountPercentage = (discountsDTO.DiscountPercentage == null ? 0 : (double)discountsDTO.DiscountPercentage);
                double maxProductDiscountPercentage = 0;
                foreach (var discountedProductsDTO in discountsDTO.DiscountedProductsDTOList)
                {
                    if (discountedProductsDTO.IsActive &&
                        discountedProductsDTO.Discounted == "Y")
                    {
                        if (discountedProductsDTO.DiscountAmount != null &&
                            discountedProductsDTO.DiscountAmount > 0)
                        {
                            discountedProductsAmount += (double)discountedProductsDTO.DiscountAmount;
                        }
                        if (discountedProductsDTO.DiscountPercentage != null &&
                           discountedProductsDTO.DiscountPercentage > maxProductDiscountPercentage)
                        {
                            maxProductDiscountPercentage = (double)discountedProductsDTO.DiscountPercentage;
                        }

                        if (discountedProductsDTO.DiscountedPrice.HasValue &&
                            discountedProductsDTO.DiscountedPrice.Value > 0)
                        {
                            discountedPriceExist = true;
                        }
                    }
                }

                if (discountPercentage > 0 && maxProductDiscountPercentage > discountPercentage)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1224);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Individual discounted products percentage can't be greater than the discount percentage.", "Discount", "DiscountPercentage", errorMessage);
                }
                if ((discountPercentage > 0 || discountAmount > 0) && discountedPriceExist)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2616);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Discounted price is not valid for the discount.", "Discount", "DiscountPercentage", errorMessage);
                }
                if (discountsDTO.VariableDiscounts != "Y" &&
                    discountAmount == 0 &&
                    discountPercentage == 0 &&
                    discountedProductsAmount == 0 &&
                    discountedPriceExist == false &&
                    discountsDTO.DiscountedProductsDTOList != null &&
                    discountsDTO.DiscountedProductsDTOList.Any(x => x.IsActive))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Discount Percentage"));
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Please enter valid value for Discount Percentage.", "Discount", "DiscountPercentage", errorMessage);
                }

                if (discountsDTO.VariableDiscounts == "Y" &&
                    discountedProductsAmount > 0)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1223);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Total discounted products amount and discount amount should be equal.", "Discount", "DiscountAmount", errorMessage);
                }
                if (discountsDTO.VariableDiscounts == "Y" &&
                    discountedPriceExist)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2616);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Discouted price is not valid for the discount.", "Discount", "DiscountedPrice", errorMessage);
                }
                if (discountsDTO.VariableDiscounts == "Y" &&
                    maxProductDiscountPercentage > 0)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1224);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Individual discounted products percentage can't be greater than the discount percentage.", "Discount", "DiscountPercentage", errorMessage);
                }
                if (discountedProductsAmount != 0 && discountedProductsAmount != discountAmount &&
                      (discountAmount > 0 || discountPercentage > 0))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1223);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Total discounted products amount and discount amount should be equal.", "Discount", "DiscountAmount", errorMessage);
                }
            }
            log.LogMethodExit();
        }

        private void ValidateIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if ((discountsDTO != null && discountsDTO.DiscountId > -1) && isActive == false)
            {
                DiscountsDataHandler discountsDataHandler = new DiscountsDataHandler(executionContext, unitOfWork);
                int referenceCount = discountsDataHandler.GetDiscountsReferenceCount(discountsDTO.DiscountId);
                if (referenceCount > 0)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1869);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Unable to delete this record. Please check the reference record first.", "Discount", "IsActive", errorMessage);
                }
            }
            log.LogMethodExit();
        }

        

        private void ValidateDiscountName(string name)
        {
            log.LogMethodEntry(name);
            if (string.IsNullOrWhiteSpace(name))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Name"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("DiscountName is empty.", "Discount", "DiscountName", errorMessage);
            }
            if (name.Length > 50)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Name"), 50);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("DiscountName greater than 50 characters.", "Discount", "DiscountName", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateDiscountPercentage(double? discountPercentage)
        {
            log.LogMethodEntry(discountPercentage);
            if (discountPercentage.HasValue && discountPercentage < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Discount Percentage"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Discount Percentage can't be negative.", "Discount", "DiscountPercentage", errorMessage);
            }
            if (discountPercentage.HasValue && discountPercentage > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1876);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Discount Percentage should not exceed 100%.", "Discount", "DiscountPercentage", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateMinimumSaleAmount(double? minimumSaleAmount)
        {
            log.LogMethodEntry(minimumSaleAmount);
            
            if (minimumSaleAmount.HasValue && minimumSaleAmount < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Minimum Sale Amount"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Minimum Sale Amount can't be negative.", "Discount", "MinimumSaleAmount", errorMessage);
            }
            log.LogMethodExit();
        }

        

        private void ValidateMinimumCredits(double? minimumCredits)
        {
            log.LogMethodEntry(minimumCredits);
            if (minimumCredits.HasValue && minimumCredits < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Minimum Credits"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Minimum Credits can't be negative.", "Discount", "MinimumCredits", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateApplicationLimit(int? applicationLimit)
        {
            log.LogMethodEntry(applicationLimit);
            if (applicationLimit.HasValue && applicationLimit < 0 )
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Application Limit"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Application Limit can't be negative.", "Discount", "ApplicationLimit", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateSortOrder(int sortOrder)
        {
            log.LogMethodEntry(sortOrder);
            if (sortOrder < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Sort Order"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Sort Order can't be negative.", "Discount", "SortOrder", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateDiscountType(string discountType)
        {
            log.LogMethodEntry(discountType);
            if (string.IsNullOrWhiteSpace(discountType) ||
                (discountType != DISCOUNT_TYPE_GAMEPLAY &&
                 discountType != DISCOUNT_TYPE_LOYALTY &&
                 discountType != DISCOUNT_TYPE_TRANSACTION))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Discount Type"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Invalid discountType.", "Discount", "DiscountType", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateDiscountAmount(double? discountAmount)
        {
            log.LogMethodEntry(discountAmount);
            if (discountAmount.HasValue && discountAmount < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5048, MessageContainerList.GetMessage(executionContext, "Discount Amount"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Discount Amount can't be negative.", "Discount", "DiscountAmount", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for Discounts object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords)
        {
            log.LogMethodEntry(activeChildRecords);
            DiscountsBuilderBL discountsBuilderBL = new DiscountsBuilderBL(executionContext, unitOfWork);
            discountsBuilderBL.Build(discountsDTO, activeChildRecords, false);
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves theDiscounts
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save()
        {
            log.LogMethodEntry();
            if(discountsDTO.ScheduleCalendarDTO != null && discountsDTO.ScheduleCalendarDTO.IsChanged)
            {
                ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, discountsDTO.ScheduleCalendarDTO);
                scheduleCalendarBL.Save(unitOfWork.SQLTrx);
                discountsDTO.ScheduleCalendarDTO = scheduleCalendarBL.ScheduleCalendarDTO;
            }
            if(discountsDTO.ScheduleCalendarDTO != null &&
                discountsDTO.ScheduleId != discountsDTO.ScheduleCalendarDTO.ScheduleId)
            {
                discountsDTO.ScheduleId = discountsDTO.ScheduleCalendarDTO.ScheduleId;
            }
            DiscountsDataHandler discountsDataHandler = new DiscountsDataHandler(executionContext, unitOfWork);
            if (discountsDTO.IsChanged)
            {
                log.LogVariableState("discountsDTO", discountsDTO);
                discountsDTO = SiteContainerList.ToSiteDateTime(executionContext, discountsDTO);
                discountsDTO = discountsDataHandler.Save(discountsDTO);
                discountsDTO = SiteContainerList.FromSiteDateTime(executionContext, discountsDTO);
                discountsDTO.AcceptChanges();
            }
            // Will Save the Child DiscountsDTO
            log.LogVariableState("discountsDTO.DiscountedProductsDTOList Value :", discountsDTO.DiscountedProductsDTOList);
            if (discountsDTO.DiscountedProductsDTOList != null && discountsDTO.DiscountedProductsDTOList.Any())
            {
                List<DiscountedProductsDTO> updatedDiscountedProductsDTOList = new List<DiscountedProductsDTO>();
                foreach (DiscountedProductsDTO discountedProductsDTO in discountsDTO.DiscountedProductsDTOList)
                {
                    if (discountedProductsDTO.DiscountId != discountsDTO.DiscountId)
                    {
                        discountedProductsDTO.DiscountId = discountsDTO.DiscountId;
                    }
                    if (discountedProductsDTO.IsChanged)
                    {
                        updatedDiscountedProductsDTOList.Add(discountedProductsDTO);
                    }
                }
                log.LogVariableState("updatedDiscountedProductsDTOList :", updatedDiscountedProductsDTOList);
                DiscountedProductsListBL discountedProductsListBL = new DiscountedProductsListBL(executionContext, unitOfWork);
                discountedProductsListBL.Save(updatedDiscountedProductsDTOList);
            }
            log.LogVariableState("discountsDTO.DiscountPurchaseCriteriaDTOList Value :", discountsDTO.DiscountPurchaseCriteriaDTOList);
            if (discountsDTO.DiscountPurchaseCriteriaDTOList != null && discountsDTO.DiscountPurchaseCriteriaDTOList.Any())
            {
                List<DiscountPurchaseCriteriaDTO> updatedDiscountPurchaseCriteriaDTOList = new List<DiscountPurchaseCriteriaDTO>();
                foreach (DiscountPurchaseCriteriaDTO discountPurchaseCriteriaDTO in discountsDTO.DiscountPurchaseCriteriaDTOList)
                {
                    if (discountPurchaseCriteriaDTO.DiscountId != discountsDTO.DiscountId)
                    {
                        discountPurchaseCriteriaDTO.DiscountId = discountsDTO.DiscountId;
                    }
                    if (discountPurchaseCriteriaDTO.IsChanged)
                    {
                        updatedDiscountPurchaseCriteriaDTOList.Add(discountPurchaseCriteriaDTO);
                    }
                }
                log.LogVariableState("updatedDiscountPurchaseCriteriaDTOList :", updatedDiscountPurchaseCriteriaDTOList);
                DiscountPurchaseCriteriaListBL discountPurchaseCriteriaListBL = new DiscountPurchaseCriteriaListBL(executionContext, unitOfWork);
                discountPurchaseCriteriaListBL.Save(updatedDiscountPurchaseCriteriaDTOList);
            }
            log.LogVariableState("discountsDTO.DiscountedGamesDTOList Value :", discountsDTO.DiscountedGamesDTOList);
            if (discountsDTO.DiscountedGamesDTOList != null && discountsDTO.DiscountedGamesDTOList.Any())
            {
                List<DiscountedGamesDTO> updatedDiscountedGamesDTOList = new List<DiscountedGamesDTO>();
                foreach (DiscountedGamesDTO discountedGamesDTO in discountsDTO.DiscountedGamesDTOList)
                {
                    if (discountedGamesDTO.DiscountId != discountsDTO.DiscountId)
                    {
                        discountedGamesDTO.DiscountId = discountsDTO.DiscountId;
                    }
                    if (discountedGamesDTO.IsChanged)
                    {
                        updatedDiscountedGamesDTOList.Add(discountedGamesDTO);
                    }
                }
                log.LogVariableState("updatedDiscountedGamesDTOList :", updatedDiscountedGamesDTOList);
                DiscountedGamesListBL discountedGamesListBL = new DiscountedGamesListBL(executionContext, unitOfWork);
                discountedGamesListBL.Save(updatedDiscountedGamesDTOList);
            }
            log.LogMethodExit();
        }

        public void InactivateDiscountedGames()
        {
            log.LogMethodEntry();
            if (discountsDTO.DiscountedGamesDTOList == null ||
               discountsDTO.DiscountedGamesDTOList.Any() == false)
            {
                log.LogMethodExit("DiscountedGamesDTOList is empty");
                return;
            }
            foreach (var discountedGamesDTO in discountsDTO.DiscountedGamesDTOList)
            {
                DiscountedGamesBL discountedGamesBL = new DiscountedGamesBL(executionContext, discountedGamesDTO, unitOfWork);
                if (discountedGamesBL.IsActive == false)
                {
                    continue;
                }
                discountedGamesBL.ChangeIsActive(false);
            }
            ValidateDiscountsConstaints();
            Save();
            log.LogMethodExit();
        }

        public void InactivateDiscountedProducts()
        {
            log.LogMethodEntry();
            if (discountsDTO.DiscountedProductsDTOList == null ||
               discountsDTO.DiscountedProductsDTOList.Any() == false)
            {
                log.LogMethodExit("DiscountedProductsDTOList is empty");
                return;
            }
            foreach (var discountedProductsDTO in discountsDTO.DiscountedProductsDTOList)
            {
                DiscountedProductsBL discountedProductsBL = new DiscountedProductsBL(executionContext, discountedProductsDTO, unitOfWork);
                if (discountedProductsBL.IsActive == false)
                {
                    continue;
                }
                discountedProductsBL.ChangeIsActive(false);
            }
            ValidateDiscountsConstaints();
            Save();
            log.LogMethodExit();
        }

        public void ClearDiscountedProducts()
        {
            log.LogMethodEntry();
            if (discountsDTO.DiscountedProductsDTOList == null ||
               discountsDTO.DiscountedProductsDTOList.Any() == false)
            {
                log.LogMethodExit("DiscountedProductsDTOList is empty");
                return;
            }
            foreach (var discountedProductsDTO in discountsDTO.DiscountedProductsDTOList)
            {
                DiscountedProductsBL discountedProductsBL = new DiscountedProductsBL(executionContext, discountedProductsDTO, unitOfWork);
                if (discountedProductsBL.Discounted == "N")
                {
                    continue;
                }
                discountedProductsBL.ChangeDiscounted("N");
            }
            ValidateDiscountsConstaints();
            Save();
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DiscountsDTO DiscountsDTO
        {
            get
            {
                DiscountsDTO result = new DiscountsDTO(discountsDTO);
                return result;
            }
        }
    }

    /// <summary>
    /// Manages the list of Discounts
    /// </summary>
    public class DiscountsListBL
    {
        private readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DiscountsListBL()
        {
            log.LogMethodEntry();
            unitOfWork = new UnitOfWork();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of DiscountsListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public DiscountsListBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method should be used to Save and Update the Discounts.
        /// </summary>
        public List<DiscountsDTO> Save(List<DiscountsDTO> discountsDTOList)
        {
            log.LogMethodEntry();
            List<DiscountsDTO> savedDiscountsDTOList = new List<DiscountsDTO>();
            if (discountsDTOList == null || discountsDTOList.Any() == false)
            {
                log.LogMethodExit(savedDiscountsDTOList);
                return savedDiscountsDTOList;
            }
            foreach (DiscountsDTO discountsDTO in discountsDTOList)
            {
                DiscountsBL discountsBL = new DiscountsBL(executionContext, discountsDTO, unitOfWork);
                discountsBL.Save();
                savedDiscountsDTOList.Add(discountsBL.DiscountsDTO);
            }
            log.LogMethodExit(savedDiscountsDTOList);
            return savedDiscountsDTOList;
        }

        /// <summary>
        /// Returns the discounts DTO list count matching the search criteria
        /// </summary>
        public int GetTransactionDTOListCount(SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            DiscountsDataHandler discountsDataHandler = new DiscountsDataHandler(executionContext, unitOfWork);
            int result = discountsDataHandler.GetDiscountsDTOListCount(searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the Discounts list
        /// </summary>
        public List<DiscountsDTO> GetDiscountsDTOList(SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters,
                                                      bool loadChildRecords = false,
                                                      bool loadActiveChildRecords = false,
                                                      bool onlyDiscountedChildRecord = false,
                                                      int pageNumber = 0,
                                                      int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            DiscountsDataHandler discountsDataHandler = new DiscountsDataHandler(executionContext, unitOfWork);
            List<DiscountsDTO> discountsDTOsList = discountsDataHandler.GetDiscountsDTOList(searchParameters, pageNumber, pageSize);
            if (discountsDTOsList != null && discountsDTOsList.Any() && loadChildRecords)
            {
                DiscountsBuilderBL discountsBuilderBL = new DiscountsBuilderBL(executionContext, unitOfWork);
                discountsBuilderBL.Build(discountsDTOsList, loadActiveChildRecords, onlyDiscountedChildRecord);
            }
            log.LogMethodExit(discountsDTOsList);
            return discountsDTOsList;
        }

        /// <summary>
        /// Returns the Discounts list
        /// </summary>
        public List<DiscountsDTO> GetDiscountsDTOList(List<string> discountGuidList,
                                                      bool loadChildRecords = false,
                                                      bool loadActiveChildRecords = false)
        {
            log.LogMethodEntry(discountGuidList, loadChildRecords, loadActiveChildRecords);
            DiscountsDataHandler discountsDataHandler = new DiscountsDataHandler(executionContext, unitOfWork);
            List<DiscountsDTO> discountsDTOsList = discountsDataHandler.GetDiscountsDTOList(discountGuidList);
            if (discountsDTOsList != null && discountsDTOsList.Any() && loadChildRecords)
            {
                DiscountsBuilderBL discountsBuilderBL = new DiscountsBuilderBL(executionContext, unitOfWork);
                discountsBuilderBL.Build(discountsDTOsList, loadActiveChildRecords, false);
            }
            log.LogMethodExit(discountsDTOsList);
            return discountsDTOsList;
        }

        /// <summary>
        /// Returns the Discounts module last update time list
        /// </summary>
        public DateTime? GetDiscountsModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DiscountsDataHandler discountsDataHandler = new DiscountsDataHandler(executionContext, unitOfWork);
            DateTime? result = discountsDataHandler.GetDiscountsModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

    }


    /// <summary>
    /// Builds the complex discounts entity structure
    /// </summary>
    internal class DiscountsBuilderBL
    {
        private readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;
        private Dictionary<int, DiscountsDTO> discountsDTODictionary = new Dictionary<int, DiscountsDTO>();
        private Dictionary<int, DiscountsDTO> scheduleIdDiscountsDTODictionary = new Dictionary<int, DiscountsDTO>();
        private HashSet<string> discountTypeSet = new HashSet<string>();
        private List<DiscountsDTO> discountsDTOList;
        private List<int> scheduleIdList;
        private List<int> discountIdList;
        private bool activeChildRecords;
        private bool onlyDiscountedChildRecord;
        public DiscountsBuilderBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        public void Build(DiscountsDTO discountsDTO, bool activeChildRecords, bool onlyDiscountedChildRecord)
        {
            log.LogMethodEntry(discountsDTO, activeChildRecords, onlyDiscountedChildRecord);
            Build(new List<DiscountsDTO>() { discountsDTO }, activeChildRecords, onlyDiscountedChildRecord);
            log.LogMethodExit();
        }



        /// <summary>
        /// Builds the complex discount entity
        /// </summary>
        /// <param name="discountsDTOList">discounts dto list</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="onlyDiscountedChildRecord">whether to load only active child records</param>
        public void Build(List<DiscountsDTO> discountsDTOList, bool activeChildRecords, bool onlyDiscountedChildRecord)
        {
            log.LogMethodEntry(discountsDTOList, activeChildRecords, onlyDiscountedChildRecord);
            if (discountsDTOList != null && discountsDTOList.Any() == false)
            {
                log.LogMethodExit("Empty list");
                return;
            }
            this.activeChildRecords = activeChildRecords;
            this.onlyDiscountedChildRecord = onlyDiscountedChildRecord;
            this.discountsDTOList = discountsDTOList;
            CreateDictionaries();
            BuildScheduleCalendars();
            BuildDiscountPurchaseCriteria();
            BuildDiscountedProducts();
            BuildDiscountedGames();
            log.LogMethodExit();
        }

        private void BuildDiscountedGames()
        {
            log.LogMethodEntry();
            if (discountTypeSet.Contains(DiscountsBL.DISCOUNT_TYPE_GAMEPLAY) == false &&
                discountTypeSet.Contains(DiscountsBL.DISCOUNT_TYPE_LOYALTY) == false)
            {
                log.LogMethodExit("discountTypeSet doesn't have either DISCOUNT_TYPE_GAMEPLAY or DISCOUNT_TYPE_LOYALTY");
                return;
            }
            DiscountedGamesListBL discountedGamesListBL = new DiscountedGamesListBL(executionContext, unitOfWork);
            List<DiscountedGamesDTO> discountedGamesDTOList = discountedGamesListBL.GetDiscountedGamesDTOListOfDiscounts(discountIdList, activeChildRecords, onlyDiscountedChildRecord);
            if (discountedGamesDTOList == null || discountedGamesDTOList.Any() == false)
            {
                log.LogMethodExit("discountedGamesDTOList is empty");
                return;
            }
            foreach (var discountedGamesDTO in discountedGamesDTOList)
            {
                if (discountsDTODictionary.ContainsKey(discountedGamesDTO.DiscountId))
                {
                    if (discountsDTODictionary[discountedGamesDTO.DiscountId].DiscountedGamesDTOList == null)
                    {
                        discountsDTODictionary[discountedGamesDTO.DiscountId].DiscountedGamesDTOList = new List<DiscountedGamesDTO>();
                    }
                    discountsDTODictionary[discountedGamesDTO.DiscountId].DiscountedGamesDTOList.Add(discountedGamesDTO);
                }
            }
            log.LogMethodExit();
        }

        private void BuildDiscountedProducts()
        {
            log.LogMethodEntry();
            if (discountTypeSet.Contains(DiscountsBL.DISCOUNT_TYPE_TRANSACTION) == false)
            {
                log.LogMethodExit("discountTypeSet doesn't have DISCOUNT_TYPE_TRANSACTION");
                return;
            }
            DiscountedProductsListBL discountedProductsListBL = new DiscountedProductsListBL(executionContext, unitOfWork);
            List<DiscountedProductsDTO> discountedProductsDTOList = discountedProductsListBL.GetDiscountedProductsDTOListOfDiscounts(discountIdList, activeChildRecords, onlyDiscountedChildRecord);
            if (discountedProductsDTOList == null || discountedProductsDTOList.Any() == false)
            {
                log.LogMethodExit("discountedProductsDTOList is empty");
                return;
            }
            foreach (var discountedProductsDTO in discountedProductsDTOList)
            {
                if (discountsDTODictionary.ContainsKey(discountedProductsDTO.DiscountId))
                {
                    if (discountsDTODictionary[discountedProductsDTO.DiscountId].DiscountedProductsDTOList == null)
                    {
                        discountsDTODictionary[discountedProductsDTO.DiscountId].DiscountedProductsDTOList = new List<DiscountedProductsDTO>();
                    }
                    discountsDTODictionary[discountedProductsDTO.DiscountId].DiscountedProductsDTOList.Add(discountedProductsDTO);
                }
            }
            log.LogMethodExit();
        }

        private void BuildDiscountPurchaseCriteria()
        {
            log.LogMethodEntry();
            if (discountTypeSet.Contains(DiscountsBL.DISCOUNT_TYPE_TRANSACTION) == false)
            {
                log.LogMethodExit("discountTypeSet doesn't have DISCOUNT_TYPE_TRANSACTION");
                return;
            }
            DiscountPurchaseCriteriaListBL discountPurchaseCriteriaListBL = new DiscountPurchaseCriteriaListBL(executionContext, unitOfWork);
            List<DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTOList = discountPurchaseCriteriaListBL.GetDiscountPurchaseCriteriaDTOListOfDiscounts(discountIdList, activeChildRecords);
            if (discountPurchaseCriteriaDTOList == null || discountPurchaseCriteriaDTOList.Any() == false)
            {
                log.LogMethodExit("discountedProductsDTOList is empty");
                return;
            }
            foreach (var discountPurchaseCriteriaDTO in discountPurchaseCriteriaDTOList)
            {
                if (discountsDTODictionary.ContainsKey(discountPurchaseCriteriaDTO.DiscountId))
                {
                    if (discountsDTODictionary[discountPurchaseCriteriaDTO.DiscountId].DiscountPurchaseCriteriaDTOList == null)
                    {
                        discountsDTODictionary[discountPurchaseCriteriaDTO.DiscountId].DiscountPurchaseCriteriaDTOList = new List<DiscountPurchaseCriteriaDTO>();
                    }
                    discountsDTODictionary[discountPurchaseCriteriaDTO.DiscountId].DiscountPurchaseCriteriaDTOList.Add(discountPurchaseCriteriaDTO);
                }
            }
            log.LogMethodExit();
        }
        private void BuildScheduleCalendars()
        {
            log.LogMethodEntry();
            if (scheduleIdList.Any() == false)
            {
                log.LogMethodExit("scheduleIdList is empty");
                return;
            }
            ScheduleCalendarListBL scheduleCalendarListBL = new ScheduleCalendarListBL(executionContext);
            List<ScheduleCalendarDTO> scheduleCalendarDTOList = scheduleCalendarListBL.GetScheduleDTOListOfSchedules(scheduleIdList, false, unitOfWork.SQLTrx);
            if (scheduleCalendarDTOList == null || scheduleCalendarDTOList.Any() == false)
            {
                log.LogMethodExit("scheduleCalendarDTOList is empty");
                return;
            }
            foreach (var scheduleCalendarDTO in scheduleCalendarDTOList)
            {
                if (scheduleIdDiscountsDTODictionary.ContainsKey(scheduleCalendarDTO.ScheduleId) == false)
                {
                    continue;
                }
                scheduleIdDiscountsDTODictionary[scheduleCalendarDTO.ScheduleId].ScheduleCalendarDTO = scheduleCalendarDTO;
            }
            log.LogMethodExit();
        }

        private void CreateDictionaries()
        {
            log.LogMethodEntry();
            for (int i = 0; i < discountsDTOList.Count; i++)
            {
                if (discountsDTODictionary.ContainsKey(discountsDTOList[i].DiscountId))
                {
                    continue;
                }
                discountTypeSet.Add(discountsDTOList[i].DiscountType);
                discountsDTODictionary.Add(discountsDTOList[i].DiscountId, discountsDTOList[i]);
                if (discountsDTOList[i].ScheduleId == -1 || scheduleIdDiscountsDTODictionary.ContainsKey(discountsDTOList[i].ScheduleId))
                {
                    continue;
                }
                scheduleIdDiscountsDTODictionary.Add(discountsDTOList[i].ScheduleId, discountsDTOList[i]);
            }
            scheduleIdList = scheduleIdDiscountsDTODictionary.Keys.ToList();
            discountIdList = discountsDTODictionary.Keys.ToList();
            log.LogMethodExit();
        }

    }

    /// <summary>
    /// Represents variable discounts error that occur during application execution. 
    /// </summary>
    public class VariableDiscountException : Exception
    {
        /// <summary>
        /// Default constructor of VariableDiscountException.
        /// </summary>
        public VariableDiscountException()
        {
        }

        /// <summary>
        /// Initializes a new instance of VariableDiscountException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public VariableDiscountException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of VariableDiscountException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public VariableDiscountException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Represents remarks mandatory error that occur during application execution. 
    /// </summary>
    public class RemarksMandatoryException : Exception
    {
        /// <summary>
        /// Default constructor of RemarksMandatoryException.
        /// </summary>
        public RemarksMandatoryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of RemarksMandatoryException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public RemarksMandatoryException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of RemarksMandatoryException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public RemarksMandatoryException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Represents manager approval mandatory error that occur during application execution. 
    /// </summary>
    public class ApprovalMandatoryException : Exception
    {
        /// <summary>
        /// Default constructor of ApprovalMandatoryException.
        /// </summary>
        public ApprovalMandatoryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of ApprovalMandatoryException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public ApprovalMandatoryException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of ApprovalMandatoryException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public ApprovalMandatoryException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
