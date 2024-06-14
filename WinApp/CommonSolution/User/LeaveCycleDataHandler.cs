/********************************************************************************************
 * Project Name - User
 * Description  - Data Handler File for LeaveCycle 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
 *2.80        20-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// LeaveCycle Data Handler - Handles insert, update and select of Leave objects
    /// </summary>
    class LeaveCycleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM LeaveCycle as lc ";

        /// <summary>
        /// Dictionary for searching Parameters for the LeaveCycle object.
        /// </summary>
        private static readonly Dictionary<LeaveCycleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LeaveCycleDTO.SearchByParameters, string>
        {
            { LeaveCycleDTO.SearchByParameters.LEAVE_CYCLE_ID,"lc.LeaveCycleId"},
            { LeaveCycleDTO.SearchByParameters.NAME,"lc.Name"},
            { LeaveCycleDTO.SearchByParameters.IS_ACTIVE,"lc.IsActive"},
            { LeaveCycleDTO.SearchByParameters.SITE_ID,"lc.site_id"},
            { LeaveCycleDTO.SearchByParameters.MASTER_ENTITY_ID,"lc.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for LeaveCycleDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public LeaveCycleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LeaveCycle Record.
        /// </summary>
        /// <param name="leaveCycleDTO">LeaveCycleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(LeaveCycleDTO leaveCycleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(leaveCycleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeaveCycleId", leaveCycleDTO.LeaveCycleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", leaveCycleDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartDate", leaveCycleDTO.StartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", leaveCycleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", leaveCycleDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to LeaveCycleDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of LeaveCycleDTO</returns>
        private LeaveCycleDTO GetLeaveCycleDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LeaveCycleDTO leaveCycleDTO = new LeaveCycleDTO(dataRow["LeaveCycleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LeaveCycleId"]),
                                                dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                dataRow["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["StartDate"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? false : (dataRow["IsActive"].ToString() == "Y" ? true : false));
            return leaveCycleDTO;
        }

        /// <summary>
        /// Gets the LeaveCycle data of passed LeaveCycle ID
        /// </summary>
        /// <param name="leaveCycleId">leaveCycleId of LeaveCycle is passed as parameter</param>
        /// <returns>Returns LeaveCycleDTO</returns>
        public LeaveCycleDTO GetLeaveCycleDTO(int leaveCycleId)
        {
            log.LogMethodEntry(leaveCycleId);
            LeaveCycleDTO result = null;
            string query = SELECT_QUERY + @" WHERE lc.LeaveCycleId = @LeaveCycleId";
            SqlParameter parameter = new SqlParameter("@LeaveId", leaveCycleId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLeaveCycleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Deletes the Leave Cycle record
        /// </summary>
        /// <param name="promotionDTO"></param>
        internal void Delete(int LeaveCycleId)
        {
            log.LogMethodEntry(LeaveCycleId);
            string query = @"DELETE  
                             FROM LeaveCycle
                             WHERE LeaveCycle.LeaveCycleId = @leaveCycle_Id";
            SqlParameter parameter = new SqlParameter("@leaveCycle_Id", LeaveCycleId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="leaveCycleDTO">LeaveCycleDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshLeaveCycleDTO(LeaveCycleDTO leaveCycleDTO, DataTable dt)
        {
            log.LogMethodEntry(leaveCycleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                leaveCycleDTO.LeaveCycleId = Convert.ToInt32(dt.Rows[0]["LeaveCycleId"]);
                leaveCycleDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                leaveCycleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                leaveCycleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                leaveCycleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                leaveCycleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                leaveCycleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        ///  Inserts the record to the LeaveCycle Table. 
        /// </summary>
        /// <param name="leaveCycleDTO">LeaveCycleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LeaveCycleDTO</returns>
        public LeaveCycleDTO Insert(LeaveCycleDTO leaveCycleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(leaveCycleDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[LeaveCycle]
                            (
                            Name,
                            StartDate,
                            LastUpdatedBy,
                            LastUpdatedDate,
                            Guid,
                            site_id,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            IsActive
                            )
                            VALUES
                            (
                            @Name,
                            @StartDate,
                            @LastUpdatedBy,
                            GETDATE(),
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),  
                            @IsActive
                            )
                            SELECT * FROM LeaveCycle WHERE LeaveCycleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(leaveCycleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLeaveCycleDTO(leaveCycleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LeaveCycleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(leaveCycleDTO);
            return leaveCycleDTO;
        }

        /// <summary>
        /// Update the record in the LeaveCycle Table. 
        /// </summary>
        /// <param name="leaveCycleDTO">LeaveCycleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LeaveCycleDTO</returns>
        public LeaveCycleDTO Update(LeaveCycleDTO leaveCycleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(leaveCycleDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[LeaveCycle]
                             SET
                             Name = @Name,
                             StartDate = @StartDate,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdatedDate = GETDATE(),
                             --Guid = @Guid,
                             --site_id = @site_id,
                             IsActive = @IsActive,
                             MasterEntityId = @MasterEntityId
                             WHERE LeaveCycleId = @LeaveCycleId
                            SELECT * FROM LeaveCycle WHERE LeaveCycleId = @LeaveCycleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(leaveCycleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLeaveCycleDTO(leaveCycleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating LeaveCycleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(leaveCycleDTO);
            return leaveCycleDTO;
        }

        /// <summary>
        /// Returns the List of LeaveCycleDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>List of LeaveCycleDTO </returns>
        public List<LeaveCycleDTO> GetLeaveCycleDTOList(List<KeyValuePair<LeaveCycleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LeaveCycleDTO> leaveCycleDTOList = new List<LeaveCycleDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LeaveCycleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LeaveCycleDTO.SearchByParameters.LEAVE_CYCLE_ID ||
                            searchParameter.Key == LeaveCycleDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LeaveCycleDTO.SearchByParameters.NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == LeaveCycleDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LeaveCycleDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                    LeaveCycleDTO leaveCycleDTO = GetLeaveCycleDTO(dataRow);
                    leaveCycleDTOList.Add(leaveCycleDTO);
                }
            }
            log.LogMethodExit(leaveCycleDTOList);
            return leaveCycleDTOList;
        }
    }
}
