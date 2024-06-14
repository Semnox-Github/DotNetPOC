/********************************************************************************************
 * Project Name - Screen Zone Content Map Data Handler
 * Description  - Data handler of the Screen Zone Content Map Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        07-Mar-2017   Raghuveera          Created 
 *2.70.2        31-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor
 *2.70.2       06-Dec-2019   Jinto Thomas         Removed siteid from update query 
 *2.100.0         13-Aug-2020   Mushahid Faizan     Modified : Added GetScreenZoneContentMapDTOList() and changed default isActive value to true.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Screen Zone Content Map Data Handler  - Handles insert, update and select of screen zone content map data handler
    /// </summary>
    public class ScreenZoneContentMapDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ScreenZoneContentMap AS sz";

        /// <summary>
        /// Dictionary for searching Parameters for the ScreenZoneContentMap object.
        /// </summary>
        private static readonly Dictionary<ScreenZoneContentMapDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ScreenZoneContentMapDTO.SearchByParameters, string>
            {
                {ScreenZoneContentMapDTO.SearchByParameters.ZONE_ID, "sz.ZoneID"},
                {ScreenZoneContentMapDTO.SearchByParameters.CONTENT_ID, "sz.ContentID"},
                {ScreenZoneContentMapDTO.SearchByParameters.CONTENT_TYPE, "sz.ContentType"},
                {ScreenZoneContentMapDTO.SearchByParameters.CONTENT_TYPE_ID, "sz.ContentTypeID"},
                {ScreenZoneContentMapDTO.SearchByParameters.SCREEN_CONTENT_ID, "sz.ScreenContentID"},
                {ScreenZoneContentMapDTO.SearchByParameters.IS_ACTIVE, "sz.Active_Flag"},
                {ScreenZoneContentMapDTO.SearchByParameters.SITE_ID, "sz.site_id"},
                {ScreenZoneContentMapDTO.SearchByParameters.MASTER_ENTITY_ID, "sz.MasterEntityId"}
            };

        /// <summary>
        /// Default constructor of ScreenZoneContentMapDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScreenZoneContentMapDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating screenZoneContentMapDTO parameters Record.
        /// </summary>
        /// <param name="screenZoneContentMapDTO">screenZoneContentMapDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(ScreenZoneContentMapDTO screenZoneContentMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenZoneContentMapDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@screenContentId", screenZoneContentMapDTO.ScreenContentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@zoneId", screenZoneContentMapDTO.ZoneId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentTypeId", screenZoneContentMapDTO.ContentTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentType", string.IsNullOrEmpty(screenZoneContentMapDTO.ContentType) ? DBNull.Value : (object)screenZoneContentMapDTO.ContentType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentId", screenZoneContentMapDTO.ContentId == -1 ? DBNull.Value : (object)screenZoneContentMapDTO.ContentId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@backImage", screenZoneContentMapDTO.BackImage == -1 ? DBNull.Value : (object)screenZoneContentMapDTO.BackImage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@backColor", string.IsNullOrEmpty(screenZoneContentMapDTO.BackColor) ? DBNull.Value : (object)screenZoneContentMapDTO.BackColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@borderSize", string.IsNullOrEmpty(screenZoneContentMapDTO.BorderSize) ? DBNull.Value : (object)screenZoneContentMapDTO.BorderSize));
            parameters.Add(dataAccessHandler.GetSQLParameter("@borderColor", string.IsNullOrEmpty(screenZoneContentMapDTO.BorderColor) ? DBNull.Value : (object)screenZoneContentMapDTO.BorderColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@imgSize", screenZoneContentMapDTO.ImgSize == -1 ? DBNull.Value : (object)screenZoneContentMapDTO.ImgSize));
            parameters.Add(dataAccessHandler.GetSQLParameter("@imgAlignment", string.IsNullOrEmpty(screenZoneContentMapDTO.ImgAlignment) ? DBNull.Value : (object)screenZoneContentMapDTO.ImgAlignment));
            parameters.Add(dataAccessHandler.GetSQLParameter("@imgRefreshSecs", screenZoneContentMapDTO.ImgRefreshSecs == -1 ? DBNull.Value : (object)screenZoneContentMapDTO.ImgRefreshSecs));
            parameters.Add(dataAccessHandler.GetSQLParameter("@videoRepeat", string.IsNullOrEmpty(screenZoneContentMapDTO.VideoRepeat) ? DBNull.Value : (object)screenZoneContentMapDTO.VideoRepeat));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lookupRefreshSecs", screenZoneContentMapDTO.LookupRefreshSecs == -1 ? DBNull.Value : (object)screenZoneContentMapDTO.LookupRefreshSecs));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lookupHeaderDisplay", string.IsNullOrEmpty(screenZoneContentMapDTO.LookupHeaderDisplay) ? DBNull.Value : (object)screenZoneContentMapDTO.LookupHeaderDisplay));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tickerScrollDirection", screenZoneContentMapDTO.TickerScrollDirection == -1 ? DBNull.Value : (object)screenZoneContentMapDTO.TickerScrollDirection));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tickerSpeed", screenZoneContentMapDTO.TickerSpeed == -1 ? DBNull.Value : (object)screenZoneContentMapDTO.TickerSpeed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tickerRefreshSecs", screenZoneContentMapDTO.TickerRefreshSecs == -1 ? DBNull.Value : (object)screenZoneContentMapDTO.TickerRefreshSecs));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayOrder", screenZoneContentMapDTO.DisplayOrder == -1 ? DBNull.Value : (object)screenZoneContentMapDTO.DisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", screenZoneContentMapDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active_Flag", (screenZoneContentMapDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentGuid", string.IsNullOrEmpty(screenZoneContentMapDTO.ContentGuid)? DBNull.Value : (object)screenZoneContentMapDTO.ContentGuid));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the screen zone content map record to the database
        /// </summary>
        /// <param name="screenZoneContentMapDTO">ScreenZoneContentMapDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>ScreenZoneContentMap DTO</returns>
        public ScreenZoneContentMapDTO InsertScreenZoneContentMap(ScreenZoneContentMapDTO screenZoneContentMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenZoneContentMapDTO, loginId, siteId, sqlTransaction);
            string insertScreenZoneContentMapQuery = @"insert into ScreenZoneContentMap 
                                                        ( 
                                                        ZoneID,
                                                        ContentTypeID,
                                                        ContentType,
                                                        ContentID,
                                                        BackImage,
                                                        BackColor,
                                                        BorderSize,
                                                        BorderColor,
                                                        ImgSize,
                                                        ImgAlignment,
                                                        ImgRefreshSecs,
                                                        VideoRepeat,
                                                        LookupRefreshSecs,
                                                        LookupHeaderDisplay,
                                                        TickerScrollDirection,
                                                        TickerSpeed,
                                                        TickerRefreshSecs,
                                                        DisplayOrder,
                                                        Active_Flag,
                                                        CreatedUser,
                                                        Creationdate,
                                                        last_updated_user,
                                                        last_updated_date,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        ContentGuid
                                                        ) 
                                                values 
                                                        (                                                         
                                                         @zoneID,
                                                         @contentTypeID,
                                                         @contentType,
                                                         @contentID,
                                                         @backImage,
                                                         @backColor,
                                                         @borderSize,
                                                         @borderColor,
                                                         @imgSize,
                                                         @imgAlignment,
                                                         @imgRefreshSecs,
                                                         @videoRepeat,
                                                         @lookupRefreshSecs,
                                                         @lookupHeaderDisplay,
                                                         @tickerScrollDirection,
                                                         @tickerSpeed,
                                                         @tickerRefreshSecs,
                                                         @displayOrder,
                                                         @active_Flag,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId,
                                                         @contentGuid
                                                        )SELECT * FROM ScreenZoneContentMap WHERE ScreenContentID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertScreenZoneContentMapQuery, GetSQLParameters(screenZoneContentMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScreenZoneContentMapDTO(screenZoneContentMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting screenZoneContentMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(screenZoneContentMapDTO);
            return screenZoneContentMapDTO;
        }

        /// <summary>
        /// Updates the Screen Zone Content Map record
        /// </summary>
        /// <param name="screenZoneContentMapDTO">ScreenZoneContentMapDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>ScreenZoneContentMap DTO</returns>
        public ScreenZoneContentMapDTO UpdateScreenZoneContentMap(ScreenZoneContentMapDTO screenZoneContentMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenZoneContentMapDTO, loginId, siteId, sqlTransaction);
            string updateScreenZoneContentMapQuery = @"update ScreenZoneContentMap 
                                         set ZoneID = @zoneID,
                                             ContentTypeID = @contentTypeID,
                                             ContentType = @contentType,
                                             ContentID = @contentID,
                                             BackImage = @backImage,
                                             BackColor = @backColor,
                                             BorderSize = @borderSize,
                                             BorderColor = @borderColor,
                                             ImgSize = @imgSize,
                                             ImgAlignment = @imgAlignment,
                                             ImgRefreshSecs = @imgRefreshSecs,
                                             VideoRepeat = @videoRepeat,
                                             LookupRefreshSecs = @lookupRefreshSecs,
                                             LookupHeaderDisplay = @lookupHeaderDisplay,
                                             TickerScrollDirection = @tickerScrollDirection,
                                             TickerSpeed = @tickerSpeed,
                                             TickerRefreshSecs = @tickerRefreshSecs,
                                             DisplayOrder = @displayOrder,
                                             Active_Flag = @active_Flag,
                                             last_updated_user = @lastUpdatedBy, 
                                             last_updated_date = Getdate(),
                                             --site_id = @siteid,
                                             MasterEntityId = @masterEntityId,
                                             ContentGuid = @contentGuid
                                       where ScreenContentID = @screenContentID
                                       SELECT * FROM ScreenZoneContentMap WHERE ScreenContentID = @screenContentID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateScreenZoneContentMapQuery, GetSQLParameters(screenZoneContentMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScreenZoneContentMapDTO(screenZoneContentMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating screenZoneContentMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(screenZoneContentMapDTO);
            return screenZoneContentMapDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="screenZoneContentMapDTO">screenZoneContentMapDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshScreenZoneContentMapDTO(ScreenZoneContentMapDTO screenZoneContentMapDTO, DataTable dt)
        {
            log.LogMethodEntry(screenZoneContentMapDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                screenZoneContentMapDTO.ScreenContentId = Convert.ToInt32(dt.Rows[0]["ScreenContentId"]);
                screenZoneContentMapDTO.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                screenZoneContentMapDTO.CreationDate = dataRow["Creationdate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Creationdate"]);
                screenZoneContentMapDTO.Guid = dataRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["guid"]);
                screenZoneContentMapDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                screenZoneContentMapDTO.CreatedBy = dataRow["CreatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedUser"]);
                screenZoneContentMapDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ScreenZoneContentMapDTO class type
        /// </summary>
        /// <param name="screenZoneContentMapDataRow">ScreenZoneContentMap DataRow</param>
        /// <returns>Returns ScreenZoneContentMap</returns>
        private ScreenZoneContentMapDTO GetScreenZoneContentMapDTO(DataRow screenZoneContentMapDataRow)
        {
            log.LogMethodEntry(screenZoneContentMapDataRow);
            ScreenZoneContentMapDTO screenZoneContentMapDataObject = new ScreenZoneContentMapDTO(Convert.ToInt32(screenZoneContentMapDataRow["ScreenContentID"]),
                                            screenZoneContentMapDataRow["ZoneID"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneContentMapDataRow["ZoneID"]),
                                            screenZoneContentMapDataRow["ContentTypeID"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneContentMapDataRow["ContentTypeID"]),
                                            screenZoneContentMapDataRow["ContentType"].ToString(),
                                            screenZoneContentMapDataRow["ContentID"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneContentMapDataRow["ContentID"]),
                                            screenZoneContentMapDataRow["BackImage"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneContentMapDataRow["BackImage"]),
                                            screenZoneContentMapDataRow["BackColor"].ToString(),
                                            screenZoneContentMapDataRow["BorderSize"].ToString(),
                                            screenZoneContentMapDataRow["BorderColor"].ToString(),
                                            screenZoneContentMapDataRow["ImgSize"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneContentMapDataRow["ImgSize"]),
                                            screenZoneContentMapDataRow["ImgAlignment"].ToString(),
                                            screenZoneContentMapDataRow["ImgRefreshSecs"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneContentMapDataRow["ImgRefreshSecs"]),
                                            screenZoneContentMapDataRow["VideoRepeat"] == DBNull.Value ? "N" : screenZoneContentMapDataRow["VideoRepeat"].ToString(),
                                            screenZoneContentMapDataRow["LookupRefreshSecs"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneContentMapDataRow["LookupRefreshSecs"]),
                                            screenZoneContentMapDataRow["LookupHeaderDisplay"] == DBNull.Value ? "N" : screenZoneContentMapDataRow["LookupHeaderDisplay"].ToString(),
                                            screenZoneContentMapDataRow["TickerScrollDirection"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneContentMapDataRow["TickerScrollDirection"]),
                                            screenZoneContentMapDataRow["TickerSpeed"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneContentMapDataRow["TickerSpeed"]),
                                            screenZoneContentMapDataRow["TickerRefreshSecs"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneContentMapDataRow["TickerRefreshSecs"]),
                                            screenZoneContentMapDataRow["DisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneContentMapDataRow["DisplayOrder"]),
                                            screenZoneContentMapDataRow["Active_Flag"] == DBNull.Value ? true : (screenZoneContentMapDataRow["Active_Flag"].ToString() == "Y"? true: false),
                                            screenZoneContentMapDataRow["CreatedUser"].ToString(),
                                            screenZoneContentMapDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(screenZoneContentMapDataRow["CreationDate"]),
                                            screenZoneContentMapDataRow["last_updated_user"].ToString(),
                                            screenZoneContentMapDataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(screenZoneContentMapDataRow["last_updated_date"]),
                                            screenZoneContentMapDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneContentMapDataRow["site_id"]),
                                            screenZoneContentMapDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneContentMapDataRow["MasterEntityId"]),
                                            screenZoneContentMapDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(screenZoneContentMapDataRow["SynchStatus"]),
                                            screenZoneContentMapDataRow["Guid"].ToString(),
                                            screenZoneContentMapDataRow["ContentGuid"].ToString()
                                            );
            log.LogMethodExit(screenZoneContentMapDataObject);
            return screenZoneContentMapDataObject;
        }

        /// <summary>
        /// Gets the Screen Zone Content Map data of passed asset asset Group Id
        /// </summary>
        /// <param name="screenZoneContentMapId">integer type parameter</param>
        /// <returns>Returns ScreenZoneContentMapDTO</returns>
        public ScreenZoneContentMapDTO GetScreenZoneContentMap(int screenZoneContentMapId)
        {
            log.LogMethodEntry(screenZoneContentMapId);
            string selectScreenZoneContentMapQuery = SELECT_QUERY + @" WHERE sz.ScreenContentID = @screenContentID";
            SqlParameter[] selectScreenZoneContentMapParameters = new SqlParameter[1];
            selectScreenZoneContentMapParameters[0] = new SqlParameter("@screenContentID", screenZoneContentMapId);
            DataTable screenZoneContentMap = dataAccessHandler.executeSelectQuery(selectScreenZoneContentMapQuery, selectScreenZoneContentMapParameters, sqlTransaction);
            if (screenZoneContentMap.Rows.Count > 0)
            {
                DataRow ScreenZoneContentMapRow = screenZoneContentMap.Rows[0];
                ScreenZoneContentMapDTO screenZoneContentMapDataObject = GetScreenZoneContentMapDTO(ScreenZoneContentMapRow);
                log.LogMethodExit(screenZoneContentMapDataObject);
                return screenZoneContentMapDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the ScreenZoneContentMapDTO List for Zone Id List
        /// </summary>
        /// <param name="zoneIdList">integer list parameter</param>
        /// <returns>Returns List of ScreenZoneContentMapDTO</returns>
        public List<ScreenZoneContentMapDTO> GetScreenZoneContentMapDTOList(List<int> zoneIdList, bool activeRecords)
        {
            log.LogMethodEntry(zoneIdList);
            List<ScreenZoneContentMapDTO> list = new List<ScreenZoneContentMapDTO>();
            string query = @"SELECT ScreenZoneContentMap.*
                            FROM ScreenZoneContentMap, @zoneIdList List
                            WHERE ZoneID = List.Id ";
            if (activeRecords)
            {
                query += " AND Active_Flag = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@zoneIdList", zoneIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetScreenZoneContentMapDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the ScreenZoneContentMapDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScreenZoneContentMapDTO matching the search criteria</returns>
        public List<ScreenZoneContentMapDTO> GetScreenZoneContentMapList(List<KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<ScreenZoneContentMapDTO> screenZoneContentMapList = null;
            List <SqlParameter> parameters = new List<SqlParameter>();
            string selectScreenZoneContentMapQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScreenZoneContentMapDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == ScreenZoneContentMapDTO.SearchByParameters.SCREEN_CONTENT_ID
                            || searchParameter.Key == ScreenZoneContentMapDTO.SearchByParameters.CONTENT_TYPE_ID
                            || searchParameter.Key == ScreenZoneContentMapDTO.SearchByParameters.CONTENT_ID
                            || searchParameter.Key == ScreenZoneContentMapDTO.SearchByParameters.ZONE_ID
                            || searchParameter.Key == ScreenZoneContentMapDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScreenZoneContentMapDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScreenZoneContentMapDTO.SearchByParameters.IS_ACTIVE)
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
                if (searchParameters.Count > 0)
                    selectScreenZoneContentMapQuery = selectScreenZoneContentMapQuery + query;
            }
            DataTable screenZoneContentMapData = dataAccessHandler.executeSelectQuery(selectScreenZoneContentMapQuery, parameters.ToArray(), sqlTransaction);
            if (screenZoneContentMapData.Rows.Count > 0)
            {
                screenZoneContentMapList = new List<ScreenZoneContentMapDTO>();
                foreach (DataRow screenZoneContentMapDataRow in screenZoneContentMapData.Rows)
                {
                    ScreenZoneContentMapDTO screenZoneContentMapDataObject = GetScreenZoneContentMapDTO(screenZoneContentMapDataRow);
                    screenZoneContentMapList.Add(screenZoneContentMapDataObject);
                }
                
            }
            log.LogMethodExit(screenZoneContentMapList);
            return screenZoneContentMapList;
        }
    }
}
