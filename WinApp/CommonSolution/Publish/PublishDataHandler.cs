/********************************************************************************************
 * Project Name - Publish
 * Description  - Datahandler of publish to site function
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      17-Oct-2019   Guru S A       Waiver-Phase-2 Enhancements
 *2.80.3      02-Apr-2020   Girish Kundar  Modified : Added code to add dbaudit log entry for all the publishable entities
  ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Publish
{
    /// <summary>
    /// class of PublishDataHandler
    /// </summary>
    public class PublishDataHandler
    {
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Dictionary<string, string> lstDependentColumns;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PublishDataHandler()
        {
            log.Debug("Starts-PublishDataHandler() method");

            lstDependentColumns = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            //Products child table
            lstDependentColumns.Add("product_type", "product_type");
            lstDependentColumns.Add("tax", "tax_name");
            lstDependentColumns.Add("TaxStructure", "Taxid");
            lstDependentColumns.Add("CheckInFacility", "FacilityName");
            lstDependentColumns.Add("POSTypes", "POSTypeName");
            //lstDependentColumns.Add("CardType", "CardType");
            lstDependentColumns.Add("MembershipRule", "RuleName");
            lstDependentColumns.Add("Membership", "MembershipName");
            lstDependentColumns.Add("MembershipRewards", "RewardName");
            lstDependentColumns.Add("Category", "Name");
            lstDependentColumns.Add("ReceiptPrintTemplateHeader", "TemplateName");
            lstDependentColumns.Add("AttractionMasterSchedule", "MasterScheduleName");
            lstDependentColumns.Add("EmailTemplate", "Name");

            //lstDependentColumns.Add("products", "product_name");
            lstDependentColumns.Add("games", "game_name");
            lstDependentColumns.Add("game_profile", "profile_name");
            lstDependentColumns.Add("discounts", "discount_name");
            lstDependentColumns.Add("ModifierSet", "SetName");
            lstDependentColumns.Add("SpecialPricing", "PricingName");
            lstDependentColumns.Add("ProductDisplayGroupFormat", "DisplayGroup");
            lstDependentColumns.Add("Languages", " LanguageCode");
            lstDependentColumns.Add("machines", "machine_name");
            lstDependentColumns.Add("GameProfileAttributes", "Attribute");
            lstDependentColumns.Add("Messages", "MessageNo");
            lstDependentColumns.Add("Theme", "ThemeNumber");
            lstDependentColumns.Add("CustomAttributes", "Name");
            lstDependentColumns.Add("CustomAttributeValueList", "Value");
            lstDependentColumns.Add("CustomDataSet", "CustomDataSetId");
            lstDependentColumns.Add("CustomData", "CustomAttributeId");
            lstDependentColumns.Add("Vendor", "Name");
            lstDependentColumns.Add("Location", "Name");
            lstDependentColumns.Add("UOM", "UOM");
            lstDependentColumns.Add("PurchaseTax", "TaxName");
            lstDependentColumns.Add("RedemptionCurrency", "CurrencyName");
            lstDependentColumns.Add("Product", "Code");
            lstDependentColumns.Add("Segment_Definition", "SegmentName");
            lstDependentColumns.Add("Segment_Definition_Source_Mapping", "SegmentDefinitionId");
            lstDependentColumns.Add("Segment_Definition_Source_Values", "SegmentDefinitionSourceId");
            //lstDependentColumns.Add("Segment_Categorization", "SegmentCategoryId");
            lstDependentColumns.Add("Segment_Categorization_Values", "SegmentCategoryId");
            lstDependentColumns.Add("ProductBarcode", "BarCode");
            lstDependentColumns.Add("InventoryDocumentType", "Code");
            lstDependentColumns.Add("CustFeedbackSurvey", "SurveyName");
            lstDependentColumns.Add("parafait_defaults", "default_value_name");
            lstDependentColumns.Add("Sequences", "SeqName");
            lstDependentColumns.Add("Lookups", "LookupName");
            lstDependentColumns.Add("LookupValues", "LookupValue,Description");
            lstDependentColumns.Add("Maint_Assets", "Name");
            lstDependentColumns.Add("Maint_Asset_Types", "Name");
            lstDependentColumns.Add("Maint_AssetGroups", "AssetGroupName");
            lstDependentColumns.Add("Maint_Tasks", "TaskName");
            lstDependentColumns.Add("Maint_TaskGroups", "TaskGroupName");

            log.Debug("Ends-PublishDataHandler() method");
        }

        /// <summary>
        /// class of Entity
        /// </summary>
        public class Entity
        {
            internal string Name;
            internal string ForeignKey; // specify the foreign key for child entities, in case there are multiple, and you want to be sure which one (e.g., UpsellOffers has 2 productId fks - ProductId & OfferProductId)
            /// <summary>
            /// List of Entity
            /// </summary>
            public List<Entity> ChildList = new List<Entity>();

            /// <summary>
            /// Constructor of Entity
            /// </summary>
            /// <param name="entityName"></param>
            public Entity(string entityName)
            {
                Name = entityName;
            }

            /// <summary>
            /// Constructor of Entity
            /// </summary>
            /// <param name="entityName"></param>
            /// <param name="foreignKey"></param>
            public Entity(string entityName, string foreignKey)
            {
                Name = entityName;
                ForeignKey = foreignKey;
            }
        }

        /// <summary>
        /// Returns the PK Column from table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public object getPKColName(string tableName)
        {
            log.Debug("Starts-getPKColName() method");
            string Query = @"select c.name column_name
		                                            from sys.tables t, sys.columns c 
		                                            where c.object_id = t.object_id
		                                            and c.is_identity = 1
		                                            and t.name = @TableName";

            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@tableName", tableName);

            object pkColName = ExecuteScalar(Query, selectParameters);

            if (pkColName == null)
            {
                log.Debug("Ends-getPKColName() method");
                throw new ApplicationException("Primary Key column not found for table: " + tableName);
            }

            log.Debug("Ends-getPKColName() method");
            return pkColName;
        }

        /// <summary>
        /// Returns object for passed query and sql Parameters
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        object ExecuteScalar(string query, SqlParameter[] sqlParameters, SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-ExecuteScalar() method");
            DataTable dt = dataAccessHandler.executeSelectQuery(query, sqlParameters, SQLTrx);

            if (dt != null && dt.Rows.Count > 0)
            {
                log.Debug("ends-ExecuteScalar() method");
                return dt.Rows[0][0];
            }
            else
            {
                log.Debug("ends-ExecuteScalar() method");
                return null;
            }
        }

        /// <summary>
        /// Added to update master entity in site for passed primary Key
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="pkId"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="SQLTrx"></param>
        public void UpdateSiteEntity(string entityName, int pkId, int masterEntityId, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-UpdateSiteEntity() Handler method");

            Object pkColName = getPKColName(entityName);
            string query;

            query = "UPDATE " + entityName + " SET MasterEntityId =" + masterEntityId.ToString() +
                    " WHERE " + pkColName.ToString() + " = " + pkId.ToString();

            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[0], SQLTrx);

            log.Debug("Ends-UpdateSiteEntity() Handler method");
        }

        /// <summary>
        /// Update the site entity details
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="pkId"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="SQLTrx"></param>
        /// <param name="activeFlagColumnName"></param>
        public void InActivateSiteEntity(string entityName, int pkId, int masterEntityId, SqlTransaction SQLTrx, string activeFlagColumnName)
        {
            log.Debug("Ends-InActivateSiteEntity(string entityName, int pkId,  int masterEntityId, SqlTransaction SQLTrx, string activeFlagColumnName) Handler method");

            Object pkColName = getPKColName(entityName);
            string query;

            object dataType = GetDataType(entityName, activeFlagColumnName);
            if (dataType != null && !string.IsNullOrEmpty(pkColName.ToString()))
            {
                query = "UPDATE " + entityName + " SET " + activeFlagColumnName + " = @activeflag " +
                            "WHERE " + pkColName.ToString() + " = " + pkId.ToString();

                SqlParameter[] updateParameters = new SqlParameter[1];

                if (dataType.ToString().Equals("char"))
                {
                    updateParameters[0] = new SqlParameter("@activeflag", "N");
                }
                else if (dataType.ToString().Equals("bit"))
                {
                    updateParameters[0] = new SqlParameter("@activeflag", false);
                }

                dataAccessHandler.executeUpdateQuery(query, updateParameters, SQLTrx);
            }

            log.Debug("Ends-InActivateSiteEntity(string entityName, string columnName, int pkId, SqlTransaction SQLTrx) Handler method");
        }

        /// <summary>
        /// Publish the passed entity details
        /// </summary>
        /// <param name="masterEntityId"></param> 
        /// <param name="siteId"></param>
        /// <param name="MasterEntity"></param>
        /// <param name="SQLTrx"></param>
        public void Publish(object masterEntityId, int siteId, Entity MasterEntity, SqlTransaction SQLTrx, string userId = null)
        {
            log.Debug("Starts-Publish() method");
            object pkColName = getPKColName(MasterEntity.Name);
            publishRecord(MasterEntity.Name, pkColName.ToString(), masterEntityId, siteId, SQLTrx , userId);

            foreach (Entity childTable in MasterEntity.ChildList)
            {
                // objectTransaltions uses elementGuid - guids of parents instead of foreign keys. hence processing is done differently
                if (childTable.Name.Equals("ObjectTranslations", StringComparison.CurrentCultureIgnoreCase))
                {
                    publishObjectTranslations(masterEntityId, siteId, MasterEntity, pkColName, SQLTrx);
                }
                else
                {
                    object childColumn = "";
                    if (!string.IsNullOrEmpty(childTable.ForeignKey))
                        childColumn = childTable.ForeignKey;
                    else
                    {
                        string childColumnQuery = @"select c.name child_column
		                                                    from sys.tables t,
		                                                    sys.columns c,
		                                                    sys.foreign_key_columns f,
		                                                    sys.columns cParent,
		                                                    sys.tables tParent
		                                                    where f.parent_object_id = c.object_id
		                                                    and f.parent_column_id = c.column_id
		                                                    and f.referenced_object_id = cParent.object_id
		                                                    and f.referenced_column_id = cParent.column_id
		                                                    and f.referenced_object_id = tParent.object_id
		                                                    and c.object_id = t.object_id
		                                                    and t.name = @TableName
								                            and cParent.name = @parentColName";

                        SqlParameter[] selectParameters = new SqlParameter[2];
                        selectParameters[0] = new SqlParameter("@tableName", childTable.Name);
                        selectParameters[1] = new SqlParameter("@parentColName", pkColName);

                        childColumn = ExecuteScalar(childColumnQuery, selectParameters, SQLTrx);
                    }

                    object childPKCol = getPKColName(childTable.Name);

                    DataTable dtChild = dataAccessHandler.executeSelectQuery(@"select " + childPKCol.ToString() +
                                                                    " from " + childTable.Name +
                                                                    " where " + childColumn + " = " + masterEntityId.ToString(), new SqlParameter[0], SQLTrx);

                    foreach (DataRow dr in dtChild.Rows)
                    {
                        UpdateSiteMasterEntity(childTable.Name, Convert.ToInt32(dr[0]), siteId, SQLTrx);
                        Publish(dr[0], siteId, childTable, SQLTrx,userId);
                    }
                }
            }
            log.Debug("Ends-Publish() method");
        }
        /// <summary>
        /// objectTransaltions uses elementGuid - guids of parents instead of foreign keys. hence processing is done differently
        /// </summary>
        /// <param name="masterEntityId"></param>
        /// <param name="SiteId"></param>
        /// <param name="MasterEntity"></param>
        /// <param name="pkColName"></param>
        /// <param name="SQLTrx"></param>
        void publishObjectTranslations(object masterEntityId, int SiteId, Entity MasterEntity, object pkColName, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-publishObjectTranslations() method");
            string dtChildQuery = @"select Id " +
                                                        " from ObjectTranslations" +
                                                        " where Object = '" + MasterEntity.Name + "'" +
                                                        @" and ElementGuid = (select Guid 
                                                                                from " + MasterEntity.Name +
                                                                               " where " + pkColName.ToString() + " = " + masterEntityId.ToString() + ")";


            DataTable dtChild = dataAccessHandler.executeSelectQuery(dtChildQuery, new SqlParameter[0], SQLTrx);

            if (dtChild.Rows.Count == 0)
                return;

            string tableName = "ObjectTranslations";

            string sqlQuery = @"select c.name column_name
		                        from sys.tables t,
		                        sys.columns c 
		                        where c.object_id = t.object_id
		                        and c.is_identity != 1
                                and c.name != 'guid'
		                        and t.name = @TableName";

            SqlParameter[] selectParams = new SqlParameter[1];
            selectParams[0] = new SqlParameter("@TableName", tableName);

            DataTable dt = dataAccessHandler.executeSelectQuery(sqlQuery, selectParams, SQLTrx);

            string column_name;
            string columnList, fromClause;

            columnList = "update tbl set ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                column_name = dt.Rows[i]["column_name"].ToString();

                if (column_name.Equals("site_id", StringComparison.CurrentCultureIgnoreCase))
                    continue;

                if (column_name.Equals("LanguageId", StringComparison.CurrentCultureIgnoreCase))
                {
                    columnList = columnList + "tbl." + column_name + " = (select top 1 lSite.LanguageId from Languages lSite where lSite.site_id = @site_id and lSite.MasterEntityId = src.LanguageId), ";
                }
                if (!column_name.Equals("ElementGuid", StringComparison.CurrentCultureIgnoreCase) && !column_name.Equals("LanguageId", StringComparison.CurrentCultureIgnoreCase))
                {
                    columnList = columnList + "tbl." + column_name + " = src." + column_name + ", ";
                }
            }
            columnList = columnList.TrimEnd(',', ' ');

            fromClause = "from " + tableName + " as tbl, " + tableName + " src where src.MasterEntityId = tbl.MasterEntityId and src.Id = @pkId and tbl.site_id = @site_id";

            string updateQuery = columnList + ' ' + fromClause;

            columnList = "insert into " + tableName + "(";
            fromClause = "from " + tableName + " src where src.Id = @pkId";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                column_name = dt.Rows[i]["column_name"].ToString();
                columnList = columnList + column_name + ", ";
            }
            columnList = columnList.TrimEnd(',', ' ');
            columnList += ") select ";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                column_name = dt.Rows[i]["column_name"].ToString();

                if (column_name.Equals("site_id", StringComparison.CurrentCultureIgnoreCase))
                {
                    columnList = columnList + "@site_id, ";
                }
                else if (column_name.Equals("LanguageId", StringComparison.CurrentCultureIgnoreCase))
                {
                    columnList = columnList + " (select top 1 lSite.LanguageId from Languages lSite where lSite.site_id = @site_id and lSite.MasterEntityId =  src.LanguageId ), ";
                }
                else if (column_name.Equals("ElementGuid", StringComparison.CurrentCultureIgnoreCase))
                {
                    columnList = columnList + "(select Guid from " + MasterEntity.Name + " where site_id = @site_id and MasterEntityId = @MasterEntityId)" + ", ";
                }
                else
                {
                    columnList = columnList + column_name + ", ";
                }
            }
            columnList = columnList.TrimEnd(',', ' ');

            string insertQuery = columnList + ' ' + fromClause;

            foreach (DataRow dr in dtChild.Rows)
            {
                updateMasterEntityId(tableName, "Id", dr["Id"], SQLTrx);

                string updateSqlQuery = updateQuery + "; select @@ROWCOUNT";


                int UpdatedRows = 0;

                try
                {
                    selectParams = new SqlParameter[3];
                    selectParams[0] = new SqlParameter("@pkId", dr["Id"]);
                    selectParams[1] = new SqlParameter("@site_id", SiteId);
                    selectParams[2] = new SqlParameter("@MasterEntityId", masterEntityId);
                    UpdatedRows = Convert.ToInt32(dataAccessHandler.executeUpdateQuery(updateSqlQuery, selectParams, SQLTrx));
                }
                catch (Exception ex)
                {
                    log.Debug("Ends-publishObjectTranslations() method");
                    throw new Exception(tableName + ":" + dr["Id"].ToString() + " - " + ex.Message + ": " + updateSqlQuery);
                }

                if (UpdatedRows == 0)
                {
                    updateSqlQuery = insertQuery + "; select @@ROWCOUNT";

                    int InsertedRows = 0;
                    try
                    {
                        selectParams = new SqlParameter[3];
                        selectParams[0] = new SqlParameter("@pkId", dr["Id"]);
                        selectParams[1] = new SqlParameter("@site_id", SiteId);
                        selectParams[2] = new SqlParameter("@MasterEntityId", masterEntityId);
                        InsertedRows = Convert.ToInt32(dataAccessHandler.executeInsertQuery(updateSqlQuery, selectParams, SQLTrx));
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Ends-publishObjectTranslations() method");
                        throw new Exception(tableName + ":" + dr["Id"].ToString() + " - " + ex.Message + ": " + updateSqlQuery);
                    }
                }
            }
            log.Debug("Ends-publishObjectTranslations() method");
        }

        /// <summary>
        /// Update master entityid
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pkColName"></param>
        /// <param name="PrimaryKeyId"></param>
        /// <param name="SQLTrx"></param>
        void updateMasterEntityId(string tableName, string pkColName, object PrimaryKeyId, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-updateMasterEntityId() method");

            string query = "update " + tableName + " set MasterEntityId = @PrimaryKeyId" +
                                @" where " + pkColName + " = @PrimaryKeyId" + " and isnull(MasterEntityId, -1) != @PrimaryKeyId";

            SqlParameter[] selectParameter = new SqlParameter[1];
            selectParameter[0] = new SqlParameter("@PrimaryKeyId", PrimaryKeyId);

            dataAccessHandler.executeUpdateQuery(query, selectParameter, SQLTrx);
            log.Debug("Ends-updateMasterEntityId() method");
        }

        /// <summary>
        /// Publish the depedent records
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pkColName"></param>
        /// <param name="pkId"></param>
        /// <param name="SiteId"></param>
        /// <param name="SQLTrx"></param>
        void publishRecord(string tableName, string pkColName, object pkId, int SiteId, SqlTransaction SQLTrx, string userId = null)
        {
            log.Debug("Starts-publishRecord() method");
            updateMasterEntityId(tableName, pkColName, pkId, SQLTrx);

            string sqlQuery = @"select c.name column_name, tParent.name parent_table, cParent.name parent_column
		                        from sys.tables t,
		                        sys.columns c left outer join
		                        sys.foreign_key_columns f 
		                        on f.parent_object_id = c.object_id
		                        and f.parent_column_id = c.column_id
		                        left outer join sys.columns cParent
		                        on f.referenced_object_id = cParent.object_id
		                        and f.referenced_column_id = cParent.column_id
		                        left outer join sys.tables tParent
		                        on  f.referenced_object_id = tParent.object_id
		                        where c.object_id = t.object_id
		                        and c.is_identity != 1
                                and c.name != 'guid'
		                        and t.name = @TableName";

            SqlParameter[] selectParameter = new SqlParameter[1];
            selectParameter[0] = new SqlParameter("@TableName", tableName);

            string column_name, parent_table, parent_column;
            string columnList, fromClause;
            string query;

            DataTable dt = dataAccessHandler.executeSelectQuery(sqlQuery, selectParameter, SQLTrx);

            columnList = "update tbl set ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                column_name = dt.Rows[i]["column_name"].ToString();
                parent_table = dt.Rows[i]["parent_table"].ToString();
                parent_column = dt.Rows[i]["parent_column"].ToString();

                if (column_name.Equals("site_id", StringComparison.CurrentCultureIgnoreCase))
                    continue;

                if (parent_table == "")
                {
                    columnList = columnList + "tbl." + column_name + " = src." + column_name + ", ";
                }
                else
                {
                    string fkValueQuery = @"select " + column_name + " from " + tableName + " where " + pkColName + " = @pkId";

                    selectParameter = new SqlParameter[1];
                    selectParameter[0] = new SqlParameter("@pkId", pkId);

                    object fkValue = ExecuteScalar(fkValueQuery, selectParameter, SQLTrx);

                    if (fkValue != DBNull.Value)
                    {
                        // if (parent_table.Equals("CardType", StringComparison.CurrentCultureIgnoreCase))
                        if (parent_table.Equals("Membership", StringComparison.CurrentCultureIgnoreCase))
                        {
                            columnList = columnList + "tbl." + column_name + " = " + fkValue.ToString() + ", ";
                        }
                        else
                        {
                            string fkValueQuery2 = "select top 1 1 from "
                                                     + parent_table +
                                                     " where MasterEntityId = " + fkValue.ToString()
                                                     + " and site_id = @site_id";

                            selectParameter = new SqlParameter[1];
                            selectParameter[0] = new SqlParameter("@site_id", SiteId);

                            if (column_name.Equals("CustomDataSetId", StringComparison.CurrentCultureIgnoreCase)
                                && !tableName.Equals("CustomData", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Entity customDataSet = new Entity("CustomDataSet");
                                Entity customData = new Entity("CustomData");
                                customDataSet.ChildList.Add(customData);

                                Publish(fkValue, SiteId, customDataSet, SQLTrx,userId);
                            }
                            else if (ExecuteScalar(fkValueQuery2, selectParameter, SQLTrx) == null)
                            {
                                UpdateSiteMasterEntity(parent_table, Convert.ToInt32(fkValue), SiteId, SQLTrx); // Update master entityId in site
                                publishRecord(parent_table, parent_column, fkValue, SiteId, SQLTrx, userId);
                            }

                            columnList = columnList + "tbl." + column_name + " = (select top 1 fk." + parent_column + " from " + parent_table + " fk where fk.MasterEntityId = src." + column_name + " and fk.site_id = @site_id), ";
                        }
                    }
                    else
                        columnList = columnList + "tbl." + column_name + " = null, ";
                }
            }
            columnList = columnList.TrimEnd(',', ' ');

            fromClause = "from " + tableName + " as tbl, " + tableName + " src where src.MasterEntityId = tbl.MasterEntityId and src." + pkColName + " = @pkId and tbl.site_id = @site_id";

            query = columnList + ' ' + fromClause;

            string updtQuery = query + "; select @@ROWCOUNT";
            selectParameter = new SqlParameter[2];
            selectParameter[0] = new SqlParameter("@pkId", pkId);
            selectParameter[1] = new SqlParameter("@site_id", SiteId);

            int UpdatedRows = 0;
            try
            {
                UpdatedRows = Convert.ToInt32(dataAccessHandler.executeUpdateQuery(updtQuery, selectParameter, SQLTrx));
                string queryToGetGuid = "select Guid  from " + tableName + " where site_id = @site_id and MasterEntityId = @pkId";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@tableName", tableName));
                parameters.Add(new SqlParameter("@site_id", SiteId));
                parameters.Add(new SqlParameter("@pkId", pkId));
                DataTable dataTable = dataAccessHandler.executeSelectQuery(queryToGetGuid, parameters.ToArray(), SQLTrx);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow dataRow = dataTable.Rows[0];
                    string guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                    InsertIntoDBAuditLog(tableName, guid, SiteId, userId, SQLTrx);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Ends-publishRecord() method");
                throw new Exception(tableName + ":" + pkId.ToString() + " - " + ex.Message + ": " + updtQuery);
            }

            if (UpdatedRows == 0)
            {
                columnList = "insert into " + tableName + "(";
                fromClause = "from " + tableName + " src where src." + pkColName + " = @pkId";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    column_name = dt.Rows[i]["column_name"].ToString();
                    columnList = columnList + column_name + ", ";
                }
                columnList = columnList.TrimEnd(',', ' ');
                columnList += ") select ";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    column_name = dt.Rows[i]["column_name"].ToString();
                    parent_table = dt.Rows[i]["parent_table"].ToString();
                    parent_column = dt.Rows[i]["parent_column"].ToString();

                    if (column_name.Equals("site_id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        columnList = columnList + "@site_id, ";
                    }
                    else if (parent_table == "")
                    {
                        columnList = columnList + column_name + ", ";
                    }
                    else
                    {
                        string fkValueQuery = @"select " + column_name + " from " + tableName + " where " + pkColName + " = @pkId";

                        selectParameter = new SqlParameter[1];
                        selectParameter[0] = new SqlParameter("@pkId", pkId);

                        object fkValue = ExecuteScalar(fkValueQuery, selectParameter, SQLTrx);
                        if (fkValue != DBNull.Value)
                        {
                            // cardType is not duplicated. all refer to HQ cardType records
                            // if (parent_table.Equals("CardType", StringComparison.CurrentCultureIgnoreCase))
                            if (parent_table.Equals("Membership", StringComparison.CurrentCultureIgnoreCase))
                            {
                                columnList = columnList + fkValue.ToString() + ", ";
                            }
                            else
                            {
                                string fkValueQuery2 = "select top 1 1 from "
                                                   + parent_table +
                                                   " where MasterEntityId = " + fkValue.ToString()
                                                   + " and site_id = @site_id";

                                selectParameter = new SqlParameter[1];
                                selectParameter[0] = new SqlParameter("@site_id", SiteId);

                                // if customData is enabled for a table, publish custom data
                                if (column_name.Equals("CustomDataSetId", StringComparison.CurrentCultureIgnoreCase)
                                    && !tableName.Equals("CustomData", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Entity customDataSet = new Entity("CustomDataSet");
                                    Entity customData = new Entity("CustomData");
                                    customDataSet.ChildList.Add(customData);

                                    Publish(fkValue, SiteId, customDataSet, SQLTrx,userId);
                                }
                                else if (ExecuteScalar(fkValueQuery2, selectParameter, SQLTrx) == null)
                                {
                                    //check and update master enityId in site
                                    UpdateSiteMasterEntity(parent_table, Convert.ToInt32(fkValue), SiteId, SQLTrx);
                                    // parent not yet published to site
                                    publishRecord(parent_table, parent_column, fkValue, SiteId, SQLTrx, userId);
                                }

                                columnList = columnList + " (select top 1 fk." + parent_column + " from " + parent_table + " fk where fk.MasterEntityId = src." + column_name + " and fk.site_id = @site_id), ";
                            }
                        }
                        else
                            columnList = columnList + " null, ";
                    }
                }
                columnList = columnList.TrimEnd(',', ' ');

                query = columnList + ' ' + fromClause;
                sqlQuery = query + "; select @@ROWCOUNT";

                selectParameter = new SqlParameter[2];
                selectParameter[0] = new SqlParameter("@pkId", pkId);
                selectParameter[1] = new SqlParameter("@site_id", SiteId);

                int InsertedRows = 0;
                try
                {
                    InsertedRows = Convert.ToInt32(dataAccessHandler.executeInsertQuery(sqlQuery, selectParameter, SQLTrx));

                    string queryToGetGuid = "select Guid  from " + tableName + " where site_id = @site_id and MasterEntityId = @pkId";
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@tableName", tableName));
                    parameters.Add(new SqlParameter("@site_id", SiteId));
                    parameters.Add(new SqlParameter("@pkId", pkId));
                    DataTable dataTable = dataAccessHandler.executeSelectQuery(queryToGetGuid, parameters.ToArray(), SQLTrx);
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow dataRow = dataTable.Rows[0];
                        parameters = new List<SqlParameter>();
                        string guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                        InsertIntoDBAuditLog(tableName, guid, SiteId, userId, SQLTrx);
                    }


                }
                catch (Exception ex)
                {
                    log.Debug("Ends-publishRecord() method");
                    throw new Exception(tableName + ":" + pkId.ToString() + " - " + ex.Message + ": " + sqlQuery);
                }
            }
            log.Debug("Ends-publishRecord() method");
        }

        private void InsertIntoDBAuditLog(string tableName , string guid ,int siteId , string userName, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry();
            
            string insertQuery = @"Insert into DBAuditLog (Type,TableName,RecordID, FieldName,OldValue,NewValue,DateOfLog,UserName,site_id,LastUpdatedBy,LastUpdateDate) 
                                                      select case when d.RecordId is null then 'I' else 'U' end , @tableName ,  @guid," +
                                        " null, null, null, getdate() , @userName ,@site_id, @userName, getdate() from " + tableName + " i  " +
                                        "left outer join (select * from (select top 1 RecordID from DBAuditLog " +
                                        "where TableName = @tableName and RecordId = Convert(varchar(100), @guid) " +
                                        "order by DateOfLog desc) v1) d" +
                                        " on  i.Guid = d.RecordId " +
                                        "where i.Guid = @guid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@tableName", tableName));
            parameters.Add(new SqlParameter("@guid", guid));
            parameters.Add(new SqlParameter("@userName", userName));
            parameters.Add(new SqlParameter("@site_id", siteId));
            dataAccessHandler.executeScalar(insertQuery, parameters.ToArray(), SQLTrx);
            log.LogMethodExit();
        }
        /// <summary>
        /// Added to update masterentityId in site based on the column comparison
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="_masterEntityId"></param>
        /// <param name="siteId"></param>
        /// <param name="SQLTrx"></param>
        void UpdateSiteMasterEntity(string entityName, int _masterEntityId, int siteId, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-UpdateSiteMasterEntity() method");
            string value;
            if (lstDependentColumns.TryGetValue(entityName, out value))
            {
                object pkColumn = getPKColName(entityName);

                string[] var = value.Split(',');
                string query = " SELECT TOP 1 P2." + pkColumn + " FROM " + entityName + " P1, " + entityName + " P2  WHERE P2.MasterEntityId IS NULL AND P1." + pkColumn + " = " + _masterEntityId +
                                            " AND P2.site_id = " + siteId;

                foreach (string d in var)
                {
                    query += " AND " + " P1." + d + " = P2." + d;
                }

                object exists = ExecuteScalar(query, new SqlParameter[0], SQLTrx);

                if (exists != null)
                {
                    UpdateSiteEntity(entityName, Convert.ToInt32(exists), _masterEntityId, SQLTrx);
                }
            }
            log.Debug("Ends-UpdateSiteMasterEntity() method");
        }

        /// <summary>
        /// Method to delete GameProfileAttributeValues and update GameProfileAttributes in site
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="pkId"></param>
        /// <param name="selectedSiteId"></param>
        /// <param name="SQLTrx"></param>
        public void UpdateGameProfileAttributes(string columnName, int masterEntityId, int pkId, int selectedSiteId, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-UpdateGameProfileAttributes() method");
            string selectQuery = @"SELECT id, AttributeId 
                                           FROM GameProfileAttributeValues
                                                        WHERE " + columnName + "=@masterEntityId";

            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@masterEntityId", masterEntityId);

            //Get Publishing Records
            DataTable dt = dataAccessHandler.executeSelectQuery(selectQuery, sqlParameters, SQLTrx);

            //Delete the records from GameProfileAttributeValues
            DeleteSiteEntityRecords("GameProfileAttributeValues", columnName, pkId, SQLTrx);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow rw in dt.Rows)
                {
                    string query = @"SELECT AttributeId FROM GameProfileAttributes WHERE site_id = @selectedSiteId and Attribute = 
                                            (SELECT top 1 Attribute from GameProfileAttributes where AttributeId = @attributeId) AND MasterEntityId IS NULL";

                    SqlParameter[] sqlParams = new SqlParameter[2];
                    sqlParams[0] = new SqlParameter("@selectedSiteId", selectedSiteId);
                    sqlParams[1] = new SqlParameter("@attributeId", rw[1]);

                    DataTable attributeDT = dataAccessHandler.executeSelectQuery(query, sqlParams, SQLTrx);

                    if (attributeDT != null && attributeDT.Rows.Count > 0 && !attributeDT.Rows[0][0].Equals(DBNull.Value))
                    {
                        dataAccessHandler.executeUpdateQuery("UPDATE GameProfileAttributes SET MasterEntityId = "
                                        + rw[1] + " WHERE AttributeId =" + attributeDT.Rows[0][0], new SqlParameter[0], SQLTrx);
                    }
                }
            }
            log.Debug("Ends-UpdateGameProfileAttributes() method");
        }

        /// <summary>
        /// Deletes the records based on the passed parameter
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columnName"></param>
        /// <param name="pkId"></param>
        /// <param name="SQLTrx"></param>
        public void DeleteSiteEntityRecords(string entityName, string columnName, int pkId, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-DeleteSiteEntityRecords(string entityName, string columnName, int pkId, SqlTransaction SQLTrx) Handler method");
            string query = @"DELETE FROM " + entityName +
                                " WHERE " + columnName + " = @pkId";

            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@pkId", pkId);

            dataAccessHandler.executeUpdateQuery(query, selectParameters, SQLTrx);
            log.Debug("Ends-DeleteSiteEntityRecords(string entityName, string columnName, int pkId, SqlTransaction SQLTrx) Handler method");
        }

        /// <summary>
        /// GetCardTypeRule method
        /// </summary>
        /// <param name="membershipId"></param>
        /// <param name="siteId"></param>
        /// <param name="HqSiteId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public DataTable GetCardTypeRule(int membershipId, int siteId, int HqSiteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-GetCardTypeRule(string entityName, string columnName, int pkId, SqlTransaction SQLTrx) Handler method");
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@membershipId", membershipId);
            sqlParams[1] = new SqlParameter("@site_id", HqSiteId);
            string query = "select ID from CardTypeRule where MembershipId = @membershipId and site_id = @site_id";

            DataTable dt = dataAccessHandler.executeSelectQuery(query, sqlParams, SQLTrx);

            if (dt != null && dt.Rows.Count > 0)
            {
                log.Debug("Ends-GetCardTypeRule() method");
                return dt;
            }
            else
            {
                log.Debug("Ends-GetCardTypeRule() method");
                return new DataTable();
            }
        }

        /// <summary>
        /// GetMembershipRewards method
        /// </summary>
        /// <param name="membershipId"></param>
        /// <param name="siteId"></param>
        /// <param name="HqSiteId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public DataTable GetMembershipRewards(int membershipId, int siteId, int HqSiteId, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(membershipId, siteId, HqSiteId, sqlTrx);
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@membershipId", membershipId);
            sqlParams[1] = new SqlParameter("@site_id", HqSiteId);
            string query = "select MembershipRewardsId from MembershipRewards where MembershipId = @membershipId and site_id = @site_id";

            DataTable dt = dataAccessHandler.executeSelectQuery(query, sqlParams, sqlTrx);

            if (dt != null && dt.Rows.Count > 0)
            {
                log.LogMethodExit();
                return dt;
            }
            else
            {
                log.LogMethodExit();
                return new DataTable();
            }
        }

        ///// <summary>
        ///// Returns product_game_id
        ///// </summary>
        ///// <param name="cardTypeId"></param>
        ///// <param name="siteId"></param>
        ///// <param name="HqSiteId"></param>
        ///// <param name="SQLTrx"></param>
        ///// <returns></returns>
        //public DataTable GetProductGame(int cardTypeId, int siteId, int HqSiteId, SqlTransaction SQLTrx = null)
        //{
        //    log.Debug("Starts-GetProductGame() method");
        //    SqlParameter[] sqlParams = new SqlParameter[2];
        //    sqlParams[0] = new SqlParameter("@cardTypeId", cardTypeId);
        //    sqlParams[1] = new SqlParameter("@site_id", HqSiteId);
        //    string query = "select product_game_id from ProductGames where CardTypeId = @cardTypeId and site_id = @site_id"; 

        //    DataTable dt = dataAccessHandler.executeSelectQuery(query, sqlParams, SQLTrx);

        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        log.Debug("Ends-GetProductGame() method");
        //        return dt;
        //    }
        //    else
        //    {
        //        log.Debug("Ends-GetProductGame() method");
        //        return new DataTable();
        //    }
        //}

        /// <summary>
        /// Retuns datatype of the passed Column
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        object GetDataType(string entity, string columnName)
        {
            log.Debug("Starts-GetDataType() Handler method");
            string query = @"SELECT DATA_TYPE 
                                FROM INFORMATION_SCHEMA.COLUMNS 
                                WHERE  TABLE_NAME = @entity
                                AND COLUMN_NAME = @columnName";

            SqlParameter[] selectParams = new SqlParameter[2];
            selectParams[0] = new SqlParameter("@entity", entity);
            selectParams[1] = new SqlParameter("@columnName", columnName);

            DataTable dataDT = dataAccessHandler.executeSelectQuery(query, selectParams);

            if (dataDT != null && dataDT.Rows.Count > 0)
            {
                return dataDT.Rows[0][0];
            }

            log.Debug("Ends-GetDataType() Handler method");
            return null;
        }
    }
}
