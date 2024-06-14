/********************************************************************************************
 * Project Name - ShiftConfigurationsDataHandler
 * Description  - Data handler file for  Shift Configurations
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90.0      03-Jul-2020   Akshay Gulaganji        Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Shift Configurations Data Handler - Handles insert, update and select of Shift Configurations objects
    /// </summary>
    public class ShiftConfigurationsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ShiftConfigurations as sc ";

        /// <summary>
        /// Dictionary for searching Parameters for the ShiftConfigurations object.
        /// </summary>
        private static readonly Dictionary<ShiftConfigurationsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ShiftConfigurationsDTO.SearchByParameters, string>
        {
            { ShiftConfigurationsDTO.SearchByParameters.SHIFT_CONFIGURATION_ID,"sc.ShiftConfigurationId"},
            { ShiftConfigurationsDTO.SearchByParameters.SHIFT_CONFIGURATION_NAME,"sc.ShiftConfigurationName"},
            { ShiftConfigurationsDTO.SearchByParameters.SHIFT_TRACK_ALLOWED,"sc.ShiftTrackAllowed"},
            { ShiftConfigurationsDTO.SearchByParameters.OVERTIME_ALLOWED,"sc.OvertimeAllowed"},
            { ShiftConfigurationsDTO.SearchByParameters.IS_ACTIVE,"sc.IsActive"},
            { ShiftConfigurationsDTO.SearchByParameters.SITE_ID,"sc.site_id"},
            { ShiftConfigurationsDTO.SearchByParameters.MASTER_ENTITY_ID,"sc.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for ShiftConfigurationsDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction Object</param>
        public ShiftConfigurationsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ShiftConfigurations Record.
        /// </summary>
        /// <param name="shiftConfigurationsDTO">shiftConfigurationsDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ShiftConfigurationsDTO shiftConfigurationsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(shiftConfigurationsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@shiftConfigurationId", shiftConfigurationsDTO.ShiftConfigurationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shiftConfigurationName", shiftConfigurationsDTO.ShiftConfigurationName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shiftMinutes", shiftConfigurationsDTO.ShiftMinutes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@weeklyShiftMinutes", shiftConfigurationsDTO.WeeklyShiftMinutes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@graceMinutes", shiftConfigurationsDTO.GraceMinutes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shiftTrackAllowed", shiftConfigurationsDTO.ShiftTrackAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@overtimeAllowed", shiftConfigurationsDTO.OvertimeAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maximumOvertimeMinutes", shiftConfigurationsDTO.MaximumOvertimeMinutes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maximumWeeklyOvertimeMinutes", shiftConfigurationsDTO.MaximumWeeklyOvertimeMinutes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", shiftConfigurationsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", shiftConfigurationsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to ShiftConfigurationsDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of ShiftConfigurationsDTO</returns>
        private ShiftConfigurationsDTO GetShiftConfigurationsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ShiftConfigurationsDTO shiftConfigurationsDTO = new ShiftConfigurationsDTO(
                                                dataRow["ShiftConfigurationId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ShiftConfigurationId"]),
                                                dataRow["ShiftConfigurationName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ShiftConfigurationName"]),
                                                dataRow["ShiftMinutes"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ShiftMinutes"]),
                                                dataRow["WeeklyShiftMinutes"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["WeeklyShiftMinutes"]),
                                                dataRow["GraceMinutes"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GraceMinutes"]),
                                                dataRow["ShiftTrackAllowed"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ShiftTrackAllowed"]),
                                                dataRow["OvertimeAllowed"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["OvertimeAllowed"]),
                                                dataRow["MaximumOvertimeMinutes"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MaximumOvertimeMinutes"]),
                                                dataRow["MaximumWeeklyOvertimeMinutes"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MaximumWeeklyOvertimeMinutes"]),
                                                dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"].ToString()),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                );
            log.LogMethodExit(shiftConfigurationsDTO);
            return shiftConfigurationsDTO;
        }

        /// <summary>
        /// Gets the Shift Configurations data of passed Shift Configuration ID
        /// </summary>
        /// <param name="ShiftConfigurationId">shiftConfigurationId is passed as Parameter</param>
        /// <returns>Returns PromotionDTO</returns>
        public ShiftConfigurationsDTO GetShiftConfigurationsDTO(int shiftConfigurationId)
        {
            log.LogMethodEntry(shiftConfigurationId);
            ShiftConfigurationsDTO shiftConfigurationsDTO = null;
            string query = SELECT_QUERY + @" WHERE sc.ShiftConfigurationId = @shiftConfigurationId";
            SqlParameter parameter = new SqlParameter("@shiftConfigurationId", shiftConfigurationId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                shiftConfigurationsDTO = GetShiftConfigurationsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(shiftConfigurationsDTO);
            return shiftConfigurationsDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="shiftConfigurationsDTO">ShiftConfigurationsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshShiftConfigurationsDTO(ShiftConfigurationsDTO shiftConfigurationsDTO, DataTable dt)
        {
            log.LogMethodEntry(shiftConfigurationsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                shiftConfigurationsDTO.ShiftConfigurationId = Convert.ToInt32(dt.Rows[0]["ShiftConfigurationId"]);
                shiftConfigurationsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                shiftConfigurationsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                shiftConfigurationsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                shiftConfigurationsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                shiftConfigurationsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                shiftConfigurationsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the ShiftConfigurations Table. 
        /// </summary>
        /// <param name="shiftConfigurationsDTO">shiftConfigurationsDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PromotionDTO</returns>
        public ShiftConfigurationsDTO Insert(ShiftConfigurationsDTO shiftConfigurationsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(shiftConfigurationsDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ShiftConfigurations]
                            (
                            ShiftConfigurationName,
                            ShiftMinutes,
                            WeeklyShiftMinutes,
                            GraceMinutes,
                            ShiftTrackAllowed,
                            OvertimeAllowed,
                            MaximumOvertimeMinutes,
                            MaximumWeeklyOvertimeMinutes,
                            IsActive,
                            Guid,
                            site_id,
                            MasterEntityId,
                            LastUpdatedDate,
                            LastUpdatedBy,
                            CreatedBy,
                            CreationDate
                            )
                            VALUES
                            (
                            @shiftConfigurationName,
                            @shiftMinutes,
                            @weeklyShiftMinutes,
                            @graceMinutes,
                            @shiftTrackAllowed,
                            @overtimeAllowed,
                            @maximumOvertimeMinutes,
                            @maximumWeeklyOvertimeMinutes,
                            @isActive,
                            NEWID(),
                            @siteId,
                            @masterEntityId,
                            GETDATE(),
                            @lastUpdatedBy,
                            @CreatedBy,
                            GETDATE()                      
                            )
                            SELECT * FROM ShiftConfigurations WHERE ShiftConfigurationId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(shiftConfigurationsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshShiftConfigurationsDTO(shiftConfigurationsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ShiftConfigurationsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(shiftConfigurationsDTO);
            return shiftConfigurationsDTO;
        }

        /// <summary>
        /// Update the record in the ShiftConfigurations Table. 
        /// </summary>
        /// <param name="shiftConfigurationsDTO">ShiftConfigurationsDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PromotionDTO</returns>
        public ShiftConfigurationsDTO Update(ShiftConfigurationsDTO shiftConfigurationsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(shiftConfigurationsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ShiftConfigurations]
                             SET
                             ShiftConfigurationName = @shiftConfigurationName,
                             ShiftMinutes = @shiftMinutes,
                             WeeklyShiftMinutes = @weeklyShiftMinutes,
                             GraceMinutes = @graceMinutes,
                             ShiftTrackAllowed = @shiftTrackAllowed,
                             OvertimeAllowed = @overtimeAllowed,
                             MaximumOvertimeMinutes = @maximumOvertimeMinutes,
                             MaximumWeeklyOvertimeMinutes = @maximumWeeklyOvertimeMinutes,
                             IsActive = @isActive,
                             LastUpdatedDate = GETDATE(),
                             LastUpdatedBy = @lastUpdatedBy        
                             WHERE ShiftConfigurationId = @shiftConfigurationId
                            SELECT * FROM ShiftConfigurations WHERE ShiftConfigurationId = @shiftConfigurationId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(shiftConfigurationsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshShiftConfigurationsDTO(shiftConfigurationsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating ShiftConfigurationsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(shiftConfigurationsDTO);
            return shiftConfigurationsDTO;
        }

        /// <summary>
        /// Returns the List of ShiftConfigurationsDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of ShiftConfigurationsDTO</returns>
        public List<ShiftConfigurationsDTO> GetShiftConfigurationsDTOList(List<KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ShiftConfigurationsDTO> shiftConfigurationsDTOList = new List<ShiftConfigurationsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ShiftConfigurationsDTO.SearchByParameters.SHIFT_CONFIGURATION_ID ||
                            searchParameter.Key == ShiftConfigurationsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ShiftConfigurationsDTO.SearchByParameters.SHIFT_CONFIGURATION_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ShiftConfigurationsDTO.SearchByParameters.OVERTIME_ALLOWED ||
                                 searchParameter.Key == ShiftConfigurationsDTO.SearchByParameters.SHIFT_TRACK_ALLOWED ||
                                 searchParameter.Key == ShiftConfigurationsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == ShiftConfigurationsDTO.SearchByParameters.SITE_ID)
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ShiftConfigurationsDTO shiftConfigurationsDTO = GetShiftConfigurationsDTO(dataRow);
                    shiftConfigurationsDTOList.Add(shiftConfigurationsDTO);
                }
            }
            log.LogMethodExit(shiftConfigurationsDTOList);
            return shiftConfigurationsDTOList;
        }
    }
}
