/********************************************************************************************
 * Project Name - Concurrent Program Arguments Data Handler
 * Description  - Data handler of the Concurrent Programs Argument class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        7-Mar-2016    Amaresh             Created 
 *2.70.2        24-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                          SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas      Removed siteid from update query    
 *2.90          26-May-2020   Mushahid Faizan   Modified: 3 tier changes for Rest API.
 *2.100.0     31-Aug-2020     Mushahid Faizan   siteId changes in GetSQLParameters().
 *2.140      14-Sep-2021      Fiona          Modified: Added ARGUMENT_ID as search parameter and GetConcurrentProgramArgumentsDTO method
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Concurrent Program Argument DataHandler - Handles insert, update and select of Concurrent Program Arguments objects
    /// </summary>

    public class ConcurrentProgramArgumentsDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ConcurrentProgramArguments as cp ";

        /// <summary>
        /// Dictionary for searching Parameters for the ConcurrentProgramArguments object.
        /// </summary>
        private static readonly Dictionary<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string> DBSearchParameters = new Dictionary<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>
        {
             {ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.ARGUMENT_ID, "cp.ArgumentId"},
              {ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.PROGRAM_ID, "cp.ProgramId"},
              {ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.PROGRAM_ID_LIST, "cp.ProgramId"},
              {ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.IS_ACTIVE, "cp.IsActive"},
              {ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.SITE_ID, "cp.site_id"},
              {ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.MASTER_ENTITY_ID, "cp.MasterEntityId"}
        };

        /// <summary>
        /// Default constructor of ConcurrentProgramArgumentsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentProgramArgumentsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ConcurrentProgramSchedules parameters Record.
        /// </summary>
        /// <param name="concurrentProgramArgumentsDTO">concurrentProgramArgumentsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(ConcurrentProgramArgumentsDTO concurrentProgramArgumentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramArgumentsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@argumentId", concurrentProgramArgumentsDTO.ArgumentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@programId", concurrentProgramArgumentsDTO.ProgramId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@argumentValue", string.IsNullOrEmpty(concurrentProgramArgumentsDTO.ArgumentValue) ? DBNull.Value : (object)(concurrentProgramArgumentsDTO.ArgumentValue)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@argumentType", concurrentProgramArgumentsDTO.ArgumentType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", concurrentProgramArgumentsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", concurrentProgramArgumentsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            return parameters;
        }

        /// <summary>
        /// Inserts the Concurrent Program Arguments record to the database
        /// </summary>
        /// <param name="concurrentProgramArguments">ConcurrentProgramArgumentsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ConcurrentProgramArgumentsDTO InsertConcurrentProgramArguments(ConcurrentProgramArgumentsDTO concurrentProgramArguments, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramArguments, loginId, siteId);
            string insertConcurrentQuery = @"insert into ConcurrentProgramArguments 
                                                                        ( 
                                                                          ProgramId, 
                                                                          ArgumentValue,
                                                                          ArgumentType,
                                                                          site_id,
                                                                          IsActive,
                                                                          Guid,
                                                                          MasterEntityId,
                                                                          CreatedBy,
                                                                          CreationDate,
                                                                          LastUpdatedBy, 
                                                                          LastUpdateDate
                                                                         )
                                                                  Values
                                                                         (
                                                                           @programId,
                                                                           @argumentValue,
                                                                           @argumentType,
                                                                           @siteId,
                                                                           @isActive,
                                                                           NEWID(),
                                                                           @masterEntityId,
                                                                           @createdBy,
                                                                           Getdate(),
                                                                           @lastUpdatedBy,
                                                                           Getdate()
                                                                          )SELECT * FROM ConcurrentProgramArguments WHERE ArgumentId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertConcurrentQuery, GetSQLParameters(concurrentProgramArguments, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentProgramArguments(concurrentProgramArguments, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting concurrentProgramArguments", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentProgramArguments);
            return concurrentProgramArguments;
        }

        /// <summary>
        /// Updates the Concurrent Program Arguments record
        /// </summary>
        /// <param name="concurrentProgramArguments">ConcurrentProgramArgumentsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ConcurrentProgramArgumentsDTO UpdateConcurrentProgramArguments(ConcurrentProgramArgumentsDTO concurrentProgramArguments, string loginId, int siteId)
        {
            log.LogMethodEntry(concurrentProgramArguments, loginId, siteId);
            string updateConcurrentQuery = @"update ConcurrentProgramArguments 
                                                   set   ArgumentValue =@argumentValue,
                                                         ArgumentType = @argumentType,
                                                         -- site_id = @siteId,
                                                         IsActive = @isActive,  
                                                         LastUpdateDate =Getdate(),
                                                         LastUpdatedBy =@lastUpdatedBy
                                                         Where ArgumentId = @argumentId
                                                         SELECT* FROM ConcurrentProgramArguments WHERE ArgumentId = @argumentId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateConcurrentQuery, GetSQLParameters(concurrentProgramArguments, loginId, siteId).ToArray(), sqlTransaction);
                RefreshConcurrentProgramArguments(concurrentProgramArguments, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating concurrentProgramsArguments", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(concurrentProgramArguments);
            return concurrentProgramArguments;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="concurrentProgramArgumentsDTO">concurrentProgramArgumentsDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshConcurrentProgramArguments(ConcurrentProgramArgumentsDTO concurrentProgramArgumentsDTO, DataTable dt)
        {
            log.LogMethodEntry(concurrentProgramArgumentsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                concurrentProgramArgumentsDTO.ArgumentId = Convert.ToInt32(dt.Rows[0]["ArgumentId"]);
                concurrentProgramArgumentsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                concurrentProgramArgumentsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                concurrentProgramArgumentsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                concurrentProgramArgumentsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                concurrentProgramArgumentsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                concurrentProgramArgumentsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ConcurrentProgramArgumentsDTO class type
        /// </summary>
        /// <param name="concurrentProgramArgumentsDataRow">Concurrent DataRow</param>
        /// <returns>Returns ConcurrentPrograms</returns>
        private ConcurrentProgramArgumentsDTO GetConcurrentProgramArgumentsDTO(DataRow concurrentProgramArgumentsDataRow)
        {
            log.LogMethodEntry(concurrentProgramArgumentsDataRow);
            ConcurrentProgramArgumentsDTO ConcurrentProgramArgumentsDataObject = new ConcurrentProgramArgumentsDTO(Convert.ToInt32(concurrentProgramArgumentsDataRow["ArgumentId"]),
                                                   Convert.ToInt32(concurrentProgramArgumentsDataRow["ProgramId"]),
                                                    concurrentProgramArgumentsDataRow["ArgumentValue"].ToString(),
                                                    concurrentProgramArgumentsDataRow["ArgumentType"].ToString(),
                                                    concurrentProgramArgumentsDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(concurrentProgramArgumentsDataRow["IsActive"]),
                                                    concurrentProgramArgumentsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramArgumentsDataRow["site_id"]),
                                                    concurrentProgramArgumentsDataRow["Guid"].ToString(),
                                                    concurrentProgramArgumentsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(concurrentProgramArgumentsDataRow["SynchStatus"]),
                                                    concurrentProgramArgumentsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(concurrentProgramArgumentsDataRow["LastUpdateDate"]),
                                                    concurrentProgramArgumentsDataRow["LastUpdatedBy"].ToString(),
                                                    concurrentProgramArgumentsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(concurrentProgramArgumentsDataRow["MasterEntityId"]),
                                                    concurrentProgramArgumentsDataRow["CreatedBy"].ToString(),
                                                    concurrentProgramArgumentsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(concurrentProgramArgumentsDataRow["CreationDate"])
                                                    );
            log.LogMethodExit(ConcurrentProgramArgumentsDataObject);
            return ConcurrentProgramArgumentsDataObject;
        }
        /// <summary>
        /// GetConcurrentProgramArgumentsDTO
        /// </summary>
        /// <param name="argumentId"></param>
        /// <returns></returns>
        public ConcurrentProgramArgumentsDTO GetConcurrentProgramArgumentsDTO(int argumentId)
        {
            log.LogMethodEntry(argumentId);
            ConcurrentProgramArgumentsDTO returnValue = null;
            string query = SELECT_QUERY + " WHERE cp.ArgumentId = @Id";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@Id", argumentId, true) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetConcurrentProgramArgumentsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the Concurrent Program Arguments data of passed Argument Id
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>Returns ConcurrentProgramArgumentsDTO</returns>
        public List<ConcurrentProgramArgumentsDTO> GetConcurrentProgramArguments(List<KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectConcurrentProgramArgumentsQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");

                foreach (KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperator = (count == 0 ? string.Empty : " and ");

                        if (searchParameter.Key == ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.ARGUMENT_ID
                            ||searchParameter.Key == ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.PROGRAM_ID
                            || searchParameter.Key == ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.IS_ACTIVE)
                        {
                            query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.PROGRAM_ID_LIST)
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.SITE_ID)
                        {
                            query.Append(joinOperator + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                    selectConcurrentProgramArgumentsQuery = selectConcurrentProgramArgumentsQuery + query;
            }

            DataTable concurrentProgramArgumentData = dataAccessHandler.executeSelectQuery(selectConcurrentProgramArgumentsQuery, parameters.ToArray(), sqlTransaction);
            if (concurrentProgramArgumentData.Rows.Count > 0)
            {
                List<ConcurrentProgramArgumentsDTO> concurrentProgramArgumentList = new List<ConcurrentProgramArgumentsDTO>();
                foreach (DataRow concurrentDataRow in concurrentProgramArgumentData.Rows)
                {
                    ConcurrentProgramArgumentsDTO concurrentProgramArgumentsDataObject = GetConcurrentProgramArgumentsDTO(concurrentDataRow);
                    concurrentProgramArgumentList.Add(concurrentProgramArgumentsDataObject);
                }
                log.LogMethodExit(concurrentProgramArgumentList);
                return concurrentProgramArgumentList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}