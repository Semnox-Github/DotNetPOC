/********************************************************************************************
*Project Name -                                                                           
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*.00        13-Aug-2016            Soumya                      Created.
*
 ********************************************************************************************
 *2.60      13-04-2019            Girish                       Modified
 *2.70.2    16-07-2019            Deeksha                      Modifications as per three tier changes.
 *2.70.2    09-Dec-2019           Jinto Thomas                 Removed siteid from update query 
 *2.100.0   17-Sep-2020           Deeksha                      Modified to handle UOMId field
 *2.110.0   28-Dec-2020           Mushahid Faizan              Modified for Web Inventory changes with Rest API.
 *2.130.0   04-Jun-2021           Girish Kundar                Modified - POS stock changes
 *2.150.0   23-Jun-2022           Abhishek                     Modified - Addition of Column IsAutoPO for Auto Purchase orders
 **********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using System.Globalization;

namespace Semnox.Parafait.Inventory
{
    public class PurchaseOrderDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM PurchaseOrder AS po ";
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string> DBSearchParameters = new Dictionary<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>
               {
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDERID, "po.PurchaseOrderId"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERNUMBER, "po.OrderNumber"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERSTATUS, "po.OrderStatus"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERNUMBER_LIKE, "po.OrderNumber"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERDATE, "po.OrderDate"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.VENDORID, "po.VendorId"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.SITE_ID,"po.Site_id"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_TYPE_ID,"po.DocumentTypeId"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.FROM_DATE,"po.Fromdate"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.TO_DATE,"po.ToDate"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_STATUS,"po.DocumentStatus"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.GUID,"po.Guid"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.FROM_SITE_ID,"po.FromSiteId"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.TO_SITE_ID,"po.ToSiteId"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORIGINAL_REFERENCE_GUID,"po.OriginalReferenceGUID"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.LAST_MODIFIED_DATE_GREATER_THAN,"po.lastmoddttm"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.LAST_MODIFIED_DATE_LESS_THAN_EQUAL_TO,"po.lastmoddttm"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.ISACTIVE,"po.IsActive"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDER_ID_LIST,"po.PurchaseOrderId"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.GUID_ID_LIST,"po.Guid"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.HAS_EXTERNAL_SYSTEM_REFERENCE,"po.ExternalSystemReference"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.PRODUCT_ID,"poLine.ProductId"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.PRODUCT_ID_LIST,"poLine.ProductId"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.MASTER_ENTITY_ID,"po.MasterEntityId"},
                    {PurchaseOrderDTO.SearchByPurchaseOrderParameters.IS_AUTO_PO,"po.IsAutoPO"}
               };

        private DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>();
        /// <summary>
        /// Default constructor of PurchaseOrderDataHandler class
        /// </summary>
        public PurchaseOrderDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to VendorDTO class type
        /// </summary>
        /// <param name="vendorDataRow">VendorDTO DataRow</param>
        /// <returns>Returns VendorDTO</returns>
        private PurchaseOrderDTO GetPurchaseOrderDTO(DataRow purchaseOrderDataRow)
        {
            log.LogMethodEntry(purchaseOrderDataRow);

            PurchaseOrderDTO PurchaseOrderDataObject = new PurchaseOrderDTO(Convert.ToInt32(purchaseOrderDataRow["PurchaseOrderId"]),
                                            purchaseOrderDataRow["OrderStatus"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["OrderStatus"]),
                                            purchaseOrderDataRow["OrderNumber"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["OrderNumber"]),
                                            purchaseOrderDataRow["OrderDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderDataRow["OrderDate"]),
                                            purchaseOrderDataRow["VendorId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderDataRow["VendorId"]),
                                            purchaseOrderDataRow["ContactName"].ToString(),
                                            purchaseOrderDataRow["Phone"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["Phone"]),
                                            purchaseOrderDataRow["VendorAddress1"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["VendorAddress1"]),
                                            purchaseOrderDataRow["VendorAddress2"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["VendorAddress2"]),
                                            purchaseOrderDataRow["VendorCity"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["VendorCity"]),
                                            purchaseOrderDataRow["VendorState"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["VendorState"]),
                                            purchaseOrderDataRow["VendorCountry"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["VendorCountry"]),
                                            purchaseOrderDataRow["VendorPostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["VendorPostalCode"]),
                                            purchaseOrderDataRow["ShipToAddress1"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["ShipToAddress1"]),
                                            purchaseOrderDataRow["ShipToAddress2"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["ShipToAddress2"]),
                                            purchaseOrderDataRow["ShipToCity"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["ShipToCity"]),
                                            purchaseOrderDataRow["ShipToState"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["ShipToState"]),
                                            purchaseOrderDataRow["ShipToCountry"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["ShipToCountry"]),
                                            purchaseOrderDataRow["ShipToPostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["ShipToPostalCode"]),
                                            purchaseOrderDataRow["ShipToAddressRemarks"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["ShipToAddressRemarks"]),
                                            purchaseOrderDataRow["RequestShipDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderDataRow["RequestShipDate"]),
                                            purchaseOrderDataRow["OrderTotal"] == DBNull.Value ? 0 : Convert.ToDouble(purchaseOrderDataRow["OrderTotal"]),
                                            purchaseOrderDataRow["LastModUserId"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["LastModUserId"]),
                                            purchaseOrderDataRow["lastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderDataRow["lastModDttm"]),
                                            purchaseOrderDataRow["ReceivedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderDataRow["ReceivedDate"]),
                                            purchaseOrderDataRow["ReceiveRemarks"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["ReceiveRemarks"]),
                                            purchaseOrderDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderDataRow["site_id"]),
                                            purchaseOrderDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["Guid"]),
                                            purchaseOrderDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(purchaseOrderDataRow["SynchStatus"]),
                                            purchaseOrderDataRow["CancelledDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderDataRow["CancelledDate"]),
                                            purchaseOrderDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderDataRow["MasterEntityId"]),
                                            purchaseOrderDataRow["IsCreditPO"] == DBNull.Value ? "N" : purchaseOrderDataRow["IsCreditPO"].ToString(),
                                            purchaseOrderDataRow["DocumentTypeID"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderDataRow["DocumentTypeID"]),
                                            purchaseOrderDataRow["Fromdate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderDataRow["Fromdate"]),
                                            purchaseOrderDataRow["Todate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderDataRow["Todate"]),
                                            purchaseOrderDataRow["OrderRemarks"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["OrderRemarks"]),
                                            purchaseOrderDataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["ExternalSystemReference"]),
                                            purchaseOrderDataRow["ReprintCount"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderDataRow["ReprintCount"]),
                                            purchaseOrderDataRow["AmendmentNumber"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderDataRow["AmendmentNumber"]),
                                            purchaseOrderDataRow["DocumentStatus"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["DocumentStatus"]),
                                            purchaseOrderDataRow["FromSiteId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderDataRow["FromSiteId"]),
                                            purchaseOrderDataRow["ToSiteId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderDataRow["ToSiteId"]),
                                            purchaseOrderDataRow["OriginalReferenceGUID"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["OriginalReferenceGUID"]),
                                            new List<PurchaseOrderLineDTO>(),
                                            new List<InventoryReceiptDTO>(),
                                            purchaseOrderDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderDataRow["CreatedBy"]),
                                            purchaseOrderDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderDataRow["CreationDate"]),
                                            purchaseOrderDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(purchaseOrderDataRow["IsActive"]),
                                            purchaseOrderDataRow["IsAutoPO"] == DBNull.Value ? false : Convert.ToBoolean(purchaseOrderDataRow["IsAutoPO"])
                                            );
            log.LogMethodExit(PurchaseOrderDataObject);
            return PurchaseOrderDataObject;
        }
        /// <summary>
        /// GetPurchaseOrder
        /// </summary>
        /// <param name="PurchaseOrderId"></param>
        /// <param name="loginid"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public PurchaseOrderDTO GetPurchaseOrder(int PurchaseOrderId, string loginid, int siteId)
        {
            //ExecutionContext machineContext = ExecutionContext.GetExecutionContext();
            log.LogMethodEntry(PurchaseOrderId, loginid, siteId);
            string selectPurchaseOrderQuery = @"declare @userid int;select @userid = user_id from users where loginId = @loginUserId ; exec dbo.SetContextInfo @userid;select *
                                         from PurchaseOrder po  
                                        where po.PurchaseOrderId = @PurchaseOrderId
                                        and isnull(po.AmendmentNumber, 0) = (select max(isnull(p.AmendmentNumber, 0)) 
                                                                             from PurchaseOrder p 
                                                                             where p.ordernumber = po.OrderNumber  
                                                                               and (p.site_id = @siteId OR @siteId = -1)
                                                                             group by p.ordernumber)
                                         and LastModUserId in (select loginId 
	                                                           from  DataAccessView dav
					                                           where
						                                    ( ((Entity = 'PO' or Entity = 'Receiving') and dav.DataAccessRuleId is not null)
						                                    OR 
						                                   dav.DataAccessRuleId is null
						                                    )
						                         ) ";
            SqlParameter[] selectPurchaseOrderParameters = new SqlParameter[3];
            selectPurchaseOrderParameters[0] = new SqlParameter("@PurchaseOrderId", PurchaseOrderId);
            selectPurchaseOrderParameters[1] = new SqlParameter("@loginUserId", loginid);
            selectPurchaseOrderParameters[2] = new SqlParameter("@siteId", siteId);
            DataTable purchaseorder = dataAccessHandler.executeSelectQuery(selectPurchaseOrderQuery, selectPurchaseOrderParameters, sqlTransaction);
            if (purchaseorder.Rows.Count > 0)
            {
                DataRow purchaseorderRow = purchaseorder.Rows[0];
                PurchaseOrderDTO purchaseorderDataObject = GetPurchaseOrderDTO(purchaseorderRow);
                log.LogMethodExit(purchaseorderDataObject);
                return purchaseorderDataObject;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
        /// <summary>
        /// Returns the purchase order table columns
        /// </summary>
        /// <returns></returns>
        public DataTable GetPurchaseOrderColumns()
        {
            log.LogMethodEntry();
            string selectPurchaseOrderQuery = "Select columns from(Select '' as columns UNION ALL Select COLUMN_NAME as columns from INFORMATION_SCHEMA.COLUMNS  Where TABLE_NAME='Purchaseorder') a order by columns";
            DataTable purchasaeorderTableColumns = dataAccessHandler.executeSelectQuery(selectPurchaseOrderQuery, null, sqlTransaction);
            log.LogMethodExit(purchasaeorderTableColumns);
            return purchasaeorderTableColumns;
        }

        /// <summary>
        /// Retriving purchaseorder by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the purchaseorder</param>
        /// <returns> List of PurchaseOrderDTO </returns>
        public List<PurchaseOrderDTO> GetPurchaseOrderList(string sqlQuery)
        {
            log.LogMethodEntry(sqlQuery);
            string Query = sqlQuery.ToUpper();
            if (Query.Contains("DROP") || Query.Contains("UPDATE") || Query.Contains("DELETE"))
            {
                log.LogMethodExit();
                return null;
            }
            DataTable purchaseorderData = dataAccessHandler.executeSelectQuery(sqlQuery, null, sqlTransaction);
            if (purchaseorderData.Rows.Count > 0)
            {
                List<PurchaseOrderDTO> purchaseorderList = new List<PurchaseOrderDTO>();
                foreach (DataRow purchaseorderDataRow in purchaseorderData.Rows)
                {
                    PurchaseOrderDTO purchaseorderDataObject = GetPurchaseOrderDTO(purchaseorderDataRow);
                    purchaseorderList.Add(purchaseorderDataObject);
                }
                log.LogMethodExit(purchaseorderList);
                return purchaseorderList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Returns the List of PurchaseOrderDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of PurchaseOrderDTO</returns>
        public string GetFilterQuery(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> searchParameters, string loginId)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDERID)
                            || searchParameter.Key.Equals(PurchaseOrderDTO.SearchByPurchaseOrderParameters.VENDORID)
                            || searchParameter.Key.Equals(PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_TYPE_ID)
                            || searchParameter.Key.Equals(PurchaseOrderDTO.SearchByPurchaseOrderParameters.FROM_SITE_ID)
                            || searchParameter.Key.Equals(PurchaseOrderDTO.SearchByPurchaseOrderParameters.MASTER_ENTITY_ID)
                            || searchParameter.Key.Equals(PurchaseOrderDTO.SearchByPurchaseOrderParameters.TO_SITE_ID))
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key.Equals(PurchaseOrderDTO.SearchByPurchaseOrderParameters.HAS_EXTERNAL_SYSTEM_REFERENCE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IS NOT NULL");
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }


                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.GUID_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERDATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.LAST_MODIFIED_DATE_GREATER_THAN)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "dd-MMM-yyyy hh:mm:ss.fff tt", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.LAST_MODIFIED_DATE_LESS_THAN_EQUAL_TO)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "dd-MMM-yyyy hh:mm:ss.fff tt", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.GUID
                            || searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORIGINAL_REFERENCE_GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.PRODUCT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.PRODUCT_ID_LIST)  //int
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERSTATUS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));

                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.ISACTIVE
                            || searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.IS_AUTO_PO)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDER_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }

                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                query.Append(((count == 0) ? " " : " and ") + " isnull(po.AmendmentNumber, 0) = ( select max(isnull(p.AmendmentNumber,0)) from PurchaseOrder p where p.ordernumber=po.OrderNumber  group by p.ordernumber) and LastModUserId in (select loginId "
                                                   + @"from DataAccessView dav
                                                               where
                                                            (((Entity = 'PO' or Entity = 'Receiving') and dav.DataAccessRuleId is not null)
						                                    OR
                                                           dav.DataAccessRuleId is null
						                                    )
						                         )");

            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// Gets the PurchaseOrderDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqltrxn">SqlTrasanction object</param>
        /// <returns>Returns the list of PurchaseOrderDTO matching the search criteria</returns>
        public List<PurchaseOrderDTO> GetPurchaseOrderList(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> searchParameters, string loginId,
                                                            int currentPage = 0, int pageSize = 0)
        {
            try
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);
                List<PurchaseOrderDTO> purchaseOrderDTOList = new List<PurchaseOrderDTO>();
                parameters.Clear();

                string selectQuery = @"declare @userid int;select @userid = user_id from users where loginId = @loginUserId ;exec dbo.SetContextInfo @userid ;select *
                                         from purchaseorder  po ";
                parameters.Add(new SqlParameter("@loginUserId", loginId));
                foreach (KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string> searchParameter in searchParameters)
                {
                    if (searchParameter.Key == (PurchaseOrderDTO.SearchByPurchaseOrderParameters.PRODUCT_ID_LIST))
                    {
                        string productQuery = @" left outer join purchaseOrder_line poLine on poLine.PurchaseOrderId = po.PurchaseOrderId";
                        selectQuery += productQuery;
                    }
                }
                selectQuery += GetFilterQuery(searchParameters, loginId);

                if (currentPage > 0 || pageSize > 0)
                {
                    selectQuery += " ORDER BY po.PurchaseOrderId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                    selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
                }

                DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
                if (dataTable.Rows.Count > 0)
                {
                    purchaseOrderDTOList = new List<PurchaseOrderDTO>();
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        PurchaseOrderDTO purchaseOrderDTO = GetPurchaseOrderDTO(dataRow);
                        purchaseOrderDTOList.Add(purchaseOrderDTO);
                    }
                }
                log.LogMethodExit(purchaseOrderDTOList);
                return purchaseOrderDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }



        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PurchaseOrderDataHandler Record.
        /// </summary>
        /// <param name="PurchaseOrderDTO">PurchaseOrderDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PurchaseOrderDTO purchaseOrderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrderId", purchaseOrderDTO.PurchaseOrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderStatus", string.IsNullOrEmpty(purchaseOrderDTO.OrderStatus) ? string.Empty : purchaseOrderDTO.OrderStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderNumber", string.IsNullOrEmpty(purchaseOrderDTO.OrderNumber) ? string.Empty : purchaseOrderDTO.OrderNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderDate", purchaseOrderDTO.OrderDate.Equals(DateTime.MinValue) ? DBNull.Value : (object)purchaseOrderDTO.OrderDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VendorId", purchaseOrderDTO.VendorId, true));
            parameters.Add(new SqlParameter("@LastModDttm", purchaseOrderDTO.LastModDttm == DateTime.MinValue ? DBNull.Value : (object)purchaseOrderDTO.LastModDttm));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DocumentTypeID", purchaseOrderDTO.DocumentTypeID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReprintCount", purchaseOrderDTO.ReprintCount, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AmendmentNumber", purchaseOrderDTO.AmendmentNumber, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fromSiteId", purchaseOrderDTO.FromSiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@toSiteId", purchaseOrderDTO.ToSiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReceivedDate", purchaseOrderDTO.ReceivedDate.Equals(DateTime.MinValue) ? DBNull.Value : (object)purchaseOrderDTO.ReceivedDate));
            parameters.Add(new SqlParameter("@ContactName", string.IsNullOrEmpty(purchaseOrderDTO.ContactName) ? string.Empty : purchaseOrderDTO.ContactName));
            parameters.Add(new SqlParameter("@Phone", string.IsNullOrEmpty(purchaseOrderDTO.Phone) ? string.Empty : purchaseOrderDTO.Phone));
            parameters.Add(new SqlParameter("@VendorAddress1", string.IsNullOrEmpty(purchaseOrderDTO.VendorAddress1) ? string.Empty : purchaseOrderDTO.VendorAddress1));
            parameters.Add(new SqlParameter("@VendorAddress2", string.IsNullOrEmpty(purchaseOrderDTO.VendorAddress2) ? string.Empty : purchaseOrderDTO.VendorAddress2));
            parameters.Add(new SqlParameter("@VendorCity", string.IsNullOrEmpty(purchaseOrderDTO.VendorCity) ? string.Empty : purchaseOrderDTO.VendorCity));
            parameters.Add(new SqlParameter("@VendorState", string.IsNullOrEmpty(purchaseOrderDTO.VendorState) ? string.Empty : purchaseOrderDTO.VendorState));
            parameters.Add(new SqlParameter("@VendorCountry", string.IsNullOrEmpty(purchaseOrderDTO.VendorCountry) ? string.Empty : purchaseOrderDTO.VendorCountry));
            parameters.Add(new SqlParameter("@VendorPostalCode", string.IsNullOrEmpty(purchaseOrderDTO.VendorPostalCode) ? string.Empty : purchaseOrderDTO.VendorPostalCode));
            parameters.Add(new SqlParameter("@ShipToAddress1", string.IsNullOrEmpty(purchaseOrderDTO.ShipToAddress1) ? string.Empty : purchaseOrderDTO.ShipToAddress1));
            parameters.Add(new SqlParameter("@ShipToAddress2", string.IsNullOrEmpty(purchaseOrderDTO.ShipToAddress2) ? string.Empty : purchaseOrderDTO.ShipToAddress2));
            parameters.Add(new SqlParameter("@ShipToCity", string.IsNullOrEmpty(purchaseOrderDTO.ShipToCity) ? string.Empty : purchaseOrderDTO.ShipToCity));
            parameters.Add(new SqlParameter("@ShipToState", string.IsNullOrEmpty(purchaseOrderDTO.ShipToState) ? string.Empty : purchaseOrderDTO.ShipToState));
            parameters.Add(new SqlParameter("@ShipToCountry", string.IsNullOrEmpty(purchaseOrderDTO.ShipToCountry) ? string.Empty : purchaseOrderDTO.ShipToCountry));
            parameters.Add(new SqlParameter("@ShipToPostalCode", string.IsNullOrEmpty(purchaseOrderDTO.ShipToPostalCode) ? string.Empty : purchaseOrderDTO.ShipToPostalCode));
            parameters.Add(new SqlParameter("@ShipToAddressRemarks", string.IsNullOrEmpty(purchaseOrderDTO.ShipToAddressRemarks) ? string.Empty : purchaseOrderDTO.ShipToAddressRemarks));
            parameters.Add(new SqlParameter("@RequestShipDate", purchaseOrderDTO.RequestShipDate == DateTime.MinValue ? DBNull.Value : (object)purchaseOrderDTO.RequestShipDate));
            parameters.Add(new SqlParameter("@OrderRemarks", string.IsNullOrEmpty(purchaseOrderDTO.OrderRemarks) ? string.Empty : purchaseOrderDTO.OrderRemarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", purchaseOrderDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderTotal", purchaseOrderDTO.OrderTotal));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastModUserId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@loginUserId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancelledDate", purchaseOrderDTO.CancelledDate == (DateTime.MinValue) ? (object)purchaseOrderDTO.CancelledDate : DBNull.Value));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsCreditPO", string.IsNullOrEmpty(purchaseOrderDTO.IsCreditPO) ? "N" : purchaseOrderDTO.IsCreditPO));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Fromdate", purchaseOrderDTO.Fromdate == (DateTime.MinValue) ? DBNull.Value : (object)purchaseOrderDTO.Fromdate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToDate", purchaseOrderDTO.ToDate == (DateTime.MinValue) ? DBNull.Value : (object)purchaseOrderDTO.ToDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@originalReferenceGUID", string.IsNullOrEmpty(purchaseOrderDTO.IsCreditPO) ? DBNull.Value : (object)purchaseOrderDTO.OriginalReferenceGUID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DocumentStatus", string.IsNullOrEmpty(purchaseOrderDTO.DocumentStatus) ? string.Empty : purchaseOrderDTO.DocumentStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", purchaseOrderDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", purchaseOrderDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsAutoPO", purchaseOrderDTO.IsAutoPO));
            log.LogMethodExit(parameters);
            return parameters;
        }


        public PurchaseOrderDTO InsertPurchaseOrder(PurchaseOrderDTO purchaseorder, string loginId, int siteId)
        {
            //DateTime LastModDttm = DateTime.Today;
            int id = 0;
            log.LogMethodEntry(purchaseorder, loginId, siteId);
            string query = @"INSERT INTO[dbo].[PurchaseOrder]
                                                   (OrderStatus, 
                                                    OrderNumber, 
                                                    OrderDate, 
                                                    VendorId, 
                                                    ContactName, 
                                                    Phone,VendorAddress1,VendorAddress2,VendorCity,VendorState,VendorCountry,VendorPostalCode, 
                                                    ShipToAddress1,
                                                    ShipToAddress2,
                                                    ShipToCity,
                                                    ShipToState,
                                                    ShipToCountry,
                                                    ShipToPostalCode,
                                                    ShipToAddressRemarks,
                                                    RequestShipDate,
                                                    OrderRemarks, 
                                                    ExternalSystemReference,
                                                    OrderTotal, 
                                                    LastModUserId, 
                                                    LastModDttm, site_id,
                                                    CancelledDate,
                                                    MasterEntityId,
                                                    IsCreditPO,
                                                    DocumentTypeID,
                                                    Fromdate, 
                                                    ToDate,
                                                    ReprintCount,
                                                    AmendmentNumber,
                                                    DocumentStatus,
                                                    ReceivedDate,
                                                    FromSiteId,
                                                    ToSiteId,
                                                    OriginalReferenceGUID,
                                                    CreatedBy,
                                                    CreationDate,
                                                    IsActive,
                                                    IsAutoPO
                                                    )
                                            Values (@OrderStatus, 
                                                    @OrderNumber, 
                                                    @OrderDate, 
                                                    @VendorId, 
                                                    @ContactName, 
                                                    @Phone,@VendorAddress1,@VendorAddress2,@VendorCity,@VendorState,@VendorCountry,@VendorPostalCode, 
                                                    @ShipToAddress1,
                                                    @ShipToAddress2,
                                                    @ShipToCity,
                                                    @ShipToState,
                                                    @ShipToCountry,
                                                    @ShipToPostalCode,
                                                    @ShipToAddressRemarks,
                                                    @RequestShipDate,
                                                    @OrderRemarks, 
                                                    @ExternalSystemReference,
                                                    @OrderTotal, 
                                                    @LastModUserId, 
                                                    @LastModDttm, @site_id,
                                                    @CancelledDate,
                                                    @MasterEntityId,
                                                    @IsCreditPO,
                                                    @DocumentTypeID,
                                                    @Fromdate, 
                                                    @ToDate,
                                                    @ReprintCount,
                                                    @AmendmentNumber,
                                                    @DocumentStatus,
                                                    @ReceivedDate,
                                                    @fromSiteId,
                                                    @toSiteId,
                                                    @originalReferenceGUID,
                                                    @createdBy,
                                                    GETDATE(),
                                                    @isActive,
                                                    @IsAutoPO
                                                    ) 
                                            SELECT * FROM PurchaseOrder WHERE PurchaseOrderId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(purchaseorder, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPurchaseOrderDTO(purchaseorder, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting purchase order", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(purchaseorder);
            return purchaseorder;
        }


        /// <summary>
        public PurchaseOrderDTO UpdatePurchaseOrder(PurchaseOrderDTO purchaseorder, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseorder, loginId, siteId);
            int id = 0;
            string query = @"UPDATE  [dbo].[PurchaseOrder]
                                    SET  OrderStatus = @OrderStatus, 
                                            OrderNumber = @OrderNumber, 
                                            VendorId = @VendorId, 
                                            ContactName = @ContactName, 
                                            Phone = @Phone,
                                            VendorAddress1 = @VendorAddress1,
                                            VendorAddress2 = @VendorAddress2,   
                                            VendorCity = @VendorCity,
                                            VendorState = @VendorState,
                                            VendorCountry = @VendorCountry,
                                            VendorPostalCode = @VendorPostalCode, 
                                            ShipToAddress1 = @ShipToAddress1,
                                            ShipToAddress2 = @ShipToAddress2,
                                            ShipToCity = @ShipToCity,
                                            ShipToState =@ShipToState,
                                            ShipToCountry =@ShipToCountry,
                                            ShipToPostalCode =@ShipToPostalCode,
                                            ShipToAddressRemarks =@ShipToAddressRemarks,
                                            RequestShipDate =@RequestShipDate,
                                            OrderRemarks =@OrderRemarks, 
                                            ExternalSystemReference = @ExternalSystemReference, 
                                            OrderTotal =@OrderTotal, 
                                            LastModUserId =@LastModUserId, 
                                            LastModDttm =getdate(), 
                                            ReceivedDate = @ReceivedDate,
                                            --site_id = @site_id,
                                            CancelledDate =@CancelledDate,
                                            MasterEntityId =@MasterEntityId,
                                            IsCreditPO =@IsCreditPO,
                                            DocumentTypeID =@DocumentTypeID,
                                            Fromdate =@Fromdate, 
                                            ToDate =@ToDate,
                                            ReprintCount =@ReprintCount,
                                            AmendmentNumber =@AmendmentNumber,
                                            DocumentStatus =@DocumentStatus,
                                            FromSiteId = @fromSiteId,
                                            ToSiteId = @toSiteId,
                                            OriginalReferenceGUID = @originalReferenceGUID,
                                            IsActive = @isActive
                                        where PurchaseOrderId = @PurchaseOrderId
                            SELECT * FROM PurchaseOrder WHERE PurchaseOrderId = @PurchaseOrderId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(purchaseorder, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPurchaseOrderDTO(purchaseorder, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating purchase order", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(purchaseorder);
            return purchaseorder;
        }


        /// <summary>
        /// Delete the record from the PurchaseOrder database based on PurchaseOrderId
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int purchaseOrderId)
        {
            log.LogMethodEntry(purchaseOrderId);
            string query = @"DELETE  
                             FROM PurchaseOrder
                             WHERE PurchaseOrder.PurchaseOrderId = @PurchaseOrderId";
            SqlParameter parameter = new SqlParameter("@PurchaseOrderId", purchaseOrderId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="PurchaseOrderDTO">PurchaseOrderDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPurchaseOrderDTO(PurchaseOrderDTO purchaseOrderDTO, DataTable dt)
        {
            log.LogMethodEntry(purchaseOrderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                purchaseOrderDTO.PurchaseOrderId = Convert.ToInt32(dt.Rows[0]["PurchaseOrderId"]);
                purchaseOrderDTO.LastModDttm = dataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastModDttm"]);
                purchaseOrderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                purchaseOrderDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                purchaseOrderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                purchaseOrderDTO.LastModUserId = dataRow["LastModUserId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastModUserId"]);
                purchaseOrderDTO.site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the record requied for the grid view
        /// </summary>
        /// <param name="requistionId">integer parameter</param>
        /// <param name="siteId">integer parameter</param>
        /// <param name="requisitionType">Requistion Type</param>
        /// <param name="sqlTrx">sql transaction</param>
        /// <returns>Data Table of the required fields</returns>
        public DataTable GetPORecord(int requistionId, int siteId, string requisitionType, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(requistionId, siteId, requisitionType, sqlTrx);
            string selectQuery = @"select Code, product.Description, InventoryRequisitionLines.RequestedQuantity, isnull(Marketlistitem, 0) Marketlistitem, 
	                                case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) else isnull(LastPurchasePrice, cost) end else isnull(LastPurchasePrice, cost) end as cost, 
	                                case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then tax_Percentage/100 * isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) else isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else 0 end as taxAmount, 
	                                (case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost) else isnull(LastPurchasePrice, cost) + isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else isnull(LastPurchasePrice, cost) end) * ReorderQuantity as SubTotal, 
	                                (case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost) else isnull(LastPurchasePrice, cost) + isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else isnull(LastPurchasePrice, cost) end) as CPOSubTotal, 
	                                LowerLimitCost, UpperLimitCost, CostVariancePercentage, product.ProductId, uom,InventoryRequisitionLines.UomId, requisitionid, requisitionlineid, InventoryRequisitionLines.ExpectedReceiptDate
                                from InventoryRequisitionLines, Product left outer join Tax on Tax.tax_Id = product.PurchaseTaxId 
	                                left outer join uom on uom.uomid = product.uomid 
                                where Product.productId = " + ((requisitionType.Equals("ITRQ")) ? "(select prd.MasterEntityId from Product prd where prd.ProductId = InventoryRequisitionLines.productid ) " : " InventoryRequisitionLines.productid ") +
                                    @"AND Product.IsActive = 'Y' 
	                                and isPurchaseable = 'Y' 
	                                and isnull(InventoryRequisitionLines.IsActive, 0) = 1
	                                and InventoryRequisitionLines.RequisitionId = @requisitionId and (InventoryRequisitionLines.site_id = @siteId or @siteId = -1)";

            List<SqlParameter> purchaseOrderParameters = new List<SqlParameter>();
            purchaseOrderParameters.Add(new SqlParameter("@siteId", siteId));
            purchaseOrderParameters.Add(new SqlParameter("@requisitionId", requistionId));

            DataTable dTable = dataAccessHandler.executeSelectQuery(selectQuery, purchaseOrderParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(dTable);
            return dTable;
        }

        //public int UpdatePurchaseOrderStatus(int PurchaseorderId, SqlTransaction sqlTrx)
        public int UpdatePurchaseOrderStatus(PurchaseOrderDTO purchaseOrderDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(purchaseOrderDTO, sqlTrx);

            string updatePOQuery = @"update purchaseorder 
                                    set orderstatus = 'InProgress' 
                                    where purchaseorderid = @id 
                                    and exists (select pol.itemcode, pol.description, pol.ordqty, pol.ordqty-isnull(sum(r.quantity), 0) remqty 
                                                from (select pl.purchaseorderid, prd.productid, prd.code itemcode, prd.description, sum(quantity) ordqty 
                                                    	from product prd, purchaseorder_line pl 
                                                    	where pl.purchaseorderid = @id 
                                                        and pl.productId = prd.productId 
                                                    	and isnull(pl.isactive, 'Y') = 'Y' 
                                                    	group by pl.purchaseorderid, prd.productid, prd.code, prd.description) pol 
                                                    left outer join purchaseorderreceive_line r 
                                                on pol.purchaseorderid = r.purchaseorderid 
                                                and pol.productid = r.productid 
                                                group by pol.itemcode, pol.description, pol.ordqty, pol.ordqty 
                                                having pol.ordqty - isnull(sum(r.quantity), 0) > 0)";
            List<SqlParameter> updatePurchaseOrderParameters = new List<SqlParameter>();
            //updatePurchaseOrderParameters.Add(new SqlParameter("@id", PurchaseorderId));
            updatePurchaseOrderParameters.Add(new SqlParameter("@id", purchaseOrderDTO.PurchaseOrderId));

            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updatePOQuery, updatePurchaseOrderParameters.ToArray(), sqlTrx);
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Returns the no of PurchaseOrderDTO matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of PO matching the criteria</returns>
        public int GetPurchaseOrderCount(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> searchParameters, string loginId = null)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectQuery = string.Empty;
            selectQuery += @"declare @userid int;select @userid = user_id from users where loginId = @loginUserId ;exec dbo.SetContextInfo @userid ;select *
                                         from purchaseorder  po ";
            parameters.Add(new SqlParameter("@loginUserId", loginId));

            selectQuery += GetFilterQuery(searchParameters, loginId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                count = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// Gets the PurchaseOrderDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqltrxn">SqlTrasanction object</param>
        /// <returns>Returns the list of PurchaseOrderDTO matching the search criteria</returns>
        public List<PurchaseOrderDTO> GetMostRepeatedPurchaseOrderList(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> searchParameters, string loginId,
                                                            int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<PurchaseOrderDTO> purchaseOrderDTOList = new List<PurchaseOrderDTO>();
            parameters.Clear();

            string selectQuery = @"declare @userid int;select @userid = user_id from users where loginId = @loginUserId ;exec dbo.SetContextInfo @userid ;
                                    Select * from purchaseOrder po 
                                         ";
            parameters.Add(new SqlParameter("@loginUserId", loginId));

            selectQuery += GetFilterQuery(searchParameters, loginId);
            selectQuery += @" and OrderDate >= GetDate()-30 and OrderStatus = 'Open' or OrderStatus ='ShortClose' and po.VendorId in(Select VendorId from PurchaseOrder group by(VendorId) having COUNT(VendorId) > 0) and purchaseOrderId in
                                    (select PurchaseOrderId from PurchaseOrder_Line where productId in
                                    (Select productId from PurchaseOrder_Line group by ProductId having COUNT(ProductId) > 0
                                     )) order by OrderDate desc";
            //if (currentPage > 0 || pageSize > 0)
            //{
            //    selectQuery += " OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
            //    selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            //}

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                purchaseOrderDTOList = new List<PurchaseOrderDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PurchaseOrderDTO purchaseOrderDTO = GetPurchaseOrderDTO(dataRow);
                    purchaseOrderDTOList.Add(purchaseOrderDTO);
                }
            }
            log.LogMethodExit(purchaseOrderDTOList);
            return purchaseOrderDTOList;
        }

    }
}
