/********************************************************************************************
 * Project Name - Publish
 * Description  - class of ProductMenuPanelContent publish table
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      06-May-2021      Lakshminarayana           Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Publish
{
    

    public class ManagementFormAccessTable : Table
    {
        class ManagementFormAccessData
        {
            public string Table { get; set; }
            public string FunctionGroup { get; set; }
            public string MainMenu { get; set; }
            public bool UniqueMainMenu { get; set; }
            public bool UniqueFunctionGroup { get; set; }
            public bool PublishEnabled { get; set; }
            public bool ParentPublishEnabled { get; set; }
            public string ReferencedTablePrimaryKey { get; set; }
        }
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<ManagementFormAccessData> managementFormAccessDataList = new List<ManagementFormAccessData>()
        {
            new ManagementFormAccessData()
            {
                Table = "RedemptionCurrency",
                FunctionGroup = "Data Access",
                MainMenu = "Redemption Currency",
                UniqueMainMenu = true,
                UniqueFunctionGroup = true,
                PublishEnabled = true,
                ParentPublishEnabled = false,
                ReferencedTablePrimaryKey = "CurrencyId"
            },
            new ManagementFormAccessData()
            {
                Table = "user_roles",
                FunctionGroup = "Data Access",
                MainMenu = "User Roles",
                UniqueMainMenu = true,
                UniqueFunctionGroup = true,
                PublishEnabled = true,
                ParentPublishEnabled = false,
                ReferencedTablePrimaryKey = "role_id"
            },
            new ManagementFormAccessData()
            {
                Table = "Game_Profile",
                FunctionGroup = "Data Access",
                MainMenu = "Game Profile",
                UniqueMainMenu = true,
                UniqueFunctionGroup = true,
                PublishEnabled = true,
                ParentPublishEnabled = false,
                ReferencedTablePrimaryKey = "game_profile_id"
            },
            new ManagementFormAccessData()
            {
                Table = "POSMachines",
                FunctionGroup = "Data Access",
                MainMenu = "POS Machine",
                UniqueMainMenu = true,
                UniqueFunctionGroup = true,
                PublishEnabled = true,
                ParentPublishEnabled = false,
                ReferencedTablePrimaryKey = "POSMachineId"
            },
            new ManagementFormAccessData()
            {
                Table = "Reports",
                FunctionGroup = "Reports",
                MainMenu = "Run Reports",
                UniqueMainMenu = true,
                UniqueFunctionGroup = true,
                PublishEnabled = true,
                ParentPublishEnabled = false,
                ReferencedTablePrimaryKey = "report_id"
            },
            new ManagementFormAccessData()
            {
                Table = "POSTypes",
                FunctionGroup = "Data Access",
                MainMenu = "POS Counter",
                UniqueMainMenu = true,
                UniqueFunctionGroup = true,
                PublishEnabled = true,
                ParentPublishEnabled = false,
                ReferencedTablePrimaryKey = "POSTypeId"
            },
            new ManagementFormAccessData()
            {
                Table = "site",
                FunctionGroup = "Data Access",
                MainMenu = "Sites",
                UniqueMainMenu = true,
                UniqueFunctionGroup = true,
                PublishEnabled = false,
                ParentPublishEnabled = false,
                ReferencedTablePrimaryKey = "site_id"
            },
            new ManagementFormAccessData()
            {
                Table = "SystemOptions",
                FunctionGroup = "POS Task Access",
                MainMenu = "",
                UniqueMainMenu = false,
                UniqueFunctionGroup = true,
                PublishEnabled = true,
                ParentPublishEnabled = true,
                ReferencedTablePrimaryKey = "OptionId"
            },
            new ManagementFormAccessData()
            {
                Table = "ManagementForms",
                FunctionGroup = "Management Studio",
                MainMenu = "",
                UniqueMainMenu = false,
                UniqueFunctionGroup = false,
                PublishEnabled = true,
                ParentPublishEnabled = true,
                ReferencedTablePrimaryKey = "ManagementFormId"
            }
        };

        public ManagementFormAccessTable(ExecutionContext executionContext) 
            : base(executionContext, "ManagementFormAccess")
        {
            log.LogMethodEntry(executionContext);
            foreach (var managementFormAccessData in managementFormAccessDataList)
            {
                if(managementFormAccessData.ParentPublishEnabled == false)
                {
                    continue;
                }
                Columns.Add(new ForeignKeyColumn("FunctionGUID", managementFormAccessData.Table, managementFormAccessData.ReferencedTablePrimaryKey));
            }
            log.LogMethodExit();
        }

        public override string GetPublishQuery(bool forcePublish, bool referredEntity, bool enableAuditLog)
        {
            log.LogMethodEntry(forcePublish);
            StringBuilder sb = new StringBuilder(enableAuditLog ? @"DECLARE @Output as StringType;
                                                   MERGE INTO " : "MERGE INTO ");
            string query = @"ManagementFormAccess tbl 
                            USING ( SELECT *
                                    FROM(SELECT (SELECT TOP 1 ref.role_id FROM user_roles ref  WHERE ref.MasterEntityId = ManagementFormAccess.role_id AND ref.site_id = sl.id) role_id, 
                                    ManagementFormAccess.main_menu, 
                                    ManagementFormAccess.form_name, 
                                    ManagementFormAccess.access_allowed, 
                                    ManagementFormAccess.FunctionId,
                                    ManagementFormAccess.FunctionGroup, 
                                    sl.Id as publishSite_id, 
                                    ManagementFormAccess.MasterEntityId,
                                    CASE ";
            foreach (var managementFormAccessData in managementFormAccessDataList)
            {
                if(managementFormAccessData.PublishEnabled == false)
                {
                    continue;
                }
                query += Environment.NewLine + (managementFormAccessData.UniqueFunctionGroup ? "WHEN ManagementFormAccess.FunctionGroup = '" + managementFormAccessData.FunctionGroup + @"' " : " ")  + (managementFormAccessData.UniqueMainMenu? (" AND ManagementFormAccess.main_menu = '" + managementFormAccessData.MainMenu + @"' ") : "") + (managementFormAccessData.UniqueFunctionGroup ? " THEN"  : " ELSE ") + @" (SELECT TRY_CAST(ref.Guid AS nvarchar(max)) FROM " + managementFormAccessData.Table + " ref, " + managementFormAccessData.Table + " otherRef WHERE ref.MasterEntityId = otherRef.MasterEntityId AND otherRef.Guid = ManagementFormAccess.FunctionGUId AND ref.site_id = sl.id)";
            }
            query += @"             END AS FunctionGUID, 
                                    ManagementFormAccess.CreatedBy, 
                                    dbo.TimeZoneOffset(ManagementFormAccess.CreationDate, sl.Id) AS CreationDate,
                                    ManagementFormAccess.LastUpdatedBy, 
                                    dbo.TimeZoneOffset(ManagementFormAccess.LastUpdateDate, sl.Id) AS LastUpdateDate, 
                                    ManagementFormAccess.IsActive
                                    FROM ManagementFormAccess, @pkIdList pkl, @siteIdList sl
                                    WHERE ManagementFormAccess.ManagementFormAccessId = pkl.Id) A
                                    WHERE FunctionGUID IS NOT NULL AND role_id IS NOT NULL) AS src
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
                        ) ";
                }
                query += @" THEN UPDATE SET 
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
                            , MasterEntityId = src.MasterEntityId
                            , IsActive = src.IsActive ";
            }
            query +=
                  @" WHEN NOT MATCHED THEN insert (
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

        public override string GetParentPrimaryKeyListQuery(ForeignKeyColumn foreignKeyColumn)
        {
            log.LogMethodEntry(foreignKeyColumn);
            string result = string.Empty;
            Dictionary<string, ManagementFormAccessData> referencedTables = new Dictionary<string, ManagementFormAccessData>();
            foreach (var managementFormAccessData in managementFormAccessDataList)
            {
                if (managementFormAccessData.ParentPublishEnabled == false)
                {
                    continue;
                }
                referencedTables.Add(managementFormAccessData.Table.ToLower(), managementFormAccessData);
            }
            if(referencedTables.ContainsKey(foreignKeyColumn.ReferencedTableName.ToLower()))
            {
                result = @"select " + foreignKeyColumn.ReferencedTableName + "." + foreignKeyColumn.ReferencedColumnName + @" Id
                            from ManagementFormAccess, " + foreignKeyColumn.ReferencedTableName + @", @pkIdList pkl
                            WHERE pkl.Id = ManagementFormAccess.ManagementFormAccessId
                            AND ManagementFormAccess.FunctionGUID = " + foreignKeyColumn.ReferencedTableName + ".Guid";
            }
            else
            {
                result = @"select NULL AS Id
                              WHERE 1 = 2";
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
