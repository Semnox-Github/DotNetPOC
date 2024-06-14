/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data Handler File for ApplicationRequestLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.120.10    06-Jul-2021   Abhishek                Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class ApplicationRequestLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ApplicationRequestLog AS arl ";


        private static readonly Dictionary<ApplicationRequestLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ApplicationRequestLogDTO.SearchByParameters, string>
        {
              {ApplicationRequestLogDTO.SearchByParameters.ID , "arl.Id"},
              {ApplicationRequestLogDTO.SearchByParameters.REQUEST_GUID , "arl.RequestGuid"},
              {ApplicationRequestLogDTO.SearchByParameters.MODULE , "arl.Module"},
               {ApplicationRequestLogDTO.SearchByParameters.USECASE , "arl.Usecase"},
               {ApplicationRequestLogDTO.SearchByParameters.SITE_ID , "arl.site_id"},
                {ApplicationRequestLogDTO.SearchByParameters.MASTER_ENTITY_ID , "arl.MasterEntityId"},
                 {ApplicationRequestLogDTO.SearchByParameters.IS_ACTIVE , "arl.IsActive"},
                  {ApplicationRequestLogDTO.SearchByParameters.ID_LIST , "arl.Id"}
        };

        /// <summary>
        /// Default constructor of ApplicationRequestLogDataHandler class
        /// </summary>
        public ApplicationRequestLogDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ApplicationRequestLogDTO applicationRequestLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationRequestLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", applicationRequestLogDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RequestGuid", applicationRequestLogDTO.RequestGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Module", applicationRequestLogDTO.Module));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Usecase", applicationRequestLogDTO.Usecase));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Timestamp", applicationRequestLogDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoginId", applicationRequestLogDTO.LoginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", applicationRequestLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", applicationRequestLogDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", applicationRequestLogDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the Application Request Log record to the database
        /// </summary>
        /// <param name="applicationRequestLogDTO">ApplicationRequestLogDTO type object</param>
        /// <param name="loginId">login id of user</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted ApplicationRequestLogDTO</returns>
        internal ApplicationRequestLogDTO Insert(ApplicationRequestLogDTO applicationRequestLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationRequestLogDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ApplicationRequestLog] 
                                                        (                                                 
                                                         RequestGuid,
                                                         Module,
                                                         Usecase,
                                                         Timestamp,
                                                         LoginId,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedBy,
                                                         LastUpdateDate,
                                                         site_id,
                                                         Guid,
                                                         MasterEntityId,
                                                         IsActive,
                                                         SynchStatus
                                                        ) 
                                                values 
                                                        (
                                                          @RequestGuid,
                                                          @Module,
                                                          @Usecase,
                                                          @Timestamp,
                                                          @LoginId,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @IsActive,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM ApplicationRequestLog WHERE Id = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(applicationRequestLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApplicationRequestLogDTO(applicationRequestLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(applicationRequestLogDTO);
            return applicationRequestLogDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="applicationRequestLogDTO">ApplicationRequestLogDTO object as parameter</param>
        /// <param name="dt">dt is an object of type DataTable </param>
        private void RefreshApplicationRequestLogDTO(ApplicationRequestLogDTO applicationRequestLogDTO, DataTable dt)
        {
            log.LogMethodEntry(applicationRequestLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                applicationRequestLogDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                applicationRequestLogDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                applicationRequestLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                applicationRequestLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                applicationRequestLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                applicationRequestLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                applicationRequestLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the ApplicationRequestLog  record
        /// </summary>
        /// <param name="applicationRequestLogDTO">ApplicationRequestLogDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns ApplicationRequestLogDTO</returns>
        internal ApplicationRequestLogDTO Update(ApplicationRequestLogDTO applicationRequestLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationRequestLogDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ApplicationRequestLog] set
                               [Module]                   = @Module,
                               [Usecase]                  = @Usecase,
                               [LoginId]                  = @LoginId,
                               [site_id]                  = @SiteId,
                               [IsActive]                 = @IsActive,
                               [MasterEntityId]           = @MasterEntityId,
                               [LastUpdatedBy]            = @LastUpdatedBy,
                               [LastUpdateDate]           = GETDATE()
                               where Id = @Id
                             SELECT * FROM ApplicationRequestLog WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(applicationRequestLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApplicationRequestLogDTO(applicationRequestLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(applicationRequestLogDTO);
            return applicationRequestLogDTO;
        }

        /// <summary>
        /// Converts the Data row object to ApplicationRequestLogDTO class type
        /// </summary>
        /// <param name="dataRow">ApplicationRequestLog DataRow</param>
        /// <returns>Returns ApplicationRequestLogDTO</returns>
        private ApplicationRequestLogDTO GetApplicationRequestLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ApplicationRequestLogDTO applicationRequestLogDTO = new ApplicationRequestLogDTO(Convert.ToInt32(dataRow["Id"]),
                                                    dataRow["RequestGuid"] == DBNull.Value ? string.Empty : (dataRow["RequestGuid"]).ToString(),
                                                    dataRow["Module"] == DBNull.Value ? string.Empty : (dataRow["Module"]).ToString(),
                                                    dataRow["Usecase"] == DBNull.Value ? string.Empty : (dataRow["Usecase"]).ToString(),
                                                    dataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Timestamp"]),
                                                    dataRow["LoginId"] == DBNull.Value ? string.Empty : (dataRow["LoginId"]).ToString(),
                                                     dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"].ToString(),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"].ToString(),
                                                    dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                  
                                                    );
            log.LogMethodExit();
            return applicationRequestLogDTO;
        }

        /// <summary>
        /// Gets the ApplicationRequestLog data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ApplicationRequestLogDTO</returns>
        internal ApplicationRequestLogDTO GetApplicationRequestLogDTO(int id)
        {
            log.LogMethodEntry(id);
            ApplicationRequestLogDTO applicationRequestLogDTO = null;
            string query = SELECT_QUERY + @" WHERE arl.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                applicationRequestLogDTO = GetApplicationRequestLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(applicationRequestLogDTO);
            return applicationRequestLogDTO;
        }

        /// <summary>
        /// Gets the ApplicationRequestLog data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ApplicationRequestLogDTO</returns>
        internal ApplicationRequestLogDTO GetApplicationRequestLogsDTOOfGuid(string requestGuid)
        {
            log.LogMethodEntry(requestGuid);
            ApplicationRequestLogDTO applicationRequestLogDTO = null;
            string query = SELECT_QUERY + @" WHERE arl.RequestGuid = @RequestGuid";
            SqlParameter parameter = new SqlParameter("@RequestGuid", requestGuid);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                applicationRequestLogDTO = GetApplicationRequestLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(applicationRequestLogDTO);
            return applicationRequestLogDTO;
        }


        /// <summary>
        /// Gets the ApplicationRequestLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ApplicationRequestLogDTO matching the search criteria</returns>    
        internal List<ApplicationRequestLogDTO> GetApplicationRequestLogList(List<KeyValuePair<ApplicationRequestLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ApplicationRequestLogDTO> applicationRequestLogDTOList = new List<ApplicationRequestLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ApplicationRequestLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ApplicationRequestLogDTO.SearchByParameters.ID ||
                            searchParameter.Key == ApplicationRequestLogDTO.SearchByParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApplicationRequestLogDTO.SearchByParameters.REQUEST_GUID ||
                            searchParameter.Key == ApplicationRequestLogDTO.SearchByParameters.MODULE ||
                            searchParameter.Key == ApplicationRequestLogDTO.SearchByParameters.USECASE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ApplicationRequestLogDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }

                        else if (searchParameter.Key == ApplicationRequestLogDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApplicationRequestLogDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                applicationRequestLogDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetApplicationRequestLogDTO(x)).ToList();
            }
            log.LogMethodExit(applicationRequestLogDTOList);
            return applicationRequestLogDTOList;
        }
    }
}
