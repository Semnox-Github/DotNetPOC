/********************************************************************************************
 * Project Name - Lookup Values Data Handler
 * Description  - Data handler of the lookup values class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Jan-2015   Raghuveera     Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera     Modified 
 *2.40        11-Sep-2018   Rajiv          Modified the existing file to have try catch block.
 *2.60.0      03-May-2019   Divya          SQL Injection
 *2.70        03-Jul-2019   Girish Kundar  Modified : Structure of Insert/ Update function - returns DTO instead of Id. 
 *2.70.2        11-Dec-2019   Jinto Thomas   Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Lookup Value Data Handler - Handles insert, update and select of Lookup Value Data objects
    /// </summary>
    public class LookupValuesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<LookupValuesDTO.SearchByLookupValuesParameters, string> DBSearchParameters = new Dictionary<LookupValuesDTO.SearchByLookupValuesParameters, string>
            {
                {LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, "lv.LookupValueId"},
                {LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_ID, "lv.LookupId"},
                {LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "lv.LookupValue"},
                {LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "l.LookupName"},
                {LookupValuesDTO.SearchByLookupValuesParameters.DESCRIPTION, "lv.Description"},
                {LookupValuesDTO.SearchByLookupValuesParameters.MASTER_ENTITY_ID,"lv.MasterEntityId"},//starts:Modification on 18-Jul-2016 for publish feature
                {LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, "lv.site_id"},//Ends:Modification on 18-Jul-2016 for publish feature
                {LookupValuesDTO.SearchByLookupValuesParameters.IS_ACTIVE, "lv.IsActive"},
                {LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_ID_LIST, "lv.LookupId"}

            };
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM LookupValues AS lv";

        /// <summary>
        /// Default constructor of LookupValuesDataHandler class
        /// </summary>
        public LookupValuesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LookupValues Record.
        /// </summary>
        /// <param name="lookupValuesDTO">LookupValuesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(LookupValuesDTO lookupValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lookupValuesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LookupValueId", lookupValuesDTO.LookupValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lookupId", lookupValuesDTO.LookupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lookupValue", string.IsNullOrEmpty(lookupValuesDTO.LookupValue) ? DBNull.Value : (object)lookupValuesDTO.LookupValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(lookupValuesDTO.Description) ? DBNull.Value : (object)lookupValuesDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lookupName", lookupValuesDTO.LookupName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", lookupValuesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", lookupValuesDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the lookup value record to the database
        /// </summary>
        /// <param name="lookupValuesDTO">LookupValueDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public LookupValuesDTO InsertLookupValue(LookupValuesDTO lookupValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lookupValuesDTO, loginId, siteId);
            //Modification on 18-Jul-2016 for publish feature
            string insertLookupValueQuery = @"insert into LookupValues 
                                                        (
                                                        LookupId,
                                                        LookupValue,
                                                        Description,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId ,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @lookupId,
                                                        @lookupValue,
                                                        @description,
                                                        Newid(),
                                                        @siteid,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        GETDATE(),
                                                        @LastUpdatedBy,
                                                        GETDATE(),
                                                        @isActive
                                                        )
                                                  SELECT * FROM LookupValues WHERE LookupValueId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertLookupValueQuery, GetSQLParameters(lookupValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLookupValuesDTO(lookupValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting lookupValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lookupValuesDTO);
            return lookupValuesDTO;
        }

        /// <summary>
        /// Updates the lookup values record
        /// </summary>
        /// <param name="lookupValues">LookupValuesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the LookupValuesDTO</returns>
        public LookupValuesDTO UpdateLookupValues(LookupValuesDTO lookupValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(lookupValuesDTO, loginId, siteId);
            string updateMaintenanceTaskQuery = @"update LookupValues 
                                         set LookupId=@lookupId,
                                                LookupValue=@lookupValue,
                                                Description=@description,
                                                -- site_id=@siteId,
                                                MasterEntityId=@masterEntityId,
                                                LastUpdatedBy = @lastUpdatedBy,
                                                LastUpdateDate = GETDATE(), 
                                                IsActive = @isActive
                                                where LookupValueId=@lookupValueId
                                                SELECT * FROM LookupValues WHERE LookupValueId=@lookupValueId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMaintenanceTaskQuery, GetSQLParameters(lookupValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLookupValuesDTO(lookupValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating lookupValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lookupValuesDTO);
            return lookupValuesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="lookupValuesDTO">LookupValuesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshLookupValuesDTO(LookupValuesDTO lookupValuesDTO, DataTable dt)
        {
            log.LogMethodEntry(lookupValuesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                lookupValuesDTO.LookupValueId = Convert.ToInt32(dt.Rows[0]["LookupValueId"]);
                lookupValuesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                lookupValuesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                lookupValuesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                lookupValuesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                lookupValuesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                lookupValuesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to LookupValuesDTO class type
        /// </summary>
        /// <param name="lookupValuesDataRow">LookupValues DataRow</param>
        /// <returns>Returns LookupValues</returns>
        private LookupValuesDTO GetLookupValuesDTO(DataRow lookupValuesDataRow)
        {
            log.LogMethodEntry(lookupValuesDataRow);
            LookupValuesDTO userDataObject = new LookupValuesDTO(Convert.ToInt32(lookupValuesDataRow["LookupValueId"]),
                                                    Convert.ToInt32(lookupValuesDataRow["LookupId"]),
                                                    lookupValuesDataRow["LookupName"].ToString(),
                                                    lookupValuesDataRow["LookupValue"].ToString(),
                                                    lookupValuesDataRow["Description"].ToString(),
                                                    lookupValuesDataRow["Guid"].ToString(),
                                                    lookupValuesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(lookupValuesDataRow["SynchStatus"]),
                                                    lookupValuesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(lookupValuesDataRow["site_id"]),
                                                    lookupValuesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(lookupValuesDataRow["MasterEntityId"]),
                                                    lookupValuesDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(lookupValuesDataRow["CreatedBy"]),
                                                    lookupValuesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lookupValuesDataRow["CreationDate"]),
                                                    lookupValuesDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(lookupValuesDataRow["LastUpdatedBy"]),
                                                    lookupValuesDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(lookupValuesDataRow["LastUpdateDate"]),
                                                    lookupValuesDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(lookupValuesDataRow["IsActive"])
                                                   //Modification on 18-Jul-2016 for publish feature
                                                   );
            log.LogMethodExit();
            return userDataObject;
        }

        /// <summary>
        /// Gets the lookup value data of passed lookup Value Id
        /// </summary>
        /// <param name="lookupValueId">integer type parameter</param>
        /// <returns>Returns LookupValuesDTO</returns>
        public LookupValuesDTO GetLookupValues(int lookupValueId)
        {
            try
            {
                log.LogMethodEntry(lookupValueId);
                LookupValuesDTO lookupValuesDataObject = null;
                string selectLookupValuesQuery = @"select *,'' as LookupName
                                         from LookupValues
                                        where LookupValueId = @lookupValueId";
                SqlParameter[] selectLookupValuesParameters = new SqlParameter[1];
                selectLookupValuesParameters[0] = new SqlParameter("@lookupValueId", lookupValueId);
                DataTable lookupValues = dataAccessHandler.executeSelectQuery(selectLookupValuesQuery, selectLookupValuesParameters, sqlTransaction);
                if (lookupValues.Rows.Count > 0)
                {
                    DataRow lookupValuesRow = lookupValues.Rows[0];
                    lookupValuesDataObject = GetLookupValuesDTO(lookupValuesRow);
                }
                log.LogMethodExit(lookupValuesDataObject);
                return lookupValuesDataObject;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Searching  LookupValues based on lookupValueId ", ex);
                log.LogMethodExit(null, "Throwing exception in GetLookupValues(lookupValueId) - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the server datetime
        /// </summary>
        /// <returns>Returns Server datetime</returns>
        public DateTime GetServerDateTime()
        {
            log.LogMethodEntry();
            string selectQuery = @"SELECT GETDATE() as ServerDateTime";
            DateTime datetime = DateTime.Now;
            DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            datetime = Convert.ToDateTime(table.Rows[0]["ServerDateTime"]);
            return datetime;
        }

        /// <summary>
        /// Gets the Inventory lookup value List
        /// </summary>
        /// <returns>Returns DataTable</returns>
        public List<LookupValuesDTO> GetInventoryLookupValues()
        {
            log.LogMethodEntry();
            try
            {
                List<LookupValuesDTO> lookupValuesList = new List<LookupValuesDTO>();
                string selectInventoryLookupValuesQuery = @"SELECT * FROM LookupValues
						                                         WHERE LookupId = (SELECT LookupId 
												                                   FROM Lookups 
												                                   WHERE LookupName = @lookupName)";
                SqlParameter[] selectInventoryLookupValuesParameters = new SqlParameter[1];
                selectInventoryLookupValuesParameters[0] = new SqlParameter("@lookupName", "INVENTORY_NOTES");
                DataTable lookupValuesDataTable = dataAccessHandler.executeSelectQuery(selectInventoryLookupValuesQuery, selectInventoryLookupValuesParameters, sqlTransaction);
                if (lookupValuesDataTable.Rows.Count > 0)
                {

                    foreach (DataRow lookupValuesDataRow in lookupValuesDataTable.Rows)
                    {
                        LookupValuesDTO lookupValuesDataObject = GetLookupValuesDTO(lookupValuesDataRow);
                        lookupValuesList.Add(lookupValuesDataObject);
                    }

                }
                log.LogMethodExit(lookupValuesList);
                return lookupValuesList;

            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetInventoryLookupValues() ", ex);
                log.LogMethodExit(null, "Throwing exception in GetInventoryLookupValues() - " + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Gets the LookupValuesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LookupValuesDTO matching the search criteria</returns>
        public List<LookupValuesDTO> GetLookupValuesList(List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters)
        {
            try
            {
                log.LogMethodEntry(searchParameters);
                int count = 0;
                string selectLookupValuesQuery = @"select lv.*,l.LookupName from LookupValues 
                                                lv join Lookups l on lv.LookupId=l.LookupId";
                List<SqlParameter> parameters = new List<SqlParameter>();
                List<LookupValuesDTO> lookupValuesList = null;
                if (searchParameters != null)
                {
                    string joiner = string.Empty;//starts:Modification on 18-Jul-2016 for publish feature
                    StringBuilder query = new StringBuilder(" where ");
                    foreach (KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            log.Debug(searchParameter.Key + ": " + searchParameter.Value);
                            joiner = (count == 0) ? string.Empty : " and ";
                            if (searchParameter.Key == LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_ID ||
                                searchParameter.Key == LookupValuesDTO.SearchByLookupValuesParameters.MASTER_ENTITY_ID ||
                                searchParameter.Key == LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME)
                            {
                                query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + " =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key == LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == LookupValuesDTO.SearchByLookupValuesParameters.IS_ACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                            }
                            else if (searchParameter.Key == LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_ID_LIST)  //int
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));

                            }//Ends: Modification on 18-Jul-2016 for publish feature                        
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
                        selectLookupValuesQuery = selectLookupValuesQuery + query;
                }

                DataTable lookupValuesData = dataAccessHandler.executeSelectQuery(selectLookupValuesQuery, parameters.ToArray(), sqlTransaction);
                if (lookupValuesData.Rows.Count > 0)
                {
                    lookupValuesList = new List<LookupValuesDTO>();
                    foreach (DataRow lookupValuesDataRow in lookupValuesData.Rows)
                    {
                        LookupValuesDTO lookupValuesDataObject = GetLookupValuesDTO(lookupValuesDataRow);
                        lookupValuesList.Add(lookupValuesDataObject);
                    }
                }
                log.LogMethodExit(lookupValuesList);
                return lookupValuesList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception -" + ex.Message );
                throw;
            }

        }

        /// <summary>
        /// Gets the Inventory lookup value List
        /// </summary>
        /// <returns>Returns DataTable</returns>
        public List<LookupValuesDTO> GetInventoryLookupValuesByValueName(string LookupName, int SiteID)
        {
            log.LogMethodEntry(LookupName, SiteID);
            try
            {
                List<LookupValuesDTO> lookupValuesList = new List<LookupValuesDTO>();
                string selectInventoryLookupValuesQuery = @"select v.*, l.LookupName
                                                        from lookups l, lookupvalues v
                                                        where lookupname = @lookupName
	                                                        and l.lookupid = v.lookupid 
									                        and (v.site_id = @SiteID or @SiteID = -1)";
                SqlParameter[] selectInventoryLookupValuesParameters = new SqlParameter[2];
                selectInventoryLookupValuesParameters[0] = new SqlParameter("@lookupName", LookupName);
                selectInventoryLookupValuesParameters[1] = new SqlParameter("@SiteID", SiteID);
                DataTable lookupValuesDataTable = dataAccessHandler.executeSelectQuery(selectInventoryLookupValuesQuery, selectInventoryLookupValuesParameters, sqlTransaction);
                if (lookupValuesDataTable.Rows.Count > 0)
                {

                    foreach (DataRow lookupValuesDataRow in lookupValuesDataTable.Rows)
                    {
                        LookupValuesDTO lookupValuesDataObject = GetLookupValuesDTO(lookupValuesDataRow);
                        lookupValuesList.Add(lookupValuesDataObject);
                    }
                }
                log.LogMethodExit(lookupValuesList);
                return lookupValuesList;
            }
            catch (Exception ex)
            {
                string message = "The exception occurred  at GetInventoryLookupValuesByValueName(string LookupName, int SiteID)";
                log.LogMethodExit(null, "Throwing exception -" + ex.Message + "Error: " + message);
                throw new Exception(message);
            }

        }
        /// <summary>
        /// Gets the server time zone
        /// </summary>
        /// <returns>Returns Server TimeZone</returns>
        public string GetServerTimeZone()
        {
            log.LogMethodEntry();
            string selectQuery = @"DECLARE @TimeZone VARCHAR(50) ;
                                EXEC MASTER.dbo.xp_regread 'HKEY_LOCAL_MACHINE', 'SYSTEM\CurrentControlSet\Control\TimeZoneInformation', 'TimeZoneKeyName', @TimeZone OUT ;
                                SELECT @TimeZone ";
            string serverTimeZone = dataAccessHandler.executeScalar(selectQuery, null, sqlTransaction).ToString();
            log.LogMethodExit(serverTimeZone);
            return serverTimeZone;
        }
        /// <summary>
        /// Based on the lookupValueId, appropriate LookupValues record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="lookupId">primary key of lookupValues </param>
        /// <returns>return the int </returns>
        internal int DeleteLookupValues(int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            try
            {
                string deleteLookupValuesQuery = @"delete from LookupValues where LookupValueId = @lookupValueId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@lookupValueId", lookupValueId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteLookupValuesQuery, deleteParameters, sqlTransaction);
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
    }
}
