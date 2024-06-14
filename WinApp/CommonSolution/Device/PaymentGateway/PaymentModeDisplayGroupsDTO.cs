/********************************************************************************************
 * Project Name - PaymentModeDisplayGroupsDTO 
 * Description  - Data object of PaymentModeDisplayGroups    
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 ********************************************************************************************* 
 *2.150.1      26-Jan-2023   Guru S             For Kiosk Cart Project
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class PaymentModeDisplayGroupsDTO
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int paymentModeDisplayGroupId;
        private int paymentModeId;
        private int productDisplayGroupId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by PAYMENT_MODE_DISPLAY_GROUP_ID
            /// </summary>
            PAYMENT_MODE_DISPLAY_GROUP_ID,
            /// <summary>
            /// Search by PAYMENT_MODE_ID
            /// </summary>
            PAYMENT_MODE_ID,
            /// <summary>
            /// Search by PRODUCT_DISPLAY_GROUP_ID
            /// </summary>
            PRODUCT_DISPLAY_GROUP_ID,
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
            IS_ACTIVE,
            /// <summary>
            /// Search by NOT_EXCLUDED_FOR_POS_MACHINE_ID Field
            /// </summary>
            NOT_EXCLUDED_FOR_POS_MACHINE_ID,
            /// <summary>
            /// Search by NOT_EXCLUDED_FOR_USER_ROLE_ID Field
            /// </summary>
            NOT_EXCLUDED_FOR_USER_ROLE_ID
        }
        /// <summary>
        /// Default constructor
        /// </summary> 
        public PaymentModeDisplayGroupsDTO()
        {
            log.LogMethodEntry();
            this.paymentModeDisplayGroupId = -1;
            this.paymentModeId = -1;
            this.productDisplayGroupId = -1;
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
        public PaymentModeDisplayGroupsDTO(int paymentModeDisplayGroupId, int paymentModeId, int productDisplayGroupId)
            : this()
        {
            log.LogMethodEntry(paymentModeDisplayGroupId, paymentModeId, productDisplayGroupId);
            this.paymentModeDisplayGroupId = paymentModeDisplayGroupId;
            this.paymentModeId = paymentModeId;
            this.productDisplayGroupId = productDisplayGroupId;
            log.LogMethodExit();
        }


        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public PaymentModeDisplayGroupsDTO(int paymentModeDisplayGroupId, int paymentModeId, int productDisplayGroupId, string createdBy, DateTime creationDate, 
            DateTime lastUpdatedDate, string lastUpdatedBy, int site_id, string guid, bool synchStatus, int masterEntityId, bool isActive)
            : this(paymentModeDisplayGroupId, paymentModeId, productDisplayGroupId)
        {
            log.LogMethodEntry(paymentModeDisplayGroupId, paymentModeId, productDisplayGroupId, createdBy, creationDate, lastUpdatedDate,
                               lastUpdatedBy, site_id, guid, synchStatus, masterEntityId);
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
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="paymentModeChannelsDTO"></param>
        public PaymentModeDisplayGroupsDTO(PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO) :
            this(paymentModeDisplayGroupsDTO.paymentModeDisplayGroupId,
                paymentModeDisplayGroupsDTO.paymentModeId,
                paymentModeDisplayGroupsDTO.productDisplayGroupId,
                paymentModeDisplayGroupsDTO.createdBy,
                paymentModeDisplayGroupsDTO.creationDate,
                paymentModeDisplayGroupsDTO.lastUpdatedDate,
                paymentModeDisplayGroupsDTO.lastUpdatedBy,
                paymentModeDisplayGroupsDTO.site_id,
                paymentModeDisplayGroupsDTO.guid,
                paymentModeDisplayGroupsDTO.synchStatus,
                paymentModeDisplayGroupsDTO.masterEntityId,
                paymentModeDisplayGroupsDTO.isActive)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the paymentModeDisplayGroupId field
        /// </summary>
        [DisplayName("PaymentModeDisplayGroupId")] 
        public int PaymentModeDisplayGroupId { get { return paymentModeDisplayGroupId; } set { paymentModeDisplayGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PaymentModeId field
        /// </summary>
        [DisplayName("PaymentModeId")]
        public int PaymentModeId { get { return paymentModeId; } set { paymentModeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ProductDisplayGroupId field
        /// </summary>
        [DisplayName("ProductDisplayGroupId")]
        public int ProductDisplayGroupId { get { return productDisplayGroupId; } set { productDisplayGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")] 
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return site_id; } set { site_id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")] 
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }  
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } } 
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } } 
        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true;  } }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || PaymentModeDisplayGroupId < 0;
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
