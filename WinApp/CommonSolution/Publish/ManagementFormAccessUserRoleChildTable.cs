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
using System.Collections.Generic;

namespace Semnox.Parafait.Publish
{
    public class ManagementFormAccessUserRoleChildTable : Table
    {
        class ManagementFormAccessData
        {
            public string Table { get; set; }
            public string FunctionGroup { get; set; }
            public string MainMenu { get; set; }
            public string ReferencedTablePrimaryKey { get; set; }
            public string FormNameColumn { get; set; }
            public string IsActiveColumnName { get; set; }
            public string IsActiveValue { get; set; }
        }
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<ManagementFormAccessData> managementFormAccessDataList = new List<ManagementFormAccessData>()
        {
            new ManagementFormAccessData()
            {
                Table = "RedemptionCurrency",
                FunctionGroup = "Data Access",
                MainMenu = "Redemption Currency",
                FormNameColumn = "CurrencyName",
                ReferencedTablePrimaryKey = "CurrencyId",
                IsActiveColumnName = "IsActive",
                IsActiveValue = "1"
            },
            new ManagementFormAccessData()
            {
                Table = "user_roles",
                FunctionGroup = "Data Access",
                MainMenu = "User Roles",
                FormNameColumn = "Role",
                ReferencedTablePrimaryKey = "role_id",
                IsActiveColumnName = "IsActive",
                IsActiveValue = "1"
            },
            new ManagementFormAccessData()
            {
                Table = "Game_Profile",
                FunctionGroup = "Data Access",
                MainMenu = "Game Profile",
                FormNameColumn = "profile_name",
                ReferencedTablePrimaryKey = "game_profile_id",
                IsActiveColumnName = "IsActive",
                IsActiveValue = "1"
            },
            new ManagementFormAccessData()
            {
                Table = "POSMachines",
                FunctionGroup = "Data Access",
                MainMenu = "POS Machine",
                FormNameColumn = "POSName",
                ReferencedTablePrimaryKey = "POSMachineId",
                IsActiveColumnName = "IsActive",
                IsActiveValue = "1"
            },
            new ManagementFormAccessData()
            {
                Table = "Reports",
                FunctionGroup = "Reports",
                MainMenu = "Run Reports",
                FormNameColumn = "report_name",
                ReferencedTablePrimaryKey = "report_id",
                IsActiveColumnName = "IsActive",
                IsActiveValue = "1"
            },
            new ManagementFormAccessData()
            {
                Table = "POSTypes",
                FunctionGroup = "Data Access",
                MainMenu = "POS Counter",
                FormNameColumn = "POSTypeName",
                ReferencedTablePrimaryKey = "POSTypeId",
                IsActiveColumnName = "IsActive",
                IsActiveValue = "1"
            },
        };
        public ManagementFormAccessUserRoleChildTable(ExecutionContext executionContext) :
            base(executionContext, "ManagementFormAccess")
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
            string query = @"ManagementFormAccess tbl 
                            USING (";
            int i = 0;
            foreach (var managementFormAccessData in managementFormAccessDataList)
            {
                query += (i ++ > 0? Environment.NewLine + " UNION ALL " : Environment.NewLine) + @" SELECT mfa.role_id AS role_id, 
	                        '"+ managementFormAccessData.MainMenu + @"' AS main_menu,
	                        mfa.form_name AS form_name,
	                        ISNULL(ref.access_allowed,'N') AS access_allowed, 
	                        mfa.FunctionId FunctionId,
	                        '"+ managementFormAccessData.FunctionGroup + @"' FunctionGroup,
                            mfa.publishSite_id as publishSite_id,
                            ref.MasterEntityId AS MasterEntityId, 
	                        mfa.FunctionGUID FunctionGUID, 
	                        ISNULL(ref.CreatedBy,'"+executionContext.UserId+ @"') AS CreatedBy, 
                            dbo.TimeZoneOffset(ISNULL(ref.CreationDate, GETDATE()), mfa.publishSite_id) AS CreationDate, 
                            ISNULL(ref.LastUpdatedBy,'" + executionContext.UserId + @"') AS LastUpdatedBy, 
	                        dbo.TimeZoneOffset(ISNULL(ref.LastUpdateDate, GETDATE()), mfa.publishSite_id) AS LastUpdateDate,
	                        ISNULL(ref.IsActive, IsParentActive) AS IsActive
                    FROM 
                    (SELECT ur.role_id, parent."+ managementFormAccessData.FormNameColumn + @" form_name, parent."+ managementFormAccessData.ReferencedTablePrimaryKey + @" FunctionId,
                    sl.Id publishSite_id, parent.Guid FunctionGUID, pkl.Id pklId,
                    parent.MasterEntityId ParentMasterEntityId, CASE WHEN parent." + managementFormAccessData.IsActiveColumnName + @" = " + managementFormAccessData.IsActiveValue + @" THEN 1 ELSE 0 END IsParentActive
                    FROM user_roles ur, " + managementFormAccessData.Table + @" parent, @pkIdList pkl, @siteIdList sl
                    WHERE parent.site_id = sl.Id
                    AND ur.MasterEntityId = pkl.Id
                    AND ur.site_id = sl.Id) mfa
                    LEFT OUTER JOIN 
                    (SELECT mf.access_allowed, mf.role_id, masterParent." + managementFormAccessData.ReferencedTablePrimaryKey + @" ParentId, 
                            mf.MasterEntityId, mf.CreatedBy, mf.CreationDate, 
		                    mf.LastUpdatedBy, mf.LastUpdateDate, CASE WHEN masterParent."+ managementFormAccessData.IsActiveColumnName+ " = " + managementFormAccessData.IsActiveValue + @" THEN mf.IsActive ELSE 0 END IsActive
                    FROM ManagementFormAccess mf, " + managementFormAccessData.Table + @" masterParent
                    WHERE mf.FunctionGUID = masterParent.guid
                    ) ref ON ref.role_id = mfa.pklId AND ref.ParentId = mfa.ParentMasterEntityId ";
            }
            query += @"     ) AS src
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
            //Dummy update statement so that publish engine doesn't bomb
            string result = @"UPDATE ManagementFormAccess
                              SET LastUpdateDate = GETDATE()
                              WHERE 1 = 2";
            log.LogMethodExit(result);
            return result;
        }
    }
}
