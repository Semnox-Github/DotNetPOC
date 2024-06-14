/********************************************************************************************
 * Project Name - Screen Setup
 * Description  - Bussiness logic of Screen Setup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        08-03-2017   Raghuveera          Created
 *2.40        28-Sep-2018  Jagan Mohan         Added new constructor ScreenSetupList and 
 *                                             methods SaveUpdateScreenSetupList
 *2.70.2        31-Jul-2019  Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
*2.100.0        13-Aug-2020      Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, Build() to get child records and 
 *                                                 List class changes as per 3 tier standards.
 *2.110.0     01-Dec-2020       Prajwal S          Modified : Constructor with Id parameter                                                
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Business logic for screen setup
    /// </summary>
    public class ScreenSetup
    {
        private ScreenSetupDTO screenSetupDTO;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ScreenSetup(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.screenSetupDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>  
        /// <param name="executionContext">executionContext</param>
        /// <param name="screenSetupDTO">screenSetupDTO</param>
        public ScreenSetup(ExecutionContext executionContext, ScreenSetupDTO screenSetupDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, screenSetupDTO);
            this.screenSetupDTO = screenSetupDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the screen setup DTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="screenId">screenId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>screenSetup DTO</returns>
        public ScreenSetup(ExecutionContext executionContext, int screenId, bool loadChildRecords = false,
                                  bool activeChildRecords = true, SqlTransaction sqlTransaction = null) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, screenId, sqlTransaction);
            ScreenSetupDataHandler screenSetupDataHandler = new ScreenSetupDataHandler(sqlTransaction);
            this.screenSetupDTO = screenSetupDataHandler.GetScreenSetup(screenId);
            if (screenSetupDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ScreenSetup", screenId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(screenSetupDTO);
        }
        /// <summary>
        /// Generate screenZoneDefSetupDTO list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            ScreenZoneDefSetupList screenZoneDefSetupList = new ScreenZoneDefSetupList(executionContext);

            List<KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>> searchContentMapParameters = new List<KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>>();
            if (activeChildRecords)
            {
                searchContentMapParameters.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            searchContentMapParameters.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            searchContentMapParameters.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.SCREEN_ID, Convert.ToString(screenSetupDTO.ScreenId)));
            screenSetupDTO.ScreenZoneDefSetupDTOList = screenZoneDefSetupList.GetAllScreenZoneDefSetup(searchContentMapParameters, true, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        ///  Saves the screen setup 
        /// Checks if the screen id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScreenSetupDataHandler screenSetupDataHandler = new ScreenSetupDataHandler(sqlTransaction);

            if (screenSetupDTO.IsChangedRecursive == false && screenSetupDTO.ScreenId > -1)
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

            if (screenSetupDTO.ScreenId < 0)
            {
                screenSetupDTO = screenSetupDataHandler.InsertScreenSetup(screenSetupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                screenSetupDTO.AcceptChanges();
            }
            else
            {
                if (screenSetupDTO.IsChanged)
                {
                    screenSetupDTO = screenSetupDataHandler.UpdateScreenSetup(screenSetupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    screenSetupDTO.AcceptChanges();
                }
            }
            SaveChild(sqlTransaction);
            screenSetupDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : ScreenZoneDefSetupDTO 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveChild(SqlTransaction sqlTransaction)
        {
            if (screenSetupDTO.ScreenZoneDefSetupDTOList != null &&
                screenSetupDTO.ScreenZoneDefSetupDTOList.Any())
            {
                List<ScreenZoneDefSetupDTO> updatedScreenZoneDefSetupDTOList = new List<ScreenZoneDefSetupDTO>();
                foreach (var screenZoneDefSetupDTO in screenSetupDTO.ScreenZoneDefSetupDTOList)
                {
                    if (screenZoneDefSetupDTO.ScreenId != screenSetupDTO.ScreenId)
                    {
                        screenZoneDefSetupDTO.ScreenId = screenSetupDTO.ScreenId;
                    }
                    if (screenZoneDefSetupDTO.IsChanged)
                    {
                        updatedScreenZoneDefSetupDTOList.Add(screenZoneDefSetupDTO);
                    }
                }
                if (updatedScreenZoneDefSetupDTOList.Any())
                {
                    ScreenZoneDefSetupList screenZoneDefSetupList = new ScreenZoneDefSetupList(executionContext, updatedScreenZoneDefSetupDTOList);
                    screenZoneDefSetupList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validate the screenSetupDTO
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
        /// Returns the screen settup DTO
        /// </summary>
        public ScreenSetupDTO ScreenSetupDTO { get { return screenSetupDTO; } }
    }

    /// <summary>
    /// Manages the list of screen setup  DTOs
    /// </summary>
    public class ScreenSetupList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ScreenSetupDTO> screenSetupDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Returns the screen setup DTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="screenId">screenId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>screenSetup DTO</returns>
        public ScreenSetupDTO GetScreenSetupDTO(ExecutionContext executionContext, int screenId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, screenId, sqlTransaction);
            ScreenSetupDataHandler screenSetupDataHandler = new ScreenSetupDataHandler(sqlTransaction);
            this.executionContext = executionContext;
            ScreenSetupDTO screenSetupDTO = screenSetupDataHandler.GetScreenSetup(screenId);
            log.LogMethodExit(screenSetupDTO);
            return screenSetupDTO;
        }

        /// <summary>       
        /// Default constructor of ScreenSetupList class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ScreenSetupList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// create the screen setup list object
        /// </summary>
        /// <param name="screenSetUpList">screenSetUpList</param>
        /// <param name="executionContext">executionContext</param>
        public ScreenSetupList(ExecutionContext executionContext, List<ScreenSetupDTO> screenSetupDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, screenSetupDTOList);
            this.screenSetupDTOList = screenSetupDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the screen setup  list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>screenSetUp List</returns>
        public List<ScreenSetupDTO> GetAllScreenSetup(List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> searchParameters,
                                                 bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScreenSetupDataHandler screenSetupDataHandler = new ScreenSetupDataHandler(sqlTransaction);
            this.screenSetupDTOList = screenSetupDataHandler.GetScreenSetupList(searchParameters);
            if (screenSetupDTOList != null && screenSetupDTOList.Any() && loadChildRecords)
            {
                Build(screenSetupDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(screenSetupDTOList);
            return screenSetupDTOList;
        }

        private void Build(List<ScreenSetupDTO> screenSetupDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, ScreenSetupDTO> screenSetupDTODictionary = new Dictionary<int, ScreenSetupDTO>();
            List<int> screenSetupIdList = new List<int>();
            for (int i = 0; i < screenSetupDTOList.Count; i++)
            {
                if (screenSetupDTODictionary.ContainsKey(screenSetupDTOList[i].ScreenId))
                {
                    continue;
                }
                screenSetupDTODictionary.Add(screenSetupDTOList[i].ScreenId, screenSetupDTOList[i]);
                screenSetupIdList.Add(screenSetupDTOList[i].ScreenId);
            }
            ScreenZoneDefSetupList screenZoneDefSetupList = new ScreenZoneDefSetupList(executionContext);
            List<ScreenZoneDefSetupDTO> screenZoneDefSetupDTOList = screenZoneDefSetupList.GetAllScreenZoneDefSetupDTOList(screenSetupIdList, true, activeChildRecords, sqlTransaction);

            if (screenZoneDefSetupDTOList != null && screenZoneDefSetupDTOList.Any())
            {
                for (int i = 0; i < screenZoneDefSetupDTOList.Count; i++)
                {
                    if (screenSetupDTODictionary.ContainsKey(screenZoneDefSetupDTOList[i].ScreenId) == false)
                    {
                        continue;
                    }
                    ScreenSetupDTO screenSetupDTO = screenSetupDTODictionary[screenZoneDefSetupDTOList[i].ScreenId];
                    if (screenSetupDTO.ScreenZoneDefSetupDTOList == null)
                    {
                        screenSetupDTO.ScreenZoneDefSetupDTOList = new List<ScreenZoneDefSetupDTO>();
                    }
                    screenSetupDTO.ScreenZoneDefSetupDTOList.Add(screenZoneDefSetupDTOList[i]);
                }
            }
        }

        /// <summary>
        /// Save and Updated the screen setup details
        /// </summary>        
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (screenSetupDTOList == null ||
                screenSetupDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < screenSetupDTOList.Count; i++)
            {
                var screenSetupDTO = screenSetupDTOList[i];
                if (screenSetupDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    ScreenSetup screenSetup = new ScreenSetup(executionContext, screenSetupDTO);
                    screenSetup.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving screenSetupDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("screenSetupDTO", screenSetupDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
