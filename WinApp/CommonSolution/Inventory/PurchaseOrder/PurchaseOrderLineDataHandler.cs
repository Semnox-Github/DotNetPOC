/********************************************************************************************
 * Project Name -PurchaseOrderLineDataHandler
 * Description  -Data object of asset PurchaseOrderLine
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 **********************************************************************************************
 *2.60        11-Apr-2019       Girish Kundar      Modified :Added PurchaseTaxId field in InsertPurchaseOrderLine ,
 *                                                           GetPurchaseOrderLineDTO and UpdatePurchaseOrderLine  Methods
 *2.70.2      16-jul-2019       Deeksha            Modified :Added GetSqlParameter(),SQL injection issue Fix 
 *2.70.2      09-Dec-2019       Jinto Thomas       Removed siteid from update query 
*2.100.0      27-Jul-2020       Deeksha            Modified :Added UOMid field.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class PurchaseOrderLineDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PurchaseOrder_Line AS pl ";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Dictionary for searching Parameters for the PurchaseOrderLine object.
        /// </summary>
        private static readonly Dictionary<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string> DBSearchParameters = new Dictionary<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>
               {
                    {PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PURCHASE_ORDER_LINE_ID, "pl.PurchaseOrderLineId"},
                    {PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PURCHASE_ORDER_ID, "pl.PurchaseOrderId"},
                    {PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PRODUCT_ID, "pl.ProductId"},
                    {PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.IS_ACTIVE, "pl.isactive"},
                    {PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PURCHASE_ORDER_IDS, "pl.PurchaseOrderId"},
                    {PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.ORIGINAL_REFERENCE_GUID, "pl.OriginalReferenceGUID"},
                    {PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.MASTER_ENTITY_ID, "pl.MasterEntityId"},
                    {PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.SITE_ID, "pl.site_id"},
                    {PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.UOM_ID, "pl.UOMId"}
               };


        /// <summary>
        /// Default constructor of PurchaseOrderDataHandler class
        /// </summary>
        public PurchaseOrderLineDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PurchaseOrderDataHandler  Record.
        /// </summary>
        /// <param name="PurchaseOrderLineDTO">PurchaseOrderLineDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PurchaseOrderLineDTO purchaseOrderLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderLineDTO, loginId, siteId);
            double verifyDouble = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrderLineId", purchaseOrderLineDTO.PurchaseOrderLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrderId", purchaseOrderLineDTO.PurchaseOrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RequisitionId", purchaseOrderLineDTO.RequisitionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RequisitionLineId", purchaseOrderLineDTO.RequisitionLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ItemCode", string.IsNullOrEmpty(purchaseOrderLineDTO.ItemCode) ? DBNull.Value : (object)purchaseOrderLineDTO.ItemCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", string.IsNullOrEmpty(purchaseOrderLineDTO.Description) ? DBNull.Value : (object)purchaseOrderLineDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RequiredByDate", purchaseOrderLineDTO.RequiredByDate == DateTime.MinValue ? DBNull.Value : (object)purchaseOrderLineDTO.RequiredByDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancelledDate", purchaseOrderLineDTO.CancelledDate == DateTime.MinValue ? DBNull.Value : (object)purchaseOrderLineDTO.CancelledDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Quantity", purchaseOrderLineDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnitPrice", purchaseOrderLineDTO.UnitPrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxAmount", purchaseOrderLineDTO.TaxAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubTotal", purchaseOrderLineDTO.SubTotal));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountPercentage", purchaseOrderLineDTO.DiscountPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", purchaseOrderLineDTO.isActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", purchaseOrderLineDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnitLogisticsCost", purchaseOrderLineDTO.UnitLogisticsCost, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterentityid", purchaseOrderLineDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@priceInTickets", (Double.TryParse(purchaseOrderLineDTO.PriceInTickets.ToString(), out verifyDouble) == false) || Double.IsNaN(purchaseOrderLineDTO.PriceInTickets) || purchaseOrderLineDTO.PriceInTickets.ToString() == "" ? DBNull.Value : (object)purchaseOrderLineDTO.PriceInTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseTaxId", purchaseOrderLineDTO.PurchaseTaxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@originalReferenceGUID", string.IsNullOrEmpty(purchaseOrderLineDTO.OriginalReferenceGUID) ? DBNull.Value : (object)purchaseOrderLineDTO.OriginalReferenceGUID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", string.IsNullOrEmpty(purchaseOrderLineDTO.ExternalSystemReference) ? DBNull.Value : (object)purchaseOrderLineDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", purchaseOrderLineDTO.UOMId,true));
            log.LogMethodExit(parameters);
            return parameters;
        }



        /// <summary>
        /// Converts the Data row object to VendorDTO class type
        /// </summary>
        /// <param name="vendorDataRow">VendorDTO DataRow</param>
        /// <returns>Returns VendorDTO</returns>
        private PurchaseOrderLineDTO GetPurchaseOrderLineDTO(DataRow purchaseOrderLineDataRow)
        {
            log.LogMethodEntry(purchaseOrderLineDataRow);

            PurchaseOrderLineDTO PurchaseOrderLineDataObject = new PurchaseOrderLineDTO(Convert.ToInt32(purchaseOrderLineDataRow["PurchaseOrderLineId"]),
                                            Convert.ToInt32(purchaseOrderLineDataRow["PurchaseOrderId"]),
                                            purchaseOrderLineDataRow["ItemCode"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderLineDataRow["ItemCode"]),
                                            purchaseOrderLineDataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderLineDataRow["Description"]),
                                            purchaseOrderLineDataRow["Quantity"] == DBNull.Value ? 0 : Convert.ToDouble(purchaseOrderLineDataRow["Quantity"]),
                                            purchaseOrderLineDataRow["UnitPrice"] == DBNull.Value ? 0 : Convert.ToDouble(purchaseOrderLineDataRow["UnitPrice"]),
                                            purchaseOrderLineDataRow["SubTotal"] == DBNull.Value ? 0 : Convert.ToDouble(purchaseOrderLineDataRow["SubTotal"]),
                                            purchaseOrderLineDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderLineDataRow["Timestamp"]),
                                            purchaseOrderLineDataRow["TaxAmount"] == DBNull.Value ? 0 : Convert.ToDouble(purchaseOrderLineDataRow["TaxAmount"]),
                                            purchaseOrderLineDataRow["DiscountPercentage"] == DBNull.Value ? 0 : Convert.ToDouble(purchaseOrderLineDataRow["DiscountPercentage"]),
                                            purchaseOrderLineDataRow["RequiredByDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderLineDataRow["RequiredByDate"]),
                                            purchaseOrderLineDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderLineDataRow["site_id"]),
                                            purchaseOrderLineDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderLineDataRow["ProductId"]),
                                            purchaseOrderLineDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderLineDataRow["Guid"]),
                                            purchaseOrderLineDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(purchaseOrderLineDataRow["SynchStatus"]),
                                            purchaseOrderLineDataRow["isActive"] == DBNull.Value ? "Y" : purchaseOrderLineDataRow["isActive"].ToString(),
                                            purchaseOrderLineDataRow["CancelledDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderLineDataRow["CancelledDate"]),
                                            purchaseOrderLineDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderLineDataRow["MasterEntityId"]),
                                            purchaseOrderLineDataRow["RequisitionId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderLineDataRow["RequisitionId"]),
                                            purchaseOrderLineDataRow["RequisitionLineId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderLineDataRow["RequisitionLineId"]),
                                            purchaseOrderLineDataRow["UnitLogisticsCost"] == DBNull.Value ? 0 : Convert.ToDouble(purchaseOrderLineDataRow["UnitLogisticsCost"]),
                                            purchaseOrderLineDataRow["PriceInTickets"] == DBNull.Value ? double.NaN : Convert.ToDouble(purchaseOrderLineDataRow["PriceInTickets"]),
                                            purchaseOrderLineDataRow["OriginalReferenceGUID"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderLineDataRow["OriginalReferenceGUID"]),
                                            purchaseOrderLineDataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderLineDataRow["ExternalSystemReference"]),
                                            purchaseOrderLineDataRow["PurchaseTaxId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderLineDataRow["PurchaseTaxId"]),
                                            purchaseOrderLineDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderLineDataRow["CreatedBy"]),
                                            purchaseOrderLineDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderLineDataRow["CreationDate"]),
                                            purchaseOrderLineDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderLineDataRow["LastUpdatedBy"]),
                                            purchaseOrderLineDataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderLineDataRow["LastupdateDate"]),
                                            purchaseOrderLineDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderLineDataRow["UOMId"])
                                            );
            log.LogMethodExit(PurchaseOrderLineDataObject);
            return PurchaseOrderLineDataObject;
        }


        /// <summary>
        /// Gets the purchase order line for the purchaseorderlineid passed
        /// </summary>
        /// <param name="PurchaseOrderId">integer type parameter</param>
        /// <returns>Returns PurchaseOrderLineDTO</returns>
        public PurchaseOrderLineDTO GetPurchaseOrderLine(int PurchaseOrderLineId)
        {
            log.LogMethodEntry(PurchaseOrderLineId);
            string selectPurchaseOrderLineQuery = @"select *
                                         from PurchaseOrder_line
                                        where PurchaseOrderLineId = @PurchaseOrderLineId";
            PurchaseOrderLineDTO purchaseorderlineDataObject = null;
            SqlParameter[] selectPurchaseOrderLineParameters = new SqlParameter[1];
            selectPurchaseOrderLineParameters[0] = new SqlParameter("@PurchaseOrderLineId", PurchaseOrderLineId);
            DataTable purchaseorderline = dataAccessHandler.executeSelectQuery(selectPurchaseOrderLineQuery, selectPurchaseOrderLineParameters, sqlTransaction);
            if (purchaseorderline.Rows.Count > 0)
            {
                DataRow purchaseorderlineRow = purchaseorderline.Rows[0];
                purchaseorderlineDataObject = GetPurchaseOrderLineDTO(purchaseorderlineRow);
            }
            log.LogMethodExit(purchaseorderlineDataObject);
            return purchaseorderlineDataObject;

        }

        /// <summary>
        /// Returns the purchaseorderline table columns
        /// </summary>
        /// <returns></returns>
        public DataTable GetPurchaseOrderLineColumns()
        {
            log.LogMethodEntry();
            string selectPurchaseOrderLineQuery = "Select columns from(Select '' as columns UNION ALL Select COLUMN_NAME as columns from INFORMATION_SCHEMA.COLUMNS  Where TABLE_NAME='Purchaseorder_line') a order by columns";
            DataTable purchasaeorderlineTableColumns = dataAccessHandler.executeSelectQuery(selectPurchaseOrderLineQuery, null, sqlTransaction);
            log.LogMethodExit(purchasaeorderlineTableColumns);
            return purchasaeorderlineTableColumns;
        }

        /// <summary>
        /// Retriving purchaseorder by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the purchaseorder</param>
        /// <returns> List of PurchaseOrderDTO </returns>
        public List<PurchaseOrderLineDTO> GetPurchaseOrderLineList(string sqlQuery)
        {
            log.LogMethodEntry(sqlQuery);
            string Query = sqlQuery.ToUpper();
            if (Query.Contains("DROP") || Query.Contains("UPDATE") || Query.Contains("DELETE"))
            {
                log.LogMethodExit("Ends-GetPurchaseOrderLineList(sqlQuery) Method by invalid query.");
                return null;
            }
            DataTable purchaseorderData = dataAccessHandler.executeSelectQuery(sqlQuery, null, sqlTransaction);
            if (purchaseorderData.Rows.Count > 0)
            {
                List<PurchaseOrderLineDTO> purchaseorderlineList = new List<PurchaseOrderLineDTO>();
                foreach (DataRow purchaseorderlineDataRow in purchaseorderData.Rows)
                {
                    PurchaseOrderLineDTO purchaseorderlineDataObject = GetPurchaseOrderLineDTO(purchaseorderlineDataRow);
                    purchaseorderlineList.Add(purchaseorderlineDataObject);
                }
                log.LogMethodExit(purchaseorderlineList);
                return purchaseorderlineList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Gets the PurchaseOrderLineDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of PurchaseOrderDTO matching the search criteria</returns>
        public List<PurchaseOrderLineDTO> GetPurchaseOrderLineList(List<KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<PurchaseOrderLineDTO> purchaseOrderLineDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {

                        if (searchParameter.Key.Equals(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PURCHASE_ORDER_LINE_ID)
                            || searchParameter.Key.Equals(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PURCHASE_ORDER_ID)
                            || searchParameter.Key.Equals(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.MASTER_ENTITY_ID)
                            || searchParameter.Key.Equals(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.UOM_ID)
                            || searchParameter.Key.Equals(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PRODUCT_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key.Equals(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.ORIGINAL_REFERENCE_GUID))
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PURCHASE_ORDER_IDS))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    count++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                purchaseOrderLineDTOList = new List<PurchaseOrderLineDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PurchaseOrderLineDTO purchaseOrderLineDTO = GetPurchaseOrderLineDTO(dataRow);
                    purchaseOrderLineDTOList.Add(purchaseOrderLineDTO);
                }
            }
            log.LogMethodExit(purchaseOrderLineDTOList);
            return purchaseOrderLineDTOList;
        }

        /// <summary>
        /// Inserts the record to Purchase order line
        /// </summary>
        /// <param name="purchaseorderline"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public PurchaseOrderLineDTO Insert(PurchaseOrderLineDTO purchaseorderline, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseorderline, loginId, siteId);
            string query = @"INSERT INTO[dbo].[PurchaseOrder_Line]
                                                (PurchaseOrderId, 
                                                ItemCode,
                                                Description,
                                                Quantity,
                                                RequiredByDate,
                                                UnitPrice,
                                                TaxAmount,
                                                SubTotal,
                                                DiscountPercentage, 
                                                site_id, 
                                                ProductId,
                                                Guid,
                                                TimeStamp,
                                                CancelledDate, 
                                                isActive,
                                                masterentityid,
                                                RequisitionId,
                                                RequisitionLineId,
                                                UnitLogisticsCost,
                                                PriceInTickets,
                                                OriginalReferenceGUID,
                                                ExternalSystemReference,
                                                CreatedBy ,
                                                CreationDate ,
                                                LastUpdatedBy ,
                                                LastUpdateDate ,
                                                PurchaseTaxId,
                                                UOMId)
                                        Values 
                                                (@PurchaseOrderId, 
                                                @ItemCode,
                                                @Description,
                                                @Quantity,
                                                @RequiredByDate,
                                                @UnitPrice,
                                                @TaxAmount,
                                                @SubTotal,
                                                @DiscountPercentage, 
                                                @site_id, 
                                                @productId, 
                                                NEWID(),
                                                getdate(), 
                                                @CancelledDate,
                                                @isActive,
                                                @masterentityid,
                                                @RequisitionId,
                                                @RequisitionLineId,
                                                @UnitLogisticsCost,
                                                @priceInTickets,
                                                @originalReferenceGUID,
                                                @ExternalSystemReference,
                                                @createdBy,
                                                GetDate(),
                                                @createdBy,
                                                GetDate(),
                                                @purchaseTaxId,
                                                @UOMId
                                               )SELECT* FROM PurchaseOrder_Line WHERE PurchaseOrderLineId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(purchaseorderline, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPurchaseOrderLineDTO(purchaseorderline, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting purchaseorderline", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(purchaseorderline);
            return purchaseorderline;
        }

        /// <summary>
        /// Updates the Purchase OrderLine record
        /// </summary>
        /// <param name="purchaseorderline"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public PurchaseOrderLineDTO Update(PurchaseOrderLineDTO purchaseorderline, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseorderline, loginId, siteId);
            string query = @"UPDATE  [dbo].[PurchaseOrder_Line]
                                    SET     ItemCode = @ItemCode,
                                            Description = @Description,
                                            Quantity = @Quantity,
                                            RequiredByDate = @RequiredByDate, 
                                            UnitPrice = @UnitPrice, 
                                            taxAmount = @taxAmount,
                                            SubTotal = @SubTotal, 
                                            DiscountPercentage = @DiscountPercentage, 
                                            ProductId = @productId, 
                                            TimeStamp = getdate(), 
                                            isactive = @isactive, 
                                            cancelleddate=@cancelleddate,
                                            --site_id = @site_id,
                                            masterentityid = @masterentityid,
                                            RequisitionId = @RequisitionId,
                                            RequisitionLineId = @RequisitionLineId,
                                            UnitLogisticsCost = @UnitLogisticsCost,
                                            PriceInTickets = @priceInTickets,
                                            originalReferenceGUID = @originalReferenceGUID,
                                            ExternalSystemReference = @ExternalSystemReference,
                                            LastUpdatedBy = @createdBy,
                                            LastUpdateDate= GetDate() ,
                                            PurchaseTaxId=@purchaseTaxId,
                                            UOMId=@UOMId
                                          where PurchaseOrderLineId = @PurchaseOrderLineId
            SELECT* FROM PurchaseOrder_Line WHERE PurchaseOrderLineId = @PurchaseOrderLineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(purchaseorderline, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPurchaseOrderLineDTO(purchaseorderline, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating purchaseorderline", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(purchaseorderline);
            return purchaseorderline;
        }

        /// <summary>
        /// Delete the record from the purchaseOrderLine database based on purchaseOrderLineId
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int purchaseOrderLineId)
        {
            log.LogMethodEntry(purchaseOrderLineId);
            string query = @"DELETE  
                             FROM PurchaseOrder_Line
                             WHERE PurchaseOrder_Line.PurchaseOrderLineId = @PurchaseOrderLineId";
            SqlParameter parameter = new SqlParameter("@PurchaseOrderLineId", purchaseOrderLineId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="PurchaseOrderLineDTO">PurchaseOrderLineDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPurchaseOrderLineDTO(PurchaseOrderLineDTO purchaseOrderLineDTO, DataTable dt)
        {
            log.LogMethodEntry(purchaseOrderLineDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                purchaseOrderLineDTO.PurchaseOrderLineId = Convert.ToInt32(dt.Rows[0]["PurchaseOrderLineId"]);
                purchaseOrderLineDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                purchaseOrderLineDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                purchaseOrderLineDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                purchaseOrderLineDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                purchaseOrderLineDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                purchaseOrderLineDTO.site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
    }
}
