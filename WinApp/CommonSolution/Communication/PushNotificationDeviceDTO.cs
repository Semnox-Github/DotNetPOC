/********************************************************************************************
 * Project Name - Communication
 * Description  - DTO of PushNotificationDevice Entity
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0   15-Sep-2020   Nitin Pai               Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    public class PushNotificationDeviceDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by  ProfileId field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by  ProfileIdList field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by  AddressTypeId field
            /// </summary>
            PUSH_NOTIFICATION_TOKEN,
            /// <summary>
            /// Search by  AddressType field
            /// </summary>
            DEVICE_TYPE,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by CUSTOMER_SIGNED_IN field
            /// </summary>
            CUSTOMER_SIGNED_IN,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID,
        }

        public enum DEVICETYPE
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ANDROID,
            /// <summary>
            /// Search by  ProfileId field
            /// </summary>
            IOS,
        }

        private int id;
        private int customerId;
        private string pushNotificationToken;
        private string deviceType;
        private bool isActive;
        private bool customerSignedIn;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        public PushNotificationDeviceDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.customerId = -1;
            this.pushNotificationToken = String.Empty;
            this.deviceType = String.Empty;
            this.isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public PushNotificationDeviceDTO(int id, int customerId, string pushNotificationToken, string deviceType, bool isActive, bool customerSignedId)
            : this()
        {
            log.LogMethodEntry(id, customerId, pushNotificationToken, deviceType, isActive);
            this.id = id;
            this.customerId = customerId;
            this.pushNotificationToken = pushNotificationToken;
            this.deviceType = deviceType;
            this.isActive = isActive;
            this.customerSignedIn = customerSignedId;
            log.LogMethodExit();
        }

        public PushNotificationDeviceDTO(int id, int customerId, string pushNotificationToken, string deviceType, bool isActive, bool customerSignedId,
               string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid)
            : this(id, customerId, pushNotificationToken, deviceType, isActive, customerSignedId)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id?")]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                this.IsChanged = true;
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("CustomerId")]
        public int CustomerId
        {
            get
            {
                return customerId;
            }

            set
            {
                this.IsChanged = true;
                customerId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PushNotificationToken field
        /// </summary>
        [DisplayName("PushNotificationToken")]
        public string PushNotificationToken
        {
            get
            {
                return pushNotificationToken;
            }

            set
            {
                this.IsChanged = true;
                pushNotificationToken = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DeviceType field
        /// </summary>
        [DisplayName("DeviceType")]
        public string DeviceType
        {
            get
            {
                return deviceType;
            }

            set
            {
                this.IsChanged = true;
                deviceType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("CustomerSignedIn?")]
        public bool CustomerSignedIn
        {
            get
            {
                return customerSignedIn;
            }

            set
            {
                this.IsChanged = true;
                customerSignedIn = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {

                createdBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {

                creationDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {

                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {

                lastUpdateDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }

            set
            {

                siteId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }

            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }

            set
            {
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }

            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
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
