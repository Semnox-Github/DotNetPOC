/********************************************************************************************
 * Project Name - State Data Handler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 *********************************************************************************************
 *2.60         28-Mar-2019     Mushahid Faizan    Renamed Guid_id to Guid in InsertStateDTO().
 *                                                Added SqlTransaction in the Constructor.
 *2.70.2        25-Jul-2019      Dakshakh Raj       Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix.
 *            29-Jul-2019      Mushahid Faizan    Added IsActive
 *2.70.2        06-Dec-2019     Jinto Thomas        Removed siteid from update query             
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class StateDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM State as st";

        /// <summary>
        /// Dictionary for searching Parameters for the State object.
        /// </summary>
        private static readonly Dictionary<StateDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<StateDTO.SearchByParameters, string>
            {
                {StateDTO.SearchByParameters.STATE_ID, "st.StateId"},
                {StateDTO.SearchByParameters.STATE, "st.State"},
                {StateDTO.SearchByParameters.STATE_DESCRIPTION, "st.Description"},
                {StateDTO.SearchByParameters.COUNTRY_ID, "st.CountryId"},
                {StateDTO.SearchByParameters.SITE_ID, "st.site_id"},
                {StateDTO.SearchByParameters.MASTER_ENTITY_ID, "st.MasterEntityId"}
             };

        /// <summary>
        /// Default constructor of StateDataHandler class
        /// </summary>
        public StateDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating stateDTO Reecord.
        /// </summary>
        /// <param name="stateDTO">stateDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(StateDTO stateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(stateDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@stateId", stateDTO.StateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@state", string.IsNullOrEmpty(stateDTO.State) ? DBNull.Value : (object)stateDTO.State));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(stateDTO.Description) ? DBNull.Value : (object)stateDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@countryId", stateDTO.CountryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", stateDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", stateDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the state record to the database
        /// </summary>
        /// <param name="StateDTO">StateDTO type object</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns inserted record id</returns>
        public StateDTO InsertStateDTO(StateDTO StateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(StateDTO, loginId, siteId);
            string insertStateDTOQuery = @"INSERT INTO [dbo].[State] 
                                                            ( 
                                                            State,
                                                            Description,
                                                            CountryId,
                                                            site_id,
                                                            Guid,
                                                            MasterEntityId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdateDate, 
                                                            IsActive
                                                         )
                                                       values
                                                         ( 
                                                            @state,
                                                            @description,
                                                            @countryId,
                                                            @siteId,
                                                            NewId(), 
                                                            @masterEntityId,
                                                            @createdBy,
                                                            GETDATE(),
                                                            @lastUpdatedBy,
                                                            GETDATE(),
                                                            @isActive
                                                          )SELECT * FROM State WHERE StateId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertStateDTOQuery, GetSQLParameters(StateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshStateDTO(StateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting StateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(StateDTO);
            return StateDTO;
        }

        /// <summary>
        /// Updates the state record to the database
        /// </summary>
        /// <param name="StateDTO">StateDTO type object</param>
        /// <param name="loginId">updated user id number</param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns # of rows updated</returns>
        public StateDTO UpdateStateDTO(StateDTO StateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(StateDTO, loginId, siteId);
            string updateStateDTOQuery = @"update state
                                                         set
                                                            State = @state,
                                                            Description = @description,
                                                            CountryId = @countryId,
                                                            --site_id = @siteId, 
                                                            MasterEntityId = @masterEntityId, 
                                                            LastUpdatedBy = @lastUpdatedBy,
                                                            LastUpdateDate = GETDATE(),
                                                            IsActive=@isActive
                                                        where 
                                                            StateId = @StateId
                                                        SELECT* FROM state WHERE StateId = @StateId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateStateDTOQuery, GetSQLParameters(StateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshStateDTO(StateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating StateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(StateDTO);
            return StateDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="stateDTO">stateDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshStateDTO(StateDTO stateDTO, DataTable dt)
        {
            log.LogMethodEntry(stateDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                stateDTO.StateId = Convert.ToInt32(dt.Rows[0]["StateId"]);
                stateDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                stateDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                stateDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                stateDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                stateDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                stateDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to StateDTO class type
        /// </summary>
        /// <param name="StateDTODataRow">StateDTO DataRow</param>
        /// <returns>Returns StateDTO</returns>
        private StateDTO GetStateDTO(DataRow StateDTODataRow)
        {
            log.LogMethodEntry(StateDTODataRow);
            StateDTO StateDTO = new StateDTO(
                                 Convert.ToInt32(StateDTODataRow["StateId"]),
                                 StateDTODataRow["State"].ToString(),
                                 StateDTODataRow["Description"].ToString(),
                                 StateDTODataRow["CountryId"] == DBNull.Value ? -1 : Convert.ToInt32(StateDTODataRow["CountryId"]),
                                 StateDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(StateDTODataRow["site_id"]),
                                 StateDTODataRow["Guid"].ToString(),
                                 StateDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(StateDTODataRow["SynchStatus"]),
                                 StateDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(StateDTODataRow["MasterEntityId"]),
                                 StateDTODataRow["CreatedBy"].ToString(),
                                 StateDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(StateDTODataRow["CreationDate"]),
                                 StateDTODataRow["LastUpdatedBy"].ToString(),
                                 StateDTODataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(StateDTODataRow["LastUpdateDate"]),
                                 StateDTODataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(StateDTODataRow["IsActive"])
                                 );
            log.LogMethodExit(StateDTO);
            return StateDTO;
        }

        /// <summary>
        /// Gets the StateDTO data of passed StateId
        /// </summary>
        /// <param name="stateId">integer type parameter</param>
        /// <returns>Returns StateDTO</returns>
        public StateDTO GetStateDTO(int stateId)
        {
            log.LogMethodEntry(stateId);
            string selectStateDTOQuery = SELECT_QUERY + @" WHERE st.StateId = @stateId";
            SqlParameter[] selectStateDTOParameters = new SqlParameter[1];
            selectStateDTOParameters[0] = new SqlParameter("@stateId", stateId);
            DataTable selectedStateDTO = dataAccessHandler.executeSelectQuery(selectStateDTOQuery, selectStateDTOParameters, sqlTransaction);
            StateDTO StateDTO = new StateDTO(); ;
            if (selectedStateDTO.Rows.Count > 0)
            {
                DataRow stateRow = selectedStateDTO.Rows[0];
                StateDTO = GetStateDTO(stateRow);

            }
            log.LogMethodExit(StateDTO);
            return StateDTO;
        }

        /// <summary>
        ///Delete the record from the State database based on stateId.
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns>return the in</returns>
        public int Delete(int stateId)
        {
            log.LogMethodEntry(stateId);
            try
            {
                string deleteStateQuery = @"delete  
                                                 from State
                                                where StateId = @stateId";

                SqlParameter[] deleteStateDTOParameters = new SqlParameter[1];
                deleteStateDTOParameters[0] = new SqlParameter("@stateId", stateId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteStateQuery, deleteStateDTOParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// Gets the StateDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of StateDTO matching the search criteria</returns>
        public List<StateDTO> GetStateDTOList(List<KeyValuePair<StateDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectStateDTOQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<StateDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(StateDTO.SearchByParameters.COUNTRY_ID)
                                || searchParameter.Key.Equals(StateDTO.SearchByParameters.STATE_ID)
                                || searchParameter.Key.Equals(StateDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(StateDTO.SearchByParameters.STATE) 
                                 || searchParameter.Key.Equals(StateDTO.SearchByParameters.STATE_DESCRIPTION))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == StateDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                    selectStateDTOQuery = selectStateDTOQuery + query;
                selectStateDTOQuery = selectStateDTOQuery + " Order by countryId, stateId";
            }

            DataTable StateDTOsData = dataAccessHandler.executeSelectQuery(selectStateDTOQuery, parameters.ToArray(), sqlTransaction);
            List<StateDTO> StateDTOsList = new List<StateDTO>(); 
            if (StateDTOsData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in StateDTOsData.Rows)
                {
                    StateDTO StateDTOObject = GetStateDTO(dataRow);
                    StateDTOsList.Add(StateDTOObject);
                }
                log.LogMethodExit(StateDTOsList);

            }
            return StateDTOsList;
        }
    }
}
