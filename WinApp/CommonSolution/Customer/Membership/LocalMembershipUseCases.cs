/********************************************************************************************
 * Project Name - Customer 
 * Description  - LocalMembershipUseCases class to get the data  from local DB 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
  2.120.0      07-May-2021      B Mahesh Pai              SaveAllMembership code added
 2.130.3      16-Dec-2021       Abhishek                 WMS fix : Added two parameters loadChildRecords,loadActiveChildRecords
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Membership.Sample;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Membership
{
    public class LocalMembershipUseCases : LocalUseCases, IMembershipUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMembershipUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<MembershipDTO>> GetAllMemberships(List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<MembershipDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                MembershipsList membershipsList = new MembershipsList(executionContext);
                List<MembershipDTO> membershipDTOList = membershipsList.GetAllMemberships(searchParameters, loadChildRecords, loadActiveChildRecords, sqlTransaction);
                log.LogMethodExit(membershipDTOList);
                return membershipDTOList;
            });

        }

        public async Task<MembershipContainerDTOCollection> GetMembershipContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<MembershipContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    MembershipContainerList.Rebuild(siteId);
                }
                List<MembershipContainerDTO> membershipContainerList = MembershipContainerList.GetMembershipContainerDTOList(siteId);
                MembershipContainerDTOCollection result = new MembershipContainerDTOCollection(membershipContainerList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<string> SaveAllMembership(List<MembershipDTO> membershipDTOList)
        {
            log.LogMethodEntry(membershipDTOList);
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                if (membershipDTOList == null)
                {
                    throw new ValidationException("membershipDTOList is empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        MembershipsList membershipListBL = new MembershipsList(membershipDTOList, executionContext);
                        membershipListBL.SaveUpdateMembership();
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ex;
                    }
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
