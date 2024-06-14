/********************************************************************************************
 * Project Name - Tags
 * Description  - LocalNotificationTagPattern UseCases class
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 2.120.00    12-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    public class LocalNotificationTagPatternUseCases:INotificationTagPatternUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalNotificationTagPatternUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<NotificationTagPatternDTO>> GetNotificationTagPatterns(List<KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>>
                         searchParameters)
        {
            return await Task<List<NotificationTagPatternDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                NotificationTagPatternListBL notificationTagPatternListBL = new NotificationTagPatternListBL(executionContext);
                List<NotificationTagPatternDTO> notificationTagPatternDTOList = notificationTagPatternListBL.GetAllNotificationTagPatternList(searchParameters);

                log.LogMethodExit(notificationTagPatternDTOList);
                return notificationTagPatternDTOList;
            });
        }
        public async Task<string> SaveNotificationTagPatterns(List<NotificationTagPatternDTO> notificationTagPatternDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(notificationTagPatternDTOList);
                if (notificationTagPatternDTOList == null)
                {
                    throw new ValidationException("notificationTagPatternDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (NotificationTagPatternDTO notificationTagPatternDTO in notificationTagPatternDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            NotificationTagPatternBL notificationTagPatternBL = new NotificationTagPatternBL(executionContext, notificationTagPatternDTO);
                            notificationTagPatternBL.Save(parafaitDBTrx.SQLTrx);
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
