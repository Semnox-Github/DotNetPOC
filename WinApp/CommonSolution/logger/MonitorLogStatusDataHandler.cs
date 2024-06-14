/********************************************************************************************
 * Project Name - Logger
 * Description  - Data Handler 
 **************
 ** Version Log
  **************
  * Version     Date          Modified   By       Remarks
 *********************************************************************************************
 *2.70          11-jun-2019    Girish Kundar     Created
 *2.90          23-Jul-2020    Mushahid Faizan   Modified Column name LastupdatedDate to LastupdateDate.
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
    /// MonitorLogStatusDataHandler Data Handler - Handles insert, update and select of  MonitorLogStatus objects
    /// </summary>
    public class MonitorLogStatusDataHandler
    {
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MonitorLogStatus ";
        /// <summary>
        /// Dictionary for searching Parameters for the MonitorLogStatusDTO object.
        /// </summary>
        private static readonly Dictionary<MonitorLogStatusDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MonitorLogStatusDTO.SearchByParameters, string>
        {
            {MonitorLogStatusDTO.SearchByParameters.STATUS_ID,"StatusId"},
            {MonitorLogStatusDTO.SearchByParameters.STATUS,"Status"},
            {MonitorLogStatusDTO.SearchByParameters.SITE_ID,"site_id"},
            {MonitorLogStatusDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for MonitorLogStatusDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public MonitorLogStatusDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MonitorLogStatus Record.
        /// </summary>
        /// <param name="monitorLogStatusDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(MonitorLogStatusDTO monitorLogStatusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorLogStatusDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@StatusId", monitorLogStatusDTO.StatusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", monitorLogStatusDTO.Status, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", monitorLogStatusDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to MonitorLogStatusDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>MonitorLogStatusDTO</returns>
        private MonitorLogStatusDTO GetMonitorLogStatusDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MonitorLogStatusDTO monitorLogStatusDTO = new MonitorLogStatusDTO(dataRow["StatusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["StatusId"]),
                                                         dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(monitorLogStatusDTO);
            return monitorLogStatusDTO;
        }

        /// <summary>
        /// Gets the MonitorLogStatusDTO data of passed id 
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns MonitorLogStatusDTO</returns>
        public MonitorLogStatusDTO GetMonitorLogStatus(int id)
        {
            log.LogMethodEntry(id);
            MonitorLogStatusDTO result = null;
            string query = SELECT_QUERY + @" WHERE MonitorLogStatus.StatusId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMonitorLogStatusDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the monitorLogStatus Table.
        /// </summary>
        /// <param name="monitorLogStatusDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>MonitorLogStatusDTO</returns>
        public MonitorLogStatusDTO Insert(MonitorLogStatusDTO monitorLogStatusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorLogStatusDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[MonitorLogStatus]
                           ([Status]
                           ,[site_id]
                           ,[Guid]
                           ,[MasterEntityId]
                           ,[CreatedBy]
                           ,[CreationDate]
                           ,[LastUpdatedBy]
                           ,[LastUpdateDate])
                     VALUES
                           (@Status
                           ,@site_id
                           ,@NEWID()
                           ,@MasterEntityId
                           ,@CreatedBy
                           ,@GETDATE()
                           ,@LastUpdatedBy
                           ,@GETDATE())
                            SELECT * FROM MonitorLogStatus WHERE StatusId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(monitorLogStatusDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorLogStatusDTO(monitorLogStatusDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting monitorLogStatusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorLogStatusDTO);
            return monitorLogStatusDTO;
        }

        /// <summary>
        ///  Updates the record to the MonitorLog Table.
        /// </summary>
        /// <param name="monitorLogStatusDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>MonitorLogStatusDTO</returns>
        public MonitorLogStatusDTO Update(MonitorLogStatusDTO monitorLogStatusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorLogStatusDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[MonitorLogStatus]
                           SET
                            [Status]         = @Status
                           ,[site_id]        = @site_id
                           ,[MasterEntityId] = @MasterEntityId
                           ,[LastUpdatedBy]  = @LastUpdatedBy
                           ,[LastUpdateDate] = @GETDATE()
                            WHERE Id = @Id     
                            SELECT * FROM MonitorLogStatus WHERE StatusId = @StatusId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(monitorLogStatusDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorLogStatusDTO(monitorLogStatusDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Updating MonitorLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorLogStatusDTO);
            return monitorLogStatusDTO;
        }


        private void RefreshMonitorLogStatusDTO(MonitorLogStatusDTO monitorLogStatusDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorLogStatusDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                monitorLogStatusDTO.StatusId = Convert.ToInt32(dt.Rows[0]["StatusId"]);
                monitorLogStatusDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastupdateDate"]);
                monitorLogStatusDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                monitorLogStatusDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                monitorLogStatusDTO.LastUpdatedBy = loginId;
                monitorLogStatusDTO.CreatedBy = loginId;
                monitorLogStatusDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of MonitorLogStatusDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>monitorLogStatusDTOList</returns>
        public List<MonitorLogStatusDTO> GetAllMonitorLogStatusDTO(List<KeyValuePair<MonitorLogStatusDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MonitorLogStatusDTO> monitorLogStatusDTOList = new List<MonitorLogStatusDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MonitorLogStatusDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MonitorLogStatusDTO.SearchByParameters.STATUS_ID ||
                             searchParameter.Key == MonitorLogStatusDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MonitorLogStatusDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        if (searchParameter.Key == MonitorLogStatusDTO.SearchByParameters.STATUS)
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
                    MonitorLogStatusDTO monitorLogStatusDTO = GetMonitorLogStatusDTO(dataRow);
                    monitorLogStatusDTOList.Add(monitorLogStatusDTO);
                }
            }
            log.LogMethodExit(monitorLogStatusDTOList);
            return monitorLogStatusDTOList;
        }
    }
}
