/********************************************************************************************
 * Project Name - Transaction
 * Description  - LocalNotificationTagManualEventUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    12-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
   public class LocalNotificationTagManualEventUseCases:INotificationTagManualEventUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalNotificationTagManualEventUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<NotificationTagManualEventsDTO>> GetNotificationTagManualEvents(List<KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>>
                         searchParameters)
        {
            return await Task<List<NotificationTagManualEventsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                NotificationTagManualEventsListBL notificationTagManualEventsListBL = new NotificationTagManualEventsListBL(executionContext);
                List<NotificationTagManualEventsDTO> notificationTagManualEventDTOList = notificationTagManualEventsListBL.GetAllNotificationTagManualEventsDTOList(searchParameters);

                log.LogMethodExit(notificationTagManualEventDTOList);
                return notificationTagManualEventDTOList;
            });
        }
        public async Task<string> SaveNotificationTagManualEvents(List<NotificationTagManualEventsDTO> notificationTagManualEventDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(notificationTagManualEventDTOList);
                if (notificationTagManualEventDTOList == null)
                {
                    throw new ValidationException("notificationTagManualEventDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (NotificationTagManualEventsDTO notificationTagManualEventDTO in notificationTagManualEventDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            NotificationTagManualEventsBL notificationTagManualEventsBL = new NotificationTagManualEventsBL(executionContext, notificationTagManualEventDTO);
                            notificationTagManualEventsBL.Save(parafaitDBTrx.SQLTrx);
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

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
