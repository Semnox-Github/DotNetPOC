using System;
/********************************************************************************************
* Project Name - User
* Description  - Interface for ShiftConfigurations Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     01-Apr-2021     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public interface IShiftConfigurationsUseCases
    {
        Task<List<ShiftConfigurationsDTO>> GetShiftConfigurations(List<KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        //  Task<int> GetPayConfigurationCount(List<KeyValuePair<PayConfigurationDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<ShiftConfigurationsDTO>> SaveShiftConfigurations(List<ShiftConfigurationsDTO> shiftConfigurationsDTOList);
    }
}
