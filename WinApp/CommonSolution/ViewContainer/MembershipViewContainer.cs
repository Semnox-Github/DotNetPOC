/********************************************************************************************
 * Project Name - Customer
 * Description  - MembershipViewContainer class to get the List of 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Membership;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// MembershipViewContainer holds the MembershipViewContainer for a given siteId, userId and MembershipId
    /// </summary>
    public class MembershipViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ConcurrentDictionary<int, MembershipContainerDTO> membershipDictionary;
        //private readonly ConcurrentDictionary<int, string> membershipDictionary = new ConcurrentDictionary<int, string>();
        private readonly MembershipContainerDTOCollection membershipContainerDTOCollection;
        private readonly int siteId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="membershipContainerDTOCollection">membershipContainerDTOCollection</param>
        internal MembershipViewContainer(int siteId, MembershipContainerDTOCollection membershipContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, membershipContainerDTOCollection);
            this.siteId = siteId;
            this.membershipContainerDTOCollection = membershipContainerDTOCollection;
            this.membershipDictionary = new ConcurrentDictionary<int, MembershipContainerDTO>();
            if (membershipContainerDTOCollection != null &&
                membershipContainerDTOCollection.MembershipContainerDTOList != null &&
                membershipContainerDTOCollection.MembershipContainerDTOList.Any())
            {
                foreach (var membershipContainerDTO in membershipContainerDTOCollection.MembershipContainerDTOList)
                {
                    membershipDictionary[membershipContainerDTO.MembershipId] = membershipContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal MembershipViewContainer(int siteId)
            : this(siteId, GetMembershipContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static MembershipContainerDTOCollection GetMembershipContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            MembershipContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IMembershipUseCases membershipUseCases = CustomerUseCaseFactory.GetMembershipUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<MembershipContainerDTOCollection> task = membershipUseCases.GetMembershipContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving MembershipContainerDTOCollection.", ex);
                result = new MembershipContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in MembershipContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal MembershipContainerDTOCollection GetMembershipContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (membershipContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(membershipContainerDTOCollection);
            return membershipContainerDTOCollection;
        }

        internal MembershipViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            MembershipContainerDTOCollection latestMembershipContainerDTOCollection = GetMembershipContainerDTOCollection(siteId, membershipContainerDTOCollection.Hash, rebuildCache);
            if (latestMembershipContainerDTOCollection == null ||
                latestMembershipContainerDTOCollection.MembershipContainerDTOList == null ||
                latestMembershipContainerDTOCollection.MembershipContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            MembershipViewContainer result = new MembershipViewContainer(siteId, latestMembershipContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
