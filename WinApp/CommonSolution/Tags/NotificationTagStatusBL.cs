/********************************************************************************************
 * Project Name - Tags
 * Description  - Business Logic of the NotificationTagStatus class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        21-jul-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Tags
{
    public class NotificationTagStatusBL
    {
        private NotificationTagStatusDTO notificationTagStatusDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private NotificationTagStatusBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        ///<summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagStatusDTO"></param>
        public NotificationTagStatusBL(ExecutionContext executionContext, NotificationTagStatusDTO notificationTagStatusDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagStatusDTO);
            this.executionContext = executionContext;
            this.notificationTagStatusDTO = notificationTagStatusDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the notificationTagStatusId as the parameter
        /// Would fetch the notificationTagStatusDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="notificationTagStatusId">Id</param>
        public NotificationTagStatusBL(ExecutionContext executionContext, int notificationTagStatusId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagStatusId, sqlTransaction);
            NotificationTagStatusDataHandler notificationTagStatusDataHandler = new NotificationTagStatusDataHandler(sqlTransaction);
            notificationTagStatusDTO = notificationTagStatusDataHandler.GetNotificationTagStatusDTO(notificationTagStatusId);
            if (notificationTagStatusDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "NotificationTagStatus", notificationTagStatusId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the notificationTagStatusDTO
        /// asset will be inserted if notificationTagStatusId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            NotificationTagStatusDataHandler notificationTagStatusDataHandler = new NotificationTagStatusDataHandler(sqlTransaction);
            if (notificationTagStatusDTO.IsChanged == false &&
                notificationTagStatusDTO.NotificationTagStatusId > -1)
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
            if (notificationTagStatusDTO.NotificationTagStatusId <= 0)
            {
                notificationTagStatusDTO = notificationTagStatusDataHandler.InsertNotificationTagStatus(notificationTagStatusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagStatusDTO.AcceptChanges();
            }
            else if (notificationTagStatusDTO.IsChanged)
            {
                notificationTagStatusDTO = notificationTagStatusDataHandler.UpdateNotificationTagStatus(notificationTagStatusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagStatusDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the notificationTagStatusDTO
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
        /// get NotificationTagStatusDTO Object
        /// </summary>
        public NotificationTagStatusDTO GetNotificationTagStatusDTO
        {
            get { return notificationTagStatusDTO; }
        }

        /// <summary>
        /// set NotificationTagStatusDTO Object        
        /// </summary>
        public NotificationTagStatusDTO SetNotificationTagStatusDTO
        {
            set { notificationTagStatusDTO = value; }
        }
    }

    /// <summary>
    /// Manages the list of NotificationTagStatusBL
    /// </summary>
    public class NotificationTagStatusListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<NotificationTagStatusDTO> notificationTagStatusDTOList = new List<NotificationTagStatusDTO>();
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public NotificationTagStatusListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagStatusDTO"></param>
        public NotificationTagStatusListBL(ExecutionContext executionContext, List<NotificationTagStatusDTO> notificationTagStatusDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagStatusDTOList);
            this.notificationTagStatusDTOList = notificationTagStatusDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the NotificationTagStatusDTO  list
        /// </summary>
        public List<NotificationTagStatusDTO> GetAllNotificationTagStatusDTOList(List<KeyValuePair<NotificationTagStatusDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            NotificationTagStatusDataHandler notificationTagStatusDataHandler = new NotificationTagStatusDataHandler(sqlTransaction);
            notificationTagStatusDTOList = notificationTagStatusDataHandler.GetNotificationTagStatusDTOList(searchParameters);
            log.LogMethodExit(notificationTagStatusDTOList);
            return notificationTagStatusDTOList;
        }

        /// <summary>
        /// Gets the NotificationTagStatusDTO List for notificationTagStatusIdList
        /// </summary>
        /// <param name="notificationTagStatusIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of NotificationTagStatusDTO</returns>
        public List<NotificationTagStatusDTO> GetNotificationTagStatusDTOList(List<int> notificationTagStatusIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(notificationTagStatusIdList, activeRecords);
            NotificationTagStatusDataHandler notificationTagStatusDataHandler = new NotificationTagStatusDataHandler(sqlTransaction);
            notificationTagStatusDTOList = notificationTagStatusDataHandler.GetNotificationTagStatusDTOList(notificationTagStatusIdList, activeRecords);
            log.LogMethodExit(notificationTagStatusDTOList);
            return notificationTagStatusDTOList;
        }

        /// <summary>
        /// Saves the NotificationTagStatusDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (notificationTagStatusDTOList == null ||
                notificationTagStatusDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < notificationTagStatusDTOList.Count; i++)
            {
                var notificationTagStatusDTO = notificationTagStatusDTOList[i];
                if (notificationTagStatusDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    NotificationTagStatusBL notificationTagStatusBL = new NotificationTagStatusBL(executionContext, notificationTagStatusDTO);
                    notificationTagStatusBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving notificationTagStatusDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("notificationTagStatusDTO", notificationTagStatusDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }


    }
}
