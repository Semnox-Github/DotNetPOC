/********************************************************************************************
 * Project Name - DisplayPanelThemeMap BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017      Lakshminarayana     Created 
 *2.40        29-Sep-2018      Jagan Mohan         Added new constructor DisplayPanelThemeMapBL, DisplayPanelThemeMapListBL and 
 *                                                 methods SaveUpdatePanelThemeMapList
 *2.70.2        30-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.100        28-Jul-2020      Mushahid Faizan     Modified : 3 tier changes for Rest API.
 * 2.110.0    26-Nov-2020       Prajwal S           Modified :  public DisplayPanelThemeMapBL(ExecutionContext executionContext, int id,
 *                                                              SqlTransaction sqltransaction = null)
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
    /// Business logic for DisplayPanelThemeMap class.
    /// </summary>
    public class DisplayPanelThemeMapBL
    {
        private DisplayPanelThemeMapDTO displayPanelThemeMapDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of DisplayPanelThemeMapBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private DisplayPanelThemeMapBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.displayPanelThemeMapDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the displayPanelThemeMap id as the parameter Would fetch the displayPanelThemeMap object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="id">id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public DisplayPanelThemeMapBL(ExecutionContext executionContext, int id, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqltransaction);
            DisplayPanelThemeMapDataHandler displayPanelThemeMapDataHandler = new DisplayPanelThemeMapDataHandler(sqltransaction);
            this.displayPanelThemeMapDTO = displayPanelThemeMapDataHandler.GetDisplayPanelThemeMapDTO(id);
            if (displayPanelThemeMapDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "displayPanelThemeMap", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(displayPanelThemeMapDTO);
        }

        /// <summary>
        /// Creates DisplayPanelThemeMapBL object using the DisplayPanelThemeMapDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="displayPanelThemeMapDTO"></param>
        public DisplayPanelThemeMapBL(ExecutionContext executionContext, DisplayPanelThemeMapDTO displayPanelThemeMapDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, displayPanelThemeMapDTO);
            this.displayPanelThemeMapDTO = displayPanelThemeMapDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the DisplayPanelThemeMap
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqltransaction">sqltransaction</param>
        public void Save(SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(sqltransaction);
            DisplayPanelThemeMapDataHandler displayPanelThemeMapDataHandler = new DisplayPanelThemeMapDataHandler(sqltransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (displayPanelThemeMapDTO.Id < 0)
            {
                displayPanelThemeMapDTO = displayPanelThemeMapDataHandler.InsertDisplayPanelThemeMap(displayPanelThemeMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                displayPanelThemeMapDTO.AcceptChanges();
            }
            else
            {
                if (displayPanelThemeMapDTO.IsChanged)
                {
                    displayPanelThemeMapDTO = displayPanelThemeMapDataHandler.UpdateDisplayPanelThemeMap(displayPanelThemeMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    displayPanelThemeMapDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (displayPanelThemeMapDTO == null)
            {
                //Validation to be implemented.
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DisplayPanelThemeMapDTO DisplayPanelThemeMapDTO
        {
            get
            {
                return displayPanelThemeMapDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of DisplayPanelThemeMap
    /// </summary>
    public class DisplayPanelThemeMapListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of DisplayPanelThemeMapListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public DisplayPanelThemeMapListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.displayPanelThemeMapDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="panelthememaptoList">panelthememaptoList</param>
        /// <param name="executioncontext">executioncontext</param>
        public DisplayPanelThemeMapListBL(ExecutionContext executionContext, List<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, displayPanelThemeMapDTOList);
            this.displayPanelThemeMapDTOList = displayPanelThemeMapDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the DisplayPanelThemeMap list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>DisplayPanelThemeMap list</returns>
        public List<DisplayPanelThemeMapDTO> GetDisplayPanelThemeMapDTOList(List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DisplayPanelThemeMapDataHandler displayPanelThemeMapDataHandler = new DisplayPanelThemeMapDataHandler(sqlTransaction);
            this.displayPanelThemeMapDTOList = displayPanelThemeMapDataHandler.GetDisplayPanelThemeMapDTOList(searchParameters);
            log.LogMethodExit(displayPanelThemeMapDTOList);
            return displayPanelThemeMapDTOList;
        }

        /// <summary>
        /// Save and Updated the schedule panel theme map details
        /// </summary>        
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (displayPanelThemeMapDTOList == null ||
                displayPanelThemeMapDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < displayPanelThemeMapDTOList.Count; i++)
            {
                var displayPanelThemeMapDTO = displayPanelThemeMapDTOList[i];
                if (displayPanelThemeMapDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    DisplayPanelThemeMapBL displayPanelThemeMapBL = new DisplayPanelThemeMapBL(executionContext, displayPanelThemeMapDTO);
                    displayPanelThemeMapBL.Save();
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
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving displayPanelThemeMapDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("displayPanelThemeMapDTO", displayPanelThemeMapDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
