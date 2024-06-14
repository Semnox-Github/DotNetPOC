/********************************************************************************************
 * Project Name - Transaction
 * Description  - LocalNotificationTagProfile UseCases class 
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
   public class LocalNotificationTagProfileUseCases:INotificationTagProfileUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;

        /// <summary>
        /// LocalNotificationTagProfileUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalNotificationTagProfileUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetNotificationTagProfiles
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public async Task<List<NotificationTagProfileDTO>> GetNotificationTagProfiles(List<KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<NotificationTagProfileDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                NotificationTagProfileListBL notificationTagProfileListBL = new NotificationTagProfileListBL(executionContext);
                List<NotificationTagProfileDTO> notificationTagProfileDTOList = notificationTagProfileListBL.GetAllNotificationTagProfileList(searchParameters);

                log.LogMethodExit(notificationTagProfileDTOList);
                return notificationTagProfileDTOList;
            });
        }
        /// <summary>
        /// SaveNotificationTagProfiles
        /// </summary>
        /// <param name="notificationTagProfileDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveNotificationTagProfiles(List<NotificationTagProfileDTO> notificationTagProfileDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(notificationTagProfileDTOList);
                if (notificationTagProfileDTOList == null)
                {
                    throw new ValidationException("notificationTagProfileDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (NotificationTagProfileDTO notificationTagProfileDTO in notificationTagProfileDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            NotificationTagProfileBL notificationTagProfileBL = new NotificationTagProfileBL(executionContext, notificationTagProfileDTO);
                            notificationTagProfileBL.Save(parafaitDBTrx.SQLTrx);
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
