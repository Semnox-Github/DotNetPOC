/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - CampaignCustomerProfileDetail
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.110.0     21-Jan-2020      Prajwal             Created
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
    class CampaignCustomerProfileDetailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CampaignCustomerProfileDetail AS ccpd ";


        private static readonly Dictionary<CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters, string> DBSearchParameters = new Dictionary<CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters, string>
        {
             {CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.CAMPAIGN_CUSTOMER_PROFILE_DETAIL_ID , "ccpd.CampaignCustomerProfileDetailId"},
              {CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.CAMPAIGN_CUSTOMER_PROFILE_ID , "ccpd.CampaignCustomerProfileId"},
               {CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.SITE_ID , "ccpd.site_id"},
                {CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.MASTER_ENTITY_ID , "ccpd.MasterEntityId"},
                 {CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.IS_ACTIVE , "ccpd.IsActive"},
                 {CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.ACCOUNT_ID , "ccpd.AccountId"},
            {CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.CUSTOMER_ID , "ccpd.CustomerId"},
            {CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.CONTACT_ID , "ccpd.ContactId"}

        };

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS CampaignCustomerProfileDetailType;
                                            MERGE INTO CampaignCustomerProfileDetail tbl
                                            USING @CampaignCustomerProfileDetailList AS src
                                            ON src.CampaignCustomerProfileDetailId = tbl.CampaignCustomerProfileDetailId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            CampaignCustomerProfileId = src.CampaignCustomerProfileId,
                                            ContactId = src.ContactId,
                                            CustomerId = src.CustomerId,
                                            AccountId = src.AccountId,
                                            IsActive = src.IsActive,
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            LastUpdatedDate = GETDATE(),
                                            MasterEntityId = src.MasterEntityId,
                                            site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
                                            CampaignCustomerProfileId,
                                            ContactId,
                                            CustomerId,
                                            AccountId,
                                            IsActive,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate
                                            )VALUES (
                                            src.CampaignCustomerProfileId,
                                            src.ContactId,
                                            src.CustomerId,
                                            src.AccountId,
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
                                            inserted.CampaignCustomerProfileDetailId,
                                            inserted.CampaignCustomerProfileId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdatedDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            CampaignCustomerProfileDetailId,
                                            CampaignCustomerProfileId,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdatedDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion


        /// <summary>
        /// Default constructor of CampaignCustomerProfileDetailDataHandler class
        /// </summary>
        public CampaignCustomerProfileDetailDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Inserts the CampaignCustomerProfileDetail record to the database
        /// </summary>
        /// <param name="campaignCustomerProfileDetailDTO">CampaignCustomerProfileDetailDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(CampaignCustomerProfileDetailDTO campaignCustomerProfileDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCustomerProfileDetailDTO, loginId, siteId);
            Save(new List<CampaignCustomerProfileDetailDTO>() { campaignCustomerProfileDetailDTO }, loginId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the CampaignCustomerProfileDetail record to the database
        /// </summary>
        /// <param name="campaignCustomerProfileDetailDTOList">List of CampaignCustomerProfileDetailDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public List<CampaignCustomerProfileDetailDTO> Save(List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCustomerProfileDetailDTOList, loginId, siteId);
            Dictionary<string, CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOGuidMap = GetCampaignCustomerProfileDetailDTOGuidMap(campaignCustomerProfileDetailDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(campaignCustomerProfileDetailDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "CampaignCustomerProfileDetailType",
                                                                "@CampaignCustomerProfileDetailList");
            Update(campaignCustomerProfileDetailDTOGuidMap, dataTable);
            log.LogMethodExit(campaignCustomerProfileDetailDTOList);
            return campaignCustomerProfileDetailDTOList;
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(campaignCustomerProfileDetailDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[14];
            columnStructures[0] = new SqlMetaData("CampaignCustomerProfileDetailId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("CampaignCustomerProfileId", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("ContactId", SqlDbType.Int);
            columnStructures[3] = new SqlMetaData("CustomerId", SqlDbType.Int);
            columnStructures[4] = new SqlMetaData("AccountId", SqlDbType.Int);
            columnStructures[5] = new SqlMetaData("IsActive", SqlDbType.Bit);
            columnStructures[6] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[7] = new SqlMetaData("LastUpdatedDate", SqlDbType.DateTime);
            columnStructures[8] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[9] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[10] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[11] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[12] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[13] = new SqlMetaData("CreationDate", SqlDbType.DateTime);

            for (int i = 0; i < campaignCustomerProfileDetailDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].CampaignCustomerProfileDetailId));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].CampaignCustomerProfileId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].ContactId, true));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].CustomerId, true));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].AccountId, true));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].IsActive));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].LastUpdatedBy));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(Guid.Parse(campaignCustomerProfileDetailDTOList[i].Guid)));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].SynchStatus));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(campaignCustomerProfileDetailDTOList[i].CreationDate));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, CampaignCustomerProfileDetailDTO> GetCampaignCustomerProfileDetailDTOGuidMap(List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOList)
        {
            Dictionary<string, CampaignCustomerProfileDetailDTO> result = new Dictionary<string, CampaignCustomerProfileDetailDTO>();
            for (int i = 0; i < campaignCustomerProfileDetailDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(campaignCustomerProfileDetailDTOList[i].Guid))
                {
                    campaignCustomerProfileDetailDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(campaignCustomerProfileDetailDTOList[i].Guid, campaignCustomerProfileDetailDTOList[i]);
            }
            return result;
        }

        private void Update(Dictionary<string, CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                CampaignCustomerProfileDetailDTO campaignCustomerProfileDetailDTO = campaignCustomerProfileDetailDTOGuidMap[Convert.ToString(row["Guid"])];
                campaignCustomerProfileDetailDTO.CampaignCustomerProfileDetailId = row["CampaignCustomerProfileDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(row["CampaignCustomerProfileDetailId"]);
                campaignCustomerProfileDetailDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                campaignCustomerProfileDetailDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                campaignCustomerProfileDetailDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                campaignCustomerProfileDetailDTO.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdatedDate"]);
                campaignCustomerProfileDetailDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                campaignCustomerProfileDetailDTO.AcceptChanges();
            }
        }


        /// <summary>
        /// Converts the Data row object to CampaignCustomerProfileDetailDTO class type
        /// </summary>
        /// <param name="CampaignCustomerProfileDetailDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns CampaignCustomerProfileDetailFormat</returns>
        private CampaignCustomerProfileDetailDTO GetCampaignCustomerProfileDetailDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CampaignCustomerProfileDetailDTO campaignCustomerProfileDetailDataObject = new CampaignCustomerProfileDetailDTO(Convert.ToInt32(dataRow["CampaignCustomerProfileDetailId"]),
                                                    dataRow["CampaignCustomerProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignCustomerProfileId"]),
                                                    dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                                    dataRow["AccountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AccountId"]),
                                                    dataRow["ContactId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ContactId"]),
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
            return campaignCustomerProfileDetailDataObject;
        }

        /// <summary>
        /// Gets the GetCampaignCustomerProfileDetail data of passed displaygroup
        /// </summary>
        /// <param name="campaignCustomerProfileDetailId">integer type parameter</param>
        /// <returns>Returns CampaignCustomerProfileDetailDTO</returns>
        internal CampaignCustomerProfileDetailDTO GetCampaignCustomerProfileDetail(int campaignCustomerProfileDetailId)
        {
            log.LogMethodEntry(campaignCustomerProfileDetailId);
            CampaignCustomerProfileDetailDTO result = null;
            string query = SELECT_QUERY + @" WHERE ccpd.CampaignCustomerProfileDetailId = @CampaignCustomerProfileDetailId";
            SqlParameter parameter = new SqlParameter("@CampaignCustomerProfileDetailId", campaignCustomerProfileDetailId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCampaignCustomerProfileDetailDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<CampaignCustomerProfileDetailDTO> GetCampaignCustomerProfileDetailDTOList(List<int> campaignCustomerProfileIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(campaignCustomerProfileIdList);
            List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOList = new List<CampaignCustomerProfileDetailDTO>();
            string query = @"SELECT *
                            FROM CampaignCustomerProfileDetail, @campaignCustomerProfileIdList List
                            WHERE CampaignCustomerProfileId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@campaignCustomerProfileIdList", campaignCustomerProfileIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                campaignCustomerProfileDetailDTOList = table.Rows.Cast<DataRow>().Select(x => GetCampaignCustomerProfileDetailDTO(x)).ToList();
            }
            log.LogMethodExit(campaignCustomerProfileDetailDTOList);
            return campaignCustomerProfileDetailDTOList;
        }


        /// <summary>
        /// Gets the CampaignCustomerProfileDetailDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CampaignCustomerProfileDetailDTO matching the search criteria</returns>    
        internal List<CampaignCustomerProfileDetailDTO> GetCampaignCustomerProfileDetailList(List<KeyValuePair<CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOList = new List<CampaignCustomerProfileDetailDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.CAMPAIGN_CUSTOMER_PROFILE_ID ||
                            searchParameter.Key == CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.CAMPAIGN_CUSTOMER_PROFILE_DETAIL_ID ||
                            searchParameter.Key == CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.ACCOUNT_ID ||
                            searchParameter.Key == CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.CONTACT_ID ||
                            searchParameter.Key == CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.CUSTOMER_ID ||
                            searchParameter.Key == CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.SITE_ID )
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters.IS_ACTIVE)
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
                campaignCustomerProfileDetailDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetCampaignCustomerProfileDetailDTO(x)).ToList();
            }
            log.LogMethodExit(campaignCustomerProfileDetailDTOList);
            return campaignCustomerProfileDetailDTOList;
        }
    }
}