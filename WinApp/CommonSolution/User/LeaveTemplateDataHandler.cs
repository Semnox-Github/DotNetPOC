/********************************************************************************************
 * Project Name - User
 * Description  - Data Handler File for LeaveTemplate
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        4-June-2019   Divya A                 Created 
 *2.70        15-Oct-2019   Indrajeet Kumar         Added Delete Method
 *2.80        20-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 *2.90        2- Sep-2020   Girish Kundar           Modified : Fix for isActive is null then return as true in DTO
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
    /// LeaveTemplate Data Handler - Handles insert, update and select of LeaveTemplate objects
    /// </summary>
   public class LeaveTemplateDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM LeaveTemplate as lt ";

        /// <summary>
        /// Dictionary for searching Parameters for the LeaveTemplate object.
        /// </summary>
        private static readonly Dictionary<LeaveTemplateDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LeaveTemplateDTO.SearchByParameters, string>
        {
            { LeaveTemplateDTO.SearchByParameters.LEAVE_TEMPLATE_ID,"lt.LeaveTemplateId"},
            { LeaveTemplateDTO.SearchByParameters.LEAVE_TYPE_ID,"lt.LeaveTypeId"},
            { LeaveTemplateDTO.SearchByParameters.DEPARTMENT_ID,"lt.DepartmentId"},
            { LeaveTemplateDTO.SearchByParameters.ROLE_ID,"lt.RoleId"},
            { LeaveTemplateDTO.SearchByParameters.IS_ACTIVE,"lt.IsActive"},
            { LeaveTemplateDTO.SearchByParameters.SITE_ID,"lt.site_id"},
            { LeaveTemplateDTO.SearchByParameters.MASTER_ENTITY_ID,"lt.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for LeaveTemplateDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public LeaveTemplateDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LeaveTemplate Record.
        /// </summary>
        /// <param name="leaveTemplateDTO">LeaveTemplateDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(LeaveTemplateDTO leaveTemplateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(leaveTemplateDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeaveTemplateId", leaveTemplateDTO.LeaveTemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DepartmentId", leaveTemplateDTO.DepartmentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RoleId", leaveTemplateDTO.RoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Frequency", leaveTemplateDTO.Frequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeaveDays", leaveTemplateDTO.LeaveDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeaveTypeId", leaveTemplateDTO.LeaveTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveDate", leaveTemplateDTO.EffectiveDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", leaveTemplateDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", leaveTemplateDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to LeaveTemplateDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of LeaveTemplateDTO</returns>
        private LeaveTemplateDTO GetLeaveTemplateDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LeaveTemplateDTO leaveTemplateDTO = new LeaveTemplateDTO(dataRow["LeaveTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LeaveTemplateId"]),
                                                dataRow["DepartmentId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DepartmentId"]),
                                                dataRow["RoleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RoleId"]),
                                                dataRow["Frequency"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Frequency"]),
                                                dataRow["LeaveDays"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LeaveDays"]),
                                                dataRow["LeaveTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LeaveTypeId"]),
                                                dataRow["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]));
            log.LogMethodExit(leaveTemplateDTO);
            return leaveTemplateDTO;
        }

        /// <summary>
        /// Gets the LeaveCycle data of passed LeaveTemplate ID
        /// </summary>
        /// <param name="leaveTemplateId">leaveTemplateId is passed as parameter</param>
        /// <returns>Returns LeaveTemplateDTO</returns>
        public LeaveTemplateDTO GetLeaveTemplateDTO(int leaveTemplateId)
        {
            log.LogMethodEntry(leaveTemplateId);
            LeaveTemplateDTO result = null;
            string query = SELECT_QUERY + @" WHERE lt.LeaveTemplateId = @LeaveTemplateId";
            SqlParameter parameter = new SqlParameter("@LeaveTemplateId", leaveTemplateId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLeaveTemplateDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Deletes the Leave Template record
        /// </summary>
        /// <param name="promotionDTO"></param>
        internal void Delete(int LeaveTemplateId)
        {
            log.LogMethodEntry(LeaveTemplateId);
            string query = @"DELETE  
                             FROM LeaveTemplate
                             WHERE LeaveTemplate.LeaveTemplateId = @leaveTemplate_Id";
            SqlParameter parameter = new SqlParameter("@leaveTemplate_Id", LeaveTemplateId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="leaveTemplateDTO">LeaveTemplateDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshLeaveTemplateDTO(LeaveTemplateDTO leaveTemplateDTO, DataTable dt)
        {
            log.LogMethodEntry(leaveTemplateDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                leaveTemplateDTO.LeaveTemplateId = Convert.ToInt32(dt.Rows[0]["LeaveTemplateId"]);
                leaveTemplateDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                leaveTemplateDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                leaveTemplateDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                leaveTemplateDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                leaveTemplateDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                leaveTemplateDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        ///  Inserts the record to the LeaveTemplate Table. 
        /// </summary>
        /// <param name="leaveTemplateDTO">LeaveTemplateDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LeaveTemplateDTO</returns>
        public LeaveTemplateDTO Insert(LeaveTemplateDTO leaveTemplateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(leaveTemplateDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[LeaveTemplate]
                            (
                            DepartmentId,
                            RoleId,
                            Frequency,
                            LeaveDays,
                            LastUpdatedBy,
                            LastUpdatedDate,
                            Guid,
                            site_id,
                            LeaveTypeId,
                            EffectiveDate,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate, 
                            IsActive
                            )
                            VALUES
                            (
                            @DepartmentId,
                            @RoleId,
                            @Frequency,
                            @LeaveDays,
                            @LastUpdatedBy,
                            GETDATE(),
                            NEWID(),
                            @site_id,
                            @LeaveTypeId,
                            @EffectiveDate,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @IsActive
                            )
                            SELECT * FROM LeaveTemplate WHERE LeaveTemplateId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(leaveTemplateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLeaveTemplateDTO(leaveTemplateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LeaveTemplateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(leaveTemplateDTO);
            return leaveTemplateDTO;
        }

        /// <summary>
        /// Update the record in the LeaveTemplate Table. 
        /// </summary>
        /// <param name="leaveTemplateDTO">LeaveTemplateDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LeaveTemplateDTO</returns>
        public LeaveTemplateDTO Update(LeaveTemplateDTO leaveTemplateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(leaveTemplateDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[LeaveTemplate]
                             SET
                            DepartmentId = @DepartmentId,
                            RoleId = @RoleId,
                            Frequency = @Frequency,
                            LeaveDays = @LeaveDays,
                            LastUpdatedBy = @LastUpdatedBy,
                            LastUpdatedDate = GETDATE(),
                            --site_id = @site_id,
                            LeaveTypeId = @LeaveTypeId,
                            EffectiveDate = @EffectiveDate,
                            IsActive = @IsActive,
                            MasterEntityId = @MasterEntityId     
                            WHERE LeaveTemplateId = @LeaveTemplateId
                            SELECT * FROM LeaveTemplate WHERE LeaveTemplateId = @LeaveTemplateId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(leaveTemplateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLeaveTemplateDTO(leaveTemplateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LeaveTemplateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(leaveTemplateDTO);
            return leaveTemplateDTO;
        }

        /// <summary>
        /// Returns the List of LeaveTemplateDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List o fLeaveTemplateDTO </returns>
        public List<LeaveTemplateDTO> GetLeaveTemplateDTOList(List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LeaveTemplateDTO> leaveTemplateDTOList = new List<LeaveTemplateDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LeaveTemplateDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LeaveTemplateDTO.SearchByParameters.LEAVE_TEMPLATE_ID ||
                            searchParameter.Key == LeaveTemplateDTO.SearchByParameters.LEAVE_TYPE_ID ||
                            searchParameter.Key == LeaveTemplateDTO.SearchByParameters.DEPARTMENT_ID ||
                            searchParameter.Key == LeaveTemplateDTO.SearchByParameters.ROLE_ID ||
                            searchParameter.Key == LeaveTemplateDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LeaveTemplateDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LeaveTemplateDTO.SearchByParameters.IS_ACTIVE) 
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                    LeaveTemplateDTO leaveTemplateDTO = GetLeaveTemplateDTO(dataRow);
                    leaveTemplateDTOList.Add(leaveTemplateDTO);
                }
            }
            log.LogMethodExit(leaveTemplateDTOList);
            return leaveTemplateDTOList;
        }
    }
}
