using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Publish
{
    public class ObjectTranslationTable : Table
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public ObjectTranslationTable(ExecutionContext executionContext) :
            base(executionContext, "ObjectTranslations")
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public override void AddChild(Table childTable)
        {
            log.LogMethodEntry(childTable);
            AddChild(childTable, string.Empty);
            log.LogMethodExit();
        }

        public override void AddChild(Table childTable, string foreignKeyColumnName)
        {
            log.LogMethodEntry(childTable, foreignKeyColumnName);
            log.LogMethodExit(null, "Invalid operation - ObjectTranslations doesn't have a child table");
            throw new InvalidOperationException("Invalid operation - ObjectTranslations doesn't have a child table");
        }

        public override string GetParentPrimaryKeyListQuery(ForeignKeyColumn foreignKeyColumn)
        {
            log.LogMethodEntry(foreignKeyColumn);
            string query = @"SELECT DISTINCT CAST(ObjectTranslations.LanguageId AS int) AS Id
                             FROM ObjectTranslations, @pkIdList pkl
                             WHERE ObjectTranslations.Id = pkl.Id";
            log.LogMethodExit(query);
            return query;
        }

        public override string GetPublishQuery(bool forcePublish, bool referredEntity, bool enableAuditLog)
        {
            log.LogMethodEntry(forcePublish);
            StringBuilder sb = new StringBuilder(enableAuditLog ? @"DECLARE @Output as StringType;
                                                   MERGE INTO " : "MERGE INTO ");
            string query = @"ObjectTranslations tbl 
                            USING(SELECT(SELECT TOP 1 ref.LanguageId FROM Languages ref WHERE ref.MasterEntityId = ObjectTranslations.LanguageId AND ref.site_id = sl.id) LanguageId,
                            CASE WHEN ObjectTranslations.Object = 'PRODUCTS' THEN(SELECT TOP 1 ref.guid FROM products ref WHERE ref.MasterEntityId = (SELECT product_id from Products Where guid = ObjectTranslations.ElementGuid) AND ref.site_id = sl.id) END ElementGuid, ObjectTranslations.Object, ObjectTranslations.Element, ObjectTranslations.Translation, ObjectTranslations.LastUpdatedBy, ObjectTranslations.LastUpdatedDate, sl.Id as publishSite_id, ObjectTranslations.MasterEntityId, ObjectTranslations.CreatedBy, ObjectTranslations.CreationDate
                            FROM ObjectTranslations, @pkIdList pkl, @siteIdList sl
                            WHERE ObjectTranslations.Id = pkl.Id) AS src
                            ON src.MasterEntityId = tbl.MasterEntityId AND tbl.site_id = src.publishSite_id ";
            if(referredEntity == false)
            {
                query += " WHEN MATCHED ";
                if (forcePublish == false)
                {
                    query +=
                            @"
                        AND
                        (EXISTS(SELECT tbl.LanguageId EXCEPT SELECT src.LanguageId)
                        OR  EXISTS(SELECT tbl.ElementGuid EXCEPT SELECT src.ElementGuid)
                        OR  EXISTS(SELECT tbl.Object EXCEPT SELECT src.Object)
                        OR  EXISTS(SELECT tbl.Element EXCEPT SELECT src.Element)
                        OR  EXISTS(SELECT tbl.Translation EXCEPT SELECT src.Translation)
                        OR  EXISTS(SELECT tbl.LastUpdatedBy EXCEPT SELECT src.LastUpdatedBy)
                        OR  EXISTS(SELECT tbl.LastUpdatedDate EXCEPT SELECT src.LastUpdatedDate)
                        OR  EXISTS(SELECT tbl.CreatedBy EXCEPT SELECT src.CreatedBy)
                        OR  EXISTS(SELECT tbl.CreationDate EXCEPT SELECT src.CreationDate)
                    )";
                }
                query +=
                      @"
                    THEN UPDATE SET
                    LanguageId = src.LanguageId
                    , ElementGuid = src.ElementGuid
                    , Object = src.Object
                    , Element = src.Element
                    , Translation = src.Translation
                    , LastUpdatedBy = src.LastUpdatedBy
                    , LastUpdatedDate = src.LastUpdatedDate
                    , CreatedBy = src.CreatedBy
                    , CreationDate = src.CreationDate";
            }

            query += @" WHEN NOT MATCHED THEN insert(
                    LanguageId
                    , ElementGuid
                    , Object
                    , Element
                    , Translation
                    , LastUpdatedBy
                    , LastUpdatedDate
                    , site_id
                    , MasterEntityId
                    , CreatedBy
                    , CreationDate
                    ) VALUES(
                    src.LanguageId
                    , src.ElementGuid
                    , src.Object
                    , src.Element
                    , src.Translation
                    , src.LastUpdatedBy
                    , src.LastUpdatedDate
                    , src.publishSite_id
                    , src.MasterEntityId
                    , src.CreatedBy
                    , src.CreationDate
                    ) ";
            sb.Append(query);
            if (enableAuditLog)
            {
                sb.Append(Environment.NewLine);
                sb.Append(GetOutputQuery());
            }
            sb.Append(";");
            if (enableAuditLog)
            {
                sb.Append(Environment.NewLine);
                sb.Append(GetDBAuditQuery());
            }
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        public override string GetChildPrimaryKeyListQuery(TableReference tableReference)
        {
            log.LogMethodEntry(tableReference);
            string result = string.Empty;
            if(tableReference.ParentTable.Name.ToLower() == "products")
            {
                result = @"select ObjectTranslations.Id
                            from ObjectTranslations, products, @pkIdList pkl
                            WHERE  ObjectTranslations.ElementGuid = products.Guid
                            AND pkl.Id = products.product_id";
            }
            else
            {
                string message = "Object translation for " + tableReference.ParentTable.Name + " is not implemented";
                log.LogMethodExit(null, message);
                throw new NotImplementedException(message);
            }
            log.LogMethodExit(result);
            return result;
        }

        public override List<ForeignKeyColumn> GetReferencingColumns(Table parentTable)
        {
            return new List<ForeignKeyColumn>();
        }
        public override void Traverse(HashSet<int> primaryKeyIdHashSet, List<string> input, string indent, int depth, string skipForeignKeyColumn)
        {
            input.Add(indent + Name);
        }
    }
}
