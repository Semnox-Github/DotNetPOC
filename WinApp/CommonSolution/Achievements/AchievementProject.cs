/********************************************************************************************
* Project Name - Achievements 
* Description  - Business Logic of AchievementProject  
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.70        03-JUl-2019    Deeksha                 Modified : Save() method for Insert /Update returns DTO instead of Id
*                                                              Added Execution Context object for Constructors.                                                         
*2.70        27-Aug-2019     Indrajeet Kumar     Modified- Added Parameterized Constructor, 
                                                 Created SaveUpdateAchievmentProjects() method.
*2.80        04-Mar-2020    Vikas Dwivedi        Modified as per the Standard for Phase 1 Changes.
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.Languages;
using System.Text;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// Bussiness Logic for AchievementProject
    /// </summary>
    public class AchievementProject
    {
        private AchievementProjectDTO achievementProjectDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AchievementProject class
        /// <param name="executionContext">execution context</param>
        /// </summary>
        private AchievementProject(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AchievementProjectBL object using the AchievementProjectDTO
        /// </summary>
        /// <param name="executionContext"></param>
        public AchievementProject(ExecutionContext executionContext, AchievementProjectDTO achievementProjectDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementProjectDTO);
            this.achievementProjectDTO = achievementProjectDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AchievementProject id as the parameter
        /// Would fetch the achievementProject object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id - AchievementProjectId</param>
        /// <param name="loadChildRecords">loadChildRecords either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AchievementProject(ExecutionContext executionContext, int achievementProjectId, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementProjectId, sqlTransaction);
            AchievementProjectDataHandler achievementProjectDataHandler = new AchievementProjectDataHandler(sqlTransaction);
            this.achievementProjectDTO = achievementProjectDataHandler.GetAchievementProjectDTO(achievementProjectId);
            if (achievementProjectDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AchievementProject", achievementProjectId);
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
        /// Builds the child records for AppUIPanel object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            AchievementClassesList achievementClassesList = new AchievementClassesList(executionContext);
            List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.ACHIEVEMENT_PROJECT_ID, achievementProjectDTO.AchievementProjectId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            achievementProjectDTO.AchievementClassDTOList = achievementClassesList.GetAchievementClassList(searchParameters, true, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AchievementProject
        /// Checks if the achievementProjectid is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementProjectDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to Save");
                return;
            }
            AchievementProjectDataHandler achievementProjectDataHandler = new AchievementProjectDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (achievementProjectDTO.AchievementProjectId < 0)
            {
                log.LogVariableState("AchievementProjectDTO", achievementProjectDTO);
                achievementProjectDTO = achievementProjectDataHandler.InsertAchievementProject(achievementProjectDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementProjectDTO.AcceptChanges();
            }
            else if (achievementProjectDTO.IsChanged)
            {
                achievementProjectDataHandler.UpdateAchievementProject(achievementProjectDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementProjectDTO.AcceptChanges();
            }
            SaveAchievementClass(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : AchievementClassDTOList 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveAchievementClass(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementProjectDTO.AchievementClassDTOList != null &&
                achievementProjectDTO.AchievementClassDTOList.Any())
            {
                List<AchievementClassDTO> updatedAchievementClassDTOList = new List<AchievementClassDTO>();
                foreach (var achievementClassDTO in achievementProjectDTO.AchievementClassDTOList)
                {
                    if (achievementClassDTO.AchievementProjectId != achievementProjectDTO.AchievementProjectId)
                    {
                        achievementClassDTO.AchievementProjectId = achievementClassDTO.AchievementProjectId;
                    }
                    if (achievementClassDTO.IsChangedRecursive)
                    {
                        updatedAchievementClassDTOList.Add(achievementClassDTO);
                    }
                }
                if (updatedAchievementClassDTOList.Any())
                {
                    AchievementClassesList achievementClassesList = new AchievementClassesList(executionContext, updatedAchievementClassDTOList);
                    achievementClassesList.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AchievementProjectDTO  ,AchievementClassDTOList - child 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;

            // Validation Logic here
            if (string.IsNullOrWhiteSpace(achievementProjectDTO.ProjectName))
            {
                validationErrorList.Add(new ValidationError("AchievementProject", "ProjectName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Project Name"))));
            }

            if (!string.IsNullOrWhiteSpace(achievementProjectDTO.ProjectName) && achievementProjectDTO.ProjectName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("AchievementProject", "ProjectName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Project Name"), 50)));
            }

            // validate Child list
            if (achievementProjectDTO.AchievementClassDTOList != null &&
                achievementProjectDTO.AchievementClassDTOList.Count > 0)
            {
                foreach (AchievementClassDTO achievementClassDTO in achievementProjectDTO.AchievementClassDTOList)
                {
                    AchievementClass achievementClass = new AchievementClass(executionContext, achievementClassDTO);
                    validationErrorList.AddRange(achievementClass.Validate());
                }
            }
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AchievementProjectDTO GetAchievementProjectDTO
        {
            get { return achievementProjectDTO; }
        }
    }

    /// <summary>
    /// Manages the list of AchievementProjects
    /// </summary>
    public class AchievementProjectsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AchievementProjectDTO> achievementProjectDTOList = new List<AchievementProjectDTO>();

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AchievementProjectsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="languagesList"></param>
        public AchievementProjectsList(ExecutionContext executionContext, List<AchievementProjectDTO> achievementProjectDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementProjectDTOList);
            this.achievementProjectDTOList = achievementProjectDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AchievementProjects list
        /// </summary>
        public List<AchievementProjectDTO> GetAchievementProjectsList(List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>> searchParameters,
                                                                            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            // child records needs to build
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AchievementProjectDataHandler achievementProjectDataHandler = new AchievementProjectDataHandler(sqlTransaction);
            List<AchievementProjectDTO> achievementProjectDTOList = achievementProjectDataHandler.GetAchievementProjectDTOList(searchParameters);
            if (achievementProjectDTOList !=null && achievementProjectDTOList.Any() && loadChildRecords)
            {
                Build(achievementProjectDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(achievementProjectDTOList);
            return achievementProjectDTOList;
        }

        /// <summary>
        /// Builds the List of AchievementProject object based on the list of AchievementProject id.
        /// </summary>
        /// <param name="achievementProjectDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<AchievementProjectDTO> achievementProjectDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(achievementProjectDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AchievementProjectDTO> achievementProjectIdAchievementProjectDictionary = new Dictionary<int, AchievementProjectDTO>();
            string achievementProjectIdSet;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < achievementProjectDTOList.Count; i++)
            {
                if (achievementProjectDTOList[i].AchievementProjectId == -1 ||
                    achievementProjectIdAchievementProjectDictionary.ContainsKey(achievementProjectDTOList[i].AchievementProjectId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(achievementProjectDTOList[i].AchievementProjectId);
                achievementProjectIdAchievementProjectDictionary.Add(achievementProjectDTOList[i].AchievementProjectId, achievementProjectDTOList[i]);
            }
            achievementProjectIdSet = sb.ToString();
            AchievementClassesList achievementClassesList = new AchievementClassesList(executionContext);
            List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.ACHIEVEMENT_PROJECT_ID_LIST, achievementProjectIdSet.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            List<AchievementClassDTO> achievementClassDTOList = achievementClassesList.GetAchievementClassList(searchParameters, true, activeChildRecords, sqlTransaction);
            if (achievementClassDTOList != null && achievementClassDTOList.Any())
            {
                log.LogVariableState("AchievementClassDTOList", achievementClassDTOList);
                foreach (var achievementClassDTO in achievementClassDTOList)
                {
                    if (achievementProjectIdAchievementProjectDictionary.ContainsKey(achievementClassDTO.AchievementProjectId))
                    {
                        if (achievementProjectIdAchievementProjectDictionary[achievementClassDTO.AchievementProjectId].AchievementClassDTOList == null)
                        {
                            achievementProjectIdAchievementProjectDictionary[achievementClassDTO.AchievementProjectId].AchievementClassDTOList = new List<AchievementClassDTO>();
                        }
                        achievementProjectIdAchievementProjectDictionary[achievementClassDTO.AchievementProjectId].AchievementClassDTOList.Add(achievementClassDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///Takes AchievementProjectParams as parameter
        /// </summary>
        /// <returns>Returns List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>> by converting achievementProjectParams</returns>
        public List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>> BuildAchievementProjectSearchParametersList(AchievementProjectParams achievementProjectParams)
        {
            log.LogMethodEntry(achievementProjectParams);
            List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>> achievementClassSearchParams = new List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>>();
            if (achievementProjectParams != null)
            {
                if (achievementProjectParams.AchievementProjectId > 0)
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementProjectDTO.SearchByParameters, string>(AchievementProjectDTO.SearchByParameters.ACHIEVEMENTPROJECT_ID, achievementProjectParams.AchievementProjectId.ToString()));


                if (achievementProjectParams.IsActive)
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementProjectDTO.SearchByParameters, string>(AchievementProjectDTO.SearchByParameters.IS_ACTIVE, "1"));


                if (!string.IsNullOrEmpty(achievementProjectParams.ProjectName))
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementProjectDTO.SearchByParameters, string>(AchievementProjectDTO.SearchByParameters.PROJECT_NAME, achievementProjectParams.ProjectName));

                if (achievementProjectParams.SiteId > 0)
                    achievementClassSearchParams.Add(new KeyValuePair<AchievementProjectDTO.SearchByParameters, string>(AchievementProjectDTO.SearchByParameters.SITE_ID, achievementProjectParams.SiteId.ToString()));
            }
            log.LogMethodExit(achievementClassSearchParams);

            return achievementClassSearchParams;
        }

        /// <summary>
        /// GetAchievementProjectsList(AchievementProjectParams achievementProjectParams) method search based on achievementProjectParams
        /// </summary>
        /// <param name="achievementProjectParams">AchievementProjectParams achievementProjectParams</param>
        /// <returns>List of AchievementProjectDTO object</returns>
        public List<AchievementProjectDTO> GetAchievementProjectsList(AchievementProjectParams achievementProjectParams, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(achievementProjectParams);
                List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>> searchParameters = BuildAchievementProjectSearchParametersList(achievementProjectParams);
                AchievementProjectDataHandler achievementProjectDataHandler = new AchievementProjectDataHandler(sqlTransaction);
                List<AchievementProjectDTO> achievementProjectDTOList = new List<AchievementProjectDTO>();
                achievementProjectDTOList = achievementProjectDataHandler.GetAchievementProjectDTOList(searchParameters);
                log.LogMethodExit(achievementProjectDTOList);
                return achievementProjectDTOList;
            }
            catch (Exception ex)
            {
                string message = "Exception at GetAchievementProjectsList()";
                log.LogMethodExit(null, "Throwing exception -" + ex.Message + ":" + message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Saves the list of AchievementProject DTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementProjectDTOList == null ||
                achievementProjectDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < achievementProjectDTOList.Count; i++)
            {
                var achievementProjectDTO = achievementProjectDTOList[i];
                if (achievementProjectDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    AchievementProject achievementProject = new AchievementProject(executionContext, achievementProjectDTO);
                    achievementProject.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving achievementProjectDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AchievementProjectDTO", achievementProjectDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
