/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - CampaignExecutionLog
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.110.0     18-Jan-2020      Prajwal             Created
********************************************************************************************/
using Microsoft.SqlServer.Server;
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
    class CampaignExecutionLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CampaignExecutionLog AS cel ";


        private static readonly Dictionary<CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters, string> DBSearchParameters = new Dictionary<CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters, string>
        {
             {CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters.CAMPAIGN_EXECUTION_LOG_ID , "cel.CampaignExecutionLogId"},
             {CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters.SITE_ID , "cel.site_id"},
             {CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters.MASTER_ENTITY_ID , "cel.MasterEntityId"},
             {CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters.IS_ACTIVE , "cel.IsActive"},
             {CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters.CAMPAIGN_DEFINITION_ID , "cel.CampaignDefinitionId"}

        };

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS CampaignExecutionLogType;
                                            MERGE INTO CampaignExecutionLog tbl
                                            USING @CampaignExecutionLogList AS src
                                            ON src.CampaignExecutionLogId = tbl.CampaignExecutionLogId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            CampaignDefinitionId = src.CampaignDefinitionId,
                                            RunDate = src.RunDate,
                                            IsActive = src.IsActive,
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            LastUpdatedDate = GETDATE(),
                                            MasterEntityId = src.MasterEntityId,
                                            site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
                                            CampaignDefinitionId,
                                            RunDate,
                                            IsActive,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate
                                            )VALUES (
                                            src.CampaignDefinitionId,
                                            src.RunDate,
                                            src.IsActive,
                                            src.LastUpdatedBy,
                                            GETDATE(),
                                            src.site_id,
                                            src.Guid,
                                            src.MasterEntityId,
                                            src.CreatedBy,
                                            GETDATE()
                                            )
                                            OUTPUT
                                            inserted.CampaignExecutionLogId,
                                            inserted.CampaignDefinitionId,
                                            inserted.RunDate,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdatedDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            CampaignExecutionLogId,
                                            CampaignDefinitionId,
                                            RunDate,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdatedDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion

        /// <summary>
        /// Default constructor of CampaignExecutionLogDataHandler class
        /// </summary>
        public CampaignExecutionLogDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the CampaignExecutionLog record to the database
        /// </summary>
        /// <param name="campaignExecutionLogDTO">CampaignExecutionLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(CampaignExecutionLogDTO campaignExecutionLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignExecutionLogDTO, loginId, siteId);
            Save(new List<CampaignExecutionLogDTO>() { campaignExecutionLogDTO }, loginId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the CampaignExecutionLog record to the database
        /// </summary>
        /// <param name="campaignExecutionLogDTOList">List of CampaignExecutionLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<CampaignExecutionLogDTO> campaignExecutionLogDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignExecutionLogDTOList, loginId, siteId);
            Dictionary<string, CampaignExecutionLogDTO> campaignExecutionLogDTOGuidMap = GetCampaignExecutionLogDTOGuidMap(campaignExecutionLogDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(campaignExecutionLogDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "CampaignExecutionLogType",
                                                                "@CampaignExecutionLogList");
            Update(campaignExecutionLogDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<CampaignExecutionLogDTO> campaignExecutionLogDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(campaignExecutionLogDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[12];
            columnStructures[0] = new SqlMetaData("CampaignExecutionLogId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("CampaignDefinitionId", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("RunDate", SqlDbType.DateTime);
            columnStructures[3] = new SqlMetaData("IsActive", SqlDbType.Bit);
            columnStructures[4] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[5] = new SqlMetaData("LastUpdatedDate", SqlDbType.DateTime);
            columnStructures[6] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[7] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[8] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[9] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[11] = new SqlMetaData("CreationDate", SqlDbType.DateTime);

            for (int i = 0; i < campaignExecutionLogDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(campaignExecutionLogDTOList[i].CampaignExecutionLogId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(campaignExecutionLogDTOList[i].CampaignDefinitionId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(campaignExecutionLogDTOList[i].RunDate));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(campaignExecutionLogDTOList[i].IsActive));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(campaignExecutionLogDTOList[i].LastUpdatedBy));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(campaignExecutionLogDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(Guid.Parse(campaignExecutionLogDTOList[i].Guid)));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(campaignExecutionLogDTOList[i].SynchStatus));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(campaignExecutionLogDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(campaignExecutionLogDTOList[i].CreationDate));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, CampaignExecutionLogDTO> GetCampaignExecutionLogDTOGuidMap(List<CampaignExecutionLogDTO> CampaignExecutionLogDTOList)
        {
            Dictionary<string, CampaignExecutionLogDTO> result = new Dictionary<string, CampaignExecutionLogDTO>();
            for (int i = 0; i < CampaignExecutionLogDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(CampaignExecutionLogDTOList[i].Guid))
                {
                    CampaignExecutionLogDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(CampaignExecutionLogDTOList[i].Guid, CampaignExecutionLogDTOList[i]);
            }
            return result;
        }

        private void Update(Dictionary<string, CampaignExecutionLogDTO> CampaignExecutionLogDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                CampaignExecutionLogDTO CampaignExecutionLogDTO = CampaignExecutionLogDTOGuidMap[Convert.ToString(row["Guid"])];
                CampaignExecutionLogDTO.CampaignExecutionLogId = row["CampaignExecutionLogId"] == DBNull.Value ? -1 : Convert.ToInt32(row["CampaignExecutionLogId"]);
                CampaignExecutionLogDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                CampaignExecutionLogDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                CampaignExecutionLogDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                CampaignExecutionLogDTO.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdatedDate"]);
                CampaignExecutionLogDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                CampaignExecutionLogDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Converts the Data row object to CampaignExecutionLogDTO class type
        /// </summary>
        /// <param name="CampaignExecutionLogDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns CampaignExecutionLogFormat</returns>
        private CampaignExecutionLogDTO GetCampaignExecutionLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CampaignExecutionLogDTO CampaignExecutionLogDataObject = new CampaignExecutionLogDTO(Convert.ToInt32(dataRow["CampaignExecutionLogId"]),
                                                    dataRow["CampaignDefinitionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignDefinitionId"]),
                                                    dataRow["RunDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["RunDate"]),
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
            return CampaignExecutionLogDataObject;
        }

        /// <summary>
        /// Gets the GetCampaignExecutionLog data of passed displaygroup
        /// </summary>
        /// <param name="campaignExecutionLogId">integer type parameter</param>
        /// <returns>Returns CampaignExecutionLogDTO</returns>
        internal CampaignExecutionLogDTO GetCampaignExecutionLog(int campaignExecutionLogId)
        {
            log.LogMethodEntry(campaignExecutionLogId);
            CampaignExecutionLogDTO result = null;
            string query = SELECT_QUERY + @" WHERE cel.CampaignExecutionLogId = @CampaignExecutionLogId";
            SqlParameter parameter = new SqlParameter("@CampaignExecutionLogId", campaignExecutionLogId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCampaignExecutionLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }



        /// <summary>
        /// Gets the CampaignExecutionLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CampaignExecutionLogDTO matching the search criteria</returns>    
        internal List<CampaignExecutionLogDTO> GetCampaignExecutionLogList(List<KeyValuePair<CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CampaignExecutionLogDTO> campaignExecutionLogDTOList = new List<CampaignExecutionLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters.CAMPAIGN_DEFINITION_ID ||
                            searchParameter.Key == CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters.CAMPAIGN_EXECUTION_LOG_ID ||
                            searchParameter.Key == CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters.IS_ACTIVE)
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
                campaignExecutionLogDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetCampaignExecutionLogDTO(x)).ToList();
            }
            log.LogMethodExit(campaignExecutionLogDTOList);
            return campaignExecutionLogDTOList;
        }
    }
}