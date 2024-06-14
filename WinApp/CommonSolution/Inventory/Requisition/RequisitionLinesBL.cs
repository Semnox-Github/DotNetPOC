
/********************************************************************************************************
* Project Name - Inventory Requisition 
* Description  - Bussiness logic of Requisition Lines
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************************
*1.00        16-Aug-2016   Suneetha.S          Created 
*2.70        16-Jul-2019   Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
*2.110.0    11-Dec-2020   Mushahid Faizan     Modified : Web Inventory Changes
*********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// To create Requisition templates
    /// </summary> 
    public class RequisitionLinesBL
    {
        private RequisitionLinesDTO requisitionLineDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of Requisition Lines class
        /// </summary>
        public RequisitionLinesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Requsition Line id as the parameter
        /// Would fetch the Templates object from the database based on the id passed. 
        /// </summary>
        /// <param name="requsitionLineId">Req Template id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RequisitionLinesBL(ExecutionContext executionContext,int requsitionLineId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(requsitionLineId, sqlTransaction);
            RequisitionLinesDataHandler requisitionLinesDataHandler = new RequisitionLinesDataHandler(sqlTransaction);
            requisitionLineDTO = requisitionLinesDataHandler.GetRequisitionLine(requsitionLineId);
            if (requisitionLineDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Requisition Lines ", requsitionLineId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates templates type object using the reqTemplatesDTO
        /// </summary>
        /// <param name="requisitionLinesDTO">RequisitionLinesDTO object</param>
        public RequisitionLinesBL(ExecutionContext executionContext, RequisitionLinesDTO requisitionLinesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(requisitionLinesDTO);
            this.requisitionLineDTO = requisitionLinesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the templates 
        /// Checks if the template id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>id</returns>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (requisitionLineDTO.IsChanged == false
                && requisitionLineDTO.RequisitionLineId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RequisitionLinesDataHandler requisitionLineDataHandler = new RequisitionLinesDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (requisitionLineDTO.RequisitionLineId < 0)
            {
                requisitionLineDTO = requisitionLineDataHandler.Insert(requisitionLineDTO,  executionContext.GetUserId(), executionContext.GetSiteId());
                requisitionLineDTO.AcceptChanges();
            }
            else
            {
                if (requisitionLineDTO.IsChanged)
                {
                    requisitionLineDTO = requisitionLineDataHandler.Update(requisitionLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    requisitionLineDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the RequisitionLinesDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (requisitionLineDTO.RequestedQuantity < 1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2404);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RequisitionLinesDTO GetRequisitionLineDTO { get { return requisitionLineDTO; } }

        /// <summary>
        ///  Update approved quanity and status for the line
        /// </summary>
        /// <param name="pendingRequisitionQty">pendingRequisitionQty</param>
        public void UpdateRequisitionLineQtyNStatus(double pendingRequisitionQty)
        {
            requisitionLineDTO.ApprovedQuantity = requisitionLineDTO.ApprovedQuantity + pendingRequisitionQty;
            if (requisitionLineDTO.ApprovedQuantity < requisitionLineDTO.RequestedQuantity)
            {
                requisitionLineDTO.Status = "InProgress";
            }
            else if (requisitionLineDTO.ApprovedQuantity == requisitionLineDTO.RequestedQuantity)
            { requisitionLineDTO.Status = "Closed"; }
        }
    }

    /// <summary>
    /// Manages the list of RequisitionTemplate
    /// </summary>
    public class RequisitionLinesList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;//added
        List<RequisitionLinesDTO> requisitionLinesDTOList = null;

        /// <summary>
        /// Default constructor with executionContext
        /// </summary>
        public RequisitionLinesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parmeterised constructor with 2 params
        /// </summary>
        /// <param name="requisitionLinesDTOList"></param>
        /// <param name="executionContext"></param>
        public RequisitionLinesList(ExecutionContext executionContext, List<RequisitionLinesDTO> requisitionLinesDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, requisitionLinesDTOList);
            this.requisitionLinesDTOList = requisitionLinesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Requisitions list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>requisitionLinesDTOList</returns>
        public List<RequisitionLinesDTO> GetAllRequisitionLines(List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RequisitionLinesDataHandler requisitionLinesDataHandler = new RequisitionLinesDataHandler(sqlTransaction);
            List<RequisitionLinesDTO> requisitionLinesDTOList = requisitionLinesDataHandler.GetRequisitionLinesList(searchParameters);
            log.LogMethodExit(requisitionLinesDTOList);
            return requisitionLinesDTOList;
        }

        /// <summary>
        /// Gets the RequisitionLinesDTO List for RequisitionIdList
        /// </summary>
        /// <param name="requisitionIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of segmentDefinitionSourceMapDTOList</returns>
        public List<RequisitionLinesDTO> GetRequisitionLinesDTOList(List<int> requisitionIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(requisitionIdList);
            RequisitionLinesDataHandler requisitionLinesDataHandler = new RequisitionLinesDataHandler(sqlTransaction);
            this.requisitionLinesDTOList = requisitionLinesDataHandler.GetRequisitionLinesDTOList(requisitionIdList, sqlTransaction);  
            log.LogMethodExit(requisitionLinesDTOList);
            return requisitionLinesDTOList;
        }

        /// <summary>
        /// Returns the Requisition line DTO
        /// </summary>
        /// <param name="requisitionLineId">requisitionLineId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>requisitionLinesDTO</returns>
        public RequisitionLinesDTO GetRequisitionLine(int requisitionLineId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(requisitionLineId, sqlTransaction);
            RequisitionLinesDataHandler requisitionLinesDataHandler = new RequisitionLinesDataHandler(sqlTransaction);
            RequisitionLinesDTO requisitionLinesDTO = requisitionLinesDataHandler.GetRequisitionLine(requisitionLineId);
            log.LogMethodExit(requisitionLinesDTO);
            return requisitionLinesDTO;
        }

        /// <summary>
        /// Returns the List of Requisitions DTO
        /// </summary>
        /// <param name="requisitionId">requisitionId</param>
        /// <param name="isActive">isActive</param>
        /// <param name="locationId">locationId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>requisitionLinesDTOList</returns>
        public List<RequisitionLinesDTO> GetRequisitionsList(int requisitionId, bool isActive, int locationId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(requisitionId, isActive, locationId, sqlTransaction);
            RequisitionLinesDataHandler requisitionLinesDataHandler = new RequisitionLinesDataHandler(sqlTransaction);
            List<RequisitionLinesDTO> requisitionLinesDTOList = requisitionLinesDataHandler.GetRequisitionLineList(requisitionId, isActive, locationId);
            log.LogMethodExit(requisitionLinesDTOList);
            return requisitionLinesDTOList;
        }
        /// <summary>
        /// Returns Pending Rquested Quantity
        /// </summary>
        /// <param name="requisitionLinesDTO">requisitionLinesDTO</param>
        /// <returns>RequestedQuantity</returns>
        public double GetPendingRequestedQty(RequisitionLinesDTO requisitionLinesDTO)
        {
            log.LogMethodEntry(requisitionLinesDTO);

            if (requisitionLinesDTO != null)
                return requisitionLinesDTO.RequestedQuantity - ((requisitionLinesDTO.ApprovedQuantity == -1) ? 0 : requisitionLinesDTO.ApprovedQuantity);
            else
                log.LogMethodExit(0);
            return 0;
        }
    }

    /// <summary>
    /// RequisitionLineGenerics
    /// </summary>
    public class RequisitionLineGenerics
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Converts the Data row object to UOM class type
        /// </summary>
        /// <param name="productId">GetProductStock</param>
        /// <param name="requisitionId">GetProductStock</param>
        /// <returns>Returns string total stock at location name</returns>
        public double GetProductStock(int productId, int requisitionId)
        {
            log.LogMethodEntry(productId, requisitionId);
            double count = 0;
            try
            {
                int locationId = -1;
                RequisitionList requisitionList = new RequisitionList(executionContext);
                RequisitionDTO requisitionDTO = requisitionList.GetRequisition(requisitionId);
                if (requisitionDTO != null)
                {
                    if (!DBNull.Value.Equals(requisitionDTO.ToDepartment))
                        locationId = Convert.ToInt32(requisitionDTO.ToDepartment);
                }
                InventoryList inventoryList = new InventoryList();
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();

                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, productId.ToString()));
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, locationId.ToString()));

                List<InventoryDTO> inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams);
                if (inventoryDTOList != null && inventoryDTOList.Count > 0)
                {
                    count = Convert.ToDouble(inventoryDTOList[0].Quantity);
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex.Message);
            }
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// Converts the Data row object to UOM class type
        /// </summary>
        /// <param name="productId">GetProductStock</param>
        /// <param name="toDeptId">GetProductStock</param>
        /// <returns>Returns string total stock at location name</returns>
        public double GetProductStockForNew(int productId, int toDeptId)
        {
            log.LogMethodEntry(productId, toDeptId);
            double count = 0;
            try
            {
                InventoryList inventoryList = new InventoryList();
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();

                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, productId.ToString()));
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, toDeptId.ToString()));

                List<InventoryDTO> inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams);
                if (inventoryDTOList != null && inventoryDTOList.Count > 0)
                {
                    count = Convert.ToDouble(inventoryDTOList[0].Quantity);
                }
            }
            catch (Exception ex)
            {
                log.LogMethodEntry(ex.Message);
            }
            log.LogMethodExit(count);
            return count;
        }
    }
}
