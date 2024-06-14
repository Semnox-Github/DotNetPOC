/********************************************************************************************
 * Project Name - Customer App Configuration                                                                     
 * Description  - DH for Customer App configuration
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai            Created for Guest app
 *2.70.2        04-Feb-2020      Nitin Pai        Guest App phase 2 & tablet waiver changes
 *2.110        10-Feb-2021   Nitin Pai            Externalization enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.CustomerApp
{
    public class CustomerAppConfigurationDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;

        public CustomerAppConfigurationDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        public CustomerAppConfigurationDTO GetCustomerAppConfiguration(int siteId, ExecutionContext executionContext)
        {
            log.LogMethodEntry(siteId);

            Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();
            Utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
            Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            Utilities.ParafaitEnv.IsCorporate = true;
            Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            int virtualSiteId = -1;
            String virtualSiteNumber = Utilities.getParafaitDefaults("VIRTUAL_STORE_SITE_ID");
            if (!String.IsNullOrEmpty(virtualSiteNumber))
                int.TryParse(virtualSiteNumber, out virtualSiteId);

            CustomerAppConfigurationDTO configurationDTP = new CustomerAppConfigurationDTO(
                 Utilities.getParafaitDefaults("APP_SPLASH_IMAGE"),
                 Utilities.getParafaitDefaults("APP_CARD_IMAGE"),
                 Utilities.getParafaitDefaults("GOOGLE_REGISTRATION").Equals("Y") ? false : false,
                 Utilities.getParafaitDefaults("FACEBOOK_REGISTRATION").Equals("Y") ? false : false,
                 Utilities.getParafaitDefaults("PROMOTION_LAYOUT").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("GAMES_LAYOUT").Equals("Y") ? false : false,
                 Utilities.getParafaitDefaults("EVENTS_LAYOUT").Equals("Y") ? false : false,
                 Utilities.getParafaitDefaults("NEWCARD_CUSTOMERAPP").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("LINKCARD_CUSTOMERAPP").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("TRANSFERBALANCE_CUSTOMERAPP").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("REDEEMCARD_CUSTOMERAPP").Equals("Y") ? false : false,
                 Utilities.getParafaitDefaults("RECHARGECARD_CUSTOMERAPP").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("LOSTCARD_CUSTOMERAPP").Equals("Y") ? true : false,
                 !String.IsNullOrEmpty(Utilities.getParafaitDefaults("APP_IDLE_TIMEOUT")) ? Convert.ToInt32(Utilities.getParafaitDefaults("APP_IDLE_TIMEOUT").ToString()) : 3500,
                 !String.IsNullOrEmpty(Utilities.getParafaitDefaults("APP_VALIDITY_CHECK_DELAY")) ? Convert.ToInt32(Utilities.getParafaitDefaults("APP_VALIDITY_CHECK_DELAY").ToString()) : 3500,
                 !String.IsNullOrEmpty(Utilities.getParafaitDefaults("APP_HQ_REFRESH_THRESHOLD")) ? Convert.ToInt32(Utilities.getParafaitDefaults("APP_HQ_REFRESH_THRESHOLD").ToString()) : 4500,
                 !String.IsNullOrEmpty(Utilities.getParafaitDefaults("CURRENCY_CODE")) ? Utilities.getParafaitDefaults("CURRENCY_CODE").ToString() : "USD",
                 Utilities.getParafaitDefaults("SHOW_CREDITS_ON_CARD").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("SHOW_BONUS_ON_CARD").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("SHOW_TIME_ON_CARD").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("SHOW_TICKETS_ON_CARD").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("SHOW_LOYALTY_ON_CARD").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("SHOW_COURTESY_ON_CARD").Equals("Y") ? true : false,
                 virtualSiteId != -1 ? true : false,
                 virtualSiteId,
                 Utilities.getParafaitDefaults("GAMEPLAY_CUSTOMERAPP").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("PLAYBOX_LINK"),
                 Utilities.getParafaitDefaults("SHOW_MEMBERSHIP_ON_CARD").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("READER_PRICE_DISPLAY_FORMAT"),
                 Utilities.getParafaitDefaults("PROMO_IMAGE_FOLDER_URL"),
                 ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "AGE_OF_MAJORITY"),
                 ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "DEFAULT_LANGUAGE"),
                 ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_DEACTIVATION_NEEDS_MANAGER_APPROVAL"),
                 ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_CODE_IS_MANDATORY_TO_FETCH_CUSTOMER"),
                 ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "CHECK_WAIVER_REGISTRATION_COUNT_FOR_TRANSACTION"),
                 ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "OTP_CHECK_REQUIRED_FOR_WAIVER_REGISTRATION"),
                 ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "VALIDITY_PERIOD_FOR_WAIVER_REGISTRATION_OTP"),
                 ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_WAIVER_SIGN_IN_KIOSK"),
                 ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_OVERRIDE_NEEDS_MANAGER_APPROVAL"),
                 ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_WAIVER_OVERRIDE"),
                 Utilities.getParafaitDefaults("ALLOW_ONLINE_RECHARGE_LOCATION_OVERIDE").Equals("Y") ? true : false,
                 ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "OPTIONS_FOR_WAIVER_SET_SELECTION"),
                 Utilities.getParafaitDefaults("SHOW_CUSTOM_ATTRIBUTES_IN_WAIVER_SCREEN").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("PROMOTION_LAYOUT_URL"),
                 Utilities.getParafaitDefaults("STORE_LOCATOR_URL"),
                 Utilities.getParafaitDefaults("SHOW_CARD_EXPIRY").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("SHOW_CARD_ISSUED").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("SOCIAL_LAYOUT").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("STORE_LOCATOR_LAYOUT").Equals("Y") ? true : false,
                 Utilities.getParafaitDefaults("SUPPORT_CONTACT_NUMBER"),
                 Utilities.getParafaitDefaults("SUPPORT_CONTACT_EMAIL"),
                 Utilities.getParafaitDefaults("SUPPORT_MESSAGE_TEXT")
                 );

            log.LogMethodExit("New Card value" + Utilities.getParafaitDefaults("NEWCARD_CUSTOMERAPP"));
            log.LogMethodExit("is Corporate" + Utilities.ParafaitEnv.IsCorporate);
            log.LogMethodExit("site id" + Utilities.ParafaitEnv.SiteId);

            log.LogMethodExit(configurationDTP);
            return configurationDTP;
        }

    }
}
