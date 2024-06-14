/********************************************************************************************
 * Project Name - Customer
 * Description  - MembershipViewContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020       Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Membership;

namespace Semnox.Parafait.ViewContainer
{
    public class MembershipViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, MembershipViewContainer> membershipViewContainerCache = new Cache<int, MembershipViewContainer>();
        private static Timer refreshTimer;
        static MembershipViewContainerList()
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
            var uniqueKeyList = membershipViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                MembershipViewContainer membershipViewContainer;
                if (membershipViewContainerCache.TryGetValue(uniqueKey, out membershipViewContainer))
                {
                    membershipViewContainerCache[uniqueKey] = membershipViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static MembershipViewContainer GetMembershipViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            MembershipViewContainer result = membershipViewContainerCache.GetOrAdd(siteId, (k)=> new MembershipViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static MembershipContainerDTOCollection GetMembershipContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            MembershipViewContainer container = GetMembershipViewContainer(siteId);
            MembershipContainerDTOCollection membershipContainerDTOCollection = container.GetMembershipContainerDTOCollection(hash);
            return membershipContainerDTOCollection;
        }

        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            MembershipViewContainer container = GetMembershipViewContainer(siteId);
            membershipViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
