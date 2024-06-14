/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - CampaignCustomerProfileMap
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.110.0     21-Jan-2020      Prajwal             Created
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
    class CampaignCustomerProfileMapDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CampaignCustomerProfileMap AS ccpm ";


        private static readonly Dictionary<CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters, string> DBSearchParameters = new Dictionary<CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters, string>
        {
             {CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.CAMPAIGN_CUSTOMER_PROFILE_MAP_ID , "ccpm.CampaignCustomerProfileMapId"},
              {CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.CAMPAIGN_CUSTOMER_PROFILE_ID , "ccpm.CampaignCustomerProfileId"},
               {CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.SITE_ID , "ccpm.site_id"},
                {CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.MASTER_ENTITY_ID , "ccpm.MasterEntityId"},
                 {CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.IS_ACTIVE , "ccpm.IsActive"},
                 {CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.CAMPAIGN_DEFINITION_ID , "ccpm.CampaignDefinitionId"}

        };

        /// <summary>
        /// Default constructor of CampaignCustomerProfileMapDataHandler class
        /// </summary>
        public CampaignCustomerProfileMapDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(CampaignCustomerProfileMapDTO CampaignCustomerProfileMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(CampaignCustomerProfileMapDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignCustomerProfileMapId", CampaignCustomerProfileMapDTO.CampaignCustomerProfileMapId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignCustomerProfileId", CampaignCustomerProfileMapDTO.CampaignCustomerProfileId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignDefinitionId", CampaignCustomerProfileMapDTO.CampaignDefinitionId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", CampaignCustomerProfileMapDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", CampaignCustomerProfileMapDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", CampaignCustomerProfileMapDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the CampaignCustomerProfileMap record to the database
        /// </summary>
        /// <param name="CampaignCustomerProfileMap">CampaignCustomerProfileMapDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record DTO</returns>
        internal CampaignCustomerProfileMapDTO Insert(CampaignCustomerProfileMapDTO CampaignCustomerProfileMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(CampaignCustomerProfileMapDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[CampaignCustomerProfileMap] 
                                                        (                                                 
                                                         CampaignCustomerProfileId,
                                                         CampaignDefinitionId,
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
                                                          @CampaignCustomerProfileId,
                                                          @CampaignDefinitionId,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @IsActive,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM CampaignCustomerProfileMap WHERE CampaignCustomerProfileMapId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(CampaignCustomerProfileMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignCustomerProfileMapDTO(CampaignCustomerProfileMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(CampaignCustomerProfileMapDTO);
            return CampaignCustomerProfileMapDTO;
        }

        private void RefreshCampaignCustomerProfileMapDTO(CampaignCustomerProfileMapDTO campaignCustomerProfileMapDTO, DataTable dt)
        {
            log.LogMethodEntry(campaignCustomerProfileMapDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                campaignCustomerProfileMapDTO.CampaignCustomerProfileMapId = Convert.ToInt32(dt.Rows[0]["CampaignCustomerProfileMapId"]);
                campaignCustomerProfileMapDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                campaignCustomerProfileMapDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                campaignCustomerProfileMapDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                campaignCustomerProfileMapDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                campaignCustomerProfileMapDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                campaignCustomerProfileMapDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal List<CampaignCustomerProfileMapDTO> GetCampaignCustomerProfileMapDTOList(List<int> campaignDefinitionIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(campaignDefinitionIdList);
            List<CampaignCustomerProfileMapDTO> campaignCustomerProfileMapDTOList = new List<CampaignCustomerProfileMapDTO>();
            string query = @"SELECT *
                            FROM CampaignCustomerProfileMap, @campaignDefinitionIdList List
                            WHERE CampaignDefinitionId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@campaignDefinitionIdList", campaignDefinitionIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                campaignCustomerProfileMapDTOList = table.Rows.Cast<DataRow>().Select(x => GetCampaignCustomerProfileMapDTO(x)).ToList();
            }
            log.LogMethodExit(campaignCustomerProfileMapDTOList);
            return campaignCustomerProfileMapDTOList;
        }

        /// <summary>
        /// Updates the CampaignCustomerProfileMap  record
        /// </summary>
        /// <param name="CampaignCustomerProfileMap">CampaignCustomerProfileMapDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal CampaignCustomerProfileMapDTO Update(CampaignCustomerProfileMapDTO campaignCustomerProfileMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCustomerProfileMapDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CampaignCustomerProfileMap] set
                               [CampaignCustomerProfileId]    = @CampaignCustomerProfileId,
                               [CampaignDefinitionId]         = @CampaignDefinitionId,
                               [MasterEntityId]               = @MasterEntityId,
                               [IsActive]                     = @IsActive,
                               [site_id]                      = @SiteId,
                               [LastUpdatedBy]              = @LastUpdatedBy,
                               [LastUpdatedDate]              = GETDATE()
                               where CampaignCustomerProfileMapId = @CampaignCustomerProfileMapId
                             SELECT * FROM CampaignCustomerProfileMap WHERE CampaignCustomerProfileMapId = @CampaignCustomerProfileMapId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignCustomerProfileMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignCustomerProfileMapDTO(campaignCustomerProfileMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignCustomerProfileMapDTO);
            return campaignCustomerProfileMapDTO;
        }

        /// <summary>
        /// Converts the Data row object to CampaignCustomerProfileMapDTO class type
        /// </summary>
        /// <param name="CampaignCustomerProfileMapDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns CampaignCustomerProfileMapFormat</returns>
        private CampaignCustomerProfileMapDTO GetCampaignCustomerProfileMapDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CampaignCustomerProfileMapDTO CampaignCustomerProfileMapDataObject = new CampaignCustomerProfileMapDTO(Convert.ToInt32(dataRow["CampaignCustomerProfileMapId"]),
                                                    dataRow["CampaignDefinitionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignDefinitionId"]),
                                                    dataRow["CampaignCustomerProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignCustomerProfileId"]),
                                                    dataRow["CreatedBy"].ToString(),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"].ToString(),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                    );
            log.LogMethodExit();
            return CampaignCustomerProfileMapDataObject;
        }

        /// <summary>
        /// Gets the GetCampaignCustomerProfileMap data of passed displaygroup
        /// </summary>
        /// <param name="campaignCustomerProfileMapId">integer type parameter</param>
        /// <returns>Returns CampaignCustomerProfileMapDTO</returns>
        internal CampaignCustomerProfileMapDTO GetCampaignCustomerProfileMap(int campaignCustomerProfileMapId)
        {
            log.LogMethodEntry(campaignCustomerProfileMapId);
            CampaignCustomerProfileMapDTO result = null;
            string query = SELECT_QUERY + @" WHERE ccpm.CampaignCustomerProfileMapId = @CampaignCustomerProfileMapId";
            SqlParameter parameter = new SqlParameter("@CampaignCustomerProfileMapId", campaignCustomerProfileMapId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCampaignCustomerProfileMapDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }



        /// <summary>
        /// Gets the CampaignCustomerProfileMapDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CampaignCustomerProfileMapDTO matching the search criteria</returns>    
        internal List<CampaignCustomerProfileMapDTO> GetCampaignCustomerProfileMapList(List<KeyValuePair<CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CampaignCustomerProfileMapDTO> campaignCustomerProfileMapDTOList = new List<CampaignCustomerProfileMapDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.CAMPAIGN_CUSTOMER_PROFILE_ID ||
                            searchParameter.Key == CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.CAMPAIGN_CUSTOMER_PROFILE_MAP_ID ||
                            searchParameter.Key == CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.CAMPAIGN_CUSTOMER_PROFILE_ID ||
                            searchParameter.Key == CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.SITE_ID )
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters.IS_ACTIVE)
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
                campaignCustomerProfileMapDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetCampaignCustomerProfileMapDTO(x)).ToList();
            }
            log.LogMethodExit(campaignCustomerProfileMapDTOList);
            return campaignCustomerProfileMapDTOList;
        }
    }
}