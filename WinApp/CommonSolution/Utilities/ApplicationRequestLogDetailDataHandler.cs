/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data Handler File for ApplicationRequestLogDetail
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
    public class ApplicationRequestLogDetailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ApplicationRequestLogDetail AS arld";

        /// <summary>
        /// Dictionary for searching Parameters for the ProductUserGroupsMappingDTO object.
        /// </summary>
        private static readonly Dictionary<ApplicationRequestLogDetailDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ApplicationRequestLogDetailDTO.SearchByParameters, string>
        {
            { ApplicationRequestLogDetailDTO.SearchByParameters.ID,"arld.Id"},
            { ApplicationRequestLogDetailDTO.SearchByParameters.APPLICATION_REQUEST_LOG_ID,"arld.ApplicationRequestLogId"},
            { ApplicationRequestLogDetailDTO.SearchByParameters.ENTITY_GUID,"arld.EntityGuid"},
            { ApplicationRequestLogDetailDTO.SearchByParameters.ID_LIST,"arld.Id"},
            { ApplicationRequestLogDetailDTO.SearchByParameters.SITE_ID,"arld.site_id"},
            { ApplicationRequestLogDetailDTO.SearchByParameters.MASTER_ENTITY_ID,"arld.MasterEntityId"},
            { ApplicationRequestLogDetailDTO.SearchByParameters.IS_ACTIVE,"arld.IsActive"}
        };

        /// <summary>
        /// Parameterized Constructor for ApplicationRequestLogDetailDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object </param>
        public ApplicationRequestLogDetailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ApplicationRequestLogDetails Record.
        /// </summary>
        /// <param name="applicationRequestLogDetailDTO">ApplicationRequestLogDetailDTO object as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ApplicationRequestLogDetailDTO applicationRequestLogDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationRequestLogDetailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", applicationRequestLogDetailDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicationRequestLogId", applicationRequestLogDetailDTO.ApplicationRequestLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EntityGuid", applicationRequestLogDetailDTO.EntityGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", applicationRequestLogDetailDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", applicationRequestLogDetailDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to ApplicationRequestLogDetailDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object of DataRow</param>
        /// <returns>returns the object of ApplicationRequestLogDetailDTO</returns>
        private ApplicationRequestLogDetailDTO GetApplicationRequestLogDetailDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ApplicationRequestLogDetailDTO applicationRequestLogDetailDTO = new ApplicationRequestLogDetailDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                         dataRow["ApplicationRequestLogId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApplicationRequestLogId"]),
                                         dataRow["EntityGuid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EntityGuid"]),
                                         dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));

            log.LogMethodExit(applicationRequestLogDetailDTO);
            return applicationRequestLogDetailDTO;
        }

        /// <summary>
        ///  Inserts the record to the ApplicationRequestLogDetail Table.
        /// </summary>
        /// <param name="applicationRequestLogDetailDTO">ApplicationRequestLogDetailDTO object as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns> ApplicationRequestLogDetailDTO</returns>
        internal ApplicationRequestLogDetailDTO Insert(ApplicationRequestLogDetailDTO applicationRequestLogDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationRequestLogDetailDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ApplicationRequestLogDetail]
                               (
                                [ApplicationRequestLogId],
                                [EntityGuid],
                                [IsActive],                               
                                [CreatedBy],
                                [CreationDate],
                                [LastUpdatedBy],
                                [LastUpdateDate],
                                [site_id],
                                [Guid],
                                [MasterEntityId]
                               )
                               VALUES
                               (@ApplicationRequestLogId,
                                @EntityGuid,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                @site_id,
                                NEWID(),
                                @MasterEntityId                                
                                )
                                SELECT * FROM ApplicationRequestLogDetail WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(applicationRequestLogDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductUserGroupsMappingDTO(applicationRequestLogDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ApplicationRequestLogDetailDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(applicationRequestLogDetailDTO);
            return applicationRequestLogDetailDTO;
        }

        /// <summary>
        ///  Updates the record to the ApplicationRequestLogDetail Table.
        /// </summary>
        /// <param name="applicationRequestLogDetailDTO">ApplicationRequestLogDetailDTO object as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns> ApplicationRequestLogDetailDTO</returns>
        internal ApplicationRequestLogDetailDTO Update(ApplicationRequestLogDetailDTO applicationRequestLogDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationRequestLogDetailDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ApplicationRequestLogDetail]
                               SET
                                [ApplicationRequestLogId] = @ApplicationRequestLogId,
                                [EntityGuid] = @EntityGuid,
                               -- [site_id] = @site_id,
                                [MasterEntityId] = @MasterEntityId,
                                [LastUpdatedBy] = @LastUpdatedBy,
                                [LastUpdateDate] = GETDATE() ,
                                [IsActive] = @IsActive
                               WHERE Id = @Id
                               SELECT * FROM ApplicationRequestLogDetail WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(applicationRequestLogDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductUserGroupsMappingDTO(applicationRequestLogDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ApplicationRequestLogDetailDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(applicationRequestLogDetailDTO);
            return applicationRequestLogDetailDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="applicationRequestLogDetailDTO">ApplicationRequestLogDetailDTO object as parameter</param>
        /// <param name="dt">dt is an object of type DataTable </param>
        private void RefreshProductUserGroupsMappingDTO(ApplicationRequestLogDetailDTO applicationRequestLogDetailDTO, DataTable dt)
        {
            log.LogMethodEntry(applicationRequestLogDetailDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                applicationRequestLogDetailDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                applicationRequestLogDetailDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                applicationRequestLogDetailDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                applicationRequestLogDetailDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                applicationRequestLogDetailDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                applicationRequestLogDetailDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                applicationRequestLogDetailDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ApplicationRequestLogDetailDTO data of passed Id 
        /// </summary>
        /// <param name="id">id of ApplicationRequestLogDetail is passed as parameter</param>
        /// <returns>Returns the object of ApplicationRequestLogDetail</returns>
        internal ApplicationRequestLogDetailDTO GetApplicationRequestLogDetailDTO(int id)
        {
            log.LogMethodEntry(id);
            ApplicationRequestLogDetailDTO result = null;
            string query = SELECT_QUERY + @" WHERE arld.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetApplicationRequestLogDetailDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of ApplicationRequestLogDetailDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of ApplicationRequestLogDetailDTO</returns>
        internal List<ApplicationRequestLogDetailDTO> GetApplicationRequestLogDetailDTOList(List<KeyValuePair<ApplicationRequestLogDetailDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ApplicationRequestLogDetailDTO> applicationRequestLogDetailDTOList = new List<ApplicationRequestLogDetailDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ApplicationRequestLogDetailDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ApplicationRequestLogDetailDTO.SearchByParameters.ID
                            || searchParameter.Key == ApplicationRequestLogDetailDTO.SearchByParameters.APPLICATION_REQUEST_LOG_ID
                            || searchParameter.Key == ApplicationRequestLogDetailDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApplicationRequestLogDetailDTO.SearchByParameters.ENTITY_GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ApplicationRequestLogDetailDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ApplicationRequestLogDetailDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApplicationRequestLogDetailDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
                applicationRequestLogDetailDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetApplicationRequestLogDetailDTO(x)).ToList();
            }
            log.LogMethodExit(applicationRequestLogDetailDTOList);
            return applicationRequestLogDetailDTOList;
        }

        /// <summary>
        /// Used to get the list of values of product user groups mapping DTO 
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="idList">ApplicationRequestLogDetail id list as parameter</param>
        /// <param name="activeRecords">activeRecords as a parameter to obtain active records </param>
        internal List<ApplicationRequestLogDetailDTO> GetApplicationRequestLogDetailDTOListOfRequest(List<int> idList,
                                                                                               bool activeRecords)
        {
            log.LogMethodEntry(idList, activeRecords);
            List<ApplicationRequestLogDetailDTO> applicationRequestLogDetailDTOList = new List<ApplicationRequestLogDetailDTO>();
            string query = SELECT_QUERY + @" , @ApplicationRequestLogsIdList List
                            WHERE arld.Id = List.Id ";
            if (activeRecords)
            {
                query += " AND arld.isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ApplicationRequestLogsIdList", idList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                applicationRequestLogDetailDTOList = table.Rows.Cast<DataRow>().Select(x => GetApplicationRequestLogDetailDTO(x)).ToList();
            }
            log.LogMethodExit(applicationRequestLogDetailDTOList);
            return applicationRequestLogDetailDTOList;
        }
    }
}
