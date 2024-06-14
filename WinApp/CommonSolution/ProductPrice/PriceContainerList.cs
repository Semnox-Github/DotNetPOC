/********************************************************************************************
 * Project Name - ProductPrice
 * Description  - PricesContainerList class to get the List of  price containers
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 * 2.130.0    18-Aug-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Timers;

namespace Semnox.Parafait.ProductPrice
{
    public class PriceContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, PriceContainer> priceContainerCache = new Cache<int, PriceContainer>();
        private static Timer refreshTimer;

        static PriceContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = priceContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                PriceContainer priceContainer;
                if (priceContainerCache.TryGetValue(uniqueKey, out priceContainer))
                {
                    priceContainerCache[uniqueKey] = priceContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static PriceContainer GetPriceContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            PriceContainer result = priceContainerCache.GetOrAdd(siteId, (k) => new PriceContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the price container dto collection
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="membershipId"></param>
        /// <param name="userRoleId"></param>
        /// <param name="transactionProfileId"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static PriceContainerDTOCollection GetPriceContainerDTOCollection(int siteId, int membershipId, int userRoleId, int transactionProfileId, DateTime startDateTime, DateTime endDateTime, string hash)
        {
            log.LogMethodEntry(siteId);
            PriceContainer container = GetPriceContainer(siteId);
            PriceContainerDTOCollection result = container.GetPriceContainerDTOCollection(membershipId, userRoleId, transactionProfileId, new DateTimeRange(startDateTime, endDateTime));
            if (hash == result.Hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            PriceContainer priceContainer = GetPriceContainer(siteId);
            priceContainerCache[siteId] = priceContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PriceContainerDTO based on the site and priceId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="productId">product Id</param>
        /// <param name="membershipId">membership Id</param>
        /// <param name="userRoleId">user role Id</param>
        /// <param name="transactionProfileId">transaction profile id</param>
        /// <param name="dateTime">date time range</param>
        /// <returns></returns>
        public static PriceContainerDTO GetPriceContainerDTO(int siteId, int productId, int membershipId, int userRoleId, int transactionProfileId, DateTime dateTime)
        {
            log.LogMethodEntry(siteId, productId, membershipId, userRoleId, dateTime);
            DateTimeRange dateTimeRange = GetDateTimeRange(dateTime);
            PriceContainerDTO result = GetPriceContainerDTO(siteId, productId, membershipId, userRoleId, transactionProfileId, dateTimeRange);
            log.LogMethodExit();
            return result;
        }

        private static DateTimeRange GetDateTimeRange(DateTime dateTime)
        {
            return new DateTimeRange(dateTime.Date, dateTime.Date.AddDays(1));
        }

        /// <summary>
        /// Returns the PriceContainerDTO based on the site and priceId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="productId">product Id</param>
        /// <param name="membershipId">membership Id</param>
        /// <param name="userRoleId">user role Id</param>
        /// <param name="transactionProfileId">transaction profile Id</param>
        /// <param name="dateTimeRange">date time range</param>
        /// <returns></returns>
        public static PriceContainerDTO GetPriceContainerDTO(int siteId, int productId, int membershipId, int userRoleId, int transactionProfileId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(siteId, productId, membershipId, userRoleId, dateTimeRange);
            PriceContainer priceContainer = GetPriceContainer(siteId);
            PriceContainerDTO result = priceContainer.GetPriceContainerDTO(productId, membershipId, userRoleId, transactionProfileId, dateTimeRange);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the PriceContainerDTO based on the site and priceId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="productId">product Id</param>
        /// <param name="membershipId">membershipId</param>
        /// <param name="userRoleId">user role Id</param>
        /// <param name="transactionProfileId">transaction profile Id</param>
        /// <param name="dateTime">date time</param>
        /// <returns></returns>
        public static PriceContainerDetailDTO GetPriceContainerDetailDTO(int siteId, int productId, int membershipId, int userRoleId, int transactionProfileId, DateTime dateTime)
        {
            log.LogMethodEntry(siteId, productId, membershipId, userRoleId, dateTime);
            PriceContainer priceContainer = GetPriceContainer(siteId);
            DateTimeRange dateTimeRange = GetDateTimeRange(dateTime);
            PriceContainerDetailDTO result = priceContainer.GetPriceContainerDetailDTO(productId, membershipId, userRoleId, transactionProfileId, dateTimeRange, dateTime);
            log.LogMethodExit();
            return result;
        }
    }
}
