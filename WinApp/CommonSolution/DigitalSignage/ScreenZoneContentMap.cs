/********************************************************************************************
 * Project Name - Screen Zone Content Map
 * Description  - Bussiness logic of Screen Zone Content Map
 * 
 **************
 **Version Log
 **************
 *Version     Date         Modified By         Remarks          
 *********************************************************************************************
 *1.00        08-03-2017   Raghuveera          Created 
 *2.40        28-Sep-2018  Jagan Mohan         Added new constructor ScreenZoneContentMap
 *2.70.2        31-Jul-2019  Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
*2.100.0        13-Aug-2020      Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, and 
 *                                                 List class changes as per 3 tier standards.
  *2.110.0     26-Nov-2020       Prajwal S          Modified : Constructor with Id parameter                                                 
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
    /// Business logic for screen zone content map
    /// </summary>
    public class ScreenZoneContentMap
    {
        private ScreenZoneContentMapDTO screenZoneContentMapDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ScreenZoneContentMap(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.screenZoneContentMapDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the screen zone content map DTO
        /// </summary>
        /// <param name="screenContentId">screenId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>screenZoneContentMap DTO</returns>
        public ScreenZoneContentMap(ExecutionContext executionContext, int screenContentId, SqlTransaction sqlTransaction = null) : this(executionContext)
        {
            log.LogMethodEntry(screenContentId, sqlTransaction);
            ScreenZoneContentMapDataHandler screenZoneContentMapDataHandler = new ScreenZoneContentMapDataHandler(sqlTransaction);
            this.screenZoneContentMapDTO = screenZoneContentMapDataHandler.GetScreenZoneContentMap(screenContentId);
            if (screenZoneContentMapDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "screenZoneContentMap", screenContentId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }

            log.LogMethodExit(screenZoneContentMapDTO);
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="screenZoneContentMapDTO">ScreenZoneContentMap DTO</param>
        public ScreenZoneContentMap(ExecutionContext executionContext, ScreenZoneContentMapDTO screenZoneContentMapDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, screenZoneContentMapDTO);
            this.screenZoneContentMapDTO = screenZoneContentMapDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the screen zone content map 
        /// Checks if the screen id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScreenZoneContentMapDataHandler screenZoneContentMapDataHandler = new ScreenZoneContentMapDataHandler(sqlTransaction);
            if (screenZoneContentMapDTO.IsChanged == false 
                && screenZoneContentMapDTO.ScreenContentId > -1)
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
            if (screenZoneContentMapDTO.ScreenContentId < 0)
            {
                screenZoneContentMapDTO = screenZoneContentMapDataHandler.InsertScreenZoneContentMap(screenZoneContentMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                screenZoneContentMapDTO.AcceptChanges();
            }
            else
            {
                if (screenZoneContentMapDTO.IsChanged)
                {
                    screenZoneContentMapDTO = screenZoneContentMapDataHandler.UpdateScreenZoneContentMap(screenZoneContentMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    screenZoneContentMapDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the screenZoneContentMapDTO
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
        public ScreenZoneContentMapDTO ScreenZoneContentMapDTO { get { return screenZoneContentMapDTO; } }

    }

    /// <summary>
    /// Manages the list of screen zone content map  DTOs
    /// </summary>
    public class ScreenZoneContentMapList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ScreenZoneContentMapDTO> screenZoneContentMapDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ScreenZoneContentMapList class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ScreenZoneContentMapList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor of ScreenZoneContentMapList class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ScreenZoneContentMapList(ExecutionContext executionContext, List<ScreenZoneContentMapDTO> screenZoneContentMapDTOList) : this(executionContext)
        {
            log.LogMethodEntry(screenZoneContentMapDTOList);
            this.screenZoneContentMapDTOList = screenZoneContentMapDTOList;
            log.LogMethodExit();
        }


        ///// <summary>
        ///// Returns the screen zone content map DTO
        ///// </summary>
        ///// <param name="screenId">screenId</param>
        ///// <param name="sqlTransaction">sqlTransaction</param>
        ///// <returns>screenZoneContentMap DTO</returns>
        //public ScreenZoneContentMapDTO GetScreenZoneContentMapDTO(int screenId, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(screenId, sqlTransaction);
        //    ScreenZoneContentMapDataHandler screenZoneContentMapDataHandler = new ScreenZoneContentMapDataHandler(sqlTransaction);
        //    ScreenZoneContentMapDTO screenZoneContentMapDTO = screenZoneContentMapDataHandler.GetScreenZoneContentMap(screenId);
        //    log.LogMethodExit(screenZoneContentMapDTO);
        //    return screenZoneContentMapDTO;
        //}
        /// <summary>
        /// Returns the screen zone content map list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>screenZoneContent List</returns>
        public List<ScreenZoneContentMapDTO> GetAllScreenZoneContentMap(List<KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScreenZoneContentMapDataHandler screenZoneContentMapDataHandler = new ScreenZoneContentMapDataHandler(sqlTransaction);
            this.screenZoneContentMapDTOList = screenZoneContentMapDataHandler.GetScreenZoneContentMapList(searchParameters);
            log.LogMethodExit(screenZoneContentMapDTOList);
            return screenZoneContentMapDTOList;
        }

        /// <summary>
        /// Gets the ScreenZoneContentMapDTO List for zoneIdList
        /// </summary>
        /// <param name="zoneIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ScreenZoneContentMapDTO</returns>
        public List<ScreenZoneContentMapDTO> GetScreenZoneContentMapDTOList(List<int> zoneIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(zoneIdList, activeRecords);
            ScreenZoneContentMapDataHandler screenZoneContentMapDataHandler = new ScreenZoneContentMapDataHandler(sqlTransaction);
            this.screenZoneContentMapDTOList = screenZoneContentMapDataHandler.GetScreenZoneContentMapDTOList(zoneIdList, activeRecords);
            log.LogMethodExit(screenZoneContentMapDTOList);
            return screenZoneContentMapDTOList;
        }

        /// <summary>
        /// Saves the screenZoneContentMapDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (screenZoneContentMapDTOList == null ||
                screenZoneContentMapDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < screenZoneContentMapDTOList.Count; i++)
            {
                var screenZoneContentMapDTO = screenZoneContentMapDTOList[i];
                if (screenZoneContentMapDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ScreenZoneContentMap screenZoneContentMap = new ScreenZoneContentMap(executionContext, screenZoneContentMapDTO);
                    screenZoneContentMap.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving screenZoneContentMapDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("screenZoneContentMapDTO", screenZoneContentMapDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
