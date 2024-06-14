/********************************************************************************************
 * Project Name - logger
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        07-Jun-2019   Girish Kundar           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// MonitorApplicationDataHandler Data Handler - Handles insert, update and select of  MonitorApplication objects
    /// </summary>
    public class MonitorApplicationDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MonitorApplication ";
        /// <summary>
        /// Dictionary for searching Parameters for the MonitorApplicationDTO object.
        /// </summary>
        private static readonly Dictionary<MonitorApplicationDTO.SearchByMonitorApplicationParameters, string> DBSearchParameters = new Dictionary<MonitorApplicationDTO.SearchByMonitorApplicationParameters, string>
        {
            { MonitorApplicationDTO.SearchByMonitorApplicationParameters.APPLICATION_ID,"ApplicationId"},
            { MonitorApplicationDTO.SearchByMonitorApplicationParameters.APPLICATION_NAME,"ApplicationName"},
            { MonitorApplicationDTO.SearchByMonitorApplicationParameters.SITE_ID,"site_id"},
            { MonitorApplicationDTO.SearchByMonitorApplicationParameters.APP_EXE_NAME,"AppExeName"},
            { MonitorApplicationDTO.SearchByMonitorApplicationParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };
        /// <summary>
        /// Parameterized Constructor for MonitorApplicationDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public MonitorApplicationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating monitorApplication Record.
        /// </summary>
        /// <param name="monitorApplicationDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(MonitorApplicationDTO monitorApplicationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorApplicationDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicationId", monitorApplicationDTO.ApplicationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AppExeName", monitorApplicationDTO.AppExeName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicationName", monitorApplicationDTO.ApplicationName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", monitorApplicationDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to MonitorApplicationDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>MonitorApplicationDTO</returns>
        private MonitorApplicationDTO GetMonitorApplicationDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MonitorApplicationDTO monitorApplicationDTO = new MonitorApplicationDTO(dataRow["ApplicationId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApplicationId"]),
                                          dataRow["ApplicationName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ApplicationName"]),
                                          dataRow["AppExeName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["AppExeName"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                          );
            log.LogMethodExit(monitorApplicationDTO);
            return monitorApplicationDTO;
        }

        /// <summary>
        /// Gets the MonitorApplicationDTO data of passed Id 
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns MonitorApplicationDTO</returns>
        public MonitorApplicationDTO GetMonitorApplicationDTO(int id)
        {
            log.LogMethodEntry(id);
            MonitorApplicationDTO result = null;
            string query = SELECT_QUERY + @" WHERE MonitorApplication.ApplicationId = @ApplicationId";
            SqlParameter parameter = new SqlParameter("@ApplicationId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMonitorApplicationDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        ///  Inserts the record to the monitorApplication Table.
        /// </summary>
        /// <param name="monitorApplicationDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns> MonitorApplicationDTO</returns>
        public MonitorApplicationDTO Insert(MonitorApplicationDTO monitorApplicationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorApplicationDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[MonitorApplication]
                           ([ApplicationName]
                           ,[AppExeName]
                           ,[site_id]
                           ,[Guid]
                           ,[MasterEntityId]
                           ,[CreatedBy]
                           ,[CreationDate]
                           ,[LastUpdatedBy]
                           ,[LastUpdateDate])
                     VALUES
                           (@ApplicationName
                           ,@AppExeName
                           ,@site_id
                           ,NEWID()
                           ,@MasterEntityId
                           ,@CreatedBy
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,GETDATE())
                  SELECT * FROM MonitorApplication WHERE ApplicationId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(monitorApplicationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorApplicationDTO(monitorApplicationDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting MonitorApplicationDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorApplicationDTO);
            return monitorApplicationDTO;
        }

        /// <summary>
        ///  Update the record to the monitorApplication Table.
        /// </summary>
        /// <param name="monitorApplicationDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns> MonitorApplicationDTO</returns>
        public MonitorApplicationDTO Update(MonitorApplicationDTO monitorApplicationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorApplicationDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[MonitorApplication]
                           SET
                            [ApplicationName] = @ApplicationName
                           ,[AppExeName]      = @AppExeName
                           ,[site_id]         = @site_id
                           ,[MasterEntityId]  = @MasterEntityId
                           ,[LastUpdatedBy]   = @LastUpdatedBy
                           ,[LastUpdateDate]) = GETDATE()
                        WHERE ApplicationId = @ApplicationId
                        SELECT * FROM MonitorApplication WHERE ApplicationId = @ApplicationId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(monitorApplicationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorApplicationDTO(monitorApplicationDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting MonitorApplicationDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorApplicationDTO);
            return monitorApplicationDTO;
        }


        private void RefreshMonitorApplicationDTO(MonitorApplicationDTO monitorApplicationDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorApplicationDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                monitorApplicationDTO.ApplicationId = Convert.ToInt32(dt.Rows[0]["ApplicationId"]);
                monitorApplicationDTO.LastUpdateDate = Convert.ToDateTime(dt.Rows[0]["LastUpdatedBy"]);
                monitorApplicationDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                monitorApplicationDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                monitorApplicationDTO.LastUpdatedBy = loginId;
                monitorApplicationDTO.CreatedBy = loginId;
                monitorApplicationDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the List of MonitorApplicationDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>monitorApplicationDTOList</returns>
        public List<MonitorApplicationDTO> GetAllMonitorApplicationDTO(List<KeyValuePair<MonitorApplicationDTO.SearchByMonitorApplicationParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MonitorApplicationDTO> monitorApplicationDTOList = new List<MonitorApplicationDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MonitorApplicationDTO.SearchByMonitorApplicationParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MonitorApplicationDTO.SearchByMonitorApplicationParameters.APPLICATION_ID
                            || searchParameter.Key == MonitorApplicationDTO.SearchByMonitorApplicationParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MonitorApplicationDTO.SearchByMonitorApplicationParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MonitorApplicationDTO.SearchByMonitorApplicationParameters.APPLICATION_NAME
                            || searchParameter.Key == MonitorApplicationDTO.SearchByMonitorApplicationParameters.APP_EXE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                    MonitorApplicationDTO monitorApplicationDTO = GetMonitorApplicationDTO(dataRow);
                    monitorApplicationDTOList.Add(monitorApplicationDTO);
                }
            }
            log.LogMethodExit(monitorApplicationDTOList);
            return monitorApplicationDTOList;
        }
    }
}
