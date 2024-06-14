/********************************************************************************************
 * Project Name -Inventory 
 * Description  -Data handler of inventory Transaction  
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        14-Jul-2019    Deeksha          Modifications as per three tier standard
 *2.70.2        09-Dec-2019    Jinto Thomas     Removed site id from update query 
 *2.100.0       27-Jul-2020    Deeksha          Modified : Added UOMId field
 ********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory Transaction Data Handler
    /// </summary>
    public class InventoryTransactionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM InventoryTransaction AS it ";
       
        /// <summary>
        /// Dictionary for searching Parameters for the Inventory Transaction  object.
        /// </summary>
        private static readonly Dictionary<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string> DBSearchParameters = new Dictionary<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>
               {
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.TRANSACTION_ID, "it.TrxId"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.PRODUCT_ID, "it.ProductId"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.LOCATION_ID, "it.LocationId"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.LOT_ID,"it.LotId"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.INVENTORY_TRANSACTION_TYPE_ID,"it.InventoryTransactionTypeId"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.SITE_ID,"it.site_id"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.MASTER_ENTITY_ID,"it.MasterEntityId"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.LINE_ID,"it.LineId"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.APPLICABILITY,"it.Applicability"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.ORIGINAL_REFERENCE_GUID,"it.OriginalReferenceGUID"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.UOM_ID,"it.UOMId"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.TRANSACTION_FROM_DATE,"it.TrxDate"},
                    {InventoryTransactionDTO.SearchByInventoryTransactionParameters.TRANSACTION_TO_DATE,"it.TrxDate "}
               };

        /// <summary>
        /// Default constructor of InventoryTransactionDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public InventoryTransactionDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to InventoryTransactionDTO class type
        /// </summary>
        /// <param name="inventoryTransactionDataRow">InventoryTransaction DataRow</param>
        /// <returns>Returns InventoryTransactionDTO</returns>
        private InventoryTransactionDTO GetInventoryTransactionDTO(DataRow inventoryTransactionDataRow)
        {
            log.LogMethodEntry(inventoryTransactionDataRow);
            InventoryTransactionDTO inventoryTransactionDataObject = new InventoryTransactionDTO(Convert.ToInt32(inventoryTransactionDataRow["TrxId"]),
                                                          inventoryTransactionDataRow["ParafaitTrxId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryTransactionDataRow["ParafaitTrxId"]),
                                                          inventoryTransactionDataRow["TrxDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryTransactionDataRow["TrxDate"]),
                                                          inventoryTransactionDataRow["UserName"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryTransactionDataRow["UserName"]),
                                                          inventoryTransactionDataRow["POSMachine"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryTransactionDataRow["POSMachine"]),
                                                          inventoryTransactionDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryTransactionDataRow["ProductId"]),
                                                          inventoryTransactionDataRow["LocationId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryTransactionDataRow["LocationId"]),
                                                          inventoryTransactionDataRow["Quantity"] == DBNull.Value ? 0 : Convert.ToDouble(inventoryTransactionDataRow["Quantity"]),
                                                          inventoryTransactionDataRow["SalePrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryTransactionDataRow["SalePrice"]),
                                                          inventoryTransactionDataRow["TaxPercentage"] == DBNull.Value ? 0 : Convert.ToDouble(inventoryTransactionDataRow["TaxPercentage"]),
                                                          inventoryTransactionDataRow["TaxInclusivePrice"] == DBNull.Value ? "N" : Convert.ToString(inventoryTransactionDataRow["TaxInclusivePrice"]),
                                                          inventoryTransactionDataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryTransactionDataRow["LineId"]),
                                                          inventoryTransactionDataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryTransactionDataRow["POSMachineId"]),
                                                          inventoryTransactionDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryTransactionDataRow["site_id"]),
                                                          inventoryTransactionDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryTransactionDataRow["Guid"]),
                                                          inventoryTransactionDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryTransactionDataRow["SynchStatus"]),
                                                          inventoryTransactionDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryTransactionDataRow["MasterEntityId"]),
                                                          inventoryTransactionDataRow["InventoryTransactionTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryTransactionDataRow["InventoryTransactionTypeId"]),
                                                          inventoryTransactionDataRow["LotId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryTransactionDataRow["LotId"]),
                                                          inventoryTransactionDataRow["Applicability"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryTransactionDataRow["Applicability"]),
                                                          inventoryTransactionDataRow["originalReferenceGUID"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryTransactionDataRow["originalReferenceGUID"]),
                                                          inventoryTransactionDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryTransactionDataRow["CreatedBy"]),
                                                          inventoryTransactionDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryTransactionDataRow["CreationDate"]),
                                                          inventoryTransactionDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryTransactionDataRow["LastUpdatedBy"]),
                                                          inventoryTransactionDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryTransactionDataRow["LastUpdateDate"]),
                                                          inventoryTransactionDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryTransactionDataRow["UOMId"])
                                                         );
            log.LogMethodExit(inventoryTransactionDataObject);
            return inventoryTransactionDataObject;
        }
       
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Inventory Transaction Record.
        /// </summary>
        /// <param name="inventoryTransactionDTO">InventoryTransactionDTO type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(InventoryTransactionDTO inventoryTransactionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryTransactionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@transactionId", inventoryTransactionDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parafaitTransactionId", inventoryTransactionDTO.ParafaitTransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@transactionDate", ServerDateTime.Now));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userName", inventoryTransactionDTO.UserName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posMachine", inventoryTransactionDTO.POSMachine));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", inventoryTransactionDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@locationId", inventoryTransactionDTO.LocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@quantity", inventoryTransactionDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@salePrice", inventoryTransactionDTO.SalePrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxPercentage", inventoryTransactionDTO.TaxPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxInclusivePrice", inventoryTransactionDTO.TaxInclusivePrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lineId", inventoryTransactionDTO.LineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posMachineId", inventoryTransactionDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", inventoryTransactionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@inventoryTransactionTypeId", inventoryTransactionDTO.InventoryTransactionTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lotId", inventoryTransactionDTO.LotId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@applicability", inventoryTransactionDTO.Applicability, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@originalReferenceGUID", inventoryTransactionDTO.OriginalReferenceGUID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", inventoryTransactionDTO.UOMId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ProductDiscounts record to the database
        /// </summary>
        /// <param name="inventoryTransactionDTO">ProductDiscountsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">Sql transaction object.</param>
        /// <returns>Returns inserted record id</returns>
        public InventoryTransactionDTO InsertInventoryTransactionDTO(InventoryTransactionDTO inventoryTransactionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryTransactionDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[InventoryTransaction]
                                        (   ParafaitTrxId,
                                            TrxDate,
                                            Username,
                                            POSMachine,
                                            ProductId,
                                            LocationId,
                                            Quantity,
                                            SalePrice,
                                            TaxPercentage,
                                            TaxInclusivePrice,
                                            LineId,
                                            POSMachineId,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            InventoryTransactionTypeID,
                                            LotID,
                                            Applicability,
                                            OriginalReferenceGUID,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            UOMId
                                        ) 
                                VALUES 
                                        (
                                            @parafaitTransactionId,
                                            @transactionDate,
                                            @userName,
                                            @posMachine,
                                            @productId,
                                            @locationId,
                                            @quantity,
                                            @salePrice,
                                            @taxPercentage,
                                            @taxInclusivePrice,
                                            @lineId,
                                            @posMachineId,                                            
                                            @site_id,
                                            NEWID(),
                                            @MasterEntityId,
                                            @inventoryTransactionTypeId,
                                            @lotId,
                                            @applicability,
                                            @originalReferenceGUID,
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE(),
                                            @UOMId
                                            )    
                                       SELECT* FROM InventoryTransaction WHERE TrxId = scope_identity()";


            try
            {
               
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(inventoryTransactionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryTransactionDTO(inventoryTransactionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting InventoryTransactionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryTransactionDTO);
            return inventoryTransactionDTO;
        }



        /// <summary>
        /// Updates the ProductDiscounts record
        /// </summary>
        /// <param name="inventoryTransactionDTO">ProductDiscountsDTO type parameter</param>
        /// <param name="loginId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">Sql transaction object.</param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryTransactionDTO UpdateInventoryTransactionDTO(InventoryTransactionDTO inventoryTransactionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryTransactionDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[InventoryTransaction]
                                    SET  ParafaitTrxId = @parafaitTransactionId,
                                            TrxDate = @transactionDate,
                                            Username = @userName,
                                            POSMachine=@posMachine,
                                            ProductId=@productId,
                                            LocationId=@locationId,
                                            Quantity=@quantity,
                                            SalePrice=@salePrice,
                                            TaxPercentage=@taxPercentage,
                                            TaxInclusivePrice=@taxInclusivePrice,
                                            LineId=@lineId,
                                            POSMachineId=@posMachineId,
                                            --site_id=@site_id,
                                            MasterEntityId=@MasterEntityId,
                                            InventoryTransactionTypeID=@inventoryTransactionTypeId,
                                            LotID=@lotId,
                                            Applicability = @applicability ,
                                            OriginalReferenceGUID = @originalReferenceGUID,
                                            lastUpdatedBy=@lastUpdatedBy,
                                            lastUpdateDate=getdate(),
                                            UOMId=@UOMId
                                         WHERE TrxId  = @transactionId
                                                 SELECT * FROM InventoryTransaction WHERE TrxId = @transactionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(inventoryTransactionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryTransactionDTO(inventoryTransactionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating inventoryTransactionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryTransactionDTO);
            return inventoryTransactionDTO;
        }

        /// <summary>
        /// Delete the record from the InventoryTransaction database based on transactionId
        /// </summary>
        /// <param name="transactionId">transactionId </param>
        /// <returns>return the int </returns>
        internal int Delete(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            string query = @"DELETE  
                             FROM InventoryTransaction
                             WHERE InventoryTransaction.TrxId = @transactionId";
            SqlParameter parameter = new SqlParameter("@transactionId", transactionId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="InventoryTransactionDTO">InventoryTransactionDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshInventoryTransactionDTO(InventoryTransactionDTO inventoryTransactionDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryTransactionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryTransactionDTO.TransactionId = Convert.ToInt32(dt.Rows[0]["TrxId"]);
                inventoryTransactionDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                inventoryTransactionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryTransactionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryTransactionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryTransactionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryTransactionDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the InventoryTransaction data of passed id 
        /// </summary>
        /// <param name="id">id of InventoryTransaction is passed as parameter</param>
        /// <returns>Returns InventoryTransaction</returns>
        public InventoryTransactionDTO GetInventoryTransactionDTO(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            InventoryTransactionDTO result = null;
            string query = SELECT_QUERY + @" WHERE it.TrxId= @transactionId";
            SqlParameter parameter = new SqlParameter("@transactionId", transactionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetInventoryTransactionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the InventoryTransactionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">List of search parameters sql transaction</param>
        /// <returns>Returns the list of InventoryTransactionDTO matching the search criteria</returns>
        public List<InventoryTransactionDTO> GetInventoryTransactionList(List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryTransactionDTO> inventoryTransactionDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectTransactionQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.INVENTORY_TRANSACTION_TYPE_ID) ||
                                searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.LOCATION_ID) ||
                                searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.LOT_ID) ||
                                searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.PRODUCT_ID) ||
                                searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.TRANSACTION_ID) ||
                                searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.MASTER_ENTITY_ID) ||
                                searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.UOM_ID) ||
                                searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.LINE_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.APPLICABILITY) ||
                        searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.ORIGINAL_REFERENCE_GUID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryTransactionDTO.SearchByInventoryTransactionParameters.TRANSACTION_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == InventoryTransactionDTO.SearchByInventoryTransactionParameters.TRANSACTION_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(InventoryTransactionDTO.SearchByInventoryTransactionParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                selectTransactionQuery = selectTransactionQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectTransactionQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryTransactionDTOList = new List<InventoryTransactionDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryTransactionDTO inventoryTransactionDTO = GetInventoryTransactionDTO(dataRow);
                    inventoryTransactionDTOList.Add(inventoryTransactionDTO);
                }
            }
            log.LogMethodExit(inventoryTransactionDTOList);
            return inventoryTransactionDTOList;
        }
    }
}