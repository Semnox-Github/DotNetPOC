/********************************************************************************************
 * Project Name - DataAccessDetail Data Handler
 * Description  - Data handler of the DataAccessDetail class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        15-Jul-2019      Girish Kundar       Modified : Added GetSQLParameter(),SQL Injection Fix.
 *2.70.2        11-Dec-2019      Jinto Thomas        Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Data Access Detail Data Handler  - Handles insert, update and select of Data Access Detail objects
    /// </summary>
    public class DataAccessDetailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string> DBSearchParameters = new Dictionary<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>
            {
                {DataAccessDetailDTO.SearchByDataAccessDetailParameters.DATA_ACCESS_RULE_ID, "da.DataAccessRuleId"},
                {DataAccessDetailDTO.SearchByDataAccessDetailParameters.ACCESS_LEVEL_ID, "da.AccessLevelId"},
                {DataAccessDetailDTO.SearchByDataAccessDetailParameters.ACCESS_LIMIT_ID, "da.AccessLimitId"},
                {DataAccessDetailDTO.SearchByDataAccessDetailParameters.RULE_DETAIL_ID, "da.RuleDetailId"},
                {DataAccessDetailDTO.SearchByDataAccessDetailParameters.ENTITY_ID, "da.EntityId"},
                {DataAccessDetailDTO.SearchByDataAccessDetailParameters.ACTIVE_FLAG, "da.IsActive"},
                {DataAccessDetailDTO.SearchByDataAccessDetailParameters.MASTER_ENTITY_ID,"da.MasterEntityId"},
                {DataAccessDetailDTO.SearchByDataAccessDetailParameters.SITE_ID, "da.site_id"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM DataAccessDetail AS da ";
        private readonly SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of DataAccessDetailDataHandler class
        /// </summary>
        public DataAccessDetailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DataAccessDetail Record.
        /// </summary>
        /// <param name="dataAccessDetailDTO">DataAccessDetailDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(DataAccessDetailDTO dataAccessDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dataAccessDetailDTO, loginId, siteId);
        
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@ruleDetailId", dataAccessDetailDTO.RuleDetailId, true);
            ParametersHelper.ParameterHelper(parameters, "@dataAccessRuleId", dataAccessDetailDTO.DataAccessRuleId, true);
            ParametersHelper.ParameterHelper(parameters, "@entityId", dataAccessDetailDTO.EntityId, true);
            ParametersHelper.ParameterHelper(parameters, "@accessLimitId", dataAccessDetailDTO.AccessLimitId, true);
            ParametersHelper.ParameterHelper(parameters, "@accessLevelId", dataAccessDetailDTO.AccessLevelId, true);
            ParametersHelper.ParameterHelper(parameters, "@isActive", dataAccessDetailDTO.IsActive);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@siteId", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", dataAccessDetailDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }



        /// <summary>
        /// Inserts the Data Access Detail record to the database
        /// </summary>
        /// <param name="dataAccessDetailDTO">DataAccessDetailDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>Returns inserted record id</returns>
        public DataAccessDetailDTO InsertDataAccessDetail(DataAccessDetailDTO dataAccessDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dataAccessDetailDTO, loginId, siteId);
            string insertDataAccessDetailQuery = @"insert into DataAccessDetail 
                                                        ( 
                                                         DataAccessRuleId
                                                        ,EntityId
                                                        ,AccessLevelId
                                                        ,AccessLimitId
                                                        ,CreatedBy
                                                        ,CreationDate
                                                        ,LastUpdatedBy
                                                        ,LastupdatedDate
                                                        ,Guid
                                                        ,site_id
                                                        ,MasterEntityId
                                                        ,IsActive
                                                        ) 
                                                values 
                                                        (                                                         
                                                         @dataAccessRuleId
                                                        ,@entityId
                                                        ,@accessLevelId
                                                        ,@accessLimitId
                                                        ,@createdBy
                                                        ,getdate()
                                                        ,@lastUpdatedBy
                                                        ,getdate()
                                                        ,NewId()
                                                        ,@siteId
                                                        ,@masterEntityId
                                                        ,@isActive)
                                                  SELECT  * from DataAccessDetail where RuleDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertDataAccessDetailQuery, BuildSQLParameters(dataAccessDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDataAccessDetailDTO(dataAccessDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting dataAccessDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dataAccessDetailDTO);
            return dataAccessDetailDTO;
        }

        /// <summary>
        /// Updates the Data Access Detail record
        /// </summary>
        /// <param name="dataAccessDetailDTO">DataAccessDetailDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>Returns DataAccessDetailDTO</returns>
        public DataAccessDetailDTO UpdateDataAccessDetail(DataAccessDetailDTO dataAccessDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dataAccessDetailDTO, loginId, siteId);
            string updateDataAccessDetailQuery = @"update DataAccessDetail 
                                         set DataAccessRuleId = @dataAccessRuleId,
                                             EntityId = @entityId,
                                             AccessLevelId = @accessLevelId,
                                             AccessLimitId = @accessLimitId,
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             -- site_id=@siteid,
                                             MasterEntityId=@masterEntityId                                                                                       
                                       where RuleDetailId = @ruleDetailId
                                       SELECT* from DataAccessDetail where RuleDetailId = @ruleDetailId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateDataAccessDetailQuery, BuildSQLParameters(dataAccessDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDataAccessDetailDTO(dataAccessDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating dataAccessDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dataAccessDetailDTO);
            return dataAccessDetailDTO;
        }



        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="dataAccessDetailDTO">DataAccessDetailDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshDataAccessDetailDTO(DataAccessDetailDTO dataAccessDetailDTO, DataTable dt)
        {
            log.LogMethodEntry(dataAccessDetailDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                dataAccessDetailDTO.RuleDetailId = Convert.ToInt32(dt.Rows[0]["RuleDetailId"]);
                dataAccessDetailDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                dataAccessDetailDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                dataAccessDetailDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                dataAccessDetailDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                dataAccessDetailDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                dataAccessDetailDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to dataAccessDetailDTO class type
        /// </summary>
        /// <param name="dataAccessDetailDataRow">dataAccessDetail DataRow</param>
        /// <returns>Returns DataAccessDetailDTO</returns>
        private DataAccessDetailDTO GetDataAccessDetailDTO(DataRow dataAccessDetailDataRow)
        {
            log.LogMethodEntry(dataAccessDetailDataRow);
            DataAccessDetailDTO dataAccessDetailDataObject = new DataAccessDetailDTO(Convert.ToInt32(dataAccessDetailDataRow["RuleDetailId"]),
                                            dataAccessDetailDataRow["DataAccessRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataAccessDetailDataRow["DataAccessRuleId"]),
                                            dataAccessDetailDataRow["EntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataAccessDetailDataRow["EntityId"]),
                                            dataAccessDetailDataRow["AccessLevelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataAccessDetailDataRow["AccessLevelId"]),
                                            dataAccessDetailDataRow["AccessLimitId"] == DBNull.Value ? -1 : Convert.ToInt32(dataAccessDetailDataRow["AccessLimitId"]),
                                            dataAccessDetailDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataAccessDetailDataRow["IsActive"]),
                                            dataAccessDetailDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataAccessDetailDataRow["CreatedBy"]),
                                            dataAccessDetailDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataAccessDetailDataRow["CreationDate"]),
                                            dataAccessDetailDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataAccessDetailDataRow["LastUpdatedBy"]),
                                            dataAccessDetailDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataAccessDetailDataRow["LastupdatedDate"]),
                                            dataAccessDetailDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataAccessDetailDataRow["Guid"]),
                                            dataAccessDetailDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataAccessDetailDataRow["site_id"]),
                                            dataAccessDetailDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataAccessDetailDataRow["SynchStatus"]),
                                            dataAccessDetailDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataAccessDetailDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(dataAccessDetailDataObject);
            return dataAccessDetailDataObject;
        }

        /// <summary>
        /// Gets the Data Access Detail data of passed ruleDetailId
        /// </summary>
        /// <param name="dataAccessRuleId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns list DataAccessDetailDTO</returns>
        public List<DataAccessDetailDTO> GetDataAccessDetailList(int dataAccessRuleId)
        {
            log.LogMethodEntry(dataAccessRuleId);
            List<KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>> dataAccessDetailSearchParams = new List<KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>>();
            //dataAccessDetailSearchParams.Add(new KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>(DataAccessDetailDTO.SearchByDataAccessDetailParameters.ACTIVE_FLAG, "1"));
            dataAccessDetailSearchParams.Add(new KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>(DataAccessDetailDTO.SearchByDataAccessDetailParameters.DATA_ACCESS_RULE_ID, dataAccessRuleId.ToString()));
            List<DataAccessDetailDTO> dataAccessDetailDTOList = GetDataAccessDetailList(dataAccessDetailSearchParams);
            log.LogMethodExit(dataAccessDetailDTOList);
            return dataAccessDetailDTOList;
        }

        /// <summary>
        /// Gets the Data Access Detail data of passed dataAccessDetailId
        /// </summary>
        /// <param name="dataAccessDetailId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns DataAccessDetailDTO</returns>
        public DataAccessDetailDTO GetDataAccessDetail(int dataAccessDetailId)
        {
            log.LogMethodEntry(dataAccessDetailId);
            string selectDataAccessDetailQuery = SELECT_QUERY + "  WHERE da.RuleDetailId = @dataAccessDetailId";
            DataAccessDetailDTO dataAccessDetailDataObject = null;
            SqlParameter[] selectDataAccessDetailParameters = new SqlParameter[1];
            selectDataAccessDetailParameters[0] = new SqlParameter("@dataAccessDetailId", dataAccessDetailId);
            DataTable dataAccessDetail = dataAccessHandler.executeSelectQuery(selectDataAccessDetailQuery, selectDataAccessDetailParameters, sqlTransaction);
            if (dataAccessDetail.Rows.Count > 0)
            {
                DataRow dataAccessDetailRow = dataAccessDetail.Rows[0];
                dataAccessDetailDataObject = GetDataAccessDetailDTO(dataAccessDetailRow);
                EntityExclusionDetailDataHandler entityExclusionDetailDataHandler = new EntityExclusionDetailDataHandler(sqlTransaction);
                dataAccessDetailDataObject.EntityExclusionDetailDTOList = entityExclusionDetailDataHandler.GetEntityExclusionDetailList(dataAccessDetailDataObject.RuleDetailId);
                log.LogMethodExit(dataAccessDetailDataObject, "returning dataAccessDetailDataObject.");
            }
            log.LogMethodExit(dataAccessDetailDataObject);
            return dataAccessDetailDataObject;

        }

        /// <summary>
        /// Gets the DataAccessDetailDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of DataAccessDetailDTO matching the search criteria</returns>
        public List<DataAccessDetailDTO> GetDataAccessDetailList(List<KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectDataAccessDetailQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<DataAccessDetailDTO> dataAccessDetailList = null;
            if (searchParameters != null)
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == DataAccessDetailDTO.SearchByDataAccessDetailParameters.DATA_ACCESS_RULE_ID
                            || searchParameter.Key == DataAccessDetailDTO.SearchByDataAccessDetailParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == DataAccessDetailDTO.SearchByDataAccessDetailParameters.ACCESS_LEVEL_ID
                            || searchParameter.Key == DataAccessDetailDTO.SearchByDataAccessDetailParameters.ACCESS_LIMIT_ID
                            || searchParameter.Key == DataAccessDetailDTO.SearchByDataAccessDetailParameters.ENTITY_ID
                            || searchParameter.Key == DataAccessDetailDTO.SearchByDataAccessDetailParameters.RULE_DETAIL_ID
                            || searchParameter.Key == DataAccessDetailDTO.SearchByDataAccessDetailParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DataAccessDetailDTO.SearchByDataAccessDetailParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DataAccessDetailDTO.SearchByDataAccessDetailParameters.ACTIVE_FLAG) //char
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'~') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                if (searchParameters.Count > 0)
                    selectDataAccessDetailQuery = selectDataAccessDetailQuery + query;
            }
            DataTable dataAccessDetailData = dataAccessHandler.executeSelectQuery(selectDataAccessDetailQuery, parameters.ToArray(), sqlTransaction);
            if (dataAccessDetailData.Rows.Count > 0)
            {
                dataAccessDetailList = new List<DataAccessDetailDTO>();
                foreach (DataRow dataAccessDetailDataRow in dataAccessDetailData.Rows)
                {
                    DataAccessDetailDTO dataAccessDetailDataObject = GetDataAccessDetailDTO(dataAccessDetailDataRow);
                    dataAccessDetailList.Add(dataAccessDetailDataObject);
                }
            }
            log.LogMethodExit(dataAccessDetailList, "returning dataAccessDetailList.");
            return dataAccessDetailList;

        }
    }
}
