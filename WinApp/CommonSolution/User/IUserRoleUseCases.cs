/********************************************************************************************
 * Project Name - User  
 * Description  - Specification of the user role use cases  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Specification of the user role use cases 
    /// </summary>
    public interface IUserRoleUseCases
    {
        Task<List<UserRolesDTO>> GetUserRoles(List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchParameters, bool loadChildRecords, bool activeChildRecords, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null);
        // Task<int> GetUserRoleCount(List<KeyValuePair<UserRoleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<UserRolesDTO>> SaveUserRoles(List<UserRolesDTO> userRolesDTOList);
        Task<UserRoleContainerDTOCollection> GetUserRoleContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
