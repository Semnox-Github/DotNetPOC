using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Publish
{
    public class Table
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string name;
        private readonly List<Column> columns;
        private PrimaryKeyColumn primaryKeyColumn;
        private List<Column> uniqueColumns;
        private readonly List<TableReference> children;
        private ExecutionContext executionContext;

        public Table(ExecutionContext executionContext, string name, string commaSeparatedUniqueColumnNameList = null)
        {
            log.LogMethodEntry(executionContext, name, commaSeparatedUniqueColumnNameList);
            this.name = name;
            children = new List<TableReference>();
            this.executionContext = executionContext;
            BatchPublishDataHandler batchPublishDataHandler = new BatchPublishDataHandler();
            columns = batchPublishDataHandler.GetColumns(name);
            if (columns == null || columns.Any() == false)
            {
                throw new Exception("Invalid Table :" + name);
            }
            AssignPrimaryKeyColumn();
            AssignUniqueKeyColumns(commaSeparatedUniqueColumnNameList);
            log.LogMethodExit();
        }

        private void AssignUniqueKeyColumns(string commaSeparatedUniqueColumnNameList)
        {
            log.LogMethodEntry(commaSeparatedUniqueColumnNameList);
            uniqueColumns = new List<Column>();
            if(string.IsNullOrWhiteSpace(commaSeparatedUniqueColumnNameList))
            {
                log.LogMethodExit(null, "commaSeparatedUniqueColumnNameList is empty");
                return;
            }
            string[] uniqueColumnNameList = commaSeparatedUniqueColumnNameList.Replace(" ", string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> uniqueColumnNameHashSet = null;
            if (uniqueColumnNameList.Any())
            {
                uniqueColumnNameHashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                uniqueColumnNameHashSet.UnionWith(uniqueColumnNameList);
            }
            foreach (Column column in columns)
            {
                if (uniqueColumnNameHashSet != null && 
                    uniqueColumnNameHashSet.Contains(column.Name))
                {
                    uniqueColumns.Add(column);
                }
            }
            log.LogMethodExit();
        }

        public virtual void AddChild(Table childTable)
        {
            log.LogMethodEntry(childTable);
            children.Add(new TableReference(this, childTable));
            log.LogMethodExit();
        }

        public virtual void AddChild(Table childTable, string foreignKeyColumnName)
        {
            log.LogMethodEntry(childTable, foreignKeyColumnName);
            children.Add(new TableReference(this, childTable, foreignKeyColumnName));
            log.LogMethodExit();
        }

        private void AssignPrimaryKeyColumn()
        {
            log.LogMethodEntry();
            foreach (Column column in columns)
            {
                if (column is PrimaryKeyColumn)
                {
                    primaryKeyColumn = column as PrimaryKeyColumn;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Query like
        /// MERGE INTO Lookups tbl 
        /// USING(SELECT Lookups.LookupName, Lookups.Protected, sl.Id as publishSite_id, Lookups.MasterEntityId, Lookups.CreatedBy, Lookups.CreationDate, Lookups.LastUpdatedBy, Lookups.LastUpdateDate
        ///       FROM Lookups, @pkIdList pkl, @siteIdList sl
        ///       WHERE Lookups.LookupId = pkl.Id) AS src
        /// ON src.MasterEntityId = tbl.MasterEntityId AND tbl.site_id = src.publishSite_id
        /// WHEN MATCHED
        /// THEN UPDATE SET
        /// LookupName = src.LookupName
        /// , Protected = src.Protected
        /// , CreatedBy = src.CreatedBy
        /// , CreationDate = src.CreationDate
        /// , LastUpdatedBy = src.LastUpdatedBy
        /// , LastUpdateDate = src.LastUpdateDate
        ///
        ///     WHEN NOT MATCHED THEN insert (
        ///     LookupName
        ///     , Protected
        ///     , site_id
        ///     , MasterEntityId
        ///     , CreatedBy
        ///     , CreationDate
        ///     , LastUpdatedBy
        ///     , LastUpdateDate
        /// ) VALUES (
        ///     src.LookupName
        ///     , src.Protected
        ///    , src.publishSite_id
        ///    , src.MasterEntityId
        ///     , src.CreatedBy
        ///    , src.CreationDate
        ///     , src.LastUpdatedBy
        ///     , src.LastUpdateDate
        /// );
        
        /// </summary>
        /// <param name="forcePublish"></param>
        /// <returns></returns>
        public virtual string GetPublishQuery(bool forcePublish, bool referredEntity, bool enableAuditLog)
        {
            log.LogMethodEntry();
            StringBuilder sb = new StringBuilder(enableAuditLog ? @"DECLARE @Output as StringType;
                                                   MERGE INTO " : "MERGE INTO ");
            sb.Append(name);
            sb.Append(" tbl ");
            sb.Append(Environment.NewLine);
            sb.Append("USING (");
            sb.Append(GetSourceTableQuery());
            sb.Append(") AS src");
            sb.Append(Environment.NewLine);
            sb.Append("ON src.MasterEntityId = tbl.MasterEntityId AND tbl.site_id = src.publishSite_id");
            sb.Append(Environment.NewLine);
            if(referredEntity == false)
            {
                sb.Append("WHEN MATCHED");
                sb.Append(Environment.NewLine);
                if (forcePublish == false)
                {
                    sb.Append(GetInEqualQuery());
                }
                sb.Append(Environment.NewLine);
                sb.Append("THEN UPDATE SET ");
                sb.Append(Environment.NewLine);
                sb.Append(GetUpdateQuery());
            }
            sb.Append(Environment.NewLine);
            sb.Append("WHEN NOT MATCHED THEN insert (");
            sb.Append(GetInsertDefinitionQuery());
            sb.Append(Environment.NewLine);
            sb.Append(") VALUES ( ");
            sb.Append(GetInsertQuery());
            sb.Append(Environment.NewLine);
            sb.Append(")");
            if(enableAuditLog)
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

        protected string GetOutputQuery()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return @"OUTPUT
                    inserted.Guid
                    INTO @Output(value);";
        }

        protected string GetDBAuditQuery()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return @" exec AuditDBTableBatch '" + name + "', '" + executionContext.GetUserId() + "', @output ";
        }

        private string GetInsertQuery()
        {
            StringBuilder sb = new StringBuilder();
            string joiner = string.Empty;
            foreach (Column column in columns)
            {
                string insertQuery = column.GetInsertQuery();
                if (string.IsNullOrWhiteSpace(insertQuery) == false)
                {
                    sb.Append(Environment.NewLine);
                    sb.Append(joiner);
                    sb.Append(insertQuery);
                    joiner = ", ";
                }
            }
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        private string GetInsertDefinitionQuery()
        {
            StringBuilder sb = new StringBuilder();
            string joiner = string.Empty;
            foreach (Column column in columns)
            {
                string insertDefinitionQuery = column.GetInsertDefinitionQuery();
                if (string.IsNullOrWhiteSpace(insertDefinitionQuery) == false)
                {
                    sb.Append(Environment.NewLine);
                    sb.Append(joiner);
                    sb.Append(insertDefinitionQuery);
                    joiner = ", ";
                }
            }
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        private string GetUpdateQuery()
        {
            StringBuilder sb = new StringBuilder();
            string joiner = string.Empty;
            foreach (Column column in columns)
            {
                string updateQuery = column.GetUpdateQuery();
                if (string.IsNullOrWhiteSpace(updateQuery) == false)
                {
                    sb.Append(joiner);
                    sb.Append(updateQuery);
                    sb.Append(Environment.NewLine);
                    joiner = ", ";
                }
            }
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;

        }

        private string GetInEqualQuery()
        {
            log.LogMethodEntry();
            StringBuilder sb = new StringBuilder(" AND ");
            sb.Append(Environment.NewLine);
            sb.Append(" (");
            string joiner = string.Empty;
            foreach (Column column in columns)
            {
                string inEqualQuery = column.GetInEqualQuery();
                if(string.IsNullOrWhiteSpace(inEqualQuery) == false)
                {
                    sb.Append(joiner);
                    sb.Append(inEqualQuery);
                    joiner = Environment.NewLine + " OR ";
                }
            }
            sb.Append(Environment.NewLine);
            sb.Append(")");
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        private string GetSourceTableQuery()
        {
            log.LogMethodEntry();
            StringBuilder sb = new StringBuilder(" SELECT ");
            string joiner = string.Empty;
            foreach (Column column in columns)
            {
                string columnSourceTableQuery = column.GetSourceTableQuery(name);
                if (string.IsNullOrWhiteSpace(columnSourceTableQuery) == false)
                {
                    sb.Append(joiner);
                    sb.Append(columnSourceTableQuery);
                    joiner = ", ";
                }
            }
            sb.Append(Environment.NewLine);
            sb.Append("FROM ");
            sb.Append(name);
            sb.Append(", @pkIdList pkl, @siteIdList sl");
            sb.Append(Environment.NewLine);
            sb.Append("WHERE ");
            sb.Append(name);
            sb.Append(".");
            sb.Append(primaryKeyColumn.Name);
            sb.Append(" = pkl.Id");
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        public virtual string GetParentPrimaryKeyListQuery(ForeignKeyColumn foreignKeyColumn)
        {
            log.LogMethodEntry(foreignKeyColumn);
            StringBuilder sb = new StringBuilder("SELECT DISTINCT ");
            sb.Append(name);
            sb.Append(".");
            sb.Append(foreignKeyColumn.Name);
            sb.Append(" AS Id");
            sb.Append(Environment.NewLine);
            sb.Append("FROM ");
            sb.Append(name);
            sb.Append(", @pkIdList pkl");
            sb.Append(Environment.NewLine);
            sb.Append("WHERE ");
            sb.Append(name);
            sb.Append(".");
            sb.Append(primaryKeyColumn.Name);
            sb.Append(" = pkl.Id");
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns query like
        /// UPDATE tbl 
        /// SET tbl.MasterEntityId = tbl.LookupId
        /// FROM Lookups tbl, @pkIdList pkl
        /// WHERE EXISTS (SELECT tbl.MasterEntityId EXCEPT SELECT tbl.LookupId)
        /// AND site_id = (SELECT TOP 1 Master_Site_Id FROM Company) 
        /// AND tbl.LookupId = pkl.Id
        /// </summary>
        /// <returns></returns>
        public virtual string GetMasterSiteUpdateMasterEntityIdQuery()
        {
            log.LogMethodEntry();
            StringBuilder sb = new StringBuilder("UPDATE tbl ");
            sb.Append(Environment.NewLine);
            sb.Append(" SET tbl.MasterEntityId = tbl.");
            sb.Append(primaryKeyColumn.Name);
            sb.Append(Environment.NewLine);
            sb.Append(" FROM ");
            sb.Append(name);
            sb.Append(" tbl , @pkIdList pkl");
            sb.Append(Environment.NewLine);
            sb.Append(" WHERE EXISTS (SELECT tbl.MasterEntityId  EXCEPT SELECT tbl.");
            sb.Append(primaryKeyColumn.Name);
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append(" AND site_id = (SELECT TOP 1 Master_Site_Id FROM Company) ");
            sb.Append(Environment.NewLine);
            sb.Append(" AND tbl.");
            sb.Append(primaryKeyColumn.Name);
            sb.Append(" = pkl.Id ");
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns query like
        /// UPDATE tbl
        /// SET tbl.MasterEntityId = src.LookupId
        /// FROM Lookups tbl, Lookups src, @pkIdList pkl, @siteIdList sl
        /// WHERE src.LookupId = pkl.Id
        /// AND tbl.site_id = sl.Id
        /// AND tbl.MasterEntityId IS NULL
        /// AND EXISTS(SELECT src.LookupName INTERSECT SELECT tbl.LookupName)
        /// </summary>
        /// <returns></returns>
        public virtual string GetUpdateMasterEntityIdUsingUniqueColumnsQuery()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            if(uniqueColumns.Any() == false)
            {
                log.LogMethodExit(result, "No unique columns for table " + name);
                return result;
            }
            StringBuilder sb = new StringBuilder("UPDATE tbl");
            sb.Append(Environment.NewLine);
            sb.Append("SET tbl.MasterEntityId = src.");
            sb.Append(primaryKeyColumn.Name);
            sb.Append(Environment.NewLine);
            sb.Append("FROM ");
            sb.Append(name);
            sb.Append(" tbl, ");
            sb.Append(name);
            sb.Append(" src, ");
            sb.Append("@pkIdList pkl, @siteIdList sl ");
            sb.Append(Environment.NewLine);
            sb.Append("WHERE src.");
            sb.Append(primaryKeyColumn.Name);
            sb.Append(" = pkl.Id");
            sb.Append(Environment.NewLine);
            sb.Append("AND tbl.site_id = sl.Id");
            sb.Append(Environment.NewLine);
            sb.Append("AND tbl.MasterEntityId IS NULL");
            
            foreach (Column uniqueColumn in uniqueColumns)
            {
                sb.Append(Environment.NewLine);
                sb.Append(uniqueColumn.GetUpdateMasterEntityIdUsingUniqueColumnQuery());
            }
            result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns query like
        /// SELECT LookupValues.LookupValueId AS Id
        /// FROM LookupValues, @pkIdList pkl
        /// WHERE LookupValues.LookupId = pkl.Id
        /// </summary>
        /// <param name="tableReference"></param>
        /// <returns></returns>
        public virtual string GetChildPrimaryKeyListQuery(TableReference tableReference)
        {
            log.LogMethodEntry();
            StringBuilder sb = new StringBuilder("SELECT ");
            sb.Append(name);
            sb.Append(".");
            sb.Append(primaryKeyColumn.Name);
            sb.Append(" AS Id");
            sb.Append(Environment.NewLine);
            sb.Append("FROM ");
            sb.Append(name);
            sb.Append(", @pkIdList pkl");
            sb.Append(Environment.NewLine);
            sb.Append("WHERE ");
            sb.Append(name);
            sb.Append(".");
            sb.Append(tableReference.ForeignKeyColumn.Name);
            sb.Append(" = pkl.Id");
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns query like
        /// </summary>
        /// <returns></returns>
        public virtual string GetCreateMasterEntityIdAndSiteIdIndexQuery()
        {
            log.LogMethodEntry();
            StringBuilder sb = new StringBuilder("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_");
            sb.Append(name);
            sb.Append("_site_id_MasterEntityId_");
            sb.Append(primaryKeyColumn.Name);
            sb.Append("' and OBJECT_NAME(object_id) = '");
            sb.Append(name);
            sb.Append("')");
            sb.Append(Environment.NewLine);
            sb.Append("BEGIN");
            sb.Append(Environment.NewLine);
            sb.Append("CREATE NONCLUSTERED INDEX IX_");
            sb.Append(name);
            sb.Append("_site_id_MasterEntityId_");
            sb.Append(primaryKeyColumn.Name);
            sb.Append(Environment.NewLine);
            sb.Append("ON [dbo].[");
            sb.Append(name);
            sb.Append("] ([MasterEntityId],[site_id])");
            sb.Append(Environment.NewLine);
            sb.Append("END");
            sb.Append(Environment.NewLine);
            sb.Append("GO");
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        public virtual void BuildCreateIndexQuery(HashSet<int> primaryKeyIdHashSet, Dictionary<string, string> indexQueryMap, string ignoreForeignKeyColumn)
        {
            log.LogMethodEntry(primaryKeyIdHashSet, indexQueryMap, ignoreForeignKeyColumn);
            if(indexQueryMap.ContainsKey(name))
            {
                return;
            }
            indexQueryMap.Add(name, GetCreateMasterEntityIdAndSiteIdIndexQuery());
            BatchPublishDataHandler batchPublishDataHandler = new BatchPublishDataHandler();
            List<ForeignKeyColumn> foreignKeyColumns = GetForeignKeyColumns();
            foreach (var foreignKeyColumn in foreignKeyColumns)
            {
                if (foreignKeyColumn.Name.ToLower() == ignoreForeignKeyColumn.ToLower())
                {
                    continue;
                }
                HashSet<int> parentTablePrimaryKeyIdHashSet = new HashSet<int>();
                string parentPrimaryKeyListQuery = GetParentPrimaryKeyListQuery(foreignKeyColumn);
                BatchProcess(primaryKeyIdHashSet, (pkIds) =>
                {
                    List<int> integerList = batchPublishDataHandler.GetList(parentPrimaryKeyListQuery, pkIds);
                    parentTablePrimaryKeyIdHashSet.UnionWith(integerList);
                });
                if (parentTablePrimaryKeyIdHashSet.Any())
                {
                    Table parentTable = TableFactory.GetTable(executionContext, foreignKeyColumn.ReferencedTableName);
                    parentTable.BuildCreateIndexQuery(parentTablePrimaryKeyIdHashSet, indexQueryMap, string.Empty);
                }
            }
            if (children.Any())
            {
                foreach (var tableReference in children)
                {
                    HashSet<int> childTablePrimaryKeyIdHashSet = new HashSet<int>();
                    string childPrimaryKeyListQuery = tableReference.GetChildPrimaryKeyListQuery();
                    BatchProcess(primaryKeyIdHashSet, (pkIds) =>
                    {
                        List<int> integerList = batchPublishDataHandler.GetList(childPrimaryKeyListQuery, pkIds);
                        childTablePrimaryKeyIdHashSet.UnionWith(integerList);
                    });
                    if (childTablePrimaryKeyIdHashSet.Any())
                    {
                        string referenceName = tableReference.ParentTable.Name + "-" + tableReference.ChildTable.Name;
                        if (indexQueryMap.ContainsKey(referenceName) == false)
                        {
                            indexQueryMap.Add(referenceName, tableReference.GetCreateForeignKeyIndexQuery());
                        }
                        tableReference.ChildTable.BuildCreateIndexQuery(childTablePrimaryKeyIdHashSet,
                                                                          indexQueryMap,
                                                                          tableReference.ForeignKeyColumn.Name);
                    }
                }
            }
            log.LogMethodExit();
        }

        public virtual string GetDropIndexQuery()
        {
            log.LogMethodEntry();
            StringBuilder sb = new StringBuilder("DROP INDEX [IX_");
            sb.Append(name);
            sb.Append("_site_id_MasterEntityId_");
            sb.Append(primaryKeyColumn.Name);
            sb.Append("] ON [dbo].[");
            sb.Append(name);
            sb.Append("]");
            sb.Append(Environment.NewLine);
            sb.Append("GO");
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        public virtual void Publish(HashSet<int> primaryKeyIdHashSet,
                                    HashSet<int> siteIdHashSet,
                                    string ignoreForeignKeyColumn,
                                    bool forcePublish,
                                    int depth,
                                    bool referredEntity,
                                    bool enableAuditLog,
                                    SqlTransaction trx)
        {
            log.LogMethodEntry(primaryKeyIdHashSet, siteIdHashSet, ignoreForeignKeyColumn, forcePublish, depth, referredEntity, trx);
            if(depth < 0)
            {
                log.LogMethodEntry(null, "depth is less than 0");
                return;
            }
            BatchPublishDataHandler batchPublishDataHandler = new BatchPublishDataHandler(trx);
            string masterSiteUpdateMasterEntityIdQuery = GetMasterSiteUpdateMasterEntityIdQuery();
            BatchProcess(primaryKeyIdHashSet, siteIdHashSet, (pkIds, siteIds) =>
            {
                batchPublishDataHandler.ExecuteQuery(masterSiteUpdateMasterEntityIdQuery, pkIds, siteIds);
            });
            string updateMasterEntityIdUsingUniqueColumnsQuery = GetUpdateMasterEntityIdUsingUniqueColumnsQuery();
            if(string.IsNullOrWhiteSpace(updateMasterEntityIdUsingUniqueColumnsQuery) == false)
            {
                BatchProcess(primaryKeyIdHashSet, siteIdHashSet, (pkIds, siteIds) =>
                {
                    batchPublishDataHandler.ExecuteQuery(updateMasterEntityIdUsingUniqueColumnsQuery, pkIds, siteIds);
                });
            }
            List<ForeignKeyColumn> foreignKeyColumns = GetForeignKeyColumns();
            foreach (var foreignKeyColumn in foreignKeyColumns)
            {
                if (foreignKeyColumn.Name.ToLower() == ignoreForeignKeyColumn.ToLower())
                {
                    continue;
                }
                HashSet<int> parentTablePrimaryKeyIdHashSet = new HashSet<int>();
                string parentPrimaryKeyListQuery = GetParentPrimaryKeyListQuery(foreignKeyColumn);
                BatchProcess(primaryKeyIdHashSet, (pkIds) =>
                {
                    List<int> integerList = batchPublishDataHandler.GetList(parentPrimaryKeyListQuery, pkIds);
                    parentTablePrimaryKeyIdHashSet.UnionWith(integerList);
                });
                if(parentTablePrimaryKeyIdHashSet.Any())
                {
                    Table parentTable = TableFactory.GetTable(executionContext, foreignKeyColumn.ReferencedTableName);
                    parentTable.Publish(parentTablePrimaryKeyIdHashSet, siteIdHashSet, string.Empty, forcePublish, depth -1, true, enableAuditLog, trx);
                }
            }
            string publishQuery = GetPublishQuery(forcePublish, referredEntity, enableAuditLog);
            BatchProcess(primaryKeyIdHashSet, siteIdHashSet, (pkIds, siteIds) =>
            {
                batchPublishDataHandler.ExecuteQuery(publishQuery, pkIds, siteIds);
            });
            if(children.Any())
            {
                foreach (var tableReference in children)
                {
                    HashSet<int> childTablePrimaryKeyIdHashSet = new HashSet<int>();
                    string childPrimaryKeyListQuery = tableReference.GetChildPrimaryKeyListQuery();
                    BatchProcess(primaryKeyIdHashSet, (pkIds) =>
                    {
                        List<int> integerList = batchPublishDataHandler.GetList(childPrimaryKeyListQuery, pkIds);
                        childTablePrimaryKeyIdHashSet.UnionWith(integerList);
                    });
                    if (childTablePrimaryKeyIdHashSet.Any())
                    {
                        tableReference.ChildTable.Publish(childTablePrimaryKeyIdHashSet, 
                                                          siteIdHashSet, 
                                                          tableReference.ForeignKeyColumn.Name,
                                                          forcePublish,
                                                          depth - 1,
                                                          false,
                                                          enableAuditLog,
                                                          trx);
                    }
                }
            }
            log.LogMethodExit();
        }

        protected void BatchProcess(HashSet<int> pkIdHashSet, HashSet<int> siteIdHashSet, Action<List<int>, List<int>> action)
        {
            if (siteIdHashSet == null ||
                siteIdHashSet.Count == 0)
            {
                throw new Exception("siteIdHashSet is empty");
            }
            if (pkIdHashSet == null ||
                pkIdHashSet.Count == 0)
            {
                throw new Exception("pkIdList is empty");
            }
            List<int> pkIdList = new List<int>(pkIdHashSet);
            List<int> siteIdList = new List<int>(siteIdHashSet);
            int batchSize = 20000 / siteIdHashSet.Count;
            if (batchSize == 0)
            {
                batchSize++;
            }
            int totalNoOfRecords = pkIdHashSet.Count;
            int noOfBatches = totalNoOfRecords / batchSize;
            if (totalNoOfRecords % batchSize > 0)
            {
                noOfBatches++;
            }
            for (int i = 0; i < noOfBatches; i++)
            {
                int index = i * batchSize;
                int count = batchSize;
                if (index + count > totalNoOfRecords)
                {
                    count = totalNoOfRecords - index;
                }
                if (count <= 0)
                {
                    continue;
                }
                List<int> subset = pkIdList.GetRange(index, count);
                action(subset, siteIdList);
            }
        }

        protected void BatchProcess(HashSet<int> pkIdHashSet, Action<List<int>> action)
        {
            if (pkIdHashSet == null ||
                pkIdHashSet.Count == 0)
            {
                throw new Exception("pkIdList is empty");
            }
            List<int> pkIdList = new List<int>(pkIdHashSet);
            int batchSize = 5000;
            if (batchSize == 0)
            {
                batchSize++;
            }
            int totalNoOfRecords = pkIdHashSet.Count;
            int noOfBatches = totalNoOfRecords / batchSize;
            if (totalNoOfRecords % batchSize > 0)
            {
                noOfBatches++;
            }
            for (int i = 0; i < noOfBatches; i++)
            {
                int index = i * batchSize;
                int count = batchSize;
                if (index + count > totalNoOfRecords)
                {
                    count = totalNoOfRecords - index;
                }
                if (count <= 0)
                {
                    continue;
                }
                List<int> subset = pkIdList.GetRange(index, count);
                action(subset);
            }
        }

        public virtual List<ForeignKeyColumn> GetForeignKeyColumns()
        {
            log.LogMethodEntry();
            List<ForeignKeyColumn> foreignKeyColumns = columns
                                                        .Where(x => x is ForeignKeyColumn)
                                                        .Select(x => x as ForeignKeyColumn)
                                                        .ToList();
            log.LogMethodExit(foreignKeyColumns);
            return foreignKeyColumns;
        }

        public virtual List<ForeignKeyColumn> GetReferencingColumns(Table parentTable)
        {
            log.LogMethodEntry(parentTable);
            List<ForeignKeyColumn> referencingColumns = new List<ForeignKeyColumn>();
            foreach (var column in columns)
            {
                ForeignKeyColumn foreignKeyColumn = column as ForeignKeyColumn;
                if (foreignKeyColumn == null)
                {
                    continue;
                }
                if (foreignKeyColumn.ReferencedTableName.ToLower() != parentTable.Name.ToLower())
                {
                    continue;
                }
                referencingColumns.Add(foreignKeyColumn);
            }
            log.LogMethodExit(referencingColumns);
            return referencingColumns;
        }

        public virtual void Traverse(HashSet<int> primaryKeyIdHashSet, List<string> input, string indent, int depth, string skipForeignKeyColumn)
        {
            log.LogMethodEntry(input);
            BatchPublishDataHandler batchPublishDataHandler = new BatchPublishDataHandler();
            input.Add(indent + name);
            if (depth <= 0)
            {
                log.LogMethodExit();
                return;
            }
            List<ForeignKeyColumn> foreignKeyColumns = columns.Where(x => x is ForeignKeyColumn).Select(x => x as ForeignKeyColumn).ToList();
            foreach (var foreignKeyColumn in foreignKeyColumns)
            {
                if(foreignKeyColumn.Name == skipForeignKeyColumn)
                {
                    continue;
                }

                HashSet<int> parentTablePrimaryKeyIdHashSet = new HashSet<int>();
                string parentPrimaryKeyListQuery = GetParentPrimaryKeyListQuery(foreignKeyColumn);
                BatchProcess(primaryKeyIdHashSet, (pkIds) =>
                {
                    List<int> integerList = batchPublishDataHandler.GetList(parentPrimaryKeyListQuery, pkIds);
                    parentTablePrimaryKeyIdHashSet.UnionWith(integerList);
                });
                if (parentTablePrimaryKeyIdHashSet.Any())
                {
                    Table parentTable = TableFactory.GetTable(executionContext, foreignKeyColumn.ReferencedTableName);
                    parentTable.Traverse(parentTablePrimaryKeyIdHashSet, input, indent + "----", depth - 1, string.Empty);
                }
            }
            foreach (var tableReference in children)
            {
                HashSet<int> childTablePrimaryKeyIdHashSet = new HashSet<int>();
                string childPrimaryKeyListQuery = tableReference.GetChildPrimaryKeyListQuery();
                BatchProcess(primaryKeyIdHashSet, (pkIds) =>
                {
                    List<int> integerList = batchPublishDataHandler.GetList(childPrimaryKeyListQuery, pkIds);
                    childTablePrimaryKeyIdHashSet.UnionWith(integerList);
                });
                if (childTablePrimaryKeyIdHashSet.Any())
                {
                    tableReference.ChildTable.Traverse(childTablePrimaryKeyIdHashSet,
                                                      input,
                                                      indent + "####",
                                                      depth - 1,
                                                      tableReference.ForeignKeyColumn.Name);
                }
            }
            log.LogMethodExit();
        }

        public List<Column> Columns
        {
            get
            {
                return columns;
            }
        }

        public PrimaryKeyColumn PrimaryKeyColumn
        {
            get
            {
                return primaryKeyColumn;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public List<TableReference> Children
        {
            get
            {
                return children;
            }
        }
    }
}
