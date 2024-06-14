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
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Tags
{
    /// <summary>
    /// LocalNotificationTagStatusUseCases
    /// </summary>
    public class LocalNotificationTagStatusUseCases : INotificationTagStatusUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        // <summary>
        /// LocalNotificationTagStatusUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalNotificationTagStatusUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        // <summary>
        /// SaveNotificationTagStatus
        /// </summary>
        /// <param name="notificationTagId">notificationTagId</param>
        /// <param name="notificationTagStatusDTOList">notificationTagStatusDTOList</param>
        /// <returns>notificationTagStatusDTOList</returns>
        public async Task<List<NotificationTagStatusDTO>> SaveNotificationTagStatus(int notificationTagId, List<NotificationTagStatusDTO> notificationTagStatusDTOList)
        {
            return await Task<List<NotificationTagStatusDTO>>.Factory.StartNew(() =>
            {
                List<NotificationTagStatusDTO> result = new List<NotificationTagStatusDTO>();
                log.LogMethodEntry(notificationTagId, notificationTagStatusDTOList);
                NotificationTagsBL notificationTagsBL = new NotificationTagsBL(executionContext, notificationTagId);
                if (notificationTagsBL.GetNotificationTagsDTO == null)
                {
                    string message = MessageContainerList.GetMessage(executionContext, "NotificationTag not found");
                    log.Error(message);
                    throw new ValidationException(message);
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (NotificationTagStatusDTO notificationTagStatusDTO in notificationTagStatusDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            NotificationTagStatusBL notificationTagPatternBL = new NotificationTagStatusBL(executionContext, notificationTagStatusDTO);
                            notificationTagPatternBL.Save(parafaitDBTrx.SQLTrx);
                            result.Add(notificationTagPatternBL.GetNotificationTagStatusDTO);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
