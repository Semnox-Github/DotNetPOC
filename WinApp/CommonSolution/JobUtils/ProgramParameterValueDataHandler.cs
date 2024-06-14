/********************************************************************************************
 * Project Name - ProgramParameterValue Datahandler
 * Description  - Datahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021       Deeksha             Created as part of AWS Concurrent Programs enhancements
 *2.155.1       13-Aug-2023       Guru S A            Modified for Chile fiscaliation
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.JobUtils
{
    public class ProgramParameterValueDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProgramParameterValue AS ppv ";

        private static readonly Dictionary<ProgramParameterValueDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProgramParameterValueDTO.SearchByParameters, string>
            {
                {ProgramParameterValueDTO.SearchByParameters.PARAMETER_ID, "ppv.ParameterId"},
                {ProgramParameterValueDTO.SearchByParameters.PROGRAM_ID, "ppv.ProgramId"},
                {ProgramParameterValueDTO.SearchByParameters.CONCURRENTPROGRAM_SCHEDULE_ID, "ppv.ConcurrentProgramScheduleId"},
                {ProgramParameterValueDTO.SearchByParameters.PROGRAM_PARAMETER_VALUE_ID, "ppv.ProgramParameterValueId"},
                {ProgramParameterValueDTO.SearchByParameters.IS_ACTIVE, "ppv.IsActive"},
                {ProgramParameterValueDTO.SearchByParameters.MASTER_ENTITY_ID, "ppv.MasterEntityId"},
                {ProgramParameterValueDTO.SearchByParameters.SITE_ID, "ppv.site_id"},
                {ProgramParameterValueDTO.SearchByParameters.PROGRAM_EXECUTABLE_NAME, "cp.ExecutableName"},
                {ProgramParameterValueDTO.SearchByParameters.PARAMETER_NAME, "cpp.ParameterName"},
                {ProgramParameterValueDTO.SearchByParameters.PROGRAM_REQUEST_IS_IN_WIP, "cp.ExecutableName"},
            };

        /// <summary>
        /// Parameterized Constructor for ProgramParameterValueDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public ProgramParameterValueDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get SQL Parameters
        /// </summary>
        /// <param name="ProgramParameterValueDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(ProgramParameterValueDTO programParameterValueDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(programParameterValueDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProgramParameterValueId", programParameterValueDTO.ProgramParameterValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConcurrentProgramScheduleId", programParameterValueDTO.ConcurrentProgramScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProgramId", programParameterValueDTO.ProgramId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParameterId", programParameterValueDTO.ParameterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParameterValue", programParameterValueDTO.ParameterValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", programParameterValueDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", programParameterValueDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastupdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private ProgramParameterValueDTO GetProgramParameterValueDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProgramParameterValueDTO programParameterValueDTO = new ProgramParameterValueDTO(
                dataRow["ProgramParameterValueId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProgramParameterValueId"]),
                dataRow["ConcurrentProgramScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ConcurrentProgramScheduleId"]),
                dataRow["ProgramId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProgramId"]),
                dataRow["ParameterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParameterId"]),
                dataRow["ParameterValue"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ParameterValue"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                );
            log.LogMethodExit(programParameterValueDTO);
            return programParameterValueDTO;
        }

        internal ProgramParameterValueDTO GetProgramParameterValueDTO(int programParameterValueId)
        {
            log.LogMethodEntry(programParameterValueId);
            ProgramParameterValueDTO result = null;
            string query = SELECT_QUERY + @" WHERE ppv.ProgramParameterValueId = @ProgramParameterValueId";
            SqlParameter parameter = new SqlParameter("@ProgramParameterValueId", programParameterValueId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProgramParameterValueDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        internal void Delete(int programParameterValueId)
        {
            log.LogMethodEntry(programParameterValueId);
            string query = @"DELETE  
                             FROM ProgramParameterValue
                             WHERE ProgramParameterValueId = @ProgramParameterValueId";
            SqlParameter parameter = new SqlParameter("@ProgramParameterValueId", programParameterValueId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        internal List<ProgramParameterValueDTO> GetProgramsScheduleDTOListOfPrograms(List<int> programScheduleIdList,
                                                                                           bool activeRecords)
        {
            log.LogMethodEntry(programScheduleIdList, activeRecords);
            List<ProgramParameterValueDTO> programParameterValueDTOList = new List<ProgramParameterValueDTO>();
            string query = SELECT_QUERY + @" , @programScheduleIdList List
                            WHERE ConcurrentProgramScheduleId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1'";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@programScheduleIdList", programScheduleIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                programParameterValueDTOList = table.Rows.Cast<DataRow>().Select(x => GetProgramParameterValueDTO(x)).ToList();
            }
            log.LogMethodExit(programParameterValueDTOList);
            return programParameterValueDTOList;
        }

        private void RefreshProgramParameterValueDTO(ProgramParameterValueDTO programParameterValueDTO, DataTable dt)
        {
            log.LogMethodEntry(programParameterValueDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                programParameterValueDTO.ProgramParameterValueId = Convert.ToInt32(dt.Rows[0]["ProgramParameterValueId"]);
                programParameterValueDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                programParameterValueDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                programParameterValueDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                programParameterValueDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                programParameterValueDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                programParameterValueDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal ProgramParameterValueDTO Insert(ProgramParameterValueDTO programParameterValueDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(programParameterValueDTO, loginId, siteId);
            string query = @"INSERT INTO ProgramParameterValue
                             (
                               ProgramId,
                               ParameterId,
                               ParameterValue,
                               IsActive,
                               CreatedBy,
                               CreationDate,
                               LastUpdatedBy,
                               LastUpdatedDate,
                               Guid,
                               site_id,
                               MasterEntityId,
                               ConcurrentProgramScheduleId
                            )                           
                               
                         VALUES
                             (
                                @ProgramId,
                                @ParameterId,
                                @ParameterValue,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                NEWID(), 
                                @SiteId,
                                @MasterEntityId,
                                @ConcurrentProgramScheduleId
                               )
                                SELECT * FROM ProgramParameterValue WHERE ProgramParameterValueId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(programParameterValueDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProgramParameterValueDTO(programParameterValueDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(programParameterValueDTO);
            return programParameterValueDTO;
        }
        internal ProgramParameterValueDTO Update(ProgramParameterValueDTO programParameterValueDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(programParameterValueDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ProgramParameterValue] set           
                                    [ProgramId]                         = @ProgramId,
                                    [ParameterId]                       = @ParameterId,
                                    [ParameterValue]                    = @ParameterValue,
                                    [IsActive]                          = @IsActive,
                                    [MasterEntityId]                    = @MasterEntityId,
                                    [LastUpdatedBy]                     = @LastUpdatedBy,
                                    [LastUpdatedDate]                   = GETDATE()
                                    where ProgramParameterValueId = @ProgramParameterValueId
                                    SELECT * FROM ProgramParameterValue WHERE ProgramParameterValueId = @ProgramParameterValueId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(programParameterValueDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProgramParameterValueDTO(programParameterValueDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(programParameterValueDTO);
            return programParameterValueDTO;
        }

        internal List<ProgramParameterValueDTO> GetProgramParameterValueDTOList(List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<ProgramParameterValueDTO> programParameterValueDTOList = new List<ProgramParameterValueDTO>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ProgramParameterValueDTO.SearchByParameters.PROGRAM_ID ||
                            searchParameter.Key == ProgramParameterValueDTO.SearchByParameters.PROGRAM_PARAMETER_VALUE_ID ||
                            searchParameter.Key == ProgramParameterValueDTO.SearchByParameters.PARAMETER_ID ||
                            searchParameter.Key == ProgramParameterValueDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProgramParameterValueDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProgramParameterValueDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == ProgramParameterValueDTO.SearchByParameters.PROGRAM_EXECUTABLE_NAME)
                        {
                            query.Append(joiner + " Exists (select 1 from ConcurrentPrograms cp where "
                                                   + DBSearchParameters[searchParameter.Key] + "= " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                   + " and cp.ProgramId = ppv.ProgramId) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProgramParameterValueDTO.SearchByParameters.PARAMETER_NAME)
                        {
                            query.Append(joiner + " Exists (select 1 from ConcurrentProgramParameters cpp where cpp.ProgramId = ppv.ProgramId and "
                                                   + DBSearchParameters[searchParameter.Key] + "= " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                   + " and cpp.ConcurrentProgramParameterId = ppv.ParameterId) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProgramParameterValueDTO.SearchByParameters.PROGRAM_REQUEST_IS_IN_WIP)
                        {
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                              from ConcurrentPrograms cp,
                                                                   ConcurrentProgramSchedules cps, 
						                                           ConcurrentProgramParameters cpp,
                                                                   ConcurrentRequests cr
                                                             where " + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                         + @"  and cp.ProgramId = cps.ProgramId
                                                               and cps.ProgramId = cr.ProgramId
                                                               and cps.ProgramScheduleId = cr.ProgramScheduleId
                                                               and cps.ProgramScheduleId = ppv.ConcurrentProgramScheduleId
					                                           and cpp.ProgramId = cp.ProgramId
					                                           and cpp.ConcurrentProgramParameterId = ppv.ParameterId 
                                                               and cr.Phase in ('Running', 'Pending')) ");
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
                programParameterValueDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetProgramParameterValueDTO(x)).ToList();
            }
            log.LogMethodExit(programParameterValueDTOList);
            return programParameterValueDTOList;
        }
    }
}
