/*******************************************************************************************************************************
* Project Name - GameServerEnvironment.cs
* Description  - Game Server related environment properties
* 
**************
**Version Log
**************
*Version     Date             Modified By    Remarks          
*******************************************************************************************************************************
*2.150.2     12-Dec-2022      Mathew Ninan    GameServerEnvironment as alternative to ServerStatic class  
*2.155.0     13-Jul-2023      Mathew Ninan    Added Dictionary READERMESSAGETOKENS to handle reader message translations 
* *******************************************************************************************************************************/
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ServerCore
{
    public class GameServerEnvironment
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

        private int min_seconds_between_repeat_play= 15;
        public int MIN_SECONDS_BETWEEN_REPEAT_PLAY
        {
            get { return min_seconds_between_repeat_play; }
            set { min_seconds_between_repeat_play = value; }
        }
        // public int MIN_SECONDS_BETWEEN_REPEAT_PLAY = 15;
        private int min_seconds_between_gametime_play = 45;
        public int MIN_SECONDS_BETWEEN_GAMETIME_PLAY
        {
            get { return min_seconds_between_gametime_play; }
            set { min_seconds_between_gametime_play = value; }
        }
        //public int MIN_SECONDS_BETWEEN_GAMETIME_PLAY = 45;
        //public int MIN_TIME_BETWEEN_POLLS = 1000;
        private int min_time_between_polls = 1000;
        public int MIN_TIME_BETWEEN_POLLS
        {
            get { return min_time_between_polls; }
            set { min_time_between_polls = value; }
        }
        //public int CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT = 300;
        private int consecutive_trx_fail_count_before_hub_reboot = 300;
        public int CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT
        {
            get { return consecutive_trx_fail_count_before_hub_reboot; }
            set { consecutive_trx_fail_count_before_hub_reboot = value; }
        }
        //public string CHECK_FOR_CARD_EXCEPTIONS = "N";
        private string check_for_card_exceptions = "N";
        public string CHECK_FOR_CARD_EXCEPTIONS
        {
            get { return check_for_card_exceptions; }
            set { check_for_card_exceptions = value; }
        }
        private string allow_loyalty_on_gameplay = "N";
        public string ALLOW_LOYALTY_ON_GAMEPLAY
        {
            get { return allow_loyalty_on_gameplay; }
            set { allow_loyalty_on_gameplay = value; }
        }
        //public string ALLOW_LOYALTY_ON_GAMEPLAY = "N";
        private string sitename = "Parafait";
        public string SITENAME
        {
            get { return sitename; }
            set { sitename = value; }
        }
        //public string SITENAME = "Parafait";
        private int seconds_before_timer_blink = 60;
        public int SECONDS_BEFORE_TIMER_BLINK
        {
            get { return seconds_before_timer_blink; }
            set { seconds_before_timer_blink = value; }
        }
        //public int SECONDS_BEFORE_TIMER_BLINK = 60;
        private int group_timer_extend_after_interval_percent = 20;
        public int GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT
        {
            get { return group_timer_extend_after_interval_percent; }
            set { group_timer_extend_after_interval_percent = value; }
        }
        //public int GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT = 20;
        private string reader_display_number_format = "";
        public string READER_DISPLAY_NUMBER_FORMAT
        {
            get { return reader_display_number_format; }
            set { reader_display_number_format = value; }
        }
        //public string READER_DISPLAY_NUMBER_FORMAT = "";
        private string reader_price_display_format = "";
        public string READER_PRICE_DISPLAY_FORMAT
        {
            get { return reader_price_display_format;}
            set { reader_price_display_format = value; }
        }
        //public string READER_PRICE_DISPLAY_FORMAT = "";
        private string check_balance_display_format = "";
        public string CHECK_BALANCE_DISPLAY_FORMAT
        {
            get { return check_balance_display_format; }
            set { check_balance_display_format = value; }
        }
        //public string CHECK_BALANCE_DISPLAY_FORMAT = "";
        private double reader_hardware_version = 1.0;
        public double READER_HARDWARE_VERSION
        {
            get { return reader_hardware_version; }
            set { reader_hardware_version = value; }
        }
        //public double READER_HARDWARE_VERSION = 1.0;

        private string auto_update_physical_tickets_on_card = "N";
        public string AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD
        {
            get { return auto_update_physical_tickets_on_card; }
            set { auto_update_physical_tickets_on_card = value; }
        }
        //public string AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD = "N";
        private decimal token_price;
        public decimal TOKEN_PRICE
        {
            get { return token_price; }
            set { token_price = value; }
        }

        //public decimal TOKEN_PRICE;
        private string token_machine_gameplay_card;
        public string TOKEN_MACHINE_GAMEPLAY_CARD
        {
            get { return token_machine_gameplay_card; }
            set { token_machine_gameplay_card = value; }
        }
        //public string TOKEN_MACHINE_GAMEPLAY_CARD;
        private string logTrxCommunication;
        public string LogTrxCommunication
        {
            get { return logTrxCommunication; }
            set { logTrxCommunication = value; }
        }
        //public string LogTrxCommunication;
        private string logRcvCommunication;
        public string LogRcvCommunication
        {
            get { return logRcvCommunication; }
            set { logRcvCommunication = value; }
        }
        private int log_frequency_in_polls = 30;
        public int LOG_FREQUENCY_IN_POLLS
        {
            get { return log_frequency_in_polls; }
            set { log_frequency_in_polls = value; }
        }
        //public int LOG_FREQUENCY_IN_POLLS = 30;
        private int communication_failure_retries = 3;
        public int COMMUNICATION_FAILURE_RETRIES
        {
            get { return communication_failure_retries; }
            set { communication_failure_retries = value; }
        }
        //public int COMMUNICATION_FAILURE_RETRIES = 3;
        private int communication_retry_delay = 50;
        public int COMMUNICATION_RETRY_DELAY
        {
            get { return communication_retry_delay; }
            set { communication_retry_delay = value; }
        }
        //public int COMMUNICATION_RETRY_DELAY = 50;
        private int communication_send_delay = 0;
        public int COMMUNICATION_SEND_DELAY
        {
            get { return communication_send_delay; }
            set { communication_send_delay = value; }
        }
        //public int COMMUNICATION_SEND_DELAY = 0;
        private bool log_ticket_update_event;
        public bool LOG_TICKET_UPDATE_EVENT
        {
            get { return log_ticket_update_event; }
            set { log_ticket_update_event = value; }
        }
        private int max_tickets_per_gameplay = 100;
        public int MAX_TICKETS_PER_GAMEPLAY
        {
            get { return max_tickets_per_gameplay; }
            set { max_tickets_per_gameplay = value; }
        }
        //public int MAX_TICKETS_PER_GAMEPLAY = 100;
        private string consume_credits_before_bonus;
        public string CONSUME_CREDITS_BEFORE_BONUS
        {
            get { return consume_credits_before_bonus; }
            set { consume_credits_before_bonus = value; }
        }

        private string site_real_ticket_mode;
        public string SITE_REAL_TICKET_MODE
        {
            get { return site_real_ticket_mode; }
            set { site_real_ticket_mode = value; }
        }

        private int ad_publish_window_start;
        public int AD_PUBLISH_WINDOW_START
        {
            get { return ad_publish_window_start; }
            set { ad_publish_window_start = value; }
        }

        private int ad_publish_window_end;
        public int AD_PUBLISH_WINDOW_END
        {
            get { return ad_publish_window_end; }
            set { ad_publish_window_end = value; }
        }

        private int ad_show_window_start;
        public int AD_SHOW_WINDOW_START
        {
            get { return ad_show_window_start; }
            set { ad_show_window_start = value; }
        }
        private int ad_show_window_end;
        public int AD_SHOW_WINDOW_END
        {
            get { return ad_show_window_end; }
            set { ad_show_window_end = value; }
        }
        private double days_to_keep_photos_for;
        public double DAYS_TO_KEEP_PHOTOS_FOR
        {
            get { return days_to_keep_photos_for; }
            set { days_to_keep_photos_for = value; }
        }

        private int default_tcp_port;
        public int DEFAULT_TCP_PORT
        {
            get { return default_tcp_port; }
            set { default_tcp_port = value; }
        }
        private string ip_mask_for_network_scan;
        public string IP_MASK_FOR_NETWORK_SCAN
        {
            get { return ip_mask_for_network_scan; }
            set { ip_mask_for_network_scan = value; }
        }
        private int socket_send_receive_timeout;
        public int SOCKET_SEND_RECEIVE_TIMEOUT
        {
            get { return socket_send_receive_timeout; }
            set { socket_send_receive_timeout = value; }
        }

        private bool show_polling_indicator;
        public bool SHOW_POLLING_INDICATOR
        {
            get { return show_polling_indicator; }
            set { show_polling_indicator = value; }
        }

        private bool show_detailed_poll_status;
        public bool SHOW_DETAILED_POLL_STATUS
        {
            get { return show_detailed_poll_status; }
            set { show_detailed_poll_status = value; }
        }

        private string display_vip_price_only_if_different;
        public string DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT
        {
            get { return display_vip_price_only_if_different; }
            set { display_vip_price_only_if_different = value; }
        }

        private int gameplay_tickets_expiry_days;
        public int GAMEPLAY_TICKETS_EXPIRY_DAYS
        {
            get { return gameplay_tickets_expiry_days; }
            set { gameplay_tickets_expiry_days = value; }
        }

        private string auto_extend_gameplay_tickets_on_reload;

        public string AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD
        {
            get { return auto_extend_gameplay_tickets_on_reload; }
            set { auto_extend_gameplay_tickets_on_reload = value; }
        }
        private int card_validity;
        public int CARD_VALIDITY
        {
            get { return card_validity; }
            set { card_validity = value; }
        }

        private bool zero_price_gametime_play;
        public bool ZERO_PRICE_GAMETIME_PLAY
        {
            get { return zero_price_gametime_play; }
            set { zero_price_gametime_play = value; }
        }
        private bool zero_price_cardgame_play;
        public bool ZERO_PRICE_CARDGAME_PLAY
        {
            get { return zero_price_cardgame_play; }
            set { zero_price_cardgame_play = value; }
        }

        private bool log_inactive_terminals;
        public bool LOG_INACTIVE_TERMINALS
        {
            get { return log_inactive_terminals; }
            set { log_inactive_terminals = value; }
        }
        private int reader_type;
        public int READER_TYPE
        {
            get { return reader_type; }
            set { reader_type = value; }
        }
        private bool enforce_parent_account_for_gameplay;
        public bool ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY
        {
            get { return enforce_parent_account_for_gameplay; }
            set { enforce_parent_account_for_gameplay = value; }
        }
        private bool create_ff_gameplay_if_card_not_found;
        public bool CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND
        {
            get { return create_ff_gameplay_if_card_not_found; }
            set { create_ff_gameplay_if_card_not_found = value; }
        }
        public const int COM_PACKETSIZE = 32;

        public class GameServerPlayDTO
        {
            private int currentMachineId;
            private string gamePlayType;
            private decimal cardCreditPlusCardBalance = 0;
            private decimal cardCreditPlusCredits = 0;
            private decimal cardCreditPlusBonus = 0;
            private decimal cardCreditPlusTime = 0;
            private decimal cardCredits = 0;
            private decimal cardBonus = 0;
            private decimal cardCourtesy = 0;
            private decimal cardTime = 0;
            private int cardTechGames;
            private int cardGames;
            private List<int> cardGameIdList = new List<int>();
            private int promotionId = -1;
            private int cardCreditPlusTimeId = -1;
            private decimal creditsRequired = 0;
            private string ticketMode = "";
            private bool ticketEligibility = true;
            private string machineAddress = "";
            private int cardId = -1;
            private int productId = -1;
            private int membershipId = -1;
            private string vip_Customer = null;
            private bool validGamePlay = false;
            private bool requestProcessed = false;
            private int repeatPlayIndex = 1;
            private string gameplayMessage = string.Empty;
            private char techCard = 'N';
            private string gameMachineCommand = string.Empty;
            internal DateTime playRequestTime = DateTime.Now;
            private string additionalMessage = string.Empty;
            
            public int CurrentMachineId
            {
                get { return currentMachineId; }
                set { currentMachineId = value; }
            }
            public string GamePlayType
            {
                get { return gamePlayType; }
                set { gamePlayType = value; }
            }
            public decimal CardCreditPlusCardBalance// credit plus of 'A' type
            {
                get { return cardCreditPlusCardBalance; }
                set { cardCreditPlusCardBalance = value; }
            }
            public decimal CardCreditPlusCredits // card credits + credit plus of 'G' type + credit plus of 'A' type
            {
                get { return cardCreditPlusCredits; }
                set { cardCreditPlusCredits = value; }
            }
            public decimal CardCreditPlusBonus // card bonus + credit plus of 'B' type
            {
                get { return cardCreditPlusBonus; }
                set { cardCreditPlusBonus = value; }
            }
            public decimal CardCreditPlusTime
            {
                get { return cardCreditPlusTime; }
                set { cardCreditPlusTime = value; }
            }
            public decimal CardCredits
            {
                get { return cardCredits; }
                set { cardCredits = value; }
            }
            public decimal CardBonus
            {
                get { return cardBonus; }
                set { cardBonus = value; }
            }
            public decimal CardCourtesy
            {
                get { return cardCourtesy; }
                set { cardCourtesy = value; }
            }
            public decimal CardTime
            {
                get { return cardTime; }
                set { cardTime = value; }
            }
            public int CardTechGames
            {
                get { return cardTechGames; }
                set { cardTechGames = value; }
            }
            public int CardGames
            {
                get { return cardGames; }
                set { cardGames = value; }
            }
            public List<int> CardGameIdList
            {
                get { return cardGameIdList; }
                set { cardGameIdList = value; }
            }
            public int PromotionId
            {
                get { return promotionId; }
                set { promotionId = value; }
            }
            public int CardCreditPlusTimeId
            {
                get { return cardCreditPlusTimeId; }
                set { cardCreditPlusTimeId = value; }
            }
            public decimal CreditsRequired
            {
                get { return creditsRequired; }
                set { creditsRequired = value; }
            }
            public string TicketMode
            {
                get { return ticketMode; }
                set { ticketMode = value; }
            }
            public bool TicketEligibility
            {
                get { return ticketEligibility; }
                set { ticketEligibility = value; }
            }
            public string MachineAddress
            {
                get { return machineAddress; }
                set { machineAddress = value; }
            }
            public int CardId
            {
                get { return cardId; }
                set { cardId = value; }
            }
            public int ProductId
            {
                get { return productId; }
                set { productId = value; }
            }
            //public int CardTypeId = -1;
            public int MembershipId
            {
                get { return membershipId; }
                set { membershipId = value; }
            }
            public string Vip_Customer
            {
                get { return vip_Customer; }
                set { vip_Customer = value; }
            }
            public bool ValidGamePlay
            {
                get { return validGamePlay; }
                set { validGamePlay = value; }
            }
            public bool RequestProcessed
            {
                get { return requestProcessed; }
                set { requestProcessed = value; }
            }
            public int RepeatPlayIndex
            {
                get { return repeatPlayIndex; }
                set { repeatPlayIndex = value; }
            }
            public string GameplayMessage
            {
                get { return gameplayMessage; }
                set { gameplayMessage = value; }
            }
            public char TechCard
            {
                get { return techCard; }
                set { techCard = value; }
            }
            public string GameMachineCommand
            {
                get { return gameMachineCommand; }
                set { gameMachineCommand = value; }
            }
            public DateTime PlayRequestTime
            {
                get { return playRequestTime; }
                set { playRequestTime = value; }
            }
            public string AdditionalMessage
            {
                get { return additionalMessage; }
                set { additionalMessage = value; }
            }

            public class ConsumedValuesClass
            {
                private double cardCreditPlusCardBalance = 0; // credit plus of 'A' type
                private double cardCreditPlusGameplayCredits = 0; // credit plus of 'G' type
                private decimal cardCreditPlusCardBalanceAndGPCredits = 0; // credit plus of 'G' type + credit plus of 'A' type
                private decimal cardCreditPlusBonus = 0; // credit plus of 'B' type
                private decimal cardCreditPlusTime = 0;
                private decimal cardCredits = 0;
                private decimal cardBonus = 0;
                private decimal cardCourtesy = 0;
                private decimal cardTime = 0;
                private decimal cardGame = 0;
                private int techGame = 0;

                public double CardCreditPlusCardBalance // credit plus of 'A' type
                {
                    get { return cardCreditPlusCardBalance; }
                    set { cardCreditPlusCardBalance = value; }
                }
                public double CardCreditPlusGameplayCredits // credit plus of 'G' type
                {
                    get { return cardCreditPlusGameplayCredits; }
                    set { cardCreditPlusGameplayCredits = value; }
                }
                public decimal CardCreditPlusCardBalanceAndGPCredits // credit plus of 'G' type + credit plus of 'A' type
                {
                    get { return cardCreditPlusCardBalanceAndGPCredits; }
                    set { cardCreditPlusCardBalanceAndGPCredits = value; }
                }
                public decimal CardCreditPlusBonus // credit plus of 'B' type
                {
                    get { return cardCreditPlusBonus; }
                    set { cardCreditPlusBonus = value; }
                }
                public decimal CardCreditPlusTime
                {
                    get { return cardCreditPlusTime; }
                    set { cardCreditPlusTime = value; }
                }
                public decimal CardCredits
                {
                    get { return cardCredits; }
                    set { cardCredits = value; }
                }
                public decimal CardBonus
                {
                    get { return cardBonus; }
                    set { cardBonus = value; }
                }
                public decimal CardCourtesy
                {
                    get { return cardCourtesy; }
                    set { cardCourtesy = value; }
                }
                public decimal CardTime
                {
                    get { return cardTime; }
                    set { cardTime = value; }
                }
                public decimal CardGame
                {
                    get { return cardGame; }
                    set { cardGame = value; }
                }
                public int TechGame
                {
                    get { return techGame; }
                    set { techGame = value; }
                }

                public ConsumedValuesClass()
                {

                }

                public ConsumedValuesClass(double cardCreditPlusCardBalance, double cardCreditPlusGameplayCredits,
                    decimal cardCreditPlusCardBalanceAndGPCredits, decimal cardCreditPlusBonus, decimal cardCreditPlusTime,
                    decimal cardCredits, decimal cardBonus, decimal cardCourtesy, decimal cardTime, decimal cardGame,
                    int techGame)
                {
                    this.cardCreditPlusCardBalance = cardCreditPlusCardBalance;
                    this.cardCreditPlusGameplayCredits = cardCreditPlusGameplayCredits;
                    this.cardCreditPlusCardBalanceAndGPCredits = cardCreditPlusCardBalanceAndGPCredits;
                    this.cardCreditPlusBonus = cardCreditPlusBonus;
                    this.cardCreditPlusTime = cardCreditPlusTime;
                    this.cardCredits = cardCredits;
                    this.cardBonus = cardBonus;
                    this.cardCourtesy = cardCourtesy;
                    this.cardTime = cardTime;
                    this.cardGame = cardGame;
                    this.techGame = techGame;
                }
            }

            private ConsumedValuesClass consumedValues = new ConsumedValuesClass();

            public ConsumedValuesClass ConsumedValues
            {
                get { return consumedValues; }
                set { consumedValues = value; }
            }

            public GameServerPlayDTO()
            {
                log.LogMethodEntry();
                log.LogMethodExit(null);
            }

            public GameServerPlayDTO(int MachineId)
            {
                log.LogMethodEntry(MachineId);
                CurrentMachineId = MachineId;
                log.LogMethodExit(null);
            }
        }

        private ExecutionContext gameExecutionContext;

        public ExecutionContext GameEnvExecutionContext
        {
            get { return gameExecutionContext; }
            set { gameExecutionContext = value; }
        }

        public GameServerEnvironment()
        {

        }

        public GameServerEnvironment(ExecutionContext inExecutionContext)
        {
            log.LogMethodEntry(inExecutionContext);
            GameEnvExecutionContext = inExecutionContext;
            Initialize();
            log.LogMethodExit(null);
        }

        public GameServerEnvironment(ExecutionContext inExecutionContext, GameServerEnvironment gameServerEnvironment)
        {
            log.LogMethodEntry(inExecutionContext);
            GameEnvExecutionContext = inExecutionContext;
            MIN_SECONDS_BETWEEN_REPEAT_PLAY = gameServerEnvironment.MIN_SECONDS_BETWEEN_REPEAT_PLAY;
            MIN_SECONDS_BETWEEN_GAMETIME_PLAY = gameServerEnvironment.MIN_SECONDS_BETWEEN_GAMETIME_PLAY;
            MIN_TIME_BETWEEN_POLLS = gameServerEnvironment.MIN_TIME_BETWEEN_POLLS;
            CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT = gameServerEnvironment.CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT;
            CHECK_FOR_CARD_EXCEPTIONS = gameServerEnvironment.CHECK_FOR_CARD_EXCEPTIONS;
            ALLOW_LOYALTY_ON_GAMEPLAY = gameServerEnvironment.ALLOW_LOYALTY_ON_GAMEPLAY;
            SITENAME = gameServerEnvironment.SITENAME;
            SECONDS_BEFORE_TIMER_BLINK = gameServerEnvironment.SECONDS_BEFORE_TIMER_BLINK;
            GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT = gameServerEnvironment.GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT;
            READER_DISPLAY_NUMBER_FORMAT = gameServerEnvironment.READER_DISPLAY_NUMBER_FORMAT;
            READER_PRICE_DISPLAY_FORMAT = gameServerEnvironment.READER_PRICE_DISPLAY_FORMAT;
            CHECK_BALANCE_DISPLAY_FORMAT = gameServerEnvironment.CHECK_BALANCE_DISPLAY_FORMAT;
            READER_HARDWARE_VERSION = gameServerEnvironment.READER_HARDWARE_VERSION;

            AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD = gameServerEnvironment.AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD;

            TOKEN_PRICE = gameServerEnvironment.TOKEN_PRICE;
            TOKEN_MACHINE_GAMEPLAY_CARD = gameServerEnvironment.TOKEN_MACHINE_GAMEPLAY_CARD;

            LogTrxCommunication = gameServerEnvironment.LogTrxCommunication;
            LogRcvCommunication = gameServerEnvironment.LogRcvCommunication;
            LOG_FREQUENCY_IN_POLLS = gameServerEnvironment.LOG_FREQUENCY_IN_POLLS;
            COMMUNICATION_FAILURE_RETRIES = gameServerEnvironment.COMMUNICATION_FAILURE_RETRIES;
            COMMUNICATION_RETRY_DELAY = gameServerEnvironment.COMMUNICATION_RETRY_DELAY;
            COMMUNICATION_SEND_DELAY = gameServerEnvironment.COMMUNICATION_SEND_DELAY;
            LOG_TICKET_UPDATE_EVENT = gameServerEnvironment.LOG_TICKET_UPDATE_EVENT;

            MAX_TICKETS_PER_GAMEPLAY = gameServerEnvironment.MAX_TICKETS_PER_GAMEPLAY;
            CONSUME_CREDITS_BEFORE_BONUS = gameServerEnvironment.CONSUME_CREDITS_BEFORE_BONUS;

            SITE_REAL_TICKET_MODE = gameServerEnvironment.SITE_REAL_TICKET_MODE;

            AD_PUBLISH_WINDOW_START = gameServerEnvironment.AD_PUBLISH_WINDOW_START;
            AD_PUBLISH_WINDOW_END = gameServerEnvironment.AD_PUBLISH_WINDOW_END;

            AD_SHOW_WINDOW_START = gameServerEnvironment.AD_SHOW_WINDOW_START;
            AD_SHOW_WINDOW_END = gameServerEnvironment.AD_SHOW_WINDOW_END;
            DAYS_TO_KEEP_PHOTOS_FOR = gameServerEnvironment.DAYS_TO_KEEP_PHOTOS_FOR;

            DEFAULT_TCP_PORT = gameServerEnvironment.DEFAULT_TCP_PORT;
            IP_MASK_FOR_NETWORK_SCAN = gameServerEnvironment.IP_MASK_FOR_NETWORK_SCAN;
            SOCKET_SEND_RECEIVE_TIMEOUT = gameServerEnvironment.SOCKET_SEND_RECEIVE_TIMEOUT;
            SHOW_POLLING_INDICATOR = gameServerEnvironment.SHOW_POLLING_INDICATOR;
            SHOW_DETAILED_POLL_STATUS = gameServerEnvironment.SHOW_DETAILED_POLL_STATUS;
            DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT = gameServerEnvironment.DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT;

            GAMEPLAY_TICKETS_EXPIRY_DAYS = gameServerEnvironment.GAMEPLAY_TICKETS_EXPIRY_DAYS;
            AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD = gameServerEnvironment.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD;
            CARD_VALIDITY = gameServerEnvironment.CARD_VALIDITY;

            ZERO_PRICE_GAMETIME_PLAY = gameServerEnvironment.ZERO_PRICE_GAMETIME_PLAY;
            ZERO_PRICE_CARDGAME_PLAY = gameServerEnvironment.ZERO_PRICE_CARDGAME_PLAY;

            LOG_INACTIVE_TERMINALS = gameServerEnvironment.LOG_INACTIVE_TERMINALS;
            READER_TYPE = gameServerEnvironment.READER_TYPE;
            ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY = gameServerEnvironment.ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY;
            CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND = gameServerEnvironment.CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND;
            log.LogMethodExit(null);
        }

        public void Initialize()
        {
            log.LogMethodEntry();
            CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND = (ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND") == "Y");
            ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY").Equals("Y");

            try
            {
                READER_TYPE = Convert.ToInt32(Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "READER_TYPE")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in reader_type", ex);
                READER_TYPE = 1;
            }
            log.LogVariableState("READER_TYPE", READER_TYPE);
            try
            {
                CARD_VALIDITY = Convert.ToInt32(Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "CARD_VALIDITY")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in card_validity", ex);
                CARD_VALIDITY = 12;
            }
            log.LogVariableState(" CARD_VALIDITY", CARD_VALIDITY);
            try
            {
                AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD", ex);
                AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD = "N";
            }
            log.LogVariableState("AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD", AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD);
            try
            {
                GAMEPLAY_TICKETS_EXPIRY_DAYS = Convert.ToInt32(Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "GAMEPLAY_TICKETS_EXPIRY_DAYS")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in GAMEPLAY_TICKETS_EXPIRY_DAYS ", ex);
                GAMEPLAY_TICKETS_EXPIRY_DAYS = 0;
            }
            log.LogVariableState("GAMEPLAY_TICKETS_EXPIRY_DAYS ", GAMEPLAY_TICKETS_EXPIRY_DAYS);
            try
            {
                MIN_SECONDS_BETWEEN_REPEAT_PLAY = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "MIN_SECONDS_BETWEEN_REPEAT_PLAY")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in MIN_SECONDS_BETWEEN_REPEAT_PLAY", ex);
                MIN_SECONDS_BETWEEN_REPEAT_PLAY = 15;
            }
            log.LogVariableState("MIN_SECONDS_BETWEEN_REPEAT_PLAY", MIN_SECONDS_BETWEEN_REPEAT_PLAY);
            try
            {
                MIN_SECONDS_BETWEEN_GAMETIME_PLAY = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "MIN_SECONDS_BETWEEN_GAMETIME_PLAY")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in MIN_SECONDS_BETWEEN_GAMETIME_PLAY ", ex);
                MIN_SECONDS_BETWEEN_GAMETIME_PLAY = 45;
            }
            log.LogVariableState("MIN_SECONDS_BETWEEN_GAMETIME_PLAY", MIN_SECONDS_BETWEEN_GAMETIME_PLAY);
            try
            {
                CHECK_FOR_CARD_EXCEPTIONS = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "CHECK_FOR_CARD_EXCEPTIONS");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in   CHECK_FOR_CARD_EXCEPTIONS", ex);
                CHECK_FOR_CARD_EXCEPTIONS = "N";
            }
            log.LogVariableState("CHECK_FOR_CARD_EXCEPTIONS ", CHECK_FOR_CARD_EXCEPTIONS);
            try
            {
                MIN_TIME_BETWEEN_POLLS = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "MIN_TIME_BETWEEN_POLLS")));
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
                CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT", ex);
                CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT = 300;
            }
            log.LogVariableState("CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT", CONSECUTIVE_TRX_FAIL_COUNT_BEFORE_HUB_REBOOT);
            try
            {
                SITENAME = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "READER_DISPLAY_SITENAME");
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
                SECONDS_BEFORE_TIMER_BLINK = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "SECONDS_BEFORE_TIMER_BLINK")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  SECONDS_BEFORE_TIMER_BLINK ", ex);
                SECONDS_BEFORE_TIMER_BLINK = 60;
            }
            log.LogVariableState("SECONDS_BEFORE_TIMER_BLINK", SECONDS_BEFORE_TIMER_BLINK);
            try
            {
                GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT", ex);
                GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT = 20;
            }
            log.LogVariableState("GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT", GROUP_TIMER_EXTEND_AFTER_INTERVAL_PERCENT);
            try
            {
                ALLOW_LOYALTY_ON_GAMEPLAY = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "ALLOW_LOYALTY_ON_GAMEPLAY");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  ALLOW_LOYALTY_ON_GAMEPLAY ", ex);
                ALLOW_LOYALTY_ON_GAMEPLAY = "N";
            }
            log.LogVariableState("ALLOW_LOYALTY_ON_GAMEPLAY ", ALLOW_LOYALTY_ON_GAMEPLAY);
            try
            {
                READER_HARDWARE_VERSION = Double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "READER_HARDWARE_VERSION"), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                log.Error("Error occured in READER_HARDWARE_VERSION ", ex);
                READER_HARDWARE_VERSION = 1.0;
            }
            log.LogVariableState("READER_HARDWARE_VERSION", READER_HARDWARE_VERSION);
            try
            {
                READER_DISPLAY_NUMBER_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "NUMBER_FORMAT");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  READER_DISPLAY_NUMBER_FORMAT", ex);
                READER_DISPLAY_NUMBER_FORMAT = "##0";
            }
            log.LogVariableState("READER_DISPLAY_NUMBER_FORMAT", READER_DISPLAY_NUMBER_FORMAT);
            try
            {
                READER_PRICE_DISPLAY_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "READER_PRICE_DISPLAY_FORMAT");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  READER_PRICE_DISPLAY_FORMAT", ex);
                READER_PRICE_DISPLAY_FORMAT = "##0.00";
            }
            log.LogVariableState("READER_PRICE_DISPLAY_FORMAT", READER_PRICE_DISPLAY_FORMAT);
            try
            {
                AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD", ex);
                AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD = "N";
            }
            log.LogVariableState("AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD", AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD);
            try
            {
                DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT");
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
                TOKEN_PRICE = Convert.ToDecimal(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "TOKEN_PRICE")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  TOKEN_PRICE", ex);
                TOKEN_PRICE = 10;
            }
            log.LogVariableState("TOKEN_PRICE", TOKEN_PRICE);
            try
            {
                TOKEN_MACHINE_GAMEPLAY_CARD = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "TOKEN_MACHINE_GAMEPLAY_CARD");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in TOKEN_MACHINE_GAMEPLAY_CARD ", ex);
                TOKEN_MACHINE_GAMEPLAY_CARD = "FFFFFFFFFF";
            }

            LogTrxCommunication = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "LOG_TRANSMISSION_FAILURES");
            LogRcvCommunication = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "LOG_RECEIVE_FAILURES");
            log.LogVariableState("TOKEN_MACHINE_GAMEPLAY_CARD", TOKEN_MACHINE_GAMEPLAY_CARD);
            try
            {
                LOG_FREQUENCY_IN_POLLS = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "LOG_FREQUENCY_IN_POLLS")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in LOG_FREQUENCY_IN_POLLS", ex);
                LOG_FREQUENCY_IN_POLLS = 30;
            }
            log.LogVariableState("LOG_FREQUENCY_IN_POLLS", LOG_FREQUENCY_IN_POLLS);
            try
            {
                COMMUNICATION_FAILURE_RETRIES = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "COMMUNICATION_FAILURE_RETRIES")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in COMMUNICATION_FAILURE_RETRIES", ex);
                COMMUNICATION_FAILURE_RETRIES = 3;
            }
            log.LogVariableState("COMMUNICATION_FAILURE_RETRIES", COMMUNICATION_FAILURE_RETRIES);
            try
            {
                COMMUNICATION_RETRY_DELAY = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "COMMUNICATION_RETRY_DELAY")));
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
                COMMUNICATION_SEND_DELAY = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "COMMUNICATION_SEND_DELAY")));
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
                MAX_TICKETS_PER_GAMEPLAY = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "MAX_TICKETS_PER_GAMEPLAY")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in MAX_TICKETS_PER_GAMEPLAY", ex);
                MAX_TICKETS_PER_GAMEPLAY = 100;
            }
            log.LogVariableState("MAX_TICKETS_PER_GAMEPLAY", MAX_TICKETS_PER_GAMEPLAY);
            try
            {
                CONSUME_CREDITS_BEFORE_BONUS = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "CONSUME_CREDITS_BEFORE_BONUS");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in CONSUME_CREDITS_BEFORE_BONUS", ex);
                CONSUME_CREDITS_BEFORE_BONUS = "Y";
            }
            log.LogVariableState("CONSUME_CREDITS_BEFORE_BONUS", CONSUME_CREDITS_BEFORE_BONUS);
            try
            {
                SITE_REAL_TICKET_MODE = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "REAL_TICKET_MODE");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in SITE_REAL_TICKET_MODE", ex);
                SITE_REAL_TICKET_MODE = "Y";
            }
            log.LogVariableState("SITE_REAL_TICKET_MODE", SITE_REAL_TICKET_MODE);
            try
            {
                AD_PUBLISH_WINDOW_START = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "AD_PUBLISH_WINDOW_START")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  AD_PUBLISH_WINDOW_START", ex);
                AD_PUBLISH_WINDOW_START = 7;
            }
            log.LogVariableState("AD_PUBLISH_WINDOW_START", AD_PUBLISH_WINDOW_START);
            try
            {
                AD_PUBLISH_WINDOW_END = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "AD_PUBLISH_WINDOW_END")));
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
                AD_SHOW_WINDOW_START = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "AD_SHOW_WINDOW_START")));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in AD_SHOW_WINDOW_START", ex);
                AD_SHOW_WINDOW_START = 11;
            }
            log.LogVariableState("AD_SHOW_WINDOW_START", AD_SHOW_WINDOW_START);
            try
            {
                AD_SHOW_WINDOW_END = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "AD_SHOW_WINDOW_END")));
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
                DAYS_TO_KEEP_PHOTOS_FOR = Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "DAYS_TO_KEEP_PHOTOS_FOR"));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in DAYS_TO_KEEP_PHOTOS_FOR", ex);
                DAYS_TO_KEEP_PHOTOS_FOR = 7;
            }
            log.LogVariableState("DAYS_TO_KEEP_PHOTOS_FOR", DAYS_TO_KEEP_PHOTOS_FOR);
            try
            {
                DEFAULT_TCP_PORT = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "DEFAULT_TCP_PORT")));
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
                IP_MASK_FOR_NETWORK_SCAN = ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "IP_MASK_FOR_NETWORK_SCAN");
            }
            catch (Exception ex)
            {
                log.Error("Error occured in IP_MASK_FOR_NETWORK_SCAN", ex);
                IP_MASK_FOR_NETWORK_SCAN = "192.168.1.x";
            }
            log.LogVariableState("IP_MASK_FOR_NETWORK_SCAN", IP_MASK_FOR_NETWORK_SCAN);
            try
            {
                SOCKET_SEND_RECEIVE_TIMEOUT = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "SOCKET_SEND_RECEIVE_TIMEOUT")));
                if (SOCKET_SEND_RECEIVE_TIMEOUT <= 0)
                    SOCKET_SEND_RECEIVE_TIMEOUT = 500;
            }
            catch (Exception ex)
            {
                log.Error("Error occured in  SOCKET_SEND_RECEIVE_TIMEOUT", ex);
                SOCKET_SEND_RECEIVE_TIMEOUT = 500;
            }
            log.LogVariableState("SOCKET_SEND_RECEIVE_TIMEOUT", SOCKET_SEND_RECEIVE_TIMEOUT);

            if (ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "SHOW_POLLING_INDICATOR") == "Y")
                SHOW_POLLING_INDICATOR = true;
            else
                SHOW_POLLING_INDICATOR = false;

            if (ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "SHOW_DETAILED_POLL_STATUS") == "Y")
                SHOW_DETAILED_POLL_STATUS = true;
            else
                SHOW_DETAILED_POLL_STATUS = false;

            if (ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "LOG_TICKET_UPDATE_EVENT") == "Y")
                LOG_TICKET_UPDATE_EVENT = true;
            else
                LOG_TICKET_UPDATE_EVENT = false;

            if (ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "ZERO_PRICE_GAMETIME_PLAY") == "Y")
                ZERO_PRICE_GAMETIME_PLAY = true;
            else
                ZERO_PRICE_GAMETIME_PLAY = false;

            if (ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "ZERO_PRICE_CARDGAME_PLAY") == "Y")
                ZERO_PRICE_CARDGAME_PLAY = true;
            else
                ZERO_PRICE_CARDGAME_PLAY = false;

            LOG_INACTIVE_TERMINALS = (ParafaitDefaultContainerList.GetParafaitDefault(GameEnvExecutionContext, "LOG_INACTIVE_TERMINALS") == "Y");

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
                fileMap.ExeConfigFilename = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "ParafaitServerService.exe.config";  // relative path names possible

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
                log.Info("Error occured in while calculating connection string. "+ ex.Message);
                ServerMachineName = "ParafaitServer";
            }
            isServerMachine = (string.Compare(Environment.MachineName, ServerMachineName, true) == 0);
            log.LogMethodExit(null);
        }

        //public void LogCommunication(string MasterAddress, string MachineAddress, int MachineId, string LastSentData, string ReceivedData)
        //{
        //    log.LogMethodEntry(MasterAddress, MachineAddress, MachineId, LastSentData, ReceivedData);
        //    System.Threading.ThreadStart starter = delegate
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = Utilities.createConnection();
        //        cmd.CommandText = "insert into CommunicationLog " +
        //                          "(MasterAddress, MachineAddress, MachineId, LastSentData, ReceivedData, Timestamp) " +
        //                          "values " +
        //                          "(@MasterAddress, @MachineAddress, @MachineId, @LastSentData, @ReceivedData, getdate())";
        //        cmd.Parameters.AddWithValue("@MasterAddress", MasterAddress);
        //        cmd.Parameters.AddWithValue("@MachineAddress", MachineAddress);
        //        cmd.Parameters.AddWithValue("@MachineId", MachineId);
        //        cmd.Parameters.AddWithValue("@LastSentData", LastSentData);
        //        cmd.Parameters.AddWithValue("@ReceivedData", (ReceivedData.Length > 200 ? ReceivedData.Substring(0, 200) : ReceivedData));
        //        log.LogVariableState("@MasterAddress", MasterAddress);
        //        log.LogVariableState("@MachineAddress", MachineAddress);
        //        log.LogVariableState("@MachineId", MachineId);
        //        log.LogVariableState("@LastSentData", LastSentData);
        //        log.LogVariableState("@ReceivedData", (ReceivedData.Length > 200 ? ReceivedData.Substring(0, 200) : ReceivedData));
        //        try
        //        {
        //            cmd.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error("Error in executing insert query", ex);
        //        }
        //        cmd.Connection.Close();
        //    };

        //    new System.Threading.Thread(starter).Start();
        //    log.LogMethodExit(null);
        //}

        public string formatPrice(object value)
        {
            log.LogMethodEntry(value);
            //string returnValue = Convert.ToDecimal(value).ToString(READER_PRICE_DISPLAY_FORMAT).PadRight(6).Substring(0, 6);
            //if (READER_HARDWARE_VERSION >= 2.0)
            //{
            //    returnValue = Convert.ToDecimal(value).ToString(READER_PRICE_DISPLAY_FORMAT).PadLeft(9, ' ').Substring(0, 9);
            //}
            //else if (READER_HARDWARE_VERSION >= 1.21)
            //{
            //    returnValue = Convert.ToDecimal(value).ToString(READER_PRICE_DISPLAY_FORMAT).PadLeft(6, ' ').Substring(0, 6);
            //}
            //else
            //{
            //    returnValue = Convert.ToDecimal(value).ToString(READER_PRICE_DISPLAY_FORMAT).PadLeft(4, ' ').Substring(0, 4) + "  ";
                
            //}
            string returnValue = Convert.ToDecimal(value).ToString(READER_PRICE_DISPLAY_FORMAT).PadRight(6).Substring(0, 6);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Reader Message Tokens to be used for Translations
        /// </summary>
        public static Dictionary<int, string> READERMESSAGETOKENS = new Dictionary<int, string>
        {
            {0, "PRICE_TEXT" },
            {1, "VIP_PRICE_TEXT"},
            {2, "TICKET_TEXT"},
            {3, "PLEASE_TAP_CARD_TEXT"},
            {4, "FEED_TICKETS_TEXT"},
            {5, "JACKPOT_TEXT"},
            {6, "GAME_STARTING_TEXT"},
            {7, "ENJOY_TEXT"},
            {8, "CARD_READ_TEXT"},
            {9,"PROCESSING_TEXT"},
            {10, "START_FEED_TICKETS_TEXT"},
            {11, "DID_NOT_PROCESS_TEXT"},
            {12, "PLEASE_WAIT_TEXT"},
            {13, "TIME_REMAINING_TEXT"},
            {14, "MINUTES_TEXT"},
            {15, "SECONDS_TEXT"},
            {16, "BALANCE_TEXT"},
            {17, "PLAY_GAME_TEXT"},
            {18, "TIME_OUT_TEXT"},
            {19, "GAME_PLAY_UPDATED_TEXT"},
            {20, "PLAY_YOUR_WAY_TEXT" },
            {21, "TICKETS_UPDATED_TEXT"},
            {22, "INVALID_CARD_TEXT"},
            {23, "PLEASE_RETRY_TEXT"},
            {24, "LOW_BALANCE_TEXT"},
            {25, "PLEASE_RECHARGE_TEXT"},
            {26, "VERIFYING_TEXT"},
            {27, "TIME_ELAPSED"},
            {28, "TICKETS_COUNTED"},
            {29, "TIME_LEFT"},
            {30, "MINS_SECS" },
            {31, "CHECKING_BALANCE_TEXT"},
            {32, "CREDITS_TEXT"},
            {33, "BONUS_TEXT"},
            {34, "COURTESY_TEXT" },
            {35, "TICKETS_TEXT"},
            {36, "GAMES_TEXT"},
            {37, "GAME_NOT_INCLUDED"},
            {38, "PLS_TRY_ANOTHER_GAME"},
            {39, "TIME_EXPIRED"},
            {40, "HOPE_HAD_FUN" },
            {41, "SWIPED_QUICKLY"},
            {42, "PLS_WAIT_FOR_PREV_RIDE"},
            {43, "TiCKET_EATER_ON"},
            {44, "OPEN_GATE"},
            {45, "TIME_TEXT"},
            {46, "ATTENDANCE_RECORDED_TEXT"},
            {47, "THANK_YOU_TEXT"},
            {48, "GAME_SWITCHED_OFF"},
            {49, "GAME_SWITCHED_ON"},
            {50, "PLEASE_START_USING" },
            {51, "FAILED_TO_COMPLETE"},
            {52, "PLEASE_TRY_AGAIN"},
            {53, "EXIT_FREE_PLAY"},
            {54, "FREE_PLAY_TEXT"},
            {55, "ENTERING"},
            {56, "SUBMIT_2F_AUTHEN"},
            {57, "AT_2F_AUTHEN_READER"},
            {58, "INVALID_2F_AUTHEN"},
            {59, "PLEASE_RETRY_2F_AUTHEN"},
            {60, "EXIT_2F_AUTHEN" },
            {61, "TO_PAUSE_TIME"},
            {62, "TIME_PAUSE_SUCCESS"},
            {63, "NO_VALID_TIME"},
            {64, "NO_PAUSES_REMAINING"},
            {65, "TO_REFUND_TAG"},
            {66, "REFUND_SUCCESS"},
            {67, "REFUND_FAILURE"},
            {68, "GATE_NOT_READY"},
            {69, "WAIT_FOR_ENABLE"},
            {70, "TE_PLS_WAIT_FOR_CARD" },
            {71, "LOYALTY_POINTS"},
            {72, "CARD_EXPIRY"},
            {73, "PLEASE_SLOT_CARD"},
            {74, "FOR_ETICKETS"},
            {75, "CARD_VALID"},
            {76, "ETICKET_MODE_ON"},
            {77, "PHYSICAL_DISPENSED_TEXT"},
            {78, "ETICKETS_UPDATED_TEXT"},
            {79, "PHYSICAL_DISPENSING_TEXT"},
            {80, "ETICKETS_UPDATING_TEXT"},
            {81, "INELIGIBLE_FOR_TICKETS"},
            {82, "TICKETS_COUNTING"},
            {83, "REBOOTING"}
        };
    }
}
