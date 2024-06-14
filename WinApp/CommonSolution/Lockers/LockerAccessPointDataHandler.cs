/********************************************************************************************
 * Project Name - Locker Access Point DataHandler
 * Description  - Data handler of the Locker Access Point  class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        14-Jul-2017   Raghuveera          Created 
 *2.70.2        18-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                                                                                  
 *2.90        18-May-2020   Mushahid Faizan     Modified : GetLockerAccessPointList()
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Locker Access Point Data Handler - Handles insert, update and select of locker access point data objects
    /// </summary>
    public class LockerAccessPointDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM LockerAccessPoint AS lap";

        /// <summary>
        /// Dictionary for searching Parameters for the Locker Access point object.
        /// </summary>
        private static readonly Dictionary<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string> DBSearchParameters = new Dictionary<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>
            {
                {LockerAccessPointDTO.SearchByLockerAccessPointParameters.ACCESS_POINT_ID, "lap.AccessPointId"},
                {LockerAccessPointDTO.SearchByLockerAccessPointParameters.NAME, "lap.Name"},
                {LockerAccessPointDTO.SearchByLockerAccessPointParameters.IP_ADDRESS, "lap.IPAddress"},
                {LockerAccessPointDTO.SearchByLockerAccessPointParameters.LOCKER_ID_FROM, "lap.LockerIDFrom"},
                {LockerAccessPointDTO.SearchByLockerAccessPointParameters.LOCKER_ID_TO, "lap.LockerIDTo"},
                {LockerAccessPointDTO.SearchByLockerAccessPointParameters.IS_ALIVE,"lap.IsAlive"},
                {LockerAccessPointDTO.SearchByLockerAccessPointParameters.ACTIVE_FLAG, "lap.IsActive"},
                {LockerAccessPointDTO.SearchByLockerAccessPointParameters.MASTER_ENTITY_ID, "lap.MasterEntityId"},
                {LockerAccessPointDTO.SearchByLockerAccessPointParameters.SITE_ID, "lap.site_id"},
                {LockerAccessPointDTO.SearchByLockerAccessPointParameters.LOAD_CHILD_RECORDS, "0"}
            };

        /// <summary>
        /// Default constructor of LockerAccessPointDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerAccessPointDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LockerAccessPoint parameters Record.
        /// </summary>
        /// <param name="lockerAccessPointDTO">lockerAccessPointDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(LockerAccessPointDTO lockerAccessPointDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerAccessPointDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@accessPointId", lockerAccessPointDTO.AccessPointId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(lockerAccessPointDTO.Name) ? DBNull.Value : (object)lockerAccessPointDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@iPAddress", lockerAccessPointDTO.IPAddress, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@portNo", lockerAccessPointDTO.PortNo, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@channel", lockerAccessPointDTO.Channel, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gatewayIP", string.IsNullOrEmpty(lockerAccessPointDTO.GatewayIP) ? DBNull.Value : (object)lockerAccessPointDTO.GatewayIP));
            parameters.Add(dataAccessHandler.GetSQLParameter("@baudRate", lockerAccessPointDTO.BaudRate == -1 ? DBNull.Value : (object)lockerAccessPointDTO.BaudRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lockerIDFrom", lockerAccessPointDTO.LockerIDFrom == -1 ? DBNull.Value : (object)lockerAccessPointDTO.LockerIDFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lockerIDTo", lockerAccessPointDTO.LockerIDTo == -1 ? DBNull.Value : (object)lockerAccessPointDTO.LockerIDTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataPackingTime", lockerAccessPointDTO.DataPackingTime == -1 ? DBNull.Value : (object)lockerAccessPointDTO.DataPackingTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataPackingSize", lockerAccessPointDTO.DataPackingSize == -1 ? DBNull.Value : (object)lockerAccessPointDTO.DataPackingSize));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isAlive", lockerAccessPointDTO.IsAlive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", lockerAccessPointDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", lockerAccessPointDTO.Siteid, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", lockerAccessPointDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@groupCode", string.IsNullOrEmpty(lockerAccessPointDTO.GroupCode) ? DBNull.Value : (object)lockerAccessPointDTO.GroupCode));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the lockerAccessPoint record to the database
        /// </summary>
        /// <param name="lockerAccessPoint">LockerAccessPointDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns LockerAccessPointDTO</returns>
        public LockerAccessPointDTO InsertLockerAccessPoint(LockerAccessPointDTO lockerAccessPoint, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerAccessPoint, loginId, siteId);
            string insertLockerAccessPointQuery = @"INSERT INTO[dbo].[LockerAccessPoint]  
                                                        ( 
                                                          Name,
                                                          IPAddress,
                                                          PortNo,
                                                          Channel,
                                                          GatewayIP,
                                                          BaudRate,
                                                          LockerIDFrom,
                                                          LockerIDTo,
                                                          DataPackingTime,
                                                          DataPackingSize,
                                                          IsAlive,
                                                          IsActive,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy, 
                                                          LastupdatedDate,
                                                          Guid,
                                                          site_id,
                                                          MasterEntityId,
                                                          GroupCode
                                                        ) 
                                                values 
                                                        (    
                                                          @name,
                                                          @iPAddress,
                                                          @portNo,
                                                          @channel,
                                                          @gatewayIP,
                                                          @baudRate,
                                                          @lockerIDFrom,
                                                          @lockerIDTo,
                                                          @dataPackingTime,
                                                          @dataPackingSize,
                                                          @isAlive,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate(),                                                       
                                                          Newid(),
                                                          @siteid,
                                                          @masterEntityId,
                                                          @groupCode
                                                        )SELECT * FROM LockerAccessPoint WHERE AccessPointId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertLockerAccessPointQuery, GetSQLParameters(lockerAccessPoint, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerAccessPoint(lockerAccessPoint, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting lockerAccessPoint", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerAccessPoint);
            return lockerAccessPoint;
        }

        /// <summary>
        /// Updates the lockerAccessPoint record
        /// </summary>
        /// <param name="lockerAccessPoint">LockerAccessPointDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>LockerAccessPointDTO</returns>
        public LockerAccessPointDTO UpdateLockerAccessPoint(LockerAccessPointDTO lockerAccessPoint, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerAccessPoint, loginId, siteId);
            string updateLockerAccessPointQuery = @"update LockerAccessPoint set 
                                                  Name = @name,
                                                  IPAddress = @iPAddress,
                                                  PortNo = @portNo,
                                                  Channel = @channel,
                                                  GatewayIP = @gatewayIP,
                                                  BaudRate = @baudRate,
                                                  LockerIDFrom = @lockerIDFrom,
                                                  LockerIDTo = @lockerIDTo,
                                                  DataPackingTime = @dataPackingTime,
                                                  DataPackingSize = @dataPackingSize,
                                                  IsAlive = @isAlive,
                                                  IsActive = @isActive,
                                                  LastUpdatedBy = @lastUpdatedBy, 
                                                  LastupdatedDate = Getdate(),
                                                  -- site_id=@siteid,
                                                  MasterEntityId=@masterEntityId,
                                                  GroupCode = @groupCode
                                                  where AccessPointId = @accessPointId 
                                                  SELECT* FROM LockerAccessPoint WHERE  AccessPointId = @accessPointId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateLockerAccessPointQuery, GetSQLParameters(lockerAccessPoint, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerAccessPoint(lockerAccessPoint, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LockerAccessPoint", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerAccessPoint);
            return lockerAccessPoint;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="lockerAccessPointDTO">lockerAccessPointDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshLockerAccessPoint(LockerAccessPointDTO lockerAccessPointDTO, DataTable dt)
        {
            log.LogMethodEntry(lockerAccessPointDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                lockerAccessPointDTO.AccessPointId = Convert.ToInt32(dt.Rows[0]["AccessPointId"]);
                lockerAccessPointDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                lockerAccessPointDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                lockerAccessPointDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                lockerAccessPointDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                lockerAccessPointDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                lockerAccessPointDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to LockerAccessPointDTO class type
        /// </summary>
        /// <param name="lockerAccessPointDataRow">LockerAccessPointDTO DataRow</param>
        /// <returns>Returns LockerAccessPointDTO</returns>
        private LockerAccessPointDTO GetLockerAccessPointDTO(DataRow lockerAccessPointDataRow)
        {
            log.LogMethodEntry(lockerAccessPointDataRow);

            LockerAccessPointDTO lockerAccessPointDataObject = new LockerAccessPointDTO(Convert.ToInt32(lockerAccessPointDataRow["AccessPointId"]),
                                            lockerAccessPointDataRow["Name"].ToString(),
                                            lockerAccessPointDataRow["IPAddress"].ToString(),
                                            lockerAccessPointDataRow["PortNo"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAccessPointDataRow["PortNo"]),
                                            lockerAccessPointDataRow["Channel"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAccessPointDataRow["Channel"]),
                                            lockerAccessPointDataRow["GatewayIP"].ToString(),
                                            lockerAccessPointDataRow["BaudRate"] == DBNull.Value ? -1 : Convert.ToInt64(lockerAccessPointDataRow["BaudRate"]),
                                            lockerAccessPointDataRow["LockerIDFrom"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAccessPointDataRow["LockerIDFrom"]),
                                            lockerAccessPointDataRow["LockerIDTo"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAccessPointDataRow["LockerIDTo"]),
                                            lockerAccessPointDataRow["DataPackingTime"] == DBNull.Value ? -1 : Convert.ToInt64(lockerAccessPointDataRow["DataPackingTime"]),
                                            lockerAccessPointDataRow["DataPackingSize"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAccessPointDataRow["DataPackingSize"]),
                                            lockerAccessPointDataRow["IsAlive"] == DBNull.Value ? false : Convert.ToBoolean(lockerAccessPointDataRow["IsAlive"]),
                                            lockerAccessPointDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(lockerAccessPointDataRow["IsActive"]),
                                            lockerAccessPointDataRow["CreatedBy"].ToString(),
                                            lockerAccessPointDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerAccessPointDataRow["CreationDate"]),
                                            lockerAccessPointDataRow["LastUpdatedBy"].ToString(),
                                            lockerAccessPointDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerAccessPointDataRow["LastupdatedDate"]),
                                            lockerAccessPointDataRow["Guid"].ToString(),
                                            lockerAccessPointDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAccessPointDataRow["site_id"]),
                                            lockerAccessPointDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(lockerAccessPointDataRow["SynchStatus"]),
                                            lockerAccessPointDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAccessPointDataRow["MasterEntityId"]),//Modification on 18-Jul-2016 for publish feature
                                            lockerAccessPointDataRow["GroupCode"] == DBNull.Value ? "0" : lockerAccessPointDataRow["GroupCode"].ToString()
                                            );
            log.LogMethodExit(lockerAccessPointDataObject);
            return lockerAccessPointDataObject;
        }

        /// <summary>
        /// Gets the lockerAccessPoint data of passed lockerAccessPoint Id
        /// </summary>
        /// <param name="accessPointId">integer type parameter</param>
        /// <returns>Returns LockerAccessPointDTO</returns>
        public LockerAccessPointDTO GetLockerAccessPoint(int accessPointId)
        {
            log.LogMethodEntry(accessPointId);
            string selectLockerAccessPointQuery = SELECT_QUERY + @" WHERE lap.AccessPointId = @accessPointId";
            SqlParameter[] selectLockerAccessPointParameters = new SqlParameter[1];
            selectLockerAccessPointParameters[0] = new SqlParameter("@accessPointId", accessPointId);
            DataTable lockerAccessPoint = dataAccessHandler.executeSelectQuery(selectLockerAccessPointQuery, selectLockerAccessPointParameters, sqlTransaction);
            if (lockerAccessPoint.Rows.Count > 0)
            {
                DataRow lockerAccessPointRow = lockerAccessPoint.Rows[0];
                LockerAccessPointDTO lockerAccessPointDataObject = GetLockerAccessPointDTO(lockerAccessPointRow);
                LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler(sqlTransaction);
                lockerAccessPointDataObject.LockerZonesDTOList = lockerZonesDataHandler.GetLockerZonesList(lockerAccessPointDataObject.GroupCode, lockerAccessPointDataObject.LockerIDFrom, lockerAccessPointDataObject.LockerIDTo, false);
                log.LogMethodExit(lockerAccessPointDataObject);
                return lockerAccessPointDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the Locker id exists in the assigned range
        /// </summary>
        /// <param name="lockerId">integer type parameter</param>
        /// <param name="accessPointId">integer type parameter</param>
        /// <param name="apGroupCode">string type parameter</param>
        /// <param name="channel"> channel number</param>
        /// <returns>Returns LockerAccessPointDTO</returns>
        public LockerAccessPointDTO GetLockerIdInclussiveAPDetails(int lockerId, int accessPointId, string apGroupCode, int channel)
        {
            log.LogMethodEntry(lockerId, accessPointId, apGroupCode, channel);
            LockerAccessPointDTO lockerAccessPointDataObject = null;
            string selectLockerAccessPointQuery = @"select *
                                         from LockerAccessPoint
                                        where (AccessPointId <> @accessPointId) and (@lockerId Between LockerIDFrom and LockerIDTo) and Channel = @channel and GroupCode = @zonecode ";
            SqlParameter[] selectLockerAccessPointParameters = new SqlParameter[4];
            selectLockerAccessPointParameters[0] = new SqlParameter("@lockerId", lockerId);
            selectLockerAccessPointParameters[1] = new SqlParameter("@accessPointId", accessPointId);
            selectLockerAccessPointParameters[2] = new SqlParameter("@zonecode", apGroupCode);
            selectLockerAccessPointParameters[3] = new SqlParameter("@channel", channel);
            DataTable lockerAccessPoint = dataAccessHandler.executeSelectQuery(selectLockerAccessPointQuery, selectLockerAccessPointParameters, sqlTransaction);
            if (lockerAccessPoint.Rows.Count > 0)
            {
                DataRow lockerAccessPointRow = lockerAccessPoint.Rows[0];
                lockerAccessPointDataObject = GetLockerAccessPointDTO(lockerAccessPointRow);
                //LockerDataHandler lockerDataHandler = new LockerDataHandler();
                //lockerAccessPointDataObject.LockerList = lockerDataHandler.GetLockers(lockerAccessPointDataObject.LockerIDFrom, lockerAccessPointDataObject.LockerIDTo);
                LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler(sqlTransaction);
                lockerAccessPointDataObject.LockerZonesDTOList = lockerZonesDataHandler.GetLockerZonesList(lockerAccessPointDataObject.GroupCode, lockerAccessPointDataObject.LockerIDFrom, lockerAccessPointDataObject.LockerIDTo, false);
            }
            log.LogMethodExit(lockerAccessPointDataObject);
            return lockerAccessPointDataObject;
        }

        /// <summary>
        /// Gets the LockerAccessPointDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LockerAccessPointDTO matching the search criteria</returns>
        public List<LockerAccessPointDTO> GetLockerAccessPointList(List<KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<LockerAccessPointDTO> lockerAccessPointDTOList = new List<LockerAccessPointDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            bool loadChildRecords = false;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LockerAccessPointDTO.SearchByLockerAccessPointParameters.ACCESS_POINT_ID
                             || searchParameter.Key == LockerAccessPointDTO.SearchByLockerAccessPointParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerAccessPointDTO.SearchByLockerAccessPointParameters.LOCKER_ID_FROM)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerAccessPointDTO.SearchByLockerAccessPointParameters.LOCKER_ID_TO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerAccessPointDTO.SearchByLockerAccessPointParameters.IS_ALIVE
                                  || searchParameter.Key == LockerAccessPointDTO.SearchByLockerAccessPointParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }

                        else if (searchParameter.Key == LockerAccessPointDTO.SearchByLockerAccessPointParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerAccessPointDTO.SearchByLockerAccessPointParameters.NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == LockerAccessPointDTO.SearchByLockerAccessPointParameters.IP_ADDRESS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerAccessPointDTO.SearchByLockerAccessPointParameters.LOAD_CHILD_RECORDS && searchParameter.Value == "1")
                        {
                            loadChildRecords = true;
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
                    LockerAccessPointDTO lockerAccessPointDataObject = GetLockerAccessPointDTO(dataRow);
                    if (loadChildRecords == true)
                    {
                        LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler();
                        log.Debug("GetLockerAccessPointList(searchParameters) Loading Zone details.Group:" + lockerAccessPointDataObject.GroupCode + ", locker range:" + lockerAccessPointDataObject.LockerIDFrom + "-" + lockerAccessPointDataObject.LockerIDTo);
                        log.LogVariableState("lockerAccessPointDataObject", lockerAccessPointDataObject);

                        lockerAccessPointDataObject.LockerZonesDTOList = lockerZonesDataHandler.GetLockerZonesList(lockerAccessPointDataObject.GroupCode, lockerAccessPointDataObject.LockerIDFrom, lockerAccessPointDataObject.LockerIDTo, false);
                        log.Debug("GetLockerAccessPointList(searchParameters)  Zone loaded :" + ((lockerAccessPointDataObject.LockerZonesDTOList != null) ? lockerAccessPointDataObject.LockerZonesDTOList.Count : 0));
                        log.LogVariableState("lockerAccessPointDataObject", lockerAccessPointDataObject);
                        lockerAccessPointDTOList.Add(lockerAccessPointDataObject);
                    }
                    else
                    {
                        lockerAccessPointDTOList.Add(lockerAccessPointDataObject);
                    }
                }
                log.LogMethodExit(lockerAccessPointDTOList);
                return lockerAccessPointDTOList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}
