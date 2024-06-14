/********************************************************************************************
 * Project Name - User
 * Description  - LocalUserToAttendanceRolesMapUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      01-Apr-2021      Prajwal S                 Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    class LocalUserToAttendanceRolesMapUseCases : IUserToAttendanceRolesMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalUserToAttendanceRolesMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<UserToAttendanceRolesMapDTO>> GetUserToAttendanceRolesMap(List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<UserToAttendanceRolesMapDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                UserToAttendanceRolesMapListBL userToAttendanceRolesMapListBL = new UserToAttendanceRolesMapListBL(executionContext);
                List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList = userToAttendanceRolesMapListBL.GetUserToAttendanceRolesMapDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(userToAttendanceRolesMapDTOList);
                return userToAttendanceRolesMapDTOList;
            });
        }

        //public async Task<int> GetUserToAttendanceRolesMapCount(List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        UserToAttendanceRolesMapListBL UserToAttendanceRolesMapsListBL = new UserToAttendanceRolesMapListBL(executionContext);
        //        int count = UserToAttendanceRolesMapsListBL.GetUserToAttendanceRolesMapCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<UserToAttendanceRolesMapDTO>> SaveUserToAttendanceRolesMap(List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList)
        {
            return await Task<List<UserToAttendanceRolesMapDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    UserToAttendanceRolesMapListBL userToAttendanceRolesMapList = new UserToAttendanceRolesMapListBL(executionContext, userToAttendanceRolesMapDTOList);
                    List<UserToAttendanceRolesMapDTO> result = userToAttendanceRolesMapList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    }
}
