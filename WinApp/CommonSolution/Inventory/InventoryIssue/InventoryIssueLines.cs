/********************************************************************************************
 * Project Name - Inventory Issue Lines
 * Description  - Bussiness logic of inventory issue header
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        11-08-2016   Raghuveera          Created 
 *2.70.2        14-Jul-2019  Deeksha             Modified:Save method returns DTO instead of id
 *2.110.0    29-Dec-2020    Prajwal S      Modified : Added GetAllInventoryIssueLinesDTOList - Gets the 
 *                                                    InventoryIssueLinesDTO List for inventoryIssueIdList.
 *                                                    Added Validation.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// 
    /// </summary>
    public class InventoryIssueLines
    {
        private InventoryIssueLinesDTO inventoryIssueLinesDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;// = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Constructor with the executionContext parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public InventoryIssueLines(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            inventoryIssueLinesDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="inventoryIssueLinesDTO">Parameter of the type InventoryIssueLinesDTO</param>
        /// <param name="executionContext">Execution Context</param>
        public InventoryIssueLines(InventoryIssueLinesDTO inventoryIssueLinesDTO, ExecutionContext executionContext)
            :this(executionContext)
        {
            log.LogMethodEntry(inventoryIssueLinesDTO, executionContext);
            this.inventoryIssueLinesDTO = inventoryIssueLinesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="guid">Parameter of the type guid of the record</param>
        /// <param name="executionContext">ExecutionContext</param>
        public InventoryIssueLines(string guid, ExecutionContext executionContext)
            :this(executionContext)
        {
            log.LogMethodEntry(guid, executionContext);
            List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>> searchByInventoryIssueLinesParameters = new List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>>();
            searchByInventoryIssueLinesParameters.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ACTIVE_FLAG, "1"));
            searchByInventoryIssueLinesParameters.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.GUID, guid));
            InventoryIssueLinesList inventoryIssueLinesList = new InventoryIssueLinesList();
           List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList = inventoryIssueLinesList.GetAllInventoryIssueLines(searchByInventoryIssueLinesParameters);
            if(inventoryIssueLinesDTOList==null||(inventoryIssueLinesDTOList!=null && inventoryIssueLinesDTOList.Count==0))
            {
                inventoryIssueLinesDTO = new InventoryIssueLinesDTO();
            }
            else
            {
                inventoryIssueLinesDTO = inventoryIssueLinesDTOList[0];
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Inventory Issue Lines
        /// Inventory Issue Lines will be inserted if InventoryIssueId is less than or equal to
        /// zero else updates the records based on primary key
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);            
            InventoryIssueLinesDataHandler inventoryIssueLinesDataHandler = new InventoryIssueLinesDataHandler(sqlTransaction);
            if (inventoryIssueLinesDTO.IssueLineId < 0)
            {
                inventoryIssueLinesDTO = inventoryIssueLinesDataHandler.InsertInventoryIssueLines(inventoryIssueLinesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                inventoryIssueLinesDTO.AcceptChanges();
            }
            else
            {
                if (inventoryIssueLinesDTO.IsChanged)
                {
                    inventoryIssueLinesDTO=inventoryIssueLinesDataHandler.UpdateInventoryIssueLines(inventoryIssueLinesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    inventoryIssueLinesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of inventory issue header
    /// </summary>
    public class InventoryIssueLinesList
    {
        private List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList;
        private ExecutionContext executionContext;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor with no parameter.
        /// </summary>
        public InventoryIssueLinesList()
        {
            log.LogMethodEntry();
            this.inventoryIssueLinesDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Default constructor with executionContext
        /// </summary>
        public InventoryIssueLinesList(ExecutionContext executionContext)
            : this()
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the asset list
        /// </summary>
        /// <param name="IssueLineId">IssueLineId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryIssueLinesDTO</returns>
        public InventoryIssueLinesDTO GetIssueLines(int IssueLineId, SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(IssueLineId, sqlTransaction);
            InventoryIssueLinesDataHandler inventoryIssueLinesDataHandler = new InventoryIssueLinesDataHandler(sqlTransaction);
            InventoryIssueLinesDTO inventoryIssueLinesDTO = new InventoryIssueLinesDTO();
            inventoryIssueLinesDTO = inventoryIssueLinesDataHandler.GetInventoryIssueLines(IssueLineId);
            log.LogMethodExit(inventoryIssueLinesDTO);
            return inventoryIssueLinesDTO;
        }

        /// <summary>
        /// Returns the Inventory Issue Lines list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryIssueLinesDTOList</returns>
        public List<InventoryIssueLinesDTO> GetAllInventoryIssueLines(List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryIssueLinesDataHandler inventoryIssueLinesDataHandler = new InventoryIssueLinesDataHandler(sqlTransaction);
            List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
            inventoryIssueLinesDTOList = inventoryIssueLinesDataHandler.GetInventoryIssueLinesList(searchParameters);
            log.LogMethodExit(inventoryIssueLinesDTOList);
            return inventoryIssueLinesDTOList;
        }

        /// <summary>
        /// Gets the InventoryIssueLinesDTO List for inventoryIssueIdList
        /// </summary>
        /// <param name="inventoryIssueHeaderIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of InventoryIssueLinesDTO</returns>
        public List<InventoryIssueLinesDTO> GetAllInventoryIssueLinesDTOList(List<int> inventoryIssueHeaderIdList, bool activeRecords, SqlTransaction sqlTransaction = null) //added
        {
            log.LogMethodEntry(inventoryIssueHeaderIdList, activeRecords, sqlTransaction);
            InventoryIssueLinesDataHandler inventoryIssueLinesDataHandler = new InventoryIssueLinesDataHandler(sqlTransaction);
            this.inventoryIssueLinesDTOList = inventoryIssueLinesDataHandler.GetInventoryIssueLinesDTOList(inventoryIssueHeaderIdList, activeRecords);
            log.LogMethodExit(inventoryIssueLinesDTOList);
            return inventoryIssueLinesDTOList;
        }


    }
}
