/********************************************************************************************
 * Project Name - DisplayPanelThemeMap Data Handler
 * Description  - Data handler of the DisplayPanelThemeMap class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017   Lakshminarayana     Created 
 *2.70.2        30-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query                                                          
 *2.90        28-Jul-2020   Mushahid Faizan     Modified : default isActive value to true.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    ///  DisplayPanelThemeMap Data Handler - Handles insert, update and select of  DisplayPanelThemeMap objects
    /// </summary>
    public class DisplayPanelThemeMapDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM DisplayPanelThemeMap AS dpt";

        /// <summary>
        /// Dictionary for searching Parameters for the DisplayPanelThemeMap object.
        /// </summary>
        private static readonly Dictionary<DisplayPanelThemeMapDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DisplayPanelThemeMapDTO.SearchByParameters, string>
            {
                {DisplayPanelThemeMapDTO.SearchByParameters.ID, "dpt.Id"},
                {DisplayPanelThemeMapDTO.SearchByParameters.PANEL_ID, "dpt.PanelId"},
                {DisplayPanelThemeMapDTO.SearchByParameters.THEME_ID, "dpt.ThemeId"},
                {DisplayPanelThemeMapDTO.SearchByParameters.SCHEDULE_ID, "dpt.ScheduleId"},
                {DisplayPanelThemeMapDTO.SearchByParameters.IS_ACTIVE, "dpt.IsActive"},
                {DisplayPanelThemeMapDTO.SearchByParameters.MASTER_ENTITY_ID,"dpt.MasterEntityId"},
                {DisplayPanelThemeMapDTO.SearchByParameters.SITE_ID, "dpt.site_id"}
            };

        /// <summary>
        ///  Default constructor of DisplayPanelThemeMapDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DisplayPanelThemeMapDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating displayPanelThemeMapDTO parameters Record.
        /// </summary>
        /// <param name="displayPanelThemeMapDTO">displayPanelThemeMapDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(DisplayPanelThemeMapDTO displayPanelThemeMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(displayPanelThemeMapDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", displayPanelThemeMapDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PanelId", displayPanelThemeMapDTO.PanelId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ThemeId", displayPanelThemeMapDTO.ThemeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScheduleId", displayPanelThemeMapDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (displayPanelThemeMapDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", displayPanelThemeMapDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the DisplayPanelThemeMap record to the database
        /// </summary>
        /// <param name="displayPanelThemeMapDTO">DisplayPanelThemeMapDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>DisplayPanelThemeMapDTO</returns>
        public DisplayPanelThemeMapDTO InsertDisplayPanelThemeMap(DisplayPanelThemeMapDTO displayPanelThemeMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(displayPanelThemeMapDTO, loginId, siteId);
            string query = @"INSERT INTO DisplayPanelThemeMap 
                                        ( 
                                            PanelId,
                                            ThemeId,
                                            ScheduleId,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            site_id,
                                            MasterEntityId,
                                            Guid
                                        ) 
                                VALUES 
                                        (
                                            @PanelId,
                                            @ThemeId,
                                            @ScheduleId,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId,
                                            NEWID()
                                        )SELECT * FROM DisplayPanelThemeMap WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(displayPanelThemeMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDisplayPanelThemeMapDTO(displayPanelThemeMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting DisplayPanelThemeMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(displayPanelThemeMapDTO);
            return displayPanelThemeMapDTO;
        }

        /// <summary>
        /// Updates the DisplayPanelThemeMap record
        /// </summary>
        /// <param name="displayPanelThemeMapDTO">DisplayPanelThemeMapDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>DisplayPanelThemeMapDTO</returns>
        public DisplayPanelThemeMapDTO UpdateDisplayPanelThemeMap(DisplayPanelThemeMapDTO displayPanelThemeMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(displayPanelThemeMapDTO, loginId, siteId);
            string query = @"UPDATE DisplayPanelThemeMap 
                             SET PanelId=@PanelId,
                                 ThemeId=@ThemeId,
                                 ScheduleId=@ScheduleId,
                                 IsActive=@IsActive,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdatedDate = GETDATE()
                                 --site_id=@site_id
                             WHERE Id = @Id
                             SELECT* FROM DisplayPanelThemeMap WHERE  Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(displayPanelThemeMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDisplayPanelThemeMapDTO(displayPanelThemeMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating displayPanelThemeMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(displayPanelThemeMapDTO);
            return displayPanelThemeMapDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="displayPanelThemeMapDTO">displayPanelThemeMapDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshDisplayPanelThemeMapDTO(DisplayPanelThemeMapDTO displayPanelThemeMapDTO, DataTable dt)
        {
            log.LogMethodEntry(displayPanelThemeMapDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                displayPanelThemeMapDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                displayPanelThemeMapDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                displayPanelThemeMapDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                displayPanelThemeMapDTO.Guid = dataRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["guid"]);
                displayPanelThemeMapDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                displayPanelThemeMapDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                displayPanelThemeMapDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to DisplayPanelThemeMapDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DisplayPanelThemeMapDTO</returns>
        private DisplayPanelThemeMapDTO GetDisplayPanelThemeMapDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DisplayPanelThemeMapDTO displayPanelThemeMapDTO = new DisplayPanelThemeMapDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["PanelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PanelId"]),
                                            dataRow["ThemeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ThemeId"]),
                                            dataRow["ScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScheduleId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : (dataRow["IsActive"].ToString() == "Y"? true: false),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(displayPanelThemeMapDTO);
            return displayPanelThemeMapDTO;
        }

        /// <summary>
        /// Gets the DisplayPanelThemeMap data of passed DisplayPanelThemeMap Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns DisplayPanelThemeMapDTO</returns>
        public DisplayPanelThemeMapDTO GetDisplayPanelThemeMapDTO(int id)
        {
            log.LogMethodEntry(id);
            DisplayPanelThemeMapDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE dpt.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetDisplayPanelThemeMapDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DisplayPanelThemeMapDTO matching the search criteria</returns>
        public List<DisplayPanelThemeMapDTO> GetDisplayPanelThemeMapDTOList(List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DisplayPanelThemeMapDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach(KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if(searchParameter.Key == DisplayPanelThemeMapDTO.SearchByParameters.ID || 
                            searchParameter.Key == DisplayPanelThemeMapDTO.SearchByParameters.MASTER_ENTITY_ID || 
                            searchParameter.Key == DisplayPanelThemeMapDTO.SearchByParameters.PANEL_ID ||
                            searchParameter.Key == DisplayPanelThemeMapDTO.SearchByParameters.THEME_ID ||
                            searchParameter.Key == DisplayPanelThemeMapDTO.SearchByParameters.SCHEDULE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == DisplayPanelThemeMapDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DisplayPanelThemeMapDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                list = new List<DisplayPanelThemeMapDTO>();
                foreach(DataRow dataRow in dataTable.Rows)
                {
                    DisplayPanelThemeMapDTO displayPanelThemeMapDTO = GetDisplayPanelThemeMapDTO(dataRow);
                    list.Add(displayPanelThemeMapDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
