/********************************************************************************************
 * Project Name - Inventory
 * Description  -Business logic -InventoryActivityLogBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        03-Jun-2019   Girish Kundar           Created 
 *2.110.0   01-Jan-2021     Mushahid Faizan         Modified for Inventory UI Redesign changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory
{
   public class InventoryActivityLogBL
    {
        private InventoryActivityLogDTO inventoryActivityLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="InventoryActivityLogDTO"></param>
        public InventoryActivityLogBL(ExecutionContext executionContext, InventoryActivityLogDTO InventoryActivityLogDTO)
        {
            log.LogMethodEntry(executionContext, InventoryActivityLogDTO);
            this.executionContext = executionContext;
            this.inventoryActivityLogDTO = InventoryActivityLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the InventoryActivityLogBL
        /// ads will be inserted if ads is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            int rowInserted = -1;
            InventoryActivityLogDataHandler InventoryActivityLogDataHandler = new InventoryActivityLogDataHandler(sqlTransaction);
            rowInserted = InventoryActivityLogDataHandler.Insert(InventoryActivityLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            inventoryActivityLogDTO.AcceptChanges();
            log.LogMethodExit(rowInserted);
        }

        /// <summary>
        /// get InventoryActivityLogDTO Object
        /// </summary>
        public InventoryActivityLogDTO InventoryActivityLogDTO
        {
            get { return inventoryActivityLogDTO; }
        }
    }

    /// <summary>
    /// Manages the list of Ads
    /// </summary>
    public class InventoryActivityLogBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<InventoryActivityLogDTO> inventoryActivityLogDTOList;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public InventoryActivityLogBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            inventoryActivityLogDTOList = new List<InventoryActivityLogDTO>();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the InventoryActivityLogBL list
        /// </summary>
        public List<InventoryActivityLogDTO> GetInventoryActivityLogDTOList(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryActivityLogDataHandler InventoryActivityLogDataHandler = new InventoryActivityLogDataHandler(sqlTransaction);
            List<InventoryActivityLogDTO> inventoryActivityLogDTOList = InventoryActivityLogDataHandler.GetInventoryActivityLogDTOList(searchParameters, currentPage, pageSize, sqlTransaction);
            log.LogMethodExit(inventoryActivityLogDTOList);
            return inventoryActivityLogDTOList;
        }

        /// <summary>
        /// Returns the no of Inventory Activities matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetInventoryActivityLogCount(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryActivityLogDataHandler InventoryActivityLogDataHandler = new InventoryActivityLogDataHandler(sqlTransaction);
            int inventoryActivityLogCount = InventoryActivityLogDataHandler.GetInventoryActivityLogCount(searchParameters);
            log.LogMethodExit(inventoryActivityLogCount);
            return inventoryActivityLogCount;
        }

    }
}

