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
using Semnox.Core.Utilities;
using System;
using System.Text;

namespace Semnox.Parafait.Publish
{
    public class ProductMenuPanelContentTable : Table
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public ProductMenuPanelContentTable(ExecutionContext executionContext) :
            base(executionContext, "ProductMenuPanelContent")
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            Columns.Add(new ForeignKeyColumn("ObjectGuid", "Products", "product_id"));
            Columns.Add(new ForeignKeyColumn("ObjectGuid", "ProductMenuPanel", "PanelId"));
            log.LogMethodExit();
        }

        public override string GetPublishQuery(bool forcePublish, bool referredEntity, bool enableAuditLog)
        {
            log.LogMethodEntry(forcePublish);
            StringBuilder sb = new StringBuilder(enableAuditLog ? @"DECLARE @Output as StringType;
                                                   MERGE INTO " : "MERGE INTO ");
            string query = @"ProductMenuPanelContent tbl 
                            USING ( SELECT (SELECT TOP 1 ref.PanelId FROM ProductMenuPanel ref  WHERE ref.MasterEntityId = ProductMenuPanelContent.PanelId AND ref.site_id = sl.id) PanelID, 
                            ProductMenuPanelContent.ObjectType, ProductMenuPanelContent.ButtonType, ProductMenuPanelContent.ImageURL, ProductMenuPanelContent.BackColor, ProductMenuPanelContent.TextColor, ProductMenuPanelContent.Font, ProductMenuPanelContent.RowIndex, ProductMenuPanelContent.ColumnIndex, ProductMenuPanelContent.IsActive, ProductMenuPanelContent.CreatedBy, ProductMenuPanelContent.CreationDate, ProductMenuPanelContent.LastUpdatedBy, ProductMenuPanelContent.LastupdatedDate, sl.Id as publishSite_id, ProductMenuPanelContent.MasterEntityId, 
                            (CASE WHEN ProductMenuPanelContent.ObjectType = 'PRODUCTS' THEN(SELECT TOP 1 ref.guid FROM Products ref WHERE ref.MasterEntityId = (SELECT product_id from Products Where guid = ProductMenuPanelContent.ObjectGuid) AND ref.site_id = sl.id) 
                                  WHEN ProductMenuPanelContent.ObjectType = 'PRODUCT_MENU_PANEL' THEN (SELECT TOP 1 ref.guid FROM ProductMenuPanel ref WHERE ref.MasterEntityId = (SELECT PanelId from ProductMenuPanel Where guid = ProductMenuPanelContent.ObjectGuid) AND ref.site_id = sl.id) END) ObjectGuid
                            FROM ProductMenuPanelContent, @pkIdList pkl, @siteIdList sl
                            WHERE ProductMenuPanelContent.Id = pkl.Id) AS src
                            ON src.MasterEntityId = tbl.MasterEntityId AND tbl.site_id = src.publishSite_id ";
            if (referredEntity == false)
            {
                query += " WHEN MATCHED ";
                if (forcePublish == false)
                {
                    query +=
                            @"
                        AND 
                         ( EXISTS(SELECT tbl.PanelId EXCEPT SELECT src.PanelId)
                         OR  EXISTS(SELECT tbl.ObjectType EXCEPT SELECT src.ObjectType)
                         OR  EXISTS(SELECT tbl.ObjectGuid EXCEPT SELECT src.ObjectGuid)
                         OR  EXISTS(SELECT tbl.ButtonType EXCEPT SELECT src.ButtonType)
                         OR  EXISTS(SELECT tbl.ImageURL EXCEPT SELECT src.ImageURL)
                         OR  EXISTS(SELECT tbl.BackColor EXCEPT SELECT src.BackColor)
                         OR  EXISTS(SELECT tbl.TextColor EXCEPT SELECT src.TextColor)
                         OR  EXISTS(SELECT tbl.Font EXCEPT SELECT src.Font)
                         OR  EXISTS(SELECT tbl.RowIndex EXCEPT SELECT src.RowIndex)
                         OR  EXISTS(SELECT tbl.ColumnIndex EXCEPT SELECT src.ColumnIndex)
                         OR  EXISTS(SELECT tbl.IsActive EXCEPT SELECT src.IsActive)
                         OR  EXISTS(SELECT tbl.CreatedBy EXCEPT SELECT src.CreatedBy)
                         OR  EXISTS(SELECT tbl.CreationDate EXCEPT SELECT src.CreationDate)
                         OR  EXISTS(SELECT tbl.LastUpdatedBy EXCEPT SELECT src.LastUpdatedBy)
                         OR  EXISTS(SELECT tbl.LastupdatedDate EXCEPT SELECT src.LastupdatedDate)
                        )";
                }
                query += @" THEN UPDATE SET 
                    PanelId = src.PanelId
                    , ObjectType = src.ObjectType
                    , ObjectGuid = src.ObjectGuid
                    , ButtonType = src.ButtonType
                    , ImageURL = src.ImageURL
                    , BackColor = src.BackColor
                    , TextColor = src.TextColor
                    , Font = src.Font
                    , RowIndex = src.RowIndex
                    , ColumnIndex = src.ColumnIndex
                    , IsActive = src.IsActive
                    , CreatedBy = src.CreatedBy
                    , CreationDate = src.CreationDate
                    , LastUpdatedBy = src.LastUpdatedBy
                    , LastupdatedDate = src.LastupdatedDate";
            }
            query +=
                  @" WHEN NOT MATCHED THEN insert (
                    PanelId
                    , ObjectType
                    , ObjectGuid
                    , ButtonType
                    , ImageURL
                    , BackColor
                    , TextColor
                    , Font
                    , RowIndex
                    , ColumnIndex
                    , IsActive
                    , CreatedBy
                    , CreationDate
                    , LastUpdatedBy
                    , LastupdatedDate
                    , site_id
                    , MasterEntityId
                    ) VALUES ( 
                    PanelId
                    , src.ObjectType
                    , src.ObjectGuid
                    , src.ButtonType
                    , src.ImageURL
                    , src.BackColor
                    , src.TextColor
                    , src.Font
                    , src.RowIndex
                    , src.ColumnIndex
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

        public override string GetParentPrimaryKeyListQuery(ForeignKeyColumn foreignKeyColumn)
        {
            log.LogMethodEntry(foreignKeyColumn);
            string result = string.Empty;
            if(foreignKeyColumn.ReferencedTableName.ToLower() == "products")
            {
                result = @"select products.product_id Id
                            from ProductMenuPanelContent, products, @pkIdList pkl
                            WHERE  ProductMenuPanelContent.ObjectGuid = products.Guid AND ObjectType = 'PRODUCTS'
                            AND pkl.Id = ProductMenuPanelContent.Id";
            }
            else if (foreignKeyColumn.ReferencedTableName.ToLower() == "productmenupanel")
            {
                result = @"select ProductMenuPanel.PanelId Id
                            from ProductMenuPanelContent, ProductMenuPanel, @pkIdList pkl
                            WHERE  ProductMenuPanelContent.ObjectGuid = ProductMenuPanel.Guid AND ObjectType = 'PRODUCT_MENU_PANEL'
                            AND pkl.Id = ProductMenuPanelContent.Id";
            }
            else
            {
                string message = "ProductMenuPanelContent for " + foreignKeyColumn.Name + " is not implemented";
                log.LogMethodExit(null, message);
                throw new NotImplementedException(message);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
