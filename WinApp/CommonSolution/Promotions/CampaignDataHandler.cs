/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler File for Campaign
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.80       10-June-2019   Divya A                 Created 
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Campaign Data Handler - Handles insert, update and selection of Campaign objects
    /// </summary>
    class CampaignDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Campaigns as campaigns ";

        /// <summary>
        /// Dictionary for searching Parameters for the campaigns object.
        /// </summary>
        private static readonly Dictionary<CampaignDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CampaignDTO.SearchByParameters, string>
        {
            { CampaignDTO.SearchByParameters.CAMPAIGN_ID,"campaigns.CampaignId"},
            { CampaignDTO.SearchByParameters.NAME,"campaigns.Name"},
            { CampaignDTO.SearchByParameters.DESCRIPTION,"campaigns.Description"},
            { CampaignDTO.SearchByParameters.START_DATE,"campaigns.StartDate"},
            { CampaignDTO.SearchByParameters.END_DATE,"campaigns.EndDate"},
            { CampaignDTO.SearchByParameters.COMMUNICATION_MODE,"campaigns.CommunicationMode"},
            { CampaignDTO.SearchByParameters.SITE_ID,"campaigns.site_id"},
            { CampaignDTO.SearchByParameters.IS_ACTIVE,"campaigns.IsActive"},
            { CampaignDTO.SearchByParameters.MASTER_ENTITY_ID,"campaigns.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for CampaignDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public CampaignDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Campaign Record.
        /// </summary>
        /// <param name="campaignDTO">CampaignDTO object passed as a parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(CampaignDTO campaignDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignId", campaignDTO.CampaignId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", campaignDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", campaignDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartDate", campaignDTO.StartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EndDate", campaignDTO.EndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CommunicationMode", campaignDTO.CommunicationMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageTemplate", campaignDTO.MessageTemplate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", campaignDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", campaignDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to CampaignDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of CampaignDTO</returns>
        private CampaignDTO GetCampaignDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CampaignDTO campaignDTO = new CampaignDTO(dataRow["CampaignId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignId"]),
                                                dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                                dataRow["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["StartDate"]),
                                                dataRow["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EndDate"]),
                                                dataRow["CommunicationMode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CommunicationMode"]),
                                                dataRow["MessageTemplate"] == DBNull.Value ? string.Empty: Convert.ToString(dataRow["MessageTemplate"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]));
            return campaignDTO;
        }

        /// <summary>
        /// Gets the Campaigns data of passed Campaign ID
        /// </summary>
        /// <param name="campaignId">campaignId is passed as parameter</param>
        /// <returns>Returns CampaignDTO object</returns>
        public CampaignDTO GetCampaignDTO(int campaignId)
        {
            log.LogMethodEntry(campaignId);
            CampaignDTO result = null;
            string query = SELECT_QUERY + @" WHERE campaigns.CampaignId = @CampaignId";
            SqlParameter parameter = new SqlParameter("@CampaignId", campaignId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCampaignDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the Campaign record
        /// </summary>
        /// <param name="campaignDTO">CampaignDTO is passed as parameter</param>
        internal void Delete(CampaignDTO campaignDTO)
        {
            log.LogMethodEntry(campaignDTO);
            string query = @"DELETE  
                             FROM Campaigns
                             WHERE Campaigns.CampaignId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", campaignDTO.CampaignId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            campaignDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="campaignDTO">CampaignDTO is passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshCampaignDTO(CampaignDTO campaignDTO, DataTable dt)
        {
            log.LogMethodEntry(campaignDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                campaignDTO.CampaignId = Convert.ToInt32(dt.Rows[0]["CampaignId"]);
                campaignDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                campaignDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                campaignDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                campaignDTO.LastUpdatedBy = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                campaignDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                campaignDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the Campaigns Table. 
        /// </summary>
        /// <param name="campaignDTO">CampaignDTO object passed as a parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated CampaignDTO</returns>
        public CampaignDTO Insert(CampaignDTO campaignDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[Campaigns]
                            (
                            Name,
                            Description,
                            StartDate,
                            EndDate,
                            CommunicationMode,
                            MessageTemplate,
                            LastUpdatedUser,
                            LastUpdatedDate,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate, IsActive
                            )
                            VALUES
                            (
                            @Name,
                            @Description,
                            @StartDate,
                            @EndDate,
                            @CommunicationMode,
                            @MessageTemplate,
                            @LastUpdatedUser,
                            GETDATE(),
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(), @IsActive
                            )
                            SELECT * FROM Campaigns WHERE CampaignId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignDTO(campaignDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CampaignDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignDTO);
            return campaignDTO;
        }

        /// <summary>
        /// Updates the record in the Campaigns Table. 
        /// </summary>
        /// <param name="campaignDTO">CampaignDTO object passed as a parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated CampaignDTO</returns>
        public CampaignDTO Update(CampaignDTO campaignDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[Campaigns]
                             SET
                             Name = @Name,
                             Description = @Description,
                             StartDate = @StartDate,
                             EndDate = @EndDate,
                             CommunicationMode = @CommunicationMode,
                             MessageTemplate = @MessageTemplate,
                             LastUpdatedUser = @LastUpdatedUser,
                             LastUpdatedDate = GETDATE(),
                            -- site_id = @site_id,
                             MasterEntityId = @MasterEntityId,
                             IsActive = @IsActive
                             WHERE CampaignId = @CampaignId
                            SELECT * FROM Campaigns WHERE CampaignId = @CampaignId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignDTO(campaignDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating CampaignDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignDTO);
            return campaignDTO;
        }

        /// <summary>
        /// Returns the List of CampaignDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of CampaignDTO </returns>
        public List<CampaignDTO> GetCampaignDTOList(List<KeyValuePair<CampaignDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CampaignDTO> campaignDTOList = new List<CampaignDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CampaignDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CampaignDTO.SearchByParameters.CAMPAIGN_ID ||
                            searchParameter.Key == CampaignDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignDTO.SearchByParameters.START_DATE)// to be checked
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CampaignDTO.SearchByParameters.END_DATE)// to be checked
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CampaignDTO.SearchByParameters.COMMUNICATION_MODE ||
                                searchParameter.Key == CampaignDTO.SearchByParameters.DESCRIPTION ||
                                searchParameter.Key == CampaignDTO.SearchByParameters.NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CampaignDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == CampaignDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                    CampaignDTO campaignDTO = GetCampaignDTO(dataRow);
                    campaignDTOList.Add(campaignDTO);
                }
            }
            log.LogMethodExit(campaignDTOList);
            return campaignDTOList;
        }

    }
}
