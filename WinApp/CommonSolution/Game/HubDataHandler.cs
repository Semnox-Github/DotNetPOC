/********************************************************************************************
 * Project Name - Hub Data Handler                                                                          
 * Description  - Data handler of the hub class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015   Kiran          Created 
 *2.40        06-Sept-2018  Jagan          Added new filte for site id and changed the get hublist search parametrs query.
 *2.41        07-Nov-2018   Rajiv          Modified existing logic to handle null values.
 *2.50.0      12-dec-2018   Guru S A        Who column changes
 *2.70        10-Jul-2019   Girish kundar   Added IsEByte field. 
 *2.70.2        29-Jul-2019   Deeksha         Added GetSqlParameter(),Insert/Update method returns DTO.SQL injection issue Fix.
  *2.90.0      21-Jun-2020   Girish Kundar   Modofied : Phase -2 changes /REST API report module 
*2.110.0      12-Feb-2021   Girish Kundar     Added new field for Radian Wristband (IsRadian) 
  *2.110.0     15-Dec-2020   Prajwal S       Modified : Changes for REST API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Globalization;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Hub data handler - Handles insert, update and select of hub data objects
    /// </summary>
    public class HubDataHandler
    {
        private const string SELECT_QUERY = @"SELECT * FROM masters AS h ";
        private static readonly Dictionary<HubDTO.SearchByHubParameters, string> DBSearchParameters = new Dictionary<HubDTO.SearchByHubParameters, string>
        {
                {HubDTO.SearchByHubParameters.IS_ACTIVE, "h.active_flag"},
                {HubDTO.SearchByHubParameters.HUB_NAME, "h.master_name"},
                {HubDTO.SearchByHubParameters.HUB_ID, "h.master_id"},
                {HubDTO.SearchByHubParameters.SITE_ID, "h.site_id"},
                {HubDTO.SearchByHubParameters.MASTER_ENTITY_ID, "h.MasterEntityId"},
                {HubDTO.SearchByHubParameters.IS_RADIAN, "h.IsRadian"},
                {HubDTO.SearchByHubParameters.IS_EBYTE, "h.IsEByte"},
                {HubDTO.SearchByHubParameters.RESTART_AP, "h.RestartAP"}
        };
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction; //MD
        private readonly DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>(); //added

        /// <summary>
        /// Default constructor of HubDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public HubDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating HubDataHandler Record.
        /// </summary>
        /// <param name="hubDTO">hubDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(HubDTO hubDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(hubDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterId", hubDTO.MasterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterName", hubDTO.MasterName, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@portNumber", hubDTO.PortNumber == null ? DBNull.Value : (object)hubDTO.PortNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@baudRate", hubDTO.BaudRate <= 0 ? DBNull.Value : (object)hubDTO.BaudRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notes", hubDTO.Notes == string.Empty ? DBNull.Value : (object)(hubDTO.Notes)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@activeFlag", (hubDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@address", string.IsNullOrEmpty(hubDTO.Address) ? DBNull.Value : (object)hubDTO.Address));
            parameters.Add(dataAccessHandler.GetSQLParameter("@frequency", string.IsNullOrEmpty(hubDTO.Frequency) ? DBNull.Value : (object)hubDTO.Frequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@serverMachine", string.IsNullOrEmpty(hubDTO.ServerMachine) ? DBNull.Value : (object)hubDTO.ServerMachine));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ipAddress", string.IsNullOrEmpty(hubDTO.IPAddress) ? DBNull.Value : (object)hubDTO.IPAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@directMode", hubDTO.DirectMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@macAddress", string.IsNullOrEmpty(hubDTO.MACAddress) ? DBNull.Value : (object)(hubDTO.MACAddress)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tcpPort", hubDTO.TCPPort <= 0 ? DBNull.Value : (object)(hubDTO.TCPPort)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@restartAP", hubDTO.RestartAP == false ? DBNull.Value : (object)(hubDTO.RestartAP)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isEBYTE", hubDTO.IsEByte == false ? DBNull.Value : (object)(hubDTO.IsEByte)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isRadian", hubDTO.IsRadian == false ? DBNull.Value : (object)(hubDTO.IsRadian)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", hubDTO.MasterEntityId <= 0 ? DBNull.Value : (object)(hubDTO.MasterEntityId)));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the hub records
        /// </summary>
        /// <param name="hub">HubDTO</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public HubDTO InsertHub(HubDTO hub, string loginId, int siteId)
        {
            log.LogMethodEntry(hub, loginId, siteId);
            string insertHubQuery = @"insert into masters 
                                                        (
                                                          master_name,
                                                          port_number,
                                                          baud_rate, 
                                                          notes, 
                                                          active_flag,
                                                          address,
                                                          frequency, 
                                                          ServerMachine, 
                                                          DirectMode, 
                                                          Guid, 
                                                          site_id, 
                                                          IPAddress,
                                                          TCPPort,
                                                          MACAddress,
                                                          RestartAP,
                                                          MasterEntityId,
                                                          creationDate,
                                                          createdBy,
                                                          lastupdateDate,
                                                          lastupdatedBy,
                                                          IsEBYTE,
                                                          IsRadian
                                                                ) 
                                                values 
                                                        (
                                                          @masterName,
                                                          @portNumber,
                                                          @baudRate,
                                                          @notes,
                                                          @activeFlag,
                                                          @address,
                                                          @frequency,
                                                          @serverMachine,
                                                          @directMode,
                                                          NEWID(),
                                                          @siteId, 
                                                          @ipAddress, 
                                                          @tcpPort,
                                                          @macAddress,
                                                          @restartAP,
                                                          @masterEntityId,
                                                          GetDate(),
                                                          @createdBy,
                                                          GetDate(),
                                                          @lastUpdatedBy,
                                                          @isEBYTE,
                                                          @isRadian
                                                       ) SELECT * FROM masters WHERE master_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertHubQuery, GetSQLParameters(hub, loginId, siteId).ToArray(), sqlTransaction);
                RefreshHubDTO(hub, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting hub", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(hub);
            return hub;
        }

        /// <summary>
        /// Updates the hub record
        /// </summary>
        /// <param name="hub">Parameter of the type HubDTO class</param>
        /// <param name="loginId">Parameter for loginId</param>
        /// <param name="siteId">Parameter for siteId</param>
        /// <returns>Returns the count of updated rows</returns>
        public HubDTO UpdateHub(HubDTO hub, string loginId, int siteId)
        {
            log.LogMethodEntry(hub, loginId, siteId);
            string updateHubQuery = @"update masters 
                                         set master_name = @masterName, 
                                             port_number = @portNumber, 
                                             baud_rate = @baudRate, 
                                             notes = @notes, 
                                             active_flag = @activeFlag,
                                             address = @address,
                                             frequency = @frequency,
                                             ServerMachine = @serverMachine,
                                             DirectMode = @directMode,
                                             IPAddress = @ipAddress,
                                             TCPPort = @tcpPort,
                                             MACAddress = @macAddress,
                                             RestartAP = @restartAP,
                                             MasterEntityId = @masterEntityId,
                                             lastupdateDate = GetDate(),
                                             lastupdatedBy = @lastUpdatedBy      ,   
                                             IsEBYTE  = @isEBYTE, 
                                             IsRadian  = @isRadian 
                                              where master_id = @masterId
                        SELECT * FROM masters WHERE master_id = @masterId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateHubQuery, GetSQLParameters(hub, loginId, siteId).ToArray(), sqlTransaction);
                RefreshHubDTO(hub, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating hub", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(hub);
            return hub;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="hub">hub object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshHubDTO(HubDTO hub, DataTable dt)
        {
            log.LogMethodEntry(hub, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                hub.MasterId = Convert.ToInt32(dt.Rows[0]["master_id"]);
                hub.LastupdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                hub.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                hub.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                hub.LastupdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                hub.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                hub.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the Masters record of passed Master Id
        /// </summary>
        /// <param name="masterId">integer type parameter</param>
        internal void DeleteHub(int masterId)
        {
            log.LogMethodEntry(masterId);
            string query = @"DELETE FROM [masters] WHERE [master_id] = @Original_master_id";
            try
            {
                SqlParameter parameter = new SqlParameter("@Original_master_id", masterId);
                dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error(query);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to HubDTO calss type
        /// </summary>
        /// <param name="hubDataRow">Hub DataRow</param>
        /// <returns>Returns HubDTO</returns>
        private HubDTO GetHubDTO(DataRow hubDataRow)
        {
            log.LogMethodEntry(hubDataRow);
            HubDTO hubDataObject = new HubDTO(Convert.ToInt32(hubDataRow["master_id"]),
                                                    hubDataRow["master_name"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["master_name"]),
                                                     hubDataRow["port_number"] == DBNull.Value ? (int?)null : Convert.ToInt32(hubDataRow["port_number"]),
                                                    hubDataRow["baud_rate"] == DBNull.Value ? 0 : Convert.ToInt32(hubDataRow["baud_rate"]),
                                                    hubDataRow["notes"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["notes"]),
                                                    hubDataRow["active_flag"] == DBNull.Value ? false : (hubDataRow["active_flag"]).ToString().Equals("Y") ? true : false,
                                                    hubDataRow["address"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["address"]),
                                                    hubDataRow["frequency"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["frequency"]),
                                                    hubDataRow["ServerMachine"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["ServerMachine"]),
                                                    hubDataRow["DirectMode"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["DirectMode"]),
                                                    hubDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["Guid"]),
                                                    hubDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(hubDataRow["site_id"]),
                                                    hubDataRow["IPAddress"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["IPAddress"]),
                                                    hubDataRow["TCPPort"] == DBNull.Value ? -1 : Convert.ToInt32(hubDataRow["TCPPort"]),
                                                    hubDataRow["MACAddress"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["MACAddress"]),
                                                    hubDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(hubDataRow["SynchStatus"])),
                                                    hubDataRow["RestartAP"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(hubDataRow["RestartAP"])),
                                                    hubDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(hubDataRow["MasterEntityId"]),
                                                    hubDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["CreatedBy"]),
                                                    hubDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(hubDataRow["CreationDate"]),
                                                    hubDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(hubDataRow["LastUpdatedBy"]),
                                                    hubDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(hubDataRow["LastUpdateDate"]),
                                                    hubDataRow["IsEBYTE"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(hubDataRow["IsEBYTE"])),
                                                    hubDataRow["IsRadian"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(hubDataRow["IsRadian"]))
                                                             );
            log.LogMethodExit(hubDataObject);
            return hubDataObject;
        }


        /// <summary>
        /// Gets the hub data of passed hub id
        /// </summary>
        /// <param name="hubId">Hub Id</param>
        /// <returns>Returns HubDTO</returns>
        public HubDTO GetHub(int hubId)
        {
            log.LogMethodEntry(hubId);
            string selectHubQuery = @"select *
                                        from masters
                                       where master_id = @masterId";
            List<SqlParameter> selectHubParameters = new List<SqlParameter>();
            selectHubParameters.Add(new SqlParameter("@masterId", hubId));
            DataTable hubData = dataAccessHandler.executeSelectQuery(selectHubQuery, selectHubParameters.ToArray(), sqlTransaction);
            if (hubData.Rows.Count > 0)
            {
                DataRow hubDataRow = hubData.Rows[0];
                log.LogMethodExit(hubDataRow);
                return GetHubDTO(hubDataRow);
            }
            else
            {
                log.LogMethodExit("Returning null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the HubDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of HubDTO matching the search criteria</returns>
        public List<HubDTO> GetHubList(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)//added
        {
            log.LogMethodEntry(searchParameters);
            List<HubDTO> hubDTOList = new List<HubDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage >= 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY h.master_id OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                hubDTOList = new List<HubDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    HubDTO hubDTO = GetHubDTO(dataRow);
                    hubDTOList.Add(hubDTO);
                }
            }
            log.LogMethodExit(hubDTOList);
            return hubDTOList;
        }

        /// <summary>
        /// Returns the no of Hub matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetHubsCount(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            int hubDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                hubDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(hubDTOCount);
            return hubDTOCount;
        }


        /// <summary>
        /// Gets the HubDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">SqlTransaction object</param>
        /// <returns>Returns the list of HubDTO matching the search criteria</returns>
        //public List<HubDTO> GetHubList(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters) //modified.
        //{
        //    log.LogMethodEntry(searchParameters);
        //    List<HubDTO> hubDTOList = null;
        //    parameters.Clear();
        //    string selectQuery = SELECT_QUERY;
        //    selectQuery = selectQuery + GetFilterQuery(searchParameters);
        //    DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        hubDTOList = new List<HubDTO>();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            HubDTO hubDTO = GetHubDTO(dataRow);
        //            hubDTOList.Add(hubDTO);
        //        }
        //    }
        //    log.LogMethodExit(hubDTOList);
        //    return hubDTOList;
        //}

        /// <summary>
        /// Gets the HubDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="loadAttributes">loadAttributes</param>
        /// <returns>Returns the list of HubDTO matching the search criteria</returns>
        public string GetFilterQuery(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string joiner = string.Empty;
            StringBuilder query = new StringBuilder("");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                query = new StringBuilder(" where ");
                foreach (KeyValuePair<HubDTO.SearchByHubParameters, string> searchParameter in searchParameters)
                {
                    
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == HubDTO.SearchByHubParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == HubDTO.SearchByHubParameters.HUB_ID
                            || searchParameter.Key == HubDTO.SearchByHubParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == HubDTO.SearchByHubParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == HubDTO.SearchByHubParameters.IS_EBYTE
                                || searchParameter.Key == HubDTO.SearchByHubParameters.IS_RADIAN
                                 || searchParameter.Key == HubDTO.SearchByHubParameters.RESTART_AP)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
            }
            log.LogMethodExit();
            return query.ToString();
        }
        internal List<HubDTO> GetMasterReport(int hubId, DateTime fromDate)
        {
            log.LogMethodEntry(hubId);
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            List<HubDTO> hubDTOList = new List<HubDTO>();
            string query = @"select Machine_id, Machine_name, m.Machine_address, count(l.machineAddress) total_failures, sum(case when receivedData like 'TRANSMISSION%' then 1 else 0 end) Trx_Failures, " +
                                "sum(case when receivedData like 'HUB RECEIVE%' then 1 else 0 end) Rcv_Failures " +
                                "from machines m left outer join communicationLog l " +
                                "on m.machine_id = l.MachineId " +
                                "and l.Timestamp >= @fromDate " +
                                "where (m.master_id = @master_id or @master_id = -1) " +
                                "and m.active_flag = 'Y' " +
                                "group by machine_id, m.machine_address, Machine_name order by 3 desc";
            try
            {
                sqlParameters.Add(new SqlParameter("@master_id", hubId));
                sqlParameters.Add(new SqlParameter("@fromdate", fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                DataTable hubData = dataAccessHandler.executeSelectQuery(query, sqlParameters.ToArray(), sqlTransaction);
                if (hubData.Rows.Count > 0)
                {
                    hubDTOList = new List<HubDTO>();
                    foreach (DataRow hubDataRow in hubData.Rows)
                    {
                        HubDTO hubDataObject = GetHubDTO(hubDataRow);
                        hubDTOList.Add(hubDataObject);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            return hubDTOList;
        }
        internal DateTime? GetHubModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate
                            FROM (
                            select max(LastUpdateDate) LastUpdatedDate from masters WHERE (site_id = @siteId or @siteId = -1)
                             ) hublastupdate";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
