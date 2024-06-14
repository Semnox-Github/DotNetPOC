/********************************************************************************************
* Project Name - LockerZones Programs 
* Description  - Data object of the LockerZones 
* 
**************
**Version Log
**************
*Version     Date           Modified By         Remarks          
*********************************************************************************************
*1.00        6-Nov-2017     Archana             Created 
*2.70.2        19-Jul-2019    Dakshakh raj        Modified : added GetSQLParameters(),
*                                                         SQL injection Issue Fix
*2.70.2        10-Dec-2019   Jinto Thomas       Removed siteid from update query    
*2.100.0       31-Aug-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
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
    ///  LockerZone Data Handler - Handles insert, update and select of  Event objects
    /// </summary>
    public class LockerZonesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM LockerZones ";

        /// <summary>
        /// Dictionary for searching Parameters for the Locker Zone object.
        /// </summary>
        private static readonly Dictionary<LockerZonesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LockerZonesDTO.SearchByParameters, string>
            {
                {LockerZonesDTO.SearchByParameters.ZONE_ID, "ZoneId"},
                {LockerZonesDTO.SearchByParameters.ZONE_NAME, "ZoneName"},
                {LockerZonesDTO.SearchByParameters.ACTIVE_FLAG, "ActiveFlag"},
                {LockerZonesDTO.SearchByParameters.PARENT_ZONE_ID, "ParentZoneId"},
                {LockerZonesDTO.SearchByParameters.ZONE_CODE, "ZoneCode"},
                {LockerZonesDTO.SearchByParameters.SITE_ID, "site_id"},
                {LockerZonesDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {LockerZonesDTO.SearchByParameters.LOCKER_MODE,"LockerMode"}
            };

        /// <summary>
        /// Default constructor of LockerZonesDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerZonesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameter Helper
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="parameterName">parameterName</param>
        /// <param name="value">value</param>
        /// <param name="negetiveValueNull">negetiveValueNull</param>
        private void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        {
            log.LogMethodEntry(parameters, parameterName, value, negetiveValueNull);
            if (parameters != null && !string.IsNullOrEmpty(parameterName))
            {
                if (value is int)
                {
                    if (negetiveValueNull && ((int)value) < 0)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else if (value is string)
                {
                    if (string.IsNullOrEmpty(value as string))
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else
                {
                    if (value == null)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LockerPanel parameters Record.
        /// </summary>
        /// <param name="lockerZonesDTO">lockerZonesDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(LockerZonesDTO lockerZonesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerZonesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@zoneId", lockerZonesDTO.ZoneId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ZoneName", lockerZonesDTO.ZoneName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentZoneId", lockerZonesDTO.ParentZoneId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ZoneCode", lockerZonesDTO.ZoneCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", lockerZonesDTO.ActiveFlag, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lockerMode", lockerZonesDTO.LockerMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lockerMake", lockerZonesDTO.LockerMake));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", lockerZonesDTO.MasterEntityId, true));
            return parameters;
        }

        /// <summary>
        /// Inserts the LockerZones record to the database
        /// </summary>
        /// <param name="lockerZonesDTO">LockerZonesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public LockerZonesDTO InsertLockerZones(LockerZonesDTO lockerZonesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerZonesDTO, loginId, siteId);
            string insertLockerZonesQuery = @"INSERT INTO[dbo].[LockerZones]  
                                                        ( 
                                                          ZoneName,
                                                          ParentZoneId,
                                                          ZoneCode,
                                                          ActiveFlag,
                                                          LastUpdatedBy, 
                                                          LastUpdatedDate,
                                                          site_id,
                                                          Guid,
                                                          MasterEntityId,
                                                          LockerMode,
                                                          LockerMake,
                                                          CreatedBy,
                                                          CreationDate
                                                        ) 
                                                values 
                                                        (    
                                                          @ZoneName,
                                                          @ParentZoneId,
                                                          @ZoneCode,
                                                          @ActiveFlag, 
                                                          @lastUpdatedBy,
                                                          GetDate(), 
                                                          @site_id,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @lockerMode,
                                                          @lockerMake,
                                                          @createdBy,
                                                          getdate()
                                                        )SELECT * FROM LockerZones WHERE ZoneId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertLockerZonesQuery, GetSQLParameters(lockerZonesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerZonesDTO(lockerZonesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LockerZoneDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerZonesDTO);
            return lockerZonesDTO;
        }

        /// <summary>
        /// Updates the UpdateLockerZones record
        /// </summary>
        /// <param name="lockerZonesDTO">LockerZonesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public LockerZonesDTO UpdateLockerZones(LockerZonesDTO lockerZonesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerZonesDTO, loginId, siteId);
            string query = @"UPDATE LockerZones 
                             SET ZoneName=@ZoneName,
                                 ParentZoneId=@ParentZoneId,
                                 ZoneCode=@ZoneCode,
                                 ActiveFlag=@ActiveFlag,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdatedDate = GETDATE(),
                                 -- site_id=@site_id,
                                 MasterEntityId=@MasterEntityId,
                                 LockerMode = @lockerMode,
                                 LockerMake = @lockerMake
                             WHERE ZoneId = @ZoneId
                             SELECT * FROM LockerZones WHERE  ZoneId = @ZoneId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(lockerZonesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerZonesDTO(lockerZonesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LockerZoneDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerZonesDTO);
            return lockerZonesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="lockerZonesDTO">lockerZonesDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshLockerZonesDTO(LockerZonesDTO lockerZonesDTO, DataTable dt)
        {
            log.LogMethodEntry(lockerZonesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                lockerZonesDTO.ZoneId = Convert.ToInt32(dt.Rows[0]["ZoneId"]);
                lockerZonesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                lockerZonesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                lockerZonesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                lockerZonesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                lockerZonesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                lockerZonesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to LockerZoneDTO class type
        /// </summary>
        /// <param name="lockerZonesDataRow">LockerZones DataRow</param>
        /// <returns>Returns Lockers</returns>
        private LockerZonesDTO GetLockerZonesDTO(DataRow lockerZonesDataRow)
        {
            log.LogMethodEntry(lockerZonesDataRow);
            LockerZonesDTO lockerZonesDataObject = new LockerZonesDTO(Convert.ToInt32(lockerZonesDataRow["ZoneId"]),
                                            lockerZonesDataRow["ZoneName"].ToString(),
                                            lockerZonesDataRow["ParentZoneId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerZonesDataRow["ParentZoneId"]),
                                            lockerZonesDataRow["ZoneCode"] == DBNull.Value ? "0" : lockerZonesDataRow["ZoneCode"].ToString(),
                                            lockerZonesDataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(lockerZonesDataRow["ActiveFlag"]),
                                            lockerZonesDataRow["LastUpdatedBy"].ToString(),
                                            lockerZonesDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerZonesDataRow["LastupdatedDate"]),
                                            lockerZonesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(lockerZonesDataRow["site_id"]),
                                            lockerZonesDataRow["Guid"].ToString(),
                                            lockerZonesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(lockerZonesDataRow["SynchStatus"]),
                                            lockerZonesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerZonesDataRow["MasterEntityId"]),
                                            lockerZonesDataRow["LockerMode"].ToString(),
                                            lockerZonesDataRow["LockerMake"].ToString(),
                                            lockerZonesDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(lockerZonesDataRow["CreatedBy"]),
                                            lockerZonesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerZonesDataRow["CreationDate"])
                                            );
            log.LogMethodExit(lockerZonesDataObject);
            return lockerZonesDataObject;
        }

        /// <summary>
        /// Gets the lockerZones detail which matches with the passed Zone id 
        /// </summary>
        /// <param name="zoneId">integer type parameter</param>
        /// <returns>Returns LockerZonesDTO</returns>
        public bool ValidateProductZoneMapping(int zoneId)
        {
            log.LogMethodEntry(zoneId);
            string selectLockerZonesQuery = @"select convert(bit,1) from Products where ZoneId = @zoneId";
            SqlParameter[] selectLockerZonesParameters = new SqlParameter[1];
            selectLockerZonesParameters[0] = new SqlParameter("@ZoneId", zoneId);
            DataTable lockeZonesDataTable = dataAccessHandler.executeSelectQuery(selectLockerZonesQuery, selectLockerZonesParameters, sqlTransaction);
            log.LogMethodExit((lockeZonesDataTable.Rows.Count > 0));
            return (lockeZonesDataTable.Rows.Count > 0);
        }

        /// <summary>
        /// Gets the lockerZones detail which matches with the passed Zone id 
        /// </summary>
        /// <param name="ZoneId">integer type parameter</param>
        /// <returns>Returns LockerZonesDTO</returns>
        public LockerZonesDTO GetLockerZonesDTO(int ZoneId)
        {
            log.LogMethodEntry(ZoneId);
            LockerZonesDTO result = null;
            string selectLockerZonesQuery = SELECT_QUERY + @" WHERE ZoneId = @ZoneId and ActiveFlag = 1";
            SqlParameter parameter = new SqlParameter("@ZoneId", ZoneId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectLockerZonesQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetLockerZonesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the lockerZones detail of locker id 
        /// </summary>
        /// <param name="lockerId">integer type parameter</param>
        /// <returns>Returns LockerZonesDTO</returns>
        public LockerZonesDTO LoadLockerZonebyLockerId(int lockerId)
        {
            log.LogMethodEntry(lockerId);
            LockerZonesDTO lockerZonesDataObject = null;
            string selectLockerZonesQuery = @"select lz.* from Lockers lkr 
                                              inner join LockerPanels lp on lkr.PanelId = lp.PanelId  
                                              inner join LockerZones lz on lz.ZoneId = lp.ZoneId
                                              where lkr.ActiveFlag=1 and lkr.LockerId = @lockerId";
            SqlParameter[] selectLockerZonesParameters = new SqlParameter[1];
            selectLockerZonesParameters[0] = new SqlParameter("@lockerId", lockerId);
            DataTable lockeZonesDataTable = dataAccessHandler.executeSelectQuery(selectLockerZonesQuery, selectLockerZonesParameters, sqlTransaction);
            if (lockeZonesDataTable.Rows.Count > 0)
            {
                LockerPanelDataHandler lockerPanelDataHandler = new LockerPanelDataHandler(sqlTransaction);
                DataRow lockerZonesRow = lockeZonesDataTable.Rows[0];
                lockerZonesDataObject = GetLockerZonesDTO(lockerZonesRow);
            }
            log.LogMethodExit(lockerZonesDataObject, "lockerZonesDataObject");
            return lockerZonesDataObject;
        }

        /// <summary>
        /// Gets the lockerZoneslist detail which matches with the passed Zone id 
        /// </summary>
        /// <param name="ZoneId">integer type parameter</param>
        /// <returns>Returns LockerZonesDTO</returns>
        public DataTable GetLockerZonesList(int ZoneId)
        {
            log.LogMethodEntry(ZoneId);
            string selectLockerZonesQuery = @"select * 
                                              from getLockerZoneList(@zoneId)";
            SqlParameter[] selectLockerZonesListParameters = new SqlParameter[1];
            selectLockerZonesListParameters[0] = new SqlParameter("@ZoneId", ZoneId);
            DataTable lockeZonesListDataTable = dataAccessHandler.executeSelectQuery(selectLockerZonesQuery, selectLockerZonesListParameters, sqlTransaction);
            log.LogMethodExit(lockeZonesListDataTable);
            return lockeZonesListDataTable;
        }

        /// <summary>
        /// Gets the lockerZoneslist detail which matches with the passed Zone id 
        /// </summary>
        /// <param name="identifierFrom">integer type parameter for locker identifier</param>
        /// <param name="identifierTo">integer type parameter for locker identifier</param>
        /// <param name="zoneCode">integer type parameter for Zone Code</param>
        /// <param name="loadInactiveChildRecord">Load inactive child records</param>        
        /// <returns>Returns List of LockerZonesDTO</returns>
        public List<LockerZonesDTO> GetLockerZonesList(string zoneCode, int identifierFrom, int identifierTo, bool loadInactiveChildRecord)
        {
            log.LogMethodEntry(zoneCode, identifierFrom, identifierTo, loadInactiveChildRecord);
            string selectLockerZonesQuery = @"select distinct lz.* from LockerZones lz, LockerPanels lp, Lockers l 
                                                                   where lz.ZoneCode = @zonecode and lz.ZoneId = lp.ZoneId and lp.PanelId=l.PanelId " + ((loadInactiveChildRecord) ? "" : " and lz.ActiveFlag = 1 ") +
                                                                          "and l.Identifier >= @identifierFrom and l.Identifier <= @identifierTo";
            SqlParameter[] selectLockerZonesListParameters = new SqlParameter[3];
            selectLockerZonesListParameters[0] = new SqlParameter("@zonecode", zoneCode);
            selectLockerZonesListParameters[1] = new SqlParameter("@identifierFrom", identifierFrom);
            selectLockerZonesListParameters[2] = new SqlParameter("@identifierTo", identifierTo);
            DataTable lockeZonesListDataTable = dataAccessHandler.executeSelectQuery(selectLockerZonesQuery, selectLockerZonesListParameters, sqlTransaction);
            List<LockerZonesDTO> lockerGetZonesList = new List<LockerZonesDTO>();
            if (lockeZonesListDataTable.Rows.Count > 0)
            {
                LockerPanelDataHandler lockerPanelDataHandler = new LockerPanelDataHandler(sqlTransaction);
                foreach (DataRow getZonesDataRow in lockeZonesListDataTable.Rows)
                {
                    LockerZonesDTO lockerZoneDataObject = GetLockerZonesDTO(getZonesDataRow);
                    log.Debug("GetLockerAccessPointList(searchParameters) Loading panel for zone Id:" + lockerZoneDataObject.ZoneId);
                    lockerZoneDataObject.LockerPanelDTOList = lockerPanelDataHandler.GetLockerPanelsByZoneId(lockerZoneDataObject.ZoneId, identifierFrom, identifierTo, loadInactiveChildRecord);
                    lockerGetZonesList.Add(lockerZoneDataObject);
                }
            }
            log.LogMethodExit(lockerGetZonesList);
            return lockerGetZonesList;
        }

        /// <summary>
        /// Gets the Parent Zone value List
        /// </summary>
        /// <returns>Returns DataTable</returns>
        public List<LockerZonesDTO> GetParentZones(int site_id, bool loadChildEntity, bool loadInactiveChildRecord)
        {
            log.LogMethodEntry(site_id, loadChildEntity, loadInactiveChildRecord);
            string selectParentZonesQuery = @"select *
                                                        from LockerZones
                                                        where (site_id = @site_id or @site_id = -1)  order by 2";
            SqlParameter[] selectParentZonesParameters = new SqlParameter[1];
            selectParentZonesParameters[0] = new SqlParameter("@site_id", site_id);
            DataTable parentZonesDataTable = dataAccessHandler.executeSelectQuery(selectParentZonesQuery, selectParentZonesParameters, sqlTransaction);
            List<LockerZonesDTO> lockerParentZonesList = new List<LockerZonesDTO>();
            if (parentZonesDataTable.Rows.Count > 0)
            {
                LockerPanelDataHandler lockerPanelDataHandler = new LockerPanelDataHandler(sqlTransaction);
                foreach (DataRow parentZonesDataRow in parentZonesDataTable.Rows)
                {
                    LockerZonesDTO lockerParentDataObject = GetLockerZonesDTO(parentZonesDataRow);
                    if (loadChildEntity)
                    {
                        lockerParentDataObject.LockerPanelDTOList = lockerPanelDataHandler.GetLockerPanelsByZoneId(lockerParentDataObject.ZoneId, -1, -1, loadInactiveChildRecord);
                    }
                    lockerParentZonesList.Add(lockerParentDataObject);
                }
            }
            log.Debug(lockerParentZonesList);
            return lockerParentZonesList;
        }

        /// <summary>
        /// Gets the Zones List value
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="loadChildEntity">loadChildEntity</param>
        /// <param name="loadInactiveChildRecord">loadInactiveChildRecord</param>
        /// <returns>Returns Datatable</returns>
        public List<LockerZonesDTO> GetZonesList(int site_id, bool loadChildEntity, bool loadInactiveChildRecord)
        {
            log.LogMethodEntry(site_id, loadChildEntity, loadInactiveChildRecord);
            string selectGetZonesQuery = @"select *
                                                from LockerZones lz1 
                                                where not exists 
                                                       (select 1 
                                                        from LockerZones lz2 
                                                        where lz2.ParentZoneId = lz1.ZoneId) 
                                                        order by ZoneName";
            SqlParameter[] selectGetZonesParameters = new SqlParameter[1];
            selectGetZonesParameters[0] = new SqlParameter("@site_id", site_id);
            DataTable getZonesDataTable = dataAccessHandler.executeSelectQuery(selectGetZonesQuery, selectGetZonesParameters, sqlTransaction);
            List<LockerZonesDTO> lockerGetZonesList = new List<LockerZonesDTO>();
            if (getZonesDataTable.Rows.Count > 0)
            {
                LockerPanelDataHandler lockerPanelDataHandler = new LockerPanelDataHandler(sqlTransaction);
                foreach (DataRow getZonesDataRow in getZonesDataTable.Rows)
                {
                    LockerZonesDTO getZonesDataObject = GetLockerZonesDTO(getZonesDataRow);
                    if (loadChildEntity)
                    {
                        getZonesDataObject.LockerPanelDTOList = lockerPanelDataHandler.GetLockerPanelsByZoneId(getZonesDataObject.ZoneId, -1, -1, loadInactiveChildRecord);
                    }
                    lockerGetZonesList.Add(getZonesDataObject);
                }
            }
            log.Debug(lockerGetZonesList);
            return lockerGetZonesList;
        }

        /// <summary>
        /// Gets the LockerZonesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="loadChildRecord">Load</param>
        /// <param name="loadInactiveChildRecord">Load inactive child records</param>
        /// <returns>Returns the list of LockerZonesDTO matching the search criteria</returns>
        public List<LockerZonesDTO> GetLockerZonesList(List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> searchParameters, bool loadChildRecord, bool loadInactiveChildRecord)
        {
            log.LogMethodEntry(searchParameters, loadChildRecord, loadInactiveChildRecord);
            List<LockerZonesDTO> lockerZonesDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LockerZonesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LockerZonesDTO.SearchByParameters.ZONE_ID
                            || searchParameter.Key == LockerZonesDTO.SearchByParameters.PARENT_ZONE_ID
                            || searchParameter.Key == LockerZonesDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == LockerZonesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerZonesDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));// ? 'Y' : 'N'));
                        }
                        else if (searchParameter.Key == LockerZonesDTO.SearchByParameters.ZONE_CODE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == LockerZonesDTO.SearchByParameters.LOCKER_MODE)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " is null or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " = '' )");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
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
                lockerZonesDTOList = new List<LockerZonesDTO>();
                LockerPanelDataHandler lockerPanelDataHandler = new LockerPanelDataHandler();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LockerZonesDTO lockerZonesDataObject = GetLockerZonesDTO(dataRow);
                    if (loadChildRecord)
                        lockerZonesDataObject.LockerPanelDTOList = lockerPanelDataHandler.GetLockerPanelsByZoneId(lockerZonesDataObject.ZoneId, -1, -1, loadInactiveChildRecord);
                    lockerZonesDTOList.Add(lockerZonesDataObject);
                }
            }
            log.LogMethodExit(lockerZonesDTOList);
            return lockerZonesDTOList;
        }
    }
}


