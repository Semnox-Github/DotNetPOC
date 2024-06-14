/********************************************************************************************
 * Project Name - DashboardDefinitionDataHandler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        15-Oct-2019   Jagan Mohan          Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.User
{
    class DashboardDefinitionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM DashboardDefinition ";

        /// <summary>
        /// Dictionary for searching Parameters for the DashboardDefinitionDTO object.
        /// </summary>
        private static readonly Dictionary<DashboardDefinitionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DashboardDefinitionDTO.SearchByParameters, string>
        {
            { DashboardDefinitionDTO.SearchByParameters.DASHBOARD_DEF_ID,"DashboardDefId"},
            { DashboardDefinitionDTO.SearchByParameters.DASHBOARD_NAME,"DashboardName"},
            { DashboardDefinitionDTO.SearchByParameters.DASHBOARD_SCREEN_TITLE,"DashboardScreenTitle"},
            { DashboardDefinitionDTO.SearchByParameters.ISACTIVE,"IsActive"},
            { DashboardDefinitionDTO.SearchByParameters.SITE_ID,"site_id"},
            { DashboardDefinitionDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };
        /// <summary>
        /// Parameterized Constructor for DashboardDefinition.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public DashboardDefinitionDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DashboardDefinition Record.
        /// </summary>
        /// <param name="dashboardDefinitionDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(DashboardDefinitionDTO dashboardDefinitionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(dashboardDefinitionDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DashboardDefId", dashboardDefinitionDTO.DashboardDefId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DashboardName", dashboardDefinitionDTO.DashboardName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DashboardScreenTitle", dashboardDefinitionDTO.DashboardScreenTitle));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TitleAlignment", dashboardDefinitionDTO.TitleAlignment));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TitleFont", dashboardDefinitionDTO.TitleFont));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TotalRows", dashboardDefinitionDTO.TotalRows));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TotalCols", dashboardDefinitionDTO.TotalCols));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveDate", dashboardDefinitionDTO.EffectiveDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", dashboardDefinitionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", dashboardDefinitionDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Builds ProductKey DTO from the passed DataRow
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private DashboardDefinitionDTO GetDashboardDefinitionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DashboardDefinitionDTO DashboardDefinitionDTO = new DashboardDefinitionDTO(dataRow["DashboardDefId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DashboardDefId"]),
                                                dataRow["DashboardName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DashboardName"]),
                                                dataRow["DashboardScreenTitle"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DashboardScreenTitle"]),
                                                dataRow["TitleAlignment"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TitleAlignment"]),
                                                dataRow["TitleFont"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TitleFont"]),
                                                dataRow["TotalRows"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TotalRows"]),
                                                dataRow["TotalCols"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TotalCols"]),
                                                dataRow["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                );
            return DashboardDefinitionDTO;
        }

        /// <summary>
        /// Gets the DashboardDefinitionDTO data of passed DashboardDefId
        /// </summary>
        /// <param name="dashboardDefId"></param>
        /// <returns>Returns DashboardDefinitionDTO</returns>
        public DashboardDefinitionDTO GetDashboardDefinitionDTO(int dashboardDefId)
        {
            log.LogMethodEntry(dashboardDefId);
            DashboardDefinitionDTO result = null;
            string query = SELECT_QUERY + @" WHERE DashboardDefinition.DashboardDefId = @DashboardDefId";
            SqlParameter parameter = new SqlParameter("@DashboardDefId", dashboardDefId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetDashboardDefinitionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Refreshing the DashboardDefinitionDTO
        /// </summary>
        /// <param name="dashboardDefinitionDTO"></param>
        /// <param name="dt"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        private void RefreshDashboardDefinitionDTO(DashboardDefinitionDTO dashboardDefinitionDTO, DataTable dt, string userId, int siteId)
        {
            log.LogMethodEntry(dashboardDefinitionDTO, dt, userId, siteId);
            if (dt.Rows.Count > 0)
            {
                dashboardDefinitionDTO.DashboardDefId = Convert.ToInt32(dt.Rows[0]["DashboardDefId"]);
                dashboardDefinitionDTO.CreatedBy = userId;
                dashboardDefinitionDTO.CreationDate = String.IsNullOrEmpty(dt.Rows[0]["CreationDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                dashboardDefinitionDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                dashboardDefinitionDTO.LastUpdatedBy = userId;
                dashboardDefinitionDTO.LastUpdatedDate = String.IsNullOrEmpty(dt.Rows[0]["LastUpdatedDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdatedDate"]);
                dashboardDefinitionDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the DashboardDefinition Table. 
        /// </summary>
        /// <param name="dashboardDefinitionDTO"></param>
        /// <param name="userId"></param>
        /// <param name = "siteId" ></ param >
        /// <returns>Returns updated DashboardDefinitionDTO</returns>
        public DashboardDefinitionDTO Insert(DashboardDefinitionDTO dashboardDefinitionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(dashboardDefinitionDTO, userId, siteId);
            string query = @"INSERT INTO [dbo].[DashboardDefinition]
                            (
                            DashboardName,
                            DashboardScreenTitle,
                            TitleAlignment,
                            TitleFont,
                            TotalRows,
                            TotalCols,
                            EffectiveDate,
                            Guid,
                            site_id,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedUser,
                            LastUpdatedDate,
                            IsActive
                            )
                            VALUES
                            (
                            @DashboardName,
                            @DashboardScreenTitle,
                            @TitleAlignment,
                            @TitleFont,
                            @TotalRows,
                            @TotalCols,
                            @EffectiveDate,
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATA(),
                            @LastUpdatedUser,
                            GETDATA(),
                            @IsActive          
                            )
                            SELECT * FROM DashboardDefinition WHERE DashboardDefId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(dashboardDefinitionDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshDashboardDefinitionDTO(dashboardDefinitionDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting DashboardDefinitionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dashboardDefinitionDTO);
            return dashboardDefinitionDTO;
        }

        /// <summary>
        /// Update the record in the DashboardDefinition Table. 
        /// </summary>
        /// <param name="dashboardDefinitionDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated DashboardDefinitionDTO</returns>
        public DashboardDefinitionDTO Update(DashboardDefinitionDTO dashboardDefinitionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(dashboardDefinitionDTO, userId, siteId);
            string query = @"UPDATE [dbo].[DashboardDefinition]
                             SET
                            DashboardName = @DashboardName,
                            DashboardScreenTitle = @DashboardScreenTitle,
                            TitleAlignment = @TitleAlignment,
                            TitleFont = @TitleFont,
                            TotalRows = @TotalRows,
                            TotalCols = @TotalCols,
                            EffectiveDate = @EffectiveDate,
                            site_id = @site_id,
                            MasterEntityId = @MasterEntityId,
                            LastUpdatedUser = @LastUpdatedUser,
                            LastUpdatedDate = GETDATE(),
                            IsActive = @IsActive
                            WHERE DashboardDefId = @DashboardDefId
                            SELECT * FROM DashboardDefinition WHERE DashboardDefId = @DashboardDefId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(dashboardDefinitionDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshDashboardDefinitionDTO(dashboardDefinitionDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating DashboardDefinitionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dashboardDefinitionDTO);
            return dashboardDefinitionDTO;
        }

        /// <summary>
        /// Returns the List of DashboardDefinitionDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<DashboardDefinitionDTO> GetAllDashboardDefinitionDTOList(List<KeyValuePair<DashboardDefinitionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<DashboardDefinitionDTO> dashboardDefinitionDTOList = new List<DashboardDefinitionDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<DashboardDefinitionDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == DashboardDefinitionDTO.SearchByParameters.DASHBOARD_DEF_ID ||
                            searchParameter.Key == DashboardDefinitionDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DashboardDefinitionDTO.SearchByParameters.DASHBOARD_NAME
                                || searchParameter.Key == DashboardDefinitionDTO.SearchByParameters.DASHBOARD_SCREEN_TITLE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == DashboardDefinitionDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DashboardDefinitionDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                    DashboardDefinitionDTO dashboardDefinitionDTO = GetDashboardDefinitionDTO(dataRow);
                    dashboardDefinitionDTOList.Add(dashboardDefinitionDTO);
                }
            }
            log.LogMethodExit(dashboardDefinitionDTOList);
            return dashboardDefinitionDTOList;
        }
    }
}