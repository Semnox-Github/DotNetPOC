/********************************************************************************************
* Project Name - LoyaltyAttributesDTO
* Description  - Business logic of the   LoyaltyAttributes DTO class
* 
**************
**Version Log
**************
*Version     Date          Modified By      Remarks          
*********************************************************************************************
*1.00        4-may-2017    Rakshith         Created 
*2.70       18-Jun-2019   Girish Kundar    Modified: Added Who columns and parameterized Constructor 
********************************************************************************************/
using Semnox.Parafait.Promotions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Achievements
{
    public class LoyaltyAttributesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            LOYALTY_ATTRIBUTE_ID,
            /// <summary>
            /// Search by Purchase applicable field
            /// </summary>
            PURCHASE_APPLICABLE,
            /// <summary>
            /// Search by Consumption applicable field
            /// </summary>
            CONSUMPTION_APPLICABLE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE
        }


        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private int loyaltyAttributeId;
        private string attribute;
        private string purchaseApplicable;
        private string consumptionApplicable;
        private string dBColumnName;
        private string creditPlusType;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private DateTime creationDate;
        private string createdBy;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoyaltyAttributesDTO()
        {
            log.LogMethodEntry();
            loyaltyAttributeId = -1;
            attribute = "";
            purchaseApplicable = "";
            dBColumnName = "";
            dBColumnName = "";
            creditPlusType = "";
            guid = "";
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public LoyaltyAttributesDTO(int loyaltyAttributeId, string attribute, string purchaseApplicable,
                                   string consumptionApplicable, string dBColumnName, string creditPlusType)
               : this()
        {
            log.LogMethodEntry(loyaltyAttributeId, attribute, purchaseApplicable, consumptionApplicable, dBColumnName, creditPlusType);
            this.loyaltyAttributeId = loyaltyAttributeId;
            this.attribute = attribute;
            this.purchaseApplicable = purchaseApplicable;
            this.consumptionApplicable = consumptionApplicable;
            this.dBColumnName = dBColumnName;
            this.creditPlusType = creditPlusType;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the fields.
        /// </summary>
        public LoyaltyAttributesDTO(int loyaltyAttributeId, string attribute, string purchaseApplicable,
                                    string consumptionApplicable, string dBColumnName, string creditPlusType,
                                    DateTime lastUpdatedDate, string lastUpdatedBy, string guid, bool synchStatus,
                                    int masterEntityId, int siteId, DateTime creationDate, string createdBy)
               : this(loyaltyAttributeId, attribute, purchaseApplicable, consumptionApplicable, dBColumnName, creditPlusType)
        {
            log.LogMethodEntry(loyaltyAttributeId, attribute, purchaseApplicable, consumptionApplicable, dBColumnName,
                               creditPlusType, lastUpdatedDate, lastUpdatedBy, guid, synchStatus, masterEntityId, siteId,
                               creationDate, createdBy);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("LoyaltyAttributeId")]
        [ReadOnly(true)]
        public int LoyaltyAttributeId { get { return loyaltyAttributeId; } set { loyaltyAttributeId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Attribute field
        /// </summary>
        [DisplayName("Attribute")]
        public string Attribute { get { return attribute; } set { attribute = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the PurchaseApplicable field
        /// </summary>
        [DisplayName("PurchaseApplicable")]
        public string PurchaseApplicable { get { return purchaseApplicable; } set { purchaseApplicable = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ConsumptionApplicable field
        /// </summary>
        [DisplayName("ConsumptionApplicable")]
        public string ConsumptionApplicable { get { return consumptionApplicable; } set { consumptionApplicable = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DBColumnName field
        /// </summary>
        [DisplayName("DBColumnName")]
        public string DBColumnName { get { return dBColumnName; } set { dBColumnName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreditPlusType field
        /// </summary>
        [DisplayName("CreditPlusType")]
        public string CreditPlusType { get { return creditPlusType; } set { creditPlusType = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedUser")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
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
                    return notifyingObjectIsChanged;
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
