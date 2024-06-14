/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler - PostTransactionProcessDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      30-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// PostTransactionProcessDataHandler Data Handler - Handles insert, update and select of  PostTransactionProcesses objects
    /// </summary>
    public class PostTransactionProcessDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PostTransactionProcesses As ptp  ";
       
        /// <summary>
        /// Dictionary for searching Parameters for the PostTransactionProcess object.
        /// </summary>
        private static readonly Dictionary<PostTransactionProcessDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PostTransactionProcessDTO.SearchByParameters, string>
        {
            { PostTransactionProcessDTO.SearchByParameters.ID,"ptp.Id"},
            { PostTransactionProcessDTO.SearchByParameters.ID_LIST,"ptp.Id"},
            { PostTransactionProcessDTO.SearchByParameters.IS_ISOLATED,"ptp.IsIsolated"},
            { PostTransactionProcessDTO.SearchByParameters.TYPE,"ptp.Type"},
            { PostTransactionProcessDTO.SearchByParameters.ACTIVE_FLAG,"ptp.Active"},
            { PostTransactionProcessDTO.SearchByParameters.SITE_ID,"ptp.site_id"},
            { PostTransactionProcessDTO.SearchByParameters.MASTER_ENTITY_ID,"ptp.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for PostTransactionProcessDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction  object</param>
        public PostTransactionProcessDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PostTransactionProcesses Record.
        /// </summary>
        /// <param name="postTransactionProcessDTO">PostTransactionProcessDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the List of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(PostTransactionProcessDTO postTransactionProcessDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(postTransactionProcessDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", postTransactionProcessDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Active", postTransactionProcessDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExecuteOrder", postTransactionProcessDTO.ExecuteOrder ));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsIsolated", postTransactionProcessDTO.IsIsolated ));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Process", postTransactionProcessDTO.Process));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Type", postTransactionProcessDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", postTransactionProcessDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to PostTransactionProcessDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the PostTransactionProcessDTO</returns>
        private PostTransactionProcessDTO GetPostTransactionProcessDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PostTransactionProcessDTO postTransactionProcessDTO = new PostTransactionProcessDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                       dataRow["Process"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Process"]),
                                       dataRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Type"]),
                                       dataRow["Active"] == DBNull.Value ? false: Convert.ToBoolean(dataRow["Active"].ToString()),
                                       dataRow["ExecuteOrder"] == DBNull.Value ? (int?) null : Convert.ToInt32(dataRow["ExecuteOrder"].ToString()),
                                       dataRow["IsIsolated"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["IsIsolated"]),
                                       dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                       dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                       dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                       dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                       dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                       dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                       dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                       dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                      );
            log.LogMethodExit(postTransactionProcessDTO);
            return postTransactionProcessDTO;
        }
        /// <summary>
        /// Gets the PostTransactionProcessDTO data of passed id 
        /// </summary>
        /// <param name="id">id of PostTransactionProcessDTO is passed as parameter</param>
        /// <returns>Returns PostTransactionProcessDTO</returns>
        public PostTransactionProcessDTO GetPostTransactionProcessDTO(int id)
        {
            log.LogMethodEntry(id);
            PostTransactionProcessDTO result = null;
            string query = SELECT_QUERY + @" WHERE ptp.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPostTransactionProcessDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the PostTransactionProcess record
        /// </summary>
        /// <param name="postTransactionProcessDTO">PostTransactionProcessDTO is passed as parameter</param>
        internal void Delete(PostTransactionProcessDTO postTransactionProcessDTO)
        {
            log.LogMethodEntry(postTransactionProcessDTO);
            string query = @"DELETE  
                             FROM PostTransactionProcesses
                             WHERE PostTransactionProcesses.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", postTransactionProcessDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            postTransactionProcessDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the postTransactionProcess Table.
        /// </summary>
        /// <param name="postTransactionProcessDTO">PostTransactionProcessDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the  PostTransactionProcessDTO</returns>
        public PostTransactionProcessDTO Insert(PostTransactionProcessDTO postTransactionProcessDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(postTransactionProcessDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[PostTransactionProcesses]
                           (Process,
                            Type,
                            Active,
                            ExecuteOrder,
                            Guid,
                            site_id,
                            IsIsolated,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@Process,
                            @Type,
                            @Active,
                            @ExecuteOrder,
                            NEWID(),
                            @site_id,
                            @IsIsolated,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE() )
                                    SELECT * FROM PostTransactionProcesses WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(postTransactionProcessDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPostTransactionProcessDTO(postTransactionProcessDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting PostTransactionProcessDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(postTransactionProcessDTO);
            return postTransactionProcessDTO;
        }

        /// <summary>
        ///  Updates the record to the postTransactionProcess Table.
        /// </summary>
        /// <param name="postTransactionProcessDTO">PostTransactionProcessDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the  PostTransactionProcessDTO</returns>
        public PostTransactionProcessDTO Update(PostTransactionProcessDTO postTransactionProcessDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(postTransactionProcessDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[PostTransactionProcesses]
                           SET 
                            Process       =  @Process,
                            Type          =  @Type,
                            Active        =  @Active,
                            ExecuteOrder  =  @ExecuteOrder,
                            IsIsolated    =  @IsIsolated,
                            MasterEntityId=  @MasterEntityId,
                            LastUpdatedBy =  @LastUpdatedBy,
                            LastUpdateDate=  GETDATE() 
                            WHERE Id =@Id
                                    SELECT * FROM PostTransactionProcesses WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(postTransactionProcessDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPostTransactionProcessDTO(postTransactionProcessDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating PostTransactionProcessDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(postTransactionProcessDTO);
            return postTransactionProcessDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="postTransactionProcessDTO">PostTransactionProcessDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
     
        private void RefreshPostTransactionProcessDTO(PostTransactionProcessDTO postTransactionProcessDTO, DataTable dt)
        {
            log.LogMethodEntry(postTransactionProcessDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                postTransactionProcessDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                postTransactionProcessDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                postTransactionProcessDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                postTransactionProcessDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                postTransactionProcessDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                postTransactionProcessDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                postTransactionProcessDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of PostTransactionProcessDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Return the List of PostTransactionProcessDTO</returns>
        public List<PostTransactionProcessDTO> GetPostTransactionProcessDTOList(List<KeyValuePair<PostTransactionProcessDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<PostTransactionProcessDTO> postTransactionProcessDTOList = new List<PostTransactionProcessDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PostTransactionProcessDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PostTransactionProcessDTO.SearchByParameters.ID
                            || searchParameter.Key == PostTransactionProcessDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                       else if (searchParameter.Key == PostTransactionProcessDTO.SearchByParameters.TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),searchParameter.Value));
                        }
                        else if (searchParameter.Key == PostTransactionProcessDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PostTransactionProcessDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PostTransactionProcessDTO.SearchByParameters.ACTIVE_FLAG) // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == PostTransactionProcessDTO.SearchByParameters.IS_ISOLATED) // Char
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                    PostTransactionProcessDTO postTransactionProcessDTO = GetPostTransactionProcessDTO(dataRow);
                    postTransactionProcessDTOList.Add(postTransactionProcessDTO);
                }
            }
            log.LogMethodExit(postTransactionProcessDTOList);
            return postTransactionProcessDTOList;
        }
    }
}
