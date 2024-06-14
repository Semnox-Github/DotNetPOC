/**************************************************************************************************
 * Project Name - Tags 
 * Description  - RemoteNotificationTagStatusUseCases Class
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
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Tags
{
    /// <summary>
    /// RemoteNotificationTagStatusUseCases
    /// </summary>
    public class RemoteNotificationTagStatusUseCases : RemoteUseCases,INotificationTagStatusUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// RemoteCommunicationLogUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteNotificationTagStatusUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// SaveNotificationTagStatus
        /// </summary>
        /// <param name="notificationTagId">notificationTagId</param>
        /// <param name="notificationTagStatusDTOList">notificationTagStatusDTOList</param>
        /// <returns>communicationLogDTOList</returns>
        public async Task<List<NotificationTagStatusDTO>> SaveNotificationTagStatus(int notificationTagId, List<NotificationTagStatusDTO> notificationTagStatusDTOList)
        {
            log.LogMethodEntry(notificationTagId, notificationTagStatusDTOList);
            string NOTIFICATION_TAG__STATUS_URL = "api/Tag/NotificationTag/{"+ notificationTagId +"}/StatusLog";
            try
            {
                List<NotificationTagStatusDTO> responseData = await Post<List<NotificationTagStatusDTO>>(NOTIFICATION_TAG__STATUS_URL, notificationTagStatusDTOList);
                log.LogMethodExit(responseData);
                return responseData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
