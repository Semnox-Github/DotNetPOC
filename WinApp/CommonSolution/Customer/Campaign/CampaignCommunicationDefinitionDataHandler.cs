/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - CampaignCommunicationDefinition
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.110.0     20-Jan-2020      Prajwal             Created
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
    class CampaignCommunicationDefinitionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CampaignCommunicationDefinition AS ccd ";


        private static readonly Dictionary<CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters, string> DBSearchParameters = new Dictionary<CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters, string>
        {
             {CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.CAMPAIGN_COMMUNICATION_DEFINITION_ID , "ccd.CampaignCommunicationDefinitionId"},
             {CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.SITE_ID , "ccd.site_id"},
             {CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.MASTER_ENTITY_ID , "ccd.MasterEntityId"},
             {CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.IS_ACTIVE , "ccd.IsActive"},
             {CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.CAMPAIGN_DEFINITION_ID , "ccd.CampaignDefinitionId"},
             {CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.MESSAGING_CLIENT_ID , "ccd.MessagingClientId"},
             {CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.MESSAGE_TEMPLATE_ID , "ccd.MessageTemplateId"}

};

        /// <summary>
        /// Default constructor of CampaignCommunicationDefinitionDataHandler class
        /// </summary>
        public CampaignCommunicationDefinitionDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(CampaignCommunicationDefinitionDTO campaignCommunicationDefinitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCommunicationDefinitionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignCommunicationDefinitionId", campaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignDefinitionId", campaignCommunicationDefinitionDTO.CampaignDefinitionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageTemplateid", campaignCommunicationDefinitionDTO.MessageTemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessagingClientId", campaignCommunicationDefinitionDTO.MessagingClientId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Retry", campaignCommunicationDefinitionDTO.Retry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", campaignCommunicationDefinitionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", campaignCommunicationDefinitionDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", campaignCommunicationDefinitionDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the CampaignCommunicationDefinition record to the database
        /// </summary>
        /// <param name="CampaignCommunicationDefinition">CampaignCommunicationDefinitionDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record DTO</returns>
        internal CampaignCommunicationDefinitionDTO Insert(CampaignCommunicationDefinitionDTO campaignCommunicationDefinitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCommunicationDefinitionDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[CampaignCommunicationDefinition] 
                                                        (                                                 
                                                         CampaignDefinitionId,
                                                         MessagingClientId,
                                                         MessageTemplateid,
                                                         Retry,
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
                                                          @CampaignDefinitionId,
                                                          @MessagingClientId,
                                                          @MessageTemplateid,
                                                          @Retry,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @IsActive,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM CampaignCommunicationDefinition WHERE CampaignCommunicationDefinitionId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignCommunicationDefinitionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignCommunicationDefinitionDTO(campaignCommunicationDefinitionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignCommunicationDefinitionDTO);
            return campaignCommunicationDefinitionDTO;
        }

        private void RefreshCampaignCommunicationDefinitionDTO(CampaignCommunicationDefinitionDTO campaignCommunicationDefinitionDTO, DataTable dt)
        {
            log.LogMethodEntry(campaignCommunicationDefinitionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                campaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId = Convert.ToInt32(dt.Rows[0]["CampaignCommunicationDefinitionId"]);
                campaignCommunicationDefinitionDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                campaignCommunicationDefinitionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                campaignCommunicationDefinitionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                campaignCommunicationDefinitionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                campaignCommunicationDefinitionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                campaignCommunicationDefinitionDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the CampaignCommunicationDefinition  record
        /// </summary>
        /// <param name="CampaignCommunicationDefinition">CampaignCommunicationDefinitionDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal CampaignCommunicationDefinitionDTO Update(CampaignCommunicationDefinitionDTO campaignCommunicationDefinitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCommunicationDefinitionDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CampaignCommunicationDefinition] set
                               [CampaignDefinitionId]         = @CampaignDefinitionId,
                               [MessagingClientId]            = @MessagingClientId,
                               [MessageTemplateid]            = @MessageTemplateid,
                               [Retry]                        = @Retry,
                               [MasterEntityId]               = @MasterEntityId,
                               [site_id]                      = @SiteId,
                               [IsActive]                     = @IsActive,
                               [LastUpdatedBy]              = @LastUpdatedBy,
                               [LastUpdatedDate]              = GETDATE()
                               where CampaignCommunicationDefinitionId = @CampaignCommunicationDefinitionId
                             SELECT * FROM CampaignCommunicationDefinition WHERE CampaignCommunicationDefinitionId = @CampaignCommunicationDefinitionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignCommunicationDefinitionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignCommunicationDefinitionDTO(campaignCommunicationDefinitionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignCommunicationDefinitionDTO);
            return campaignCommunicationDefinitionDTO;
        }

        /// <summary>
        /// Converts the Data row object to CampaignCommunicationDefinitionDTO class type
        /// </summary>
        /// <param name="CampaignCommunicationDefinitionDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns CampaignCommunicationDefinitionFormat</returns>
        private CampaignCommunicationDefinitionDTO GetCampaignCommunicationDefinitionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CampaignCommunicationDefinitionDTO campaignCommunicationDefinitionDataObject = new CampaignCommunicationDefinitionDTO(Convert.ToInt32(dataRow["CampaignCommunicationDefinitionId"]),
                                                    dataRow["CampaignDefinitionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignDefinitionId"]),
                                                    dataRow["MessagingClientId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MessagingClientId"]),
                                                    dataRow["MessageTemplateid"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MessageTemplateid"]),
                                                    dataRow["Retry"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["Retry"]),
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
            return campaignCommunicationDefinitionDataObject;
        }

        internal List<CampaignCommunicationDefinitionDTO> GetCampaignCommunicationDefinitionDTOList(List<int> campaignDefinitionIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(campaignDefinitionIdList);
            List<CampaignCommunicationDefinitionDTO> campaignCommunicationDefinitionDTOList = new List<CampaignCommunicationDefinitionDTO>();
            string query = @"SELECT *
                            FROM CampaignCommunicationDefinition, @campaignDefinitionIdList List
                            WHERE CampaignDefinitionId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@campaignDefinitionIdList", campaignDefinitionIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                campaignCommunicationDefinitionDTOList = table.Rows.Cast<DataRow>().Select(x => GetCampaignCommunicationDefinitionDTO(x)).ToList();
            }
            log.LogMethodExit(campaignCommunicationDefinitionDTOList);
            return campaignCommunicationDefinitionDTOList;
        }

        /// <summary>
        /// Gets the GetCampaignCommunicationDefinition data of passed displaygroup
        /// </summary>
        /// <param name="campaignCommunicationDefinitionId">integer type parameter</param>
        /// <returns>Returns CampaignCommunicationDefinitionDTO</returns>
        internal CampaignCommunicationDefinitionDTO GetCampaignCommunicationDefinition(int campaignCommunicationDefinitionId)
        {
            log.LogMethodEntry(campaignCommunicationDefinitionId);
            CampaignCommunicationDefinitionDTO result = null;
            string query = SELECT_QUERY + @" WHERE ccd.CampaignCommunicationDefinitionId = @CampaignCommunicationDefinitionId";
            SqlParameter parameter = new SqlParameter("@CampaignCommunicationDefinitionId", campaignCommunicationDefinitionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCampaignCommunicationDefinitionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }



        /// <summary>
        /// Gets the CampaignCommunicationDefinitionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CampaignCommunicationDefinitionDTO matching the search criteria</returns>    
        internal List<CampaignCommunicationDefinitionDTO> GetCampaignCommunicationDefinitionList(List<KeyValuePair<CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CampaignCommunicationDefinitionDTO> campaignCommunicationDefinitionDTOList = new List<CampaignCommunicationDefinitionDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.CAMPAIGN_DEFINITION_ID ||
                            searchParameter.Key == CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.CAMPAIGN_COMMUNICATION_DEFINITION_ID ||
                            searchParameter.Key == CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.MESSAGE_TEMPLATE_ID ||
                            searchParameter.Key == CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.MESSAGING_CLIENT_ID ||
                            searchParameter.Key == CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.SITE_ID )
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters.IS_ACTIVE)
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
                campaignCommunicationDefinitionDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetCampaignCommunicationDefinitionDTO(x)).ToList();
            }
            log.LogMethodExit(campaignCommunicationDefinitionDTOList);
            return campaignCommunicationDefinitionDTOList;
        }
    }
}