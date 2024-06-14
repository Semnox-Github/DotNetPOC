using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Publish
{
    public class Column
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly string name;

        public Column(string name)
        {
            log.LogMethodEntry(name);
            this.name = name;
            log.LogMethodExit();
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public virtual string GetUpdateQuery()
        {
            return name + " = src." + name;
        }

        public virtual string GetInsertQuery()
        {
            return "src." + name;
        }

        public virtual string GetInsertDefinitionQuery()
        {
            return name;
        }

        internal virtual string GetSourceTableQuery(string tableName)
        {
            log.LogMethodEntry(tableName);
            string result = tableName + "." + name;
            log.LogMethodExit(result); 
            return result;
        }

        internal virtual string GetInEqualQuery()
        {
            log.LogMethodEntry();
            string result = " EXISTS(SELECT tbl." + name + " EXCEPT SELECT src."+ name  + ")" ;
            log.LogMethodExit(result);
            return result;
        }

        internal virtual string GetUpdateMasterEntityIdUsingUniqueColumnQuery()
        {
            log.LogMethodEntry();
            StringBuilder sb = new StringBuilder();
            sb.Append("AND EXISTS(SELECT src.");
            sb.Append(name);
            sb.Append(" INTERSECT SELECT tbl.");
            sb.Append(name);
            sb.Append(")");
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }
    }

    public class ForeignKeyColumn : Column
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly string referencedTableName;
        protected readonly string referencedColumnName;

        public ForeignKeyColumn(string name, string referencedTableName, string referencedColumnName)
            :base(name)
        {
            log.LogMethodEntry(name, referencedTableName, referencedColumnName);
            this.referencedTableName = referencedTableName;
            this.referencedColumnName = referencedColumnName;
            log.LogMethodExit();
        }


        internal override string GetSourceTableQuery(string tableName)
        {
            log.LogMethodEntry(tableName);
            string result = tableName + "." + name;
            StringBuilder sb = new StringBuilder("(SELECT TOP 1 ref.");
            sb.Append(referencedColumnName);
            sb.Append(" FROM ");
            sb.Append(referencedTableName);
            sb.Append(" ref ");
            sb.Append(" WHERE ref.MasterEntityId = ");
            sb.Append(tableName);
            sb.Append(".");
            sb.Append(name);
            sb.Append(" AND ref.site_id = sl.id) ");
            sb.Append(name);
            result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        internal override string GetUpdateMasterEntityIdUsingUniqueColumnQuery()
        {
            log.LogMethodEntry();
            StringBuilder sb = new StringBuilder();
            sb.Append("AND EXISTS(SELECT src.");
            sb.Append(name);
            sb.Append(" INTERSECT SELECT MasterEntityId FROM ");
            sb.Append(referencedTableName);
            sb.Append(" ref WHERE ref.");
            sb.Append(referencedColumnName);
            sb.Append(" = tbl.");
            sb.Append(name);
            sb.Append(")");
            string result = sb.ToString();
            log.LogMethodExit(result);
            return result;
        }

        public string ReferencedTableName
        {
            get
            {
                return referencedTableName;
            }
        }

        public string ReferencedColumnName
        {
            get
            {
                return referencedColumnName;
            }
        }
    }

    public class DateTimeColumn : Column
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DateTimeColumn(string name)
            : base(name)
        {
            log.LogMethodEntry(name);
            log.LogMethodExit();
        }


        internal override string GetSourceTableQuery(string tableName)
        {
            log.LogMethodEntry(tableName);
            string result = "dbo.TimeZoneOffset(" + tableName + "." + name + ", sl.Id) AS " + name;
            log.LogMethodExit(result);
            return result;
        }

    }

    public class PrimaryKeyColumn : Column
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PrimaryKeyColumn(string name)
            : base(name)
        {
            log.LogMethodEntry(name);
            log.LogMethodExit();
        }

        public override string GetUpdateQuery()
        {
            return string.Empty;
        }

        public override string GetInsertQuery()
        {
            return string.Empty;
        }

        public override string GetInsertDefinitionQuery()
        {
            return string.Empty;
        }

        internal override string GetSourceTableQuery(string tableName)
        {
            log.LogMethodEntry(tableName);
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }

        internal override string GetInEqualQuery()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }

        internal override string GetUpdateMasterEntityIdUsingUniqueColumnQuery()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }
    }

    public class IgnoreColumn : Column
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IgnoreColumn(string name)
            : base(name)
        {
            log.LogMethodEntry(name);
            log.LogMethodExit();
        }

        public override string GetUpdateQuery()
        {
            return string.Empty;
        }

        public override string GetInsertQuery()
        {
            return string.Empty;
        }

        public override string GetInsertDefinitionQuery()
        {
            return string.Empty;
        }

        internal override string GetSourceTableQuery(string tableName)
        {
            log.LogMethodEntry(tableName);
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }

        internal override string GetInEqualQuery()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }

        internal override string GetUpdateMasterEntityIdUsingUniqueColumnQuery()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }
    }

    public class SiteIdColumn : Column
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SiteIdColumn(string name)
            : base(name)
        {
            log.LogMethodEntry(name);
            log.LogMethodExit();
        }

        public override string GetUpdateQuery()
        {
            return string.Empty;
        }

        public override string GetInsertQuery()
        {
            return "src.publishSite_id";
        }

        internal override string GetSourceTableQuery(string tableName)
        {
            log.LogMethodEntry(tableName);
            string result = "sl.Id as publishSite_id";
            log.LogMethodExit(result);
            return result;
        }

        internal override string GetInEqualQuery()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }

        internal override string GetUpdateMasterEntityIdUsingUniqueColumnQuery()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }
    }

    public class MasterEntityIdColumn : Column
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MasterEntityIdColumn(string name)
            : base(name)
        {
            log.LogMethodEntry(name);
            log.LogMethodExit();
        }

        public override string GetUpdateQuery()
        {
            return string.Empty;
        }

        public override string GetInsertQuery()
        {
            return "src." + name;
        }

        public override string GetInsertDefinitionQuery()
        {
            return name;
        }

        internal override string GetSourceTableQuery(string tableName)
        {
            log.LogMethodEntry(tableName);
            string result = tableName + "." + name;
            log.LogMethodExit(result);
            return result;
        }

        internal override string GetInEqualQuery()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }

        internal override string GetUpdateMasterEntityIdUsingUniqueColumnQuery()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }
    }
}
