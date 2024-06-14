/********************************************************************************************
 * Project Name - DiscountsContainer
 * Description  - DiscountsContainer class to get the data from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      12-Apr-2021      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// Class holds the Discount Values.
    /// </summary>
    public class DiscountContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<int, Dictionary<int, DiscountedGamesContainerDTO>> discountIdDiscountedGamesDictionary = new Dictionary<int, Dictionary<int, DiscountedGamesContainerDTO>>();
        
        private Dictionary<int, Dictionary<int, HashSet<int>>> discountIdCriteriaIdProductIdListDictionary = new Dictionary<int, Dictionary<int, HashSet<int>>>();
        private Dictionary<int, Dictionary<int, HashSet<int>>> discountIdDiscountedProductIdProductIdListDictionary = new Dictionary<int, Dictionary<int, HashSet<int>>>();
        private Dictionary<int, HashSet<int>> discountIdCriteriaProductIdListDictionary = new Dictionary<int, HashSet<int>>();
        private Dictionary<int, HashSet<int>> discountIdDiscountedProductIdListDictionary = new Dictionary<int, HashSet<int>>();
        private Dictionary<int, Dictionary<int, DiscountedProductsContainerDTO>> discountIdDiscountedProductIdDiscountedProductsContainerDTODictionary = new Dictionary<int, Dictionary<int, DiscountedProductsContainerDTO>>();

        private Cache<DateTimeRange, DiscountAvailabilityContainer> discountAvailabilityContainerCache = new Cache<DateTimeRange, DiscountAvailabilityContainer>();
        private readonly Dictionary<int, DiscountContainerDTO> discountIdDiscountContainerDTODictionary = new Dictionary<int, DiscountContainerDTO>();
        private List<DiscountContainerDTO> autoApplicableDiscountList = new List<DiscountContainerDTO>();
        private List<DiscountContainerDTO> transactionDiscountList = new List<DiscountContainerDTO>();
        private List<DiscountContainerDTO> loyaltyGameDiscountList = new List<DiscountContainerDTO>();
        private List<DiscountContainerDTO> manualDiscountList = new List<DiscountContainerDTO>();
        private readonly DiscountContainerDTOCollection discountContainerDTOCollection;
        private readonly DateTime? discountModuleLastUpdateTime;
        private readonly int siteId;
        private readonly List<DiscountsDTO> discountsDTOList;

        public DiscountContainer(int siteId) : this(siteId, GetDiscountDTOList(siteId), GetDiscountModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public DiscountContainer(int siteId, List<DiscountsDTO> discountsDTOList, DateTime? discountModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.discountsDTOList = discountsDTOList;
            this.discountModuleLastUpdateTime = discountModuleLastUpdateTime;
            List<DiscountContainerDTO> discountContainerDTOList = new List<DiscountContainerDTO>();
            foreach (DiscountsDTO discountsDTO in discountsDTOList)
            {
                DiscountContainerDTO discountContainerDTO = CreateDiscountContainerDTO(discountsDTO);
                discountContainerDTOList.Add(discountContainerDTO);
                AddToDiscountDictionary(discountContainerDTO);
            }
            discountContainerDTOCollection = new DiscountContainerDTOCollection(discountContainerDTOList);
            log.Info("Number of items loaded by DiscountContainer " + siteId + ":" + discountsDTOList.Count);
            log.LogMethodExit();
        }

        private static List<DiscountsDTO> GetDiscountDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<DiscountsDTO> discountsDTOList = null;
            try
            {
                DiscountsListBL discountsListBL = new DiscountsListBL();
                SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                searchParameters.Add(DiscountsDTO.SearchByParameters.SITE_ID, siteId);
                discountsDTOList = discountsListBL.GetDiscountsDTOList(searchParameters, true, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the discounts.", ex);
            }

            if (discountsDTOList == null)
            {
                discountsDTOList = new List<DiscountsDTO>();
            }
            log.LogMethodExit(discountsDTOList);
            return discountsDTOList;
        }

        private static DateTime? GetDiscountModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                DiscountsListBL discountsListBL = new DiscountsListBL();
                result = discountsListBL.GetDiscountsModuleLastUpdateTime(siteId);

                ProductsList productsList = new ProductsList();
                DateTime? productModuleLastUpdateTime = productsList.GetProductsLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (productModuleLastUpdateTime.HasValue && result.Value < productModuleLastUpdateTime.Value))
                {
                    result = productModuleLastUpdateTime;
                }

                ProductGroupListBL productGroupListBL = new ProductGroupListBL();
                DateTime? productGroupModuleLastUpdateTime = productGroupListBL.GetProductGroupModuleLastUpdateTime(siteId);
                if (result.HasValue == false ||
                    (productGroupModuleLastUpdateTime.HasValue && result.Value < productGroupModuleLastUpdateTime.Value))
                {
                    result = productGroupModuleLastUpdateTime;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the discounts max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private DiscountContainerDTO CreateDiscountContainerDTO(DiscountsDTO discountsDTO)
        {
            log.LogMethodEntry(discountsDTO);
            int? discountPurchaseCriteriaCount = 0;
            int? discountPurchaseCriteriaQuantityCount = 0;
            int? discountPurchaseCriteriaValidityQuantityCount = 0;
            bool? overridingDiscountAmountExists = false;

            bool? overridingDiscountedPriceExists = false;
            bool? allProductsAreDiscounted = false;
            if (discountsDTO.DiscountPurchaseCriteriaDTOList != null)
            {
                foreach (var discountPurchaseCriteriaDTO in discountsDTO.DiscountPurchaseCriteriaDTOList)
                {
                    discountPurchaseCriteriaCount += discountPurchaseCriteriaDTO.MinQuantity == null ? 1 : discountPurchaseCriteriaDTO.MinQuantity;
                    discountPurchaseCriteriaQuantityCount += discountPurchaseCriteriaDTO.MinQuantity == null ? 0 : (int)discountPurchaseCriteriaDTO.MinQuantity;
                    discountPurchaseCriteriaValidityQuantityCount += discountPurchaseCriteriaDTO.MinQuantity == null || discountPurchaseCriteriaDTO.MinQuantity == 0 ? 1 : (int)discountPurchaseCriteriaDTO.MinQuantity;
                }
            }
            if (discountsDTO.DiscountedProductsDTOList != null)
            {
                foreach (var discountedProductsDTO in discountsDTO.DiscountedProductsDTOList)
                {
                    if (discountedProductsDTO.Discounted == "Y")
                    {
                        if (discountedProductsDTO.DiscountAmount != null && discountedProductsDTO.DiscountAmount > 0)
                        {
                            overridingDiscountAmountExists = true;
                            break;
                        }
                    }
                    if (discountedProductsDTO.Discounted == "Y")
                    {
                        if (discountedProductsDTO.DiscountedPrice != null && discountedProductsDTO.DiscountedPrice > 0)
                        {
                            overridingDiscountedPriceExists = true;
                            break;
                        }
                    }
                }
            }
            allProductsAreDiscounted = discountsDTO.DiscountedProductsDTOList == null || discountsDTO.DiscountedProductsDTOList.Count == 0;
            TransactionDiscountType transactionDiscountType = discountsDTO.DiscountPurchaseCriteriaDTOList == null || discountsDTO.DiscountPurchaseCriteriaDTOList.Count == 0? TransactionDiscountType.GENERIC : TransactionDiscountType.SPECIFIC;
            DiscountContainerDTO discountContainerDTO = new DiscountContainerDTO(discountsDTO.DiscountId, discountsDTO.DiscountName,
                discountsDTO.DiscountAmount, discountsDTO.DiscountPercentage, discountsDTO.DiscountType, discountsDTO.ManagerApprovalRequired, discountsDTO.AutomaticApply,
                discountsDTO.MinimumCredits, discountsDTO.MinimumSaleAmount, discountsDTO.DiscountCriteriaLines, discountsDTO.CouponMandatory, discountsDTO.VariableDiscounts,
                discountsDTO.RemarksMandatory, discountsDTO.DisplayInPOS, discountsDTO.AllowMultipleApplication, discountsDTO.TransactionProfileId, discountsDTO.ScheduleId, discountsDTO.ApplicationLimit, discountsDTO.IsActive,
                discountPurchaseCriteriaCount, discountPurchaseCriteriaQuantityCount, discountPurchaseCriteriaValidityQuantityCount, overridingDiscountAmountExists,
                overridingDiscountedPriceExists, allProductsAreDiscounted, transactionDiscountType, discountsDTO.SortOrder);

            if (discountsDTO.DiscountPurchaseCriteriaDTOList != null &&
                discountsDTO.DiscountPurchaseCriteriaDTOList.Any())
            {
                foreach (var discountPurchaseCriteriaDTO in discountsDTO.DiscountPurchaseCriteriaDTOList)
                {
                    DiscountPurchaseCriteriaContainerDTO discountPurchaseCriteriaContainerDTO = new DiscountPurchaseCriteriaContainerDTO(discountPurchaseCriteriaDTO.CriteriaId,
                                                                                                                                         discountPurchaseCriteriaDTO.DiscountId,
                                                                                                                                         discountPurchaseCriteriaDTO.ProductId,
                                                                                                                                         discountPurchaseCriteriaDTO.ProductGroupId,
                                                                                                                                         discountPurchaseCriteriaDTO.CategoryId,
                                                                                                                                         discountPurchaseCriteriaDTO.MinQuantity);
                    if (discountPurchaseCriteriaContainerDTO.CategoryId >= 0)
                    {
                        discountPurchaseCriteriaContainerDTO.ProductIdList = ProductsContainerList.GetProductsContainerDTOListOfCategory(siteId, discountPurchaseCriteriaContainerDTO.CategoryId).Select(x => x.ProductId).ToList();
                    }
                    else if(discountPurchaseCriteriaContainerDTO.ProductGroupId >= 0)
                    {
                        discountPurchaseCriteriaContainerDTO.ProductIdList = ProductGroupContainerList.GetRefferedProductIdHashSet(siteId, discountPurchaseCriteriaContainerDTO.ProductGroupId).ToList();
                    }
                    else if (discountPurchaseCriteriaContainerDTO.ProductId >= 0)
                    {
                        discountPurchaseCriteriaContainerDTO.ProductIdList = new List<int>() { discountPurchaseCriteriaContainerDTO.ProductId };
                    }
                    discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList.Add(discountPurchaseCriteriaContainerDTO);
                }
            }
            if (discountsDTO.DiscountedProductsDTOList != null &&
               discountsDTO.DiscountedProductsDTOList.Any())
            {
                foreach (var discountedProductsDTO in discountsDTO.DiscountedProductsDTOList)
                {
                    DiscountedProductsContainerDTO discountedProductsContainerDTO = new DiscountedProductsContainerDTO(discountedProductsDTO.Id,
                                                                                                                       discountedProductsDTO.DiscountId,
                                                                                                                       discountedProductsDTO.ProductId,
                                                                                                                       discountedProductsDTO.ProductGroupId,
                                                                                                                       discountedProductsDTO.CategoryId,
                                                                                                                       discountedProductsDTO.Quantity,
                                                                                                                       discountedProductsDTO.DiscountPercentage,
                                                                                                                       discountedProductsDTO.DiscountAmount,
                                                                                                                       discountedProductsDTO.DiscountedPrice,
                                                                                                                       discountedProductsDTO.Discounted);
                    if(discountedProductsContainerDTO.CategoryId >= 0)
                    {
                        discountedProductsContainerDTO.ProductIdList = ProductsContainerList.GetProductsContainerDTOListOfCategory(siteId, discountedProductsContainerDTO.CategoryId).Select(x =>x.ProductId).ToList();
                    }
                    else if (discountedProductsContainerDTO.ProductGroupId >= 0)
                    {
                        discountedProductsContainerDTO.ProductIdList = ProductGroupContainerList.GetRefferedProductIdHashSet(siteId, discountedProductsContainerDTO.ProductGroupId).ToList();
                    }
                    else if(discountedProductsContainerDTO.ProductId >= 0)
                    {
                        discountedProductsContainerDTO.ProductIdList = new List<int>() { discountedProductsContainerDTO.ProductId };
                    }
                    discountContainerDTO.DiscountedProductsContainerDTOList.Add(discountedProductsContainerDTO);
                }
            }
            if (discountsDTO.DiscountedGamesDTOList != null &&
                discountsDTO.DiscountedGamesDTOList.Any())
            {
                foreach (var discountedGamesDTO in discountsDTO.DiscountedGamesDTOList)
                {
                    discountContainerDTO.DiscountedGamesContainerDTOList.Add(new DiscountedGamesContainerDTO(discountedGamesDTO.Id,
                                                                                                           discountedGamesDTO.GameId,
                                                                                                           discountedGamesDTO.DiscountId,
                                                                                                           discountedGamesDTO.Discounted));
                }
            }
            log.LogMethodExit(discountContainerDTO);
            return discountContainerDTO;
        }

        private void AddToDiscountDictionary(DiscountContainerDTO discountContainerDTO)
        {
            log.LogMethodEntry(discountContainerDTO);
            if (discountContainerDTO.IsActive)
            {
                if (discountContainerDTO.DiscountType == DiscountsBL.DISCOUNT_TYPE_TRANSACTION)
                {
                    if (discountContainerDTO.AutomaticApply == "Y")
                    {
                        autoApplicableDiscountList.Add(discountContainerDTO);
                    }
                    else
                    {
                        manualDiscountList.Add(discountContainerDTO);
                    }
                    transactionDiscountList.Add(discountContainerDTO);
                }

                if (discountContainerDTO.DiscountType == DiscountsBL.DISCOUNT_TYPE_LOYALTY)
                {
                    loyaltyGameDiscountList.Add(discountContainerDTO);
                }
            }
            SetDefaultValues(discountContainerDTO);
            discountIdDiscountContainerDTODictionary[discountContainerDTO.DiscountId] = discountContainerDTO;
            if (discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList != null)
            {
                foreach (var discountPurchaseCriteriaContainerDTO in discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList)
                {
                    discountIdCriteriaProductIdListDictionary[discountPurchaseCriteriaContainerDTO.DiscountId].UnionWith(discountPurchaseCriteriaContainerDTO.ProductIdList);
                    if(discountIdCriteriaIdProductIdListDictionary[discountPurchaseCriteriaContainerDTO.DiscountId].ContainsKey(discountPurchaseCriteriaContainerDTO.CriteriaId) == false)
                    {
                        discountIdCriteriaIdProductIdListDictionary[discountPurchaseCriteriaContainerDTO.DiscountId].Add(discountPurchaseCriteriaContainerDTO.CriteriaId, new HashSet<int>(discountPurchaseCriteriaContainerDTO.ProductIdList));
                    }
                }
            }
            if (discountContainerDTO.DiscountedGamesContainerDTOList != null)
            {
                foreach (var discountedGamesContainerDTO in discountContainerDTO.DiscountedGamesContainerDTOList)
                {
                    if (discountedGamesContainerDTO.Discounted == "Y" &&
                        discountedGamesContainerDTO.GameId != -1 &&
                        discountIdDiscountedGamesDictionary[discountContainerDTO.DiscountId].ContainsKey(discountedGamesContainerDTO.GameId) == false)
                    {
                        discountIdDiscountedGamesDictionary[discountContainerDTO.DiscountId].Add(discountedGamesContainerDTO.GameId, discountedGamesContainerDTO);
                    }
                }
            }
            if (discountContainerDTO.DiscountedProductsContainerDTOList != null)
            {
                foreach (var discountedProductsContainerDTO in discountContainerDTO.DiscountedProductsContainerDTOList)
                {
                    if(discountedProductsContainerDTO.Discounted == "Y")
                    {
                        discountIdDiscountedProductIdListDictionary[discountedProductsContainerDTO.DiscountId].UnionWith(discountedProductsContainerDTO.ProductIdList);
                        if(discountIdDiscountedProductIdDiscountedProductsContainerDTODictionary[discountedProductsContainerDTO.DiscountId].ContainsKey(discountedProductsContainerDTO.Id) == false)
                        {
                            discountIdDiscountedProductIdDiscountedProductsContainerDTODictionary[discountedProductsContainerDTO.DiscountId].Add(discountedProductsContainerDTO.Id, discountedProductsContainerDTO);
                        }
                    }
                    if (discountedProductsContainerDTO.Discounted == "Y" &&  
                        discountIdDiscountedProductIdProductIdListDictionary[discountedProductsContainerDTO.DiscountId].ContainsKey(discountedProductsContainerDTO.Id) == false)
                    {
                        discountIdDiscountedProductIdProductIdListDictionary[discountedProductsContainerDTO.DiscountId].Add(discountedProductsContainerDTO.Id, new HashSet<int>(discountedProductsContainerDTO.ProductIdList));
                    }
                }
                discountContainerDTO.OverridingDiscountPercentageExists = discountContainerDTO.DiscountedProductsContainerDTOList.Any(x => x.DiscountPercentage.HasValue && x.DiscountPercentage.Value > 0);
            }
            
        }

        private void SetDefaultValues(DiscountContainerDTO discountContainerDTO)
        {
            log.LogMethodEntry();
            if (discountIdDiscountContainerDTODictionary.ContainsKey(discountContainerDTO.DiscountId) == false)
            {
                discountIdDiscountContainerDTODictionary.Add(discountContainerDTO.DiscountId, new DiscountContainerDTO());
            }
            if (discountIdDiscountedGamesDictionary.ContainsKey(discountContainerDTO.DiscountId) == false)
            {
                discountIdDiscountedGamesDictionary.Add(discountContainerDTO.DiscountId, new Dictionary<int, DiscountedGamesContainerDTO>());
            }
            if(discountIdCriteriaIdProductIdListDictionary.ContainsKey(discountContainerDTO.DiscountId) == false)
            {
                discountIdCriteriaIdProductIdListDictionary.Add(discountContainerDTO.DiscountId, new Dictionary<int, HashSet<int>>());
            }
            if (discountIdDiscountedProductIdProductIdListDictionary.ContainsKey(discountContainerDTO.DiscountId) == false)
            {
                discountIdDiscountedProductIdProductIdListDictionary.Add(discountContainerDTO.DiscountId, new Dictionary<int, HashSet<int>>());
            }
            if (discountIdCriteriaProductIdListDictionary.ContainsKey(discountContainerDTO.DiscountId) == false)
            {
                discountIdCriteriaProductIdListDictionary.Add(discountContainerDTO.DiscountId, new HashSet<int>());
            }
            if (discountIdDiscountedProductIdListDictionary.ContainsKey(discountContainerDTO.DiscountId) == false)
            {
                discountIdDiscountedProductIdListDictionary.Add(discountContainerDTO.DiscountId, new HashSet<int>());
            }
            if(discountIdDiscountedProductIdDiscountedProductsContainerDTODictionary.ContainsKey(discountContainerDTO.DiscountId) == false)
            {
                discountIdDiscountedProductIdDiscountedProductsContainerDTODictionary.Add(discountContainerDTO.DiscountId, new Dictionary<int, DiscountedProductsContainerDTO>());
            }
        }

        #region Methods

        public bool IsSimpleCriteria(int discountId, int criteriaId)
        {
            log.LogMethodEntry(discountId, criteriaId);
            bool result = false;
            if(discountIdCriteriaIdProductIdListDictionary.ContainsKey(discountId) == false ||
               discountIdCriteriaIdProductIdListDictionary[discountId].ContainsKey(criteriaId) == false)
            {
                string message = "Invalid discountId: " + discountId + " or criteriaId: " + criteriaId;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            result = discountIdCriteriaIdProductIdListDictionary[discountId][criteriaId].Count <= 1;
            log.LogMethodExit(result);
            return result;
        }

        public bool IsCriteriaProduct(int discountId, int criteriaId, int productId)
        {
            log.LogMethodEntry(discountId, criteriaId, productId);
            bool result = false;
            if (discountIdCriteriaIdProductIdListDictionary.ContainsKey(discountId) == false ||
               discountIdCriteriaIdProductIdListDictionary[discountId].ContainsKey(criteriaId) == false)
            {
                string message = "Invalid discountId: " + discountId + " or criteriaId: " + criteriaId;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            result = discountIdCriteriaIdProductIdListDictionary[discountId][criteriaId].Contains(productId);
            log.LogMethodExit(result);
            return result;
        }

        public bool IsDiscountedProduct(int discountId, int discountedProductId, int productId)
        {
            log.LogMethodEntry(discountId, discountedProductId, productId);
            bool result = false;
            if (discountIdDiscountedProductIdProductIdListDictionary.ContainsKey(discountId) == false ||
               discountIdDiscountedProductIdProductIdListDictionary[discountId].ContainsKey(discountedProductId) == false)
            {
                string message = "Invalid discountId: " + discountId + " or discountedProductId: " + discountedProductId;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            result = discountIdDiscountedProductIdProductIdListDictionary[discountId][discountedProductId].Contains(productId);
            log.LogMethodExit(result);
            return result;
        }

        public bool IsSimpleDiscountedProduct(int discountId, int discountedProductId)
        {
            log.LogMethodEntry(discountId, discountedProductId);
            bool result = false;
            if (discountIdDiscountedProductIdProductIdListDictionary.ContainsKey(discountId) == false ||
               discountIdDiscountedProductIdProductIdListDictionary[discountId].ContainsKey(discountedProductId) == false)
            {
                string message = "Invalid discountId: " + discountId + " or discountedProductId: " + discountedProductId;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            result = discountIdDiscountedProductIdProductIdListDictionary[discountId][discountedProductId].Count <= 1;
            log.LogMethodExit(result);
            return result;
        }

        public bool IsDiscounted(int discountId, int productId)
        {
            log.LogMethodEntry(discountId, productId);
            bool result = false;
            if (discountIdDiscountContainerDTODictionary.ContainsKey(discountId) == false)
            {
                string message = MessageContainerList.GetMessage(siteId, -1, 2196, "Discount", discountId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            result = discountIdDiscountContainerDTODictionary[discountId].AllProductsAreDiscounted || (discountIdDiscountedProductIdListDictionary.ContainsKey(discountId) && discountIdDiscountedProductIdListDictionary[discountId].Contains(productId));
            log.LogMethodExit(result);
            return result;
        }

        public bool IsDiscountedGame(int discountId, int gameId)
        {
            log.LogMethodEntry(discountId, gameId);
            bool result = false;
            if (discountIdDiscountedGamesDictionary.ContainsKey(discountId) == false)
            {
                string message = MessageContainerList.GetMessage(siteId, -1, 2196, "Discount", discountId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            result = discountIdDiscountedGamesDictionary[discountId].ContainsKey(gameId);
            log.LogMethodExit(result);
            return result;
        }

        public bool IsCriteria(int discountId, int productId)
        {
            log.LogMethodEntry(discountId, productId);
            bool result = false;
            if (discountIdCriteriaProductIdListDictionary.ContainsKey(discountId) == false)
            {
                string message = MessageContainerList.GetMessage(siteId, -1, 2196, "Discount", discountId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            result = discountIdCriteriaProductIdListDictionary[discountId].Contains(productId);
            log.LogMethodExit(result);
            return result;
        }

        internal DiscountedProductsContainerDTO GetDiscountedProductsContainerDTO(int discountId, int discountedProductId)
        {
            log.LogMethodEntry(discountId, discountedProductId);
            if (discountIdDiscountedProductIdDiscountedProductsContainerDTODictionary.ContainsKey(discountId) == false ||
                discountIdDiscountedProductIdDiscountedProductsContainerDTODictionary[discountId].ContainsKey(discountedProductId) == false)
            {
                string message = "Invalid discountId: " + discountId + " or discountedProductId: " + discountedProductId;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            DiscountedProductsContainerDTO result = discountIdDiscountedProductIdDiscountedProductsContainerDTODictionary[discountId][discountedProductId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Check whether schedule is active
        /// <param name="discountId">discountId</param>
        /// <param name="dateTime">dateTime</param>
        /// </summary>
        /// <returns></returns>
        public bool IsDiscountAvailable(int discountId, DateTime dateTime)
        {
            log.LogMethodEntry(discountId, dateTime);
            DateTimeRange dateTimeRange = GetDateTimeRange(dateTime);
            DiscountAvailabilityContainer discountAvailabilityContainer = GetDiscountAvailabilityContainer(dateTimeRange);
            bool returnValue = discountAvailabilityContainer.IsDiscountAvailable(discountId, dateTime);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private DiscountAvailabilityContainer GetDiscountAvailabilityContainer(DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(dateTimeRange);
            DiscountAvailabilityContainer result = discountAvailabilityContainerCache.GetOrAdd(dateTimeRange, (k) => new DiscountAvailabilityContainer(siteId, discountsDTOList, dateTimeRange, TimeSpan.FromMinutes(5)));
            log.LogMethodExit(result);
            return result;
        }

        private DateTimeRange GetDateTimeRange(DateTime dateTime)
        {
            return new DateTimeRange(dateTime.Date, dateTime.Date.AddDays(1));
        }

        public DiscountAvailabilityContainerDTOCollection GetDiscountAvailabilityDTOCollection(DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(dateTimeRange);
            DiscountAvailabilityContainer discountAvailabilityContainer = GetDiscountAvailabilityContainer(dateTimeRange);
            DiscountAvailabilityContainerDTOCollection returnValue = discountAvailabilityContainer.GetDiscountAvailabilityDTOCollection();
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// check for minimum required sale amount
        /// </summary>
        /// <param name="transactionAmount">Transaction amount</param>
        /// <returns></returns>
        public bool CheckMinimumSaleAmount(int discountId, decimal transactionAmount)
        {
            log.LogMethodEntry(transactionAmount);
            if (discountIdDiscountContainerDTODictionary.ContainsKey(discountId) == false)
            {
                string message = MessageContainerList.GetMessage(siteId, -1, 2196, "Discount", discountId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            bool returnValue = (discountIdDiscountContainerDTODictionary[discountId].MinimumSaleAmount == null ? 0 : Convert.ToDecimal(discountIdDiscountContainerDTODictionary[discountId].MinimumSaleAmount)) <=
                               transactionAmount;
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// check for minimum required credits played.
        /// </summary>
        /// <param name="maxCreditsPlayed">max Credits Played</param>
        /// <returns></returns>
        public bool CheckMinimumCreditsPlayed(int discountId, decimal maxCreditsPlayed)
        {
            log.LogMethodEntry(discountId, maxCreditsPlayed);
            bool returnValue = false;
            if (discountIdDiscountContainerDTODictionary.ContainsKey(discountId) == false)
            {
                string message = MessageContainerList.GetMessage(siteId, -1, 2196, "Discount", discountId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            if ((discountIdDiscountContainerDTODictionary[discountId].MinimumCredits == null ? 0 : Convert.ToDecimal(discountIdDiscountContainerDTODictionary[discountId].MinimumCredits)) > 0)
            {
                if (Convert.ToDecimal(discountIdDiscountContainerDTODictionary[discountId].MinimumCredits) <= maxCreditsPlayed)
                {
                    returnValue = true;
                }
            }
            else
            {
                returnValue = true;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public DiscountContainerDTO GetDiscountContainerDTO(int discountId)
        {
            log.LogMethodEntry(discountId);
            if (discountIdDiscountContainerDTODictionary.ContainsKey(discountId) == false)
            {
                string message = MessageContainerList.GetMessage(siteId, -1, 2196, "Discount", discountId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            DiscountContainerDTO result = discountIdDiscountContainerDTODictionary[discountId];
            log.LogMethodExit(result);
            return result;
        }

        public DiscountContainerDTO GetDiscountContainerDTOOrDefault(int discountId)
        {
            log.LogMethodEntry(discountId);
            if (discountIdDiscountContainerDTODictionary.ContainsKey(discountId) == false)
            {
                log.LogMethodExit(null, "discountIdDiscountContainerDTODictionary.ContainsKey(discountId) == false");
                return null;
            }
            DiscountContainerDTO result = discountIdDiscountContainerDTODictionary[discountId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the automatic discounts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DiscountContainerDTO> GetAutomaticDiscountsBLList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(autoApplicableDiscountList);
            return autoApplicableDiscountList;
        }

        /// <summary>
        /// Returns the transaction discounts 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DiscountContainerDTO> GetTransactionDiscountsBLList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(transactionDiscountList);
            return transactionDiscountList;
        }

        /// <summary>
        /// returns the loyalty game discounts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DiscountContainerDTO> GetManualDiscountsBLList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(manualDiscountList);
            return manualDiscountList;
        }

        /// <summary>
        /// returns the loyalty game discounts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DiscountContainerDTO> GetLoyaltyGameDiscountsBLList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(loyaltyGameDiscountList);
            return loyaltyGameDiscountList;
        }

        #endregion

        public DiscountContainerDTOCollection GetDiscountContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(discountContainerDTOCollection);
            return discountContainerDTOCollection;
        }

        /// <summary>
        /// Rebuilds the container
        /// </summary>
        public DiscountContainer Refresh()
        {
            log.LogMethodEntry();
            var dateTimeRangeList = discountAvailabilityContainerCache.Keys;
            foreach (var dateTimeRange in dateTimeRangeList)
            {
                if (dateTimeRange.EndDateTime < ServerDateTime.Now)
                {
                    DiscountAvailabilityContainer value;
                    if (discountAvailabilityContainerCache.TryRemove(dateTimeRange, out value))
                    {
                        log.Debug("Removing DiscountAvailabilityContainer of date range" + dateTimeRange);
                    }
                    else
                    {
                        log.Debug("Unable to remove DiscountAvailabilityContainer of date range" + dateTimeRange);
                    }
                }
            }
            DiscountsListBL discountsListBL = new DiscountsListBL();
            DateTime? updateTime = GetDiscountModuleLastUpdateTime(siteId);
            if (discountModuleLastUpdateTime.HasValue
                && discountModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in discount since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ProductsContainerList.Rebuild(siteId);
            ProductGroupContainerList.Rebuild(siteId);

            DiscountContainer result = new DiscountContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
