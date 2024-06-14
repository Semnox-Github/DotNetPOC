/********************************************************************************************
 * Project Name - Publish
 * Description  - class of EntityOverrideDateTable publish table
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.3      27-Mar-2023      Abhishek                  Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Publish
{
    public class EntityOverrideDateTable : Table
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public EntityOverrideDateTable(ExecutionContext executionContext) :
            base(executionContext, "EntityOverrideDates")
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public override string GetPublishQuery(bool forcePublish, bool referredEntity, bool enableAuditLog)
        {
            log.LogMethodEntry(forcePublish);
            StringBuilder sb = new StringBuilder(enableAuditLog ? @"DECLARE @Output as StringType;
                                                   MERGE INTO " : "MERGE INTO ");
            string query = @"EntityOverrideDates tbl 
                            USING ( SELECT (SELECT TOP 1 ref.Id FROM EntityOverrideDates ref  WHERE ref.MasterEntityId = EntityOverrideDates.Id AND ref.site_id = sl.id) Id, 
                            EntityOverrideDates.EntityName, EntityOverrideDates.OverrideDate, EntityOverrideDates.IncludeExcludeFlag, EntityOverrideDates.Day, EntityOverrideDates.Remarks, EntityOverrideDates.IsActive, EntityOverrideDates.CreatedBy, EntityOverrideDates.CreationDate, EntityOverrideDates.LastUpdatedBy, EntityOverrideDates.LastupdatedDate, sl.Id as publishSite_id, EntityOverrideDates.MasterEntityId, 
                            (CASE WHEN EntityOverrideDates.EntityName = 'PRODUCTCREDITPLUS' THEN(SELECT TOP 1 ref.guid FROM ProductCreditPlus ref WHERE ref.MasterEntityId = (SELECT ProductCreditPlusId from ProductCreditPlus Where guid = EntityOverrideDates.EntityGuid) AND ref.site_id = sl.id) 
                                  WHEN EntityOverrideDates.EntityName = 'PRODUCTGAMESENTITLEMENTS' THEN (SELECT TOP 1 ref.guid FROM ProductGames ref WHERE ref.MasterEntityId = (SELECT Product_Game_Id from ProductGames Where guid = EntityOverrideDates.EntityGuid) AND ref.site_id = sl.id)
                                   END) EntityGuid
                            FROM EntityOverrideDates, @pkIdList pkl, @siteIdList sl
                            WHERE EntityOverrideDates.Id = pkl.Id) AS src
                            ON src.MasterEntityId = tbl.MasterEntityId AND tbl.site_id = src.publishSite_id ";
            if (referredEntity == false)
            {
                query += " WHEN MATCHED ";
                if (forcePublish == false)
                {
                    query +=
                            @"
                        AND 
                         ( EXISTS(SELECT tbl.EntityName EXCEPT SELECT src.EntityName)
                         OR  EXISTS(SELECT tbl.EntityGuid EXCEPT SELECT src.EntityGuid)
                         OR  EXISTS(SELECT tbl.OverrideDate EXCEPT SELECT src.OverrideDate)
                         OR  EXISTS(SELECT tbl.IncludeExcludeFlag EXCEPT SELECT src.IncludeExcludeFlag)
                         OR  EXISTS(SELECT tbl.Day EXCEPT SELECT src.Day)
                         OR  EXISTS(SELECT tbl.Remarks EXCEPT SELECT src.Remarks)
                         OR  EXISTS(SELECT tbl.IsActive EXCEPT SELECT src.IsActive)
                         OR  EXISTS(SELECT tbl.CreatedBy EXCEPT SELECT src.CreatedBy)
                         OR  EXISTS(SELECT tbl.CreationDate EXCEPT SELECT src.CreationDate)
                         OR  EXISTS(SELECT tbl.LastUpdatedBy EXCEPT SELECT src.LastUpdatedBy)
                         OR  EXISTS(SELECT tbl.LastupdatedDate EXCEPT SELECT src.LastupdatedDate)
                        )";
                }
                query +=
                      @"
                    THEN UPDATE SET 
                      EntityName = src.EntityName
                    , EntityGuid = src.EntityGuid
                    , OverrideDate = src.OverrideDate
                    , IncludeExcludeFlag = src.IncludeExcludeFlag
                    , Day = src.Day
                    , Remarks = src.Remarks
                    , IsActive = src.IsActive
                    , CreatedBy = src.CreatedBy
                    , CreationDate = src.CreationDate
                    , LastUpdatedBy = src.LastUpdatedBy
                    , LastupdatedDate = src.LastupdatedDate";
            }
            query += @" WHEN NOT MATCHED THEN insert (
                      EntityName
                    , EntityGuid
                    , OverrideDate
                    , IncludeExcludeFlag
                    , Day
                    , Remarks
                    , IsActive
                    , CreatedBy
                    , CreationDate
                    , LastUpdatedBy
                    , LastupdatedDate
                    , site_id
                    , MasterEntityId
                    ) VALUES ( 
                      src.EntityName
                    , src.EntityGuid
                    , src.OverrideDate
                    , src.IncludeExcludeFlag
                    , src.Day
                    , src.Remarks
                    , src.IsActive
                    , src.CreatedBy
                    , src.CreationDate
                    , src.LastUpdatedBy
                    , src.LastupdatedDate
                    , src.publishSite_id
                    , src.MasterEntityId
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
            if (tableReference.ParentTable.Name.ToLower() == "productcreditplus")
            {
                result = @"select EntityOverrideDates.Id
                            from EntityOverrideDates, ProductCreditPlus, @pkIdList pkl
                            WHERE  EntityOverrideDates.EntityGuid = ProductCreditPlus.Guid AND EntityName = 'PRODUCTCREDITPLUS'
                            AND pkl.Id = ProductCreditPlus.ProductCreditPlusId";
            }
            else if (tableReference.ParentTable.Name.ToLower() == "productgames")
            {
                result = @"select EntityOverrideDates.Id
                            from EntityOverrideDates, ProductGames, @pkIdList pkl
                            WHERE  EntityOverrideDates.EntityGuid = ProductGames.Guid AND EntityName = 'PRODUCTGAMESENTITLEMENTS'
                            AND pkl.Id = ProductGames.Product_Game_Id";
            }
            else
            {
                string message = "EntityOverrideDates for " + tableReference.ParentTable.Name + " is not implemented";
                log.LogMethodExit(null, message);
                throw new NotImplementedException(message);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
