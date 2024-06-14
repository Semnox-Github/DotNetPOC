/********************************************************************************************
 * Project Name - AchievementsBL
 * Description  - Bussiness logic of the   AchievementScoreLog class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        4-may-2017   Rakshith         Created 
 *2.70        4-jul-2019   Deeksha          Modified:Save() method for Insert /Update returns DTO instead of Id
 *                                                    Added Execution Context object for Constructors.
 *                                                    changed log.debug to log.logMethodEntry
 *                                                    and log.logMethodExit
 *2.80        4-Mar-2020    Vikas Dwivedi   Modified as per the Standard for Phase 1 Changes.
 ********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using System.Linq;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// Business Logic for AchievementScoreLog
    /// </summary>
    public class AchievementScoreLog
    {
        private AchievementScoreLogDTO achievementScoreLogDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AchievementScoreLog
        /// </summary>
        private AchievementScoreLog(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AchievementScoreLog object using the achievementScoreLogDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="achievementScoreLogDTO">AchievementScoreLogDTO object</param>
        public AchievementScoreLog(ExecutionContext executionContext, AchievementScoreLogDTO achievementScoreLogDTO)
          : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementScoreLogDTO);
            this.achievementScoreLogDTO = achievementScoreLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the achievementScoreLog id as the parameter
        /// Would fetch the achievementScoreLog object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of AchievementScoreLog Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AchievementScoreLog(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AchievementScoreLogDataHandler achievementScoreLogDataHandler = new AchievementScoreLogDataHandler(sqlTransaction);
            this.achievementScoreLogDTO = achievementScoreLogDataHandler.GetAchievementScoreLogDTO(id);
            if (achievementScoreLogDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " AchievementScoreLogDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(); ;
        }

        /// <summary>
        /// Saves the AchievementScoreLog
        /// Checks if the AchievementScoreLogid is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AchievementScoreLogDataHandler achievementScoreLogDatahandler = new AchievementScoreLogDataHandler(sqlTransaction);
            if (achievementScoreLogDTO.Id < 0)
            {
                log.LogVariableState("AchievementScoreLogDTO", achievementScoreLogDTO);
                achievementScoreLogDTO = achievementScoreLogDatahandler.InsertAchievementScoreLog(achievementScoreLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementScoreLogDTO.AcceptChanges();
            }
            else if (achievementScoreLogDTO.IsChanged)
            {
                log.LogVariableState("AchievementScoreLogDTO", achievementScoreLogDTO);
                achievementScoreLogDTO = achievementScoreLogDatahandler.UpdateAchievementScoreLog(achievementScoreLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementScoreLogDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AchievementScoreLogDTO
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
        /// Gets AchievementScoreLogDTO Object
        /// </summary>
        public AchievementScoreLogDTO GetAchievementScoreLogDTO
        {
            get { return achievementScoreLogDTO; }
        }
    }

    /// <summary>
    /// Manages the list of AchievementScoreLogs
    /// </summary>
    public class AchievementScoreLogsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AchievementScoreLogDTO> achievementScoreLogDTOList = new List<AchievementScoreLogDTO>();

        /// <summary>
        /// Parameterized constructor for AchievementScoreLogsList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public AchievementScoreLogsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for AchievementScoreLogsList with AchievementScoreLogsDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="achievementScoreLogsDTOList">achievementScoreLogsDTOList object is passed as parameter</param>
        public AchievementScoreLogsList(ExecutionContext executionContext,
                                                List<AchievementScoreLogDTO> achievementScoreLogDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementScoreLogDTOList);
            this.achievementScoreLogDTOList = achievementScoreLogDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AchievementScoreLogs list
        /// </summary>
        public List<AchievementScoreLogDTO> GetAllAchievementScoreLogs(List<KeyValuePair<AchievementScoreLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AchievementScoreLogDataHandler achievementScoreLogDatahandler = new AchievementScoreLogDataHandler(sqlTransaction);
            List<AchievementScoreLogDTO> achievementScoreLogDTOList = new List<AchievementScoreLogDTO>();
            achievementScoreLogDTOList = achievementScoreLogDatahandler.GetAchievementScoreLogDTOList(searchParameters);
            log.LogMethodExit(achievementScoreLogDTOList);
            return achievementScoreLogDTOList;
        }

        /// <summary>
        /// Saves the AchievementScoreLog List
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementScoreLogDTOList == null ||
               achievementScoreLogDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < achievementScoreLogDTOList.Count; i++)
            {
                var achievementScoreLogDTO = achievementScoreLogDTOList[i];
                if (achievementScoreLogDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AchievementScoreLog achievementScoreLog = new AchievementScoreLog(executionContext, achievementScoreLogDTO);
                    achievementScoreLog.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AchievementScoreLogDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AchievementScoreLogDTO", achievementScoreLogDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
