/********************************************************************************************
 * Project Name - Discount
 * Description  - DiscountContainerList class to get the List of values from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.150.0     12-Apr-2021       Abhishek          Created: POS Redesign
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Discounts
{
    public static class DiscountContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, DiscountContainer> discountContainerCache = new Cache<int, DiscountContainer>();
        private static Timer refreshTimer;

        /// <summary>
        /// Default Constructor 
        /// </summary>
        static DiscountContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = discountContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                DiscountContainer discountContainer;
                if (discountContainerCache.TryGetValue(uniqueKey, out discountContainer))
                {
                    discountContainerCache[uniqueKey] = discountContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static DiscountContainer GetDiscountContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            DiscountContainer result = discountContainerCache.GetOrAdd(siteId, (k) => new DiscountContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the collection of discounts
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static DiscountContainerDTOCollection GetDiscountContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            DiscountContainer container = GetDiscountContainer(siteId);
            DiscountContainerDTOCollection result = container.GetDiscountContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public static DiscountAvailabilityContainerDTOCollection GetDiscountAvailabilityDTOCollection(int siteId, DateTime startDateTime, DateTime endDateTime)
        {
            log.LogMethodEntry(siteId);
            DiscountContainer container = GetDiscountContainer(siteId);
            var result = container.GetDiscountAvailabilityDTOCollection(new DateTimeRange(startDateTime, endDateTime));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            discountContainerCache[siteId] = discountContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the  true or false based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="productId">product id</param>
        /// <returns></returns>
        public static bool IsCriteriaProduct(int siteId, int discountId, int productId)
        {
            log.LogMethodEntry(siteId, discountId, productId);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool isCriteriaProduct = discountContainer.IsCriteria(discountId, productId);
            log.LogMethodExit(isCriteriaProduct);
            return isCriteriaProduct;
        }

        /// <summary>
        /// Returns the true or false based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="productId">product id</param>
        /// <returns></returns>
        public static bool IsCriteriaProduct(ExecutionContext executionContext, int discountId, int productId)
        {
            log.LogMethodEntry(executionContext, discountId, productId);
            bool isCriteriaProduct = IsCriteriaProduct(executionContext.GetSiteId(), discountId, productId);
            log.LogMethodExit(isCriteriaProduct);
            return isCriteriaProduct;
        }

        /// <summary>
        /// Returns the  true or false based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="criteriaId">criteria id</param>
        /// <returns></returns>
        public static bool IsSimpleCriteria(int siteId, int discountId, int criteriaId)
        {
            log.LogMethodEntry(siteId, discountId, criteriaId);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool result = discountContainer.IsSimpleCriteria(discountId, criteriaId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the true or false based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="criteriaId">criteria id</param>
        /// <returns></returns>
        public static bool IsSimpleCriteria(ExecutionContext executionContext, int discountId, int criteriaId)
        {
            log.LogMethodEntry(executionContext, discountId, criteriaId);
            bool isCriteriaProduct = IsSimpleCriteria(executionContext.GetSiteId(), discountId, criteriaId);
            log.LogMethodExit(isCriteriaProduct);
            return isCriteriaProduct;
        }

        /// <summary>
        /// Returns the  true or false based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="criteriaId">criteria id</param>
        /// <param name="productId">product id</param>
        /// <returns></returns>
        public static bool IsCriteriaProduct(int siteId, int discountId, int criteriaId, int productId)
        {
            log.LogMethodEntry(siteId, discountId, criteriaId);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool result = discountContainer.IsCriteriaProduct(discountId, criteriaId, productId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the true or false based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="criteriaId">criteria id</param>
        /// <param name="productId">criteria id</param>
        /// <returns></returns>
        public static bool IsCriteriaProduct(ExecutionContext executionContext, int discountId, int criteriaId, int productId)
        {
            log.LogMethodEntry(executionContext, discountId, criteriaId, productId);
            bool isCriteriaProduct = IsCriteriaProduct(executionContext.GetSiteId(), discountId, criteriaId, productId);
            log.LogMethodExit(isCriteriaProduct);
            return isCriteriaProduct;
        }


        /// <summary>
        /// Returns the  true or false based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="discountedProductId">criteria id</param>
        /// <param name="productId">product id</param>
        /// <returns></returns>
        public static bool IsDiscountedProduct(int siteId, int discountId, int discountedProductId, int productId)
        {
            log.LogMethodEntry(siteId, discountId, discountedProductId, productId);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool result = discountContainer.IsDiscountedProduct(discountId, discountedProductId, productId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the true or false based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="discountedProductId">criteria id</param>
        /// <param name="productId">criteria id</param>
        /// <returns></returns>
        public static bool IsDiscountedProduct(ExecutionContext executionContext, int discountId, int discountedProductId, int productId)
        {
            log.LogMethodEntry(executionContext, discountId, discountedProductId, productId);
            bool result = IsDiscountedProduct(executionContext.GetSiteId(), discountId, discountedProductId, productId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the  true or false based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="discountedProductId">discounted product id</param>
        /// <returns></returns>
        public static bool IsSimpleDiscountedProduct(int siteId, int discountId, int discountedProductId)
        {
            log.LogMethodEntry(siteId, discountId, discountedProductId);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool result = discountContainer.IsSimpleDiscountedProduct(discountId, discountedProductId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the true or false based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="discountedProductId">discounted product id</param>
        /// <returns></returns>
        public static bool IsSimpleDiscountedProduct(ExecutionContext executionContext, int discountId, int discountedProductId)
        {
            log.LogMethodEntry(executionContext, discountId, discountedProductId);
            bool isDiscountedProductProduct = IsSimpleDiscountedProduct(executionContext.GetSiteId(), discountId, discountedProductId);
            log.LogMethodExit(isDiscountedProductProduct);
            return isDiscountedProductProduct;
        }

        /// <summary>
        /// Returns the true or false based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="productId">product id</param>
        /// <param name="categoryId">category id</param>
        /// <returns></returns>
        public static bool IsCriteria(int siteId, int discountId, int productId)
        {
            log.LogMethodEntry(siteId, discountId, productId);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool isCriteria = discountContainer.IsCriteria(discountId, productId);
            log.LogMethodExit(isCriteria);
            return isCriteria;
        }

        /// <summary>
        /// Returns the true or false based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="productId">product id</param>
        /// <param name="categoryId">category id</param>
        /// <returns></returns>
        public static bool IsCriteria(ExecutionContext executionContext, int discountId, int productId)
        {
            log.LogMethodEntry(executionContext, discountId, productId);
            bool isCriteria = IsCriteria(executionContext.GetSiteId(), discountId, productId);
            log.LogMethodExit(isCriteria);
            return isCriteria;
        }

        

        /// <summary>
        /// Returns the true or false based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="productId">product id</param>
        /// <param name="categoryId">category id</param>
        /// <returns></returns>
        public static bool IsDiscounted(int siteId, int discountId, int productId)
        {
            log.LogMethodEntry(siteId, discountId, productId);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool isDiscounted = discountContainer.IsDiscounted(discountId, productId);
            log.LogMethodExit(isDiscounted);
            return isDiscounted;
        }

        /// <summary>
        /// Returns the true or false based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="productId">product id</param>
        /// <param name="categoryId">category id</param>
        /// <returns></returns>
        public static bool IsDiscounted(ExecutionContext executionContext, int discountId, int productId)
        {
            log.LogMethodEntry(executionContext, discountId, productId);
           
            bool isDiscounted = IsDiscounted(executionContext.GetSiteId(), discountId, productId);
            log.LogMethodExit(isDiscounted);
            return isDiscounted;
        }

        /// <summary>
        /// Check whether schedule is active based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="dateTime">dateTime</param>
        /// <returns></returns>
        public static bool IsDiscountAvailable(int siteId, int discountId, DateTime dateTime)
        {
            log.LogMethodEntry(siteId, discountId, dateTime);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool returnValue = discountContainer.IsDiscountAvailable(discountId, dateTime);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Check whether schedule is active based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="dateTime">dateTime</param>
        /// <returns></returns>
        public static bool IsDiscountAvailable(ExecutionContext executionContext, int discountId, DateTime dateTime)
        {
            log.LogMethodEntry(executionContext, discountId,dateTime);
            bool returnValue = IsDiscountAvailable(executionContext.GetSiteId(), discountId, dateTime);
            log.LogMethodExit(returnValue);
            return returnValue; 
        }

        /// <summary>
        /// check for minimum required minimum sale amount based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="transactionAmount">transactionAmount</param>
        /// <returns></returns>
        public static bool CheckMinimumSaleAmount(int siteId, int discountId, decimal transactionAmount)
        {
            log.LogMethodEntry(siteId, discountId, transactionAmount);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool returnValue = discountContainer.CheckMinimumSaleAmount(discountId, transactionAmount);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// check for minimum required sale amount based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="transactionAmount">transactionAmount</param>
        /// <returns></returns>
        public static bool CheckMinimumSaleAmount(ExecutionContext executionContext, int discountId, decimal transactionAmount)
        {
            log.LogMethodEntry(executionContext, discountId, transactionAmount);
            bool returnValue = CheckMinimumSaleAmount(executionContext.GetSiteId(), discountId, transactionAmount);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// check for minimum required credits played based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="maxCreditsPlayed">maxCreditsPlayed</param>
        /// <returns></returns>
        public static bool CheckMinimumCreditsPlayed(int siteId, int discountId, decimal maxCreditsPlayed)
        {
            log.LogMethodEntry(siteId, discountId, maxCreditsPlayed);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool returnValue = discountContainer.CheckMinimumCreditsPlayed(discountId, maxCreditsPlayed);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// check for minimum required credits played based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="maxCreditsPlayed">maxCreditsPlayed</param>
        /// <returns></returns>
        public static bool CheckMinimumCreditsPlayed(ExecutionContext executionContext, int discountId, decimal maxCreditsPlayed)
        {
            log.LogMethodEntry(executionContext, discountId, maxCreditsPlayed);
            bool returnValue = CheckMinimumCreditsPlayed(executionContext.GetSiteId(), discountId, maxCreditsPlayed);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        

        /// <summary>
        /// Returns the true or false based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="gameId">game Id</param>
        /// <returns></returns>
        public static bool IsDiscountedGame(int siteId, int discountId, int gameId)
        {
            log.LogMethodEntry(siteId, discountId, gameId);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            bool isDiscountedGame = discountContainer.IsDiscountedGame(discountId, gameId);
            log.LogMethodExit(isDiscountedGame);
            return isDiscountedGame;
        }

        /// <summary>
        /// Returns the true or false based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="gameId">game Id</param>
        /// <returns></returns>
        public static bool IsDiscountedGame(ExecutionContext executionContext, int discountId, int gameId)
        {
            log.LogMethodEntry(executionContext, discountId, gameId);
            bool returnValue = IsDiscountedGame(executionContext.GetSiteId(), discountId, gameId);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the true or false based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="gameId">game Id</param>
        /// <returns></returns>
        public static DiscountContainerDTO GetDiscountContainerDTO(int siteId, int discountId)
        { 
            log.LogMethodEntry(siteId, discountId);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            DiscountContainerDTO discountContainerDTO = discountContainer.GetDiscountContainerDTO(discountId);
            log.LogMethodExit(discountContainerDTO);
            return discountContainerDTO;
        }

        /// <summary>
        /// Returns the true or false based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="gameId">game Id</param>
        /// <returns></returns>
        public static DiscountContainerDTO GetDiscountContainerDTO(ExecutionContext executionContext, int discountId)
        {
            log.LogMethodEntry(executionContext, discountId);
            DiscountContainerDTO discountContainerDTO = GetDiscountContainerDTO(executionContext.GetSiteId(), discountId);
            log.LogMethodExit(discountContainerDTO);
            return discountContainerDTO;
        }

        /// <summary>
        /// Returns the true or false based on the siteId
        /// </summary>
        /// <param name="siteId">current application siteId</param>
        /// <param name="discountId">discount id</param>
        /// <param name="gameId">game Id</param>
        /// <returns></returns>
        public static DiscountContainerDTO GetDiscountContainerDTOOrDefault(int siteId, int discountId)
        {
            log.LogMethodEntry(siteId, discountId);
            DiscountContainer discountContainer = GetDiscountContainer(siteId);
            DiscountContainerDTO discountContainerDTO = discountContainer.GetDiscountContainerDTOOrDefault(discountId);
            log.LogMethodExit(discountContainerDTO);
            return discountContainerDTO;
        }

        public static DiscountedProductsContainerDTO GetDiscountedProductsContainerDTO(ExecutionContext executionContext, int discountId, int discountedProductId)
        {
            log.LogMethodEntry(executionContext, discountId, discountedProductId);
            DiscountContainer discountContainer = GetDiscountContainer(executionContext.SiteId);
            DiscountedProductsContainerDTO result = discountContainer.GetDiscountedProductsContainerDTO(discountId, discountedProductId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the true or false based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="discountId">discount id</param>
        /// <param name="gameId">game Id</param>
        /// <returns></returns>
        public static DiscountContainerDTO GetDiscountContainerDTOOrDefault(ExecutionContext executionContext, int discountId)
        {
            log.LogMethodEntry(executionContext, discountId);
            DiscountContainerDTO discountContainerDTO = GetDiscountContainerDTOOrDefault(executionContext.GetSiteId(), discountId);
            log.LogMethodExit(discountContainerDTO);
            return discountContainerDTO;
        }

        public static IEnumerable<DiscountContainerDTO> GetAutomaticDiscountsBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            DiscountContainer discountContainer = GetDiscountContainer(executionContext.GetSiteId());
            IEnumerable<DiscountContainerDTO> result = discountContainer.GetAutomaticDiscountsBLList();
            log.LogMethodExit(result);
            return result;
        }

        public static IEnumerable<DiscountContainerDTO> GetTransactionDiscountsBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            DiscountContainer discountContainer = GetDiscountContainer(executionContext.GetSiteId());
            IEnumerable<DiscountContainerDTO> result = discountContainer.GetTransactionDiscountsBLList();
            log.LogMethodExit(result);
            return result;
        }

        public static IEnumerable<DiscountContainerDTO> GetLoyaltyGameDiscountsBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            DiscountContainer discountContainer = GetDiscountContainer(executionContext.GetSiteId());
            IEnumerable<DiscountContainerDTO> result = discountContainer.GetLoyaltyGameDiscountsBLList();
            log.LogMethodExit(result);
            return result;
        }

        public static IEnumerable<DiscountContainerDTO> GetManualDiscountsBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            DiscountContainer discountContainer = GetDiscountContainer(executionContext.GetSiteId());
            IEnumerable<DiscountContainerDTO> result = discountContainer.GetManualDiscountsBLList();
            log.LogMethodExit(result);
            return result;
        }

    }
}
