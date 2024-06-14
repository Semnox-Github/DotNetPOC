/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - CampaignCustomerProfile
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.110.0     22-Jan-2020      Prajwal             Created
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Campaign
{
    class CampaignCustomerProfileDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CampaignCustomerProfile AS ccp ";


        private static readonly Dictionary<CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters, string> DBSearchParameters = new Dictionary<CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters, string>
        {
              {CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters.CAMPAIGN_CUSTOMER_PROFILE_ID , "ccp.CampaignCustomerProfileId"},
              {CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters.CAMPAIGN_CUSTOMER_PROFILE_ID_LIST , "ccp.CampaignCustomerProfileId"},
              {CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters.SITE_ID , "ccp.site_id"},
              {CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters.MASTER_ENTITY_ID , "ccp.MasterEntityId"},
              {CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters.IS_ACTIVE , "ccp.IsActive"}
        };

        /// <summary>
        /// Default constructor of CampaignCustomerProfileDataHandler class
        /// </summary>
        public CampaignCustomerProfileDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(CampaignCustomerProfileDTO campaignCustomerProfileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCustomerProfileDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignCustomerProfileId", campaignCustomerProfileDTO.CampaignCustomerProfileId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", campaignCustomerProfileDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", campaignCustomerProfileDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Type", campaignCustomerProfileDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Query", campaignCustomerProfileDTO.Query));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", campaignCustomerProfileDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", campaignCustomerProfileDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", campaignCustomerProfileDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the CampaignCustomerProfile record to the database
        /// </summary>
        /// <param name="CampaignCustomerProfile">CampaignCustomerProfileDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal CampaignCustomerProfileDTO Insert(CampaignCustomerProfileDTO campaignCustomerProfileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCustomerProfileDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[CampaignCustomerProfile] 
                                                        (                                                 
                                                         Name,
                                                         Description,
                                                         Type,
                                                         Query,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedBy,
                                                         LastUpdatedDate,
                                                         site_id,
                                                         Guid,
                                                         MasterEntityId,
                                                         IsActive,
                                                         SynchStatus
                                                        ) 
                                                values 
                                                        (
                                                          @Name,
                                                          @Description,
                                                          @Type,
                                                          @Query,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @IsActive,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM CampaignCustomerProfile WHERE CampaignCustomerProfileId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignCustomerProfileDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignCustomerProfileDTO(campaignCustomerProfileDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignCustomerProfileDTO);
            return campaignCustomerProfileDTO;
        }

        private void RefreshCampaignCustomerProfileDTO(CampaignCustomerProfileDTO campaignCustomerProfileDTO, DataTable dt)
        {
            log.LogMethodEntry(campaignCustomerProfileDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                campaignCustomerProfileDTO.CampaignCustomerProfileId = Convert.ToInt32(dt.Rows[0]["CampaignCustomerProfileId"]);
                campaignCustomerProfileDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                campaignCustomerProfileDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                campaignCustomerProfileDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                campaignCustomerProfileDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                campaignCustomerProfileDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                campaignCustomerProfileDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the CampaignCustomerProfile  record
        /// </summary>
        /// <param name="CampaignCustomerProfile">CampaignCustomerProfileDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal CampaignCustomerProfileDTO Update(CampaignCustomerProfileDTO campaignCustomerProfileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCustomerProfileDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CampaignCustomerProfile] set
                               [Name]                         = @Name,
                               [Description]                  = @Description,
                               [Query]                        = @Query,
                               [Type]                         = @Type,
                               [MasterEntityId]               = @MasterEntityId,
                               [LastUpdatedBy]                = @LastUpdatedBy,
                               [site_id]                      = @SiteId,
                               [LastUpdatedDate]              = GETDATE(),
                               [IsActive]                     = @IsActive
                               where CampaignCustomerProfileId = @CampaignCustomerProfileId
                             SELECT * FROM CampaignCustomerProfile WHERE CampaignCustomerProfileId = @CampaignCustomerProfileId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignCustomerProfileDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignCustomerProfileDTO(campaignCustomerProfileDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignCustomerProfileDTO);
            return campaignCustomerProfileDTO;
        }

        /// <summary>
        /// Converts the Data row object to CampaignCustomerProfileDTO class type
        /// </summary>
        /// <param name="CampaignCustomerProfileDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ProductDisplayGroupFormat</returns>
        private CampaignCustomerProfileDTO GetCampaignCustomerProfileDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CampaignCustomerProfileDTO campaignCustomerProfileDataObject = new CampaignCustomerProfileDTO(Convert.ToInt32(dataRow["CampaignCustomerProfileId"]),
                                                    dataRow["Name"] == DBNull.Value ? string.Empty : (dataRow["Name"]).ToString(),
                                                    dataRow["Description"] == DBNull.Value ? string.Empty : (dataRow["Description"]).ToString(),
                                                    dataRow["Type"] == DBNull.Value ? "N" : (dataRow["Type"]).ToString(),
                                                    dataRow["Query"] == DBNull.Value ? string.Empty : (dataRow["Query"]).ToString(),
                                                    dataRow["CreatedBy"].ToString(),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"].ToString(),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"])
                                                    );
            log.LogMethodExit();
            return campaignCustomerProfileDataObject;
        }

        /// <summary>
        /// Gets the GetCampaignCustomerProfile data of passed displaygroup
        /// </summary>
        /// <param name="campaignCustomerProfileId">integer type parameter</param>
        /// <returns>Returns CampaignCustomerProfileDTO</returns>
        internal CampaignCustomerProfileDTO GetCampaignCustomerProfile(int campaignCustomerProfileId)
        {
            log.LogMethodEntry(campaignCustomerProfileId);
            CampaignCustomerProfileDTO result = null;
            string query = SELECT_QUERY + @" WHERE ccp.CampaignCustomerProfileId = @CampaignCustomerProfileId";
            SqlParameter parameter = new SqlParameter("@CampaignCustomerProfileId", campaignCustomerProfileId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCampaignCustomerProfileDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }



        /// <summary>
        /// Gets the CampaignCustomerProfileDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CampaignCustomerProfileDTO matching the search criteria</returns>    
        internal List<CampaignCustomerProfileDTO> GetCampaignCustomerProfileList(List<KeyValuePair<CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CampaignCustomerProfileDTO> campaignCustomerProfileDTOList = new List<CampaignCustomerProfileDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters.CAMPAIGN_CUSTOMER_PROFILE_ID ||
                            searchParameter.Key == CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters.SITE_ID )
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters.CAMPAIGN_CUSTOMER_PROFILE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                campaignCustomerProfileDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetCampaignCustomerProfileDTO(x)).ToList();
            }
            log.LogMethodExit(campaignCustomerProfileDTOList);
            return campaignCustomerProfileDTOList;
        }
    }
}