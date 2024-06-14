/********************************************************************************************
 * Project Name - Partners DTO Programs 
 * Description  - Data object of the PartnersDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        10-May-2016   Rakshith           Created 
 *2.70.2        15-Jul-2019   Girish Kundar      Modified : Added Parametrized Constructor with required fields
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the PartnersDTO data object class. This acts as data holder for the PartnersDTO business object
    /// </summary>
    public class PartnersDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected int partnerId;
        protected string partnerName;
        protected string remarks;
        protected int customerId;
        protected string createdBy;
        protected DateTime creationDate;
        protected string lastUpdatedUser;
        protected DateTime lastUpdatedDate;
        protected bool active;
        protected int siteId;
        protected string guid;
        protected bool synchStatus;
        protected int masterEntityId;
        protected List<PartnerRevenueShareDTO> partnerRevenueShareDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public PartnersDTO()
        {
            log.LogMethodEntry();
            this.partnerId = -1;
            this.partnerName = string.Empty;
            this.remarks = string.Empty;
            this.siteId = -1;
            this.masterEntityId = -1;
            this.customerId = -1;
            this.active = true;
            partnerRevenueShareDTOList = new List<PartnerRevenueShareDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        ///Constructor With  required Parameter
        /// </summary>
        public PartnersDTO(int partnerId, string partnerName, string remarks, int customerId, bool active)
            : this()
        {
            log.LogMethodEntry(partnerId, partnerName, remarks, customerId, active);
            this.partnerId = partnerId;
            this.partnerName = partnerName;
            this.remarks = remarks;
            this.customerId = customerId;
            this.active = active;
            log.LogMethodExit();

        }


        /// <summary>
        ///Constructor With All the  Parameter
        /// </summary>
        public PartnersDTO(int partnerId, string partnerName, string remarks, int customerId, string createdBy,
                              DateTime creationDate, string lastUpdatedUser, DateTime lastUpdatedDate, bool active,
                              int site_id, string guid, bool synchStatus, int masterEntityId)
            : this(partnerId, partnerName, remarks, customerId, active)
        {
            log.LogMethodEntry(partnerId, partnerName, remarks, customerId, createdBy,
                               creationDate, lastUpdatedUser, lastUpdatedDate, active,
                               site_id, guid, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();

        }

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by CUSTOMER ID field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by PARTNER_ID field
            /// </summary>
            PARTNER_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID
        }


        /// <summary>
        /// Get/Set method of the PartnerId field
        /// </summary>
        [DisplayName("Partner Id")]
        public int PartnerId { get { return partnerId; } set { partnerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PartnerName field
        /// </summary>
        [DisplayName("Partner Name")]
        public string PartnerName { get { return partnerName; } set { partnerName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Customer_Id field
        /// </summary>
        [DisplayName("Customer Id")]
        public int Customer_Id { get { return customerId; } set { customerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }


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
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedUser")]
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }


        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site id ")]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid ")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the PartnerRevenueShareList field
        /// </summary>
        [DisplayName("PartnerRevenueShareDTO")]
        public List<PartnerRevenueShareDTO> PartnerRevenueShareList { get { return partnerRevenueShareDTOList; } set { partnerRevenueShareDTOList = value; } }

        /// <summary>
        /// Returns whether the PartnerDTO changed or any of its PartnerRevenueShareDTOList children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (partnerRevenueShareDTOList != null &&
                   partnerRevenueShareDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
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
                    return notifyingObjectIsChanged || partnerId < 0;
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
