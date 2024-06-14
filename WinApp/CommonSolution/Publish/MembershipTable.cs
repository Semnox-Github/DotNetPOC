/********************************************************************************************
 * Project Name - MembershipTable
 * Description  - Membership Table to handle the publish of membership 
 * 
 **************
  *Version     Date          Modified By       Remarks
 ********************************************************
  *2.140.0     07-Dec-2021   Lakshminarayana   Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Publish
{
    public class MembershipTable : Table
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public MembershipTable(ExecutionContext executionContext): base(executionContext,"membership")
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public override void Publish(HashSet<int> primaryKeyIdHashSet,
                                    HashSet<int> siteIdHashSet,
                                    string ignoreForeignKeyColumn,
                                    bool forcePublish,
                                    int depth,
                                    bool referredEntity,
                                    bool enableAuditLog,
                                    SqlTransaction trx)
        {
            log.LogMethodEntry(primaryKeyIdHashSet, siteIdHashSet, ignoreForeignKeyColumn, forcePublish, depth, referredEntity, trx);
            if (depth < 0)
            {
                log.LogMethodEntry(null, "depth is less than 0");
                return;
            }
            BatchPublishDataHandler batchPublishDataHandler = new BatchPublishDataHandler(trx);
            HashSet<int> childTablePrimaryKeyIdHashSet = new HashSet<int>();
            string childPrimaryKeyListQuery = @"SELECT CardTypeRule.ID AS Id
                                                FROM CardTypeRule, @pkIdList pkl
                                                WHERE CardTypeRule.MembershipID = pkl.Id
                                                AND CardTypeRule.site_id = (select master_Site_id from company)";
            BatchProcess(primaryKeyIdHashSet, (pkIds) =>
            {
                List<int> integerList = batchPublishDataHandler.GetList(childPrimaryKeyListQuery, pkIds);
                childTablePrimaryKeyIdHashSet.UnionWith(integerList);
            });
            if (childTablePrimaryKeyIdHashSet.Any())
            {
                Table cardTypeRuleTable = TableFactory.GetTable(executionContext,"CardTypeRule");
                cardTypeRuleTable.Publish(childTablePrimaryKeyIdHashSet,
                                            siteIdHashSet,
                                            "MembershipID",
                                            forcePublish,
                                            depth - 1,
                                            referredEntity,
                                            enableAuditLog,
                                            trx);
            }
            childTablePrimaryKeyIdHashSet = new HashSet<int>();
            childPrimaryKeyListQuery = @"SELECT MembershipRewards.MembershipRewardsId AS Id
                                         FROM MembershipRewards, @pkIdList pkl
                                         WHERE MembershipRewards.MembershipID = pkl.Id
                                         AND MembershipRewards.site_id = (select master_Site_id from company) ";
            BatchProcess(primaryKeyIdHashSet, (pkIds) =>
            {
                List<int> integerList = batchPublishDataHandler.GetList(childPrimaryKeyListQuery, pkIds);
                childTablePrimaryKeyIdHashSet.UnionWith(integerList);
            });
            if (childTablePrimaryKeyIdHashSet.Any())
            {
                Table membershipRewardsTable = TableFactory.GetTable(executionContext,"MembershipRewards");
                membershipRewardsTable.Publish(childTablePrimaryKeyIdHashSet,
                                                  siteIdHashSet,
                                                  "MembershipID",
                                                  forcePublish,
                                                  depth - 1,
                                                  referredEntity,
                                                  enableAuditLog,
                                                  trx);
            }
            log.LogMethodExit();
        }
    }
}
