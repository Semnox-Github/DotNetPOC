using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Publish
{
    public class SystemOptionsTable : Table
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public SystemOptionsTable(ExecutionContext executionContext) :
            base(executionContext, "systemoptions", "OptionType,OptionName")
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public override string GetPublishQuery(bool forcePublish, bool referredEntity, bool enableAuditLog)
        {
            log.LogMethodEntry(forcePublish);
            StringBuilder sb = new StringBuilder(enableAuditLog ? @"DECLARE @Output as StringType;
                                                   MERGE INTO " : "MERGE INTO ");
            string query = @"systemoptions tbl 
                            USING ( SELECT systemoptions.OptionType, systemoptions.OptionName, systemoptions.OptionValue, sl.Id as publishSite_id, systemoptions.MasterEntityId, systemoptions.CreatedBy, dbo.TimeZoneOffset(systemoptions.CreationDate, sl.Id) AS CreationDate, systemoptions.LastUpdatedBy, dbo.TimeZoneOffset(systemoptions.LastUpdateDate, sl.Id) AS LastUpdateDate, systemoptions.IsActive
                            FROM systemoptions, @pkIdList pkl, @siteIdList sl
                            WHERE systemoptions.OptionId = pkl.Id
                            AND NOT EXISTS(SELECT 1 FROM systemoptions so where so.site_id = sl.Id)) AS src
                            ON src.MasterEntityId = tbl.MasterEntityId AND tbl.site_id = src.publishSite_id ";
            if (referredEntity == false)
            {
                query += " WHEN MATCHED ";
                if (forcePublish == false)
                {
                    query +=
                            @"
                        AND 
                         ( EXISTS(SELECT tbl.OptionType EXCEPT SELECT src.OptionType)
                         OR  EXISTS(SELECT tbl.OptionName EXCEPT SELECT src.OptionName)
                         OR  EXISTS(SELECT tbl.OptionValue EXCEPT SELECT src.OptionValue)
                         OR  EXISTS(SELECT tbl.CreatedBy EXCEPT SELECT src.CreatedBy)
                         OR  EXISTS(SELECT tbl.CreationDate EXCEPT SELECT src.CreationDate)
                         OR  EXISTS(SELECT tbl.LastUpdatedBy EXCEPT SELECT src.LastUpdatedBy)
                         OR  EXISTS(SELECT tbl.LastUpdateDate EXCEPT SELECT src.LastUpdateDate)
                         OR  EXISTS(SELECT tbl.IsActive EXCEPT SELECT src.IsActive)
                        )";
                }
                query +=
                      @"
                    THEN UPDATE SET 
                    OptionType = src.OptionType
                    , OptionName = src.OptionName
                    , OptionValue = src.OptionValue
                    , CreatedBy = src.CreatedBy
                    , CreationDate = src.CreationDate
                    , LastUpdatedBy = src.LastUpdatedBy
                    , LastUpdateDate = src.LastUpdateDate
                    , IsActive = src.IsActive";
            }
            query += @" WHEN NOT MATCHED THEN insert (
                      OptionType
                    , OptionName
                    , OptionValue
                    , site_id
                    , MasterEntityId
                    , CreatedBy
                    , CreationDate
                    , LastUpdatedBy
                    , LastUpdateDate
                    , IsActive
                    ) VALUES ( 
                    src.OptionType
                    , src.OptionName
                    , src.OptionValue
                    , src.publishSite_id
                    , src.MasterEntityId
                    , src.CreatedBy
                    , src.CreationDate
                    , src.LastUpdatedBy
                    , src.LastUpdateDate
                    , src.IsActive
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

    }
}
