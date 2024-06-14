/********************************************************************************************
 * Project Name - Customer
 * Description  - MembershipMasterList class to get the List of 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020      Vikas Dwivedi             Modified : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Membership.Sample
{
    public class MembershipContainerList
    {
        /// Factory/Container Changes.
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, MembershipContainer> membershipContainerCache = new Cache<int, MembershipContainer>();
        private static Timer refreshTimer;

        /// <summary>
        /// Default Constructor - Factory Changes
        /// </summary>
        static MembershipContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        /// Factory/Container Changes.
        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = membershipContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                MembershipContainer membershipContainer;
                if (membershipContainerCache.TryGetValue(uniqueKey, out membershipContainer))
                {
                    membershipContainerCache[uniqueKey] = membershipContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        /// Factory/Container Changes.
        private static MembershipContainer GetMembershipContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            MembershipContainer result = membershipContainerCache.GetOrAdd(siteId, (k) => new MembershipContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// Factory/Container Changes.
        public static List<MembershipContainerDTO> GetMembershipContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            MembershipContainer container = GetMembershipContainer(siteId);
            List<MembershipContainerDTO> membershipContainerDTOList = container.GetMembershipContainerDTOList();
            log.LogMethodExit(membershipContainerDTOList);
            return membershipContainerDTOList;
        }

        public static MembershipContainerDTO GetMembershipContainerDTO(int siteId, int membershipId)
        {
            log.LogMethodEntry(siteId, membershipId);
            MembershipContainer container = GetMembershipContainer(siteId);
            MembershipContainerDTO result = container.GetMembershipContainerDTO(membershipId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the MembershipContainerDTO based on the site and executionContext else returns null
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static MembershipContainerDTO GetMembershipContainerDTOOrDefault(ExecutionContext executionContext, int membershipId)
        {
            log.LogMethodEntry(executionContext, membershipId);
            log.LogMethodExit();
            return GetMembershipContainerDTOOrDefault(executionContext.SiteId, membershipId);
        }

        /// <summary>
        /// /// Gets the MembershipContainerDTO based on the site and membershipId else returns null
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static MembershipContainerDTO GetMembershipContainerDTOOrDefault(int siteId, int membershipId)
        {
            log.LogMethodEntry(siteId, membershipId);
            MembershipContainer container = GetMembershipContainer(siteId);
            var result = container.GetMembershipContainerDTOOrDefault(membershipId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Rebuilds the Container - Factory/Container Changes.
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            MembershipContainer membershipContainer = GetMembershipContainer(siteId);
            membershipContainerCache[siteId] = membershipContainer.Refresh();
            log.LogMethodExit();
        }


        public static MembershipRewardsContainerDTO GetMembershipRewardsContainerDTO(int siteId, int membershipRewardsId)
        {
            log.LogMethodEntry(siteId, membershipRewardsId);
            MembershipContainer container = GetMembershipContainer(siteId);
            MembershipRewardsContainerDTO result = container.GetMembershipRewardsContainerDTO(membershipRewardsId);
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Gets the MembershipRewardsContainerDTO based on the site and executionContext else returns null
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static MembershipRewardsContainerDTO GetMembershipRewardsContainerDTOOrDefault(ExecutionContext executionContext, int membershipRewardsId)
        {
            log.LogMethodEntry(executionContext, membershipRewardsId);
            log.LogMethodExit();
            return GetMembershipRewardsContainerDTOOrDefault(executionContext.SiteId, membershipRewardsId);
        }

        /// <summary>
        /// /// Gets the MembershipRewardsContainerDTO based on the site and membershipId else returns null
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static MembershipRewardsContainerDTO GetMembershipRewardsContainerDTOOrDefault(int siteId, int membershipRewardsId)
        {
            log.LogMethodEntry(siteId, membershipRewardsId);
            MembershipContainer container = GetMembershipContainer(siteId);
            var result = container.GetMembershipRewardsContainerDTOOrDefault(membershipRewardsId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
