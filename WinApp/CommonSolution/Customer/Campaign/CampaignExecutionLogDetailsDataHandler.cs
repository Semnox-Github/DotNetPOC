/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - CampaignExecutionLogDetail
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
    class CampaignExecutionLogDetailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CampaignExecutionLogDetail AS celd ";


        private static readonly Dictionary<CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters, string> DBSearchParameters = new Dictionary<CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters, string>
        {
             {CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.CAMPAIGN_EXECUTION_LOG_DETAIL_ID , "celd.CampaignExecutionLogDetailId"},
             {CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.SITE_ID , "celd.site_id"},
             {CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.MASTER_ENTITY_ID , "celd.MasterEntityId"},
             {CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.IS_ACTIVE , "celd.IsActive"},
             {CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.CAMPAIGN_EXECUTION_LOG_ID , "celd.CampaignExecutionLogId"},
             {CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.ACCOUNT_ID , "celd.AccountId"},
             {CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.CONTACT_ID , "celd.ContactId"},
             {CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.CUSTOMER_ID , "celd.CustomerId"},
             {CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.COUPON_SET_ID , "celd.CouponSetId"}

        };

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS CampaignExecutionLogDetailType;
                                            MERGE INTO CampaignExecutionLogDetail tbl
                                            USING @CampaignExecutionLogDetailList AS src
                                            ON src.CampaignExecutionLogDetailId = tbl.CampaignExecutionLogDetailId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            CampaignExecutionLogId = src.CampaignExecutionLogId,
                                            ContactId = src.ContactId,
                                            CustomerId = src.CustomerId,
                                            AccountId = src.AccountId,
                                            CouponSetId = src.CouponSetId,
                                            IsActive = src.IsActive,
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            LastUpdatedDate = GETDATE(),
                                            MasterEntityId = src.MasterEntityId,
                                            site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
                                            CampaignExecutionLogId,
                                            ContactId,
                                            CustomerId,
                                            AccountId,
                                            CouponSetId,
                                            IsActive,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate
                                            )VALUES (
                                            src.CampaignExecutionLogId,
                                            src.ContactId,
                                            src.CustomerId,
                                            src.AccountId,
                                            src.CouponSetId,
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
                                            inserted.CampaignExecutionLogDetailId,
                                            inserted.CampaignExecutionLogId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdatedDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            CampaignExecutionLogDetailId,
                                            CampaignExecutionLogId,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdatedDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion


        /// <summary>
        /// Default constructor of CampaignExecutionLogDetailDataHandler class
        /// </summary>
        public CampaignExecutionLogDetailDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Inserts the CampaignExecutionLogDetail record to the database
        /// </summary>
        /// <param name="CampaignExecutionLogDetailDTO">CampaignExecutionLogDetailDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(CampaignExecutionLogDetailDTO CampaignExecutionLogDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(CampaignExecutionLogDetailDTO, loginId, siteId);
            Save(new List<CampaignExecutionLogDetailDTO>() { CampaignExecutionLogDetailDTO }, loginId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the CampaignExecutionLogDetail record to the database
        /// </summary>
        /// <param name="campaignExecutionLogDetailDTOList">List of CampaignExecutionLogDetailDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignExecutionLogDetailDTOList, loginId, siteId);
            Dictionary<string, CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOGuidMap = GetCampaignExecutionLogDetailDTOGuidMap(campaignExecutionLogDetailDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(campaignExecutionLogDetailDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "CampaignExecutionLogDetailType",
                                                                "@CampaignExecutionLogDetailList");
            Update(campaignExecutionLogDetailDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(campaignExecutionLogDetailDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[14];
            columnStructures[0] = new SqlMetaData("CampaignExecutionLogDetailId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("CampaignExecutionLogId", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("ContactId", SqlDbType.Int);
            columnStructures[3] = new SqlMetaData("CustomerId", SqlDbType.Int);
            columnStructures[4] = new SqlMetaData("AccountId", SqlDbType.Int);
            columnStructures[5] = new SqlMetaData("CouponSetId", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("IsActive", SqlDbType.Bit);
            columnStructures[7] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[8] = new SqlMetaData("LastUpdatedDate", SqlDbType.DateTime);
            columnStructures[9] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[11] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[12] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[13] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[14] = new SqlMetaData("CreationDate", SqlDbType.DateTime);

            for (int i = 0; i < campaignExecutionLogDetailDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].CampaignExecutionLogDetailId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].CampaignExecutionLogId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].ContactId));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].CustomerId));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].AccountId));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].CouponSetId));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].IsActive));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].LastUpdatedBy));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(Guid.Parse(campaignExecutionLogDetailDTOList[i].Guid)));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].SynchStatus));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(campaignExecutionLogDetailDTOList[i].CreationDate));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, CampaignExecutionLogDetailDTO> GetCampaignExecutionLogDetailDTOGuidMap(List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOList)
        {
            Dictionary<string, CampaignExecutionLogDetailDTO> result = new Dictionary<string, CampaignExecutionLogDetailDTO>();
            for (int i = 0; i < campaignExecutionLogDetailDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(campaignExecutionLogDetailDTOList[i].Guid))
                {
                    campaignExecutionLogDetailDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(campaignExecutionLogDetailDTOList[i].Guid, campaignExecutionLogDetailDTOList[i]);
            }
            return result;
        }

        private void Update(Dictionary<string, CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                CampaignExecutionLogDetailDTO campaignExecutionLogDetailDTO = campaignExecutionLogDetailDTOGuidMap[Convert.ToString(row["Guid"])];
                campaignExecutionLogDetailDTO.CampaignExecutionLogDetailId = row["CampaignExecutionLogDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(row["CampaignExecutionLogDetailId"]);
                campaignExecutionLogDetailDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                campaignExecutionLogDetailDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                campaignExecutionLogDetailDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                campaignExecutionLogDetailDTO.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdatedDate"]);
                campaignExecutionLogDetailDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                campaignExecutionLogDetailDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Converts the Data row object to CampaignExecutionLogDetailDTO class type
        /// </summary>
        /// <param name="CampaignExecutionLogDetailDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns CampaignExecutionLogDetailFormat</returns>
        private CampaignExecutionLogDetailDTO GetCampaignExecutionLogDetailDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CampaignExecutionLogDetailDTO campaignExecutionLogDetailDataObject = new CampaignExecutionLogDetailDTO(Convert.ToInt32(dataRow["CampaignExecutionLogDetailId"]),
                                                    dataRow["CampaignExecutionLogId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignExecutionLogId"]),
                                                    dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                                    dataRow["AccountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AccountId"]),
                                                    dataRow["ContactId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ContactId"]),
                                                    dataRow["CouponSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CouponSetId"]),
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
            return campaignExecutionLogDetailDataObject;
        }

        /// <summary>
        /// Gets the GetCampaignExecutionLogDetail data of passed displaygroup
        /// </summary>
        /// <param name="campaignExecutionLogDetailId">integer type parameter</param>
        /// <returns>Returns CampaignExecutionLogDetailDTO</returns>
        internal CampaignExecutionLogDetailDTO GetCampaignExecutionLogDetail(int campaignExecutionLogDetailId)
        {
            log.LogMethodEntry(campaignExecutionLogDetailId);
            CampaignExecutionLogDetailDTO result = null;
            string query = SELECT_QUERY + @" WHERE celd.CampaignExecutionLogDetailId = @CampaignExecutionLogDetailId";
            SqlParameter parameter = new SqlParameter("@CampaignExecutionLogDetailId", campaignExecutionLogDetailId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCampaignExecutionLogDetailDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<CampaignExecutionLogDetailDTO> GetCampaignExecutionLogDetailDTOList(List<int> campaignExecutionLogIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(campaignExecutionLogIdList);
            List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOList = new List<CampaignExecutionLogDetailDTO>();
            string query = @"SELECT *
                            FROM CampaignExecutionLogDetail, @campaignExecutionLogIdList List
                            WHERE CampaignExecutionLogId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@campaignExecutionLogIdList", campaignExecutionLogIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                campaignExecutionLogDetailDTOList = table.Rows.Cast<DataRow>().Select(x => GetCampaignExecutionLogDetailDTO(x)).ToList();
            }
            log.LogMethodExit(campaignExecutionLogDetailDTOList);
            return campaignExecutionLogDetailDTOList;
        }

        /// <summary>
        /// Gets the CampaignExecutionLogDetailDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CampaignExecutionLogDetailDTO matching the search criteria</returns>    
        internal List<CampaignExecutionLogDetailDTO> GetCampaignExecutionLogDetailList(List<KeyValuePair<CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOList = new List<CampaignExecutionLogDetailDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.CAMPAIGN_EXECUTION_LOG_DETAIL_ID ||
                            searchParameter.Key == CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.ACCOUNT_ID ||
                            searchParameter.Key == CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.CONTACT_ID ||
                            searchParameter.Key == CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.COUPON_SET_ID ||
                            searchParameter.Key == CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.CUSTOMER_ID ||
                            searchParameter.Key == CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.CAMPAIGN_EXECUTION_LOG_ID ||
                            searchParameter.Key == CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.IS_ACTIVE)
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
                campaignExecutionLogDetailDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetCampaignExecutionLogDetailDTO(x)).ToList();
            }
            log.LogMethodExit(campaignExecutionLogDetailDTOList);
            return campaignExecutionLogDetailDTOList;
        }
    }
}