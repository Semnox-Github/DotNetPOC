/********************************************************************************************
 * Project Name - Inventory Issue Lines Data Handler
 * Description  - Data handler of the inventory issue lines class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        09-Aug-2015   Raghuveera        Created 
 *2.60        22-March-2019 Girish Kundar     Adding Issue Number
 *2.70.2      14-Jul-2019   Deeksha           Modified:Added GetSqlParameter(),SQL injection issue Fix
 *2.100.0     27-Jul-2020   Deeksha           Modified : Added UOMId field.
 *2.110.0     29-Dec-2020   Prajwal S         Modified : Added GetInventoryIssueLinesDTOList, Returns List of InventoryIssueLinesDTO
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory Issue Lines  - Handles insert, update and select of inventory issue header objects
    /// </summary>
    public class InventoryIssueLinesDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM InventoryIssueLines AS il ";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Dictionary for searching Parameters for the InventoryIssueLines object.
        /// </summary>
        private static readonly Dictionary<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string> DBSearchParameters = new Dictionary<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>
            {
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ISSUE_LINE_ID, "il.IssueLineId"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ISSUE_ID, "il.IssueId"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.PRODUCT_ID, "il.ProductId"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.QUANTITY, "il.Quantity"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.MASTER_ENTITY_ID,"il.MasterEntityId"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.SITE_ID, "il.site_id"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.REQUISITION_LINE_ID, "il.RequisitionLineId"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.FROM_LOCATION_ID, "il.FromLocationID"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.TO_LOCATION_ID, "il.ToLocationID"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.REQUISITION_ID, "il.RequisitionID"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ACTIVE_FLAG, "il.IsActive"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ORIGINAL_REFERENCE_GUID, "il.OriginalReferenceGUID"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.GUID, "il.Guid"},
                {InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.UOM_ID, "il.UOMId"}
             };

        /// <summary>
        /// Default constructor of InventoryIssueLinesDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public InventoryIssueLinesDataHandler(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryIssueLinesHandler Record.
        /// </summary>
        /// <param name="InventoryIssueLinesDTO">InventoryIssueLinesDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(InventoryIssueLinesDTO inventoryIssueLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryIssueLinesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@IssueLineId", inventoryIssueLinesDTO.IssueLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@issueId", inventoryIssueLinesDTO.IssueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", inventoryIssueLinesDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@quantity", inventoryIssueLinesDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fromLocationID", inventoryIssueLinesDTO.FromLocationID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@toLocationID", inventoryIssueLinesDTO.ToLocationID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionID", inventoryIssueLinesDTO.RequisitionID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionLineId", inventoryIssueLinesDTO.RequisitionLineID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", inventoryIssueLinesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", inventoryIssueLinesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@originalReferenceGUID", string.IsNullOrEmpty(inventoryIssueLinesDTO.OriginalReferenceGUID) ? DBNull.Value : (object)inventoryIssueLinesDTO.OriginalReferenceGUID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@issueNumber", string.IsNullOrEmpty(inventoryIssueLinesDTO.IssueNumber) ? DBNull.Value : (object)inventoryIssueLinesDTO.IssueNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", inventoryIssueLinesDTO.UOMId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the asset type record to the database
        /// </summary>
        /// <param name="inventoryIssueLinesDTO">InventoryIssueLinesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">SQL Transactions </param>
        /// <returns>Returns inserted record id</returns>
        public InventoryIssueLinesDTO InsertInventoryIssueLines(InventoryIssueLinesDTO inventoryIssueLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryIssueLinesDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[InventoryIssueLines]
                                                        (
                                                        IssueId,
                                                        ProductId,
                                                        Quantity,
                                                        FromLocationID,
                                                        ToLocationID,
                                                        RequisitionID,
                                                        RequisitionLineId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id,
                                                        OriginalReferenceGUID,
                                                        IssueNumber,
                                                        UOMId
                                                        ) 
                                                values 
                                                        ( 
                                                         @issueId,
                                                         @productId,
                                                         @quantity,
                                                         @fromLocationID,
                                                         @toLocationID,
                                                         @requisitionID,
                                                         @requisitionLineId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @originalReferenceGUID,
                                                         @issueNumber,
                                                         @UOMId)
                                SELECT * FROM InventoryIssueLines WHERE IssueLineId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryIssueLinesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryIssueLinesDTO(inventoryIssueLinesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting inventoryIssueLinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryIssueLinesDTO);
            return inventoryIssueLinesDTO;
        }


        /// <summary>
        /// Updates the Asset type record
        /// </summary>
        /// <param name="inventoryIssueLinesDTO">InventoryIssueLinesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">SQL Transactions </param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryIssueLinesDTO UpdateInventoryIssueLines(InventoryIssueLinesDTO inventoryIssueLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryIssueLinesDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[InventoryIssueLines]
                                    SET     IssueId=@issueId,
                                             ProductId=@productId,
                                             Quantity=@quantity,
                                             FromLocationID=@fromLocationID,
                                             ToLocationID=@toLocationID,
                                             RequisitionID=@requisitionID,
                                             RequisitionLineId=@requisitionLineId,                                         
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             site_id=@siteid,
                                             MasterEntityId=@masterEntityId,
                                             OriginalReferenceGUID=@originalReferenceGUID,
                                             IssueNumber = @issueNumber,
                                             UOMId = @UOMId
                                        WHERE IssueLineId =@IssueLineId 
                                    SELECT * FROM InventoryIssueLines WHERE IssueLineId = @IssueLineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryIssueLinesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryIssueLinesDTO(inventoryIssueLinesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating inventoryIssueLinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryIssueLinesDTO);
            return inventoryIssueLinesDTO;
        }


        /// <summary>
        /// Converts the Data row object to InventoryIssueLinesDTO class type
        /// </summary>
        /// <param name="inventoryIssueLinesDataRow">InventoryIssueLines DataRow</param>
        /// <returns>Returns InventoryIssueLines</returns>
        private InventoryIssueLinesDTO GetInventoryIssueLinesDTO(DataRow inventoryIssueLinesDataRow)
        {
            log.LogMethodEntry(inventoryIssueLinesDataRow);
            InventoryIssueLinesDTO inventoryIssueLinesDataObject = new InventoryIssueLinesDTO(Convert.ToInt32(inventoryIssueLinesDataRow["IssueLineId"]),
                                            inventoryIssueLinesDataRow["IssueId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueLinesDataRow["IssueId"]),
                                            inventoryIssueLinesDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueLinesDataRow["ProductId"]),
                                            inventoryIssueLinesDataRow["Quantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(inventoryIssueLinesDataRow["Quantity"]),
                                            inventoryIssueLinesDataRow["FromLocationID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueLinesDataRow["FromLocationID"]),
                                            inventoryIssueLinesDataRow["ToLocationID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueLinesDataRow["ToLocationID"]),
                                            inventoryIssueLinesDataRow["RequisitionID"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueLinesDataRow["RequisitionID"]),
                                            inventoryIssueLinesDataRow["RequisitionLineId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueLinesDataRow["RequisitionLineId"]),
                                            inventoryIssueLinesDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(inventoryIssueLinesDataRow["IsActive"]),
                                            inventoryIssueLinesDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueLinesDataRow["CreatedBy"]),
                                            inventoryIssueLinesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryIssueLinesDataRow["CreationDate"]),
                                            inventoryIssueLinesDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueLinesDataRow["LastUpdatedBy"]),
                                            inventoryIssueLinesDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryIssueLinesDataRow["LastupdatedDate"]),
                                            inventoryIssueLinesDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueLinesDataRow["Guid"]),
                                            inventoryIssueLinesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueLinesDataRow["site_id"]),
                                            inventoryIssueLinesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryIssueLinesDataRow["SynchStatus"]),
                                            inventoryIssueLinesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueLinesDataRow["MasterEntityId"]),
                                            inventoryIssueLinesDataRow["OriginalReferenceGUID"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueLinesDataRow["OriginalReferenceGUID"]),
                                            inventoryIssueLinesDataRow["IssueNumber"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryIssueLinesDataRow["IssueNumber"]),
                                            inventoryIssueLinesDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryIssueLinesDataRow["UOMId"])
                                            );
            log.LogMethodExit(inventoryIssueLinesDataObject);
            return inventoryIssueLinesDataObject;
        }

        /// <summary>
        /// Gets the InventoryIssueLinesDTO List for inventoryIssueIdList
        /// </summary>
        /// <param name="inventoryIssueIdList">integer list parameter</param>
        /// <returns>Returns List of InventoryIssueLinesDTO List</returns>
        public List<InventoryIssueLinesDTO> GetInventoryIssueLinesDTOList(List<int> inventoryIssueIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(inventoryIssueIdList);
            List<InventoryIssueLinesDTO> list = new List<InventoryIssueLinesDTO>();
            string query = @"SELECT InventoryIssueLines.*
                            FROM InventoryIssueLines, @inventoryIssueIdList List
                            WHERE IssueId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@inventoryIssueIdList", inventoryIssueIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetInventoryIssueLinesDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Delete the record from the inventoryIssueLine database based on IssueLineId
        /// </summary>
        /// <param name="issueLineId">issueLineId </param>
        /// <returns>return the int </returns>
        internal int Delete(int issueLineId)
        {
            log.LogMethodEntry(issueLineId);
            string query = @"DELETE  
                             FROM InventoryIssueLines
                             WHERE InventoryIssueLines.IssueLineId = @IssueLineId";
            SqlParameter parameter = new SqlParameter("@IssueLineId", issueLineId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="inventoryIssueLinesDTO">inventoryIssueLinesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshInventoryIssueLinesDTO(InventoryIssueLinesDTO inventoryIssueLinesDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryIssueLinesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryIssueLinesDTO.IssueLineId = Convert.ToInt32(dt.Rows[0]["IssueLineId"]);
                inventoryIssueLinesDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                inventoryIssueLinesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryIssueLinesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryIssueLinesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryIssueLinesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the InventoryIssueLines data of passed id 
        /// </summary>
        /// <param name="id">id of InventoryIssueLines is passed as parameter</param>
        /// <returns>Returns InventoryIssueLinesDTO</returns>
        public InventoryIssueLinesDTO GetInventoryIssueLines(int issueLineId)
        {
            log.LogMethodEntry(issueLineId);
            InventoryIssueLinesDTO result = null;
            string query = SELECT_QUERY + @" WHERE il.IssueLineId= @IssueLineId";
            SqlParameter parameter = new SqlParameter("@IssueLineId", issueLineId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetInventoryIssueLinesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
                     
        /// <summary>
        /// Gets the InventoryIssueLinesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">Sql transactions</param>
        /// <returns>Returns the list of InventoryIssueLinesDTO matching the search criteria</returns>
        public List<InventoryIssueLinesDTO> GetInventoryIssueLinesList(List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectInventoryIssueLinesQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        
                        if (searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ISSUE_LINE_ID
                            || searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ISSUE_ID
                            || searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.REQUISITION_ID
                            || searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.PRODUCT_ID
                            || searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.FROM_LOCATION_ID
                            || searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.UOM_ID
                            || searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.TO_LOCATION_ID)
                        
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.QUANTITY)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDouble(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ORIGINAL_REFERENCE_GUID)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.SITE_ID)
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
                selectInventoryIssueLinesQuery = selectInventoryIssueLinesQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectInventoryIssueLinesQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryIssueLinesDTO inventoryIssueLinesDTO = GetInventoryIssueLinesDTO(dataRow);
                    inventoryIssueLinesDTOList.Add(inventoryIssueLinesDTO);
                }
            }
            log.LogMethodExit(inventoryIssueLinesDTOList);
            return inventoryIssueLinesDTOList;
        }
    }
}