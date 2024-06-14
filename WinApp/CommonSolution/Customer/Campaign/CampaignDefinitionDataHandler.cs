/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - CampaignDefinition
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
    class CampaignDefinitionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CampaignDefinition AS cd ";


        private static readonly Dictionary<CampaignDefinitionDTO.SearchByCampaignDefinitionParameters, string> DBSearchParameters = new Dictionary<CampaignDefinitionDTO.SearchByCampaignDefinitionParameters, string>
        {
              {CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.CAMPAIGN_DEFINITION_ID , "cd.CampaignDefinitionId"},
              {CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.SCHEDULE_ID , "cd.ScheduleId"},
               {CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.SITE_ID , "cd.site_id"},
                {CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.MASTER_ENTITY_ID , "cd.MasterEntityId"},
                 {CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.IS_ACTIVE , "cd.IsActive"},
                  {CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.CAMPAIGN_DEFINITION_ID_LIST , "cd.CampaignDefinitionId"}
        };

        /// <summary>
        /// Default constructor of CampaignDefinitionDataHandler class
        /// </summary>
        public CampaignDefinitionDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(CampaignDefinitionDTO CampaignDefinitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(CampaignDefinitionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignDefinitionId", CampaignDefinitionDTO.CampaignDefinitionId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScheduleId", CampaignDefinitionDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", CampaignDefinitionDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", CampaignDefinitionDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Startdate", CampaignDefinitionDTO.StartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EndDate", CampaignDefinitionDTO.EndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Recurr", CampaignDefinitionDTO.Recurr));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", CampaignDefinitionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", CampaignDefinitionDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", CampaignDefinitionDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the userRole DisplayGroups record to the database
        /// </summary>
        /// <param name="CampaignDefinition">CampaignDefinitionDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal CampaignDefinitionDTO Insert(CampaignDefinitionDTO CampaignDefinitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(CampaignDefinitionDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[CampaignDefinition] 
                                                        (                                                 
                                                         ScheduleId,
                                                         Name,
                                                         Description,
                                                         Startdate,
                                                         EndDate,
                                                         Recurr,
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
                                                          @ScheduleId,
                                                          @Name,
                                                          @Description,
                                                          @Startdate,
                                                          @EndDate,
                                                          @Recurr,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @IsActive,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM CampaignDefinition WHERE CampaignDefinitionId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(CampaignDefinitionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignDefinitionDTO(CampaignDefinitionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(CampaignDefinitionDTO);
            return CampaignDefinitionDTO;
        }

        private void RefreshCampaignDefinitionDTO(CampaignDefinitionDTO campaignDefinitionDTO, DataTable dt)
        {
            log.LogMethodEntry(campaignDefinitionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                campaignDefinitionDTO.CampaignDefinitionId = Convert.ToInt32(dt.Rows[0]["CampaignDefinitionId"]);
                campaignDefinitionDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                campaignDefinitionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                campaignDefinitionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                campaignDefinitionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                campaignDefinitionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                campaignDefinitionDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the CampaignDefinition  record
        /// </summary>
        /// <param name="CampaignDefinition">CampaignDefinitionDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal CampaignDefinitionDTO Update(CampaignDefinitionDTO CampaignDefinitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(CampaignDefinitionDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CampaignDefinition] set
                               [Name]                     = @Name,
                               [Description]              = @Description,
                               [Startdate]                = @Startdate,
                               [EndDate]                  = @EndDate,
                               [ScheduleId]               = @ScheduleId,
                               [site_id]                  = @SiteId,
                               [IsActive]                 = @IsActive,
                               [MasterEntityId]           = @MasterEntityId,
                               [Recurr]                   = @Recurr,
                               [LastUpdatedBy]            = @LastUpdatedBy,
                               [LastUpdatedDate]          = GETDATE()
                               where CampaignDefinitionId = @CampaignDefinitionId
                             SELECT * FROM CampaignDefinition WHERE CampaignDefinitionId = @CampaignDefinitionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(CampaignDefinitionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignDefinitionDTO(CampaignDefinitionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(CampaignDefinitionDTO);
            return CampaignDefinitionDTO;
        }

        /// <summary>
        /// Converts the Data row object to CampaignDefinitionDTO class type
        /// </summary>
        /// <param name="CampaignDefinitionDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns CampaignDefinition</returns>
        private CampaignDefinitionDTO GetCampaignDefinitionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CampaignDefinitionDTO CampaignDefinitionDataObject = new CampaignDefinitionDTO(Convert.ToInt32(dataRow["CampaignDefinitionId"]),
                                                    dataRow["Name"] == DBNull.Value ? string.Empty : (dataRow["Name"]).ToString(),
                                                    dataRow["Description"] == DBNull.Value ? string.Empty : (dataRow["Description"]).ToString(),
                                                    dataRow["StartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["StartDate"]),
                                                    dataRow["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EndDate"]),
                                                    dataRow["Recurr"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["Recurr"]),
                                                    dataRow["ScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScheduleId"]),
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
            return CampaignDefinitionDataObject;
        }

        /// <summary>
        /// Gets the GetCampaignDefinition data of passed displaygroup
        /// </summary>
        /// <param name="campaignDefinitionId">integer type parameter</param>
        /// <returns>Returns CampaignDefinitionDTO</returns>
        internal CampaignDefinitionDTO GetCampaignDefinition(int campaignDefinitionId)
        {
            log.LogMethodEntry(campaignDefinitionId);
            CampaignDefinitionDTO result = null;
            string query = SELECT_QUERY + @" WHERE cd.CampaignDefinitionId = @CampaignDefinitionId";
            SqlParameter parameter = new SqlParameter("@CampaignDefinitionId", campaignDefinitionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCampaignDefinitionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }



        /// <summary>
        /// Gets the CampaignDefinitionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CampaignDefinitionDTO matching the search criteria</returns>    
        internal List<CampaignDefinitionDTO> GetCampaignDefinitionList(List<KeyValuePair<CampaignDefinitionDTO.SearchByCampaignDefinitionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CampaignDefinitionDTO> campaignDefinitionDTOList = new List<CampaignDefinitionDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CampaignDefinitionDTO.SearchByCampaignDefinitionParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.CAMPAIGN_DEFINITION_ID ||
                            searchParameter.Key == CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.SCHEDULE_ID ||
                            searchParameter.Key == CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.CAMPAIGN_DEFINITION_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }

                        else if (searchParameter.Key == CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignDefinitionDTO.SearchByCampaignDefinitionParameters.IS_ACTIVE)
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
                campaignDefinitionDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetCampaignDefinitionDTO(x)).ToList();
            }
            log.LogMethodExit(campaignDefinitionDTOList);
            return campaignDefinitionDTOList;
        }
    }
}