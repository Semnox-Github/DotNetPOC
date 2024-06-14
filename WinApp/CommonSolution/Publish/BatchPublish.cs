using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Publish
{
    public class BatchPublish
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public BatchPublish(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
        }

        public void Publish(string tableName, 
                            HashSet<int> primaryKeyIdHashSet, 
                            HashSet<int> siteIdHashSet,
                            bool forcePublish = true,
                            int depth = 20,
                            bool enableAuditLog = false,
                            SqlTransaction trx = null)
        {
            log.LogMethodEntry(tableName, primaryKeyIdHashSet, siteIdHashSet);
            if(string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName is empty");
            }
            if (primaryKeyIdHashSet == null || 
                primaryKeyIdHashSet.Any() == false)
            {
                throw new ArgumentNullException("primaryKeyIdHashSet is empty");
            }
            if (siteIdHashSet == null ||
                siteIdHashSet.Any() == false)
            {
                throw new ArgumentNullException("siteIdList is empty");
            }
            Table table = TableFactory.GetTable(executionContext,tableName);
            table.Publish(primaryKeyIdHashSet, siteIdHashSet, string.Empty, forcePublish, depth, false, enableAuditLog, trx);
            log.LogMethodExit();
        }
    }
}
