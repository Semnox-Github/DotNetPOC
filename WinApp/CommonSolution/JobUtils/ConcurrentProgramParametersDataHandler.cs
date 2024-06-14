/********************************************************************************************
 * Project Name - ConcurrentProgramParameters Datahandler
 * Description  - Datahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.120.1       24-May-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 ********************************************************************************************/
using System;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.JobUtils
{
    public class ConcurrentProgramParametersDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ConcurrentProgramParameters AS cpp ";

        private static readonly Dictionary<ConcurrentProgramParametersDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ConcurrentProgramParametersDTO.SearchByParameters, string>
            {
                {ConcurrentProgramParametersDTO.SearchByParameters.CONCURRENT_PROGRAM_PARAMETER_ID, "cpp.ConcurrentProgramParameterId"},
                {ConcurrentProgramParametersDTO.SearchByParameters.PROGRAM_ID, "cpp.ProgramId"},
                {ConcurrentProgramParametersDTO.SearchByParameters.IS_ACTIVE, "cpp.IsActive"},
                {ConcurrentProgramParametersDTO.SearchByParameters.MASTER_ENTITY_ID, "cpp.MasterEntityId"},
                {ConcurrentProgramParametersDTO.SearchByParameters.SITE_ID, "cpp.site_id"},
            };

        /// <summary>
        /// Parameterized Constructor for ConcurrentProgramParametersDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public ConcurrentProgramParametersDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get SQL Parameters
        /// </summary>
        /// <param name="ConcurrentProgramParametersDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(ConcurrentProgramParametersDTO concurrentProgramParametersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramParametersDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConcurrentProgramParameterId", concurrentProgramParametersDTO.ConcurrentProgramParameterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProgramId", concurrentProgramParametersDTO.ProgramId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParameterName", concurrentProgramParametersDTO.ParameterName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SQLParameter", concurrentProgramParametersDTO.SQLParameter));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParameterDescription", concurrentProgramParametersDTO.ParameterDescription));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DataType", concurrentProgramParametersDTO.DataType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DataSource", concurrentProgramParametersDTO.DataSource));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DataSourceType", concurrentProgramParametersDTO.DataSourceType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Operator", concurrentProgramParametersDTO.Operator));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DisplayOrder", concurrentProgramParametersDTO.DisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Mandatory", concurrentProgramParametersDTO.Mandatory));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", concurrentProgramParametersDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", concurrentProgramParametersDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastupdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private ConcurrentProgramParametersDTO GetConcurrentProgramParametersDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ConcurrentProgramParametersDTO concurrentProgramParametersDTO = new ConcurrentProgramParametersDTO(
                dataRow["ConcurrentProgramParameterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ConcurrentProgramParameterId"]),
                dataRow["ProgramId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProgramId"]),
                dataRow["ParameterName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ParameterName"]),
                dataRow["SQLParameter"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SQLParameter"]),
                dataRow["ParameterDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ParameterDescription"]),
                dataRow["DataType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DataType"]),
                dataRow["DataSource"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DataSource"]),
                dataRow["DataSourceType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DataSourceType"]),
                dataRow["Operator"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Operator"]),
                dataRow["DisplayOrder"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DisplayOrder"]),
                dataRow["Mandatory"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["Mandatory"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                );
            log.LogMethodExit(concurrentProgramParametersDTO);
            return concurrentProgramParametersDTO;
        }

        internal ConcurrentProgramParametersDTO GetConcurrentProgramParametersDTO(int concurrentProgramParameterId)
        {
            log.LogMethodEntry(concurrentProgramParameterId);
            ConcurrentProgramParametersDTO result = null;
            string query = SELECT_QUERY + @" WHERE cpp.ConcurrentProgramParameterId = @ConcurrentProgramParameterId";
            SqlParameter parameter = new SqlParameter("@ConcurrentProgramParameterId", concurrentProgramParameterId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetConcurrentProgramParametersDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<KeyValuePair<string,string>> GetProgramParameterValuesOfDBQuery(string query)
        {
            log.LogMethodEntry(query);
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
            if (dataTable.Columns.Count > 0 && dataTable.Rows.Count > 0)
            {
                if(dataTable.Columns.Count > 2)
                {
                    string valueMember = dataTable.Columns[0].ToString();
                    string displayMember = dataTable.Columns[1].ToString();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string id  = row[valueMember].ToString();
                        string value  = row[displayMember].ToString();
                        result.Add(new KeyValuePair<string, string>(id,value));
                    }
                }
                else
                {
                    string displayMember = dataTable.Columns[0].ToString();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string id = row[displayMember].ToString();
                        string value = row[displayMember].ToString();
                        result.Add(new KeyValuePair<string, string>(id, value));
                    }
                }
            }
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Gets the List of ConcurrentProgramParametersDTO based on the ConcurrentProgramParameters Id List
        /// </summary>
        /// <param name="ConcurrentProgramParametersIdList">List of ConcurrentProgramParameters Ids </param>
        /// <param name="activeRecords">activeRecords </param>
        /// <returns>returns the ConcurrentProgramParametersDTO List</returns>
        internal List<ConcurrentProgramParametersDTO> GetConcurrentProgramParametersDTOList(List<int> programIdList, bool activeRecords)
        {
            log.LogMethodEntry(programIdList, activeRecords);
            List<ConcurrentProgramParametersDTO> concurrentProgramParametersDTOList = new List<ConcurrentProgramParametersDTO>();
            string query = @"SELECT *
                            FROM ConcurrentProgramParameters, @programIdList List
                            WHERE ProgramId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@programIdList", programIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                concurrentProgramParametersDTOList = table.Rows.Cast<DataRow>().Select(x => GetConcurrentProgramParametersDTO(x)).ToList();
            }
            log.LogMethodExit(concurrentProgramParametersDTOList);
            return concurrentProgramParametersDTOList;
        }

        internal void Delete(ConcurrentProgramParametersDTO concurrentProgramParametersDTO)
        {
            log.LogMethodEntry(concurrentProgramParametersDTO);
            string query = @"DELETE  
                             FROM ConcurrentProgramParameters
                             WHERE ConcurrentProgramParameterId = @ConcurrentProgramParameterId";
            SqlParameter parameter = new SqlParameter("@ConcurrentProgramParameterId", concurrentProgramParametersDTO.ConcurrentProgramParameterId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
        private void RefreshConcurrentProgramParametersDTO(ConcurrentProgramParametersDTO concurrentProgramParametersDTO, DataTable dt)
        {
            log.LogMethodEntry(concurrentProgramParametersDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                concurrentProgramParametersDTO.ConcurrentProgramParameterId = Convert.ToInt32(dt.Rows[0]["ConcurrentProgramParameterId"]);
                concurrentProgramParametersDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                concurrentProgramParametersDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                concurrentProgramParametersDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                concurrentProgramParametersDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                concurrentProgramParametersDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                concurrentProgramParametersDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal ConcurrentProgramParametersDTO Insert(ConcurrentProgramParametersDTO concurrentProgramParametersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramParametersDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ConcurrentProgramParameters]
                               ([ProgramId]
                               ,[ParameterName]
                               ,[SQLParameter]
                               ,[ParameterDescription]
                               ,[DataType]
                               ,[DataSource]
                               ,[DataSourceType]
                               ,[Operator]
                               ,[DisplayOrder]
                               ,[Mandatory]
                               ,[IsActive]
                               ,[CreatedBy]
                               ,[CreationDate]
                               ,[LastUpdatedBy]
                               ,[LastUpdatedDate]
                               ,[Guid]
                               ,[site_id]
                               ,[MasterEntityId]
                                )
                               
                         VALUES
                               (
                                @ProgramId,
                                @ParameterName,
                                @SQLParameter,
                                @ParameterDescription,
                                @DataType,
                                @DataSource,
                                @DataSourceType,
                                @Operator,
                                @DisplayOrder,
                                @Mandatory,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                NEWID(), 
                                @SiteId,
                                @MasterEntityId
                                 )
                                SELECT * FROM ConcurrentProgramParameters WHERE ConcurrentProgramParameterId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(concurrentProgramParametersDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentProgramParametersDTO(concurrentProgramParametersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentProgramParametersDTO);
            return concurrentProgramParametersDTO;
        }
        internal ConcurrentProgramParametersDTO Update(ConcurrentProgramParametersDTO concurrentProgramParametersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramParametersDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ConcurrentProgramParameters] set 
                                    [ProgramId]                 = @ProgramId,
                                    [ParameterName]             = @ParameterName,
                                    [SQLParameter]              = @SQLParameter,
                                    [ParameterDescription]      = @ParameterDescription,
                                    [DataType]                  = @DataType,
                                    [DataSource]                = @DataSource,
                                    [DataSourceType]            = @DataSourceType,
                                    [Operator]                  = @Operator,
                                    [DisplayOrder]              = @DisplayOrder,
                                    [Mandatory]                 = @Mandatory,
                                    [IsActive]                  = @IsActive,
                                    [MasterEntityId]            = @MasterEntityId,
                                    [LastUpdatedBy]             = @LastUpdatedBy,
                                    [LastUpdatedDate]           = GETDATE()
                                    where ConcurrentProgramParameterId = @ConcurrentProgramParameterId
                                    SELECT * FROM ConcurrentProgramParameters WHERE ConcurrentProgramParameterId = @ConcurrentProgramParameterId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(concurrentProgramParametersDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentProgramParametersDTO(concurrentProgramParametersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentProgramParametersDTO);
            return concurrentProgramParametersDTO;
        }

        internal List<ConcurrentProgramParametersDTO> GetConcurrentProgramParametersDTOList(List<KeyValuePair<ConcurrentProgramParametersDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<ConcurrentProgramParametersDTO> concurrentProgramParametersDTOList = new List<ConcurrentProgramParametersDTO>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ConcurrentProgramParametersDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ConcurrentProgramParametersDTO.SearchByParameters.CONCURRENT_PROGRAM_PARAMETER_ID ||
                            searchParameter.Key == ConcurrentProgramParametersDTO.SearchByParameters.PROGRAM_ID ||
                            searchParameter.Key == ConcurrentProgramParametersDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramParametersDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramParametersDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                concurrentProgramParametersDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetConcurrentProgramParametersDTO(x)).ToList();
            }
            log.LogMethodExit(concurrentProgramParametersDTOList);
            return concurrentProgramParametersDTOList;
        }
    }
}
