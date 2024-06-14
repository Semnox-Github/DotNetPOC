/********************************************************************************************
 * Project Name - MachineInputDevices Data Handler                                                                          
 * Description  - Data handler of the MachineInputDevices class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      12-dec-2018    Guru S A      Who column changes
 *2.70.2        18-Jun-2019    Girish Kundar  Modified: Fix for the SQL Injection Issue 
 *2.70.2        29-Jul-2019    Deeksha        Modified: Added GetSqlParameter()
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Game
{
    public class MachineInputDevicesDataHandler
    {
        /// <summary>
        /// DataHandler for MachineInputDevices
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private List<SqlParameter> parameters = new List<SqlParameter>(); //added
        private const string SELECT_QUERY = @"SELECT * FROM MachineInputDevices AS machineInpDev ";

        /// <summary>
        /// Dictionary for searching Parameters for the MachineInputDevices object.
        /// </summary>
        private static readonly Dictionary<MachineInputDevicesDTO.SearchByParameters, string> InputDevicesDBSearchParameters = new Dictionary<MachineInputDevicesDTO.SearchByParameters, string>
            {
                {MachineInputDevicesDTO.SearchByParameters.DEVICE_ID, "machineInpDev.DeviceId"},
                {MachineInputDevicesDTO.SearchByParameters.DEVICE_NAME, "machineInpDev.DeviceName"},
                {MachineInputDevicesDTO.SearchByParameters.DEVICE_TYPE_ID, "machineInpDev.DeviceTypeId"},
                {MachineInputDevicesDTO.SearchByParameters.DEVICE_MODEL_ID, "machineInpDev.DeviceModelId"},
                {MachineInputDevicesDTO.SearchByParameters.MACHINE_ID, "machineInpDev.MachineId"},
                {MachineInputDevicesDTO.SearchByParameters.IP_ADDRESS, "machineInpDev.IPAddress"},
                {MachineInputDevicesDTO.SearchByParameters.MAC_ADDRESS, "machineInpDevMacAddress"},
                {MachineInputDevicesDTO.SearchByParameters.SITE_ID, "machineInpDev.site_id"},
                {MachineInputDevicesDTO.SearchByParameters.IS_ACTIVE, "machineInpDev.IsActive"},
                {MachineInputDevicesDTO.SearchByParameters.MASTER_ENTITY_ID, "machineInpDev.MasterEntityId"}
             };

        /// <summary>
        /// Default constructor of MachineInputDevicesDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MachineInputDevicesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating GamePlayInfo Record.
        /// </summary>
        /// <param name="machineInputDevicesDTO">machineInputDevicesDTO object</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MachineInputDevicesDTO machineInputDevicesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineInputDevicesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@deviceId", machineInputDevicesDTO.DeviceId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deviceModelId", machineInputDevicesDTO.DeviceModelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deviceTypeId", machineInputDevicesDTO.DeviceTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineId", machineInputDevicesDTO.MachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fPTemplateFormat", machineInputDevicesDTO.FPTemplateFormat, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deviceName", string.IsNullOrEmpty(machineInputDevicesDTO.DeviceName) ? DBNull.Value : (object)machineInputDevicesDTO.DeviceName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@iPAddress", string.IsNullOrEmpty(machineInputDevicesDTO.IPAddress) ? DBNull.Value : (object)machineInputDevicesDTO.IPAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@macAddress", string.IsNullOrEmpty(machineInputDevicesDTO.MacAddress) ? DBNull.Value : (object)machineInputDevicesDTO.MacAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", machineInputDevicesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@portNo", machineInputDevicesDTO.PortNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", machineInputDevicesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;

        }

        /// <summary>
        /// Inserts the MachineInputDevice record to the database
        /// </summary>
        /// <param name="machineInputDeviceDTO">MachineInputDevicesDTO type object</param>
        /// <param name="loginId">user id who updated the details</param>
        /// <param name="siteId">site id where its get updated</param>
        /// <returns>Returns inserted record id</returns>
        public MachineInputDevicesDTO InsertMachineInputDevice(MachineInputDevicesDTO machineInputDeviceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineInputDeviceDTO, loginId, siteId);
            string insertMachineInputDeviceDTOQuery = @"insert into MachineInputDevices
                                                         (
                                                            DeviceName,
                                                            DeviceTypeId,
                                                            DeviceModelId,
                                                            MachineId,
                                                            IsActive,
                                                            IPAddress,
                                                            PortNo,
                                                            MacAddress,
                                                            FPTemplateFormat,
                                                            site_id,
                                                            Guid,
                                                            LastUpdatedDate,
                                                            LastUpdatedBy,
                                                            CreationDate,
                                                            CreatedBy, 
                                                            MasterEntityId
                                                         )
                                                       values
                                                         (
                                                            @deviceName,
                                                            @deviceTypeId,
                                                            @deviceModelId,
                                                            @machineId,
                                                            @isActive,
                                                            @iPAddress,
                                                            @portNo,
                                                            @macAddress,
                                                            @fPTemplateFormat,
                                                            @site_id,
                                                            NewId(),
                                                            GetDate(),
                                                            @lastUpdatedBy,
                                                            GetDate(),
                                                            @createdBy, 
                                                            @masterEntityId ) 
                                    SELECT * FROM MachineInputDevices WHERE DeviceId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMachineInputDeviceDTOQuery, GetSQLParameters(machineInputDeviceDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineInputDeviceDTO(machineInputDeviceDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting machineInputDeviceDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineInputDeviceDTO);
            return machineInputDeviceDTO;
        }

        /// <summary>
        /// Updates the MachineInputDeviceDTO record in the database
        /// </summary>
        /// <param name="machineInputDeviceDTO">MachineInputDeviceDTO type object</param>
        /// <param name="loginId">user id who updated the details</param>
        /// <param name="siteId">site id where its get updated</param>
        /// <returns>Returns the count of updated rows</returns>
        public MachineInputDevicesDTO UpdateMachineInputDevice(MachineInputDevicesDTO machineInputDeviceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineInputDeviceDTO, loginId, siteId);
            string updateMachineInputDeviceDTOQuery = @"update MachineInputDevices
                                                       set
                                                            DeviceName = @deviceName,
                                                            DeviceTypeId = @deviceTypeId,
                                                            DeviceModelId = @deviceModelId,
                                                            MachineId = @machineId,
                                                            IsActive = @isActive,
                                                            IPAddress = @iPAddress,
                                                            PortNo = @portNo,
                                                            MacAddress = @macAddress,
                                                            FPTemplateFormat = @fPTemplateFormat,
                                                            site_id = @site_id,
                                                            LastUpdatedDate = GetDate(),
                                                            LastUpdatedBy = @lastUpdatedBy,
                                                            MasterEntityId = @masterEntityId
                                                Where DeviceId = @deviceId
                                             SELECT * FROM MachineInputDevices WHERE DeviceId = @deviceId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMachineInputDeviceDTOQuery, GetSQLParameters(machineInputDeviceDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineInputDeviceDTO(machineInputDeviceDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating machineInputDeviceDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineInputDeviceDTO);
            return machineInputDeviceDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="machineInputDeviceDTO">machineInputDeviceDTO+ object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshMachineInputDeviceDTO(MachineInputDevicesDTO machineInputDeviceDTO, DataTable dt)
        {
            log.LogMethodEntry(machineInputDeviceDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                machineInputDeviceDTO.DeviceId = Convert.ToInt32(dt.Rows[0]["DeviceId"]);
                machineInputDeviceDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                machineInputDeviceDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                machineInputDeviceDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                machineInputDeviceDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                machineInputDeviceDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                machineInputDeviceDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to MachineInputDevicesDTO class type
        /// </summary>
        /// <param name="machineInputDeviceDataRow">MachineInputDevicesDTO DataRow</param>
        /// <returns>Returns MachineInputDevicesDTO</returns>
        private MachineInputDevicesDTO GetMachineInputDevicesDTO(DataRow machineInputDeviceDataRow)
        {
            log.LogMethodEntry(machineInputDeviceDataRow);
            MachineInputDevicesDTO turnstileDTO = new MachineInputDevicesDTO(
                                Convert.ToInt32(
                                machineInputDeviceDataRow["DeviceId"]),
                                machineInputDeviceDataRow["DeviceName"] == DBNull.Value ? string.Empty : Convert.ToString(machineInputDeviceDataRow["DeviceName"]),
                                machineInputDeviceDataRow["DeviceTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(machineInputDeviceDataRow["DeviceTypeId"]),
                                machineInputDeviceDataRow["DeviceModelId"] == DBNull.Value ? -1 : Convert.ToInt32(machineInputDeviceDataRow["DeviceModelId"]),
                                machineInputDeviceDataRow["MachineId"] == DBNull.Value ? -1 : Convert.ToInt32(machineInputDeviceDataRow["MachineId"]),
                                machineInputDeviceDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(machineInputDeviceDataRow["IsActive"]),
                                machineInputDeviceDataRow["IPAddress"] == DBNull.Value ? string.Empty : Convert.ToString(machineInputDeviceDataRow["IPAddress"]),
                                machineInputDeviceDataRow["PortNo"] == DBNull.Value ? -1 : Convert.ToInt32(machineInputDeviceDataRow["PortNo"]),
                                machineInputDeviceDataRow["MacAddress"] == DBNull.Value ? string.Empty : Convert.ToString(machineInputDeviceDataRow["MacAddress"]),
                                machineInputDeviceDataRow["FPTemplateFormat"] == DBNull.Value ? -1 : Convert.ToInt32(machineInputDeviceDataRow["FPTemplateFormat"]),
                                machineInputDeviceDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(machineInputDeviceDataRow["site_id"]),
                                machineInputDeviceDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(machineInputDeviceDataRow["Guid"]),
                                machineInputDeviceDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(machineInputDeviceDataRow["SynchStatus"]),
                                machineInputDeviceDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineInputDeviceDataRow["LastupdatedDate"]),
                                machineInputDeviceDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(machineInputDeviceDataRow["LastUpdatedBy"]),
                                machineInputDeviceDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineInputDeviceDataRow["CreationDate"]),
                                machineInputDeviceDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(machineInputDeviceDataRow["CreatedBy"]),
                                machineInputDeviceDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(machineInputDeviceDataRow["MasterEntityId"])
                                );

            log.LogMethodExit(turnstileDTO);
            return turnstileDTO;
        }

        /// <summary>
        /// Gets the MachineInputDevicesDTO data of passed achievementClassId
        /// </summary>
        /// <param name="deviceId">integer type parameter</param>
        /// <returns>Returns MachineInputDevicesDTO</returns>
        public MachineInputDevicesDTO GetMachineInputDeviceDTO(int deviceId)
        {
            log.LogMethodEntry(deviceId);

            string selectMachineInputDeviceDTOQuery = SELECT_QUERY + "where machineInpDev.DeviceId = @deviceId";
            SqlParameter[] selectMachineInputDeviceDTOParameters = new SqlParameter[1];
            selectMachineInputDeviceDTOParameters[0] = new SqlParameter("@deviceId", deviceId);
            DataTable selectMachineInputDeviceDTO = dataAccessHandler.executeSelectQuery(selectMachineInputDeviceDTOQuery, selectMachineInputDeviceDTOParameters, sqlTransaction);
            MachineInputDevicesDTO turnstileDTO = new MachineInputDevicesDTO(); ;
            if (selectMachineInputDeviceDTO.Rows.Count > 0)
            {
                turnstileDTO = GetMachineInputDevicesDTO(selectMachineInputDeviceDTO.Rows[0]);
            }
            log.LogMethodExit(turnstileDTO);
            return turnstileDTO;
        }

        /// <summary>
        /// Delete the record from the MachineInputDevicesDTO database based on deviceId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(int deviceId)
        {
            log.LogMethodEntry(deviceId);
            try
            {
                string deleteMachineInputDevicesDTOQuery = @"delete  
                                              from MachineInputDevices
                                              where DeviceId = @deviceId";

                SqlParameter[] deleteMachineInputDevicesDTOParameters = new SqlParameter[1];
                deleteMachineInputDevicesDTOParameters[0] = new SqlParameter("@deviceId", deviceId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteMachineInputDevicesDTOQuery, deleteMachineInputDevicesDTOParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                log.Error(expn);
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// Gets the MachineInputDevicesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MachineInputDevicesDTO matching the search criteria</returns>
        public List<MachineInputDevicesDTO> GetMachineInputDevicesLists(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)//added
        {
            log.LogMethodEntry(searchParameters);
            List<MachineInputDevicesDTO> machineInputDevicesDTOList = new List<MachineInputDevicesDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters, sqlTransaction);
            if (currentPage >= 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY gp.DeviceId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                machineInputDevicesDTOList = new List<MachineInputDevicesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MachineInputDevicesDTO machineInputDevicesDTO = GetMachineInputDevicesDTO(dataRow);
                    machineInputDevicesDTOList.Add(machineInputDevicesDTO);
                }
            }
            log.LogMethodExit(machineInputDevicesDTOList);
            return machineInputDevicesDTOList;
        }

        /// <summary>
        /// Returns the no of MachineInputDevices matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetMachineInputDevicessCount(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            int machineInputDevicesDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters, sqlTransaction);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                machineInputDevicesDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(machineInputDevicesDTOCount);
            return machineInputDevicesDTOCount;
        }


        /// <summary>
        /// Gets the MachineInputDevicesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">SqlTransaction object</param>
        /// <returns>Returns the list of MachineInputDevicesDTO matching the search criteria</returns>
        public List<MachineInputDevicesDTO> GetMachineInputDevicesDTOsList(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null) //modified.
        {
            log.LogMethodEntry(searchParameters);
            List<MachineInputDevicesDTO> machineInputDevicesDTOList = null;
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters, sqlTransaction);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                machineInputDevicesDTOList = new List<MachineInputDevicesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MachineInputDevicesDTO machineInputDevicesDTO = GetMachineInputDevicesDTO(dataRow);
                    machineInputDevicesDTOList.Add(machineInputDevicesDTO);
                }
            }
            log.LogMethodExit(machineInputDevicesDTOList);
            return machineInputDevicesDTOList;
        }

        /// <summary>
        /// Gets the MachineInputDevicesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MachineInputDevicesDTO matching the search criteria</returns>
        public string GetFilterQuery(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            string joiner;
            StringBuilder query = new StringBuilder(" where ");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                foreach (KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (InputDevicesDBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(MachineInputDevicesDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + InputDevicesDBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MachineInputDevicesDTO.SearchByParameters.DEVICE_ID) ||
                                searchParameter.Key.Equals(MachineInputDevicesDTO.SearchByParameters.DEVICE_TYPE_ID) ||
                                searchParameter.Key.Equals(MachineInputDevicesDTO.SearchByParameters.DEVICE_MODEL_ID) ||
                                searchParameter.Key.Equals(MachineInputDevicesDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                searchParameter.Key.Equals(MachineInputDevicesDTO.SearchByParameters.MACHINE_ID))
                        {
                            query.Append(joiner + InputDevicesDBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MachineInputDevicesDTO.SearchByParameters.DEVICE_NAME) ||
                                   searchParameter.Key.Equals(MachineInputDevicesDTO.SearchByParameters.MAC_ADDRESS) ||
                                   searchParameter.Key.Equals(MachineInputDevicesDTO.SearchByParameters.IP_ADDRESS))
                        {
                            query.Append(joiner + InputDevicesDBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(MachineInputDevicesDTO.SearchByParameters.IS_ACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + InputDevicesDBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

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
    }
}
