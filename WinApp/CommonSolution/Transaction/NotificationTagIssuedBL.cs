/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business Logic of the NotificationTagIssued class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        20-jul-2020   Mushahid Faizan         Created.
 *2.140.0     09-Sep-2021   Girish Kundar  Modified: Check In/Check out changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    public class NotificationTagIssuedBL
    {
        private NotificationTagIssuedDTO notificationTagIssuedDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private NotificationTagIssuedBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        ///<summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagIssuedDTO"></param>
        public NotificationTagIssuedBL(ExecutionContext executionContext, NotificationTagIssuedDTO notificationTagIssuedDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagIssuedDTO);
            this.executionContext = executionContext;
            this.notificationTagIssuedDTO = notificationTagIssuedDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the notificationTagIssuedId as the parameter
        /// Would fetch the notificationTagIssuedDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="notificationTagIssuedId">Id</param>
        public NotificationTagIssuedBL(ExecutionContext executionContext, int notificationTagIssuedId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagIssuedId, sqlTransaction);
            NotificationTagIssuedDataHandler notificationTagIssuedDataHandler = new NotificationTagIssuedDataHandler(sqlTransaction);
            notificationTagIssuedDTO = notificationTagIssuedDataHandler.GetNotificationTagIssuedDTO(notificationTagIssuedId);
            if (notificationTagIssuedDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "NotificationTagIssued", notificationTagIssuedId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the notificationTagIssuedDTO
        /// asset will be inserted if notificationTagIssuedId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            NotificationTagIssuedDataHandler notificationTagIssuedDataHandler = new NotificationTagIssuedDataHandler(sqlTransaction);
            if (notificationTagIssuedDTO.IsChanged == false &&
                notificationTagIssuedDTO.NotificationTagIssuedId > -1)
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
            if (notificationTagIssuedDTO.NotificationTagIssuedId < 0)
            {
                notificationTagIssuedDTO = notificationTagIssuedDataHandler.InsertNotificationTagIssued(notificationTagIssuedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagIssuedDTO.AcceptChanges();
            }
            else if (notificationTagIssuedDTO.IsChanged)
            {
                notificationTagIssuedDTO = notificationTagIssuedDataHandler.UpdateNotificationTagIssued(notificationTagIssuedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagIssuedDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the notificationTagIssuedDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// get NotificationTagIssuedDTO Object
        /// </summary>
        public NotificationTagIssuedDTO GetNotificationTagIssuedDTO
        {
            get { return notificationTagIssuedDTO; }
        }

        /// <summary>
        /// set NotificationTagIssuedDTO Object        
        /// </summary>
        public NotificationTagIssuedDTO SetNotificationTagIssuedDTO
        {
            set { notificationTagIssuedDTO = value; }
        }

        /// <summary>
        /// UpdateStartTime
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void UpdateStartTime(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (notificationTagIssuedDTO != null && notificationTagIssuedDTO.NotificationTagIssuedId > -1)
            {
                if (notificationTagIssuedDTO.StartDate != DateTime.MinValue)
                {
                    log.Debug("Start time is not null. Cannnot set the start time . returning");
                    return;
                }
                else if (notificationTagIssuedDTO.ExpiryDate <= ServerDateTime.Now)
                {
                    log.Debug("Tag is expired. Cannnot set the start time . returning");
                    return;
                }
                else if (notificationTagIssuedDTO.IsReturned)
                {
                    log.Debug("Tag is IsReturned. Cannnot set the start time . returning");
                    return;
                }
                NotificationTagIssuedDataHandler notificationTagIssuedDataHandler = new NotificationTagIssuedDataHandler(sqlTransaction);
                notificationTagIssuedDTO = notificationTagIssuedDataHandler.UpdateTagStartTime(notificationTagIssuedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagIssuedDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of NotificationTagIssuedBL
    /// </summary>
    public class NotificationTagIssuedListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<NotificationTagIssuedDTO> notificationTagIssuedDTOList = new List<NotificationTagIssuedDTO>();
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public NotificationTagIssuedListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagIssuedDTO"></param>
        public NotificationTagIssuedListBL(ExecutionContext executionContext, List<NotificationTagIssuedDTO> notificationTagIssuedDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagIssuedDTOList);
            this.notificationTagIssuedDTOList = notificationTagIssuedDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the NotificationTagIssuedDTO  list
        /// </summary>
        public List<NotificationTagIssuedDTO> GetAllNotificationTagIssuedDTOList(List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            NotificationTagIssuedDataHandler notificationTagIssuedDataHandler = new NotificationTagIssuedDataHandler(sqlTransaction);
            notificationTagIssuedDTOList = notificationTagIssuedDataHandler.GetNotificationTagIssuedDTOList(searchParameters);
            log.LogMethodExit();
            return notificationTagIssuedDTOList;
        }

        /// <summary>
        /// Gets the NotificationTagIssuedDTO List for notificationTagIssuedIdList
        /// </summary>
        /// <param name="notificationTagIssuedIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of NotificationTagIssuedDTO</returns>
        public List<NotificationTagIssuedDTO> GetNotificationTagIssuedDTOList(List<int> notificationTagIssuedIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(notificationTagIssuedIdList, activeRecords);
            NotificationTagIssuedDataHandler notificationTagIssuedDataHandler = new NotificationTagIssuedDataHandler(sqlTransaction);
            notificationTagIssuedDTOList = notificationTagIssuedDataHandler.GetNotificationTagIssuedDTOList(notificationTagIssuedIdList, activeRecords);
            log.LogMethodExit();
            return notificationTagIssuedDTOList;
        }

        /// <summary>
        /// Saves the NotificationTagIssuedDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (notificationTagIssuedDTOList == null ||
                notificationTagIssuedDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < notificationTagIssuedDTOList.Count; i++)
            {
                var notificationTagIssuedDTO = notificationTagIssuedDTOList[i];
                if (notificationTagIssuedDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    NotificationTagIssuedBL notificationTagIssuedBL = new NotificationTagIssuedBL(executionContext, notificationTagIssuedDTO);
                    notificationTagIssuedBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving notificationTagIssuedDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("notificationTagIssuedDTO", notificationTagIssuedDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}


