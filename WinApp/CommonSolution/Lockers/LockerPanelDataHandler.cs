/********************************************************************************************
 * Project Name - Locker Panels Data Handler
 * Description  - Data handler of the locker panels class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        18-Apr-2017   Raghuveera          Created
 *2.70.2        19-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2      10-Dec-2019   Jinto Thomas         Removed siteid from update query  
 *2.100.0     31-Aug-2020   Mushahid Faizan      siteId changes in GetSQLParameters().
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Locker Panels Data Handler - Handles insert, update and select of locker panels objects
    /// </summary>
    public class LockerPanelDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM LockerPanels AS lp";

        /// <summary>
        /// Dictionary for searching Parameters for the Locker Panel object.
        /// </summary>
        private static readonly Dictionary<LockerPanelDTO.SearchByLockerPanelsParameters, string> DBSearchParameters = new Dictionary<LockerPanelDTO.SearchByLockerPanelsParameters, string>
            {
                {LockerPanelDTO.SearchByLockerPanelsParameters.PANEL_ID, "lp.PanelId"},
                {LockerPanelDTO.SearchByLockerPanelsParameters.PANEL_NAME, "lp.PanelName"},
                {LockerPanelDTO.SearchByLockerPanelsParameters.IS_ACTIVE, "lp.ActiveFlag"},
                {LockerPanelDTO.SearchByLockerPanelsParameters.ZONE_ID, "lp.ZoneId"},
                {LockerPanelDTO.SearchByLockerPanelsParameters.MASTER_ENTITY_ID,"lp.MasterEntityId"},
                {LockerPanelDTO.SearchByLockerPanelsParameters.SITE_ID, "lp.site_id"},
                {LockerPanelDTO.SearchByLockerPanelsParameters.Zone_ID_LIST, "lp.ZoneId"}
            };

        /// <summary>
        ///  Default constructor of LockerPanelsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerPanelDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LockerPanel parameters Record.
        /// </summary>
        /// <param name="lockerPanelDTO">lockerPanelDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(LockerPanelDTO lockerPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerPanelDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@panelId", lockerPanelDTO.PanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@panelName", string.IsNullOrEmpty(lockerPanelDTO.PanelName) ? DBNull.Value : (object)lockerPanelDTO.PanelName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@zoneId", lockerPanelDTO.ZoneId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sequencePrefix", string.IsNullOrEmpty(lockerPanelDTO.SequencePrefix) ? DBNull.Value : (object)lockerPanelDTO.SequencePrefix));
            parameters.Add(dataAccessHandler.GetSQLParameter("@numRows", lockerPanelDTO.NumRows == -1 ? DBNull.Value : (object)lockerPanelDTO.NumRows));
            parameters.Add(dataAccessHandler.GetSQLParameter("@numCols", lockerPanelDTO.NumCols == -1 ? DBNull.Value : (object)lockerPanelDTO.NumCols));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@activeFlag", lockerPanelDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", lockerPanelDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            return parameters;
        }

        /// <summary>
        /// Inserts the locker record to the database
        /// </summary>
        /// <param name="lockerPanelDTO">LockerPanelDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public LockerPanelDTO InsertLockerPanel(LockerPanelDTO lockerPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerPanelDTO, loginId, siteId);
            string insertLockerQuery = @"INSERT INTO [dbo].[LockerPanels]  
                                                        ( 
                                                          PanelName,
                                                          ZoneId,
                                                          SequencePrefix,
                                                          NumRows,
                                                          NumCols,
                                                          ActiveFlag,
                                                          LastUpdatedBy,
                                                          LastUpdatedDate,
                                                          site_id,
                                                          Guid,
                                                          MasterEntityId,
                                                          CreatedBy,
                                                          CreationDate
                                                        ) 
                                                values 
                                                        (    
                                                          @panelName,
                                                          @zoneId,
                                                          @sequencePrefix,
                                                          @numRows,
                                                          @numCols,
                                                          @activeFlag,
                                                          @lastUpdatedBy,
                                                          getdate(),
                                                          @siteId,
                                                          NewId(),
                                                          @masterEntityId,
                                                          @createdBy,
                                                          getdate()
                                                        )SELECT * FROM LockerPanels WHERE PanelId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertLockerQuery, GetSQLParameters(lockerPanelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerPanelDTO(lockerPanelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LockerPanelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerPanelDTO);
            return lockerPanelDTO;
        }

        /// <summary>
        /// Updates the locker panel record
        /// </summary>
        /// <param name="lockerPanelDTO">LockerPanelDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public LockerPanelDTO UpdateLocker(LockerPanelDTO lockerPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lockerPanelDTO, loginId, siteId);
            string updateLockerQuery = @"update LockerPanels 
                                         set PanelName = @panelName,
                                             ZoneId = @zoneId,
                                             SequencePrefix = @sequencePrefix,
                                             NumRows = @numRows,
                                             NumCols = @numCols,
                                             ActiveFlag = @activeFlag,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             -- site_id = @siteid,
                                             MasterEntityId = @masterEntityId
                                       where PanelId = @panelId 
                                       SELECT * FROM LockerPanels WHERE  PanelId = @panelId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateLockerQuery, GetSQLParameters(lockerPanelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLockerPanelDTO(lockerPanelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LockerPanelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lockerPanelDTO);
            return lockerPanelDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="lockerPanelDTO">lockerPanelDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshLockerPanelDTO(LockerPanelDTO lockerPanelDTO, DataTable dt)
        {
            log.LogMethodEntry(lockerPanelDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                lockerPanelDTO.PanelId = Convert.ToInt32(dt.Rows[0]["PanelId"]);
                lockerPanelDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                lockerPanelDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                lockerPanelDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                lockerPanelDTO.LastUpdatedUserId = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                lockerPanelDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                lockerPanelDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to LockerPanelsDTO class type
        /// </summary>
        /// <param name="lockerPanelsDataRow">LockerPanels DataRow</param>
        /// <returns>Returns LockerPanels</returns>
        private LockerPanelDTO GetLockerPanelDTO(DataRow lockerPanelsDataRow)
        {
            log.LogMethodEntry(lockerPanelsDataRow);
            LockerPanelDTO lockerPanelsDataObject = new LockerPanelDTO(Convert.ToInt32(lockerPanelsDataRow["PanelId"]),
                                            lockerPanelsDataRow["PanelName"].ToString(),
                                            lockerPanelsDataRow["ZoneId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerPanelsDataRow["ZoneId"]),
                                            lockerPanelsDataRow["SequencePrefix"].ToString(),
                                            lockerPanelsDataRow["NumRows"] == DBNull.Value ? -1 : Convert.ToInt32(lockerPanelsDataRow["NumRows"]),
                                            lockerPanelsDataRow["NumCols"] == DBNull.Value ? -1 : Convert.ToInt32(lockerPanelsDataRow["NumCols"]),
                                            lockerPanelsDataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(lockerPanelsDataRow["ActiveFlag"]),
                                            lockerPanelsDataRow["LastUpdatedBy"].ToString(),
                                            lockerPanelsDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerPanelsDataRow["LastupdatedDate"]),
                                            lockerPanelsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(lockerPanelsDataRow["site_id"]),
                                            lockerPanelsDataRow["Guid"].ToString(),
                                            lockerPanelsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(lockerPanelsDataRow["SynchStatus"]),
                                            lockerPanelsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(lockerPanelsDataRow["MasterEntityId"]),
                                            lockerPanelsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(lockerPanelsDataRow["CreatedBy"]),
                                            lockerPanelsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lockerPanelsDataRow["CreationDate"])
                                            );
            log.LogMethodExit(lockerPanelsDataObject);
            return lockerPanelsDataObject;
        }

        /// <summary>
        /// Gets the locker panels which matches the locker panel id
        /// </summary>
        /// <param name="panelId">integer type parameter</param>
        /// <returns>Returns LockerPanelsDTO</returns>
        public LockerPanelDTO GetLockerPanel(int panelId)
        {
            log.LogMethodEntry(panelId);
            LockerPanelDTO result = null;
            string selectLockerPanelsQuery = SELECT_QUERY + @" WHERE PanelId = @panelId and ActiveFlag = 1";
            SqlParameter parameter = new SqlParameter("@panelId", panelId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectLockerPanelsQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetLockerPanelDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the locker panels which matches the locker zone Id
        /// </summary>
        /// <param name="zoneId">integer type parameter</param>
        /// <param name="identifierFrom">integer type parameter locker numeber from</param>
        /// <param name="identifierTo">integer type parameter locker numeber to</param>
        /// <param name="loadInactiveChildRecord">Load inactive child record</param>
        /// <returns>Returns list LockerPanelsDTO</returns>
        public List<LockerPanelDTO> GetLockerPanelsByZoneId(int zoneId, int identifierFrom, int identifierTo, bool loadInactiveChildRecord)
        {
            log.LogMethodEntry(zoneId, identifierFrom, identifierTo, loadInactiveChildRecord);
            List<KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>> searchParameters = new List<KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>>();
            if (!loadInactiveChildRecord)
                searchParameters.Add(new KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>(LockerPanelDTO.SearchByLockerPanelsParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>(LockerPanelDTO.SearchByLockerPanelsParameters.ZONE_ID, zoneId.ToString()));
            log.LogMethodExit(GetLockerPanelsList(searchParameters, identifierFrom, identifierTo, loadInactiveChildRecord));
            return GetLockerPanelsList(searchParameters, identifierFrom, identifierTo, loadInactiveChildRecord);
        }

        /// <summary>
        /// Gets the LockerPanelsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="identifierFrom">identifier from</param>
        /// <param name="identifierTo">identifier to</param>
        /// <param name="loadInactiveChildRecord">Load inactive child record</param>
        /// <returns>Returns the list of LockerPanelsDTO matching the search criteria</returns>
        public List<LockerPanelDTO> GetLockerPanelsList(List<KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>> searchParameters, int identifierFrom, int identifierTo, bool loadInactiveChildRecord)
        {
            log.LogMethodEntry(searchParameters, identifierFrom, identifierTo, loadInactiveChildRecord);
            List<LockerPanelDTO> lockerPanelDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.PANEL_ID
                            || searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.ZONE_ID
                            || searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));// ? 'Y' : 'N'));
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
                lockerPanelDTOList = new List<LockerPanelDTO>();
                LockerDataHandler lockerDataHandler = new LockerDataHandler();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LockerPanelDTO lockerPanelDTO = GetLockerPanelDTO(dataRow);
                    lockerPanelDTO.LockerDTOList = lockerDataHandler.GetLockers(identifierFrom, identifierTo, lockerPanelDTO.PanelId, loadInactiveChildRecord);
                    lockerPanelDTOList.Add(lockerPanelDTO);
                }
            }
            log.LogMethodExit(lockerPanelDTOList);
            return lockerPanelDTOList;
        }

        /// <summary>
        /// Gets the LockerPanelsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="loadChildren">Load child items</param>
        /// <returns>Returns the list of LockerPanelsDTO matching the search criteria</returns>
        public List<LockerPanelDTO> GetLockerPanelsList(List<KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>> searchParameters, bool loadChildren)
        {
            log.LogMethodEntry(searchParameters, loadChildren);
            List<LockerPanelDTO> lockerPanelDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.PANEL_ID
                            || searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.ZONE_ID
                            || searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));// ? 'Y' : 'N'));
                        }
                        else if (searchParameter.Key == LockerPanelDTO.SearchByLockerPanelsParameters.Zone_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                lockerPanelDTOList = new List<LockerPanelDTO>();
                LockerDataHandler lockerDataHandler = new LockerDataHandler();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LockerPanelDTO lockerPanelDTO = GetLockerPanelDTO(dataRow);
                    if (loadChildren)
                        lockerPanelDTO.LockerDTOList = lockerDataHandler.GetLockers(-1, -1, lockerPanelDTO.PanelId);
                    lockerPanelDTOList.Add(lockerPanelDTO);
                }
            }
            log.LogMethodExit(lockerPanelDTOList);
            return lockerPanelDTOList;
        }
    }
}
