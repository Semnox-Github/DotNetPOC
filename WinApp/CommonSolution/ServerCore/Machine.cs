/********************************************************************************************
 * Project Name - Machine
 * Description  - Business Logic to create Machine object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ******************************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad Created 
 *2.50.0      24-Dec-2018      Mathew Ninan   Out of Service based on Generic Calendar Theme
 *2.50.3      05-May-2019      Mathew Ninan   Handle theme calendar time spanning across days 
 *2.80.0      10-Sep-2020      Indrajeet Kumar Moved Fingerprint scan to Biometric reference 
 *2.130.0     07-Jul-2021      Mathew Ninan   Added additional properties to handle QR Code Play 
 *2.130.7     10-May-2022      Mathew Ninan   Adding Promotion Detail Id parameter. Get Config
 *                                            value based on Promotion and indicate config change
 *                                            for promotion enabled profile attributes
 *2.155.0     24-Jul-2023      Mathew Ninan   Play Game method updated to capture multi game play count                                            
 *****************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.Printer;
using Semnox.Parafait.Game;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Transaction;
using System.Threading.Tasks;
using Semnox.Parafait.Customer.Accounts;
using System.Threading;

namespace Semnox.Parafait.ServerCore
{
    public class Machine
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int MachineId;
        public int GameplayMachineId;
        public int GameId;
        public string machine_name;
        public string machine_address;
        public int MachineAddress;
        public string number_of_coins;
        public string timer_machine;
        public int AccessPointId;
        public int timer_interval;
        public string group_timer;
        public decimal play_credits;
        public decimal vip_play_credits;
        public int game_profile_id;
        public string TokenRedemption;
        public decimal TokenPrice;
        public string CoinPusher;
        public string TicketEater;
        public string RedeemTokenTo;
        public string isVIPPrice;
        public int ToggleVIP;
        public string TicketMode;
        public string ThemeNumber;
        public int VisualizationThemeNumber;
        public int AudioThemeNumber;
        public string ShowAd;
        public string NextAction = "DEFAULT";
        public int msgNumber = 0;
        public IPAddress ipAddress;
        public string MacAddress;
        public int TCPPort;
        public int RepeatPlayDelay;
        public int ConsecutiveTimePlayDelay;
        public int MaxTicketsPerGamePlay;
        public int GPUserIdentifier = 255;
        public int GameUserIdentifier = 255;
        public int InventoryLocationId = -1;
        public int CommunicationToken = 0;
        public int CommunicationRetries = 0;
        public int CommunicationFailureCount = 0;
        public int CommunicationSuccessCount = 0;
        public int ContinuousFailureCount = 0;
        public int ProcessRetryCount = 0;
        public string LastSentData = "";
        public string ReceivedData = "";
        public bool MachineInactive = false;
        public string Version;
        public string REQTagNumber = "XYZCXYZC";
        public Semnox.Parafait.Device.Biometric.FingerPrintReader fingerPrintReader = null;
        public bool TwoFactorAuthentication = false;
        public string credit_allowed, bonus_allowed, courtesy_allowed, time_allowed;

        public bool OutOfService = false;
        public bool GamePlayDisabled = false;
        public int ReaderType = 1;
        public DateTime LastRefreshTime = DateTime.Now;
        public int ContinuousExceptionCount = 0;
        public int ContinuousINVRCount = 0;

        public bool ForceRedeemToCard = false;
        public int GameplayMultiplier = 1;
        public bool IsExternalGame = false;
        public string GameURL = string.Empty;
        public string MachineExternalReference = string.Empty;
        //2.130.0
        public bool EraseQRPlayIdentifier = false;
        public string QRPlayIdentifier = string.Empty;
        public bool isPromotionConfigValueChanged = false;
        public int PromotionDetailId = -1;
        private MachineConfigurationClass PromotionConfiguration;

        public class TimerMachine
        {
            public int MachineId;
            public DateTime EndTime;
            public string PlayToken;
            public decimal CardBalance;
            public bool IgnoreCardTap = true;
        }
        public List<TimerMachine> TimerMachinesList = new List<TimerMachine>();

        public class TicketSessionClass
        {
            string SessionCardNumber = string.Empty;
            int TotalSessionTickets = 0;
            GameServerEnvironment gameServerEnvironment;
            Machine CurrentMachine;
            System.Timers.Timer TicketTimer;
            object LockObject = new object();

            public TicketSessionClass(GameServerEnvironment gameServerEnvironment, Machine CurrentMachine)
            {
                this.gameServerEnvironment = gameServerEnvironment;
                this.CurrentMachine = CurrentMachine;

                TicketTimer = new System.Timers.Timer();
                TicketTimer.Interval = 10 * 1000; // flush every 10 seconds
                TicketTimer.AutoReset = true;
                TicketTimer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
                {
                    if (TotalSessionTickets > 0)
                        flushTickets();
                };

                TicketTimer.Start();
            }

            public void SetCardTickets(string cardNumber, int ticketCount)
            {
                if (SessionCardNumber == string.Empty)
                {
                    SessionCardNumber = cardNumber;
                }
                else if (cardNumber != SessionCardNumber)
                {
                    if (TotalSessionTickets > 0)
                        flushTickets();
                    SessionCardNumber = cardNumber;
                }

                TotalSessionTickets += ticketCount;
            }

            void flushTickets()
            {
                if (Monitor.TryEnter(LockObject, 1))
                {
                    try
                    {
                        TicketTimer.Stop();
                        int curTickets = TotalSessionTickets;
                        int retries = 3;
                        while (curTickets > 0 && retries-- > 0)
                        {
                            try
                            {
                                List<AccountDTO> accountDTOList = new List<AccountDTO>();
                                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(gameServerEnvironment.GameEnvExecutionContext);
                                using (NoSynchronizationContextScope.Enter())
                                {
                                    Task<AccountDTOCollection> accountDTOCollection = accountUseCases.GetAccounts("Y", false, true, false, null, null, -1, -1, SessionCardNumber);
                                    accountDTOCollection.Wait();
                                    accountDTOList = accountDTOCollection.Result.data;
                                    if (accountDTOList == null ||
                                        (accountDTOList != null && accountDTOList.Count == 0))
                                    {
                                        AccountDTO accountDTO = new AccountDTO();
                                        accountDTO.TagNumber = "XYZCXYZC";
                                        if (accountDTOList == null)
                                            accountDTOList = new List<AccountDTO>();
                                        accountDTOList.Add(accountDTO);
                                    }
                                }
                                string message = string.Empty;
                                //GamePlay.UpdateTickets(CurrentMachine, SessionCardNumber, curTickets, gameServerEnvironment, ref message);
                                GamePlayDTO gamePlayDTO = new GamePlayDTO();
                                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(gameServerEnvironment.GameEnvExecutionContext);
                                using (NoSynchronizationContextScope.Enter())
                                {
                                    Task<GamePlayDTO> taskGamePlayDTO = gameTransactionUseCases.GameplayTickets(accountDTOList[0].AccountId, CurrentMachine.MachineId, curTickets);
                                    taskGamePlayDTO.Wait();
                                    gamePlayDTO = taskGamePlayDTO.Result;
                                }
                                retries = 0;
                            }
                            catch (Exception ex)
                            {
                                log.Info(ex.Message);
                            }
                        }
                        TotalSessionTickets -= curTickets;
                    }
                    finally
                    {
                        Monitor.Exit(LockObject);
                        TicketTimer.Start();
                    }
                }
            }

            public void Flush()
            {
                if (TotalSessionTickets > 0)
                    flushTickets();
            }
        }

        public TicketSessionClass TicketSession;

        public class TwoFactorClass
        {
            public enum States
            {
                NONE,
                PAUSED,
                DONE
            }
            public int _MaxAttempts;
            public int Attempts;
            public States State = States.NONE;
            public bool Successful = false;
            public string CardNumber;

            public void setMaxAttempts(int maxAttempts)
            {
                _MaxAttempts = maxAttempts;
            }

            public void resetAttempts()
            {
                Attempts = _MaxAttempts;
            }
        }
        public TwoFactorClass TwoFactor;
        public AdManagement adManagement;
        public MachineSocket machineSocket;
        public AdManagement.MachineAdContext machineAdContext;
        public AdManagement.MachineAdShowContext machineAdShowContext;
        public MachineConfigurationClass Configuration;
        public List<MachineConfigurationClass.clsConfig> ConfigurationList;
        Utilities _Utilities;
        ServerStatic _ServerStatic;
        Semnox.Core.Utilities.ExecutionContext ExecutionContext;
        GameServerEnvironment GameServerEnvironment;

        public List<Semnox.Parafait.Game.MachineInputDevice> InputDevices = new List<Semnox.Parafait.Game.MachineInputDevice>();
        List<CoreKeyValueStruct> gameDetailsList;

        Machine(ServerStatic inServerStatic)
        {
            log.LogMethodEntry(inServerStatic);
            _ServerStatic = inServerStatic;
            _Utilities = _ServerStatic.Utilities;
            //Configuration = new ConfigurationClass(_Utilities);

            RepeatPlayDelay = _ServerStatic.MIN_SECONDS_BETWEEN_REPEAT_PLAY;
            MaxTicketsPerGamePlay = _ServerStatic.MAX_TICKETS_PER_GAMEPLAY;
            log.LogMethodExit(null);
        }

        Machine(GameServerEnvironment inGameServerEnvironment)
        {
            log.LogMethodEntry(inGameServerEnvironment);
            //_ServerStatic = inServerStatic;
            //_Utilities = _ServerStatic.Utilities;
            GameServerEnvironment = inGameServerEnvironment;
            ExecutionContext = inGameServerEnvironment.GameEnvExecutionContext;
            Configuration = new MachineConfigurationClass(ExecutionContext);

            RepeatPlayDelay = GameServerEnvironment.MIN_SECONDS_BETWEEN_REPEAT_PLAY;
            MaxTicketsPerGamePlay = GameServerEnvironment.MAX_TICKETS_PER_GAMEPLAY;
            log.LogMethodExit(null);
        }

        public Machine(int pMachineId, ServerStatic inServerStatic)
            : this(inServerStatic)
        {
            log.LogMethodEntry(pMachineId, inServerStatic);
            MachineId = pMachineId;

            RefreshMachine();
            getInputDevices();
            log.LogMethodExit(null);
        }

        public Machine(int pMachineId, GameServerEnvironment inGameServerEnvironment)
            : this(inGameServerEnvironment)
        {
            log.LogMethodEntry(pMachineId, inGameServerEnvironment);
            MachineId = pMachineId;
            //RefreshMachine();
            //getInputDevices();
            log.LogMethodExit(null);
        }

        public Machine(Core.Utilities.ExecutionContext executionContext, int machineId)
            : this(new GameServerEnvironment(executionContext))
        {
            log.LogMethodEntry(executionContext, machineId);
            MachineId = machineId;
            log.LogMethodExit();
        }

        public GameCustomDTO RefreshMachine(bool isPromotionActive = false)
        {
            log.LogMethodEntry();
            string promo_bonus_allowed = "Y";
            string promo_courtesy_allowed = "Y";
            string promo_time_allowed = "Y";
            string promo_ticket_allowed = "Y";
            int promoThemeNumber = -1;
            int promoVisualizationThemeNumber = -1;

            MachineList machineList = new MachineList(ExecutionContext);
            GameCustomDTO gameCustomDTO = machineList.RefreshMachine(MachineId);

            log.LogVariableState("@machineId", MachineId);

            if (gameCustomDTO == null)
            {
                log.LogMethodExit(null, "Throwing  Application Exception-Machine not found");
                throw new ApplicationException("Machine not found");
            }

            //Populate base PromotionConfiguration class to identify differences
            //once promotion method is executed
            if (GameServerEnvironment.READER_HARDWARE_VERSION >= 1.4)
            {
                if (PromotionConfiguration == null)
                {
                    PromotionConfiguration = new MachineConfigurationClass(ExecutionContext);
                    PromotionConfiguration.Populate(this.MachineId, -1);
                    gameCustomDTO.PromotionConfiguration = new List<MachineConfigurationClass.clsConfig>();
                    gameCustomDTO.PromotionConfiguration.AddRange(PromotionConfiguration.Configuration);
                }
            }

            decimal promotion_credits = 0;
            decimal promotion_vip_credits = 0;

            int promotionId = -1;
            int promotionDetailId = -1;
            GamePromotionBL.getPromotionDetails(-2,
                                         Convert.ToInt32(gameCustomDTO.Game_Id),
                                         Convert.ToInt32(gameCustomDTO.Game_Profile_Id),
                                         Convert.ToDecimal(gameCustomDTO.PlayCredits),
                                         Convert.ToDecimal(gameCustomDTO.VipPlayCredits),
                                         ref promotion_credits,
                                         ref promotion_vip_credits,
                                         ref promo_bonus_allowed,
                                         ref promo_courtesy_allowed,
                                         ref promo_time_allowed,
                                         ref promo_ticket_allowed,
                                         ref promoThemeNumber,
                                         ref promoVisualizationThemeNumber,
                                         ref promotionId,
                                         ref promotionDetailId,
                                         ExecutionContext
                                         );

            gameCustomDTO.PlayCredits = Convert.ToDouble(promotion_credits);
            gameCustomDTO.VipPlayCredits = Convert.ToDouble(promotion_vip_credits);

            if (GameServerEnvironment.DISPLAY_VIP_PRICE_ONLY_IF_DIFFERENT == "N")
            {
                this.isVIPPrice = "Y";
            }
            else
            {
                if (promotion_credits == promotion_vip_credits)
                    this.isVIPPrice = "N";
                else
                    this.isVIPPrice = "Y";
            }
            gameCustomDTO.IsVIPPrice = this.isVIPPrice;

            this.MachineId = gameCustomDTO.MachineId;
            if (gameCustomDTO.ReferenceMachineId != -1)
                this.GameplayMachineId = gameCustomDTO.ReferenceMachineId;
            else
                this.GameplayMachineId = this.MachineId;

            this.GameId = gameCustomDTO.Game_Id;
            this.game_profile_id = gameCustomDTO.Game_Profile_Id;
            this.group_timer = gameCustomDTO.GroupTimer;
            this.machine_address = gameCustomDTO.MachineAddress;
            this.machine_name = gameCustomDTO.MachineName;
            this.number_of_coins = gameCustomDTO.NumberOfCoins.ToString().PadLeft(2, '0').Substring(0, 2);
            this.CoinPusher = gameCustomDTO.PhysicalToken;
            this.TicketEater = gameCustomDTO.TicketEater;
            this.play_credits = Convert.ToDecimal(gameCustomDTO.PlayCredits);
            this.RedeemTokenTo = gameCustomDTO.RedeemTokenTo;
            this.timer_interval = gameCustomDTO.TimerInterval;
            this.timer_machine = gameCustomDTO.TimerMachine;
            this.ToggleVIP = 0;
            this.TokenPrice = Convert.ToDecimal(gameCustomDTO.TokenPrice);
            this.TokenRedemption = gameCustomDTO.TokenRedemption;
            this.vip_play_credits = Convert.ToDecimal(gameCustomDTO.VipPlayCredits);
            this.TicketMode = gameCustomDTO.TicketMode == "D" ? (GameServerEnvironment.SITE_REAL_TICKET_MODE == "Y" ? "T" : "E") : gameCustomDTO.TicketMode.ToString();
            gameCustomDTO.TicketMode = this.TicketMode;

            if (promoThemeNumber == -1)
                this.ThemeNumber = gameCustomDTO.ThemeNumber.PadLeft(2, '0').Substring(0, 2);
            else
                this.ThemeNumber = promoThemeNumber.ToString().PadLeft(2, '0').Substring(0, 2);

            this.ShowAd = gameCustomDTO.ShowAd == "Y" ? "1" : "0";

            this.credit_allowed = gameCustomDTO.CreditAllowed == "Y" ? "1" : "0";
            this.bonus_allowed = gameCustomDTO.BonusAllowed == "Y" ? "1" : "0";
            this.courtesy_allowed = gameCustomDTO.CourtesyAllowed == "Y" ? "1" : "0";
            this.time_allowed = gameCustomDTO.TimeAllowed == "Y" ? "1" : "0";

            this.ForceRedeemToCard = gameCustomDTO.ForceRedeemToCard;

            this.GameURL = gameCustomDTO.GameURL;
            this.IsExternalGame = gameCustomDTO.IsExternalGame;
            this.MachineExternalReference = gameCustomDTO.ExternalMachineReference;
            this.EraseQRPlayIdentifier = gameCustomDTO.EraseQRPlayIdentifier;
            this.QRPlayIdentifier = gameCustomDTO.QRPlayIdentifier;

            if (gameCustomDTO.InventoryLocationId != -1)
                this.InventoryLocationId = gameCustomDTO.InventoryLocationId;

            if (gameCustomDTO.ReaderType == -1)
                this.ReaderType = GameServerEnvironment.READER_TYPE;
            else
                this.ReaderType = gameCustomDTO.ReaderType;

            this.ConsecutiveTimePlayDelay = GameServerEnvironment.MIN_SECONDS_BETWEEN_GAMETIME_PLAY;

            try
            {
                if (GameServerEnvironment.READER_HARDWARE_VERSION >= 1.2)
                {
                    this.MachineAddress = Convert.ToInt32(this.machine_address.PadLeft(4, '0').Substring(2, 2));
                }
                else
                {
                    this.MachineAddress = Convert.ToInt32(this.machine_address.Substring(1, 1), 16);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in GameServerEnvironment.READER_HARDWARE_VERSION", ex);
            }

            //this.MacAddress = dataTable.Rows[i]["MacAddress"].ToString().Trim().Replace('-', ':').Replace(" ", "").ToUpper();
            this.MacAddress = gameCustomDTO.MacAddress.Trim().Replace('-', ':').Replace(" ", "").ToUpper();

            try
            {
                //this.ipAddress = System.Net.IPAddress.Parse(dataTable.Rows[i]["IPAddress"].ToString());
                this.ipAddress = System.Net.IPAddress.Parse(gameCustomDTO.IPAddress);
            }
            catch (Exception ex)
            {
                log.Debug("Error occured  in calculating the ip address. " + ex.Message);
            }

            //if (dataTable.Rows[i]["TCPPort"] != DBNull.Value)
            //    this.TCPPort = Convert.ToInt32(dataTable.Rows[i]["TCPPort"]);

            if (gameCustomDTO.TCPPort != -1)
                this.TCPPort = gameCustomDTO.TCPPort;

            if (this.TCPPort == 0)
                this.TCPPort = GameServerEnvironment.DEFAULT_TCP_PORT;

            if (GameServerEnvironment.READER_HARDWARE_VERSION >= 1.4)
            {
                this.Configuration.Populate(this.MachineId, promotionDetailId);
                //Set the Promotion config changed flag in case of promotion window
                //and if configuration values are different
                gameCustomDTO.Configuration = new List<MachineConfigurationClass.clsConfig>();
                gameCustomDTO.Configuration.AddRange(PromotionConfiguration.Configuration);
                isPromotionConfigValueChanged = CheckPromotionConfigChange(promotionDetailId);
                gameCustomDTO.PromotionDetailId = promotionDetailId;
                this.PromotionDetailId = promotionDetailId;
                if (isPromotionActive != isPromotionConfigValueChanged)
                {
                    isPromotionConfigValueChanged = true;
                    gameCustomDTO.IsPromotionConfigValueChanged = isPromotionConfigValueChanged;
                }
                if (gameCustomDTO.IsPromotionConfigValueChanged)
                {
                    gameCustomDTO.PromotionConfiguration = new List<MachineConfigurationClass.clsConfig>();
                    gameCustomDTO.PromotionConfiguration.AddRange(PromotionConfiguration.Configuration);
                }
                if (promoVisualizationThemeNumber == -1)
                {
                    string vt = this.Configuration.getValue("INITIAL_LED_PATTERN");
                    if (string.IsNullOrEmpty(vt.Trim()))
                        this.VisualizationThemeNumber = 0;
                    else
                        this.VisualizationThemeNumber = Convert.ToInt32(vt);
                }
                else
                    this.VisualizationThemeNumber = promoVisualizationThemeNumber;

                gameCustomDTO.VisualizationThemeNumber = this.VisualizationThemeNumber;

                string at = this.Configuration.getValue("AUDIO_THEME_NUMBER");
                if (string.IsNullOrEmpty(at.Trim()))
                    this.AudioThemeNumber = 0;
                else
                    this.AudioThemeNumber = Convert.ToInt32(at);

                gameCustomDTO.AudioThemeNumber = this.AudioThemeNumber;

                try
                {
                    this.RepeatPlayDelay = Convert.ToInt32(this.Configuration.getValue("MIN_SECONDS_BETWEEN_REPEAT_PLAY"));
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in min_seconds_between_repeat_play", ex);
                    this.RepeatPlayDelay = GameServerEnvironment.MIN_SECONDS_BETWEEN_REPEAT_PLAY;
                }

                gameCustomDTO.RepeatPlayDelay = this.RepeatPlayDelay;

                try
                {
                    this.MaxTicketsPerGamePlay = Convert.ToInt32(this.Configuration.getValue("MAX_TICKETS_PER_GAMEPLAY"));
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in Max_tickets_per_gameplay", ex);
                    this.MaxTicketsPerGamePlay = GameServerEnvironment.MAX_TICKETS_PER_GAMEPLAY;
                }
                gameCustomDTO.MaxTicketsPerGamePlay = this.MaxTicketsPerGamePlay;

                this.TicketEater = this.Configuration.getValue("TICKET_EATER") == "1" ? "Y" : "N";
                this.CoinPusher = this.Configuration.getValue("COIN_PUSHER_MACHINE") == "1" ? "Y" : "N";
                this.ShowAd = this.Configuration.getValue("SHOW_STATIC_AD") == "1" ? "Y" : "N";

                this.OutOfService = this.Configuration.getValue("OUT_OF_SERVICE") == "1";
                if (this.OutOfService)
                    this.ThemeNumber = this.Configuration.getValue("OUT_OF_SERVICE_THEME").PadLeft(2, '0').Substring(0, 2);
                else //Check Theme Calendar flag and mark OOS flag accordingly. Theme Number will be as per Calendar. 
                {
                    if (gameCustomDTO.EnabledOutOfservice)
                        this.OutOfService = gameCustomDTO.EnabledOutOfservice;
                    if (this.OutOfService)
                        this.ThemeNumber = gameCustomDTO.ThemeNumber.PadLeft(2, '0').Substring(0, 2);
                }
                gameCustomDTO.TicketEater = this.TicketEater;
                gameCustomDTO.CoinPusher = this.CoinPusher;
                gameCustomDTO.ShowAd = this.ShowAd;
                gameCustomDTO.EnabledOutOfservice = this.OutOfService;
                gameCustomDTO.ThemeNumber = this.ThemeNumber;

                if (GameServerEnvironment.READER_HARDWARE_VERSION >= 1.5)
                {
                    try
                    {
                        this.ConsecutiveTimePlayDelay = Convert.ToInt32(this.Configuration.getValue("GAMEPLAY_DURATION"));
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured in gameplay_duration", ex);
                        this.ConsecutiveTimePlayDelay = GameServerEnvironment.MIN_SECONDS_BETWEEN_GAMETIME_PLAY;
                    }
                    gameCustomDTO.ConsecutiveTimePlayDelay = this.ConsecutiveTimePlayDelay;
                }

                try
                {
                    this.GameplayMultiplier = Convert.ToInt32(this.Configuration.getValue("GAMEPLAY_MULTIPLIER"));
                    if (this.GameplayMultiplier <= 0)
                        this.GameplayMultiplier = 1;
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in Gameplay_multiplier", ex);
                }

                gameCustomDTO.GameplayMultiplier = this.GameplayMultiplier;

                int numCoins = 1;
                try
                {
                    numCoins = Convert.ToInt32(this.Configuration.getValue("NUMBER_OF_COINS"));
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in Number_of_coins", ex);
                }

                this.number_of_coins = (numCoins * this.GameplayMultiplier).ToString().PadLeft(2, '0').Substring(0, 2);
                gameCustomDTO.NumberOfCoins = this.number_of_coins;

            }
            gameCustomDTO.MachineInputDevicesListDTO = getInputDevices();
            gameCustomDTO.LastRefreshTime = ServerDateTime.Now;
            LastRefreshTime = ServerDateTime.Now;
            log.LogMethodExit(gameCustomDTO);
            return gameCustomDTO;
        }

        /// <summary>
        /// Method to identify if Promotion specific configuration are modified
        /// Comparison is done with existing class
        /// </summary>
        /// <returns>Boolean</returns>
        bool CheckPromotionConfigChange(int PromotionDetailId)
        {
            log.LogMethodEntry();
            bool isConfigChanged = false;
            if (PromotionConfiguration != null)
            {
                List<MachineConfigurationClass.clsConfig> promotionConfigList = PromotionConfiguration.GetConfigList(true);
                foreach (MachineConfigurationClass.clsConfig config in promotionConfigList)
                {
                    string value = this.Configuration.getValue(config.ConfigParameter);
                    if (!value.Equals(config.Value)) //Config value does not match for Promotion configuration
                    {
                        if (!isConfigChanged)
                        {
                            log.LogVariableState("isConfigChanged: ", isConfigChanged);
                            isConfigChanged = true;
                        }
                        log.LogVariableState("Promotion Config parameter: " + config.ConfigParameter + " updated with: ", value);
                        PromotionConfiguration.addValue(config.ConfigParameter, value, config.EnableForPromotion);
                    }
                }
            }
            else
            {
                PromotionConfiguration = new MachineConfigurationClass(ExecutionContext);
                if (this.Configuration != null)
                {
                    List<MachineConfigurationClass.clsConfig> promotionConfigList = this.Configuration.GetConfigList(true);
                    foreach (MachineConfigurationClass.clsConfig config in promotionConfigList)
                    {
                        PromotionConfiguration.addValue(config.ConfigParameter, config.Value, config.EnableForPromotion);
                    }
                }
            }
            if (!isConfigChanged
              && PromotionDetailId > -1
              && PromotionConfiguration != null
              && PromotionConfiguration.Configuration.Exists(x => x.EnableForPromotion))
            {
                isConfigChanged = true;
            }

            log.LogMethodExit(isConfigChanged);
            return isConfigChanged;
        }

        List<MachineInputDevicesDTO> getInputDevices()
        {
            log.LogMethodEntry();
            MachineInputDevicesList machineInputDevicesList = new MachineInputDevicesList();//not done
            List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.MACHINE_ID, MachineId.ToString()));
            List<MachineInputDevicesDTO> machineInputDevicesDTOList = machineInputDevicesList.GetAllMachineInputDevicesDTOList(searchParameters);
            foreach (MachineInputDevicesDTO machineInputDevicesDTO in machineInputDevicesDTOList)
            {
                int deviceId = machineInputDevicesDTO.DeviceId;

                Semnox.Parafait.Game.MachineInputDevice.InputDeviceType deviceType = Semnox.Parafait.Game.MachineInputDevice.InputDeviceType.FingerPrint;
                if (!string.IsNullOrEmpty(machineInputDevicesDTO.DeviceType))
                    Enum.TryParse(machineInputDevicesDTO.DeviceType, true, out deviceType);

                Semnox.Parafait.Game.MachineInputDevice.InputDeviceModel deviceModel = Semnox.Parafait.Game.MachineInputDevice.InputDeviceModel.Other;
                if (!string.IsNullOrEmpty(machineInputDevicesDTO.DeviceModel))
                    Enum.TryParse(machineInputDevicesDTO.DeviceModel, true, out deviceModel);

                Semnox.Parafait.Game.MachineInputDevice.FPTemplateFormats FPTemplateFormat = Semnox.Parafait.Game.MachineInputDevice.FPTemplateFormats.None;
                if (!string.IsNullOrEmpty(machineInputDevicesDTO.FPTFormat))
                    Enum.TryParse(machineInputDevicesDTO.FPTFormat, true, out FPTemplateFormat);

                string IPAddress = machineInputDevicesDTO.IPAddress;
                string MacAddress = machineInputDevicesDTO.MacAddress;

                int portNo = -1;
                if (machineInputDevicesDTO.PortNo > -1)
                    portNo = machineInputDevicesDTO.PortNo;

                Semnox.Parafait.Game.MachineInputDevice device = new Semnox.Parafait.Game.MachineInputDevice(deviceId, deviceType, deviceModel, IPAddress, portNo, MacAddress, FPTemplateFormat);
                InputDevices.Add(device);
            }
            log.LogVariableState("@machineId", MachineId);
            log.LogMethodExit(machineInputDevicesDTOList);
            return machineInputDevicesDTOList;
            //DataTable dt = _Utilities.executeDataTable(@"select mid.*, l1.LookupValue DeviceType, l2.LookupValue DeviceModel, l3.LookupValue FPTFormat
            //                                            from MachineInputDevices mid 
            //                                                left outer join LookupView l1
            //                                                    on l1.LookupValueId = mid.DeviceTypeId
            //                                                    and l1.LookupName = 'MACHINE_INPUT_DEVICE_TYPE'
            //                                                left outer join LookupView l2
            //                                                    on l2.LookupValueId = mid.DeviceModelId
            //                                                    and l2.LookupName = 'MACHINE_INPUT_DEVICE_MODEL'
            //                                                left outer join LookupView l3
            //                                                    on l3.LookupValueId = mid.FPTemplateFormat
            //                                                    and l3.LookupName = 'FP_TEMPLATE_FORMAT'
            //                                            where mid.MachineId = @machineId 
            //                                            and IsActive = 1",
            //                                            new SqlParameter("@machineId", MachineId));
            //log.LogVariableState("@machineId", MachineId);
            //List<MachineInputDevicesDTO> machineInputDevicesDTOList = new List<MachineInputDevicesDTO>();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    int deviceId = (int)dr["DeviceId"];

            //    Semnox.Parafait.Game.MachineInputDevice.InputDeviceType deviceType = Semnox.Parafait.Game.MachineInputDevice.InputDeviceType.FingerPrint;
            //    if (dr["DeviceType"] != null)
            //        Enum.TryParse(dr["DeviceType"].ToString(), true, out deviceType);

            //    Semnox.Parafait.Game.MachineInputDevice.InputDeviceModel deviceModel = Semnox.Parafait.Game.MachineInputDevice.InputDeviceModel.Other;
            //    if (dr["DeviceModel"] != null)
            //        Enum.TryParse(dr["DeviceModel"].ToString(), true, out deviceModel);

            //    Semnox.Parafait.Game.MachineInputDevice.FPTemplateFormats FPTemplateFormat = Semnox.Parafait.Game.MachineInputDevice.FPTemplateFormats.None;
            //    if (dr["FPTFormat"] != DBNull.Value)
            //        Enum.TryParse(dr["FPTFormat"].ToString(), true, out FPTemplateFormat);

            //    string IPAddress = dr["IPAddress"].ToString();
            //    string MacAddress = dr["MacAddress"].ToString();
            //    int deviceModelId = (int)dr["DeviceModelId"];
            //    int deviceTypeId = (int)dr["DeviceTypeId"];
            //    string deviceName = dr["DeviceName"].ToString();

            //    int portNo = -1;
            //    if (dr["PortNo"] != DBNull.Value)
            //        portNo = Convert.ToInt32(dr["PortNo"]);
            //    MachineInputDevicesDTO machineInputDevicesDTO = new MachineInputDevicesDTO(deviceId,deviceName,deviceTypeId,
            //        deviceModelId, MachineId, true,IPAddress,portNo,MacAddress, Convert.ToInt32(FPTemplateFormat));
            //    machineInputDevicesDTOList.Add(machineInputDevicesDTO);
            //    Semnox.Parafait.Game.MachineInputDevice device = new Semnox.Parafait.Game.MachineInputDevice(deviceId, deviceType, deviceModel, IPAddress, portNo, MacAddress, FPTemplateFormat);
            //    InputDevices.Add(device);
            //}
            //log.LogMethodExit(machineInputDevicesDTOList);
            //return machineInputDevicesDTOList;

        }

        public Semnox.Parafait.Device.Biometric.FingerPrintReader GetFingerPrintReader()
        {
            log.LogMethodEntry();
            foreach (Semnox.Parafait.Game.MachineInputDevice mid in InputDevices)
            {
                if (mid.DeviceType == Semnox.Parafait.Game.MachineInputDevice.InputDeviceType.FingerPrint)
                {
                    if (mid.DeviceModel == Semnox.Parafait.Game.MachineInputDevice.InputDeviceModel.FutronicFS84)
                    {
                        if (mid.PortNo <= 0)
                            mid.PortNo = 5001;
                        Semnox.Parafait.Device.Biometric.FingerPrintReader fpr = new Semnox.Parafait.Device.Biometric.FutronicFS84.FutronicFS84(mid.IPAddress, mid.PortNo);
                        log.LogMethodExit(fpr);
                        return fpr;
                    }
                }
            }
            log.LogMethodExit(null);
            return null;
        }

        public class ConfigurationClass
        {
            Utilities Utilities;
            Core.Utilities.ExecutionContext ConfigExecutionContext;
            public ConfigurationClass(Utilities inUtilities)
            {
                log.LogMethodEntry(inUtilities);
                Utilities = inUtilities;
                log.LogMethodExit(null);
            }

            public ConfigurationClass(Core.Utilities.ExecutionContext inExecutionContext)
            {
                log.LogMethodEntry(inExecutionContext);
                ConfigExecutionContext = inExecutionContext;
                log.LogMethodExit(null);
            }

            public class clsConfig
            {
                public string configParameter;
                public string Value;
                public bool enableForPromotion;

                public clsConfig(string _configParameter, string _Value, bool _enableForPromotion)
                {
                    log.LogMethodEntry(_configParameter, _Value, _enableForPromotion);
                    configParameter = _configParameter;
                    Value = _Value;
                    enableForPromotion = _enableForPromotion;
                    log.LogMethodExit(null);
                }
            }

            List<clsConfig> Configuration = new List<clsConfig>();


            public string getValue(string ConfigParameter)
            {
                log.LogMethodEntry(ConfigParameter);
                clsConfig config = Configuration.Find(delegate (clsConfig keyValue) { return keyValue.configParameter == ConfigParameter; });
                if (config != null)
                {
                    log.LogMethodExit(config.Value);
                    return config.Value;
                }

                else
                {
                    log.LogMethodExit("0");
                    return "0";
                }
            }

            /// <summary>
            /// Added method to give configuration object based on enableForPromotio flag
            /// </summary>
            /// <param name="enableForPromotion">Bool</param>
            /// <returns>List(ClsConfig)</returns>
            internal List<clsConfig> GetConfigList(bool enableForPromotion)
            {
                log.LogMethodEntry(enableForPromotion);
                List<clsConfig> listConfig = Configuration.FindAll(x => x.enableForPromotion);
                log.LogMethodExit(listConfig);
                return listConfig;
            }

            //Added new parameter to set EnableForPromotion property from GameProfileAttributes
            public void addValue(string ConfigParameter, string Value, bool EnableForPromotion)
            {
                log.LogMethodEntry(ConfigParameter, Value, EnableForPromotion);
                clsConfig keyValue = Configuration.Find(delegate (clsConfig searchKeyValue) { return searchKeyValue.configParameter == ConfigParameter; });
                if (keyValue == null)
                {
                    keyValue = new clsConfig(ConfigParameter, Value, EnableForPromotion);
                    Configuration.Add(keyValue);
                }
                else
                {
                    keyValue.Value = Value;
                    keyValue.enableForPromotion = EnableForPromotion;
                }
                log.LogMethodExit(null);
            }

            /// <summary>
            /// Populate configuration value considering Promotion, PromotionDetail
            /// GameProfile, Game, Machine
            /// </summary>
            /// <param name="MachineId">Machineid</param>
            /// <param name="PromotionDetailId">Promotion Detail Id if applicable else -1</param>
            public void Populate(int MachineId, int PromotionDetailId)
            {
                log.LogMethodEntry(MachineId, PromotionDetailId);
                DataTable dt = Utilities.executeDataTable(
                                @"select defaults.attribute attribute, 
                                         isnull(gpapd.attributeValue, 
                                                isnull(gpap.attributeValue, 
                                                       isnull(gpa.attributeValue, defaults.attributeValue))) value,
		                                 isnull(defaults.EnableForPromotion, 0) enableForPromotion
                                    from (
                                        select a.attribute, a.EnableForPromotion, isnull(g1.attributeValue, isnull(g2.attributeValue, g3.attributeValue)) attributeValue, a.attributeId
                                        from GameProfileAttributes a 
                                                left outer join GameProfileAttributeValues g1
                                                on g1.game_id = (select game_id from machines where machine_id = @machine_id)
                                                and a.attributeId = g1.attributeId
                                                left outer join GameProfileAttributeValues g2
                                                on (g2.game_profile_id = (select game_profile_id from games g, machines m
                                                                        where m.machine_id = @machine_id and g.game_id = m.game_id))
                                                and a.attributeId = g2.attributeId
                                                left outer join GameProfileAttributeValues g3
                                                on a.attributeId = g3.attributeId 
                                                    and g3.game_profile_id is null 
                                                    and g3.game_id is null 
                                                    and g3.machine_id is null
													AND g3.PromotionId is null
													AND g3.PromotionDetailId is null) defaults
                                left outer join GameProfileAttributeValues gpa
                                    on defaults.attributeId = gpa.attributeId
                                    and machine_id = @machine_id
                                left outer join GameProfileAttributeValues gpap
                                    on defaults.attributeId = gpap.attributeId
                                    and gpap.PromotionId = (select promotion_id from promotion_detail 
                                                             where promotion_detail_id = @promotionDetailId)
                                left outer join GameProfileAttributeValues gpapd
                                    on defaults.attributeId = gpapd.attributeId
                                    and gpapd.PromotionDetailId = @promotionDetailId",
                            new SqlParameter("machine_id", MachineId),
                            new SqlParameter("promotionDetailId", PromotionDetailId));
                log.LogVariableState("machine_id", MachineId);
                log.LogVariableState("promotionDetailId", PromotionDetailId);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    addValue(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), Convert.ToBoolean(dt.Rows[i][2]));
                }
                log.LogMethodExit(null);
            }
        }


        /// <summary>
        /// Returns game details matching the parameters passed
        /// </summary>
        /// <param name="macAddress">macAddress</param>
        /// <param name="machineId">machineId</param>
        /// <returns>Returns key value pair</returns>
        public List<CoreKeyValueStruct> GetGameDetails(string macAddress, string machineId)
        {
            log.Debug("Starts-GetGameDetails(string macAddress, string machineId) Method");

            List<CoreKeyValueStruct> gameDetails = new List<CoreKeyValueStruct>();
            string promo_bonus_allowed = "Y";
            string promo_courtesy_allowed = "Y";
            string promo_time_allowed = "Y";
            string promo_ticket_allowed = "Y";
            int themeNumber = 0; //Theme based promotion

            try
            {
                int machineIdInternal;

                List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> mSearchParams = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, "Y"));

                if (int.TryParse(machineId, out machineIdInternal) == false)
                    mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACADDRESS, macAddress));
                else
                    mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_ID, machineIdInternal.ToString()));

                MachineDataHandler machineDataHandler = new MachineDataHandler(null);
                List<MachineDTO> machineList = machineDataHandler.GetMachineList(mSearchParams);
                if (machineList != null && machineList.Any())
                {
                    MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(null);
                    foreach (MachineDTO machine in machineList)
                    {
                        List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machine.MachineId, _Utilities.ExecutionContext.GetSiteId());
                        machine.SetAttributeList(machineAttributes);
                    }
                }

                if (machineList == null)
                {
                    gameDetails.Add(new CoreKeyValueStruct("ErrorMsg", "Invalid game machine"));
                    return gameDetails;
                }
                else if (machineList.Count > 1)
                {
                    gameDetails.Add(new CoreKeyValueStruct("ErrorMsg", "Multiple game machines setup with the same address"));
                    return gameDetails;
                }

                System.Data.DataTable dataTable = new System.Data.DataTable();
                dataTable = new GameDataHandler().GetGameDetailsWithProfile(machineList[0].MachineId);
                if (dataTable.Rows.Count == 0)
                {
                    gameDetails.Add(new CoreKeyValueStruct("ErrorMsg", "Invalid game machine"));
                    return gameDetails;
                }

                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                using (Utilities parafaitUtility = new Utilities(dataAccessHandler.ConnectionString))
                {
                    decimal promotion_credits = 0;
                    decimal promotion_vip_credits = 0;

                    String creditBeforeBonus = parafaitUtility.getParafaitDefaults("CONSUME_CREDITS_BEFORE_BONUS");

                    //added promotionId parameter = 0 as part of may release fixes, 16-Jun-2017
                    int promotionId = 0;
                    int promotionDetailId = 0;
                    Promotions.getPromotionDetails(-1, Convert.ToInt32(dataTable.Rows[0]["game_id"]),
                                                    Convert.ToInt32(dataTable.Rows[0]["game_profile_id"]),
                                                    Convert.ToDecimal(dataTable.Rows[0]["play_credits"]),
                                                    Convert.ToDecimal(dataTable.Rows[0]["vip_play_credits"]),
                                                    ref promotion_credits,
                                                    ref promotion_vip_credits,
                                                    ref promo_bonus_allowed,
                                                    ref promo_courtesy_allowed,
                                                    ref promo_time_allowed,
                                                    ref promo_ticket_allowed,
                                                    ref themeNumber, ref themeNumber, ref promotionId, ref promotionDetailId, parafaitUtility); //Theme based promotion

                    dataTable.Rows[0]["play_credits"] = promotion_credits;
                    dataTable.Rows[0]["vip_play_credits"] = promotion_vip_credits;
                    gameDetails.Add(new CoreKeyValueStruct("GameName", dataTable.Rows[0]["machine_name"].ToString()));
                    gameDetails.Add(new CoreKeyValueStruct("GamePrice", dataTable.Rows[0]["play_credits"].ToString().Replace(',', '.')));
                    gameDetails.Add(new CoreKeyValueStruct("GameVIPPrice", dataTable.Rows[0]["vip_play_credits"].ToString().Replace(',', '.')));
                    gameDetails.Add(new CoreKeyValueStruct("CreditsBeforeBonus", creditBeforeBonus));
                    gameDetails.Add(new CoreKeyValueStruct("CreditsAllowed", dataTable.Rows[0]["credit_allowed"].ToString()));
                    gameDetails.Add(new CoreKeyValueStruct("BonusAllowed", promo_bonus_allowed));
                    gameDetails.Add(new CoreKeyValueStruct("CourtesyAllowed", promo_courtesy_allowed));
                    gameDetails.Add(new CoreKeyValueStruct("CCPlusAllowed", dataTable.Rows[0]["credit_allowed"].ToString()));
                    gameDetails.Add(new CoreKeyValueStruct("GameProfileId", dataTable.Rows[0]["gameProfileId"].ToString()));
                    gameDetails.Add(new CoreKeyValueStruct("GameId", dataTable.Rows[0]["gameId"].ToString()));
                    gameDetails.Add(new CoreKeyValueStruct("GamesAllowed", "Y"));

                }
            }
            catch (Exception ex)
            {
                log.Debug("Ends-GetGameDetails(string macAddress, string machineId) by throwing exception");
                gameDetails.Clear();
                gameDetails.Add(new CoreKeyValueStruct("ErrorMsg", ex.Message));
            }
            log.Debug("Ends-GetGameDetails(string macAddress, string machineId) by returning listOfValues");

            return gameDetails;
        }

        /// <summary>
        /// Method to play game involving game play count and Game price tier
        /// </summary>
        /// <param name="machineId">Machine Id</param>
        /// <param name="cardNumber">Tag Number</param>
        /// <param name="multiGamePlayReference">Common Guid to mark multiple game plays</param>
        /// <param name="gamePriceTierId">Price Tier id selected for a game</param>
        /// <param name="gamePlayCount">Count of Game Plays selected</param>
        /// <param name="inSQLTrx">Sql Transaction</param>
        /// <returns>GamePlayDTO</returns>
        public List<GamePlayDTO> PlayGame(int machineId, string cardNumber, Guid? multiGamePlayReference, int gamePriceTierId, int gamePlayCount, SqlTransaction inSQLTrx = null)
        {
            log.Debug("Starts-int machineId, string cardNumber, Guid? multiGamePlayReference, int gamePriceTierId, int gamePlayCount Method");
            log.LogMethodEntry(machineId, cardNumber, multiGamePlayReference, gamePriceTierId, gamePlayCount);
            if (machineId == -1)
            {
                throw new Exception("Machine is Invalid");
            }
            else if (String.IsNullOrEmpty(cardNumber))
            {
                throw new Exception("Card Number is Invalid");
            }
            if (gamePlayCount <= 0)
            {
                throw new Exception("Game Play count cannot be zero");
            }

            DateTime currentTime = DateTime.Now;
            DataAccessHandler dataAccessHandler = new DataAccessHandler();

            GameServerEnvironment gameServerEnvironment = null;            
            gameServerEnvironment = this.GameServerEnvironment;

            
            string gamepriceTierInfo = string.Empty;

            GamePriceTierDTO gamePriceTierDTO = null;
            if(gamePriceTierId > -1)
            {
                gamePriceTierDTO = new GamePriceTierBL(ExecutionContext, gamePriceTierId, inSQLTrx).GamePriceTierDTO;
            }
            if (gamePriceTierDTO != null && gamePriceTierDTO.GamePriceTierId > -1)
            {//If gamePriceTier is provided, derive play count from Game Price Tier, else use parameter value
                gamepriceTierInfo =  gamePriceTierDTO.PlayCount.ToString();
                gamePlayCount = gamePriceTierDTO.PlayCount;
            }
            //In case Game play reference is not passed, count or tier id is passed then generate game play reference
            if ((gamePriceTierId > -1 || gamePlayCount > 1) && multiGamePlayReference == null)
            {
                multiGamePlayReference = Guid.NewGuid();
            }

            



            //string gamePlayMessage = "";
            string statusMessage = string.Empty;
            string detailedStatus = string.Empty;
            List<GamePlayDTO> gamePlayDTOList = new List<GamePlayDTO>();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                if (inSQLTrx != null)
                {
                    parafaitDBTrx.SQLTrx = inSQLTrx;
                }
                else
                {
                    parafaitDBTrx.BeginTransaction();
                }
                ValidateBalance(cardNumber, gameServerEnvironment, multiGamePlayReference, gamePlayCount, gamePriceTierId, parafaitDBTrx);
                for (int i = 1; i <= gamePlayCount; i++)
                {
                    bool proceedWithGamePlay = false;
                    statusMessage = string.Empty;
                    detailedStatus = string.Empty;
                    int tempGamePlayCount = 1;//One gameplay at a time using gamepricetier info
                                              //Semnox.Parafait.ServerCore.Machine localMachine = new Semnox.Parafait.ServerCore.Machine(Convert.ToInt32(machineDTO.MachineId), serverstatic);
                    Semnox.Parafait.ServerCore.GameServerEnvironment.GameServerPlayDTO gamePlayStats = new Semnox.Parafait.ServerCore.GameServerEnvironment.GameServerPlayDTO(this.MachineId);
                    // localMachine.MachineId = machineId; commented and used constructor as above. Change to handle reference machine id
                    GameTransactionBL gameTransactionBL = new GameTransactionBL(ExecutionContext, this.MachineId);
                    gamePlayStats = gameTransactionBL.getGamePlayDetails(this.MachineId, cardNumber, gameServerEnvironment, multiGamePlayReference, tempGamePlayCount, gamePriceTierId, parafaitDBTrx.SQLTrx);
                    if (this.OutOfService && gamePlayStats.TechCard.Equals('Y') == false)
                    {
                        string errorMessage = "Wait:No Service";
                        log.Error(this.machine_name + " out of service");
                        log.LogMethodExit("Throwing Exception - " + errorMessage);
                        throw new Exception(errorMessage);
                    }
                    switch (gamePlayStats.GamePlayType)
                    {
                        case GameServerEnvironment.VALIDCREDIT:
                        case GameServerEnvironment.VALIDBONUS:
                        case GameServerEnvironment.VALIDCREDITBONUS:
                        case GameServerEnvironment.VALIDCOURTESY:
                        case GameServerEnvironment.VALIDCREDITCOURTESY:
                        case GameServerEnvironment.TIMEPLAY:
                        case GameServerEnvironment.VALIDCARDGAME:
                            proceedWithGamePlay = true;
                            break;
                        case GameServerEnvironment.LOBALCREDIT:
                            statusMessage = "Low Balance, please recharge";
                            break;
                        case GameServerEnvironment.INVCARD:
                            statusMessage = "Invalid card, please check";
                            break;
                        case GameServerEnvironment.GAMETIMEMINGAP:
                            int GameTimeMinGap = gameServerEnvironment.MIN_SECONDS_BETWEEN_GAMETIME_PLAY;
                            statusMessage = "Wait for " + GameTimeMinGap + " seconds";
                            break;
                        case GameServerEnvironment.TECHPLAY:
                            proceedWithGamePlay = true;
                            statusMessage = "Technician Play";
                            break;
                        case GameServerEnvironment.MULTIPLESWIPE:
                            statusMessage = "Wait for 3 seconds";
                            break;
                        case GameServerEnvironment.NOTALLOWED:
                            statusMessage = "Card not allowed";
                            break;
                        case GameServerEnvironment.RESETTIMER:
                            statusMessage = "Sending off command";
                            break;
                        case GameServerEnvironment.NOENTITLEMENT:
                            statusMessage = "Card does not have balance to play this game, please check the card or recharge";
                            break;
                        default:
                            statusMessage = "Invalid response. Please retry";
                            break;
                    }

                    long gamePlayId = -1;

                    if (proceedWithGamePlay == true)
                    {
                        string gamePlayCreateMessage = "";
                        //gamePlayId = GamePlay.CreateGamePlay(machineId, cardNumber, gamePlayStats, gameServerEnvironment, ref gamePlayCreateMessage, inSQLTrx);
                        AccountBL accountBL = new AccountBL(ExecutionContext, cardNumber, false, false, parafaitDBTrx.SQLTrx);
                        GamePlayDTO gamePlaysDTO = new GamePlayDTO();
                        gamePlaysDTO.MachineId = this.MachineId;
                        gamePlaysDTO.CardId = accountBL.AccountDTO.AccountId;
                        GamePlayBuildDTO gamePlayBuildDTO = new GamePlayBuildDTO("R", gamePlaysDTO, gamePlayStats, string.Empty, string.Empty, string.Empty, string.Empty, null, -1, true, multiGamePlayReference, gamepriceTierInfo);
                        try
                        {
                            //string message = string.Empty;
                            //gamePlaysDTO = gameTransactionBL.PlayGame(accountBL.AccountDTO.AccountId, gamePlayBuildDTO);
                            Utilities utilities = GetUtility(GameServerEnvironment.GameEnvExecutionContext);
                            gamePlayId = GameTransactionBL.CreateGamePlay(gamePlayBuildDTO.GamePlayDTO.MachineId, accountBL.AccountDTO.TagNumber, gamePlayStats, GameServerEnvironment, gamePlayBuildDTO.MultiGamePlayReference, gamePlayBuildDTO.GamePriceTierInfo, ref gamePlayCreateMessage, parafaitDBTrx.SQLTrx, utilities);
                            gamePlaysDTO = new GamePlayBL(ExecutionContext, Convert.ToInt32(gamePlayId), parafaitDBTrx.SQLTrx).GamePlayDTO;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            gamePlayCreateMessage += ex.Message;
                            parafaitDBTrx.RollBack();
                            throw new Exception(gamePlayCreateMessage);
                        }
                        gamePlayId = gamePlaysDTO.GameplayId;
                        if (gamePlayId != -1)
                        {
                            statusMessage = "Success";
                            detailedStatus = gamePlayCreateMessage;
                            gamePlayStats.RequestProcessed = true;
                            gamePlayDTOList.Add(gamePlaysDTO);
                        }
                        else
                            throw new Exception(gamePlayCreateMessage);
                    }
                    else
                        throw new Exception(statusMessage);
                }
                parafaitDBTrx.EndTransaction();
            }
            log.LogMethodExit(gamePlayDTOList);
            return gamePlayDTOList;
        }

        private void ValidateBalance(string cardNumber, GameServerEnvironment gameServerEnvironment, Guid? multiGamePlayReference, int gamePlayCount, int gamePriceTierId, ParafaitDBTransaction parafaitDBTrx)
        {
            log.LogMethodEntry(cardNumber, gameServerEnvironment, multiGamePlayReference, gamePlayCount, gamePriceTierId, parafaitDBTrx);
            bool proceedWithGamePlay = false;
            string statusMessage = string.Empty;
            Semnox.Parafait.ServerCore.GameServerEnvironment.GameServerPlayDTO gamePlayStats = new Semnox.Parafait.ServerCore.GameServerEnvironment.GameServerPlayDTO(this.MachineId);
            GameTransactionBL gameTransactionBL = new GameTransactionBL(ExecutionContext, this.MachineId);
            gamePlayStats = gameTransactionBL.getGamePlayDetails(this.MachineId, cardNumber, gameServerEnvironment, multiGamePlayReference, gamePlayCount, gamePriceTierId, parafaitDBTrx.SQLTrx);
            if (this.OutOfService && gamePlayStats.TechCard.Equals('Y') == false)
            {
                string errorMessage = "Wait:No Service";
                log.Error(this.machine_name + " out of service");
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            switch (gamePlayStats.GamePlayType)
            {
                case GameServerEnvironment.VALIDCREDIT:
                case GameServerEnvironment.VALIDBONUS:
                case GameServerEnvironment.VALIDCREDITBONUS:
                case GameServerEnvironment.VALIDCOURTESY:
                case GameServerEnvironment.VALIDCREDITCOURTESY:
                case GameServerEnvironment.TIMEPLAY:
                case GameServerEnvironment.VALIDCARDGAME:
                    proceedWithGamePlay = true;
                    break;
                case GameServerEnvironment.LOBALCREDIT:
                    statusMessage = "Low Balance, please recharge";
                    break;
                case GameServerEnvironment.INVCARD:
                    statusMessage = "Invalid card, please check";
                    break;
                case GameServerEnvironment.GAMETIMEMINGAP:
                    int GameTimeMinGap = gameServerEnvironment.MIN_SECONDS_BETWEEN_GAMETIME_PLAY;
                    statusMessage = "Wait for " + GameTimeMinGap + " seconds";
                    break;
                case GameServerEnvironment.TECHPLAY:
                    proceedWithGamePlay = true;
                    statusMessage = "Technician Play";
                    break;
                case GameServerEnvironment.MULTIPLESWIPE:
                    statusMessage = "Wait for 3 seconds";
                    break;
                case GameServerEnvironment.NOTALLOWED:
                    statusMessage = "Card not allowed";
                    break;
                case GameServerEnvironment.RESETTIMER:
                    statusMessage = "Sending off command";
                    break;
                case GameServerEnvironment.NOENTITLEMENT:
                    statusMessage = "Card does not have balance to play this game, please check the card or recharge";
                    break;
                default:
                    statusMessage = "Invalid response. Please retry";
                    break;
            }

            if (proceedWithGamePlay == false)
            {
                throw new Exception(statusMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// PlayGame(MachineDTO machineDTO, CardCoreDTO cardCoreDTO) method 
        /// </summary>
        /// <param name="machineDTO">MachineDTO</param>
        /// <param name="cardCoreDTO">CardCoreDTO</param>
        /// <returns>GamePlayDTO object</returns>
        public GamePlayDTO PlayGame(int machineId, string cardNumber, SqlTransaction inSQLTrx = null)
        {
            log.Debug("Starts-PlayGame(MachineDTO machineDTO, CardCoreDTO cardCoreDTO) Method");

            if (machineId == -1)
            {
                throw new Exception("Machine is Invalid");
            }
            else if (String.IsNullOrEmpty(cardNumber))
            {
                throw new Exception("Card Number is Invalid");
            }

            DateTime currentTime = DateTime.Now;
            DataAccessHandler dataAccessHandler = new DataAccessHandler();

            GameServerEnvironment gameServerEnvironment = null;

            //Utilities parafaitUtility = null;
            //if (this._ServerStatic == null)
            //{
            //    parafaitUtility = new Utilities(dataAccessHandler.ConnectionString);//new Semnox.Core.DataAccess.DataAccessHandler().ConnectionString);
            //    parafaitUtility.ParafaitEnv.Initialize();

            //    serverstatic = new Semnox.Parafait.ServerCore.ServerStatic(parafaitUtility);
            //    serverstatic.Initialize();
            //}
            //else
            {
                gameServerEnvironment = this.GameServerEnvironment;
            }

            bool proceedWithGamePlay = false;

            if (machineId != -1)
            {
                string gamePlayMessage = "";
                string statusMessage = "";
                string detailedStatus = "";
                //Semnox.Parafait.ServerCore.Machine localMachine = new Semnox.Parafait.ServerCore.Machine(Convert.ToInt32(machineDTO.MachineId), serverstatic);
                Semnox.Parafait.ServerCore.GameServerEnvironment.GameServerPlayDTO gamePlayStats = new Semnox.Parafait.ServerCore.GameServerEnvironment.GameServerPlayDTO(this.MachineId);
                // localMachine.MachineId = machineId; commented and used constructor as above. Change to handle reference machine id
                GameTransactionBL gameTransactionBL = new GameTransactionBL(ExecutionContext, this.MachineId);
                gamePlayStats = gameTransactionBL.getGamePlayDetails(this.MachineId, cardNumber, gameServerEnvironment, inSQLTrx);
                if (this.OutOfService && gamePlayStats.TechCard.Equals('Y') == false)
                {
                    string errorMessage = "Wait:No Service";
                    log.Error(this.machine_name + " out of service");
                    log.LogMethodExit("Throwing Exception - " + errorMessage);
                    throw new Exception(errorMessage);
                }
                switch (gamePlayStats.GamePlayType)
                {
                    case GameServerEnvironment.VALIDCREDIT:
                    case GameServerEnvironment.VALIDBONUS:
                    case GameServerEnvironment.VALIDCREDITBONUS:
                    case GameServerEnvironment.VALIDCOURTESY:
                    case GameServerEnvironment.VALIDCREDITCOURTESY:
                    case GameServerEnvironment.TIMEPLAY:
                    case GameServerEnvironment.VALIDCARDGAME:
                        proceedWithGamePlay = true;
                        break;
                    case GameServerEnvironment.LOBALCREDIT:
                        statusMessage = "Low Balance, please recharge";
                        break;
                    case GameServerEnvironment.INVCARD:
                        statusMessage = "Invalid card, please check";
                        break;
                    case GameServerEnvironment.GAMETIMEMINGAP:
                        int GameTimeMinGap = gameServerEnvironment.MIN_SECONDS_BETWEEN_GAMETIME_PLAY;
                        statusMessage = "Wait for " + GameTimeMinGap + " seconds";
                        break;
                    case GameServerEnvironment.TECHPLAY:
                        proceedWithGamePlay = true;
                        statusMessage = "Technician Play";
                        break;
                    case GameServerEnvironment.MULTIPLESWIPE:
                        statusMessage = "Wait for 3 seconds";
                        break;
                    case GameServerEnvironment.NOTALLOWED:
                        statusMessage = "Card not allowed";
                        break;
                    case GameServerEnvironment.RESETTIMER:
                        statusMessage = "Sending off command";
                        break;
                    case GameServerEnvironment.NOENTITLEMENT:
                        statusMessage = "Card does not have balance to play this game, please check the card or recharge";
                        break;
                    default:
                        statusMessage = "Invalid response. Please retry";
                        break;
                }

                long gamePlayId = -1;

                if (proceedWithGamePlay == true)
                {
                    string gamePlayCreateMessage = "";
                    //gamePlayId = GamePlay.CreateGamePlay(machineId, cardNumber, gamePlayStats, gameServerEnvironment, ref gamePlayCreateMessage, inSQLTrx);
                    AccountBL accountBL = new AccountBL(ExecutionContext, cardNumber, false, false);
                    GamePlayDTO gamePlaysDTO = new GamePlayDTO();
                    gamePlaysDTO.MachineId = this.MachineId;
                    gamePlaysDTO.CardId = accountBL.AccountDTO.AccountId;
                    GamePlayBuildDTO gamePlayBuildDTO = new GamePlayBuildDTO("R", gamePlaysDTO, gamePlayStats, string.Empty, string.Empty, string.Empty, string.Empty, null, -1, true);
                    gamePlaysDTO = gameTransactionBL.PlayGame(accountBL.AccountDTO.AccountId, gamePlayBuildDTO);
                    gamePlayId = gamePlaysDTO.GameplayId;
                    if (gamePlayId != -1)
                    {
                        statusMessage = "Success";
                        detailedStatus = gamePlayCreateMessage;
                        gamePlayStats.RequestProcessed = true;
                    }
                    else
                        throw new Exception(gamePlayCreateMessage);
                }
                else
                    throw new Exception(statusMessage);

                GamePlayDataHandler gamePlayDataHandler = new GamePlayDataHandler();
                GamePlayDTO gamePlayDTO = gamePlayDataHandler.GetGamePlayDTO(Convert.ToInt32(gamePlayId), inSQLTrx);
                Utilities Utilities = GetUtility(gameServerEnvironment.GameEnvExecutionContext);
                if (gameDetailsList != null)
                {
                    string loginId = "";
                    gameDetailsList = new List<CoreKeyValueStruct>();
                    CardCoreBL cardCore = new CardCoreBL();
                    Card card = new Card(Utilities);
                    gameDetailsList = card.GetCardDetails(cardNumber, loginId);
                    gameDetailsList.Add(new CoreKeyValueStruct("PlayStatus", statusMessage.Replace(",", "")));
                    gameDetailsList.Add(new CoreKeyValueStruct("DetailedStatus", detailedStatus));
                    gameDetailsList.Add(new CoreKeyValueStruct("GamePlayId", gamePlayId.ToString()));
                    gameDetailsList.Add(new CoreKeyValueStruct("GamePrice", gamePlayStats.CreditsRequired.ToString()));

                }

                log.Debug("Ends-PlayGame(MachineDTO machineDTO, CardCoreDTO cardCoreDTO) Method");
                return gamePlayDTO;
            }
            else
            {
                log.Debug("Starts-PlayGame(MachineDTO machineDTO, CardCoreDTO cardCoreDTO) Method by throwing 'Machine name is Invalid' exception");
                throw new Exception("Machine name is Invalid");
            }
        }
        /// <summary>
        /// PlayGame(string machineReference, string cardNumber) method 
        /// this populates the MachineDTO and CardCoreDTO
        /// then internally this will call the PlayGame(MachineDTO machineDTO, CardCoreDTO cardCoreDTO) by passing these created two objects as params
        /// </summary>
        /// <param name="cardNumber">Card number of the user.</param>
        /// <param name="machineReference">external reference of the machine</param>
        /// <returns>GamePlayDTO object</returns>
        public GamePlayDTO PlayGame(string cardNumber, string machineReference)
        {
            log.Debug("Starts-PlayGame(string machineReference, string cardNumber) Method");

            List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> mSearchParams = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
            mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, "Y"));
            mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.EXTERNAL_MACHINE_REFERENCE, machineReference));
            MachineDataHandler machineDataHandler = new MachineDataHandler(null);
            List<MachineDTO> machineList = machineDataHandler.GetMachineList(mSearchParams);
            if (machineList != null && machineList.Any())
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(null);
                foreach (MachineDTO machine in machineList)
                {
                    List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machine.MachineId, _Utilities.ExecutionContext.GetSiteId());
                    machine.SetAttributeList(machineAttributes);
                }
            }
            if (machineList == null)
                throw new Exception("Game machine is not setup in the Parafait system for given " + machineReference);
            else if (machineList.Count > 1)
                throw new Exception("Multiple game machines setup with the same address");

            MachineDTO machineDTO = machineList[0]; //fetching machineDTO from the list based on passed machine reference

            log.Debug("Ends-PlayGame(string machineReference, string cardNumber) Method");
            return PlayGame(machineDTO.MachineId, cardNumber);
        }


        /// <summary>
        /// performs a game play entry and returns the updated card details
        /// </summary>
        /// <param name="macAddress">macAddress</param>
        /// <param name="cardNumber">cardNumber</param>
        /// <param name="loginId">valid loginId</param>
        /// <param name="machineIdStrg">machineId</param>
        /// <returns>Returns List of CoreKeyValueStruct </returns>
        public List<CoreKeyValueStruct> PlayGame(string macAddress, string cardNumber, string loginId, string machineIdStrg)
        {
            List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> mSearchParams = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();

            int machineId;
            if (int.TryParse(machineIdStrg, out machineId) == false)
            {
                if (!string.IsNullOrEmpty(macAddress))
                    throw new Exception("Game machine reference is not Valid ");

                mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACADDRESS, macAddress));
            }
            else
                mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_ID, machineId.ToString()));

            mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, "Y"));

            MachineDataHandler machineDataHandler = new MachineDataHandler(null);
            List<MachineDTO> machineList = machineDataHandler.GetMachineList(mSearchParams);
            if (machineList != null && machineList.Any())
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(null);
                foreach (MachineDTO machine in machineList)
                {
                    List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machine.MachineId, _Utilities.ExecutionContext.GetSiteId());
                    machine.SetAttributeList(machineAttributes);
                }
            }
            if (machineList == null)
                throw new Exception("Game machine is not setup in the Parafait system for given " + macAddress);
            else if (machineList.Count > 1)
                throw new Exception("Multiple game machines setup with the same address");

            MachineDTO machineDTO = machineList[0]; //fetching machineDTO from the list based on passed machine reference

            gameDetailsList = new List<CoreKeyValueStruct>();
            Utilities Utilities = GetUtility(this.GameServerEnvironment.GameEnvExecutionContext);
            GamePlayDTO gamePlayDTO = PlayGame(machineDTO.MachineId, cardNumber);
            if (gameDetailsList != null)
            {
                gameDetailsList = new List<CoreKeyValueStruct>();
                CardCoreBL cardCore = new CardCoreBL();
                Card card = new Card(Utilities);
                gameDetailsList = card.GetCardDetails(cardNumber, loginId);
                gameDetailsList.Add(new CoreKeyValueStruct("PlayStatus", "Success"));
                gameDetailsList.Add(new CoreKeyValueStruct("DetailedStatus", ""));
                gameDetailsList.Add(new CoreKeyValueStruct("GamePlayId", gamePlayDTO.GameplayId.ToString()));
                gameDetailsList.Add(new CoreKeyValueStruct("GamePrice", (gamePlayDTO.Bonus + gamePlayDTO.Credits + gamePlayDTO.CPCardBalance + gamePlayDTO.CPCredits + gamePlayDTO.CPBonus).ToString()));

            }
            return gameDetailsList;
        }


        /// <summary>
        /// performs a game play entry and returns the updated card details
        /// </summary>
        /// <param name="macAddress">macAddress</param>
        /// <param name="cardNumber">cardNumber</param>
        /// <param name="loginId">valid loginId</param>
        /// <param name="machineIdStrg">machineId</param>
        /// <returns>Returns key value pair</returns>
        public List<CoreKeyValueStruct> PlayGames(string macAddress, string cardNumber, string loginId, string machineIdStrg)
        {
            log.Debug("Starts-PlayGame(string macAddress, string cardNumber, string loginId, string machineIdStrg) Method");
            List<CoreKeyValueStruct> gameDetails = new List<CoreKeyValueStruct>();
            DateTime currentTime = DateTime.Now;

            try
            {
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                string connstring = dataAccessHandler.ConnectionString;
                using (Utilities parafaitUtility = new Utilities(connstring))
                {
                    parafaitUtility.ParafaitEnv.Initialize();
                    ServerStatic serverstatic = new ServerStatic(parafaitUtility);

                    serverstatic.Initialize();

                    ServerStatic.GlobalGamePlayStats gamePlayStats = new ServerStatic.GlobalGamePlayStats();

                    //gamePlayStats.Initialize();
                    string gamePlayMessage = "";
                    string statusMessage = "";
                    string detailedStatus = "";
                    int machineId;
                    if (int.TryParse(machineIdStrg, out machineId) == false)
                    {
                        MachineDTO machineDTO = getGameMachine(macAddress);
                        machineId = machineDTO.MachineId;
                    }

                    bool proceedWithGamePlay = false;
                    if (machineId != -1)
                    {

                        Machine localMachine = new Machine(Convert.ToInt32(machineId), serverstatic);
                        // localMachine.MachineId = machineId; commented and used constructor as above. Change to handle reference machine id
                        GamePlay.getGamePlayDetails(localMachine, cardNumber, gamePlayStats, serverstatic, ref gamePlayMessage);

                        switch (gamePlayStats.GamePlayType)
                        {
                            case ServerStatic.VALIDCREDIT:
                            case ServerStatic.VALIDBONUS:
                            case ServerStatic.VALIDCREDITBONUS:
                            case ServerStatic.VALIDCOURTESY:
                            case ServerStatic.VALIDCREDITCOURTESY:
                            case ServerStatic.TIMEPLAY:
                            case ServerStatic.VALIDCARDGAME:
                                proceedWithGamePlay = true;
                                break;
                            case ServerStatic.LOBALCREDIT:
                                statusMessage = "Low Balance, please recharge";
                                break;
                            case ServerStatic.INVCARD:
                                statusMessage = "Invalid card, please check";
                                break;
                            case ServerStatic.GAMETIMEMINGAP:
                                int GameTimeMinGap = serverstatic.MIN_SECONDS_BETWEEN_GAMETIME_PLAY;
                                statusMessage = "Wait for " + GameTimeMinGap + " seconds";
                                break;
                            case ServerStatic.TECHPLAY:
                                proceedWithGamePlay = true;
                                statusMessage = "Technician Play";
                                break;
                            case ServerStatic.MULTIPLESWIPE:
                                statusMessage = "Wait for 3 seconds";
                                break;
                            case ServerStatic.NOTALLOWED:
                                statusMessage = "Card not allowed";
                                break;
                            case ServerStatic.RESETTIMER:
                                statusMessage = "Sending off command";
                                break;
                            case ServerStatic.NOENTITLEMENT:
                                statusMessage = "Card does not have balance to play this game, please check the card or recharge";
                                break;
                            default:
                                statusMessage = "Invalid response. Please retry";
                                break;
                        }

                        long gamePlayId = -1;

                        if (proceedWithGamePlay == true)
                        {
                            string gamePlayCreateMessage = "";
                            gamePlayId = GamePlay.CreateGamePlay(machineId, cardNumber, gamePlayStats, serverstatic, ref gamePlayCreateMessage);
                            if (gamePlayId != -1)
                            {
                                statusMessage = "Success";
                                detailedStatus = gamePlayCreateMessage;
                                gamePlayStats.RequestProcessed = true;
                            }
                            else
                                throw new Exception(gamePlayCreateMessage);
                        }
                        else
                            throw new Exception(statusMessage);

                        CardCoreBL cardCore = new CardCoreBL();
                        Card card = new Card(parafaitUtility);
                        gameDetails = card.GetCardDetails(cardNumber, loginId);
                        gameDetails.Add(new CoreKeyValueStruct("PlayStatus", statusMessage.Replace(",", "")));
                        gameDetails.Add(new CoreKeyValueStruct("DetailedStatus", detailedStatus));
                        gameDetails.Add(new CoreKeyValueStruct("GamePlayId", gamePlayId.ToString()));
                        gameDetails.Add(new CoreKeyValueStruct("GamePlayCreditsRequired", gamePlayStats.CreditsRequired.ToString()));
                    }
                    else
                    {
                        throw new Exception("Machine name is Invalid");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Ends-PlayGame(string macAddress, string cardNumber, string loginId, string machineIdStrg) by throwing Exception");
                throw new Exception(ex.Message);
            }

            DateTime endTime = DateTime.Now;
            TimeSpan timeDiff = endTime - currentTime;
            gameDetails.Add(new CoreKeyValueStruct("DebugTime", timeDiff.Milliseconds.ToString()));
            log.Debug("Ends-PlayGame(string macAddress, string cardNumber, string loginId, string machineIdStrg) by returning listOfValues");
            return gameDetails;
        }

        private MachineDTO getGameMachine(String macAddress)
        {
            List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> mSearchParams = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
            mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACADDRESS, macAddress));
            MachineDataHandler machineDataHandler = new MachineDataHandler(null);
            List<MachineDTO> machineList = machineDataHandler.GetMachineList(mSearchParams);
            if (machineList != null && machineList.Any())
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(null);
                foreach (MachineDTO machine in machineList)
                {
                    List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machine.MachineId, ExecutionContext.GetSiteId());
                    machine.SetAttributeList(machineAttributes);
                }
            }
            if (machineList == null)
                throw new Exception("Game machine is not setup in the Parafait system for ddress " + macAddress);
            else if (machineList.Count > 1)
                throw new Exception("Multiple game machines setup with the same address");

            MachineDTO machineDTO = machineList[0];
            return machineDTO;
        }

        private Utilities GetUtility(Semnox.Core.Utilities.ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            utilities.ExecutionContext.SetMachineId(executionContext.MachineId);
            utilities.ExecutionContext.SetPosMachineGuid(executionContext.PosMachineGuid);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit();
            return utilities;
        }
    }
}

