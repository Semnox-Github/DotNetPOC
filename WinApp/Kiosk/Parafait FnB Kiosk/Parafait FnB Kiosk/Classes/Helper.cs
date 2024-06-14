/********************************************************************************************
* Project Name - Parafait_Kiosk -Helper.cs
* Description  - Helper 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_FnB_Kiosk
{
    public static class Helper
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        public static DataTable getProductDetails(int productId)
        {
            log.LogMethodEntry(productId);
            DataTable dataTable = Common.utils.executeDataTable(@"select p.product_id, 
                                            isnull((select translation 
                                                    from ObjectTranslations
                                                    where ElementGuid = p.Guid
                                                    and Element = 'PRODUCT_NAME'
                                                    and LanguageId = @lang), p.product_name) as product_name, 
                                            isnull((select translation 
                                                    from ObjectTranslations
                                                    where ElementGuid = p.Guid
                                                    and Element = 'DESCRIPTION'
                                                    and LanguageId = @lang), p.description) as description, pt.product_type,
                                       case when TaxInclusivePrice = 'Y' then p.price else p.price * (1 + isnull(t.tax_percentage, 0)/100.0) end as Price,
                                       p.QuantityPrompt, isnull(p.CardCount, 0) CardCount, isnull(Credits, 0) + isnull(bonus, 0) Credits
                                from products p left outer join tax t
                                    on p.tax_id = t.tax_id,
                                    product_type pt
                                where pt.product_type_id = p.product_type_id
                                and product_id = @productId",
                    new SqlParameter("@productId", productId),
                    new SqlParameter("@lang", Common.utils.ParafaitEnv.LanguageId));
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        public static DataTable getUpsellProducts(object ProductId)//Suggestive sale changes
        {
            log.LogMethodEntry(ProductId);
            DataTable dataTable = getUpsellSuggestiveProducts(ProductId, true);
            log.LogMethodExit(dataTable);
            return dataTable;
        }//Suggestive sale changes

        public static DataTable getSuggestiveSellProducts(object ProductId)//Suggestive sale changes
        {
            log.LogMethodEntry(ProductId);
            DataTable dataTable = getUpsellSuggestiveProducts(ProductId, false);
            log.LogMethodExit(dataTable);
            return dataTable;
        }//Suggestive sale changes

        private static DataTable getUpsellSuggestiveProducts(object ProductId, bool UpsellSuggestive)
        {//Suggestive sale changes
            log.LogMethodEntry(ProductId, UpsellSuggestive);
            DataTable dataTable = Common.utils.executeDataTable(@"select top 1 p.product_id, 
                                                            isnull((select translation 
                                                                    from ObjectTranslations
                                                                    where ElementGuid = p.Guid
                                                                    and Element = 'PRODUCT_NAME'
                                                                    and LanguageId = @lang), p.product_name) as product_name,
                                                        uso.OfferMessage, isnull(case when TaxInclusivePrice = 'Y' then p.price else p.price * (1 + isnull(t.tax_percentage, 0)/100.0) end, 0) as Price,
                                                        isnull((select translation 
                                                                    from ObjectTranslations
                                                                    where ElementGuid = p.Guid
                                                                    and Element = 'DESCRIPTION'
                                                                    and LanguageId = @lang), p.description) description,
                                                        isnull((select translation 
                                                                    from ObjectTranslations
                                                                    where ElementGuid = p.Guid
                                                                    and Element = 'PRODUCT_IMAGE'
                                                                    and LanguageId = @lang), p.ImageFileName) as ImageFileName
                                                    from products p left outer join tax t
                                                            on p.tax_id = t.tax_id, UpsellOffers uso
                                                    where uso.productId = @productId
                                                    and (uso.OfferProductId = p.product_id 
                                                        or exists(select * from SaleGroupProductMap sgpm join SalesOfferGroup sog on sog.SaleGroupId = sgpm.SaleGroupId
                                                    where   IsUpsell = @isUpsell and uso.SaleGroupId=sog.SaleGroupId
                                                and p.product_id = sgpm.ProductId))
                                                    and p.active_flag = 'Y'
                                                    and uso.ActiveFlag = 1
                                                    and isnull(uso.EffectiveDate, getdate()) <= getdate()
                                                    order by uso.effectiveDate desc",
                                                        new SqlParameter("@lang", Common.utils.ParafaitEnv.LanguageId),
                                                        new SqlParameter("@isUpsell", UpsellSuggestive),
                                                        new SqlParameter("@productId", ProductId));
            log.LogMethodExit(dataTable);
            return dataTable;
        }//Suggestive sale changes


        public static DataTable getSuggestiveProducts(List<int> productIdList)
        {//Suggestive sale changes
            log.LogMethodEntry(productIdList);
            string productIds = "(-1";
            foreach (int id in productIdList)
            {
                productIds += ", " + id;
            }

            productIds += ")";

            int dayofweek = -1;
            switch (ServerDateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday: dayofweek = 0; break;
                case DayOfWeek.Monday: dayofweek = 1; break;
                case DayOfWeek.Tuesday: dayofweek = 2; break;
                case DayOfWeek.Wednesday: dayofweek = 3; break;
                case DayOfWeek.Thursday: dayofweek = 4; break;
                case DayOfWeek.Friday: dayofweek = 5; break;
                case DayOfWeek.Saturday: dayofweek = 6; break;
                default: break;
            }

            DataTable dt = Common.utils.executeDataTable(@"select product_id, product_name,
                                                            Price, description, ImageFileName, a.SequenceId from (
                                                                        select distinct p.product_id,
                                                                        isnull((select translation
                                                                                from ObjectTranslations
                                                                                where ElementGuid = p.Guid
                                                                                and Element = 'PRODUCT_NAME'
                                                                               and LanguageId = @lang), p.product_name) as product_name,
                                                                          isnull(case when TaxInclusivePrice = 'Y' then p.price else p.price * (1 + isnull(t.tax_percentage, 0)/100.0) end, 0) as Price,
                                                                    isnull((select translation
                                                                                from ObjectTranslations
                                                                                where ElementGuid = p.Guid
                                                                                and Element = 'DESCRIPTION'
                                                                                and LanguageId = @lang), p.description) description,
                                                                    isnull((select translation
                                                                                from ObjectTranslations
                                                                                where ElementGuid = p.Guid
                                                                                and Element = 'PRODUCT_IMAGE'
                                                                                and LanguageId = @lang), p.ImageFileName) as ImageFileName, sugestiveoffers.SequenceId
                                                                from products p left outer join tax t
                                                                        on p.tax_id = t.tax_id, 
                                                                    (SELECT ProductId, SequenceId, SaleGroupId 
                                                                       FROM(SELECT sgpm.ProductId, sgpm.SequenceId, sog.SaleGroupId, DENSE_RANK() OVER (Partition by sgpm.ProductId  ORDER BY sgpm.SequenceId) as ranking
                                                                        FROM (select distinct SaleGroupId 
                                                                                from UpsellOffers USO 
                                                                                where uso.ActiveFlag = 1
                                                                                and isnull(uso.EffectiveDate, getdate()) <= getdate()
                                                                                and USO.ProductId in" + productIds + @") uso,
                                                                         SalesOfferGroup SoG,
                                                                         SaleGroupProductMap SGPM
                                                                    WHERE uso.SaleGroupId = SOG.SaleGroupId
                                                                        AND SGPM.SaleGroupId = SOG.SaleGroupId
                                                                        AND SOG.IsUpsell = 0
                                                                        AND SOG.IsActive = 1
                                                                        AND SGPM.IsActive = 1
                                                                        AND sgpm.ProductId NOT in" + productIds + @")offers 
                                                                        where  ranking = 1 and (not exists (select 1
                                                                                    from ProductCalendar pc
                                                                                    where pc.product_id = offers.ProductId)
                                                                        or exists (select 1 from 
                                                                                    (select top 1 date, day, -- select in the order of specific date, day of month, weekday, every day. if there are multiple slots on same day, take the one which is in current hour
                                                                                            case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort, 
                                                                                            FromTime, ToTime, ShowHide  
                                                                                    from ProductCalendar pc 
                                                                                        where pc.product_id = offers.ProductId 
                                                                                        and (Date = @today -- specific day
                                                                                        or Day = @DayNumber -- day number 1001 - 1031
                                                                                        or Day = @weekDay -- week day 0-6
                                                                                        or Day = -1) -- everyday
                                                                                        order by 1 desc, 2 desc, 3) inView 
                                                                                        where (ShowHide = 'Y' 
                                                                                            and (@nowHour >= isnull(FromTime, 0) and @nowHour <= case isnull(ToTime, 0) when 0 then 24 else ToTime end))
                                                                                        or (ShowHide = 'N'
                                                                                            and (@nowHour < isnull(FromTime, 0) or @nowHour > case isnull(ToTime, 0) when 0 then 24 else ToTime end)))))sugestiveoffers
                                                                where p.product_id = sugestiveoffers.productId 
                                                                   and p.Modifier = 'N'
                                                                   and p.active_flag = 'Y') as a 
                                                              order by SequenceId",
                                                        new SqlParameter("@lang", Common.utils.ParafaitEnv.LanguageId),
                                                        new SqlParameter("@today", ServerDateTime.Now),
                                                        new SqlParameter("@nowHour", ServerDateTime.Now.Hour + ServerDateTime.Now.Minute / 100.0),
                                                        new SqlParameter("@DayNumber", ServerDateTime.Now.Day + 1000), // day of month stored as 1000 + day of month
                                                        new SqlParameter("@weekDay", dayofweek));

            if (dt.Rows.Count > 0)
            {
                log.LogMethodExit(dt);
                return dt;
            }
            else
            {
                DataTable dataTable = Common.utils.executeDataTable(@"select product_id, product_name,
                                                            Price, description, ImageFileName, a.SequenceId from (
                                                                        select distinct p.product_id,
                                                                        isnull((select translation
                                                                                from ObjectTranslations
                                                                                where ElementGuid = p.Guid
                                                                                and Element = 'PRODUCT_NAME'
                                                                               and LanguageId = @lang), p.product_name) as product_name,
                                                                          isnull(case when TaxInclusivePrice = 'Y' then p.price else p.price * (1 + isnull(t.tax_percentage, 0)/100.0) end, 0) as Price,
                                                                    isnull((select translation
                                                                                from ObjectTranslations
                                                                                where ElementGuid = p.Guid
                                                                                and Element = 'DESCRIPTION'
                                                                                and LanguageId = @lang), p.description) description,
                                                                    isnull((select translation
                                                                                from ObjectTranslations
                                                                                where ElementGuid = p.Guid
                                                                                and Element = 'PRODUCT_IMAGE'
                                                                                and LanguageId = @lang), p.ImageFileName) as ImageFileName, sugestiveoffers.SequenceId
                                                                from products p left outer join tax t
                                                                        on p.tax_id = t.tax_id,                                                                     
                                                                    (SELECT ProductId, SequenceId, SaleGroupId 
                                                                       FROM(SELECT sgpm.ProductId, sgpm.SequenceId, sog.SaleGroupId, DENSE_RANK() OVER (Partition by sgpm.ProductId  ORDER BY sgpm.SequenceId) as ranking
                                                                        FROM (select distinct SaleGroupId 
                                                                                from UpsellOffers USO 
                                                                                where uso.ActiveFlag = 1
                                                                                and isnull(uso.EffectiveDate, getdate()) <= getdate()) uso,
                                                                         SalesOfferGroup SoG,
                                                                         SaleGroupProductMap SGPM
                                                                    WHERE uso.SaleGroupId = SOG.SaleGroupId
                                                                        AND SGPM.SaleGroupId = SOG.SaleGroupId
                                                                        AND SOG.IsUpsell = 0
                                                                        AND SOG.IsActive = 1
                                                                        AND SGPM.IsActive = 1
                                                                        AND sgpm.ProductId NOT in" + productIds + @") offers 
                                                                        where ranking = 1 and  (not exists (select 1
                                                                                    from ProductCalendar pc
                                                                                    where pc.product_id = offers.ProductId)
                                                                        or exists (select 1 from 
                                                                                    (select top 1 date, day, -- select in the order of specific date, day of month, weekday, every day. if there are multiple slots on same day, take the one which is in current hour
                                                                                            case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort, 
                                                                                            FromTime, ToTime, ShowHide  
                                                                                    from ProductCalendar pc 
                                                                                        where pc.product_id = offers.ProductId 
                                                                                        and (Date = @today -- specific day
                                                                                        or Day = @DayNumber -- day number 1001 - 1031
                                                                                        or Day = @weekDay -- week day 0-6
                                                                                        or Day = -1) -- everyday
                                                                                        order by 1 desc, 2 desc, 3) inView 
                                                                                        where (ShowHide = 'Y' 
                                                                                            and (@nowHour >= isnull(FromTime, 0) and @nowHour <= case isnull(ToTime, 0) when 0 then 24 else ToTime end))
                                                                                        or (ShowHide = 'N'
                                                                                            and (@nowHour < isnull(FromTime, 0) or @nowHour > case isnull(ToTime, 0) when 0 then 24 else ToTime end)))))sugestiveoffers
                                                                where p.product_id = sugestiveoffers.productId 
                                                                   and p.Modifier = 'N'
                                                                   and p.active_flag = 'Y') as a 
                                                              order by SequenceId",
                                                        new SqlParameter("@lang", Common.utils.ParafaitEnv.LanguageId),
                                                        new SqlParameter("@today", ServerDateTime.Now),
                                                        new SqlParameter("@nowHour", ServerDateTime.Now.Hour + ServerDateTime.Now.Minute / 100.0),
                                                        new SqlParameter("@DayNumber", ServerDateTime.Now.Day + 1000), // day of month stored as 1000 + day of month
                                                        new SqlParameter("@weekDay", dayofweek));
                log.LogMethodExit(dataTable);
                return dataTable;
            }

        } //Suggestive sale changes

        
        public static Image GetProductImage(string ImageFileName)
        {
            log.LogMethodEntry(ImageFileName);
            if (!string.IsNullOrEmpty(ImageFileName))
            {
                try
                {
                    string imageFolder = Common.utils.getParafaitDefaults("IMAGE_DIRECTORY");
                    object o = Common.utils.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                            new SqlParameter("@FileName", imageFolder + "\\" + ImageFileName));
                    Image image = Common.utils.ConvertToImage(o);
                    log.LogMethodExit(image);
                    return image;
                }
                catch(Exception ex)
                {
                    log.Error("Error occurred while executing GetProductImage()" + ex.Message);
                    log.LogMethodExit();
                    return null;
                }
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        internal static RemotingClient CardRoamingRemotingClient = null;
        public static bool refreshCardFromHQ(ref Card CurrentCard, ref string message)
        {
            log.LogMethodEntry(CurrentCard, message);
            try
            {
                if (CardRoamingRemotingClient == null)
                {
                    if (Common.utils.ParafaitEnv.ALLOW_ROAMING_CARDS == "Y" && Common.utils.ParafaitEnv.ENABLE_ON_DEMAND_ROAMING == "Y")
                    {
                        try
                        {
                            CardRoamingRemotingClient = new RemotingClient();
                            Common.logToFile("Remoting client initialized");
                        }
                        catch (Exception ex)
                        {
                            message = ex.Message;
                            Common.logException(ex);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
                CardUtils cardUtils = new CardUtils(Common.utils);
                bool returnValue = cardUtils.getCardFromHQ(CardRoamingRemotingClient, ref CurrentCard, ref message);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                if (ex.Message.ToLower().Contains("fault"))
                {
                    message = Common.utils.MessageUtils.getMessage(216);
                    try
                    {
                        CardRoamingRemotingClient = new RemotingClient();
                    }
                    catch (Exception exe)
                    {
                        Common.logException(exe);
                        message = Common.utils.MessageUtils.getMessage(217);
                    }
                }
                else
                    message = "On-Demand Roaming: " + ex.Message;
                log.LogMethodExit(false);
                return false;
            }
        }


        public class CreditCardPaymentModeDetails
        {
            public int PaymentModeId;
            public string PaymentMode;
            public string Gateway;
            public double SurchargePercentage;

            public object GatewayObject;
        }

        static CreditCardPaymentModeDetails ccPaymentModeDetails = null;

        public static CreditCardPaymentModeDetails getCreditCardDetails()
        {
            log.LogMethodEntry();
            if (ccPaymentModeDetails != null)
            {
                log.LogMethodExit(ccPaymentModeDetails);
                return ccPaymentModeDetails;
            }

            string pmntMode = Common.utils.getParafaitDefaults("KIOSK_CREDITCARD_PAYMENT_MODE");
            int paymentModeId;
            if (int.TryParse(pmntMode, out paymentModeId))
            {

                DataTable dtCreditCards = Common.utils.executeDataTable(
                                                        @"select PaymentModeId, PaymentMode, isnull(CreditCardSurchargePercentage, 0) Surcharge, lv.LookupValue Gateway
                                                           from PaymentModes p
                                                            left outer join LookupView lv
                                                            on lv.LookupValueId = p.Gateway
                                                            and lv.LookupName = 'PAYMENT_GATEWAY'
                                                           where PaymentModeId = @paymentModeId",
                                                               new SqlParameter("@paymentModeId", paymentModeId));
                if (dtCreditCards.Rows.Count > 0)
                {
                    ccPaymentModeDetails = new CreditCardPaymentModeDetails();
                    ccPaymentModeDetails.PaymentModeId = paymentModeId;
                    ccPaymentModeDetails.PaymentMode = dtCreditCards.Rows[0]["PaymentMode"].ToString();
                    ccPaymentModeDetails.SurchargePercentage = Convert.ToDouble(dtCreditCards.Rows[0]["Surcharge"]);
                    ccPaymentModeDetails.Gateway = dtCreditCards.Rows[0]["Gateway"].ToString();

                    if (ccPaymentModeDetails.Gateway.Equals(PaymentGateways.ElementExpress.ToString()))
                    {
                        ccPaymentModeDetails.GatewayObject = new ElementPSAdaper(Common.utils);
                    }
                    else if (ccPaymentModeDetails.Gateway.Equals(PaymentGateways.FirstData.ToString()))//Modified on 2015-09-22 for selection of debit or credit
                    {
                        ccPaymentModeDetails.GatewayObject = new FirstDataAdapter(Common.utils, null);
                        FirstDataAdapter.IsUnattended = true;
                    }

                    log.LogMethodExit(ccPaymentModeDetails);
                    return ccPaymentModeDetails;
                }
                else
                {
                    throw new ApplicationException("Credit card gateway not defined");
                }
            }
            else
            {
                throw new ApplicationException("Invalid KIOSK_CREDITCARD_PAYMENT_MODE");
            }
        }

        public static object GetProductExternalSystemReference(int ProductId)
        {
            log.LogMethodEntry(ProductId);
            object ret = Common.utils.executeScalar(@"select CustomDataNumber 
                                                from CustomDataView v, 
                                                    products p
                                                where p.CustomDataSetId = v.CustomDataSetId
                                                and v.Name = 'External System Identifier'
                                                and p.product_id = @productId",
                                                new SqlParameter("@productId", ProductId));
            log.LogMethodExit(ret);
            return ret;
        }

        public static double GetProductPrice(int ProductId)
        {
            log.LogMethodEntry(ProductId);
            object o = Common.utils.executeScalar(@"select isnull(Price, 0) 
                                                from products p
                                                where p.product_id = @productId",
                                                new SqlParameter("@productId", ProductId));
            if (o != null)
            {
                log.LogMethodExit(o);
                return Convert.ToDouble(o);
            }
            else
            {
                log.LogMethodExit();
                return 0;
            }
        }

        internal static bool CheckProductAvailability(object productId)
        {
            log.LogMethodEntry(productId);
            int dayofweek = -1;
            switch (ServerDateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday: dayofweek = 0; break;
                case DayOfWeek.Monday: dayofweek = 1; break;
                case DayOfWeek.Tuesday: dayofweek = 2; break;
                case DayOfWeek.Wednesday: dayofweek = 3; break;
                case DayOfWeek.Thursday: dayofweek = 4; break;
                case DayOfWeek.Friday: dayofweek = 5; break;
                case DayOfWeek.Saturday: dayofweek = 6; break;
                default: break;
            }

            object o = Common.utils.executeScalar(
                @"select product_id 
                    from products p
                    where product_id = @productId
                    and (not exists (select 1
                                    from ProductCalendar pc
                                    where pc.product_id = p.product_id)
                        or exists (select 1 from 
                                    (select top 1 date, day, -- select in the order of specific date, day of month, weekday, every day. if there are multiple slots on same day, take the one which is in current hour
                                            case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort, 
                                            FromTime, ToTime, ShowHide  
                                    from ProductCalendar pc 
                                        where pc.product_id = p.product_id 
                                        and (Date = @today -- specific day
                                        or Day = @DayNumber -- day number 1001 - 1031
                                        or Day = @weekDay -- week day 0-6
                                        or Day = -1) -- everyday
                                        order by 1 desc, 2 desc, 3) inView 
                                        where (ShowHide = 'Y' 
                                            and (@nowHour >= isnull(FromTime, 0) and @nowHour <= case isnull(ToTime, 0) when 0 then 24 else ToTime end))
                                        or (ShowHide = 'N'
                                            and (@nowHour < isnull(FromTime, 0) or @nowHour > case isnull(ToTime, 0) when 0 then 24 else ToTime end))))",
            new SqlParameter("@productId", productId),
            new SqlParameter("@today", ServerDateTime.Now),
            new SqlParameter("@nowHour", ServerDateTime.Now.Hour + ServerDateTime.Now.Minute / 100.0),
            new SqlParameter("@DayNumber", ServerDateTime.Now.Day + 1000), // day of month stored as 1000 + day of month
            new SqlParameter("@weekDay", dayofweek));

            if (o == null)
            {
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
        }

        internal static bool ShowTent()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
            searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "FNBKIOSK_SETUP"));
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "FNB_SHOW_TENTSELECTION"));
            List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
            string showTentSelection = "";
            if (lookupValuesListDTO != null)
            {
                showTentSelection = lookupValuesListDTO[0].Description;
            }
            else
                showTentSelection = "N";
            if (showTentSelection.Trim().ToUpper() == "Y")
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        internal static bool GetEnableBillCollectorFlag()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
            searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "FNBKIOSK_SETUP"));
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "FNB_ENABLE_BILLCOLLECTOR"));
            List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
            string enableBillCollector = "";
            if (lookupValuesListDTO != null)
            {
                enableBillCollector = lookupValuesListDTO[0].Description;
            }
            else
                enableBillCollector = "N";
            if (enableBillCollector.Trim().ToUpper() == "Y")
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        internal static bool GetShowPartySizeFlag()
        {
            log.LogMethodEntry();
            string showPartySize = "";
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
            searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "FNBKIOSK_SETUP"));
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "FNB_SHOW_PARTYSIZE"));
            List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);

            if (lookupValuesListDTO != null)
            {
                showPartySize = lookupValuesListDTO[0].Description;
            }
            else
                showPartySize = "N";

            if (showPartySize.Trim().ToUpper() == "Y")
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        public static void PrintReceipt(bool isAbort, KioskStatic.acceptance ac)
        {
            log.LogMethodEntry();
            // if (KioskStatic.isUSBPrinter)
            PrintReceiptUSB(isAbort, ac);
            log.LogMethodExit();
        }

        static void PrintReceiptUSB(bool isAbort, KioskStatic.acceptance ac)
        {
            log.LogMethodEntry(isAbort, ac);
            Common.logToFile("print_receiptUSB; isAbort = " + isAbort.ToString());
            if (isAbort)
            {
                System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                List<string> printString = new List<string>();
                KioskStatic.receipt_format rc = new KioskStatic.receipt_format();
                rc.head = Common.utils.ParafaitEnv.SiteName;
                rc.a1 = "Date: @Date";
                rc.a21 = "Kiosk: @POS";

                printString.Add(rc.head);
                if (!string.IsNullOrEmpty(rc.a1))
                {
                    printString.Add(rc.a1.Replace("@Date", DateTime.Now.ToString("ddd, dd-MMM-yyyy h:mm tt")));
                }

                if (!string.IsNullOrEmpty(rc.a21))
                {
                    printString.Add(rc.a21.Replace("@POS", Common.utils.ParafaitEnv.POSMachine));
                }

                printString.Add(Environment.NewLine);
                printString.Add("*******************");
                printString.Add(Common.utils.MessageUtils.getMessage(439)); //"TRANSACTION ABORTED";
                printString.Add("*******************");
                printString.Add(Environment.NewLine);

                printString.Add(Common.utils.MessageUtils.getMessage(440, ac.totalValue.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                printString.Add(Environment.NewLine);

                decimal cashAmount = ac.totalValue;
                decimal ccAmount = 0;
                if (ac.totalValue > 0)
                {
                    if (UserTransaction.OrderDetails.transactionPaymentsDTO != null && UserTransaction.OrderDetails.transactionPaymentsDTO.GatewayPaymentProcessed)
                        ccAmount = (decimal)UserTransaction.OrderDetails.transactionPaymentsDTO.Amount;
                    cashAmount -= ccAmount;
                }
                printString.Add("Cash: " + cashAmount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                printString.Add("Credit Card: " + ccAmount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                printString.Add(Environment.NewLine);

                printString.Add(Common.utils.MessageUtils.getMessage(441));

                pd.PrintPage += (sender, e) => PrintUSB(e, printString);
                pd.Print();
            }
            log.LogMethodExit();
        }

        static void PrintUSB(System.Drawing.Printing.PrintPageEventArgs e, List<string> input)
        {
            log.LogMethodEntry(input);
            Font f = new Font("Verdana", 10f);
            float height = e.Graphics.MeasureString("ABCD", f).Height;
            float locY = 10;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            foreach (string s in input)
            {
                e.Graphics.DrawString(s, f, Brushes.Black, new Rectangle(0, (int)locY, e.PageBounds.Width, (int)height), sf);
                locY += height;
            }
            log.LogMethodExit();
        }

    }
}
