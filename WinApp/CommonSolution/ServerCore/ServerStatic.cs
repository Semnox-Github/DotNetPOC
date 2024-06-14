/*******************************************************************************************************************************
* Project Name - ServerStatic.cs
* Description  - Initial configuration for Primary game server
* 
**************
**Version Log
**************
*Version     Date             Modified By    Remarks          
*******************************************************************************************************************************
*1.00        14-Apr-2008      Iqbal Mohammad Created  
*2.130.0     09-Aug-2021      Mathew Ninan   Defined lower limit for MIN TIME between polling based on reader testing. 750 ms 
*                                            is minimum threshold. New property in Server Static
* *******************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ServerCore
{
    public class ServerStatic
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string VALIDCREDIT = "VALIDCREDIT";
        public const string VALIDCOURTESY = "VALIDCOURTESY";
        public const string LOBALCREDIT = "LOBALCREDIT";
        public const string VALIDBONUS = "VALIDBONUS";
        public const string VALIDCREDITBONUS = "VALIDCREDITBONUS";
        public const string VALIDCREDITCOURTESY = "VALIDCREDITCOURTESY";
        public const string TIMEPLAY = "TIMEPLAY";
        public const string TECHPLAY = "TECHPLAY";
        public const string INVCARD = "INVCARD";
        public const string GAMETIMEMINGAP = "GAMETIMEMINGAP";
        public const string MULTIPLESWIPE = "MULTIPLESWIPE";
        public const string NOTALLOWED = "NOTALLOWED";
        public const string VALIDCARDGAME = "VALIDCARDGAME";
        public const string RESETTIMER = "RESETTIMER";
        public const string GAMETIMEWAIT = "GAMETIMEWAIT";
        public const string NOENTITLEMENT = "NOENTITLEMENT";
        public const string GAMETIMEPACKAGEEXPIRED = "GAMETIMEPACKAGEEXPIRED";
        public const string ERROR = "ERROR";

        public int MIN_SECONDS_BETWEEN_REPEAT_PLAY = 15;
        public int MIN_SECONDS_BETWEEN_GAMETIME_PLAY = 45;
        public int MIN_TIME_BETWEEN_POLLS = 1000;
        public int CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT = 300;
        public string CHECK_FOR_CARD_EXCEPTIONS = "N";
        public string ALLOW_LOYALTY_ON_GAMEPLAY = "N";
        public string SITENAME = "Parafait";
        public int SECONDS_BEFORE_TIMER_BLINK = 60;
        public int GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT = 20;
        public string READER_DISPLAY_NUMBER_FORMAT = "";
        public string READER_PRICE_DISPLAY_FORMAT = "";
        public string CHECK_BALANCE_DISPLAY_FORMAT = "";
        public double READER_HARDWARE_VERSION = 1.0;

        public string AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD = "N";

        public decimal TOKEN_PRICE;
        public string TOKEN_MACHINE_GAMEPLAY_CARD;

        public string LogTrxCommunication;
        public string LogRcvCommunication;
        public int LOG_FREQUENCY_IN_POLLS = 30;
        public int COMMUNICATION_FAILURE_RETRIES = 3;
        public int COMMUNICATION_RETRY_DELAY = 50;
        public int COMMUNICATION_SEND_DELAY = 0;
        public bool LOG_TICKET_UPDATE_EVENT;

        public int MAX_TICKETS_PER_GAMEPLAY = 100;
        public string CONSUME_CREDITS_BEFORE_BONUS;

        public string SITE_REAL_TICKET_MODE;

        public int AD_PUBLISH_WINDOW_START;
        public int AD_PUBLISH_WINDOW_END;

        public int AD_SHOW_WINDOW_START;
        public int AD_SHOW_WINDOW_END;
        public double DAYS_TO_KEEP_PHOTOS_FOR;

        public int DEFAULT_TCP_PORT;
        public string IP_MASK_FOR_NETWORK_SCAN;
        public int SOCKET_SEND_RECEIVE_TIMEOUT;
        public bool SHOW_POLLING_INDICATOR;
        public bool SHOW_DETAILED_POLL_STATUS;
        public string DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT;

        public int GAMEPLAY_TICKETS_EXPIRY_DAYS;
        public string AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD;
        public int CARD_VALIDITY;

        public bool ZERO_PRICE_GAMETIME_PLAY;
        public bool ZERO_PRICE_CARDGAME_PLAY;

        public bool LOG_INACTIVE_TERMINALS;
        public int READER_TYPE;
        public bool ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY;
        public bool CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND;

        public class GlobalGamePlayStats
        {
            public Machine CurrentMachine;
            public string GamePlayType = "";
            public decimal CardCreditPlusCardBalance = 0; // credit plus of 'A' type
            public decimal CardCreditPlusCredits = 0; // card credits + credit plus of 'G' type + credit plus of 'A' type
            public decimal CardCreditPlusBonus = 0; // card bonus + credit plus of 'B' type
            public decimal CardCreditPlusTime = 0;
            public decimal CardCredits = 0;
            public decimal CardBonus = 0;
            public decimal CardCourtesy = 0;
            public decimal CardTime = 0;
            public int CardTechGames = 0;
            public int CardGames = 0;
            public List<int> CardGameIdList = new List<int>();
            public int PromotionId = -1;
            public int CardCreditPlusTimeId = -1;
            public decimal CreditsRequired = 0;
            public string TicketMode = "";
            public bool TicketEligibility = true;
            public string MachineAddress = "";
            public int CardId = -1;
            public int ProductId = -1;
            //public int CardTypeId = -1;
            public int MembershipId = -1;
            public string Vip_Customer = null;
            public bool ValidGamePlay = false;
            public bool RequestProcessed = false;
            public int RepeatPlayIndex = 1;
            public string GameplayMessage = "";
            public char TechCard = 'N';
            internal DateTime PlayRequestTime = DateTime.Now;

            public class ConsumedValuesClass
            {
                public double CardCreditPlusCardBalance = 0; // credit plus of 'A' type
                public double CardCreditPlusGameplayCredits = 0; // credit plus of 'G' type
                public decimal CardCreditPlusCardBalanceAndGPCredits = 0; // credit plus of 'G' type + credit plus of 'A' type
                public decimal CardCreditPlusBonus = 0; // credit plus of 'B' type
                public decimal CardCreditPlusTime = 0;
                public decimal CardCredits = 0;
                public decimal CardBonus = 0;
                public decimal CardCourtesy = 0;
                public decimal CardTime = 0;
                public decimal CardGame = 0;
                public int TechGame = 0;
            }

            public ConsumedValuesClass ConsumedValues = new ConsumedValuesClass();

            public GlobalGamePlayStats()
            {
                log.LogMethodEntry();
                log.LogMethodExit(null);
            }

            public GlobalGamePlayStats(Machine machine)
            {
                log.LogMethodEntry(machine);
                CurrentMachine = machine;
                log.LogMethodExit(null);
            }
        }

        public Utilities Utilities;
        public ServerStatic(Utilities inUtilities)
        {
            log.LogMethodEntry(inUtilities);
            Utilities = inUtilities;
            Initialize();
            log.LogMethodExit(null);
        }

        public ServerStatic(Utilities inUtilities, ServerStatic cachedServerStatic)
        {
            log.LogMethodEntry(inUtilities);
            Utilities = inUtilities;
            MIN_SECONDS_BETWEEN_REPEAT_PLAY = cachedServerStatic.MIN_SECONDS_BETWEEN_REPEAT_PLAY;
            MIN_SECONDS_BETWEEN_GAMETIME_PLAY = cachedServerStatic.MIN_SECONDS_BETWEEN_GAMETIME_PLAY;
            MIN_TIME_BETWEEN_POLLS = cachedServerStatic.MIN_TIME_BETWEEN_POLLS;
            CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT = cachedServerStatic.CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT;
            CHECK_FOR_CARD_EXCEPTIONS = cachedServerStatic.CHECK_FOR_CARD_EXCEPTIONS;
            ALLOW_LOYALTY_ON_GAMEPLAY = cachedServerStatic.ALLOW_LOYALTY_ON_GAMEPLAY;
            SITENAME = cachedServerStatic.SITENAME;
            SECONDS_BEFORE_TIMER_BLINK = cachedServerStatic.SECONDS_BEFORE_TIMER_BLINK;
            GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT = cachedServerStatic.GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT;
            READER_DISPLAY_NUMBER_FORMAT = cachedServerStatic.READER_DISPLAY_NUMBER_FORMAT;
            READER_PRICE_DISPLAY_FORMAT = cachedServerStatic.READER_PRICE_DISPLAY_FORMAT;
            CHECK_BALANCE_DISPLAY_FORMAT = cachedServerStatic.CHECK_BALANCE_DISPLAY_FORMAT;
            READER_HARDWARE_VERSION = cachedServerStatic.READER_HARDWARE_VERSION;

            AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD = cachedServerStatic.AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD;

            TOKEN_PRICE = cachedServerStatic.TOKEN_PRICE;
            TOKEN_MACHINE_GAMEPLAY_CARD = cachedServerStatic.TOKEN_MACHINE_GAMEPLAY_CARD;

            LogTrxCommunication = cachedServerStatic.LogTrxCommunication;
            LogRcvCommunication = cachedServerStatic.LogRcvCommunication;
            LOG_FREQUENCY_IN_POLLS = cachedServerStatic.LOG_FREQUENCY_IN_POLLS;
            COMMUNICATION_FAILURE_RETRIES = cachedServerStatic.COMMUNICATION_FAILURE_RETRIES;
            COMMUNICATION_RETRY_DELAY = cachedServerStatic.COMMUNICATION_RETRY_DELAY;
            COMMUNICATION_SEND_DELAY = cachedServerStatic.COMMUNICATION_SEND_DELAY;
            LOG_TICKET_UPDATE_EVENT = cachedServerStatic.LOG_TICKET_UPDATE_EVENT;

            MAX_TICKETS_PER_GAMEPLAY = cachedServerStatic.MAX_TICKETS_PER_GAMEPLAY;
            CONSUME_CREDITS_BEFORE_BONUS = cachedServerStatic.CONSUME_CREDITS_BEFORE_BONUS;

            SITE_REAL_TICKET_MODE = cachedServerStatic.SITE_REAL_TICKET_MODE;

            AD_PUBLISH_WINDOW_START = cachedServerStatic.AD_PUBLISH_WINDOW_START;
            AD_PUBLISH_WINDOW_END = cachedServerStatic.AD_PUBLISH_WINDOW_END;

            AD_SHOW_WINDOW_START = cachedServerStatic.AD_SHOW_WINDOW_START;
            AD_SHOW_WINDOW_END = cachedServerStatic.AD_SHOW_WINDOW_END;
            DAYS_TO_KEEP_PHOTOS_FOR = cachedServerStatic.DAYS_TO_KEEP_PHOTOS_FOR;

            DEFAULT_TCP_PORT = cachedServerStatic.DEFAULT_TCP_PORT;
            IP_MASK_FOR_NETWORK_SCAN = cachedServerStatic.IP_MASK_FOR_NETWORK_SCAN;
            SOCKET_SEND_RECEIVE_TIMEOUT = cachedServerStatic.SOCKET_SEND_RECEIVE_TIMEOUT;
            SHOW_POLLING_INDICATOR = cachedServerStatic.SHOW_POLLING_INDICATOR;
            SHOW_DETAILED_POLL_STATUS = cachedServerStatic.SHOW_DETAILED_POLL_STATUS;
            DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT = cachedServerStatic.DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT;

            GAMEPLAY_TICKETS_EXPIRY_DAYS = cachedServerStatic.GAMEPLAY_TICKETS_EXPIRY_DAYS;
            AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD = cachedServerStatic.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD;
            CARD_VALIDITY = cachedServerStatic.CARD_VALIDITY;

            ZERO_PRICE_GAMETIME_PLAY = cachedServerStatic.ZERO_PRICE_GAMETIME_PLAY;
            ZERO_PRICE_CARDGAME_PLAY = cachedServerStatic.ZERO_PRICE_CARDGAME_PLAY;

            LOG_INACTIVE_TERMINALS = cachedServerStatic.LOG_INACTIVE_TERMINALS;
            READER_TYPE = cachedServerStatic.READER_TYPE;
            ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY = cachedServerStatic.ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY;
            CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND = cachedServerStatic.CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND;
            log.LogMethodExit(null);
        }

        public void Initialize()
        {
            log.LogMethodEntry();
            CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND = (Utilities.getParafaitDefaults("CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND") == "Y");
            ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY = Utilities.getParafaitDefaults("ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY").Equals("Y");

            try
            {
                READER_TYPE = Convert.ToInt32(Convert.ToDouble(Utilities.getParafaitDefaults("READER_TYPE")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in reader_type", ex);
                READER_TYPE = 1;
            }
            log.LogVariableState("READER_TYPE", READER_TYPE);
            try
            {
                CARD_VALIDITY = Convert.ToInt32(Convert.ToDouble(Utilities.getParafaitDefaults("CARD_VALIDITY")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in card_validity", ex);
                CARD_VALIDITY = 12;
            }
            log.LogVariableState(" CARD_VALIDITY", CARD_VALIDITY);
            try
            {
                AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD = Utilities.getParafaitDefaults("AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD", ex);
                AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD = "N";
            }
            log.LogVariableState("AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD", AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD);
            try
            {
                GAMEPLAY_TICKETS_EXPIRY_DAYS = Convert.ToInt32(Convert.ToDouble(Utilities.getParafaitDefaults("GAMEPLAY_TICKETS_EXPIRY_DAYS")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in GAMEPLAY_TICKETS_EXPIRY_DAYS ", ex);
                GAMEPLAY_TICKETS_EXPIRY_DAYS = 0;
            }
            log.LogVariableState("GAMEPLAY_TICKETS_EXPIRY_DAYS ", GAMEPLAY_TICKETS_EXPIRY_DAYS);
            try
            {
                MIN_SECONDS_BETWEEN_REPEAT_PLAY = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("MIN_SECONDS_BETWEEN_REPEAT_PLAY")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in MIN_SECONDS_BETWEEN_REPEAT_PLAY", ex);
                MIN_SECONDS_BETWEEN_REPEAT_PLAY = 15;
            }
            log.LogVariableState("MIN_SECONDS_BETWEEN_REPEAT_PLAY", MIN_SECONDS_BETWEEN_REPEAT_PLAY);
            try
            {
                MIN_SECONDS_BETWEEN_GAMETIME_PLAY = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("MIN_SECONDS_BETWEEN_GAMETIME_PLAY")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in MIN_SECONDS_BETWEEN_GAMETIME_PLAY ", ex);
                MIN_SECONDS_BETWEEN_GAMETIME_PLAY = 45;
            }
            log.LogVariableState("MIN_SECONDS_BETWEEN_GAMETIME_PLAY", MIN_SECONDS_BETWEEN_GAMETIME_PLAY);
            try
            {
                CHECK_FOR_CARD_EXCEPTIONS = Utilities.getParafaitDefaults("CHECK_FOR_CARD_EXCEPTIONS");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in   CHECK_FOR_CARD_EXCEPTIONS", ex);
                CHECK_FOR_CARD_EXCEPTIONS = "N";
            }
            log.LogVariableState("CHECK_FOR_CARD_EXCEPTIONS ", CHECK_FOR_CARD_EXCEPTIONS);
            try
            {
                MIN_TIME_BETWEEN_POLLS = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("MIN_TIME_BETWEEN_POLLS")));
                MIN_TIME_BETWEEN_POLLS = Math.Max(MIN_TIME_BETWEEN_POLLS, 750); //have lower limit of 750 ms as per discussion with Iqbal
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  MIN_TIME_BETWEEN_POLLS", ex);
                MIN_TIME_BETWEEN_POLLS = 1000;
            }
            log.LogVariableState("MIN_TIME_BETWEEN_POLLS", MIN_TIME_BETWEEN_POLLS);
            try
            {
                CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT", ex);
                CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT = 300;
            }
            log.LogVariableState("CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT", CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT);
            try
            {
                SITENAME = Utilities.getParafaitDefaults("READER_DISPLAY_SITENAME");
                if (SITENAME.Length > 10)
                    SITENAME = SITENAME.Substring(0, 10);
            }
            catch (Exception ex)
            {
                log.Error("Error occured in READER_DISPLAY_SITENAME", ex);
                SITENAME = "Parafait";
            }
            SITENAME = SITENAME.PadRight(10, ' ');
            log.LogVariableState(" SITENAME", SITENAME);
            try
            {
                SECONDS_BEFORE_TIMER_BLINK = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("SECONDS_BEFORE_TIMER_BLINK")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  SECONDS_BEFORE_TIMER_BLINK ", ex);
                SECONDS_BEFORE_TIMER_BLINK = 60;
            }
            log.LogVariableState("SECONDS_BEFORE_TIMER_BLINK", SECONDS_BEFORE_TIMER_BLINK);
            try
            {
                GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT", ex);
                GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT = 20;
            }
            log.LogVariableState("GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT", GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT);
            try
            {
                ALLOW_LOYALTY_ON_GAMEPLAY = Utilities.getParafaitDefaults("ALLOW_LOYALTY_ON_GAMEPLAY");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  ALLOW_LOYALTY_ON_GAMEPLAY ", ex);
                ALLOW_LOYALTY_ON_GAMEPLAY = "N";
            }
            log.LogVariableState("ALLOW_LOYALTY_ON_GAMEPLAY ", ALLOW_LOYALTY_ON_GAMEPLAY);
            try
            {
                //READER_HARDWARE_VERSION = Convert.ToDouble(Utilities.getParafaitDefaults("READER_HARDWARE_VERSION"));
                READER_HARDWARE_VERSION = Double.Parse(Utilities.getParafaitDefaults("READER_HARDWARE_VERSION"), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                log.Error("Error occured in READER_HARDWARE_VERSION ", ex);
                READER_HARDWARE_VERSION = 1.0;
            }
            log.LogVariableState("READER_HARDWARE_VERSION", READER_HARDWARE_VERSION);
            try
            {
                READER_DISPLAY_NUMBER_FORMAT = Utilities.getParafaitDefaults("NUMBER_FORMAT");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  READER_DISPLAY_NUMBER_FORMAT", ex);
                READER_DISPLAY_NUMBER_FORMAT = "##0";
            }
            log.LogVariableState("READER_DISPLAY_NUMBER_FORMAT", READER_DISPLAY_NUMBER_FORMAT);
            try
            {
                READER_PRICE_DISPLAY_FORMAT = Utilities.getParafaitDefaults("READER_PRICE_DISPLAY_FORMAT");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  READER_PRICE_DISPLAY_FORMAT", ex);
                READER_PRICE_DISPLAY_FORMAT = "##0.00";
            }
            log.LogVariableState("READER_PRICE_DISPLAY_FORMAT", READER_PRICE_DISPLAY_FORMAT);
            try
            {
                AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD = Utilities.getParafaitDefaults("AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD", ex);
                AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD = "N";
            }
            log.LogVariableState("AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD", AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD);
            try
            {
                DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT = Utilities.getParafaitDefaults("DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT", ex);
                DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT = "N";
            }
            log.LogVariableState("DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT", DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT);
            try
            {
                int posDot = READER_PRICE_DISPLAY_FORMAT.IndexOf(".");
                if (posDot > 0)
                {
                    CHECK_BALANCE_DISPLAY_FORMAT = "####." + READER_PRICE_DISPLAY_FORMAT.Substring(posDot + 1);
                }
                else
                    CHECK_BALANCE_DISPLAY_FORMAT = READER_PRICE_DISPLAY_FORMAT;
            }
            catch (Exception ex)
            {
                log.Error("Error occured in CHECK_BALANCE_DISPLAY_FORMAT", ex);
                CHECK_BALANCE_DISPLAY_FORMAT = READER_PRICE_DISPLAY_FORMAT;
            }
            log.LogVariableState(" CHECK_BALANCE_DISPLAY_FORMAT", CHECK_BALANCE_DISPLAY_FORMAT);
            try
            {
                TOKEN_PRICE = Convert.ToDecimal(double.Parse(Utilities.getParafaitDefaults("TOKEN_PRICE")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  TOKEN_PRICE", ex);
                TOKEN_PRICE = 10;
            }
            log.LogVariableState("TOKEN_PRICE", TOKEN_PRICE);
            try
            {
                TOKEN_MACHINE_GAMEPLAY_CARD = Utilities.getParafaitDefaults("TOKEN_MACHINE_GAMEPLAY_CARD");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in TOKEN_MACHINE_GAMEPLAY_CARD ", ex);
                TOKEN_MACHINE_GAMEPLAY_CARD = "FFFFFFFFFF";
            }

            LogTrxCommunication = Utilities.getParafaitDefaults("LOG_TRANSMISSION_FAILURES");
            LogRcvCommunication = Utilities.getParafaitDefaults("LOG_RECEIVE_FAILURES");
            log.LogVariableState("TOKEN_MACHINE_GAMEPLAY_CARD", TOKEN_MACHINE_GAMEPLAY_CARD);
            try
            {
                LOG_FREQUENCY_IN_POLLS = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("LOG_FREQUENCY_IN_POLLS")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in LOG_FREQUENCY_IN_POLLS", ex);
                LOG_FREQUENCY_IN_POLLS = 30;
            }
            log.LogVariableState("LOG_FREQUENCY_IN_POLLS", LOG_FREQUENCY_IN_POLLS);
            try
            {
                COMMUNICATION_FAILURE_RETRIES = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("COMMUNICATION_FAILURE_RETRIES")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in COMMUNICATION_FAILURE_RETRIES", ex);
                COMMUNICATION_FAILURE_RETRIES = 3;
            }
            log.LogVariableState("COMMUNICATION_FAILURE_RETRIES", COMMUNICATION_FAILURE_RETRIES);
            try
            {
                COMMUNICATION_RETRY_DELAY = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("COMMUNICATION_RETRY_DELAY")));
                if (COMMUNICATION_RETRY_DELAY < 30)
                    COMMUNICATION_RETRY_DELAY = 30;
            }
            catch (Exception ex)
            {
                log.Error("Error occured in COMMUNICATION_RETRY_DELAY", ex);
                COMMUNICATION_RETRY_DELAY = 50;
            }
            log.LogVariableState(" COMMUNICATION_RETRY_DELAY", COMMUNICATION_RETRY_DELAY);
            try
            {
                COMMUNICATION_SEND_DELAY = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("COMMUNICATION_SEND_DELAY")));
                if (COMMUNICATION_SEND_DELAY < 0)
                    COMMUNICATION_SEND_DELAY = 0;
            }
            catch (Exception ex)
            {
                log.Error("Error occured in COMMUNICATION_SEND_DELAY", ex);
                COMMUNICATION_SEND_DELAY = 0;
            }
            log.LogVariableState("COMMUNICATION_SEND_DELAY", COMMUNICATION_SEND_DELAY);
            try
            {
                MAX_TICKETS_PER_GAMEPLAY = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("MAX_TICKETS_PER_GAMEPLAY")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in MAX_TICKETS_PER_GAMEPLAY", ex);
                MAX_TICKETS_PER_GAMEPLAY = 100;
            }
            log.LogVariableState("MAX_TICKETS_PER_GAMEPLAY", MAX_TICKETS_PER_GAMEPLAY);
            try
            {
                CONSUME_CREDITS_BEFORE_BONUS = Utilities.getParafaitDefaults("CONSUME_CREDITS_BEFORE_BONUS");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in CONSUME_CREDITS_BEFORE_BONUS", ex);
                CONSUME_CREDITS_BEFORE_BONUS = "Y";
            }
            log.LogVariableState("CONSUME_CREDITS_BEFORE_BONUS", CONSUME_CREDITS_BEFORE_BONUS);
            try
            {
                SITE_REAL_TICKET_MODE = Utilities.getParafaitDefaults("REAL_TICKET_MODE");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in SITE_REAL_TICKET_MODE", ex);
                SITE_REAL_TICKET_MODE = "Y";
            }
            log.LogVariableState("SITE_REAL_TICKET_MODE", SITE_REAL_TICKET_MODE);
            try
            {
                AD_PUBLISH_WINDOW_START = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("AD_PUBLISH_WINDOW_START")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  AD_PUBLISH_WINDOW_START", ex);
                AD_PUBLISH_WINDOW_START = 7;
            }
            log.LogVariableState("AD_PUBLISH_WINDOW_START", AD_PUBLISH_WINDOW_START);
            try
            {
                AD_PUBLISH_WINDOW_END = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("AD_PUBLISH_WINDOW_END")));
                AD_PUBLISH_WINDOW_END = (AD_PUBLISH_WINDOW_END == 0 ? 24 : AD_PUBLISH_WINDOW_END);
            }
            catch (Exception ex)
            {
                log.Error("Error occured in AD_PUBLISH_WINDOW_END", ex);
                AD_PUBLISH_WINDOW_END = 11;
            }
            log.LogVariableState("AD_PUBLISH_WINDOW_END", AD_PUBLISH_WINDOW_END);
            try
            {
                AD_SHOW_WINDOW_START = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("AD_SHOW_WINDOW_START")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in AD_SHOW_WINDOW_START", ex);
                AD_SHOW_WINDOW_START = 11;
            }
            log.LogVariableState("AD_SHOW_WINDOW_START", AD_SHOW_WINDOW_START);
            try
            {
                AD_SHOW_WINDOW_END = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("AD_SHOW_WINDOW_END")));
                AD_SHOW_WINDOW_END = (AD_SHOW_WINDOW_END == 0 ? 24 : AD_SHOW_WINDOW_END);
            }
            catch (Exception ex)
            {
                log.Error("Error occured in AD_SHOW_WINDOW_END ", ex);
                AD_PUBLISH_WINDOW_END = 11;
            }
            log.LogVariableState("AD_SHOW_WINDOW_END", AD_SHOW_WINDOW_END);
            try
            {
                DAYS_TO_KEEP_PHOTOS_FOR = Convert.ToDouble(Utilities.getParafaitDefaults("DAYS_TO_KEEP_PHOTOS_FOR"));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in DAYS_TO_KEEP_PHOTOS_FOR", ex);
                DAYS_TO_KEEP_PHOTOS_FOR = 7;
            }
            log.LogVariableState("DAYS_TO_KEEP_PHOTOS_FOR", DAYS_TO_KEEP_PHOTOS_FOR);
            try
            {
                DEFAULT_TCP_PORT = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("DEFAULT_TCP_PORT")));
                if (DEFAULT_TCP_PORT == 0)
                    DEFAULT_TCP_PORT = 2000;
            }
            catch (Exception ex)
            {
                log.Error("Error occured in DEFAULT_TCP_PORT", ex);
                DEFAULT_TCP_PORT = 2000;
            }
            log.LogVariableState("DEFAULT_TCP_PORT", DEFAULT_TCP_PORT);
            try
            {
                IP_MASK_FOR_NETWORK_SCAN = Utilities.getParafaitDefaults("IP_MASK_FOR_NETWORK_SCAN");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in IP_MASK_FOR_NETWORK_SCAN", ex);
                IP_MASK_FOR_NETWORK_SCAN = "192.168.1.x";
            }
            log.LogVariableState("IP_MASK_FOR_NETWORK_SCAN", IP_MASK_FOR_NETWORK_SCAN);
            try
            {
                SOCKET_SEND_RECEIVE_TIMEOUT = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("SOCKET_SEND_RECEIVE_TIMEOUT")));
                if (SOCKET_SEND_RECEIVE_TIMEOUT <= 0)
                    SOCKET_SEND_RECEIVE_TIMEOUT = 500;
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  SOCKET_SEND_RECEIVE_TIMEOUT", ex);
                SOCKET_SEND_RECEIVE_TIMEOUT = 500;
            }
            log.LogVariableState("SOCKET_SEND_RECEIVE_TIMEOUT", SOCKET_SEND_RECEIVE_TIMEOUT);

            if (Utilities.getParafaitDefaults("SHOW_POLLING_INDICATOR") == "Y")
                SHOW_POLLING_INDICATOR = true;
            else
                SHOW_POLLING_INDICATOR = false;

            if (Utilities.getParafaitDefaults("SHOW_DETAILED_POLL_STATUS") == "Y")
                SHOW_DETAILED_POLL_STATUS = true;
            else
                SHOW_DETAILED_POLL_STATUS = false;

            if (Utilities.getParafaitDefaults("LOG_TICKET_UPDATE_EVENT") == "Y")
                LOG_TICKET_UPDATE_EVENT = true;
            else
                LOG_TICKET_UPDATE_EVENT = false;

            if (Utilities.getParafaitDefaults("ZERO_PRICE_GAMETIME_PLAY") == "Y")
                ZERO_PRICE_GAMETIME_PLAY = true;
            else
                ZERO_PRICE_GAMETIME_PLAY = false;

            if (Utilities.getParafaitDefaults("ZERO_PRICE_CARDGAME_PLAY") == "Y")
                ZERO_PRICE_CARDGAME_PLAY = true;
            else
                ZERO_PRICE_CARDGAME_PLAY = false;

            LOG_INACTIVE_TERMINALS = (Utilities.getParafaitDefaults("LOG_INACTIVE_TERMINALS") == "Y");

            Utilities.getMifareCustomerKey();

            CheckIfServerMachine();
            log.LogMethodExit(null);
        }

        void CheckIfServerMachine()
        {
            log.LogMethodEntry();
            string ServerMachineName = "";
            bool isServerMachine = false;

            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = "ParafaitServer.exe.config";  // relative path names possible

                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                ConnectionStringsSection conSection = config.ConnectionStrings;

                ConnectionStringSettingsCollection conStrings = conSection.ConnectionStrings;

                string cnnString = conStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
                int serverNameStart = cnnString.IndexOf("=") + 1;
                int serverNameEnd = cnnString.IndexOf("\\", serverNameStart);
                if (serverNameEnd < 0)
                {
                    serverNameEnd = cnnString.IndexOf(";", serverNameStart);
                }

                ServerMachineName = cnnString.Substring(serverNameStart, serverNameEnd - serverNameStart);
            }
            catch (Exception ex)
            {
                log.Error("Error occured in while calculating connection string", ex);
                ServerMachineName = "ParafaitServer";
            }
            isServerMachine = (string.Compare(Environment.MachineName, ServerMachineName, true) == 0);
            log.LogMethodExit(null);
        }

        public void LogCommunication(string MasterAddress, string MachineAddress, int MachineId, string LastSentData, string ReceivedData)
        {
            log.LogMethodEntry(MasterAddress, MachineAddress, MachineId, LastSentData, ReceivedData);
            System.Threading.ThreadStart starter = delegate
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Utilities.createConnection();
                cmd.CommandText = "insert into CommunicationLog " +
                                  "(MasterAddress, MachineAddress, MachineId, LastSentData, ReceivedData, Timestamp) " +
                                  "values " +
                                  "(@MasterAddress, @MachineAddress, @MachineId, @LastSentData, @ReceivedData, getdate())";
                cmd.Parameters.AddWithValue("@MasterAddress", MasterAddress);
                cmd.Parameters.AddWithValue("@MachineAddress", MachineAddress);
                cmd.Parameters.AddWithValue("@MachineId", MachineId);
                cmd.Parameters.AddWithValue("@LastSentData", LastSentData);
                cmd.Parameters.AddWithValue("@ReceivedData", (ReceivedData.Length > 200 ? ReceivedData.Substring(0, 200) : ReceivedData));
                log.LogVariableState("@MasterAddress", MasterAddress);
                log.LogVariableState("@MachineAddress", MachineAddress);
                log.LogVariableState("@MachineId", MachineId);
                log.LogVariableState("@LastSentData", LastSentData);
                log.LogVariableState("@ReceivedData", (ReceivedData.Length > 200 ? ReceivedData.Substring(0, 200) : ReceivedData));
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error("Error in executing insert query", ex);
                }
                cmd.Connection.Close();
            };

            new System.Threading.Thread(starter).Start();
            log.LogMethodExit(null);
        }

        public string formatPrice(object value)
        {
            log.LogMethodEntry(value);
            string returnValue = Convert.ToDecimal(value).ToString(READER_PRICE_DISPLAY_FORMAT).PadRight(6).Substring(0, 6);
            log.LogMethodExit(returnValue);
            return (returnValue);
        }
    }
}
