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
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public interface IShiftUseCases
    {
        Task<List<ShiftDTO>> GetShift(List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParameters, bool loadChildRecords, bool buildReceipt = false, SqlTransaction sqlTransaction = null);
        //  Task<int> GetPayConfigurationCount(List<KeyValuePair<PayConfigurationDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<ShiftDTO>> SaveShift(List<ShiftDTO> shiftDTOList);
    }
}
