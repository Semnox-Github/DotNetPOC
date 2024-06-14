/********************************************************************************************
 * Project Name - Utilities
 * Description  - Get and Insert or update methods for Lookups details.
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.60        13-Mar-2019   Jagan Mohana          Created  
 *2.60        09-Apr-2019   Akshay Gulaganji      modified GetLookupsDTO(),InsertLookups() and UpdateLookups()
 *2.60        10-Apr-2019   Mushahid Faizan       Modified- logMethodEntry/logMethodExit.
 *2.70        3- Jul- 2019  Girish Kundar         Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue  
 *2.70.2      11-Dec-2019   Jinto Thomas        Removed siteid from update query     
 *2.90        11-Aug-2020   Girish Kundar       Modified : Added GetLookupModuleLastUpdateTime method to get last update date 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Core.Utilities
{
    public class LookupsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * from lookups AS lp";
        private static readonly Dictionary<LookupsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LookupsDTO.SearchByParameters, string>
        {
            { LookupsDTO.SearchByParameters.LOOKUP_ID,"lp.LookupId"},
            { LookupsDTO.SearchByParameters.LOOKUP_NAME, "lp.LookupName"},
            { LookupsDTO.SearchByParameters.SITE_ID, "lp.site_id"},
            { LookupsDTO.SearchByParameters.MASTER_ENTITY_ID, "lp.MasterEntityId"},
            { LookupsDTO.SearchByParameters.ISACTIVE, "lp.IsActive"}
        };

        /// <summary>
        /// Default constructor of LookupsDataHandler class
        /// </summary>
        public LookupsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to LookupsDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow  object</param>
        /// <returns>LookupsDTO</returns>
        private LookupsDTO GetLookupsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LookupsDTO lookupsDTO = new LookupsDTO(Convert.ToInt32(dataRow["LookupId"]),
                                            dataRow["LookupName"] == DBNull.Value ?string.Empty : dataRow["LookupName"].ToString(),
                                            dataRow["Protected"] == DBNull.Value ?string.Empty : dataRow["Protected"].ToString(),
                                            dataRow["Guid"] == DBNull.Value ?string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),                                            
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ?string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                            );
            log.LogMethodExit(lookupsDTO);
            return lookupsDTO;
        }


        /// <summary>
        /// Gets the LookupsDTO data of passed id 
        /// </summary>
        /// <param name="id">id of Lookups is passed as parameter</param>
        /// <returns>Returns LookupsDTO</returns>
        public LookupsDTO GetLookupsDTO(int id)
        {
            log.LogMethodEntry(id);
            LookupsDTO result = null;
            string query = SELECT_QUERY + @" WHERE lp.LookupId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLookupsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Gets the LookupsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LookupsDTO matching the search criteria</returns>
        public List<LookupsDTO> GetLookups(List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<LookupsDTO> lookupsDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<LookupsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        {
                            if (searchParameter.Key.Equals(LookupsDTO.SearchByParameters.LOOKUP_ID)
                                || searchParameter.Key.Equals(LookupsDTO.SearchByParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == LookupsDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == LookupsDTO.SearchByParameters.LOOKUP_NAME)
                            {
                                query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + " =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key == LookupsDTO.SearchByParameters.ISACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                            }
                            else
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
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
                    selectQuery = selectQuery + query;
            }

            DataTable companyData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (companyData.Rows.Count > 0)
            {
                lookupsDTOList = new List<LookupsDTO>();
                foreach (DataRow lookupDataRow in companyData.Rows)
                {
                    LookupsDTO lookupDataObject = GetLookupsDTO(lookupDataRow);
                    lookupsDTOList.Add(lookupDataObject);
                }
            }
            log.LogMethodExit(lookupsDTOList);
            return lookupsDTOList;
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating lookups Record.
        /// </summary>
        /// <param name="lookupsDTO">LookupsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        ///  Modified by Mushahid Faizan on 10-Apr-2019 to call DataAccessHandler GetSQLParameter method
        private List<SqlParameter> GetSQLParameters(LookupsDTO lookupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lookupsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@lookupId", lookupsDTO.LookupId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@protected", lookupsDTO.IsProtected));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lookupName", lookupsDTO.LookupName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));           
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", lookupsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", lookupsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the lookup record to the database
        /// </summary>
        /// <param name="lookupsDTO">LookupsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns LookupsDTO</returns>
        public LookupsDTO InsertLookups(LookupsDTO lookupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lookupsDTO, loginId, siteId);
            string query = @"insert into lookups 
                                                        (                                                         
                                                        LookupName,
                                                        Protected,                                                                                                              
                                                        Guid, 
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @lookupName,
                                                        @protected,                                                        
                                                        NewId(), 
                                                        @siteId,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        GETDATE(),                                                        
                                                        @lastUpdatedBy,
                                                        GetDate(),
                                                        @isActive
                                            ) SELECT * FROM lookups WHERE LookupId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(lookupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLookupsDTO(lookupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LookupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lookupsDTO);
            return lookupsDTO;
        }

        /// <summary>
        /// Updates the lookup record
        /// </summary>
        /// <param name="lookupsDTO">LookupsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns LookupsDTO</returns>
        public LookupsDTO UpdateLookups(LookupsDTO lookupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lookupsDTO, loginId, siteId);
            string query = @"update Lookups 
                                         set LookupName=@lookupName,
                                             Protected= @protected, 
                                             MasterEntityId = @masterEntityId,
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastUpdateDate = GETDATE(),
                                             IsActive = @isActive
                                       where LookupId = @lookupId
                                       SELECT * FROM lookups WHERE LookupId = @lookupId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(lookupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLookupsDTO(lookupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LookupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lookupsDTO);
            return lookupsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="lookupsDTO">LookupsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshLookupsDTO(LookupsDTO lookupsDTO, DataTable dt)
        {
            log.LogMethodEntry(lookupsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                lookupsDTO.LookupId = Convert.ToInt32(dt.Rows[0]["LookupId"]);
                lookupsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                lookupsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                lookupsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                lookupsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                lookupsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                lookupsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Based on the lookupId, appropriate Lookups record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="lookupId">primary key of lookups </param>
        /// <returns>return the int </returns>
        public int DeleteLookups(int lookupId)
        {
            log.LogMethodEntry(lookupId);
            try
            {
                string deleteLookupQuery = @"delete from lookups where LookupId = @lookupId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@lookupId", lookupId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteLookupQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        internal DateTime? GetLookupModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate
                            FROM (
                            select max(LastUpdateDate) LastUpdatedDate from lookups WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from LookupValues WHERE (site_id = @siteId or @siteId = -1)
                             )a";
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
    }
}
