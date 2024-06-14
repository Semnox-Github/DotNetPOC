/********************************************************************************************
 * Project Name - InventoryHistory 
 * Description  - Bussiness logic of InventoryHistory 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       3-Jan-2016    Amaresh          Created 
 *2.70.2     18-Aug-2019   Deeksha          Added logger methods.
 *2.70.2     18-Aug-2019   Deeksha          Modified : Save method() return History Id.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// class of InventoryHistory
    /// </summary>
    public class InventoryHistory
    {
        private InventoryHistoryDTO inventoryHistoryDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        //Utilities _utilities;

        /// <summary>
        /// Default constructor
        /// </summary>
        private InventoryHistory(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Constructor with the Inventory DTO parameter
        /// </summary>
        /// <param name="inventoryHistoryDTO">Parameter of the type InventoryHistoryDTO</param>
        public InventoryHistory(ExecutionContext executionContext,InventoryHistoryDTO inventoryHistoryDTO) : this(executionContext)
        {
            log.LogMethodEntry(inventoryHistoryDTO);
            this.inventoryHistoryDTO = inventoryHistoryDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Inventory
        /// Inventory  will be inserted if ProductId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public int Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
           // ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
            InventoryHistoryDataHandler inventoryHistoryDataHandler = new InventoryHistoryDataHandler(sqlTransaction);

            if (inventoryHistoryDTO.Id <= 0)
            {
                inventoryHistoryDTO = inventoryHistoryDataHandler.InsertInventoryHistory(inventoryHistoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                inventoryHistoryDTO.AcceptChanges();
            }
            else
            {
                if (inventoryHistoryDTO.IsChanged == true)
                {
                    inventoryHistoryDTO = inventoryHistoryDataHandler.UpdateInventoryHistoryDTO(inventoryHistoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    inventoryHistoryDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
            return inventoryHistoryDTO.Id;
        }
    }
    /// <summary>
    /// Manages the list of InventoryHistory
    /// </summary>
    public class InventoryHistoryList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<InventoryHistoryDTO> inventoryHistoryDTOList = new List<InventoryHistoryDTO>();

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public InventoryHistoryList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public InventoryHistoryList(ExecutionContext executionContext, List<InventoryHistoryDTO> inventoryHistoryDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.inventoryHistoryDTOList = inventoryHistoryDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the InventoryHistory list
        /// </summary>
        public List<InventoryHistoryDTO> GetInventoryHistory(List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            InventoryHistoryDataHandler inventoryHistoryDataHandler = new InventoryHistoryDataHandler(sqlTransaction);
            this.inventoryHistoryDTOList = inventoryHistoryDataHandler.GetInventoryHistoryList(searchParameters);
            log.LogMethodExit(inventoryHistoryDTOList);
            return inventoryHistoryDTOList;
        }

        /// <summary>
        /// Returns the InventoryHistory DTO
        /// </summary>
        public InventoryHistoryDTO GetInventoryHistory(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            InventoryHistoryDataHandler inventoryHistoryDataHandler = new InventoryHistoryDataHandler(sqlTransaction);
            InventoryHistoryDTO inventoryHistoryDTO = new InventoryHistoryDTO();
            inventoryHistoryDTO = inventoryHistoryDataHandler.GetInventoryHistory(id);
            log.LogMethodExit(inventoryHistoryDTO);
            return inventoryHistoryDTO;
        }

        /// <summary>
        /// Returns the no of Inventory History matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetInventoryHistoryCount(List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryHistoryDataHandler inventoryHistoryDataHandler = new InventoryHistoryDataHandler(sqlTransaction);
            int count = inventoryHistoryDataHandler.GetInventoryHistoryCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// Returns the InventoryHistoryDTO
        /// </summary>
        public InventoryHistoryDTO GetInventoryHistory(int productId, int locationId, int lotId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId, locationId, lotId, sqlTransaction);
            InventoryHistoryDataHandler inventoryHistoryDataHandler = new InventoryHistoryDataHandler(sqlTransaction);
            InventoryHistoryDTO inventoryHistoryDTO = new InventoryHistoryDTO();
            inventoryHistoryDTO = inventoryHistoryDataHandler.GetInventoryHistory(productId, locationId, lotId);
            log.LogMethodExit(inventoryHistoryDTO);
            return inventoryHistoryDTO;
        }

        /// <summary>
        /// Returns the Inventory List
        /// </summary>
        public List<InventoryHistoryDTO> GetAllInventoryHistory(List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryHistoryDataHandler inventoryHistoryDataHandler = new InventoryHistoryDataHandler(sqlTransaction);
            List<InventoryHistoryDTO> inventoryHistoryDTOs = new List<InventoryHistoryDTO>();
            inventoryHistoryDTOs = inventoryHistoryDataHandler.GetInventoryHistoryList(searchParameters);
            log.LogMethodExit(inventoryHistoryDTOs);
            return inventoryHistoryDTOs;
        }
    }
}
