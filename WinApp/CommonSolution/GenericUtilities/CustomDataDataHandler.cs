/********************************************************************************************
* Project Name - CustomData Data Handler
* Description  - Data handler of the CustomData class
* CustomData
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*1.00        15-May-2017   Lakshminarayana     Created 
*2.60.2      29-May-2019   Jagan Mohan         Code merge from Development to WebManagementStudio
*2.70.2        25-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(), 
*                                                          SQL injection Issue Fix.
*2.70.2        06-Dec-2019   Jinto Thomas            Removed siteid from update query                                                           
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    ///  CustomData Data Handler - Handles insert, update and select of  CustomData objects
    /// </summary>
    public class CustomDataDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction = null;
        DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CustomData as cd ";

        /// <summary>
        /// Dictionary for searching Parameters for the CustomData object.
        /// </summary>
        private static readonly Dictionary<CustomDataDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomDataDTO.SearchByParameters, string>
            {
                {CustomDataDTO.SearchByParameters.CUSTOM_DATA_ID, "cd.CustomDataId"},
                {CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID, "cd.CustomDataSetId"},
                {CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID_LIST, "cd.CustomDataSetId"},
                {CustomDataDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, "cd.CustomAttributeId"},
                {CustomDataDTO.SearchByParameters.VALUE_ID, "cd.ValueId"},
                {CustomDataDTO.SearchByParameters.SITE_ID, "cd.site_id"},
                {CustomDataDTO.SearchByParameters.MASTER_ENTITY_ID, "cd.MasterEntityId"}
            };

        /// <summary>
        /// Dictionary for searching Parameters for the CustomDataView object.
        /// </summary>
        private static readonly Dictionary<CustomDataViewDTO.SearchByParameters, string> customDataViewDBSearchParameters = new Dictionary<CustomDataViewDTO.SearchByParameters, string>
            {
                {CustomDataViewDTO.SearchByParameters.CUSTOM_DATA_SET_ID, "cdv.CustomDataSetId"},
                {CustomDataViewDTO.SearchByParameters.APPLICABILITY, "cdv.Applicability"},
                {CustomDataViewDTO.SearchByParameters.NAME, "cdv.Name"},
                {CustomDataViewDTO.SearchByParameters.TYPE, "cdv.Type"},
                {CustomDataViewDTO.SearchByParameters.VALUECHAR, "cdv.ValueChar"}
            };
        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS CustomDataType;
                                            MERGE INTO CustomData tbl
                                            USING @CustomDataList AS src
                                            ON src.CustomDataId = tbl.CustomDataId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            CustomDataSetId = src.CustomDataSetId,
                                            CustomAttributeId = src.CustomAttributeId,
                                            ValueId = src.ValueId,
                                            CustomDataText = src.CustomDataText,
                                            CustomDataNumber = src.CustomDataNumber,
                                            CustomDataDate = src.CustomDataDate,
                                            MasterEntityId = src.MasterEntityId,
                                            -- site_id = src.site_id,
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            LastUpdateDate = GETDATE()
                                            
                                            WHEN NOT MATCHED THEN INSERT (
                                            CustomDataSetId,
                                            CustomAttributeId,
                                            ValueId,
                                            CustomDataText,
                                            CustomDataNumber,
                                            CustomDataDate,
                                            Guid,
                                            site_id,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdateDate,
                                            LastUpdatedBy
                                            )VALUES (
                                            src.CustomDataSetId,
                                            src.CustomAttributeId,
                                            src.ValueId,
                                            src.CustomDataText,
                                            src.CustomDataNumber,
                                            src.CustomDataDate,
                                            src.Guid,
                                            src.site_id,
                                            src.MasterEntityId,
                                            src.CreatedBy,
                                            GETDATE(),
                                            GETDATE(),
                                            src.LastUpdatedBy
                                            )
                                            OUTPUT
                                            inserted.CustomDataId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdateDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            CustomDataId,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdateDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion

        /// <summary>
        /// Default constructor of CustomDataDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CustomDataDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Inserts the CustomData record to the database
        /// </summary>
        /// <param name="customDataDTO">CustomDataDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(CustomDataDTO customDataDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customDataDTO, loginId, siteId);
            Save(new List<CustomDataDTO>() { customDataDTO }, loginId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the CustomData record to the database
        /// </summary>
        /// <param name="customDataDTOList">List of CustomDataDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<CustomDataDTO> customDataDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(customDataDTOList, loginId, siteId);
            Dictionary<string, CustomDataDTO> customDataDTOGuidMap = GetCustomDataDTOGuidMap(customDataDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(customDataDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "CustomDataType",
                                                                "@CustomDataList");
            UpdateCustomDataDTOList(customDataDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets Sql Data Records
        /// </summary>
        /// <param name="customDataDTOList">customDataDTOList</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>SqlDataRecords</returns>
        private List<SqlDataRecord> GetSqlDataRecords(List<CustomDataDTO> customDataDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(customDataDTOList, loginId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[15];
            columnStructures[0] = new SqlMetaData("CustomDataId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("CustomDataSetId", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("CustomAttributeId", SqlDbType.Int);
            columnStructures[3] = new SqlMetaData("ValueId", SqlDbType.Int);
            columnStructures[4] = new SqlMetaData("CustomDataText", SqlDbType.NVarChar, 100);
            columnStructures[5] = new SqlMetaData("CustomDataNumber", SqlDbType.Decimal, 18, 4);
            columnStructures[6] = new SqlMetaData("CustomDataDate", SqlDbType.DateTime);
            columnStructures[7] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[8] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[9] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[10] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[11] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[12] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[13] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);
            columnStructures[14] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);

            for (int i = 0; i < customDataDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(customDataDTOList[i].CustomDataId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(customDataDTOList[i].CustomDataSetId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(customDataDTOList[i].CustomAttributeId, true));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(customDataDTOList[i].ValueId, true));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(customDataDTOList[i].CustomDataText));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(customDataDTOList[i].CustomDataNumber));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(customDataDTOList[i].CustomDataDate));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(Guid.Parse(customDataDTOList[i].Guid)));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(customDataDTOList[i].SynchStatus));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(customDataDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(loginId));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(customDataDTOList[i].CreationDate));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(customDataDTOList[i].LastUpdateDate));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(loginId));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets CustomDataDTO GuidMap
        /// </summary>
        /// <param name="customDataDTOList"></param>
        /// <returns>CustomDataDTO</returns>
        private Dictionary<string, CustomDataDTO> GetCustomDataDTOGuidMap(List<CustomDataDTO> customDataDTOList)
        {
            log.LogMethodEntry(customDataDTOList);
            Dictionary<string, CustomDataDTO> result = new Dictionary<string, CustomDataDTO>();
            for (int i = 0; i < customDataDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(customDataDTOList[i].Guid))
                {
                    customDataDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(customDataDTOList[i].Guid, customDataDTOList[i]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Updates CustomDataDTOList
        /// </summary>
        /// <param name="customDataDTOGuidMap">customDataDTOGuidMap</param>
        /// <param name="table">table</param>
        private void UpdateCustomDataDTOList(Dictionary<string, CustomDataDTO> customDataDTOGuidMap, DataTable table)
        {
            log.LogMethodEntry(customDataDTOGuidMap, table);
            foreach (DataRow row in table.Rows)
            {
                CustomDataDTO customDataDTO = customDataDTOGuidMap[row["Guid"].ToString()];
                customDataDTO.CustomDataId = row["customDataID"] == DBNull.Value ? -1 : Convert.ToInt32(row["customDataID"]);
                customDataDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                customDataDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                customDataDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                customDataDTO.LastUpdateDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                customDataDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                customDataDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to CustomDataDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomDataDTO</returns>
        private CustomDataDTO GetCustomDataDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomDataDTO customDataDTO = new CustomDataDTO(Convert.ToInt32(dataRow["CustomDataId"]),
                                            dataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomDataSetId"]),
                                            dataRow["CustomAttributeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomAttributeId"]),
                                            dataRow["ValueId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ValueId"]),
                                            dataRow["CustomDataText"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CustomDataText"]),
                                            dataRow["CustomDataNumber"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CustomDataNumber"]),
                                            dataRow["CustomDataDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CustomDataDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(customDataDTO);
            return customDataDTO;
        }

        /// <summary>
        /// Converts the Data row object to CustomDataViewDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomDataViewDTO</returns>
        private CustomDataViewDTO GetCustomDataViewDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomDataViewDTO customDataViewDTO = new CustomDataViewDTO(dataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomDataSetId"]),
                                            dataRow["Applicability"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Applicability"]),
                                            dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                            dataRow["ValueChar"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ValueChar"]),
                                            dataRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Type"]),
                                            dataRow["CustomDataText"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CustomDataText"]),
                                            dataRow["CustomDataNumber"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CustomDataNumber"]),
                                            dataRow["CustomDataDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CustomDataDate"])
                                            );
            log.LogMethodExit(customDataViewDTO);
            return customDataViewDTO;
        }

        /// <summary>
        /// Gets the CustomData data of passed CustomData Id
        /// </summary>
        /// <param name="customDataId">integer type parameter</param>
        /// <returns>Returns CustomDataDTO</returns>
        public CustomDataDTO GetCustomDataDTO(int customDataId)
        {
            log.LogMethodEntry(customDataId);
            CustomDataDTO result = null;
            string query = SELECT_QUERY + @" WHERE cd.CustomDataId = @CustomDataId";
            DataTable table = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@CustomDataId", customDataId, true) }, sqlTransaction);
            if (table != null && table.Rows.Count > 0)
            {
                var list = table.Rows.Cast<DataRow>().Select(x => GetCustomDataDTO(x));
                if (list != null)
                {
                    result = list.FirstOrDefault();
                }
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the CustomDataDTO List for CustomDataSet Id List
        /// </summary>
        /// <param name="customDataSetIdList">integer list parameter</param>
        /// <returns>Returns List of CustomDataSetDTO</returns>
        public List<CustomDataDTO> GetCustomDataDTOListOfCustomDataSets(List<int> customDataSetIdList)
        {
            log.LogMethodEntry(customDataSetIdList);
            List<CustomDataDTO> list = new List<CustomDataDTO>();
            string query = @"SELECT CustomData.*
                            FROM CustomData, @CustomDataSetIdList List
                            WHERE CustomDataSetId = List.Id";
            DataTable table = dataAccessHandler.BatchSelect(query, "@CustomDataSetIdList", customDataSetIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetCustomDataDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the CustomDataDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomDataDTO matching the search criteria</returns>
        public List<CustomDataDTO> GetCustomDataDTOList(List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomDataDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomDataDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomDataDTO.SearchByParameters.VALUE_ID
                            || searchParameter.Key == CustomDataDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID
                            || searchParameter.Key == CustomDataDTO.SearchByParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == CustomDataDTO.SearchByParameters.CUSTOM_DATA_ID
                            || searchParameter.Key == CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomDataDTO.SearchByParameters.SITE_ID)
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
            DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetCustomDataDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the CustomDataViewDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomDataViewDTO matching the search criteria</returns>
        public List<CustomDataViewDTO> GetCustomDataViewDTOList(List<KeyValuePair<CustomDataViewDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomDataViewDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = "select * from CustomDataView as cdv ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomDataViewDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (customDataViewDBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomDataViewDTO.SearchByParameters.CUSTOM_DATA_SET_ID)
                        {
                            query.Append(joiner + customDataViewDBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == CustomDataViewDTO.SearchByParameters.APPLICABILITY
                            || searchParameter.Key == CustomDataViewDTO.SearchByParameters.TYPE
                            || searchParameter.Key == CustomDataViewDTO.SearchByParameters.NAME
                            )
                        {
                            query.Append(joiner + customDataViewDBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + customDataViewDBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
            DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetCustomDataViewDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
