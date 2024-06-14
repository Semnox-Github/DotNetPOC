/********************************************************************************************
* Project Name - User
* Description  - Interface for DataAccessRule Controller.
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
    public interface IDataAccessRuleUseCases
    {
        Task<List<DataAccessRuleDTO>> GetDataAccessRule(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters, bool loadChildRecord = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null);
        Task<int> GetDataAccessRuleCount(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<DataAccessRuleDTO>> SaveDataAccessRule(List<DataAccessRuleDTO> dataAccessRuleDTOList);
        Task<List<EntityExclusionDetailDTO>> GetMaskUIFields(string uiName, SqlTransaction sqlTransaction = null);
    }
}
