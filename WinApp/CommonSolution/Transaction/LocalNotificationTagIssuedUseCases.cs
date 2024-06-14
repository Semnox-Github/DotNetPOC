/********************************************************************************************
 * Project Name - ServerCore
 * Description  - IAdBroadcastUseCases
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
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// LocalNotificationTagIssuedUseCases
    /// </summary>
    public class LocalNotificationTagIssuedUseCases : INotificationTagIssuedUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// LocalNotificationTagIssuedUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalNotificationTagIssuedUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetNotificationTagManualEvents
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>notificationTagIssuedDTOList</returns>
        public async Task<List<NotificationTagIssuedDTO>> GetNotificationTagIssued(List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<NotificationTagIssuedDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(sqlTransaction);
                NotificationTagIssuedListBL notificationTagIssuedListBL = new NotificationTagIssuedListBL(executionContext);
                List<NotificationTagIssuedDTO> notificationTagIssuedDTOList = notificationTagIssuedListBL.GetAllNotificationTagIssuedDTOList(searchParameters, sqlTransaction);
                log.LogMethodExit(notificationTagIssuedDTOList);
                return notificationTagIssuedDTOList;
            });
        }

        /// <summary>
        /// SaveNotificationTagIssued
        /// </summary>
        /// <param name="notificationTagIssuedDTOList"></param>
        /// <returns></returns>
        public async Task<List<NotificationTagIssuedDTO>> SaveNotificationTagIssued(List<NotificationTagIssuedDTO> notificationTagIssuedDTOList)
        {
            return await Task<List<NotificationTagIssuedDTO>>.Factory.StartNew(() =>
            {
                List<NotificationTagIssuedDTO> result = new List<NotificationTagIssuedDTO>();
                log.LogMethodEntry(notificationTagIssuedDTOList);
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (NotificationTagIssuedDTO notificationTagIssuedDTO in notificationTagIssuedDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            NotificationTagIssuedBL notificationTagIssuedBL = new NotificationTagIssuedBL(executionContext, notificationTagIssuedDTO);
                            notificationTagIssuedBL.Save(parafaitDBTrx.SQLTrx);
                            result.Add(notificationTagIssuedBL.GetNotificationTagIssuedDTO);
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

        /// <summary>
        /// SaveNotificationTagIssuedTime
        /// </summary>
        /// <param name="notificationTagIssuedDTO"></param>
        /// <returns></returns>
        public async Task<NotificationTagIssuedDTO> SaveNotificationTagIssuedTime(int notificationTagIssuedId, NotificationTagIssuedDTO notificationTagIssuedDTO)
        {
            return await Task<NotificationTagIssuedDTO>.Factory.StartNew(() =>
            {
                NotificationTagIssuedDTO result = new NotificationTagIssuedDTO();
                log.LogMethodEntry(notificationTagIssuedId, notificationTagIssuedDTO);
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        NotificationTagIssuedBL notificationTagIssuedBL = new NotificationTagIssuedBL(executionContext, notificationTagIssuedId);
                        NotificationTagIssuedDTO saveNotificationTagIssuedDTO = notificationTagIssuedBL.GetNotificationTagIssuedDTO;
                        if (saveNotificationTagIssuedDTO != null)
                        {
                            saveNotificationTagIssuedDTO.LastSessionAlertTime = notificationTagIssuedDTO.LastSessionAlertTime;
                            saveNotificationTagIssuedDTO.LastAlertTimeOnExpiry = notificationTagIssuedDTO.LastAlertTimeOnExpiry;
                            saveNotificationTagIssuedDTO.LastAlertTimeBeforeExpiry = notificationTagIssuedDTO.LastAlertTimeBeforeExpiry;
                            notificationTagIssuedBL = new NotificationTagIssuedBL(executionContext, saveNotificationTagIssuedDTO);
                            notificationTagIssuedBL.Save(parafaitDBTrx.SQLTrx);
                        }
                        result = notificationTagIssuedBL.GetNotificationTagIssuedDTO;
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
                log.LogMethodExit(result);
                return result;
            });
        }


        //public async Task<NotificationTagIssuedDTO> SaveNotificationTagIssuedStatus(int notificationTagIssuedId, NotificationTagIssuedDTO notificationTagIssuedDTO)
        //{
        //    return await Task<NotificationTagIssuedDTO>.Factory.StartNew(() =>
        //    {
        //        NotificationTagIssuedDTO result = null;
        //        log.LogMethodEntry(notificationTagIssuedDTO);
        //        if (notificationTagIssuedDTO == null)
        //        {
        //            throw new ValidationException("notificationTagIssuedDTO is Empty");
        //        }
        //        NotificationTagIssuedBL notificationTagIssuedBL = new NotificationTagIssuedBL(executionContext, notificationTagIssuedId);
        //        if (notificationTagIssuedBL.GetNotificationTagIssuedDTO == null)
        //        {
        //            string message = MessageContainerList.GetMessage(executionContext, "Notification Tag Issued Not Found");
        //            log.Error(message);
        //            throw new ValidationException(message);
        //        }
        //        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
        //        {
        //            try
        //            {
        //                parafaitDBTrx.BeginTransaction();
        //                notificationTagIssuedBL = new NotificationTagIssuedBL(executionContext, notificationTagIssuedDTO);
        //                notificationTagIssuedBL.Save(parafaitDBTrx.SQLTrx);
        //                parafaitDBTrx.EndTransaction();
        //            }
        //            catch (ValidationException valEx)
        //            {
        //                parafaitDBTrx.RollBack();
        //                log.Error(valEx);
        //                throw valEx;
        //            }
        //            catch (Exception ex)
        //            {
        //                parafaitDBTrx.RollBack();
        //                log.Error(ex);
        //                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
        //                throw ex;
        //            }
        //        }
        //        log.LogMethodExit(result);
        //        return result;
        //    });
        //}
    }
}
