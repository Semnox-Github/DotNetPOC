/********************************************************************************************
 * Project Name - EntityExclusionDetail Data Handler
 * Description  - Data handler of the EntityExclusionDetail class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        15-Jul-2019      Girish Kundar       Modified : Added GetSQLParameter(),SQL Injection Fix,Missed Who columns
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
    /// Data handler for entity exclusion
    /// </summary>
    public class EntityExclusionDetailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string> DBSearchParameters = new Dictionary<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>
            {
                {EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.RULE_DETAIL_ID, "exd.RuleDetailId"},
                {EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.EXCLUSION_ID, "exd.ExclusionId"},
                {EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.TABLE_ATTRIBUTE_GUID, "exd.TableAttributeGuid"},
                {EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.TABLE_ATTRIBUTE_ID, "exd.TableAttributeId"},
                {EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.TABLE_NAME, "exd.TableName"},
                {EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.ACTIVE_FLAG, "exd.IsActive"},
                {EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.MASTER_ENTITY_ID,"exd.MasterEntityId"},
                {EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.SITE_ID, "exd.site_id"},
                 {EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.FIELD_NAME, "exd.FieldName"}
            };
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM EntityExclusionDetail AS exd ";
        private SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of EntityExclusionDetailDataHandler class
        /// </summary>
        public EntityExclusionDetailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating EntityExclusionDetail Record.
        /// </summary>
        /// <param name="entityExclusionDetailDTO">EntityExclusionDetailDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(EntityExclusionDetailDTO entityExclusionDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(entityExclusionDetailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@exclusionId", entityExclusionDetailDTO.ExclusionId, true);
            ParametersHelper.ParameterHelper(parameters, "@ruleDetailId", entityExclusionDetailDTO.RuleDetailId, true);
            ParametersHelper.ParameterHelper(parameters, "@tableAttributeId", entityExclusionDetailDTO.TableAttributeId, true);
            ParametersHelper.ParameterHelper(parameters, "@tableName", string.IsNullOrEmpty(entityExclusionDetailDTO.TableName) ? DBNull.Value : (object)entityExclusionDetailDTO.TableName);
            ParametersHelper.ParameterHelper(parameters, "@tableAttributeGuid", string.IsNullOrEmpty(entityExclusionDetailDTO.TableAttributeGuid) ? DBNull.Value : (object)entityExclusionDetailDTO.TableAttributeGuid);
            ParametersHelper.ParameterHelper(parameters, "@fieldName", string.IsNullOrEmpty(entityExclusionDetailDTO.FieldName) ? DBNull.Value : (object)entityExclusionDetailDTO.FieldName);
            ParametersHelper.ParameterHelper(parameters, "@isActive", entityExclusionDetailDTO.IsActive);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@siteId", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", entityExclusionDetailDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the Entity Exclusion Detail record to the database
        /// </summary>
        /// <param name="entityExclusionDetailDTO">EntityExclusionDetailDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>Returns EntityExclusionDetailDTO</returns>
        public EntityExclusionDetailDTO InsertEntityExclusionDetail(EntityExclusionDetailDTO entityExclusionDetailDTO, string loginId, int siteId)
        {

            log.LogMethodEntry(entityExclusionDetailDTO, loginId, siteId);
            string insertEntityExclusionDetailQuery = @"insert into EntityExclusionDetail 
                                                        ( 
                                                         RuleDetailId
                                                        ,TableName
                                                        ,TableAttributeId
                                                        ,TableAttributeGuid
                                                        ,CreatedBy
                                                        ,CreationDate
                                                        ,LastUpdatedBy
                                                        ,LastupdatedDate
                                                        ,Guid
                                                        ,site_id
                                                        ,MasterEntityId
                                                        ,IsActive
                                                        ,FieldName
                                                        ) 
                                                values 
                                                        (                                                         
                                                         @ruleDetailId
                                                        ,@tableName
                                                        ,@tableAttributeId
                                                        ,@tableAttributeGuid
                                                        ,@createdBy
                                                        ,getdate()
                                                        ,@lastUpdatedBy
                                                        ,getdate()
                                                        ,NewId()
                                                        ,@siteId
                                                        ,@masterEntityId
                                                        ,@isActive
                                                        ,@fieldName )SELECT  * from EntityExclusionDetail where ExclusionId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertEntityExclusionDetailQuery, BuildSQLParameters(entityExclusionDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshEntityExclusionDetailDTO(entityExclusionDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting entityExclusionDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(entityExclusionDetailDTO);
            return entityExclusionDetailDTO;
        }

        /// <summary>
        /// Updates the Entity Exclusion Detail record
        /// </summary>
        /// <param name="entityExclusionDetailDTO">EntityExclusionDetailDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>Returns the count of updated rows</returns>
        public EntityExclusionDetailDTO UpdateEntityExclusionDetail(EntityExclusionDetailDTO entityExclusionDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(entityExclusionDetailDTO, loginId, siteId);
            string updateEntityExclusionDetailQuery = @"update EntityExclusionDetail 
                                         set RuleDetailId = @ruleDetailId,
                                             TableName = @tableName,
                                             TableAttributeId = @tableAttributeId,
                                             TableAttributeGuid = @tableAttributeGuid,
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             -- site_id=@siteid,
                                             MasterEntityId=@masterEntityId,
                                             FieldName = @fieldName
                                       where ExclusionId = @exclusionId
                                  SELECT  * from EntityExclusionDetail where ExclusionId = @exclusionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateEntityExclusionDetailQuery, BuildSQLParameters(entityExclusionDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshEntityExclusionDetailDTO(entityExclusionDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating entityExclusionDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(entityExclusionDetailDTO);
            return entityExclusionDetailDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="entityExclusionDetailDTO">EntityExclusionDetailDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshEntityExclusionDetailDTO(EntityExclusionDetailDTO entityExclusionDetailDTO, DataTable dt)
        {
            log.LogMethodEntry(entityExclusionDetailDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                entityExclusionDetailDTO.ExclusionId = Convert.ToInt32(dt.Rows[0]["ExclusionId"]);
                entityExclusionDetailDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                entityExclusionDetailDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                entityExclusionDetailDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                entityExclusionDetailDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                entityExclusionDetailDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                entityExclusionDetailDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to entityExclusionDetailDTO class type
        /// </summary>
        /// <param name="entityExclusionDetailDataRow">entityExclusionDetail DataRow</param>
        /// <returns>Returns EntityExclusionDetailDTO</returns>
        private EntityExclusionDetailDTO GetEntityExclusionDetailDTO(DataRow entityExclusionDetailDataRow)
        {
            log.LogMethodEntry(entityExclusionDetailDataRow);
            EntityExclusionDetailDTO entityExclusionDetailDataObject = new EntityExclusionDetailDTO(Convert.ToInt32(entityExclusionDetailDataRow["ExclusionId"]),
                                            entityExclusionDetailDataRow["RuleDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(entityExclusionDetailDataRow["RuleDetailId"]),
                                            entityExclusionDetailDataRow["TableName"] == DBNull.Value ? string.Empty : Convert.ToString(entityExclusionDetailDataRow["TableName"]),
                                            entityExclusionDetailDataRow["TableAttributeId"] == DBNull.Value ? -1 : Convert.ToInt32(entityExclusionDetailDataRow["TableAttributeId"]),
                                            entityExclusionDetailDataRow["TableAttributeGuid"] == DBNull.Value ? string.Empty : Convert.ToString(entityExclusionDetailDataRow["TableAttributeGuid"]),
                                            entityExclusionDetailDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(entityExclusionDetailDataRow["IsActive"]),
                                            entityExclusionDetailDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(entityExclusionDetailDataRow["CreatedBy"]),
                                            entityExclusionDetailDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(entityExclusionDetailDataRow["CreationDate"]),
                                            entityExclusionDetailDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(entityExclusionDetailDataRow["LastUpdatedBy"]),
                                            entityExclusionDetailDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(entityExclusionDetailDataRow["LastupdatedDate"]),
                                            entityExclusionDetailDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(entityExclusionDetailDataRow["Guid"]),
                                            entityExclusionDetailDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(entityExclusionDetailDataRow["site_id"]),
                                            entityExclusionDetailDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(entityExclusionDetailDataRow["SynchStatus"]),
                                            entityExclusionDetailDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(entityExclusionDetailDataRow["MasterEntityId"]),
                                            entityExclusionDetailDataRow["FieldName"] == DBNull.Value ? string.Empty : Convert.ToString(entityExclusionDetailDataRow["FieldName"])
                                            );
            log.LogMethodExit(entityExclusionDetailDataObject);
            return entityExclusionDetailDataObject;
        }
        /// <summary>
        /// Gets the Entity Exclusion Detail data of passed ruleDetailId
        /// </summary>
        /// <param name="ruleDetailId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns list EntityExclusionDetailDTO</returns>
        public List<EntityExclusionDetailDTO> GetEntityExclusionDetailList(int ruleDetailId)
        {
            log.LogMethodEntry(ruleDetailId, sqlTransaction);
            List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>> entityExclusionDetailSearchParams = new List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>>();
            //entityExclusionDetailSearchParams.Add(new KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>(EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.ACTIVE_FLAG, "1"));
            entityExclusionDetailSearchParams.Add(new KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>(EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.RULE_DETAIL_ID, ruleDetailId.ToString()));
            List<EntityExclusionDetailDTO> entityExclusionDetailDTOList = GetEntityExclusionDetailList(entityExclusionDetailSearchParams);
            log.LogMethodExit(entityExclusionDetailDTOList);
            return entityExclusionDetailDTOList;
        }
        /// <summary>
        /// Getting entity data
        /// </summary>
        /// <param name="entityName">Entity name </param>
        /// <returns></returns>
        public DataTable GetEntityData(string entityName)
        {
            log.LogMethodEntry(entityName);
            string selectEntityExclusionDetailQuery = @"SELECT *, Convert(Varchar(40), guid) as AttributeGuid 
                                              FROM " + entityName;
            DataTable entitydata = dataAccessHandler.executeSelectQuery(selectEntityExclusionDetailQuery, null, sqlTransaction);
            log.LogMethodExit(entitydata);
            return entitydata;

        }

        /// <summary>
        /// Getting entity field
        /// </summary>
        /// <param name="entityName">Entity name </param>
        /// <returns></returns>
        public DataTable GetEntityField(string entityName)
        {
            log.LogMethodEntry(entityName);
            string selectEntityExclusionDetailQuery = @"SELECT '' as AttributeGuid, '" + entityName + "' as FieldName";
            DataTable entitydata = dataAccessHandler.executeSelectQuery(selectEntityExclusionDetailQuery, null, sqlTransaction);
            log.LogMethodExit(entitydata);
            return entitydata;
        }

        /// <summary>
        /// Gets the Entity Exclusion Detail data of passed entityExclusionDetailId
        /// </summary>
        /// <param name="entityExclusionDetailId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns EntityExclusionDetailDTO</returns>
        public EntityExclusionDetailDTO GetEntityExclusionDetail(int entityExclusionDetailId)
        {
            log.LogMethodEntry(entityExclusionDetailId);
            EntityExclusionDetailDTO entityExclusionDetailDataObject = null;
            string selectEntityExclusionDetailQuery = SELECT_QUERY + "  WHERE exd.ExclusionId = @entityExclusionDetailId";
            SqlParameter[] selectEntityExclusionDetailParameters = new SqlParameter[1];
            selectEntityExclusionDetailParameters[0] = new SqlParameter("@entityExclusionDetailId", entityExclusionDetailId);
            DataTable entityExclusionDetail = dataAccessHandler.executeSelectQuery(selectEntityExclusionDetailQuery, selectEntityExclusionDetailParameters, sqlTransaction);
            if (entityExclusionDetail.Rows.Count > 0)
            {
                DataRow entityExclusionDetailRow = entityExclusionDetail.Rows[0];
                entityExclusionDetailDataObject = GetEntityExclusionDetailDTO(entityExclusionDetailRow);

            }
            log.LogMethodExit(entityExclusionDetailDataObject, "returning entityExclusionDetailDataObject.");
            return entityExclusionDetailDataObject;
        }

        /// <summary>
        /// Gets the EntityExclusionDetailDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of EntityExclusionDetailDTO matching the search criteria</returns>
        public List<EntityExclusionDetailDTO> GetEntityExclusionDetailList(List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<EntityExclusionDetailDTO> entityExclusionDetailList = null;
            string selectEntityExclusionDetailQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.EXCLUSION_ID
                            || searchParameter.Key == EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.RULE_DETAIL_ID
                            || searchParameter.Key == EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.ACTIVE_FLAG
                            || searchParameter.Key == EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.TABLE_ATTRIBUTE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.TABLE_ATTRIBUTE_GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.FIELD_NAME
                           || searchParameter.Key == EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.TABLE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                    selectEntityExclusionDetailQuery = selectEntityExclusionDetailQuery + query;
            }
            DataTable entityExclusionDetailData = dataAccessHandler.executeSelectQuery(selectEntityExclusionDetailQuery, parameters.ToArray(), sqlTransaction);
            if (entityExclusionDetailData.Rows.Count > 0)
            {
                entityExclusionDetailList = new List<EntityExclusionDetailDTO>();
                foreach (DataRow entityExclusionDetailDataRow in entityExclusionDetailData.Rows)
                {
                    EntityExclusionDetailDTO entityExclusionDetailDataObject = GetEntityExclusionDetailDTO(entityExclusionDetailDataRow);
                    entityExclusionDetailList.Add(entityExclusionDetailDataObject);
                }
            }
                log.LogMethodExit(entityExclusionDetailList, "returning entityExclusionDetailList.");
                return entityExclusionDetailList;
        }
    }
}
