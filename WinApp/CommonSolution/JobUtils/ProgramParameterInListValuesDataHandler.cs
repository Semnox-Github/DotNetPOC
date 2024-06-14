/********************************************************************************************
 * Project Name -     public class ProgramParameterInListValues DataHandler
 * Description  - Datahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    public class ProgramParameterInListValuesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProgramParameterInListValues AS ppl ";

        private static readonly Dictionary<ProgramParameterInListValuesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProgramParameterInListValuesDTO.SearchByParameters, string>
            {
                {ProgramParameterInListValuesDTO.SearchByParameters.PROGRAM_PARAMETER_VALUE_ID, "ppl.ProgramParameterValueId"},
                {ProgramParameterInListValuesDTO.SearchByParameters.PROGRAM_PARAMETER_IN_LIST_VALUE_ID, "ppl.ProgramParameterInListValueId"},
                {ProgramParameterInListValuesDTO.SearchByParameters.IS_ACTIVE, "ppl.IsActive"},
                {ProgramParameterInListValuesDTO.SearchByParameters.MASTER_ENTITY_ID, "ppl.MasterEntityId"},
                {ProgramParameterInListValuesDTO.SearchByParameters.SITE_ID, "ppl.site_id"},
            };

        /// <summary>
        /// Parameterized Constructor for ProgramParameterInListValuesDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public ProgramParameterInListValuesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get SQL Parameters
        /// </summary>
        /// <param name="ProgramParameterInListValuesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(ProgramParameterInListValuesDTO programParameterInListValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(programParameterInListValuesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProgramParameterInListValueId", programParameterInListValuesDTO.ProgramParameterInListValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProgramParameterValueId", programParameterInListValuesDTO.ProgramParameterValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InListValue", programParameterInListValuesDTO.InListValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", programParameterInListValuesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", programParameterInListValuesDTO.MasterEntityId, true));
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
        private ProgramParameterInListValuesDTO GetProgramParameterInListValuesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProgramParameterInListValuesDTO programParameterInListValuesDTO = new ProgramParameterInListValuesDTO(
                dataRow["ProgramParameterInListValueId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProgramParameterInListValueId"]),
                dataRow["ProgramParameterValueId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProgramParameterValueId"]),
                dataRow["InListValue"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["InListValue"]),
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
            log.LogMethodExit(programParameterInListValuesDTO);
            return programParameterInListValuesDTO;
        }

        internal ProgramParameterInListValuesDTO GetProgramParameterInListValuesDTO(int programParameterInListValueId)
        {
            log.LogMethodEntry(programParameterInListValueId);
            ProgramParameterInListValuesDTO result = null;
            string query = SELECT_QUERY + @" WHERE ppl.ProgramParameterInListValueId = @ProgramParameterInListValueId";
            SqlParameter parameter = new SqlParameter("@ProgramParameterInListValueId", programParameterInListValueId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProgramParameterInListValuesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        internal List<ProgramParameterInListValuesDTO> GetProgramParameterInListValuesDTOListOfValues(List<int> programParameterValueIdList,
                                                                                           bool activeRecords)
        {
            log.LogMethodEntry(programParameterValueIdList, activeRecords);
            List<ProgramParameterInListValuesDTO> programParameterInListValuesDTOList = new List<ProgramParameterInListValuesDTO>();
            string query = SELECT_QUERY + @" , @ProgramParameterValueIdList List
                            WHERE ProgramParameterValueId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ProgramParameterValueIdList", programParameterValueIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                programParameterInListValuesDTOList = table.Rows.Cast<DataRow>().Select(x => GetProgramParameterInListValuesDTO(x)).ToList();
            }
            log.LogMethodExit(programParameterInListValuesDTOList);
            return programParameterInListValuesDTOList;
        }


        /// <summary>
        /// Gets the List of ProgramParameterInListValuesDTO based on the ProgramParameterInListValues Id List
        /// </summary>
        /// <param name="ProgramParameterInListValuesIdList">List of ProgramParameterInListValues Ids </param>
        /// <param name="activeRecords">activeRecords </param>
        /// <returns>returns the ProgramParameterInListValuesDTO List</returns>
        internal List<ProgramParameterInListValuesDTO> GetProgramParameterInListValuesDTOList(List<int> programParameterInListValuesIdList, bool activeRecords)
        {
            log.LogMethodEntry(programParameterInListValuesIdList, activeRecords);
            List<ProgramParameterInListValuesDTO> ProgramParameterInListValuesDTOList = new List<ProgramParameterInListValuesDTO>();
            string query = @"SELECT *
                            FROM ProgramParameterInListValues, @ProgramParameterInListValueId List
                            WHERE ProgramParameterInListValueId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ProgramParameterInListValueId", programParameterInListValuesIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                ProgramParameterInListValuesDTOList = table.Rows.Cast<DataRow>().Select(x => GetProgramParameterInListValuesDTO(x)).ToList();
            }
            log.LogMethodExit(ProgramParameterInListValuesDTOList);
            return ProgramParameterInListValuesDTOList;
        }
        internal void Delete(ProgramParameterInListValuesDTO programParameterInListValuesDTO)
        {
            log.LogMethodEntry(programParameterInListValuesDTO);
            string query = @"DELETE  
                             FROM ProgramParameterInListValues
                             WHERE ProgramParameterInListValueId = @ProgramParameterInListValueId";
            SqlParameter parameter = new SqlParameter("@ProgramParameterInListValueId", programParameterInListValuesDTO.ProgramParameterInListValueId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
        private void RefreshProgramParameterInListValuesDTO(ProgramParameterInListValuesDTO programParameterInListValuesDTO, DataTable dt)
        {
            log.LogMethodEntry(programParameterInListValuesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                programParameterInListValuesDTO.ProgramParameterInListValueId = Convert.ToInt32(dt.Rows[0]["ProgramParameterInListValueId"]);
                programParameterInListValuesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                programParameterInListValuesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                programParameterInListValuesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                programParameterInListValuesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                programParameterInListValuesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                programParameterInListValuesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal ProgramParameterInListValuesDTO Insert(ProgramParameterInListValuesDTO programParameterInListValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(programParameterInListValuesDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ProgramParameterInListValues]
                               ([ProgramParameterValueId]
                               ,[InListValue]
                               ,[IsActive]
                               ,[CreatedBy]
                               ,[CreationDate]
                               ,[LastUpdatedBy]
                               ,[LastUpdatedDate]
                               ,[Guid]
                               ,[site_id]
                               ,[MasterEntityId])
                               
                         VALUES
                               (
                                @ProgramParameterValueId,
                                @InListValue,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                NEWID(), 
                                @SiteId,
                                @MasterEntityId
                                 )
                                SELECT * FROM ProgramParameterInListValues WHERE ProgramParameterInListValueId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(programParameterInListValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProgramParameterInListValuesDTO(programParameterInListValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(programParameterInListValuesDTO);
            return programParameterInListValuesDTO;
        }
        internal ProgramParameterInListValuesDTO Update(ProgramParameterInListValuesDTO programParameterInListValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(programParameterInListValuesDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ProgramParameterInListValues] set 
                                    [ProgramParameterValueId]                = @ProgramParameterValueId,
                                    [InListValue]                            = @InListValue,
                                    [IsActive]                               = @IsActive,
                                    [MasterEntityId]                         = @MasterEntityId,
                                    [LastUpdatedBy]                          = @LastUpdatedBy,
                                    [LastUpdatedDate]                        = GETDATE()
                                    where ProgramParameterInListValueId = @ProgramParameterInListValueId
                                    SELECT * FROM ProgramParameterInListValues WHERE ProgramParameterInListValueId = @ProgramParameterInListValueId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(programParameterInListValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProgramParameterInListValuesDTO(programParameterInListValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(programParameterInListValuesDTO);
            return programParameterInListValuesDTO;
        }

        internal List<ProgramParameterInListValuesDTO> GetAllProgramParameterInListValuesDTOList(List<KeyValuePair<ProgramParameterInListValuesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<ProgramParameterInListValuesDTO> programParameterInListValuesDTOList = new List<ProgramParameterInListValuesDTO>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ProgramParameterInListValuesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ProgramParameterInListValuesDTO.SearchByParameters.PROGRAM_PARAMETER_IN_LIST_VALUE_ID ||
                            searchParameter.Key == ProgramParameterInListValuesDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == ProgramParameterInListValuesDTO.SearchByParameters.PROGRAM_PARAMETER_VALUE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProgramParameterInListValuesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProgramParameterInListValuesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
                programParameterInListValuesDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetProgramParameterInListValuesDTO(x)).ToList();
            }
            log.LogMethodExit(programParameterInListValuesDTOList);
            return programParameterInListValuesDTOList;
        }
    }
}
