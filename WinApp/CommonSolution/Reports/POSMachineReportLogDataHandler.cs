/********************************************************************************************
 * Project Name - Reports
 * Description  - Data Handler of POSMachineReportLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      13-June-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// POSMachineReportLogDataHandler handles  Insert, update  and Search for PosMachinereportLog objects.
    /// </summary>
    public class POSMachineReportLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PosMachinereportLog AS pmrl ";
        /// <summary>
        /// Dictionary for searching Parameters for the POSMachineReportLog object.
        /// </summary>
        private static readonly Dictionary<POSMachineReportLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<POSMachineReportLogDTO.SearchByParameters, string>
        {
            { POSMachineReportLogDTO.SearchByParameters.ID,"pmrl.Id"},
            { POSMachineReportLogDTO.SearchByParameters.ACTIVE_FLAG,"pmrl.IsActive"},
            { POSMachineReportLogDTO.SearchByParameters.POS_MACHINE_NAME,"pmrl.POSMachineName"},
            { POSMachineReportLogDTO.SearchByParameters.REPORT_ID,"pmrl.Reportid"},
            { POSMachineReportLogDTO.SearchByParameters.REPORT_SEQUENCE_NO,"pmrl.ReportSequenceNo"},
            { POSMachineReportLogDTO.SearchByParameters.SITE_ID,"pmrl.site_id"},
            { POSMachineReportLogDTO.SearchByParameters.MASTER_ENTITY_ID,"pmrl.MasterEntityId"},
            { POSMachineReportLogDTO.SearchByParameters.START_TIME,"pmrl.StartTime"},
            { POSMachineReportLogDTO.SearchByParameters.END_TIME,"pmrl.EndTime"}
        };

        /// <summary>
        /// Parameterized Constructor for POSMachineReportLogDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public POSMachineReportLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating posMachineReportLog Record.
        /// </summary>
        /// <param name="posMachineReportLogDTO">POSMachineReportLogDTO object is passed</param>
        /// <param name="loginId"> login id of user</param>
        /// <param name="siteId">site id of user</param>
        /// <returns>returns SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(POSMachineReportLogDTO posMachineReportLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posMachineReportLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", posMachineReportLogDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", posMachineReportLogDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EndTime", posMachineReportLogDTO.EndTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineName", posMachineReportLogDTO.POSMachineName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Reportid", posMachineReportLogDTO.ReportId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReportSequenceNo", posMachineReportLogDTO.ReportSequenceNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartTime", posMachineReportLogDTO.StartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", posMachineReportLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to CommunicationLogDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the CommunicationLogDTO</returns>
        private POSMachineReportLogDTO GetPOSMachineReportLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            POSMachineReportLogDTO posMachineReportLogDTO = new POSMachineReportLogDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                         dataRow["POSMachineName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["POSMachineName"]),
                                                         dataRow["Reportid"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Reportid"]),
                                                         dataRow["StartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["StartTime"]),
                                                         dataRow["EndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EndTime"]),
                                                         dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]), // to be checked if null true or false
                                                         dataRow["ReportSequenceNo"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ReportSequenceNo"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(posMachineReportLogDTO);
            return posMachineReportLogDTO;
        }

        /// <summary>
        /// Gets the POSMachineReportLogDTO data of passed Id 
        /// </summary>
        /// <param name="id">id of POSMachineReportLog is passed as Parameter</param>
        /// <returns>returns POSMachineReportLogDTO object</returns>
        public POSMachineReportLogDTO GetPOSMachineReportLogDTO(int id)
        {
            log.LogMethodEntry(id);
            POSMachineReportLogDTO result = null;
            string query = SELECT_QUERY + @" WHERE pmrl.Id = @id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPOSMachineReportLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the POSMachineReportLog record.
        /// </summary>
        /// <param name="posMachineReportLogDTO">POSMachineReportLogDTO is passed</param>
        internal void Delete(POSMachineReportLogDTO posMachineReportLogDTO)
        {
            log.LogMethodEntry(posMachineReportLogDTO);
            string query = @"DELETE  
                             FROM PosMachinereportLog
                             WHERE PosMachinereportLog.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", posMachineReportLogDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            posMachineReportLogDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the record to the POSMachineReportLogDTO Table.
        /// </summary>
        /// <param name="posMachineReportLogDTO">posMachineReportLogDTO object is passed</param>
        /// <param name="loginId"> login id of user</param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the POSMachineReportLogDTO</returns>
        public POSMachineReportLogDTO Insert(POSMachineReportLogDTO posMachineReportLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posMachineReportLogDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[PosMachinereportLog]
                           ([POSMachineName],
                            [Reportid],
                            [StartTime],
                            [EndTime],
                            [IsActive],
                            [CreatedBy],
                            [CreationDate],
                            [LastUpdatedBy],
                            [LastupdatedDate],
                            [Guid],
                            [site_id],
                            [MasterEntityId],
                            [ReportSequenceNo])
                     VALUES
                           (@POSMachineName,
                            @Reportid,
                            @StartTime,
                            @EndTime,
                            @IsActive,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @ReportSequenceNo)
                            SELECT * FROM PosMachinereportLog WHERE Id = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(posMachineReportLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSMachineReportLogDTO(posMachineReportLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting POSMachineReportLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(posMachineReportLogDTO);
            return posMachineReportLogDTO;
        }

        /// <summary>
        /// Updates the record to the POSMachineReportLogDTO Table.
        /// </summary>
        /// <param name="posMachineReportLogDTO">posMachineReportLogDTO object is passed</param>
        /// <param name="loginId"> login id of user</param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the POSMachineReportLogDTO</returns>
        public POSMachineReportLogDTO Update(POSMachineReportLogDTO posMachineReportLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posMachineReportLogDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[PosMachinereportLog]
                           SET
                            [POSMachineName]  = @POSMachineName,
                            [Reportid]        = @Reportid,
                            [StartTime]       = @StartTime,
                            [EndTime]         = @EndTime,
                            [IsActive]        = @IsActive,
                            [LastUpdatedBy]   = @LastUpdatedBy,
                            [LastupdatedDate] = GETDATE(),
                            [MasterEntityId]  = @MasterEntityId,
                            [ReportSequenceNo]= @ReportSequenceNo
                            WHERE Id = @Id
                            SELECT * FROM PosMachinereportLog WHERE Id = @Id";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(posMachineReportLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSMachineReportLogDTO(posMachineReportLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Update POSMachineReportLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(posMachineReportLogDTO);
            return posMachineReportLogDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="posMachineReportLogDTO">POSMachineReportLogDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        private void RefreshPOSMachineReportLogDTO(POSMachineReportLogDTO posMachineReportLogDTO, DataTable dt)
        {
            log.LogMethodEntry(posMachineReportLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                posMachineReportLogDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                posMachineReportLogDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                posMachineReportLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                posMachineReportLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                posMachineReportLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                posMachineReportLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                posMachineReportLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of POSMachineReportLogDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>returns the list of POSMachineReportLogDTO</returns>
        public List<POSMachineReportLogDTO> GetPOSMachineReportLogDTOList(List<KeyValuePair<POSMachineReportLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<POSMachineReportLogDTO> posMachineReportLogDTOList = new List<POSMachineReportLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<POSMachineReportLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == POSMachineReportLogDTO.SearchByParameters.ID
                            || searchParameter.Key == POSMachineReportLogDTO.SearchByParameters.REPORT_ID
                            || searchParameter.Key == POSMachineReportLogDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSMachineReportLogDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSMachineReportLogDTO.SearchByParameters.POS_MACHINE_NAME
                            || searchParameter.Key == POSMachineReportLogDTO.SearchByParameters.REPORT_SEQUENCE_NO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSMachineReportLogDTO.SearchByParameters.ACTIVE_FLAG)  // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == POSMachineReportLogDTO.SearchByParameters.START_TIME)  // datetime
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSMachineReportLogDTO.SearchByParameters.END_TIME)  // datetime
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                    POSMachineReportLogDTO posMachineReportLogDTO = GetPOSMachineReportLogDTO(dataRow);
                    posMachineReportLogDTOList.Add(posMachineReportLogDTO);
                }
            }
            log.LogMethodExit(posMachineReportLogDTOList);
            return posMachineReportLogDTOList;
        }
    }
}
