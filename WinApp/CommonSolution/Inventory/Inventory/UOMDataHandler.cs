/********************************************************************************************
* Project Name - UOMDataHandler 
* Description  - Data handler object of the UOM
* 
**************
**Version Log
**************
*Version       Date           Modified By             Remarks          
*********************************************************************************************
*2.60.3        14-June-2019   Nagesh Badiger           Added who columns in insert and update method.
*2.70.2        20-Jul-2019    Deeksha                  Modifications as per three tier standard.Removed GetUOMList(string sqlQuery).
*2.70.2        09-Dec-2019    Jinto Thomas             Removed siteid from update query 
*2.100.0       23-Jul-2020    Deeksha                  Modified for Recipe Management enhancement.
*2.110.0       07-Oct-2020    Mushahid Faizan          Web Inventory UI redesign with REST API.
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// UOM data handler - Handles insert, update and select of game data objects
    /// </summary>
    public class UOMDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM UOM AS uom ";
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        /// Dictionary for searching Parameters for the UOM object.
        /// </summary>
        private static readonly Dictionary<UOMDTO.SearchByUOMParameters, string> DBSearchParameters = new Dictionary<UOMDTO.SearchByUOMParameters, string>
               {
                    {UOMDTO.SearchByUOMParameters.UOMID, "uom.UOMId"},
                    {UOMDTO.SearchByUOMParameters.UOMID_LIST, "uom.UOMId"},
                    {UOMDTO.SearchByUOMParameters.UOM, "uom.UOM"},
                    {UOMDTO.SearchByUOMParameters.SITEID, "uom.site_id"},
                    {UOMDTO.SearchByUOMParameters.IS_ACTIVE, "uom.isactive"},
                    {UOMDTO.SearchByUOMParameters.MASTER_ENTITY_ID, "uom.MasterEntityId"}
               };

        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Constructor with SQltransaction as parameter
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public UOMDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to UOMDTO class type
        /// </summary>
        /// <param name="uomDataRow">UOMDTO DataRow</param>
        /// <returns>Returns UOMDTO</returns>
        private UOMDTO GetUOMDTO(DataRow uomDataRow)
        {
            log.LogMethodEntry(uomDataRow);

            UOMDTO uomDataObject = new UOMDTO(Convert.ToInt32(uomDataRow["UOMId"]),
                                            uomDataRow["UOM"] == DBNull.Value ? string.Empty : Convert.ToString(uomDataRow["UOM"]),
                                            uomDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(uomDataRow["Remarks"]),
                                            uomDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(uomDataRow["site_id"]),
                                            uomDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(uomDataRow["Guid"]),
                                            uomDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(uomDataRow["SynchStatus"]),
                                            uomDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(uomDataRow["MasterEntityId"]),
                                            uomDataRow["isactive"] == DBNull.Value ? true : Convert.ToBoolean(uomDataRow["isactive"]),
                                            uomDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(uomDataRow["LastUpdatedBy"]),
                                            uomDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(uomDataRow["CreationDate"]),
                                            uomDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(uomDataRow["LastUpdatedBy"]),
                                            uomDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(uomDataRow["LastUpdateDate"])
                                           );
            log.LogMethodExit(uomDataObject);
            return uomDataObject;
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryReceipt Record.
        /// </summary>
        /// <param name="uOMDTO">uOMDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(UOMDTO uOMDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(uOMDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", uOMDTO.UOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOM", string.IsNullOrEmpty(uOMDTO.UOM) ? string.Empty : uOMDTO.UOM));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", string.IsNullOrEmpty(uOMDTO.Remarks) ? string.Empty : uOMDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isactive", uOMDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityID", uOMDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Gets the UOM data of passed UOM id
        /// </summary>
        /// <param name="UOMId">integer type parameter</param>
        /// <returns>Returns UOMDTO</returns>
        public UOMDTO GetUOM(int UOMId)
        {
            log.LogMethodEntry(UOMId);
            UOMDTO result = null;
            string selectUOMQuery = SELECT_QUERY + @" WHERE uom.UOMId= @UOMId";
            SqlParameter parameter = new SqlParameter("@UOMId", UOMId);
            DataTable uom = dataAccessHandler.executeSelectQuery(selectUOMQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (uom.Rows.Count > 0)
            {

                result = GetUOMDTO(uom.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the uom table columns
        /// </summary>
        /// <returns>uomTableColumns</returns>
        public DataTable GetUOMColumns()
        {
            log.LogMethodEntry();
            string selectUOMQuery = "Select columns from(Select '' as columns UNION ALL Select COLUMN_NAME as columns from INFORMATION_SCHEMA.COLUMNS  Where TABLE_NAME='UOM') a order by columns";
            DataTable uomTableColumns = dataAccessHandler.executeSelectQuery(selectUOMQuery, null, sqlTransaction);
            log.LogMethodExit(uomTableColumns);
            return uomTableColumns;
        }

        /// <summary>
        /// Returns the no of UOM matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetUOMCount(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchParameters)
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


        internal List<UOMDTO> GetUOMDTOListOfConversionFactor(List<int> uomIdList, bool activeRecords)
        {
            log.LogMethodEntry(uomIdList);
            List<UOMDTO> uomDTOList = new List<UOMDTO>();
            string query = @"SELECT *
                            FROM UOM, @UomIdList List
                            WHERE UOMId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@UomIdList", uomIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                uomDTOList = table.Rows.Cast<DataRow>().Select(x => GetUOMDTO(x)).ToList();
            }
            log.LogMethodExit(uomDTOList);
            return uomDTOList;
        }

        public DateTime? GetUOMModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate  
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from UOM WHERE (site_id = -1 or -1 = -1)) LastUpdateDate";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the UOMDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UOMDTO matching the search criteria</returns>
        public List<UOMDTO> GetUOMList(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters);
            List<UOMDTO> uOMDTOList = new List<UOMDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY UOM.UOMId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                uOMDTOList = new List<UOMDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UOMDTO uomDTO = GetUOMDTO(dataRow);
                    uOMDTOList.Add(uomDTO);
                }
            }
            log.LogMethodExit(uOMDTOList);
            return uOMDTOList;
        }


        /// <summary>
        /// Returns the sql query based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of CategoryDTO</returns>
        public string GetFilterQuery(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<UOMDTO.SearchByUOMParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(UOMDTO.SearchByUOMParameters.UOMID) ||
                                searchParameter.Key.Equals(UOMDTO.SearchByUOMParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UOMDTO.SearchByUOMParameters.SITEID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UOMDTO.SearchByUOMParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == UOMDTO.SearchByUOMParameters.UOMID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
        /// Inserts data into UOM table
        /// </summary>
        /// <param name="uom">uom</param>
        /// <param name="siteId">siteId</param>
        /// <param name="loginId">loginId</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <returns>uomDTO</returns>
        public UOMDTO InsertUOM(UOMDTO uomDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(uomDTO, loginId, siteId);
            string insertUOMQuery = @"INSERT INTO [UOM]
                                               ([UOM]
                                               ,[Remarks]
                                               ,[site_id]
                                               ,[Guid]
                                               ,[MasterEntityId]
                                               ,[isactive]
                                               ,[CreatedBy]
                                               ,[CreationDate]
                                               ,[LastUpdatedBy]
                                               ,[LastUpdateDate])
                                         VALUES
                                               (@UOM
                                               ,@Remarks
                                               ,@site_id
                                               ,NewID()
                                               ,@MasterEntityId
                                               ,@isactive
                                               ,@createdBy
                                               ,GETDATE()   
                                               ,@lastUpdatedBy
                                               ,GETDATE())  
                                       SELECT * FROM UOM WHERE UOMId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertUOMQuery, GetSQLParameters(uomDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUOMDTO(uomDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting uomDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(uomDTO);
            return uomDTO;
        }


        /// <summary>
        /// Updates the UOM record
        /// </summary>
        /// <param name="uom">uom</param>
        /// <param name="siteId">siteId</param>
        /// <param name="loginId">loginId</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <returns>uomDTO</returns>
        public UOMDTO UpdateUOM(UOMDTO uomDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(uomDTO, siteId, loginId);
            string updateUOMQuery = @"UPDATE [dbo].[UOM]
                                           SET [UOM] = @UOM
                                              ,[Remarks] = @Remarks
                                              --,[site_id] = @site_id
                                              ,[MasterEntityId] = @MasterEntityId
                                              ,[isactive] = @isactive
                                              ,[LastUpdatedBy] = @lastUpdatedBy 
                                              ,LastUpdateDate =getdate()
                                         WHERE UOMID = @UOMId
                            SELECT* FROM UOM WHERE UOMId = @UOMId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateUOMQuery, GetSQLParameters(uomDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUOMDTO(uomDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating uomDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(uomDTO);
            return uomDTO;
        }

        /// <summary>
        /// Delete the record from the UOM database based on UOMId
        /// </summary>
        /// <param name="uOMId">uOMId</param>
        /// <returns>return the int </returns>
        internal int Delete(int uOMId)
        {
            log.LogMethodEntry(uOMId);
            string query = @"DELETE  
                             FROM UOM
                             WHERE UOM.UOMId = @UOMId";
            SqlParameter parameter = new SqlParameter("@UOMId", uOMId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="uOMDTO">UOMDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshUOMDTO(UOMDTO uOMDTO, DataTable dt)
        {
            log.LogMethodEntry(uOMDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                uOMDTO.UOMId = Convert.ToInt32(dt.Rows[0]["UOMId"]);
                uOMDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                uOMDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                uOMDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                uOMDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                uOMDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                uOMDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
    }
}