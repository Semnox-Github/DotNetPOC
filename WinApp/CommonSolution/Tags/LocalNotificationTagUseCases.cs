/********************************************************************************************
 * Project Name - Tags
 * Description  - LocalNotificationTagUseCases class 
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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    public class LocalNotificationTagUseCases : INotificationTagUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private readonly ExecutionContext executionContext;
        public LocalNotificationTagUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<NotificationTagsDTO>> GetNotificationTags(List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> searchParameters,
                                                                                 bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<NotificationTagsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                NotificationTagsListBL notificationTagListBL = new NotificationTagsListBL(executionContext);
                List<NotificationTagsDTO> notificationTagDTOList = notificationTagListBL.GetAllNotificationTagsList(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                log.LogMethodExit(notificationTagDTOList);
                return notificationTagDTOList;
            });
        }

        public async Task<List<string>> GetNotificationTagColumns()
        {
            return await Task<List<string>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry();
                NotificationTagsListBL notificationTagListBL = new NotificationTagsListBL(executionContext);
                List<string> result = notificationTagListBL.GetNotificationTagColumns();
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<List<NotificationTagViewDTO>> GetNotificationTagViewDTOList(List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<NotificationTagViewDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                NotificationTagViewDataHandler notificationTagViewDataHandler = new NotificationTagViewDataHandler(sqlTransaction);
                List<NotificationTagViewDTO> result = notificationTagViewDataHandler.GetNotificationTagViewDTOList(searchParameters);
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<string> SaveNotificationTags(List<NotificationTagsDTO> notificationTagDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(notificationTagDTOList);
                if (notificationTagDTOList == null)
                {
                    throw new ValidationException("notificationTagDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (NotificationTagsDTO notificationTagDTO in notificationTagDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            NotificationTagsBL notificationTagsBL = new NotificationTagsBL(executionContext, notificationTagDTO);
                            notificationTagsBL.Save(parafaitDBTrx.SQLTrx);
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

        public async Task<string> StorageInOutStatusChange(int tagId, bool isInStorage)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(tagId, isInStorage);
                if (tagId < 0)
                {
                    throw new ValidationException("Tag Id is not valid");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        NotificationTagsBL notificationTagsBL = new NotificationTagsBL(executionContext, tagId);
                        if (notificationTagsBL.GetNotificationTagsDTO !=  null)
                        {
                            notificationTagsBL.GetNotificationTagsDTO.IsInStorage = isInStorage;
                            notificationTagsBL.Save(parafaitDBTrx.SQLTrx);
                        }
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

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> NotificationStatusChange(int tagId, string notificationStatus)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(tagId, notificationStatus);
                if (tagId < 0)
                {
                    throw new ValidationException("Tag Id is not valid");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        notificationStatus.Replace(" ", "_");
                        TagNotificationStatus tagNotificationStatus = TagNotificationStatus.IN_USE;
                        try
                        {
                            tagNotificationStatus = (TagNotificationStatus)Enum.Parse(typeof(TagNotificationStatus), notificationStatus, true);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while parsing the OrderMessageStatus type", ex);
                            throw ex;
                        }
                        NotificationTagsBL notificationTagsBL = new NotificationTagsBL(executionContext, tagId);
                        if (notificationTagsBL.GetNotificationTagsDTO != null)
                        {
                            notificationTagsBL.GetNotificationTagsDTO.TagNotificationStatus = tagNotificationStatus.ToString();
                            notificationTagsBL.Save(parafaitDBTrx.SQLTrx);
                        }
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

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
