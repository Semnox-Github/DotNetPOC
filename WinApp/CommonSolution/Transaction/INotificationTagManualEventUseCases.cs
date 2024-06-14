/********************************************************************************************
* Project Name - Transcation
* Description  - Specification of the NotificationTagManualEvent use cases. 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
  public interface INotificationTagManualEventUseCases
    {
        Task<List<NotificationTagManualEventsDTO>> GetNotificationTagManualEvents(List<KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveNotificationTagManualEvents(List<NotificationTagManualEventsDTO> notificationTagManualEventDTOList);
    }
}
