/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business Logic of the NotificationTags class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        20-jul-2020   Mushahid Faizan         Created.
 *2.110.2     26-Mar-2021   Mushahid Faizan         Added Excel sheet functionality..
 *2.120.2     01-May-2021   Mushahid Faizan         WMS UI changes
 *2.130.0     31-Aug-2021   Guru S A                Enable Serial number based card load
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Tags
{
    public class NotificationTagsBL
    {
        private NotificationTagsDTO notificationTagsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private NotificationTagsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        ///<summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagsDTO"></param>
        public NotificationTagsBL(ExecutionContext executionContext, NotificationTagsDTO notificationTagsDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagsDTO);
            this.executionContext = executionContext;
            this.notificationTagsDTO = notificationTagsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the NotificationTagId as the parameter
        /// Would fetch the notificationTagsDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="NotificationTagId">Id</param>
        public NotificationTagsBL(ExecutionContext executionContext, int NotificationTagId, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, NotificationTagId, loadChildRecords, activeChildRecords, sqlTransaction);
            NotificationTagsDataHandler notificationTagsDataHandler = new NotificationTagsDataHandler(sqlTransaction);
            notificationTagsDTO = notificationTagsDataHandler.GetNotificationTagsDTO(NotificationTagId);
            if (notificationTagsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "NotificationTags", NotificationTagId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                NotificationTagStatusListBL notificationTagStatusListBL = new NotificationTagStatusListBL(executionContext);
                notificationTagsDTO.NotificationTagStatusDTOList = notificationTagStatusListBL.GetNotificationTagStatusDTOList(new List<int> { NotificationTagId }, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the notificationTagsDTO
        /// asset will be inserted if NotificationTagId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            NotificationTagsDataHandler notificationTagsDataHandler = new NotificationTagsDataHandler(sqlTransaction);
            if (notificationTagsDTO.IsChangedRecursive == false &&
                notificationTagsDTO.NotificationTagId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (notificationTagsDTO.NotificationTagId < 0)
            {
                notificationTagsDTO = notificationTagsDataHandler.InsertNotificationTags(notificationTagsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagsDTO.AcceptChanges();
            }
            else if (notificationTagsDTO.IsChanged)
            {
                notificationTagsDTO = notificationTagsDataHandler.UpdateNotificationTags(notificationTagsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagsDTO.AcceptChanges();
            }
            SaveNotificationTagStatus(sqlTransaction);
            notificationTagsDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : NotificationTagStatusDTOList 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveNotificationTagStatus(SqlTransaction sqlTransaction)
        {
            if (notificationTagsDTO.NotificationTagStatusDTOList != null &&
                notificationTagsDTO.NotificationTagStatusDTOList.Any())
            {
                List<NotificationTagStatusDTO> updatedNotificationTagStatusList = new List<NotificationTagStatusDTO>();
                foreach (var notificationTagStatusDTO in notificationTagsDTO.NotificationTagStatusDTOList)
                {
                    if (notificationTagStatusDTO.NotificationTagId != notificationTagsDTO.NotificationTagId)
                    {
                        notificationTagStatusDTO.NotificationTagId = notificationTagsDTO.NotificationTagId;
                    }
                    if (notificationTagStatusDTO.IsChanged)
                    {
                        updatedNotificationTagStatusList.Add(notificationTagStatusDTO);
                    }
                }
                if (updatedNotificationTagStatusList.Any())
                {
                    NotificationTagStatusListBL notificationTagStatusListBL = new NotificationTagStatusListBL(executionContext, updatedNotificationTagStatusList);
                    notificationTagStatusListBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validate the notificationTagsDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();

            NotificationTagsDataHandler notificationTagsDataHandler = new NotificationTagsDataHandler(sqlTransaction);
            List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<NotificationTagsDTO> notificationTagsDTOList = notificationTagsDataHandler.GetNotificationTagsDTOList(searchParameters);

            if (notificationTagsDTOList != null && notificationTagsDTOList.Any())
            {
                if (notificationTagsDTO.NotificationTagId < 0)
                {
                    if (notificationTagsDTOList.Exists(x => x.TagNumber.ToLower() == notificationTagsDTO.TagNumber.ToLower()) && notificationTagsDTO.NotificationTagId == -1)
                    {
                        log.Debug("Duplicate entries detail");
                        validationErrorList.Add(new ValidationError("NotificationTag", "TagNumber", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "TagNumber"))));
                    }
                }
                if (notificationTagsDTO.NotificationTagId > 0)
                {
                    if (notificationTagsDTOList.Exists(x => x.TagNumber.ToLower() == notificationTagsDTO.TagNumber.ToLower() && x.NotificationTagId != notificationTagsDTO.NotificationTagId))
                    {
                        log.Debug("Duplicate Update detail");
                        validationErrorList.Add(new ValidationError("NotificationTag", "TagNumber", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "TagNumber"))));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// get NotificationTagsDTO Object
        /// </summary>
        public NotificationTagsDTO GetNotificationTagsDTO
        {
            get { return notificationTagsDTO; }
        }

        /// <summary>
        /// set NotificationTagsDTO Object        
        /// </summary>
        public NotificationTagsDTO SetNotificationTagsDTO
        {
            set { notificationTagsDTO = value; }
        }
    }

    /// <summary>
    /// Manages the list of NotificationTagsBL
    /// </summary>
    public class NotificationTagsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<NotificationTagsDTO> notificationTagsDTOList = new List<NotificationTagsDTO>();
        private ExecutionContext executionContext;
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public NotificationTagsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagsDTO"></param>
        public NotificationTagsListBL(ExecutionContext executionContext, List<NotificationTagsDTO> notificationTagsDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagsDTOList);
            this.notificationTagsDTOList = notificationTagsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the NotificationTagsDTO  list
        /// </summary>
        public List<NotificationTagsDTO> GetAllNotificationTagsList(List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> searchParameters,
                                                                                 bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            NotificationTagsDataHandler notificationTagsDataHandler = new NotificationTagsDataHandler(sqlTransaction);
            this.notificationTagsDTOList = notificationTagsDataHandler.GetNotificationTagsDTOList(searchParameters);
            if (notificationTagsDTOList != null && notificationTagsDTOList.Any() && loadChildRecords)
            {
                Build(notificationTagsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(notificationTagsDTOList);
            return notificationTagsDTOList;
        }

        private void Build(List<NotificationTagsDTO> notificationTagsDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, NotificationTagsDTO> notificationTagsDTODictionary = new Dictionary<int, NotificationTagsDTO>();
            List<int> notificationTagsIdList = new List<int>();
            for (int i = 0; i < notificationTagsDTOList.Count; i++)
            {
                if (notificationTagsDTODictionary.ContainsKey(notificationTagsDTOList[i].NotificationTagId))
                {
                    continue;
                }
                notificationTagsDTODictionary.Add(notificationTagsDTOList[i].NotificationTagId, notificationTagsDTOList[i]);
                notificationTagsIdList.Add(notificationTagsDTOList[i].NotificationTagId);
            }
            NotificationTagStatusListBL notificationTagStatusListBL = new NotificationTagStatusListBL(executionContext);
            List<NotificationTagStatusDTO> notificationTagStatusDTOList = notificationTagStatusListBL.GetNotificationTagStatusDTOList(notificationTagsIdList, activeChildRecords, sqlTransaction);

            if (notificationTagStatusDTOList != null && notificationTagStatusDTOList.Any())
            {
                for (int i = 0; i < notificationTagStatusDTOList.Count; i++)
                {
                    if (notificationTagsDTODictionary.ContainsKey(notificationTagStatusDTOList[i].NotificationTagId) == false)
                    {
                        continue;
                    }
                    NotificationTagsDTO notificationTagsDTO = notificationTagsDTODictionary[notificationTagStatusDTOList[i].NotificationTagId];
                    if (notificationTagsDTO.NotificationTagStatusDTOList == null)
                    {
                        notificationTagsDTO.NotificationTagStatusDTOList = new List<NotificationTagStatusDTO>();
                    }
                    notificationTagsDTO.NotificationTagStatusDTOList.Add(notificationTagStatusDTOList[i]);
                }
            }
        }

        /// <summary>
        /// Saves the NotificationTagsDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (notificationTagsDTOList == null ||
                notificationTagsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < notificationTagsDTOList.Count; i++)
            {
                var notificationTagsDTO = notificationTagsDTOList[i];
                if (notificationTagsDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    NotificationTagsBL notificationTagsBL = new NotificationTagsBL(executionContext, notificationTagsDTO);
                    notificationTagsBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving notificationTagsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("notificationTagsDTO", notificationTagsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This method is will return Sheet object for NotificationTagsDTO.
        /// <returns></returns>
        public Sheet BuildTemplate(bool buildTemplate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            NotificationTagExcelDTODefinition notificatinTagExcelDTODefinition = new NotificationTagExcelDTODefinition(executionContext, "");
            ///Building headers from NotificationTagExcelDTODefinition
            notificatinTagExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (!buildTemplate)
            {
                NotificationTagsDataHandler notificationTagsDataHandler = new NotificationTagsDataHandler(sqlTransaction);
                List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                notificationTagsDTOList = notificationTagsDataHandler.GetNotificationTagsDTOList(searchParameters);


                if (notificationTagsDTOList != null && notificationTagsDTOList.Any())
                {
                    foreach (NotificationTagsDTO notificationTagsDTO in notificationTagsDTOList)
                    {
                        notificatinTagExcelDTODefinition.Configure(notificationTagsDTO);

                        Row row = new Row();
                        notificatinTagExcelDTODefinition.Serialize(row, notificationTagsDTO);
                        sheet.AddRow(row);
                    }
                }
            }
            log.LogMethodExit();
            return sheet;
        }



        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            NotificationTagExcelDTODefinition notifictaionTagExcelDTODefinition = new NotificationTagExcelDTODefinition(executionContext, "");
            List<NotificationTagsDTO> rowNotificationTagsDTOList = new List<NotificationTagsDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    NotificationTagsDTO rowNotificationTagsDTO = (NotificationTagsDTO)notifictaionTagExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowNotificationTagsDTOList.Add(rowNotificationTagsDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowNotificationTagsDTOList != null && rowNotificationTagsDTOList.Any())
                    {
                        NotificationTagsListBL inventoryPhysicalCountsListBL = new NotificationTagsListBL(executionContext, rowNotificationTagsDTOList);
                        inventoryPhysicalCountsListBL.Save(sqlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            log.LogMethodExit(keyValuePairs);
            return keyValuePairs;

        }
        internal List<string> GetNotificationTagColumns(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            NotificationTagsDataHandler notificationTagsDataHandler = new NotificationTagsDataHandler(sqlTransaction);
            List<string>  result = notificationTagsDataHandler.GetNotificationTagColumns();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetAllNotificationTagsList
        /// </summary>
        /// <param name="cardNumberList"></param>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<NotificationTagsDTO> GetAllNotificationTagsList(List<string> cardNumberList, 
                                                                    List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> searchParameters,
                                                                    bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardNumberList, searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            NotificationTagsDataHandler notificationTagsDataHandler = new NotificationTagsDataHandler(sqlTransaction);
            this.notificationTagsDTOList = notificationTagsDataHandler.GetNotificationTagsDTOList(cardNumberList, searchParameters);
            if (notificationTagsDTOList != null && notificationTagsDTOList.Any() && loadChildRecords)
            {
                Build(notificationTagsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(notificationTagsDTOList);
            return notificationTagsDTOList;
        }
    }

}
