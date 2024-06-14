/********************************************************************************************
 * Project Name - Product                                                                          
 * Description  - Downloads all Inventory products. 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.0    28-Jun-2019      Mehraj        Created 
 *2.90.0    02-Jul-2020      Deeksha       Inventory process : Weighted Avg Costing changes.
 *2.100.0   28-Jul-2020      Deeksha       Modified for Recipe Management enhancement 
 *2.140.0   11-Jan-2022      Abhishek      WMS Fix:modified GetProductData() query to fetch only segment of product 
 ********************************************************************************************/

using System;
using System.Data;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    class UploadInventoryDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        Utilities utilities;
        public UploadInventoryDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Populate UploadInventoryProductDTO with DataTable
        /// </summary>
        /// <param name="InventoryProductDataRow"></param>
        /// <returns></returns>
        private UploadInventoryProductDTO GetUploadInventoryProductDTO(DataRow InventoryProductDataRow)
        {
            //DataRow InventoryProductDataRow = new DataRow();
            log.LogMethodEntry(InventoryProductDataRow);
            /// If the below constructor method add/remove the parameters then inventoryProductColumnCount should be updated.
            UploadInventoryProductDTO uploadInventoryProduct = new UploadInventoryProductDTO(InventoryProductDataRow["Code"].ToString(),
                                                                      InventoryProductDataRow["Product Name"].ToString(),
                                                                      InventoryProductDataRow["Description"].ToString(),
                                                                      InventoryProductDataRow["Category"].ToString(),
                                                                      InventoryProductDataRow["Price In Tickets"] == DBNull.Value ? 0 : Convert.ToDouble(InventoryProductDataRow["Price In Tickets"]),
                                                                      InventoryProductDataRow["Cost"] == DBNull.Value ? 0 : Convert.ToDouble(InventoryProductDataRow["Cost"]),
                                                                      InventoryProductDataRow["Reorder Point"] == DBNull.Value ? 0 : Convert.ToDouble(InventoryProductDataRow["Reorder Point"]),
                                                                      InventoryProductDataRow["Reorder Qty"] == DBNull.Value ? 0 : Convert.ToDouble(InventoryProductDataRow["Reorder Qty"]),
                                                                      InventoryProductDataRow["Sale Price"] == DBNull.Value ? 0 : Convert.ToDecimal(InventoryProductDataRow["Sale Price"]),
                                                                      InventoryProductDataRow["Vendor Name"].ToString(),
                                                                      InventoryProductDataRow["BarCode"].ToString(),
                                                                      InventoryProductDataRow["LotControlled"] == DBNull.Value ? false : Convert.ToBoolean(InventoryProductDataRow["LotControlled"]),
                                                                      InventoryProductDataRow["MarketListItem"] == DBNull.Value ? false : Convert.ToBoolean(InventoryProductDataRow["MarketListItem"]),
                                                                      InventoryProductDataRow["ExpiryType"].ToString(),
                                                                      InventoryProductDataRow["IssuingApproach"] == DBNull.Value ? "None" : InventoryProductDataRow["IssuingApproach"].ToString(),
                                                                      InventoryProductDataRow["ExpiryDays"] == DBNull.Value ? 0 : Convert.ToInt32(InventoryProductDataRow["ExpiryDays"]),
                                                                      InventoryProductDataRow["Remarks"].ToString(),
                                                                      InventoryProductDataRow["Redeemable"] == DBNull.Value ? "N" : InventoryProductDataRow["Redeemable"].ToString(),
                                                                      InventoryProductDataRow["Sellable"].ToString(),
                                                                      InventoryProductDataRow["UOM"] == DBNull.Value ? string.Empty : Convert.ToString(InventoryProductDataRow["UOM"]),
                                                                      InventoryProductDataRow["Item Markup %"] == DBNull.Value ? double.NaN : Convert.ToDouble(InventoryProductDataRow["Item Markup %"]),
                                                                      InventoryProductDataRow["Auto Update PIT?"] == DBNull.Value ? false : Convert.ToBoolean(InventoryProductDataRow["Auto Update PIT?"]),
                                                                      InventoryProductDataRow["Display In POS?"].ToString(),
                                                                      InventoryProductDataRow["Display Group"].ToString(),
                                                                      InventoryProductDataRow["HSN SAC Code"].ToString(),
                                                                      0,
                                                                      0,
                                                                      string.Empty,
                                                                      InventoryProductDataRow["SalesTax"].ToString(),
                                                                      InventoryProductDataRow["CostIncludesTax"] == DBNull.Value ? true : Convert.ToBoolean(InventoryProductDataRow["CostIncludesTax"]),
                                                                      InventoryProductDataRow["ItemType"].ToString(),
                                                                      InventoryProductDataRow["YieldPercentage"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(InventoryProductDataRow["YieldPercentage"]),
                                                                      InventoryProductDataRow["IncludeInPlan"] == DBNull.Value ? false : Convert.ToBoolean(InventoryProductDataRow["IncludeInPlan"]),
                                                                      InventoryProductDataRow["RecipeDescription"].ToString(),
                                                                      InventoryProductDataRow["InventoryUOM"].ToString(),
                                                                      InventoryProductDataRow["PreparationTime"] == DBNull.Value ? (int?)null : Convert.ToInt32(InventoryProductDataRow["PreparationTime"])
                                            );
            //After the 25th column their might be possibility of segments 
            int index = 0;
            /// Invenotry product fixed column count. This will be update when UploadInventoryProductDTO constuctor add/remove the parameters
            int inventoryProductColumnCount = 36;
            Dictionary<string, string> segmentDefinitionDynamicValues = new Dictionary<string, string>();
            foreach (var columnName in InventoryProductDataRow.Table.Columns)
            {
                if (index >= inventoryProductColumnCount)
                {
                    string columnValue = InventoryProductDataRow.Field<string>(columnName.ToString());
                    segmentDefinitionDynamicValues.Add(columnName.ToString(), columnValue);
                }
                index++;
            }
            uploadInventoryProduct.CustomSegmentDefinitionList = segmentDefinitionDynamicValues;
            log.LogMethodExit();
            return uploadInventoryProduct;
        }


        /// <summary>
        /// Populates the in DataTable which will further populated to excel sheet
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<UploadInventoryProductDTO> GetProductData(string siteId)
        {
            log.LogMethodEntry();
            List<UploadInventoryProductDTO> uploadInventoryProductDTOList = new List<UploadInventoryProductDTO>();

            //string siteId = machineUserContext.GetSiteId().ToString();
            string condition = " and (p.site_id = " + siteId + " or " + siteId + " = -1) ";
            string lookupCondition = " and (LookUpValues.site_id = " + siteId + " or " + siteId + " = -1) ";
            //if (utilities.ParafaitEnv.IsCorporate)
            //{
            //    condition = " and (p.site_id = @site_id or @site_id = -1)";
            //    cmd.Parameters.AddWithValue("@site_id", machineUserContext.GetSiteId().ToString());
            //}
            //31-Mar-2016
            //20-May-2016 changed delimiter to '|' instead of '||' in the query
            //Updated query to exclude Inventory Qty column 23-Feb-2017
            string selectQuery = @"DECLARE @cols AS NVARCHAR(MAX),
	                                @pivot as nvarchar(max) = '',
                                    @query  AS NVARCHAR(MAX)

                                select @cols = STUFF((SELECT ',' + QUOTENAME(SegmentName) 
                                                      from Segment_Definition where (site_id = " + siteId + " or " + siteId + " = -1) and ApplicableEntity = 'PRODUCT'" +
                                                      @"FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,1,'')
                                if(@cols is not null)
	                                set @pivot = 'pivot 
				                                 (
					                                max(valuechar)
					                                for SegmentName in  (' + @cols + N')
				                                 ) p'
                                set @query = N'select Code,ProductName ''Product Name'', Description, Category, PriceInTickets ''Price In Tickets'', 
	                                                Cost, ReorderPoint ''Reorder Point'', ReorderQuantity ''Reorder Qty'', SalePrice ''Sale Price'',
                                                    Name ''Vendor Name'', case when isnull(BarCode, '''') <> '''' then substring(BarCode, 2, len(BarCode)) else BarCode end BarCode,
                                                    LotControlled, MarketListItem, ExpiryType, IssuingApproach, ExpiryDays,
                                                    Remarks, IsRedeemable Redeemable,
	                                                IsSellable Sellable, UOM , ItemMarkupPercent ''Item Markup %'', AutoUpdateMarkup ''Auto Update PIT?'', DisplayInPOS ''Display In POS?'', display_group ''Display Group'', HsnSacCode ''HSN SAC Code'',salesTax ''SalesTax'', CostIncludesTax , 
                                                    ItemType, YieldPercentage, IncludeInPlan, RecipeDescription, InventoryUOM, PreparationTime ,null,null,null' + isnull(',' + @cols, '') + N'
                                                from(
		                                                select Code, p.ProductName, p.Description, c.Name Category, PriceInTickets, 
											                Cost, ReorderPoint, ReorderQuantity, SalePrice, DisplayInPOS, 
                                                            (select top 1 displaygroup 
																from ProductDisplayGroupFormat pdgf, ProductsDisplayGroup pdg 
																where pdgf.Id  = Displaygroupid 
																and pdg.ProductId = ps.product_id
                                                            )display_group,
                                                            HsnSacCode,
											                v.Name,
											                 (SELECT ''|''+ BarCode 
														        FROM ProductBarcode 
														        WHERE productid = p.ProductId and isactive = ''Y''
														        FOR XML PATH('''') 
															) BarCode, 
                                                        case isnull(LotControlled, 0) when 0 then 0 else 1 end LotControlled,
                                                        case isnull(MarketListItem, 0) when 0 then 0 else 1 end MarketListItem,
                                                        isnull(ExpiryType, ''N'') ExpiryType,
                                                        ExpiryDays,
                                                        isnull(IssuingApproach, ''None'') IssuingApproach, 
                                                        p.Remarks, IsRedeemable, t.tax_name as salesTax,
											                IsSellable, UOM, ItemMarkupPercent, case isnull(AutoUpdateMarkup,0) when 0 then 0
                                                        else 1 end AutoUpdateMarkup ,CostIncludesTax ,
                                                         (select description from lookupValues where lookupvalueid in(
                                                         isnull(p.ItemType , (select top 1 lookupValueId from LookUpValues where LookupValue = ''STANDARD_ITEM'' " + lookupCondition + @")))) ItemType,
														 YieldPercentage,IncludeInPlan -- case isnull(IncludeInPlan,0) when 0 then 0 else 1 end IncludeInPlan
                                                         , RecipeDescription,
														case isnull(p.InventoryUOMId , p.uomId)  when p.InventoryUOMId then (select uom from UOM where uomid = p.InventoryUOMid) else
														(select uom from UOM where uomId = p.UOMid) end
														InventoryUOM, PreparationTime ' + isnull(',' + @cols, '') + N' 
										            from product p left outer join products ps on p.ManualProductId = ps.product_id 
										                    left outer join (SELECT segmentcategoryid ' + isnull(',' + @cols, '') + N'
															                 from 
															                 (
															                    select segmentcategoryid, d.SegmentName, valuechar
															                    from Segment_Definition d join segmentdataview s on d.SegmentDefinitionId = s.SegmentDefinitionId 
															                    where d.ApplicableEntity = ''PRODUCT''
														                     ) x
														                    ' + @pivot + N'
													                        )s on p.segmentcategoryid = s.segmentcategoryid
											left outer join vendor v on p.defaultvendorid = v.VendorId left outer join tax t on t.tax_id = ps.tax_id, category c, uom u" +
                                            " where p.categoryid = c.categoryid " + condition +
                                                "and p.uomid = u.uomid)v '" +
                                            "exec sp_executesql @query ";
            //23-Mar-2016
            DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, null);

            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                uploadInventoryProductDTOList = table.Rows.Cast<DataRow>().Select(x => GetUploadInventoryProductDTO(x)).ToList();
            }

            log.LogMethodExit(uploadInventoryProductDTOList);
            return uploadInventoryProductDTOList;
        }

        /// <summary>
        /// Save the segements from Excel object 
        /// </summary>
        /// <param name="rowSegments"></param>
        /// <param name="customSegmentDTO"></param>
        /// <param name="ProductId"></param>
        /// <param name="message"></param>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool SaveSegmentData(string rowSegments, CustomSegmentDTO customSegmentDTO, int ProductId, ref string message, string siteId, string userId)
        {
            log.LogMethodEntry(rowSegments, customSegmentDTO, ProductId, message);
            int SegmentCategoryID = -1;//Added 06-May-2016
            #region
            try
            {
                using (SqlCommand cmd = utilities.getCommand())
                {
                    cmd.CommandText = "select SegmentCategoryID from Product where Productid = " + ProductId.ToString();
                    object o = cmd.ExecuteScalar();

                    if (o != DBNull.Value)
                    {
                        SegmentCategoryID = Convert.ToInt32(o);
                    }

                    using (SqlCommand cmdDMLSql = utilities.getCommand())
                    {
                        SqlTransaction SQLTrx = cmdDMLSql.Connection.BeginTransaction();
                        cmdDMLSql.Transaction = SQLTrx;
                        SqlCommand cmdSql = utilities.getCommand();
                        cmdSql.Transaction = SQLTrx;
                        if (SegmentCategoryID == -1)
                        {
                            cmdSql.CommandText = @"insert into Segment_Categorization 
                                                            (
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastupdatedDate,
                                                            Guid,
                                                            site_id) 
                                                        values 
                                                                (                                                        
                                                                @createdBy,
                                                                getDate(),
                                                                @lastUpdatedBy,
                                                                getDate(),
                                                                NewId(),
                                                                @site_id)SELECT CAST(scope_identity() AS int)";
                            cmdSql.Parameters.AddWithValue("@createdBy", userId);
                            cmdSql.Parameters.AddWithValue("@lastUpdatedBy", userId);
                            if (utilities.ParafaitEnv.IsCorporate)
                                cmdSql.Parameters.AddWithValue("@site_id", siteId);
                            else
                                cmdSql.Parameters.AddWithValue("@site_id", DBNull.Value);

                            SegmentCategoryID = Convert.ToInt32(cmdSql.ExecuteScalar());
                            cmdSql.CommandText = "update product set segmentcategoryid = " + SegmentCategoryID.ToString() + " where productid = " + ProductId.ToString();
                            cmdSql.ExecuteNonQuery();
                        }
                        try
                        {
     
                                string dataType = Convert.ToString(customSegmentDTO.DataSourceType);
                                string SegmentName = customSegmentDTO.Name.ToString();
                                int SegmentDefinitionID = Convert.ToInt32(customSegmentDTO.SegmentDefinationId);

                                if (customSegmentDTO.SegmentDefinationId != null)
                                {
                                    if (dataType.Equals("TEXT") || dataType.Equals("DATE"))
                                    {
                                        string val = Convert.ToString(rowSegments);
                                        cmdDMLSql.Parameters.Clear();
                                        if (val == "")
                                        {
                                            //Start Update 20-May-2016
                                            //Added condition to see if segment value is mandatory
                                            if (customSegmentDTO.IsMandatory == "Y")
                                            {
                                                message = SegmentName + utilities.MessageUtils.getMessage(" is a mandatory field.");
                                                SQLTrx.Rollback();
                                                log.LogVariableState("message", message);
                                                log.LogMethodExit(false);
                                                return false;
                                            }
                                            else
                                            {
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueText", DBNull.Value);
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueDate", DBNull.Value);
                                            }
                                            //End Update 20-May-2016
                                        }
                                        else
                                        {
                                            if (dataType == "TEXT")
                                            {
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueText", val);
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueDate", DBNull.Value);
                                            }
                                            else if (dataType == "DATE")
                                            {
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueText", DBNull.Value);
                                                try
                                                {
                                                    cmdDMLSql.Parameters.AddWithValue("@SegmentValueDate", Convert.ToDateTime(val));
                                                }
                                                catch
                                                {
                                                    message = utilities.MessageUtils.getMessage(15, " :") + val;
                                                    SQLTrx.Rollback();
                                                    log.LogVariableState("message", message);
                                                    log.LogMethodExit(false);
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueText", val);
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueDate", DBNull.Value);
                                            }
                                        }
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentCategoryId", SegmentCategoryID);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentDefinitionId", SegmentDefinitionID);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentStaticValueId", DBNull.Value);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentDynamicValueId", DBNull.Value);
                                        cmdDMLSql.Parameters.AddWithValue("@LastUpdatedBy", utilities.ParafaitEnv.Username);

                                        cmdDMLSql.CommandText = "update segment_categorization_values set SegmentStaticValueId = @SegmentStaticValueId, SegmentValueText = @SegmentValueText, SegmentValueDate = @SegmentValueDate, " +
                                                        " LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = getdate() " +
                                                        "where SegmentCategoryId = @SegmentCategoryId and SegmentDefinitionID = @SegmentDefinitionId";

                                        if (cmdDMLSql.ExecuteNonQuery() == 0)
                                        {
                                            cmdDMLSql.CommandText = "insert into segment_categorization_values (SegmentCategoryId, SegmentDefinitionID, SegmentStaticValueId, SegmentValueText, SegmentValueDate, site_id, CreatedBy, CreationDate, LastUpdatedBy, LastUpdatedDate, IsActive) " +
                                                        "values (@SegmentCategoryId, @SegmentDefinitionId, @SegmentStaticValueId, @SegmentValueText, @SegmentValueDate, @site_id, @CreatedBy, getdate(), @LastUpdatedBy, getdate(), 'Y')";
                                            cmdDMLSql.Parameters.AddWithValue("@CreatedBy", utilities.ParafaitEnv.Username);
                                            //cmdDMLSql.Parameters.AddWithValue("@LastUpdatedBy", utilities.ParafaitEnv.Username);
                                            if (utilities.ParafaitEnv.IsCorporate)
                                                cmdDMLSql.Parameters.AddWithValue("@site_id", siteId);
                                            else
                                                cmdDMLSql.Parameters.AddWithValue("@site_id", DBNull.Value);

                                            cmdDMLSql.ExecuteNonQuery();
                                        }
                                    }
                                    else if (dataType.Contains("LIST"))
                                    {
                                        if (dataType.Equals("STATIC LIST"))
                                        {
                                            cmdDMLSql.Parameters.Clear();
                                            cmdDMLSql.Parameters.AddWithValue("@SegmentDynamicValueId", DBNull.Value);
                                            if (string.IsNullOrEmpty(rowSegments))
                                            {
                                                //Start Update 20-May-2016
                                                //Added condition to see if segment value is mandatory
                                                if (customSegmentDTO.IsMandatory == "Y")
                                                {
                                                    message = SegmentName + utilities.MessageUtils.getMessage(" is a mandatory field.");
                                                    SQLTrx.Rollback();
                                                    log.LogVariableState("message", message);
                                                    log.LogMethodExit(false);
                                                    return false;
                                                }
                                                else
                                                {
                                                    cmdDMLSql.Parameters.AddWithValue("@SegmentStaticValueId", DBNull.Value);
                                                }
                                                //End Update 20-May-2016
                                            }
                                            else
                                            {
                                                SqlCommand cmdValues = utilities.getCommand();
                                                cmdValues.Transaction = SQLTrx;
                                                cmdValues.CommandText = @"select segmentdefinitionsourceValueid
                                                                from segment_definition_source_mapping m, segment_definition_source_values s
                                                                where m.SegmentDefinitionSourceId = s.SegmentDefinitionSourceId
	                                                                and segmentdefinitionid = @segmentdefinitionid
	                                                                and listvalue = @ListValue";
                                                cmdValues.Parameters.AddWithValue("@SegmentDefinitionId", SegmentDefinitionID);
                                                cmdValues.Parameters.AddWithValue("@ListValue", rowSegments);
                                                o = cmdValues.ExecuteScalar();
                                                if (o != null && o != DBNull.Value)
                                                {
                                                    cmdDMLSql.Parameters.AddWithValue("@SegmentStaticValueId", Convert.ToInt32(o));
                                                }
                                                else
                                                {
                                                    message = utilities.MessageUtils.getMessage("Invalid value for Segment ") + SegmentName;
                                                    SQLTrx.Rollback();
                                                    log.LogVariableState("message", message);
                                                    log.LogMethodExit(false);
                                                    return false;
                                                }
                                            }
                                        }
                                        else if (dataType.Equals("DYNAMIC LIST"))
                                        {
                                            cmdDMLSql.Parameters.Clear();
                                            cmdDMLSql.Parameters.AddWithValue("@SegmentStaticValueId", DBNull.Value);
                                            if (string.IsNullOrEmpty(rowSegments))
                                            {
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentDynamicValueId", DBNull.Value);
                                            }
                                            else
                                            {
                                                SqlCommand cmdValues = utilities.getCommand();
                                                cmdValues.Transaction = SQLTrx;
                                                cmdValues.CommandText = @"select DBQuery, m.DataSourceEntity, m.DataSourceColumn
                                                                from segment_definition_source_mapping m, segment_definition_source_values s
                                                                where m.SegmentDefinitionSourceId = s.SegmentDefinitionSourceId
	                                                                and segmentdefinitionid = @SegmentDefinitionId
	                                                                and m.isactive = 'Y'
	                                                                and s.isactive = 'Y'";
                                                cmdValues.Parameters.Clear();
                                                cmdValues.Parameters.AddWithValue("@SegmentDefinitionId", SegmentDefinitionID);
                                                DataTable dt = new DataTable();
                                                SqlDataAdapter da = new SqlDataAdapter(cmdValues);
                                                da.Fill(dt);
                                                if (dt.Rows.Count > 0)
                                                {
                                                    if (!string.IsNullOrEmpty(dt.Rows[0]["DBQuery"].ToString()) && !string.IsNullOrEmpty(dt.Rows[0]["DataSourceEntity"].ToString()) && !string.IsNullOrEmpty(dt.Rows[0]["DataSourceColumn"].ToString()))
                                                    {
                                                        string query = "select " + dt.Rows[0]["DataSourceColumn"].ToString() +
                                                                       " from " + dt.Rows[0]["DataSourceEntity"].ToString() + " " + dt.Rows[0]["DataSourceEntity"].ToString().Substring(0, 1) +
                                                                       " where " + dt.Rows[0]["DBQuery"].ToString() + " and " + dt.Rows[0]["DataSourceColumn"].ToString() + " = '" + rowSegments + "'";
                                                        cmdValues.CommandText = query;
                                                        o = cmdValues.ExecuteScalar();
                                                        if (o != null && o != DBNull.Value)
                                                            cmdDMLSql.Parameters.AddWithValue("@SegmentDynamicValueId", Convert.ToString(o));
                                                        else
                                                        {
                                                            message = utilities.MessageUtils.getMessage("Invalid value for Segment ") + SegmentName;
                                                            SQLTrx.Rollback();
                                                            log.LogVariableState("message", message);
                                                            log.LogMethodExit(false);
                                                            return false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        message = utilities.MessageUtils.getMessage("Invalid value for Segment ") + SegmentName;
                                                        SQLTrx.Rollback();
                                                        log.LogVariableState("message", message);
                                                        log.LogMethodExit(false);
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    message = utilities.MessageUtils.getMessage("Invalid value for Segment ") + SegmentName;
                                                    log.LogVariableState("message", message);
                                                    SQLTrx.Rollback();
                                                    log.LogMethodExit(false);
                                                    return false;
                                                }
                                            }
                                        }
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentCategoryId", SegmentCategoryID);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentDefinitionId", SegmentDefinitionID);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentValueText", DBNull.Value);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentValueDate", DBNull.Value);

                                        cmdDMLSql.CommandText = "update segment_categorization_values set SegmentStaticValueId = @SegmentStaticValueId, SegmentDynamicValueId = @SegmentDynamicValueId, SegmentValueText = @SegmentValueText, SegmentValueDate = @SegmentValueDate " +
                                                        "where SegmentCategoryId = @SegmentCategoryId and SegmentDefinitionId = @SegmentDefinitionId";

                                        if (cmdDMLSql.ExecuteNonQuery() == 0)
                                        {
                                            cmdDMLSql.CommandText = "insert into segment_categorization_values (SegmentDefinitionId, SegmentCategoryId, SegmentStaticValueId, SegmentDynamicValueId, SegmentValueText, SegmentValueDate, site_id, CreatedBy, CreationDate, LastUpdatedBy, LastUpdatedDate, IsActive) " +
                                                        "values (@SegmentDefinitionId, @SegmentCategoryId, @SegmentStaticValueId, @SegmentDynamicValueId, @SegmentValueText, @SegmentValueDate, @site_id, @CreatedBy, getdate(), @LastUpdatedBy, getdate(), 'Y')";
                                            cmdDMLSql.Parameters.AddWithValue("@CreatedBy", userId);
                                            cmdDMLSql.Parameters.AddWithValue("@LastUpdatedBy", userId);
                                            if (utilities.ParafaitEnv.IsCorporate)
                                                cmdDMLSql.Parameters.AddWithValue("@site_id", siteId);
                                            else
                                                cmdDMLSql.Parameters.AddWithValue("@site_id", DBNull.Value);
                                            cmdDMLSql.ExecuteNonQuery();
                                        }
                                    }
                                }
                            SQLTrx.Commit();
                            log.LogMethodExit(true);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            message = ex.Message;
                            SQLTrx.Rollback();
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = "Error: " + ex.Message;
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit();
            #endregion
        }//End update 06-May-2016
    }
}
