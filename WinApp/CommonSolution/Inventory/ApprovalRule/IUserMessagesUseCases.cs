/********************************************************************************************
 * Project Name - UserMessages
 * Description  - IUserMessagesUseCases Interface.
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     01-Jan-2021       Prajwal S                Created : POS UI Redesign with REST API
*2.110.0    18-Jan-2021       Mushahid Faizan           Web Inventory Changes with REST API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IUserMessagesUseCases
    {
        Task<List<UserMessagesDTO>> GetUserMessages(int roleId, int userId, string moduleType, string loginId, int siteId, string status, bool buildPendingApprovalUserMessage = false,
                                                    SqlTransaction sqlTransaction = null);
        Task<string> SaveUserMessages(List<UserMessagesDTO> userMessagesDTOList, SqlTransaction sqlTransaction = null);
        Task<int> GetUserMessagesCount(int roleId, int userId, string moduleType, string loginId, int siteId, string status,
                                    SqlTransaction sqlTransaction = null);
    }
}
