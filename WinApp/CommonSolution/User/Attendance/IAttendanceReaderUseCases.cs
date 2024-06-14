/********************************************************************************************
* Project Name - User
* Description  - Interface for AttendanceReader Controller.
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
    public interface IAttendanceReaderUseCases
    {
        Task<List<AttendanceReaderDTO>> GetAttendanceReader(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<int> GetAttendanceReaderCount(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<AttendanceReaderDTO>> SaveAttendanceReader(List<AttendanceReaderDTO> AttendanceReaderDTOList);
        Task<string> DeleteAttendanceReader(List<AttendanceReaderDTO> AttendanceReaderDTOList);
    }
}
