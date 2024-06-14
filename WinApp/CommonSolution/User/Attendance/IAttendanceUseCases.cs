/********************************************************************************************
* Project Name - User
* Description  - Interface for Attendance Controller.
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
    public interface IAttendanceUseCases
    {
        Task<List<AttendanceDTO>> GetAttendance(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction = null);
        Task<int> GetAttendanceCount(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<AttendanceDTO>> SaveAttendance(List<AttendanceDTO> AttendanceDTOList);
    }
}
