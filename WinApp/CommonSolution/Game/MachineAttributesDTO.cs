/********************************************************************************************
 * Project Name - MachineAttribute Data dto                                                                          
 * Description  - Dto of the MachineAttribute class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      12-dec-2018   Guru S A      Who column changes
 *2.60        05-Apr-2019   Jagan         Added the Guid paramenter to the constructor
 *2.70.2        31-Jul-2019   Deeksha       Modified to make all te datafields as private.
 *2.80.0      08-Apr-2020   Mathew Ninan  Added additional Game Profile attributes 
 *2.130.8   14-Apr-2022      Abhishek      Modified : Added enableForPromotion field as a part of Promotion Game Attribute Enhancement
 *2.155.0     14-Jul-2023    Mathew Ninan  Added new attributes for Free play 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// MachineAttributeDTO
    /// </summary>
    public class MachineAttributeDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByGameProfileParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// GAME_PROFILE ID search field
            /// </summary>
            GAME_PROFILE_ID,
            /// <summary>
            /// GAME ID search field
            /// </summary>
            GAME_ID ,
            /// <summary>
            /// MACHINE ID search field
            /// </summary>
            MACHINE_ID ,
            /// <summary>
            /// SITE ID search field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// MASTER ENTITY ID search field
            /// </summary>
            MASTER_ENTITY_ID ,
            /// <summary>
            /// ATTRIBUTE ID search field
            /// </summary>
            ATTRIBUTE_ID ,
            /// <summary>
            /// IS ACTIVE ID search field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// PROMOTION_ID search field
            /// </summary>
            PROMOTION_ID,
            /// <summary>
            /// PROMOTION_DETAIL_ID search field
            /// </summary>
            PROMOTION_DETAIL_ID

        }

        /// <summary>
        /// AttributeContext
        /// </summary>
        public enum AttributeContext
        {
            /// <summary>
            /// Search by SYSTEM
            /// </summary>
            SYSTEM,
            /// <summary>
            /// Search by GAME PROFILE
            /// </summary>
            GAME_PROFILE,
            /// <summary>
            /// Search by GAME
            /// </summary>
            GAME,
            /// <summary>
            /// Search by MACHINE
            /// </summary>
            MACHINE,
            /// <summary>
            /// Search by PROMOTION
            /// </summary>
            PROMOTION,
            /// <summary>
            /// Search by PROMOTION_DETAIL
            /// </summary>
            PROMOTION_DETAIL
        }
        /// <summary>
        /// MachineAttribute
        /// </summary>
        public enum MachineAttribute
        {          
            /// <summary>
            /// Search by START IN PHYSICAL TICKET MODE
            /// </summary>
            START_IN_PHYSICAL_TICKET_MODE,
            /// <summary>
            /// Search by SHOW AD
            /// </summary>
            SHOW_AD,
            /// <summary>
            /// Search by NUMBER OF COINS
            /// </summary>
            NUMBER_OF_COINS,
            /// <summary>
            /// Search by COIN PULSE WIDTH
            /// </summary>
            COIN_PULSE_WIDTH,
            /// <summary>
            /// Search by TICKET PULSE WIDTH
            /// </summary>
            TICKET_PULSE_WIDTH,
            /// <summary>
            /// Search by COIN PULSE GAP
            /// </summary>
            COIN_PULSE_GAP,
            /// <summary>
            /// Search by TICKET PULSE GAP
            /// </summary>
            TICKET_PULSE_GAP,
            /// <summary>
            /// Search by SENSOR INTERVAL
            /// </summary>
            SENSOR_INTERVAL,
            /// <summary>
            /// Search by POWER ON TICKET DELAY
            /// </summary>
            POWER_ON_TICKET_DELAY,
            /// <summary>
            /// Search by DISABLE TICKETS
            /// </summary>
            DISABLE_TICKETS,
            /// <summary>
            /// Search by REVERSE DISPLAY DIRECTION
            /// </summary>
            REVERSE_DISPLAY_DIRECTION,
            /// <summary>
            /// Search by SHOW STATIC AD
            /// </summary>
            SHOW_STATIC_AD,
            /// <summary>
            /// Search by DEFAULT THEME
            /// </summary>
            DEFAULT_THEME,
            /// <summary>
            /// Search by TICKET EATER
            /// </summary>
            TICKET_EATER,
            /// <summary>
            /// Search by TICKET EATER CARD WAIT INTERVAL
            /// </summary>
            TICKET_EATER_CARD_WAIT_INTERVAL,
            /// <summary>
            /// Serach by TICKET EATER TICKET WAIT INTERVAL,
            /// </summary>
            TICKET_EATER_TICKET_WAIT_INTERVAL,
            /// <summary>
            /// Search by START SCREEN NUMBER
            /// </summary>
            START_SCREEN_NUMBER,
            /// <summary>
            /// Search by COIN PUSHER MACHINE
            /// </summary>
            COIN_PUSHER_MACHINE,
            /// <summary>
            /// Search by BALANCE DELAY
            /// </summary>
            BALANCE_DELAY,
            /// <summary>
            /// Search by DEBUG MODE
            /// </summary>
            DEBUG_MODE,
            /// <summary>
            /// Search by ENABLE RESET PULSE
            /// </summary>
            ENABLE_RESET_PULSE,
            /// <summary>
            /// Search by CARD RETRIES
            /// </summary>
            CARD_RETRIES,
            /// <summary>
            /// Search by MIN SECONDS BETWEEN REPEAT PLAY
            /// </summary>
            MIN_SECONDS_BETWEEN_REPEAT_PLAY,
            /// <summary>
            /// Search by DISPLAY LANGUAGE
            /// </summary>
            DISPLAY_LANGUAGE,
            /// <summary>
            /// Search by QUEUE SETUP REQUIRED
            /// </summary>
            QUEUE_SETUP_REQUIRED,
            /// <summary>
            /// Search by MAX TICKETS PER GAMEPLAY
            /// </summary>
            MAX_TICKETS_PER_GAMEPLAY,
            /// <summary>
            /// Search by ENABLE EXT ANTENNA
            /// </summary>
            ENABLE_EXT_ANTENNA,
            /// <summary>
            /// Search by OUT OF SERVICE
            /// </summary>
            OUT_OF_SERVICE,
            /// <summary>
            /// Search by OUT OF SERVICE THEME
            /// </summary>
            OUT_OF_SERVICE_THEME,
            /// <summary>
            /// Search by GAMEPLAY DURATION
            /// </summary>
            GAMEPLAY_DURATION,
            /// <summary>
            /// Search by CARD RETIRES
            /// </summary>
            CARD_RETIRES,
            /// <summary>
            /// Search by FREE PLAY THEME
            /// </summary>
            FREE_PLAY_THEME,
            /// <summary>
            /// Search by COIN INTERRUPT_DELAY
            /// </summary>
            COIN_INTERRUPT_DELAY,
            /// <summary>
            /// Search by INITIAL LED PATTERN
            /// </summary>
            INITIAL_LED_PATTERN,
            /// <summary>
            /// Search by READER VOLUME
            /// </summary>
            READER_VOLUME,
            /// <summary>
            /// Search by TICKET DELAY
            /// </summary>
            TICKET_DELAY,
            /// <summary>
            /// Search by ENABLE INVALID LIGHT
            /// </summary>
            ENABLE_INVALID_LIGHT,
            /// <summary>
            /// Search by SHOW BIG BALANCE
            /// </summary>
            SHOW_BIG_BALANCE,
            /// <summary>
            /// Search by AD DELAY
            /// </summary>
            AD_DELAY,
            /// <summary>
            /// Search by AD INTERVAL
            /// </summary>
            AD_INTERVAL,
            /// <summary>
            /// Search by AD IMPRESSION
            /// </summary>
            AD_IMPRESSION,
            /// <summary>
            /// Search by GAMEPLAY MULTIPLIER
            /// </summary>
            GAMEPLAY_MULTIPLIER,
            /// <summary>
            /// Search by DYNAMIC QUERY EVENT FREQUENCY
            /// </summary>
            DYNAMIC_QUERY_EVENT_FREQUENCY,
            /// <summary>
            /// Search by DATA REFRESH FREQUENCY
            /// </summary>
            DATA_REFRESH_FREQUENCY,
            /// <summary>
            /// Search PRICE REFRESH FREQUENCY
            /// </summary>
            PRICE_REFRESH_FREQUENCY,
            /// <summary>
            /// Search by RESOLUTION X
            /// </summary>
            RESOLUTION_X,
            /// <summary>
            /// Search by RESOLUTION_Y
            /// </summary>
            RESOLUTION_Y,
            /// <summary>
            /// Search by AUDIO THEME NUMBER
            /// </summary>
            AUDIO_THEME_NUMBER,
            /// <summary>
            /// Search by TICKET MULTIPLIER
            /// </summary>
            TICKET_MULTIPLIER,
            /// <summary>
            /// Search by ATTENDANCE READER
            /// Updated by Manoj - 24/Sep/2018
            /// </summary>
            ATTENDANCE_READER,
            /// <summary>
            /// Wait period before customer chooses an option
            /// </summary>
            MULTI_GAME_PLAY_WAIT_PERIOD,
            /// <summary>
            /// Default no of game plays when the user didn't choose an option
            /// </summary>
            DEFAULT_MULTI_GAME_PLAY_VALUE,
            /// <summary>
            /// Whether Ticket Enable Input signal should be checked when reader reboots
            /// </summary>
            CHECK_TICKET_INPUT_SIGNAL_REBOOT,
            /// <summary>
            /// Whether Ticket Notch Output signal should be checked
            /// </summary>
            CHECK_TICKET_NOTCH_OUTPUT_SIGNAL,
            /// <summary>
            /// Indicates if it's CEC customer - As there are some specific setups related to this customer. 
            /// </summary>
            IS_CEC,
            /// <summary>
            /// Defines how often the machine name is shown on the reader
            /// </summary>
            MACHINE_NAME_DISPLAY_FREQUENCY,
            /// <summary>
            /// 0 = Initial Sound Please Tap Card will  play, 1 = Initial Sound Please Tap Card will  not play
            /// </summary>
            ENABLE_INIT_SOUND_TAP_CARD,
            /// <summary>
            /// To decide priority of the card when tickets are being counted
            /// </summary>
            CARD_PRIORITY,
            /// <summary>
            /// To decide whether inhibit line to should be considered or not
            /// </summary>
            ENABLE_INHIBIT_LINE,
            /// <summary>
            /// Time Pause reader. 0 - Normal Reader, 1 - Pause Time reader
            /// </summary>
            PAUSE_TIME_READER,
            /// <summary>
            /// To decide if e-ticket needs to be forced even during coin play 
            /// </summary>
            ENFORCE_ETICKET_COIN_PLAY,
            /// <summary>
            /// Enable auth key verification if ultralight card
            /// </summary>
            VERIFY_AUTH_FOR_ULC,
            /// <summary>
            /// 0-Normal Reader, 1- Refund reader
            /// </summary>
            REFUND_READER,
            /// <summary>
            /// Direction of coin pulse - High to low or low to high
            /// </summary>
            COIN_PULSE_DIRECTION,
            /// <summary>
            /// Is it LAN board
            /// </summary>
            LAN_BOARD,
            /// <summary>
            /// Is it Slot machine
            /// </summary>
            SLOT_READER,
            /// <summary>
            /// Check balance reader
            /// </summary>
            CHECK_BALANCE_READER,
            /// <summary>
            /// User provided with option to choose no of game plays
            /// </summary>
            MULTI_GAME_PLAY_READER,
            /// <summary>
            /// Whether to remember the tapped card after multi game play
            /// </summary>
            REPEAT_MULTI_GAME_PLAY,
            /// <summary>
            /// Whether machine should show QR Code for play
            /// </summary>
            IS_QRPLAY_ENABLED,
            /// <summary>
            /// Whether machine should authenticate mifare card
            /// </summary>
            VERIFY_AUTH_FOR_MIFARE,
            /// <summary>
            /// Message translation to be sent from Server
            /// </summary>
            ENABLE_IN_MEMORY_MESSAGE,
            /// <summary>
            /// Enable reader to move to Automatic Free play
            /// </summary>
            AUTO_FREE_PLAY,
            /// <summary>
            /// Number of restarts required to initiate auto free play mode
            /// </summary>
            RESTART_COUNT_AUTO_FREE_PLAY,
            /// <summary>
            /// VIP Price to be hidden for specific readers
            /// </summary>
            HIDE_VIP_PRICE,
            /// <summary>
            /// Free Play LED Pattern
            /// </summary>
            FREE_PLAY_LED_PATTERN
        }

        private MachineAttribute attribute;
        private string attributeValue;
        private string attributeNameText;
        private int attributeId;
        private string isFlag;
        private string isSoftwareAttribute;
        private AttributeContext contextOfAttrib;
        private bool enableForPromotion;
        private string guid;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int masterEntityId;
        private int siteId;
        private bool synchStatus;
        private List<MachineAttributeLogDTO> machineAttributeLogDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineAttributeDTO()
        {
            log.LogMethodEntry();
            attributeId = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = true;
            enableForPromotion = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>      
        public MachineAttributeDTO(MachineAttribute machineAttribute, string attributeValue, AttributeContext attributeContext)
            : this()
        {
            log.LogMethodEntry(machineAttribute, attributeValue, attributeContext);
            attribute = machineAttribute;
            attributeNameText = machineAttribute.ToString().Replace("_", " ");
            this.attributeValue = attributeValue;
            isFlag = "Y";
            isSoftwareAttribute = "N";
            this.isActive = true;
            this.contextOfAttrib = attributeContext;
            this.attributeId = -1;
            this.masterEntityId = -1;
            this.siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>  

        public MachineAttributeDTO(int attributeId, MachineAttribute machineAttribute, string attributeValue, string isFlag, string isSoftwareAttribute,
                                AttributeContext attributeContext, string guid, bool enableForPromotion)
            : this(machineAttribute, attributeValue, attributeContext)
        {
            log.LogMethodEntry(attributeId, machineAttribute, attributeValue, isFlag, isSoftwareAttribute, attributeContext,guid, enableForPromotion);
            this.attributeId = attributeId;
            this.isFlag = isFlag;
            this.isSoftwareAttribute = isSoftwareAttribute;
            this.isActive = true;
            this.guid = guid;
            this.enableForPromotion = enableForPromotion;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>  

        public MachineAttributeDTO(int attributeId, MachineAttribute machineAttribute, string attributeValue, string isFlag, string isSoftwareAttribute,
                                AttributeContext attributeContext, string guid)
            : this(machineAttribute, attributeValue, attributeContext)
        {
            log.LogMethodEntry(attributeId, machineAttribute, attributeValue, isFlag, isSoftwareAttribute, attributeContext, guid);
            this.attributeId = attributeId;
            this.isFlag = isFlag;
            this.isSoftwareAttribute = isSoftwareAttribute;
            this.isActive = true;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the parameter
        /// </summary>
        public MachineAttributeDTO(int attributeId, MachineAttribute machineAttribute, string attributeValue, string isFlag, string isSoftwareAttribute,
                                AttributeContext attributeContext, string guid ,bool synchStatus, int siteId,
                            string lastUpdatedBy, DateTime lastUpdatedDate, int masterEntityId, string createdBy, DateTime creationDate)
            : this(attributeId, machineAttribute, attributeValue, isFlag, isSoftwareAttribute, attributeContext, guid)
        {
            log.LogMethodEntry(attributeId, machineAttribute, attributeValue, isFlag, isSoftwareAttribute, attributeContext, guid);
            this.masterEntityId = -1;
            this.siteId = -1;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.masterEntityId = masterEntityId;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdatedDate;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Due to rendering issue in Angular, we have to pass MachineAttributeName as string.
        ///  Earlier tried with ENUM, however it was going as ENUM Value. Hence created an attribute with string
        /// </summary>
        public string AttributeNameText { get { return attributeNameText; } set { attributeNameText = value; } }
        
        /// <summary>
        /// Get/Set for AttributeValue
        /// </summary>
        public string AttributeValue
        {
            get { return attributeValue; }
            set
            {
                attributeValue = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set for AttributeName
        /// </summary>
        public MachineAttribute AttributeName { get { return attribute; } set { attribute = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for AttributeId
        /// </summary>
        public int AttributeId { get { return attributeId; } set { attributeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for IsFlag
        /// </summary>
        public string IsFlag { get { return isFlag; } set { isFlag = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for IsSoftwareAttribute
        /// </summary>
        public string IsSoftwareAttribute { get { return isSoftwareAttribute; } set { isSoftwareAttribute = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for ContextAttribute
        /// </summary>
        public AttributeContext ContextOfAttribute { get { return contextOfAttrib; } set { contextOfAttrib = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for IsActive
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for CreationDate
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set for CreatedBy
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }
        /// <summary>
        /// Get/Set for LastUpdateDate
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; }set { lastUpdateDate = value; } }
        /// <summary>
        /// Get/Set for CreatedBy
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for masterEntityId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for SiteId
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set for SynchStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        public List<MachineAttributeLogDTO> MachineAttributeLogDTOList { get { return machineAttributeLogDTOList; } set { machineAttributeLogDTOList = value; } }
        
        /// <summary>
        /// Get/Set for enableForPromotion
        /// </summary>
        public bool EnableForPromotion { get { return enableForPromotion; } set { enableForPromotion = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || attributeId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

    }
}
