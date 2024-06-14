/********************************************************************************************
 * Project Name - Achievements
 * Description  - Buisness Logic of AchievementLevel
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
*2.70        03-JUl-2019    Deeksha                 Modified : Save() method for Insert /Update returns DTO instead of Id
*                                                              Added Execution Context object for Constructors. 
*2.80        04-Mar-2020    Vikas Dwivedi           Modified as per the Standard for Phase 1 Changes.
 ********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// Business Logic for AchievementLevel
    /// </summary>
    public class AchievementLevel
    {
        private AchievementLevelDTO achievementLevelDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AchievementClassLevel class
        ///<param name="executionContext">execution context</param>
        /// </summary>
        public AchievementLevel(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AchievementLevelBL object using the AchievementLevelDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="achievementLevelDTO">achievementsLevel object</param>
        public AchievementLevel(ExecutionContext executionContext, AchievementLevelDTO achievementLevelDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementLevelDTO);
            this.achievementLevelDTO = achievementLevelDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the achievementLevel id as the parameter
        /// Would fetch the achievementLevel object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of AchievementLevel Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AchievementLevel(ExecutionContext executionContext, int achievementLevelId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, achievementLevelId, sqlTransaction);
            AchievementLevelDataHandler achievementLevelDataHandler = new AchievementLevelDataHandler(sqlTransaction);
            this.achievementLevelDTO = achievementLevelDataHandler.GetAchievementLevelDTO(achievementLevelId);
            if (achievementLevelDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " AchievementScoreLogDTO", achievementLevelId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets AchievementLevelDTO Object
        /// </summary>
        public AchievementLevelDTO GetAchievementLevelDTO
        {
            get { return achievementLevelDTO; }
        }

        /// <summary>
        /// Saves the AchievementLevel
        /// Checks if the achievementlevelid is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementLevelDTO.IsChanged == false &&
               achievementLevelDTO.Id > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            AchievementLevelDataHandler achievementLevelDataHandler = new AchievementLevelDataHandler(sqlTransaction);
            if (achievementLevelDTO.Id < 0)
            {
                log.LogVariableState("AchievementLevelDTO", achievementLevelDTO);
                achievementLevelDTO = achievementLevelDataHandler.InsertAchievementLevel(achievementLevelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementLevelDTO.AcceptChanges();
            }
            else if (achievementLevelDTO.IsChanged)
            {
                log.LogVariableState("AchievementLevelDTO", achievementLevelDTO);
                achievementLevelDTO = achievementLevelDataHandler.UpdateAchievementLevel(achievementLevelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementLevelDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AchievementLevelDTO
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
        /// Returns AchievementLevelExtended List
        /// </summary>
        /// <param name="achievementParams">achievementParams</param>
        /// <returns>AchievementLevelExtended List</returns>
        public List<AchievementLevelExtended> GetAchievementLevelListExtended(AchievementParams achievementParams)
        {
            log.LogMethodEntry(achievementParams);
            List<AchievementLevelExtended> achievementLevelExtendedList = new AchievementLevelDataHandler().GetAchievementLevelListExtended(achievementParams);
            log.LogMethodExit(achievementLevelExtendedList);
            return achievementLevelExtendedList;
        }

        /// <summary>
        /// UpdatePlayerAchivementLevel
        /// </summary>
        /// <param name="achievementParams"></param>
        public bool UpdatePlayerAchievementLevel(AchievementParams achievementParams)
        {
            log.LogMethodEntry(achievementParams);

            try
            {

                int playerScore = new PlayerAchievementDataHandler().GetPlayerAchievementScore(achievementParams);
                //Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();

                // Start Class Level Progression Check 

                //// Get Player Card Achivement class level for card
                //AchievementLevelParams achievementLevelParams = new AchievementLevelParams();
                //achievementLevelParams.IsValid = false;
                //achievementLevelParams.CardId = achievementParams.CardId;
                //List<AchievementLevelDTO> achievementLevelDTOList = new AchievementLevelsList().GetAllAchievementLevels(achievementLevelParams);


                // Get Achivement class level List for Progression check 
                AchievementClassLevelParams achievementClassLevelParams = new AchievementClassLevelParams();
                achievementClassLevelParams.AchievementClassId = achievementParams.AchievementClassId;
                achievementClassLevelParams.IsActive = true;
                achievementClassLevelParams.SiteId = executionContext.GetSiteId();

                AchievementClassLevelDataHandler achClassLevelDataHandler = new AchievementClassLevelDataHandler();
                List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>> achClassLevelSearchList;

                achClassLevelSearchList = new AchievementClassLevelsList(executionContext).BuildAchievementClassLevelSearchParametersList(achievementClassLevelParams);
                List<AchievementClassLevelDTO> achievementClassLevelDTOList = achClassLevelDataHandler.GetAchievementClassLevelsList(achClassLevelSearchList, null).OrderBy(c => c.QualifyingScore).ToList();

                int playerNewAchievementClassLevelId = -1;

                AchievementLevelParams achievementLevelParams = new AchievementLevelParams();
                achievementLevelParams.IsValid = false;
                achievementLevelParams.CardId = achievementParams.CardId;
                List<AchievementLevelDTO> achievementLevelDTOList = new List<AchievementLevelDTO>();

                AchievementLevelsList achievementLevelsList = new AchievementLevelsList(executionContext);

                foreach (AchievementClassLevelDTO achClassLevelDTO in achievementClassLevelDTOList)
                {
                    if (playerScore >= achClassLevelDTO.QualifyingScore || achClassLevelDTO.QualifyingScore == 0)
                    {
                        // Insert Achivement progression level after check
                        bool IsQualified = true;

                        if (achClassLevelDTO.RegistrationRequired && achievementParams.CustomerId == -1)
                        {
                            IsQualified = false;
                        }
                        else if (achClassLevelDTO.QualifyingLevelId > 0)
                        {

                            achievementLevelParams.AchievementClassLevelId = achClassLevelDTO.QualifyingLevelId;
                            achievementLevelDTOList = achievementLevelsList.GetAllAchievementLevels(achievementLevelParams);

                            if (achievementLevelDTOList.Where(c => c.AchievementClassLevelId == achClassLevelDTO.QualifyingLevelId).Count() == 0)
                                IsQualified = false;

                        }

                        if (!IsQualified)
                            break;

                        achievementLevelParams.AchievementClassLevelId = achClassLevelDTO.AchievementClassLevelId;
                        achievementLevelDTOList = achievementLevelsList.GetAllAchievementLevels(achievementLevelParams);
                        if (achievementLevelDTOList.Where(c => c.AchievementClassLevelId == achClassLevelDTO.AchievementClassLevelId).Count() == 0)
                        {
                            AchievementLevelDTO achLevelDTO = new AchievementLevelDTO();
                            achLevelDTO.CardId = achievementParams.CardId;
                            achLevelDTO.AchievementClassLevelId = achClassLevelDTO.AchievementClassLevelId;
                            achLevelDTO.IsValid = true;
                            achLevelDTO.EffectiveDate = DateTime.Now;

                            AchievementLevel achievementLevel = new AchievementLevel(executionContext, achLevelDTO);
                            achievementLevel.Save();

                            playerNewAchievementClassLevelId = achClassLevelDTO.AchievementClassLevelId;

                        }

                    }
                }

                // Check New Achievement Level Inserted and make old levels InValid
                if (playerNewAchievementClassLevelId > 0)
                {
                    foreach (AchievementClassLevelDTO achClassLevelDTO in achievementClassLevelDTOList)
                    {
                        if (achClassLevelDTO.AchievementClassLevelId != playerNewAchievementClassLevelId)
                        {
                            achievementLevelParams.AchievementClassLevelId = achClassLevelDTO.AchievementClassLevelId;
                            achievementLevelParams.IsValid = true;
                            achievementLevelDTOList = achievementLevelsList.GetAllAchievementLevels(achievementLevelParams);
                            if (achievementLevelDTOList.Where(c => c.AchievementClassLevelId == achClassLevelDTO.AchievementClassLevelId).Count() == 1)
                            {
                                int achievementLevelId = achievementLevelDTOList.Where(c => c.AchievementClassLevelId == achClassLevelDTO.AchievementClassLevelId).First().Id;
                                AchievementLevelDTO achievementLevelDTO = new AchievementLevel(executionContext, achievementLevelId).GetAchievementLevelDTO;
                                if (achievementLevelDTO.Id > 0 && achievementLevelDTO.IsValid == true)
                                {
                                    achievementLevelDTO.IsValid = false;
                                    new AchievementLevel(executionContext, achievementLevelDTO).Save();
                                }
                            }
                        }
                    }
                }

                log.LogMethodExit(true);
                return true;
            }
            catch
            {
                //throw;
                return false;
            }
        }

    }


    /// <summary>
    /// Manages the list of AchievementLevels
    /// </summary>
    public class AchievementLevelsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AchievementLevelDTO> achievementLevelDTOList = new List<AchievementLevelDTO>();

        /// <summary>
        /// Parameterized Constructor of AchievementLevelsListBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public AchievementLevelsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrize Constructor
        /// </summary>
        /// <param name="executionContext">achievementLevelDTOList</param>
        /// <param name="achievementLevelDTOList">achievementLevelDTOList</param>
        public AchievementLevelsList(ExecutionContext executionContext, List<AchievementLevelDTO> achievementLevelDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementLevelDTOList);
            this.achievementLevelDTOList = achievementLevelDTOList;
            log.LogMethodExit();
        }

        public List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>> BuildAchievementLevelSearchParametersList(AchievementLevelParams achievementLevelParams)
        {
            log.LogMethodEntry(achievementLevelParams);
            List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>> achievementLevelSearchParams = new List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>>();
            if (achievementLevelParams != null)
            {
                if (achievementLevelParams.Id > 0)
                    achievementLevelSearchParams.Add(new KeyValuePair<AchievementLevelDTO.SearchByParameters, string>(AchievementLevelDTO.SearchByParameters.ID, achievementLevelParams.Id.ToString()));
                if (achievementLevelParams.AchievementClassLevelId > 0)
                    achievementLevelSearchParams.Add(new KeyValuePair<AchievementLevelDTO.SearchByParameters, string>(AchievementLevelDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID, achievementLevelParams.AchievementClassLevelId.ToString()));
                if (achievementLevelParams.CardId > 0)
                    achievementLevelSearchParams.Add(new KeyValuePair<AchievementLevelDTO.SearchByParameters, string>(AchievementLevelDTO.SearchByParameters.CARD_ID, achievementLevelParams.CardId.ToString()));
                if (achievementLevelParams.IsValid)
                    achievementLevelSearchParams.Add(new KeyValuePair<AchievementLevelDTO.SearchByParameters, string>(AchievementLevelDTO.SearchByParameters.ISVALID, "1"));

            }
            log.LogMethodExit(achievementLevelSearchParams);
            return achievementLevelSearchParams;
        }


        /// <summary>
        /// Returns the AchievementLevel DTO 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>AchievementLevelDTO</returns>
        public AchievementLevelDTO GetAchievementLevel(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            AchievementLevelDataHandler achievementLevelDataHandler = new AchievementLevelDataHandler(sqlTransaction);
            AchievementLevelDTO achievementLevelDTO = new AchievementLevelDTO();
            achievementLevelDTO = achievementLevelDataHandler.GetAchievementLevelDTO(id);
            log.LogMethodExit(achievementLevelDTO);
            return achievementLevelDTO;
        }

        /// <summary>
        /// Returns AchievementLevelDTO List
        /// </summary>
        /// <param name="achievementLevelParams">achievementLevelParams</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>AchievementLevelDTO List</returns>
        public List<AchievementLevelDTO> GetAllAchievementLevels(AchievementLevelParams achievementLevelParams, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(achievementLevelParams);
                List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>> searchParameters = BuildAchievementLevelSearchParametersList(achievementLevelParams);
                AchievementLevelDataHandler achievementLevelDataHandler = new AchievementLevelDataHandler(sqlTransaction);
                List<AchievementLevelDTO> achievementLevelDTOList = new List<AchievementLevelDTO>();
                achievementLevelDTOList = achievementLevelDataHandler.GetAchievementClassLevelsList(searchParameters);
                log.LogMethodExit(achievementLevelDTOList);
                return achievementLevelDTOList;
            }
            catch (Exception ex)
            {
                string message = "Exception at GetAllAchievementLevels()";
                log.LogMethodExit(null, "Throwing exception -" + ex.Message + ":" + message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Returns AchievementLevelDTO List
        /// </summary>
        /// <param name="searchParams"searchParams></param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>AchievementLevelDTO List</returns>
        public List<AchievementLevelDTO> GetAllAchievementLevels(List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>> searchParams, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(searchParams, sqlTransaction);
                AchievementLevelDataHandler achievementLevelDataHandler = new AchievementLevelDataHandler(sqlTransaction);
                List<AchievementLevelDTO> achievementLevelDTOList = new List<AchievementLevelDTO>();
                achievementLevelDTOList = achievementLevelDataHandler.GetAchievementClassLevelsList(searchParams);
                log.LogMethodExit(achievementLevelDTOList);
                return achievementLevelDTOList;
            }
            catch (Exception ex)
            {
                string message = "Exception at GetAllAchievementLevels()";
                log.LogMethodExit(null, "Throwing exception -" + ex.Message + ":" + message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the AchievementLevel List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementLevelDTOList == null ||
                achievementLevelDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < achievementLevelDTOList.Count; i++)
            {
                var achievementLevelDTO = achievementLevelDTOList[i];
                if (achievementLevelDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AchievementLevel achievementLevel = new AchievementLevel(executionContext, achievementLevelDTO);
                    achievementLevel.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AchievementLevelDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AchievementLevelDTO", achievementLevelDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }

}
