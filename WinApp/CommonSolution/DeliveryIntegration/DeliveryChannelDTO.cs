/********************************************************************************************
 * Project Name - DeliveryIntegration                                                                        
 * Description  - DeliveryChannelDTO holds the delivery partners details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
  *2.140.0     21-Jun-2021   Fiona Lishal      Modified for Delivery Order enhancements for F&B
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DeliveryIntegration
{
    public class DeliveryChannelDTO 
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int deliveryChannelId;
        private string channelName;
        private string channelAPIUrl;
        private string channelAPIKey;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool autoAcceptOrders;
        private bool manualRiderAssignmentAllowed;
        private bool reconfirmOrder;
        private bool reConfirmPreparation;
        private int defaultRiderId;
        private string externalSourceReference;
        private int deliveryIntegrationId;
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By deliveryChannelId
            /// </summary>
            DELIVERY_CHANNEL_ID,
            /// <summary>
            /// Search By channelName 
            /// </summary>
            CHANNEL_NAME,
            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search By DELIVERY_INTEGRATION_ID
            /// </summary>
            DELIVERY_INTEGRATION_ID,
            /// <summary>
            /// Search By DELIVERY_INTEGRATION_NAME
            /// </summary>
            DELIVERY_INTEGRATION_NAME
        }
        /// <summary>
        /// default constructor
        /// </summary>
        public DeliveryChannelDTO()
        {
            log.LogMethodEntry();
            deliveryChannelId = -1;
            channelName = string.Empty;
            channelAPIKey = string.Empty;
            channelAPIUrl = string.Empty;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            defaultRiderId = -1;
            deliveryIntegrationId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public DeliveryChannelDTO(int deliveryChannelId, string channelName, string channelAPIUrl,
                                               string channelAPIKey, bool autoAcceptOrders, bool manualRiderAssignmentAllowed,int defaultRiderId, string externalSourceReference, bool reconfirmOrder, bool reConfirmPreparation, bool isActive, int deliveryIntegrationId)
    : this()
        {
            log.LogMethodEntry(deliveryChannelId, channelName, channelAPIUrl, channelAPIKey, reconfirmOrder, reConfirmPreparation, isActive, deliveryIntegrationId);
            this.deliveryChannelId = deliveryChannelId;
            this.channelName = channelName;
            this.channelAPIKey = channelAPIKey;
            this.channelAPIUrl = channelAPIUrl;
            this.autoAcceptOrders = autoAcceptOrders;
            this.manualRiderAssignmentAllowed = manualRiderAssignmentAllowed;
            this.defaultRiderId = defaultRiderId;
            this.externalSourceReference = externalSourceReference;
            this.reconfirmOrder = reconfirmOrder;
            this.reConfirmPreparation = reConfirmPreparation;
            this.isActive = isActive;
            this.deliveryIntegrationId = deliveryIntegrationId;
            log.LogMethodExit();
        }        
        /// <summary>
        /// Constructor with All data fields.
        /// </summary> 
        public DeliveryChannelDTO(int deliveryChannelId, string channelName, string channelAPIUrl,
                                               string channelAPIKey, bool autoAcceptOrders, bool manualRiderAssignmentAllowed, int defaultRiderId, string externalSourceReference, bool reconfirmOrder, bool reConfirmPreparation, bool isActive,  string createdBy, DateTime creationDate, string lastUpdatedBy,
                                      DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId, int deliveryIntegrationId)
    : this(deliveryChannelId, channelName, channelAPIUrl, channelAPIKey, autoAcceptOrders, manualRiderAssignmentAllowed, defaultRiderId, externalSourceReference, 
          reconfirmOrder, reConfirmPreparation, isActive, deliveryIntegrationId)
        {
            log.LogMethodEntry(deliveryChannelId, channelName, channelAPIUrl, channelAPIKey, defaultRiderId, externalSourceReference, reconfirmOrder,
                reConfirmPreparation, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId, deliveryIntegrationId);
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
        /// Constructor with Required  data fields
        /// </summary>
        public DeliveryChannelDTO(DeliveryChannelDTO deliveryChannelDTO)
            : this(deliveryChannelDTO.DeliveryChannelId,
                 deliveryChannelDTO.ChannelName,
                 deliveryChannelDTO.ChannelAPIKey,
                 deliveryChannelDTO.ChannelAPIUrl,
                 deliveryChannelDTO.AutoAcceptOrders,
                 deliveryChannelDTO.ManualRiderAssignmentAllowed,
                 deliveryChannelDTO.DefaultRiderId,
                 deliveryChannelDTO.ExternalSourceReference,
                 deliveryChannelDTO.ReConfirmOrder,
                 deliveryChannelDTO.ReConfirmPreparation,
                 deliveryChannelDTO.IsActive,
                 deliveryChannelDTO.CreatedBy,
                 deliveryChannelDTO.CreationDate,
                 deliveryChannelDTO.LastUpdatedBy,
                 deliveryChannelDTO.LastUpdateDate,
                 deliveryChannelDTO.Guid,
                 deliveryChannelDTO.siteId,
                 deliveryChannelDTO.SynchStatus,
                 deliveryChannelDTO.MasterEntityId,
                 deliveryChannelDTO.DeliveryIntegrationId)
        {
            log.LogMethodEntry(deliveryChannelDTO);
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the deliveryChannelId field
        /// </summary>
        public int DeliveryChannelId { get { return deliveryChannelId; } set { this.IsChanged = true; deliveryChannelId = value; } }
        /// <summary>
        /// Get/Set method of the channelName field
        /// </summary>
        public string ChannelName { get { return channelName; } set { this.IsChanged = true; channelName = value; } }
        /// <summary>
        /// Get/Set method of the channelAPIUrl field
        /// </summary>
        public string ChannelAPIUrl { get { return channelAPIUrl; } set { this.IsChanged = true; channelAPIUrl = value; } }
        /// <summary>
        /// Get/Set method of the channelAPIKey field
        /// </summary>
        public string ChannelAPIKey { get { return channelAPIKey; } set { this.IsChanged = true; channelAPIKey = value; } }
        /// <summary>
        /// Get/Set method of the AutoAcceptOrders field
        /// </summary>
        public bool AutoAcceptOrders{ get { return autoAcceptOrders; } set { this.IsChanged = true; autoAcceptOrders = value; } }
        /// <summary>
        /// Get/Set method of the ManualRiderAssignmentAllowed field
        /// </summary>
        public bool ManualRiderAssignmentAllowed { get { return manualRiderAssignmentAllowed; } set { this.IsChanged = true; manualRiderAssignmentAllowed = value; } }
        /// <summary>
        ///  Get/Set method of the DefaultRiderId field
        /// </summary>
        public int DefaultRiderId {  get { return defaultRiderId; } set { this.IsChanged = true; defaultRiderId = value; } }
        /// <summary>
        /// ExternalSourceReference
        /// </summary>
        public string ExternalSourceReference
        {
            get { return externalSourceReference; }
            set { externalSourceReference = value; this.IsChanged = true; }
        }
        /// <summary>
        ///  Get/Set method of the reconfirmOrder field
        /// </summary>
        public bool ReConfirmOrder
        {
            get
            {
                return reconfirmOrder;
            }
            set
            {
                this.IsChanged = true;
                reconfirmOrder = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ReConfirmPreparation field
        /// </summary>
        public bool ReConfirmPreparation
        {
            get
            {
                return reConfirmPreparation;
            }
            set
            {
                this.IsChanged = true;
                reConfirmPreparation = value;
            }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }
        /// <summary>
        /// Get/Set method of the DeliveryIntegrationId field
        /// </summary>
        public int DeliveryIntegrationId { get { return deliveryIntegrationId; } set { this.IsChanged = true; deliveryIntegrationId = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || deliveryChannelId < 0;
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
