/********************************************************************************************
 * Project Name - Vendor Return Data Handler
 * Description  - Data handler of the vendor Return type class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Aug-2016   Suneetha.S          Created 
 *2.70.2        09-Jul-2019   Girish Kundar    Modified : LogMethodEntry() and LogMethodExit()
 *                                                     
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    ///  Type Data Handler - Handles insert, update and select of Requisition type objects
    /// </summary>
    public class VendorReturnsDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        ///  Type Data Handler - Handles insert, update and select of Requisition type objects
        /// </summary>

         
        public VendorReturnsDataHandler(SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to Vendor Return DTO class type
        /// </summary>
        /// <param name="vendorReturnDataRow">VendorReturnsDTO DataRow</param>
        /// <returns>Returns RequisitionsDTO</returns>
        private VendorReturnsDTO GetVendorReturnDTO(DataRow vendorReturnDataRow)
        {
            log.LogMethodEntry(vendorReturnDataRow);
            VendorReturnsDTO vendorReturnsDTO = new VendorReturnsDTO(
                                                          Convert.ToInt32(vendorReturnDataRow["ReceiptId"]),
                                                          vendorReturnDataRow["PO"] == DBNull.Value ? string.Empty : Convert.ToString(vendorReturnDataRow["PO"]),
                                                          vendorReturnDataRow["OrderDate"] == DBNull.Value ? DateTime.MinValue :  Convert.ToDateTime(vendorReturnDataRow["OrderDate"]),
                                                          vendorReturnDataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(vendorReturnDataRow["Name"]),
                                                          vendorReturnDataRow["receiptAmount"] == DBNull.Value ? 0 : Convert.ToDouble(vendorReturnDataRow["RequestingDept"]),
                                                          vendorReturnDataRow["OrderNumber"] == DBNull.Value ? string.Empty : Convert.ToString(vendorReturnDataRow["OrderNumber"]),
                                                          vendorReturnDataRow["VendorBillNumber"] == DBNull.Value ? string.Empty : Convert.ToString(vendorReturnDataRow["VendorBillNumber"]),
                                                          vendorReturnDataRow["GRN"] == DBNull.Value ? string.Empty : Convert.ToString(vendorReturnDataRow["GRN"])                                                         
                                                         );
            log.LogMethodExit(vendorReturnsDTO);
            return vendorReturnsDTO;
        }

        /// <summary>
        /// Converts the Data row object to Vendor Return DTO class type
        /// </summary>
        /// <param name="vendorReturnDataRow">VendorReturnsDTO DataRow</param>
        /// <returns>Returns RequisitionsDTO</returns>
        private VendorReturnsDTO GetVendorReturnDTOForReceipt(DataRow vendorReturnDataRow)
        {
            log.LogMethodEntry(vendorReturnDataRow);
            VendorReturnsDTO vendorReturnsDTO = new VendorReturnsDTO(
                                                          vendorReturnDataRow["OrderNumber"] == DBNull.Value ? string.Empty : Convert.ToString(vendorReturnDataRow["OrderNumber"]),
                                                          Convert.ToDateTime(vendorReturnDataRow["OrderDate"]),
                                                          Convert.ToString(vendorReturnDataRow["Name"]),
                                                         vendorReturnDataRow["receiptAmount"] == DBNull.Value ? 0 : Convert.ToDouble(vendorReturnDataRow["receiptAmount"])
                                                         );
            log.LogMethodExit(vendorReturnsDTO);
            return vendorReturnsDTO;
        }

        /// <summary>
        /// Gets the receipts data of passed POId and ReceiptId
        /// <param name="poId">integer type parameter</param>
        /// <param name="receiptId">integer type parameter</param>
        /// <returns>Returns VendorReturnsDTO</returns>
        /// </summary>
        public VendorReturnsDTO GetReceiptsList(int poId, int receiptId)
        {
            log.LogMethodEntry(poId, receiptId);
            VendorReturnsDTO receiptsData = null;
            string selectReceiptsquery = @"select OrderNumber, OrderDate, v.Name, (select sum(amount) from PurchaseOrderreceive_line where ReceiptId = @receiptId) receiptAmount 
                                        from PurchaseOrder po, vendor v 
                                        where po.vendorId = v.vendorId
                                        and po.PurchaseOrderId = @id";
            SqlParameter[] selectReceiptParameters = new SqlParameter[2];
            selectReceiptParameters[0] = new SqlParameter("@id", poId);
            selectReceiptParameters[1] = new SqlParameter("@receiptId", receiptId);
            DataTable receiptsDT = dataAccessHandler.executeSelectQuery(selectReceiptsquery, selectReceiptParameters,sqlTransaction);
            if (receiptsDT.Rows.Count > 0)
            {
                receiptsData = new VendorReturnsDTO();
                foreach (DataRow receiptRow in receiptsDT.Rows)
                {
                    receiptsData = GetVendorReturnDTOForReceipt(receiptRow);
                    log.LogMethodExit(receiptRow);
                }
            }
            log.LogMethodExit(receiptsData);
            return receiptsData;
        }

        /// <summary>
        /// Gets the receipts data of passed POId and ReceiptId
        /// </summary>
        /// <param name="vendorName">string type parameter</param>
        /// <param name="orderNo">string type parameter</param>
        /// <param name="vendorBillNo">string type parameter</param>
        ///  <param name="grn">string type parameter</param>
        ///   <param name="siteId">integer type parameter</param>
        /// <returns>Returns DataTable</returns>
        public DataTable GetReceiptsIdsOnSearchKeys(string vendorName, string orderNo, string vendorBillNo, string grn, int siteId)
        {
            log.LogMethodEntry(vendorName, orderNo,vendorBillNo, grn);
            string incmd = string.Empty;
            DataTable receiptsDT = new DataTable();
            string condition = string.Empty;
            if (siteId != -1)
            {
                condition = "and (r.site_id = @site_id or @site_id = -1) ";
            }
            string selectReceiptsquery = @"select ReceiptId " +
                                        "from Receipt r " +
                                        "where exists (select 1 " +
                                                        "from PurchaseOrder po, vendor v " +
                                                        "where po.PurchaseOrderId = r.PurchaseOrderId " +
                                                        "and po.vendorId = v.vendorId " +
                                                        "and (v.name like '%' + @Vendor + '%' or @Vendor = '') " +
                                                        "and (po.OrderNumber like '%' + @OrderNumber + '%' or @OrderNumber = '')) " +
                                        "and (VendorBillNumber like '%' + @VendorBillNumber + '%' or @VendorBillNumber = '') " +
                                        "and (GRN like '%' + @GRN + '%' or @GRN = '') " + condition + " order by ReceiveDate Desc";
            SqlParameter[] selectReceiptParameters = new SqlParameter[4];
            selectReceiptParameters[0] = new SqlParameter("@Vendor", vendorName);
            selectReceiptParameters[1] = new SqlParameter("@OrderNumber", orderNo);
            selectReceiptParameters[2] = new SqlParameter("@VendorBillNumber", vendorBillNo);
            selectReceiptParameters[3] = new SqlParameter("@GRN", grn);
            receiptsDT = dataAccessHandler.executeSelectQuery(selectReceiptsquery, selectReceiptParameters,sqlTransaction);
            log.LogMethodExit(receiptsDT);
            return receiptsDT;
        }
    }
}
