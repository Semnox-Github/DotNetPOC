/********************************************************************************************
* Project Name - CustomDataSet Data Handler
* Description  - Data handler of the CustomDataSet class
* 
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
    ///  CustomDataSet Data Handler - Handles insert, update and select of  CustomDataSet objects
    /// </summary>
    public class CustomDataSetDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction = null;
        DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CustomDataSet as cd";

        /// <summary>
        /// Dictionary for searching Parameters for the CustomDataSet object.
        /// </summary>
        private static readonly Dictionary<CustomDataSetDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomDataSetDTO.SearchByParameters, string>
            {
                {CustomDataSetDTO.SearchByParameters.CUSTOM_DATA_SET_ID, "cd.CustomDataSetId"},
                {CustomDataSetDTO.SearchByParameters.CUSTOM_DATA_SET_ID_LIST, "cd.CustomDataSetId"},
                {CustomDataSetDTO.SearchByParameters.SITE_ID, "cd.site_id"},
                {CustomDataSetDTO.SearchByParameters.MASTER_ENTITY_ID, "cd.MasterEntityId"}
            };

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS CustomDataSetType;
                                            MERGE INTO CustomDataSet tbl
                                            USING @CustomDataSetList AS src
                                            ON src.CustomDataSetId = tbl.CustomDataSetId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            Dummy = src.Dummy,
                                            MasterEntityId = src.MasterEntityId,
                                            --site_id = src.site_id,
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            LastUpdateDate = GETDATE()
                                            WHEN NOT MATCHED THEN INSERT (
                                            Dummy,
                                            Guid,
                                            site_id,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdateDate,
                                            LastUpdatedBy
                                            )VALUES (
                                            src.Dummy,
                                            src.Guid,
                                            src.site_id,
                                            src.MasterEntityId,
                                            src.CreatedBy,
                                            GETDATE(),
                                            GETDATE(),
                                            src.LastUpdatedBy
                                            )
                                            OUTPUT
                                            inserted.CustomDataSetId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdateDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            CustomDataSetId,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdateDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion
        
        /// <summary>
        /// Default constructor of CustomDataSetDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CustomDataSetDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Inserts the CustomDataSet record to the database
        /// </summary>
        /// <param name="customDataSetDTO">CustomDataSetDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(CustomDataSetDTO customDataSetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customDataSetDTO, loginId, siteId);
            Save(new List<CustomDataSetDTO>() { customDataSetDTO }, loginId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the CustomDataSet record to the database
        /// </summary>
        /// <param name="customDataSetDTOList">List of CustomDataSetDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<CustomDataSetDTO> customDataSetDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(customDataSetDTOList, loginId, siteId);
            Dictionary<string, CustomDataSetDTO> customDataSetDTOGuidMap = GetCustomDataSetDTOGuidMap(customDataSetDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(customDataSetDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                            sqlTransaction,
                                                            MERGE_QUERY,
                                                            "CustomDataSetType",
                                                            "@CustomDataSetList");
            UpdateCustomDataSetDTOList(customDataSetDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets Sql Data Records
        /// </summary>
        /// <param name="customDataSetDTOList">customDataSetDTOList</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Sql Data Records</returns>
        private List<SqlDataRecord> GetSqlDataRecords(List<CustomDataSetDTO> customDataSetDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(customDataSetDTOList, loginId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[10];
            columnStructures[0] = new SqlMetaData("CustomDataSetId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("Dummy", SqlDbType.NVarChar, 50);
            columnStructures[2] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[3] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[4] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[5] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[7] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[8] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);
            columnStructures[9] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);

            for (int i = 0; i < customDataSetDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(customDataSetDTOList[i].CustomDataSetId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(customDataSetDTOList[i].Dummy));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(Guid.Parse(customDataSetDTOList[i].Guid)));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(customDataSetDTOList[i].SynchStatus));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(customDataSetDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(loginId));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(customDataSetDTOList[i].CreationDate));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(customDataSetDTOList[i].LastUpdateDate));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(loginId));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets CustomDataSetDTO GuidMap
        /// </summary>
        /// <param name="customDataSetDTOList">customDataSetDTOList</param>
        /// <returns>CustomDataSetDTO GuidMap</returns>
        private Dictionary<string, CustomDataSetDTO> GetCustomDataSetDTOGuidMap(List<CustomDataSetDTO> customDataSetDTOList)
        {
            Dictionary<string, CustomDataSetDTO> result = new Dictionary<string, CustomDataSetDTO>();
            for (int i = 0; i < customDataSetDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(customDataSetDTOList[i].Guid))
                {
                    customDataSetDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(customDataSetDTOList[i].Guid, customDataSetDTOList[i]);
            }
            return result;
        }

        /// <summary>
        /// Update CustomDataSetDTO List
        /// </summary>
        /// <param name="customDataSetDTOGuidMap">customDataSetDTOGuidMap</param>
        /// <param name="table">table</param>
        private void UpdateCustomDataSetDTOList(Dictionary<string, CustomDataSetDTO> customDataSetDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                CustomDataSetDTO customDataSetDTO = customDataSetDTOGuidMap[Convert.ToString(row["Guid"])];
                customDataSetDTO.CustomDataSetId = row["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(row["CustomDataSetId"]);
                customDataSetDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                customDataSetDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                customDataSetDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                customDataSetDTO.LastUpdateDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                customDataSetDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                customDataSetDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Converts the Data row object to CustomDataSetDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomDataSetDTO</returns>
        private CustomDataSetDTO GetCustomDataSetDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomDataSetDTO customDataSetDTO = new CustomDataSetDTO(Convert.ToInt32(dataRow["CustomDataSetId"]),
                                            dataRow["Dummy"] == DBNull.Value ? "" : Convert.ToString(dataRow["Dummy"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(customDataSetDTO);
            return customDataSetDTO;
        }

        /// <summary>
        /// Gets the CustomDataSet data of passed CustomDataSet Id
        /// </summary>
        /// <param name="customDataSetId">integer type parameter</param>
        /// <returns>Returns CustomDataSetDTO</returns>
        public CustomDataSetDTO GetCustomDataSetDTO(int customDataSetId)
        {
            log.LogMethodEntry(customDataSetId);
            CustomDataSetDTO result = null;
            string query = SELECT_QUERY + @" WHERE cd.CustomDataSetId = @CustomDataSetId";
            DataTable table = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@CustomDataSetId", customDataSetId, true) }, sqlTransaction);
            if (table != null && table.Rows.Count > 0)
            {
                var list = table.Rows.Cast<DataRow>().Select(x => GetCustomDataSetDTO(x));
                if (list != null)
                {
                    result = list.FirstOrDefault();
                }
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Gets the CustomDataSetDTO List for CustomDataSet Id List
        /// </summary>
        /// <param name="customDataSetIdList">integer list parameter</param>
        /// <returns>Returns List of CustomDataSetDTO</returns>
        public List<CustomDataSetDTO> GetCustomDataSetDTOList(List<int> customDataSetIdList)
        {
            log.LogMethodEntry(customDataSetIdList);
            List<CustomDataSetDTO> list = null;
            string query = @"SELECT CustomDataSet.*
                            FROM CustomDataSet, @CustomDataSetIdList List
                            WHERE CustomDataSetId = List.Id";
            DataTable table = dataAccessHandler.BatchSelect(query, "@CustomDataSetIdList", customDataSetIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetCustomDataSetDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the CustomDataSetDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomDataSetDTO matching the search criteria</returns>
        public List<CustomDataSetDTO> GetCustomDataSetDTOList(List<KeyValuePair<CustomDataSetDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomDataSetDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CustomDataSetDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CustomDataSetDTO.SearchByParameters.CUSTOM_DATA_SET_ID ||
                            searchParameter.Key == CustomDataSetDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomDataSetDTO.SearchByParameters.CUSTOM_DATA_SET_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));

                        }
                        else if (searchParameter.Key == CustomDataSetDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetCustomDataSetDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
