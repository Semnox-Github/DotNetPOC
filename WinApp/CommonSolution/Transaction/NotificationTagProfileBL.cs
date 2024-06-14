/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business of the NotificationTagProfile class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        20-jul-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    public class NotificationTagProfileBL
    {
        private NotificationTagProfileDTO notificationTagProfileDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private NotificationTagProfileBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        ///<summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagProfileDTO"></param>
        public NotificationTagProfileBL(ExecutionContext executionContext, NotificationTagProfileDTO notificationTagProfileDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagProfileDTO);
            this.executionContext = executionContext;
            this.notificationTagProfileDTO = notificationTagProfileDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the notificationTagProfileId as the parameter
        /// Would fetch the notificationTagProfileDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="notificationTagProfileId">Id</param>
        public NotificationTagProfileBL(ExecutionContext executionContext, int notificationTagProfileId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagProfileId, sqlTransaction);
            NotificationTagProfileDataHandler notificationTagProfileDataHandler = new NotificationTagProfileDataHandler(sqlTransaction);
            notificationTagProfileDTO = notificationTagProfileDataHandler.GetNotificationTagProfileDTO(notificationTagProfileId);

            if (notificationTagProfileDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "NotificationTagProfile", notificationTagProfileId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the notificationTagProfileDTO
        /// asset will be inserted if notificationTagProfileId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            NotificationTagProfileDataHandler notificationTagProfileDataHandler = new NotificationTagProfileDataHandler(sqlTransaction);
            if (notificationTagProfileDTO.IsChangedRecursive == false &&
                notificationTagProfileDTO.NotificationTagProfileId > -1)
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
            if (notificationTagProfileDTO.NotificationTagProfileId < 0)
            {
                notificationTagProfileDTO = notificationTagProfileDataHandler.InsertNotificationTagProfile(notificationTagProfileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagProfileDTO.AcceptChanges();
            }
            else if (notificationTagProfileDTO.IsChanged)
            {
                notificationTagProfileDTO = notificationTagProfileDataHandler.UpdateNotificationTagProfile(notificationTagProfileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                notificationTagProfileDTO.AcceptChanges();
            }
            notificationTagProfileDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the notificationTagProfileDTO
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
        /// get NotificationTagProfileDTO Object
        /// </summary>
        public NotificationTagProfileDTO GetNotificationTagProfileDTO
        {
            get { return notificationTagProfileDTO; }
        }

        /// <summary>
        /// set NotificationTagProfileDTO Object        
        /// </summary>
        public NotificationTagProfileDTO SetNotificationTagProfileDTO
        {
            set { notificationTagProfileDTO = value; }
        }
    }

    /// <summary>
    /// Manages the list of NotificationTagProfileBL
    /// </summary>
    public class NotificationTagProfileListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<NotificationTagProfileDTO> notificationTagProfileDTOList = new List<NotificationTagProfileDTO>();
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public NotificationTagProfileListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="notificationTagProfileDTO"></param>
        public NotificationTagProfileListBL(ExecutionContext executionContext, List<NotificationTagProfileDTO> notificationTagProfileDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, notificationTagProfileDTOList);
            this.notificationTagProfileDTOList = notificationTagProfileDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the NotificationTagProfileDTO  list
        /// </summary>
        public List<NotificationTagProfileDTO> GetAllNotificationTagProfileList(List<KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>> searchParameters,
                                                                                 bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            NotificationTagProfileDataHandler notificationTagProfileDataHandler = new NotificationTagProfileDataHandler(sqlTransaction);
            this.notificationTagProfileDTOList = notificationTagProfileDataHandler.GetNotificationTagProfileDTOList(searchParameters);
            log.LogMethodExit(notificationTagProfileDTOList);
            return notificationTagProfileDTOList;
        }


        /// <summary>
        /// Saves the NotificationTagProfileDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (notificationTagProfileDTOList == null ||
                notificationTagProfileDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < notificationTagProfileDTOList.Count; i++)
            {
                var notificationTagProfileDTO = notificationTagProfileDTOList[i];
                if (notificationTagProfileDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    NotificationTagProfileBL notificationTagProfileBL = new NotificationTagProfileBL(executionContext, notificationTagProfileDTO);
                    notificationTagProfileBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving notificationTagProfileDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("notificationTagProfileDTO", notificationTagProfileDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }

}
