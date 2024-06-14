/********************************************************************************************
 * Project Name - Publish
 * Description  - class of EntityOverrideDateTable publish table
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.3      25-May-2023      Lakshminarayana                  Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Publish
{
    public class ManagementFormAccessChildTable : Table
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private string formNameColumn;
        private PrimaryKeyColumn parentPrimaryKeyColumn;
        private string parentTable;
        private string mainMenu;
        private string functionGroup;
        private string isActiveColumnName;
        private string isActiveValue;
        public ManagementFormAccessChildTable(ExecutionContext executionContext, 
                                              string parentTable, 
                                              string functionGroup,
                                              string mainMenu, 
                                              string formNameColumn,
                                              string isActiveColumnName,
                                              string isActiveValue) :
            base(executionContext, "ManagementFormAccess")
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            this.formNameColumn = formNameColumn;
            this.parentTable = parentTable;
            this.mainMenu = mainMenu;
            this.functionGroup = functionGroup;
            this.isActiveColumnName = isActiveColumnName;
            this.isActiveValue = isActiveValue;
            BatchPublishDataHandler batchPublishDataHandler = new BatchPublishDataHandler();
            var columns = batchPublishDataHandler.GetColumns(parentTable);
            parentPrimaryKeyColumn = (PrimaryKeyColumn) columns.First(x => x is PrimaryKeyColumn);
            log.LogMethodExit();
        }

        public override string GetPublishQuery(bool forcePublish, bool referredEntity, bool enableAuditLog)
        {
            log.LogMethodEntry(forcePublish);
            StringBuilder sb = new StringBuilder(enableAuditLog ? @"DECLARE @Output as StringType;
                                                   MERGE INTO " : "MERGE INTO ");
            string query = @"ManagementFormAccess tbl 
                            USING ( SELECT mfa.role_id AS role_id, 
	                           '"+ mainMenu + @"' AS main_menu,
	                           mfa.form_name AS form_name,
	                           ISNULL(ref.access_allowed,'N') AS access_allowed, 
	                           mfa.FunctionId FunctionId,
	                           '"+ functionGroup + @"' FunctionGroup,
                               mfa.publishSite_id as publishSite_id,
                               ref.MasterEntityId AS MasterEntityId, 
	                           mfa.FunctionGUID FunctionGUID, 
	                           ISNULL(ref.CreatedBy,'"+executionContext.UserId+ @"') AS CreatedBy, 
                               dbo.TimeZoneOffset(ISNULL(ref.CreationDate, GETDATE()), mfa.publishSite_id) AS CreationDate, 
                               ISNULL(ref.LastUpdatedBy,'" + executionContext.UserId + @"') AS LastUpdatedBy, 
	                           dbo.TimeZoneOffset(ISNULL(ref.LastUpdateDate, GETDATE()), mfa.publishSite_id) AS LastUpdateDate,
	                           ISNULL(ref.IsActive, IsParentActive) AS IsActive
                        FROM 
                        (SELECT ur.role_id, parent." + formNameColumn + @" form_name, parent."+ parentPrimaryKeyColumn.Name+ @" FunctionId,
                        sl.Id publishSite_id, parent.Guid FunctionGUID, pkl.Id pklId,
                        ur.MasterEntityId RoleMasterEntityId, CASE WHEN parent."+ isActiveColumnName + @" = "+ isActiveValue + @" THEN 1 ELSE 0 END IsParentActive
                        FROM user_roles ur, "+ parentTable + @" parent, @pkIdList pkl, @siteIdList sl
                        WHERE ur.site_id = sl.Id
                        AND parent.MasterEntityId = pkl.Id
                        AND parent.site_id = sl.Id) mfa
                        LEFT OUTER JOIN 
                        (SELECT mf.access_allowed, mf.role_id, masterParent." + parentPrimaryKeyColumn.Name + @" ParentId, 
                                mf.MasterEntityId, mf.CreatedBy, mf.CreationDate, 
		                        mf.LastUpdatedBy, mf.LastUpdateDate, CASE WHEN masterParent." + isActiveColumnName + " = " + isActiveValue + @" THEN mf.IsActive ELSE 0 END IsActive
                        FROM ManagementFormAccess mf, " + parentTable + @" masterParent
                        WHERE mf.FunctionGUID = masterParent.guid
                        ) ref ON ref.ParentId = mfa.pklId AND ref.role_id = mfa.RoleMasterEntityId) AS src
                            ON src.role_id = tbl.role_id AND src.FunctionGUID = tbl.FunctionGUID AND tbl.site_id = src.publishSite_id ";
            if (referredEntity == false)
            {
                query += " WHEN MATCHED ";
                if (forcePublish == false)
                {
                    query +=
                            @"
                        AND 
                         ( EXISTS(SELECT tbl.role_id EXCEPT SELECT src.role_id)
                            OR  EXISTS(SELECT tbl.main_menu EXCEPT SELECT src.main_menu)
                            OR  EXISTS(SELECT tbl.form_name EXCEPT SELECT src.form_name)
                            OR  EXISTS(SELECT tbl.access_allowed EXCEPT SELECT src.access_allowed)
                            OR  EXISTS(SELECT tbl.FunctionId EXCEPT SELECT src.FunctionId)
                            OR  EXISTS(SELECT tbl.FunctionGroup EXCEPT SELECT src.FunctionGroup)
                            OR  EXISTS(SELECT tbl.FunctionGUID EXCEPT SELECT src.FunctionGUID)
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
                      role_id = src.role_id
                    , main_menu = src.main_menu
                    , form_name = src.form_name
                    , access_allowed = src.access_allowed
                    , FunctionId = src.FunctionId
                    , FunctionGroup = src.FunctionGroup
                    , FunctionGUID = src.FunctionGUID
                    , CreatedBy = src.CreatedBy
                    , CreationDate = src.CreationDate
                    , LastUpdatedBy = src.LastUpdatedBy
                    , LastUpdateDate = src.LastUpdateDate
                    , IsActive = src.IsActive";
            }
            query += @" WHEN NOT MATCHED THEN insert (
                      role_id
                    , main_menu
                    , form_name
                    , access_allowed
                    , FunctionId
                    , FunctionGroup
                    , site_id
                    , MasterEntityId
                    , FunctionGUID
                    , CreatedBy
                    , CreationDate
                    , LastUpdatedBy
                    , LastUpdateDate
                    , IsActive
                    ) VALUES ( 
                      src.role_id
                    , src.main_menu
                    , src.form_name
                    , src.access_allowed
                    , src.FunctionId
                    , src.FunctionGroup
                    , src.publishSite_id
                    , src.MasterEntityId
                    , src.FunctionGUID
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

        public override string GetChildPrimaryKeyListQuery(TableReference tableReference)
        {
            log.LogMethodEntry(tableReference);
            string result = @"select pkl.Id
                            from @pkIdList pkl";
            log.LogMethodExit(result);
            return result;
        }

        public override string GetParentPrimaryKeyListQuery(ForeignKeyColumn foreignKeyColumn)
        {
            log.LogMethodEntry(foreignKeyColumn);
            string result = @"select NULL AS Id
                              WHERE 1 = 2";
            log.LogMethodExit(result);
            return result;
        }

        public override string GetMasterSiteUpdateMasterEntityIdQuery()
        {
            log.LogMethodEntry();
            string result = @"UPDATE tbl 
                             SET tbl.MasterEntityId = tbl.ManagementFormAccessId
                             FROM managementformaccess tbl , " + parentTable + @" parent, @pkIdList pkl
                             WHERE EXISTS (SELECT tbl.MasterEntityId  EXCEPT SELECT tbl.ManagementFormAccessId)
                             AND tbl.site_id = (SELECT TOP 1 Master_Site_Id FROM Company) 
                             AND parent." + parentPrimaryKeyColumn.Name + @" = pkl.Id
                             AND tbl.FunctionGUID =  parent.Guid";
            log.LogMethodExit(result);
            return result;
        }
    }
}
