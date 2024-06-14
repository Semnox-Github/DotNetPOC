/********************************************************************************************
 * Project Name - Ticker Data Handler
 * Description  - Data handler of the Ticker class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017   Lakshminarayana     Created 
 *2.70.2        31-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query 
 *2.100        29-Jul-2020   Mushahid Faizan     Modified : default isActive value to true.
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
    ///  Ticker Data Handler - Handles insert, update and select of  Ticker objects
    /// </summary>
    public class TickerDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Ticker as tic";

        /// <summary>
        /// Dictionary for searching Parameters for the Ticker object.
        /// </summary>
        private static readonly Dictionary<TickerDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TickerDTO.SearchByParameters, string>
            {
                {TickerDTO.SearchByParameters.TICKER_ID, "tic.TickerID"},
                {TickerDTO.SearchByParameters.NAME, "tic.Name"},
                {TickerDTO.SearchByParameters.IS_ACTIVE, "tic.Active_Flag"},
                {TickerDTO.SearchByParameters.MASTER_ENTITY_ID,"tic.MasterEntityId"},
                {TickerDTO.SearchByParameters.SITE_ID, "tic.site_id"}
            };

        /// <summary>
        /// Default constructor of TickerDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public TickerDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating screenTransitionsDTO parameters Record.
        /// </summary>
        /// <param name="tickerDTO">tickerDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(TickerDTO tickerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tickerDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TickerId", tickerDTO.TickerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", string.IsNullOrEmpty(tickerDTO.Name) ? DBNull.Value : (object)tickerDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TickerText", string.IsNullOrEmpty(tickerDTO.TickerText) ? DBNull.Value : (object)tickerDTO.TickerText));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TextColor", string.IsNullOrEmpty(tickerDTO.TextColor) ? DBNull.Value : (object)tickerDTO.TextColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScrollDirection", tickerDTO.ScrollDirection,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Font", string.IsNullOrEmpty(tickerDTO.Font) ? DBNull.Value : (object)tickerDTO.Font));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BackColor", string.IsNullOrEmpty(tickerDTO.BackColor) ? DBNull.Value : (object)tickerDTO.BackColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TickerSpeed", tickerDTO.TickerSpeed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Active_Flag", (tickerDTO.IsActive == true? "Y":"N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_updated_user", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", tickerDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Ticker record to the database
        /// </summary>
        /// <param name="tickerDTO">TickerDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns TickerDTO</returns>
        public TickerDTO InsertTicker(TickerDTO tickerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tickerDTO, loginId, siteId);
            string query = @"INSERT INTO Ticker 
                                        ( 
                                            Name,
                                            TickerText,
                                            Active_Flag,
                                            ScrollDirection,
                                            TextColor,
                                            Font,
                                            BackColor,
                                            TickerSpeed,
                                            site_id,
                                            guid,
                                            last_updated_user,
                                            last_updated_date,
                                            CreationDate,
                                            CreatedUser,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @Name,
                                            @TickerText,
                                            @Active_Flag,
                                            @ScrollDirection,
                                            @TextColor,
                                            @Font,
                                            @BackColor,
                                            @TickerSpeed,
                                            @site_id,
                                            NEWID(),
                                            @last_updated_user,
                                            GETDATE(),
                                            GETDATE(),
                                            @CreatedUser,
                                            @MasterEntityId
                                        )SELECT * FROM Ticker WHERE TickerId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(tickerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTickerDTO(tickerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting tickerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tickerDTO);
            return tickerDTO;
        }

        /// <summary>
        /// Updates the Ticker record
        /// </summary>
        /// <param name="tickerDTO">TickerDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns TickerDTO</returns>
        public TickerDTO UpdateTicker(TickerDTO tickerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tickerDTO, loginId, siteId);
            string query = @"UPDATE Ticker 
                             SET Name=@Name,
                                 TickerText=@TickerText,
                                 ScrollDirection=@ScrollDirection,
                                 TextColor=@TextColor,
                                 Font=@Font,  
                                 BackColor=@BackColor,
                                 TickerSpeed=@TickerSpeed,   
                                 Active_Flag=@Active_Flag,
                                 last_updated_user=@last_updated_user,
                                 last_updated_date = GETDATE()
                                 --site_id=@site_id
                             WHERE TickerId = @TickerId
                             SELECT * FROM Ticker WHERE TickerId = @TickerId";
            try
            {
                if (string.Equals(tickerDTO.IsActive, "N") && GetTickerReferenceCount(tickerDTO.TickerId) > 0)
                {
                    throw new ForeignKeyException("Cannot Inactivate records for which matching detail data exists.");
                }
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(tickerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTickerDTO(tickerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating tickerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tickerDTO);
            return tickerDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="tickerDTO">tickerDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshTickerDTO(TickerDTO tickerDTO, DataTable dt)
        {
            log.LogMethodEntry(tickerDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                tickerDTO.TickerId = Convert.ToInt32(dt.Rows[0]["TickerId"]);
                tickerDTO.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                tickerDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                tickerDTO.Guid = dataRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["guid"]);
                tickerDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                tickerDTO.CreatedBy = dataRow["CreatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedUser"]);
                tickerDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether ticker is in use.
        /// <param name="id">Ticker Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        private int GetTickerReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT COUNT(*) AS ReferenceCount
                             FROM ScreenZoneContentMap
                             WHERE ContentType='TICKER' AND Active_Flag = 'Y' AND ContentID = @TickerId";
            SqlParameter parameter = new SqlParameter("@TickerId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        /// <summary>
        /// Converts the Data row object to TickerDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns TickerDTO</returns>
        private TickerDTO GetTickerDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TickerDTO tickerDTO = new TickerDTO(Convert.ToInt32(dataRow["TickerId"]),
                                            dataRow["Name"] == DBNull.Value ? "" : dataRow["Name"].ToString(),
                                            dataRow["TickerText"] == DBNull.Value ? "" : dataRow["TickerText"].ToString(),
                                            dataRow["TextColor"] == DBNull.Value ? "" : dataRow["TextColor"].ToString(),
                                            dataRow["ScrollDirection"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScrollDirection"]),
                                            dataRow["Font"] == DBNull.Value ? "" : dataRow["Font"].ToString(),
                                            dataRow["BackColor"] == DBNull.Value ? "" : dataRow["BackColor"].ToString(),
                                            dataRow["TickerSpeed"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["TickerSpeed"]),
                                            dataRow["Active_Flag"] == DBNull.Value ? true : (dataRow["Active_Flag"].ToString() == "Y"? true: false),
                                            dataRow["CreatedUser"] == DBNull.Value ? "" : dataRow["CreatedUser"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["last_updated_user"] == DBNull.Value ? "" : dataRow["last_updated_user"].ToString(),
                                            dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["guid"] == DBNull.Value ? "" : dataRow["guid"].ToString()
                                            );
            log.LogMethodExit(tickerDTO);
            return tickerDTO;
        }

        /// <summary>
        /// Gets the Ticker data of passed Ticker Id
        /// </summary>
        /// <param name="tickerId">integer type parameter</param>
        /// <returns>Returns TickerDTO</returns>
        public TickerDTO GetTickerDTO(int tickerId)
        {
            log.LogMethodEntry(tickerId);
            TickerDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE tic.TickerId = @TickerId";
            SqlParameter parameter = new SqlParameter("@TickerId", tickerId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetTickerDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the Ticker data of passed contentGuid
        /// </summary>
        /// <param name="contentGuid">string type parameter</param>
        /// <returns>Returns TickerDTO</returns>
        public TickerDTO GetTickerDTOByGuid(string contentGuid)
        {
            log.LogMethodEntry(contentGuid);
            TickerDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE tic.guid = @ContentGuid";
            SqlParameter parameter = new SqlParameter("@ContentGuid", contentGuid);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetTickerDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the TickerDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TickerDTO matching the search criteria</returns>
        public List<TickerDTO> GetTickerDTOList(List<KeyValuePair<TickerDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<TickerDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach(KeyValuePair<TickerDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if(searchParameter.Key == TickerDTO.SearchByParameters.TICKER_ID ||
                            searchParameter.Key == TickerDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == TickerDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == TickerDTO.SearchByParameters.IS_ACTIVE)
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
                list = new List<TickerDTO>();
                foreach(DataRow dataRow in dataTable.Rows)
                {
                    TickerDTO tickerDTO = GetTickerDTO(dataRow);
                    list.Add(tickerDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }   
}
