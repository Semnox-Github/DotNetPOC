/********************************************************************************************
 * Project Name - TagsUI
 * Description  - NotificationTagViewDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.120       04-Mar-2021   Girish Kundar          Created - Is Radian change
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Tags
{

    /// <summary> 
    /// NotificationTagViewDTO
    /// </summary>
    public class NotificationTagViewDTO : ICloneable
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int notificationTagId;
        private bool isInStorage;
        private string tagNumber;
        private DateTime? lastCommunicatedOn;
        private string deviceStatus;
        private bool pingStatus;
        private string batteryStatus;
        private string signalStrength;
        private DateTime? expiryDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public enum SearchByParameters
        {
            TAG_NUMBER,
            DEVICE_STATUS,
            CHANNEL,
            PING_STATUS,
            IS_IN_STORAGE,
            MARKED_FOR_STORAGE,
            SIGNAL_STRENGTH,
            TAG_ID,
            TAG_NOTIFICATION_STATUS,
            SITE_ID,
            EXPIRING_TODAY,
            EXPIRING_IN_X_MINUTES,
            EXPIRED,
            ISSUED_TODAY,
            BATTERY_PERCENTAGE,
            ALL,
            IS_RETURNED
        }
        public NotificationTagViewDTO()
        {
            log.LogMethodEntry();
            isInStorage = false;
            pingStatus = false;
            log.LogMethodExit();
        }

        public NotificationTagViewDTO(int notificationTagId ,bool isInStorage, string tagNumber, DateTime? lastCommunicatedOn, string deviceStatus,
                                        bool pingStatus, string batteryStatus, string signalStrength, DateTime? expiryDate)
            : this()
        {
            log.LogMethodEntry(isInStorage, tagNumber, lastCommunicatedOn, deviceStatus, pingStatus, batteryStatus, signalStrength, expiryDate);
            this.isInStorage = isInStorage;
            this.tagNumber = tagNumber;
            this.lastCommunicatedOn = lastCommunicatedOn;
            this.deviceStatus = deviceStatus;
            this.pingStatus = pingStatus;
            this.notificationTagId = notificationTagId;
            this.batteryStatus = batteryStatus;
            this.signalStrength = signalStrength;
            this.expiryDate = expiryDate;
            log.LogMethodExit();
        }

        public int NotificationTagId { get { return notificationTagId; } set { notificationTagId = value;   } }
        [DisplayName("Is In Storage")]
        public bool IsInStorage { get { return isInStorage; } set { this.IsChanged = (isInStorage != value); isInStorage = value; } }
        [DisplayName("Tag Number")]
        public string TagNumber { get { return tagNumber; } set { tagNumber = value;  } }
        [DisplayName("Last Communicated On")]
        public DateTime? LastCommunicatedOn { get { return lastCommunicatedOn; } set { lastCommunicatedOn = value;   } }
        [DisplayName("Device Status")]
        public string DeviceStatus { get { return deviceStatus; } set { deviceStatus = value;   } }
        [DisplayName("Ping Status")]
        public bool PingStatus { get { return pingStatus; } set { pingStatus = value;   } }
        [DisplayName("Battery Status")]
        public string BatteryStatus { get { return batteryStatus; } set { batteryStatus = value;   } }
        [DisplayName("Signal Strength")]
        public string SignalStrength { get { return signalStrength; } set { signalStrength = value;  } }
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value;   } }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || notificationTagId < 0;
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
}
