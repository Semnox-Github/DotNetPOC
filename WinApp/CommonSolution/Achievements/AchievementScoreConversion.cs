/****************************************************************************************************************
 * Project Name - AchievementsBL
 * Description  - Bussiness logic of the   AchievementScoreConversion class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *****************************************************************************************************************
 *1.00        4-may-2017    Rakshith         Created 
 *2.70        4-jul-2019   Deeksha           Modified:Save() method for Insert /Update returns DTO instead of Id
*                                                     Added Execution Context object for Constructors.
*                                                     changed log.debug to log.logMethodEntry
 *                                                    and log.logMethodExit
 *2.80        27-Aug-2019   Vikas Dwivedi    Added Constructor with ExecutionContext as a Parameter
 *                                           in AchievementScoreConversion as well as AchievementScoreConversionsList,
 *                                           Added Constructor with ExecutionContext 
 *                                           and AchievementScoreConversionDTO in AchievementScoreConversion, 
 *                                           Added Added Constructor with ExecutionContext 
 *                                           and List<AchievementScoreConversion> in AchievementScoreConversionsList,
 *                                           Added SqlTransaction as a parameter in Save() of AchievementScoreConversion
 *2.80        19-Nov-2019   Vikas Dwivedi    Added Logger Method                                     
 ******************************************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using System.Linq;
using Semnox.Parafait.Promotions;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// Business Logic for AchievementScoreConversion
    /// </summary>
    public class AchievementScoreConversion
    {
        private AchievementScoreConversionDTO achievementScoreConversionDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AchievementScoreConversion
        /// </summary>

        private AchievementScoreConversion(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AchievementScoreConversionBL object using the AchievementScoreConversionDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="achievementScoreConversionDTO">AchievementScoreConversionDTO object</param>
        public AchievementScoreConversion(ExecutionContext executionContext, AchievementScoreConversionDTO achievementScoreConversionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementScoreConversionDTO);
            this.achievementScoreConversionDTO = achievementScoreConversionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the achievementScoreConversion id as the parameter
        /// Would fetch the achievementScoreConversion object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of AchievementScoreConversion Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AchievementScoreConversion(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AchievementScoreConversionDataHandler achievementScoreConversionDataHandler = new AchievementScoreConversionDataHandler(sqlTransaction);
            this.achievementScoreConversionDTO = achievementScoreConversionDataHandler.GetAchievementScoreConversionDTO(id);
            if (achievementScoreConversionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " AppUIElementParameterAttributeDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AchievementScoreConversion
        /// Checks if the achievementScoreConversion id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AchievementScoreConversionDataHandler achievementScoreConversionDatahandler = new AchievementScoreConversionDataHandler(sqlTransaction);
            if (achievementScoreConversionDTO.Id < 0)
            {
                log.LogVariableState("AchievementScoreConversionDTO", achievementScoreConversionDTO);
                achievementScoreConversionDTO = achievementScoreConversionDatahandler.InsertAchievementScoreConversion(achievementScoreConversionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementScoreConversionDTO.AcceptChanges();
            }
            else if (achievementScoreConversionDTO.IsChanged)
            {
                log.LogVariableState("AchievementScoreConversionDTO", achievementScoreConversionDTO);
                achievementScoreConversionDTO = achievementScoreConversionDatahandler.UpdateAchievementScoreConversion(achievementScoreConversionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                achievementScoreConversionDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AchievementScoreConversionDTO
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
        /// Gets AchievementScoreConversionDTO Object
        /// </summary>
        public AchievementScoreConversionDTO GetAchievementScoreConversionDTO
        {
            get { return achievementScoreConversionDTO; }
        }

        ///// <summary>
        ///// Delete the record AchievementScoreConversionDTO from the  database based on id
        ///// </summary>
        ///// <param name="id">int id </param>
        ///// <returns>return the int </returns>
        //public int Delete(int id, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(id);
        //    AchievementScoreConversionDataHandler achievementScoreConversionDatahandler = new AchievementScoreConversionDataHandler(sqlTransaction);
        //    int rowDeleted = achievementScoreConversionDatahandler.Delete(id);
        //    log.LogMethodExit(rowDeleted);
        //    return rowDeleted;
        //}
    }

    /// <summary>
    /// Manages the list of AchievementScoreConversions
    /// </summary>
    public class AchievementScoreConversionsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AchievementScoreConversionDTO> achievementScoreConversionsDTOList = new List<AchievementScoreConversionDTO>();

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public AchievementScoreConversionsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="achievementScoreConversionsDTOList"></param>
        public AchievementScoreConversionsList(ExecutionContext executionContext, List<AchievementScoreConversionDTO> achievementScoreConversionsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, achievementScoreConversionsDTOList);
            this.achievementScoreConversionsDTOList = achievementScoreConversionsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///Takes AchievementScoreConversionParams as parameter
        /// </summary>
        /// <returns>Returns List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>> by converting achievementScoreConversionParams</returns>
        public List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>> BuildAchievementScoreConversionSearchParametersList(AchievementScoreConversionParams achievementScoreConversionParams)
        {
            log.LogMethodEntry(achievementScoreConversionParams);
            List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>> achievementScoreConversionSearchParams = new List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>>();
            if (achievementScoreConversionParams != null)
            {
                if (achievementScoreConversionParams.AchievementClassLevelId > 0)
                    achievementScoreConversionSearchParams.Add(new KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>(AchievementScoreConversionDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID, achievementScoreConversionParams.AchievementClassLevelId.ToString()));
                if (achievementScoreConversionParams.Id > 0)
                    achievementScoreConversionSearchParams.Add(new KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>(AchievementScoreConversionDTO.SearchByParameters.ID, achievementScoreConversionParams.Id.ToString()));
                if (achievementScoreConversionParams.IsActive)
                    achievementScoreConversionSearchParams.Add(new KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>(AchievementScoreConversionDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            log.LogMethodExit(achievementScoreConversionSearchParams);
            return achievementScoreConversionSearchParams;
        }

        /// <summary>
        /// Returns the AchievementScoreConversions list
        /// </summary>
        public List<AchievementScoreConversionDTO> GetAllAchievementScoreConversions(List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AchievementScoreConversionDataHandler achievementScoreConversionDatahandler = new AchievementScoreConversionDataHandler(sqlTransaction);
            List<AchievementScoreConversionDTO> achievementScoreConversionDTOList = new List<AchievementScoreConversionDTO>();
            achievementScoreConversionDTOList = achievementScoreConversionDatahandler.GetAchievementScoreConversionList(searchParameters, sqlTransaction);
            log.LogMethodExit(achievementScoreConversionDTOList);
            return achievementScoreConversionDTOList;
        }

        /// <summary>
        /// Returns the AchievementScoreConversions list
        /// </summary>
        public List<AchievementScoreConversionDTO> GetAllAchievementScoreConversions(AchievementScoreConversionParams achievementScoreConversionParams, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(achievementScoreConversionParams, sqlTransaction);
                List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>> searchParameters = BuildAchievementScoreConversionSearchParametersList(achievementScoreConversionParams);
                AchievementScoreConversionDataHandler achievementScoreConversionDataHandler = new AchievementScoreConversionDataHandler(sqlTransaction);
                List<AchievementScoreConversionDTO> achievementscoreConversionDTOList = new List<AchievementScoreConversionDTO>();
                achievementscoreConversionDTOList = achievementScoreConversionDataHandler.GetAchievementScoreConversionList(searchParameters, sqlTransaction);
                log.LogMethodExit(achievementscoreConversionDTOList);
                return achievementscoreConversionDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Exception at GetAllAchievementScoreConversions()");
                log.LogMethodExit(null, "Throwing exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// get the record LoyaltyAttributesDTO list  
        /// </summary>
        /// <returns>return the list of LoyaltyAttributesDTO </returns>
        public List<LoyaltyAttributesDTO> GetLoyaltyAttributes(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            AchievementScoreConversionDataHandler achievementScoreConversionDataHandler = new AchievementScoreConversionDataHandler(sqlTransaction);
            List<LoyaltyAttributesDTO> LoyaltyAttributesDTOList = new List<LoyaltyAttributesDTO>();
            LoyaltyAttributesDTOList = achievementScoreConversionDataHandler.GetLoyaltyAttributes();
            log.LogMethodExit(LoyaltyAttributesDTOList);
            return LoyaltyAttributesDTOList;

        }

        /// <summary>
        /// Saves the AchievementScoreConversion List
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (achievementScoreConversionsDTOList == null ||
               achievementScoreConversionsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < achievementScoreConversionsDTOList.Count; i++)
            {
                var achievementScoreConversionsDTO = achievementScoreConversionsDTOList[i];
                if (achievementScoreConversionsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AchievementScoreConversion achievementScoreConversion = new AchievementScoreConversion(executionContext, achievementScoreConversionsDTO);
                    achievementScoreConversion.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AchievementScoreConversionsDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AchievementScoreConversionsDTOList", achievementScoreConversionsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}


