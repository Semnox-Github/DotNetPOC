/********************************************************************************************
 * Project Name - CardTypeToMembership
 * Description  - Created for card type migration class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By       Remarks          
 *********************************************************************************************
 *2.32.0      25-June-2018   Guru S A          Created 
 *2.70.0      08-Aug-2019    Jagan Mohana      Moved the class file Parafait.Tools.CardTypeMigration to Accounts
 *2.120.1     09-Jun-2021   Deeksha                 Modified as part of AWS concurrent programs enhancement.   
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.Customer.Accounts
{
    public class CardTypeToMembership
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        Utilities utilities;
        DataTable cardTypeDT;
        DataTable nonCardTypeDT;
        DataTable customerSetup;
        int stateId;
        int countryId;
        // string addressType;
        string imageFolderPath;
        string passPhrase;
        int siteId;
        string loginId;

        public CardTypeToMembership(ExecutionContext executionContext, Utilities utilities)
        {
            log.LogMethodEntry(executionContext, utilities);
            this.executionContext = executionContext;
            this.utilities = utilities;
            log.LogMethodExit();
        }

        public bool DataAvailableForMigration()
        {
            log.LogMethodEntry();
            bool retValue = false;
            this.siteId = executionContext.GetSiteId();
            this.loginId = executionContext.GetUserId();
            try
            {
                cardTypeDT = utilities.executeDataTable(@"SELECT ct.*, m.VIP, mr.QualifyingPoints, mr.QualificationWindow, mr.UnitOfQualificationWindow, mr.RetentionPoints, mr.RetentionWindow, mr.UnitOfRetentionWindow,
                                                                 ISNULL((select top 1 'Y' from MembershipRewards mr
                                                                   where mr.membershipId = m.membershipId 
                                                                     and RewardFunction = 'DTAVP'
                                                                     and IsActive= 1),'N') as createDailyCardBalanceEntry
                                                            FROM cardType ct ,
                                                                 membership m,
                                                                 membershiprule mr
                                                           WHERE ct.newMembershipId = m.membershipId 
                                                             and m.membershipRuleId = mr.membershipRuleId
                                                             and m.isactive = 1
                                                             and ISNULL(ct.cardTypeMigrated,0) = 0
                                                             AND exists (SELECT 1 from cards cc where cc.CardTypeId = ct.cardtypeId and cc.valid_flag='Y')
                                                             AND (ct.site_id = @siteId or @siteId = -1)
                                                        ORDER BY ct.MigrationOrder, ct.AutomaticApply, ct.BaseCardTypeId", new SqlParameter("@siteId", this.siteId));

                nonCardTypeDT = utilities.executeDataTable(@"SELECT ct.*
                                                               FROM cardType ct 
                                                              WHERE ct.cardType = 'NONCARDTYPEMIGRATIONENTRY'
                                                                and ISNULL(ct.cardTypeMigrated,0) = 0 
                                                                AND (ct.site_id = @siteId or @siteId = -1) ", new SqlParameter("@siteId", this.siteId));

                if (cardTypeDT.Rows.Count > 0 || nonCardTypeDT.Rows.Count > 0)
                {
                    string tablesFound = "N";
                    try
                    {
                        tablesFound = utilities.executeScalar(@"SELECT top 1 'Y' from membership m, membershiprule mr 
                                                                where m.isactive=1 and m.membershipRuleId = mr.membershipruleId and mr.isactive=1
                                                                   AND (m.site_id = @siteId or @siteId = -1) ", new SqlParameter("@siteId", this.siteId)).ToString();
                        if (tablesFound == "Y")
                        {
                            customerSetup = utilities.executeDataTable(@"SELECT default_value_name, default_value
                                                                           from parafait_defaults
                                                                          Where default_value in ('M') OR default_value_name in ('CUSTOMER_EMAIL_OR_PHONE_MANDATORY','CUSTOMER_USERNAME_LENGTH','CUSTOMER_NAME_VALIDATION','ENABLE_CUSTOMER_BACKWARD_COMPATIBILITY','CUSTOMER_PHONE_NUMBER_WIDTH')
                                                                            and default_value_name in  ('CUSTOMER_EMAIL_OR_PHONE_MANDATORY', 'CUSTOMER_USERNAME_LENGTH','CUSTOMER_NAME_VALIDATION','ENABLE_CUSTOMER_BACKWARD_COMPATIBILITY','CUSTOMER_PHONE_NUMBER_WIDTH','ADDRESS1', 'ADDRESS2', 'ADDRESS3', 'CITY', 'STATE', 'COUNTRY', 'PIN', 'EMAIL', 'BIRTH_DATE', 'GENDER', 'ANNIVERSARY', 'CONTACT_PHONE', 'CONTACT_PHONE2', 'NOTES', 'COMPANY', 'DESIGNATION', 'UNIQUE_ID', 'USERNAME', 'FBUSERID', 'FBACCESSTOKEN', 'TWACCESSTOKEN', 'TWACCESSSECRET', 'RIGHTHANDED', 'TEAMUSER',  'LAST_NAME', 'CUSTOMER_PHOTO', 'TITLE', 'CUSTOMER_NAME', 'WECHAT_ACCESS_TOKEN', 'CHANNEL', 'TAXCODE', 'ADDRESS_TYPE', 'MIDDLE_NAME')
                                                                            and (site_id = @siteId or @siteId = -1) ", new SqlParameter("@siteId", this.siteId));
                            if (customerSetup.Rows.Count == 0)
                            {
                                throw new Exception("Please check customer setup");
                            }
                            this.stateId = Convert.ToInt32(utilities.executeScalar("SELECT ISNULL((select top 1 StateId from State where (site_id = @siteId or @siteId = -1) and CountryId = (SELECT top 1 countryId from Country where (site_id = @siteId or @siteId = -1) )),-1) ", new SqlParameter("@siteId", this.siteId)));
                            this.countryId = Convert.ToInt32(utilities.executeScalar("SELECT ISNULL((select top 1 countryId from Country  where (site_id = @siteId or @siteId = -1)),-1) ", new SqlParameter("@siteId", this.siteId)));
                            // this.addressType = utilities.executeScalar("select top 1 id from AddressType WHERE Name = 'HOME'").ToString();
                            try
                            {
                                this.imageFolderPath = utilities.executeScalar("select top 1 default_value from parafait_defaults where default_value_name = 'IMAGE_DIRECTORY' and (site_id = @siteId or @siteId = -1)", new SqlParameter("@siteId", this.siteId)).ToString();
                                byte[] imageData;
                                using (var memoryStream = new MemoryStream())
                                {
                                    Properties.Resources.Semnox_Small.Save(memoryStream, ImageFormat.Bmp);
                                    imageData = memoryStream.ToArray();
                                }
                                SaveImageFile(this.imageFolderPath + "\\Semnox_CardTypeMigration.bmp", imageData);
                            }
                            catch (Exception ex) { log.Error(ex); throw new Exception("Please check image save setup"); }
                            this.passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
                            retValue = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        tablesFound = "N";
                        log.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        public string DoMembershipMigration()
        {
            log.LogMethodEntry();

            this.siteId = executionContext.GetSiteId();
            this.loginId = executionContext.GetUserId();
            StringBuilder outString = new StringBuilder(MessageContainerList.GetMessage(executionContext, 1568));
            try
            {
                DisableEnableDBTriggers(false);
                if (cardTypeDT.Rows.Count > 0)
                {
                    foreach (DataRow cardTypeRow in cardTypeDT.Rows)
                    {
                        //if 
                        int newMembershipId = -1;
                        int discountId = -1;
                        int vipMembership = 0;
                        int cardTypeId = Convert.ToInt32(cardTypeRow["cardTypeId"]);
                        DateTime newMembershipEffeciveFromDate = DateTime.Now;
                        DateTime newMembershipEffeciveToDate = DateTime.Now;
                        bool createDailyCardBalanceEntry = false;
                        double loyaltyPointConversionRatio = 0;
                        bool canRedeemloyaltyPoints = false;
                        int loyaltyPointsCalculationOption = -1;
                        int loyaltyPointsCalculationDuration = -1;

                        try
                        {
                            newMembershipId = Convert.ToInt32(cardTypeRow["newMembershipId"]);

                            loyaltyPointConversionRatio = Convert.ToDouble(cardTypeRow["LoyaltyPointConvRatio"]);
                            canRedeemloyaltyPoints = Convert.ToBoolean(cardTypeRow["RedeemLoyaltyPoints"]);
                            loyaltyPointsCalculationOption = Convert.ToInt32(cardTypeRow["ExistingTriggerSource"]);
                            loyaltyPointsCalculationDuration = Convert.ToInt32(cardTypeRow["QualifyingDuration"]);
                        }
                        catch (Exception ex)
                        {
                            newMembershipId = -1;
                            //Console.WriteLine("Invalid membership id for CardType {0} ", cardTypeRow["cardTypeId"].ToString());
                            outString.Append(MessageContainerList.GetMessage(executionContext, 1571, cardTypeRow["cardTypeId"].ToString()) + Environment.NewLine);
                            log.Error(MessageContainerList.GetMessage(executionContext, 1571, cardTypeRow["cardTypeId"].ToString()));
                            log.Error(ex);
                        }
                        try
                        {
                            discountId = Convert.ToInt32(cardTypeRow["discount_id"]);
                        }
                        catch
                        {
                            discountId = -1;
                        }
                        try
                        {
                            vipMembership = Convert.ToInt32(cardTypeRow["VIP"]);
                        }
                        catch
                        {
                            vipMembership = 0;
                            newMembershipId = -1;
                            // Console.WriteLine("Invalid membership setup for CardType {0} ", cardTypeRow["cardTypeId"].ToString());
                            outString.Append(MessageContainerList.GetMessage(executionContext, 1572, cardTypeRow["cardTypeId"].ToString()) + Environment.NewLine);
                        }


                        try
                        { createDailyCardBalanceEntry = (cardTypeRow["createDailyCardBalanceEntry"].ToString() == "Y" ? true : false); }
                        catch
                        {
                            createDailyCardBalanceEntry = false;
                            newMembershipId = -1;
                            outString.Append(MessageContainerList.GetMessage(executionContext, 1573, cardTypeRow["cardTypeId"].ToString()) + Environment.NewLine);
                        }
                        try
                        {
                            //RetentionPoints, mr.RetentionWindow, mr.UnitOfRetentionWindow
                            int retentionWindow = Convert.ToInt32(cardTypeRow["RetentionWindow"]);
                            string retentionWindowUnit = cardTypeRow["UnitOfRetentionWindow"].ToString();
                            switch (retentionWindowUnit)
                            {
                                case "D":
                                    newMembershipEffeciveToDate = DateTime.Now.AddDays(retentionWindow);
                                    break;
                                case "M":
                                    newMembershipEffeciveToDate = DateTime.Now.AddMonths(retentionWindow);
                                    break;
                                case "Y":
                                    newMembershipEffeciveToDate = DateTime.Now.AddYears(retentionWindow);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            newMembershipId = -1;
                            //Console.WriteLine("Invalid membership setup for CardType {0} ", cardTypeRow["cardTypeId"].ToString());
                            outString.Append(MessageContainerList.GetMessage(executionContext, 1574, cardTypeRow["cardTypeId"].ToString()) + Environment.NewLine);
                            log.Error(MessageContainerList.GetMessage(executionContext, 1574, cardTypeRow["cardTypeId"].ToString()), ex);
                        }
                        if (newMembershipId != -1)
                        {

                            try
                            {
                                SqlCommand sqlCmd = utilities.getCommand(utilities.createConnection().BeginTransaction());
                                SqlTransaction sqlCmdTrx = sqlCmd.Transaction;
                                SqlConnection DBConnection = sqlCmd.Connection;
                                try
                                {
                                    outString.Append(MessageContainerList.GetMessage(executionContext, 1575, cardTypeRow["cardTypeId"].ToString()) + Environment.NewLine);
                                    //Products table: For each card type id entry set corresponding membership id field with mapped membership id
                                    SqlParameter[] sqlParameterList = new SqlParameter[5];
                                    sqlParameterList[0] = new SqlParameter("@cardTypeId", cardTypeId);
                                    sqlParameterList[1] = new SqlParameter("@newMembershipId", newMembershipId);
                                    sqlParameterList[2] = new SqlParameter("@discountId", discountId);
                                    sqlParameterList[3] = new SqlParameter("@siteId", this.siteId);
                                    sqlParameterList[4] = new SqlParameter("@loginId", this.loginId);
                                    sqlCmd.Parameters.AddRange(sqlParameterList);

                                    sqlCmd.CommandText = @"update products
                                                               set membershipId = @newMembershipId,
                                                                   cardTypeId = null,
                                                                   last_updated_date = getdate(),
                                                                   last_updated_user = @loginId
                                                             where CardTypeId = @cardTypeId 
                                                               AND (site_id = @siteId or @siteId = -1) ";
                                    sqlCmd.ExecuteNonQuery();

                                    //ProductGames:  No updates to ProductGames table
                                    //CardTypeRule:  For each card type id entry set corresponding membership id field with mapped membership id 
                                    sqlCmd.CommandText = @"update cardtyperule
                                                               set membershipId = @newMembershipId,
                                                                   cardTypeId = null,
                                                                   LastUpdatedDate = getdate(),
                                                                   LastUpdatedBy = @loginId
                                                            where CardTypeId = @cardTypeId 
                                                              AND (site_id = @siteId or @siteId = -1) ";
                                    sqlCmd.ExecuteNonQuery();

                                    // PromotionRule:  For each card type id entry set corresponding membership id field with mapped membership id 
                                    sqlCmd.CommandText = @"update PromotionRule
                                                               set membershipId = @newMembershipId,
                                                                   cardTypeId = null
                                                            where CardTypeId = @cardTypeId 
                                                             AND (site_id = @siteId or @siteId = -1) ";

                                    sqlCmd.ExecuteNonQuery();

                                    //LoyaltyRule:  For each card type id entry set corresponding membership id field with mapped membership id 
                                    sqlCmd.CommandText = @"update LoyaltyRule
                                                               set membershipId = @newMembershipId,
                                                                   cardTypeId = null,
                                                                   LastUpdatedDate = getdate(),
                                                                   LastUpdatedBy = @loginId
                                                            where CardTypeId = @cardTypeId 
                                                              AND (site_id = @siteId or @siteId = -1) ";

                                    sqlCmd.ExecuteNonQuery();

                                    //CardGames - for the active entries with cardTypeId, set membership --and corresponding membershipRewardId 
                                    sqlCmd.CommandText = @"update CardGames
                                                               set membershipId = @newMembershipId,
                                                                   cardTypeId = null,
                                                                   Last_update_date = getdate(), 
                                                                   LastUpdatedBy = @loginId
                                                            where CardTypeId = @cardTypeId 
                                                              AND (site_id = @siteId or @siteId = -1) ";

                                    sqlCmd.ExecuteNonQuery();
                                    //CardCreditPlus - no action
                                    //CardDiscounts - cardTypeId mentioned and DiscountID is same as cardType discount then set expire with membershipid as Yes and set membershipRewardId from new membership
                                    sqlCmd.CommandText = @"update CardDiscounts
                                                               set membershipId = @newMembershipId,
                                                                   cardTypeId = null,
                                                                   expireWithMembership =  (CASE WHEN @discountId = discount_id AND  @discountId > -1 THEN
                                                                                                      'Y'
                                                                                                 ELSE 
                                                                                                      expireWithMembership
                                                                                                 END),
                                                                  last_updated_date = getdate(),
                                                                  Last_updated_user = @loginId,
                                                                  membershiprewardsId = (select top 1 mrwd.membershiprewardsid
                                                                                           from membershiprewards mrwd, ProductDiscounts pd
                                                                                          where mrwd.membershipId = @newMembershipId 
                                                                                            and mrwd.rewardproductId = pd.product_id
                                                                                            and pd.discount_id is not null
                                                                                            and pd.isactive = 'Y')
                                                            where CardTypeId = @cardTypeId 
                                                              AND (site_id = @siteId or @siteId = -1) ";

                                    sqlCmd.ExecuteNonQuery();

                                    sqlCmdTrx.Commit();
                                    DBConnection.Close();
                                    DBConnection.Dispose();
                                    sqlCmd.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    outString.Append(MessageContainerList.GetMessage(executionContext, 1576, cardTypeId.ToString()) + "  " + ex.Message + Environment.NewLine);
                                    log.Error("Error while processing cardTypeId: " + cardTypeId.ToString(), ex);
                                    sqlCmdTrx.Rollback();
                                    DBConnection.Close();
                                    DBConnection.Dispose();
                                    sqlCmd.Dispose();
                                    UpdateCardTypeRecord(cardTypeId, false, ex.Message, this.siteId, null);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1576, cardTypeId.ToString()));
                                }
                                // Customer migration:
                                //Fetch customers with active cards
                                //sqlCmd.CommandText = @"select cu.*, 
                                //                             (select Max(mp.effectiveDate)
                                //                                 from MembershipProgression mp, cards cc
                                //                                 where cc.customer_id = cu.customer_id
                                //                                     AND (mp.site_id = @siteId or @siteId = -1) 
                                //                                     AND (cc.site_id = @siteId or @siteId = -1) 
                                //                                     and cc.valid_flag = 'Y'
                                //                                     and cc.cardTypeId = @cardTypeId
                                //                                     and cc.card_id = mp.CardId) latestEffectiveDate
                                //                         from customers cu
                                //                         where EXISTS (SELECT 1 
                                //                                         from cards cc
                                //                                         WHERE cc.customer_id = cu.customer_id
                                //                                         AND (cc.site_id = @siteId or @siteId = -1) 
                                //                                         and cc.valid_flag = 'Y'
                                //                                         and cc.cardTypeId = @cardTypeId) 
                                //                             and cu.membershipId is null 
                                //                             AND (cu.site_id = @siteId or @siteId = -1) ";

                                // DataTable dtCardTypeCustomers = new DataTable();
                                // SqlDataAdapter daCTC = new SqlDataAdapter(sqlCmd);
                                // daCTC.Fill(dtCardTypeCustomers);
                                List<SqlParameter> sqlParamCTC = new List<SqlParameter>();
                                sqlParamCTC.Add(new SqlParameter("@cardTypeIdCTC", cardTypeId));
                                sqlParamCTC.Add(new SqlParameter("@siteIdCTC", this.siteId));
                                DataTable dtCardTypeCustomers = utilities.executeDataTable(@"select cu.*, 
                                                                                                     (select Max(mp.effectiveDate)
                                                                                                         from MembershipProgression mp, cards cc
                                                                                                         where cc.customer_id = cu.customer_id
                                                                                                             AND (mp.site_id = @siteIdCTC or @siteIdCTC = -1) 
                                                                                                             AND (cc.site_id = @siteIdCTC or @siteIdCTC = -1) 
                                                                                                             and cc.valid_flag = 'Y'
                                                                                                             and cc.cardTypeId = @cardTypeIdCTC
                                                                                                             and cc.card_id = mp.CardId) latestEffectiveDate
                                                                                                 from customers cu
                                                                                                 where EXISTS (SELECT 1 
                                                                                                                 from cards cc
                                                                                                                 WHERE cc.customer_id = cu.customer_id
                                                                                                                 AND (cc.site_id = @siteIdCTC or @siteIdCTC = -1) 
                                                                                                                 and cc.valid_flag = 'Y'
                                                                                                                 and cc.technician_card = 'N' 
                                                                                                                 and cc.cardTypeId = @cardTypeIdCTC) 
                                                                                                     and cu.membershipId is null 
                                                                                                     AND (cu.site_id = @siteIdCTC or @siteIdCTC = -1) ", sqlParamCTC.ToArray());


                                if (dtCardTypeCustomers.Rows.Count > 0)
                                {
                                    //sqlCmd.CommandText = @"select cc.* , 
                                    //                        (SELECT top 1 'Y' 
                                    //                            from parentchildcards pcc
                                    //                            where pcc.ParentCardId = cc.card_id
                                    //                            AND (pcc.site_id = @siteId or @siteId = -1) 
                                    //                            AND pcc.ActiveFlag = '1') as parentCard,
                                    //                        ( isnull(cc.loyalty_points, 0) + ISNULL((select sum(ISNULL(cp.CreditPlus,0)) 
                                    //                                                            from CardCreditPlus cp
                                    //                                                            where cp.card_id = cc.card_id
                                    //                                                                AND (cp.site_id = @siteId or @siteId = -1) 
                                    //                                                                and cp.CreditPlusType = 'L' 
                                    //                                                                and creationDate > getdate()-365),0)) loyaltyPoints
                                    //                    from customers cu, cards cc
                                    //                    where cc.customer_id = cu.customer_id
                                    //                    AND (cu.site_id = @siteId or @siteId = -1) 
                                    //                    AND (cc.site_id = @siteId or @siteId = -1) 
                                    //                    and cc.valid_flag = 'Y'
                                    //                    and cc.cardTypeId = @cardTypeId 
                                    //                    and cu.membershipId is null
                                    //                order by cc.customer_id, cc.card_id";

                                    //DataTable dtCardTypeCards = new DataTable();
                                    List<SqlParameter> sqlParamCTC2 = new List<SqlParameter>();
                                    sqlParamCTC2.Add(new SqlParameter("@cardTypeIdCTC", cardTypeId));
                                    sqlParamCTC2.Add(new SqlParameter("@siteIdCTC", this.siteId));
                                    //SqlDataAdapter daCTCC = new SqlDataAdapter(sqlCmd);
                                    //daCTCC.Fill(dtCardTypeCards);
                                    DataTable dtCardTypeCards = utilities.executeDataTable(@"select cc.* , 
                                                                        (SELECT top 1 'Y' 
                                                                            from parentchildcards pcc
                                                                            where pcc.ParentCardId = cc.card_id
                                                                            AND (pcc.site_id = @siteIdCTC or @siteIdCTC = -1) 
                                                                            AND pcc.ActiveFlag = '1') as parentCard,
                                                                        ( isnull(cc.loyalty_points, 0) + ISNULL((select sum(ISNULL(cp.CreditPlus,0)) 
                                                                                                            from CardCreditPlus cp
                                                                                                            where cp.card_id = cc.card_id
                                                                                                                AND (cp.site_id = @siteIdCTC or @siteIdCTC = -1) 
                                                                                                                and cp.CreditPlusType = 'L' 
                                                                                                                and creationDate > getdate()-365),0)) loyaltyPoints
                                                                    from customers cu, cards cc
                                                                    where cc.customer_id = cu.customer_id
                                                                    AND (cu.site_id = @siteIdCTC or @siteIdCTC = -1) 
                                                                    AND (cc.site_id = @siteIdCTC or @siteIdCTC = -1) 
                                                                    and cc.valid_flag = 'Y'
                                                                    and cc.technician_card = 'N' 
                                                                    and cc.cardTypeId = @cardTypeIdCTC 
                                                                    and cu.membershipId is null
                                                                order by cc.customer_id, cc.card_id", sqlParamCTC2.ToArray());

                                    foreach (DataRow cardTypeCustomerRow in dtCardTypeCustomers.Rows)
                                    {
                                        SqlCommand sqlCmdCTC = utilities.getCommand(utilities.createConnection().BeginTransaction());
                                        SqlTransaction sqlCmdTrxCTC = sqlCmdCTC.Transaction;
                                        SqlConnection DBConnectionCTC = sqlCmdCTC.Connection;
                                        int customerId = Convert.ToInt32(cardTypeCustomerRow["customer_id"]);
                                        try
                                        {

                                            if (customerId > -1)
                                            {
                                                DataRow[] foundCards;
                                                foundCards = dtCardTypeCards.Select("customer_id  = " + customerId.ToString());
                                                if (foundCards.Length > 0)
                                                {
                                                    int primaryCardId = -1;
                                                    //Set Primary and VIP flags
                                                    primaryCardId = SetPrimaryNVIPCard(false, customerId, vipMembership, dtCardTypeCards, foundCards, this.siteId, sqlCmdTrxCTC);
                                                    //calculate the purchase / recharge done by the customer as per membership qualification period from current time
                                                    //double loyaltyPoints = 0;
                                                    List<Tuple<string, double>> loyaltyPoints = new List<Tuple<string, double>>();

                                                    if (loyaltyPointsCalculationOption == 1)
                                                    {
                                                        loyaltyPoints = GetSpendRechargeByMembershipTier(customerId, -1, loyaltyPointsCalculationDuration, "S", sqlCmdTrxCTC);
                                                    }
                                                    else if (loyaltyPointsCalculationOption == 2)
                                                    {
                                                        loyaltyPoints = GetSpendRechargeByMembershipTier(customerId, -1, loyaltyPointsCalculationDuration, "R", sqlCmdTrxCTC);
                                                    }
                                                    //Create loyalty point credit plus entry with for membership only flag as per program input 
                                                    foreach (Tuple<string, double> lpEntry in loyaltyPoints)
                                                    {
                                                        CreateGenericCreditPlusLine(primaryCardId, "L", Math.Round(lpEntry.Item2 * loyaltyPointConversionRatio, 0), false, 0, "N", this.loginId, "cardTypeMigration: " + lpEntry.Item1, DateTime.Now, -1, -1, (canRedeemloyaltyPoints == true ? "N" : "Y"), this.siteId, sqlCmdTrxCTC);
                                                    }


                                                    if (cardTypeCustomerRow["latestEffectiveDate"].ToString() != "")
                                                    {
                                                        newMembershipEffeciveFromDate = Convert.ToDateTime(cardTypeCustomerRow["latestEffectiveDate"]);
                                                    }
                                                    else
                                                    {
                                                        newMembershipEffeciveFromDate = DateTime.Now;
                                                    }
                                                    //Create new membershipprogression entry for the customer  set(get latest effectiveDate) as effective from date and calculate effective to date based on membership rule setup
                                                    CreateMembershipProgresssionEntry(customerId, primaryCardId, newMembershipId, newMembershipEffeciveFromDate, newMembershipEffeciveToDate, this.loginId, this.siteId, sqlCmdTrxCTC);
                                                    // Update customer record with membership id
                                                    UpdateCustomerRecord(customerId, newMembershipId, this.loginId, this.siteId, sqlCmdTrxCTC);

                                                    if (createDailyCardBalanceEntry)
                                                    {
                                                        foreach (DataRow cardRow in foundCards)
                                                        {
                                                            CreateDailyCardBalance(Convert.ToInt32(cardRow["card_id"]), customerId, DateTime.Now, this.loginId, this.siteId, sqlCmdTrxCTC);
                                                        }
                                                    }
                                                }
                                            }
                                            sqlCmdTrxCTC.Commit();
                                            DBConnectionCTC.Close();
                                            DBConnectionCTC.Dispose();
                                            sqlCmdCTC.Dispose();
                                        }
                                        catch (Exception ex)
                                        {
                                            outString.Append(MessageContainerList.GetMessage(executionContext, 1576, cardTypeId.ToString()) + " CustomerID: " + customerId.ToString() + "  " + ex.Message + Environment.NewLine);
                                            log.Error("Error while processing cardTypeId: " + cardTypeId.ToString() + " CustomerID: " + customerId.ToString(), ex);
                                            sqlCmdTrxCTC.Rollback();
                                            DBConnectionCTC.Close();
                                            DBConnectionCTC.Dispose();
                                            sqlCmdCTC.Dispose();
                                            UpdateCardTypeRecord(cardTypeId, false, ex.Message, this.siteId, null);
                                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1576, cardTypeId.ToString()));
                                        }
                                    }
                                }

                                List<SqlParameter> sqlParamCTNC = new List<SqlParameter>();
                                sqlParamCTNC.Add(new SqlParameter("@cardTypeIdCTNC", cardTypeId));
                                sqlParamCTNC.Add(new SqlParameter("@siteIdCTNC", this.siteId));
                                //Fetch active cards without a customer record  
                                //sqlCmd.CommandText = @"select cc.* ,
                                //                                (select Max(mp.effectiveDate)
                                //                                from MembershipProgression mp
                                //                                where cc.card_id = mp.CardId
                                //                                    AND (mp.site_id = @siteId or @siteId = -1) ) latestEffectiveDate
                                //                        from cards cc
                                //                        where cc.customer_id IS NULL
                                //                        and cc.valid_flag = 'Y'
                                //                        AND (cc.site_id = @siteId or @siteId = -1) 
                                //                        and cc.cardTypeId = @cardTypeId  
                                //                    order by cc.card_id";

                                //DataTable dtCardTypeCardsWithOutCustomer = new DataTable();
                                //SqlDataAdapter daCTCWOTC = new SqlDataAdapter(sqlCmd);
                                //daCTCWOTC.Fill(dtCardTypeCardsWithOutCustomer); 
                                DataTable dtCardTypeCardsWithOutCustomer = utilities.executeDataTable(@"select cc.* ,
                                                                                                                (select Max(mp.effectiveDate)
                                                                                                                   from MembershipProgression mp
                                                                                                                  where cc.card_id = mp.CardId
                                                                                                                    AND (mp.site_id = @siteIdCTNC or @siteIdCTNC = -1) ) latestEffectiveDate
                                                                                                        from cards cc
                                                                                                        where cc.customer_id IS NULL
                                                                                                        and cc.valid_flag = 'Y'
                                                                                                        and cc.technician_card = 'N' 
                                                                                                        AND (cc.site_id = @siteIdCTNC or @siteIdCTNC = -1) 
                                                                                                        and cc.cardTypeId = @cardTypeIdCTNC  
                                                                                                    order by cc.card_id", sqlParamCTNC.ToArray());
                                if (dtCardTypeCardsWithOutCustomer.Rows.Count > 0)
                                {
                                    // int counter = 0;
                                    foreach (DataRow cardswithoutCustomer in dtCardTypeCardsWithOutCustomer.Rows)
                                    {
                                        //counter++;
                                        SqlCommand sqlCmdCTNC = utilities.getCommand(utilities.createConnection().BeginTransaction());
                                        SqlTransaction sqlCmdTrxCTNC = sqlCmdCTNC.Transaction;
                                        SqlConnection DBConnectionCTNC = sqlCmdCTNC.Connection;
                                        int cardId = Convert.ToInt32(cardswithoutCustomer["card_id"]);
                                        try
                                        {

                                            string cardNumber = cardswithoutCustomer["card_number"].ToString();
                                            int customerId = CreateCustomerRecord(cardId, cardNumber, customerSetup, sqlCmdTrxCTNC);
                                            SetPrimaryNVIPCard(true, customerId, vipMembership, cardId, this.siteId, sqlCmdTrxCTNC);
                                            //calculate the purchase / recharge done by the customer as per membership qualification period from current time
                                            List<Tuple<string, double>> loyaltyPoints = new List<Tuple<string, double>>();

                                            if (loyaltyPointsCalculationOption == 1)
                                            {
                                                loyaltyPoints = GetSpendRechargeByMembershipTier(customerId, -1, loyaltyPointsCalculationDuration, "S", sqlCmdTrxCTNC);
                                            }
                                            else if (loyaltyPointsCalculationOption == 2)
                                            {
                                                loyaltyPoints = GetSpendRechargeByMembershipTier(customerId, -1, loyaltyPointsCalculationDuration, "R", sqlCmdTrxCTNC);
                                            }
                                            //Create loyalty point credit plus entry with for membership only flag as per program input 
                                            foreach (Tuple<string, double> lpEntry in loyaltyPoints)
                                            {
                                                CreateGenericCreditPlusLine(cardId, "L", Math.Round(lpEntry.Item2 * loyaltyPointConversionRatio, 0), false, 0, "N", this.loginId, "cardTypeMigration: " + lpEntry.Item1, DateTime.Now, -1, -1, (canRedeemloyaltyPoints == true ? "N" : "Y"), this.siteId, sqlCmdTrxCTNC);
                                            }
                                            if (cardswithoutCustomer["latestEffectiveDate"].ToString() != "")
                                            {
                                                newMembershipEffeciveFromDate = Convert.ToDateTime(cardswithoutCustomer["latestEffectiveDate"]);
                                            }
                                            else
                                            {
                                                newMembershipEffeciveFromDate = DateTime.Now;
                                            }
                                            //Create new membershipprogression entry for the customer  set(get latest effectiveDate) as effective from date and calculate effective to date based on membership rule setup
                                            CreateMembershipProgresssionEntry(customerId, cardId, newMembershipId, newMembershipEffeciveFromDate, newMembershipEffeciveToDate, this.loginId, this.siteId, sqlCmdTrxCTNC);
                                            // Update customer record with membership id
                                            UpdateCustomerRecord(customerId, newMembershipId, this.loginId, this.siteId, sqlCmdTrxCTNC);

                                            if (createDailyCardBalanceEntry)
                                            {
                                                CreateDailyCardBalance(cardId, customerId, DateTime.Now, this.loginId, this.siteId, sqlCmdTrxCTNC);
                                            }

                                            sqlCmdTrxCTNC.Commit();
                                            DBConnectionCTNC.Close();
                                            DBConnectionCTNC.Dispose();
                                            sqlCmdCTNC.Dispose();
                                        }
                                        catch (Exception ex)
                                        {
                                            outString.Append(MessageContainerList.GetMessage(executionContext, 1576, cardTypeId.ToString()) + " CardId: " + cardId.ToString() + "  " + ex.Message + Environment.NewLine);
                                            log.Error("Error while processing cardTypeId: " + cardTypeId.ToString() + " CardId: " + cardId.ToString(), ex);
                                            sqlCmdTrxCTNC.Rollback();
                                            DBConnectionCTNC.Close();
                                            DBConnectionCTNC.Dispose();
                                            sqlCmdCTNC.Dispose();
                                            UpdateCardTypeRecord(cardTypeId, false, ex.Message, this.siteId, null);
                                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1576, cardTypeId.ToString()));
                                        }
                                    }
                                }

                                UpdateCardTypeRecord(cardTypeId, true, "", this.siteId, null);
                                //sqlCmdTrx.Commit();
                                //DBConnection.Close();
                                //DBConnection.Dispose();
                                //sqlCmd.Dispose();
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine("Error while processing cardTypeId: " + cardTypeId.ToString() + "  " + ex.Message);
                                outString.Append(MessageContainerList.GetMessage(executionContext, 1576, cardTypeId.ToString()) + "  " + ex.Message + Environment.NewLine);
                                log.Error("Error while processing cardTypeId: " + cardTypeId.ToString(), ex);
                                //sqlCmdTrx.Rollback();
                                //DBConnection.Close();
                                //DBConnection.Dispose();
                                //sqlCmd.Dispose();
                                UpdateCardTypeRecord(cardTypeId, false, ex.Message, this.siteId, null);
                                throw new Exception(MessageContainerList.GetMessage(executionContext, 1576, cardTypeId.ToString()));
                            }
                        }
                        // if (counter % 1000 == 0 && counter > 0)
                        //     Console.WriteLine(counter.ToString() + " records processed");
                        outString.Append(MessageContainerList.GetMessage(executionContext, 1577, cardTypeId.ToString()) + Environment.NewLine);
                    }
                    outString.Append(MessageContainerList.GetMessage(executionContext, 1577, " ALL ") + Environment.NewLine);
                }

                if (nonCardTypeDT.Rows.Count > 0)
                {
                    outString.Append(MessageContainerList.GetMessage(executionContext, 1578) + Environment.NewLine);
                    SqlCommand sqlCmdNCT = utilities.getCommand(utilities.createConnection().BeginTransaction());
                    SqlTransaction sqlCmdNCTTrx = sqlCmdNCT.Transaction;
                    SqlConnection DBConnectionNCT = sqlCmdNCT.Connection;
                    int nonCardTypeId = -1;
                    int loyaltyPointsCalculationOptionNCT = -1;
                    int loyaltyPointsCalculationDurationNCT = -1;
                    double loyaltyPointConversionRatioNCT = 0;
                    bool canRedeemLoyaltyPointsNCT = false;
                    try
                    {
                        nonCardTypeId = Convert.ToInt32(nonCardTypeDT.Rows[0]["CardTypeId"]);
                        loyaltyPointConversionRatioNCT = Convert.ToDouble(nonCardTypeDT.Rows[0]["LoyaltyPointConvRatio"]);
                        canRedeemLoyaltyPointsNCT = Convert.ToBoolean(nonCardTypeDT.Rows[0]["RedeemLoyaltyPoints"]);
                        loyaltyPointsCalculationOptionNCT = Convert.ToInt32(nonCardTypeDT.Rows[0]["ExistingTriggerSource"]);
                        loyaltyPointsCalculationDurationNCT = Convert.ToInt32(nonCardTypeDT.Rows[0]["QualifyingDuration"]);
                    }
                    catch
                    {  //Console.WriteLine("Invalid membership id for CardType {0} ", cardTypeRow["cardTypeId"].ToString());
                        outString.Append(MessageContainerList.GetMessage(executionContext, 1579) + Environment.NewLine);
                        log.Error("Invalid set up details Non CardType " + Environment.NewLine);
                        nonCardTypeId = -1;
                    }
                    if (nonCardTypeId > -1)
                    {
                        try
                        {
                            //All cards types are processed. 
                            //fetch active cards without card type. But linked to customers
                            sqlCmdNCT.Parameters.AddWithValue("@siteId", this.siteId);
                            sqlCmdNCT.CommandText = @"select cu.* 
                                                        from customers cu
                                                        where EXISTS (SELECT 1 
                                                                        from cards cc
                                                                        WHERE cc.customer_id = cu.customer_id
                                                                        AND (cc.site_id = @siteId or @siteId = -1) 
                                                                        and cc.valid_flag = 'Y'
                                                                        and cc.technician_card = 'N' 
                                                                        and cc.cardTypeId IS NULL) 
                                                            and cu.membershipId is null";
                            DataTable dtNonCardTypeCustomers = new DataTable();
                            SqlDataAdapter dtcardsWOTCTWithCustomers = new SqlDataAdapter(sqlCmdNCT);
                            dtcardsWOTCTWithCustomers.Fill(dtNonCardTypeCustomers);

                            if (dtNonCardTypeCustomers.Rows.Count > 0)
                            {
                                sqlCmdNCT.CommandText = @"select cc.*,
								                                (SELECT top 1 'Y' 
                                                                            from parentchildcards pcc
                                                                            where pcc.ParentCardId = cc.card_id
                                                                            AND (pcc.site_id = @siteId or @siteId = -1) 
                                                                            AND pcc.ActiveFlag = '1') as parentCard,
                                                                ( isnull(cc.loyalty_points, 0) + ISNULL((select sum(ISNULL(cp.CreditPlus,0)) 
                                                                                                           from CardCreditPlus cp
                                                                                                          where cp.card_id = cc.card_id
                                                                                                            AND (cp.site_id = @siteId or @siteId = -1) 
                                                                                                            and cp.CreditPlusType = 'L' 
                                                                                                            and creationDate > getdate()-365),0)) loyaltyPoints
                                                           from customers cu, cards cc
                                                         where cc.customer_id = cu.customer_id
                                                          and cc.valid_flag = 'Y'
                                                          and cc.technician_card = 'N' 
                                                          and cc.cardTypeId IS NULL
                                                          AND (cu.site_id = @siteId or @siteId = -1) 
                                                          AND (cc.site_id = @siteId or @siteId = -1) 
                                                          and cu.membershipId is null
                                                    order by cc.customer_id, cc.card_id";

                                DataTable dtNonCardTypeCards = new DataTable();
                                SqlDataAdapter dtcardsWOTCTCardWithCustomers = new SqlDataAdapter(sqlCmdNCT);
                                dtcardsWOTCTCardWithCustomers.Fill(dtNonCardTypeCards);

                                outString.Append(MessageContainerList.GetMessage(executionContext, 1580) + Environment.NewLine);
                                foreach (DataRow nonCardTypeCustomerRow in dtNonCardTypeCards.Rows)
                                {
                                    int customerId = Convert.ToInt32(nonCardTypeCustomerRow["customer_id"]);
                                    if (customerId > -1)
                                    {
                                        DataRow[] foundNCTCards;
                                        foundNCTCards = dtNonCardTypeCards.Select("customer_id  = " + customerId.ToString());
                                        if (foundNCTCards.Length > 0)
                                        {
                                            int primaryCardId = -1;
                                            //Set Primary and VIP flags
                                            primaryCardId = SetPrimaryNVIPCard(false, customerId, 0, dtNonCardTypeCards, foundNCTCards, this.siteId, sqlCmdNCTTrx);
                                            //calculate the purchase / recharge done by the customer as per membership qualification period from current time
                                            List<Tuple<string, double>> loyaltyPoints = new List<Tuple<string, double>>();

                                            if (loyaltyPointsCalculationOptionNCT == 1)
                                            {
                                                loyaltyPoints = GetSpendRechargeByMembershipTier(customerId, -1, loyaltyPointsCalculationDurationNCT, "S", sqlCmdNCTTrx);
                                            }
                                            else if (loyaltyPointsCalculationOptionNCT == 2)
                                            {
                                                loyaltyPoints = GetSpendRechargeByMembershipTier(customerId, -1, loyaltyPointsCalculationDurationNCT, "R", sqlCmdNCTTrx);
                                            }
                                            //Create loyalty point credit plus entry with for membership only flag as per program input 
                                            foreach (Tuple<string, double> lpEntry in loyaltyPoints)
                                            {
                                                CreateGenericCreditPlusLine(primaryCardId, "L", Math.Round(lpEntry.Item2 * loyaltyPointConversionRatioNCT, 0), false, 0, "N", this.loginId, "cardTypeMigration: " + lpEntry.Item1, DateTime.Now, -1, -1, (canRedeemLoyaltyPointsNCT == true ? "N" : "Y"), this.siteId, sqlCmdNCTTrx);
                                            }
                                        }

                                    }
                                }
                            }

                            //fetch active cards without card type. But not linked to customers
                            //Fetch active cards without a customer record 
                            sqlCmdNCT.CommandText = @"select cc.*  
                                                        from cards cc
                                                        where cc.customer_id IS NULL
                                                        and cc.valid_flag = 'Y'
                                                        and cc.technician_card = 'N' 
                                                        and cc.cardTypeId IS NULL
                                                        AND (cc.site_id = @siteId or @siteId = -1) 
                                                    order by cc.card_id";

                            DataTable dtNonCardTypeCardsWithOutCustomer = new DataTable();
                            SqlDataAdapter dtcardsWOTCTWithOutCustomers = new SqlDataAdapter(sqlCmdNCT);
                            dtcardsWOTCTWithOutCustomers.Fill(dtNonCardTypeCardsWithOutCustomer);

                            if (dtNonCardTypeCardsWithOutCustomer.Rows.Count > 0)
                            {

                                foreach (DataRow nctCardswithoutCustomer in dtNonCardTypeCardsWithOutCustomer.Rows)
                                {
                                    int cardId = Convert.ToInt32(nctCardswithoutCustomer["card_id"]);
                                    string cardNumber = nctCardswithoutCustomer["card_number"].ToString();
                                    // int customerId = CreateCustomerRecord(cardId, cardNumber, customerSetup, sqlCmdNCTTrx);
                                    //  SetPrimaryNVIPCard(true, customerId, 0, cardId, this.siteId, sqlCmdNCTTrx);
                                    //calculate the purchase / recharge done by the customer as per membership qualification period from current time
                                    List<Tuple<string, double>> loyaltyPoints = new List<Tuple<string, double>>();

                                    if (loyaltyPointsCalculationOptionNCT == 1)
                                    {
                                        loyaltyPoints = GetSpendRechargeByMembershipTier(-1, cardId, loyaltyPointsCalculationDurationNCT, "S", sqlCmdNCTTrx);
                                    }
                                    else if (loyaltyPointsCalculationOptionNCT == 2)
                                    {
                                        loyaltyPoints = GetSpendRechargeByMembershipTier(-1, cardId, loyaltyPointsCalculationDurationNCT, "R", sqlCmdNCTTrx);
                                    }
                                    //Create loyalty point credit plus entry with for membership only flag as per program input 
                                    foreach (Tuple<string, double> lpEntry in loyaltyPoints)
                                    {
                                        CreateGenericCreditPlusLine(cardId, "L", Math.Round(lpEntry.Item2 * loyaltyPointConversionRatioNCT, 0), false, 0, "N", this.loginId, "cardTypeMigration: " + lpEntry.Item1, DateTime.Now, -1, -1, (canRedeemLoyaltyPointsNCT == true ? "N" : "Y"), this.siteId, sqlCmdNCTTrx);
                                    }
                                }
                            }

                            UpdateCardTypeRecord(nonCardTypeId, true, "", this.siteId, sqlCmdNCTTrx);
                            sqlCmdNCTTrx.Commit();
                            DBConnectionNCT.Close();
                            DBConnectionNCT.Dispose();
                            sqlCmdNCT.Dispose();
                        }
                        catch (Exception ex)
                        {
                            // Console.WriteLine("Error while processing non cardType : " + ex.Message);
                            outString.Append(MessageContainerList.GetMessage(executionContext, 1581, ex.Message) + Environment.NewLine);
                            log.Error("Error while processing non cardTypeId: ", ex);
                            sqlCmdNCTTrx.Rollback();
                            DBConnectionNCT.Close();
                            DBConnectionNCT.Dispose();
                            sqlCmdNCTTrx.Dispose();
                            UpdateCardTypeRecord(nonCardTypeId, false, ex.Message, this.siteId, null);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("Error DoMembershipMigration", ex);
                outString.Append(MessageContainerList.GetMessage(executionContext, 1582) + ex.Message + Environment.NewLine);
                throw;
            }
            finally
            {
                DisableEnableDBTriggers(true);
            }
            outString.Append(MessageContainerList.GetMessage(executionContext, 1582) + Environment.NewLine);
            log.LogMethodExit(outString);
            return outString.ToString();
        }

        List<Tuple<string, double>> GetSpendRechargeByMembershipTier(int customerId, int cardId, int days, string returnDataOption, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(customerId, cardId, days, returnDataOption, sqlTrx);
            string query;
            List<Tuple<string, double>> retValue = new List<Tuple<string, double>>();
            if (returnDataOption == "S") //Spend
            {
                query = @"select  ISNULL((select sum(isnull( p.amount, 0))
                                        from trx_header h, trxPayments p, cards c 
                                        where c.valid_flag='Y'
                                          and c.card_id = @cardId
                                          --and c.customer_id = @customerId 
                                          AND (h.site_id = @siteId or @siteId = -1) 
                                          AND (p.site_id = @siteId or @siteId = -1) 
                                          AND (c.site_id = @siteId or @siteId = -1)   
                                          and isnull(p.PaymentDate, h.trxDate) >= getdate()-@days
                                          and p.cardId = c.card_id
                                          and p.trxId = h.trxId),0)
                                    + ISNULL((select isnull(sum(g.credits + g.bonus + g.time + g.cardGame + g.CPCardBalance + g.CPCredits + g.CPBonus), 0) 
                                        from gameplay g, cards c 
                                        where c.valid_flag='Y'
                                          and c.card_id = @cardId
                                          --and c.customer_id = @customerId  
                                          AND (c.site_id = @siteId or @siteId = -1) 
                                          AND (g.site_id = @siteId or @siteId = -1) 
                                          and g.play_date >= getdate()-@days 
                                          and c.card_id = g.card_id),0) SpendAmount ";
            }
            else //recharge
            {
                query = @"select ISNULL((select SUM(isnull(l.amount, 0))
                                        from trx_header h, trx_lines l, cards c 
                                        where  c.card_id = @cardId      
                                          --and c.customer_id = @customerId  
                                          AND (h.site_id = @siteId or @siteId = -1) 
                                          AND (l.site_id = @siteId or @siteId = -1) 
                                          AND (c.site_id = @siteId or @siteId = -1) 
                                          and c.valid_flag='Y'
                                          and h.trxDate >= getdate()-@days
                                          and l.card_id = c.card_id                                 
                                          and l.trxId = h.trxId),0) RechargeAmount  ";
            }
            SqlCommand cmd = utilities.getCommand(sqlTrx);

            if (customerId == -1 && cardId != -1)
            {
                cmd.CommandText = query;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@siteId", this.siteId);
                cmd.Parameters.AddWithValue("@cardId", cardId.ToString());
                cmd.Parameters.AddWithValue("@days", days);

                DataTable dt = new DataTable();
                SqlDataAdapter daSpendRecharge = new SqlDataAdapter(cmd);
                daSpendRecharge.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    double retAmount = Convert.ToDouble(dt.Rows[0][0]);
                    retValue.Add(new Tuple<string, double>(cardId.ToString(), retAmount));
                }
            }
            else
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.Parameters.AddWithValue("@siteId", this.siteId);
                cmd.CommandText = @"SELECT card_id , card_number from cards c where c.customer_id = @customerId and c.valid_flag='Y' and  (c.site_id = @siteId or @siteId = -1) ";
                DataTable dtCards = new DataTable();
                SqlDataAdapter daGetSpendRecharge = new SqlDataAdapter(cmd);
                daGetSpendRecharge.Fill(dtCards);
                if (dtCards.Rows.Count > 0 && customerId != -1)
                {
                    cmd.CommandText = query;
                    foreach (DataRow cardRow in dtCards.Rows)
                    {
                        cmd.Parameters.Clear();
                        //cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@siteId", this.siteId);
                        cmd.Parameters.AddWithValue("@cardId", cardRow["card_id"].ToString());
                        cmd.Parameters.AddWithValue("@days", days);

                        DataTable dt = new DataTable();
                        SqlDataAdapter daSpendRecharge = new SqlDataAdapter(cmd);
                        daSpendRecharge.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            double retAmount = Convert.ToDouble(dt.Rows[0][0]);
                            retValue.Add(new Tuple<string, double>(cardRow["card_number"].ToString(), retAmount));
                        }
                    }
                }
            }


            cmd.Dispose();
            // DataTable dt = utilities.executeDataTable(query, SQLTrx, new SqlParameter("@customerId", customerId), new SqlParameter("@days", days));

            log.LogMethodExit(retValue);
            return retValue;
        }

        void CreateGenericCreditPlusLine(int cardId, string creditPlusType, double amount, bool refundable, int expiryDays, string autoExtend, string loginId, string remarks, DateTime? periodFrom, int parafaitMembershipId, int membershipRewardId, string forMembershipOnly, int siteId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(cardId, creditPlusType, amount, refundable, expiryDays, autoExtend, loginId, remarks, periodFrom, parafaitMembershipId, membershipRewardId, forMembershipOnly, siteId, sqlTrx);

            SqlCommand cmd = utilities.getCommand(sqlTrx);

            cmd.CommandText = @"insert into CardCreditPlus  
                                       ([CreditPlus] 
                                       ,[CreditPlusType] 
                                       ,[Refundable] 
                                       ,[Remarks] 
                                       ,[Card_id] 
                                       ,[CreditPlusBalance] 
                                       ,[PeriodFrom] 
                                       ,[PeriodTo] 
                                       ,[ExtendOnReload] 
                                       ,[PlayStartTime] 
                                       ,[CreationDate] 
                                       ,[LastupdatedDate] 
                                       ,[LastUpdatedBy]
                                       ,[MembershipId]
                                       ,[MembershipRewardsId]
                                       ,[ForMembershipOnly]
                                       , [Site_id] ) 
                                       values (
                                        @CreditPlus,
                                        @CreditPlusType,
                                        @Refundable,
                                        @Remarks,
                                        @card_id,
                                        @CreditPlus,
                                        CASE @PeriodFrom WHEN  NULL THEN NULL ELSE dateadd(HH, 6, convert(datetime, convert(varchar, @PeriodFrom, 101), 101)) end,
                                        case @ExpiryDays when 0 then NULL else dateadd(HH, 6, convert(datetime, convert(varchar, getdate(), 101), 101) + @ExpiryDays) end,
                                        @ExtendOnReload,
                                        @PlayStartTime,
                                        @CreationDate,
                                        getdate(),
                                        @LastUpdatedBy,
                                        @MembershipId,
                                        @MembershipRewardsId,
                                        @ForMembershipOnly, 
                                        @siteId ) ";

            cmd.Parameters.AddWithValue("@ExpiryDays", expiryDays);
            cmd.Parameters.AddWithValue("@ExtendOnReload", autoExtend);
            cmd.Parameters.AddWithValue("@CreditPlusType", creditPlusType);
            cmd.Parameters.AddWithValue("@Refundable", refundable ? "Y" : "N");
            cmd.Parameters.AddWithValue("@card_id", cardId);
            cmd.Parameters.AddWithValue("@CreditPlus", amount);
            cmd.Parameters.AddWithValue("@LastUpdatedBy", loginId);
            cmd.Parameters.AddWithValue("@Remarks", remarks);
            cmd.Parameters.AddWithValue("@PlayStartTime", DBNull.Value);
            if (periodFrom == null)
            {
                cmd.Parameters.AddWithValue("@PeriodFrom", DBNull.Value);
                cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PeriodFrom", periodFrom);
                cmd.Parameters.AddWithValue("@CreationDate", periodFrom);
            }
            // parafaitMembershipId, onePercentRewardId, enrollmentRewardid,
            if (parafaitMembershipId == -1)
            {
                cmd.Parameters.AddWithValue("@MembershipId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@MembershipId", parafaitMembershipId);
            }
            if (membershipRewardId == -1)
            {
                cmd.Parameters.AddWithValue("@MembershipRewardsId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@MembershipRewardsId", membershipRewardId);
            }

            if (forMembershipOnly == "Y")
            {
                cmd.Parameters.AddWithValue("@ForMembershipOnly", forMembershipOnly);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ForMembershipOnly", DBNull.Value);
            }
            if (siteId == -1)
            {
                cmd.Parameters.AddWithValue("@siteId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@siteId", siteId);
            }
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            log.LogMethodExit();
        }

        void CreateMembershipProgresssionEntry(int customerId, int cardId, int membershipId, DateTime effectiveFromDate, DateTime effectiveToDate, string loginId, int siteId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(customerId, cardId, membershipId, effectiveFromDate, effectiveToDate, loginId, siteId, sqlTrx);
            SqlCommand cmd = utilities.getCommand(sqlTrx);

            cmd.CommandText = @"INSERT INTO MembershipProgression 
                                        ( 
                                              CardId 
                                            , EffectiveDate
                                            , site_id
                                            , Guid 
                                            , MembershipId
                                            , CustomerId
                                            , EffectiveFromDate
                                            , EffectiveToDate
                                            , LastRetentionDate
                                            , CreatedBy
                                            , CreationDate
                                            , LastUpdatedBy
                                            , LastUpdateDate
                                        ) 
                                VALUES 
                                        (
                                             @CardId 
                                            ,@EffectiveDate
                                            ,@siteId
                                            ,NEWID() 
                                            ,@MembershipId
                                            ,@CustomerId
                                            ,@EffectiveFromDate
                                            ,@EffectiveToDate
                                            ,@LastRetentionDate
                                            ,@CreatedBy
                                            ,GETDATE()
                                            ,@LastUpdatedBy
                                            ,GETDATE()
                                        )";
            if (cardId == -1)
            {
                cmd.Parameters.AddWithValue("@CardId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CardId", cardId);
            }
            cmd.Parameters.AddWithValue("@EffectiveDate", effectiveFromDate);
            if (siteId == -1)
            {
                cmd.Parameters.AddWithValue("@siteId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@siteId", siteId);
            }
            if (membershipId == -1)
            {
                cmd.Parameters.AddWithValue("@MembershipId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@MembershipId", membershipId);
            }
            if (customerId == -1)
            {
                cmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
            }

            cmd.Parameters.AddWithValue("@EffectiveFromDate", effectiveFromDate);
            cmd.Parameters.AddWithValue("@EffectiveToDate", effectiveToDate);
            cmd.Parameters.AddWithValue("@LastRetentionDate", DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", loginId);
            cmd.Parameters.AddWithValue("@LastUpdatedBy", loginId);
            cmd.ExecuteNonQuery();
            //object newMembershipProgressionId = cmd.ExecuteScalar();
            cmd.Dispose();
            log.LogMethodExit();
        }

        void UpdateCustomerRecord(int customerId, int membershipId, string loginId, int siteId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(customerId, membershipId, loginId, siteId, sqlTrx);
            SqlCommand sqlCmd = utilities.getCommand(sqlTrx);

            sqlCmd.CommandText = @"Update customers 
                                      set membershipId = @membershipId, Last_Updated_user = @loginId, Last_Updated_Date = getDate()
                                    WHERE customer_id = @customerId 
                                      and (site_id = @siteId OR  @siteId = -1) ";
            if (customerId > -1)
                sqlCmd.Parameters.AddWithValue("@customerId", customerId);
            else
                sqlCmd.Parameters.AddWithValue("@customerId", DBNull.Value);
            if (membershipId > -1)
                sqlCmd.Parameters.AddWithValue("@membershipId", membershipId);
            else
                sqlCmd.Parameters.AddWithValue("@membershipId", DBNull.Value);

            sqlCmd.Parameters.AddWithValue("@loginId", loginId);
            sqlCmd.Parameters.AddWithValue("@siteId", siteId);
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
            log.LogMethodExit();
        }

        void UpdateCardTypeRecord(int cardTypeId, bool migrationStatus, string migrationMessage, int siteId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(cardTypeId, migrationStatus, migrationMessage, sqlTrx);
            string cmdText = @"UPDATE cardtype 
                                  SET MigrationMessage = @migrationMessage, cardTypeMigrated = @migrationStatus
                                WHERE cardtypeId = @cardTypeId 
                                  and (site_id = @siteId OR  @siteId = -1) ";
            SqlParameter[] sqlParameterList = new SqlParameter[4];

            sqlParameterList[0] = new SqlParameter("@cardTypeId", cardTypeId.ToString());

            if (migrationStatus)
                sqlParameterList[1] = new SqlParameter("@migrationStatus", true);
            else
                sqlParameterList[1] = new SqlParameter("@migrationStatus", DBNull.Value);

            if (String.IsNullOrEmpty(migrationMessage))
                sqlParameterList[2] = new SqlParameter("@migrationMessage", DBNull.Value);
            else
                sqlParameterList[2] = new SqlParameter("@migrationMessage", migrationMessage);

            sqlParameterList[3] = new SqlParameter("@siteid", siteId);

            if (sqlTrx == null)
            {
                utilities.executeNonQuery(cmdText, sqlTrx, sqlParameterList);
            }
            else
            {
                SqlCommand cmd = utilities.getCommand(sqlTrx);
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(sqlParameterList);
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            log.LogMethodExit();
        }
        int SetPrimaryNVIPCard(bool linkCustomer, int customerId, int vipMembership, DataTable dtCardTypeCards, DataRow[] foundCards, int siteId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(linkCustomer, customerId, vipMembership, dtCardTypeCards, foundCards, siteId, sqlTrx);
            int primaryCardId = -1;
            SqlCommand cmd = utilities.getCommand(sqlTrx);
            //VIP check
            if (vipMembership == 1 || linkCustomer)
            {
                StringBuilder cardIds = new StringBuilder(); ;
                int i = 0;
                foreach (DataRow cardRow in foundCards)
                {
                    if (i == foundCards.Length - 1)
                    {
                        cardIds.Append(cardRow["card_id"].ToString());
                    }
                    else
                    {
                        cardIds.Append(cardRow["card_id"].ToString() + " , ");
                    }
                    i = i + 1;
                }
                //set cards as VIP
                cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.Parameters.AddWithValue("@cardIds", cardIds.ToString());
                cmd.Parameters.AddWithValue("@siteId", siteId);
                cmd.Parameters.AddWithValue("@vipMembership", vipMembership);
                cmd.Parameters.AddWithValue("@loginId", this.loginId);

                if (linkCustomer)
                {
                    cmd.CommandText = @"update cards
                                            set customer_id  = @customerId ,
                                                last_update_time = getdate(),
                                                LastUpdatedBy = @loginId
                                          where card_id in ( @cardIds ) 
                                            and (site_id = @siteId OR  @siteId = -1)  ";
                    cmd.ExecuteNonQuery();
                }
                if (vipMembership == 1)
                {
                    cmd.CommandText = @"update cards
                                            set vip_customer = (CASE @vipMembership WHEN 1 THEN 'Y' ELSE vip_customer END) ,
                                                last_update_time = getdate(),
                                                LastUpdatedBy = @loginId
                                          where customer_id  = @customerId 
                                            and card_id in ( @cardIds ) 
                                            and (site_id = @siteId OR  @siteId = -1)  ";
                    cmd.ExecuteNonQuery();
                }
            }

            if (foundCards.Length == 1)
            {
                primaryCardId = Convert.ToInt32(foundCards[0]["card_id"]);
            }
            else
            {
                //check for parentCard  presence
                DataRow[] foundPrimary = dtCardTypeCards.Select("parentCard = 'Y' and customer_id  = " + customerId.ToString());
                if (foundPrimary.Length > 0)
                {
                    if (foundPrimary.Length == 1)
                    {
                        primaryCardId = Convert.ToInt32(foundPrimary[0]["card_id"]);
                    }
                }
                else
                {  //check for card with Max loyalty points
                    DataRow[] sortedCardRows = foundCards.OrderByDescending(row => row["loyaltyPoints"]).ToArray();
                    if (sortedCardRows.Length > 1)
                    {
                        if (Convert.ToInt32(sortedCardRows[0]["loyaltyPoints"]) > Convert.ToInt32(sortedCardRows[1]["loyaltyPoints"]))
                        {
                            //Set card with max loyltyPoints as Primary
                            primaryCardId = Convert.ToInt32(sortedCardRows[0]["card_id"]);
                        }
                        else
                        {
                            sortedCardRows = foundCards.OrderBy(row => row["issue_date"]).ToArray();
                            if (sortedCardRows.Length > 0)
                            {
                                //Set oldest issued card as Primary
                                primaryCardId = Convert.ToInt32(sortedCardRows[0]["card_id"]);
                            }
                        }
                    }
                    else
                    {
                        if (sortedCardRows.Length == 1)
                        {
                            primaryCardId = Convert.ToInt32(sortedCardRows[0]["card_id"]);
                        }
                    }
                }
            }
            if (primaryCardId > -1)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.Parameters.AddWithValue("@cardId", primaryCardId);
                cmd.Parameters.AddWithValue("@siteId", siteId);
                cmd.Parameters.AddWithValue("@loginId", this.loginId);
                cmd.CommandText = @"update cards
                                        set primarycard = 'Y',
                                            last_update_time = getdate(),
                                            LastUpdatedBy = @loginId
                                      where customer_id  = @customerId and card_id = @cardId 
                                        and (site_id = @siteId OR  @siteId = -1)  ";
                cmd.ExecuteNonQuery();
            }
            cmd.Dispose();
            log.LogMethodExit(primaryCardId);
            return primaryCardId;
        }

        void SetPrimaryNVIPCard(bool linkCustomer, int customerId, int vipMembership, int cardId, int siteId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(customerId, vipMembership, cardId, siteId, sqlTrx);

            if (cardId > -1)
            {
                SqlCommand cmd = utilities.getCommand(sqlTrx);
                cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.Parameters.AddWithValue("@cardId", cardId);
                cmd.Parameters.AddWithValue("@vipMembership", vipMembership);
                cmd.Parameters.AddWithValue("@siteId", siteId);
                cmd.Parameters.AddWithValue("@loginId", this.loginId);
                if (linkCustomer)
                {
                    cmd.CommandText = @"update cards
                                                   set customer_id  = @customerId , primarycard = 'Y', vip_customer = (CASE WHEN @vipMembership = 1 THEN 'Y' ELSE 'N' END),
                                                       last_update_time = getdate(),
                                                       LastUpdatedBy = @loginId
                                               where card_id = @cardId 
                                                 and (site_id = @siteId OR  @siteId = -1) ";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = @"update cards
                                                   set primarycard = 'Y', vip_customer = (CASE WHEN @vipMembership = 1 THEN 'Y' ELSE 'N' END),
                                                       last_update_time = getdate(),
                                                       LastUpdatedBy = @loginId
                                                where customer_id  = @customerId 
                                                  and card_id = @cardId 
                                                  and (site_id = @siteId OR  @siteId = -1) ";
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
            }
            log.LogMethodExit();
        }

        int CreateCustomerRecord(int cardId, string cardNumber, DataTable customerSetup, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(cardId, customerSetup, sqlTrx);
            int customerId = -1;
            bool emailOrPhoneMandatory = false;
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string city = "";
            int stateId = -1;
            int countryId = -1;
            string pinCode = "";
            string emailId = "";
            string phoneNumber = "";
            string secondaryPhoneNumber = "";
            string fBUserId = "";
            string fBAccessToken = "";
            string tWAccessToken = "";
            string tWAccessSecret = "";
            string weChatAccessToken = "";
            AddressType addressType = AddressType.NONE;
            //string addressType = "";
            string customerUserNameLength = "";
            string backwardCompatibility = "";
            string custNameValidation = "";
            string phoneNoWidth = "";
            bool createFBContact = false;
            bool createTwContact = false;
            bool createPhone1Contact = false;
            bool createPhone2Contact = false;
            bool createEmailContact = false;
            bool createWeChatContact = false;
            bool createAddress = false;
            CustomerDTO customerDTO = new CustomerDTO();
            customerDTO.Password = "";
            //string dateOfBirth = "";
            //string gender = "";
            //string anniversary = "";
            //string notes = cardNumber;
            //string company = "";
            //string designation = "";
            //string uniqueIdentifier = "";
            //string userName = "";
            //string rightHanded = "";
            //string teamUser = "";
            //string lastName = "";
            //string photoURL = "";
            //string title = "";
            //string customerName = "";
            //string channel = "";
            //string taxCode = "";
            //string middleName = "";

            foreach (DataRow customerSetupRow in customerSetup.Rows)
            {
                switch (customerSetupRow["default_value_name"].ToString())
                {
                    case "CUSTOMER_EMAIL_OR_PHONE_MANDATORY":
                        if (customerSetupRow["default_value"].ToString() == "Y")
                        { emailOrPhoneMandatory = true; }
                        break;
                    case "ADDRESS1":
                        createAddress = true;
                        address1 = cardNumber + "Add1"; break;
                    case "ADDRESS2":
                        createAddress = true;
                        address2 = cardNumber + "Add2"; break;
                    case "ADDRESS3":
                        createAddress = true;
                        address3 = cardNumber + "Add3"; break;
                    case "CITY":
                        createAddress = true;
                        city = cardNumber + "City"; break;
                    case "STATE":
                        createAddress = true;
                        stateId = this.stateId; break;
                    case "COUNTRY":
                        createAddress = true;
                        countryId = this.countryId; break;
                    case "PIN":
                        createAddress = true;
                        pinCode = "123456"; break;
                    case "EMAIL":
                        createEmailContact = true;
                        emailId = cardNumber + "@test.com"; break;
                    case "BIRTH_DATE":
                        customerDTO.DateOfBirth = Convert.ToDateTime("01-Jan-1901");
                        //dateOfBirth = "01-Jan-1901";
                        break;
                    case "GENDER":
                        customerDTO.Gender = "M";
                        //gender = "M";
                        break;
                    case "ANNIVERSARY":
                        customerDTO.Anniversary = Convert.ToDateTime("01-Jan-1901");
                        //anniversary = "01-Jan-1901";
                        break;
                    case "CONTACT_PHONE":
                        createPhone1Contact = true;
                        phoneNumber = "1237894560"; break;
                    case "CONTACT_PHONE2":
                        createPhone2Contact = true;
                        secondaryPhoneNumber = "1237894560"; break;
                    case "NOTES":
                        customerDTO.Notes = cardNumber;
                        //notes = cardNumber;
                        break;
                    case "COMPANY":
                        customerDTO.Company = cardNumber;
                        //company = cardNumber;
                        break;
                    case "DESIGNATION":
                        customerDTO.Designation = "NA";
                        //designation = "NA";
                        break;
                    case "UNIQUE_ID":
                        customerDTO.UniqueIdentifier = cardNumber;
                        //uniqueIdentifier = "";
                        break;
                    case "USERNAME":
                        customerDTO.UserName = cardNumber;
                        //userName = cardNumber;
                        break;
                    case "FBUSERID":
                        createFBContact = true;
                        fBUserId = cardNumber; break;
                    case "FBACCESSTOKEN":
                        createFBContact = true;
                        fBAccessToken = "CARDTYPEMIGRATION"; break;
                    case "TWACCESSTOKEN":
                        createTwContact = true;
                        tWAccessToken = cardNumber; break;
                    case "TWACCESSSECRET":
                        createTwContact = true;
                        tWAccessSecret = "CARDTYPEMIGRATION"; break;
                    case "RIGHTHANDED":
                        customerDTO.RightHanded = false;
                        //rightHanded = "N";
                        break;
                    case "TEAMUSER":
                        customerDTO.TeamUser = false;
                        //teamUser = "N";
                        break;
                    case "LAST_NAME":
                        customerDTO.LastName = cardNumber + "LastName";
                        //lastName = cardNumber + "LastName";
                        break;
                    case "CUSTOMER_PHOTO":
                        customerDTO.PhotoURL = customerDTO.IdProofFileURL = "Semnox_CardTypeMigration.bmp";
                        //photoURL = "Semnox_CardTypeMigration.bmp";
                        break;
                    case "TITLE":
                        customerDTO.Title = "Mr.";
                        //title = "Mr.";
                        break;
                    case "CUSTOMER_NAME":
                        customerDTO.FirstName = cardNumber;
                        //customerName = cardNumber;
                        break;
                    case "WECHAT_ACCESS_TOKEN":
                        createWeChatContact = true;
                        weChatAccessToken = cardNumber; break;
                    case "CHANNEL":
                        customerDTO.Channel = "Default";
                        //channel = "Default";
                        break;
                    case "TAXCODE":
                        customerDTO.TaxCode = "CARDTYPEMIGRATION";
                        //taxCode = "CARDTYPEMIGRATION";
                        break;
                    case "ADDRESS_TYPE":
                        addressType = AddressType.HOME;
                        //addressType = "HOME";
                        break;
                    case "MIDDLE_NAME":
                        customerDTO.MiddleName = cardNumber + "MiddleName";
                        //middleName = cardNumber + "MiddleName";
                        break;
                    case "CUSTOMER_USERNAME_LENGTH":
                        customerUserNameLength = customerSetupRow["default_value"].ToString(); break;
                    case "ENABLE_CUSTOMER_BACKWARD_COMPATIBILITY":
                        backwardCompatibility = customerSetupRow["default_value"].ToString(); break;
                    case "CUSTOMER_NAME_VALIDATION":
                        custNameValidation = customerSetupRow["default_value"].ToString(); break;
                    case "CUSTOMER_PHONE_NUMBER_WIDTH":
                        phoneNoWidth = customerSetupRow["default_value"].ToString(); break;
                }
            }
            if (emailOrPhoneMandatory)
            {
                if (createEmailContact == false)
                {
                    createEmailContact = true;
                    emailId = cardNumber + "@test.com";
                }
                if (createPhone1Contact == false)
                {
                    createPhone1Contact = true;
                    phoneNumber = "1237894560";
                }
            }

            int customerUserNameLenValue = 0;
            if (!String.IsNullOrEmpty(customerUserNameLength))
            {
                try { customerUserNameLenValue = Convert.ToInt32(customerUserNameLength); } catch { customerUserNameLenValue = 0; }
            }
            if (customerUserNameLenValue > 0)
            {

                if (!String.IsNullOrEmpty(customerDTO.UserName))
                {
                    string uName = customerDTO.UserName;
                    if (uName.Length >= customerUserNameLenValue)
                        uName = uName.Substring(0, customerUserNameLenValue);

                    customerDTO.UserName = uName;
                }
            }
            if (custNameValidation == "Y")
            {
                customerDTO.FirstName = "FirstName";

                if (!String.IsNullOrEmpty(customerDTO.LastName))
                    customerDTO.LastName = "LastName";

                if (!String.IsNullOrEmpty(customerDTO.MiddleName))
                    customerDTO.MiddleName = "MiddleName";
            }

            if (!String.IsNullOrEmpty(phoneNoWidth) && Convert.ToInt32(phoneNoWidth) > 0)
            {
                if (createPhone1Contact)
                    phoneNumber = AdjustPhoneNumberLength(phoneNumber, Convert.ToInt32(phoneNoWidth));
                if (createPhone2Contact)
                    secondaryPhoneNumber = AdjustPhoneNumberLength(secondaryPhoneNumber, Convert.ToInt32(phoneNoWidth));
            }

            if (String.IsNullOrEmpty(customerDTO.Notes))
                customerDTO.Notes = cardNumber;

            //customerId = CreateCustomer(cardNumber, title, customerName, middleName, lastName, notes, dateOfBirth, gender, anniversary,
            //                      photoURL, rightHanded, teamUser, uniqueIdentifier, taxCode, company, designation,
            //                      userName, this.loginId, "", this.siteId, phoneNumber, secondaryPhoneNumber, emailId,
            //                      fBUserId, fBAccessToken, tWAccessToken, tWAccessSecret, weChatAccessToken, address1,
            //                      address2, address3, pinCode, city, stateId, countryId, addressType, channel,
            //                      customerUserNameLength, backwardCompatibility, custNameValidation, phoneNoWidth, emailOrPhoneMandatory, createEmailContact, createPhone1Contact,
            //                      createPhone2Contact, createAddress, createFBContact, createTwContact, createWeChatContact, sqlTrx);

            if (createAddress)
            {
                AddressDTO addressDTO = new AddressDTO();
                addressDTO.AddressType = addressType;

                if (!String.IsNullOrEmpty(address1))
                    addressDTO.Line1 = address1;

                if (!String.IsNullOrEmpty(address2))
                    addressDTO.Line2 = address2;

                if (!String.IsNullOrEmpty(address3))
                    addressDTO.Line3 = address3;

                if (!String.IsNullOrEmpty(city))
                    addressDTO.City = city;

                if (stateId != -1)
                    addressDTO.StateId = stateId;

                if (countryId != -1)
                    addressDTO.CountryId = countryId;

                if (!String.IsNullOrEmpty(pinCode))
                    addressDTO.PostalCode = pinCode;

                customerDTO.AddressDTOList = new List<AddressDTO>();
                customerDTO.AddressDTOList.Add(addressDTO);
            }

            customerDTO.ContactDTOList = new List<ContactDTO>();
            if (createEmailContact)
            {
                ContactDTO contactDTO = new ContactDTO();
                contactDTO.Attribute1 = emailId;
                contactDTO.ContactType = ContactType.EMAIL;
                customerDTO.ContactDTOList.Add(contactDTO);
            }

            if (createPhone1Contact)
            {
                ContactDTO contactDTO = new ContactDTO();
                contactDTO.Attribute1 = phoneNumber;
                contactDTO.ContactType = ContactType.PHONE;
                customerDTO.ContactDTOList.Add(contactDTO);
            }

            if (createPhone2Contact)
            {
                ContactDTO contactDTO = new ContactDTO();
                contactDTO.Attribute1 = secondaryPhoneNumber;
                contactDTO.ContactType = ContactType.PHONE;
                customerDTO.ContactDTOList.Add(contactDTO);
            }

            if (createFBContact)
            {
                ContactDTO contactDTO = new ContactDTO();
                contactDTO.Attribute1 = fBUserId;
                contactDTO.Attribute2 = fBAccessToken;
                contactDTO.ContactType = ContactType.FACEBOOK;
                customerDTO.ContactDTOList.Add(contactDTO);
            }

            if (createTwContact)
            {
                ContactDTO contactDTO = new ContactDTO();
                contactDTO.Attribute1 = tWAccessToken;
                contactDTO.Attribute2 = tWAccessSecret;
                contactDTO.ContactType = ContactType.TWITTER;
                customerDTO.ContactDTOList.Add(contactDTO);
            }

            if (createWeChatContact)
            {
                ContactDTO contactDTO = new ContactDTO();
                contactDTO.Attribute1 = weChatAccessToken;
                contactDTO.ContactType = ContactType.WECHAT;
                customerDTO.ContactDTOList.Add(contactDTO);
            }

            CustomerBL customerBL = new CustomerBL(executionContext, customerDTO);
            customerBL.Save(sqlTrx);
            customerId = customerBL.CustomerDTO.Id;
            log.LogMethodExit(customerId);
            return customerId;
        }

        //int CreateCustomerRecord(int cardId, string cardNumber, DataTable customerSetup, SqlTransaction sqlTrx)
        //{
        //    log.LogMethodEntry(cardId, customerSetup, sqlTrx);
        //    int customerId = -1;
        //    bool emailOrPhoneMandatory = false;
        //    string address1 = "";
        //    string address2 = "";
        //    string address3 = "";
        //    string city = "";
        //    int stateId = -1;
        //    int countryId = -1;
        //    string pinCode = "";
        //    string emailId = "";
        //    string phoneNumber = "";
        //    string secondaryPhoneNumber = "";
        //    string fBUserId = "";
        //    string fBAccessToken = "";
        //    string tWAccessToken = "";
        //    string tWAccessSecret = "";
        //    string weChatAccessToken = "";
        //    // AddressType addressType = AddressType.NONE; 
        //    string addressType = "";
        //    string customerUserNameLength = "";
        //    string backwardCompatibility = "";
        //    string custNameValidation = "";
        //    string phoneNoWidth = "";
        //    bool createFBContact = false;
        //    bool createTwContact = false;
        //    bool createPhone1Contact = false;
        //    bool createPhone2Contact = false;
        //    bool createEmailContact = false;
        //    bool createWeChatContact = false;
        //    bool createAddress = false;
        //    //CustomerDTO customerDTO = new CustomerDTO();
        //    //customerDTO.Password = "";
        //    string dateOfBirth = "";
        //    string gender = "";
        //    string anniversary = "";
        //    string notes = cardNumber;
        //    string company = "";
        //    string designation = "";
        //    string uniqueIdentifier = "";
        //    string userName = "";

        //    string rightHanded = "";
        //    string teamUser = "";
        //    string lastName = "";
        //    string photoURL = "";
        //    string title = "";
        //    string customerName = "";
        //    string channel = "";
        //    string taxCode = "";
        //    string middleName = "";

        //    foreach (DataRow customerSetupRow in customerSetup.Rows)
        //    {
        //        switch (customerSetupRow["default_value_name"].ToString())
        //        {
        //            case "CUSTOMER_EMAIL_OR_PHONE_MANDATORY":
        //                if (customerSetupRow["default_value"].ToString() == "Y")
        //                { emailOrPhoneMandatory = true; }
        //                break;
        //            case "ADDRESS1":
        //                createAddress = true;
        //                address1 = cardNumber + "Add1"; break;
        //            case "ADDRESS2":
        //                createAddress = true;
        //                address2 = cardNumber + "Add2"; break;
        //            case "ADDRESS3":
        //                createAddress = true;
        //                address3 = cardNumber + "Add3"; break;
        //            case "CITY":
        //                createAddress = true;
        //                city = cardNumber + "City"; break;
        //            case "STATE":
        //                createAddress = true;
        //                stateId = this.stateId; break;
        //            case "COUNTRY":
        //                createAddress = true;
        //                countryId = this.countryId; break;
        //            case "PIN":
        //                createAddress = true;
        //                pinCode = "123456"; break;
        //            case "EMAIL":
        //                createEmailContact = true;
        //                emailId = cardNumber + "@test.com"; break;
        //            case "BIRTH_DATE":
        //                //customerDTO.DateOfBirth = Convert.ToDateTime("01-Jan-1901"); 
        //                dateOfBirth = "01-Jan-1901";
        //                break;
        //            case "GENDER":
        //                //customerDTO.Gender = "M";
        //                gender = "M";
        //                break;
        //            case "ANNIVERSARY":
        //                //customerDTO.Anniversary = Convert.ToDateTime("01-Jan-1901"); 
        //                anniversary = "01-Jan-1901";
        //                break;
        //            case "CONTACT_PHONE":
        //                createPhone1Contact = true;
        //                phoneNumber = "1237894560"; break;
        //            case "CONTACT_PHONE2":
        //                createPhone2Contact = true;
        //                secondaryPhoneNumber = "1237894560"; break;
        //            case "NOTES":
        //                // customerDTO.Notes = cardNumber; 
        //                notes = cardNumber;
        //                break;
        //            case "COMPANY":
        //                //customerDTO.Company = cardNumber; 
        //                company = cardNumber;
        //                break;
        //            case "DESIGNATION":
        //                // customerDTO.Designation = "NA";
        //                designation = "NA";
        //                break;
        //            case "UNIQUE_ID":
        //                //customerDTO.UniqueIdentifier = cardNumber;
        //                uniqueIdentifier = "";
        //                break;
        //            case "USERNAME":
        //                //customerDTO.UserName = cardNumber;
        //                userName = cardNumber;
        //                break;
        //            case "FBUSERID":
        //                createFBContact = true;
        //                fBUserId = cardNumber; break;
        //            case "FBACCESSTOKEN":
        //                createFBContact = true;
        //                fBAccessToken = "CARDTYPEMIGRATION"; break;
        //            case "TWACCESSTOKEN":
        //                createTwContact = true;
        //                tWAccessToken = cardNumber; break;
        //            case "TWACCESSSECRET":
        //                createTwContact = true;
        //                tWAccessSecret = "CARDTYPEMIGRATION"; break;
        //            case "RIGHTHANDED":
        //                //customerDTO.RightHanded = false;
        //                rightHanded = "N";
        //                break;
        //            case "TEAMUSER":
        //                //customerDTO.TeamUser = false;
        //                teamUser = "N";
        //                break;
        //            case "LAST_NAME":
        //                //customerDTO.LastName = cardNumber+"LastName";
        //                lastName = cardNumber + "LastName";
        //                break;
        //            case "CUSTOMER_PHOTO":
        //                //customerDTO.PhotoURL = customerDTO.IdProofFileURL = "Semnox_CardTypeMigration.bmp"; 
        //                photoURL = "Semnox_CardTypeMigration.bmp";
        //                break;
        //            case "TITLE":
        //                //customerDTO.Title = "Mr."; 
        //                title = "Mr.";
        //                break;
        //            case "CUSTOMER_NAME":
        //                //customerDTO.FirstName = cardNumber; 
        //                customerName = cardNumber;
        //                break;
        //            case "WECHAT_ACCESS_TOKEN":
        //                createWeChatContact = true;
        //                weChatAccessToken = cardNumber; break;
        //            case "CHANNEL":
        //                // customerDTO.Channel = "Default"; 
        //                channel = "Default";
        //                break;
        //            case "TAXCODE":
        //                // customerDTO.TaxCode = "CARDTYPEMIGRATION";
        //                taxCode = "CARDTYPEMIGRATION";
        //                break;
        //            case "ADDRESS_TYPE":
        //                //addressType = AddressType.HOME;
        //                addressType = "HOME";
        //                break;
        //            case "MIDDLE_NAME":
        //                //customerDTO.MiddleName = cardNumber+"MiddleName";
        //                middleName = cardNumber + "MiddleName";
        //                break;
        //            case "CUSTOMER_USERNAME_LENGTH":
        //                customerUserNameLength = customerSetupRow["default_value"].ToString(); break;
        //            case "ENABLE_CUSTOMER_BACKWARD_COMPATIBILITY":
        //                backwardCompatibility = customerSetupRow["default_value"].ToString(); break;
        //            case "CUSTOMER_NAME_VALIDATION":
        //                custNameValidation = customerSetupRow["default_value"].ToString(); break;
        //            case "CUSTOMER_PHONE_NUMBER_WIDTH":
        //                phoneNoWidth = customerSetupRow["default_value"].ToString(); break;
        //        }
        //    }
        //    //if (emailOrPhoneMandatory)
        //    //{
        //    //    if (createEmailContact == false)
        //    //    {
        //    //        createEmailContact = true;
        //    //        emailId = cardNumber+"@test.com";
        //    //    }
        //    //    if(createPhone1Contact == false)
        //    //    {
        //    //        createPhone1Contact = true;
        //    //        phoneNumber = "1237894560";
        //    //    }
        //    //}

        //    //int customerUserNameLenValue = 0;
        //    //if (!String.IsNullOrEmpty(customerUserNameLength))
        //    //{
        //    //    try { customerUserNameLenValue = Convert.ToInt32(customerUserNameLength); } catch { customerUserNameLenValue = 0; }
        //    //}
        //    //if (customerUserNameLenValue > 0)
        //    //{

        //    //    if (!String.IsNullOrEmpty(customerDTO.UserName))
        //    //    {
        //    //        string uName = customerDTO.UserName;
        //    //        if (uName.Length >= customerUserNameLenValue)
        //    //            uName = uName.Substring(0, customerUserNameLenValue);

        //    //        customerDTO.UserName = uName;
        //    //    }
        //    //}
        //    //if (custNameValidation == "Y")
        //    //{
        //    //    customerDTO.FirstName = "FirstName";

        //    //    if (!String.IsNullOrEmpty(customerDTO.LastName))
        //    //        customerDTO.LastName = "LastName";

        //    //    if (!String.IsNullOrEmpty(customerDTO.MiddleName))
        //    //        customerDTO.MiddleName = "MiddleName";
        //    //}

        //    //if (!String.IsNullOrEmpty(phoneNoWidth) && Convert.ToInt32(phoneNoWidth) > 0)
        //    //{
        //    //    if (createPhone1Contact)
        //    //        phoneNumber = AdjustPhoneNumberLength(phoneNumber, Convert.ToInt32(phoneNoWidth));
        //    //    if (createPhone2Contact)
        //    //        secondaryPhoneNumber = AdjustPhoneNumberLength(secondaryPhoneNumber, Convert.ToInt32(phoneNoWidth));
        //    //}

        //    //if (String.IsNullOrEmpty(customerDTO.Notes))
        //    //     customerDTO.Notes = cardNumber;

        //    customerId = CreateCustomer(cardNumber, title, customerName, middleName, lastName, notes, dateOfBirth, gender, anniversary,
        //                          photoURL, rightHanded, teamUser, uniqueIdentifier, taxCode, company, designation,
        //                          userName, this.loginId, "", this.siteId, phoneNumber, secondaryPhoneNumber, emailId,
        //                          fBUserId, fBAccessToken, tWAccessToken, tWAccessSecret, weChatAccessToken, address1,
        //                          address2, address3, pinCode, city, stateId, countryId, addressType, channel,
        //                          customerUserNameLength, backwardCompatibility, custNameValidation, phoneNoWidth, emailOrPhoneMandatory, createEmailContact, createPhone1Contact,
        //                          createPhone2Contact, createAddress, createFBContact, createTwContact, createWeChatContact, sqlTrx);

        //    //if (createAddress)
        //    //{
        //    //    AddressDTO addressDTO = new AddressDTO();
        //    //    addressDTO.AddressType = addressType;

        //    //    if (!String.IsNullOrEmpty(address1))
        //    //        addressDTO.Line1 = address1;

        //    //    if (!String.IsNullOrEmpty(address2))
        //    //        addressDTO.Line2 = address2;

        //    //    if (!String.IsNullOrEmpty(address3))
        //    //        addressDTO.Line3 = address3;

        //    //    if (!String.IsNullOrEmpty(city))
        //    //        addressDTO.City = city;

        //    //    if (stateId != -1)
        //    //        addressDTO.StateId = stateId;

        //    //    if (countryId != -1)
        //    //        addressDTO.CountryId = countryId;

        //    //    if (!String.IsNullOrEmpty(pinCode))
        //    //        addressDTO.PostalCode = pinCode;

        //    //    customerDTO.AddressDTOList = new List<AddressDTO>();
        //    //    customerDTO.AddressDTOList.Add(addressDTO);
        //    //}

        //    //customerDTO.ContactDTOList = new List<ContactDTO>();
        //    //if (createEmailContact)
        //    //{
        //    //    ContactDTO contactDTO = new ContactDTO();
        //    //    contactDTO.Attribute1 = emailId;
        //    //    contactDTO.ContactType = ContactType.EMAIL;
        //    //    customerDTO.ContactDTOList.Add(contactDTO);
        //    //}

        //    //if (createPhone1Contact)
        //    //{
        //    //    ContactDTO contactDTO = new ContactDTO();
        //    //    contactDTO.Attribute1 = phoneNumber;
        //    //    contactDTO.ContactType = ContactType.PHONE;
        //    //    customerDTO.ContactDTOList.Add(contactDTO);
        //    //}

        //    //if (createPhone2Contact)
        //    //{
        //    //    ContactDTO contactDTO = new ContactDTO();
        //    //    contactDTO.Attribute1 = secondaryPhoneNumber;
        //    //    contactDTO.ContactType = ContactType.PHONE;
        //    //    customerDTO.ContactDTOList.Add(contactDTO);
        //    //}

        //    //if (createFBContact)
        //    //{
        //    //    ContactDTO contactDTO = new ContactDTO();
        //    //    contactDTO.Attribute1 = fBUserId;
        //    //    contactDTO.Attribute2 = fBAccessToken;
        //    //    contactDTO.ContactType = ContactType.FACEBOOK;
        //    //    customerDTO.ContactDTOList.Add(contactDTO);
        //    //}

        //    //if (createTwContact)
        //    //{
        //    //    ContactDTO contactDTO = new ContactDTO();
        //    //    contactDTO.Attribute1 = tWAccessToken;
        //    //    contactDTO.Attribute2 = tWAccessSecret;
        //    //    contactDTO.ContactType = ContactType.TWITTER;
        //    //    customerDTO.ContactDTOList.Add(contactDTO);
        //    //}

        //    //if (createWeChatContact)
        //    //{
        //    //    ContactDTO contactDTO = new ContactDTO();
        //    //    contactDTO.Attribute1 = weChatAccessToken; 
        //    //    contactDTO.ContactType = ContactType.WECHAT;
        //    //    customerDTO.ContactDTOList.Add(contactDTO);
        //    //}

        //    //CustomerBL customerBL = new CustomerBL(executionContext, customerDTO);
        //    //customerBL.Save(sqlTrx);
        //    //customerId = customerBL.CustomerDTO.Id;
        //    log.LogMethodExit(customerId);
        //    return customerId;
        //}


        void SaveImageFile(string destinationFileUrl, byte[] file)
        {
            log.LogMethodEntry(destinationFileUrl, "file");
            try
            {
                string query = @"exec SaveBinaryDataToFile @bytes, @FileName";
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@bytes", file), dataAccessHandler.GetSQLParameter("@FileName", destinationFileUrl) });
            }
            catch (Exception ex)
            {
                log.Error("Error occured while saving the file", ex);
                throw ex;
            }
            log.LogMethodExit();
        }

        string ReplaceNumbers(string text)
        {
            string output = Regex.Replace(text, @"\d{2,}", "");
            output = Regex.Replace(output, @"\d", "");
            if (output.Length < 3)
                output = output + "ABC";
            return output;
        }

        string AdjustPhoneNumberLength(string contactPhone, int phoneNoWidth)
        {
            log.LogMethodEntry(contactPhone, phoneNoWidth);
            if (contactPhone.Length == phoneNoWidth)
            {
                // contactPhone = contactPhone;
            }
            else if (contactPhone.Length >= phoneNoWidth)
                contactPhone = contactPhone.Substring(0, phoneNoWidth);
            else
            {
                Random random = new Random();
                do
                {
                    contactPhone = contactPhone + random.Next(9).ToString();
                } while (contactPhone.Length != phoneNoWidth);
            }
            log.LogMethodExit(contactPhone);
            return contactPhone;


        }

        void CreateDailyCardBalance(int newCardId, int newCustomerId, DateTime currentDate, string loginId, int siteId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(newCardId, newCustomerId, currentDate, loginId, siteId, sqlTrx);
            double totalTickets;
            double earnedTickets;
            double totalLoyalty;
            double redeemableLoyalty;
            DateTime dailyCardBalanceDate = currentDate.Date;
            SqlCommand cmd = utilities.getCommand(sqlTrx);
            cmd.Parameters.AddWithValue("@newCardId", newCardId);
            cmd.Parameters.AddWithValue("@siteId", siteId);

            cmd.CommandText = @"select ISNULL(c.ticket_count + (SELECT SUM(CreditPlusBalance)
                                                                        from CardCreditPlus cp
                                                                        where cp.Card_id  =c.card_id
                                                                        and cp.CreditPlusType = 'T'
                                                                        and (ISNULL(cp.PeriodTo, getdate()) <= getdate())
                                                                        ),0) as totalTickets,
                                                ISNULL(c.ticket_count + (SELECT SUM(CreditPlusBalance)
                                                                        from CardCreditPlus cp
                                                                        where cp.Card_id  =c.card_id
                                                                        and cp.CreditPlusType = 'T'
                                                                        and (ISNULL(cp.PeriodTo, getdate()) <= getdate())
                                                                        and ISNULL(cp.MembershipRewardsId,-1) = -1
                                                                        ),0) as totalEarnedTickets,
                                                ISNULL(c.loyalty_points + (SELECT SUM(CreditPlusBalance)
                                                                        from CardCreditPlus cp
                                                                        where cp.Card_id  =c.card_id
                                                                        and cp.CreditPlusType = 'L'
                                                                        and (ISNULL(cp.PeriodTo, getdate()) <= getdate())
                                                                        ) ,0)as totalLoyalty,
                                                ISNULL(c.loyalty_points + (SELECT SUM(CreditPlusBalance)
                                                                        from CardCreditPlus cp
                                                                        where cp.Card_id  =c.card_id
                                                                        and cp.CreditPlusType = 'L'
                                                                        and (ISNULL(cp.PeriodTo, getdate()) <= getdate())
                                                                        and ISNULL(cp.MembershipRewardsId,-1) = -1
                                                                        and ISNULL(cp.ForMembershipOnly,'N') = 'N'
                                                                        ),0) as totalRedeemableLoyalty
                                        from cards c  
                                        where c.card_id = @newCardId 
                                            and (C.site_id = @siteId OR @siteId = -1) ";

            DataTable balanceDT = new DataTable();
            SqlDataAdapter daDCB = new SqlDataAdapter(cmd);
            daDCB.Fill(balanceDT);

            if (balanceDT.Rows.Count > 0)
            {

                totalTickets = Convert.ToDouble(balanceDT.Rows[0]["totalTickets"]);
                earnedTickets = Convert.ToDouble(balanceDT.Rows[0]["totalEarnedTickets"]);
                totalLoyalty = Convert.ToDouble(balanceDT.Rows[0]["totalLoyalty"]);
                redeemableLoyalty = Convert.ToDouble(balanceDT.Rows[0]["totalRedeemableLoyalty"]);


                for (int i = 0; i < 2; i++)
                {
                    string creditPlusAttribute = "";
                    double totalCreditPlusBalance = 0;
                    double earnedCreditPlusBalance = 0;
                    if (i == 0)
                    {
                        creditPlusAttribute = "T";
                        totalCreditPlusBalance = totalTickets;
                        earnedCreditPlusBalance = earnedTickets;
                    }
                    else if (i == 1)
                    {
                        creditPlusAttribute = "L";
                        totalCreditPlusBalance = totalLoyalty;
                        earnedCreditPlusBalance = redeemableLoyalty;
                    }

                    cmd.CommandText = @"INSERT INTO DailyCardBalance 
                                        ( 
                                             [CustomerId]
                                            ,[CardId]
                                            ,[CardBalanceDate]
                                            ,[CreditPlusAttribute]
                                            ,[TotalCreditPlusBalance]
                                            ,[EarnedCreditPlusBalance]
                                            ,[CreatedBy]
                                            ,[CreationDate]
                                            ,[LastUpdatedBy]
                                            ,[LastupdateDate]
                                            ,[Guid]
                                            ,[site_id]  
                                        ) 
                                VALUES 
                                        (
                                            @customerId ,
                                            @cardId ,
                                            @cardBalanceDate,
                                            @creditPlusAttribute,
                                            @totalCreditPlusBalance,
                                            @earnedCreditPlusBalance, 
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE(),
                                            NEWID(),
                                            @siteId 
                                        )";
                    cmd.Parameters.Clear();
                    if (newCardId == -1)
                    {
                        cmd.Parameters.AddWithValue("@CardId", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CardId", newCardId);
                    }
                    if (newCustomerId == -1)
                    {
                        cmd.Parameters.AddWithValue("@customerId", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@customerId", newCustomerId);
                    }
                    cmd.Parameters.AddWithValue("@cardBalanceDate", dailyCardBalanceDate);
                    cmd.Parameters.AddWithValue("@creditPlusAttribute", creditPlusAttribute);
                    cmd.Parameters.AddWithValue("@totalCreditPlusBalance", totalCreditPlusBalance);
                    cmd.Parameters.AddWithValue("@earnedCreditPlusBalance", earnedCreditPlusBalance);
                    if (siteId == -1)
                    {
                        cmd.Parameters.AddWithValue("@siteId", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@siteId", siteId);
                    }
                    cmd.Parameters.AddWithValue("@createdBy", loginId);
                    cmd.Parameters.AddWithValue("@lastUpdatedBy", loginId);

                    cmd.ExecuteNonQuery();
                }
            }

            cmd.Dispose();
            log.LogMethodExit();
        }

        public void DisableEnableDBTriggers(bool enableTRigger)
        {
            log.LogMethodEntry(enableTRigger);

            DataTable dtTriggers = utilities.executeDataTable(@"SELECT distinct upper(OBJECT_NAME(parent_id)) tableName FROM sys.triggers
                                                                 where type='TR'
                                                                   and upper(OBJECT_NAME(parent_id)) in ('CARDGAMES', 'CARDDISCOUNTS', 'CARDCREDITPLUS', 'MEMBERSHIPPROGRESSION', 
                                                                                  'CUSTOMERS', 'CARDTYPE', 'CARDS', 'PROFILE', 'CONTACT', 'ADDRESS', 'DAILYCARDBALANCE' )");

            if (dtTriggers.Rows.Count > 0)
            {
                if (enableTRigger)
                {
                    foreach (DataRow TableEntry in dtTriggers.Rows)
                    {
                        utilities.executeNonQuery("ALTER TABLE " + TableEntry["tableName"].ToString() + " ENABLE TRIGGER ALL");
                    }
                }
                else
                {
                    foreach (DataRow TableEntry in dtTriggers.Rows)
                    {
                        utilities.executeNonQuery("ALTER TABLE " + TableEntry["tableName"].ToString() + " DISABLE TRIGGER ALL");
                    }
                }
            }
            log.LogMethodExit();
        }

        //int CreateCustomer(string cardNumber, string title, string customerName, string middleName, string lastName, string notes, string birthDate, string gender, string anniversary,
        //                           string customerPhoto, string rightHanded, string teamUser, string uniqueId, string taxCode, string company, string designation,
        //                           string userName, string loginId, string password, int siteId, string contactPhone, string contactPhone2, string email,
        //                           string fbUserId, string fbAccessToken, string twAccessToken, string twAccessSecret, string weChatAccessToken, string address1,
        //                           string address2, string address3, string pinCode, string city, int stateId, int countryId, string addressType, string channel,
        //                           string customerUserNameLength, string backwardCompatibility, string custNameValidation, string phoneNoWidth, bool emailOrPhoneMandatory, bool createEmailContact,
        //                           bool createPhone1Contact, bool createPhone2Contact, bool createAddress, bool createFBContact, bool createTwContact, bool createWeChatContact,
        //                           SqlTransaction sqlTrx)
        //{
        //    log.LogMethodEntry();


        //    if (emailOrPhoneMandatory)
        //    {
        //        if (createEmailContact == false)
        //        {
        //            createEmailContact = true;
        //            email = cardNumber + "@test.com";
        //        }
        //        if (createPhone1Contact == false)
        //        {
        //            createPhone1Contact = true;
        //            contactPhone = "1237894560";
        //        }
        //    }


        //    int customerUserNameLenValue = 0;
        //    if (!String.IsNullOrEmpty(customerUserNameLength))
        //    {
        //        try { customerUserNameLenValue = Convert.ToInt32(customerUserNameLength); } catch { customerUserNameLenValue = 0; }
        //    }
        //    if (customerUserNameLenValue > 0)
        //    {
        //        if (!String.IsNullOrEmpty(userName))
        //        {
        //            if (userName.Length >= customerUserNameLenValue)
        //                userName = userName.Substring(0, customerUserNameLenValue);

        //        }
        //    }
        //    if (custNameValidation == "Y")
        //    {
        //         customerName = "FirstName";

        //        if (!String.IsNullOrEmpty(lastName))
        //            lastName = "LastName";

        //        if (!String.IsNullOrEmpty(middleName))
        //            middleName = "MiddleName";
        //    }

        //    if (!String.IsNullOrEmpty(phoneNoWidth) && Convert.ToInt32(phoneNoWidth) > 0)
        //    {
        //        if (!String.IsNullOrEmpty(contactPhone))
        //            contactPhone = AdjustPhoneNumberLength(contactPhone, Convert.ToInt32(phoneNoWidth));
        //        if (!String.IsNullOrEmpty(contactPhone2))
        //            contactPhone2 = AdjustPhoneNumberLength(contactPhone2, Convert.ToInt32(phoneNoWidth));
        //    }


        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@passPhrase", this.passPhrase));
        //    if (String.IsNullOrEmpty(title))
        //        sqlParams.Add(new SqlParameter("@title", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@title", title));

        //    if (String.IsNullOrEmpty(customerName))
        //        sqlParams.Add(new SqlParameter("@customerName", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@customerName", customerName));

        //    if (String.IsNullOrEmpty(middleName))
        //        sqlParams.Add(new SqlParameter("@middleName", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@middleName", middleName));

        //    if (String.IsNullOrEmpty(lastName))
        //        sqlParams.Add(new SqlParameter("@lastName", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@lastName", lastName));

        //    if (String.IsNullOrEmpty(uniqueId))
        //        sqlParams.Add(new SqlParameter("@uniqueId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@uniqueId", uniqueId));

        //    if (String.IsNullOrEmpty(notes))
        //        sqlParams.Add(new SqlParameter("@notes", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@notes", notes));

        //    if (String.IsNullOrEmpty(birthDate))
        //        sqlParams.Add(new SqlParameter("@birthDate", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@birthDate", birthDate));

        //    if (String.IsNullOrEmpty(gender))
        //        sqlParams.Add(new SqlParameter("@gender", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@gender", gender));

        //    if (String.IsNullOrEmpty(anniversary))
        //        sqlParams.Add(new SqlParameter("@anniversary", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@anniversary", anniversary));

        //    if (String.IsNullOrEmpty(customerPhoto))
        //        sqlParams.Add(new SqlParameter("@customerPhoto", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@customerPhoto", customerPhoto));

        //    if (String.IsNullOrEmpty(rightHanded))
        //        sqlParams.Add(new SqlParameter("@rightHanded", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@rightHanded", customerPhoto));

        //    if (String.IsNullOrEmpty(teamUser))
        //        sqlParams.Add(new SqlParameter("@teamUser", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@teamUser", customerPhoto));

        //    //if (String.IsNullOrEmpty(uniqueId))
        //    //    sqlParams.Add(new SqlParameter("@uniqueId", DBNull.Value));
        //    //else
        //    //    sqlParams.Add(new SqlParameter("@uniqueId", uniqueId));

        //    if (String.IsNullOrEmpty(taxCode))
        //        sqlParams.Add(new SqlParameter("@taxCode", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@taxCode", taxCode));

        //    if (String.IsNullOrEmpty(company))
        //        sqlParams.Add(new SqlParameter("@company", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@company", company));

        //    if (String.IsNullOrEmpty(designation))
        //        sqlParams.Add(new SqlParameter("@designation", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@designation", designation));

        //    if (String.IsNullOrEmpty(userName))
        //        sqlParams.Add(new SqlParameter("@userName", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@userName", userName));

        //    if (String.IsNullOrEmpty(loginId))
        //        sqlParams.Add(new SqlParameter("@loginId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@loginId", loginId));

        //    if (String.IsNullOrEmpty(password))
        //        sqlParams.Add(new SqlParameter("@password", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@password", password));


        //    if (siteId == -1)
        //        sqlParams.Add(new SqlParameter("@siteId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@siteId", siteId));

        //    int profileId = -1;
        //    object profileObj = utilities.executeScalar(@"insert into Profile(ProfileTypeId, Title, FirstName, MiddleName, LastName, Notes, DateOfBirth, Gender, Anniversary,
        //                                                        PhotoURL, RightHanded, TeamUser, IdProofFileURL, UniqueId, TaxCode, Company, Designation, Username,
        //                                                        IsActive, CreatedBy, CreationDate, LastUpdatedBy, LastUpdateDate, Guid, Password, site_id)
        //                                                values ((SELECT Id FROM ProfileType WHERE NAME = 'PERSON'),
        //                                                        @title, 
        //                                                        @customerName,
        //                                                        @middleName,
        //                                                        @lastName, 
        //                                                        @notes,
        //                                                        EncryptByPassPhrase(@passPhrase, convert(nvarchar(max), @birthDate, 121)),
        //                                                        @gender,
        //                                                        EncryptByPassPhrase(@passPhrase, convert(nvarchar(max), @anniversary, 121)),
        //                                                        @customerPhoto, 
        //                                                        case when @rightHanded = 'Y' then 1 else 0 end,
        //                                                        case WHEN @teamUser = 'Y' then 1 else 0 end,
        //                                                        @customerPhoto,
        //                                                        EncryptByPassPhrase(@passPhrase, @uniqueId),
        //                                                        EncryptByPassPhrase(@passPhrase, @taxCode),
        //                                                        @company, 
        //                                                        @designation,
        //                                                        EncryptByPassPhrase(@passPhrase, @userName),
        //                                                        1,
        //                                                        @loginId,
        //                                                        getdate(),
        //                                                        @loginId,
        //                                                        getdate(),
        //                                                        newid(),
        //                                                        @password,
        //                                                        @siteId);  select @@identity", sqlTrx, sqlParams.ToArray());

        //    if (profileObj != null)
        //        profileId = Convert.ToInt32(profileObj);

        //    if (createPhone1Contact)
        //    {
        //        CreateCustomerContact(profileId, "PHONE", contactPhone, "", executionContext.GetUserId(), executionContext.GetSiteId(), sqlTrx);
        //    }
        //    if (createPhone2Contact)
        //    {
        //        CreateCustomerContact(profileId, "PHONE", contactPhone2, "", executionContext.GetUserId(), executionContext.GetSiteId(), sqlTrx);
        //    }

        //    if (createEmailContact)
        //    {
        //        CreateCustomerContact(profileId, "EMAIL", email, "", executionContext.GetUserId(), executionContext.GetSiteId(), sqlTrx);
        //    }
        //    if (createFBContact)
        //    {
        //        CreateCustomerContact(profileId, "FACEBOOK", fbUserId, fbAccessToken, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTrx);
        //    }
        //    if (createTwContact)
        //    {
        //        CreateCustomerContact(profileId, "TWITTER", twAccessToken, twAccessSecret, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTrx);
        //    }
        //    if (createWeChatContact)
        //    {
        //        CreateCustomerContact(profileId, "WECHAT", weChatAccessToken, "", executionContext.GetUserId(), executionContext.GetSiteId(), sqlTrx);
        //    } 
        //    if (createAddress)
        //    {
        //        CreateCustomerAddresss(profileId, addressType, address1, address2, address3, pinCode, city, stateId, countryId, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTrx);
        //    }


        //    int customerId = -1;
        //    if (backwardCompatibility == "Y")
        //    {

        //        customerId = CreateCustomerRecord(profileId, title, customerName, middleName, lastName, notes, birthDate, gender,
        //                         anniversary, customerPhoto, rightHanded, teamUser, uniqueId, taxCode, company, designation,
        //                         userName, executionContext.GetUserId(), password, executionContext.GetSiteId(), contactPhone, contactPhone2, email, fbUserId,
        //                         fbAccessToken, twAccessToken, twAccessSecret, weChatAccessToken, address1, address2, address3,
        //                         pinCode, city, stateId, countryId, channel, sqlTrx);
        //    }
        //    else
        //    {
        //        customerId = CreateCustomerRecord(profileId, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", executionContext.GetUserId(), "",
        //                                          executionContext.GetSiteId(), "", "", "", "", "", "", "", "", "", "", "", "", "", -1,-1, channel, sqlTrx);
        //    }

        //    log.LogMethodExit(customerId);
        //    return customerId;
        //}

        //void CreateCustomerContact(int profileId, string contactType, string contactValue1, string contactValue2, string loginId, int siteId, SqlTransaction sqlTrx)
        //{
        //    log.LogMethodEntry(profileId, contactType, contactValue1, loginId, siteId, sqlTrx);
        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@passPhrase", this.passPhrase));
        //    sqlParams.Add(new SqlParameter("@profileId", profileId));
        //    sqlParams.Add(new SqlParameter("@contactType", contactType));
        //    sqlParams.Add(new SqlParameter("@contactValue", contactValue1));
        //    if (String.IsNullOrEmpty(contactValue2))
        //        sqlParams.Add(new SqlParameter("@contactValue2", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@contactValue2", contactValue2));

        //    if (String.IsNullOrEmpty(loginId))
        //        sqlParams.Add(new SqlParameter("@loginId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@loginId", loginId));

        //    if (siteId == -1)
        //        sqlParams.Add(new SqlParameter("@siteId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@siteId", siteId));

        //    utilities.executeNonQuery(@"insert into Contact(ContactTypeId, ProfileId, 
        //                                                              Attribute1,attribute2, IsActive, CreatedBy, CreationDate, LastUpdatedBy, 
        //                                                              LastUpdateDate, site_id)
        //                                                  values ((SELECT Id FROM ContactType WHERE Name = @contactType),
        //                                                           @profileId,
        //                                                           EncryptByPassPhrase(@passPhrase,  @contactValue),
        //                                                           CASE WHEN @contactValue2 != null THEN 
        //                                                                     EncryptByPassPhrase(@passPhrase,  @contactValue2)
        //                                                                ELSE null
        //                                                                END,
        //                                                           1,
        //                                                           @loginId, 
        //                                                           getdate(),
        //                                                           @loginId, 
        //                                                           getdate(), 
        //                                                           @siteId);", sqlTrx, sqlParams.ToArray());

        //    log.LogMethodExit();
        //}

        //void CreateCustomerAddresss(int profileId, string addressType, string address1, string address2, string address3, string pinCode, string city, int stateId, int countryId, string loginId, int siteId, SqlTransaction sqlTrx)
        //{
        //    log.LogMethodEntry(profileId, addressType, address1, address2, address3, pinCode, city, stateId, countryId, sqlTrx);

        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@passPhrase", this.passPhrase));
        //    sqlParams.Add(new SqlParameter("@profileId", profileId));

        //    if (String.IsNullOrEmpty(addressType))
        //        sqlParams.Add(new SqlParameter("@addressType", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@addressType", addressType));

        //    if (String.IsNullOrEmpty(address1))
        //        sqlParams.Add(new SqlParameter("@address1", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@address1", address1));

        //    if (String.IsNullOrEmpty(address2))
        //        sqlParams.Add(new SqlParameter("@address2", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@address2", address2));

        //    if (String.IsNullOrEmpty(address3))
        //        sqlParams.Add(new SqlParameter("@address3", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@address3", address3));

        //    if (String.IsNullOrEmpty(pinCode))
        //        sqlParams.Add(new SqlParameter("@pinCode", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@pinCode", pinCode));

        //    if (String.IsNullOrEmpty(city))
        //        sqlParams.Add(new SqlParameter("@city", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@city", city));

        //    if (stateId == -1)
        //        sqlParams.Add(new SqlParameter("@stateId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@stateId", stateId));

        //    if (countryId == -1)
        //        sqlParams.Add(new SqlParameter("@countryId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@countryId", countryId));

        //    if (String.IsNullOrEmpty(loginId))
        //        sqlParams.Add(new SqlParameter("@loginId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@loginId", loginId));

        //    if (siteId == -1)
        //        sqlParams.Add(new SqlParameter("@siteId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@siteId", siteId));

        //    utilities.executeNonQuery(@"insert into Address(AddressTypeId,ProfileId,Line1, Line2, Line3,City,PostalCode,StateId,CountryId,
        //                                                                      IsActive,CreatedBy,CreationDate,LastUpdatedBy,LastUpdateDate, site_id)
        //                                                    values ((select id from AddressType WHERE Name = ISNULL(@addressType,'HOME')), 
        //                                                             @profileId,
        //                                                             EncryptByPassPhrase(@passPhrase,@address1),
        //                                                             EncryptByPassPhrase(@passPhrase,@address2),
        //                                                             EncryptByPassPhrase(@passPhrase,@address3),
        //                                                             @city,
        //                                                             @pinCode,
        //                                                             @stateId,
        //                                                             @countryId,
        //                                                             1,
        //                                                             @loginId,
        //                                                             getdate(),
        //                                                             @loginId,
        //                                                             getdate(),
        //                                                             @siteId ); ", sqlTrx, sqlParams.ToArray());
        //    log.LogMethodExit();
        //}

        //int CreateCustomerRecord(int profileId, string title, string customerName, string middleName, string lastName, string notes, string birthDate, string gender,
        //                         string anniversary, string customerPhoto, string rightHanded, string teamUser, string uniqueId, string taxCode, string company, string designation,
        //                         string userName, string loginId, string password, int siteId, string contactPhone, string contactPhone2, string email, string fbUserId,
        //                         string fbAccessToken, string twAccessToken, string twAccessSecret, string weChatAccessToken, string address1, string address2, string address3,
        //                         string pinCode, string city, int stateId, int countryId, string channel, SqlTransaction sqlTrx)
        //{
        //    log.LogMethodEntry();
        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    if (String.IsNullOrEmpty(title))
        //        sqlParams.Add(new SqlParameter("@title", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@title", title));

        //    if (String.IsNullOrEmpty(customerName))
        //        sqlParams.Add(new SqlParameter("@customerName", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@customerName", customerName));

        //    if (String.IsNullOrEmpty(middleName))
        //        sqlParams.Add(new SqlParameter("@middleName", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@middleName", middleName));

        //    if (String.IsNullOrEmpty(lastName))
        //        sqlParams.Add(new SqlParameter("@lastName", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@lastName", lastName));

        //    if (String.IsNullOrEmpty(uniqueId))
        //        sqlParams.Add(new SqlParameter("@uniqueId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@uniqueId", uniqueId));

        //    if (String.IsNullOrEmpty(notes))
        //        sqlParams.Add(new SqlParameter("@notes", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@notes", notes));

        //    if (String.IsNullOrEmpty(birthDate))
        //        sqlParams.Add(new SqlParameter("@birthDate", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@birthDate", birthDate));

        //    if (String.IsNullOrEmpty(gender))
        //        sqlParams.Add(new SqlParameter("@gender", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@gender", gender));

        //    if (String.IsNullOrEmpty(anniversary))
        //        sqlParams.Add(new SqlParameter("@anniversary", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@anniversary", anniversary));

        //    if (String.IsNullOrEmpty(customerPhoto))
        //        sqlParams.Add(new SqlParameter("@customerPhoto", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@customerPhoto", customerPhoto));

        //    if (String.IsNullOrEmpty(rightHanded))
        //        sqlParams.Add(new SqlParameter("@rightHanded", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@rightHanded", customerPhoto));

        //    if (String.IsNullOrEmpty(teamUser))
        //        sqlParams.Add(new SqlParameter("@teamUser", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@teamUser", customerPhoto));

        //    if (String.IsNullOrEmpty(taxCode))
        //        sqlParams.Add(new SqlParameter("@taxCode", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@taxCode", taxCode));

        //    if (String.IsNullOrEmpty(company))
        //        sqlParams.Add(new SqlParameter("@company", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@company", company));

        //    if (String.IsNullOrEmpty(designation))
        //        sqlParams.Add(new SqlParameter("@designation", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@designation", designation));

        //    if (String.IsNullOrEmpty(userName))
        //        sqlParams.Add(new SqlParameter("@userName", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@userName", userName));

        //    if (String.IsNullOrEmpty(loginId))
        //        sqlParams.Add(new SqlParameter("@loginId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@loginId", loginId));

        //    if (String.IsNullOrEmpty(password))
        //        sqlParams.Add(new SqlParameter("@password", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@password", password));


        //    if (siteId == -1)
        //        sqlParams.Add(new SqlParameter("@siteId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@siteId", siteId));

        //    sqlParams.Add(new SqlParameter("@profileId", profileId));

        //    if (String.IsNullOrEmpty(contactPhone))
        //        sqlParams.Add(new SqlParameter("@contactPhone", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@contactPhone", contactPhone));

        //    if (String.IsNullOrEmpty(contactPhone2))
        //        sqlParams.Add(new SqlParameter("@contactPhone2", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@contactPhone2", contactPhone2));


        //    if (String.IsNullOrEmpty(email))
        //        sqlParams.Add(new SqlParameter("@email", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@email", email));

        //    if (String.IsNullOrEmpty(fbUserId))
        //        sqlParams.Add(new SqlParameter("@fbUserId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@fbUserId", fbUserId));

        //    if (String.IsNullOrEmpty(fbAccessToken))
        //        sqlParams.Add(new SqlParameter("@fbAccessToken", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@fbAccessToken", fbUserId));

        //    if (String.IsNullOrEmpty(twAccessToken))
        //        sqlParams.Add(new SqlParameter("@twAccessToken", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@twAccessToken", twAccessToken));

        //    if (String.IsNullOrEmpty(twAccessSecret))
        //        sqlParams.Add(new SqlParameter("@twAccessSecret", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@twAccessSecret", twAccessSecret));

        //    if (String.IsNullOrEmpty(weChatAccessToken))
        //        sqlParams.Add(new SqlParameter("@weChatAccessToken", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@weChatAccessToken", weChatAccessToken));

        //    if (String.IsNullOrEmpty(address1))
        //        sqlParams.Add(new SqlParameter("@address1", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@address1", address1));

        //    if (String.IsNullOrEmpty(address2))
        //        sqlParams.Add(new SqlParameter("@address2", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@address2", address2));

        //    if (String.IsNullOrEmpty(address3))
        //        sqlParams.Add(new SqlParameter("@address3", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@address3", address3));

        //    if (String.IsNullOrEmpty(pinCode))
        //        sqlParams.Add(new SqlParameter("@pinCode", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@pinCode", pinCode));


        //    if (String.IsNullOrEmpty(pinCode))
        //        sqlParams.Add(new SqlParameter("@city", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@city", city));


        //    if (stateId == -1)
        //        sqlParams.Add(new SqlParameter("@stateId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@stateId", stateId));


        //    if (countryId == -1)
        //        sqlParams.Add(new SqlParameter("@countryId", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@countryId", countryId));


        //    if (String.IsNullOrEmpty(channel))
        //        sqlParams.Add(new SqlParameter("@channel", DBNull.Value));
        //    else
        //        sqlParams.Add(new SqlParameter("@channel", channel));

        //    object customerObj = utilities.executeScalar(@"INSERT INTO customers
        //                                                       (customer_name, address1, address2, address3, city, state, country, pin, email, birth_date
        //                                                        , gender, anniversary, contact_phone1, contact_phone2, notes, last_updated_date
        //                                                        , last_updated_user, middle_name, last_name, Company, Designation, PhotoFileName
        //                                                        , Guid, site_id, Unique_ID, Username, FBUserId, FBAccessToken, TWAccessToken, TWAccessSecret
        //                                                       , RightHanded, TeamUser, Password, IDProofFileName
        //                                                       , Title, Channel, WeChatAccessToken, TaxCode, ProfileId, CreatedBy, CreationDate)
        //                                                 VALUES
        //                                                       ( @customerName
        //                                                       , @address1
        //                                                       , @address2
        //                                                       , @address3
        //                                                       , @city
        //                                                       , (select top 1 state from state where stateId = @stateId )
        //                                                       , (select top 1 CountryName from country where CountryId = @countryId)
        //                                                       , @pinCode
        //                                                       , @email
        //                                                       , @birthDate
        //                                                       , @gender
        //                                                       , @anniversary
        //                                                       , @contactPhone
        //                                                       , @contactPhone2
        //                                                       , @notes
        //                                                       , getdate()
        //                                                       , @loginId
        //                                                       , @middleName
        //                                                       , @lastName 
        //                                                       , @company
        //                                                       , @designation
        //                                                       , @customerPhoto
        //                                                       , newid()
        //                                                       , @siteId
        //                                                       , @uniqueId
        //                                                       , @userName
        //                                                       , @fbUserId
        //                                                       , @fbAccessToken
        //                                                       , @twAccessToken
        //                                                       , @twAccessSecret
        //                                                       , @rightHanded
        //                                                       , @teamUser 
        //                                                       , @passWord  
        //                                                       , @customerPhoto
        //                                                       , @title
        //                                                       , @channel
        //                                                       , @weChatAccessToken
        //                                                       , @taxCode
        //                                                       , @profileId
        //                                                       , @loginId
        //                                                       , getdate() ); select @@identity", sqlTrx, sqlParams.ToArray());
        //    int customerid = -1;
        //    if (customerObj != null)
        //        customerid = Convert.ToInt32(customerObj);

        //    log.LogMethodExit(customerid);
        //    return customerid;
        //}
    }
}
