/********************************************************************************************
 * Project Name - SystemOptionsDataHandler
 * Description  - Data handler which inserts,updates  and selects the data from system options table
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera        Created 
 *2.70        4-Jul- 2019      Girish Kundar     Modified : Changed the structure of Data Handler,  Added Active Flag as Search Parameter 
 *                                                          SQL Injection Issue Fix.
 *2.70.2        11-Dec-2019      Jinto Thomas      Removed siteid from update query                                                          
 *2.100.0     31-Aug-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Core.Utilities
{
    internal class SystemOptionsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private static readonly Dictionary<SystemOptionsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SystemOptionsDTO.SearchByParameters, string>
            {
                {SystemOptionsDTO.SearchByParameters.OPTION_ID,"OptionId"},
                {SystemOptionsDTO.SearchByParameters.OPTION_TYPE,"OptionType"},
                {SystemOptionsDTO.SearchByParameters.OPTION_NAME,"OptionName"},
                {SystemOptionsDTO.SearchByParameters.OPTION_VALUE,"OptionValue"},
                {SystemOptionsDTO.SearchByParameters.SITE_ID,"site_id"},
                {SystemOptionsDTO.SearchByParameters.IS_ACTIVE,"IsActive"},
                {SystemOptionsDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"}
             };

        /// <summary>
        /// Default constructor of SystemOptionsDataHandler class
        /// </summary>
        public SystemOptionsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SqlParameter for SystemOptionsDTO
        /// </summary>
        /// <param name="systemOptionsDTO">SystemOptionsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>SqlParameter</returns>
        private List<SqlParameter> GetSQLParameters(SystemOptionsDTO systemOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(systemOptionsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@optionId", systemOptionsDTO.OptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@optionType ", string.IsNullOrEmpty(systemOptionsDTO.OptionType) ? DBNull.Value : (object)systemOptionsDTO.OptionType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@optionName", string.IsNullOrEmpty(systemOptionsDTO.OptionName) ? DBNull.Value : (object)systemOptionsDTO.OptionName));
           // parameters.Add(dataAccessHandler.GetSQLParameter("@OptionValue", string.IsNullOrEmpty(systemOptionsDTO.OptionValue) ? SqlDbType.VarBinary,: (object) (systemOptionsDTO.OptionValue)));
            SqlParameter optionparameter = new SqlParameter("@OptionValue", SqlDbType.VarBinary);
            if (string.IsNullOrEmpty(systemOptionsDTO.OptionValue))
            {
                optionparameter.Value = DBNull.Value;
            }
            else
            {   byte[] optionvalue  = Encoding.UTF8.GetBytes(systemOptionsDTO.OptionValue);
                optionparameter.Value = optionvalue;
            }
            parameters.Add(optionparameter);
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", systemOptionsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", systemOptionsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;

        }

        /// <summary>
        /// Inserts the SystemOptions record to the database
        /// </summary>
        /// <param name="SystemOptionsDTO">SystemOptionsDTO type object</param>
        /// <param name="loginId"></param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns SystemOptionsDTO</returns>
        public SystemOptionsDTO InsertSystemOptionsDTO(SystemOptionsDTO systemOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(systemOptionsDTO, loginId, siteId);
            string insertSystemOptionsDTOQuery = @"insert into SystemOptions
                                                            ( 
                                                            OptionType,
                                                            OptionName,
                                                            OptionValue,
                                                            site_id,
                                                            Guid,
                                                            MasterEntityId,
                                                            IsActive,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdateDate
                                                         )
                                                       values
                                                         ( 
                                                            @optionType,
                                                            @optionName,
                                                            @optionValue,
                                                            @siteId,
                                                            NewId(), 
                                                            @masterEntityId,
                                                            @isActive,
                                                            @createdBy,
                                                            GETDATE(),
                                                            @lastUpdatedBy,
                                                            GETDATE()
                                                          )SELECT * FROM SystemOptions WHERE OptionId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertSystemOptionsDTOQuery, GetSQLParameters(systemOptionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshSystemOptionsDTO(systemOptionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting systemOptionsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(systemOptionsDTO);
            return systemOptionsDTO;

        }


        /// <summary>
        /// Updates the SystemOptions record to the database
        /// </summary>
        /// <param name="SystemOptionsDTO">SystemOptionsDTO type object</param>
        /// <param name="loginId">updated loginId number</param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns # of rows updated</returns>
        public SystemOptionsDTO UpdateSystemOptionsDTO(SystemOptionsDTO systemOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(systemOptionsDTO, loginId, siteId);
            string updateSystemOptionsDTOQuery = @"update SystemOptions
                                                         set
                                                            OptionType = @optionType,
                                                            OptionName = @optionName,
                                                            OptionValue = @optionValue,
                                                            -- site_id = @siteId, 
                                                            IsActive = @isActive,
                                                            MasterEntityId = @masterEntityId, 
                                                            LastUpdatedBy = @lastUpdatedBy,
                                                            LastUpdateDate = GETDATE()
                                                        where OptionId = @optionId
                                                        SELECT * FROM SystemOptions WHERE OptionId = @optionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateSystemOptionsDTOQuery, GetSQLParameters(systemOptionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshSystemOptionsDTO(systemOptionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating systemOptionsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(systemOptionsDTO);
            return systemOptionsDTO;
        }

        internal DateTime? GetSystemOptionModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(LastUpdateDate) LastUpdatedDate from SystemOptions WHERE (site_id = @siteId or @siteId = -1)";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// updates the SystemOptionsDTO with Id ,who columns values for further process.
        /// </summary>
        /// <param name="systemOptionsDTO">SystemOptionsDTO</param>
        /// <param name="dt">dt object of DataTable</param>
        private void RefreshSystemOptionsDTO(SystemOptionsDTO systemOptionsDTO, DataTable dt)
        {
            log.LogMethodEntry(systemOptionsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
               systemOptionsDTO.OptionId = Convert.ToInt32(dt.Rows[0]["OptionId"]);
               systemOptionsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
               systemOptionsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
               systemOptionsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
               systemOptionsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
               systemOptionsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
               systemOptionsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to SystemOptionsDTO class type
        /// </summary>
        /// <param name="systemOptionsDTODataRow">SystemOptionsDTO DataRow</param>
        /// <returns>Returns SystemOptionsDTO</returns>
        private SystemOptionsDTO GetSystemOptionsDTO(DataRow systemOptionsDTODataRow)
        {
            log.LogMethodEntry(systemOptionsDTODataRow);
            SystemOptionsDTO systemOptionsDTO = new SystemOptionsDTO(
                                 Convert.ToInt32(systemOptionsDTODataRow["OptionId"]),
                                 systemOptionsDTODataRow["OptionType"] == DBNull.Value ? string.Empty : systemOptionsDTODataRow["OptionType"].ToString(),
                                 systemOptionsDTODataRow["OptionName"] == DBNull.Value ? string.Empty : systemOptionsDTODataRow["OptionName"].ToString(),
                                 systemOptionsDTODataRow["OptValInStringForm"].ToString(),
                                 systemOptionsDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(systemOptionsDTODataRow["site_id"]),
                                 systemOptionsDTODataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(systemOptionsDTODataRow["Guid"]),
                                 systemOptionsDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(systemOptionsDTODataRow["SynchStatus"]),
                                 systemOptionsDTODataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(systemOptionsDTODataRow["IsActive"]),
                                 systemOptionsDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(systemOptionsDTODataRow["MasterEntityId"]),
                                 systemOptionsDTODataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(systemOptionsDTODataRow["site_id"]),
                                 systemOptionsDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(systemOptionsDTODataRow["CreationDate"]),
                                 systemOptionsDTODataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(systemOptionsDTODataRow["LastUpdatedBy"]),
                                 systemOptionsDTODataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(systemOptionsDTODataRow["LastUpdateDate"])
                                 );
            log.LogMethodExit(systemOptionsDTO);
            return systemOptionsDTO;
        }

        /// <summary>
        /// Gets the SystemOptionsDTO data of passed OptionId
        /// </summary>
        /// <param name="optionId">integer type parameter</param>
        /// <returns>Returns SystemOptionsDTO</returns>
        public SystemOptionsDTO GetSystemOptionsDTO(int optionId)
        {
            log.LogMethodEntry(optionId);
            string selectSystemOptionsDTOQuery = @"select *,convert(varchar(max),OptionValue) OptValInStringForm
                                               from SystemOptions
                                              where OptionId = @optionId";
            SqlParameter[] selectSystemOptionsDTOParameters = new SqlParameter[1];
            selectSystemOptionsDTOParameters[0] = new SqlParameter("@optionId", optionId);
            DataTable selectedSystemOptionsDTO = dataAccessHandler.executeSelectQuery(selectSystemOptionsDTOQuery, selectSystemOptionsDTOParameters,sqlTransaction);
            SystemOptionsDTO systemOptionsDTO = new SystemOptionsDTO(); ;
            if (selectedSystemOptionsDTO.Rows.Count > 0)
            {
                DataRow SystemOptionsRow = selectedSystemOptionsDTO.Rows[0];
                systemOptionsDTO = GetSystemOptionsDTO(SystemOptionsRow);
                log.LogMethodExit(systemOptionsDTO);
            }
            return systemOptionsDTO;
        }        

        /// <summary>
        /// Gets the SystemOptionsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SystemOptionsDTO matching the search criteria</returns>
        public List<SystemOptionsDTO> GetSystemOptionsDTOList(List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectSystemOptionsDTOQuery = @"select *, convert(varchar(max),OptionValue) OptValInStringForm from SystemOptions  ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SystemOptionsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : "  and ";
                        if (searchParameter.Key.Equals(SystemOptionsDTO.SearchByParameters.OPTION_ID)
                            || searchParameter.Key.Equals(SystemOptionsDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(SystemOptionsDTO.SearchByParameters.SITE_ID) )
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SystemOptionsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key.Equals(SystemOptionsDTO.SearchByParameters.OPTION_NAME) 
                            || searchParameter.Key.Equals(SystemOptionsDTO.SearchByParameters.OPTION_TYPE)
                            || searchParameter.Key.Equals(SystemOptionsDTO.SearchByParameters.OPTION_VALUE))
                        {
                            query.Append(joinOperartor +  DBSearchParameters[searchParameter.Key] + " =  N''+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+''");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }

                if (searchParameters.Count > 0)
                    selectSystemOptionsDTOQuery = selectSystemOptionsDTOQuery + query;                
            }

            DataTable systemOptionsDTOsData = dataAccessHandler.executeSelectQuery(selectSystemOptionsDTOQuery, parameters.ToArray(),sqlTransaction);
            List<SystemOptionsDTO> systemOptionsDTOsList = new List<SystemOptionsDTO>();
            if (systemOptionsDTOsData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in systemOptionsDTOsData.Rows)
                {
                    SystemOptionsDTO systemOptionsDTOObject = GetSystemOptionsDTO(dataRow);
                    systemOptionsDTOsList.Add(systemOptionsDTOObject);
                }
                //log.Debug("Ends-GetSystemOptionsDTOsList(searchParameters) Method by returning SystemOptionsDTOsList.");
            }
            log.LogMethodExit(systemOptionsDTOsList);
            return systemOptionsDTOsList;
        }
    }
}
