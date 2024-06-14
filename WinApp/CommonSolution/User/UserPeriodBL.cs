/********************************************************************************************
 * Project Name - Reports
 * Description  - A high level structure created to classify the User Period
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
    /// Bussiness Logic Of UserPeriod
    /// </summary>
    public class UserPeriodBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private UserPeriodDTO userPeriodDTO;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private UserPeriodBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Customer Feedback Survey object using the UserPeriodDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="userPeriodDTO">userPeriodDTO</param>
        public UserPeriodBL(ExecutionContext executionContext, UserPeriodDTO userPeriodDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userPeriodDTO);
            this.userPeriodDTO = userPeriodDTO;
            log.LogMethodExit();
        }

        public UserPeriodBL(ExecutionContext executionContext, int periodId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, periodId, sqlTransaction);
            UserPeriodDataHandler userPeriodDataHandler = new UserPeriodDataHandler(sqlTransaction);
            userPeriodDTO = userPeriodDataHandler.GetUserPeriodDTO(periodId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the UserPeriod
        /// Checks if the userTarget id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            UserPeriodDataHandler userPeriodDataHandler = new UserPeriodDataHandler(sqlTransaction);
            if (userPeriodDTO.PeriodId < 0)
            {
                log.LogVariableState("UserPeriodDTO", userPeriodDTO);
                userPeriodDTO = userPeriodDataHandler.InsertUserPeriod(userPeriodDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                userPeriodDTO.AcceptChanges();
            }
            else if (userPeriodDTO.IsChanged)
            {
                log.LogVariableState("UserPeriodDTO", userPeriodDTO);
                userPeriodDTO = userPeriodDataHandler.UpdateUserPeriod(userPeriodDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                userPeriodDTO.AcceptChanges();
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
        /// Delete the record UserPeriodDTO from the  database based on id
        /// </summary>
        /// <param name="id">int id </param>
        /// <returns>return the int </returns>
        public int Delete(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id);
            UserPeriodDataHandler userPeriodDataHandler = new UserPeriodDataHandler(sqlTransaction);
            int rowDeleted = userPeriodDataHandler.Delete(id);
            log.LogMethodExit(rowDeleted);
            return rowDeleted;
        }

        /// <summary>
        /// Gets UserPeriodDTO Object
        /// </summary>
        public UserPeriodDTO GetUserPeriodDTO
        {
            get { return userPeriodDTO; }
        }

    }

    /// <summary>
    /// Manages the list of User Period
    /// </summary>
    public class UserPeriodListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<UserPeriodDTO> userPeriodDTOList = new List<UserPeriodDTO>();

        /// <summary>
        /// Returns the User Period List BL
        /// </summary>
        public UserPeriodListBL(ExecutionContext executionContext)
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
        public UserPeriodListBL(ExecutionContext executionContext, List<UserPeriodDTO> userPeriodDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userPeriodDTOList);
            this.userPeriodDTOList = userPeriodDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the User Target DTO list
        /// </summary>
        public List<UserPeriodDTO> GetUserPeriodDTOList(List<KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UserPeriodDataHandler userPeriodDataHandler = new UserPeriodDataHandler(sqlTransaction);
            List<UserPeriodDTO> userPeriodDTOList = userPeriodDataHandler.GetUserPeriodDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(userPeriodDTOList);
            return userPeriodDTOList;
        }

        /// <summary>
        /// Saves the AchievementScoreConversion List
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userPeriodDTOList == null ||
               userPeriodDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < userPeriodDTOList.Count; i++)
            {
                var userPeriodDTO = userPeriodDTOList[i];
                if (userPeriodDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    UserPeriodBL userPeriodBL = new UserPeriodBL(executionContext, userPeriodDTO);
                    userPeriodBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving UserPeriodDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("UserPeriodDTOList", userPeriodDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
