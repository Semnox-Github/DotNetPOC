/********************************************************************************************
 * Project Name - Reports
 * Description  - A high level structure created to classify the User Target
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.80        01-Jun-2020   Vikas Dwivedi       Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Bussiness Logic Of UserTarget
    /// </summary>
    class UserTargetBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private UserTargetDTO userTargetDTO;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private UserTargetBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Customer Feedback Survey object using the UserTargetDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="userTargetDTO">userTargetDTO</param>
        public UserTargetBL(ExecutionContext executionContext, UserTargetDTO userTargetDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, userTargetDTO);
            this.userTargetDTO = userTargetDTO;
            log.LogMethodExit();
        }

        public UserTargetBL(ExecutionContext executionContext, int userTargetId, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, userTargetId, sqlTransaction);
            UserTargetDataHandler userTargetDataHandler = new UserTargetDataHandler(sqlTransaction);
            userTargetDTO = userTargetDataHandler.GetUserTargetDTO(userTargetId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the UserTarget
        /// Checks if the userTarget id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            UserTargetDataHandler userTargetDataHandler = new UserTargetDataHandler(sqlTransaction);
            if (userTargetDTO.UserTargetId < 0)
            {
                log.LogVariableState("UserTargetDTO", userTargetDTO);
                userTargetDTO = userTargetDataHandler.InsertUserTarget(userTargetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                userTargetDTO.AcceptChanges();
            }
            else if (userTargetDTO.IsChanged)
            {
                log.LogVariableState("UserTargetDTO", userTargetDTO);
                userTargetDTO = userTargetDataHandler.UpdateUserTarget(userTargetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                userTargetDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the UserTargetDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            return validationErrorList;
            // Validation Logic here 
        }

        /// <summary>
        /// Gets UserTargetDTO Object
        /// </summary>
        public UserTargetDTO GetUserTargetDTO
        {
            get { return userTargetDTO; }
        }

    }

    /// <summary>
    /// Manages the list of User Target
    /// </summary>
    public class UserTargetListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<UserTargetDTO> userTargetDTOList = new List<UserTargetDTO>();

        /// <summary>
        /// Returns the User Target List BL
        /// </summary>
        public UserTargetListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="userTargetDTOList">userTargetDTOList</param>
        public UserTargetListBL(ExecutionContext executionContext, List<UserTargetDTO> userTargetDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userTargetDTOList);
            this.userTargetDTOList = userTargetDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the User Target DTO list
        /// </summary>
        public List<UserTargetDTO> GetUserTargetDTOList(List<KeyValuePair<UserTargetDTO.SearchByUserTargetSearchParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UserTargetDataHandler userTargetDataHandler = new UserTargetDataHandler(sqlTransaction);
            List<UserTargetDTO> userTargetDTOList = userTargetDataHandler.GetUserTargetDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(userTargetDTOList);
            return userTargetDTOList;
        }

        /// <summary>
        /// Saves the AchievementScoreConversion List
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userTargetDTOList == null ||
               userTargetDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < userTargetDTOList.Count; i++)
            {
                var userTargetDTO = userTargetDTOList[i];
                if (userTargetDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    UserTargetBL userTargetBL = new UserTargetBL(executionContext, userTargetDTO);
                    userTargetBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving UserTargetDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("UserTargetDTOList", userTargetDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
