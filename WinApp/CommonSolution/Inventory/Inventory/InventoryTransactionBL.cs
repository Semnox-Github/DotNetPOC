/********************************************************************************************
* Project Name - Inventory Transaction
* Description  - Bussiness logic of Inventory Transaction
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*2.70.2       14-Jul-2019    Deeksha        Modifications as per three tier standard
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// InventoryTransactionBL
    /// </summary>
    public class InventoryTransactionBL
    {
        private InventoryTransactionDTO inventoryTransactionDTO;
        private logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;//= Semnox.Parafait.Context.ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Default constructor of Requisition  class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public InventoryTransactionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            inventoryTransactionDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates inventoryTransaction object using the InventoryTransactionDTO
        /// </summary>
        /// <param name="inventoryTransactionDTO">InventoryTransactionDTO object</param>
        /// <param name="executionContext">ExecutionContext</param>
        public InventoryTransactionBL(InventoryTransactionDTO inventoryTransactionDTO, ExecutionContext executionContext)
            : this(executionContext)
        {
            log.LogMethodEntry(inventoryTransactionDTO, executionContext);
            this.inventoryTransactionDTO = inventoryTransactionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the templates 
        /// Checks if the template id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// <param name="sqlTrxn">sqlTrxn</param>
        /// </summary>
        public void Save(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);
            int transactionId = -1;
            InventoryTransactionDataHandler inventoryTransactionDataHandler = new InventoryTransactionDataHandler(SQLTrx);
            if (inventoryTransactionDTO.TransactionId < 0)
            {
                inventoryTransactionDTO = inventoryTransactionDataHandler.InsertInventoryTransactionDTO(inventoryTransactionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                inventoryTransactionDTO.AcceptChanges();
            }
            else
            {
                if (inventoryTransactionDTO.IsChanged)
                {
                    inventoryTransactionDTO = inventoryTransactionDataHandler.UpdateInventoryTransactionDTO(inventoryTransactionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    transactionId = inventoryTransactionDTO.TransactionId;
                    inventoryTransactionDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Class to retrive InventoryTransaction data 
    /// </summary>
    public class InventoryTransactionList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<InventoryTransactionDTO> inventoryTransactionDTOList = null;
        /// <summary>
        /// Default constructor of Requisition  class
        /// </summary>
        /// <param name="executionContext">v</param>
        public InventoryTransactionList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public InventoryTransactionList(ExecutionContext executionContext, List<InventoryTransactionDTO> inventoryTransactionDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.inventoryTransactionDTOList = inventoryTransactionDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns InventoryTransactionDTO list based on search parameters
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTrxn">sqlTrxn</param>
        /// <returns>inventoryTransactionDTOList</returns>
        public List<InventoryTransactionDTO> GetInventoryTransactionList(List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>> searchParameters, SqlTransaction sqlTrxn = null)
        {
            log.LogMethodEntry(searchParameters, sqlTrxn);
            InventoryTransactionDataHandler inventoryTransactionDataHandler = new InventoryTransactionDataHandler(sqlTrxn);
            List<InventoryTransactionDTO> inventoryTransactionDTOList = new List<InventoryTransactionDTO>();
            inventoryTransactionDTOList = inventoryTransactionDataHandler.GetInventoryTransactionList(searchParameters);
            log.LogMethodExit(inventoryTransactionDTOList);
            return inventoryTransactionDTOList;
        }

        /// <summary>
        /// method to get InventoryTransactionTypeId 
        /// </summary>
        /// <param name="inventoryTransactionTypeName"></param>
        /// <returns>inventoryTransactionTypeId</returns>
        public int GetInventoryTransactionTypeId(string inventoryTransactionTypeName)
        {
            log.LogMethodEntry(inventoryTransactionTypeName);
            int inventoryTransactionTypeId = -1;
            if (!String.IsNullOrEmpty(inventoryTransactionTypeName))
            {

                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
                searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>
                {
                    new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INVENTORY_TRANSACTION_TYPE"),
                    new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, inventoryTransactionTypeName)
                };
                List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
                if (lookupValuesListDTO != null)
                {
                    inventoryTransactionTypeId = lookupValuesListDTO[0].LookupValueId;
                }
            }
            log.LogMethodExit(inventoryTransactionTypeId);
            return inventoryTransactionTypeId;
        }

        public decimal GetEstimatedQuantity()
        {
            log.LogMethodEntry();
            decimal estQty = 0;
            int inventoryUOMId = -1;
            decimal factor = 1;
            ProductContainer productContainer = new ProductContainer(executionContext);
            if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
            {
                inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == inventoryTransactionDTOList[0].ProductId).InventoryUOMId;
                foreach (InventoryTransactionDTO trxDTO in inventoryTransactionDTOList)
                {
                    if (trxDTO.UOMId != -1 && inventoryUOMId != trxDTO.UOMId)
                    {
                        factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(trxDTO.UOMId, inventoryUOMId));
                    }
                    estQty += Convert.ToDecimal(trxDTO.Quantity) * factor;
                }
            }
            log.LogMethodExit(estQty);
            return estQty;
        }
    }
}
