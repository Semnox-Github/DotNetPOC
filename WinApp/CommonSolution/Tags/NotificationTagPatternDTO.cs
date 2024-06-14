/********************************************************************************************
 * Project Name - Tags
 * Description  - Data Object of the NotificationTagPattern class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        20-jul-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Tags
{
    public class NotificationTagPatternDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by NOTIFICATIONTAGPATTERNID
            /// </summary>
            NOTIFICATIONTAGPATTERNID,
            /// <summary>
            /// Search by NOTIFICATIONTAGPATTERNNAME
            /// </summary>
            NOTIFICATIONTAGPATTERNNAME,
            /// <summary>
            ///  Search by LEDPATTERNNUMBER
            /// </summary>
            LEDPATTERNNUMBER,
            /// <summary>
            /// Search by BUZZERPATTERNNUMBER
            /// </summary>
            BUZZERPATTERNNUMBER,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
           
        }

        private int notificationTagPatternId;
        private string notificationTagPatternName;
        private int patternDurationInSeconds;
        private string ledPatternNumber;
        private string vibrationPatternNumber;
        private string buzzerPatternNumber;
        private string customColor;
        private string customAnimation;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NotificationTagPatternDTO()
        {
            log.LogMethodEntry();
            notificationTagPatternId = -1;
            buzzerPatternNumber = string.Empty;
            ledPatternNumber = string.Empty;
            vibrationPatternNumber = string.Empty;
            customColor = string.Empty;
            customAnimation = string.Empty;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationTagPatternId"></param>
        /// <param name="notificationTagPatternName"></param>
        /// <param name="patternDurationInSeconds"></param>
        /// <param name="ledPatternNumber"></param>
        /// <param name="buzzerPatternNumber"></param>
        /// <param name="isActive"></param>
        public NotificationTagPatternDTO(int notificationTagPatternId, string notificationTagPatternName, int patternDurationInSeconds, string ledPatternNumber, string vibrationPatternNumber, string buzzerPatternNumber,
                                         string customColor, string customAnimation, bool isActive)
        {
            log.LogMethodEntry(notificationTagPatternId, notificationTagPatternName, patternDurationInSeconds, ledPatternNumber, vibrationPatternNumber,buzzerPatternNumber, customColor, customAnimation, isActive);
            this.notificationTagPatternId = notificationTagPatternId;
            this.notificationTagPatternName = notificationTagPatternName;
            this.patternDurationInSeconds = patternDurationInSeconds;
            this.ledPatternNumber = ledPatternNumber;
            this.vibrationPatternNumber = vibrationPatternNumber;
            this.buzzerPatternNumber = buzzerPatternNumber;
            this.customColor = customColor;
            this.customAnimation = customAnimation;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        public NotificationTagPatternDTO(int notificationTagPatternId, string notificationTagPatternName, int patternDurationInSeconds, string ledPatternNumber, string vibrationPatternNumber, string buzzerPatternNumber,
                                         string customColor, string customAnimation, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                           int siteId, int masterEntityId, bool synchStatus, string guid)
            : this(notificationTagPatternId, notificationTagPatternName, patternDurationInSeconds, ledPatternNumber, vibrationPatternNumber, buzzerPatternNumber, customColor, customAnimation, isActive)
        {
            log.LogMethodEntry(notificationTagPatternId, notificationTagPatternName, patternDurationInSeconds, ledPatternNumber, vibrationPatternNumber, buzzerPatternNumber, customColor, customAnimation, isActive, createdBy,
                                creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);

            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
        }

        /// <summary>
        /// Get/Set method of the NotificationTagPatternId field
        /// </summary>
        public int NotificationTagPatternId { get { return notificationTagPatternId; } set { this.IsChanged = true; notificationTagPatternId = value; } }

        /// <summary>
        /// Get/Set method of the PatternDurationInSeconds field
        /// </summary>
        public int PatternDurationInSeconds { get { return patternDurationInSeconds; } set { this.IsChanged = true; patternDurationInSeconds = value; } }

        /// <summary>
        /// Get/Set method of the LEDPatternNumber field
        /// </summary>
        public string LEDPatternNumber { get { return ledPatternNumber; } set { this.IsChanged = true; ledPatternNumber = value; } }

        /// <summary>
        /// Get/Set method of the VibrationPatternNumber field
        /// </summary>
        public string VibrationPatternNumber { get { return vibrationPatternNumber; } set { this.IsChanged = true; vibrationPatternNumber = value; } }

        /// <summary>
        /// Get/Set method of the BuzzerPatternNumber field
        /// </summary>
        public string BuzzerPatternNumber { get { return buzzerPatternNumber; } set { this.IsChanged = true; buzzerPatternNumber = value; } }

        /// <summary>
        /// Get/Set method of the NotificationTagPatternName field
        /// </summary>
        public string NotificationTagPatternName { get { return notificationTagPatternName; } set { this.IsChanged = true; notificationTagPatternName = value; } }

        /// <summary>
        /// Get/Set method of the CustomColor field
        /// </summary>
        public string CustomColor { get { return customColor; } set { this.IsChanged = true; customColor = value; } }

        /// <summary>
        /// Get/Set method of the CustomAnimation field
        /// </summary>
        public string CustomAnimation { get { return customAnimation; } set { this.IsChanged = true; customAnimation = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || notificationTagPatternId < 0;
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }

    public enum TagNotificationLEDPattern
    {
        OFF = 0,
        SolidGreen = 1,
        SolidRed = 2,
        SolidBlue = 3,
        BlinkGreen = 4,
        BlinkRed = 5,
        BlinkBlue = 6,
        ChaseGreen = 7,
        ChaseRed = 8,
        ChaseBlue = 9,
        Custom = 0x80
    }

    public enum TagCustomLEDPatternColors
    {
        Red = 0x10,
        Green = 0x20,
        Blue = 0x40
    }

    public enum TagCustomLEDPatternAnimations
    {
        SOLID,
        BLINK,
        ROTATE_ANTICLOCKWISE,
        ROTATE_CLOCKWISE
    }

    public class TagNotificationLEDPatternConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Converts TagNotificationLEDPattern from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TagNotificationLEDPattern FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case "OFF":
                    {
                        return TagNotificationLEDPattern.OFF;
                    }
                case "SolidGreen":
                    {
                        return TagNotificationLEDPattern.SolidGreen;
                    }
                case "SolidRed":
                    {
                        return TagNotificationLEDPattern.SolidRed;
                    }
                case "SolidBlue":
                    {
                        return TagNotificationLEDPattern.SolidBlue;
                    }
                case "BlinkGreen":
                    {
                        return TagNotificationLEDPattern.BlinkGreen;
                    }
                case "BlinkRed":
                    {
                        return TagNotificationLEDPattern.BlinkRed;
                    }
                case "BlinkBlue":
                    {
                        return TagNotificationLEDPattern.BlinkBlue;
                    }
                case "ChaseGreen":
                    {
                        return TagNotificationLEDPattern.ChaseGreen;
                    }
                case "ChaseRed":
                    {
                        return TagNotificationLEDPattern.ChaseRed;
                    }
                case "ChaseBlue":
                    {
                        return TagNotificationLEDPattern.ChaseBlue;
                    }
                default:
                    {
                        log.Error("Error :Not a valid Tag Notification LED Pattern type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Tag Notification LED Pattern type");
                    }
            }
        }
        /// <summary>
        /// Converts Tag Notification Led Pattern to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(TagNotificationLEDPattern value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case TagNotificationLEDPattern.OFF:
                    {
                        return "OFF";
                    }
                case TagNotificationLEDPattern.SolidGreen:
                    {
                        return "SolidGreen";
                    }
                case TagNotificationLEDPattern.SolidRed:
                    {
                        return "SolidRed";
                    }
                case TagNotificationLEDPattern.SolidBlue:
                    {
                        return "SolidBlue";
                    }
                case TagNotificationLEDPattern.BlinkGreen:
                    {
                        return "BlinkGreen";
                    }
                case TagNotificationLEDPattern.BlinkRed:
                    {
                        return "BlinkRed";
                    }
                case TagNotificationLEDPattern.BlinkBlue:
                    {
                        return "BlinkBlue";
                    }
                case TagNotificationLEDPattern.ChaseGreen:
                    {
                        return "ChaseGreen";
                    }
                case TagNotificationLEDPattern.ChaseRed:
                    {
                        return "ChaseRed";
                    }
                case TagNotificationLEDPattern.ChaseBlue:
                    {
                        return "ChaseBlue";
                    }
                default:
                    {
                        log.Error("Error :Not a valid Tag Notification Led Pattern type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Tag Notification Led Pattern");
                    }
            }
        }
    }

    public enum TagNotificationVibratePattern
    {
        OFF = 0,
        Freq2Hz50Percent = 1,
        Freq4Hz50Percent = 2,
        Freq4Hz25Percent = 3,
        Freq6Hz25Percent = 4
    }

    public class TagNotificationVibratePatternConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Converts TagNotificationVibratePattern from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TagNotificationVibratePattern FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case "OFF":
                    {
                        return TagNotificationVibratePattern.OFF;
                    }
                case "Freq2Hz50Percent":
                    {
                        return TagNotificationVibratePattern.Freq2Hz50Percent;
                    }
                case "Freq4Hz50Percent":
                    {
                        return TagNotificationVibratePattern.Freq4Hz50Percent;
                    }
                case "Freq4Hz25Percent":
                    {
                        return TagNotificationVibratePattern.Freq4Hz25Percent;
                    }
                case "Freq6Hz25Percent":
                    {
                        return TagNotificationVibratePattern.Freq6Hz25Percent;
                    }
                default:
                    {
                        log.Error("Error :Not a valid Tag Notification Vibration Pattern type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Tag Notification Vibration Pattern type");
                    }
            }
        }
        /// <summary>
        /// Converts Tag Notification Vibrate Pattern to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(TagNotificationVibratePattern value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case TagNotificationVibratePattern.OFF:
                    {
                        return "OFF";
                    }
                case TagNotificationVibratePattern.Freq2Hz50Percent:
                    {
                        return "Freq2Hz50Percent";
                    }
                case TagNotificationVibratePattern.Freq4Hz50Percent:
                    {
                        return "Freq4Hz50Percent";
                    }
                case TagNotificationVibratePattern.Freq4Hz25Percent:
                    {
                        return "Freq4Hz25Percent";
                    }
                case TagNotificationVibratePattern.Freq6Hz25Percent:
                    {
                        return "Freq6Hz25Percent";
                    }
                default:
                    {
                        log.Error("Error :Not a valid Tag Notification Vibrate Pattern type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Tag Notification Vibrate Pattern");
                    }
            }
        }
    }

    public enum TagNotificationBuzzerPattern
    {
        OFF = 0,
        Freq2Hz50Percent = 1,
        Freq4Hz50Percent = 2,
        Freq4Hz25Percent = 3,
        Freq6Hz25Percent = 4
    }

    public class TagNotificationBuzzerPatternConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Converts TagNotificationBuzzerPattern from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TagNotificationBuzzerPattern FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case "OFF":
                    {
                        return TagNotificationBuzzerPattern.OFF;
                    }
                case "Freq2Hz50Percent":
                    {
                        return TagNotificationBuzzerPattern.Freq2Hz50Percent;
                    }
                case "Freq4Hz50Percent":
                    {
                        return TagNotificationBuzzerPattern.Freq4Hz50Percent;
                    }
                case "Freq4Hz25Percent":
                    {
                        return TagNotificationBuzzerPattern.Freq4Hz25Percent;
                    }
                case "Freq6Hz25Percent":
                    {
                        return TagNotificationBuzzerPattern.Freq6Hz25Percent;
                    }
                default:
                    {
                        log.Error("Error :Not a valid Tag Notification Vibration Pattern type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Tag Notification Vibration Pattern type");
                    }
            }
        }
        /// <summary>
        /// Converts Tag Notification Buzzer Pattern to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(TagNotificationBuzzerPattern value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case TagNotificationBuzzerPattern.OFF:
                    {
                        return "OFF";
                    }
                case TagNotificationBuzzerPattern.Freq2Hz50Percent:
                    {
                        return "Freq2Hz50Percent";
                    }
                case TagNotificationBuzzerPattern.Freq4Hz50Percent:
                    {
                        return "Freq4Hz50Percent";
                    }
                case TagNotificationBuzzerPattern.Freq4Hz25Percent:
                    {
                        return "Freq4Hz25Percent";
                    }
                case TagNotificationBuzzerPattern.Freq6Hz25Percent:
                    {
                        return "Freq6Hz25Percent";
                    }
                default:
                    {
                        log.Error("Error :Not a valid Tag Notification Vibrate Pattern type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Tag Notification Vibrate Pattern");
                    }
            }
        }
    }
}
