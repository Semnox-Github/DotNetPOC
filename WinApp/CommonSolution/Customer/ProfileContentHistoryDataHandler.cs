/********************************************************************************************
 * Project Name - Profile Content History Data andler
 * Description  - Data handler of the Profile Content History class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2       19-Jul-2019    Girish Kundar       Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue  
 *2.70.2       05-Dec-2019   Jinto Thomas            Removed siteid from update query                  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    class ProfileContentHistoryDataHandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction = null;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from ProfileContentHistory AS pch";

        private static readonly Dictionary<ProfileContentHistoryDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProfileContentHistoryDTO.SearchByParameters, string>
            {
                {ProfileContentHistoryDTO.SearchByParameters.ID, "pch.Id"},
                {ProfileContentHistoryDTO.SearchByParameters.PROFILE_ID, "pch.ProfileID"},
                {ProfileContentHistoryDTO.SearchByParameters.RICH_CONTENT_ID, "pch.RichContentId"},
                {ProfileContentHistoryDTO.SearchByParameters.SITE_ID, "pch.site_id"},
                {ProfileContentHistoryDTO.SearchByParameters.IS_ACTIVE, "pch.IsActive"},
                {ProfileContentHistoryDTO.SearchByParameters.MASTER_ENTITY_ID, "pch.MasterEntityId"},
                {ProfileContentHistoryDTO.SearchByParameters.PROFILE_ID_LIST,"pch.ProfileID" }
             };

        /// <summary>
        /// Default constructor of CustomerContentHistoryDataHandler class
        /// </summary>
        public ProfileContentHistoryDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

     
        /// <summary>
        /// Default constructor of CustomerContentHistoryDataHandler class
        /// </summary>
        public ProfileContentHistoryDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProfileContentHistory Record.
        /// </summary>
        /// <param name="profileContentHistoryDTO">ProfileContentHistoryDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProfileContentHistoryDTO profileContentHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(profileContentHistoryDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", profileContentHistoryDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@profileId", profileContentHistoryDTO.ProfileId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@richContentId", profileContentHistoryDTO.RichContentId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentSignedDate", profileContentHistoryDTO.ContentSignedDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", profileContentHistoryDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", profileContentHistoryDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }



        /// <summary>
        /// Inserts the profileContentHistoryDTO record to the database
        /// </summary>
        /// <param name="profileContentHistoryDTO">profileContentHistoryDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        ///<returns>Returns ProfileContentHistoryDTO</returns>
        public ProfileContentHistoryDTO InsertProfileContentHistory(ProfileContentHistoryDTO profileContentHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(profileContentHistoryDTO, loginId, siteId);
            string query = @"insert into ProfileContentHistory
                                                            (
                                                            ProfileId,
                                                            RichContentId,
                                                            ContentSignedDate,
                                                            IsActive,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdateDate,
                                                            site_id,
                                                            Guid,
                                                            MasterEntityId
                                                         )
                                                       values
                                                         ( 
                                                            @profileId,
                                                            @richContentId,
                                                            @contentSignedDate,
                                                            @isActive,
                                                            @createdBy,
                                                            GETDATE(),
                                                            @lastUpdatedBy,
                                                            GETDATE(),
                                                            @siteId,
                                                            NewID(),
                                                            @masterEntityId
                                                          ) SELECT * FROM ProfileContentHistory WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(profileContentHistoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProfileContentHistoryDTO(profileContentHistoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting profileContentHistoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(profileContentHistoryDTO);
            return profileContentHistoryDTO;
        }



        /// <summary>
        /// Updates the ProfileContentHistory record to the database
        /// </summary>
        /// <param name="profileContentHistoryDTO">profileContentHistoryDTO</param>
        /// <param name="loginId"loginId></param>
        /// <param name="siteId">siteId</param>
        /// <returns>ProfileContentHistoryDTO</returns>
        public ProfileContentHistoryDTO UpdateProfileContentHistory(ProfileContentHistoryDTO profileContentHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(profileContentHistoryDTO, loginId, siteId);
            string query = @"update ProfileContentHistory
                                                         set
															ProfileId = @profileId,
                                                            RichContentId = @richContentId,
                                                            ContentSignedDate = @contentSignedDate,
                                                            LastUpdatedBy = @lastUpdatedBy,
                                                            LastUpdateDate = GETDATE(),
                                                            IsActive = @isActive,
                                                            --site_id = @siteId,
                                                            MasterEntityId = @masterEntityId
                                                          where    Id = @id 
                                                          SELECT * FROM ProfileContentHistory WHERE Id  = @id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(profileContentHistoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProfileContentHistoryDTO(profileContentHistoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating profileContentHistoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(profileContentHistoryDTO);
            return profileContentHistoryDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="profileContentHistoryDTO">ProfileContentHistoryDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshProfileContentHistoryDTO(ProfileContentHistoryDTO profileContentHistoryDTO, DataTable dt)
        {
            log.LogMethodEntry(profileContentHistoryDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                profileContentHistoryDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                profileContentHistoryDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                profileContentHistoryDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                profileContentHistoryDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                profileContentHistoryDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                profileContentHistoryDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                profileContentHistoryDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        ///  Converts the Data row object to ProfileContentHistoryDataRow class type
        /// </summary>
        /// <param name="CustomerContentHistoryDTODataRow"></param>
        /// <returns>Returns CustomerContentHistoryDTO</returns>
        private ProfileContentHistoryDTO GetProfileContentHistory(DataRow ProfileContentHistoryDataRow)
        {
            ProfileContentHistoryDTO profileContentHistoryDTO = new ProfileContentHistoryDTO(
								 ProfileContentHistoryDataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(ProfileContentHistoryDataRow["Id"]),
								 ProfileContentHistoryDataRow["ProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(ProfileContentHistoryDataRow["ProfileId"]),
								 ProfileContentHistoryDataRow["RichContentId"] == DBNull.Value ? -1 : Convert.ToInt32(ProfileContentHistoryDataRow["RichContentId"]),
								 ProfileContentHistoryDataRow["ContentSignedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ProfileContentHistoryDataRow["ContentSignedDate"]),
                                 ProfileContentHistoryDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(ProfileContentHistoryDataRow["IsActive"]),
                                 ProfileContentHistoryDataRow["CreatedBy"] == DBNull.Value ? "" : ProfileContentHistoryDataRow["CreatedBy"].ToString(),
								 ProfileContentHistoryDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ProfileContentHistoryDataRow["CreationDate"]),
								 ProfileContentHistoryDataRow["LastUpdatedBy"] == DBNull.Value ? "" : ProfileContentHistoryDataRow["LastUpdatedBy"].ToString(),
								 ProfileContentHistoryDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ProfileContentHistoryDataRow["LastUpdateDate"]),
								 ProfileContentHistoryDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(ProfileContentHistoryDataRow["Site_id"]),
								 ProfileContentHistoryDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(ProfileContentHistoryDataRow["SynchStatus"]),
								 ProfileContentHistoryDataRow["Guid"].ToString(),
								 ProfileContentHistoryDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(ProfileContentHistoryDataRow["MasterEntityId"])
                                 );
            log.LogMethodExit();
            return profileContentHistoryDTO;
        }


		/// <summary>
		/// Gets the ProfileContentHistoryDTO data of passed id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Returns ProfileContentHistoryDTO</returns>
		public ProfileContentHistoryDTO GetProfileContentHistory(int id)
        {
            log.LogMethodEntry(id);
            ProfileContentHistoryDTO profileContentHistoryDTO = null;
            string selectProfileContentHistoryDTOQuery = SELECT_QUERY + "  where pch.id = @id";
            SqlParameter[] selectProfileContentHistoryDTOParameters = new SqlParameter[1];
			selectProfileContentHistoryDTOParameters[0] = new SqlParameter("@id", id);
            DataTable selectedProfileContentHistory = dataAccessHandler.executeSelectQuery(selectProfileContentHistoryDTOQuery, selectProfileContentHistoryDTOParameters ,sqlTransaction);
            if (selectedProfileContentHistory.Rows.Count > 0)
            {
                DataRow profileContentHistoryRow = selectedProfileContentHistory.Rows[0];
				profileContentHistoryDTO = GetProfileContentHistory(profileContentHistoryRow);
            }

            log.LogMethodExit();
            return profileContentHistoryDTO;
        }



		/// <summary>
		/// Gets the ProfileContentHistoryDTO list matching the search key
		/// </summary>
		/// <param name="searchParameters"></param>
		/// <returns>Returns the list of ProfileContentHistoryDTO matching the search criteria</returns>
		public List<ProfileContentHistoryDTO> GetProfileContentHistoryDTOList(List<KeyValuePair<ProfileContentHistoryDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectProfileContentHistoryDTOQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProfileContentHistoryDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(ProfileContentHistoryDTO.SearchByParameters.ID)
                            || searchParameter.Key.Equals(ProfileContentHistoryDTO.SearchByParameters.PROFILE_ID)
                            || searchParameter.Key.Equals(ProfileContentHistoryDTO.SearchByParameters.RICH_CONTENT_ID)
                            || searchParameter.Key.Equals(ProfileContentHistoryDTO.SearchByParameters.MASTER_ENTITY_ID)
                            || searchParameter.Key.Equals(ProfileContentHistoryDTO.SearchByParameters.IS_ACTIVE))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(ProfileContentHistoryDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(ProfileContentHistoryDTO.SearchByParameters.PROFILE_ID_LIST))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
					selectProfileContentHistoryDTOQuery = selectProfileContentHistoryDTOQuery + query;
                // selectCustomerContentHistoryDTOQuery = selectCustomerContentHistoryDTOQuery + " Order by PK_CustomerContentHistory";
            }

            DataTable ProfileContentHistoryData = dataAccessHandler.executeSelectQuery(selectProfileContentHistoryDTOQuery, parameters.ToArray(),sqlTransaction);
            List<ProfileContentHistoryDTO> profileContentHistoryDTOList = new List<ProfileContentHistoryDTO>();
            if (ProfileContentHistoryData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in ProfileContentHistoryData.Rows)
                {
                    ProfileContentHistoryDTO CustomerContentHistoryDTOObject = GetProfileContentHistory(dataRow);
					profileContentHistoryDTOList.Add(CustomerContentHistoryDTOObject);
                }

            }
            log.LogMethodExit(profileContentHistoryDTOList);
            return profileContentHistoryDTOList;
        }

        /// <summary>
        /// Gets the ProfileContentHistoryDTO List for profileId List
        /// </summary>
        /// <param name="profileIdList">integer list parameter</param>
        /// <returns>Returns List of ProfileContentHistoryDTO</returns>
        public List<ProfileContentHistoryDTO> GetProfileContentHistoryDTOList(List<int> profileIdList, bool activeRecords)
        {
            log.LogMethodEntry(profileIdList);
            List<ProfileContentHistoryDTO> list = null;
            string query = SELECT_QUERY + @" INNER JOIN @ProfileIdList List ON pch.ProfileId = List.Id";
            if (activeRecords)
            {
                query += " WHERE pch.IsActive = 1 ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ProfileIdList", profileIdList, null, sqlTransaction);
            if (table.Rows.Count > 0)
            {
                list = new List<ProfileContentHistoryDTO>();
                foreach (DataRow dataRow in table.Rows)
                {
                    ProfileContentHistoryDTO profileContentHistoryDTO = GetProfileContentHistory(dataRow);
                    list.Add(profileContentHistoryDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

    }
}
