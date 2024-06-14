/**************************************************************************************************
 * Project Name - Tags 
 * Description  - INotificationTagStatusUseCases Class
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.150.2     28-Nov-2022       Abhishek                  Created - Game Server Cloud Movement.
 **************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    public interface INotificationTagStatusUseCases
    {
        /// <summary>
        /// SaveNotificationTagStatus
        /// </summary>
        /// <param name="notificationTagId">notificationTagId</param>
        /// <param name="notificationTagStatusDTOList">notificationTagStatusDTOList</param>
        /// <returns></returns>
        Task<List<NotificationTagStatusDTO>> SaveNotificationTagStatus(int notificationTagId, List<NotificationTagStatusDTO> notificationTagStatusDTOList);
    }
}
