/********************************************************************************************
 * Project Name - User
 * Description  - Data Handler File for WorkShiftUser 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        4-June-2019   Divya A                 Created 
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
    /// WorkShiftUser Data Handler - Handles insert, update and select of WorkShift objects
    /// </summary>
   public class WorkShiftUserDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM WorkShiftUsers AS wsu";

        /// <summary>
        /// Dictionary for searching Parameters for the WorkShiftUser object.
        /// </summary>
        private static readonly Dictionary<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string> DBSearchParameters = new Dictionary<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>
        {
            { WorkShiftUserDTO.SearchByWorkShiftUserParameters.WORK_SHIFT_USERS_ID,"wsu.ID"},
            { WorkShiftUserDTO.SearchByWorkShiftUserParameters.WORK_SHIFT_ID,"wsu.WorkShiftId"},
            { WorkShiftUserDTO.SearchByWorkShiftUserParameters.WORK_SHIFT_ID_LIST,"wsu.WorkShiftId"},
            { WorkShiftUserDTO.SearchByWorkShiftUserParameters.USER_ID,"wsu.UserId"},
            { WorkShiftUserDTO.SearchByWorkShiftUserParameters.SITE_ID,"wsu.site_id"},
            { WorkShiftUserDTO.SearchByWorkShiftUserParameters.IS_ACTIVE,"wsu.IsActive"},
            { WorkShiftUserDTO.SearchByWorkShiftUserParameters.MASTER_ENTITY_ID,"wsu.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for WorkShiftUserDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public WorkShiftUserDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating WorkShiftUser Record.
        /// </summary>
        /// <param name="workShiftUserDTO">WorkShiftUserDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(WorkShiftUserDTO workShiftUserDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(workShiftUserDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", workShiftUserDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@WorkShiftId", workShiftUserDTO.WorkShiftId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserId", workShiftUserDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", workShiftUserDTO.MasterEntityId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", workShiftUserDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        ///<summary>
        /// Converts the Data row object to WorkShiftUserDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of WorkShiftUserDTO</returns>
        private WorkShiftUserDTO GetWorkShiftUserDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            WorkShiftUserDTO workShiftUserDTO = new WorkShiftUserDTO(dataRow["ID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ID"]),
                                                dataRow["WorkShiftId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["WorkShiftId"]),
                                                dataRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserId"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["IsActive"] == DBNull.Value ?true : Convert.ToBoolean(dataRow["IsActive"])
                                                );
            log.LogMethodExit(workShiftUserDTO);
            return workShiftUserDTO;
        }

        /// <summary>
        /// Gets the  WorkShiftUser data of passed id 
        /// </summary>
        /// <param name="id">id is passed as parameter</param>
        /// <returns>Returns WorkShiftUserDTO</returns>
        public WorkShiftUserDTO GetWorkShiftUser(int id)
        {
            log.LogMethodEntry(id);
            WorkShiftUserDTO result = null;
            string query = SELECT_QUERY + @" WHERE wsu.ID = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetWorkShiftUserDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="workShiftUserDTO">WorkShiftUserDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshWorkShiftUserDTO(WorkShiftUserDTO workShiftUserDTO, DataTable dt)
        {
            log.LogMethodEntry(workShiftUserDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                workShiftUserDTO.Id = Convert.ToInt32(dataRow["ID"]);
                workShiftUserDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                workShiftUserDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                workShiftUserDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                workShiftUserDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                workShiftUserDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                workShiftUserDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the WorkShiftUser Table. 
        /// </summary>
        /// <param name="workShiftUserDTO">WorkShiftUserDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns updated WorkShiftUserDTO</returns>
        public WorkShiftUserDTO Insert(WorkShiftUserDTO workShiftUserDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(workShiftUserDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[WorkShiftUsers]
                            (
                            WorkShiftId,
                            UserId,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate , IsActive
                            )
                            VALUES
                            (
                            @WorkShiftId,
                            @UserId,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),@isActive
                            )
                            SELECT * FROM WorkShiftUsers WHERE ID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(workShiftUserDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWorkShiftUserDTO(workShiftUserDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting WorkShiftUserDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(workShiftUserDTO);
            return workShiftUserDTO;
        }

        /// <summary>
        /// Update the record in the WorkShiftUser Table. 
        /// </summary>
        /// <param name="workShiftUserDTO">WorkShiftUserDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated WorkShiftUserDTO</returns>
        public WorkShiftUserDTO Update(WorkShiftUserDTO workShiftUserDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(workShiftUserDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[WorkShiftUsers]
                             SET
                             WorkShiftId = @WorkShiftId,
                             UserId = @UserId,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdateDate = GETDATE(),
                             IsActive = @isActive
                             WHERE ID = @Id
                             SELECT * FROM WorkShiftUsers WHERE ID = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(workShiftUserDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWorkShiftUserDTO(workShiftUserDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating WorkShiftUserDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(workShiftUserDTO);
            return workShiftUserDTO;
        }

        /// <summary>
        /// Returns the List of WorkShiftUserDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of WorkShiftUserDTO </returns>
        public List<WorkShiftUserDTO> GetWorkShiftUserDTOList(List<KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>> searchParameters,
                                      SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<WorkShiftUserDTO> workShiftUserDTOList = new List<WorkShiftUserDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == WorkShiftUserDTO.SearchByWorkShiftUserParameters.WORK_SHIFT_USERS_ID ||
                            searchParameter.Key == WorkShiftUserDTO.SearchByWorkShiftUserParameters.WORK_SHIFT_ID ||
                            searchParameter.Key == WorkShiftUserDTO.SearchByWorkShiftUserParameters.USER_ID ||
                            searchParameter.Key == WorkShiftUserDTO.SearchByWorkShiftUserParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WorkShiftUserDTO.SearchByWorkShiftUserParameters.WORK_SHIFT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == WorkShiftUserDTO.SearchByWorkShiftUserParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WorkShiftUserDTO.SearchByWorkShiftUserParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    WorkShiftUserDTO workShiftUserDTO = GetWorkShiftUserDTO(dataRow);
                    workShiftUserDTOList.Add(workShiftUserDTO);
                }
            }
            log.LogMethodExit(workShiftUserDTOList);
            return workShiftUserDTOList;
        }

    }
}
