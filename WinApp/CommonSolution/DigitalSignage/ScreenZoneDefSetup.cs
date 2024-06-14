/********************************************************************************************
 * Project Name - Screen Zone Def Setup
 * Description  - Bussiness logic of Screen Zone Def Setup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        08-03-2017        Raghuveera          Created
 *2.40        28-Sep-2018       Jagan Mohan         Added new constructor ScreenZoneDefSetup, ScreenZoneDefSetupList and 
 *                                                  new methods SaveUpdateScreenZoneDefSetupList,GetAllScreenZoneDefSetupContentMap,
 *                                                  SaveUpdateScreenZoneDefSetupContentMapList
 *2.70.2        31-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.100.0       13-Aug-2020      Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, Build() to get child records and 
 *                                                 List class changes as per 3 tier standards.
*2.110.0     01-Nov-2020       Prajwal S          Modified : Constructor with Id parameter                                                 
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
    /// Business Logic for zone definition setup
    /// </summary>
    public class ScreenZoneDefSetup
    {        
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ScreenZoneDefSetupDTO screenZoneDefSetupDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ScreenZoneDefSetup(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.screenZoneDefSetupDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="screenZoneDefSetupDTO">screenZoneDefSetupDTO</param>
        public ScreenZoneDefSetup(ExecutionContext executionContext, ScreenZoneDefSetupDTO screenZoneDefSetupDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext,screenZoneDefSetupDTO);
            this.screenZoneDefSetupDTO = screenZoneDefSetupDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the screen zone def setup DTO
        /// </summary>
        /// <param name="zoneId">zoneId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>screenZoneDefSetup DTO</returns>        
        public ScreenZoneDefSetup(ExecutionContext executionContext, int zoneId, bool loadChildRecords = false,
                                  bool activeChildRecords = true, SqlTransaction sqlTransaction = null) : this(executionContext)
        {
            log.LogMethodEntry(zoneId, sqlTransaction);
            ScreenZoneDefSetupDataHandler screenZoneDefSetupDataHandler = new ScreenZoneDefSetupDataHandler(sqlTransaction);
            this.screenZoneDefSetupDTO = screenZoneDefSetupDataHandler.GetScreenZoneDefSetup(zoneId);
            if (screenZoneDefSetupDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ScreenZoneDefSetup", zoneId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(screenZoneDefSetupDTO);
        }
        /// <summary>
        /// Generate screenZoneDefSetupDTO list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            ScreenZoneContentMapList screenZoneContentMapList = new ScreenZoneContentMapList(executionContext);


            List<KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>> searchContentMapParameters = new List<KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>>();
            if (activeChildRecords)
            {
                searchContentMapParameters.Add(new KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>(ScreenZoneContentMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            searchContentMapParameters.Add(new KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>(ScreenZoneContentMapDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            searchContentMapParameters.Add(new KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>(ScreenZoneContentMapDTO.SearchByParameters.ZONE_ID, Convert.ToString(screenZoneDefSetupDTO.ZoneId)));
            screenZoneDefSetupDTO.ScreenZoneContentMapDTOList = screenZoneContentMapList.GetAllScreenZoneContentMap(searchContentMapParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the screen zone def setup 
        /// Checks if the zone id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScreenZoneDefSetupDataHandler screenZoneDefSetupDataHandler = new ScreenZoneDefSetupDataHandler(sqlTransaction);
            if (screenZoneDefSetupDTO.IsChangedRecursive == false && screenZoneDefSetupDTO.ZoneId > -1)
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

            if (screenZoneDefSetupDTO.ZoneId < 0)
            {
                screenZoneDefSetupDTO = screenZoneDefSetupDataHandler.InsertScreenZoneDefSetup(screenZoneDefSetupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                screenZoneDefSetupDTO.AcceptChanges(); ;
            }
            else
            {
                if (screenZoneDefSetupDTO.IsChanged)
                {
                    screenZoneDefSetupDTO = screenZoneDefSetupDataHandler.UpdateScreenZoneDefSetup(screenZoneDefSetupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    screenZoneDefSetupDTO.AcceptChanges();
                }
            }
            SaveChild(sqlTransaction);
            screenZoneDefSetupDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : screenZoneContentMapDTO 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveChild(SqlTransaction sqlTransaction)
        {
            if (screenZoneDefSetupDTO.ScreenZoneContentMapDTOList != null &&
                screenZoneDefSetupDTO.ScreenZoneContentMapDTOList.Any())
            {
                List<ScreenZoneContentMapDTO> updatedScreenZoneContentMapDTOList = new List<ScreenZoneContentMapDTO>();
                foreach (var screenZoneContentMapDTO in screenZoneDefSetupDTO.ScreenZoneContentMapDTOList)
                {
                    if (screenZoneContentMapDTO.ZoneId != screenZoneDefSetupDTO.ZoneId)
                    {
                        screenZoneContentMapDTO.ZoneId = screenZoneDefSetupDTO.ZoneId;
                    }
                    if (screenZoneContentMapDTO.IsChanged)
                    {
                        updatedScreenZoneContentMapDTOList.Add(screenZoneContentMapDTO);
                    }
                }
                if (updatedScreenZoneContentMapDTOList.Any())
                {
                    ScreenZoneContentMapList screenZoneContentMapList = new ScreenZoneContentMapList(executionContext, updatedScreenZoneContentMapDTOList);
                    screenZoneContentMapList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validate the screenZoneDefSetupDTO
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
        public ScreenZoneDefSetupDTO ScreenZoneDefSetupDTO { get { return screenZoneDefSetupDTO; } }
    }


    /// <summary>
    /// Manages the list of screen zone def setup  DTOs
    /// </summary>
    public class ScreenZoneDefSetupList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ScreenZoneDefSetupDTO> screenZoneDefSetupDTOList;
        private ExecutionContext executionContext;
        
        /// <summary>
        /// Default constructor of ScreenZoneContentMapList class
        /// </summary>
        /// <param name="executionContex">executionContex</param>
        public ScreenZoneDefSetupList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// create the screen zone list object
        /// </summary>
        /// <param name="screenZoneList">screenZoneList</param>
        /// <param name="executionContext">executionContext</param>
        /// <param name="executionContext">executionContext</param>
        public ScreenZoneDefSetupList(ExecutionContext executionContext, List<ScreenZoneDefSetupDTO> screenZoneDefSetupDTOList) : this(executionContext)
        {
            log.LogMethodEntry(screenZoneDefSetupDTOList, executionContext);
            this.screenZoneDefSetupDTOList = screenZoneDefSetupDTOList;
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Returns the screen zone def setup DTO
        ///// </summary>
        ///// <param name="zoneId">zoneId</param>
        ///// <param name="sqlTransaction">sqlTransaction</param>
        ///// <returns>screenZoneDefSetup DTO</returns>        
        //public ScreenZoneDefSetupDTO GetScreenZoneDefSetupDTO(int zoneId, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(zoneId, sqlTransaction);
        //    ScreenZoneDefSetupDataHandler screenZoneDefSetupDataHandler = new ScreenZoneDefSetupDataHandler(sqlTransaction);
        //    ScreenZoneDefSetupDTO screenZoneDefSetupDTO = screenZoneDefSetupDataHandler.GetScreenZoneDefSetup(zoneId);
        //    log.LogMethodExit(screenZoneDefSetupDTO);
        //    return screenZoneDefSetupDTO;
        //}

        /// <summary>
        /// Gets the ScreenZoneDefSetupDTO List for screenIdList
        /// </summary>
        /// <param name="screenIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ScreenZoneDefSetupDTO</returns>
        public List<ScreenZoneDefSetupDTO> GetAllScreenZoneDefSetupDTOList(List<int> screenIdList, bool loadChildRecords = true, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(screenIdList, activeRecords);
            ScreenZoneDefSetupDataHandler screenZoneDefSetupDataHandler = new ScreenZoneDefSetupDataHandler(sqlTransaction);
            this.screenZoneDefSetupDTOList = screenZoneDefSetupDataHandler.GetScreenZoneDefSetupDTOList(screenIdList, activeRecords);
            if (screenZoneDefSetupDTOList != null && screenZoneDefSetupDTOList.Any() && loadChildRecords)
            {
                Build(screenZoneDefSetupDTOList, activeRecords, sqlTransaction);
            }
            log.LogMethodExit(screenZoneDefSetupDTOList);
            return screenZoneDefSetupDTOList;
        }


        /// <summary>
        /// Returns the screen zone def setup  list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>screenZone List</returns>
        public List<ScreenZoneDefSetupDTO> GetAllScreenZoneDefSetup(List<KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>> searchParameters,
                                                                     bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScreenZoneDefSetupDataHandler screenZoneDefSetupDataHandler = new ScreenZoneDefSetupDataHandler(sqlTransaction);
            this.screenZoneDefSetupDTOList = screenZoneDefSetupDataHandler.GetScreenZoneDefSetupList(searchParameters);
            if (screenZoneDefSetupDTOList != null && screenZoneDefSetupDTOList.Any() && loadChildRecords)
            {
                Build(screenZoneDefSetupDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(screenZoneDefSetupDTOList);
            return screenZoneDefSetupDTOList;
        }

        private void Build(List<ScreenZoneDefSetupDTO> screenZoneDefSetupDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, ScreenZoneDefSetupDTO> screenZoneDefSetupDTODictionary = new Dictionary<int, ScreenZoneDefSetupDTO>();
            List<int> zoneIdList = new List<int>();
            for (int i = 0; i < screenZoneDefSetupDTOList.Count; i++)
            {
                if (screenZoneDefSetupDTODictionary.ContainsKey(screenZoneDefSetupDTOList[i].ZoneId))
                {
                    continue;
                }
                screenZoneDefSetupDTODictionary.Add(screenZoneDefSetupDTOList[i].ZoneId, screenZoneDefSetupDTOList[i]);
                zoneIdList.Add(screenZoneDefSetupDTOList[i].ZoneId);
            }
            ScreenZoneContentMapList screenZoneContentMapList = new ScreenZoneContentMapList(executionContext);
            List<ScreenZoneContentMapDTO> screenzoneContentMapList = screenZoneContentMapList.GetScreenZoneContentMapDTOList(zoneIdList, activeChildRecords, sqlTransaction);

            if (screenzoneContentMapList != null && screenzoneContentMapList.Any())
            {
                for (int i = 0; i < screenzoneContentMapList.Count; i++)
                {
                    if (screenZoneDefSetupDTODictionary.ContainsKey(screenzoneContentMapList[i].ZoneId) == false)
                    {
                        continue;
                    }
                    ScreenZoneDefSetupDTO screenZoneDefSetupDTO = screenZoneDefSetupDTODictionary[screenzoneContentMapList[i].ZoneId];
                    if (screenZoneDefSetupDTO.ScreenZoneContentMapDTOList == null)
                    {
                        screenZoneDefSetupDTO.ScreenZoneContentMapDTOList = new List<ScreenZoneContentMapDTO>();
                    }
                    screenZoneDefSetupDTO.ScreenZoneContentMapDTOList.Add(screenzoneContentMapList[i]);
                }
            }
        }


        /// <summary>
        /// Saves the screenZoneDefSetupDTOList 
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (screenZoneDefSetupDTOList == null ||
                screenZoneDefSetupDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < screenZoneDefSetupDTOList.Count; i++)
            {
                var screenZoneDefSetupDTO = screenZoneDefSetupDTOList[i];
                if (screenZoneDefSetupDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    ScreenZoneDefSetup screenZone = new ScreenZoneDefSetup(executionContext, screenZoneDefSetupDTO);
                    screenZone.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving screenZoneDefSetupDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("screenZoneDefSetupDTO", screenZoneDefSetupDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Save and Updated the screen content map details for the Screen Content Map Page : For Web Management UI implmnetations
        ///// This function is to save or update the child content list(ScreenZoneContentMapDTOList) and assigned to the attribute(ScreenZoneContentMapDTO) in the ScreenZoneContentMap
        ///// </summary>        
        //public void SaveUpdateScreenZoneDefSetupContentMapList()
        //{
        //    try
        //    {
        //        log.LogMethodEntry();
        //        SqlTransaction sqlTrxn = null;
        //        if (screenZoneList != null)
        //        {
        //            foreach (ScreenZoneDefSetupDTO screenZoneDefSetupDTO in screenZoneList)
        //            {
        //                if (screenZoneDefSetupDTO.ScreenZoneContentMapDTOList != null)
        //                {
        //                    foreach (ScreenZoneContentMapDTO screenZoneContentMapDTO in screenZoneDefSetupDTO.ScreenZoneContentMapDTOList)
        //                    {
        //                        screenZoneContentMapDTO.ZoneId = screenZoneDefSetupDTO.ZoneId;
        //                        ScreenZoneContentMap screenZoneContentMap = new ScreenZoneContentMap(executionContext, screenZoneContentMapDTO);
        //                        screenZoneContentMap.Save(sqlTrxn);
        //                    }
        //                }
        //            }
        //        }
        //        log.LogMethodExit();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
        //        throw;
        //    }
        //}
    }
}
