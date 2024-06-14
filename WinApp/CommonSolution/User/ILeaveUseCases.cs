/********************************************************************************************
* Project Name - User
* Description  - Interface for Leave Controller.
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
    public interface ILeaveUseCases
    {
        Task<List<LeaveDTO>> GetLeave(List<KeyValuePair<LeaveDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        //  Task<int> GetLeaveCount(List<KeyValuePair<LeaveDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<LeaveDTO>> SaveLeave(List<LeaveDTO> leaveDTOList);
        Task<string> DeleteLeave(List<LeaveDTO> leaveDTOList);

        Task<List<LeaveDTO>> GenerateLeave(int leaveCycleId = -1);

        Task<List<LeaveDTO>> PopulateInbox(int userId = -1);
    }
}
