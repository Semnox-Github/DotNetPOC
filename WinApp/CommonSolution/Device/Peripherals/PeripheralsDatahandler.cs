/********************************************************************************************
 * Project Name - Device
 * Description  - Data Handler - PeripheralsDatahandler
 * 
 **************
 **Version Log
 **************
 *Version       Date            Modified By         Remarks          
 *********************************************************************************************
 *2.60         02-Mar-2019      Akshay Gulaganji    modified posMachineId parameter In UpdatePeripheralsDTO() method
 *2.70.2        01-Jul-2019   Girish Kundar       Modified : For SQL Injection Issue.  
 *                                                         Added missed Columns to Insert/Update
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query 
 *2.80          09-Mar-2020      Vikas Dwivedi      Modified as per the standards for Phase 1 changes.
 *2.110.0       07-Dec-2020     Mathew Ninan        Added support for two new fields 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Linq;


namespace Semnox.Parafait.Device.Peripherals
{
    public class PeripheralsDatahandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM Peripherals AS p ";

        /// <summary>
        /// Dictionary for searching Parameters for the Peripherals object.
        /// </summary>
        private static readonly Dictionary<PeripheralsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PeripheralsDTO.SearchByParameters, string>
            {
                {PeripheralsDTO.SearchByParameters.DEVICE_ID, "p.DeviceId"},
                {PeripheralsDTO.SearchByParameters.DEVICE_NAME, "p.DeviceName"},
                {PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, "p.POSMachineId"},
                {PeripheralsDTO.SearchByParameters.POS_MACHINE_ID_LIST, "p.POSMachineId"},
                {PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "p.DeviceType"},
                {PeripheralsDTO.SearchByParameters.DEVICE_SUBTYPE, "p.DeviceSubType"},
                {PeripheralsDTO.SearchByParameters.VID, "p.VID"},
                {PeripheralsDTO.SearchByParameters.PID, "p.PID"},
                {PeripheralsDTO.SearchByParameters.SITE_ID, "p.site_id"},
                {PeripheralsDTO.SearchByParameters.GUID, "p.Guid"},
                {PeripheralsDTO.SearchByParameters.ACTIVE, "p.Active"},
                {PeripheralsDTO.SearchByParameters.MASTER_ENTITY_ID, "p.MasterEntityId"},
                {PeripheralsDTO.SearchByParameters.ENABLE_TAG_ENCRYPTION, "p.EnableTagDecryption"}
             };

        /// <summary>
        /// Parameterized Constructor of PeripheralsDataHandler
        /// </summary>
        public PeripheralsDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Peripheral Record.
        /// </summary>
        /// <param name="PeripheralsDTO">PeripheralsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>SqlParameter</returns>
        private List<SqlParameter> GetSQLParameters(PeripheralsDTO peripheralsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(peripheralsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeviceId", peripheralsDTO.DeviceId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posMachineId", peripheralsDTO.PosMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", peripheralsDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deviceName", string.IsNullOrEmpty(peripheralsDTO.DeviceName) ? DBNull.Value : (object)peripheralsDTO.DeviceName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deviceType", string.IsNullOrEmpty(peripheralsDTO.DeviceType) ? DBNull.Value : (object)peripheralsDTO.DeviceType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deviceSubType", string.IsNullOrEmpty(peripheralsDTO.DeviceSubType) ? DBNull.Value : (object)peripheralsDTO.DeviceSubType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@vid", string.IsNullOrEmpty(peripheralsDTO.Vid) ? DBNull.Value : (object)peripheralsDTO.Vid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pid", string.IsNullOrEmpty(peripheralsDTO.Pid) ? DBNull.Value : (object)peripheralsDTO.Pid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@optionalString", string.IsNullOrEmpty(peripheralsDTO.OptionalString) ? DBNull.Value : (object)peripheralsDTO.OptionalString));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", peripheralsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@enableTagDecryption", peripheralsDTO.EnableTagDecryption));
            parameters.Add(dataAccessHandler.GetSQLParameter("@excludeDecryptionForTagLength", peripheralsDTO.ExcludeDecryptionForTagLength));
            parameters.Add(dataAccessHandler.GetSQLParameter("@readerIsForRechargeOnly", peripheralsDTO.ReaderIsForRechargeOnly));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Peripherals record to the database
        /// </summary>
        /// <param name="peripheralsDTO">peripheralsDTO </param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId"siteId></param>
        ///<returns>Returns PeripheralsDTO</returns>
        public PeripheralsDTO InsertPeripheralsDTO(PeripheralsDTO peripheralsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(peripheralsDTO, loginId, siteId);
            string query = @"insert into Peripherals
                                                            ( 
                                                            DeviceName,
                                                            POSMachineId,
                                                            DeviceType,
                                                            DeviceSubType,
                                                            VID,
                                                            PID,
                                                            OptionalString,
                                                            site_id,
                                                            Guid,
                                                            LastUpdatedDate,
                                                            LastUpdatedUser,
                                                            Active,
                                                            MasterEntityId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            EnableTagDecryption,
                                                            ExcludeDecryptionForTagLength,
                                                            ReaderIsForRechargeOnly
                                                         )
                                                       values
                                                         ( 
                                                            @deviceName,
                                                            @posMachineId,
                                                            @deviceType,
                                                            @deviceSubType,
                                                            @vid,
                                                            @pid,
                                                            @optionalString,
                                                            @siteId,
                                                            NewId(), 
                                                            GETDATE(),
                                                            @lastUpdatedUser,
                                                            @active,
                                                            @masterEntityId,
                                                            @CreatedBy,
                                                            GETDATE(),
                                                            @enableTagDecryption,
                                                            @excludeDecryptionForTagLength,
                                                            @readerIsForRechargeOnly
                                                          ) SELECT * FROM Peripherals WHERE DeviceId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(peripheralsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPeripheralsDTO(peripheralsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting peripheralsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(peripheralsDTO);
            return peripheralsDTO;
        }

        /// <summary>
        /// Updates the Peripherals record to the database
        /// </summary>
        /// <param name="peripheralsDTO">peripheralsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns PeripheralsDTO</returns>
        public PeripheralsDTO UpdatePeripheralsDTO(PeripheralsDTO peripheralsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(peripheralsDTO, loginId, siteId);
            string query = @"update Peripherals
                                                         set
                                                            DeviceName = @deviceName,
                                                            POSMachineId = @posMachineId,
                                                            DeviceType = @deviceType,
                                                            DeviceSubType = @deviceSubType,
                                                            VID = @vid,
                                                            PID = @pid,
                                                            OptionalString = @optionalString,
                                                            --site_id = @siteId,                                                       
                                                            LastUpdatedDate = GETDATE() ,
                                                            LastUpdatedUser = @lastUpdatedUser,
                                                            Active = @active,
                                                            MasterEntityId = @masterEntityId,
                                                            EnableTagDecryption = @enableTagDecryption,
                                                            ExcludeDecryptionForTagLength = @excludeDecryptionForTagLength,
                                                            ReaderIsForRechargeOnly = @readerIsForRechargeOnly
                                                            where   DeviceId = @DeviceId
                                                            SELECT * FROM Peripherals WHERE DeviceId = @DeviceId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(peripheralsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPeripheralsDTO(peripheralsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating peripheralsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(peripheralsDTO);
            return peripheralsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="PeripheralsDTO">PeripheralsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPeripheralsDTO(PeripheralsDTO peripheralsDTO, DataTable dt)
        {
            log.LogMethodEntry(peripheralsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                peripheralsDTO.DeviceId = Convert.ToInt32(dt.Rows[0]["DeviceId"]);
                peripheralsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                peripheralsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                peripheralsDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                peripheralsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                peripheralsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                peripheralsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to PeripheralsDTO class type
        /// </summary>
        /// <param name="PeripheralsDTODataRow"></param>
        /// <returns>Returns PeripheralsDTO</returns>
        private PeripheralsDTO GetPeripheralsDTO(DataRow PeripheralsDTODataRow)
        {
            log.LogMethodEntry(PeripheralsDTODataRow);
            PeripheralsDTO PeripheralsDTO = new PeripheralsDTO(
                                 Convert.ToInt32(PeripheralsDTODataRow["DeviceId"]),
                                 PeripheralsDTODataRow["DeviceName"] == DBNull.Value ? string.Empty : Convert.ToString(PeripheralsDTODataRow["DeviceName"]),
                                 PeripheralsDTODataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(PeripheralsDTODataRow["POSMachineId"]),
                                 PeripheralsDTODataRow["DeviceType"] == DBNull.Value ? string.Empty : Convert.ToString(PeripheralsDTODataRow["DeviceType"]),
                                 PeripheralsDTODataRow["DeviceSubType"] == DBNull.Value ? string.Empty : Convert.ToString(PeripheralsDTODataRow["DeviceSubType"]),
                                 PeripheralsDTODataRow["VID"] == DBNull.Value ? string.Empty : Convert.ToString(PeripheralsDTODataRow["VID"]),
                                 PeripheralsDTODataRow["PID"] == DBNull.Value ? string.Empty : Convert.ToString(PeripheralsDTODataRow["PID"]),
                                 PeripheralsDTODataRow["OptionalString"] == DBNull.Value ? string.Empty : Convert.ToString(PeripheralsDTODataRow["OptionalString"]),
                                 PeripheralsDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(PeripheralsDTODataRow["site_id"]),
                                 PeripheralsDTODataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(PeripheralsDTODataRow["Guid"]),
                                 PeripheralsDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(PeripheralsDTODataRow["SynchStatus"]),
                                 PeripheralsDTODataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(PeripheralsDTODataRow["LastUpdatedDate"]),
                                 PeripheralsDTODataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(PeripheralsDTODataRow["LastUpdatedUser"]),
                                 PeripheralsDTODataRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(PeripheralsDTODataRow["Active"]),
                                 PeripheralsDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(PeripheralsDTODataRow["MasterEntityId"]),
                                 PeripheralsDTODataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(PeripheralsDTODataRow["CreatedBy"]),
                                 PeripheralsDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(PeripheralsDTODataRow["CreationDate"]),
                                 PeripheralsDTODataRow["EnableTagDecryption"] == DBNull.Value ? false : Convert.ToBoolean(PeripheralsDTODataRow["EnableTagDecryption"]),
                                 PeripheralsDTODataRow["ExcludeDecryptionForTagLength"] == DBNull.Value ? string.Empty : Convert.ToString(PeripheralsDTODataRow["ExcludeDecryptionForTagLength"]),
                                 PeripheralsDTODataRow["ReaderIsForRechargeOnly"] == DBNull.Value ? false : Convert.ToBoolean(PeripheralsDTODataRow["ReaderIsForRechargeOnly"])
                                 );
            log.LogMethodExit(PeripheralsDTO);
            return PeripheralsDTO;
        }


        /// <summary>
        /// Gets the PeripheralsDTO data of passed deviceId
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>Returns PeripheralsDTO</returns>
        public PeripheralsDTO GetPeripheralsDTO(int deviceId)
        {
            log.LogMethodEntry(deviceId);
            string selectPeripheralsDTOQuery = SELECT_QUERY + "  where p.DeviceId = @deviceId";
            SqlParameter[] selectPeripheralsDTOParameters = new SqlParameter[1];
            selectPeripheralsDTOParameters[0] = new SqlParameter("@deviceId", deviceId);
            DataTable selectedPeripheralsDTO = dataAccessHandler.executeSelectQuery(selectPeripheralsDTOQuery, selectPeripheralsDTOParameters, sqlTransaction);
            PeripheralsDTO PeripheralsDTO = new PeripheralsDTO();
            if (selectedPeripheralsDTO.Rows.Count > 0)
            {
                DataRow peripheralsRow = selectedPeripheralsDTO.Rows[0];
                PeripheralsDTO = GetPeripheralsDTO(peripheralsRow);
            }
            log.LogMethodExit(PeripheralsDTO);
            return PeripheralsDTO;
        }

        internal List<PeripheralsDTO> GetPeripheralsDTOList(List<int> pOSMachineIdList, bool activeRecords)
        {
            log.LogMethodEntry(pOSMachineIdList);
            List<PeripheralsDTO> peripheralsDTOList = new List<PeripheralsDTO>();
            string query = @"SELECT *
                            FROM Peripherals, @POSMachineIdList List
                            WHERE POSMachineId = List.Id ";
            if (activeRecords)
            {
                query += " AND Active = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@POSMachineIdList", pOSMachineIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                peripheralsDTOList = table.Rows.Cast<DataRow>().Select(x => GetPeripheralsDTO(x)).ToList();
            }
            log.LogMethodExit(peripheralsDTOList);
            return peripheralsDTOList;
        }


        /// <summary>
        /// Delete the record from the Peripherals database based on deviceId
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public int Delete(int deviceId)
        {
            log.LogMethodEntry(deviceId);
            string deletePeripheralsQuery = @"delete  
                                                from Peripherals
                                                where DeviceId = @deviceId";

            SqlParameter[] deletePeripheralsDTOParameters = new SqlParameter[1];
            deletePeripheralsDTOParameters[0] = new SqlParameter("@deviceId", deviceId);
            int deleteStatus = dataAccessHandler.executeUpdateQuery(deletePeripheralsQuery, deletePeripheralsDTOParameters, sqlTransaction);
            log.LogMethodExit(deleteStatus);
            return deleteStatus;
        }

        /// <summary>
        /// Gets the PeripheralsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>Returns the list of PeripheralsDTO matching the search criteria</returns>
        public List<PeripheralsDTO> GetPeripheralsDTOList(List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectPeripheralsDTOQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<PeripheralsDTO> PeripheralsDTOsList = new List<PeripheralsDTO>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PeripheralsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }

                        else if (searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.DEVICE_ID)
                            || searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.MASTER_ENTITY_ID)
                            || searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID)
                            || searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.DEVICE_NAME)
                           || searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.DEVICE_TYPE)
                           || searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.DEVICE_SUBTYPE)
                           || searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.VID)
                           || searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.PID)
                           || searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.GUID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.ACTIVE)
                                 || searchParameter.Key.Equals(PeripheralsDTO.SearchByParameters.ENABLE_TAG_ENCRYPTION))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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

                if (searchParameters.Count > 0)
                    selectPeripheralsDTOQuery = selectPeripheralsDTOQuery + query;
                selectPeripheralsDTOQuery = selectPeripheralsDTOQuery + " Order by DeviceId";
            }

            DataTable PeripheralsDTOsData = dataAccessHandler.executeSelectQuery(selectPeripheralsDTOQuery, parameters.ToArray(), sqlTransaction);
            if (PeripheralsDTOsData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in PeripheralsDTOsData.Rows)
                {
                    PeripheralsDTO PeripheralsDTOObject = GetPeripheralsDTO(dataRow);
                    PeripheralsDTOsList.Add(PeripheralsDTOObject);
                }
                log.LogMethodExit(PeripheralsDTOsList);
            }
            return PeripheralsDTOsList;
        }
    }
}
