/********************************************************************************************
 * Project Name -EntityOverrideDate DataHandler
 * Description  -Data object of EntityOverrideDate
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By         Remarks          
 *********************************************************************************************
 *1.00       10-July-2017    Amaresh             Created
 *2.70       07-May-2019     Akshay Gulaganji    Added isActive
 *2.70.2       25-Jul-2019     Dakshakh Raj        Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix.
 *2.70.2       06-Dec-2019     Jinto Thomas            Removed siteid from update query                                                           
 *2.100.0     31-Aug-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// EntityOverrideDateDH - Handles insert, update and select of EntityOverrideDates objects
    /// </summary>
    public class EntityOverrideDatesDH
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM EntityOverrideDates as eod";

        /// <summary>
        /// Dictionary for searching Parameters for the EntityOverrideDates object.
        /// </summary>
        private static readonly Dictionary<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string> DBSearchParameters = new Dictionary<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>
            {
                {EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ID, "eod.Id"},
                {EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_NAME, "eod.EntityName"},
                {EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_GUID, "eod.EntityGuid"},
                {EntityOverrideDatesDTO.SearchByEntityOverrideParameters.OVERRIDE_DATE, "eod.OverrideDate"},
                {EntityOverrideDatesDTO.SearchByEntityOverrideParameters.INCLUDE_EXCLUDE_FLAG, "eod.IncludeExcludeFlag"},
                {EntityOverrideDatesDTO.SearchByEntityOverrideParameters.DAY, "eod.Day"},
                {EntityOverrideDatesDTO.SearchByEntityOverrideParameters.SITE_ID, "eod.site_id"},
                {EntityOverrideDatesDTO.SearchByEntityOverrideParameters.IS_ACTIVE, "eod.IsActive"},
                {EntityOverrideDatesDTO.SearchByEntityOverrideParameters.MASTER_ENTITY_ID, "eod.MasterEntityId"}
            };
        
        /// <summary>
        /// Default constructor of EntityOverrideDateDH class
        /// </summary>
        public EntityOverrideDatesDH(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating EntityOverrideDates Reecord.
        /// </summary>
        /// <param name="entityOverrideDatesDTO">entityOverrideDatesDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(EntityOverrideDatesDTO entityOverrideDatesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(entityOverrideDatesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", entityOverrideDatesDTO.ID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@entityName", entityOverrideDatesDTO.EntityName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@entityGuid", entityOverrideDatesDTO.EntityGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@overrideDate", string.IsNullOrEmpty(entityOverrideDatesDTO.OverrideDate) ? DBNull.Value : (object)DateTime.Parse(entityOverrideDatesDTO.OverrideDate)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@includeExcludeFlag", entityOverrideDatesDTO.IncludeExcludeFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@day", entityOverrideDatesDTO.Day == -1 ? DBNull.Value : (object)entityOverrideDatesDTO.Day));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(entityOverrideDatesDTO.Remarks) ? DBNull.Value : (object)entityOverrideDatesDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", entityOverrideDatesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", entityOverrideDatesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the EntityOverrideDatesDT record to the database
        /// </summary>
        /// <param name="entityOverrideDatesDTO">EntityOverrideDatesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        /// <returns>Returns inserted record id</returns>
        public EntityOverrideDatesDTO InsertEntityOverride(EntityOverrideDatesDTO entityOverrideDatesDTO, string loginId, int siteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(entityOverrideDatesDTO, loginId, siteId);
            string insertEntityOverrideQuery = @"insert into EntityOverrideDates 
                                                        (
                                                        EntityName,
                                                        EntityGuid,
                                                        OverrideDate,
                                                        IncludeExcludeFlag,
                                                        Day,
                                                        Remarks,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate
                                                        ) 
                                                values 
                                                       ( 
                                                        @entityName,
                                                        @entityGuid,
                                                        @overrideDate,
                                                        @includeExcludeFlag,
                                                        @day,
                                                        @remarks,
                                                        NEWID(),
                                                        @siteId,
                                                        @masterEntityId,
                                                        @lastUpdatedBy,
                                                        GetDate(),
                                                        @IsActive,
                                                        @createdBy,
                                                        GETDATE()
                                                      )SELECT * FROM EntityOverrideDates WHERE Id = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertEntityOverrideQuery, GetSQLParameters(entityOverrideDatesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshEntityOverrideDatesDTO(entityOverrideDatesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting entityOverrideDatesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(entityOverrideDatesDTO);
            return entityOverrideDatesDTO;
        }

        /// <summary>
        /// Updates the EntityOverride record
        /// </summary>
        /// <param name="entityOverrideDatesDTO">EntityOverrideDatesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public EntityOverrideDatesDTO UpdateEntityOverride(EntityOverrideDatesDTO entityOverrideDatesDTO, string loginId, int siteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(entityOverrideDatesDTO, loginId, siteId);
            string updateEntityOverrideQuery = @"update EntityOverrideDates 
                                               set  EntityName = @entityName,
                                                    EntityGuid = @entityGuid,
                                                    OverrideDate = @overrideDate,
                                                    IncludeExcludeFlag = @includeExcludeFlag,
                                                    Day = @day,
                                                    Remarks = @remarks,
                                                    -- site_id = @siteId,
                                                    MasterEntityId = @masterEntityId,
                                                    LastUpdatedBy = @lastUpdatedBy,
                                                    IsActive = @IsActive,
                                                    LastUpdatedDate = Getdate()
                                                WHERE Id = @id
                                                SELECT* FROM EntityOverrideDates WHERE Id = @id ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateEntityOverrideQuery, GetSQLParameters(entityOverrideDatesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshEntityOverrideDatesDTO(entityOverrideDatesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating entityOverrideDatesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(entityOverrideDatesDTO);
            return entityOverrideDatesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="stateDTO">stateDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshEntityOverrideDatesDTO(EntityOverrideDatesDTO entityOverrideDatesDTO, DataTable dt)
        {
            log.LogMethodEntry(entityOverrideDatesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                entityOverrideDatesDTO.ID = Convert.ToInt32(dt.Rows[0]["Id"]);
                entityOverrideDatesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                entityOverrideDatesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                entityOverrideDatesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                entityOverrideDatesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                entityOverrideDatesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                entityOverrideDatesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Deete the ProductsDisplayGroup data of passed Id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public int DeleteEntityOverride(int id, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(id, sqlTransaction);
            try
            {
                string deleteEntityOverrideQuery = @"DELETE 
                                                            FROM EntityOverrideDates
                                                            WHERE Id = @id";

                SqlParameter[] deleteEntityExlusionParameters = new SqlParameter[1];
                deleteEntityExlusionParameters[0] = new SqlParameter("@id", id);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteEntityOverrideQuery, deleteEntityExlusionParameters, sqlTransaction);

                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// Converts the Data row object to EntityOverrideDatesDTO class type
        /// </summary>
        /// <param name="entityOverrideDataRow">EntityOverride DataRow</param>
        /// <returns>Returns EntityOverride</returns>
        private EntityOverrideDatesDTO GetEntityOverrideDatesDTO(DataRow entityOverrideDataRow)
        {
            log.LogMethodEntry(entityOverrideDataRow);
            EntityOverrideDatesDTO entityOverrideDataObject = new EntityOverrideDatesDTO(
                                            Convert.ToInt32(entityOverrideDataRow["Id"]),
                                            entityOverrideDataRow["EntityName"].ToString(),
                                            entityOverrideDataRow["EntityGuid"].ToString(),
                                            entityOverrideDataRow["OverrideDate"].ToString(),
                                            entityOverrideDataRow["IncludeExcludeFlag"] == DBNull.Value ? false : Convert.ToBoolean(entityOverrideDataRow["IncludeExcludeFlag"]),
                                            entityOverrideDataRow["Day"] == DBNull.Value ? -1 : Convert.ToInt32(entityOverrideDataRow["Day"]),
                                            entityOverrideDataRow["Remarks"].ToString(),
                                            entityOverrideDataRow["LastUpdatedBy"].ToString(),
                                            entityOverrideDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(entityOverrideDataRow["LastUpdatedDate"]),
                                            entityOverrideDataRow["Guid"].ToString(),
                                            entityOverrideDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(entityOverrideDataRow["SynchStatus"]),
                                            entityOverrideDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(entityOverrideDataRow["site_id"]),
                                            entityOverrideDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(entityOverrideDataRow["MasterEntityId"]),
                                            entityOverrideDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(entityOverrideDataRow["IsActive"]),
                                            entityOverrideDataRow["CreatedBy"].ToString(),
                                            entityOverrideDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(entityOverrideDataRow["CreationDate"])
                                            );
            log.LogMethodExit(entityOverrideDataObject);
            return entityOverrideDataObject;
        }

        /// <summary>
        /// Gets the EntityOverride data of passed Id
        /// </summary>
        /// <param name="id">Int type parameter</param>
        /// <returns>Returns EntityOverrideDatesDTO</returns>
        public EntityOverrideDatesDTO GetEntityOverride(int id)
        {
            log.LogMethodEntry(id);
            string selectEntityOverrideQuery = SELECT_QUERY + @" WHERE eod.Id = @id";
            EntityOverrideDatesDTO result = null;
            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectEntityOverrideQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetEntityOverrideDatesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the EntityOverrideDatesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of EntityOverrideDatesDTO matching the search criteria</returns>
        public List<EntityOverrideDatesDTO> GetEntityOverrideList(List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<EntityOverrideDatesDTO> entityOverrideList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectEntityOverrideQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ID ||
                            searchParameter.Key == EntityOverrideDatesDTO.SearchByEntityOverrideParameters.DAY ||
                            searchParameter.Key == EntityOverrideDatesDTO.SearchByEntityOverrideParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == EntityOverrideDatesDTO.SearchByEntityOverrideParameters.INCLUDE_EXCLUDE_FLAG)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_NAME ||
                                searchParameter.Key == EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == EntityOverrideDatesDTO.SearchByEntityOverrideParameters.OVERRIDE_DATE)

                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == EntityOverrideDatesDTO.SearchByEntityOverrideParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == EntityOverrideDatesDTO.SearchByEntityOverrideParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectEntityOverrideQuery = selectEntityOverrideQuery + query;
            }

            DataTable entityOverrideData = dataAccessHandler.executeSelectQuery(selectEntityOverrideQuery, parameters.ToArray(), sqlTransaction);

            if (entityOverrideData.Rows.Count > 0)
            {
                entityOverrideList = new List<EntityOverrideDatesDTO>();
                foreach (DataRow entityOverrideDataRow in entityOverrideData.Rows)
                {
                    EntityOverrideDatesDTO entityOverrideDataObject = GetEntityOverrideDatesDTO(entityOverrideDataRow);
                    entityOverrideList.Add(entityOverrideDataObject);
                }
            }
                log.LogMethodExit(entityOverrideList);
                return entityOverrideList;
        }

        /// <summary>
        /// Gets the EntityOverrideDatesDTO list belonging to the account credit plus
        /// </summary>
        /// <param name="accountIdList">Account Id List</param>
        /// <param name="sqlTransaction">Account Id</param>
        /// <returns>Returns the list of EntityOverrideDatesDTO belonging to the account credit plus</returns>
        public List<EntityOverrideDatesDTO> GetEntityOverrideDatesDTOListForAccountCreditPlus(string accountIdList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(accountIdList, sqlTransaction);
            string selectEntityOverrideQuery = @"SELECT EntityOverrideDates.*
                                                FROM EntityOverrideDates, CardCreditPlus
                                                WHERE EntityOverrideDates.EntityName = 'CARDCREDITPLUS'
                                                AND EntityOverrideDates.EntityGuid = CardCreditPlus.Guid
                                                AND CardCreditPlus.Card_id IN(" + accountIdList + ")";
            DataTable entityOverrideData = dataAccessHandler.executeSelectQuery(selectEntityOverrideQuery, null, sqlTransaction);
            if (entityOverrideData.Rows.Count > 0)
            {
                List<EntityOverrideDatesDTO> entityOverrideList = new List<EntityOverrideDatesDTO>();
                foreach (DataRow entityOverrideDataRow in entityOverrideData.Rows)
                {
                    EntityOverrideDatesDTO entityOverrideDataObject = GetEntityOverrideDatesDTO(entityOverrideDataRow);
                    entityOverrideList.Add(entityOverrideDataObject);
                }
                log.LogMethodExit(entityOverrideList);
                return entityOverrideList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }


        /// <summary>
        /// Get Entity Override Dates DTO List For Account Credit Plus By Account Ids
        /// </summary>
        /// <param name="accountIdList"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<EntityOverrideDatesDTO> GetEntityOverrideDatesDTOListForAccountCreditPlusByAccountIds(List<int> accountIdList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(accountIdList, sqlTransaction);
            List<EntityOverrideDatesDTO> list = new List<EntityOverrideDatesDTO>();
           string selectEntityOverrideQuery = @"SELECT EntityOverrideDates.*
                                                FROM EntityOverrideDates, CardCreditPlus, @accountIdList List 
                                                WHERE EntityOverrideDates.EntityName = 'CARDCREDITPLUS'
                                                AND EntityOverrideDates.EntityGuid = CardCreditPlus.Guid
                                                AND CardCreditPlus.Card_id = List.Id ";
            DataTable table = dataAccessHandler.BatchSelect(selectEntityOverrideQuery, "@accountIdList", accountIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetEntityOverrideDatesDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Gets the EntityOverrideDatesDTO list belonging to the account credit plus
        /// </summary>
        /// <param name="accountIdList">Account Id List</param>
        /// <param name="sqlTransaction">Account Id</param>
        /// <returns>Returns the list of EntityOverrideDatesDTO belonging to the account credit plus</returns>
        public List<EntityOverrideDatesDTO> GetEntityOverrideDatesDTOListForAccountGame(string accountIdList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(accountIdList, sqlTransaction);
            string selectEntityOverrideQuery = @"SELECT EntityOverrideDates.*
                                                FROM EntityOverrideDates, CardGames
                                                WHERE EntityOverrideDates.EntityName = 'CARDGAMES'
                                                AND EntityOverrideDates.EntityGuid = CardGames.Guid
                                                AND CardGames.Card_id IN(" + accountIdList + ")";
            DataTable entityOverrideData = dataAccessHandler.executeSelectQuery(selectEntityOverrideQuery, null, sqlTransaction);
            if (entityOverrideData.Rows.Count > 0)
            {
                List<EntityOverrideDatesDTO> entityOverrideList = new List<EntityOverrideDatesDTO>();
                foreach (DataRow entityOverrideDataRow in entityOverrideData.Rows)
                {
                    EntityOverrideDatesDTO entityOverrideDataObject = GetEntityOverrideDatesDTO(entityOverrideDataRow);
                    entityOverrideList.Add(entityOverrideDataObject);
                }
                log.LogMethodExit(entityOverrideList);
                return entityOverrideList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
    }
}
