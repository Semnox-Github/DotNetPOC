/********************************************************************************************
* Project Name - User
* Description  - Interface for Agents Controller.
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
    public interface IAgentsUseCases
    {
        Task<List<AgentsDTO>> GetAgents(List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveAgents(List<AgentsDTO> agentsDTOList);
        Task<string> DeleteAgents(List<AgentsDTO> agentsDTOList);
    }
}
