using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Publish
{
    public class TableReference
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Table parentTable;
        private Table childTable;
        private Column foreignKeyColumn;
        public TableReference(Table parentTable, Table childTable)
        {
            log.LogMethodEntry(parentTable, childTable);
            this.parentTable = parentTable;
            this.childTable = childTable;
            List<ForeignKeyColumn> foreignKeyColumns = GetForeignKeyColumns(parentTable, childTable);
            if (foreignKeyColumns.Count > 1)
            {
                throw new Exception("Multiple references from table " + childTable.Name + " to table " + parentTable.Name);
            }
            foreignKeyColumn = foreignKeyColumns[0];
            log.LogMethodExit();
        }

        public TableReference(Table parentTable, Table childTable, string foreignKeyColumnName)
        {
            log.LogMethodEntry(parentTable, childTable);
            this.parentTable = parentTable;
            this.childTable = childTable;
            this.foreignKeyColumn = childTable.Columns.FirstOrDefault(x => x.Name.ToLower() == foreignKeyColumnName.ToLower());
            if(foreignKeyColumn == null)
            {
                throw new Exception(foreignKeyColumnName + " doesn't exist in table " + childTable.Name);
            }
            log.LogMethodExit();
        }

        public string GetCreateForeignKeyIndexQuery()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            StringBuilder sb = new StringBuilder("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_");
            sb.Append(childTable.Name);
            sb.Append("_");
            sb.Append(foreignKeyColumn.Name);
            sb.Append("' and OBJECT_NAME(object_id) = '");
            sb.Append(childTable.Name);
            sb.Append("')");
            sb.Append(Environment.NewLine);
            sb.Append("BEGIN");
            sb.Append(Environment.NewLine);
            sb.Append("CREATE NONCLUSTERED INDEX IX_");
            sb.Append(childTable.Name);
            sb.Append("_");
            sb.Append(foreignKeyColumn.Name);
            sb.Append(Environment.NewLine);
            sb.Append("ON [dbo].[");
            sb.Append(childTable.Name);
            sb.Append("] ([");
            sb.Append(foreignKeyColumn.Name);
            sb.Append("])");
            sb.Append(Environment.NewLine);
            sb.Append("END");
            sb.Append(Environment.NewLine);
            sb.Append("GO");
            result = sb.ToString();
            log.LogMethodExit(result);
            return result;

        }

        private List<ForeignKeyColumn> GetForeignKeyColumns(Table parentTable, Table childTable)
        {
            List<ForeignKeyColumn> foreignKeyColumns = childTable.GetReferencingColumns(parentTable);
            if (foreignKeyColumns == null || foreignKeyColumns.Any() == false)
            {
                throw new Exception(childTable.Name + " is not referencing " + parentTable.Name);
            }

            return foreignKeyColumns;
        }

        public string GetChildPrimaryKeyListQuery()
        {
            return childTable.GetChildPrimaryKeyListQuery(this);
        }

        public Table ParentTable
        {
            get
            {
                return parentTable;
            }
        }

        public Table ChildTable
        {
            get
            {
                return childTable;
            }
        }

        public Column ForeignKeyColumn
        {
            get
            {
                return foreignKeyColumn;
            }
        }
    }
}
