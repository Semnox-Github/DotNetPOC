/********************************************************************************************
* Project Name - User
* Description  - Interface for Shift Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     01-Apr-2021     Prajwal S             Created : Web Inventory UI Redesign with REST API
*2.140.0     14-Sep-2021     Deeksha               Modified : Provisional Shift changes
*2.140.0     16-Aug-2021     Girish                Modified : Multicash drawer changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Semnox.Parafait.User
{
    public interface IShiftUseCases
    {
        Task<List<ShiftDTO>> GetShift(List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParameters, bool loadChildRecords, bool buildReceipt = false, SqlTransaction sqlTransaction = null);
        Task<List<ShiftDTO>> SaveShift(List<ShiftDTO> shiftDTOList);
        Task<ShiftDTO> AssignCashdrawer(int shiftId, CashdrawerActivityDTO assignCashdrawerDTO);
        Task<ShiftDTO> UnAssignCashdrawer(int shiftId, CashdrawerActivityDTO assignCashdrawerDTO);
    }
}
