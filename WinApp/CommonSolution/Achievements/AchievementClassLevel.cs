/*****************************************************************************************************************
 * Project Name - Achievements
 * Description  - Bussiness Logic of AchievementClassLevel
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *****************************************************************************************************************
 *2.70        03-JUl-2019    Deeksha                 Modified : Modified : Save() method for Insert /Update returns DTO instead of Id
*                                                              Added Execution Context object for Constructors.  
*                                                              changed log.debug to log.logMethodEntry
*                                                              and log.logMethodExit
 *2.80        27-Aug-2019   Vikas Dwivedi         Added Constructor with ExecutionContext as a Parameter
 *                                                in AchievementClassLevel as well as AchievementClassLevelsList,
 *                                                Added Constructor with ExecutionContext 
 *                                                and AchievementClassLevelDTO in AchievementClassLevel as well as
 *                                                Added Constructor with ExecutionContext 
 *                                                and List<AcheievementClasslLevelDTO> in  AchievementClassLevelsList, 
 *                                                Added SqlTransaction as a parameter in Save() of AchievementClassLevel
 *2.80        19-Nov-2019   Vikas Dwivedi         Added Logger method  
 *2.80        04-Mar-2020   Vikas Dwivedi         Modified as per the Standard for Phase 1 Changes.
 *****************************************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using System.Linq;
using System.Text;
using Semnox.Parafait.Achievements;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// Business Logic For AchievementClassLevel
    /// </summary>
    public class AchievementClassLevel
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private AchievementClassLevelDTO achievementClassLevelDTO;

        /// <summary>
        ///  Parameterized constructor of AchievementClassLevel class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        private AchievementClassLevel(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AchievementClassLevelBL object using the AchievementClassLevelDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="achievementClassLevelDTO">achievementClassLevel object</param>
        public AchievementClassLevel(ExecutionContext executionContext, AchievementClassLevelDTO achievementClassLevelDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementClassLevelDTO);
            this.achievementClassLevelDTO = achievementClassLevelDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the achievementClassLevelId parameter
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="achievementClassLevelId">achievementClassLevelId</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public AchievementClassLevel(ExecutionContext executionContext, int achievementClassLevelId, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementClassLevelId, sqlTransaction);
            AchievementClassLevelDataHandler achievementClassLevelDataHandler = new AchievementClassLevelDataHandler(sqlTransaction);
            this.achievementClassLevelDTO = achievementClassLevelDataHandler.GetAchievementClassLevelDTO(achievementClassLevelId);
            if (achievementClassLevelDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AchievementClassLevel", achievementClassLevelId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (achievementClassLevelDTO != null && loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for AchievementClassLevel object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            AchievementScoreConversionsList achievementScoreConversionsList = new AchievementScoreConversionsList(executionContext);
            List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>> searchAchievementScoreConversionParams = new List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>>();
            searchAchievementScoreConversionParams.Add(new KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>(AchievementScoreConversionDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID, achievementClassLevelDTO.AchievementClassLevelId.ToString()));
            if (activeChildRecords)
            {
                searchAchievementScoreConversionParams.Add(new KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>(AchievementScoreConversionDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            achievementClassLevelDTO.AchievementScoreConversionDTOList = achievementScoreConversionsList.GetAllAchievementScoreConversions(searchAchievementScoreConversionParams, sqlTransaction);

            AchievementLevelsList achievementLevelsList = new AchievementLevelsList(executionContext);
            List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>> searchAchievementLevelParams = new List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>>();
            searchAchievementLevelParams.Add(new KeyValuePair<AchievementLevelDTO.SearchByParameters, string>(AchievementLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID, achievementClassLevelDTO.AchievementClassLevelId.ToString()));
            if (activeChildRecords)
            {
                searchAchievementLevelParams.Add(new KeyValuePair<AchievementLevelDTO.SearchByParameters, string>(AchievementLevelDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            achievementClassLevelDTO.AchievementLevelList = achievementLevelsList.GetAllAchievementLevels(searchAchievementLevelParams, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AchievementClassLevel
        /// Checks if the AchievementClassLevelId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementClassLevelDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation failed", validationErrorList);
            }
            AchievementClassLevelDataHandler achievementClassLevelDataHandler = new AchievementClassLevelDataHandler(sqlTransaction);
            if (achievementClassLevelDTO.AchievementClassLevelId < 0)
            {
                log.LogVariableState("AchievementClassLevelDTO", achievementClassLevelDTO);
                achievementClassLevelDTO = achievementClassLevelDataHandler.InsertAchievementClassLevelDTO(achievementClassLevelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementClassLevelDTO.AcceptChanges();
            }
            else if (achievementClassLevelDTO.IsChanged)
            {
                log.LogVariableState("AchievementClassLevelDTO", achievementClassLevelDTO);
                achievementClassLevelDTO = achievementClassLevelDataHandler.UpdateAchievementClassLevel(achievementClassLevelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementClassLevelDTO.AcceptChanges();
            }
            SaveAchievementScoreConversion(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : AchievementScoreConversionDTOList and AchievementLevelDTOList
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        private void SaveAchievementScoreConversion(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            // For child records : AchievementScoreConversion
            if (achievementClassLevelDTO.AchievementScoreConversionDTOList != null &&
                achievementClassLevelDTO.AchievementScoreConversionDTOList.Any())
            {
                List<AchievementScoreConversionDTO> updatedAchievementScoreConversionDTOList = new List<AchievementScoreConversionDTO>();
                foreach (var achievementScoreConversionDTO in achievementClassLevelDTO.AchievementScoreConversionDTOList)
                {
                    if (achievementScoreConversionDTO.AchievementClassLevelId != achievementClassLevelDTO.AchievementClassLevelId)
                    {
                        achievementScoreConversionDTO.AchievementClassLevelId = achievementClassLevelDTO.AchievementClassLevelId;
                    }
                    if (achievementScoreConversionDTO.IsChanged)
                    {
                        updatedAchievementScoreConversionDTOList.Add(achievementScoreConversionDTO);
                    }
                }
                if (updatedAchievementScoreConversionDTOList.Any())
                {
                    log.LogVariableState("UpdatedAchievementScoreConversionDTOList", updatedAchievementScoreConversionDTOList);
                    AchievementScoreConversionsList achievementScoreConversionsListBL = new AchievementScoreConversionsList(executionContext, updatedAchievementScoreConversionDTOList);
                    achievementScoreConversionsListBL.Save(sqlTransaction);
                }
            }
            // For child records : AchievementLevel
            if (achievementClassLevelDTO.AchievementLevelList != null &&
                achievementClassLevelDTO.AchievementLevelList.Any())
            {
                List<AchievementLevelDTO> updatedAchievementLevelDTOList = new List<AchievementLevelDTO>();
                foreach (var achievementLevelDTO in achievementClassLevelDTO.AchievementLevelList)
                {
                    if (achievementLevelDTO.AchievementClassLevelId != achievementClassLevelDTO.AchievementClassLevelId)
                    {
                        achievementLevelDTO.AchievementClassLevelId = achievementClassLevelDTO.AchievementClassLevelId;
                    }
                    if (achievementLevelDTO.IsChanged)
                    {
                        updatedAchievementLevelDTOList.Add(achievementLevelDTO);
                    }
                }
                if (updatedAchievementLevelDTOList.Any())
                {
                    AchievementLevelsList achievementLevelsListBL = new AchievementLevelsList(executionContext, updatedAchievementLevelDTOList);
                    achievementLevelsListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AchievementClassLevelDTO, AchievementScoreConversionDTOList and AchievementLevelDTOList - child 
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
            if (string.IsNullOrWhiteSpace(achievementClassLevelDTO.AchievementClassLevelId.ToString()))
            {
                validationErrorList.Add(new ValidationError("AchievementClassLevel", "AchievementClassLevelId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Achievement Class Level Id"))));
            }

            if (!string.IsNullOrWhiteSpace(achievementClassLevelDTO.AchievementClassLevelId.ToString()) && achievementClassLevelDTO.AchievementClassLevelId.ToString().Length > 50)
            {
                validationErrorList.Add(new ValidationError("AchievementClassLevel", "AchievementClassLevelId", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Achievement Class Level Id"), 50)));
            }

            // Validate Child List 
            if (achievementClassLevelDTO.AchievementLevelList != null &&
               achievementClassLevelDTO.AchievementLevelList.Count > 0)
            {
                foreach (AchievementLevelDTO achievementLevelDTO in achievementClassLevelDTO.AchievementLevelList)
                {
                    AchievementLevel achievementLevelBL = new AchievementLevel(executionContext, achievementLevelDTO);
                    validationErrorList.AddRange(achievementLevelBL.Validate(sqlTransaction));
                }
            }
            // validate Child list
            if (achievementClassLevelDTO.AchievementScoreConversionDTOList != null &&
               achievementClassLevelDTO.AchievementScoreConversionDTOList.Count > 0)
            {
                foreach (AchievementScoreConversionDTO achievementScoreConversionDTO in achievementClassLevelDTO.AchievementScoreConversionDTOList)
                {
                    AchievementScoreConversion achievementScoreConversionBL = new AchievementScoreConversion(executionContext, achievementScoreConversionDTO);
                    validationErrorList.AddRange(achievementScoreConversionBL.Validate(sqlTransaction));
                }
            }
            return validationErrorList;
        }

        /// <summary>
        /// Gets AchievementClassLevelDTO Object
        /// </summary>
        public AchievementClassLevelDTO GetAchievementClassLevelDTO
        {
            get { return achievementClassLevelDTO; }
        }

        ///// <summary>
        ///// Delete the record from the achievementClassLevel database based on achievementClassLevelId
        ///// </summary>
        ///// <returns>return the int </returns>
        //public int Delete(int achievementClassLevelId, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(achievementClassLevelId, sqlTransaction);
        //    int id;
        //    AchievementClassLevelDataHandler achievementClassLevelDataHandler = new AchievementClassLevelDataHandler(sqlTransaction);
        //    id = achievementClassLevelDataHandler.Delete(achievementClassLevelId);
        //    log.LogMethodExit(id);
        //    return id;
    }
}

/// <summary>
/// Manages the list of AchievementClassLevel
/// </summary>
public class AchievementClassLevelsList
{
    private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private readonly ExecutionContext executionContext;
    private List<AchievementClassLevelDTO> achievementClassLevelDTOList = new List<AchievementClassLevelDTO>();

    /// <summary>
    /// Parametrized Constructor with ExecutionContext
    /// </summary>
    /// <param name="executionContext"></param>
    public AchievementClassLevelsList(ExecutionContext executionContext)
    {
        log.LogMethodEntry(executionContext);
        this.executionContext = executionContext;
        log.LogMethodExit();
    }

    /// <summary>
    /// Parameterized Constructor with ExecutionContext and DTO Parameter
    /// </summary>
    /// <param name="executionContext"></param>
    /// <param name="achievementClassLevelDTOList"></param>
    public AchievementClassLevelsList(ExecutionContext executionContext, List<AchievementClassLevelDTO> achievementClassLevelDTOList)
        : this(executionContext)
    {
        log.LogMethodEntry(executionContext, achievementClassLevelDTOList);
        this.achievementClassLevelDTOList = achievementClassLevelDTOList;
        log.LogMethodExit();
    }

    ///// <summary>
    ///// This method is called from the Parent class AppUIPanelBL.
    ///// Returns the List of AppUIPanelElementDTO based in the AppUIPanel Id list.
    ///// </summary>
    ///// <param name="achievementClassIdList">appUIPanelIdList holds the list of AppUIPanel Id </param>
    ///// <param name="loadChildRecords">loadChildRecords holds either true or false</param>
    ///// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
    ///// <param name="sqlTransaction">SqlTransaction object</param>
    ///// <returns>The list of AppUIPanelElementDTO</returns>
    //internal List<AchievementClassLevelDTO> GetAchievementClassLevelList(List<int> achievementClassIdList,
    //                                                                 bool loadChildRecords = false,
    //                                                                 bool activeChildRecords = true,
    //                                                                 SqlTransaction sqlTransaction = null)
    //{
    //    log.LogMethodEntry(achievementClassIdList, loadChildRecords, activeChildRecords, sqlTransaction);
    //    AchievementClassLevelDataHandler achievementClassLevelDataHandler = new AchievementClassLevelDataHandler(sqlTransaction);
    //    List<AchievementClassLevelDTO> achievementClassLevelDTOList = achievementClassLevelDataHandler.GetAchievementClassLevelsList(achievementClassIdList, activeChildRecords);
    //    if (loadChildRecords && achievementClassLevelDTOList.Any())
    //    {
    //        Build(achievementClassLevelDTOList, activeChildRecords, sqlTransaction);
    //    }
    //    log.LogMethodExit(achievementClassLevelDTOList);
    //    return achievementClassLevelDTOList;
    //}

    /// <summary>
    /// Returns the AchievementClassLevel list
    /// </summary>
    /// <param name="searchParameters">searchParameters</param>
    /// <param name="loadChildRecords">loadChildRecords</param>
    /// <param name="activeChildRecords">activeChildRecords</param>
    /// <param name="sqlTransaction">sqlTransaction</param>
    /// <returns>List of AchievementClassLevelDTO</returns>
    public List<AchievementClassLevelDTO> GetAchievementClassLevelList(List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>> searchParameters,
        bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
    {
        // child records needs to build
        log.LogMethodEntry(searchParameters, sqlTransaction);
        AchievementClassLevelDataHandler achievementClassLevelDataHandler = new AchievementClassLevelDataHandler(sqlTransaction);
        List<AchievementClassLevelDTO> achievementClassLevelDTOList = achievementClassLevelDataHandler.GetAchievementClassLevelsList(searchParameters, sqlTransaction);
        if (achievementClassLevelDTOList !=null && achievementClassLevelDTOList.Any() && loadChildRecords)
        {
            Build(achievementClassLevelDTOList, activeChildRecords, sqlTransaction);
        }
        log.LogMethodExit(achievementClassLevelDTOList);
        return achievementClassLevelDTOList;
    }

    /// <summary>
    /// Builds the List of AchievementClass object based on the list of AchievementClass id.
    /// </summary>
    /// <param name="achievementClassLevelDTOList"></param>
    /// <param name="activeChildRecords"></param>
    /// <param name="sqlTransaction"></param>
    private void Build(List<AchievementClassLevelDTO> achievementClassLevelDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(achievementClassLevelDTOList, achievementClassLevelDTOList, sqlTransaction);
        Dictionary<int, AchievementClassLevelDTO> achievementClassLevelIdAchievementClassLevelDictionary = new Dictionary<int, AchievementClassLevelDTO>();
        StringBuilder sb = new StringBuilder();
        string achievementClassLevelIdSet;
        for (int i = 0; i < achievementClassLevelDTOList.Count; i++)
        {
            if (achievementClassLevelDTOList[i].AchievementClassLevelId == -1 ||
                achievementClassLevelIdAchievementClassLevelDictionary.ContainsKey(achievementClassLevelDTOList[i].AchievementClassLevelId))
            {
                continue;
            }
            if (i != 0)
            {
                sb.Append(",");
            }
            sb.Append(achievementClassLevelDTOList[i].AchievementClassLevelId);
            achievementClassLevelIdAchievementClassLevelDictionary.Add(achievementClassLevelDTOList[i].AchievementClassLevelId, achievementClassLevelDTOList[i]);
        }
        achievementClassLevelIdSet = sb.ToString();

        // Build child records - AchievementScoreConversions
        AchievementScoreConversionsList achievementScoreConversionsList = new AchievementScoreConversionsList(executionContext);
        List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>> searchAchievementScoreConversionParams = new List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>>();
        searchAchievementScoreConversionParams.Add(new KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>(AchievementScoreConversionDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID_LIST, achievementClassLevelIdSet.ToString()));
        if (activeChildRecords)
        {
            searchAchievementScoreConversionParams.Add(new KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>(AchievementScoreConversionDTO.SearchByParameters.IS_ACTIVE, "1"));
        }
        List<AchievementScoreConversionDTO> achievementScoreConversionsDTOList = achievementScoreConversionsList.GetAllAchievementScoreConversions(searchAchievementScoreConversionParams, sqlTransaction);
        if (achievementScoreConversionsDTOList != null && achievementScoreConversionsDTOList.Any())
        {
            log.LogVariableState("AchievementScoreConversionsDTOList", achievementScoreConversionsDTOList);
            foreach (var achievementScoreConversionsDTO in achievementScoreConversionsDTOList)
            {
                if (achievementClassLevelIdAchievementClassLevelDictionary.ContainsKey(achievementScoreConversionsDTO.AchievementClassLevelId))
                {
                    if (achievementClassLevelIdAchievementClassLevelDictionary[achievementScoreConversionsDTO.AchievementClassLevelId].AchievementScoreConversionDTOList == null)
                    {
                        achievementClassLevelIdAchievementClassLevelDictionary[achievementScoreConversionsDTO.AchievementClassLevelId].AchievementScoreConversionDTOList = new List<AchievementScoreConversionDTO>();
                    }
                    achievementClassLevelIdAchievementClassLevelDictionary[achievementScoreConversionsDTO.AchievementClassLevelId].AchievementScoreConversionDTOList.Add(achievementScoreConversionsDTO);
                }
            }
        }

        // Build child records - AchievementLevel
        AchievementLevelsList achievementLevelsList = new AchievementLevelsList(executionContext);
        List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>> searchAchievementLevelParams = new List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>>();
        searchAchievementLevelParams.Add(new KeyValuePair<AchievementLevelDTO.SearchByParameters, string>(AchievementLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID_LIST, achievementClassLevelIdSet.ToString()));
        if (activeChildRecords)
        {
            searchAchievementLevelParams.Add(new KeyValuePair<AchievementLevelDTO.SearchByParameters, string>(AchievementLevelDTO.SearchByParameters.IS_ACTIVE, "1"));
        }
        List<AchievementLevelDTO> achievementLevelDTOList = achievementLevelsList.GetAllAchievementLevels(searchAchievementLevelParams, sqlTransaction);
        if (achievementLevelDTOList != null && achievementLevelDTOList.Any())
        {
            log.LogVariableState("AchievmentLevelDTOList", achievementLevelDTOList);
            foreach (var achievementLevelDTO in achievementLevelDTOList)
            {
                if (achievementClassLevelIdAchievementClassLevelDictionary.ContainsKey(achievementLevelDTO.AchievementClassLevelId))
                {
                    if (achievementClassLevelIdAchievementClassLevelDictionary[achievementLevelDTO.AchievementClassLevelId].AchievementLevelList == null)
                    {
                        achievementClassLevelIdAchievementClassLevelDictionary[achievementLevelDTO.AchievementClassLevelId].AchievementLevelList = new List<AchievementLevelDTO>();
                    }
                    achievementClassLevelIdAchievementClassLevelDictionary[achievementLevelDTO.AchievementClassLevelId].AchievementLevelList.Add(achievementLevelDTO);
                }
            }
        }

        log.LogMethodExit();
    }

    /// <summary>
    ///Takes LookupParams as parameter
    /// </summary>
    /// <returns>Returns List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>> by converting achievementClassLevelParams</returns>

    public List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>> BuildAchievementClassLevelSearchParametersList(AchievementClassLevelParams achievementClassLevelParams)
    {
        log.LogMethodEntry(achievementClassLevelParams);
        List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>> achievementClassSearchParams = new List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>>();
        if (achievementClassLevelParams != null)
        {
            if (achievementClassLevelParams.AchievementClassLevelId > 0)
                achievementClassSearchParams.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID, achievementClassLevelParams.AchievementClassLevelId.ToString()));
            if (!string.IsNullOrEmpty(achievementClassLevelParams.LevelName))
                achievementClassSearchParams.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.LEVEL_NAME, achievementClassLevelParams.LevelName));
            if (achievementClassLevelParams.AchievementClassId > 0)
                achievementClassSearchParams.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_ID, achievementClassLevelParams.AchievementClassId.ToString()));

            if (achievementClassLevelParams.ParentLevelId > 0)
                achievementClassSearchParams.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.PARENT_LEVEL_ID, achievementClassLevelParams.ParentLevelId.ToString()));

            if (achievementClassLevelParams.QualifyingLevelId > 0)
                achievementClassSearchParams.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.QUALIFYING_LEVEL_ID, achievementClassLevelParams.QualifyingLevelId.ToString()));
            if (achievementClassLevelParams.RegistrationRequired)
                achievementClassSearchParams.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.REGISTRATION_REQUIRED, "1"));

            if (achievementClassLevelParams.IsActive)
                achievementClassSearchParams.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.IS_ACTIVE, "1"));
            if (achievementClassLevelParams.SiteId > 0)
                achievementClassSearchParams.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.SITE_ID, achievementClassLevelParams.SiteId.ToString()));
        }
        log.LogMethodExit(achievementClassSearchParams);
        return achievementClassSearchParams;
    }

    /// <summary>
    /// Returns the AchievementClassLevelDTO list
    /// </summary>
    public List<AchievementClassLevelDTO> GetAchievementClassLevelList(AchievementClassLevelParams achievementClassLevelParams, SqlTransaction sqlTransaction = null)
    {
        try
        {
            log.LogMethodEntry(achievementClassLevelParams, sqlTransaction);
            List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>> searchParameters = BuildAchievementClassLevelSearchParametersList(achievementClassLevelParams);
            AchievementClassLevelDataHandler achievementClassLevelDataHandler = new AchievementClassLevelDataHandler(sqlTransaction);
            List<AchievementClassLevelDTO> achievementClassLevelDTOList = new List<AchievementClassLevelDTO>();
            achievementClassLevelDTOList = achievementClassLevelDataHandler.GetAchievementClassLevelsList(searchParameters, sqlTransaction);
            log.LogMethodExit(achievementClassLevelDTOList);
            return achievementClassLevelDTOList;
        }
        catch (Exception ex)
        {
            log.Error("Exception at GetAchievementClassLevelList()");
            log.LogMethodExit(null, "Throwing exception -" + ex.Message);
            throw;
        }
    }

    /// <summary>
    /// This method should be called from the Parent Class BL method Save().
    /// Saves the AchievementClasses List
    /// Checks if the  id is not less than or equal to 0
    /// If it is less than or equal to 0, then inserts
    /// else updates
    /// </summary>
    /// <param name="sqlTransaction">SqlTransaction</param>
    internal void Save(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        if (achievementClassLevelDTOList == null ||
            achievementClassLevelDTOList.Any() == false)
        {
            log.LogMethodExit(null, "List is empty");
            return;
        }

        for (int i = 0; i < achievementClassLevelDTOList.Count; i++)
        {
            var achievementClassLevelDTO = achievementClassLevelDTOList[i];
            if (achievementClassLevelDTO.IsChangedRecursive == false)
            {
                continue;
            }
            try
            {
                AchievementClassLevel achievementClassLevel = new AchievementClassLevel(executionContext, achievementClassLevelDTO);
                achievementClassLevel.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while saving AchievementClassLevelDTO.", ex);
                log.LogVariableState("Record Index ", i);
                log.LogVariableState("AchievementClassLevelDTO", achievementClassLevelDTO);
                throw;
            }
        }
        log.LogMethodExit();
    }
}
