/********************************************************************************************
* Project Name -Inventory DataHandler
* Description  -Data object of inventoryHistory 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        3-Jan-2017    Amaresh          Created 
*2.70.2      18-Aug-2019   Deeksha          Modifications as per 3 tier standard.
*2.70.2      11-Dec-2019   Jinto Thomas     Removed siteid from update query
*2.70.2      26-Dec-2019   Deeksha          Inventory Next-Rel Enhancement changes.
*2.100.0     27-Jul-2020   Deeksha          Added UOMId field.
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// class of InventoryHistoryDataHandler
    /// </summary>
    public class InventoryHistoryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM InventoryHist AS ih ";
        private List<SqlParameter> parameters = new List<SqlParameter>();
        private static readonly Dictionary<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string> DBSearchParameters = new Dictionary<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>
               {
                    {InventoryHistoryDTO.SearchByInventoryHistoryParameters.ID, "ih.ID"},
                    {InventoryHistoryDTO.SearchByInventoryHistoryParameters.PRODUCT_ID, "ih.ProductId"},
                    {InventoryHistoryDTO.SearchByInventoryHistoryParameters.LOCATION_ID, "ih.LocationId"},
                    {InventoryHistoryDTO.SearchByInventoryHistoryParameters.PHYSICAL_COUNT_ID, "ih.PhysicalCountId"},
                    {InventoryHistoryDTO.SearchByInventoryHistoryParameters.SITE_ID, "ih.site_id"},
                    {InventoryHistoryDTO.SearchByInventoryHistoryParameters.QUANTITY, "ih.Quantity"},
                    {InventoryHistoryDTO.SearchByInventoryHistoryParameters.LOT_ID, "ih.LotId"},
                    {InventoryHistoryDTO.SearchByInventoryHistoryParameters.MASTER_ENTITY_ID, "ih.MasterEntityId"},
                    {InventoryHistoryDTO.SearchByInventoryHistoryParameters.MODIFIED_DURING_PHYSICAL_COUNT, ""},
                    {InventoryHistoryDTO.SearchByInventoryHistoryParameters.UOM_ID, "ih.UOMId"}
               };
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of InventoryHistoryDataHandler class
        /// </summary>
        public InventoryHistoryDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to InventoryHistoryDTO class type
        /// </summary>
        /// <param name="inventoryHistoryDataRow">InventoryHistoryDTO DataRow</param>
        /// <returns>Returns InventoryHistoryDTO</returns>
        private InventoryHistoryDTO GetInventoryHistoryDTO(DataRow inventoryHistoryDataRow)
        {
            log.LogMethodEntry(inventoryHistoryDataRow);

            InventoryHistoryDTO inventoryHistoryDataObject = new InventoryHistoryDTO(Convert.ToInt32(inventoryHistoryDataRow["ID"]),
                                             inventoryHistoryDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryHistoryDataRow["ProductId"]),
                                             inventoryHistoryDataRow["LocationId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryHistoryDataRow["LocationId"]),
                                             inventoryHistoryDataRow["PhysicalCountId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryHistoryDataRow["PhysicalCountId"]),
                                             inventoryHistoryDataRow["Quantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryHistoryDataRow["Quantity"]),
                                             inventoryHistoryDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryHistoryDataRow["Timestamp"]),
                                             inventoryHistoryDataRow["Lastupdated_userid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryHistoryDataRow["Lastupdated_userid"]),
                                             inventoryHistoryDataRow["AllocatedQuantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryHistoryDataRow["AllocatedQuantity"]),
                                             inventoryHistoryDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryHistoryDataRow["site_id"]),
                                             inventoryHistoryDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryHistoryDataRow["Guid"]),
                                             inventoryHistoryDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryHistoryDataRow["SynchStatus"]),
                                             inventoryHistoryDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryHistoryDataRow["MasterEntityId"]),
                                             inventoryHistoryDataRow["LotId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryHistoryDataRow["LotId"]),
                                             inventoryHistoryDataRow["ReceivePrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryHistoryDataRow["ReceivePrice"]),
                                             inventoryHistoryDataRow["InitialCount"] == DBNull.Value ? false : Convert.ToBoolean(inventoryHistoryDataRow["InitialCount"]),
                                             inventoryHistoryDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryHistoryDataRow["CreatedBy"]),
                                             inventoryHistoryDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryHistoryDataRow["CreationDate"]),
                                             inventoryHistoryDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryHistoryDataRow["LastUpdateDate"]),
                                             inventoryHistoryDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryHistoryDataRow["UOMId"])
                                             );
            log.LogMethodExit(inventoryHistoryDataObject);
            return inventoryHistoryDataObject;
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryHistoryDataHandler Record.
        /// </summary>
        /// <param name="InventoryHistoryDTO">InventoryHistoryDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="site_id">site_Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(InventoryHistoryDTO inventoryHistoryDTO, string loginId, int site_id)
        {
            log.LogMethodEntry(inventoryHistoryDTO, loginId, site_id);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", inventoryHistoryDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", inventoryHistoryDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@locationId", inventoryHistoryDTO.LocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@physicalCountId", inventoryHistoryDTO.PhysicalCountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lotId", inventoryHistoryDTO.LotId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@quantity", inventoryHistoryDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@allocatedQuantity", inventoryHistoryDTO.AllocatedQuantity == 0 ? DBNull.Value : (object)inventoryHistoryDTO.AllocatedQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@receivePrice", inventoryHistoryDTO.ReceivePrice == 0 ? DBNull.Value : (object)inventoryHistoryDTO.ReceivePrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", inventoryHistoryDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", site_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastupdatedUserid", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InitialCount", inventoryHistoryDTO.InitialCount == true ? true : false));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", inventoryHistoryDTO.UOMId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the inventoryHistory record to the database
        /// </summary>
        /// <param name="InventoryHistoryDTO">InventoryHistoryDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="site_id">site_id</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <returns></returns>
        public InventoryHistoryDTO InsertInventoryHistory(InventoryHistoryDTO InventoryHistoryDTO, string loginId, int site_id)
        {
            log.LogMethodEntry(InventoryHistoryDTO, loginId, site_id);
            string insertInventoryQuery = @"insert into InventoryHist 
                                                        (
                                                        ProductId,
                                                        LocationId,
                                                        PhysicalCountId,
                                                        Quantity,
                                                        Timestamp,
                                                        Lastupdated_userid,
                                                        AllocatedQuantity,
                                                        site_id,
                                                        Guid,
                                                        MasterEntityId,
                                                        LotId,
                                                        ReceivePrice,
                                                        InitialCount,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdateDate,
                                                        UOMId
                                                        ) 
                                                values 
                                                        ( 
                                                         @productId,
                                                         @locationId,
                                                         @physicalCountId,
                                                         @quantity,
                                                         Getdate(),
                                                         @lastupdatedUserid,
                                                         @allocatedQuantity,        
                                                         @site_id,
                                                         NEWID(),
                                                         @masterEntityId,
                                                         @lotId, 
                                                         @receivePrice,
                                                         @InitialCount,
                                                         @createdBy,
                                                         Getdate(),
                                                         Getdate(),
                                                         @UOMId
                                                        )SELECT * FROM InventoryHist WHERE ID = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertInventoryQuery, GetSQLParameters(InventoryHistoryDTO, loginId, site_id).ToArray(), sqlTransaction);
                RefreshInventoryHistoryDTO(InventoryHistoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting InventoryHistoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(InventoryHistoryDTO);
            return InventoryHistoryDTO;
        }

        /// <summary>
        /// Updates the InventoryHistory record
        /// </summary>
        /// <param name="inventoryHistoryDTO">InventoryHistoryDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="site_id">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryHistoryDTO UpdateInventoryHistoryDTO(InventoryHistoryDTO inventoryHistoryDTO, string loginId, int site_id)
        {
            log.LogMethodEntry(inventoryHistoryDTO, loginId, site_id);
            string query = @"UPDATE  [dbo].[InventoryHist]
                                    SET 
                                             ProductId = @productId,
                                             LocationId = @locationId,
                                             PhysicalCountId = @physicalCountId,
                                             Quantity = @quantity,
                                             Timestamp = Getdate() ,
                                             Lastupdated_userid = @lastupdatedUserid,
                                             AllocatedQuantity = @allocatedQuantity,
                                             --site_id = @site_id,
                                             MasterEntityId = @masterEntityId,
                                             LotId = @lotId,
                                             ReceivePrice = @receivePrice,
                                             InitialCount = @InitialCount,
                                             LastUpdateDate = GETDATE(),
                                             UOMId = @UOMId
                                       WHERE ID =@Id 
                                    SELECT * FROM InventoryHist WHERE ID = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryHistoryDTO, loginId, site_id).ToArray(), sqlTransaction);
                RefreshInventoryHistoryDTO(inventoryHistoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating inventoryHistoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryHistoryDTO);
            return inventoryHistoryDTO;
        }

        /// <summary>
        /// Delete the record from the InventoryHistory database based on Id
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM InventoryHist
                             WHERE InventoryHist.ID = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            int id1 = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id1);
            return id1;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="inventoryHistoryDTO">InventoryHistoryDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshInventoryHistoryDTO(InventoryHistoryDTO inventoryHistoryDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryHistoryDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryHistoryDTO.Id = Convert.ToInt32(dt.Rows[0]["ID"]);
                inventoryHistoryDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                inventoryHistoryDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryHistoryDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryHistoryDTO.LastupdatedUserid = dataRow["Lastupdated_userid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Lastupdated_userid"]);
                inventoryHistoryDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryHistoryDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the no of InventoryHistoryDTO matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetInventoryHistoryCount(List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                count = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// Returns the sql query based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of query</returns>
        public string GetFilterQuery(List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.ID
                            || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.PRODUCT_ID
                            || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.LOCATION_ID
                            || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.PHYSICAL_COUNT_ID
                            || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.LOT_ID
                            || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.UOM_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.QUANTITY)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDecimal(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.MODIFIED_DURING_PHYSICAL_COUNT)
                        {
                            query.Append(joiner + "(ISNULL((select top 1 1 from InventoryAdjustments ia where ia.OriginalReferenceId = ih.id), 0) = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " )");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// Gets the InventoryHistoryDTO data of passed Id
        /// </summary>
        /// <param name="id">Int type parameter</param>
        /// <returns>Returns InventoryHistoryDTO</returns>
        public InventoryHistoryDTO GetInventoryHistory(int id)
        {
            log.LogMethodEntry(id);
            string selectInventoryHistoryQuery = @"SELECT *
                                                    FROM InventoryHist
                                                    WHERE ID = @Id";

            SqlParameter[] selectInventoryParameters = new SqlParameter[1];
            selectInventoryParameters[0] = new SqlParameter("@Id", id);
            DataTable inventoryHistory = dataAccessHandler.executeSelectQuery(selectInventoryHistoryQuery, selectInventoryParameters, sqlTransaction);

            if (inventoryHistory.Rows.Count > 0)
            {
                DataRow inventoryHistoryRow = inventoryHistory.Rows[0];
                InventoryHistoryDTO inventoryDataObject = GetInventoryHistoryDTO(inventoryHistoryRow);
                log.LogMethodExit(inventoryDataObject);
                return inventoryDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the InventoryHistoryDTO data of passed productId, locationId, lotId
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="locationId">locationId</param>
        /// <param name="lotId">lotId</param>
        /// <returns></returns>
        public InventoryHistoryDTO GetInventoryHistory(int productId, int locationId, int lotId = -1)
        {
            log.LogMethodEntry(productId, locationId, lotId);

            string selectInventoryQuery = @"SELECT *
                                                    FROM InventoryHist
                                                    WHERE ProductId = @productId 
                                                    AND LocationId = @locationId
                                                    AND (LotId = @lotId or Isnull(LotId,-1) = @lotId)";

            SqlParameter[] selectInventoryParameters = new SqlParameter[3];

            selectInventoryParameters[0] = new SqlParameter("@productId", productId);
            selectInventoryParameters[1] = new SqlParameter("@locationId", locationId);
            selectInventoryParameters[2] = new SqlParameter("@lotId", lotId);

            DataTable inventoryHistory = dataAccessHandler.executeSelectQuery(selectInventoryQuery, selectInventoryParameters, sqlTransaction);

            if (inventoryHistory.Rows.Count > 0)
            {
                DataRow inventoryHistoryRow = inventoryHistory.Rows[0];
                InventoryHistoryDTO inventoryHistoryDataObject = GetInventoryHistoryDTO(inventoryHistoryRow);
                log.LogMethodExit(inventoryHistoryDataObject);
                return inventoryHistoryDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the InventoryHistoryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryHistoryDTO matching the search criteria</returns>
        public List<InventoryHistoryDTO> GetInventoryHistoryList(List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryHistoryDTO> inventoryHistoryDTOList = null;
            parameters.Clear();
            string selectInventoryHistoryQuery = SELECT_QUERY;
            selectInventoryHistoryQuery += GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectInventoryHistoryQuery += " ORDER BY ih.Id OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectInventoryHistoryQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectInventoryHistoryQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryHistoryDTOList = new List<InventoryHistoryDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryHistoryDTO inventoryHistoryDTO = GetInventoryHistoryDTO(dataRow);
                    inventoryHistoryDTOList.Add(inventoryHistoryDTO);
                }
            }
            log.LogMethodExit(inventoryHistoryDTOList);
            return inventoryHistoryDTOList;
        }
        /* if ((searchParameters != null) && (searchParameters.Count > 0))
         {
             string joiner;
             int count = 0;
             StringBuilder query = new StringBuilder(" where ");
             foreach (KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string> searchParameter in searchParameters)
             {
                 joiner = (count == 0) ? string.Empty : " and ";
                 if (DBSearchParameters.ContainsKey(searchParameter.Key))
                 {
                     if (searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.ID
                         || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.PHYSICAL_COUNT_ID
                         || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.PRODUCT_ID
                         || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.LOCATION_ID
                         || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.LOT_ID
                         || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.UOM_ID
                         || searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.MASTER_ENTITY_ID)
                     {
                         query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                         parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                     }
                     else if (searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.QUANTITY)
                     {
                         query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                         parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDecimal(searchParameter.Value)));
                     }
                     else if (searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.MODIFIED_DURING_PHYSICAL_COUNT)
                     {
                         query.Append(joiner + "(ISNULL((select top 1 1 from InventoryAdjustments ia where ia.OriginalReferenceId = ih.id), 0) = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " )");
                         parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                     }
                     else if (searchParameter.Key == InventoryHistoryDTO.SearchByInventoryHistoryParameters.SITE_ID)
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
             if (searchParameters.Count > 0)
                 selectInventoryHistoryQuery = selectInventoryHistoryQuery + query;
         }
         DataTable inventoryHistoryData = dataAccessHandler.executeSelectQuery(selectInventoryHistoryQuery, parameters.ToArray(), sqlTransaction);
         if (inventoryHistoryData.Rows.Count > 0)
         {
             inventoryHistoryList = new List<InventoryHistoryDTO>();
             foreach (DataRow inventoryhistotyDataRow in inventoryHistoryData.Rows)
             {
                 InventoryHistoryDTO inventoryHistDataObject = GetInventoryHistoryDTO(inventoryhistotyDataRow);
                 inventoryHistoryList.Add(inventoryHistDataObject);
             }
         }
         log.LogMethodExit(inventoryHistoryList);
         return inventoryHistoryList;
     }*/
    }
}
