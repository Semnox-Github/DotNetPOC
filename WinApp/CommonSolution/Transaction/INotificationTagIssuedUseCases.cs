/********************************************************************************************
 * Project Name - Transaction
 * Description  - INotificationTagIssuedUseCases
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.150.2     05-Dec-2022   Abhishek              Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
  public interface INotificationTagIssuedUseCases
    {
        /// <summary>
        /// GetNotificationTagIssued Records
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns></returns>
        Task<List<NotificationTagIssuedDTO>> GetNotificationTagIssued(List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        /// <summary>
        /// SaveNotificationTagIssued Records
        /// </summary>
        /// <param name="notificationTagIssuedDTOList">notificationTagIssuedDTOList</param>
        /// <returns></returns>
        Task<List<NotificationTagIssuedDTO>> SaveNotificationTagIssued(List<NotificationTagIssuedDTO> notificationTagIssuedDTOList);
        Task<NotificationTagIssuedDTO> SaveNotificationTagIssuedTime(int notificationTagIssuedId, NotificationTagIssuedDTO notificationTagIssuedDTO);
    }
}
