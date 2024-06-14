/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - CampaignDiscountDefinition
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.110.0     19-Jan-2020      Prajwal             Created
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
    class CampaignDiscountDefinitionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CampaignDiscountDefinition AS cdd ";


        private static readonly Dictionary<CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters, string> DBSearchParameters = new Dictionary<CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters, string>
        {
              {CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.CAMPAIGN_DISCOUNT_DEFINITION_ID , "cdd.CampaignDiscountDefinitionId"},
              {CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.DISCOUNT_ID , "cdd.DiscountId"},
              {CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.CAMPAIGN_DEFINITION_ID , "cdd.CampaignDefinitionId"},
               {CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.SITE_ID , "cdd.site_id"},
                {CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.MASTER_ENTITY_ID , "cdd.MasterEntityId"},
                 {CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.IS_ACTIVE , "cdd.IsActive"}
        };

        /// <summary>
        /// Default constructor of CampaignDiscountDefinitionDataHandler class
        /// </summary>
        public CampaignDiscountDefinitionDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(CampaignDiscountDefinitionDTO campaignDiscountDefinitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignDiscountDefinitionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignDiscountDefinitionId", campaignDiscountDefinitionDTO.CampaignDiscountDefinitionId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountId", campaignDiscountDefinitionDTO.DiscountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", campaignDiscountDefinitionDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidFor", campaignDiscountDefinitionDTO.ValidFor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidForDaysMonths", campaignDiscountDefinitionDTO.ValidForDaysMonths));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignDefinitionId", campaignDiscountDefinitionDTO.CampaignDefinitionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", campaignDiscountDefinitionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", campaignDiscountDefinitionDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", campaignDiscountDefinitionDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the CampaignDiscountDefinition record to the database
        /// </summary>
        /// <param name="CampaignDiscountDefinition">CampaignDiscountDefinitionDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal CampaignDiscountDefinitionDTO Insert(CampaignDiscountDefinitionDTO campaignDiscountDefinitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignDiscountDefinitionDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[CampaignDiscountDefinition] 
                                                        (                                                 
                                                         CampaignDefinitionId,
                                                         DiscountId,
                                                         ExpiryDate,
                                                         ValidFor,
                                                         ValidForDaysMonths,
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
                                                          @DiscountId,
                                                          @ExpiryDate,
                                                          @ValidFor,
                                                          @ValidForDaysMonths,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @IsActive,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM CampaignDiscountDefinition WHERE CampaignDiscountDefinitionId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignDiscountDefinitionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignDiscountDefinitionDTO(campaignDiscountDefinitionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignDiscountDefinitionDTO);
            return campaignDiscountDefinitionDTO;
        }

        private void RefreshCampaignDiscountDefinitionDTO(CampaignDiscountDefinitionDTO campaignDiscountDefinitionDTO, DataTable dt)
        {
            log.LogMethodEntry(campaignDiscountDefinitionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                campaignDiscountDefinitionDTO.CampaignDiscountDefinitionId = Convert.ToInt32(dt.Rows[0]["CampaignDiscountDefinitionId"]);
                campaignDiscountDefinitionDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                campaignDiscountDefinitionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                campaignDiscountDefinitionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                campaignDiscountDefinitionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                campaignDiscountDefinitionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                campaignDiscountDefinitionDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the CampaignDiscountDefinition  record
        /// </summary>
        /// <param name="CampaignDiscountDefinition">CampaignDiscountDefinitionDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal CampaignDiscountDefinitionDTO Update(CampaignDiscountDefinitionDTO campaignDiscountDefinitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignDiscountDefinitionDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CampaignDiscountDefinition] set
                               [CampaignDefinitionId]         = @CampaignDefinitionId,
                               [DiscountId]                   = @DiscountId,
                               [ValidFor]                     = @ValidFor,
                               [ValidForDaysMonths]           = @ValidForDaysMonths,
                               [ExpiryDate]                   = @ExpiryDate,
                               [site_id]                      = @SiteId,
                               [MasterEntityId]               = @MasterEntityId,
                               [IsActive]                     = @IsActive,
                               [LastUpdatedBy]              = @LastUpdatedBy,
                               [LastUpdatedDate]              = GETDATE()
                               where CampaignDiscountDefinitionId = @CampaignDiscountDefinitionId
                             SELECT * FROM CampaignDiscountDefinition WHERE CampaignDiscountDefinitionId = @CampaignDiscountDefinitionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignDiscountDefinitionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignDiscountDefinitionDTO(campaignDiscountDefinitionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignDiscountDefinitionDTO);
            return campaignDiscountDefinitionDTO;
        }

        /// <summary>
        /// Converts the Data row object to CampaignDiscountDefinitionDTO class type
        /// </summary>
        /// <param name="CampaignDiscountDefinitionDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns CampaignDiscountDefinition</returns>
        private CampaignDiscountDefinitionDTO GetCampaignDiscountDefinitionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CampaignDiscountDefinitionDTO CampaignDiscountDefinitionDataObject = new CampaignDiscountDefinitionDTO(Convert.ToInt32(dataRow["CampaignDiscountDefinitionId"]),
                                                    dataRow["CampaignDefinitionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignDefinitionId"]),
                                                    dataRow["DiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DiscountId"]),
                                                    dataRow["ExpiryDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                                    dataRow["ValidFor"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ValidFor"]),
                                                    dataRow["ValidForDaysMonths"] == DBNull.Value ? string.Empty : (dataRow["ValidForDaysMonths"]).ToString(),
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
            return CampaignDiscountDefinitionDataObject;
        }

        /// <summary>
        /// Gets the GetCampaignDiscountDefinition data of passed displaygroup
        /// </summary>
        /// <param name="campaignDiscountDefinitionId">integer type parameter</param>
        /// <returns>Returns CampaignDiscountDefinitionDTO</returns>
        internal CampaignDiscountDefinitionDTO GetCampaignDiscountDefinition(int campaignDiscountDefinitionId)
        {
            log.LogMethodEntry(campaignDiscountDefinitionId);
            CampaignDiscountDefinitionDTO result = null;
            string query = SELECT_QUERY + @" WHERE cdd.CampaignDiscountDefinitionId = @CampaignDiscountDefinitionId";
            SqlParameter parameter = new SqlParameter("@CampaignDiscountDefinitionId", campaignDiscountDefinitionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCampaignDiscountDefinitionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<CampaignDiscountDefinitionDTO> GetCampaignDiscountDefinitionDTOList(List<int> campaignDefinitionIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(campaignDefinitionIdList);
            List<CampaignDiscountDefinitionDTO> campaignDiscountDefinitionDTOList = new List<CampaignDiscountDefinitionDTO>();
            string query = @"SELECT *
                            FROM CampaignDiscountDefinition, @campaignDefinitionIdList List
                            WHERE CampaignDefinitionId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@campaignDefinitionIdList", campaignDefinitionIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                campaignDiscountDefinitionDTOList = table.Rows.Cast<DataRow>().Select(x => GetCampaignDiscountDefinitionDTO(x)).ToList();
            }
            log.LogMethodExit(campaignDiscountDefinitionDTOList);
            return campaignDiscountDefinitionDTOList;
        }

        /// <summary>
        /// Gets the CampaignDiscountDefinitionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CampaignDiscountDefinitionDTO matching the search criteria</returns>    
        internal List<CampaignDiscountDefinitionDTO> GetCampaignDiscountDefinitionList(List<KeyValuePair<CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CampaignDiscountDefinitionDTO> campaignDiscountDefinitionDTOList = new List<CampaignDiscountDefinitionDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.CAMPAIGN_DEFINITION_ID ||
                            searchParameter.Key == CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.DISCOUNT_ID ||
                            searchParameter.Key == CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.CAMPAIGN_DISCOUNT_DEFINITION_ID ||
                            searchParameter.Key == CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.SITE_ID )
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters.IS_ACTIVE)
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
                campaignDiscountDefinitionDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetCampaignDiscountDefinitionDTO(x)).ToList();
            }
            log.LogMethodExit(campaignDiscountDefinitionDTOList);
            return campaignDiscountDefinitionDTOList;
        }
    }

}
