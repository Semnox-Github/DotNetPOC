/********************************************************************************************
 * Project Name - View Container
 * Description  - PriceViewContainerList holds view container values based on siteId, userRoleId, POSMachineId, languageId and dateTimeRange
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.1      10-Aug-2021      Lakshminarayana           Created : price container enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.ProductPrice;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// holds view container values based on siteId, userRoleId, POSMachineId, languageId and dateTimeRange
    /// </summary>
    public class PriceViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<PriceContainerKey, PriceViewContainer> priceViewContainerCache = new Cache<PriceContainerKey, PriceViewContainer>();
        private static Timer refreshTimer;

        static PriceViewContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = priceViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                PriceViewContainer priceViewContainer;
                if (priceViewContainerCache.TryGetValue(uniqueKey, out priceViewContainer))
                {
                    priceViewContainerCache[uniqueKey] = priceViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static PriceViewContainer GetPriceViewContainer(int siteId, int membershipId, int userRoleId, int transactionProfileId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(siteId);
            PriceContainerKey key = new PriceContainerKey(siteId, membershipId, userRoleId, transactionProfileId, dateTimeRange);
            var result = priceViewContainerCache.GetOrAdd(key, (k) => new PriceViewContainer(key));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the PriceViewContainerDTOCollection for a given keys and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="membershipId"></param>
        /// <param name="userRoleId"></param>
        /// <param name="transactionProfileId"></param>
        /// <param name="startDateTime">start date time</param>
        /// <param name="endDateTime">end date time</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static PriceContainerDTOCollection GetPriceContainerDTOCollection(int siteId, int membershipId, int userRoleId, int transactionProfileId, DateTime startDateTime, DateTime endDateTime, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            PriceViewContainer priceViewContainer = GetPriceViewContainer(siteId, membershipId, userRoleId, transactionProfileId, new DateTimeRange(startDateTime, endDateTime));
            PriceContainerDTOCollection result = priceViewContainer.GetPriceContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(ExecutionContext executionContext, int membershipId, int transactionProfileId)
        {
            log.LogMethodEntry(executionContext);
            Rebuild(executionContext, membershipId, transactionProfileId, DateTime.Now);
            log.LogMethodExit();
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(ExecutionContext executionContext, int membershipId, int transactionProfileId, DateTime dateTime)
        {
            log.LogMethodEntry(executionContext, dateTime);
            int userRoleId = UserViewContainerList.GetUserContainerDTO(executionContext.SiteId, executionContext.UserId).RoleId;
            Rebuild(executionContext.SiteId, membershipId, userRoleId, transactionProfileId, GetDateTimeRange(dateTime));
            log.LogMethodExit();
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId, int membershipId, int userRoleId, int transactionProfileId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(siteId, dateTimeRange);
            PriceContainerKey key = new PriceContainerKey(siteId, membershipId, userRoleId, transactionProfileId, dateTimeRange);
            PriceViewContainer priceViewContainer = GetPriceViewContainer(siteId, membershipId, userRoleId, transactionProfileId, dateTimeRange);
            priceViewContainerCache[key] = priceViewContainer.Refresh(true);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PriceViewContainerDTO based on the site and priceId
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

        /// <summary>
        /// Returns the PriceViewContainerDTO based on the site and priceId
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
            PriceViewContainer priceViewContainer = GetPriceViewContainer(siteId, membershipId, userRoleId, transactionProfileId, dateTimeRange);
            PriceContainerDTO result = priceViewContainer.GetPriceContainerDTO(productId);
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
        public static PriceContainerDetailDTO GetPriceViewContainerDetailDTO(int siteId, int productId, int membershipId, int userRoleId, int transactionProfileId, DateTime dateTime)
        {
            log.LogMethodEntry(siteId, productId, membershipId, userRoleId, dateTime);
            PriceViewContainer priceViewContainer = GetPriceViewContainer(siteId, membershipId, userRoleId, transactionProfileId, GetDateTimeRange(dateTime));
            PriceContainerDetailDTO result = priceViewContainer.GetPriceContainerDetailDTO(productId, dateTime);
            log.LogMethodExit();
            return result;
        }

        private static DateTimeRange GetDateTimeRange(DateTime dateTime)
        {
            return new DateTimeRange(dateTime.Date, dateTime.Date.AddDays(1));
        }

    }
}
