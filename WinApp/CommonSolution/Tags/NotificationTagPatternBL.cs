/********************************************************************************************
 * Project Name - Tags
 * Description  - Business of the NotificationTagPattern class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        21-jul-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Tags
{
    public class NotificationTagPatternBL
    {
        private NotificationTagPatternDTO notificationTagPatternDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private NotificationTagPatternBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        ///<summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagPatternDTO"></param>
        public NotificationTagPatternBL(ExecutionContext executionContext, NotificationTagPatternDTO notificationTagPatternDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagPatternDTO);
            this.executionContext = executionContext;
            this.notificationTagPatternDTO = notificationTagPatternDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the notificationTagPatternId as the parameter
        /// Would fetch the notificationTagPatternDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="notificationTagPatternId">Id</param>
        public NotificationTagPatternBL(ExecutionContext executionContext, int notificationTagPatternId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagPatternId, sqlTransaction);
            NotificationTagPatternDataHandler notificationTagPatternDataHandler = new NotificationTagPatternDataHandler(sqlTransaction);
            notificationTagPatternDTO = notificationTagPatternDataHandler.GetNotificationTagPatternDTO(notificationTagPatternId);
            if (notificationTagPatternDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "NotificationTagPattern", notificationTagPatternId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the notificationTagPatternDTO
        /// asset will be inserted if notificationTagPatternId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            NotificationTagPatternDataHandler notificationTagPatternDataHandler = new NotificationTagPatternDataHandler(sqlTransaction);
            if (notificationTagPatternDTO.IsChanged == false &&
                notificationTagPatternDTO.NotificationTagPatternId > -1)
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
            if (notificationTagPatternDTO.NotificationTagPatternId <= 0)
            {
                notificationTagPatternDTO = notificationTagPatternDataHandler.InsertNotificationTagPattern(notificationTagPatternDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagPatternDTO.AcceptChanges();
            }
            else if (notificationTagPatternDTO.IsChanged)
            {
                notificationTagPatternDTO = notificationTagPatternDataHandler.UpdateNotificationTagPattern(notificationTagPatternDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagPatternDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the notificationTagPatternDTO
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
        /// get NotificationTagPatternDTO Object
        /// </summary>
        public NotificationTagPatternDTO GetNotificationTagPatternDTO
        {
            get { return notificationTagPatternDTO; }
        }

        /// <summary>
        /// set NotificationTagPatternDTO Object        
        /// </summary>
        public NotificationTagPatternDTO SetNotificationTagPatternDTO
        {
            set { notificationTagPatternDTO = value; }
        }
    }

    /// <summary>
    /// Manages the list of NotificationTagPatternBL
    /// </summary>
    public class NotificationTagPatternListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<NotificationTagPatternDTO> notificationTagPatternDTOList = new List<NotificationTagPatternDTO>();
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public NotificationTagPatternListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagPatternDTO"></param>
        public NotificationTagPatternListBL(ExecutionContext executionContext, List<NotificationTagPatternDTO> notificationTagPatternDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagPatternDTOList);
            this.notificationTagPatternDTOList = notificationTagPatternDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the NotificationTagPatternDTO  list
        /// </summary>
        public List<NotificationTagPatternDTO> GetAllNotificationTagPatternList(List<KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            NotificationTagPatternDataHandler notificationTagPatternDataHandler = new NotificationTagPatternDataHandler(sqlTransaction);
            notificationTagPatternDTOList = notificationTagPatternDataHandler.GetNotificationTagPatternDTOList(searchParameters);
            log.LogMethodExit();
            return notificationTagPatternDTOList;
        }

        /// <summary>
        /// Saves the NotificationTagPatternDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (notificationTagPatternDTOList == null ||
                notificationTagPatternDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < notificationTagPatternDTOList.Count; i++)
            {
                var notificationTagPatternDTO = notificationTagPatternDTOList[i];
                if (notificationTagPatternDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    NotificationTagPatternBL notificationTagPatternBL = new NotificationTagPatternBL(executionContext, notificationTagPatternDTO);
                    notificationTagPatternBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving notificationTagPatternDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("notificationTagPatternDTO", notificationTagPatternDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
