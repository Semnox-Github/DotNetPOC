/********************************************************************************************
 * Project Name - Site                                                                       
 * Description  - SiteDetailDTO holds the delivery channel like start time ,end time etc
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Site
{
   public  class SiteDetailDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int siteDetailId;
        private int parentSiteId;
        private int deliveryChannelId;
        private decimal? onlineChannelStartHour;
        private decimal? onlineChannelEndHour;
        private int orderDeliveryType;
        private string zipCodes;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        public enum SearchByParameters
        {
            /// <summary>
            /// Search By siteDetailId
            /// </summary>
            SITE_DETAIL_ID,
            // <summary>
            /// Search By PARENT_SITE_ID
            /// </summary>
            PARENT_SITE_ID,
            /// <summary>
            /// Search By deliveryChannelId 
            /// </summary>
            DELIVERY_CHANNEL_ID,
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
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE
        }
        /// <summary>
        /// default constructor
        /// </summary>
        public SiteDetailDTO()
        {
            log.LogMethodEntry();
            parentSiteId = -1;
            siteDetailId = -1;
            deliveryChannelId =-1;
            onlineChannelStartHour = null;
            onlineChannelEndHour = null;
            zipCodes = string.Empty;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public SiteDetailDTO(int siteDetailId, int parentSiteId, int deliveryChannelId,
                                               decimal? onlineChannelStartHour,decimal? onlineChannelEndHour,
                                               int orderDeliveryType,string zipCodes,bool isActive)
    : this()
        {
            log.LogMethodEntry(siteDetailId, parentSiteId, deliveryChannelId, onlineChannelStartHour, onlineChannelEndHour, orderDeliveryType, zipCodes, isActive);
            this.siteDetailId = siteDetailId;
            this.parentSiteId = parentSiteId;
            this.deliveryChannelId = deliveryChannelId;
            this.onlineChannelStartHour = onlineChannelStartHour;
            this.onlineChannelEndHour = onlineChannelEndHour;
            this.orderDeliveryType = orderDeliveryType;
            this.zipCodes = zipCodes;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public SiteDetailDTO(SiteDetailDTO siteDetailDTO)
        {
            log.LogMethodEntry(siteDetailDTO);
            this.deliveryChannelId = siteDetailDTO.DeliveryChannelId;
            this.siteDetailId = siteDetailDTO.SiteDetailId;
            this.parentSiteId = siteDetailDTO.ParentSiteId;
            this.deliveryChannelId = siteDetailDTO.DeliveryChannelId;
            this.onlineChannelEndHour = siteDetailDTO.OnlineChannelEndHour;
            this.onlineChannelStartHour = siteDetailDTO.OnlineChannelStartHour;
            this.orderDeliveryType = siteDetailDTO.OrderDeliveryType;
            this.zipCodes = siteDetailDTO.ZipCodes;
            this.isActive = siteDetailDTO.IsActive;
            this.createdBy = siteDetailDTO.CreatedBy;
            this.creationDate = siteDetailDTO.CreationDate;
            this.lastUpdatedBy = siteDetailDTO.LastUpdatedBy;
            this.lastUpdateDate = siteDetailDTO.LastUpdateDate;
            this.siteId = siteDetailDTO.SiteId;
            this.masterEntityId = siteDetailDTO.MasterEntityId;
            this.synchStatus = siteDetailDTO.SynchStatus;
            this.guid = siteDetailDTO.Guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with All data fields.
        /// </summary>
        public SiteDetailDTO(int siteDetailId, int parentSiteId, int deliveryChannelId,
                                               decimal? onlineChannelStartHour, decimal? onlineChannelEndHour,
                                               int orderDeliveryType, string zipCodes, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                      DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId)
    : this(siteDetailId, parentSiteId, deliveryChannelId, onlineChannelStartHour, onlineChannelEndHour, orderDeliveryType, zipCodes, isActive)
        {
            log.LogMethodEntry(siteDetailId, parentSiteId, deliveryChannelId, onlineChannelStartHour, onlineChannelEndHour, orderDeliveryType, zipCodes, isActive, createdBy,
                                creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
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
        /// Get/Set method of the deliveryChannelId field
        /// </summary>
        public int DeliveryChannelId { get { return deliveryChannelId; } set { this.IsChanged = true; deliveryChannelId = value; } }
        /// <summary>
        /// Get/Set method of the siteDetailId field
        /// </summary>
        public int SiteDetailId { get { return siteDetailId; } set { this.IsChanged = true; siteDetailId = value; } }
        /// <summary>
        /// Get/Set method of the parentSiteId field
        /// </summary>
        public int ParentSiteId { get { return parentSiteId; } set { this.IsChanged = true; parentSiteId = value; } }

        /// <summary>
        /// Get/Set method of the onlineChannelStartHour field
        /// </summary>
        public decimal? OnlineChannelStartHour { get { return onlineChannelStartHour; } set { this.IsChanged = true; onlineChannelStartHour = value; } }
        /// <summary>
        /// Get/Set method of the onlineChannelStartHour field
        /// </summary>
        public decimal? OnlineChannelEndHour { get { return onlineChannelEndHour; } set { this.IsChanged = true; onlineChannelEndHour = value; } }
        /// <summary>
        /// Get/Set method of the orderDeliveryType field
        /// </summary>
        public int OrderDeliveryType { get { return orderDeliveryType; } set { this.IsChanged = true; orderDeliveryType = value; } }
        /// <summary>
        /// Get/Set method of the zipCodes field
        /// </summary>
        public string ZipCodes { get { return zipCodes; } set { this.IsChanged = true; zipCodes = value; } }

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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || siteDetailId < 0;
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
