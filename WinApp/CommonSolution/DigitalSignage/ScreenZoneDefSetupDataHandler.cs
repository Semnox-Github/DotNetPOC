/********************************************************************************************
 * Project Name - Screen Zone Def Setup Data Handler
 * Description  - Data handler of the Screen Zone Def Setup Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        09-Mar-2017   Raghuveera          Created 
 *2.70.2        31-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query 
 *2.100.0         13-Aug-2020   Mushahid Faizan     Modified : Added GetScreenZoneDefSetupDTOList() and changed default isActive value to true.
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
    /// Screen Zone Def Setup Data Handler  - Handles insert, update and select of screen zone def setup data handler
    /// </summary>
    public class ScreenZoneDefSetupDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ScreenZoneDefSetup AS szd";

        /// <summary>
        /// Dictionary for searching Parameters for the screen zone def setup object.
        /// </summary>
        private static readonly Dictionary<ScreenZoneDefSetupDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ScreenZoneDefSetupDTO.SearchByParameters, string>
            {
                {ScreenZoneDefSetupDTO.SearchByParameters.ZONE_ID, "szd.ZoneID"},
                {ScreenZoneDefSetupDTO.SearchByParameters.SCREEN_ID, "szd.ScreenId"},
                {ScreenZoneDefSetupDTO.SearchByParameters.NAME, "szd.Name"},
                {ScreenZoneDefSetupDTO.SearchByParameters.IS_ACTIVE, "szd.Active_Flag"},
                {ScreenZoneDefSetupDTO.SearchByParameters.SITE_ID, "szd.site_id"},
                {ScreenZoneDefSetupDTO.SearchByParameters.MASTER_ENTITY_ID, "szd.MasterEntityId"}
            };

        /// <summary>
        /// Default constructor of ScreenZoneDefSetupDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScreenZoneDefSetupDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating screenZoneDefSetupDTO parameters Record.
        /// </summary>
        /// <param name="screenZoneDefSetupDTO">screenZoneDefSetupDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(ScreenZoneDefSetupDTO screenZoneDefSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenZoneDefSetupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@zoneId", screenZoneDefSetupDTO.ZoneId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@screenId", screenZoneDefSetupDTO.ScreenId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(screenZoneDefSetupDTO.Name) ? DBNull.Value : (object)screenZoneDefSetupDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@topLeft", screenZoneDefSetupDTO.TopLeft == 0 ? DBNull.Value : (object)screenZoneDefSetupDTO.TopLeft));
            parameters.Add(dataAccessHandler.GetSQLParameter("@bottomRight", screenZoneDefSetupDTO.BottomRight == 0 ? DBNull.Value : (object)screenZoneDefSetupDTO.BottomRight));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(screenZoneDefSetupDTO.Description) ? DBNull.Value : (object)screenZoneDefSetupDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayOrder", screenZoneDefSetupDTO.DisplayOrder == -1 ? DBNull.Value : (object)screenZoneDefSetupDTO.DisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@topOffsetY", screenZoneDefSetupDTO.TopOffsetY == 0 ? DBNull.Value : (object)screenZoneDefSetupDTO.TopOffsetY));
            parameters.Add(dataAccessHandler.GetSQLParameter("@bottomOffsetY", screenZoneDefSetupDTO.BottomOffsetY == 0 ? DBNull.Value : (object)screenZoneDefSetupDTO.BottomOffsetY));
            parameters.Add(dataAccessHandler.GetSQLParameter("@leftOffsetX", screenZoneDefSetupDTO.LeftOffsetX == 0 ? DBNull.Value : (object)screenZoneDefSetupDTO.LeftOffsetX));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rightOffsetX", screenZoneDefSetupDTO.RightOffsetX == 0 ? DBNull.Value : (object)screenZoneDefSetupDTO.RightOffsetX));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active_Flag", (screenZoneDefSetupDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", screenZoneDefSetupDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the screen zone def setup record to the database
        /// </summary>
        /// <param name="screenZoneDefSetupDTO">ScreenZoneDefSetupDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>Returns Screen Zone Def Setup DTO</returns>
        public ScreenZoneDefSetupDTO InsertScreenZoneDefSetup(ScreenZoneDefSetupDTO screenZoneDefSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenZoneDefSetupDTO, loginId, siteId, sqlTransaction);
            string insertScreenZoneDefSetupQuery = @"insert into ScreenZoneDefSetup 
                                                        ( 
                                                        ScreenID,
                                                        Name,
                                                        TopLeft,
                                                        BottomRight,
                                                        Description,
                                                        DisplayOrder,
                                                        TopOffsetY,
                                                        BottomOffsetY,
                                                        LeftOffsetX,
                                                        RightOffsetX,
                                                        Active_Flag,
                                                        CreatedUser,
                                                        Creationdate,
                                                        last_updated_user,
                                                        last_updated_date,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                         @screenID,
                                                         @name,
                                                         @topLeft,
                                                         @bottomRight,
                                                         @description,
                                                         @displayOrder,
                                                         @topOffsetY,
                                                         @bottomOffsetY,
                                                         @leftOffsetX,
                                                         @rightOffsetX,
                                                         @active_Flag,
                                                         @createdBy,
                                                         Getdate(),
                                                         @lastUpdatedBy,
                                                         Getdate(),
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId
                                                        )SELECT * FROM ScreenZoneDefSetup WHERE ZoneId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertScreenZoneDefSetupQuery, GetSQLParameters(screenZoneDefSetupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScreenZoneDefSetupDTO(screenZoneDefSetupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting screenZoneDefSetupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(screenZoneDefSetupDTO);
            return screenZoneDefSetupDTO;
        }

        /// <summary>
        /// Updates the Screen Zone Def Setup record
        /// </summary>
        /// <param name="screenZoneDefSetupDTO">ScreenZoneDefSetupDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>Returns Screen Zone Def Setup DTO</returns>
        public ScreenZoneDefSetupDTO UpdateScreenZoneDefSetup(ScreenZoneDefSetupDTO screenZoneDefSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenZoneDefSetupDTO, loginId, siteId, sqlTransaction);
            string updateScreenZoneDefSetupQuery = @"update ScreenZoneDefSetup 
                                         set Name = @name,
                                             ScreenID=@screenID,
                                             TopLeft=@topLeft,
                                             BottomRight=@bottomRight,
                                             Description=@description,
                                             DisplayOrder=@displayOrder,
                                             TopOffsetY=@topOffsetY,
                                             BottomOffsetY=@bottomOffsetY,
                                             LeftOffsetX=@leftOffsetX,
                                             RightOffsetX=@rightOffsetX,                                             
                                             Active_Flag = @active_Flag,
                                             last_updated_user = @lastUpdatedBy, 
                                             last_updated_date = Getdate(),
                                             --site_id = @siteid,
                                             MasterEntityId = @masterEntityId                                            
                                       where ZoneId = @zoneId
                                       SELECT * FROM ScreenZoneDefSetup WHERE ZoneId = @zoneId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateScreenZoneDefSetupQuery, GetSQLParameters(screenZoneDefSetupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScreenZoneDefSetupDTO(screenZoneDefSetupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating screenZoneDefSetupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(screenZoneDefSetupDTO);
            return screenZoneDefSetupDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="screenZoneContentMapDTO">screenZoneContentMapDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshScreenZoneDefSetupDTO(ScreenZoneDefSetupDTO screenZoneDefSetupDTO, DataTable dt)
        {
            log.LogMethodEntry(screenZoneDefSetupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                screenZoneDefSetupDTO.ZoneId = Convert.ToInt32(dt.Rows[0]["ZoneId"]);
                screenZoneDefSetupDTO.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                screenZoneDefSetupDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                screenZoneDefSetupDTO.Guid = dataRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["guid"]);
                screenZoneDefSetupDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                screenZoneDefSetupDTO.CreatedBy = dataRow["CreatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedUser"]);
                screenZoneDefSetupDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ScreenZoneDefSetupDTO class type
        /// </summary>
        /// <param name="screenZoneDefSetupDataRow">ScreenZoneDefSetup DataRow</param>
        /// <returns>Returns ScreenZoneDefSetup</returns>
        private ScreenZoneDefSetupDTO GetScreenZoneDefSetupDTO(DataRow screenZoneDefSetupDataRow)
        {
            log.LogMethodEntry(screenZoneDefSetupDataRow);
            ScreenZoneDefSetupDTO screenZoneDefSetupDataObject = new ScreenZoneDefSetupDTO(Convert.ToInt32(screenZoneDefSetupDataRow["ZoneID"]),
                                            screenZoneDefSetupDataRow["ScreenID"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneDefSetupDataRow["ScreenID"]),
                                            screenZoneDefSetupDataRow["Name"].ToString(),
                                            screenZoneDefSetupDataRow["TopLeft"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneDefSetupDataRow["TopLeft"]),
                                            screenZoneDefSetupDataRow["BottomRight"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneDefSetupDataRow["BottomRight"]),
                                            screenZoneDefSetupDataRow["Description"].ToString(),
                                            screenZoneDefSetupDataRow["DisplayOrder"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneDefSetupDataRow["DisplayOrder"]),
                                            screenZoneDefSetupDataRow["TopOffsetY"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneDefSetupDataRow["TopOffsetY"]),
                                            screenZoneDefSetupDataRow["BottomOffsetY"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneDefSetupDataRow["BottomOffsetY"]),
                                            screenZoneDefSetupDataRow["LeftOffsetX"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneDefSetupDataRow["LeftOffsetX"]),
                                            screenZoneDefSetupDataRow["RightOffsetX"] == DBNull.Value ? 0 : Convert.ToInt32(screenZoneDefSetupDataRow["RightOffsetX"]),
                                            screenZoneDefSetupDataRow["Active_Flag"] == DBNull.Value ? true : (screenZoneDefSetupDataRow["Active_Flag"].ToString() == "Y"? true: false), 
                                            screenZoneDefSetupDataRow["CreatedUser"].ToString(),
                                            screenZoneDefSetupDataRow["Creationdate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(screenZoneDefSetupDataRow["Creationdate"]),
                                            screenZoneDefSetupDataRow["last_updated_user"].ToString(),
                                            screenZoneDefSetupDataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(screenZoneDefSetupDataRow["last_updated_date"]),
                                            screenZoneDefSetupDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneDefSetupDataRow["site_id"]),
                                            screenZoneDefSetupDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(screenZoneDefSetupDataRow["MasterEntityId"]),
                                            screenZoneDefSetupDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(screenZoneDefSetupDataRow["SynchStatus"]),
                                            screenZoneDefSetupDataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(screenZoneDefSetupDataObject);
            return screenZoneDefSetupDataObject;
        }

        /// <summary>
        /// Gets the Screen Zone Def Setup data of passed asset asset Group Id
        /// </summary>
        /// <param name="zoneID">integer type parameter</param>
        /// <returns>Returns ScreenZoneDefSetupDTO</returns>
        public ScreenZoneDefSetupDTO GetScreenZoneDefSetup(int zoneID)
        {
            log.LogMethodEntry(zoneID);
            string selectScreenZoneDefSetupQuery = SELECT_QUERY + @" WHERE  szd.ZoneID = @zoneID";
            SqlParameter[] selectScreenZoneDefSetupParameters = new SqlParameter[1];
            selectScreenZoneDefSetupParameters[0] = new SqlParameter("@zoneID", zoneID);
            DataTable screenZoneDefSetup = dataAccessHandler.executeSelectQuery(selectScreenZoneDefSetupQuery, selectScreenZoneDefSetupParameters, sqlTransaction);
            if (screenZoneDefSetup.Rows.Count > 0)
            {
                DataRow ScreenZoneDefSetupRow = screenZoneDefSetup.Rows[0];
                ScreenZoneDefSetupDTO screenZoneDefSetupDataObject = GetScreenZoneDefSetupDTO(ScreenZoneDefSetupRow);
                log.LogMethodExit(screenZoneDefSetupDataObject);
                return screenZoneDefSetupDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the ScreenZoneDefSetupDTO List for screen Id List
        /// </summary>
        /// <param name="screenIdList">integer list parameter</param>
        /// <returns>Returns List of DSignageLookupValuesDTOList</returns>
        public List<ScreenZoneDefSetupDTO> GetScreenZoneDefSetupDTOList(List<int> screenIdList, bool activeRecords)
        {
            log.LogMethodEntry(screenIdList);
            List<ScreenZoneDefSetupDTO> list = new List<ScreenZoneDefSetupDTO>();
            string query = @"SELECT ScreenZoneDefSetup.*
                            FROM ScreenZoneDefSetup, @screenIdList List
                            WHERE ScreenId = List.Id ";
            if (activeRecords)
            {
                query += " AND Active_Flag = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@screenIdList", screenIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetScreenZoneDefSetupDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the ScreenZoneDefSetupDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScreenZoneDefSetupDTO matching the search criteria</returns>
        public List<ScreenZoneDefSetupDTO> GetScreenZoneDefSetupList(List<KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<ScreenZoneDefSetupDTO> screenZoneDefSetupList = null;
            string selectScreenZoneDefSetupQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == ScreenZoneDefSetupDTO.SearchByParameters.SCREEN_ID
                            || searchParameter.Key == ScreenZoneDefSetupDTO.SearchByParameters.ZONE_ID
                            || searchParameter.Key == ScreenZoneDefSetupDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScreenZoneDefSetupDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScreenZoneDefSetupDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')= " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                    selectScreenZoneDefSetupQuery = selectScreenZoneDefSetupQuery + query;
            }
            DataTable screenZoneDefSetupData = dataAccessHandler.executeSelectQuery(selectScreenZoneDefSetupQuery, parameters.ToArray(), sqlTransaction);
            if (screenZoneDefSetupData.Rows.Count > 0)
            {
                screenZoneDefSetupList = new List<ScreenZoneDefSetupDTO>();
                foreach (DataRow screenZoneDefSetupDataRow in screenZoneDefSetupData.Rows)
                {
                    ScreenZoneDefSetupDTO screenZoneDefSetupDataObject = GetScreenZoneDefSetupDTO(screenZoneDefSetupDataRow);
                    screenZoneDefSetupList.Add(screenZoneDefSetupDataObject);
                }
                
            }
            log.LogMethodExit(screenZoneDefSetupList);
            return screenZoneDefSetupList;
        }
    }
}
