/********************************************************************************************
* Project Name - Tags
* Description  - Specification of the NotificationTag use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   12-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    public interface INotificationTagUseCases
    {
        Task<List<NotificationTagsDTO>> GetNotificationTags(List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> searchParameters,
                                                                                 bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null);
        Task<string> SaveNotificationTags(List<NotificationTagsDTO> notificationTagDTOList);
        Task<string> StorageInOutStatusChange(int tagId , bool isInStorage);
        Task<string> NotificationStatusChange(int tagId , string NotificationStatus);
        Task<List<string>> GetNotificationTagColumns();

        Task<List<NotificationTagViewDTO>> GetNotificationTagViewDTOList(List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
    }
}
