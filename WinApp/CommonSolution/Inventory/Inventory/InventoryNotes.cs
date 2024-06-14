/********************************************************************************************
 * Project Name - Inventory Notes
 * Description  - Bussiness logic of Inventory Notes
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       12-Aug-2016    Amaresh        Created 
 *2.70.2       09-Jul-2019    Deeksha        Modifications as per three tier standard.
 *2.150.1       12-Apr-2023   Abhishek       Modified : Added validations on save
  ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory lot will creates and modifies the inventory Notes
    /// </summary>
    public class InventoryNotes
    {
        private InventoryNotesDTO inventoryNotesDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryNotes()
        {
            log.LogMethodEntry();
            inventoryNotesDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the InventoryNotes DTO parameter
        /// </summary>
        /// <param name="inventoryNotesDTO">Parameter of the type InventoryNotesDTO</param>
        public InventoryNotes(InventoryNotesDTO inventoryNotesDTO)
        {
            log.LogMethodEntry(inventoryNotesDTO);
            this.inventoryNotesDTO = inventoryNotesDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the InventoryNotes DTO parameter
        /// </summary>
        /// <param name="inventoryNotesDTO">Parameter of the type InventoryNotesDTO</param>
        public InventoryNotes(ExecutionContext executionContext, InventoryNotesDTO inventoryNotesDTO)
        {
            log.LogMethodEntry(inventoryNotesDTO);
            this.executionContext = executionContext;
            this.inventoryNotesDTO = inventoryNotesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Inventory Notes
        /// Inventory Notes will be inserted if lotId is less than or equal to
        /// zero else updates the records based on primary key
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            InventoryNotesDataHandler inventoryNotesDataHandler = new InventoryNotesDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (inventoryNotesDTO.InventoryNoteId <= 0)
            {
                inventoryNotesDTO = inventoryNotesDataHandler.InsertInventoryNotes(inventoryNotesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                inventoryNotesDTO.AcceptChanges();
            }
            else
            {
                if (inventoryNotesDTO.IsChanged)
                {
                    inventoryNotesDTO = inventoryNotesDataHandler.UpdateInventoryNotes(inventoryNotesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    inventoryNotesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the InventoryNotesDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inventoryNotesDTO.NoteTypeId <= 0)
            {
                log.Error("Please enter Note Type");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4908);
                throw new ValidationException(errorMessage);
            }
            if (string.IsNullOrEmpty(inventoryNotesDTO.Notes))
            {
                log.Error("Notes cannot be empty");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5077);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of Inventory Notes List
    /// </summary>
    public class InventoryNotesList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public InventoryNotesList(ExecutionContext executionContext = null)
        {
            this.executionContext = executionContext;
        }
        /// <summary>
        ///  Returns the Inventory Notes
        /// </summary>
        /// <param name="inventoryNoteId">inventoryNoteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryNotesDTO</returns>
        public InventoryNotesDTO GetInventoryNotes(int inventoryNoteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(inventoryNoteId, sqlTransaction);
            InventoryNotesDataHandler inventoryNotesDataHandler = new InventoryNotesDataHandler(sqlTransaction);
            InventoryNotesDTO inventoryNotesDTO = new InventoryNotesDTO();
            inventoryNotesDTO = inventoryNotesDataHandler.GetInventoryNotes(inventoryNoteId);
            log.LogMethodExit(inventoryNotesDTO);
            return inventoryNotesDTO;
        }

        /// <summary>
        /// Returns the Inventory lot List
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryNotesDTOList</returns>
        public List<InventoryNotesDTO> GetAllInventoryNotes(List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryNotesDataHandler inventoryNotesDataHandler = new InventoryNotesDataHandler(sqlTransaction);
            List<InventoryNotesDTO> inventoryNotesDTOList = new List<InventoryNotesDTO>();
            inventoryNotesDTOList = inventoryNotesDataHandler.GetInventoryNotesList(searchParameters);
            log.LogMethodExit(inventoryNotesDTOList);
            return inventoryNotesDTOList;
        }

        /// <summary>
        /// Returns the no of Inventory Notes matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetInventoryNotesCount(List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryNotesDataHandler inventoryNotesDataHandler = new InventoryNotesDataHandler(sqlTransaction);
            int count = inventoryNotesDataHandler.GetInventoryNotesCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }
    }
}
