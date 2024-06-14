/********************************************************************************************
 * Project Name - MachineCommunicationLogDataHandler
 * Description  - Data handler of MachineCommunicationLog Class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************
 *1.00        04-Mar-2019   Indhu          Created 
 *2.60.3      18-Jun-2019   Girish Kundar  Modified: Fix for the SQL Injection Issue 
 *2.70.2        29-Jul-2019   Deeksha        Modified: Added refresh() method
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Game
{
    public class MachineCommunicationLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM MachinesCommunicationLog AS machinesComLog ";
        private static readonly Dictionary<MachineCommunicationLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MachineCommunicationLogDTO.SearchByParameters, string>
            {
                {MachineCommunicationLogDTO.SearchByParameters.MACHINE_ID, "machinesComLog.MachineId"},
                {MachineCommunicationLogDTO.SearchByParameters.MASTER_ENTITY_ID, "machinesComLog.MasterEntityId"},
                {MachineCommunicationLogDTO.SearchByParameters.SITE_ID, "machinesComLog.site_id"},
            };

        /// <summary>
        /// Default constructor of MachineCommunicationLogDataHandler class
        /// </summary>
        public MachineCommunicationLogDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of MachineCommunicationLogDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MachineCommunicationLogDataHandler(SqlTransaction sqlTransaction) : this()
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            //dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MachineCommunicationLog Record.
        /// </summary>
        /// <param name="machineCommunicationLogDTO">machineCommunicationLogDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MachineCommunicationLogDTO machineCommunicationLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineCommunicationLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@MachinesCommunicationLogId", machineCommunicationLogDTO.MachinesCommunicationLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MachineId", machineCommunicationLogDTO.MachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", machineCommunicationLogDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CommunicationSuccessRatio", machineCommunicationLogDTO.CommunicationSuccessRatio));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", machineCommunicationLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastServerCommunicatedTime", machineCommunicationLogDTO.LastServerCommunicatedTime));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Shift record to the database
        /// </summary>
        /// <param name="machineCommunicationLogDTO">MachineCommunicationLogDTO type object</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns inserted record id</returns>
        public MachineCommunicationLogDTO InsertMachineCommunicationLogDTO(MachineCommunicationLogDTO machineCommunicationLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineCommunicationLogDTO, loginId, siteId);
            string query = @"insert into MachinesCommunicationLog
                                        (
                                        MachineId,
                                        LastUpdateDate,
                                        LastUpdatedBy,
                                        IsActive,
                                        site_id,
                                        MasterEntityId,
                                        CreatedBy,
                                        CreationDate,
                                        CommunicationSuccessRatio,
                                        LastServerCommunicatedTime,
                                        Guid                       
                                        )
                                    values
                                        (
                                        @MachineId,
                                        GETDATE(),
                                        @LastUpdatedBy,
                                        @IsActive,
                                        @site_id,
                                        @MasterEntityId,
                                        @CreatedBy,
                                        GETDATE(),
                                        @CommunicationSuccessRatio,
                                        @LastServerCommunicatedTime,
                                        NEWID()
                                        )
                          SELECT * FROM MachinesCommunicationLog WHERE MachinesCommunicationLogId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(machineCommunicationLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineCommunicationLogDTO(machineCommunicationLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting machineCommunicationLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineCommunicationLogDTO);
            return machineCommunicationLogDTO;
        }

        /// <summary>
        /// Update the MachineCommunicationLog record to the database
        /// </summary>
        /// <param name="machineCommunicationLogDTO">MachineCommunicationLogDTO type object</param>
        /// <param name="loginId"></param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns inserted record id</returns>
        public MachineCommunicationLogDTO UpdateMachineCommunicationLogDTO(MachineCommunicationLogDTO machineCommunicationLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineCommunicationLogDTO, loginId, siteId);
            string query = @"update MachinesCommunicationLog set 
                                            MachineId = @MachineId,
                                            LastUpdateDate = getdate(),
                                            IsActive = @IsActive,
                                            site_id = @site_id,
                                            MasterEntityId = @masterEntityId,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            CommunicationSuccessRatio = @CommunicationSuccessRatio,
                                            LastServerCommunicatedTime = @LastServerCommunicatedTime
                                            where MachinesCommunicationLogId = @MachinesCommunicationLogId
                                SELECT * FROM MachinesCommunicationLog WHERE MachinesCommunicationLogId = @MachinesCommunicationLogId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(machineCommunicationLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineCommunicationLogDTO(machineCommunicationLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating machineCommunicationLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineCommunicationLogDTO);
            return machineCommunicationLogDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="MachineCommunicationLogDTO">MachineCommunicationLogDTO+ object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshMachineCommunicationLogDTO(MachineCommunicationLogDTO machineCommunicationLogDTO, DataTable dt)
        {
            log.LogMethodEntry(machineCommunicationLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                machineCommunicationLogDTO.MachinesCommunicationLogId = Convert.ToInt32(dt.Rows[0]["MachinesCommunicationLogId"]);
                machineCommunicationLogDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                machineCommunicationLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                machineCommunicationLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                machineCommunicationLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                machineCommunicationLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                machineCommunicationLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to MachineCommunicationLogDTO class type
        /// </summary>
        /// <param name="machineCommunicationLogDataRow">MachineCommunicationLogDTO DataRow</param>
        /// <returns>Returns MachineCommunicationLogDTO</returns>
        private MachineCommunicationLogDTO GetMachineCommunicationLogDTO(DataRow machineCommunicationLogDataRow)
        {
            log.LogMethodEntry(machineCommunicationLogDataRow);
            MachineCommunicationLogDTO machineCommunicationLogDataObject = new MachineCommunicationLogDTO(Convert.ToInt32(machineCommunicationLogDataRow["MachinesCommunicationLogId"]),
                                                    machineCommunicationLogDataRow["MachineId"] == DBNull.Value ? -1 : Convert.ToInt32(machineCommunicationLogDataRow["MachineId"]),
                                                    machineCommunicationLogDataRow["CommunicationSuccessRatio"] == DBNull.Value ? -1 : Convert.ToDouble(machineCommunicationLogDataRow["CommunicationSuccessRatio"]),
                                                    machineCommunicationLogDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(machineCommunicationLogDataRow["IsActive"]),
                                                    machineCommunicationLogDataRow["CreatedBy"] == DBNull.Value ? string.Empty : machineCommunicationLogDataRow["CreatedBy"].ToString(),
                                                    machineCommunicationLogDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineCommunicationLogDataRow["CreationDate"]),
                                                    machineCommunicationLogDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : machineCommunicationLogDataRow["LastUpdatedBy"].ToString(),
                                                    machineCommunicationLogDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineCommunicationLogDataRow["LastUpdateDate"]),
                                                    machineCommunicationLogDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(machineCommunicationLogDataRow["site_id"]),
                                                    machineCommunicationLogDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(machineCommunicationLogDataRow["MasterEntityId"]),
                                                    machineCommunicationLogDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(machineCommunicationLogDataRow["SynchStatus"]),
                                                    machineCommunicationLogDataRow["Guid"] == DBNull.Value ? "" : machineCommunicationLogDataRow["Guid"].ToString(),
                                                    machineCommunicationLogDataRow["LastServerCommunicatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineCommunicationLogDataRow["LastServerCommunicatedTime"])
                                                    );
            log.LogMethodExit(machineCommunicationLogDataObject);
            return machineCommunicationLogDataObject;
        }


        /// <summary>
        /// Gets the MachineCommunicationLog data of passed machineCommunicationId
        /// </summary>
        /// <param name="machineCommunicationLogId">integer type parameter</param>
        /// <returns>Returns MachineCommunicationLogDTO</returns>
        public MachineCommunicationLogDTO GetMachineCommunicationLogDTO(int idmachineCommunicationLogId)
        {
            log.LogMethodEntry(idmachineCommunicationLogId);
            MachineCommunicationLogDTO result = null;
            string selectQuery = SELECT_QUERY + " where machinesComLog.MachineCommunicationLogId = @MachinesCommunicationLogId";
            SqlParameter parameter = new SqlParameter("@MachinesCommunicationLogId", idmachineCommunicationLogId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMachineCommunicationLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Gets the MachineCommunicationLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MachineCommunicationLogDTO matching the search criteria</returns>
        public List<MachineCommunicationLogDTO> GetMachineCommunicationLogList(List<KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(searchParameters, sqlTrx);
            try
            {
                int count = 0;
                string selectQuery = SELECT_QUERY;
                List<MachineCommunicationLogDTO> machineCommunicationLogList = null;
                List<SqlParameter> parameters = new List<SqlParameter>();
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                if (searchParameters != null)
                {
                    foreach (KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            if (count == 0)
                            {
                                if (searchParameter.Key == MachineCommunicationLogDTO.SearchByParameters.MACHINE_ID
                                    || searchParameter.Key == MachineCommunicationLogDTO.SearchByParameters.MASTER_ENTITY_ID)
                                {
                                    query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                                }
                               else if (searchParameter.Key.Equals(MachineCommunicationLogDTO.SearchByParameters.SITE_ID))
                                {
                                    query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                                }
                                else
                                {
                                    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                }
                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    selectQuery = selectQuery + query;
                }
                DataTable machineCommunicationLogData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
                if (machineCommunicationLogData.Rows.Count > 0)
                {
                    machineCommunicationLogList = new List<MachineCommunicationLogDTO>();
                    foreach (DataRow machineCommunicationLogDataRow in machineCommunicationLogData.Rows)
                    {
                        MachineCommunicationLogDTO machineCommunicationLogDataObject = GetMachineCommunicationLogDTO(machineCommunicationLogDataRow);
                        machineCommunicationLogList.Add(machineCommunicationLogDataObject);
                    }
                }
                log.LogMethodExit(machineCommunicationLogList);
                return machineCommunicationLogList;


            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ;
            }
        }

    }
}
