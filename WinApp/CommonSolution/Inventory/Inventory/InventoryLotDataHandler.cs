/********************************************************************************************
* Project Name -Inventory lot DataHandler
* Description  -Data object of inventory lot
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        12-Aug-2016   Amaresh          Created 
*2.70.2      14-jul-2019   Deeksha          Modifications as per three tier standard
*2.70.2      27-Nov-2019   Girish Kundar    Modified: Issue fix - Inventory Adjustment
*2.70.2      09-Dec-2019   Jinto Thomas     Removed site id from update query 
*2.100.0     27-Jul-2020   Deeksha          Modified : Added UOMId field.
*2.110.0     29-Dec-2020   Prajwal          Modified : Added GetInventoryLotDTOList to return InventoryLotDTO list using parent Id List. 
*2.110.4     01-Oct-2021   Guru S A         Physical count performance fixes
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using Microsoft.SqlServer.Server;
using System.Linq;


namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory lot - Handles insert, update and select of inventory lot objects
    /// </summary>
    public class InventoryLotDataHandler
    {
            private readonly SqlTransaction sqlTransaction;
            private readonly DataAccessHandler dataAccessHandler;
            private const string SELECT_QUERY = @"SELECT * FROM InventoryLot AS il ";
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            /// <summary>
            ///  Dictionary for searching Parameters for the inventory lot object.
            /// </summary>
            private static readonly Dictionary<InventoryLotDTO.SearchByInventoryLotParameters, string> DBSearchParameters = new Dictionary<InventoryLotDTO.SearchByInventoryLotParameters, string>
            {
                {InventoryLotDTO.SearchByInventoryLotParameters.LOT_ID, "il.LotId"},
                {InventoryLotDTO.SearchByInventoryLotParameters.LOT_NUMBER, "il.LotNumber"},
                {InventoryLotDTO.SearchByInventoryLotParameters.ORIGINAL_QUANTITY, "il.OriginalQuantity"},
                {InventoryLotDTO.SearchByInventoryLotParameters.BALANCE_QUANTITY,"il.BalanceQuantity"},
                {InventoryLotDTO.SearchByInventoryLotParameters.PURCHASEORDER_RECEIVE_LINEID, "il.PurchaseOrderReceiveLineId"},
                {InventoryLotDTO.SearchByInventoryLotParameters.EXPIRY_DATE, "il.Expirydate"},
                {InventoryLotDTO.SearchByInventoryLotParameters.IS_ACTIVE, "il.IsActive"},
                {InventoryLotDTO.SearchByInventoryLotParameters.SITE_ID, "il.site_id"},
                {InventoryLotDTO.SearchByInventoryLotParameters.SOURCE_SYSTEM_RECEIVELINEID, "il.SourceSystemReceiveLineID"},
                {InventoryLotDTO.SearchByInventoryLotParameters.LOT_ID_LIST, "il.LotId" },
                {InventoryLotDTO.SearchByInventoryLotParameters.MASTER_ENTITY_ID, "il.MasterEntityId" },
                {InventoryLotDTO.SearchByInventoryLotParameters.UOM_ID, "il.UOMId" }
            };

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS InventoryLotType;
                                            MERGE INTO InventoryLot tbl
                                            USING @InventoryLotList AS src
                                            ON src.LotId = tbl.LotId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                             LotNumber=src.LotNumber,
                                             OriginalQuantity=src.OriginalQuantity,
                                             BalanceQuantity=src.BalanceQuantity,
                                             ReceivePrice=src.ReceivePrice,
                                             PurchaseOrderReceiveLineId=src.PurchaseOrderReceiveLineId,
                                             Expirydate=src.Expirydate,
                                             IsActive=src.IsActive,
                                             --site_id=@siteId,
                                             MasterEntityId=src.MasterEntityId,
                                             SourceSystemReceiveLineID=src.SourceSystemReceiveLineID,
                                             LastUpdatedDate = getdate(),
                                             LastUpdatedBy =src.LastUpdatedBy,
                                             UOMId =src.UOMId
                                            WHEN NOT MATCHED THEN INSERT (
                                            LotNumber,
                                            OriginalQuantity,
                                            BalanceQuantity,
                                            ReceivePrice,
                                            PurchaseOrderReceiveLineId,
                                            Expirydate,
                                            IsActive,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            SourceSystemReceiveLineID,
                                            LastUpdatedDate,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            UOMId
                                            )VALUES (
                                            src.LotNumber,
                                            src.OriginalQuantity,
                                            src.BalanceQuantity,
                                            src.ReceivePrice,
                                            src.PurchaseOrderReceiveLineId
                                            src.Expirydate,
                                            src.IsActive,  
                                            src.site_id,
                                            src.Guid,
                                            src.MasterEntityId,
                                            src.SourceSystemReceiveLineID,
                                            GETDATE(), 
                                            src.CreatedBy,
                                            GETDATE(), 
                                            LastUpdatedBy, 
                                            src.UOMId 
                                            )
                                            OUTPUT
                                            inserted.LotId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdatedDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            LotId,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdatedDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion 

        /// <summary>
        /// Default constructor of InventoryLotDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public InventoryLotDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryLotDataHandler Record.
        /// </summary>
        /// <param name="inventoryLotDTO">InventoryLotDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(InventoryLotDTO inventoryLotDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryLotDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@lotId", inventoryLotDTO.LotId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lotNumber", inventoryLotDTO.LotNumber)); // not nullable field
            parameters.Add(dataAccessHandler.GetSQLParameter("@originalQuantity", inventoryLotDTO.OriginalQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@balanceQuantity", inventoryLotDTO.BalanceQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@receivePrice", inventoryLotDTO.ReceivePrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseOrderReceiveLineId", inventoryLotDTO.PurchaseOrderReceiveLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expirydate", inventoryLotDTO.Expirydate == DateTime.MinValue ? DBNull.Value:(object) inventoryLotDTO.Expirydate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", inventoryLotDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", inventoryLotDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sourceSystemID", string.IsNullOrEmpty(inventoryLotDTO.SourceSystemReceiveLineID) ? DBNull.Value : (object)inventoryLotDTO.SourceSystemReceiveLineID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", inventoryLotDTO.UOMId, true));                      
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the inventory lot record to the database
        /// </summary>
        /// <param name="inventoryLotDTO">InventoryLotDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">Sql transaction</param>
        /// <returns>Returns inserted record id</returns>
        public InventoryLotDTO InsertInventoryLot(InventoryLotDTO inventoryLotDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryLotDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[InventoryLot]
                                                        (
                                                        LotNumber,
                                                        OriginalQuantity,
                                                        BalanceQuantity,
                                                        ReceivePrice,
                                                        PurchaseOrderReceiveLineId,
                                                        Expirydate,
                                                        IsActive,
                                                        site_id,
                                                        Guid,
                                                        MasterEntityId,
                                                        SourceSystemReceiveLineID,
                                                        LastUpdatedDate,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        UOMId
                                                        ) 
                                                values 
                                                        ( 
                                                         @lotNumber,
                                                         @originalQuantity,
                                                         @balanceQuantity,
                                                         @receivePrice,
                                                         @purchaseOrderReceiveLineId,
                                                         @expirydate,        
                                                         @isActive,
                                                         @siteId,
                                                         NEWID(),
                                                         @masterEntityId,
                                                         @sourceSystemID,
                                                         getdate(),
                                                         @createdBy,
                                                         getdate(),
                                                         @lastUpdatedBy,
                                                         @UOMId)
                                            SELECT* FROM InventoryLot WHERE LotId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryLotDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryLotDTO(inventoryLotDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting InventoryLotDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryLotDTO);
            return inventoryLotDTO;
        }

        /// <summary>
        /// Updates the Inventory Lot record
        /// </summary>
        /// <param name="inventoryLotDTO">InventoryLotDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">Sql transaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryLotDTO UpdateInventoryLot(InventoryLotDTO inventoryLotDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryLotDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[InventoryLot]
                                         SET LotNumber=@lotNumber,
                                             OriginalQuantity=@originalQuantity,
                                             BalanceQuantity=@balanceQuantity,
                                             ReceivePrice=@receivePrice,
                                             PurchaseOrderReceiveLineId=@purchaseOrderReceiveLineId,
                                             Expirydate=@expirydate,
                                             IsActive=@isActive,
                                             --site_id=@siteId,
                                             MasterEntityId=@masterEntityId,
                                             SourceSystemReceiveLineID=@sourceSystemID,
                                             LastUpdatedDate = getdate(),
                                             LastUpdatedBy =@lastUpdatedBy,
                                             UOMId =@UOMId
                                      WHERE LotId =@lotId 
                                    SELECT * FROM InventoryLot WHERE LotId = @lotId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryLotDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryLotDTO(inventoryLotDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating inventoryLotDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryLotDTO);
            return inventoryLotDTO;
        }


        /// <summary>
        /// Delete the record from the InventoryLot database based on LotId
        /// </summary>
        /// <param name="lotId">lotId</param>
        /// <returns>return the int </returns>
        internal int Delete(int lotId)
        {
            log.LogMethodEntry(lotId);
            string query = @"DELETE  
                             FROM InventoryLot
                             WHERE InventoryLot.LotId = @lotId";
            SqlParameter parameter = new SqlParameter("@lotId", lotId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="InventoryLotDTO">InventoryLotDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshInventoryLotDTO(InventoryLotDTO inventoryLotDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryLotDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryLotDTO.LotId = Convert.ToInt32(dt.Rows[0]["LotId"]);
                inventoryLotDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                inventoryLotDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryLotDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryLotDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryLotDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryLotDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
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
        /// <param name="SQLTrx">Sql transaction</param>
        public void ExecuteInventoryLotIssue(int productId, double quantity, int fromLocationId, int toLocationId, int siteId, string userId, string applicability, string originalReferenceGuid, int DocumentTypeId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(productId, quantity, fromLocationId, toLocationId, siteId, userId, applicability, originalReferenceGuid, DocumentTypeId, SQLTrx);
            List<SqlParameter> updateInventoryLotParameters = new List<SqlParameter>();
            updateInventoryLotParameters.Add(dataAccessHandler.GetSQLParameter("@toLocationId", toLocationId, true));
            dataAccessHandler.executeSelectQuery("exec UpdateLotInventoryAdjustment " + productId + ", " + quantity + ", " + fromLocationId + ", @toLocationId, " + siteId + ", '" + userId + "', " + DocumentTypeId + ",'" + applicability + "','" + originalReferenceGuid + "'", updateInventoryLotParameters.ToArray(), SQLTrx);
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to InventoryLotDTO class type
        /// </summary>
        /// <param name="inventoryLotDataRow">InventoryLot DataRow</param>
        /// <returns>Returns inventoryLot</returns>
        private InventoryLotDTO GetInventoryLotDTO(DataRow inventoryLotDataRow)
        {
            log.LogMethodEntry(inventoryLotDataRow);
            InventoryLotDTO inventoryLotDataObject = new InventoryLotDTO(
                                            inventoryLotDataRow["LotId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryLotDataRow["LotId"]),
                                            inventoryLotDataRow["LotNumber"] == DBNull.Value ? string.Empty : inventoryLotDataRow["LotNumber"].ToString(),
                                            inventoryLotDataRow["OriginalQuantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryLotDataRow["OriginalQuantity"]),
                                            inventoryLotDataRow["BalanceQuantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryLotDataRow["BalanceQuantity"]),
                                            inventoryLotDataRow["ReceivePrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryLotDataRow["ReceivePrice"]),
                                            inventoryLotDataRow["PurchaseOrderReceiveLineId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryLotDataRow["PurchaseOrderReceiveLineId"]),
                                            inventoryLotDataRow["Expirydate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryLotDataRow["Expirydate"]),
                                            inventoryLotDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(inventoryLotDataRow["IsActive"]),
                                            inventoryLotDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryLotDataRow["site_id"]),
                                            inventoryLotDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryLotDataRow["Guid"]),
                                            inventoryLotDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryLotDataRow["SynchStatus"]),
                                            inventoryLotDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryLotDataRow["MasterEntityId"]),
                                            inventoryLotDataRow["SourceSystemReceiveLineID"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryLotDataRow["SourceSystemReceiveLineID"]),
                                            inventoryLotDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryLotDataRow["LastUpdatedDate"]),
                                            inventoryLotDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryLotDataRow["CreatedBy"]),
                                            inventoryLotDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryLotDataRow["CreationDate"]),
                                            inventoryLotDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryLotDataRow["LastUpdatedBy"]),
                                            inventoryLotDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryLotDataRow["UOMId"])
                                            );
            log.LogMethodExit(inventoryLotDataObject);
            return inventoryLotDataObject;
        }

        /// <summary>
        /// Gets the InventoryLotDTO List for purchaseOrderReceiveLineIdList
        /// </summary>
        /// <param name="purchaseOrderReceiveLineIdList">integer list parameter</param>
        /// <returns>Returns List of InventoryLotDTO List</returns>
        public List<InventoryLotDTO> GetInventoryLotDTOList(List<int> purchaseOrderReceiveLineIdList, bool activeRecords)
        {
            log.LogMethodEntry(purchaseOrderReceiveLineIdList);
            List<InventoryLotDTO> list = new List<InventoryLotDTO>();
            string query = @"SELECT InventoryLot.*
                            FROM InventoryLot, @purchaseOrderReceiveLineIdList List
                            WHERE PurchaseOrderReceiveLineId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@purchaseOrderReceiveLineIdList", purchaseOrderReceiveLineIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetInventoryLotDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }



        /// <summary>
        /// Gets the Inventory Lot data of passed Id
        /// </summary>
        /// <param name="lotId">Int type parameter</param>
        /// <returns>Returns InventoryLotDTO</returns>
        public InventoryLotDTO GetInventoryLotDTO(int lotId)
        {
            log.LogMethodEntry(lotId);
            InventoryLotDTO result = null;
            string query = SELECT_QUERY + @" WHERE il.LotId= @lotId";
            SqlParameter parameter = new SqlParameter("@lotId", lotId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetInventoryLotDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }  

        //Added 21-Feb-2017
        /// <summary>
        /// Converts non lot inventory record to lot when item is updated to lot controlled
        /// </summary>
        /// <param name="productId">Product id of the product</param>
        /// <param name="SQLTrx">Sql transaction</param>
        public void UpdateNonLotableToLotable(int productId)
        {
            log.LogMethodEntry(productId);
            List<SqlParameter> updateInventoryLotParameters = new List<SqlParameter>();
            dataAccessHandler.executeSelectQuery("exec UpdateNonLotableToLotable null, " + productId, updateInventoryLotParameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the InventoryLotDTO list matching the search key
        /// </summary>
        /// <param name="lastSyncTime">LastSyncTime</param>
        /// <param name="maxRowsToFetch">MaxRowsToFetch</param>
        /// <param name="lastLotId">LastLotId</param>
        /// <returns></returns>
        public List<InventoryLotDTO> GetInventoryLotList(DateTime lastSyncTime, int maxRowsToFetch, int lastLotId)
        {
            log.LogMethodEntry(lastSyncTime, maxRowsToFetch, lastLotId);
            List<InventoryLotDTO> inventoryLotList = new List<InventoryLotDTO>();
            string selectInventoryLotQuery = @"select top (@maxRowsToFetch) LotId, LotNumber, OriginalQuantity, BalanceQuantity, ReceivePrice, 
                                                Expirydate, PurchaseOrderReceiveLineId, IsActive, site_id, Guid, SynchStatus, 
                                                MasterEntityId, SourceSystemReceiveLineId, LastUpdatedDate ,CreatedBy,CreationDate,LastUpdatedBy,UOMId 
                                                from InventoryLot where LotId>@lastLotId and LastUpdatedDate>@lastSyncTime ";
            SqlParameter[] selectInventoryParameter = new SqlParameter[3];
            selectInventoryParameter[0] = new SqlParameter("@maxRowsToFetch", maxRowsToFetch);
            selectInventoryParameter[1] = new SqlParameter("@lastLotId", lastLotId);
            selectInventoryParameter[2] = new SqlParameter("@lastSyncTime", lastSyncTime);

            DataTable inventoryLotData = dataAccessHandler.executeSelectQuery(selectInventoryLotQuery, selectInventoryParameter, sqlTransaction);

            if (inventoryLotData.Rows.Count > 0)
            {
                foreach (DataRow inventoryLotDataRow in inventoryLotData.Rows)
                {
                    InventoryLotDTO inventoryLotDataObject = GetInventoryLotDTO(inventoryLotDataRow);
                    inventoryLotList.Add(inventoryLotDataObject);
                }
                log.LogMethodExit(inventoryLotList);
            }
            else
            {
                log.LogMethodExit(inventoryLotList);
            }
            return inventoryLotList;
        }

        /// <summary>
        /// Gets the InventoryLotDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryLotDTO matching the search criteria</returns>
        public List<InventoryLotDTO> GetInventoryLotList(List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryLotDTO> inventoryLotDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectInventoryLotQuery = SELECT_QUERY;
            if ((searchParameters != null)  && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.LOT_ID
                            || searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.UOM_ID
                            || searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.PURCHASEORDER_RECEIVE_LINEID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));

                        }
                        else if (searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.ORIGINAL_QUANTITY
                                 || searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.BALANCE_QUANTITY)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDecimal(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.IS_ACTIVE)
                        {

                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));

                        }
                        else if (searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.EXPIRY_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.LOT_NUMBER
                               || searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.SOURCE_SYSTEM_RECEIVELINEID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                        else if (searchParameter.Key == InventoryLotDTO.SearchByInventoryLotParameters.LOT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }

                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    count++;
                }
                selectInventoryLotQuery = selectInventoryLotQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectInventoryLotQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryLotDTOList = new List<InventoryLotDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryLotDTO inventoryLotDTO = GetInventoryLotDTO(dataRow);
                    inventoryLotDTOList.Add(inventoryLotDTO);
                }
            }
            log.LogMethodExit(inventoryLotDTOList);
            return inventoryLotDTOList;
        }


        /// <summary>
        /// Inserts the InventoryLot record to the database
        /// </summary>
        /// <param name="inventoryLotDTOList">List of InventoryLotDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public List<InventoryLotDTO> Save(List<InventoryLotDTO> inventoryLotDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryLotDTOList, loginId, siteId);
            Dictionary<string, InventoryLotDTO> inventoryHistoryDTOGuidMap = GetDTOGuidMap(inventoryLotDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(inventoryLotDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "InventoryLotType",
                                                                "@InventoryLotList");
            Update(inventoryHistoryDTOGuidMap, dataTable);
            List<InventoryLotDTO> updatedDTOList = GetUpdatedDTOListFromDictonary(inventoryHistoryDTOGuidMap);
            log.LogMethodExit(updatedDTOList);
            return updatedDTOList;
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<InventoryLotDTO> inventoryLotDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(inventoryLotDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[18];
            columnStructures[0] = new SqlMetaData("LotId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("LotNumber", SqlDbType.NVarChar, 40);
            columnStructures[2] = new SqlMetaData("OriginalQuantity", SqlDbType.Decimal, 18, 4);
            columnStructures[3] = new SqlMetaData("BalanceQuantity", SqlDbType.Decimal, 18, 4);
            columnStructures[4] = new SqlMetaData("ReceivePrice", SqlDbType.Decimal, 18, 4);
            columnStructures[5] = new SqlMetaData("PurchaseOrderReceiveLineId", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("Expirydate", SqlDbType.DateTime);
            columnStructures[7] = new SqlMetaData("IsActive", SqlDbType.Bit);
            columnStructures[8] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[9] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[10] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[11] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[12] = new SqlMetaData("SourceSystemReceiveLineID", SqlDbType.NVarChar, 100);
            columnStructures[13] = new SqlMetaData("LastUpdatedDate", SqlDbType.DateTime);
            columnStructures[14] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[15] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[16] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[17] = new SqlMetaData("UOMId", SqlDbType.Int);

            for (int i = 0; i < inventoryLotDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].LotId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].LotNumber));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue((decimal)inventoryLotDTOList[i].OriginalQuantity));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue((decimal)inventoryLotDTOList[i].BalanceQuantity));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue((decimal)inventoryLotDTOList[i].ReceivePrice));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].PurchaseOrderReceiveLineId, true));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].Expirydate));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].IsActive));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(Guid.Parse(inventoryLotDTOList[i].Guid)));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].SynchStatus));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].SourceSystemReceiveLineID));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].CreationDate));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(inventoryLotDTOList[i].UOMId, true));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }
        private Dictionary<string, InventoryLotDTO> GetDTOGuidMap(List<InventoryLotDTO> inventoryLotDTOList)
        {
            log.LogMethodEntry();
            Dictionary<string, InventoryLotDTO> result = new Dictionary<string, InventoryLotDTO>();
            for (int i = 0; i < inventoryLotDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(inventoryLotDTOList[i].Guid))
                {
                    inventoryLotDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(inventoryLotDTOList[i].Guid, inventoryLotDTOList[i]);
            }
            log.LogMethodExit();
            return result;
        }
        private void Update(Dictionary<string, InventoryLotDTO> inventoryLotDTOGuidMap, DataTable table)
        {
            log.LogMethodEntry();
            foreach (DataRow row in table.Rows)
            {
                InventoryLotDTO inventoryLotDTO = inventoryLotDTOGuidMap[Convert.ToString(row["Guid"])];
                inventoryLotDTO.LotId = row["LotId"] == DBNull.Value ? -1 : Convert.ToInt32(row["LotId"]);
                inventoryLotDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                inventoryLotDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                inventoryLotDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                inventoryLotDTO.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdatedDate"]);
                inventoryLotDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                inventoryLotDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
        private List<InventoryLotDTO> GetUpdatedDTOListFromDictonary(Dictionary<string, InventoryLotDTO> inventoryLotDTOGuidMap)
        {
            log.LogMethodEntry();
            List<InventoryLotDTO> inventoryLotDTOList = new List<InventoryLotDTO>();
            Dictionary<string, InventoryLotDTO>.ValueCollection valueCollectionList = inventoryLotDTOGuidMap.Values;
            if (valueCollectionList != null)
            {
                foreach (InventoryLotDTO dtoValue in valueCollectionList)
                {
                    inventoryLotDTOList.Add(dtoValue);
                }
            }
            log.LogMethodExit();
            return inventoryLotDTOList;
        }
    }

}
