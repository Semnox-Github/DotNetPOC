/********************************************************************************************
* Project Name - Inventory Lot
* Description  - Bussiness logic of Inventory Lot
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00       12-Aug-2016    Amaresh        Created 
*2.70.2     09-Jul-2019    Deeksha        Modifications as per three tier standard
*2.70.2     27-Nov-2019    Girish Kundar  Modified: Issue fix - Inventory Adjustment 
*2.100.0    27-Nov-2019    Deeksha        Modified: Recipe Management enhancement changes
*2.110.0    29-Dec-2020    Prajwal        Added : Added GetAllInventoryLotDTOList to get 
 *                                                InventoryLotDTOList using parent Id list. 
*2.110.0    20-Feb-2020    Dakshakh Raj   Modified: Get Sequence method changes
*2.110.4    01-Oct-2021    Guru S A       Physical count performance fixes
********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory lot will creates and modifies the inventory Lot
    /// </summary>
    public class InventoryLotBL
    {
        private InventoryLotDTO inventoryLotDTO;
        private logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionUserContext;//= ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionUserContext">ExecutionContext</param>
        public InventoryLotBL(ExecutionContext executionUserContext)
        {
            log.LogMethodEntry(executionUserContext);
            this.executionUserContext = executionUserContext;
            inventoryLotDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the InventoryLot DTO parameter
        /// </summary>
        /// <param name="inventoryLotDTO">Parameter of the type InventoryLotDTO</param>
        /// <param name="executionUserContext">Excecution context</param>
        public InventoryLotBL(InventoryLotDTO inventoryLotDTO, ExecutionContext executionUserContext)
        {
            log.LogMethodEntry(inventoryLotDTO, executionUserContext);
            this.inventoryLotDTO = inventoryLotDTO;
            this.executionUserContext = executionUserContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Inventory Lot
        /// Inventory Lot will be inserted if lotId is less than or equal to
        /// zero else updates the records based on primary key
        /// <param name="sqlTransaction">Sql transaction </param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            InventoryLotDataHandler inventoryLotDataHandler = new InventoryLotDataHandler(sqlTransaction);
            if (inventoryLotDTO.LotId < 0)
            {
                GetSequenceNumber(sqlTransaction, executionUserContext);
                if (!string.IsNullOrEmpty(inventoryLotDTO.LotNumber))
                {
                    inventoryLotDTO = inventoryLotDataHandler.InsertInventoryLot(inventoryLotDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    inventoryLotDTO.AcceptChanges();
                }
                else
                {
                    throw new Exception("Unable to generate lot number.");
                }
            }
            else
            {
                if (inventoryLotDTO.IsChanged)
                {
                    inventoryLotDTO = inventoryLotDataHandler.UpdateInventoryLot(inventoryLotDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    inventoryLotDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Sequence Number
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="executionContext"></param>
        private void GetSequenceNumber(SqlTransaction sqlTransaction, ExecutionContext executionContext)
        {
            log.LogMethodEntry(sqlTransaction, executionContext);
            SequencesListBL sequencesListBL = new SequencesListBL(executionContext);
            SequencesDTO sequencesDTO = null;
            List<KeyValuePair<SequencesDTO.SearchByParameters, string>> searchBySeqParameters = new List<KeyValuePair<SequencesDTO.SearchByParameters, string>>();
            searchBySeqParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SEQUENCE_NAME, "InventoryLot"));
            List<SequencesDTO> sequencesDTOList = sequencesListBL.GetAllSequencesList(searchBySeqParameters);
            if (sequencesDTOList != null && sequencesDTOList.Any())
            {
                if (sequencesDTOList.Count == 1)
                {
                    sequencesDTO = sequencesDTOList[0];
                }
                else
                {
                    sequencesDTO = sequencesDTOList.FirstOrDefault(seq => seq.POSMachineId == executionContext.GetMachineId());
                    if (sequencesDTO == null)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2956, executionContext.GetMachineId()));
                    }
                }
                SequencesBL sequenceBL = new SequencesBL(executionContext, sequencesDTO);
                inventoryLotDTO.LotNumber = sequenceBL.GetNextSequenceNo(sqlTransaction);
            }
            log.LogMethodExit();
        }

        public InventoryLotDTO GetInventoryLot
        {
            get
            {
                return inventoryLotDTO;
            }
        }

        /// <summary>
        /// Inventory Lot Logic will inserts/Updates the records to the inventory,InventoryLot and inventoryAdjustment table
        /// </summary>
        /// <param name="productId">Product id of the product</param>
        /// <param name="quantity">issued Quantity</param>
        /// <param name="fromLocationId">Source location id</param>
        /// <param name="toLocationId">Destinition location id</param>
        /// <param name="siteId">Site id</param>
        /// <param name="userId">user login id</param>
        /// <param name="applicability">applicability</param>
        /// <param name="originalReferenceGuid">originalReferenceGuid</param>
        /// <param name="DocumentTypeId">Issue document type id </param>
        /// <param name="sqlTransaction">Sql transaction </param>
        public void ExecuteInventoryLotIssue(int productId, double quantity, int fromLocationId, int toLocationId, int siteId, string userId, string applicability, string originalReferenceGuid, int DocumentTypeId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId, quantity, fromLocationId, toLocationId, siteId, userId, applicability, originalReferenceGuid, DocumentTypeId, sqlTransaction);
            InventoryLotDataHandler inventoryLotDataHandler = new InventoryLotDataHandler(sqlTransaction);
            inventoryLotDataHandler.ExecuteInventoryLotIssue(productId, quantity, fromLocationId, toLocationId, siteId, userId, applicability, originalReferenceGuid, DocumentTypeId, sqlTransaction);
            log.LogMethodExit();
        }

        //Added 21-Feb-2017
        /// <summary>
        /// Converts non lot inventory record to lot when item is updated to lot controlled
        /// </summary>
        /// <param name="productId">Product id of the product</param>
        /// <param name="sqlTransaction">Sql transaction</param>
        public void UpdateNonLotableToLotable(int productId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId, sqlTransaction);
            InventoryLotDataHandler inventoryLotDataHandler = new InventoryLotDataHandler(sqlTransaction);
            inventoryLotDataHandler.UpdateNonLotableToLotable(productId);
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of Inventory lot List
    /// </summary>
    public class InventoryLotList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<InventoryLotDTO> inventoryLotDTOList = new List<InventoryLotDTO>();

        public InventoryLotList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="RecipeManufacturingHeaderDTOList">RecipeManufacturingHeader DTO List as parameter </param>
        public InventoryLotList(ExecutionContext executionContext,
                                               List<InventoryLotDTO> inventoryLotDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, inventoryLotDTOList);
            this.inventoryLotDTOList = inventoryLotDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Inventory Lot
        /// </summary>
        /// <param name="lotId">lotId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryLotDTO</returns>
        public InventoryLotDTO GetInventoryLot(int lotId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lotId, sqlTransaction);
            InventoryLotDataHandler inventoryLotDataHandler = new InventoryLotDataHandler(sqlTransaction);
            InventoryLotDTO inventoryLotDTO = new InventoryLotDTO();
            inventoryLotDTO = inventoryLotDataHandler.GetInventoryLotDTO(lotId);
            log.LogMethodExit(inventoryLotDTO);
            return inventoryLotDTO;
        }

        /// <summary>
        /// Returns the Inventory lot List
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryLotDTOList</returns>
        public List<InventoryLotDTO> GetAllInventoryLot(List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryLotDataHandler inventoryLotDataHandler = new InventoryLotDataHandler(sqlTransaction);
            List<InventoryLotDTO> inventoryLotDTOList = new List<InventoryLotDTO>();
            inventoryLotDTOList = inventoryLotDataHandler.GetInventoryLotList(searchParameters);
            log.LogMethodExit(inventoryLotDTOList);
            return inventoryLotDTOList;
        }

        /// <summary>
        /// Returns the Inventory lot List
        /// </summary>
        /// <param name="lastSyncTime">lastSyncTime</param>
        /// <param name="maxRowsToFetch">maxRowsToFetch</param>
        /// <param name="lastLotId">lastLotId</param>
        /// <param name="sqlTransaction"></param>
        /// <returns>inventoryLotList</returns>
        public List<InventoryLotDTO> GetAllInventoryLot(DateTime lastSyncTime, int maxRowsToFetch, int lastLotId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lastSyncTime, maxRowsToFetch, lastLotId, sqlTransaction);
            InventoryLotDataHandler inventoryLotDataHandler = new InventoryLotDataHandler(sqlTransaction);
            List<InventoryLotDTO> inventoryLotList = new List<InventoryLotDTO>();
            inventoryLotList = inventoryLotDataHandler.GetInventoryLotList(lastSyncTime, maxRowsToFetch, lastLotId);
            log.LogMethodExit(inventoryLotList);
            return inventoryLotList;
        }

        /// <summary>
        /// Get list of lots for specific receive line
        /// </summary>
        /// <param name="receiptLineId">receiptLineId</param>
        /// <returns>inventoryLotsOnDisplay</returns>
        public List<InventoryLotDTO> GetInventoryLotListByReceiveLineID(int receiptLineId)
        {
            log.LogMethodEntry(receiptLineId);
            try
            {
                InventoryLotList inventoryLotList = new InventoryLotList(executionContext);
                List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>> inventoryLotLinesSearchParams = new List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>>();
                inventoryLotLinesSearchParams.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.PURCHASEORDER_RECEIVE_LINEID, Convert.ToString(receiptLineId)));
                List<InventoryLotDTO> inventoryLotsOnDisplay = inventoryLotList.GetAllInventoryLot(inventoryLotLinesSearchParams);
                if (inventoryLotsOnDisplay != null && inventoryLotsOnDisplay.Count > 0)
                {
                    log.LogMethodExit(inventoryLotsOnDisplay);
                    return inventoryLotsOnDisplay;
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in GetInventoryLotListByReceiveLineID() method." + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets the InventoryLotDTO List for screenIdList
        /// </summary>
        /// <param name="purchaseOrderReceiveLineIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of InventoryLotDTO</returns>
        public List<InventoryLotDTO> GetAllInventoryLotDTOList(List<int> purchaseOrderReceiveLineIdList, bool activeRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(purchaseOrderReceiveLineIdList, activeRecords, sqlTransaction);
            InventoryLotDataHandler InventoryLotDataHandler = new InventoryLotDataHandler(sqlTransaction);
            this.inventoryLotDTOList = InventoryLotDataHandler.GetInventoryLotDTOList(purchaseOrderReceiveLineIdList, activeRecords);
            log.LogMethodExit(inventoryLotDTOList);
            return inventoryLotDTOList;
        }
        
        /// <summary>
        /// Validates and saves the InventoryAdjustmentsDTO list to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public List<InventoryLotDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inventoryLotDTOList == null || inventoryLotDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return inventoryLotDTOList;
            }
            InventoryLotDataHandler inventoryLotDataHandler = new InventoryLotDataHandler(sqlTransaction);
            inventoryLotDTOList = inventoryLotDataHandler.Save(inventoryLotDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
            return inventoryLotDTOList;
        }
    }
}
