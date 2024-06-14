/********************************************************************************************
* Project Name - Utilities
* Description  - Specification of the user use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.110.0     12-Nov-2019   Lakshminarayana         Created 
********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Specification of the user use cases
    /// </summary>
    public interface IUserUseCases
    {
        Task<List<UsersDTO>> GetUserDTOList(List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = true);
        Task<List<UsersDTO>> SaveUsersDTOList(List<UsersDTO> usersDTOList);
        Task<UserContainerDTOCollection> GetUserContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<UserIdentificationTagsDTO> UpdateUserIdentificationTagStatus(int userId, int tagId, UserIdentificationTagsDTO userIdentificationTagsDTO);
        Task RecordAttendance(int userId, AttendanceLogDTO attendanceLogDTO, SqlTransaction sqlTransaction);
        Task<List<UserIdentificationTagsDTO>> GetUserIdentificationTagsDTOList(List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> parameters);

        Task<List<ManagementFormAccessContainerDTO>> GetUserManagementFormAccess();
    }
}
