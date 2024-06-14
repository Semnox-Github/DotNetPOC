/********************************************************************************************
* Project Name - User
* Description  - Interface for AttendanceRole Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     01-Apr-2021     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public interface IAttendanceRoleUseCases
    {
        Task<List<AttendanceRoleDTO>> GetAttendanceRole(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<int> GetAttendanceRoleCount(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<AttendanceRoleDTO>> SaveAttendanceRole(List<AttendanceRoleDTO> attendanceRoleDTOList);
        Task<string> DeleteAttendanceRole(List<AttendanceRoleDTO> attendanceRoleDTOList);
    }
}
