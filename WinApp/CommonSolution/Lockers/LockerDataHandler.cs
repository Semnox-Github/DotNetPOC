/********************************************************************************************
 * Project Name - Lockers Data Handler
 * Description  - Data handler of the locker class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Apr-2017   Raghuveera     Created 
 *2.40.1      12-07-2018    Raghuveera     Innovate locker to block the locker card added field cardOverrideSequence
 *2.80        19-Jul-2019   Dakshakh raj   Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.80        19-Dec-2019   Lakshminarayana  Modified signage locker layout enhancement
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query       
  *2.100.0     31-Aug-2020   Mushahid Faizan      siteId changes in GetSQLParameters().
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
    /// Lockers Data Handler - Handles insert, update and select of locker objects
    /// </summary>
    public class LockerDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Lockers";

        /// <summary>
        /// Dictionary for searching Parameters for the Locker object.
        /// </summary>
        private static readonly Dictionary<LockerDTO.SearchByLockersParameters, string> DBSearchParameters = new Dictionary<LockerDTO.SearchByLockersParameters, string>
            {
                {LockerDTO.SearchByLockersParameters.IDENTIFIRE, "l.Identifier"},
                {LockerDTO.SearchByLockersParameters.IS_DISABLED, "l.Disabled"},
                {LockerDTO.SearchByLockersParameters.IS_ACTIVE, "l.ActiveFlag"},
                {LockerDTO.SearchByLockersParameters.LOCKER_ID, "l.LockerId"},
                {LockerDTO.SearchByLockersParameters.LOCKER_NAME, "l.LockerName"},
                {LockerDTO.SearchByLockersParameters.PANEL_ID, "l.PanelId"},
                {LockerDTO.SearchByLockersParameters.MASTER_ENTITY_ID,"l.MasterEntityId"},
                {LockerDTO.SearchByLockersParameters.SITE_ID, "l.site_id"},
                {LockerDTO.SearchByLockersParameters.LOCKER_STATUS,"l.LockerStatus"},
                {LockerDTO.SearchByLockersParameters.BATTERY_STATUS, "l.BatteryStatus"},
                {LockerDTO.SearchByLockersParameters.Zone_ID_LIST, "lp.ZoneId"}
            };

        /// <summary>
        /// Default constructor of LockersDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Locker parameters Record.
        /// </summary>
        /// <param name="lockerDTO">lockerDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(LockerDTO lockerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(dataAccessHandler.GetSQLParameter("@lockerId", lockerDTO.LockerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lockerName", lockerDTO.LockerName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@panelId", lockerDTO.PanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rowIndex", lockerDTO.RowIndex == -1 ? DBNull.Value : (object)lockerDTO.RowIndex));
            parameters.Add(dataAccessHandler.GetSQLParameter("@columnIndex", lockerDTO.ColumnIndex == -1 ? DBNull.Value : (object)lockerDTO.ColumnIndex));
            parameters.Add(dataAccessHandler.GetSQLParameter("@identifier", lockerDTO.Identifier == -1 ? DBNull.Value : (object)lockerDTO.Identifier));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", lockerDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", lockerDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lockerStatus", string.IsNullOrEmpty(lockerDTO.LockerStatus) ? DBNull.Value : (object)lockerDTO.LockerStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@statusChangeDate", lockerDTO.StatusChangeDate == DateTime.MinValue ? DBNull.Value : (object)lockerDTO.StatusChangeDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@batteryStatus", lockerDTO.BatteryStatus == -1 ? DBNull.Value : (object)lockerDTO.BatteryStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@batteryStatusChangeDate", lockerDTO.BatteryStatusChangeDate == DateTime.MinValue ? DBNull.Value : (object)lockerDTO.BatteryStatusChangeDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardOverrideSequence", lockerDTO.CardOverrideSequence == -1 ? DBNull.Value : (object)lockerDTO.CardOverrideSequence));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PositionX", lockerDTO.PositionX));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PositionY", lockerDTO.PositionY));
            parameters.Add(dataAccessHandler.GetSQLParameter("@externalIdentifier", lockerDTO.ExternalIdentifier));
            return parameters;
        }

        /// <summary>
        /// Inserts the locker record to the database
        /// </summary>
        /// <param name="locker">LockerDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public LockerDTO InsertLocker(LockerDTO locker, string loginId, int siteId)
        {
            log.LogMethodEntry(locker, loginId, siteId);
            string insertLockerQuery = @"INSERT INTO [dbo].[Lockers] 
                                                        ( 
                                                          LockerName,
                                                          PanelId,
                                                          RowIndex,
                                                          ColumnIndex,
                                                          Identifier,
                                                          LockerStatus,
                                                          StatusChangeDate,
                                                          BatteryStatus,
                                                          BatteryStatusChangeDate,
                                                          ActiveFlag,
                                                          LastUpdatedBy, 
                                                          LastUpdatedDate,
                                                          Guid,
                                                          site_id,
                                                          MasterEntityId,
                                                          CardOverrideSequence,
                                                          CreatedBy,
                                                          CreationDate,
                                                          PositionX,
                                                          PositionY,
                                                          ExternalIdentifier
                                                        ) 
                                                values 
                                                        (    
                                                          @lockerName,
                                                          @panelId,
                                                          @rowIndex,
                                                          @columnIndex,
                                                          @identifier,
                                                          @lockerStatus,
                                                          @statusChangeDate,
                                                          @batteryStatus,
                                                          @batteryStatusChangeDate,
                                                          @isActive, 
                                                          @lastUpdatedBy,
                                                          GetDate(),                                                       
                                                          Newid(),
                                                          @siteid,
                                                          @masterEntityId,
                                                          @cardOverrideSequence,
                                                          @createdBy,
                                                          Getdate(),
                                                          @PositionX,
                                                          @PositionY,
                                                          @externalIdentifier
                                                        )SELECT * FROM Lockers WHERE LockerId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertLockerQuery, GetSQLParameters(locker, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerDTO(locker, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting RequisitionLinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(locker);
            return locker;
        }

        /// <summary>
        /// Updates the locker record
        /// </summary>
        /// <param name="locker">LockerDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public LockerDTO UpdateLocker(LockerDTO locker, string loginId, int siteId)
        {
            log.LogMethodEntry(locker, loginId, siteId);
            string updateLockerQuery = @"update Lockers 
                                         set LockerName = @lockerName,
                                             PanelId = @panelId,
                                             RowIndex = @rowIndex,
                                             ColumnIndex = @columnIndex,
                                             Identifier = @identifier,
                                             LockerStatus = @lockerStatus,
                                             StatusChangeDate = @statusChangeDate,
                                             BatteryStatus = @batteryStatus,
                                             BatteryStatusChangeDate = @batteryStatusChangeDate,
                                             ActiveFlag = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             site_id=@siteid,
                                             MasterEntityId=@masterEntityId,
                                             CardOverrideSequence = @cardOverrideSequence,
                                             PositionX = @PositionX,
                                             PositionY = @PositionY,
                                             ExternalIdentifier = @externalIdentifier
                                       where LockerId = @lockerId
                                       SELECT * FROM Lockers WHERE LockerId = @lockerId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateLockerQuery, GetSQLParameters(locker, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerDTO(locker, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LockerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(locker);
            return locker;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="lockerDTO">lockerDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshLockerDTO(LockerDTO lockerDTO, DataTable dt)
        {
            log.LogMethodEntry(lockerDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                lockerDTO.LockerId = Convert.ToInt32(dt.Rows[0]["LockerId"]);
                lockerDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                lockerDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                lockerDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                lockerDTO.LastUpdatedUserId = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                lockerDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                lockerDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to LockersDTO class type
        /// </summary>
        /// <param name="lockerDataRow">Lockers DataRow</param>
        /// <param name="loadAllocationDetail">Loads locker allocation details</param>
        /// <returns>Returns Lockers</returns>
        private LockerDTO GetLockerDTO(DataRow lockerDataRow, bool loadAllocationDetail = true)
        {
            log.LogMethodEntry(lockerDataRow, loadAllocationDetail);
            LockerDTO lockerDataObject = new LockerDTO(Convert.ToInt32(lockerDataRow["LockerId"]),
                                            lockerDataRow["LockerName"].ToString(),
                                            lockerDataRow["PanelId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerDataRow["PanelId"]),
                                            lockerDataRow["RowIndex"] == DBNull.Value ? -1 : Convert.ToInt32(lockerDataRow["RowIndex"]),
                                            lockerDataRow["ColumnIndex"] == DBNull.Value ? -1 : Convert.ToInt32(lockerDataRow["ColumnIndex"]),
                                            lockerDataRow["Identifier"] == DBNull.Value ? -1 : Convert.ToInt32(lockerDataRow["Identifier"]),
                                            lockerDataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(lockerDataRow["ActiveFlag"]),
                                            lockerDataRow["Disabled"] == DBNull.Value ? false : Convert.ToBoolean(lockerDataRow["Disabled"]),
                                            lockerDataRow["LastUpdatedBy"].ToString(),
                                            lockerDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerDataRow["LastupdatedDate"]),
                                            lockerDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(lockerDataRow["site_id"]),
                                            lockerDataRow["Guid"].ToString(),
                                            lockerDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(lockerDataRow["SynchStatus"]),
                                            lockerDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerDataRow["MasterEntityId"]),
                                            lockerDataRow["LockerStatus"].ToString(),
                                            lockerDataRow["StatusChangeDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerDataRow["StatusChangeDate"]),
                                            lockerDataRow["BatteryStatus"] == DBNull.Value ? -1 : Convert.ToInt32(lockerDataRow["BatteryStatus"]),
                                            lockerDataRow["BatteryStatusChangeDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerDataRow["BatteryStatusChangeDate"]),
                                            lockerDataRow["CardOverrideSequence"] == DBNull.Value ? -1 : Convert.ToInt32(lockerDataRow["CardOverrideSequence"]),
                                            lockerDataRow["CreatedBy"].ToString(),
                                            lockerDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerDataRow["CreationDate"]),
                                            lockerDataRow["PositionX"] == DBNull.Value ? (int?)null : Convert.ToInt32(lockerDataRow["PositionX"]),
                                            lockerDataRow["PositionY"] == DBNull.Value ? (int?)null : Convert.ToInt32(lockerDataRow["PositionY"]),
                                            lockerDataRow["ExternalIdentifier"] == DBNull.Value ? string.Empty : Convert.ToString(lockerDataRow["ExternalIdentifier"])
                                            );
            if (loadAllocationDetail)
            {
                LockerAllocationDataHandler lockerAllocationDataHandler = new LockerAllocationDataHandler();
                lockerDataObject.LockerAllocated = lockerAllocationDataHandler.GetLockerAllocationOnLockerId(lockerDataObject.LockerId);
            }
            log.LogMethodExit(lockerDataObject);
            return lockerDataObject;
        }

        /// <summary>
        /// Deletes the locker record
        /// </summary>
        /// <param name="panelId">int parameter</param>
        /// <param name="rows">int parameter</param>
        /// <param name="columns">int parameter</param>
        public void RemoveLockers(int panelId, int rows, int columns)
        {
            log.LogMethodEntry(panelId, rows, columns);
            string selectLockersQuery = @"Delete FROM Lockers
                                             WHERE PanelId = @panelId " + ((rows > -1) ? " and RowIndex > @rows" : "") + ((columns > -1) ? " and ColumnIndex > @colums" : "");
            SqlParameter[] selectLockersParameters = new SqlParameter[3];
            selectLockersParameters[0] = new SqlParameter("@panelId", panelId);
            selectLockersParameters[1] = new SqlParameter("@rows", rows);
            selectLockersParameters[2] = new SqlParameter("@colums", columns);
            dataAccessHandler.executeScalar(selectLockersQuery, selectLockersParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the locker detail which matches with the passed locker id 
        /// </summary>
        /// <param name="lockerId">integer type parameter</param>
        /// <returns>Returns LockersDTO</returns>
        public LockerDTO GetLocker(int lockerId)
        {
            log.LogMethodEntry(lockerId);
            LockerDTO lockerDataObject = null;
            string selectLockersQuery = SELECT_QUERY + @" WHERE LockerId = @lockerId and ActiveFlag = 1";
            SqlParameter[] selectLockersParameters = new SqlParameter[1];
            selectLockersParameters[0] = new SqlParameter("@lockerId", lockerId);
            DataTable lockerDataTable = dataAccessHandler.executeSelectQuery(selectLockersQuery, selectLockersParameters, sqlTransaction);
            if (lockerDataTable.Rows.Count > 0)
            {
                DataRow lockersRow = lockerDataTable.Rows[0];
                lockerDataObject = GetLockerDTO(lockersRow);
            }
            log.LogMethodExit(lockerDataObject);
            return lockerDataObject;
        }

        /// <summary>
        /// Gets the locker detail which matches with the passed locker id 
        /// </summary>
        /// <param name="lockerIdFrom">integer type parameter</param>
        /// <param name="lockerIdTo">integer type parameter</param>
        /// <param name="panelId">integer type parameter</param>
        /// <param name="loadInactiveChildRecord">Load inactive child record</param>
        /// <returns>Returns LockersDTO</returns>
        public List<LockerDTO> GetLockers(int lockerIdFrom, int lockerIdTo, int panelId = -1, bool loadInactiveChildRecord = false)
        {
            log.LogMethodEntry(lockerIdFrom, lockerIdTo, panelId, loadInactiveChildRecord);
            List<LockerDTO> lockerList = new List<LockerDTO>();
            string selectLockersQuery = @"SELECT *
                                              FROM Lockers
                                             WHERE (Identifier >= @lockerIdFrom or @lockerIdFrom = -1) and (Identifier <= @lockerIdTo or @lockerIdTo = -1) and (PanelId = @panelId or @panelId = -1 )" + ((loadInactiveChildRecord) ? "" : " and ActiveFlag = 1");
            SqlParameter[] selectLockersParameters = new SqlParameter[3];
            selectLockersParameters[0] = new SqlParameter("@lockerIdFrom", lockerIdFrom);
            selectLockersParameters[1] = new SqlParameter("@lockerIdTo", lockerIdTo);
            selectLockersParameters[2] = new SqlParameter("@panelId", panelId);
            DataTable lockerDataTable = dataAccessHandler.executeSelectQuery(selectLockersQuery, selectLockersParameters,sqlTransaction);
            if (lockerDataTable.Rows.Count > 0)
            {
                foreach (DataRow lockerDataRow in lockerDataTable.Rows)
                {
                    LockerDTO lockerDataObject = GetLockerDTO(lockerDataRow);
                    lockerList.Add(lockerDataObject);
                }
            }
            log.LogMethodExit(lockerList);
            return lockerList;
        }

        /// <summary>
        /// Gets the locker detail which matches with the passed locker id 
        /// </summary>
        /// <param name="lockerIdFrom">integer type parameter</param>
        /// <param name="lockerIdTo">integer type parameter</param>
        /// <param name="lockerStatus">string type parameter</param>
        /// <param name="batteryStatus">string type parameter</param>
        /// <returns>Returns LockersDTO</returns>
        public List<LockerDTO> GetLockersWithStatus(int lockerIdFrom, int lockerIdTo, string lockerStatus, string batteryStatus)
        {
            log.LogMethodEntry(lockerIdFrom, lockerIdTo, lockerStatus, batteryStatus);
            List<LockerDTO> lockerList = new List<LockerDTO>();
            List<SqlParameter> selectLockersParameters = new List<SqlParameter>();
            string selectLockersQuery = @"SELECT *
                                              FROM Lockers
                                             WHERE Identifier >= @lockerIdFrom and Identifier <= @lockerIdTo  and ActiveFlag = 1";
            if (!string.IsNullOrEmpty(lockerStatus))
            {
                selectLockersQuery += " and LockerStatus = @lockerStatus";
                selectLockersParameters.Add(new SqlParameter("@lockerStatus", lockerStatus));
            }
            if (!string.IsNullOrEmpty(batteryStatus))
            {
                selectLockersQuery += " and BatteryStatus = @batteryStatus";
                selectLockersParameters.Add(new SqlParameter("@batteryStatus", batteryStatus));
            }
            selectLockersParameters.Add(new SqlParameter("@lockerIdFrom", lockerIdFrom));
            selectLockersParameters.Add(new SqlParameter("@lockerIdTo", lockerIdTo));
            DataTable lockerDataTable = dataAccessHandler.executeSelectQuery(selectLockersQuery, selectLockersParameters.ToArray(), sqlTransaction);
            if (lockerDataTable.Rows.Count > 0)
            {
                foreach (DataRow lockerDataRow in lockerDataTable.Rows)
                {
                    LockerDTO lockerDataObject = GetLockerDTO(lockerDataRow);
                    lockerList.Add(lockerDataObject);
                }

                log.LogMethodExit(lockerList);
                return lockerList;
            }
            else
            {
                log.LogMethodExit(lockerList);
                return lockerList;
            }
        }

        /// <summary>
        /// Gets the locker details on identifire
        /// </summary>
        /// <param name="identifier">integer type parameter</param>
        /// <returns>Returns LockersDTO</returns>
        public LockerDTO GetLockerDetailsOnIdentifier(int identifier)
        {
            log.LogMethodEntry(identifier);
            LockerDTO lockerDataObject = null;
            string selectLockersQuery = @"SELECT l.*
                                          FROM Lockers l, LockerPanels lp 
                                          WHERE l.PanelId = lp.PanelId
                                                and l.Identifier = @id";
            SqlParameter[] selectLockersParameters = new SqlParameter[1];
            selectLockersParameters[0] = new SqlParameter("@id", identifier);
            DataTable lockerDataTable = dataAccessHandler.executeSelectQuery(selectLockersQuery, selectLockersParameters, sqlTransaction);
            if (lockerDataTable.Rows.Count > 0)
            {
                DataRow lockersRow = lockerDataTable.Rows[0];
                lockerDataObject = GetLockerDTO(lockersRow);
            }
            log.LogMethodExit(lockerDataObject);
            return lockerDataObject;
        }

        /// <summary>
        /// Gets the locker details on card id
        /// </summary>
        /// <param name="cardId">integer type parameter</param>
        /// <returns>Returns LockersDTO</returns>
        public LockerDTO GetLockerDetailsOnCardId(int cardId)
        {
            log.LogMethodEntry(cardId);
            LockerDTO lockerDataObject = null;
            string selectLockersQuery = @"SELECT l.*
                                          FROM LockerAllocation la, Lockers l
                                          WHERE cardId = @cardId
                                                and getdate() between la.ValidFromTime and la.ValidToTime 
                                                and la.Refunded = 0
                                                and l.LockerId = la.LockerId";
            SqlParameter[] selectLockersParameters = new SqlParameter[1];
            selectLockersParameters[0] = new SqlParameter("@cardId", cardId);
            DataTable locker = dataAccessHandler.executeSelectQuery(selectLockersQuery, selectLockersParameters, sqlTransaction);
            if (locker.Rows.Count > 0)
            {
                DataRow lockersRow = locker.Rows[0];
                lockerDataObject = GetLockerDTO(lockersRow);
            }
            log.LogMethodExit(lockerDataObject);
            return lockerDataObject;
        }

        /// <summary>
        /// Gets the LockersDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="loadChildRecord">Load child record</param>
        /// <returns>Returns the list of LockersDTO matching the search criteria</returns>
        public List<LockerDTO> GetLockersList(List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>> searchParameters, bool loadChildRecord)
        {
            log.LogMethodEntry(searchParameters, loadChildRecord);
            List<LockerDTO> lockerDTOList = null; 
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"select l.*
                                          from LockerPanels lp 
                                          join Lockers l on lp.PanelId = l.PanelId ";

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LockerDTO.SearchByLockersParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LockerDTO.SearchByLockersParameters.LOCKER_ID
                            || searchParameter.Key == LockerDTO.SearchByLockersParameters.PANEL_ID
                            || searchParameter.Key == LockerDTO.SearchByLockersParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == LockerDTO.SearchByLockersParameters.IDENTIFIRE
                            || searchParameter.Key == LockerDTO.SearchByLockersParameters.BATTERY_STATUS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerDTO.SearchByLockersParameters.LOCKER_STATUS)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == LockerDTO.SearchByLockersParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerDTO.SearchByLockersParameters.IS_ACTIVE
                                 || searchParameter.Key == LockerDTO.SearchByLockersParameters.IS_DISABLED)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == LockerDTO.SearchByLockersParameters.Zone_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                selectQuery = selectQuery + query + " order by lp.PanelName, l.Identifier ";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                lockerDTOList = new List<LockerDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LockerDTO lockerDTO = GetLockerDTO(dataRow, loadChildRecord);
                    lockerDTOList.Add(lockerDTO);
                }
            }
            log.LogMethodExit(lockerDTOList);
            return lockerDTOList;
        }
    }
}
