/********************************************************************************************
 * Project Name - logger
 * Description  - Data Handler -DBAuditLogDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3     07-Jun-2019   Girish Kundar           Created
 *2.140.0    25-Nov-2021   Fiona Lishal      Modified : Added Searcch Parameter DATE_OF_LOG
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// DBAuditLogDataHandler Data Handler - Handles insert, update and select of  DBAuditLog objects
    /// </summary>
    public class DBAuditLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM DBAuditLog AS dbal ";
        /// <summary>
        /// Dictionary for searching Parameters for the DBAuditLogDTO object.
        /// </summary>
        private static readonly Dictionary<DBAuditLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DBAuditLogDTO.SearchByParameters, string>
        {
            { DBAuditLogDTO.SearchByParameters.RECORD_ID,"dbal.RecordID"},
            { DBAuditLogDTO.SearchByParameters.FIELD_NAME,"dbal.FieldName"},
            { DBAuditLogDTO.SearchByParameters.SITE_ID,"dbal.site_id"},
            { DBAuditLogDTO.SearchByParameters.TABLE_NAME,"dbal.TableName"},
            { DBAuditLogDTO.SearchByParameters.TYPE,"dbal.Type"},
            { DBAuditLogDTO.SearchByParameters.MASTER_ENTITY_ID,"dbal.MasterEntityId"},
            { DBAuditLogDTO.SearchByParameters.DATE_OF_LOG,"dbal.DateOfLog"}
        };

        /// <summary>
        /// Parameterized Constructor for DBAuditLogDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public DBAuditLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DBAuditLog Record.
        /// </summary>
        /// <param name="dbAuditLogDTO">DBAuditLogDTO object passed as parameter</param>
        /// <param name="loginId">login id of the user</param>
        /// <param name="siteId">site id of the user </param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(DBAuditLogDTO dbAuditLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dbAuditLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DateOfLog", dbAuditLogDTO.DateOfLog));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NewValue", dbAuditLogDTO.NewValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OldValue", dbAuditLogDTO.OldValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecordId", dbAuditLogDTO.RecordId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FieldName", dbAuditLogDTO.FieldName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableName", dbAuditLogDTO.TableName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Type", dbAuditLogDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserName", dbAuditLogDTO.UserName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", dbAuditLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to DBAuditLogDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object </param>
        /// <returns>Returns the DBAuditLogDTO</returns>
        private DBAuditLogDTO GetDBAuditLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DBAuditLogDTO dbauditLogDTO = new DBAuditLogDTO(dataRow["DateOfLog"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["DateOfLog"]),
                                          dataRow["TableName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TableName"]),
                                          dataRow["RecordId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RecordId"]),
                                          dataRow["FieldName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FieldName"]),
                                          dataRow["Type"] == DBNull.Value ? 'I' : Convert.ToChar(dataRow["Type"]),
                                          dataRow["OldValue"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["OldValue"]),
                                          dataRow["NewValue"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["NewValue"]),
                                          dataRow["UserName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UserName"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                          );
            log.LogMethodExit(dbauditLogDTO);
            return dbauditLogDTO;
        }

        /// <summary>
        ///  Inserts the record to the DBAuditLog Table.
        /// </summary>3
        /// <param name="dbAuditLogDTO">DBAuditLogDTO object passed as parameter</param>
        /// <param name="loginId">login id of the user</param>
        /// <param name="siteId">site id of the user </param>
        /// <returns> returns the DBAuditLogDTO</returns>
        public int Insert(DBAuditLogDTO dbAuditLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dbAuditLogDTO, loginId, siteId);
            int rowInserted = 0;
            string query = @"INSERT INTO [dbo].[DBAuditLog]
                           ([DateOfLog]
                           ,[TableName]
                           ,[FieldName]
                           ,[RecordID]
                           ,[Type]
                           ,[OldValue]
                           ,[NewValue]
                           ,[UserName]
                           ,[Guid]
                           ,[site_id]
                           ,[MasterEntityId]
                           ,[CreatedBy]
                           ,[CreationDate]
                           ,[LastUpdatedBy]
                           ,[LastUpdateDate])
                     VALUES
                           (@DateOfLog
                           ,@TableName
                           ,@FieldName
                           ,@RecordID
                           ,@Type
                           ,@OldValue
                           ,@NewValue
                           ,@UserName
                           ,NEWID()
                           ,@site_id
                           ,@MasterEntityId
                           ,@CreatedBy
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,GETDATE())";
            try
            {
                rowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(dbAuditLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                //RefreshDBAuditLogDTO(dbAuditLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting DBAuditLogDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(rowInserted);
            return rowInserted;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="dbAuditLogDTO">DBAuditLogDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshDBAuditLogDTO(DBAuditLogDTO dbAuditLogDTO, DataTable dt)
        {
            log.LogMethodEntry(dbAuditLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                dbAuditLogDTO.DateOfLog = Convert.ToDateTime(dt.Rows[0]["DateOfLog"]);
                dbAuditLogDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                dbAuditLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                dbAuditLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                dbAuditLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                dbAuditLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                dbAuditLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of DBAuditLogDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>returns the list of DBAuditLogDTO</returns>
        public List<DBAuditLogDTO> GetDBAuditLogDTOList(List<KeyValuePair<DBAuditLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<DBAuditLogDTO> dbAuditLogDTOList = new List<DBAuditLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<DBAuditLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == DBAuditLogDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DBAuditLogDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DBAuditLogDTO.SearchByParameters.TABLE_NAME
                            || searchParameter.Key == DBAuditLogDTO.SearchByParameters.FIELD_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == DBAuditLogDTO.SearchByParameters.DATE_OF_LOG)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DBAuditLogDTO dbAuditLogDTO = GetDBAuditLogDTO(dataRow);
                    dbAuditLogDTOList.Add(dbAuditLogDTO);
                }
            }
            log.LogMethodExit(dbAuditLogDTOList);
            return dbAuditLogDTOList;
        }

    }
}
