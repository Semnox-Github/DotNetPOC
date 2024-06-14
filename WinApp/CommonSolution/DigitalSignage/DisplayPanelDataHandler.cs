/********************************************************************************************
 * Project Name - DisplayPanel Data Handler
 * Description  - Data handler of the DisplayPanel class
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
    ///  DisplayPanel Data Handler - Handles insert, update and select of  DisplayPanel objects
    /// </summary>
    public class DisplayPanelDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM DisplayPanel AS dpl";

        /// <summary>
        /// Dictionary for searching Parameters for the DisplayPanel object.
        /// </summary>
        private static readonly Dictionary<DisplayPanelDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DisplayPanelDTO.SearchByParameters, string>
            {
                {DisplayPanelDTO.SearchByParameters.PANEL_ID, "dpl.PanelID"},
                {DisplayPanelDTO.SearchByParameters.PANEL_NAME, "dpl.PanelName"},
                {DisplayPanelDTO.SearchByParameters.PC_NAME, "dpl.PCName"},
                {DisplayPanelDTO.SearchByParameters.PC_NAME_EXACT, "dpl.PCName"},
                {DisplayPanelDTO.SearchByParameters.DISPLAY_GROUP, "dpl.Display_Group"},
                {DisplayPanelDTO.SearchByParameters.MAC_ADDRESS , "dpl.MACAddress"},
                {DisplayPanelDTO.SearchByParameters.IS_ACTIVE, "dpl.Active"},
                {DisplayPanelDTO.SearchByParameters.MASTER_ENTITY_ID,"dpl.MasterEntityId"},
                {DisplayPanelDTO.SearchByParameters.SITE_ID, "dpl.site_id"}
            };

        /// <summary>
        /// Default constructor of DisplayPanelDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DisplayPanelDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating displayPanelDTO parameters Record.
        /// </summary>
        /// <param name="displayPanelDTO">displayPanelDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(DisplayPanelDTO displayPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(displayPanelDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@panelId", displayPanelDTO.PanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PanelName", string.IsNullOrEmpty(displayPanelDTO.PanelName) ? DBNull.Value : (object)displayPanelDTO.PanelName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PCName", string.IsNullOrEmpty(displayPanelDTO.PCName) ? DBNull.Value : (object)displayPanelDTO.PCName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Display_Group", string.IsNullOrEmpty(displayPanelDTO.DisplayGroup) ? DBNull.Value : (object)displayPanelDTO.DisplayGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MACAddress", string.IsNullOrEmpty(displayPanelDTO.MACAddress) ? DBNull.Value : (object)displayPanelDTO.MACAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Location", string.IsNullOrEmpty(displayPanelDTO.Location) ? DBNull.Value : (object)displayPanelDTO.Location));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", string.IsNullOrEmpty(displayPanelDTO.Description) ? DBNull.Value : (object)displayPanelDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartTime", displayPanelDTO.StartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EndTime", displayPanelDTO.EndTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ResolutionX", displayPanelDTO.ResolutionX, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ResolutionY", displayPanelDTO.ResolutionY, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LocalFolder", displayPanelDTO.LocalFolder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RestartFlag", displayPanelDTO.RestartFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastRestartTime", displayPanelDTO.LastRestartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ShutdownSec", displayPanelDTO.ShutdownSec));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Active", (displayPanelDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_update_user", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", displayPanelDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the DisplayPanel record to the database
        /// </summary>
        /// <param name="displayPanelDTO">DisplayPanelDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>DisplayPanelDTO</returns>
        public DisplayPanelDTO InsertDisplayPanel(DisplayPanelDTO displayPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(displayPanelDTO, loginId, siteId);
            string query = @"INSERT INTO DisplayPanel 
                                        ( 
                                            PanelName,
                                            PCName,
                                            Display_Group,
                                            Location,
                                            MACAddress,
                                            Active,
                                            Description,
                                            last_update_date,
                                            last_update_user,
                                            StartTime,
                                            EndTime,
                                            ResolutionX,
                                            ResolutionY,
                                            CreationDate,
                                            CreatedUser,
                                            site_id,
                                            guid,
                                            LocalFolder,
                                            RestartFlag,
                                            LastRestartTime,
                                            ShutdownSec,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @PanelName,
                                            @PCName,
                                            @Display_Group,
                                            @Location,
                                            @MACAddress,
                                            @Active,
                                            @Description,
                                            GETDATE(),
                                            @last_update_user,
                                            @StartTime,
                                            @EndTime,
                                            @ResolutionX,
                                            @ResolutionY,
                                            GETDATE(),
                                            @CreatedUser,
                                            @site_id,
                                            NEWID(),
                                            @LocalFolder,
                                            @RestartFlag,
                                            @LastRestartTime,
                                            @ShutdownSec,
                                            @MasterEntityId
                                        )SELECT * FROM DisplayPanel WHERE PanelID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(displayPanelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDisplayPanelDTO(displayPanelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting displayPanelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(displayPanelDTO);
            return displayPanelDTO;
        }

        /// <summary>
        /// Updates the DisplayPanel record
        /// </summary>
        /// <param name="displayPanelDTO">DisplayPanelDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>DisplayPanelDTO</returns>
        public DisplayPanelDTO UpdateDisplayPanel(DisplayPanelDTO displayPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(displayPanelDTO, loginId, siteId);
            string query = @"UPDATE DisplayPanel 
                             SET PanelName=@PanelName,
                                 PCName=@PCName,
                                 Display_Group=@Display_Group,
                                 Location=@Location,
                                 MACAddress=@MACAddress,
                                 Active=@Active,
                                 Description=@Description,
                                 last_update_date = GETDATE(),
                                 last_update_user=@last_update_user,
                                 StartTime=@StartTime,
                                 EndTime=@EndTime,
                                 ResolutionX=@ResolutionX,
                                 ResolutionY=@ResolutionY,
                                 --site_id=@site_id,
                                 LocalFolder=@LocalFolder,
                                 RestartFlag=@RestartFlag,
                                 LastRestartTime=@LastRestartTime,
                                 ShutdownSec=@ShutdownSec
                             WHERE PanelID = @PanelID
                             SELECT * FROM DisplayPanel WHERE PanelID = @PanelID ";

            try
            {
                if (string.Equals(displayPanelDTO.IsActive, "N") && GetDisplayPanelReferenceCount(displayPanelDTO.PanelId) > 0)
                {
                    throw new ForeignKeyException("Cannot Inactivate records for which matching detail data exists.");
                }
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(displayPanelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDisplayPanelDTO(displayPanelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating displayPanelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(displayPanelDTO);
            return displayPanelDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="displayPanelDTO">displayPanelDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshDisplayPanelDTO(DisplayPanelDTO displayPanelDTO, DataTable dt)
        {
            log.LogMethodEntry(displayPanelDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                displayPanelDTO.PanelId = Convert.ToInt32(dt.Rows[0]["PanelId"]);
                displayPanelDTO.LastUpdateDate = dataRow["last_update_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_date"]);
                displayPanelDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                displayPanelDTO.Guid = dataRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["guid"]);
                displayPanelDTO.LastUpdatedBy = dataRow["last_update_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_update_user"]);
                displayPanelDTO.CreatedBy = dataRow["CreatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedUser"]);
                displayPanelDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether displayPanel is in use.
        /// <param name="id">Panel Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        private int GetDisplayPanelReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT Count(*) as ReferenceCount
                             FROM DisplayPanelThemeMap
                             WHERE PanelId = @PanelId AND IsActive = 'Y'";
            SqlParameter parameter = new SqlParameter("@PanelId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        /// <summary>
        /// Converts the Data row object to DisplayPanelDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DisplayPanelDTO</returns>
        private DisplayPanelDTO GetDisplayPanelDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DisplayPanelDTO displayPanelDTO = new DisplayPanelDTO(Convert.ToInt32(dataRow["PanelID"]),
                                            dataRow["PanelName"] == DBNull.Value ? "" : dataRow["PanelName"].ToString(),
                                            dataRow["PCName"] == DBNull.Value ? "" : dataRow["PCName"].ToString(),
                                            dataRow["Display_Group"] == DBNull.Value ? "" : dataRow["Display_Group"].ToString(),
                                            dataRow["Location"] == DBNull.Value ? "" : dataRow["Location"].ToString(),
                                            dataRow["MACAddress"] == DBNull.Value ? "" : dataRow["MACAddress"].ToString(),
                                            dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                                            dataRow["StartTime"] == DBNull.Value ? new decimal(12.0) : Convert.ToDecimal(dataRow["StartTime"]),
                                            dataRow["EndTime"] == DBNull.Value ? new decimal(12.0) : Convert.ToDecimal(dataRow["EndTime"]),
                                            dataRow["ResolutionX"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ResolutionX"]),
                                            dataRow["ResolutionY"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ResolutionY"]),
                                            dataRow["LocalFolder"] == DBNull.Value ? "" : dataRow["LocalFolder"].ToString(),
                                            dataRow["RestartFlag"] == DBNull.Value ? "N" : dataRow["RestartFlag"].ToString(),
                                            dataRow["LastRestartTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastRestartTime"]),
                                            dataRow["ShutdownSec"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ShutdownSec"]),
                                            dataRow["Active"] == DBNull.Value ? true : (dataRow["Active"].ToString() == "Y" ? true : false),
                                            dataRow["CreatedUser"] == DBNull.Value ? "" : dataRow["CreatedUser"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["last_update_user"] == DBNull.Value ? "" : dataRow["last_update_user"].ToString(),
                                            dataRow["last_update_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_date"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["guid"] == DBNull.Value ? "" : dataRow["guid"].ToString()
                                            );
            log.LogMethodExit(displayPanelDTO);
            return displayPanelDTO;
        }

        /// <summary>
        /// Gets the DisplayPanel data of passed DisplayPanel Id
        /// </summary>
        /// <param name="panelID">integer type parameter</param>
        /// <returns>Returns DisplayPanelDTO</returns>
        public DisplayPanelDTO GetDisplayPanelDTO(int panelID)
        {
            log.LogMethodEntry(panelID);
            DisplayPanelDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE dpl.PanelID = @PanelID";
            SqlParameter parameter = new SqlParameter("@PanelID", panelID);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetDisplayPanelDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DisplayPanelDTO matching the search criteria</returns>
        public List<DisplayPanelDTO> GetDisplayPanelDTOList(List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DisplayPanelDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<DisplayPanelDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == DisplayPanelDTO.SearchByParameters.PANEL_ID ||
                            searchParameter.Key == DisplayPanelDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DisplayPanelDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DisplayPanelDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == DisplayPanelDTO.SearchByParameters.PC_NAME_EXACT)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
            if (dataTable.Rows.Count > 0)
            {
                list = new List<DisplayPanelDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DisplayPanelDTO displayPanelDTO = GetDisplayPanelDTO(dataRow);
                    list.Add(displayPanelDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        internal DateTime? GetDisplayPanelModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(last_update_date) last_update_date
                            FROM (
                            select max(last_update_date) last_update_date from DisplayPanel WHERE (site_id = @siteId or @siteId = -1)
                             ) modefierSetlastupdate";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["last_update_date"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["last_update_date"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}


