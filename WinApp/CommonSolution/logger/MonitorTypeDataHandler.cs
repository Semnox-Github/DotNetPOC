/********************************************************************************************
 * Project Name - Logger
 * Description  - Data object of MonitorLogStatus
 *
 **************
 ** Version Log
  **************
  * Version     Date          Modified By            Remarks
 *********************************************************************************************
 *2.70         29-May-2019   Girish Kundar           Created
 ********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// This is the MonitorTypeDataHandler data object class. This acts as data holder for the MonitorType business object
    /// </summary>
    public class MonitorTypeDataHandler
    {
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MonitorType ";
        /// <summary>
        /// Dictionary for searching Parameters for the MonitorTypeDTO object.
        /// </summary>
        private static readonly Dictionary<MonitorTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MonitorTypeDTO.SearchByParameters, string>
        {
            {MonitorTypeDTO.SearchByParameters.MONITOR_TYPE_ID,"MonitorTypeId"},
            {MonitorTypeDTO.SearchByParameters.MINITOR_TYPE,"MonitorType"},
            {MonitorTypeDTO.SearchByParameters.DESCRIPTION,"Description"},
            {MonitorTypeDTO.SearchByParameters.SITE_ID,"site_id"},
            {MonitorTypeDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for MonitorTypeDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public MonitorTypeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MonitorType Record.
        /// </summary>
        /// <param name="monitorTypeDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(MonitorTypeDTO monitorTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@MonitorTypeId", monitorTypeDTO.MonitorTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", monitorTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MonitorType", monitorTypeDTO.MonitorType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", monitorTypeDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to MonitorTypeDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>MonitorTypeDTO</returns>
        private MonitorTypeDTO GetMonitorTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MonitorTypeDTO monitorTypeDTO = new MonitorTypeDTO(dataRow["MonitorTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MonitorTypeId"]),
                                                         dataRow["MonitorType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MonitorType"]),
                                                         dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(monitorTypeDTO);
            return monitorTypeDTO;
        }

        /// <summary>
        /// Gets the MonitorTypeDTO data of passed id 
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns MonitorTypeDTO</returns>
        public MonitorTypeDTO GetMonitorType(int id)
        {
            log.LogMethodEntry(id);
            MonitorTypeDTO result = null;
            string query = SELECT_QUERY + @" WHERE MonitorType.MonitorTypeId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMonitorTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the MonitorType Table.
        /// </summary>
        /// <param name="monitorTypeDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>MonitorTypeDTO</returns>
        public MonitorTypeDTO Insert(MonitorTypeDTO monitorTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorTypeDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[MonitorType]
                           ([MonitorType]
                           ,[Description]
                           ,[site_id]
                           ,[Guid]
                           ,[MasterEntityId]
                           ,[CreatedBy]
                           ,[CreationDate]
                           ,[LastUpdatedBy]
                           ,[LastUpdateDate])
                     VALUES
                           (@MonitorType
                           ,@Description
                           ,@site_id
                            @NEWID()
                           ,@MasterEntityId
                           ,@CreatedBy
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,GETDATE())
                            SELECT * FROM MonitorType WHERE MonitorTypeId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(monitorTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorTypeDTO(monitorTypeDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting MonitorTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorTypeDTO);
            return monitorTypeDTO;
        }

        /// <summary>
        ///  Updates the record to the MonitorType Table.
        /// </summary>
        /// <param name="monitorTypeDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>MonitorTypeDTO</returns>
        public MonitorTypeDTO Update(MonitorTypeDTO monitorTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorTypeDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[MonitorType]
                           SET
                            [MonitorType]    =@MonitorType
                           ,[Description]    =@Description
                           ,[site_id]        =@site_id
                           ,[MasterEntityId] =@MasterEntityId
                           ,[LastUpdatedBy]  =@LastUpdatedBy
                           ,[LastUpdateDate] = GETDATE()
                            WHERE MonitorTypeId = @MonitorTypeId
                            SELECT * FROM MonitorType WHERE MonitorTypeId = @MonitorTypeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(monitorTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMonitorTypeDTO(monitorTypeDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Updating MonitorTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(monitorTypeDTO);
            return monitorTypeDTO;
        }

        private void RefreshMonitorTypeDTO(MonitorTypeDTO monitorTypeDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(monitorTypeDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                monitorTypeDTO.MonitorTypeId = Convert.ToInt32(dt.Rows[0]["MonitorTypeId"]);
                monitorTypeDTO.LastUpdateDate = Convert.ToDateTime(dt.Rows[0]["LastupdateDate"]);
                monitorTypeDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                monitorTypeDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                monitorTypeDTO.LastUpdatedBy = loginId;
                monitorTypeDTO.CreatedBy = loginId;
                monitorTypeDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of MonitorTypeDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>monitorTypeDTOList</returns>
        public List<MonitorTypeDTO> GetAllMonitorTypeDTO(List<KeyValuePair<MonitorTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MonitorTypeDTO> monitorTypeDTOList = new List<MonitorTypeDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MonitorTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MonitorTypeDTO.SearchByParameters.MONITOR_TYPE_ID ||
                             searchParameter.Key == MonitorTypeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MonitorTypeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MonitorTypeDTO.SearchByParameters.MINITOR_TYPE
                            || searchParameter.Key == MonitorTypeDTO.SearchByParameters.DESCRIPTION)
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
                    MonitorTypeDTO monitorTypeDTO = GetMonitorTypeDTO(dataRow);
                    monitorTypeDTOList.Add(monitorTypeDTO);
                }
            }
            log.LogMethodExit(monitorTypeDTOList);
            return monitorTypeDTOList;
        }
    }
}
