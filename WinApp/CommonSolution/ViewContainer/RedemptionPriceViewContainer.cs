
/********************************************************************************************
 * Project Name - Utilities
 * Description  - RedemptionPriceViewContainer holds the price in tickets for a siteId, productId and membershipId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// RedemptionPriceViewContainer holds the price in tickets for a siteId, productId and membershipId
    /// </summary>
    public class RedemptionPriceViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RedemptionPriceContainerDTOCollection redemptionPriceContainerDTOCollection;
        private readonly Dictionary<int, Dictionary<int, decimal>> productIdMembershipIdPriceInTicketsDictionary = new Dictionary<int, Dictionary<int, decimal>>();
        private readonly int siteId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="redemptionPriceContainerDTOCollection">redemptionPriceContainerDTOCollection</param>
        internal RedemptionPriceViewContainer(int siteId, RedemptionPriceContainerDTOCollection redemptionPriceContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, redemptionPriceContainerDTOCollection);
            this.siteId = siteId;
            this.redemptionPriceContainerDTOCollection = redemptionPriceContainerDTOCollection;
            if (redemptionPriceContainerDTOCollection != null &&
                redemptionPriceContainerDTOCollection.RedemptionPriceContainerDTOList != null &&
                redemptionPriceContainerDTOCollection.RedemptionPriceContainerDTOList.Any())
            {
                foreach (var redemptionPriceContainerDTO in redemptionPriceContainerDTOCollection.RedemptionPriceContainerDTOList)
                {
                    if (productIdMembershipIdPriceInTicketsDictionary.ContainsKey(redemptionPriceContainerDTO.ProductId) == false)
                    {
                        productIdMembershipIdPriceInTicketsDictionary.Add(redemptionPriceContainerDTO.ProductId, new Dictionary<int, decimal>());
                    }
                    if (productIdMembershipIdPriceInTicketsDictionary[redemptionPriceContainerDTO.ProductId].ContainsKey(redemptionPriceContainerDTO.MembershipId) == false)
                    {
                        productIdMembershipIdPriceInTicketsDictionary[redemptionPriceContainerDTO.ProductId].Add(redemptionPriceContainerDTO.MembershipId, redemptionPriceContainerDTO.PriceInTickets);
                    }
                }
            }
            log.LogMethodExit();
        }

        internal RedemptionPriceViewContainer(int siteId)
            : this(siteId, GetRedemptionPriceContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static RedemptionPriceContainerDTOCollection GetRedemptionPriceContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            RedemptionPriceContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<RedemptionPriceContainerDTOCollection> task = redemptionUseCases.GetRedemptionPriceContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving RedemptionPriceContainerDTOCollection.", ex);
                result = new RedemptionPriceContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }

        internal decimal GetPriceInTickets(int productId, int membershipId)
        {
            log.LogMethodEntry(productId, membershipId);

            if (productIdMembershipIdPriceInTicketsDictionary.ContainsKey(productId) == false)
            {
                string errorMessage = "Redeemable product with productId :" + productId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            if (productIdMembershipIdPriceInTicketsDictionary[productId].ContainsKey(membershipId) == false)
            {
                string errorMessage = "Membership with membershipId :" + membershipId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            decimal result = productIdMembershipIdPriceInTicketsDictionary[productId][membershipId];
            log.LogMethodExit(result);
            return result;
        }

        internal decimal GetLeastPriceInTickets(int productId, List<int> membershipIdList)
        {
            log.LogMethodEntry(productId, membershipIdList);
            decimal? priceInTickets = null;
            if (membershipIdList == null || membershipIdList.Any() == false)
            {
                priceInTickets = GetPriceInTickets(productId,-1);
                //string errorMessage = "MembershipIdList is empty.";
                //log.LogMethodExit("Throwing Exception - " + errorMessage);
                //throw new Exception(errorMessage);
            }
            foreach (var membershipId in membershipIdList)
            {
                var value = GetPriceInTickets(productId, membershipId);
                if (priceInTickets == null || value < priceInTickets.Value)
                {
                    priceInTickets = value;
                }
            }
            decimal result = priceInTickets.Value;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in RedemptionPriceContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal RedemptionPriceContainerDTOCollection GetRedemptionPriceContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (redemptionPriceContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(redemptionPriceContainerDTOCollection);
            return redemptionPriceContainerDTOCollection;
        }

        internal RedemptionPriceViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            RedemptionPriceContainerDTOCollection latestRedemptionPriceContainerDTOCollection = GetRedemptionPriceContainerDTOCollection(siteId, redemptionPriceContainerDTOCollection.Hash, rebuildCache);
            if (latestRedemptionPriceContainerDTOCollection == null ||
                latestRedemptionPriceContainerDTOCollection.RedemptionPriceContainerDTOList == null ||
                latestRedemptionPriceContainerDTOCollection.RedemptionPriceContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            RedemptionPriceViewContainer result = new RedemptionPriceViewContainer(siteId, latestRedemptionPriceContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
