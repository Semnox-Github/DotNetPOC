/*****************************************************************************************************************
 * Project Name - Achievements
 * Description  - Bussiness Logic of AchievementClass
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *****************************************************************************************************************
 *2.70        02-JUl-2019    Deeksha                 Modified : Save() method for Insert /Update returns DTO instead of Id
 *                                                              Added Execution Context object for Constructors. 
 *                                                              changed log.debug to log.logMethodEntry
 *                                                             and log.logMethodExit
 *2.80        27-Aug-2019   Vikas Dwivedi         Added Parameterized Constructor in both class,
 *2.80        19-Nov-2019   Vikas Dwivedi         Added Logger Method
 *2.80        04-Mar-2020   Vikas Dwivedi         Modified as per the Standard for Phase 1 changes.
 *****************************************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// Business Logic for AchievementClass
    /// </summary>
    public class AchievementClass
    {
        private AchievementClassDTO achievementClassDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor of AchievementClass
        /// </summary>
        /// <param name="executionContext"></param>
        private AchievementClass(ExecutionContext executionContext) 
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AchievementClass object using the appUIPanelElementDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="achievementDTO">AchievementClassDTO object is passed as parameter</param>
        public AchievementClass(ExecutionContext executionContext, AchievementClassDTO achievementClassDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementClassDTO);
            this.achievementClassDTO = achievementClassDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AchievementClass id as the parameter
        /// Would fetch the AchievementClass object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="achievementClassId">id of AchievementClass Object </param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false.</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false.</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AchievementClass(ExecutionContext executionContext, int achievementClassId, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, achievementClassId, sqlTransaction);
            AchievementClassDataHandler achievementClassDataHandler = new AchievementClassDataHandler(sqlTransaction);
            achievementClassDTO = achievementClassDataHandler.GetAchievementClassDTO(achievementClassId);
            if (achievementClassDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AchievementClass", achievementClassId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get AchievementClassDTO Object
        /// </summary>
        public AchievementClassDTO GetAchievementClassDTO
        {
            get
            {
                return achievementClassDTO;
            }
        }

        /// <summary>
        /// Builds the child records for AchievementClass object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            AchievementClassLevelsList achievementClassLevelsList = new AchievementClassLevelsList(executionContext);
            List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID, achievementClassDTO.AchievementClassId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            achievementClassDTO.AchievementClassLevelDTOList = achievementClassLevelsList.GetAchievementClassLevelList(searchParameters, true, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AchievementClass
        /// Checks if the achievementClassId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementClassDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation failed", validationErrorList);
            }
            AchievementClassDataHandler achievementClassDataHandler = new AchievementClassDataHandler(sqlTransaction);
            if (achievementClassDTO.AchievementClassId < 0)
            {
                log.LogVariableState("AchievementClassDTO", achievementClassDTO);
                achievementClassDTO = achievementClassDataHandler.InsertAchievementClass(achievementClassDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementClassDTO.AcceptChanges();
            }
            else if (achievementClassDTO.IsChanged)
            {
                log.LogVariableState("AchievementClassDTO", achievementClassDTO);
                achievementClassDTO = achievementClassDataHandler.UpdateAchievementClass(achievementClassDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementClassDTO.AcceptChanges();
            }
            SaveAchievementClassLevel(sqlTransaction);
            achievementClassDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : AchievementClassLevelDTOList and AchievementScoreLogDTOList
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        private void SaveAchievementClassLevel(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementClassDTO.AchievementClassLevelDTOList != null &&
                achievementClassDTO.AchievementClassLevelDTOList.Any())
            {
                List<AchievementClassLevelDTO> updatedAchievementClassLevelDTOList = new List<AchievementClassLevelDTO>();
                foreach (var achievementClassLevelDTO in achievementClassDTO.AchievementClassLevelDTOList)
                {
                    if (achievementClassLevelDTO.AchievementClassId != achievementClassDTO.AchievementClassId)
                    {
                        achievementClassLevelDTO.AchievementClassId = achievementClassDTO.AchievementClassId;
                    }
                    if (achievementClassLevelDTO.IsChanged)
                    {
                        updatedAchievementClassLevelDTOList.Add(achievementClassLevelDTO);
                    }
                }
                if (updatedAchievementClassLevelDTOList.Any())
                {
                    log.LogVariableState("UpdatedAchievementClassLevelDTOList", updatedAchievementClassLevelDTOList);
                    AchievementClassLevelsList achievementClassLevelsListBL = new AchievementClassLevelsList(executionContext, updatedAchievementClassLevelDTOList);
                    achievementClassLevelsListBL.Save(sqlTransaction);
                }
            }
            // For child records : AchievementScoreLog
            if (achievementClassDTO.AchievementScoreLogDTOList != null &&
                achievementClassDTO.AchievementScoreLogDTOList.Any())
            {
                List<AchievementScoreLogDTO> updatedAchievementScoreLogDTOList = new List<AchievementScoreLogDTO>();
                foreach (var achievementScoreLogDTO in achievementClassDTO.AchievementScoreLogDTOList)
                {
                    if (achievementScoreLogDTO.AchievementClassId != achievementClassDTO.AchievementClassId)
                    {
                        achievementScoreLogDTO.AchievementClassId = achievementClassDTO.AchievementClassId;
                    }
                    if (achievementScoreLogDTO.IsChanged)
                    {
                        updatedAchievementScoreLogDTOList.Add(achievementScoreLogDTO);
                    }
                }
                if (updatedAchievementScoreLogDTOList.Any())
                {
                    log.LogVariableState("UpdatedAchievementScoreLogDTOList", updatedAchievementScoreLogDTOList);
                    AchievementScoreLogsList achievementScoreLogsListBL = new AchievementScoreLogsList(executionContext, updatedAchievementScoreLogDTOList);
                    achievementScoreLogsListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AchievementClass, AchievementClassLevelDTOList and AchievementScoreLogDTOList- child 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;

            // Validation Logic here
            if (string.IsNullOrWhiteSpace(achievementClassDTO.ClassName))
            {
                validationErrorList.Add(new ValidationError("AchievementClass", "ClassName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Class Name"))));
            }

            if (!string.IsNullOrWhiteSpace(achievementClassDTO.ClassName) && achievementClassDTO.ClassName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("AchievementClass", "ClassName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Class Name"), 50)));
            }

            // validate Child list
            if (achievementClassDTO.AchievementClassLevelDTOList != null &&
                achievementClassDTO.AchievementClassLevelDTOList.Count > 0)
            {
                foreach (AchievementClassLevelDTO achievementClassLevelDTO in achievementClassDTO.AchievementClassLevelDTOList)
                {
                    AchievementClassLevel achievementClassLevelBL = new AchievementClassLevel(executionContext, achievementClassLevelDTO);
                    validationErrorList.AddRange(achievementClassLevelBL.Validate());
                }
            }

            // validate Child List
            if (achievementClassDTO.AchievementScoreLogDTOList != null &&
                achievementClassDTO.AchievementScoreLogDTOList.Count > 0)
            {
                foreach (AchievementScoreLogDTO achievementScoreLogDTO in achievementClassDTO.AchievementScoreLogDTOList)
                {
                    AchievementScoreLog achievementScoreLogBL = new AchievementScoreLog(executionContext, achievementScoreLogDTO);
                    validationErrorList.AddRange(achievementScoreLogBL.Validate());
                }
            }
            return validationErrorList;
        }

        ///// <summary>
        /////  Delete the record from the AchievementClass database based on achievementClassId
        ///// </summary>
        ///// <param name="achievementClassId">achievementClassId</param>
        ///// <param name="sqlTransaction">sqlTransaction</param>
        ///// <returns>id </returns>
        //public int Delete(int achievementClassId, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(achievementClassId, sqlTransaction);

        //    AchievementClassDataHandler achievementClassDataHandler = new AchievementClassDataHandler(sqlTransaction);
        //    int id = achievementClassDataHandler.Delete(achievementClassId);
        //    log.LogMethodExit(id);
        //    return id;
        //}
    }

    public class AchievementClassesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AchievementClassDTO> achievementClassDTOList = new List<AchievementClassDTO>();

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        public AchievementClassesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="achievementClassDTOList"></param>
        public AchievementClassesList(ExecutionContext executionContext, List<AchievementClassDTO> achievementClassDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementClassDTOList);
            this.achievementClassDTOList = achievementClassDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AchievementClassDTO list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of AchievementClassDTO</returns>
        public List<AchievementClassDTO> GetAchievementClassList(List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            // child records needs to build
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AchievementClassDataHandler achievementClassDataHandler = new AchievementClassDataHandler(sqlTransaction);
            List<AchievementClassDTO> achievementClassDTOList = achievementClassDataHandler.GetAchievementClassDTOList(searchParameters, sqlTransaction);
            if (achievementClassDTOList != null && achievementClassDTOList.Any() && loadChildRecords)
            {
                Build(achievementClassDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(achievementClassDTOList);
            return achievementClassDTOList;
        }

        /// <summary>
        ///Takes AchievementClassParams as parameter
        /// </summary>
        /// <returns>Returns List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>> by converting achievementClassParams</returns>
        public List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>> BuildAchievementClassSearchParametersList(AchievementClassParams achievementClassParams)
        {
            log.LogMethodEntry(achievementClassParams);
            List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>> achievementClassSearchParams = new List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>>();
            if (achievementClassParams != null)
            {
                if (achievementClassParams.AchievementClassId > 0)
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID, achievementClassParams.AchievementClassId.ToString()));
                if (achievementClassParams.AchievementProjectId > 0)
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.ACHIEVEMENT_PROJECT_ID, achievementClassParams.AchievementProjectId.ToString()));
                if (!string.IsNullOrEmpty(achievementClassParams.ClassName))
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.CLASS_NAME, achievementClassParams.ClassName));
                if (achievementClassParams.GameId > 0)
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.GAME_ID, achievementClassParams.GameId.ToString()));
                if (achievementClassParams.IsActive)
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.IS_ACTIVE, "1"));
                if (achievementClassParams.SiteId > 0)
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.SITE_ID, achievementClassParams.SiteId.ToString()));
                if (!string.IsNullOrEmpty(achievementClassParams.ExternalSystemReference))
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, achievementClassParams.ExternalSystemReference));
            }
            log.LogMethodExit(achievementClassSearchParams);
            return achievementClassSearchParams;
        }

        /// <summary>
        /// Returns the AchievementClassDTO list
        /// </summary>
        public List<AchievementClassDTO> GetAchievementClassList(AchievementClassParams achievementClassParams, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(achievementClassParams, sqlTransaction);
                List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>> searchParameters = BuildAchievementClassSearchParametersList(achievementClassParams);
                AchievementClassDataHandler achievementClassDataHandler = new AchievementClassDataHandler(sqlTransaction);
                List<AchievementClassDTO> achievementClassDTOList = new List<AchievementClassDTO>();
                achievementClassDTOList = achievementClassDataHandler.GetAchievementClassDTOList(searchParameters, sqlTransaction);
                log.LogMethodExit(achievementClassDTOList);
                return achievementClassDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Exception at GetAchievementClassList()", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }

        /// <summary>
        /// Builds the List of AchievementClass object based on the list of AchievementClass id.
        /// </summary>
        /// <param name="achievementClassDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<AchievementClassDTO> achievementClassDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(achievementClassDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AchievementClassDTO> achievementClassIdAchievementClassDictionary = new Dictionary<int, AchievementClassDTO>();
            StringBuilder sb = new StringBuilder(string.Empty);
            string achievementClassIdSet;
            for (int i = 0; i < achievementClassDTOList.Count; i++)
            {
                if (achievementClassDTOList[i].AchievementClassId == -1 ||
                    achievementClassIdAchievementClassDictionary.ContainsKey(achievementClassDTOList[i].AchievementClassId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(achievementClassDTOList[i].AchievementClassId);
                achievementClassIdAchievementClassDictionary.Add(achievementClassDTOList[i].AchievementClassId, achievementClassDTOList[i]);
            }

            achievementClassIdSet = sb.ToString();

            // Build child Records - AchievementClassLevel
            AchievementClassLevelsList achievementClassLevelsList = new AchievementClassLevelsList(executionContext);
            List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>> searchAchievementClassLevelParams = new List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>>();
            searchAchievementClassLevelParams.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID_LIST, achievementClassIdSet.ToString()));
            if (activeChildRecords)
            {
                searchAchievementClassLevelParams.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            List<AchievementClassLevelDTO> achievementClassLevelDTOList = achievementClassLevelsList.GetAchievementClassLevelList(searchAchievementClassLevelParams, true, activeChildRecords, sqlTransaction);
            if (achievementClassLevelDTOList != null && achievementClassLevelDTOList.Any())
            {
                log.LogVariableState("AchievementClassLevelsDTOList", achievementClassLevelDTOList);
                foreach (var achievementClassLevelDTO in achievementClassLevelDTOList)
                {
                    if (achievementClassIdAchievementClassDictionary.ContainsKey(achievementClassLevelDTO.AchievementClassId))
                    {
                        if (achievementClassIdAchievementClassDictionary[achievementClassLevelDTO.AchievementClassId].AchievementClassLevelDTOList == null)
                        {
                            achievementClassIdAchievementClassDictionary[achievementClassLevelDTO.AchievementClassId].AchievementClassLevelDTOList = new List<AchievementClassLevelDTO>();
                        }
                        achievementClassIdAchievementClassDictionary[achievementClassLevelDTO.AchievementClassId].AchievementClassLevelDTOList.Add(achievementClassLevelDTO);
                    }
                }
            }

            // Build child Records - AchievementScoreLog
            AchievementScoreLogsList achievementScoreLogsList = new AchievementScoreLogsList(executionContext);
            List<KeyValuePair<AchievementScoreLogDTO.SearchByParameters, string>> searchAchievementScoreLogParams = new List<KeyValuePair<AchievementScoreLogDTO.SearchByParameters, string>>();
            searchAchievementScoreLogParams.Add(new KeyValuePair<AchievementScoreLogDTO.SearchByParameters, string>(AchievementScoreLogDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID_LIST, achievementClassIdSet.ToString()));
            if (activeChildRecords)
            {
                searchAchievementScoreLogParams.Add(new KeyValuePair<AchievementScoreLogDTO.SearchByParameters, string>(AchievementScoreLogDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            List<AchievementScoreLogDTO> achievementScoreLogDTOList = achievementScoreLogsList.GetAllAchievementScoreLogs(searchAchievementScoreLogParams, sqlTransaction);
            if (achievementScoreLogDTOList != null && achievementScoreLogDTOList.Any())
            {
                log.LogVariableState("AchievementScoreLogDTOList", achievementScoreLogDTOList);
                foreach (var achievementScoreLogDTO in achievementScoreLogDTOList)
                {
                    if (achievementClassIdAchievementClassDictionary.ContainsKey(achievementScoreLogDTO.AchievementClassId))
                    {
                        if (achievementClassIdAchievementClassDictionary[achievementScoreLogDTO.AchievementClassId].AchievementScoreLogDTOList == null)
                        {
                            achievementClassIdAchievementClassDictionary[achievementScoreLogDTO.AchievementClassId].AchievementScoreLogDTOList = new List<AchievementScoreLogDTO>();
                        }
                        achievementClassIdAchievementClassDictionary[achievementScoreLogDTO.AchievementClassId].AchievementScoreLogDTOList.Add(achievementScoreLogDTO);
                    }
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the AchievementClasses List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null) // Got an inaccessible error.
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementClassDTOList == null ||
                achievementClassDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < achievementClassDTOList.Count; i++)
            {
                var achievementClassDTO = achievementClassDTOList[i];
                if (achievementClassDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    AchievementClass achievementClass = new AchievementClass(executionContext, achievementClassDTO);
                    achievementClass.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AchievementClassDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AchievementClassDTO", achievementClassDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}