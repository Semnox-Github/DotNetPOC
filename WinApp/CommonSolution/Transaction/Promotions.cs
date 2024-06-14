using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.Transaction
{
    public static class Promotions
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static int getProductPromotionPrice(CustomerDTO inCustomerDTO, int ProductId, object CategoryId, string TaxInclusive, double taxPercentage, ref double Price, Utilities Utilities)
        {
            log.LogMethodEntry(inCustomerDTO, ProductId, CategoryId, TaxInclusive, taxPercentage, Price, Utilities);

            DateTime BookDate = DateTime.MinValue ;

            int returnValue = getProductPromotionPrice(inCustomerDTO, ProductId, CategoryId, TaxInclusive, taxPercentage, ref Price, Utilities, BookDate);

            log.LogVariableState("Price ", Price);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        public static int getProductPromotionPrice(CustomerDTO inCustomerDTO, int ProductId, object CategoryId, string TaxInclusive, double taxPercentage, ref double Price, Utilities Utilities, DateTime BookDate)
        {
            log.LogMethodEntry(inCustomerDTO, ProductId, CategoryId, TaxInclusive, taxPercentage,  Price, Utilities, BookDate);
            // check for promotion
           
            DataTable dtPromo;

            if (BookDate == DateTime.MinValue)
            {

                //string CommandText = @"select v.promotion_id, absolute_credits, discount_on_credits, DiscountAmount,
                //                    case when @cardTypeId < 0 then r.cardTypeId else case when r.cardTypeId = @cardTypeId then -2 else isnull(r.cardTypeId, -1) end end sort1, 1 sort2 
                //                from PromotionView v
                //                left outer join PromotionRule r
                //                    on r.promotion_id = v.promotion_id
                //            where product_id = @product_id
                //            union all
                //            select v.promotion_id, absolute_credits, discount_on_credits, DiscountAmount,
                //                    case when @cardTypeId < 0 then r.cardTypeId else case when r.cardTypeId = @cardTypeId then -2 else isnull(r.cardTypeId, -1) end end, 2 
                //                from PromotionView v
                //                left outer join PromotionRule r
                //                    on r.promotion_id = v.promotion_id
                //            where CategoryId in (select categoryId from getCategoryList(@categoryId))
                //            union all
                //            select v.promotion_id, absolute_credits, discount_on_credits, DiscountAmount,
                //                    case when @cardTypeId < 0 then r.cardTypeId else case when r.cardTypeId = @cardTypeId then -2 else isnull(r.cardTypeId, -1) end end, 3 
                //                from PromotionView v
                //                left outer join PromotionRule r
                //                    on r.promotion_id = v.promotion_id
                //            where product_id is null 
                //            and CategoryId is null
                //            and PromotionType = 'P'
                //        order by sort1, sort2";

                //dtPromo = Utilities.executeDataTable(CommandText,
                //                                      new SqlParameter("@CategoryId", CategoryId),
                //                                      new SqlParameter("@product_id", ProductId),
                //                                      new SqlParameter("@cardTypeId", (inCard == null ? -1 : inCard.CardTypeId)));

                //log.LogVariableState("@CategoryId", CategoryId);
                //log.LogVariableState("@product_id", ProductId);
                //log.LogVariableState("@cardTypeId", (inCard == null ? -1 : inCard.CardTypeId));
                string CommandText = @"select v.promotion_id, absolute_credits, discount_on_credits, DiscountAmount,
                                    case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end sort1, 1 sort2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                            where product_id = @product_id
                            union all
                            select v.promotion_id, absolute_credits, discount_on_credits, DiscountAmount,
                                    case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                            where CategoryId in (select categoryId from getCategoryList(@categoryId))
                            union all
                            select v.promotion_id, absolute_credits, discount_on_credits, DiscountAmount,
                                    case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 3 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                            where product_id is null 
                            and CategoryId is null
                            and PromotionType = 'P'
                        order by sort1, sort2";

                dtPromo = Utilities.executeDataTable(CommandText,
                                                      new SqlParameter("@CategoryId", CategoryId),
                                                      new SqlParameter("@product_id", ProductId),
                                                      new SqlParameter("@membershipId", (inCustomerDTO == null ? -1 : inCustomerDTO.MembershipId)));

                log.LogVariableState("@CategoryId", CategoryId);
                log.LogVariableState("@product_id", ProductId);
                log.LogVariableState("@membershipId", (inCustomerDTO == null ? -1 : inCustomerDTO.MembershipId));

            }
            else
            {

                //string CommandText = "[getPromotionData] @BookDate , @cardTypeId , @product_id , @categoryId ";
                string CommandText = "[getPromotionData] @BookDate , @membershipId , @product_id , @categoryId ";

                dtPromo = Utilities.executeDataTable(CommandText,
                                                            new SqlParameter("@CategoryId", CategoryId),
                                                            new SqlParameter("@BookDate", BookDate),
                                                            new SqlParameter("@product_id", ProductId),
                                                            new SqlParameter("@membershipId", (inCustomerDTO == null ? -1 : inCustomerDTO.MembershipId)));

                log.LogVariableState("@CategoryId", CategoryId);
                log.LogVariableState("@BookDate", BookDate);
                log.LogVariableState("@product_id", ProductId);
                log.LogVariableState("@membershipId", (inCustomerDTO == null ? -1 : inCustomerDTO.MembershipId));
            }
   
            int promotionId = -1;
            if (dtPromo.Rows.Count > 0)
            {
                //if (Utilities.executeScalar(@"select top 1 1 
                //                                where not exists (select 1 
                //                                                from PromotionRule 
                //                                                where promotion_id = @promoId) 
                //                                or (exists (select 1 
                //                                                from PromotionRule pr, cards c
                //                                                where promotion_id = @promoId
                //                                                and c.card_id = @cardId
                //                                                and c.cardTypeId = pr.CardTypeId))",
                //                            new SqlParameter("@promoId", dtPromo.Rows[0]["promotion_id"]),
                //                            new SqlParameter("@cardId", (inCard == null ? -1 : inCard.card_id))) == null)
                if (Utilities.executeScalar(@"select top 1 1 
                                                where not exists (select 1 
                                                                from PromotionRule 
                                                                where promotion_id = @promoId) 
                                                or (exists (select 1 
                                                                from PromotionRule pr, 
                                                                      customers cu  
                                                                where promotion_id = @promoId
                                                                and cu.customer_id = @customerId
                                                                and cu.MembershipId = pr.MembershipId))",
                                           new SqlParameter("@promoId", dtPromo.Rows[0]["promotion_id"]),
                                           new SqlParameter("@customerId", (inCustomerDTO == null ? -1 : inCustomerDTO.Id))) == null)
                {
                    log.LogVariableState("@promoId", dtPromo.Rows[0]["promotion_id"]);
                    log.LogVariableState("@customerId", (inCustomerDTO == null ? -1 : inCustomerDTO.Id));

                    log.LogMethodExit(-1);
                    return -1;
                }
                    

                if (dtPromo.Rows[0]["absolute_credits"] != DBNull.Value)
                {
                    Price = Convert.ToDouble(dtPromo.Rows[0]["absolute_credits"]);
                    try
                    {
                        if (TaxInclusive == "Y")
                        {
                            Price = Price / (1.0 + taxPercentage / 100.0);
                        }
                    }
                    catch (Exception e)
                    {
                        log.Error("Error occured while claculating price", e);
                        log.LogVariableState("Price", Price);
                    }

                    promotionId = Convert.ToInt32(dtPromo.Rows[0]["promotion_id"]);
                    log.LogVariableState("promotionId ",promotionId);
                }
                else if (dtPromo.Rows[0]["DiscountAmount"] != DBNull.Value) //Added on 7-Nov-2016 for selecting discount amount
                {
                    Price = Price - Convert.ToDouble(dtPromo.Rows[0]["DiscountAmount"]);
                    //try
                    //{
                    //    if (TaxInclusive == "Y")
                    //    {
                    //        Price = Price / (1.0 + taxPercentage / 100.0);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    log.Error("Error occured while claculating price", ex);
                    //    log.LogVariableState("Price", Price);
                    //}
                    promotionId = Convert.ToInt32(dtPromo.Rows[0]["promotion_id"]);
                    log.LogVariableState("promotionId ", promotionId);
                }//end
                else if (dtPromo.Rows[0]["discount_on_credits"] != DBNull.Value)
                {
                    Price = Price - Price * Convert.ToDouble(dtPromo.Rows[0]["discount_on_credits"]) / 100;
                    promotionId = Convert.ToInt32(dtPromo.Rows[0]["promotion_id"]);
                }

                Price = Math.Max(Price, 0);
            }

            log.LogVariableState("Price", Price);
            log.LogMethodExit(promotionId);
            return promotionId;
        }

        public static bool CheckProductPromotionExitsForDay(CustomerDTO inCustomerDTO, int ProductId, object CategoryId, Utilities Utilities, DateTime BookDate)
        {
            log.LogMethodEntry(inCustomerDTO, ProductId, CategoryId, Utilities, BookDate);
            // check for promotion

            DataTable dtPromo;

            if (BookDate == DateTime.MinValue)
            {
                return false;
            }
            else
            {

                string CommandText = "[CheckPromotionDataForDay] @BookDate , @membershipId , @product_id , @categoryId ";

                dtPromo = Utilities.executeDataTable(CommandText,
                                                            new SqlParameter("@CategoryId", CategoryId),
                                                            new SqlParameter("@BookDate", BookDate),
                                                            new SqlParameter("@product_id", ProductId),
                                                            new SqlParameter("@membershipId", (inCustomerDTO == null ? -1 : inCustomerDTO.MembershipId)));

                log.LogVariableState("@CategoryId", CategoryId);
                log.LogVariableState("@BookDate", BookDate);
                log.LogVariableState("@product_id", ProductId);
                log.LogVariableState("@membershipId", (inCustomerDTO == null ? -1 : inCustomerDTO.MembershipId));
            }

            bool promotionExists = false;
            if (dtPromo.Rows.Count > 0)
            {
                if (Utilities.executeScalar(@"select top 1 1 
                                                where not exists (select 1 
                                                                from PromotionRule 
                                                                where promotion_id = @promoId) 
                                                or (exists (select 1 
                                                                from PromotionRule pr, 
                                                                      customers cu  
                                                                where promotion_id = @promoId
                                                                and cu.customer_id = @customerId
                                                                and cu.MembershipId = pr.MembershipId))",
                                           new SqlParameter("@promoId", dtPromo.Rows[0]["promotion_id"]),
                                           new SqlParameter("@customerId", (inCustomerDTO == null ? -1 : inCustomerDTO.Id))) == null)
                {
                    log.LogVariableState("@promoId", dtPromo.Rows[0]["promotion_id"]);
                    log.LogVariableState("@customerId", (inCustomerDTO == null ? -1 : inCustomerDTO.Id));

                    log.LogMethodExit(-1);
                    return false;
                }
                promotionExists = true;
            }

            log.LogVariableState("promotionExists:", promotionExists);
            return promotionExists;
        }
    }
}
