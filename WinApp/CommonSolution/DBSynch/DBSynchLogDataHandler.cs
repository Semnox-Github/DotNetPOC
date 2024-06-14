/********************************************************************************************
 * Project Name - DBSynch
 * Description  - Data handler of the DBSynchLog class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana   Created 
 *2.70        01-Jul-2019   Girish Kundar     Modified : For SQL Injection Issue. 
 *2.70.2        21-Oct-2019   Rakesh            Implemented the CreateMasterDataFromHQSite() methods 
 *2.100.0     31-Aug-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DBSynch
{
    /// <summary>
    ///  DBSynchLog Data Handler - Handles insert, update and select of  DBSynchLog objects
    /// </summary>
    public class DBSynchLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM DBSynchLog AS dbl ";
        private static readonly Dictionary<DBSynchLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DBSynchLogDTO.SearchByParameters, string>
            {
                {DBSynchLogDTO.SearchByParameters.TIME_STAMP_GREATER_THAN, "dbl.TimeStamp"},
                {DBSynchLogDTO.SearchByParameters.TIME_STAMP_LESSER_THAN, "dbl.TimeStamp"},
                {DBSynchLogDTO.SearchByParameters.TABLE_NAME, "dbl.TableName"},
                {DBSynchLogDTO.SearchByParameters.OPERATION, "dbl.Operation"},
                {DBSynchLogDTO.SearchByParameters.GUID, "dbl.Guid"},
                {DBSynchLogDTO.SearchByParameters.SITE_ID, "dbl.site_id"}
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of DBSynchLogDataHandler class
        /// </summary>
        public DBSynchLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DBSynchLog Record.
        /// </summary>
        /// <param name="dBSynchLogDTO">DBSynchLogDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(DBSynchLogDTO dBSynchLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dBSynchLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Operation", dBSynchLogDTO.Operation));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", dBSynchLogDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableName", dBSynchLogDTO.TableName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeStamp", dBSynchLogDTO.TimeStamp == DateTime.MinValue ? DBNull.Value : (object)dBSynchLogDTO.TimeStamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", dBSynchLogDTO.SiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the DBSynchLog record to the database
        /// </summary>
        /// <param name="dBSynchLogDTO">DBSynchLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void InsertDBSynchLog(DBSynchLogDTO dBSynchLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dBSynchLogDTO, loginId, siteId);
            string query = @"INSERT INTO DBSynchLog 
                                        ( 
                                            Operation,
                                            Guid,
                                            TableName,
                                            TimeStamp,
                                            site_id,
                                            CreatedBy,
                                            CreationDate, 
                                            LastUpdatedBy,
                                            LastUpdateDate
                                        ) 
                                VALUES 
                                        (
                                            @Operation,
                                            @Guid,
                                            @TableName,
                                            @TimeStamp,
                                            @site_id,
                                            @CreatedBy,
                                            GETDATE(), 
                                            @LastUpdatedBy,
                                            GETDATE()
                                        )SELECT 1";
            try
            {
                dataAccessHandler.executeInsertQuery(query, GetSQLParameters(dBSynchLogDTO, loginId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to DBSynchLogDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DBSynchLogDTO</returns>
        private DBSynchLogDTO GetDBSynchLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO(Convert.ToString(dataRow["Operation"]),
                                                            dataRow["Guid"].ToString(),
                                                            Convert.ToString(dataRow["TableName"]),
                                                            Convert.ToDateTime(dataRow["TimeStamp"]),
                                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                            );
            log.LogMethodExit(dBSynchLogDTO);
            return dBSynchLogDTO;
        }

        /// <summary>
        /// Gets the DBSynchLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DBSynchLogDTO matching the search criteria</returns>
        public List<DBSynchLogDTO> GetDBSynchLogDTOList(List<KeyValuePair<DBSynchLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DBSynchLogDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
              
                StringBuilder query = new StringBuilder("  WHERE ");
                foreach (KeyValuePair<DBSynchLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == DBSynchLogDTO.SearchByParameters.TIME_STAMP_GREATER_THAN)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == DBSynchLogDTO.SearchByParameters.TIME_STAMP_LESSER_THAN)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));   //"yyyy-MM-dd HH:mm:ss.fff" 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == DBSynchLogDTO.SearchByParameters.GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == DBSynchLogDTO.SearchByParameters.SITE_ID)
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
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<DBSynchLogDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DBSynchLogDTO dBSynchLogDTO = GetDBSynchLogDTO(dataRow);
                    list.Add(dBSynchLogDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        internal void CreateMasterDataDBSynchLog(int siteId)
        {
            log.LogMethodEntry(siteId);
            string errorMessage = string.Empty;
            bool errorStatus = false;
            dataAccessHandler.CommandTimeOut = 0;
            string query = @" DECLARE @errormessage varchar(max);
                                DECLARE @errorstatus bit;
                                EXEC SyncDataForSite @siteId, @errormessage OUTPUT, @errorstatus OUTPUT
                                SELECT @errormessage ErrorMessage, @errorstatus ErrorStatus ";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@siteId", siteId, true) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                if(dataTable.Rows[0]["ErrorMessage"] != DBNull.Value)
                {
                    errorMessage = dataTable.Rows[0]["ErrorMessage"].ToString();
                }
                if (dataTable.Rows[0]["ErrorStatus"] != DBNull.Value)
                {
                    errorStatus = Convert.ToBoolean(dataTable.Rows[0]["ErrorStatus"]);
                }
                if(errorStatus)
                {
                    throw new ValidationException(errorMessage);
                }
            }
        }

        public void CreateMasterDataFromHQSite(int siteId)
        {
            log.LogMethodEntry(siteId);
            DBSynchTableDataHandler dBSynchDataHandler = new DBSynchTableDataHandler(sqlTransaction);
            DataTable dtTables = dBSynchDataHandler.GetAllTables(siteId);
            List<DBSynchTrxTable> trxTables = new DBSynchTrxTable().GetDBSynchTrxTables();
            foreach (DataRow drTable in dtTables.Rows)
            {
                string tableName = drTable["Name"].ToString();
                if (trxTables.Find(x => x.TableName.ToLower().Equals(tableName.ToLower())) != null)
                {
                    continue;
                }
                string query = @"insert into DBSynchLog (Operation, Guid, TableName, TimeStamp, site_id)
                                select 'U', Guid, @tableName, getdate(), site_id
                                from " + tableName +
                               " where site_id = @siteId";
                SqlParameter[] dbSynchLogTableParameters = new SqlParameter[2];
                dbSynchLogTableParameters[0] = new SqlParameter("@siteid", siteId);
                dbSynchLogTableParameters[1] = new SqlParameter("@tableName", tableName);
                try
                {
                    dataAccessHandler.executeUpdateQuery(query, dbSynchLogTableParameters, sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "throwing exception");
                    throw ex;
                }
            }
            object LastUploadTime = dataAccessHandler.executeScalar("select last_upload_time from site where site_id = @siteId", new[] { new SqlParameter("@siteId", siteId) }, sqlTransaction);
            if (LastUploadTime == DBNull.Value)
            {
                LastUploadTime = DateTime.Now.AddYears(-100);
            }
            foreach (DBSynchTrxTable table in trxTables)
            {
                if (!string.IsNullOrEmpty(table.TrxDateColumn))
                {
                    string query = @"insert into DBSynchLog (Operation, Guid, TableName, TimeStamp, site_id)
                                select 'U', Guid, @tableName, " + table.TrxDateColumn + ", site_id" +
                              " from " + table.TableName +
                              " where site_id = @siteId" +
                              " and " + table.TrxDateColumn + " > @lastUploadTime";
                    try
                    {
                        dataAccessHandler.executeUpdateQuery(query, new[] { new SqlParameter("@lastUploadTime", LastUploadTime),new SqlParameter("@siteId", siteId),
                                                            new SqlParameter("@tableName", table.TableName) }, sqlTransaction);
                    }
                    catch (Exception ex)
                    {
                        log.Error("", ex);
                        log.LogMethodExit(null, "throwing exception");
                        throw ex;
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}