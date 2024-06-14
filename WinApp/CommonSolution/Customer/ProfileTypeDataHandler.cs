/********************************************************************************************
 * Project Name - ProfileType Data Handler
 * Description  - Data handler of the ProfileType class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.70       19-Jul-2019    Girish Kundar   Modified :Structure of data Handler - insert /Update methods
*                                                    Fix for SQL Injection Issue  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    ///  ProfileType Data Handler - Handles insert, update and select of  ProfileType objects
    /// </summary>
    public class ProfileTypeDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<ProfileTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProfileTypeDTO.SearchByParameters, string>
            {
                {ProfileTypeDTO.SearchByParameters.ID, "pt.Id"},
                {ProfileTypeDTO.SearchByParameters.NAME, "pt.Name"},
                {ProfileTypeDTO.SearchByParameters.IS_ACTIVE,"pt.IsActive"},
                {ProfileTypeDTO.SearchByParameters.MASTER_ENTITY_ID,"pt.MasterEntityId"},
                {ProfileTypeDTO.SearchByParameters.SITE_ID, "pt.site_id"}
            };
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from ProfileType AS pt";
        /// <summary>
        /// Default constructor of ProfileTypeDataHandler class
        /// </summary>
        public ProfileTypeDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new  DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProfileType Record.
        /// </summary>
        /// <param name="profileTypeDTO">ProfileTypeDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProfileTypeDTO profileTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(profileTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", profileTypeDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", profileTypeDTO.Name, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", profileTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", profileTypeDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", profileTypeDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ProfileType record to the database
        /// </summary>
        /// <param name="profileTypeDTO">ProfileTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns ProfileTypeDTO</returns>
        public ProfileTypeDTO InsertProfileType(ProfileTypeDTO profileTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(profileTypeDTO, loginId, siteId);
            string query = @"INSERT INTO ProfileType 
                                        ( 
                                            Name,
                                            Description,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId

                                        ) 
                                VALUES 
                                        (
                                            @Name,
                                            @Description,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM ProfileType WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(profileTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProfileTypeDTO(profileTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting profileTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(profileTypeDTO);
            return profileTypeDTO;
        }

        /// <summary>
        /// Updates the ProfileType record
        /// </summary>
        /// <param name="profileTypeDTO">ProfileTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns ProfileTypeDTO</returns>
        public ProfileTypeDTO UpdateProfileType(ProfileTypeDTO profileTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(profileTypeDTO, loginId, siteId);
            string query = @"UPDATE ProfileType 
                             SET Name=@Name,
                                 Description=@Description,
                                 IsActive = @IsActive,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate=GETDATE(),
                                 MasterEntityId=@MasterEntityId
                             WHERE Id = @Id 
                       SELECT * FROM ProfileType WHERE Id  = @Id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(profileTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProfileTypeDTO(profileTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating profileTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(profileTypeDTO);
            return profileTypeDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="profileTypeDTO">ProfileTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshProfileTypeDTO(ProfileTypeDTO profileTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(profileTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                profileTypeDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                profileTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                profileTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                profileTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                profileTypeDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                profileTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                profileTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ProfileTypeDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ProfileTypeDTO</returns>
        private ProfileTypeDTO GetProfileTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProfileTypeDTO profileTypeDTO = new ProfileTypeDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                            dataRow["Description"] == DBNull.Value ? string.Empty : dataRow["Description"].ToString(),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(profileTypeDTO);
            return profileTypeDTO;
        }

        /// <summary>
        /// Gets the ProfileType data of passed ProfileType Id
        /// </summary>
        /// <param name="profileTypeId">integer type parameter</param>
        /// <returns>Returns ProfileTypeDTO</returns>
        public ProfileTypeDTO GetProfileTypeDTO(int profileTypeId)
        {
            log.LogMethodEntry(profileTypeId);
            ProfileTypeDTO returnValue = null;
            string query = @"SELECT *
                            FROM ProfileType
                            WHERE Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", profileTypeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetProfileTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the ProfileTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProfileTypeDTO matching the search criteria</returns>
        public List<ProfileTypeDTO> GetProfileTypeDTOList(List<KeyValuePair<ProfileTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ProfileTypeDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProfileTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProfileTypeDTO.SearchByParameters.ID
                            || searchParameter.Key == ProfileTypeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProfileTypeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProfileTypeDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ProfileTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProfileTypeDTO profileTypeDTO = GetProfileTypeDTO(dataRow);
                    list.Add(profileTypeDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
