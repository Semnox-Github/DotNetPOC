/********************************************************************************************
 * Project Name - Customer
 * Description  - RemoteMembershipUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.120.0      07-May-2021      B Mahesh Pai              SaveAllMembership code added
 2.140.1      20-Dec-2021      Abhishek                  WMS fix : Added parameter loadActiveChildRecords
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Membership
{
    public class RemoteMembershipUseCases : RemoteUseCases, IMembershipUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MEMBERSHIP_URL = "api/Customer/Membership/Memberships";
        private const string MEMBERSHIP_CONTAINER_URL = "api/Customer/Membership/MembershipsContainer";

        public RemoteMembershipUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MembershipDTO.SearchByParameters, string>> memberShipSearchParams)
        {
            log.LogMethodEntry(memberShipSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MembershipDTO.SearchByParameters, string> searchParameter in memberShipSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MembershipDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case MembershipDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MembershipDTO.SearchByParameters.MEMBERSHIP_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("membershipName".ToString(), searchParameter.Value));
                        }
                        break;
                    case MembershipDTO.SearchByParameters.MEMBERSHIP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("membershipId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<MembershipDTO>> GetAllMemberships(List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords", loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChildRecords", loadActiveChildRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<MembershipDTO> result = await Get<List<MembershipDTO>>(MEMBERSHIP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<MembershipContainerDTOCollection> GetMembershipContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            MembershipContainerDTOCollection result = await Get<MembershipContainerDTOCollection>(MEMBERSHIP_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
        public async Task<string> SaveAllMembership(List<MembershipDTO> membershipDTOList)
        {
            log.LogMethodEntry(membershipDTOList);
            try
            {
                string responseString = await Post<string>(MEMBERSHIP_CONTAINER_URL, membershipDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
