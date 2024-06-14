/********************************************************************************************
 * Project Name - Turnstile Data Handler
 * Description  - Data handler of the Turnstile class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        01-Jul-2019   Girish Kundar       Modified : For SQL Injection Issue.  
 *                                                         Added missed Columns to Insert/Update
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query    
 *2.90         09-Jun-2020   Mushahid Faizan     Modified : 3 Tier changes for Rest API.   
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Turnstile
{
    public class TurnstileDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM Turnstiles AS t ";
        private static readonly Dictionary<TurnstileDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TurnstileDTO.SearchByParameters, string>
            {
                {TurnstileDTO.SearchByParameters.TURNSTILE_ID, "t.TurnstileId"},
                {TurnstileDTO.SearchByParameters.TURNSTILE_NAME, "t.TurnstileName"},
                {TurnstileDTO.SearchByParameters.ACTIVE, "t.Active"},
                {TurnstileDTO.SearchByParameters.TYPE, "t.Type"},
                {TurnstileDTO.SearchByParameters.MAKE, "t.Make"},
                {TurnstileDTO.SearchByParameters.MODEL, "t.Model"},
                {TurnstileDTO.SearchByParameters.GAME_PROFILE_ID, "t.GameProfileId"},
                {TurnstileDTO.SearchByParameters.MASTER_ENTITY_ID, "t.MasterEntityId"},
                {TurnstileDTO.SearchByParameters.SITE_ID, "t.site_id"}
             };

        /// <summary>
        /// Default constructor of TurnstileDataHandler class
        /// </summary>
        public TurnstileDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CheckIns Record.
        /// </summary>
        /// <param name="turnstileDTO">TurnstileDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>SqlParameter</returns>
        private List<SqlParameter> GetSQLParameters(TurnstileDTO turnstileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(turnstileDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@turnstileId", turnstileDTO.TurnstileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameProfileId", turnstileDTO.GameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", turnstileDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@turnstileName", string.IsNullOrEmpty(turnstileDTO.TurnstileName) ? DBNull.Value : (object)turnstileDTO.TurnstileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@passageAAlias", string.IsNullOrEmpty(turnstileDTO.PassageAAlias) ? DBNull.Value : (object)turnstileDTO.PassageAAlias));
            parameters.Add(dataAccessHandler.GetSQLParameter("@passageBAlias", string.IsNullOrEmpty(turnstileDTO.PassageBAlias) ? DBNull.Value : (object)turnstileDTO.PassageBAlias));
            parameters.Add(dataAccessHandler.GetSQLParameter("@iPAddress", string.IsNullOrEmpty(turnstileDTO.IPAddress) ? DBNull.Value : (object)turnstileDTO.IPAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@portNumber", string.IsNullOrEmpty(turnstileDTO.PortNumber.ToString()) || turnstileDTO.PortNumber == -1 ? DBNull.Value : (object)turnstileDTO.PortNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@type", turnstileDTO.Type == -1 ? DBNull.Value : (object)turnstileDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@make", turnstileDTO.Make == -1 ? DBNull.Value : (object)turnstileDTO.Make));
            parameters.Add(dataAccessHandler.GetSQLParameter("@model", turnstileDTO.Model == -1 ? DBNull.Value : (object)turnstileDTO.Model));
            parameters.Add(dataAccessHandler.GetSQLParameter("@useRSProtocol", turnstileDTO.UseRSProtocol));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", turnstileDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Turnstile record to the database
        /// </summary>
        /// <param name="turnstileDTO">TurnstileDTO type object</param>
        /// <param name="loginId">updated user id number</param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns TurnstileDTO</returns>
        public TurnstileDTO InsertTurnstile(TurnstileDTO turnstileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(turnstileDTO, loginId, siteId);
            string query = @"insert into Turnstiles
                                                         (
                                                            TurnstileName,
                                                            PassageAAlias,
                                                            PassageBAlias,
                                                            IPAddress,
                                                            PortNumber,
                                                            UseRSProtocol,
                                                            Active,
                                                            Type,
                                                            Make,
                                                            Model,
                                                            GameProfileId,
                                                            LastUpdatedDate,
                                                            LastUpdatedUser,
                                                            Guid,
                                                            MasterEntityId,
                                                            site_id,
                                                            CreatedBy,
                                                            CreationDate
                                                         )
                                                       values
                                                         (
                                                            @turnstileName,
                                                            @passageAAlias,
                                                            @passageBAlias,
                                                            @iPAddress,
                                                            @portNumber,
                                                            @useRSProtocol,
                                                            @active,
                                                            @type,
                                                            @make,
                                                            @model,
                                                            @gameProfileId,
                                                            GetDate(),
                                                            @lastUpdatedUser,
                                                            NewId(),
                                                            @masterEntityId,
                                                            @siteId,
                                                            @createdBy,
                                                            GETDATE() 
                                                        )SELECT * FROM Turnstiles WHERE TurnstileId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(turnstileDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTurnstileDTO(turnstileDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting turnstileDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(turnstileDTO);
            return turnstileDTO;
        }

        /// <summary>
        /// Updates the TurnstileDTO record to the database
        /// </summary>
        /// <param name="turnstileDTO">TurnstileDTO type object</param>
        /// <returns>Returns the TurnstileDTO</returns>
        public TurnstileDTO UpdateTurnstile(TurnstileDTO turnstileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(turnstileDTO, loginId, siteId);
            string query = @"update Turnstiles
                                                     set
                                                            TurnstileName = @turnstileName,
                                                            PassageAAlias = @passageAAlias,
                                                            PassageBAlias = @passageBAlias,
                                                            IPAddress = @iPAddress,
                                                            PortNumber = @portNumber,
                                                            UseRSProtocol =  @useRSProtocol,
                                                            Active = @active,
                                                            Type = @type,
                                                            Make = @make,
                                                            Model = @model,
                                                            GameProfileId = @gameProfileId,
                                                            LastUpdatedDate = GetDate(),
                                                            LastUpdatedUser = @lastUpdatedUser,
                                                            MasterEntityId = @masterEntityId
                                                            --site_id = @siteId
                                                Where TurnstileId = @turnstileId
                                               SELECT * FROM Turnstiles WHERE TurnstileId = @turnstileId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(turnstileDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTurnstileDTO(turnstileDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting turnstileDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(turnstileDTO);
            return turnstileDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="TurnstileDTO">TurnstileDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshTurnstileDTO(TurnstileDTO turnstileDTO, DataTable dt)
        {
            log.LogMethodEntry(turnstileDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                turnstileDTO.TurnstileId = Convert.ToInt32(dt.Rows[0]["TurnstileId"]);
                turnstileDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                turnstileDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                turnstileDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                turnstileDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                turnstileDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                turnstileDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to TurnstileDTO class type
        /// </summary>
        /// <param name="turnstileDTODataRow">TurnstileDTO DataRow</param>
        /// <returns>Returns TurnstileDTO</returns>
        private TurnstileDTO GetTurnstileDTO(DataRow turnstileDTODataRow)
        {
            log.LogMethodEntry(turnstileDTODataRow);
            TurnstileDTO turnstileDTO = new TurnstileDTO(
                                Convert.ToInt32(turnstileDTODataRow["TurnstileId"]),
                                turnstileDTODataRow["TurnstileName"] == DBNull.Value ? string.Empty : Convert.ToString(turnstileDTODataRow["TurnstileName"]),
                                turnstileDTODataRow["PassageAAlias"] == DBNull.Value ? string.Empty : Convert.ToString(turnstileDTODataRow["PassageAAlias"]),
                                turnstileDTODataRow["PassageBAlias"] == DBNull.Value ? string.Empty : Convert.ToString(turnstileDTODataRow["PassageBAlias"]),
                                turnstileDTODataRow["IPAddress"] == DBNull.Value ? string.Empty : Convert.ToString(turnstileDTODataRow["IPAddress"]),
                                turnstileDTODataRow["PortNumber"] == DBNull.Value ? (int?)null : Convert.ToInt32(turnstileDTODataRow["PortNumber"]),
                                turnstileDTODataRow["UseRSProtocol"] == DBNull.Value ? false : Convert.ToBoolean(turnstileDTODataRow["UseRSProtocol"]),
                                turnstileDTODataRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(turnstileDTODataRow["Active"]),
                                turnstileDTODataRow["Type"] == DBNull.Value ? -1 : Convert.ToInt32(turnstileDTODataRow["Type"]),
                                turnstileDTODataRow["Make"] == DBNull.Value ? -1 : Convert.ToInt32(turnstileDTODataRow["Make"]),
                                turnstileDTODataRow["Model"] == DBNull.Value ? -1 : Convert.ToInt32(turnstileDTODataRow["Model"]),
                                turnstileDTODataRow["GameProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(turnstileDTODataRow["GameProfileId"]),
                                turnstileDTODataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(turnstileDTODataRow["LastupdatedDate"]),
                                turnstileDTODataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(turnstileDTODataRow["LastUpdatedUser"]),
                                turnstileDTODataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(turnstileDTODataRow["Guid"]),
                                turnstileDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(turnstileDTODataRow["SynchStatus"]),
                                turnstileDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(turnstileDTODataRow["MasterEntityId"]),
                                turnstileDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(turnstileDTODataRow["site_id"]),
                                turnstileDTODataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(turnstileDTODataRow["CreatedBy"]),
                                turnstileDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(turnstileDTODataRow["CreationDate"]));

            log.LogMethodExit(turnstileDTO);
            return turnstileDTO;
        }


        /// <summary>
        /// Gets the TurnstileDTO data of passed achievementClassId
        /// </summary>
        /// <param name="turnstileId">integer type parameter</param>
        /// <returns>Returns TurnstileDTO</returns>
        public TurnstileDTO GetTurnstileDTO(int turnstileId)
        {
            log.LogMethodEntry(turnstileId);
            string selectTurnstileDTOQuery = SELECT_QUERY + "    where t.TurnstileId = @turnstileId";
            SqlParameter[] selectTurnstileDTOParameters = new SqlParameter[1];
            selectTurnstileDTOParameters[0] = new SqlParameter("@turnstileId", turnstileId);
            DataTable selectTurnstileDTO = dataAccessHandler.executeSelectQuery(selectTurnstileDTOQuery, selectTurnstileDTOParameters, sqlTransaction);
            TurnstileDTO turnstileDTO = new TurnstileDTO();
            if (selectTurnstileDTO.Rows.Count > 0)
            {
                DataRow achievementClassRow = selectTurnstileDTO.Rows[0];
                turnstileDTO = GetTurnstileDTO(achievementClassRow);
            }
            log.LogMethodExit(turnstileDTO);
            return turnstileDTO;
        }

        /// <summary>
        /// Delete the record from the Turnstiles database based on achievementClassId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(int turnstileId)
        {
            log.LogMethodEntry(turnstileId);
            string deleteTurnstileDTOQuery = @"delete  
                                              from Turnstiles
                                              where TurnstileId = @turnstileId";

            SqlParameter[] deleteTurnstileDTOParameters = new SqlParameter[1];
            deleteTurnstileDTOParameters[0] = new SqlParameter("@turnstileId", turnstileId);
            int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteTurnstileDTOQuery, deleteTurnstileDTOParameters, sqlTransaction);
            log.LogMethodExit(deleteStatus);
            return deleteStatus;
        }

        //public string GetTurnstileType(int Type)
        //{
        //    log.LogMethodEntry(Type);
        //    if (Type == -1)
        //        return null;

        //    DataAccessHandler dataHandler = new DataAccessHandler();
        //    log.LogVariableState("QueryParam", Type);
        //    string lookupValue = dataHandler.executeSelectQuery(@"select LookupValue
        //                                        from LookupView
        //                                        where LookupName = 'TURNSTILE_TYPE'
        //                                        and LookupValueId = @id",
        //                                        new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@id", Type) },
        //                                        null).Rows[0][0].ToString();
        //    log.LogMethodExit(lookupValue);
        //    return lookupValue;

        //}

        public string GetTurnstileValues(string lookupName, int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            if (lookupValueId == -1)
                return null;

            DataAccessHandler dataHandler = new DataAccessHandler();
            log.LogVariableState("QueryParam", lookupValueId);
            string query = @"select LookupValue
                                                from LookupView
                                                where LookupName = @lookupName
                                                and LookupValueId = @lookupValueId";
            SqlParameter[] selectTurnstileDTOParameters = new SqlParameter[2];
            selectTurnstileDTOParameters[0] = new SqlParameter("@lookupValueId", lookupValueId);
            selectTurnstileDTOParameters[1] = new SqlParameter("@lookupName", lookupName);

            string lookupValue = dataHandler.executeSelectQuery(query, selectTurnstileDTOParameters, null).Rows[0][0].ToString();

            log.LogMethodExit(lookupValue);
            return lookupValue;
        }

        //public string GetTurnstileMake(int Make)
        //{
        //    log.LogMethodEntry(Make);
        //    if (Make == -1)
        //        return null;

        //    DataAccessHandler dataHandler = new DataAccessHandler();
        //    log.LogVariableState("QueryParam", Make);
        //    string lookupValue = dataHandler.executeSelectQuery(@"select LookupValue
        //                                        from LookupView
        //                                        where LookupName = 'TURNSTILE_MAKE'
        //                                        and LookupValueId = @id",
        //                                        new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@id", Make) },
        //                                        null).Rows[0][0].ToString();
        //    log.LogMethodExit(lookupValue);
        //    return lookupValue;
        //}

        //public string GetTurnstileModel(int Model)
        //{
        //    log.LogMethodEntry(Model);
        //    if (Model == -1)
        //        return null;

        //    DataAccessHandler dataHandler = new DataAccessHandler();
        //    string lookupValue = dataHandler.executeSelectQuery(@"select LookupValue
        //                                        from LookupView
        //                                        where LookupName = 'TURNSTILE_MODEL'
        //                                        and LookupValueId = @id",
        //                                        new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@id", Model) },
        //                                        null).Rows[0][0].ToString();
        //    log.LogMethodExit(lookupValue);
        //    return lookupValue;
        //}

        public string GetProfileName(int profileId)
        {
            log.LogMethodEntry(profileId);
            if (profileId == -1)
                return null;

            DataAccessHandler dataHandler = new DataAccessHandler();
            string profileName = dataHandler.executeSelectQuery(@"select profile_name 
                                                from game_profile 
                                                where game_profile_id = @id",
                                                new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@id", profileId) },
                                                null).Rows[0][0].ToString();
            log.LogMethodExit(profileName);
            return profileName;
        }


        /// <summary>
        /// Gets the TurnstileDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TurnstileDTO matching the search criteria</returns>
        public List<TurnstileDTO> GetTurnstileDTOsList(List<KeyValuePair<TurnstileDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectTurnstileDTOQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<TurnstileDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(TurnstileDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(TurnstileDTO.SearchByParameters.TURNSTILE_ID) ||
                                searchParameter.Key.Equals(TurnstileDTO.SearchByParameters.GAME_PROFILE_ID) ||
                                 searchParameter.Key.Equals(TurnstileDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(TurnstileDTO.SearchByParameters.TYPE) ||
                                   searchParameter.Key.Equals(TurnstileDTO.SearchByParameters.MAKE) ||
                                   searchParameter.Key.Equals(TurnstileDTO.SearchByParameters.MODEL))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(TurnstileDTO.SearchByParameters.ACTIVE))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                    selectTurnstileDTOQuery = selectTurnstileDTOQuery + query;
                selectTurnstileDTOQuery = selectTurnstileDTOQuery + " Order by TurnstileId";
            }

            DataTable TurnstileDTOsData = dataAccessHandler.executeSelectQuery(selectTurnstileDTOQuery, parameters.ToArray(), sqlTransaction);
            List<TurnstileDTO> turnstileDTOsList = new List<TurnstileDTO>();
            if (TurnstileDTOsData.Rows.Count > 0)
            {

                foreach (DataRow dataRow in TurnstileDTOsData.Rows)
                {
                    TurnstileDTO turnstileDTOObject = GetTurnstileDTO(dataRow);
                    turnstileDTOsList.Add(turnstileDTOObject);
                }
            }
            log.LogMethodExit(turnstileDTOsList);
            return turnstileDTOsList;
        }

    }
}
