/********************************************************************************************
 * Project Name - Common
 * Description  - UI Class for InvUtils
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80         20-Aug-2019     Girish Kundar        Modified : Added Logger methods and Removed unused namespace's 
 *********************************************************************************************/
using System;
using System.Data.SqlClient;

namespace Parafait_POS
{
    public static class InvUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static bool ReceiveTurnInGift(object productId, object turnInFromLocationId, object turnInToLocationId, int Quantity, SqlTransaction SQLTrx, ref string message)
        {
            log.LogMethodEntry(productId, turnInFromLocationId, turnInToLocationId, Quantity);
            String remarks = "Customer gift turn-in"; //Receive remarks field.
            String ordernumber = "PO-" + POSStatic.Utilities.executeScalar("select count(purchaseOrderId) + 1 from PurchaseOrder", SQLTrx).ToString() + " Auto";
            SqlCommand ins_cmd = POSStatic.Utilities.getCommand(SQLTrx);

            try
            {
                object amount = POSStatic.Utilities.executeScalar(@"select TurnInPriceInTickets * (select Convert(decimal(18, 6), default_value) 
                                                                    from parafait_defaults 
                                                                    where default_value_name = 'TICKET_COST')
                                                                    from product 
                                                                     where ProductId = @ProductId", SQLTrx, new SqlParameter("@ProductId", productId));
                if (amount == null || amount == DBNull.Value)
                    amount = 0;

                decimal total = Quantity * Convert.ToDecimal(amount);
                log.Debug("Total  :" + total);
                // New order to be inserted.
                ins_cmd.CommandText = "Insert into PurchaseOrder " +
                                              "(OrderStatus, " +
                                              "OrderNumber, " +
                                              "OrderDate, " +
                                              "VendorId, " +
                                              "ContactName, " +
                                              "Phone, VendorAddress1, VendorAddress2, VendorCity, VendorState, VendorCountry, VendorPostalCode, " +
                                              "OrderRemarks, " +
                                              "ReceiveRemarks, " +
                                              "ReceivedDate, " +
                                              "LastModUserId, " +
                                              "LastModDttm, OrderTotal, site_id )" +
                                          " Values (@OrderStatus, " +
                                              "@OrderNumber, " +
                                              "getdate(), " +
                                              "(select isnull(DefaultVendorId, (select top 1 vendorId from vendor)) from product where productId = @productId), " +
                                              "'', " +
                                              "'', '', '', '', '', '', '', " +
                                              "@OrderRemarks, " +
                                              "@ReceiveRemarks, " +
                                              "getdate(), " +
                                              "@LastModUserId, " +
                                              "getdate(), @OrderTotal, @site_id);" +
                                      " SELECT @@IDENTITY";

                ins_cmd.Parameters.AddWithValue("@OrderStatus", "Received");
                ins_cmd.Parameters.AddWithValue("@OrderNumber", ordernumber);
                ins_cmd.Parameters.AddWithValue("@OrderRemarks", remarks);
                ins_cmd.Parameters.AddWithValue("@ReceiveRemarks", turnInFromLocationId == null ? DBNull.Value : (object)turnInFromLocationId.ToString());
                ins_cmd.Parameters.AddWithValue("@LastModUserId", POSStatic.ParafaitEnv.LoginID);
                ins_cmd.Parameters.AddWithValue("@OrderTotal", total);
                ins_cmd.Parameters.AddWithValue("@productId", productId);
                ins_cmd.Parameters.AddWithValue("@site_id", DBNull.Value);

                int OrderId = Convert.ToInt32(ins_cmd.ExecuteScalar());

                SqlCommand cmdr = POSStatic.Utilities.getCommand(SQLTrx);
                cmdr.CommandText = "Insert into PurchaseOrder_Line(PurchaseOrderId, ItemCode, ProductId, Description, Quantity, UnitPrice, SubTotal, TaxAmount, site_id, TimeStamp)" +
                                    @" select @PurchaseOrderId, Code, ProductId, Description, @Quantity, 
                                            @price, @total, 0, @site_id, getdate()
                                    from product 
                                     where ProductId = @ProductId; select @@identity";

                int retPurchaseOrderLineId = -1;
                cmdr.Parameters.AddWithValue("@PurchaseOrderId", OrderId);
                cmdr.Parameters.AddWithValue("@ProductId", productId);
                cmdr.Parameters.AddWithValue("@Quantity", Quantity);
                cmdr.Parameters.AddWithValue("@price", amount);
                cmdr.Parameters.AddWithValue("@total", total);
                cmdr.Parameters.AddWithValue("@site_id", DBNull.Value);
                object o = cmdr.ExecuteScalar();
                if (o == null)
                {
                    message = "Error occurred while inserting Auto PO line";
                    log.Debug(message);
                    log.LogMethodExit(false);
                    return false;
                }
                else
                    retPurchaseOrderLineId = Convert.ToInt32(o);

                SqlCommand cmdReceipt = POSStatic.Utilities.getCommand(SQLTrx);

                cmdReceipt.CommandText = @"insert into Receipt (VendorBillNumber, GatePassNumber, GRN, PurchaseOrderId, Remarks, ReceiveDate, ReceivedBy, site_id)
                                                values (@VendorBillNumber, @GatePassNumber, @GRN, @PurchaseOrderId, @Remarks, getdate(), @ReceivedBy, @site_id); select @@identity";
                cmdReceipt.Parameters.AddWithValue("@VendorBillNumber", "");
                cmdReceipt.Parameters.AddWithValue("@GatePassNumber", "");
                cmdReceipt.Parameters.AddWithValue("@GRN", "");
                cmdReceipt.Parameters.AddWithValue("@PurchaseOrderId", OrderId);
                cmdReceipt.Parameters.AddWithValue("@Remarks", remarks);
                cmdReceipt.Parameters.AddWithValue("@ReceivedBy", POSStatic.ParafaitEnv.LoginID);
                cmdReceipt.Parameters.AddWithValue("@site_id", DBNull.Value);

                int ReceiptId = Convert.ToInt32(cmdReceipt.ExecuteScalar());

                SqlCommand cmd = POSStatic.Utilities.getCommand(SQLTrx);
                cmd.CommandText = @"Insert into PurchaseOrderReceive_Line(PurchaseOrderId, ProductId, Description, VendorItemCode, 
                                        Quantity, IsReceived, LocationId, ReceiptId, PurchaseOrderLineId, Price, TaxId, Tax_Percentage, 
                                        tax_inclusive, amount, site_id) " +
                                    @"select PurchaseOrderId, l.productId, l.Description, null, 
                                        l.quantity, 'Y', @toLocationId, @ReceiptId, 
                                        l.PurchaseOrderLineId, unitPrice, null, null, 
                                        null, subTotal, l.site_id 
                                        from PurchaseOrder_line l
                                        where PurchaseOrderId = @OrderId";

