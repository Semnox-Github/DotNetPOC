/********************************************************************************************
 * Project Name - Locker Allocation Data Handler
 * Description  - The locker allocation data handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        18-Apr-2017   Raghuveera          Created 
 *2.70        18-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query     
  *2.100.0     31-Aug-2020   Mushahid Faizan      siteId changes in GetSQLParameters().
 *2.130.00    29-Jun-2021   Dakshakh raj       Modified as part of Metra locker integration 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Locker Allocation Data Handler - Handles insert, update and select of locker allocation  objects
    /// </summary>
    public class LockerAllocationDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM LockerAllocation AS la";

        /// <summary>
        /// Dictionary for searching Parameters for the Locker Allocation object.
        /// </summary>
        private static readonly Dictionary<LockerAllocationDTO.SearchByLockerAllocationParameters, string> DBSearchParameters = new Dictionary<LockerAllocationDTO.SearchByLockerAllocationParameters, string>
            {
                {LockerAllocationDTO.SearchByLockerAllocationParameters.ID, "la.Id"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.CARD_ID, "la.CardId"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.CARD_NUMBER, "la.CardNumber"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.LOCKER_ID, "la.LockerId"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.REFUNDED, "la.Refunded"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.TRX_ID, "la.TrxId"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.TRX_LINE_ID, "la.TrxLineId"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.VALID_FROM_TIME, "la.ValidFromTime"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.VALID_TO_TIME, "la.ValidToTime"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.VALID_BETWEEN_DATE,"Between la.ValidFromTime and la.ValidToTime"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.MASTER_ENTITY_ID,"la.MasterEntityId"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.SITE_ID, "la.site_id"},
                {LockerAllocationDTO.SearchByLockerAllocationParameters.LOCKER_RETURN_POLICY_LIMIT, "la.ValidToTime"}
            };

        /// <summary>
        /// Default constructor of LockerAllocationDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerAllocationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LockerAllocation parameters Record.
        /// </summary>
        /// <param name="lockerAllocationDTO">lockerAllocationDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(LockerAllocationDTO lockerAllocationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerAllocationDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", lockerAllocationDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardId", lockerAllocationDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardNumber", string.IsNullOrEmpty(lockerAllocationDTO.CardNumber) ? DBNull.Value : (object)lockerAllocationDTO.CardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lockerId", lockerAllocationDTO.LockerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@issuedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@validFromTime", lockerAllocationDTO.ValidFromTime == DateTime.MinValue ? DBNull.Value : (object)lockerAllocationDTO.ValidFromTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@validToTime", lockerAllocationDTO.ValidToTime == DateTime.MinValue ? DBNull.Value : (object)lockerAllocationDTO.ValidToTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@trxId", lockerAllocationDTO.TrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@trxLineId", lockerAllocationDTO.TrxLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@refunded", lockerAllocationDTO.Refunded));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", lockerAllocationDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@zoneCode", string.IsNullOrEmpty(lockerAllocationDTO.ZoneCode) ? DBNull.Value : (object)lockerAllocationDTO.ZoneCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            return parameters;
        }


        /// <summary>
        /// Inserts the locker allocation record to the database
        /// </summary>
        /// <param name="lockerAllocation">LockerAllocationDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">Sql transaction</param>
        /// <returns>Returns inserted record id</returns>
        public LockerAllocationDTO InsertLockerAllocation(LockerAllocationDTO lockerAllocation, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerAllocation, loginId, siteId, sqlTransaction);
            string insertLockerAllocationQuery = @"INSERT INTO [dbo].[LockerAllocation]  
                                                        ( 
                                                        CardId,
                                                        CardNumber,
                                                        LockerId,
                                                        ValidFromTime,
                                                        ValidToTime,
                                                        MasterEntityId,
                                                        Guid,
                                                        site_id,
                                                        IssuedBy,
                                                        IssuedTime,
                                                        LastUpdatedTime,
                                                        LastUpdatedBy,
                                                        TrxId,
                                                        TrxLineId,
                                                        Refunded,
                                                        ZoneCode,
                                                        CreatedBy,
                                                        CreationDate
                                                        ) 
                                                values 
                                                        (                                                         
                                                        @cardId,
                                                        @cardNumber,
                                                        @lockerId,
                                                        @validFromTime,
                                                        @validToTime,
                                                        @masterEntityId,
                                                        NEWID(),
                                                        @siteid,
                                                        @issuedBy,
                                                        Getdate(),
                                                        Getdate(),                                                         
                                                        @lastUpdatedBy,
                                                        @trxId,
                                                        @trxLineId,
                                                        @refunded,
                                                        @zoneCode,
                                                        @createdBy,
                                                        Getdate()
                                                        )SELECT * FROM LockerAllocation WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertLockerAllocationQuery, GetSQLParameters(lockerAllocation, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerAllocation(lockerAllocation, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting lockerAccessPoint", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerAllocation);
            return lockerAllocation;
        }


        /// <summary>
        /// Updates the locker allocation record
        /// </summary>
        /// <param name="lockerAllocation">LockerAllocationDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">Sql transaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public LockerAllocationDTO UpdateLockerAllocation(LockerAllocationDTO lockerAllocation, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerAllocation, loginId, siteId, sqlTransaction);
            string updateLockerAllocationQuery = @"UPDATE LockerAllocation 
                                         set CardId = @cardId,
                                             CardNumber = @cardNumber,
                                             LockerId = @lockerId,
                                             ValidFromTime = @validFromTime,
                                             ValidToTime = @validToTime,
                                             MasterEntityId = @masterEntityId,
                                             -- site_id = @siteId,
                                             LastUpdatedTime = getdate(),
                                             LastUpdatedBy = @lastUpdatedBy,
                                             TrxId = @trxId,
                                             TrxLineId = @trxLineId,
                                             Refunded = @refunded,
                                             ZoneCode = @zoneCode
                                       WHERE Id = @id 
                                       SELECT * FROM LockerAllocation WHERE  Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateLockerAllocationQuery, GetSQLParameters(lockerAllocation, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerAllocation(lockerAllocation, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LockerAccessPoint", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerAllocation);
            return lockerAllocation;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="lockerAllocationDTO">lockerAllocationDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshLockerAllocation(LockerAllocationDTO lockerAllocationDTO, DataTable dt)
        {
            log.LogMethodEntry(lockerAllocationDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                lockerAllocationDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                lockerAllocationDTO.LastUpdatedTime = dataRow["LastUpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedTime"]);
                lockerAllocationDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                lockerAllocationDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                lockerAllocationDTO.LastUpdatedUserId = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                lockerAllocationDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                lockerAllocationDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to LockerAllocationDTO class type
        /// </summary>
        /// <param name="lockerAllocationDataRow">LockerAllocation DataRow</param>
        /// <returns>Returns LockerAllocation</returns>
        private LockerAllocationDTO GetLockerAllocationDTO(DataRow lockerAllocationDataRow)
        {
            log.LogMethodEntry(lockerAllocationDataRow);
            LockerAllocationDTO lockerAllocationDataObject = new LockerAllocationDTO(Convert.ToInt32(lockerAllocationDataRow["Id"]),
                                            lockerAllocationDataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAllocationDataRow["CardId"]),
                                            lockerAllocationDataRow["CardNumber"].ToString(),
                                            lockerAllocationDataRow["LockerId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAllocationDataRow["LockerId"]),
                                            lockerAllocationDataRow["IssuedBy"].ToString(),
                                            lockerAllocationDataRow["IssuedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerAllocationDataRow["IssuedTime"]),
                                            lockerAllocationDataRow["ValidFromTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerAllocationDataRow["ValidFromTime"]),
                                            lockerAllocationDataRow["ValidToTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerAllocationDataRow["ValidToTime"]),
                                            lockerAllocationDataRow["Guid"].ToString(),
                                            lockerAllocationDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAllocationDataRow["site_id"]),
                                            lockerAllocationDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(lockerAllocationDataRow["SynchStatus"]),
                                            lockerAllocationDataRow["LastUpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerAllocationDataRow["LastUpdatedTime"]),
                                            lockerAllocationDataRow["LastUpdatedBy"].ToString(),
                                            lockerAllocationDataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAllocationDataRow["TrxId"]),
                                            lockerAllocationDataRow["TrxLineId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAllocationDataRow["TrxLineId"]),
                                            lockerAllocationDataRow["Refunded"] == DBNull.Value ? false : Convert.ToBoolean(lockerAllocationDataRow["Refunded"]),
                                            lockerAllocationDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerAllocationDataRow["MasterEntityId"]),
                                            lockerAllocationDataRow["ZoneCode"].ToString(),
                                            lockerAllocationDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(lockerAllocationDataRow["CreatedBy"]),
                                            lockerAllocationDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerAllocationDataRow["CreationDate"])
                                            );
            log.LogMethodExit(lockerAllocationDataObject);
            return lockerAllocationDataObject;
        }

        /// <summary>
        /// Gets the locker allocation record which matches the identifier 
        /// </summary>
        /// <param name="lockerId">integer type parameter</param>
        /// <returns>Returns LockerAllocationDTO</returns>
        public LockerAllocationDTO GetLockerAllocationOnLockerId(int lockerId)
        {
            log.LogMethodEntry(lockerId);
            LockerAllocationDTO result = null;
            string selectLockerAllocationQuery = SELECT_QUERY + @" WHERE la.LockerId = @lockerId 
                                                                and getdate() between ValidFromTime and ValidToTime
                                                                and Refunded = 0
                                                                order by IssuedTime desc";
            SqlParameter parameter = new SqlParameter("@lockerId", lockerId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectLockerAllocationQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetLockerAllocationDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the locker allocation record by zone list
        /// </summary>
        /// <param name="zoneList">string type which has zone id list</param>
        /// <returns>Returns LockerAllocationDTO</returns>
        public List<LockerAllocationDTO> GetLockerAllocationOnZoneList(string zoneList)
        {
            log.LogMethodEntry(zoneList);
            string selectLockerAllocationQuery = @"select la.* from LockerAllocation la
                                                     join Lockers l on la.LockerId = l.LockerId
                                                     join LockerPanels lp on l.PanelId= lp.PanelId
		                                           where  getdate() between ValidFromTime and ValidToTime
                                                        and Refunded = 0
		                                                and lp.ZoneId in" +"("+ zoneList +")"+
                                                        "order by IssuedTime desc";
            List<LockerAllocationDTO> lockerAllocationList = new List<LockerAllocationDTO>();
            DataTable lockerAllocation = dataAccessHandler.executeSelectQuery(selectLockerAllocationQuery, null, sqlTransaction);
            if (lockerAllocation.Rows.Count > 0)

            {
                foreach (DataRow lockerAllocationDataRow in lockerAllocation.Rows)
                {
                    LockerAllocationDTO lockerAllocationDataObject = GetLockerAllocationDTO(lockerAllocationDataRow);
                    lockerAllocationList.Add(lockerAllocationDataObject);
                }
            }
            log.LogMethodExit(lockerAllocationList);
            return lockerAllocationList;

        }

        /// <summary>
        /// Gets the locker allocation record which matches the CardNumber 
        /// </summary>
        /// <param name="cardnumber">string type parameter</param>
        /// <returns>Returns LockerAllocationDTO</returns>
        public LockerAllocationDTO GetLockerAllocationOnAPCardNumber(string cardnumber)
        {
            log.LogMethodEntry(cardnumber);
            LockerAllocationDTO lockerAllocationDataObject = null;
            string selectLockerAllocationQuery = @"SELECT top 1 * 
                                                   FROM  LockerAllocation 
                                                    WHERE cardnumber = @cardnumber                                                           
                                                     ORDER BY IssuedTime DESC";
            SqlParameter[] selectLockerAllocationParameters = new SqlParameter[1];
            selectLockerAllocationParameters[0] = new SqlParameter("@cardnumber", cardnumber);
            DataTable lockerAllocation = dataAccessHandler.executeSelectQuery(selectLockerAllocationQuery, selectLockerAllocationParameters, sqlTransaction);
            if (lockerAllocation.Rows.Count > 0)
            {
                DataRow lockerAllocationRow = lockerAllocation.Rows[0];
                lockerAllocationDataObject = GetLockerAllocationDTO(lockerAllocationRow);
            }
            log.LogMethodExit(lockerAllocationDataObject);
            return lockerAllocationDataObject;
        }

        /// <summary>
        /// Gets the locker allocation record which matches the identifier 
        /// </summary>
        /// <param name="lockerNumber">integer type parameter</param>
        /// <returns>Returns LockerAllocationDTO</returns>
        public LockerAllocationDTO GetLockerAllocationOnAPLockerNumber(int lockerNumber)
        {
            log.LogMethodEntry(lockerNumber);
            LockerAllocationDTO lockerAllocationDataObject = null;
            string selectLockerAllocationQuery = @"SELECT la.* 
                                                   FROM Lockers l, LockerAllocation la
                                                    WHERE l.LockerId = la.lockerId
                                                           and l.Identifier = @identifire                                                           
                                                     ORDER BY IssuedTime DESC";
            SqlParameter[] selectLockerAllocationParameters = new SqlParameter[1];
            selectLockerAllocationParameters[0] = new SqlParameter("@identifire", lockerNumber);
            DataTable lockerAllocation = dataAccessHandler.executeSelectQuery(selectLockerAllocationQuery, selectLockerAllocationParameters, sqlTransaction);
            if (lockerAllocation.Rows.Count > 0)
            {
                DataRow lockerAllocationRow = lockerAllocation.Rows[0];
                lockerAllocationDataObject = GetLockerAllocationDTO(lockerAllocationRow);
            }
            log.LogMethodExit(lockerAllocationDataObject);
            return lockerAllocationDataObject;
        }

        /// <summary>
        /// Gets the locker allocation record which matches the locker allocation id  
        /// </summary>
        /// <param name="lockerAllocationId">integer type parameter</param>
        /// <returns>Returns LockerAllocationDTO</returns>
        public LockerAllocationDTO GetLockerAllocation(int lockerAllocationId)
        {
            log.LogMethodEntry(lockerAllocationId);
            LockerAllocationDTO lockerAllocationDataObject = null;
            string selectLockerAllocationQuery = SELECT_QUERY + @" WHERE la.Id = @lockerAllocationId
                                                   ORDER BY IssuedTime DESC";
            SqlParameter[] selectLockerAllocationParameters = new SqlParameter[1];
            selectLockerAllocationParameters[0] = new SqlParameter("@lockerAllocationId", lockerAllocationId);
            DataTable lockerAllocation = dataAccessHandler.executeSelectQuery(selectLockerAllocationQuery, selectLockerAllocationParameters, sqlTransaction);
            if (lockerAllocation.Rows.Count > 0)
            {
                DataRow lockerAllocationRow = lockerAllocation.Rows[0];
                lockerAllocationDataObject = GetLockerAllocationDTO(lockerAllocationRow);
            }
            log.LogMethodExit(lockerAllocationDataObject);
            return lockerAllocationDataObject;
        }

        /// <summary>
        /// Gets the locker allocation record which matches the locker card id  
        /// </summary>
        /// <param name="cardId">integer type parameter</param>
        /// <returns>Returns LockerAllocationDTO</returns>
        public LockerAllocationDTO GetLockerAllocationOnCardId(int cardId)
        {
            log.LogMethodEntry(cardId);
            LockerAllocationDTO lockerAllocationDataObject = null;
            string selectLockerAllocationQuery = @"SELECT *
                                                   FROM LockerAllocation
                                                   WHERE cardId = @cardId
                                                         and getdate() between ValidFromTime and ValidToTime 
                                                         and Refunded = 0
                                                   ORDER BY IssuedTime DESC";
            SqlParameter[] selectLockerAllocationParameters = new SqlParameter[1];
            selectLockerAllocationParameters[0] = new SqlParameter("@cardId", cardId);
            DataTable lockerAllocation = dataAccessHandler.executeSelectQuery(selectLockerAllocationQuery, selectLockerAllocationParameters, sqlTransaction);
            if (lockerAllocation.Rows.Count > 0)
            {
                DataRow lockerAllocationRow = lockerAllocation.Rows[0];
                lockerAllocationDataObject = GetLockerAllocationDTO(lockerAllocationRow);
            }
            log.LogMethodExit(lockerAllocationDataObject);
            return lockerAllocationDataObject;
        }

        /// <summary>
        /// Gets the LockerAllocationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LockerAllocationDTO matching the search criteria</returns>
        public List<LockerAllocationDTO> GetLockerAllocationList(List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<LockerAllocationDTO> lockerAllocationDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.ID
                            || searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.CARD_ID
                            || searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.LOCKER_ID
                            || searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.TRX_ID
                            || searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.TRX_LINE_ID
                            || searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.REFUNDED
                            || searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.VALID_FROM_TIME)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.VALID_TO_TIME)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.LOCKER_RETURN_POLICY_LIMIT)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == LockerAllocationDTO.SearchByLockerAllocationParameters.VALID_BETWEEN_DATE)
                        {
                            query.Append(joiner + dataAccessHandler.GetParameterName(searchParameter.Key) +" "+ DBSearchParameters[searchParameter.Key]);
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                selectQuery = selectQuery + query + " Order by IssuedTime desc";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                lockerAllocationDTOList = new List<LockerAllocationDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LockerAllocationDTO lockerAllocationDTO = GetLockerAllocationDTO(dataRow);
                    lockerAllocationDTOList.Add(lockerAllocationDTO);
                }
            }
            log.LogMethodExit(lockerAllocationDTOList);
            return lockerAllocationDTOList;
        }
    }
}
