using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using Semnox.Parafait.TransactionDiscounts;
//using Semnox.Parafait.DiscountCouponsUsed;
//using Semnox.Parafait.DiscountCoupons;
using Semnox.Core.Utilities;
using Semnox.Parafait.ITransaction;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.Transaction
{
    public class Order
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public class Header
        {
            public int OrderId = -1;
            public long CardId = -1;
            public string Tablenumber, CustomerName, WaiterName, Remarks, OrderStatus = "OPEN";
            public DateTime OrderDate;
            public int TableId = -1;
            public int TrxId = -1;
        }

        public Header OrderHeader;
        public Transaction Trx;

        Utilities Utilities;
        public Order(Transaction transaction)
        {
            log.LogMethodEntry(transaction);

            OrderHeader = new Header();
            Trx = transaction;
            Utilities = Trx.StaticDataExchange.Utilities;
            Trx.Order = this;
            if (Trx.PrimaryCard != null && Trx.PrimaryCard.CardStatus.Equals("ISSUED"))
                OrderHeader.CardId = Trx.PrimaryCard.card_id;

            log.LogMethodExit(null);
        }

        public void AddLines(Transaction inTrx)
        {
            log.LogMethodEntry(inTrx);

            Transaction newTrx;
            if (OrderHeader.TrxId > 0)
            {
                TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                newTrx = TransactionUtils.CreateTransactionFromDB(OrderHeader.TrxId, inTrx.StaticDataExchange);
                using (SqlConnection cnn = Utilities.createConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("", cnn))
                    {
                        cmd.Transaction = cmd.Connection.BeginTransaction();
                        try
                        {
                            foreach (Transaction.TransactionLine line in inTrx.TrxLines)
                            {
                                line.DBLineId = 0;
                                newTrx.TrxLines.Add(line);

                                if (!string.IsNullOrEmpty(line.InventoryProductCode))
                                    Inventory.updateStock(line.InventoryProductCode, cmd, line.quantity * -1, inTrx.StaticDataExchange.POSMachineId, inTrx.StaticDataExchange.LoginId, inTrx.Trx_id, line.DBLineId, line.Price, line.tax_percentage, line.TaxInclusivePrice);
                            }
                            newTrx.updateAmounts();

                            cmd.CommandText = @"delete from trxTaxLines where TrxId = @trxId;
                                                update trx_lines set Price = 0, Amount = 0, UserPrice = 0, 
                                                    credits = 0, bonus = 0, courtesy = 0, time = 0, tickets = 0,
                                                        LastUpdateDate = getdate(),
                                                        LastUpdatedBy = @user,
                                                    cancelledTime = getdate(), cancelledBy = @user 
                                                 where TrxId = @trxId;
                                                update trxPayments set trxId = @newTrxId where trxId = @trxId;
                                                update trx_header 
                                                   set status = 'CANCELLED', trxAmount = 0, TaxAmount = 0, trxNetAmount = 0
                                                      , cashAmount = 0, lastUpdateTime = getdate(), LastUpdatedBy = @user 
                                                  where TrxId = @trxId;";

                            cmd.Parameters.AddWithValue("@trxId", inTrx.Trx_id);
                            cmd.Parameters.AddWithValue("@newTrxId", newTrx.Trx_id);
                            cmd.Parameters.AddWithValue("@user", Utilities.ParafaitEnv.LoginID);
                            cmd.ExecuteNonQuery();
                            inTrx.InsertTrxLogs(inTrx.Trx_id, -1, Utilities.ParafaitEnv.LoginID, "DELETE LINES", "Order moved to another Trx " + newTrx.Trx_id.ToString(), cmd.Transaction);
                            log.LogVariableState("@trxId", inTrx.Trx_id);

                            TransactionDiscountsListBL transactionDiscountsListBL = new TransactionDiscountsListBL();
                            List<KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>> searchTransactionDiscountsParams = new List<KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>>();
                            searchTransactionDiscountsParams.Add(new KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>(TransactionDiscountsDTO.SearchByParameters.TRANSACTION_ID, inTrx.Trx_id.ToString()));
                            List<TransactionDiscountsDTO> transactionDiscountsDTOList = transactionDiscountsListBL.GetTransactionDiscountsDTOList(searchTransactionDiscountsParams, cmd.Transaction);
                            if(transactionDiscountsDTOList != null)
                            {
                                foreach (var transactionDiscountsDTO in transactionDiscountsDTOList)
                                {
                                    TransactionDiscountsBL transactionDiscountsBL = new TransactionDiscountsBL(transactionDiscountsDTO);
                                    transactionDiscountsBL.Delete(cmd.Transaction);
                                }
                            }

                            DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL();
                            List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchDiscountCouponsUsedParams = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
                            searchDiscountCouponsUsedParams.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.TRANSACTION_ID, inTrx.Trx_id.ToString()));
                            searchDiscountCouponsUsedParams.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                            List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(searchDiscountCouponsUsedParams);
                            if(discountCouponsUsedDTOList != null)
                            {
                                foreach (var discountCouponsUsedDTO in discountCouponsUsedDTOList)
                                {
                                    discountCouponsUsedDTO.IsActive = "N";
                                    DiscountCouponsUsedBL discountCouponsUsedBL = new DiscountCouponsUsedBL(discountCouponsUsedDTO);
                                    discountCouponsUsedBL.Save(cmd.Transaction);
                                }
                            }
                            DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL();
                            List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchDiscountCouponsParams = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                            searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.TRANSACTION_ID, inTrx.Trx_id.ToString()));
                            searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate? Utilities.ParafaitEnv.SiteId:-1).ToString()));
                            List<DiscountCouponsDTO> discountCouponsDTOList = discountCouponsListBL.GetDiscountCouponsDTOList(searchDiscountCouponsParams, cmd.Transaction);
                            if (discountCouponsDTOList != null)
                            {
                                foreach (var discountCouponsDTO in discountCouponsDTOList)
                                {
                                    discountCouponsDTO.IsActive = "N";
                                    DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(discountCouponsDTO);
                                    discountCouponsBL.Save(cmd.Transaction);
                                }
                            }
                            cmd.Transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while cancelling merged transaction", ex);

                            cmd.Transaction.Rollback();

                            log.LogMethodExit("Throwing Exception - "+ ex);
                            throw ex;
                        }
                    }
                }
            }
            else
                newTrx = inTrx;
            
            Trx = newTrx;
            Trx.Order = this;

            log.LogMethodExit(null);
        }

        public Order(int OrderId, Utilities Utils)
        {
            log.LogMethodEntry(Utils);

            OrderHeader = new Header();
            OrderHeader.OrderId = OrderId;
            Utilities = Utils;

            DataTable dt = Utilities.executeDataTable("select o.*, h.trxId from OrderHeader o left outer join trx_header h " +
                                "on h.OrderId = o.OrderId and h.status = 'OPEN' where o.OrderId = @OrderId ",
                                new SqlParameter("@OrderId", OrderId));

            log.LogVariableState("@OrderId", OrderId);

            OrderHeader.CustomerName = dt.Rows[0]["CustomerName"].ToString();
            OrderHeader.OrderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"]);
            OrderHeader.OrderId = Convert.ToInt32(dt.Rows[0]["OrderId"]);
            OrderHeader.OrderStatus = dt.Rows[0]["OrderStatus"].ToString();
            OrderHeader.Remarks = dt.Rows[0]["Remarks"].ToString();
            OrderHeader.Tablenumber = dt.Rows[0]["Tablenumber"].ToString();
            OrderHeader.WaiterName = dt.Rows[0]["WaiterName"].ToString();
            if (dt.Rows[0]["TableId"] != DBNull.Value)
                OrderHeader.TableId = Convert.ToInt32(dt.Rows[0]["TableId"]);
            if (dt.Rows[0]["TrxId"] != DBNull.Value)
                OrderHeader.TrxId = (int)dt.Rows[0]["TrxId"];

            log.LogMethodExit(null);
        }

        public void CreateOrder()
        {
            log.LogMethodEntry();

            using (SqlConnection cnn = Utilities.createConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {

                    if (OrderHeader.OrderId == -1)
                    {
                        cmd.CommandText = "insert into OrderHeader (TableNumber, WaiterName, CustomerName, Remarks, OrderStatus, POSMachineId, UserId, OrderDate, CardId, TableId) " +
                                          "Values (@TableNumber, @WaiterName, @CustomerName, @Remarks, @OrderStatus, @POSMachineId, @UserId, getdate(), @CardId, @TableId); select @@identity";
                        cmd.Parameters.AddWithValue("@TableNumber", OrderHeader.Tablenumber);
                        cmd.Parameters.AddWithValue("@WaiterName", OrderHeader.WaiterName);
                        cmd.Parameters.AddWithValue("@CustomerName", OrderHeader.CustomerName);
                        cmd.Parameters.AddWithValue("@Remarks", OrderHeader.Remarks);
                        cmd.Parameters.AddWithValue("@OrderStatus", OrderHeader.OrderStatus);

                        log.LogVariableState("@TableNumber", OrderHeader.Tablenumber);
                        log.LogVariableState("@WaiterName", OrderHeader.WaiterName);
                        log.LogVariableState("@CustomerName", OrderHeader.CustomerName);
                        log.LogVariableState("@Remarks", OrderHeader.Remarks);
                        log.LogVariableState("@OrderStatus", OrderHeader.OrderStatus);

                        if (Trx.StaticDataExchange.POSMachineId > 0)
                        {
                            cmd.Parameters.AddWithValue("@POSMachineId", Trx.StaticDataExchange.POSMachineId);
                            log.LogVariableState("@POSMachineId", Trx.StaticDataExchange.POSMachineId);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@POSMachineId", DBNull.Value);
                            log.LogVariableState("@POSMachineId", DBNull.Value);
                        }

                            cmd.Parameters.AddWithValue("@UserId", Trx.StaticDataExchange.UserId);
                        log.LogVariableState("@UserId", Trx.StaticDataExchange.UserId);

                        if (OrderHeader.CardId == -1)
                        {
                            cmd.Parameters.AddWithValue("@CardId", DBNull.Value);
                            log.LogVariableState("@CardId", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CardId", OrderHeader.CardId);
                            log.LogVariableState("@CardId", OrderHeader.CardId);
                        }

                            cmd.Parameters.AddWithValue("@TableId", (OrderHeader.TableId == -1 ? DBNull.Value : (object)OrderHeader.TableId));
                        log.LogVariableState("@TableId", (OrderHeader.TableId == -1 ? DBNull.Value : (object)OrderHeader.TableId));

                        OrderHeader.OrderId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    else
                    {
                        if (OrderHeader.CardId == -1)
                        {
                            if (Trx.PrimaryCard != null && Trx.PrimaryCard.CardStatus != "NEW")
                            {
                                OrderHeader.CardId = Trx.PrimaryCard.card_id;
                                cmd.CommandText = "update OrderHeader set CardId = @CardId where OrderId = @OrderId";
                                cmd.Parameters.AddWithValue("@CardId", OrderHeader.CardId);
                                cmd.Parameters.AddWithValue("@OrderId", OrderHeader.OrderId);
                                cmd.ExecuteNonQuery();

                                log.LogVariableState("@CardId", OrderHeader.CardId);
                                log.LogVariableState("@OrderId", OrderHeader.OrderId);
                            }
                        }
                    }
                }
            }

            log.LogMethodExit(null);
        }

        public void UpdateOrder()
        {
            log.LogMethodEntry();

            Utilities.executeNonQuery("update OrderHeader set TableNumber = @TableNumber, CustomerName = @CustomerName, WaiterName = @WaiterName, Remarks = @Remarks, LastUpdateTime = getdate() " +
                                        "where OrderId = @OrderId",
                                        new SqlParameter("@OrderID", OrderHeader.OrderId),
                                        new SqlParameter("@TableNumber", OrderHeader.Tablenumber),
                                        new SqlParameter("@WaiterName", OrderHeader.WaiterName),
                                        new SqlParameter("@CustomerName", OrderHeader.CustomerName),
                                        new SqlParameter("@Remarks", OrderHeader.Remarks));

            log.LogVariableState("@OrderID", OrderHeader.OrderId);
            log.LogVariableState("@TableNumber", OrderHeader.Tablenumber);
            log.LogVariableState("@WaiterName", OrderHeader.WaiterName);
            log.LogVariableState("@CustomerName", OrderHeader.CustomerName);
            log.LogVariableState("@Remarks", OrderHeader.Remarks);

            log.LogMethodExit(null);
        }

        public Transaction CreateTransactionFromOrder(staticDataExchange StaticDataExchange, ref string message)
        {
            log.LogMethodEntry(StaticDataExchange, message);

            int TrxId = (int)Utilities.executeScalar("select TrxId from trx_header where OrderId = @OrderId", 
                                                new SqlParameter("@OraderId", OrderHeader.OrderId));

            log.LogVariableState("@OraderId", OrderHeader.OrderId);

            TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
            Trx = TransactionUtils.CreateTransactionFromDB(TrxId, StaticDataExchange);

            Trx.Order = this;
            Trx.TransactionInfo.TableNumber = OrderHeader.Tablenumber;
            Trx.TransactionInfo.WaiterName = OrderHeader.WaiterName;
            Trx.TransactionInfo.OrderCustomerName = OrderHeader.CustomerName;

            log.LogVariableState("message", message);
            log.LogMethodExit(Trx);
            return Trx;
        }
    }
}
