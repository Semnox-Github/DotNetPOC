/********************************************************************************************
* Project Name - Achievements
* Description  - Data Handler - AchievementProjectDataHandler
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.70        27-Aug-2019     Indrajeet Kumar     Modified - Added Who Columns.
*2.70        03-JUl-2019   Deeksha                 Modified :Added GetSqlParameter() 
 *                                                            SQL injection issue has been fixed
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query
 *2.80       04-Mar-2020     Vikas Dwivedi       Modified as per the Standard for Phase 1 changes.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Achievements
{
    public class AchievementProjectDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AchievementProject AS ap ";

        /// <summary>
        /// Dictionary for searching Parameters for the AchievementProject object.
        /// </summary>
        private static readonly Dictionary<AchievementProjectDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AchievementProjectDTO.SearchByParameters, string>
            {
                {AchievementProjectDTO.SearchByParameters.ACHIEVEMENTPROJECT_ID, "ap.AchievementProjectId"},
                { AchievementProjectDTO.SearchByParameters.PROJECT_NAME, "ap.ProjectName"},
                {AchievementProjectDTO.SearchByParameters.IS_ACTIVE, "ap.IsActive"},
                {AchievementProjectDTO.SearchByParameters.SITE_ID, "ap.site_id"},
                {AchievementProjectDTO.SearchByParameters.MASTER_ENTITY_ID, "ap.MasterEntityId"}
             };
        /// <summary>
        /// Parameterized Constructor for AchievementProjectDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public AchievementProjectDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AchievementProject Record.
        /// </summary>
        /// <param name="AchievementProjectDTO">AchievementProjectDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(AchievementProjectDTO achievementProjectDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementProjectDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AchievementProjectId", achievementProjectDTO.AchievementProjectId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProjectName", achievementProjectDTO.ProjectName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", achievementProjectDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", achievementProjectDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", achievementProjectDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to AchievementProjectDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the AchievementProjectDTO</returns>
        private AchievementProjectDTO GetAchievementProjectDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AchievementProjectDTO achievementProjectDTO = new AchievementProjectDTO(dataRow["AchievementProjectId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AchievementProjectId"]),
                                                         dataRow["ProjectName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProjectName"]),
                                                         dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                         dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                         dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ExternalSystemReference"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(achievementProjectDTO);
            return achievementProjectDTO;
        }
        /// <summary>
        /// Gets the AchievementProject data of passed id 
        /// </summary>
        /// <param name="id">id of AchievementProject is passed as parameter</param>
        /// <returns>Returns AchievementProjectDTO</returns>
        public AchievementProjectDTO GetAchievementProjectDTO(int id)
        {
            log.LogMethodEntry(id);
            AchievementProjectDTO result = null;
            string query = SELECT_QUERY + @" WHERE ap.AchievementProjectId= @AchievementProjectId";
            SqlParameter parameter = new SqlParameter("@AchievementProjectId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAchievementProjectDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Inserts the AchievementProjectDTO record to the database
        /// </summary>
        /// <param name="AchievementProjectDTO">AchievementProjectDTO type object</param>
        /// <returns>Returns inserted record id</returns>
        public AchievementProjectDTO InsertAchievementProject(AchievementProjectDTO achievementProjectDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementProjectDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[AchievementProject]
                                                         (
                                                            ProjectName,
                                                            IsActive,
                                                            LastUpdatedDate,
                                                            LastUpdatedUser,
                                                            Guid,
                                                            MasterEntityId,
                                                            site_id,
                                                            ExternalSystemReference,
                                                            CreatedBy,
                                                            CreationDate
                                                         )
                                                       values
                                                         (
                                                            @ProjectName,
                                                            @IsActive,
                                                            GetDate(),
                                                            @LastUpdatedUser,
                                                            NewId(),
                                                            @MasterEntityId,
                                                            @site_id,
                                                            @ExternalSystemReference,
                                                            @CreatedBy,
                                                            GETDATE()
                                                        ) SELECT* FROM AchievementProject WHERE AchievementProjectId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementProjectDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementProjectDTO(achievementProjectDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AchievementProjectDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementProjectDTO);
            return achievementProjectDTO;
        }
        /// <summary>
        /// Updates the AchievementProjectDTO record to the database
        /// </summary>
        /// <param name="AchievementProjectDTO">AchievementProjectDTO type object</param>
        /// <returns>Returns the count of updated rows</returns>
        public AchievementProjectDTO UpdateAchievementProject(AchievementProjectDTO achievementProjectDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementProjectDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AchievementProject]
                           SET 
                                                            ProjectName =  @ProjectName,
                                                            IsActive = @IsActive,
                                                            LastUpdatedDate = GetDate(),
                                                            LastUpdatedUser =  @LastUpdatedUser,                                                         
                                                            MasterEntityId = @MasterEntityId,
                                                            -- site_id = @site_id,
                                                            ExternalSystemReference = @ExternalSystemReference
                                                         WHERE AchievementProjectId =@AchievementProjectId 
                                    SELECT * FROM AchievementProject WHERE AchievementProjectId = @AchievementProjectId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementProjectDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementProjectDTO(achievementProjectDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating AchievementProjectDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementProjectDTO);
            return achievementProjectDTO;
        }
        /// <summary>
        /// Delete the record from the AchievementProject database based on achievementProjectId
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int achievementProjectId)
        {
            log.LogMethodEntry(achievementProjectId);
            string query = @"DELETE  
                             FROM AchievementProject
                             WHERE AchievementProject.AchievementProjectId = @AchievementProjectId";
            SqlParameter parameter = new SqlParameter("@AchievementProjectId", achievementProjectId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="achievementProjectDTO">AchievementProjectDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAchievementProjectDTO(AchievementProjectDTO achievementProjectDTO, DataTable dt)
        {
            log.LogMethodEntry(achievementProjectDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                achievementProjectDTO.AchievementProjectId = Convert.ToInt32(dt.Rows[0]["AchievementProjectId"]);
                achievementProjectDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                achievementProjectDTO.CreatedDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                achievementProjectDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                achievementProjectDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                achievementProjectDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                achievementProjectDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of AchievementProject based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AchievementProjectDTO</returns>
        public List<AchievementProjectDTO> GetAchievementProjectDTOList(List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AchievementProjectDTO> achievementProjectDTOList = new List<AchievementProjectDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AchievementProjectDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (
                           searchParameter.Key.Equals(AchievementProjectDTO.SearchByParameters.ACHIEVEMENTPROJECT_ID) ||
                            searchParameter.Key.Equals(AchievementProjectDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AchievementProjectDTO.SearchByParameters.PROJECT_NAME)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));

                        }
                        else if (searchParameter.Key == AchievementProjectDTO.SearchByParameters.SITE_ID)

                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AchievementProjectDTO.SearchByParameters.IS_ACTIVE) // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AchievementProjectDTO achievementProjectDTO = GetAchievementProjectDTO(dataRow);
                    achievementProjectDTOList.Add(achievementProjectDTO);
                }
            }
            log.LogMethodExit(achievementProjectDTOList);
            return achievementProjectDTOList;
        }
    }

}
