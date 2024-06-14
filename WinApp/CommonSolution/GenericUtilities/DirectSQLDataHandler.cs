/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Data Handler -DirectSQLDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3     31-May-2019     Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// DirectSQLDataHandler Data Handler - Handles insert, update and select of  DirectSQL objects
    /// </summary>
    public  class DirectSQLDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM DirectSQL AS ds";
        /// <summary>
        /// Dictionary for searching Parameters for the DirectSQL object.
        /// </summary>
        private static readonly Dictionary<DirectSQLDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DirectSQLDTO.SearchByParameters, string>
        {
            { DirectSQLDTO.SearchByParameters.DIRECT_SQL_ID,"ds.DirectSQLId"},
            { DirectSQLDTO.SearchByParameters.SITE_ID,"ds.site_id"},
            { DirectSQLDTO.SearchByParameters.TABLE_NAME,"ds.TableName"}
        };

        /// <summary>
        /// Parameterized Constructor for DirectSQLDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object </param>
        public DirectSQLDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DirectSQL Record.
        /// </summary>
        /// <param name="directSQLDTO">DirectSQLDTO object passed as parameter</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(DirectSQLDTO directSQLDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(directSQLDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DirectSQLId", directSQLDTO.DirectSQLId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Query", directSQLDTO.Query));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableName", directSQLDTO.TableName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to DirectSQLDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow</param>
        /// <returns>returns the DirectSQLDTO</returns>
        private DirectSQLDTO GetDirectSQLDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DirectSQLDTO directSQLDTO = new DirectSQLDTO(dataRow["DirectSQLId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DirectSQLId"]),
                                                         dataRow["Query"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Query"]),
                                                         dataRow["TableName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TableName"]),
                                                         dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                                         dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(directSQLDTO);
            return directSQLDTO;
        }

        /// <summary>
        /// Gets the DirectSQL data of passed DirectSQLId 
        /// </summary>
        /// <param name="directSQLId">directSQLId is passed</param>
        /// <returns>returns DirectSQLDTO</returns>
        public DirectSQLDTO GetDirectSQL(int directSQLId)
        {
            log.LogMethodEntry(directSQLId);
            DirectSQLDTO result = null;
            string query = SELECT_QUERY + @" WHERE ds.DirectSQLId = @DirectSQLId";
            SqlParameter parameter = new SqlParameter("@DirectSQLId", directSQLId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetDirectSQLDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Inserts the record to the DirectSQLDTO Table.
        /// </summary>
        /// <param name="directSQLDTO">DirectSQLDTO object passed as parameter</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        /// <returns>returns DirectSQLDTO</returns>
        public DirectSQLDTO Insert(DirectSQLDTO directSQLDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(directSQLDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[DirectSQL]
                           (Query,
                            last_updated_date,
                            last_updated_user,
                            site_id,
                            TableName,
                            CreatedBy,
                            CreationDate)
                     VALUES
                           (@Query,
                            GETDATE(),
                            @LastUpdatedBy,
                            @site_id,
                            @TableName,
                            @CreatedBy,
                            GETDATE() ) SELECT * FROM DirectSQL WHERE DirectSQLId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(directSQLDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDirectSQLDTO(directSQLDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting DirectSQLDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(directSQLDTO);
            return directSQLDTO;
        }

        /// <summary>
        /// Updates the record to the DirectSQLDTO Table.
        /// </summary>
        /// <param name="directSQLDTO">DirectSQLDTO object passed as parameter</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        /// <returns>returns DirectSQLDTO</returns>
        public DirectSQLDTO Update(DirectSQLDTO directSQLDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(directSQLDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[DirectSQL]
                           SET 
                           Query = @Query,
                           last_updated_date = GETDATE(),
                           last_updated_user = @LastUpdatedBy,
                           TableName = @TableName
                           WHERE DirectSQLId = @DirectSQLId
                           SELECT * FROM DirectSQL WHERE DirectSQLId = @DirectSQLId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(directSQLDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDirectSQLDTO(directSQLDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating DirectSQLDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(directSQLDTO);
            return directSQLDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="directSQLDTO">DirectSQLDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshDirectSQLDTO(DirectSQLDTO directSQLDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(directSQLDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                directSQLDTO.DirectSQLId = Convert.ToInt32(dt.Rows[0]["DirectSQLId"]);
                directSQLDTO.LastUpdatedDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                directSQLDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                directSQLDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                directSQLDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                directSQLDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of DirectSQLDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>returns List of DirectSQLDTO</returns>
        public List<DirectSQLDTO> GetDirectSQLDTOList(List<KeyValuePair<DirectSQLDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<DirectSQLDTO> directSQLDTOList = new List<DirectSQLDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<DirectSQLDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == DirectSQLDTO.SearchByParameters.DIRECT_SQL_ID)
                           
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DirectSQLDTO.SearchByParameters.TABLE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),searchParameter.Value));
                        }
                        else if (searchParameter.Key == DirectSQLDTO.SearchByParameters.SITE_ID)
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
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DirectSQLDTO directSQLDTO = GetDirectSQLDTO(dataRow);
                    directSQLDTOList.Add(directSQLDTO);
                }
            }
            log.LogMethodExit(directSQLDTOList);
            return directSQLDTOList;
        }


    }
}