                cmd.Parameters.AddWithValue("@OrderId", OrderId);
                cmd.Parameters.AddWithValue("@ReceiptId", ReceiptId);
                cmd.Parameters.AddWithValue("@PurchaseOrderLineId", retPurchaseOrderLineId);
                cmd.Parameters.AddWithValue("@toLocationId", turnInToLocationId);
                cmd.ExecuteNonQuery();

                cmd.Parameters.AddWithValue("@Quantity", Quantity);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                cmd.CommandText = "Update Inventory " +
                                    "Set Quantity = (Quantity + @Quantity), Lastupdated_userid = @lmid, timestamp = getdate() " +
                                    "where Inventory.ProductId = @ProductId " +
                                      "and Inventory.LocationId = @toLocationId";

                cmd.Parameters.AddWithValue("@lmid", POSStatic.ParafaitEnv.LoginID);

                if (cmd.ExecuteNonQuery() == 0)
                {
                    log.Debug("Inserting Inventory Details ");
                    cmd.CommandText = "insert into Inventory (productId, locationId, Quantity, Lastupdated_userid, timestamp) " +
                                      @"values (@ProductId, @toLocationId, @Quantity, @lmid, getdate())"; 
                    cmd.ExecuteNonQuery();
                }

                if (turnInFromLocationId != DBNull.Value)
                {
                    cmd.Parameters.AddWithValue("@inLocationId", turnInFromLocationId);
                    cmd.CommandText = "Update Inventory " +
                                    "Set Quantity = (Quantity - @Quantity), Lastupdated_userid = @lmid, timestamp = getdate() " +
                                    "where ProductId = @ProductId " +
                                      "and Inventory.LocationId = @inLocationId";

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        cmd.CommandText = "insert into Inventory (productId, locationId, Quantity, Lastupdated_userid, timestamp) " +
                                          "values (@ProductId, @inLocationId, @Quantity * -1, @lmid, getdate())";
                        cmd.ExecuteNonQuery();
                    }
                }
                
            }
            catch (Exception Ex)
            {
                message = Ex.Message;
                log.Error("Exception :", Ex);
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }
    }
}
