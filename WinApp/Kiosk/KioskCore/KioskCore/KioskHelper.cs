/********************************************************************************************
 * Project Name - Kiosk Core
 * Description  - KioskHelper.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      3-Sep-2019   Deeksha        Added logger methods.
 *2.70.2       3-Dec-2019   Guru S A       Waiver phase 2 chanages
 *2.70.3      08-Aug-2020   Guru S A       Fix kiosk crash due to large data in card activity/game play 
 *2.140.0     18-Oct-2021   Sathyavathi    Check-In Check-out Enhancement
 *2.150.0.0   18-Oct-2022   Sathyavathi    Check-In feature Phase-2
 *2.150.0.0   10-Oct-2022   Sathyavathi    Mask card umber
 *2.150.0.0   02-Dec-2022   Sathyavathi    Check-In feature Phase-2 Additional features
 *2.150.1     22-Feb-2023   Guru S A       Kiosk Cart Enhancements
 *2.155.0     16-Jun-2023   Sathyavathi    Attraction Sale in Kiosk 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Product;
using Semnox.Parafait.Category;
using Semnox.Parafait.User;
using System.Collections.Concurrent;
using System.Text;
using Semnox.Parafait.POS;
using Semnox.Parafait.Device.PaymentGateway;
using System.Drawing.Imaging;
using System.IO;
using Semnox.Parafait.CommonUI;
using System.Globalization;
using System.Windows.Input;
using System.ComponentModel;
using System.Reflection;
using Semnox.Parafait.ViewContainer;
using System.Windows.Controls;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.logger;

namespace Semnox.Parafait.KioskCore
{
    public static class KioskHelper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string KIOSK_SETUP_LOOKUP = "KIOSK_SETUP";
        private static string DATE_TIME_FORMAT_FOR_UI = "DATE_TIME_FORMAT_FOR_UI";
        private static string DEFAULT_DATE_TIME_FOR_UI = "d-MMM-yyyy h:mm tt";


        public static DataTable getProducts(string displayGroup, string Function, string entitlementType)
        {
            log.LogMethodEntry(displayGroup, Function, entitlementType);
            DateTime serverTime = KioskStatic.Utilities.getServerTime();

            string ProductType;

            if (Function == "I")
            {
                if (KioskStatic.Utilities.getParafaitDefaults("ALLOW_VARIABLE_NEW_CARD_ISSUE").Equals("Y"))
                {
                    if (entitlementType == "Time")
                        ProductType = "('GAMETIME', 'VARIABLECARD')";
                    else
                        ProductType = "('NEW', 'CARDSALE', 'VARIABLECARD') ";
                }
                else
                {
                    if (entitlementType == "Time")
                        ProductType = "('GAMETIME')";
                    else
                        ProductType = "('NEW', 'CARDSALE') ";
                }
            }
            else if (Function == "C")
            {
                ProductType = "('COMBO', 'CHECK-IN')";
            }
            else if (Function == "F")
            {
                ProductType = "('MANUAL')";
            }
            else if (Function == "A")
            {
                ProductType = "('ATTRACTION', 'COMBO') ";
            }
            else
            {
                if (entitlementType == "Time")
                    ProductType = "('GAMETIME', 'VARIABLECARD')";
                else
                    ProductType = "('RECHARGE', 'CARDSALE', 'VARIABLECARD') ";
            }
            string CommandText = @"select p.product_id,  
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
                                          pt.product_type, isnull(case pdgf.displayGroup when '' then null else pdgf.displayGroup end, 'Others') display_group, 
                                          ButtonColor, (select valueChar from CustomDataView where CustomDataSetId = p.CustomDataSetId and Applicability = 'PRODUCT' and Name = 'KIOSK BEST DEAL PRODUCT') bestDeal, 
                                          isnull((select translation 
                                                    from ObjectTranslations
                                                   where ElementGuid = p.Guid
                                                     and Element = 'PRODUCT_IMAGE'
                                                     and LanguageId = @lang), p.ImageFileName) as ImageFileName 
                                      from products p left outer join tax t on p.tax_id = t.tax_id
                                                        join (select pdg.ProductId, pdgf.DisplayGroup
					                                                     from ProductsDisplayGroup PDG, ProductDisplayGroupFormat PDGF
					                                                     where pdg.DisplayGroupId = pdgf.id 
									                                     and not exists (select 1  
														                                 from POSProductExclusions e  
														                                 where e.POSMachineId = @POSMachine  
														                                 and e.ProductDisplayGroupFormatId = pdg.DisplayGroupId)) pdgf on pdgf.ProductId = p.product_id, product_type pt 
                                     where p.product_type_id = pt.product_type_id
                                       and p.active_flag = 'Y' 
                                       and (p.POSTypeId = @Counter or @Counter = -1 or p.POSTypeId is null) 
                                       --and (p.expiryDate >= @today or p.expiryDate is null)
                                       and (p.expiryDate > dateadd(HOUR, @nowHour, @today) or p.expiryDate is null)
                                       and (p.StartDate <= @today or p.StartDate is null)
                                       and not exists (select 1 -- Skip fund raiser/donation products
                                                        from lookupView lv, category ct
                                                       where lv.LookupName ='FUND_RAISER_AND_DONATIONS'
                                                         and ISNULL(lv.description,'A') = ct.Name
                                                         and ct.CategoryId = p.CategoryId)
                                       --and ISNULL(p.WaiverSetId, -1) = -1
                                       --and not exists (select 1 
                                       --                  from FacilityWaiver fw, CheckInFacility fac, WaiverSet ws, FacilityMapDetails fmd, FacilityMap fm,
	                                   --                       ProductsAllowedInFacility paif
                                       --                 where fac.FacilityId = fw.FacilityId
                                       --                   and fac.active_flag = 'Y'
                                       --                   and ISNULL(fw.EffectiveFrom, getdate()) >= getdate()
                                       --                   and ISNULL(fw.EffectiveTo, getdate()) <= getdate()
                                       --                   and fw.IsActive = 1
                                       --                   and fw.FacilityWaiverId = ws.WaiverSetId
                                       --                   and ws.IsActive = 1
                                       --                   and fac.FacilityId = fmd.FacilityId
                                       --                   and fmd.IsActive = 1
                                       --                   and fmd.FacilityMapId = fm.FacilityMapId
                                       --                   and fm.IsActive = 1
                                       --                   and paif.FacilityMapId = fmd.FacilityMapId
                                       --                   and paif.IsActive = 1
                                       --                   and paif.ProductsId = p.product_id)
                                       and not exists (select 1 
                                                         from comboproduct cp, 
                                                              products childp, 
                                                              product_type cpt
                                                        where cp.product_id = p.product_id
                                                          and cp.childProductId = childp.Product_id
                                                          and cpt.product_type_id = childp.product_type_id
                                                          and ((@functionType = 'A' and cpt.product_type in ('CHECK-IN'))
                                                               OR (@functionType = 'C' and cpt.product_type in ('ATTRACTION'))
                                                             )
                                                       )
                                       and case when pt.product_type = 'COMBO' then 
                                              (select count(1) 
                                                 from comboproduct cp,
                                                      products childp, 
                                                      product_type cpt
                                                where cp.product_id = p.product_id
                                                  and cp.isactive = 1
                                                  and cp.childProductId = childp.Product_id
                                                  and cpt.product_type_id = childp.product_type_id
                                                  and ((@functionType = 'A' and cpt.product_type in ('ATTRACTION'))
                                                        OR (@functionType = 'C' and cpt.product_type in ('CHECK-IN'))
                                                       )) 
                                              else 1 end > 0
                                       and (not exists (select 1 
                                                          from ProductCalendar pc
                                                         where pc.product_id = p.product_id) 
                                             or exists (select 1 from 
                                                        (select top 1 date, day, 
                                                                case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort, 
                                                                isnull(FromTime, @nowHour) FromTime, isnull(ToTime, @nowHour) ToTime, ShowHide -- // select in the order of specific date, day of month, weekday, every day. 
                                                             --//if there are multiple slots on same day, take the one which is in current hour
                                                           from ProductCalendar pc 
                                                          where pc.product_id = p.product_id
                                                            and (Date = @today -- // specific date
                                                                or Day = @DayNumber -- // 1001 to 1031
                                                                or Day = @weekDay -- // 0 to 6
                                                                or Day = -1) -- //everyday
                                                          order by 1 desc, 2 desc, 3) inView 
                                                         where (ShowHide = 'Y' 
                                                                and (@nowHour >= FromTime and @nowHour <= case ToTime when 0 then 24 else ToTime end)) 
                                                                 or (ShowHide = 'N' 
                                                                and (@nowHour < FromTime or @nowHour > case ToTime when 0 then 24 else ToTime end)))) 
                                      and pt.product_type in " + ProductType +
                                      @"and (@displayGroup = 'ALL' or isnull(case pdgf.displayGroup when '' then null else pdgf.displayGroup end, 'Others') = @displayGroup) 
                                      order by display_group, sort_order";

            int dayofweek = -1;
            switch (serverTime.DayOfWeek)
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
            DataTable dataTable = KioskStatic.Utilities.executeDataTable(CommandText,
                                                new SqlParameter("@functionType", Function),
                                                new SqlParameter("@POSMachine", KioskStatic.Utilities.ParafaitEnv.POSMachineId),
                                                new SqlParameter("@Counter", KioskStatic.Utilities.ParafaitEnv.POSTypeId),
                                                new SqlParameter("@displayGroup", displayGroup),
                                                new SqlParameter("@today", serverTime.Date),
                                                new SqlParameter("@lang", KioskStatic.Utilities.ParafaitEnv.LanguageId),
                                                new SqlParameter("@nowHour", serverTime.Hour + serverTime.Minute / 100.0),
                                                new SqlParameter("@DayNumber", serverTime.Day + 1000), // day of month stored as 1000 + day of month
                                                new SqlParameter("@weekDay", dayofweek));
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        public static DataTable getUpsellProducts(object ProductId)
        {
            log.LogMethodEntry(ProductId);
            int dayofweek = -1;
            DateTime serverTime = ServerDateTime.Now;
            switch (serverTime.DayOfWeek)
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

            DataTable dataTable = KioskStatic.Utilities.executeDataTable(@"select top 1 p.product_id, 
                                                                                  isnull((select translation 
                                                                                            from ObjectTranslations
                                                                                           where ElementGuid = p.Guid
                                                                                             and Element = 'PRODUCT_NAME'
                                                                                             and LanguageId = @lang), p.product_name) as product_name,
                                                                                  uso.OfferMessage, 
                                                                                  isnull(case when TaxInclusivePrice = 'Y' then p.price else p.price * (1 + isnull(t.tax_percentage, 0)/100.0) end, 0) as Price,
                                                                                  isnull((select translation 
                                                                                            from ObjectTranslations
                                                                                           where ElementGuid = p.Guid
                                                                                             and Element = 'DESCRIPTION'
                                                                                             and LanguageId = @lang), p.description) description
                                                                             from products p left outer join tax t on p.tax_id = t.tax_id, UpsellOffers uso
                                                                            where uso.productId = @productId
                                                                              and uso.OfferProductId = p.product_id
                                                                              and p.active_flag = 'Y'
                                                                              and uso.ActiveFlag = 1
                                                                              and ISNULL(p.WaiverSetId, -1) = -1
                                                                              and not exists (select 1 
                                                                                                from FacilityWaiver fw, CheckInFacility fac, 
                                                                                                     WaiverSet ws, FacilityMapDetails fmd,
                                                                                                     FacilityMap fm,
	                                                                                                 ProductsAllowedInFacility paif
                                                                                               where fac.FacilityId = fw.FacilityId
                                                                                                 and fac.active_flag = 'Y'
                                                                                                 and ISNULL(fw.EffectiveFrom, getdate()) >= getdate()
                                                                                                 and ISNULL(fw.EffectiveTo, getdate()) <= getdate()
                                                                                                 and fw.IsActive = 1
                                                                                                 and fw.FacilityWaiverId = ws.WaiverSetId
                                                                                                 and ws.IsActive = 1
                                                                                                 and fac.FacilityId = fmd.FacilityId
                                                                                                 and fmd.IsActive = 1
                                                                                                 and fmd.FacilityMapId = fm.FacilityMapId
                                                                                                 and fm.IsActive = 1
                                                                                                 and paif.FacilityMapId = fmd.FacilityMapId
                                                                                                 and paif.IsActive = 1
                                                                                                 and paif.ProductsId = p.product_id)
                                                                             and isnull(uso.EffectiveDate, getdate()) <= getdate()
                                                                             and (not exists (select 1 
                                                                                                 from ProductCalendar pc
                                                                                                 where pc.product_id = p.product_id)
                                                                                                 or exists (select 1 
                                                                                                            from (select top 1 date, day, case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort, 
                                                                                                            isnull(FromTime, @nowHour) FromTime, isnull(ToTime, @nowHour) ToTime, ShowHide -- // select in the order of specific date, day of month, weekday, every day. 
                                                                                                        --if there are multiple slots on same day, take the one which is in current hour
                                                                                                                        from ProductCalendar pc
                                                                                                     where pc.product_id = p.product_id
                                                                                                     and (Date = @today -- // specific date
                                                                                                     or Day = @DayNumber -- // 1001 to 1031
                                                                                                     or Day = @weekDay -- // 0 to 6
                                                                                                     or Day = -1) -- //everyday
                                                                                                     order by 1 desc, 2 desc, 3) inView
                                                                                                     where (ShowHide = 'Y' 
                                                                                                            and (@nowHour >= FromTime and @nowHour <= case ToTime when 0 then 24 else ToTime end))
                                                                                                        or (ShowHide = 'N' 
                                                                                                            and (@nowHour < FromTime or @nowHour > case ToTime when 0 then 24 else ToTime end)))) 
                                                                             order by uso.effectiveDate desc",
                                                        new SqlParameter("@lang", KioskStatic.Utilities.ParafaitEnv.LanguageId),
                                                        new SqlParameter("@productId", ProductId),
                                                        new SqlParameter("@POSMachine", KioskStatic.Utilities.ParafaitEnv.POSMachineId),
                                                        new SqlParameter("@Counter", KioskStatic.Utilities.ParafaitEnv.POSTypeId),
                                                        new SqlParameter("@today", serverTime.Date),
                                                        new SqlParameter("@nowHour", serverTime.Hour + serverTime.Minute / 100.0),
                                                        new SqlParameter("@DayNumber", serverTime.Day + 1000),
                                                        new SqlParameter("@weekDay", dayofweek)); // day of month stored as 1000 + day of month  
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        internal static RemotingClient CardRoamingRemotingClient = null;
        public static bool refreshCardFromHQ(ref Card CurrentCard, ref string message)
        {
            log.LogMethodEntry(CurrentCard, message);
            try
            {
                if (CardRoamingRemotingClient == null)
                {
                    if (KioskStatic.Utilities.ParafaitEnv.ALLOW_ROAMING_CARDS == "Y" && KioskStatic.Utilities.ParafaitEnv.ENABLE_ON_DEMAND_ROAMING == "Y")
                    {
                        try
                        {
                            CardRoamingRemotingClient = new RemotingClient();
                            KioskStatic.logToFile("Remoting client initialized");
                        }
                        catch (Exception ex)
                        {
                            message = KioskStatic.Utilities.MessageUtils.getMessage(1840);
                            KioskStatic.logToFile(message);
                            KioskStatic.logToFile(ex.Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
                KioskStatic.logToFile("Calling getCardFromHQ()");
                bool returnValue = new CardUtils(KioskStatic.Utilities).getCardFromHQ(CardRoamingRemotingClient, ref CurrentCard, ref message);

                KioskStatic.logToFile("Response from getCardFromHQ() - " + (returnValue ? "true": "false"));
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in refreshCardFromHQ: " + ex.Message);
                log.Error(ex);
                if (ex.Message.ToLower().Contains("fault"))
                {
                    message = KioskStatic.Utilities.MessageUtils.getMessage(216);
                    try
                    {
                        CardRoamingRemotingClient = new RemotingClient();
                    }
                    catch (Exception exe)
                    {
                        log.Error("Error occurred while executing refreshCardFromHQ()", ex);
                        KioskStatic.logToFile(exe.Message);
                        message = KioskStatic.Utilities.MessageUtils.getMessage(217);
                    }
                }
                else
                {
                    message = KioskStatic.Utilities.MessageUtils.getMessage(1840);
                    KioskStatic.logToFile(message + ex.Message);
                    //message = "On-Demand Roaming: " + ex.Message;
                }
                log.LogMethodExit(false);
                return false;
            }
        }

        public static bool isTimeEnabledStore()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(KioskStatic.Utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "TIME_ENABLED_STORE"));

            List<LookupValuesDTO> entitlementValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (entitlementValueList != null && entitlementValueList[0].LookupValue == "Y")
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

        public static void SendOTPEmail(CustomerDTO customerDTO, string OTP, Utilities utilities)
        {
            log.LogMethodEntry(customerDTO, OTP);
            if (customerDTO != null && string.IsNullOrEmpty(OTP) == false)
            {
                if (string.IsNullOrEmpty(customerDTO.Email) == false)
                {
                    string emailTemplateName = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "SEND_WAIVER_OTP_EMAIL_TEMPLATE");
                    EmailTemplateDTO emailTemplateDTO = null;
                    if (string.IsNullOrEmpty(emailTemplateName) == false)
                    {
                        EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(utilities.ExecutionContext);
                        List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, emailTemplateName));
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                        if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
                        {
                            emailTemplateDTO = emailTemplateDTOList[0];
                        }
                        if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > -1)
                        {

                            BuildEmailBodyAndSend(customerDTO, OTP, emailTemplateDTO, utilities);
                        }
                        else
                        {
                            log.Error("SEND_WAIVER_OTP_EMAIL_TEMPLATE is not set with valid email template");
                            throw new Semnox.Core.Utilities.ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2328)
                                                                         + "." + MessageContainerList.GetMessage(utilities.ExecutionContext, "Email Sending Failed"));
                        }
                    }
                    else
                    {
                        log.Error("SEND_WAIVER_OTP_EMAIL_TEMPLATE is not set with valid email template");
                        throw new Semnox.Core.Utilities.ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2328)
                                                                         + "." + MessageContainerList.GetMessage(utilities.ExecutionContext, "Email Sending Failed"));
                    }
                }
                else
                {
                    log.Error("No email id provided to send waiver email");
                    throw new Semnox.Core.Utilities.ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, "No Email Id")
                                                                         + "." + MessageContainerList.GetMessage(utilities.ExecutionContext, "Email Sending Failed"));
                }
            }
            else
            {
                log.Error("OTP /Customer info is missing");
                throw new Semnox.Core.Utilities.ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2327)
                                                                     + "." + MessageContainerList.GetMessage(utilities.ExecutionContext, "Email Sending Failed"));
            }
            log.LogMethodExit();
        }

        private static void BuildEmailBodyAndSend(CustomerDTO customerDTO, string OTP, EmailTemplateDTO emailTemplateDTO, Utilities utilities)
        {
            log.LogMethodEntry();
            string body = emailTemplateDTO.EmailTemplate;
            body = GenerateWaiverOTPEmailContent(body, customerDTO, OTP, utilities);
            Messaging messaging = new Messaging(utilities);
            messaging.SendEMail(customerDTO.Email, emailTemplateDTO.Description, body, customerDTO.Id.ToString(), string.Empty, null, null, null, MessageContainerList.GetMessage(utilities.ExecutionContext, "Waiver OTP"));
            log.LogMethodExit();
        }

        private static string GenerateWaiverOTPEmailContent(string body, CustomerDTO customerDTO, string OTP, Utilities utilities)
        {
            log.LogMethodEntry(body, customerDTO, OTP);
            if (customerDTO != null)
            {
                body = body.Replace("@siteName", utilities.ParafaitEnv.SiteName);
                body = body.Replace("@customerName", customerDTO.FirstName);
                body = body.Replace("@emailId", customerDTO.Email);
                body = body.Replace("@emailAddress", customerDTO.Email);
                body = body.Replace("@waiverOTP", OTP);
            }
            log.LogMethodExit(body);
            return body;
        }

        public static int GetShowXRecordsInCustomerDashboard(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            int showXRecordsInCustomerDashboard = 1000;
            try
            {
                List<LookupValuesDTO> lookupValuesDTOList = GetLookupValueDTOList(executionContext, KIOSK_SETUP_LOOKUP, "SHOW_X_RECORDS_IN_CUSTOMER_DASHBOARD");
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    showXRecordsInCustomerDashboard = Convert.ToInt32(lookupValuesDTOList[0].Description);
                    log.Info("showXRecordsInCustomerDashboard: " + showXRecordsInCustomerDashboard.ToString());
                    KioskStatic.logToFile("showXRecordsInCustomerDashboard: " + showXRecordsInCustomerDashboard.ToString());
                    if (showXRecordsInCustomerDashboard > 1000 || showXRecordsInCustomerDashboard < 1)
                    {
                        showXRecordsInCustomerDashboard = 1000;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                showXRecordsInCustomerDashboard = 1000;
            }
            log.LogMethodExit(showXRecordsInCustomerDashboard);
            return showXRecordsInCustomerDashboard;
        }
        public static List<LookupValuesDTO> GetLookupValueDTOList(ExecutionContext executionContext, string lookupName, string lookupValue = null)
        {
            log.LogMethodEntry(executionContext, lookupName, lookupValue);
            List<LookupValuesDTO> lookupValuesDTOList = null;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            if (string.IsNullOrWhiteSpace(lookupValue) == false)
            {
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, lookupValue));
            }
            searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParam);
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList;
        }
        public static List<ProductsDTO> GetActiveFundRaiserProducts(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            List<ProductsDTO> activeProductList = GetProductsByFundRaiseOrDonationCategory(executionContext, KioskStatic.FUND_RAISER_LOOKUP_VALUE);
            log.LogMethodExit(activeProductList);
            return activeProductList;
        }
        public static List<ProductsDTO> GetActiveDonationProducts(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            List<ProductsDTO> activeProductList = GetProductsByFundRaiseOrDonationCategory(executionContext, KioskStatic.DONATIONS_LOOKUP_VALUE);
            log.LogMethodExit(activeProductList);
            return activeProductList;
        }

        private static List<ProductsDTO> GetProductsByFundRaiseOrDonationCategory(ExecutionContext executionContext, string categoryType)
        {
            log.LogMethodEntry(executionContext, categoryType);
            List<ProductsDTO> activeProductList = new List<ProductsDTO>();
            try
            {
                int currentUserRoleId = GetCurrentUserRoleId(executionContext);
                string categoryName = string.Empty;
                LookupsContainerDTO fundsDonationsLookupValuesList = LookupsContainerList.GetLookupsContainerDTO(-1, KioskStatic.FUND_RAISER_AND_DONATIONS_LOOKUP);
                for (int i = 0; i < fundsDonationsLookupValuesList.LookupValuesContainerDTOList.Count; i++)
                {
                    if (fundsDonationsLookupValuesList.LookupValuesContainerDTOList[i].LookupValue == categoryType)
                    {
                        categoryName = fundsDonationsLookupValuesList.LookupValuesContainerDTOList[i].Description;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(categoryName))
                {
                    try
                    {
                        int categoryId = -1;
                        //get category id from category name
                        List<CategoryContainerDTO> categoryContainerDTOList = CategoryContainerList.GetCategoryContainerDTOList(KioskStatic.Utilities.ExecutionContext.SiteId);
                        if (categoryContainerDTOList != null && categoryContainerDTOList.Any())
                        {
                            for (int i = 0; i < categoryContainerDTOList.Count; i++)
                            {
                                if (categoryContainerDTOList[i].Name == categoryName)
                                {
                                    categoryId = categoryContainerDTOList[i].CategoryId;
                                    break;
                                }
                            }

                            if (categoryId > -1)
                            {
                                ProductsList productsList = new ProductsList(executionContext);
                                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> productSearchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                                productSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                                productSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.CATEGORY_ID, categoryId.ToString()));
                                productSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "Y"));

                                //Transaction product Search Params
                                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> trxProductsSearchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                                trxProductsSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));

                                System.Globalization.CultureInfo currentcultureprovider = System.Globalization.CultureInfo.CurrentCulture;
                                DateTime currentTime = Convert.ToDateTime(KioskStatic.Utilities.getServerTime().ToString(), currentcultureprovider);

                                trxProductsSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.TRX_ONLY_PRODUCT_PURCHASE_DATE, currentTime.ToString()));
                                if (currentUserRoleId > -1)
                                {
                                    trxProductsSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.TRX_ONLY_USER_ROLE, currentUserRoleId.ToString()));
                                }
                                trxProductsSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.TRX_ONLY_POS_MACHINE, KioskStatic.Utilities.ExecutionContext.POSMachineName.ToString()));
                                int membershipId = -1;
                                trxProductsSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.TRX_ONLY_MEMBERSHIP, membershipId.ToString()));
                                activeProductList = productsList.GetProductsList(productSearchParams, 0, 1000, false, true, null, true, trxProductsSearchParams);
                            }
                            else
                            {
                                log.Debug(string.Format(" categoryName - {0} - does not exists", categoryName));
                                KioskStatic.logToFile("categoryName - "+ categoryName+" - does not exists" );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        KioskStatic.logToFile("Error while fetching fund/donation products : " + ex.Message);
                    }
                }
                else
                {
                    log.Debug(string.Format(" categoryName - {0} - does not exists", categoryName));
                    KioskStatic.logToFile("categoryName - " + categoryName + " - does not exists");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while fetching fund/donation products : " + ex.Message);
            }
            log.LogMethodExit(activeProductList);
            return activeProductList;
        }

        private static int GetCurrentUserRoleId(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            int currentUserRoleId = -1;
            try
            {
                string loginId = executionContext.GetUserId();
                if (!string.IsNullOrWhiteSpace(loginId))
                {
                    UsersList usersList = new UsersList(executionContext);
                    List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                    searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, loginId));
                    searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<UsersDTO> usersDTOList = usersList.GetAllUsers(searchParameters);
                    if (usersDTOList != null && usersDTOList.Any())
                    {
                        currentUserRoleId = usersDTOList[0].RoleId;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while fetching current user role id details. " + ex.Message);
            }
            log.LogMethodExit(currentUserRoleId);
            return currentUserRoleId;
        }
        /// <summary>
        /// GetRoamingClient
        /// </summary>
        /// <returns></returns>
        public static RemotingClient GetRoamingClient()
        {
            log.LogMethodEntry();
            if (CardRoamingRemotingClient == null)
            {
                if (KioskStatic.Utilities.ParafaitEnv.ALLOW_ROAMING_CARDS == "Y" && KioskStatic.Utilities.ParafaitEnv.ENABLE_ON_DEMAND_ROAMING == "Y")
                {
                    CardRoamingRemotingClient = new RemotingClient();
                    KioskStatic.logToFile("Remoting client initialized");
                }
            }
            log.LogMethodExit(CardRoamingRemotingClient);
            return CardRoamingRemotingClient;
        }

        public static DataGridViewCellStyle gridViewDateOfBirthCellStyle()
        {
            log.LogMethodEntry();

            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.Format = KioskStatic.DateMonthFormat;
            log.LogMethodExit(style);
            return style;
        }

        public static List<CustomerRelationshipDTO> GetAllLinkedRelations(int customerId, bool buildChildRecords = false, bool activeChildRecords = false)
        {
            log.LogMethodEntry(customerId, buildChildRecords, activeChildRecords);
            List<CustomerRelationshipDTO> customerRelationshipDTOList = null;
            try
            {
                KioskStatic.logToFile("Getting Customer Linked Relations");
                List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchCustomerRelationshipParams = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
                searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));

                CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(KioskStatic.Utilities.ExecutionContext);
                customerRelationshipDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchCustomerRelationshipParams, buildChildRecords, activeChildRecords, null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in GetAllLinkedRelations(): " + ex);
            }
            log.LogMethodExit(customerRelationshipDTOList);
            return customerRelationshipDTOList;
        }

        public static List<CustomerRelationshipDTO> GetLinkedRelationsFilteredByAge(int customerId, decimal lowerAgeLimit, decimal upperAgeLimit)
        {
            log.LogMethodEntry(customerId, lowerAgeLimit, upperAgeLimit);
            List<CustomerRelationshipDTO> relationsFilteredByAgeDTOList = null;
            try
            {
                List<CustomerRelationshipDTO> customerRelationshipDTOList = GetAllLinkedRelations(customerId, true, true);
                relationsFilteredByAgeDTOList = new List<CustomerRelationshipDTO>();

                if ((customerRelationshipDTOList != null) && customerRelationshipDTOList.Any())
                {
                    foreach (CustomerRelationshipDTO customerRelationshipDTO in customerRelationshipDTOList)
                    {
                        DateTime dateOfBirth = DateTime.MinValue;
                        if (customerRelationshipDTO.RelatedCustomerDTO.DateOfBirth != null
                            && DateTime.TryParse(customerRelationshipDTO.RelatedCustomerDTO.DateOfBirth.ToString(), out dateOfBirth) == true)
                        {
                            decimal age = GetAge(customerRelationshipDTO.RelatedCustomerDTO.DateOfBirth.ToString());
                            if ((age >= Convert.ToInt32(lowerAgeLimit)) && (age <= Convert.ToInt32(upperAgeLimit)))
                            {
                                relationsFilteredByAgeDTOList.Add(customerRelationshipDTO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while getting populating Linked Children: " + ex);
            }
            log.LogMethodExit(relationsFilteredByAgeDTOList);
            return relationsFilteredByAgeDTOList;
        }

        /// <summary>
        /// GetAge
        /// </summary>
        /// <param name="dateOfBirthValue"></param>
        /// <returns></returns>
        public static decimal GetAge(string dateOfBirthValue)
        {
            log.LogMethodEntry(dateOfBirthValue);
            decimal age = -1;
            try
            {
                if (!string.IsNullOrEmpty(dateOfBirthValue))
                {
                    DateTime dateOfBirth = Convert.ToDateTime(dateOfBirthValue);
                    age = Convert.ToDecimal(ServerDateTime.Now.Subtract(dateOfBirth).TotalDays * 0.00273790926);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in GetAge() - Kiosk helper: " + ex.Message);
            }
            log.LogMethodExit(age);
            return age;
        }

        /// <summary>
        /// GetAge
        /// </summary>
        /// <param name="dateOfBirthValue"></param>
        /// <returns></returns>
        public static decimal GetAge(DateTime? dateOfBirthValue)
        {
            log.LogMethodEntry(dateOfBirthValue);
            decimal age = -1;
            try
            {
                if (dateOfBirthValue != null)
                {
                    DateTime dateOfBirth = Convert.ToDateTime(dateOfBirthValue);
                    age = Convert.ToDecimal(ServerDateTime.Now.Subtract(dateOfBirth).TotalDays * 0.00273790926);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in GetAge() - Kiosk helper: " + ex.Message);
            }
            log.LogMethodExit(age);
            return age;
        }

        /// <summary>
        /// GetMaskedCardNumber
        /// </summary> 
        public static string GetMaskedCardNumber(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            string maskedCardNumber = cardNumber;
            try
            {
                if (!string.IsNullOrEmpty(cardNumber) && KioskStatic.MASKCARDNUMBER == true)
                {
                    string temp = cardNumber.ToString();
                    temp = temp.Substring(cardNumber.Length - KioskStatic.LASTXDIGITSTOSHOW);
                    maskedCardNumber = new string('X', cardNumber.Length - KioskStatic.LASTXDIGITSTOSHOW) + temp;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in GetMaskedCardNumber() of CheckInSummary : " + ex.Message);
            }
            log.LogMethodExit(maskedCardNumber);
            return maskedCardNumber;
        }

        /// <summary>
        /// AddNewCardProduct
        /// </summary> 
        public static KioskTransaction AddNewCardProduct(KioskTransaction kioskTransaction, DataRow productRow, int quantity, string selectedEntitlementType)
        {
            log.LogMethodEntry("kioskTransaction", productRow, quantity, selectedEntitlementType);
            int productId = Convert.ToInt32(productRow["product_id"]);
            bool variableProduct = (productRow["product_type"].ToString().Equals("VARIABLECARD"));
            double? overridePrice = (variableProduct ? Convert.ToDouble(productRow["price"]) : (double?)null);
            kioskTransaction.SetSelectedEntitlementType(selectedEntitlementType);
            if (variableProduct)
            {
                kioskTransaction.AddVariableCardProduct(productId, (double)overridePrice, selectedEntitlementType, null, quantity);
            }
            else
            {
                kioskTransaction.AddNewCardProduct(productId, quantity, overridePrice);
            }
            log.LogMethodExit();
            return kioskTransaction;
        }
        /// <summary>
        /// AddRechargeCardProduct
        /// </summary> 
        /// <returns></returns>
        public static KioskTransaction AddRechargeCardProduct(KioskTransaction kioskTransaction, Card rechargeCard, DataRow productRow, int quantity,
                                      string selectedEntitlementType)
        {
            log.LogMethodEntry("kioskTransaction", rechargeCard, productRow, quantity, selectedEntitlementType);
            int productId = Convert.ToInt32(productRow["product_id"]);
            bool variableProduct = (productRow["product_type"].ToString().Equals("VARIABLECARD"));
            double? overridePrice = (variableProduct ? Convert.ToDouble(productRow["price"]) : (double?)null);
            kioskTransaction.SetSelectedEntitlementType(selectedEntitlementType);
            if (variableProduct)
            {
                kioskTransaction.AddVariableCardProduct(productId, (double)overridePrice, selectedEntitlementType, rechargeCard, quantity);
            }
            else
            {
                kioskTransaction.AddRechargeCardProduct(rechargeCard, productId, quantity, overridePrice);
            }
            log.LogMethodExit();
            return kioskTransaction;
        }

        public class LineData
        {
            string productName;
            string lineType;
            Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine;
            public LineData(string lineType, string prodName, Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine)
            {
                this.lineType = lineType;
                this.productName = prodName;
                this.trxLine = trxLine;
            }
            public string LineType { get { return lineType; } }
            public string ProductName { get { return productName; } }
            public Semnox.Parafait.Transaction.Transaction.TransactionLine TrxLine { get { return trxLine; } }
        }

        public static List<List<Semnox.Parafait.Transaction.Transaction.TransactionLine>> ConvertTrxLineData(List<List<KioskHelper.LineData>> displayTrxLineData)
        {
            log.LogMethodEntry();
            List<List<Semnox.Parafait.Transaction.Transaction.TransactionLine>> trxLineData = new List<List<Transaction.Transaction.TransactionLine>>();
            if (displayTrxLineData != null && displayTrxLineData.Any())
            {
                for (int i = 0; i < displayTrxLineData.Count; i++)
                {
                    List<KioskHelper.LineData> lineDataList = displayTrxLineData[i];
                    if (lineDataList != null && lineDataList.Any())
                    {
                        List<Semnox.Parafait.Transaction.Transaction.TransactionLine> finalLineData = new List<Transaction.Transaction.TransactionLine>();
                        for (int j = 0; j < lineDataList.Count; j++)
                        {
                            finalLineData.Add(lineDataList[j].TrxLine);
                        }
                        trxLineData.Add(finalLineData);
                    }
                }
            }
            log.LogMethodExit();
            return trxLineData;
        }

        public static List<PaymentModesContainerDTO> GetPaymentModesWithDisplayGroups()
        {
            log.LogMethodEntry();
            List<PaymentModesContainerDTO> paymentModesContainerDTOsWithDisplayGroups = null;
            try
            {
                POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(KioskStatic.Utilities.ExecutionContext.SiteId, KioskStatic.Utilities.ExecutionContext.MachineId);
                List<int> posDisplayGroupInclusionListIds = pOSMachineContainerDTO.ProductDisplayGroupFormatContainerDTOList.Select(p => p.Id).ToList();
                bool isPaymentModeDrivenSalesInKioskEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_PAYMENT_MODE_DRIVEN_SALES", false);
                if (isPaymentModeDrivenSalesInKioskEnabled == true && KioskStatic.pOSPaymentModeInclusionDTOList != null && KioskStatic.pOSPaymentModeInclusionDTOList.Any())
                {
                    foreach (POSPaymentModeInclusionDTO pOSPaymentModeInclusion in KioskStatic.pOSPaymentModeInclusionDTOList)
                    {
                        PaymentModesContainerDTO containerDTO = PaymentModesContainerList.GetPaymentModesContainerDTO(KioskStatic.Utilities.ExecutionContext, pOSPaymentModeInclusion.PaymentModeId);
                        if (containerDTO.PaymentModeDisplayGroupsContainerDTOList != null && containerDTO.PaymentModeDisplayGroupsContainerDTOList.Any())
                        {
                            if (paymentModesContainerDTOsWithDisplayGroups == null)
                            {
                                paymentModesContainerDTOsWithDisplayGroups = new List<PaymentModesContainerDTO>();
                            }

                            List<int> preSelectPaymentDisplayGroupIds = containerDTO.PaymentModeDisplayGroupsContainerDTOList.Select(p => p.ProductDisplayGroupId).ToList();
                            List<int> CommonDisplayGroupIds = posDisplayGroupInclusionListIds.Intersect(preSelectPaymentDisplayGroupIds).ToList();

                            if (CommonDisplayGroupIds != null && CommonDisplayGroupIds.Any())
                            {
                                paymentModesContainerDTOsWithDisplayGroups.Add(containerDTO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(paymentModesContainerDTOsWithDisplayGroups);
            return paymentModesContainerDTOsWithDisplayGroups;
        }

        public static List<int> GetPaymentModesIdsWithDisplayGroup(PaymentModesContainerDTO preSelectedPaymentMode)
        {
            log.LogMethodEntry();
            List<int> displayGroupIds = null;
            try
            {
                if (preSelectedPaymentMode != null)
                {
                    POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(KioskStatic.Utilities.ExecutionContext.SiteId, KioskStatic.Utilities.ExecutionContext.MachineId);
                    List<int> posDisplayGroupInclusionListIds = pOSMachineContainerDTO.ProductDisplayGroupFormatContainerDTOList.Select(p => p.Id).ToList();
                    bool isPaymentModeDrivenSalesInKioskEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_PAYMENT_MODE_DRIVEN_SALES", false);
                    if (isPaymentModeDrivenSalesInKioskEnabled == true && KioskStatic.pOSPaymentModeInclusionDTOList != null && KioskStatic.pOSPaymentModeInclusionDTOList.Any())
                    {
                        if (preSelectedPaymentMode.PaymentModeDisplayGroupsContainerDTOList != null && preSelectedPaymentMode.PaymentModeDisplayGroupsContainerDTOList.Any())
                        {
                            List<int> preSelectPaymentDisplayGroupIds = preSelectedPaymentMode.PaymentModeDisplayGroupsContainerDTOList.Select(p => p.ProductDisplayGroupId).ToList();
                            List<int> commonIds = posDisplayGroupInclusionListIds.Intersect(preSelectPaymentDisplayGroupIds).ToList();
                            if (commonIds != null && commonIds.Any())
                            {
                                if (displayGroupIds == null)
                                {
                                    displayGroupIds = new List<int>();
                                }
                                displayGroupIds.AddRange(commonIds);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(displayGroupIds);
            return displayGroupIds;
        }

        public static double GetChargesToDisplay(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry("kioskTransaction");
            double serviceCharges = 0;
            double gratuityCharges = 0;
            serviceCharges = kioskTransaction.GetTransactionServiceCharges();
            gratuityCharges = kioskTransaction.GetTransactionGratuityAmount();
            double taxAmount = kioskTransaction.GetTaxAmountOnCharges();
            double totalCharges = 0;
            totalCharges = serviceCharges + gratuityCharges - taxAmount;
            log.LogMethodExit(totalCharges);
            return totalCharges;
        }

        public static double GetSubTotalForDisplay(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry("kioskTransaction");
            double totalAmount = kioskTransaction.GetTransactionAmount();
            double totalCharges = GetChargesToDisplay(kioskTransaction);
            double taxAmount = kioskTransaction.GetTrxTaxAmount();
            //double discountAmount = kioskTransaction.GetTransactionDiscountsAmount();
            double subTotal = totalAmount - totalCharges - taxAmount;
            log.LogMethodExit(subTotal);
            return subTotal;
        }
        /// <summary>
        /// SendPrintSummaryEmail
        /// </summary>
        public static void SendPrintSummaryEmail(ExecutionContext executionContext, string emailId, string imageBase64)
        {
            log.LogMethodEntry(executionContext, emailId, "imageBase64");
            if (string.IsNullOrWhiteSpace(emailId) == false)
            {
                log.LogVariableState("imageBase64", imageBase64);
                string msgRef = MessageContainerList.GetMessage(executionContext, "Kiosk Print Summary"); //add as literal
                string msgSubject = MessageContainerList.GetMessage(executionContext, "Kiosk Print Summary Report");//add as literal
                string firstHalfMsg = MessageContainerList.GetMessage(executionContext, 5091);
                string secondHalfMsg = MessageContainerList.GetMessage(executionContext, 5092, imageBase64, executionContext.POSMachineName);
                string msgBody = firstHalfMsg + secondHalfMsg;//full msg does not fit in a single message number in db
                MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, msgRef, MessagingClientDTO.SourceEnumToString(MessagingClientDTO.MessagingChanelType.EMAIL), emailId, null, null, null, null, null, null, msgSubject, msgBody, -1, null, null, true, null, null, -1, false, null);
                MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                messagingRequestBL.Save();
            }
            else
            {

            }
            log.LogMethodExit();
        }
        /// <summary>
        /// ConvertTextToBase64String
        /// </summary> 
        /// <returns></returns>
        public static string ConvertTextToBase64String(List<string> input, int pageWidth, string fontName, float fontSize)
        {
            log.LogMethodEntry("input", pageWidth, fontName, fontSize);
            string base64String = null;
            try
            {
                Font f = new Font(fontName, fontSize);

                int imageWidth = pageWidth;
                int imageHeight = (int)(f.GetHeight() * input.Count + 5);
                Bitmap bitmap = new Bitmap(imageWidth, imageHeight);

                Graphics graphics = Graphics.FromImage(bitmap);
                float height = graphics.MeasureString("ABCD", f).Height;
                float locY = 10;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                foreach (string s in input)
                {
                    graphics.DrawString(s, f, Brushes.Black, new Rectangle(0, (int)locY, imageWidth, (int)height), sf);
                    locY += height;
                }

                MemoryStream memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, ImageFormat.Png);
                // Get the byte array from the memory stream
                byte[] imageBytes = memoryStream.ToArray();
                base64String = Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in ConvertTextToBase64String() : " + ex.Message);
            }
            log.LogMethodExit(base64String);
            return base64String;
        }

        public static List<List<LineData>> GroupDisplayTrxLinesNew(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLineList)
        {
            log.LogMethodEntry(trxLineList);
            List<List<LineData>> trxLineData = new List<List<LineData>>();
            for (int i = 0; i < trxLineList.Count; i++)
            {
                if (trxLineList[i].ProductTypeCode == "LOYALTY")
                    trxLineList[i].LineProcessed = true;
                else
                    trxLineList[i].LineProcessed = false;
            }
            if (trxLineList != null && trxLineList.Count == 1
                && trxLineList.Exists(tl => tl.LineProcessed == false && tl.LineValid
                        && (tl.ProductTypeCode == ProductTypeValues.CARDDEPOSIT
                         || tl.ProductTypeCode == ProductTypeValues.DEPOSIT
                         || tl.ProductTypeCode == ProductTypeValues.LOCKERDEPOSIT)))
            {

                List<LineData> newList = new List<LineData>();
                newList = AddLine(trxLineList, trxLineList[0], newList);
                if (newList != null && newList.Any())
                {
                    trxLineData.Add(newList);
                }
            }
            else
            {
                for (int i = 0; i < trxLineList.Count; i++)
                {
                    Semnox.Parafait.Transaction.Transaction.TransactionLine currentLine = trxLineList[i];
                    if (currentLine.LineValid && currentLine.LineProcessed == false
                        && currentLine.ProductTypeCode != ProductTypeValues.SERVICECHARGE
                        && currentLine.ProductTypeCode != ProductTypeValues.GRATUITY
                        && currentLine.ProductTypeCode != ProductTypeValues.CARDDEPOSIT
                        && currentLine.ProductTypeCode != ProductTypeValues.DEPOSIT
                        && currentLine.ProductTypeCode != ProductTypeValues.LOCKERDEPOSIT)
                    {
                        List<LineData> newList = new List<LineData>();
                        newList = AddLine(trxLineList, currentLine, newList);
                        if (newList != null && newList.Any())
                        {
                            trxLineData.Add(newList);
                        }
                    }
                }
            }
            log.LogMethodExit(trxLineData);
            return trxLineData;
        }

        private static List<LineData> AddLine(List<Transaction.Transaction.TransactionLine> trxLineList, Transaction.Transaction.TransactionLine currentLine, List<LineData> newList)
        {
            log.LogMethodEntry("trxLineList", currentLine, newList);
            bool hasChildren = HasChildren(currentLine, trxLineList);

            LineData lineData = CreateLineData(currentLine);
            newList.Add(lineData);
            currentLine.LineProcessed = true;
            if (hasChildren)
            {
                newList = AddChildren(currentLine, trxLineList, newList);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(currentLine.CardNumber) == false)
                {
                    newList = AddLinesWithSameCardNumber(currentLine, trxLineList, newList);
                }
                else
                {
                    newList = AddLinesWithSameProduct(currentLine, trxLineList, newList);
                }
            }
            log.LogMethodExit(newList);
            return newList;
        }

        private static LineData CreateLineData(Transaction.Transaction.TransactionLine currentLine)
        {
            log.LogMethodEntry(currentLine);
            string productName = string.Empty;
            if (currentLine.ProductTypeCode == "ATTRACTION")
            {
                productName = currentLine.ProductName + (string.IsNullOrEmpty(currentLine.AttractionDetails) ? "" : "-" + currentLine.AttractionDetails) + (string.IsNullOrEmpty(currentLine.Remarks) ? "" : "-" + currentLine.Remarks);
            }
            else if (currentLine.ProductTypeCode == "LOCKER")
            {
                productName = currentLine.ProductName + "-Locker:" + currentLine.LockerName + (string.IsNullOrEmpty(currentLine.Remarks) ? "" : "-" + currentLine.Remarks);
            }
            else
            {
                productName = currentLine.ProductName + (string.IsNullOrEmpty(currentLine.Remarks) ? "" : "-" + currentLine.Remarks);
            }

            LineData lineData = new LineData(currentLine.ProductTypeCode, productName, currentLine);
            log.LogMethodExit();
            return lineData;
        }

        private static bool HasChildren(Transaction.Transaction.TransactionLine currentLine, List<Transaction.Transaction.TransactionLine> trxLineList)
        {
            log.LogMethodEntry(currentLine);
            bool hasChildLines = false;
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tlChild in trxLineList)
            {
                if (tlChild.LineProcessed == false && tlChild.LineValid && tlChild.ParentLine != null && currentLine.Equals(tlChild.ParentLine))
                {
                    hasChildLines = true;
                    break;
                }
            }
            log.LogMethodExit(hasChildLines);
            return hasChildLines;
        }

        private static List<LineData> AddChildren(Transaction.Transaction.TransactionLine currentLine, List<Transaction.Transaction.TransactionLine> trxLineList, List<LineData> newList)
        {
            log.LogMethodEntry(currentLine, "trxLineList", newList);
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tlChild in trxLineList)
            {
                if (tlChild.LineProcessed == false && tlChild.LineValid && tlChild.ParentLine != null && currentLine.Equals(tlChild.ParentLine))
                {
                    newList = AddLine(trxLineList, tlChild, newList);
                }
            }
            newList = AddDepositLine(currentLine, trxLineList, newList);
            log.LogMethodExit(newList);
            return newList;
        }

        private static List<LineData> AddLinesWithSameCardNumber(Transaction.Transaction.TransactionLine currentLine, List<Transaction.Transaction.TransactionLine> trxLineList, List<LineData> newList)
        {
            log.LogMethodEntry(currentLine, "trxLineList", newList);
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tlChild in trxLineList)
            {
                //same card number - idenpendent lines or belongs to same parent
                if (tlChild.LineProcessed == false && tlChild.LineValid && string.IsNullOrWhiteSpace(tlChild.CardNumber) == false
                    && tlChild.CardNumber == currentLine.CardNumber
                    && string.IsNullOrWhiteSpace(tlChild.ProductName) == false
                    && tlChild.ProductName == currentLine.ProductName
                    && ((tlChild.ParentLine == null && currentLine.ParentLine == null)
                       || (tlChild.ParentLine != null && currentLine.ParentLine != null && tlChild.ParentLine.Equals(currentLine.ParentLine))
                       ))
                {
                    LineData lineData = CreateLineData(tlChild);
                    newList.Add(lineData);
                    tlChild.LineProcessed = true;
                    newList = AddChildren(tlChild, trxLineList, newList);
                }
            }
            newList = AddDepositLine(currentLine, trxLineList, newList);
            log.LogMethodExit(newList);
            return newList;
        }

        private static List<LineData> AddDepositLine(Transaction.Transaction.TransactionLine currentLine, List<Transaction.Transaction.TransactionLine> trxLineList, List<LineData> newList)
        {
            log.LogMethodEntry(currentLine, "trxLineList", newList);
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tlChild in trxLineList)
            {
                //same card number - idenpendent lines or belongs to same parent
                if (tlChild.LineProcessed == false && tlChild.LineValid && string.IsNullOrWhiteSpace(tlChild.CardNumber) == false
                    && tlChild.CardNumber == currentLine.CardNumber
                    && tlChild.ParentLine == null
                    && (tlChild.ProductTypeCode == ProductTypeValues.CARDDEPOSIT
                         || tlChild.ProductTypeCode == ProductTypeValues.DEPOSIT
                         || tlChild.ProductTypeCode == ProductTypeValues.LOCKERDEPOSIT))
                {
                    LineData lineData = CreateLineData(tlChild);
                    newList.Add(lineData);
                    tlChild.LineProcessed = true;
                }
            }
            log.LogMethodExit(newList);
            return newList;
        }

        private static List<LineData> AddLinesWithSameProduct(Transaction.Transaction.TransactionLine currentLine, List<Transaction.Transaction.TransactionLine> trxLineList, List<LineData> newList)
        {
            log.LogMethodEntry(currentLine, "trxLineList", newList);
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tlChild in trxLineList)
            {
                //same product name - idenpendent lines or belongs to same parent
                if (tlChild.LineProcessed == false && tlChild.LineValid && string.IsNullOrWhiteSpace(tlChild.ProductName) == false
                    && tlChild.ProductName == currentLine.ProductName
                    && ((tlChild.ParentLine == null && currentLine.ParentLine == null)
                       || (tlChild.ParentLine != null && currentLine.ParentLine != null && tlChild.ParentLine.Equals(currentLine.ParentLine))
                       ))
                {
                    LineData lineData = CreateLineData(tlChild);
                    newList.Add(lineData);
                    tlChild.LineProcessed = true;
                    newList = AddChildren(tlChild, trxLineList, newList);
                }
            }
            log.LogMethodExit(newList);
            return newList;
        }

        private static int inactivityTimeoutSeconds = 10;
        private static bool eventAttached = false;
        private static DateTime lastActivityTime;
        private static DatePickerView datePickerView = null;
        private static Timer datePickerInactivityTimer = null;
        public delegate void PopupAlerts(string statusMessage);
        private static PopupAlerts KioskPopupAlerts;

        public static DateTime LaunchCalendar(DateTime defaultDateTimeToShow, bool enableDaySelection, bool enableMonthSelection,
            bool enableYearSelection, DateTime disableTill, bool showTimePicker, PopupAlerts popupAlerts, int inactivityTimeoutSec = 10)
        {
            log.LogMethodEntry(defaultDateTimeToShow, enableDaySelection, enableMonthSelection,
                disableTill, showTimePicker, popupAlerts, inactivityTimeoutSec);
            DateTime selectedDate = defaultDateTimeToShow;
            datePickerInactivityTimer = new Timer();
            try
            {
                KioskPopupAlerts = popupAlerts;
                inactivityTimeoutSeconds = inactivityTimeoutSec;
                datePickerInactivityTimer.Interval = 1000;
                datePickerInactivityTimer.Tick += new EventHandler(DatePickerInactivityTicker);
                lastActivityTime = DateTime.Now;
                eventAttached = false;
                string Theme = KioskStatic.CurrentTheme.ThemeId.ToString();
                string ThemeFolder = string.Empty;
                string defaultThemeFolderPath =
                    Application.StartupPath + @"\Styles\Styles.xaml";
                string themeFolderPath =
                    Application.StartupPath + @"\Media\Images\Theme" + Theme + @"\Styles\KioskStyles.xaml";
                string bkpThemeFolderFullPath =
                    Application.StartupPath + @"\Media\Images\DefaultThemes\Theme" + Theme + @"\Styles\KioskStyles.xaml";
                if (File.Exists(themeFolderPath))
                {
                    string msg = "Kiosk Theme folder " + themeFolderPath + " is found";
                    KioskStatic.logToFile(msg);
                    ThemeFolder = themeFolderPath;
                }
                else if (File.Exists(bkpThemeFolderFullPath))
                {
                    string msg = "Backup Kiosk Theme folder " + bkpThemeFolderFullPath + " is found";
                    KioskStatic.logToFile(msg);
                    ThemeFolder = bkpThemeFolderFullPath;
                }
                else if (File.Exists(defaultThemeFolderPath))
                {
                    string msg = "Separate Kiosk Theme folder " + themeFolderPath + " is not defined. Kiosk will try use images available under the root folder: " + defaultThemeFolderPath;
                    KioskStatic.logToFile(msg);
                    ThemeFolder = defaultThemeFolderPath;
                }
                else
                {
                    throw new Semnox.Core.Utilities.ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5161));  //Calendar is not set properly. Please check the setup
                }
                ParafaitPOS.App.machineUserContext = KioskStatic.Utilities.ExecutionContext;
                DatePickerValidationType ValidationType = enableYearSelection ? DatePickerValidationType.DateWithYear : DatePickerValidationType.DateWithoutYear;
                string defaultDate = defaultDateTimeToShow.ToString();
                ParafaitPOS.App.EnsureApplicationResources(new List<string>() { ThemeFolder });
                datePickerView = new DatePickerView();
                DatePickerVM datePickerVM = new DatePickerVM();
                datePickerVM.ExecutionContext = KioskStatic.Utilities.ExecutionContext;
                datePickerVM.DisableTill = disableTill;
                datePickerVM.EnableDaySelection = enableDaySelection;
                datePickerVM.EnableMonthSelection = enableMonthSelection;
                datePickerVM.EnableYearSelection = ValidationType == DatePickerValidationType.DateWithoutYear ? false : true;
                datePickerVM.ShowTimePicker = showTimePicker;
                datePickerVM.EditableMonthYear = true; //allow manual text entry

                datePickerView.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(DatePickerKeyAction);
                datePickerView.PreviewKeyUp += new System.Windows.Input.KeyEventHandler(DatePickerKeyAction);
                datePickerView.Loaded += new System.Windows.RoutedEventHandler(ResizeDatePickerOnLoaded);
                datePickerView.KeyDown += new System.Windows.Input.KeyEventHandler(DatePickerKeyAction);
                datePickerView.KeyUp += new System.Windows.Input.KeyEventHandler(DatePickerKeyAction);
                datePickerView.MouseDown += new MouseButtonEventHandler(DatePickerMouseAction);
                datePickerView.MouseUp += new MouseButtonEventHandler(DatePickerMouseAction);
                datePickerView.PreviewMouseDown += new MouseButtonEventHandler(DatePickerMouseAction);
                datePickerView.PreviewMouseUp += new MouseButtonEventHandler(DatePickerMouseAction);
                datePickerView.PreviewMouseWheel += new MouseWheelEventHandler(DatePickerMouseWheelAction);
                datePickerView.Deactivated += new EventHandler(DatePickerDeactivated);
                if (!string.IsNullOrEmpty(defaultDate))
                {
                    string decimalValue = "D2";
                    DateTime result;
                    if (DateTime.TryParse(defaultDate, out result) && result != DateTime.MinValue)
                    {
                        datePickerVM.SelectedYear = result.Year;
                        datePickerVM.SelectedMonth = datePickerVM.Months[result.Month - 1];
                        datePickerVM.SelectedDate = result.Day;
                        if (showTimePicker)
                        {
                            datePickerVM.SelectedHour = result.Hour > 12 ? (result.Hour - 12).ToString(decimalValue) : result.Hour.ToString(decimalValue);
                            datePickerVM.SelectedMinute = result.Minute.ToString(decimalValue);
                            datePickerVM.AMorPM = result.ToString("tt", CultureInfo.InvariantCulture).ToLower() == "am" ? AMorPM.AM : AMorPM.PM;
                        }
                    }
                }
                datePickerView.DataContext = datePickerVM;
                lastActivityTime = DateTime.Now;
                datePickerInactivityTimer.Start();
                datePickerView.ShowDialog();
                if (!string.IsNullOrWhiteSpace(datePickerView.SelectedDate))
                {
                    selectedDate = Convert.ToDateTime(datePickerView.SelectedDate, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error in Launch Calendar: " + ex.Message);
                SendKioskPopupAlerts(ex.Message);
            }
            finally
            {
                KioskPopupAlerts = null;
                eventAttached = false;
                datePickerInactivityTimer.Stop();
                datePickerInactivityTimer.Dispose();
                datePickerInactivityTimer = null;
                datePickerView = null;
            }
            log.LogMethodExit(selectedDate);
            return selectedDate;
        }

        private static void ResizeDatePickerOnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Border border = (Border)datePickerView.Template.FindName("GridBorder", datePickerView);
                border.Width = border.ActualWidth * 1.1;
                border.Height = border.ActualHeight * 1.1;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private static void DatePickerDeactivated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DatePickerView datePickerView = (DatePickerView)sender;
                if (datePickerView != null && eventAttached == false)
                {
                    foreach (System.Windows.Window item in datePickerView.OwnedWindows)
                    {
                        AttachEvents(item);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private static void DatePickerInactivityTicker(object sender, EventArgs e)
        {
            datePickerInactivityTimer.Stop();
            try
            {
                bool restartTimer = true;
                TimeSpan inactivityTimeSpan = DateTime.Now - lastActivityTime;
                if (inactivityTimeSpan.Seconds > inactivityTimeoutSeconds)
                {
                    if (datePickerView != null)
                    {
                        restartTimer = false;
                        datePickerView.Close();
                        datePickerInactivityTimer.Stop();
                    }
                }
                if (restartTimer)
                {
                    datePickerInactivityTimer.Start();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private static void DatePickerMouseAction(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                lastActivityTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private static void DatePickerKeyAction(object sender, System.Windows.Input.KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                lastActivityTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private static void DatePickerMouseWheelAction(object sender, MouseWheelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                lastActivityTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// AttractionIsOfTypeCardSale
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static bool AttractionIsOfTypeCardSale(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            bool cardSale = false;
            ProductTypeContainerDTOCollection collection = ProductTypeContainerList.GetProductTypeContainerDTOCollection(executionContext.SiteId);
            if (collection.ProductTypeContainerDTOList != null && collection.ProductTypeContainerDTOList.Any())
            {
                cardSale = collection.ProductTypeContainerDTOList.Exists(pt => pt.ProductType == ProductTypeValues.ATTRACTION && pt.CardSale);
            }
            log.LogMethodExit(cardSale);
            return cardSale;
        }

        private static void AttachEvents(System.Windows.Window wpfWindow)
        {
            log.LogMethodEntry(wpfWindow.Name);
            try
            {
                if (eventAttached == false)
                {
                    wpfWindow.PreviewKeyDown += DatePickerKeyAction;
                    wpfWindow.PreviewKeyUp += DatePickerKeyAction;
                    wpfWindow.KeyDown += new System.Windows.Input.KeyEventHandler(DatePickerKeyAction);
                    wpfWindow.KeyUp += new System.Windows.Input.KeyEventHandler(DatePickerKeyAction);
                    wpfWindow.MouseDown += new MouseButtonEventHandler(DatePickerMouseAction);
                    wpfWindow.MouseUp += new MouseButtonEventHandler(DatePickerMouseAction);
                    wpfWindow.PreviewMouseDown += new MouseButtonEventHandler(DatePickerMouseAction);
                    wpfWindow.PreviewMouseUp += new MouseButtonEventHandler(DatePickerMouseAction);
                    wpfWindow.PreviewMouseWheel += new MouseWheelEventHandler(DatePickerMouseWheelAction);
                    eventAttached = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            lastActivityTime = DateTime.Now;
            log.LogMethodExit();
        }

        private static void SendKioskPopupAlerts(string message)
        {
            log.LogMethodEntry(message);
            if (KioskPopupAlerts != null)
            {
                KioskPopupAlerts(message);
            }
            else
            {
                log.Info("KioskPopupAlerts is not defined. Hence no message sent back");
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Get UI Date Time Format
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static string GetUIDateTimeFormat(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            string datetimeFormatUI = DEFAULT_DATE_TIME_FOR_UI;
            LookupsContainerDTO containerDTO = LookupsViewContainerList.GetLookupsContainerDTO(executionContext.SiteId, KIOSK_SETUP_LOOKUP);
            if (containerDTO != null && containerDTO.LookupValuesContainerDTOList != null)
            {
                LookupValuesContainerDTO lookupValue = containerDTO.LookupValuesContainerDTOList.Find(lv => lv.LookupValue == DATE_TIME_FORMAT_FOR_UI);
                if (lookupValue != null && string.IsNullOrWhiteSpace(lookupValue.Description) == false)
                {
                    datetimeFormatUI = lookupValue.Description;
                }
            }
            log.LogMethodExit(datetimeFormatUI);
            return datetimeFormatUI;
        }
        /// <summary>
        /// GetSlotDate
        /// </summary> 
        public static DateTime GetSlotDate(ExecutionContext executionContext, DateTime scheduleFromDate)
        {
            log.LogMethodEntry(executionContext);
            DateTime slotDateTime = scheduleFromDate.Date;
            int businessStartHour = 6;
            try
            {
                businessStartHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME", 6);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                businessStartHour = 6;
            }
            DateTime refStartTime = scheduleFromDate.Date.AddHours(businessStartHour);
            if (scheduleFromDate < refStartTime)
            {
                slotDateTime = scheduleFromDate.AddDays(-1).Date;
            }
            log.LogMethodExit(slotDateTime);
            return slotDateTime;
        }
        /// <summary>
        /// CopyDefaultKioskStyles
        /// </summary>
        public static void CopyDefaultKioskStyles()
        {
            log.LogMethodEntry();
            try
            {
                string rootFolder = Application.StartupPath;
                string fskBaseFolder = rootFolder + @"\Kiosk_Theme1\Styles\";
                string fskBaseBackupFolder = rootFolder + @"\Kiosk_Theme1\Styles\Bkp\";
                string fskStyleFileName = rootFolder + @"\Kiosk_Theme1\Styles\KioskStyles";
                string fskStyleFileNameExtension = @".xaml";
                string fskCustomThemeFolder = rootFolder + @"\Media\Images\Theme";
                string destinationFolder =  @"\Styles\";
                string destinationXmlName = @"KioskStyles";
                string destinationXmlExtebsion = @".xaml";
                if (Directory.Exists(fskBaseFolder))
                {
                    string[] styleFiles = Directory.GetFiles(fskBaseFolder, "*.xaml", SearchOption.TopDirectoryOnly);
                    if (styleFiles != null && styleFiles.Length > 0)
                    {
                        for (int i = 1; i < 6; i++)
                        {
                            string custThemeFolder = fskCustomThemeFolder + i;

                            string styleFileName = fskStyleFileName + i + fskStyleFileNameExtension;
                            if (File.Exists(styleFileName))
                            {
                                if (Directory.Exists(custThemeFolder))
                                {
                                    string destinationFileName = custThemeFolder + destinationFolder + destinationXmlName + destinationXmlExtebsion;
                                    if (Directory.Exists(custThemeFolder + destinationFolder) == false)
                                    {
                                        Directory.CreateDirectory(custThemeFolder + destinationFolder);
                                    }
                                    if (File.Exists(destinationFileName) == false)
                                    {
                                        File.Copy(styleFileName, destinationFileName);
                                    }
                                }
                                string nameAppender = ServerDateTime.Now.Day.ToString() + ServerDateTime.Now.Month.ToString() + ServerDateTime.Now.Year.ToString() + "_";
                                string backupDestinationFileName = fskBaseBackupFolder + nameAppender + destinationXmlName + i + destinationXmlExtebsion;
                                if (Directory.Exists(fskBaseBackupFolder) == false)
                                {
                                    Directory.CreateDirectory(fskBaseBackupFolder);
                                }
                                File.Copy(styleFileName, backupDestinationFileName, true);
                                File.Delete(styleFileName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in CopyDefaultKioskStyles:" + ex.Message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// ValidateDispenserCardNumber
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="scannedTagNumber"></param>
        /// <returns></returns>
        public static string ValidateDispenserCardNumber(ExecutionContext executionContext, string scannedTagNumber)
        {
            log.LogMethodEntry(executionContext, scannedTagNumber);
            string retCardNumber = string.Empty;
            if (string.IsNullOrWhiteSpace(scannedTagNumber) == false)
            {
                TagNumber tagNumber;
                TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
                //if (tagNumberParser.IsTagDecryptApplicable(encryptedTagDevice, cardNumber.Length))
                //{
                //    string decryptedTagNumber = string.Empty;
                //    try
                //    {
                //        decryptedTagNumber = tagNumberParser.GetDecryptedTagData(encryptedTagDevice, cardNumber);
                //    }
                //    catch (Exception ex)
                //    {
                //        log.LogVariableState("Decrypted Tag Number result: ", ex); 
                //        return string.Empty; 
                //    }
                //    try
                //    {
                //        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, executionContext.SiteId);
                //    }
                //    catch (ValidationException ex)
                //    {
                //        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                //        return string.Empty;
                //    }
                //    catch (Exception ex)
                //    {
                //        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                //        return string.Empty;
                //    }
                //}
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return string.Empty;
                }
                retCardNumber = tagNumber.Value;
            }
            log.LogMethodExit(retCardNumber);
            return retCardNumber;
        }
        /// <summary>
        /// CheckForCardExpiry
        /// </summary>
        /// <param name="accountDTO"></param>
        public static void CheckForCardExpiry(AccountDTO accountDTO, PopupAlerts popupAlerts)
        {
            log.LogMethodEntry((accountDTO != null ? GetMaskedCardNumber(accountDTO.TagNumber) : ""));
            try
            {
                KioskPopupAlerts = popupAlerts;
                if (accountDTO != null)
                {
                    if (accountDTO.ExpiryDate != null)
                    {
                        DateTime cardExpiryDate = (DateTime)accountDTO.ExpiryDate;
                        DateTime currentDateTime = ServerDateTime.Now;
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Card &1 is an expired card", GetMaskedCardNumber(accountDTO.TagNumber));
                        if (accountDTO.ValidFlag && accountDTO.RefundFlag == false)
                        {
                            string ruleName = ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "CARD_EXPIRY_RULE");
                            int gradePeriod = ParafaitDefaultContainerList.GetParafaitDefault<int>(KioskStatic.Utilities.ExecutionContext, "CARD_EXPIRY_GRACE_PERIOD", 0);
                            if (string.IsNullOrWhiteSpace(ruleName) == false)
                            {
                                if ((ruleName == "ISSUEDATE" || ruleName == "LASTACTIVITY"))
                                {
                                    if (cardExpiryDate < currentDateTime && cardExpiryDate.AddDays(gradePeriod) > currentDateTime)
                                    {
                                        string msgOne = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Card &1 is expired and under grace period. Please contact our staff to reactivate.", GetMaskedCardNumber(accountDTO.TagNumber));
                                        SendKioskPopupAlerts(msgOne);
                                    }
                                    else if (cardExpiryDate.AddDays(gradePeriod) < currentDateTime)
                                    {
                                        throw new ValidationException(msg);
                                    }
                                }
                                else if (cardExpiryDate < currentDateTime)
                                {
                                    throw new ValidationException(msg);
                                }
                            }
                            else if (cardExpiryDate < currentDateTime)
                            {
                                throw new ValidationException(msg);
                            }
                        }
                        else if (cardExpiryDate < currentDateTime)
                        {
                            throw new ValidationException(msg);
                        }
                    }
                }
            }
            finally
            {
                KioskPopupAlerts = null;
            }
        }
        /// <summary>
        /// CanCloseTheKiosk
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static bool CanCloseTheKiosk(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            bool retValue = true;
            DateTime oneHourTime = ServerDateTime.Now.AddHours(-1);
            KioskActivityLogListBL kioskActivityLogListBL = new KioskActivityLogListBL(executionContext);
            List<KeyValuePair<KioskActivityLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<KioskActivityLogDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<KioskActivityLogDTO.SearchByParameters, string>(KioskActivityLogDTO.SearchByParameters.KIOSK_NAME, executionContext.POSMachineName));
            searchParameters.Add(new KeyValuePair<KioskActivityLogDTO.SearchByParameters, string>(KioskActivityLogDTO.SearchByParameters.KIOSK_ID, executionContext.MachineId.ToString()));
            searchParameters.Add(new KeyValuePair<KioskActivityLogDTO.SearchByParameters, string>(KioskActivityLogDTO.SearchByParameters.ACTIVITY, KioskTransaction.PAYMENTMODE_ERRROR_EXITKIOSK_MSG));
            searchParameters.Add(new KeyValuePair<KioskActivityLogDTO.SearchByParameters, string>(KioskActivityLogDTO.SearchByParameters.FROM_TIME_STAMP, oneHourTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            List<KioskActivityLogDTO> kioskActivityLogDTOList = kioskActivityLogListBL.GetKioskActivityLogList(searchParameters);
            if (kioskActivityLogDTOList != null && kioskActivityLogDTOList.Count >= 3)
            {
                retValue = false;
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        public static string GetProductName(int productId)
        {
            log.LogMethodEntry(productId);
            ProductsContainerDTO selectedProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, productId);
            string productName = ObjectTranslationContainerList.GetObjectTranslation(KioskStatic.Utilities.ExecutionContext.SiteId, KioskStatic.Utilities.ExecutionContext.LanguageId, "PRODUCTS", "PRODUCT_NAME", selectedProductsContainerDTO.Guid, selectedProductsContainerDTO.ProductName);
            log.LogMethodExit(productName);
            return productName;
        }

        public static List<Semnox.Parafait.Transaction.Transaction.TransactionLine> GetChildLinesWithCards(ExecutionContext executionContext, Semnox.Parafait.Transaction.Transaction.TransactionLine trxL,
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> selectedTrxLines)
        {
            log.LogMethodEntry(executionContext, trxL, "selectedTrxLines");
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> cardChildrenList = new List<Semnox.Parafait.Transaction.Transaction.TransactionLine>();
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> allChildren = selectedTrxLines.Where(tl => tl.LineValid && tl.ParentLine == trxL).ToList();
            if (allChildren != null && allChildren.Any())
            {
                foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine childLine in allChildren)
                {
                    if (childLine.card != null && string.IsNullOrWhiteSpace(childLine.CardNumber) == false)
                    {
                        cardChildrenList.Add(childLine);
                    }
                    List<Semnox.Parafait.Transaction.Transaction.TransactionLine> myCardChildList = GetChildLinesWithCards(executionContext, childLine, selectedTrxLines);
                    if (myCardChildList != null && myCardChildList.Any())
                    {
                        cardChildrenList.AddRange(myCardChildList);
                    }
                }
            }
            log.LogMethodExit(cardChildrenList);
            return cardChildrenList;
        }

        public static List<Semnox.Parafait.Transaction.Transaction.TransactionLine> GetRelatedComboLines(ExecutionContext executionContext, KioskTransaction kioskTransaction, Semnox.Parafait.Transaction.Transaction.TransactionLine trxL, List<string> cardNumberList)
        {
            log.LogMethodEntry(executionContext, "kioskTransaction", trxL, cardNumberList);
            List <Semnox.Parafait.Transaction.Transaction.TransactionLine> relatedComboListList = new List<Semnox.Parafait.Transaction.Transaction.TransactionLine>();
            if (cardNumberList != null && cardNumberList.Any())
            {
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> allTrxLines = kioskTransaction.GetActiveTransactionLines;
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> combList = allTrxLines.Where(tl => tl.LineValid && tl != trxL
                           && tl.ProductTypeCode == ProductTypeValues.COMBO).ToList();
                if (combList != null && combList.Any())
                {
                    foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine comboLine in combList)
                    {
                        List<Semnox.Parafait.Transaction.Transaction.TransactionLine> childLines = GetAllChildLines(comboLine, allTrxLines);
                        bool hasRelatedCards = false;
                        if (childLines != null && childLines.Any())
                        {
                            hasRelatedCards = childLines.Exists(tl => tl.LineValid && tl.card != null && string.IsNullOrWhiteSpace(tl.CardNumber) == false
                                                     && cardNumberList.Contains(tl.CardNumber) == true);
                            if (hasRelatedCards)
                            {
                                relatedComboListList.Add(comboLine);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(relatedComboListList);
            return relatedComboListList;
        }

        public static List<Semnox.Parafait.Transaction.Transaction.TransactionLine> GetAllChildLines(Semnox.Parafait.Transaction.Transaction.TransactionLine tLine, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> allTrxLines)
        {
            log.LogMethodEntry(tLine, "allTrxLines");
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> childrenList = new List<Semnox.Parafait.Transaction.Transaction.TransactionLine>();
            if (allTrxLines != null && allTrxLines.Any())
            {
                if (string.IsNullOrWhiteSpace(tLine.CardNumber) == false && tLine.card != null)
                {
                    int lineIndex = allTrxLines.IndexOf(tLine);
                    if (lineIndex > 0)
                    {
                        Semnox.Parafait.Transaction.Transaction.TransactionLine previousLine = allTrxLines[lineIndex - 1];
                        if (previousLine != null && previousLine.ProductTypeCode == ProductTypeValues.CARDDEPOSIT
                            && string.IsNullOrWhiteSpace(previousLine.CardNumber) == false && previousLine.CardNumber == tLine.CardNumber)
                        {
                            childrenList.Add(previousLine);
                        }
                    }
                }
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> myChildList = allTrxLines.Where(tl => tl.LineValid && tl.ParentLine == tLine).ToList();
                if (myChildList != null && myChildList.Any())
                {
                    foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine childLine in myChildList)
                    {
                        childrenList.Add(childLine);
                        List<Semnox.Parafait.Transaction.Transaction.TransactionLine> subChildList = GetAllChildLines(childLine, allTrxLines);
                        if (subChildList != null && subChildList.Any())
                        {
                            childrenList.AddRange(subChildList);
                        }
                    }
                }

            }
            log.LogMethodExit(childrenList);
            return childrenList;
        }
        /// <summary> 
        /// GetFormatedDateValue - to be used for DOB or anniversay field only
        /// </summary> 
        /// <returns></returns>
        public static DateTime GetFormatedDateValue(DateTime inputDate)
        {
            // to be used for DOB or anniversay field only
            log.LogMethodEntry(inputDate);
            DateTime dateofBirthValue = inputDate;
            bool ignoreYear = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR");
            if (ignoreYear 
                || (ignoreYear == false && KioskStatic.enableYearSelection == false))
            {
                dateofBirthValue = new DateTime(1904, inputDate.Month, inputDate.Day);
            }
            if (KioskStatic.enableMonthSelection == false)
            {
                dateofBirthValue = new DateTime(dateofBirthValue.Year, 01, dateofBirthValue.Day);
            }
            if (KioskStatic.enableDaySelection == false)
            {
                dateofBirthValue = new DateTime(dateofBirthValue.Year, dateofBirthValue.Month, 01);
            }
            log.LogMethodExit(dateofBirthValue);
            return dateofBirthValue;
        }
    }
}
