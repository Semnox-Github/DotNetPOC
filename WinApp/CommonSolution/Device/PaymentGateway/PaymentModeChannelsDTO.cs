/********************************************************************************************
 * Project Name - PaymentModeChannels Programs
 * Description  - Data object of PaymentChannels DTO    
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        07-Feb-2017   Rakshith            Created 
 *2.70.2        10-Jul-2019   Girish kundar       Modified :Added constructor for required fields .
 *            26-Jul-2019   Mushahid Faizan     Added IsActive 
 *2.140.0      07-Sep-2021   Fiona                Added Copy Constructor
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  PaymentChannelsDTO Class
    /// </summary>
    public class PaymentModeChannelsDTO
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int paymentModeChannelId;
        private int paymentModeId;
        private int lookupValueId;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by PAYMENT_MODE_ID
            /// </summary>
            PAYMENT_MODE_CHANNEL_ID,
            /// <summary>
            /// Search by PAYMENT_MODE_ID
            /// </summary>
            PAYMENT_MODE_ID,
            /// <summary>
            /// Search by LOOKUP_VALUE_ID
            /// </summary>
            LOOKUP_VALUE_ID,
            /// <summary>
            /// Search by SITE_ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IsActive Field
            /// </summary>
            ISACTIVE

        }

        /// <summary>
        /// Default constructor
        /// </summary> 
        public PaymentModeChannelsDTO()
        {
            log.LogMethodEntry();
            this.paymentModeChannelId = -1;
            this.paymentModeId = -1;
            this.lookupValueId = -1;
            this.createdBy = string.Empty;
            this.creationDate = DateTime.MinValue;
            this.lastUpdatedDate = DateTime.MinValue;
            this.lastUpdatedBy = string.Empty;
            this.site_id = -1;
            this.guid = string.Empty;
            this.synchStatus = false;
            this.isActive = true;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        ///  constructor with Required Parameter
        /// </summary>
        public PaymentModeChannelsDTO(int paymentChannelId, int paymentModeId, int lookupValueId)
            :this()
        {
            log.LogMethodEntry(paymentChannelId, paymentModeId, lookupValueId);
            this.paymentModeChannelId = paymentChannelId;
            this.paymentModeId = paymentModeId;
            this.lookupValueId = lookupValueId;
            log.LogMethodExit();
        }


        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public PaymentModeChannelsDTO(int paymentChannelId, int paymentModeId, int lookupValueId, string createdBy, DateTime creationDate, DateTime lastUpdatedDate,
                                      string lastUpdatedBy, int site_id, string guid, bool synchStatus, int masterEntityId, bool isActive)
            :this(paymentChannelId, paymentModeId, lookupValueId)
        {
            log.LogMethodEntry(paymentChannelId,  paymentModeId,lookupValueId,createdBy,creationDate, lastUpdatedDate,
                               lastUpdatedBy, site_id,guid,synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        public PaymentModeChannelsDTO(PaymentModeChannelsDTO paymentModeChannelsDTO):
            this(paymentModeChannelsDTO.paymentModeChannelId,
                paymentModeChannelsDTO.paymentModeId,
                paymentModeChannelsDTO.lookupValueId,
                paymentModeChannelsDTO.createdBy,
                paymentModeChannelsDTO.creationDate,
                paymentModeChannelsDTO.lastUpdatedDate,
                paymentModeChannelsDTO.lastUpdatedBy,
                paymentModeChannelsDTO.site_id,                
                paymentModeChannelsDTO.guid,
                paymentModeChannelsDTO.synchStatus,
                paymentModeChannelsDTO.masterEntityId,
                paymentModeChannelsDTO.isActive)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the PaymentChannelId field
        /// </summary>
        [DisplayName("PaymentChannelId")]
        [DefaultValue(-1)]
        public int PaymentModeChannelId { get { return paymentModeChannelId; } set { paymentModeChannelId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PaymentModeId field
        /// </summary>
        [DisplayName("PaymentModeId")]
        [DefaultValue(-1)]
        public int PaymentModeId { get { return paymentModeId; } set { paymentModeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LookupValueId field
        /// </summary>
        [DisplayName("LookupValueId")]
        [DefaultValue(-1)]
        public int LookupValueId { get { return lookupValueId; } set { lookupValueId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [DefaultValue("")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }


        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; }   }


        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; }  }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [DefaultValue("")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; }   }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return site_id; } set { site_id = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [DefaultValue("")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [DefaultValue(-1)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || paymentModeChannelId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
