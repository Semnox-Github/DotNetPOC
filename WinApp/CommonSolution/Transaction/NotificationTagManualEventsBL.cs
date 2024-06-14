/********************************************************************************************
* Project Name - NotificationTagManualEventsBL
* Description - BL for NotificationTagManualEvents 
*
**************
**Version Log 
**************
*Version    Date        Modified By     Remarks
*********************************************************************************************
*2.110.0    07-Jan-2021  Fiona          Created 
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Transaction
{
    public class NotificationTagManualEventsBL
    {
        private NotificationTagManualEventsDTO notificationTagManualEventsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private NotificationTagManualEventsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        ///<summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagManualEventsDTO"></param>
        public NotificationTagManualEventsBL(ExecutionContext executionContext, NotificationTagManualEventsDTO notificationTagManualEventsDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagManualEventsDTO);
            this.executionContext = executionContext;
            this.notificationTagManualEventsDTO = notificationTagManualEventsDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the NotificationTagManualEventsId as the parameter
        /// Would fetch the NotificationTagManualEventsDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="notificationTagManualEventsId">Id</param>
        public NotificationTagManualEventsBL(ExecutionContext executionContext, int notificationTagManualEventsId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagManualEventsId, sqlTransaction);
            NotificationTagManualEventsDataHandler notificationTagManualEventsDataHandler = new NotificationTagManualEventsDataHandler(sqlTransaction);
            notificationTagManualEventsDTO = notificationTagManualEventsDataHandler.GetNotificationTagManualEventsDTO(notificationTagManualEventsId);
            if (notificationTagManualEventsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "NotificationTagManualEvents", notificationTagManualEventsId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the NotificationTagManualEventsDTO
        /// asset will be inserted if NotificationTagManualEventsId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            NotificationTagManualEventsDataHandler notificationTagManualEventsDataHandler = new NotificationTagManualEventsDataHandler(sqlTransaction);
            if (notificationTagManualEventsDTO.IsChanged == false &&
                notificationTagManualEventsDTO.NotificationTagMEventId > -1)
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
            if (notificationTagManualEventsDTO.NotificationTagMEventId < 0)
            {
                notificationTagManualEventsDTO = notificationTagManualEventsDataHandler.InsertNotificationTagManualEvents(notificationTagManualEventsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagManualEventsDTO.AcceptChanges();
            }
            else if (notificationTagManualEventsDTO.IsChanged)
            {
                notificationTagManualEventsDTO = notificationTagManualEventsDataHandler.UpdateNotificationTagManualEvents(notificationTagManualEventsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagManualEventsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validate the NotificationTagManualEventsDTO
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
    }
    public class NotificationTagManualEventsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<NotificationTagManualEventsDTO> notificationTagManualEventsDTOList = new List<NotificationTagManualEventsDTO>();
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public NotificationTagManualEventsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagManualEventsDTOList"></param>
        public NotificationTagManualEventsListBL(ExecutionContext executionContext, List<NotificationTagManualEventsDTO> notificationTagManualEventsDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagManualEventsDTOList);
            this.notificationTagManualEventsDTOList = notificationTagManualEventsDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the NotificationTagManualEventsDTO  list
        /// </summary>
        public List<NotificationTagManualEventsDTO> GetAllNotificationTagManualEventsDTOList(List<KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            NotificationTagManualEventsDataHandler notificationTagManualEventsDataHandler = new NotificationTagManualEventsDataHandler(sqlTransaction);
            notificationTagManualEventsDTOList = notificationTagManualEventsDataHandler.GetNotificationTagManualEventsDTOList(searchParameters);
            log.LogMethodExit();
            return notificationTagManualEventsDTOList;
        }
        
        /// <summary>
        /// Saves the NotificationTagManualEventsDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (notificationTagManualEventsDTOList == null ||
                notificationTagManualEventsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < notificationTagManualEventsDTOList.Count; i++)
            {
                var notificationTagManualEventsDTO = notificationTagManualEventsDTOList[i];
                if (notificationTagManualEventsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    NotificationTagManualEventsBL notificationTagManualEventsBL = new NotificationTagManualEventsBL(executionContext, notificationTagManualEventsDTO);
                    notificationTagManualEventsBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving NotificationTagManualEventsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("NotificationTagManualEventsDTO", notificationTagManualEventsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }

    
}
