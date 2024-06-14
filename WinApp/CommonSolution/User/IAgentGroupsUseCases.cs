/********************************************************************************************
* Project Name - User
* Description  - Interface for AgentGroups Controller.
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
    public interface IAgentGroupsUseCases
    {
        Task<List<AgentGroupsDTO>> GetAgentGroups(List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords, bool activeChildRecords,  SqlTransaction sqlTransaction = null);
        Task<string> SaveAgentGroups(List<AgentGroupsDTO> agentGroupsDTOList);
        Task<string> DeleteAgentGroups(List<AgentGroupsDTO> agentGroupsDTOList);
    }
}
