/********************************************************************************************
 * Project Name - Utilities
 * Description  - Get and Insert methods for machine transfer log details.
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        11-Mar-2019   Jagan Mohana          Created 
 *2.60.3      18-Jun-2019   Girish Kundar  Modified: Fix for the SQL Injection Issue 
 *2.70.2        29-Jul-2019   Deeksha        Modifications as per 3 tier standard.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Datahandler for MachineTransferLog
    /// </summary>
    public class MachineTransferLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM MachineTransferLog AS machineTranLog ";
        private static readonly Dictionary<MachineTransferLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MachineTransferLogDTO.SearchByParameters, string>
        {
            { MachineTransferLogDTO.SearchByParameters.MACHINE_ID,"machineTranLog.MachineId"},
            { MachineTransferLogDTO.SearchByParameters.FROM_SITE_ID, "machineTranLog.FromSiteId"},
            { MachineTransferLogDTO.SearchByParameters.TO_SITE_ID, "machineTranLog.ToSiteId"},
            { MachineTransferLogDTO.SearchByParameters.MASTER_ENTIY_ID, "machineTranLog.MasterentityId"}
        };

        /// <summary>
        /// Default constructor of MachineTransferLogDataHandler class
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public MachineTransferLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to MachineTransferLogDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow</param>
        /// <returns>machineTransferLogDTO</returns>
        private MachineTransferLogDTO GetMachineTransferLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MachineTransferLogDTO machineTransferLogDTO = new MachineTransferLogDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["MachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MachineId"]),
                                            dataRow["FromSiteId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FromSiteId"]),
                                            dataRow["ToSiteId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ToSiteId"]),
                                            dataRow["TransferDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["TransferDate"]),
                                            dataRow["TransferedBy"] == DBNull.Value ? string.Empty : dataRow["TransferedBy"].ToString(),
                                            dataRow["FromMachineStatus"] == DBNull.Value ? string.Empty : dataRow["FromMachineStatus"].ToString(),
                                            dataRow["ToMachineStatus"] == DBNull.Value ? string.Empty : dataRow["ToMachineStatus"].ToString(),
                                            dataRow["SourceMachineGUID"] == DBNull.Value ? string.Empty : dataRow["SourceMachineGUID"].ToString(),
                                            dataRow["TargetMachineGUID"] == DBNull.Value ? string.Empty : dataRow["TargetMachineGUID"].ToString(),
                                            dataRow["Remarks"] == DBNull.Value ? string.Empty : dataRow["Remarks"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),                                            
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ISActive"])
                                            );
            log.LogMethodExit(machineTransferLogDTO);
            return machineTransferLogDTO;
        }

        /// <summary>
        /// Gets the MachineTransferLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MachineTransferLogDTO matching the search criteria</returns>
        public List<MachineTransferLogDTO> GetMachineTransferLogs(List<KeyValuePair<MachineTransferLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<MachineTransferLogDTO> machineTransferLogDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MachineTransferLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        {
                            if (searchParameter.Key.Equals(MachineTransferLogDTO.SearchByParameters.MACHINE_ID) 
                                || searchParameter.Key.Equals(MachineTransferLogDTO.SearchByParameters.FROM_SITE_ID)
                                || searchParameter.Key.Equals(MachineTransferLogDTO.SearchByParameters.MASTER_ENTIY_ID)
                                || searchParameter.Key.Equals(MachineTransferLogDTO.SearchByParameters.TO_SITE_ID) )
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                           
                            else if (searchParameter.Key == MachineTransferLogDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),searchParameter.Value));
                            }
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
                    selectQuery = selectQuery + query;
            }

            DataTable companyData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (companyData.Rows.Count > 0)
            {
                machineTransferLogDTOList = new List<MachineTransferLogDTO>();
                foreach (DataRow machineTransferLogDataRow in companyData.Rows)
                {
                    MachineTransferLogDTO machineTransferLogDataObject = GetMachineTransferLogDTO(machineTransferLogDataRow);
                    machineTransferLogDTOList.Add(machineTransferLogDataObject);
                }
            }
                log.LogMethodExit(machineTransferLogDTOList);
                return machineTransferLogDTOList;       
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating machine transfer log Record.
        /// </summary>
        /// <param name="machineTransferLogDTO">MachineTransferLogDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MachineTransferLogDTO machineTransferLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(machineTransferLogDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", machineTransferLogDTO.MachineTransferLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MachineId", machineTransferLogDTO.MachineId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromSiteId", machineTransferLogDTO.FromSiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToSiteId", machineTransferLogDTO.ToSiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransferedBy", machineTransferLogDTO.TransferedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromMachineStatus", machineTransferLogDTO.FromMachineStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToMachineStatus", machineTransferLogDTO.ToMachineStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SourceMachineGUID", machineTransferLogDTO.SourceMachineGUID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TargetMachineGUID", machineTransferLogDTO.TargetMachineGUID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", machineTransferLogDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", machineTransferLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", machineTransferLogDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the machine transfer log record to the database
        /// </summary>
        /// <param name="machineTransferLogDTO">machineTransferLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MachineTransferLogDTO InsertMachineTransferLog(MachineTransferLogDTO machineTransferLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineTransferLogDTO, loginId, siteId);
            string query = @"insert into MachineTransferLog 
                                                        (                                                         
                                                        MachineId,
                                                        FromSiteId,
                                                        ToSiteId,
                                                        TransferDate,
                                                        TransferedBy,
                                                        FromMachineStatus,
                                                        ToMachineStatus,
                                                        SourceMachineGUID,
                                                        TargetMachineGUID,
                                                        Remarks,
                                                        site_id,
                                                        Guid,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        ISActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @MachineId,
                                                        @FromSiteId,
                                                        @ToSiteId,
                                                        GETDATE(),
                                                        @TransferedBy,
                                                        @FromMachineStatus,
                                                        @ToMachineStatus,
                                                        @SourceMachineGUID,
                                                        @TargetMachineGUID,
                                                        @Remarks,
                                                        @SiteId,
                                                        NewId(),
                                                        @MasterEntityId,
                                                        @CreatedBy,
                                                        GETDATE(),
                                                        @LastUpdatedBy,
                                                        GETDATE(),
                                                        @IsActive
                                            ) 
                                    SELECT * FROM MachineTransferLog WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(machineTransferLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineTransferLogDTO(machineTransferLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting machineTransferLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineTransferLogDTO);
            return machineTransferLogDTO;
        }

        /// <summary>
        /// Updates the machine transfer log record to the database
        /// </summary>
        /// <param name="machineTransferLogDTO">machineTransferLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MachineTransferLogDTO UpdateMachineTransferLog(MachineTransferLogDTO machineTransferLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineTransferLogDTO, loginId, siteId);
            string query = @"UPDATE  MachineTransferLog 
                                             SET                                                        
                                                        MachineId = @MachineId,
                                                        FromSiteId =  @FromSiteId,
                                                        ToSiteId = @ToSiteId,
                                                        TransferDate = GETDATE(),
                                                        TransferedBy = @TransferedBy,
                                                        FromMachineStatus =  @FromMachineStatus,
                                                        ToMachineStatus = @ToMachineStatus,
                                                        SourceMachineGUID = @SourceMachineGUID,
                                                        TargetMachineGUID = @TargetMachineGUID,
                                                        Remarks  = @Remarks,
                                                        site_id = @SiteId,
                                                        MasterEntityId = @MasterEntityId,
                                                        LastUpdatedBy = @LastUpdatedBy,
                                                        LastUpdateDate = GETDATE(),
                                                        ISActive = @IsActive
                                                        WHERE Id = @Id 
                                    SELECT * FROM MachineTransferLog WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(machineTransferLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineTransferLogDTO(machineTransferLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating machineTransferLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineTransferLogDTO);
            return machineTransferLogDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="machineTransferLogDTO">machineTransferLogDTO+ object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshMachineTransferLogDTO(MachineTransferLogDTO machineTransferLogDTO, DataTable dt)
        {
            log.LogMethodEntry(machineTransferLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                machineTransferLogDTO.MachineTransferLogId = Convert.ToInt32(dt.Rows[0]["Id"]);
                machineTransferLogDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                machineTransferLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                machineTransferLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                machineTransferLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                machineTransferLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                machineTransferLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
    }
}
