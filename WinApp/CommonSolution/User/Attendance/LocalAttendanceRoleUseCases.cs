/********************************************************************************************
 * Project Name - User
 * Description  - LocalAttendanceRoleUseCases class 
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
    class LocalAttendanceRoleUseCases : IAttendanceRoleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalAttendanceRoleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<AttendanceRoleDTO>> GetAttendanceRole(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<AttendanceRoleDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AttendanceRolesList attendanceRolesListBL = new AttendanceRolesList(executionContext);
                List<AttendanceRoleDTO> attendanceRoleDTOList = attendanceRolesListBL.GetAttendanceRoles(searchParameters, sqlTransaction);

                log.LogMethodExit(attendanceRoleDTOList);
                return attendanceRoleDTOList;
            });
        }

        public async Task<int> GetAttendanceRoleCount(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>>
                                                      searchParameters, SqlTransaction sqlTransaction = null
                             )
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AttendanceRolesList attendanceRolesListBL = new AttendanceRolesList(executionContext);
                int count = attendanceRolesListBL.GetAttendanceRolesCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<List<AttendanceRoleDTO>> SaveAttendanceRole(List<AttendanceRoleDTO> attendanceRoleDTOList)
        {
            return await Task<List<AttendanceRoleDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    AttendanceRolesList attendanceRolesList = new AttendanceRolesList(executionContext, attendanceRoleDTOList);
                    List<AttendanceRoleDTO> result = attendanceRolesList.SaveUpdateAttendanceRolesList();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }

        public async Task<string> DeleteAttendanceRole(List<AttendanceRoleDTO> attendanceRoleDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(attendanceRoleDTOList);
                    AttendanceRolesList attendanceRolesList = new AttendanceRolesList(executionContext, attendanceRoleDTOList);
                    if (attendanceRoleDTOList.Any(x => x.IsActive == false))
                    {
                        attendanceRolesList.SaveUpdateAttendanceRolesList();
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }

    }
}
