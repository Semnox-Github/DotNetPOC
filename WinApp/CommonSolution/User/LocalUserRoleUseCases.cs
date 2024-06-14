/********************************************************************************************
 * Project Name - User 
 * Description  - Implementation of the user use cases 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 2.120.0      01-Apr-2021      Prajwal S                 Modified : Added Get and save.
 ********************************************************************************************/

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Implementation of the user use cases 
    /// </summary>
    public class LocalUserRoleUseCases : LocalUseCases, IUserRoleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalUserRoleUseCases(ExecutionContext executionContext) 
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<UserRolesDTO>> GetUserRoles(List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>
                      searchParameters,bool loadChildRecords, bool activeChildRecords, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null
                     )
        {
            return await Task<List<UserRolesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                UserRolesList userRolessListBL = new UserRolesList(executionContext);
                List<UserRolesDTO> userRolesDTOList = userRolessListBL.GetAllUserRoles(searchParameters, loadChildRecords, activeChildRecords,/*currentPage, pageSize,*/ sqlTransaction);

                log.LogMethodExit(userRolesDTOList);
                return userRolesDTOList;
            });
        }

        //public async Task<int> GetUserRolesCount(List<KeyValuePair<UserRolesDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        UserRolesListBL UserRolessListBL = new UserRolesListBL(executionContext);
        //        int count = UserRolessListBL.GetUserRolesCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<UserRolesDTO>> SaveUserRoles(List<UserRolesDTO> userRolesDTOList)
        {
            return await Task<List<UserRolesDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    UserRolesList userRolesList = new UserRolesList(executionContext, userRolesDTOList);
                    List<UserRolesDTO> result = userRolesList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
        public async Task<UserRoleContainerDTOCollection> GetUserRoleContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<UserRoleContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    UserRoleContainerList.Rebuild(siteId);
                }
                UserRoleContainerDTOCollection result = UserRoleContainerList.GetUserRoleContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });

        }

    }
}
