/********************************************************************************************
* Project Name - User
* Description  - Interface for PayConfigurations Controller.
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
    public interface IPayConfigurationsUseCases
    {
        Task<List<PayConfigurationsDTO>> GetPayConfigurations(List<KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null);
        //  Task<int> GetPayConfigurationCount(List<KeyValuePair<PayConfigurationDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<PayConfigurationsDTO>> SavePayConfigurations(List<PayConfigurationsDTO> payConfigurationsDTOList);
    }
}
