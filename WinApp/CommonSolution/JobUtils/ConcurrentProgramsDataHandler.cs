
/********************************************************************************************
 * Project Name - Concurrent Programs Data Handler
 * Description  - Data handler of the Concurrent Programs class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016    Amaresh             Created 
 *2.70.2      24-Jul-2019    Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2      10-Dec-2019    Jinto Thomas        Removed siteid from update query       
 *2.100.0     31-Aug-2020    Mushahid Faizan     siteId changes in GetSQLParameters().
 *2.120.1     09-Jun-2021    Deeksha             Modified: As part of AWS concurrent program enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Text;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Concurrent Programs DataHandler - Handles insert, update and select of ConcurrentPrograms objects
    /// </summary>
    public class ConcurrentProgramsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ConcurrentPrograms as cc ";

        /// <summary>
        /// Dictionary for searching Parameters for the ConcurrentPrograms object.
        /// </summary>
        private static readonly Dictionary<ConcurrentProgramsDTO.SearchByProgramsParameters, string> DBSearchParameters = new Dictionary<ConcurrentProgramsDTO.SearchByProgramsParameters, string>
        {
              {ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_ID, "cc.ProgramId"},
              {ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME, "cc.ProgramName"},
              {ConcurrentProgramsDTO.SearchByProgramsParameters.EXECUTABLE_NAME, "cc.ExecutableName"},
              {ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, "cc.Active"},
              {ConcurrentProgramsDTO.SearchByProgramsParameters.SYSTEM_PROGRAM , "cc.SystemProgram"},
              {ConcurrentProgramsDTO.SearchByProgramsParameters.ERROR_NOTIFICATION_MAIL , "cc.ErrorNotificationMailId"},
              {ConcurrentProgramsDTO.SearchByProgramsParameters.SUCCESS_NOTIFICATION_MAIL , "cc.SuccessNotificationMailId"},
              {ConcurrentProgramsDTO.SearchByProgramsParameters.KEEP_RUNNING , "cc.KeepRunning"},
              {ConcurrentProgramsDTO.SearchByProgramsParameters.MULTIPLE_INSTANCE_RUN_ALLOWED , "cc.MutlipleInstanceRunAllowed"},
              {ConcurrentProgramsDTO.SearchByProgramsParameters.MASTER_ENTITY_ID , "cc.MasterEntityId"},
              {ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID , "cc.site_id"},

        };

        /// <summary>
        /// Default constructor of ConcurrentProgramsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentProgramsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ConcurrentPrograms parameters Record.
        /// </summary>
        /// <param name="concurrentProgramsDTO">concurrentProgramsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(ConcurrentProgramsDTO concurrentProgramsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@programId", concurrentProgramsDTO.ProgramId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@programName", string.IsNullOrEmpty(concurrentProgramsDTO.ProgramName) ? DBNull.Value : (object)concurrentProgramsDTO.ProgramName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@executionMethod", string.IsNullOrEmpty(concurrentProgramsDTO.ExecutionMethod) ? DBNull.Value : (object)concurrentProgramsDTO.ExecutionMethod));
            parameters.Add(dataAccessHandler.GetSQLParameter("@executableName", string.IsNullOrEmpty(concurrentProgramsDTO.ExecutableName) ? DBNull.Value : (object)concurrentProgramsDTO.ExecutableName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@argumentCount", concurrentProgramsDTO.ArgumentCount == -1 ? DBNull.Value : (object)concurrentProgramsDTO.ArgumentCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", concurrentProgramsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@systemProgram", concurrentProgramsDTO.SystemProgram));
            parameters.Add(dataAccessHandler.GetSQLParameter("@keepRunning", concurrentProgramsDTO.KeepRunning));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MutlipleInstanceRunAllowed", concurrentProgramsDTO.MutlipleInstanceRunAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastExecutedOn", string.IsNullOrEmpty(concurrentProgramsDTO.LastExecutedOn) ? DBNull.Value : (object)DateTime.Parse(concurrentProgramsDTO.LastExecutedOn)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@errorNotificationMailId", string.IsNullOrEmpty(concurrentProgramsDTO.ErrorNotificationMailId) ? DBNull.Value : (object)concurrentProgramsDTO.ErrorNotificationMailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@successNotificationMailId", string.IsNullOrEmpty(concurrentProgramsDTO.SuccessNotificationMailId) ? DBNull.Value : (object)concurrentProgramsDTO.SuccessNotificationMailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", concurrentProgramsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            return parameters;
        }

        /// <summary>
        /// Inserts the Concurrent Programs record to the database
        /// </summary>
        /// <param name="concurrentProgramDTO">ConcurrentProgramsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ConcurrentProgramsDTO InsertConcurrentPrograms(ConcurrentProgramsDTO concurrentProgramDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramDTO, loginId, siteId);
            string insertconcurrentQuery = @"INSERT INTO [dbo].[ConcurrentPrograms]  
                                                        (                                                 
                                                         ProgramName,
                                                         ExecutionMethod,
                                                         ExecutableName,
                                                         SystemProgram,
                                                         ArgumentCount,
                                                         KeepRunning,
                                                         MutlipleInstanceRunAllowed,
                                                         Active,
                                                         LastUpdatedDate,
                                                         LastUpdatedUser,
                                                         LastExecutedOn,
                                                         site_id,
                                                         Guid,
                                                         ErrorNotificationMailId,
                                                         SuccessNotificationMailId,
                                                         MasterEntityId,
                                                         CreatedBy,
                                                         CreationDate
                                                        ) 
                                                values 
                                                        (
                                                          @programName,
                                                          @executionMethod,
                                                          @executableName,
                                                          @systemProgram,
                                                          @argumentCount,
                                                          @keepRunning,
                                                          @MutlipleInstanceRunAllowed,
                                                          @isActive,
                                                          Getdate(),
                                                          @lastUpdatedUser,
                                                          @lastExecutedOn,
                                                          @siteId,
                                                          Newid(),
                                                          @errorNotificationMailId,
                                                          @successNotificationMailId,
                                                          @masterEntityId,
                                                          @createdBy,
                                                          Getdate()
                                                         )SELECT * FROM ConcurrentPrograms WHERE ProgramId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertconcurrentQuery, GetSQLParameters(concurrentProgramDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentProgramDTO(concurrentProgramDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting concurrentProgram", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentProgramDTO);
            return concurrentProgramDTO;
        }

        /// <summary>
        /// Updates the Concurrent Programs record
        /// </summary>
        /// <param name="concurrentProgramDTO">ConcurrentProgramsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ConcurrentProgramsDTO UpdateConcurrentProgram(ConcurrentProgramsDTO concurrentProgramDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramDTO, loginId, siteId);
            string updateConcurQuery = @"update ConcurrentPrograms 
                                                   set   ProgramName =@programName, 
                                                         ExecutionMethod =@executionMethod,
                                                         ExecutableName =@executableName,
                                                         ArgumentCount =@argumentCount,
                                                         SystemProgram = @systemProgram,
                                                         KeepRunning =@keepRunning,
                                                         MutlipleInstanceRunAllowed=@MutlipleInstanceRunAllowed,
                                                         Active =@isActive,                                                    
                                                         LastUpdatedDate =Getdate(),
                                                         LastUpdatedUser =@lastUpdatedUser,
                                                         LastExecutedOn = @lastExecutedOn,
                                                         -- site_id =@siteId,
                                                         SuccessNotificationMailId = @successNotificationMailId,
                                                         ErrorNotificationMailId = @errorNotificationMailId
                                                         where ProgramId = @programId
                                                         SELECT* FROM ConcurrentPrograms WHERE  ProgramId = @programId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateConcurQuery, GetSQLParameters(concurrentProgramDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentProgramDTO(concurrentProgramDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating concurrentProgram", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentProgramDTO);
            return concurrentProgramDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="concurrentProgramsDTO">concurrentProgramsDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshConcurrentProgramDTO(ConcurrentProgramsDTO concurrentProgramsDTO, DataTable dt)
        {
            log.LogMethodEntry(concurrentProgramsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                concurrentProgramsDTO.ProgramId = Convert.ToInt32(dt.Rows[0]["ProgramId"]);
                concurrentProgramsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                concurrentProgramsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                concurrentProgramsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                concurrentProgramsDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                concurrentProgramsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                concurrentProgramsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ConcurrentProgramsDTO class type
        /// </summary>
        /// <param name="concurrentProgramsDataRow">Concurrent DataRow</param>
        /// <returns>Returns ConcurrentPrograms</returns>
        private ConcurrentProgramsDTO GetConcurrentProgramsDTO(DataRow concurrentProgramsDataRow)
        {
            log.LogMethodEntry(concurrentProgramsDataRow);
            ConcurrentProgramsDTO ConProDataObject = new ConcurrentProgramsDTO(Convert.ToInt32(concurrentProgramsDataRow["ProgramId"]),
                                                    concurrentProgramsDataRow["ProgramName"].ToString(),
                                                    concurrentProgramsDataRow["ExecutionMethod"].ToString(),
                                                    concurrentProgramsDataRow["ExecutableName"].ToString(),
                                                    concurrentProgramsDataRow["SystemProgram"] == DBNull.Value ? false : Convert.ToBoolean(concurrentProgramsDataRow["SystemProgram"]),
                                                    concurrentProgramsDataRow["KeepRunning"] == DBNull.Value ? false : Convert.ToBoolean(concurrentProgramsDataRow["KeepRunning"]),
                                                    concurrentProgramsDataRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(concurrentProgramsDataRow["Active"]),
                                                    concurrentProgramsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramsDataRow["site_id"]),
                                                    concurrentProgramsDataRow["Guid"].ToString(),
                                                    concurrentProgramsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(concurrentProgramsDataRow["SynchStatus"]),
                                                    concurrentProgramsDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(concurrentProgramsDataRow["LastUpdatedDate"]),
                                                    concurrentProgramsDataRow["LastUpdatedUser"].ToString(),
                                                    concurrentProgramsDataRow["LastExecutedOn"].ToString(),
                                                    concurrentProgramsDataRow["SuccessNotificationMailId"].ToString(),
                                                    concurrentProgramsDataRow["ArgumentCount"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramsDataRow["ArgumentCount"]),
                                                    concurrentProgramsDataRow["ErrorNotificationMailId"].ToString(),
                                                    concurrentProgramsDataRow["MutlipleInstanceRunAllowed"] == DBNull.Value ? false : Convert.ToBoolean(concurrentProgramsDataRow["MutlipleInstanceRunAllowed"]),
                                                    concurrentProgramsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramsDataRow["MasterEntityId"]),
                                                    concurrentProgramsDataRow["CreatedBy"].ToString(),
                                                    concurrentProgramsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(concurrentProgramsDataRow["CreationDate"])
                                                    );
            log.LogMethodExit(ConProDataObject);
            return ConProDataObject;
        }

        /// <summary>
        /// Gets the Concurrent Programs data of passed Program Id
        /// </summary>
        /// <param name="programId">integer type parameter</param>
        /// <returns>Returns ConcurrentProgramsDTO</returns>
        public ConcurrentProgramsDTO GetConcurrentPrograms(int programId)
        {
            log.LogMethodEntry(programId);
            ConcurrentProgramsDTO result = null;
            string selectConcurrentProgramsQuery = SELECT_QUERY + @" WHERE ProgramId = @programId";

            SqlParameter parameter = new SqlParameter("@programId", programId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectConcurrentProgramsQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetConcurrentProgramsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ConcurrentProgramsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ConcurrentProgramsDTO matching the search criteria</returns>
        public List<ConcurrentProgramsDTO> GetConcurrentProgramsList(List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ConcurrentProgramsDTO> concurrentProgramsDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_ID
                             || searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.SYSTEM_PROGRAM
                                  || searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.KEEP_RUNNING 
                                  || searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.MULTIPLE_INSTANCE_RUN_ALLOWED)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.ERROR_NOTIFICATION_MAIL
                                  || searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.SUCCESS_NOTIFICATION_MAIL
                                  || searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME
                                  || searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.EXECUTABLE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID)
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
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query + " Order by ProgramId";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                concurrentProgramsDTOList = new List<ConcurrentProgramsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ConcurrentProgramsDTO concurrentProgramsDTO = GetConcurrentProgramsDTO(dataRow);
                    concurrentProgramsDTOList.Add(concurrentProgramsDTO);
                }
            }
            log.LogMethodExit(concurrentProgramsDTOList);
            return concurrentProgramsDTOList;
        }
    }
}
