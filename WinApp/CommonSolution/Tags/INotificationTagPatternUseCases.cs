/********************************************************************************************
* Project Name - Tags
* Description  - Specification of the NotificationTagPattern use cases. 
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

namespace Semnox.Parafait.Tags
{
    public interface INotificationTagPatternUseCases
    {
        Task<List<NotificationTagPatternDTO>> GetNotificationTagPatterns(List<KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveNotificationTagPatterns(List<NotificationTagPatternDTO> notificationTagPatternDTOList);
    }
}
