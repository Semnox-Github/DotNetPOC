using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Publish
{
    public class ManagementFormsTable : Table
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public ManagementFormsTable(ExecutionContext executionContext) :
            base(executionContext, "ManagementForms", "FunctionGroup,GroupName,FormName")
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public override string GetPublishQuery(bool forcePublish, bool referredEntity, bool enableAuditLog)
        {
            log.LogMethodEntry(forcePublish);
            StringBuilder sb = new StringBuilder(enableAuditLog ? @"DECLARE @Output as StringType;
                                                   MERGE INTO " : "MERGE INTO ");
            string query = @"managementforms tbl 
                            USING ( SELECT managementforms.FunctionGroup, managementforms.GroupName, managementforms.FormName, managementforms.FormLookupTable, managementforms.FontImageIcon, managementforms.FormTargetPath, managementforms.EnableAccess, managementforms.DisplayOrder, sl.Id as publishSite_id, managementforms.MasterEntityId, managementforms.CreatedBy, dbo.TimeZoneOffset(managementforms.CreationDate, sl.Id) AS CreationDate, managementforms.LastUpdatedBy, dbo.TimeZoneOffset(managementforms.LastUpdateDate, sl.Id) AS LastUpdateDate, managementforms.IsActive
                            FROM managementforms, @pkIdList pkl, @siteIdList sl
                            WHERE managementforms.ManagementFormId = pkl.Id
                            AND NOT EXISTS(SELECT 1 FROM managementforms mf where mf.site_id = sl.Id)) AS src
                            ON src.MasterEntityId = tbl.MasterEntityId AND tbl.site_id = src.publishSite_id ";
            if (referredEntity == false)
            {
                query += " WHEN MATCHED ";
                if (forcePublish == false)
                {
                    query +=
                            @"
                        AND 
                         ( EXISTS(SELECT tbl.FunctionGroup EXCEPT SELECT src.FunctionGroup)
                         OR  EXISTS(SELECT tbl.GroupName EXCEPT SELECT src.GroupName)
                         OR  EXISTS(SELECT tbl.FormName EXCEPT SELECT src.FormName)
                         OR  EXISTS(SELECT tbl.FormLookupTable EXCEPT SELECT src.FormLookupTable)
                         OR  EXISTS(SELECT tbl.FontImageIcon EXCEPT SELECT src.FontImageIcon)
                         OR  EXISTS(SELECT tbl.FormTargetPath EXCEPT SELECT src.FormTargetPath)
                         OR  EXISTS(SELECT tbl.EnableAccess EXCEPT SELECT src.EnableAccess)
                         OR  EXISTS(SELECT tbl.DisplayOrder EXCEPT SELECT src.DisplayOrder)
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
                      FunctionGroup = src.FunctionGroup
                    , GroupName = src.GroupName
                    , FormName = src.FormName
                    , FormLookupTable = src.FormLookupTable
                    , FontImageIcon = src.FontImageIcon
                    , FormTargetPath = src.FormTargetPath
                    , EnableAccess = src.EnableAccess
                    , DisplayOrder = src.DisplayOrder
                    , CreatedBy = src.CreatedBy
                    , CreationDate = src.CreationDate
                    , LastUpdatedBy = src.LastUpdatedBy
                    , LastUpdateDate = src.LastUpdateDate
                    , IsActive = src.IsActive";
            }
            query += @" WHEN NOT MATCHED THEN insert (
                      FunctionGroup
                    , GroupName
                    , FormName
                    , FormLookupTable
                    , FontImageIcon
                    , FormTargetPath
                    , EnableAccess
                    , DisplayOrder
                    , site_id
                    , MasterEntityId
                    , CreatedBy
                    , CreationDate
                    , LastUpdatedBy
                    , LastUpdateDate
                    , IsActive
                    ) VALUES ( 
                    src.FunctionGroup
                    , src.GroupName
                    , src.FormName
                    , src.FormLookupTable
                    , src.FontImageIcon
                    , src.FormTargetPath
                    , src.EnableAccess
                    , src.DisplayOrder
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
