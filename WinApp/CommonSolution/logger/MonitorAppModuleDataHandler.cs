/********************************************************************************************
 * Project Name - Logger
 * Description  - Data Handler 
 **************
 ** Version Log
  **************
  * Version     Date Modified   By              Remarks
 *********************************************************************************************
 *2.70          11-jun-2019    Girish Kundar     Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// MonitorAppModuleDataHandler Data Handler - Handles insert, update and select of  MonitorAppModule objects
    /// </summary>
    public class MonitorAppModuleDataHandler
    {
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MonitorAppModule ";
        /// <summary>
        /// Dictionary for searching Parameters for the MonitorApplicationDTO object.
        /// </summary>
        private static readonly Dictionary<MonitorAppModuleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MonitorAppModuleDTO.SearchByParameters, string>
        {
            {MonitorAppModuleDTO.SearchByParameters.MODULE_ID,"ModuleId"},
            {MonitorAppModuleDTO.SearchByParameters.MODULE_NAME,"ModuleName"},
            {MonitorAppModuleDTO.SearchByParameters.SITE_ID,"site_id"},
            {MonitorAppModuleDTO.SearchByParameters.DESCRIPTION,"Description"},
            {MonitorAppModuleDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for MonitorAppModuleDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public MonitorAppModuleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating monitorAppModule Record.
        /// </summary>
        /// <param name="monitorAppModuleDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(MonitorAppModuleDTO monitorAppModuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorAppModuleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ModuleId", monitorAppModuleDTO.ModuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", monitorAppModuleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ModuleName", monitorAppModuleDTO.ModuleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", monitorAppModuleDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to MonitorAppModuleDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>MonitorAppModuleDTO</returns>
        private MonitorAppModuleDTO GetMonitorAppModuleDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MonitorAppModuleDTO monitorAppModuleDTO = new MonitorAppModuleDTO(dataRow["ModuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ModuleId"]),
                                                         dataRow["ModuleName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ModuleName"]),
                                                         dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(monitorAppModuleDTO);
            return monitorAppModuleDTO;
        }

        /// <summary>
        /// Gets the AppUIPanels data of passed id 
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns MonitorAppModuleDTO</returns>
        public MonitorAppModuleDTO GetMonitorAppModule(int id)
        {
            log.LogMethodEntry(id);
            MonitorAppModuleDTO result = null;
            string query = SELECT_QUERY + @" WHERE MonitorAppModule.ModuleId = @ModuleId";
            SqlParameter parameter = new SqlParameter("@ModuleId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMonitorAppModuleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the MonitorAppModule Table.
        /// </summary>
        /// <param name="monitorAppModuleDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>MonitorAppModuleDTO</returns>
        public MonitorAppModuleDTO Insert(MonitorAppModuleDTO monitorAppModuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorAppModuleDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[MonitorAppModule]
                           ([ModuleName]
                           ,[Description]
                           ,[site_id]
                           ,[Guid]
                           ,[MasterEntityId]
                           ,[CreatedBy]
                           ,[CreationDate]
                           ,[LastUpdatedBy]
                           ,[LastUpdateDate])
                     VALUES
                           (@ModuleName
                           ,@Description
                           ,@site_id
                           ,NEWID()
                           ,@MasterEntityId
                           ,@CreatedBy
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,GETDATE())
                            SELECT * FROM MonitorAppModule WHERE ModuleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(monitorAppModuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorAppModuleDTO(monitorAppModuleDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting MonitorAppModuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorAppModuleDTO);
            return monitorAppModuleDTO;
        }

        /// <summary>
        ///  Updates the record to the MonitorAppModule Table.
        /// </summary>
        /// <param name="monitorAppModuleDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>MonitorAppModuleDTO</returns>
        public MonitorAppModuleDTO Update(MonitorAppModuleDTO monitorAppModuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorAppModuleDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[MonitorAppModule]
                           SET
                            [ModuleName]     = @ModuleName
                           ,[Description]    = @Description
                           ,[site_id]        = @site_id
                           ,[MasterEntityId] = @MasterEntityId
                           ,[LastUpdatedBy]  = @LastUpdatedBy
                           ,[LastUpdateDate] =  GETDATE()
                            WHERE ModuleId = @ModuleId
                            SELECT * FROM MonitorAppModule WHERE ModuleId = @ModuleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(monitorAppModuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorAppModuleDTO(monitorAppModuleDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Updating MonitorAppModuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorAppModuleDTO);
            return monitorAppModuleDTO;
        }
        private void RefreshMonitorAppModuleDTO(MonitorAppModuleDTO monitorAppModuleDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorAppModuleDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                monitorAppModuleDTO.ModuleId = Convert.ToInt32(dt.Rows[0]["ModuleId"]);
                monitorAppModuleDTO.LastUpdateDate = Convert.ToDateTime(dt.Rows[0]["LastupdateDate"]);
                monitorAppModuleDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                monitorAppModuleDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                monitorAppModuleDTO.LastUpdatedBy = loginId;
                monitorAppModuleDTO.CreatedBy = loginId;
                monitorAppModuleDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of MonitorAppModuleDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>monitorAppModuleDTOList</returns>
        public List<MonitorAppModuleDTO> GetAllMonitorAppModuleDTO(List<KeyValuePair<MonitorAppModuleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MonitorAppModuleDTO> monitorAppModuleDTOList = new List<MonitorAppModuleDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            // should get child records also
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MonitorAppModuleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MonitorAppModuleDTO.SearchByParameters.MODULE_ID ||
                            searchParameter.Key == MonitorAppModuleDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MonitorAppModuleDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MonitorAppModuleDTO.SearchByParameters.MODULE_NAME
                            || searchParameter.Key == MonitorAppModuleDTO.SearchByParameters.DESCRIPTION)
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
                    MonitorAppModuleDTO monitorAppModuleDTO = GetMonitorAppModuleDTO(dataRow);
                    monitorAppModuleDTOList.Add(monitorAppModuleDTO);
                }
            }
            log.LogMethodExit(monitorAppModuleDTOList);
            return monitorAppModuleDTOList;
        }
    }
}
