/********************************************************************************************
 * Project Name - Locker Log Data Handler
 * Description  - Data handler of the Locker Log  Data Handler  class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        17-Jul-2017   Raghuveera          Created 
 *2.70.2        19-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.130.00      31-Aug-2018   Dakshakh raj        Modified as part of Metra locker integration
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Locker Log Data Handler - Handles insert, update and select of locker log data objects
    /// </summary>
    public class LockerLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM LockerLogs AS ll";

        /// <summary>
        /// Default constructor of LockerLogDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating lockerLog parameters Record.
        /// </summary>
        /// <param name="lockerLogDTO">lockerLogDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(LockerLogDTO lockerLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@logId", lockerLogDTO.LogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lockerId", lockerLogDTO.LockerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@source", string.IsNullOrEmpty(lockerLogDTO.Source) ? DBNull.Value : (object)lockerLogDTO.Source));
            parameters.Add(dataAccessHandler.GetSQLParameter("@logType", string.IsNullOrEmpty(lockerLogDTO.LogType) ? DBNull.Value : (object)lockerLogDTO.LogType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(lockerLogDTO.Description) ? DBNull.Value : (object)lockerLogDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", string.IsNullOrEmpty(lockerLogDTO.Status) ? DBNull.Value : (object)lockerLogDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", lockerLogDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", lockerLogDTO.Siteid, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", lockerLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            return parameters;
        }

        /// <summary>
        /// Inserts the lockerLog record to the database
        /// </summary>
        /// <param name="lockerLog">LockerLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public LockerLogDTO InsertLockerLog(LockerLogDTO lockerLog, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerLog, loginId, siteId);
            string insertLockerLogQuery = @"INSERT INTO[dbo].[LockerLogs]  
                                                        ( 
                                                          TimeStamp,
                                                          LockerId,
                                                          Source,
                                                          LogType,
                                                          Description,
                                                          Status,
                                                          IsActive,
                                                          CreatedBy,
                                                          CreationDate,
                                                          Guid,
                                                          site_id,
                                                          MasterEntityId,
                                                          LastUpdatedBy,
                                                          LastUpdateDate
                                                        ) 
                                                values 
                                                        (    
                                                          getdate(),
                                                          @lockerId,
                                                          @source,
                                                          @logType,
                                                          @description,
                                                          @status,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),                                                     
                                                          Newid(),
                                                          @siteid,
                                                          @masterEntityId,
                                                          @lastUpdatedBy,
                                                          Getdate()                                                 
                                                        )SELECT * FROM LockerLogs WHERE LogId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertLockerLogQuery, GetSQLParameters(lockerLog, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerLog(lockerLog, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LockerLog", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerLog);
            return lockerLog;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="lockerLogDTO">lockerLogDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshLockerLog(LockerLogDTO lockerLogDTO , DataTable dt)
        {
            log.LogMethodEntry(lockerLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                lockerLogDTO.LogId = Convert.ToInt32(dt.Rows[0]["LogId"]);
                lockerLogDTO.LastupdateDate = dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]);
                lockerLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                lockerLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                lockerLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                lockerLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                lockerLogDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to LockerLogDTO class type
        /// </summary>
        /// <param name="lockerLogDataRow">LockerLogDTO DataRow</param>
        /// <returns>Returns LockerLogDTO</returns>
        private LockerLogDTO GetLockerLogDTO(DataRow lockerLogDataRow)
        {
            log.LogMethodEntry(lockerLogDataRow);
            LockerLogDTO lockerLogDataObject = new LockerLogDTO(Convert.ToInt32(lockerLogDataRow["logId"]),
                                            lockerLogDataRow["TimeStamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerLogDataRow["TimeStamp"]),
                                            lockerLogDataRow["LockerId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerLogDataRow["LockerId"]),
                                            lockerLogDataRow["Source"].ToString(),
                                            lockerLogDataRow["logType"].ToString(),
                                            lockerLogDataRow["Description"].ToString(),
                                            lockerLogDataRow["Status"].ToString(),                                            
                                            lockerLogDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(lockerLogDataRow["IsActive"]),
                                            lockerLogDataRow["CreatedBy"].Equals(string.Empty) ? "Semnox" : lockerLogDataRow["CreatedBy"].ToString(),
                                            lockerLogDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerLogDataRow["CreationDate"]),
                                            lockerLogDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(lockerLogDataRow["site_id"]),
                                            lockerLogDataRow["Guid"].ToString(),                                            
                                            lockerLogDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(lockerLogDataRow["SynchStatus"]),
                                            lockerLogDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerLogDataRow["MasterEntityId"]),//Modification on 18-Jul-2016 for publish feature
                                            lockerLogDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerLogDataRow["LastUpdatedTime"]),
                                            lockerLogDataRow["LastUpdatedBy"].ToString()
                                            );
            log.LogMethodExit(lockerLogDataObject);
            return lockerLogDataObject;
        }

        /// <summary>
        /// Clearing the log less than the passed date
        /// </summary>
        /// <param name="upToDate">The log will be deleted less than the passed date</param>
        /// <param name="exclusionSourceList">exclusionSourceList</param>
        public void ClearLockerLog(DateTime upToDate, List<string> exclusionSourceList)
        {
            log.LogMethodEntry(upToDate, exclusionSourceList);
            string exclusionSourceString="";
            if(exclusionSourceList!=null)
            {
                foreach(string s in exclusionSourceList)
                {
                    exclusionSourceString += "'" + s + "', ";
                }
                if(!string.IsNullOrEmpty(exclusionSourceString))
                exclusionSourceString+= "'" + -1 + "'";
            }
            string selectLockerLogQuery = @"Delete
                                            from LockerLogs
                                        where TimeStamp < @upToDate"+(string.IsNullOrEmpty(exclusionSourceString) ?"": " and Source not in("+ exclusionSourceString + ")");
            SqlParameter[] selectLockerLogParameters = new SqlParameter[1];
            selectLockerLogParameters[0] = new SqlParameter("@upToDate", upToDate);
            object lockerLog = dataAccessHandler.executeScalar(selectLockerLogQuery, selectLockerLogParameters,sqlTransaction);
            log.LogMethodExit();             
        }

        /// <summary>
        /// Gets the lockerLog data of passed lockerLog Id
        /// </summary>
        /// <param name="accessPointId">integer type parameter</param>
        /// <returns>Returns LockerLogDTO</returns>
        public LockerLogDTO GetLockerLog(int accessPointId)
        {
            log.LogMethodEntry(accessPointId);
            LockerLogDTO result = null;
            string selectLockerLogQuery = SELECT_QUERY + @" WHERE ll.LogId = @accessPointId";
            SqlParameter parameter = new SqlParameter("@accessPointId", accessPointId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectLockerLogQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetLockerLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }



        /// <summary>
        /// Gets the LockerLogDTO list matching the search key
        /// </summary>
        /// <returns>Returns the list of LockerLogDTO matching the search criteria</returns>
        public List<LockerLogDTO> GetLockerLogList()
        {
            log.LogMethodExit();
            string selectLockerLogQuery = SELECT_QUERY;
            List<LockerLogDTO> lockerLogList = null;
            DataTable lockerLogData = dataAccessHandler.executeSelectQuery(selectLockerLogQuery, null);
            if (lockerLogData.Rows.Count > 0)
            {
                lockerLogList = new List<LockerLogDTO>();
                foreach (DataRow lockerLogDataRow in lockerLogData.Rows)
                {
                    LockerLogDTO lockerLogDataObject = GetLockerLogDTO(lockerLogDataRow);
                    lockerLogList.Add(lockerLogDataObject);
                }
            }
            log.LogMethodExit(lockerLogList);
            return lockerLogList;
        }
    }
}
