/********************************************************************************************
 * Project Name - PhysicalCountReview
 * Description  - Data Handler of PhysicalCountReview
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        19-Aug-2019   Deeksha          Added logger methods.
 *2.70.2        25-Dec-2019   Deeksha          Inventory Next-Rel Enhancement changes.
 *2.90.0        28-Jul-2020   Deeksha          Physical Count Lot controllable products Issue fix
 *2.100.0       07-Aug-2020   Deeksha          Modified for Recipe management enhancement
 *2.110.0       04-Jan-2021   Mushahid Faizan  Modified : Web Inventory Changes
 ********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory.PhysicalCount
{
    /// <summary>
    /// class of PhysicalCountReviewDataHandler
    /// </summary>
    public class PhysicalCountReviewDataHandler
    {
        private SqlTransaction sqlTransaction;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SqlParameter> selectInventoryParameters = new List<SqlParameter>();

        private static readonly Dictionary<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string> DBSearchParameters = new Dictionary<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>
        {
                {PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.SITE_ID, "site_id"},
                {PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.LOCATIONID, "LocationID"},
                {PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.PRODUCT_ID, "ProductID"},
                {PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.INVENTORYITEMSONLY, "IsPurchaseable"},
                {PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.CODE, "Code"},
                {PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.DESCRIPTION, "Description"},
                {PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.BARCODE, "Barcode"},
                {PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.CATEGORYID, "CategoryID"},
                {PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.INVENTORYQTY, "CurrentInventoryQuantity"},
                {PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.UOM_ID, "UomId"}
        };
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of PhysicalCountReviewDataHandler class
        /// </summary>
        public PhysicalCountReviewDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to InventoryPhysicalCountDTO class type
        /// </summary>
        /// <param name="physicalCountReviewDataRow">InventoryPhysicalCountDTO DataRow</param>
        /// <returns>Returns InventoryPhysicalCountDTO</returns>
        private PhysicalCountReviewDTO GetPhysicalCountReviewDTO(DataRow physicalCountReviewDataRow)
        {
            log.LogMethodEntry(physicalCountReviewDataRow);
            PhysicalCountReviewDTO physicalCountReviewDataObject = new PhysicalCountReviewDTO(physicalCountReviewDataRow["Code"].ToString(),
                                                    physicalCountReviewDataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(physicalCountReviewDataRow["Description"]),
                                                    physicalCountReviewDataRow["Category"] == DBNull.Value ? "" : physicalCountReviewDataRow["Category"].ToString(),
                                                    physicalCountReviewDataRow["Barcode"] == DBNull.Value ? "" : physicalCountReviewDataRow["Barcode"].ToString(),
                                                    physicalCountReviewDataRow["SKU"] == DBNull.Value ? string.Empty : Convert.ToString(physicalCountReviewDataRow["SKU"]),
                                                    physicalCountReviewDataRow["LotNumber"] == DBNull.Value ? "" : physicalCountReviewDataRow["LotNumber"].ToString(),
                                                    physicalCountReviewDataRow["Location"] == DBNull.Value ? string.Empty : Convert.ToString(physicalCountReviewDataRow["Location"]),
                                                    physicalCountReviewDataRow["StartingQuantity"] == DBNull.Value ? 0 : Convert.ToDouble(physicalCountReviewDataRow["StartingQuantity"]),
                                                    physicalCountReviewDataRow["UpdatedPhysicalQuantity"] == DBNull.Value ? 0 : Convert.ToDouble(physicalCountReviewDataRow["UpdatedPhysicalQuantity"]),
                                                    physicalCountReviewDataRow["CurrentInventoryQuantity"] == DBNull.Value ? 0 : Convert.ToDouble(physicalCountReviewDataRow["CurrentInventoryQuantity"]),
                                                    physicalCountReviewDataRow["NewQuantity"] == DBNull.Value ? -1 : Convert.ToDouble(physicalCountReviewDataRow["NewQuantity"]),
                                                    physicalCountReviewDataRow["PhysicalCountRemarks"] == DBNull.Value ? "" : physicalCountReviewDataRow["physicalCountRemarks"].ToString(),
                                                    physicalCountReviewDataRow["ProductID"] == DBNull.Value ? -1 : Convert.ToInt32(physicalCountReviewDataRow["ProductID"]),
                                                    physicalCountReviewDataRow["LocationID"] == DBNull.Value ? -1 : Convert.ToInt32(physicalCountReviewDataRow["LocationID"]),
                                                    physicalCountReviewDataRow["LotID"] == DBNull.Value ? -1 : Convert.ToInt32(physicalCountReviewDataRow["LotID"]),
                                                    physicalCountReviewDataRow["Site_ID"] == DBNull.Value ? -1 : Convert.ToInt32(physicalCountReviewDataRow["Site_ID"]),
                                                    physicalCountReviewDataRow["CategoryID"] == DBNull.Value ? -1 : Convert.ToInt32(physicalCountReviewDataRow["CategoryID"]),
                                                    physicalCountReviewDataRow["IsPurchaseable"] == DBNull.Value ? "N" : physicalCountReviewDataRow["IsPurchaseable"].ToString(),
                                                    physicalCountReviewDataRow["RemarksMandatory"] == DBNull.Value ? "N" : physicalCountReviewDataRow["RemarksMandatory"].ToString(),
                                                    physicalCountReviewDataRow["ID"] == DBNull.Value ? -1 : Convert.ToInt32(physicalCountReviewDataRow["ID"]),
                                                    physicalCountReviewDataRow["LotControlled"] == DBNull.Value ? false : Convert.ToBoolean(physicalCountReviewDataRow["LotControlled"]),
                                                    physicalCountReviewDataRow["UOM"] == DBNull.Value ? string.Empty : Convert.ToString(physicalCountReviewDataRow["UOM"]),
                                                    physicalCountReviewDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(physicalCountReviewDataRow["UOMId"]),
                                                    physicalCountReviewDataRow["PhysicalCountId"] == DBNull.Value ? -1 : Convert.ToInt32(physicalCountReviewDataRow["PhysicalCountId"])
                                                    );
            log.LogMethodExit(physicalCountReviewDataObject);
            return physicalCountReviewDataObject;
        }


        /// <summary>
        /// Gets the InventoryPhysicalCountDTO list matching the search key
        /// </summary>
        /// <returns>Returns the list of InventoryPhysicalCountDTO matching the search criteria</returns>
        public List<PhysicalCountReviewDTO> GetPhysicalCountReviewsList(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters, int physicalCountId, DateTime startDate, int locationId)
        {
            log.LogMethodEntry(searchParameters, physicalCountId, startDate, locationId);
            List<PhysicalCountReviewDTO> physicalCountReviewList = null;
            int count = 0;
            selectInventoryParameters.Clear();
            //Updated query 13-Jun-2017
            string selectInventoryPhysicalCountQuery = @"select *
                                                         from (
                                                                select code,
			                                                        description,
																	c.name category,
																	(select isnull(v.Barcode, '')
						                                                        from (
							                                                        select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
							                                                        from productbarcode 
							                                                        where isactive = 'Y'
								                                                            and productid = p.productid)v 
						                                                        where num = 1) Barcode,
			                                                        STUFF((SELECT '.'+ valuechar 
					                                                        FROM segmentdataview 
					                                                        WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
					                                                        order by segmentdefinitionid 
					                                                        FOR XML PATH('')),1,1,'') SKU,
			                                                        il.LotNumber,
			                                                        l.Name Location,
			                                                        StartingQuantity.Quantity StartingQuantity,
																	case isnull(BulkUpdated.BulkUpdated, 0) when 1 then isnull(BulkUpdated.BulkUpdatedQuantity, 0) + isnull(StartingQuantity.Quantity, 0) else 0 end updatedPhysicalQuantity,
																	i.Quantity currentInventoryQuantity,
																	-1 NewQuantity,
																	'' physicalCountRemarks,
			                                                        i.ProductId,
			                                                        i.LocationId,
			                                                        i.LotId,
                                                                    i.Site_ID,
																	p.CategoryId,
                                                                    p.IsPurchaseable,
                                                                    l.RemarksMandatory,
                                                                    p.LotControlled
                                                        from Location l, Product p left outer join Category c on c.CategoryId = p.categoryid, inventory i left outer join InventoryLot il on il.LotId = i.lotid
		                                                        left outer join(select quantity, productid, locationid, lotid
						                                                            from InventoryHist
						                                                            where InitialCount = 1
							                                                        and PhysicalCountId = @physicalCountID
						                                                        )StartingQuantity on i.ProductId = StartingQuantity.productid and StartingQuantity.LocationId = i.LocationId and isnull(StartingQuantity.lotid, -1) = ISNULL(i.LotId, -1)
		                                                        left outer join (select sum(BulkUpdatedQuantity) BulkUpdatedQuantity, FromLocationId, productid, lotid, 1 bulkupdated
																					from (
																							select sum(case when bulkupdated = 1 then adjustmentquantity 
																												when adjustmentType = 'Transfer' then
																													case when fromlocationid = ia.FromLocationId then -1 * AdjustmentQuantity 
																													end
																												else AdjustmentQuantity
																											 end) BulkUpdatedQuantity, 
																									FromLocationId, 
																									productid, 
																									lotid
																							from InventoryAdjustments ia
																							where exists ( select FromLocationId, productid, lotid, max(timestamp) timestamp
																											from inventoryadjustments
																											where bulkupdated = 1
																												and Timestamp > @timestamp
																												and (FromLocationId = @LocationID or @LocationID = -1)
																												and FromLocationId = ia.FromLocationId
																												and isnull(lotid, -1) = isnull(ia.lotid, -1)
																											group by FromLocationId, productid, lotid)
																								and Timestamp > @timestamp and Timestamp <= (select max(timestamp)
																																			 from inventoryadjustments
																																				where bulkupdated = 1
																																					and Timestamp > @timestamp
																																					and (FromLocationId = @LocationID or @LocationID = -1)
																																					and FromLocationId = ia.FromLocationId
																																					and isnull(lotid, -1) = isnull(ia.lotid, -1))
																							group by FromLocationId, productid, lotid
																							union all
																							select sum(case when bulkupdated = 1 then adjustmentquantity 
																												when adjustmentType = 'Transfer' then
																													case when tolocationid = ia.tolocationid then AdjustmentQuantity 
																													end
																												else AdjustmentQuantity
																											 end) BulkUpdatedQuantity, 
																									ToLocationId, 
																									productid, 
																									lotid
																							from InventoryAdjustments ia
																							where exists ( select FromLocationId, productid, lotid, max(timestamp) timestamp
																											from inventoryadjustments
																											where bulkupdated = 1
																												and Timestamp > @timestamp
																												and (FromLocationId = @LocationID or @LocationID = -1)
																												and FromLocationId = ia.ToLocationId
																												and isnull(lotid, -1) = isnull(ia.lotid, -1)
																											group by FromLocationId, productid, lotid)
																								and Timestamp > @timestamp and Timestamp <= (select max(timestamp)
																																			 from inventoryadjustments
																																				where bulkupdated = 1
																																					and Timestamp > @timestamp
																																					and (FromLocationId = @LocationID or @LocationID = -1)
																																					and FromLocationId = ia.ToLocationId
																																					and isnull(lotid, -1) = isnull(ia.lotid, -1))
																							group by ToLocationId, productid, lotid
																					)v
																					group by FromLocationId, productid, lotid
						                                                        ) BulkUpdated  on i.ProductId = BulkUpdated.productid and BulkUpdated.FromLocationId = i.LocationId and isnull(BulkUpdated.lotid, -1) = ISNULL(i.LotId, -1) 
														where i.ProductId = p.ProductId
	                                                        and i.LocationId = l.LocationId
	                                                        and l.MassUpdateAllowed = 'Y'
                                                            and p.IsActive = 'Y'
                                                            and (i.locationid = @LocationID or @LocationID = -1)
                                                        )v";
            //List<SqlParameter> selectInventoryParameters = new List<SqlParameter>();
            selectInventoryParameters.Add(new SqlParameter("@timestamp", startDate));
            selectInventoryParameters.Add(new SqlParameter("@LocationID", locationId));
            selectInventoryParameters.Add(new SqlParameter("@physicalCountID", physicalCountId));

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.LOCATIONID
                            || searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.PRODUCT_ID
                            || searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.CATEGORYID
                           )
                        {
                            //query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            selectInventoryParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        if (searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.CODE
                            || searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.BARCODE
                            || searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.INVENTORYITEMSONLY
                            || searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.DESCRIPTION
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            selectInventoryParameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.INVENTORYQTY)
                        {
                            //query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            selectInventoryParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDecimal(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.SITE_ID)
                        {
                            //query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            selectInventoryParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else
                        {
                            //query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'~') like " + "'%" + searchParameter.Value + "%'");
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            selectInventoryParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        //log.Debug("Ends-GetPhysicalCountReviewList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        //throw new Exception("The query parameter does not exist " + searchParameter.Key);
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }

                if (searchParameters.Count > 0)
                    selectInventoryPhysicalCountQuery = selectInventoryPhysicalCountQuery + query;
                selectInventoryPhysicalCountQuery = selectInventoryPhysicalCountQuery + " Order by PhysicalCountId";
            }
            DataTable inventoryPhysicalCountData = dataAccessHandler.executeSelectQuery(selectInventoryPhysicalCountQuery, selectInventoryParameters.ToArray(), sqlTransaction);
            if (inventoryPhysicalCountData.Rows.Count > 0)
            {
                physicalCountReviewList = new List<PhysicalCountReviewDTO>();
                foreach (DataRow inventoryPhysicalCountDataRow in inventoryPhysicalCountData.Rows)
                {
                    PhysicalCountReviewDTO physicalCountReviewDataObject = GetPhysicalCountReviewDTO(inventoryPhysicalCountDataRow);
                    physicalCountReviewList.Add(physicalCountReviewDataObject);
                }
            }
            log.LogMethodExit(physicalCountReviewList);
            return physicalCountReviewList;
        }

        /// <summary>
        /// Gets the InventoryDTO list matching the search key
        /// </summary>
        ///<param name="filterCondition"></param>
        ///<param name="LocationID"></param>
        ///<param name="PhysicalCountID"></param>
        ///<param name="StartDate"></param>
        /// <returns>Returns the list of InventoryDTO matching the search criteria</returns>
        private string GetFilterQuery(string filterCondition, int PhysicalCountID, DateTime StartDate, int LocationID, ExecutionContext executionContext)
        {
            log.LogMethodEntry(filterCondition, PhysicalCountID, StartDate, LocationID, executionContext);
            string filterQuery = filterCondition == null ? string.Empty : filterCondition.ToUpper();
            if (filterQuery.Contains("DROP") || filterQuery.Contains("UPDATE ") || filterQuery.Contains("DELETE"))
            {
                log.LogMethodExit();
                return null;
            }

            string selectInventoryQuery;

            SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(executionContext);
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "Y"));
            segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionSearchParams);

            if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any())
            {
                List<string> segmentNameList = segmentDefinitionDTOList.Select(x => x.SegmentName.Trim().ToLower()).ToList();
                if (segmentDefinitionDTOList.Count != segmentNameList.Distinct().Count())
                {
                    log.Error("Duplicate segment name exists.Failed to proceed");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2704));
                }
                string pivotColumns = "";
                foreach (SegmentDefinitionDTO sd in segmentDefinitionDTOList)
                {
                    pivotColumns += ", [" + sd.SegmentName + "]";
                }
                selectInventoryQuery = "select * " +
                                            pivotColumns +
                                     @"from (
                                                select code,
			                                        description,
													c.name category,
													(select isnull(v.Barcode, '')
						                                        from (
							                                        select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
							                                        from productbarcode 
							                                        where isactive = 'Y'
								                                            and productid = p.productid)v 
						                                        where num = 1) Barcode,
			                                        STUFF((SELECT '.'+ valuechar 
					                                        FROM segmentdataview 
					                                        WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
					                                        order by segmentdefinitionid 
					                                        FOR XML PATH('')),1,1,'') SKU,
			                                        il.LotNumber,
			                                        l.Name Location,
			                                        StartingQuantity.Quantity StartingQuantity,
													case isnull(BulkUpdated.BulkUpdated, 0) when 1 then isnull(BulkUpdated.BulkUpdatedQuantity, 0) + isnull(StartingQuantity.Quantity, 0) + isnull(PQuantity.quantity ,0)  else 0 end updatedPhysicalQuantity,
													isnull(StartingQuantity.Quantity, 0) - isnull(i.quantity, 0) Difference,
			                                        i.Quantity currentInventoryQuantity,
													null NewQuantity,
													'' physicalCountRemarks,
			                                        i.ProductId,
			                                        i.LocationId,
			                                        i.LotId,
                                                    i.Site_ID,
													p.CategoryId,
                                                    p.IsPurchaseable,
                                                    l.RemarksMandatory,
												    segmentname,
												    valuechar,
                                                    StartingQuantity.ID,
                                                    p.LotControlled,
                                                    i.UOMId,
													u.UOM,
                                                    StartingQuantity.PhysicalCountId
                                        from Location l, Product p left outer join (select * 
					                            from (
													    select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
													    from productbarcode 
													    where isactive = 'Y')v 
											    where num = 1) b on p.productid = b.productid 
                                                        left outer join Category c on c.CategoryId = p.categoryid
													    left outer join SegmentDataView sdv on sdv.SegmentCategoryId = p.segmentcategoryid,
                                                        inventory i left outer join InventoryLot il on il.LotId = i.lotid left outer join UOM u on i.UOMId = u.UOMId
                                                        
                                                left outer join(select quantity, productid, locationid, lotid
						                                            from InventoryHist
						                                            where InitialCount = 1
							                                        and PhysicalCountId = @physicalCountID
						                                        )SystemCount on i.ProductId = SystemCount.productid and SystemCount.LocationId = i.LocationId and isnull(SystemCount.lotid, -1) = ISNULL(i.LotId, -1)
                                                
		                                        left outer join(select quantity, productid, locationid, lotid ,ID, PhysicalCountId
						                                        from InventoryHist
						                                        where InitialCount = 1
							                                    and PhysicalCountId = @physicalCountID
						                                    )StartingQuantity on i.ProductId = StartingQuantity.productid and StartingQuantity.LocationId = i.LocationId and isnull(StartingQuantity.lotid, -1) = ISNULL(i.LotId, -1)

                                                  left outer join(select sum( quantity) quantity, productid, locationid
						                                        from ProductActivityView
						                                        where LocationId = @LocationID
																and TrxType in ('Redemption' ,'Sales','Receipts')
							                                    and TimeStamp > @timestamp
																group by ProductId , LocationId
						                                    )PQuantity on i.ProductId = PQuantity.productid 

		                                        left outer join (select sum(BulkUpdatedQuantity) BulkUpdatedQuantity, FromLocationId, productid, lotid, 1 bulkupdated
																from (
																		select sum(case when bulkupdated = 1 then adjustmentquantity 
																							when adjustmentType = 'Transfer' then
																								case when fromlocationid = ia.FromLocationId then -1 * AdjustmentQuantity 
																								end
																							else AdjustmentQuantity
																							end) BulkUpdatedQuantity, 
																				FromLocationId, 
																				productid, 
																				lotid
																		from InventoryAdjustments ia
																		where exists ( select FromLocationId, productid, lotid, max(timestamp) timestamp
																						from inventoryadjustments
																						where bulkupdated = 1
																							and Timestamp > @timestamp
																							and (FromLocationId = @LocationID or @LocationID = -1)
																							and FromLocationId = ia.FromLocationId
																							and isnull(lotid, -1) = isnull(ia.lotid, -1)
																						group by FromLocationId, productid, lotid)
																			and Timestamp > @timestamp and Timestamp <= (select max(timestamp)
																															from inventoryadjustments
																															where bulkupdated = 1
																																and Timestamp > @timestamp
																																and (FromLocationId = @LocationID or @LocationID = -1)
																																and FromLocationId = ia.FromLocationId
																																and isnull(lotid, -1) = isnull(ia.lotid, -1))
                                                                            and NOT exists (SELECT 1 --Ignore records that are created before hist entry for the product
                                                                                              from inventoryHist ihin 
                                                                                             where ihin.physicalCountId = @physicalCountID 
                                                                                               and ihin.productId = ia.productid 
                                                                                               and ihin.InitialCount = 1
                                                                                               and ia.timestamp < ihin.timestamp )
																		group by FromLocationId, productid, lotid
																		union all
																		select sum(case when bulkupdated = 1 then adjustmentquantity 
																							when adjustmentType = 'Transfer' then
																								case when tolocationid = ia.tolocationid then AdjustmentQuantity 
																								end
																							else AdjustmentQuantity
																							end) BulkUpdatedQuantity, 
																				ToLocationId, 
																				productid, 
																				lotid
																		from InventoryAdjustments ia
																		where exists ( select FromLocationId, productid, lotid, max(timestamp) timestamp
																						from inventoryadjustments
																						where bulkupdated = 1
																							and Timestamp > @timestamp
																							and (FromLocationId = @LocationID or @LocationID = -1)
																							and FromLocationId = ia.ToLocationId
																							and isnull(lotid, -1) = isnull(ia.lotid, -1)
																						group by FromLocationId, productid, lotid)
																			and Timestamp > @timestamp and Timestamp <= (select max(timestamp)
																															from inventoryadjustments
																															where bulkupdated = 1
																																and Timestamp > @timestamp
																																and (FromLocationId = @LocationID or @LocationID = -1)
																																and FromLocationId = ia.ToLocationId
																																and isnull(lotid, -1) = isnull(ia.lotid, -1))
                                                                             and NOT exists (SELECT 1 --Ignore records that are created before hist entry for the product
                                                                                               from inventoryHist ihin 
                                                                                              where ihin.physicalCountId = @physicalCountID 
                                                                                                and ihin.productId = ia.productid 
                                                                                                and ihin.InitialCount = 1
                                                                                                and ia.timestamp < ihin.timestamp )
																		group by ToLocationId, productid, lotid
																)v
																group by FromLocationId, productid, lotid
						                                    ) BulkUpdated  on i.ProductId = BulkUpdated.productid and BulkUpdated.FromLocationId = i.LocationId and isnull(BulkUpdated.lotid, -1) = ISNULL(i.LotId, -1) 
                                        where i.ProductId = p.ProductId
	                                        and i.LocationId = l.LocationId
	                                        and l.MassUpdateAllowed = 'Y'
                                            and p.IsActive = 'Y'
                                            and (i.locationid = @LocationID or @LocationID = -1)
			                            )p 
                                    PIVOT 
							        ( max(valuechar) for segmentname in " + "(" + pivotColumns.Substring(2) + ")" + ")  as v2  ";
            }
            else
            {
                selectInventoryQuery = @"select *
                                                         from (
                                                                    select code,
			                                                        description,
																	c.name category,
																	(select isnull(v.Barcode, '')
						                                                        from (
							                                                        select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
							                                                        from productbarcode 
							                                                        where isactive = 'Y'
								                                                            and productid = p.productid)v 
						                                                        where num = 1) Barcode,
			                                                        STUFF((SELECT '.'+ valuechar 
					                                                        FROM segmentdataview 
					                                                        WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
					                                                        order by segmentdefinitionid 
					                                                        FOR XML PATH('')),1,1,'') SKU,
			                                                        il.LotNumber,
			                                                        l.Name Location,
			                                                        StartingQuantity.Quantity StartingQuantity,
																	case isnull(BulkUpdated.BulkUpdated, 0) when 1 then isnull(BulkUpdated.BulkUpdatedQuantity, 0) + isnull(StartingQuantity.Quantity, 0)+ isnull(PQuantity.quantity ,0)  else 0 end updatedPhysicalQuantity,
																	isnull(StartingQuantity.Quantity, 0) - isnull(i.quantity, 0) Difference,
			                                                        i.Quantity currentInventoryQuantity,
																	null NewQuantity,
																	'' physicalCountRemarks,
			                                                        i.ProductId,
			                                                        i.LocationId,
			                                                        i.LotId,
                                                                    i.Site_ID,
																	p.CategoryId,
                                                                    p.IsPurchaseable,
                                                                    l.RemarksMandatory,
                                                                    StartingQuantity.ID,
                                                                    p.LotControlled,
                                                                    i.UOMId,
													                u.UOM,
                                                                    StartingQuantity.PhysicalCountId
                                                        from Location l, Product p left outer join Category c on c.CategoryId = p.categoryid
                                                                           , inventory i left outer join InventoryLot il on il.LotId = i.lotid  left outer join UOM u on i.UOMId = u.uomId
		                                                        left outer join(select quantity, productid, locationid, lotid,ID, PhysicalCountId
						                                                            from InventoryHist
						                                                            where InitialCount = 1
							                                                        and PhysicalCountId = @physicalCountID
						                                                        )StartingQuantity on i.ProductId = StartingQuantity.productid and StartingQuantity.LocationId = i.LocationId and isnull(StartingQuantity.lotid, -1) = ISNULL(i.LotId, -1)
                                                                
                                                                left outer join(select sum( quantity) quantity, productid, locationid
						                                                    from ProductActivityView
						                                                    where LocationId = @LocationID
															                and TrxType in ('Redemption' ,'Sales','Receipts')
							                                                and TimeStamp > @timestamp
															                group by ProductId , LocationId
						                                                )PQuantity on i.ProductId = PQuantity.productid 
                                                                
		                                                        left outer join (select sum(BulkUpdatedQuantity) BulkUpdatedQuantity, FromLocationId, productid, lotid, 1 bulkupdated
																from (
																		select sum(case when bulkupdated = 1 then adjustmentquantity 
																							when adjustmentType = 'Transfer' then
																								case when fromlocationid = ia.FromLocationId then -1 * AdjustmentQuantity 
																								end
																							else AdjustmentQuantity
																							end) BulkUpdatedQuantity, 
																				FromLocationId, 
																				productid, 
																				lotid
																		from InventoryAdjustments ia
																		where exists ( select FromLocationId, productid, lotid, max(timestamp) timestamp
																						from inventoryadjustments
																						where bulkupdated = 1
																							and Timestamp > @timestamp
																							and (FromLocationId = @LocationID or @LocationID = -1)
																							and FromLocationId = ia.FromLocationId
																							and isnull(lotid, -1) = isnull(ia.lotid, -1)
																						group by FromLocationId, productid, lotid)
																			and Timestamp > @timestamp and Timestamp <= (select max(timestamp)
																															from inventoryadjustments
																															where bulkupdated = 1
																																and Timestamp > @timestamp
																																and (FromLocationId = @LocationID or @LocationID = -1)
																																and FromLocationId = ia.FromLocationId
																																and isnull(lotid, -1) = isnull(ia.lotid, -1))
                                                                        and NOT exists (SELECT 1 --Ignore records that are created before hist entry for the product
                                                                                              from inventoryHist ihin 
                                                                                             where ihin.physicalCountId = @physicalCountID 
                                                                                               and ihin.productId = ia.productid 
                                                                                               and ihin.InitialCount = 1
                                                                                               and ia.timestamp < ihin.timestamp )
																		group by FromLocationId, productid, lotid
																		union all
																		select sum(case when bulkupdated = 1 then adjustmentquantity 
																							when adjustmentType = 'Transfer' then
																								case when tolocationid = ia.tolocationid then AdjustmentQuantity 
																								end
																							else AdjustmentQuantity
																							end) BulkUpdatedQuantity, 
																				ToLocationId, 
																				productid, 
																				lotid
																		from InventoryAdjustments ia
																		where exists ( select FromLocationId, productid, lotid, max(timestamp) timestamp
																						from inventoryadjustments
																						where bulkupdated = 1
																							and Timestamp > @timestamp
																							and (FromLocationId = @LocationID or @LocationID = -1)
																							and FromLocationId = ia.ToLocationId
																							and isnull(lotid, -1) = isnull(ia.lotid, -1)
																						group by FromLocationId, productid, lotid)
																			and Timestamp > @timestamp and Timestamp <= (select max(timestamp)
																															from inventoryadjustments
																															where bulkupdated = 1
																																and Timestamp > @timestamp
																																and (FromLocationId = @LocationID or @LocationID = -1)
																																and FromLocationId = ia.ToLocationId
																																and isnull(lotid, -1) = isnull(ia.lotid, -1))
                                                                        and NOT exists (SELECT 1 --Ignore records that are created before hist entry for the product
                                                                                              from inventoryHist ihin 
                                                                                             where ihin.physicalCountId = @physicalCountID 
                                                                                               and ihin.productId = ia.productid 
                                                                                               and ihin.InitialCount = 1
                                                                                               and ia.timestamp < ihin.timestamp )
																		group by ToLocationId, productid, lotid
																)v
																group by FromLocationId, productid, lotid
						                                    ) BulkUpdated  on i.ProductId = BulkUpdated.productid and BulkUpdated.FromLocationId = i.LocationId and isnull(BulkUpdated.lotid, -1) = ISNULL(i.LotId, -1) 
                                                        where i.ProductId = p.ProductId
	                                                        and i.LocationId = l.LocationId
	                                                        and l.MassUpdateAllowed = 'Y'
                                                            and p.IsActive = 'Y'
                                                            and (i.locationid = @LocationID or @LocationID = -1)
                                                        )v";
            }

            selectInventoryQuery = selectInventoryQuery + ((string.IsNullOrEmpty(filterQuery)) ? " " : " Where " + filterCondition);

            selectInventoryParameters.Add(new SqlParameter("@timestamp", StartDate));
            selectInventoryParameters.Add(new SqlParameter("@LocationID", LocationID));
            selectInventoryParameters.Add(new SqlParameter("@physicalCountID", PhysicalCountID));
            return selectInventoryQuery.ToString();
        }

        public List<PhysicalCountReviewDTO> GetPhysicalCountReviewList(string filterCondition, int PhysicalCountID, DateTime StartDate, int LocationID, ExecutionContext executionContext)
        {
            log.LogMethodEntry(filterCondition, PhysicalCountID, StartDate, LocationID, executionContext);
            List<PhysicalCountReviewDTO> reviewList = null;
            string selectInventoryQuery = GetFilterQuery(filterCondition, PhysicalCountID, StartDate, LocationID, executionContext);
            selectInventoryParameters.Clear();
            DataTable reviewData = dataAccessHandler.executeSelectQuery(selectInventoryQuery, selectInventoryParameters.ToArray());
            if (reviewData.Rows.Count > 0)
            {
                reviewList = new List<PhysicalCountReviewDTO>();
                foreach (DataRow reviewDataRow in reviewData.Rows)
                {
                    PhysicalCountReviewDTO reviewDataObject = GetPhysicalCountReviewDTO(reviewDataRow);
                    reviewList.Add(reviewDataObject);
                }
            }
            log.LogMethodExit(reviewList);
            return reviewList;
        }


        public List<PhysicalCountReviewDTO> GetAllPhysicalCountReviewList(string filterCondition, int physicalCountId, DateTime startDate, int locationId, ExecutionContext executionContext, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(filterCondition, physicalCountId, startDate, locationId, currentPage, pageSize);
            List<PhysicalCountReviewDTO> physicalCountReviewList = null;
            selectInventoryParameters.Clear();
            string selectInventoryPhysicalCountQuery = GetFilterQuery(filterCondition, physicalCountId, startDate, locationId, executionContext);
            if (currentPage > 0 || pageSize > 0)
            {
                selectInventoryPhysicalCountQuery += " order by ProductId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectInventoryPhysicalCountQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable inventoryPhysicalCountData = dataAccessHandler.executeSelectQuery(selectInventoryPhysicalCountQuery, selectInventoryParameters.ToArray(), sqlTransaction);
            if (inventoryPhysicalCountData.Rows.Count > 0)
            {
                physicalCountReviewList = new List<PhysicalCountReviewDTO>();
                foreach (DataRow inventoryPhysicalCountDataRow in inventoryPhysicalCountData.Rows)
                {
                    PhysicalCountReviewDTO physicalCountReviewDataObject = GetPhysicalCountReviewDTO(inventoryPhysicalCountDataRow);
                    physicalCountReviewList.Add(physicalCountReviewDataObject);
                }
            }
            log.LogMethodExit(physicalCountReviewList);
            return physicalCountReviewList;
        }

        public List<PhysicalCountReviewDTO> GetAllPhysicalCountReviewDTOList(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters, string advancedSearch, string filterCondition, int physicalCountId, DateTime startDate, int locationId, ExecutionContext executionContext, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(filterCondition, physicalCountId, startDate, locationId, currentPage, pageSize);
            List<PhysicalCountReviewDTO> physicalCountReviewList = null;
            selectInventoryParameters.Clear();
            string selectInventoryPhysicalCountQuery = GetFilterSearchQuery(searchParameters, advancedSearch, filterCondition, physicalCountId, startDate, locationId, executionContext);
            if (currentPage > 0 || pageSize > 0)
            {
                selectInventoryPhysicalCountQuery += " order by ProductId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectInventoryPhysicalCountQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable inventoryPhysicalCountData = dataAccessHandler.executeSelectQuery(selectInventoryPhysicalCountQuery, selectInventoryParameters.ToArray(), sqlTransaction);
            if (inventoryPhysicalCountData.Rows.Count > 0)
            {
                physicalCountReviewList = new List<PhysicalCountReviewDTO>();
                foreach (DataRow inventoryPhysicalCountDataRow in inventoryPhysicalCountData.Rows)
                {
                    PhysicalCountReviewDTO physicalCountReviewDataObject = GetPhysicalCountReviewDTO(inventoryPhysicalCountDataRow);
                    physicalCountReviewList.Add(physicalCountReviewDataObject);
                }
            }
            log.LogMethodExit(physicalCountReviewList);
            return physicalCountReviewList;
        }

        /// <summary>
        /// Gets the InventoryDTO list matching the search key
        /// </summary>
        ///<param name="filterCondition"></param>
        ///<param name="LocationID"></param>
        ///<param name="PhysicalCountID"></param>
        ///<param name="StartDate"></param>
        /// <returns>Returns the list of InventoryDTO matching the search criteria</returns>
        private string GetFilterSearchQuery(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters, string advancedSearch, string filterCondition, int PhysicalCountID, DateTime StartDate, int LocationID, ExecutionContext executionContext)
        {
            log.LogMethodEntry(filterCondition, PhysicalCountID, StartDate, LocationID, executionContext);
            string filterQuery = filterCondition == null ? string.Empty : filterCondition.ToUpper();
            if (filterQuery.Contains("DROP") || filterQuery.Contains("UPDATE ") || filterQuery.Contains("DELETE"))
            {
                log.LogMethodExit();
                return null;
            }

            string selectInventoryQuery;

            SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(executionContext);
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "Y"));
            segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionSearchParams);

            if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any())
            {
                List<string> segmentNameList = segmentDefinitionDTOList.Select(x => x.SegmentName.Trim().ToLower()).ToList();
                if (segmentDefinitionDTOList.Count != segmentNameList.Distinct().Count())
                {
                    log.Error("Duplicate segment name exists.Failed to proceed");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2704));
                }
                string pivotColumns = "";
                foreach (SegmentDefinitionDTO sd in segmentDefinitionDTOList)
                {
                    pivotColumns += ", [" + sd.SegmentName + "]";
                }
                selectInventoryQuery = "select * " +
                                            pivotColumns +
                                     @"from (
                                                select code,
			                                        description,
													c.name category,
													(select isnull(v.Barcode, '')
						                                        from (
							                                        select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
							                                        from productbarcode 
							                                        where isactive = 'Y'
								                                            and productid = p.productid)v 
						                                        where num = 1) Barcode,
			                                        STUFF((SELECT '.'+ valuechar 
					                                        FROM segmentdataview 
					                                        WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
					                                        order by segmentdefinitionid 
					                                        FOR XML PATH('')),1,1,'') SKU,
			                                        il.LotNumber,
			                                        l.Name Location,
			                                        StartingQuantity.Quantity StartingQuantity,
													case isnull(BulkUpdated.BulkUpdated, 0) when 1 then isnull(BulkUpdated.BulkUpdatedQuantity, 0) + isnull(StartingQuantity.Quantity, 0) + isnull(PQuantity.quantity ,0)  else 0 end updatedPhysicalQuantity,
													isnull(StartingQuantity.Quantity, 0) - isnull(i.quantity, 0) Difference,
			                                        i.Quantity currentInventoryQuantity,
													null NewQuantity,
													'' physicalCountRemarks,
			                                        i.ProductId,
			                                        i.LocationId,
			                                        i.LotId,
                                                    i.Site_ID,
													p.CategoryId,
                                                    p.IsPurchaseable,
                                                    l.RemarksMandatory,
												    segmentname,
												    valuechar,
                                                    StartingQuantity.ID,
                                                    p.LotControlled,
                                                    i.UOMId,
													u.UOM,
                                                    StartingQuantity.PhysicalCountId
                                        from Location l, Product p left outer join (select * 
					                            from (
													    select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
													    from productbarcode 
													    where isactive = 'Y')v 
											    where num = 1) b on p.productid = b.productid 
                                                        left outer join Category c on c.CategoryId = p.categoryid
													    left outer join SegmentDataView sdv on sdv.SegmentCategoryId = p.segmentcategoryid,
                                                        inventory i left outer join InventoryLot il on il.LotId = i.lotid left outer join UOM u on i.UOMId = u.UOMId
                                                        
                                                left outer join(select quantity, productid, locationid, lotid
						                                            from InventoryHist
						                                            where InitialCount = 1
							                                        and PhysicalCountId = @physicalCountID
						                                        )SystemCount on i.ProductId = SystemCount.productid and SystemCount.LocationId = i.LocationId and isnull(SystemCount.lotid, -1) = ISNULL(i.LotId, -1)
                                                
		                                        left outer join(select quantity, productid, locationid, lotid ,ID, PhysicalCountId
						                                        from InventoryHist
						                                        where InitialCount = 1
							                                    and PhysicalCountId = @physicalCountID
						                                    )StartingQuantity on i.ProductId = StartingQuantity.productid and StartingQuantity.LocationId = i.LocationId and isnull(StartingQuantity.lotid, -1) = ISNULL(i.LotId, -1)

                                                  left outer join(select sum( quantity) quantity, productid, locationid
						                                        from ProductActivityView
						                                        where LocationId = @LocationID
																and TrxType in ('Redemption' ,'Sales','Receipts')
							                                    and TimeStamp > @timestamp
																group by ProductId , LocationId
						                                    )PQuantity on i.ProductId = PQuantity.productid 

		                                        left outer join (select sum(BulkUpdatedQuantity) BulkUpdatedQuantity, FromLocationId, productid, lotid, 1 bulkupdated
																from (
																		select sum(case when bulkupdated = 1 then adjustmentquantity 
																							when adjustmentType = 'Transfer' then
																								case when fromlocationid = ia.FromLocationId then -1 * AdjustmentQuantity 
																								end
																							else AdjustmentQuantity
																							end) BulkUpdatedQuantity, 
																				FromLocationId, 
																				productid, 
																				lotid
																		from InventoryAdjustments ia
																		where exists ( select FromLocationId, productid, lotid, max(timestamp) timestamp
																						from inventoryadjustments
																						where bulkupdated = 1
																							and Timestamp > @timestamp
																							and (FromLocationId = @LocationID or @LocationID = -1)
																							and FromLocationId = ia.FromLocationId
																							and isnull(lotid, -1) = isnull(ia.lotid, -1)
																						group by FromLocationId, productid, lotid)
																			and Timestamp > @timestamp and Timestamp <= (select max(timestamp)
																															from inventoryadjustments
																															where bulkupdated = 1
																																and Timestamp > @timestamp
																																and (FromLocationId = @LocationID or @LocationID = -1)
																																and FromLocationId = ia.FromLocationId
																																and isnull(lotid, -1) = isnull(ia.lotid, -1))
                                                                            and NOT exists (SELECT 1 --Ignore records that are created before hist entry for the product
                                                                                              from inventoryHist ihin 
                                                                                             where ihin.physicalCountId = @physicalCountID 
                                                                                               and ihin.productId = ia.productid 
                                                                                               and ihin.InitialCount = 1
                                                                                               and ia.timestamp < ihin.timestamp )
																		group by FromLocationId, productid, lotid
																		union all
																		select sum(case when bulkupdated = 1 then adjustmentquantity 
																							when adjustmentType = 'Transfer' then
																								case when tolocationid = ia.tolocationid then AdjustmentQuantity 
																								end
																							else AdjustmentQuantity
																							end) BulkUpdatedQuantity, 
																				ToLocationId, 
																				productid, 
																				lotid
																		from InventoryAdjustments ia
																		where exists ( select FromLocationId, productid, lotid, max(timestamp) timestamp
																						from inventoryadjustments
																						where bulkupdated = 1
																							and Timestamp > @timestamp
																							and (FromLocationId = @LocationID or @LocationID = -1)
																							and FromLocationId = ia.ToLocationId
																							and isnull(lotid, -1) = isnull(ia.lotid, -1)
																						group by FromLocationId, productid, lotid)
																			and Timestamp > @timestamp and Timestamp <= (select max(timestamp)
																															from inventoryadjustments
																															where bulkupdated = 1
																																and Timestamp > @timestamp
																																and (FromLocationId = @LocationID or @LocationID = -1)
																																and FromLocationId = ia.ToLocationId
																																and isnull(lotid, -1) = isnull(ia.lotid, -1))
                                                                             and NOT exists (SELECT 1 --Ignore records that are created before hist entry for the product
                                                                                               from inventoryHist ihin 
                                                                                              where ihin.physicalCountId = @physicalCountID 
                                                                                                and ihin.productId = ia.productid 
                                                                                                and ihin.InitialCount = 1
                                                                                                and ia.timestamp < ihin.timestamp )
																		group by ToLocationId, productid, lotid
																)v
																group by FromLocationId, productid, lotid
						                                    ) BulkUpdated  on i.ProductId = BulkUpdated.productid and BulkUpdated.FromLocationId = i.LocationId and isnull(BulkUpdated.lotid, -1) = ISNULL(i.LotId, -1) 
                                        where i.ProductId = p.ProductId
	                                        and i.LocationId = l.LocationId
	                                        and l.MassUpdateAllowed = 'Y'
                                            and p.IsActive = 'Y'
                                            and (i.locationid = @LocationID or @LocationID = -1)
			                            )p 
                                     PIVOT
                                              (max(valuechar) for segmentname in (" + pivotColumns.Substring(2) + ")" + ")v1 " +
                                         advancedSearch;

            }
            else
            {
                selectInventoryQuery = @"select *
                                                         from (
                                                                    select code,
			                                                        description,
																	c.name category,
																	(select isnull(v.Barcode, '')
						                                                        from (
							                                                        select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
							                                                        from productbarcode 
							                                                        where isactive = 'Y'
								                                                            and productid = p.productid)v 
						                                                        where num = 1) Barcode,
			                                                        STUFF((SELECT '.'+ valuechar 
					                                                        FROM segmentdataview 
					                                                        WHERE segmentcategoryid = p.SegmentCategoryId and ValueChar is not null 
					                                                        order by segmentdefinitionid 
					                                                        FOR XML PATH('')),1,1,'') SKU,
			                                                        il.LotNumber,
			                                                        l.Name Location,
			                                                        StartingQuantity.Quantity StartingQuantity,
																	case isnull(BulkUpdated.BulkUpdated, 0) when 1 then isnull(BulkUpdated.BulkUpdatedQuantity, 0) + isnull(StartingQuantity.Quantity, 0)+ isnull(PQuantity.quantity ,0)  else 0 end updatedPhysicalQuantity,
																	isnull(StartingQuantity.Quantity, 0) - isnull(i.quantity, 0) Difference,
			                                                        i.Quantity currentInventoryQuantity,
																	null NewQuantity,
																	'' physicalCountRemarks,
			                                                        i.ProductId,
			                                                        i.LocationId,
			                                                        i.LotId,
                                                                    i.Site_ID,
																	p.CategoryId,
                                                                    p.IsPurchaseable,
                                                                    l.RemarksMandatory,
                                                                    StartingQuantity.ID,
                                                                    p.LotControlled,
                                                                    i.UOMId,
													                u.UOM,
                                                                    StartingQuantity.PhysicalCountId
                                                        from Location l, Product p left outer join Category c on c.CategoryId = p.categoryid
                                                                           , inventory i left outer join InventoryLot il on il.LotId = i.lotid  left outer join UOM u on i.UOMId = u.uomId
		                                                        left outer join(select quantity, productid, locationid, lotid,ID,PhysicalCountId
						                                                            from InventoryHist
						                                                            where InitialCount = 1
							                                                        and PhysicalCountId = @physicalCountID
						                                                        )StartingQuantity on i.ProductId = StartingQuantity.productid and StartingQuantity.LocationId = i.LocationId and isnull(StartingQuantity.lotid, -1) = ISNULL(i.LotId, -1)
                                                                
                                                                left outer join(select sum( quantity) quantity, productid, locationid
						                                                    from ProductActivityView
						                                                    where LocationId = @LocationID
															                and TrxType in ('Redemption' ,'Sales','Receipts')
							                                                and TimeStamp > @timestamp
															                group by ProductId , LocationId
						                                                )PQuantity on i.ProductId = PQuantity.productid 
                                                                
		                                                        left outer join (select sum(BulkUpdatedQuantity) BulkUpdatedQuantity, FromLocationId, productid, lotid, 1 bulkupdated
																from (
																		select sum(case when bulkupdated = 1 then adjustmentquantity 
																							when adjustmentType = 'Transfer' then
																								case when fromlocationid = ia.FromLocationId then -1 * AdjustmentQuantity 
																								end
																							else AdjustmentQuantity
																							end) BulkUpdatedQuantity, 
																				FromLocationId, 
																				productid, 
																				lotid
																		from InventoryAdjustments ia
																		where exists ( select FromLocationId, productid, lotid, max(timestamp) timestamp
																						from inventoryadjustments
																						where bulkupdated = 1
																							and Timestamp > @timestamp
																							and (FromLocationId = @LocationID or @LocationID = -1)
																							and FromLocationId = ia.FromLocationId
																							and isnull(lotid, -1) = isnull(ia.lotid, -1)
																						group by FromLocationId, productid, lotid)
																			and Timestamp > @timestamp and Timestamp <= (select max(timestamp)
																															from inventoryadjustments
																															where bulkupdated = 1
																																and Timestamp > @timestamp
																																and (FromLocationId = @LocationID or @LocationID = -1)
																																and FromLocationId = ia.FromLocationId
																																and isnull(lotid, -1) = isnull(ia.lotid, -1))
                                                                        and NOT exists (SELECT 1 --Ignore records that are created before hist entry for the product
                                                                                              from inventoryHist ihin 
                                                                                             where ihin.physicalCountId = @physicalCountID 
                                                                                               and ihin.productId = ia.productid 
                                                                                               and ihin.InitialCount = 1
                                                                                               and ia.timestamp < ihin.timestamp )
																		group by FromLocationId, productid, lotid
																		union all
																		select sum(case when bulkupdated = 1 then adjustmentquantity 
																							when adjustmentType = 'Transfer' then
																								case when tolocationid = ia.tolocationid then AdjustmentQuantity 
																								end
																							else AdjustmentQuantity
																							end) BulkUpdatedQuantity, 
																				ToLocationId, 
																				productid, 
																				lotid
																		from InventoryAdjustments ia
																		where exists ( select FromLocationId, productid, lotid, max(timestamp) timestamp
																						from inventoryadjustments
																						where bulkupdated = 1
																							and Timestamp > @timestamp
																							and (FromLocationId = @LocationID or @LocationID = -1)
																							and FromLocationId = ia.ToLocationId
																							and isnull(lotid, -1) = isnull(ia.lotid, -1)
																						group by FromLocationId, productid, lotid)
																			and Timestamp > @timestamp and Timestamp <= (select max(timestamp)
																															from inventoryadjustments
																															where bulkupdated = 1
																																and Timestamp > @timestamp
																																and (FromLocationId = @LocationID or @LocationID = -1)
																																and FromLocationId = ia.ToLocationId
																																and isnull(lotid, -1) = isnull(ia.lotid, -1))
                                                                        and NOT exists (SELECT 1 --Ignore records that are created before hist entry for the product
                                                                                              from inventoryHist ihin 
                                                                                             where ihin.physicalCountId = @physicalCountID 
                                                                                               and ihin.productId = ia.productid 
                                                                                               and ihin.InitialCount = 1
                                                                                               and ia.timestamp < ihin.timestamp )
																		group by ToLocationId, productid, lotid
																)v
																group by FromLocationId, productid, lotid
						                                    ) BulkUpdated  on i.ProductId = BulkUpdated.productid and BulkUpdated.FromLocationId = i.LocationId and isnull(BulkUpdated.lotid, -1) = ISNULL(i.LotId, -1) 
                                                        where i.ProductId = p.ProductId
	                                                        and i.LocationId = l.LocationId
	                                                        and l.MassUpdateAllowed = 'Y'
                                                            and p.IsActive = 'Y'
                                                            and (i.locationid = @LocationID or @LocationID = -1)
                                                        )v";
            }

            int count = 0;
            selectInventoryParameters.Add(new SqlParameter("@timestamp", StartDate));
            selectInventoryParameters.Add(new SqlParameter("@LocationID", LocationID));
            selectInventoryParameters.Add(new SqlParameter("@physicalCountID", PhysicalCountID));
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.LOCATIONID
                            || searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.PRODUCT_ID
                            || searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.CATEGORYID
                           )
                        {
                            //query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            selectInventoryParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.CODE
                            || searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.BARCODE
                            || searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.INVENTORYITEMSONLY
                            || searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.DESCRIPTION
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            selectInventoryParameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.INVENTORYQTY)
                        {
                            //query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            selectInventoryParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDecimal(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.SITE_ID)
                        {
                            //query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            selectInventoryParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else
                        {
                            //query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'~') like " + "'%" + searchParameter.Value + "%'");
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            selectInventoryParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        //log.Debug("Ends-GetPhysicalCountReviewList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        //throw new Exception("The query parameter does not exist " + searchParameter.Key);
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }

                if (searchParameters.Count > 0)
                    selectInventoryQuery = selectInventoryQuery + query;
            }
            selectInventoryQuery = selectInventoryQuery + ((string.IsNullOrEmpty(filterQuery)) ? " " : " AND " + filterCondition);
            return selectInventoryQuery.ToString();
        }

        /// <summary>
        /// Returns the no of Physical Count Review matching the search parameters
        /// </summary> 
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>

        public int GetPhysicalCountReviewsCount(string filterCondition, int physicalCountId, DateTime startDate, int locationId)
        {
            log.LogMethodEntry(filterCondition);
            int physicalCountReviewsDTOCount = 0;
            selectInventoryParameters.Clear();
            string selectQuery = GetFilterQuery(filterCondition, physicalCountId, startDate, locationId, ExecutionContext.GetExecutionContext());
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, selectInventoryParameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                physicalCountReviewsDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(physicalCountReviewsDTOCount);
            return physicalCountReviewsDTOCount;
        }

        /// <summary>
        /// Returns the no of Physical Count Review matching the search parameters
        /// </summary> 
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>

        public int GetPhysicalCountReviewsDTOListCount(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters,
                                                       string advancedSearch, string filterCondition, int physicalCountId, DateTime startDate, int locationId, ExecutionContext executionContext)
        {
            log.LogMethodEntry(filterCondition);
            int physicalCountReviewsDTOCount = 0;
            selectInventoryParameters.Clear();
            string selectQuery = GetFilterSearchQuery(searchParameters, advancedSearch, filterCondition, physicalCountId, startDate, locationId, executionContext);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, selectInventoryParameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                physicalCountReviewsDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(physicalCountReviewsDTOCount);
            return physicalCountReviewsDTOCount;
        }
    }
}