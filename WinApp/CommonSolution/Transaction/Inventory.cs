using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    public static class Inventory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum AdjustmentTypes
        {
            Adjustment,
            Transfer,
            Payout,
            TradeIn
        };

        public static bool checkValidInvProduct(string ProductCode, SqlCommand InvCmd) // check is linked inv product code exists in inv db
        {
            log.LogMethodEntry(ProductCode, InvCmd);
            
            InvCmd.CommandText = "select 1 " +
                              "from Product " +
                              "where Code = @Code";
            InvCmd.Parameters.Clear();
            InvCmd.Parameters.AddWithValue("@Code", ProductCode);

            log.LogVariableState("@Code", ProductCode);

            bool retrnValueNew = (InvCmd.ExecuteScalar() == null ? false : true);
            log.LogMethodExit(retrnValueNew);
            return retrnValueNew;
        }

        public static decimal getStock(string ProductCode, SqlCommand InvCmd, string POSMachine, int siteId)
        {
            log.LogMethodEntry(ProductCode, InvCmd, POSMachine, siteId);

            InvCmd.CommandText = "select 1 from Product P " + // ignore if its a BOM product
                    "where P.Code = @Code " +
                    (siteId == -1 ? "" : " and P.Site_id = @siteId ")+
                    "and exists (select 1 from ProductBOM b where b.productId = P.productId) " +
                    "and isnull(Includeinplan, 0) = 0" +
                    "union all " +
                    "select isnull(sum(quantity), 0) " +
                    "from Inventory I, Product P  " +
                    "where P.Code = @Code " +
                      (siteId == -1 ? "" : " and P.Site_id = @siteId ") +
                    "and not exists (select 1 from ProductBOM b where b.productId = P.productId) " +
                    "and P.productId = I.ProductId " +
                    "and I.LocationId = (select isnull(pos.InventoryLocationId, p.outboundLocationId) " +
                                       "from (select 1 a) v left outer join POSMachines pos " +
                                       "on posName = @posName " + (siteId == -1 ? "" : " and pos.Site_id = @siteId ") + ")";
            InvCmd.Parameters.Clear();
            InvCmd.Parameters.AddWithValue("@Code", ProductCode);
            InvCmd.Parameters.AddWithValue("@posName", POSMachine);
            InvCmd.Parameters.AddWithValue("@siteId", siteId);

            log.LogVariableState("@Code", ProductCode);
            log.LogVariableState("@posName", POSMachine);
            log.LogVariableState("@siteId", siteId);

            decimal returnValueNew = (Convert.ToDecimal(InvCmd.ExecuteScalar()));
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        public static void updateStock(string ProductCode, SqlCommand InvCmd, decimal Quantity, int POSMachine, string loginId, long ParafaitTrxId, int lineId, double Price, double TaxPercentage, string TaxInclusive, int siteId, int originalTrxId, int originalLineId,string transactionType = null )
        {
            log.LogMethodEntry(ProductCode, InvCmd, Quantity, POSMachine, loginId, ParafaitTrxId, lineId, Price, TaxPercentage, TaxInclusive, siteId, originalTrxId,originalLineId, transactionType);

            InvCmd.CommandText = "select ProductId from product where Code = @Code " + (siteId == -1 ? "" : " and Site_id = @siteId "); 
            InvCmd.Parameters.Clear();
            InvCmd.Parameters.AddWithValue("@Code", ProductCode);
            InvCmd.Parameters.AddWithValue("@siteId", siteId);

            log.LogVariableState("@Code", ProductCode);
            log.LogVariableState("@SiteId", siteId);
            log.LogVariableState("InvCmd.CommandText: ", InvCmd.CommandText);

            object productId = InvCmd.ExecuteScalar(); // inv prod code may not exist


            if (productId != null)
            {
                InvCmd.CommandText = "exec UpdateStock @ProductId, @Quantity, @POSMachine, @login, @ParafaitTrxId, @lineId, @Price, @TaxPercentage, @TaxInclusive, @transactionType,@originalTrxId,@originalLineId";
                InvCmd.Parameters.Clear();
                InvCmd.Parameters.AddWithValue("@ProductId", productId);
                InvCmd.Parameters.AddWithValue("@quantity", Quantity);
                InvCmd.Parameters.AddWithValue("@POSMachine", POSMachine);
                InvCmd.Parameters.AddWithValue("@login", loginId);
                if (ParafaitTrxId == -1)
                    InvCmd.Parameters.AddWithValue("@ParafaitTrxId", DBNull.Value);
                else
                    InvCmd.Parameters.AddWithValue("@ParafaitTrxId", ParafaitTrxId);

                InvCmd.Parameters.AddWithValue("@lineId", lineId);
                InvCmd.Parameters.AddWithValue("@Price", Price);
                InvCmd.Parameters.AddWithValue("@TaxPercentage", TaxPercentage);
                InvCmd.Parameters.AddWithValue("@TaxInclusive", TaxInclusive);
                if (transactionType == null)
                    InvCmd.Parameters.AddWithValue("@transactionType", DBNull.Value);
                else
                    InvCmd.Parameters.AddWithValue("@transactionType", transactionType);
                if (originalTrxId == -1)
                    InvCmd.Parameters.AddWithValue("@originalTrxId", DBNull.Value);
                else
                    InvCmd.Parameters.AddWithValue("@originalTrxId", originalTrxId);
                if (originalLineId == -1)
                    InvCmd.Parameters.AddWithValue("@originalLineId", DBNull.Value);
                else
                    InvCmd.Parameters.AddWithValue("@originalLineId", originalLineId);
                InvCmd.ExecuteNonQuery();

                log.LogVariableState("@ProductId", productId);
                log.LogVariableState("@quantity", Quantity);
                log.LogVariableState("@POSMachine", POSMachine);
                log.LogVariableState("@login", loginId);
                log.LogVariableState("@ParafaitTrxId", ParafaitTrxId);
                log.LogVariableState("@lineId", lineId);
                log.LogVariableState("@Price", Price);
                log.LogVariableState("@TaxPercentage", TaxPercentage);
                log.LogVariableState("@TaxInclusive", TaxInclusive);
                log.LogVariableState("@transactionType", transactionType);
            }

            log.LogMethodExit(null);
        }

        public static void updateComboStock(int ProductId, SqlCommand InvCmd, long ParafaitTrxId, int lineId, int Quantity, double TaxPercentage, string TaxInclusive, int POSMachine, string loginId)
        {
            log.LogMethodEntry(ProductId, InvCmd, ParafaitTrxId, lineId, Quantity, TaxPercentage, TaxInclusive, POSMachine, loginId);

            InvCmd.CommandText = @"select invP.Code, invP.productId ChildInvProductId, cp.Quantity  
                                    from ComboProduct cp, products p, product invP  
                                   where cp.product_id = @product_id  
                                     and cp.ChildProductId = p.product_id
                                     and p.product_id = invP.manualproductId 
                                     and isnull(cp.IsActive, 1) = 1 ";
            InvCmd.Parameters.Clear();
            InvCmd.Parameters.AddWithValue("@product_id", ProductId);

            log.LogVariableState("@product_id", ProductId);

            SqlDataAdapter da = new SqlDataAdapter(InvCmd);
            DataTable dtChildProducts = new DataTable();
            da.Fill(dtChildProducts);

            for (int i = 0; i < dtChildProducts.Rows.Count; i++)
            {
                int ChildInvProductId = Convert.ToInt32(dtChildProducts.Rows[i]["ChildInvProductId"]);

                InvCmd.CommandText = "exec UpdateStock @ProductId, @Quantity, @POSMachine, @login, @ParafaitTrxId, @lineId";
                InvCmd.Parameters.Clear();
                InvCmd.Parameters.AddWithValue("@ProductId", ChildInvProductId);
                int quantity = Quantity * Convert.ToInt32(dtChildProducts.Rows[i]["Quantity"]);
                InvCmd.Parameters.AddWithValue("@quantity", quantity);
                InvCmd.Parameters.AddWithValue("@POSMachine", POSMachine);
                InvCmd.Parameters.AddWithValue("@login", loginId);
                InvCmd.Parameters.AddWithValue("@ParafaitTrxId", ParafaitTrxId);
                InvCmd.Parameters.AddWithValue("@lineId", lineId);

                InvCmd.ExecuteNonQuery();

                log.LogVariableState("@ProductId", ChildInvProductId);
                log.LogVariableState("@quantity", quantity);
                log.LogVariableState("@POSMachine", POSMachine);
                log.LogVariableState("@login", loginId);
                log.LogVariableState("@ParafaitTrxId", ParafaitTrxId);
                log.LogVariableState("@lineId", lineId);
            }

            log.LogMethodExit(null);
        }

        public static void AdjustInventory(AdjustmentTypes AdjustmentType, 
                                            Utilities Utilities, 
                                            int LocationId, 
                                            int ProductId, 
                                            decimal Quantity, 
                                            string User, 
                                            string Remarks, 
                                            object SiteId = null,
                                            SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(AdjustmentType, Utilities, LocationId, ProductId, Quantity, User, Remarks, SiteId, SQLTrx);

            Utilities.executeNonQuery("insert into InventoryAdjustments ( " +
                                               "[AdjustmentType] " +
                                               ",[AdjustmentQuantity] " +
                                               ",[FromLocationId] " +
                                               ",[Remarks] " +
                                               ",[ProductId] " +
                                               ",[Timestamp] " +
                                               ",[UserId] " +
                                               ",[site_id]) " +
                                         "VALUES ( " +
                                               "@AdjustmentType " +
                                               ",@AdjustmentQuantity " +
                                               ",@FromLocationId " +
                                               ",@Remarks " +
                                               ",@ProductId " +
                                               ",getdate() " +
                                               ",@User " +
                                               ",@site_id)",
                                               SQLTrx,
                                                new SqlParameter("@AdjustmentType", AdjustmentType.ToString()),
                                                new SqlParameter("@ProductId", ProductId),
                                                new SqlParameter("@FromLocationId", LocationId),
                                                new SqlParameter("@AdjustmentQuantity", Quantity),
                                                new SqlParameter("@User", User),
                                                new SqlParameter("@Remarks", Remarks),
                                                new SqlParameter("@site_id", SiteId == null ? DBNull.Value : SiteId));

            Utilities.executeNonQuery(@"update top (1) inventory set quantity = quantity + @quantity,
                                            timestamp = getdate(), Lastupdated_userId = @User
                                            where locationId = @locationId
                                            and productId = @productId",
                                                SQLTrx,
                                                new SqlParameter("@locationId", LocationId),
                                                new SqlParameter("@productId", ProductId),
                                                new SqlParameter("@quantity", Quantity),
                                                new SqlParameter("@User", User));


            log.LogVariableState("@AdjustmentType", AdjustmentType.ToString());
            log.LogVariableState("@ProductId", ProductId);
            log.LogVariableState("@FromLocationId", LocationId);
            log.LogVariableState("@AdjustmentQuantity", Quantity);
            log.LogVariableState("@User", User);
            log.LogVariableState("@Remarks", Remarks);
            log.LogVariableState("@site_id", SiteId == null ? DBNull.Value : SiteId);
            log.LogVariableState("@locationId", LocationId);
            log.LogVariableState("@productId", ProductId);
            log.LogVariableState("@quantity", Quantity);
            log.LogVariableState("@User", User);

            log.LogMethodExit(null);
        }
    }
}
