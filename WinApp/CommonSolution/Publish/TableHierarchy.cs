using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Publish
{
    public class TableHierarchy1
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string tableName;
        private readonly List<TableHierarchy> children;
        private List<string> uniqueIdColumns;
        private readonly string foreignKey;

        public TableHierarchy(string tableName) :
            this(tableName, string.Empty, new List<string>())
        {
            log.LogMethodEntry(tableName);
            log.LogMethodExit();
        }

        public TableHierarchy(string tableName, string foreignKey) :
            this(tableName, foreignKey, new List<string>())
        {
            log.LogMethodEntry(tableName, foreignKey);
            log.LogMethodExit();
        }

        public TableHierarchy(string tableName, string foreignKey, List<string> uniqueIdColumns)
        {
            log.LogMethodEntry(tableName, foreignKey, uniqueIdColumns);
            this.tableName = tableName;
            this.uniqueIdColumns = uniqueIdColumns;
            this.foreignKey = foreignKey;
            children = new List<TableHierarchy>();
            log.LogMethodExit();
        }

        public void AddChild(TableHierarchy tableHierarchy)
        {
            log.LogMethodEntry(tableName, foreignKey);
            children.Add(tableHierarchy);
            log.LogMethodExit();
        }

        public string TableName { get { return tableName; } }
        public string ForeignKey { get { return foreignKey; } }
        public List<string> UniqueIdColumns { get { return uniqueIdColumns; } }
        public List<TableHierarchy> Children { get { return children; } }
    }
}
