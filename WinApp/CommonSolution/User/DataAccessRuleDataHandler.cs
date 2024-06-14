/********************************************************************************************
 * Project Name - DataAccessRuleDataHandler
 * Description  - Data handler of the DataAccessRule class
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
    /// Data Access Rule Data Handler  - Handles insert, update and select of Data Access Rule objects
    /// </summary>
    public class DataAccessRuleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string> DBSearchParameters = new Dictionary<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>
            {
                {DataAccessRuleDTO.SearchByDataAccessRuleParameters.DATA_ACCESS_RULE_ID, "dr.DataAccessRuleId"},
                {DataAccessRuleDTO.SearchByDataAccessRuleParameters.NAME, "dr.Name"},
                {DataAccessRuleDTO.SearchByDataAccessRuleParameters.ACTIVE_FLAG, "dr.IsActive"},
                {DataAccessRuleDTO.SearchByDataAccessRuleParameters.MASTER_ENTITY_ID,"dr.MasterEntityId"},
                {DataAccessRuleDTO.SearchByDataAccessRuleParameters.SITE_ID, "dr.site_id"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM DataAccessRule AS dr ";
        private readonly SqlTransaction sqlTransaction;
        List<SqlParameter> parameters = new List<SqlParameter>();


        /// <summary>
        /// Default constructor of DataAccessRuleDataHandler class
        /// </summary>
        public DataAccessRuleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DataAccessRule Record.
        /// </summary>
        /// <param name="dataAccessRuleDTO">DataAccessRuleDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(DataAccessRuleDTO dataAccessRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dataAccessRuleDTO, loginId, siteId);
         
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@dataAccessRuleId", dataAccessRuleDTO.DataAccessRuleId, true);
            ParametersHelper.ParameterHelper(parameters, "@name", string.IsNullOrEmpty(dataAccessRuleDTO.Name) ? DBNull.Value : (object)dataAccessRuleDTO.Name);
            ParametersHelper.ParameterHelper(parameters, "@isActive", dataAccessRuleDTO.IsActive);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@siteId", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", dataAccessRuleDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }




        /// <summary>
        /// Inserts the Data Access Rule record to the database
        /// </summary>
        /// <param name="dataAccessRuleDTO">DataAccessRuleDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>Returns DataAccessRuleDTO</returns>
        public DataAccessRuleDTO InsertDataAccessRule(DataAccessRuleDTO dataAccessRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dataAccessRuleDTO, loginId, siteId);
            string insertDataAccessRuleQuery = @"insert into DataAccessRule 
                                                        ( 
                                                         Name                                                        
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
                                                         @name                                                        
                                                        ,@createdBy
                                                        ,getdate()
                                                        ,@lastUpdatedBy
                                                        ,getdate()
                                                        ,NewId()
                                                        ,@siteId
                                                        ,@masterEntityId
                                                        ,@isActive) SELECT  * from DataAccessRule where DataAccessRuleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertDataAccessRuleQuery, BuildSQLParameters(dataAccessRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDataAccessRuleDTO(dataAccessRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting dataAccessRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dataAccessRuleDTO);
            return dataAccessRuleDTO;
        }

        /// <summary>
        /// Updates the Data Access Rule record
        /// </summary>
        /// <param name="dataAccessRuleDTO">DataAccessRuleDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>Returns DataAccessRuleDTO</returns>
        public DataAccessRuleDTO UpdateDataAccessRule(DataAccessRuleDTO dataAccessRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dataAccessRuleDTO, loginId, siteId);
            string updateDataAccessRuleQuery = @"update DataAccessRule 
                                         set Name = @name,                                             
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             -- site_id=@siteid,
                                             MasterEntityId=@masterEntityId                                                                                       
                                       where DataAccessRuleId = @dataAccessRuleId
                               SELECT  * from DataAccessRule where DataAccessRuleId = @dataAccessRuleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateDataAccessRuleQuery, BuildSQLParameters(dataAccessRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDataAccessRuleDTO(dataAccessRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating dataAccessRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dataAccessRuleDTO);
            return dataAccessRuleDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="dataAccessRuleDTO">DataAccessRuleDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshDataAccessRuleDTO(DataAccessRuleDTO dataAccessRuleDTO, DataTable dt)
        {
            log.LogMethodEntry(dataAccessRuleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                dataAccessRuleDTO.DataAccessRuleId = Convert.ToInt32(dt.Rows[0]["DataAccessRuleId"]);
                dataAccessRuleDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                dataAccessRuleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                dataAccessRuleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                dataAccessRuleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                dataAccessRuleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                dataAccessRuleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to dataAccessRuleDTO class type
        /// </summary>
        /// <param name="dataAccessRuleDataRow">dataAccessRule DataRow</param>
        /// <returns>Returns DataAccessRuleDTO</returns>
        private DataAccessRuleDTO GetDataAccessRuleDTO(DataRow dataAccessRuleDataRow)
        {
            log.LogMethodEntry(dataAccessRuleDataRow);
            DataAccessRuleDTO dataAccessRuleDataObject = new DataAccessRuleDTO(Convert.ToInt32(dataAccessRuleDataRow["DataAccessRuleId"]),
                                            dataAccessRuleDataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataAccessRuleDataRow["Name"]),
                                            dataAccessRuleDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataAccessRuleDataRow["IsActive"]),
                                            dataAccessRuleDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataAccessRuleDataRow["CreatedBy"]),
                                            dataAccessRuleDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataAccessRuleDataRow["CreationDate"]),
                                            dataAccessRuleDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataAccessRuleDataRow["LastUpdatedBy"]),
                                            dataAccessRuleDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataAccessRuleDataRow["LastupdatedDate"]),
                                            dataAccessRuleDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataAccessRuleDataRow["Guid"]),
                                            dataAccessRuleDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataAccessRuleDataRow["site_id"]),
                                            dataAccessRuleDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataAccessRuleDataRow["SynchStatus"]),
                                            dataAccessRuleDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataAccessRuleDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(dataAccessRuleDataObject);
            return dataAccessRuleDataObject;
        }

        /// <summary>
        /// Gets the Data Access Rule data of passed dataAccessRuleId
        /// </summary>
        /// <param name="dataAccessRuleId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns DataAccessRuleDTO</returns>
        public DataAccessRuleDTO GetDataAccessRule(int dataAccessRuleId)
        {
            log.LogMethodEntry(dataAccessRuleId);
            DataAccessRuleDTO dataAccessRuleDataObject = null;
            string selectDataAccessRuleQuery = SELECT_QUERY + "  WHERE dr.DataAccessRuleId = @dataAccessRuleId";
            SqlParameter[] selectDataAccessRuleParameters = new SqlParameter[1];
            selectDataAccessRuleParameters[0] = new SqlParameter("@dataAccessRuleId", dataAccessRuleId);
            DataTable dataAccessRule = dataAccessHandler.executeSelectQuery(selectDataAccessRuleQuery, selectDataAccessRuleParameters, sqlTransaction);
            if (dataAccessRule.Rows.Count > 0)
            {
                DataRow dataAccessRuleRow = dataAccessRule.Rows[0];
                dataAccessRuleDataObject = GetDataAccessRuleDTO(dataAccessRuleRow);
                DataAccessDetailDataHandler dataAccessDetailDataHandler = new DataAccessDetailDataHandler(sqlTransaction);
                dataAccessRuleDataObject.DataAccessDetailDTOList = dataAccessDetailDataHandler.GetDataAccessDetailList(dataAccessRuleDataObject.DataAccessRuleId);
            }

            log.LogMethodExit(dataAccessRuleDataObject, "returning dataAccessRuleDataObject.");
            return dataAccessRuleDataObject;
        }

        /// <summary>
        /// Gets the DataAccessRuleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DataAccessRuleDTO matching the search criteria</returns>
        public List<DataAccessRuleDTO> GetDataAccessRuleLists(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters);
            List<DataAccessRuleDTO> dataAccessRuleDTOList = new List<DataAccessRuleDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage > -1 && pageSize > 0)
            {
                selectQuery += " ORDER BY dr.DataAccessRuleId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                dataAccessRuleDTOList = new List<DataAccessRuleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataAccessRuleDTO dataAccessRuleDTO = GetDataAccessRuleDTO(dataRow);
                    dataAccessRuleDTOList.Add(dataAccessRuleDTO);
                }
            }
            log.LogMethodExit(dataAccessRuleDTOList);
            return dataAccessRuleDTOList;
        }

        /// <summary>
        /// Returns the no of DataAccessRule matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetDataAccessRuleCount(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            int dataAccessRuleDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                dataAccessRuleDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(dataAccessRuleDTOCount);
            return dataAccessRuleDTOCount;
        }

        /// <summary>
        /// Gets the DataAccessRuleDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic DataAccessRuleDTO matching the search criteria</returns>
        public List<DataAccessRuleDTO> GetDataAccessRuleList(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<DataAccessRuleDTO> dataAccessRuleDTOList = new List<DataAccessRuleDTO>();
            parameters.Clear();
            string selectDataAccessRuleDTOQuery = SELECT_QUERY;
            selectDataAccessRuleDTOQuery = selectDataAccessRuleDTOQuery + GetFilterQuery(searchParameters, sqlTransaction);
            DataTable dtDataAccessRuleDTO = dataAccessHandler.executeSelectQuery(selectDataAccessRuleDTOQuery, parameters.ToArray(), sqlTransaction);
            if (dtDataAccessRuleDTO.Rows.Count > 0)
            {
                foreach (DataRow dataAccessRuleDTORow in dtDataAccessRuleDTO.Rows)
                {
                    DataAccessRuleDTO dataAccessRuleDTO = GetDataAccessRuleDTO(dataAccessRuleDTORow);
                    dataAccessRuleDTOList.Add(dataAccessRuleDTO);
                }

            }
            log.LogMethodExit(dataAccessRuleDTOList);
            return dataAccessRuleDTOList;
        }


        /// <summary>
        /// Gets the DataAccessRuleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of DataAccessRuleDTO matching the search criteria</returns>
        public string GetFilterQuery(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder("");
            if (searchParameters != null)
            {
                string joiner = string.Empty;
                query = new StringBuilder(" where ");
                foreach (KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == DataAccessRuleDTO.SearchByDataAccessRuleParameters.DATA_ACCESS_RULE_ID
                            || searchParameter.Key == DataAccessRuleDTO.SearchByDataAccessRuleParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DataAccessRuleDTO.SearchByDataAccessRuleParameters.NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == DataAccessRuleDTO.SearchByDataAccessRuleParameters.ACTIVE_FLAG) // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == DataAccessRuleDTO.SearchByDataAccessRuleParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
            }
            log.LogMethodExit();
            return query.ToString();
        }
    }
}
